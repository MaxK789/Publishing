using Publishing.Core.Domain;

namespace Publishing.Core.Interfaces
{
    public interface IOrderRepository
    {
        void Save(Order order);
    }
}
