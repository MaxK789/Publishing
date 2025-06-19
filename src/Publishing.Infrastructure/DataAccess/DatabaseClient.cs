using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.DataAccess
{
    public class DatabaseClient : IDatabaseClient
    {
        public void OpenConnection() => DataBase.OpenConnection();
        public void OpenConnection(string login, string password) => DataBase.OpenConnection(login, password);
        public DataTable ExecuteQueryToDataTable(string query, List<SqlParameter>? parameters = null) =>
            DataBase.ExecuteQueryToDataTable(query, parameters);
        public List<string[]> ExecuteQueryList(string query, List<SqlParameter>? parameters = null) =>
            DataBase.ExecuteQueryList(query, parameters);
        public string ExecuteQuery(string query, List<SqlParameter>? parameters = null) =>
            DataBase.ExecuteQuery(query, parameters);
        public void ExecuteQueryWithoutResponse(string query, List<SqlParameter>? parameters = null) =>
            DataBase.ExecuteQueryWithoutResponse(query, parameters);
        public void CloseConnection() => DataBase.CloseConnection();
    }
}
