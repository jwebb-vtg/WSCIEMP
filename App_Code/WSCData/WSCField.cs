using System;
using System.Configuration;
using System.Data;
using MDAAB.Classic;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

namespace WSCData {

    /// <summary>
    /// Summary description for WSCField.
    /// </summary>
    public class WSCField {

        private const string MOD_NAME = "WSCData.WSCField.";
        private static string LF = System.Environment.NewLine;
        private static string NO_VALUE = "None";

        public delegate void ControlHostPageLoadHandler(object sender, ContractSelectorEventArgs e);
        public delegate void ContractNumberChangeHandler(object sender, ContractSelectorEventArgs e);
        public delegate void ContractNumberFindHandler(object sender, ContractSelectorEventArgs e);
        public delegate void ContractNumberPrevHandler(object sender, ContractSelectorEventArgs e);
        public delegate void ContractNumberNextHandler(object sender, ContractSelectorEventArgs e);
        public delegate void ShareholderFindHandler(object sender, ContractSelectorEventArgs e);
        public delegate void SequenceNumberChangeHandler(object sender, ContractSelectorEventArgs e);
        public delegate void FieldExceptionHandler(object sender, WSCIEMP.Common.CErrorEventArgs e);


        public static SqlDataReader ContractBrowser(SqlConnection conn, int contractID,
            int contractNumber, int cropYear, int userID) {

            const string METHOD_NAME = "ContractBrowser";
            SqlDataReader dr = null;
            
            try {

                string procName = "bawpContractBrowser";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = contractID;
                spParams[1].Value = contractNumber;
                spParams[2].Value = cropYear;
                spParams[3].Value = userID;
                //SetTimeout();

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

        public static SqlDataReader ContractBrowserNext(SqlConnection conn, int contractNumber, int cropYear, int userID) {

            const string METHOD_NAME = "ContractBrowserNext";
            SqlDataReader dr = null;

            try {

                string procName = "bawpContractBrowserNext";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = contractNumber;
                spParams[1].Value = cropYear;
                spParams[2].Value = userID;

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

        public static SqlDataReader ContractBrowserPrev(SqlConnection conn, int contractNumber, int cropYear, int userID) {

            const string METHOD_NAME = "ContractBrowserPrev";
            SqlDataReader dr = null;

            try {

                string procName = "bawpContractBrowserPrev";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = contractNumber;
                spParams[1].Value = cropYear;
                spParams[2].Value = userID;

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

        public static void CntLldGetOverview(int contractID,
            int contractNumber, int addressID, int cropYear, ref int clldContrctID,
            ref int clldContractNo, ref int clldCropYear,
            ref int clldGrowerSHID, ref string clldGrowerName,
            ref int clldMaxSequence) {

            const string METHOD_NAME = "CntLldGetOverview";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpCntLldGetOverview";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = contractID;
                    spParams[1].Value = contractNumber;
                    spParams[2].Value = addressID;
                    spParams[3].Value = cropYear;
                    SetTimeout();

                    try {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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

                    clldContrctID = Convert.ToInt32(spParams[4].Value);
                    clldContractNo = Convert.ToInt32(spParams[5].Value);
                    clldCropYear = Convert.ToInt32(spParams[6].Value);
                    clldGrowerSHID = Convert.ToInt32(spParams[7].Value);
                    clldGrowerName = spParams[8].Value.ToString();
                    clldMaxSequence = Convert.ToInt32(spParams[9].Value);
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static void LegalLandDescSave(
            int lld_lld_id, string lld_state, string lld_county, string lld_township,
            string lld_range, string lld_section, string lld_quadrant,
            string lld_quarter_quadrant, decimal lld_latitude, decimal lld_longitude,
            string lld_description, string lld_field_name, int lld_acres,
            bool lld_fsa_official, string lld_fsa_number, string lld_fsa_state, string lld_fsa_county,
            string farmNumber, string tractNumber, string fieldNumber, string quarterField,
            string UserName, ref int lld_id_out,
            bool editForceNewRecord, int cntlld_cntlld_id, int contract_id, int cropYear, ref int cntlld_cntlld_id_out) {

            const string METHOD_NAME = "LegalLandDescSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpLegalLandDescSave";
                    string procName2 = "bawpCntLldAddField";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    System.Data.SqlClient.SqlParameter[] spParams2 =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName2, false);

                    spParams[1].Value = lld_lld_id;
                    spParams[2].Value = lld_state;
                    spParams[3].Value = lld_county;
                    spParams[4].Value = lld_township;
                    spParams[5].Value = lld_range;
                    spParams[6].Value = lld_section;
                    spParams[7].Value = lld_quadrant;
                    spParams[8].Value = lld_quarter_quadrant;
                    spParams[9].Value = lld_latitude;
                    spParams[10].Value = lld_longitude;
                    spParams[11].Value = lld_description;
                    spParams[12].Value = lld_field_name;
                    spParams[13].Value = lld_acres;
                    spParams[14].Value = lld_fsa_official;
                    spParams[15].Value = lld_fsa_number;
                    spParams[16].Value = lld_fsa_state;
                    spParams[17].Value = lld_fsa_county;
                    spParams[18].Value = farmNumber;
                    spParams[19].Value = tractNumber;
                    spParams[20].Value = fieldNumber;
                    spParams[21].Value = quarterField;
                    spParams[22].Value = UserName;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {

                        try {

                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);

                            lld_id_out = Convert.ToInt32(spParams[0].Value);

                            if (editForceNewRecord && cntlld_cntlld_id > 0) {

                                spParams2[0].Value = contract_id;
                                spParams2[1].Value = lld_id_out;
                                spParams2[2].Value = cropYear;
                                spParams2[3].Value = UserName;

                                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName2, spParams2);
                                cntlld_cntlld_id_out = Convert.ToInt32(spParams2[4].Value);

                            } else {
                                if (lld_lld_id != lld_id_out) {
                                    cntlld_cntlld_id_out = 0;
                                }
                            }

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

		public static void SeedVarietySave(string factoryArea, string variety, bool isDelete) {

				const string METHOD_NAME = "SeedVarietySave";

			try {

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					string procName = "bawpSeedVarietySave";

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = factoryArea;
					spParams[1].Value = variety;
					spParams[2].Value = isDelete;

					SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
				}
			}
			catch (System.Exception e) {
				string errMsg = MOD_NAME + METHOD_NAME;
				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
				throw (wscEx);
			}
		}

		public static DataTable FactoryAreaGetAll() {

			const string METHOD_NAME = "FactoryAreaGetAll";
			DataTable dt = null;

			try {

				string procName = "bawpFactoryAreaGetAll";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					dt = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, procName).Tables[0];
				}
			}
			catch (System.Exception e) {
				string errMsg = MOD_NAME + METHOD_NAME;
				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
				throw (wscEx);
			}

			return dt;
		}

		public static DataTable SeedVarietyGetByArea(string factoryArea) {

			const string METHOD_NAME = "SeedVarietyGetByArea";
			DataTable dt= null;

			try {

				string procName = "bawpSeedVarietyGetByArea";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }

					System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);
					spParams[0].Value = factoryArea;

					dt = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, procName, spParams).Tables[0];
				}
			}
			catch (System.Exception e) {
				string errMsg = MOD_NAME + METHOD_NAME;
				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
				throw (wscEx);
			}

			return dt;
		}

        public static void LegalLandDescDelete(int lld_lld_id) {

            const string METHOD_NAME = "LegalLandDescDelete";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpLegalLandDescDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = lld_lld_id;
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

        public static void CntLldDelete(int cntlld_cntlld_id) {

            const string METHOD_NAME = "CntLldDelete";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpCntLldDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cntlld_cntlld_id;
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

        public static void CntLldAddField(int cntlld_contract_id, int cntlld_lld_id,
            int cropYear, string userName, ref int cntlld_cntlld_id_out) {

            const string METHOD_NAME = "CntLldAddField";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpCntLldAddField";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cntlld_contract_id;
                    spParams[1].Value = cntlld_lld_id;
                    spParams[2].Value = cropYear;
                    spParams[3].Value = userName;
                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            cntlld_cntlld_id_out = Convert.ToInt32(spParams[4].Value);
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

        public static List<ListBeetFieldFinderItem> FieldFind(int cropYear, string fsaNumber, string fieldName, string state,
            string county, string township, string range, string section, string quadrant, string quarterQuadrant, string description, 
            string fsaState, string fsaCounty, string farmNo, string tractNo, string fieldNo, string quarterField) {

            const string METHOD_NAME = "FieldFind";

            List<ListBeetFieldFinderItem> stateList = new List<ListBeetFieldFinderItem>();

            try {

                string procName = "bawpLLDFinder";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = fsaNumber;
                    spParams[2].Value = fieldName;
                    spParams[3].Value = state;
                    spParams[4].Value = county;
                    spParams[5].Value = township;
                    spParams[6].Value = range;
                    spParams[7].Value = section;
                    spParams[8].Value = quadrant;
                    spParams[9].Value = quarterQuadrant;
                    spParams[10].Value = description;
                    spParams[11].Value = fsaState;
                    spParams[12].Value = fsaCounty;
                    spParams[13].Value = farmNo;
                    spParams[14].Value = tractNo;
                    spParams[15].Value = fieldNo;
                    spParams[16].Value = quarterField;

                    try {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int illd_lld_id = dr.GetOrdinal("lld_lld_id");
                            int illd_state = dr.GetOrdinal("lld_state");
                            int illd_county = dr.GetOrdinal("lld_county");
                            int illd_township = dr.GetOrdinal("lld_township");
                            int illd_range = dr.GetOrdinal("lld_range");
                            int illd_section = dr.GetOrdinal("lld_section");
                            int illd_quadrant = dr.GetOrdinal("lld_quadrant");
                            int illd_quarter_quadrant = dr.GetOrdinal("lld_quarter_quadrant");
                            int illd_latitude = dr.GetOrdinal("lld_latitude");
                            int illd_longitude = dr.GetOrdinal("lld_longitude");
                            int illd_description = dr.GetOrdinal("lld_description");
                            int illd_field_name = dr.GetOrdinal("lld_field_name");
                            int illd_acres = dr.GetOrdinal("lld_acres");
                            int illd_fsa_official = dr.GetOrdinal("lld_fsa_official");
                            int illd_fsa_number = dr.GetOrdinal("lld_fsa_number");
                            int illd_fsa_state = dr.GetOrdinal("lld_fsa_state");
                            int illd_fsa_county = dr.GetOrdinal("lld_fsa_county");
                            int illd_contract_no = dr.GetOrdinal("lld_contract_no");
                            int illd_farm_number = dr.GetOrdinal("lld_farm_number");
                            int illd_tract_number = dr.GetOrdinal("lld_tract_number");
                            int illd_field_number = dr.GetOrdinal("lld_field_number");
                            int illd_quarter_field = dr.GetOrdinal("lld_quarter_field");

                            while(dr.Read()) {

                                stateList.Add(new ListBeetFieldFinderItem(dr.GetInt32(illd_lld_id),
                                    dr.GetString(illd_state), dr.GetString(illd_county), dr.GetString(illd_township),
                                    dr.GetString(illd_range), dr.GetString(illd_section), dr.GetString(illd_quadrant),
                                    dr.GetString(illd_quarter_quadrant), dr.GetDecimal(illd_latitude), dr.GetDecimal(illd_longitude),
                                    dr.GetString(illd_description), dr.GetString(illd_field_name), dr.GetInt32(illd_acres),
                                    dr.GetString(illd_fsa_official), dr.GetString(illd_fsa_number), dr.GetString(illd_fsa_state),
                                    dr.GetString(illd_fsa_county), dr.GetString(illd_contract_no), dr.GetString(illd_farm_number),
                                    dr.GetString(illd_tract_number), dr.GetString(illd_field_number), dr.GetString(illd_quarter_field)));
                            }

                            if (stateList.Count == 0) {
                                stateList.Add(new ListBeetFieldFinderItem(0, "*", "", "", "", "", "", "", 0, 0, "", "", 0,
                                    "", "", "", "", "", "", "", "", ""));
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

        public static SqlDataReader CntLldGetDetail(SqlConnection conn, int contractID, int sequenceNumber) {

            const string METHOD_NAME = "CntLldGetDetail";
            SqlDataReader dr = null;

            try {

                string procName = "bawpCntLldGetDetailByCntSeq";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = contractID;
                spParams[1].Value = sequenceNumber;
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

        public static SqlDataReader GrowerAdviceGetBySHID(SqlConnection conn, int growerPerformanceID) {

            const string METHOD_NAME = "GrowerAdviceGetBySHID";
            SqlDataReader dr = null;

            try {

                string procName = "bawpGrowerAdviceGetBySHID";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = growerPerformanceID;
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

        public static SqlDataReader CntLldGetDetailByLLD(SqlConnection conn, int lld_id, int cropYear) {

            const string METHOD_NAME = "CntLldGetDetailByLLD";
            SqlDataReader dr = null;

            try {

                string procName = "bawpLegalLandDescGetDetailByLld";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = lld_id;
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
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader GetLookupList(SqlConnection conn, string type) {
            return GetLookupList(conn, type, "Y");
        }

        public static SqlDataReader GetLookupList(SqlConnection conn, string type, string isActive) {

            const string METHOD_NAME = "GetLookupList";
            SqlDataReader dr = null;

            try {

                string procName = "bawpLookupGetByType";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = type;
                if (isActive == null) {
                    spParams[1].Value = null;
                } else {
                    spParams[1].Value = (isActive == "Y" ? 1 : 0);
                }
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
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
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static string FieldGetAgriculturist(int contractID) {

            const string METHOD_NAME = "FieldGetAgriculturist";
            string aggie = "";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpContractGetAgriculturist";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = contractID;
                    SetTimeout();

                    try {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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

                    aggie = spParams[1].Value.ToString();
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return aggie;
        }

        public static SqlDataReader FieldGetContractingData(SqlConnection conn,
            string factoryIDList, string stationIDList, string contractIDList, string fieldIDList,
            int userID) {

            const string METHOD_NAME = "FieldGetContractingData";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldGetContractingData";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                if (factoryIDList.Length >= spParams[0].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Factories in your selection.");
                    throw (wscex);
                }
                spParams[0].Value = factoryIDList;

                if (stationIDList.Length >= spParams[1].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Stations in your selection.");
                    throw (wscex);
                }
                spParams[1].Value = stationIDList;

                if (contractIDList.Length >= spParams[2].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Contracts in your selection.");
                    throw (wscex);
                }
                spParams[2].Value = contractIDList;

                if (fieldIDList.Length >= spParams[3].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Fields in your selection.");
                    throw (wscex);
                }
                spParams[3].Value = fieldIDList;
                spParams[4].Value = userID;

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

        public static SqlDataReader FieldGetContractingData2(SqlConnection conn,
            string factoryIDList, string stationIDList, string contractIDList, string SHID,
            string fromSHID, string toSHID, int cropYear) {

            const string METHOD_NAME = "FieldGetContractingData2";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldGetContractingData2";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                if (factoryIDList.Length >= spParams[0].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Factories in your selection.");
                    throw (wscex);
                }
                spParams[0].Value = factoryIDList;

                if (stationIDList.Length >= spParams[1].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Stations in your selection.");
                    throw (wscex);
                }
                spParams[1].Value = stationIDList;

                if (contractIDList.Length >= spParams[2].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Contracts in your selection.");
                    throw (wscex);
                }
                spParams[2].Value = contractIDList;

                spParams[3].Value = (SHID == "" ? 0 : Convert.ToInt32(SHID));
                spParams[4].Value = (fromSHID == "" ? 0 : Convert.ToInt32(fromSHID));
                spParams[5].Value = (toSHID == "" ? 0 : Convert.ToInt32(toSHID));
                spParams[6].Value = cropYear;

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

        public static ArrayList ShareholderSummaryContracts(SqlConnection conn, int shid, int cropYear, string regionCode, string areaCode) {

            SqlDataReader dr = null;
            ArrayList perfs = new ArrayList();
            const string METHOD_NAME = "ShareholderSummaryContracts";

            try {

                string procName = "bawpRptShareholderSummaryContracts";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = shid;
                spParams[2].Value = regionCode;
                spParams[3].Value = areaCode;
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);

                    while (dr.Read()) {
                        perfs.Add(new ContractPerformanceState(
                            dr.GetInt32(dr.GetOrdinal("rowType")),
                            dr["tSHID"].ToString(),
                            dr["tBusName"].ToString(),
                            dr["tCropYear"].ToString(),
                            dr["tFieldRegion"].ToString(),
                            dr["tFieldRegionCode"].ToString(),
                            dr["tFieldArea"].ToString(),
                            dr["tFieldAreaCode"].ToString(),
                            dr["tContractNumber"].ToString(),
                            dr["tContractStation"].ToString(),
                            dr["tFieldDescription"].ToString(),
                            dr["tLandownerName"].ToString(),
                            dr["tHarvestFinalNetTons"].ToString(),
                            dr["tTonsPerAcre"].ToString(),
                            dr["tHarvestSugarPct"].ToString(),
                            dr["tHarvestTarePct"].ToString(),
                            dr["tHarvestSLMPct"].ToString(),
                            dr["tHarvestExtractableSugar"].ToString(),
                            dr["tBeetsPerAcre"].ToString()));
                    }

                    if (perfs.Count == 0) {
                        perfs.Add(new ContractPerformanceState(0, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                    }

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

            return perfs;
        }

        public static SqlDataReader SharholderSummaryGetAreas(SqlConnection conn, int cropYear, int shid) {

            const string METHOD_NAME = "SharholderSummaryGetAreas";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRptSharholderSummaryGetAreas";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = shid.ToString();
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
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
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader FieldGetHarvestTotals(SqlConnection conn,
            int addressID, int shid, int cropYear) {

            const string METHOD_NAME = "FieldGetHarvestTotals";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldGetHarvestTotals";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = addressID;
                spParams[1].Value = shid;
                spParams[2].Value = cropYear;

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

        public static SqlDataReader FieldGetAgronomyData(SqlConnection conn,
            string factoryIDList, string stationIDList, string contractIDList, string fieldIDList,
            int userID) {

            const string METHOD_NAME = "FieldGetAgronomyData";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldGetAgronomyData";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                if (factoryIDList.Length >= spParams[0].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Factories in your selection.");
                    throw (wscex);
                }
                spParams[0].Value = factoryIDList;

                if (stationIDList.Length >= spParams[1].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Stations in your selection.");
                    throw (wscex);
                }
                spParams[1].Value = stationIDList;

                if (contractIDList.Length >= spParams[2].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Contracts in your selection.");
                    throw (wscex);
                }
                spParams[2].Value = contractIDList;

                if (fieldIDList.Length >= spParams[3].Size) {
                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning("Too many Fields in your selection.");
                    throw (wscex);
                }
                spParams[3].Value = fieldIDList;
                spParams[4].Value = userID;

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

        public static SqlDataReader FieldPerformanceGet(SqlConnection conn, int cropYear, int contractID, int shid) {

            const string METHOD_NAME = "FieldPerformanceGet";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldPerformanceGet";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = contractID;
                spParams[2].Value = shid;
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

        public static SqlDataReader FieldPerformance2Get(SqlConnection conn, int contractID) {

            const string METHOD_NAME = "FieldPerformance2Get";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldPerformance2Get";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

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
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader FieldPerformance2GetYear(SqlConnection conn, int cropYear) {

            const string METHOD_NAME = "FieldPerformance2GetYear";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldPerformance2GetYear";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
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

        public static SqlDataReader FieldGetDetailByCntLLD(SqlConnection conn, int cntlld_id) {

            const string METHOD_NAME = "FieldGetDetailByCntLLD";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldGetDetail";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cntlld_id;
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

        public static ArrayList FieldGetByGrower(SqlConnection conn, int shid, int cropYear) {

            SqlDataReader dr = null;
            ArrayList fields = new ArrayList();
            const string METHOD_NAME = "FieldGetByGrower";

            try {

                string procName = "bawpFieldGetByGrower";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = shid;
                spParams[1].Value = cropYear;
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);

                    while (dr.Read()) {
                        fields.Add(new ContractFieldState(dr.GetInt32(dr.GetOrdinal("FieldID")),
                            dr["FieldName"].ToString(), dr["FieldAcres"].ToString(),
                            dr["ContractNumber"].ToString(), dr["FieldState"].ToString(),
                            dr["FieldCounty"].ToString(), dr["FieldTownship"].ToString(),
                            dr["FieldRange"].ToString(), dr["FieldSection"].ToString(),
                            dr["FieldQuadrant"].ToString(), dr["FieldQuarterQuadrant"].ToString(),
                            dr["FieldFarmNumber"].ToString(), dr["FieldNumber"].ToString(),
                            dr["FieldTractNumber"].ToString(), dr["FieldDescription"].ToString()));
                    }

                    if (fields.Count == 0) {
                        fields.Add(new ContractFieldState(0, "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                    }

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

            return fields;
        }

        public static ArrayList SoilSampleLabGetByGrower(SqlConnection conn, int shid, int cropYear) {

            SqlDataReader dr = null;
            ArrayList labs = new ArrayList();
            const string METHOD_NAME = "SoilSampleLabGetByGrower";

            try {

                string procName = "bawpSoilSampleLabGetByGrower";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = shid;
                spParams[1].Value = cropYear;
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);

                    while (dr.Read()) {

                        labs.Add(new SoilSampleLabState(dr.GetInt32(dr.GetOrdinal("SoilSampleLabID")),
                            dr["ContractNumber"].ToString(), dr["FieldName"].ToString(),
                            dr.GetInt32(dr.GetOrdinal("CropYear")),
                            dr["AccountNumber"].ToString(), dr["LabNumber"].ToString(),
                            dr["AssocNumber"].ToString(), dr["SubmittedBy"].ToString(),
                            dr["Address1"].ToString(), dr["Address2"].ToString(),
                            dr["CityStateZip"].ToString(), dr["Grower"].ToString(),
                            dr["DateReceived"].ToString(), dr["DateReported"].ToString()));
                    }

                    //					if (labs.Count == 0) {						
                    //						labs.Add(new SoilSampleLabState(0, "", "", 0, "", "", "", "", "", "", "", "", "", ""));
                    //					}

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

            return labs;
        }

        public static ArrayList SoilSampleLabGetByField(SqlConnection conn, int fieldID, int cropYear) {

            SqlDataReader dr = null;
            ArrayList labs = new ArrayList();
            const string METHOD_NAME = "SoilSampleLabGetByField";

            try {

                string procName = "bawpSoilSampleLabGetByField";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = fieldID;
                spParams[1].Value = cropYear;
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);

                    while (dr.Read()) {

                        labs.Add(new SoilSampleLabState(dr.GetInt32(dr.GetOrdinal("SoilSampleLabID")),
                            dr["ContractNumber"].ToString(), dr["FieldName"].ToString(),
                            dr.GetInt32(dr.GetOrdinal("CropYear")),
                            dr["AccountNumber"].ToString(), dr["LabNumber"].ToString(),
                            dr["AssocNumber"].ToString(), dr["SubmittedBy"].ToString(),
                            dr["Address1"].ToString(), dr["Address2"].ToString(),
                            dr["CityStateZip"].ToString(), dr["Grower"].ToString(),
                            dr["DateReceived"].ToString(), dr["DateReported"].ToString()));
                    }

                    //					if (labs.Count == 0) {						
                    //						labs.Add(new SoilSampleLabState(0, "", "", 0, "", "", "", "", "", "", "", "", "", ""));
                    //					}

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

            return labs;
        }

        public static void FieldSampleLabSave(SqlConnection conn, int fieldID, int cropYear, int soilSampleLabID) {

            const string METHOD_NAME = "FieldSampleLabSave";

            try {

                string procName = "bawpFieldSampleLabSave";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = fieldID;
                spParams[1].Value = cropYear;
                spParams[2].Value = soilSampleLabID;
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

        public static void FieldSampleLabRemove(SqlConnection conn, int soilSampleLabID) {

            const string METHOD_NAME = "FieldSampleLabSave";

            try {

                string procName = "bawpFieldSampleLabRemove";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = soilSampleLabID;
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

        public static SqlDataReader SoilSampleLabGetDetail(SqlConnection conn, ArrayList soilSampleIDList) {

            const string METHOD_NAME = "SoilSampleLabGetDetail";
            SqlDataReader dr = null;

            try {

                string procName = "bawpSoilSampleLabGetDetail";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (string item in soilSampleIDList) {
                    sb.Append(item);
                    sb.Append(",");
                }
                sb.Length = sb.Length - 1;
                string sampleList = sb.ToString();

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = sampleList;
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

        public static Domain GetDomainData() {

            const string METHOD_NAME = "GetDomainData";
            Domain domain = null;
            try {

                domain = (Domain)AppDomain.CurrentDomain.GetData("Domain");
                if (domain == null) {

                    domain = new Domain();
                    domain.Load(WSCIEMP.Common.AppHelper.AppPath() + @"ZHost/XML/FieldDomain.xml");
                    AppDomain.CurrentDomain.SetData("Domain", domain);
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
            return domain;
        }

        public static void GrowerAdviceSave(SqlConnection conn, int growerPerformanceID, string GoodFertilityManagement,
            string TextFertilityManagement, string GoodIrrigationManagement, string TextIrrigationManagement, string GoodStandEstablishment,
            string TextStandEstablishment, string GoodWeedControl, string TextWeedControl, string GoodDiseaseControl, string TextDiseaseControl,
            string GoodVarietySelection, string TextVarietySelection, string UserName) {

            const string METHOD_NAME = "GrowerAdviceSave";

            try {

                string procName = "bawpGrowerAdviceSave";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = growerPerformanceID;
                spParams[1].Value = GoodFertilityManagement;
                spParams[2].Value = TextFertilityManagement;
                spParams[3].Value = GoodIrrigationManagement;
                spParams[4].Value = TextIrrigationManagement;
                spParams[5].Value = GoodStandEstablishment;
                spParams[6].Value = TextStandEstablishment;
                spParams[7].Value = GoodWeedControl;
                spParams[8].Value = TextWeedControl;
                spParams[9].Value = GoodDiseaseControl;
                spParams[10].Value = TextDiseaseControl;
                spParams[11].Value = GoodVarietySelection;
                spParams[12].Value = TextVarietySelection;
                spParams[13].Value = UserName;

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

        public static RptAgronomy GetAgronomyData() {

            const string METHOD_NAME = "GetAgronomyData";
            RptAgronomy agronomy = null;
            try {

				System.Web.Caching.Cache cache = System.Web.HttpContext.Current.Cache;
                agronomy = cache["RptAgronomy"] as RptAgronomy;
                if (agronomy == null) {

                    agronomy = new RptAgronomy();
					//agronomy.Load(WSCI.Common.AppHelper.AppPath() + @"ZHost/XML/RptAgronomy.xml");
					agronomy.Load();
                    //AppDomain.CurrentDomain.SetData("RptAgronomy", agronomy);
					cache.Insert("RptAgronomy", agronomy, null, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
            return agronomy;
        }

        public static void FieldContractingSave(int fld_field_id,
            string fld_rotation_len, string fld_prev_crop_type, string fld_beet_years,
            bool fld_suspect_rhizomania, bool fld_suspect_aphanomyces, bool fld_suspect_curly_top,
            bool fld_suspect_fusarium, bool fld_suspect_rhizoctonia, bool fld_suspect_nematode,
            bool fld_suspect_cercospora, bool fld_suspect_root_aphid, bool fld_suspect_powdery_mildew,
            string fld_post_Aphanomyces, string fld_post_Cercospora, string fld_post_CurlyTop,
            string fld_post_Fusarium, string fld_post_Nematode, string fld_post_PowderyMildew,
            string fld_post_Rhizoctonia, string fld_post_Rhizomania, string fld_post_RootAphid,
            bool fld_post_water, string fld_irrigation_source, string fld_irrigation_method,
            string fld_ownership, string fld_tillage, string UserName) {

            const string METHOD_NAME = "FieldContractingSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpFieldContractingSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = fld_field_id;
                    spParams[1].Value = (fld_rotation_len == "0" ? null : fld_rotation_len);
                    spParams[2].Value = (fld_prev_crop_type == NO_VALUE ? null : fld_prev_crop_type);
                    spParams[3].Value = (fld_beet_years == "0" ? null : fld_beet_years);
                    spParams[4].Value = fld_suspect_rhizomania;
                    spParams[5].Value = fld_suspect_aphanomyces;
                    spParams[6].Value = fld_suspect_curly_top;
                    spParams[7].Value = fld_suspect_fusarium;
                    spParams[8].Value = fld_suspect_rhizoctonia;
                    spParams[9].Value = fld_suspect_nematode;
                    spParams[10].Value = fld_suspect_cercospora;
                    spParams[11].Value = fld_suspect_root_aphid;
                    spParams[12].Value = fld_suspect_powdery_mildew;
                    spParams[13].Value = (fld_irrigation_source == NO_VALUE ? null : fld_irrigation_source);
                    spParams[14].Value = (fld_irrigation_method == NO_VALUE ? null : fld_irrigation_method);
                    spParams[15].Value = UserName;

                    spParams[16].Value = (fld_post_Aphanomyces == NO_VALUE ? null : fld_post_Aphanomyces);
                    spParams[17].Value = (fld_post_Cercospora == NO_VALUE ? null : fld_post_Cercospora);
                    spParams[18].Value = (fld_post_CurlyTop == NO_VALUE ? null : fld_post_CurlyTop);
                    spParams[19].Value = (fld_post_Fusarium == NO_VALUE ? null : fld_post_Fusarium);
                    spParams[20].Value = (fld_post_Nematode == NO_VALUE ? null : fld_post_Nematode);
                    spParams[21].Value = (fld_post_PowderyMildew == NO_VALUE ? null : fld_post_PowderyMildew);
                    spParams[22].Value = (fld_post_Rhizoctonia == NO_VALUE ? null : fld_post_Rhizoctonia);
                    spParams[23].Value = (fld_post_Rhizomania == NO_VALUE ? null : fld_post_Rhizomania);
                    spParams[24].Value = (fld_post_RootAphid == NO_VALUE ? null : fld_post_RootAphid);
                    spParams[25].Value = fld_post_water;

                    spParams[26].Value = (fld_ownership == NO_VALUE ? null : fld_ownership);
                    spParams[27].Value = (fld_tillage == NO_VALUE ? null : fld_tillage);

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

        public static void FieldAgronomySave(ref int fld_field_id_OUT, int fld_field_id,
            string fld_seed_variety, string fld_seed_primed,
            string fld_seed_treatment_chemical, string fld_row_spacing, string fld_planting_date,
            string fld_plant_spacing, string fld_replant_date, string fld_replant_seed_variety,
            string fld_acres_replanted, string fld_acres_lost, string fld_replant_reason, string fld_lost_reason,
            string fld_test_season, string fld_test_depth,
            string fld_test_N, string fld_test_P, string fld_test_K,
            string fld_test_pH, string fld_test_org_mat, string fld_last_yr_manure,
            string fld_fert_fal_N, string fld_fert_fal_P, string fld_fert_fal_K,
            string fld_fert_spr_N, string fld_fert_spr_P, string fld_fert_spr_K,
            string fld_fert_ins_N, string fld_fert_ins_P, string fld_fert_ins_K,
            string fld_fert_starter, string fld_pre_insecticide, string fld_post_insectcide,
            string fld_pre_weed_ctrl, string fld_layby_herbicide, string fld_layby_herbicide_chemical,
            string fld_root_maggot_insecticide, string fld_rootm_counter_lbs, string fld_rootm_temik_lbs,
            string fld_rootm_thimet_lbs, string fld_cercsp_app1_chemical, string fld_cercospora_app1_date,
            string fld_cercsp_app2_chemical, string fld_cercospora_app2_date, string fld_cercsp_app3_chemical,
            string fld_cercospora_app3_date, string fld_hail_stress, string fld_weed_control,
            string fld_treated_powdery_mildew, string fld_treated_nematode, string fld_treated_rhizoctonia,
            string fld_reviewed, string fld_grid_zone, string fld_include, string fld_add_user,
            string fld_soil_texture, string fld_test_salts, string fld_herbicide_rx_count, string fld_emerg_80_date,
            string fld_comment) {

            const string METHOD_NAME = "FieldAgronomySave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpFieldAgronomySave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[1].Value = fld_field_id;

                    if (fld_seed_variety.Length == 0 || fld_seed_variety == NO_VALUE) {
                        spParams[2].Value = null;
                    } else { spParams[2].Value = fld_seed_variety; }

                    if (fld_seed_primed.Length == 0 || fld_seed_primed == NO_VALUE) {
                        spParams[3].Value = null;
                    } else { spParams[3].Value = fld_seed_primed; }

                    if (fld_seed_treatment_chemical.Length == 0 || fld_seed_treatment_chemical == NO_VALUE) {
                        spParams[4].Value = null;
                    } else { spParams[4].Value = fld_seed_treatment_chemical; }

                    if (fld_row_spacing.Length == 0 || fld_row_spacing == "0") {
                        spParams[5].Value = null;
                    } else { spParams[5].Value = Convert.ToInt16(fld_row_spacing); }

                    if (fld_planting_date.Length == 0) {
                        spParams[6].Value = null;
                    } else { spParams[6].Value = fld_planting_date; }

                    if (fld_plant_spacing.Length == 0 || fld_plant_spacing == "0") {
                        spParams[7].Value = null;
                    } else { spParams[7].Value = Convert.ToDecimal(fld_plant_spacing); }

                    if (fld_replant_date.Length == 0) {
                        spParams[8].Value = null;
                    } else { spParams[8].Value = fld_replant_date; }

                    if (fld_replant_seed_variety.Length == 0 || fld_replant_seed_variety == NO_VALUE) {
                        spParams[9].Value = null;
                    } else { spParams[9].Value = fld_replant_seed_variety; }

                    if (fld_acres_replanted.Length == 0) {
                        spParams[10].Value = null;
                    } else { spParams[10].Value = Convert.ToInt32(fld_acres_replanted); }

                    if (fld_test_season.Length == 0 || fld_test_season == NO_VALUE) {
                        spParams[11].Value = null;
                    } else { spParams[11].Value = fld_test_season; }

                    if (fld_test_depth.Length == 0 || fld_test_depth == "0") {
                        spParams[12].Value = null;
                    } else { spParams[12].Value = Convert.ToInt16(fld_test_depth); }

                    if (fld_test_N.Length == 0) {
                        spParams[13].Value = null;
                    } else { spParams[13].Value = Convert.ToDecimal(fld_test_N); }

                    if (fld_test_P.Length == 0 || fld_test_P == NO_VALUE) {
                        spParams[14].Value = null;
                    } else { spParams[14].Value = fld_test_P; }

                    if (fld_test_K.Length == 0) {
                        spParams[15].Value = null;
                    } else { spParams[15].Value = Convert.ToDecimal(fld_test_K); }

                    if (fld_test_pH.Length == 0) {
                        spParams[16].Value = null;
                    } else { spParams[16].Value = Convert.ToDecimal(fld_test_pH); }

                    if (fld_test_org_mat.Length == 0) {
                        spParams[17].Value = null;
                    } else { spParams[17].Value = Convert.ToDecimal(fld_test_org_mat); }

                    if (fld_last_yr_manure.Length == 0 || fld_last_yr_manure == NO_VALUE) {
                        spParams[18].Value = null;
                    } else { spParams[18].Value = Convert.ToInt32(fld_last_yr_manure); }

                    if (fld_fert_fal_N.Length == 0) {
                        spParams[19].Value = null;
                    } else { spParams[19].Value = Convert.ToDecimal(fld_fert_fal_N); }

                    if (fld_fert_fal_P.Length == 0) {
                        spParams[20].Value = null;
                    } else { spParams[20].Value = Convert.ToDecimal(fld_fert_fal_P); }

                    if (fld_fert_fal_K.Length == 0) {
                        spParams[21].Value = null;
                    } else { spParams[21].Value = Convert.ToDecimal(fld_fert_fal_K); }

                    if (fld_fert_spr_N.Length == 0) {
                        spParams[22].Value = null;
                    } else { spParams[22].Value = Convert.ToDecimal(fld_fert_spr_N); }

                    if (fld_fert_spr_P.Length == 0) {
                        spParams[23].Value = null;
                    } else { spParams[23].Value = Convert.ToDecimal(fld_fert_spr_P); }

                    if (fld_fert_spr_K.Length == 0) {
                        spParams[24].Value = null;
                    } else { spParams[24].Value = Convert.ToDecimal(fld_fert_spr_K); }

                    if (fld_fert_ins_N.Length == 0) {
                        spParams[25].Value = null;
                    } else { spParams[25].Value = Convert.ToDecimal(fld_fert_ins_N); }

                    if (fld_fert_ins_P.Length == 0) {
                        spParams[26].Value = null;
                    } else { spParams[26].Value = Convert.ToDecimal(fld_fert_ins_P); }

                    if (fld_fert_ins_K.Length == 0) {
                        spParams[27].Value = null;
                    } else { spParams[27].Value = Convert.ToDecimal(fld_fert_ins_K); }

                    if (fld_fert_starter.Length == 0) {
                        spParams[28].Value = null;
                    } else { spParams[28].Value = (fld_fert_starter == "Yes" ? 1 : 0); }

                    if (fld_pre_insecticide.Length == 0) {
                        spParams[29].Value = null;
                    } else { spParams[29].Value = (fld_pre_insecticide == "Yes" ? 1 : 0); }

                    if (fld_post_insectcide.Length == 0) {
                        spParams[30].Value = null;
                    } else { spParams[30].Value = (fld_post_insectcide == "Yes" ? 1 : 0); }

                    if (fld_pre_weed_ctrl.Length == 0) {
                        spParams[31].Value = null;
                    } else { spParams[31].Value = (fld_pre_weed_ctrl == "Yes" ? 1 : 0); }

                    if (fld_layby_herbicide.Length == 0) {
                        spParams[32].Value = null;
                    } else { spParams[32].Value = (fld_layby_herbicide == "Yes" ? 1 : 0); }

                    if (fld_layby_herbicide_chemical.Length == 0 || fld_layby_herbicide_chemical == NO_VALUE) {
                        spParams[33].Value = null;
                    } else { spParams[33].Value = fld_layby_herbicide_chemical; }

                    if (fld_root_maggot_insecticide.Length == 0) {
                        spParams[34].Value = null;
                    } else { spParams[34].Value = (fld_root_maggot_insecticide == "Yes" ? 1 : 0); }

                    if (fld_rootm_counter_lbs.Length == 0) {
                        spParams[35].Value = null;
                    } else { spParams[35].Value = Convert.ToDecimal(fld_rootm_counter_lbs); }

                    if (fld_rootm_temik_lbs.Length == 0) {
                        spParams[36].Value = null;
                    } else { spParams[36].Value = Convert.ToDecimal(fld_rootm_temik_lbs); }

                    if (fld_rootm_thimet_lbs.Length == 0) {
                        spParams[37].Value = null;
                    } else { spParams[37].Value = Convert.ToDecimal(fld_rootm_thimet_lbs); }

                    if (fld_cercsp_app1_chemical.Length == 0 || fld_cercsp_app1_chemical == NO_VALUE) {
                        spParams[38].Value = null;
                    } else { spParams[38].Value = fld_cercsp_app1_chemical; }

                    if (fld_cercospora_app1_date.Length == 0) {
                        spParams[39].Value = null;
                    } else { spParams[39].Value = fld_cercospora_app1_date; }

                    if (fld_cercsp_app2_chemical.Length == 0 || fld_cercsp_app2_chemical == NO_VALUE) {
                        spParams[40].Value = null;
                    } else { spParams[40].Value = fld_cercsp_app2_chemical; }

                    if (fld_cercospora_app2_date.Length == 0) {
                        spParams[41].Value = null;
                    } else { spParams[41].Value = fld_cercospora_app2_date; }

                    if (fld_cercsp_app3_chemical.Length == 0 || fld_cercsp_app3_chemical == NO_VALUE) {
                        spParams[42].Value = null;
                    } else { spParams[42].Value = fld_cercsp_app3_chemical; }

                    if (fld_cercospora_app3_date.Length == 0) {
                        spParams[43].Value = null;
                    } else { spParams[43].Value = fld_cercospora_app3_date; }

                    if (fld_hail_stress.Length == 0 || fld_hail_stress == NO_VALUE) {
                        spParams[44].Value = null;
                    } else { spParams[44].Value = fld_hail_stress; }

                    if (fld_weed_control.Length == 0 || fld_weed_control == NO_VALUE) {
                        spParams[45].Value = null;
                    } else { spParams[45].Value = fld_weed_control; }

                    if (fld_treated_powdery_mildew.Length == 0) {
                        spParams[46].Value = null;
                    } else { spParams[46].Value = (fld_treated_powdery_mildew == "Yes" ? 1 : 0); }

                    if (fld_treated_nematode.Length == 0) {
                        spParams[47].Value = null;
                    } else { spParams[47].Value = (fld_treated_nematode == "Yes" ? 1 : 0); }

                    if (fld_treated_rhizoctonia.Length == 0) {
                        spParams[48].Value = null;
                    } else { spParams[48].Value = (fld_treated_rhizoctonia == "Yes" ? 1 : 0); }

                    if (fld_reviewed.Length == 0) {
                        spParams[49].Value = null;
                    } else { spParams[49].Value = (fld_reviewed == "Yes" ? 1 : 0); }

                    if (fld_include.Length == 0) {
                        spParams[50].Value = null;
                    } else { spParams[50].Value = (fld_include == "Yes" ? 1 : 0); }

                    spParams[51].Value = fld_add_user;

                    if (fld_replant_reason.Length == 0 || fld_replant_reason == NO_VALUE) {
                        spParams[52].Value = null;
                    } else { spParams[52].Value = fld_replant_reason; }

                    if (fld_lost_reason.Length == 0 || fld_lost_reason == NO_VALUE) {
                        spParams[53].Value = null;
                    } else { spParams[53].Value = fld_lost_reason; }

                    if (fld_acres_lost.Length == 0) {
                        spParams[54].Value = null;
                    } else { spParams[54].Value = Convert.ToInt32(fld_acres_lost); }

                    if (fld_grid_zone.Length == 0) {
                        spParams[55].Value = null;
                    } else { spParams[55].Value = (fld_grid_zone == "Yes" ? 1 : 0); }

                    if (fld_soil_texture.Length == 0 || fld_soil_texture == NO_VALUE) {
                        spParams[56].Value = null;
                    } else { spParams[56].Value = fld_soil_texture; }

                    if (fld_test_salts.Length == 0) {
                        spParams[57].Value = null;
                    } else { spParams[57].Value = Convert.ToDecimal(fld_test_salts); }

                    if (fld_herbicide_rx_count.Length == 0) {
                        spParams[58].Value = null;
                    } else { spParams[58].Value = Convert.ToInt32(fld_herbicide_rx_count); }

                    if (fld_emerg_80_date.Length == 0) {
                        spParams[59].Value = null;
                    } else { spParams[59].Value = fld_emerg_80_date; }

                    if (fld_comment.Length == 0) {
                        spParams[60].Value = null;
                    } else { spParams[60].Value = fld_comment; }

                    SetTimeout();

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            fld_field_id_OUT = Convert.ToInt32(spParams[0].Value);
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

        public static SqlDataReader GetContractCards(SqlConnection conn,
            int cropYear, int fromContractNo, int toContractNo) {

            const string METHOD_NAME = "GetContractCards";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRPTContractCardGenerator";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = fromContractNo;
                spParams[2].Value = toContractNo;
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

        public static SqlDataReader GetTonsByTruckByContract(SqlConnection conn,
            int cropYear, int shid, string contractNumbers, bool isCSV) {

            const string METHOD_NAME = "GetTonsByTruckByContract";
            SqlDataReader dr = null;

            try {

                string procName = "s70_RPT_GrowerDetailExport";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = shid;
                spParams[2].Value = contractNumbers;
                spParams[3].Value = (isCSV ? 1 : 0);
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

        public static ArrayList GetCropYears() {

            const string METHOD_NAME = "GetCropYears";
            ArrayList cropYears = new ArrayList();

            try {

                string strCropYears = Globals.BeetCropYears;
                if (strCropYears.Length == 0) {

                    string procName = "bawpFactoryGetCropYears";

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        conn.Open();
                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName)) {
                            while (dr.Read()) {
                                cropYears.Add(dr.GetInt32(dr.GetOrdinal("CropYear")).ToString());
                            }
                        }
                    }
                    foreach (string s in cropYears) {
                        if (strCropYears.Length > 0) {
                            strCropYears += "," + s;
                        } else {
                            strCropYears = s;
                        }
                    }

                    Globals.BeetCropYears = strCropYears;

                } else {
                    string[] cys = strCropYears.Split(new char[] {','});
                    cropYears.AddRange(cys);
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return cropYears;
        }

        public static ArrayList GrowerPerformanceShidsByRange(SqlConnection conn, int cropYear, string fromShid, string toShid) {

            const string METHOD_NAME = "GrowerPerformanceShidsByRange";
            SqlDataReader dr = null;
            ArrayList alst = new ArrayList();

            try {

                string procName = "bawpGrowerPerformanceShidsByRange";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = fromShid;
                spParams[2].Value = toShid;
                SetTimeout();

                try {

                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);

                    while (dr.Read()) {
                        alst.Add(dr.GetString(0));
                    }
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

            return alst;
        }

        public static SqlDataReader SeedVarietyDetailGetByResistance(SqlConnection conn,
            string disease1, string wantResis1, string disease2, string wantResis2,
            string disease3, string wantResis3, string disease4, string wantResis4,
            string disease5, string wantResis5, string disease6, string wantResis6, string prodArea, string orderCol) {

            const string METHOD_NAME = "SeedVarietyDetailGetByResistance";
            SqlDataReader dr = null;

            try {

                string procName = "bawpSeedVarietyDetailGetByResistance";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                if (disease1 != "") {
                    spParams[0].Value = disease1;
                } else {
                    spParams[0].Value = DBNull.Value;
                }
                if (wantResis1 != "") {
                    spParams[1].Value = wantResis1;
                } else {
                    spParams[1].Value = DBNull.Value;
                }
                if (disease2 != "") {
                    spParams[2].Value = disease2;
                } else {
                    spParams[2].Value = DBNull.Value;
                }
                if (wantResis2 != "") {
                    spParams[3].Value = wantResis2;
                } else {
                    spParams[3].Value = DBNull.Value;
                }
                if (disease3 != "") {
                    spParams[4].Value = disease3;
                } else {
                    spParams[4].Value = DBNull.Value;
                }
                if (wantResis3 != "") {
                    spParams[5].Value = wantResis3;
                } else {
                    spParams[5].Value = DBNull.Value;
                }
                if (disease4 != "") {
                    spParams[6].Value = disease4;
                } else {
                    spParams[6].Value = DBNull.Value;
                }
                if (wantResis4 != "") {
                    spParams[7].Value = wantResis4;
                } else {
                    spParams[7].Value = DBNull.Value;
                }
                if (disease5 != "") {
                    spParams[8].Value = disease5;
                } else {
                    spParams[8].Value = DBNull.Value;
                }
                if (wantResis5 != "") {
                    spParams[9].Value = wantResis5;
                } else {
                    spParams[9].Value = DBNull.Value;
                }
                if (disease6 != "") {
                    spParams[10].Value = disease6;
                } else {
                    spParams[10].Value = DBNull.Value;
                }
                if (wantResis6 != "") {
                    spParams[11].Value = wantResis6;
                } else {
                    spParams[11].Value = DBNull.Value;
                }
                spParams[12].Value = prodArea;
                spParams[13].Value = orderCol;

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

        public static void FillCropYear(System.Web.UI.WebControls.DropDownList ddl, string defaultText) {

            const string METHOD_NAME = "FillCropYear";
            try {

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

        private static void SetTimeout(int timeout) {
            int timeOut = timeout;
            SqlHelper.CommandTimeout = timeOut.ToString();
        }

        private static void SetTimeout() {
            int timeOut = Convert.ToInt32(ConfigurationManager.AppSettings["sql.command.timeout"].ToString());
            SqlHelper.CommandTimeout = timeOut.ToString();
        }
    }

    public class ContractSelectorEventArgs : System.EventArgs {

        private System.Exception _error = null;

        public ContractSelectorEventArgs() { }

        public ContractSelectorEventArgs(System.Exception ex) {
            _error = ex;
        }

        public System.Exception Error() {
            return _error;
        }
    }

    [Serializable]
    public class ContractFieldState {

        private int _fieldID;
        private string _fieldName;
        private string _fieldAcres;
        private string _contractNumber;
        private string _fieldState;
        private string _fieldCounty;
        private string _fieldTownship;
        private string _fieldRange;
        private string _fieldSection;
        private string _fieldQuadrant;
        private string _fieldQuarterQuadrant;
        private string _fieldFarmNumber;
        private string _fieldNumber;
        private string _fieldTractNumber;
        private string _fieldDescription;


        public int FieldID { get { return _fieldID; } }
        public string FieldName { get { return _fieldName; } }
        public string FieldAcres { get { return _fieldAcres; } }
        public string ContractNumber { get { return _contractNumber; } }
        public string FieldState { get { return _fieldState; } }
        public string FieldCounty { get { return _fieldCounty; } }
        public string FieldTownship { get { return _fieldTownship; } }
        public string FieldRange { get { return _fieldRange; } }
        public string FieldSection { get { return _fieldSection; } }
        public string FieldQuadrant { get { return _fieldQuadrant; } }
        public string FieldQuarterQuadrant { get { return _fieldQuarterQuadrant; } }
        public string FieldFarmNumber { get { return _fieldFarmNumber; } }
        public string FieldNumber { get { return _fieldNumber; } }
        public string FieldTractNumber { get { return _fieldTractNumber; } }
        public string FieldDescription { get { return _fieldDescription; } }

        public ContractFieldState(int fieldID, string fieldName, string fieldAcres, string contractNumber, string fieldState,
            string fieldCounty, string fieldTownship, string fieldRange, string fieldSection, string fieldQuadrant,
            string fieldQuarterQuadrant, string fieldFarmNumber, string fieldNumber, string fieldTractNumber, string fieldDescription) {

            _fieldID = fieldID;
            _fieldName = fieldName;
            _fieldAcres = fieldAcres;
            _contractNumber = contractNumber;
            _fieldState = fieldState;
            _fieldCounty = fieldCounty;
            _fieldTownship = fieldTownship;
            _fieldRange = fieldRange;
            _fieldSection = fieldSection;
            _fieldQuadrant = fieldQuadrant;
            _fieldQuarterQuadrant = fieldQuarterQuadrant;
            _fieldFarmNumber = fieldFarmNumber;
            _fieldNumber = fieldNumber;
            _fieldTractNumber = fieldTractNumber;
            _fieldDescription = fieldDescription;

        }
    }

    [Serializable]
    public class ContractPerformanceState {

        private int _rowType = 0;
        private string _tSHID = "";
        private string _tBusName = "";
        private string _tCropYear = "";
        private string _tFieldRegion = "";
        private string _tFieldRegionCode = "";
        private string _tFieldArea = "";
        private string _tFieldAreaCode = "";
        private string _tContractNumber = "";
        private string _tContractStation = "";
        private string _tFieldDescription = "";
        private string _tLandownerName = "";
        private string _tHarvestFinalNetTons = "";
        private string _tTonsPerAcre = "";
        private string _tHarvestSugarPct = "";
        private string _tHarvestTarePct = "";
        private string _tHarvestSLMPct = "";
        private string _tHarvestExtractableSugar = "";
        private string _tBeetsPerAcre = "";

        public int RowType { get { return _rowType; } }
        public string SHID { get { return _tSHID; } }
        public string BusName { get { return _tBusName; } }
        public string CropYear { get { return _tCropYear; } }
        public string FieldRegion { get { return _tFieldRegion; } }
        public string FieldRegionCode { get { return _tFieldRegionCode; } }
        public string FieldArea { get { return _tFieldArea; } }
        public string FieldAreaCode { get { return _tFieldAreaCode; } }
        public string ContractNumber { get { return _tContractNumber; } }
        public string ContractStation { get { return _tContractStation; } }
        public string FieldDescription { get { return _tFieldDescription; } }
        public string LandownerName { get { return _tLandownerName; } }
        public string HarvestFinalNetTons { get { return _tHarvestFinalNetTons; } }
        public string TonsPerAcre { get { return _tTonsPerAcre; } }
        public string HarvestSugarPct { get { return _tHarvestSugarPct; } }
        public string HarvestTarePct { get { return _tHarvestTarePct; } }
        public string HarvestSLMPct { get { return _tHarvestSLMPct; } }
        public string HarvestExtractableSugar { get { return _tHarvestExtractableSugar; } }
        public string BeetsPerAcre { get { return _tBeetsPerAcre; } }

        public ContractPerformanceState(int rowType, string tSHID, string tBusName, string tCropYear,
            string tFieldRegion, string tFieldRegionCode, string tFieldArea, string tFieldAreaCode,
            string tContractNumber, string tContractStation, string tFieldDescription, string tLandownerName,
            string tHarvestFinalNetTons, string tTonsPerAcre, string tHarvestSugarPct, string tHarvestTarePct,
            string tHarvestSLMPct, string tHarvestExtractableSugar, string tBeetsPerAcre) {

            _rowType = rowType;
            _tSHID = tSHID;
            _tBusName = tBusName;
            _tCropYear = tCropYear;
            _tFieldRegion = tFieldRegion;
            _tFieldRegionCode = tFieldRegionCode;
            _tFieldArea = tFieldArea;
            _tFieldAreaCode = tFieldAreaCode;
            _tContractNumber = tContractNumber;
            _tContractStation = tContractStation;
            _tFieldDescription = tFieldDescription;
            _tLandownerName = tLandownerName;
            _tHarvestFinalNetTons = tHarvestFinalNetTons;

            if (rowType < 4) {
                _tTonsPerAcre = tTonsPerAcre;
                _tHarvestSugarPct = tHarvestSugarPct;
                _tHarvestTarePct = tHarvestTarePct;
                _tHarvestSLMPct = tHarvestSLMPct;
                _tHarvestExtractableSugar = tHarvestExtractableSugar;
                _tBeetsPerAcre = tBeetsPerAcre;
            } else {

                // rowType 4 and rowType 5 are rankings
                string totParts = Convert.ToInt32(Convert.ToDecimal(tHarvestFinalNetTons)).ToString();
                _tTonsPerAcre = Convert.ToInt32(Convert.ToDecimal(tTonsPerAcre)).ToString() + " of " + totParts;
                _tHarvestSugarPct = Convert.ToInt32(Convert.ToDecimal(tHarvestSugarPct)).ToString() + " of " + totParts;
                _tHarvestTarePct = Convert.ToInt32(Convert.ToDecimal(tHarvestTarePct)).ToString() + " of " + totParts;
                _tHarvestSLMPct = Convert.ToInt32(Convert.ToDecimal(tHarvestSLMPct)).ToString() + " of " + totParts;
                _tHarvestExtractableSugar = Convert.ToInt32(Convert.ToDecimal(tHarvestExtractableSugar)).ToString() + " of " + totParts;
                _tBeetsPerAcre = Convert.ToInt32(Convert.ToDecimal(tBeetsPerAcre)).ToString() + " of " + totParts;
            }
        }
    }

    [Serializable]
    public class SoilSampleLabState {

        //private string MOD_NAME = "WSCData.SoilSampleLabState.";

        private int _soilSampleLabID;
        private string _contractNumber;
        private string _fieldName;
        private string _cropYear;
        private string _accountNumber;
        private string _labNumber;
        private string _assocNumber;
        private string _submittedBy;
        private string _address1;
        private string _address2;
        private string _city_state_zip;
        private string _grower;
        private string _dateReceived;
        private string _dateReported;

        public int SoilSampleLabID { get { return _soilSampleLabID; } }
        public string ContractNumber { get { return _contractNumber; } }
        public string FieldName { get { return _fieldName; } }
        public string CropYear { get { return _cropYear; } }
        public string AccountNumber { get { return _accountNumber; } }
        public string LabNumber { get { return _labNumber; } }
        public string AssocNumber { get { return _assocNumber; } }
        public string SubmittedBy { get { return _submittedBy; } }
        public string Address1 { get { return _address1; } }
        public string Address2 { get { return _address2; } }
        public string CityStateZip { get { return _city_state_zip; } }
        public string Grower { get { return _grower; } }
        public string DateReceived { get { return _dateReceived; } }
        public string DateReported { get { return _dateReported; } }

        public SoilSampleLabState(int soilSampleLabID, string contractNumber, string fieldName, int cropYear, string accountNumber, string labNumber, string assocNumber,
            string submittedBy, string address1, string address2, string cityStateZip, string grower,
            string dateReceived, string dateReported) {

            _soilSampleLabID = soilSampleLabID;
            _contractNumber = contractNumber;
            _fieldName = fieldName;

            if (soilSampleLabID > 0) {
                _cropYear = cropYear.ToString();
            } else {
                _cropYear = "";
            }

            _accountNumber = accountNumber;
            _labNumber = labNumber;
            _assocNumber = assocNumber;
            _submittedBy = submittedBy;
            _address1 = address1;
            _address2 = address2;
            _city_state_zip = cityStateZip;
            _grower = grower;
            _dateReceived = dateReceived;
            _dateReported = dateReported;
        }
    }
}
