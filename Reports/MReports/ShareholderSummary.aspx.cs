using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Reports.MReports {

    public partial class ShareholderSummary : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.ShareholderSummary.";

        protected void Page_Load(object sender, EventArgs e) {

            //const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((MasterReportTemplate)Master).LocPDF = "";

            // Sink the Master page event, PrintReady
            ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                string shidEntry = txtSsSHID.Text.Replace(" ", "");
                ArrayList alst = new ArrayList();

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");
                string cropYear = ((MasterReportTemplate)Master).CropYear;

                if (shidEntry.IndexOf("-") != -1) {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        string[] sRange = shidEntry.Split(new char[] { '-' });
                        if (sRange.Length == 2) {
                            string fromShid = sRange[0];
                            string toShid = sRange[1];
                            alst = WSCField.GrowerPerformanceShidsByRange(conn, Convert.ToInt32(cropYear), fromShid, toShid);
                        }
                    }
                } else {
                    if (shidEntry.IndexOf(",") != -1) {
                        string[] tmp = shidEntry.Split(new char[] { ',' });
                        foreach (string s in tmp) {
                            alst.Add(s);
                        }

                    } else {
                        if (shidEntry.Length > 0) {
                            alst.Add(shidEntry);
                        }
                    }
                }

                string busName = "";
                string pdf = WSCReports.rptShareholderSummary.ReportPackager(Convert.ToInt32(cropYear), alst, busName, "", "", "", "", "", auth.UserName, fileName, logoUrl, pdfTempFolder);

                if (alst.Count > 0) {
                    if (pdf.Length > 0) {
                        // convert file system path to virtual path
                        pdf = pdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                    }
                }

                ((MasterReportTemplate)sender).LocPDF = pdf;
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
