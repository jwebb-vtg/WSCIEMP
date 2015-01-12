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
    /// Summary description for rptDeliveryByDaySummary.
    /// </summary>
    public class rptDeliveryByDaySummary {

        private const string MOD_NAME = "WSCReports.rptDeliveryByDaySummary.";
        private static float[] _primaryTableLayout = new float[] { 81F, 81F, 46.6F, 111.4F, 59.4F, 37.8F, 37.8F, 37.8F, 47.2F };
        private static string[] _detailTableNames = {"Contract", "Delivery", "Contract", "Landowner", "Final ", " ", 
            " ", " ", "Lbs. Ext.", "Factory", "Station", "No", "Name", "Net Tons", "Sugar %", "Tare %", "SLM %", "Sugar/Ton"};

        private static Font _headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        public static string ReportPackager(int cropYear, int memberID, int shid, string busName, string deliveryDate, string fileName, string logoUrl, string pdfTempfolder) {

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
                        using (SqlDataReader dr = WSCReportsExec.RptContractDeliveryByDay(conn,
                                  memberID, cropYear, deliveryDate)) {
                            using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {

                                ReportBuilder(dr, cropYear, shid, busName, deliveryDate, logoUrl, fs);
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

        public static void ReportBuilder(SqlDataReader dr, int cropYear,
            int shid, string busName, string deliveryDate, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilder";
            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            iTextSharp.text.Image imgLogo = null;
            DeliveryByDayEvent pgEvent = null;

            const int RowBlockSize = 4;
            int rowCount = 1;
            int resetFlag = 0;

            string rptTitle = "Delivery By Day - Summary";

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
                        pgEvent = new DeliveryByDayEvent();
                        pgEvent.FillEvent(busName, shid, deliveryDate, resetFlag, rptTitle, imgLogo, _detailTableNames, _primaryTableLayout);

                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                    }

                    table = PdfReports.CreateTable(_primaryTableLayout, 1);

                    // =======================================================
                    // Add Delivery information
                    // =======================================================			
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("ContractFactory")),
                        _normalFont, "left");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("DeliveryStation")),
                        _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("ContractNo")),
                        _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("LandownerName")),
                        _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("FinalNetTons")).ToString("#,##0.0000"),
                        _normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("SugarPct")).ToString("##0.00"),
                        _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("TarePct")).ToString("##0.00"),
                        _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("SLMPct")).ToString("##0.0000"),
                        _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("ExtractableSugarPerTon")).ToString("#,##0"),
                        _normalFont, "center");

                    // add a blank after every row-block-size for visual breaks.
                    if (rowCount % RowBlockSize == 0) {
                        PdfReports.AddText2Table(table, " ", _normalFont, _primaryTableLayout.Length);
                        rowCount = 1;
                    } else {
                        rowCount++;
                    }

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
    }

    public class DeliveryByDayEvent : PdfPageEventHelper, ICustomPageEvent {

        private Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private Font _subTitlePlusFont = FontFactory.GetFont("HELVETICA", 10F, Font.NORMAL);
        private Font _subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        private Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private Font _subNormalFont = FontFactory.GetFont("HELVETICA", 6F, Font.NORMAL);
        private Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        private Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        // This is the contentbyte object of the writer
        PdfContentByte _cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;
        ColumnText _ct = null;

        private bool _isDocumentClosing = false;
        private float _headerBottomYLine;
        private string _title = "";
        private int _pageNumber = 0;
        private string _shareholderName = "";
        private string _shid = "";
        private string _deliveryDate = "";
        private iTextSharp.text.Image _imgLogo = null;
        private string[] _hdrTableNames = null;
        private float[] _hdrTableLayout = null;

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document) {

            _bf = _normalFont.GetCalculatedBaseFont(false);
            _cb = writer.DirectContent;
            _ct = new ColumnText(_cb);
            base.OnOpenDocument(writer, document);
        }

        // we override the onEndPage method
        public override void OnEndPage(PdfWriter writer, Document document) {

            String text = "Page " + _pageNumber.ToString();
            float len = _bf.GetWidthPoint(text, 8);
            _cb.BeginText();
            _cb.SetFontAndSize(_bf, 8);
            _cb.SetTextMatrix(280, 30);
            _cb.ShowText(text);
            _cb.EndText();

            base.OnEndPage(writer, document);
        }

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
                // Add Logo
                // =======================================================
                if (_pageNumber == 1) {

                    float[] wscLogoLayout = new float[] { 413F, 127F };
                    PdfPTable logoTable = PdfReports.CreateTable(wscLogoLayout, 0);
                    PdfReports.AddText2Table(logoTable, " ", _normalFont);                    
                    PdfReports.AddImage2Table(logoTable, _imgLogo);
                    PdfReports.AddText2Table(logoTable, " ", _titleFont, wscLogoLayout.Length);

                    PdfReports.AddTableNoSplit(document, this, logoTable);

                }

                float[] headerLayout = new float[] { 50F, 200F, 290F };
                PdfPTable table = PdfReports.CreateTable(headerLayout, 1);

                Paragraph p = new Paragraph(_title, _titleFont);
                PdfReports.AddText2Table(table, p, "center", headerLayout.Length);

                // Add blank lines
                PdfReports.AddText2Table(table, " ", _subNormalFont, headerLayout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, headerLayout.Length);

                // Add Header information
                PdfReports.AddText2Table(table, "SHID", _labelFont);
                PdfReports.AddText2Table(table, "Shareholder Name", _labelFont);
                PdfReports.AddText2Table(table, "Delivery Date", _labelFont);

                PdfReports.AddText2Table(table, _shid, _normalFont);
                PdfReports.AddText2Table(table, _shareholderName, _normalFont);
                PdfReports.AddText2Table(table, _deliveryDate, _normalFont);

                PdfReports.AddText2Table(table, " ", _normalFont, headerLayout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, headerLayout.Length);

                PdfReports.AddTableNoSplit(document, this, table);

                table = PdfReports.CreateTable(_hdrTableLayout, 1);
                PdfReports.FillHeaderLabels(ref table, _hdrTableNames, _labelFont);
                PdfReports.AddTableNoSplit(document, this, table);

                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(string shareholderName, int shid, string deliveryDate,
            int pageNumber, string title, iTextSharp.text.Image imgLogo, string[] hdrTableNames, float[] hdrTableLayout) {

            ShareholderName = shareholderName;
            SHID = shid.ToString();
            DeliveryDate = deliveryDate;
            PageNumber = pageNumber;
            Title = title;
            _imgLogo = imgLogo;
            _hdrTableNames = hdrTableNames;
            _hdrTableLayout = hdrTableLayout;
        }

        public string ShareholderName {
            get { return _shareholderName; }
            set { _shareholderName = value; }
        }
        public string SHID {
            get { return _shid; }
            set { _shid = value; }
        }
        public string DeliveryDate {
            get { return _deliveryDate; }
            set { _deliveryDate = value; }
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
            return _ct;
        }
    }
}

