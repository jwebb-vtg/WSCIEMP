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

    public partial class MasterReportTemplate : System.Web.UI.MasterPage {

        //=============================================================================
        // Content Pages must sink this event to respond to a Print button click.
        //=============================================================================
        public event CommandEventHandler PrintReady;
        public event CommandEventHandler CropYearChange;

        protected void Page_Load(object sender, EventArgs e) {

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

            if (ddlCropYear.Items.Count == 0) {
                string cy = WSCField.GetCropYears()[0].ToString();
                WSCField.FillCropYear(ddlCropYear, cy);
            }
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

        public string CropYear {
            get {
                
                if (ddlCropYear.SelectedItem == null) {
                    if (ddlCropYear.Items.Count == 0) {
                        string cy = WSCField.GetCropYears()[0].ToString();
                        WSCField.FillCropYear(ddlCropYear, cy);
                    }
                }

                return ddlCropYear.SelectedItem.Text; 
            }
        }

        public string CropYearLabel {
            get { return lblCropYearTop.Text; }
            set { lblCropYearTop.Text = value; }
        }

        public string ReportName {
            get { return Common.UILib.GetDropDownText(ddlReports); }
        }

        public string LocPDF {
            get { return txtPDF.Text;}
            set {txtPDF.Text = value;}
        }

        public string LocLastPDF {
            get { return txtLastPDF.Text; }
            set { txtLastPDF.Text = value; }
        }

        protected void btnPrint_Click(object sender, EventArgs e) {

            // Raise the event for the content page.
            if (PrintReady != null) {
                PrintReady(this, new CommandEventArgs(btnPrint.Text, ""));
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            // Raise the event for the content page.
            if (CropYearChange != null) {
                CropYearChange(this, new CommandEventArgs(ddlCropYear.SelectedItem.Text, ddlCropYear.SelectedItem.Value));
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

		internal void ShowWarning(Exception ex, string nonWarningMessage, HtmlGenericControl div) {

			if (WSCIEMP.Common.CException.IsWarning(ex)) {
				ShowWarning(WSCIEMP.Common.CException.GetWarningMessage(ex), div);
			} else {
				if (!String.IsNullOrEmpty(nonWarningMessage)) {
					ShowWarning(nonWarningMessage, div);
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
