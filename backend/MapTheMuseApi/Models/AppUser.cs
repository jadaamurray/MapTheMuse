using Microsoft.AspNetCore.Identity;
using MapTheMuseApi.Models;

namespace MapTheMuseApi.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string? Country { get; set; }
        public string? PreferredLanguage { get; set; }

        // Navigation properties
        public ICollection<UserArtEngagement> ArtEngagements { get; set; } = new List<UserArtEngagement>();
        public ICollection<UserMediaEngagement> MediaEngagements { get; set; } = new List<UserMediaEngagement>();
    }
}
