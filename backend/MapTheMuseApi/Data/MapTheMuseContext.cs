using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Models;

namespace MapTheMuseApi.Data
{
    public class MapTheMuseContext : DbContext
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
