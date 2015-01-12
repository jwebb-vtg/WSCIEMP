using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.Reports.MReports {

    public partial class LandownerLetter : Common.BasePage {

        private const string MOD_NAME = "Reports.MReports.LandownerLetter.";
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
                    FillFactory();
                    FillStation();
                    FillContract();
                } else {

                    //-------------------------------------------------------------------------
                    // Getting each list from the native control is odd, but having the
                    // selections as CSV type strings if very handy.
                    //-------------------------------------------------------------------------
                    System.Collections.Specialized.NameValueCollection nv = Page.Request.Form;

                    _factoryList = nv[lstLlFactory.ClientID.Replace("_", "$")];
                    _stationList = nv[lstLlStation.ClientID.Replace("_", "$")];
                    _contractList = nv[lstLlContract.ClientID.Replace("_", "$")];

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
                ListBox lst = lstLlFactory;
                lst.Items.Clear();

                string cropYear = ((MasterReportTemplate)Master).CropYear;
                List<ListBeetFactoryIDItem> stateList = BeetDataDomain.BeetFactoryIDGetList(Convert.ToInt32(cropYear));

                System.Text.StringBuilder sbList = new System.Text.StringBuilder("");
                foreach (ListBeetFactoryIDItem state in stateList) {

                    ListItem item = new ListItem(state.FactoryLongName, state.FactoryID);
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

                ArrayList listIDs = new System.Collections.ArrayList(_stationList.Split(new char[] { ',' }));
                ListBox lst = lstLlStation;
                lst.Items.Clear();

                List<ListBeetStationIDItem> stateList = WSCReportsExec.StationListGetByFactory(_factoryList);

                System.Text.StringBuilder sbList = new System.Text.StringBuilder("");
                foreach (ListBeetStationIDItem state in stateList) {

                    ListItem item = new ListItem(state.StationNumberName, state.StationID);
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

                ArrayList listIDs = new System.Collections.ArrayList(_contractList.Split(new char[] { ',' }));
                ListBox lst = lstLlContract;
                lst.Items.Clear();

                List<ListBeetContractIDItem> stateList = WSCReportsExec.ContractListByStations(_stationList);

                foreach (ListBeetContractIDItem state in stateList) {

                    ListItem item = new ListItem(state.ContractNumber, state.ContractID);
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

        private void DoPrintReady(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoPrintReady";

            try {

                WSCSecurity auth = Globals.SecurityState;
                string logoUrl = Page.MapPath(WSCReportsExec.GetReportLogo());
                string pdfTempFolder = Page.MapPath(WSCReportsExec.GetPDFFolderPath());
                string fileName = auth.UserID + "_" + ((MasterReportTemplate)Master).ReportName.Replace(" ", "");
                string cropYear = ((MasterReportTemplate)Master).CropYear;

                string letterDate = txtLlLetterDate.Text;
                DateTime dtLetterDate = DateTime.Now;
                try {
                    dtLetterDate = Convert.ToDateTime(letterDate);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid Leter Date.");
                    throw(warn);
                }
                string deadlineDate = txtLlDeadlineDate.Text;
                DateTime dtDeadlineDate = DateTime.Now;
                try {
                    dtDeadlineDate = Convert.ToDateTime(deadlineDate);
                }
                catch {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid Deadline Date.");
                    throw(warn);
                }

                string pdf = WSCReports.rptLandownerLetter.ReportPackager(Convert.ToInt32(cropYear), dtLetterDate, dtDeadlineDate, 
                    _factoryList, _stationList, _contractList, fileName, logoUrl, pdfTempFolder);

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

        protected void lstLlFactory_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "lstLlFactory_SelectedIndexChanged";

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

        protected void lstLlStation_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "lstLlStation_SelectedIndexChanged";

            try {
                FillContract();
                uplCriteria.Update();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex, "Unable to load page correctly at this time.", divAjaxWarning);
            }
        }

        private void DoCropYearChange(object sender, CommandEventArgs e) {

            const string METHOD_NAME = "DoCropYearChange";

            try {

                FillFactory();
                FillStation();
                FillContract();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((MasterReportTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
