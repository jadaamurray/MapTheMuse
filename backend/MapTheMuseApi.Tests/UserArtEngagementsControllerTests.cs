using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using MapTheMuseApi.Dtos;
using MapTheMuseApi;

namespace MapTheMuseApi.Tests
{
    public class UserArtEngagementsControllerTests
        : IClassFixture<CustomWebAppFactory>
    {
        private readonly HttpClient _client;

        public UserArtEngagementsControllerTests(CustomWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Creates_And_Returns_ReadDto()
        {
            var create = new UserArtEngagementCreateDto
            {
                UserId = "abc",
                DestinationId = 1,
                PhysicalArtId = 1
            };

            var resp = await _client.PostAsJsonAsync("/api/userartengagements", create);
            resp.StatusCode.Should().Be(HttpStatusCode.Created);

            var read = await resp.Content.ReadFromJsonAsync<UserArtEngagementReadDto>();
            read.Should().NotBeNull();
            read!.Destination.Id.Should().Be(1);
            read.PhysicalArt.Id.Should().Be(1);
            read.DateVisited.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task GetByUser_Returns_All_For_User()
        {
            // First add two engagements for user 99
            for (int i = 0; i < 2; i++)
            {
                var create = new UserArtEngagementCreateDto
                {
                    UserId = "cde",
                    DestinationId = 1,
                    PhysicalArtId = 1
                };
                await _client.PostAsJsonAsync("/api/userartengagements", create);
            }

            var resp = await _client.GetAsync("/api/userartengagements/user/cde");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var list = await resp.Content.ReadFromJsonAsync<List<UserArtEngagementReadDto>>();
            list.Should().HaveCount(2);
            list.All(e => e.Destination.Id == 1).Should().BeTrue();
        }
    }
}