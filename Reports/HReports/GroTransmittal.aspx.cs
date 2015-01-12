using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.HReports {

    public partial class GroTransmittal : Common.BasePage {

        private const string MOD_NAME = "Reports.HReports.GroTransmittal.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((HarvestReportTemplate)Master).LocPDF = "";

            // Sink the Master page events...
            ((HarvestReportTemplate)Master).PrintReady += new CommandEventHandler(DoPrintReady);
            ((HarvestReportTemplate)Master).ShidChange += new CommandEventHandler(DoShidChange);
            ((HarvestReportTemplate)Master).CropYearChange += new CommandEventHandler(DoCropYearChange);

            try {

                if (!Page.IsPostBack) {
                    FillControls();
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((HarvestReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((HarvestReportTemplate)Master).ReportName.Replace(" ", "");

                bool isCumulative = chkGtIsTransmittalCumulative.Checked;
                string paymentDesc = Common.UILib.GetDropDownText(ddlGtPaymentDesc);
                if (paymentDesc.StartsWith("None")) {
                    Common.CWarning warn = new Common.CWarning("Please select a Payment other than 'None Available'.");
                    throw (warn);
                }
                int paymentNumber = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlGtPaymentDesc));

                string contractList = Common.UILib.GetListText(lstGtContract, ",");
                if (contractList.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please select a Contract.");
                    throw (warn);
                }

                string cropYear = ((HarvestReportTemplate)Master).CropYear;
                string pdf = WSCReports.rptGroTransmittal.ReportPackager(Convert.ToInt32(cropYear), paymentNumber, paymentDesc, null, null,
                    null, "", "", contractList, isCumulative, fileName, logoUrl, pdfTempFolder);

                if (pdf.Length > 0) {
                    // convert file system path to virtual path
                    pdf = pdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                }

                ((HarvestReportTemplate)sender).LocPDF = pdf;
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((HarvestReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillContract() {

            const string METHOD_NAME = "FillContract";

            try {

                lstGtContract.Items.Clear();
                string contractList = Common.UILib.GetListText(lstGtContract, ",");
                string[] list = contractList.Split(new char[] { ',' });
                System.Collections.ArrayList contractIDs = new System.Collections.ArrayList(list);

                string cropYear = ((HarvestReportTemplate)Master).CropYear;
                int shid = ((HarvestReportTemplate)Master).SHID;                

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCReportsExec.ContractsByPayeeNumber(conn, Convert.ToInt32(cropYear), shid)) {

                        while (dr.Read()) {

                            string contractNo = dr.GetInt32(dr.GetOrdinal("cnt_contract_no")).ToString();

                            System.Web.UI.WebControls.ListItem item = new ListItem(contractNo, contractNo);

                            lstGtContract.Items.Add(item);
                            if (contractIDs.Contains(contractNo)) {
                                lstGtContract.Items[lstGtContract.Items.Count - 1].Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillControls() {

            const string METHOD_NAME = "FillControls";

            try {
                FillPaymentDesc();
                FillContract();
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillPaymentDesc() {

            string cropYear = ((HarvestReportTemplate)Master).CropYear;

            ddlGtPaymentDesc.Items.Clear();
            List<ListPaymentDescItem> stateList = BeetDataDomain.GetPaymentDescriptions(Convert.ToInt32(cropYear), true);

            ddlGtPaymentDesc.DataValueField = "PaymentNumber";
            ddlGtPaymentDesc.DataTextField = "PaymentDesc";
            ddlGtPaymentDesc.DataSource = stateList;
            ddlGtPaymentDesc.DataBind();

            // Handle empty controls.
            if (ddlGtPaymentDesc.Items.Count == 0) {
                ddlGtPaymentDesc.Items.Add("None Available");
            }
        }

        private void DoCropYearChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoCropYearChange";

            try {

                FillControls();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((HarvestReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void DoShidChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoShidChange";

            try {

                FillControls();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((HarvestReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
