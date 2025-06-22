using System.Threading.Tasks;
using Publishing.Core.Commands;

namespace Publishing.Core.Interfaces
{
    public interface IProfileRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task UpdateAsync(UpdateProfileCommand cmd);
    }
}
