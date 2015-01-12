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

    public partial class DirectDeliveryDetails : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.DirectDeliveryDetails.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                ((MasterReportTemplate)Master).LocPDF = "";

                // Sink the Master page event, PrintReady
                ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);

                if (!Page.IsPostBack) {
                    FillPaymentNumber();
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillPaymentNumber() {

            const string METHOD_NAME = "FillPaymentNumber";
            try {

                ddlPaymentNumber.Items.Clear();
                string cropYear = ((MasterReportTemplate)Master).CropYear;

                List<ListPaymentDescItem> stateList = BeetDataDomain.GetPaymentDescriptions(Convert.ToInt32(cropYear), false);

                ddlPaymentNumber.DataValueField = "PaymentNumber";
                ddlPaymentNumber.DataTextField = "PaymentDescSpecial";
                ddlPaymentNumber.DataSource = stateList;
                ddlPaymentNumber.DataBind();

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
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                //string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");
                string fileName = txtFileName.Text;
                string cropYear = ((MasterReportTemplate)Master).CropYear;

                string fromDateTest = txtFromDate.Text;
                if (fromDateTest.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter a From Date.");
                    throw (warn);
                }
                DateTime fromDate;
                try {
                    fromDate = Convert.ToDateTime(fromDateTest);
                }
                catch {
                    Common.CWarning warn = new Common.CWarning("Please enter a valid From Date.");
                    throw (warn);
                }

                string toDateTest = txtToDate.Text;
                if (toDateTest.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter a To Date.");
                    throw (warn);
                }
                DateTime toDate;
                try {
                    toDate = Convert.ToDateTime(toDateTest);
                }
                catch {
                    Common.CWarning warn = new Common.CWarning("Please enter a valid To Date.");
                    throw (warn);
                }

                int paymentDescID = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlPaymentNumber));

                string warnMsg;
                string pdf = WSCReports.rptDirectDelivery.ReportPackager(Convert.ToInt32(cropYear), fromDate, toDate,
                    paymentDescID, fileName, pdfTempFolder, out warnMsg);

                if (warnMsg.Length > 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), warnMsg);
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