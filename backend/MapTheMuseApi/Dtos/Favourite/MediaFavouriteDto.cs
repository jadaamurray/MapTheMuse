using MapTheMuseApi.Models;

public record FavouriteMediaRequestDto(
    MediaType MediaType,
    string ExternalId,
    string Source = "TMDB",
    string? Title = null,
    string? PosterPath = null
);

public record MediaFavouriteDto(
    int MediaId,
    string Source,
    string ExternalId,
    MediaType Type,
    string? Title,
    string? PosterPath,
    string? Overview,
    DateTime CreatedUtc
);