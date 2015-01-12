using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.SHS {
    public partial class optRecDailyHarvestRpt : Common.BasePage {

        private const string MOD_NAME = "SHS.optRecDailyHarvestRpt.";
        private WSCShsData _shs = null;

        private string _busName = "";
        private string _emailAddress = "";
        private string _fax = "";

        private const string SEND_RPT_OPT_WEB = "W";
        private const string SEND_RPT_OPT_EMAIL = "E";
        private const string SEND_RPT_OPT_FAX = "F";
        private const string SEND_RPT_OPT_MAIL = "M";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";            

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                Common.AppHelper.HideWarning(lblEmail);
                Common.AppHelper.HideWarning(lblFax);

                _shs = Globals.ShsData;

                if (Globals.IsUserPermissionReadOnly((RolePrincipal)User)) {
                    btnSave.Enabled = false;
                }

                if (!Page.IsPostBack) {

                    FillCropYear();

                    if (_shs.SHID != 0) {

                        FindAddress(_shs.SHID.ToString());

                        if (txtEmail.Text.Length == 0) {
                            lblEmail.CssClass = "WarningOn";
                            lblEmail.Text = "* Missing *";
                        } else {
                            lblEmail.CssClass = "WarningOff";
                            lblEmail.Text = "";
                        }

                        if (txtFax.Text.Length == 0) {
                            lblFax.CssClass = "WarningOn";
                            lblFax.Text = "* Missing *";
                        } else {
                            lblFax.CssClass = "WarningOff";
                            lblFax.Text = "";
                        }

                        string sendRptOption = null;
                        WSCMember.GetSendRptOption(MemberID, ref sendRptOption);
                        SetSendRptOption(sendRptOption);

                    } else {
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter a SHID and press the Find button.");
                    }
                } else {
                    CropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                }
            }
            catch (System.Exception ex) {

                ResetShareholder();
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

        private void FindAddress(string shid) {

            const string METHOD_NAME = "FindAddress";

            if (shid == null || shid.Length == 0) {

                ResetShareholder();
                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter a SHID.");
                return;
            }

            try {

                ResetMemberInfo(shid);

                txtSHID.Text = SHID.ToString();
                lblBusName.Text = _busName;
                txtEmail.Text = _emailAddress;
                txtFax.Text = _fax;

                string sendRptOption = null;
                if (MemberID > 0) {
                    WSCMember.GetSendRptOption(MemberID, ref sendRptOption);
                }
                SetSendRptOption(sendRptOption);

                if (txtEmail.Text.Length == 0) {
                    lblEmail.CssClass = "WarningOn";
                    lblEmail.Text = "* Missing *";
                } else {
                    lblEmail.CssClass = "WarningOff";
                    lblEmail.Text = "";
                }

                if (txtFax.Text.Length == 0) {
                    lblFax.CssClass = "WarningOn";
                    lblFax.Text = "* Missing *";
                } else {
                    lblFax.CssClass = "WarningOff";
                    lblFax.Text = "";
                }

            }
            catch (System.Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ResetShareholder() {

            _shs.ResetShareholder();
            SHID = 0;
            MemberID = 0;
            AddressID = 0;
            txtSHID.Text = "0";
            lblBusName.Text = "";
            _busName = "";
            txtEmail.Text = "";
            _emailAddress = "";            
            txtFax.Text = "";
            _fax = "";
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

			const string METHOD_NAME = "ddlCropYear_SelectedINdexChanged";

            try {
                if (Common.CodeLib.IsValidSHID(txtSHID.Text)) {
                    string shid = txtSHID.Text;
                    FindAddress(shid);
                } else {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                    throw (warn);
                }
            }
            catch (Exception ex) {

                ResetShareholder();

				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnFind_Click";

            try {
                if (Common.CodeLib.IsValidSHID(txtSHID.Text)) {
                    string shid = txtSHID.Text;
                    FindAddress(shid);
                } else {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                    throw (warn);
                }
            }
            catch (Exception ex) {

                ResetShareholder();
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnSave_Click";

            try {

                int currentCropYear = Convert.ToInt32(WSCField.GetCropYears()[0].ToString());

                if (currentCropYear != CropYear) {
                    Common.CWarning wex = new Common.CWarning("Sorry, you cannot update information for a prior crop year.");
                    throw (wex);
                }

                if (!Common.CodeLib.IsValidSHID(txtSHID.Text)) {
                    Common.CWarning wex = new Common.CWarning("Please enter a valid SHID.");
                    throw (wex);
                }

                if (SHID.ToString() != txtSHID.Text) {
                    Common.CWarning wex = new Common.CWarning("Please press the Find button to ensure you're editing the correct Shareholder before trying to Save.");
                    throw (wex);
                }

                if (SHID == 0) {
                    Common.CWarning wex = new Common.CWarning("Please enter a SHID and press the Find button.");
                    throw (wex);
                }

                WSCSecurity auth = Globals.SecurityState;
                if (auth.AuthorizeShid(SHID, CropYear) < WSCSecurity.ShsPermission.shsReadWrite) {

                    Common.CWarning wex = new Common.CWarning("Sorry, you are not authorized to update this information");
                    throw (wex);
                }

                // Validate Email or fax when needed.
                string sendRptOption = GetSendRptOption();

                if (txtEmail.Text.Length > 0 || sendRptOption == SEND_RPT_OPT_EMAIL) {

                    if (!Common.CodeLib.ValidateEmail(txtEmail.Text)) {
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter the information in the correct format");
                        Common.AppHelper.ShowWarning(lblEmail, "Required to receive reports by email.");
                        return;
                    }
                }

                if (txtFax.Text.Length > 0 || sendRptOption == SEND_RPT_OPT_FAX) {

                    if (!Common.CodeLib.ValidateFax(txtFax.Text)) {
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter the information in the correct format");
                        Common.AppHelper.ShowWarning(lblFax, "Required to receive reports by fax.");
                        return;
                    }
                }

                try {

                    WSCMember.UpdateSendRptOption(MemberID, sendRptOption);
                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Your changes have been successfully updated!");
                }
                catch (System.Exception ex) {
                    if (Common.AppHelper.IsDebugBuild()) {
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), ex);
                    } else {
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Unable to save your changes at this time.", ex);
                        Common.AppHelper.LogException(ex, HttpContext.Current);
                    }
                    return;
                }


                try {

                    WSCMember.UpdateAddress(MemberID, txtEmail.Text, txtFax.Text, Globals.SecurityState.UserName);
                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Your changes have been successfully updated!");
                }
                catch (System.Exception ex) {
                    if (Common.AppHelper.IsDebugBuild()) {
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), ex);
                    } else {
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Unable to save your changes at this time.", ex);
                        Common.AppHelper.LogException(ex, HttpContext.Current);
                    }
                }

                if (((HtmlGenericControl)Master.FindControl("divWarning")).InnerHtml.Length == 0) {
                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Your information has been updated!");
                }
            }
            catch (Exception ex) {

                ResetShareholder();
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void SetSendRptOption(string sendRptOption) {

            switch (sendRptOption) {
                case SEND_RPT_OPT_WEB:
                    radDHRIntranetOnly.Checked = true;
                    break;

                case SEND_RPT_OPT_EMAIL:
                    radDHREmail.Checked = true;
                    break;

                case SEND_RPT_OPT_FAX:
                    radDHRFax.Checked = true;
                    break;

                case SEND_RPT_OPT_MAIL:
                    radDHRRegMail.Checked = true;
                    break;
            }
        }

        private string GetSendRptOption() {

            string sendRptOption = null;

            if (sendRptOption == null && radDHRIntranetOnly.Checked) { sendRptOption = SEND_RPT_OPT_WEB; }
            if (sendRptOption == null && radDHREmail.Checked) { sendRptOption = SEND_RPT_OPT_EMAIL; }
            if (sendRptOption == null && radDHRFax.Checked) { sendRptOption = SEND_RPT_OPT_FAX; }
            if (sendRptOption == null && radDHRRegMail.Checked) { sendRptOption = SEND_RPT_OPT_MAIL; }

            return sendRptOption;

        }

        public void ResetMemberInfo(string shid) {

            int memberID = 0, addressID = 0;
            string phone = "";

            try {
                WSCMember.GetInfo(shid, ref memberID, ref addressID, ref _busName, ref phone, ref _emailAddress, ref _fax);
            }
            catch (System.Exception ex) {
                throw (ex);
            }

            if (memberID > 0) {

                MemberID = memberID;
                AddressID = addressID;
                SHID = Convert.ToInt32(shid);

            } else {
                ResetShareholder();
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
