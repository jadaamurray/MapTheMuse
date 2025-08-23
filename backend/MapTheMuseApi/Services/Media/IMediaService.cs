using MapTheMuseApi.Dtos;

public interface IMediaService
{
    Task<List<MediaListDto>> GetAllAsync(int? skip = null, int? take = null);
    Task<MediaDetailDto?> GetByIdAsync(int id);
    Task<MediaDetailDto> CreateAsync(MediaCreateUpdateDto dto);
    Task<bool> UpdateAsync(int id, MediaCreateUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
