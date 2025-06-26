using System.Data;
using System.Threading.Tasks;
using Publishing.Core.Domain;

namespace Publishing.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> SaveAsync(Order order);

        Task UpdateExpiredAsync();

        Task<DataTable> GetActiveAsync();

        Task<DataTable> GetByPersonAsync(string personId);

        Task<DataTable> GetAllAsync();

        Task DeleteAsync(int id);

        Task DeleteLatestAsync(string personId);

        Task UpdateStatusAsync(int id, string status);

        Task<string?> GetPersonIdAsync(int id);
    }
}
