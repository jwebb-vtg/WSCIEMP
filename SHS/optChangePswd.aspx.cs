using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.SHS {
    public partial class optChangePswd : Common.BasePage {

        private const string MOD_NAME = "SHS.optChangePswd.";
        private WSCShsData _shs = null;
        private string _busName = "";        

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                _shs = Globals.ShsData;

                if (Globals.IsUserPermissionReadOnly((RolePrincipal)User)) {
                    btnChange.Enabled = false;
                }

                if (!Common.CodeLib.IsValidSHID(txtSHID.Text)) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID and press the Find button.");
                    throw (warn);
                }

                if (!Page.IsPostBack) {
                    FillCropYear();
                    FindAddress(_shs.SHID.ToString());
                }
            }
            catch (System.Exception ex) {

                ResetShareholder();
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillCropYear() {

            System.Collections.ArrayList cropYears = WSCField.GetCropYears();
            CropYear = Convert.ToInt32(cropYears[0].ToString());
        }

        protected void btnChange_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnChange_Click";

            try {

                // Did page load already throw an error?
                if (((HtmlGenericControl)Master.FindControl("divWarning")).InnerHtml.Length > 0) {
                    return;
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

                    Common.CWarning warn = new Common.CWarning("Sorry, you are not authorized to update this information");
                    throw (warn);
                }

                if (EmailAddress.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please create an email address for this member.");
                    throw (warn);
                }

                string warnMsg = null;
                WSCMember.ResetPassword(SHID.ToString(), EmailAddress, ref warnMsg);
                if (warnMsg == null) {
                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Password has been reset and sent to you via email.");
                } else {
                    Common.CWarning warn = new Common.CWarning(warnMsg);
                    throw (warn);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }

        }

        protected void btnFind_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnFind_Click";

            string shid = txtSHID.Text;

            try {
                if (((HtmlGenericControl)Master.FindControl("divWarning")).InnerHtml.Length == 0) {
                    FindAddress(shid);
                }
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
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
                lblEmail.Text = _emailAddress;
            }
            catch (System.Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        public void ResetMemberInfo(string shid) {

            int memberID = 0, addressID = 0;
            string phone = "", fax = "", emailAddress = "";

            try {
                WSCMember.GetInfo(shid, ref memberID,
                    ref addressID, ref _busName, ref phone, ref emailAddress, ref fax);
            }
            catch (System.Exception ex) {
                throw (ex);
            }

            if (memberID > 0) {

                MemberID = memberID;
                AddressID = addressID;
                SHID = Convert.ToInt32(shid);
                EmailAddress = emailAddress;

            } else {
                ResetShareholder();
            }
        }

        private void ResetShareholder() {

            _shs.ResetShareholder();
            SHID = 0;
            MemberID = 0;
            AddressID = 0;
            EmailAddress = "";
            txtSHID.Text = "0";
            lblBusName.Text = "";
            lblEmail.Text = "";
            _busName = "";            
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
        private string _emailAddress = null;
        private string EmailAddress {

            set {
                ViewState["EmailAddress"] = value;
                _emailAddress = value;
            }
            get {
                if (_emailAddress == null) {
                    _emailAddress = (ViewState["EmailAddress"] != null ? ViewState["EmailAddress"].ToString() : "");
                }
                return _emailAddress;
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
