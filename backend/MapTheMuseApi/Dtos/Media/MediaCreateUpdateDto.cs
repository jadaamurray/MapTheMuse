using System.ComponentModel.DataAnnotations;
using MapTheMuseApi.Models;

public class MediaCreateUpdateDto
{
    [Required] public string Source { get; set; } = "TMDB";
    [Required] public string ExternalId { get; set; } = default!;

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Creator { get; set; }
    public string? PosterPath { get; set; }
    public MediaType Type { get; set; }
    public DateOnly? ReleaseDate { get; set; }
}
