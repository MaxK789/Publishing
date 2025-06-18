using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Publishing.Core.Services
{
    public static class SqlParameterBuilder
    {
        public static List<SqlParameter> FromDictionary(Dictionary<string, object?> values)
        {
            var list = new List<SqlParameter>();
            foreach (var kvp in values)
            {
                list.Add(new SqlParameter("@" + kvp.Key, kvp.Value ?? DBNull.Value));
            }
            return list;
        }
    }
}
