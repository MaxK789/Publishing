using Microsoft.AspNetCore.Mvc;
using Publishing.Core.Interfaces;
using Publishing.Core.Domain;
using Publishing.Core.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Data;

namespace Publishing.Orders.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orders;
    private readonly IOrderSaga _saga;
    private readonly IDistributedCache _cache;
    private readonly IOrderInputValidator _validator;

    public OrdersController(IOrderRepository orders, IDistributedCache cache, IOrderInputValidator validator, IOrderSaga saga)
    {
        _orders = orders;
        _cache = cache;
        _validator = validator;
        _saga = saga;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cached = await _cache.GetStringAsync("orders_all");
        if (cached != null)
        {
            var orders = JsonSerializer.Deserialize<List<Dictionary<string, object?>>>(cached);
            return Ok(orders);
        }

        DataTable table = await _orders.GetAllAsync();
        var list = table.Rows.Cast<DataRow>()
            .Select(r => table.Columns.Cast<DataColumn>()
                .ToDictionary(c => c.ColumnName, c => r[c]))
            .ToList();
        await _cache.SetStringAsync("orders_all", JsonSerializer.Serialize(list));
        return Ok(list);
    }

    [HttpGet("person/{id}")]
    public async Task<IActionResult> ByPerson(string id)
    {
        DataTable table = await _orders.GetByPersonAsync(id);
        var list = table.Rows.Cast<DataRow>()
            .Select(r => table.Columns.Cast<DataColumn>()
                .ToDictionary(c => c.ColumnName, c => r[c]))
            .ToList();
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        _validator.Validate(dto);
        var order = await _saga.ExecuteAsync(dto);
        await _cache.RemoveAsync("orders_all");
        return Ok(order);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _orders.DeleteAsync(id);
        await _cache.RemoveAsync("orders_all");
        return NoContent();
    }
}
