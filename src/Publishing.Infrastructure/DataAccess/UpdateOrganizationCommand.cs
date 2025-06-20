namespace Publishing.Infrastructure.DataAccess;

public sealed class UpdateOrganizationCommand : SqlCommand
{
    public UpdateOrganizationCommand(string id, string? name, string? email, string? phone, string? fax, string? address)
    {
        Id = id; Name = name; Email = email; Phone = phone; Fax = fax; Address = address;
    }

    public string Id { get; }
    public string? Name { get; }
    public string? Email { get; }
    public string? Phone { get; }
    public string? Fax { get; }
    public string? Address { get; }

    public override string Sql => @"UPDATE Organization SET
        nameOrganization = ISNULL(@Name,nameOrganization),
        emailOrganization = ISNULL(@Email,emailOrganization),
        phoneOrganization = ISNULL(@Phone,phoneOrganization),
        faxOrganization = ISNULL(@Fax,faxOrganization),
        addressOrganization = ISNULL(@Address,addressOrganization)
        WHERE idPerson = @Id";

    public override object Parameters => new { Id, Name, Email, Phone, Fax, Address };
}
