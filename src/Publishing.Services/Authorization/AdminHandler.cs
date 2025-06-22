using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Publishing.Services.Authorization;

public class AdminHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
    {
        if (context.User.IsInRole("admin"))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
