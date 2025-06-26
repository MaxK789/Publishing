using Microsoft.AspNetCore.Mvc;
using Publishing.Core.DTOs;
using Publishing.Core.Commands;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;
using Publishing.Core.Interfaces;
using Publishing.Services;

namespace Publishing.Profile.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;
    private readonly IProfileRepository _profiles;
    private readonly IProfileEventsPublisher _events;

    public ProfileController(IMediator mediator, IDistributedCache cache, IMapper mapper, IProfileRepository profiles, IProfileEventsPublisher events)
    {
        _mediator = mediator;
        _cache = cache;
        _mapper = mapper;
        _profiles = profiles;
        _events = events;
    }

    [HttpGet]
    public async Task<IEnumerable<ProfileDto>> GetAll()
    {
        return await _profiles.GetAllAsync();
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Create(CreateProfileDto dto)
    {
        var cmd = _mapper.Map<CreateProfileCommand>(dto);
        await _profiles.CreateAsync(cmd);
        return NoContent();
    }

    [HttpPost("update")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Update(UpdateProfileDto dto)
    {
        var cmd = _mapper.Map<UpdateProfileCommand>(dto);
        await _mediator.Send(cmd);
        var profile = await _profiles.GetAsync(dto.Id);
        if (profile is not null)
            _events.PublishProfileUpdated(profile);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Delete(string id)
    {
        await _profiles.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileDto>> Get(string id)
    {
        var profile = await _profiles.GetAsync(id);
        if (profile is null) return NotFound();
        return Ok(profile);
    }
}
