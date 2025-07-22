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

        /// <summary>
        /// Optional start date/time for this stop in the itinerary
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Optional end date/time for this stop in the itinerary
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Ordering index
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Any notes or comments about this itinerary item
        /// </summary>
        public string? Note { get; set; }

        [JsonIgnore]
        public Itinerary Itinerary { get; set; }
        public Destination Destination { get; set; }
        public PhysicalArt? PhysicalArt { get; set; }
    }
}
