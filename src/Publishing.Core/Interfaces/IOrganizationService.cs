using System.Threading.Tasks;
using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{
    public interface IOrganizationService
    {
        Task UpdateAsync(UpdateOrganizationDto dto);
    }
}
