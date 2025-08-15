using MapTheMuseApi.Dtos;

public interface IDestinationService
{
    Task<List<DestinationListDto>>   GetAllDestinationsAsync(
        string? continent = null,
        string? factKey = null,
        string? factValue = null);
    Task<DestinationDetailDto?>      GetDestinationByIdAsync(int id);
    Task<DestinationDetailDto>       CreateDestinationAsync(DestinationCreateUpdateDto dto);
    Task<bool>                       UpdateDestinationAsync(int id, DestinationCreateUpdateDto dto);
    Task<bool>                       DeleteDestinationAsync(int id);
}