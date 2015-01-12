using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Xml;
using System.Text;
using WSCData;
using PdfHelper;

namespace WSCReports {
    /// <summary>
    /// Summary description for rptContractPayeeSummary.
    /// </summary>
    public class rptContractPayeeSummary {
        private const string MOD_NAME = "WSCReports.rptContractPayeeSummary.";
        private static float[] _primaryTableLayout = new float[] { 133F, 133F, 88F, 108F, 78F };

        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 10F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 10F, Font.BOLD);
        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);

        public string ReportPackager(int cropYear, DateTime reportDate, string shid, string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "ReportPackager: ";
            DirectoryInfo pdfDir = null;
            FileInfo[] pdfFiles = null;
            string filePath = "";

            try {

                try {

                    pdfDir = new DirectoryInfo(pdfTempfolder);

                    // Build the output file name by getting a list of all PDF files 
                    // that begin with this session ID: use this as a name incrementer.				
                    pdfFiles = pdfDir.GetFiles(fileName + "*.pdf");
                    fileName += "_" + Convert.ToString(pdfFiles.Length + 1) + ".pdf";

                    filePath = pdfDir.FullName + @"\" + fileName;

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        if (shid != null && shid.Length == 0) {
                            shid = null;
                        }

                        using (SqlDataReader dr = WSCPayment.RptContractPayeeSummary1(conn, cropYear, shid)) {
                            using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {

                                ReportBuilder(dr, cropYear, reportDate, logoUrl, fs);
                            }
                        }
                    }
                }
                catch (System.Exception ex) {
                    string errMsg = "cropYear: " + cropYear.ToString();

                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME + ": " + errMsg, ex);
                    throw (wscEx);
                }

                return filePath;
            }
            catch (System.Exception ex) {
                string errMsg = "cropYear: " + cropYear.ToString();

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME + ": " + errMsg, ex);
                throw (wscEx);
            }
        }

        private void ReportBuilder(SqlDataReader dr, int cropYear, DateTime reportDate, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilder: ";

            int contractNumber = 0;
            int groSHID = 0;
            int lastGroSHID = 0;

            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            iTextSharp.text.Image imgLogo = null;

            ContractPayeeSummaryEvent pgEvent = null;
            string rptTitle = "Western Sugar Cooperative\n" +
                "Contract Payee Summary\n" +
                "Crop Year " + cropYear;

            try {

                while (dr.Read()) {

                    contractNumber = Convert.ToInt32(dr.GetString(dr.GetOrdinal("ContractNumber")));
                    groSHID = Convert.ToInt32(dr.GetString(dr.GetOrdinal("GroAdrNumber")));
                    if (document == null) {

                        lastGroSHID = groSHID;

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        imgLogo = PdfReports.GetImage(logoUrl, 127, 50, iTextSharp.text.Element.ALIGN_CENTER);

                        // Attach my override event handler(s)
                        pgEvent = new ContractPayeeSummaryEvent();
                        pgEvent.FillEvent(dr, reportDate.ToShortDateString(), 0, rptTitle, imgLogo);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                    } else {
                        if (lastGroSHID != groSHID) {

                            lastGroSHID = groSHID;
                            pgEvent.FillEvent(dr, reportDate.ToShortDateString(), 0, rptTitle, imgLogo);

                            document.NewPage();
                        }
                    }

                    // =======================================================
                    // Build Report
                    // =======================================================
                    table = PdfReports.CreateTable(_primaryTableLayout, 0);

                    Phrase phrase = new Phrase("Contract: ", _labelFont);
                    Paragraph p = new Paragraph("", _normalFont);
                    p.Add(phrase);
                    phrase = new Phrase(dr.GetString(dr.GetOrdinal("ContractNumber")), _normalFont);
                    p.Add(phrase);
                    PdfReports.AddText2Table(table, p);

                    phrase = new Phrase("Grower #: ", _labelFont);
                    p = new Paragraph("", _normalFont);
                    p.Add(phrase);
                    phrase = new Phrase(dr.GetString(dr.GetOrdinal("GroAdrNumber")), _normalFont);
                    p.Add(phrase);
                    PdfReports.AddText2Table(table, p);

                    phrase = new Phrase("Landowner #: ", _labelFont);
                    p = new Paragraph("", _normalFont);
                    p.Add(phrase);
                    phrase = new Phrase(dr.GetString(dr.GetOrdinal("LdoAdrNumber")) + " - " + dr.GetString(dr.GetOrdinal("LandownerName")), _normalFont);
                    p.Add(phrase);
                    PdfReports.AddText2Table(table, p, 3);

                    PdfReports.AddText2Table(table, "Factory", _labelFont);
                    PdfReports.AddText2Table(table, "Station", _labelFont);
                    PdfReports.AddText2Table(table, "Assoc Member", _labelFont);
                    PdfReports.AddText2Table(table, "LO Name on Check", _labelFont);
                    PdfReports.AddText2Table(table, "Pac Dues", _labelFont);

                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("FactoryName")), _normalFont);
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("StationName")), _normalFont);
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("AssocMember")), _normalFont, "center");
                    PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("CashRent")), _normalFont, "center");
                    PdfReports.AddText2Table(table, "$" + dr.GetDecimal(dr.GetOrdinal("PacDues")).ToString("0.00"), _normalFont);

                    // Add Payee information
                    ShowPayeeInformation(table, contractNumber, cropYear, dr.GetBoolean(dr.GetOrdinal("SplitRetain")), dr.GetBoolean(dr.GetOrdinal("SplitChemical")));

                    ShowAcreage(table, dr.GetInt16(dr.GetOrdinal("ContractAcres")), dr.GetInt16(dr.GetOrdinal("PlantedAcres")),
                        dr.GetInt16(dr.GetOrdinal("AcresLost")), dr.GetInt16(dr.GetOrdinal("HarvestAcres")));

                    PdfReports.AddText2Table(table, " ", _normalFont, _primaryTableLayout.Length);
                    PdfReports.AddText2Table(table, " ", _normalFont, _primaryTableLayout.Length);

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

        private void ShowAcreage(PdfPTable table, int contractAcres, int plantedAcres, int lostAcres, int harvestAcres) {

            const string METHOD_NAME = "rptContractPayeeSummary.ShowAcreage";

            try {

                Paragraph p = new Paragraph("", _normalFont);
                Phrase phrase = new Phrase("Acres Contracted: ", _labelFont);
                p.Add(phrase);
                phrase = new Phrase(contractAcres.ToString("#,##0"), _normalFont);
                p.Add(phrase);
                PdfReports.AddText2Table(table, p);

                p = new Paragraph("", _normalFont);
                phrase = new Phrase("Acres Planted: ", _labelFont);
                p.Add(phrase);
                phrase = new Phrase(plantedAcres.ToString("#,##0"), _normalFont);
                p.Add(phrase);
                PdfReports.AddText2Table(table, p);

                p = new Paragraph("", _normalFont);
                phrase = new Phrase("Acres Lost: ", _labelFont);
                p.Add(phrase);
                phrase = new Phrase(lostAcres.ToString("#,##0"), _normalFont);
                p.Add(phrase);
                PdfReports.AddText2Table(table, p);

                p = new Paragraph("", _normalFont);
                phrase = new Phrase("Acres Harvested: ", _labelFont);
                p.Add(phrase);
                phrase = new Phrase(harvestAcres.ToString("#,##0"), _normalFont);
                p.Add(phrase);
                PdfReports.AddText2Table(table, p);

                PdfReports.AddText2Table(table, " ", _normalFont);
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(METHOD_NAME, ex);
                throw (wscex);
            }
        }

        private void ShowPayeeInformation(PdfPTable table, int contractNumber, int cropYear, bool splitRetain, bool splitChemical) {

            const string METHOD_NAME = "rptContractPayeeSummary.ShowPayeeInformation";

            try {

                Paragraph p = new Paragraph("", _normalFont);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCPayment.RptContractPayeeSummary2(conn, contractNumber, cropYear)) {

                        while (dr.Read()) {

                            if (dr.GetInt16(dr.GetOrdinal("PayeeNumber")) == 1) {

                                Phrase phrase = new Phrase("1st Payee: ", _labelFont);
                                p.Add(phrase);
                                phrase = new Phrase(dr.GetString(dr.GetOrdinal("PayeeName")) + "\n", _normalFont);
                                p.Add(phrase);
                                phrase = new Phrase(dr.GetFloat(dr.GetOrdinal("SplitPercent")).ToString("##.0000") + "%" + "\n", _normalFont);
                                p.Add(phrase);
                                phrase = new Phrase("Mailed To: ", _labelFont);
                                p.Add(phrase);
                                phrase = new Phrase(dr.GetString(dr.GetOrdinal("AddressNumber")) + "  " + dr.GetString(dr.GetOrdinal("BusinessName")), _normalFont);
                                p.Add(phrase);

                                phrase = new Phrase("     Split Retains: ", _labelFont);
                                p.Add(phrase);
                                phrase = new Phrase((splitRetain ? "Y" : "N"), _normalFont);
                                p.Add(phrase);

                                phrase = new Phrase("     Split Chemicals: ", _labelFont);
                                p.Add(phrase);
                                phrase = new Phrase((splitChemical ? "Y" : "N") + "\n", _normalFont);
                                p.Add(phrase);

                            } else {
                                Phrase phrase = new Phrase("2nd Payee: ", _labelFont);
                                p.Add(phrase);
                                phrase = new Phrase(dr.GetString(dr.GetOrdinal("PayeeName")) + "\n", _normalFont);
                                p.Add(phrase);
                                phrase = new Phrase(dr.GetFloat(dr.GetOrdinal("SplitPercent")).ToString("##.0000") + "%" + "\n", _normalFont);
                                p.Add(phrase);
                                phrase = new Phrase("Mailed To: ", _labelFont);
                                p.Add(phrase);
                                phrase = new Phrase(dr.GetString(dr.GetOrdinal("AddressNumber")) + "  " + dr.GetString(dr.GetOrdinal("BusinessName")) + "\n", _normalFont);
                                p.Add(phrase);
                            }
                        }

                        PdfReports.AddText2Table(table, p, _primaryTableLayout.Length);
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(METHOD_NAME, ex);
                throw (wscex);
            }
        }
    }

    public class ContractPayeeSummaryEvent : PdfPageEventHelper, ICustomPageEvent {

        Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        Font _subTitleFont = FontFactory.GetFont("HELVETICA", 10F, Font.NORMAL);
        Font _normalFont = FontFactory.GetFont("HELVETICA", 10F, Font.NORMAL);
        Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        Font _labelFont = FontFactory.GetFont("HELVETICA", 10F, Font.BOLD);

        // This is the contentbyte object of the writer
        PdfContentByte _cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;
        ColumnText _ct = null;
        // we will put the final number of pages in a template
        PdfTemplate _template = null;

        private bool _isDocumentClosing = false;
        private float _headerBottomYLine;
        private string _title = "";
        private int _pageNumber = 0;
        private int _lastPageNumber = 0;
        private iTextSharp.text.Image _imgLogo = null;

        private string _groName = "";
        private string _groAdr1 = "";
        private string _groAdr2 = "";
        private string _groCSZ = "";

        private string _statementDate = "";

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

            if (_pageNumber == 0) {
                // close out the last section of pages
                _template.BeginText();
                _template.SetFontAndSize(_bf, 8);
                _template.ShowText((_lastPageNumber).ToString());
                _template.EndText();
            }

            if (_lastPageNumber != _pageNumber) {
                _lastPageNumber = _pageNumber;
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

                _pageNumber++;

                if (_pageNumber == 1) {
                    _template = _cb.CreateTemplate(PortraitPageSize.HdrUpperRightX - PortraitPageSize.HdrLowerLeftX, 50);
                }

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

                float[] addrLayout = new float[] { 50F, 355F, 135F };
                PdfPTable addrTable = PdfReports.CreateTable(addrLayout, 0);

                if (_pageNumber == 1) {

                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                    PdfReports.AddText2Table(addrTable, " ", _normalFont);

                    Paragraph p = PdfReports.GetAddressBlock(_groName, _groAdr1, _groAdr2, _groCSZ,
                        0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, _uspsFont);
                    PdfReports.AddText2Table(addrTable, p);

                    PdfReports.AddText2Table(addrTable, " ", _normalFont);

                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);

                }

                PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);

                PdfReports.AddTableNoSplit(document, this, addrTable);
                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(SqlDataReader dr, string statementDate, int pageNumber, string title, iTextSharp.text.Image imgLogo) {

            if (statementDate != null && statementDate.Length > 0) {
                _statementDate = statementDate;
            } else {
                _statementDate = "";
            }

            _groName = dr.GetString(dr.GetOrdinal("GrowerName"));
            _groAdr1 = dr.GetString(dr.GetOrdinal("GroAdr1"));
            _groAdr2 = dr.GetString(dr.GetOrdinal("GroAdr2"));
            _groCSZ = dr.GetString(dr.GetOrdinal("GroCSZ"));

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

