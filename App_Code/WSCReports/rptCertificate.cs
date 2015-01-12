using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Summary description for rptCertificate.
    /// </summary>
    public class rptCertificate {

        private const string MOD_NAME = "WSCReports.rptCertificate.";

        const float _primaryLeading = 13.5F;
        //const float _primaryLeftIndent = 53F;
        const float _primaryLeftIndent = 50F;
        const float _primaryRightIndent = 54F;

        private static float[] _bottomRetLayout = new float[] { _primaryLeftIndent, 270F - _primaryLeftIndent, 270F - _primaryRightIndent, _primaryRightIndent };   // total must be 540
        private static float[] _bottomPatLayout = new float[] 
        {
            _primaryLeftIndent, 
            270F - _primaryLeftIndent, 
            (310F - _primaryRightIndent)/2, 
            (230F - _primaryRightIndent)/2, 
            _primaryRightIndent 
        };   // total must be 540

        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 11F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 11F, Font.BOLD);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);

        private const string HEADER_DATE_FORMAT = "MMMM dd, yyyy";

        public static string ReportPackager(int cropYear, string equityType, DateTime certificateDate,
            string shid, string fromShid, string toShid, string fileName, string logoUrl, string pdfTempfolder,
            string sigName, string sigTitle, string sigImagePath) {

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
                    
                    if (equityType == "RET") {
						
						List<ListStatementPatRetainItem> stateList = WSCReportsExec.RptCertificateRetains(cropYear, shid, fromShid, toShid);

						if (stateList.Count > 0) {
							using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
								ReportBuilderRetain(stateList, cropYear, certificateDate, shid, fromShid, toShid, logoUrl, fs, sigName, sigTitle, sigImagePath);
							}
						} else {
							WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("No records matched your report criteria.");
							throw (warn);
						}

                    } else {

						List<ListStatementPatRetainItem> stateList = WSCReportsExec.RptCertificatePat(cropYear, shid, fromShid, toShid);

						if (stateList.Count > 0) {
							using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
								ReportBuilderPatronage(stateList, cropYear, certificateDate, shid, fromShid, toShid, logoUrl, fs, sigName, sigTitle, sigImagePath);
							}
						} else {
							WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("No records matched your report criteria.");
							throw (warn);
						}
                    }
                }
                catch (System.Exception ex) {

                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME, ex);
                    throw (wscEx);
                }

                return filePath;
            }
            catch (System.Exception ex) {

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME, ex);
                throw (wscEx);
            }
        }

        private static void ReportBuilderRetain(List<ListStatementPatRetainItem> stateList, 
			int cropYear, DateTime certificateDate, string shid, string fromShid, string toShid, string logoUrl, System.IO.FileStream fs,
            string sigName, string sigTitle, string sigImagePath) {

            const string METHOD_NAME = "ReportBuilderRetain: ";

            string SHID = "";
            string equityCropYear = "";
            string rptTitle = "";
            string certificateDateStr = certificateDate.ToString(HEADER_DATE_FORMAT);
            string qualifiedStr = "";
            float pdfYPos = 0;
            float pdfXPos = -4;

            Document document = null;
            PdfWriter writer = null;
            PdfPTable bottomTable = null;
            ColumnText ct = null;
            PdfImportedPage pdfImport = null;

            CertificateEvent pgEvent = null;

            try {

                if (stateList.Count > 0) {
                    qualifiedStr = (stateList[0].Qualified.ToLower() == "qualified" ? "Qualified" : "NonQualified");
                }

                iTextSharp.text.Image imgSignature = PdfReports.GetImage(sigImagePath, 228, 68, iTextSharp.text.Element.ALIGN_LEFT);

                foreach (ListStatementPatRetainItem state in stateList) {

                    if (rptTitle.Length == 0) {
                        rptTitle = qualifiedStr.ToUpper() + " UNIT RETAIN NOTICE\n" + "CROP YEAR " + cropYear.ToString();
                    }

                    SHID = state.SHID;
                    equityCropYear = state.EquityCropYear;

                    if (document == null) {

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        // Attach my override event handler(s)
                        pgEvent = new CertificateEvent();
                        pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, rptTitle, logoUrl);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                        // ================================================================================================================
                        // FROM PDF -- Pulls a background image out of a pdf and uses it as the under layer! Works BUT NEEDS ADJUSTMENT
                        // ================================================================================================================
                        string _certBorderPath = sigImagePath.Replace("CertificateSignature.gif", "CertBorders.pdf");
                        PdfReader rdrCertBorder = new PdfReader(_certBorderPath);
                        pdfImport = writer.GetImportedPage(rdrCertBorder, 1); // import only page 1.                        

                    } else {

                        pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, rptTitle, logoUrl);
                        document.NewPage();
                    }

                    writer.DirectContentUnder.AddTemplate(pdfImport, pdfXPos, pdfYPos);

                    // =======================================================
                    // Build Report
                    // =======================================================

                    // Fill report Body
                    ct = pgEvent.GetColumnObject();
                    BuildRetainBodyText(cropYear, qualifiedStr, ct);

                    // Add Bottom section -- Date, Amount, SigLine.
                    bottomTable = PdfReports.CreateTable(_bottomRetLayout, 0);

                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);
                    PdfReports.AddText2Table(bottomTable, "Dated: " + certificateDateStr, _normalFont, "left");
                    PdfReports.AddText2Table(bottomTable, "Amount: " + Convert.ToDecimal(state.EquityAmt).ToString("$#,##0.00"), _normalFont, "left");
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, _bottomRetLayout.Length);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, _bottomRetLayout.Length);

                    //----------------------------------
                    // Add Signature
                    //----------------------------------
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, 2);
                    PdfReports.AddImage2Table(bottomTable, imgSignature);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    // add signature info
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(bottomTable, sigName, _normalFont);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(bottomTable, sigTitle, _normalFont);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    ct.AddElement(bottomTable);
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

        private static void BuildRetainBodyText(int cropYear, string qualified, ColumnText ct) {

            const string METHOD_NAME = "BuildRetainBodyText";

            Paragraph p = null;

            try {

                p = new Paragraph(new Phrase(_primaryLeading, "The total amount set forth below has been retained and credited to your account for Crop Year " +
                cropYear.ToString() + " by THE WESTERN SUGAR COOPERATIVE pursuant to Article IX of the By-Laws.\n\n", _normalFont));
                p.IndentationLeft = _primaryLeftIndent;
                p.IndentationRight = _primaryRightIndent;
                ct.AddElement(p);

                p = new Paragraph(new Phrase(_primaryLeading, "This notice constitutes a " + qualified.ToLower() + " written notice of allocation as defined in Section 1388(d) of the " +
                    "Internal Revenue Code of 1954, as amended.  The amount hereof need not be included in your gross income for federal income tax purposes unless " +
                    "and until this unit retain is redeemed, and then only to the extent redeemed.  As provided in Article X of the By-Laws, unit retains may be paid, " +
                    "redeemed, or revolved in whole or in part at a time and in a manner determined by the Board of Directors.  The Board of Directors shall have " +
                    "complete discretion over all matters related to paying, redeeming, or revolving unit retains.\n\n", _normalFont));
                p.IndentationLeft = _primaryLeftIndent;
                p.IndentationRight = _primaryRightIndent;
                ct.AddElement(p);

                p = new Paragraph(new Phrase(_primaryLeading, "As stated in Article XI of the By-laws, no proposed assignment or transfer of common stock, membership agreements, patron preferred stock, " +
                    "patronge equities, or unit retains shall be binding upon the Cooperative without the consent of the Board of Directors, nor until it shall have been " +
                    "entered on the books of the Cooperative\n\n", _normalFont));
                p.IndentationLeft = _primaryLeftIndent;
                p.IndentationRight = _primaryRightIndent;
                ct.AddElement(p);

                p = new Paragraph(new Phrase("This notice has been executed as of the date set forth below by the undersigned duly authorized officer of THE WESTERN SUGAR COOPERATIVE.\n\n",
                    _normalFont));
                p.IndentationLeft = _primaryLeftIndent;
                p.IndentationRight = _primaryRightIndent;
                ct.AddElement(p);
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private static void BuildPatronageBodyText(int cropYear, string qualified, ColumnText ct) {

            const string METHOD_NAME = "BuildPatronageBodyText";

            Paragraph p = null;

            try {

                p = new Paragraph(new Phrase(_primaryLeading, "The amount set forth below has been allocated to your account for Crop Year " +
                cropYear.ToString() + " by THE WESTERN SUGAR COOPERATIVE pursuant to Article VIII of the By-Laws.\n\n", _normalFont));
                p.IndentationLeft = _primaryLeftIndent;
                p.IndentationRight = _primaryRightIndent;
                ct.AddElement(p);

                p = new Paragraph(new Phrase(_primaryLeading, "This notice constitutes a " + qualified.ToLower() + " written notice of allocation as defined in Section 1388(d) of the " +
                    "Internal Revenue Code of 1954, as amended.  The total amount of this patronage refund must be included in your gross income for federal income tax purposes " +
                    "whether distributed in cash or as equity.  As provided in Article X of the By-Laws, patronage equities may be paid, " +
                    "redeemed, or revolved in whole or in part at a time and in a manner determined by the Board of Directors.  The Board of Directors shall have " +
                    "complete discretion over all matters related to paying, redeeming, or revolving patronage equities.\n\n", _normalFont));
                p.IndentationLeft = _primaryLeftIndent;
                p.IndentationRight = _primaryRightIndent;
                ct.AddElement(p);

                p = new Paragraph(new Phrase(_primaryLeading, "As stated in Article XI of the By-laws, no proposed assignment or transfer of common stock, membership agreements, patron preferred stock, " +
                    "patronge equities, or unit retains shall be binding upon the Cooperative without the consent of the Board of Directors, nor until it shall have been " +
                    "entered on the books of the Cooperative\n\n", _normalFont));
                p.IndentationLeft = _primaryLeftIndent;
                p.IndentationRight = _primaryRightIndent;
                ct.AddElement(p);

                p = new Paragraph(new Phrase("This notice has been executed as of the date set forth below by the undersigned duly authorized officer of THE WESTERN SUGAR COOPERATIVE.\n\n",
                    _normalFont));
                p.IndentationLeft = _primaryLeftIndent;
                p.IndentationRight = _primaryRightIndent;
                ct.AddElement(p);
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private static void ReportBuilderPatronage(List<ListStatementPatRetainItem> stateList, 
			int cropYear, DateTime certificateDate, string shid, string fromShid, string toShid, string logoUrl, System.IO.FileStream fs,
            string sigName, string sigTitle, string sigImagePath) {

            const string METHOD_NAME = "ReportBuilderPatronage: ";

            string SHID = "";
            string equityCropYear = "";
            string rptTitle = "";
            string certificateDateStr = certificateDate.ToString(HEADER_DATE_FORMAT);
            string qualifiedStr = "";
            float pdfYPos = 0;
            float pdfXPos = -4;

            Document document = null;
            PdfWriter writer = null;
            PdfPTable bottomTable = null;
            ColumnText ct = null;
            PdfImportedPage pdfImport = null;

            CertificateEvent pgEvent = null;

            try {

                if (stateList.Count > 0) {
                    qualifiedStr = (stateList[0].Qualified.ToLower() == "qualified" ? "Qualified" : "NonQualified");
                }

                iTextSharp.text.Image imgSignature = PdfReports.GetImage(sigImagePath, 228, 68, iTextSharp.text.Element.ALIGN_LEFT);

                foreach (ListStatementPatRetainItem state in stateList) {

                    if (rptTitle.Length == 0) {
                        rptTitle = qualifiedStr.ToUpper() + " PATRONAGE REFUND NOTICE\n" + "CROP YEAR " + cropYear.ToString();
                    }

                    SHID = state.SHID;
                    equityCropYear = state.EquityCropYear;

                    if (document == null) {

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        // Attach my override event handler(s)
                        pgEvent = new CertificateEvent();
                        pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, rptTitle, logoUrl);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        // ** ROTATE ** ROTATE **  ---------------------------------------------------------------------------------------
                        //PdfReader reader = new PdfReader(...);
                        //for (int k = 1; k <= reader.getNumberOfPages(); ++k) {
                        //    reader.getPageN(k).put(PdfName.ROTATE, new PdfNumber(90));
                        //}
                        //PdfStamper stp = new PdfStamper(reader, ...);
                        //stp.close();
                        // ** ROTATE ** ROTATE **  ---------------------------------------------------------------------------------------
                        //string tmpFilePath = sigImagePath.Replace("CertificateSignature.gif", "WorkTemplate.pdf");
                        //PdfReader rdr = new PdfReader(tmpFilePath);                        
                        //rdr.GetPageN(1).Put(PdfName.ROTATE, new PdfNumber(90));

                        //Document docCopy = new Document(rdr.GetPageSizeWithRotation(1));
                        //PdfCopy pCopy = new PdfCopy(docCopy, new System.IO.FileStream(sigImagePath.Replace("CertificateSignature.gif", "WorkTemplate90.pdf"),
                        //    System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read));

                        //docCopy.Open();

                        //docCopy.AddCreationDate();
                        //docCopy.AddCreator("");
                        //docCopy.AddTitle("CertificateTemplate.pdf");

                        //pCopy.AddPage(pCopy.GetImportedPage(rdr, 1));
                        //docCopy.Close();

                        //using (System.IO.FileStream fsTmpOut = new FileStream(sigImagePath.Replace("CertificateSignature.gif", "WorkTemplate90.pdf"),
                        //    System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {

                        //    PdfStamper tmpStamp = new PdfStamper(rdr, fsTmpOut);
                        //    tmpStamp.Close();
                        //}

                        // ================================================================================================================
                        // FROM PDF -- Pulls a background image out of a pdf and uses it as the under layer! Works BUT NEEDS ADJUSTMENT
                        // ================================================================================================================
                        string _certBorderPath = sigImagePath.Replace("CertificateSignature.gif", "CertBorders.pdf");
                        PdfReader rdrCertBorder = new PdfReader(_certBorderPath);
                        pdfImport = writer.GetImportedPage(rdrCertBorder, 1); // import only page 1.                        

                        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        // ================================================================================================================
                        // FROM JPG -- Uses a jpg image as the under layer!!! This ROUGHLY WORKS -- NEEDS HELP !
                        // ================================================================================================================
                        //string _logo = sigImagePath.Replace("CertificateSignature.gif", "MyBorders.gif");

                        //iTextSharp.text.Image imgBack = PdfReports.GetImage(_logo, Convert.ToInt32(document.Right - document.RightMargin - document.LeftMargin),
                        //    Convert.ToInt32(document.Top - document.TopMargin - document.BottomMargin), iTextSharp.text.Element.ALIGN_CENTER);

                        //float imageYPos = document.Top - imgBack.Height;
                        //float imageXPos = 0;

                        //// To position an image  at (x,y) use addImage(image, image_width, 0, 0, image_height, x, y)
                        //writer.DirectContentUnder.AddImage(imgBack, imgBack.Width, 0, 0, imgBack.Height, imageXPos, imageYPos);

                        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    } else {

                        pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, rptTitle, logoUrl);
                        document.NewPage();
                    }

                    writer.DirectContentUnder.AddTemplate(pdfImport, pdfXPos, pdfYPos);

                    // =======================================================
                    // Build Report
                    // =======================================================

                    // Fill report Body
                    ct = pgEvent.GetColumnObject();
                    BuildPatronageBodyText(cropYear, qualifiedStr, ct);

                    // Add Bottom section -- Date, Amount, SigLine.
                    bottomTable = PdfReports.CreateTable(_bottomPatLayout, 0);

                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);
                    PdfReports.AddText2Table(bottomTable, "Dated: " + certificateDateStr, _normalFont, "left");
                    PdfReports.AddText2Table(bottomTable, "Total patronage refund: ", _normalFont, "left");
                    PdfReports.AddText2Table(bottomTable, Convert.ToDecimal(state.EquityAmt).ToString("$#,##0.00"), _normalFont, "right");
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(bottomTable, "Paid by check: ", _normalFont, "left");
                    PdfReports.AddText2Table(bottomTable, Convert.ToDecimal(state.PatInitPayment).ToString("$#,##0.00"), _normalFont, "right");
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(bottomTable, "Patronage equity: ", _normalFont, "left");
                    PdfReports.AddText2Table(bottomTable, Convert.ToDecimal(state.RedeemAmt).ToString("$#,##0.00"), _normalFont, "right");
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, _bottomRetLayout.Length);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, _bottomRetLayout.Length);
                    ct.AddElement(bottomTable);

                    //----------------------------------
                    // Add Signature
                    //----------------------------------
                    //  this is odd, but use the RET layout for sig line.
                    bottomTable = PdfReports.CreateTable(_bottomRetLayout, 0);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, 2);
                    PdfReports.AddImage2Table(bottomTable, imgSignature);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    // add signature info
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(bottomTable, sigName, _normalFont);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    PdfReports.AddText2Table(bottomTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(bottomTable, sigTitle, _normalFont);
                    PdfReports.AddText2Table(bottomTable, " ", _normalFont);

                    ct.AddElement(bottomTable);
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


    public class CertificateEvent : PdfPageEventHelper, ICustomPageEvent {

        private static float[] _logo2010Layout = new float[] { 77F, 77F, 309F, 77F };
        //const float _primaryLeftIndent = 53F;
        const float _primaryLeftIndent = 50F;
        const float _primaryRightIndent = 54F;

        Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        Font _normalFont = FontFactory.GetFont("HELVETICA", 11F, Font.NORMAL);
        Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        Font _labelFont = FontFactory.GetFont("HELVETICA", 11F, Font.BOLD);

        // This is the contentbyte object of the writer
        PdfContentByte _cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;
        ColumnText _ct = null;

        private bool _isDocumentClosing = false;
        private float _headerBottomYLine;
        private string _title = "";
        private string _logoUrl = "";

        private string _shid = "";
        private string _businessName = "";
        private string _memAddr1 = "";
        private string _memAddr2 = "";
        private string _memCSZ = "";

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document) {

            _bf = _normalFont.GetCalculatedBaseFont(false);
            _cb = writer.DirectContent;
            _ct = new ColumnText(_cb);
            base.OnOpenDocument(writer, document);
        }


        public override void OnStartPage(PdfWriter writer, Document document) {

            if (!_isDocumentClosing) {

                // ===========================================================================
                // Create header column -- in this report this is the page's column object
                // ===========================================================================
                _ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
                    PortraitPageSize.PgLeading, Element.ALIGN_CENTER);
                _ct.YLine = PortraitPageSize.HdrTopYLine;

                // =======================================================
                // Add Header: Logo 2010
                // =======================================================

                PdfPTable logoTable = PdfReports.CreateTable(_logo2010Layout, 0);

                // Only add actual logo on First Page of set
                Paragraph p = new Paragraph("", _normalFont);

                PdfReports.AddText2Table(logoTable, " ", _normalFont, _logo2010Layout.Length);

                //-----------------------------------------
                // Add and center the title
                //-----------------------------------------
                PdfReports.AddText2Table(logoTable, " ", _normalFont);
                PdfReports.AddText2Table(logoTable, _title, _titleFont, "center", 2);
                PdfReports.AddText2Table(logoTable, " ", _normalFont);
               
                PdfReports.AddTableNoSplit(document, this, logoTable);

                float[] addrLayout = new float[] { _primaryLeftIndent, 540F - _primaryLeftIndent - _primaryRightIndent, _primaryRightIndent };
                PdfPTable addrTable = PdfReports.CreateTable(addrLayout, 0);

                // Blank
                PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);
                PdfReports.AddTableNoSplit(document, this, addrTable);

                //-----------------------------------------------
                // Address block
                //-----------------------------------------------
                addrTable = PdfReports.CreateTable(addrLayout, 0);

                // SHID ONLY
                PdfReports.AddText2Table(addrTable, " ", _normalFont);
                PdfReports.AddText2Table(addrTable, "Shareholder ID#: " + _shid, _uspsFont, "right");
                PdfReports.AddText2Table(addrTable, " ", _normalFont);

                PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);

                // Modified address block: address only.
                PdfReports.AddText2Table(addrTable, " ", _normalFont);
                p = PdfReports.GetAddressBlock(_businessName, _memAddr1, _memAddr2, _memCSZ,
                    0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, _uspsFont);
                PdfReports.AddText2Table(addrTable, p);
                PdfReports.AddText2Table(addrTable, " ", _normalFont);

                PdfReports.AddText2Table(addrTable, " ", _uspsFont, addrLayout.Length);
                PdfReports.AddText2Table(addrTable, " ", _uspsFont, addrLayout.Length);
                PdfReports.AddText2Table(addrTable, " ", _uspsFont, addrLayout.Length);

                PdfReports.AddTableNoSplit(document, this, addrTable);
                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(string shid, string busName, string addr1, string addr2, string csz, string title, string logoUrl) {

            _shid = shid;
            _businessName = busName;
            _memAddr1 = addr1;
            _memAddr2 = addr2;
            _memCSZ = csz;

            _title = title;
            _logoUrl = logoUrl;
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

