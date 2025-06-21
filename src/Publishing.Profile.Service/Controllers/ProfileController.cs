using Microsoft.AspNetCore.Mvc;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;

namespace Publishing.Profile.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _service;
    private readonly IDistributedCache _cache;

    public ProfileController(IProfileService service, IDistributedCache cache)
    {
        _service = service;
        _cache = cache;
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateProfileDto dto)
    {
        await _service.UpdateAsync(dto);
        await _cache.RemoveAsync("orders_all");
        return NoContent();
    }
}
