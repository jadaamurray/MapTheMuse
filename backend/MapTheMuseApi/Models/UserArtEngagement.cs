using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using MapTheMuseApi.Models; 

namespace MapTheMuseApi.Models
{
    public class UserArtEngagement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // foreign keys
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Destination")]
        public int DestinationId { get; set; }
        [ForeignKey("PhysicalArt")]
        public int PhysicalArtId { get; set; }
        // metadata
        public DateTime DateVisited { get; set; } = DateTime.UtcNow;
        //public string? Comment { get; set; }
        //public int? Rating { get; set; }

        // navigation properties
        [JsonIgnore]
        public AppUser User { get; set; }
        public Destination Destination { get; set; }
        public PhysicalArt PhysicalArt { get; set; }
    }
}