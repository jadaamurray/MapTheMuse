using System.Threading.Tasks;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MapTheMuseApi.Tests
{
    public class UserMediaEngagementTests
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
            var media = new Media
            {
                Title = "Test Song",
                Description = "Test Description",
                Creator = "Test Creator",
                MediaType = "Test MediaType",
            };
            var user = new AppUser
            {
                UserName = "test@unit.com",
                Email = "test@unit.com"
            };

            context.Destinations.Add(dest);
            context.Media.Add(media);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // 3) create a link table record
            var engagement = new UserMediaEngagement
            {
                UserId = user.Id,
                DestinationId = dest.Id,
                MediaId = media.Id
            };
            context.UserMediaEngagements.Add(engagement);
            await context.SaveChangesAsync();

            // 4) assert we can read it back with navigation
            var loaded = await context.UserMediaEngagements
                .Include(e => e.Media)
                .Include(e => e.Destination)
                .FirstOrDefaultAsync(e => e.UserId == user.Id);

            Assert.NotNull(loaded);
            Assert.Equal("Test Song", loaded.Media.Title);
            Assert.Equal("Testville", loaded.Destination.Name);
        }
    }
}
