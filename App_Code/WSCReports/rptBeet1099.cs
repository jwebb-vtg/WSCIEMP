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
using WSCData;
using PdfHelper;

namespace WSCReports {

    public class rptBeet1099 {

        private const string MOD_NAME = "rptBeet1099.";

        public static string ReportPackager(string paymentCropYear, string fileName, string logoUrl, string pdfTempfolder) {

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
                ReportBuilder(Convert.ToInt32(paymentCropYear), filePath);

                return filePath;
            }
            catch (System.Exception ex) {

                string errMsg = "";

                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + ": " + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(int paymentCropYear, string filePath) {

            const string METHOD_NAME = "ReportBuilder";
            const string QUOTE = "\"";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    List<List1099ExportItem> stateList = WSCPayment.GetBeet1099Export(paymentCropYear);

                    using (StreamWriter sw = new StreamWriter(filePath, false)) {

                        foreach (List1099ExportItem state in stateList) {

                            //SHID, TaxID, Business Name, Address 1, Address 2, City, State, Zip, Dollars
                            sw.Write(QUOTE + state.SHID + QUOTE + ",");
                            sw.Write(QUOTE + state.TaxID + QUOTE + ",");
                            sw.Write(QUOTE + state.BusName + QUOTE + ",");
                            sw.Write(QUOTE + state.Address1 + QUOTE + ",");
                            sw.Write(QUOTE + state.Address2 + QUOTE + ",");
                            sw.Write(QUOTE + state.TaxCity + QUOTE + ",");
                            sw.Write(QUOTE + state.TaxState + QUOTE + ",");
                            sw.Write(QUOTE + state.TaxZip + QUOTE + ",");
                            sw.Write(QUOTE + state.TaxDollars + QUOTE + Environment.NewLine);
                        }

                        sw.Close();
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscex);
            }
        }
    }
}
