using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Mapping;

namespace Publishing.Core.Tests;

[TestClass]
public class MappingProfileTests
{
    [TestMethod]
    public void Profiles_AreValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderProfile>();
            cfg.AddProfile<OrganizationProfile>();
            cfg.AddProfile<ProfileProfile>();
            cfg.AddProfile<UserProfile>();
        });

        configuration.AssertConfigurationIsValid();
    }
}
