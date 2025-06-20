using System.Data;
using System.Threading.Tasks;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnectionFactory _factory;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        public UnitOfWork(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task BeginAsync()
        {
            _connection = await _factory.CreateOpenConnectionAsync();
            _transaction = _connection.BeginTransaction();
        }

        public Task CommitAsync()
        {
            _transaction?.Commit();
            Dispose();
            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            _transaction?.Rollback();
            Dispose();
            return Task.CompletedTask;
        }

        public IDbConnection Connection => _connection ?? throw new InvalidOperationException("UnitOfWork not started");
        public IDbTransaction Transaction => _transaction ?? throw new InvalidOperationException("UnitOfWork not started");

        private void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
            _transaction = null;
            _connection = null;
        }
    }
}
