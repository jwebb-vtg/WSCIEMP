using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WSCIEMP {
    public partial class NoNavTemplate : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {

            //WSCData.Globals.HideWarning(divWarning);

            // This is really GREAT !!!
            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", Page.ResolveUrl("~/Script/Common.js"));
            Page.Header.Controls.Add(script);

        }
    }
}
