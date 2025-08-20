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
        public DbSet<UserArtEngagement> UserArtEngagements { get; set; } = default!;
        public DbSet<UserMediaEngagement> UserMediaEngagements { get; set; } = default!;
        public DbSet<Itinerary> Itineraries { get; set; } = default!;
        public DbSet<ItineraryItem> ItineraryItems { get; set; } = default!;
        public DbSet<DestinationMediaLink> DestinationMediaLinks { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // Destination
            b.Entity<Destination>()
                .HasIndex(d => d.Slug)
                .IsUnique();

            // Map List<string> to Postgres text[]
            b.Entity<Destination>()
                .Property(d => d.CultureHighlights)
                .HasColumnType("text[]");
            
            b.Entity<AppUser>()
                .HasIndex(u => u.NormalizedEmail)
                .IsUnique();
        }
    }
}
