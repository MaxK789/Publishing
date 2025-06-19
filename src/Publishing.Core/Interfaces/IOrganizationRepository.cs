using System.Threading.Tasks;

namespace Publishing.Core.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<string?> GetNameIfExistsAsync(string name);
        Task InsertAsync(string name, string email, string phone, string fax, string address, string personId);
        Task UpdateAsync(string id, string? name, string? email, string? phone, string? fax, string? address);
    }
}
