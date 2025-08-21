
using System.ComponentModel.DataAnnotations;

namespace MapTheMuseApi.Dtos
{
    public class UserProfilePatchDto
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Url] public string? ProfilePictureUrl { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(10)]
        public string? PreferredLanguage { get; set; }
    }
}