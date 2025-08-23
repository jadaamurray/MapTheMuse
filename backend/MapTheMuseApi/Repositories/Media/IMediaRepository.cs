using MapTheMuseApi.Models;
using System.Linq;

public interface IMediaRepository
{
    IQueryable<Media> Query();
    Task<Media?> GetByIdAsync(int id);
    Task AddAsync(Media entity);
    Task UpdateAsync(Media entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task SaveChangesAsync();
}
