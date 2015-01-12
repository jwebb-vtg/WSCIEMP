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

    public partial class DirectDeliveryPaymentExport : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.DirectDeliveryPaymentExport.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                //((MasterReportTemplate)Master).LocPDF = "";
                lnkPaymentFile.Visible = false;

                ((MasterReportTemplate)Page.Master).CropYearChange += new CommandEventHandler(DoCropYearChange);

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

                // ------------------------------------------------------------------------
                // We need BOTH the url path and file system path to the payment file.
                // ------------------------------------------------------------------------
                string cropYear = ((MasterReportTemplate)Master).CropYear;
                string paymentDesc = Common.UILib.GetDropDownText(ddlPaymentNumber);
                string fileName = cropYear.ToString() + " Payment " + paymentDesc + ".csv";
                string urlPath = WSCReportsExec.GetPDFFolderPath() + @"/" + fileName;
                string filePath = Page.MapPath(urlPath);

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
                WSCReports.rptDirectDeliveryExport.ReportPackager(Convert.ToInt32(cropYear), fromDate, toDate, paymentDescID, filePath, out warnMsg);

                if (warnMsg.Length > 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), warnMsg);
                } 

                lnkPaymentFile.Visible = true;
                lnkPaymentFile.NavigateUrl = urlPath;
                lnkPaymentFile.Text = "Click Here to Open Your Export Payment File";
                Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Payment Export Complete!");

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void DoCropYearChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {
                FillPaymentNumber();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}