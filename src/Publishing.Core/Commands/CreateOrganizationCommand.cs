namespace Publishing.Core.Commands;

public class CreateOrganizationCommand
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Fax { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PersonId { get; set; } = string.Empty;
}
