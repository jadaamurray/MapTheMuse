using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Data;
using MapTheMuseApi.Dtos;
using MapTheMuseApi.Models;

public class DestinationService : IDestinationService
{
    private readonly MapTheMuseContext _context;

    public DestinationService(MapTheMuseContext context)
        => _context = context;

    public async Task<List<DestinationListDto>> GetAllDestinationsAsync()
    {
        return await _context.Destinations
            .AsNoTracking()
            .Select(d => new DestinationListDto {
                Id               = d.Id,
                Name             = d.Name,
                ShortDescription = d.Description.Length <= 100
                    ? d.Description
                    : d.Description.Substring(0,97) + "..."
            })
            .ToListAsync();
    }

    public async Task<DestinationDetailDto?> GetDestinationByIdAsync(int id)
    {
        var d = await _context.Destinations
            .AsNoTracking()
            .Include(d => d.PhysicalArtworks)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (d == null) return null;

        return new DestinationDetailDto {
            Id                 = d.Id,
            Name               = d.Name,
            Description        = d.Description,
            PhysicalArtworks   = d.PhysicalArtworks
                .Select(pa => new PhysicalArtListDto {
                    Id      = pa.Id,
                    Title   = pa.Title,
                    Artist  = pa.Artist
                })
        };
    }

    public async Task<DestinationDetailDto> CreateDestinationAsync(DestinationCreateUpdateDto dto)
    {
        var entity = new Destination {
            Name        = dto.Name,
            Description = dto.Description
        };
        _context.Destinations.Add(entity);
        await _context.SaveChangesAsync();

        // map back to a detail DTO
        return new DestinationDetailDto {
            Id          = entity.Id,
            Name        = entity.Name,
            Description = entity.Description,
            PhysicalArtworks = Enumerable.Empty<PhysicalArtListDto>()
        };
    }

    public async Task<bool> UpdateDestinationAsync(int id, DestinationCreateUpdateDto dto)
    {
        var entity = await _context.Destinations.FindAsync(id);
        if (entity == null) return false;

        entity.Name        = dto.Name;
        entity.Description = dto.Description;
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
}
