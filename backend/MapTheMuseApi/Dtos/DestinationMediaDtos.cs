namespace MapTheMuseApi.Dtos
{
    public record DestinationMediaItemDto(
        int LinkId,
        string MediaType,
        string Title,
        int? Year,
        string? Creator,
        string? PosterUrl,
        string? Overview,
        string Source,
        string ExternalId,
        string? ContextNote
    );

    public record CreateDestinationMediaLinkDto(
        string Source,
        string ExternalId,
        string MediaType,
        string? ContextNote,
        int? OrderIndex
    );
}