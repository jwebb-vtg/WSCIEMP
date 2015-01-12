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
    /// Summary description for rptStatementPatronageRetain.
    /// </summary>
    public class rptStatementPatronageRetain {

        private const string MOD_NAME = "WSCReports.rptStatementPatronageRetain.";
        private static float[] _primaryTableLayout = new float[] { 120F, 140F, 140F, 140F };        // total must be 540

        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 10F, Font.NORMAL);
        private static Font _normalItalicFont = FontFactory.GetFont("HELVETICA", 10F, Font.ITALIC);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 10F, Font.BOLD);
        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);

        private const string HEADER_DATE_FORMAT = "MMMM dd, yyyy";

        public static string ReportPackager(int cropYear, string groupType, string paymentType, string statementDate,
            string shid, string fromShid, string toShid, string fileName, string logoUrl, string pdfTempfolder) {

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

                    using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {

                        if (groupType == "RET") {

                            // ------------------------------------------------------------
                            // only one option here, we're reporting on Retain Redeems
                            // ------------------------------------------------------------
                            ReportBuilderRetainRedeems(cropYear, statementDate, shid, fromShid, toShid, logoUrl, fs);

                        } else {

                            // ------------------------------------------------------------
                            // Otherwise we're dealing with Patronage or Patronage Redeems.
                            // ------------------------------------------------------------
                            if (paymentType == "Patronage") {

                                //---------------------
                                // Patronage
                                //---------------------
                                ReportBuilderPatronage(cropYear, statementDate, shid, fromShid, toShid, logoUrl, fs);

                            } else {

                                //---------------------
                                // Patronage Redeems
                                //---------------------
                                ReportBuilderPatronageRedeems(cropYear, statementDate, shid, fromShid, toShid, logoUrl, fs);
                            }
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

        private static void ReportBuilderRetainRedeems(int cropYear, string paymentDate, string shid, string fromShid, string toShid, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilderRetainRedeems: ";

            string SHID = "";
            string equityCropYear = "";
            string lastSHID = "";
            string lastEquityCropYear = "";
            decimal totalCheckAmount = 0;
            string rptTitle = "";
            string statementDate = Convert.ToDateTime(paymentDate).ToString(HEADER_DATE_FORMAT);

            Document document = null;
            PdfWriter writer = null;
            PdfPTable detailTable = null;
            iTextSharp.text.Image imgLogo = null;
            StatementPatronageRetainEvent pgEvent = null;

            try {

                List<ListStatementPatRetainItem> stateList = WSCReportsExec.RptStatementRetainRedeem(cropYear, shid, fromShid, toShid, paymentDate);

                for (int i = 0; i < stateList.Count; i++) {

                    ListStatementPatRetainItem state = stateList[i];

                    if (rptTitle.Length == 0) {
                        rptTitle = "Unit Retain -- " + state.Qualified + " -- " + statementDate;
                    }

                    SHID = state.SHID;
                    equityCropYear = state.EquityCropYear;                    

                    if (document == null) {

                        // Create the detail table
                        detailTable = PdfReports.CreateTable(_primaryTableLayout, 0);
                        AddRetainRedeemDetailHeader(detailTable);

                        lastSHID = state.SHID;
                        totalCheckAmount = CalcRedeemCheck(stateList, i);

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        imgLogo = PdfReports.GetImage(logoUrl, 77, 45, iTextSharp.text.Element.ALIGN_CENTER);

                        // Attach my override event handler(s)
                        pgEvent = new StatementPatronageRetainEvent();
                        pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, totalCheckAmount, 0, rptTitle, imgLogo);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                    } else {

                        if (lastSHID != SHID) {                            

                            //-------------------------------------------------------------------------------------
                            // When you change members, kick out the page and move on to the next member, 
                            // and reset flags.
                            //-------------------------------------------------------------------------------------

                            lastSHID = SHID;
                            lastEquityCropYear = "";

                            // Add Grand Total Line
                            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
                            PdfReports.AddText2Table(detailTable, "Total Amount", _labelFont, "center");
                            PdfReports.AddText2Table(detailTable, " ", _normalFont, 2);
                            PdfReports.AddText2Table(detailTable, totalCheckAmount.ToString("$#,##0.00"), _normalFont, "right");

                            // Add this members detail lines to the document
                            PdfReports.AddTableNoSplit(document, pgEvent, detailTable);

                            // Calc the total for the new SHID
                            totalCheckAmount = CalcRedeemCheck(stateList, i);

                            pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, totalCheckAmount, 0, rptTitle, imgLogo);
                            document.NewPage();

                            // Refresh the detail table
                            detailTable = PdfReports.CreateTable(_primaryTableLayout, 0);
                            AddRetainRedeemDetailHeader(detailTable);
                        } 
                    }

                    // =======================================================
                    // Build Report
                    // =======================================================
                    if (state.EquityCropYear != lastEquityCropYear) {

                        // Before resetting the lastEquityCropYear, a non-blank crop year means this is a subsequent
                        // crop year for the same shid. Add a blank
                        if (lastEquityCropYear.Length > 0) {
                            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
                        }

                        lastEquityCropYear = state.EquityCropYear;                        

                        // Show Equity Data for Crop Year
                        PdfReports.AddText2Table(detailTable, state.EquityCropYear, _normalFont, "center");
                        PdfReports.AddText2Table(detailTable, state.EquityTons, _normalFont, "right");
                        PdfReports.AddText2Table(detailTable, state.RatePerTon, _normalFont, "right");
                        PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.RedeemAmt).ToString("$#,##0.00"), _normalFont, "right");

                        if (state.DeductionDesc.Length > 0) {

                            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);

                            PdfReports.AddText2Table(detailTable, " ", _normalFont);
                            PdfReports.AddText2Table(detailTable, "Less Deductions: ", _normalItalicFont, "left", 3);

                            PdfReports.AddText2Table(detailTable, " ", _normalFont);
                            PdfReports.AddText2Table(detailTable, state.DeductionDesc, _normalFont, "left", 2);
                            PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.DeductionAmt).ToString("$#,##0.00"), _normalFont, "right");
                        }

                    } else {

                        //----------------------------------------------------------
                        // This is a deduction only line, a subsequent deduction.
                        //----------------------------------------------------------
                        PdfReports.AddText2Table(detailTable, " ", _normalFont);
                        PdfReports.AddText2Table(detailTable, state.DeductionDesc, _normalFont, "left", 2);
                        PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.DeductionAmt).ToString("$#,##0.00"), _normalFont, "right");
                    }
                }

                if (detailTable != null) {

                    // Add Grand Total Line
                    PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
                    PdfReports.AddText2Table(detailTable, "Total Amount", _labelFont, "center");
                    PdfReports.AddText2Table(detailTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(detailTable, totalCheckAmount.ToString("$#,##0.00"), _normalFont, "right");

                    // Add this members detail lines to the document
                    PdfReports.AddTableNoSplit(document, pgEvent, detailTable);
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

        private static void ReportBuilderPatronage(int cropYear, string paymentDate, string shid, string fromShid, string toShid, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilderPatronage: ";

            string SHID = "";
            string equityCropYear = "";
            string lastSHID = "";
            string lastEquityCropYear = "";
            decimal totalCheckAmount = 0;
            string rptTitle = "";
            string statementDate = Convert.ToDateTime(paymentDate).ToString(HEADER_DATE_FORMAT);

            Document document = null;
            PdfWriter writer = null;
            PdfPTable detailTable = null;
            iTextSharp.text.Image imgLogo = null;

            StatementPatronageRetainEvent pgEvent = null;

            try {

                List<ListStatementPatRetainItem> stateList = WSCReportsExec.RptStatementPatronage(cropYear, shid, fromShid, toShid, paymentDate);

                for (int i = 0; i < stateList.Count; i++) {

                    ListStatementPatRetainItem state = stateList[i];

                    if (rptTitle.Length == 0) {
                        rptTitle = state.EquityCropYear + " Patronage -- " + state.Qualified + " -- " + statementDate;
                    }

                    SHID = state.SHID;
                    equityCropYear = state.EquityCropYear;

                    if (document == null) {

                        // Create the detail table
                        detailTable = PdfReports.CreateTable(_primaryTableLayout, 0);
                        AddPatronageDetailHeader(detailTable, state.PatInitPayPct);

                        lastSHID = state.SHID;
                        totalCheckAmount = CalcPatronageCheck(stateList, i);

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        imgLogo = PdfReports.GetImage(logoUrl, 77, 45, iTextSharp.text.Element.ALIGN_CENTER);

                        // Attach my override event handler(s)
                        pgEvent = new StatementPatronageRetainEvent();
                        pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, totalCheckAmount, 0, rptTitle, imgLogo);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                    } else {

                        if (lastSHID != SHID) {

                            //-------------------------------------------------------------------------------------
                            // When you change members, kick out the page and move on to the next member, 
                            // and reset flags.
                            //-------------------------------------------------------------------------------------

                            lastSHID = SHID;
                            lastEquityCropYear = "";

                            // Add Grand Total Line
                            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
                            PdfReports.AddText2Table(detailTable, "Total Amount", _labelFont, "center");
                            PdfReports.AddText2Table(detailTable, " ", _normalFont, 2);
                            PdfReports.AddText2Table(detailTable, totalCheckAmount.ToString("$#,##0.00"), _normalFont, "right");

                            // Add this members detail lines to the document
                            PdfReports.AddTableNoSplit(document, pgEvent, detailTable);

                            // Calc the total for the new SHID
                            totalCheckAmount = CalcPatronageCheck(stateList, i);

                            pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, totalCheckAmount, 0, rptTitle, imgLogo);
                            document.NewPage();

                            // Refresh the detail table
                            detailTable = PdfReports.CreateTable(_primaryTableLayout, 0);
                            AddPatronageDetailHeader(detailTable, state.PatInitPayPct);
                        }
                    }

                    // =======================================================
                    // Build Report
                    // =======================================================
                    if (state.EquityCropYear != lastEquityCropYear) {

                        lastEquityCropYear = state.EquityCropYear;

                        // Show Equity Data for Crop Year
                        PdfReports.AddText2Table(detailTable, "Total Amount Initial Patronage Payment", _normalFont, "left", 3);
                        PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.PatInitPayment).ToString("$#,##0.00"), _normalFont, "right");

                        if (state.DeductionDesc.Length > 0) {

                            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);

                            PdfReports.AddText2Table(detailTable, " ", _normalFont);
                            PdfReports.AddText2Table(detailTable, "Less Deductions: ", _normalItalicFont, "left", 3);

                            PdfReports.AddText2Table(detailTable, " ", _normalFont);
                            PdfReports.AddText2Table(detailTable, state.DeductionDesc, _normalFont, "left", 2);
                            PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.DeductionAmt).ToString("$#,##0.00"), _normalFont, "right");
                        }

                    } else {

                        //----------------------------------------------------------
                        // This is a deduction only line, a subsequent deduction.
                        //----------------------------------------------------------
                        PdfReports.AddText2Table(detailTable, " ", _normalFont);
                        PdfReports.AddText2Table(detailTable, state.DeductionDesc, _normalFont, "left", 2);
                        PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.DeductionAmt).ToString("$#,##0.00"), _normalFont, "right");
                    }
                }

                if (detailTable != null) {

                    // Add Grand Total Line
                    PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
                    PdfReports.AddText2Table(detailTable, "Total Amount", _labelFont, "center");
                    PdfReports.AddText2Table(detailTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(detailTable, totalCheckAmount.ToString("$#,##0.00"), _normalFont, "right");

                    // Add this members detail lines to the document
                    PdfReports.AddTableNoSplit(document, pgEvent, detailTable);
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

        private static void ReportBuilderPatronageRedeems(int cropYear, string paymentDate, string shid, string fromShid, string toShid, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilderPatronageRedeems: ";

            string SHID = "";
            string equityCropYear = "";
            string lastSHID = "";
            string lastEquityCropYear = "";
            decimal totalCheckAmount = 0;
            string rptTitle = "";
            string statementDate = Convert.ToDateTime(paymentDate).ToString(HEADER_DATE_FORMAT);

            Document document = null;
            PdfWriter writer = null;
            PdfPTable detailTable = null;
            iTextSharp.text.Image imgLogo = null;
            StatementPatronageRetainEvent pgEvent = null;

            try {

                List<ListStatementPatRetainItem> stateList = WSCReportsExec.RptStatementPatronageRedeem(cropYear, shid, fromShid, toShid, paymentDate);

                for (int i = 0; i < stateList.Count; i++) {

                    ListStatementPatRetainItem state = stateList[i];

                    if (rptTitle.Length == 0) {
                        //rptTitle = "Patronage -- " + state.Qualified + " -- " + statementDate;
						rptTitle = "Patronage Redeem " + ", " + statementDate;
                    }

                    SHID = state.SHID;
                    equityCropYear = state.EquityCropYear;

                    if (document == null) {

                        // Create the detail table
                        detailTable = PdfReports.CreateTable(_primaryTableLayout, 0);
                        AddPatronageRedeemDetailHeader(detailTable);

                        lastSHID = state.SHID;
                        totalCheckAmount = CalcRedeemCheck(stateList, i);

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        imgLogo = PdfReports.GetImage(logoUrl, 77, 45, iTextSharp.text.Element.ALIGN_CENTER);

                        // Attach my override event handler(s)
                        pgEvent = new StatementPatronageRetainEvent();
                        pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, totalCheckAmount, 0, rptTitle, imgLogo);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                    } else {

                        if (lastSHID != SHID) {

                            //-------------------------------------------------------------------------------------
                            // When you change members, kick out the page and move on to the next member, 
                            // and reset flags.
                            //-------------------------------------------------------------------------------------

                            lastSHID = SHID;
                            lastEquityCropYear = "";

                            // Add Grand Total Line
                            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
                            PdfReports.AddText2Table(detailTable, "Total Amount", _labelFont, "center");
                            PdfReports.AddText2Table(detailTable, " ", _normalFont, 2);
                            PdfReports.AddText2Table(detailTable, totalCheckAmount.ToString("$#,##0.00"), _normalFont, "right");

                            // Add this members detail lines to the document
                            PdfReports.AddTableNoSplit(document, pgEvent, detailTable);

                            // Calc the total for the new SHID
                            totalCheckAmount = CalcRedeemCheck(stateList, i);

                            pgEvent.FillEvent(state.SHID, state.BusName, state.Addr1, state.Addr2, state.CSZ, totalCheckAmount, 0, rptTitle, imgLogo);
                            document.NewPage();

                            // Refresh the detail table
                            detailTable = PdfReports.CreateTable(_primaryTableLayout, 0);
                            AddPatronageRedeemDetailHeader(detailTable);
                        }
                    }

                    // =======================================================
                    // Build Report
                    // =======================================================
                    if (state.EquityCropYear != lastEquityCropYear) {

                        // Before resetting the lastEquityCropYear, a non-blank crop year means this is a subsequent
                        // crop year for the same shid. Add a blank
                        if (lastEquityCropYear.Length > 0) {
                            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
                        }

                        lastEquityCropYear = state.EquityCropYear;

                        // Show Equity Data for Crop Year
                        PdfReports.AddText2Table(detailTable, state.EquityCropYear, _normalFont, "center");
                        PdfReports.AddText2Table(detailTable, " ", _normalFont, "right");
                        PdfReports.AddText2Table(detailTable, " ", _normalFont, "right");
                        PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.RedeemAmt).ToString("$#,##0.00"), _normalFont, "right");

                        if (state.DeductionDesc.Length > 0) {

                            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);

                            PdfReports.AddText2Table(detailTable, " ", _normalFont);
                            PdfReports.AddText2Table(detailTable, "Less Deductions: ", _normalItalicFont, "left", 3);

                            PdfReports.AddText2Table(detailTable, " ", _normalFont);
                            PdfReports.AddText2Table(detailTable, state.DeductionDesc, _normalFont, "left", 2);
                            PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.DeductionAmt).ToString("$#,##0.00"), _normalFont, "right");
                        }

                    } else {

                        //----------------------------------------------------------
                        // This is a deduction only line, a subsequent deduction.
                        //----------------------------------------------------------
                        PdfReports.AddText2Table(detailTable, " ", _normalFont);
                        PdfReports.AddText2Table(detailTable, state.DeductionDesc, _normalFont, "left", 2);
                        PdfReports.AddText2Table(detailTable, Convert.ToDecimal(state.DeductionAmt).ToString("$#,##0.00"), _normalFont, "right");
                    }
                }

                if (detailTable != null) {

                    // Add Grand Total Line
                    PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
                    PdfReports.AddText2Table(detailTable, "Total Amount", _labelFont, "center");
                    PdfReports.AddText2Table(detailTable, " ", _normalFont, 2);
                    PdfReports.AddText2Table(detailTable, totalCheckAmount.ToString("$#,##0.00"), _normalFont, "right");

                    // Add this members detail lines to the document
                    PdfReports.AddTableNoSplit(document, pgEvent, detailTable);
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

        private static void AddRetainRedeemDetailHeader(PdfPTable detailTable) {

            PdfReports.AddText2Table(detailTable, "Crop Year", _labelFont, "center");
            PdfReports.AddText2Table(detailTable, "Total Tons", _labelFont, "right");
            PdfReports.AddText2Table(detailTable, "Per Ton", _labelFont, "right");
            PdfReports.AddText2Table(detailTable, "Total Dollars", _labelFont, "right");
        }

        private static void AddPatronageRedeemDetailHeader(PdfPTable detailTable) {

            PdfReports.AddText2Table(detailTable, "Crop Year", _labelFont, "center");
            PdfReports.AddText2Table(detailTable, " ", _labelFont, "right");
            PdfReports.AddText2Table(detailTable, " ", _labelFont, "right");
            PdfReports.AddText2Table(detailTable, "Total Dollars", _labelFont, "right");
        }

        private static void AddPatronageDetailHeader(PdfPTable detailTable, string initPayPct) {

            PdfReports.AddText2Table(detailTable, "Statement of Deductions from " + initPayPct + "% Initial Payment", _labelFont, "left", _primaryTableLayout.Length);
            PdfReports.AddText2Table(detailTable, " ", _normalFont, _primaryTableLayout.Length);
        }

        private static decimal CalcRedeemCheck(List<ListStatementPatRetainItem> stateList, int index ) {

            decimal checkAmt = 0;
            int maxIndex = stateList.Count;
            string shid = stateList[index].SHID;
            string lastCropYear = "";

            while (index < maxIndex && stateList[index].SHID == shid) {

                ListStatementPatRetainItem state = stateList[index];

                if (state.EquityCropYear != lastCropYear) {
                    lastCropYear = state.EquityCropYear;
                    checkAmt += Convert.ToDecimal(state.RedeemAmt);
                }
                checkAmt -= Convert.ToDecimal(state.DeductionAmt);

                index += 1;
            }

            return checkAmt;
        }

        private static decimal CalcPatronageCheck(List<ListStatementPatRetainItem> stateList, int index) {

            decimal checkAmt = 0;
            int maxIndex = stateList.Count;
            string shid = stateList[index].SHID;
            string lastCropYear = "";

            while (index < maxIndex && stateList[index].SHID == shid) {

                ListStatementPatRetainItem state = stateList[index];

                if (state.EquityCropYear != lastCropYear) {
                    lastCropYear = state.EquityCropYear;
                    checkAmt += Convert.ToDecimal(state.PatInitPayment);
                }
                checkAmt -= Convert.ToDecimal(state.DeductionAmt);

                index += 1;
            }

            return checkAmt;
        }
    }


    public class StatementPatronageRetainEvent : PdfPageEventHelper, ICustomPageEvent {

        private static float[] _logo2010Layout = new float[] { 77F, 77F, 309F, 77F };

        Font _superTitleFont = FontFactory.GetFont("HELVETICA", 16F, Font.BOLD);
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

        private string _shid = "";
        private string _businessName = "";
        private string _memAddr1 = "";
        private string _memAddr2 = "";
        private string _memCSZ = "";

        private decimal _checkAmount = 0;

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
                // Add Header: Logo 2010
                // =======================================================

                PdfPTable logoTable = PdfReports.CreateTable(_logo2010Layout, 0);

                // Only add actual logo on First Page of set
                Paragraph p = new Paragraph("", _normalFont);
                if (_pageNumber == 1) {                    

                    // BLANK
                    PdfReports.AddText2Table(logoTable, " ", _normalFont);
                    
                    // LOGO                    
                    PdfReports.AddImage2Table(logoTable, _imgLogo);

                    // TEXT                    
                    Phrase ph = new Phrase(16F, "The Western Sugar Cooperative", _superTitleFont);
                    p.Add(ph);
                    ph = new Phrase(5F, "\n\n(Grower Owned)", _normalFont);
                    p.Add(ph);
                    PdfReports.AddText2Table(logoTable, p, "center");

                    // BLANK
                    PdfReports.AddText2Table(logoTable, " ", _normalFont);

                } else {
                    PdfReports.AddText2Table(logoTable, " ", _normalFont, _logo2010Layout.Length);
                }

                //-----------------------------------------
                // Add and center the title
                //-----------------------------------------
                PdfReports.AddText2Table(logoTable, " ", _normalFont);
                PdfReports.AddText2Table(logoTable, _title, _titleFont, "center", 2);
                PdfReports.AddText2Table(logoTable, " ", _normalFont);

                // Skip a line: adjust font size to automatically adjust leading.  Setting leading is not working  !!!
                //PdfReports.AddText2Table(logoTable, " \n ", FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL), _logo2010Layout.Length);

                PdfReports.AddTableNoSplit(document, this, logoTable);

                float[] addrLayout = new float[] { 50F, 355F, 135F };
                PdfPTable addrTable = PdfReports.CreateTable(addrLayout, 0);

                if (_pageNumber == 1) {

                    //-----------------------------------------------
                    // Next Add SHID / Check Amt
                    //-----------------------------------------------
                    float[] summaryLayout = new float[] { 385F, 60F, 95F };
                    PdfPTable summaryTable = PdfReports.CreateTable(summaryLayout, 0);

                    PdfReports.AddText2Table(summaryTable, " ", _normalFont, summaryLayout.Length);

                    PdfReports.AddText2Table(summaryTable, " ", _normalFont);
                    PdfReports.AddText2Table(summaryTable, "SHID:", _labelFont, "left");
                    PdfReports.AddText2Table(summaryTable, _shid, _normalFont, "right");

                    PdfReports.AddText2Table(summaryTable, " ", _normalFont);
                    PdfReports.AddText2Table(summaryTable, "Check Amt:", _labelFont, "left");
                    PdfReports.AddText2Table(summaryTable, _checkAmount.ToString("$#,##0.00"), _normalFont, "right");

                    PdfReports.AddTableNoSplit(document, this, summaryTable);

                    //-----------------------------------------------
                    // Address block
                    //-----------------------------------------------

                    //PdfReports.AddText2Table(addrTable, " ", _normalFont, addrLayout.Length);       // Blank Line
                    PdfReports.AddText2Table(addrTable, " ", _normalFont);

                    p = PdfReports.GetAddressBlock(_businessName, _memAddr1, _memAddr2, _memCSZ,
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

        public void FillEvent(string shid, string busName, string addr1, string addr2, string csz, decimal checkAmount, int pageNumber, string title, iTextSharp.text.Image imgLogo) {

            _shid = shid;
            _businessName = busName;
            _memAddr1 = addr1;
            _memAddr2 = addr2;
            _memCSZ = csz;
            _checkAmount = checkAmount; 

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

