using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace WSCData
{
    public class WSCContract
    {
        private const string MOD_NAME = "WSCContract.";
        private static string LF = System.Environment.NewLine;

        public static DataSet GetContracts(string shids, int cropYear, string connectionString)
        {
            var p = new List<SqlParameter>
            {
                new SqlParameter("CropYear", cropYear),
                new SqlParameter("Shids", shids)
            };

            return Utils.GetDataSet(p, "bawdContractBySHID", new SqlConnection(connectionString));
        }
    }
}