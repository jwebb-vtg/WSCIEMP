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

    public partial class BankFinder : Common.BasePage {

        private const string MOD_NAME = "Admin.BankFinder.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning(divWarning);

                // This is really GREAT !!!
                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes.Add("type", "text/javascript");
                script.Attributes.Add("src", Page.ResolveUrl("~/Script/Common.js"));
                Page.Header.Controls.Add(script);

                if (!Page.IsPostBack) {

                    radBankName.Checked = true;
                    chkIsActive.Checked = true;

                    // Get passed parameters
                    System.Collections.Specialized.NameValueCollection queryStr = Request.QueryString;
                    string bankName = queryStr["bankName"];
                    string bankNumber = queryStr["bankNumber"];

                    if (bankName.Length > 0) {
                        txtCriteria.Text = bankName;
                    } else {
                        radBankName.Checked = false;
                        radBankNumber.Checked = true;
                        txtCriteria.Text = bankNumber;
                    }
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnFind_Click";

            try {
                ShowBankResults();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void ShowBankResults() {

            try {

                grdResults.DataSource = null;
                grdResults.DataBind();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (radBankName.Checked) {

                        using (SqlDataReader dr = WSCBank.BankFindName(conn, txtCriteria.Text, chkIsActive.Checked)) {

                            grdResults.DataSource = dr;
                            grdResults.DataBind();
                        }
                    } else {

                        if (txtCriteria.Text.Length > 0) {
                            try {
                                Int32 tmp = Int32.Parse(txtCriteria.Text);
                            }
                            catch {
                                Common.AppHelper.ShowWarning(divWarning, "You did not enter a reasonable bank number.");
                                return;
                            }
                        }
                        using (SqlDataReader dr = WSCBank.BankFindNumber(conn, txtCriteria.Text, chkIsActive.Checked)) {

                            grdResults.DataSource = dr;
                            grdResults.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + "ShowBankResults", ex);
                throw (wex);
            }
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";
                row.Attributes.Add("onclick", "SelectBank(" + row.Cells[0].Text + ");");
                //row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));

            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdResults);
            base.Render(writer);
        } 
    }
}
