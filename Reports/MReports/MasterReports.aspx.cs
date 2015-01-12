using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WSCIEMP.Reports.MReports {

    public partial class MasterReports : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.MasterReports.";

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
