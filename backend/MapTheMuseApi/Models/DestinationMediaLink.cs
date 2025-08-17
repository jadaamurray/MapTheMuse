using System.ComponentModel.DataAnnotations;
namespace MapTheMuseApi.Models
{
    public class DestinationMediaLink
    {
        public int Id { get; set; }

        public int DestinationId { get; set; }
        public Destination Destination { get; set; } = default!;

        [MaxLength(32)] public required string Source { get; set; }     // "tmdb" | "openlibrary" | "spotify" | "wikiart"
        public required string ExternalId { get; set; } // e.g. TMDb numeric id as string
        [MaxLength(24)] public required string MediaType { get; set; }  // "Movie" | "TV" | "Book" | "Album" | "Artwork"

        [MaxLength(200)] public string? ContextNote { get; set; }        // why it’s relevant
        public int? OrderIndex { get; set; }

        // audit
        public string? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}