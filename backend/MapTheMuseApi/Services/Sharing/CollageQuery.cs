using MapTheMuseApi.Data;
using Microsoft.EntityFrameworkCore;

public class CollageQuery
{
    private readonly MapTheMuseContext _db;
    private readonly string _assetBase;

    public CollageQuery(MapTheMuseContext db, IConfiguration cfg)
    {
        _db = db;
        _assetBase = cfg["Assets:BaseUrl"] ?? "https://mapthemuse.world";
    }

    public async Task<(string title, List<CollageItem> items, string spaUrl)> ForUserAsync(string userId)
    {
        var favDestImgs = await _db.FavouriteDestinations
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedUtc)
            .Select(f => f.Destination.ThumbUrl ?? f.Destination.ImageUrl)
            .ToListAsync();

        var favMediaImgs = await _db.FavouriteMedia
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedUtc)
            .Select(f => f.Media.PosterPath)
            .ToListAsync();

        var urls = favDestImgs.Concat(favMediaImgs)
            .Select(u => UrlHelper.ToAbsolute(u, _assetBase))
            .Where(u => !string.IsNullOrWhiteSpace(u))
            .Distinct()
            .Take(16)
            .Select(u => new CollageItem(u!))
            .ToList();

        if (urls.Count == 0)
            urls.Add(new CollageItem(UrlHelper.ToAbsolute("/og/fallback.png", _assetBase)!));

        var title = "My Travel Style · Map The Muse";
        var spaUrl = $"https://mapthemuse.world/u/{userId}";
        return (title, urls, spaUrl);
    }
}
