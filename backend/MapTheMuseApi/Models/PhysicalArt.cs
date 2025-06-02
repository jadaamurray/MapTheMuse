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
        public string Title { get; set; }

        [MaxLength(600)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Artist { get; set; }

        [MaxLength(100)]
        public string ArtType { get; set; } // e.g. "Sculpture", "Mural", "Architecture", etc.

        public DateOnly? DateCreated { get; set; }

        [MaxLength(300)]
        public string LocationName { get; set; } // e.g. Museum, Landmark

        // Foreign key to Destination
        public int? DestinationId { get; set; }
        public Destination? Destination { get; set; }
    }
}
