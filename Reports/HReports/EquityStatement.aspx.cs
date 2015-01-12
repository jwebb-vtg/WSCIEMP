using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.HReports {

    public partial class EquityStatement : Common.BasePage {

        private const string MOD_NAME = "HReports.EquityStatement.";

        protected void Page_Load(object sender, EventArgs e) {

            //const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((HarvestReportTemplate)Master).LocPDF = "";

            ((HarvestReportTemplate)Master).PrintReady += new CommandEventHandler(DoPrintReady);
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((HarvestReportTemplate)Master).ReportName.Replace(" ", "");

                int cropYear = Convert.ToInt32(((HarvestReportTemplate)Master).CropYear);
                string shid = ((HarvestReportTemplate)Master).SHID.ToString();

				DateTime activityFromDate = DateTime.MinValue;
				DateTime activityToDate = DateTime.MinValue;
				bool isLienInfoWanted = false;

                WSCReports.rptEquityStatement rptEqStmt = new WSCReports.rptEquityStatement();
                string pdf = rptEqStmt.ReportPackager(cropYear, DateTime.Now, shid, false, fileName, logoUrl, pdfTempFolder,
					activityFromDate, activityToDate, isLienInfoWanted);

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
    }
}
