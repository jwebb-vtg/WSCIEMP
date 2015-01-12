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
using System.Xml;
using System.Text;
using WSCData;
using PdfHelper;

namespace WSCReports {

    /// <summary>
    /// Summary description for rptLandownerLetter.
    /// </summary>
    public class rptLandownerLetter {

        private const string MOD_NAME = "WSCReports.rptLandownerLetter.";

        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _subTitleFont = FontFactory.GetFont("HELVETICA", 11F, Font.ITALIC);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 11F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 11F, Font.BOLD);

        public static string ReportPackager(int cropYear, DateTime letterDate, DateTime deadlineDate,
            string factoryList, string stationList, string contractList, string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "ReportPackager";
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

                        using (SqlDataReader dr = WSCPayment.GetLandownerLetter(conn, cropYear, factoryList, stationList, contractList)) {

                            //rptLandownerLetterHelper ldoHelper = new rptLandownerLetterHelper();
                            using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                                //ldoHelper.ReportBuilder(page, dr, cropYear, letterDate, deadlineDate, fs);
                                ReportBuilder(dr, cropYear, letterDate, deadlineDate, logoUrl, fs);
                            }
                        }
                    }
                }
                catch (System.Exception ex) {
                    string errMsg = "cropYear: " + cropYear.ToString();

                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + errMsg, ex);
                    throw (wscEx);
                }

                return filePath;
            }
            catch (System.Exception ex) {
                string errMsg = "cropYear: " + cropYear.ToString();

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + errMsg, ex);
                throw (wscEx);
            }
        }

        public static void ReportBuilder(SqlDataReader dr, int cropYear, DateTime letterDate, DateTime deadlineDate, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilder";
            const float primaryLeading = 13.5F;
            const float primaryLeftIndent = 20F;
            string strLetterDate = letterDate.ToString("MMMM dd, yyyy");
            string strDeadlineDate = deadlineDate.ToString("MMMM dd, yyyy");
            string LdoBusinessName = "";
            string tmpStr = "";
            string tmpStr2 = "";

            Document document = null;
            PdfWriter writer = null;
            LandownerLetterEvent pgEvent = null;
            ColumnText ct = null;
            iTextSharp.text.Image imgLogo = null;

            try {

                while (dr.Read()) {

                    LdoBusinessName = dr["LdoBusinessName"].ToString();
                    string LdoAddressLine1 = dr["LdoAddressLine1"].ToString();
                    string LdoAddressLine2 = dr["LdoAddressLine2"].ToString();
                    string LdoCityStateZip = dr["LdoCityStateZip"].ToString();

                    if (document == null) {

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        imgLogo = PdfReports.GetImage(logoUrl, 127, 50, iTextSharp.text.Element.ALIGN_CENTER);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        // Attach my override event handler(s)
                        pgEvent = new LandownerLetterEvent();
                        pgEvent.FillEvent(imgLogo, strLetterDate, LdoBusinessName, LdoAddressLine1, LdoAddressLine2, LdoCityStateZip);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                    } else {
                        pgEvent.FillEvent(imgLogo, strLetterDate, LdoBusinessName, LdoAddressLine1, LdoAddressLine2, LdoCityStateZip);
                        document.NewPage();
                    }

                    ct = pgEvent.GetColumnObject();                   

                    Paragraph p = new Paragraph(primaryLeading, "Your tenant, " + dr["GroBusinessName"].ToString() +
                    ", has indicated that the proceeds for sugar beets grown on your land in\n" + 
                    cropYear.ToString() + " are to be split and you will be receiving " + dr.GetDecimal(dr.GetOrdinal("LdoSplitPercent")).ToString("##.0##") +
                    "% of the proceeds. Since Western Sugar Cooperative no\n" + 
                    "longer requires signatures for the second payees, this letter has been sent for your verification.\n\n", _normalFont);

                    ct.AddElement(p);                    

                    p = new Paragraph(new Phrase(primaryLeading, "Your check will be made payable as follows:", _normalFont));
                    ct.AddElement(p);
                    p = new Paragraph(primaryLeading, dr["ldoPayeeDescription"].ToString(), _normalFont);
                    p.IndentationLeft = primaryLeftIndent;
                    ct.AddElement(p);

                    p = new Paragraph(primaryLeading, "If you have a lien on the beets, the lien holder’s name should be included above.\n\n" +
                    "In addition, the following options were selected by your tenant.", _normalFont);
                    ct.AddElement(p);

                    decimal ldoChemPct = dr.GetDecimal(dr.GetOrdinal("LdoChemicalSplitPct"));
                    if (dr.GetBoolean(dr.GetOrdinal("IsSplitChemical")) && ldoChemPct != 0) {                        
                        tmpStr = ldoChemPct.ToString("##.0##") + "%";
                    } else {
                        tmpStr = "No";
                    }

                    decimal ldoRetainPct = dr.GetDecimal(dr.GetOrdinal("LdoSplitPercent"));
                    if (dr.GetBoolean(dr.GetOrdinal("IsSplitRetain")) && ldoRetainPct != 0) {
                        tmpStr2 = ldoRetainPct.ToString("##.0##") + "% of the";
                    } else {
                        tmpStr2 = "No";
                    }

                    p = new Paragraph(primaryLeading, tmpStr2 + " Unit Retains on this contract will be deducted from your proceeds.\n" +
                    tmpStr + " seed & chemical receivables charged to this contract will be deducted from your proceeds.", _normalFont);
                    p.IndentationLeft = primaryLeftIndent;
                    ct.AddElement(p);

                    p = new Paragraph(primaryLeading, "If the above information is correct, no response is needed.\n\n", _normalFont);
                    ct.AddElement(p);

                    p = new Paragraph(primaryLeading, "If any of the above information is not correct, please contact your tenant’s agriculturist, " + 
                    dr["AgriculturistName"].ToString() +
                    " at\n" + WSCIEMP.Common.CodeLib.FormatPhoneNumber2Display(dr["AgriculturistPhone"].ToString()) +
                    " by " + strDeadlineDate + ", or mail the corrections to:\n\n", _normalFont);
                    ct.AddElement(p);

                    p = new Paragraph(12F, "Western Sugar Cooperative\n" +
                    "Attn: Marty Smith\n" +
                    "1221 8th Avenue Unit E\n" +
					"Greeley, CO  80631\n\n", _normalFont);
                    p.IndentationLeft = primaryLeftIndent;
                    ct.AddElement(p);

                    p = new Paragraph(primaryLeading, "Sincerely,\n\n" +
                    "Marty Smith\n" +
                    "Beet Accounting Manager\n\n" + 
                    "Ref: " + dr["ContractNo"].ToString(), _normalFont);
                    ct.AddElement(p);

                    ct.Go(false);

                }
                // ======================================================
                // Close document
                // ======================================================
                if (document != null) {

                    pgEvent.IsDocumentClosing = true;
                    document.Close();
                    document = null;
                }
                if (writer == null || String.IsNullOrEmpty(LdoBusinessName)) {
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

    internal class LandownerLetterEvent : PdfPageEventHelper, ICustomPageEvent {

        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _subTitleFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 11F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 11F, Font.BOLD);

        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        ColumnText ct = null;
        // we will put the final number of pages in a template

        private float _headerBottomYLine;
        private bool _isDocumentClosing = false;

        private iTextSharp.text.Image _imgLogo = null;
        private string _statementDate = "";
        string _ldoBusinessName = "";
        string _ldoAddressLine1 = "";
        string _ldoAddressLine2 = "";
        string _ldoCityStateZip = "";
        
        public override void OnOpenDocument(PdfWriter writer, Document document) {

            bf = _normalFont.GetCalculatedBaseFont(false);
            cb = writer.DirectContent;
            ct = new ColumnText(cb);
            base.OnOpenDocument(writer, document);
        }

        public override void OnStartPage(PdfWriter writer, Document document) {

            if (!_isDocumentClosing) {

                // ===========================================================================
                // Create header column -- in this report this is the page's column object
                // ===========================================================================
                ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
                    PortraitPageSize.PgLeading, Element.ALIGN_CENTER);
                ct.YLine = PortraitPageSize.HdrTopYLine;

                //float[] layout = new float[] { 403F, 137F };
                float[] layout = new float[] { 371F, 137F };
                PdfPTable table = PdfReports.CreateTable(layout, 0);

                // Line 1
                PdfReports.AddText2Table(table, " ", _normalFont);
                PdfReports.AddImage2Table(table, _imgLogo);

                // Line 2
                PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont);
                PdfReports.AddText2Table(table, _statementDate, _subTitleFont, "center");
                PdfReports.AddTableNoSplit(document, this, table);

                // Blank line: manuall adjust leading to get proper vertical spacing
                Paragraph p = new Paragraph(16F, " ", _normalFont);
                ct.AddElement(p);

                // Address block
                float adrLeading = 12.5F;
                float adrLeftIndent = 50F;

                if (_ldoBusinessName.Length > 0) {
                    p = new Paragraph(adrLeading, _ldoBusinessName + " \n", _normalFont);
                    p.IndentationLeft = adrLeftIndent;
                    ct.AddElement(p);
                }
                if (_ldoAddressLine1.Length > 0) {
                    p = new Paragraph(adrLeading, _ldoAddressLine1 + " \n", _normalFont);
                    p.IndentationLeft = adrLeftIndent;
                    ct.AddElement(p);
                }
                if (_ldoAddressLine2.Length > 0) {
                    p = new Paragraph(adrLeading, _ldoAddressLine2 + " \n", _normalFont);
                    p.IndentationLeft = adrLeftIndent;
                    ct.AddElement(p);
                }
                if (_ldoCityStateZip.Length > 0) {
                    p = new Paragraph(adrLeading, _ldoCityStateZip + " \n", _normalFont);
                    p.IndentationLeft = adrLeftIndent;
                    ct.AddElement(p);
                }

                p = new Paragraph(50F, " ", _normalFont);
                ct.AddElement(p);
                ct.Go(false);

                _headerBottomYLine = ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(iTextSharp.text.Image imgLogo, string statementDate, string ldoBusinessName, string ldoAddressLine1, string ldoAddressLine2, string ldoCityStateZip) {

            _imgLogo = imgLogo;
            _statementDate = statementDate;
            _ldoBusinessName = ldoBusinessName;
            _ldoAddressLine1 = ldoAddressLine1;
            _ldoAddressLine2 = ldoAddressLine2;
            _ldoCityStateZip = ldoCityStateZip;
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
