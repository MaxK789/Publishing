namespace Publishing.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDbContext
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
        Task<int> ExecuteAsync(string sql, object? param = null);
    }
}
