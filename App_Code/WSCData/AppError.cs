using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MDAAB.Classic;

namespace WSCData {

    public class AppError {

        private const string MOD_NAME = "WSCData.Admin.";
        private static string LF = System.Environment.NewLine;

        public static void AppErrorFileSave(int appErrorInfoID, string appName, string severity, 
        DateTime errorDate, string errorStatus, string errorAction, string appLoginServer, string appLoginClient, string appLoginUser,
        string errorCode, string userName, string errorText) {

            const string METHOD_NAME = "AppErrorFileSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "srspAppErrorInfoWebSave";

                    switch (errorStatus) {
                        case "Closed":
                            errorStatus = "C";
                            break;
                        default:
                            errorStatus = "O";
                            break;

                    }

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    // This is an update, all fields except Status are ignored.
                    spParams[0].Value = appErrorInfoID;
                    spParams[1].Value = appName;
                    spParams[2].Value = severity;
                    spParams[3].Value = errorDate;
                    spParams[4].Value = errorStatus;
                    spParams[5].Value = errorAction;
                    spParams[6].Value = appLoginServer;
                    spParams[7].Value = appLoginClient;
                    spParams[8].Value = appLoginUser;
                    spParams[9].Value = errorCode;
                    spParams[10].Value = userName;
                    spParams[11].Value = errorText;

                    try {
                        using (SqlTransaction tran = conn.BeginTransaction()) {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
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
        }

        public static void AppErrorInfoCreate(int appErrorInfoID, string appName, DateTime errorDate,
            string serverName, string clientName, string loginName, string errorCode, string userName, string errorText) {

            const string METHOD_NAME = "AppErrorInfoCreate";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "srspAppErrorInfoSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = appErrorInfoID;
                    spParams[1].Value = appName;
                    spParams[2].Value = errorDate;
                    spParams[3].Value = "O";
                    spParams[4].Value = serverName;
                    spParams[5].Value = clientName;
                    spParams[6].Value = loginName;
                    spParams[7].Value = errorCode;
                    spParams[8].Value = userName;
                    spParams[9].Value = errorText;

                    try {
                        using (SqlTransaction tran = conn.BeginTransaction()) {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
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
        }

        public static void AppErrorFileDelete(int appErrorInfoID) {

            const string METHOD_NAME = "AppErrorFileDelete";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "srspAppErrorInfoDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = appErrorInfoID;

                    try {
                        using (SqlTransaction tran = conn.BeginTransaction()) {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
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
        }

        public static string AppErrorFileGetDbDetail(int appErrorInfoID) {

            const string METHOD_NAME = "AppErrorFileGetDbDetail";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "srspAppErrorInfoGetDetail";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = appErrorInfoID;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            if (dr.Read()) {
                                return dr.GetString(dr.GetOrdinal("aeiErrorText"));
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
                throw (e);
            }

            return "";
        }

        public static List<ListAppErrorItem> AppErrorFileGet(string status, DateTime fromDate, DateTime toDate) {

            const string METHOD_NAME = "AppErrorFileGet";

            string searchFolderPath = "";
            string errMsg = "";
            List<ListAppErrorItem> state = new List<ListAppErrorItem>();

            try {

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        string procName = "srspAppErrorInfoGet";

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams =
                            SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        switch (status) {
                            case "Open":
                                status = "O";
                                break;
                            case "Closed":
                                status = "C";
                                break;
                        }

                        spParams[0].Value = "";
                        spParams[1].Value = status;
                        spParams[2].Value = fromDate;
                        spParams[3].Value = toDate;

                        try {

                            // For iErrorDate use the updateDate so we can show the time as well as the date.
                            int iAppErrorInfoID = 0, iAppName = 1, iErrorDate = 2, iStatus = 3;
                            int iLoginServer = 5, iLoginClient = 6, iLoginUser = 7, iErrorCode = 8, iAction = 4, iSeverity = 12;

                            using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                                while (dr.Read()) {

                                    state.Add(new ListAppErrorItem(dr.GetInt32(iAppErrorInfoID), dr.GetString(iAppName),
                                        dr.GetDateTime(iErrorDate), dr.GetString(iStatus),
                                        dr.GetString(iAction), dr.GetString(iLoginServer), dr.GetString(iLoginClient), dr.GetString(iLoginUser),
                                        dr.GetString(iErrorCode), dr.GetString(iSeverity)));
                                }
                            }
                        }
                        catch (SqlException sqlEx) {
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                errMsg = MOD_NAME + METHOD_NAME;
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
                catch (System.Exception e) {
                    if (errMsg.Length == 0) {
                        errMsg = MOD_NAME + METHOD_NAME;
                        WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(errMsg, e);
                        throw (wex);
                    } else {
                        throw (e);
                    }
                }

                string[] errorFiles;

                // Pickup file based errors next
                searchFolderPath = WSCIEMP.Common.AppHelper.AppPath();
                if (searchFolderPath.Length > 0) {
                    errorFiles = System.IO.Directory.GetFiles(searchFolderPath, "*.txt");
                    if (errorFiles.Length > 0) {
                        foreach (string s in errorFiles) {
                            if (s.Length > 8 && s.Contains("Errors")) {
                                state.Add(new ListAppErrorItem(s));
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {

                string ident = WSCIEMP.Common.AppHelper.GetIdentityName();
                errMsg = MOD_NAME + METHOD_NAME + "; Using identity, " + ident + ", to search for folder path: " + searchFolderPath;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return state;
        }
    }
}
