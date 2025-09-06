using MapTheMuseApi.Models;

public interface IDestinationRepository
{
    Task<IEnumerable<Destination>> GetAllAsync();
    IQueryable<Destination> Query();
    Task<Destination?> GetByIdAsync(int id);
    Task<Destination?> GetBySlugAsync(string slug);
    Task AddAsync(Destination destination);
    Task UpdateAsync(Destination destination);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task SaveChangesAsync();
}
