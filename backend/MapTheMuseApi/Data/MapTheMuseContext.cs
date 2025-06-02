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

    }
}
