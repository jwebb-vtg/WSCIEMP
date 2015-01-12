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
using WSCData;

namespace WSCIEMP {

    public partial class GlobalException : Common.BasePage {

        protected void Page_Load(object sender, EventArgs e) {

            HtmlGenericControl divBc = (HtmlGenericControl)Master.FindControl("breadcrumb");
            Common.AppHelper.HideWarning(divBc);

            Menu topMenu = (Menu)Master.FindControl("TopMenuBar");
            topMenu.Visible = false;

        }
    }
}
