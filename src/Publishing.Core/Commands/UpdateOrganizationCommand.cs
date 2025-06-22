using MediatR;

namespace Publishing.Core.Commands;

public class UpdateOrganizationCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Address { get; set; }
}
