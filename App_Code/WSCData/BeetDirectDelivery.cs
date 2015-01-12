using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using MDAAB.Classic;
using WSCData;

namespace WSCData {

    public static class BeetDirectDelivery {

        private const string MOD_NAME = "BeetDirectDelivery.";

        public static List<ListDirectDeliveryItem> DirectDeliveryGet(int cropYear, int contractStationNumber, int deliveryStationNumber) {

            const string METHOD_NAME = "DirectDeliveryGet";            

            try {

                List<ListDirectDeliveryItem> state = new List<ListDirectDeliveryItem>();

                string procName = "bawpDirectDeliveryGet";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = cropYear;
                        spParams[1].Value = contractStationNumber;
                        spParams[2].Value = deliveryStationNumber;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iDirectDeliveryID = dr.GetOrdinal("DirectDeliveryID");
                            int iContractStationNumber = dr.GetOrdinal("ContractStationNumber");
                            int iContractStationName = dr.GetOrdinal("ContractStationName");
                            int iDeliveryStationNumber = dr.GetOrdinal("DeliveryStationNumber");
                            int iDeliveryStationName = dr.GetOrdinal("DeliveryStationName");
                            int iRatePerTon = dr.GetOrdinal("RatePerTon");
                            int iRowVersion = dr.GetOrdinal("RowVersion");

                            while (dr.Read()) {

                                state.Add(new ListDirectDeliveryItem(dr.GetInt32(iDirectDeliveryID), dr.GetInt32(iContractStationNumber), dr.GetString(iContractStationName),
                                    dr.GetInt32(iDeliveryStationNumber), dr.GetString(iDeliveryStationName), dr.GetDecimal(iRatePerTon),
                                    dr.GetString(iRowVersion)));
                            }
                        }

                        if (state.Count == 0) {
                            state.Add(new ListDirectDeliveryItem(0, 0, "", 0, "", 0, ""));
                        }
                    }

                    return state;
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
        }

        public static List<ListDirectDeliveryItem> DirectDeliveryGetByID(int cropYear, int directDeliveryID) {

            const string METHOD_NAME = "DirectDeliveryGetByID";

            try {

                List<ListDirectDeliveryItem> state = new List<ListDirectDeliveryItem>();

                string procName = "bawpDirectDeliveryGetByID";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = cropYear;
                        spParams[1].Value = directDeliveryID;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iDirectDeliveryID = dr.GetOrdinal("DirectDeliveryID");
                            int iContractStationNumber = dr.GetOrdinal("ContractStationNumber");
                            int iContractStationName = dr.GetOrdinal("ContractStationName");
                            int iDeliveryStationNumber = dr.GetOrdinal("DeliveryStationNumber");
                            int iDeliveryStationName = dr.GetOrdinal("DeliveryStationName");
                            int iRatePerTon = dr.GetOrdinal("RatePerTon");
                            int iRowVersion = dr.GetOrdinal("RowVersion");

                            while (dr.Read()) {

                                state.Add(new ListDirectDeliveryItem(dr.GetInt32(iDirectDeliveryID), dr.GetInt32(iContractStationNumber), dr.GetString(iContractStationName),
                                    dr.GetInt32(iDeliveryStationNumber), dr.GetString(iDeliveryStationName), dr.GetDecimal(iRatePerTon),
                                    dr.GetString(iRowVersion)));
                            }
                        }

                        if (state.Count == 0) {
                            state.Add(new ListDirectDeliveryItem(0, 0, "", 0, "", 0, ""));
                        }
                    }

