namespace MapTheMuseApi.Dtos
{
    public class ItineraryListDto
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}