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
public class AggregationContracts
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
    public async Task GetAggregatedPerson_VerifyContract()
    {
        var pact = Pact.V3("GatewayAggregation", "Consumer", new PactConfig());
        pact
            .UponReceiving("A GET request for aggregated person info")
            .WithRequest(HttpMethod.Get, "/api/aggregation/person/123")
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.GetAsync("/api/aggregation/person/123");
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }
}

