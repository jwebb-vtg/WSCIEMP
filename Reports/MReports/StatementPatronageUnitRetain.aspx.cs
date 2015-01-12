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

    public partial class StatementPatronageUnitRetain : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.StatementPatronageUnitRetain.";

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
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillEquityType() {

            ddlEquityType.Items.Clear();
            ddlEquityType.Items.Add(new ListItem("Patronage", "1"));
            ddlEquityType.Items.Add(new ListItem("Unit Retain", "2"));
            ddlEquityType.SelectedIndex = 0;
        }

        private void FillControls() {

            ddlPatPaymentDesc.Items.Clear();
            ddlRetPaymentDesc.Items.Clear();
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
                    
                    ddlPatPaymentDesc.Items.Add(liDesc);
                    ddlPatStatementDate.Items.Add(liDate);

                } else {
                    // Fill Unit Retain Payment control
                    ddlRetPaymentDesc.Items.Add(liDesc);
                    ddlRetStatementDate.Items.Add(liDate);
                }
            }

            // Handle empty controls.
            if (ddlPatPaymentDesc.Items.Count == 0) {
                ddlPatPaymentDesc.Items.Add("None Available");
                ddlPatStatementDate.Items.Add(" ");
            }
            if (ddlRetPaymentDesc.Items.Count == 0) {
                ddlRetPaymentDesc.Items.Add("None Available");
                ddlRetStatementDate.Items.Add(" ");
            }

            if (Common.UILib.GetDropDownText(ddlEquityType).StartsWith("Pat")) {

                if (ddlPatPaymentDesc.Items.Count > 0) {
                    ddlPatPaymentDesc.SelectedIndex = 0;
                    txtStatementDate.Text = ddlPatStatementDate.Items[0].Text;
                }
                wrapPatPaymentDesc.Attributes.Add("class", "DisplayOn");
                wrapRetPaymentDesc.Attributes.Add("class", "DisplayOff");

            } else {

                if (ddlRetPaymentDesc.Items.Count > 0) {
                    ddlRetPaymentDesc.SelectedIndex = 0;
                    txtStatementDate.Text = ddlRetStatementDate.Items[0].Text;
                }
                wrapPatPaymentDesc.Attributes.Add("class", "DisplayOff");
                wrapRetPaymentDesc.Attributes.Add("class", "DisplayOn");
            }
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                //string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogoIconOnly());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "").Replace(":", "_");
                string cropYear = ((MasterReportTemplate)Master).CropYear;
                string shid = txtPsSHID.Text;
                string fromShid = txtPsFromSHID.Text;
                string toShid = txtPsToSHID.Text;

                //-----------------------------------------------------
                // Given a specific shid, erase any range query.
                //-----------------------------------------------------
                if (shid.Length > 0) {
                    fromShid = "";
                    toShid = "";
                    txtPsFromSHID.Text = fromShid;
                    txtPsToSHID.Text = toShid;
                }

                string paymentType = "";
                string groupType = (Common.UILib.GetDropDownValue(ddlEquityType) == "1" ? "PAT" : "RET");

                if (groupType == "PAT") {
                    paymentType = Common.UILib.GetDropDownText(ddlPatPaymentDesc);
                } else {
                    paymentType = Common.UILib.GetDropDownText(ddlRetPaymentDesc);
                }

                if (paymentType.StartsWith("None")) {
                    Common.CWarning warn = new Common.CWarning("Please select an Equity Type and Payment Type having a Payment Date.");
                    throw (warn);
                }

                string paymentDate = txtStatementDate.Text.Trim();
                if (paymentDate.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Sorry, you need to select a Crop Year, Equity Type and Payment Type having a Payment Date.");
                    throw (warn);
                }

                string pdf = WSCReports.rptStatementPatronageRetain.ReportPackager(Convert.ToInt32(cropYear), groupType, paymentType, paymentDate,
                    shid, fromShid, toShid, fileName, logoUrl, pdfTempFolder);

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
                FillControls();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlEquityType_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlEquityType_SelectedIndexChanged";

            try {
                FillControls();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}