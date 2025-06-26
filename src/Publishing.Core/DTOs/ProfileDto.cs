namespace Publishing.Core.DTOs
{
    public class ProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
    }
}
