using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services;

namespace Publishing.Core.Tests;

[TestClass]
public class RoleServiceTests
{
    [TestMethod]
    public void IsAdmin_ReturnsTrue_ForAdmin()
    {
        var service = new RoleService();
        Assert.IsTrue(service.IsAdmin("admin"));
    }

    [TestMethod]
    public void IsAdmin_ReturnsFalse_ForOther()
    {
        var service = new RoleService();
        Assert.IsFalse(service.IsAdmin("user"));
    }

    [TestMethod]
    public void IsContactPerson_ReturnsTrue_ForContact()
    {
        var service = new RoleService();
        Assert.IsTrue(service.IsContactPerson("контактна особа"));
    }
}
