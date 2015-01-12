using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using WSCData;


namespace WSCIEMP.Admin {

	public partial class PassthroughManagement : Common.BasePage {

		private const string MOD_NAME = "Admin.PassthroughManagement.";

		protected void Page_Load(object sender, EventArgs e) {

			const string METHOD_NAME = "Page_Load";

			try {

				Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

				if (!Page.IsPostBack) {
					
					LoadTaxYear();
					LoadCropYear();

					if (bool.Parse((Request["UpdateOk"] != null ? Request["UpdateOk"].ToString() : "false"))) {

						Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Update was Successful.");

						string cropYear = Request["YR"].ToString();
						Common.UILib.SelectDropDown(ddlCropYear, cropYear);
					}

					FillForm();	
				} 										
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		private void LoadTaxYear() {

			const string METHOD_NAME = "LoadTaxYear";

			try {

				ddlTaxYear.Items.Clear();
				int taxYear = DateTime.Now.Year;

				for (int i = taxYear + 1; i >= taxYear - 5; i--) {
					ddlTaxYear.Items.Add(i.ToString());
					if (i == taxYear) {
						ddlTaxYear.Items[ddlTaxYear.Items.Count-1].Selected = true;
					}
				}

			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		private void LoadCropYear() {

			const string METHOD_NAME = "LoadCropYear";

			try {

				ddlCropYear.Items.Clear();
				int cropYear = DateTime.Now.Year;

				for (int i = cropYear + 1; i >= cropYear - 5; i--) {
					ddlCropYear.Items.Add(i.ToString());
					if (i == cropYear) {
						ddlCropYear.Items[ddlCropYear.Items.Count - 1].Selected = true;
					}
				}

			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		private void ClearForm() {
			
			txtPercentageToApply.Text = "0";
			txtRatePerTon.Text = "0";
			txtFiscalYearEndDate.Text = "";
			txtReportDate.Text = "";
			ddlTaxYear.SelectedIndex = 1;

		}

		private void FillForm() {

			const string METHOD_NAME = "FillForm";

			try {

				ClearForm();

				int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

					using (SqlDataReader dr = WSCAdmin.PassthroughAllGetByYear(conn, cropYear)) {

						if (dr.Read()) {

							int iFiscalYear = dr.GetOrdinal("pssaFiscalYearEndDate");
							int iPercentApply = dr.GetOrdinal("pssaPercentageToApply");
							int iRatePerTon = dr.GetOrdinal("pssaRatePerTon");
							int iReportDate = dr.GetOrdinal("pssaReportDate");
							int iTaxYear = dr.GetOrdinal("pssaTaxYear");

							// Copy values into form
							txtFiscalYearEndDate.Text = dr.GetString(iFiscalYear);
							txtPercentageToApply.Text = dr.GetDecimal(iPercentApply).ToString("N3");
							txtRatePerTon.Text = dr.GetDecimal(iRatePerTon).ToString("N6");
							txtReportDate.Text = dr.GetString(iReportDate);
							Common.UILib.SelectDropDown(ddlTaxYear, dr.GetInt32(iTaxYear).ToString());
						}
					}
				}
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnSave_Click";
			int taxYear = 0;
			int cropYear = 0;
			bool ok = false;

			try {

				cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
				taxYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlTaxYear));

				string tmp = txtRatePerTon.Text;
				decimal ratePerTon = 0;
				if (!Decimal.TryParse(tmp, out ratePerTon)) {
					Common.CWarning warn = new Common.CWarning("Please enter a valid Rate Per Ton.");
					throw (warn);
				}

				decimal percentToApply = 0;
				tmp = txtPercentageToApply.Text;
				if (!Decimal.TryParse(tmp, out percentToApply)) {
					Common.CWarning warn = new Common.CWarning("Please enter a valid Percentage to Apply.");
					throw (warn);
				}

				DateTime reportDate = DateTime.MinValue;
				tmp = txtReportDate.Text;
				if (tmp != "") {
					if (!DateTime.TryParse(tmp, out reportDate)) {
						Common.CWarning warn = new Common.CWarning("Please enter a valid Report Date.");
						throw (warn);
					}
				}

				DateTime fiscalYearEndDate = DateTime.MinValue;
				tmp = txtFiscalYearEndDate.Text;
				if (tmp != "") {
					if (!DateTime.TryParse(tmp, out fiscalYearEndDate)) {
						Common.CWarning warn = new Common.CWarning("Please enter a valid Fiscal Year End Date.");
						throw (warn);
					}
				}

				WSCAdmin.PassthroughAllSave(cropYear, taxYear, ratePerTon, percentToApply, 
					reportDate, fiscalYearEndDate, Globals.SecurityState.UserName);

				ok = true;

			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}

			if (ok) {
				Response.Redirect("~/Admin/PassthroughManagement.aspx?YR=" + cropYear.ToString()
					+ "&UpdateOk=true");
			}
		}

		protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

			const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

			try {
				FillForm();
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}
	}
}
