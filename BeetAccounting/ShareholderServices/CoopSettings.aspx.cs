using System;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.BeetAccounting.ShareholderServices {

    public partial class CoopSettings : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.ShareholderServices.CoopSettings.";
        private const string BLANK_CELL = "&nbsp;";
        private const string OLD_CROP_YEAR_WARNING = "You cannot Save or Post in this Crop Year.";

        private const int ftyColFactoryNumber = 0, ftyColFactoryName = 1, ftyColIsOverPlantAllowed = 2, ftyColPercentage = 3, ftyColIsPoolingAllowed = 4,
            ftyColPoolSHID = 5, ftyColCutoffDate = 6, ftyColIsPosted = 7;

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                btnPost.Attributes.Add("onclick", "confirm('This will permanently change all Undecided members to No, and cannot be undone.  Do you want to continue?');");

                if (!Page.IsPostBack) {
                    BeetDataDomain.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    FillFactoryGrid();
                }                
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdFactory);
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

        private void FillFactoryGrid() {

            const string METHOD_NAME = "FillFactoryGrid";

            try {

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                grdFactory.SelectedIndex = -1;

                // Use currently selected crop year to fill the over plant factory grid
                List<ListOverPlantFactoryItem> overPlantList = BeetDataOverPlant.OverPlantFactoryByYear(cropYear);

                grdFactory.DataSource = overPlantList;
                grdFactory.DataBind();
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillFactoryEdit() {

            const string METHOD_NAME = "FillFactoryEdit";

            try {

                if (grdFactory.SelectedRow != null) {

                    GridViewRow grdRow = grdFactory.SelectedRow;
                    
                    chkIsOverPlantAllowed.Checked = (grdRow.Cells[ftyColIsOverPlantAllowed].Text == "Yes");
                    txtPercentage.Text = grdRow.Cells[ftyColPercentage].Text.Replace(BLANK_CELL, "");
                    chkIsPoolingAllowed.Checked = (grdRow.Cells[ftyColIsPoolingAllowed].Text == "Yes");
                    txtPoolSHID.Text = grdRow.Cells[ftyColPoolSHID].Text.Replace(BLANK_CELL, "");
                    txtCutoffDate.Text = grdRow.Cells[ftyColCutoffDate].Text.Replace(BLANK_CELL, "");

                    if (ddlCropYear.SelectedIndex > 1) {

                        btnSave.Enabled = false;
                        btnPost.Enabled = false;

                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), OLD_CROP_YEAR_WARNING);

                    } else {

                        btnSave.Enabled = true;
                        if (Convert.ToDecimal(txtPercentage.Text) > 0) {
                            btnPost.Enabled = true;
                        } else {
                            btnPost.Enabled = false;
                        }
                    }

                } else {

                    chkIsOverPlantAllowed.Checked = false;
                    txtPercentage.Text = "";
                    chkIsPoolingAllowed.Checked = false;
                    txtPoolSHID.Text = "";
                    txtCutoffDate.Text = "";

                    btnSave.Enabled = true;
                    btnPost.Enabled = false;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ClearFactoryEdit() {

            chkIsOverPlantAllowed.Checked = false;
            txtPercentage.Text = "";
            chkIsOverPlantAllowed.Checked = false;
            txtPoolSHID.Text = "";
            txtCutoffDate.Text = "";

            btnSave.Enabled = false;
            btnPost.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            try {

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                GridViewRow grdRow = grdFactory.SelectedRow;

                if (grdRow == null) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please select a Over Plant Factory Detail record before pressing Save.");
                    throw warn;
                }

                int factoryNumber = Convert.ToInt32(grdRow.Cells[ftyColFactoryNumber].Text);
                string userName = Common.AppHelper.GetIdentityName();

                bool isOverPlantAllowed = chkIsOverPlantAllowed.Checked;
                decimal overPlantPct = Convert.ToDecimal((txtPercentage.Text.Length > 0? txtPercentage.Text: "0"));
                bool isPoolingAllowed = chkIsPoolingAllowed.Checked;
                int poolMemberSHID = Convert.ToInt32(txtPoolSHID.Text);
                string poolCutoffDate = txtCutoffDate.Text;
                
                // Business Rules, oh boy!                
                if (isOverPlantAllowed) {
                    // If over plant is allowed, you need to have a percentage.  
                    if (overPlantPct == 0) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You need to enter a Percentage when you check 'Over Plant Allowed'.");
                        throw warn;
                    }
                } else {
                    // if over plant not allowed, you cannot have a percentage or cutoff date or allow pooling.
                    if (overPlantPct != 0) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You cannot enter a Percentage unless you check 'Over Plant Allowed'.");
                        throw warn;
                    }
                    if (poolCutoffDate.Length > 0) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You cannot enter a Cutoff Date unless you check 'Over Plant Allowed'.");
                        throw warn;
                    }
                    if (isPoolingAllowed) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You cannot check 'Allow Pooling' unless you check 'Over Plant Allowed'.");
                        throw warn;
                    }
                }

                if (poolCutoffDate.Length > 0) {
                    if (!Common.CodeLib.IsDate(poolCutoffDate)) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid Cutoff Date (mm/dd/yyyy).");
                        throw warn;
                    }
                }

                BeetDataOverPlant.OverPlantFactorySave(cropYear, factoryNumber, isOverPlantAllowed, overPlantPct, isPoolingAllowed, poolMemberSHID, poolCutoffDate, userName);

                ClearFactoryEdit();
                FillFactoryGrid();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnPost_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnPost_Click";

            try {

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                GridViewRow grdRow = grdFactory.SelectedRow;
                int factoryNumber = Convert.ToInt32(grdRow.Cells[ftyColFactoryNumber].Text);
                string userName = Common.AppHelper.GetIdentityName();

                BeetDataOverPlant.OverPlantPost(cropYear, factoryNumber, userName);

                ClearFactoryEdit();
                FillFactoryGrid();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {

                ClearFactoryEdit();
                FillFactoryGrid();

                if (ddlCropYear.SelectedIndex > 1) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), OLD_CROP_YEAR_WARNING);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdFactory_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdFactory_SelectedIndexChanged";

            try {
                FillFactoryEdit();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
