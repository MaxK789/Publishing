using AutoMapper;
using Publishing.Core.Domain;
using Publishing.Core.DTOs;
using Publishing.AppLayer.Commands;

namespace Publishing.AppLayer.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderDto, CreateOrderCommand>();
        CreateMap<UpdateOrderDto, UpdateOrderCommand>();
        CreateMap<Order, OrderDto>();
    }
}
