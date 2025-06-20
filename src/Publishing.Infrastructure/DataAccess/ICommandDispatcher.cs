using System.Threading;
using System.Threading.Tasks;

namespace Publishing.Infrastructure.DataAccess;

public interface ICommandDispatcher
{
    Task<int> ExecuteAsync(SqlCommand command, CancellationToken token = default);
}
