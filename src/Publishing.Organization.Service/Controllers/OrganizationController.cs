using Microsoft.AspNetCore.Mvc;
using Publishing.Core.DTOs;
using Publishing.Core.Commands;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Publishing.Core.Interfaces;

namespace Publishing.Organization.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/org")]
public class OrganizationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IOrganizationRepository _organizations;

    public OrganizationController(IMediator mediator, IMapper mapper, IOrganizationRepository organizations)
    {
        _mediator = mediator;
        _mapper = mapper;
        _organizations = organizations;
    }

    [HttpGet]
    public async Task<IEnumerable<OrganizationDto>> GetAll()
    {
        return await _organizations.GetAllAsync();
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Create(CreateOrganizationDto dto)
    {
        var cmd = _mapper.Map<CreateOrganizationCommand>(dto);
        await _organizations.InsertAsync(cmd);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Delete(string id)
    {
        await _organizations.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("update")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Update(UpdateOrganizationDto dto)
    {
        var cmd = _mapper.Map<UpdateOrganizationCommand>(dto);
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpGet("person/{personId}")]
    public async Task<ActionResult<OrganizationDto>> GetByPerson(string personId)
    {
        var org = await _organizations.GetByPersonIdAsync(personId);
        if (org is null) return NotFound();
        return Ok(org);
    }
}
