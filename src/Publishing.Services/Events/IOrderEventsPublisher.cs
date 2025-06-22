namespace Publishing.Services;

using System;
using Publishing.Core.DTOs;

public interface IOrderEventsPublisher
{
    event Action<OrderDto>? OrderCreated;
    event Action<OrderDto>? OrderUpdated;

    void PublishOrderCreated(OrderDto order);
    void PublishOrderUpdated(OrderDto order);
}
