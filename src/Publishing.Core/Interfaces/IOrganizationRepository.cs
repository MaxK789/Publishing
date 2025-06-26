using System.Threading.Tasks;
using System.Collections.Generic;
using Publishing.Core.Commands;
using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<string?> GetNameIfExistsAsync(string name);
        Task InsertAsync(CreateOrganizationCommand cmd);
        Task UpdateAsync(UpdateOrganizationCommand cmd);
        Task<OrganizationDto?> GetByPersonIdAsync(string personId);
        Task<IEnumerable<OrganizationDto>> GetAllAsync();
        Task DeleteAsync(string id);
    }
}
