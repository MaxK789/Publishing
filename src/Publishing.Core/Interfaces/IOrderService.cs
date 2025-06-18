using Publishing.Core.Domain;
using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{
    public interface IOrderService
    {
        Order CreateOrder(CreateOrderDto dto);
    }
}
