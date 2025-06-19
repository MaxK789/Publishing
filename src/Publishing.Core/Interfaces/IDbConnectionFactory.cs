namespace Publishing.Core.Interfaces
{
    using System.Data;
    using System.Threading.Tasks;

    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateOpenConnectionAsync();
    }
}
