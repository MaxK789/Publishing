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
public class ProfileContracts
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
    public async Task GetProfiles_VerifyContract()
    {
        var pact = Pact.V3("ProfileService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A GET request to retrieve profiles")
            .WithRequest(HttpMethod.Get, "/profile/api/profile")
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.GetAsync("/profile/api/profile");
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }

    [TestMethod]
    public async Task GetProfileById_VerifyContract()
    {
        var pact = Pact.V3("ProfileService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A GET request to retrieve profile by id")
            .WithRequest(HttpMethod.Get, "/profile/api/profile/123")
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.GetAsync("/profile/api/profile/123");
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }

    [TestMethod]
    public async Task CreateProfile_VerifyContract()
    {
        var pact = Pact.V3("ProfileService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A POST request to create profile")
            .WithRequest(HttpMethod.Post, "/profile/api/profile")
            .WillRespond()
            .WithStatus(204);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.PostAsync("/profile/api/profile", new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.AreEqual(204, (int)response.StatusCode);
        });
    }

    [TestMethod]
    public async Task DeleteProfile_VerifyContract()
    {
        var pact = Pact.V3("ProfileService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A DELETE request to remove profile")
            .WithRequest(HttpMethod.Delete, "/profile/api/profile/123")
            .WillRespond()
            .WithStatus(204);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.DeleteAsync("/profile/api/profile/123");
            Assert.AreEqual(204, (int)response.StatusCode);
        });
    }

    [TestMethod]
    public async Task UpdateProfile_VerifyContract()
    {
        var pact = Pact.V3("ProfileService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A POST request to update profile")
            .WithRequest(HttpMethod.Post, "/profile/api/profile/update")
            .WillRespond()
            .WithStatus(204);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.PostAsync("/profile/api/profile/update", new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.AreEqual(204, (int)response.StatusCode);
        });
    }
}
