using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MDAAB.Classic;

namespace WSCData {

    public class BeetEquityDeduction {

        private const string MOD_NAME = "WSCData.BeetEquityDeduction.";
        private static string LF = System.Environment.NewLine;

        public static List<ListEquityDeductionItem> EquityDeductionGetAll() {

            const string METHOD_NAME = "EquityDeductionGetAll";
            List<ListEquityDeductionItem> state = new List<ListEquityDeductionItem>();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEquityDeductionGetAll";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    //System.Data.SqlClient.SqlParameter[] spParams =
                    //    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    // Manually synch these fields with database.                    
                    //spParams[0].Value = shid;
                    //spParams[1].Value = cropYear;

                    int iededEquityDeductionID = 0, iededNumber = 1, iededDescription = 2, iededIsActive = 3, iededRowVersion = 4;
                    //using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {
                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName)) {
                        while (dr.Read()) {

                            if (iededEquityDeductionID == -1) {

                                iededEquityDeductionID = dr.GetOrdinal("ededEquityDeductionID");
                                iededNumber = dr.GetOrdinal("ededNumber");
                                iededDescription = dr.GetOrdinal("ededDescription");
                                iededIsActive = dr.GetOrdinal("ededIsActive");
                                iededRowVersion = dr.GetOrdinal("ededRowVersion");
                            }
                            state.Add(new ListEquityDeductionItem(dr.GetInt32(iededEquityDeductionID), dr.GetInt32(iededNumber), dr.GetString(iededDescription),
                                dr.GetBoolean(iededIsActive), dr.GetString(iededRowVersion)));
                        }
                    }

