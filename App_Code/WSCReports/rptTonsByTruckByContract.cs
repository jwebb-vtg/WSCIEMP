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
    /// Summary description for rptTonsByTruckByContract.
    /// </summary>
    public class rptTonsByTruckByContract {

        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        public static string ReportPackager(
            int cropYear, int shid, string contractNumbers, bool isCSV, string fileName, string logoUrl, string pdfTempfolder) {

            DirectoryInfo pdfDir = null;
            FileInfo[] pdfFiles = null;
            string filePath = "";
            string fileExtension = (isCSV ? ".csv" : ".pdf");
            //string fileName = shid.ToString() + "_TonTrkCnt_" + DateTime.Now.ToString("yyMMddHHmmss") + (isCSV ? ".csv" : ".pdf");

            try {

                pdfDir = new DirectoryInfo(pdfTempfolder);

                // Build the output file name by getting a list of all PDF files 
                // that begin with this session ID: use this as a name incrementer.				
                pdfFiles = pdfDir.GetFiles(fileName + "*" + fileExtension);
                fileName += "_" + Convert.ToString(pdfFiles.Length + 1) + fileExtension;

                filePath = pdfDir.FullName + @"\" + fileName;
                ReportBuilder(cropYear, shid, contractNumbers, isCSV, filePath);

                return filePath;
            }
            catch (System.Exception ex) {

                string errMsg = "cropYear: " + cropYear + "; " +
                    "Contract numbers: " + contractNumbers + "; " +
                    "fileName: " + fileName + "; " +
                    "pdfDir is null: " + (pdfDir == null).ToString() + "; " +
                    "pdfFiles is null: " + (pdfFiles == null).ToString() + "; " +
                    "filesPath: " + filePath;

                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("HReports.rptTonsByTruckByContract.ReportPackager: " + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(int cropYear, int shid, string contractNumbers, bool isCSV, string filePath) {

            const string METHOD_NAME = "rptTonsByTruckByContract.ReportBuilder: ";
            const string OneQuote = "\"";
            const string QuoteComma = "\",";

            // Build the contract information.
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetTonsByTruckByContract(conn, Convert.ToInt32(cropYear), shid, contractNumbers, isCSV)) {

                        if (isCSV) {

                            StringBuilder sb = new StringBuilder("");
                            using (StreamWriter sw = new StreamWriter(filePath, false)) {

                                while (dr.Read()) {

                                    // Create header on first pass 
                                    if (sb.Length == 0) {
                                        for (int i = 0; i < dr.FieldCount; i++) {
                                            sb.Append(OneQuote + dr.GetName(i) + QuoteComma);
                                        }
                                        sb.Length = sb.Length - 1;
                                        sw.WriteLine(sb.ToString());
                                    }
                                    sb.Length = 0;

                                    // Concat fields to produce a CSV
                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Business Name")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Station")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Truck")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Contract")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Date")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Yard Card")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetInt32(dr.GetOrdinal("In Wt")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetInt32(dr.GetOrdinal("Out Wt")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetInt32(dr.GetOrdinal("Dirt Wt")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Left")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetInt32(dr.GetOrdinal("Before Dirt")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetInt32(dr.GetOrdinal("After Dirt")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetDecimal(dr.GetOrdinal("Before Tons")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetDecimal(dr.GetOrdinal("After Tons")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Rate")));
                                    sb.Append(QuoteComma);

                                    sb.Append(OneQuote);
                                    sb.Append(dr.GetString(dr.GetOrdinal("Dollars")));
                                    sb.Append(OneQuote);

                                    sw.WriteLine(sb.ToString());
                                }
                            }
                        } else {

                            // Produce a PDF instead.
                            using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                                MakePDF(dr, cropYear, shid, fs);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(METHOD_NAME, ex);
                throw (wscex);
            }
        }

        private static void MakePDF(SqlDataReader dr, int cropYear, int shid, System.IO.FileStream fs) {

            // bus    sta    trk    cnt    date   ycard  inWt   OutWt  dtWt   Left BDirt  ADirt  BTons  ATons
            float[] _primaryTableLayout = new float[] { 80.5F, 77.0F, 30F, 37.5F, 48.5F, 42.5F, 51.5F, 51.0F, 49.0F, 22F, 57.5F, 57.0F, 58.0F, 58.0F };  // Total = 720			

            const string METHOD_NAME = "ReportBuilder";
            Document document = null;
            PdfWriter writer = null;
            TonsByTruckByContractEvent pgEvent = null;

            string[] columnNames = new string[_primaryTableLayout.Length];
            string rptTitle = "Western Sugar Cooperative\nTons By Truck By Contract";
            PdfPTable table = null;

            try {

                string busName = "";

                while (dr.Read()) {

                    if (document == null) {

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        //  ***  LANDSCAPE			   ***  
                        //document = new Document(iTextSharp.text.PageSize.LETTER.Rotate(), 36, 36, 54, 72);	// may have to adjust margins?
                        document = new Document(LandscapePageSize.PgPageSize, LandscapePageSize.PgLeftMargin,
                            LandscapePageSize.PgRightMargin, LandscapePageSize.PgTopMargin, LandscapePageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        // Column Names: Drive getting the column names by the size of the receiving array.
                        // the recordset may contain additional columns that work for the excel csv format, but not here.
                        for (int i = 0; i < columnNames.Length; i++) {
                            columnNames[i] = dr.GetName(i);
                        }

                        // Attach my override event handler(s)
                        busName = dr["Business Name"].ToString();
                        pgEvent = new TonsByTruckByContractEvent();
                        pgEvent.FillEvent(cropYear, shid, busName, rptTitle, columnNames, _primaryTableLayout);

                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                    }

                    //==========================================================
                    // Add data to the pdf
                    //==========================================================
                    table = PdfReports.CreateTable(_primaryTableLayout, 0);

                    string s;
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("Business Name")), _normalFont);
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("Station")), _normalFont);

                    s = dr.GetString(dr.GetOrdinal("Truck"));
                    if (s.IndexOf("Total") != -1) {
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("Truck")), _normalFont, 2);
                    } else {
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("Truck")), _normalFont);
                        PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("Contract")), _normalFont, "center");
                    }

                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("Date")), _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("Yard Card")), _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("In Wt")).ToString(), _normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("Out Wt")).ToString(), _normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("Dirt Wt")).ToString(), _normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("Left")), _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("Before Dirt")).ToString(), _normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("After Dirt")).ToString(), _normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("Before Tons")).ToString(), _normalFont, "right");
                    PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("After Tons")).ToString(), _normalFont, "right");

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

    public class TonsByTruckByContractEvent : PdfPageEventHelper, ICustomPageEvent {

        private Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private Font _subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        private Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        private Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        private float[] _headerLayout = new float[] { 180F, 360F, 180F };
        float[] _primaryTableLayout;

        // This is the contentbyte object of the writer
        PdfContentByte _cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;
        ColumnText _ct = null;

        private bool _isDocumentClosing = false;
        private float _headerBottomYLine;
        private int _pageNumber = 0;
        private int _lastPageNumber = 0;
        private int _cropYear = 0;
        private int _shid = 0;
        private string _busName = "";
        private string _rptTitle = "";
        string[] _columnNames;

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
                _ct.SetSimpleColumn(LandscapePageSize.HdrLowerLeftX, LandscapePageSize.HdrLowerLeftY,
                    LandscapePageSize.HdrUpperRightX, LandscapePageSize.HdrUpperRightY,
                    LandscapePageSize.PgLeading, Element.ALIGN_CENTER);
                _ct.YLine = LandscapePageSize.HdrTopYLine;

                // =======================================================
                // Add Report Header
                // =======================================================			
                PdfPTable table = PdfReports.CreateTable(_headerLayout, 0);

                PdfReports.AddText2Table(table, " ", _normalFont, _headerLayout.Length);
                PdfReports.AddText2Table(table, _rptTitle, _titleFont, "center", 3);
                PdfReports.AddText2Table(table, " ", _normalFont, _headerLayout.Length);

                //---------------------------------------
                // SHID
                //---------------------------------------
                Phrase phrase = null;
                Paragraph para = new Paragraph("", _normalFont);

                phrase = new Phrase("SHID: ", _labelFont);
                para.Add(phrase);

                phrase = new Phrase(_shid.ToString(), _normalFont);
                para.Add(phrase);
                PdfReports.AddText2Table(table, para);

                //---------------------------------------
                // BUSINESS NAME
                //---------------------------------------
                para = new Paragraph("", _normalFont);

                phrase = new Phrase("Shareholder: ", _labelFont);
                para.Add(phrase);

                phrase = new Phrase(_busName, _normalFont);
                para.Add(phrase);
                PdfReports.AddText2Table(table, para);

                //---------------------------------------
                // CROP YEAR
                //---------------------------------------
                para = new Paragraph("", _normalFont);

                phrase = new Phrase("Crop Year: ", _labelFont);
                para.Add(phrase);

                phrase = new Phrase(_cropYear.ToString(), _normalFont);
                para.Add(phrase);
                PdfReports.AddText2Table(table, para);

                PdfReports.AddText2Table(table, " ", _normalFont, _headerLayout.Length);
                PdfReports.AddTableNoSplit(document, this, table);

                // Add column names here
                table = PdfReports.CreateTable(_primaryTableLayout, 0);
                for (int i = 0; i < _columnNames.Length; i++) {
                    PdfReports.AddText2Table(table, _columnNames[i], _labelFont, "center");
                }

                PdfReports.AddTableNoSplit(document, this, table);
                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(int cropYear, int shid, string busName, string rptTitle, string[] columnNames, float[] primaryTableLayout) {

            _columnNames = columnNames;
            _cropYear = cropYear;
            _shid = shid;
            _busName = busName;
            _rptTitle = rptTitle;
            _primaryTableLayout = primaryTableLayout;
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