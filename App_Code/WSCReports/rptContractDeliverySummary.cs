using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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
    /// Summary description for rptContractDeliverySummary.
    /// </summary>
    public class rptContractDeliverySummary {

        private const string MOD_NAME = "WSCReports.rptContractDeliverySummary.";
        private static float[] _primaryTableLayout = new float[] { 9.3F, 57.4F, 33.3F };

        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);
        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);

        public static string ReportPackager(int cropYear, int contractNumber, string fileName, string logoUrl, string pdfTempfolder) {

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

                    // Save the file.
                    using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                        ReportBuilder(cropYear, contractNumber, logoUrl, fs);
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

        public static void ReportBuilder(int cropYear, int contractNumber, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilder";
            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            ContractDeliverySummaryEvent pgEvent = null;
            iTextSharp.text.Image imgLogo = null;

            int contractID = 0;
            int stationNo = 0;
            int factoryNo = 0;

            int acresContracted = 0;
            int acresHarvested = 0;
            int acresLost = 0;
            int acresPlanted = 0;

            try {

                if (document == null) {

                    // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                    //  ***  US LETTER: 612 X 792  ***  // margins: L, R, T, B
                    //document = new Document(iTextSharp.text.PageSize.LETTER, 36, 36, 54, 72);
                    document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin, 
                        PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                    // we create a writer that listens to the document
                    // and directs a PDF-stream to a file				
                    writer = PdfWriter.GetInstance(document, fs);

                    imgLogo = PdfReports.GetImage(logoUrl, 127, 50, iTextSharp.text.Element.ALIGN_CENTER);

                    // Attach my override event handler(s)
                    pgEvent = new ContractDeliverySummaryEvent();
                    pgEvent.FillEvent(imgLogo, cropYear.ToString());
                    writer.PageEvent = pgEvent;

                    // Open the document
                    document.Open();                    
                }

                // =======================================================
                // Build Report
                // =======================================================

                int staExSugar = 0;
                int ftyExSugar = 0;
                decimal extractableSugarPerTon = 0;
                PdfPTable addrTable = null;                

                List<ContractGorwerLandownerItem> stateList = WSCReportsExec.ContractDeliverySummary1(cropYear, contractNumber);

                float[] addrLayout = new float[] { 9.3F, 67.6F, 23.1F };

                addrTable = PdfReports.CreateTable(addrLayout, 0);

                if (stateList.Count > 0) {

					ContractGorwerLandownerItem item = stateList[0];

                    PdfReports.AddText2Table(addrTable, "Contract", _normalFont);
                    PdfReports.AddText2Table(addrTable,
                        "Grower#: " + item.Gro_Address_Number, _normalFont);
                    PdfReports.AddText2Table(addrTable,
                        "Landowner#: " + item.Ldo_Address_Number, _normalFont);

                    PdfReports.AddText2Table(addrTable, item.Contract_Number, _normalFont);

                    Paragraph p = PdfReports.GetAddressBlock(item.Grower_Name,
                        item.Gro_Address_1,
                        item.Gro_Address_2,
                        item.Grower_City + ", " +
                        item.Grower_State + " " +
                        item.Grower_Zip,
                        0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, _uspsFont);
                    PdfReports.AddText2Table(addrTable, p);

                    p = PdfReports.GetAddressBlock(item.Landowner_Name,
                        item.Ldo_Address_1,
                        item.Ldo_Address_2,
                        item.Ldo_City + ", " +
                        item.Ldo_State + " " +
                        item.Ldo_Zip,
                        0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, _normalFont);
                    PdfReports.AddText2Table(addrTable, p);

                    PdfReports.AddText2Table(addrTable, " ", _normalFont, 3);
                    PdfReports.AddText2Table(addrTable, " ", _normalFont, 3);

                    PdfReports.AddTableNoSplit(document, pgEvent, addrTable);

                    acresContracted = item.Contract_Acres;
                    acresHarvested = item.Harvest_Acres;
                    acresPlanted = item.Planted_Acres;
                    acresLost = acresPlanted - acresHarvested;

                    contractID = item.cnt_contract_id;
                    stationNo = Convert.ToInt32(item.Station_Number);
                    factoryNo = Convert.ToInt32(item.Factory_Number);                            
                }

                table = PdfReports.CreateTable(_primaryTableLayout, 0);

                List<ContractDeliverySummary2Item> cntDelSumList = WSCReportsExec.ContractDeliverySummary2(contractID);

				if (cntDelSumList.Count > 0) {

					ContractDeliverySummary2Item cntdsItem = cntDelSumList[0];

                    PdfReports.AddText2Table(table, "Contract Tons: " + cntdsItem.ContractTons.ToString("#,###.0000"),
                        _normalFont, 3);
                    PdfReports.AddText2Table(table, "Contract % Sugar: " + cntdsItem.SugarPct.ToString("0.00") + "%",
                        _normalFont, 3);
                    PdfReports.AddText2Table(table, "Contract % SLM: " + cntdsItem.SLMPct.ToString("0.0000") + "%",
                        _normalFont, 3);

                    string tonsPerAcre = null;
                    if (acresHarvested == 0) {
                        tonsPerAcre = String.Format("{0:#,##0.00}", 0);
                    } else {
                        tonsPerAcre = (cntdsItem.ContractTons / acresHarvested).ToString("#,###.00");
                    }
                    PdfReports.AddText2Table(table, "Tons Per Acre: " + tonsPerAcre, _normalFont, 3);

                    PdfReports.AddText2Table(table, " ", _normalFont, 3);

                    extractableSugarPerTon = cntdsItem.ExtractableSugarPerTon;
                    PdfReports.AddText2Table(table, "Pounds Extractable Sugar Per Ton Contract: " +
                        extractableSugarPerTon.ToString("#,##0"),
                        _normalFont, 3);
                }


				int factoryExtSugarAvg, stationExtSugarAvg;

                WSCReportsExec.FactoryStationGetExtractSugarAvg(factoryNo, stationNo, cropYear, out factoryExtSugarAvg, out stationExtSugarAvg);

				if (stationExtSugarAvg != 0) { 
					PdfReports.AddText2Table(table, "Pounds Extractable Sugar Per Ton Receiving Station: " +
						stationExtSugarAvg.ToString("#,##0"), _normalFont, 3);
				}

				if (factoryExtSugarAvg != 0) {
					PdfReports.AddText2Table(table, "Pounds Extractable Sugar Per Ton Factory: " +
						factoryExtSugarAvg.ToString("#,##0"), _normalFont, 3);
				}

                string percentOfStation = String.Format("{0:##0.00}", 0);
                string percentOfFactory = String.Format("{0:##0.00}", 0);

				if (stationExtSugarAvg != 0) {
					percentOfStation = String.Format("{0:##0.00}", (extractableSugarPerTon / stationExtSugarAvg) * 100);
                }
				if (factoryExtSugarAvg != 0) {
					percentOfFactory = String.Format("{0:##0.00}", (extractableSugarPerTon / factoryExtSugarAvg) * 100);
                }

                PdfReports.AddText2Table(table, "Percent of Station: " +
                    percentOfStation + "%", _normalFont, 3);

                PdfReports.AddText2Table(table, "Percent of Factory: " +
                    percentOfFactory + "%", _normalFont, 3);

                PdfReports.AddText2Table(table, " ", _normalFont, 3);

                PdfReports.AddText2Table(table, "Acres Contracted: " + acresContracted.ToString("#,##0"),
                    _normalFont, 3);
                PdfReports.AddText2Table(table, "Acres Planted: " + acresPlanted.ToString("#,##0"),
                    _normalFont, 3);
                PdfReports.AddText2Table(table, "Acres Lost: " + acresLost.ToString("#,##0"),
                    _normalFont, 3);
                PdfReports.AddText2Table(table, "Acres Harvested: " + acresHarvested.ToString("#,##0"),
                    _normalFont, 3);

                PdfReports.AddTableNoSplit(document, pgEvent, table);

                // ======================================================
                // Close document
                // ======================================================
                if (document != null) {

                    pgEvent.IsDocumentClosing = true;
                    document.Close();
                    document = null;
                }
                if (addrTable == null) {
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

    internal class ContractDeliverySummaryEvent : PdfPageEventHelper, ICustomPageEvent {
        
        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _subTitleFont = FontFactory.GetFont("HELVETICA", 10F, Font.ITALIC);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 10F, Font.NORMAL);
        private static Font _subNormalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 10F, Font.BOLD);
        float[] _headerLayout = new float[] { 10F, 66.5F, 23.5F };

        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        ColumnText ct = null;
        // we will put the final number of pages in a template
        //PdfTemplate template;

        private float _headerBottomYLine;
        private bool _isDocumentClosing = false;
        private string _cropYear = "";
        private iTextSharp.text.Image _imgLogo = null;

        public override void OnOpenDocument(PdfWriter writer, Document document) {

            bf = _normalFont.GetCalculatedBaseFont(false);
            cb = writer.DirectContent;
            ct = new ColumnText(cb);
//            template = cb.CreateTemplate(PortraitPageSize.HdrUpperRightX - PortraitPageSize.HdrLowerLeftX, 50);
            base.OnOpenDocument(writer, document);
        }

        //public override void OnEndPage(PdfWriter writer, Document document) {

        //    String text = "Page " + writer.PageNumber.ToString();
        //    float len = bf.GetWidthPoint(text, 8);
        //    cb.BeginText();
        //    cb.SetFontAndSize(bf, 8);
        //    cb.SetTextMatrix(280, 30);
        //    cb.ShowText(text);
        //    cb.EndText();

        //    base.OnEndPage(writer, document);

        //    //int pageN = writer.PageNumber;
        //    ////string text = "Page " + pageN + " of ";
        //    //string text = "Page " + pageN;
        //    //float len = bf.GetWidthPoint(text, 8);
        //    //cb.BeginText();
        //    //cb.SetFontAndSize(bf, 8);
        //    //cb.SetTextMatrix(300, 30);
        //    //cb.ShowText(text);
        //    //cb.EndText();
        //    //cb.AddTemplate(template, 300 + len, 30);

        //    //base.OnEndPage(writer, document);
        //}

        // Used to supply end of document values to template.
        //public override void OnCloseDocument(PdfWriter writer, Document document) {

        //    // add template text to document here.
        //    template.BeginText();
        //    template.SetFontAndSize(bf, 8);
        //    template.ShowText((writer.PageNumber - 1).ToString());
        //    template.EndText();

        //    base.OnCloseDocument(writer, document);
        //}

        public override void OnStartPage(PdfWriter writer, Document document) {

            if (!_isDocumentClosing) {

                // ===========================================================================
                // Create header column -- in this report this is the page's column object
                // ===========================================================================
                ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
                    PortraitPageSize.PgLeading, Element.ALIGN_CENTER);
                ct.YLine = PortraitPageSize.HdrTopYLine;

                // We need a greyed (hidden) header having Trans# in the far left corner and Contract# in the far right corner.                    
                PdfPTable table = PdfReports.CreateTable(_headerLayout, 0);

                // Line 1
                PdfReports.AddText2Table(table, DateTime.Now.ToShortDateString(), _subNormalFont);

                PdfReports.AddText2Table(table, "Western Sugar Cooperative\n" +
                    "Contract/Delivery Summary\n" +
                    "Crop Year " + _cropYear, _titleFont, "center");

                PdfReports.AddImage2Table(table, _imgLogo);

                PdfReports.AddText2Table(table, " ", _normalFont, _headerLayout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, _headerLayout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, _headerLayout.Length);

                PdfReports.AddTableNoSplit(document, this, table);
                _headerBottomYLine = ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(iTextSharp.text.Image imgLogo, string cropYear) {
            _imgLogo = imgLogo;
            _cropYear = cropYear;
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



