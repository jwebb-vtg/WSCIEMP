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

    public partial class EquityPayment : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.EquityPayment.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((MasterReportTemplate)Master).LocPDF = "";

            ((MasterReportTemplate)Page.Master).CropYearChange += new CommandEventHandler(DoCropYearChange);

            // Sink the Master page event, PrintReady
            ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);

            try {                               

                if (!Page.IsPostBack) {
                    FillEquityType();
                    FillControls();
                }

                ((MasterReportTemplate)Page.Master).CropYearLabel = "Payment Crop Year:";
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillEquityType() {

            ddlEpEquityType.Items.Clear();
            ddlEpEquityType.Items.Add(new ListItem("Patronage", "1"));
            ddlEpEquityType.Items.Add(new ListItem("Unit Retain", "2"));
            ddlEpEquityType.SelectedIndex = 0;
        }

        private void FillControls() {

            ddlEpPaymentPat.Items.Clear();
            ddlEpPaymentRet.Items.Clear();
            ddlPatStatementDate.Items.Clear();
            ddlRetStatementDate.Items.Clear();
            txtStatementDate.Text = "";

            // Load the Payment controls
            string cropYear = ((MasterReportTemplate)Master).CropYear;
            List<ListEquityPaymentScheduleItem> stateList = BeetEquityDeduction.EquityPaymentSchedule(Convert.ToInt32(cropYear));

            foreach (ListEquityPaymentScheduleItem state in stateList) {

                ListItem liDesc = new ListItem(state.EquityType);
                ListItem liDate = new ListItem(state.PayDate);

                if (state.GroupType == "PAT") {

                    ddlEpPaymentPat.Items.Add(liDesc);
                    ddlPatStatementDate.Items.Add(liDate);

                } else {
                    // Fill Unit Retain Payment control
                    ddlEpPaymentRet.Items.Add(liDesc);
                    ddlRetStatementDate.Items.Add(liDate);
                }
            }

            // Handle empty controls.
            if (ddlEpPaymentPat.Items.Count == 0) {
                ddlEpPaymentPat.Items.Add("None Available");
                ddlPatStatementDate.Items.Add(" ");
            }
            if (ddlEpPaymentRet.Items.Count == 0) {
                ddlEpPaymentRet.Items.Add("None Available");
                ddlRetStatementDate.Items.Add(" ");
            }

            if (Common.UILib.GetDropDownText(ddlEpEquityType).StartsWith("Pat")) {

                if (ddlEpPaymentPat.Items.Count > 0) {
                    ddlEpPaymentPat.SelectedIndex = 0;
                    txtStatementDate.Text = ddlPatStatementDate.Items[0].Text;
                }
                wrapPatList.Attributes.Add("class", "DisplayOn");
                wrapRetList.Attributes.Add("class", "DisplayOff");

            } else {

                if (ddlEpPaymentRet.Items.Count > 0) {
                    ddlEpPaymentRet.SelectedIndex = 0;
                    txtStatementDate.Text = ddlRetStatementDate.Items[0].Text;
                }
                wrapPatList.Attributes.Add("class", "DisplayOff");
                wrapRetList.Attributes.Add("class", "DisplayOn");
            }
        }

        private void DoCropYearChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoCropYearChange";

            try {

                FillControls();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = txtEpFileName.Text;
               
                // Get File Name
                if (fileName.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter a file name.");
                    throw (warn);
                }              

                // Get equity type
                bool isPatronage = Common.UILib.GetDropDownText(ddlEpEquityType).StartsWith("Pat");

                string paymentType = "";           
                if (isPatronage) {

                    try {
                        paymentType = Common.UILib.GetDropDownText(ddlEpPaymentPat);
                    }
                    catch {
                        Common.CWarning warn = new Common.CWarning("Invalid Payment Type");
                        throw (warn);
                    }

                } else {

                    try {
                        paymentType = Common.UILib.GetDropDownText(ddlEpPaymentRet);
                    }
                    catch {
                        Common.CWarning warn = new Common.CWarning("Invalid Payment Type");
                        throw (warn);
                    }
                }
                if (paymentType.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please select a Payment Type.  Payment Type cannot be blank.");
                    throw (warn);
                }

                if (paymentType.StartsWith("None")) {
                    Common.CWarning warn = new Common.CWarning("Please select an Equity Type and Payment Type having a Payment Date.");
                    throw (warn);
                }

                // Get payment Date
                string paymentDate = txtStatementDate.Text.Trim();
                if (paymentDate.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please select an Equity Type and Payment Type having a Payment Date.");
                    throw (warn);
                }
                
                string pdf = "";              
                string badSHIDs = "";

                string cropYear = ((MasterReportTemplate)Master).CropYear;
                pdf = WSCReports.rptEquityPayment.ReportPackager(cropYear, isPatronage, paymentType, paymentDate, ref badSHIDs, fileName, logoUrl, pdfTempFolder);

                if (badSHIDs.Length > 0) {
                    txtEpWarnSHID.Text = badSHIDs;
                }

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
    }
}