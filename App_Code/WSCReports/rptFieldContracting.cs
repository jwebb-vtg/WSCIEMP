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
    /// Summary description for rptFieldContracting.
    /// </summary>
    public class rptFieldContracting {

        public static string ReportPackager(
            int cropYear, string factoryIDList, string stationIDList, string contractIDList, string fieldIDList,
            int userID, string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "rptFieldContracting.ReportPackager: ";
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

                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                            using (SqlDataReader dr = WSCField.FieldGetContractingData(conn, factoryIDList,
                                      stationIDList, contractIDList, fieldIDList, userID)) {

                                using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                                    ReportBuilder(dr, cropYear, logoUrl, fs);
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

        private static void ReportBuilder(SqlDataReader dr, int cropYear, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "rptFieldContracting.ReportBuilder: ";
            string rptTitle = "";
            StringBuilder sb = new StringBuilder();
            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            ContractingReportEvent pgEvent = null;
            iTextSharp.text.Image imgLogo = null;

            Font titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
            Font subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
            Font headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

            try {

                while (dr.Read()) {

                    try {

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
                            rptTitle = cropYear.ToString() + " Shareholder Contracting\nJanuary 15th through March 30th";

                            // Attach my override event handler(s)
                            pgEvent = new ContractingReportEvent();
                            pgEvent.FillEvent(DateTime.Now.ToShortDateString(), 0, rptTitle, imgLogo);
                            writer.PageEvent = pgEvent;

                            // Open the document
                            document.Open();
                        }

                        // ======================================================
                        // Generate a new page.
                        // ======================================================
                        if (table != null) {
                            pgEvent.FillEvent(DateTime.Now.ToShortDateString(), 0, rptTitle, imgLogo);
                            document.NewPage();
                        }

                        // ======================================================
                        // Shareholder Block
                        // ======================================================
                        string cntNo = "Contract #: " + dr.GetString(dr.GetOrdinal("cntg_contract_no"));
                        float[] addrLayout = new float[] { 270F, 270F };
                        table = PdfReports.CreateTable(addrLayout, 0);

                        PdfReports.AddText2Table(table, "\nSHID: " + dr.GetString(dr.GetOrdinal("cntg_shid")),
                            normalFont, "left");
                        PdfReports.AddText2Table(table, cntNo, normalFont, "right");

                        Paragraph p = PdfReports.GetAddressBlock(dr.GetString(dr.GetOrdinal("cntg_business_name")),
                            dr.GetString(dr.GetOrdinal("cntg_address1")),
                            dr.GetString(dr.GetOrdinal("cntg_address2")),
                            dr.GetString(dr.GetOrdinal("cntg_city")) + ", " +
                            dr.GetString(dr.GetOrdinal("cntg_state")) + " " +
                            dr.GetString(dr.GetOrdinal("cntg_zip")),
                            0F, 15F, iTextSharp.text.Element.ALIGN_LEFT, normalFont);
                        PdfReports.AddText2Table(table, p);

                        string landOwner = "Land Owner: " + dr.GetString(dr.GetOrdinal("cntg_landowner_name"));
                        PdfReports.AddText2Table(table, landOwner, normalFont, "right");

                        // Add a blank line.
                        PdfReports.AddText2Table(table, " ", normalFont, "center", addrLayout.Length);

                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        // ======================================================
                        // Legal Land Description
                        // ======================================================	

                        // ***  ROW 1  ***
                        float[] tmpLayout = new float[] { 13F, 13.5F, 11F, 13.5F, 11F, 13.5F, 11F, 13.5F };
                        table = PdfReports.CreateTable(tmpLayout, 0);

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

                        // ======================================================
                        // Field Contracting
                        // ======================================================
                        float[] fcLayout = new float[] { 120.0F, 150.0F, 100.0F, 170.0F };
                        table = PdfReports.CreateTable(fcLayout, 1);
                        table.DefaultCell.PaddingTop = 3.0F;
                        table.DefaultCell.PaddingBottom = 0;
                        table.DefaultCell.PaddingLeft = 0;
                        table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.DefaultCell.VerticalAlignment = Element.ALIGN_BOTTOM;

                        // Row 1
                        PdfReports.AddText2Table(table, "Land Ownership: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_ownership")).ToString(), normalFont);
                        PdfReports.AddText2Table(table, " ", normalFont, 2);

                        // Row 2
                        PdfReports.AddText2Table(table, "Rotation Length (years): ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_rotation_len")).ToString(), normalFont);
                        PdfReports.AddText2Table(table, "Prior Crop: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_prev_crop_type")).ToString(), normalFont);

                        // Row 3
                        PdfReports.AddText2Table(table, "Years field has had beets: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_beet_years")).ToString(), normalFont);
                        PdfReports.AddText2Table(table, "Tillage: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_tillage")).ToString(), normalFont);

                        // blank
                        PdfReports.AddText2Table(table, " ", normalFont, fcLayout.Length);

                        PdfReports.AddText2Table(table, " ", normalFont);
                        PdfReports.AddText2Table(table, "Pre-Harvest", labelFont, "center");
                        PdfReports.AddText2Table(table, "Post-Harvest", labelFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Rhizomania Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_rhizomania")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_rhizomania")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Aphanomyces Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_aphanomyces")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_aphanomyces")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Curly Top Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_curly_top")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_curlytop")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Fusarium Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_fusarium")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_fusarium")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Rhizoctonia Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_rhizoctonia")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_rhizoctonia")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Nematode Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_nematode")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_nematode")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Cercospora Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_cercospora")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_cercospora")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Root Aphid Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_root_aphid")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_rootaphid")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, "Powdery Mildew Suspect: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_suspect_powdery_mildew")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_powderymildew")).ToString(), normalFont, "center");
                        PdfReports.AddText2Table(table, " ", normalFont);

                        PdfReports.AddText2Table(table, " ", normalFont, 4);
                        PdfReports.AddTableNoSplit(document, pgEvent, table);

                        float[] lld2Layout = new float[] { 67.5F, 67.5F, 55F, 80F, 67.5F, 67.5F, 67.5F, 67.5F };
                        table = PdfReports.CreateTable(lld2Layout, 1);

                        PdfReports.AddText2Table(table, "Irrigation System: ", labelFont, 2);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_irrigation_method")).ToString(), normalFont, "left", 2);
                        PdfReports.AddText2Table(table, " ", labelFont, 4);

                        PdfReports.AddText2Table(table, "Water Source: ", labelFont, 2);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_irrigation_source")).ToString(), normalFont, "left", 2);
                        PdfReports.AddText2Table(table, "Water: ", labelFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("cntg_post_water")).ToString(), normalFont, "left");
                        PdfReports.AddText2Table(table, " ", labelFont, 2);

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
                string errMsg = "sb is null: " + (sb == null).ToString() + "; " +
                    "document is null: " + (document == null).ToString() + "; " +
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
    }

    public class ContractingReportEvent : PdfPageEventHelper, ICustomPageEvent {

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
        private string _title = "";
        private int _pageNumber = 0;
        //private int _lastPageNumber = 0;
        private iTextSharp.text.Image _imgLogo = null;
        private string _statementDate = "";

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document) {

            _bf = _normalFont.GetCalculatedBaseFont(false);
            _cb = writer.DirectContent;
            _ct = new ColumnText(_cb);
            base.OnOpenDocument(writer, document);
        }

        // we override the onEndPage method
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
                _ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
                    PortraitPageSize.PgLeading, Element.ALIGN_CENTER);
                _ct.YLine = PortraitPageSize.HdrTopYLine;

                // =======================================================
                // Add Header
                // =======================================================
                float[] wscLogoLayout = new float[] { 60, 353F, 127F };
                PdfPTable logoTable = PdfReports.CreateTable(wscLogoLayout, 0);

                PdfReports.AddText2Table(logoTable, _statementDate, _normalFont);
                PdfReports.AddText2Table(logoTable, _title, _titleFont, "center");

                // Only add actual logo on First page of set.
                if (_pageNumber == 1) {
                    PdfReports.AddImage2Table(logoTable, _imgLogo);
                } else {
                    PdfReports.AddText2Table(logoTable, " ", _normalFont);
                }
                PdfReports.AddText2Table(logoTable, " \n \n ", _titleFont, wscLogoLayout.Length);
                PdfReports.AddTableNoSplit(document, this, logoTable);

                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(string statementDate, int pageNumber, string title, iTextSharp.text.Image imgLogo) {

            if (statementDate != null && statementDate.Length > 0) {
                _statementDate = statementDate;
            } else {
                _statementDate = "";
            }

            _pageNumber = pageNumber;
            _title = title;
            _imgLogo = imgLogo;
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
