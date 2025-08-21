namespace MapTheMuseApi.Dtos
{
    public class PhysicalArtSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }
    public class UserArtEngagementReadDto
    {
        public int Id { get; set; }
        public DateTime DateVisited { get; set; }
        public DestinationSummaryDto Destination { get; set; } = null!;
        public PhysicalArtSummaryDto PhysicalArt { get; set; } = null!;
    }
}
