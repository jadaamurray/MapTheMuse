using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapTheMuseApi.Models
{
    public enum MediaType { Movie = 1, Tv = 2, Book = 3, Song = 4, Album = 5, Artwork = 6 }

    public class Media
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Source { get; set; } = "TMDB"; // tmdb, openlibrary, spotify, wikiart

        [Required, MaxLength(50)]
        public string ExternalId { get; set; } = default!;

        [Required]
        public MediaType Type { get; set; }

        // Optional cache fields (all nullable)
        [MaxLength(400)] public string? Title { get; set; }
        [MaxLength(600)] public string? Description { get; set; }
        [MaxLength(100)] public string? Creator { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        [MaxLength(300)] public string? PosterPath { get; set; }
        public DateTime? LastSyncedUtc { get; set; }

        // Navigation properties
        public ICollection<DestinationMediaLink> DestinationLinks { get; set; } = [];
        public ICollection<FavouriteMedia> FavouritedBy { get; set; } = [];

    }
}
