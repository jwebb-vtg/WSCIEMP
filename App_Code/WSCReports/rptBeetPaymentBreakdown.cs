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

	public class rptBeetPaymentBreakdown {

		private static float[] _primaryTableLayout = new float[] { 55F, 55F, 55F, 130F, 70F, 88F, 87F };
		private static string[] _hdrNames = new string[] { "Calendar\nYear", "Crop\nYear", "Payment\nNumber", "Payment\nDescription", "Transmittal\nDate", "Gross\nAmount", "Net\nAmount" };

		public static string ReportPackager(int shid, int cropYear, int calYear, string fileName, string logoUrl, string pdfTempfolder) {

			const string METHOD_NAME = "rptBeetPaymentBreakdown.ReportPackager: ";
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

					using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
						ReportBuilder(shid, cropYear, calYear, logoUrl, fs);
					}
				}
				catch (System.Exception ex) {
					string errMsg = "cropYear: " + cropYear.ToString() + "; " +
						"calYear: " + calYear.ToString() + "; " +
						"SHID: " + shid + "; " +
						"filePath: " + filePath;

					WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
					throw (wscEx);
				}

				return filePath;
			}
			catch (System.Exception ex) {
				string errMsg = "cropYear: " + cropYear.ToString() + "; " +
					"calYear: " + calYear.ToString() + "; " +
					"SHID: " + shid + "; " +
					"filePath: " + filePath;

				WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
				throw (wscEx);
			}
		}

		private static void ReportBuilder(int shid, int cropYear, int calYear, string logoUrl, FileStream fs) {

			const string METHOD_NAME = "rptBeetPaymentBreakdown.ReportBuilder: ";
			Document document = null;
			PdfWriter writer = null;
			PdfPTable table = null;
			BeetPaymentBreakdownEvent pgEvent = null;
			iTextSharp.text.Image imgLogo = null;

			int curShid = 0;
			int lastShid = 0;
			decimal grossAmt = 0;
			decimal netAmt = 0;

			string rptTitle = "Western Sugar Cooperative Beet Payments by Year";

			Font headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
			Font normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
			Font labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);

			try {

				List<BeetPaymentListItem> stateList = WSCPayment.RptBeetPayBreakdown(ConfigurationManager.ConnectionStrings["BeetConn"].ToString(),
					shid, cropYear, calYear);

				foreach (BeetPaymentListItem item in stateList) {

					curShid = item.SHID;

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
						lastShid = curShid;
						grossAmt = 0;
						netAmt = 0;

						// Attach my override event handler(s)
						pgEvent = new BeetPaymentBreakdownEvent();
						pgEvent.FillEvent(_primaryTableLayout, _hdrNames, curShid, item.PayeeName, rptTitle, imgLogo);

						writer.PageEvent = pgEvent;

						// Open the document
						document.Open();

						table = PdfReports.CreateTable(_primaryTableLayout, 1);
					}

					if (curShid != lastShid) {

						// BLANK LINE
						PdfReports.AddText2Table(table, " ", normalFont, "center", 5);
						PdfReports.AddText2Table(table, grossAmt.ToString("c2"), labelFont, "right");
						PdfReports.AddText2Table(table, netAmt.ToString("c2"), labelFont, "right");

						PdfReports.AddText2Table(table, " ", normalFont, 13);
						PdfReports.AddTableNoSplit(document, pgEvent, table);

						lastShid = curShid;
						grossAmt = 0;
						netAmt = 0;
						pgEvent.FillEvent(_primaryTableLayout, _hdrNames, curShid, item.PayeeName, rptTitle, imgLogo);
						document.NewPage();

						table = PdfReports.CreateTable(_primaryTableLayout, 1);
					}

					PdfReports.AddText2Table(table, item.CalendarYear.ToString(), normalFont, "center");
					PdfReports.AddText2Table(table, item.CropYear.ToString(), normalFont, "center");
					PdfReports.AddText2Table(table, item.PaymentNumber.ToString("N0"), normalFont, "center");
					PdfReports.AddText2Table(table, item.PaymentDescription, normalFont, "left");
					PdfReports.AddText2Table(table, item.TransmittalDate.ToString("MM/dd/yyyy"), normalFont, "center");
					PdfReports.AddText2Table(table, item.GrossDollars.ToString("c2"), normalFont, "right");
					PdfReports.AddText2Table(table, item.PaymentAmount.ToString("c2"), normalFont, "right");
					grossAmt += item.GrossDollars;
					netAmt += item.PaymentAmount;
				}

				if (document != null) { 

					// BLANK LINE
					PdfReports.AddText2Table(table, " ", normalFont, "center", 5);
					PdfReports.AddText2Table(table, grossAmt.ToString("c2"), labelFont, "right");
					PdfReports.AddText2Table(table, netAmt.ToString("c2"), labelFont, "right");

					PdfReports.AddText2Table(table, " ", normalFont, 13);
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

	public class BeetPaymentBreakdownEvent : PdfPageEventHelper, ICustomPageEvent {

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
		private string _shid = "";
		private string _payeeName = "";
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

		public override void OnStartPage(PdfWriter writer, Document document) {

			if (!_isDocumentClosing) {

				// ===========================================================================
				// Create header column -- in this report this is the page's column object
				// ===========================================================================
				_ct.SetSimpleColumn(PortraitPageSize.HdrLowerLeftX, PortraitPageSize.HdrLowerLeftY,
					PortraitPageSize.HdrUpperRightX, PortraitPageSize.HdrUpperRightY,
					PortraitPageSize.PgLeading, Element.ALIGN_TOP);
				_ct.YLine = PortraitPageSize.HdrTopYLine;

				// =======================================================
				// Add Header
				// =======================================================
				float[] layout = new float[] { 413F, 127F };
				PdfPTable table = PdfReports.CreateTable(layout, 0);
				PdfReports.AddText2Table(table, _title + "\n", titleFont, "center");
				PdfReports.AddImage2Table(table, _imgLogo);
				PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);
				PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);

				PdfReports.AddText2Table(table, "SHID: " + _shid, labelFont, "left", 2);
				PdfReports.AddText2Table(table, _payeeName, labelFont, "left", 2);
				PdfReports.AddText2Table(table, " ", normalFont, table.NumberOfColumns);

				PdfReports.AddTableNoSplit(document, this, table);

				PdfPTable hdrTab = PdfReports.CreateTable(_hdrTableLayout, 0);
				PdfReports.AddText2Table(hdrTab, _hdrNameList[0], labelFont, "center");
				PdfReports.AddText2Table(hdrTab, _hdrNameList[1], labelFont, "center");
				PdfReports.AddText2Table(hdrTab, _hdrNameList[2], labelFont, "center");
				PdfReports.AddText2Table(hdrTab, _hdrNameList[3], labelFont, "center");
				PdfReports.AddText2Table(hdrTab, _hdrNameList[4], labelFont, "center");
				PdfReports.AddText2Table(hdrTab, _hdrNameList[5], labelFont, "center");
				PdfReports.AddText2Table(hdrTab, _hdrNameList[6], labelFont, "center");

				PdfReports.AddTableNoSplit(document, this, hdrTab);

				_headerBottomYLine = _ct.YLine;

			}

			base.OnStartPage(writer, document);
		}

		public void FillEvent(float[] hdrTableLayout, string[] hdrNameList, int shid, string payeeName, string title, iTextSharp.text.Image imgLogo) {

			_hdrTableLayout = hdrTableLayout;
			_hdrNameList = hdrNameList;
			_payeeName = payeeName;
			_shid = shid.ToString();
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
