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
using Color = iTextSharp.text.BaseColor;

namespace WSCReports {

	public class rptNoticeOfPassthrough {

		private const string MOD_NAME = "WSCReports.rptNoticeOfPassthrough.";

		private static float[] _bodyLayout = new float[] { 40F, 460F, 40F };
		private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
		private static Font _normalFont = FontFactory.GetFont("HELVETICA", 11F, Font.NORMAL);
		private static Font _subFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
		private static Font _labelFont = FontFactory.GetFont("HELVETICA", 11F, Font.BOLD);
		private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);

		private const string HEADER_DATE_FORMAT = "MMMM dd, yyyy";

		public static string ReportPackager(int cropYear, string shid, string fromShid, 
			string toShid, string fileName, string logoUrl, string pdfTempfolder) {

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

					ReportBuilder(cropYear, shid, fromShid, toShid, filePath, logoUrl);
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

		private static void ReportBuilder(int cropYear, string shid, string fromShid, string toShid, string filePath, string logoUrl) {

			const string METHOD_NAME = "ReportBuilder";
			Document document = null;
			PdfWriter writer = null;
			iTextSharp.text.Image imgLogo = null;			

			NoticeOfPassthroughEvent pgEvent = null;

			string rptTitle = "NOTICE OF PASSTHROUGH OF DOMESTIC PRODUCTION ACTIVITIES\n"
				+"DEDUCTION FROM THE WESTERN SUGAR COOPERATIVE TO PATRONS  ";

            try {

                List<ListNoticeOfPassthrough> stateList = WSCReportsExec.PassthroughGetBySHID(cropYear, shid, fromShid, toShid);

				if (stateList.Count > 0) {
					using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {

						foreach (ListNoticeOfPassthrough state in stateList) {

							if (document == null) {

								// IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
								//  ***  US LETTER: 612 X 792  ***
								//document = new Document(iTextSharp.text.PageSize.LETTER, 36, 36, 54, 72);
								document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
									PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

								// we create a writer that listens to the document
								// and directs a PDF-stream to a file				
								writer = PdfWriter.GetInstance(document, fs);
								;
								imgLogo = PdfReports.GetImage(logoUrl, 127, 50, iTextSharp.text.Element.ALIGN_CENTER);

								// Attach my override event handler(s)
								pgEvent = new NoticeOfPassthroughEvent();
								pgEvent.FillEvent(state, 0, rptTitle, imgLogo);

								writer.PageEvent = pgEvent;

								// Open the document
								document.Open();
							} else {

								pgEvent.FillEvent(state, 0, rptTitle, imgLogo);
								document.NewPage();
							}

							// ======================================================
							// Fill in body of letter
							// ======================================================
							PdfPTable table = PdfReports.CreateTable(_bodyLayout, 0);

							Paragraph para = new Paragraph("The purpose of this notice  is to inform you of your allocable share of the domestic production activities "
								+ "deduction (also known as the Section 199 deduction) which The Western Sugar Cooperative (“WSC”) is passing "
								+ "through to patrons.  The deduction will be reported on your " + state.TaxYear.ToString() + " Form 1099-PATR.", 
								_normalFont);
							PdfReports.AddText2Table(table, para, _bodyLayout.Length);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);
							para = new Paragraph("Your share of the passthrough domestic production activities deduction amount is detailed below:", 
								_normalFont);
							PdfReports.AddText2Table(table, para, _bodyLayout.Length);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);

							PdfReports.AddText2Table(table, " ", _normalFont);							
							PdfReports.AddText2Table(table, "Domestic Production Activities Deduction Allocated to Patron", _labelFont);
							PdfReports.AddText2Table(table, " ", _normalFont);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);

							PdfReports.AddText2Table(table, " ", _normalFont);
							para = new Paragraph("SHID: " + state.SHID + "\n"
								+ "Member Name: " + state.MemberName + "\n"
								+ "Patron’s share of WSC Domestic Production Activities Deduction: $"
									+ state.PatronShareOfDeduction.ToString("#,###.00"), _normalFont);
							PdfReports.AddText2Table(table, para, 2);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);

							PdfReports.AddText2Table(table, "Explanation", _labelFont, _bodyLayout.Length);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);
							para = new Paragraph("Created by Congress, the domestic production activities deduction has been in effect since 2005.  It is designed "
								+ "to encourage businesses like WSC to engage in production and/or manufacturing activities in the United States "
								+ "and to employ workers in those activities.", 
								_normalFont);
							PdfReports.AddText2Table(table, para, _bodyLayout.Length);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);
							para = new Paragraph("Section 199(d)(3) of the Internal Revenue Code contains special rules governing how cooperatives and their "
								+ "patrons must compute the deduction.  The Code permits a cooperative to elect to pass through to patrons all or a "
								+ "portion of its domestic production activities deduction.  Cooperatives typically pass through to patrons whatever "
								+ "part of the deduction they can not themselves use.",
								_normalFont);
							PdfReports.AddText2Table(table, para, _bodyLayout.Length);

