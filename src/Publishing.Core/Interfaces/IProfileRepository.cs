using System.Threading.Tasks;

namespace Publishing.Core.Interfaces
{
    public interface IProfileRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task UpdateAsync(string id, string? fName, string? lName, string? email, string? status, string? phone, string? fax, string? address);
    }
}
