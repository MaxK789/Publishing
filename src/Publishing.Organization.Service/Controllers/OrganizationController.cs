using Microsoft.AspNetCore.Mvc;
using Publishing.Core.DTOs;
using Publishing.Core.Commands;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Publishing.Organization.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrganizationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrganizationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("update")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Update(UpdateOrganizationDto dto)
    {
        var cmd = _mapper.Map<UpdateOrganizationCommand>(dto);
        await _mediator.Send(cmd);
        return NoContent();
    }
}
