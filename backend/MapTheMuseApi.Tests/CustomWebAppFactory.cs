using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using MapTheMuseApi;

namespace MapTheMuseApi.Tests
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")              
                .ConfigureTestServices(services => 
            {
                // Removing the real DbContext
                services.RemoveAll(typeof(DbContextOptions<MapTheMuseContext>));

                // Adding EF InMemory only
                services.AddDbContext<MapTheMuseContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Building the service provider and seed initial data
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MapTheMuseContext>();

                // Seeding Destinations, PhysicalArt, Media, Users
                db.Destinations.Add(new Destination { Id = 1, Name = "Paris", Description = "Capital city of France" });
                db.PhysicalArtworks.Add(new PhysicalArt { Id = 1, Title = "Mona Lisa", Artist = "Leonardo Da Vinci", ArtType = "Painting", LocationName = "Museum", DestinationId = 1 });
                db.Media.Add(new Media { Id = 1, Title = "Inception", MediaType = "Film", Creator = "Christopher Nolan" });
                db.AppUsers.RemoveRange(db.AppUsers);
                db.AppUsers.Add(new AppUser { Id = "abc", FirstName = "Test", LastName = "User", Country = "UK", PreferredLanguage = "English" });
                db.AppUsers.Add(new AppUser { Id = "cde", FirstName = "Test2", LastName = "User2", Country = "France", PreferredLanguage = "French" });

                db.SaveChanges();
            });
        }
    }
}
