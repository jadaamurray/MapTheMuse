namespace MapTheMuseApi.Dtos
{
    public class DestinationDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public IEnumerable<PhysicalArtListDto> PhysicalArtworks { get; set; } = new List<PhysicalArtListDto>();
  }
}
