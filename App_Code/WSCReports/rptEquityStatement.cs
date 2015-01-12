using System;
using System.Configuration;
using System.Collections;
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
    /// <summary>
    /// Summary description for rptEquityStatement.
    /// </summary>
    public class rptEquityStatement {

        // Transation types
        const int _entTypeIDAddress = 1;
        const int _entTypeIDCommonStock = 2;
        const int _entTypeIDPatronStock = 3;
        const int _entTypeIDPatronPermTransfer = 4;
        const int _entTypeIDUnitRetain = 5;
        const int _entTypeIDPatronageRefundCert = 6;
        const int _entTypeIDEquityFinancing = 7;
        const int _entTypeIDEquityFinancingPayment = 8;

        private Color Color_Highlight = Color.YELLOW;

        // Paper width: 612; lmargin + rmargin = 72; usable width = 540
        private float[] _commonStockLayout = new float[] { 122.4F, 86.4F, 86.4F, 86.4F, 86.4F, 72.0F };
        private float[] _patronStockLayout = new float[] { 122.4F, 86.4F, 86.4F, 86.4F, 86.4F, 72.0F };
        private float[] _unitRetainsLayout = new float[] { 34.9F, 50.0F, 76.3F, 70F, 59F, 70.9F, 77.3F, 101.6F };
		private float[] _patronageRefundLayout = new float[] { 30F, 45.0F, 64F, 45F, 73F, 71F, 77F, 71F, 71F };
        private float[] _equityFinancingLayout = new float[] { 52.2F, 70.2F, 86.4F, 86.4F, 86.4F, 86.4F, 72.0F };

        private Font _headerFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
        private Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);
        private Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private Font _subTitleFont = FontFactory.GetFont("HELVETICA", 9F, Font.NORMAL);
        private Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);

        private int _cropYear = 0;
        private DateTime _reportDate = DateTime.Now;

        decimal _patronStockShares = 0;
        decimal _unitRetainTons = 0;
        decimal _unitRetainAmount = 0;
        decimal _unitRetainPaid = 0;
        decimal _unitRetainBalance = 0;
        decimal _patronageRefundTons = 0;
        decimal _patronageRefundAmount = 0;
        decimal _patronageRefundInitialPaid = 0;
        decimal _patronageRefundNewBalance = 0;
		decimal _patronageCertificatePaid = 0;
        decimal _patronageRefundLossAlloc = 0;
        decimal _financingPaid = 0;
        decimal _financingInterest = 0;
        decimal _financingBalance = 0;

        public string ReportPackager(int cropYear, DateTime reportDate, string shid, bool isActive, string fileName, string logoUrl, string pdfTempfolder,
			DateTime activityFromDate, DateTime activityToDate, bool isLienInfoWanted) {

            const string METHOD_NAME = "rptEquityStatement.ReportPackager: ";
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

                        if (String.IsNullOrEmpty(shid)) {
                            shid = null;
                        }

						using (SqlDataReader dr = WSCPayment.GetEquityStatement(conn, cropYear, shid, isActive, activityFromDate, activityToDate, isLienInfoWanted)) {
                            using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {

                                ReportBuilder(dr, cropYear, reportDate, logoUrl, fs);
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
                string errMsg = "cropYear: " + cropYear.ToString();

                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(METHOD_NAME + errMsg, ex);
                throw (wscEx);
            }
        }

        private void ReportBuilder(SqlDataReader dr, int cropYear, DateTime reportDate, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "rptEquityStatement.ReportBuilder: ";
            const int resetFlag = 0;

            Document document = null;
            PdfWriter writer = null;
            PdfPTable table = null;
            iTextSharp.text.Image imgLogo = null;
            EQStmtEvent pgEvent = null;

            _cropYear = cropYear;
            _reportDate = reportDate;
            int shid = 0;
            int lastShid = 0;
            int entityTypeID = 0;
            int lastEntityTypeID = 0;

            string rptTitle = "Shareholder Equity Statement";

            try {

                while (dr.Read()) {

                    entityTypeID = dr.GetInt32(dr.GetOrdinal("EntityTypeID"));
                    if (entityTypeID == _entTypeIDAddress) {
                        shid = dr.GetInt32(dr.GetOrdinal("MemberNumber"));
                    }

                    // when you switch entities, close out the last entity when appropriate
                    if (entityTypeID != lastEntityTypeID || entityTypeID == _entTypeIDAddress) {

                        FinishEntityBlock(writer, document, ref table, entityTypeID, lastEntityTypeID, pgEvent);
                    }

                    if (document == null) {

                        lastShid = shid;

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        imgLogo = PdfReports.GetImage(logoUrl, 127, 50, iTextSharp.text.Element.ALIGN_CENTER);

                        // Attach my override event handler(s)
                        pgEvent = new EQStmtEvent();
                        pgEvent.FillEvent(reportDate, shid, " ",
                            dr.GetString(dr.GetOrdinal("BusinessName")),
                            dr.GetString(dr.GetOrdinal("Addr1")),
                            dr.GetString(dr.GetOrdinal("Addr2")),
                            dr.GetString(dr.GetOrdinal("CityStateZip")),
                            resetFlag, rptTitle, imgLogo);

                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                    } else {

                        if (lastShid != shid) {

                            lastShid = shid;
                            lastEntityTypeID = entityTypeID;

                            _financingBalance = 0;

                            pgEvent.FillEvent(reportDate, shid, " ",
                                dr.GetString(dr.GetOrdinal("BusinessName")),
                                dr.GetString(dr.GetOrdinal("Addr1")),
                                dr.GetString(dr.GetOrdinal("Addr2")),
                                dr.GetString(dr.GetOrdinal("CityStateZip")),
                                resetFlag, rptTitle, imgLogo);

                            document.NewPage();
                        }
                    }

                    //=========================================================
                    // Add Common Stock
                    //=========================================================
                    if (entityTypeID == _entTypeIDCommonStock) {
                        if (lastEntityTypeID != _entTypeIDCommonStock) {
                            lastEntityTypeID = _entTypeIDCommonStock;
                            AddCommonHeader(ref table);
                        }
                        AddCommonData(dr, table);
                    }

                    //=========================================================
                    // Patron Stock
                    //=========================================================
                    if ((entityTypeID == _entTypeIDPatronStock ||
                        entityTypeID == _entTypeIDPatronPermTransfer) &&
                        ((lastEntityTypeID != _entTypeIDPatronStock &&
                        lastEntityTypeID != _entTypeIDPatronPermTransfer))) {

                        AddPatronStockHeader(ref table);
                    }

                    if (entityTypeID == _entTypeIDPatronStock) {
                        if (lastEntityTypeID != _entTypeIDPatronStock) {

                            lastEntityTypeID = _entTypeIDPatronStock;
                            AddPatronStockPurchaseHeader(ref table);
                        }

                        AddPatronStockPurchaseData(dr, table);
                    }

                    if (entityTypeID == _entTypeIDPatronPermTransfer) {
                        if (lastEntityTypeID != _entTypeIDPatronPermTransfer) {

                            lastEntityTypeID = _entTypeIDPatronPermTransfer;
                            AddPatronStockTransferHeader(ref table);
                        }

                        AddPatronStockTransferData(dr, table);

                    }

                    //=========================================================
                    // Add Unit Retains
                    //=========================================================
                    if (entityTypeID == _entTypeIDUnitRetain) {
                        if (lastEntityTypeID != _entTypeIDUnitRetain) {

                            lastEntityTypeID = _entTypeIDUnitRetain;
                            AddUnitRetainHeader(ref table);
                        }

                        AddUnitRetainData(dr, table);
                    }

                    //=========================================================
                    // Add Patronage Refunds
                    //=========================================================
                    if (entityTypeID == _entTypeIDPatronageRefundCert) {
                        if (lastEntityTypeID != _entTypeIDPatronageRefundCert) {

                            lastEntityTypeID = _entTypeIDPatronageRefundCert;
                            AddPatronageRefundHeader(ref table);
                        }

                        AddPatronageRefundData(dr, table);
                    }

                    //=========================================================
                    // Add Equity Financing
                    //=========================================================
                    if (entityTypeID == _entTypeIDEquityFinancing ||
                        entityTypeID == _entTypeIDEquityFinancingPayment) {

                        if (lastEntityTypeID != _entTypeIDEquityFinancing &&
                            lastEntityTypeID != _entTypeIDEquityFinancingPayment) {
                            AddFinancingHeader(ref table);
                        }
                        lastEntityTypeID = entityTypeID;

                        if (entityTypeID == _entTypeIDEquityFinancing) {
                            AddFinancingLoanData(dr, table);
                        }

                        if (entityTypeID == _entTypeIDEquityFinancingPayment) {
                            AddFinancingLoanPaymentData(dr, table);
                        }

                    }
/*   */
                }

                entityTypeID = 0;
                FinishEntityBlock(writer, document, ref table, entityTypeID, lastEntityTypeID, pgEvent);

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
                if (dr != null) {
                    if (!dr.IsClosed) {
                        dr.Close();
                    }
                }
                if (document != null) {
                    pgEvent.IsDocumentClosing = true;
                    document.Close();
                }
                if (writer != null) {
                    writer.Close();
                }
            }
        }

        private void FinishEntityBlock(PdfWriter writer, Document document, ref PdfPTable table, int entityTypeID, int lastEntityTypeID, EQStmtEvent pgEvent) {

            if (table != null) {

                // Finish Common Stock
                if (lastEntityTypeID == _entTypeIDCommonStock) {
                    FinishCommonData(writer, document, ref table, pgEvent);
                }

                // Finish Patron Stock
                if ((entityTypeID != _entTypeIDPatronStock &&
                    entityTypeID != _entTypeIDPatronPermTransfer) &&
                    (lastEntityTypeID == _entTypeIDPatronPermTransfer ||
                    lastEntityTypeID == _entTypeIDPatronStock)) {

                    FinishPatronStockData(writer, document, ref table, pgEvent);

                    _patronStockShares = 0;
                }

                // Finish Unit Retains
                if (lastEntityTypeID == _entTypeIDUnitRetain) {

                    FinishUnitRetainData(writer, document, ref table, pgEvent);

                    _unitRetainTons = 0;
                    _unitRetainAmount = 0;
                    _unitRetainPaid = 0;
                    _unitRetainBalance = 0;
                }

                // Finish Patronage Refund
                if (lastEntityTypeID == _entTypeIDPatronageRefundCert) {

                    FinishPatronageRefundData(writer, document, ref table, pgEvent);

                    _patronageRefundTons = 0;
                    _patronageRefundAmount = 0;
                    _patronageRefundInitialPaid = 0;
					_patronageCertificatePaid = 0;
                    _patronageRefundNewBalance = 0;
                    _patronageRefundLossAlloc = 0;
                }

                // Finish Loan Financing
                if ((entityTypeID != _entTypeIDEquityFinancing &&
                    entityTypeID != _entTypeIDEquityFinancingPayment) &&
                    (lastEntityTypeID == _entTypeIDEquityFinancing ||
                    lastEntityTypeID == _entTypeIDEquityFinancingPayment)) {

                    FinishFinancingData(writer, document, ref table, pgEvent);

                    _financingPaid = 0;
                    _financingInterest = 0;
                    _financingBalance = 0;
                }
            }
        }

        private void AddPatronStockHeader(ref PdfPTable table) {

            table = PdfReports.CreateTable(_patronStockLayout, 0);

            iTextSharp.text.pdf.PdfPCell cell =
                PdfReports.AddText2Cell("Patron Preferred Stock", _labelFont, "left", _patronStockLayout.Length);
            cell.BackgroundColor = Color.LIGHT_GRAY;
            table.AddCell(cell);
        }

        private void AddUnitRetainHeader(ref PdfPTable table) {

            table = PdfReports.CreateTable(_unitRetainsLayout, 0);

            iTextSharp.text.pdf.PdfPCell cell =
                PdfReports.AddText2Cell("Unit Retains", _labelFont, "left", _unitRetainsLayout.Length);
            cell.BackgroundColor = Color.LIGHT_GRAY;
            table.AddCell(cell);

            PdfReports.AddText2Table(table, "Crop Year", _labelFont, "center");
			PdfReports.AddText2Table(table, "Date", _labelFont, "center");
            PdfReports.AddText2Table(table, "Tons Delivered", _labelFont, "center");
            PdfReports.AddText2Table(table, "$/Ton", _labelFont, "center");
            PdfReports.AddText2Table(table, "Amount", _labelFont, "center");
            PdfReports.AddText2Table(table, "$ Paid", _labelFont, "center");
            PdfReports.AddText2Table(table, "Balance", _labelFont, "center");
            PdfReports.AddText2Table(table, " ", _normalFont);
        }

        private void AddPatronageRefundHeader(ref PdfPTable table) {

            table = PdfReports.CreateTable(_patronageRefundLayout, 0);

            iTextSharp.text.pdf.PdfPCell cell =
                PdfReports.AddText2Cell("Patronage Refunds", _labelFont, "left", _patronageRefundLayout.Length);
            cell.BackgroundColor = Color.LIGHT_GRAY;
            table.AddCell(cell);

            PdfReports.AddText2Table(table, "Crop Year", _labelFont, "center");
			PdfReports.AddText2Table(table, "Date", _labelFont, "center");
            PdfReports.AddText2Table(table, "Tons Delivered", _labelFont, "center");
            PdfReports.AddText2Table(table, "$/Ton", _labelFont, "center");
            PdfReports.AddText2Table(table, "Amount", _labelFont, "center");
            PdfReports.AddText2Table(table, "Initial Pay", _labelFont, "center");
            PdfReports.AddText2Table(table, "Crop Loss Allocation/Pmt", _labelFont, "center");
			PdfReports.AddText2Table(table, "Certificate Paid", _labelFont, "center");
            PdfReports.AddText2Table(table, "Balance", _labelFont, "center");
        }

        private void AddFinancingHeader(ref PdfPTable table) {

            table = PdfReports.CreateTable(_equityFinancingLayout, 0);

            iTextSharp.text.pdf.PdfPCell cell =
                PdfReports.AddText2Cell("Equity Financing", _labelFont, "left", _equityFinancingLayout.Length);
            cell.BackgroundColor = Color.LIGHT_GRAY;
            table.AddCell(cell);

            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, "Date", _labelFont, "center");
            PdfReports.AddText2Table(table, "Principal Paid", _labelFont, "center");
            PdfReports.AddText2Table(table, "Interest", _labelFont, "center");
            PdfReports.AddText2Table(table, "Balance", _labelFont, "center");
            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, " ", _normalFont);
        }

        private void AddCommonHeader(ref PdfPTable table) {

            table = PdfReports.CreateTable(_commonStockLayout, 0);
            iTextSharp.text.pdf.PdfPCell cell =
                PdfReports.AddText2Cell("Common Stock Purchase", _labelFont, "left", _commonStockLayout.Length);
            cell.BackgroundColor = Color.LIGHT_GRAY;
            table.AddCell(cell);

            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, "Shares", _labelFont, "center");
            PdfReports.AddText2Table(table, "Date", _labelFont, "center");
            PdfReports.AddText2Table(table, "Price", _labelFont, "center");
            PdfReports.AddText2Table(table, "Value", _labelFont, "center");
            PdfReports.AddText2Table(table, " ", _normalFont);
        }

        private void AddCommonData(SqlDataReader dr, PdfPTable table) {

            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, dr.GetDecimal(dr.GetOrdinal("XActQuantity")).ToString("N0"),
                _normalFont, "center");
            PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("XActDate")),
                _normalFont, "center");
            PdfReports.AddText2Table(table, "$" + dr.GetDecimal(dr.GetOrdinal("XActPrice")).ToString("#,##0.00"),
                _normalFont, "center");
            PdfReports.AddText2Table(table, "$" + dr.GetDecimal(dr.GetOrdinal("XActAmount")).ToString("#,##0.00"),
                _normalFont, "right");
            PdfReports.AddText2Table(table, " ", _normalFont);
        }

        private void AddPatronStockTransferHeader(ref PdfPTable table) {

            PdfReports.AddText2Table(table, "Transferred", _labelFont, "center");
            PdfReports.AddText2Table(table, " ", _labelFont, "center", _patronStockLayout.Length - 1);
            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, "Shares", _labelFont, "center");
            PdfReports.AddText2Table(table, "Date", _labelFont, "center");
            PdfReports.AddText2Table(table, " ", _normalFont, 3);
        }

        private void AddPatronStockPurchaseHeader(ref PdfPTable table) {

            PdfReports.AddText2Table(table, "Purchased", _labelFont, "center");
            PdfReports.AddText2Table(table, " ", _labelFont, "center", _patronStockLayout.Length - 1);
            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, "Shares", _labelFont, "center");
            PdfReports.AddText2Table(table, "Date", _labelFont, "center");
            PdfReports.AddText2Table(table, "Price", _labelFont, "center");
            PdfReports.AddText2Table(table, "Value", _labelFont, "center");
            PdfReports.AddText2Table(table, " ", _normalFont);
        }

        private void AddPatronStockTransferData(SqlDataReader dr, PdfPTable table) {

            decimal shares = dr.GetDecimal(dr.GetOrdinal("XActQuantity"));
            _patronStockShares += shares;

            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, shares.ToString("N0"),
                _normalFont, "center");
            PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("XActDate")),
                _normalFont, "center");
            PdfReports.AddText2Table(table, " ", _normalFont, 3);
        }

        private void AddPatronStockPurchaseData(SqlDataReader dr, PdfPTable table) {

            decimal shares = dr.GetDecimal(dr.GetOrdinal("XActQuantity"));
            _patronStockShares += shares;

            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, shares.ToString("N0"),
                _normalFont, "center");
            PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("XActDate")),
                _normalFont, "center");
            PdfReports.AddText2Table(table, "$" + dr.GetDecimal(dr.GetOrdinal("XActPrice")).ToString("#,##0.00"),
                _normalFont, "center");
            PdfReports.AddText2Table(table, "$" + dr.GetDecimal(dr.GetOrdinal("XActAmount")).ToString("#,##0.00"),
                _normalFont, "right");
            PdfReports.AddText2Table(table, " ", _normalFont);
        }

        private void AddUnitRetainData(SqlDataReader dr, PdfPTable table) {

            decimal amt = dr.GetDecimal(dr.GetOrdinal("XActAmount"));
            decimal paid = dr.GetDecimal(dr.GetOrdinal("FinPayPrincipal"));
            decimal tons = dr.GetDecimal(dr.GetOrdinal("XActQuantity"));
            _unitRetainAmount += amt;
            _unitRetainPaid += paid;
            _unitRetainBalance += (amt - paid);
            _unitRetainTons += tons;

            PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("XActCropYear")).ToString(), _normalFont, "center");
			PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("XActDate")), _normalFont, "center");
            PdfReports.AddText2Table(table, tons.ToString("#,##0.0000"), _normalFont, "right");
            PdfReports.AddText2Table(table, "$" + dr.GetDecimal(dr.GetOrdinal("XActPrice")).ToString("#,##0.00"), _normalFont, "center");
            PdfReports.AddText2Table(table, "$" + amt.ToString("#,##0.00"), _normalFont, "right");
            PdfReports.AddText2Table(table, "$" + paid.ToString("#,##0.00"), _normalFont, "right");
            PdfReports.AddText2Table(table, "$" + (amt - paid).ToString("#,##0.00"), _normalFont, "right");
            PdfReports.AddText2Table(table, " ", _normalFont);
        }

        private void AddPatronageRefundData(SqlDataReader dr, PdfPTable table) {

            decimal tons = dr.GetDecimal(dr.GetOrdinal("XActQuantity"));
            decimal amt = dr.GetDecimal(dr.GetOrdinal("XActAmount"));
            decimal initialPaid = dr.GetDecimal(dr.GetOrdinal("FinPayPrincipal"));
            decimal lossAlloc = dr.GetDecimal(dr.GetOrdinal("LossAlloc"));
			decimal certificatePaid = dr.GetDecimal(dr.GetOrdinal("FinPayInterest"));
            decimal newBalance = dr.GetDecimal(dr.GetOrdinal("Balance"));
            decimal tmp = 0;

            _patronageRefundTons += tons;
            _patronageRefundAmount += amt;
            _patronageRefundInitialPaid += initialPaid;
			_patronageCertificatePaid += certificatePaid;
            _patronageRefundNewBalance += newBalance;
            _patronageRefundLossAlloc += lossAlloc;

            // Crop Year
            PdfReports.AddText2Table(table, dr.GetInt32(dr.GetOrdinal("XActCropYear")).ToString(), _normalFont, "center");

			// Date
			PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("XActDate")), _normalFont, "center");
            // Tons
            PdfReports.AddText2Table(table, tons.ToString("#,##0.0000"), _normalFont, "right");
            // Rate
            tmp = dr.GetDecimal(dr.GetOrdinal("XActPrice"));
            if (tmp < 0) {
                // blank out negative rates.
                PdfReports.AddText2Table(table, "($" + Math.Abs(tmp).ToString("#,##0.0000") + ")", _normalFont, "center");
            } else {
                PdfReports.AddText2Table(table, "$" + tmp.ToString("#,##0.0000"), _normalFont, "center");
            }
            // Amount
            if (amt < 0) {
                PdfReports.AddText2Table(table, "($" + Math.Abs(amt).ToString("#,##0.00") + ")", _normalFont, "right");
            } else {
                PdfReports.AddText2Table(table, "$" + amt.ToString("#,##0.00"), _normalFont, "right");
            }

            // initialPaid
            PdfReports.AddText2Table(table, "$" + initialPaid.ToString("#,##0.00"), _normalFont, "right");

            // Loss Allocation
            if (lossAlloc < 0) {
                PdfReports.AddText2Table(table, "($" + Math.Abs(lossAlloc).ToString("#,##0.00") + ")", _normalFont, "right");
            } else {
                PdfReports.AddText2Table(table, "$" + lossAlloc.ToString("#,##0.00"), _normalFont, "right");
            }

			// Certificate Paid
			if (certificatePaid < 0) {
				PdfReports.AddText2Table(table, "($" + Math.Abs(certificatePaid).ToString("#,##0.00") + ")", _normalFont, "right");
			} else {
				PdfReports.AddText2Table(table, "$" + certificatePaid.ToString("#,##0.00"), _normalFont, "right");
			}

            // Balance
            if (newBalance < 0) {
                PdfReports.AddText2Table(table, "($" + Math.Abs(newBalance).ToString("#,##0.00") + ")", _normalFont, "right");
            } else {
                PdfReports.AddText2Table(table, "$" + newBalance.ToString("#,##0.00"), _normalFont, "right");
            }
        }

        private void AddFinancingLoanData(SqlDataReader dr, PdfPTable table) {

            decimal loanBalance = dr.GetDecimal(dr.GetOrdinal("XActAmount"));
            _financingPaid = 0;
            _financingInterest = 0;
            _financingBalance += loanBalance;

            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("XActDate")),
                _normalFont, "center");
            PdfReports.AddText2Table(table, " ", _normalFont, 2);

            if (loanBalance < 0) {
                PdfReports.AddText2Table(table, "($" + Math.Abs(loanBalance).ToString("#,##0.00") + ")", _normalFont, "right");
            } else {
                PdfReports.AddText2Table(table, "$" + loanBalance.ToString("#,##0.00"), _normalFont, "right");
            }
            PdfReports.AddText2Table(table, " ", _normalFont, 2);
        }

        private void AddFinancingLoanPaymentData(SqlDataReader dr, PdfPTable table) {

            decimal loanPayment = dr.GetDecimal(dr.GetOrdinal("FinPayPrincipal"));
            decimal interest = dr.GetDecimal(dr.GetOrdinal("FinPayInterest"));
            _financingBalance -= loanPayment;

            _financingPaid += loanPayment;
            _financingInterest += interest;

            PdfReports.AddText2Table(table, " ", _normalFont);
            PdfReports.AddText2Table(table, dr.GetString(dr.GetOrdinal("XActDate")),
                _normalFont, "center");
            PdfReports.AddText2Table(table, "$" + loanPayment.ToString("#,##0.00"),
                _normalFont, "right");
            PdfReports.AddText2Table(table, "$" + interest.ToString("#,##0.00"),
                _normalFont, "right");
            PdfReports.AddText2Table(table, "$" + _financingBalance.ToString("#,##0.00"),
                _normalFont, "right");
            PdfReports.AddText2Table(table, " ", _normalFont, 2);
        }

        private void FinishUnitRetainData(PdfWriter writer, Document document, ref PdfPTable table, EQStmtEvent pgEvent) {

            PdfReports.AddText2Table(table, " ", _normalFont, _unitRetainsLayout.Length);

            iTextSharp.text.pdf.PdfPCell cell = PdfReports.AddText2Cell("Total", _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

			// Date
			cell = PdfReports.AddText2Cell(" ", _labelFont, "right");
			cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
			table.AddCell(cell);

            cell = PdfReports.AddText2Cell(_unitRetainTons.ToString("#,##0.0000"), _labelFont, "right");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell(" ", _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("$" + _unitRetainAmount.ToString("#,##0.00"), _labelFont, "right");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("$" + _unitRetainPaid.ToString("#,##0.00"), _labelFont, "right");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell("$" + _unitRetainBalance.ToString("#,##0.00"), _labelFont, "right");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell(" ", _labelFont, "right");
            table.AddCell(cell);

            PdfReports.AddText2Table(table, " ", _normalFont, _unitRetainsLayout.Length);

            PdfReports.AddTableNoSplit(document, pgEvent, table);
            table = null;
        }

        private void FinishPatronageRefundData(PdfWriter writer, Document document, ref PdfPTable table, EQStmtEvent pgEvent) {

            PdfReports.AddText2Table(table, " ", _normalFont, _patronageRefundLayout.Length);

            // Crop Year
            iTextSharp.text.pdf.PdfPCell cell = PdfReports.AddText2Cell("Total", _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

			// Date
			cell = PdfReports.AddText2Cell(" ", _labelFont, "right");
			cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
			table.AddCell(cell);

            // Tons
            cell = PdfReports.AddText2Cell(_patronageRefundTons.ToString("#,##0.0000"), _labelFont, "right");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            // Rate
            cell = PdfReports.AddText2Cell(" ", _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            // Amount
            if (_patronageRefundAmount < 0) {
                cell = PdfReports.AddText2Cell("($" + Math.Abs(_patronageRefundAmount).ToString("#,##0.00") + ")", _labelFont, "right");
            } else {
                cell = PdfReports.AddText2Cell("$" + _patronageRefundAmount.ToString("#,##0.00"), _labelFont, "right");
            }
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            // Initial Paid
            cell = PdfReports.AddText2Cell("$" + _patronageRefundInitialPaid.ToString("#,##0.00"), _labelFont, "right");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            // Loss Alloc
            cell = PdfReports.AddText2Cell(" ", _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

			// Certificate Paid
			cell = PdfReports.AddText2Cell("$" + _patronageCertificatePaid.ToString("#,##0.00"), _labelFont, "right");
			cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
			table.AddCell(cell);

            // New Balance
            if (_patronageRefundNewBalance < 0) {
                cell = PdfReports.AddText2Cell("($" + Math.Abs(_patronageRefundNewBalance).ToString("#,##0.00") + ")", _labelFont, "right");
            } else {
                cell = PdfReports.AddText2Cell("$" + _patronageRefundNewBalance.ToString("#,##0.00"), _labelFont, "right");
            }
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            PdfReports.AddText2Table(table, " ", _normalFont, _patronageRefundLayout.Length);
            PdfReports.AddTableNoSplit(document, pgEvent, table);
            table = null;
        }

        private void FinishCommonData(PdfWriter writer, Document document, ref PdfPTable table, EQStmtEvent pgEvent) {

            PdfReports.AddText2Table(table, " ", _normalFont, _commonStockLayout.Length);
            PdfReports.AddTableNoSplit(document, pgEvent, table);
            table = null;
        }

        private void FinishFinancingData(PdfWriter writer, Document document, ref PdfPTable table, EQStmtEvent pgEvent) {

            PdfReports.AddText2Table(table, " ", _normalFont, _equityFinancingLayout.Length);

            iTextSharp.text.pdf.PdfPCell cell = PdfReports.AddText2Cell("Total", _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell(" ", _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            if (_financingPaid < 0) {
                cell = PdfReports.AddText2Cell("($" + Math.Abs(_financingPaid).ToString("#,##0.00") + ")", _labelFont, "right");
            } else {
                cell = PdfReports.AddText2Cell("$" + _financingPaid.ToString("#,##0.00"), _labelFont, "right");
            }
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            if (_financingInterest < 0) {
                cell = PdfReports.AddText2Cell("($" + Math.Abs(_financingInterest).ToString("#,##0.00") + ")", _labelFont, "right");
            } else {
                cell = PdfReports.AddText2Cell("$" + _financingInterest.ToString("#,##0.00"), _labelFont, "right");
            }
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            if (_financingBalance < 0) {
                cell = PdfReports.AddText2Cell("($" + Math.Abs(_financingBalance).ToString("#,##0.00") + ")", _labelFont, "right");
            } else {
                cell = PdfReports.AddText2Cell("$" + _financingBalance.ToString("#,##0.00"), _labelFont, "right");
            }
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            PdfReports.AddText2Table(table, " ", _normalFont, 2);

            PdfReports.AddText2Table(table, " ", _normalFont, _equityFinancingLayout.Length);
            PdfReports.AddTableNoSplit(document, pgEvent, table);
            table = null;
        }

        private void FinishPatronStockData(PdfWriter writer, Document document, ref PdfPTable table, EQStmtEvent pgEvent) {

            PdfReports.AddText2Table(table, " ", _normalFont, _patronStockLayout.Length);

            iTextSharp.text.pdf.PdfPCell cell = PdfReports.AddText2Cell("Total Shares Owned", _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            cell = PdfReports.AddText2Cell(_patronStockShares.ToString("#,##0"), _labelFont, "center");
            cell.BackgroundColor = cell.BackgroundColor = Color_Highlight;
            table.AddCell(cell);

            PdfReports.AddText2Table(table, " ", _normalFont, _patronStockLayout.Length - 2);
            PdfReports.AddText2Table(table, " ", _normalFont, _patronStockLayout.Length);
            PdfReports.AddTableNoSplit(document, pgEvent, table);
            table = null;
        }
    }

    public class EQStmtEvent : PdfPageEventHelper, ICustomPageEvent {

        Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        Font _subTitleFont = FontFactory.GetFont("HELVETICA", 10F, Font.NORMAL);
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
        private int _pageNumber = 0;
        private int _lastPageNumber = 0;
        private DateTime _reportDate;
        private int _shid = 0;
        private string _busName = "";
        private string _adrLine1 = "";
        private string _adrLine2 = "";
        private string _csz = "";
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

                Paragraph p = null;

                // =======================================================
                // Add Logo
                // =======================================================
                if (_pageNumber == 1) {

                    float[] wscLogoLayout = new float[] { 127F, 286F, 127F };
                    PdfPTable logoTable = PdfReports.CreateTable(wscLogoLayout, 0);

                    PdfReports.AddText2Table(logoTable, " ", _normalFont);
                    PdfReports.AddText2Table(logoTable, _title + "\n" + _reportDate.ToShortDateString(), _titleFont, "center");
                    PdfReports.AddImage2Table(logoTable, _imgLogo);

                    PdfReports.AddTableNoSplit(document, this, logoTable);
                }

                float[] addrLayout = new float[] { 50F, 270F, 220F };
                PdfPTable table = PdfReports.CreateTable(addrLayout, 0);
                PdfReports.AddText2Table(table, " ", _normalFont, addrLayout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, addrLayout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, addrLayout.Length);

                if (_pageNumber == 1) {

                    PdfReports.AddText2Table(table, " ", _normalFont);

                    // Left column: Full Mailling Address
                    p = PdfReports.GetAddressBlock(_busName, _adrLine1, _adrLine2,
                        _csz, 0F, 12F, iTextSharp.text.Element.ALIGN_LEFT, _uspsFont);
                    PdfReports.AddText2Table(table, p);

                    // Right column: shid / tax id				
                    PdfReports.AddText2Table(table, "Co-op ID: " + _shid.ToString() + "\n" + " ", _normalFont, "right");

                } else {

                    // Left column: Business Name
                    PdfReports.AddText2Table(table, " ", _normalFont);
                    PdfReports.AddText2Table(table, _busName, _normalFont);

                    // Right column: shid / tax id
                    PdfReports.AddText2Table(table, "Co-op ID: " + _shid.ToString() + "\n" + " ", _normalFont, "right");
                }
                PdfReports.AddText2Table(table, " ", _normalFont, addrLayout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, addrLayout.Length);
                PdfReports.AddText2Table(table, " ", _normalFont, addrLayout.Length);

                PdfReports.AddTableNoSplit(document, this, table);
                _headerBottomYLine = _ct.YLine;

            }

            base.OnStartPage(writer, document);
        }

        public void FillEvent(DateTime reportDate,
            int shid, string taxID, string busName, string adrLine1, string adrLine2, string csz,
            int pageNumber, string title, iTextSharp.text.Image imgLogo) {

            _reportDate = reportDate;
            _shid = shid;
            _busName = busName;
            _adrLine1 = adrLine1;
            _adrLine2 = adrLine2;
            _csz = csz;

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
