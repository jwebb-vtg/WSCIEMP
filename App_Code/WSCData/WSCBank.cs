using System;
using System.Configuration;
using MDAAB.Classic;
using System.Data.SqlClient;
using System.Data;

namespace WSCData {
    /// <summary>
    /// Summary description for WSCBank.
    /// </summary>
    public class WSCBank {

        private const string MOD_NAME = "WSCData.";
        private static string LF = System.Environment.NewLine;

        public static SqlDataReader BankFindName(SqlConnection conn, string bankName, bool isActive) {

            SqlDataReader dr = null;

            try {

                string procName = "bawpBankFindByName";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = bankName;
                spParams[1].Value = isActive;
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
                        string errMsg = MOD_NAME + "BankFindName";
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + "BankFindName";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader BankFindNumber(SqlConnection conn, string bankNumber, bool isActive) {

            SqlDataReader dr = null;

            try {

                string procName = "bawpBankFindByNumber";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = bankNumber;
                spParams[1].Value = isActive;
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
                        string errMsg = MOD_NAME + "BankFindNumber";
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + "BankFindNumber";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }


        public static SqlDataReader BankGetByID(SqlConnection conn, int bankID) {

            SqlDataReader dr = null;

            try {

                string procName = "bawpBankGetByID";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = bankID;
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
                        string errMsg = MOD_NAME + "BankGetByID";
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + "BankGetByID";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader BankPayeeGetList(SqlConnection conn, int addressID, int cropYear) {

            SqlDataReader dr = null;

            try {

                string procName = "bawpBankPayeeGetList";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = addressID;
                spParams[1].Value = cropYear;
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
                        string errMsg = MOD_NAME + "BankPayeeGetList";
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + "BankPayeeGetList";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader BankEquityGetList(SqlConnection conn, int memberID, int cropYear) {

            SqlDataReader dr = null;

            try {

                string procName = "bawpBankEquityGetList";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = memberID;
                spParams[1].Value = cropYear;
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
                        string errMsg = MOD_NAME + "BankEquityGetList";
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + "BankEquityGetList";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static void BankSave(ref int bankID,
            string bankName, string bankNumber, string shortName, string addrLine1,
            string addrLine2, string city, string state, string zip, string contactName,
            string phone, string fax, string email, string other, bool isActive, string UserName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[1].Value = bankID;
                    spParams[2].Value = bankNumber;
                    spParams[3].Value = addrLine1;
                    spParams[4].Value = addrLine2;
                    spParams[5].Value = city;
                    spParams[6].Value = state;
                    spParams[7].Value = zip;
                    spParams[8].Value = contactName;
                    spParams[9].Value = phone;
                    spParams[10].Value = fax;
                    spParams[11].Value = email;
                    spParams[12].Value = other;
                    spParams[13].Value = bankName;
                    spParams[14].Value = shortName;
                    spParams[15].Value = isActive;
                    spParams[16].Value = UserName;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();

                            bankID = Convert.ToInt32(spParams[0].Value);
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                string errMsg = MOD_NAME + "BankSave";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankSave";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankPayeeSave(ref int bankPayeeID,
            int bankID, int payeeID, int sequenceNumber, bool subRecd, int cropYear, string UserName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankPayeeSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[1].Value = bankPayeeID;
                    spParams[2].Value = bankID;
                    spParams[3].Value = payeeID;
                    spParams[4].Value = sequenceNumber;
                    spParams[5].Value = subRecd;
                    spParams[6].Value = cropYear;
                    spParams[7].Value = UserName;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();

                            bankPayeeID = Convert.ToInt32(spParams[0].Value);
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                string errMsg = MOD_NAME + "BankPayeeSave";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankPayeeSave";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankEquitySave(ref int bankEquityLienID,
            int equityBankID, int memberID, int sequenceNumber,
            string equityDate, bool lienPatronShares,
            bool lienPatronage, bool lienRetains, bool releasePatronShares,
            bool releasePatronage, bool releaseRetains, int cropYear, string UserName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankEquitySave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[1].Value = bankEquityLienID;
                    spParams[2].Value = equityBankID;
                    spParams[3].Value = memberID;
                    spParams[4].Value = sequenceNumber;

                    if (equityDate != null && equityDate.Length > 0) {
                        spParams[5].Value = DateTime.Parse(equityDate);
                    } else {
                        spParams[5].Value = null;
                    }

                    spParams[6].Value = lienPatronShares;
                    spParams[7].Value = lienPatronage;
                    spParams[8].Value = lienRetains;
                    spParams[9].Value = releasePatronShares;
                    spParams[10].Value = releasePatronage;
                    spParams[11].Value = releaseRetains;
                    spParams[12].Value = cropYear;
                    spParams[13].Value = UserName;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();

                            bankEquityLienID = Convert.ToInt32(spParams[0].Value);
                        }
                        catch (SqlException sqlEx) {

                            if (tran != null) {
                                tran.Rollback();
                            }
                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                string errMsg = MOD_NAME + "BankEquitySave";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankEquitySave";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankEquitySaveOtherYear(int equityBankID, int memberID,
            string equityDate, bool lienPatronShares,
            bool lienPatronage, bool lienRetains, bool releasePatronShares,
            bool releasePatronage, bool releaseRetains, int otherCropYear,
            bool isInsert, string UserName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankEquitySaveOtherYear";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = equityBankID;
                    spParams[1].Value = memberID;
                    spParams[2].Value = equityDate;
                    spParams[3].Value = lienPatronShares;
                    spParams[4].Value = lienPatronage;
                    spParams[5].Value = lienRetains;
                    spParams[6].Value = releasePatronShares;
                    spParams[7].Value = releasePatronage;
                    spParams[8].Value = releaseRetains;
                    spParams[9].Value = otherCropYear;
                    spParams[10].Value = isInsert;
                    spParams[11].Value = UserName;
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
                                string errMsg = MOD_NAME + "BankEquitySaveOtherYear";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankEquitySaveOtherYear";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankPayeeSaveOtherYear(int bankID,
            int addressID, bool subRecd, int otherCropYear, bool isInsert, string UserName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankPayeeSaveOtherYear";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = bankID;
                    spParams[1].Value = addressID;
                    spParams[2].Value = subRecd;
                    spParams[3].Value = otherCropYear;
                    spParams[4].Value = isInsert;
                    spParams[5].Value = UserName;
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
                                string errMsg = MOD_NAME + "BankPayeeSaveOtherYear";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankPayeeSaveOtherYear";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankPayeeSetSub(int bankPayeeID, bool subRecd, string UserName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankPayeeSetSub";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = bankPayeeID;
                    spParams[1].Value = subRecd;
                    spParams[2].Value = UserName;
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
                                string errMsg = MOD_NAME + "BankPayeeSetSub";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankPayeeSetSub";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankDelete(int bankID, string UserName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = bankID;
                    spParams[1].Value = UserName;
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
                                string errMsg = MOD_NAME + "BankDelete";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankDelete";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankPayeeDelete(int bankPayeeID) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankPayeeDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = bankPayeeID;
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
                                string errMsg = MOD_NAME + "BankPayeeDelete";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankPayeeDelete";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankEquityDelete(int bankEquityLienID) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankEquityDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = bankEquityLienID;
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
                                string errMsg = MOD_NAME + "BankEquityDelete";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankEquityDelete";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankPayeeDeleteOtherYear(int bankID, int addressID, int cropYear) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankPayeeDeleteOtherYear";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = bankID;
                    spParams[1].Value = addressID;
                    spParams[2].Value = cropYear;
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
                                string errMsg = MOD_NAME + "BankPayeeDeleteOtherYear";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankPayeeDeleteOtherYear";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void BankEquityDeleteOtherYear(int bankID, int memberID, int cropYear) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpBankEquityDeleteOtherYear";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = bankID;
                    spParams[1].Value = memberID;
                    spParams[2].Value = cropYear;
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
                                string errMsg = MOD_NAME + "BankEquityDeleteOtherYear";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "BankEquityDeleteOtherYear";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        private static void SetTimeout() {

			int timeOut = Convert.ToInt32(ConfigurationManager.AppSettings["sql.command.timeout"].ToString());
            SqlHelper.CommandTimeout = timeOut.ToString();
        }

        private static void SetTimeout(int timeout) {

            int timeOut = timeout;
            SqlHelper.CommandTimeout = timeOut.ToString();
        }
    }
}
