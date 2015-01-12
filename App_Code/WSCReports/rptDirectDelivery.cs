using System;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using MDAAB.Classic;
using WSCData;
using PdfHelper;

namespace WSCReports {

    public class rptDirectDelivery {

        private const string MOD_NAME = "rptDirectDelivery.";

        public static string ReportPackager(int cropYear, DateTime fromDate, DateTime toDate, int paymentID, string fileName, string pdfTempfolder, out string warnMsg) {

            const string METHOD_NAME = "ReportPackager";
            DirectoryInfo pdfDir = null;
            string filePath = "";

            try {

                pdfDir = new DirectoryInfo(pdfTempfolder);

                // Build the output file name by getting a list of all PDF files 
                // that begin with this session ID: use this as a name incrementer.				
                if (Path.GetExtension(fileName) == "") {
                    fileName += ".csv";
                }
                filePath = pdfDir.FullName + @"\" + fileName;
                ReportBuilder(cropYear, fromDate, toDate, paymentID, filePath, out warnMsg);

                return filePath;
            }
            catch (System.Exception ex) {

                string errMsg = "";

                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + ": " + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(int cropYear, DateTime fromDate, DateTime toDate, int paymentID, string filePath, out string warnMsg) {

            const string METHOD_NAME = "ReportBuilder";
            const string sQUOTE = "\"";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpRptDirectDelivery";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = fromDate;
                    spParams[2].Value = toDate;
                    spParams[3].Value = paymentID;

                    using (StreamWriter sw = new StreamWriter(filePath, false)) {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int icrop_year = dr.GetOrdinal("crop_year");
                            int icontract_no = dr.GetOrdinal("contract_no");
                            int iaddress_no = dr.GetOrdinal("address_no");
                            int ipayment_no = dr.GetOrdinal("payment_no");
                            int iaddress_contact_name = dr.GetOrdinal("address_contact_name");
                            int iaddress_business_name = dr.GetOrdinal("address_business_name");
                            int iaddress_line_1 = dr.GetOrdinal("address_line_1");
                            int iaddress_line_2 = dr.GetOrdinal("address_line_2");
                            int iaddress_city = dr.GetOrdinal("address_city");
                            int iaddress_state = dr.GetOrdinal("address_state");
                            int iaddress_zip_code = dr.GetOrdinal("address_zip_code");
                            int ipayee = dr.GetOrdinal("payee");
                            int ipayment_amount = dr.GetOrdinal("payment_amount");
                            int ifirst_net_tons = dr.GetOrdinal("first_net_tons");
                            int idelivery_station = dr.GetOrdinal("delivery_station");
                            int icontract_station = dr.GetOrdinal("contract_station");
                            int inintysix_percent = dr.GetOrdinal("nintysix_percent");
                            int irate_per_ton = dr.GetOrdinal("rate_per_ton");

                            sw.WriteLine(sQUOTE + dr.GetName(icrop_year) + sQUOTE + "," +
                            sQUOTE + dr.GetName(icontract_no) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_no) + sQUOTE + "," +
                            sQUOTE + dr.GetName(ipayment_no) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_contact_name) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_business_name) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_line_1) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_line_2) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_city) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_state) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_zip_code) + sQUOTE + "," +
                            sQUOTE + dr.GetName(ipayee) + sQUOTE + "," +
                            sQUOTE + dr.GetName(ipayment_amount) + sQUOTE + "," +
                            sQUOTE + dr.GetName(ifirst_net_tons) + sQUOTE + "," +
                            sQUOTE + dr.GetName(idelivery_station) + sQUOTE + "," +
                            sQUOTE + dr.GetName(icontract_station) + sQUOTE + "," +
                            sQUOTE + dr.GetName(inintysix_percent) + sQUOTE + "," +
                            sQUOTE + dr.GetName(irate_per_ton) + sQUOTE);

                            // Note the stored procedure is supplying quoted csv fields as needed so we don't
                            // add the quote delimiters here.
                            while (dr.Read()) {

                                sw.WriteLine(dr.GetInt32(icrop_year).ToString() + "," + 
                                dr.GetString(icontract_no) + "," + 
                                dr.GetString(iaddress_no) + "," + 
                                dr.GetInt32(ipayment_no).ToString() + "," + 
                                dr.GetString(iaddress_contact_name) + "," + 
                                dr.GetString(iaddress_business_name) + "," + 
                                dr.GetString(iaddress_line_1) + "," + 
                                dr.GetString(iaddress_line_2) + "," + 
                                dr.GetString(iaddress_city) + "," + 
                                dr.GetString(iaddress_state) + "," + 
                                dr.GetString(iaddress_zip_code) + "," + 
                                dr.GetString(ipayee) + "," + 
                                dr.GetString(ipayment_amount) + "," + 
                                dr.GetDecimal(ifirst_net_tons).ToString("0.0000") + "," + 
                                dr.GetInt32(idelivery_station).ToString() + "," + 
                                dr.GetInt32(icontract_station).ToString() + "," + 
                                dr.GetDecimal(inintysix_percent).ToString("0.0000") + "," + 
                                dr.GetDecimal(irate_per_ton).ToString("0.0000"));
                            }

                            dr.Close();
                            warnMsg = spParams[4].Value.ToString();

                            sw.Close();
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscex);
            }
        }
    }

    public class rptDirectDeliveryExport {

        private const string MOD_NAME = "rptDirectDeliveryExport.";

        public static void ReportPackager(int cropYear, DateTime fromDate, DateTime toDate, int paymentID, string filePath, out string warnMsg) {

            //cropYear, fromDate, toDate, paymentID, filePath, out warnMsg
            const string METHOD_NAME = "ReportPackager";

            try {
                ReportBuilder(cropYear, fromDate, toDate, paymentID, filePath, out warnMsg);
            }
            catch (System.Exception ex) {

                string errMsg = "cropYear: " + cropYear.ToString() +
                    "; fromDate: " + fromDate.ToShortDateString() +
                    "; toDate: " + toDate.ToShortDateString() +
                    "; PaymentID: " + paymentID.ToString() +
                    "; filePath: " + filePath;

                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + ": " + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(int cropYear, DateTime fromDate, DateTime toDate, int paymentID, string filePath, out string warnMsg) {

            const string METHOD_NAME = "ReportBuilder";
            const string sQUOTE = "\"";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpRptDirectDeliveryExport";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams =
                        SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    spParams[0].Value = cropYear;
                    spParams[1].Value = fromDate;
                    spParams[2].Value = toDate;
                    spParams[3].Value = paymentID;

                    using (StreamWriter sw = new StreamWriter(filePath, false)) {

                        using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                            int icrop_year = dr.GetOrdinal("pay_crop_year");
                            int ishid = dr.GetOrdinal("pay_payee_shid");
                            int iaddress_no = dr.GetOrdinal("pay_address_no");
                            int ipayment_no = dr.GetOrdinal("pay_payment_number");
                            int iaddress_contact_name = dr.GetOrdinal("pay_adr_contact_name");
                            int iaddress_business_name = dr.GetOrdinal("pay_adr_business_name");
                            int iaddress_line_1 = dr.GetOrdinal("pay_adr_line_1");
                            int iaddress_line_2 = dr.GetOrdinal("pay_adr_line_2");
                            int iaddress_city = dr.GetOrdinal("pay_adr_city");
                            int iaddress_state = dr.GetOrdinal("pay_adr_state");
                            int iaddress_zip_code = dr.GetOrdinal("pay_adr_zip_code");
                            int ipayee = dr.GetOrdinal("pay_payee_name");
                            int ipayment_amount = dr.GetOrdinal("pay_payment_amount");

                            sw.WriteLine(sQUOTE + dr.GetName(icrop_year) + sQUOTE + "," +
                            sQUOTE + dr.GetName(ishid) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_no) + sQUOTE + "," +
                            sQUOTE + dr.GetName(ipayment_no) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_contact_name) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_business_name) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_line_1) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_line_2) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_city) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_state) + sQUOTE + "," +
                            sQUOTE + dr.GetName(iaddress_zip_code) + sQUOTE + "," +
                            sQUOTE + dr.GetName(ipayee) + sQUOTE + "," +
                            sQUOTE + dr.GetName(ipayment_amount) + sQUOTE);

                            // Note the stored procedure is supplying quoted csv fields as needed so we don't
                            // add the quote delimiters here.
                            while (dr.Read()) {

                                sw.WriteLine(dr.GetInt32(icrop_year).ToString() + "," +
                                dr.GetString(ishid) + "," +
                                dr.GetString(iaddress_no) + "," +
                                dr.GetInt32(ipayment_no).ToString() + "," +
                                dr.GetString(iaddress_contact_name) + "," +
                                dr.GetString(iaddress_business_name) + "," +
                                dr.GetString(iaddress_line_1) + "," +
                                dr.GetString(iaddress_line_2) + "," +
                                dr.GetString(iaddress_city) + "," +
                                dr.GetString(iaddress_state) + "," +
                                dr.GetString(iaddress_zip_code) + "," +
                                dr.GetString(ipayee) + "," +
                                dr.GetDecimal(ipayment_amount).ToString("0.00"));
                            }

                            dr.Close();
                            warnMsg = spParams[4].Value.ToString();

                            sw.Close();
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscex);
            }
        }
    }

    public class rptDirectDeliveryStatement {

        private const string MOD_NAME = "rptDirectDeliveryStatement.";
        private static float[] _bodyTableLayout = new float[] { 540F };                                             // total must be 540
        private static float[] _detailTableLayout = new float[] { 45F, 45F, 70F, 65F, 100F, 100F, 65F, 50F};        // total must be 540

        private static Font _normalFont = FontFactory.GetFont("HELVETICA", 10F, Font.NORMAL);
        private static Font _normalItalicFont = FontFactory.GetFont("HELVETICA", 10F, Font.ITALIC);
        private static Font _labelFont = FontFactory.GetFont("HELVETICA", 10F, Font.BOLD);
        private static Font _titleFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
        private static Font _uspsFont = FontFactory.GetFont("HELVETICA", 12F, Font.NORMAL);

        private const string HEADER_DATE_FORMAT = "MMMM dd, yyyy";

        public static string ReportPackager(int cropYear, DateTime fromDate, DateTime toDate, string shid, string fileName,
            string logoUrl, string pdfTempFolder) {

                const string METHOD_NAME = "ReportPackager";

                DirectoryInfo pdfDir = null;
                FileInfo[] pdfFiles = null;
                string filePath = "";

            try {

                pdfDir = new DirectoryInfo(pdfTempFolder);

                // Build the output file name by getting a list of all PDF files 
                // that begin with this session ID: use this as a name incrementer.				
                pdfFiles = pdfDir.GetFiles(fileName + "*.pdf");
                fileName += "_" + Convert.ToString(pdfFiles.Length + 1) + ".pdf";

                filePath = pdfDir.FullName + @"\" + fileName;

                try {

                    using (System.IO.FileStream fs = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                        ReportBuilder(cropYear, fromDate, toDate, shid, filePath, logoUrl, fs);
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

                string errMsg = "Crop Year: " + cropYear.ToString() + "; " +
                    "from date: " + fromDate.ToShortDateString() + "; " +
                    "to date: " + toDate.ToShortDateString() + ": " +
                    "shid: " + shid + "; " +
                    "fileName: " + fileName + "; " +
                    "filesPath: " + filePath;

                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("MReports.rptEquityPayment: " + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(int cropYear, DateTime fromDate, DateTime toDate, string shid, string filePath, string logoUrl, System.IO.FileStream fs) {

            const string METHOD_NAME = "ReportBuilder: ";

            string lastPayee = "";
            decimal subTotalPayAmt;
            decimal subTotalNinetySixPct;
            decimal subTotalFirstNetTons;
            string rptTitle = cropYear.ToString() + " Direct Deliveries";

            Document document = null;
            PdfWriter writer = null;
            PdfPTable detailTable = null;
            PdfPTable bodyTable = null;
            iTextSharp.text.Image imgLogo = null;
            DirectDeliveryStatementEvent pgEvent = null;

            try {

            List<ListDirectDeliveryStatementItem> stateList = RptDirectDeliveryStatementData(cropYear, fromDate, toDate, shid);

                for (int i = 0; i < stateList.Count; i++) {

                    ListDirectDeliveryStatementItem state = stateList[i];

                    if (document == null) {

                        lastPayee = state.Payee;
                        CalcSubTotals(stateList, i, out subTotalFirstNetTons, out subTotalNinetySixPct, out subTotalPayAmt);

                        // IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
                        //  ***  US LETTER: 612 X 792  ***
                        document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
                            PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin, PortraitPageSize.PgBottomMargin);

                        // we create a writer that listens to the document
                        // and directs a PDF-stream to a file				
                        writer = PdfWriter.GetInstance(document, fs);

                        imgLogo = PdfReports.GetImage(logoUrl, 77, 45, iTextSharp.text.Element.ALIGN_CENTER);

                        // Attach my override event handler(s)
                        pgEvent = new DirectDeliveryStatementEvent();
                        pgEvent.FillEvent(state.Shid, state.BusinessName, state.AddrLine1, state.AddrLine2 , state.CSZ, subTotalPayAmt, 0, rptTitle, imgLogo);
                        writer.PageEvent = pgEvent;

                        // Open the document
                        document.Open();

                        bodyTable = PdfReports.CreateTable(_bodyTableLayout, 0);
                        detailTable = PdfReports.CreateTable(_detailTableLayout, 0);

                        AddBodyText(bodyTable, cropYear, fromDate.ToString(HEADER_DATE_FORMAT), toDate.ToString(HEADER_DATE_FORMAT), state.Payee);
                        AddDetailTable(detailTable, stateList, i, subTotalFirstNetTons, subTotalNinetySixPct, subTotalPayAmt);

                        PdfReports.AddTableNoSplit(document, pgEvent, bodyTable);
                        PdfReports.AddTableNoSplit(document, pgEvent, detailTable);
                    }


                    if (lastPayee != state.Payee) {

                        //-------------------------------------------------------------------------------------
                        // When you change members, kick out the page and move on to the next member, 
                        // and reset flags.
                        //-------------------------------------------------------------------------------------

                        lastPayee = state.Payee;

                        bodyTable = PdfReports.CreateTable(_bodyTableLayout, 0);
                        detailTable = PdfReports.CreateTable(_detailTableLayout, 0);

                        CalcSubTotals(stateList, i, out subTotalFirstNetTons, out subTotalNinetySixPct, out subTotalPayAmt);

                        pgEvent.FillEvent(state.Shid, state.BusinessName, state.AddrLine1, state.AddrLine2, state.CSZ, subTotalPayAmt, 0, rptTitle, imgLogo);
                        document.NewPage();

                        AddBodyText(bodyTable, cropYear, fromDate.ToString(HEADER_DATE_FORMAT), toDate.ToString(HEADER_DATE_FORMAT), state.Payee);
                        AddDetailTable(detailTable, stateList, i, subTotalFirstNetTons, subTotalNinetySixPct, subTotalPayAmt);

                        PdfReports.AddTableNoSplit(document, pgEvent, bodyTable);
                        PdfReports.AddTableNoSplit(document, pgEvent, detailTable);
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

        private static void AddBodyText(PdfPTable bodyTable, int cropYear, string fromDate, string toDate, string payeeName) {

            const string METHOD_NAME = "AddBodyText";

            try {

                Paragraph p = new Paragraph("", _normalFont);

                // TEXT                    
                p.Add(new Phrase(16F, "Below is a summary for the direct deliveries ", _normalFont));
                p.Add(new Phrase(16F, payeeName, _labelFont));
                p.Add(new Phrase(16F, " delivered for the " + cropYear.ToString() + " Crop from " + fromDate + " through " + toDate + ".\n\n", _normalFont));

                PdfReports.AddText2Table(bodyTable, p);

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private static void AddDetailTable(PdfPTable detailTable, List<ListDirectDeliveryStatementItem> stateList, int index,
            decimal subTotalFirstNetTons, decimal subTotalNinetySixPct, decimal subTotalPayAmt) {

            const string METHOD_NAME = "AddDetailTable";

            int maxIndex = stateList.Count;
            string payee = stateList[index].Payee;

            try {

                // Create the Header
                PdfReports.AddText2Table(detailTable, "Cont", _labelFont, "center");
                PdfReports.AddText2Table(detailTable, "SHID", _labelFont, "center");
                //PdfReports.AddText2Table(detailTable, "Payee", _labelFont, "center");
                
                PdfReports.AddText2Table(detailTable, "Pay Amt", _labelFont, "center");
                PdfReports.AddText2Table(detailTable, "1st Net Tons", _labelFont, "center");
                PdfReports.AddText2Table(detailTable, "D Station", _labelFont, "center");
                
                PdfReports.AddText2Table(detailTable, "C Station", _labelFont, "center");
                PdfReports.AddText2Table(detailTable, "96%", _labelFont, "center");
                PdfReports.AddText2Table(detailTable, "Rate", _labelFont, "center");

                // Add the Details
                while (index < maxIndex && stateList[index].Payee == payee) {

                    ListDirectDeliveryStatementItem state = stateList[index];

                    PdfReports.AddText2Table(detailTable, state.ContractNumber, _normalFont, "center");
                    PdfReports.AddText2Table(detailTable, state.Shid, _normalFont, "center");
                    //PdfReports.AddText2Table(detailTable, state.Payee, _normalFont, "left");

                    PdfReports.AddText2Table(detailTable, Math.Round(state.PaymentAmount, 2).ToString("0.00"), _normalFont, "right");
                    PdfReports.AddText2Table(detailTable, state.FirstNetTons.ToString("0.0000"), _normalFont, "right");
                    PdfReports.AddText2Table(detailTable, state.DeliveryStationName, _normalFont, "center");

                    PdfReports.AddText2Table(detailTable, state.ContractStationName, _normalFont, "center");
                    PdfReports.AddText2Table(detailTable, state.NinetySixPct.ToString("0.0000"), _normalFont, "right");
                    PdfReports.AddText2Table(detailTable, state.RatePerTon.ToString("0.0000"), _normalFont, "right");

                    index += 1;
                }

                // Add Sub Totals
                PdfReports.AddText2Table(detailTable, " ", _normalFont, "center");
                //PdfReports.AddText2Table(detailTable, " ", _normalFont, "center");
                PdfReports.AddText2Table(detailTable, "Total", _labelFont, "center");

                PdfReports.AddText2Table(detailTable, Math.Round(subTotalPayAmt, 2).ToString("0.00"), _labelFont, "right");
                PdfReports.AddText2Table(detailTable, subTotalFirstNetTons.ToString("0.0000"), _labelFont, "right");
                PdfReports.AddText2Table(detailTable, " ", _normalFont, "center");

                PdfReports.AddText2Table(detailTable, " ", _normalFont, "center");
                PdfReports.AddText2Table(detailTable, subTotalNinetySixPct.ToString("0.0000"), _labelFont, "right");
                PdfReports.AddText2Table(detailTable, " ", _normalFont, "right");

            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private static void CalcSubTotals(List<ListDirectDeliveryStatementItem> stateList, int index, out decimal firstNetTons, out decimal ninetySixPct, out decimal payAmt) {

            const string METHOD_NAME = "CalcSubTotals";

            int maxIndex = stateList.Count;
            string payee = stateList[index].Payee;
            ListDirectDeliveryStatementItem state = null;

            firstNetTons = 0;
            ninetySixPct = 0;
            payAmt = 0;

            try {

                while (index < maxIndex && stateList[index].Payee == payee) {

                    state = stateList[index];
                    firstNetTons += state.FirstNetTons;
                    ninetySixPct += state.NinetySixPct;
                    payAmt += state.PaymentAmount;

                    index += 1;
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private static List<ListDirectDeliveryStatementItem> RptDirectDeliveryStatementData(int cropYear, DateTime fromDate, DateTime toDate, string shid) {

            const string METHOD_NAME = "RptDirectDeliveryStatementData";
            List<ListDirectDeliveryStatementItem> state = new List<ListDirectDeliveryStatementItem>();

            try {

                string shidList = null;
                string shidLo = null;
                string shidHi = null;

                BeetDataDomain.SplitShidList(shid, out shidList, out shidLo, out shidHi);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    string procName = "bawpRptDirectDeliveryStatement";

                    if (conn.State != System.Data.ConnectionState.Open) { conn.Open(); }
                    System.Data.SqlClient.SqlParameter[] spParams = SqlHelperParameterCache.GetSpParameterSet(conn, procName, false);

                    // Manually synch these fields with database.                    
                    spParams[0].Value = cropYear;
                    spParams[1].Value = fromDate;
                    spParams[2].Value = toDate;
                    spParams[3].Value = shidLo;
                    spParams[4].Value = shidHi;
                    spParams[5].Value = shidList;

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, procName, spParams)) {

                        int icontractNumber = dr.GetOrdinal("contract_no");
                        int iShid = dr.GetOrdinal("address_no");
                        int iBusName = dr.GetOrdinal("address_business_name");
                        int iAddrLine1 = dr.GetOrdinal("address_line_1");
                        int iAddrLine2 = dr.GetOrdinal("address_line_2");
                        int icity = dr.GetOrdinal("address_city");
                        int istate = dr.GetOrdinal("address_state");
                        int izipCode = dr.GetOrdinal("address_zip_code");
                        int ipayee = dr.GetOrdinal("payee");
                        int ipayAmt = dr.GetOrdinal("payment_amount");
                        int ifirstNetTons = dr.GetOrdinal("first_net_tons");
                        int ideliveryStation = dr.GetOrdinal("delivery_station_no");
                        int icontractStation = dr.GetOrdinal("contract_station_no");
                        int ininetySixPct = dr.GetOrdinal("nintysix_percent");
                        int iratePerTon = dr.GetOrdinal("rate_per_ton");
                        int icontractStationName = dr.GetOrdinal("contract_station_name");
                        int ideliveryStationName = dr.GetOrdinal("delivery_station_name");

                        while (dr.Read()) {

                            state.Add(new ListDirectDeliveryStatementItem(dr.GetString(icontractNumber), dr.GetString(iShid),
                                dr.GetString(iBusName), dr.GetString(iAddrLine1), dr.GetString(iAddrLine2), dr.GetString(icity),
                                dr.GetString(istate), dr.GetString(izipCode), dr.GetString(ipayee), dr.GetDecimal(ipayAmt),
                                dr.GetDecimal(ifirstNetTons), dr.GetInt32(ideliveryStation), dr.GetInt32(icontractStation),
                                dr.GetDecimal(ininetySixPct), dr.GetDecimal(iratePerTon),
                                dr.GetString(icontractStationName), dr.GetString(ideliveryStationName)));
                        }
                    }
                }

                return state;
            }
            catch (SqlException sqlEx) {

                if (sqlEx.Number == Convert.ToInt32(WSCIEMP.Common.CException.KnownError.DataWarning)) {
                    WSCIEMP.Common.CWarning wscWarn = new WSCIEMP.Common.CWarning(sqlEx.Message, sqlEx);
                    throw (wscWarn);
                } else {
                    string errMsg = MOD_NAME + METHOD_NAME;
                    WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, sqlEx);
                    throw (wscEx);
                }
            }
            catch (System.Exception e) {

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, e);
                throw (wscEx);
            }
        }
    }

    public class DirectDeliveryStatementEvent : PdfPageEventHelper, ICustomPageEvent {

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

    public class ListDirectDeliveryStatementItem {

        public ListDirectDeliveryStatementItem() { }

        public ListDirectDeliveryStatementItem(string contractNumber, string shid, string businessName, string addrLine1,
            string addrLine2, string addrCity, string addrState, string addrZip, string payee, decimal paymentAmount, 
            decimal firstNetTons, int deliveryStation, int contractStation, decimal ninetySixPct, decimal ratePerTon,
            string contractStationName, string deliveryStationName) {

            ContractNumber = contractNumber;
            Shid = shid;
            BusinessName = businessName;
            AddrLine1 = addrLine1;
            AddrLine2 = addrLine2;
            City = addrCity;
            State = addrState;
            ZipCode = addrZip;
            Payee = payee;
            PaymentAmount = paymentAmount;
            FirstNetTons = firstNetTons;
            DeliveryStation = deliveryStation;
            ContractStation = contractStation;
            NinetySixPct = ninetySixPct;
            RatePerTon = ratePerTon;

            ContractStationName = contractStationName;
            DeliveryStationName = deliveryStationName;
        }

        string _contractNumber = "";
        public string ContractNumber {
            get { return _contractNumber; }
            set { _contractNumber = value; }
        }
        string _shid = "";
        public string Shid {
            get { return _shid; }
            set { _shid = value; }
        }
        string _businessName = "";
        public string BusinessName {
            get { return _businessName; }
            set { _businessName = value; }
        }
        string _addrLine1 = "";
        public string AddrLine1 {
            get { return _addrLine1; }
            set { _addrLine1 = value; }
        }
        string _addrLine2 = "";
        public string AddrLine2 {
            get { return _addrLine2; }
            set { _addrLine2 = value; }
        }
        string _city = "";
        public string City {
            get { return _city; }
            set { _city = value; }
        }
        string _state = "";
        public string State {
            get { return _state; }
            set { _state = value; }
        }
        string _zipCode = "";
        public string ZipCode {
            get { return _zipCode; }
            set { _zipCode = value; }
        }
        public string CSZ {
            get {
                return _city + ", " + _state + " " + _zipCode;
            }
        }
        string _payee = "";
        public string Payee {
            get { return _payee; }
            set { _payee = value; }
        }
        decimal _paymentAmount = 0;
        public decimal PaymentAmount {
            get { return _paymentAmount; }
            set { _paymentAmount = value; }
        }
        decimal _firstNetTons = 0;
        public decimal FirstNetTons {
            get { return _firstNetTons; }
            set { _firstNetTons = value; }
        }
        int _deliveryStation = 0;
        public int DeliveryStation {
            get { return _deliveryStation; }
            set { _deliveryStation = value; }
        }
        int _contractStation = 0;
        public int ContractStation {
            get { return _contractStation; }
            set { _contractStation = value; }
        }
        decimal _ninetySixPct = 0;
        public decimal NinetySixPct {
            get { return _ninetySixPct; }
            set { _ninetySixPct = value; }
        }
        decimal _ratePerTon = 0;
        public decimal RatePerTon {
            get { return _ratePerTon; }
            set { _ratePerTon = value; }
        }
        string _contractStationName = "";
        public string ContractStationName {
            get { return _contractStationName; }
            set { _contractStationName = value; }
        }
        string _deliveryStationName = "";
        public string DeliveryStationName {
            get { return _deliveryStationName; }
            set { _deliveryStationName = value; }
        }
    }
}
