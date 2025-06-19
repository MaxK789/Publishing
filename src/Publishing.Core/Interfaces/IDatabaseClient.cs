namespace Publishing.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;

    public interface IDatabaseClient
    {
        void OpenConnection();
        void OpenConnection(string login, string password);
        DataTable ExecuteQueryToDataTable(string query, List<SqlParameter>? parameters = null);
        List<string[]> ExecuteQueryList(string query, List<SqlParameter>? parameters = null);
        string ExecuteQuery(string query, List<SqlParameter>? parameters = null);
        void ExecuteQueryWithoutResponse(string query, List<SqlParameter>? parameters = null);
        void CloseConnection();
    }
}
