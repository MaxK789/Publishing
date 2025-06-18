using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Publishing
{
    internal class DataBase
    {
        public static string connectString;

        static SqlConnection sqlConnection;

        static string SQLServerName = "My-PC";

        static string dataBaseName = "Publishing";

        public static void OpenConnection()
        {
            connectString = @"Data Source=" + SQLServerName + ";Initial Catalog=" + dataBaseName + ";Integrated Security=true";

            sqlConnection = new SqlConnection(connectString);

            sqlConnection.Open();
        }

        public static void OpenConnection(string login, string password)
        {
            connectString = @"Data Source=" + SQLServerName + ";Initial Catalog=" + dataBaseName + ";User ID=" + login + ";Password=" + password + ";";

            sqlConnection = new SqlConnection(connectString);

            sqlConnection.Open();
        }

        public static DataTable ExecuteQueryToDataTable(string query)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                dataTable.Columns.Add(reader.GetName(i), typeof(string));
            }

            while (reader.Read())
            {
                DataRow row = dataTable.NewRow();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i].ToString();
                }

                dataTable.Rows.Add(row);
            }

            reader.Close();

            return dataTable;
        }

        public static DataTable ExecuteQueryToDataTable(string query, List<SqlParameter> parameters = null)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            SqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                dataTable.Columns.Add(reader.GetName(i), typeof(string));
            }

            while (reader.Read())
            {
                DataRow row = dataTable.NewRow();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i].ToString();
                }

                dataTable.Rows.Add(row);
            }

            reader.Close();

            return dataTable;
        }


        public static List<string[]> ExecuteQueryList(string query)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);

            List<string[]> result = new List<string[]>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                int col = reader.FieldCount;

                while (reader.Read())
                {
                    string[] row = new string[col];

                    for (int i = 0; i < col; i++)
                    {
                        row[i] = reader[i].ToString();
                    }

                    result.Add(row);
                }
            }

            return result;
        }

        public static List<string[]> ExecuteQueryList(string query, List<SqlParameter> parameters = null)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            List<string[]> result = new List<string[]>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                int col = reader.FieldCount;

                while (reader.Read())
                {
                    string[] row = new string[col];

                    for (int i = 0; i < col; i++)
                    {
                        row[i] = reader[i].ToString();
                    }

                    result.Add(row);
                }
            }

            return result;
        }

        public static string ExecuteQuery(string query)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);

            SqlDataReader reader = command.ExecuteReader();

            string result = null;

            while(reader.Read())
            {
                result = reader[0].ToString();
            }

            reader.Close();

            return result;
        }

        public static string ExecuteQuery(string query, List<SqlParameter> parameters = null)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);
            
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            SqlDataReader reader = command.ExecuteReader();

            string result = null;

            while (reader.Read())
            {
                result = reader[0].ToString();
            }

            reader.Close();

            return result;
        }

        public static void ExecuteQueryWithoutResponse(string query, List<SqlParameter> parameters = null)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            command.ExecuteNonQuery();
        }


        public static void CloseConnection()
        {
            sqlConnection.Close();
        }

    }
}
