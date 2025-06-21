using Microsoft.AspNetCore.Mvc;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Publishing.Organization.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _service;

    public OrganizationController(IOrganizationService service)
    {
        _service = service;
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateOrganizationDto dto)
    {
        await _service.UpdateAsync(dto);
        return NoContent();
    }
}
