using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiGateway.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;

namespace Publishing.Integration.Tests;

[TestClass]
public class AggregationCacheTests
{
    private class StubHandler : HttpMessageHandler
    {
        private readonly string _content;
        public StubHandler(string content) => _content = content;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(_content) });
    }

    private class StubFactory : IHttpClientFactory
    {
        private readonly IDictionary<string, HttpClient> _clients;
        public StubFactory(IDictionary<string, HttpClient> clients) => _clients = clients;
        public HttpClient CreateClient(string name) => _clients[name];
    }

    private class CaptureCache : IDistributedCache
    {
        public DistributedCacheEntryOptions? Options { get; private set; }
        public Task<byte[]?> GetAsync(string key, CancellationToken token = default) => Task.FromResult<byte[]?>(null);
        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            Options = options;
            return Task.CompletedTask;
        }
        public Task RefreshAsync(string key, CancellationToken token = default) => Task.CompletedTask;
        public Task RemoveAsync(string key, CancellationToken token = default) => Task.CompletedTask;
        public byte[]? Get(string key) => null;
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options) => Options = options;
        public void Refresh(string key) { }
        public void Remove(string key) { }
    }

    [TestMethod]
    public async Task Get_SetsOneMinuteCacheExpiration()
    {
        var clients = new Dictionary<string, HttpClient>
        {
            ["orders"] = new HttpClient(new StubHandler("[]")) { BaseAddress = new Uri("http://x") },
            ["profile"] = new HttpClient(new StubHandler("{}")) { BaseAddress = new Uri("http://x") },
            ["organization"] = new HttpClient(new StubHandler("{}")) { BaseAddress = new Uri("http://x") }
        };
        var factory = new StubFactory(clients);
        var cache = new CaptureCache();
        var controller = new AggregationController(factory, cache);

        await controller.Get("1");

        Assert.IsNotNull(cache.Options);
        Assert.AreEqual(TimeSpan.FromMinutes(1), cache.Options!.AbsoluteExpirationRelativeToNow);
    }
}

