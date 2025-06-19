using System.Threading.Tasks;

namespace Publishing.Core.Interfaces
{ 
    public interface IPrinteryRepository
    {
        Task<decimal> GetPricePerPageAsync();
        Task<int> GetPagesPerDayAsync();
    }
}
