using System.Threading.Tasks;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MapTheMuseApi;

namespace MapTheMuseApi.Tests
{
    public class UserArtEngagementTests
    {
        [Fact]
        public async Task Can_Add_And_ReadBack_UserArtEngagement()
        {
            // configure in-memory DbContext
            var options = new DbContextOptionsBuilder<MapTheMuseContext>()
                .UseInMemoryDatabase(databaseName: "EngagementTestDb")
                .Options;

            await using var context = new MapTheMuseContext(options);

            // seed prerequisite data
            var dest = new Destination
            {
                Name = "Testville",
                Description = "Test Description"
            };
            var art = new PhysicalArt
            {
                Title = "Test Sculpture",
                Artist = "Test Artist",
                ArtType = "Test ArtType",
                LocationName = "Test Location",
                Destination = dest
            };
            var user = new AppUser
            {
                UserName = "test@unit.com",
                Email = "test@unit.com",
                FirstName = "Test",
                LastName = "User",
                Country = "United Kingdom",
                PreferredLanguage = "English"
            };

            context.Destinations.Add(dest);
            context.PhysicalArtworks.Add(art);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // create a link table record
            var engagement = new UserArtEngagement
            {
                UserId = user.Id,
                DestinationId = dest.Id,
                PhysicalArtId = art.Id
            };
            context.UserArtEngagements.Add(engagement);
            await context.SaveChangesAsync();

            // assert we can read it back with navigation
            var loaded = await context.UserArtEngagements
                .Include(e => e.PhysicalArt)
                .Include(e => e.Destination)
                .FirstOrDefaultAsync(e => e.UserId == user.Id);

            Assert.NotNull(loaded);
            Assert.Equal("Test Sculpture", loaded.PhysicalArt.Title);
            Assert.Equal("Testville", loaded.Destination.Name);
        }
    }
}
