namespace MapTheMuseApi.Dtos
{
    public class MediaListDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? ShortDescription { get; set; }
        public required string Creator { get; set; }
        public required string MediaType { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }
}