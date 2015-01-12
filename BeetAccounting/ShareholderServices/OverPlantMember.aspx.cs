using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.BeetAccounting.ShareholderServices {

    public partial class OverPlantMember : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.ShareholderServices.OverPlantMember.";
        private const string OVERRIDE_OFF = "Override Off";
        private const string OVERRIDE_ON = "Override On";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                Common.AppHelper.HideWarning(addrWarning);

                txtSHID.Attributes.Add("onkeypress", "CheckEnterKey(event);");

                if (!Page.IsPostBack) {

                    BeetDataDomain.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    MySHID = "";
                    btnCustomPct.Text = OVERRIDE_OFF;
                    txtOverPlantPercentage.ReadOnly = true;
                    txtOverPlantPercentage.CssClass = "highlightRO";

                    BeetDataDomain.FillFactory(ddlFactoryOverPlant);
                    FillOverPlantAcceptance();
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", addrWarning);
            }
        }

        private void FillOverPlantAcceptance() {

            const string METHOD_NAME = "FillOverPlantAcceptance";

            try {

                ddlOverPlantAcceptance.Items.Clear();
                ddlOverPlantAcceptance.Items.Add("Undecided");
                ddlOverPlantAcceptance.Items.Add("Yes");
                ddlOverPlantAcceptance.Items.Add("No");
                ddlOverPlantAcceptance.Items.Add("Partial");

                ddlOverPlantAcceptance.SelectedIndex = 0;                
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
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

                    FindMemberOverPlant(memberID);
                }
                
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ResetShareholder() {

            const string METHOD_NAME = "ResetShareholder";

            try {

                lblBusName.Text = "";
                txtFactoryHome.Text = "";
                ddlOverPlantAcceptance.SelectedIndex = 0;
                txtOverPlantShares.Text = "0";
                chkOverPlantFormReceived.Checked = false;

                ddlFactoryOverPlant.SelectedIndex = 0;
                txtPatronSharesOwned.Text = "0";
                txtOverPlantPercentage.Text = "0";
                txtPossibleOverPlantShares.Text = "0";
                MySHID = "";

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FindMemberOverPlant(int memberID) {

            const string METHOD_NAME = "FindMemberOverPlant";

            try {

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                string userName = Common.AppHelper.GetIdentityName();
                bool isPriorYear = false;

                if (ddlCropYear.SelectedIndex > 1) {
                    isPriorYear = true;
                }

                List<ListOverPlantMemberItem> oplList = BeetDataOverPlant.OverPlantMemberGetInfo(memberID, cropYear, userName);

                if (oplList.Count > 0) {

                    ListOverPlantMemberItem oplMember = oplList[0];

                    MyOverPlantID = oplMember.OverPlantID;
                    txtFactoryHome.Text = oplMember.HomeFactoryName;
                    Common.UILib.SelectDropDownValue(ddlFactoryOverPlant, oplMember.OverPlantFactoryNumber);
                    Common.UILib.SelectDropDown(ddlOverPlantAcceptance, oplMember.OverPlantAccept);
                    txtPatronSharesOwned.Text = oplMember.PatronSharesOwned;
                    txtOverPlantShares.Text = oplMember.OverPlantUsed;
                    txtOverPlantPercentage.Text = oplMember.OverPlantPct;
                    chkOverPlantFormReceived.Checked = oplMember.IsFormReceived;
                    txtPossibleOverPlantShares.Text = oplMember.OverPlantPossible;

                    if (oplMember.IsOverridePct) {
                        btnCustomPct.Text = OVERRIDE_ON;
                        txtOverPlantPercentage.ReadOnly = false;
                        txtOverPlantPercentage.CssClass = "ButtonText";
                    } else {
                        btnCustomPct.Text = OVERRIDE_OFF;
                        txtOverPlantPercentage.ReadOnly = true;
                        txtOverPlantPercentage.CssClass = "highlightRO";
                    }

                    //--------------------------------------------
                    // Is Over Plant allowed?
                    //--------------------------------------------
                    if (!oplMember.IsOverPlantAllowed) {
                        OverPlantNotAllowed();
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Either over plant is not allowed or not set up for this member's factory."); 
                    } else {

                        if (!isPriorYear) {
                            OverPlantAllowed();
                        } else {
                            OverPlantNotAllowed();
                            Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Save is not allowed in a prior year."); 
                        }
                    }
                } else {
                    OverPlantNotAllowed();
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Either over plant is not allowed or not set up for this member's factory."); 
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void OverPlantNotAllowed() {
            btnSave.Enabled = false;                                  
        }
        private void OverPlantAllowed() {
            btnSave.Enabled = true;
        }

        private string MySHID {
            get { return ViewState["mySHID"].ToString(); }
            set { ViewState["mySHID"] = value; }
        }

        private string MyOverPlantID {
            get { return ViewState["myOverPlantID"].ToString(); }
            set { ViewState["myOverPlantID"] = value; }
        }

        private string MyMemberID {
            get { return ViewState["myMemberID"].ToString(); }
            set { ViewState["myMemberID"] = value; }
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            try {

                string shid = txtSHID.Text;
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                string tmp = "";
                tmp = txtOverPlantShares.Text;
                if (tmp.Length == 0) {
                    tmp = "0";
                }
                if (!Common.CodeLib.IsNumeric(tmp)) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a number for 'Over Plant Shares'.");
                    throw (warn);
                }
                int overPlantUsed = Convert.ToInt32(tmp.Replace(",", ""));
                tmp = txtOverPlantPercentage.Text;
                if (tmp.Length == 0) {
                    tmp = "0";
                }
                if (!Common.CodeLib.IsNumeric(tmp)) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a number for 'Over Plant Percentage'.");
                    throw (warn);
                }
                decimal opMemberPct = Convert.ToDecimal(tmp.Replace(",", ""));

                int overPlantPossible = Convert.ToInt32(txtPossibleOverPlantShares.Text.Replace(",", ""));
                string overPlantAccept = ddlOverPlantAcceptance.SelectedItem.ToString().Substring(0, 1);
                bool isFormReceived = chkOverPlantFormReceived.Checked;

                CalculateOverPlant();
                CheckOverPlantShares();

                bool isPossibleDifferent = (overPlantPossible.ToString() != txtPossibleOverPlantShares.Text);
                bool isSharesDifferent = (overPlantUsed.ToString() != txtOverPlantShares.Text);

                // We should not have a difference in possible or shares used after calling
                // CalculateOverPlant, but if we do, throw a warning to the user.
                if (isPossibleDifferent && isSharesDifferent) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("The number of Possible Over Plant Shares and the number of Over Plant Shares were recalculated.  If you approve, press Save again.");
                    throw (warn);
                }
                if (isPossibleDifferent) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("The number of Possible Over Plant Shares were recalculated.  If you approve, press Save again.");
                    throw (warn);
                }
                if (isSharesDifferent) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("The number of Over Plant Shares were recalculated.  If you approve, press Save again.");
                    throw (warn);
                }

                if (overPlantUsed > overPlantPossible) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please make sure that the number of Over Plant Shares is not greater than the number of Possible Over Plant Shares.");
                    throw (warn);
                }

                switch (overPlantAccept) {
                    case "U":
                        if (overPlantUsed != 0) {
                            Common.CWarning warn = new WSCIEMP.Common.CWarning("When Over Plant Acceptance is 'Undecided', the Over Plant Shares must equal 0.");
                            throw (warn);
                        }
                        break;
                    case "N":
                        if (overPlantUsed != 0) {
                            Common.CWarning warn = new WSCIEMP.Common.CWarning("When Over Plant Acceptance is 'No', the Over Plant Shares must equal 0.");
                            throw (warn);
                        }
                        break;
                    case "Y":
                        if (overPlantUsed != overPlantPossible) {
                            Common.CWarning warn = new WSCIEMP.Common.CWarning("When Over Plant Acceptance is 'Yes', the Over Plant Shares must equal the 'Possible Over Plant Shares'.");
                            throw (warn);
                        }
                        break;
                    case "P":
                        break;
                }

                int opFactoryNumber = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlFactoryOverPlant));
                if (opFactoryNumber == 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please select an Over Plant Factory.");
                    throw (warn);
                }

                string userName = Common.AppHelper.GetIdentityName();
                bool isOverridePct = (btnCustomPct.Text == OVERRIDE_ON);

                BeetDataOverPlant.OverPlantSave(Convert.ToInt32(MyOverPlantID), Convert.ToInt32(MyMemberID), cropYear, overPlantUsed, overPlantAccept,
                    isFormReceived, isOverridePct, opMemberPct, opFactoryNumber, userName);
                FindAddress(shid);             
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {

                string shid = txtSHID.Text;
                FindAddress(shid);                 
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnCustomPct_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnCustomPct_Click";

            try {

                bool isOverrideOff = (btnCustomPct.Text == OVERRIDE_OFF);

                if (isOverrideOff) {
                    
                    // turn it on
                    btnCustomPct.Text = OVERRIDE_ON;
                    txtOverPlantPercentage.ReadOnly = false;
                    txtOverPlantPercentage.CssClass = "ButtonText";

                } else {

                    // turn it off
                    btnCustomPct.Text = OVERRIDE_OFF;
                    txtOverPlantPercentage.ReadOnly = true;
                    txtOverPlantPercentage.CssClass = "highlightRO";

                    // When turning off, you have to go back to the factory percentage.
                    ddlFactoryOverPlant_SelectedIndexChanged(null, null);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlFactoryOverPlant_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlFactoryOverPlant_SelectedIndexChanged";

            try {
                //-----------------------------------------------------------------------------------------
                // Changing the Over Plant factory can change the Over Plant percentage, which cna impact
                // the Possible Over Plant Shares, and if Acceptance is 'Yes' then the Over Plant Shares are 
                // also changed.
                //-----------------------------------------------------------------------------------------

                if (ddlFactoryOverPlant.SelectedIndex != 0) {

                    int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                    int factoryNumber = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlFactoryOverPlant));

                    List<ListOverPlantFactoryItem> opfItem = BeetDataOverPlant.OverPlantFactoryByNumber(cropYear, factoryNumber);

                    if (opfItem.Count > 0) {
                        txtOverPlantPercentage.Text = opfItem[0].Percentage;
                        if (opfItem[0].IsOverPlantAllowed == "Yes") {
                            OverPlantAllowed();
                        } else {
                            OverPlantNotAllowed();
                            Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Either over plant is not allowed or not set up for this member's factory."); 
                        }
                        CalculateOverPlant();
                        CheckOverPlantShares();
                    }
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void CalculateOverPlant() {

            const string METHOD_NAME = "CalculateOverPlant";

            try {

                decimal opPossible = 0;
                int opSharesOwned = 0;
                string tempSharesOwned = txtPatronSharesOwned.Text;
                decimal opPct = 0;
                string tempPct = txtOverPlantPercentage.Text;

                //----------------------------------------
                // Handle empty input.
                //----------------------------------------
                if (String.IsNullOrEmpty(tempSharesOwned)) {
                    opSharesOwned = 0;
                } else {
                    opSharesOwned = Convert.ToInt32(tempSharesOwned.Replace(",", ""));
                }
                
                if (String.IsNullOrEmpty(tempPct)) {
                    opPct = 0;
                } else {
                    opPct = Convert.ToDecimal(tempPct);
                }

                // Keep in sync w/ database.  Db adds .499
                opPossible = Math.Round((opSharesOwned * (opPct / 100)) + 0.499M, 0);
                txtPossibleOverPlantShares.Text = opPossible.ToString("#,##0");

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void CheckOverPlantShares() {

            const string METHOD_NAME = "CheckOverPlantShares";

            try {

                string opAcceptance = Common.UILib.GetDropDownText(ddlOverPlantAcceptance);

                if (opAcceptance == "Yes") {
                    
                    // when 'Yes', Over Plant Shares = Possible Over Plant Shares
                    txtOverPlantShares.Text = txtPossibleOverPlantShares.Text;
                } else {

                    if (opAcceptance == "No" || opAcceptance == "Undecided") {
                        txtOverPlantShares.Text = "0";
                    } else {

                        // That leaves "Partial", just warn if Over Plant Shares > Possible Over Plant Shares.
                        if (txtOverPlantShares.Text.Length == 0) {
                            txtOverPlantShares.Text = "0";
                        }
                        if (txtPossibleOverPlantShares.Text.Length == 0) {
                            txtPossibleOverPlantShares.Text = "0";
                        }

                        if (Convert.ToInt32(txtOverPlantShares.Text.Replace(",", "")) > Convert.ToInt32(txtPossibleOverPlantShares.Text.Replace(",", ""))) {
                            Common.CWarning warn = new WSCIEMP.Common.CWarning("Please make the number of Over Plant Shares less than or equal to the number of Possible Over Plant Shares.");
                            throw (warn);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }


        protected void btnCalc_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnCalc_Click";

            try {
                CalculateOverPlant();
                CheckOverPlantShares();
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
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
