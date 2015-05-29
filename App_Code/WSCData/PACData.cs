using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MDAAB.Classic;

namespace WSCData {
    /// <summary>
    /// Summary description for PACAdministration
    /// </summary
    public class PACData {

        private const string MOD_NAME = "WSCData.";
        private static string LF = System.Environment.NewLine;

        public static List<Individual> GetPACIndividuals(int id, string name) {

            const string METHOD_NAME = "GetPACIndividuals";

            List<Individual> individuals = new List<Individual>();

            try {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "pacIndividual_Get";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = id;
                    spParams[1].Value = (name != null ? name.ToLower() : null);

                    try {
                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iIndividualId = dr.GetOrdinal("individual_id");
                            int iFullName = dr.GetOrdinal("full_name");
                            
                            while (dr.Read()) {
                                individuals.Add(new Individual(dr.GetInt16(iIndividualId), dr.GetString(iFullName)));
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

            return individuals;
        }

        public static int SaveIndividual(Individual individual)
        {
            const string METHOD_NAME = "SaveIndividual";
            int individualId = 0;

            if (individual.FullName != null && individual.FullName.Length > 1)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString()))
                    {
                        string procName = "pacIndividuals_Save";

                        if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                        System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                        spParams[1].Value = individual.IndividualID;
                        spParams[2].Value = individual.FullName;

                        using (SqlTransaction tran = conn.BeginTransaction())
                        {
                            try
                            {
                                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                                tran.Commit();

                                individualId = Convert.ToInt16(spParams[0].Value);
                            }
                            catch (SqlException sqlEx)
                            {
                                if (tran != null)
                                {
                                    tran.Rollback();
                                }

                                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning))
                                {
                                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                    throw (wscWarn);
                                }
                                else
                                {
                                    string errMsg = MOD_NAME;
                                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                    throw (wscEx);
                                }
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    string errMsg = MOD_NAME + METHOD_NAME;
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                    throw (wscEx);
                }
            }

            return individualId;
        }

        public static PACAgreement GetPACAgreement(string shid, int crop_year) {

            const string METHOD_NAME = "GetPACAgreement";
            PACAgreement pacAgreement = null;

            try {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "pacAgreement_Get";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = shid;
                    spParams[1].Value = crop_year;

                    try {
                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int iShid = dr.GetOrdinal("shid");
                            int iPacContribution = dr.GetOrdinal("pacContribution");
                            int iPacDate = dr.GetOrdinal("pacDate");
                            int iPacCropYear = dr.GetOrdinal("crop_year");
                            int iPacIndividuals = dr.GetOrdinal("pacIndividuals");

                            while (dr.Read()) {
                                if (!dr.IsDBNull(iShid))
                                {
                                    string pacIndividuals = "";
                                    string pacDate = "";
                                    double pacContribution = 0;
                                    int pacCropYear = 0;

                                    if (!dr.IsDBNull(iPacIndividuals))
                                        pacIndividuals = dr.GetString(iPacIndividuals);
                                    if (!dr.IsDBNull(iPacDate))
                                        pacDate = dr.GetDateTime(iPacDate).ToString("MM/dd/yyyy");
                                    if (!dr.IsDBNull(iPacContribution))
                                        pacContribution = Convert.ToDouble(dr.GetDecimal(iPacContribution));
                                    pacCropYear = dr.GetInt32(iPacCropYear);
                                    pacAgreement = new PACAgreement(shid, pacContribution, pacDate, pacCropYear, pacIndividuals);
                                }
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning))
                        {
                            WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                            throw (wscWarn);
                        }
                        else
                        {
                            string errMsg = MOD_NAME + METHOD_NAME;
                            WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                            throw (wscEx);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return pacAgreement;
        }

        public static int SavePACAgreement(PACAgreement pacAgreement) {

            const string METHOD_NAME = "SavePACAgreement";
            int pacId = 0;

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "pacAgreement_Save";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[1].Value = pacAgreement.SHID;
                    spParams[2].Value = pacAgreement.Contribution;
                    spParams[3].Value = pacAgreement.PACDate;
                    spParams[4].Value = pacAgreement.PACCropYear;
                    spParams[5].Value = pacAgreement.IndividualsString;

                    using (SqlTransaction tran = conn.BeginTransaction()) {
                        try {
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, procName, spParams);
                            tran.Commit();

                            pacId = Convert.ToInt16(spParams[0].Value);
                        } catch (SqlException sqlEx) {
                            if (tran != null) {
                                tran.Rollback();
                            }

                            if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                                WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                                throw (wscWarn);
                            } else {
                                string errMsg = MOD_NAME;
                                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                                throw (wscEx);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }

            return pacId;
        }

    }
}