using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using MapTheMuseApi.Dtos;

public class FavouritesService : IFavouritesService
{
    private readonly MapTheMuseContext _db;
    private readonly IMediaSpineService _mediaSpine;
    private readonly TmdbClient _tmdb;
    public FavouritesService(MapTheMuseContext db, IMediaSpineService mediaSpine, TmdbClient tmdb)
    { _db = db; _mediaSpine = mediaSpine; _tmdb = tmdb;}

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
                Summary = f.Destination.Summary,
                Slug = f.Destination.Slug,
                ThumbUrl = f.Destination.ThumbUrl})
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
            .Include(f => f.Media)
            .Where(f => f.UserId == userId);

        if (type.HasValue)
            query = query.Where(f => f.Media.Type == type.Value);

        var favs = await query
            .OrderByDescending(f => f.CreatedUtc)
            .Skip(skip).Take(pageSize)
            .ToListAsync(ct);
            
        var tasks = favs.Select(f => BuildFavouriteDtoAsync(f, ct)).ToArray();
        await Task.WhenAll(tasks);
        return tasks.Select(t => t.Result).ToList();
    }
    private async Task<MediaFavouriteDto> BuildFavouriteDtoAsync(FavouriteMedia fav, CancellationToken ct)
    {
        var m = fav.Media ?? throw new InvalidOperationException("Favourite has no Media.");

        string? title = m.Title;
        string? posterPath = m.PosterPath;  // we store full url here
        string? overview = m.Description;

        var isStale = m.LastSyncedUtc == null || (DateTime.UtcNow - m.LastSyncedUtc.Value).TotalDays > 7;
        var needsEnrich = string.Equals(m.Source, "TMDB", StringComparison.OrdinalIgnoreCase) &&
                          (isStale || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(posterPath));

        if (needsEnrich)
        {
            try
            {
                if (m.Type == MediaType.Movie)
                {
                    var (t, _, _, p, o) = await _tmdb.GetMovieAsync(m.ExternalId, null, ct);
                    title = t ?? title;
                    posterPath = p ?? posterPath;
                    overview = o ?? overview;
                }
                else if (m.Type == MediaType.Tv)
                {
                    var (t, _, _, p, o) = await _tmdb.GetTvAsync(m.ExternalId, null, ct);
                    title = t ?? title;
                    posterPath = p ?? posterPath;
                    overview = o ?? overview;
                }

                // (optional) update cache if we improved anything
                if ((title != m.Title) || (posterPath != m.PosterPath) || (overview != m.Description))
                {
                    // track and save
                    var tracked = await _db.Media.FirstOrDefaultAsync(x => x.Id == m.Id, ct);
                    if (tracked != null)
                    {
                        tracked.Title = title ?? tracked.Title;
                        tracked.PosterPath = posterPath ?? tracked.PosterPath;
                        tracked.Description = overview ?? tracked.Description;
                        tracked.LastSyncedUtc = DateTime.UtcNow;
                        await _db.SaveChangesAsync(ct);
                    }
                }
            }
            catch
            {
                // swallow TMDB/network errors and fall back to cached values
            }
        }

        return new MediaFavouriteDto(
            MediaId: m.Id,
            Source: m.Source,
            ExternalId: m.ExternalId,
            Type: m.Type,
            Title: title,
            PosterPath: posterPath,
            Overview: overview,
            CreatedUtc: fav.CreatedUtc
        );
    }
}
