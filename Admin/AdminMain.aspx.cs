using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class AdminMain : Common.BasePage {

        private const string MOD_NAME = "Admin.AdminMain.";

        protected void Page_Load(object sender, EventArgs e) {
        }

        protected void mnuLocal_PreRender(object sender, EventArgs e) {

            foreach (MenuItem itm in mnuLocal.Items) {
                string tt = itm.ToolTip;
                itm.ToolTip = "";
                if (tt.Length > 0) {
                    divMenuDesc.Controls.Add(new LiteralControl("<span>" + tt + "</span>" + "<br />"));
                }
            }

        }
    }
}
