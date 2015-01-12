using System;
using System.Configuration;
using System.Data;
using MDAAB.Classic;
using System.Data.SqlClient;

namespace WSCData {

    /// <summary>
    /// Summary description for WSCMember.
    /// </summary>
    public class WSCMember {

        private const string MOD_NAME = "WSCMember.";

        private static string LF = System.Environment.NewLine;

        public static void GetInfo(string shid, ref int memberID,
            ref int addressID, ref string busName, ref string phone, ref string email, ref string fax) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpMemberGetInfo";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = shid;
                    SetTimeout();

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);

                    if (spParams[1].Value == System.DBNull.Value) {
                        memberID = 0;
                    } else {
                        memberID = Convert.ToInt32(spParams[1].Value);
                    }
                    if (spParams[2].Value == System.DBNull.Value) {
                        addressID = 0;
                    } else {
                        addressID = Convert.ToInt32(spParams[2].Value);
                    }
                    if (spParams[3].Value == System.DBNull.Value) {
                        busName = "";
                    } else {
                        busName = spParams[3].Value.ToString();
                    }
                    if (spParams[4].Value == System.DBNull.Value) {
                        phone = "";
                    } else {
                        phone = spParams[4].Value.ToString();
                    }
                    if (spParams[5].Value == System.DBNull.Value) {
                        email = "";
                    } else {
                        email = spParams[5].Value.ToString();
                    }
                    if (spParams[6].Value == System.DBNull.Value) {
                        fax = "";
                    } else {
                        fax = spParams[6].Value.ToString();
                    }
                }
            }
            catch (SqlException sqlEx) {

                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    string errMsg = MOD_NAME + "GetInfo" + LF + "SHID: " + shid;
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                    throw (wscEx);
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + "GetInfo" + LF + "SHID: " + shid;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static SqlDataReader GetEmailByShid(SqlConnection conn, string shidList, int cropYear, bool SubYesActYes, bool SubYesActNo, 
            bool SubNoActYes, bool isInternalOnly, bool isExternalOnly) {

            SqlDataReader dr = null;

            try {

                string procName = "bawpAddressEmailGetByShid";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = shidList;
                spParams[1].Value = cropYear;
                spParams[2].Value = SubYesActYes;
                spParams[3].Value = SubYesActNo;
                spParams[4].Value = SubNoActYes;
                spParams[5].Value = isInternalOnly;
                spParams[6].Value = isExternalOnly;
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
                        string errMsg = MOD_NAME + "UserFindLogon";
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + "UserFindLogon";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static void GetAddress(int memberID,
            ref string email, ref string fax, ref string busName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserGetAddress";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = memberID;
                    SetTimeout();

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);

                    email = spParams[1].Value.ToString();
                    fax = spParams[2].Value.ToString();
                    busName = spParams[3].Value.ToString();
                }
            }
            catch (SqlException sqlEx) {

                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    string errMsg = MOD_NAME + "GetAddress";
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                    throw (wscEx);
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + "GetAddress";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void GetSendRptOption(int memberID,
            ref string sendRptOption) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserGetSendRptOption";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = memberID;
                    SetTimeout();

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);

                    sendRptOption = spParams[1].Value.ToString();
                }
            }
            catch (SqlException sqlEx) {

                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    string errMsg = MOD_NAME + "GetSendRptOption";
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                    throw (wscEx);
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + "GetSendRptOption";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static string GetFactoryNorthSouth(int addressID) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserGetFactoryNorthSouth";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = addressID;
                    SetTimeout();

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);

                    return spParams[1].Value.ToString();
                }
            }
            catch (SqlException sqlEx) {

                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    string errMsg = MOD_NAME + "GetFactoryNorthSouth";
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                    throw (wscEx);
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + "GetFactoryNorthSouth";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void UpdateAddress(int memberID, string email, string fax, string userName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserUpdateAddress";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = memberID;
                    spParams[1].Value = email;
                    spParams[2].Value = fax;
                    spParams[3].Value = userName;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {

                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                string errMsg = MOD_NAME + "UpdateAddress";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "UpdateAddress";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void UpdatePassword(int memberID, string oldPassword, string newPassword, ref bool isSuccess) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserUpdatePassword";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = memberID;
                    spParams[1].Value = oldPassword;
                    spParams[2].Value = newPassword;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {

                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            isSuccess = Convert.ToBoolean(spParams[3].Value);
                            tran.Commit();
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                string errMsg = "Error in " + MOD_NAME + "UpdatePassword" + LF +
                                    "Description: " + sqlEx.Message;
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {

                string errMsg = "Error in " + MOD_NAME + "UpdatePassword" + LF +
                    "Description: " + e.Message;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }


        public static void UpdateSendRptOption(int memberID, string sendRptOption) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserUpdateSendRptOption";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = memberID;
                    spParams[1].Value = sendRptOption;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                string errMsg = "Error in " + MOD_NAME + "UpdateSendRptOption" + LF +
                                    "Description: " + sqlEx.Message;
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {

                string errMsg = "Error in " + MOD_NAME + "UpdateSendRptOption" + LF +
                    "Description: " + e.Message;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        private static void SetTimeout() {

            int timeOut = Convert.ToInt32(ConfigurationManager.AppSettings["sql.command.timeout"].ToString());
            SqlHelper.CommandTimeout = timeOut.ToString();
        }

        public static void ResetPassword(string shid, string toAddress, ref string warnMsg) {

            string password = null;

            // Reset user's password using SHID
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserResetPassword";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = shid;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {

                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                string errMsg = "Error in " + MOD_NAME + "ResetPassword" + LF +
                                    "SHID: " + shid + LF +
                                    "To Address: " + toAddress;
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = "Error in " + MOD_NAME + "ResetPassword" + LF +
                    "SHID: " + shid + LF +
                    "To Address: " + toAddress;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            // Get password using SHID and Email Address			
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpUserGetPasswordByEmail";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = shid;
                    spParams[1].Value = toAddress;
                    SetTimeout();

                    try {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
                    }
                    catch (SqlException sqlEx) {

                        if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                            WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                            throw (wscWarn);
                        } else {
                            string errMsg = "Error in WSCMember.ResetPassword" + LF +
                                "SHID: " + shid + LF +
                                "To Address: " + toAddress;
                            WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                            throw (wscEx);
                        }
                    }

                    if (spParams[2].Value != DBNull.Value) {
                        password = spParams[2].Value.ToString();
                    }
                }
            }
            catch (System.Exception e) {

                string errMsg = "Error in WSCMember.ResetPassword" + LF +
                    "SHID: " + shid + LF +
                    "To Address: " + toAddress;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            if (password != null) {

                string subject = "Western Sugar Cooperative: User Information";
                string emailMessage = "Your new password is: " + password;

                try {

					WSCIEMP.Common.AppHelper.SendEmail(ConfigurationManager.AppSettings["email.smtpServer"].ToString(),
						ConfigurationManager.AppSettings["email.smtpServerPort"].ToString(), emailMessage,
                        ConfigurationManager.AppSettings["email.smtpUser"].ToString(),
                        ConfigurationManager.AppSettings["email.smtpPassword"].ToString(), toAddress,
                        ConfigurationManager.AppSettings["email.employeeServiceFrom"].ToString(),
                        "", 
                        subject, false);
                }
                catch (Exception MailEx) {
                    string errMsg = "Error in " + MOD_NAME + "ResetPassword" + LF +
                        "Subject: " + subject + LF +
                        "To Address: " + toAddress;
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, MailEx);
                    throw (wscEx);
                }

            } else {
                // Could not match shid and password to a user.
                warnMsg = "Please make sure this is the member's correct email address.";
            }
        }
    }
}
