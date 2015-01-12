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

    public partial class CoopEquityDeduction : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.ShareholderServices.CoopSettings.";        

        private enum EqDedGridCols {
            colEquityDeductionID = 0,
            colRowVersion,
            colDeductionNumber,
            colDeductionDescription,
            colIsActive
        }

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                if (!Page.IsPostBack) {
                    FillEqDeductionGrid();
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdEqDeduction);
            base.Render(writer);
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                if (row.Cells[(int)EqDedGridCols.colDeductionNumber].Text != "*") {

                    row.Attributes["onmouseover"] = "HoverOn(this)";
                    row.Attributes["onmouseout"] = "HoverOff(this)";
                    //row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
                    row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
                }
            }
        }

        private void ResetEquityDeductionEdit() {
            txtEDedNumber.Text = "";
            chkEDedIsActive.Checked = false;
            txtEDedDescription.Text = "";
        }

        private void FillEqDeductionGrid() {

            const string METHOD_NAME = "FillEqDeductionGrid";        

            try {

                List<ListEquityDeductionItem> stateList = BeetEquityDeduction.EquityDeductionGetAll();

                grdEqDeduction.SelectedIndex = -1;
                grdEqDeduction.DataSource = stateList;
                grdEqDeduction.DataBind();
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void grdEqDeduction_RowCreated(object sender, GridViewRowEventArgs e) {

            //========================================================================
            // This is just too funny.  I call it either a BUG or a Design-MISTAKE!
            // In order to Hide a grid row that you want to hold data you must
            // turn visibility off here, after databinding has taken place.  It 
            // seems the control was not designed to understand this basic need.
            //========================================================================
            if (e.Row.RowType != DataControlRowType.EmptyDataRow) {

                e.Row.Cells[(int)EqDedGridCols.colEquityDeductionID].CssClass = "DisplayNone";
                e.Row.Cells[(int)EqDedGridCols.colRowVersion].CssClass = "DisplayNone";
            }
        }

        protected void grdEqDeduction_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdEqDeduction_SelectedIndexChanged";
            try {

                ResetEquityDeductionEdit();
                
                //-----------------------------------
                //	Fill Equity Deduction Edit
                //-----------------------------------
                //      Note: here we don't care about the ID or rowVersion stamp
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                GridViewRow row = grdEqDeduction.SelectedRow;

                ListEquityDeductionItem state = new ListEquityDeductionItem("", 
                    row.Cells[(int)EqDedGridCols.colDeductionNumber].Text,
                    row.Cells[(int)EqDedGridCols.colDeductionDescription].Text,
                    row.Cells[(int)EqDedGridCols.colIsActive].Text, 
                    "");

                txtEDedNumber.Text = state.DeductionNumber;
                txtEDedDescription.Text = state.DeductionDescription;
                chkEDedIsActive.Checked = state.IsActiveAsBool();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAdd_Click";

            try {               

                //----------------------------------------------------------------------
                // Use the constructor to clean up state values, especially from grid.
                //----------------------------------------------------------------------
                ListEquityDeductionItem state = new ListEquityDeductionItem(
                    "",
                    txtEDedNumber.Text,
                    txtEDedDescription.Text,
                    (chkEDedIsActive.Checked ? "Y" : "N"),
                    "");


                string tmpNum = state.DeductionNumber;
                int deductionNumber = 0;
                try {
                    deductionNumber = Convert.ToInt32(tmpNum);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a positive number for Deduction Number.");
                    throw (warn);
                }
                if (deductionNumber == 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a Deduction Number to a number greater than zero.");
                    throw (warn);
                }

                if (state.DeductionDescription.Length == 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a Deduction Description.");
                    throw (warn);
                }

                string userName = Common.AppHelper.GetIdentityName();
                
                BeetEquityDeduction.EquityDeductionSave(Convert.ToInt32(state.EquityDeductionID), Convert.ToInt32(state.DeductionNumber), 
                    state.DeductionDescription, state.IsActiveAsBool(), state.RowVersion, userName);

                ResetEquityDeductionEdit();
                FillEqDeductionGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnUpdate_Click";

            try {

                //------------------------------------
                // Pull info from grid selection.
                //------------------------------------                
                GridViewRow row = grdEqDeduction.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before pressing Update, please select an Equity Deduction from the Equity Deductions grid.");
                    throw (warn);
                }

                //----------------------------------------------------------------------
                // Use the constructor to clean up state values, especially from grid.
                //----------------------------------------------------------------------
                ListEquityDeductionItem state = new ListEquityDeductionItem(
                    row.Cells[(int)EqDedGridCols.colEquityDeductionID].Text,
                    txtEDedNumber.Text,
                    txtEDedDescription.Text,
                    (chkEDedIsActive.Checked? "Y": "N"),
                    row.Cells[(int)EqDedGridCols.colRowVersion].Text);

                int equityDeductionID = Convert.ToInt32(state.EquityDeductionID);
                if (equityDeductionID <= 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Unable to identify your Equity Deductions grid selection.  Try making your selection again.");
                    throw (warn);
                }

                string rowVersion = state.RowVersion;
                if (rowVersion.Length == 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Unable to identify your Equity Deductions grid selection.  Try making your selection again.");
                    throw (warn);
                }

                bool isActive = state.IsActiveAsBool();
                string deductionDescription = state.DeductionDescription;

                if (deductionDescription.Length == 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a Deduction Description.");
                    throw (warn);
                }

                string tmpNum = state.DeductionNumber;
                int deductionNumber = 0;
                try {
                    deductionNumber = Convert.ToInt32(tmpNum);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a positive number for Deduction Number.");
                    throw (warn);
                }
                if (deductionNumber == 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a Deduction Number to a number greater than zero.");
                    throw (warn);
                }
                if (deductionNumber != Convert.ToInt32(row.Cells[(int)EqDedGridCols.colDeductionNumber].Text)) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("You cannot update the Deduction Number of a Deduction.  If the deduction has never been used, you can delete the deduction.");
                    throw (warn);
                }

                string userName = Common.AppHelper.GetIdentityName();

                BeetEquityDeduction.EquityDeductionSave(equityDeductionID, deductionNumber, deductionDescription, isActive, rowVersion, userName);

                ResetEquityDeductionEdit();
                FillEqDeductionGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDelete_Click";

            try {

                //------------------------------------
                // Pull info from grid selection.
                //------------------------------------                
                GridViewRow row = grdEqDeduction.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before pressing Delete, please select an Equity Deduction from the Equity Deductions grid.");
                    throw (warn);
                }

                //----------------------------------------------------------------------
                // Use the constructor to clean up state values, especially from grid.
                //----------------------------------------------------------------------
                ListEquityDeductionItem state = new ListEquityDeductionItem(
                    row.Cells[(int)EqDedGridCols.colEquityDeductionID].Text,
                    txtEDedNumber.Text,
                    txtEDedDescription.Text,
                    (chkEDedIsActive.Checked ? "Y" : "N"),
                    row.Cells[(int)EqDedGridCols.colRowVersion].Text);

                int equityDeductionID = Convert.ToInt32(state.EquityDeductionID);
                if (equityDeductionID <= 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Unable to identify your Equity Deductions grid selection.  Try making your selection again.");
                    throw (warn);
                }

                string rowVersion = state.RowVersion;
                if (rowVersion.Length == 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Unable to identify your Equity Deductions grid selection.  Try making your selection again.");
                    throw (warn);
                }

                BeetEquityDeduction.EquityDeductionDelete(equityDeductionID, rowVersion);

                ResetEquityDeductionEdit();
                FillEqDeductionGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}