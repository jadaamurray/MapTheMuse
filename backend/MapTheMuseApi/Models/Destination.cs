using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapTheMuseApi.Models
{
    public class Destination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // navigation properties
        public ICollection<PhysicalArt> PhysicalArtworks { get; set; } = new List<PhysicalArt>();
        public ICollection<UserArtEngagement> ArtEngagements { get; set; } = new List<UserArtEngagement>();
        public ICollection<UserMediaEngagement> MediaEngagements { get; set; } = new List<UserMediaEngagement>();

    }
}