							string pctToApply = "All";
							if (state.PercentageToApply != 100) {
								pctToApply = state.PercentageToApply.ToString("N3");
								if (pctToApply.EndsWith("000")) {
									pctToApply = state.PercentageToApply.ToString("N0");
								}
								pctToApply += "%";
							}
							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);

							para = new Paragraph("WSC is passing through to its patrons " + pctToApply + " of the domestic production activities deduction it earned "
								+ "for the fiscal year ending " + state.FiscalYearEndDate.ToShortDateString() + ".  The passthrough is being allocated to patrons based on sugar "
								+ "beet tonnage for Crop Year " + state.CropYear.ToString() 
								+ " (fiscal year " + state.FiscalYear.ToString() + ").  This notice identifies your share of the "
								+ "passthrough. ",
								_normalFont);
							PdfReports.AddText2Table(table, para, _bodyLayout.Length);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);
							para = new Paragraph("A domestic production activities deduction passed through to a patron can generally be used to reduce the "
								+ "patron’s taxable income.  In order to claim this passed-through deduction, a patron must include a Form 8903 "
								+ "(Domestic Production Activities Deduction) with the patron’s income tax return and report the passthrough "
								+ "deduction on the appropriate line of that form.",
								_normalFont);
							PdfReports.AddText2Table(table, para, _bodyLayout.Length);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);
							PdfReports.AddText2Table(table, " ", _normalFont);
							PdfReports.AddText2Table(table, "We strongly advise you consult with your tax advisor to determine how to take\n"
								+ "advantage of this potential tax benefit.", _labelFont, "center");
							PdfReports.AddText2Table(table, " ", _normalFont);

							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);
							PdfReports.AddText2Table(table, " ", _subFont, _bodyLayout.Length);

							para = new Paragraph("______________________________\n\n", _subFont);
							Chunk ck = new Chunk("1", _subFont);
							ck.SetTextRise(5);
							para.Add(ck);
							Chunk chunk = new Chunk(" This notice is issued pursuant to Section 199(d)(3) of the Internal Revenue Code.", _subFont);
							para.Add(chunk);
							PdfReports.AddText2Table(table, para, _bodyLayout.Length);
							
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
				} else {
					// No recs qualified for query.
					WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("No records matched your query.");
					throw(warn);
				}
            }
            catch (System.Exception ex) {

                string errMsg = "cropYear: " + cropYear.ToString();
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(MOD_NAME + "." + METHOD_NAME + ": " + errMsg, ex);
                throw (wscEx);
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

    internal class NoticeOfPassthroughEvent : PdfPageEventHelper, ICustomPageEvent {

        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
		private static Font _superTitleFont = FontFactory.GetFont("HELVETICA", 14F, Font.BOLD);
        private static Font _sub_titleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 11F, Font.NORMAL);
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
        private string _memberName = "";
        private string _adr1 = "";
        private string _adr2 = "";
        private string _city = "";
        private string _state = "";
        private string _postalCode = "";
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

			//_lastPageNumber++;
			//string text = "Page " + _lastPageNumber.ToString();
			//float len = _bf.GetWidthPoint(text, 8);
			//_cb.BeginText();
			//_cb.SetFontAndSize(_bf, 8);
			//_cb.SetTextMatrix(280, 30);
			//_cb.ShowText(text);
			//_cb.EndText();

			//if (_lastPageNumber != _pageNumber) {
			//    _lastPageNumber = _pageNumber;
			//}
            base.OnEndPage(writer, document);
        }

        public override void OnStartPage(PdfWriter writer, Document document) {

			int borderTypeAll = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;

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
                float[] layout = new float[] {320F, 180F, 40F};				
				PdfPTable table = PdfReports.CreateTable(layout, 0);

				PdfReports.AddText2Table(table, _title + "\n", _titleFont, "center", layout.Length);
				PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
				//PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);

				PdfReports.AddText2Table(table, _statementDate, _normalFont, "left");

				//PdfReports.AddText2Table(table, " ", _normalFont);
				iTextSharp.text.pdf.PdfPCell cell = 
					PdfReports.AddText2Cell("URGENT TAX\n" 
											+ "NOTIFICATION", 
											_superTitleFont, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_BOTTOM,
											3F, 1.0F, borderTypeAll, Color.BLACK);
				table.AddCell(cell);
				PdfReports.AddText2Table(table, " ", _normalFont);
				PdfReports.AddText2Table(table, " ", _superTitleFont, layout.Length);	// larger font to push address line down

				PdfReports.AddTableNoSplit(document, this, table);

                // Address
				string csz = _city + ", " + _state + " " + _postalCode;
				Paragraph p = PdfReports.GetAddressBlock(_memberName, _adr1, _adr2,
					csz, 0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, _uspsFont);

				float[] addrLayout = new float[] { 50F, 270F, 220F };
				table = PdfReports.CreateTable(addrLayout, 0);

				PdfReports.AddText2Table(table, " ", _normalFont);
				PdfReports.AddText2Table(table, p);
				PdfReports.AddText2Table(table, " ", _normalFont);

				PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
				PdfReports.AddText2Table(table, " ", _normalFont, layout.Length);
                   
                PdfReports.AddTableNoSplit(document, this, table);
                _headerBottomYLine = _ct.YLine;
            }

            base.OnStartPage(writer, document);
        }

		public void FillEvent(ListNoticeOfPassthrough state, int pageNumber, string title, iTextSharp.text.Image imgLogo) {

			_statementDate = state.ReportDate.ToString("MMMM dd, yyyy");

            _memberName = state.MemberName;
            _adr1 = state.AdrLine1;
            _adr2 = state.AdrLine2;
            _city = state.AdrCity;
            _state = state.AdrState;
            _postalCode = state.AdrZip;
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
