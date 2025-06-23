namespace Publishing.Services;

using System;
using Publishing.Core.DTOs;

public class OrderEventsPublisher : IOrderEventsPublisher
{
    public event Action<OrderDto>? OrderCreated;
    public event Action<OrderDto>? OrderUpdated;

    public void PublishOrderCreated(OrderDto order)
    {
        OrderCreated?.Invoke(order);
    }

    public void PublishOrderUpdated(OrderDto order)
    {
        OrderUpdated?.Invoke(order);
    }
}
