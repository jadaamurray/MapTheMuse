using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapTheMuseApi.Models
{
    public class Media
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
        public required string Creator { get; set; } // Author, Director, Artist, etc

        [Required]
        [MaxLength(50)]
        public required string MediaType { get; set; } // Book, Film, Music, etc

        public DateOnly? ReleaseDate { get; set; }

        // Navigation property

    }
}
