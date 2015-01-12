using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.HReports {

    public partial class TonsByTruckByContract : Common.BasePage {

        private const string MOD_NAME = "Reports.HReports.TonsByTruckByContract.";

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

                string cropYear = ((HarvestReportTemplate)Master).CropYear;

                int shid = ((HarvestReportTemplate)Master).SHID;
                if (shid == 0) {
                    Common.CWarning warn = new Common.CWarning("You must first Find a SHID.");
                    throw (warn);
                }

                string contractList = Common.UILib.GetListText(lstTtcContract, ",");
                if (contractList.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("You must first select a contract.");
                    throw (warn);
                }

                bool isCSV = radTtcPrintCSV.Checked;
                string pdf = WSCReports.rptTonsByTruckByContract.ReportPackager(Convert.ToInt32(cropYear), shid, contractList.Replace(" ", ""), isCSV, fileName, logoUrl, pdfTempFolder);

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


                lstTtcContract.Items.Clear();

                WSCSecurity auth = Globals.SecurityState;
                string cropYear = ((HarvestReportTemplate)Master).CropYear;
                int memberID = ((HarvestReportTemplate)Master).MemberID;

                List<ListBeetContractIDItem> stateList = WSCReportsExec.GetContractListSecure(memberID, Convert.ToInt32(cropYear), auth.UserID);

                lstTtcContract.DataSource = stateList;
                lstTtcContract.DataTextField = "ContractNumber";
                lstTtcContract.DataBind();
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
