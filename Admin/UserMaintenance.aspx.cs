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

namespace WSCIEMP.Admin {

    public partial class UserMaintenance : Common.BasePage {

        private const string MOD_NAME = "Admin.UserMaintenance.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                if (Page.IsPostBack) {

                    // =====================================
                    // Take ACTION: perform prep work here.
                    // =====================================
                    string action = txtAction.Text;
                    txtAction.Text = "";

                    switch (action) {

                        case "FindUser":

                            // Check for user id
                            if (txtUserID.Text.Length > 0) {

                                int userID = Convert.ToInt32(txtUserID.Text);
                                txtUserID.Text = "";

                                ShowUserDetail(userID);

                            }
                            break;
                    }
                }


            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void ClearUserDetail() {

            txtUserName.Text = "";
            txtUserID.Text = "";
            txtDisplayName.Text = "";
            txtPhoneNumber.Text = "";

        }

        private void ShowUserDetail(int userID) {

            ClearUserDetail();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCEmployee.UserGetByID(conn, userID)) {

                        if (dr.Read()) {

                            txtUserName.Text = dr.GetString(dr.GetOrdinal("usr_login_name"));
                            txtUserID.Text = dr.GetInt16(dr.GetOrdinal("usr_user_id")).ToString();
                            txtDisplayName.Text = dr.GetString(dr.GetOrdinal("usr_display_name"));
                            txtPhoneNumber.Text = dr.GetString(dr.GetOrdinal("usr_phone_number"));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("UserMaintenance.ShowUserDetail", ex);
                throw (wex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            WSCSecurity auth = Globals.SecurityState;
            if (auth.SecurityGroupName.ToUpper().IndexOf("AG ADMIN") == -1) {
                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                return;
            }

            try {

                string phoneNumber = Common.CodeLib.FormatPhoneNumber2Db(txtPhoneNumber.Text);

                if (phoneNumber.Length != 0 && phoneNumber.Length != 10) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "The phone number must contain 10 digits.");
                    return;
                }
                int userID = 0;
                if (txtUserID.Text.Length > 0) {
                    userID = Convert.ToInt32(txtUserID.Text);
                }
				//if (userID == 0) {
				//    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You MUST use the find button, '...', to lookup this user before editing his information.");
				//    return;
				//}
                WSCEmployee.UserSave(ref userID, txtUserName.Text, txtDisplayName.Text,
                    phoneNumber, Globals.SecurityState.UserName);

                if (userID > 0) {
					Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Save was Successful.");
                    ShowUserDetail(userID);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
