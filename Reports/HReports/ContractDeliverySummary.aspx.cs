using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.HReports {

    public partial class ContractDeliverySummary : Common.BasePage {

        private const string MOD_NAME = "Reports.HReports.ContractDeliverySummary.";

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
                string contractNumber = Common.UILib.GetListText(lstCdsContract, ",");

                // Check required fields: contract number
                if (String.IsNullOrEmpty(contractNumber)) {
                    Common.CWarning warn = new Common.CWarning("You must select a Contract.");
                    throw (warn);
                }

                string cropYear = ((HarvestReportTemplate)Master).CropYear;
                string pdf = WSCReports.rptContractDeliverySummary.ReportPackager(Convert.ToInt32(cropYear), Convert.ToInt32(contractNumber), fileName, logoUrl, pdfTempFolder);

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

        private void FillControls() {

            const string METHOD_NAME = "FillControls";

            try {

                lstCdsContract.Items.Clear();

                WSCSecurity auth = Globals.SecurityState;
                string cropYear = ((HarvestReportTemplate)Master).CropYear;
                int memberID = ((HarvestReportTemplate)Master).MemberID;

                List<ListBeetContractIDItem> stateList = WSCReportsExec.GetContractListSecure(memberID, Convert.ToInt32(cropYear), auth.UserID);

                lstCdsContract.DataSource = stateList;
                lstCdsContract.DataTextField = "ContractNumber";
                lstCdsContract.DataBind();
            }
            catch (System.Exception ex) {
                Common.CException wscEx = new Common.CException(MOD_NAME + METHOD_NAME + ": ", ex);
                throw (wscEx);
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
