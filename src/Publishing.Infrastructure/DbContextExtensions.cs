namespace Publishing.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using Publishing.Core.Interfaces;

    public static class DbContextExtensions
    {
        public static async Task<DataTable> QueryDataTableAsync(this IDbContext db, string sql, object? param = null)
        {
            var result = await db.QueryAsync<dynamic>(sql, param);
            var table = new DataTable();
            bool columnsCreated = false;
            foreach (var row in result)
            {
                var dict = (IDictionary<string, object>)row;
                if (!columnsCreated)
                {
                    foreach (var k in dict.Keys)
                    {
                        table.Columns.Add(k);
                    }
                    columnsCreated = true;
                }
                var dataRow = table.NewRow();
                foreach (var k in dict.Keys)
                {
                    dataRow[k] = dict[k] ?? DBNull.Value;
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        public static async Task<List<string[]>> QueryStringListAsync(this IDbContext db, string sql, object? param = null)
        {
            var result = await db.QueryAsync<dynamic>(sql, param);
            var list = new List<string[]>();
            foreach (var row in result)
            {
                var dict = (IDictionary<string, object>)row;
                var arr = new string[dict.Count];
                int i = 0;
                foreach (var val in dict.Values)
                {
                    arr[i++] = val?.ToString() ?? string.Empty;
                }
                list.Add(arr);
            }
            return list;
        }
    }
}
