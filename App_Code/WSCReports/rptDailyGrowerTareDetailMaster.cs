using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using WSCData;
using PdfHelper;
using WSCIEMP.Common;

namespace WSCReports {

	/// <summary>
	/// Summary description for rptDailyGrowerTareDetailMaster.
	/// </summary>
	public class rptDailyGrowerTareDetailMaster {

		private static readonly Font _headerFont = FontFactory.GetFont("HELVETICA", 12F, Font.BOLD);
		private static readonly Font _normalFont = FontFactory.GetFont("HELVETICA", 8F, Font.NORMAL);
		private static readonly Font _labelFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD);
		private static readonly Font _failFont = FontFactory.GetFont("HELVETICA", 8F, Font.BOLD, BaseColor.RED);

		private const string MOD_NAME = "WSCReports.rptDailyGrowerTareDetailMaster.";
		private static readonly float[] _primaryTableLayout = new float[] { 67.5F, 67.5F, 67.5F, 67.5F, 67.5F, 67.5F, 67.5F, 67.5F };
		private static readonly float[] _tareTableLayout = new float[] { 60.0F, 60.0F, 60.0F, 60.0F, 60.0F, 60.0F, 60.0F, 60.0F, 60.0F };
		private static float[] _processSummaryLayout = new float[] { 540F };

		private static readonly string[] _sampleDetailHdrNames = { "Yard Card", "Sample", "% Sugar", "Unclean Wt.", "Clean Wt.", "% Tare", "High Tare", "Topping", "SLM" };
		private static readonly string[] _truckDetailHdrNames = { "Yard Card", "Delivery Date", "Truck #", "Weight In", "Weight Out", "Dirt Weight", "Dirt Taken", "Net Weight" };

