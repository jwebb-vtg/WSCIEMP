using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.MReports {

    public partial class ContractPayeeSummary : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.ContractPayeeSummary.";

        protected void Page_Load(object sender, EventArgs e) {

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((MasterReportTemplate)Master).LocPDF = "";

            // Sink the Master page event, PrintReady
            ((MasterReportTemplate)Master).PrintReady += new CommandEventHandler(DoPrintReady);
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");
                string reportDate = txtCpsReportDate.Text;
                string cropYear = ((MasterReportTemplate)Master).CropYear;
                string shid = txtCpsSHID.Text;

                if (reportDate == null || reportDate.Length == 0 || !Common.CodeLib.IsDate(reportDate)) {
                    Common.CWarning warn = new Common.CWarning("You must enter a valid report date, mm/dd/yyyy");
                    throw (warn);
                }

                WSCReports.rptContractPayeeSummary rpt = new WSCReports.rptContractPayeeSummary();
                string pdf = rpt.ReportPackager(Int32.Parse(cropYear), DateTime.Parse(reportDate), shid, fileName, logoUrl, pdfTempFolder);

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
