using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Publishing.Core.DTOs;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AggregationController : ControllerBase
{
    private readonly IHttpClientFactory _factory;

    public AggregationController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    [HttpGet("person/{id}")]
    public async Task<ActionResult<AggregatedResponseDto>> Get(string id)
    {
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
        return Ok(result);
    }
}
