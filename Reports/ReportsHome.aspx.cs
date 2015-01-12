using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WSCIEMP.Reports {

    public partial class ReportsHome : Common.BasePage {

        protected void Page_Load(object sender, EventArgs e) {

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
