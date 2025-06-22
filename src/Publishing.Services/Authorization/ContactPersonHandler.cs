namespace Publishing.Services.Authorization;

using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

public class ContactPersonHandler : AuthorizationHandler<ContactPersonRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ContactPersonRequirement requirement)
    {
        if (context.User.IsInRole("contact"))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
