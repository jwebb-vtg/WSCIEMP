using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Reports.MReports {

    public partial class Certificate : System.Web.UI.Page {

        private const string MOD_NAME = "Reports.MReports.Certificate.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((MasterReportTemplate)Master).LocPDF = "";

            // Sink the Master page event, PrintReady
            ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);

            try {

                if (!Page.IsPostBack) {
                    FillEquityType();
                    FillSigDetails();
                }

                btnDeletePDF.Enabled = (((MasterReportTemplate)Page.Master).LocLastPDF.Length > 0);
            }
            catch (System.Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillEquityType() {

            ddlEquityType.Items.Clear();
            ddlEquityType.Items.Add(new ListItem("Patronage", "1"));
            ddlEquityType.Items.Add(new ListItem("Unit Retain", "2"));
            ddlEquityType.SelectedIndex = 0;
        }

        private void FillSigDetails() {

            const string METHOD_NAME = "FillSigDetails";

            try {
                
                string sigInfoPath = Page.MapPath("~/ZHost/XML/CertificateSignature.xml");

                if (!System.IO.File.Exists(sigInfoPath)) {
                    Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME + ": Notify your Network Administrator the required file CertificateSignature.xml is missing.");
                    throw (wex);
                }

                using (System.Data.DataSet dsSigInfo = new System.Data.DataSet()) {

                    dsSigInfo.ReadXml(sigInfoPath);
                    lblSigName.Text = dsSigInfo.Tables[0].Rows[0]["SigName"].ToString() ;
                    lblSigTitle.Text = dsSigInfo.Tables[0].Rows[0]["SigTitle"].ToString();
                }
            }
            catch (Exception ex) {
                Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogoIconOnly());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "").Replace(":", "_");
                string cropYear = ((MasterReportTemplate)Master).CropYear;
                string shid = txtPsSHID.Text;
                string fromShid = txtPsFromSHID.Text;
                string toShid = txtPsToSHID.Text;
                string sigImagePath = Page.MapPath("~/ZHost/Misc/CertificateSignature.gif");
                string sigName = lblSigName.Text;
                string sigTitle = lblSigTitle.Text;

                //-----------------------------------------------------
                // Given a specific shid, erase any range query.
                //-----------------------------------------------------
                if (shid.Length > 0) {
                    fromShid = "";
                    toShid = "";
                    txtPsFromSHID.Text = fromShid;
                    txtPsToSHID.Text = toShid;
                }

                string equityType = (Common.UILib.GetDropDownValue(ddlEquityType) == "1" ? "PAT" : "RET");

                string certificateDate = txtCertificateDate.Text.Trim();
                if (certificateDate.Length == 0) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "You need to enter a Certificate Date.");
                    return;
                }
                DateTime dtCertificateDate = DateTime.Now;
                try {
                    dtCertificateDate = Convert.ToDateTime(certificateDate);
                }
                catch {
                    Common.CWarning warn = new Common.CWarning("Please enter a valid Certificate Date.");
                    throw (warn);
                }

                string pdf = WSCReports.rptCertificate.ReportPackager(Convert.ToInt32(cropYear), equityType, dtCertificateDate,
                    shid, fromShid, toShid, fileName, logoUrl, pdfTempFolder, sigName, sigTitle, sigImagePath);

                if (pdf.Length > 0) {
                    // convert file system path to virtual pathb
                    pdf = pdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                }

                ((MasterReportTemplate)sender).LocPDF = pdf;
                btnDeletePDF.Enabled = true;

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
			}
        }

        protected void btnDeletePDF_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDeletePDF_Click";

            try {

                //------------------------------------------
                // Delete the PDF File.
                //------------------------------------------
                string locLastPdf = ((MasterReportTemplate)Page.Master).LocLastPDF;
                string fileName = Page.MapPath(locLastPdf);

                if (System.IO.File.Exists(fileName)) {
                    System.IO.File.Delete(fileName);
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Successfully deleted " + fileName + ".");
                } else {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("PDF file not found on file system.");
                    throw (warn);
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
