using System.Threading.Tasks;
using Publishing.Core.Commands;
using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{
    public interface IProfileRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task UpdateAsync(UpdateProfileCommand cmd);
        Task<ProfileDto?> GetAsync(string id);
        Task<IEnumerable<ProfileDto>> GetAllAsync();
        Task CreateAsync(CreateProfileCommand cmd);
        Task DeleteAsync(string id);
    }
}
