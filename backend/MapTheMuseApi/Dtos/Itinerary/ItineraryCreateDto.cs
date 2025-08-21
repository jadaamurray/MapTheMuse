namespace MapTheMuseApi.Dtos
{
    public class ItineraryCreateDto
    {
         public required string UserId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
