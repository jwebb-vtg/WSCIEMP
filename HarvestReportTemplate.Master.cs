using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WSCData;

namespace WSCIEMP {

    public partial class HarvestReportTemplate : System.Web.UI.MasterPage {

        //=============================================================================
        // Content Pages must sink this event to respond to a Print button click.
        //=============================================================================
        public event CommandEventHandler PrintReady;
        public event CommandEventHandler CropYearChange;
        public event CommandEventHandler ShidChange;

        private const string MOD_NAME = "HarvestReportTemplate.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                //_shs = Globals.ShsData;

                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes.Add("type", "text/javascript");
                script.Attributes.Add("src", Page.ResolveUrl("~/Script/Common.js"));
                Page.Header.Controls.Add(script);

                // ** ** **  Add JQuery.js to page Head tag  ** ** ** 
				//script = new HtmlGenericControl("script");
				//script.Attributes.Add("type", "text/javascript");
				//script.Attributes.Add("src", Page.ResolveUrl("~/Script/JQuery.js"));
				//Page.Header.Controls.Add(script);

                // ** ** **  Add MyJQuery.js extensions to page Head tag  ** ** ** 
                script = new HtmlGenericControl("script");
                script.Attributes.Add("type", "text/javascript");
                script.Attributes.Add("src", Page.ResolveUrl("~/Script/MyJQuery.js"));
                Page.Header.Controls.Add(script);

                // ** ** **  Add JQueryUI.css (custom version) to page Head tag  ** ** ** 
				//HtmlLink linkBlock = new HtmlLink();
				//linkBlock.Href = Page.ResolveUrl("~/Script/JQueryUI/css/blitzer/jquery-ui.css");
				//linkBlock.Attributes.Add("rel", "stylesheet");
				//linkBlock.Attributes.Add("type", "text/css");
				//Page.Header.Controls.Add(linkBlock);

                // ** ** **  Add JQueryUI.js to page Head tag  ** ** ** 
				//script = new HtmlGenericControl("script");
				//script.Attributes.Add("type", "text/javascript");
				//script.Attributes.Add("src", Page.ResolveUrl("~/Script/JQueryUI/js/jquery-ui.js"));
				//Page.Header.Controls.Add(script);

                lblDateStamp.Text = DateTime.Now.ToString("MMMM dd, yyyy");
                litYear.Text = DateTime.Now.Year.ToString();

                ddlReports.Attributes.Add("onchange", "PopReport(this)");

