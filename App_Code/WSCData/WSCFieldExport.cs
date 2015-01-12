using System;
using System.Configuration;
using System.Data;
using MDAAB.Classic;
using System.Data.SqlClient;
using OLEDB = System.Data.OleDb;

namespace WSCData {
    /// <summary>
    /// Summary description for WSCFieldExport.
    /// </summary>
    public class WSCFieldExport {

        private const string MOD_NAME = "WSCData.WSCFieldExport.";

        public static void PopulateAllTables(int cropYear) {

            PopulateContractingTable(cropYear);
            PopulateAgronomyTable(cropYear);
            PopulateGrowerPerformanceTable(cropYear);
            WSCFieldExport.PopulateBeetAccountingPerformance(cropYear);
            WSCFieldExport.PopulateBeetAccountingPerformance2(cropYear);
            WSCFieldExport.PopulatePerformanceTable(cropYear);
            WSCFieldExport.PopulateContractDirtTable(cropYear);
        }

        public static void DeleteAllTables(int cropYear) {

            DeleteContractingTable(cropYear);
            DeleteAgronomyTable(cropYear);
            DeletePerformanceTable(cropYear);
            DeleteContractDirtTable(cropYear);
        }

        public static void DeleteContractingTable(int cropYear) {

            try {

                using (OLEDB.OleDbConnection msConn =
                          new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

                    string qry = "FieldContractingDelete";

                    if (msConn.State != System.Data.ConnectionState.Open) { msConn.Open(); }
                    using (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand(qry, msConn)) {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("pCropYear", OLEDB.OleDbType.Integer, 0).Value = cropYear;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "DeleteContractingTable";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void DeleteAgronomyTable(int cropYear) {

            try {

                using (OLEDB.OleDbConnection msConn =
                          new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

                    string qry = "FieldAgronomyDelete";

                    if (msConn.State != System.Data.ConnectionState.Open) { msConn.Open(); }
                    using (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand(qry, msConn)) {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("pCropYear", OLEDB.OleDbType.Integer, 0).Value = cropYear;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "DeleteAgronomyTable";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void DeletePerformanceTable(int cropYear) {

            try {

                using (OLEDB.OleDbConnection msConn =
                          new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

                    string qry = "FieldPerformanceDelete";

                    if (msConn.State != System.Data.ConnectionState.Open) { msConn.Open(); }
                    using (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand(qry, msConn)) {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("pCropYear", OLEDB.OleDbType.Integer, 0).Value = cropYear;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "DeletePerformanceTable";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void DeleteContractDirtTable(int cropYear) {

            try {

                using (OLEDB.OleDbConnection msConn =
                          new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

                    string qry = "ContractDirtDelete";

                    if (msConn.State != System.Data.ConnectionState.Open) { msConn.Open(); }
                    using (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand(qry, msConn)) {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("pCropYear", OLEDB.OleDbType.Integer, 0).Value = cropYear;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "DeleteContractDirtTable";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        //public static void TestQuery() {

        //    try {

        //        using (OLEDB.OleDbConnection msConn = new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

        //            using  (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand("select * from FieldContracting where City = 'Ovid'", msConn)) {

        //                msConn.Open();
        //                using (OLEDB.OleDbDataReader odr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)) {

        //                    while (odr.Read()) {
        //                        int colNum = odr.GetOrdinal("FieldAvgHarvestDate");
                                
        //                    }
        //                }
        //            }
        //        }          

        //    } catch(Exception ex) {
        //        string errMsg = MOD_NAME;
        //        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
        //        throw (wscEx);
        //    }
        //}

        public static void PopulateContractingTable(int cropYear) {

            const string METHOD_NAME = "PopulateContractingTable";
            string tmp = "";

            try {

                //TestQuery();

                // Pull data
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = FieldContractingGetExport(conn, cropYear)) {

                        using (OLEDB.OleDbConnection msConn =
                                  new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

                            string qry = "FieldContractingInsert";

                            if (msConn.State != System.Data.ConnectionState.Open) { msConn.Open(); }
                            using (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand(qry, msConn)) {

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("pSHID", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pCropYear", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pBusinessName", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pTaxID", OLEDB.OleDbType.VarChar, 12);
                                cmd.Parameters.Add("pAddrLine1", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pAddrLine2", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pCity", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pState", OLEDB.OleDbType.VarChar, 2);
                                cmd.Parameters.Add("pPostalCode", OLEDB.OleDbType.VarChar, 11);
                                cmd.Parameters.Add("pPhone", OLEDB.OleDbType.VarChar, 16);
                                cmd.Parameters.Add("pFax", OLEDB.OleDbType.VarChar, 16);
                                cmd.Parameters.Add("pCellPhone", OLEDB.OleDbType.VarChar, 16);
                                cmd.Parameters.Add("pOther", OLEDB.OleDbType.VarChar, 40);
                                cmd.Parameters.Add("pEmail", OLEDB.OleDbType.VarChar, 50);
                                cmd.Parameters.Add("pContractNumber", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAgriculturist", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pContractFactoryNo", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pContractFactoryName", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pContractStationNumber", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pContractStationName", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pFieldName", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pFieldSequenceNo", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldCounty", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pFieldState", OLEDB.OleDbType.VarChar, 2);
                                cmd.Parameters.Add("pFieldTownship", OLEDB.OleDbType.VarChar, 3);
                                cmd.Parameters.Add("pFieldRange", OLEDB.OleDbType.VarChar, 4);
                                cmd.Parameters.Add("pFieldSection", OLEDB.OleDbType.VarChar, 2);
                                cmd.Parameters.Add("pFieldQuadrant", OLEDB.OleDbType.VarChar, 2);
                                cmd.Parameters.Add("pFieldQuarterQuandrant", OLEDB.OleDbType.VarChar, 2);
                                cmd.Parameters.Add("pFieldLatitude", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldLatitude"].Precision = 18;
                                cmd.Parameters["pFieldLatitude"].Scale = 6;
                                cmd.Parameters.Add("pFieldLongitude", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldLongitude"].Precision = 18;
                                cmd.Parameters["pFieldLongitude"].Scale = 6;
                                cmd.Parameters.Add("pFieldDescription", OLEDB.OleDbType.VarChar, 100);
                                cmd.Parameters.Add("pFieldAcres", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFSAOfficial", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldFSANumber", OLEDB.OleDbType.VarChar, 26);
                                cmd.Parameters.Add("pFieldFSAState", OLEDB.OleDbType.VarChar, 2);
                                cmd.Parameters.Add("pFieldFSACounty", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pFieldFarmNumber", OLEDB.OleDbType.VarChar, 5);
                                cmd.Parameters.Add("pFieldTractNumber", OLEDB.OleDbType.VarChar, 5);
                                cmd.Parameters.Add("pFieldNumber", OLEDB.OleDbType.VarChar, 4);
                                cmd.Parameters.Add("pFieldQuarterField", OLEDB.OleDbType.VarChar, 2);
                                cmd.Parameters.Add("pFieldRotationLength", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPriorCrop", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pFieldYearsHavingBeets", OLEDB.OleDbType.VarChar, 15);
                                cmd.Parameters.Add("pFieldSuspectRhizomania", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSuspectAphanomyces", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSuspectCurlyTop", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSuspectFusarium", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSuspectRhizoctonia", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSuspectNematode", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSuspectCercospora", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSuspectRootAphid", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSuspectPowderyMildew", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldWaterSource", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pFieldIrrigationSystem", OLEDB.OleDbType.VarChar, 20);
                                // Added 2/2007
                                cmd.Parameters.Add("pLandOwner", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pFieldPostAphanomyces", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostCercospora", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostCurlyTop", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostFusarium", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostNematode", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostPowderyMildew", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostRhizoctonia", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostRhizomania", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostRootAphid", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pSampleGridZone", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldPostWater", OLEDB.OleDbType.Boolean, 0);
                                // Added 2/2008
                                cmd.Parameters.Add("pFieldOwnership", OLEDB.OleDbType.VarChar, 15);
                                cmd.Parameters.Add("pFieldTillage", OLEDB.OleDbType.VarChar, 30);
                                // Added 11/2009
                                cmd.Parameters.Add("pFieldAvgHarvestDate", OLEDB.OleDbType.Date);

                                cmd.Prepare();
                                
                                while (dr.Read()) {

                                    cmd.Parameters["pSHID"].Value = dr.GetString(dr.GetOrdinal("SHID"));
                                    cmd.Parameters["pCropYear"].Value = dr.GetInt32(dr.GetOrdinal("CropYear"));
                                    cmd.Parameters["pBusinessName"].Value = dr.GetString(dr.GetOrdinal("BusinessName"));
                                    cmd.Parameters["pTaxID"].Value = dr.GetString(dr.GetOrdinal("TaxID"));
                                    cmd.Parameters["pAddrLine1"].Value = dr.GetString(dr.GetOrdinal("AddrLine1"));
                                    cmd.Parameters["pAddrLine2"].Value = dr.GetString(dr.GetOrdinal("AddrLine2"));
                                    cmd.Parameters["pCity"].Value = dr.GetString(dr.GetOrdinal("City"));
                                    cmd.Parameters["pState"].Value = dr.GetString(dr.GetOrdinal("State"));
                                    cmd.Parameters["pPostalCode"].Value = dr.GetString(dr.GetOrdinal("PostalCode"));
                                    cmd.Parameters["pPhone"].Value = dr.GetString(dr.GetOrdinal("Phone"));
                                    cmd.Parameters["pFax"].Value = dr.GetString(dr.GetOrdinal("Fax"));
                                    cmd.Parameters["pCellPhone"].Value = dr.GetString(dr.GetOrdinal("CellPhone"));
                                    cmd.Parameters["pOther"].Value = dr.GetString(dr.GetOrdinal("Other"));
                                    cmd.Parameters["pEmail"].Value = dr.GetString(dr.GetOrdinal("Email"));
                                    cmd.Parameters["pContractNumber"].Value = Convert.ToInt32(dr.GetString(dr.GetOrdinal("ContractNumber")));
                                    cmd.Parameters["pAgriculturist"].Value = dr.GetString(dr.GetOrdinal("Agriculturist"));
                                    cmd.Parameters["pContractFactoryNo"].Value = dr.GetInt16(dr.GetOrdinal("ContractFactoryNo"));
                                    cmd.Parameters["pContractFactoryName"].Value = dr.GetString(dr.GetOrdinal("ContractFactoryName"));
                                    cmd.Parameters["pContractStationNumber"].Value = dr.GetInt16(dr.GetOrdinal("ContractStationNumber"));
                                    cmd.Parameters["pContractStationName"].Value = dr.GetString(dr.GetOrdinal("ContractStationName"));
                                    cmd.Parameters["pFieldName"].Value = dr.GetString(dr.GetOrdinal("FieldName"));
                                    cmd.Parameters["pFieldSequenceNo"].Value = dr.GetInt16(dr.GetOrdinal("FieldSequenceNo"));
                                    cmd.Parameters["pFieldCounty"].Value = dr.GetString(dr.GetOrdinal("FieldCounty"));
                                    cmd.Parameters["pFieldState"].Value = dr.GetString(dr.GetOrdinal("FieldState"));
                                    cmd.Parameters["pFieldTownship"].Value = dr.GetString(dr.GetOrdinal("FieldTownship"));
                                    cmd.Parameters["pFieldRange"].Value = dr.GetString(dr.GetOrdinal("FieldRange"));
                                    cmd.Parameters["pFieldSection"].Value = dr.GetString(dr.GetOrdinal("FieldSection"));
                                    cmd.Parameters["pFieldQuadrant"].Value = dr.GetString(dr.GetOrdinal("FieldQuadrant"));
                                    cmd.Parameters["pFieldQuarterQuandrant"].Value = dr.GetString(dr.GetOrdinal("FieldQuadrant"));
                                    cmd.Parameters["pFieldLatitude"].Value = dr.GetDecimal(dr.GetOrdinal("FieldLatitude"));
                                    cmd.Parameters["pFieldLongitude"].Value = dr.GetDecimal(dr.GetOrdinal("FieldLongitude"));
                                    cmd.Parameters["pFieldDescription"].Value = dr.GetString(dr.GetOrdinal("FieldDescription"));
                                    cmd.Parameters["pFieldAcres"].Value = dr.GetInt32(dr.GetOrdinal("FieldAcres"));
                                    cmd.Parameters["pFSAOfficial"].Value = (dr.GetString(dr.GetOrdinal("FSAOfficial")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldFSANumber"].Value = dr.GetString(dr.GetOrdinal("FieldFSANumber"));
                                    cmd.Parameters["pFieldFSAState"].Value = dr.GetString(dr.GetOrdinal("FieldFSAState"));
                                    cmd.Parameters["pFieldFSACounty"].Value = dr.GetString(dr.GetOrdinal("FieldFSACounty"));
                                    cmd.Parameters["pFieldFarmNumber"].Value = dr.GetString(dr.GetOrdinal("FieldFarmNumber"));
                                    cmd.Parameters["pFieldTractNumber"].Value = dr.GetString(dr.GetOrdinal("FieldTractNumber"));
                                    cmd.Parameters["pFieldNumber"].Value = dr.GetString(dr.GetOrdinal("FieldNumber"));
                                    cmd.Parameters["pFieldQuarterField"].Value = dr.GetString(dr.GetOrdinal("FieldQuarterField"));
                                    cmd.Parameters["pFieldRotationLength"].Value = dr.GetString(dr.GetOrdinal("FieldRotationLength"));
                                    cmd.Parameters["pFieldPriorCrop"].Value = dr.GetString(dr.GetOrdinal("FieldPriorCrop"));
                                    cmd.Parameters["pFieldYearsHavingBeets"].Value = dr.GetString(dr.GetOrdinal("FieldYearsHavingBeets"));
                                    cmd.Parameters["pFieldSuspectRhizomania"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectRhizomania")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSuspectAphanomyces"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectAphanomyces")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSuspectCurlyTop"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectCurlyTop")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSuspectFusarium"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectFusarium")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSuspectRhizoctonia"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectRhizoctonia")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSuspectNematode"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectNematode")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSuspectCercospora"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectCercospora")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSuspectRootAphid"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectRootAphid")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSuspectPowderyMildew"].Value = (dr.GetString(dr.GetOrdinal("FieldSuspectPowderyMildew")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldWaterSource"].Value = dr.GetString(dr.GetOrdinal("FieldWaterSource"));
                                    cmd.Parameters["pFieldIrrigationSystem"].Value = dr.GetString(dr.GetOrdinal("FieldIrrigationSystem"));
                                    // Added 2/2007
                                    cmd.Parameters["pLandOwner"].Value = dr.GetString(dr.GetOrdinal("LandOwner"));
                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostAphanomyces"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostAphanomyces"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostAphanomyces"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostCercospora"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostCercospora"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostCercospora"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostCurlyTop"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostCurlyTop"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostCurlyTop"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostFusarium"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostFusarium"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostFusarium"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostNematode"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostNematode"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostNematode"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostPowderyMildew"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostPowderyMildew"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostPowderyMildew"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostRhizoctonia"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostRhizoctonia"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostRhizoctonia"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostRhizomania"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostRhizomania"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostRhizomania"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostRootAphid"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostRootAphid"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostRootAphid"].Value = tmp;
                                    }

                                    cmd.Parameters["pSampleGridZone"].Value = (dr.GetString(dr.GetOrdinal("SampleGridZone")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldPostWater"].Value = (dr.GetString(dr.GetOrdinal("FieldPostWater")) == "Y" ? true : false);

                                    tmp = dr.GetString(dr.GetOrdinal("FieldOwnership"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldOwnership"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldOwnership"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldTillage"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldTillage"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldTillage"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("AvgDeliveryDate"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldAvgHarvestDate"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldAvgHarvestDate"].Value = tmp;
                                    }

                                    try {
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (OLEDB.OleDbException oex) {
                                        string errMsg = MOD_NAME + METHOD_NAME + ": OLEDB exception calling ExecuteQuery.";
                                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, oex);
                                        throw (wscEx);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME + ": tmp: " + tmp;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void PopulateAgronomyTable(int cropYear) {

            try {

                // Pull data
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = FieldAgronomyGetExport(conn, cropYear)) {

                        using (OLEDB.OleDbConnection msConn =
                                  new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

                            string qry = "FieldAgronomyInsert";

                            if (msConn.State != System.Data.ConnectionState.Open) { msConn.Open(); }
                            using (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand(qry, msConn)) {

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("pSHID", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pCropYear", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pContractNumber", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldSequenceNo", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldSeedVariety", OLEDB.OleDbType.VarChar, 16);
                                cmd.Parameters.Add("pFieldSeedPrimed", OLEDB.OleDbType.VarChar, 12);
                                cmd.Parameters.Add("pFieldSeedRxChemical", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pFieldRowSpacing", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldPlantingDate", OLEDB.OleDbType.DBDate, 0);
                                cmd.Parameters.Add("pFieldPlantSpacing", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldPlantSpacing"].Precision = 18;
                                cmd.Parameters["pFieldPlantSpacing"].Scale = 2;
                                cmd.Parameters.Add("pFieldRePlantingDate", OLEDB.OleDbType.DBDate, 0);
                                cmd.Parameters.Add("pFieldReplantSeedVariety", OLEDB.OleDbType.VarChar, 16);
                                cmd.Parameters.Add("pFieldAcresReplanted", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldReplantReason", OLEDB.OleDbType.VarChar, 60);
                                cmd.Parameters.Add("pFieldTestSeason", OLEDB.OleDbType.VarChar, 6);
                                cmd.Parameters.Add("pFieldTestDepth", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldTest_N", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldTest_N"].Precision = 18;
                                cmd.Parameters["pFieldTest_N"].Scale = 2;
                                cmd.Parameters.Add("pFieldTest_P", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldTest_K", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldTest_K"].Precision = 18;
                                cmd.Parameters["pFieldTest_K"].Scale = 2;
                                cmd.Parameters.Add("pFieldTest_pH", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldTest_pH"].Precision = 18;
                                cmd.Parameters["pFieldTest_pH"].Scale = 2;
                                cmd.Parameters.Add("pFieldTest_OrgMat", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldTest_OrgMat"].Precision = 18;
                                cmd.Parameters["pFieldTest_OrgMat"].Scale = 2;
                                cmd.Parameters.Add("pFieldLastYrManure", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldFertFall_N", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertFall_N"].Precision = 18;
                                cmd.Parameters["pFieldFertFall_N"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertFall_P", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertFall_P"].Precision = 18;
                                cmd.Parameters["pFieldFertFall_P"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertFall_K", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertFall_K"].Precision = 18;
                                cmd.Parameters["pFieldFertFall_K"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertSpring_N", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertSpring_N"].Precision = 18;
                                cmd.Parameters["pFieldFertSpring_N"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertSpring_P", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertSpring_P"].Precision = 18;
                                cmd.Parameters["pFieldFertSpring_P"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertSpring_K", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertSpring_K"].Precision = 18;
                                cmd.Parameters["pFieldFertSpring_K"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertInSeason_N", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertInSeason_N"].Precision = 18;
                                cmd.Parameters["pFieldFertInSeason_N"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertInSeason_P", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertInSeason_P"].Precision = 18;
                                cmd.Parameters["pFieldFertInSeason_P"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertInSeason_K", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldFertInSeason_K"].Precision = 18;
                                cmd.Parameters["pFieldFertInSeason_K"].Scale = 2;
                                cmd.Parameters.Add("pFieldFertStarter", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldPreInsecticide", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldPostInsecticide", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldPreWeedControl", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldLaybyHerbicide", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldLaybyChemical", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pFieldRootMaggotInsecticide", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldCounterLbs", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldCounterLbs"].Precision = 18;
                                cmd.Parameters["pFieldCounterLbs"].Scale = 2;
                                cmd.Parameters.Add("pFieldTemikLbs", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldTemikLbs"].Precision = 18;
                                cmd.Parameters["pFieldTemikLbs"].Scale = 2;
                                cmd.Parameters.Add("pFieldThimetLbs", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldThimetLbs"].Precision = 18;
                                cmd.Parameters["pFieldThimetLbs"].Scale = 2;
                                cmd.Parameters.Add("pFieldCercspApp1Chem", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pFieldCercosporaApp1Date", OLEDB.OleDbType.DBDate, 20);
                                cmd.Parameters.Add("pFieldCercspApp2Chem", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pFieldCercosporaApp2Date", OLEDB.OleDbType.DBDate, 20);
                                cmd.Parameters.Add("pFieldCercspApp3Chem", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pFieldCercosporaApp3Date", OLEDB.OleDbType.DBDate, 20);
                                cmd.Parameters.Add("pFieldHailStress", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldWeedControl", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldTreatedPowderyMildew", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldTreatedNematode", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldTreatedRhizoctonia", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldAgronomyReviewed", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldAgronomyInclude", OLEDB.OleDbType.Boolean, 0);
                                // Added 2/2007: changed 2/2008
                                cmd.Parameters.Add("pLandOwner", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pFieldPostAphanomyces", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostCercospora", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostCurlyTop", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostFusarium", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostNematode", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostPowderyMildew", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostRhizoctonia", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostRhizomania", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostRootAphid", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pFieldPostWater", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldGridZone", OLEDB.OleDbType.Boolean, 0);
                                // Added 2/2008
                                cmd.Parameters.Add("pFieldOwnership", OLEDB.OleDbType.VarChar, 15);
                                cmd.Parameters.Add("pFieldTillage", OLEDB.OleDbType.VarChar, 30);

                                cmd.Parameters.Add("pFieldSoilTexture", OLEDB.OleDbType.VarChar, 15);
                                cmd.Parameters.Add("pFieldTestSalts", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pFieldTestSalts"].Precision = 18;
                                cmd.Parameters["pFieldTestSalts"].Scale = 4;
                                cmd.Parameters.Add("pFieldHerbicideRxCount", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldEmerg80Date", OLEDB.OleDbType.DBDate, 0);
                                cmd.Parameters.Add("pFieldSeedRxPoncho", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSeedRxTachigaren", OLEDB.OleDbType.Boolean, 0);
                                cmd.Parameters.Add("pFieldSeedRxCruiser", OLEDB.OleDbType.Boolean, 0);

                                cmd.Prepare();

                                string tmp = "";
                                while (dr.Read()) {

                                    cmd.Parameters["pSHID"].Value = dr.GetString(dr.GetOrdinal("SHID"));
                                    cmd.Parameters["pCropYear"].Value = dr.GetInt32(dr.GetOrdinal("CropYear"));
                                    cmd.Parameters["pContractNumber"].Value = Convert.ToInt32(dr.GetString(dr.GetOrdinal("ContractNumber")));
                                    cmd.Parameters["pFieldSequenceNo"].Value = dr.GetInt16(dr.GetOrdinal("FieldSequenceNo"));
                                    cmd.Parameters["pFieldSeedVariety"].Value = dr.GetString(dr.GetOrdinal("FieldSeedVariety"));
                                    cmd.Parameters["pFieldSeedPrimed"].Value = dr.GetString(dr.GetOrdinal("FieldSeedPrimed"));
                                    cmd.Parameters["pFieldSeedRxChemical"].Value = dr.GetString(dr.GetOrdinal("FieldSeedRxChemical"));
                                    cmd.Parameters["pFieldRowSpacing"].Value = dr.GetInt16(dr.GetOrdinal("FieldRowSpacing"));

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPlantingDate"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPlantingDate"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPlantingDate"].Value = tmp;
                                    }

                                    cmd.Parameters["pFieldPlantSpacing"].Value = dr.GetDecimal(dr.GetOrdinal("FieldPlantSpacing"));

                                    tmp = dr.GetString(dr.GetOrdinal("FieldRePlantingDate"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldRePlantingDate"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldRePlantingDate"].Value = tmp;
                                    }
                                    cmd.Parameters["pFieldReplantSeedVariety"].Value = dr.GetString(dr.GetOrdinal("FieldReplantSeedVariety"));
                                    cmd.Parameters["pFieldAcresReplanted"].Value = dr.GetInt32(dr.GetOrdinal("FieldAcresReplanted"));
                                    cmd.Parameters["pFieldReplantReason"].Value = dr.GetString(dr.GetOrdinal("FieldReplantReason"));
                                    cmd.Parameters["pFieldTestSeason"].Value = dr.GetString(dr.GetOrdinal("FieldTestSeason"));
                                    cmd.Parameters["pFieldTestDepth"].Value = dr.GetInt16(dr.GetOrdinal("FieldTestDepth"));
                                    cmd.Parameters["pFieldTest_N"].Value = dr.GetDecimal(dr.GetOrdinal("FieldTest_N"));
                                    cmd.Parameters["pFieldTest_P"].Value = dr.GetString(dr.GetOrdinal("FieldTest_P"));
                                    cmd.Parameters["pFieldTest_K"].Value = dr.GetDecimal(dr.GetOrdinal("FieldTest_K"));
                                    cmd.Parameters["pFieldTest_pH"].Value = dr.GetDecimal(dr.GetOrdinal("FieldTest_pH"));
                                    cmd.Parameters["pFieldTest_OrgMat"].Value = dr.GetDecimal(dr.GetOrdinal("FieldTest_OrgMat"));
                                    cmd.Parameters["pFieldLastYrManure"].Value = dr.GetInt32(dr.GetOrdinal("FieldLastYrManure"));
                                    cmd.Parameters["pFieldFertFall_N"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertFall_N"));
                                    cmd.Parameters["pFieldFertFall_P"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertFall_P"));
                                    cmd.Parameters["pFieldFertFall_K"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertFall_K"));
                                    cmd.Parameters["pFieldFertSpring_N"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertSpring_N"));
                                    cmd.Parameters["pFieldFertSpring_P"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertSpring_P"));
                                    cmd.Parameters["pFieldFertSpring_K"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertSpring_K"));
                                    cmd.Parameters["pFieldFertInSeason_N"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertInSeason_N"));
                                    cmd.Parameters["pFieldFertInSeason_P"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertInSeason_P"));
                                    cmd.Parameters["pFieldFertInSeason_K"].Value = dr.GetDecimal(dr.GetOrdinal("FieldFertInSeason_K"));
                                    cmd.Parameters["pFieldFertStarter"].Value = (dr.GetString(dr.GetOrdinal("FieldFertStarter")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldPreInsecticide"].Value = (dr.GetString(dr.GetOrdinal("FieldPreInsecticide")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldPostInsecticide"].Value = (dr.GetString(dr.GetOrdinal("FieldPostInsecticide")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldPreWeedControl"].Value = (dr.GetString(dr.GetOrdinal("FieldPreWeedControl")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldLaybyHerbicide"].Value = (dr.GetString(dr.GetOrdinal("FieldLaybyHerbicide")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldLaybyChemical"].Value = dr.GetString(dr.GetOrdinal("FieldLaybyChemical"));
                                    cmd.Parameters["pFieldRootMaggotInsecticide"].Value = (dr.GetString(dr.GetOrdinal("FieldRootMaggotInsecticide")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldCounterLbs"].Value = dr.GetDecimal(dr.GetOrdinal("FieldCounterLbs"));
                                    cmd.Parameters["pFieldTemikLbs"].Value = dr.GetDecimal(dr.GetOrdinal("FieldTemikLbs"));
                                    cmd.Parameters["pFieldThimetLbs"].Value = dr.GetDecimal(dr.GetOrdinal("FieldThimetLbs"));
                                    cmd.Parameters["pFieldCercspApp1Chem"].Value = dr.GetString(dr.GetOrdinal("FieldCercspApp1Chem"));

                                    tmp = dr.GetString(dr.GetOrdinal("FieldCercosporaApp1Date"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldCercosporaApp1Date"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldCercosporaApp1Date"].Value = tmp;
                                    }
                                    cmd.Parameters["pFieldCercspApp2Chem"].Value = dr.GetString(dr.GetOrdinal("FieldCercspApp2Chem"));

                                    tmp = dr.GetString(dr.GetOrdinal("FieldCercosporaApp2Date"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldCercosporaApp2Date"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldCercosporaApp2Date"].Value = tmp;
                                    }
                                    cmd.Parameters["pFieldCercspApp3Chem"].Value = dr.GetString(dr.GetOrdinal("FieldCercspApp3Chem"));

                                    tmp = dr.GetString(dr.GetOrdinal("FieldCercosporaApp3Date"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldCercosporaApp3Date"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldCercosporaApp3Date"].Value = tmp;
                                    }
                                    cmd.Parameters["pFieldHailStress"].Value = dr.GetString(dr.GetOrdinal("FieldHailStress"));
                                    cmd.Parameters["pFieldWeedControl"].Value = dr.GetString(dr.GetOrdinal("FieldWeedControl"));
                                    cmd.Parameters["pFieldTreatedPowderyMildew"].Value = (dr.GetString(dr.GetOrdinal("FieldTreatedPowderyMildew")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldTreatedNematode"].Value = (dr.GetString(dr.GetOrdinal("FieldTreatedNematode")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldTreatedRhizoctonia"].Value = (dr.GetString(dr.GetOrdinal("FieldTreatedRhizoctonia")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldAgronomyReviewed"].Value = (dr.GetString(dr.GetOrdinal("FieldAgronomyReviewed")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldAgronomyInclude"].Value = (dr.GetString(dr.GetOrdinal("FieldAgronomyInclude")) == "Y" ? true : false);
                                    // Added 2/2007
                                    cmd.Parameters["pLandOwner"].Value = dr.GetString(dr.GetOrdinal("LandOwner"));

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostAphanomyces"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostAphanomyces"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostAphanomyces"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostCercospora"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostCercospora"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostCercospora"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostCurlyTop"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostCurlyTop"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostCurlyTop"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostFusarium"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostFusarium"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostFusarium"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostNematode"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostNematode"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostNematode"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostPowderyMildew"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostPowderyMildew"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostPowderyMildew"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostRhizoctonia"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostRhizoctonia"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostRhizoctonia"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostRhizomania"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostRhizomania"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostRhizomania"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldPostRootAphid"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldPostRootAphid"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldPostRootAphid"].Value = tmp;
                                    }

                                    cmd.Parameters["pFieldPostWater"].Value = (dr.GetString(dr.GetOrdinal("FieldPostWater")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldGridZone"].Value = (dr.GetString(dr.GetOrdinal("FieldGridZone")) == "Y" ? true : false);

                                    tmp = dr.GetString(dr.GetOrdinal("FieldOwnership"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldOwnership"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldOwnership"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldTillage"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldTillage"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldTillage"].Value = tmp;
                                    }

                                    tmp = dr.GetString(dr.GetOrdinal("FieldSoilTexture"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldSoilTexture"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldSoilTexture"].Value = tmp;
                                    }

                                    cmd.Parameters["pFieldTestSalts"].Value = dr.GetDecimal(dr.GetOrdinal("FieldTestSalts"));
                                    cmd.Parameters["pFieldHerbicideRxCount"].Value = dr.GetInt32(dr.GetOrdinal("FieldHerbicideRxCount"));

                                    tmp = dr.GetString(dr.GetOrdinal("FieldEmerg80Date"));
                                    if (tmp.Length == 0) {
                                        cmd.Parameters["pFieldEmerg80Date"].Value = DBNull.Value;
                                    } else {
                                        cmd.Parameters["pFieldEmerg80Date"].Value = tmp;
                                    }

                                    cmd.Parameters["pFieldSeedRxPoncho"].Value = (dr.GetString(dr.GetOrdinal("FieldSeedRxPoncho")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSeedRxTachigaren"].Value = (dr.GetString(dr.GetOrdinal("FieldSeedRxTachigaren")) == "Y" ? true : false);
                                    cmd.Parameters["pFieldSeedRxCruiser"].Value = (dr.GetString(dr.GetOrdinal("FieldSeedRxCruiser")) == "Y" ? true : false);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "PopulateAgronomyTable";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void PopulateBeetAccountingPerformance(int cropYear) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    FieldPerformanceSave(conn, cropYear);
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "PopulateBeetAccountingPerformance";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void PopulateGrowerPerformanceTable(int cropYear) {

            try {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    GrowerPerformanceSave(conn, cropYear);
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "PopulateGrowerPerformanceTable";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }


        public static void PopulateBeetAccountingPerformance2(int cropYear) {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    FieldPerformance2Save(conn, cropYear);
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "PopulateBeetAccountingPerformance2";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void PopulatePerformanceTable(int cropYear) {

            try {

                // Pull data
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCField.FieldPerformanceGet(conn, cropYear, 0, 0)) {

                        using (OLEDB.OleDbConnection msConn =
                                  new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

                            string qry = "FieldPerformanceInsert";

                            if (msConn.State != System.Data.ConnectionState.Open) { msConn.Open(); }
                            using (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand(qry, msConn)) {

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("pSHID", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pCropYear", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldName", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pContractNo", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldStateName", OLEDB.OleDbType.VarChar, 2);
                                cmd.Parameters.Add("pFieldCountyName", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pAreaName", OLEDB.OleDbType.VarChar, 60);
                                cmd.Parameters.Add("pRegionName", OLEDB.OleDbType.VarChar, 60);
                                cmd.Parameters.Add("pAcresContracted", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAcresHarvested", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pSugarPct", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pSugarPct"].Precision = 18;
                                cmd.Parameters["pSugarPct"].Scale = 2;
                                cmd.Parameters.Add("pExtractableSugar", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pExtractableSugar"].Precision = 18;
                                cmd.Parameters["pExtractableSugar"].Scale = 4;
                                cmd.Parameters.Add("pTonsPerAcre", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pTonsPerAcre"].Precision = 18;
                                cmd.Parameters["pTonsPerAcre"].Scale = 4;
                                cmd.Parameters.Add("pGrossDollarsPerAcre", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pGrossDollarsPerAcre"].Precision = 18;
                                cmd.Parameters["pGrossDollarsPerAcre"].Scale = 4;
                                cmd.Parameters.Add("pSLMPct", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pSLMPct"].Precision = 18;
                                cmd.Parameters["pSLMPct"].Scale = 4;
                                cmd.Parameters.Add("pTarePct", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pTarePct"].Precision = 18;
                                cmd.Parameters["pTarePct"].Scale = 4;
                                cmd.Parameters.Add("pTotalAvailableNitrogen", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pTotalAvailableNitrogen"].Precision = 18;
                                cmd.Parameters["pTotalAvailableNitrogen"].Scale = 4;
                                cmd.Parameters.Add("pTotalAvailableNitrogenGroup", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCountyCount", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAreaCount", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pRegionCount", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCoopCount", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCountySugarPctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCountyExtractableSugarRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCountyTonsPerAcreRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCountyGrossDollarsPerAcreRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCountySlmPctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCountyTarePctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAreaSugarPctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAreaExtractableSugarRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAreaTonsPerAcreRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAreaGrossDollarsPerAcreRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAreaSlmPctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAreaTarePctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pRegionSugarPctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pRegionExtractableSugarRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pRegionTonsPerAcreRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pRegionGrossDollarsPerAcreRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pRegionSlmPctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pRegionTarePctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCoopSugarPctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCoopExtractableSugarRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCoopTonsPerAcreRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCoopGrossDollarsPerAcreRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCoopSlmPctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pCoopTarePctRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pBeetsPerAcre", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pAvgTopping", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pAvgTopping"].Precision = 18;
                                cmd.Parameters["pAvgTopping"].Scale = 2;

                                cmd.Prepare();

                                while (dr.Read()) {

                                    cmd.Parameters["pSHID"].Value = dr.GetString(dr.GetOrdinal("SHID"));
                                    cmd.Parameters["pCropYear"].Value = dr.GetInt32(dr.GetOrdinal("CropYear"));
                                    cmd.Parameters["pFieldName"].Value = dr.GetString(dr.GetOrdinal("FieldName"));
                                    cmd.Parameters["pContractNo"].Value = dr.GetInt32(dr.GetOrdinal("ContractNo"));
                                    cmd.Parameters["pFieldStateName"].Value = dr.GetString(dr.GetOrdinal("FieldStateName"));
                                    cmd.Parameters["pFieldCountyName"].Value = dr.GetString(dr.GetOrdinal("FieldCountyName"));
                                    cmd.Parameters["pAreaName"].Value = dr.GetString(dr.GetOrdinal("AreaName"));
                                    cmd.Parameters["pRegionName"].Value = dr.GetString(dr.GetOrdinal("RegionName"));
                                    cmd.Parameters["pAcresContracted"].Value = dr.GetInt32(dr.GetOrdinal("AcresContracted"));
                                    cmd.Parameters["pAcresHarvested"].Value = dr.GetInt32(dr.GetOrdinal("AcresHarvested"));
                                    cmd.Parameters["pSugarPct"].Value = dr.GetDecimal(dr.GetOrdinal("SugarPct"));
                                    cmd.Parameters["pExtractableSugar"].Value = dr.GetDecimal(dr.GetOrdinal("ExtractableSugar"));
                                    cmd.Parameters["pTonsPerAcre"].Value = dr.GetDecimal(dr.GetOrdinal("TonsPerAcre"));
                                    cmd.Parameters["pGrossDollarsPerAcre"].Value = dr.GetDecimal(dr.GetOrdinal("GrossDollarsPerAcre"));
                                    cmd.Parameters["pSLMPct"].Value = dr.GetDecimal(dr.GetOrdinal("SLMPct"));
                                    cmd.Parameters["pTarePct"].Value = dr.GetDecimal(dr.GetOrdinal("TarePct"));
                                    cmd.Parameters["pTotalAvailableNitrogen"].Value = dr.GetDecimal(dr.GetOrdinal("TotalAvailableNitrogen"));
                                    cmd.Parameters["pTotalAvailableNitrogenGroup"].Value = dr.GetInt32(dr.GetOrdinal("TotalAvailableNitrogenGroup"));
                                    cmd.Parameters["pCountyCount"].Value = dr.GetInt32(dr.GetOrdinal("CountyCount"));
                                    cmd.Parameters["pAreaCount"].Value = dr.GetInt32(dr.GetOrdinal("AreaCount"));
                                    cmd.Parameters["pRegionCount"].Value = dr.GetInt32(dr.GetOrdinal("RegionCount"));
                                    cmd.Parameters["pCoopCount"].Value = dr.GetInt32(dr.GetOrdinal("CoopCount"));
                                    cmd.Parameters["pCountySugarPctRank"].Value = dr.GetInt32(dr.GetOrdinal("CountySugarPctRank"));
                                    cmd.Parameters["pCountyExtractableSugarRank"].Value = dr.GetInt32(dr.GetOrdinal("CountyExtractableSugarRank"));
                                    cmd.Parameters["pCountyTonsPerAcreRank"].Value = dr.GetInt32(dr.GetOrdinal("CountyTonsPerAcreRank"));
                                    cmd.Parameters["pCountyGrossDollarsPerAcreRank"].Value = dr.GetInt32(dr.GetOrdinal("CountyGrossDollarsPerAcreRank"));
                                    cmd.Parameters["pCountySlmPctRank"].Value = dr.GetInt32(dr.GetOrdinal("CountySlmPctRank"));
                                    cmd.Parameters["pCountyTarePctRank"].Value = dr.GetInt32(dr.GetOrdinal("CountyTarePctRank"));
                                    cmd.Parameters["pAreaSugarPctRank"].Value = dr.GetInt32(dr.GetOrdinal("AreaSugarPctRank"));
                                    cmd.Parameters["pAreaExtractableSugarRank"].Value = dr.GetInt32(dr.GetOrdinal("AreaExtractableSugarRank"));
                                    cmd.Parameters["pAreaTonsPerAcreRank"].Value = dr.GetInt32(dr.GetOrdinal("AreaTonsPerAcreRank"));
                                    cmd.Parameters["pAreaGrossDollarsPerAcreRank"].Value = dr.GetInt32(dr.GetOrdinal("AreaGrossDollarsPerAcreRank"));
                                    cmd.Parameters["pAreaSlmPctRank"].Value = dr.GetInt32(dr.GetOrdinal("AreaSlmPctRank"));
                                    cmd.Parameters["pAreaTarePctRank"].Value = dr.GetInt32(dr.GetOrdinal("AreaTarePctRank"));
                                    cmd.Parameters["pRegionSugarPctRank"].Value = dr.GetInt32(dr.GetOrdinal("RegionSugarPctRank"));
                                    cmd.Parameters["pRegionExtractableSugarRank"].Value = dr.GetInt32(dr.GetOrdinal("RegionExtractableSugarRank"));
                                    cmd.Parameters["pRegionTonsPerAcreRank"].Value = dr.GetInt32(dr.GetOrdinal("RegionTonsPerAcreRank"));
                                    cmd.Parameters["pRegionGrossDollarsPerAcreRank"].Value = dr.GetInt32(dr.GetOrdinal("RegionGrossDollarsPerAcreRank"));
                                    cmd.Parameters["pRegionSlmPctRank"].Value = dr.GetInt32(dr.GetOrdinal("RegionSlmPctRank"));
                                    cmd.Parameters["pRegionTarePctRank"].Value = dr.GetInt32(dr.GetOrdinal("RegionTarePctRank"));
                                    cmd.Parameters["pCoopSugarPctRank"].Value = dr.GetInt32(dr.GetOrdinal("CoopSugarPctRank"));
                                    cmd.Parameters["pCoopExtractableSugarRank"].Value = dr.GetInt32(dr.GetOrdinal("CoopExtractableSugarRank"));
                                    cmd.Parameters["pCoopTonsPerAcreRank"].Value = dr.GetInt32(dr.GetOrdinal("CoopTonsPerAcreRank"));
                                    cmd.Parameters["pCoopGrossDollarsPerAcreRank"].Value = dr.GetInt32(dr.GetOrdinal("CoopGrossDollarsPerAcreRank"));
                                    cmd.Parameters["pCoopSlmPctRank"].Value = dr.GetInt32(dr.GetOrdinal("CoopSlmPctRank"));
                                    cmd.Parameters["pCoopTarePctRank"].Value = dr.GetInt32(dr.GetOrdinal("CoopTarePctRank"));
                                    cmd.Parameters["pBeetsPerAcre"].Value = dr.GetInt32(dr.GetOrdinal("BeetsPerAcre"));
                                    cmd.Parameters["pAvgTopping"].Value = dr.GetDecimal(dr.GetOrdinal("AvgTopping"));

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "PopulatePerformanceTable";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static void PopulateContractDirtTable(int cropYear) {

            try {

                // Pull data
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCField.FieldPerformance2GetYear(conn, cropYear)) {

                        using (OLEDB.OleDbConnection msConn =
                                  new OLEDB.OleDbConnection(Globals.BeetExportConnectionString())) {

                            string qry = "ContractDirtInsert";

                            if (msConn.State != System.Data.ConnectionState.Open) { msConn.Open(); }
                            using (OLEDB.OleDbCommand cmd = new OLEDB.OleDbCommand(qry, msConn)) {

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("pSHID", OLEDB.OleDbType.VarChar, 10);
                                cmd.Parameters.Add("pCropYear", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pFieldName", OLEDB.OleDbType.VarChar, 20);
                                cmd.Parameters.Add("pFieldSequenceNo", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pContractNo", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pStationNo", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pStationName", OLEDB.OleDbType.VarChar, 30);
                                cmd.Parameters.Add("pDirtPercent", OLEDB.OleDbType.Decimal, 0);
                                cmd.Parameters["pDirtPercent"].Precision = 18;
                                cmd.Parameters["pDirtPercent"].Scale = 4;
                                cmd.Parameters.Add("pStationContracts", OLEDB.OleDbType.Integer, 0);
                                cmd.Parameters.Add("pDirtPercentRank", OLEDB.OleDbType.Integer, 0);
                                cmd.Prepare();

                                while (dr.Read()) {

                                    cmd.Parameters["pSHID"].Value = dr.GetString(dr.GetOrdinal("fldp2_shid"));
                                    cmd.Parameters["pCropYear"].Value = dr.GetInt32(dr.GetOrdinal("fldp2_crop_year"));
                                    cmd.Parameters["pFieldName"].Value = dr.GetString(dr.GetOrdinal("fldp2_field_name"));
                                    cmd.Parameters["pFieldSequenceNo"].Value = dr.GetInt32(dr.GetOrdinal("fldp2_field_sequence_no"));
                                    cmd.Parameters["pContractNo"].Value = dr.GetInt32(dr.GetOrdinal("fldp2_contract_no"));
                                    cmd.Parameters["pStationNo"].Value = dr.GetInt32(dr.GetOrdinal("fldp2_station_no"));
                                    cmd.Parameters["pStationName"].Value = dr.GetString(dr.GetOrdinal("fldp2_station_name"));
                                    cmd.Parameters["pDirtPercent"].Value = dr.GetDecimal(dr.GetOrdinal("fldp2_dirt_pct"));
                                    cmd.Parameters["pStationContracts"].Value = dr.GetInt32(dr.GetOrdinal("fldp2_station_contracts"));
                                    cmd.Parameters["pDirtPercentRank"].Value = dr.GetInt32(dr.GetOrdinal("fldp2_dirt_pct_rank"));

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + "PopulateContractDirtTable";
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public static SqlDataReader FieldContractingGetExport(SqlConnection conn, int cropYear) {

            const string METHOD_NAME = "FieldContractingGetExport";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldContractingExport";

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

        public static SqlDataReader FieldAgronomyGetExport(SqlConnection conn, int cropYear) {

            const string METHOD_NAME = "FieldAgronomyGetExport";
            SqlDataReader dr = null;

            try {

                string procName = "bawpFieldAgronomyExport";

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

        public static void GrowerPerformanceSave(SqlConnection conn, int cropYear) {

            const string METHOD_NAME = "GrowerPerformanceSave";
            try {

                string procName = "bawpGrowerPerformanceSave";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
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
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

        }

        public static void FieldPerformanceSave(SqlConnection conn, int cropYear) {

            const string METHOD_NAME = "FieldPerformanceSave";
            try {

                string procName = "bawpFieldPerformanceSave";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
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
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

        }

        public static void FieldPerformance2Save(SqlConnection conn, int cropYear) {

            const string METHOD_NAME = "FieldPerformance2Save";

            try {

                string procName = "bawpFieldPerformance2Save";

                if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                System.Data.SqlClient.SqlParameter[] spParams =
                    SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                spParams[0].Value = cropYear;
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
}
