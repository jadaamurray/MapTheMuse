namespace MapTheMuseApi.Dtos
{
    public class ItineraryDetailDto
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Embed the list of item read-DTOs
        public List<ItineraryItemReadDto> Items { get; set; }
            = new List<ItineraryItemReadDto>();
    }
}
