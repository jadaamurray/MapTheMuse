namespace MapTheMuseApi.Dtos
{
    // A minimal reference to a Destination
    public class DestinationSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class ItineraryItemReadDto
    {
        public int Id { get; set; }
        public DestinationSummaryDto Destination { get; set; } = null!;
        public PhysicalArtSummaryDto? PhysicalArt { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Order { get; set; }
        public string? Note { get; set; }
    }
}
