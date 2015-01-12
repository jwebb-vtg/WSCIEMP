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

    public partial class SoilLab : Common.BasePage {

        private const string MOD_NAME = "Fields.SoilLab.";
        private const string BLANK_CELL = "&nbsp;";
        private WSCShsData _shs = null;
        private string _busName = "";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

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

                        FillFieldGrid();
                        FillFieldLabResults();
                        FillOtherLabResults();

                    } else {

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

                if (Common.CodeLib.IsValidSHID(txtSHID.Text)) {

                    string query = txtSHID.Text;
                    int shid = (query.Length > 0 ? Convert.ToInt32(query) : 0);
                    FindAddress(shid);

                } else {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                    throw (warn);
                }
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

        private void FillFieldGrid() {

            const string METHOD_NAME = "FillFieldGrid";
            try {

                int shid = SHID;
                int cropYear = CropYear;
                int divHeight = 160;

                divFieldResultsEmpty.Attributes.Add("class", "DisplayOff");
                divFieldResultsEmpty.InnerHtml = "";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    ArrayList fields = WSCField.FieldGetByGrower(conn, shid, cropYear);

                    // Do we have any valid data?  If not show warning.						
                    if (fields.Count == 1) {
                        ContractFieldState field = (ContractFieldState)fields[0];
                        if (field.FieldID == 0 && shid > 0) {
                            divFieldResultsEmpty.Attributes.Add("class", "WarnNoData");
                            divFieldResultsEmpty.InnerHtml = "No fields are assigned to contracts for SHID " + shid.ToString() +
                                " in crop year " + cropYear.ToString();
                        }
                    } else {
                        divHeight += 17 * (fields.Count + 1);
                    }

                    string style = "BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; OVERFLOW: auto; BORDER-LEFT: black 1px solid; WIDTH: 935px; BORDER-BOTTOM: black 1px solid; HEIGHT: " + divHeight.ToString() + "px;";
                    divFields.Attributes.Add("style", style);

                    grdFieldResults.SelectedIndex = -1;
                    grdFieldResults.DataSource = fields;
                    grdFieldResults.DataBind();
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillOtherLabResults() {

            const string METHOD_NAME = "FillOtherLabResults";
            try {

                int shid = SHID;
                int cropYear = CropYear;
                int divHeight = 160;

                divOtherLabResultsEmpty.Attributes.Add("class", "DisplayOff");
                divOtherLabResultsEmpty.InnerHtml = "";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    ArrayList labs = WSCField.SoilSampleLabGetByGrower(conn, shid, cropYear);

                    // Do we have any valid data?  If not show warning.						
                    if (labs.Count == 1) {
                        SoilSampleLabState lab = (SoilSampleLabState)labs[0];
                        if (lab.SoilSampleLabID == 0 && shid > 0) {
                            divOtherLabResultsEmpty.Attributes.Add("class", "WarnNoData");
                            divOtherLabResultsEmpty.InnerHtml = "No soil sample labs are assigned to SHID " + shid.ToString() +
                                " in crop year " + cropYear.ToString();
                        }
                    } else {
                        divHeight += 17 * (labs.Count + 1);
                    }

                    string style = "BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; OVERFLOW: auto; BORDER-LEFT: black 1px solid; WIDTH: 935px; BORDER-BOTTOM: black 1px solid; HEIGHT: " + divHeight.ToString() + "px;";
                    divOtherLabs.Attributes.Add("style", style);

                    grdOtherLabResults.SelectedIndex = -1;
                    grdOtherLabResults.DataSource = labs;
                    grdOtherLabResults.DataBind();
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillFieldLabResults() {

            const string METHOD_NAME = "FillFieldLabResults";
            try {

                int cropYear = CropYear;
                int fieldID = 0;
                int divHeight = 120;

                divFieldLabResultsEmpty.Attributes.Add("class", "DisplayOff");
                divFieldLabResultsEmpty.InnerHtml = "";

                if (grdFieldResults.SelectedRow != null) {
                    fieldID = Convert.ToInt32(grdFieldResults.SelectedRow.Cells[0].Text.Replace(BLANK_CELL, ""));
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    ArrayList labs = WSCField.SoilSampleLabGetByField(conn, fieldID, cropYear);

                    // Do we have any valid data?  If not show warning.						
                    if (labs.Count == 1) {
                        SoilSampleLabState lab = (SoilSampleLabState)labs[0];
                        if (lab.SoilSampleLabID == 0 && fieldID > 0) {
                            divFieldLabResultsEmpty.Attributes.Add("class", "WarnNoData");
                            divFieldLabResultsEmpty.InnerHtml = "No soil sample labs are assigned to the selected field " +
                                "in crop year " + cropYear.ToString();
                        }
                    } else {
                        divHeight += 17 * (labs.Count + 1);
                    }

                    string style = "BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; OVERFLOW: auto; BORDER-LEFT: black 1px solid; WIDTH: 935px; BORDER-BOTTOM: black 1px solid; HEIGHT: " + divHeight.ToString() + "px;";
                    divFieldLabs.Attributes.Add("style", style);

                    grdFieldLabResults.SelectedIndex = -1;
                    grdFieldLabResults.DataSource = labs;
                    grdFieldLabResults.DataBind();
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private ArrayList GetAllGridIDs(GridView grd, string labelName) {

            const string METHOD_NAME = "GetAllGridIDs";
            try {

                ArrayList rtn = new ArrayList();
                foreach (GridViewRow dgi in grd.Rows) {
                    rtn.Add(((Label)dgi.FindControl(labelName)).Text);
                }

                return rtn;
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private ArrayList GetSelectedIDs(GridView grd, string checkBoxName, string labelName) {

            const string METHOD_NAME = "GetSelectedIDs";
            try {

                int selectedIndex = -1;
                int rowPtr = 0;
                ArrayList rtn = new ArrayList();
                CheckBox chk;
                foreach (GridViewRow dgi in grd.Rows) {
                    chk = (CheckBox)dgi.FindControl(checkBoxName);
                    if (chk.Checked) {
                        selectedIndex = rowPtr;
                        rtn.Add(((Label)dgi.FindControl(labelName)).Text);
                    }
                    rowPtr++;
                }

                if (rtn.Count == 1) {
                    grd.SelectedIndex = selectedIndex;
                } else {
                    grd.SelectedIndex = -1;
                }
                return rtn;
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ShowHideFrames() {

            const string METHOD_NAME = "ShowHideFrames";
            try {

                if (txtContractFields.Text == "Show") {
                    divFields.Attributes.Add("class", "DisplayOff");
                    switchContractFields.InnerHtml = "Show";
                } else {
                    divFields.Attributes.Add("class", "DisplayOn");
                    switchContractFields.InnerHtml = "Hide";
                }

                if (txtFieldSamples.Text == "Show") {
                    divFieldLabs.Attributes.Add("class", "DisplayOff");
                    switchFieldLabResults.InnerHtml = "Show";
                } else {
                    divFieldLabs.Attributes.Add("class", "DisplayOn");
                    switchFieldLabResults.InnerHtml = "Hide";
                }

                if (txtAllSamples.Text == "Show") {
                    divOtherLabs.Attributes.Add("class", "DisplayOff");
                    switchOtherLabResults.InnerHtml = "Show";
                } else {
                    divOtherLabs.Attributes.Add("class", "DisplayOn");
                    switchOtherLabResults.InnerHtml = "Hide";
                }

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {

                CropYear = Convert.ToInt32(ddlCropYear.SelectedValue);

                if (MemberID > 0) {

                    FillFieldGrid();
                    FillFieldLabResults();
                    FillOtherLabResults();
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridViewFieldResults(grdFieldResults);
            AddRowSelectToGridViewFieldLabResults(grdFieldLabResults);
            AddRowSelectToGridViewOtherLabResults(grdOtherLabResults);
            base.Render(writer);
        }

        private void AddRowSelectToGridViewFieldResults(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";
                //row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
                row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
            }
        }

        private void AddRowSelectToGridViewFieldLabResults(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";
                //row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
                row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
            }
        }

        private void AddRowSelectToGridViewOtherLabResults(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";
                //row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
                //row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
            }
        }

        protected void btnAddLab_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAddLab_Click";

            try {
                if (grdFieldResults.SelectedRow == null) {
                    Common.CWarning warn = new Common.CWarning("You must first select a field in the Contract Fields grid.");
                    throw (warn);
                }

                ArrayList selectedIDs = GetSelectedIDs(grdOtherLabResults, "chkAllLabsSelection", "lblAllLabsSoilSampleLabID");
                if (selectedIDs.Count == 0) {
                    Common.CWarning warn = new Common.CWarning("You must select a lab result in the All Lab Results grid.");
                    throw (warn);
                }
                if (selectedIDs.Count > 1) {
                    Common.CWarning warn = new Common.CWarning("Please select only one All Lab Results lab to add to the Field.");
                    throw (warn);
                }

                string fieldName = grdOtherLabResults.SelectedRow.Cells[3].Text.Replace(BLANK_CELL, "");
                if (fieldName.Length > 0) {
                    string contract = grdOtherLabResults.SelectedRow.Cells[2].Text.Replace(BLANK_CELL, "");
                    Common.CWarning warn = new Common.CWarning("This lab already belongs to contract " + contract + " and field " + fieldName + ".");
                    throw (warn);
                }

                int cropYear = CropYear;
                int fieldID = Convert.ToInt32(grdFieldResults.SelectedRow.Cells[0].Text.Replace(BLANK_CELL, ""));
                int soilSampleLabID = Convert.ToInt32(grdOtherLabResults.SelectedRow.Cells[1].Text.Replace(BLANK_CELL, ""));

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    WSCField.FieldSampleLabSave(conn, fieldID, cropYear, soilSampleLabID);
                }

                FillFieldLabResults();
                FillOtherLabResults();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnRemoveLab_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnRemoveLab_Click";

            try {
                if (grdFieldLabResults.SelectedRow == null) {
                    Common.CWarning warn = new Common.CWarning("You must first select a lab from the Field's Soil Sample Lab Results grid.");
                    throw (warn);
                }
                int soilSampleLabID = Convert.ToInt32(grdFieldLabResults.SelectedRow.Cells[0].Text.Replace(BLANK_CELL, ""));

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    WSCField.FieldSampleLabRemove(conn, soilSampleLabID);
                }

                FillFieldLabResults();
                FillOtherLabResults();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }	
        }

        protected void btnPrintAll_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnPrintAll_Click";

            try {
                ArrayList labs = GetAllGridIDs(grdOtherLabResults, "lblAllLabsSoilSampleLabID");
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnPrintSelected_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnPrintSelected_Click";

            try {
                ArrayList labs = GetSelectedIDs(grdOtherLabResults, "chkAllLabsSelection", "lblAllLabsSoilSampleLabID");
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

                if (MemberID > 0) {
                    FillFieldGrid();
                    FillFieldLabResults();
                    FillOtherLabResults();
                } else {
                    Common.CWarning warn = new Common.CWarning("Please enter a valid SHID and press the Find button.");
                    throw (warn);
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdFieldResults_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdFieldResults_SelectedIndexChanged";

            try {
                FillFieldLabResults();
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
