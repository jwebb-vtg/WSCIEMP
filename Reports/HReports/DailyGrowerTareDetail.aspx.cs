using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.HReports {

    public partial class DailyGrowerTareDetail : Common.BasePage {

        private const string MOD_NAME = "Reports.HReports.DailyGrowerTareDetail.";
        private string _deliveryDates = "";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
            ((HarvestReportTemplate)Master).LocPDF = "";

            // Sink the Master page events...
            ((HarvestReportTemplate)Master).PrintReady += new CommandEventHandler(DoPrintReady);
            ((HarvestReportTemplate)Master).ShidChange += new CommandEventHandler(DoShidChange);
            ((HarvestReportTemplate)Master).CropYearChange += new CommandEventHandler(DoCropYearChange);

            lstDgtdContract.Attributes.Add("onchange", "SetSubRefOptions(this," +
                "'" + lstDgtdDeliveryDate.ClientID + "', " + "'" + lstDgtdRefDeliveryDate.ClientID + "');");

            try {

                System.Collections.Specialized.NameValueCollection nv = Page.Request.Form;
                _deliveryDates = nv[lstDgtdDeliveryDate.ClientID.Replace("_", "$")];
                if (_deliveryDates == null) { _deliveryDates = ""; }

                if (!Page.IsPostBack) {
                    FillControls();
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((HarvestReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillControls() {

            const string METHOD_NAME = "FillControls";

            try {

                WSCSecurity auth = Globals.SecurityState;

                string sCntNo = Common.UILib.GetListText(lstDgtdContract, ",");
                int contractNumber = 0;
                if (sCntNo.Length > 0) {
                    contractNumber = Convert.ToInt32(sCntNo);
                }

                int memberID = ((HarvestReportTemplate)Master).MemberID;
                int cropYear = Convert.ToInt32(((HarvestReportTemplate)Master).CropYear);

                lstDgtdContract.Items.Clear();
                lstDgtdRefDeliveryDate.Items.Clear();

                List<ListBeetContractIDItem> stateList = WSCReportsExec.GetContractListSecure(memberID, cropYear, auth.UserID);
                lstDgtdContract.DataSource = stateList;
                lstDgtdContract.DataTextField = "ContractNumber";
                lstDgtdContract.DataBind();

                List<ListBeetContractDeliveryDateItem> deliveryList = WSCReportsExec.GetContractDeliveryDates(memberID, cropYear);

                // Fill reference list of all delivery dates for all contracts.
                lstDgtdRefDeliveryDate.DataSource = deliveryList;
                lstDgtdRefDeliveryDate.DataValueField = "ContractNumber";
                    lstDgtdRefDeliveryDate.DataTextField = "DeliveryDate";
                    lstDgtdRefDeliveryDate.DataBind();

                lstDgtdDeliveryDate.Items.Clear();
                if (contractNumber > 0) {

                    for (int i = 0; i < lstDgtdContract.Items.Count; i++) {
                        if (lstDgtdContract.Items[i].Text == contractNumber.ToString()) {
                            lstDgtdContract.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (lstDgtdContract.SelectedIndex == -1) {
                    contractNumber = 0;
                    _deliveryDates = "";
                }

                if (contractNumber > 0) {

                    for (int i = 0; i < lstDgtdRefDeliveryDate.Items.Count; i++) {
                        if (lstDgtdRefDeliveryDate.Items[i].Value == contractNumber.ToString()) {
                            lstDgtdDeliveryDate.Items.Add(lstDgtdRefDeliveryDate.Items[i].Text);
                        }
                    }
                }

                if (_deliveryDates.Length > 0) {
                    for (int i = 0; i < lstDgtdDeliveryDate.Items.Count; i++) {
                        if (_deliveryDates.IndexOf(lstDgtdDeliveryDate.Items[i].Text) != -1) {
                            lstDgtdDeliveryDate.Items[i].Selected = true;
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Common.CException wscEx = new Common.CException(MOD_NAME + METHOD_NAME + ": ", ex);
                throw (wscEx);
            }
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                RebuildDeliveryDates();

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((HarvestReportTemplate)Master).ReportName.Replace(" ", "");

                int cropYear = Convert.ToInt32(((HarvestReportTemplate)Master).CropYear);

                string cntNo = Common.UILib.GetListText(lstDgtdContract, ",");
                if (cntNo.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please Choose a Contract.");
                    throw (warn);
                }
                int contractNumber = Convert.ToInt32(cntNo);

                string pdf = WSCReports.rptDailyGrowerTareDetail.ReportPackager(cropYear, contractNumber, _deliveryDates, fileName, logoUrl, pdfTempFolder);

                if (pdf.Length > 0) {
                    // convert file system path to virtual path
                    pdf = pdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
                }

                ((HarvestReportTemplate)sender).LocPDF = pdf;
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((HarvestReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void RebuildDeliveryDates() {

            const string METHOD_NAME = "RebuildDeliveryDates";

            try {

                lstDgtdDeliveryDate.Items.Clear();

                string sCntNo = Common.UILib.GetListText(lstDgtdContract, ",");
                int contractNumber = 0;
                if (sCntNo.Length > 0) {
                    contractNumber = Convert.ToInt32(sCntNo);
                }

                if (contractNumber > 0) {

                    for (int i = 0; i < lstDgtdRefDeliveryDate.Items.Count; i++) {
                        if (lstDgtdRefDeliveryDate.Items[i].Value == contractNumber.ToString()) {
                            lstDgtdDeliveryDate.Items.Add(lstDgtdRefDeliveryDate.Items[i].Text);
                        }
                    }
                }

                if (_deliveryDates.Length > 0) {
                    for (int i = 0; i < lstDgtdDeliveryDate.Items.Count; i++) {
                        if (_deliveryDates.IndexOf(lstDgtdDeliveryDate.Items[i].Text) != -1) {
                            lstDgtdDeliveryDate.Items[i].Selected = true;
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Common.CException wscEx = new Common.CException(MOD_NAME + METHOD_NAME + ": ", ex);
                throw (wscEx);
            }
        }

        private void DoCropYearChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoCropYearChange";

            try {

                FillControls();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((HarvestReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void DoShidChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoShidChange";

            try {

                FillControls();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((HarvestReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
