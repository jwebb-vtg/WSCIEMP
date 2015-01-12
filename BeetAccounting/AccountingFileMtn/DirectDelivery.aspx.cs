using System;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.BeetAccounting.AccountingFileMtn {

    public partial class DirectDelivery : System.Web.UI.Page {

        private const string MOD_NAME = "WSCIEMP.BeetAccounting.AccountingFileMtn.";
        private string _contractSearch = "";

        private enum GrdDirectDeliveryCols {
            colDirectDeliveryID = 0,
            colRowVersion,
            colDeliveryStation,
            colContractStation,
            colRatePerTon
        }

        private enum GrdContractCols {
            colDirectDeliveryID = 0,
            colContractID,
            colRowVersion,
            colContractNumber,
            colRatePerTon
        }

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";
            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                if (!Page.IsPostBack) {
                    FillControls();
                    FillGridDirectDelivery();
                    FillGridContract();
                } else {

                    _contractSearch = txtContractSearch.Text;
                    txtContractSearch.Text = "";
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillControls() {

            const string METHOD_NAME = "FillControls";

            try {

                BeetDataDomain.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                FillStations();

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ResetDirectDeliveryEdit() {

            txtEditRatePerTon.Text = "";
            ddlEditContractStation.SelectedIndex = -1;
            ddlEditDeliveryStation.SelectedIndex = -1;
        }

        private void ResetContractEdit() {

            txtEditContractRatePerTon.Text = "";
            ddlEditContractNumber.Items.Clear();
        }

        private void FillGridDirectDelivery() {

            const string METHOD_NAME = "FillGridDirectDelivery";

            try {                

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int contractStationNumber = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlCriteriaContractStation));
                int deliveryStationNumber = Convert.ToInt32(Common.UILib.GetDropDownValue(ddlCriteriaDeliveryStation));

                List<ListDirectDeliveryItem> stateList = BeetDirectDelivery.DirectDeliveryGet(cropYear, contractStationNumber, deliveryStationNumber);

                grdDirectDelivery.SelectedIndex = -1;
                grdDirectDelivery.DataSource = stateList;
                grdDirectDelivery.DataBind();

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillGridContract() {

            const string METHOD_NAME = "FillGridContract";

            try {

                if (grdDirectDelivery.SelectedRow != null) {

                    int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                    int directDeliveryID = Convert.ToInt32(grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colDirectDeliveryID].Text);
                    int contractID = 0, contractNumber = 0;

                    List<ListDirectDeliveryContractItem> stateList = BeetDirectDelivery.DirectDeliveryContractGet(cropYear, directDeliveryID, contractID, contractNumber);

                    grdContract.SelectedIndex = -1;
                    grdContract.DataSource = stateList;
                    grdContract.DataBind();

                } else {

                    List<ListDirectDeliveryContractItem> stateList = new List<ListDirectDeliveryContractItem>();
                    stateList.Add(new ListDirectDeliveryContractItem(0, 0, "", 0, 0));

                    grdContract.SelectedIndex = -1;
                    grdContract.DataSource = stateList;
                    grdContract.DataBind();
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillStations() {

            const string METHOD_NAME = "FillStations";

            try {

                 //List<ListBeetFactoryIDItem> BeetFactoryIDGetList(int cropYear) 
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                List<ListBeetStationIDItem> stationList = BeetDataDomain.StationListGetAll(cropYear);

                ddlCriteriaContractStation.Items.Add(new ListItem("Any", "0"));
                ddlCriteriaDeliveryStation.Items.Add(new ListItem("Any", "0"));

                ddlEditContractStation.Items.Add(new ListItem(" ", "0"));
                ddlEditDeliveryStation.Items.Add(new ListItem(" ", "0"));

                // Fill various station controls with list of stations.
                foreach (ListBeetStationIDItem station in stationList) {

                    ddlCriteriaContractStation.Items.Add(new ListItem(station.StationNumberName, station.StationNumber));
                    ddlCriteriaDeliveryStation.Items.Add(new ListItem(station.StationNumberName, station.StationNumber));
                        
                    ddlEditContractStation.Items.Add(new ListItem(station.StationNumberName, station.StationNumber));
                    ddlEditDeliveryStation.Items.Add(new ListItem(station.StationNumberName, station.StationNumber));
                }

                ddlCriteriaContractStation.SelectedIndex = 0;
                ddlCriteriaDeliveryStation.SelectedIndex = 0;
                ddlEditContractStation.SelectedIndex = 0;
                ddlEditDeliveryStation.SelectedIndex = 0;

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillContractList() {

            const string METHOD_NAME = "FillContractList";

            try {

                // After slecting a direct delivery, get the selected contract station and fill 
                // list control with contracts related to the station.
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int contractStationNumber = 0;

                // do we have a specific grid row slected?
                if (grdDirectDelivery.SelectedRow != null) {
                    contractStationNumber = Convert.ToInt32(grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colContractStation].Text.Substring(0, 2));
                } else {
                    // Otherwise has the user selected contract station in the criteria.
                    if (ddlCriteriaContractStation.SelectedIndex > 0) {
                        contractStationNumber = Convert.ToInt32(ddlCriteriaContractStation.SelectedItem.Value);
                    }
                }

                if (contractStationNumber > 0) {
                    List<ListBeetContractIDItem> stateList = WSCReportsExec.ContractsByContractStationNo(cropYear, contractStationNumber.ToString());

                    ddlEditContractNumber.SelectedIndex = -1;
                    ddlEditContractNumber.DataSource = stateList;
                    ddlEditContractNumber.DataBind();
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void PopDirectDeliveryEdit() {

            const string METHOD_NAME = "PopDirectDeliveryEdit";

            try {

                Common.UILib.SelectDropDown(ddlEditDeliveryStation, grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colDeliveryStation].Text);
                Common.UILib.SelectDropDown(ddlEditContractStation, grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colContractStation].Text);
                txtEditRatePerTon.Text = grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colRatePerTon].Text;

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void PopContractEdit() {

            const string METHOD_NAME = "PopContractEdit";

            try {

                if (grdContract.SelectedRow != null) {
                    Common.UILib.SelectDropDown(ddlEditContractNumber, grdContract.SelectedRow.Cells[(int)GrdContractCols.colContractNumber].Text);
                    txtEditContractRatePerTon.Text = grdContract.SelectedRow.Cells[(int)GrdContractCols.colRatePerTon].Text;
                }

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdDirectDelivery);
            AddRowSelectToGridView(grdContract);
            base.Render(writer);
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                if ((gv.ID == grdDirectDelivery.ID && row.Cells[(int)GrdDirectDeliveryCols.colDeliveryStation].Text != "*") ||
                    (gv.ID == grdContract.ID && row.Cells[(int)GrdContractCols.colContractNumber].Text != "*")) {

                    row.Attributes["onmouseover"] = "HoverOn(this)";
                    row.Attributes["onmouseout"] = "HoverOff(this)";
                    //row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
                    row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
                }
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";
            try {

                ResetDirectDeliveryEdit();
                ResetContractEdit();
                FillGridDirectDelivery();
                FillContractList();
                FillGridContract();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlCriteriaContractStation_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCriteriaContractStation_SelectedIndexChanged";
            try {

                ResetDirectDeliveryEdit();
                ResetContractEdit();
                FillGridDirectDelivery();
                FillContractList();
                FillGridContract();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlCriteriaDeliveryStation_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCriteriaDeliveryStation_SelectedIndexChanged";
            try {

                ResetDirectDeliveryEdit();
                ResetContractEdit();
                FillGridDirectDelivery();
                FillContractList();
                FillGridContract();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdDirectDelivery_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdDirectDelivery_SelectedIndexChanged";
            try {

                ResetContractEdit();
                PopDirectDeliveryEdit();
                FillContractList();
                FillGridContract();
                PopContractEdit();

                //grdDirectDelivery.SelectedRow.Focus();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdDirectDelivery_RowCreated(object sender, GridViewRowEventArgs e) {
            //========================================================================
            // This is just too funny.  I call it either a BUG or a Design-MISTAKE!
            // In order to Hide a grid row that you want to hold data you must
            // turn visibility off here, after databinding has taken place.  It 
            // seems the control was not designed to understand this basic need.
            //========================================================================
            if (e.Row.RowType != DataControlRowType.EmptyDataRow) {

                e.Row.Cells[(int)GrdDirectDeliveryCols.colDirectDeliveryID].CssClass = "DisplayNone";
                e.Row.Cells[(int)GrdDirectDeliveryCols.colRowVersion].CssClass = "DisplayNone";
            }
        }

        protected void grdContract_RowCreated(object sender, GridViewRowEventArgs e) {
            //========================================================================
            // This is just too funny.  I call it either a BUG or a Design-MISTAKE!
            // In order to Hide a grid row that you want to hold data you must
            // turn visibility off here, after databinding has taken place.  It 
            // seems the control was not designed to understand this basic need.
            //========================================================================
            if (e.Row.RowType != DataControlRowType.EmptyDataRow) {

                e.Row.Cells[(int)GrdContractCols.colDirectDeliveryID].CssClass = "DisplayNone";
                e.Row.Cells[(int)GrdContractCols.colContractID].CssClass = "DisplayNone";
                e.Row.Cells[(int)GrdContractCols.colRowVersion].CssClass = "DisplayNone";
            }
        }

        protected void grdContract_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdContract_SelectedIndexChanged";
            try {

                ResetContractEdit();
                FillContractList();
                PopContractEdit();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDirectDeliveryAdd_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDirectDeliveryAdd_Click";
            try {

                string userName = Common.AppHelper.GetIdentityName();
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int directDeliveryID = 0;
                string rowVersion = "";

                // Get the delivery station
                if (ddlEditDeliveryStation.SelectedItem == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Delivery Station in the Direct Delivery Edit area.");
                    throw (warn);
                }
                int deliveryStationNumber = Convert.ToInt32(ddlEditDeliveryStation.SelectedItem.Value);
                
                // Get the cotnract station
                if (ddlEditContractStation.SelectedItem == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Contract Station in the Direct Delivery Edit area.");
                    throw (warn);
                }
                int contractStationNumber = Convert.ToInt32(ddlEditContractStation.SelectedItem.Value);

                // Get the rate per ton.  Rate is not required, but if given it must be a decimal
                string ratePerTonText = txtEditRatePerTon.Text;
                decimal dRatePerTon = 0;
                if (ratePerTonText.Length > 0) {
                    try {
                        dRatePerTon = Convert.ToDecimal(ratePerTonText);
                    }
                    catch {
                        Common.CWarning warn = new Common.CWarning("Please enter a valid dollar amount for the Rate Per Ton.");
                        throw (warn);
                    }
                }

                // Save it !
                BeetDirectDelivery.DirectDeliverySave(directDeliveryID, cropYear, contractStationNumber, deliveryStationNumber, dRatePerTon, rowVersion, userName);

                ResetDirectDeliveryEdit();
                ResetContractEdit();
                FillGridDirectDelivery();
                FillContractList();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDirectDeliveryUpdate_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDirectDeliveryUpdate_Click";
            try {

                // First walk thru all business rules.
                if (grdDirectDelivery.SelectedRow == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Direct Delivery to edit.");
                    throw (warn);
                }

                string userName = Common.AppHelper.GetIdentityName();
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                int directDeliveryID = Convert.ToInt32(grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colDirectDeliveryID].Text);
                string rowVersion = grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colRowVersion].Text;

                // Get the delivery station
                if (ddlEditDeliveryStation.SelectedItem == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Delivery Station in the Direct Delivery Edit area.");
                    throw (warn);
                }
                int deliveryStationNumber = Convert.ToInt32(ddlEditDeliveryStation.SelectedItem.Value);

                // Get the cotnract station
                if (ddlEditContractStation.SelectedItem == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Contract Station in the Direct Delivery Edit area.");
                    throw (warn);
                }
                int contractStationNumber = Convert.ToInt32(ddlEditContractStation.SelectedItem.Value);

                // Get the rate per ton.  Rate is not required, but if given it must be a decimal
                string ratePerTonText = txtEditRatePerTon.Text;
                decimal dRatePerTon = 0;
                if (ratePerTonText.Length > 0) {
                    try {
                        dRatePerTon = Convert.ToDecimal(ratePerTonText);
                    }
                    catch {
                        Common.CWarning warn = new Common.CWarning("Please enter a valid dollar amount for the Rate Per Ton.");
                        throw (warn);
                    }
                }

                // Save it !
                BeetDirectDelivery.DirectDeliverySave(directDeliveryID, cropYear, contractStationNumber, deliveryStationNumber, dRatePerTon, rowVersion, userName);

                ResetDirectDeliveryEdit();
                ResetContractEdit();
                FillGridDirectDelivery();
                FillContractList();

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDirectDeliveryDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDirectDeliveryDelete_Click";
            try {

                // First walk thru all business rules.
                if (grdDirectDelivery.SelectedRow == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Direct Delivery to Delete.");
                    throw (warn);
                }

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int directDeliveryID = Convert.ToInt32(grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colDirectDeliveryID].Text);

                // Delete it !
                BeetDirectDelivery.DirectDeliveryDelete(directDeliveryID, cropYear);

                ResetDirectDeliveryEdit();
                ResetContractEdit();
                FillGridDirectDelivery();
                FillContractList();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnOverrideAdd_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnOverrideAdd_Click";
            try {

                string userName = Common.AppHelper.GetIdentityName();
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));                
                string rowVersion = "";

                // First walk thru all business rules.
                if (grdDirectDelivery.SelectedRow == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Direct Delivery Scenario before adding a contract to a scenario.");
                    throw (warn);
                }

                // Get the contractID
                if (ddlEditContractNumber.SelectedItem == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Contract from the drop down list before adding a contract to the scenario.");
                    throw (warn);
                }
                int contractID = Convert.ToInt32(ddlEditContractNumber.SelectedItem.Value);

                // No duplicates allowed in Contract grid.
                string cntNo = ddlEditContractNumber.SelectedItem.Text;
                foreach (GridViewRow row in grdContract.Rows) {
                    if (row.Cells[(int)GrdContractCols.colContractNumber].Text == cntNo) {
                        Common.CWarning warn = new Common.CWarning("This contract is already a listed override, please select another contract to Add.");
                        throw (warn);
                    }
                }

                // Get the rate per ton.  Rate is not required, but if given it must be a decimal
                string ratePerTonText = txtEditContractRatePerTon.Text;
                decimal dRatePerTon = 0;
                if (ratePerTonText.Length > 0) {
                    try {
                        dRatePerTon = Convert.ToDecimal(ratePerTonText);
                    }
                    catch {
                        Common.CWarning warn = new Common.CWarning("Please enter a valid dollar amount for the Rate Per Ton.");
                        throw (warn);
                    }
                }

                int directDeliveryID = Convert.ToInt32(grdDirectDelivery.SelectedRow.Cells[(int)GrdDirectDeliveryCols.colDirectDeliveryID].Text);

                // Save it !
                BeetDirectDelivery.DirectDeliveryContractSave(directDeliveryID, cropYear, contractID, dRatePerTon, rowVersion, userName);

                ResetContractEdit();
                FillContractList();
                FillGridContract();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnOverrideUpdate_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnOverrideUpdate_Click";
            try {

                // First walk thru all business rules.
                if (grdContract.SelectedRow == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Contract to edit.");
                    throw (warn);
                }

                string userName = Common.AppHelper.GetIdentityName();
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int directDeliveryID = Convert.ToInt32(grdContract.SelectedRow.Cells[(int)GrdContractCols.colDirectDeliveryID].Text);
                int contractID = Convert.ToInt32(grdContract.SelectedRow.Cells[(int)GrdContractCols.colContractID].Text);
                string rowVersion = grdContract.SelectedRow.Cells[(int)GrdContractCols.colRowVersion].Text;

                // Get the rate per ton.  Rate is not required, but if given it must be a decimal
                string ratePerTonText = txtEditContractRatePerTon.Text;
                decimal dRatePerTon = 0;
                if (ratePerTonText.Length > 0) {
                    try {
                        dRatePerTon = Convert.ToDecimal(ratePerTonText);
                    }
                    catch {
                        Common.CWarning warn = new Common.CWarning("Please enter a valid dollar amount for the Contract Rate Per Ton.");
                        throw (warn);
                    }
                }

                // Save it !
                BeetDirectDelivery.DirectDeliveryContractSave(directDeliveryID, cropYear, contractID, dRatePerTon, rowVersion, userName);

                ResetContractEdit();
                FillContractList();
                FillGridContract();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnOverrideDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnOverrideDelete_Click";
            try {

                // First walk thru all business rules.
                if (grdContract.SelectedRow == null) {
                    Common.CWarning warn = new Common.CWarning("Please select a Contract to Delete from the grid.");
                    throw (warn);
                }

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int directDeliveryID = Convert.ToInt32(grdContract.SelectedRow.Cells[(int)GrdContractCols.colDirectDeliveryID].Text);
                int contractID = Convert.ToInt32(grdContract.SelectedRow.Cells[(int)GrdContractCols.colContractID].Text);
                string rowVersion = grdContract.SelectedRow.Cells[(int)GrdContractCols.colRowVersion].Text;

                // Delete it !
                BeetDirectDelivery.DirectDeliveryContractDelete(directDeliveryID, cropYear, contractID, rowVersion);

                ResetContractEdit();
                FillContractList();
                FillGridContract();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnContractSearch_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnContractSearch_Click";

            try {

                DoContractSearch();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void DoContractSearch() {

            const string METHOD_NAME = "DoContractSearch";

            try {

                //---------------------------------------------------------------------------------------------
                // Work from bottom up.  Arrange Contract override then arrange the direct delivery definition.
                //  *  Fill Contract Grid  *
                //---------------------------------------------------------------------------------------------
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                int directDeliveryID = 0, contractID = 0;
                int contractNumber = 0;

                try {
                    contractNumber = Convert.ToInt32(_contractSearch);
                }
                catch {
                    Common.CWarning wex = new Common.CWarning("Please enter a contract number.");
                    throw (wex);
                }                

                List<ListDirectDeliveryContractItem> cntList = BeetDirectDelivery.DirectDeliveryContractGet(cropYear, directDeliveryID, contractID, contractNumber);
                directDeliveryID = Convert.ToInt32(cntList[0].DirectDeliveryID);
                contractID = Convert.ToInt32(cntList[0].ContractID);

                List<ListDirectDeliveryItem> deliveryList = BeetDirectDelivery.DirectDeliveryGetByID(cropYear, directDeliveryID);
                string deliveryStation = deliveryList[0].DeliveryStation;
                string contractStation = deliveryList[0].ContractStation;
                if (deliveryStation.Length == 0 || deliveryStation == "*" || cntList.Count > 1) {
                    deliveryStation = "Any";
                }
                if (contractStation.Length == 0) {
                    contractStation = "Any";
                }

                Common.UILib.SelectDropDown(ddlCriteriaDeliveryStation, deliveryStation);
                Common.UILib.SelectDropDown(ddlCriteriaContractStation, contractStation);

                ResetDirectDeliveryEdit();
                ResetContractEdit();
                FillGridDirectDelivery();

                if (directDeliveryID > 0) {
                    // Find the matching row in the Direct Delivery grid.
                    foreach (GridViewRow row in grdDirectDelivery.Rows) {
                        if (row.Cells[(int)GrdDirectDeliveryCols.colDirectDeliveryID].Text == directDeliveryID.ToString()) {
                            grdDirectDelivery.SelectedIndex = row.DataItemIndex;
                        }
                    }
                }

                if (grdDirectDelivery.SelectedRow != null) {
                    PopDirectDeliveryEdit();
                }
                FillContractList();
                FillGridContract();

                if (contractID > 0) {
                    // Find the matching row in the Direct Delivery grid.
                    foreach (GridViewRow row in grdContract.Rows) {
                        if (row.Cells[(int)GrdContractCols.colContractID].Text == contractID.ToString()) {
                            grdContract.SelectedIndex = row.DataItemIndex;
                        }
                    }
                }

                if (grdContract.SelectedRow != null) {
                    PopContractEdit();
                }

            }
            catch (System.Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }
    }
}