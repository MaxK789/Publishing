using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Publishing.Core.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AggregationController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    private readonly IDistributedCache _cache;

    public AggregationController(IHttpClientFactory factory, IDistributedCache cache)
    {
        _factory = factory;
        _cache = cache;
    }

    [HttpGet("person/{id}")]
    public async Task<ActionResult<AggregatedResponseDto>> Get(string id)
    {
        var cached = await _cache.GetStringAsync($"agg_{id}");
        if (cached is not null)
        {
            var dto = JsonSerializer.Deserialize<AggregatedResponseDto>(cached);
            return Ok(dto);
        }

        var ordersClient = _factory.CreateClient("orders");
        var profileClient = _factory.CreateClient("profile");
        var orgClient = _factory.CreateClient("organization");

        var ordersTask = ordersClient.GetFromJsonAsync<List<OrderDto>>($"/api/orders/person/{id}");
        var profileTask = profileClient.GetFromJsonAsync<ProfileDto>($"/api/profile/{id}");
        var orgTask = orgClient.GetFromJsonAsync<OrganizationDto>($"/api/org/person/{id}");

        await Task.WhenAll(ordersTask, profileTask, orgTask);

        var result = new AggregatedResponseDto
        {
            Orders = ordersTask.Result ?? new List<OrderDto>(),
            Profile = profileTask.Result,
            Organization = orgTask.Result
        };
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        };
        await _cache.SetStringAsync($"agg_{id}", JsonSerializer.Serialize(result), options);
        return Ok(result);
    }
}
