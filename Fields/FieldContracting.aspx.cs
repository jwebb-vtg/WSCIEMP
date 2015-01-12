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

namespace WSCIEMP.Fields {

    public partial class FieldContracting : Common.BasePage//, IFieldDetails 
    {

        private const string MOD_NAME = "Fields.FieldContracting.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            UsrCntSelector.ContractNumberChange += new WSCField.ContractNumberChangeHandler(UsrCntSelector_ContractNumberChange);
            UsrCntSelector.ContractNumberFind += new WSCField.ContractNumberFindHandler(UsrCntSelector_ContractNumberFind);
            UsrCntSelector.ContractNumberNext += new WSCField.ContractNumberNextHandler(UsrCntSelector_ContractNumberNext);
            UsrCntSelector.ContractNumberPrev += new WSCField.ContractNumberPrevHandler(UsrCntSelector_ContractNumberPrev);
            UsrCntSelector.ShareholderFind += new WSCField.ShareholderFindHandler(UsrCntSelector_ShareholderFind);
            UsrCntSelector.SequenceNumberChange += new WSCField.SequenceNumberChangeHandler(UsrCntSelector_SequenceNumberChange);
            UsrCntSelector.ExceptionShow += new WSCField.FieldExceptionHandler(UsrCntSelector_FieldExceptionHandler);

            if (Globals.IsUserPermissionReadOnly((RolePrincipal)User)) {
                btnSaveAddress.Enabled = false;
                btnSaveContract.Enabled = false;
            }

