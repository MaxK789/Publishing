namespace Publishing.Infrastructure.DataAccess;

public sealed class UpdateProfileCommand : SqlCommand
{
    public UpdateProfileCommand(string id, string? fName, string? lName, string? email, string? status, string? phone, string? fax, string? address)
    {
        Id = id; FName = fName; LName = lName; Email = email; Status = status; Phone = phone; Fax = fax; Address = address;
    }

    public string Id { get; }
    public string? FName { get; }
    public string? LName { get; }
    public string? Email { get; }
    public string? Status { get; }
    public string? Phone { get; }
    public string? Fax { get; }
    public string? Address { get; }

    public override string Sql => @"UPDATE Person SET
        FName = ISNULL(@FName,FName),
        LName = ISNULL(@LName,LName),
        emailPerson = ISNULL(@Email,emailPerson),
        typePerson = ISNULL(@Status,typePerson),
        phonePerson = ISNULL(@Phone,phonePerson),
        faxPerson = ISNULL(@Fax,faxPerson),
        addressPerson = ISNULL(@Address,addressPerson)
        WHERE idPerson = @Id";

    public override object Parameters => new { Id, FName, LName, Email, Status, Phone, Fax, Address };
}
