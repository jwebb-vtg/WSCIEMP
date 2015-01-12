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

    public partial class ManageLookups : Common.BasePage {

        private const string MOD_NAME = "Admin.ManageLookups.";
        private string _action = "";
        private string _isNew = "";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                btnNew.Attributes.Add("onclick", "SetupNew(event);");
                btnSave.Attributes.Add("onclick", "CheckChange(event);");
                btnDelete.Attributes.Add("onclick", "CheckDelete(event);");

                if (!Page.IsPostBack) {
                    FillLookupTypes();
                    FillDescriptions();
                    FillDescriptionDetails();
                } else {

                    _action = txtAction.Text;
                    txtAction.Text = "";

                    _isNew = txtIsNew.Text;
                    txtIsNew.Text = "";
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillLookupTypes() {

            const string METHOD_NAME = "FillLookupTypes";

            try {

                ddlType.Items.Clear();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCAdmin.LookupGetTypesAll(conn)) {
                        while (dr.Read()) {
                            ddlType.Items.Add(dr.GetString(dr.GetOrdinal("lkp_lookup_type")));
                        }
                    }
                }

                if (ddlType.SelectedIndex == -1) {
                    ddlType.SelectedIndex = 0;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillDescriptions() {

            const string METHOD_NAME = "FillDescriptions";

            try {

                ddlDescription.Items.Clear();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, ddlType.SelectedValue, null)) {
                        while (dr.Read()) {
                            ListItem item = new ListItem(dr.GetString(dr.GetOrdinal("lkp_description")),
                                (dr.GetBoolean(dr.GetOrdinal("lkp_is_active")) == true ? "1" : "0") + "^" +
                                dr.GetString(dr.GetOrdinal("lkp_description")));
                            ddlDescription.Items.Add(item);
                        }
                    }
                }

                if (ddlDescription.SelectedIndex == -1) {
                    ddlDescription.SelectedIndex = 0;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillDescriptionDetails() {

            const string METHOD_NAME = "FillDescriptionDetails";

            try {

                string itemData = ddlDescription.SelectedItem.Value;
                string[] vals = itemData.Split(new char[] { '^' });
                bool isActive = (vals[0] == "1");

                chkIsActive.Checked = isActive;
                txtDescription.Text = vals[1];

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void DeleteLookup() {

            const string METHOD_NAME = "DeleteLookup";

            try {

                string type = ddlType.SelectedValue;
                string description = ddlDescription.SelectedValue.Substring(1);

                WSCAdmin.LookupDelete(type, description);
            }
            catch (System.Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void SaveLookup() {

            const string METHOD_NAME = "SaveLookup";

            string oldDescription = "";
            string newDescription = txtDescription.Text;
            bool fixOldValues = false;

            try {

                if (txtDescription.Text.Length == 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please enter Description text.");
                }

                if (_isNew != "NEW") {

                    // Update Description
                    string itemData = ddlDescription.SelectedItem.Value;
                    string[] vals = itemData.Split(new char[] { '^' });
                    oldDescription = vals[1];
                    fixOldValues = (_action == "UPDATE");
                }

                WSCAdmin.LookupSave(ddlType.SelectedValue, chkIsActive.Checked,
                    oldDescription, newDescription, fixOldValues);
            }
            catch (System.Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlType_SelectedIndexChanged";

            try {

                FillDescriptions();
                FillDescriptionDetails();
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlDescription_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlDescription_SelectedIndexChanged";

            try {

                FillDescriptionDetails();
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            try {

                SaveLookup();
                FillDescriptions();
                FillDescriptionDetails();
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDelete_Click";

            try {

                DeleteLookup();
                ddlDescription.SelectedIndex = 0;
                FillDescriptions();
                FillDescriptionDetails();
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
