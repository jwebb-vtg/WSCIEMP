using System;
using System.Web;

namespace WSCIEMP.Common {

    /// <summary>
    /// Summary description for BasePage
    /// </summary>
    public class BasePage : System.Web.UI.Page {

        protected override void OnLoad(EventArgs e) {

            base.OnLoad(e);
        }

        protected override void OnError(EventArgs e) {


            // At this point we have information about the error 
            HttpContext ctx = HttpContext.Current;

            Exception exception = ctx.Server.GetLastError();

            // see if this will log any unhandled exception
            Common.AppHelper.LogException(exception, ctx);

            //ctx.Server.ClearError(); 

            base.OnError(e);
        }
    }
}