using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.BeetAccounting {

    public partial class BeetAccountingHome : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.BeetAccountingHome.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void mnuLocal_PreRender(object sender, EventArgs e) {

            foreach (MenuItem itm in mnuLocal.Items) {
                string tt = itm.ToolTip;
                itm.ToolTip = "";
                if (tt.Length > 0) {
                    divMenuDesc.Controls.Add(new LiteralControl("<span>" + tt + "</span>" + "<br />"));
                }

                foreach (MenuItem child in itm.ChildItems) {
                    string childTip = child.ToolTip;
                    child.ToolTip = "";
                    if (childTip.Length > 0) {
                        divMenuDesc.Controls.Add(new LiteralControl("<span>" + childTip + "</span>" + "<br />"));
                    }
                }
            }
        }
    }
}
