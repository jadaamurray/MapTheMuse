using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MapTheMuseApi.Dtos;
using MapTheMuseApi.Models;

public interface IFavouritesService
{
    // Destinations
    Task FavouriteDestinationAsync(string userId, int destinationId, CancellationToken ct = default);
    Task UnfavouriteDestinationAsync(string userId, int destinationId, CancellationToken ct = default);
    Task<IReadOnlyList<DestinationListDto>> GetMyDestinationsAsync(
        string userId, int page = 1, int pageSize = 50, CancellationToken ct = default);

    // Media
    Task FavouriteMediaAsync(string userId, FavouriteMediaRequestDto req, CancellationToken ct = default);
    Task UnfavouriteMediaByMediaIdAsync(string userId, int mediaId, CancellationToken ct = default);
    Task UnfavouriteMediaByExternalAsync(string userId, string source, MediaType type, string externalId, CancellationToken ct = default);
    Task<IReadOnlyList<MediaFavouriteDto>> GetMyMediaAsync(
        string userId, MediaType? type = null, int page = 1, int pageSize = 50, CancellationToken ct = default);
}
