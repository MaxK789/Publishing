using System.Threading.Tasks;
using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{
    public interface IProfileService
    {
        Task UpdateAsync(UpdateProfileDto dto);
    }
}
