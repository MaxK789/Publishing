namespace Publishing.Services.Authorization;

using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

public class StatisticsViewerHandler : AuthorizationHandler<StatisticsViewerRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StatisticsViewerRequirement requirement)
    {
        if (context.User.IsInRole("statistics"))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
