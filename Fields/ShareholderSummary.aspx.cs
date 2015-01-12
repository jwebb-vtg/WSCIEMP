using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using WSCData;

namespace WSCIEMP.Fields {

    public partial class ShareholderSummary : Common.BasePage {

        private const string MOD_NAME = "Fields.ShareholderSummary.";
        private WSCShsData _shs = null;
        private string _busName = "";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            if (Globals.IsUserPermissionReadOnly((RolePrincipal)User)) {
                btnSave.Enabled = false;
            }

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                _shs = Globals.ShsData;

                HtmlGenericControl body = (HtmlGenericControl)this.Master.FindControl("MasterBody");
                body.Attributes.Add("onload", "DoOnLoad();");

                locPDF.Text = "";
                ShowHideFrames();

                if (!Page.IsPostBack) {

                    FillCropYear();
                    FindAddress(_shs.SHID);
                    InitShareholder();

                    if (MemberID > 0) {

                        FillContractSummary();
                        FillContractPerfGrid();
                        FillGrowerAdvice();

                    } else {
                        ResetGrowerAdvice();
                        Common.CWarning warn = new Common.CWarning("Please enter a valid SHID and press the Find button.");
                        throw (warn);
                    }

                }

                _busName = lblBusName.Text;
                if (ddlCropYear.SelectedIndex != -1) {
                    CropYear = Convert.ToInt32(ddlCropYear.SelectedValue);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillCropYear() {

            if (ddlCropYear.Items.Count > 0) {
                CropYear = Int32.Parse(ddlCropYear.SelectedValue);
            }
            if (CropYear == 0) {
                CropYear = _shs.CropYear;
            }
            if (CropYear == 0) {
                CropYear = Convert.ToInt32(WSCField.GetCropYears()[0].ToString());
            }

            WSCField.FillCropYear(ddlCropYear, CropYear.ToString());
        }

        private void InitShareholder() {

            int initShid = 0;

            if (Common.CodeLib.IsValidSHID(txtSHID.Text)) {
                initShid = Int32.Parse(txtSHID.Text);
            }

            if (initShid == 0) {
                initShid = SHID;
            }

            // Set the global value and the local control
            if (initShid > 0) {
                SHID = initShid;
                txtSHID.Text = initShid.ToString();
                lblBusName.Text = _busName;
            } else {
                SHID = initShid;
                txtSHID.Text = "";
            }
        }

        private void FindAddress(int shid) {

            ResetShareholder();
            GetShareholder(shid, CropYear);
            ShowShareholder();
        }

        private void FindAddress() {

            try {
                
                int shid = 0;
                if (Common.CodeLib.IsValidSHID(txtSHID.Text)) {
                    shid = Convert.ToInt32(txtSHID.Text);
                } else {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                }
                FindAddress(shid);
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("SoilLab.FindAddress", ex);
                throw (wex);
            }
        }

        private void GetShareholder(int shid, int cropYear) {

            try {

                int memberID = 0, addressID = 0;
                string phone = "", email = "", fax = "";

                WSCMember.GetInfo(shid.ToString(), ref memberID, ref addressID, ref _busName, ref phone, ref email, ref fax);

                SHID = shid;
                CropYear = cropYear;
                MemberID = memberID;
                AddressID = addressID;
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("UserContractSelector.GetShareholder", ex);
                throw (wex);
            }
        }

        private void ShowShareholder() {
            Common.UILib.SelectDropDown(ddlCropYear, CropYear.ToString());
            txtSHID.Text = SHID.ToString();
            lblBusName.Text = _busName;
        }

        private void ResetShareholder() {

            _shs.ResetShareholder();
            SHID = 0;
            MemberID = 0;
            AddressID = 0;
            txtSHID.Text = "0";
            lblBusName.Text = "";
            _busName = "";
        }

        private void ShowHideFrames() {

            const string METHOD_NAME = "ShowHideFrames";
            try {

                if (txtContractPerf.Text == "Show") {
                    divPerf.Attributes.Add("class", "DisplayOff");
                    switchContractPerf.InnerHtml = "Show";
                } else {
                    divPerf.Attributes.Add("class", "DisplayOn");
                    switchContractPerf.InnerHtml = "Hide";
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillContractSummary() {

            const string METHOD_NAME = "FillContractSummary";
            try {

                int shid = SHID;
                int cropYear = CropYear;

                divRegionAreaEmpty.Attributes.Add("class", "DisplayOff");
                divRegionAreaEmpty.InnerHtml = "";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.SharholderSummaryGetAreas(conn, cropYear, shid)) {

                        grdRegionArea.SelectedIndex = -1;
                        grdRegionArea.DataSource = dr;
                        grdRegionArea.DataBind();

                        if (grdRegionArea.Rows.Count == 0) {
                            divRegionAreaEmpty.Attributes.Add("class", "WarnNoData");
                            divRegionAreaEmpty.InnerHtml = "No contract performance values were found for contracts of SHID " + shid.ToString() +
                                " in crop year " + cropYear.ToString();
                        } else {
                            grdRegionArea.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillContractPerfGrid() {

            const string METHOD_NAME = "FillContractPerfGrid";
            const string DIV_STD_STYLE = "BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; OVERFLOW: auto; BORDER-LEFT: black 1px solid; WIDTH: 935px; BORDER-BOTTOM: black 1px solid; HEIGHT: ";
            try {

                int shid = SHID;
                int cropYear = CropYear;
                string regionCode = "";
                string areaCode = "";
                int divHeight = 160;
                int growerPerformanceID = 0;
                string style = DIV_STD_STYLE;

                txtGrowerPerformanceID.Text = "";

                divPerfResultsEmpty.Attributes.Add("class", "DisplayOff");
                divPerfResultsEmpty.InnerHtml = "";

                if (grdRegionArea.SelectedRow != null) {
                    txtGrowerPerformanceID.Text = grdRegionArea.SelectedRow.Cells[0].Text;
                    growerPerformanceID = Convert.ToInt32(txtGrowerPerformanceID.Text);
                    regionCode = grdRegionArea.SelectedRow.Cells[1].Text;
                    areaCode = grdRegionArea.SelectedRow.Cells[2].Text;
                } else {

                    if (grdRegionArea.Rows.Count == 0) {

                        style += divHeight.ToString() + "px;";
                        divPerf.Attributes.Add("style", style);

                        Common.CWarning warn = new Common.CWarning("Please select a Crop Year that has harvest tons for this SHID.");
                        throw (warn);
                    }
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    ArrayList cntPerfs = WSCField.ShareholderSummaryContracts(conn, shid, cropYear, regionCode, areaCode);

                    // Do we have any valid data?  If not show warning.						
                    if (cntPerfs.Count == 1) {

                        ContractPerformanceState perf = (ContractPerformanceState)cntPerfs[0];
                        if (perf.SHID == "") {

                            divPerfResultsEmpty.Attributes.Add("class", "WarnNoData");
                            divPerfResultsEmpty.InnerHtml = @"Please select a Region / Area from the above gird.";
                        }
                    } else {
                        divHeight += 17 * (cntPerfs.Count + 6);
                    }

                    style += divHeight.ToString() + "px;";
                    divPerf.Attributes.Add("style", style);

                    MakeCntPerfTable(cntPerfs);

                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ResetGrowerAdvice() {

            // First clear all data entry fields than fill them.
            chkDiseaseBad.Checked = false;
            chkDiseaseOkay.Checked = false;
            chkFertilityBad.Checked = false;
            chkFertilityOkay.Checked = false;
            chkIrrigationBad.Checked = false;
            chkIrrigationOkay.Checked = false;
            chkStandBad.Checked = false;
            chkStandOkay.Checked = false;
            chkVarietyBad.Checked = false;
            chkVarietyOkay.Checked = false;
            chkWeedBad.Checked = false;
            chkWeedOkay.Checked = false;

            txtDiseaseRec.Text = "";
            txtFertilityRec.Text = "";
            txtIrrigationRec.Text = "";
            txtStandRec.Text = "";
            txtVarietyRec.Text = "";
            txtWeedRec.Text = "";
        }

        private void FillGrowerAdvice() {

            const string METHOD_NAME = "FillGrowerAdvice";

            try {

                ResetGrowerAdvice();

                int growerPerformanceID = 0;

                string sGrowerPerformanceID = txtGrowerPerformanceID.Text;
                if (sGrowerPerformanceID.Length > 0) {
                    growerPerformanceID = Convert.ToInt32(sGrowerPerformanceID);
                }

                if (growerPerformanceID > 0) {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                        using (SqlDataReader dr = WSCField.GrowerAdviceGetBySHID(conn, growerPerformanceID)) {

                            if (dr.Read()) {

                                chkDiseaseBad.Checked = (dr["gadGoodDiseaseControl"].ToString() == "N");
                                chkDiseaseOkay.Checked = (dr["gadGoodDiseaseControl"].ToString() == "Y");
                                chkFertilityBad.Checked = (dr["gadGoodFertilityManagement"].ToString() == "N");
                                chkFertilityOkay.Checked = (dr["gadGoodFertilityManagement"].ToString() == "Y");
                                chkIrrigationBad.Checked = (dr["gadGoodIrrigationManagement"].ToString() == "N");
                                chkIrrigationOkay.Checked = (dr["gadGoodIrrigationManagement"].ToString() == "Y");
                                chkStandBad.Checked = (dr["gadGoodStandEstablishment"].ToString() == "N");
                                chkStandOkay.Checked = (dr["gadGoodStandEstablishment"].ToString() == "Y");
                                chkVarietyBad.Checked = (dr["gadGoodVarietySelection"].ToString() == "N");
                                chkVarietyOkay.Checked = (dr["gadGoodVarietySelection"].ToString() == "Y");
                                chkWeedBad.Checked = (dr["gadGoodWeedControl"].ToString() == "N");
                                chkWeedOkay.Checked = (dr["gadGoodWeedControl"].ToString() == "Y");

                                txtDiseaseRec.Text = dr["gadTextDiseaseControl"].ToString();
                                txtFertilityRec.Text = dr["gadTextFertilityManagement"].ToString();
                                txtIrrigationRec.Text = dr["gadTextIrrigationManagement"].ToString();
                                txtStandRec.Text = dr["gadTextStandEstablishment"].ToString();
                                txtVarietyRec.Text = dr["gadTextVarietySelection"].ToString();
                                txtWeedRec.Text = dr["gadTextWeedControl"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        //================================================
        // Build same results into a table.
        //================================================
        private void MakeCntPerfTable(ArrayList cntPerfs) {

            tabPerfResults.Rows.Clear();
            tabPerfResults.Attributes.Add("style", "width:1100px; border: 1px solid #000000; background-color:#ffffff;");

            if (cntPerfs.Count > 0 && ((ContractPerformanceState)cntPerfs[0]).SHID != "") {

                //=====================================
                // build header
                //=====================================
                TableRow tr = new TableRow();

                tr.CssClass = "text";
                tr.Attributes.Add("style", "font-weight:bold;");

                TableCell td = new TableCell();
                td.Text = "Contract";
                td.Attributes.Add("style", "width: 7%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Station";
                td.Attributes.Add("style", "width: 12%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Field Desc.";
                td.Attributes.Add("style", "width: 14%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Landowner";
                td.Attributes.Add("style", "width: 11%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Final Net Tons";
                td.Attributes.Add("style", "width: 8%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Tons / Acre";
                td.Attributes.Add("style", "width: 8%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Sugar %";
                td.Attributes.Add("style", "width: 8%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Tare %";
                td.Attributes.Add("style", "width: 8%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "SLM %";
                td.Attributes.Add("style", "width: 8%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Ext. Sugar / Ton";
                td.Attributes.Add("style", "width: 8%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Plant Pop.";
                td.Attributes.Add("style", "width: 8%; border:1px solid #000000;");
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                tabPerfResults.Rows.Add(tr);
                tr.CssClass = "text";

                // Data
                for (int i = 0; i < cntPerfs.Count; i++) {

                    ContractPerformanceState perf = (ContractPerformanceState)cntPerfs[i];
                    tr = new TableRow();
                    tr.CssClass = "text";

                    if (perf.RowType == 1) {

                        td = new TableCell();
                        td.Text = perf.ContractNumber;
                        td.Attributes.Add("style", "text-align: center; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.ContractStation;
                        td.Attributes.Add("style", "text-align: left; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.FieldDescription;
                        td.Attributes.Add("style", "text-align: left; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.LandownerName;
                        td.Attributes.Add("style", "text-align: left; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.HarvestFinalNetTons;
                        td.Attributes.Add("style", "text-align: right; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.TonsPerAcre;
                        td.Attributes.Add("style", "text-align: center; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.HarvestSugarPct;
                        td.Attributes.Add("style", "text-align: center; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.HarvestTarePct;
                        td.Attributes.Add("style", "text-align: center; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.HarvestSLMPct;
                        td.Attributes.Add("style", "text-align: center; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.HarvestExtractableSugar;
                        td.Attributes.Add("style", "text-align: center; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = perf.BeetsPerAcre;
                        td.Attributes.Add("style", "text-align: center; border:1px solid #000000;");
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                    } else {

                        switch (perf.RowType) {

                            case 2:				//	** Overall Average **

                                td = new TableCell();
                                td.ColumnSpan = 11;
                                td.Attributes.Add("style", "border-color: #ffffff; line-height:8px; height: 8px;");
                                td.Text = "&nbsp;";
                                tr.Cells.Add(td);
                                tabPerfResults.Rows.Add(tr);

                                tr = new TableRow();
                                tr.CssClass = "text";

                                td = new TableCell();
                                td.Text = "&nbsp;";
                                td.Attributes.Add("style", "border-top:none;border-right:none;border-bottom:none;");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.ColumnSpan = 3;
                                td.Text = "Overall Average";
                                td.Attributes.Add("style", "border-left:none;border-top:none;border-right:none;border-bottom: 1px solid #000000;font-weight:bold;");
                                td.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestFinalNetTons;
                                td.Attributes.Add("style", "text-align: right; border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.TonsPerAcre;
                                td.Attributes.Add("style", "text-align: center; border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestSugarPct;
                                td.Attributes.Add("style", "text-align: center; border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestTarePct;
                                td.Attributes.Add("style", "text-align: center; border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestSLMPct;
                                td.Attributes.Add("style", "text-align: center; border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestExtractableSugar;
                                td.Attributes.Add("style", "text-align: center; border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.BeetsPerAcre;
                                td.Attributes.Add("style", "text-align: center; border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                break;

                            case 3:				// ** Top 20% Area Average

                                td = new TableCell();
                                td.Text = "&nbsp;";
                                td.Attributes.Add("style", "border-top:none;border-right:none;border-bottom:none;");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.ColumnSpan = 4;
                                td.Text = "Top 20% Area Average";
                                td.Attributes.Add("style", "border-left:none;border-right:none;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.TonsPerAcre;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestSugarPct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestTarePct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestSLMPct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestExtractableSugar;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.BeetsPerAcre;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                break;

                            case 4:				// ** Area Average **

                                td = new TableCell();
                                td.ColumnSpan = 11;
                                td.Attributes.Add("style", "border-top:none;border-bottom:none; line-height:8px; height: 8px;");
                                td.Text = "&nbsp;";
                                tr.Cells.Add(td);
                                tabPerfResults.Rows.Add(tr);

                                tr = new TableRow();
                                tr.CssClass = "text";

                                td = new TableCell();
                                td.Text = "&nbsp;";
                                td.Attributes.Add("style", "border-top:none;border-right:none;border-bottom:none;");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.ColumnSpan = 10;
                                td.Attributes.Add("style", "border-left:none;border-top:none;border-bottom:none;font-weight:bold;");
                                td.Text = "Your Rankings";
                                tr.Cells.Add(td);
                                tabPerfResults.Rows.Add(tr);

                                tr = new TableRow();
                                tr.CssClass = "text";

                                td = new TableCell();
                                td.Text = "&nbsp;";
                                td.Attributes.Add("style", "border-top:none;border-right:none;border-bottom:none;");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.ColumnSpan = 4;
                                td.Text = perf.FieldArea;
                                td.Attributes.Add("style", "border-left:none;border-right:none;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.TonsPerAcre;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestSugarPct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestTarePct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestSLMPct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestExtractableSugar;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.BeetsPerAcre;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                break;

                            case 5:				// Region Average

                                td = new TableCell();
                                td.Text = "&nbsp;";
                                td.Attributes.Add("style", "border-top:none;border-right:none;border-bottom:none;");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.ColumnSpan = 4;
                                td.Text = perf.FieldRegion;
                                td.Attributes.Add("style", "border:thin; border-top:1px solid #000000; border-bottom:none");
                                td.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.TonsPerAcre;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestSugarPct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestTarePct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestSLMPct;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.HarvestExtractableSugar;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = perf.BeetsPerAcre;
                                td.Attributes.Add("style", "text-align: center;border-bottom: 1px solid #000000;");
                                td.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(td);

                                break;

                        }
                    }

                    tabPerfResults.Rows.Add(tr);
                }
            }
        }

        private void RefreshAfterUpdate() {

            int selectedRegion = grdRegionArea.SelectedIndex;

            FillContractSummary();
            grdRegionArea.SelectedIndex = selectedRegion;
            FillGrowerAdvice();
            FillContractPerfGrid();
        }

        private void ClearContractSummary() {

            grdRegionArea.DataSource = null;
            grdRegionArea.DataBind();
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));                

                CropYear = Convert.ToInt32(ddlCropYear.SelectedValue);
                ClearContractSummary();
                ResetGrowerAdvice();

                if (MemberID > 0) {
                    FillContractSummary();
                    FillContractPerfGrid();
                    FillGrowerAdvice();
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdRegionArea);
            base.Render(writer);
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";
                //row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
                row.Attributes.Add("onclick", "SelectRow(this); " + Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));

            }
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            try {

                string sGrowerPerformanceID = txtGrowerPerformanceID.Text;
                int growerPerformanceID = 0;

                if (sGrowerPerformanceID.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please select a Region / Area from the top most grid.");
                    throw (warn);
                } else {
                    growerPerformanceID = Convert.ToInt32(sGrowerPerformanceID);
                }

                string sFertilityChoice = "";
                bool isFertilityOkay = chkFertilityOkay.Checked;
                bool isFertilityBad = chkFertilityBad.Checked;

                if (isFertilityOkay && isFertilityBad) {
                    Common.CWarning warn = new Common.CWarning("You cannot set Fertility to both Okay and Needs Improvement.");
                    throw (warn);
                }
                if (isFertilityOkay) {
                    sFertilityChoice = "Y";
                } else {
                    if (isFertilityBad) {
                        sFertilityChoice = "N";
                    }
                }

                string sIrrigationChoice = "";
                bool isIrrigationOkay = chkIrrigationOkay.Checked;
                bool isIrrigationBad = chkIrrigationBad.Checked;

                if (isIrrigationOkay && isIrrigationBad) {
                    Common.CWarning warn = new Common.CWarning("You cannot set Irrigation Water Management to both Okay and Needs Improvement.");
                    throw (warn);
                }
                if (isIrrigationOkay) {
                    sIrrigationChoice = "Y";
                } else {
                    if (isIrrigationBad) {
                        sIrrigationChoice = "N";
                    }
                }

                string sStandChoice = "";
                bool isStandOkay = chkStandOkay.Checked;
                bool isStandBad = chkStandBad.Checked;

                if (isStandOkay && isStandBad) {
                    Common.CWarning warn = new Common.CWarning("You cannot set Stand Establishment to both Okay and Needs Improvement.");
                    throw (warn);
                }
                if (isStandOkay) {
                    sStandChoice = "Y";
                } else {
                    if (isStandBad) {
                        sStandChoice = "N";
                    }
                }

                string sWeedChoice = "";
                bool isWeedOkay = chkWeedOkay.Checked;
                bool isWeedBad = chkWeedBad.Checked;

                if (isWeedOkay && isWeedBad) {
                    Common.CWarning warn = new Common.CWarning("You cannot set Weed Control to both Okay and Needs Improvement.");
                    throw (warn);
                }
                if (isWeedOkay) {
                    sWeedChoice = "Y";
                } else {
                    if (isWeedBad) {
                        sWeedChoice = "N";
                    }
                }

                string sDiseaseChoice = "";
                bool isDiseaseOkay = chkDiseaseOkay.Checked;
                bool isDiseaseBad = chkDiseaseBad.Checked;

                if (isDiseaseOkay && isDiseaseBad) {
                    Common.CWarning warn = new Common.CWarning("You cannot set Disease & Insect Control to both Okay and Needs Improvement.");
                    throw (warn);
                }
                if (isDiseaseOkay) {
                    sDiseaseChoice = "Y";
                } else {
                    if (isDiseaseBad) {
                        sDiseaseChoice = "N";
                    }
                }

                string sVarietyChoice = "";
                bool isVarietyOkay = chkVarietyOkay.Checked;
                bool isVarietyBad = chkVarietyBad.Checked;

                if (isVarietyOkay && isVarietyBad) {
                    Common.CWarning warn = new Common.CWarning("You cannot set Proper Variety Selection to both Okay and Needs Improvement.");
                    throw (warn);
                }
                if (isVarietyOkay) {
                    sVarietyChoice = "Y";
                } else {
                    if (isVarietyBad) {
                        sVarietyChoice = "N";
                    }
                }

                string sFertility = txtFertilityRec.Text;
                string sIrrigation = txtIrrigationRec.Text;
                string sStand = txtStandRec.Text;
                string sWeed = txtWeedRec.Text;
                string sDisease = txtDiseaseRec.Text;
                string sVariety = txtVarietyRec.Text;

                WSCSecurity auth = Globals.SecurityState;
                string userName = auth.UserName;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    WSCField.GrowerAdviceSave(conn, growerPerformanceID, sFertilityChoice, sFertility,
                        sIrrigationChoice, sIrrigation, sStandChoice, sStand, sWeedChoice, sWeed, sDiseaseChoice, sDisease,
                        sVarietyChoice, sVariety, userName);
                }

                Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "** Edits Successfully Saved **");
                RefreshAfterUpdate();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnPrint_Click";

            try {

                ArrayList shidList = new ArrayList(1);
                _shs = Globals.ShsData;
                shidList.Add(SHID);

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + "Shareholder Summary".Replace(" ", "");

                if (grdRegionArea.SelectedIndex != -1) {

                    string regionCode = grdRegionArea.SelectedRow.Cells[1].Text;
                    string regionName = grdRegionArea.SelectedRow.Cells[3].Text;
                    string areaCode = grdRegionArea.SelectedRow.Cells[2].Text;
                    string areaName = grdRegionArea.SelectedRow.Cells[4].Text;
                    string growerPerformanceID = grdRegionArea.SelectedRow.Cells[0].Text;

                    string busName = _busName;

                    // pro-actively refresh the manually crafted table before we make the call to generate the PDF.
                    FillContractPerfGrid();

                    string locpdf = WSCReports.rptShareholderSummary.ReportPackager( CropYear, shidList, busName,
                        growerPerformanceID, regionCode, areaCode, regionName, areaName, auth.UserName, fileName, logoUrl, pdfTempFolder);

                    if (locpdf.Length > 0) {
                        // convert file system path to virtual path
                        locpdf = locpdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                    }

                    locPDF.Text = locpdf;

                } else {
                    Common.CWarning warn = new Common.CWarning("Please select a Region / Area from the top grid, or use the Print All button.");
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnPrintAll_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnPrintAll_Click";

            try {

                ArrayList shidList = new ArrayList(1);
                shidList.Add(SHID);

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + "Shareholder Summary".Replace(" ", "");

                // pro-actively refresh the manually crafted table before we make the call to generate the PDF.
                FillContractPerfGrid();

                if (grdRegionArea.Rows.Count > 0) {

                    string busName = _busName;
                    string locpdf = WSCReports.rptShareholderSummary.ReportPackager(CropYear, shidList, busName, 
                        "", "", "", "", "", auth.UserName, fileName, logoUrl, pdfTempFolder);

                    if (locpdf.Length > 0) {
                        // convert file system path to virtual path
                        locpdf = locpdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                    }

                    locPDF.Text = locpdf;

                } else {
                    Common.CWarning warn = new Common.CWarning("Please select a Region / Area from the top grid, or use the Print All button.");
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnAdrFind_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAdrFind_Click";

            try {

                FindAddress();
                ClearContractSummary();
                ResetGrowerAdvice();

                if (MemberID > 0) {

                    FillContractSummary();
                    FillContractPerfGrid();
                    FillGrowerAdvice();

                } else {

                    ResetShareholder();
                    grdRegionArea.DataSource = null;
                    grdRegionArea.DataBind();
                    ResetGrowerAdvice();

                    Common.CWarning warn = new Common.CWarning("Please enter a valid SHID and press the Find button.");
                    throw (warn);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdRegionArea_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdRegionArea_SelectedIndexChanged";

            try {
                ResetGrowerAdvice();
                FillContractPerfGrid();
                FillGrowerAdvice();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private string _shid = null;
        private int SHID {

            set {
                ViewState["SHID"] = value;
                _shid = value.ToString();
                _shs.SHID = value;
            }
            get {
                if (_shid == null) {
                    _shid = (ViewState["SHID"] != null ? Convert.ToInt32(ViewState["SHID"]).ToString() : "0");
                }
                return Convert.ToInt32(_shid);
            }
        }
        private string _memberID = null;
        private int MemberID {

            set {
                ViewState["MemberID"] = value;
                _memberID = value.ToString();
            }
            get {
                if (_memberID == null) {
                    _memberID = (ViewState["MemberID"] != null ? Convert.ToInt32(ViewState["MemberID"]).ToString() : "0");
                }
                return Convert.ToInt32(_memberID);
            }
        }
        private string _addressID = null;
        private int AddressID {

            set {
                ViewState["AddressID"] = value;
                _addressID = value.ToString();
            }
            get {
                if (_addressID == null) {
                    _addressID = (ViewState["AddressID"] != null ? Convert.ToInt32(ViewState["AddressID"]).ToString() : "0");
                }
                return Convert.ToInt32(_addressID);
            }
        }
        private string _cropYear = null;
        private int CropYear {

            set {
                ViewState["CropYear"] = value;
                _shs.CropYear = value;
                _cropYear = value.ToString();
            }
            get {
                if (_cropYear == null) {
                    if (ViewState["CropYear"] != null) {
                        _cropYear = Convert.ToInt32(ViewState["CropYear"]).ToString();
                    } else {
                        _cropYear = _shs.CropYear.ToString();
                        ViewState["CropYear"] = _cropYear;
                    }
                }
                return Convert.ToInt32(_cropYear);
            }
        }
    }
}
