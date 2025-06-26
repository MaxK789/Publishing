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
public class OrderContracts
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
    public async Task GetOrders_VerifyContract()
    {
        var pact = Pact.V3("OrdersService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A GET request to retrieve orders")
            .WithRequest(HttpMethod.Get, "/orders/api/orders")
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var response = await client.GetAsync("/orders/api/orders");
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }

    [TestMethod]
    public async Task CreateOrder_VerifyContract()
    {
        var pact = Pact.V3("OrdersService", "Gateway", new PactConfig());
        pact
            .UponReceiving("A POST request to create an order")
            .WithRequest(HttpMethod.Post, "/orders/api/orders")
            .WithJsonBody(new { Name = "Book", Type = "Standard", Pages = 1, Tirage = 1, DateStart = "2021-01-01", DateFinish = "2021-01-02", PersonId = "p1", Printery = "main", Price = 10 })
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt());
            var content = new StringContent("{\"name\":\"Book\",\"type\":\"Standard\",\"pages\":1,\"tirage\":1,\"dateStart\":\"2021-01-01\",\"dateFinish\":\"2021-01-02\",\"personId\":\"p1\",\"printery\":\"main\",\"price\":10}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/orders/api/orders", content);
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }
}
