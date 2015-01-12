using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class BankPayeeMaintenance : Common.BasePage {

        private const string MOD_NAME = "Admin.BankPayeeMaintenance.";
        private int _cropYear = 0;
        private int _otherYear = 0;

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                txtSHID.Attributes.Add("onfocus", "DoGotFocus(this);");
                txtSHID.Attributes.Add("onfocusout", "DoGotFocus(null);");
                txtSHID.Attributes.Add("onkeypress", "DoKeyPress(this, event);CheckEnterKey(event);");
                txtBusName.Attributes.Add("onfocus", "DoGotFocus(this);");
                txtBusName.Attributes.Add("onfocusout", "DoGotFocus(null);");
                txtBusName.Attributes.Add("onkeypress", "DoKeyPress(this, event);AbortEnterKey(event);");
                ddlCropYear.Attributes.Add("onchange", "ChangeCropYear();");

                btnAdd.Attributes.Add("onfocus", "DoGotFocus(this);");
                btnAdd.Attributes.Add("onfocusout", "DoGotFocus(null);");
                btnAdd.Attributes.Add("onclick", "DoKeyPress(this, event) && CheckPayeeAction('', this, event);");

                btnAddEquityBank.Attributes.Add("onfocus", "DoGotFocus(this);");
                btnAddEquityBank.Attributes.Add("onfocusout", "DoGotFocus(null);");
                btnAddEquityBank.Attributes.Add("onclick", "DoKeyPress(this, event) && CheckEquityAction('', this, event);");

                btnDelete.Attributes.Add("onfocus", "DoGotFocus(this);");
                btnDelete.Attributes.Add("onfocusout", "DoGotFocus(null);");
                btnDelete.Attributes.Add("onclick", "DoKeyPress(this, event) && CheckPayeeAction('', this, event);");

                btnDeleteEquityBank.Attributes.Add("onfocus", "DoGotFocus(this);");
                btnDeleteEquityBank.Attributes.Add("onfocusout", "DoGotFocus(null);");
                btnDeleteEquityBank.Attributes.Add("onclick", "DoKeyPress(this, event) && CheckEquityAction('', this, event);");

                // Show the selected frame
                if (txtActiveFrame.Text == "" || txtActiveFrame.Text == "Payee") {

                    txtActiveFrame.Text = "Payee";
                    switchCropBank.InnerHtml = "Hide";
                    CropBanks.Attributes.Add("class", "DisplayOn");
                    switchEqBank.InnerHtml = "Show";
                    EquityBanks.Attributes.Add("class", "DisplayOff");

                } else {

                    switchCropBank.InnerHtml = "Show";
                    CropBanks.Attributes.Add("class", "DisplayOff");
                    switchEqBank.InnerHtml = "Hide";
                    EquityBanks.Attributes.Add("class", "DisplayOn");
                }

                if (!Page.IsPostBack) {

                    try {
                        txtOtherYears.Text = "0";
                        FillDomainData();
                    }
                    catch (System.Exception ex) {
						Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
						((PrimaryTemplate)Page.Master).ShowWarning(ex);
                    }
                    finally {
                        try {
                            ShowBankPayeeList(0, _cropYear);
                            ShowBankEquityList(0, _cropYear);
                        }
                        catch {
                            //NOP
                        }
                    }

                } else {

                    _cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                    _otherYear = Convert.ToInt32(txtOtherYears.Text);
                    txtOtherYears.Text = "0";

                    // =====================================
                    // Take ACTION: perform prep work here.
                    // =====================================
                    string action = txtAction.Text;
                    txtAction.Text = "";

                    if (action.Length > 0) {

                        try {

                            SetAddMode();
                            SetAddEquityMode();

                            switch (action) {

                                case "ChangeCropYear":
                                    ChangeCropYear();
                                    break;

                                case "FindSHID":

                                    ClearBankDetail();
                                    ClearEquityBankDetail();

                                    // Check for shid
                                    if (Common.CodeLib.IsValidSHID(txtSHID.Text)) {

                                        int shid = Convert.ToInt32(txtSHID.Text);
                                        txtSHID.Text = "";
                                        ShowSHIDDetail(shid, 0);

                                    } else {
                                        //Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You did not enter a reasonable SHID.");
                                        //return;
                                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                                        throw (warn);
                                    }
                                    break;

                                case "FindBank":

                                    // Check for bank id
                                    if (txtBankID.Text.Length > 0) {

                                        int bankID = Convert.ToInt32(txtBankID.Text);
                                        txtBankID.Text = "";

                                        ShowBankDetail(bankID);

                                    }
                                    break;

                                case "FindEquityBank":

                                    // Check for bank id
                                    if (txtEquityBankID.Text.Length > 0) {

                                        int bankID = Convert.ToInt32(txtEquityBankID.Text);
                                        txtEquityBankID.Text = "";

                                        ShowEquityBankDetail(bankID);

                                    }
                                    break;
                            }
                        }
                        catch (System.Exception ex) {
							Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
							((PrimaryTemplate)Page.Master).ShowWarning(ex);
                        }
                        finally {
                            try {

                                int addressID = 0;
                                if (txtAddressID.Text.Length > 0) {
                                    addressID = Convert.ToInt32(txtAddressID.Text);
                                }

                                int memberID = 0;
                                if (txtMemberID.Text.Length > 0) {
                                    memberID = Convert.ToInt32(txtMemberID.Text);
                                }

                                ShowBankPayeeList(addressID, _cropYear);
                                ShowBankEquityList(memberID, _cropYear);
                            }
                            catch {
                                //NOP
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void ShowSHIDDetail(int shid, int addressID) {

            try {

                ClearSHID();

                List<ListAddressItem> addressList = BeetDataAddress.AddressGetInfo(shid, addressID, _cropYear);
                if (addressList.Count > 0) {

                    ListAddressItem item = addressList[0];
                    txtSHID.Text = item.SHID;
                    txtAddressID.Text = item.AddressID.ToString();
                    txtMemberID.Text = item.MemberID.ToString();
                    txtFirstName.Text = item.FirstName;
                    txtLastName.Text = item.LastName;
                    txtBusName.Text = item.BusName;
                    txtAddrLine1.Text = item.AdrLine1;
                    txtAddrLine2.Text = item.AdrLine2;
                    txtCity.Text = item.CityName;
                    txtState.Text = item.StateName;
                    txtZip.Text = item.PostalCode;
                    txtTaxID.Text = item.TaxID;
                    txtPhone.Text = item.PhoneNo;
                    txtEmail.Text = item.Email;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("BankPayeeMaintenance.ShowSHIDDetail", ex);
                throw (wex);
            }
        }

        private void ClearSHID() {

            txtAddressID.Text = "";
            txtMemberID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtBusName.Text = "";
            txtAddrLine1.Text = "";
            txtAddrLine2.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtZip.Text = "";
            txtTaxID.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
        }

        private void ClearBankDetail() {

            txtBankPayeeID.Text = "";
            txtBankID.Text = "";
            txtBankName.Text = "";
            chkSubordination.Checked = false;
        }

        private void ClearEquityBankDetail() {

            txtBankEquityLienID.Text = "";
            txtEquityBankID.Text = "";
            txtEquityBankName.Text = "";
            txtEquityDate.Text = "";

            chkEqPatronStock.Checked = false;
            chkEqPatronage.Checked = false;
            chkEqRetains.Checked = false;
            chkReleasePatronage.Checked = false;
            chkReleasePatronStock.Checked = false;
            chkReleaseRetains.Checked = false;
        }

        private void ClearBankPayeeList() {
            grdResults.DataSource = null;
            grdResults.DataBind();
        }

        private void ClearBankEquityList() {
            grdEquityResults.DataSource = null;
            grdEquityResults.DataBind();
        }

        private void ShowBankDetail(int bankID) {

            ClearBankDetail();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCBank.BankGetByID(conn, bankID)) {

                        if (dr.Read()) {
                            txtBankName.Text = dr.GetString(dr.GetOrdinal("bnk_name"));
                            txtBankID.Text = dr.GetInt32(dr.GetOrdinal("bnk_bank_id")).ToString();
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("BankMaintenance.ShowBankDetail", ex);
                throw (wex);
            }
        }

        private void ShowEquityBankDetail(int bankID) {

            ClearEquityBankDetail();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCBank.BankGetByID(conn, bankID)) {

                        if (dr.Read()) {
                            txtEquityBankName.Text = dr.GetString(dr.GetOrdinal("bnk_name"));
                            txtEquityBankID.Text = dr.GetInt32(dr.GetOrdinal("bnk_bank_id")).ToString();
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("BankMaintenance.ShowEquityBankDetail", ex);
                throw (wex);
            }
        }

        private void ShowBankPayeeList(int addressID, int cropYear) {

            ClearBankPayeeList();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCBank.BankPayeeGetList(conn, addressID, cropYear)) {

                        grdResults.DataSource = dr;
                        grdResults.DataBind();
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("BankMaintenance.ShowBankPayeeList", ex);
                throw (wex);
            }
        }

        private void ShowBankEquityList(int memberID, int cropYear) {

            ClearBankEquityList();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCBank.BankEquityGetList(conn, memberID, cropYear)) {

                        grdEquityResults.DataSource = dr;
                        grdEquityResults.DataBind();
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("BankMaintenance.ShowBankEquityList", ex);
                throw (wex);
            }
        }

        private void FillDomainData() {

            WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
        }

        private void SetAddMode() {
            btnAdd.Text = "Add";
        }

        private void SetUpdateMode() {
            btnAdd.Text = "Update";
        }

        private void SetAddEquityMode() {
            btnAddEquityBank.Text = "Add";
        }

        private void SetUpdateEquityMode() {
            btnAddEquityBank.Text = "Update";
        }

        private void ChangeCropYear() {

            ClearBankDetail();
            ClearBankPayeeList();
            ClearSHID();
            txtSHID.Text = "";

            SetAddMode();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridResults(grdResults);
            AddRowSelectToGridEquity(grdEquityResults);
            base.Render(writer);
        }

        private void AddRowSelectToGridResults(GridView gv) {            

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";

                // "SelectItemRow( ctl, bankPayeeID, bankName, bankID, subRecd )
                row.Attributes.Add("onclick",
                    "SelectItemRow(this, " + row.Cells[0].Text + ", \"" +
                    row.Cells[4].Text + "\", " +
                    row.Cells[1].Text + ", '" +
                    row.Cells[5].Text + "'" + ");");

                //row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
            }
        }

        private void AddRowSelectToGridEquity(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";

                // SelectEqRow(ctl, bankEquityLienID, bankName, equityBankID, 
	            //  lienShares, lienPatronage, lienRetains, releaseShares, releasePatronage, releaseRetains,
	            //  equityDate)
                row.Attributes.Add("onclick",
                    "SelectEqRow(this, " + row.Cells[0].Text + ", \"" +
                    row.Cells[4].Text + "\", " +
                    row.Cells[1].Text + "," +
                    "'" + row.Cells[5].Text + "'," +
                    "'" + row.Cells[6].Text + "'," +
                    "'" + row.Cells[7].Text + "'," +
                    "'" + row.Cells[8].Text + "'," +
                    "'" + row.Cells[9].Text + "'," +
                    "'" + row.Cells[10].Text + "'," +
                    "'" + row.Cells[11].Text + "'" + ");");

                //row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAdd_Click";

            WSCSecurity auth = Globals.SecurityState;
            if (auth.SecurityGroupName.ToUpper().IndexOf("AG ADMIN") == -1) {
                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                return;
            }

            int addressID = 0;
            try {

                int bankPayeeID = 0;
                if (txtBankPayeeID.Text.Length > 0) {
                    bankPayeeID = Convert.ToInt32(txtBankPayeeID.Text);
                }

                if (txtAddressID.Text.Length > 0) {
                    addressID = Convert.ToInt32(txtAddressID.Text);
                }

                int bankID = 0;
                if (txtBankID.Text.Length > 0) {
                    bankID = Convert.ToInt32(txtBankID.Text);
                }

                bool subRecd = chkSubordination.Checked;

                int sequenceNo = 0;
                bool isInsert = false;

                if (bankPayeeID == 0) {

                    WSCBank.BankPayeeSave(ref bankPayeeID, bankID, addressID,
                        sequenceNo, subRecd, _cropYear, Globals.SecurityState.UserName);
                    isInsert = true;
                } else {
                    WSCBank.BankPayeeSetSub(bankPayeeID, subRecd,
                        Globals.SecurityState.UserName);
                }

                // add/update other year?
                if (_otherYear > 0) {

                    WSCBank.BankPayeeSaveOtherYear(bankID, addressID, subRecd,
                        Convert.ToInt32(_otherYear), isInsert, Globals.SecurityState.UserName);
                }

                ClearBankDetail();
                SetAddMode();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
            finally {
                try {
                    ShowBankPayeeList(addressID, _cropYear);
                }
                catch {
                    //NOP
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDelete_Click";

            WSCSecurity auth = Globals.SecurityState;
            if (auth.SecurityGroupName.ToUpper().IndexOf("AG ADMIN") == -1) {
                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                return;
            }

            int bankID = 0;
            if (txtBankID.Text.Length > 0) {
                bankID = Convert.ToInt32(txtBankID.Text);
            }

            int addressID = 0;
            if (txtAddressID.Text.Length > 0) {
                addressID = Convert.ToInt32(txtAddressID.Text);
            }

            try {

                int bankPayeeID = 0;
                if (txtBankPayeeID.Text.Length > 0) {
                    bankPayeeID = Convert.ToInt32(txtBankPayeeID.Text);
                    WSCBank.BankPayeeDelete(bankPayeeID);
                }

                // delete other year?
                if (_otherYear > 0) {

                    WSCBank.BankPayeeDeleteOtherYear(bankID, addressID, _otherYear);
                }

                ClearBankDetail();
                SetAddMode();
            }

            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
            finally {
                try {
                    if (txtAddressID.Text.Length > 0) {
                        addressID = Convert.ToInt32(txtAddressID.Text);
                    }
                    ShowBankPayeeList(addressID, _cropYear);
                }
                catch {
                    //NOP
                }
            }
        }

        protected void btnAddEquityBank_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAdd_Click";

            WSCSecurity auth = Globals.SecurityState;
            if (auth.SecurityGroupName.ToUpper().IndexOf("AG ADMIN") == -1) {
                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                return;
            }

            int addressID = 0;
            int memberID = 0;

            try {

                // Date required
                string eqDate = txtEquityDate.Text;
                if (eqDate == null || eqDate.Length == 0) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You must enter a date.");
                    throw (warn);
                }
                if (!Common.CodeLib.IsDate(eqDate)) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You must enter a valid date (mm/dd/yyyy).");
                    throw (warn);
                }

                int bankEquityLienID = 0;
                if (txtBankEquityLienID.Text.Length > 0) {
                    bankEquityLienID = Convert.ToInt32(txtBankEquityLienID.Text);
                }

                if (txtAddressID.Text.Length > 0) {
                    addressID = Convert.ToInt32(txtAddressID.Text);
                }

                if (txtMemberID.Text.Length > 0) {
                    memberID = Convert.ToInt32(txtMemberID.Text);
                }

                int equityBankID = 0;
                if (txtEquityBankID.Text.Length > 0) {
                    equityBankID = Convert.ToInt32(txtEquityBankID.Text);
                }

                bool lienPatronShares = chkEqPatronStock.Checked;
                bool lienPatronage = chkEqPatronage.Checked;
                bool lienRetains = chkEqRetains.Checked;
                bool releasePatronShares = chkReleasePatronStock.Checked;
                bool releasePatronage = chkReleasePatronage.Checked;
                bool releaseRetains = chkReleaseRetains.Checked;

                int sequenceNo = 0;
                bool isInsert = false;

                if (bankEquityLienID == 0) {
                    isInsert = true;
                }

                string userName = Globals.SecurityState.UserName;
                WSCBank.BankEquitySave(ref bankEquityLienID, equityBankID, memberID,
                    sequenceNo, eqDate, lienPatronShares, lienPatronage, lienRetains,
                    releasePatronShares, releasePatronage, releaseRetains,
                    _cropYear, userName);

                // add/update other year?
                if (_otherYear > 0) {
                    WSCBank.BankEquitySaveOtherYear(equityBankID, memberID,
                        eqDate, lienPatronShares, lienPatronage, lienRetains,
                        releasePatronShares, releasePatronage, releaseRetains,
                        Convert.ToInt32(_otherYear), isInsert, userName);
                }

                ClearEquityBankDetail();
                SetAddEquityMode();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
            finally {
                try {
                    ShowBankEquityList(memberID, _cropYear);
                }
                catch {
                    //NOP
                }
            }
        }

        protected void btnDeleteEquityBank_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDeleteEquityBank_Click";

            WSCSecurity auth = Globals.SecurityState;
            if (auth.SecurityGroupName.ToUpper().IndexOf("AG ADMIN") == -1) {
                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                return;
            }

            int equityBankID = 0;
            if (txtEquityBankID.Text.Length > 0) {
                equityBankID = Convert.ToInt32(txtEquityBankID.Text);
            }

            int addressID = 0;
            if (txtAddressID.Text.Length > 0) {
                addressID = Convert.ToInt32(txtAddressID.Text);
            }

            int memberID = 0;
            if (txtMemberID.Text.Length > 0) {
                memberID = Convert.ToInt32(txtMemberID.Text);
            }

            try {

                int bankEquityLienID = 0;
                if (txtBankEquityLienID.Text.Length > 0) {
                    bankEquityLienID = Convert.ToInt32(txtBankEquityLienID.Text);
                    WSCBank.BankEquityDelete(bankEquityLienID);
                }

                // delete other year?
                if (_otherYear > 0) {
                    WSCBank.BankEquityDeleteOtherYear(equityBankID, memberID, _otherYear);
                }

                ClearEquityBankDetail();
                SetAddEquityMode();
            }

            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
            finally {
                try {
                    if (txtAddressID.Text.Length > 0) {
                        addressID = Convert.ToInt32(txtAddressID.Text);
                    }
                    ShowBankEquityList(memberID, _cropYear);
                }
                catch {
                    //NOP
                }
            }
        }

        // ===============================================
        // ** Address Finder Helper Routines.
        // ===============================================
        protected void btnResolveShid_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnResolveShid_Click";
            try {

                string shid = txtSHID.Text;
                if (Common.CodeLib.IsValidSHID(shid)) {

                    int iShid = Convert.ToInt32(shid);

                    ClearBankDetail();
                    ClearEquityBankDetail();
                    ShowSHIDDetail(iShid, 0);

					// Show any related bank details
					int addressID = 0;
					if (txtAddressID.Text.Length > 0) {
						addressID = Convert.ToInt32(txtAddressID.Text);
					}

					int memberID = 0;
					if (txtMemberID.Text.Length > 0) {
						memberID = Convert.ToInt32(txtMemberID.Text);
					}

					ShowBankPayeeList(addressID, _cropYear);
					ShowBankEquityList(memberID, _cropYear);

                } else {
                    Common.CWarning warn = new Common.CWarning("Please enter a valid SHID.");
                    throw (warn);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnAddrOk_Click(object sender, EventArgs e) {

            Common.AppHelper.HideWarning(popWarning);

            // If we have a selected address, use the shid for our main page.
            if (lstAddressName.SelectedItem != null) {

                string shid = txtAddrSHID.Text;
                txtSHID.Text = shid;
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

                Common.AppHelper.HideWarning(popWarning);

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
				((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", popWarning);
            }
        }

        protected void lstAddressName_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "lstAddressName_SelectedIndexChanged";
            try {

                Common.AppHelper.HideWarning(popWarning);

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
				((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", popWarning);
            }
        }
    }
}
