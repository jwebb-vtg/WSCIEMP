using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using WSCData;

namespace WSCIEMP.SHS {
	public partial class SeedVariety : Common.BasePage {

		private const string MOD_NAME = "SHS.SeedVariety.";
		private const string BLANK_CELL = "&nbsp;";

		protected void Page_Load(object sender, EventArgs e) {
		
            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

				if (!Page.IsPostBack) {

					string lastArea = "";
					bool isAddOk = false;
					bool isDeleteOk = false;					
					try {isDeleteOk = bool.Parse(Request["DeleteOk"].ToString());}
					catch{}

					if (isDeleteOk) {
						Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Record was Deleted");
					} else {
						
						try { isAddOk = bool.Parse(Request["AddOk"].ToString());}
						catch {}

						if (isAddOk) {
							Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Record was Added");
						}
					}

					
					if (isDeleteOk || isAddOk) {
						try { lastArea = Request["lastArea"].ToString(); }
						catch { }
					}

					FillFactoryArea(lastArea);
					FillSeedVarietyGrid();
				}

			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer) {

			// Add click event to GridView do not do this in _RowCreated or _RowDataBound
			AddRowSelectToGridView(grdResults);
			base.Render(writer);
		}

		private void AddRowSelectToGridView(GridView gv) {

			foreach (GridViewRow row in gv.Rows) {

				row.Attributes["onmouseover"] = "HoverOn(this)";
				row.Attributes["onmouseout"] = "HoverOff(this)";
				//row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
				row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
			}
		}

		private void FillFactoryArea(string factoryArea) {

			ddlFactoryArea.Items.Add("NORTH");
			ddlFactoryArea.Items.Add("SOUTH");

			if (factoryArea == "SOUTH") {
				ddlFactoryArea.SelectedIndex = 1;
			} else {
				ddlFactoryArea.SelectedIndex = 0;
			}

		}

		private void FillSeedVarietyGrid() {

			const string METHOD_NAME = "FillSeedVarietyGrid";
			try {

				string factoryArea = ddlFactoryArea.SelectedItem.Text;

				grdResults.DataSource = null;
				grdResults.DataBind();

				DataTable dt = WSCField.SeedVarietyGetByArea(factoryArea);

				grdResults.SelectedIndex = -1;
				grdResults.DataSource = dt;
				grdResults.DataBind();

			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		private void HandleVarietySelect() {

			const string METHOD_NAME = "HandleVarietySelect";

			try {

				GridViewRow grdRow = grdResults.SelectedRow;
				if (grdRow != null) {
					txtVarietyName.Text = grdRow.Cells[0].Text.Replace(BLANK_CELL, "");

				} else {
					txtVarietyName.Text = "";
				}
			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		protected void grdResults_SelectedIndexChanged(object sender, EventArgs e) {

			const string METHOD_NAME = "grdResults_SelectedIndexChanged";

			try {
				HandleVarietySelect();
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

		protected void btnDelete_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnDelete_Click";

			bool isOk= false;
			string factoryArea = "";

			try {

				GridViewRow row = grdResults.SelectedRow;
				if (row == null) {
					Common.CWarning cex = new Common.CWarning("Please select a variety before pressing Delete.");
					throw (cex);
				}

				factoryArea = ddlFactoryArea.SelectedItem.Text;
				string varietyName = row.Cells[0].Text;
				bool isDelete = true;

				WSCField.SeedVarietySave(factoryArea, varietyName, isDelete);
				txtVarietyName.Text = "";
				grdResults.SelectedIndex = -1;
				isOk = true;
					
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}

			if (isOk) {
				Response.Redirect("~/SHS/SeedVariety.aspx?DeleteOk=true&lastArea=" + factoryArea);
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnAdd_Click";
			bool isOk = false;
			string factoryArea = "";

			try {

				string variety = txtVarietyName.Text;
				if (String.IsNullOrEmpty(variety)) {
					Common.CWarning cex = new Common.CWarning("Please enter a variety name before pressing Add.");
					throw(cex);
				}

				// Make sure enterd variety name is not already in the grid.
				foreach(GridViewRow row in grdResults.Rows) {
					if (row.Cells[0].Text == variety) {
						Common.CWarning cex = new Common.CWarning("The Variety Name entered is already in the list.  Please enter a different name or change Factory Area.");
						throw (cex);
					}
				}

				factoryArea = ddlFactoryArea.SelectedItem.Text;
				string varietyName = variety;
				bool isDelete = false;

				WSCField.SeedVarietySave(factoryArea, varietyName, isDelete);
				isOk = true;

			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}

			if (isOk) {
				Response.Redirect("~/SHS/SeedVariety.aspx?AddOk=true&lastArea=" + factoryArea);
			}
		}

		protected void ddlFactoryArea_SelectedIndexChanged(object sender, EventArgs e) {

			const string METHOD_NAME = "ddlFactoryArea_SelectedIndexChanged";

			try {

				FillSeedVarietyGrid();
			}
			catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
			}
		}

	}
}