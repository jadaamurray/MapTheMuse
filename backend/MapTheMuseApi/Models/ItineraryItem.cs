using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MapTheMuseApi.Models
{
    public class ItineraryItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Itinerary")]
        public int ItineraryId { get; set; }

        [ForeignKey("Destination")]
        public int DestinationId { get; set; }
        [ForeignKey("PhysicalArt")]
        public int? PhysicalArtId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Order { get; set; }
        public string? Note { get; set; }

        [JsonIgnore]
        public Itinerary Itinerary { get; set; }
        public Destination Destination { get; set; }
        public PhysicalArt? PhysicalArt { get; set; }
    }
}
