namespace MapTheMuseApi.Dtos
{
    public class ItineraryItemCreateDto
    {
        public int ItineraryId { get; set; }
        public int DestinationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Order { get; set; }
        public string? Note { get; set; }
    }
}
