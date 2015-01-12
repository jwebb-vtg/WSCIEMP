using System;
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

    public partial class Transmittal : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.Transmittal.";
        private string _factoryList = "";
        private string _stationList = "";
        private string _contractList = "";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
			Common.AppHelper.HideWarning(divAjaxWarning);

            ((MasterReportTemplate)Master).LocPDF = "";

            ((MasterReportTemplate)Page.Master).CropYearChange += new CommandEventHandler(DoCropYearChange);

            // Sink the Master page event, PrintReady
            ((MasterReportTemplate)Page.Master).PrintReady += new CommandEventHandler(DoPrintReady);

            try {

                if (!Page.IsPostBack) {
                    FillPaymentDesc();
                    FillFactory();
                    FillStation();
                    FillContract();
                } else {

                    //-------------------------------------------------------------------------
                    // Getting each list from the native control is odd, but having the
                    // selections as CSV type strings if very handy.
                    //-------------------------------------------------------------------------
                    System.Collections.Specialized.NameValueCollection nv = Page.Request.Form;

                    _factoryList = nv[lstTxFactory.ClientID.Replace("_", "$")];
                    _stationList = nv[lstTxStation.ClientID.Replace("_", "$")];
                    _contractList = nv[lstTxContract.ClientID.Replace("_", "$")];

                    if (_factoryList == null) { _factoryList = ""; }
                    if (_stationList == null) { _stationList = ""; }
                    if (_contractList == null) { _contractList = ""; }
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillFactory() {

            const string METHOD_NAME = "FillFactory";

            try {

                ArrayList listIDs = new System.Collections.ArrayList(_factoryList.Split(new char[] { ',' }));
                ListBox lst = lstTxFactory;
                lst.Items.Clear();

                string cropYear = ((MasterReportTemplate)Master).CropYear;
                List<ListBeetFactoryIDItem> stateList = BeetDataDomain.BeetFactoryIDGetList(Convert.ToInt32(cropYear));

                System.Text.StringBuilder sbList = new System.Text.StringBuilder("");
                foreach (ListBeetFactoryIDItem state in stateList) {

                    ListItem item = new ListItem(state.FactoryLongName, state.FactoryNumber);
                    lst.Items.Add(item);
                    if (listIDs.Contains(item.Value)) {

                        lst.Items[lst.Items.Count - 1].Selected = true;

                        // Rebuild factoryList because the listIDs are now qualified
                        sbList.Append(item.Value + ",");
                    }
                }
                if (sbList.Length > 0) {
                    sbList.Length = sbList.Length - 1;
                }
                _factoryList = sbList.ToString();
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillStation() {

            const string METHOD_NAME = "FillStation";

            try {

                string cropYear = ((MasterReportTemplate)Master).CropYear;
                ArrayList listIDs = new System.Collections.ArrayList(_stationList.Split(new char[] { ',' }));
                ListBox lst = lstTxStation;
                lst.Items.Clear();

                List<ListBeetStationIDItem> stateList = WSCReportsExec.StationGetByFactoryNo(Convert.ToInt32(cropYear), _factoryList);

                System.Text.StringBuilder sbList = new System.Text.StringBuilder("");
                foreach (ListBeetStationIDItem state in stateList) {

                    ListItem item = new ListItem(state.StationNumberName, state.StationNumber);
                    lst.Items.Add(item);
                    if (listIDs.Contains(item.Value)) {

                        lst.Items[lst.Items.Count - 1].Selected = true;

                        // Rebuild stationList because the listIDs is now qualified against
                        // stations assigned to the valid factory list.
                        sbList.Append(item.Value + ",");
                    }
                }
                if (sbList.Length > 0) {
                    sbList.Length = sbList.Length - 1;
                }
                _stationList = sbList.ToString();

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillContract() {

            const string METHOD_NAME = "FillContract";

            try {

                string cropYear = ((MasterReportTemplate)Master).CropYear;
                ArrayList listIDs = new System.Collections.ArrayList(_contractList.Split(new char[] { ',' }));
                ListBox lst = lstTxContract;
                lst.Items.Clear();

                List<ListBeetContractIDItem> stateList = WSCReportsExec.ContractsByContractStationNo(Convert.ToInt32(cropYear), _stationList);

                foreach (ListBeetContractIDItem state in stateList) {

                    ListItem item = new ListItem(state.ContractNumber, state.ContractNumber);
                    lst.Items.Add(item);
                    if (listIDs.Contains(item.Value)) {
                        lst.Items[lst.Items.Count - 1].Selected = true;
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillPaymentDesc() {

            const string METHOD_NAME = "FillPaymentDesc";
            try {

                ddlTxPaymentDesc.Items.Clear();
                ddlTxStatementDate.Items.Clear();
                txtStatementDate.Text = "";
                string cropYear = ((MasterReportTemplate)Master).CropYear;

                List<ListPaymentDescItem> stateList = BeetDataDomain.GetPaymentDescriptions(Convert.ToInt32(cropYear), false);

                foreach (ListPaymentDescItem state in stateList) {

                    // Only show transmitted payments.
                    if (state.TransmittalDate.Length > 0) {

                        int paymentNumber =Convert.ToInt32(state.PaymentNumber);
                        string paymentDesc = state.PaymentDesc;

                        ListItem liDesc = new ListItem(paymentDesc, paymentNumber.ToString());
                        ddlTxPaymentDesc.Items.Add(liDesc);

                        ddlTxStatementDate.Items.Add(state.TransmittalDate);
                    }
                }

                // Handle empty controls.
                if (ddlTxPaymentDesc.Items.Count == 0) {
                    ddlTxPaymentDesc.Items.Add("None Available");
                    ddlTxStatementDate.Items.Add(" ");
                    txtStatementDate.Text = " ";
                }

                if (ddlTxPaymentDesc.Items.Count > 0) {
                    ddlTxPaymentDesc.SelectedIndex = 0;
                    ddlTxStatementDate.SelectedIndex = 0;
                    txtStatementDate.Text = Common.UILib.GetDropDownText(ddlTxStatementDate);
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void DoCropYearChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoCropYearChange";

            try {

                FillPaymentDesc();
                FillFactory();
                FillStation();
                FillContract();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void lstTxFactory_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "lstTxFactory_SelectedIndexChanged";

            try {

                FillStation();
                FillContract();
                uplCriteria.Update();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", divAjaxWarning);
            }
        }

        protected void lstTxStation_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "lstTxStation_SelectedIndexChanged";

            try {
                FillContract();
                uplCriteria.Update();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", divAjaxWarning);
            }
        }

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");
                string cropYear = ((MasterReportTemplate)Master).CropYear;

                // Minimally you must at least pick a factory
                if (_factoryList.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("You must select at least one Factory");
                    throw (warn);
                }

                // Must have selected a payment ID
                if (ddlTxPaymentDesc.SelectedItem == null) {
                    Common.CWarning warn = new Common.CWarning("You must select a payment");
                    throw (warn);
                }
                string paymentDescription = Common.UILib.GetDropDownText(ddlTxPaymentDesc);
                if (paymentDescription.StartsWith("None")) {
                    Common.CWarning warn = new Common.CWarning("Please select a Payment having a Payment Date.");
                    throw (warn);
                }

                int paymentNumber = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlTxPaymentDesc));                
                string statementDate = txtStatementDate.Text;

                // Must enter a valid from date
                string fromDate = txtTxFromDate.Text;
                if (fromDate.Length == 0 || !Common.CodeLib.IsDate(fromDate)) {
                    Common.CWarning warn = new Common.CWarning("You must enter a valid From Date.");
                    throw (warn);
                }

                // Must enter a valid to date
                string toDate = txtTxToDate.Text;
                if (toDate.Length == 0 || !Common.CodeLib.IsDate(toDate)) {
                    Common.CWarning warn = new Common.CWarning("You must enter a valid To Date.");
                    throw (warn);
                }

                bool isCumulative = chkTxIsTransmittalCumulative.Checked;

                string pdf = WSCReports.rptTransmittal.ReportPackager(Convert.ToInt32(cropYear), paymentNumber, paymentDescription,
                    fromDate, toDate, statementDate, _factoryList,
                    _stationList, _contractList, isCumulative, fileName, logoUrl, pdfTempFolder);

                if (pdf.Length > 0) {
                    // convert file system path to virtual path
                    pdf = pdf.Replace(Common.AppHelper.AppPath(), Page.ResolveUrl("~")).Replace(@"\", @"/");
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
