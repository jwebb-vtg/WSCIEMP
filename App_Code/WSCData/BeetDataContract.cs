using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MDAAB.Classic;

namespace WSCData {

    public class BeetDataContract {

        private const string MOD_NAME = "WSCData.BeetDataContract.";

        /*
         * Get List of contracts by type:
         *  0 - SHID
         *  1 - Business Name
         *  2 - Last Name
         *  3 - Contract Number
         */
        public static List<Contract> GetContracts(string searchTerm, int cropYear, int iType)
        {

            const string METHOD_NAME = "GetSHIDContracts";

            List<Contract> contracts = new List<Contract>();

            try {

                string procName = "s70cnt_Find";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = searchTerm;
                        spParams[1].Value = cropYear;
                        spParams[2].Value = iType;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iContractNo = dr.GetOrdinal("contractNo");

                            while (dr.Read()) {
                                int contractNo = Convert.ToInt32(dr.GetString(iContractNo));
                                contracts.Add(GetContract(contractNo, cropYear));
                            }
                        }
                    }
                }
                catch (SqlException sqlEx) {
                    if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                        WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                        throw (wscWarn);
                    } else {
                        throw (sqlEx);
                    }
                }
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return contracts;
        }

        public static Contract GetContract(int contractNo, int cropYear) {

            const string METHOD_NAME = "GetContract";

            Contract contract = null;

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "s70cnt_GetContract";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = contractNo;
                    spParams[1].Value = cropYear;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iContarctID = dr.GetOrdinal("cnt_contract_id");
                            int iCropYear = dr.GetOrdinal("cnt_crop_year");
                            int iContractNo = dr.GetOrdinal("cnt_contract_no");
                            int iFactoryId = dr.GetOrdinal("cnt_factory_id");
                            int iPACDues = dr.GetOrdinal("cnt_pack_dues");

                            while (dr.Read()) {
                                int contractId = dr.GetInt32(iContarctID);
                                int crop_year = Convert.ToInt32(dr.GetDateTime(iCropYear).Year);
                                int contract_no = Convert.ToInt32(dr.GetString(iContractNo));
                                int factory_id = dr.GetInt32(iFactoryId);
                                double pac_dues = Convert.ToDouble(dr.GetDecimal(iPACDues));
                                contract = new Contract(contractId, crop_year, contract_no, factory_id, pac_dues);
                            }
                        }
                    }
                    catch (SqlException sqlEx) {
                        if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                            WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                            throw (wscWarn);
                        } else {
                            string errMsg = MOD_NAME + METHOD_NAME;
                            WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                            throw (wscEx);
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return contract;
        }
    }
}
