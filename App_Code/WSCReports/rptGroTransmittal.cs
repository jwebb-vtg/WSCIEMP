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
    /// Summary description for rptGroTransmittal.
    /// </summary>
    public class rptGroTransmittal {

        private static float[] _primaryTableLayout = new float[] { 118.5F, 40.0F, 46.2F, 46.2F, 46.2F, 82.3F, 82.3F, 92.3F };

        private static Font _headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        public static string ReportPackager(
            int cropYear, int paymentNumber, string paymentDescription,
            string fromDate, string toDate, string statementDate,
            string factoryList, string stationList, string contractList,
            bool isCumulative, string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "rptGroTransmittal.ReportPackager: ";
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

                    using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                        ReportBuilder(cropYear, factoryList, stationList, contractList,
						paymentNumber, paymentDescription, fromDate, toDate, statementDate, isCumulative, logoUrl, fs);
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

        public static void ReportBuilder(int cropYear, string factoryList, string stationList, string contractList, int paymentNumber, 
		string paymentDescription, string fromDate, string toDate, string statementDate, bool isCumulative, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "rptGroTransmittal.ReportBuilder: ";
            const int resetFlag = 0;

            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            iTextSharp.text.Image imgLogo = null;
            GroTransmittalEvent pgEvent = null;

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

            try {

				List<TransmittalPaymentItem> stateList = WSCPayment.GetTransmittalPayment(cropYear, paymentNumber, 
					factoryList, stationList, contractList, isCumulative);

				int minContractNumber = stateList.Min(c => Convert.ToInt32(c.Contract_Number));
				int maxContractNumber = stateList.Max(c => Convert.ToInt32(c.Contract_Number));

				List<TransDeductionListItem> deductionList = WSCPayment.GetTransmittalDeduction2(ConfigurationManager.ConnectionStrings["BeetConn"].ToString(),
					cropYear, 0, paymentNumber, minContractNumber, maxContractNumber, isCumulative);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    foreach (TransmittalPaymentItem item in stateList) {

                        try {

                            contractNumber = Convert.ToInt32(item.Contract_Number);
                            payeeNumber = item.Payee_Number;

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

                                if (statementDate == null) {
                                    statementDate = WSCPayment.GetPaymentTransmittalDate(paymentNumber, cropYear);
                                }

                                // Attach my override event handler(s)
                                pgEvent = new GroTransmittalEvent();
                                pgEvent.FillEvent(item, cropYear, statementDate, resetFlag, rptTitle, paymentDescription, imgLogo);

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
                                    pgEvent.FillEvent(item, cropYear, statementDate, resetFlag, rptTitle, paymentDescription, imgLogo);
                                    document.NewPage();
                                }
                            }

                            // =======================================================
                            // Add Payment Detail
                            // =======================================================
                            table = PdfReports.CreateTable(_primaryTableLayout, 1);
                            PdfReports.AddText2Table(table, item.Payment_Description + " Payment", _labelFont, 8);

                            // Header
                            PdfReports.AddText2Table(table, " ", _labelFont);
                            PdfReports.AddText2Table(table, "SLM", _labelFont, "center");
                            PdfReports.AddText2Table(table, "% Sugar", _labelFont);
                            PdfReports.AddText2Table(table, "% Paid", _labelFont);
                            PdfReports.AddText2Table(table, "Price/Ton", _labelFont);
                            PdfReports.AddText2Table(table, "Tons", _labelFont);
                            PdfReports.AddText2Table(table, "YTD Amount", _labelFont);
                            PdfReports.AddText2Table(table, "Current Amount", _labelFont);

                            // EH Premium
                            PdfReports.AddText2Table(table, "Early Harvest Premium", _labelFont, "left");
                            PdfReports.AddText2Table(table, " ", _labelFont);
                            PdfReports.AddText2Table(table, " ", _labelFont);
                            PdfReports.AddText2Table(table, " ", _labelFont);
                            PdfReports.AddText2Table(table, " ", _labelFont);
                            PdfReports.AddText2Table(table, " ", _labelFont);

                            if (isCumulative) {
                                ytdEHBonus += item.EH_Bonus;
                            } else {
                                ytdEHBonus = item.YtdEhBonus;
                            }

                            PdfReports.AddText2Table(table, ytdEHBonus.ToString("$#,##0.00"), _normalFont);
                            PdfReports.AddText2Table(table, item.EH_Bonus.ToString("$#,##0.00"), _normalFont);

                            // Early Harvest
							actualSugar = item.EH_Sugar;
							curSLM = item.EH_SLM;
							pctPaid = item.Pct_EH_Paid;
                            PdfReports.AddText2Table(table, "Early Harvest", _labelFont, "left");

                            if (item.EH_SLM != 0) {
								PdfReports.AddText2Table(table, curSLM.ToString("N4"), _normalFont, "center");
                            } else {
                                PdfReports.AddText2Table(table, "------", _normalFont, "center");
                            }

                            if (item.EH_Sugar != 0) {
								PdfReports.AddText2Table(table, actualSugar.ToString("N2"), _normalFont);
                            } else {
                                PdfReports.AddText2Table(table, "------", _normalFont);
                            }

                            if (item.EH_Sugar != 0) {
								PdfReports.AddText2Table(table, pctPaid.ToString("N3"), _normalFont);
                            } else {
                                PdfReports.AddText2Table(table, "------", _normalFont);
                            }

                            if (item.EH_Price != 0) {
                                PdfReports.AddText2Table(table, item.EH_Price.ToString("$#,##0.000"), _normalFont);
                            } else {
                                PdfReports.AddText2Table(table, "------", _normalFont);
                            }

							if (item.EH_Tons != 0 || item.EH_Tons_Moved != 0) {
                                PdfReports.AddText2Table(table, (item.EH_Tons - item.EH_Tons_Moved).ToString("N4"), _normalFont);
                            } else {
                                PdfReports.AddText2Table(table, "------", _normalFont);
                            }

                            if (isCumulative) {
                                ytdEHAmount += item.EH_Gross_Pay - item.EH_Amt_Moved;
                            } else {
                                ytdEHAmount = item.YtdEhGrossPay - item.YtdEhAmtMoved;
                            }
                            PdfReports.AddText2Table(table, ytdEHAmount.ToString("$#,##0.00"), _normalFont);
                            PdfReports.AddText2Table(table, (item.EH_Gross_Pay - item.EH_Amt_Moved).ToString("$#,##0.00"), _normalFont);

                            // Regular Harvest
                            actualSugar = item.RH_Sugar;
                            curSLM = item.RH_SLM;
                            pctPaid = item.Pct_RH_Paid;

                            PdfReports.AddText2Table(table, "Regular Harvest", _labelFont, "left");
                            PdfReports.AddText2Table(table, curSLM.ToString("N4"), _normalFont, "center");
                            PdfReports.AddText2Table(table, actualSugar.ToString("N2"), _normalFont);
                            PdfReports.AddText2Table(table, pctPaid.ToString("N3"), _normalFont);
                            PdfReports.AddText2Table(table, item.RH_Price.ToString("$#,##0.000"), _normalFont);
                            PdfReports.AddText2Table(table, (item.RH_Tons - item.RH_Tons_Moved).ToString("N4"), _normalFont);
                            if (isCumulative) {
                                ytdRHAmount += item.RH_Gross_Pay - item.RH_Amt_Moved;
                            } else {
                                ytdRHAmount = item.YtdRhGrossPay - item.YtdRhAmtMoved;
                            }
                            PdfReports.AddText2Table(table, ytdRHAmount.ToString("$#,##0.00"), _normalFont);
                            PdfReports.AddText2Table(table, (item.RH_Gross_Pay - item.RH_Amt_Moved).ToString("$#,##0.00"), _normalFont);

                            PdfReports.AddText2Table(table, " ", _normalFont, 8);

							// Reduced Early Harvest
							PdfReports.AddText2Table(table, "Reduced Early Harvest Excess Beets", _labelFont, "left", 4);
							PdfReports.AddText2Table(table, " ", _normalFont);
							PdfReports.AddText2Table(table, item.EH_Tons_Moved.ToString("#,#.0000;(#,#.0000)"), _normalFont);
							if (isCumulative) {
								ytdEHAmountMoved += item.EH_Amt_Moved;
							} else {
								ytdEHAmountMoved = item.YtdEhAmtMoved;
							}
							PdfReports.AddText2Table(table, ytdEHAmountMoved.ToString("$#,#.00;$(#,#.00)"), _normalFont);
							PdfReports.AddText2Table(table, item.EH_Amt_Moved.ToString("$#,#.00;$(#,#.00)"), _normalFont);

							// Reduced Regular Harvest
							PdfReports.AddText2Table(table, "Reduced Regular Harvest Excess Beets", _labelFont, "left", 4);
							PdfReports.AddText2Table(table, " ", _normalFont);
							PdfReports.AddText2Table(table, item.RH_Tons_Moved.ToString("#,#.0000;(#,#.0000)"), _normalFont);
							if (isCumulative) {
								ytdRHAmountMoved += item.RH_Amt_Moved;
							} else {
								ytdRHAmountMoved = item.YtdRhAmtMoved;
							}
							PdfReports.AddText2Table(table, ytdRHAmountMoved.ToString("$#,#.00;$(#,#.00)"), _normalFont);
							PdfReports.AddText2Table(table, item.RH_Amt_Moved.ToString("$#,#.00;$(#,#.00)"), _normalFont);

							PdfReports.AddText2Table(table, " ", _normalFont, 8);

                            // Total lines
                            PdfReports.AddText2Table(table, " ", _normalFont, 4);
                            PdfReports.AddText2Table(table, "Total Gross Amount: ", _labelFont, "right", 2);
                            PdfReports.AddText2Table(table, (ytdEHBonus 
								+ ytdEHAmount 
								+ ytdRHAmount
								+ ytdEHAmountMoved
								+ ytdRHAmountMoved).ToString("#,##0.00"), _normalFont);
                            PdfReports.AddText2Table(table, (item.EH_Bonus  
								+ item.EH_Gross_Pay  
								+ item.RH_Gross_Pay
								//+ item.EH_Amt_Moved
								//+ item.RH_Amt_Moved
								).ToString("#,##0.00"), _normalFont);

                            PdfReports.AddText2Table(table, " ", _normalFont, 5);
                            PdfReports.AddText2Table(table, "Deduction Total: ", _labelFont, "right");

                            if (isCumulative) {
                                ytdDeductions += item.Deduct_Total;
                            } else {
                                ytdDeductions = item.YtdDeductTotal;
                            }
                            PdfReports.AddText2Table(table, ytdDeductions.ToString("#,##0.00"), _normalFont);
                            PdfReports.AddText2Table(table, item.Deduct_Total.ToString("#,##0.00"), _normalFont);

                            PdfReports.AddText2Table(table, " ", _normalFont, 5);
                            PdfReports.AddText2Table(table, "Net Payment: ", _labelFont, "right");

                            decimal currentNet = item.EH_Bonus + item.EH_Gross_Pay + item.RH_Gross_Pay - item.Deduct_Total;

                            if (isCumulative) {
                                ytdNet += currentNet;
                            } else {
                                ytdNet = item.YtdEhBonus + item.YtdEhGrossPay + item.YtdRhGrossPay - item.YtdDeductTotal;
                            }

                            PdfReports.AddText2Table(table, ytdNet.ToString("#,##0.00"), _normalFont);
                            PdfReports.AddText2Table(table, currentNet.ToString("#,##0.00"), _normalFont);

                            PdfReports.AddText2Table(table, " ", _normalFont, 8);
                            PdfReports.AddText2Table(table, " ", _normalFont, 6);
                            PdfReports.AddText2Table(table, "Grower Net: ", _labelFont, "right");
                            PdfReports.AddText2Table(table, item.Payment_Amount.ToString("#,##0.00"), _normalFont);

                            PdfReports.AddText2Table(table, " ", _normalFont, 6);
                            PdfReports.AddText2Table(table, "Landowner Net: ", _labelFont, "right");
                            PdfReports.AddText2Table(table, item.Split_Payment.ToString("#,##0.00"), _normalFont);

                            PdfReports.AddText2Table(table, " ", _normalFont, 8);

                            PdfReports.AddTableNoSplit(document, pgEvent, table);

                            if (paymentNumber == item.Payment_Number) {

                                //================================================================
                                // Add Deduction information
                                //================================================================
                                table = PdfReports.CreateTable(_primaryTableLayout, 1);

								//using (SqlDataReader drDed = WSCPayment.GetTransmittalDeduction(conn,
								//          cropYear, contractNumber, item.Payment_Number, isCumulative)) {

                                    PdfReports.AddText2Table(table, "Deduction", _labelFont, 5);
                                    PdfReports.AddText2Table(table, "Payment", _labelFont);
                                    PdfReports.AddText2Table(table, "Amount", _labelFont);
                                    PdfReports.AddText2Table(table, " ", _labelFont);
									
									var contractDeductions = from deduction in deductionList
															 where deduction.Contract_Number.ToString() == item.Contract_Number
															 && deduction.Payment_Number <= item.Payment_Number
															 orderby deduction.Payment_Number, deduction.Deduction_Number
															 select deduction;

									foreach (TransDeductionListItem dedItem in contractDeductions) {

										if (dedItem.Amount != 0) {
											PdfReports.AddText2Table(table, dedItem.Deduction_Desc, _normalFont, 5);
											PdfReports.AddText2Table(table, dedItem.Payment_Description, _normalFont);
											PdfReports.AddText2Table(table, dedItem.Amount.ToString("$#,##0.00"), _normalFont);
											PdfReports.AddText2Table(table, " ", _labelFont);
										}
									}


									// OLD CODE - replace by For loop above
									//while (drDed.Read()) {

									//    PdfReports.AddText2Table(table, drDed.GetString(drDed.GetOrdinal("Deduction_Desc")), _normalFont, 5);
									//    PdfReports.AddText2Table(table, drDed.GetString(drDed.GetOrdinal("Payment_Description")), _normalFont);
									//    PdfReports.AddText2Table(table, drDed.GetDecimal(drDed.GetOrdinal("Amount")).ToString("$#,##0.00"), _normalFont);
									//    PdfReports.AddText2Table(table, " ", _labelFont);
									//}
                                    PdfReports.AddText2Table(table, " ", _normalFont, 8);

                                    PdfReports.AddTableNoSplit(document, pgEvent, table);
                                //}

                                //================================================================
                                // Add Delivery information
                                //================================================================
                                float[] deliveryLayout = new float[] { 110.8F, 110.8F, 55.4F, 110.8F, 55.4F, 55.4F, 55.4F };
                                table = PdfReports.CreateTable(deliveryLayout, 1);

                                List<TransmittalDeliveryItem> deliveryList = WSCPayment.GetTransmittalDelivery(cropYear, contractNumber, paymentNumber, null, null);

                                PdfReports.AddText2Table(table, "Delivery Date", _labelFont);
                                PdfReports.AddText2Table(table, "First Net Pounds", _labelFont);
                                PdfReports.AddText2Table(table, "Tare %", _labelFont);
                                PdfReports.AddText2Table(table, "Final Net Lbs", _labelFont);
                                PdfReports.AddText2Table(table, "Sugar %", _labelFont);
                                PdfReports.AddText2Table(table, "SLM", _labelFont);
                                PdfReports.AddText2Table(table, "Loads", _labelFont);

								foreach (TransmittalDeliveryItem deliveryDay in deliveryList) {

                                    PdfReports.AddText2Table(table, deliveryDay.Delivery_Date.ToShortDateString(), _normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.First_Net_Pounds.ToString("###,###"), _normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.Tare.ToString("N2"), _normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.Final_Net_Pounds.ToString("###,###"), _normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.Sugar_Content.ToString("N2"), _normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.SLM_Pct.ToString("N4"), _normalFont);
                                    PdfReports.AddText2Table(table, deliveryDay.Loads.ToString(), _normalFont);
                                }
                                PdfReports.AddText2Table(table, " ", _normalFont, 6);
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

    internal class GroTransmittalEvent : PdfPageEventHelper, ICustomPageEvent {

        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _sub_titleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

        // This is the contentbyte object of the writer
        PdfContentByte _cb;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont _bf = null;
        ColumnText _ct = null;

        private bool _isDocumentClosing = false;
        private float _headerBottomYLine;
        private int _pageNumber = 0;
        private int _lastPageNumber = 0;

        private string _title = "";
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

            _bf = _normalFont.GetCalculatedBaseFont(false);
            _cb = writer.DirectContent;
            _ct = new ColumnText(_cb);
            base.OnOpenDocument(writer, document);
        }

        // we override the onEndPage method
        public override void OnEndPage(PdfWriter writer, Document document) {

            _lastPageNumber++;
            string text = "Page " + _lastPageNumber.ToString();
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
                _ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY,
                    PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
                    PortraitPageSize.PgLeading, Element.ALIGN_CENTER);
                _ct.YLine = PortraitPageSize.HdrTopYLine;

                // =======================================================
                // Add Header
                // =======================================================
                float[] layout = new float[] { 413F, 127F };
                PdfPTable table = PdfReports.CreateTable(layout, 0);
                Paragraph p = null;

                PdfReports.AddText2Table(table, _title + "\n" +
                    _paymentDesc + " - " + _cropYear + " Crop " + (_statementDate.Length > 0 ? "- " : "") +
                    _statementDate, _titleFont, "center");

                if (_pageNumber == 1) {                   
                    PdfReports.AddImage2Table(table, _imgLogo);
                } else {
                    PdfReports.AddText2Table(table, " ", _normalFont);
                }
                PdfReports.AddTableNoSplit(document, this, table);

                // Add blank lines
                layout = new float[] { 50F, 270F, 220F };
                table = PdfReports.CreateTable(layout, 0);

                PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                PdfReports.AddText2Table(table, " ", FontFactory.GetFont("HELVETICA", 7F, Font.NORMAL), layout.Length);

                if (_pageNumber == 1) {

                    string csz = _city + ", " + _state + " " + _postalCode;
                    p = PdfReports.GetAddressBlock(_growerName, _adr1, _adr2,
                        csz, 0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, _uspsFont);
                    PdfReports.AddText2Table(table, " ", _uspsFont);
                    PdfReports.AddText2Table(table, p);

                    PdfReports.AddText2Table(table, _contractNumber.ToString() + "\n" +
                        _factoryName + "\n" +
                        _stationName, _uspsFont, "right");
                    PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                    PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                   
                } else {

                    PdfReports.AddText2Table(table, " ", _uspsFont);
                    PdfReports.AddText2Table(table, _growerName, _uspsFont);

                    PdfReports.AddText2Table(table, _factoryName + "\n" +
                        _stationName, _uspsFont, "right");
                    PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                    PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                }

                PdfReports.AddTableNoSplit(document, this, table);
                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

		public void FillEvent(TransmittalPaymentItem payItem, int cropYear, string statementDate, int pageNumber, string title, string paymentDescription, iTextSharp.text.Image imgLogo) {

            _paymentDesc = paymentDescription;
            _cropYear = cropYear.ToString();
            if (statementDate.Length > 0) {
                _statementDate = DateTime.Parse(statementDate).ToString("MMMM dd, yyyy");
            } else {
                _statementDate = statementDate;
            }

            _growerName = payItem.Business_Name;
            _adr1 = payItem.Address_1;
            _adr2 = payItem.Address_2;
            _city = payItem.City;
            _state = payItem.State;
            _postalCode = payItem.Zip;
            _contractNumber = payItem.Contract_Number;
            _factoryName = payItem.Factory_Name;
            _stationName = payItem.Station_Name;
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