            try {

                locPDF.Text = "";
                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                HtmlGenericControl body = (HtmlGenericControl)this.Master.FindControl("MasterBody");
                body.Attributes.Add("onload", "DoOnload();");

                if (!Page.IsPostBack) {
                    UsrCntSelector.ControlHostPageLoad += new WSCField.ControlHostPageLoadHandler(UsrCntSelector_ControlHostPageLoad);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void UsrCntSelector_ControlHostPageLoad(object sender, ContractSelectorEventArgs e) {

            if (e.Error() == null) {

                FillDomainData();
                PopEmail(UsrCntSelector.EmailAddress);
                ShowFieldDescription(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                ShowFieldDetail(UsrCntSelector.CntLLDID);

            } else {
                ShowError(e.Error(), "Unable to load page contract numbers at this time.");
            }
        }

        private void UsrCntSelector_ContractNumberChange(object sender, ContractSelectorEventArgs e) {

            try {

                if (e.Error() == null) {
                    ShowFieldDescription(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    ShowFieldDetail(UsrCntSelector.CntLLDID);
                    //CheckFields();
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
                    PopEmail(UsrCntSelector.EmailAddress);
                    ShowFieldDescription(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    ShowFieldDetail(UsrCntSelector.CntLLDID);
                    //CheckFields();
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
                    PopEmail(UsrCntSelector.EmailAddress);
                    ShowFieldDescription(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    ShowFieldDetail(UsrCntSelector.CntLLDID);
                    //CheckFields();
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
                    PopEmail(UsrCntSelector.EmailAddress);
                    ShowFieldDescription(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    ShowFieldDetail(UsrCntSelector.CntLLDID);
                    //CheckFields();
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
                    PopEmail(UsrCntSelector.EmailAddress);
                    ShowFieldDescription(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    ShowFieldDetail(UsrCntSelector.CntLLDID);

                    //CheckFields();
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
                    ShowFieldDescription(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    ShowFieldDetail(UsrCntSelector.CntLLDID);
                    //CheckFields();
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

        private void ShowFieldDetail(int cntlldID) {

            try {

                ClearFieldDeail();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCField.FieldGetDetailByCntLLD(conn, cntlldID)) {

                        if (dr.Read()) {

                            UsrCntSelector.CntLLDID = dr.GetInt32(dr.GetOrdinal("fld_cntlld_id"));
                            UsrCntSelector.FieldID = dr.GetInt32(dr.GetOrdinal("fld_field_id"));
                            UsrCntSelector.LldID = dr.GetInt32(dr.GetOrdinal("fld_lld_id"));

                            Common.UILib.SelectDropDown(ddlRotationLength, dr.GetString(dr.GetOrdinal("fld_rotation_len")));
                            Common.UILib.SelectDropDown(ddlPriorCrop, dr.GetString(dr.GetOrdinal("fld_prev_crop_type")));
                            Common.UILib.SelectDropDown(ddlBeetYears, dr.GetString(dr.GetOrdinal("fld_beet_years")));
                            chkRhizomania.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_rhizomania"));
                            chkAphanomyces.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_aphanomyces"));
                            chkCurlyTop.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_curly_top"));
                            chkFusarium.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_fusarium"));
                            chkRhizoctonia.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_rhizoctonia"));
                            chkNematode.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_nematode"));
                            chkCercospora.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_cercospora"));
                            chkRootAphid.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_root_aphid"));
                            chkPowderyMildew.Checked = dr.GetBoolean(dr.GetOrdinal("fld_suspect_powdery_mildew"));
                            Common.UILib.SelectDropDown(ddlWaterSource, dr.GetString(dr.GetOrdinal("fld_irrigation_source")));
                            Common.UILib.SelectDropDown(ddlIrrigation, dr.GetString(dr.GetOrdinal("fld_irrigation_method")));

                            Common.UILib.SelectDropDown(ddlPostAphanomyces, dr.GetString(dr.GetOrdinal("fld_post_Aphanomyces")));
                            Common.UILib.SelectDropDown(ddlPostCercospora, dr.GetString(dr.GetOrdinal("fld_post_Cercospora")));
                            Common.UILib.SelectDropDown(ddlPostCurlyTop, dr.GetString(dr.GetOrdinal("fld_post_CurlyTop")));
                            Common.UILib.SelectDropDown(ddlPostFusarium, dr.GetString(dr.GetOrdinal("fld_post_Fusarium")));
                            Common.UILib.SelectDropDown(ddlPostNematode, dr.GetString(dr.GetOrdinal("fld_post_Nematode")));
                            Common.UILib.SelectDropDown(ddlPostPowderyMildew, dr.GetString(dr.GetOrdinal("fld_post_PowderyMildew")));
                            Common.UILib.SelectDropDown(ddlPostRhizoctonia, dr.GetString(dr.GetOrdinal("fld_post_Rhizoctonia")));
                            Common.UILib.SelectDropDown(ddlPostRhizomania, dr.GetString(dr.GetOrdinal("fld_post_Rhizomania")));
                            Common.UILib.SelectDropDown(ddlPostRootAphid, dr.GetString(dr.GetOrdinal("fld_post_RootAphid")));

                            Common.UILib.SelectDropDown(ddlLandOwnership, dr.GetString(dr.GetOrdinal("fld_ownership")));
                            Common.UILib.SelectDropDown(ddlTillage, dr.GetString(dr.GetOrdinal("fld_tillage")));

                            chkPostWater.Checked = dr.GetBoolean(dr.GetOrdinal("fld_post_water"));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("FieldContracting.ShowFieldDetail", ex);
                throw (wex);
            }
        }

        private void ClearFieldDeail() {

            Common.UILib.SelectDropDown(ddlRotationLength, "");
            Common.UILib.SelectDropDown(ddlPriorCrop, "");
            Common.UILib.SelectDropDown(ddlBeetYears, "");
            Common.UILib.SelectDropDown(ddlWaterSource, "");
            Common.UILib.SelectDropDown(ddlIrrigation, "");

            chkRhizomania.Checked = false;
            chkAphanomyces.Checked = false;
            chkCurlyTop.Checked = false;
            chkFusarium.Checked = false;
            chkRhizoctonia.Checked = false;
            chkNematode.Checked = false;
            chkCercospora.Checked = false;
            chkRootAphid.Checked = false;
            chkPowderyMildew.Checked = false;

            Common.UILib.SelectDropDown(ddlPostAphanomyces, "");
            Common.UILib.SelectDropDown(ddlPostCercospora, "");
            Common.UILib.SelectDropDown(ddlPostCurlyTop, "");
            Common.UILib.SelectDropDown(ddlPostFusarium, "");
            Common.UILib.SelectDropDown(ddlPostNematode, "");
            Common.UILib.SelectDropDown(ddlPostPowderyMildew, "");
            Common.UILib.SelectDropDown(ddlPostRhizoctonia, "");
            Common.UILib.SelectDropDown(ddlPostRhizomania, "");
            Common.UILib.SelectDropDown(ddlPostRootAphid, "");

            chkPostWater.Checked = false;
        }

        private void ShowFieldDescription(int contractID, int sequenceNumber) {

            try {

                ClearFieldDescription();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCField.CntLldGetDetail(conn, UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber)) {

                        if (dr.Read()) {

                            UsrCntSelector.CntLLDID = dr.GetInt32(dr.GetOrdinal("cntlld_cntlld_id"));
                            UsrCntSelector.LldID = dr.GetInt32(dr.GetOrdinal("cntlld_lld_id"));
                            UsrCntSelector.FieldName = dr.GetString(dr.GetOrdinal("cntlld_field_name"));
                            
                            UsrCntSelector.FsaNumber = dr.GetString(dr.GetOrdinal("cntlld_fsa_number"));
                            UsrCntSelector.FieldState = dr.GetString(dr.GetOrdinal("cntlld_state"));

                            lblFieldName.Text = dr.GetString(dr.GetOrdinal("cntlld_field_name"));
                            lblFsaNumber.Text = dr.GetString(dr.GetOrdinal("cntlld_fsa_number"));
                            lblState.Text = dr.GetString(dr.GetOrdinal("cntlld_state"));
                            lblCounty.Text = dr.GetString(dr.GetOrdinal("cntlld_county"));
                            lblTownship.Text = dr.GetString(dr.GetOrdinal("cntlld_township"));
                            lblRange.Text = dr.GetString(dr.GetOrdinal("cntlld_range"));
                            lblSection.Text = dr.GetString(dr.GetOrdinal("cntlld_section"));
                            lblQuadrant.Text = dr.GetString(dr.GetOrdinal("cntlld_quadrant"));
                            lblQuarterQuadrant.Text = dr.GetString(dr.GetOrdinal("cntlld_quarter_quadrant"));
                            lblLongitude.Text = (dr.GetDecimal(dr.GetOrdinal("cntlld_latitude"))).ToString();
                            lblLatitude.Text = (dr.GetDecimal(dr.GetOrdinal("cntlld_longitude"))).ToString();
                            lblFsaOfficial.Text = (dr.GetBoolean(dr.GetOrdinal("cntlld_fsa_official")) ? "Yes" : "No");
                            lblAcres.Text = dr.GetInt32(dr.GetOrdinal("cntlld_acres")).ToString();
                            lblDescription.Text = dr.GetString(dr.GetOrdinal("cntlld_description"));

                            lblFsaState.Text = dr.GetString(dr.GetOrdinal("cntlld_fsa_state"));
                            lblFsaCounty.Text = dr.GetString(dr.GetOrdinal("cntlld_fsa_county"));
                            lblFarmNo.Text = dr.GetString(dr.GetOrdinal("cntlld_farm_number"));
                            lblTractNo.Text = dr.GetString(dr.GetOrdinal("cntlld_tract_number"));
                            lblFieldNo.Text = dr.GetString(dr.GetOrdinal("cntlld_field_number"));
                            lblQuarterField.Text = dr.GetString(dr.GetOrdinal("cntlld_quarter_field"));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("FieldContracting.ShowFieldDescription", ex);
                throw (wex);
            }
        }

        private void ClearFieldDescription() {

            lblFieldName.Text = "";
            lblFsaNumber.Text = "";
            lblState.Text = "";
            lblCounty.Text = "";
            lblTownship.Text = "";
            lblRange.Text = "";
            lblSection.Text = "";
            lblQuadrant.Text = "";
            lblQuarterQuadrant.Text = "";
            lblLongitude.Text = "";
            lblLatitude.Text = "";
            lblFsaOfficial.Text = "No";
            lblAcres.Text = "";
            lblDescription.Text = "";

            lblFsaState.Text = "";
            lblFsaCounty.Text = "";
            lblFarmNo.Text = "";
            lblTractNo.Text = "";
            lblFieldNo.Text = "";
            lblQuarterField.Text = "";
        }

        private void FillDomainData() {

            Domain domain = WSCField.GetDomainData();

            ddlIrrigation.Items.Clear();
            ddlPriorCrop.Items.Clear();
            ddlRotationLength.Items.Clear();
            ddlWaterSource.Items.Clear();
            ddlBeetYears.Items.Clear();

            // Beet Years
            foreach (TItem item in domain.FieldBeetYearsList.Items) {
                ddlBeetYears.Items.Add(item.Name);
            }
            ddlBeetYears.SelectedIndex = 0;

            // Irrigation
            foreach (TItem item in domain.FieldIrrigationSystemList.Items) {
                ddlIrrigation.Items.Add(item.Name);
            }
            ddlIrrigation.SelectedIndex = 0;

            // Prior Crop
            foreach (TItem item in domain.FieldPriorCropList.Items) {
                ddlPriorCrop.Items.Add(item.Name);
            }
            ddlPriorCrop.SelectedIndex = 0;

            // Rotation Length
            foreach (TItem item in domain.FieldRotationLengthList.Items) {
                ddlRotationLength.Items.Add(item.Name);
            }
            ddlRotationLength.SelectedIndex = 0;

            // Land Ownership
            foreach (TItem item in domain.LandOwnershipList.Items) {
                ddlLandOwnership.Items.Add(item.Name);
            }
            ddlLandOwnership.SelectedIndex = 0;

            // Tillage
            foreach (TItem item in domain.TillageTypeList.Items) {
                ddlTillage.Items.Add(item.Name);
            }
            ddlTillage.SelectedIndex = 0;

            // Post Rhizomania
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostRhizomania.Items.Add(item.Name);
            }
            ddlPostRhizomania.SelectedIndex = 0;

            // Post Aphanomyces
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostAphanomyces.Items.Add(item.Name);
            }
            ddlPostAphanomyces.SelectedIndex = 0;

            // Post CurlyTop
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostCurlyTop.Items.Add(item.Name);
            }
            ddlPostCurlyTop.SelectedIndex = 0;

            // Post Fusarium
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostFusarium.Items.Add(item.Name);
            }
            ddlPostFusarium.SelectedIndex = 0;

            // Post Rhizoctonia
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostRhizoctonia.Items.Add(item.Name);
            }
            ddlPostRhizoctonia.SelectedIndex = 0;

            // Post Nematode
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostNematode.Items.Add(item.Name);
            }
            ddlPostNematode.SelectedIndex = 0;

            // Post Cercospora
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostCercospora.Items.Add(item.Name);
            }
            ddlPostCercospora.SelectedIndex = 0;

            // Post RootAphid
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostRootAphid.Items.Add(item.Name);
            }
            ddlPostRootAphid.SelectedIndex = 0;

            // Post Powdery Mildew
            foreach (TItem item in domain.DiseaseSeverityList.Items) {
                ddlPostPowderyMildew.Items.Add(item.Name);
            }
            ddlPostPowderyMildew.SelectedIndex = 0;

            // Water Source
            foreach (TItem item in domain.FieldWaterSourceList.Items) {
                ddlWaterSource.Items.Add(item.Name);
            }
            ddlWaterSource.SelectedIndex = 0;
        }

        private void PopEmail(string email) {
            txtEmail.Text = email;
        }

        private void ShowError(Exception ex, string userMessage) {
			((PrimaryTemplate)Page.Master).ShowWarning(ex, userMessage);
        }

        protected void btnPrintForm_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnPrintForm_Click";

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                // Give client the url to open the pdf
                string filePath = "";
                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + "Field Contracting".Replace(" ", "");

                filePath = WSCReports.rptFieldContracting.ReportPackager(UsrCntSelector.CropYear, "", "", "", UsrCntSelector.FieldID.ToString(), auth.UserID, fileName, logoUrl, pdfTempFolder);

                if (filePath.Length > 0) {
                    // convert file system path to virtual path
                    filePath = filePath.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                }

                locPDF.Text = filePath; 

            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnSaveContract_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnSaveContract_Click";

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                if (UsrCntSelector.FieldID == 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "  You cannot Save unless you are viewing a contracted field.");
                    return;
                }

                WSCSecurity auth = Globals.SecurityState;
                if (!UsrCntSelector.IsOwner) {

                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                    return;
                }

                WSCField.FieldContractingSave(UsrCntSelector.FieldID,
                    Common.UILib.GetDropDownText(ddlRotationLength),
                    Common.UILib.GetDropDownText(ddlPriorCrop), Common.UILib.GetDropDownText(ddlBeetYears),
                    chkRhizomania.Checked, chkAphanomyces.Checked, chkCurlyTop.Checked,
                    chkFusarium.Checked, chkRhizoctonia.Checked, chkNematode.Checked,
                    chkCercospora.Checked, chkRootAphid.Checked, chkPowderyMildew.Checked,
                    Common.UILib.GetDropDownText(ddlPostAphanomyces), Common.UILib.GetDropDownText(ddlPostCercospora), Common.UILib.GetDropDownText(ddlPostCurlyTop),
                    Common.UILib.GetDropDownText(ddlPostFusarium), Common.UILib.GetDropDownText(ddlPostNematode), Common.UILib.GetDropDownText(ddlPostPowderyMildew),
                    Common.UILib.GetDropDownText(ddlPostRhizoctonia), Common.UILib.GetDropDownText(ddlPostRhizomania), Common.UILib.GetDropDownText(ddlPostRootAphid),
                    chkPostWater.Checked, Common.UILib.GetDropDownText(ddlWaterSource), Common.UILib.GetDropDownText(ddlIrrigation),
                    Common.UILib.GetDropDownText(ddlLandOwnership), Common.UILib.GetDropDownText(ddlTillage), auth.UserName);

                ShowFieldDetail(UsrCntSelector.CntLLDID);
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnSaveAddress_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnSaveAddress_Click";

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

                string email = txtEmail.Text;

                if (!Common.CodeLib.ValidateEmail(email)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter a valid email address.");
                    return;
                }

                WSCMember.UpdateAddress(UsrCntSelector.MemberID, email, UsrCntSelector.FaxNumber, Globals.SecurityState.UserName);
                UsrCntSelector.EmailAddress = email;


            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        //#region IFieldDetails Members

        //public void GeneralExceptionShow(object sender, WSCErrorEventArgs e) {
        //    ShowError(e.Error(), "");
        //}

        //#endregion
    }
}
