using System.Threading.Tasks;
using Publishing.Core.Commands;

namespace Publishing.Core.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<string?> GetNameIfExistsAsync(string name);
        Task InsertAsync(CreateOrganizationCommand cmd);
        Task UpdateAsync(UpdateOrganizationCommand cmd);
    }
}
