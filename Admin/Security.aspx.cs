using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class Security : Common.BasePage {

        private const string MOD_NAME = "Admin.Security.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                if (!Page.IsPostBack) {

                    txtActiveTab.Text = "USER";
                    FillRoles(ddlUserSearchRole);
                    FillRoles(lstUserEditRole);
                    FillRegions(lstUserEditRegion);
                    FillRegions(ddlFactoryRegion);
                    FillFactory(lstFactory);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillFactory(ListBox lst) {

            const string METHOD_NAME = "FillFactory";
            try {

                lst.Items.Clear();                
                List<ListBeetFactoryNameItem> ftyList = BeetDataDomain.BeetFactoryNameGetList();

                lst.DataTextField = "FactoryLongName";
                lst.DataValueField = "FactoryNumber";
                lst.DataSource = ftyList;
                lst.DataBind();

                lst.SelectedIndex = -1;

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillRegions(ListBox lst) {

            try {

                lst.Items.Clear();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCSecurity.SecurityRegionGetAll(conn)) {

                        while (dr.Read()) {

                            int regionID = dr.GetInt32(dr.GetOrdinal("rgn_region_id"));
                            ListItem item = new ListItem(dr.GetString(dr.GetOrdinal("rgn_name")),
                                regionID.ToString());

                            lst.Items.Add(item);
                        }
                    }
                }
                lst.SelectedIndex = -1;

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("Security.FillRegions", ex);
                throw (wex);
            }
        }

        private void FillRegions(DropDownList ddl) {

            try {

                ddl.Items.Clear();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCSecurity.SecurityRegionGetAll(conn)) {

                        while (dr.Read()) {

                            int regionID = dr.GetInt32(dr.GetOrdinal("rgn_region_id"));
                            ListItem item = new ListItem(dr.GetString(dr.GetOrdinal("rgn_name")),
                                regionID.ToString());

                            ddl.Items.Add(item);
                        }
                    }
                }
                ddl.SelectedIndex = -1;

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("Security.FillRegions", ex);
                throw (wex);
            }
        }

        private void FillRoles(DropDownList ddl) {

            try {

                ddl.Items.Clear();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCSecurity.SecurityRoleGetAll(conn)) {

                        System.Web.UI.WebControls.ListItem item = new ListItem("All", "0");
                        ddl.Items.Add(item);

                        while (dr.Read()) {

                            int roleID = dr.GetInt16(dr.GetOrdinal("sro_role_id"));
                            item = new ListItem(dr.GetString(dr.GetOrdinal("sro_role_name")),
                                roleID.ToString());

                            ddl.Items.Add(item);
                        }
                    }
                }
                ddl.SelectedIndex = 0;

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("Security.FillRoles", ex);
                throw (wex);
            }
        }

        private void FillRoles(ListBox lst) {

            try {

                lst.Items.Clear();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCSecurity.SecurityRoleGetAll(conn)) {

                        while (dr.Read()) {

                            int roleID = dr.GetInt16(dr.GetOrdinal("sro_role_id"));
                            ListItem item = new ListItem(dr.GetString(dr.GetOrdinal("sro_role_name")),
                                roleID.ToString());

                            lst.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("Security.FillRoles", ex);
                throw (wex);
            }
        }

        private void FillUserSearchGrid() {

            try {

                grdUserResults.DataSource = null;
                grdUserResults.DataBind();

                string login = txtUserSearchLogin.Text;
                int roleID = 0;
                bool inActive = chkUserSearchShowInActive.Checked;

                if (ddlUserSearchRole.SelectedIndex > 0) {
                    roleID = Convert.ToInt32(ddlUserSearchRole.SelectedItem.Value);
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCSecurity.SecurityRoleGetUsers(conn, login, roleID, inActive)) {

                        grdUserResults.DataSource = dr;
                        grdUserResults.DataBind();
                    }
                }

                if (grdUserResults.Rows.Count == 1) {
                    grdUserResults.SelectedIndex = 0;
                    FillEditArea();
                } else {
                    if (grdUserResults.SelectedRow != null) {
                        FillEditArea();
                    }
                }

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("Security.FillUserSearchGrid", ex);
                throw (wex);
            }
        }

        private void ClearEditArea() {

            try {

                if (grdUserResults.Rows.Count > 0) {
                    grdUserResults.SelectedIndex = -1;
                }
                chkUserEditIsActive.Checked = false;
                lstUserEditRegion.SelectedIndex = -1;
                lstUserEditRole.SelectedIndex = -1;

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("Security.FillEditArea", ex);
                throw (wex);
            }
        }

        private void SelectFactoryByRegion() {

            try {

                lstFactory.SelectedIndex = -1;

                int regionID = 0;
                if (ddlFactoryRegion.SelectedIndex != -1) {
                    regionID = Convert.ToInt32(ddlFactoryRegion.SelectedItem.Value);
                }

                if (regionID != 0) {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                        using (SqlDataReader dr = WSCSecurity.RegionFactoryGetByRegion(conn, regionID)) {

                            while (dr.Read()) {

                                string factoryNo = dr.GetInt32(dr.GetOrdinal("fty_factory_no")).ToString();
                                foreach (ListItem li in lstFactory.Items) {
                                    if (li.Value == factoryNo) {
                                        li.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Common.CException wex = new Common.CException("Security.SelectFactoryByRegion", ex);
                throw (wex);
            }
        }

        private void FillEditArea() {

            try {

                int userID = Convert.ToInt32(grdUserResults.SelectedRow.Cells[0].Text);

                // Add User's Roles
                lstUserEditRole.SelectedIndex = -1;
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
				
                    using (SqlDataReader dr = WSCSecurity.SecurityGetUserRoles(conn, userID)) {

                        while (dr.Read()) {

                            string roleID = dr.GetInt32(dr.GetOrdinal("sro_role_id")).ToString();
                            foreach (ListItem li in lstUserEditRole.Items) {
                                if (li.Value == roleID) {
                                    li.Selected = true;
                                    break;
                                }
                            }
                        }
                    }

                    // Add User's Regions
                    lstUserEditRegion.SelectedIndex = -1;
                    using (SqlDataReader dr = WSCSecurity.SecurityGetUserRegions(conn, userID)) {

                        while (dr.Read()) {

                            string regionID = dr.GetInt32(dr.GetOrdinal("rgn_region_id")).ToString();
                            foreach (ListItem li in lstUserEditRegion.Items) {
                                if (li.Value == regionID) {
                                    li.Selected = true;
                                    break;
                                }
                            }
                        }
                    }

                }

                // Check whether user is active
                string isActive = grdUserResults.SelectedRow.Cells[3].Text;
                chkUserEditIsActive.Checked = (isActive == "Y");
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("Security.FillEditArea", ex);
                throw (wex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdUserResults);
            base.Render(writer);
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                row.Attributes["onmouseover"] = "HoverOn(this)";
                row.Attributes["onmouseout"] = "HoverOff(this)";
                //row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
                row.Attributes.Add("onclick", "SelectRow(this); " + Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
            }
        }

        protected void btnUserSearch_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnUserSearch_Click";

            try {
                ClearEditArea();
                FillUserSearchGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnUserEditSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnUserEditSave_Click";

            try {

                int userID = 0;
                if (grdUserResults.SelectedRow == null) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please select a user from the User Search Results");
                    throw (warn);
                } else {
                    userID = Convert.ToInt32(grdUserResults.SelectedRow.Cells[0].Text);
                }

                // Save User's Region assignments and Role assignments
                bool isActive = chkUserEditIsActive.Checked;
                string roles = "";

                foreach (ListItem li in lstUserEditRole.Items) {
                    if (li.Selected) {
                        if (roles.Length == 0) {
                            roles = li.Value;
                        } else {
                            roles += "," + li.Value;
                        }
                    }
                }

                string regions = "";
                foreach (ListItem li in lstUserEditRegion.Items) {
                    if (li.Selected) {
                        if (regions.Length == 0) {
                            regions = li.Value;
                        } else {
                            regions += "," + li.Value;
                        }
                    }
                }

                WSCSecurity.UserSecuritySave(userID, isActive, roles, regions);
                FillUserSearchGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlFactoryRegion_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlFactoryRegion_SelectedIndexChanged";

            try {
                ClearEditArea();
                FillUserSearchGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnFactorySave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnFactorySave_Click";

            try {

                int regionID = 0;
                if (ddlFactoryRegion.SelectedItem == null) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please select Region from the list.");
                    throw (warn);
                } else {

                    if (ddlFactoryRegion.SelectedItem.Text.ToUpper() == "ALL COOP") {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please do not associate factories to the region All Coop.");
                        throw (warn);
                    }
                    regionID = Convert.ToInt32(ddlFactoryRegion.SelectedItem.Value);
                }

                string factories = "";
                foreach (ListItem li in lstFactory.Items) {
                    if (li.Selected) {
                        if (factories.Length == 0) {
                            factories = li.Value;
                        } else {
                            factories += "," + li.Value;
                        }
                    }
                }

                WSCSecurity.UserSecurityRegionFactorySave(regionID, factories);
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void grdUserResults_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdUserResults_SelectedIndexChanged";

            try {
                
                FillEditArea();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
