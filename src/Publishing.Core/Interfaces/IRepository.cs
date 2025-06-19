namespace Publishing.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        Task<IEnumerable<T>> QueryAsync(string sql, object? param = null);
        Task<int> ExecuteAsync(string sql, object? param = null);
    }
}
