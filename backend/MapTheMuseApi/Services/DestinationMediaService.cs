using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Dtos;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;

public interface IDestinationMediaService
{
    Task<List<DestinationMediaItemDto>> GetForDestinationAsync(int destinationId);
    Task<int> LinkAsync(int destinationId, CreateDestinationMediaLinkDto dto, string? createdById);
    Task UnlinkAsync(int linkId);
}

public class DestinationMediaService : IDestinationMediaService
{
    private readonly MapTheMuseContext _db;
    private readonly TmdbClient _tmdb;

    public DestinationMediaService(MapTheMuseContext db, TmdbClient tmdb)
    {
        _db = db; _tmdb = tmdb;
    }

    public async Task<List<DestinationMediaItemDto>> GetForDestinationAsync(int destinationId)
    {
        var links = await _db.DestinationMediaLinks
            .AsNoTracking()
            .Where(l => l.DestinationId == destinationId)
            .OrderBy(l => l.OrderIndex).ThenBy(l => l.Id)
            .ToListAsync();

        var items = new List<DestinationMediaItemDto>(links.Count);

        foreach (var l in links)
        {
            string? title = null, overview = null, creator = null, poster = null;
            int? year = null;

            if (l.Source == "tmdb")
            {
                if (string.Equals(l.MediaType, "Movie", StringComparison.OrdinalIgnoreCase))
                {
                    (title, year, creator, poster, overview) = await _tmdb.GetMovieAsync(l.ExternalId);
                }
                else
                {
                    (title, year, creator, poster, overview) = await _tmdb.GetTvAsync(l.ExternalId);
                }
            }
            // else: other providers later

            items.Add(new DestinationMediaItemDto(
                LinkId: l.Id,
                MediaType: l.MediaType,
                Title: title ?? l.ExternalId,
                Year: year,
                Creator: creator,
                PosterUrl: poster,
                Overview: overview,
                Source: l.Source,
                ExternalId: l.ExternalId,
                ContextNote: l.ContextNote
            ));
        }

        return items;
    }

    public async Task<int> LinkAsync(int destinationId, CreateDestinationMediaLinkDto dto, string? createdById)
    {
        // prevent duplicates per destination/source/externalId
        var exists = await _db.DestinationMediaLinks.AnyAsync(x =>
            x.DestinationId == destinationId &&
            x.Source == dto.Source &&
            x.ExternalId == dto.ExternalId);

        if (exists) throw new InvalidOperationException("Media already linked to this destination.");

        var link = new DestinationMediaLink
        {
            DestinationId = destinationId,
            Source = dto.Source,
            ExternalId = dto.ExternalId,
            MediaType = dto.MediaType,
            ContextNote = dto.ContextNote,
            OrderIndex = dto.OrderIndex,
            CreatedById = createdById
        };

        _db.DestinationMediaLinks.Add(link);
        await _db.SaveChangesAsync();
        return link.Id;
    }

    public async Task UnlinkAsync(int linkId)
    {
        var link = await _db.DestinationMediaLinks.FindAsync(linkId);
        if (link == null) return;
        _db.DestinationMediaLinks.Remove(link);
        await _db.SaveChangesAsync();
    }
}
