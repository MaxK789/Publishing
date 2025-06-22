using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Publishing.Services.Authorization;

namespace Publishing.Core.Tests;

[TestClass]
public class AdminHandlerTests
{
    [TestMethod]
    public async Task HandleRequirementAsync_Succeeds_ForAdmin()
    {
        var handler = new AdminHandler();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "admin") }));
        var requirement = new AdminRequirement();
        var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);
        await handler.HandleAsync(context);
        Assert.IsTrue(context.HasSucceeded);
    }

    [TestMethod]
    public async Task HandleRequirementAsync_Fails_ForNonAdmin()
    {
        var handler = new AdminHandler();
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var requirement = new AdminRequirement();
        var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);
        await handler.HandleAsync(context);
        Assert.IsFalse(context.HasSucceeded);
    }
}
