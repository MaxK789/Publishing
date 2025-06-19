using Publishing.Core.Interfaces;
using System.Threading.Tasks;

namespace Publishing.Infrastructure.Repositories
{
    public class PrinteryRepository : IPrinteryRepository
    {
        public Task<decimal> GetPricePerPageAsync() => Task.FromResult(2.5m);
        public Task<int> GetPagesPerDayAsync() => Task.FromResult(100);
    }
}
