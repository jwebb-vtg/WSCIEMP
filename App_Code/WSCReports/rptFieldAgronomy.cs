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

    public class rptFieldAgronomy {

        public static string ReportPackager(int cropYear, string factoryIDList, string stationIDList, string contractIDList, string fieldIDList,
            int userID, string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "WSCReports.rptFieldAgronomy.ReportPackager: ";
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
                        fieldIDList.Length > 0) {

                        using (SqlConnection conn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                            using (SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                                using (SqlDataReader dr = WSCField.FieldGetAgronomyData(conn1, factoryIDList, stationIDList,
                                          contractIDList, fieldIDList, userID)) {
                                    using (SqlDataReader drLandDesc = WSCField.FieldGetContractingData(conn2,
                                              factoryIDList, stationIDList, contractIDList, fieldIDList, userID)) {

                                        using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                                            ReportBuilder(dr, drLandDesc, cropYear, logoUrl, fs);
                                        }
                                    }
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
                        "contractIDList: " + contractIDList + "; " +
                        "fieldIDList: " + fieldIDList + "; " +
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
                    "contractIDList: " + contractIDList + "; " +
                    "fieldIDList: " + fieldIDList + "; " +
                    "fileName: " + fileName + "; " +
                    "pdfDir is null: " + (pdfDir == null).ToString() + "; " +
                    "pdfFiles is null: " + (pdfFiles == null).ToString() + "; " +
                    "filesPath: " + filePath;
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(SqlDataReader dr, SqlDataReader drLandDesc, int cropYear, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "WSCReports.rpfFieldAgronomy.ReportBuilder: ";
            string tmp = null;
            Document document = null;
            PdfWriter writer = null;
            ArrayList values = null;
            PdfPTable table = null;

            float[] tmpLayout = null;
            iTextSharp.text.Paragraph para = null;
            iTextSharp.text.Phrase phrase = null;
            RptAgronomy agronomy = null;
            AgronomyReportEvent pgEvent = null;
            iTextSharp.text.pdf.PdfPCell cell = null;

            Font titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
            Font subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
            Font headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

            try {

                while (dr.Read()) {

                    try {

                        if (document == null) {

                            agronomy = WSCField.GetAgronomyData();

                            // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                            //  ***  US LETTER: 612 X 792  ***
                            //document = new Document(PageSize.LETTER);

                            document = new Document(PageSize.LETTER, PortraitPageSize.PgLeftMargin,
                                PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin - 18, PortraitPageSize.PgBottomMargin - 36);//54

                            // we create a writer that listens to the document
                            // and directs a PDF-stream to a file				
                            writer = PdfWriter.GetInstance(document, fs);

                            // Attach my override event handler(s)
                            pgEvent = new AgronomyReportEvent();
                            pgEvent.FillEvent(cropYear.ToString(), dr.GetString(dr.GetOrdinal("fld_shid")), dr.GetString(dr.GetOrdinal("fld_business_name")),
                                dr.GetString(dr.GetOrdinal("fld_address1")), dr.GetString(dr.GetOrdinal("fld_address2")), dr.GetString(dr.GetOrdinal("fld_city")),
                                dr.GetString(dr.GetOrdinal("fld_state")), dr.GetString(dr.GetOrdinal("fld_zip")), dr.GetString(dr.GetOrdinal("fld_agriculturist")),
                                dr.GetString(dr.GetOrdinal("fld_contract_no")), dr.GetString(dr.GetOrdinal("fld_landowner_name")));

                            writer.PageEvent = pgEvent;

                            // Open the document
                            document.Open();

                        } else {

                            pgEvent.FillEvent(cropYear.ToString(), dr.GetString(dr.GetOrdinal("fld_shid")), dr.GetString(dr.GetOrdinal("fld_business_name")),
                                dr.GetString(dr.GetOrdinal("fld_address1")), dr.GetString(dr.GetOrdinal("fld_address2")), dr.GetString(dr.GetOrdinal("fld_city")),
                                dr.GetString(dr.GetOrdinal("fld_state")), dr.GetString(dr.GetOrdinal("fld_zip")), dr.GetString(dr.GetOrdinal("fld_agriculturist")),
                                dr.GetString(dr.GetOrdinal("fld_contract_no")), dr.GetString(dr.GetOrdinal("fld_landowner_name")));
                        }

                        // ======================================================
                        // Generate a new page.
                        // ======================================================
                        if (
                            table != null) {
                            document.NewPage();
                        }

                        // ======================================================
                        // Legal Land Description
                        // ======================================================						

                        if (drLandDesc.Read()) {
                            
                            // ***  ROW 1  ***
                            tmpLayout = new float[] { 13F, 13.5F, 11F, 13.5F, 11F, 13.5F, 11F, 13.5F };
                            table = PdfReports.CreateTable(tmpLayout, 0);

                            PdfReports.AddText2Table(table, "Field Name: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_field_name")), normalFont);

                            PdfReports.AddText2Table(table, "Acres: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetInt32(drLandDesc.GetOrdinal("cntg_acres")).ToString(), normalFont);

                            PdfReports.AddText2Table(table, "State: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_lld_state")), normalFont);

                            PdfReports.AddText2Table(table, "County: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_lld_county")), normalFont);

                            // ***  ROW 2  ***
                            PdfReports.AddText2Table(table, "1/4 Quadrant: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_lld_quarter_quadrant")), normalFont);

                            PdfReports.AddText2Table(table, "Quadrant: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_lld_quadrant")), normalFont);

                            PdfReports.AddText2Table(table, "Section: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_lld_section")), normalFont);

                            PdfReports.AddText2Table(table, "Township: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_lld_township")), normalFont);


                            // ***  ROW 3  ***
                            PdfReports.AddText2Table(table, "Range: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_lld_range")), normalFont);

                            PdfReports.AddText2Table(table, "Latitude: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetDecimal(drLandDesc.GetOrdinal("cntg_latitude")).ToString(), normalFont);

                            PdfReports.AddText2Table(table, "Longitude: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetDecimal(drLandDesc.GetOrdinal("cntg_longitude")).ToString(), normalFont);

                            PdfReports.AddText2Table(table, "FSA Official: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_fsa_official")), normalFont);


                            // ***  ROW 4  ***
                            PdfReports.AddText2Table(table, "FSA Number: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_fsa_number")), normalFont, 3);

                            PdfReports.AddText2Table(table, "FSA State: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_fsa_state")), normalFont);

                            PdfReports.AddText2Table(table, "FSA County: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_fsa_county")), normalFont);

                            // ***  ROW 5  ***
                            PdfReports.AddText2Table(table, "Farm No: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_farm_number")), normalFont);

                            PdfReports.AddText2Table(table, "Tract No: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_tract_number")), normalFont);

                            PdfReports.AddText2Table(table, "Field No: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_field_number")), normalFont);

                            PdfReports.AddText2Table(table, "1/4 Field: ", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_quarter_field")), normalFont, 7);


                            // ***  ROW 6  ***
                            PdfReports.AddText2Table(table, "Description", labelFont);
                            PdfReports.AddText2Table(table, drLandDesc.GetString(drLandDesc.GetOrdinal("cntg_description")), normalFont, 7);

                            PdfReports.AddTableNoSplit(document, pgEvent, table);
                        }

                        // ======================================================
                        // Planting Block
                        // ======================================================
                        phrase = new Phrase("\nVariety: ", labelFont);
                        para = new Paragraph();
                        para.Add(phrase);

                        tmp = dr.GetString(dr.GetOrdinal("fld_seed_variety"));
                        if (tmp.Length == 0) {

                            tmpLayout = new float[] { 540F };
                            table = PdfReports.CreateTable(tmpLayout, 0);

                            // No seed found in Field record, so list the possible seed options 
                            // based on state information
                            string factoryNumber = dr.GetInt16(dr.GetOrdinal("fld_factory_no")).ToString();
                            if (factoryNumber.Length == 0) {

                                PdfReports.AddText2Table(table, para);

                                // No state in Field record, list all possible seed options
                                para = GetSeedsAsText(agronomy, "6", labelFont, normalFont, true);

                                cell = new iTextSharp.text.pdf.PdfPCell(para);
                                cell.BorderWidth = 1;
                                table.AddCell(cell);

                                phrase = new Phrase("\n", labelFont);
                                PdfReports.AddText2Table(table, phrase);

                                para = GetSeedsAsText(agronomy, "2", labelFont, normalFont, true);

                                cell = new iTextSharp.text.pdf.PdfPCell(para);
                                cell.BorderWidth = 1;

                                table.AddCell(cell);

                            } else {

                                PdfReports.AddText2Table(table, para);
                                para = GetSeedsAsText(agronomy, factoryNumber, labelFont, normalFont, true);

                                cell = new iTextSharp.text.pdf.PdfPCell(para);
                                cell.BorderWidth = 1;

                                table.AddCell(cell);
                            }

                        } else {

                            tmpLayout = new float[] { 540F };
                            table = PdfReports.CreateTable(tmpLayout, 0);

                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                            PdfReports.AddText2Table(table, para);
                        }

                        PdfReports.AddText2Table(table, new Paragraph(" \n ", FontFactory.GetFont("HELVETICA", 4F)), tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Seed Block
                        // ======================================================

                        tmpLayout = new float[] { 140F, 30F, 370F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // Seed Primed						
                        para = new Paragraph();
                        phrase = new Phrase("Seed: ", labelFont);
                        para.Add(phrase);

                        tmp = dr.GetString(dr.GetOrdinal("fld_seed_primed"));
                        if (tmp.Length > 0) {
                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                        } else {
                            values = new ArrayList();
                            foreach (TItem item in agronomy.SeedList.Items) {
                                if (item.Name.Length > 0) { values.Add(item.Name); }
                            }
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);
                        PdfReports.AddText2Table(table, " ", normalFont);

                        // Treatment
                        para = new Paragraph();
                        phrase = new Phrase("Treatment: ", labelFont);
                        para.Add(phrase);

                        tmp = dr.GetString(dr.GetOrdinal("fld_seed_treatment_chemical"));
                        if (tmp.Length > 0) {
                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                        } else {
                            values = new ArrayList();
                            foreach (TItem item in agronomy.SeedTreatmentList.Items) {
                                if (item.Name.Length > 0) { values.Add(item.Name); }
                            }
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, 4F)), tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Planting Stuff
                        // ======================================================
                        tmpLayout = new float[] { 170F, 370F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // Row Spacing.
                        para = new Paragraph();
                        phrase = new Phrase("Row Spacing: ", labelFont);
                        para.Add(phrase);

                        tmp = (dr.GetInt16(dr.GetOrdinal("fld_row_spacing"))).ToString();
                        if (tmp.Length > 0 && tmp == "0") { tmp = ""; }

                        if (tmp.Length > 0) {
                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                        } else {

                            values = new ArrayList();
                            foreach (TItem item in agronomy.RowSpacingList.Items) {
                                if (item.Name.Length > 0 && item.Name != "0") { values.Add(item.Name); }
                            }

                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);

                        // Planting Spacing
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_plant_spacing"))).ToString("#.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("Planting Spacing: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length > 0) {

                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                            PdfReports.AddText2Table(table, para);

                        } else {

                            values = new ArrayList();
                            foreach (TItem item in agronomy.PlantSpacingList.Items) {
                                if (item.Name.Length > 0 && item.Name != "0") { values.Add(item.Name); }
                            }

                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                            PdfReports.AddText2Table(table, para);
                        }

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Planting Date Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 370F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // Planting Date
                        tmp = dr.GetString(dr.GetOrdinal("fld_planting_date"));
                        para = new Paragraph();
                        phrase = new Phrase("Planting Date: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) {
                            tmp = "___________";
                        }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        // 80% Emerg Date
                        tmp = dr.GetString(dr.GetOrdinal("fld_emerg_80_date"));
                        para = new Paragraph();
                        phrase = new Phrase("80% Emerg Date: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) {
                            tmp = "___________";
                        }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Replanting Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 180F, 190F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // Replanting Date
                        tmp = dr.GetString(dr.GetOrdinal("fld_replant_date"));
                        para = new Paragraph();
                        phrase = new Phrase("Replanting Date: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) {
                            tmp = "____________";
                        }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        // Replant Variety
                        tmp = dr.GetString(dr.GetOrdinal("fld_replant_seed_variety"));
                        para = new Paragraph();
                        phrase = new Phrase("Replant Variety: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) {
                            tmp = "____________";
                        }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        // Acres Replanted
                        tmp = (dr.GetInt32(dr.GetOrdinal("fld_acres_replanted"))).ToString();
                        if (tmp == "0") { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("Acres Replanted: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) {
                            tmp = "____________";
                        }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        // Replant Reason
                        tmp = dr.GetString(dr.GetOrdinal("fld_replant_reason"));
                        para = new Paragraph();
                        phrase = new Phrase("Replant Reason: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) {
                            tmp = "____________";
                        }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        // Lost Reason
                        tmp = dr.GetString(dr.GetOrdinal("fld_lost_reason"));
                        para = new Paragraph();
                        phrase = new Phrase("Reason Lost: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) {
                            tmp = "____________";
                        }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        // Lost Acres
                        tmp = (dr.GetInt32(dr.GetOrdinal("fld_acres_lost"))).ToString();
                        if (tmp == "0") { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("Acres Lost: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) {
                            tmp = "____________";
                        }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Soil Sample Block
                        // ======================================================
                        tmpLayout = new float[] { 165F, 120F, 135F, 120F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // 1. Soil Texture
                        tmp = dr.GetString(dr.GetOrdinal("fld_soil_texture"));
                        para = new Paragraph();
                        phrase = new Phrase("Soil Texture: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length > 0) {
                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                        } else {

                            values = new ArrayList();
                            foreach (TItem item in agronomy.SoilTextureList.Items) {
                                if (item.Name.Length > 0) { values.Add(item.Name); }
                            }

                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);

                        // 2. Soil Sample
                        tmp = dr.GetString(dr.GetOrdinal("fld_test_season"));
                        para = new Paragraph();
                        phrase = new Phrase("Soil Sample: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length > 0) {
                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                        } else {

                            values = new ArrayList();
                            foreach (TItem item in agronomy.SoilTestList.Items) {
                                if (item.Name.Length > 0) { values.Add(item.Name); }
                            }

                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);

                        // 3. Nitrogen Sample Depth
                        tmp = (dr.GetInt16(dr.GetOrdinal("fld_test_depth"))).ToString();
                        if (tmp == "0") { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("N Sample Depth: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length > 0) {
                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                        } else {

                            values = new ArrayList();
                            foreach (TItem item in agronomy.SampleDepthList.Items) {
                                if (item.Name.Length > 0 && item.Name != "0") { values.Add(item.Name); }
                            }

                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);

                        // 4. grid / zone
                        para = new Paragraph();
                        para.Add(new Phrase(@"Grid\Zone: ", labelFont));
                        tmp = dr.GetString(dr.GetOrdinal("fld_grid_zone"));
                        if (tmp.Length == 0) { tmp = @"Yes \ No"; }
                        para.Add(new Phrase(tmp, normalFont));
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Soil Sample Test Results Block
                        // ======================================================
                        tmpLayout = new float[] { 90F, 72F, 90F, 72F, 72F, 72F, 72F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // Results: Label
                        para = new Paragraph();
                        phrase = new Phrase("Test Results: ", labelFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        // Nitrogen
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_N"))).ToString("#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("N: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) { tmp = "______"; }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);

                        para.Add(new Phrase(" lbs", normalFont));
                        PdfReports.AddText2Table(table, para);

                        // Phosphorus
                        para = new Paragraph();
                        phrase = new Phrase("P: ", labelFont);
                        para.Add(phrase);

                        tmp = dr.GetString(dr.GetOrdinal("fld_test_P"));
                        if (tmp.Length == 0) {
                            values = new ArrayList();
                            foreach (TItem item in agronomy.SamplePList.Items) {
                                if (item.Name.Length > 0) { values.Add(item.Name); }
                            }
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        } else {
                            para.Add(new Phrase(tmp, normalFont));
                        }
                        PdfReports.AddText2Table(table, para);

                        // Potasium
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_K"))).ToString("#.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("K: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) { tmp = "______"; }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);

                        para.Add(new Phrase(" ppm", normalFont));
                        PdfReports.AddText2Table(table, para);

                        // Salts
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_salts"))).ToString("#.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("Salts: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) { tmp = "______"; }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);

                        PdfReports.AddText2Table(table, para);

                        // pH
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_pH"))).ToString("#.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("pH: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) { tmp = "______"; }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);

                        PdfReports.AddText2Table(table, para);

                        // Organic Material
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_test_org_mat"))).ToString("#.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("O.M.: ", labelFont);
                        para.Add(phrase);

                        if (tmp.Length == 0) { tmp = "______"; }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);

                        para.Add(new Phrase(" %", normalFont));
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Last Yr Manure Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 370F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // Last year
                        tmp = (dr.GetInt32(dr.GetOrdinal("fld_last_yr_manure"))).ToString();
                        if (tmp.Length > 0 && tmp == "0") { tmp = ""; }
                        para = new Paragraph();
                        phrase = new Phrase("Last Year Manure Applied: ", labelFont);
                        //para.Add(phrase);
                        PdfReports.AddText2Table(table, phrase);

                        if (tmp.Length > 0) {
                            phrase = new Phrase(tmp, normalFont);
                            //para.Add(phrase);
                            PdfReports.AddText2Table(table, phrase);
                        } else {
                            values = new ArrayList();
                            for (int i = 0; i < 5; i++) {
                                values.Add((cropYear - i).ToString());
                            }

                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Fertilizer Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 75F, 75F, 75F, 145F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        PdfReports.AddText2Table(table, "", labelFont);
                        PdfReports.AddText2Table(table, "Actual units/Acre      Average Over Whole Field", labelFont, 4);

                        // Fall
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_N"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "Fall " + (cropYear - 1).ToString() + " Applied Fertilizer", labelFont);
                        PdfReports.AddText2Table(table, "N: " + tmp + " lbs", normalFont);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_P"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "P: " + tmp + " lbs", normalFont);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_K"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "K: " + tmp + " lbs", normalFont);

                        PdfReports.AddText2Table(table, "", normalFont);

                        // Spring
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_N"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "Spring " + cropYear.ToString() + " Applied Fertilizer", labelFont);
                        PdfReports.AddText2Table(table, "N: " + tmp + " lbs", normalFont);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_P"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "P: " + tmp + " lbs", normalFont);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_K"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "K: " + tmp + " lbs", normalFont);

                        PdfReports.AddText2Table(table, "", normalFont);

                        // In Season
                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_N"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "In Season " + cropYear.ToString() + " Applied Fertilizer", labelFont);
                        PdfReports.AddText2Table(table, "N: " + tmp + " lbs", normalFont);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_P"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "P: " + tmp + " lbs", normalFont);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_K"))).ToString("#,###.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        PdfReports.AddText2Table(table, "K: " + tmp + " lbs", normalFont);

                        PdfReports.AddText2Table(table, "", normalFont);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Start Fert Block (mostly)
                        // ======================================================
                        tmpLayout = new float[] { 170F, 370F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // Starter fert
                        para = new Paragraph();
                        phrase = new Phrase("Starter Fertilizer: ", labelFont);
                        para.Add(phrase);

                        tmp = dr.GetString(dr.GetOrdinal("fld_fert_starter"));
                        if (tmp.Length == 0) { tmp = @"Yes \ No"; }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);
                        PdfReports.AddText2Table(table, "", normalFont);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Insecticide Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 370F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        // pre-emerg. insect.
                        para = new Paragraph();
                        phrase = new Phrase("Pre-Emergence Insecticide: ", labelFont);
                        para.Add(phrase);

                        tmp = dr.GetString(dr.GetOrdinal("fld_pre_insecticide"));
                        if (tmp.Length == 0) { tmp = @"Yes \ No"; }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        // post.emerg. insect.
                        para = new Paragraph();
                        phrase = new Phrase("Post-Emergence Insecticide: ", labelFont);
                        para.Add(phrase);

                        tmp = dr.GetString(dr.GetOrdinal("fld_post_insectcide"));
                        if (tmp.Length == 0) { tmp = @"Yes \ No"; }
                        phrase = new Phrase(tmp, normalFont);
                        para.Add(phrase);
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Root Maggot Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 123F, 123F, 124F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        para = new Paragraph();
                        para.Add(new Phrase("Insecticide-Root Maggot: ", labelFont));

                        tmp = dr.GetString(dr.GetOrdinal("fld_root_maggot_insecticide"));
                        if (tmp == "") { tmp = @"Yes \ No"; }
                        para.Add(new Phrase(tmp, normalFont));

                        PdfReports.AddText2Table(table, para);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_rootm_counter_lbs"))).ToString("#.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        tmp = "Counter 15G: " + tmp + " lbs/A";
                        PdfReports.AddText2Table(table, tmp, normalFont);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_rootm_temik_lbs"))).ToString("#.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        tmp = "Temik: " + tmp + " lbs/A";
                        PdfReports.AddText2Table(table, tmp, normalFont);

                        tmp = (dr.GetDecimal(dr.GetOrdinal("fld_rootm_thimet_lbs"))).ToString("#.#");
                        if (tmp.Length > 0 && Decimal.Parse(tmp) == 0) { tmp = ""; }
                        if (tmp.Length == 0) { tmp = "______"; }
                        tmp = "Thimet: " + tmp + " lbs/A";
                        PdfReports.AddText2Table(table, tmp, normalFont);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Weed Control Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 370F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        para = new Paragraph();
                        para.Add(new Phrase("Pre-Emergence Weed Control: ", labelFont));

                        string origAnswer = (dr.GetString(dr.GetOrdinal("fld_pre_weed_ctrl"))).ToString();
                        string displayAnswer = origAnswer;
                        if (displayAnswer.Length == 0) { displayAnswer = @"Yes \ No"; }

                        para.Add(new Phrase(displayAnswer, normalFont));
                        PdfReports.AddText2Table(table, para);

                        // Number of Herbicide RX
                        para = new Paragraph();
                        phrase = new Phrase("Number of Herbicide Treatments: ", labelFont);
                        para.Add(phrase);

                        tmp = dr.GetInt32(dr.GetOrdinal("fld_herbicide_rx_count")).ToString();
                        if (tmp.Length > 0 && tmp != "0") {
                            phrase = new Phrase(tmp, normalFont);
                            para.Add(phrase);
                        } else {
                            values = new ArrayList();
                            foreach (TItem item in agronomy.HerbicideRxCountList.Items) {
                                if (item.Name.Length > 0 && item.Name != "0") { values.Add(item.Name); }
                            }
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Layby Herbicide Block
                        // =====================================================
                        tmpLayout = new float[] { 170F, 370F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        PdfReports.AddText2Table(table, "Layby Herbicide: ", labelFont);

                        para = new Paragraph();
                        tmp = dr.GetString(dr.GetOrdinal("fld_layby_herbicide_chemical"));

                        if (tmp.Length > 0) {
                            para.Add(new Phrase(tmp, normalFont));
                        } else {
                            values = new ArrayList();
                            foreach (TItem item in agronomy.LaybyHerbicideList.Items) {
                                if (item.Name.Length > 0) { values.Add(item.Name); }
                            }
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        }
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Cercospora Program Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 250F, 120F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        PdfReports.AddText2Table(table, "Cercospora Program: ", labelFont);
                        PdfReports.AddText2Table(table, "", normalFont);
                        PdfReports.AddText2Table(table, "", normalFont);

                        // Get values array for any missing chemical entries
                        values = new ArrayList();
                        foreach (TItem item in agronomy.CercosporaChemList.Items) {
                            if (item.Name.Length > 0) { values.Add(item.Name); }
                        }

                        // Application #1
                        cell = new iTextSharp.text.pdf.PdfPCell(new Phrase("Application 1 Chemical: ", labelFont));
                        cell.BorderWidth = 0;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        tmp = dr.GetString(dr.GetOrdinal("fld_cercsp_app1_chemical"));
                        para = new Paragraph();
                        if (tmp.Length == 0) {
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        } else {
                            para.Add(new Phrase(tmp, normalFont));
                        }

                        PdfReports.AddText2Table(table, para);

                        tmp = dr.GetString(dr.GetOrdinal("fld_cercospora_app1_date"));
                        if (tmp.Length == 0) { tmp = "____________"; }
                        para = new Paragraph();
                        para.Add(new Phrase("Date: ", labelFont));
                        para.Add(new Phrase(tmp, normalFont));
                        PdfReports.AddText2Table(table, para);

                        // Application #2
                        cell = new iTextSharp.text.pdf.PdfPCell(new Phrase("Application 2 Chemical: ", labelFont));
                        cell.BorderWidth = 0;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        tmp = dr.GetString(dr.GetOrdinal("fld_cercsp_app2_chemical"));
                        para = new Paragraph();
                        if (tmp.Length == 0) {
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        } else {
                            para.Add(new Phrase(tmp, normalFont));
                        }

                        PdfReports.AddText2Table(table, para);

                        tmp = dr.GetString(dr.GetOrdinal("fld_cercospora_app2_date"));
                        if (tmp.Length == 0) { tmp = "____________"; }
                        para = new Paragraph();
                        para.Add(new Phrase("Date: ", labelFont));
                        para.Add(new Phrase(tmp, normalFont));
                        PdfReports.AddText2Table(table, para);

                        // Application #3
                        cell = new iTextSharp.text.pdf.PdfPCell(new Phrase("Application 3 Chemical: ", labelFont));
                        cell.BorderWidth = 0;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        tmp = dr.GetString(dr.GetOrdinal("fld_cercsp_app3_chemical"));
                        para = new Paragraph();
                        if (tmp.Length == 0) {
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        } else {
                            para.Add(new Phrase(tmp, normalFont));
                        }

                        PdfReports.AddText2Table(table, para);

                        tmp = dr.GetString(dr.GetOrdinal("fld_cercospora_app3_date"));
                        if (tmp.Length == 0) { tmp = "____________"; }
                        para = new Paragraph();
                        para.Add(new Phrase("Date: ", labelFont));
                        para.Add(new Phrase(tmp, normalFont));
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Powdery Mildew Block
                        // ======================================================
                        tmpLayout = new float[] { 170F, 180F, 190F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

                        para = new Paragraph();
                        para.Add(new Phrase("Treated for Powdery Mildew: ", labelFont));
                        tmp = dr.GetString(dr.GetOrdinal("fld_treated_powdery_mildew"));
                        if (tmp.Length == 0) { tmp = @"Yes \ No"; }
                        para.Add(new Phrase(tmp, normalFont));
                        PdfReports.AddText2Table(table, para);

                        // Hail
                        para = new Paragraph();
                        para.Add(new Phrase("Hail Stress: ", labelFont));

                        tmp = dr.GetString(dr.GetOrdinal("fld_hail_stress"));
                        if (tmp.Length == 0) {
                            values = new ArrayList();
                            foreach (TItem item in agronomy.HailStressList.Items) {
                                if (item.Name.Length > 0) { values.Add(item.Name); }
                            }
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        } else {
                            para.Add(new Phrase(tmp, normalFont));
                        }
                        PdfReports.AddText2Table(table, para);

                        // Weed Control
                        para = new Paragraph();
                        para.Add(new Phrase("Weed Control: ", labelFont));

                        tmp = dr.GetString(dr.GetOrdinal("fld_weed_control"));
                        if (tmp.Length == 0) {
                            values = new ArrayList();
                            foreach (TItem item in agronomy.WeedControlList.Items) {
                                if (item.Name.Length > 0) { values.Add(item.Name); }
                            }
                            PdfReports.AddValues2Paragraph(para, values, normalFont);
                        } else {
                            para.Add(new Phrase(tmp, normalFont));
                        }
                        PdfReports.AddText2Table(table, para);

                        //PdfReports.AddText2Table(table, new Paragraph(" \n ", new Font(Font.HELVETICA, NewLineHeight)), tmpLayout.Length);
                        //PdfReports.AddText2Table(table, " ", normalFont, tmpLayout.Length);
                        //PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Nematode \ Rhizoctonia Block
                        // ======================================================
                        //tmpLayout = new float[] { 170F, 370F };
                        //table = PdfReports.CreateTable(tmpLayout, 0);

                        // Nematode
                        para = new Paragraph();
                        para.Add(new Phrase("Treated for Nematode: ", labelFont));
                        tmp = dr.GetString(dr.GetOrdinal("fld_treated_nematode"));
                        if (tmp.Length == 0) { tmp = @"Yes \ No"; }
                        para.Add(new Phrase(tmp, normalFont));
                        PdfReports.AddText2Table(table, para);

                        // Rhizoctonia
                        para = new Paragraph();
                        para.Add(new Phrase("Treated for Rhizoctonia: ", labelFont));
                        tmp = dr.GetString(dr.GetOrdinal("fld_treated_rhizoctonia"));
                        if (tmp.Length == 0) { tmp = @"Yes \ No"; }
                        para.Add(new Phrase(tmp, normalFont));
                        PdfReports.AddText2Table(table, para);

                        PdfReports.AddText2Table(table, " ", normalFont);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                    }
                    catch (System.Exception ex) {
                        WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME, ex);
                        throw (wscEx);
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
                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                if (drLandDesc != null) {
                    if (!drLandDesc.IsClosed) {
                        drLandDesc.Close();
                    }
                }
                if (document != null) {
                    pgEvent.IsDocumentClosing = true;
                    document.Close();
                }                
                if (writer != null) {
                    writer.Close();
                }
            }
        }

        private static iTextSharp.text.Paragraph GetSeedsAsText(RptAgronomy agronomy, string factoryNumber,
            Font labelFont, Font normalFont, bool underline) {

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
                    label = varietyType.Substring(0, 1).ToUpper() + varietyType.Substring(1).ToLower() + " ";
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
    }

    public class AgronomyReportEvent : PdfPageEventHelper, ICustomPageEvent {

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
        private int _lastPageNumber = 0;
        private string _cropYear;
        string _shid;
        string _busName;
        string _adr1;
        string _adr2;
        string _city;
        string _state;
        string _postalCode;
        string _aggie;
        string _contractNo;
        string _landownerName;

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document) {

            _bf = _normalFont.GetCalculatedBaseFont(false);
            _cb = writer.DirectContent;
            _ct = new ColumnText(_cb);
            base.OnOpenDocument(writer, document);
        }

        // we override the onEndPage method
        public override void OnEndPage(PdfWriter writer, Document document) {

            _lastPageNumber++;
            String text = "Page " + _lastPageNumber.ToString();
            float len = _bf.GetWidthPoint(text, 8);
            _cb.BeginText();
            _cb.SetFontAndSize(_bf, 8);
            _cb.SetTextMatrix(280, 30);
            _cb.ShowText(text);
            _cb.EndText();

            if (_lastPageNumber != _pageNumber) {
                _lastPageNumber = _pageNumber;
            }

            base.OnEndPage(writer, document);
        }

        public override void OnStartPage(PdfWriter writer, Document document) {

            if (!_isDocumentClosing) {

                _pageNumber++;

                // ===========================================================================
                // Create header column -- in this report this is the page's column object
                // ===========================================================================
                _ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY-18,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY+18,
                    PortraitPageSize.PgLeading, Element.ALIGN_TOP);
                _ct.YLine = PortraitPageSize.HdrTopYLine+18;

                StringBuilder sb = new StringBuilder();
                float[] logoLayout = new float[] { 240F, 60F, 240F };
                PdfPTable table = PdfReports.CreateTable(logoLayout, 0);                

                // ======================================================
                // Header / Title Block
                // ======================================================
                iTextSharp.text.pdf.PdfPCell cell = PdfReports.AddText2Table(table, "Western Sugar Cooperative\n(Grower Owned)", _subTitleFont);
                PdfReports.AddText2Table(table, "", _normalFont);
                cell = PdfReports.AddText2Table(table, "WSC " + _cropYear + " Agronomic Database", _titleFont);

                PdfReports.AddText2Table(table, "", _normalFont, logoLayout.Length);

                // ======================================================
                // Shareholder Block
                // ======================================================
                string tmp;
                sb.Append("\nSHID: " + _shid);
                sb.Append("\n" + _busName);
                tmp = _adr1;
                if (tmp.Length > 0) {
                    sb.Append("\n" + tmp);
                }
                tmp = _adr2;
                if (tmp.Length > 0) {
                    sb.Append("\n" + tmp);
                }
                tmp = _city + ", " + _state + " " + _postalCode;
                sb.Append("\n" + tmp + "\n");

                iTextSharp.text.Phrase p = new Phrase(sb.ToString() + "\n", _normalFont);
                sb.Length = 0;
                PdfReports.AddText2Table(table, p);

                PdfReports.AddText2Table(table, "", _normalFont);

                sb.Append("Agriculturist: " + _aggie);
                sb.Append("\nContract: " + _contractNo);
                sb.Append("\nLand Owner: " + _landownerName);

                p = new Phrase(sb.ToString() + "\n", _normalFont);
                sb.Length = 0;
                PdfReports.AddText2Table(table, p);

                PdfReports.AddTableNoSplit(document, this, table);
                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(string cropYear, string shid, string busName,
            string adr1, string adr2, string city, string state, string postalCode, string aggie, string contractNo, string landownerName) {

            _cropYear = cropYear;
            _shid = shid;
            _busName = busName;
            _adr1 = adr1;
            _adr2 = adr2;
            _city = city;
            _state = state;
            _postalCode = postalCode;
            _aggie = aggie;
            _contractNo = contractNo;
            _landownerName = landownerName;
            _pageNumber = 0;
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
