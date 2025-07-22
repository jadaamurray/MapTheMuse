using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MapTheMuseApi.Data
{
    public class MapTheMuseContext : IdentityDbContext<AppUser>
    {
        public MapTheMuseContext(DbContextOptions<MapTheMuseContext> options)
            : base(options)
        {
        }

        public DbSet<Destination> Destinations { get; set; } = default!;
        public DbSet<Media> Media { get; set; } = default!;
        public DbSet<PhysicalArt> PhysicalArtworks { get; set; } = default!;
        public DbSet<MapTheMuseApi.Models.AppUser> AppUser { get; set; } = default!;
        public DbSet<MapTheMuseApi.Models.UserArtEngagement> UserArtEngagements { get; set; } = default!;
        public DbSet<MapTheMuseApi.Models.UserMediaEngagement> UserMediaEngagements { get; set; } = default!;
        public DbSet<MapTheMuseApi.Models.Itinerary> Itineraries { get; set; } = default!;
        public DbSet<MapTheMuseApi.Models.ItineraryItem> ItineraryItems { get; set; } = default!;



    }
}
