using System;
using System.Configuration;
using MDAAB.Classic;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace WSCData {

    public class WSCReportsExec {        

        private const string MOD_NAME = "WSCData.WSCReportsExec.";
        private static string LF = System.Environment.NewLine;

        public static string GetReportLogo() {
            return "~/img/WSCLogo1.jpg";
        }

        public static string GetReportLogoIconOnly() {
            return "~/img/WSCLogo2010.gif";
        }

        public static string GetPDFFolderPath() {
            return "~/PDF";
        }

        public static List<ListBeetContractIDItem> ContractListByStations(string stationIDList) {

            const string METHOD_NAME = "ContractListByStations";
            List<ListBeetContractIDItem> state = new List<ListBeetContractIDItem>();

            try {

                string procName = "bawpContractsByContractStation";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = stationIDList;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iCntID = dr.GetOrdinal("cnt_contract_id");
                            int iCntNum = dr.GetOrdinal("cnt_contract_no");

                            while (dr.Read()) {
                                state.Add(new ListBeetContractIDItem(dr.GetInt32(iCntID), dr.GetString(iCntNum)));
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
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return state;
        }

        public static SqlDataReader SHIDsByContractStation(SqlConnection conn, string stationIDList) {

            const string METHOD_NAME = "SHIDsByContractStation";
            SqlDataReader dr = null;

            try {

                string procName = "bawpSHIDsByContractStation";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
               SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = stationIDList;
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

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static List<ListBeetContractIDItem> ContractsByContractStationNo(int cropYear, string stationIDList) {

            const string METHOD_NAME = "ContractsByContractStationNo";
            List<ListBeetContractIDItem> state = new List<ListBeetContractIDItem>();

            try {

                string procName = "bawpContractsByContractStationNo";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = stationIDList;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iCntID = dr.GetOrdinal("cnt_contract_id");
                            int iCntNum = dr.GetOrdinal("cnt_contract_no");

                            while (dr.Read()) {
                                state.Add(new ListBeetContractIDItem(dr.GetInt32(iCntID), dr.GetString(iCntNum)));
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
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return state;
        }

        public static SqlDataReader ContractsByPayeeNumber(SqlConnection conn, int cropYear,
            int shid) {

            const string METHOD_NAME = "ContractsByPayeeNumber";
            SqlDataReader dr = null;

            try {

                string procName = "bawpPayeeGetContracts";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
               SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = shid;
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
                        string errMsg = MOD_NAME + METHOD_NAME;
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader ContractsByDeliveryStation(SqlConnection conn, string stationIDList) {

            const string METHOD_NAME = "ContractsByDeliveryStation";
            SqlDataReader dr = null;

            try {

                string procName = "bawpContractsByDeliveryStation";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
               SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = stationIDList;
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

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static List<ListBeetStationIDItem> StationListGetByFactory(string factoryIDList) {

            const string METHOD_NAME = "StationListGetByFactory";
            List<ListBeetStationIDItem> state = new List<ListBeetStationIDItem>();

            try {

                string procName = "bawpStationGetByFactory";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = factoryIDList;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iStaID = dr.GetOrdinal("sta_station_id");
                            int iStaName = dr.GetOrdinal("sta_name");
                            int iStaNum = dr.GetOrdinal("sta_station_no");
                            int iStaNumName = dr.GetOrdinal("station_no_name");

                            while (dr.Read()) {
                                state.Add(new ListBeetStationIDItem(dr.GetInt32(iStaID), Convert.ToInt32(dr.GetInt16(iStaNum)), dr.GetString(iStaName), dr.GetString(iStaNumName)));
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
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return state;
        }

        public static List<ListBeetStationIDItem> StationGetByFactoryNo(int cropYear, string factoryNumList) {

            const string METHOD_NAME = "StationGetByFactoryNo";
            List<ListBeetStationIDItem> state = new List<ListBeetStationIDItem>();

            try {

                string procName = "bawpStationGetByFactoryNo";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = factoryNumList;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iStaID = dr.GetOrdinal("sta_station_id");
                            int iStaName = dr.GetOrdinal("sta_name");
                            int iStaNum = dr.GetOrdinal("sta_station_no");
                            int iStaNumName = dr.GetOrdinal("station_no_name");

                            while (dr.Read()) {
                                state.Add(new ListBeetStationIDItem(dr.GetInt32(iStaID), Convert.ToInt32(dr.GetInt16(iStaNum)), dr.GetString(iStaName), dr.GetString(iStaNumName)));
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
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return state;
        }

        public static SqlDataReader GrowerDetailReportByDelivery(SqlConnection conn, int contractNo, int cropYear,
            string dates) {

            const string METHOD_NAME = "GrowerDetailReportByDelivery";
            SqlDataReader dr = null;

            try {

                string procName = "s70_RPT_GrowerDetailReportByDelivery2";

                if (conn.State != ConnectionState.Open) { conn.Open(); }
                SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = dates;
                spParams[2].Value = contractNo;
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

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static List<ListGrowerTareItem> GrowerDetailReportMasterHdr(int cropYear, DateTime fromDate, DateTime toDate, 
            string factoryList, string stationList, string contractList, bool isPosted, bool isHardCopy, bool isEmail, bool isFax) {

            const string METHOD_NAME = "GrowerDetailReportMasterHdr";

            List<ListGrowerTareItem> stateList = new List<ListGrowerTareItem>();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName;
                    if (isPosted) {
                        procName = "bawpRptGrowerDetailHdrPosted";
                    } else {
                        procName = "bawpRptGrowerDetailHdr";
                    }

                    if (conn.State != ConnectionState.Open) {
                        conn.Open();
                    }
                    SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams.Single(p => p.ParameterName == "@CropYear").Value = cropYear;
                    spParams.Single(p => p.ParameterName == "@fromDate").Value = fromDate;
                    spParams.Single(p => p.ParameterName == "@toDate").Value = toDate;
                    spParams.Single(p => p.ParameterName == "@FactoryList").Value = factoryList;
                    spParams.Single(p => p.ParameterName == "@StationList").Value = stationList;
                    spParams.Single(p => p.ParameterName == "@ContractList").Value = contractList;
                    spParams.Single(p => p.ParameterName == "@isWebHCOnly").Value = isHardCopy;
                    spParams.Single(p => p.ParameterName == "@isWebEmailOnly").Value = isEmail;
                    spParams.Single(p => p.ParameterName == "@isWebFaxOnly").Value = isFax;

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                        int iDeliveryID = dr.GetOrdinal("DeliveryID"), iContractID = dr.GetOrdinal("ContractID"), iContract_No = dr.GetOrdinal("Contract_No");
                        int iDelivery_Date = dr.GetOrdinal("Delivery_Date"), iDelivery_Station_ID = dr.GetOrdinal("Delivery_Station_ID");
                        int iDelivery_Station_No = dr.GetOrdinal("Delivery_Station_No"), iDelivery_Station_Name = dr.GetOrdinal("Delivery_Station_Name");
                        int iDelivery_Factory_No = dr.GetOrdinal("Delivery_Factory_No"), iFirst_Net_Pounds = dr.GetOrdinal("First_Net_Pounds");
                        int iTare_Pounds = dr.GetOrdinal("Tare_Pounds"), iFinal_Net_Pounds = dr.GetOrdinal("Final_Net_Pounds");
                        int iTare = dr.GetOrdinal("Tare"), iSugar_Content = dr.GetOrdinal("Sugar_Content"), iSLM_Pct = dr.GetOrdinal("SLM_Pct");
                        int iExSugarPerTon = dr.GetOrdinal("ExSugarPerTon"), iShowTare = dr.GetOrdinal("SHOW_TARE"), iRptType = dr.GetOrdinal("RptType");

                        while (dr.Read()) {

                            stateList.Add(new ListGrowerTareItem(dr.GetInt32(iDeliveryID), dr.GetInt32(iContractID), dr.GetInt32(iContract_No), 
                                dr.GetString(iDelivery_Date), dr.GetInt32(iDelivery_Station_ID), dr.GetString(iDelivery_Station_No), 
                                dr.GetString(iDelivery_Station_Name), dr.GetString(iDelivery_Factory_No), dr.GetInt32(iFirst_Net_Pounds), 
                                dr.GetInt32(iTare_Pounds), dr.GetInt32(iFinal_Net_Pounds), dr.GetDecimal(iTare), dr.GetDecimal(iSugar_Content), 
                                dr.GetDecimal(iSLM_Pct), dr.GetInt32(iExSugarPerTon), dr.GetBoolean(iShowTare), dr.GetString(iRptType) ));
                        }
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return stateList;
        }

        public static SqlDataReader GrowerDetailReportTares(SqlConnection conn, int contractID,
            string deliveryDate) {

            const string METHOD_NAME = "GrowerDetailReportTares";
            SqlDataReader dr = null;

            try {

                string procName = "s70_RPT_GrowerDetailReportTares2";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = contractID;
                spParams[1].Value = deliveryDate;
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

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader GrowerDetailReportASH(SqlConnection conn, int contractID, int stationID,
            string deliveryDate, ref SqlParameter[] spParams) {

            const string METHOD_NAME = "GrowerDetailReportASH";
            SqlDataReader dr = null;

            try {

                string procName = "s70_RPT_GrowerDetailReportASH2";

                spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = contractID;
                spParams[1].Value = stationID;
                spParams[2].Value = deliveryDate;
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

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader GrowerDetailReportAddr(SqlConnection conn, int contractID) {

            const string METHOD_NAME = "GrowerDetailReportAddr";
            SqlDataReader dr = null;

            try {

                string procName = "s70_RPT_GrowerDetailReportAddr";

                if (conn.State != ConnectionState.Open) { conn.Open(); }
                    SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = contractID;
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

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader RptContractDeliverySummary(SqlConnection conn, int memberID, int cropYear) {

            const string METHOD_NAME = "RptContractDeliverySummary";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRptContractDeliverySummary";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
               SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

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
                        string errMsg = MOD_NAME + METHOD_NAME;
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static List<ListDeliveryDateItem> GetDeliveryDays(int memberID, int cropYear) {

            const string METHOD_NAME = "GetDeliveryDays";
            List<ListDeliveryDateItem> stateList = new List<ListDeliveryDateItem>();

            try {

                string procName = "bawpDeliveryDaysByMember";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = memberID;
                    spParams[1].Value = cropYear;

                    try {
                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {
                            while (dr.Read()) {
                                stateList.Add(new ListDeliveryDateItem(dr.GetString(0)));
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

            return stateList;
        }

        public static List<ListBeetContractDeliveryDateItem> GetContractDeliveryDates(int memberID, int cropYear) {

            const string METHOD_NAME = "GetContractDeliveryDates";

            List<ListBeetContractDeliveryDateItem> stateList = new List<ListBeetContractDeliveryDateItem>();
            try {

                string procName = "bawpContractDeliveryDates";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != ConnectionState.Open) { conn.Open(); }
                    SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = memberID;
                    spParams[1].Value = cropYear;
                    SetTimeout();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                        int iContractNumber = dr.GetOrdinal("ContractNo");
                        int iDeliveryDate = dr.GetOrdinal("DeliveryDate");

                        while (dr.Read()) {
                            stateList.Add(new ListBeetContractDeliveryDateItem(dr.GetString(iContractNumber), dr.GetString(iDeliveryDate)));
                        }
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
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return stateList;
        }


        public static List<ContractGorwerLandownerItem> ContractDeliverySummary1(int cropYear, int contractNo) {

            const string METHOD_NAME = "ContractDeliverySummary1";

			List<ContractGorwerLandownerItem> stateList = new List<ContractGorwerLandownerItem>();

            try {

                string procName = "s70_RPT_ContractDeliverySummary1";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = cropYear;
					spParams[1].Value = contractNo;
					SetTimeout();

					try {

						using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

							int iFactory_Number = dr.GetOrdinal("Factory_Number"),
							iFactory_Name = dr.GetOrdinal("Factory_Name"),
							iStation_Name = dr.GetOrdinal("Station_Name"),
							iStation_Number = dr.GetOrdinal("Station_Number"),
							iContract_Number = dr.GetOrdinal("Contract_Number"),
							iCrop_Year = dr.GetOrdinal("Crop_Year"),
							iLdo_Address_Number = dr.GetOrdinal("Ldo_Address_Number"),
							iLandowner_Name = dr.GetOrdinal("Landowner_Name"),
							iLdo_Address_1 = dr.GetOrdinal("Ldo_Address_1"),
							iLdo_Address_2 = dr.GetOrdinal("Ldo_Address_2"),
							iLdo_City = dr.GetOrdinal("Ldo_City"),
							iLdo_State = dr.GetOrdinal("Ldo_State"),
							iLdo_Zip = dr.GetOrdinal("Ldo_Zip"),
							iGro_Address_Number = dr.GetOrdinal("Gro_Address_Number"),
							iGrower_Name = dr.GetOrdinal("Grower_Name"),
							iGro_Address_1 = dr.GetOrdinal("Gro_Address_1"),
							iGro_Address_2 = dr.GetOrdinal("Gro_Address_2"),
							iGrower_City = dr.GetOrdinal("Grower_City"),
							iGrower_State = dr.GetOrdinal("Grower_State"),
							iGrower_Zip = dr.GetOrdinal("Grower_Zip"),
							iLegal_State = dr.GetOrdinal("Legal_State"),
							iLegal_County = dr.GetOrdinal("Legal_County"),
							iContract_Acres = dr.GetOrdinal("Contract_Acres"),
							iPlanted_Acres = dr.GetOrdinal("Planted_Acres"),
							iHarvest_Acres = dr.GetOrdinal("Harvest_Acres"),
							iAssoc_Member = dr.GetOrdinal("Assoc_Member"),
							iASCS_Signed = dr.GetOrdinal("ASCS_Signed"),
							iCash_Rent = dr.GetOrdinal("Cash_Rent"),
							iPac_Dues = dr.GetOrdinal("Pac_Dues"),
							icnt_contract_id = dr.GetOrdinal("cnt_contract_id"),
							iLegal_Seq_Number = dr.GetOrdinal("Legal_Seq_Number"),
							iGrower_Seq_Number = dr.GetOrdinal("Grower_Seq_Number"),
							iLandowner_Seq_Number = dr.GetOrdinal("Landowner_Seq_Number"),
							iPayee_Percent = dr.GetOrdinal("Payee_Percent"),
							iPayee_Name = dr.GetOrdinal("Payee_Name"),
							iPayee_Addr_No = dr.GetOrdinal("Payee_Addr_No"),
							iPayee_Sequence_Number = dr.GetOrdinal("Payee_Sequence_Number");

							while (dr.Read()) {

								stateList.Add (
									new ContractGorwerLandownerItem(
										dr.GetString(iFactory_Number),
										dr.GetString(iFactory_Name),
										dr.GetString(iStation_Name),
										dr.GetString(iStation_Number),
										dr.GetString(iContract_Number),
										dr.GetDateTime(iCrop_Year),
										dr.GetString(iLdo_Address_Number),
										dr.GetString(iLandowner_Name),
										dr.GetString(iLdo_Address_1),
										dr.GetString(iLdo_Address_2),
										dr.GetString(iLdo_City),
										dr.GetString(iLdo_State),
										dr.GetString(iLdo_Zip),
										dr.GetString(iGro_Address_Number),
										dr.GetString(iGrower_Name),
										dr.GetString(iGro_Address_1),
										dr.GetString(iGro_Address_2),
										dr.GetString(iGrower_City),
										dr.GetString(iGrower_State),
										dr.GetString(iGrower_Zip),
										dr.GetString(iLegal_State),
										dr.GetString(iLegal_County),
										dr.GetInt16(iContract_Acres),
										dr.GetInt16(iPlanted_Acres),
										dr.GetInt16(iHarvest_Acres),
										dr.GetBoolean(iAssoc_Member),
										dr.GetBoolean(iASCS_Signed),
										dr.GetBoolean(iCash_Rent),
										dr.GetDecimal(iPac_Dues),
										dr.GetInt32(icnt_contract_id),
										dr.GetString(iLegal_Seq_Number),
										dr.GetString(iGrower_Seq_Number),
										dr.GetString(iLandowner_Seq_Number),
										dr.GetFloat(iPayee_Percent),
										dr.GetString(iPayee_Name),
										dr.GetString(iPayee_Addr_No),
										dr.GetString(iPayee_Sequence_Number)								
									)
								);
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

            return stateList;
        }

        public static List<ContractDeliverySummary2Item> ContractDeliverySummary2(int contractID) {

            const string METHOD_NAME = "ContractDeliverySummary2";
            List<ContractDeliverySummary2Item> stateList = new List<ContractDeliverySummary2Item>();

            try {

                string procName = "s70_RPT_ContractDeliverySummary2";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
				   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = contractID;
					SetTimeout();

					try {
						using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

							while (dr.Read()) {

								stateList.Add(
									new ContractDeliverySummary2Item(
										dr.GetDecimal(dr.GetOrdinal("ContractTons")),
										dr.GetDecimal(dr.GetOrdinal("SugarPct")),
										dr.GetDecimal(dr.GetOrdinal("SLMPct")),
										dr.GetDecimal(dr.GetOrdinal("ExtractableSugarPerTon"))
									)
								);
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

            return stateList;
        }

        public static SqlDataReader StationGetExtractSugarAvg(int stationNo, int cropYear) {

            const string METHOD_NAME = "StationGetExtractSugarAvg";
            SqlDataReader dr = null;

            try {

                string procName = "s70sta_GetExtractSugarAvg";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = stationNo;
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

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader FactoryGetExtractSugarAvg(int factoryNo, int cropYear) {

            const string METHOD_NAME = "FactoryGetExtractSugarAvg";
            SqlDataReader dr = null;

            try {

                string procName = "s70fty_GetExtractSugarAvg";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = factoryNo;
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

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

		public static void FactoryStationGetExtractSugarAvg(int factoryNo, int stationNo, int cropYear,
			out int factoryExtSugarAvg, out int stationExtSugarAvg) {

			const string METHOD_NAME = "FactoryStationGetExtractSugarAvg";

			try {

				string procName = "bawpFactoryStationExtractSugarAvg";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = factoryNo;
					spParams[1].Value = stationNo;
					spParams[2].Value = cropYear;
					SetTimeout();

					try {

						SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
						if (spParams[3].Value == DBNull.Value) {
							factoryExtSugarAvg = 0;
						} else {
							factoryExtSugarAvg = Convert.ToInt32(spParams[3].Value);
						}

						if (spParams[4].Value == DBNull.Value) {
							stationExtSugarAvg = 0;
						} else {
							stationExtSugarAvg = Convert.ToInt32(spParams[4].Value);
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

        public static List<ListBeetContractIDItem> GetContractListSecure(int memberID, int cropYear, int userID) {

            const string METHOD_NAME = "GetContractListSecure";
            List<ListBeetContractIDItem> stateList = new List<ListBeetContractIDItem>();

            try {

                string procName = "bawpContractsByMemberSecure";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = memberID;
                    spParams[1].Value = cropYear;
                    spParams[2].Value = userID;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iCntID = dr.GetOrdinal("ContractID");
                            int iCntNum = dr.GetOrdinal("ContractNo");

                            while (dr.Read()) {
                                stateList.Add(new ListBeetContractIDItem(dr.GetInt32(iCntID), dr.GetString(iCntNum)));
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

            return stateList;
        }

        public static SqlDataReader GetContractList(SqlConnection conn, int memberID, int cropYear) {

            const string METHOD_NAME = "GetContractList";
            SqlDataReader dr = null;

            try {

                string procName = "bawpContractsByMember";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
               SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

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
                        string errMsg = MOD_NAME + METHOD_NAME;
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader RptContractDeliveryByDay(SqlConnection conn, int memberID, int cropYear, string deliveryDate) {

            const string METHOD_NAME = "RptContractDeliveryByDay";
            SqlDataReader dr = null;

            string procName = null;
            try {

                procName = "bawpRptContractDeliveryByDay";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
               SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = memberID;
                spParams[1].Value = cropYear;
                spParams[2].Value = deliveryDate;
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
                        WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException("Error calling " + procName + " memberID=" + memberID.ToString() + ", " +
                            "cropYear=" + cropYear.ToString() + ", " +
                            "deliveryDate=" + deliveryDate, sqlEx);
                        throw (wex);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }

                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + ": Error calling " + procName + " memberID=" + memberID.ToString() + ", " +
                    "cropYear=" + cropYear.ToString() + ", " +
                    "deliveryDate=" + deliveryDate, e);
                throw (e);
            }

            return dr;
        }

		public static List<ListNoticeOfPassthrough> PassthroughGetBySHID(int cropYear, string shid, string fromShid, string toShid) {

			const string METHOD_NAME = "PassthroughGetBySHID";
			List<ListNoticeOfPassthrough> stateList = new List<ListNoticeOfPassthrough>();

			string procName = null;

			try {

				procName = "bawpPassthroughGetBySHID";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);
				
					spParams[0].Value = cropYear;
					spParams[1].Value = shid;
					spParams[2].Value = fromShid;
					spParams[3].Value = toShid;

					try {
						using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

							int iSHID = dr.GetOrdinal("SHID"), 
							iMemberName = dr.GetOrdinal("MemberName"), 
							iAdrLine1 = dr.GetOrdinal("AdrLine1"), 
							iAdrLine2 = dr.GetOrdinal("AdrLine2"), 
							iCity = dr.GetOrdinal("AdrCity"),
							iState = dr.GetOrdinal("AdrState"),
							iZip = dr.GetOrdinal("AdrZip"), 
							iTaxYear = dr.GetOrdinal("TaxYear"), 
							iCropYear = dr.GetOrdinal("CropYear"), 
							iRatePerTon = dr.GetOrdinal("RatePerTon"), 
							iPercentageToApply = dr.GetOrdinal("PercentageToApply"), 
							iReportDate = dr.GetOrdinal("ReportDate"), 
							iFiscalYearEndDate = dr.GetOrdinal("FiscalYearEndDate"), 
							iFiscalYear = dr.GetOrdinal("FiscalYear"), 
							iInitialPayDate = dr.GetOrdinal("InitialPayDate"), 
							iTons = dr.GetOrdinal("Tons"),
							iPatronShareOfDeduction = dr.GetOrdinal("PatronShareOfDeduction");

							while (dr.Read()) {
								stateList.Add(new ListNoticeOfPassthrough(
									dr.GetString(iSHID), dr.GetString(iMemberName), dr.GetString(iAdrLine1),
									dr.GetString(iAdrLine2), dr.GetString(iCity), dr.GetString(iState), dr.GetString(iZip), 
									dr.GetInt32(iTaxYear), dr.GetInt32(iCropYear),
									dr.GetDecimal(iRatePerTon), dr.GetDecimal(iPercentageToApply), dr.GetDateTime(iReportDate),
									dr.GetDateTime(iFiscalYearEndDate), dr.GetInt32(iFiscalYear), dr.GetDateTime(iInitialPayDate),
									dr.GetDecimal(iTons), dr.GetDecimal(iPatronShareOfDeduction)
								));
							}
						}
					}
					catch (SqlException sqlEx) {

						if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
							WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
							throw (wscWarn);
						} else {
							WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException("Error calling " + procName, sqlEx);
							throw (wex);
						}
					}
				}
			}
			catch (System.Exception ex) {
				WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + ": Error calling " + procName, ex);
				throw (wex);
			}

			return stateList;
		}

        public static List<ListStatementPatRetainItem> RptStatementPatronage(int cropYear, string shid, string fromShid, string toShid, string paymentDate) {

            const string METHOD_NAME = "RptStatementPatronage";
            List<ListStatementPatRetainItem> state = new List<ListStatementPatRetainItem>();

            try {

                string procName = "bawpRptStatementPatronage";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = Convert.ToInt32(cropYear);
                    spParams[1].Value = paymentDate;
                    spParams[2].Value = shid;
                    spParams[3].Value = fromShid;
                    spParams[4].Value = toShid;


                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iequityCropYear = dr.GetOrdinal("EquityCropYear");
                            int irefundDate = dr.GetOrdinal("RefundDate");
                            int iqualified = dr.GetOrdinal("Qualified");
                            int imemberID = dr.GetOrdinal("MemberID");
                            int iratePerTon = dr.GetOrdinal("RatePerTon");
                            int ishid = dr.GetOrdinal("SHID");
                            int ibusName = dr.GetOrdinal("BusName");
                            int iaddr1 = dr.GetOrdinal("Address1");
                            int iaddr2 = dr.GetOrdinal("Address2");
                            int icsz = dr.GetOrdinal("CSZ");
                            int ideductionDesc = dr.GetOrdinal("DeductionDescription");
                            int ideductionAmt = dr.GetOrdinal("DeductionAmount");
                            int iequityTons = dr.GetOrdinal("PatronageTons");
                            int iequityAmt = dr.GetOrdinal("PatronageAmount");
                            int ipatCertPct = dr.GetOrdinal("CertPct");
                            int ipatInitPayPct = dr.GetOrdinal("InitPayPct");
                            int ipatInitPayDate = dr.GetOrdinal("InitPayDate");
                            int ipatInitPayment = dr.GetOrdinal("InitialPayment");

                            while (dr.Read()) {

                                state.Add(new ListStatementPatRetainItem(dr.GetInt32(iequityCropYear),
                                    dr.GetString(irefundDate), dr.GetString(iqualified), dr.GetInt32(imemberID), 
                                    dr.GetDecimal(iratePerTon), dr.GetString(ishid), dr.GetString(ibusName),
                                    dr.GetString(iaddr1), dr.GetString(iaddr2), dr.GetString(icsz), dr.GetString(ideductionDesc),
                                    dr.GetDecimal(ideductionAmt), dr.GetDecimal(iequityTons), dr.GetDecimal(iequityAmt), 
                                    dr.GetDecimal(ipatCertPct), dr.GetDecimal(ipatInitPayPct),
                                    dr.GetString(ipatInitPayDate), dr.GetDecimal(ipatInitPayment)));
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
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return state;
        }

        public static List<ListStatementPatRetainItem> RptStatementPatronageRedeem(int cropYear, string shid, string fromShid, string toShid, string paymentDate) {

            const string METHOD_NAME = "RptStatementPatronageRedeem";
            List<ListStatementPatRetainItem> state = new List<ListStatementPatRetainItem>();

            try {

                string procName = "bawpRptStatementPatronageRedeem";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = Convert.ToInt32(cropYear);
                    spParams[1].Value = paymentDate;
                    spParams[2].Value = shid;
                    spParams[3].Value = fromShid;
                    spParams[4].Value = toShid;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iequityCropYear = dr.GetOrdinal("EquityCropYear");
                            int irefundDate = dr.GetOrdinal("RefundDate");
                            int iqualified = dr.GetOrdinal("Qualified");
                            int imemberID = dr.GetOrdinal("MemberID");
                            int iratePerTon = dr.GetOrdinal("RatePerTon");
                            int ishid = dr.GetOrdinal("SHID");
                            int ibusName = dr.GetOrdinal("BusName");
                            int iaddr1 = dr.GetOrdinal("Address1");
                            int iaddr2 = dr.GetOrdinal("Address2");
                            int icsz = dr.GetOrdinal("CSZ");
                            int ideductionDesc = dr.GetOrdinal("DeductionDescription");
                            int ideductionAmt = dr.GetOrdinal("DeductionAmount");
                            int iequityTons = dr.GetOrdinal("PatronageTons");
                            int iequityAmt = dr.GetOrdinal("PatronageAmount");
                            int iredeemPct = dr.GetOrdinal("RedeemPct");
                            int iredeemAmt = dr.GetOrdinal("RedeemAmount");
                            int ipatInitPayment = dr.GetOrdinal("InitialPayment");

                            while (dr.Read()) {

                                state.Add(new ListStatementPatRetainItem(dr.GetInt32(iequityCropYear),
                                    dr.GetString(irefundDate), dr.GetString(iqualified), dr.GetInt32(imemberID),
                                    dr.GetDecimal(iratePerTon), dr.GetString(ishid), dr.GetString(ibusName),
                                    dr.GetString(iaddr1), dr.GetString(iaddr2), dr.GetString(icsz), dr.GetString(ideductionDesc),
                                    dr.GetDecimal(ideductionAmt), dr.GetDecimal(iequityTons), dr.GetDecimal(iequityAmt), 
                                    dr.GetDecimal(iredeemPct), dr.GetDecimal(iredeemAmt),
                                    dr.GetDecimal(ipatInitPayment)));
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
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return state;
        }

        public static List<ListStatementPatRetainItem> RptStatementRetainRedeem(int cropYear, string shid, string fromShid, string toShid, string paymentDate) {

            const string METHOD_NAME = "RptStatementRetainRedeem";
            List<ListStatementPatRetainItem> state = new List<ListStatementPatRetainItem>();

            try {

                string procName = "bawpRptStatementRetainRedeem";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = Convert.ToInt32(cropYear);
                    spParams[1].Value = paymentDate;
                    spParams[2].Value = shid;
                    spParams[3].Value = fromShid;
                    spParams[4].Value = toShid;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iequityCropYear = dr.GetOrdinal("EquityCropYear");
                            int irefundDate = dr.GetOrdinal("RefundDate");
                            int iqualified = dr.GetOrdinal("Qualified");
                            int imemberID = dr.GetOrdinal("MemberID");
                            int iratePerTon = dr.GetOrdinal("RatePerTon");
                            int ishid = dr.GetOrdinal("SHID");
                            int ibusName = dr.GetOrdinal("BusName");
                            int iaddr1 = dr.GetOrdinal("Address1");
                            int iaddr2 = dr.GetOrdinal("Address2");
                            int icsz = dr.GetOrdinal("CSZ");
                            int ideductionDesc = dr.GetOrdinal("DeductionDescription");
                            int ideductionAmt = dr.GetOrdinal("DeductionAmount");
                            int iequityTons = dr.GetOrdinal("RetainTons");
                            int iequityAmt = dr.GetOrdinal("RetainAmount");
                            int iredeemPct = dr.GetOrdinal("RedeemPct");
                            int iredeemAmt = dr.GetOrdinal("RedeemAmount");

                            while (dr.Read()) {

                                state.Add(new ListStatementPatRetainItem(dr.GetInt32(iequityCropYear),
                                    dr.GetString(irefundDate), dr.GetString(iqualified), dr.GetInt32(imemberID),
                                    dr.GetDecimal(iratePerTon), dr.GetString(ishid), dr.GetString(ibusName),
                                    dr.GetString(iaddr1), dr.GetString(iaddr2), dr.GetString(icsz), dr.GetString(ideductionDesc),
                                    dr.GetDecimal(ideductionAmt), dr.GetDecimal(iequityTons), dr.GetDecimal(iequityAmt), 
                                    dr.GetDecimal(iredeemPct), dr.GetDecimal(iredeemAmt)));
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
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return state;
        }

        public static List<ListStatementPatRetainItem> RptCertificateRetains(int cropYear, string shid, string fromShid, string toShid) {

            const string METHOD_NAME = "RptCertificateRetains";
            List<ListStatementPatRetainItem> state = new List<ListStatementPatRetainItem>();

            try {

                string procName = "bawpRptCertificateRetain";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = Convert.ToInt32(cropYear);
                    spParams[1].Value = shid;
                    spParams[2].Value = fromShid;
                    spParams[3].Value = toShid;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iequityCropYear = dr.GetOrdinal("CropYear");
                            int iqualified = dr.GetOrdinal("Qualified");
                            int ishid = dr.GetOrdinal("SHID");
                            int ibusName = dr.GetOrdinal("BusName");
                            int iaddr1 = dr.GetOrdinal("Address1");
                            int iaddr2 = dr.GetOrdinal("Address2");
                            int icsz = dr.GetOrdinal("CSZ");
                            int iequityAmt = dr.GetOrdinal("RetainAmount");

                            while (dr.Read()) {

                                state.Add(new ListStatementPatRetainItem(dr.GetInt32(iequityCropYear),
                                    "", dr.GetString(iqualified), 0,
                                    0, dr.GetString(ishid), dr.GetString(ibusName),
                                    dr.GetString(iaddr1), dr.GetString(iaddr2), dr.GetString(icsz), "",
                                    0, 0, dr.GetDecimal(iequityAmt),
                                    0, 0));
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
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return state;
        }

        public static List<ListStatementPatRetainItem> RptCertificatePat(int cropYear, string shid, string fromShid, string toShid) {

            const string METHOD_NAME = "RptCertificatePat";
            List<ListStatementPatRetainItem> state = new List<ListStatementPatRetainItem>();

            try {

                string procName = "bawpRptCertificatePatronage";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                   SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = Convert.ToInt32(cropYear);
                    spParams[1].Value = shid;
                    spParams[2].Value = fromShid;
                    spParams[3].Value = toShid;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            // In order to reuse ListStatementPatRetainItem, a worthy cause, we're going to cheat
                            // by using RemdeemAmt to reference Patronage.

                            int iequityCropYear = dr.GetOrdinal("CropYear");
                            int iqualified = dr.GetOrdinal("Qualified");
                            int ishid = dr.GetOrdinal("SHID");
                            int ibusName = dr.GetOrdinal("BusName");
                            int iaddr1 = dr.GetOrdinal("Address1");
                            int iaddr2 = dr.GetOrdinal("Address2");
                            int icsz = dr.GetOrdinal("CSZ");
                            int iequityAmt = dr.GetOrdinal("TotalPatronage");
                            int ipatInitPayment = dr.GetOrdinal("InitialPay");
                            int iredeemAmt = dr.GetOrdinal("Patronage");

                            while (dr.Read()) {

                                state.Add(new ListStatementPatRetainItem(dr.GetInt32(iequityCropYear),
                                    "", dr.GetString(iqualified), 0,
                                    0, dr.GetString(ishid), dr.GetString(ibusName),
                                    dr.GetString(iaddr1), dr.GetString(iaddr2), dr.GetString(icsz), "",
                                    0, 0, dr.GetDecimal(iequityAmt),
                                    0, dr.GetDecimal(iredeemAmt), dr.GetDecimal(ipatInitPayment)));
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
            }
            catch (System.Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }

            return state;
        }

        private static void SetTimeout() {
            string timeOut = ConfigurationManager.AppSettings["sql.command.timeout"].ToString();
            SqlHelper.CommandTimeout = timeOut;
        }
    }

	public class ListNoticeOfPassthrough {

		public ListNoticeOfPassthrough() {}

		public ListNoticeOfPassthrough(string shid, string memberName, string adrLine1, string adrLine2,
			string adrCity, string adrState, string adrZip, int taxYear, int cropYear, decimal ratePerTon, 
			decimal percentageToApply, DateTime reportDate,
			DateTime fiscalYearEndDate, int fiscalYear, DateTime initialPayDate, decimal tons, decimal patronShareOfDeduction) {

			SHID = shid;
			MemberName = memberName;
			AdrLine1 = adrLine1;
			AdrLine2 = adrLine2;
			AdrCity = adrCity;
			AdrState = adrState;
			AdrZip = adrZip;
			TaxYear = taxYear;
			CropYear = cropYear;
			RatePerTon = ratePerTon;
			PercentageToApply = percentageToApply;
			ReportDate = reportDate;
			FiscalYearEndDate = fiscalYearEndDate;
			FiscalYear = fiscalYear;
			InitialPayDate = initialPayDate;
			Tons = tons;
			PatronShareOfDeduction = patronShareOfDeduction;
		}

		public string SHID { get; set; }
		public string MemberName { get; set; }
		public string AdrLine1 { get; set; }
		public string AdrLine2 { get; set; }
		public string AdrCity { get; set; }
		public string AdrState { get; set; }
		public string AdrZip { get; set; }
		public int TaxYear { get; set; }
		public int CropYear { get; set; }
		public decimal RatePerTon { get; set; }
		public decimal PercentageToApply { get; set; }
		public DateTime ReportDate { get; set; }
		public DateTime FiscalYearEndDate { get; set; }
		public int FiscalYear { get; set; }
		public DateTime InitialPayDate { get; set; }
		public decimal Tons { get; set; }
		public decimal PatronShareOfDeduction { get; set; }
	}

	public class ContractDeliverySummary2Item {

		public ContractDeliverySummary2Item() {}

		public ContractDeliverySummary2Item(Decimal pContractTons, Decimal pSugarPct, Decimal pSLMPct, Decimal pExtractableSugarPerTon) { 
		
			ContractTons = pContractTons;
			SugarPct = pSugarPct;
			SLMPct = pSLMPct;
			ExtractableSugarPerTon = pExtractableSugarPerTon;
		}

		public Decimal ContractTons {get; set;}
		public Decimal SugarPct {get; set;}
		public Decimal SLMPct {get; set;}
		public Decimal ExtractableSugarPerTon { get; set; }
		
	}

	public class ContractGorwerLandownerItem {


		public ContractGorwerLandownerItem() {}

		public ContractGorwerLandownerItem(
			string pFactory_Number, string pFactory_Name, string pStation_Name, string pStation_Number,
			string pContract_Number, DateTime pCrop_Year, string pLdo_Address_Number, string pLandowner_Name,
			string pLdo_Address_1, string pLdo_Address_2, string pLdo_City, string pLdo_State,
			string pLdo_Zip, string pGro_Address_Number, string pGrower_Name, string pGro_Address_1,
			string pGro_Address_2, string pGrower_City, string pGrower_State, string pGrower_Zip,
			string pLegal_State, string pLegal_County, Int16 pContract_Acres, Int16 pPlanted_Acres,
			Int16 pHarvest_Acres, bool pAssoc_Member, bool pASCS_Signed, bool pCash_Rent,
			Decimal pPac_Dues, int pcnt_contract_id, string pLegal_Seq_Number, string pGrower_Seq_Number,
			string pLandowner_Seq_Number, float pPayee_Percent, string pPayee_Name, string pPayee_Addr_No,
			string pPayee_Sequence_Number 
		) {

			Factory_Number = pFactory_Number;
			Factory_Name = pFactory_Name;
			Station_Name = pStation_Name;
			Station_Number = pStation_Number;
			Contract_Number = pContract_Number;
			Crop_Year = pCrop_Year;
			Ldo_Address_Number = pLdo_Address_Number;
			Landowner_Name = pLandowner_Name;
			Ldo_Address_1 = pLdo_Address_1;
			Ldo_Address_2 = pLdo_Address_2;
			Ldo_City = pLdo_City;
			Ldo_State = pLdo_State;
			Ldo_Zip = pLdo_Zip;
			Gro_Address_Number = pGro_Address_Number;
			Grower_Name = pGrower_Name;
			Gro_Address_1 = pGro_Address_1;
			Gro_Address_2 = pGro_Address_2;
			Grower_City = pGrower_City;
			Grower_State = pGrower_State;
			Grower_Zip = pGrower_Zip;
			Legal_State = pLegal_State;
			Legal_County = pLegal_County;
			Contract_Acres = pContract_Acres;
			Planted_Acres = pPlanted_Acres;
			Harvest_Acres = pHarvest_Acres;
			Assoc_Member = pAssoc_Member;
			ASCS_Signed = pASCS_Signed;
			Cash_Rent = pCash_Rent;
			Pac_Dues = pPac_Dues;
			cnt_contract_id = pcnt_contract_id;
			Legal_Seq_Number = pLegal_Seq_Number;
			Grower_Seq_Number = pGrower_Seq_Number;
			Landowner_Seq_Number = pLandowner_Seq_Number;
			Payee_Percent = pPayee_Percent;
			Payee_Name = pPayee_Name;
			Payee_Addr_No = pPayee_Addr_No;
			Payee_Sequence_Number = pPayee_Sequence_Number;
		}

		public string Factory_Number { get; set; }
		public string Factory_Name { get; set; }
		public string Station_Name { get; set; }
		public string Station_Number { get; set; }
		public string Contract_Number { get; set; }
		public DateTime Crop_Year { get; set; }
		public string Ldo_Address_Number { get; set; }
		public string Landowner_Name { get; set; }
		public string Ldo_Address_1 { get; set; }
		public string Ldo_Address_2 { get; set; }
		public string Ldo_City { get; set; }
		public string Ldo_State { get; set; }
		public string Ldo_Zip { get; set; }
		public string Gro_Address_Number { get; set; }
		public string Grower_Name { get; set; }
		public string Gro_Address_1 { get; set; }
		public string Gro_Address_2 { get; set; }
		public string Grower_City { get; set; }
		public string Grower_State { get; set; }
		public string Grower_Zip { get; set; }
		public string Legal_State { get; set; }
		public string Legal_County { get; set; }
		public Int16 Contract_Acres { get; set; }
		public Int16 Planted_Acres { get; set; }
		public Int16 Harvest_Acres { get; set; }
		public bool Assoc_Member { get; set; }
		public bool ASCS_Signed { get; set; }
		public bool Cash_Rent { get; set; }
		public Decimal Pac_Dues { get; set; }
		public int cnt_contract_id { get; set; }
		public string Legal_Seq_Number { get; set; }
		public string Grower_Seq_Number { get; set; }
		public string Landowner_Seq_Number { get; set; }
		public float Payee_Percent { get; set; }
		public string Payee_Name { get; set; }
		public string Payee_Addr_No { get; set; }
		public string Payee_Sequence_Number { get; set; }
	}

    public class ListGrowerTareItem {

        public ListGrowerTareItem() { }

        public ListGrowerTareItem(ListGrowerTareItem item) {

            DeliveryID = item.DeliveryID;
            ContractID = item.ContractID;
            Contract_No = item.Contract_No;
            Delivery_Date = item.Delivery_Date;
            Delivery_Station_ID = item.Delivery_Station_ID;
            Delivery_Station_No = item.Delivery_Station_No;
            Delivery_Station_Name = item.Delivery_Station_Name;
            Delivery_Factory_No = item.Delivery_Factory_No;
            First_Net_Pounds = item.First_Net_Pounds;
            Tare_Pounds = item.Tare_Pounds;
            Final_Net_Pounds = item.Final_Net_Pounds;
            Tare = item.Tare;
            Sugar_Content = item.Sugar_Content;
            SLM_Pct = item.SLM_Pct;
            ExSugarPerTon = item.ExSugarPerTon;
            ShowTare = item.ShowTare;
            RptType = item.RptType;
        }

        public ListGrowerTareItem(int deliveryID, int contractID, int contract_No, string delivery_Date, int delivery_Station_ID,
            string delivery_Station_No, string delivery_Station_Name, string delivery_Factory_No, int first_Net_Pounds, int tare_Pounds, 
            int final_Net_Pounds, decimal tare, decimal sugar_Content, decimal slm_Pct, int exSugarPerTon, bool showTare, string rptType) {

                DeliveryID = deliveryID;
                ContractID = contractID;
                Contract_No = contract_No;
                Delivery_Date = delivery_Date;
                Delivery_Station_ID = delivery_Station_ID;
                Delivery_Station_No = delivery_Station_No;
                Delivery_Station_Name = delivery_Station_Name;
                Delivery_Factory_No = delivery_Factory_No;
                First_Net_Pounds = first_Net_Pounds;
                Tare_Pounds = tare_Pounds;
                Final_Net_Pounds = final_Net_Pounds;
                Tare = tare;
                Sugar_Content = sugar_Content;
                SLM_Pct = slm_Pct;
                ExSugarPerTon = exSugarPerTon;
                ShowTare = showTare;
                RptType = rptType;            
        }

        public int DeliveryID { get; set; }
        public int ContractID { get; set; }
        public int Contract_No { get; set; }
        public string Delivery_Date { get; set; }
        public int Delivery_Station_ID { get; set; }
        public string Delivery_Station_No { get; set; }
        public string Delivery_Station_Name { get; set; }
        public string Delivery_Factory_No { get; set; }
        public int First_Net_Pounds { get; set; }
        public int Tare_Pounds { get; set; }
        public int Final_Net_Pounds { get; set; }
        public decimal Tare { get; set; }
        public decimal Sugar_Content { get; set; }
        public decimal SLM_Pct { get; set; }
        public int ExSugarPerTon { get; set; }
        public bool ShowTare { get; set; }
        public string RptType { get; set; }

        public string Success { get; set; }

    }
}
