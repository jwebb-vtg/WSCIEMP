using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MDAAB.Classic;

namespace WSCData {

    public class BeetDataMember {

        private const string MOD_NAME = "WSCData.BeetDataMember.";
        private static string LF = System.Environment.NewLine;

        public static List<ListMemberStockSummaryItem> MemberStockGetSummary(int memberID, int cropYear) {

            const string METHOD_NAME = "MemberStockGetSummary";

            List<ListMemberStockSummaryItem> state = new List<ListMemberStockSummaryItem>();

            try {

                string procName = "bawpMemberStockGetSummary";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = memberID;
                        spParams[1].Value = cropYear;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iMemberNumber = dr.GetOrdinal("MemberNumber");
                            int iFactoryName = dr.GetOrdinal("FactoryName");
                            int iPatronShares = dr.GetOrdinal("PatronShares");
                            int iPatronOverPlant = dr.GetOrdinal("PatronOverPlant");
                            int iTransfereeShares = dr.GetOrdinal("TransfereeShares");
                            int iTransfereeOverPlant = dr.GetOrdinal("TransfereeOverPlant");
                            int iTransferorShares = dr.GetOrdinal("TransferorShares");
                            int iDeliveryShareRights = dr.GetOrdinal("DeliveryShareRights");
                            int iSharesUsed = dr.GetOrdinal("SharesUsed");
                            int iSharesUnassigned = dr.GetOrdinal("SharesUnassigned");
                            int iHasLien = dr.GetOrdinal("HasLien");
                            int iIsSubscriber = dr.GetOrdinal("IsSubscriber");                                    

                            while (dr.Read()) {
           
                                state.Add(new ListMemberStockSummaryItem(dr.GetString(iMemberNumber), dr.GetString(iFactoryName), dr.GetInt32(iPatronShares),
                                    dr.GetInt32(iPatronOverPlant), dr.GetInt32(iTransfereeShares), dr.GetInt32(iTransfereeOverPlant),
                                    dr.GetInt32(iTransferorShares), dr.GetInt32(iDeliveryShareRights), dr.GetInt32(iSharesUsed), dr.GetInt32(iSharesUnassigned),
                                    dr.GetBoolean(iHasLien), dr.GetBoolean(iIsSubscriber)));
                            }
                        }

                        if (state.Count == 0) {
                            state.Add(new ListMemberStockSummaryItem("", "", 0, 0, 0, 0, 0, 0, 0, 0, false, false));
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

        public static void GetMemberInfo(string shid, int cropYear, ref int memberID, ref int addressID, ref string busName, ref string phone, ref string email, ref string fax,
                ref int factoryID, ref int factoryNumber, ref string factoryName) {

            const string METHOD_NAME = "GetMemberInfo";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpMemberGetBeetInfo";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    // Manually synch these fields with database.
                    int imemberID = 0, iaddressID = 1, ibusname = 2, ipone = 3, iemail = 4, ifax = 5, ifactoryID = 6, ifactoryNumber = 7, ifactoryName = 8;
                    spParams[0].Value = shid;
                    spParams[1].Value = cropYear;

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {
                        if (dr.Read()) {

                            memberID = dr.GetInt32(imemberID);
                            addressID = dr.GetInt32(iaddressID);
                            busName = dr.GetString(ibusname);
                            phone = dr.GetString(ipone);
                            email = dr.GetString(iemail);
                            fax = dr.GetString(ifax);
                            factoryID = dr.GetInt32(ifactoryID);
                            factoryNumber = dr.GetInt32(ifactoryNumber);
                            factoryName = dr.GetString(ifactoryName);

                        }
                    }
                }
            }
            catch (SqlException sqlEx) {

                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    string errMsg = MOD_NAME + METHOD_NAME + LF + "SHID: " + shid;
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                    throw (wscEx);
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME + LF + "SHID: " + shid;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static List<ListShareTransferItem> ShareTransferGetYear(int memberID, int cropYear) {

            const string METHOD_NAME = "ShareTransferGetYear";

            List<ListShareTransferItem> state = new List<ListShareTransferItem>();

            try {

                string procName = "bawpShareTransferGetYear";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = memberID;
                        spParams[1].Value = cropYear;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int ishareTransferID = dr.GetOrdinal("ShareTransferID");
                            int icontractNumber = dr.GetOrdinal("ContractNumber");
                            int itoMemberID = dr.GetOrdinal("ToMemberID");
                            int itoShid = dr.GetOrdinal("ToShid");
                            int itoFactoryName = dr.GetOrdinal("ToFactoryName");
                            int itoRetainPct = dr.GetOrdinal("ToRetainPct");
                            int itoCropPct = dr.GetOrdinal("ToCropPct");
                            int ipricePerAcre = dr.GetOrdinal("PricePerAcre");
                            int ishares = dr.GetOrdinal("Shares");
                            int ifromMemberID = dr.GetOrdinal("FromMemberID");
                            int ifromShid = dr.GetOrdinal("FromShid");
                            int ifromFactoryName = dr.GetOrdinal("FromFactoryName");
                            int ifromRetainPct = dr.GetOrdinal("FromRetainPct"); ;
                            int ihasLienOnShares = dr.GetOrdinal("HasLienOnShares"); ;
                            int ihasConsentForm = dr.GetOrdinal("HasConsentForm"); ;
                            int iapprovalDate = dr.GetOrdinal("ApprovalDate"); ;
                            int iIsFeePaid = dr.GetOrdinal("IsFeePaid");
                            int iShareTransferNumber = dr.GetOrdinal("ShareTransferNumber");
                            int iShareTransferTimeStamp = dr.GetOrdinal("ShareTransferTimeStamp");

                            while (dr.Read()) {

                                state.Add(new ListShareTransferItem(dr.GetInt32(ishareTransferID), dr.GetString(icontractNumber), dr.GetInt32(itoMemberID), 
                                    dr.GetString(itoShid), dr.GetString(itoFactoryName), dr.GetDecimal(itoRetainPct), dr.GetDecimal(itoCropPct), 
                                    dr.GetDecimal(ipricePerAcre), dr.GetInt32(ishares), dr.GetInt32(ifromMemberID), dr.GetString(ifromShid), 
                                    dr.GetString(ifromFactoryName), dr.GetDecimal(ifromRetainPct), dr.GetBoolean(ihasLienOnShares),
                                    dr.GetBoolean(ihasConsentForm), dr.GetString(iapprovalDate), dr.GetBoolean(iIsFeePaid),
                                    dr.GetInt32(iShareTransferNumber), dr.GetString(iShareTransferTimeStamp)));
                            }
                        }

                        if (state.Count == 0) {
                            state.Add(new ListShareTransferItem(0, "", 0, "", "", 0, 0, 0, 0, 0, "", "", 0, false, false, "", false, 0, ""));
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

        public static void ShareTransferDelete(int shareTransferID) {

            const string METHOD_NAME = "ShareTransferDelete";
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpShareTransferDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = shareTransferID;

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

        public static void ShareTransferSave(int shareTransferID, string contractNumber, int cropYear, int transferNumber, int fromSHID, int fromFactoryNumber, 
            decimal fromRetainPct, int toSHID, decimal toRetainPct, int toFactoryNumber, int shares, string transferDate, bool isFeePaid, 
            string approvalDate, decimal paidPricePerAcre, decimal paidPctCrop, string shareTransferTimeStamp, string userName) {

            const string METHOD_NAME = "ShareTransferSave";
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpShareTransferSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = shareTransferID;
                    spParams[1].Value = contractNumber;
                    spParams[2].Value = cropYear;
                    spParams[3].Value = transferNumber;
                    spParams[4].Value = fromSHID;
                    spParams[5].Value = fromFactoryNumber;
                    spParams[6].Value = fromRetainPct;
                    spParams[7].Value = toSHID;
                    spParams[8].Value = toRetainPct;
                    spParams[9].Value = toFactoryNumber;
                    spParams[10].Value = shares;

                    if (transferDate.Length == 0) {
                        spParams[11].Value = DBNull.Value;
                    } else {
                        spParams[11].Value = transferDate;
                    }

                    spParams[12].Value = isFeePaid;

                    if (approvalDate.Length == 0) {
                        spParams[13].Value = DBNull.Value;
                    } else {
                        spParams[13].Value = approvalDate;
                    }

                    spParams[14].Value = paidPricePerAcre;
                    spParams[15].Value = paidPctCrop;
                    spParams[16].Value = shareTransferTimeStamp;
                    spParams[17].Value = userName;

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
