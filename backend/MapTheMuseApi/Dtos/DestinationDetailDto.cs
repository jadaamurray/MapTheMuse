namespace MapTheMuseApi.Dtos
{
  public class DestinationDetailDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = "";
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Continent { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public List<string> CultureHighlights { get; set; } = new();
    public IEnumerable<PhysicalArtListDto> PhysicalArtworks { get; set; } = new List<PhysicalArtListDto>();
  }
}
