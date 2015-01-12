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

    public partial class AlertAnnouncements : Common.BasePage {

        private const string MOD_NAME = "SHS.AlertAnnouncements.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";
            string filePath = "";

            try {

                using (DataSet dsAnnounce = new DataSet()) {
                    filePath = MapPath(@"~/ZHost/XML/AlertAnnouncement.xml");
                    dlResults.Visible = false;

                    if (File.Exists(filePath)) {

                        dsAnnounce.ReadXml(filePath);

                        // Condition the data as needed.
                        if (dsAnnounce.Tables["AlertItem"].Rows.Count > 0) {

                            foreach (DataRow row in dsAnnounce.Tables["AlertItem"].Rows) {

                                if (row["UpdateDate"].ToString().Length == 0) {
                                    string url = row["Url"].ToString();
                                    FileInfo fi = new FileInfo(MapPath(url));
                                    DateTime updateDate = fi.LastWriteTime;
                                    row["UpdateDate"] = updateDate.ToString("MMMM dd, yyyy");
                                }

                                row["Url"] = Page.ResolveUrl(row["Url"].ToString());
                            }

                            DataView dvAnnounce = dsAnnounce.Tables["AlertItem"].DefaultView;
                            dvAnnounce.RowFilter = "IsActive = 1";

                            if (dvAnnounce.Count > 0) {
                                dlResults.DataSource = dvAnnounce;
                                dlResults.DataBind();
                                dlResults.Visible = true;
                            }
                        }
                    }

                    if (dlResults.Visible == false) {
                        divResults.Attributes.Add("style", "text-align: center;");
                        divResults.InnerHtml = "<br /><br /><br /><br /><br />Sorry, there are no current Announcements.<br /><br /><br /><br /><br /><br /><br /><br /><br />" +
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
