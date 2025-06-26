using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Publishing.Contracts.Tests;

[TestClass]
public class OrganizationContracts
{
    private static string CreateJwt()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKey1234567890123456"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "example.com",
            audience: "example.com",
            claims: new[] { new Claim("sub", "contract-test") },
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [TestMethod]
    public async Task GetOrganizations_VerifyContract()
    {
        var pact = Pact.V3("OrganizationService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A GET request to retrieve organizations")
            .WithRequest(HttpMethod.Get, "/organization/api/organization")
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.GetAsync("/organization/api/organization");
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }

    [TestMethod]
    public async Task GetOrganizationByPerson_VerifyContract()
    {
        var pact = Pact.V3("OrganizationService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A GET request to retrieve organization by person")
            .WithRequest(HttpMethod.Get, "/organization/api/org/person/123")
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.GetAsync("/organization/api/org/person/123");
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }

    [TestMethod]
    public async Task CreateOrganization_VerifyContract()
    {
        var pact = Pact.V3("OrganizationService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A POST request to create organization")
            .WithRequest(HttpMethod.Post, "/organization/api/org")
            .WillRespond()
            .WithStatus(204);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.PostAsync("/organization/api/org", new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.AreEqual(204, (int)response.StatusCode);
        });
    }

    [TestMethod]
    public async Task DeleteOrganization_VerifyContract()
    {
        var pact = Pact.V3("OrganizationService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A DELETE request to remove organization")
            .WithRequest(HttpMethod.Delete, "/organization/api/org/123")
            .WillRespond()
            .WithStatus(204);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.DeleteAsync("/organization/api/org/123");
            Assert.AreEqual(204, (int)response.StatusCode);
        });
    }

    [TestMethod]
    public async Task UpdateOrganization_VerifyContract()
    {
        var pact = Pact.V3("OrganizationService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A POST request to update organization")
            .WithRequest(HttpMethod.Post, "/organization/api/org/update")
            .WillRespond()
            .WithStatus(204);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.PostAsync("/organization/api/org/update", new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.AreEqual(204, (int)response.StatusCode);
        });
    }
}
