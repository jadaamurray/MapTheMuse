namespace MapTheMuseApi.Dtos
{
    public class MediaSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }
    public class UserMediaEngagementReadDto
    {
        public int Id { get; set; }
        public DateTime DateVisited { get; set; }

        public DestinationSummaryDto Destination { get; set; } = null!;
        public MediaSummaryDto Media { get; set; } = null!;
    }
}
