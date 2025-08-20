namespace MapTheMuseApi.Models {

    public class FavouriteDestination
    {
        public string UserId { get; set; } = default!;
        public AppUser User { get; set; } = default!;
        public int DestinationId { get; set; }
        public Destination Destination { get; set; } = default!;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }

    public class FavouriteMedia
    {
        public string UserId { get; set; } = default!;
        public AppUser User { get; set; } = default!;
        public int MediaId { get; set; }
        public Media Media { get; set; } = default!;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }

}