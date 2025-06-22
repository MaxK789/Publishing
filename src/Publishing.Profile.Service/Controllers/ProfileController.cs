using Microsoft.AspNetCore.Mvc;
using Publishing.Core.DTOs;
using Publishing.Core.Commands;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;

namespace Publishing.Profile.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public ProfileController(IMediator mediator, IDistributedCache cache, IMapper mapper)
    {
        _mediator = mediator;
        _cache = cache;
        _mapper = mapper;
    }

    [HttpPost("update")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Update(UpdateProfileDto dto)
    {
        var cmd = _mapper.Map<UpdateProfileCommand>(dto);
        await _mediator.Send(cmd);
        await _cache.RemoveAsync("orders_all");
        return NoContent();
    }
}
