using MapTheMuseApi.Models;

namespace MapTheMuseApi.Dtos
{
    public class MediaListDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string ExternalId { get; set; }
        public string Source { get; set; }
        public string? ShortDescription { get; set; }
        public string? Creator { get; set; }
        public MediaType Type { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }

    public class MediaDetailDto : MediaListDto
    {
        public string? Description { get; set; }
        public string? PosterPath { get; set; }
        public DateTime? LastSyncedUtc { get; set; }
    }
}