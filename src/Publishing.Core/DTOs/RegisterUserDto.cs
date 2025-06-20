namespace Publishing.Core.DTOs
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Status { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
