using System.Threading;
using System.Threading.Tasks;

namespace Publishing.Infrastructure.DataAccess;

public interface IQueryDispatcher
{
    Task<List<T>> QueryAsync<T>(SqlQuery<T> query, CancellationToken token = default);
    Task<T?> QuerySingleAsync<T>(SqlQuery<T> query, CancellationToken token = default);
}
