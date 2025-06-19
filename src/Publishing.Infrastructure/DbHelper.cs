using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure
{
    public class DbHelper : IDbHelper
    {
        private readonly IDbContext _db;

        public DbHelper(IDbContext db)
        {
            _db = db;
        }

        public async Task<DataTable> QueryDataTableAsync(string sql, object? param = null)
        {
            var result = await _db.QueryAsync<dynamic>(sql, param);
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

        public async Task<List<string[]>> QueryStringListAsync(string sql, object? param = null)
        {
            var result = await _db.QueryAsync<dynamic>(sql, param);
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
