using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class PlantPopulation : Common.BasePage {

        private const string MOD_NAME = "Admin.PlantPopulation.";
        private const string BLANK_CELL = "&nbsp;";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                btnDelete.Attributes.Add("onclick", "confirm('Are you sure you want to delete this Row Width?');");

                if (!Page.IsPostBack) {

                    WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    FillRowWidth();
                    FillRowWidthDetails();

                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillRowWidth() {

            const string METHOD_NAME = "FillRowWidth";
            try {

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                grdResults.DataSource = null;
                grdResults.DataBind();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCAdmin.PlantPopulationGetByYear(conn, cropYear)) {

                        grdResults.SelectedIndex = -1;
                        grdResults.DataSource = dr;
                        grdResults.DataBind();
                    }
                }

                if (grdResults.Rows.Count > 0) {
                    grdResults.SelectedIndex = 0;
                } else {
                    grdResults.SelectedIndex = -1;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillRowWidthDetails() {

            const string METHOD_NAME = "FillRowWidthDetails";

            try {

                if (grdResults.SelectedRow != null) {

                    GridViewRow grdRow = grdResults.SelectedRow;

                    txtRowWidth.Text = grdRow.Cells[1].Text.Replace(BLANK_CELL, "");
                    txtBPAFactor.Text = grdRow.Cells[2].Text.Replace(BLANK_CELL, "");
                    txtStandFactor.Text = grdRow.Cells[3].Text.Replace(BLANK_CELL, "");

                } else {

                    txtRowWidth.Text = "";
                    txtBPAFactor.Text = "";
                    txtStandFactor.Text = "";
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void DeletePlantPopulation() {

            const string METHOD_NAME = "DeletePlantPopulation";

            try {

                if (grdResults.SelectedRow == null) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please select a row width item in the grid before trying to Delete.");
                    throw (warn);
                } else {
                    int plantPopulationID = Convert.ToInt32(grdResults.SelectedRow.Cells[0].Text);
                    WSCAdmin.PlantPopulationDelete(plantPopulationID);
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void SavePlantPopulation() {

            const string METHOD_NAME = "SavePlantPopulation";
            try {

                bool isNew = false;

                if (grdResults.Rows.Count == 0) {
                    isNew = true;
                } else {
                    isNew = (grdResults.SelectedIndex == -1);
                }

                int plantPopulationID = 0;
                if (!isNew) {

                    // Updates require a row width selection
                    if (grdResults.SelectedRow != null) {
                        plantPopulationID = Convert.ToInt32(grdResults.SelectedRow.Cells[0].Text);
                    } else {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please select a Row Width entry in the grid.");
                        throw (warn);
                    }
                }

                string rowWidth = txtRowWidth.Text;
                string bpaFactor = txtBPAFactor.Text;
                string standFactor = txtStandFactor.Text;

                WSCAdmin.PlantPopulationSave(plantPopulationID, Convert.ToInt32(ddlCropYear.SelectedValue), rowWidth, bpaFactor, standFactor, WSCSecurity.Identity);
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
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

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {
                FillRowWidth();
                FillRowWidthDetails();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            try {

                int selectedIndex = grdResults.SelectedIndex;
                SavePlantPopulation();
                FillRowWidth();

                if (selectedIndex == -1) {
                    // this was an insert.  Highlight the inserted record
                    grdResults.SelectedIndex = grdResults.Rows.Count - 1;
                } else {
                    // this was an update.  Highlight the updated record
                    grdResults.SelectedIndex = selectedIndex;
                }
                FillRowWidthDetails();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDelete_Click";

            try {

                DeletePlantPopulation();
                FillRowWidth();
                FillRowWidthDetails();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnNew_Click";

            try {

                grdResults.SelectedIndex = -1;
                txtBPAFactor.Text = "";
                txtRowWidth.Text = "";
                txtStandFactor.Text = "";
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdResults_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdResults_SelectedIndexChanged";

            try {
                FillRowWidthDetails();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
