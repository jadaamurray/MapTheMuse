using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MapTheMuseApi.Models
{
    public enum DestinationType { Country = 0, City = 1, Region = 2 }
    public class Destination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }
        [MaxLength(200)]
        public string Slug { get; set; } = "";
        public DestinationType Type { get; set; } = DestinationType.Country;

        // Content
        [MaxLength(300)]
        public string? Summary { get; set; }
        [MaxLength(300)]
        public string? Description { get; set; }

        // Media
        public string? ImageUrl { get; set; }              // hero/banner
        public string? ThumbUrl { get; set; }              // card/thumbnail
        public string? SpotifyPlaylistId { get; set; }
 
        // Geography
        [MaxLength(100)] public string? Continent { get; set; }
        [MaxLength(100)] public string? Country { get; set; }
        [MaxLength(100)] public string? Region { get; set; }

        // culture highlights 
        [Column(TypeName = "jsonb")]
        public Dictionary<string, string>? QuickFacts { get; set; }
        public List<string>? CultureHighlights { get; set; } = new();

        [JsonIgnore]
        // navigation properties
        ICollection<DestinationMediaLink> MediaLinks { get; set; } = new List<DestinationMediaLink>();
        public ICollection<PhysicalArt> PhysicalArtworks { get; set; } = new List<PhysicalArt>();
        public ICollection<UserArtEngagement> ArtEngagements { get; set; } = new List<UserArtEngagement>();
        public ICollection<UserMediaEngagement> MediaEngagements { get; set; } = new List<UserMediaEngagement>();
        public ICollection<ItineraryItem> ItineraryItems { get; set; } = new List<ItineraryItem>();


    }
} 