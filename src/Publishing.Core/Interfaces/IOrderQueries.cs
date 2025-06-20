using System.Data;
using System.Threading.Tasks;

namespace Publishing.Core.Interfaces
{
    public interface IOrderQueries
    {
        Task<DataTable> GetActiveAsync();
        Task<DataTable> GetByPersonAsync(string personId);
        Task<DataTable> GetAllAsync();
    }
}