                    if (state.Count == 0) {
                        state.Add(new ListEquityDeductionItem(0, 0, "", false, ""));
                    }
                }

                return state;
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void EquityDeductionDelete(int equityDeductionID, string rowVersion) {

            const string METHOD_NAME = "EquityDeductionDelete";
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEquityDeductionDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = equityDeductionID;
                    spParams[1].Value = rowVersion;

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void EquityDeductionSave(int equityDeductionID, int deductionNumber, string deductionDescription, bool isActive, string rowVersion, string userName) {

            const string METHOD_NAME = "EquityDeductionSave";
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEquityDeductionSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = equityDeductionID;
                    spParams[1].Value = deductionNumber;
                    spParams[2].Value = deductionDescription;
                    spParams[3].Value = isActive;
                    spParams[4].Value = rowVersion;
                    spParams[5].Value = userName;

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static List<ListMemberEquityDeductionItem> EquityDeductionMemberGetBySHID(string shid, int cropYear) {

            const string METHOD_NAME = "EquityDeductionMemberGetBySHID";
            List<ListMemberEquityDeductionItem> state = new List<ListMemberEquityDeductionItem>();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEquityDeductionMemberGetBySHID";

                    if (shid.Length > 0 && shid != "0") {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams =
                            SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        // Manually synch these fields with database.                    
                        spParams[0].Value = shid;
                        spParams[1].Value = cropYear;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iEquityDeductionMemberID = dr.GetOrdinal("EquityDeductionMemberID");
                            int iEquityDeductionID = dr.GetOrdinal("EquityDeductionID");
                            int iDeductionDesc = dr.GetOrdinal("DeductionDesc");
                            int iEquityCropYear = dr.GetOrdinal("EquityCropYear");
                            int iEquityType = dr.GetOrdinal("EquityType");
                            int iPaySequence = dr.GetOrdinal("PaymentSequence");
                            int iPaymentDesc = dr.GetOrdinal("PaymentDesc");
                            int iDeductionAmount = dr.GetOrdinal("DeductionAmount");
                            int iRowVersion = dr.GetOrdinal("RowVersion");

                            while (dr.Read()) {

                                state.Add(new ListMemberEquityDeductionItem(dr.GetInt32(iEquityDeductionMemberID), dr.GetInt32(iEquityDeductionID),
                                    dr.GetString(iRowVersion), dr.GetInt32(iEquityCropYear), dr.GetString(iEquityType),
                                    dr.GetInt32(iPaySequence), dr.GetString(iPaymentDesc), dr.GetString(iDeductionDesc), dr.GetDecimal(iDeductionAmount)));
                            }
                        }
                    }

                    if (state.Count == 0) {
                        state.Add(new ListMemberEquityDeductionItem("", "", "", "*", "", "", "", "", ""));
                    }
                }

                return state;
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static List<ListEquityPaymentScheduleItem> EquityPaymentSchedule(int cropYear) {

            const string METHOD_NAME = "EquityPaymentSchedule";
            List<ListEquityPaymentScheduleItem> state = new List<ListEquityPaymentScheduleItem>();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEquityPaymentSchedule";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    // Manually synch these fields with database.                    
                    spParams[0].Value = cropYear;                    

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                        int igroupType = dr.GetOrdinal("GroupType");
                        int iequityType = dr.GetOrdinal("EquityType");
                        int ipayDate = dr.GetOrdinal("PayDate");

                        while (dr.Read()) {

                            state.Add(new ListEquityPaymentScheduleItem(0, dr.GetString(igroupType), dr.GetString(iequityType),
                                "", dr.GetString(ipayDate)));
                        }
                    }
                }

                return state;
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static List<ListMemberEquityPaymentItem> EquityPaymentsByShid(string shid, DateTime matchDate) {

            const string METHOD_NAME = "EquityPaymentsByShid";
            List<ListMemberEquityPaymentItem> state = new List<ListMemberEquityPaymentItem>();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEquityPaymentsByShid";

                    if (shid.Length > 0 && shid != "0") {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams =
                            SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        // Manually synch these fields with database.                    
                        spParams[0].Value = shid;
                        spParams[1].Value = matchDate;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int icropYear = dr.GetOrdinal("CropYear");
                            int iequityType = dr.GetOrdinal("EquityType");
                            int ipaymentDesc = dr.GetOrdinal("PaymentDesc");
                            int ipaymentAmount = dr.GetOrdinal("PayAmount");
                            int isequence = dr.GetOrdinal("Sequence");

                            while (dr.Read()) {

                                state.Add(new ListMemberEquityPaymentItem(dr.GetInt32(icropYear), dr.GetString(iequityType), dr.GetString(ipaymentDesc),
                                    dr.GetDecimal(ipaymentAmount), dr.GetInt32(isequence)));
                            }
                        }
                    }

                    if (state.Count == 0) {
                        state.Add(new ListMemberEquityPaymentItem(0, "", "", 0, 0));
                    }
                }

                return state;
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void EquityDeductionMemberSave(int equityDeductionMemberID, int memberID, int equityDeductionID, int equityCropYear, int cropYear,
            string equityType, int sequence, string paymentDesc, decimal deductionAmount, DateTime deductionDate, string rowVersion, string userName) {

            const string METHOD_NAME = "EquityDeductionMemberSave";
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEquityDeductionMemberSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = equityDeductionMemberID;
                    spParams[1].Value = memberID;
                    spParams[2].Value = equityDeductionID;
                    spParams[3].Value = equityCropYear;
                    spParams[4].Value = cropYear;
                    spParams[5].Value = equityType;
                    spParams[6].Value = sequence;
                    spParams[7].Value = paymentDesc;
                    spParams[8].Value = deductionAmount;
                    spParams[9].Value = deductionDate;
                    spParams[10].Value = rowVersion;
                    spParams[11].Value = userName;

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void EquityDeductionMemberDelete(int equityDeductionMemberID, string rowVersion) {

            const string METHOD_NAME = "EquityDeductionMemberDelete";
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpEquityDeductionMemberDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = equityDeductionMemberID;
                    spParams[1].Value = rowVersion;

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }
    }
}
