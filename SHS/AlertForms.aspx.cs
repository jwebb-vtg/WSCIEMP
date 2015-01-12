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
using System.IO;
using WSCData;

namespace WSCIEMP.SHS {

    public partial class AlertForms : Common.BasePage {

        private const string MOD_NAME = "SHS.AlertAnnouncements.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";
            string filePath = "";

            try {

                using (DataSet dsForms = new DataSet()) {
                    filePath = MapPath(@"~/ZHost/XML/AlertForms.xml");
                    dlResults.Visible = false;

                    if (File.Exists(filePath)) {

                        dsForms.ReadXml(filePath);

                        // Condition the data as needed.
                        if (dsForms.Tables["AlertItem"].Rows.Count > 0) {

                            foreach (DataRow row in dsForms.Tables["AlertItem"].Rows) {

                                if (row["UpdateDate"].ToString().Length == 0) {
                                    string url = row["Url"].ToString();
                                    FileInfo fi = new FileInfo(MapPath(url));
                                    DateTime updateDate = fi.LastWriteTime;
                                    row["UpdateDate"] = updateDate.ToString("MMMM dd, yyyy");
                                }

                                row["Url"] = Page.ResolveUrl(row["Url"].ToString());
                            }

                            DataView dvForms = dsForms.Tables["AlertItem"].DefaultView;
                            dvForms.RowFilter = "IsActive = 1";

                            if (dvForms.Count > 0) {
                                dlResults.DataSource = dvForms;
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
