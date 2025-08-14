using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MapTheMuseApi.Models
{
    public class Destination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        [MaxLength(300)]
        public required string Summary { get; set; }

        [MaxLength(200)]
        public string Slug { get; set; } = "";

        [MaxLength(300)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }              // hero/banner
        public string? ThumbUrl { get; set; }              // card/thumbnail

        // basic geography for filters
        [MaxLength(100)] public string? Continent { get; set; }
        [MaxLength(100)] public string? Country { get; set; }
        [MaxLength(100)] public string? Region { get; set; }

        // culture highlights 
        public List<string> CultureHighlights { get; set; } = new();

        [JsonIgnore]
        // navigation properties
        public ICollection<PhysicalArt> PhysicalArtworks { get; set; } = new List<PhysicalArt>();
        public ICollection<UserArtEngagement> ArtEngagements { get; set; } = new List<UserArtEngagement>();
        public ICollection<UserMediaEngagement> MediaEngagements { get; set; } = new List<UserMediaEngagement>();
        public ICollection<ItineraryItem> ItineraryItems { get; set; } = new List<ItineraryItem>();


    }
}