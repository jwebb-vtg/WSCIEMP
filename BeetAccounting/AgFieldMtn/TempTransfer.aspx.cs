using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.BeetAccounting.AgFieldMtn {

    public partial class TempTransfer : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.AgFieldMtn.TempTransfer.";
        private const string BLANK_CELL = "&nbsp;";
        private const string OLD_CROP_YEAR_WARNING = "You cannot Save in an older Crop Year.";

        private bool _hideTransferContractCol = false;

        private enum TempTransferCols {
            colShareTransferID = 0,
            colToMemberID,
            colFromMemberID,
            colTransferNumber,
            colContractNumber,
            colToShid,
            colToFactoryName,
            colToRetainPct,
            colShares,
            colFromShid,
            colFromFactoryName,
            colHasLienOnShares,
            colToCropPct,
            colPricePerAcre,      
            colIsFeePaid,
            colFromRetainPct,
            colHasConsentForm,
            colApprovalDate,
            colTransferTimeStamp
        }

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";            

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                txtFromSHID.Attributes.Add("onkeypress", "CheckFromEnterKey(event);");
                txtToSHID.Attributes.Add("onkeypress", "CheckToEnterKey(event);");
                btnTransferDelete.Attributes.Add("onclick", "confirm('Are you sure you want to delete this transfer?  Press Ok to continue or Cancel to abort.');");
                btnCustomOk.Attributes.Add("onclick", "copyCustomSelection();");

                locPDF.Text = "";

                if (Globals.IsUserPermissionReadOnly((RolePrincipal)User)) {
                    btnTransferAdd.Enabled = false;
                    btnTransferUpdate.Enabled = false;
                    btnTransferDelete.Enabled = false;
                }

                if (!Page.IsPostBack) {

                    BeetDataDomain.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    ResetFromSHID();
                    ResetToSHID();
                    DoCropYearChange();
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdTransfers);
            base.Render(writer);
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                if (row.Cells[(int)TempTransferCols.colTransferNumber].Text != "*") {

                    row.Attributes["onmouseover"] = "HoverOn(this)";
                    row.Attributes["onmouseout"] = "HoverOff(this)";
                    //row.Attributes.Add("onclick", "SelectAddress(" + row.Cells[0].Text + ");");
                    row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
                }
            }
        }

        protected void grdTransfers_RowCreated(object sender, GridViewRowEventArgs e) {

            //========================================================================
            // This is just too funny.  I call it either a BUG or a Design-MISTAKE!
            // In order to Hide a grid row that you want to hold data you must
            // turn visibility off here, after databinding has taken place.  It 
            // seems the control was not designed to understand this basic need.
            //========================================================================
            if (e.Row.RowType != DataControlRowType.EmptyDataRow) {

                e.Row.Cells[(int)TempTransferCols.colShareTransferID].CssClass = "DisplayNone";
                e.Row.Cells[(int)TempTransferCols.colToMemberID].CssClass = "DisplayNone";
                e.Row.Cells[(int)TempTransferCols.colFromMemberID].CssClass = "DisplayNone";

                if (_hideTransferContractCol) {
                    e.Row.Cells[(int)TempTransferCols.colContractNumber].CssClass = "DisplayNone";
                }
                e.Row.Cells[(int)TempTransferCols.colToCropPct].CssClass = "DisplayNone";
                e.Row.Cells[(int)TempTransferCols.colPricePerAcre].CssClass = "DisplayNone";
                e.Row.Cells[(int)TempTransferCols.colIsFeePaid].CssClass = "DisplayNone";
                e.Row.Cells[(int)TempTransferCols.colFromRetainPct].CssClass = "DisplayNone";                
                e.Row.Cells[(int)TempTransferCols.colHasConsentForm].CssClass = "DisplayNone";
                e.Row.Cells[(int)TempTransferCols.colApprovalDate].CssClass = "DisplayNone";
                e.Row.Cells[(int)TempTransferCols.colTransferTimeStamp].CssClass = "DisplayNone";                
            }
        }

        private string MyFromSHID {
            get { return ViewState["myFromSHID"].ToString(); }
            set { ViewState["myFromSHID"] = value; }
        }

        private string MyFromFactoryNumber {
            get { return ViewState["myFromFactoryNumber"].ToString(); }
            set { ViewState["myFromFactoryNumber"] = value; }
        }

        private string MyToSHID {
            get { return ViewState["myToSHID"].ToString(); }
            set { ViewState["myToSHID"] = value; }
        }

        private string MyToFactoryNumber {
            get { return ViewState["myToFactoryNumber"].ToString(); }
            set { ViewState["myToFactoryNumber"] = value; }
        }

        private string MyFromMemberID {
            get { return ViewState["myFromMemberID"].ToString(); }
            set { ViewState["myFromMemberID"] = value; }
        }

        private string MyToMemberID {
            get { return ViewState["myToMemberID"].ToString(); }
            set { ViewState["myToMemberID"] = value; }
        }

        private void ResetFromSHID() {
            lblFromBusName.Text = "";
            lblFromFactory.Text = "";
            MyFromSHID = "0";
            MyFromMemberID = "0";
            MyFromFactoryNumber = "0";
        }

        private void FindFromAddress(string shid, int cropYear) {

            const string METHOD_NAME = "FindFromAddress";

            try {

                ResetFromSHID();                

                if (shid.Length > 0) {

                    int memberID = 0, addressID = 0, factoryID = 0, factoryNumber = 0;
                    string phone = "", email = "", fax = "", busName = "", factoryName = "";

                    BeetDataMember.GetMemberInfo(shid, cropYear, ref memberID, ref addressID, ref busName, ref phone, ref email, ref fax,
                        ref factoryID, ref factoryNumber, ref factoryName);

                    txtFromSHID.Text = shid;
                    MyFromSHID = shid;
                    MyFromMemberID = memberID.ToString();
                    lblFromBusName.Text = busName;
                    MyFromFactoryNumber = factoryNumber.ToString();
                    lblFromFactory.Text = factoryNumber.ToString("0#") + " " + factoryName;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ResetToSHID() {
            lblToBusName.Text = "";
            lblToFactory.Text = "";
            MyToSHID = "0";
            MyToMemberID = "0";
            MyToFactoryNumber = "0";
        }

        private void ResetTransferEdit() {
            txtTransferShares.Text = "";
            chkTransferAdminFee.Checked = false;
            txtTransferPricePerAcre.Text = "";
            txtTransferToPctRetain.Text = "";
            txtTransferToPctCrop.Text = "";
            txtTransferApprovalDate.Text = "";
        }

        private void FindToAddress(string shid, int cropYear) {

            const string METHOD_NAME = "FindToAddress";

            try {

                ResetToSHID();                

                if (shid.Length > 0) {

                    int memberID = 0, addressID = 0, factoryID = 0, factoryNumber = 0;
                    string phone = "", email = "", fax = "", busName = "", factoryName = "";

                    BeetDataMember.GetMemberInfo(shid, cropYear, ref memberID, ref addressID, ref busName, ref phone, ref email, ref fax,
                        ref factoryID, ref factoryNumber, ref factoryName);

                    txtToSHID.Text = shid;
                    MyToSHID = shid;
                    MyToMemberID = memberID.ToString();
                    lblToBusName.Text = busName;
                    MyToFactoryNumber = factoryNumber.ToString();
                    lblToFactory.Text = factoryNumber.ToString("0#") + " " + factoryName;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillTransferGrid(int memberID, int cropYear) {

            const string METHOD_NAME = "FillTransferGrid";

            try {

                grdTransfers.SelectedIndex = -1;
                List<ListShareTransferItem> listItems = BeetDataMember.ShareTransferGetYear(memberID, cropYear);

                _hideTransferContractCol = true;
                foreach (ListShareTransferItem item in listItems) {
                    if (item.ContractNumber.Length > 0) {
                        _hideTransferContractCol = false;
                        break;
                    }
                }

                grdTransfers.DataSource = listItems;
                grdTransfers.DataBind();
                
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void DoCropYearChange() {

            const string METHOD_NAME = "DoCropYearChange";

            try {

                string shid = txtFromSHID.Text;
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                ResetFromSHID();
                if (shid.Length > 0) {
                    FindFromAddress(shid, cropYear);
                    FillGridFromSummary(Convert.ToInt32(MyFromMemberID), cropYear);
                } else {
                    FillGridFromSummary(0, cropYear);
                }

                shid = txtToSHID.Text;
                ResetToSHID();
                if (shid.Length > 0) {
                    FindToAddress(shid, cropYear);
                    FillGridToSummary(Convert.ToInt32(MyToMemberID), cropYear);
                    ResetTransferEdit();
                    FillTransferGrid(Convert.ToInt32(MyToMemberID), cropYear);
                } else {
                    FillGridToSummary(0, cropYear);
                    ResetTransferEdit();
                    FillTransferGrid(0, cropYear);
                }

            }
            catch (System.Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {
                ResetTransferEdit();
                DoCropYearChange();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillGridFromSummary(int memberID, int cropYear) {

            const string METHOD_NAME = "FillGridFromSummary";

            try {

                List<ListMemberStockSummaryItem> itemList = BeetDataMember.MemberStockGetSummary(memberID, cropYear);

                grdFromSummary.DataSource = itemList;
                grdFromSummary.DataBind();

                grdFromSummary2.DataSource = itemList;
                grdFromSummary2.DataBind();

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillGridToSummary(int memberID, int cropYear) {

            const string METHOD_NAME = "FillGridToSummary";

            try {

                List<ListMemberStockSummaryItem> itemList = BeetDataMember.MemberStockGetSummary(memberID, cropYear);

                grdToSummary.DataSource = itemList;
                grdToSummary.DataBind();

                grdToSummary2.DataSource = itemList;
                grdToSummary2.DataBind();

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
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

        protected void btnAddrOk_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAddrOk_Click";
            Common.AppHelper.HideWarning(addrWarning);
            int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

            try {

                // If we have a selected address, use the shid for our main page.
                if (lstAddressName.SelectedItem != null) {

                    string shid = txtAddrSHID.Text;

                    if (txtActiveControl.Text.Contains("From")) {

                        txtFromSHID.Text = shid;
                        lblFromBusName.Text = txtAddrBName.Text;
                        FindFromAddress(shid, cropYear);
                        uplFromShid.Update();

                    } else {

                        txtToSHID.Text = shid;
                        lblToBusName.Text = txtAddrBName.Text;
                        FindToAddress(shid, cropYear);
                        uplToShid.Update();
                    }
                }

                CloseAndResolve("AddressFinder");
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
        }

        private void CloseAndResolve(string dialogId) {

            string script = string.Format(@"closeAndResolve('{0}', '{1}')", dialogId, txtActiveControl.Text);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, script, true);
        }

        protected void btnResolveFromShid_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnResolveFromShid_Click";
            try {

                string shid = txtFromSHID.Text;
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                FindFromAddress(shid, cropYear);
                FillGridFromSummary(Convert.ToInt32(MyFromMemberID), cropYear);
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnResolveToShid_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnResolveToShid_Click";
            try {

                string shid = txtToSHID.Text;
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                FindToAddress(shid, cropYear);
                FillGridToSummary(Convert.ToInt32(MyToMemberID), cropYear);
                ResetTransferEdit();
                FillTransferGrid(Convert.ToInt32(MyToMemberID), cropYear);
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
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

        protected void btnTransferAdd_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnTransferAdd_Click";

            try {

                // only allowed in this crop year and last crop year.
                if (ddlCropYear.SelectedIndex > 1) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Add is not allowed to a older Crop Year.");
                    throw (warn);
                }

                int shareTransferID = 0;
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int transferNumber = 0;

                string tmpToRetainPct = txtTransferToPctRetain.Text;
                decimal toRetainPct = 0;
                try {
                    toRetainPct = Convert.ToDecimal(tmpToRetainPct);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a positive number between 0 and 100 for the Transferee % Retain.");
                    throw (warn);
                }
                decimal fromRetainPct = 100 - toRetainPct;

                string tmpShares = txtTransferShares.Text;
                int shares = 0;
                try {
                    shares = Convert.ToInt32(tmpShares);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a positive number for Transferee Shares.");
                    throw (warn);
                }
                
                string transferDate = DateTime.Now.ToShortDateString();             // Add creates the transfer date and is not editable.
                bool isFeePaid = (chkTransferAdminFee.Checked == true);

                string approvalDate = txtTransferApprovalDate.Text;
                try {
                    if (approvalDate.Length > 0) {
                        DateTime tmpApprovalDate = Convert.ToDateTime(approvalDate);
                    }
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter valid Transferee Approval Date: mm/dd/yyyy.");
                    throw (warn);
                }

                string tmpPaidPricePerAcre = txtTransferPricePerAcre.Text;
                decimal pricePerAcre = 0;
                try {
                    pricePerAcre = Convert.ToDecimal(tmpPaidPricePerAcre);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid number for the Transferee Price Per Acre.");
                    throw (warn);
                }

                string tmpPctCrop = txtTransferToPctCrop.Text;
                decimal PctCrop = 0;
                try {
                    PctCrop = Convert.ToDecimal(tmpPctCrop);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid number for the Transferee % of Crop.");
                    throw (warn);
                }

                string shareTransferTimeStamp = "";
                string userName = Common.AppHelper.GetIdentityName();
                string contractNumber = "";

                int toSHID = 0;
                string tmpToSHID = txtToSHID.Text;
                if (Common.CodeLib.IsValidSHID(tmpToSHID)) {
                    toSHID = Convert.ToInt32(tmpToSHID);
                } else {
                    Common.CWarning warn = new Common.CWarning("Please enter a valid Transferee SHID and make sure you're seeing the correct Business Name.");
                    throw (warn);
                }

                int fromSHID = 0;
                string tmpFromSHID = txtFromSHID.Text;
                if (Common.CodeLib.IsValidSHID(tmpFromSHID)) {
                    fromSHID = Convert.ToInt32(tmpFromSHID);
                } else {
                    Common.CWarning warn = new Common.CWarning("Please enter a valid Transferor SHID and make sure you're seeing the correct Business Name.");
                    throw (warn);
                }

                int fromFactoryNumber = 0;
                try {
                    fromFactoryNumber = Convert.ToInt32(MyFromFactoryNumber);
                }
                catch {
                    Common.CException warn = new WSCIEMP.Common.CException("Cannot determine the Transferor factory: " + lblFromFactory.Text);
                    throw (warn);
                }
                int toFactoryNumber = 0;
                try {
                    toFactoryNumber = Convert.ToInt32(MyToFactoryNumber);
                }
                catch {
                    Common.CException warn = new WSCIEMP.Common.CException("Cannot determine the Transferee factory: " + lblToFactory.Text);
                    throw (warn);
                }
                
                BeetDataMember.ShareTransferSave(shareTransferID, contractNumber, cropYear, transferNumber, fromSHID,
                    fromFactoryNumber, fromRetainPct, toSHID, toRetainPct, toFactoryNumber, shares, transferDate,
                    isFeePaid, approvalDate, pricePerAcre, PctCrop, shareTransferTimeStamp, userName);

                int toMemberID = Convert.ToInt32(MyToMemberID);

                ResetTransferEdit();
                FillGridFromSummary(Convert.ToInt32(MyFromMemberID), cropYear);
                FillGridToSummary(toMemberID, cropYear);
                FillTransferGrid(toMemberID, cropYear);
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnTransferUpdate_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnTransferUpdate_Click";

            try {

                // only allowed in this crop year and last crop year.
                if (ddlCropYear.SelectedIndex > 1) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Updates are not allowed to a older Crop Year.");
                    throw (warn);
                }

                GridViewRow row = grdTransfers.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before pressing Update, please select a transfer from the Transferee -- Temp Transfer listing.");
                    throw (warn);
                } else {

                    int shareTransferID = Convert.ToInt32(row.Cells[(int)TempTransferCols.colShareTransferID].Text);
                    int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                    int transferNumber = Convert.ToInt32(row.Cells[(int)TempTransferCols.colTransferNumber].Text);

                    string tmpToRetainPct = txtTransferToPctRetain.Text;
                    decimal toRetainPct = 0;
                    try {
                        toRetainPct = Convert.ToDecimal(tmpToRetainPct);
                    }
                    catch {
                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a positive number between 0 and 100 for the Transferee % Retain.");
                        throw (warn);
                    }
                    decimal fromRetainPct = 100 - toRetainPct;

                    string tmpShares = txtTransferShares.Text;
                    int shares = 0;
                    try {
                        shares = Convert.ToInt32(tmpShares);
                    }
                    catch {
                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a positive number for Transferee Shares.");
                        throw (warn);
                    }

                    string transferDate = "";                                                           // Do not change transfer date on update.
                    bool isFeePaid = (chkTransferAdminFee.Checked == true);

                    string approvalDate = txtTransferApprovalDate.Text;
                    try {
                        if (approvalDate.Length > 0) {
                            DateTime tmpApprovalDate = Convert.ToDateTime(approvalDate);
                        }
                    }
                    catch {
                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter valid Transferee Approval Date: mm/dd/yyyy.");
                        throw (warn);
                    }

                    string tmpPaidPricePerAcre = txtTransferPricePerAcre.Text;
                    decimal pricePerAcre = 0;
                    try {
                        pricePerAcre = Convert.ToDecimal(tmpPaidPricePerAcre);
                    }
                    catch {
                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid number for the Transferee Price Per Acre.");
                        throw (warn);
                    }

                    string tmpPctCrop = txtTransferToPctCrop.Text;
                    decimal PctCrop = 0;
                    try {
                        PctCrop = Convert.ToDecimal(tmpPctCrop);
                    }
                    catch {
                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid number for the Transferee % of Crop.");
                        throw (warn);
                    }

                    string shareTransferTimeStamp = row.Cells[(int)TempTransferCols.colTransferTimeStamp].Text.Replace(BLANK_CELL, "");
                    string userName = Common.AppHelper.GetIdentityName();
                    string contractNumber = row.Cells[(int)TempTransferCols.colContractNumber].Text.Replace(BLANK_CELL, "");                    

                    string tmpToSHID = txtToSHID.Text;
                    int toSHID = 0;
                    if (Common.CodeLib.IsValidSHID(tmpToSHID)) {
                        toSHID = Convert.ToInt32(tmpToSHID);
                    } else {
                        Common.CWarning warn = new Common.CWarning("Please enter a valid Transferee SHID and make sure you're seeing the correct Business Name.");
                        throw (warn);
                    }

                    string tmpFromSHID = txtFromSHID.Text;
                    int fromSHID = 0;
                    if (Common.CodeLib.IsValidSHID(tmpFromSHID)) {
                        fromSHID = Convert.ToInt32(tmpFromSHID);
                    } else {
                        Common.CWarning warn = new Common.CWarning("Please enter a valid Transferor SHID and make sure you're seeing the correct Business Name.");
                        throw (warn);
                    }

                    int fromFactoryNumber = Convert.ToInt32(MyFromFactoryNumber);
                    int toFactoryNumber = Convert.ToInt32(MyToFactoryNumber);
 
                    BeetDataMember.ShareTransferSave(shareTransferID, contractNumber, cropYear, transferNumber, fromSHID, 
                        fromFactoryNumber, fromRetainPct, toSHID, toRetainPct, toFactoryNumber, shares, transferDate, 
                        isFeePaid, approvalDate, pricePerAcre, PctCrop, shareTransferTimeStamp, userName);

                    int toMemberID = Convert.ToInt32(MyToMemberID);

                    ResetTransferEdit();
                    FillGridFromSummary(Convert.ToInt32(MyFromMemberID), cropYear);
                    FillGridToSummary(toMemberID, cropYear);
                    FillTransferGrid(toMemberID, cropYear);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnTransferDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnTransferDelete_Click";
            try {

                // only allowed in this crop year and last crop year.
                if (ddlCropYear.SelectedIndex > 1) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Delets is not allowed to a older Crop Year.");
                    throw (warn);
                }

                GridViewRow row = grdTransfers.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before pressing Delete, please select a transfer from the Transferee -- Temp Transfer listing.");
                    throw (warn);
                } else {

                    int shareTransferID = Convert.ToInt32(row.Cells[(int)TempTransferCols.colShareTransferID].Text);
                    BeetDataMember.ShareTransferDelete(shareTransferID);

                    // Clear edit controls                    
                    int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                    int toMemberID = Convert.ToInt32(MyToMemberID);

                    ResetTransferEdit();
                    FillGridFromSummary(Convert.ToInt32(MyFromMemberID), cropYear);
                    FillGridToSummary(toMemberID, cropYear);
                    FillTransferGrid(toMemberID, cropYear);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void SelectTransferGridItem(string shareTransferID) {

			const string METHOD_NAME = "SelectTransferGridItem";

            try {
                foreach (GridViewRow row in grdTransfers.Rows) {
                    if (row.Cells[(int)TempTransferCols.colShareTransferID].Text == shareTransferID) {
                        grdTransfers.SelectedIndex = row.RowIndex;
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void grdTransfers_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdTransfers_SelectedIndexChanged";
            try {

                GridViewRow row = grdTransfers.SelectedRow;

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                string fromShid = txtFromSHID.Text;
                string toShid = txtToSHID.Text;
                string shareTransferID = "";

                ResetTransferEdit();                

                //-----------------------------------
                //	FillTransferEdit
                //-----------------------------------
                shareTransferID = row.Cells[(int)TempTransferCols.colShareTransferID].Text;
                txtTransferShares.Text = row.Cells[(int)TempTransferCols.colShares].Text.Replace(BLANK_CELL, "");
                chkTransferAdminFee.Checked = (row.Cells[(int)TempTransferCols.colIsFeePaid].Text == "Y" ? true : false);
                txtTransferPricePerAcre.Text = row.Cells[(int)TempTransferCols.colPricePerAcre].Text.Replace(BLANK_CELL, "0.00");
                txtTransferToPctRetain.Text = row.Cells[(int)TempTransferCols.colToRetainPct].Text.Replace(BLANK_CELL, "0");
                txtTransferToPctCrop.Text = row.Cells[(int)TempTransferCols.colToCropPct].Text.Replace(BLANK_CELL, "0");
                txtTransferApprovalDate.Text = row.Cells[(int)TempTransferCols.colApprovalDate].Text.Replace(BLANK_CELL, "");

                //-----------------------------------
                //	Refresh SHIDs as needed
                //-----------------------------------
                string selectedFromShid = row.Cells[(int)TempTransferCols.colFromShid].Text;
                string selectedToShid = row.Cells[(int)TempTransferCols.colToShid].Text;

                if (selectedFromShid != fromShid) {
                    FindFromAddress(selectedFromShid, cropYear);
                    FillGridFromSummary(Convert.ToInt32(MyFromMemberID), cropYear);
                }
                if (selectedToShid != toShid) {
                    FindToAddress(selectedToShid, cropYear);
                    FillGridToSummary(Convert.ToInt32(MyToMemberID), cropYear);
                    FillTransferGrid(Convert.ToInt32(MyToMemberID), cropYear);
                    SelectTransferGridItem(shareTransferID);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdTransfers_RowDataBound(object sender, GridViewRowEventArgs e) {
            // Whilie we're here and doing all of the hiding above, let's bold up the transferee SHID.
            if (e.Row.RowType == DataControlRowType.DataRow) {
                string shid = MyToSHID;
                if (e.Row.Cells[(int)TempTransferCols.colFromShid].Text == shid) {
                    e.Row.Cells[(int)TempTransferCols.colFromShid].Font.Bold = true;
                } else {
                    if (e.Row.Cells[(int)TempTransferCols.colToShid].Text == shid) {
                        e.Row.Cells[(int)TempTransferCols.colToShid].Font.Bold = true;
                    }
                }
            }
        }

        private void ShowPDF() {
            string script = string.Format(@"showPDF()");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, script, true);
        }

        protected void btnCYOk_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnCYOk_Click";
            try {

                string pdfFile = "";
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));  // txtCYFirstCropYear
                string lastcy = txtLastCropYear.Text;
                if (lastcy == "") {
                    lastcy = cropYear.ToString();
                }
                int lastCropYear = Convert.ToInt32(lastcy);     // if we didn't capture the last crop year, let the guy print using from/to cy equal
                                                                // the current crop year.  Then user can print and report this as a bug.

                GridViewRow row = grdTransfers.SelectedRow;
                if (row == null) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Before printing, please select a transfer from the Transferee -- Temp Transfer listing.");
                    throw (warn);
                }

                int shareTransferID = Convert.ToInt32(row.Cells[(int)TempTransferCols.colShareTransferID].Text);

                WSCSecurity auth = Globals.SecurityState;
                pdfFile = ConfigurationManager.AppSettings["appControl.rptServer"].ToString() +
                    "?RPTSEL=TEMPTRANSFER" +
                    "&USR=" + Globals.SecurityState.UserName +
                    "&PRMS=" + Common.CodeLib.EncodeString(ConfigurationManager.ConnectionStrings["BeetConn"].ToString()) +
                    "^" + cropYear.ToString() +             // int first crop year: actual crop year.
                    "^" + lastCropYear.ToString() +         // int last crop year
                    "^-1" +                                 // int contractID; -1 when no contract 
                    "^" + shareTransferID.ToString() +      // int shareTransferID
                    "^";                                    // string save path

                locPDF.Text = pdfFile.Replace("localhost", System.Environment.MachineName);
                //uplCropYearFinder.Update();

                //CloseAndPrint();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnCustomOk_Click(object sender, EventArgs e) {


            const string METHOD_NAME = "btnCustomOk_Click";
            try {

                string pdfFile = "";
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int firstCropYear = Convert.ToInt32(txtCustomFirstCropYear.Text);
                int lastCropYear = Convert.ToInt32(txtCustomLastCropYear.Text);

                string toSHID = txtCustomTransfereeSHID.Text;
                string fromSHID = txtCustomTransferorSHID.Text;

                if (!Common.CodeLib.IsNumeric(toSHID)) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid Transferee SHID.");
                    throw (warn);
                }
                if (!Common.CodeLib.IsNumeric(fromSHID)) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid Transferor SHID.");
                    throw (warn);
                }

                //----------------------------
                // These are optional
                //----------------------------
                string shares = txtCustomShares.Text;
                if (shares.Length > 0) {
                    if (!Common.CodeLib.IsNumeric(shares)) {
                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a number for shares or leave it blank.");
                        throw (warn);
                    }
                } else {
                    shares = "0";
                }
                    
                string toRetainPct = txtCustomToPctRetain.Text;
                if (toRetainPct.Length > 0) {
                    if (!Common.CodeLib.IsNumeric(toRetainPct)) {
                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a number for Transferee Retain % or leave it blank.");
                        throw (warn);
                    }
                } else {
                    toRetainPct = "0";
                }

                WSCSecurity auth = Globals.SecurityState;
                pdfFile = ConfigurationManager.AppSettings["appControl.rptServer"].ToString() +
                    "?RPTSEL=TEMPTRANSFERX" +
                    "&USR=" + Globals.SecurityState.UserName +
                    "&PRMS=" + Common.CodeLib.EncodeString(ConfigurationManager.ConnectionStrings["BeetConn"].ToString()) +
                    "^" + cropYear.ToString() +             // int crop year: actual crop year.
                    "^" + firstCropYear.ToString() +        // int first crop year.
                    "^" + lastCropYear.ToString() +         // int last crop year
                    "^" + toSHID +                          // int transferee
                    "^" + fromSHID +                        // int transferor
                    "^" + shares +                          // int shares
                    "^" + toRetainPct +                     // int to retain pct
                    "^";                                    // string save path

                locPDF.Text = pdfFile.Replace("localhost", System.Environment.MachineName);
                //uplPrintCustom.Update();

                //CloseAndCustomPrint();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
