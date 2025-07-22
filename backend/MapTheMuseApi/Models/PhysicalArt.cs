using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapTheMuseApi.Models
{
    public class PhysicalArt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(400)]
        public required string Title { get; set; }

        [MaxLength(600)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public required string Artist { get; set; }

        [MaxLength(100)]
        public required string ArtType { get; set; } // Sculpture, Mural, Architecture, etc

        public DateOnly? DateCreated { get; set; }

        [MaxLength(300)]
        public required string LocationName { get; set; } // Museum, Landmark

        // Foreign key to Destination
        public int? DestinationId { get; set; }
        public Destination? Destination { get; set; }

        // Navigation property
        public ICollection<UserArtEngagement> UserEngagements { get; set; } = new List<UserArtEngagement>();
        public ICollection<ItineraryItem> ItineraryItems { get; set; } = new List<ItineraryItem>();

    }
}
