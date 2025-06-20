using System.Threading.Tasks;

namespace Publishing.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
        System.Data.IDbConnection Connection { get; }
        System.Data.IDbTransaction Transaction { get; }
    }
}
