using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using MDAAB.Classic;
using WSCData;

namespace WSCData {

    public class LimsEx {

        private const string MOD_NAME = "WSCData.LimsEx.";

        public static List<ListIMSFactoryItem> GetWSCFactoryList() {

            const string METHOD_NAME = "GetWSCFactoryList";
            const string cacheKey = "WSCFactoryList";

            List<ListIMSFactoryItem> factoryList = (List<ListIMSFactoryItem>)HttpRuntime.Cache[cacheKey];

            try {

                if (factoryList == null) {

                    factoryList = new List<ListIMSFactoryItem>();
                    factoryList.Add(new ListIMSFactoryItem(20, "Billings"));
                    factoryList.Add(new ListIMSFactoryItem(30, "Ft Morgan"));
                    factoryList.Add(new ListIMSFactoryItem(50, "Lovell"));
                    factoryList.Add(new ListIMSFactoryItem(60, "Scottsbluff"));
                    factoryList.Add(new ListIMSFactoryItem(70, "Torrington"));

                    HttpRuntime.Cache.Insert(cacheKey, factoryList);
                }

                return factoryList;

            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        public static RehaulFactoryStationData GetRehaulFactoryStationData(int factoryID, DateTime rehaulDate) {

            const string METHOD_NAME = "GetRehaulFactoryStationData";
            string cacheKey = "RehaulFactoryStationData";
            string filePath = "";

            RehaulFactoryStationData rehaulData = null;
            try {

                rehaulData = (RehaulFactoryStationData)HttpRuntime.Cache.Get(cacheKey);
                if (rehaulData == null) {

                    rehaulData = new RehaulFactoryStationData();
                    filePath = WSCIEMP.Common.AppHelper.AppPath() + @"ZHost/XML/RehaulFactoryStation.xml";
                    rehaulData.Load(filePath);
                    HttpRuntime.Cache.Insert(cacheKey, rehaulData, new CacheDependency(filePath));
                }

                // Always check database for data.
                SqlDataReader dr = null;

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LimsConn"].ToString())) {

                    string procName = "stapRehaulDailyGetEntry";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = factoryID;
                    spParams[1].Value = rehaulDate;

                    dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams);
                    rehaulData.FillFactoryDetail((factoryID * 10).ToString(), dr);
                }

                return rehaulData;
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME + "; factoryID: " + factoryID.ToString() + "; rehaulDate: " + rehaulDate.ToString();
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void RehaulDailySave(int cropYear, int factoryID, DateTime rehaulDate, string chipsPctTailings, string rehaulLoadAvgWt,
        string yardLoadAvgWt, string chipsDiscardedTons, string beetsSlidLoads, string stationNumberList, string stationRehaulLoadsList) {

            const string METHOD_NAME = "RehaulDailySave";

            try {

				string connString = ConfigurationManager.ConnectionStrings["LimsConn"].ToString();
                using (SqlConnection conn = new SqlConnection(connString)) {

                    string procName = "stapRehaulDailySave";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = factoryID;
                    spParams[2].Value = rehaulDate;

                    if (chipsPctTailings.Length > 0) {
                        spParams[3].Value = Convert.ToDecimal(chipsPctTailings);
                    } else {
                        spParams[3].Value = DBNull.Value;
                    }
                    if (rehaulLoadAvgWt.Length > 0) {
                        spParams[4].Value = Convert.ToDecimal(rehaulLoadAvgWt);
                    } else {
                        spParams[4].Value = DBNull.Value;
                    }
                    if (yardLoadAvgWt.Length > 0) {
                        spParams[5].Value = Convert.ToDecimal(yardLoadAvgWt);
                    } else {
                        spParams[5].Value = DBNull.Value;
                    }
                    if (chipsDiscardedTons.Length > 0) {
                        spParams[6].Value = Convert.ToDecimal(chipsDiscardedTons);
                    } else {
                        spParams[6].Value = DBNull.Value;
                    }
                    if (beetsSlidLoads.Length > 0) {
                        spParams[7].Value = Convert.ToDecimal(beetsSlidLoads);
                    } else {
                        spParams[7].Value = DBNull.Value;
                    }
                    spParams[8].Value = stationNumberList;
                    spParams[9].Value = stationRehaulLoadsList;
                    spParams[10].Value = WSCIEMP.Common.AppHelper.GetIdentityName();

					SqlHelper.CommandTimeout = ConfigurationManager.AppSettings["sql.command.timeout"].ToString();

                    try {
                        using (SqlTransaction tran = conn.BeginTransaction()) {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();
                        }
                    }
                    catch (SqlException sqlEx) {

                        if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                            WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                            throw (wscWarn);
                        } else {
                            string errMsg = MOD_NAME + METHOD_NAME;
                            errMsg += ".  factoryID: " + factoryID.ToString() + "; rehaulDate: " + rehaulDate.ToString() +
                                "; chipsPctTailings: " + chipsPctTailings + "; rehaulLoadAvgWt: " + rehaulLoadAvgWt +
                                "; yardLoadAvgWt: " + yardLoadAvgWt + "; chipsDiscardedTons: " + chipsDiscardedTons +
                                "; beetsSlidLoads: " + beetsSlidLoads + "; stationNumberList: " + stationNumberList +
                                "; stationRehaulLoadsList: " + stationRehaulLoadsList;

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
    }
}
