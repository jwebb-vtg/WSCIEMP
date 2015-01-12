using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.BeetAccounting.PaymentProcessing {

    public partial class ExportPaymentData : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.PaymentProcessing.ExportPaymentData.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                lnkPaymentFile.Visible = false;
                txtSumExported.Text = "";

                if (!Page.IsPostBack) {
                    WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    FillPaymentNumber();
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillPaymentNumber() {

            const string METHOD_NAME = "FillPaymentNumber";
            try {

                lstPaymentNumber.Items.Clear();

                lstPaymentNumber.DataValueField = "PaymentNumber";
                lstPaymentNumber.DataTextField = "PaymentNumDesc";
                lstPaymentNumber.DataSource = BeetDataDomain.GetPaymentDescriptions(Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear)), false);
                lstPaymentNumber.DataBind();   

                if (lstPaymentNumber.Items.Count > 0) {
                    lstPaymentNumber.Items[0].Selected = true;
                    btnExport.Enabled = true;
                } else {
                    btnExport.Enabled = false;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void DoExport() {

            const string METHOD_NAME = "DoExport";
            const string dblQuote = "\"";

            // Recordset returned by calling either stored procedure.
            const int eepexCrop_year = 1, 
                eepexContract_no = 2,   // this is really SHID/contract no depending on New vs Old proc call
                eepexAddress_no = 3, 
                eepexPayment_no = 4, 
                eepexAdr_contact_name = 5,
                eepexAdr_business_name = 6, 
                eepexAdr_line_1 = 7, 
                eepexAdr_line_2 = 8,
                eepexAdr_city = 9, 
                eepexAdr_state = 10, 
                eepexAdr_zip_code = 11, 
                eepexPayee_name = 12, 
                eepexPayment_amount = 13;

            try {
              
                int paymentDescID = 0;
                string procName = "";
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int countExported = 0;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                
                paymentDescID = Convert.ToInt32(Common.UILib.GetListValues(lstPaymentNumber));

                if (cropYear < 2006) {
                    procName = "s70pay_GetPaymentExport";
                } else {
                    procName = "s70pay_GetPaymentExport2";
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCPayment.GetPaymentExport(conn, procName, paymentDescID, cropYear)) {

                        while (dr.Read()) {

                            //=============================
                            // First time logic: HEADER !!!
                            //=============================
                            if (countExported == 0) {

                                sb.Append(dblQuote);

                                sb.Append(dr.GetName(eepexCrop_year));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexContract_no));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexAddress_no));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexPayment_no));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexAdr_contact_name));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexAdr_business_name));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexAdr_line_1));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexAdr_line_2));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexAdr_city));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexAdr_state));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexAdr_zip_code));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexPayee_name));
                                sb.Append(dblQuote);
                                sb.Append(",");

                                sb.Append(dblQuote);
                                sb.Append(dr.GetName(eepexPayment_amount));
                                sb.Append(dblQuote);

                                sb.Append("\n");

                            }

                            sb.Append(dr.GetInt32(eepexCrop_year).ToString());
                            sb.Append(",");
                            sb.Append(dr.GetString(eepexContract_no));
                            sb.Append(",");
                            sb.Append(dr.GetInt32(eepexAddress_no).ToString("#"));       // SHID
                            sb.Append(",");
                            sb.Append(dr.GetInt32(eepexPayment_no).ToString("#"));
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetString(eepexAdr_contact_name));
                            sb.Append(dblQuote);
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetString(eepexAdr_business_name));
                            sb.Append(dblQuote);
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetString(eepexAdr_line_1));
                            sb.Append(dblQuote);
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetString(eepexAdr_line_2));
                            sb.Append(dblQuote);
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetString(eepexAdr_city));
                            sb.Append(dblQuote);
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetString(eepexAdr_state));
                            sb.Append(dblQuote);
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetString(eepexAdr_zip_code));
                            sb.Append(dblQuote);
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetString(eepexPayee_name));
                            sb.Append(dblQuote);
                            sb.Append(",");
                            sb.Append(dblQuote);
                            sb.Append(dr.GetDecimal(eepexPayment_amount).ToString("#0.00"));
                            sb.Append(dblQuote);
                            sb.Append("\n");

                            countExported += 1;
                        }
                    }
                }

                // ------------------------------------------------------------------------
                // We need BOTH the url path and file system path to the payment file.
                // ------------------------------------------------------------------------
                string paymentDesc = Common.UILib.GetListText(lstPaymentNumber, "");
                string fileName = cropYear.ToString() + " Payment " + paymentDesc + ".csv";
                string urlPath = WSCReportsExec.GetPDFFolderPath() + @"/" + fileName;

                // Convert to file system path
                string filePath = Page.MapPath(urlPath);              

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false)) {
                    sw.Write(sb.ToString());
                }                                   

                txtSumExported.Text = countExported.ToString("#,##0");
                lnkPaymentFile.Visible = true;
                lnkPaymentFile.NavigateUrl = urlPath;
                lnkPaymentFile.Text = "Click Here to Open Your Export Payment File";
                Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Payment Export Complete!");
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnExport_Click";

            try {
                DoExport();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {

                FillPaymentNumber();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
