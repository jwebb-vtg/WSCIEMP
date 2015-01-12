using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Fields {

    public partial class FieldInfo : Common.BasePage {

        private const string MOD_NAME = "Fields.FieldInfo.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            UsrCntSelector.ContractNumberChange += new WSCField.ContractNumberChangeHandler(UsrCntSelector_ContractNumberChange);
            UsrCntSelector.ContractNumberFind += new WSCField.ContractNumberFindHandler(UsrCntSelector_ContractNumberFind);
            UsrCntSelector.ContractNumberNext += new WSCField.ContractNumberNextHandler(UsrCntSelector_ContractNumberNext);
            UsrCntSelector.ContractNumberPrev += new WSCField.ContractNumberPrevHandler(UsrCntSelector_ContractNumberPrev);
            UsrCntSelector.ShareholderFind += new WSCField.ShareholderFindHandler(UsrCntSelector_ShareholderFind);
            UsrCntSelector.SequenceNumberChange += new WSCField.SequenceNumberChangeHandler(UsrCntSelector_SequenceNumberChange);
            UsrCntSelector.ExceptionShow += new WSCField.FieldExceptionHandler(UsrCntSelector_FieldExceptionHandler);

            // MUST DO THIS OUTSIDE OF Try/Catch block.
            if (Globals.IsBeetTransfer) {

                // check a bad BeetAccounting transfer.  A contractID < 0 occurs when you have a new 
                // contract in BeetAccounting that you have not saved to the databae, but you transfer
                // to this system anyway.
                if (Globals.FieldData.ContractID < 0) {

                    // This is a weak point in the whole getting rid of _shs and _fld concept.  However i estimate
                    // that there is only the smallest chance of _shs being corrupted or corrupting another page during 
                    // a transfer from the BeetAccounting application.
                    Globals.WarningMessage = "You must first Save your contract in BeetAccounting before adding any fields.  " +
                        "Please close this screen, return to BeetAccounting, Save you contract, then return here.";
                    Server.Transfer(Page.ResolveUrl("~/SHS/ShsHome.aspx"));
                }
            }

            if (Globals.IsUserPermissionReadOnly((RolePrincipal)User)) {
                btnAddField.Enabled = false;
                btnDelete.Enabled = false;
                btnNew.Enabled = false;
                btnRemoveField.Enabled = false;
                btnSvrSave.Enabled = false;
            }

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                btnDelete.Attributes.Add("onclick", "CheckDelete(event);");

                if (!Page.IsPostBack) {
                    UsrCntSelector.ControlHostPageLoad += new WSCField.ControlHostPageLoadHandler(UsrCntSelector_ControlHostPageLoad);
                } 
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void ClearFieldDetail() {

            txtFSANumber.Text = "";
            txtWscFieldName.Text = "";
            txtAcres.Text = "";

            txtLatitude.Text = "";
            txtLongitude.Text = "";
            txtDescription.Text = "";

            chkFSAOfficial.Checked = false;

            txtCntLldID.Text = "";
            txtLldID.Text = "";

            SelectState(0);
            SelectFsaState(0);

            Common.UILib.SelectDropDown(ddlCounty, -1);
            Common.UILib.SelectDropDown(ddlFSAState, -1);
            Common.UILib.SelectDropDown(ddlFSACounty, -1);
            Common.UILib.SelectDropDown(ddlQuadrant, -1);
            Common.UILib.SelectDropDown(ddlQuarterQuadrant, -1);
            Common.UILib.SelectDropDown(ddlRange, -1);
            Common.UILib.SelectDropDown(ddlSection, -1);
            Common.UILib.SelectDropDown(ddlState, -1);
            Common.UILib.SelectDropDown(ddlTownship, -1);
            Common.UILib.SelectDropDown(ddlQuarterField, -1);

            txtFarmNo.Text = "";
            txtTractNo.Text = "";
            txtFieldNo.Text = "";
        }

        private void FillDomainData() {

            Domain domain = WSCField.GetDomainData();

            ddlState.Items.Clear();
            foreach (TItem item in domain.StateList.Items) {
                ddlState.Items.Add(item.Name);
            }

            ddlFSAState.Items.Clear();
            foreach (TItem item in domain.StateList.Items) {
                ddlFSAState.Items.Add(item.Name);
            }

            // Section
            ddlSection.Items.Clear();
            foreach (TItem item in domain.SectionList.Items) {
                ddlSection.Items.Add(item.Name);
            }

            // Quadrant
            ddlQuadrant.Items.Clear();
            foreach (TItem item in domain.QuadrantList.Items) {
                ddlQuadrant.Items.Add(item.Name);
            }

            // Quarter Quadrant
            ddlQuarterQuadrant.Items.Clear();
            foreach (TItem item in domain.QuarterQuadrantList.Items) {
                ddlQuarterQuadrant.Items.Add(item.Name);
            }

            // Quarter Field
            ddlQuarterField.Items.Clear();
            foreach (TItem item in domain.QuarterQuadrantList.Items) {
                ddlQuarterField.Items.Add(item.Name);
            }
        }

        private void SelectState(int index) {

            string stateName = ddlState.Items[index].Text;
            SelectState(stateName);
        }

        private void SelectFsaState(int index) {

            string stateName = ddlFSAState.Items[index].Text;
            SelectFsaState(stateName);
        }

        private void SelectState(string stateName) {

            Domain domain = WSCField.GetDomainData();
            int selectedIndex = -1;
            foreach (System.Web.UI.WebControls.ListItem item in ddlState.Items) {

                if (item.Text == stateName) {
                    item.Selected = true;
                    selectedIndex = ddlState.Items.IndexOf(item);
                } else {
                    item.Selected = false;
                }
            }
            ddlState.SelectedIndex = selectedIndex;

            // Fill County Dropdown
            ddlCounty.Items.Clear();
            foreach (TItem2 item in domain.CountyList.Items) {
                if (item.Field1 == stateName) {
                    ddlCounty.Items.Add(item.Field2);
                }
            }

            // Fill Township Dropdown
            ddlTownship.Items.Clear();
            foreach (TItem2 item in domain.TownshipList.Items) {
                if (item.Field1 == stateName) {
                    ddlTownship.Items.Add(item.Field2);
                }
            }

            // Fill Range Dropdown
            ddlRange.Items.Clear();
            foreach (TItem2 item in domain.RangeList.Items) {
                if (item.Field1 == stateName) {
                    ddlRange.Items.Add(item.Field2);
                }
            }
        }

        private void SelectFsaState(string stateName) {

            Domain domain = WSCField.GetDomainData();
            int selectedIndex = -1;
            foreach (System.Web.UI.WebControls.ListItem item in ddlFSAState.Items) {

                if (item.Text == stateName) {
                    item.Selected = true;
                    selectedIndex = ddlFSAState.Items.IndexOf(item);
                } else {
                    item.Selected = false;
                }
            }
            ddlFSAState.SelectedIndex = selectedIndex;

            // Fill FSA County Dropdown
            ddlFSACounty.Items.Clear();
            foreach (TItem2 item in domain.CountyList.Items) {
                if (item.Field1 == stateName) {
                    ddlFSACounty.Items.Add(item.Field2);
                }
            }
        }

        private void ShowContractFieldDetail() {

            // Always clear the new field indicators
            ArrayList badAttributes = new ArrayList();

            ClearFieldDetail();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCField.CntLldGetDetail(conn, UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber)) {

                        if (dr.Read()) {

                            UsrCntSelector.CntLLDID = dr.GetInt32(dr.GetOrdinal("cntlld_cntlld_id"));
                            txtCntLldID.Text = UsrCntSelector.CntLLDID.ToString();
                            UsrCntSelector.LldID = dr.GetInt32(dr.GetOrdinal("cntlld_lld_id"));

                            UsrCntSelector.ContractID = dr.GetInt32(dr.GetOrdinal("cntlld_contract_id"));
                            UsrCntSelector.ContractNumber = dr.GetInt32(dr.GetOrdinal("cntlld_contract_no"));
                            UsrCntSelector.FieldName = dr.GetString(dr.GetOrdinal("cntlld_field_name"));
                            UsrCntSelector.FsaNumber = dr.GetString(dr.GetOrdinal("cntlld_fsa_number"));
                            UsrCntSelector.SequenceNumber = dr.GetInt32(dr.GetOrdinal("cntlld_sequence_number"));
                            UsrCntSelector.FieldState = dr.GetString(dr.GetOrdinal("cntlld_state"));
                            SelectState(dr.GetString(dr.GetOrdinal("cntlld_state")));
                            SelectFsaState(dr.GetString(dr.GetOrdinal("cntlld_fsa_state")));

                            if (!Common.UILib.SelectDropDown(ddlCounty, dr.GetString(dr.GetOrdinal("cntlld_county")))) {
                                badAttributes.Add("County");
                            }
                            if (!Common.UILib.SelectDropDown(ddlFSACounty, dr.GetString(dr.GetOrdinal("cntlld_fsa_county")))) {
                                badAttributes.Add("FSA County");
                            }
                            if (!Common.UILib.SelectDropDown(ddlTownship, dr.GetString(dr.GetOrdinal("cntlld_township")))) {
                                badAttributes.Add("Township");
                            }
                            if (!Common.UILib.SelectDropDown(ddlRange, dr.GetString(dr.GetOrdinal("cntlld_range")))) {
                                badAttributes.Add("Range");
                            }
                            if (!Common.UILib.SelectDropDown(ddlSection, dr.GetString(dr.GetOrdinal("cntlld_section")))) {
                                badAttributes.Add("Section");
                            }
                            if (!Common.UILib.SelectDropDown(ddlQuadrant, dr.GetString(dr.GetOrdinal("cntlld_quadrant")))) {
                                badAttributes.Add("Quadrant");
                            }
                            if (!Common.UILib.SelectDropDown(ddlQuarterQuadrant, dr.GetString(dr.GetOrdinal("cntlld_quarter_quadrant")))) {
                                badAttributes.Add("Quarter Quadrant");
                            }
                            if (!Common.UILib.SelectDropDown(ddlQuarterField, dr.GetString(dr.GetOrdinal("cntlld_quarter_field")))) {
                                badAttributes.Add("Quarter Field");
                            }
                            txtDescription.Text = dr.GetString(dr.GetOrdinal("cntlld_description"));
                            txtLongitude.Text = (dr.GetDecimal(dr.GetOrdinal("cntlld_longitude"))).ToString("0.00000");
                            txtLatitude.Text = (dr.GetDecimal(dr.GetOrdinal("cntlld_latitude"))).ToString("0.00000");
                            txtWscFieldName.Text = dr.GetString(dr.GetOrdinal("cntlld_field_name"));
                            txtAcres.Text = (dr.GetInt32(dr.GetOrdinal("cntlld_acres"))).ToString();
                            chkFSAOfficial.Checked = dr.GetBoolean(dr.GetOrdinal("cntlld_fsa_official"));
                            txtFSANumber.Text = dr.GetString(dr.GetOrdinal("cntlld_fsa_number"));

                            Common.UILib.SelectDropDown(ddlFSACounty, dr.GetString(dr.GetOrdinal("cntlld_fsa_county")));
                            txtFarmNo.Text = dr.GetString(dr.GetOrdinal("cntlld_farm_number"));
                            txtTractNo.Text = dr.GetString(dr.GetOrdinal("cntlld_tract_number"));
                            txtFieldNo.Text = dr.GetString(dr.GetOrdinal("cntlld_field_number"));

                            //txtOtherLldContracts.Text = (dr.GetBoolean(dr.GetOrdinal("cntlld_other_lld_contracts")) ? "1" : "");
                        }
                    }
                }

                if (badAttributes.Count > 0) {

                    string atribList = "";
                    foreach (string s in badAttributes) {
                        if (atribList.Length == 0) {
                            atribList = s;
                        } else {
                            atribList += ", " + s;
                        }
                    }
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The following Fields have values that are no longer supported and " +
                        "cannot be saved back to the system: " + atribList + ".");
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("FieldInfo.ShowContractFieldDetail", ex);
                throw (wex);
            }
        }

        private void ShowFieldDetail(int lldID) {

            // Always clear the new field indicators
            ArrayList badAttributes = new ArrayList();

            ClearFieldDetail();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCField.CntLldGetDetailByLLD(conn, lldID, UsrCntSelector.CropYear)) {

                        if (dr.Read()) {

                            UsrCntSelector.LldID = dr.GetInt32(dr.GetOrdinal("lld_lld_id"));
                            UsrCntSelector.FieldName = dr.GetString(dr.GetOrdinal("lld_field_name"));
                            UsrCntSelector.FsaNumber = dr.GetString(dr.GetOrdinal("lld_fsa_number"));

                            UsrCntSelector.FieldState = dr.GetString(dr.GetOrdinal("lld_state"));
                            SelectState(dr.GetString(dr.GetOrdinal("lld_state")));
                            SelectFsaState(dr.GetString(dr.GetOrdinal("lld_fsa_state")));

                            // COUNTY
                            if (!Common.UILib.SelectDropDown(ddlCounty, dr.GetString(dr.GetOrdinal("lld_county")))) {
                                badAttributes.Add("County");
                            }

                            // FSA COUNTY
                            if (!Common.UILib.SelectDropDown(ddlFSACounty, dr.GetString(dr.GetOrdinal("lld_fsa_county")))) {
                                badAttributes.Add("FSA County");
                            }

                            // TOWNSHIP
                            if (!Common.UILib.SelectDropDown(ddlTownship, dr.GetString(dr.GetOrdinal("lld_township")))) {
                                badAttributes.Add("Township");
                            }

                            // RANGE
                            if (!Common.UILib.SelectDropDown(ddlRange, dr.GetString(dr.GetOrdinal("lld_range")))) {
                                badAttributes.Add("Range");
                            }

                            // SECTION
                            if (!Common.UILib.SelectDropDown(ddlSection, dr.GetString(dr.GetOrdinal("lld_section")))) {
                                badAttributes.Add("Section");
                            }

                            // QUADRANT
                            if (!Common.UILib.SelectDropDown(ddlQuadrant, dr.GetString(dr.GetOrdinal("lld_quadrant")))) {
                                badAttributes.Add("Quadrant");
                            }

                            // QUARTER QUADRANT
                            if (!Common.UILib.SelectDropDown(ddlQuarterQuadrant, dr.GetString(dr.GetOrdinal("lld_quarter_quadrant")))) {
                                badAttributes.Add("Quarter Quadrant");
                            }

                            // QUARTER FIELD
                            if (!Common.UILib.SelectDropDown(ddlQuarterField, dr.GetString(dr.GetOrdinal("lld_quarter_field")))) {
                                badAttributes.Add("Quarter Field");
                            }

                            txtCntLldID.Text = dr.GetInt32(dr.GetOrdinal("lld_cntlld_id")).ToString();
                            txtDescription.Text = dr.GetString(dr.GetOrdinal("lld_description"));
                            txtLongitude.Text = (dr.GetDecimal(dr.GetOrdinal("lld_longitude"))).ToString("0.00000");
                            txtLatitude.Text = (dr.GetDecimal(dr.GetOrdinal("lld_latitude"))).ToString("0.00000");
                            txtWscFieldName.Text = dr.GetString(dr.GetOrdinal("lld_field_name"));
                            txtAcres.Text = (dr.GetInt32(dr.GetOrdinal("lld_acres"))).ToString();
                            chkFSAOfficial.Checked = dr.GetBoolean(dr.GetOrdinal("lld_fsa_official"));
                            txtFSANumber.Text = dr.GetString(dr.GetOrdinal("lld_fsa_number"));
                            //txtOtherLldContracts.Text = (dr.GetBoolean(dr.GetOrdinal("lld_other_contracts")) ? "1" : "");

                            Common.UILib.SelectDropDown(ddlFSACounty, dr.GetString(dr.GetOrdinal("lld_fsa_county")));
                            txtFarmNo.Text = dr.GetString(dr.GetOrdinal("lld_farm_number"));
                            txtTractNo.Text = dr.GetString(dr.GetOrdinal("lld_tract_number"));
                            txtFieldNo.Text = dr.GetString(dr.GetOrdinal("lld_field_number"));
                        }
                    }
                }
                if (badAttributes.Count > 0) {

                    string atribList = "";
                    foreach (string s in badAttributes) {
                        if (atribList.Length == 0) {
                            atribList = s;
                        } else {
                            atribList += ", " + s;
                        }
                    }
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The following Fields have values that are no longer supported and " +
                        "cannot be saved back to the system: " + atribList + ".");
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("FieldInfo.ShowFieldDetail", ex);
                throw (wex);
            }
        }

        private void NewContractAlertOn() {

            lblNewField.Text = " *** New Field ***";
            lblNewField.CssClass = "WarningOn";
        }

        private void NewContractAlertOff() {

            lblNewField.Text = "";
            lblNewField.CssClass = "WarningOff";
        }

        private void ShowError(Exception ex, string userMessage) {
			((PrimaryTemplate)Page.Master).ShowWarning(ex, userMessage);
        }

        private void UsrCntSelector_ControlHostPageLoad(object sender, ContractSelectorEventArgs e) {

            FillDomainData();
            //txtCropYear.Text = UsrCntSelector.CropYear.ToString();

            if (UsrCntSelector.ContractID > 0) {

                // This is a transfer from BeetAccounting via BeetActConnect.aspx, or
                // returning to this page from another page.
                WSCSecurity auth = Globals.SecurityState;
                if (!UsrCntSelector.IsOwner) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to view this information.");
                    return;
                }

                ShowContractFieldDetail();

            } 
        }

        private void UsrCntSelector_ContractNumberChange(object sender, ContractSelectorEventArgs e) {

            try {

                if (e.Error() == null) {
                    NewContractAlertOff();
                    ShowContractFieldDetail();
                } else {
                    ShowError(e.Error(), "Unable to change contract numbers at this time.");
                }
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to change contract numbers at this time.");
            }
        }

        private void UsrCntSelector_ContractNumberFind(object sender, ContractSelectorEventArgs e) {

            try {

                if (e.Error() == null) {
                    NewContractAlertOff();
                    ShowContractFieldDetail();
                } else {
                    ShowError(e.Error(), "Unable to find contract numbers at this time.");
                }
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to find contract numbers at this time.");
            }
        }

        private void UsrCntSelector_ContractNumberPrev(object sender, ContractSelectorEventArgs e) {

            try {

                if (e.Error() == null) {
                    NewContractAlertOff();
                    ShowContractFieldDetail();
                } else {
                    ShowError(e.Error(), "Unable to change contract numbers at this time.");
                }
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to change contract numbers at this time.");
            }
        }

        private void UsrCntSelector_ContractNumberNext(object sender, ContractSelectorEventArgs e) {

            try {

                if (e.Error() == null) {
                    NewContractAlertOff();
                    ShowContractFieldDetail();
                } else {
                    ShowError(e.Error(), "Unable to change contract numbers at this time.");
                }
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to change contract numbers at this time.");
            }
        }

        private void UsrCntSelector_ShareholderFind(object sender, ContractSelectorEventArgs e) {

            try {

                if (e.Error() == null) {
                    NewContractAlertOff();
                    ShowContractFieldDetail();
                } else {
                    ShowError(e.Error(), "Unable to find shareholder at this time.");
                }
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to find shareholder at this time.");
            }
        }

        private void UsrCntSelector_SequenceNumberChange(object sender, ContractSelectorEventArgs e) {

            try {
                if (e.Error() == null) {
                    NewContractAlertOff();
                    ShowContractFieldDetail();
                } else {
                    ShowError(e.Error(), "Unable to change sequence numbers at this time.");
                }
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to change sequence numbers at this time.");
            }
        }

        private void UsrCntSelector_FieldExceptionHandler(object Sender, Common.CErrorEventArgs e) {
            ShowError(e.Error(), "");
        }

        protected void btnAddField_Click(object sender, EventArgs e) {

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                WSCSecurity auth = Globals.SecurityState;
                if (!UsrCntSelector.IsOwner) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                    return;
                }

                // Add the field if it has a valid lld_id
                if (UsrCntSelector.LldID == 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You must first Save this field before adding it to the Contract.");
                    return;
                }
                if (UsrCntSelector.CntLLDID > 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "This field is already contracted for this season.");
                    return;
                }
                if (!(UsrCntSelector.ContractID > 0)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You must first select a contract before adding a field to the contract.");
                    return;
                }

                // okay, add the field to the contract.
                int cntlld_cntlld_id_out = 0;

                WSCField.CntLldAddField(UsrCntSelector.ContractID, UsrCntSelector.LldID, UsrCntSelector.CropYear,
                    auth.UserName, ref cntlld_cntlld_id_out);

                // Bounce sequence number to max value
                if (cntlld_cntlld_id_out > 0) {
                    int newMaxSeq = UsrCntSelector.SequenceNumberMax + 1;
                    UsrCntSelector.SetSequenceNumber(newMaxSeq, newMaxSeq);
                }

                ShowContractFieldDetail();
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to Add this Field to this Contract at this time.");
            }
        }

        protected void btnRemoveField_Click(object sender, EventArgs e) {

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                WSCSecurity auth = Globals.SecurityState;
                if (!UsrCntSelector.IsOwner) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                    return;
                }

                // Remove the field from the contract if it has a valid cntlld entry.
                if (UsrCntSelector.SequenceNumber > 0) {

                    int cntlld_id = UsrCntSelector.CntLLDID;
                    if (cntlld_id > 0) {
                        WSCField.CntLldDelete(cntlld_id);
                    }

                    UsrCntSelector.SequenceNumberMax = UsrCntSelector.SequenceNumberMax - 1;
                    UsrCntSelector.SetSequenceNumber(UsrCntSelector.SequenceNumberMax, 1);
                    ShowContractFieldDetail();
                } else {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You must be viewing a field on this contract before removing a field from the contract.");
                    return;
                }
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to Remove this Field to this Contract at this time.");
            }		
        }

        protected void btnReset_Click(object sender, EventArgs e) {

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                int seqMax = UsrCntSelector.SequenceNumberMax;
                UsrCntSelector.ResetField();
                UsrCntSelector.SetSequenceNumber(seqMax, 0);

                ClearFieldDetail();
                NewContractAlertOff();
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to Resest the Field at this time.");
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                WSCSecurity auth = Globals.SecurityState;
                if (!UsrCntSelector.IsOwner) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                    return;
                }

                NewContractAlertOn();

                // This process creates a temporary new field, but Does Not clear or reset
                // the sequence list for any current contract.

                int seqMax = UsrCntSelector.SequenceNumberMax;
                UsrCntSelector.ResetField();
                UsrCntSelector.SetSequenceNumber(seqMax, 0);
                ClearFieldDetail();

                //txtOtherLldContracts.Text = "";


            }
            catch (Exception ex) {
                ShowError(ex, "Unable to generate a New Field at this time.");
            }		
        }

        protected void btnSvrSave_Click(object sender, EventArgs e) {

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                WSCSecurity auth = Globals.SecurityState;
                if (!UsrCntSelector.IsOwner) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                    return;
                }

                if (!UsrCntSelector.IsOwner) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                    return;
                }

                // Can only Save crop years >= 2006
                if (UsrCntSelector.CropYear < 2006) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You cannot Save information for Crop Years before 2006.");
                    return;
                }

                // This performs an Insert or Update as needed for the current 
                // Field \ Land Description.  
                int lldIDOUT = 0;
                Domain domain = WSCField.GetDomainData();

                // STATE
                string state = Common.UILib.GetDropDownText(ddlState);
                if (state.Length == 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You must select a State from the list.");
                    return;
                }

                if (!domain.StateList.Contains(state)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The State you selected, " + state + ", is no longer supported.  Please select a new value.");
                    return;
                }

                // County
                string county = Common.UILib.GetDropDownText(ddlCounty);
                if (county.Length == 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You must select a County from the list.");
                    return;
                }
                if (!domain.CountyList.Contains(state, county)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The County you selected, " + county + ", is no longer supported.  Please select a new value.");
                    return;
                }

                // FSA STATE
                string fsaState = Common.UILib.GetDropDownText(ddlFSAState);
                //if (state.Length == 0) {
                    //Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You must select a State from the list.");
                    //return;
                //}

                // FSA County
                string fsaCounty = Common.UILib.GetDropDownText(ddlFSACounty);
                if (!domain.CountyList.Contains(fsaState, fsaCounty)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The FSA County you selected, " + fsaCounty + ", is no longer supported.  Please select a new value.");
                    return;
                }

                // TOWNSHIP
                string township = Common.UILib.GetDropDownText(ddlTownship);
                if (!domain.TownshipList.Contains(state, township)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Township you selected, " + township + ", is no longer supported.  Please select a new value.");
                    return;
                }

                // RANGE
                string range = Common.UILib.GetDropDownText(ddlRange);
                if (!domain.RangeList.Contains(state, range)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Range you selected, " + range + ", is no longer supported.  Please select a new value.");
                    return;
                }

                // SECTION
                string section = Common.UILib.GetDropDownText(ddlSection);
                if (!domain.SectionList.Contains(section)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Section you selected, " + section + ", is no longer supported.  Please select a new value.");
                    return;
                }

                // QUADRANT
                string quadrant = Common.UILib.GetDropDownText(ddlQuadrant);
                if (!domain.QuadrantList.Contains(quadrant)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Quadrant you selected, " + quadrant + ", is no longer supported.  Please select a new value.");
                    return;
                }

                // QUARTER QUADRANT
                string quarterQuadrant = Common.UILib.GetDropDownText(ddlQuarterQuadrant);
                if (!domain.QuarterQuadrantList.Contains(quarterQuadrant)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Quarter Quadrant you selected, " + quarterQuadrant + ", is no longer supported.  Please select a new value.");
                    return;
                }

                // QUARTER FIELD
                string quarterField = Common.UILib.GetDropDownText(ddlQuarterField);
                if (!domain.QuarterQuadrantList.Contains(quarterField)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Quarter Field you selected, " + quarterField + ", is no longer supported.  Please select a new value.");
                    return;
                }

                string latitude = txtLatitude.Text;
				decimal dLatitude = 0;
                if (latitude.Length == 0) {
                    latitude = "0";
                } else {
					if (!Decimal.TryParse(latitude, out dLatitude)) {
						Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter just a number for GPS Latitude, no degree character.");
						return;
					}
					latitude = dLatitude.ToString("0.00000");
                }

                string longitude = txtLongitude.Text;
				decimal dLongitude = 0;
                if (longitude.Length == 0) {
                    longitude = "0";
                } else {
					if (!Decimal.TryParse(longitude, out dLongitude)) {
						Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter just a number for GPS Longitude, no degree character.");
						return;
					}
					longitude = dLongitude.ToString("0.00000");
                }

                string description = txtDescription.Text;
                if (description != Server.HtmlEncode(description)) {
                    Common.CWarning wex = new Common.CWarning("Your Description cannot contain '<', '>', or '&' characters.");
                    throw (wex);
                }

                if (description.Length > 100) {
                    description = description.Substring(0, 100);
                }

                string farmNumber = txtFarmNo.Text;
                if (farmNumber != Server.HtmlEncode(farmNumber)) {
                    Common.CWarning wex = new Common.CWarning("Your Farm Number cannot contain '<', '>', or '&' characters.");
                    throw (wex);
                }
                string tractNumber = txtTractNo.Text;
                if (tractNumber != Server.HtmlEncode(tractNumber)) {
                    Common.CWarning wex = new Common.CWarning("Your Tract Number cannot contain '<', '>', or '&' characters.");
                    throw (wex);
                }
                string fieldNumber = txtFieldNo.Text;
                if (fieldNumber != Server.HtmlEncode(fieldNumber)) {
                    Common.CWarning wex = new Common.CWarning("Your Field Number cannot contain '<', '>', or '&' characters.");
                    throw (wex);
                }

                string acres = txtAcres.Text;
                if (acres.Length > 0) {
                    if (acres.Contains(".")) {
                        Common.CWarning wex = new Common.CWarning("Your must enter a whole number of acres.");
                        throw (wex);
                    }
                }

                string fsaNumber = txtFSANumber.Text;
                if (fsaNumber != Server.HtmlEncode(fsaNumber)) {
                    Common.CWarning wex = new Common.CWarning("Your FSA Number cannot contain '<', '>', or '&' characters.");
                    throw (wex);
                }

                bool editForceNewRecord = false;

                if (txtSaveToPriorYears.Text.Length > 0 && txtSaveToPriorYears.Text != "1") {
                    editForceNewRecord = true;
                }

                int lldID = 0;
                if (editForceNewRecord) {
                    lldID = -1;
                } else {
                    lldID = UsrCntSelector.LldID;
                }

                int cntlldOUT = UsrCntSelector.CntLLDID;
                WSCField.LegalLandDescSave(lldID,
                    state, county, township,
                    range, section,
                    quadrant, quarterQuadrant,
                    (latitude == "" ? 0 : Convert.ToDecimal(latitude)),
                    (longitude == "" ? 0 : Convert.ToDecimal(longitude)),
                    description, UsrCntSelector.FieldName,
                    (acres == "" ? 0 : Convert.ToInt32(acres)),
                    chkFSAOfficial.Checked, UsrCntSelector.FsaNumber, fsaState, fsaCounty,
                    farmNumber, tractNumber, fieldNumber, quarterField,
                    auth.UserName, ref lldIDOUT,
                    editForceNewRecord, UsrCntSelector.CntLLDID, UsrCntSelector.ContractID, UsrCntSelector.CropYear, ref cntlldOUT);

                if (UsrCntSelector.CntLLDID > 0) {

                    if (editForceNewRecord) {
                        UsrCntSelector.InitControl(UsrCntSelector.SHID, UsrCntSelector.CropYear, UsrCntSelector.ContractNumber, UsrCntSelector.SequenceNumber + 1);
                    } else {
                        UsrCntSelector.InitControl(UsrCntSelector.SHID, UsrCntSelector.CropYear, UsrCntSelector.ContractNumber, UsrCntSelector.SequenceNumber);
                    }
                }

                UsrCntSelector.LldID = lldIDOUT;
                UsrCntSelector.CntLLDID = cntlldOUT;

                ShowFieldDetail(UsrCntSelector.LldID);
                NewContractAlertOff();
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to Save this Field Information at this time.");
            }	
        }

        protected void btnDelete_Click(object sender, EventArgs e) {

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                WSCSecurity auth = Globals.SecurityState;
                if (!UsrCntSelector.IsOwner) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                    return;
                }

                // Delete this Field \ Land Description when it has a valid LLD_ID.
                // Otherwise just blow it off and do a little reset.

                int lld_id = UsrCntSelector.LldID;
                if (lld_id > 0) {
                    WSCField.LegalLandDescDelete(lld_id);
                } else {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You must be viewing a field already in the system in order to delete it.");
                    return;
                }

                int seq = 0;
                int seqMax = 0;
                if (UsrCntSelector.SequenceNumber > 0) {
                    seq = 1;
                    seqMax = UsrCntSelector.SequenceNumberMax - 1;
                }

                ClearFieldDetail();
                UsrCntSelector.InitControl(UsrCntSelector.SHID, UsrCntSelector.CropYear, UsrCntSelector.ContractNumber, seq);

            }
            catch (Exception ex) {
                ShowError(ex, @"Unable to Delete this Land Description \ Field at this time.");
            }				
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlState_SelectedIndexChanged";
            try {

                SelectState(ddlState.SelectedIndex);
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlFSAState_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlFSAState_SelectedIndexChanged";
            try {

                SelectFsaState(ddlFSAState.SelectedIndex);
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnSvrFindField_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSvrFindField_Click";
            try {

                if (txtLldID.Text.Length > 0) {

                    int lldID = Convert.ToInt32(txtLldID.Text);
                    txtLldID.Text = "";
                    int maxFields = UsrCntSelector.SequenceNumberMax;

                    UsrCntSelector.ResetField();

                    UsrCntSelector.SetSequenceNumber(maxFields, 0);
                    ShowFieldDetail(lldID);
                }
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
