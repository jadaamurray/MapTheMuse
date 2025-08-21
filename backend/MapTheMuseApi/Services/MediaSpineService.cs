using MapTheMuseApi.Models;
using MapTheMuseApi.Data;
using System.Linq;  
using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore;
using Npgsql; 

public interface IMediaSpineService
{
    Task<Media> EnsureAsync(
        MediaType type, 
        string externalId, 
        string source = "TMDB",
        string? title = null, 
        string? posterPath = null);
}

public class MediaSpineService : IMediaSpineService
{
    private readonly MapTheMuseContext _db;
    public MediaSpineService(MapTheMuseContext db) => _db = db;

    public async Task<Media> EnsureAsync(MediaType type, string externalId, string source = "TMDB",
                                         string? title = null, string? posterPath = null)
    {
        var existing = await _db.Media
            .FirstOrDefaultAsync(m => m.Source == source && m.Type == type && m.ExternalId == externalId);
        if (existing != null) return existing;

        var media = new Media
        {
            Source = source, Type = type, ExternalId = externalId,
            Title = title, PosterPath = posterPath, LastSyncedUtc = DateTime.UtcNow
        };

        _db.Media.Add(media);
        try
    {
        await _db.SaveChangesAsync();
        return media;
    }
    catch (DbUpdateException ex) when (ex.InnerException is PostgresException pex && pex.SqlState == "23505")
    {
        // someone else inserted first — fetch and return
        return await _db.Media.FirstAsync(m => m.Source == source && m.Type == type && m.ExternalId == externalId);
    }
    }
}
