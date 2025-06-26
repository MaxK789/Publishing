using System.Collections.Generic;
using Publishing.Core.DTOs;

namespace ApiGateway.Models;

public class AggregatedResponseDto
{
    public ProfileDto? Profile { get; set; }
    public OrganizationDto? Organization { get; set; }
    public List<OrderDto> Orders { get; set; } = new();
}
