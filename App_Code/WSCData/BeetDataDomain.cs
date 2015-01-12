using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using MDAAB.Classic;

namespace WSCData {

    public class BeetDataDomain {

        private const string MOD_NAME = "WSCData.BeetDataDomain.";

        public static void FillCropYear(DropDownList ddl, string defaultText) {

            const string METHOD_NAME = "FillCropYear";
            try {

                const int BEET_MAX_YEARS = 10;
                int selectedIndex = 0;
                int i = 0;

                if (ddl.Items.Count == 0) {

                    ArrayList cropYear = WSCField.GetCropYears();

                    foreach (string cy in cropYear) {

                        ddl.Items.Add(cy);

                        if (cy == defaultText) {
                            ddl.Items[i].Selected = true;
                            selectedIndex = i;
                        }
                        i++;

                        if (i == BEET_MAX_YEARS) {
                            break;
                        }
                    }
                } else {

                    foreach (System.Web.UI.WebControls.ListItem li in ddl.Items) {

                        if (li.Selected) {
                            selectedIndex = i;
                            break;
                        } else {
                            if (li.Text == defaultText) {
                                selectedIndex = i;
                                break;
                            }
                        }
                        i++;

                        if (i == BEET_MAX_YEARS) {
                            break;
                        }
                    }
                }

                ddl.SelectedIndex = selectedIndex;
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static List<ListBeetFactoryNameItem> BeetFactoryNameGetList() {

            const string METHOD_NAME = MOD_NAME + "BeetFactoryNameGetList: ";
            List<ListBeetFactoryNameItem> state = new List<ListBeetFactoryNameItem>();

            int iFtyNumber = 0, iFtyName = 1;

            try {

                string procName = "bawpFactoryGetList";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName)) {
                            while (dr.Read()) {
                                state.Add(new ListBeetFactoryNameItem(dr.GetInt32(iFtyNumber), dr.GetString(iFtyName)));
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

        public static List<ListPaymentDescItem> GetPaymentDescriptions(int cropYear, bool isFinished) {

            const string METHOD_NAME = "GetPaymentDescriptions";
            try {

                string procName = "bawpPaymentGetDesc";

                List<ListPaymentDescItem> stateList = new List<ListPaymentDescItem>();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                        int ipdeID = dr.GetOrdinal("pde_description_id");
                        int ipdeNum = dr.GetOrdinal("pde_payment_number");
                        int ipdeDesc = dr.GetOrdinal("pde_payment_description");
                        int ipdeRequired = dr.GetOrdinal("pde_required");
                        int ipdeFinished = dr.GetOrdinal("pde_payment_finished");
                        int ipdeTransmittalDate = dr.GetOrdinal("pde_transmittal_date");

                        while (dr.Read()) {

                            if (!isFinished) {

                                string transDate = (dr.IsDBNull(ipdeTransmittalDate) ? "" : dr.GetString(ipdeTransmittalDate));

                                stateList.Add(new ListPaymentDescItem(dr.GetInt32(ipdeID), Convert.ToInt32(dr.GetInt16(ipdeNum)), 
                                    dr.GetBoolean(ipdeRequired), dr.GetBoolean(ipdeFinished), dr.GetString(ipdeDesc), transDate));
                            } else {

                                // When caller wants only Finished payments, check that payment is required and is finished.
                                if (dr.GetBoolean(ipdeRequired) && dr.GetBoolean(ipdeFinished)) {

                                    string transDate = (dr.IsDBNull(ipdeTransmittalDate) ? "" : dr.GetString(ipdeTransmittalDate));

                                    stateList.Add(new ListPaymentDescItem(dr.GetInt32(ipdeID), Convert.ToInt32(dr.GetInt16(ipdeNum)),
                                    dr.GetBoolean(ipdeRequired), dr.GetBoolean(ipdeFinished), dr.GetString(ipdeDesc), transDate));
                                }
                            }
                        }

                        return stateList;
                    }
                }
            }
            catch (SqlException sqlEx) {
                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                    throw (wscEx);
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }
        }

        public static List<ListBeetFactoryIDItem> BeetFactoryIDGetList(int cropYear) {

            const string METHOD_NAME = "BeetFactoryIDGetList";
            List<ListBeetFactoryIDItem> state = new List<ListBeetFactoryIDItem>();

            try {

                string procName = "bawpFactoryGetAll";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                        int iFtyID = dr.GetOrdinal("fty_factory_id");
                        int iFtyNumber = dr.GetOrdinal("fty_factory_no");
                        int iFtyName = dr.GetOrdinal("fty_name");

                        while (dr.Read()) {
                            state.Add(new ListBeetFactoryIDItem(dr.GetInt32(iFtyID), Convert.ToInt32(dr.GetInt16(iFtyNumber)), dr.GetString(iFtyName)));
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

        public static List<ListBeetStationIDItem> StationListGetAll(int cropYear) {

            const string METHOD_NAME = "StationListGetAll";
            List<ListBeetStationIDItem> state = new List<ListBeetStationIDItem>();

            try {

                string procName = "bawpStationGetAll";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;

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

        public static void SplitShidList(string shid, out string shidList, out string shidLo, out string shidHi) {

            const string METHOD_NAME = "SplitShidList";

            // Now we have one or more shids.
            shidList = "";
            shidLo = "";
            shidHi = "";

            // First squeeze out any blanks
            string shidTest = shid.Replace(" ", "");

            try {

                // CSV check -- string has a "," character
                if (shidTest.IndexOf(",") != -1) {

                    // Cannot ALSO have an embedded dash "-"
                    if (shidTest.IndexOf("-") != -1) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You can enter a list or a range of SHIDs but not both.");
                        throw (warn);
                    } else {
                        shidList = shidTest;
                    }

                } else {

                    // Range check -- string has a dash separating the low/high values.
                    if (shidTest.IndexOf("-") != -1) {

                        string[] shidParts = shidTest.Split(new char[] { '-' });
                        if (shidParts.Length != 2) {
                            WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Range not entered correctly.  You indicate a range by placing a dash between a low value of SHID and a higher value of SHID.");
                            throw (warn);
                        }

                        shidLo = shidParts[0];
                        shidHi = shidParts[1];
                    } else {

                        // No comma and no dash, must be a sigle shid specified.
                        shidList = shidTest;
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscEx);
            }
        }

        public static void FillFactory(DropDownList ddl) {

            const string METHOD_NAME = "FillFactory";
            
            try {

                ddl.Items.Clear();

                ddl.Items.Add(new ListItem("Select a Factory", "0"));               // The blank item.
                List<ListBeetFactoryNameItem> ftyList = BeetFactoryNameGetList();
                foreach (ListBeetFactoryNameItem ftyItem in ftyList) {

                    ListItem item = new ListItem(ftyItem.FactoryLongName, ftyItem.FactoryNumber);
                    ddl.Items.Add(item);
                }
                ddl.SelectedIndex = 0;

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }
    }

    public class ListPaymentDescItem {

        public ListPaymentDescItem() { }

        public ListPaymentDescItem(int paymentDescID, int paymentNumber, bool isRequired, bool isFinished, string paymentDesc, string transmittalDate) {

            PaymentDescID = paymentDescID.ToString();
            PaymentNumber = paymentNumber.ToString();
            IsRequired = isRequired;
            IsFinished = isFinished;
            PaymentDesc = paymentDesc;
            TransmittalDate = transmittalDate;
        }

        private string _paymentDescID = "";
        public string PaymentDescID {
            get { return _paymentDescID; }
            set { _paymentDescID = value; }
        }
        private string _paymentNumber = "";
        public string PaymentNumber {
            get { return _paymentNumber; }
            set { _paymentNumber = value; }
        }
        private string _paymentDesc = "";
        public string PaymentDesc {
            get { return _paymentDesc; }
            set { _paymentDesc = value; }
        }
        private bool _isRequired = false;
        public bool IsRequired {
            get { return _isRequired; }
            set { _isRequired = value; }
        }
        private bool _isFinished = false;
        public bool IsFinished {
            get { return _isFinished; }
            set { _isFinished = value; }
        }
        public string PaymentNumDesc {
            get { return _paymentNumber + " " + _paymentDesc; }
        }
        public string PaymentDescSpecial {
            get { 

                string payNum = "";
                switch (Convert.ToInt32(_paymentNumber)) {
                    case 1:
                        payNum = _paymentNumber + "st";
                        break;
                    case 2:
                        payNum = _paymentNumber + "nd";
                        break;
                    case 3:
                        payNum = _paymentNumber + "rd";
                        break;
                    default:
                        payNum = _paymentNumber + "th";
                        break;
                }

                return payNum + " Payment";                        
            }
        }
        private string _transmittalDate = "";
        public string TransmittalDate {
            get { return _transmittalDate; }
            set { _transmittalDate = value; }
        }
    }
}
