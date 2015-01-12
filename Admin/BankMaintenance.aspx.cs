using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class BankMaintenance : Common.BasePage {

        private const string MOD_NAME = "Admin.BankMaintenance.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                btnDelete.Attributes.Add("onclick", "CheckDelete(event);");

                if (!Page.IsPostBack) {
                    FillDomainData();
                } else {

                    // =====================================
                    // Take ACTION: perform prep work here.
                    // =====================================
                    string action = txtAction.Text;
                    txtAction.Text = "";

                    switch (action) {

                        case "FindBank":

                            // Check for bank id
                            if (txtBankID.Text.Length > 0) {

                                if (txtBankID.Text.Length > 0) {
                                    try {
                                        Int32 tmp = Int32.Parse(txtBankID.Text);
                                    }
                                    catch {
                                        Common.CWarning warn = new Common.CWarning("You did not enter a reasonable bank number.");
                                        throw (warn);
                                    }
                                }
                                int bankID = Convert.ToInt32(txtBankID.Text);
                                txtBankID.Text = "";

                                ShowBankDetail(bankID);

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

        private void FillDomainData() {

            try {

                ddlState.Items.Clear();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCAdmin.StateGetAll(conn)) {

                        ddlState.Items.Add("");
                        while (dr.Read()) {
                            ddlState.Items.Add(dr.GetString(dr.GetOrdinal("ste_abbr")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + "FillDomainData", ex);
                throw (wex);
            }
        }

        private void ClearBankDetail() {

            NewBankAlertOff();
            Common.UILib.SelectDropDown(ddlState, -1);
            txtBankName.Text = "";
            txtBankID.Text = "";
            txtShortName.Text = "";
            txtBankNumber.Text = "";
            txtAddrLine1.Text = "";
            txtAddrLine2.Text = "";
            txtCity.Text = "";
            txtZip.Text = "";
            txtContactName.Text = "";
            txtPhone.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtOther.Text = "";
            chkIsActive.Checked = false;
        }

        private void NewBankAlertOn() {

            lblNewBank.Text = " *** New Bank ***";
            lblNewBank.CssClass = "WarningOn";
        }

        private void NewBankAlertOff() {

            lblNewBank.Text = "";
            lblNewBank.CssClass = "WarningOff";
        }

        private void ShowBankDetail(int bankID) {

            ClearBankDetail();

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCBank.BankGetByID(conn, bankID)) {

                        if (dr.Read()) {

                            txtBankName.Text = dr.GetString(dr.GetOrdinal("bnk_name"));
                            txtBankID.Text = dr.GetInt32(dr.GetOrdinal("bnk_bank_id")).ToString();
                            txtShortName.Text = dr.GetString(dr.GetOrdinal("bnk_short_name"));
                            txtBankNumber.Text = dr.GetString(dr.GetOrdinal("bnk_number"));
                            txtAddrLine1.Text = dr.GetString(dr.GetOrdinal("bnk_addr_line1"));
                            txtAddrLine2.Text = dr.GetString(dr.GetOrdinal("bnk_addr_line2"));
                            txtCity.Text = dr.GetString(dr.GetOrdinal("bnk_city"));

                            Common.UILib.SelectDropDown(ddlState, dr.GetString(dr.GetOrdinal("bnk_state")));

                            txtZip.Text = dr.GetString(dr.GetOrdinal("bnk_postal_code"));
                            txtContactName.Text = dr.GetString(dr.GetOrdinal("bnk_contact_name"));
                            txtPhone.Text = dr.GetString(dr.GetOrdinal("bnk_phone"));
                            txtFax.Text = dr.GetString(dr.GetOrdinal("bnk_fax"));
                            txtEmail.Text = dr.GetString(dr.GetOrdinal("bnk_email"));
                            txtOther.Text = dr.GetString(dr.GetOrdinal("bnk_other"));
                            chkIsActive.Checked = dr.GetBoolean(dr.GetOrdinal("bnk_is_active"));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + "ShowBankDetail", ex);
                throw (wex);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnNew_Click";
            try {
                ClearBankDetail();
                NewBankAlertOn();
                chkIsActive.Checked = true;
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDelete_Click";

            WSCSecurity auth = Globals.SecurityState;
            if (auth.SecurityGroupName.ToUpper().IndexOf("AG ADMIN") == -1) {
                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Sorry, you are not authorized to update this information");
                return;
            }

            try {
                int bankID = 0;
                if (txtBankID.Text.Length > 0) {
                    bankID = Convert.ToInt32(txtBankID.Text);
                }
                WSCBank.BankDelete(bankID, Globals.SecurityState.UserName);

                ClearBankDetail();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
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
                int bankID = 0;
                if (txtBankID.Text.Length > 0) {
                    bankID = Convert.ToInt32(txtBankID.Text);
                }
                WSCBank.BankSave(ref bankID, txtBankName.Text, txtBankNumber.Text,
                    txtShortName.Text, txtAddrLine1.Text, txtAddrLine2.Text, txtCity.Text,
                    Common.UILib.GetDropDownText(ddlState), txtZip.Text, txtContactName.Text, txtPhone.Text, txtFax.Text,
                    txtEmail.Text, txtOther.Text, chkIsActive.Checked, (Globals.SecurityState).UserName);

                if (bankID > 0) {
                    ShowBankDetail(bankID);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
