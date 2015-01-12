using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.SHS {
    public partial class optContactInfo : Common.BasePage {

        private const string MOD_NAME = "SHS.optContactInfo.";
        private WSCShsData _shs = null;
        private string _busName = "";
        private string _emailAddress = "";
        private string _fax = "";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            _shs = Globals.ShsData;

            lblEmail.CssClass = "WarningOff";
            lblEmail.Text = "";
            lblFax.CssClass = "WarningOff";
            lblFax.Text = "";

            if (Globals.IsUserPermissionReadOnly((RolePrincipal)User)) {
                btnSave.Enabled = false;
            }

            if (!Page.IsPostBack) {

                try {

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

                    } else {
                        Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter a SHID and press the Find button.");
                    }
                }
                catch (System.Exception ex) {
					Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
					((PrimaryTemplate)Page.Master).ShowWarning(ex);
                }
            } 	
        }

        private void FillCropYear() {

            System.Collections.ArrayList cropYears = WSCField.GetCropYears();
            CropYear = Convert.ToInt32(cropYears[0].ToString());
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            string email = "";
            string fax = "";

            try {

                if (!Common.CodeLib.IsValidSHID(txtSHID.Text)) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID");
                    throw (warn);
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
                string userName = auth.UserName;
                email = txtEmail.Text;
                fax = txtFax.Text;

                if (email.Length > 0 && !Common.CodeLib.ValidateEmail(email)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter the information in the correct format");
                    Common.AppHelper.ShowWarning(lblEmail, "Required to receive reports by email.");
                    return;
                }

                if (fax.Length > 0 && !Common.CodeLib.ValidateFax(fax)) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter the information in the correct format");
                    Common.AppHelper.ShowWarning(lblFax, "Required to receive reports by fax.");
                    return;
                }

                // Update the email address or fax number when necessary.
                try {
                    WSCMember.UpdateAddress(MemberID, email, fax, userName);
                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Your changes have been successfully updated!");
                }
                catch (System.Exception ex) {
                    ResetShareholder();
                    throw(ex);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
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

        private void FindAddress(string shid) {

            const string METHOD_NAME = "FindAddress";

            if (!Common.CodeLib.IsValidSHID(shid)) {

                ResetShareholder();
                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter a valid SHID.");
                return;
            }

            try {

                ResetMemberInfo(shid);

                txtSHID.Text = SHID.ToString();
                lblBusName.Text = _busName;
                txtEmail.Text = _emailAddress;
                txtFax.Text = _fax;

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
            _fax = "";
            txtFax.Text = "";
        }

        protected void btnFind_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnFind_Click";

            string shid = txtSHID.Text;

            try {
                if (Common.CodeLib.IsValidSHID(shid)) {
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
