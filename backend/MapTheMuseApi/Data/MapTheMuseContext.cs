using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MapTheMuseApi.Data
{
    public class MapTheMuseContext : IdentityDbContext<IdentityUser>
    {
        public MapTheMuseContext(DbContextOptions<MapTheMuseContext> options)
            : base(options)
        {
        }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<PhysicalArt> PhysicalArtworks { get; set; }
        public DbSet<MapTheMuseApi.Models.AppUser> AppUser { get; set; } = default!;
        public DbSet<MapTheMuseApi.Models.UserArtEngagement> UserArtEngagements { get; set; } = default!;
        public DbSet<MapTheMuseApi.Models.UserMediaEngagement> UserMediaEngagements { get; set; } = default!;

    }
}
