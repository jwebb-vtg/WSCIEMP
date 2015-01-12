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

    public partial class UserFinder : Common.BasePage {

        private const string MOD_NAME = "Admin.UserFinder.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning(divWarning);

                // This is really GREAT !!!
                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes.Add("type", "text/javascript");
                script.Attributes.Add("src", Page.ResolveUrl("~/Script/Common.js"));
                Page.Header.Controls.Add(script);

                radLogonName.Checked = true;
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void ShowResults() {

            try {

                grdResults.DataSource = null;
                grdResults.DataBind();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    if (radLogonName.Checked) {

                        using (SqlDataReader dr = WSCEmployee.UserFindLogon(conn, txtCriteria.Text)) {

                            grdResults.DataSource = dr;
                            grdResults.DataBind();
                        }
                    } else {

                        using (SqlDataReader dr = WSCEmployee.UserFindDisplay(conn, txtCriteria.Text)) {

                            grdResults.DataSource = dr;
                            grdResults.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("UserFinder.ShowResults", ex);
                throw (wex);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnFind_Click";

            try {
                ShowResults();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";
                row.Attributes.Add("onclick", "SelectUser(" + row.Cells[0].Text + ");");
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
