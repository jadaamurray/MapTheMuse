using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using Microsoft.EntityFrameworkCore;

public class MediaRepository : IMediaRepository
{
    private readonly MapTheMuseContext _db;
    public MediaRepository(MapTheMuseContext db) => _db = db;

    public IQueryable<Media> Query() => _db.Media.AsNoTracking();

    public Task<Media?> GetByIdAsync(int id) => _db.Media.FindAsync(id).AsTask();

    public async Task AddAsync(Media entity)
    {
        _db.Media.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Media entity)
    {
        _db.Media.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _db.Media.FindAsync(id);
        if (e is null) return;
        _db.Media.Remove(e);
        await _db.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(int id) => _db.Media.AnyAsync(m => m.Id == id);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
