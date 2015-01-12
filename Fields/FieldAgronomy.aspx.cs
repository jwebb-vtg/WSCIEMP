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

    public partial class FieldAgronomy : Common.BasePage {

        private const string MOD_NAME = "Fields.FieldAgronomy.";
        private const string NO_VALUE = "None";

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
                btnSave.Enabled = false;
            }

            try {
                
                locPDF.Text = "";
                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                HtmlGenericControl body = (HtmlGenericControl)this.Master.FindControl("MasterBody");
                body.Attributes.Add("onload", "DoOnLoad();");               

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
                FillFieldHdr(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                FillAgronomyData();
                ShowFieldAgronomy(UsrCntSelector.CntLLDID);
            } else {
                ShowError(e.Error(), "Unable to load page contract numbers at this time.");
            }
        }

        private void UsrCntSelector_ContractNumberChange(object sender, ContractSelectorEventArgs e) {

            try {

                if (e.Error() == null) {
                    FillFieldHdr(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    FillAgronomyData();
                    ShowFieldAgronomy(UsrCntSelector.CntLLDID);
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
                    FillFieldHdr(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    FillAgronomyData();
                    ShowFieldAgronomy(UsrCntSelector.CntLLDID);
                } else {

                    // Clear field details.
                    lblFieldName.Text = "";
                    lblFsaNumber.Text = "";
                    lblAcres.Text = "";
                    lblDesc.Text = "";

                    ClearFieldAgronomy();
                    ShowError(e.Error(), "Unable to find contract number at this time.");
                }
            }
            catch (Exception ex) {
                ShowError(ex, "Unable to find contract number at this time.");
            }
        }

        private void UsrCntSelector_ContractNumberPrev(object sender, ContractSelectorEventArgs e) {

            try {

                if (e.Error() == null) {
                    FillFieldHdr(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    FillAgronomyData();
                    ShowFieldAgronomy(UsrCntSelector.CntLLDID);
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
                    FillFieldHdr(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    FillAgronomyData();
                    ShowFieldAgronomy(UsrCntSelector.CntLLDID);
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
                    FillFieldHdr(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    FillAgronomyData();
                    ShowFieldAgronomy(UsrCntSelector.CntLLDID);
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
                    FillFieldHdr(UsrCntSelector.ContractID, UsrCntSelector.SequenceNumber);
                    FillAgronomyData();
                    ShowFieldAgronomy(UsrCntSelector.CntLLDID);
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

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }
                if (UsrCntSelector.FieldID == 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), " You cannot Save unless you are viewing a contracted field.");
                    return;
                }

                if (!UsrCntSelector.IsOwner) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                    return;
                }

                string dateFormat = Common.CodeLib.DateEntryFormat().ToUpper();

                string fld_planting_date = txtPlantingDate.Text;
                if (!Common.CodeLib.ValidateMMDDDate(ref fld_planting_date, UsrCntSelector.CropYear)) {
                    Common.CWarning warn = new Common.CWarning("Planting Date is not valid.  Please enter all dates in MM/DD format.");
                    throw (warn);
                }

                string fld_emerg_80_date = txt80EmergDate.Text;
                if (!Common.CodeLib.ValidateMMDDDate(ref fld_emerg_80_date, UsrCntSelector.CropYear)) {
                    Common.CWarning warn = new Common.CWarning("80% Emerg Date is not valid.  Please enter all dates in MM/DD format.");
                    throw (warn);
                }

                string fld_replant_date = txtReplantingDate.Text;
                if (!Common.CodeLib.ValidateMMDDDate(ref fld_replant_date, UsrCntSelector.CropYear)) {
                    Common.CWarning warn = new Common.CWarning("Replant Date Date is not valid.  Please enter all dates in MM/DD format.");
                    throw (warn);
                }

                string fld_cercospora_app1_date = txtApp1Date.Text;
                if (!Common.CodeLib.ValidateMMDDDate(ref fld_cercospora_app1_date, UsrCntSelector.CropYear)) {
                    Common.CWarning warn = new Common.CWarning("Application 1 Date is not valid.  Please enter all dates in MM/DD format.");
                    throw (warn);
                }

                string fld_cercospora_app2_date = txtApp2Date.Text;
                if (!Common.CodeLib.ValidateMMDDDate(ref fld_cercospora_app2_date, UsrCntSelector.CropYear)) {
                    Common.CWarning warn = new Common.CWarning("Application 2 Date is not valid.  Please enter all dates in MM/DD format.");
                    throw (warn);
                }

                string fld_cercospora_app3_date = txtApp3Date.Text;
                if (!Common.CodeLib.ValidateMMDDDate(ref fld_cercospora_app3_date, UsrCntSelector.CropYear)) {
                    Common.CWarning warn = new Common.CWarning("Application 3 Date is not valid.  Please enter all dates in MM/DD format.");
                    throw (warn);
                }

                string fld_comment = txtComment.Text;
                if (fld_comment.Length > 200) {
                    Common.CWarning warn = new Common.CWarning("Field Comment is longer than 200 characters.  Please shorten the length of your comment.");
                    throw (warn);
                }

                // is numeric replant acres
                string fld_acres_replanted = txtReplantAcres.Text.Trim().Replace(",", "");
                if (fld_acres_replanted.Length > 0) {
                    try { fld_acres_replanted = Int32.Parse(fld_acres_replanted).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Replanted Acres is not a valid number."); return; }
                }

                // is numeric lost acres
                string fld_acres_lost = txtLostAcres.Text.Trim().Replace(",", "");
                if (fld_acres_lost.Length > 0) {
                    try { fld_acres_lost = Int32.Parse(fld_acres_lost).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Lost Acres is not a valid number."); return; }
                }

                string fld_test_N = txtTestedN.Text.Trim().Replace(",", "");
                if (fld_test_N.Length > 0) {
                    try { fld_test_N = Decimal.Parse(fld_test_N).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Nitrogen test result is not a valid number."); return; }
                }

                string fld_test_K = txtTestedK.Text.Trim().Replace(",", "");
                if (fld_test_K.Length > 0) {
                    try { fld_test_K = Decimal.Parse(fld_test_K).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Potasium test result is not a valid number."); return; }
                }

                string fld_test_salts = txtTestedSalts.Text.Trim().Replace(",", "");
                if (fld_test_salts.Length > 0) {
                    try { fld_test_salts = Decimal.Parse(fld_test_salts).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The salts test result is not a valid number."); return; }
                }

                string fld_test_pH = txtTestedpH.Text.Trim().Replace(",", "");
                if (fld_test_pH.Length > 0) {
                    try { fld_test_pH = Decimal.Parse(fld_test_pH).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The pH test result is not a valid number."); return; }
                }

                string fld_test_org_mat = txtTestedOm.Text.Trim().Replace(",", "");
                if (fld_test_org_mat.Length > 0) {
                    try { fld_test_org_mat = Decimal.Parse(fld_test_org_mat).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Organic Material test result is not a valid number."); return; }
                }

                string fld_fert_fal_N = txtFertFallN.Text.Trim().Replace(",", "");
                if (fld_fert_fal_N.Length > 0) {
                    try { fld_fert_fal_N = Decimal.Parse(fld_fert_fal_N).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Fall Fertilizer Nitrogen is not a valid number."); return; }
                }

                string fld_fert_fal_P = txtFertFallP.Text.Trim().Replace(",", "");
                if (fld_fert_fal_P.Length > 0) {
                    try { fld_fert_fal_P = Decimal.Parse(fld_fert_fal_P).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Fall Fertilizer Phosphorus is not a valid number."); return; }
                }

                string fld_fert_fal_K = txtFertFallK.Text.Trim().Replace(",", "");
                if (fld_fert_fal_K.Length > 0) {
                    try { fld_fert_fal_K = Decimal.Parse(fld_fert_fal_K).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Fall Fertilizer Potasium is not a valid number."); return; }
                }

                string fld_fert_spr_N = txtFertSpringN.Text.Trim().Replace(",", "");
                if (fld_fert_spr_N.Length > 0) {
                    try { fld_fert_spr_N = Decimal.Parse(fld_fert_spr_N).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Spring Fertilizer Nitrogen is not a valid number."); return; }
                }

                string fld_fert_spr_P = txtFertSpringP.Text.Trim().Replace(",", "");
                if (fld_fert_spr_P.Length > 0) {
                    try { fld_fert_spr_P = Decimal.Parse(fld_fert_spr_P).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Spring Fertilizer Phosphorus is not a valid number."); return; }
                }

                string fld_fert_spr_K = txtFertSpringK.Text.Trim().Replace(",", "");
                if (fld_fert_spr_K.Length > 0) {
                    try { fld_fert_spr_K = Decimal.Parse(fld_fert_spr_K).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Spring Fertilizer Potasium is not a valid number."); return; }
                }

                string fld_fert_ins_N = txtFertSeasonN.Text.Trim().Replace(",", "");
                if (fld_fert_ins_N.Length > 0) {
                    try { fld_fert_ins_N = Decimal.Parse(fld_fert_ins_N).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The In Season Fertilizer Nitrogen is not a valid number."); return; }
                }

                string fld_fert_ins_P = txtFertSeasonP.Text.Trim().Replace(",", "");
                if (fld_fert_ins_P.Length > 0) {
                    try { fld_fert_ins_P = Decimal.Parse(fld_fert_ins_P).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The In Season Fertilizer Phosphorus is not a valid number."); return; }
                }

                string fld_fert_ins_K = txtFertSeasonK.Text.Trim().Replace(",", "");
                if (fld_fert_ins_K.Length > 0) {
                    try { fld_fert_ins_K = Decimal.Parse(fld_fert_ins_K).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The In Season Fertilizer Potasium is not a valid number."); return; }
                }

                string fld_rootm_counter_lbs = txtChemCounter.Text.Trim().Replace(",", "");
                if (fld_rootm_counter_lbs.Length > 0) {
                    try { fld_rootm_counter_lbs = Decimal.Parse(fld_rootm_counter_lbs).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Counter 15G value is not a valid number."); return; }
                }

                string fld_rootm_temik_lbs = txtChemTemik.Text.Trim().Replace(",", "");
                if (fld_rootm_temik_lbs.Length > 0) {
                    try { fld_rootm_temik_lbs = Decimal.Parse(fld_rootm_temik_lbs).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Temik value is not a valid number."); return; }
                }

                string fld_rootm_thimet_lbs = txtChemThimet.Text.Trim().Replace(",", "");
                if (fld_rootm_thimet_lbs.Length > 0) {
                    try { fld_rootm_thimet_lbs = Decimal.Parse(fld_rootm_thimet_lbs).ToString(); }
                    catch { Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The Thimet value is not a valid number."); return; }
                }

                int fld_field_id_OUT = 0;
                int fld_field_id = UsrCntSelector.FieldID;

                string fld_seed_variety = Common.UILib.GetDropDownText(ddlVariety);
                string fld_seed_primed = Common.UILib.GetDropDownText(ddlSeed);
                string fld_seed_treatment_chemical = GetSeedTreatment();
                string fld_row_spacing = Common.UILib.GetDropDownText(ddlRowSpacing);
                string fld_plant_spacing = Common.UILib.GetDropDownText(ddlPlantSpacing);
                string fld_replant_seed_variety = Common.UILib.GetDropDownText(ddlReplantVariety);

                string fld_soil_texture = Common.UILib.GetDropDownText(ddlSoilTexture);
                string fld_test_season = Common.UILib.GetDropDownText(ddlSoilTest);
                string fld_test_depth = Common.UILib.GetDropDownText(ddlSampleDepth);
                string fld_last_yr_manure = Common.UILib.GetDropDownText(ddlManureYear);
                string fld_fert_starter = Common.UILib.GetDropDownText(ddlFertStarter);
                string fld_pre_insecticide = Common.UILib.GetDropDownText(ddlPreInsecticide);
                string fld_post_insectcide = Common.UILib.GetDropDownText(ddlPostInsecticide);
                string fld_pre_weed_ctrl = Common.UILib.GetDropDownText(ddlPreWeedControl);

                string fld_herbicide_rx_count = Common.UILib.GetDropDownText(ddlHerbicideRxCount);
                string fld_layby_herbicide = Common.UILib.GetDropDownText(ddlIsLaybyHerbicide);
                string fld_layby_herbicide_chemical = Common.UILib.GetDropDownText(ddlLaybyHerbicide);
                string fld_root_maggot_insecticide = Common.UILib.GetDropDownText(ddlRootMaggot);
                string fld_cercsp_app1_chemical = Common.UILib.GetDropDownText(ddlApp1Chem);
                string fld_cercsp_app2_chemical = Common.UILib.GetDropDownText(ddlApp2Chem);
                string fld_cercsp_app3_chemical = Common.UILib.GetDropDownText(ddlApp3Chem);
                string fld_hail_stress = Common.UILib.GetDropDownText(ddlHailStress);
                string fld_weed_control = Common.UILib.GetDropDownText(ddlWeedControl);
                string fld_replant_reason = Common.UILib.GetDropDownText(ddlReplantReason);
                string fld_lost_reason = Common.UILib.GetDropDownText(ddlLostReason);
                string fld_treated_powdery_mildew = Common.UILib.GetDropDownText(ddlTreatedPowderyMildew);
                string fld_treated_nematode = Common.UILib.GetDropDownText(ddlTreatedNematode);
                string fld_treated_rhizoctonia = Common.UILib.GetDropDownText(ddlTreatedRhizoctonia);
                string fld_reviewed = Common.UILib.GetDropDownText(ddlReviewed);
                string fld_grid_zone = Common.UILib.GetDropDownText(ddlSampleGridZone);
                string fld_include = Common.UILib.GetDropDownText(ddlIncludeData);
                string fld_test_P = Common.UILib.GetDropDownText(ddlTestedP);
                string fld_add_user = UsrCntSelector.SHID.ToString();

                WSCField.FieldAgronomySave(ref fld_field_id_OUT,
                    fld_field_id,
                    fld_seed_variety, fld_seed_primed,
                    fld_seed_treatment_chemical, fld_row_spacing, fld_planting_date,
                    fld_plant_spacing, fld_replant_date, fld_replant_seed_variety,
                    fld_acres_replanted, fld_acres_lost, fld_replant_reason, fld_lost_reason,
                    fld_test_season, fld_test_depth,
                    fld_test_N, fld_test_P, fld_test_K,
                    fld_test_pH, fld_test_org_mat, fld_last_yr_manure,
                    fld_fert_fal_N, fld_fert_fal_P, fld_fert_fal_K,
                    fld_fert_spr_N, fld_fert_spr_P, fld_fert_spr_K,
                    fld_fert_ins_N, fld_fert_ins_P, fld_fert_ins_K,
                    fld_fert_starter, fld_pre_insecticide, fld_post_insectcide,
                    fld_pre_weed_ctrl, fld_layby_herbicide, fld_layby_herbicide_chemical,
                    fld_root_maggot_insecticide, fld_rootm_counter_lbs, fld_rootm_temik_lbs,
                    fld_rootm_thimet_lbs, fld_cercsp_app1_chemical, fld_cercospora_app1_date,
                    fld_cercsp_app2_chemical, fld_cercospora_app2_date, fld_cercsp_app3_chemical,
                    fld_cercospora_app3_date, fld_hail_stress, fld_weed_control,
                    fld_treated_powdery_mildew, fld_treated_nematode, fld_treated_rhizoctonia,
                    fld_reviewed, fld_grid_zone, fld_include, fld_add_user,
                    fld_soil_texture, fld_test_salts, fld_herbicide_rx_count, fld_emerg_80_date,
                    fld_comment);
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnPrint_Click";

            try {

                if (UsrCntSelector.IsChangedSHID) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You changed the SHID without pressing the Find button.  Please press Find.");
                    return;
                }

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + "Field Agronomy".Replace(" ", "");

                string locpdf = WSCReports.rptFieldAgronomy.ReportPackager(UsrCntSelector.CropYear, "", "", "",
                    UsrCntSelector.FieldID.ToString(), auth.UserID, fileName, logoUrl, pdfTempFolder);

                if (locpdf.Length > 0) {
                    // convert file system path to virtual path
                    locPDF.Text = locpdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                }

            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void ShowError(Exception ex, string userMessage) {
			((PrimaryTemplate)Page.Master).ShowWarning(ex, userMessage);
        }

        private void SelectSeedTreatment(string treatment) {

            chkSeedRxCruiser.Checked = false;
            chkSeedRxPoncho.Checked = false;
            chkSeedRxTachigaren.Checked = false;

            string[] rx = treatment.Trim().Split(new char[] { ' ' });
            foreach (string s in rx) {
                switch (s) {
                    case "Cruiser":
                        chkSeedRxCruiser.Checked = true;
                        break;
                    case "Poncho":
                        chkSeedRxPoncho.Checked = true;
                        break;
                    case "Tachigaren":
                        chkSeedRxTachigaren.Checked = true;
                        break;
                }
            }
        }

        private string GetSeedTreatment() {

            string rx = (chkSeedRxCruiser.Checked ? "Cruiser " : "") + (chkSeedRxPoncho.Checked ? "Poncho " : "") + (chkSeedRxTachigaren.Checked ? "Tachigaren " : "");
            if (rx.Length > 0) {
                return rx.Substring(0, rx.Length - 1);
            } else {
                return rx;
            }
        }

        private void InitBoolDropDown(System.Web.UI.WebControls.DropDownList ddl) {

            ddl.Items.Clear();
            ddl.Items.Add("No");
            ddl.Items.Add("Yes");
        }

        private void FillAgronomyData() {

            try {

                RptAgronomy agronomy = WSCField.GetAgronomyData();

                // Seed Variety 
                ddlVariety.Items.Clear();
                string factoryNumber = UsrCntSelector.FieldFactoryNumber.ToString();
                string varietyType = "";

                // Given the factory number look up whether this is a north or south variety.
                foreach (TItem2 item in agronomy.FactoryVarietyList.Items) {
                    if (item.Field1 == factoryNumber) {
                        varietyType = item.Field2;
                        break;
                    }
                }

                // With a factory number match on variety type to acquire the variety name.				
                this.ddlVariety.Items.Add(NO_VALUE);
                foreach (TItem2 item in agronomy.VarietyList.Items) {
                    if (item.Field1 == varietyType) {
                        ddlVariety.Items.Add(item.Field2);
                    }
                }

                ddlSeed.Items.Clear();
                this.ddlSeed.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.SeedList.Items) {
                    ddlSeed.Items.Add(item.Name);
                }

                ddlRowSpacing.Items.Clear();
                foreach (TItem item in agronomy.RowSpacingList.Items) {
                    ddlRowSpacing.Items.Add(item.Name);
                }

                ddlPlantSpacing.Items.Clear();
                foreach (TItem item in agronomy.PlantSpacingList.Items) {
                    ddlPlantSpacing.Items.Add(item.Name);
                }

                ddlReplantVariety.Items.Clear();
                ddlReplantVariety.Items.Add(NO_VALUE);
                foreach (TItem2 item in agronomy.VarietyList.Items) {
                    if (item.Field1 == varietyType) {
                        ddlReplantVariety.Items.Add(item.Field2);
                    }
                }

                ddlSoilTexture.Items.Clear();
                ddlSoilTexture.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.SoilTextureList.Items) {
                    ddlSoilTexture.Items.Add(item.Name);
                }

                ddlSoilTest.Items.Clear();
                ddlSoilTest.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.SoilTestList.Items) {
                    ddlSoilTest.Items.Add(item.Name);
                }

                ddlSampleDepth.Items.Clear();
                foreach (TItem item in agronomy.SampleDepthList.Items) {
                    ddlSampleDepth.Items.Add(item.Name);
                }

                ddlTestedP.Items.Clear();
                ddlTestedP.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.SamplePList.Items) {
                    ddlTestedP.Items.Add(item.Name);
                }

                ddlManureYear.Items.Clear();
                int cropYear = UsrCntSelector.CropYear;
                ddlManureYear.Items.Add(NO_VALUE);
                for (int i = 0; i < 7; i++) {
                    ddlManureYear.Items.Add((cropYear - i).ToString());
                }

                ddlLaybyHerbicide.Items.Clear();
                ddlLaybyHerbicide.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.LaybyHerbicideList.Items) {
                    ddlLaybyHerbicide.Items.Add(item.Name);
                }

                ddlApp1Chem.Items.Clear();
                ddlApp1Chem.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.CercosporaChemList.Items) {
                    ddlApp1Chem.Items.Add(item.Name);
                }

                ddlApp2Chem.Items.Clear();
                ddlApp2Chem.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.CercosporaChemList.Items) {
                    ddlApp2Chem.Items.Add(item.Name);
                }

                ddlApp3Chem.Items.Clear();
                ddlApp3Chem.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.CercosporaChemList.Items) {
                    ddlApp3Chem.Items.Add(item.Name);
                }

                ddlHailStress.Items.Clear();
                ddlHailStress.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.HailStressList.Items) {
                    ddlHailStress.Items.Add(item.Name);
                }

                ddlWeedControl.Items.Clear();
                ddlWeedControl.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.WeedControlList.Items) {
                    ddlWeedControl.Items.Add(item.Name);
                }

                ddlReplantReason.Items.Clear();
                ddlReplantReason.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.ReplantReasonList.Items) {
                    ddlReplantReason.Items.Add(item.Name);
                }

                ddlLostReason.Items.Clear();
                ddlLostReason.Items.Add(NO_VALUE);
                foreach (TItem item in agronomy.LostReasonList.Items) {
                    ddlLostReason.Items.Add(item.Name);
                }

                ddlHerbicideRxCount.Items.Clear();
                foreach (TItem item in agronomy.HerbicideRxCountList.Items) {
                    ddlHerbicideRxCount.Items.Add(item.Name);
                }

                // Fill all Boolean drop downs
                InitBoolDropDown(ddlReviewed);
                InitBoolDropDown(ddlSampleGridZone);
                InitBoolDropDown(ddlFertStarter);
                InitBoolDropDown(ddlPreInsecticide);
                InitBoolDropDown(ddlPostInsecticide);
                InitBoolDropDown(ddlPreWeedControl);
                InitBoolDropDown(ddlIsLaybyHerbicide);
                InitBoolDropDown(ddlRootMaggot);
                InitBoolDropDown(ddlTreatedPowderyMildew);
                InitBoolDropDown(ddlTreatedNematode);
                InitBoolDropDown(ddlTreatedRhizoctonia);
                InitBoolDropDown(ddlIncludeData);

            }
            catch (Exception ex) {
                Common.CException wscex = new Common.CException("FieldAgronomy.FillAgronomyData", ex);
                throw (wscex);
            }
        }

        private void FillFieldHdr(int contractID, int seqNumber) {

            const string METHOD_NAME = "FillFieldHdr";

            try {

                lblFieldName.Text = "";
                lblFsaNumber.Text = "";
                lblAcres.Text = "";
                lblDesc.Text = "";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.CntLldGetDetail(conn, contractID, seqNumber)) {

                        if (dr.Read()) {

                            UsrCntSelector.CntLLDID = dr.GetInt32(dr.GetOrdinal("cntlld_cntlld_id"));
                            UsrCntSelector.LldID = dr.GetInt32(dr.GetOrdinal("cntlld_lld_id"));
                            UsrCntSelector.FieldFactoryNumber = dr.GetInt16(dr.GetOrdinal("cntlld_factory_no"));

                            string sFieldName = dr.GetString(dr.GetOrdinal("cntlld_field_name"));
                            UsrCntSelector.FieldName = sFieldName;
                            string sFsaNumber = dr.GetString(dr.GetOrdinal("cntlld_fsa_number"));
                            UsrCntSelector.FsaNumber = sFsaNumber;
                            UsrCntSelector.FieldState = dr.GetString(dr.GetOrdinal("cntlld_state"));

                            lblFieldName.Text = sFieldName;
                            lblFsaNumber.Text = sFsaNumber;
                            lblAcres.Text = (dr.GetInt32(dr.GetOrdinal("cntlld_acres"))).ToString();
                            lblDesc.Text = dr.GetString(dr.GetOrdinal("cntlld_description"));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wscex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wscex);
            }
        }

        private void ClearFieldAgronomy() {

            Common.UILib.SelectDropDown(ddlVariety, 0);
            Common.UILib.SelectDropDown(ddlSeed, 0);
            SelectSeedTreatment("");
            Common.UILib.SelectDropDown(ddlRowSpacing, 0);
            Common.UILib.SelectDropDown(ddlPlantSpacing, 0);
            Common.UILib.SelectDropDown(ddlReplantVariety, 0);
            Common.UILib.SelectDropDown(ddlSoilTexture, 0);
            Common.UILib.SelectDropDown(ddlSoilTest, 0);
            Common.UILib.SelectDropDown(ddlSampleDepth, 0);
            Common.UILib.SelectDropDown(ddlManureYear, 0);
            Common.UILib.SelectDropDown(ddlFertStarter, 0);
            Common.UILib.SelectDropDown(ddlPreInsecticide, 0);
            Common.UILib.SelectDropDown(ddlPostInsecticide, 0);
            Common.UILib.SelectDropDown(ddlPreWeedControl, 0);
            Common.UILib.SelectDropDown(ddlHerbicideRxCount, 0);
            Common.UILib.SelectDropDown(ddlIsLaybyHerbicide, 0);
            Common.UILib.SelectDropDown(ddlLaybyHerbicide, 0);
            Common.UILib.SelectDropDown(ddlRootMaggot, 0);
            Common.UILib.SelectDropDown(ddlApp1Chem, 0);

            Common.UILib.SelectDropDown(ddlApp2Chem, 0);
            Common.UILib.SelectDropDown(ddlHailStress, 0);
            Common.UILib.SelectDropDown(ddlWeedControl, 0);
            Common.UILib.SelectDropDown(ddlReplantReason, 0);
            Common.UILib.SelectDropDown(ddlLostReason, 0);
            Common.UILib.SelectDropDown(ddlTreatedPowderyMildew, 0);
            Common.UILib.SelectDropDown(ddlTreatedNematode, 0);
            Common.UILib.SelectDropDown(ddlTreatedRhizoctonia, 0);
            Common.UILib.SelectDropDown(ddlReviewed, 1);
            Common.UILib.SelectDropDown(ddlSampleGridZone, 0);
            Common.UILib.SelectDropDown(ddlIncludeData, 1);
            Common.UILib.SelectDropDown(ddlApp3Chem, 0);
            Common.UILib.SelectDropDown(ddlTestedP, 0);

            txtPlantingDate.Text = "";
            txt80EmergDate.Text = "";
            txtReplantingDate.Text = "";
            txtReplantAcres.Text = "";
            txtLostAcres.Text = "";
            txtTestedN.Text = "";
            txtTestedK.Text = "";
            txtTestedSalts.Text = "";
            txtTestedpH.Text = "";
            txtTestedOm.Text = "";
            txtFertFallN.Text = "";
            txtFertFallP.Text = "";
            txtFertFallK.Text = "";
            txtFertSpringN.Text = "";
            txtFertSpringP.Text = "";
            txtFertSpringK.Text = "";
            txtFertSeasonN.Text = "";
            txtFertSeasonP.Text = "";
            txtFertSeasonK.Text = "";

            txtChemCounter.Text = "";
            txtChemTemik.Text = "";
            txtChemThimet.Text = "";
            txtApp1Date.Text = "";
            txtApp2Date.Text = "";
            txtApp3Date.Text = "";

            txtComment.Text = "";
        }

        private void ShowFieldAgronomy(int cntlldID) {

            try {

                ClearFieldAgronomy();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.FieldGetDetailByCntLLD(conn, cntlldID)) {

                        if (dr.Read()) {

                            UsrCntSelector.CntLLDID = dr.GetInt32(dr.GetOrdinal("fld_cntlld_id"));
                            UsrCntSelector.FieldID = dr.GetInt32(dr.GetOrdinal("fld_field_id"));
                            UsrCntSelector.LldID = dr.GetInt32(dr.GetOrdinal("fld_lld_id"));

                            Common.UILib.SelectDropDown(ddlVariety, dr.GetString(dr.GetOrdinal("fld_seed_variety")));
                            Common.UILib.SelectDropDown(ddlSeed, dr.GetString(dr.GetOrdinal("fld_seed_primed")));
                            SelectSeedTreatment(dr.GetString(dr.GetOrdinal("fld_seed_treatment_chemical")));

                            string strTmp = dr.GetInt16(dr.GetOrdinal("fld_row_spacing")).ToString();
                            if (strTmp == "0") strTmp = "";
                            Common.UILib.SelectDropDown(ddlRowSpacing, strTmp);

                            string tmpDate = dr.GetString(dr.GetOrdinal("fld_planting_date"));
                            if (tmpDate.Length > 0) {
                                tmpDate = DateTime.Parse(tmpDate).ToString("M/d");
                            }
                            txtPlantingDate.Text = tmpDate;

                            tmpDate = dr.GetString(dr.GetOrdinal("fld_emerg_80_date"));
                            if (tmpDate.Length > 0) {
                                tmpDate = DateTime.Parse(tmpDate).ToString("M/d");
                            }
                            txt80EmergDate.Text = tmpDate;

                            if (dr.GetDecimal(dr.GetOrdinal("fld_plant_spacing")) == 0) {
                                Common.UILib.SelectDropDown(ddlPlantSpacing, "");
                            } else {
                                Common.UILib.SelectDropDown(ddlPlantSpacing, dr.GetDecimal(dr.GetOrdinal("fld_plant_spacing")).ToString("##.#"));
                            }

                            tmpDate = dr.GetString(dr.GetOrdinal("fld_replant_date"));
                            if (tmpDate.Length > 0) {
                                tmpDate = DateTime.Parse(tmpDate).ToString("M/d");
                            }
                            txtReplantingDate.Text = tmpDate;

                            Common.UILib.SelectDropDown(ddlReplantVariety, dr.GetString(dr.GetOrdinal("fld_replant_seed_variety")));
                            txtReplantAcres.Text = dr.GetInt32(dr.GetOrdinal("fld_acres_replanted")).ToString();
                            txtLostAcres.Text = dr.GetInt32(dr.GetOrdinal("fld_acres_lost")).ToString();

                            Common.UILib.SelectDropDown(ddlSoilTexture, dr.GetString(dr.GetOrdinal("fld_soil_texture")));
                            Common.UILib.SelectDropDown(ddlSoilTest, dr.GetString(dr.GetOrdinal("fld_test_season")));

                            if (dr.GetInt16(dr.GetOrdinal("fld_test_depth")) == 0) {
                                Common.UILib.SelectDropDown(ddlSampleDepth, "");
                            } else {
                                Common.UILib.SelectDropDown(ddlSampleDepth, dr.GetInt16(dr.GetOrdinal("fld_test_depth")).ToString());
                            }

                            txtTestedN.Text = dr.GetDecimal(dr.GetOrdinal("fld_test_N")).ToString("#,##0.00");
                            Common.UILib.SelectDropDown(ddlTestedP, dr.GetString(dr.GetOrdinal("fld_test_P")));
                            txtTestedK.Text = dr.GetDecimal(dr.GetOrdinal("fld_test_K")).ToString("#,##0.00");
                            txtTestedSalts.Text = dr.GetDecimal(dr.GetOrdinal("fld_test_salts")).ToString("#,##0.00");
                            txtTestedpH.Text = dr.GetDecimal(dr.GetOrdinal("fld_test_pH")).ToString("#,##0.00");
                            txtTestedOm.Text = dr.GetDecimal(dr.GetOrdinal("fld_test_org_mat")).ToString("#,##0.00");

                            if (dr.GetInt32(dr.GetOrdinal("fld_last_yr_manure")) == 0) {
                                Common.UILib.SelectDropDown(ddlManureYear, NO_VALUE);
                            } else {
                                Common.UILib.SelectDropDown(ddlManureYear, dr.GetInt32(dr.GetOrdinal("fld_last_yr_manure")).ToString());
                            }

                            txtFertFallN.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_N")).ToString("#,##0");
                            txtFertFallP.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_P")).ToString("#,##0");
                            txtFertFallK.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_fal_K")).ToString("#,##0");
                            txtFertSpringN.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_N")).ToString("#,##0");
                            txtFertSpringP.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_P")).ToString("#,##0");
                            txtFertSpringK.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_spr_K")).ToString("#,##0");
                            txtFertSeasonN.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_N")).ToString("#,##0");
                            txtFertSeasonP.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_P")).ToString("#,##0");
                            txtFertSeasonK.Text = dr.GetDecimal(dr.GetOrdinal("fld_fert_ins_K")).ToString("#,##0");
                            Common.UILib.SelectDropDown(ddlFertStarter, dr.GetString(dr.GetOrdinal("fld_fert_starter")));
                            Common.UILib.SelectDropDown(ddlPreInsecticide, dr.GetString(dr.GetOrdinal("fld_pre_insecticide")));
                            Common.UILib.SelectDropDown(ddlPostInsecticide, dr.GetString(dr.GetOrdinal("fld_post_insectcide")));
                            Common.UILib.SelectDropDown(ddlPreWeedControl, dr.GetString(dr.GetOrdinal("fld_pre_weed_ctrl")));
                            Common.UILib.SelectDropDown(ddlHerbicideRxCount, dr.GetInt32(dr.GetOrdinal("fld_herbicide_rx_count")).ToString());

                            Common.UILib.SelectDropDown(ddlIsLaybyHerbicide, dr.GetString(dr.GetOrdinal("fld_layby_herbicide")));
                            Common.UILib.SelectDropDown(ddlLaybyHerbicide, dr.GetString(dr.GetOrdinal("fld_layby_herbicide_chemical")));
                            Common.UILib.SelectDropDown(ddlRootMaggot, dr.GetString(dr.GetOrdinal("fld_root_maggot_insecticide")));
                            txtChemCounter.Text = dr.GetDecimal(dr.GetOrdinal("fld_rootm_counter_lbs")).ToString("#,##0");
                            txtChemTemik.Text = dr.GetDecimal(dr.GetOrdinal("fld_rootm_temik_lbs")).ToString("#,##0");
                            txtChemThimet.Text = dr.GetDecimal(dr.GetOrdinal("fld_rootm_thimet_lbs")).ToString("#,##0");
                            Common.UILib.SelectDropDown(ddlApp1Chem, dr.GetString(dr.GetOrdinal("fld_cercsp_app1_chemical")));

                            tmpDate = dr.GetString(dr.GetOrdinal("fld_cercospora_app1_date"));
                            if (tmpDate.Length > 0) {
                                tmpDate = DateTime.Parse(tmpDate).ToString("M/d");
                            }
                            txtApp1Date.Text = tmpDate;

                            Common.UILib.SelectDropDown(ddlApp2Chem, dr.GetString(dr.GetOrdinal("fld_cercsp_app2_chemical")));

                            tmpDate = dr.GetString(dr.GetOrdinal("fld_cercospora_app2_date"));
                            if (tmpDate.Length > 0) {
                                tmpDate = DateTime.Parse(tmpDate).ToString("M/d");
                            }
                            txtApp2Date.Text = tmpDate;

                            Common.UILib.SelectDropDown(ddlApp3Chem, dr.GetString(dr.GetOrdinal("fld_cercsp_app3_chemical")));

                            tmpDate = dr.GetString(dr.GetOrdinal("fld_cercospora_app3_date"));
                            if (tmpDate.Length > 0) {
                                tmpDate = DateTime.Parse(tmpDate).ToString("M/d");
                            }
                            txtApp3Date.Text = tmpDate;

                            Common.UILib.SelectDropDown(ddlHailStress, dr.GetString(dr.GetOrdinal("fld_hail_stress")));
                            Common.UILib.SelectDropDown(ddlWeedControl, dr.GetString(dr.GetOrdinal("fld_weed_control")));
                            Common.UILib.SelectDropDown(ddlReplantReason, dr.GetString(dr.GetOrdinal("fld_replant_reason")));
                            Common.UILib.SelectDropDown(ddlLostReason, dr.GetString(dr.GetOrdinal("fld_lost_reason")));
                            Common.UILib.SelectDropDown(ddlTreatedPowderyMildew, dr.GetString(dr.GetOrdinal("fld_treated_powdery_mildew")));
                            Common.UILib.SelectDropDown(ddlTreatedNematode, dr.GetString(dr.GetOrdinal("fld_treated_nematode")));
                            Common.UILib.SelectDropDown(ddlTreatedRhizoctonia, dr.GetString(dr.GetOrdinal("fld_treated_rhizoctonia")));
                            Common.UILib.SelectDropDown(ddlReviewed, dr.GetString(dr.GetOrdinal("fld_reviewed")));
                            Common.UILib.SelectDropDown(ddlIncludeData, dr.GetString(dr.GetOrdinal("fld_include")));
                            Common.UILib.SelectDropDown(ddlSampleGridZone, dr.GetString(dr.GetOrdinal("fld_grid_zone")));

                            txtComment.Text = dr.GetString(dr.GetOrdinal("fld_comment"));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("FieldAgronomy.ShowFieldAgronomy", ex);
                throw (wex);
            }
        }
    }
}