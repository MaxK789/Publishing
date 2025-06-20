using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    [Obsolete("Replaced by query objects")]
    public class PrinteryRepository : IPrinteryRepository
    {
        public decimal GetPricePerPage() => 2.5m;
        public int GetPagesPerDay() => 100;
    }
}
