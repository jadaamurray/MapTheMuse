using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using Microsoft.EntityFrameworkCore;

public class DestinationRepository : IDestinationRepository
{
    private readonly MapTheMuseContext _db;

    public DestinationRepository(MapTheMuseContext db)
    {
        _db = db;
    }

    public IQueryable<Destination> Query() => _db.Destinations.AsNoTracking();

    public async Task<IEnumerable<Destination>> GetAllAsync()
    {
        return await _db.Destinations.AsNoTracking().ToListAsync();
    }

    public async Task<Destination?> GetByIdAsync(int id)
    {
        return await _db.Destinations.FindAsync(id);
    }

    public async Task<Destination?> GetBySlugAsync(string slug)
    {
        return await _db.Destinations.FirstOrDefaultAsync(d => d.Slug == slug);
    }

    public async Task AddAsync(Destination destination)
    {
        _db.Destinations.Add(destination);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Destination destination)
    {
        _db.Destinations.Update(destination);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dest = await _db.Destinations.FindAsync(id);
        if (dest != null)
        {
            _db.Destinations.Remove(dest);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Destinations.AnyAsync(e => e.Id == id);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
