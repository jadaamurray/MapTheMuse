using System.ComponentModel.DataAnnotations;
namespace MapTheMuseApi.Models
{
    public class DestinationMediaLink
    {
        public int Id { get; set; }

        public int DestinationId { get; set; }
        public Destination Destination { get; set; } = default!;

        public int? MediaId { get; set; }
        public Media Media { get; set; } = default!;

        [MaxLength(200)] public string? ContextNote { get; set; }        // why it’s relevant
        public int? OrderIndex { get; set; }

        // audit
        public string? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}