                    return state;
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
        }

        public static void DirectDeliveryContractDelete(int directDeliveryID, int cropYear, int contractID, string rowVersion) {

            const string METHOD_NAME = "DirectDeliveryContractDelete";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpDirectDeliveryContractDelete";
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = directDeliveryID;
                    spParams[1].Value = cropYear;
                    spParams[2].Value = contractID;
                    spParams[3].Value = rowVersion;

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

        public static List<ListDirectDeliveryContractItem> DirectDeliveryContractGet(int cropYear, int directDeliveryID, int contractID, int contractNumber) {

            const string METHOD_NAME = "DirectDeliveryContractGet";

            try {

                List<ListDirectDeliveryContractItem> state = new List<ListDirectDeliveryContractItem>();

                string procName = "bawpDirectDeliveryContractGet";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = cropYear;
                        spParams[1].Value = directDeliveryID;
                        spParams[2].Value = contractID;
                        spParams[3].Value = contractNumber;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iDirectDeliveryID = dr.GetOrdinal("DirectDeliveryID");
                            int iContractID = dr.GetOrdinal("ContractID");
                            int iRowVersion = dr.GetOrdinal("RowVersion");
                            int iContractNumber = dr.GetOrdinal("ContractNumber");
                            int iRatePerTon = dr.GetOrdinal("RatePerTon");

                            while (dr.Read()) {

                                state.Add(new ListDirectDeliveryContractItem(dr.GetInt32(iDirectDeliveryID), dr.GetInt32(iContractID), dr.GetString(iRowVersion),
                                    dr.GetInt32(iContractNumber), dr.GetDecimal(iRatePerTon)));
                            }
                        }

                        if (state.Count == 0) {
                            state.Add(new ListDirectDeliveryContractItem(0, 0, "", 0, 0));
                        }
                    }

                    return state;
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
        }

        public static void DirectDeliveryContractSave(int directDeliveryID, int cropYear, int contractID, decimal ratePerTon, string rowVersion, string userName) {

            const string METHOD_NAME = "DirectDeliveryContractSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpDirectDeliveryContractSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = directDeliveryID;
                    spParams[1].Value = cropYear;
                    spParams[2].Value = contractID;
                    spParams[3].Value = ratePerTon;
                    spParams[4].Value = rowVersion;
                    spParams[5].Value = userName;

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

        public static void DirectDeliverySave(int directDeliveryID, int cropYear, int contractStationNumber, int deliveryStationNumber, decimal ratePerTon,
            string rowVersion, string userName) {

        const string METHOD_NAME = "DirectDeliverySave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpDirectDeliverySave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = directDeliveryID;
                    spParams[1].Value = cropYear;
                    spParams[2].Value = contractStationNumber;
                    spParams[3].Value = deliveryStationNumber;
                    spParams[4].Value = ratePerTon;
                    spParams[5].Value = rowVersion;
                    spParams[6].Value = userName;

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

        public static void DirectDeliveryDelete(int directDeliveryID, int cropYear) {

            const string METHOD_NAME = "DirectDeliveryDelete";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpDirectDeliveryDelete";
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = directDeliveryID;
                    spParams[1].Value = cropYear;

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
    }

    public class ListDirectDeliveryItem {

        public ListDirectDeliveryItem() { }

        public ListDirectDeliveryItem(int directDeliveryID, int contractStationNumber, string contractStationName, int deliveryStationNumber, 
            string deliveryStationName, decimal ratePerTon, string rowVersion) {

                if (directDeliveryID == 0) {
                    DirectDeliveryID = "0";
                    DeliveryStation = "*";
                } else {
                    DirectDeliveryID = directDeliveryID.ToString();
                    ContractStation = contractStationNumber.ToString("0#") + " " + contractStationName;
                    DeliveryStation = deliveryStationNumber.ToString("0#") + " " + deliveryStationName;
                    RatePerTon = ratePerTon.ToString("0.0000");
                    RowVersion = rowVersion;
                }                                
        }

        private string _directDeliveryID = "";
        public string DirectDeliveryID {
            get { return _directDeliveryID; }
            set { _directDeliveryID = value; }
        }

        private string _contractStation = "";
        public string ContractStation {
            get { return _contractStation; }
            set { _contractStation = value; }
        }

        private string _deliveryStation = "";
        public string DeliveryStation {
            get { return _deliveryStation; }
            set { _deliveryStation = value; }
        }

        private string _ratePerTon = "";
        public string RatePerTon {
            get { return _ratePerTon; }
            set { _ratePerTon = value; }
        }

        private string _rowVersion = "";
        public string RowVersion {
            get { return _rowVersion; }
            set { _rowVersion = value; }
        }
    }

    public class ListDirectDeliveryContractItem {

        public ListDirectDeliveryContractItem() { }

        public ListDirectDeliveryContractItem(int directDeliveryID, int contractID, string rowVersion, int contractNumber, decimal ratePerTon) {

            if (directDeliveryID == 0) {
                DirectDeliveryID = "0";
                ContractID = "0";
                ContractNumber = "*";
            } else {
                DirectDeliveryID = directDeliveryID.ToString();
                ContractID = contractID.ToString();
                RowVersion = rowVersion;
                ContractNumber = contractNumber.ToString();
                RatePerTon = ratePerTon.ToString("0.0000");
            }      
        }

        private string _directDeliveryID = "";
        public string DirectDeliveryID {
            get { return _directDeliveryID; }
            set { _directDeliveryID = value; }
        }

        private string _contractID = "";
        public string ContractID {
            get { return _contractID; }
            set { _contractID = value; }
        }

        private string _rowVersion = "";
        public string RowVersion {
            get { return _rowVersion; }
            set { _rowVersion = value; }
        }

        private string _contractNumber = "";
        public string ContractNumber {
            get { return _contractNumber; }
            set { _contractNumber = value; }
        }

        private string _ratePerTon = "";
        public string RatePerTon {
            get { return _ratePerTon; }
            set { _ratePerTon = value; }
        }
    }

}