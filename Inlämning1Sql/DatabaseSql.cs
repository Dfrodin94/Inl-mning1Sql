using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text;

namespace Inlämning1Sql
{
    class DatabaseSql
    {
        private string ConnectionString { get; set; } = @"Data Source=.\SQLExpress;Integrated Security=true;database={0}";
        private string DatabaseName { get; set; } = "";

        public DataTable GetDataTable(string sqlString, params(string,string) [] parameters)
        {
            var dt = new DataTable();
            var connString = string.Format(ConnectionString, DatabaseName);

            using (var cnn = new SqlConnection(connString))
            {
                cnn.Open();
                using(var command = new SqlCommand(sqlString,cnn))
                {
                    foreach(var item in parameters)
                    {
                        command.Parameters.AddWithValue(item.Item1, item.Item2);
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }

                }

            }
            return dt;
        }

        private static void SetParameters((string, string)[] parameters, SqlCommand command)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2);
            }
        }

    }
}