                if (!Page.IsPostBack) {

                    if (ddlCropYear.Items.Count == 0) {

                        string cy = WSCField.GetCropYears()[0].ToString();
                        WSCField.FillCropYear(ddlCropYear, cy);
                        CropYear = cy;
                    }                    

                    ResetMemberInfo(SHID.ToString());
                    ShowMemberInfo();

                } else {

                    string newShid = txtSHID.Text;
                    if (!Common.CodeLib.IsValidSHID(newShid)) {
                        newShid = "";
                    }

                    string newCropYear = Common.UILib.GetDropDownText(ddlCropYear);

                    if (newShid != SHID.ToString() || newCropYear != CropYear.ToString()) {
                        CropYear = newCropYear;
                        FindSHID(newShid);
                    }
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				ShowWarning(ex);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnFind_Click";

            try {

                FindSHID(txtSHID.Text);

                if (ShidChange != null) {
                    ShidChange(this, new CommandEventArgs(txtSHID.Text, MemberID.ToString()));
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				ShowWarning(ex);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e) {

            // Raise the event for the content page.
            if (PrintReady != null) {
                PrintReady(this, new CommandEventArgs(btnPrint.Text, ""));
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            CropYear = Common.UILib.GetDropDownText(ddlCropYear);

            // Raise the event for the content page.
            if (CropYearChange != null) {
                CropYearChange(this, new CommandEventArgs(ddlCropYear.SelectedItem.Text, ddlCropYear.SelectedItem.Value));
            }
        }

        private void FindSHID(string newShid) {

            const string METHOD_NAME = "FindSHID";
            try {

                ResetShareholder();

                int shid = 0;
                if (Common.CodeLib.IsValidSHID(newShid)) {
                    shid = Convert.ToInt32(newShid);
                } else {
                    Common.CWarning warn = new Common.CWarning("Please enter a valid SHID.");
                    throw (warn);
                }
                SHID = shid;

                if (SHID == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter a SHID.");
                    throw (warn);
                }

                ResetMemberInfo(SHID.ToString());
                ShowMemberInfo();
            }
            catch (Exception ex) {
                Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ShowMemberInfo() {

            txtSHID.Text = SHID.ToString();
            lblBusName.Text = BusName;

            if (MemberID <= 0) {
                txtSHID.Text = "";
                Common.AppHelper.ShowWarning(divWarning, "Please enter a valid SHID and press the Find button.");
            }
        }

        private void ResetMemberInfo(string shid) {

            const string METHOD_NAME = "ResetMemberInfo";

            int memberID = 0, addressID = 0;
            string busName = "", phone = "", email = "", fax = "";

            try {

                WSCMember.GetInfo(shid, ref memberID, ref addressID, ref busName, ref phone, ref email, ref fax);
                
                ResetShareholder();
                if (memberID > 0) {

                    MemberID = memberID;
                    AddressID = addressID;
                    SHID = Convert.ToInt32(shid);
                    BusName = busName;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void ResetShareholder() {

            //_shs.ResetShareholder();
            SHID = 0;
            MemberID = 0;
            AddressID = 0;
            txtSHID.Text = "0";
            lblBusName.Text = "";
            BusName = "";
        }

        private void FillReports(MenuItem mnuSelectedRpt) {

            if (ddlReports.Items.Count == 0) {

                MenuItem mnuMaster = mnuSelectedRpt.Parent;
                ddlReports.Items.Clear();

                // iterate thru entire menu system
                foreach (MenuItem item in mnuMaster.ChildItems) {

                    string mnuName = item.Text;
                    string mnuUrl = item.NavigateUrl;

                    ddlReports.Items.Add(new ListItem(mnuName, mnuUrl));

                    if (mnuSelectedRpt.NavigateUrl == mnuUrl) {
                        ddlReports.Items[ddlReports.Items.Count - 1].Selected = true;
                    }
                }
            }
        }

        //==========================================================================================
        // This method is wired to the OnPreRender setting of the menu control on this page.
        //==========================================================================================
        protected void CheckSelectedNode(object sender, EventArgs e) {

            System.Web.UI.WebControls.Menu mnu = (System.Web.UI.WebControls.Menu)sender;
            MenuItem ancestor = mnu.SelectedItem;   // on initial assignment "ancestor" name makes no sense.

            // ----------------------------------------------------
            // First item of business is fill the report drop down
            //-----------------------------------------------------
            FillReports(ancestor);

            //=====================================================
            // Algorithm depends on the Root not having a title.
            // This shows up using the Text property.
            //=====================================================
            while (ancestor != null) {

                ancestor = ancestor.Parent as MenuItem;
                if (ancestor != null && ancestor.Text.Length > 0) {
                    if (ancestor.Selectable) {
                        ancestor.Selected = true;
                    }
                } else {
                    ancestor = null;
                }
            }

            foreach (MenuItem itm in mnu.Items) {
                HideToolTip(itm);
            }
        }

        private void HideToolTip(MenuItem item) {

            if (item.ChildItems.Count > 0) {
                foreach (MenuItem itm in item.ChildItems) {
                    HideToolTip(itm);
                }
            }

            item.ToolTip = "";
        }

        private string _cropYear = null;
        public string CropYear {
            set {
                ViewState["HRT_CropYear"] = value;
                _cropYear = value;
                //_shs.CropYear = Convert.ToInt32(_cropYear);
            }
            get {

                if (ddlCropYear.SelectedItem == null) {
                    if (ddlCropYear.Items.Count == 0) {
                        string _cropYear = WSCField.GetCropYears()[0].ToString();
                        WSCField.FillCropYear(ddlCropYear, _cropYear);
                        ViewState["HRT_CropYear"] = _cropYear;
                    }
                }

                return ddlCropYear.SelectedItem.Text;
            }
        }

        public string ReportName {
            get { return Common.UILib.GetDropDownText(ddlReports); }
        }

        public string LocPDF {
            get { return txtPDF.Text; }
            set { txtPDF.Text = value; }
        }

        private string _shid = null;
        public int SHID {

            set {
                ViewState["HRT_SHID"] = value;
                _shid = value.ToString();
                //_shs.SHID = value;
            }
            get {
                if (_shid == null) {
                    _shid = (ViewState["HRT_SHID"] != null ? Convert.ToInt32(ViewState["HRT_SHID"]).ToString() : "0");
                    //if (_shid == "0") {
                    //    _shid = _shs.SHID.ToString();
                    //}
                }
                return Convert.ToInt32(_shid);
            }
        }
        private string _memberID = null;
        public int MemberID {

            set {
                ViewState["HRT_MemberID"] = value;
                _memberID = value.ToString();                
            }
            get {
                if (_memberID == null) {
                    _memberID = (ViewState["HRT_MemberID"] != null ? Convert.ToInt32(ViewState["HRT_MemberID"]).ToString() : "0");
                }
                return Convert.ToInt32(_memberID);
            }
        }
        private string _addressID = null;
        public int AddressID {

            set {
                ViewState["HRT_AddressID"] = value;
                _addressID = value.ToString();
            }
            get {
                if (_addressID == null) {
                    _addressID = (ViewState["HRT_AddressID"] != null ? Convert.ToInt32(ViewState["HRT_AddressID"]).ToString() : "0");
                }
                return Convert.ToInt32(_addressID);
            }
        }

		public string TextShid {
			get {
				return txtSHID.Text;
			}
		}

        private string _busName = null;
        public string BusName {

            set {
                ViewState["HRT_BusName"] = value;
                _busName = value;
            }
            get {
                if (_busName == null) {
                    _busName = (ViewState["HRT_BusName"] != null ? ViewState["HRT_BusName"].ToString() : "");
                }
                return _busName;
            }
        }

		//===========================================================
		// Show/Log Warning and Errors
		//===========================================================
		internal void ShowWarning(string msg) {
			ShowWarning(msg, divWarning);
		}

		internal void ShowWarning(string msg, HtmlGenericControl div) {
			div.Attributes.Add("class", "WarningOn");
			div.InnerHtml = msg;
		}

		internal void ShowWarning(Exception ex) {

			if (WSCIEMP.Common.CException.IsWarning(ex)) {
				ShowWarning(WSCIEMP.Common.CException.GetWarningMessage(ex));
			} else {
				ShowWarning(WSCIEMP.Common.CException.GetErrorMessages(ex));
			}
		}

		internal void ShowWarning(Exception ex, string nonWarningMessage) {

			if (WSCIEMP.Common.CException.IsWarning(ex)) {
				ShowWarning(WSCIEMP.Common.CException.GetWarningMessage(ex));
			} else {
				if (!String.IsNullOrEmpty(nonWarningMessage)) {
					ShowWarning(nonWarningMessage);
					Common.AppHelper.LogException(ex, HttpContext.Current);
				}
			}
		}

		internal void ShowWarning(Exception ex, HtmlGenericControl div) {

			if (WSCIEMP.Common.CException.IsWarning(ex)) {
				ShowWarning(WSCIEMP.Common.CException.GetWarningMessage(ex), div);
			} else {
				ShowWarning(WSCIEMP.Common.CException.GetErrorMessages(ex), div);
			}
		}

		internal void ShowConfirmation(string msg) {
			divWarning.Attributes.Add("class", "ConfirmationOn");
			divWarning.InnerHtml = msg;
		}

		internal void HideWarning(HtmlGenericControl div) {
			div.Attributes.Add("class", "WarningOff");
			div.InnerText = "";
		}

		internal void HideWarning() {
			HideWarning(divWarning);
		}
    }
}