		public static string ReportPackager(int cropYear, DateTime fromDate, DateTime toDate, string factoryList, string stationList,
			string contractList, bool isPosted, bool isPreview, bool isHardCopy, bool isEmail, bool isFax, string fileName, string logoUrl, string pdfTempfolder) {

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

						List<ListGrowerTareItem> hdrList = WSCReportsExec.GrowerDetailReportMasterHdr(cropYear, fromDate, toDate, factoryList,
							stationList, contractList, isPosted, isHardCopy, isEmail, isFax);

						if (hdrList.Count == 0) {
							CWarning warn = new CWarning("No results matched your search criteria.");
							throw (warn);
						}


					using (FileStream fsHardCopy = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {

							ReportBuilder(hdrList, cropYear, isPosted, isPreview, isHardCopy, isEmail, isFax, fsHardCopy, filePath);
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

		public static void ReportBuilder(List<ListGrowerTareItem> hdrList, int cropYear, bool isPosted, bool isPreview, bool isHardCopy, bool isEmail,
			bool isFax, FileStream fs, string filePath) {

			const string METHOD_NAME = "ReportBuilder";
			const int RESET_FLAG = 0;

			Document document = null;
			PdfWriter writer = null;
			DailyGrowerTareDetailEvent pgEvent = null;
			List<ListGrowerTareItem> emailFaxList = null;
			List<ListGrowerTareItem> hardCopyList = null;

			int lastContractID = 0, lastStationID = 0;
			int index = 0;
			string firstDeliveryDate = "", busName = "", address1 = "", address2 = "", CSZ = "", emailAddress = "", faxNumber = "";

			string rptTitle = "Western Sugar Cooperative\nDaily Grower Tare Detail Report";

			try {

				if (!isPreview) {
					emailFaxList = hdrList.FindAll(item => item.RptType == "E" || item.RptType == "F");
					hardCopyList = hdrList.FindAll(item => item.RptType == "M");
				}
				else {
					// In preview mode we're going to lump these all together and print them.
					hardCopyList = hdrList;
				}

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					if (emailFaxList != null && emailFaxList.Count > 0) {
						GrowerTareReportEmailFax(conn, filePath, ref emailFaxList, rptTitle, cropYear);
					}


					for (index = 0; index < hardCopyList.Count; index++) {
					  
						ListGrowerTareItem hdrItem = hardCopyList[index];

						//----------------------------------------------------------------
						// Changed contract or station, start a new print out.
						//----------------------------------------------------------------
						if (hdrItem.ContractID != lastContractID || hdrItem.Delivery_Station_ID != lastStationID) {

							if (document == null) {

								GetAddressInfo(conn, hdrItem.ContractID, out busName, out address1, out address2, out CSZ,
											   out emailAddress, out faxNumber);

								// IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
								//  ***  US LETTER: 612 X 792  ***
								document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
														PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin,
														PortraitPageSize.PgBottomMargin);

								// we create a writer that listens to the document
								// and directs a PDF-stream to a file				
								writer = PdfWriter.GetInstance(document, fs);

								// Attach my override event handler(s)
								pgEvent = new DailyGrowerTareDetailEvent();
								pgEvent.FillEvent(hdrItem.Contract_No, busName, hdrItem.Delivery_Factory_No, address1, address2, CSZ,
												  RESET_FLAG, rptTitle + (!isPreview ? "" : " - " + hdrItem.RptType));
								writer.PageEvent = pgEvent;

								// Open the document
								document.Open();
							}

							if (lastContractID != 0) {

								//--------------------------------------------------------------------------
								// Display Truck information for the first delivery day.
								//--------------------------------------------------------------------------
								int loadCount = 0;
								//if (nextDeliveryDate == firstDeliveryDate) {

								try {
									// Get the truck data.
									SqlParameter[] spParams = null;
									using (
										SqlDataReader drTrucks = WSCReportsExec.GrowerDetailReportASH(conn,
										lastContractID, lastStationID, firstDeliveryDate, ref spParams)) {

										rptDailyGrowerTareDetail.AddTruckDetail(ref document, drTrucks, pgEvent);
										drTrucks.Close();

										loadCount = Convert.ToInt32(spParams[3].Value);

										if (loadCount > 0) {

											rptDailyGrowerTareDetail.AddTruckTotals(ref document, loadCount.ToString(),
												Convert.ToInt32(spParams[4].Value).ToString("#,##0"),
												Convert.ToInt32(spParams[5].Value).ToString("#,##0"), pgEvent);
										}
									}
								}
								catch {                                
									hardCopyList[index-1].Success += "Fail: Truck Detail ";
								}
								GetAddressInfo(conn, hdrItem.ContractID, out busName, out address1, out address2, out CSZ,
											   out emailAddress, out faxNumber);

								// New Page !: in Preview mode, append the report type to the title of the report.
								pgEvent.FillEvent(hdrItem.Contract_No, busName, hdrItem.Delivery_Factory_No, address1, address2, CSZ,
												  RESET_FLAG, rptTitle + (!isPreview ? "" : " - " + hdrItem.RptType));
								document.NewPage();

							}

							firstDeliveryDate = hdrItem.Delivery_Date;
							lastStationID = hdrItem.Delivery_Station_ID;
							lastContractID = hdrItem.ContractID;                                                        
						}

						try {
							AddSampleHdr(ref document, hdrItem, cropYear, pgEvent);

							using (
								SqlDataReader drSamples = WSCReportsExec.GrowerDetailReportTares(conn, hdrItem.ContractID, hdrItem.Delivery_Date)) {
								rptDailyGrowerTareDetail.AddSampleDetail(ref document, drSamples, pgEvent);
							}
						}
						catch {
							hardCopyList[index].Success += "Fail: Sample Detail ";
						}
					}

					if (lastContractID != 0) {

						//--------------------------------------------------------------------------
						// Display Truck information for the first delivery day.
						//--------------------------------------------------------------------------
						int loadCount = 0;

						try {
							// Get the truck data.
							SqlParameter[] spParams = null;
							using (SqlDataReader drTrucks = WSCReportsExec.GrowerDetailReportASH(conn,
								lastContractID, lastStationID, firstDeliveryDate, ref spParams)) {

								rptDailyGrowerTareDetail.AddTruckDetail(ref document, drTrucks, pgEvent);
								drTrucks.Close();

								loadCount = Convert.ToInt32(spParams[3].Value);

								if (loadCount > 0) {

									rptDailyGrowerTareDetail.AddTruckTotals(ref document, loadCount.ToString(),
										Convert.ToInt32(spParams[4].Value).ToString("#,##0"), Convert.ToInt32(spParams[5].Value).ToString("#,##0"),
										pgEvent);
								}
							}
						}
						catch {
							hardCopyList[index-1].Success += "Fail: Truck Detail ";
						}
					}
				}

				//------------------------------------
				// Print a Process Summary
				//------------------------------------
				if (document != null && writer != null) {

					pgEvent.IsSummary = true;
					document.NewPage();

					PdfPTable procSumTab = null;
					PrintSummary(document, ref procSumTab, hdrList, isPreview);
					PdfReports.AddTableNoSplit(document, pgEvent, procSumTab);

				} else {

					document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
											PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin,
											PortraitPageSize.PgBottomMargin);

					// we create a writer that listens to the document
					// and directs a PDF-stream to a file				
					writer = PdfWriter.GetInstance(document, fs);

					// Attach my override event handler(s)
					pgEvent = new DailyGrowerTareDetailEvent();
					pgEvent.IsSummary = true;
					writer.PageEvent = pgEvent;

					// Open the document
					document.Open();

					PdfPTable procSumTab = null;
					PrintSummary(document, ref procSumTab, hdrList, isPreview);
					PdfReports.AddTableNoSplit(document, pgEvent, procSumTab);
				}


			// ======================================================
				// Close document
				// ======================================================
				if (document != null) {
					pgEvent.IsDocumentClosing = true;
					document.Close();
					document = null;
				}
			}
			catch (Exception ex) {
				string errMsg = "document is null: " + (document == null).ToString() + "; " +
					"writer is null: " + (writer == null).ToString();
				CException wscex = new CException(METHOD_NAME + errMsg, ex);
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

		private static void PrintSummary(Document document, ref PdfPTable table, List<ListGrowerTareItem> hdrList, bool isPreview) {

			table = PdfReports.CreateTable(_processSummaryLayout, 1);
			Paragraph p;
			Phrase ph;

			if (isPreview) {
				p = new Paragraph("Note: Preview mode was active, so all documents were printed and none were emailed or faxed.\n\n", _normalFont);
				PdfReports.AddText2Table(table, p);
			}

			//-------------------
			// Printed
			//-------------------
			string prefixSep = "";
			Font font = _normalFont;
			p = new Paragraph("\nPrinted:\n", _labelFont);

			var mailList = hdrList.FindAll(item => item.RptType == "M");
			foreach (ListGrowerTareItem item in mailList) {

				if (String.IsNullOrEmpty(item.Success)) {
					ph = new Phrase(prefixSep + item.Contract_No + " (" + 
						item.Delivery_Station_No + ": " + DateTime.Parse(item.Delivery_Date).ToString("MM/dd") + ")", _normalFont);
				} else {
					ph = new Phrase(prefixSep + item.Contract_No + " (" + 
						item.Delivery_Station_No + ": " + 
						DateTime.Parse(item.Delivery_Date).ToString("MM/dd") + " : " + item.Success + ")", _failFont);					
				}
				
				p.Add(ph);

				if (prefixSep.Length == 0) {
					prefixSep = ", ";
				}
			}
			PdfReports.AddText2Table(table, p);

			//-------------------
			// Email
			//-------------------
			prefixSep = "";
			p = new Paragraph("\nEmail:\n", _labelFont);

			var emailList = hdrList.FindAll(item => item.RptType == "E");
			foreach (ListGrowerTareItem item in emailList) {

				if (String.IsNullOrEmpty(item.Success)) {
					ph = new Phrase(prefixSep + item.Contract_No + " (" + item.Delivery_Station_No + ": " + 
					DateTime.Parse(item.Delivery_Date).ToString("MM/dd") + ")", _normalFont);
				} else {
					ph = new Phrase(prefixSep + item.Contract_No + " (" + 
						item.Delivery_Station_No + ": " + 
						DateTime.Parse(item.Delivery_Date).ToString("MM/dd") + " : " + item.Success + ")", _failFont);					
				}
				
				p.Add(ph);

				if (prefixSep.Length == 0) {
					prefixSep = ", ";
				}
			}
			PdfReports.AddText2Table(table, p);

			//-------------------
			// Fax
			//-------------------
			prefixSep = "";
			p = new Paragraph("\nFax:\n", _labelFont);

			var faxList = hdrList.FindAll(item => item.RptType == "F");
			foreach (ListGrowerTareItem item in faxList) {

				if (String.IsNullOrEmpty(item.Success)) {
					ph = new Phrase(prefixSep + item.Contract_No + " (" + 
						item.Delivery_Station_No + ": " + DateTime.Parse(item.Delivery_Date).ToString("MM/dd") + ")", _normalFont);
				} else {
					ph = new Phrase(prefixSep + item.Contract_No + " (" + 
						item.Delivery_Station_No + ": " + 
						DateTime.Parse(item.Delivery_Date).ToString("MM/dd") + " : " + item.Success + ")", _failFont);					
				}
				
				p.Add(ph);

				if (prefixSep.Length == 0) {
					prefixSep = ", ";
				}
			}
			PdfReports.AddText2Table(table, p);

			//-------------------
			// Web View by Member
			//-------------------
			prefixSep = "";
			p = new Paragraph("\nMember Web View (WSCI):\n", _labelFont);

			var webList = hdrList.FindAll(item => item.RptType == "W");
			foreach (ListGrowerTareItem item in webList) {

				if (String.IsNullOrEmpty(item.Success)) {
					ph = new Phrase(prefixSep + item.Contract_No + " (" +
						item.Delivery_Station_No + ": " + DateTime.Parse(item.Delivery_Date).ToString("MM/dd") + ")", _normalFont);
				} else {
					ph = new Phrase(prefixSep + item.Contract_No + " (" +
						item.Delivery_Station_No + ": " +
						DateTime.Parse(item.Delivery_Date).ToString("MM/dd") + " : " + item.Success + ")", _failFont);
				}

				p.Add(ph);

				if (prefixSep.Length == 0) {
					prefixSep = ", ";
				}
			}

			PdfReports.AddText2Table(table, p);
		}

		private static void GrowerTareReportEmailFax(SqlConnection conn, string filePath, ref List<ListGrowerTareItem> emailList, string rptTitle, int cropYear ) {

			const string METHOD_NAME = "GrowerTareReportEmailFax";
			const string SEND_RPT_SUBJECT = "WESTERN SUGAR COOPERATIVE - Daily Grower Tare Detail Report";
			const int RESET_FLAG = 0;

			FileStream fs = null;
			Document document = null;
			PdfWriter writer = null;
			DailyGrowerTareDetailEvent pgEvent = null;

			int lastContractID = 0, lastStationID = 0;
			int index = 0;
			string destinationFile = "", lastRptType = "";
			string firstDeliveryDate = "", busName = "", address1 = "", address2 = "", CSZ = "", emailAddress = "", faxNumber = "";

			try {

				for (index = 0; index < emailList.Count; index++) {

					ListGrowerTareItem hdrItem = emailList[index];

					//----------------------------------------------------------------
					// Changed contract or station, start a new print out.
					//----------------------------------------------------------------
					if (hdrItem.ContractID != lastContractID || hdrItem.Delivery_Station_ID != lastStationID) {
						
						if (document == null) {

							GetAddressInfo(conn, hdrItem.ContractID, out busName, out address1, out address2, out CSZ,
											out emailAddress, out faxNumber);

							// IF YOU CHANGE MARGINS, CHANGE YOUR TABLE LAYOUTS !!!
							//  ***  US LETTER: 612 X 792  ***
							document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
													PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin,
													PortraitPageSize.PgBottomMargin);

							destinationFile = filePath.Substring(0, filePath.Length - 4) + "_" + hdrItem.Delivery_Station_No + "_" + hdrItem.Contract_No.ToString() + ".pdf";

							// we create a writer that listens to the document
							// and directs a PDF-stream to a file
							if (File.Exists(destinationFile)) {
								File.Delete(destinationFile);
							}
							fs = new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.Read);				
							writer = PdfWriter.GetInstance(document, fs);

							// Attach my override event handler(s)
							pgEvent = new DailyGrowerTareDetailEvent();
							pgEvent.FillEvent(hdrItem.Contract_No, busName, hdrItem.Delivery_Factory_No, address1, address2, CSZ,
												RESET_FLAG, rptTitle);
							writer.PageEvent = pgEvent;

							// Open the document
							document.Open();
						}

						if (lastContractID != 0) {

							//--------------------------------------------------------------------------
							// Display Truck information for the first delivery day.
							//--------------------------------------------------------------------------
							int loadCount = 0;

							try {
								// Get the truck data.
								SqlParameter[] spParams = null;
								using (
									SqlDataReader drTrucks = WSCReportsExec.GrowerDetailReportASH(conn,
										lastContractID, lastStationID, firstDeliveryDate, ref spParams)) {

									rptDailyGrowerTareDetail.AddTruckDetail(ref document, drTrucks, pgEvent);
									drTrucks.Close();

									loadCount = Convert.ToInt32(spParams[3].Value);

									if (loadCount > 0) {

										rptDailyGrowerTareDetail.AddTruckTotals(ref document, loadCount.ToString(),
											Convert.ToInt32(spParams[4].Value).ToString("#,##0"),
											Convert.ToInt32(spParams[5].Value).ToString("#,##0"), pgEvent);
									}
								}
							}
							catch {
								emailList[index - 1].Success += "Fail: Truck Detail ";
							}

							// Save File & Send File
							// ======================================================
							// Close document and write effectively saves the file
							// ======================================================
							if (document != null) {
								if (pgEvent != null) {
									pgEvent.IsDocumentClosing = true;
								}
								document.Close();
								document = null;
							}
							if (writer != null) {
								writer.Close();
								writer = null;
							}
							fs.Close();
							fs = null;

							// Send Report File
							if (lastRptType == "E") {

								if (!SendEmailReport(SEND_RPT_SUBJECT,
									ConfigurationManager.AppSettings["email.target.employeeServiceFrom"].ToString(), emailAddress, destinationFile)) {

									emailList[index - 1].Success += "Fail: Email ";
								}
							} else {
							
								// Send FAX
								if (!SendFaxReport(SEND_RPT_SUBJECT, 
									ConfigurationManager.AppSettings["email.target.employeeServiceFrom"].ToString(), 
									"", faxNumber, busName, "", busName, destinationFile)) {

									emailList[index - 1].Success += "Fail: Fax ";
								}

							}

							GetAddressInfo(conn, hdrItem.ContractID, out busName, out address1, out address2, out CSZ,
											out emailAddress, out faxNumber);

							// New document needed.
							document = new Document(PortraitPageSize.PgPageSize, PortraitPageSize.PgLeftMargin,
													PortraitPageSize.PgRightMargin, PortraitPageSize.PgTopMargin,
													PortraitPageSize.PgBottomMargin);

							destinationFile = filePath.Substring(0, filePath.Length - 4) + "_" + hdrItem.Delivery_Station_No + "_" + hdrItem.Contract_No.ToString() + ".pdf";

							// we create a writer that listens to the document
							// and directs a PDF-stream to a file
							if (File.Exists(destinationFile)) {
								File.Delete(destinationFile);
							}
							fs = new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.Read);
							writer = PdfWriter.GetInstance(document, fs);

							// Attach my override event handler(s)
							pgEvent = new DailyGrowerTareDetailEvent();
							pgEvent.FillEvent(hdrItem.Contract_No, busName, hdrItem.Delivery_Factory_No, address1, address2, CSZ,
												RESET_FLAG, rptTitle);
							writer.PageEvent = pgEvent;

							// Open the document
							document.Open();

						}

						firstDeliveryDate = hdrItem.Delivery_Date;
						lastStationID = hdrItem.Delivery_Station_ID;
						lastContractID = hdrItem.ContractID;
						lastRptType = hdrItem.RptType;
					}

					try {
						AddSampleHdr(ref document, hdrItem, cropYear, pgEvent);

						using (
							SqlDataReader drSamples = WSCReportsExec.GrowerDetailReportTares(conn, hdrItem.ContractID, hdrItem.Delivery_Date)) {
							rptDailyGrowerTareDetail.AddSampleDetail(ref document, drSamples, pgEvent);
						}
					}
					catch {
						emailList[index].Success += "Fail: Sample Detail ";
					}
				}

				if (lastContractID != 0) {

					//--------------------------------------------------------------------------
					// Display Truck information for the first delivery day.
					//--------------------------------------------------------------------------
					int loadCount = 0;

					try {
						// Get the truck data.
						SqlParameter[] spParams = null;
						using (SqlDataReader drTrucks = WSCReportsExec.GrowerDetailReportASH(conn,
							lastContractID, lastStationID, firstDeliveryDate, ref spParams)) {

							rptDailyGrowerTareDetail.AddTruckDetail(ref document, drTrucks, pgEvent);
							drTrucks.Close();

							loadCount = Convert.ToInt32(spParams[3].Value);

							if (loadCount > 0) {

								rptDailyGrowerTareDetail.AddTruckTotals(ref document, loadCount.ToString(),
									Convert.ToInt32(spParams[4].Value).ToString("#,##0"), Convert.ToInt32(spParams[5].Value).ToString("#,##0"),
									pgEvent);
							}
						}
					}
					catch {
						emailList[index - 1].Success += "Fail: Truck Detail ";
					}
				}

				// Save File & Send File
				// ======================================================
				// Close document and write effectively saves the file
				// ======================================================
				if (document != null) {
					if (pgEvent != null) {
						pgEvent.IsDocumentClosing = true;
					}
					document.Close();
					document = null;
				}
				if (writer != null) {
					writer.Close();
					writer = null;
				}
				fs.Close();
				fs = null;

				// Send Report File
				if (lastRptType == "E") {

					if (!SendEmailReport(SEND_RPT_SUBJECT,
						ConfigurationManager.AppSettings["email.target.employeeServiceFrom"].ToString(), emailAddress, destinationFile)) {

						emailList[index - 1].Success += "Fail: Email ";
					}
				} else {

					// Send FAX
					if (!SendFaxReport(SEND_RPT_SUBJECT,
						ConfigurationManager.AppSettings["email.target.employeeServiceFrom"].ToString(),
						"", faxNumber, busName, "", busName, destinationFile)) {

						emailList[index - 1].Success += "Fail: FAx ";
					}

				}
			}
			catch (Exception ex) {
				string errMsg = "document is null: " + (document == null).ToString() + "; " +
					"writer is null: " + (writer == null).ToString();
				CException wscex = new CException(METHOD_NAME + errMsg, ex);
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

		public static void GetAddressInfo(SqlConnection conn, int contractID, out string busName, out string address1, out string address2, 
			out string CSZ, out string emailAddress, out string faxNumber) {

			using (SqlDataReader drAddr = WSCReportsExec.GrowerDetailReportAddr(conn, contractID)) {

				if (drAddr.Read()) {

					busName = drAddr.GetString(drAddr.GetOrdinal("Business_Name"));
					address1 = drAddr.GetString(drAddr.GetOrdinal("Address_1"));
					address2 = drAddr.GetString(drAddr.GetOrdinal("Address_2"));
					CSZ = drAddr.GetString(drAddr.GetOrdinal("City")) + ", " +
						  drAddr.GetString(drAddr.GetOrdinal("State")) + " " +
						  drAddr.GetString(drAddr.GetOrdinal("Zip"));
					emailAddress = drAddr.GetString(drAddr.GetOrdinal("EmailAddress"));
					faxNumber = drAddr.GetString(drAddr.GetOrdinal("FaxNumber"));

				} else {
					busName = "";
					address1 = "";
					address2 = "";
					CSZ = "";
					emailAddress = "";
					faxNumber = "";
				}
			}
		}

		private static void AddSampleHdr(ref Document document, ListGrowerTareItem hdrItem, int cropYear, DailyGrowerTareDetailEvent pgEvent) {

			PdfPTable table = PdfReports.CreateTable(_primaryTableLayout, 1);

			PdfReports.AddText2Table(table, "Delivery Date", _normalFont);
			PdfReports.AddText2Table(table, hdrItem.Delivery_Date, _normalFont);
			PdfReports.AddText2Table(table, "1st Net Lbs", _normalFont);
			PdfReports.AddText2Table(table, hdrItem.First_Net_Pounds.ToString(), _normalFont);
			PdfReports.AddText2Table(table, " ", _normalFont);
			PdfReports.AddText2Table(table, "% Sugar", _normalFont);
			PdfReports.AddText2Table(table, hdrItem.Sugar_Content.ToString("0.00"), _normalFont);
			PdfReports.AddText2Table(table, " ", _normalFont);

			PdfReports.AddText2Table(table, "Station No", _normalFont);
			PdfReports.AddText2Table(table, hdrItem.Delivery_Station_No, _normalFont);
			PdfReports.AddText2Table(table, "Tare Lbs", _normalFont);
			PdfReports.AddText2Table(table, hdrItem.Tare_Pounds.ToString(), _normalFont);
			PdfReports.AddText2Table(table, hdrItem.Tare.ToString("0.00"), _normalFont);
			PdfReports.AddText2Table(table, "SLM", _normalFont);
			PdfReports.AddText2Table(table, hdrItem.SLM_Pct.ToString("0.0000"), _normalFont);
			PdfReports.AddText2Table(table, " ", _normalFont);

			PdfReports.AddText2Table(table, "Station Name", _normalFont);
			PdfReports.AddText2Table(table, hdrItem.Delivery_Station_Name, _normalFont);
			PdfReports.AddText2Table(table, "Final Net Lbs", _normalFont);
			PdfReports.AddText2Table(table, hdrItem.Final_Net_Pounds.ToString(), _normalFont);
			PdfReports.AddText2Table(table, " ", _normalFont);
			PdfReports.AddText2Table(table, "Lbs Extractable Sugar/Ton", _normalFont, 2);
			PdfReports.AddText2Table(table, hdrItem.ExSugarPerTon.ToString(), _normalFont);
			PdfReports.AddText2Table(table, " ", _labelFont, _primaryTableLayout.Length);
			PdfReports.AddTableNoSplit(document, pgEvent, table);

		}

		private static bool SendEmailReport(string subject, string fromEmail, string toEmail, string pathAttach) {

			const string METHOD_NAME = "SendEmailReport";

			try {

				string message = "The attached PDF file contains your Western Sugar Cooperative report.";

				WSCIEMP.Common.AppHelper.SendEmailWithAttach(
					ConfigurationManager.AppSettings["email.smtpServer"].ToString(),
					ConfigurationManager.AppSettings["email.smtpServerPort"].ToString(),
					ConfigurationManager.AppSettings["email.smtpUser"].ToString(),
					ConfigurationManager.AppSettings["email.smtpPassword"].ToString(),
					fromEmail, toEmail, "", "", subject, message, pathAttach);

				return true;
			}
			catch (Exception ex) {

				return false;
			}
		}

		private static bool SendFaxReport(string subject, string fromName, string fromVoiceNumber, string toFaxNumber, string toFaxName, 
		string toVoiceNumber, string toFaxBusName, string pathAttach) {

			const string METHOD_NAME = "SendFaxReport";

			try {

				string message = "The attached PDF file contains your Western Sugar Cooperative report.";

				//WSCIEMP.Common.AppHelper.SendFax(
				//    WSCIEMP.Config.Local.Settings.EmailSettings.FaxServer, 
				//    toFaxName, toFaxNumber, toVoiceNumber, toFaxBusName, 
				//    fromName, fromVoiceNumber, pathAttach, subject);

				return true;
			}
			catch (Exception ex) {

				return false;
			}
		}
	}
}

