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

namespace WSCIEMP.UControls {

    public partial class TextEntry : Common.BasePage {

        private const string MOD_NAME = "UControls.TextEntry.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning(divWarning);

                // This is really GREAT !!!
                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes.Add("type", "text/javascript");
                script.Attributes.Add("src", Page.ResolveUrl("~/Script/Common.js"));
                Page.Header.Controls.Add(script);
                
                System.Collections.Specialized.NameValueCollection nv = Page.Request.QueryString;
                MasterControlName.Text = nv["MasterControlName"];
                ActionControlName.Text = nv["ActionControlName"];
                Action.Text = nv["Action"];
                lblTextEntryLabel.Text = nv["Label"];

                string clickCtrl = (nv["clickCtrlName"] != null ? nv["clickCtrlName"].ToString() : "");
                btnOk.Attributes.Add("onclick", "DoOk('" + clickCtrl + "');");

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
