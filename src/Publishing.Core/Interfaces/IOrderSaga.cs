namespace Publishing.Core.Interfaces;

using Publishing.Core.DTOs;
using Publishing.Core.Domain;

public interface IOrderSaga
{
    Task<Order> ExecuteAsync(CreateOrderDto dto);
}
