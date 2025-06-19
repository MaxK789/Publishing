using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IDbContext _db;

        public OrganizationRepository(IDbContext db)
        {
            _db = db;
        }

        public async Task<string?> GetNameIfExistsAsync(string name)
        {
            var res = await _db.QueryAsync<string>("SELECT nameOrganization FROM Organization WHERE nameOrganization = @orgName", new { orgName = name });
            return res.FirstOrDefault();
        }

        public Task InsertAsync(string name, string email, string phone, string fax, string address, string personId)
        {
            const string sql = "INSERT INTO Organization(nameOrganization, emailOrganization, phoneOrganization, faxOrganization, addressOrganization, idPerson) VALUES (@orgName, @Email, @phone, @fax, @address, @id)";
            return _db.ExecuteAsync(sql, new { orgName = name, Email = email, phone, fax, address, id = personId });
        }

        public Task UpdateAsync(string id, string? name, string? email, string? phone, string? fax, string? address)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var sb = new StringBuilder("UPDATE Organization SET");
            void Add(string field, string? value, string param)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append($" {field} = @{param},");
                    parameters.Add($"@{param}", value);
                }
            }
            Add("nameOrganization", name, "orgName");
            Add("emailOrganization", email, "Email");
            Add("phoneOrganization", phone, "phone");
            Add("faxOrganization", fax, "fax");
            Add("addressOrganization", address, "address");
            if (sb[sb.Length - 1] == ',')
                sb.Remove(sb.Length - 1, 1);
            sb.Append(" WHERE idPerson = @id");
            return _db.ExecuteAsync(sb.ToString(), parameters);
        }
    }
}
