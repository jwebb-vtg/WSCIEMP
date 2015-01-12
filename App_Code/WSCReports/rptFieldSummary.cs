using System;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using WSCData;
using PdfHelper;

namespace WSCReports {
    /// <summary>
    /// Summary description for rptFieldSummary.
    /// </summary>
    public class rptFieldSummary {

        static Font titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        static Font subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        static Font headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        static Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        static Font subNormalFont = FontFactory.GetFont("HELVETICA", 4F, Font.NORMAL);
        static Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        public static string ReportPackager(
            int cropYear, string factoryIDList, string stationIDList, string contractIDList,
            string shid, string fromShid, string toShid, int userID, string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "WSCReports.rptFieldSummary.ReportPackager: ";
            DirectoryInfo pdfDir = null;
            FileInfo[] pdfFiles = null;
            string filePath = "";

            try {

                pdfDir = new DirectoryInfo(pdfTempfolder);

                // Build the output file name by getting a list of all PDF files 
                // that begin with this session ID: use this as a name incrementer.				
                pdfFiles = pdfDir.GetFiles(fileName + "*.pdf");
                fileName += "_" + Convert.ToString(pdfFiles.Length + 1) + ".pdf";

                filePath = pdfDir.FullName + @"\" + fileName;

                try {

                    if (factoryIDList.Length > 0 || stationIDList.Length > 0 || contractIDList.Length > 0 ||
                        shid.Length > 0 || fromShid.Length > 0 || toShid.Length > 0) {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                            using (SqlDataReader dr = WSCField.FieldGetContractingData2(conn,
                                factoryIDList, stationIDList, contractIDList, shid, fromShid, toShid, cropYear)) {

                                using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                                    ReportBuilder(dr, cropYear, logoUrl, userID, fs);
                                }
                            }
                        }
                    } else {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please make criteria selections before requesting a report.");
                        throw (warn);
                    }
                }
                catch (System.Exception ex) {
                    string errMsg = "factoryIDList: " + factoryIDList + "; " +
                        "stationIDList: " + stationIDList + "; " +
                        "filePath: " + filePath + "; " +
                        "cropYear: " + cropYear.ToString();

                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                    throw (wscEx);
                }

