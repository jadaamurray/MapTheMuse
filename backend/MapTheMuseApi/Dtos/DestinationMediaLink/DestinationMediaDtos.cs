using MapTheMuseApi.Models;

namespace MapTheMuseApi.Dtos
{
    public record DestinationMediaItemDto(
        int LinkId,
        string Title,
        int? Year,
        string? Creator,
        string? PosterPath,
        string? Overview,
        string Source,
        string ExternalId,
        string? ContextNote,
        MediaType Type
    );

    public record CreateDestinationMediaLinkDto(
        string Source,
        string ExternalId,
        MediaType Type,
        string? PosterPath,
        string Title,
        string? ContextNote,
        int? OrderIndex
    );
}
