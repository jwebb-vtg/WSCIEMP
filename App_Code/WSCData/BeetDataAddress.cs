using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MDAAB.Classic;

namespace WSCData {

    public class BeetDataAddress {

        private const string MOD_NAME = "WSCData.BeetDataAddress.";

        public static List<ListAddressItem> AddressFindByTerm(string searchTerm, int cropYear, int iType) {

            const string METHOD_NAME = "AddressFindByTerm";

            List<ListAddressItem> state = new List<ListAddressItem>();

            try {

                string procName = "bawpAddressFind";

                try {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[0].Value = searchTerm;
                        spParams[1].Value = cropYear;
                        spParams[2].Value = iType;

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iAddressID = dr.GetOrdinal("adr_address_id");
                            int iMemberID = dr.GetOrdinal("adr_MemberID");
                            int iShid = dr.GetOrdinal("adr_address_no");
                            int iIsSubscriber = dr.GetOrdinal("adr_IsSubscriber");
                            int iFName = dr.GetOrdinal("adr_contact_first_name");
                            int iLName = dr.GetOrdinal("adr_contact_last_name");
                            int iBusName = dr.GetOrdinal("adr_business_name");
                            int iTaxID = dr.GetOrdinal("adr_tax_id");
                            int iAdrLine1 = dr.GetOrdinal("adr_addr_1");
                            int iAdrLine2 = dr.GetOrdinal("adr_addr_2");
                            int iCityName = dr.GetOrdinal("adr_city");
                            int iStateName = dr.GetOrdinal("adr_state");
                            int iPostalCode = dr.GetOrdinal("adr_postal_code");
                            int iPhoneNo = dr.GetOrdinal("adr_phone");
                            int iEmail = dr.GetOrdinal("adr_email");
                            int iAddressType = dr.GetOrdinal("adr_address_type_id");

                            while (dr.Read()) {

                                state.Add(new ListAddressItem(dr.GetInt32(iAddressID), dr.GetInt32(iMemberID), dr.GetString(iShid), 
                                    dr.GetBoolean(iIsSubscriber), dr.GetString(iFName),
                                    dr.GetString(iLName), dr.GetString(iBusName), dr.GetString(iTaxID),
                                    dr.GetString(iAdrLine1), dr.GetString(iAdrLine2), dr.GetString(iCityName), dr.GetString(iStateName),
                                    dr.GetString(iPostalCode), dr.GetString(iPhoneNo), dr.GetString(iEmail), dr.GetInt32(iAddressType)));
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

        public static List<ListAddressItem> AddressGetInfo(int shid, int addressID, int cropYear) {

            const string METHOD_NAME = "AddressGetInfo";

            List<ListAddressItem> state = new List<ListAddressItem>();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpAddressGetInfo";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = shid;
                    spParams[1].Value = addressID;
                    spParams[2].Value = cropYear;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iAddressID = dr.GetOrdinal("adr_address_id");
                            int iMemberID = dr.GetOrdinal("adr_member_id");
                            int iShid = dr.GetOrdinal("adr_address_no");
                            int iIsSubscriber = dr.GetOrdinal("adr_IsSubscriber");
                            int iFName = dr.GetOrdinal("adr_contact_first_name");
                            int iLName = dr.GetOrdinal("adr_contact_last_name");
                            int iBusName = dr.GetOrdinal("adr_business_name");
                            int iTaxID = dr.GetOrdinal("adr_tax_id");
                            int iAdrLine1 = dr.GetOrdinal("adr_addr_1");
                            int iAdrLine2 = dr.GetOrdinal("adr_addr_2");
                            int iCityName = dr.GetOrdinal("adr_city");
                            int iStateName = dr.GetOrdinal("adr_state");
                            int iPostalCode = dr.GetOrdinal("adr_postal_code");
                            int iPhoneNo = dr.GetOrdinal("adr_phone");
                            int iEmail = dr.GetOrdinal("adr_email");
                            int iAddressType = dr.GetOrdinal("adr_address_type_id");

                            while (dr.Read()) {

                                state.Add(new ListAddressItem(dr.GetInt32(iAddressID), dr.GetInt32(iMemberID), dr.GetString(iShid),
                                    dr.GetBoolean(iIsSubscriber), dr.GetString(iFName),
                                    dr.GetString(iLName), dr.GetString(iBusName), dr.GetString(iTaxID),
                                    dr.GetString(iAdrLine1), dr.GetString(iAdrLine2), dr.GetString(iCityName), dr.GetString(iStateName),
                                    dr.GetString(iPostalCode), dr.GetString(iPhoneNo), dr.GetString(iEmail), dr.GetInt32(iAddressType)));
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

            return state;
        }
    }
}