                return filePath;
            }
            catch (System.Exception ex) {
                string errMsg = "factoryIDList: " + factoryIDList + "; " +
                    "stationIDList: " + stationIDList + "; " +
                    "fileName: " + fileName + "; " +
                    "pdfDir is null: " + (pdfDir == null).ToString() + "; " +
                    "pdfFiles is null: " + (pdfFiles == null).ToString() + "; " +
                    "filesPath: " + filePath;
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(SqlDataReader drContracting, int cropYear, string logoUrl,
            int userID, System.IO.FileStream fs) {

            const string METHOD_NAME = "WSCReports.rpfFieldSummary.ReportBuilder: ";
            string rptTitle = "";
            Document document = null;
            PdfWriter writer = null;
            iTextSharp.text.Image imgLogo = null;            
            RptAgronomy agronomy = null;
            FieldSummaryReportEvent pgEvent = null;

            try {

                // this is the second connection object within this scope.
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string oldShid = "";
                    string shid = "";
                    int contractID = 0;
                    agronomy = WSCField.GetAgronomyData();

                    while (drContracting.Read()) {

                        try {

                            shid = drContracting.GetString(drContracting.GetOrdinal("cntg_shid"));

                            if (oldShid == "") {
                                oldShid = shid;
                            }

                            if (document == null) {

                                agronomy = WSCField.GetAgronomyData();

                                // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                                //  ***  US LETTER: 612 X 792  ***
                                //document = new Document(iTextSharp.text.PageSize.LETTER);
                                //document = new Document(iTextSharp.text.PageSize.LETTER, 36, 36, 54, 72);
                                document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                                    PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                                // we create a writer that listens to the document
                                // and directs a PDF-stream to a file				
                                writer = PdfWriter.GetInstance(document, fs);

                                imgLogo = PdfReports.GetImage(logoUrl, 127, 50, iTextSharp.text.Element.ALIGN_CENTER);

                                // =================================
                                // Connect pgEvent
                                // =================================
                                rptTitle = "Field Summary Report " + cropYear.ToString();
                                pgEvent = new FieldSummaryReportEvent();
                                pgEvent.FillEvent(rptTitle, imgLogo, 0,
                                    drContracting.GetString(drContracting.GetOrdinal("cntg_shid")),
                                    drContracting.GetString(drContracting.GetOrdinal("cntg_business_name")),
                                    drContracting.GetString(drContracting.GetOrdinal("cntg_landowner_name")));

                                writer.PageEvent = pgEvent;

                                // Open the document
                                document.Open();
                            }

                            // Check for a change in SHID which triggers a dump of all contracts
                            if (oldShid != shid) {

                                // Also duplicat this chunk of logic outside of the loop, to catch the last Read().
                                // Ensure this report section starts on a new page.
                                using (SqlDataReader drRanking = WSCField.FieldPerformanceGet(conn, cropYear, 0, Convert.ToInt32(oldShid))) {
                                    AddAllContractDetails(drRanking, document, oldShid, cropYear, pgEvent);
                                }

                                oldShid = shid;
                                pgEvent.PageNumber = 0;
                                pgEvent.SHID = drContracting.GetString(drContracting.GetOrdinal("cntg_shid"));
                                pgEvent.BusinessName = drContracting.GetString(drContracting.GetOrdinal("cntg_business_name"));
                                pgEvent.LandownerName = drContracting.GetString(drContracting.GetOrdinal("cntg_landowner_name"));
                            }

                            // ======================================================
                            // Generate a new page for each contract
                            // ======================================================
                            if (contractID > 0) {
                                document.NewPage();
                            }

                            // Get Agronomy data
                            contractID = drContracting.GetInt32(drContracting.GetOrdinal("cntg_contract_id"));
                            using (SqlDataReader drAgronomy = WSCField.FieldGetAgronomyData(conn, "", "",
                                      contractID.ToString(), "", userID)) {

                                if (drAgronomy.Read()) {

                                    // ==========================================================
                                    // Print Contracting details for this contract
                                    // Must be within Agronomy loop to capture agriculturist
                                    // ==========================================================
                                    //		Shareholder Block
                                    string agriculturist = drAgronomy.GetString(drAgronomy.GetOrdinal("fld_agriculturist"));
                                    AddShareholderBlock(drContracting, document, agriculturist, pgEvent);

                                    AddLandDescriptionBlock(drContracting, document, pgEvent);
                                    AddContractingBlock(drContracting, document, pgEvent);
                                    AddAgronomyBlock(drAgronomy, document, cropYear, pgEvent);
                                }

                            }

                            // Get Field Performance data
                            using (SqlDataReader drRanking = WSCField.FieldPerformanceGet(conn, cropYear, contractID, 0)) {
                                AddFieldSummaryBlock(drRanking, document, pgEvent);
                            }
                            using (SqlDataReader drRanking = WSCField.FieldPerformance2Get(conn, contractID)) {
                                AddFieldSummary2Block(drRanking, document, pgEvent);
                            }
                        }
                        catch (System.Exception ex) {
                            WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME, ex);
                            throw (wscEx);
                        }
                    }

                    if (shid.Length > 0) {
                        using (SqlDataReader drRanking = WSCField.FieldPerformanceGet(conn, cropYear, 0, Convert.ToInt32(oldShid))) {
                            AddAllContractDetails(drRanking, document, shid, cropYear, pgEvent);
                        }
                    }
                }

                // ======================================================
                // Close document
                // ======================================================
                if (document != null) {
                    pgEvent.IsDocumentClosing = true;
                    document.Close();
                    document = null;
                }
                if (writer == null) {
                    // Warn that we have no data.
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("No records matched your report criteria.");
                    throw (warn);
                }
            }
            catch (Exception ex) {
                string errMsg = "document is null: " + (document == null).ToString() + "; " +
                    "writer is null: " + (writer == null).ToString();
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                throw (wscex);
            }
            finally {

                if (document != null) {
                    pgEvent.IsDocumentClosing = true;
                    document.Close();
                }
                if (writer != null) {
                    writer.Close();
                }
            }
        }

        private static void AddShareholderBlock(SqlDataReader dr, iTextSharp.text.Document document, string agriculturist, FieldSummaryReportEvent pgEvent) {

            float[] layout = new float[] { 67.5F, 270F, 67.5F, 135F };

            try {

                PdfPTable table = PdfReports.CreateTable(layout, 0);

                PdfReports.AddText2Table(table, "Contract #:", labelFont, "left");
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_contract_no")), normalFont, "left");

                PdfReports.AddText2Table(table, "Agriculturist:", labelFont, "left");
                PdfReports.AddText2Table(table, agriculturist, normalFont, "left");
                PdfReports.AddText2Table(table, " ", subNormalFont, "left", 4);

                PdfReports.AddTableNoSplit(document, pgEvent, table);
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.AddShareholderBlock", ex);
                throw (wscex);
            }
        }

        private static void AddLandDescriptionBlock(SqlDataReader dr, iTextSharp.text.Document document, FieldSummaryReportEvent pgEvent) {

            try {

                // ***  ROW 1  ***
                float[] tmpLayout = new float[] { 13F, 13.5F, 11F, 13.5F, 11F, 13.5F, 11F, 13.5F };
                PdfPTable table = PdfReports.CreateTable(tmpLayout, 0);

                PdfReports.AddText2Table(table, "Field Name: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_field_name")), normalFont);

                PdfReports.AddText2Table(table, "Acres: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("cntg_acres")).ToString(), normalFont);

                PdfReports.AddText2Table(table, "State: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_lld_state")), normalFont);

                PdfReports.AddText2Table(table, "County: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_lld_county")), normalFont);

                // ***  ROW 2  ***
                PdfReports.AddText2Table(table, "1/4 Quadrant: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_lld_quarter_quadrant")), normalFont);

                PdfReports.AddText2Table(table, "Quadrant: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_lld_quadrant")), normalFont);

                PdfReports.AddText2Table(table, "Section: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_lld_section")), normalFont);

                PdfReports.AddText2Table(table, "Township: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_lld_township")), normalFont);


                // ***  ROW 3  ***
                PdfReports.AddText2Table(table, "Range: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_lld_range")), normalFont);

                PdfReports.AddText2Table(table, "Latitude: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("cntg_latitude")).ToString(), normalFont);

                PdfReports.AddText2Table(table, "Longitude: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("cntg_longitude")).ToString(), normalFont);

                PdfReports.AddText2Table(table, "FSA Official: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_fsa_official")), normalFont);

                // ***  ROW 4  ***
                PdfReports.AddText2Table(table, "FSA Number: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_fsa_number")), normalFont, 3);

                PdfReports.AddText2Table(table, "FSA State: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_fsa_state")), normalFont);

                PdfReports.AddText2Table(table, "FSA County: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_fsa_county")), normalFont);

                // ***  ROW 5  ***
                PdfReports.AddText2Table(table, "Farm No: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_farm_number")), normalFont);

                PdfReports.AddText2Table(table, "Tract No: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_tract_number")), normalFont);

                PdfReports.AddText2Table(table, "Field No: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_field_number")), normalFont);

                PdfReports.AddText2Table(table, "1/4 Field: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_quarter_field")), normalFont, 7);


                // ***  ROW 6  ***                
                PdfReports.AddText2Table(table, "Description", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_description")), normalFont, 7);
                PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);

                PdfReports.AddTableNoSplit(document, pgEvent, table);			

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.AddLandDescriptionBlock", ex);
                throw (wscex);
            }
        }

        private static void AddContractingBlock(SqlDataReader dr, iTextSharp.text.Document document, FieldSummaryReportEvent pgEvent) {

            try {

                string tmp = "";
                string tmp2 = "";
                float[] layout = new float[] { 110F, 80F, 110F, 65F, 110F, 65F };
                PdfPTable table = PdfReports.CreateTable(layout, 0);

                table.DefaultCell.PaddingTop = 3.0F;
                table.DefaultCell.PaddingBottom = 0;
                table.DefaultCell.PaddingLeft = 0;
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_BOTTOM;

                // Row 1
                PdfReports.AddText2Table(table, "Land Ownership: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_ownership")).ToString();
                PdfReports.AddText2Table(table, (tmp.Length == 0 ? " " : tmp), normalFont);

                PdfReports.AddText2Table(table, "Rotation (yrs): ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_rotation_len")).ToString(), normalFont);

                PdfReports.AddText2Table(table, @"Prior Crop: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_prev_crop_type")).ToString(), normalFont);

                // ROW 2
                PdfReports.AddText2Table(table, "Years having beets: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_beet_years")).ToString(), normalFont);

                PdfReports.AddText2Table(table, "Irrigation System: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_irrigation_method")).ToString(), normalFont);

                PdfReports.AddText2Table(table, "Water Source: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_irrigation_source")).ToString(), normalFont);

                // ROW 3
                PdfReports.AddText2Table(table, @"Water: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_post_water")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                PdfReports.AddText2Table(table, tmp, normalFont, "left");

                PdfReports.AddText2Table(table, "Tillage: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_tillage")).ToString();
                PdfReports.AddText2Table(table, (tmp.Length == 0 ? " " : tmp), normalFont, 3);

                // Row 4
                PdfReports.AddText2Table(table, @"Aphanomyces (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_aphanomyces")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_aphanomyces")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont);

                PdfReports.AddText2Table(table, @"Nematode (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_nematode")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_nematode")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont);

                PdfReports.AddText2Table(table, @"Cercospora (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_cercospora")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_cercospora")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont);

                // ROW 5
                PdfReports.AddText2Table(table, @"Pwd Mildew (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_powdery_mildew")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_powderymildew")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont, "left");

                PdfReports.AddText2Table(table, @"Curly Top (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_curly_top")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_curlytop")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont);

                PdfReports.AddText2Table(table, @"Rhizoctonia (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_rhizoctonia")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_rhizoctonia")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont);

                // Row 6
                PdfReports.AddText2Table(table, @"Fusarium (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_fusarium")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_fusarium")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont);

                PdfReports.AddText2Table(table, @"Rhizomania (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_rhizomania")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_rhizomania")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont);

                PdfReports.AddText2Table(table, @"Root Aphid (pre\post): ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("cntg_suspect_root_aphid")).ToString();
                tmp = (tmp == "Y" ? "Yes" : "No");
                tmp2 = dr.GetString(dr.GetOrdinal("cntg_post_rootaphid")).ToString();
                tmp2 = (tmp2.Length == 0 ? "None" : tmp2);
                PdfReports.AddText2Table(table, tmp + @" \ " + tmp2, normalFont);

                // Blank
                PdfReports.AddText2Table(table, " ", subNormalFont, "left", layout.Length);
                PdfReports.AddTableNoSplit(document, pgEvent, table);

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.AddContractingBlock", ex);
                throw (wscex);
            }
        }

        private static void AddAgronomyBlock(SqlDataReader dr, iTextSharp.text.Document document, int cropYear, FieldSummaryReportEvent pgEvent) {

            try {

                string tmp = "";

                float[] layout = new float[] { 85F, 90F, 80F, 95F, 75F, 115F };
                PdfPTable table = PdfReports.CreateTable(layout, 0);

                //table.DefaultCell.PaddingTop = 3.0F;
                table.DefaultCell.PaddingBottom = 0;
                table.DefaultCell.PaddingLeft = 0;
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_BOTTOM;

                // ===================
                // Planting
                // ===================
                // Row 1
                PdfReports.AddText2Table(table, "Variety: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_seed_variety")), normalFont);

                PdfReports.AddText2Table(table, "Seed: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_seed_primed")), normalFont);

                PdfReports.AddText2Table(table, "Treatment: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("fld_seed_treatment_chemical")).Replace(" ", ", ");
                PdfReports.AddText2Table(table, tmp, normalFont);

                // Row 2
                PdfReports.AddText2Table(table, "Row Spacing: ", labelFont);
                PdfReports.AddText2Table(table, (dr.GetInt16(dr.GetOrdinal("fld_row_spacing"))).ToString(), normalFont);

                PdfReports.AddText2Table(table, "Plant Spacing: ", labelFont);
                PdfReports.AddText2Table(table, (dr.GetDecimal(dr.GetOrdinal("fld_plant_spacing"))).ToString("#.#"), normalFont);

                PdfReports.AddText2Table(table, "Planting Date: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_planting_date")), normalFont);

                // Row 3
                PdfReports.AddText2Table(table, "80% Emerg Date: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_planting_date")), normalFont);

                PdfReports.AddText2Table(table, "Replant Date: ", labelFont);
                PdfReports.AddText2Table(table, (dr.GetString(dr.GetOrdinal("fld_replant_date"))), normalFont);

                PdfReports.AddText2Table(table, "Replant Variety: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_replant_seed_variety")), normalFont);

                // Row 4
                PdfReports.AddText2Table(table, "Replant Acres: ", labelFont);
                PdfReports.AddText2Table(table, (dr.GetInt32(dr.GetOrdinal("fld_acres_replanted"))).ToString(), normalFont);

                PdfReports.AddText2Table(table, "Replant Reason: ", labelFont);
                PdfReports.AddText2Table(table, (dr.GetString(dr.GetOrdinal("fld_replant_reason"))), normalFont, "left");

                PdfReports.AddText2Table(table, "Reason Lost: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_lost_reason")), normalFont);

                // Row 5
                PdfReports.AddText2Table(table, "Lost Acres: ", labelFont);
                PdfReports.AddText2Table(table, (dr.GetInt32(dr.GetOrdinal("fld_acres_lost"))).ToString(), normalFont, 5);

                PdfReports.AddText2Table(table, " ", subNormalFont, "left", layout.Length);
                PdfReports.AddTableNoSplit(document, pgEvent, table);

                // ===================
                // Soil Sample
                // ===================
                layout = new float[] { 75F, 85F, 70F, 60F, 75F, 60F, 65F, 50F };
                table = PdfReports.CreateTable(layout, 0);

                // Row 1
                PdfReports.AddText2Table(table, "Soil Texture: ", labelFont);
                PdfReports.AddText2Table(table, (dr.GetString(dr.GetOrdinal("fld_soil_texture"))), normalFont);

                PdfReports.AddText2Table(table, "Soil Sample: ", labelFont);
                PdfReports.AddText2Table(table, (dr.GetString(dr.GetOrdinal("fld_test_season"))), normalFont);

                PdfReports.AddText2Table(table, "N Sample Depth: ", labelFont);
                tmp = (dr.GetInt16(dr.GetOrdinal("fld_test_depth"))).ToString();
                if (tmp == "0") { tmp = " "; }
                PdfReports.AddText2Table(table, tmp, normalFont);

                PdfReports.AddText2Table(table, @"Grid\Zone: ", labelFont);
                tmp = (dr.GetString(dr.GetOrdinal("fld_grid_zone")));
                tmp = (tmp == "Y" ? "Yes" : "No");
                PdfReports.AddText2Table(table, tmp, normalFont);

                // Row 2
                PdfReports.AddText2Table(table, "Results: ", labelFont, "left");

                Paragraph p = new Paragraph();
                string spacer = "        ";

                p = new Paragraph();
                p.Add(new Phrase("N: ", labelFont));
                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_N"))).ToString("#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " " + spacer;
                } else {
                    tmp += " lbs" + spacer;
                }
                p.Add(new Phrase(tmp, normalFont));

                p.Add(new Phrase("P: ", labelFont));
                tmp = (dr.GetString(dr.GetOrdinal("fld_test_P")));
                p.Add(new Phrase(tmp + spacer, normalFont));

                p.Add(new Phrase("K: ", labelFont));
                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_K"))).ToString("#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " " + spacer;
                } else {
                    tmp += " ppm" + spacer;
                }
                p.Add(new Phrase(tmp, normalFont));

                p.Add(new Phrase("Salts: ", labelFont));
                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_salts"))).ToString("#.00");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " " + spacer;
                } else {
                    tmp += spacer;
                }
                p.Add(new Phrase(tmp, normalFont));

                p.Add(new Phrase("pH: ", labelFont));
                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_pH"))).ToString("#.0");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                p.Add(new Phrase(tmp + spacer, normalFont));

                p.Add(new Phrase("O.M.: ", labelFont));
                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_org_mat"))).ToString("#.00");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                p.Add(new Phrase(tmp + " %", normalFont));
                PdfReports.AddText2Table(table, p, 7);

                PdfReports.AddText2Table(table, " ", subNormalFont, "left", layout.Length);
                PdfReports.AddTableNoSplit(document, pgEvent, table);

                // ===================
                // Fertilizer
                // ===================
                layout = new float[] { 100F, 90F, 90F, 90F, 90F, 80F };
                table = PdfReports.CreateTable(layout, 0);

                PdfReports.AddText2Table(table, "Last Yr Manure Applied: ", labelFont);
                tmp = (dr.GetInt32(dr.GetOrdinal("fld_last_yr_manure"))).ToString();
                if (tmp.Length > 0 && tmp == "0") { tmp = ""; }
                PdfReports.AddText2Table(table, tmp, normalFont, 5);

                // Fall
                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_N"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                PdfReports.AddText2Table(table, "Fall " + (cropYear - 1).ToString() + " Applied Fertilizer", labelFont, "left", 2);
                p = new Paragraph();
                p.Add(new Phrase("N: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_P"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                p = new Paragraph();
                p.Add(new Phrase("P: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_K"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                p = new Paragraph();
                p.Add(new Phrase("K: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);
                PdfReports.AddText2Table(table, " ", normalFont);

                // Spring
                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_N"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                PdfReports.AddText2Table(table, "Spring " + cropYear.ToString() + " Applied Fertilizer", labelFont, "left", 2);
                p = new Paragraph();
                p.Add(new Phrase("N: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_P"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                p = new Paragraph();
                p.Add(new Phrase("P: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_K"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                p = new Paragraph();
                p.Add(new Phrase("K: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);
                PdfReports.AddText2Table(table, " ", normalFont);

                // In Season
                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_N"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                PdfReports.AddText2Table(table, "In Season " + cropYear.ToString() + " Applied Fertilizer", labelFont, "left", 2);
                p = new Paragraph();
                p.Add(new Phrase("N: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_P"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                p = new Paragraph();
                p.Add(new Phrase("P: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_K"))).ToString("#,###.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) {
                    tmp = " ";
                } else {
                    tmp += " lbs";
                }
                p = new Paragraph();
                p.Add(new Phrase("K: ", labelFont));
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);
                PdfReports.AddText2Table(table, " ", normalFont);

                PdfReports.AddText2Table(table, "Starter Fertilizer: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("fld_fert_starter"));
                if (tmp.Length == 0) { tmp = " "; }
                PdfReports.AddText2Table(table, tmp, normalFont, "left", layout.Length - 1);

                PdfReports.AddText2Table(table, " ", subNormalFont, "left", layout.Length);
                PdfReports.AddTableNoSplit(document, pgEvent, table);

                // ========================
                // Pre\Post Insecticide
                // ========================
                layout = new float[] { 100F, 90F, 120F, 115F, 115F };
                table = PdfReports.CreateTable(layout, 0);

                p = new Paragraph();
                p.Add(new Phrase("Pre-Emergence Insecticide: ", labelFont));
                tmp = dr.GetString(dr.GetOrdinal("fld_pre_insecticide"));
                if (tmp.Length == 0) { tmp = " "; }
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p, 2);

                p = new Paragraph();
                p.Add(new Phrase("Post-Emergence Insecticide: ", labelFont));
                tmp = dr.GetString(dr.GetOrdinal("fld_post_insectcide"));
                if (tmp.Length == 0) { tmp = " "; }
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p, 3);

                PdfReports.AddText2Table(table, "Insecticide-Root Maggot: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("fld_root_maggot_insecticide"));
                if (tmp == "") { tmp = " "; }
                PdfReports.AddText2Table(table, tmp, normalFont);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_rootm_counter_lbs"))).ToString("#.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = " "; }
                tmp = "Counter 15G: " + tmp + " lbs/A";
                PdfReports.AddText2Table(table, tmp, normalFont);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_rootm_temik_lbs"))).ToString("#.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = " "; }
                tmp = "Temik: " + tmp + " lbs/A";
                PdfReports.AddText2Table(table, tmp, normalFont);

                tmp = (dr.GetDecimal(dr.GetOrdinal("fld_rootm_thimet_lbs"))).ToString("#.#");
                if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = " "; }
                tmp = "Thimet: " + tmp + " lbs/A";
                PdfReports.AddText2Table(table, tmp, normalFont);

                PdfReports.AddText2Table(table, " ", subNormalFont, "left", layout.Length);
                PdfReports.AddTableNoSplit(document, pgEvent, table);

                // ===================
                // Weed Control
                // ===================
                layout = new float[] { 180F, 360F };
                table = PdfReports.CreateTable(layout, 0);

                p = new Paragraph();
                p.Add(new Phrase("Pre-Emergence Weed Control: ", labelFont));
                tmp = dr.GetString(dr.GetOrdinal("fld_pre_weed_ctrl"));
                if (tmp.Length == 0) { tmp = " "; }
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                p = new Paragraph();
                p.Add(new Phrase("Number of Herbicide Treatments: ", labelFont));
                tmp = (dr.GetInt32(dr.GetOrdinal("fld_herbicide_rx_count"))).ToString("#");
                if (tmp.Length == 0) { tmp = " "; }
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                PdfReports.AddTableNoSplit(document, pgEvent, table);

                // ===================
                // Herbicide
                // ===================
                layout = new float[] { 180F, 360F };
                table = PdfReports.CreateTable(layout, 0);

                p = new Paragraph();
                p.Add(new Phrase("Layby Herbicide: ", labelFont));
                tmp = dr.GetString(dr.GetOrdinal("fld_layby_herbicide"));
                if (tmp.Length == 0) { tmp = " "; }
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                p = new Paragraph();
                p.Add(new Phrase("Chemical: ", labelFont));
                tmp = dr.GetString(dr.GetOrdinal("fld_layby_herbicide_chemical"));
                if (tmp.Length == 0) { tmp = " "; }
                p.Add(new Phrase(tmp, normalFont));
                PdfReports.AddText2Table(table, p);

                PdfReports.AddText2Table(table, " ", subNormalFont, "left", layout.Length);
                PdfReports.AddTableNoSplit(document, pgEvent, table);

                // ===================
                // Cercospora Program
                // ===================
                layout = new float[] { 100F, 30F, 120F, 100F, 50F, 140F };
                table = PdfReports.CreateTable(layout, 0);

                // Application 1
                PdfReports.AddText2Table(table, "Cercospora Program: ", labelFont, "left", 2);
                PdfReports.AddText2Table(table, "Application 1 Chemical: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_cercsp_app1_chemical")), normalFont);
                PdfReports.AddText2Table(table, "Date: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_cercospora_app1_date")), normalFont);

                // Application #2
                PdfReports.AddText2Table(table, " ", labelFont, "left", 2);
                PdfReports.AddText2Table(table, "Application 2 Chemical: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_cercsp_app2_chemical")), normalFont);
                PdfReports.AddText2Table(table, "Date: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_cercospora_app2_date")), normalFont);

                // Application #3
                PdfReports.AddText2Table(table, " ", labelFont, "left", 2);
                PdfReports.AddText2Table(table, "Application 3 Chemical: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_cercsp_app3_chemical")), normalFont);
                PdfReports.AddText2Table(table, "Date: ", labelFont);
                PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("fld_cercospora_app3_date")), normalFont);

                PdfReports.AddText2Table(table, " ", subNormalFont, "left", layout.Length);
                PdfReports.AddTableNoSplit(document, pgEvent, table);

                // ====================================
                // Treatments, Hail, and Weeds
                // ====================================
                layout = new float[] { 120F, 60F, 120F, 60F, 120F, 60F };
                table = PdfReports.CreateTable(layout, 0);

                PdfReports.AddText2Table(table, "Treated Powdery Mildew: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("fld_treated_powdery_mildew"));
                if (tmp.Length == 0) { tmp = " "; }
                PdfReports.AddText2Table(table, tmp, normalFont);

                PdfReports.AddText2Table(table, "Treated Nematode: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("fld_treated_nematode"));
                if (tmp.Length == 0) { tmp = " "; }
                PdfReports.AddText2Table(table, tmp, normalFont);

                PdfReports.AddText2Table(table, "Treated Rhizoctonia: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("fld_treated_rhizoctonia"));
                if (tmp.Length == 0) { tmp = " "; }
                PdfReports.AddText2Table(table, tmp, normalFont);

                PdfReports.AddText2Table(table, "Hail Stress: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("fld_hail_stress"));
                if (tmp.Length == 0) { tmp = " "; }
                PdfReports.AddText2Table(table, tmp, normalFont);

                PdfReports.AddText2Table(table, "Weed Control: ", labelFont);
                tmp = dr.GetString(dr.GetOrdinal("fld_weed_control"));
                if (tmp.Length == 0) { tmp = " "; }
                PdfReports.AddText2Table(table, tmp, normalFont, "left", 3);

                PdfReports.AddText2Table(table, " ", normalFont, "left", layout.Length);
                PdfReports.AddTableNoSplit(document, pgEvent, table);

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.AddAgronomyBlock", ex);
                throw (wscex);
            }

        }

        private static void AddFieldSummaryBlock(SqlDataReader dr, iTextSharp.text.Document document, FieldSummaryReportEvent pgEvent) {

            try {

                string tmp = "";

                float[] layout = new float[] { 240F, 60F, 60F, 60F, 60F, 60F };
                PdfPTable table = PdfReports.CreateTable(layout, 0);

                if (dr.Read()) {

                    // Header
                    PdfReports.AddText2Table(table, " ", labelFont);
                    PdfReports.AddText2Table(table, "TPA", labelFont);
                    PdfReports.AddText2Table(table, "% Sugar", labelFont);
                    PdfReports.AddText2Table(table, "% SLM", labelFont);
                    PdfReports.AddText2Table(table, "% Tare", labelFont);
                    PdfReports.AddText2Table(table, "$/Acre", labelFont);

                    // Contract
                    PdfReports.AddText2Table(table, "Contract: ", labelFont);
                    tmp = dr.GetDecimal(dr.GetOrdinal("TonsPerAcre")).ToString("#,###.0000");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetDecimal(dr.GetOrdinal("SugarPct")).ToString("###.00");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetDecimal(dr.GetOrdinal("SLMPct")).ToString("###.00");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetDecimal(dr.GetOrdinal("TarePct")).ToString("###.00");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetDecimal(dr.GetOrdinal("GrossDollarsPerAcre")).ToString("#,###.00");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    // Area
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("AreaName")) +
                        " (out of " + dr.GetInt32(dr.GetOrdinal("AreaCount")).ToString("#,###") + "): ", labelFont);
                    tmp = dr.GetInt32(dr.GetOrdinal("AreaTonsPerAcreRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("AreaSugarPctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("AreaSLMPctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("AreaTarePctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("AreaGrossDollarsPerAcreRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    // Region
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("RegionName")) +
                        " (out of " + dr.GetInt32(dr.GetOrdinal("RegionCount")).ToString("#,###") + "): ", labelFont);
                    tmp = dr.GetInt32(dr.GetOrdinal("RegionTonsPerAcreRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("RegionSugarPctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("RegionSLMPctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("RegionTarePctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("RegionGrossDollarsPerAcreRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    // Coop
                    PdfReports.AddText2Table(table, "Coop" +
                        " (out of " + dr.GetInt32(dr.GetOrdinal("CoopCount")).ToString("#,###") + "): ", labelFont);
                    tmp = dr.GetInt32(dr.GetOrdinal("CoopTonsPerAcreRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("CoopSugarPctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("CoopSLMPctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("CoopTarePctRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    tmp = dr.GetInt32(dr.GetOrdinal("CoopGrossDollarsPerAcreRank")).ToString("#,###");
                    PdfReports.AddText2Table(table, tmp, normalFont);

                    PdfReports.AddText2Table(table, " ", subNormalFont, "left", layout.Length);
                }

                PdfReports.AddTableNoSplit(document, pgEvent, table);

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.AddFieldSummaryBlock", ex);
                throw (wscex);
            }
        }

        private static void AddFieldSummary2Block(SqlDataReader dr, iTextSharp.text.Document document, FieldSummaryReportEvent pgEvent) {

            try {

                string tmp = "";
                float[] layout = new float[] { 120F, 420F };
                PdfPTable table = PdfReports.CreateTable(layout, 0);

                while (dr.Read()) {

                    PdfReports.AddText2Table(table,
                        dr.GetString(dr.GetOrdinal("fldp2_station_name")) + ":", labelFont);

                    // Rank
                    tmp = " (" +
                        dr.GetInt32(dr.GetOrdinal("fldp2_dirt_pct_rank")).ToString("#,###") +
                        " out of " + dr.GetInt32(dr.GetOrdinal("fldp2_station_contracts")).ToString("#,###") + ")" +
                        " Tare Dirt " +
                        dr.GetDecimal(dr.GetOrdinal("fldp2_dirt_pct")).ToString("#,###.0000") +
                        "%";
                    PdfReports.AddText2Table(table, tmp, normalFont);

                }

                PdfReports.AddTableNoSplit(document, pgEvent, table);

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.AddFieldSummary2Block", ex);
                throw (wscex);
            }
        }

        private static void AddAllContractDetails(SqlDataReader dr, iTextSharp.text.Document document,
            string shid, int cropYear, FieldSummaryReportEvent pgEvent) {

            try {

                string tmp = "";
                int harvestAcres = 0;
                float[] layout = new float[] { 50F, 68F, 35F, 61F, 47F, 37.0F, 32.0F, 41F, 35F, 50F, 38F, 46F };
                PdfPTable table = null;
                decimal grossDollars = 0;

                while (dr.Read()) {

                    if (table == null) {

                        document.NewPage();

                        table = PdfReports.CreateTable(layout, 0);
                        PdfReports.AddText2Table(table, " ", normalFont, "left", layout.Length);

                        // Header
                        //	Row 1
                        PdfReports.AddText2Table(table, " ", labelFont, "left");
                        PdfReports.AddText2Table(table, " ", labelFont, "left");
                        PdfReports.AddText2Table(table, "Acres", labelFont, "center");
                        PdfReports.AddText2Table(table, " ", labelFont, "right");
                        PdfReports.AddText2Table(table, " ", labelFont, "right");

                        PdfReports.AddText2Table(table, " ", labelFont, "right");
                        PdfReports.AddText2Table(table, " ", labelFont, "right");

                        PdfReports.AddText2Table(table, " ", labelFont, "center");
                        PdfReports.AddText2Table(table, " ", labelFont, "center");
                        PdfReports.AddText2Table(table, @"Ext. Sugar", labelFont, "right");
                        PdfReports.AddText2Table(table, " ", labelFont, "center");
                        PdfReports.AddText2Table(table, " ", labelFont, "center");

                        //	Row 2
                        PdfReports.AddText2Table(table, "Contract #", labelFont, "left");
                        PdfReports.AddText2Table(table, "Field Name", labelFont, "left");
                        PdfReports.AddText2Table(table, "Harvest", labelFont, "center");
                        PdfReports.AddText2Table(table, "Net Tons", labelFont, "right");
                        PdfReports.AddText2Table(table, "TPA", labelFont, "right");

                        PdfReports.AddText2Table(table, "BPA", labelFont, "right");
                        PdfReports.AddText2Table(table, "Top", labelFont, "right");

                        PdfReports.AddText2Table(table, "% Sugar", labelFont, "right");
                        PdfReports.AddText2Table(table, "SLM", labelFont, "right");
                        PdfReports.AddText2Table(table, "Lbs/Acre", labelFont, "right");
                        PdfReports.AddText2Table(table, "% Tare", labelFont, "right");
                        PdfReports.AddText2Table(table, @"$/Acre", labelFont, "right");

                    }

                    tmp = dr.GetInt32(dr.GetOrdinal("ContractNo")).ToString();
                    PdfReports.AddText2Table(table, tmp, normalFont, "left");

                    tmp = dr.GetString(dr.GetOrdinal("FieldName"));
                    PdfReports.AddText2Table(table, tmp, normalFont, "left");

                    harvestAcres = dr.GetInt32(dr.GetOrdinal("AcresHarvested"));
                    PdfReports.AddText2Table(table, harvestAcres.ToString(), normalFont, "center");

                    tmp = dr.GetDecimal(dr.GetOrdinal("TonsHarvested")).ToString("#,###.0000");
                    PdfReports.AddText2Table(table, tmp, normalFont, "right");

                    tmp = dr.GetDecimal(dr.GetOrdinal("TonsPerAcre")).ToString("#,###.0000");
                    PdfReports.AddText2Table(table, tmp, normalFont, "right");

                    tmp = dr.GetInt32(dr.GetOrdinal("BeetsPerAcre")).ToString("#,##0");
                    PdfReports.AddText2Table(table, tmp, normalFont, "right");

                    tmp = dr.GetDecimal(dr.GetOrdinal("AvgTopping")).ToString("0.00");
                    PdfReports.AddText2Table(table, tmp, normalFont, "right");

                    tmp = dr.GetDecimal(dr.GetOrdinal("SugarPct")).ToString("#,###.00");
                    PdfReports.AddText2Table(table, tmp, normalFont, "right");

                    tmp = dr.GetDecimal(dr.GetOrdinal("SLMPct")).ToString("#,###.00");
                    PdfReports.AddText2Table(table, tmp, normalFont, "right");

                    // convert to lbs\acre extractable
                    int extSugar = 0;
                    if (harvestAcres > 0) {
                        extSugar = Convert.ToInt32(Math.Round(dr.GetDecimal(dr.GetOrdinal("ExtractableSugar")) / harvestAcres, 0));
                    }
                    PdfReports.AddText2Table(table, extSugar.ToString("#,###"), normalFont, "right");

                    tmp = dr.GetDecimal(dr.GetOrdinal("TarePct")).ToString("#,###.00");
                    PdfReports.AddText2Table(table, tmp, normalFont, "right");

                    tmp = dr.GetDecimal(dr.GetOrdinal("GrossDollarsPerAcre")).ToString("#,###.00");
                    PdfReports.AddText2Table(table, tmp, normalFont, "right");

                    grossDollars += dr.GetDecimal(dr.GetOrdinal("GrossDollars"));

                }

                if (table != null) {

                    PdfReports.AddTableNoSplit(document, pgEvent, table);
                    AddHarvestTotals(document, shid, cropYear, grossDollars, layout, pgEvent);
                }

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.AddAllContractDetails", ex);
                throw (wscex);
            }

        }

        private static void AddHarvestTotals(iTextSharp.text.Document document, string shid,
            int cropYear, decimal grossDollars, float[] layout, FieldSummaryReportEvent pgEvent) {

            try {

                string tmp = "";
                PdfPTable table = PdfReports.CreateTable(layout, 0);

                int harvestAcres = 0;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.FieldGetHarvestTotals(conn, 0, Convert.ToInt32(shid), cropYear)) {

                        if (dr.Read()) {

                            PdfReports.AddText2Table(table, " ", normalFont, "left");   // Skip Contract #
                            PdfReports.AddText2Table(table, " ", normalFont, "left");   // Skip Field Name

                            if (dr.IsDBNull(dr.GetOrdinal("HarvestAcres"))) {

                                // Not contracts qualified via Reviewed/Include to produce any acres.
                                PdfReports.AddText2Table(table, "Please consult your Agriculturist, no contracts qualify for totals.", labelFont, "center", 10);

                            } else {

                                harvestAcres = dr.GetInt32(dr.GetOrdinal("HarvestAcres"));
                                PdfReports.AddText2Table(table, harvestAcres.ToString(), labelFont, "center");

                                tmp = dr.GetDecimal(dr.GetOrdinal("HarvestFinalNetTons")).ToString("#,###.0000");
                                PdfReports.AddText2Table(table, tmp, labelFont, "right");

                                tmp = dr.GetDecimal(dr.GetOrdinal("TonsPerAcre")).ToString("#,###.0000");
                                PdfReports.AddText2Table(table, tmp, labelFont, "right");

                                tmp = dr.GetInt32(dr.GetOrdinal("HarvestBeetsPerAcre")).ToString("#,##0");
                                PdfReports.AddText2Table(table, tmp, labelFont, "right");

                                tmp = dr.GetDecimal(dr.GetOrdinal("HarvestAvgTopping")).ToString("0.00");
                                PdfReports.AddText2Table(table, tmp, labelFont, "right");

                                tmp = dr.GetDecimal(dr.GetOrdinal("HarvestSugarPct")).ToString("#,###.00");
                                PdfReports.AddText2Table(table, tmp, labelFont, "right");

                                tmp = dr.GetDecimal(dr.GetOrdinal("HarvestSLMPct")).ToString("#,###.00");
                                PdfReports.AddText2Table(table, tmp, labelFont, "right");

                                int extSugar = 0;
                                if (harvestAcres > 0) {
                                    extSugar = Convert.ToInt32(Math.Round(dr.GetDecimal(dr.GetOrdinal("ExtractableSugarLbs")) / harvestAcres, 0));
                                }
                                PdfReports.AddText2Table(table, extSugar.ToString("#,###"), labelFont, "right");

                                tmp = dr.GetDecimal(dr.GetOrdinal("HarvestTarePct")).ToString("#,###.00");
                                PdfReports.AddText2Table(table, tmp, labelFont, "right");

                                if (harvestAcres > 0) {
                                    tmp = (grossDollars / harvestAcres).ToString("#,###.00");
                                } else {
                                    tmp = "0.00";
                                }

                                PdfReports.AddText2Table(table, tmp, labelFont, "right");
                            }
                        }
                    }
                }

                PdfReports.AddTableNoSplit(document, pgEvent, table);

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.AddHarvestTotals", ex);
                throw (wscex);
            }

        }

        private static iTextSharp.text.Paragraph GetSeedsAsText(RptAgronomy agronomy, string factoryNumber,
            Font labelFont, Font normalFont, bool underline) {

            try {

                string varietyType = "";
                string label = "";

                const int MaxItemsPerLine = 12;

                iTextSharp.text.Paragraph para = new Paragraph();
                iTextSharp.text.Phrase p = null;
                int lineCount = 0;

                if (factoryNumber.Length > 0) {

                    // Given the factory number look up whether this is a north or south variety.
                    foreach (TItem2 item in agronomy.FactoryVarietyList.Items) {
                        if (item.Field1 == factoryNumber) {
                            varietyType = item.Field2;
                            break;
                        }
                    }

                    if (varietyType.Length > 0) {
                        label = varietyType.Substring(0, 1).ToUpper() + varietyType.Substring(1).ToLower();
                    }

                    if (label.Length > 0) {
                        p = new Phrase(label, labelFont);
                        para.Add(p);
                        p = null;
                        lineCount++;
                    }

                    foreach (TItem2 item in agronomy.VarietyList.Items) {
                        if (item.Field1 == varietyType) {

                            if (p != null) {
                                p = new Phrase(@" \ " + item.Field2, normalFont);
                            } else {
                                p = new Phrase(item.Field2, normalFont);
                            }
                            para.Add(p);
                            lineCount++;

                            if (lineCount == MaxItemsPerLine) {
                                p = new Phrase("\n", normalFont);
                                para.Add(p);
                                lineCount = 0;
                            }
                        }
                    }

                    p = new Phrase("\n ", normalFont);
                    para.Add(p);
                }

                return para;

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("rptFieldSummary.GetSeedsAsText", ex);
                throw (wscex);
            }
        }
    }

    public class FieldSummaryReportEvent : PdfPageEventHelper, ICustomPageEvent {

        Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        Font _subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        // This is the contentbyte object of the writer
        PdfContentByte _cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;
        ColumnText _ct = null;

        private bool _isDocumentClosing = false;
        private float _headerBottomYLine;
        private int _pageNumber = 0;
        //private int _lastPageNumber = 0;

        private string _title = "";
        private iTextSharp.text.Image _imgLogo = null;
        private string _shid = "";
        private string _businessName = "";
        private string _landownerName = "";

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document) {

            _bf = _normalFont.GetCalculatedBaseFont(false);
            _cb = writer.DirectContent;
            _ct = new ColumnText(_cb);
            base.OnOpenDocument(writer, document);
        }

        //// we override the onEndPage method
        //public override void OnEndPage(PdfWriter writer, Document document) {

        //    String text = "Page " + _pageNumber.ToString();
        //    float len = _bf.GetWidthPoint(text, 8);
        //    _cb.BeginText();
        //    _cb.SetFontAndSize(_bf, 8);
        //    _cb.SetTextMatrix(280, 30);
        //    _cb.ShowText(text);
        //    _cb.EndText();

        //    base.OnEndPage(writer, document);
        //}

        public override void OnStartPage(PdfWriter writer, Document document) {

            if (!_isDocumentClosing) {

                _pageNumber++;

                // ===========================================================================
                // Create header column -- in this report this is the page's column object
                // ===========================================================================
                _ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY-24,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
                    PortraitPageSize.PgLeading, Element.ALIGN_TOP);
                _ct.YLine = PortraitPageSize.HdrTopYLine;

                // ======================================================
                // Header / Title Block
                // ======================================================
                float[] layout = new float[] { 216F, 108F, 216F };
                PdfPTable table = PdfReports.CreateTable(layout, 0);
                
                PdfReports.AddImage2Table(table, _imgLogo, "left");
                PdfReports.AddText2Table(table, " ", _normalFont);

                Paragraph p = new Paragraph();
                p.Add(new Phrase(_title, _titleFont));
                p.Add(new Phrase("\nPage " + _pageNumber.ToString(), _normalFont));
                PdfReports.AddText2Table(table, p, "center");

                // blank row
                PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                PdfReports.AddTableNoSplit(document, this, table);

                // ======================================================
                // Shareholder Block
                // ======================================================
                layout = new float[] { 67.5F, 67.5F, 55F, 147.5F, 67.5F, 135F };
                table = PdfReports.CreateTable(layout, 0);

                PdfReports.AddText2Table(table, "SHID:", _labelFont, "left");
                PdfReports.AddText2Table(table, _shid, _normalFont, "left");

                PdfReports.AddText2Table(table, "Name:", _labelFont, "left");
                PdfReports.AddText2Table(table, _businessName, _normalFont, "left");

                PdfReports.AddText2Table(table, "Land Owner:", _labelFont, "left");
                PdfReports.AddText2Table(table, _landownerName, _normalFont, "left");

                PdfReports.AddTableNoSplit(document, this, table);

                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(string title, iTextSharp.text.Image imgLogo, int pageNumber, string shid, string businessName, string landownerName) {

            _title = title;
            _imgLogo = imgLogo;
            _shid = shid;
            _businessName = businessName;
            _landownerName = landownerName;
            _pageNumber = pageNumber;

        }

        public int PageNumber {
            set { _pageNumber = value; }
        }
        public string SHID {
            set { _shid = value; }
        }
        public string BusinessName {
            set { _businessName = value; }
        }
        public string LandownerName {
            set { _landownerName = value; }
        }

        public float HeaderBottomYLine {
            get { return _headerBottomYLine; }
        }
        public bool IsDocumentClosing {
            set { _isDocumentClosing = value; }
            get { return _isDocumentClosing; }
        }
        public ColumnText GetColumnObject() {
            return _ct;
        }
    }
}
