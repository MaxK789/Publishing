using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using PactNet.Matchers;
using System;
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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey123"));
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
}
