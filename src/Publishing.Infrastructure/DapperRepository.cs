using System.Collections.Generic;
using System.Threading.Tasks;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure
{
    public abstract class DapperRepository<T> : IRepository<T>
    {
        protected readonly IDbContext Db;

        protected DapperRepository(IDbContext db)
        {
            Db = db;
        }

        public Task<IEnumerable<T>> QueryAsync(string sql, object? param = null)
            => Db.QueryAsync<T>(sql, param);

        public Task<int> ExecuteAsync(string sql, object? param = null)
            => Db.ExecuteAsync(sql, param);
    }
}
