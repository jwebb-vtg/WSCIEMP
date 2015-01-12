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
    /// Summary description for rptContractCards.
    /// </summary>
    public class rptContractCards {

        public static string ReportPackager(string cropYear, string startContractNumber, string stopContractNumber, string fileName, string logoUrl, string pdfTempfolder) {

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
                ReportBuilder(Convert.ToInt32(cropYear), Convert.ToInt32(startContractNumber),
                    Convert.ToInt32(stopContractNumber), filePath);

                return filePath;
            }
            catch (System.Exception ex) {

                string errMsg = "cropYear: " + cropYear + "; " +
                    "start contract number: " + startContractNumber + "; " +
                    "stop contract number: " + stopContractNumber + "; " +
                    "fileName: " + fileName + "; " +
                    "pdfDir is null: " + (pdfDir == null).ToString() + "; " +
                    "filesPath: " + filePath;

                WSCIEMP.Common.CException wscex = new WSCIEMP.Common.CException("MReports.rptContractCards.ReportPackager: " + errMsg, ex);
                throw (wscex);
            }
        }

        private static void ReportBuilder(int cropYear,
            int startContractNumber, int stopContractNumber, string filePath) {

            const string METHOD_NAME = "rptContractCards.ReportBuilder: ";
            string[] truckChar = new string[]{" ", "A","B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
												 "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
												 "AA","BB", "CC", "DD", "EE", "FF", "GG", "HH", "II", "JJ", "KK",
												 "LL", "MM", "NN", "OO", "PP", "QQ", "RR", "SS", "TT", "UU", "VV", 
												 "WW", "XX", "YY", "ZZ"};

            // Build the contract information.
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetContractCards(conn,
                              Convert.ToInt32(cropYear), Convert.ToInt32(startContractNumber),
                              Convert.ToInt32(stopContractNumber))) {

                        using (StreamWriter sw = new StreamWriter(filePath, false)) {

                            while (dr.Read()) {

                                int truckCount = dr.GetInt16(dr.GetOrdinal("Number_Of_Trucks"));
                                if (truckCount == 0) {
                                    truckCount++;
                                }

                                if (truckCount >= truckChar.Length) {

                                    string badCntNo = dr.GetString(dr.GetOrdinal("Contract_Number"));
                                    string msg = "Contract Number, " + badCntNo + " has " + truckCount.ToString() +
                                        " trucks.  The maximum number of trucks allowed is " +
                                        Convert.ToString(truckChar.Length - 1);
                                    WSCIEMP.Common.CWarning wscex = new WSCIEMP.Common.CWarning(msg);
                                    throw (wscex);
                                }

                                for (int i = 1; i <= truckCount; i++) {

                                    string cntNo = dr.GetString(dr.GetOrdinal("Contract_Number"));
                                    cntNo = cntNo.PadLeft(5);
                                    sw.Write(cntNo + ",");

                                    string truck = truckChar[i];
                                    sw.Write(truck + ",");

                                    string acres = dr.GetInt16(dr.GetOrdinal("Acres_Contracted")).ToString();
                                    sw.Write(acres + ",");

                                    string groNumber = dr.GetString(dr.GetOrdinal("Grower_Number"));
                                    sw.Write(groNumber + ",");

                                    string groName = dr.GetString(dr.GetOrdinal("Grower_Name"));
                                    sw.Write(groName + ",");

                                    string ldoNumber = dr.GetString(dr.GetOrdinal("Landowner_Number"));
                                    sw.Write(ldoNumber + ",");

                                    string ldoName = dr.GetString(dr.GetOrdinal("Landowner_Name"));
                                    sw.Write(ldoName + ",");

                                    // Replace CRLF (ascii 13+10 with a single space
                                    string fieldDesc1 = dr.GetString(dr.GetOrdinal("FieldDesc1")); //.Replace("\r\n", " ");
                                    sw.Write(fieldDesc1 + ",");

                                    // Replace CRLF (ascii 13+10 with a single space
                                    string fieldDesc2 = dr.GetString(dr.GetOrdinal("FieldDesc2")); //.Replace("\r\n", " ");
                                    sw.Write(fieldDesc2 + ",");

                                    string barcode = "X" + cropYear.ToString().Substring(2, 2) +
                                        cntNo + " " + truck.Trim();
                                    sw.WriteLine(barcode);
                                }
                            }
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
