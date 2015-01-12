using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Reports.MReports {

    public partial class PaymentSummary : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.PaymentSummary.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((MasterReportTemplate)Master).LocPDF = "";

            ((MasterReportTemplate)Page.Master).CropYearChange += new CommandEventHandler(DoCropYearChange);

            // Sink the Master page event, PrintReady
            ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);

            try {

                if (!Page.IsPostBack) {
                    FillPaymentDesc();
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillPaymentDesc() {

            const string METHOD_NAME = "FillPaymentDesc";
            try {

                ddlPsPaymentDesc.Items.Clear();
                ddlPsStatementDate.Items.Clear();
                txtStatementDate.Text = "";
                string cropYear = ((MasterReportTemplate)Master).CropYear;

                List<ListPaymentDescItem> stateList = BeetDataDomain.GetPaymentDescriptions(Convert.ToInt32(cropYear), false);
                

                foreach (ListPaymentDescItem state in stateList) {

                    // Only show payments with transmittal dates.
                    if (state.TransmittalDate.Length > 0) {

                        int paymentID = Convert.ToInt32(state.PaymentDescID);
                        string paymentDesc = state.PaymentDesc;

                        ListItem liDesc = new ListItem(paymentDesc, paymentID.ToString());
                        ddlPsPaymentDesc.Items.Add(liDesc);

                        ddlPsStatementDate.Items.Add(state.TransmittalDate);
                    }
                }

                // Handle empty controls.
                if (ddlPsPaymentDesc.Items.Count == 0) {
                    ddlPsPaymentDesc.Items.Add("None Available");
                    ddlPsStatementDate.Items.Add(" ");
                    txtStatementDate.Text = " ";
                }

                if (ddlPsPaymentDesc.Items.Count > 0) {
                    ddlPsPaymentDesc.SelectedIndex = 0;
                    ddlPsStatementDate.SelectedIndex = 0;
                    txtStatementDate.Text = Common.UILib.GetDropDownText(ddlPsStatementDate);
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");
                string cropYear = ((MasterReportTemplate)Master).CropYear;

                string statementDate = txtStatementDate.Text.Trim();
                string shid = txtPsSHID.Text;
                string fromShid = txtPsFromSHID.Text;
                string toShid = txtPsToSHID.Text;
                string footerText = rptParam_Footer.Value;

                string paymentDesc = Common.UILib.GetDropDownText(ddlPsPaymentDesc);
                if (paymentDesc.StartsWith("None")) {
                    Common.CWarning warn = new Common.CWarning("Please select a Payment having a Payment Date.");
                    throw (warn);
                }

                int paymentID = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlPsPaymentDesc));
                bool isCumulative = chkPsIsPaymentSummaryCumulative.Checked;

                string pdf = WSCReports.rptPaymentSummary.ReportPackager(Convert.ToInt32(cropYear), statementDate, shid, fromShid, toShid,
                    paymentID, isCumulative, footerText, fileName, logoUrl, pdfTempFolder);

                if (pdf.Length > 0) {
                    // convert file system path to virtual path
                    pdf = pdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                }

                ((MasterReportTemplate)sender).LocPDF = pdf;

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void DoCropYearChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoCropYearChange";

            try {
                FillPaymentDesc();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
