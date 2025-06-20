using System;
using Microsoft.Extensions.Caching.Memory;

namespace Publishing.Infrastructure.DataAccess;

public sealed class MemoryCacheQueryDispatcher : IQueryDispatcher
{
    private readonly IQueryDispatcher _inner;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _ttl;

    public MemoryCacheQueryDispatcher(IQueryDispatcher inner, IMemoryCache cache, TimeSpan ttl)
    {
        _inner = inner;
        _cache = cache;
        _ttl = ttl;
    }

    public async Task<List<T>> QueryAsync<T>(SqlQuery<T> query, CancellationToken token = default)
    {
        var key = $"sql:{query.Sql}:{query.Parameters}";
        if (_cache.TryGetValue(key, out List<T> cached))
            return cached;
        var result = await _inner.QueryAsync(query, token);
        _cache.Set(key, result, _ttl);
        return result;
    }

    public async Task<T?> QuerySingleAsync<T>(SqlQuery<T> query, CancellationToken token = default)
    {
        var list = await QueryAsync(query, token);
        return list.Count > 0 ? list[0] : default;
    }
}
