using System;
using System.Net.Http.Json;
using Publishing.Core.DTOs;
using Publishing.Core.Domain;
using Publishing.Core.Interfaces;

namespace Publishing.Orders.Service.Sagas;

public class OrderSaga : IOrderSaga
{
    private readonly IHttpClientFactory _factory;
    private readonly IOrderRepository _orders;

    public OrderSaga(IHttpClientFactory factory, IOrderRepository orders)
    {
        _factory = factory;
        _orders = orders;
    }

    public async Task<Order> ExecuteAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            Type = dto.Type,
            Name = dto.Name,
            Pages = dto.Pages,
            Tirage = dto.Tirage,
            DateStart = dto.DateStart,
            DateFinish = dto.DateFinish,
            PersonId = dto.PersonId,
            Printery = dto.Printery,
            Price = dto.Price
        };

        await _orders.SaveAsync(order);
        try
        {
            var profile = _factory.CreateClient("profile-updates");
            var res1 = await profile.PostAsJsonAsync($"/api/profile/update", new UpdateProfileDto { Id = dto.PersonId });
            res1.EnsureSuccessStatusCode();
            var organization = _factory.CreateClient("organization-updates");
            var res2 = await organization.PostAsJsonAsync("/api/org/update", new UpdateOrganizationDto { Id = dto.PersonId });
            res2.EnsureSuccessStatusCode();
            return order;
        }
        catch
        {
            await _orders.DeleteLatestAsync(dto.PersonId);
            throw;
        }
    }
}
