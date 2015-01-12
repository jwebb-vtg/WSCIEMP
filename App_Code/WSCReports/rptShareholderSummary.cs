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
using Color = iTextSharp.text.BaseColor;

namespace WSCReports {
    /// <summary>
    /// Summary description for rptShareholderSummary.
    /// </summary>
    public class rptShareholderSummary {

        private const string MOD_NAME = "WSCReports.rptShareholderSummary.";

        private static float[] _adviceTableLayout = new float[] { 70F, 70F, 300F };
        private static float[] _contractTableLayout = new float[] { 34.0F, 64.8F, 75.6F, 59.4F, 47.0F, 43.2F, 43.2F, 43.2F, 43.2F, 43.2F, 43.2F };

        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        public static string ReportPackager(
            int cropYear, ArrayList shidList, string busName, string growerPerformanceID, string regionCode, string areaCode,
            string regionName, string areaName, string userName, string fileName, string logoUrl, string pdfTempfolder) {

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

                ArrayList growerPerformanceList = new ArrayList();
                ArrayList regionCodeList = new ArrayList();
                ArrayList areaCodeList = new ArrayList();
                ArrayList regionNameList = new ArrayList();
                ArrayList areaNameList = new ArrayList();

                if (growerPerformanceID.Length > 0) {
                    growerPerformanceList.Add(growerPerformanceID);
                    regionCodeList.Add(regionCode);
                    areaCodeList.Add(areaCode);
                    regionNameList.Add(regionName);
                    areaNameList.Add(areaName);
                }

                using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {

                    ReportBuilder(cropYear, shidList, busName, growerPerformanceList, regionCodeList, areaCodeList, regionNameList, areaNameList, logoUrl, filePath, fs);
                }



                return filePath;
            }
            catch (System.Exception ex) {

                string errMsg = "cropYear: " + cropYear + "; " +
                    "fileName: " + fileName + "; " +
                    "pdfDir is null: " + (pdfDir == null).ToString() + "; " +
                    "pdfFiles is null: " + (pdfFiles == null).ToString() + "; " +
                    "filesPath: " + filePath;

                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("HReports.rptShareholderSummary.ReportPackager: " + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(int cropYear, ArrayList shidList, string busName, ArrayList growerPerformanceList, ArrayList regionCodeList, ArrayList areaCodeList,
            ArrayList regionNameList, ArrayList areaNameList, string logoUrl, string filePath, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilder: ";
            const string CharBlank = " ";
            const string CharAffirm = "X";

            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            ShareholderSummaryEvent pgEvent = null;
            iTextSharp.text.Image imgLogo = null;

            int iShid = 0;
            string areaCode = "";
            string regionCode = "";
            string regionName = "";
            string areaName = "";
            int growerPerformanceID = 0;

            string rptTitle = "Western Sugar Cooperative\nShareholder Summary for " + cropYear.ToString() + " Crop Year";

            string okFertility = "";
            string okIrrigation = "";
            string okStand = "";
            string okWeed = "";
            string okDisease = "";
            string okVariety = "";

            string descFertility = "";
            string descIrrigation = "";
            string descStand = "";
            string descWeed = "";
            string descDisease = "";
            string descVariety = "";

            // Build the contract information.
            try {

                for (int j = 0; j < shidList.Count; j++) {

                    string shid = shidList[j].ToString();
                    iShid = Convert.ToInt32(shid);

                    if (growerPerformanceList.Count == 0) {

                        busName = "";
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                            using (SqlDataReader dr = WSCField.SharholderSummaryGetAreas(conn, cropYear, iShid)) {

                                while (dr.Read()) {

                                    growerPerformanceList.Add(dr["GrowerPerformanceID"].ToString());
                                    regionCodeList.Add(dr["RegionCode"].ToString());
                                    areaCodeList.Add(dr["AreaCode"].ToString());
                                    regionNameList.Add(dr["RegionName"].ToString());
                                    areaNameList.Add(dr["AreaName"].ToString());
                                    if (busName.Length == 0) {
                                        busName = dr["BusName"].ToString();
                                    }
                                }
                            }
                        }
                    }

                    // ---------------------------------------------------------------------------------------------------------
                    // Given all of the pieces, crop year, shid, growerPerformanceID, region, and area, generate the report
                    // ---------------------------------------------------------------------------------------------------------
                    if (areaCodeList.Count > 0) {

                        for (int i = 0; i < areaCodeList.Count; i++) {

                            growerPerformanceID = Convert.ToInt32(growerPerformanceList[i]);
                            regionCode = regionCodeList[i].ToString();
                            areaCode = areaCodeList[i].ToString();
                            regionName = regionNameList[i].ToString();
                            areaName = areaNameList[i].ToString();

                            // ------------------------------------------------
                            // Collect the data: Get the report card.
                            // ------------------------------------------------
                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                                using (SqlDataReader dr = WSCField.GrowerAdviceGetBySHID(conn, growerPerformanceID)) {

                                    if (dr.Read()) {

                                        //busName = dr["gadBusinessName"].ToString();
                                        okFertility = dr["gadGoodFertilityManagement"].ToString();
                                        okIrrigation = dr["gadGoodIrrigationManagement"].ToString();
                                        okStand = dr["gadGoodStandEstablishment"].ToString();
                                        okWeed = dr["gadGoodWeedControl"].ToString();
                                        okDisease = dr["gadGoodDiseaseControl"].ToString();
                                        okVariety = dr["gadGoodVarietySelection"].ToString();

                                        descFertility = dr["gadTextFertilityManagement"].ToString();
                                        descIrrigation = dr["gadTextIrrigationManagement"].ToString();
                                        descStand = dr["gadTextStandEstablishment"].ToString();
                                        descWeed = dr["gadTextWeedControl"].ToString();
                                        descDisease = dr["gadTextDiseaseControl"].ToString();
                                        descVariety = dr["gadTextVarietySelection"].ToString();

                                    } else {

                                        //busName = "";
                                        okFertility = "";
                                        okIrrigation = "";
                                        okStand = "";
                                        okWeed = "";
                                        okDisease = "";
                                        okVariety = "";

                                        descFertility = "";
                                        descIrrigation = "";
                                        descStand = "";
                                        descWeed = "";
                                        descDisease = "";
                                        descVariety = "";
                                    }
                                }
                            }

                            if (document == null) {

                                // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                                //  ***  US LETTER: 612 X 792  ***
                                //document = new Document(iTextSharp.text.PageSize.LETTER, 36, 36, 54, 72);
                                document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                                    PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                                // we create a writer that listens to the document
                                // and directs a PDF-stream to a file				
                                writer = PdfWriter.GetInstance(document, fs);

                                imgLogo = PdfReports.GetImage(logoUrl, 127, 50, iTextSharp.text.Element.ALIGN_CENTER);

                                // Attach my override event handler(s)
                                //busName = dr["Business Name"].ToString();
                                pgEvent = new ShareholderSummaryEvent();
                                pgEvent.FillEvent(cropYear, shid, busName, rptTitle, regionName, areaName, imgLogo);

                                writer.PageEvent = pgEvent;

                                // Open the document
                                document.Open();

                            } else {

                                // everytime thru kick out a new page because we're on a different shid/region/area combination.
                                pgEvent.FillEvent(cropYear, shid, busName, rptTitle, regionName, areaName, imgLogo);
                                document.NewPage();
                            }

                            // -----------------------------------------------------
                            // Create the report card
                            // -----------------------------------------------------
                            table = PdfReports.CreateTable(_adviceTableLayout, 1);

                            Color borderColor = Color.BLACK;	//new Color(255, 0, 0);
                            float borderWidth = 1.0F;
                            int borderTypeAll = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                            float fPadding = 3;

                            // HEADER
                            iTextSharp.text.pdf.PdfPCell cell = PdfReports.AddText2Cell("Okay", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell("Opportunity\nfor\nImprovement", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell("Big Six Growing Practices", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            /// ----------------------------------------
                            // TBODY
                            /// ----------------------------------------
                            // Fertility
                            cell = PdfReports.AddText2Cell((okFertility == "Y" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell((okFertility == "N" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell("Fertility Management", _labelFont, PdfPCell.ALIGN_LEFT, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            // Irrigation
                            cell = PdfReports.AddText2Cell((okIrrigation == "Y" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell((okIrrigation == "N" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell("Irrigation Water Management", _labelFont, PdfPCell.ALIGN_LEFT, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            // Stand
                            cell = PdfReports.AddText2Cell((okStand == "Y" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell((okStand == "N" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell("Stand Establishment (Harvest Plant Population)", _labelFont, PdfPCell.ALIGN_LEFT, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            // Weed
                            cell = PdfReports.AddText2Cell((okWeed == "Y" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell((okWeed == "N" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell("Weed Control", _labelFont, PdfPCell.ALIGN_LEFT, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            // Disease
                            cell = PdfReports.AddText2Cell((okDisease == "Y" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell((okDisease == "N" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell("Disease & Insect Control", _labelFont, PdfPCell.ALIGN_LEFT, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            // Varitey
                            cell = PdfReports.AddText2Cell((okVariety == "Y" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell((okVariety == "N" ? CharAffirm : CharBlank), _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            cell = PdfReports.AddText2Cell("Proper Variety Selection", _labelFont, PdfPCell.ALIGN_LEFT, PdfPCell.ALIGN_BOTTOM,
                                fPadding, borderWidth, borderTypeAll, borderColor);
                            table.AddCell(cell);

                            PdfReports.AddText2Table(table, " ", _normalFont, 3);

                            PdfReports.AddTableNoSplit(document, pgEvent, table);

                            // ==========================================================
                            // Recommendations for Improvement.
                            // ==========================================================
                            table = PdfReports.CreateTable(_adviceTableLayout, 1);

                            // Caption
                            iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase("Recommendations for Improvement", _labelFont);
                            cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                            cell.Colspan = 3;

                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                            cell.Padding = fPadding;
                            cell.BorderWidth = borderWidth;
                            cell.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                            cell.BorderColor = borderColor;
                            table.AddCell(cell);

                            // Content
                            phrase = new iTextSharp.text.Phrase((descFertility.Length > 0 ? descFertility + "\n\n" : "") +
                                (descIrrigation.Length > 0 ? descIrrigation + "\n\n" : "") +
                                (descStand.Length > 0 ? descStand + "\n\n" : "") +
                                (descWeed.Length > 0 ? descWeed + "\n\n" : "") +
                                (descDisease.Length > 0 ? descDisease + "\n\n" : "") +
                                (descVariety.Length > 0 ? descVariety + "\n\n" : ""), _normalFont);

                            cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                            cell.Colspan = 3;

                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                            cell.Padding = fPadding;
                            cell.BorderWidth = borderWidth;
                            cell.Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                            cell.BorderColor = borderColor;
                            table.AddCell(cell);

                            PdfReports.AddText2Table(table, " ", _normalFont, table.NumberOfColumns);
                            PdfReports.AddText2Table(table, " ", _normalFont, table.NumberOfColumns);

                            PdfReports.AddTableNoSplit(document, pgEvent, table);

                            // ------------------------------------------------
                            // Create the contract dump.
                            // ------------------------------------------------
                            ArrayList cntPerfs;
                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                                cntPerfs = WSCField.ShareholderSummaryContracts(conn, iShid, cropYear, regionCode, areaCode);
                            }

                            // =======================================
                            // HEADER
                            // =======================================
                            table = PdfReports.CreateTable(_contractTableLayout, 0);
                            pgEvent.BuildContractDumpHeader(document, _contractTableLayout);
                            pgEvent.ContractTableLayout = _contractTableLayout;

                            // DATA
                            for (int k = 0; k < cntPerfs.Count; k++) {

                                ContractPerformanceState perf = (ContractPerformanceState)cntPerfs[k];

                                switch (perf.RowType) {

                                    case 1:

                                        table = PdfReports.CreateTable(_contractTableLayout, 0);

                                        PdfReports.AddText2Table(table, perf.ContractNumber, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.ContractStation, _normalFont);
                                        PdfReports.AddText2Table(table, perf.FieldDescription, _normalFont);
                                        PdfReports.AddText2Table(table, perf.LandownerName, _normalFont);
                                        PdfReports.AddText2Table(table, perf.HarvestFinalNetTons, _normalFont, "right");
                                        PdfReports.AddText2Table(table, perf.TonsPerAcre, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSugarPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestTarePct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSLMPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestExtractableSugar, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.BeetsPerAcre, _normalFont, "center");

                                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                                        break;

                                    case 2:

                                        table = PdfReports.CreateTable(_contractTableLayout, 0);

                                        PdfReports.AddText2Table(table, " ", _normalFont, _contractTableLayout.Length);
                                        PdfReports.AddText2Table(table, " ", _normalFont);
                                        PdfReports.AddText2Table(table, "Overall Average", _labelFont, 3);
                                        PdfReports.AddText2Table(table, perf.HarvestFinalNetTons, _normalFont, "right");
                                        PdfReports.AddText2Table(table, perf.TonsPerAcre, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSugarPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestTarePct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSLMPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestExtractableSugar, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.BeetsPerAcre, _normalFont, "center");

                                        break;

                                    case 3:

                                        PdfReports.AddText2Table(table, " ", _normalFont);
                                        PdfReports.AddText2Table(table, "Top 20% Area Average", _labelFont, 4);
                                        PdfReports.AddText2Table(table, perf.TonsPerAcre, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSugarPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestTarePct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSLMPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestExtractableSugar, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.BeetsPerAcre, _normalFont, "center");

                                        break;

                                    case 4:

                                        PdfReports.AddText2Table(table, " ", _normalFont);
                                        PdfReports.AddText2Table(table, "Your Rankings", _labelFont, 10);

                                        PdfReports.AddText2Table(table, " ", _normalFont);
                                        PdfReports.AddText2Table(table, areaName, _labelFont, 4);
                                        PdfReports.AddText2Table(table, perf.TonsPerAcre, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSugarPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestTarePct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSLMPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestExtractableSugar, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.BeetsPerAcre, _normalFont, "center");

                                        break;

                                    case 5:

                                        PdfReports.AddText2Table(table, " ", _normalFont);
                                        PdfReports.AddText2Table(table, regionName, _labelFont, 4);
                                        PdfReports.AddText2Table(table, perf.TonsPerAcre, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSugarPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestTarePct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestSLMPct, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.HarvestExtractableSugar, _normalFont, "center");
                                        PdfReports.AddText2Table(table, perf.BeetsPerAcre, _normalFont, "center");

                                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                                        break;
                                }                                
                            }

                            pgEvent.ContractTableLayout = null;
                        }
                    }

                    // --------------------------------------------
                    // --------  reset for next iteration  --------
                    // --------------------------------------------
                    growerPerformanceList.Clear();
                    regionCodeList.Clear();
                    areaCodeList.Clear();
                    regionNameList.Clear();
                    areaNameList.Clear();
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
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + errMsg, ex);
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
    }

    public class ShareholderSummaryEvent : PdfPageEventHelper, ICustomPageEvent {

        private Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private Font _subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        private Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        private Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        private float[] _headerLayout = new float[] { 127F, 286F, 127F };

        // This is the contentbyte object of the writer
        PdfContentByte _cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;
        ColumnText _ct = null;
        // we will put the final number of pages in a template
        PdfTemplate _template = null;

        private float _headerBottomYLine;
        private bool _isDocumentClosing = false;
        private int _pageNumber = 0;
        private int _lastPageNumber = 0;
        private int _cropYear = 0;
        private int _shid = 0;
        private int _lastShid = 0;
        private string _busName = "";
        private string _rptTitle = "";
        private string _regionName = "";
        private string _areaName = "";
        private string _lastAreaName = "";
        private iTextSharp.text.Image _imgLogo = null;
        private float[] _contractTableLayout = null;

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

            int pageN = _lastPageNumber;
            string text = "Page " + pageN.ToString() + " of ";
            float len = _bf.GetWidthPoint(text, 8);
            _cb.BeginText();
            _cb.SetFontAndSize(_bf, 8);
            _cb.SetTextMatrix(280, 30);
            _cb.ShowText(text);
            _cb.EndText();
            _cb.AddTemplate(_template, 280 + len, 30);

            if (_lastShid != _shid) {
                
                // close out the last section of pages
                _template.BeginText();
                _template.SetFontAndSize(_bf, 8);
                _template.ShowText((_lastPageNumber).ToString());
                _template.EndText();
            }

            base.OnEndPage(writer, document);
        }

        // Used to supply end of document values to template.
        public override void OnCloseDocument(PdfWriter writer, Document document) {

            // add template text to document here.
            _template.BeginText();
            _template.SetFontAndSize(_bf, 8);
            _template.ShowText((_lastPageNumber).ToString());
            _template.EndText();

            base.OnCloseDocument(writer, document);
        }

        public override void OnStartPage(PdfWriter writer, Document document) {

            if (!_isDocumentClosing) {

                bool isNewArea = false;

                if (_pageNumber == 0 || _lastShid != _shid) {

                    _pageNumber = 0;
                    _lastPageNumber = 0;
                    _lastShid = _shid;

                    isNewArea = true;
                    _lastAreaName = _areaName;  

                    _template = _cb.CreateTemplate(PortraitPageSize.HdrUpperRightX - PortraitPageSize.HdrLowerLeftX, 50);

                }
                if (_lastAreaName != _areaName) {
                    isNewArea = true;
                    _lastAreaName = _areaName;  
                }

                _pageNumber++;

                // ===========================================================================
                // Create header column -- in this report this is the page's column object
                // ===========================================================================
                _ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
                    PortraitPageSize.PgLeading, Element.ALIGN_TOP);
                _ct.YLine = PortraitPageSize.HdrTopYLine;

                // =======================================================
                // Add Report Header
                // =======================================================			
                PdfPTable table = PdfReports.CreateTable(_headerLayout, 0);

                PdfReports.AddText2Table(table, " ", _normalFont, table.NumberOfColumns);

                PdfReports.AddText2Table(table, DateTime.Now.ToShortDateString(), _normalFont);
                PdfReports.AddText2Table(table, _rptTitle, _titleFont, "center");
                if (isNewArea) {
                    PdfReports.AddImage2Table(table, _imgLogo);
                } else {
                    PdfReports.AddText2Table(table, " ", _normalFont);
                }

                //---------------------------------------
                // SHID
                //---------------------------------------
                Paragraph para = new Paragraph("", _normalFont);
                Phrase phrase = new Phrase("SHID: ", _labelFont);
                para.Add(phrase);
                phrase = new Phrase(_shid.ToString(), _normalFont);
                para.Add(phrase);
                PdfReports.AddText2Table(table, para);

                //---------------------------------------			
                // BUSINESS NAME
                //---------------------------------------
                para = new Paragraph("", _normalFont);
                phrase = new Phrase("Name: ", _labelFont);
                para.Add(phrase);
                phrase = new Phrase(_busName, _labelFont);
                para.Add(phrase);
                PdfReports.AddText2Table(table, para, 2);

                //---------------------------------------			
                // REGION NAME
                //---------------------------------------
                para = new Paragraph("", _normalFont);
                phrase = new Phrase("Region: ", _labelFont);
                para.Add(phrase);
                phrase = new Phrase(_regionName, _normalFont);
                para.Add(phrase);
                PdfReports.AddText2Table(table, para);

                //---------------------------------------			
                // AREA NAME
                //---------------------------------------
                para = new Paragraph("", _normalFont);
                phrase = new Phrase("Area: ", _labelFont);
                para.Add(phrase);
                phrase = new Phrase(_areaName, _normalFont);
                para.Add(phrase);
                PdfReports.AddText2Table(table, para, 2);

                PdfReports.AddText2Table(table, " ", _normalFont, table.NumberOfColumns);
                PdfReports.AddText2Table(table, " ", _normalFont, table.NumberOfColumns);
                PdfReports.AddTableNoSplit(document, this, table);

                if (_contractTableLayout != null) {
                    BuildContractDumpHeader(document, _contractTableLayout);       // this routine adds table to document
                }

                _headerBottomYLine = _ct.YLine;

            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(int cropYear, string shid, string busName, string rptTitle, string regionName, string areaName, iTextSharp.text.Image imgLogo) {

            if (_shid == 0) {
                _lastShid = Convert.ToInt32(shid);
                _lastAreaName = areaName;
            }

            _cropYear = cropYear;
            _shid = Convert.ToInt32(shid);
            _busName = busName;
            _rptTitle = rptTitle;
            _regionName = regionName;
            _areaName = areaName;
            _imgLogo = imgLogo;
        }

        public void BuildContractDumpHeader(Document document, float[] layout) {

            PdfPTable table = PdfReports.CreateTable(layout, 0);
            
            //--------------------------------
            // HEADER
            //--------------------------------
            Color borderColor = Color.BLACK;	//new Color(255, 0, 0);
            float borderWidth = 1.0F;
            int borderTypeAll = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            float fPadding = 2;

            iTextSharp.text.pdf.PdfPCell cell = PdfReports.AddText2Cell("Cnt #", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Station", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Field Desc.", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Landowner", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Final Net Tons", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Tons / Acre", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Sugar %", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Tare %", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("SLM %", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Lbs Ext. Sugar / Ton", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("Plant Pop", _labelFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
                fPadding, borderWidth, borderTypeAll, borderColor);
            table.AddCell(cell);

            PdfReports.AddTableNoSplit(document, this, table);
        }

        public float[] ContractTableLayout {
            set { _contractTableLayout = value; }
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