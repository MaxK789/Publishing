using Publishing.Core.Domain;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void Save(Order order)
        {
            // Database save logic would be here.
        }
    }
}
