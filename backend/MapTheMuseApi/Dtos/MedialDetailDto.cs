namespace MapTheMuseApi.Dtos
{
    public class MediaDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Creator { get; set; }
        public string MediaType { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }
}