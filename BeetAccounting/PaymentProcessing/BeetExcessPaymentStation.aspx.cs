using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Linq;
using WSCData;

namespace WSCIEMP.BeetAccounting.PaymentProcessing {

	public partial class BeetExcessPaymentStation : Common.BasePage {

		private const string MOD_NAME = "BeetAccounting.PaymentProcessing.BeetExcessPaymentStation.";

		private string _factoryList = "";
		private string _stationList = "";
		private string _contractList = "";
		private string _paymentList = "";

		private enum grdPayDescStationColumns {
            pdecnt_description_contract_id = 0,
			pdecnt_description_id,
			pdecnt_factory_id,
			pdecnt_station_id,
			pdecnt_contract_id,
            pdecnt_rowversion,
            pdecnt_factory_friendly_name,
            pdecnt_station_friendly_name,
			pdecnt_grower_no,		// SHID
			pdecnt_grower_name,
			pdecnt_contract_no,
            pdecnt_payment_friendly_name,            
            pdecnt_excess_beet_pct
		}

		protected void Page_Load(object sender, EventArgs e) {

			const string METHOD_NAME = "Page_Load";

			try {

				Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

				if (!Page.IsPostBack) {
					
					WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
					FillFactory();
					FillStation();
					FillContract();
					FillPayment();
					btnSaveExcessPct.Enabled = false;

				} else {

					//-------------------------------------------------------------------------
					// Getting each list from the native control is odd, but having the
					// selections as CSV type strings if very handy.
					//-------------------------------------------------------------------------
					System.Collections.Specialized.NameValueCollection nv = Page.Request.Form;

					_factoryList = nv[lstFactory.ClientID.Replace("_", "$")];
					_stationList = nv[lstStation.ClientID.Replace("_", "$")];
					_contractList = nv[lstContract.ClientID.Replace("_", "$")];
					_paymentList = nv[lstPayment.ClientID.Replace("_", "$")];

					if (_factoryList == null) { _factoryList = ""; }
					if (_stationList == null) { _stationList = ""; }
					if (_contractList == null) { _contractList = ""; }
					if (_paymentList == null) { _paymentList = ""; }
				}

			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer) {

			// Add click event to GridView do not do this in _RowCreated or _RowDataBound
			AddRowSelectToGridView(grdPayDescStation);
			base.Render(writer);
		}

		private void AddRowSelectToGridView(GridView gv) {

			foreach (GridViewRow row in gv.Rows) {

				row.Attributes["onmouseover"] = "HoverOn(this)";
				row.Attributes["onmouseout"] = "HoverOff(this)";
			}
		}

		private void FillFactory() {

			const string METHOD_NAME = "FillFactory";

			try {

				ArrayList listIDs = new System.Collections.ArrayList(_factoryList.Split(new char[] { ',' }));
				ListBox lst = lstFactory;
				lst.Items.Clear();

				string cropYear = ddlCropYear.Text;
				List<ListBeetFactoryIDItem> stateList = BeetDataDomain.BeetFactoryIDGetList(Convert.ToInt32(cropYear));

				System.Text.StringBuilder sbList = new System.Text.StringBuilder("");
				foreach (ListBeetFactoryIDItem state in stateList) {

					ListItem item = new ListItem(state.FactoryLongName, state.FactoryNumber);
					lst.Items.Add(item);
					if (listIDs.Contains(item.Value)) {

						lst.Items[lst.Items.Count - 1].Selected = true;

						// Rebuild factoryList because the listIDs are now qualified
						sbList.Append(item.Value + ",");
					}
				}
				if (sbList.Length > 0) {
					sbList.Length = sbList.Length - 1;
				}
				_factoryList = sbList.ToString();
			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		private void FillStation() {

			const string METHOD_NAME = "FillStation";

			try {

				string cropYear = ddlCropYear.Text;
				ArrayList listIDs = new System.Collections.ArrayList(_stationList.Split(new char[] { ',' }));
				ListBox lst = lstStation;
				lst.Items.Clear();

				List<ListBeetStationIDItem> stateList = WSCReportsExec.StationGetByFactoryNo(Convert.ToInt32(cropYear), _factoryList);

				System.Text.StringBuilder sbList = new System.Text.StringBuilder("");
				foreach (ListBeetStationIDItem state in stateList) {

					ListItem item = new ListItem(state.StationNumberName, state.StationNumber);
					lst.Items.Add(item);
					if (listIDs.Contains(item.Value)) {

						lst.Items[lst.Items.Count - 1].Selected = true;

						// Rebuild stationList because the listIDs is now qualified against
						// stations assigned to the valid factory list.
						sbList.Append(item.Value + ",");
					}
				}
				if (sbList.Length > 0) {
					sbList.Length = sbList.Length - 1;
				}
				_stationList = sbList.ToString();

			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		private void FillContract() {

			const string METHOD_NAME = "FillContract";

			try {

				string cropYear = ddlCropYear.Text;
				ArrayList listIDs = new System.Collections.ArrayList(_contractList.Split(new char[] { ',' }));
				ListBox lst = lstContract;
				lst.Items.Clear();

				List<ListBeetContractIDItem> stateList = WSCReportsExec.ContractsByContractStationNo(Convert.ToInt32(cropYear), _stationList);

				foreach (ListBeetContractIDItem state in stateList) {

					ListItem item = new ListItem(state.ContractNumber, state.ContractNumber);
					lst.Items.Add(item);
					if (listIDs.Contains(item.Value)) {
						lst.Items[lst.Items.Count - 1].Selected = true;
					}
				}
			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		private void FillPayment() {

			const string METHOD_NAME = "FillPayment";
			try {

				lstPayment.Items.Clear();

				lstPayment.DataValueField = "PaymentNumber";
				lstPayment.DataTextField = "PaymentNumDesc";

				// We want non-finished, but use Linq to remove the payments without valid descriptions.
				var payments = BeetDataDomain.GetPaymentDescriptions(Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear)), false)
					.Where(p => p.PaymentDesc != "Blank" 
						&& p.IsFinished == false 
						&& p.IsRequired == true);

				lstPayment.DataSource = payments;
				lstPayment.DataBind();

				if (lstPayment.Items.Count > 0) {
					lstPayment.Items[0].Selected = true;
					//btnProcess.Enabled = true;
				} else {
					//btnProcess.Enabled = false;
				}
			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

			const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

			try {

				ClearResults();
				FillFactory();
				FillStation();
				FillContract();
				FillPayment();

			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		private void ClearResults() {

			btnSaveExcessPct.Enabled = false;
			grdPayDescStation.DataSource = null;
			grdPayDescStation.DataBind();
		}

		protected void lstFactory_SelectedIndexChanged(object sender, EventArgs e) {

			const string METHOD_NAME = "lstFactory_SelectedIndexChanged";

			try {
				
				ClearResults();
				FillStation();
				FillContract();
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		protected void btnSearch_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnSearch_Click";

			try {

				btnSaveExcessPct.Enabled = true;
				FillGridPayDescStation();
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		private void FillGridPayDescStation() {

			const string METHOD_NAME = "FillGridPayDescStation";
			bool isShidSearch = false;

			try {

				grdPayDescStation.SelectedIndex = -1;
								
				int description_station_id = 0;
				string description_id_list = "";

				int factory_no = 0;
				if (lstFactory.SelectedIndex != -1) { 
					factory_no = Convert.ToInt32(Common.UILib.GetListValues(lstFactory));
				}
				string station_no_list = _stationList;
				string contract_no_list = _contractList;
				string payment_no_list = _paymentList;
				int icrop_year = Convert.ToInt32(ddlCropYear.Text);
				string Shid = txtSHID.Text.Replace("&nbsp;", "").Trim();

				if (Shid != "") {
					lstFactory.SelectedIndex = -1;
					lstStation.Items.Clear();
					isShidSearch = true;
				}

				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
					using (SqlDataReader dr = WSCPayment.GetPaymentDescriptionContract(conn, description_station_id, description_id_list, factory_no,
						station_no_list, contract_no_list, payment_no_list, Shid, icrop_year)) {

						grdPayDescStation.DataSource = dr;
						grdPayDescStation.DataBind();

						if (isShidSearch) {

							lstContract.Items.Clear();
							foreach (GridViewRow row in grdPayDescStation.Rows) {

								ListItem item = new ListItem(
									row.Cells[(int)grdPayDescStationColumns.pdecnt_contract_no].Text,
									row.Cells[(int)grdPayDescStationColumns.pdecnt_contract_no].Text
								);
								lstContract.Items.Add(item);	
								lstContract.Items[lstContract.Items.Count-1].Selected = true;							
							}
						}
					}
				}
			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		protected void lstStation_SelectedIndexChanged(object sender, EventArgs e) {

			const string METHOD_NAME = "lstStation_SelectedIndexChanged";

			try {

				ClearResults();
				FillContract();
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		protected void grdPayDescStation_RowCreated(object sender, GridViewRowEventArgs e) {

			//========================================================================
			// This is just too funny.  I call it either a BUG or a Design-MISTAKE!
			// In order to Hide a grid row that you want to hold data you must
			// turn visibility off here, after databinding has taken place.  It 
			// seems the control was not designed to understand this basic need.
			//========================================================================
			if (e.Row.RowType != DataControlRowType.EmptyDataRow) {
				e.Row.Cells[(int)grdPayDescStationColumns.pdecnt_description_contract_id].CssClass = "DisplayNone";
				e.Row.Cells[(int)grdPayDescStationColumns.pdecnt_rowversion].CssClass = "DisplayNone";

				e.Row.Cells[(int)grdPayDescStationColumns.pdecnt_description_id].CssClass = "DisplayNone";
				e.Row.Cells[(int)grdPayDescStationColumns.pdecnt_factory_id].CssClass = "DisplayNone";
				e.Row.Cells[(int)grdPayDescStationColumns.pdecnt_station_id].CssClass = "DisplayNone";
				e.Row.Cells[(int)grdPayDescStationColumns.pdecnt_contract_id].CssClass = "DisplayNone";
			}
		}

		protected void btnSaveExcessPct_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnSaveExcessPct_Click";

			try {

				WSCSecurity auth = Globals.SecurityState;  
				string userName = auth.UserName;
				string tmpValue = "";

				// Iterate on each grid row and save that row back to the database using
				// the entered Excess Percentage.
				foreach (GridViewRow row in grdPayDescStation.Rows) {

					tmpValue = row.Cells[(int)grdPayDescStationColumns.pdecnt_description_contract_id].Text.Replace("&nbsp;", "0");
					int pdecnt_description_contract_id = 0;
					if (!int.TryParse(tmpValue, out pdecnt_description_contract_id)) {
						Common.CWarning warn = new Common.CWarning("Cannot read grid value pdecnt_description_contract_id");
						throw (warn);
					}

					tmpValue = row.Cells[(int)grdPayDescStationColumns.pdecnt_description_id].Text.Replace("&nbsp;", "");
					int pdecnt_description_id = 0;
					if (tmpValue != "" && !int.TryParse(tmpValue, out pdecnt_description_id)) {
						Common.CWarning warn = new Common.CWarning("Cannot read grid value pdecnt_description_id");
						throw (warn);
					}


					tmpValue = row.Cells[(int)grdPayDescStationColumns.pdecnt_factory_id].Text.Replace("&nbsp;", "");
					int pdecnt_factory_id = 0;
					if (tmpValue != "" && !int.TryParse(tmpValue, out pdecnt_factory_id)) {
						Common.CWarning warn = new Common.CWarning("Cannot read grid value pdecnt_factory_id");
						throw (warn);
					}

					tmpValue = row.Cells[(int)grdPayDescStationColumns.pdecnt_station_id].Text.Replace("&nbsp;", "");
					int pdecnt_station_id = 0;
					if (tmpValue != "" && !int.TryParse(tmpValue, out pdecnt_station_id)) {
						Common.CWarning warn = new Common.CWarning("Cannot read grid value pdecnt_station_id");
						throw (warn);
					}

					tmpValue = row.Cells[(int)grdPayDescStationColumns.pdecnt_contract_id].Text.Replace("&nbsp;", "");
					int pdecnt_contract_id = 0;
					if (tmpValue != "" && !int.TryParse(tmpValue, out pdecnt_contract_id)) {
						Common.CWarning warn = new Common.CWarning("Cannot read grid value pdecnt_contract_id");
						throw (warn);
					}
					
					tmpValue = Common.UILib.GetDropDownText(ddlCropYear);
					int pdecnt_icrop_year = Convert.ToInt32(tmpValue);

					tmpValue = txtExcessPct.Text;
					decimal pdecnt_excess_beet_pct = 0;
					if (!Decimal.TryParse(tmpValue, out pdecnt_excess_beet_pct)) {
						Common.CWarning warn = new Common.CWarning("Please enter a number for Excess Percent.  This can be zero.");
						throw (warn);
					}

					tmpValue = row.Cells[(int)grdPayDescStationColumns.pdecnt_rowversion].Text.Replace("&nbsp;", "");
					string pdecnt_rowversion = tmpValue;
					
					WSCPayment.PaymentDescriptionContractSave( pdecnt_description_contract_id,
						pdecnt_description_id, pdecnt_factory_id, pdecnt_station_id,
						pdecnt_contract_id, pdecnt_icrop_year, pdecnt_excess_beet_pct,
						userName, pdecnt_rowversion);

					FillGridPayDescStation();
				}				
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}
	}
}