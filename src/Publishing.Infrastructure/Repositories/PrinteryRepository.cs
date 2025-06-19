using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    public class PrinteryRepository : IPrinteryRepository
    {
        public decimal GetPricePerPage() => 2.5m;
        public int GetPagesPerDay() => 100;
    }
}
