using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MDAAB.Classic;

namespace WSCData {

    public class BeetDataOverPlant {

        private const string MOD_NAME = "WSCData.BeetDataOverPlant.";

        public static List<ListOverPlantFactoryItem> OverPlantFactoryByYear(int cropYear) {

            const string METHOD_NAME = "OverPlantFactoryByYear";

            List<ListOverPlantFactoryItem> state = new List<ListOverPlantFactoryItem>();

            try {

                string procName = "bawpOverPlantFactoryByYear";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = cropYear;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iftyNum = dr.GetOrdinal("fty_factory_no");
                            int iftyName = dr.GetOrdinal("fty_name");
                            int iisOverPlantAllow = dr.GetOrdinal("IsOverPlantAllowed");
                            int ipct = dr.GetOrdinal("PctOverPlant");
                            int iisPoolAllowed = dr.GetOrdinal("IsPoolAllowed");
                            int ipoolShid = dr.GetOrdinal("PoolingSHID");
                            int ipoolCutoffDate = dr.GetOrdinal("PoolCutoffDate");
                            int iisPosted = dr.GetOrdinal("IsPosted");

                            while (dr.Read()) {

                                state.Add(new ListOverPlantFactoryItem(dr.GetString(iftyNum), dr.GetString(iftyName), dr.GetBoolean(iisOverPlantAllow),
                                    dr.GetDecimal(ipct), dr.GetBoolean(iisPoolAllowed), dr.GetString(ipoolShid), dr.GetString(ipoolCutoffDate), dr.GetBoolean(iisPosted)));
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

            return state;
        }

        public static List<ListOverPlantMemberItem> OverPlantMemberGetInfo(int memberID, int cropYear, string userNname) {

            const string METHOD_NAME = "OverPlantMemberGetInfo";

            List<ListOverPlantMemberItem> state = new List<ListOverPlantMemberItem>();

            try {

                string procName = "bawpOverPlantMemberGetInfo";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = memberID;
                        spParams[1].Value = cropYear;
                        spParams[2].Value = userNname;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iOverPlantID = dr.GetOrdinal("OverPlantID");
                            int iMemberID = dr.GetOrdinal("MemberID");
                            int iShid = dr.GetOrdinal("SHID");
                            int iOverPlantAccept = dr.GetOrdinal("OverPlantAccept");
                            int iOverPlantUsed = dr.GetOrdinal("OverPlantUsed");
                            int iIsFormReceived = dr.GetOrdinal("IsFormReceived");
                            int iPatronSharesOwned = dr.GetOrdinal("PatronSharesOwned");
                            int iOverPlantPct = dr.GetOrdinal("OverPlantPct");
                            int iOverPlantPossible = dr.GetOrdinal("OverPlantPossible");
                            int iHomeFactoryNumber = dr.GetOrdinal("HomeFactoryNumber");
                            int iHomeFactoryName = dr.GetOrdinal("HomeFactoryName");
                            int iOverPlantFactoryNumber = dr.GetOrdinal("OverPlantFactoryNumber");
                            int iOverPlantFactoryName = dr.GetOrdinal("OverPlantFactoryName");
                            int iIsOverridePct = dr.GetOrdinal("IsOverridePct");
                            int iIsOverPlantAllowed = dr.GetOrdinal("IsOverPlantAllowed");

                            while (dr.Read()) {

                                state.Add(new ListOverPlantMemberItem(dr.GetInt32(iOverPlantID), dr.GetInt32(iMemberID),
                                    dr.GetString(iShid), dr.GetString(iOverPlantAccept), dr.GetInt32(iOverPlantUsed),
                                    dr.GetBoolean(iIsFormReceived), dr.GetInt32(iPatronSharesOwned), dr.GetDecimal(iOverPlantPct), dr.GetInt32(iOverPlantPossible), 
                                    dr.GetString(iHomeFactoryNumber), dr.GetString(iHomeFactoryName), dr.GetString(iOverPlantFactoryNumber), dr.GetString(iOverPlantFactoryName),
                                    dr.GetBoolean(iIsOverridePct), dr.GetBoolean(iIsOverPlantAllowed)));
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

            return state;
        }

        public static void OverPlantPost(int cropYear, int factoryNumber, string userName) {

            const string METHOD_NAME = "OverPlantPost";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpOverPlantFactoryPost";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = factoryNumber;
                    spParams[2].Value = userName;

                    try {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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
            }
            catch (System.Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void OverPlantFactorySave(int cropYear, int factoryNumber, bool isOverPlantAllowed, decimal overPlantPct, bool isPoolingAllowed, int poolMemberSHID, 
            string poolCutoffDate, string userName) {

            const string METHOD_NAME = "OverPlantFactorySave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpOverPlantFactorySave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = factoryNumber;
                    spParams[2].Value = isOverPlantAllowed;
                    spParams[3].Value = overPlantPct;
                    spParams[4].Value = isPoolingAllowed;
                    spParams[5].Value = poolMemberSHID;
                    if (poolCutoffDate.Length > 0) {
                        spParams[6].Value = poolCutoffDate;
                    } else {
                        spParams[6].Value = DBNull.Value;
                    }                    
                    spParams[7].Value = userName;

                    try {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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
            }
            catch (System.Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void OverPlantSave(int overPlantID, int memberID, int cropYear, int overPlantUsed, string overPlantAccept, bool isFormReceived, 
            bool isOverridePct, decimal opMemberPct, int opFactoryNumber, string userName) {

            const string METHOD_NAME = "OverPlantSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpOverPlantSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = overPlantID;
                    spParams[1].Value = memberID;
                    spParams[2].Value = cropYear;
                    spParams[3].Value = overPlantUsed;
                    spParams[4].Value = overPlantAccept;
                    spParams[5].Value = isFormReceived;
                    spParams[6].Value = isOverridePct;
                    spParams[7].Value = opMemberPct;
                    spParams[8].Value = opFactoryNumber;
                    spParams[9].Value = userName;

                    try {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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
            }
            catch (System.Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static List<ListOverPlantFactoryItem> OverPlantFactoryByNumber(int cropYear, int factoryNumber) {

            const string METHOD_NAME = "OverPlantFactoryByNumber";

            List<ListOverPlantFactoryItem> state = new List<ListOverPlantFactoryItem>();

            try {

                string procName = "bawpOverPlantFactoryByNumber";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = cropYear;
                        spParams[1].Value = factoryNumber;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iftyNum = dr.GetOrdinal("FactoryNumber");
                            int iftyName = dr.GetOrdinal("FactoryName");
                            int iisOverPlantAllow = dr.GetOrdinal("IsOverPlantAllowed");
                            int ipct = dr.GetOrdinal("OverPlantPct");
                            int iisPoolAllowed = dr.GetOrdinal("IsPoolAllowed");
                            int ipoolCutoffDate = dr.GetOrdinal("PoolCutoffDate");

                            if (dr.Read()) {

                                state.Add(new ListOverPlantFactoryItem(dr.GetInt32(iftyNum).ToString(), dr.GetString(iftyName), dr.GetBoolean(iisOverPlantAllow),
                                    dr.GetDecimal(ipct), dr.GetBoolean(iisPoolAllowed), "", dr.GetString(ipoolCutoffDate), false));
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

            return state;
        }
    }
}
