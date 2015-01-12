using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.SHS {

    public partial class ShsHome : Common.BasePage {

        private const string MOD_NAME = "SHS.ShsHome.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                CleanUpTempFolder();

                if (Globals.WarningMessage.Length > 0) {
                    string warn = Globals.WarningMessage;
                    Globals.WarningMessage = "";
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), warn);                    
                }
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

        private void CleanUpTempFolder() {

            try {

                if (!Globals.IsTempCleanUpDone) {

                    Globals.IsTempCleanUpDone = true;
                    string baseReportPath = Server.MapPath("~/PDF");

                    //====================================================
                    // Clean up all old files in PDF folder
                    //====================================================
                    System.IO.DirectoryInfo pdfDir = new System.IO.DirectoryInfo(baseReportPath);
                    DateTime minDate = DateTime.Now.AddMinutes(-10);

                    System.IO.FileInfo[] pdfFiles = pdfDir.GetFiles("*.*");
                    foreach (System.IO.FileInfo fi in pdfFiles) {
                        if (fi.CreationTime < minDate) {
                            System.IO.File.Delete(fi.FullName);
                        }
                    }
                }
            }
            catch {
                // IGNORE ERRORS !
            }
        }
    }
}
