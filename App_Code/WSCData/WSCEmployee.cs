using System;
using System.Configuration;
using MDAAB.Classic;
using System.Data.SqlClient;
using System.Data;

namespace WSCData {
    /// <summary>
    /// Summary description for WSCEmployee.
    /// </summary>
    public class WSCEmployee {

        private const string MOD_NAME = "WSCEmployee.";
        private static string LF = System.Environment.NewLine;
        
        public static SqlDataReader UserFindLogon(SqlConnection conn, string logonName) {

            const string METHOD_NAME = "UserFindLogon";
            SqlDataReader dr = null;

            try {

                string procName = "bawpUserFindLogon";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = logonName;
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                }
                catch (SqlException sqlEx) {
                    if (dr != null && !dr.IsClosed) {
                        dr.Close();
                    }
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
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader UserGetByID(SqlConnection conn, int userID) {

            const string METHOD_NAME = "UserGetByID";
            SqlDataReader dr = null;

            try {

                string procName = "bawpUserGetByID";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = userID;
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                }
                catch (SqlException sqlEx) {
                    if (dr != null && !dr.IsClosed) {
                        dr.Close();
                    }
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
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader UserFindDisplay(SqlConnection conn, string displayName) {

            const string METHOD_NAME = "UserFindDisplay";
            SqlDataReader dr = null;

            try {

                string procName = "bawpUserFindDisplay";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = displayName;
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                }
                catch (SqlException sqlEx) {
                    if (dr != null && !dr.IsClosed) {
                        dr.Close();
                    }
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
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static bool IsMemberAgriculturist(int userID, int shid, int cropYear) {

            const string METHOD_NAME = "IsMemberAgriculturist";
            string errMsg = "";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEmpIsMemAgriculturist";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = userID;
                    spParams[1].Value = shid;
                    spParams[2].Value = cropYear;
                    SetTimeout();

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);

                    if (spParams[3].Value == System.DBNull.Value) {
                        return false;
                    } else {
                        return Convert.ToBoolean(spParams[3].Value);
                    }
                }
            }
            catch (SqlException sqlEx) {

                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    errMsg = MOD_NAME + METHOD_NAME + LF +
                        "UserID: " + userID.ToString() + 
                        "SHID: " + shid.ToString() +
                        "CropYear: " + cropYear.ToString();
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                    throw (wscEx);
                }
            }
            catch (System.Exception e) {

                if (errMsg.Length == 0)  {

                    errMsg = MOD_NAME + METHOD_NAME + LF +
                        "UserID: " + userID.ToString() + 
                        "SHID: " + shid.ToString() +
                        "CropYear: " + cropYear.ToString();
                } else {errMsg = "";}

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void UserSave(ref int userID,
            string userName, string displayName, string phoneNumber, string UserName) {

            const string METHOD_NAME = "UserSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[1].Value = userID;
                    spParams[2].Value = userName;
                    spParams[3].Value = displayName;
                    spParams[4].Value = phoneNumber;
                    spParams[5].Value = UserName;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();

                            userID = Convert.ToInt32(spParams[0].Value);
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
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
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        private static void SetTimeout() {

            int timeOut = Convert.ToInt32(ConfigurationManager.AppSettings["sql.command.timeout"].ToString());
            SqlHelper.CommandTimeout = timeOut.ToString();
        }
    }
}
