using System;
using System.Configuration;
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
    /// Summary description for rptTransmittal.
    /// </summary>
    public class rptTransmittal {

        private static float[] _primaryTableLayout = new float[] { 118.5F, 40.0F, 46.2F, 46.2F, 46.2F, 82.3F, 82.3F, 92.3F };

        public static string ReportPackager(
            int cropYear, int paymentNumber, string paymentDescription,
            string fromDate, string toDate, string statementDate,
            string factoryList, string stationList, string contractList,
            bool isCumulative, string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "rptTransmittal.ReportPackager: ";
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

                        using (SqlDataReader dr = WSCPayment.GetTransmittalPayment(conn,
                            cropYear, paymentNumber, factoryList, stationList, contractList, isCumulative)) {

                            using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                                ReportBuilder(dr, cropYear, paymentNumber, paymentDescription, fromDate, toDate, statementDate, isCumulative, logoUrl, fs);
                            }
                        }
                    }
                }
                catch (System.Exception ex) {
                    string errMsg = "cropYear: " + cropYear.ToString();

                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                    throw (wscEx);
                }

                return filePath;
            }
            catch (System.Exception ex) {
                string errMsg = "cropYear: " + cropYear.ToString() + "; " +
                    "Payment Number: " + paymentNumber.ToString();

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                throw (wscEx);
            }
        }

        private static void ReportBuilder(SqlDataReader drPay, int cropYear, int paymentNumber, string paymentDescription,
            string fromDate, string toDate, string statementDate, bool isCumulative, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "rptTransmittal.ReportBuilder: ";
            const int resetFlag = 0;

            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            TransmittalReportEvent pgEvent = null;
            iTextSharp.text.Image imgLogo = null;

            int contractNumber = 0;
            int lastContractNumber = 0;
            int payeeNumber = 0;
            int lastPayeeNumber = 0;

            decimal ytdEHBonus = 0;
            decimal ytdEHAmount = 0;
            decimal ytdRHAmount = 0;
            decimal ytdDeductions = 0;
			decimal ytdEHAmountMoved = 0;
			decimal ytdRHAmountMoved = 0;
            decimal ytdNet = 0;
            decimal curSLM = 0;
            decimal pctPaid = 0;
            decimal actualSugar = 0;

            string rptTitle = "Western Sugar Cooperative Payment Transmittal";

            Font headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    while (drPay.Read()) {

                        try {

                            contractNumber = Convert.ToInt32(drPay.GetString(drPay.GetOrdinal("Contract_Number")));
                            payeeNumber = drPay.GetInt16(drPay.GetOrdinal("Payee_Number"));

                            if (document == null) {

                                lastContractNumber = contractNumber;
                                lastPayeeNumber = payeeNumber;

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
                                pgEvent = new TransmittalReportEvent();
                                pgEvent.FillEvent(drPay, cropYear, statementDate, resetFlag, rptTitle, paymentDescription, imgLogo);
                                writer.PageEvent = pgEvent;

                                // Open the document
                                document.Open();

                            } else {

                                if (contractNumber != lastContractNumber || payeeNumber != lastPayeeNumber) {

                                    ytdEHBonus = 0;
                                    ytdEHAmount = 0;
                                    ytdRHAmount = 0;
                                    ytdDeductions = 0;
                                    ytdNet = 0;
									ytdEHAmountMoved = 0;
									ytdRHAmountMoved = 0;

                                    lastContractNumber = contractNumber;
                                    if (payeeNumber != lastPayeeNumber) {
                                        lastPayeeNumber = payeeNumber;
                                    }
                                    pgEvent.FillEvent(drPay, cropYear, statementDate, resetFlag, rptTitle, paymentDescription, imgLogo);
                                    document.NewPage();
                                }
                            }

                            // =======================================================
                            // Add Payment Detail
                            // =======================================================
                            table = PdfReports.CreateTable(_primaryTableLayout, 1);
                            PdfReports.AddText2Table(table, drPay.GetString(drPay.GetOrdinal("Payment_Description")) + " Payment", labelFont, 8);

                            // Header
                            PdfReports.AddText2Table(table, " ", labelFont);
                            PdfReports.AddText2Table(table, "SLM", labelFont, "center");
                            PdfReports.AddText2Table(table, "% Sugar", labelFont);
                            PdfReports.AddText2Table(table, "% Paid", labelFont);
                            PdfReports.AddText2Table(table, "Price/Ton", labelFont);
                            PdfReports.AddText2Table(table, "Tons", labelFont);
                            PdfReports.AddText2Table(table, "YTD Amount", labelFont);
                            PdfReports.AddText2Table(table, "Current Amount", labelFont);

                            // EH Premium
                            PdfReports.AddText2Table(table, "Early Harvest Premium", labelFont, "left");
                            PdfReports.AddText2Table(table, " ", labelFont);
                            PdfReports.AddText2Table(table, " ", labelFont);
                            PdfReports.AddText2Table(table, " ", labelFont);
                            PdfReports.AddText2Table(table, " ", labelFont);
                            PdfReports.AddText2Table(table, " ", labelFont);

                            if (isCumulative) {
                                ytdEHBonus += drPay.GetDecimal(drPay.GetOrdinal("EH_Bonus"));
                            } else {
                                ytdEHBonus = drPay.GetDecimal(drPay.GetOrdinal("ncYtdEhBonus"));
                            }

                            PdfReports.AddText2Table(table, ytdEHBonus.ToString("$#,##0.00"), normalFont);
                            PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("EH_Bonus")).ToString("$#,##0.00"), normalFont);

                            // Early Harvest
							actualSugar = drPay.GetDecimal(drPay.GetOrdinal("EH_Sugar"));
							curSLM = drPay.GetDecimal(drPay.GetOrdinal("EH_SLM"));
							pctPaid = drPay.GetDecimal(drPay.GetOrdinal("Pct_EH_Paid"));

                            PdfReports.AddText2Table(table, "Early Harvest   ", labelFont, "left");

                            if (drPay.GetDecimal(drPay.GetOrdinal("EH_SLM")) != 0) {
								PdfReports.AddText2Table(table, curSLM.ToString("N4"), normalFont, "center");
                            } else {
                                PdfReports.AddText2Table(table, "------", normalFont, "center");
                            }

                            if (drPay.GetDecimal(drPay.GetOrdinal("EH_Sugar")) != 0) {
                                PdfReports.AddText2Table(table, actualSugar.ToString("N2"), normalFont);
                            } else {
                                PdfReports.AddText2Table(table, "------", normalFont);
                            }

							if (drPay.GetDecimal(drPay.GetOrdinal("Pct_EH_Paid")) != 0) {
								PdfReports.AddText2Table(table, pctPaid.ToString("N3"), normalFont);
                            } else {
                                PdfReports.AddText2Table(table, "------", normalFont);
                            }

                            if (drPay.GetDecimal(drPay.GetOrdinal("EH_Price")) != 0) {
                                PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("EH_Price")).ToString("$#,##0.000"), normalFont);
                            } else {
                                PdfReports.AddText2Table(table, "------", normalFont);
                            }

                            if (drPay.GetDecimal(drPay.GetOrdinal("EH_Tons")) != 0 || drPay.GetDecimal(drPay.GetOrdinal("EH_tons_moved")) != 0) {
								PdfReports.AddText2Table(table, (drPay.GetDecimal(drPay.GetOrdinal("EH_Tons")) - drPay.GetDecimal(drPay.GetOrdinal("EH_tons_moved"))).ToString("N4"), normalFont);
                            } else {
                                PdfReports.AddText2Table(table, "------", normalFont);
                            }

                            if (isCumulative) {
								ytdEHAmount += drPay.GetDecimal(drPay.GetOrdinal("EH_Gross_Pay")) - drPay.GetDecimal(drPay.GetOrdinal("EH_amt_moved"));
                            } else {
                                ytdEHAmount = drPay.GetDecimal(drPay.GetOrdinal("ncYtdEhGrossPay")) - drPay.GetDecimal(drPay.GetOrdinal("ncYtdEhAmtMoved"));
                            }
                            PdfReports.AddText2Table(table, ytdEHAmount.ToString("$#,##0.00"), normalFont);
							PdfReports.AddText2Table(table, (drPay.GetDecimal(drPay.GetOrdinal("EH_Gross_Pay")) - drPay.GetDecimal(drPay.GetOrdinal("EH_amt_moved"))).ToString("$#,##0.00"), normalFont);

                            // Regular Harvest
                            actualSugar = drPay.GetDecimal(drPay.GetOrdinal("RH_Sugar"));
                            curSLM = drPay.GetDecimal(drPay.GetOrdinal("RH_SLM"));
                            pctPaid = drPay.GetDecimal(drPay.GetOrdinal("Pct_RH_Paid"));

                            PdfReports.AddText2Table(table, "Regular Harvest ", labelFont, "left");
                            PdfReports.AddText2Table(table, curSLM.ToString("N4"), normalFont, "center");
                            PdfReports.AddText2Table(table, actualSugar.ToString("N2"), normalFont);
                            PdfReports.AddText2Table(table, pctPaid.ToString("N3"), normalFont);
                            PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("RH_Price")).ToString("$#,##0.000"), normalFont);
                            PdfReports.AddText2Table(table, (drPay.GetDecimal(drPay.GetOrdinal("RH_Tons"))
								- drPay.GetDecimal(drPay.GetOrdinal("RH_Tons_moved"))).ToString("N4"), normalFont);
                            if (isCumulative) {
								ytdRHAmount += drPay.GetDecimal(drPay.GetOrdinal("RH_Gross_Pay")) - drPay.GetDecimal(drPay.GetOrdinal("RH_amt_moved"));
                            } else {
								ytdRHAmount = drPay.GetDecimal(drPay.GetOrdinal("ncYtdRhGrossPay")) - drPay.GetDecimal(drPay.GetOrdinal("ncYtdRhAmtMoved"));
                            }
							// YTD Amount
                            PdfReports.AddText2Table(table, ytdRHAmount.ToString("$#,##0.00"), normalFont);
							// Current Amount
							PdfReports.AddText2Table(table, (drPay.GetDecimal(drPay.GetOrdinal("RH_Gross_Pay")) - drPay.GetDecimal(drPay.GetOrdinal("RH_amt_moved"))).ToString("$#,##0.00"), normalFont);

							// Reduced Early Harvest
							PdfReports.AddText2Table(table, "Reduced Early Harvest Excess Beets  ", labelFont, "left", 4);
							PdfReports.AddText2Table(table, " ", normalFont);
							PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("EH_tons_moved")).ToString("#,#.0000;(#,#.0000)"), normalFont);

							if (isCumulative) {
								ytdEHAmountMoved += drPay.GetDecimal(drPay.GetOrdinal("EH_amt_moved"));
							} else {
								ytdEHAmountMoved = drPay.GetDecimal(drPay.GetOrdinal("ncYtdEhAmtMoved"));
							}
							PdfReports.AddText2Table(table, ytdEHAmountMoved.ToString("$#,#.00;$(#,#.00)"), normalFont);
							PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("EH_amt_moved")).ToString("$#,#.00;$(#,#.00)"), normalFont);

							// Reduced Regular Harvest
							PdfReports.AddText2Table(table, "Reduced Regular Harvest Excess Beets", labelFont, "left", 4);
							PdfReports.AddText2Table(table, " ", normalFont);
							PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("RH_tons_moved")).ToString("#,#.0000;(#,#.0000)"), normalFont);
							if (isCumulative) {
								ytdRHAmountMoved += drPay.GetDecimal(drPay.GetOrdinal("RH_amt_moved"));
							} else {
								ytdRHAmountMoved = drPay.GetDecimal(drPay.GetOrdinal("ncYtdRhAmtMoved"));
							}
							PdfReports.AddText2Table(table, ytdRHAmountMoved.ToString("$#,#.00;$(#,#.00)"), normalFont);
							PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("RH_amt_moved")).ToString("$#,#.00;$(#,#.00)"), normalFont);

                            PdfReports.AddText2Table(table, " ", normalFont, 8);

                            // Total lines
							//		YTD Amount
                            PdfReports.AddText2Table(table, " ", normalFont, 4);
                            PdfReports.AddText2Table(table, "Total Gross Amount: ", labelFont, "right", 2);
                            PdfReports.AddText2Table(table, ((ytdEHBonus + ytdEHAmount + ytdRHAmount)
								+ ytdEHAmountMoved
								+ ytdRHAmountMoved
								//+ drPay.GetDecimal(drPay.GetOrdinal("ncYtdEhAmtMoved"))
								//+ drPay.GetDecimal(drPay.GetOrdinal("ncYtdRhAmtMoved"))
								).ToString("#,##0.00"), normalFont);
					
							//		Current Amount
                            PdfReports.AddText2Table(table, (drPay.GetDecimal(drPay.GetOrdinal("EH_Bonus")) +
                                drPay.GetDecimal(drPay.GetOrdinal("EH_Gross_Pay")) +
                                drPay.GetDecimal(drPay.GetOrdinal("RH_Gross_Pay")) //+
								//drPay.GetDecimal(drPay.GetOrdinal("EH_amt_moved")) +
								//drPay.GetDecimal(drPay.GetOrdinal("RH_amt_moved"))
								).ToString("#,##0.00"), normalFont);

							PdfReports.AddText2Table(table, " ", normalFont, 5);
							PdfReports.AddText2Table(table, "Deduction Total: ", labelFont, "right");

							if (isCumulative) {
								ytdDeductions += drPay.GetDecimal(drPay.GetOrdinal("Deduct_Total"));
							} else {
								ytdDeductions = drPay.GetDecimal(drPay.GetOrdinal("ncYtdDeductTotal"));
							}
							PdfReports.AddText2Table(table, ytdDeductions.ToString("#,##0.00"), normalFont);
							PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("Deduct_Total")).ToString("#,##0.00"), normalFont);

                            PdfReports.AddText2Table(table, " ", normalFont, 5);
                            PdfReports.AddText2Table(table, "Net Payment: ", labelFont, "right");

                            decimal currentNet = drPay.GetDecimal(drPay.GetOrdinal("EH_Bonus")) 
                                + drPay.GetDecimal(drPay.GetOrdinal("EH_Gross_Pay")) 
                                + drPay.GetDecimal(drPay.GetOrdinal("RH_Gross_Pay")) 
								//drPay.GetDecimal(drPay.GetOrdinal("EH_amt_moved")) 
								//drPay.GetDecimal(drPay.GetOrdinal("RH_amt_moved")) 
                                - drPay.GetDecimal(drPay.GetOrdinal("Deduct_Total"));

                            if (isCumulative) {
                                ytdNet += currentNet;
                            } else {
                                ytdNet = drPay.GetDecimal(drPay.GetOrdinal("ncYtdEhBonus")) 
                                + drPay.GetDecimal(drPay.GetOrdinal("ncYtdEhGrossPay")) 
                                + drPay.GetDecimal(drPay.GetOrdinal("ncYtdRhGrossPay")) 
								//drPay.GetDecimal(drPay.GetOrdinal("ncYtdEhAmtMoved")) 
								//drPay.GetDecimal(drPay.GetOrdinal("ncYtdRhAmtMoved")) 
                                - drPay.GetDecimal(drPay.GetOrdinal("ncYtdDeductTotal"));
                            }

                            PdfReports.AddText2Table(table, ytdNet.ToString("#,##0.00"), normalFont);
                            PdfReports.AddText2Table(table, currentNet.ToString("#,##0.00"), normalFont);

                            PdfReports.AddText2Table(table, " ", normalFont, 8);
                            PdfReports.AddText2Table(table, " ", normalFont, 6);
                            PdfReports.AddText2Table(table, "Grower Net: ", labelFont, "right");
                            PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("Payment_Amount")).ToString("#,##0.00"), normalFont);

                            PdfReports.AddText2Table(table, " ", normalFont, 6);
                            PdfReports.AddText2Table(table, "Landowner Net: ", labelFont, "right");
                            PdfReports.AddText2Table(table, drPay.GetDecimal(drPay.GetOrdinal("Split_Payment")).ToString("#,##0.00"), normalFont);

                            PdfReports.AddText2Table(table, " ", normalFont, 8);

                            PdfReports.AddTableNoSplit(document, pgEvent, table);

                            if (paymentNumber == drPay.GetInt16(drPay.GetOrdinal("Payment_Number"))) {

                                //================================================================
                                // Add Deduction information
                                //================================================================
                                table = PdfReports.CreateTable(_primaryTableLayout, 1);

                                using (SqlDataReader drDed = WSCPayment.GetTransmittalDeduction(conn,
                                          cropYear, contractNumber,
                                          drPay.GetInt16(drPay.GetOrdinal("Payment_Number")), isCumulative)) {

                                    PdfReports.AddText2Table(table, "Deduction", labelFont, 5);
                                    PdfReports.AddText2Table(table, "Payment", labelFont);
                                    PdfReports.AddText2Table(table, "Amount", labelFont);
                                    PdfReports.AddText2Table(table, " ", labelFont);

                                    while (drDed.Read()) {

                                        PdfReports.AddText2Table(table, drDed.GetString(drDed.GetOrdinal("Deduction_Desc")), normalFont, 5);
                                        PdfReports.AddText2Table(table, drDed.GetString(drDed.GetOrdinal("Payment_Description")), normalFont);
                                        PdfReports.AddText2Table(table, drDed.GetDecimal(drDed.GetOrdinal("Amount")).ToString("$#,##0.00"), normalFont);
                                        PdfReports.AddText2Table(table, " ", labelFont);
                                    }
                                    PdfReports.AddText2Table(table, " ", normalFont, 8);

                                    PdfReports.AddTableNoSplit(document, pgEvent, table);
                                }

                                //================================================================
                                // Add Delivery information
                                //================================================================
                                float[] deliveryLayout = new float[] { 110.8F, 110.8F, 55.4F, 110.8F, 55.4F, 55.4F, 55.4F };
                                table = PdfReports.CreateTable(deliveryLayout, 1);

								List<TransmittalDeliveryItem> deliveryList = WSCPayment.GetTransmittalDelivery(cropYear, contractNumber, paymentNumber, fromDate, toDate);

                                PdfReports.AddText2Table(table, "Delivery Date", labelFont);
                                PdfReports.AddText2Table(table, "First Net Pounds", labelFont);
                                PdfReports.AddText2Table(table, "Tare %", labelFont);
                                PdfReports.AddText2Table(table, "Final Net Lbs", labelFont);
                                PdfReports.AddText2Table(table, "Sugar %", labelFont);
                                PdfReports.AddText2Table(table, "SLM", labelFont);
                                PdfReports.AddText2Table(table, "Loads", labelFont);

								foreach (TransmittalDeliveryItem deliveryDay in deliveryList) {

                                    PdfReports.AddText2Table(table, deliveryDay.Delivery_Date.ToShortDateString(), normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.First_Net_Pounds.ToString("###,###"), normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.Tare.ToString("N2"), normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.Final_Net_Pounds.ToString("###,###"), normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.Sugar_Content.ToString("N2"), normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.SLM_Pct.ToString("N4"), normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.Loads.ToString(), normalFont);
                                }
                                PdfReports.AddText2Table(table, " ", normalFont, 6);
                                PdfReports.AddTableNoSplit(document, pgEvent, table);
                            }
                        }
                        catch (System.Exception ex) {
                            WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME, ex);
                            throw (wscEx);
                        }
                    }
                }

                // ======================================================
                // Close document
                // ======================================================
                if (document != null) {

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
                    document.Close();
                }
                if (drPay != null) {
                    if (!drPay.IsClosed) {
                        drPay.Close();
                    }
                }
                if (writer != null) {
                    writer.Close();
                }
            }
        }
    }

    internal class TransmittalReportEvent : PdfPageEventHelper, ICustomPageEvent {

        Font titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        Font subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        Font uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        // This is the contentbyte object of the writer
        PdfContentByte _cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;
        ColumnText _ct = null;
        // we will put the final number of pages in a template
        PdfTemplate _template = null;

        private float _headerBottomYLine;
        private bool _isDocumentClosing = false;
        private string _title = "";
        private int _pageNumber = 0;
        private int _lastPageNumber = 0;
        private string _growerName = "";
        private string _adr1 = "";
        private string _adr2 = "";
        private string _city = "";
        private string _state = "";
        private string _postalCode = "";
        private string _contractNumber = "";
        private string _factoryName = "";
        private string _stationName = "";
        private string _paymentDesc = "";
        private string _cropYear = "";
        private string _statementDate = "";
        private iTextSharp.text.Image _imgLogo = null;

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document) {

            _bf = normalFont.GetCalculatedBaseFont(false);
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
                float[] logoLayout = new float[] { 413F, 127F };
                PdfPTable logoTable = PdfReports.CreateTable(logoLayout, 0);
                Paragraph p = null;

                PdfReports.AddText2Table(logoTable, _title + "\n" +
                    _paymentDesc + " - " + _cropYear + " Crop " + (_statementDate.Length > 0 ? "- " : "") +
                    _statementDate, titleFont, "center");

                if (_pageNumber == 1) {
                    PdfReports.AddImage2Table(logoTable, _imgLogo);
                } else {
                    PdfReports.AddText2Table(logoTable, " ", normalFont);
                }
                PdfReports.AddTableNoSplit(document, this, logoTable);

                float[] headerLayout = new float[] { 51.5F, 306.4F, 44.1F, 51.5F, 86.5F };
                PdfPTable table = PdfReports.CreateTable(headerLayout, 1);
                
                p = new Paragraph(23F, " ", normalFont);
                _ct.AddElement(p);
                _ct.Go(false);

                // Add blank lines
                float[] shareholderLayout = new float[] { 50F, 270F, 220F };
                PdfPTable addrTable = PdfReports.CreateTable(shareholderLayout, 0);

                PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);
                PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);

                if (_pageNumber == 1) {

                    string csz = _city + ", " + _state + " " + _postalCode;
                    p = PdfReports.GetAddressBlock(_growerName, _adr1, _adr2,
                        csz, 0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, uspsFont);
                    PdfReports.AddText2Table(addrTable, " ", uspsFont);
                    PdfReports.AddText2Table(addrTable, p);

                    PdfReports.AddText2Table(addrTable, _contractNumber.ToString() + "\n" +
                        _factoryName + "\n" +
                        _stationName, uspsFont, "right");
                    PdfReports.AddText2Table(addrTable, " ", normalFont, 3);
                    PdfReports.AddText2Table(addrTable, " ", normalFont, 3);

                    PdfReports.AddTableNoSplit(document, this, addrTable);

                } else {

                    PdfReports.AddText2Table(addrTable, " ", uspsFont);
                    PdfReports.AddText2Table(addrTable, _growerName, uspsFont);

                    PdfReports.AddText2Table(addrTable, _factoryName + "\n" +
                        _stationName, uspsFont, "right");
                    PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);
                    PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);

                    PdfReports.AddTableNoSplit(document, this, addrTable);
                }

                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(SqlDataReader dr, int cropYear, string statementDate,
            int pageNumber, string title, string paymentDescription, iTextSharp.text.Image imgLogo) {

            _paymentDesc = paymentDescription;
            _cropYear = cropYear.ToString();

            if (statementDate != null && statementDate.Length > 0) {
                _statementDate = DateTime.Parse(statementDate).ToString("MMMM dd, yyyy");
            } else {
                _statementDate = "";
            }

            _growerName = dr.GetString(dr.GetOrdinal("Business_Name"));
            _adr1 = dr.GetString(dr.GetOrdinal("Address_1"));
            _adr2 = dr.GetString(dr.GetOrdinal("Address_2"));
            _city = dr.GetString(dr.GetOrdinal("City"));
            _state = dr.GetString(dr.GetOrdinal("State"));
            _postalCode = dr.GetString(dr.GetOrdinal("Zip"));
            _contractNumber = dr.GetString(dr.GetOrdinal("Contract_Number"));
            _factoryName = dr.GetString(dr.GetOrdinal("Factory_Name"));
            _stationName = dr.GetString(dr.GetOrdinal("Station_Name"));
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
