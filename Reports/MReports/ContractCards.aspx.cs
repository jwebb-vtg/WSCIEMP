using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.MReports {

    public partial class ContractCards : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.ContractCards.";

        protected void Page_Load(object sender, EventArgs e) {

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((MasterReportTemplate)Master).LocPDF = "";

            // Sink the Master page event, PrintReady
            ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = txtCcFileName.Text;

                // Check required fields: crop year, start #, stop #, file name.
                string contractNumberStart = txtCcContractNumberStart.Text;
                if (contractNumberStart.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter the First Contract Number.");
                    throw (warn);
                }
                string contractNumberStop = txtCcContractNumberStop.Text;
                if (contractNumberStop.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter the Last Contract Number.");
                    throw (warn);
                }
                if (fileName.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter a file name.");
                    throw (warn);
                }

                string cropYear = ((MasterReportTemplate)Master).CropYear;
                string pdf = WSCReports.rptContractCards.ReportPackager(cropYear, contractNumberStart, contractNumberStop, fileName, logoUrl, pdfTempFolder);

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