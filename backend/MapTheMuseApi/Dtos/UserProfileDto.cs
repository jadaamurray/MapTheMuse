namespace MapTheMuseApi.Dtos
{
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Country { get; set; }
        public string PreferredLanguage { get; set; }

    }
}