using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WSCIEMP {
    public partial class _Default : Common.BasePage {
        protected void Page_Load(object sender, EventArgs e) {

            string transferPage = Page.ResolveUrl("~/SHS/ShsHome.aspx");
            //Server.Transfer(transferPage, false);
            Response.Redirect(transferPage, false);
        }
    }
}
