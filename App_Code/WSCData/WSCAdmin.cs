using System;
using System.Configuration;
using System.Data;
using MDAAB.Classic;
using System.Data.SqlClient;

namespace WSCData {
    /// <summary>
    /// Summary description for WSCAdmin.
    /// </summary>
    public class WSCAdmin {

        private const string MOD_NAME = "WSCData.WSCAdmin.";     
        private static string LF = System.Environment.NewLine;

        public static SqlDataReader AddressFindName(SqlConnection conn, string addressName) {

            const string METHOD_NAME = "AddressFindName";
            SqlDataReader dr = null;

            try {

                string procName = "bawpAddressFindByName";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = addressName;
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

        public static SqlDataReader AddressFindNumber(SqlConnection conn, string addressNumber) {

            const string METHOD_NAME = "AddressFindNumber";
            SqlDataReader dr = null;

            try {

                string procName = "bawpAddressFindByNumber";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = addressNumber;
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

        public static SqlDataReader StateGetAll(SqlConnection conn) {

            const string METHOD_NAME = "StateGetAll";
            SqlDataReader dr = null;

            try {

                string procName = "bawpStateGetAll";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, null);
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

        public static SqlDataReader LookupGetTypesAll(SqlConnection conn) {

            const string METHOD_NAME = "LookupGetTypesAll";
            SqlDataReader dr = null;

            try {

                string procName = "bawpLookupGetTypes";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                SetTimeout();

                try {
                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, null);
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

        private static void SetTimeout() {
			int timeOut = Convert.ToInt32(ConfigurationManager.AppSettings["sql.command.timeout"].ToString());
            SqlHelper.CommandTimeout = timeOut.ToString();
        }

        public static void LookupDelete(string type, string description) {

            const string METHOD_NAME = "LookupDelete";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpLookupDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = type;
                    spParams[1].Value = description;

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
                                throw (sqlEx);
                            }
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

        public static void LookupSave(
            string lookupType, bool lookupIsActive, string lookupOldDescription,
            string lookupDescription, bool fixOldValues) {

            const string METHOD_NAME = "LookupSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpLookupSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = lookupType;
                    spParams[1].Value = lookupIsActive;
                    spParams[2].Value = lookupOldDescription;
                    spParams[3].Value = lookupDescription;
                    spParams[4].Value = fixOldValues;

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
                                throw (sqlEx);
                            }
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

        public static void PlantPopulationSave(int plantPopulationID, int cropYear, string rowWidth,
            string bpaFactor, string standFactor, string userName) {

            const string METHOD_NAME = "PlantPopulationSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpPlantPopulationSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[1].Value = plantPopulationID;
                    spParams[2].Value = cropYear;
                    spParams[3].Value = Convert.ToInt32((rowWidth.Length == 0 ? "0" : rowWidth));
                    spParams[4].Value = Convert.ToDecimal((bpaFactor.Length == 0 ? "0" : bpaFactor));
                    spParams[5].Value = Convert.ToDecimal((standFactor.Length == 0 ? "0" : standFactor));
                    spParams[6].Value = userName.Substring(0, (userName.Length > 20 ? 20 : userName.Length));

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
                                throw (sqlEx);
                            }
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

        public static SqlDataReader PlantPopulationGetByYear(SqlConnection conn, int cropYear) {

            SqlDataReader dr = null;
            const string METHOD_NAME = "PlantPopulationGetByYear";

            try {

                string procName = "bawpPlantPopulationGetByYear";

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

        public static void PlantPopulationDelete(int plantPopulationID) {

            const string METHOD_NAME = "PlantPopulationDelete";
            try {


                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpPlantPopulationDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = plantPopulationID;

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
                                throw (sqlEx);
                            }
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

        public static SqlDataReader ContractDeductionQualify(SqlConnection conn, int cropYear, string xmlDataMap) {

            SqlDataReader dr = null;
            const string METHOD_NAME = "ContractDeductionQualify";

            try {

                string procName = "bawpContractDeductionQualify";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = xmlDataMap;
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

        public static void ContractDeductionSave(int cropYear, string userName, string xmlDataMap) {

            const string METHOD_NAME = "ContractDeductionSave";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpContractDeductionSave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = userName;
                    spParams[2].Value = xmlDataMap;

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
                                throw (sqlEx);
                            }
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

        public static void ContractDeductionDelete(int cropYear, string userName, string xmlDataMap) {

            const string METHOD_NAME = "ContractDeductionDelete";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpContractDeductionDelete";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = userName;
                    spParams[2].Value = xmlDataMap;

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
                                throw (sqlEx);
                            }
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

		public static SqlDataReader PassthroughAllGetByYear(SqlConnection conn, int cropYear) {

			const string METHOD_NAME = "PassthroughAllGetByYear";
			SqlDataReader dr = null;

			try {

				string procName = "bawpPassthroughAllGetByYear";

				if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
				System.Data.SqlClient.SqlParameter[] spParams =
					SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

				spParams[0].Value = cropYear;

				dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);

			}
			catch (System.Exception e) {
				string errMsg = MOD_NAME + METHOD_NAME;
				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
				throw (wscEx);
			}

			return dr;
		}

		public static void PassthroughAllSave(int cropYear, int taxYear, decimal ratePerTon, decimal percentToApply,
			DateTime reportDate, DateTime fiscalYearEndDate, string userName) {

			const string METHOD_NAME = "PassthroughAllSave";

			try {

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					string procName = "bawpPassthroughAllSave";

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					System.Data.SqlClient.SqlParameter[] spParams =
						SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = taxYear;
					spParams[1].Value = cropYear;
					spParams[2].Value = ratePerTon;
					spParams[3].Value = percentToApply;
					if (reportDate == DateTime.MinValue) {
						spParams[4].Value = DBNull.Value;
					} else {
						spParams[4].Value = reportDate;
					}

					if (fiscalYearEndDate == DateTime.MinValue) {
						spParams[5].Value = DBNull.Value;
					} else {
						spParams[5].Value = fiscalYearEndDate; 
					}					
					spParams[6].Value = userName;

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
								throw (sqlEx);
							}
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
}
