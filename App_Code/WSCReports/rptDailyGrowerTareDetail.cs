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
    /// Summary description for rptDailyGrowerTareDetail.
    /// </summary>
    public class rptDailyGrowerTareDetail {

        private static Font _headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        private const string MOD_NAME = "WSCReports.rptDailyGrowerTareDetail.";
        private static float[] _primaryTableLayout = new float[] { 67.5F, 67.5F, 67.5F, 67.5F, 67.5F, 67.5F, 67.5F, 67.5F };
        private static float[] _tareTableLayout = new float[] { 60.0F, 60.0F, 60.0F, 60.0F, 60.0F, 60.0F, 60.0F, 60.0F, 60.0F };        

        private static string[] _sampleDetailHdrNames = { "Yard Card", "Sample", "% Sugar", "Unclean Wt.", "Clean Wt.", "% Tare", "High Tare", "Topping", "SLM" };
        private static string[] _truckDetailHdrNames = { "Yard Card", "Delivery Date", "Truck #", "Weight In", "Weight Out", "Dirt Weight", "Dirt Taken", "Net Weight" };

        public static string ReportPackager(int cropYear, int contractNumber, string deliveryDates, string fileName, string logoUrl, string pdfTempfolder) {

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
                        using (SqlDataReader drHdr = WSCReportsExec.GrowerDetailReportByDelivery(conn, contractNumber, cropYear, deliveryDates)) {
                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {

                                ReportBuilder(drHdr, cropYear, contractNumber, deliveryDates, fs);
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

        public static void ReportBuilder(SqlDataReader drHdr, int cropYear, int contractNumber, string deliveryDates, FileStream fs) {

            const string METHOD_NAME = "ReportBuilder";
            Document document = null;
            PdfWriter writer = null;

            DailyGrowerTareDetailEvent pgEvent = null;

            int resetFlag = 0;
            string dates = null;
            int contractID = 0;
            string factoryNo = "";
            string firstDeliveryDate = null;
            string nextDeliveryDate = null;
            string lastDeliveryDate = null;
            string busName = "";
            string address1 = "";
            string address2 = "";
            string CSZ = "";

            string rptTitle = "Western Sugar Cooperative\nDaily Grower Tare Detail Report";

            try {

                if (drHdr.Read()) {

                    contractID = drHdr.GetInt32(drHdr.GetOrdinal("ContractID"));
                    factoryNo = drHdr.GetString(drHdr.GetOrdinal("Delivery_Factory_No"));
                    firstDeliveryDate = drHdr.GetString(drHdr.GetOrdinal("Delivery_Date"));
                    nextDeliveryDate = firstDeliveryDate;

                } else {
                    // Warn that we have no data.
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("No records matched your report criteria.");
                    throw (warn);
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    // Header Address Info
                    if (contractID > 0) {

                        using (SqlDataReader drAddr = WSCReportsExec.GrowerDetailReportAddr(conn, contractID)) {

                            if (drAddr.Read()) {

                                busName = drAddr.GetString(drAddr.GetOrdinal("Business_Name"));
                                address1 = drAddr.GetString(drAddr.GetOrdinal("Address_1"));
                                address2 = drAddr.GetString(drAddr.GetOrdinal("Address_2"));
                                CSZ = drAddr.GetString(drAddr.GetOrdinal("City")) + ", " +
                                    drAddr.GetString(drAddr.GetOrdinal("State")) + " " +
                                    drAddr.GetString(drAddr.GetOrdinal("Zip"));
                            }
                        }
                    }

                    // Sample / Tare information
                    // I use nextDeliveryDate as a trick to allow reading drHdr.  Initially,
                    // nextDeliveryDate is loaded and we skip reading drHdr.  Subsequently,
                    // the bottom of each iteration will null nextDeliveryDate and force reading drHdr.
                    int loadCount = 0;

                    while (nextDeliveryDate != null || drHdr.Read()) {

                        if (document == null) {

                            // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                            //  ***  US LETTER: 612 X 792  ***
                            document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                                PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                            // we create a writer that listens to the document
                            // and directs a PDF-stream to a file				
                            writer = PdfWriter.GetInstance(document, fs);

                            // Attach my override event handler(s)
                            pgEvent = new DailyGrowerTareDetailEvent();
                            pgEvent.FillEvent(contractNumber, busName, factoryNo, address1, address2, CSZ, resetFlag, rptTitle);
                            writer.PageEvent = pgEvent;

                            // Open the document
                            document.Open();
                        }

                        // load the delivery date.
                        nextDeliveryDate = drHdr.GetString(drHdr.GetOrdinal("Delivery_Date"));

                        // Acquire the sample details for the first station on a given day
                        if (lastDeliveryDate != nextDeliveryDate) {

                            AddSampleHdr(ref document, drHdr, cropYear, pgEvent);
                            lastDeliveryDate = nextDeliveryDate;
                            using (SqlDataReader drSamples = WSCReportsExec.GrowerDetailReportTares(conn, contractID, nextDeliveryDate)) {
                                AddSampleDetail(ref document, drSamples, pgEvent);
                            }
                        }

                        loadCount = 0;

                        // Display Truck information for each station
                        // on the first delivery day.
                        if (nextDeliveryDate == firstDeliveryDate) {

                            // Get the truck data.
                            SqlParameter[] spParams = null;
                            using (SqlDataReader drTrucks = WSCReportsExec.GrowerDetailReportASH(conn, contractID,
                                      drHdr.GetInt32(drHdr.GetOrdinal("Delivery_Station_ID")),
                                      firstDeliveryDate, ref spParams)) {

                                AddTruckDetail(ref document, drTrucks, pgEvent);

                                drTrucks.Close();
                                loadCount = Convert.ToInt32(spParams[3].Value);
                                if (loadCount > 0) {

                                    AddTruckTotals(ref document, loadCount.ToString(),
                                        Convert.ToInt32(spParams[4].Value).ToString("#,##0"),
                                        Convert.ToInt32(spParams[5].Value).ToString("#,##0"), pgEvent);
                                }
                            }
                        }

                        // clear date to force read in top of loop
                        nextDeliveryDate = null;

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

        private static void AddSampleHdr(ref Document document, SqlDataReader drHdr, int cropYear, DailyGrowerTareDetailEvent pgEvent) {

            PdfPTable table = PdfReports.CreateTable(_primaryTableLayout, 1);

            PdfReports.AddText2Table(table, "Delivery Date", _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetString(drHdr.GetOrdinal("Delivery_Date")), _normalFont);
            PdfReports.AddText2Table(table, "1st Net Lbs", _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetInt32(drHdr.GetOrdinal("First_Net_Pounds")).ToString(), _normalFont);
            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, "% Sugar", _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetDecimal(drHdr.GetOrdinal("Sugar_Content")).ToString("0.00"), _normalFont);
            PdfReports.AddText2Table(table, " ", _normalFont);

            PdfReports.AddText2Table(table, "Station No", _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetInt32(drHdr.GetOrdinal("Delivery_Station_No")).ToString(), _normalFont);
            PdfReports.AddText2Table(table, "Tare Lbs", _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetInt32(drHdr.GetOrdinal("Tare_Pounds")).ToString(), _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetDecimal(drHdr.GetOrdinal("Tare")).ToString("0.00"), _normalFont);
            PdfReports.AddText2Table(table, "SLM", _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetDecimal(drHdr.GetOrdinal("SLM_Pct")).ToString("0.0000"), _normalFont);
            PdfReports.AddText2Table(table, " ", _normalFont);

            PdfReports.AddText2Table(table, "Station Name", _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetString(drHdr.GetOrdinal("Delivery_Station_Name")), _normalFont);
            PdfReports.AddText2Table(table, "Final Net Lbs", _normalFont);
            PdfReports.AddText2Table(table, drHdr.GetInt32(drHdr.GetOrdinal("Final_Net_Pounds")).ToString(), _normalFont);
            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, "Lbs Extractable Sugar/Ton", _normalFont, 2);
            PdfReports.AddText2Table(table, drHdr.GetInt32(drHdr.GetOrdinal("ExSugarPerTon")).ToString(), _normalFont);
            PdfReports.AddText2Table(table, " ", _labelFont, _primaryTableLayout.Length);
            PdfReports.AddTableNoSplit(document, pgEvent, table);
        }

        public static void AddSampleDetail(ref Document document, SqlDataReader drSamples, DailyGrowerTareDetailEvent pgEvent) {

            const string METHOD_NAME = "AddSampleDetail";
            string yardCard = null;
            PdfPTable table = null;

            try {

                pgEvent.HeaderNameList = _sampleDetailHdrNames;

                while (drSamples.Read()) {

                    table = PdfReports.CreateTable(_tareTableLayout, 1);

                    // On first pass, add the header labels here.
                    if (yardCard == null) {
                        PdfReports.FillHeaderLabels(ref table, _sampleDetailHdrNames, _labelFont);
                    } else {
                        // Now let the PDF Page event handler take care of adding column labels on page breaks.
                        pgEvent.HeaderTableLayout = _tareTableLayout;
                    }

                    yardCard = drSamples.GetString(drSamples.GetOrdinal("Yard_Card_No"));
                    PdfReports.AddText2Table(table, yardCard, _normalFont, "center");
                    PdfReports.AddText2Table(table, drSamples.GetInt32(drSamples.GetOrdinal("SampleNo")).ToString("#"), _normalFont, "center");
                    PdfReports.AddText2Table(table, drSamples.GetDecimal(drSamples.GetOrdinal("Sugar_Content")).ToString("0.00"), _normalFont, "center");
                    PdfReports.AddText2Table(table, drSamples.GetDecimal(drSamples.GetOrdinal("Gross_Weight")).ToString("#,##0.0"), _normalFont, "right");
                    PdfReports.AddText2Table(table, drSamples.GetDecimal(drSamples.GetOrdinal("Clean_Weight")).ToString("#,##0.0"), _normalFont, "right");
                    PdfReports.AddText2Table(table, drSamples.GetDecimal(drSamples.GetOrdinal("Tare")).ToString("0.00"), _normalFont, "center");
                    PdfReports.AddText2Table(table, drSamples.GetString(drSamples.GetOrdinal("High_Tare")), _normalFont, "center");
                    PdfReports.AddText2Table(table, drSamples.GetInt32(drSamples.GetOrdinal("Sample_Quality")).ToString(), _normalFont, "center");
                    PdfReports.AddText2Table(table, drSamples.GetDecimal(drSamples.GetOrdinal("SLM_Pct")).ToString("0.0000"), _normalFont, "center");

                    PdfReports.AddTableNoSplit(document, pgEvent, table);
                }

                pgEvent.HeaderNameList = null;
                pgEvent.HeaderTableLayout = null;

                // Follow the table with a blank line
                table = PdfReports.CreateTable(_tareTableLayout, 1);
                PdfReports.AddText2Table(table, " ", _normalFont, _tareTableLayout.Length);

                PdfReports.AddTableNoSplit(document, pgEvent, table);
            }
            catch (Exception ex) {

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME, ex);
                throw (wscEx);
            }
        }

        public static void AddTruckTotals(ref Document document, string loadCount, string dirtWeight, string netWeight, DailyGrowerTareDetailEvent pgEvent) {

            const string METHOD_NAME = "AddTruckTotals";

            try {

                PdfPTable table = PdfReports.CreateTable(_primaryTableLayout, 1);

                // Let the PDF Page event handler take care of adding column labels on page breaks.
                pgEvent.HeaderNameList = _truckDetailHdrNames;
                pgEvent.HeaderTableLayout = _primaryTableLayout;

                PdfReports.AddText2Table(table, " ", _normalFont);
                PdfReports.AddText2Table(table, "Load Count:", _labelFont);
                PdfReports.AddText2Table(table, loadCount, _normalFont);
                PdfReports.AddText2Table(table, " ", _normalFont);
                PdfReports.AddText2Table(table, " ", _normalFont);
                PdfReports.AddText2Table(table, dirtWeight, _normalFont, "right");
                PdfReports.AddText2Table(table, " ", _normalFont);
                PdfReports.AddText2Table(table, netWeight, _normalFont, "right");
                PdfReports.AddText2Table(table, " ", _normalFont, _primaryTableLayout.Length);

                PdfReports.AddTableNoSplit(document, pgEvent, table);
                pgEvent.HeaderNameList = null;
                pgEvent.HeaderTableLayout = null;

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME, ex);
                throw (wscEx);
            }
        }

        public static void AddTruckDetail(ref Document document, SqlDataReader drTrucks, DailyGrowerTareDetailEvent pgEvent) {

            const string METHOD_NAME = "AddTruckDetail";
            string yardCard = null;

            try {

                pgEvent.HeaderNameList = _truckDetailHdrNames;
                //PdfPTable table = PdfReports.CreateTable(_primaryTableLayout, 1);

                int iYardCard = drTrucks.GetOrdinal("Yard_Card_Sequence_Number");
                int iDeliveryDate = drTrucks.GetOrdinal("Delivery_Date");
                int iTruckNumber = drTrucks.GetOrdinal("Truck_Number");
                int iWeightIn = drTrucks.GetOrdinal("Weight_In");
                int iWeightOut = drTrucks.GetOrdinal("Weight_Out");
                int iDirtWt = drTrucks.GetOrdinal("Dirt_Weight");
                int iDirtTaken = drTrucks.GetOrdinal("Dirt_Taken");
                int iNetWt = drTrucks.GetOrdinal("Net_Weight");

                while (drTrucks.Read()) {

                    PdfPTable table = PdfReports.CreateTable(_primaryTableLayout, 1);

                    // On first pass, add the header labels here.
                    if (yardCard == null) {
                        PdfReports.FillHeaderLabels(ref table, _truckDetailHdrNames, _labelFont);
                    } else {
                        // Now let the PDF Page event handler take care of adding column labels on page breaks.                        
                        pgEvent.HeaderTableLayout = _primaryTableLayout;
                    }

                    yardCard = drTrucks.GetString(iYardCard);
                    PdfReports.AddText2Table(table, yardCard, _normalFont, "center");
                    PdfReports.AddText2Table(table, drTrucks.GetString(iDeliveryDate), _normalFont, "center");
                    PdfReports.AddText2Table(table, drTrucks.GetString(iTruckNumber), _normalFont, "center");
                    PdfReports.AddText2Table(table, drTrucks.GetInt32(iWeightIn).ToString("#,##0"), _normalFont, "right");
                    PdfReports.AddText2Table(table, drTrucks.GetInt32(iWeightOut).ToString("#,##0"), _normalFont, "right");
                    PdfReports.AddText2Table(table, drTrucks.GetInt32(iDirtWt).ToString("#,##0"), _normalFont, "right");
                    PdfReports.AddText2Table(table, drTrucks.GetString(iDirtTaken), _normalFont, "center");
                    PdfReports.AddText2Table(table, drTrucks.GetInt32(iNetWt).ToString("#,##0"), _normalFont, "right");

                    PdfReports.AddTableNoSplit(document, pgEvent, table);
                }

                //PdfReports.AddTableNoSplit(document, pgEvent, table);

                pgEvent.HeaderNameList = null;
                pgEvent.HeaderTableLayout = null;
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME, ex);
                throw (wscEx);
            }
        }
    }

    public class DailyGrowerTareDetailEvent : PdfPageEventHelper, ICustomPageEvent {

        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

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
        private string[] _hdrNameList = null;
        private float[] _hdrTableLayout = null;
        private bool _isSummary = false;

        private int _contractNumber = 0;
        private string _busName = "";
        private string _factoryNo = "";
        private string _address1 = "";
        private string _address2 = "";
        private string _csz = "";

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
            string text = "Page " + pageN + " of ";
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
            _lastPageNumber = _pageNumber;

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
                // Add Report Header
                // =======================================================
                float[] headerLayout = new float[] {40F, 120F, 220F, 160F};
                PdfPTable table = PdfReports.CreateTable(headerLayout, 1);

                if (_isSummary) {

                    PdfReports.AddText2Table(table, "Process Summary", _titleFont, "center", headerLayout.Length);
                    PdfReports.AddText2Table(table, " ", _normalFont, headerLayout.Length);
                    PdfReports.AddText2Table(table, " ", _normalFont, headerLayout.Length);
                    PdfReports.AddTableNoSplit(document, this, table);

                } else {

                PdfReports.AddText2Table(table, " ", _normalFont, headerLayout.Length);
                    PdfReports.AddText2Table(table, _contractNumber.ToString(), _normalFont);
                    PdfReports.AddText2Table(table, " ", _normalFont);
                    PdfReports.AddText2Table(table, _title, _titleFont, "center");
                    PdfReports.AddText2Table(table, " ", _normalFont);

                    PdfReports.AddTableNoSplit(document, this, table);

                    float[] addrLayout = new float[] {50F, 270F, 220F};
                    PdfPTable addrTable = PdfReports.CreateTable(addrLayout, 0);
                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);

                    if (_pageNumber == 1) {

                        PdfReports.AddText2Table(addrTable, _contractNumber.ToString(), _normalFont);

                        // Left column: Full Mailling Address
                        Paragraph p = PdfReports.GetAddressBlock(_busName, _address1, _address2,
                            _csz, 0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, _uspsFont);
                        PdfReports.AddText2Table(addrTable, p);

                        // Right column: shid / tax id				
                        PdfReports.AddText2Table(addrTable, "Factory: " + _factoryNo, _normalFont, "right");

                    }
                    else {

                        // Left column
                        PdfReports.AddText2Table(addrTable, _contractNumber.ToString(), _normalFont);
                        PdfReports.AddText2Table(addrTable, _busName, _normalFont);

                        // Right column
                        PdfReports.AddText2Table(addrTable, "Factory: " + _factoryNo, _normalFont, "right");
                    }
                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                    PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);

                    PdfReports.AddTableNoSplit(document, this, addrTable);

                    if (_hdrTableLayout != null) {
                        PdfPTable hdrTab = PdfReports.CreateTable(_hdrTableLayout, 0);
                        PdfReports.FillHeaderLabels(ref hdrTab, _hdrNameList, _labelFont);
                        PdfReports.AddTableNoSplit(document, this, hdrTab);
                    }
                }

                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(int contractNumber, string busName, string factoryNo,
            string address1, string address2, string CSZ, int pageNumber, string title) {

            _contractNumber = contractNumber;
            _busName = busName;
            _factoryNo = factoryNo;
            _address1 = address1;
            _address2 = address2;
            _csz = CSZ;
            _title = title;
            _pageNumber = pageNumber;
        }

        public string[] HeaderNameList {
            get { return _hdrNameList; }
            set { _hdrNameList = value; }
        }
        public float[] HeaderTableLayout {
            get { return _hdrTableLayout; }
            set { _hdrTableLayout = value; }
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
        public bool IsSummary {
            set { 
                _isSummary = value;
                _pageNumber = 0;
            }            
        }
    }
}

