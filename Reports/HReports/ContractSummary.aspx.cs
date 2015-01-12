using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.HReports {

    public partial class ContractSummary : Common.BasePage {

        private const string MOD_NAME = "Reports.HReports.ContractSummary.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {
                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                ((HarvestReportTemplate)Master).LocPDF = "";

                // Sink the Master page events...
                ((HarvestReportTemplate)Master).PrintReady += new CommandEventHandler(DoPrintReady);
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

                int cropYear = Convert.ToInt32(((HarvestReportTemplate)Master).CropYear);
                int memberID = ((HarvestReportTemplate)Master).MemberID;
                int shid = ((HarvestReportTemplate)Master).SHID;
                string busName = ((HarvestReportTemplate)Master).BusName;

                string pdf = WSCReports.rptContractSummary.ReportPackager(cropYear, memberID, shid, busName, fileName, logoUrl, pdfTempFolder);

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
