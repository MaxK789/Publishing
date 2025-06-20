namespace Publishing.Infrastructure.DataAccess;

public sealed class InsertOrganizationCommand : SqlCommand
{
    public InsertOrganizationCommand(string name, string email, string phone, string fax, string address, string personId)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Fax = fax;
        Address = address;
        PersonId = personId;
    }

    public string Name { get; }
    public string Email { get; }
    public string Phone { get; }
    public string Fax { get; }
    public string Address { get; }
    public string PersonId { get; }

    public override string Sql =>
        "INSERT INTO Organization(nameOrganization, emailOrganization, phoneOrganization, faxOrganization, addressOrganization, idPerson) VALUES (@Name, @Email, @Phone, @Fax, @Address, @PersonId)";

    public override object Parameters => new { Name, Email, Phone, Fax, Address, PersonId };
}
