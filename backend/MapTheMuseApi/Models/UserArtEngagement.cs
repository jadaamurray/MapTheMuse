using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace MapTheMuseApi.Models
{
    public class UserArtEngagement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // foreign keys
        public int UserId { get; set; }
        public int DestinationId { get; set; }
        public int PhysicalArtId { get; set; }
        // Metadata
        public DateTime DateVisited { get; set; } = DateTime.UtcNow;
        //public string? Comment { get; set; }
        //public int? Rating { get; set; }

        // navigation properties
        [JsonIgnore]
        public IdentityUser User { get; set; }
        public Destination Destination { get; set; }
        public PhysicalArt PhysicalArt { get; set; }
    }
}