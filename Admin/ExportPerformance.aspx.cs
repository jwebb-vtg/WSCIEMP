using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class ExportPerformance : Common.BasePage {

        private const string MOD_NAME = "Admin.ExportPerformance.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                if (!Page.IsPostBack) {
                    WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnExport_Click";

            try {

                int cropYear = Convert.ToInt32(ddlCropYear.SelectedValue);                

                // Jump to the appropriate export routine.
                if (chkExportAll.Checked) {
                    WSCFieldExport.DeleteAllTables(cropYear);
                    WSCFieldExport.PopulateAllTables(cropYear);
                } else {

                    if (chkExportAgronomy.Checked) {
                        WSCFieldExport.DeleteAgronomyTable(cropYear);
                        WSCFieldExport.PopulateAgronomyTable(cropYear);
                    }
                    if (chkExportContracting.Checked) {
                        WSCFieldExport.DeleteContractingTable(cropYear);
                        WSCFieldExport.PopulateContractingTable(cropYear);
                    }
                    if (chkExportReportCard.Checked) {
                        WSCFieldExport.PopulateGrowerPerformanceTable(cropYear);
                    }

                    if (chkExportDeletePerformance.Checked) {
                        WSCFieldExport.DeletePerformanceTable(cropYear);
                    }
                    if (chkExportDeleteDirt.Checked) {
                        WSCFieldExport.DeleteContractDirtTable(cropYear);
                    }
                    if (chkExportGenPerformance.Checked) {
                        WSCFieldExport.PopulateBeetAccountingPerformance(cropYear);
                    }
                    if (chkExportGenDirt.Checked) {
                        WSCFieldExport.PopulateBeetAccountingPerformance2(cropYear);
                    }
                    if (chkExportPerformanceData.Checked) {
                        WSCFieldExport.PopulatePerformanceTable(cropYear);
                    }
                    if (chkExportDirtData.Checked) {
                        WSCFieldExport.PopulateContractDirtTable(cropYear);
                    }
                }

                Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Export is complete.");
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
