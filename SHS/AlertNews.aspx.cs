using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using WSCData;

namespace WSCIEMP.SHS {
    public partial class AlertNews : Common.BasePage {

        private const string MOD_NAME = "SHS.AlertNews.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";
            string filePath = "";
            DateTime curDate = DateTime.Now;

            try {

                using (DataSet dsNews = new DataSet()) {
                    filePath = MapPath(@"~/ZHost/XML/AlertNews.xml");
                    dlResults.Visible = false;

                    if (File.Exists(filePath)) {

                        dsNews.ReadXml(filePath);

                        // Condition the data as needed.
                        if (dsNews.Tables["AlertItem"].Rows.Count > 0) {

                            foreach (DataRow row in dsNews.Tables["AlertItem"].Rows) {

                                if (row["url"].ToString().Length > 0) {

                                    string url = row["Url"].ToString();
                                    row["Url"] = Page.ResolveUrl(url);

                                    FileInfo fi = new FileInfo(MapPath(url));

                                    if (row["UpdateDate"].ToString().Length == 0) {
                                        row["UpdateDate"] = fi.LastWriteTime.ToString("MMMM dd, yyyy");
                                    }
                                }

                                if (row["UpdateDate"].ToString().Length == 0) {
                                    row["UpdateDate"] = curDate.ToString("MMMM dd, yyyy");
                                }
                            }

                            DataView dvNews = dsNews.Tables["AlertItem"].DefaultView;
                            dvNews.RowFilter = "IsActive = 1";

                            if (dvNews.Count > 0) {
                                dlResults.DataSource = dvNews;
                                dlResults.DataBind();
                                dlResults.Visible = true;
                            }
                        }

                    }

                    if (dlResults.Visible == false) {
                        divResults.Attributes.Add("style", "text-align: center;");
                        divResults.InnerHtml = "<br /><br /><br /><br /><br />Sorry, there are no current news items.<br /><br /><br /><br /><br /><br /><br /><br /><br />" +
                            "<br /><br /><br /><br />";
                    }
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
