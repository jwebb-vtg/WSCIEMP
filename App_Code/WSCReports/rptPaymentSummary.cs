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
    /// Summary description for rptPaymentSummary.
    /// </summary>
    public class rptPaymentSummary {

        private static float[] _primaryTableLayout = new float[] { 37.8F, 110.0F, 15.8F, 35.5F, 35.5F, 30F, 38.3F, 56.5F, 32.5F, 50.1F, 52.5F, 53F, 6.5F };
        private static string[] _detailSectionHdrNames = new string[] {" ", " ", " ", " ", "%", "%", "Price", " ", "EH", " ", " ", " ", " ", 
            " ", " ", " ", "SLM", "Sugar", "Paid", "Per Ton", "Tons", "Prem", "Gross", "Deductions", "Net", " "};

        public static string ReportPackager(
            int cropYear, string statementDate, string shid, string fromShid, string toShid, int paymentDescID,
            bool isCumulative, string footerText, string fileName, string logoUrl, string pdfTempfolder) {

            const string METHOD_NAME = "rptPaymentSummary.ReportPackager: ";
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

                    // Because this report is built by data in one data set driving the selection of
                    // data in a dependent dataset, all the data retrival has to be driven down to the
                    // ReportBuilder.  This is not the typical situation.
                    if (shid.Length == 0 && fromShid.Length == 0 && toShid.Length == 0) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter SHID information before requesting a report.");
                        throw (warn);
                    } else {

                        if (shid.Length > 0 && (fromShid.Length > 0 || toShid.Length > 0)) {
                            WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a single SHID or a SHID range, but not both.");
                            throw (warn);
                        }


						List<SHPaySumListItem> stateList = WSCPayment.GetPaymentSummary(ConfigurationManager.ConnectionStrings["BeetConn"].ToString(), 
							cropYear, shid, fromShid, toShid, paymentDescID, isCumulative);

						if (stateList.Count != 0) {
							using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
								ReportBuilder(stateList, cropYear, statementDate, shid, fromShid, toShid, paymentDescID, isCumulative, footerText, logoUrl, fs);
							}
						} else {
							WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("SHID does not have the required transmittal records or rollup records to create a Payment Summary report.");
							throw (warn);
						}
                    }
                }
                catch (System.Exception ex) {
                    string errMsg = "cropYear: " + cropYear.ToString() + "; " +
                        "SHID: " + shid + "; " +
                        "From SHID: " + fromShid + "; " +
                        "To SHID: " + toShid + "; " +
						"filePath: " + filePath + "; " +
                        "Payment Desc ID: " + paymentDescID.ToString();

                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                    throw (wscEx);
                }

                return filePath;
            }
            catch (System.Exception ex) {
                string errMsg = "cropYear: " + cropYear.ToString() + "; " +
                    "SHID: " + shid + "; " +
                    "From SHID: " + fromShid + "; " +
                    "To SHID: " + toShid + "; " +
                    "Payment Desc ID: " + paymentDescID.ToString();

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                throw (wscEx);
            }
        }

		private static void ReportBuilder(List<SHPaySumListItem> stateList, int cropYear, string statementDate, string shid,
            string fromShid, string toShid, int paymentDescID, bool isCumulative, string rptFooter, string logoUrl, FileStream fs) {

            const string METHOD_NAME = "rptPaymentSummary.ReportBuilder: ";
            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            PaymentSummaryEvent pgEvent = null;
            iTextSharp.text.Image imgLogo = null;

            decimal totalTons = 0;
            decimal totalEHPrem = 0;
            decimal totalGross = 0;
            decimal totalDeductions = 0;
            decimal totalNet = 0;
            decimal totalGrowerNet = 0;
            decimal totalLandownerNet = 0;
            decimal checkAmount = 0;
            int resetFlag = 0;
            int checkSequence = 0;
            int payeeNumber = 0;

            string rptTitle = "Western Sugar Cooperative Payment Summary";

            Font headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
            Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

            try {

				int firstContractNumber = stateList.Min(c => c.i_ContractNumber);
				int lastContractNumber = stateList.Max(c => c.i_ContractNumber);

				List<TransDeductionListItem> deductionList = WSCPayment.GetTransmittalDeduction2(ConfigurationManager.ConnectionStrings["BeetConn"].ToString(),
					cropYear, paymentDescID, 0, firstContractNumber, lastContractNumber, isCumulative);

				foreach (SHPaySumListItem item in stateList) {

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
						checkSequence = item.i_CheckSequence;
						pgEvent = new PaymentSummaryEvent();
						pgEvent.FillEvent(item, cropYear, statementDate, resetFlag, rptTitle, imgLogo);

						writer.PageEvent = pgEvent;
						pgEvent.HeaderNameList = _detailSectionHdrNames;
						pgEvent.HeaderTableLayout = _primaryTableLayout;

						// Open the document
						document.Open();
						pgEvent.HeaderTableLayout = null;

						table = PdfReports.CreateTable(_primaryTableLayout, 1);
						checkAmount = item.d_checkAmount;
						payeeNumber = item.i_Payee_Number;

					} else {

						if (checkSequence != item.i_CheckSequence) {

							AddTotals(ref writer, ref document, ref table, labelFont, normalFont, totalTons,
								totalEHPrem, totalGross, totalDeductions, totalNet, checkAmount,
								payeeNumber, totalGrowerNet, totalLandownerNet, pgEvent);
							AddFooter(ref writer, ref document, normalFont, rptFooter, pgEvent);

							// Clear totals values
							totalTons = 0;
							totalEHPrem = 0;
							totalGross = 0;
							totalDeductions = 0;
							totalNet = 0;
							totalGrowerNet = 0;
							totalLandownerNet = 0;

							checkSequence = item.i_CheckSequence;

							// NEW CHECK
							pgEvent.FillEvent(item, cropYear, statementDate, resetFlag, rptTitle, imgLogo);
							pgEvent.HeaderTableLayout = _primaryTableLayout;

							document.NewPage();
							pgEvent.HeaderTableLayout = null;

							table = PdfReports.CreateTable(_primaryTableLayout, 1);
							checkAmount = item.d_checkAmount;
							payeeNumber = item.i_Payee_Number;

							//AddDetailSectionHdr(ref table, labelFont, normalFont);
						}
					}

					// =======================================================
					// Contract Number Line
					// =======================================================
					PdfReports.AddText2Table(table, "Contract", labelFont, "left");
					PdfReports.AddText2Table(table, item.i_ContractNumber.ToString(), labelFont, "center");
					PdfReports.AddText2Table(table, " ", normalFont);
					PdfReports.AddText2Table(table, item.d_Avg_SLM.ToString("N4"), normalFont, "right");

					PdfReports.AddText2Table(table, " ", normalFont, 3);
					PdfReports.AddText2Table(table, item.d_EH_Bonus.ToString("N2"), normalFont, "right", 2);
					totalEHPrem += item.d_EH_Bonus;
					PdfReports.AddText2Table(table, " ", normalFont, 4);

					// =======================================================
					// Station Name Line
					// =======================================================
					PdfReports.AddText2Table(table, "Station", normalFont);
					PdfReports.AddText2Table(table, item.s_Station_Name, normalFont, "center");
					PdfReports.AddText2Table(table, "EH", normalFont, "center");
					PdfReports.AddText2Table(table, item.d_EH_SLM.ToString("N4"), normalFont, "right");
					PdfReports.AddText2Table(table, item.d_EH_Sugar.ToString("N2"), normalFont, "right");
					PdfReports.AddText2Table(table, item.d_EH_Paid.ToString("N3"), normalFont, "right");
					PdfReports.AddText2Table(table, item.d_EH_Price.ToString("N3"), normalFont, "right");

					PdfReports.AddText2Table(table, (item.d_EH_Tons - item.d_EH_tons_moved).ToString("N4"), normalFont, "right");
					totalTons += item.d_EH_Tons;

					PdfReports.AddText2Table(table, " ", normalFont);

					PdfReports.AddText2Table(table, (item.d_EH_Gross_Pay - item.d_EH_amt_moved).ToString("N2"), normalFont, "right");
					totalGross += item.d_EH_Gross_Pay;

					PdfReports.AddText2Table(table, " ", normalFont, 3);

					// =======================================================
					// Landowner Name Line
					// =======================================================
					PdfReports.AddText2Table(table, "LO", normalFont);
					PdfReports.AddText2Table(table, item.s_LOName, normalFont, "center");
					PdfReports.AddText2Table(table, "RH", normalFont, "center");

					PdfReports.AddText2Table(table, item.d_RH_SLM.ToString("N4"), normalFont, "right");
					PdfReports.AddText2Table(table, item.d_RH_Sugar.ToString("N2"), normalFont, "right");
					PdfReports.AddText2Table(table, item.d_RH_Paid.ToString("N3"), normalFont, "right");
					PdfReports.AddText2Table(table, item.d_RH_Price.ToString("N3"), normalFont, "right");

					PdfReports.AddText2Table(table, (item.d_RH_Tons - item.d_RH_tons_moved).ToString("N4"), normalFont, "right");
					totalTons += item.d_RH_Tons;

					PdfReports.AddText2Table(table, " ", normalFont);

					PdfReports.AddText2Table(table, (item.d_RH_Gross_Pay - item.d_RH_amt_moved).ToString("N2"), normalFont, "right");
					totalGross += item.d_RH_Gross_Pay;

					PdfReports.AddText2Table(table, item.d_Deduct_Total.ToString("N2"), normalFont, "right");
					totalDeductions += item.d_Deduct_Total;

					PdfReports.AddText2Table(table, (item.d_Total_Net - item.d_EH_amt_moved - item.d_RH_amt_moved).ToString("N2"), normalFont, "right");
					totalNet += item.d_Total_Net;

					PdfReports.AddText2Table(table, " ", normalFont);

					// =======================================================
					// Reduced for Excess Beets
					// =======================================================
					// Reduced Early Harvest
					PdfReports.AddText2Table(table, "Reduced Early Harvest Excess Beets", normalFont, 3);
					PdfReports.AddText2Table(table, " ", normalFont, "right");
					PdfReports.AddText2Table(table, " ", normalFont, "right");
					PdfReports.AddText2Table(table, " ", normalFont, "right");
					PdfReports.AddText2Table(table, " ", normalFont, "right");

					PdfReports.AddText2Table(table, item.d_EH_tons_moved.ToString("#,#.0000;(#,#.0000)"), normalFont, "right");
					//totalTons += item.d_RH_Tons;

					PdfReports.AddText2Table(table, " ", normalFont);

					PdfReports.AddText2Table(table, item.d_EH_amt_moved.ToString("#,#.00;(#,#.00)"), normalFont, "right");
					//totalGross += item.d_RH_Gross_Pay;

					PdfReports.AddText2Table(table, " ", normalFont, "right");
					//totalDeductions += item.d_Deduct_Total;

					PdfReports.AddText2Table(table, item.d_EH_amt_moved.ToString("#,#.00;(#,#.00)"), normalFont, "right");
					//totalNet += item.d_Total_Net;

					PdfReports.AddText2Table(table, " ", normalFont);

					// Reduced Regular Harvest
					PdfReports.AddText2Table(table, "Reduced Regular Harvest  Excess Beets", normalFont, 3);
					PdfReports.AddText2Table(table, " ", normalFont, "right");
					PdfReports.AddText2Table(table, " ", normalFont, "right");
					PdfReports.AddText2Table(table, " ", normalFont, "right");
					PdfReports.AddText2Table(table, " ", normalFont, "right");

					PdfReports.AddText2Table(table, item.d_RH_tons_moved.ToString("#,#.0000;(#,#.0000)"), normalFont, "right");
					//totalTons += item.d_RH_Tons;

					PdfReports.AddText2Table(table, " ", normalFont);

					PdfReports.AddText2Table(table, item.d_RH_amt_moved.ToString("#,#.00;(#,#.00)"), normalFont, "right");
					//totalGross += item.d_RH_Gross_Pay;

					PdfReports.AddText2Table(table, " ", normalFont, "right");
					//totalDeductions += item.d_Deduct_Total;

					PdfReports.AddText2Table(table, item.d_RH_amt_moved.ToString("#,#.00;(#,#.00)"), normalFont, "right");
					//totalNet += item.d_Total_Net;

					PdfReports.AddText2Table(table, " ", normalFont);

					// =======================================================
					// Grower / Landowner NET Split
					// =======================================================
					PdfReports.AddText2Table(table, " ", normalFont, 9);
					PdfReports.AddText2Table(table, "Grower Net", labelFont, 2);
					totalGrowerNet += item.d_groAmount;
					PdfReports.AddText2Table(table, item.d_groAmount.ToString("N2"), normalFont, "right");
					PdfReports.AddText2Table(table, " ", normalFont);

					PdfReports.AddText2Table(table, " ", normalFont, 9);
					PdfReports.AddText2Table(table, "Landowner Net", labelFont, 2);
					totalLandownerNet += item.d_ldoAmount;
					PdfReports.AddText2Table(table, item.d_ldoAmount.ToString("N2"), normalFont, "right");
					PdfReports.AddText2Table(table, " ", normalFont);

					// BLANK LINE
					PdfReports.AddText2Table(table, " ", normalFont, 13);

					pgEvent.HeaderTableLayout = _primaryTableLayout;
					PdfReports.AddTableNoSplit(document, pgEvent, table);
					pgEvent.HeaderTableLayout = null;

					//================================================================
					// Add Deduction information
					//================================================================

					table = PdfReports.CreateTable(_primaryTableLayout, 1);

					PdfReports.AddText2Table(table, " ", labelFont, 4);
					PdfReports.AddText2Table(table, "Deduction", labelFont, "left", 4);
					PdfReports.AddText2Table(table, "Payment", labelFont, "left", 2);
					PdfReports.AddText2Table(table, "Amount", labelFont, "center");
					PdfReports.AddText2Table(table, " ", labelFont, 2);

					var contractDeductions = from deduction in deductionList
											where deduction.Contract_Number == item.i_ContractNumber
											&& deduction.Payment_Number <= item.i_PaymentNumber
											orderby deduction.Payment_Number, deduction.Deduction_Number
											select deduction;

					foreach(TransDeductionListItem dedItem in contractDeductions) {

						if (dedItem.Amount != 0) {
							PdfReports.AddText2Table(table, " ", labelFont, 4);
							PdfReports.AddText2Table(table, dedItem.Deduction_Desc, normalFont, "left", 4);
							PdfReports.AddText2Table(table, dedItem.Payment_Description, normalFont, "left", 2);
							PdfReports.AddText2Table(table, dedItem.Amount.ToString("$#,##0.00"), normalFont, "right");
							PdfReports.AddText2Table(table, " ", labelFont, 2);
						}
					}

					PdfReports.AddText2Table(table, " ", normalFont, 13);
				}

				// BLANK LINE
				PdfReports.AddText2Table(table, " ", normalFont, 13);
				PdfReports.AddTableNoSplit(document, pgEvent, table);

				table = PdfReports.CreateTable(_primaryTableLayout, 1);

                // ======================================================
                // Close document
                // ======================================================
                if (document != null) {

                    table = PdfReports.CreateTable(_primaryTableLayout, 1);

                    AddTotals(ref writer, ref document, ref table, labelFont, normalFont, totalTons,
                        totalEHPrem, totalGross, totalDeductions, totalNet, checkAmount,
                        payeeNumber, totalGrowerNet, totalLandownerNet, pgEvent);

                    AddFooter(ref writer, ref document, normalFont, rptFooter, pgEvent);

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

        private static void AddTotals(ref PdfWriter writer, ref Document document,
            ref PdfPTable table, Font labelFont, Font normalFont,
            decimal totalTons, decimal totalEHPrem, decimal totalGross, decimal totalDeductions,
            decimal totalNet, decimal checkAmount, int payeeNumber,
            decimal totalGrowerNet, decimal totalLandownerNet, PaymentSummaryEvent pgEvent) {

            decimal growerAmt = 0;
            decimal landownerAmt = 0;

            growerAmt = totalGrowerNet;
            landownerAmt = totalLandownerNet;

            // ===============================================================================
            // Total Line: totalTons, totalEHPrem, totalGross, totalDeductions, totalNet
            // ===============================================================================
            PdfReports.AddText2Table(table, " ", normalFont, 13);
            PdfReports.AddText2Table(table, " ", normalFont, 5);
            PdfReports.AddText2Table(table, "Tons", labelFont, "center", 2);
            PdfReports.AddText2Table(table, "EH", labelFont, "right", 2);
            PdfReports.AddText2Table(table, "Gross", labelFont, "center");
            PdfReports.AddText2Table(table, "Deductions", labelFont, "center");
            PdfReports.AddText2Table(table, "Net", labelFont, "center");
            PdfReports.AddText2Table(table, " ", normalFont);

            PdfReports.AddText2Table(table, " ", normalFont, 5);
            PdfReports.AddText2Table(table, totalTons.ToString("N4"), normalFont, "right", 2);
            PdfReports.AddText2Table(table, totalEHPrem.ToString("N2"), normalFont, "right", 2);
            PdfReports.AddText2Table(table, totalGross.ToString("N2"), normalFont, "right");
            PdfReports.AddText2Table(table, totalDeductions.ToString("N2"), normalFont, "right");
            PdfReports.AddText2Table(table, totalNet.ToString("N2"), normalFont, "right");
            PdfReports.AddText2Table(table, " ", normalFont);

            PdfReports.AddText2Table(table, " ", normalFont, 13);

            PdfReports.AddText2Table(table, " ", normalFont, 9);
            PdfReports.AddText2Table(table, "Grower Net", labelFont, 2);
            PdfReports.AddText2Table(table, growerAmt.ToString("N2"), normalFont, "right");
            PdfReports.AddText2Table(table, " ", normalFont);

            PdfReports.AddText2Table(table, " ", normalFont, 9);
            PdfReports.AddText2Table(table, "Landowner Net", labelFont, 2);
            PdfReports.AddText2Table(table, landownerAmt.ToString("N2"), normalFont, "right");
            PdfReports.AddText2Table(table, " ", normalFont);

            PdfReports.AddText2Table(table, " ", normalFont, 13);
            PdfReports.AddText2Table(table, " ", normalFont, 13);

            PdfReports.AddTableNoSplit(document, pgEvent, table);
        }

        private static void AddFooter(ref PdfWriter writer, ref Document document, Font normalFont, string rptFooter, PaymentSummaryEvent pgEvent) {

            float[] footerLayout = new float[] { 540 };
            PdfPTable table = PdfReports.CreateTable(footerLayout, 1);

            PdfReports.AddText2Table(table, rptFooter, normalFont, "center");
            PdfReports.AddTableNoSplit(document, pgEvent, table);
        }

        private static void AddDetailSectionHdr(ref PdfPTable table, Font labelFont, Font normalFont) {

            // Detail column headers (2 lines)
            PdfReports.AddText2Table(table, " ", labelFont, 4);
            PdfReports.AddText2Table(table, "%", labelFont, "center");
            PdfReports.AddText2Table(table, "%", labelFont, "center");
            PdfReports.AddText2Table(table, "Price", labelFont, "center");
            PdfReports.AddText2Table(table, " ", labelFont);
            PdfReports.AddText2Table(table, "EH", labelFont, "center");
            PdfReports.AddText2Table(table, " ", labelFont, 4);

            PdfReports.AddText2Table(table, " ", labelFont, 3);
            PdfReports.AddText2Table(table, "SLM", labelFont, "center");
            PdfReports.AddText2Table(table, "Sugar", labelFont, "center");
            PdfReports.AddText2Table(table, "Paid", labelFont, "center");
            PdfReports.AddText2Table(table, "Per Ton", labelFont, "center");
            PdfReports.AddText2Table(table, "Tons", labelFont, "center");
            PdfReports.AddText2Table(table, "Prem", labelFont, "center");
            PdfReports.AddText2Table(table, "Gross", labelFont, "center");
            PdfReports.AddText2Table(table, "Deductions", labelFont, "center");
            PdfReports.AddText2Table(table, "Net", labelFont, "center");
            PdfReports.AddText2Table(table, " ", labelFont, "center");
        }
    }

    public class PaymentSummaryEvent : PdfPageEventHelper, ICustomPageEvent {

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
        private string _shid = "";
        private decimal _checkAmount = 0;
        private string _paymentDesc = "";
        private string _cropYear = "";
        private string _statementDate = "";
        private iTextSharp.text.Image _imgLogo = null;
        private float[] _hdrTableLayout = null;
        private string[] _hdrNameList = null;

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
                    PortraitPageSize.PgLeading, Element.ALIGN_TOP);
                _ct.YLine = PortraitPageSize.HdrTopYLine;

                // =======================================================
                // Add Payment Header
                // =======================================================
                float[] layout = new float[] { 413F, 127F };
                PdfPTable table = PdfReports.CreateTable(layout, 0);
                PdfReports.AddText2Table(table, _title + "\n" +
                    _paymentDesc + " - " + _cropYear + " Crop " + (_statementDate.Length > 0 ? "- " : "") +
                    _statementDate, titleFont, "center");

                if (_pageNumber == 1) {
                    PdfReports.AddImage2Table(table, _imgLogo);
                } else {
                    PdfReports.AddText2Table(table, " ", normalFont);
                }
                PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);
                PdfReports.AddTableNoSplit(document, this, table);

                float[] headerLayout = new float[] { 31.5F, 326.4F, 44.1F, 91.5F, 46.5F };
                table = PdfReports.CreateTable(headerLayout, 1);

                // Add blank lines
                float[] shareholderLayout = new float[] { 50F, 270F, 220F };
                PdfPTable addrTable = PdfReports.CreateTable(shareholderLayout, 0);
                PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);
                PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);
                PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);

                if (_pageNumber == 1) {

                    string csz = _city + ", " + _state + " " + _postalCode;
                    Paragraph p = PdfReports.GetAddressBlock(_growerName, _adr1, _adr2,
                        csz, 0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, uspsFont);
                    PdfReports.AddText2Table(addrTable, " ", uspsFont);
                    PdfReports.AddText2Table(addrTable, p);

                    PdfReports.AddText2Table(addrTable, "SHID: " + _shid + "\n" +
                        "Amount: " + _checkAmount.ToString("N2"), uspsFont, "right");
                    PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);
                    PdfReports.AddTableNoSplit(document, this, addrTable);

                    PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);
                    PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);

                    PdfReports.AddText2Table(table, " ", normalFont);
                    PdfReports.AddText2Table(table, "EH - Early Harvest", normalFont, table.NumberOfColumns - 1);

                    PdfReports.AddText2Table(table, " ", normalFont);
                    PdfReports.AddText2Table(table, "RH - Regular Harvest", normalFont, table.NumberOfColumns - 1);
                    
                    PdfReports.AddText2Table(table, " ", normalFont);
                    PdfReports.AddText2Table(table, "SLM - Sugar Lost to Molasses", normalFont, table.NumberOfColumns - 1);

                    PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);

                } else {

                    PdfReports.AddText2Table(addrTable, " ", uspsFont);
                    PdfReports.AddText2Table(addrTable, _growerName, uspsFont);

                    PdfReports.AddText2Table(addrTable, "SHID: " + _shid + "\n" +
                        "Amount: " + _checkAmount.ToString("N2"), uspsFont, "right");
                    PdfReports.AddText2Table(addrTable, " ", normalFont, addrTable.NumberOfColumns);
                    PdfReports.AddTableNoSplit(document, this, addrTable);

                    PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);
                    PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);
                }

                PdfReports.AddTableNoSplit(document, this, table);

                if (_hdrTableLayout != null) {
                    PdfPTable hdrTab = PdfReports.CreateTable(_hdrTableLayout, 0);
                    PdfReports.FillHeaderLabels(ref hdrTab, _hdrNameList, labelFont);
                    PdfReports.AddTableNoSplit(document, this, hdrTab);
                }

                _headerBottomYLine = _ct.YLine;

            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(SHPaySumListItem item, int cropYear, string statementDate,
            int pageNumber, string title, iTextSharp.text.Image imgLogo) {

            _paymentDesc = item.s_PaymentDesc;
            _cropYear = cropYear.ToString();

            if (statementDate != null && statementDate.Length > 0) {
                _statementDate = DateTime.Parse(statementDate).ToString("MMMM dd, yyyy");
            } else {
                _statementDate = "";
            }

            _growerName = item.s_PayeeName;
            _adr1 = item.s_Address1;
			_adr2 = item.s_Address2;
            _city = item.s_City;
            _state = item.s_State;
            _postalCode = item.s_PostalCode;
            _shid = item.i_SHID.ToString();
            _checkAmount = item.d_checkAmount;
            _pageNumber = pageNumber;
            _title = title;
            _imgLogo = imgLogo;
        }

        public string[] HeaderNameList {
            get { return _hdrNameList; }
            set { _hdrNameList = value; }
        }
        public float[] HeaderTableLayout {
            get { return _hdrTableLayout; }
            set { _hdrTableLayout = value; }
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
            return _ct;
        }
    }
}
