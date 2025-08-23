using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Data;
using MapTheMuseApi.Dtos;
using MapTheMuseApi.Models;
using MapTheMuseApi.Infrastructure.Text;
using Npgsql;
public class DestinationService : IDestinationService
{
    private readonly MapTheMuseContext _context;

    public DestinationService(MapTheMuseContext context)
        => _context = context;

    public async Task<List<DestinationListDto>> GetAllDestinationsAsync(
        string? continent = null,
        string? factKey = null,
        string? factValue = null)
    {
        var q = _context.Destinations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(continent))
            q = q.Where(d => d.Continent == continent);

        if (!string.IsNullOrWhiteSpace(factKey) && string.IsNullOrWhiteSpace(factValue))
        {
            // has key
            q = q.Where(d => EF.Functions.JsonExists(d.QuickFacts, factKey));
        }

        if (!string.IsNullOrWhiteSpace(factKey) && !string.IsNullOrWhiteSpace(factValue))
        {
            var probe = new Dictionary<string, string> { { factKey, factValue } };
            q = q.Where(d => EF.Functions.JsonContains(d.QuickFacts, probe));
        }

        return await q.Select(d => new DestinationListDto
        {
            Id = d.Id,
            Name = d.Name,
            Summary = string.IsNullOrWhiteSpace(d.Summary)
                ? ((d.Description ?? "").Length <= 100 ? d.Description : (d.Description ?? "").Substring(0, 97) + "…")
                : d.Summary,
            Slug = d.Slug,
            ThumbUrl = d.ThumbUrl,
        }).ToListAsync();
    }

    public async Task<DestinationDetailDto?> GetDestinationByIdAsync(int id)
    {
        return await _context.Destinations
        .AsNoTracking()
        .Where(x => x.Id == id)
        .Select(d => new DestinationDetailDto
        {
            Id = d.Id,
            Name = d.Name,
            Slug = d.Slug,
            Summary = d.Summary,
            Description = d.Description,
            ImageUrl = d.ImageUrl,
            SpotifyPlaylistId = d.SpotifyPlaylistId,
            Continent = d.Continent,
            Country = d.Country,
            Region = d.Region,
            CultureHighlights = d.CultureHighlights,
            QuickFacts = d.QuickFacts,
            PhysicalArtworks = d.PhysicalArtworks
                .Select(pa => new PhysicalArtListDto { Id = pa.Id, Title = pa.Title, Artist = pa.Artist })
                .ToList()
        })
        .SingleOrDefaultAsync();
    }

    public async Task<DestinationDetailDto> CreateDestinationAsync(DestinationCreateUpdateDto dto)
    {
        var entity = new Destination
        {
            Name = dto.Name,
            Slug = Guid.NewGuid().ToString("n"), // temp unique slug
            Summary = dto.Summary,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            ThumbUrl = dto.ThumbUrl,
            SpotifyPlaylistId = dto.SpotifyPlaylistId,
            Continent = dto.Continent,
            Country = dto.Country,
            Region = dto.Region,
            CultureHighlights = dto.CultureHighlights,
            QuickFacts = dto.QuickFacts ?? new Dictionary<string, string>()

        };
        _context.Destinations.Add(entity);
        await _context.SaveChangesAsync();

        var baseSlug = Slugify.From(entity.Name);
        if (string.IsNullOrWhiteSpace(baseSlug))
            baseSlug = $"destination-{entity.Id}";

        // Try to claim base slug
        entity.Slug = baseSlug;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            // If taken, append the Id
            entity.Slug = $"{baseSlug}-{entity.Id}";
            await _context.SaveChangesAsync();
        }

        // map back to a detail DTO
        return new DestinationDetailDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Slug = entity.Slug,
            Summary = entity.Summary,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl,
            SpotifyPlaylistId = entity.SpotifyPlaylistId,
            Continent = entity.Continent,
            Country = entity.Country,
            Region = entity.Region,
            CultureHighlights = entity.CultureHighlights,
            QuickFacts = entity.QuickFacts,
            PhysicalArtworks = Enumerable.Empty<PhysicalArtListDto>()
        };
    }

    public async Task<bool> UpdateDestinationAsync(int id, DestinationCreateUpdateDto dto)
    {
        var entity = await _context.Destinations.FindAsync(id);
        if (entity == null) return false;

        entity.Name = dto.Name;
        entity.Summary = dto.Summary ?? entity.Summary;
        entity.Description = dto.Description ?? entity.Description;
        entity.ImageUrl = dto.ImageUrl ?? entity.ImageUrl;
        entity.ThumbUrl = dto.ThumbUrl ?? entity.ThumbUrl;
        entity.SpotifyPlaylistId = dto.SpotifyPlaylistId ?? entity.SpotifyPlaylistId;
        entity.Continent = dto.Continent ?? entity.Continent;
        entity.Country = dto.Country ?? entity.Country;
        entity.Region = dto.Region ?? entity.Region;
        entity.CultureHighlights = dto.CultureHighlights ?? entity.CultureHighlights ?? new List<string>();
        entity.QuickFacts = dto.QuickFacts ?? entity.QuickFacts ?? new();

        // slug recalculation only if needed
        var baseSlug = Slugify.From(entity.Name);
        if (string.IsNullOrWhiteSpace(baseSlug))
            baseSlug = $"destination-{entity.Id}";

        var currentBase = entity.Slug ?? "";
        var idSuffix = "-" + entity.Id;
        if (currentBase.EndsWith(idSuffix, StringComparison.Ordinal))
            currentBase = currentBase[..^idSuffix.Length];
        if (!string.Equals(baseSlug, currentBase, StringComparison.Ordinal))
        {
            entity.Slug = baseSlug;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                entity.Slug = $"{baseSlug}-{entity.Id}";
                await _context.SaveChangesAsync();
            }
            return true;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteDestinationAsync(int id)
    {
        var entity = await _context.Destinations.FindAsync(id);
        if (entity == null) return false;
        _context.Destinations.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
    private static bool IsUniqueViolation(DbUpdateException ex)
    {
        // Npgsql -> Postgres unique violation SQLSTATE is 23505
        return ex.InnerException is Npgsql.PostgresException pg && pg.SqlState == "23505";
    }
}
