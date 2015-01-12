using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.HReports {

	public partial class BeetPaymentBreakdown : Common.BasePage {

		private const string MOD_NAME = "HReports.BeetPaymentBreakdown.";
		private WSCShsData _shs = null;

		protected void Page_Load(object sender, EventArgs e) {

			const string METHOD_NAME = "Page_Load";

			Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
			((HarvestReportTemplate)Master).LocPDF = "";

			((HarvestReportTemplate)Master).PrintReady += new CommandEventHandler(DoPrintReady);

			try {

				// Hide Master page Crop Year control
				DropDownList ddlMasterCropYear = (DropDownList)((HarvestReportTemplate)Master).FindControl("ddlCropYear");
				ddlMasterCropYear.CssClass = "DisplayOff";

				HtmlGenericControl lblMasterCropYear = (HtmlGenericControl)((HarvestReportTemplate)Master).FindControl("lblCropYear");
				lblMasterCropYear.Attributes.Add("class", "DisplayOff");

				if (!Page.IsPostBack) {
					radCalYear.Checked = true;
					WSCField.FillCropYear(ddlYear, DateTime.Now.Year.ToString());

					// Only allow years from 2010 forward.
					for (int i = ddlYear.Items.Count - 1; i >= 0; i--) {
						if (Convert.ToInt32(ddlYear.Items[i].Value) < 2010) {
							ddlYear.Items.Remove(ddlYear.Items[i]);
						}
					}
				}
				_shs = Globals.ShsData;
			}
			catch (System.Exception ex) {

				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				if (Common.AppHelper.IsDebugBuild()) {
					Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), wex);
				} else {
					Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Unable to load page correctly at this time.", wex);
					Common.AppHelper.LogException(wex, HttpContext.Current);
				}
			}
		}

		private void DoPrintReady(object sender, CommandEventArgs e) {

			const string METHOD_NAME = "DoPrintReady";

			try {

				string shid = ((HarvestReportTemplate)Master).TextShid;
				string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
				string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
				string fileName = shid + "_" + ((HarvestReportTemplate)Master).ReportName.Replace(" ", "");

				int cropYear = 0;
				int calYear = 0;
				if (radCalYear.Checked) {
					calYear = Convert.ToInt32(ddlYear.Text);
				} else {
					cropYear = Convert.ToInt32(ddlYear.Text);
				}

				int iShid = 0;
				if (!int.TryParse(shid, out iShid)) {
					Common.CWarning warn = new Common.CWarning("Please enter a number for the SHID.  Enter a specific SHID or 0 to return all shareholders.");
					throw(warn);					
				}

				string pdf = WSCReports.rptBeetPaymentBreakdown.ReportPackager(iShid, Convert.ToInt32(cropYear), calYear, fileName, logoUrl, pdfTempFolder);
				if (pdf.Length > 0) {
					// convert file system path to virtual path
					pdf = pdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
				}

				((HarvestReportTemplate)sender).LocPDF = pdf;

			}
			catch (System.Exception ex) {

				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				if (Common.AppHelper.IsDebugBuild()) {
					Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), wex);
				} else {
					Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Unable to load page correctly at this time.", wex);
					Common.AppHelper.LogException(wex, HttpContext.Current);
				}
			}
		}
	}
}
