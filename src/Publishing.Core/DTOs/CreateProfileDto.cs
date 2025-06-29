namespace Publishing.Core.DTOs;

public class CreateProfileDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Address { get; set; }
}
