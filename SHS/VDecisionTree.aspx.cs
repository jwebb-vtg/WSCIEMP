using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.SHS {

    public partial class VDecisionTree : Common.BasePage {

        const string MOD_NAME = "SHS.VDecisionTree.";
        private WSCShsData _shs = null;
        private string _busName = "";

        // ***********************************************************************************
        // Note to publish new html Nurser data into ZHost\SeedDecision tree use the 
        // utility Nurseries_Publisher.xls.
        // To publish variety data to the database, use the utility Variety_Publisher.xls
        // Both are found in a Western\Utility\WSCIEMP folder.
        // ***********************************************************************************
        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                _shs = Globals.ShsData;

                if (!Page.IsPostBack) {

                    FillCropYear();

                    if (Common.CodeLib.IsValidSHID(_shs.SHID.ToString())) {
                        
                        FindAddress(_shs.SHID.ToString());
                        txtSHID.Text = SHID.ToString();
                        lblBusName.Text = _busName;

                        SetupNorthSouth();

                    } else {

                        divNurseryNorth.Attributes.Add("class", "DisplayOff");
                        divNurserySouth.Attributes.Add("class", "DisplayOff");

                        Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                        throw (warn);
                    }                
                }

            }
            catch (System.Exception ex) {

                ResetShareholder();
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillCropYear() {

            System.Collections.ArrayList cropYears = WSCField.GetCropYears();
            CropYear = Convert.ToInt32(cropYears[0].ToString());
        }

        protected void btnFind_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnFind_Click";
            string shid = txtSHID.Text;

            try {


                if (!Common.CodeLib.IsValidSHID(shid)) {

                    divNurseryNorth.Attributes.Add("class", "DisplayOff");
                    divNurserySouth.Attributes.Add("class", "DisplayOff");

                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                    throw (warn);
                }
                ClearResults();
                FindAddress(shid);
                SetupNorthSouth();

            }
            catch (Exception ex) {

                divNurseryNorth.Attributes.Add("class", "DisplayOff");
                divNurserySouth.Attributes.Add("class", "DisplayOff");
                ResetShareholder();
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FindAddress(string shid) {

            const string METHOD_NAME = "FindAddress";

            try {

                ResetMemberInfo(shid, CropYear);
                lblBusName.Text = _busName;

                if (MemberID == 0) {
                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                    throw (warn);
                }
            }
            catch (System.Exception ex) {

                Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (ex);
            }
        }

        public void ResetMemberInfo(string shid, int cropYear) {

            int memberID = 0, addressID = 0;
            string phone = "", email = "", fax = "";

            try {
                WSCMember.GetInfo(shid, ref memberID,
                    ref addressID, ref _busName, ref phone, ref email, ref fax);
            }
            catch (System.Exception ex) {
                throw (ex);
            }

            if (memberID > 0) {

                MemberID = memberID;
                AddressID = addressID;
                SHID = Convert.ToInt32(shid);

            } else {
                ResetShareholder();
            }
        }

        private void SetupNorthSouth() {

            try {

                if (MemberID > 0) {

                    string northSouth = WSCMember.GetFactoryNorthSouth(AddressID);

                    FillDecisionQueries(ddlQuery1, northSouth);
                    FillDecisionQueries(ddlQuery2, northSouth);
                    FillDecisionQueries(ddlQuery3, northSouth);
                    FillDecisionQueries(ddlQuery4, northSouth);
                    FillDecisionQueries(ddlQuery5, northSouth);
                    FillDecisionQueries(ddlQuery6, northSouth);

                    FillResList(ddlRes1);
                    FillResList(ddlRes2);
                    FillResList(ddlRes3);
                    FillResList(ddlRes4);
                    FillResList(ddlRes5);
                    FillResList(ddlRes6);

                    btnSearch.Enabled = true;

                    if (northSouth == "NORTH") {
                        divNurseryNorth.Attributes.Add("class", "BlockNav");
                        divNurserySouth.Attributes.Add("class", "DisplayOff");
                    } else {
                        divNurseryNorth.Attributes.Add("class", "DisplayOff");
                        divNurserySouth.Attributes.Add("class", "BlockNav");
                    }
                    txtNorthSouth.Text = northSouth;

                } else {

                    // disable all north/south controls.
                    ddlQuery1.Enabled = false;
                    ddlQuery2.Enabled = false;
                    ddlQuery3.Enabled = false;
                    ddlQuery4.Enabled = false;
                    ddlQuery5.Enabled = false;
                    ddlQuery6.Enabled = false;

                    ddlRes1.Enabled = false;
                    ddlRes2.Enabled = false;
                    ddlRes3.Enabled = false;
                    ddlRes4.Enabled = false;
                    ddlRes5.Enabled = false;
                    ddlRes6.Enabled = false;
                    btnSearch.Enabled = false;

                    txtNorthSouth.Text = "";
                }
            }
            catch (System.Exception ex) {
                throw (ex);
            }
        }

        private void ResetShareholder() {

            _shs.ResetShareholder();
            SHID = 0;
            MemberID = 0;
            AddressID = 0;
            txtSHID.Text = "0";
            lblBusName.Text = "";
            _busName = "";

            ddlQuery1.SelectedIndex = 0;
            ddlQuery2.SelectedIndex = 0;
            ddlQuery3.SelectedIndex = 0;
            ddlQuery4.SelectedIndex = 0;
            ddlQuery5.SelectedIndex = 0;
            ddlQuery6.SelectedIndex = 0;

            ddlRes1.SelectedIndex = 0;
            ddlRes2.SelectedIndex = 0;
            ddlRes3.SelectedIndex = 0;
            ddlRes4.SelectedIndex = 0;
            ddlRes5.SelectedIndex = 0;
            ddlRes6.SelectedIndex = 0;

            ClearResults();

        }

        private void FillResList(System.Web.UI.WebControls.DropDownList ddl) {

            ddl.Enabled = true;
            ddl.Items.Clear();

            System.Web.UI.WebControls.ListItem li = new ListItem("Select Resistance", "0");
            ddl.Items.Add(li);

            li = new ListItem("Very Resistant", "1");
            ddl.Items.Add(li);

            li = new ListItem("Resistant", "2");
            ddl.Items.Add(li);

            li = new ListItem("Moderately Resistant", "3");
            ddl.Items.Add(li);

            li = new ListItem("Moderately Susceptible", "4");
            ddl.Items.Add(li);

            li = new ListItem("Susceptible", "5");
            ddl.Items.Add(li);

            li = new ListItem("Very Susceptible", "6");
            ddl.Items.Add(li);

        }

        private void FillDecisionQueries(System.Web.UI.WebControls.DropDownList ddl, string northSouth) {

            ddl.Enabled = true;
            ddl.Items.Clear();

            System.Web.UI.WebControls.ListItem li = new ListItem("Select a question", "any");
            ddl.Items.Add(li);

            li = new ListItem("Aphanomyces resistance of at least:", "aphanomyces");
            ddl.Items.Add(li);

			li = new ListItem("Nematode resistance of at least:", "nematode");
			ddl.Items.Add(li);

            li = new ListItem("Curly Top resistance of at least:", "curlytop");
            ddl.Items.Add(li);

            li = new ListItem("Fusarium resistance of at least:", "fusarium");
            ddl.Items.Add(li);

            li = new ListItem("Root Aphid resistance of at least:", "rootaphid");
            ddl.Items.Add(li);

            li = new ListItem("Cercospora resistance of at least:", "cercospora");
            ddl.Items.Add(li);

            li = new ListItem("Rhizoctonia resistance of at least:", "rhizoctonia");
            ddl.Items.Add(li);

        }

        private void ClearResults() {
			HideResults();
            grdResults.DataSource = null;
            grdResults.DataBind();
        }

		private void BindGrid() {

			const string METHOD_NAME = "BindGrid";

			try {
				BindGrid("0");
			}
			catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				throw (wex);
			}
		}

		private void BindGrid(string sortColumn) {

            try {

                ClearResults();

                string shid = txtSHID.Text;
                if (!Common.CodeLib.IsValidSHID(shid)) {

                    divNurseryNorth.Attributes.Add("class", "DisplayOff");
                    divNurserySouth.Attributes.Add("class", "DisplayOff");

                    Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid SHID.");
                    throw (warn);

                }

                // Collect the current search criteria
                string disease1 = Common.UILib.GetDropDownValue(ddlQuery1);
                if (disease1 == "any") disease1 = "";
                string disease2 = Common.UILib.GetDropDownValue(ddlQuery2);
                if (disease2 == "any") disease2 = "";
                string disease3 = Common.UILib.GetDropDownValue(ddlQuery3);
                if (disease3 == "any") disease3 = "";
                string disease4 = Common.UILib.GetDropDownValue(ddlQuery4);
                if (disease4 == "any") disease4 = "";
                string disease5 = Common.UILib.GetDropDownValue(ddlQuery5);
                if (disease5 == "any") disease5 = "";
                string disease6 = Common.UILib.GetDropDownValue(ddlQuery6);
                if (disease5 == "any") disease6 = "";

                string wantResis1 = Common.UILib.GetDropDownValue(ddlRes1);
                if (wantResis1 == "0") wantResis1 = "";
                string wantResis2 = Common.UILib.GetDropDownValue(ddlRes2);
                if (wantResis2 == "0") wantResis2 = "";
                string wantResis3 = Common.UILib.GetDropDownValue(ddlRes3);
                if (wantResis3 == "0") wantResis3 = "";
                string wantResis4 = Common.UILib.GetDropDownValue(ddlRes4);
                if (wantResis4 == "0") wantResis4 = "";
                string wantResis5 = Common.UILib.GetDropDownValue(ddlRes5);
                if (wantResis5 == "0") wantResis5 = "";
                string wantResis6 = Common.UILib.GetDropDownValue(ddlRes6);
                if (wantResis6 == "0") wantResis6 = "";

                string prodArea = txtNorthSouth.Text;

                if ((disease1 + disease2 + disease3 +
                    disease4 + disease5 + disease6).Length == 0 ||
                    (wantResis1 + wantResis2 + wantResis3 +
                    wantResis4 + wantResis5 + wantResis6).Length == 0) {

                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please select one or more diseases and select its resistance level.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    using (SqlDataReader dr = WSCField.SeedVarietyDetailGetByResistance(conn,
                              disease1, wantResis1,
                              disease2, wantResis2,
                              disease3, wantResis3,
                              disease4, wantResis4,
                              disease5, wantResis5,
                              disease6, wantResis6,
							  prodArea, sortColumn)) {

                        grdResults.DataSource = dr;
                        grdResults.DataBind();

						if (grdResults.Rows.Count == 0) {
							HideResults();
						} else {
							ShowResults();
						}
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("VDecisionTree.BindGrid", ex);
                throw (wex);
            }
        }

		private void HideResults() {
			divResult.Attributes.Add("class", "DisplayOff");
		}

		private void ShowResults() {
			divResult.Attributes.Add("class", "DisplayOn");
		}

        protected void btnSearch_Click(object sender, EventArgs e) {

			const string METHOD_NAME = "btnSearch_Click";

            try {

                BindGrid();
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

		protected void grdResults_RowDataBound(object sender, GridViewRowEventArgs e) {

			//e.Item.EnableViewState = false;
			//string dataGridID = ((DataGrid)sender).ID + "$"; // suffix a $ to datagrid ID

			if (e.Row.RowType == DataControlRowType.DataRow) {
				// Add blue/green background color on N Roundup-Ready
				//if (e.Row.Cells[9].Text == "N") {
				//    e.Row.Style.Add("background-color", "#99FFCC");
				//}

				// Add yellow background color on selected rows
				string b = e.Row.Cells[9].Text;
				if (b.Length > 0 && Convert.ToBoolean(b) == true) {
					e.Row.Style.Add("background-color", "#FFFF33");
				}
			}
		}

		protected void grdResults_RowCreated(object sender, GridViewRowEventArgs e) {

			//========================================================================
			// This is just too funny.  I call it either a BUG or a Design-MISTAKE!
			// In order to Hide a grid row that you want to hold data you must
			// turn visibility off here, after databinding has taken place.  It 
			// seems the control was not designed to understand this basic need.
			//========================================================================
			if (e.Row.RowType != DataControlRowType.EmptyDataRow && e.Row.RowType != DataControlRowType.Pager) {
				e.Row.Cells[9].CssClass = "DisplayNone";
			}
		}

		protected void grdResults_Sorting(object sender, GridViewSortEventArgs e) {			
			BindGrid(e.SortExpression);
		}

        private string _shid = null;
        private int SHID {

            set {
                ViewState["SHID"] = value;
                _shid = value.ToString();
                _shs.SHID = value;
            }
            get {
                if (_shid == null) {
                    _shid = (ViewState["SHID"] != null ? Convert.ToInt32(ViewState["SHID"]).ToString() : "0");
                }
                return Convert.ToInt32(_shid);
            }
        }
        private string _memberID = null;
        private int MemberID {

            set {
                ViewState["MemberID"] = value;
                _memberID = value.ToString();
            }
            get {
                if (_memberID == null) {
                    _memberID = (ViewState["MemberID"] != null ? Convert.ToInt32(ViewState["MemberID"]).ToString() : "0");
                }
                return Convert.ToInt32(_memberID);
            }
        }
        private string _addressID = null;
        private int AddressID {

            set {
                ViewState["AddressID"] = value;
                _addressID = value.ToString();
            }
            get {
                if (_addressID == null) {
                    _addressID = (ViewState["AddressID"] != null ? Convert.ToInt32(ViewState["AddressID"]).ToString() : "0");
                }
                return Convert.ToInt32(_addressID);
            }
        }
        private string _cropYear = null;
        private int CropYear {

            set {
                ViewState["CropYear"] = value;
                _shs.CropYear = value;
                _cropYear = value.ToString();
            }
            get {
                if (_cropYear == null) {
                    if (ViewState["CropYear"] != null) {
                        _cropYear = Convert.ToInt32(ViewState["CropYear"]).ToString();
                    } else {
                        _cropYear = _shs.CropYear.ToString();
                        ViewState["CropYear"] = _cropYear;
                    }
                }
                return Convert.ToInt32(_cropYear);
            }
        }
    }
}
