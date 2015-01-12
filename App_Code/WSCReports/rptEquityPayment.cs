using System;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using WSCData;
using PdfHelper;

namespace WSCReports {
    /// <summary>
    /// Summary description for rptEquityPayment.
    /// </summary>
    public class rptEquityPayment {

        public static string ReportPackager(string paymentCropYear, bool isPatronage, string paymentType, string paymentDate, ref string warnings, 
            string fileName, string logoUrl, string pdfTempfolder) {

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
                ReportBuilder(Convert.ToInt32(paymentCropYear), isPatronage, paymentType, paymentDate, filePath, ref warnings);

                return filePath;
            }
            catch (System.Exception ex) {

                string errMsg = "Payment Crop Year: " + paymentCropYear + "; " +
                    "Is Patronage: " + isPatronage.ToString() + "; " +
                    "Payment Type: " + paymentType + "; " +
                    "Payment Date: " + paymentDate + "; " +
                    "fileName: " + fileName + "; " +
                    "pdfDir is null: " + (pdfDir == null).ToString() + "; " +
                    "filesPath: " + filePath;

                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("MReports.rptEquityPayment: " + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(int paymentCropYear, bool isPatronage, string paymentType, string paymentDate, string filePath, ref string warnings) {

            const string METHOD_NAME = "rptEquityPayment.ReportBuilder: ";
            const string QUOTE = "\"";

            // Build the Equity Payment information.
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    System.Data.SqlClient.SqlParameter outParam = null;
                    using (SqlDataReader dr = WSCPayment.GetEquityPaymentExport(conn, paymentCropYear, isPatronage, paymentType, paymentDate, ref outParam)) {

                        using (StreamWriter sw = new StreamWriter(filePath, false)) {

                            while (dr.Read()) {

                                sw.Write(dr.GetInt32(dr.GetOrdinal("Pay_Crop_Year")).ToString() + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Pay_SHID")) + QUOTE + ",");
                                sw.Write(dr.GetInt32(dr.GetOrdinal("Pay_Address_No")).ToString() + ",");
                                sw.Write(dr.GetInt32(dr.GetOrdinal("Pay_Number")) + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Pay_Contact_Name")) + QUOTE + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Pay_Business_Name")) + QUOTE + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Pay_Adr_Line_1")) + QUOTE + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Pay_Adr_Line_2")) + QUOTE + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Pay_Adr_City")) + QUOTE + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Pay_Adr_State")) + QUOTE + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Pay_Adr_Zip_Code")) + QUOTE + ",");
                                sw.Write(QUOTE + dr.GetString(dr.GetOrdinal("Payee_Name")) + QUOTE + ",");
                                sw.WriteLine(dr.GetDecimal(dr.GetOrdinal("Payment_Amount")));

                            }
                            dr.Close();

                            warnings = outParam.Value.ToString();
                            sw.Close();
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(METHOD_NAME, ex);
                throw (wscex);
            }
        }
    }
}
