using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.MReports {

    public partial class EquityStatement : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.EquityStatement.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((MasterReportTemplate)Master).LocPDF = "";

            try {

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
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");

                string reportDate = txtEsReportDate.Text;
                string paymentCropYear = ((MasterReportTemplate)Master).CropYear;
                string shid = txtEsSHID.Text;
                bool isActive = chkEsActiveOnly.Checked;

                if (reportDate == null || reportDate.Length == 0 || !Common.CodeLib.IsDate(reportDate)) {
                    Common.CWarning warn = new Common.CWarning("You must enter a valid report date, mm/dd/yyyy");
                    throw (warn);
                }

				DateTime activityFromDate = DateTime.MinValue;
				DateTime activityToDate = DateTime.MinValue;

				string testActivityFromDate = txtActivityFromDate.Text;
				string testActivityToDate = txtActivityToDate.Text;

				if (!String.IsNullOrEmpty(testActivityFromDate)) {
					if (!DateTime.TryParse(testActivityFromDate, out activityFromDate)) {
						Common.CWarning warn = new Common.CWarning("The Activity From Date is not a valid date.  Please enter as mm/dd/yyyy");
						throw (warn);
					}
				}

				if (!String.IsNullOrEmpty(testActivityToDate)) {
					if (!DateTime.TryParse(testActivityToDate, out activityToDate)) {
						Common.CWarning warn = new Common.CWarning("The Activity To Date is not a valid date.  Please enter as mm/dd/yyyy");
						throw (warn);
					}
				}

				if (activityFromDate != DateTime.MinValue || activityToDate != DateTime.MinValue) {
					if (activityFromDate == DateTime.MinValue || activityToDate == DateTime.MinValue) {
						if (!DateTime.TryParse(testActivityToDate, out activityToDate)) {
							Common.CWarning warn = new Common.CWarning("When using the Optional Activity Dates, you must enter both the From and To Dates.");
							throw (warn);
						}
					}
					if (activityToDate < activityFromDate) {
						Common.CWarning warn = new Common.CWarning("When using the Optional Activity Dates, the From Date must be less than the To Date");
						throw (warn);
					}
				}

				bool isLienInfoWanted = chkIsLienInfoWanted.Checked;

                WSCReports.rptEquityStatement rptEqStmt = new WSCReports.rptEquityStatement();
                string pdf = rptEqStmt.ReportPackager(Int32.Parse(paymentCropYear), DateTime.Parse(reportDate), shid, isActive, fileName, logoUrl, pdfTempFolder,
					activityFromDate, activityToDate, isLienInfoWanted);

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
