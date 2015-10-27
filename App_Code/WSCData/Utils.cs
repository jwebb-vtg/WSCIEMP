using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WSCData
{
    class Utils
    {
        public static DataSet GetDataSet(List<SqlParameter> parameters, string storedProcedure, SqlConnection connectionString)
        {
            var dataset = new DataSet();
            var cmd = new SqlCommand(storedProcedure, connectionString);

            foreach (var p in parameters)
                cmd.Parameters.Add(p);

            var da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;

            da.Fill(dataset);
            return dataset;
        }

        public static DataSet GetDataSet(string storedProcedure, string connectionString)
        {
            var dataset = new DataSet();
            var con = new SqlConnection(connectionString);
            var cmd = new SqlCommand(storedProcedure, con);

            var da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            da.Fill(dataset);
            return dataset;
        }

        public static string ExecuteScalar(List<SqlParameter> parameters, string storedProcedure, string connectionString)
        {
            var con = new SqlConnection(connectionString);
            var cmd = new SqlCommand(storedProcedure, con);

            foreach (var p in parameters)
                cmd.Parameters.Add(p);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            var s = cmd.ExecuteScalar();
            con.Close();

            return (s == null) ? "" : s.ToString();
        }
    }
}