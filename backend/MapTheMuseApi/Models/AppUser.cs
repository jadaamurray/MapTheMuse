using Microsoft.AspNetCore.Identity;
using MapTheMuseApi.Models;

namespace MapTheMuseApi.Models
{
    public class AppUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public required string Country { get; set; }
        public required string PreferredLanguage { get; set; }

        // Navigation properties
        public ICollection<UserArtEngagement> ArtEngagements { get; set; } = new List<UserArtEngagement>();
        public ICollection<UserMediaEngagement> MediaEngagements { get; set; } = new List<UserMediaEngagement>();
    }
}
