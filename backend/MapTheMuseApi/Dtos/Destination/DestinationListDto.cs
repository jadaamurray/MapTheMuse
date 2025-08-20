namespace MapTheMuseApi.Dtos
{
    public class DestinationListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Summary { get; set; }
        public string Slug { get; set; } = "";
        public string? ThumbUrl { get; set; }
    }
}