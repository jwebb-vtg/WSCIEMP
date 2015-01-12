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

    public partial class DirectDeliveryStatement : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.DirectDeliveryStatement.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                ((MasterReportTemplate)Master).LocPDF = "";

                ((MasterReportTemplate)Page.Master).CropYearChange += new CommandEventHandler(DoCropYearChange);

                // Sink the Master page event, PrintReady
                ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);

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
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogoIconOnly());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");
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

                string shid = txtShid.Text;

                string pdf = WSCReports.rptDirectDeliveryStatement.ReportPackager(Convert.ToInt32(cropYear), fromDate, toDate,
                    shid, fileName, logoUrl, pdfTempFolder);

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

            const string METHOD_NAME = "DoPrintReady";

            try {

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}