namespace MapTheMuseApi.Dtos
{
    public class PhysicalArtDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Artist { get; set; }
        public string ArtType { get; set; }
        public DateOnly? DateCreated { get; set; }
        public string LocationName { get; set; }
        public int? DestinationId { get; set; }
    }
}