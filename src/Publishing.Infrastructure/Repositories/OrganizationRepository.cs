using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using Publishing.Core.Interfaces;
using Publishing.Core.Commands;
using Publishing.Core.DTOs;

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

        public Task InsertAsync(CreateOrganizationCommand cmd)
        {
            const string sql = "INSERT INTO Organization(nameOrganization, emailOrganization, phoneOrganization, faxOrganization, addressOrganization, idPerson) VALUES (@orgName, @Email, @phone, @fax, @address, @id)";
            return _db.ExecuteAsync(sql, new { orgName = cmd.Name, Email = cmd.Email, phone = cmd.Phone, fax = cmd.Fax, address = cmd.Address, id = cmd.PersonId });
        }

        public Task UpdateAsync(UpdateOrganizationCommand cmd)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", cmd.Id);
            var sb = new StringBuilder("UPDATE Organization SET");
            void Add(string field, string? value, string param)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append($" {field} = @{param},");
                    parameters.Add($"@{param}", value);
                }
            }
            Add("nameOrganization", cmd.Name, "orgName");
            Add("emailOrganization", cmd.Email, "Email");
            Add("phoneOrganization", cmd.Phone, "phone");
            Add("faxOrganization", cmd.Fax, "fax");
            Add("addressOrganization", cmd.Address, "address");
            if (sb[sb.Length - 1] == ',')
                sb.Remove(sb.Length - 1, 1);
            sb.Append(" WHERE idPerson = @id");
            return _db.ExecuteAsync(sb.ToString(), parameters);
        }

        public async Task<OrganizationDto?> GetByPersonIdAsync(string personId)
        {
            const string sql = @"SELECT TOP (1) nameOrganization AS Name, emailOrganization AS Email,
                phoneOrganization AS Phone, faxOrganization AS Fax, addressOrganization AS Address
                FROM Organization WHERE idPerson = @personId";
            var res = await _db.QueryAsync<OrganizationDto>(sql, new { personId });
            return res.FirstOrDefault();
        }

        public Task<IEnumerable<OrganizationDto>> GetAllAsync()
        {
            const string sql = @"SELECT nameOrganization AS Name, emailOrganization AS Email,
                phoneOrganization AS Phone, faxOrganization AS Fax, addressOrganization AS Address
                FROM Organization";
            return _db.QueryAsync<OrganizationDto>(sql);
        }

        public Task DeleteAsync(string id)
        {
            const string sql = "DELETE FROM Organization WHERE idPerson = @id";
            return _db.ExecuteAsync(sql, new { id });
        }
    }
}
