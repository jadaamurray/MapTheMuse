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

    public async Task<List<DestinationListDto>> GetAllDestinationsAsync()
    {
        return await _context.Destinations
            .AsNoTracking()
            .Select(d => new DestinationListDto
            {
                Id = d.Id,
                Name = d.Name,
                Summary = string.IsNullOrWhiteSpace(d.Summary)
                    ? ((d.Description ?? "").Length <= 100 ? d.Description : (d.Description ?? "").Substring(0, 97) + "…")
                    : d.Summary,
                Slug = d.Slug,
                ThumbUrl = d.ThumbUrl
            })
            .ToListAsync();
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
            Continent = d.Continent,
            Country = d.Country,
            Region = d.Region,
            CultureHighlights = d.CultureHighlights,
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
            Continent = dto.Continent,
            Country = dto.Country,
            Region = dto.Region,
            CultureHighlights = dto.CultureHighlights,
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
            Continent = entity.Continent,
            Country = entity.Country,
            Region = entity.Region,
            CultureHighlights = entity.CultureHighlights,
            PhysicalArtworks = Enumerable.Empty<PhysicalArtListDto>()
        };
    }

    public async Task<bool> UpdateDestinationAsync(int id, DestinationCreateUpdateDto dto)
    {
        var entity = await _context.Destinations.FindAsync(id);
        if (entity == null) return false;

        entity.Name = dto.Name;
        entity.Summary = dto.Summary;
        entity.Description = dto.Description;
        entity.ImageUrl = dto.ImageUrl;
        entity.ThumbUrl = dto.ThumbUrl;
        entity.Continent = dto.Continent;
        entity.Country = dto.Country;
        entity.Region = dto.Region;
        entity.CultureHighlights = dto.CultureHighlights;

        var baseSlug = Slugify.From(entity.Name);

        // Only change the slug if the base would differ from current (ignoring an "-id" suffix)
        var currentBase = entity.Slug?.Split('-', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
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
