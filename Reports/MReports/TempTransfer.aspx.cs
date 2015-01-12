using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.MReports {

    public partial class TempTransfer : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.TempTransfer.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {
                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                ((MasterReportTemplate)Master).LocPDF = "";

                // Sink the Master page event, PrintReady
                ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            //const string METHOD_NAME = "DoPrintReady";

            WSCSecurity auth = Globals.SecurityState;
            string cropYear = ((MasterReportTemplate)Master).CropYear;
            string pdf = ConfigurationManager.AppSettings["appControl.rptServer"].ToString() +
                "?RPTSEL=TEMPTRANSFER" +
                "&USR=" + Globals.SecurityState.UserName +
                "&PRMS=" + Common.CodeLib.EncodeString(ConfigurationManager.ConnectionStrings["BeetConn"].ToString()) +
                "^" + cropYear +
                "^" + cropYear +
                "^0" +
                "^0" +
                "^";

            if (pdf.Length > 0) {
                // convert file system path to virtual path
                pdf = pdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
            }

            ((MasterReportTemplate)sender).LocPDF = pdf;

        }
    }
}
