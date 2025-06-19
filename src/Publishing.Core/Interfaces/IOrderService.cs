using Publishing.Core.Domain;
using Publishing.Core.DTOs;
using System.Threading.Tasks;

namespace Publishing.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderDto dto);
    }
}
