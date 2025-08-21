using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using MapTheMuseApi.Dtos;

public class FavouritesService : IFavouritesService
{
    private readonly MapTheMuseContext _db;
    private readonly IMediaSpineService _mediaSpine;
    public FavouritesService(MapTheMuseContext db, IMediaSpineService mediaSpine)
    { _db = db; _mediaSpine = mediaSpine; }

    // ---- Destinations ----
    public async Task FavouriteDestinationAsync(string userId, int destinationId, CancellationToken ct = default)
    {
        var exists = await _db.FavouriteDestinations.FindAsync([userId, destinationId], ct);
        if (exists != null) return;

        _db.FavouriteDestinations.Add(new FavouriteDestination { UserId = userId, DestinationId = destinationId });
        try { await _db.SaveChangesAsync(ct); } catch (DbUpdateException) { /* composite PK prevents dupes */ }
    }

    public async Task UnfavouriteDestinationAsync(string userId, int destinationId, CancellationToken ct = default)
    {
        var fav = await _db.FavouriteDestinations.FindAsync([userId, destinationId], ct);
        if (fav == null) return;
        _db.FavouriteDestinations.Remove(fav);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<DestinationListDto>> GetMyDestinationsAsync(
        string userId, int page = 1, int pageSize = 50, CancellationToken ct = default)
    {
        var skip = Math.Max(0, (page - 1) * pageSize);
        return await _db.FavouriteDestinations
            .AsNoTracking()
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedUtc)
            .Skip(skip).Take(pageSize)
            .Select(f => new DestinationListDto
            {
                Id = f.Destination.Id,
                Name = f.Destination.Name,
                Summary = f.Destination.Summary})
            .ToListAsync(ct);
    }

    // ---- Media ----
    public async Task FavouriteMediaAsync(string userId, FavouriteMediaRequestDto req, CancellationToken ct = default)
    {
        var media = await _mediaSpine.EnsureAsync(req.MediaType, req.ExternalId, req.Source, req.Title, req.PosterPath);
        var exists = await _db.FavouriteMedia.FindAsync([userId, media.Id], ct);
        if (exists != null) return;

        _db.FavouriteMedia.Add(new FavouriteMedia { UserId = userId, MediaId = media.Id });
        try { await _db.SaveChangesAsync(ct); } catch (DbUpdateException) { /* composite PK prevents dupes */ }
    }

    public async Task UnfavouriteMediaByMediaIdAsync(string userId, int mediaId, CancellationToken ct = default)
    {
        var fav = await _db.FavouriteMedia.FindAsync([userId, mediaId], ct);
        if (fav == null) return;
        _db.FavouriteMedia.Remove(fav);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UnfavouriteMediaByExternalAsync(string userId, string source, MediaType type, string externalId, CancellationToken ct = default)
    {
        var media = await _db.Media.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Source == source && m.Type == type && m.ExternalId == externalId, ct);
        if (media == null) return;
        await UnfavouriteMediaByMediaIdAsync(userId, media.Id, ct);
    }

    public async Task<IReadOnlyList<MediaFavouriteDto>> GetMyMediaAsync(
        string userId, MediaType? type = null, int page = 1, int pageSize = 50, CancellationToken ct = default)
    {
        var skip = Math.Max(0, (page - 1) * pageSize);
        var query = _db.FavouriteMedia
            .AsNoTracking()
            .Where(f => f.UserId == userId);

        if (type.HasValue)
            query = query.Where(f => f.Media.Type == type.Value);

        return await query
            .OrderByDescending(f => f.CreatedUtc)
            .Skip(skip).Take(pageSize)
            .Select(f => new MediaFavouriteDto(
                f.MediaId,
                f.Media.Source,
                f.Media.ExternalId,
                f.Media.Type,
                f.Media.Title,
                f.Media.PosterPath,
                f.CreatedUtc
            ))
            .ToListAsync(ct);
    }
}
