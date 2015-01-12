using System;
using System.Configuration;
using System.Data;
using MDAAB.Classic;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace WSCData {
    /// <summary>
    /// Summary description for WSCPayment.
    /// </summary>
    public class WSCPayment {

        private const string MOD_NAME = "WSCData.WSCPayment.";
        private static string LF = System.Environment.NewLine;

		public static List<SHPaySumListItem> GetPaymentSummary(string connStr, int cropYear, string shid, string fromShid, string toShid, int paymentDescID, bool isCumulative) {

            const string METHOD_NAME = "GetPaymentSummary: ";

            try {

                string procName = "bawpRptPaymentSummary";

				List<SHPaySumListItem> stateList = new List<SHPaySumListItem>();

				int xi_Shid = 0, xs_PayeeName = 0, xi_Payee_Number = 0, xs_Address1 = 0, xs_Address2	 = 0,
					xs_City = 0, xs_State = 0, xs_PostalCode = 0, xd_Amount = 0, xd_checkAmount = 0,
					xd_groAmount = 0, xd_ldoAmount = 0, xi_PaymentNumber = 0, xs_PaymentDesc = 0, 
					xi_CheckSequence = 0, xi_ContractNumber = 0, xs_Station_Name = 0, xs_LOName = 0, 
					xd_EH_Sugar = 0, xd_RH_Sugar = 0, xd_EH_Paid = 0, xd_RH_Paid = 0, xd_EH_Price = 0,
					xd_RH_Price = 0, xd_EH_Tons = 0, xd_RH_Tons = 0, xd_EH_Gross_Pay = 0, xd_RH_Gross_Pay = 0,
					xd_EH_Bonus = 0, xd_Deduct_Total = 0, xd_Total_Net = 0, xd_EH_SLM = 0, xd_RH_SLM = 0,
					xd_EH_tons_moved = 0, xd_RH_tons_moved = 0, xd_EH_amt_moved = 0, xd_RH_amt_moved = 0,
					xd_Avg_SLM = 0;

                // Check passed parameters
                if (shid.Length == 0) { shid = null; }
                if (fromShid.Length == 0) { fromShid = null; }
                if (toShid.Length == 0) { toShid = null; }

                try {

					using (SqlConnection conn = new SqlConnection(connStr)) { 

						if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
						System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

						spParams[0].Value = cropYear;
						spParams[1].Value = shid;
						spParams[2].Value = fromShid;
						spParams[3].Value = toShid;
						spParams[4].Value = paymentDescID;
						spParams[5].Value = isCumulative;

						SetTimeout();

						using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

							xi_Shid = dr.GetOrdinal("SHID");
							xs_PayeeName = dr.GetOrdinal("PayeeName"); 
							xi_Payee_Number = dr.GetOrdinal("Payee_Number"); 
							xs_Address1 = dr.GetOrdinal("Address1");
							xs_Address2	 = dr.GetOrdinal("Address2");
							xs_City = dr.GetOrdinal("City");
							xs_State = dr.GetOrdinal("State");
							xs_PostalCode = dr.GetOrdinal("PostalCode");
							xd_Amount = dr.GetOrdinal("Amount");
							xd_checkAmount = dr.GetOrdinal("checkAmount");
							xd_groAmount = dr.GetOrdinal("groAmount");
							xd_ldoAmount = dr.GetOrdinal("ldoAmount");
							xi_PaymentNumber = dr.GetOrdinal("PaymentNumber");
							xs_PaymentDesc = dr.GetOrdinal("PaymentDesc");
							xi_CheckSequence = dr.GetOrdinal("CheckSequence");
							xi_ContractNumber = dr.GetOrdinal("ContractNumber");
							xs_Station_Name = dr.GetOrdinal("Station_Name");
							xs_LOName = dr.GetOrdinal("LOName");
							xd_EH_Sugar = dr.GetOrdinal("EH_Sugar");
							xd_RH_Sugar = dr.GetOrdinal("RH_Sugar");
							xd_EH_Paid = dr.GetOrdinal("EH_Paid");
							xd_RH_Paid = dr.GetOrdinal("RH_Paid");
							xd_EH_Price = dr.GetOrdinal("EH_Price");
							xd_RH_Price = dr.GetOrdinal("RH_Price");
							xd_EH_Tons = dr.GetOrdinal("EH_Tons"); 
							xd_RH_Tons = dr.GetOrdinal("RH_Tons");
							xd_EH_Gross_Pay = dr.GetOrdinal("EH_Gross_Pay");
							xd_RH_Gross_Pay = dr.GetOrdinal("RH_Gross_Pay");
							xd_EH_tons_moved = dr.GetOrdinal("EH_tons_moved");
							xd_RH_tons_moved = dr.GetOrdinal("RH_tons_moved");
							xd_EH_amt_moved = dr.GetOrdinal("EH_amt_moved");
							xd_RH_amt_moved = dr.GetOrdinal("RH_amt_moved");
							xd_EH_Bonus = dr.GetOrdinal("EH_Bonus");
							xd_Deduct_Total = dr.GetOrdinal("Deduct_Total");
							xd_Total_Net = dr.GetOrdinal("Total_Net");
							xd_EH_SLM = dr.GetOrdinal("EH_SLM");
							xd_RH_SLM = dr.GetOrdinal("RH_SLM");
							xd_Avg_SLM = dr.GetOrdinal("Avg_SLM");

							while (dr.Read()) {

								stateList.Add(new SHPaySumListItem(
									dr.GetInt32(xi_Shid),
									dr.GetString(xs_PayeeName),
									dr.GetInt32(xi_Payee_Number),
									dr.GetString(xs_Address1),
									dr.GetString(xs_Address2),
									dr.GetString(xs_City),
									dr.GetString(xs_State),
									dr.GetString(xs_PostalCode),
									dr.GetDecimal(xd_Amount),
									dr.GetDecimal(xd_checkAmount),
									dr.GetDecimal(xd_groAmount),
									dr.GetDecimal(xd_ldoAmount),
									dr.GetInt32(xi_PaymentNumber),
									dr.GetString(xs_PaymentDesc),
									dr.GetInt32(xi_CheckSequence),
									dr.GetInt32(xi_ContractNumber),
									dr.GetString(xs_Station_Name),
									dr.GetString(xs_LOName),
									dr.GetDecimal(xd_EH_Sugar),
									dr.GetDecimal(xd_RH_Sugar),
									dr.GetDecimal(xd_EH_Paid),
									dr.GetDecimal(xd_RH_Paid),
									dr.GetDecimal(xd_EH_Price),
									dr.GetDecimal(xd_RH_Price),
									dr.GetDecimal(xd_EH_Tons),
									dr.GetDecimal(xd_RH_Tons),
									dr.GetDecimal(xd_EH_Gross_Pay),
									dr.GetDecimal(xd_RH_Gross_Pay),
									dr.GetDecimal(xd_EH_tons_moved),
									dr.GetDecimal(xd_RH_tons_moved),
									dr.GetDecimal(xd_EH_amt_moved),
									dr.GetDecimal(xd_RH_amt_moved),
									dr.GetDecimal(xd_EH_Bonus),
									dr.GetDecimal(xd_Deduct_Total),
									dr.GetDecimal(xd_Total_Net),
									dr.GetDecimal(xd_EH_SLM),
									dr.GetDecimal(xd_RH_SLM),
									dr.GetDecimal(xd_Avg_SLM)
								));
							}
						}

						return stateList;
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
            }
            catch (System.Exception e) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }            
        }

        public static SqlDataReader GetPaymentExport(SqlConnection conn, string procName, int paymentDescID, int cropYear) {

            const string METHOD_NAME = "GetPaymentExport: ";
            SqlDataReader dr = null;

            try {

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = paymentDescID;
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
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader GetEquityStatement(SqlConnection conn, int cropYear, string shid, bool isActive,
			DateTime activityFromDate, DateTime activityToDate, bool isLienInfoWanted) {

            const string METHOD_NAME = "GetEquityStatement: ";
            SqlDataReader dr = null;
            string shidList = null;
            string shidLo = null;
            string shidHi = null;

            try {

                // Configure the appropriate shid passed parameters based on the incoming
                // shid string.
				//if (shid == null || shid.Length == 0) {
				//    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You must enter a SHID or a list or range of SHIDs.");
				//    throw (warn);
				//}

				if (!String.IsNullOrEmpty(shid)) {
					BeetDataDomain.SplitShidList(shid, out shidList, out shidLo, out shidHi);
				}

                string procName = "bawpRptEquityStatement";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = shidList;
                spParams[2].Value = shidLo;
                spParams[3].Value = shidHi;
                spParams[4].Value = isActive;
                spParams[5].Value = 0;
				if (activityFromDate == DateTime.MinValue) {
					spParams[6].Value = DBNull.Value;
				} else {
					spParams[6].Value = activityFromDate;
				}				
				if (activityToDate == DateTime.MinValue) {
					spParams[7].Value = DBNull.Value;
				} else {
					spParams[7].Value = activityToDate;
				}
				spParams[8].Value = isLienInfoWanted;				
				
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
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static string GetPaymentTransmittalDate(int paymentNumber, int cropYear) {

            const string METHOD_NAME = "GetPaymentTransmittalDate: ";
            string transDate = "";

            try {

                string procName = "bawpPaymentGetTransmittalDate";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) { 

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					System.Data.SqlClient.SqlParameter[] spParams =
						SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = cropYear;
					spParams[1].Value = paymentNumber;
					SetTimeout();

					try {
						SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
						transDate = spParams[2].Value.ToString();
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
				}
            }
            catch (System.Exception e) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return transDate;
        }

        public static SqlDataReader RptContractPayeeSummary1(SqlConnection conn, int cropYear, string shid) {

            const string METHOD_NAME = "RptContractPayeeSummary1: ";
            SqlDataReader dr = null;
            string shidList = null;
            string shidLo = null;
            string shidHi = null;

            // Configure the appropriate shid passed parameters based on the incoming
            // shid string.
            if (shid == null || shid.Length == 0) {
                WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You must enter a SHID or a list or range of SHIDs.");
                throw (warn);
            }
       
            try {

                BeetDataDomain.SplitShidList(shid, out shidList, out shidLo, out shidHi);

                string procName = "bawpRptContractPayeeSummary1";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = shidList;
                spParams[2].Value = shidLo;
                spParams[3].Value = shidHi;
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
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader RptContractPayeeSummary2(SqlConnection conn, int contractNumber, int cropYear) {

            const string METHOD_NAME = "RptContractPayeeSummary2: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRptContractPayeeSummary2";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = contractNumber;
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
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static void PostCalculatePayment(SqlConnection conn, int factoryID, int paymentNumber, int cropYear, string userName) {

            const string METHOD_NAME = "PostCalculatePayment: ";

            try {

                string procName = "s70pay_PostCalculatePayment";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = factoryID;
                spParams[1].Value = paymentNumber;
                spParams[2].Value = cropYear;
                spParams[3].Value = userName;
                SetTimeout();

                try {
                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);
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
            }
            catch (System.Exception e) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }
        }

        public static void CalculatePayment(SqlConnection conn,
            Decimal trialSugarContent, Decimal trialSLMPct, Decimal trialNetReturn, int cropYear,
            ref Decimal qcBeetPaymentPerTon, ref Decimal oldNorthBeetPaymentPerTon,
            ref Decimal oldSouthBeetPaymentPerTon) {

            const string METHOD_NAME = "CalculatePayment: ";

            try {

                string procName = "bawpPaymentCalculator";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = trialSugarContent;
                spParams[1].Value = trialSLMPct;
                spParams[2].Value = trialNetReturn;
                spParams[3].Value = cropYear;
                SetTimeout();

                try {

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, procName, spParams);

                    qcBeetPaymentPerTon = Convert.ToDecimal(spParams[4].Value);
                    oldNorthBeetPaymentPerTon = Convert.ToDecimal(spParams[5].Value);
                    oldSouthBeetPaymentPerTon = Convert.ToDecimal(spParams[6].Value);
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
            }
            catch (System.Exception e) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }
        }

        public static SqlDataReader GetTransmittalPayment(SqlConnection conn,
            int cropYear, int paymentNumber, string factoryList, string stationList, string contractList,
            bool isCumulative) {

            const string METHOD_NAME = "GetTransmittalPayment: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRptTransmittalPayment";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = paymentNumber;
                spParams[2].Value = factoryList;
                spParams[3].Value = stationList;
                spParams[4].Value = contractList;
                spParams[5].Value = isCumulative;

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
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

		public static SqlDataReader GetPaymentDescriptionContract(SqlConnection conn, int description_station_id, 
		string description_id_list, int factory_no, string station_id_list, string contract_no_list, string pdesta_payment_no_list,
		string Shid, int icrop_year) {

			const string METHOD_NAME = "GetPaymentDescriptionContract: ";
			SqlDataReader dr = null;

			try {

				string procName = "bawpPdeCntGet";

				if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
				System.Data.SqlClient.SqlParameter[] spParams =
					SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

				spParams[0].Value = description_station_id;
				spParams[1].Value = description_id_list;
				spParams[2].Value = factory_no;
				spParams[3].Value = station_id_list;
				spParams[4].Value = contract_no_list;
				spParams[5].Value = pdesta_payment_no_list;
				spParams[6].Value = Shid;
				spParams[7].Value = icrop_year;

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
						WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
						throw (wscEx);
					}
				}
			}
			catch (System.Exception e) {

				if (dr != null && !dr.IsClosed) {
					dr.Close();
				}
				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
				throw (wscEx);
			}

			return dr;
		}


		public static List<TransmittalPaymentItem> GetTransmittalPayment(int cropYear, int paymentNumber, string factoryList, string stationList, string contractList,
			bool isCumulative) {

			const string METHOD_NAME = "GetTransmittalPayment: ";
			List<TransmittalPaymentItem> stateList = new List<TransmittalPaymentItem>();

			try {

				string procName = "bawpRptTransmittalPayment";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = cropYear;
					spParams[1].Value = paymentNumber;
					spParams[2].Value = factoryList;
					spParams[3].Value = stationList;
					spParams[4].Value = contractList;
					spParams[5].Value = isCumulative;

					SetTimeout();

					using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

						int iContract_Number = dr.GetOrdinal("Contract_Number"),
						iPayment_Description = dr.GetOrdinal("Payment_Description"),
						iCrop_Year = dr.GetOrdinal("Crop_Year"),
						iBusiness_Name = dr.GetOrdinal("Business_Name"),
						iFirst_Name = dr.GetOrdinal("First_Name"),
						iLast_Name = dr.GetOrdinal("Last_Name"),
						iAddress_1 = dr.GetOrdinal("Address_1"),
						iAddress_2 = dr.GetOrdinal("Address_2"),
						iCity = dr.GetOrdinal("City"),
						iState = dr.GetOrdinal("State"),
						iZip = dr.GetOrdinal("Zip"),
						iPayee_Number = dr.GetOrdinal("Payee_Number"),
						iSplit_Percent = dr.GetOrdinal("Split_Percent"),
						iPayment_Number = dr.GetOrdinal("Payment_Number"),
						iPayment_Amount = dr.GetOrdinal("Payment_Amount"),
						iSplit_Payment = dr.GetOrdinal("Split_Payment"),
						iEH_Bonus = dr.GetOrdinal("EH_Bonus"),
						iEH_Gross_Pay = dr.GetOrdinal("EH_Gross_Pay"),
						iRH_Gross_Pay = dr.GetOrdinal("RH_Gross_Pay"),
						iEH_Tons_Moved = dr.GetOrdinal("EH_tons_moved"),
						iRH_Tons_Moved = dr.GetOrdinal("RH_tons_moved"),
						iEH_Amt_Moved = dr.GetOrdinal("EH_amt_moved"),
						iRH_Amt_Moved = dr.GetOrdinal("RH_amt_moved"),
						iDeduct_Total = dr.GetOrdinal("Deduct_Total"),
						iEH_SLM = dr.GetOrdinal("EH_SLM"),
						iRH_SLM = dr.GetOrdinal("RH_SLM"),
						iEH_Price = dr.GetOrdinal("EH_Price"),
						iRH_Price = dr.GetOrdinal("RH_Price"),
						iEH_Sugar = dr.GetOrdinal("EH_Sugar"),
						iRH_Sugar = dr.GetOrdinal("RH_Sugar"),
						iEH_Tons = dr.GetOrdinal("EH_Tons"),
						iRH_Tons = dr.GetOrdinal("RH_Tons"),
						iFactory_Number = dr.GetOrdinal("Factory_Number"),
						iFactory_Shrinkage = dr.GetOrdinal("Factory_Shrinkage"),
						iPct_EH_Paid = dr.GetOrdinal("Pct_EH_Paid"),
						iPct_RH_Paid = dr.GetOrdinal("Pct_RH_Paid"),
						iFactory_Name = dr.GetOrdinal("Factory_Name"),
						iStation_Name = dr.GetOrdinal("Station_Name"),
						iStation_Number = dr.GetOrdinal("Station_Number"),
						iYtdEhBonus = dr.GetOrdinal("ncYtdEhBonus"),
						iYtdEhGrossPay = dr.GetOrdinal("ncYtdEhGrossPay"),
						iYtdRhGrossPay = dr.GetOrdinal("ncYtdRhGrossPay"),
						iYtdDeductTotal = dr.GetOrdinal("ncYtdDeductTotal"),
						iYtdEhAmtMoved = dr.GetOrdinal("ncYtdEhAmtMoved"),
						iYtdRhAmtMoved = dr.GetOrdinal("ncYtdRhAmtMoved");

						while (dr.Read()) {

							stateList.Add(new TransmittalPaymentItem(
							    dr.GetString(iContract_Number),
							    dr.GetString(iPayment_Description),
							    dr.GetDateTime(iCrop_Year),
							    dr.GetString(iBusiness_Name),
							    dr.GetString(iFirst_Name),
							    dr.GetString(iLast_Name),
							    dr.GetString(iAddress_1),
							    dr.GetString(iAddress_2),
							    dr.GetString(iCity),
							    dr.GetString(iState),
							    dr.GetString(iZip),
							    dr.GetInt16(iPayee_Number),
								dr.GetDecimal(iSplit_Percent),
							    dr.GetInt16(iPayment_Number),
							    dr.GetDecimal(iPayment_Amount),
							    dr.GetDecimal(iSplit_Payment),
							    dr.GetDecimal(iEH_Bonus),
							    dr.GetDecimal(iEH_Gross_Pay),
							    dr.GetDecimal(iRH_Gross_Pay),
								dr.GetDecimal(iEH_Tons_Moved),
								dr.GetDecimal(iRH_Tons_Moved),
								dr.GetDecimal(iEH_Amt_Moved),
								dr.GetDecimal(iRH_Amt_Moved),
							    dr.GetDecimal(iDeduct_Total),
							    dr.GetDecimal(iEH_SLM),
							    dr.GetDecimal(iRH_SLM),
							    dr.GetDecimal(iEH_Price),
							    dr.GetDecimal(iRH_Price),
							    dr.GetDecimal(iEH_Sugar),
							    dr.GetDecimal(iRH_Sugar),
							    dr.GetDecimal(iEH_Tons),
							    dr.GetDecimal(iRH_Tons),
							    dr.GetInt16(iFactory_Number),
							    dr.GetDecimal(iFactory_Shrinkage),
							    dr.GetDecimal(iPct_EH_Paid),
								dr.GetDecimal(iPct_RH_Paid),
							    dr.GetString(iFactory_Name),
							    dr.GetString(iStation_Name),
							    dr.GetInt16(iStation_Number),
							    dr.GetDecimal(iYtdEhBonus),
							    dr.GetDecimal(iYtdEhGrossPay),
							    dr.GetDecimal(iYtdRhGrossPay),
							    dr.GetDecimal(iYtdDeductTotal),
								dr.GetDecimal(iYtdEhAmtMoved),
								dr.GetDecimal(iYtdRhAmtMoved)
							));
						}
					}
				}
			}
			catch (System.Exception e) {
				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
				throw (wscEx);
			}

			return stateList;
		}

		public static List<TransDeductionListItem> GetTransmittalDeduction2(string connStr, int cropYear, int paymentDescID, 
			int paymentDescNumber, int firstContractNumber, int lastContractNumber, bool isCumulative) {

			const string METHOD_NAME = "GetTransmittalDeduction2: ";

			try {

				string procName = "bawpRptTransmittalDeduction2";

				List<TransDeductionListItem> stateList = new List<TransDeductionListItem>();

				int iContractNumber = 0, iPaymentNumber = 0, iDeductionNumber = 0, iDeductionDesc = 0,
					iPaymentDescription = 0, iAmount = 0;

				using (SqlConnection conn = new SqlConnection(connStr)) { 

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = cropYear;
					spParams[1].Value = paymentDescID;
					spParams[2].Value = paymentDescNumber;
					spParams[3].Value = firstContractNumber;
					spParams[4].Value = lastContractNumber;
					spParams[5].Value = isCumulative;

					SetTimeout();

					try {

						using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

							iContractNumber = dr.GetOrdinal("Contract_Number");
							iPaymentNumber = dr.GetOrdinal("Payment_Number");
							iDeductionNumber = dr.GetOrdinal("Deduction_Number");
							iDeductionDesc = dr.GetOrdinal("Deduction_Desc");
							iPaymentDescription = dr.GetOrdinal("Payment_Description");
							iAmount = dr.GetOrdinal("Amount");

							while (dr.Read()) {

								stateList.Add(new TransDeductionListItem(
									dr.GetInt32(iContractNumber), 
									dr.GetInt32(iPaymentNumber), 
									dr.GetInt32(iDeductionNumber), 
									dr.GetString(iDeductionDesc),
									dr.GetString(iPaymentDescription), 
									dr.GetDecimal(iAmount)
									
								));
							}
						}

						return stateList;
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
				}
			}
			catch (System.Exception e) {
				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
				throw (wscEx);
			}
		}

        public static SqlDataReader GetTransmittalDeduction(SqlConnection conn,
            int cropYear, int contractNumber, int paymentNumber, bool isCumulative) {

            const string METHOD_NAME = "GetTransmittalDeduction: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRptTransmittalDeduction";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = contractNumber;
                spParams[2].Value = paymentNumber;
                spParams[3].Value = isCumulative;

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
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static List<TransmittalDeliveryItem> GetTransmittalDelivery(int cropYear, int contractNumber, int paymentNumber, string fromDate, string toDate) {

            const string METHOD_NAME = "GetTransmittalDelivery: ";

			List<TransmittalDeliveryItem> stateList = new List<TransmittalDeliveryItem>();
            try {

                string procName = "bawpRptTransmittalDelivery";

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) { 

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					System.Data.SqlClient.SqlParameter[] spParams =
						SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = cropYear;
					spParams[1].Value = contractNumber;
					spParams[2].Value = paymentNumber;
					if (fromDate != null) {
						spParams[3].Value = DateTime.Parse(fromDate);
					} else {
						spParams[3].Value = DBNull.Value;
					}
					if (toDate != null) {
						spParams[4].Value = DateTime.Parse(toDate);
					} else {
						spParams[4].Value = DBNull.Value;
					}

					SetTimeout();

					try {

						using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

							int iDelivery_Date = dr.GetOrdinal("Delivery_Date"),
							iSLM_Pct = dr.GetOrdinal("SLM_Pct"),
							iFirst_Net_Pounds = dr.GetOrdinal("First_Net_Pounds"),
							iFinal_Net_Pounds = dr.GetOrdinal("Final_Net_Pounds"),
							iSugar_Content = dr.GetOrdinal("Sugar_Content"),
							iTare = dr.GetOrdinal("Tare"),
							iTares = dr.GetOrdinal("Tares"),
							iLoads = dr.GetOrdinal("Loads");

							while (dr.Read()) { 

								stateList.Add(
									new TransmittalDeliveryItem(
										dr.GetDateTime(iDelivery_Date),
										dr.GetDecimal(iSLM_Pct),
										dr.GetInt32(iFirst_Net_Pounds),
										dr.GetInt32(iFinal_Net_Pounds),
										dr.GetDecimal(iSugar_Content),
										dr.GetDecimal(iTare),
										dr.GetInt32(iTares),
										dr.GetInt32(iLoads)
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
							WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
							throw (wscEx);
						}
					}
				}
            }
            catch (System.Exception e) {

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return stateList;
        }

        public static SqlDataReader GetLandownerLetter(SqlConnection conn,
            int cropYear, string factoryList, string stationList, string contractList) {

            const string METHOD_NAME = "bawpRptLandownerLetter: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpRptLandownerLetter";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
                spParams[1].Value = factoryList;
                spParams[2].Value = stationList;
                spParams[3].Value = contractList;

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
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static void PaymentDescriptionSave(int payDescriptionID,
            int payNumber, int cropYear, string payDesc, bool required, bool finished,
            string transmittalDate, string UserName) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpPaymentSaveDesc";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[1].Value = payDescriptionID;
                    spParams[2].Value = payNumber;
                    spParams[3].Value = cropYear;
                    spParams[4].Value = payDesc;
                    spParams[5].Value = required;
                    spParams[6].Value = finished;
                    if (transmittalDate != null && transmittalDate.Length > 0) {
                        spParams[7].Value = DateTime.Parse(transmittalDate);
                    } else {
                        spParams[7].Value = DBNull.Value;
                    }
                    spParams[8].Value = UserName;
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
                                string errMsg = MOD_NAME + "PaymentDescriptionSave";
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + "PaymentDescriptionSave";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static SqlDataReader GetEquityPaymentTypes(SqlConnection conn, int cropYear) {

            const string METHOD_NAME = "GetEquityPaymentTypes: ";
            SqlDataReader dr = null;

            try {

                string procName = "bawpEquityPaymentTypes";

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
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                        throw (wscEx);
                    }
                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static SqlDataReader GetEquityPaymentExport(SqlConnection conn, int paymentCropYear, bool isPatronage, string paymentType, string paymentDate,
            ref System.Data.SqlClient.SqlParameter outParam) {

            const string METHOD_NAME = "GetEquityPaymentExport";
            SqlDataReader dr = null;

            try {

                if (isPatronage) {

                    string procName = "bawpPatronagePaymentExport";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = paymentCropYear;
                    spParams[1].Value = paymentType;
                    spParams[2].Value = paymentDate;
                    outParam = spParams[3];
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
                            WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                            throw (wscEx);
                        }
                    }

                } else {

                    string procName = "bawpRetainPaymentExport";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = paymentCropYear;
                    spParams[1].Value = paymentDate;
                    outParam = spParams[2];
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
                            WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, sqlEx);
                            throw (wscEx);
                        }
                    }

                }
            }
            catch (System.Exception e) {

                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                }
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
                throw (wscEx);
            }

            return dr;
        }

        public static List<List1099ExportItem> GetBeet1099Export(int calendarYear) {

            const string METHOD_NAME = "GetBeet1099Export";
            List<List1099ExportItem> state = new List<List1099ExportItem>();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpPayment1099Export";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = calendarYear;

                    int iSHID = 0, iTaxID = 0, iBusinessName = 0, iAddress1 = 0, iAddress2 = 0, iCity = 0, iState = 0, iZip = 0, iDollars = 0;
                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                        while (dr.Read()) {

                            if (iSHID == 0) {

                                iSHID = dr.GetOrdinal("cShid");
                                iTaxID = dr.GetOrdinal("cTaxID");
                                iBusinessName = dr.GetOrdinal("cBusName");
                                iAddress1 = dr.GetOrdinal("cAddress1");
                                iAddress2 = dr.GetOrdinal("cAddress2");
                                iCity = dr.GetOrdinal("cCity");
                                iState = dr.GetOrdinal("cState");
                                iZip = dr.GetOrdinal("cZip");
                                iDollars = dr.GetOrdinal("cDollars");
                            }
                            state.Add(new List1099ExportItem(dr.GetString(iSHID), dr.GetString(iTaxID), dr.GetString(iBusinessName),
                                dr.GetString(iAddress1), dr.GetString(iAddress2), dr.GetString(iCity), dr.GetString(iState), dr.GetString(iZip), dr.GetString(iDollars)));
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

		public static void PaymentDescriptionContractSave(int pdecnt_description_contract_id,
			int pdecnt_description_id, int pdecnt_factory_id, int pdecnt_station_id,
			int pdecnt_contract_id, int pdecnt_icrop_year, decimal pdecnt_excess_beet_pct,
			string UserName, string pdecnt_rowversion) {

			const string METHOD_NAME = "PaymentDescriptionContractSave";

			try {

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					string procName = "bawpPdeCntSave";

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					System.Data.SqlClient.SqlParameter[] spParams =
						SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = pdecnt_description_contract_id;
					spParams[1].Value = pdecnt_description_id;
					spParams[2].Value = pdecnt_factory_id;
					spParams[3].Value = pdecnt_station_id;
					spParams[4].Value = pdecnt_contract_id;
					spParams[5].Value = pdecnt_icrop_year;
					spParams[6].Value = pdecnt_excess_beet_pct;
					spParams[7].Value = UserName;
					if (pdecnt_rowversion == "") {
						spParams[8].Value = DBNull.Value;
					} else {
						spParams[8].Value = pdecnt_rowversion;
					}					

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

		public static List<BeetPaymentListItem> RptBeetPayBreakdown(string connStr, int shid,
			int cropYear, int calYear) {

			const string METHOD_NAME = "RptBeetPayBreakdown: ";

			try {

				string procName = "bawpRptBeetPayBreakdown";

				List<BeetPaymentListItem> stateList = new List<BeetPaymentListItem>();

				using (SqlConnection conn = new SqlConnection(connStr)) {

					if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
					System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

					spParams[0].Value = shid;
					spParams[1].Value = cropYear;
					spParams[2].Value = calYear;

					try {

						using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

							int iSHID = dr.GetOrdinal("SHID"),
							iPayeeName = dr.GetOrdinal("PayeeName"),
							iCropYear = dr.GetOrdinal("CropYear"),
							iCalendarYear = dr.GetOrdinal("CalendarYear"),
							iPaymentNumber = dr.GetOrdinal("PaymentNumber"),
							iPaymentDescription = dr.GetOrdinal("PaymentDescription"),
							iTransmittalDate = dr.GetOrdinal("TransmittalDate"),
							iGrossDollars = dr.GetOrdinal("GrossDollars"),
							iPaymentAmount = dr.GetOrdinal("PaymentAmount");

							while (dr.Read()) {

								stateList.Add(new BeetPaymentListItem(
									dr.GetInt32(iSHID),
									dr.GetString(iPayeeName),
									dr.GetInt32(iCropYear),
									dr.GetInt32(iCalendarYear),
									dr.GetInt32(iPaymentNumber),
									dr.GetString(iPaymentDescription),
									dr.GetDateTime(iTransmittalDate),
									dr.GetDecimal(iGrossDollars),
									dr.GetDecimal(iPaymentAmount)

								));
							}
						}

						return stateList;
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
				}
			}
			catch (System.Exception e) {
				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, e);
				throw (wscEx);
			}
		}

        private static void SetTimeout() {
            int timeOut = Convert.ToInt32(ConfigurationManager.AppSettings["sql.command.timeout"].ToString());
            SqlHelper.CommandTimeout = timeOut.ToString();
        }
    }

	public class TransDeductionListItem {

		public TransDeductionListItem() {}

		public TransDeductionListItem(int contract_Number, int payment_Number, int deduction_Number, string deduction_Desc,
			string payment_description, decimal amount) {

			Contract_Number = contract_Number;
			Payment_Number = payment_Number;
			Deduction_Number = deduction_Number;
			Deduction_Desc = deduction_Desc;
			Payment_Description = payment_description;
			Amount = amount;
		}

		public int Contract_Number { get; set; }
		public int Payment_Number { get; set; }
		public int Deduction_Number { get; set; }
		public string Deduction_Desc { get; set; }
		public string Payment_Description { get; set; }
		public decimal Amount { get; set; }
	}

	public class SHPaySumListItem {

		public SHPaySumListItem() { }

		public SHPaySumListItem(int pi_Shid, string ps_PayeeName, int pi_Payee_Number, string ps_Address1, string ps_Address2,
			string ps_City, string ps_State, string ps_PostalCode, decimal pd_Amount,
			decimal pd_checkAmount, decimal pd_groAmount, decimal pd_ldoAmount, int pi_PaymentNumber,
			string ps_PaymentDesc, int pi_CheckSequence, int pi_ContractNumber, string ps_Station_Name,
			string ps_LOName, decimal pd_EH_Sugar, decimal pd_RH_Sugar, decimal pd_EH_Paid,
			decimal pd_RH_Paid, decimal pd_EH_Price, decimal pd_RH_Price, decimal pd_EH_Tons,
			decimal pd_RH_Tons, decimal pd_EH_Gross_Pay, decimal pd_RH_Gross_Pay, 
			decimal pd_EH_tons_moved, decimal pd_RH_tons_moved, decimal pd_EH_amt_moved, decimal pd_RH_amt_moved,
			decimal pd_EH_Bonus,
			decimal pd_Deduct_Total, decimal pd_Total_Net, decimal pd_EH_SLM, decimal pd_RH_SLM,
			decimal pd_Avg_SLM) {

			i_SHID = pi_Shid;
			s_PayeeName = ps_PayeeName;
			i_Payee_Number = pi_Payee_Number;
			s_Address1 = ps_Address1;
			s_Address2 = ps_Address2;
			s_City = ps_City;
			s_State = ps_State;
			s_PostalCode = ps_PostalCode;
			d_Amount = pd_Amount;
			d_checkAmount = pd_checkAmount;
			d_groAmount = pd_groAmount;
			d_ldoAmount = pd_ldoAmount;
			i_PaymentNumber = pi_PaymentNumber;
			s_PaymentDesc = ps_PaymentDesc;
			i_CheckSequence = pi_CheckSequence;
			i_ContractNumber = pi_ContractNumber;
			s_Station_Name = ps_Station_Name;
			s_LOName = ps_LOName;
			d_EH_Sugar = pd_EH_Sugar;
			d_RH_Sugar = pd_RH_Sugar;
			d_EH_Paid = pd_EH_Paid;
			d_RH_Paid = pd_RH_Paid;
			d_EH_Price = pd_EH_Price;
			d_RH_Price = pd_RH_Price;
			d_EH_Tons = pd_EH_Tons;
			d_RH_Tons = pd_RH_Tons;
			d_EH_Gross_Pay = pd_EH_Gross_Pay;
			d_RH_Gross_Pay = pd_RH_Gross_Pay;
			d_EH_tons_moved = pd_EH_tons_moved;
			d_RH_tons_moved = pd_RH_tons_moved;
			d_EH_amt_moved = pd_EH_amt_moved;
			d_RH_amt_moved = pd_RH_amt_moved;
			d_EH_Bonus = pd_EH_Bonus;
			d_Deduct_Total = pd_Deduct_Total;
			d_Total_Net = pd_Total_Net;
			d_EH_SLM = pd_EH_SLM;
			d_RH_SLM = pd_RH_SLM;
			d_Avg_SLM = pd_Avg_SLM;
		}

        public int i_SHID { get; set; }
		public string s_PayeeName { get; set; }
		public int i_Payee_Number { get; set; }
		public string s_Address1 { get; set; }
		public string s_Address2 { get; set; }
		public string s_City { get; set; }
		public string s_State { get; set; }
		public string s_PostalCode { get; set; }
		public decimal d_Amount { get; set; }
		public decimal d_checkAmount { get; set; }
		public decimal d_groAmount { get; set; }
		public decimal d_ldoAmount { get; set; }
		public int i_PaymentNumber { get; set; }
		public string s_PaymentDesc { get; set; }
		public int i_CheckSequence { get; set; }
		public int i_ContractNumber { get; set; }
		public string s_Station_Name { get; set; }
		public string s_LOName { get; set; }
		public decimal d_EH_Sugar { get; set; }
		public decimal d_RH_Sugar { get; set; }
		public decimal d_EH_Paid { get; set; }
		public decimal d_RH_Paid { get; set; }
		public decimal d_EH_Price { get; set; }
		public decimal d_RH_Price { get; set; }
		public decimal d_EH_Tons { get; set; }
		public decimal d_RH_Tons { get; set; }
		public decimal d_EH_Gross_Pay { get; set; }
		public decimal d_RH_Gross_Pay { get; set; }
		public decimal d_EH_tons_moved { get; set; }
		public decimal d_RH_tons_moved { get; set; }
		public decimal d_EH_amt_moved { get; set; }
		public decimal d_RH_amt_moved { get; set; }
		public decimal d_EH_Bonus { get; set; }
		public decimal d_Deduct_Total { get; set; }
		public decimal d_Total_Net { get; set; }
		public decimal d_EH_SLM { get; set; }
		public decimal d_RH_SLM { get; set; }
		public decimal d_Avg_SLM { get; set; }

	}

	public class TransmittalDeliveryItem {

		public TransmittalDeliveryItem() {}

		public TransmittalDeliveryItem(
			DateTime pDelivery_Date, decimal pSLM_Pct, int pFirst_Net_Pounds, int pFinal_Net_Pounds,
			decimal pSugar_Content, decimal pTare, int pTares, int pLoads) {

			Delivery_Date = pDelivery_Date;
			SLM_Pct = pSLM_Pct;
			First_Net_Pounds = pFirst_Net_Pounds;
			Final_Net_Pounds = pFinal_Net_Pounds;
			Sugar_Content = pSugar_Content;
			Tare = pTare;
			Tares = pTares;
			Loads = pLoads;

		}

		public DateTime Delivery_Date {get; set;}
		public decimal SLM_Pct {get; set;}
		public int First_Net_Pounds {get; set;}
		public int Final_Net_Pounds {get; set;}
		public decimal Sugar_Content {get; set;}	
		public decimal Tare {get; set;}
		public int Tares {get; set;}
		public int Loads {get; set;}

	}

	public class TransmittalPaymentItem {


		public TransmittalPaymentItem() { }

		public TransmittalPaymentItem(string pContract_Number,
			string pPayment_Description, DateTime pCrop_Year, string pBusiness_Name, string pFirst_Name,
			string pLast_Name, string pAddress_1, string pAddress_2, string pCity,
			string pState, string pZip, short pPayee_Number, decimal pSplit_Percent,
			short pPayment_Number, decimal pPayment_Amount, decimal pSplit_Payment, decimal pEH_Bonus,
			decimal pEH_Gross_Pay, decimal pRH_Gross_Pay,
			decimal pd_EH_tons_moved, decimal pd_RH_tons_moved, decimal pd_EH_amt_moved, decimal pd_RH_amt_moved,
			decimal pDeduct_Total, decimal pEH_SLM,
			decimal pRH_SLM, decimal pEH_Price, decimal pRH_Price, decimal pEH_Sugar,
			decimal pRH_Sugar, decimal pEH_Tons, decimal pRH_Tons, short pFactory_Number,
			decimal pFactory_Shrinkage, decimal pPct_EH_Paid, decimal pPct_RH_Paid, string pFactory_Name, string pStation_Name,
			short pStation_Number, decimal pYtdEhBonus, decimal pYtdEhGrossPay, decimal pYtdRhGrossPay,
			decimal pYtdDeductTotal, decimal pYtdEhAmtMoved, decimal pYtdRhAmtMoved) {

			Contract_Number = pContract_Number;
			Payment_Description = pPayment_Description;
			Crop_Year = pCrop_Year;
			Business_Name = pBusiness_Name;
			First_Name = pFirst_Name;
			Last_Name = pLast_Name;
			Address_1 = pAddress_1;
			Address_2 = pAddress_2;
			City = pCity;
			State = pState;
			Zip = pZip;
			Payee_Number = pPayee_Number;
			Split_Percent = pSplit_Percent;
			Payment_Number = pPayment_Number;
			Payment_Amount = pPayment_Amount;
			Split_Payment = pSplit_Payment;
			EH_Bonus = pEH_Bonus;
			EH_Gross_Pay = pEH_Gross_Pay;
			RH_Gross_Pay = pRH_Gross_Pay;
			EH_Tons_Moved = pd_EH_tons_moved;
			RH_Tons_Moved = pd_RH_tons_moved; 
			EH_Amt_Moved = pd_EH_amt_moved;
			RH_Amt_Moved = pd_RH_amt_moved;
			Deduct_Total = pDeduct_Total;
			EH_SLM = pEH_SLM;
			RH_SLM = pRH_SLM;
			EH_Price = pEH_Price;
			RH_Price = pRH_Price;
			EH_Sugar = pEH_Sugar;
			RH_Sugar = pRH_Sugar;
			EH_Tons = pEH_Tons;
			RH_Tons = pRH_Tons;
			Factory_Number = pFactory_Number;
			Factory_Shrinkage = pFactory_Shrinkage;
			Pct_EH_Paid = pPct_EH_Paid;
			Pct_RH_Paid = pPct_RH_Paid;
			Factory_Name = pFactory_Name;
			Station_Name = pStation_Name;
			Station_Number = pStation_Number;
			YtdEhBonus = pYtdEhBonus;
			YtdEhGrossPay = pYtdEhGrossPay;
			YtdRhGrossPay = pYtdRhGrossPay;
			YtdDeductTotal = pYtdDeductTotal;
			YtdEhAmtMoved = pYtdEhAmtMoved;
			YtdRhAmtMoved = pYtdRhAmtMoved;
		}

		public string Contract_Number { get; set; }
		public string Payment_Description { get; set; }
		public DateTime Crop_Year { get; set; }
		public string Business_Name { get; set; }
		public string First_Name { get; set; }
		public string Last_Name { get; set; }
		public string Address_1 { get; set; }
		public string Address_2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public short Payee_Number { get; set; }
		public decimal Split_Percent { get; set; }
		public short Payment_Number { get; set; }
		public decimal Payment_Amount { get; set; }
		public decimal Split_Payment { get; set; }
		public decimal EH_Bonus { get; set; }
		public decimal EH_Gross_Pay { get; set; }
		public decimal RH_Gross_Pay { get; set; }
		public decimal EH_Tons_Moved { get; set; }
		public decimal RH_Tons_Moved { get; set; }
		public decimal EH_Amt_Moved { get; set; }
		public decimal RH_Amt_Moved { get; set; }
		public decimal Deduct_Total { get; set; }
		public decimal EH_SLM { get; set; }
		public decimal RH_SLM { get; set; }
		public decimal EH_Price { get; set; }
		public decimal RH_Price { get; set; }
		public decimal EH_Sugar { get; set; }
		public decimal RH_Sugar { get; set; }
		public decimal EH_Tons { get; set; }
		public decimal RH_Tons { get; set; }
		public short Factory_Number { get; set; }
		public decimal Factory_Shrinkage { get; set; }
		public decimal Pct_EH_Paid { get; set; }
		public decimal Pct_RH_Paid { get; set; }
		public string Factory_Name { get; set; }
		public string Station_Name { get; set; }
		public short Station_Number { get; set; }
		public decimal YtdEhBonus { get; set; }
		public decimal YtdEhGrossPay { get; set; }
		public decimal YtdRhGrossPay { get; set; }
		public decimal YtdDeductTotal { get; set; }
		public decimal YtdEhAmtMoved { get; set; }
		public decimal YtdRhAmtMoved { get; set; }
	}

	public class BeetPaymentListItem {

		public BeetPaymentListItem() { }

		public BeetPaymentListItem(int shid, string payeeName, int cropYear, int calendarYear, int paymentNumber,
			string paymentDescription, DateTime transmittalDate, decimal grossDollars, decimal paymentAmount) {

			SHID = shid;
			PayeeName = payeeName;
			CropYear = cropYear;
			CalendarYear = calendarYear;
			PaymentNumber = paymentNumber;
			PaymentDescription = paymentDescription;
			TransmittalDate = transmittalDate;
			GrossDollars = grossDollars;
			PaymentAmount = paymentAmount;
		}

		public int SHID { get; set; }
		public string PayeeName { get; set; }
		public int CropYear { get; set; }
		public int CalendarYear { get; set; }
		public int PaymentNumber { get; set; }
		public string PaymentDescription { get; set; }
		public DateTime TransmittalDate { get; set; }
		public decimal GrossDollars { get; set; }
		public decimal PaymentAmount { get; set; }
	}
}
