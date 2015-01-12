using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using WSCData;
using PdfHelper;

namespace WSCReports {
    /// <summary>
    /// Summary description for rptContractSummary.
    /// </summary>
    public class rptContractSummary {

        private const string MOD_NAME = "WSCReports.rptContractSummary.";
        private static float[] _primaryTableLayout = new float[] { 48.6F, 54F, 47.6F, 97.2F, 54F, 47.6F, 47.6F, 48.6F, 48.6F, 46.2F };

        public static string ReportPackager(int cropYear, int memberID, int shid, string busName,
            string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "ReportPackager: ";
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

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                        using (SqlDataReader dr = WSCReportsExec.RptContractDeliverySummary(conn,
                                  memberID, cropYear)) {
                            using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {

                                ReportBuilder(dr, cropYear, shid, busName, logoUrl, fs);
                            }
                        }
                    }
                }
                catch (System.Exception ex) {
                    string errMsg = "cropYear: " + cropYear.ToString() + "; " +
                        "SHID: " + shid;

                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME + ": " + errMsg, ex);
                    throw (wscEx);
                }

                return filePath;
            }
            catch (System.Exception ex) {
                string errMsg = "cropYear: " + cropYear.ToString() + "; " +
                    "SHID: " + shid;

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME + ": " + errMsg, ex);
                throw (wscEx);
            }
        }

        public static void ReportBuilder(SqlDataReader dr, int cropYear, int shid, string busName, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilder";
            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            iTextSharp.text.Image imgLogo = null;
            ContractSummaryEvent pgEvent = null;

            int resetFlag = 0;

            string rptTitle = "Contract Summary\nCrop Year " + cropYear.ToString();

            Font headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

            try {

                while (dr.Read()) {

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
                        pgEvent = new ContractSummaryEvent();                        
                        pgEvent.FillEvent(busName, shid, resetFlag, rptTitle, imgLogo, _primaryTableLayout);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                        //table = PdfReports.CreateTable(_primaryTableLayout, 1);
                        //AddDetailSectionHdr(ref table, labelFont, normalFont);
                    }

                    table = PdfReports.CreateTable(_primaryTableLayout, 1);

                    // =======================================================
                    // Add Delivery information
                    // =======================================================
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("ContractFactory")),
                        normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("ContractStation")),
                        normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("ContractNo")),
                        normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("LandownerName")),
                        normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("FinalNetTons")).ToString("#,##0.0000"),
                        normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("SugarPct")).ToString("##0.00"),
                        normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("TarePct")).ToString("##0.00"),
                        normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("TonsPerAcre")).ToString("#,##0.00"),
                        normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("SLMPct")).ToString("##0.0000"),
                        normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("ExtSugarTon")).ToString("#,##0"),
                        normalFont, "center");

                    PdfReports.AddTableNoSplit(document, pgEvent, table);
                }

                // ======================================================
                // Close document
                // ======================================================
                if (document != null) {

                    pgEvent.IsDocumentClosing = true;
                    document.Close();
                    document = null;
                }
                if (table == null) {
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
    }

    public class ContractSummaryEvent : PdfPageEventHelper, ICustomPageEvent {

        Font titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        Font subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        Font subNormalFont = FontFactory.GetFont("HELVETICA", 6F, Font.NORMAL);
        Font uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        ColumnText ct = null;

        private bool _isDocumentClosing = false;
        private float _headerBottomYLine = 0;
        private string _title = "";
        private int _pageNumber = 0;
        private string _shareholderName = "";
        private string _shid = "";
        private iTextSharp.text.Image _imgLogo = null;
        private float[] _primaryTableLayout = null;

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document) {

            bf = normalFont.GetCalculatedBaseFont(false);
            cb = writer.DirectContent;
            ct = new ColumnText(cb);
            base.OnOpenDocument(writer, document);
        }

        // we override the onEndPage method
        public override void OnEndPage(PdfWriter writer, Document document) {

            String text = "Page " + _pageNumber.ToString();
            float len = bf.GetWidthPoint(text, 8);
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(280, 30);
            cb.ShowText(text);
            cb.EndText();

            base.OnEndPage(writer, document);
        }

        public override void OnStartPage(PdfWriter writer, Document document) {

            if (!_isDocumentClosing) {

                _pageNumber++;

                // ===========================================================================
                // Create header column -- in this report this is the page's column object
                // ===========================================================================
                ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
                    PortraitPageSize.PgLeading, Element.ALIGN_CENTER);
                ct.YLine = PortraitPageSize.HdrTopYLine;

                // =======================================================
                // Add Logo
                // =======================================================
                if (_pageNumber == 1) {

                    float[] wscLogoLayout = new float[] { 413F, 127F };
                    PdfPTable logoTable = PdfReports.CreateTable(wscLogoLayout, 0);
                    PdfReports.AddText2Table(logoTable, " ", normalFont);
                    
                    PdfReports.AddImage2Table(logoTable, _imgLogo);
                    PdfReports.AddText2Table(logoTable, " ", titleFont, wscLogoLayout.Length);
                    PdfReports.AddTableNoSplit(document, this, logoTable);
                }

                float[] headerLayout = new float[] { 50F, 490F };
                PdfPTable table = PdfReports.CreateTable(headerLayout, 1);

                Paragraph p = new Paragraph(_title, titleFont);
                p.SetAlignment("center");

                PdfReports.AddText2Table(table, p, "center", headerLayout.Length);

                // Add blank lines
                PdfReports.AddText2Table(table, " ", subNormalFont, headerLayout.Length);
                PdfReports.AddText2Table(table, " ", normalFont, headerLayout.Length);

                // Add Header information
                PdfReports.AddText2Table(table, "SHID", labelFont);
                PdfReports.AddText2Table(table, "Shareholder Name", labelFont);

                PdfReports.AddText2Table(table, _shid, normalFont);
                PdfReports.AddText2Table(table, _shareholderName, normalFont);

                PdfReports.AddText2Table(table, " ", normalFont, headerLayout.Length);
                PdfReports.AddText2Table(table, " ", normalFont, headerLayout.Length);

                PdfReports.AddTableNoSplit(document, this, table);

                table = PdfReports.CreateTable(_primaryTableLayout, 0);
                AddDetailSectionHdr(ref table, labelFont, normalFont);
                PdfReports.AddTableNoSplit(document, this, table);

                _headerBottomYLine = ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        private static void AddDetailSectionHdr(ref PdfPTable table, Font labelFont, Font normalFont) {

            // Detail column headers (2 lines)
            PdfReports.AddText2Table(table, "Contract", labelFont, "center");
            PdfReports.AddText2Table(table, "Contract", labelFont, "center");
            PdfReports.AddText2Table(table, "Contract", labelFont, "center");
            PdfReports.AddText2Table(table, "Landowner", labelFont, "center");
            PdfReports.AddText2Table(table, "Final ", labelFont, "center");
            PdfReports.AddText2Table(table, " ", labelFont, "center");
            PdfReports.AddText2Table(table, " ", labelFont, "center");
            PdfReports.AddText2Table(table, "Tons Per", labelFont, "center");
            PdfReports.AddText2Table(table, " ", labelFont, "center");
            PdfReports.AddText2Table(table, "Lbs. Ext.", labelFont, "center");

            PdfReports.AddText2Table(table, "Factory", labelFont, "center");
            PdfReports.AddText2Table(table, "Station", labelFont, "center");
            PdfReports.AddText2Table(table, "No", labelFont, "center");
            PdfReports.AddText2Table(table, "Name", labelFont, "center");
            PdfReports.AddText2Table(table, "Net Tons", labelFont, "center");
            PdfReports.AddText2Table(table, "Sugar %", labelFont, "center");
            PdfReports.AddText2Table(table, "Tare %", labelFont, "center");
            PdfReports.AddText2Table(table, "Acre", labelFont, "center");
            PdfReports.AddText2Table(table, "SLM %", labelFont, "center");
            PdfReports.AddText2Table(table, "Sugar/Ton", labelFont, "center");
        }

        public void FillEvent(string shareholderName, int shid, int pageNumber, string title, iTextSharp.text.Image imgLogo, float[] primaryTableLayout) {

            _shareholderName = shareholderName;
            _shid = shid.ToString();
            _pageNumber = pageNumber;
            _title = title;
            _imgLogo = imgLogo;
            _primaryTableLayout = primaryTableLayout;
        }

        public string ShareholderName {
            get { return _shareholderName; }
            set { _shareholderName = value; }
        }
        public string SHID {
            get { return _shid; }
            set { _shid = value; }
        }
        public int PageNumber {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }
        public string Title {
            get { return _title; }
            set { _title = value; }
        }

        public float HeaderBottomYLine {
            get { return _headerBottomYLine; }
        }
        public bool IsDocumentClosing {
            set { _isDocumentClosing = value; }
            get { return _isDocumentClosing; }
        }
        public ColumnText GetColumnObject() {
            return ct;
        }
    }
}

