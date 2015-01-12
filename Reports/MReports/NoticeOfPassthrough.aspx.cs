using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Reports.MReports {

	public partial class NoticeOfPassthrough : Common.BasePage {

	private const string MOD_NAME = "Reports.MReports.NoticeOfPassthrough.";

		protected void Page_Load(object sender, EventArgs e) {

			const string METHOD_NAME = "Page_Load";

			Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

			((MasterReportTemplate)Master).LocPDF = "";

			((MasterReportTemplate)Page.Master).CropYearChange += new CommandEventHandler(DoCropYearChange);

			// Sink the Master page event, PrintReady
			((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);

			try {

				if (!Page.IsPostBack) {
					FillForm();
				} 
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
			}
		}

		private void FillForm() {

			const string METHOD_NAME = "FillForm";

			try {

				ClearForm();

				int cropYear = Convert.ToInt32(((MasterReportTemplate)Master).CropYear);

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					using (SqlDataReader dr = WSCAdmin.PassthroughAllGetByYear(conn, cropYear)) {

						if (dr.Read()) {

							int iFiscalYear = dr.GetOrdinal("pssaFiscalYearEndDate");
							int iPercentApply = dr.GetOrdinal("pssaPercentageToApply");
							int iRatePerTon = dr.GetOrdinal("pssaRatePerTon");
							int iReportDate = dr.GetOrdinal("pssaReportDate");
							int iTaxYear = dr.GetOrdinal("pssaTaxYear");

							// Copy values into form
							txtCropYear.Text = cropYear.ToString();
							txtFiscalYearEndDate.Text = dr.GetString(iFiscalYear);
							txtPctToApply.Text = dr.GetDecimal(iPercentApply).ToString("N3");
							txtRatePerTon.Text = dr.GetDecimal(iRatePerTon).ToString("N6");
							txtReportDate.Text = dr.GetString(iReportDate);
							txtTaxYear.Text = dr.GetInt32(iTaxYear).ToString();
						} else {

							Common.CWarning warn = new Common.CWarning(@"Before Printing you *MUST* set Coop values in Ag Admin\Passthrough Management for Crop Year "
							+ cropYear.ToString());
							throw(warn);
						}
					}
				}
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		private void ClearForm() {

			txtCropYear.Text = "";
			txtPctToApply.Text = "";
			txtRatePerTon.Text = "";
			txtFiscalYearEndDate.Text = "";
			txtReportDate.Text = "";
			txtTaxYear.Text = "";

		}

		private void DoPrintReady(object sender, CommandEventArgs e) {

			const string METHOD_NAME = "DoPrintReady";

			try {

				WSCSecurity auth = Globals.SecurityState;
				string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
				string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
				string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");
				string cropYear = ((MasterReportTemplate)Master).CropYear;

				string pdf = WSCReports.rptNoticeOfPassthrough.ReportPackager(Convert.ToInt32(cropYear), 
					txtSHID.Text, txtFromSHID.Text, txtToSHID.Text, fileName, logoUrl, pdfTempFolder);

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

			const string METHOD_NAME = "DoCropYearChange";

			try {
				FillForm();
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
			}
		}
	}
}