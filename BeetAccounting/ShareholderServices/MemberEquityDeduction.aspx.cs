using System;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.BeetAccounting.ShareholderServices {

    public partial class MemberEquityDeduction : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.ShareholderServices.MemberEquityDeduction.";
        private const string BLANK_CELL = "&nbsp;";
        private DateTime _paymentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

        private enum EqDedGridCols {
            colMemberEquityDeductionID = 0,
            colEquityDeductionID,
            colRowVersion,
            colSequence,
            colEquityCropYear,
            colEquityType,
            colPayDesc,
            colDeductionDesc,
            colDeductionAmount
        }

        private enum PaymentGridCols {
            colSequence = 0,
            colEquityCropYear,
            colEquityType,
            colPaymentDesc,
            colPayAmount
        }

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                Common.AppHelper.HideWarning(addrWarning);

                txtSHID.Attributes.Add("onkeypress", "CheckEnterKey(event);");
                btnDeleteDeduction.Attributes.Add("onclick", "confirm('Are you sure you want to delete this Member Equity Deduction?');");

//BBS TEST ONLY !!!!
//_paymentDate = _paymentDate.AddYears(-1);
                if (!Page.IsPostBack) {

                    BeetDataDomain.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    //WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    MySHID = "";
                    FillGridEqDeduction("0", 0);
                    FillGridEqPayment("0", _paymentDate);
                    FillEquityDeductionList();
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdEqDeduction);
            AddRowSelectToGridView(grdPayment);
            base.Render(writer);
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                if ((gv.ID == grdEqDeduction.ID && row.Cells[(int)EqDedGridCols.colEquityCropYear].Text != "*") ||
                    (gv.ID == grdPayment.ID && row.Cells[(int)PaymentGridCols.colEquityCropYear].Text != "*")) {

                    row.Attributes["onmouseover"] = "HoverOn(this)";
                    row.Attributes["onmouseout"] = "HoverOff(this)";
                    //row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
                    row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
                }
            }
        }

        private void FindAddress(string shid) {

            const string METHOD_NAME = "FindAddress";

            try {

                ResetShareholder();

                if (shid.Length > 0) {

                    int memberID = 0, addressID = 0;
                    string phone = "", email = "", fax = "", busName = "";

                    WSCMember.GetInfo(shid, ref memberID, ref addressID, ref busName, ref phone, ref email, ref fax);

                    MySHID = shid;
                    MyMemberID = memberID.ToString();
                    lblBusName.Text = busName;
                }

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ClearEdit() {

            ddlEquityDeductions.SelectedIndex = -1;
            txtDeductionAmount.Text = "";
            grdEqDeduction.SelectedIndex = -1;
            grdPayment.SelectedIndex = -1;
        }

        private void ResetShareholder() {

            const string METHOD_NAME = "ResetShareholder";

            try {

                lblBusName.Text = "";
                MySHID = "";

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private string MySHID {
            get { return ViewState["mySHID"].ToString(); }
            set { ViewState["mySHID"] = value; }
        }

        private string MyMemberID {
            get { return ViewState["myMemberID"].ToString(); }
            set { ViewState["myMemberID"] = value; }
        }

        protected void btnAddDeduction_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAddDeduction_Click";

            try {

                GridViewRow row = grdPayment.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before pressing Add, please select a Payment from the Available Equity Payments grid.");
                    throw (warn);
                }

                //--------------------------------------------------
                // Marry a Member to a Deduction and a payment.
                //--------------------------------------------------
                int equityCropYear = Convert.ToInt32(row.Cells[(int)PaymentGridCols.colEquityCropYear].Text);                
                string equityType = row.Cells[(int)PaymentGridCols.colEquityType].Text;
                int sequence = Convert.ToInt32(row.Cells[(int)PaymentGridCols.colSequence].Text);                
                string paymentDesc = row.Cells[(int)PaymentGridCols.colPaymentDesc].Text;                
                int equityDeductionID = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlEquityDeductions));
                int memberID = Convert.ToInt32(MyMemberID);
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                string amountText = txtDeductionAmount.Text.TrimEnd().Replace(BLANK_CELL, "");
                decimal deductionAmount = 0;
                try {
                    deductionAmount = Convert.ToDecimal(amountText);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a positive number for Deduction Amount.");
                    throw (warn);
                }

                string userName = Common.AppHelper.GetIdentityName();
                DateTime deductionDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                BeetEquityDeduction.EquityDeductionMemberSave(0, memberID, equityDeductionID,
                    equityCropYear, cropYear, equityType, sequence, paymentDesc, deductionAmount, deductionDate, "", userName);

                ClearEdit();
                FillGridEqDeduction(MySHID, cropYear);
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnUpdateDeduction_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnUpdateDeduction_Click";

            try {

                GridViewRow row = grdPayment.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before pressing Update, please select a Payment from the Available Equity Payments grid.");
                    throw (warn);
                }

                //--------------------------------------------------
                // Marry a Member to a Deduction and a payment.
                //--------------------------------------------------
                int equityCropYear = Convert.ToInt32(row.Cells[(int)PaymentGridCols.colEquityCropYear].Text);
                string equityType = row.Cells[(int)PaymentGridCols.colEquityType].Text;
                int sequence = Convert.ToInt32(row.Cells[(int)PaymentGridCols.colSequence].Text);
                string paymentDesc = row.Cells[(int)PaymentGridCols.colPaymentDesc].Text;
                int equityDeductionID = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlEquityDeductions));
                int memberID = Convert.ToInt32(MyMemberID);
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                // Switch Grids
                row = grdEqDeduction.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before pressing Update, please select an row from the Member Equity Deduction grid.");
                    throw (warn);
                }
                int equityDeductionMemberID = Convert.ToInt32(row.Cells[(int)EqDedGridCols.colMemberEquityDeductionID].Text);
                string rowVersion = row.Cells[(int)EqDedGridCols.colRowVersion].Text;

                string amountText = txtDeductionAmount.Text.TrimEnd().Replace(BLANK_CELL, "");
                decimal deductionAmount = 0;
                try {
                    deductionAmount = Convert.ToDecimal(amountText);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a positive number for Deduction Amount.");
                    throw (warn);
                }

                string userName = Common.AppHelper.GetIdentityName();
                DateTime deductionDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                BeetEquityDeduction.EquityDeductionMemberSave(equityDeductionMemberID, memberID, equityDeductionID,
                    equityCropYear, cropYear, equityType, sequence, paymentDesc, deductionAmount, deductionDate, rowVersion, userName);

                ClearEdit();
                FillGridEqDeduction(MySHID, cropYear);
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDeleteDeduction_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDeleteDeduction_Click";

            try {

                // Switch Grids
                GridViewRow row = grdEqDeduction.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before pressing Delete, please select an row from the Member Equity Deduction grid.");
                    throw (warn);
                }
                int equityDeductionMemberID = Convert.ToInt32(row.Cells[(int)EqDedGridCols.colMemberEquityDeductionID].Text);
                string rowVersion = row.Cells[(int)EqDedGridCols.colRowVersion].Text;

                BeetEquityDeduction.EquityDeductionMemberDelete(equityDeductionMemberID, rowVersion);
                
                ClearEdit();
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                FillGridEqDeduction(MySHID, cropYear);
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdEqDeduction_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdEqDeduction_SelectedIndexChanged";

            GridViewRow row = grdEqDeduction.SelectedRow;

            try {

                ListMemberEquityDeductionItem state = new ListMemberEquityDeductionItem(row.Cells[(int)EqDedGridCols.colMemberEquityDeductionID].Text,
                    row.Cells[(int)EqDedGridCols.colEquityDeductionID].Text, row.Cells[(int)EqDedGridCols.colRowVersion].Text,
                    row.Cells[(int)EqDedGridCols.colEquityCropYear].Text, row.Cells[(int)EqDedGridCols.colEquityType].Text,
                    row.Cells[(int)EqDedGridCols.colSequence].Text,
                    row.Cells[(int)EqDedGridCols.colPayDesc].Text, row.Cells[(int)EqDedGridCols.colDeductionDesc].Text,
                    row.Cells[(int)EqDedGridCols.colDeductionAmount].Text);

                // Populate edit controls
                txtDeductionAmount.Text = state.DeductionAmount;
                Common.UILib.SelectDropDownValue(ddlEquityDeductions, state.EquityDeductionID);
                string equityCropYear = state.EquityCropYear;

                foreach (GridViewRow payRow in grdPayment.Rows) {

                    if (payRow.Cells[(int)PaymentGridCols.colEquityCropYear].Text == equityCropYear
                        && payRow.Cells[(int)PaymentGridCols.colEquityType].Text == state.EquityType
                        && payRow.Cells[(int)PaymentGridCols.colSequence].Text == state.PaySequence) {

                        //---------------------------------------------------------------------------------------
                        // given a match on equity crop year + equity type + payment desc, highlight the row
                        //---------------------------------------------------------------------------------------
                        grdPayment.SelectedIndex = payRow.DataItemIndex;
                        break;
                    }
                }

                //----------------------------------------------------------
                // With a selected row, you can only add, update, & delete
                //----------------------------------------------------------
                btnAddDeduction.Enabled = true;
                btnUpdateDeduction.Enabled = true;
                btnDeleteDeduction.Enabled = true;
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
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

                e.Row.Cells[(int)EqDedGridCols.colMemberEquityDeductionID].CssClass = "DisplayNone";
                e.Row.Cells[(int)EqDedGridCols.colEquityDeductionID].CssClass = "DisplayNone";
                e.Row.Cells[(int)EqDedGridCols.colSequence].CssClass = "DisplayNone";
                e.Row.Cells[(int)EqDedGridCols.colRowVersion].CssClass = "DisplayNone";
            }
        }

        private void FillEquityDeductionList() {

            const string METHOD_NAME = "FillEquityDeductionList";
            try {

                ddlEquityDeductions.Items.Clear();
                List<ListEquityDeductionItem> stateList = BeetEquityDeduction.EquityDeductionGetAll();

                ddlEquityDeductions.Items.Add(new ListItem(" ", "0"));
                foreach (ListEquityDeductionItem state in stateList) {

                    ddlEquityDeductions.Items.Add(new ListItem(state.DeductionNumber + " - " + state.DeductionDescription, state.EquityDeductionID.ToString()));
                }
                if (ddlEquityDeductions.Items.Count > 0) {
                    ddlEquityDeductions.Items[0].Selected = true;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillGridEqPayment(string shid, DateTime matchDate) {

            const string METHOD_NAME = "FillGridEqPayment";

            try {

                grdPayment.SelectedIndex = -1;

                List<ListMemberEquityPaymentItem> stateList = BeetEquityDeduction.EquityPaymentsByShid(shid, matchDate);
                grdPayment.DataSource = stateList;
                grdPayment.DataBind();
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillGridEqDeduction(string shid, int cropYear) {

            const string METHOD_NAME = "FillGridEqDeduction";

            try {

                grdEqDeduction.SelectedIndex = -1;

                List<ListMemberEquityDeductionItem> stateList = BeetEquityDeduction.EquityDeductionMemberGetBySHID(shid, cropYear);
                grdEqDeduction.DataSource = stateList;
                grdEqDeduction.DataBind();

                //---------------------------------------------
                // Without a selected row, you can only add
                //---------------------------------------------
                btnAddDeduction.Enabled = true;
                btnUpdateDeduction.Enabled = false;
                btnDeleteDeduction.Enabled = false;
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {

                string shid = txtSHID.Text;

                FindAddress(shid);

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                FillGridEqDeduction(shid, cropYear);
                FillGridEqPayment(shid, _paymentDate);

                //------------------------------------------------
                // Cannot Add, Update, Delete in prior years.
                //------------------------------------------------
                if (ddlCropYear.SelectedIndex > 1) {
                    btnAddDeduction.Enabled = false;
                    btnUpdateDeduction.Enabled = false;
                    btnDeleteDeduction.Enabled = false;
                } else {
                    btnAddDeduction.Enabled = true;
                    btnUpdateDeduction.Enabled = true;
                    btnDeleteDeduction.Enabled = true;
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnAddrOk_Click(object sender, EventArgs e) {

            Common.AppHelper.HideWarning(addrWarning);

            // If we have a selected address, use the shid for our main page.
            if (lstAddressName.SelectedItem != null) {

                string shid = txtAddrSHID.Text;
                txtSHID.Text = shid;
                FindAddress(shid);
                uplShid.Update();
            }

            CloseAndResolve("AddressFinder");
        }

        private void CloseAndResolve(string dialogId) {

            string script = string.Format(@"closeAndResolve('{0}')", dialogId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, script, true);
        }

        protected void btnAddrFind_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAddrFind_Click";
            try {

                Common.AppHelper.HideWarning(addrWarning);

                lstAddressName.Items.Clear();

                string searchTerm = txtSearchString.Text.TrimEnd();
                int searchType = 0;

                if (radTypeSHID.Checked) {
                    searchType = 1;
                } else {
                    if (radTypeBusname.Checked) {
                        searchType = 2;
                    } else {
                        searchType = 3;
                    }
                }

                if (searchTerm.Length > 0) {

                    if (!searchTerm.Contains("*")) {
                        searchTerm = searchTerm + "%";
                    } else {
                        searchTerm = searchTerm.Replace("*", "%");
                    }
                }

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                List<ListAddressItem> addrList = BeetDataAddress.AddressFindByTerm(searchTerm, cropYear, searchType);
                lstAddressName.DataSource = addrList;
                lstAddressName.DataBind();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
        }

        protected void lstAddressName_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "lstAddressName_SelectedIndexChanged";
            try {

                Common.AppHelper.HideWarning(addrWarning);

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                ListItem selItem = lstAddressName.SelectedItem;
                List<ListAddressItem> addrList = BeetDataAddress.AddressGetInfo(Convert.ToInt32(selItem.Value), 0, cropYear);

                if (addrList.Count > 0) {

                    ListAddressItem item = addrList[0];
                    txtAddrSHID.Text = item.SHID;
                    chkAddrSubscriber.Checked = item.IsSubscriber;
                    txtAddrFName.Text = item.FirstName;
                    txtAddrLName.Text = item.LastName;
                    txtAddrBName.Text = item.BusName;
                    txtAddrAddress.Text = item.AdrLine1;
                    txtAddrAddressLine2.Text = item.AdrLine2;
                    txtAddrCity.Text = item.CityName;
                    txtAddrState.Text = item.StateName;
                    txtAddrZip.Text = item.PostalCode;
                    txtAddrPhoneNo.Text = item.PhoneNo;
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
        }

        protected void btnResolveShid_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnResolveShid_Click";
            try {

                string shid = txtSHID.Text;
                FindAddress(shid);

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                FillGridEqDeduction(shid, cropYear);
                FillGridEqPayment(shid, _paymentDate);
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdPayment_SelectedIndexChanged(object sender, EventArgs e) {
            // NOP
        }

        protected void grdPayment_RowCreated(object sender, GridViewRowEventArgs e) {
            //========================================================================
            // This is just too funny.  I call it either a BUG or a Design-MISTAKE!
            // In order to Hide a grid row that you want to hold data you must
            // turn visibility off here, after databinding has taken place.  It 
            // seems the control was not designed to understand this basic need.
            //========================================================================
            if (e.Row.RowType != DataControlRowType.EmptyDataRow) {

                e.Row.Cells[(int)PaymentGridCols.colSequence].CssClass = "DisplayNone";
            }
        }
    }
}
