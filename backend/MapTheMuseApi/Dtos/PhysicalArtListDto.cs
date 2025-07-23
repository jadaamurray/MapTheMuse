namespace MapTheMuseApi.Dtos
{
    public class PhysicalArtListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Artist { get; set; }
        public string ArtType { get; set; }
        public DateOnly? DateCreated { get; set; }
        public string LocationName { get; set; }
        public int? DestinationId { get; set; }
    }
}