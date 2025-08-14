namespace MapTheMuseApi.Dtos
{
    public class DestinationCreateUpdateDto
    {
        public required string Name { get; set; }
        public required string Summary { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbUrl { get; set; }
        public string? Continent { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public List<string> CultureHighlights { get; set; } = [];
    }
}