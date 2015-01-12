using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WSCData;

namespace WSCIEMP.SHS.Rehaul {

    public partial class RehaulViewer : Common.BasePage {

        private const string MOD_NAME = "WSCIEMP.SHS.Rehaul.RehaulViewer.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                if (!Page.IsPostBack) {
                    txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    LoadControls();
                }

                // ALWAYS ensure that the date textbox is a valid date
                if (!Common.CodeLib.IsDate(txtDate.Text)) {
                    txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }

                // Lastly check if we're here because of a date change.
                if (((HiddenField)Master.FindControl("txtAction")).Value == "datechange") {

                    ((HiddenField)Master.FindControl("txtAction")).Value = "";
                    HandleDateChange(ddlFactory.SelectedValue, Convert.ToDateTime(txtDate.Text));
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void LoadControls() {

            const string METHOD_NAME = "LoadControls";

            try {

                // Fill Factory list
                List<ListIMSFactoryItem> factoryList = LimsEx.GetWSCFactoryList();
                ddlFactory.Items.Clear();
                foreach (ListIMSFactoryItem factory in factoryList) {
                    ddlFactory.Items.Add(new ListItem(factory.FactoryName, factory.FactoryNumber));
                }
                ddlFactory.SelectedIndex = 0;
                HandleFactorySelection(Common.UILib.GetDropDownValue(ddlFactory), Convert.ToDateTime(txtDate.Text));
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                Common.CException wscEx = new Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        private void HandleFactorySelection(string factoryNumber, DateTime rehaulDate) {

            const string METHOD_NAME = "HandleFactorySelection";

            try {

                int factoryID = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlFactory)) / 10;
                RehaulFactoryStationData rehaulData = LimsEx.GetRehaulFactoryStationData(factoryID, rehaulDate);

                if (rehaulData.FactoryList.IsEmpty) {
                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "No data exists for the selected date.");
                }

                TFactory factory = rehaulData.FactoryList.GetFactoryByNumber(factoryNumber);

                txtBeetsSlidLoads.Text = factory.BeetsSlidLoads;
                txtChipsDiscarded.Text = factory.ChipsDiscardedTons;
                txtChipsPctTailings.Text = factory.ChipsPercentTailings;
                txtRehaulAvgWt.Text = factory.RehaulLoadAverageWeight;
                txtYardAvgWt.Text = factory.YardLoadAverageWeight;
                txtTotalLoadsRehauled.Text = factory.StationList.TotalStationRehaulLoads;

                rptrStations.DataSource = factory.StationList.Stations;
                rptrStations.DataBind();
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                Common.CException wscEx = new Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        protected void ddlFactory_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlFactory_SelectedIndexChanged";

            try {

                string rehaulDate = txtDate.Text;
                HandleFactorySelection(Common.UILib.GetDropDownValue(ddlFactory), Convert.ToDateTime(rehaulDate));
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnAddFactor_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnAddFactor_Click";

            try {

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void HandleDateChange(string factoryNumber, DateTime rehaulDate) {

            const string METHOD_NAME = "HandleFactorySelection";

            try {

                txtDate.Text = rehaulDate.ToString("MM/dd/yyyy");
                HandleFactorySelection(factoryNumber, rehaulDate);
            }
            catch (System.Exception e) {
                string errMsg = MOD_NAME + METHOD_NAME;
                Common.CException wscEx = new Common.CException(errMsg, e);
                throw (wscEx);
            }
        }

        protected void btnPrevReportDate_Click(object sender, ImageClickEventArgs e) {

            const string METHOD_NAME = "btnPrevReportDate_Click";

            try {

                DateTime rehaulDate = Convert.ToDateTime(txtDate.Text).AddDays(-1);
                string factoryNumber = Common.UILib.GetDropDownValue(ddlFactory);
                HandleDateChange(factoryNumber, rehaulDate);
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnNextReportDate_Click(object sender, ImageClickEventArgs e) {

            const string METHOD_NAME = "btnNextReportDate_Click";

            try {

                DateTime rehaulDate = Convert.ToDateTime(txtDate.Text).AddDays(1);
                string factoryNumber = Common.UILib.GetDropDownValue(ddlFactory);
                HandleDateChange(factoryNumber, rehaulDate);
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "txtDate_TextChanged";

            try {

                string rehaulDate = txtDate.Text;
                string factoryNumber = Common.UILib.GetDropDownValue(ddlFactory);
                HandleDateChange(factoryNumber, Convert.ToDateTime(rehaulDate));
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
