namespace Publishing.Core.Interfaces;

using System.Threading.Tasks;
using Publishing.Core.DTOs;
using Publishing.Core.Domain;

public interface IOrderSaga
{
    Task<Order> ExecuteAsync(CreateOrderDto dto);
}
