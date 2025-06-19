namespace Publishing.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public interface IDbHelper
    {
        Task<DataTable> QueryDataTableAsync(string sql, object? param = null);
        Task<List<string[]>> QueryStringListAsync(string sql, object? param = null);
    }
}
