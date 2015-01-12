using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class AppErrors : Common.BasePage {

        private const string MOD_NAME = "WSCIEMP.Admin.AppErrors.";

        private enum ErrorResultColumn {
            errColHiddenSelect = 0,
            errColAppName,
            errColSeverity,
            errColDate,
            errColStatus,
            errColAction,
            errColServer,
            errColClient,
            errColUser,
            errColCode,
            errColPath
        }

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                //-------------------------------------------------------------
                // Ensure we ALWAYS havce valid from/to dates.
                //-------------------------------------------------------------
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                DateTime dtFromDate;
                DateTime dtToDate;
                DateTime tmpDate;

                if (toDate.Length == 0 || !Common.CodeLib.IsDate(toDate)) {
                    toDate = DateTime.Now.ToShortDateString();
                }
                if (fromDate.Length == 0 || !Common.CodeLib.IsDate(fromDate)) {
                    fromDate = DateTime.Now.AddMonths(-1).ToShortDateString();
                }

                dtFromDate = Convert.ToDateTime(fromDate);
                dtToDate = Convert.ToDateTime(toDate);

                if (dtFromDate > dtToDate) {
                    tmpDate = dtFromDate;
                    dtFromDate = dtToDate;
                    dtToDate = tmpDate;
                }

                txtToDate.Text = dtToDate.ToString("MM/dd/yyyy");
                txtFromDate.Text = dtFromDate.ToString("MM/dd/yyyy");

                if (!Page.IsPostBack) {

                    btnDeleteErrorFile.Attributes.Add("onclick", "return confirm('Are you sure you want to Delete this file?');");
                    txtEditBreakText.Text = "_ERROR_BREAK_";

                    FillStatus();
                    FillApplication();
                    FillEditSeverity();
                    FillResultsGrid();

                }
                Page.Header.DataBind();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

		protected override void Render(System.Web.UI.HtmlTextWriter writer) {

			// Add click event to GridView do not do this in _RowCreated or _RowDataBound
			AddRowSelectToGridView(grdResult);
			base.Render(writer);
		}

		private void AddRowSelectToGridView(GridView gv) {

			foreach (GridViewRow row in gv.Rows) {

				row.Attributes["onmouseover"] = "HoverOn(this)";
				row.Attributes["onmouseout"] = "HoverOff(this)";
				//row.Attributes.Add("onclick", "SelectRow(this); SelectContract(" + row.Cells[0].Text + ", '" + row.Cells[5].Text + "');");
				row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
			}
		}

        private void FillStatus() {

            const string METHOD_NAME = "FillStatus";

            try {

                ddlStatus.Items.Add("");
                ddlStatus.Items.Add("Closed");
                ddlStatus.Items.Add("Open");
                ddlStatus.SelectedIndex = 2;        // default to OPEN only

                ddlEditStatus.Items.Add("Closed");
                ddlEditStatus.Items.Add("Open");
                ddlEditStatus.SelectedIndex = 2;
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillEditSeverity() {

            const string METHOD_NAME = "FillEditSeverity";

            try {

                ddlEditSeverity.Items.Add("");
                ddlEditSeverity.Items.Add("Critical");
                ddlEditSeverity.Items.Add("High");
                ddlEditSeverity.Items.Add("Medium");
                ddlEditSeverity.Items.Add("Low");
                ddlEditSeverity.SelectedIndex = 0;
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillApplication() {

            const string METHOD_NAME = "FillApplication";

            try {

                ddlEditApplication.Items.Add("WSCIEMP");
                ddlEditApplication.Items.Add("Other");
                ddlEditApplication.SelectedIndex = 0;
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillResultsGrid() {


            const string METHOD_NAME = "FillResultsGrid";

            try {

                txtEditAction.Text = "";
                txtEditClient.Text = "";
                txtEditErrorCode.Text = "";
                txtEditServer.Text = "";
                txtEditUser.Text = "";
                txtErrorText.Text = "";

                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                string status = Common.UILib.GetDropDownText(ddlStatus);

                List<ListAppErrorItem> errorFiles = AppError.AppErrorFileGet(status, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));

                grdResult.SelectedIndex = -1;
                if (errorFiles.Count > 0) {
                    grdResult.DataSource = errorFiles;
                } else {
                    grdResult.DataSource = null;
                }
                grdResult.DataBind();
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void grdResult_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "grdResult_SelectedIndexChanged";

            try {

                GridViewRow row = grdResult.SelectedRow;
                string path = row.Cells[Convert.ToInt32(ErrorResultColumn.errColPath)].Text;
                ClearEditValues();
                SetEditControls(row);

                if (Common.CodeLib.IsNumeric(path)) {
                    txtErrorText.Text = (AppError.AppErrorFileGetDbDetail(Convert.ToInt32(path)));
                } else {
                    txtErrorText.Text = File.ReadAllText(path);
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void SetEditControls(GridViewRow row) {

            const string METHOD_NAME = "SetEditControls";

            try {

                Common.UILib.SelectDropDown(ddlEditApplication, row.Cells[Convert.ToInt32(ErrorResultColumn.errColAppName)].Text.Replace("&nbsp;", ""));
                Common.UILib.SelectDropDown(ddlEditSeverity, row.Cells[Convert.ToInt32(ErrorResultColumn.errColSeverity)].Text.Replace("&nbsp;", ""));
                Common.UILib.SelectDropDown(ddlEditStatus, row.Cells[Convert.ToInt32(ErrorResultColumn.errColStatus)].Text.Replace("&nbsp;", ""));
                txtEditAction.Text = row.Cells[Convert.ToInt32(ErrorResultColumn.errColAction)].Text.Replace("&nbsp;", "");

                txtEditServer.Text = row.Cells[Convert.ToInt32(ErrorResultColumn.errColServer)].Text.Replace("&nbsp;", "");
                txtEditClient.Text = row.Cells[Convert.ToInt32(ErrorResultColumn.errColClient)].Text.Replace("&nbsp;", "");
                txtEditUser.Text = row.Cells[Convert.ToInt32(ErrorResultColumn.errColUser)].Text.Replace("&nbsp;", "");
                txtEditErrorCode.Text = row.Cells[Convert.ToInt32(ErrorResultColumn.errColCode)].Text.Replace("&nbsp;", "");
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }

        }

        protected void grdResult_RowDataBound(object sender, GridViewRowEventArgs e) {

            if (e.Row.RowType == DataControlRowType.DataRow) {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                e.Row.Attributes.Add("onClick", e.Row.FindControl("Button1").ClientID + ".click();");
            }
        }

        protected void grdResult_RowCreated(object sender, GridViewRowEventArgs e) {

            //========================================================================
            // This is just too funny.  I call it either a BUG or a Design-MISTAKE!
            // In order to Hide a grid row that you want to hold data you must
            // turn visibility off here, after databinding has taken place.  It 
            // seems the control was not designed to understand this basic need.
            //========================================================================
            if (e.Row.RowType != DataControlRowType.EmptyDataRow) {
                e.Row.Cells[Convert.ToInt32(ErrorResultColumn.errColHiddenSelect)].CssClass = "DisplayNone";
                // Hide the path column
                e.Row.Cells[Convert.ToInt32(ErrorResultColumn.errColPath)].Visible = false;
            }
        }

        protected void btnDeleteErrorFile_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDeleteErrorFile_Click";

            try {

                GridViewRow row = grdResult.SelectedRow;
                string path = row.Cells[Convert.ToInt32(ErrorResultColumn.errColPath)].Text;

                if (Common.CodeLib.IsNumeric(path)) {
                    AppError.AppErrorFileDelete(Convert.ToInt32(path));
                } else {
                    File.Delete(path);
                }

                ClearEditValues();
                FillResultsGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnRefresh_Click";

            try {

                ClearEditValues();
                FillResultsGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlStatus_SelectedIndexChanged";

            try {
                FillResultsGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnImportErrorFile_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnImportErrorFile_Click";

            try {

                GridViewRow row = grdResult.SelectedRow;
                string path = row.Cells[Convert.ToInt32(ErrorResultColumn.errColPath)].Text;
                int appErrorInfoID = 0;
                string appName = "";
                string severity = "";
                DateTime errorDate;
                string errorStatus = "";
                string errorAction = "";
                string errorCode = "";
                string errorText = "";
                string loginServer = "";
                string loginClient = "";
                string loginUser = "";
                string errorBreakText = txtEditBreakText.Text;
                string userName = Common.AppHelper.GetIdentityName();

                if (Common.CodeLib.IsNumeric(path)) {
                    appErrorInfoID = Convert.ToInt32(path);
                }

                appName = Common.UILib.GetDropDownText(ddlEditApplication);
                severity = Common.UILib.GetDropDownText(ddlEditSeverity);
                errorDate = Convert.ToDateTime(row.Cells[Convert.ToInt32(ErrorResultColumn.errColDate)].Text);
                errorStatus = Common.UILib.GetDropDownText(ddlEditStatus);
                errorAction = txtEditAction.Text;
                errorDate = Convert.ToDateTime(row.Cells[Convert.ToInt32(ErrorResultColumn.errColDate)].Text);
                errorCode = txtEditErrorCode.Text;
                errorText = txtErrorText.Text;
                loginServer = txtEditServer.Text;
                loginClient = txtEditClient.Text;
                loginUser = txtEditUser.Text;

                string[] errorBlocks = txtErrorText.Text.Split(new string[] { errorBreakText }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string eBlock in errorBlocks) {

                    AppError.AppErrorFileSave(appErrorInfoID, appName, severity, errorDate, errorStatus, errorAction,
                        loginServer, loginClient, loginUser, errorCode, userName, eBlock);

                    // Perhaps this was an update, but if we're splitting the error text into multiple errors,
                    // the subsequent updates must really become inserts of new errors.  That's why we reset
                    // the appErrorInfoID.
                    if (appErrorInfoID > 0) {
                        appErrorInfoID = 0;
                    }
                }

                // after a successful import, delete the file, the repop the grid
                if (!Common.CodeLib.IsNumeric(path)) {
                    File.Delete(path);
                }

                ClearEditValues();
                FillResultsGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnOpenStatus_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnOpenStatus_Click";

            try {

                GridViewRow row = grdResult.SelectedRow;
                string path = row.Cells[Convert.ToInt32(ErrorResultColumn.errColPath)].Text;

                if (Common.CodeLib.IsNumeric(path)) {

                    int appErrorInfoID = Convert.ToInt32(path);
                    string appName = Common.UILib.GetDropDownText(ddlEditApplication);
                    string severity = Common.UILib.GetDropDownText(ddlEditSeverity);
                    DateTime errorDate = Convert.ToDateTime(row.Cells[Convert.ToInt32(ErrorResultColumn.errColDate)].Text);
                    string errorStatus = "Open";
                    string errorAction = txtEditAction.Text;
                    string errorCode = txtEditErrorCode.Text;
                    string errorText = txtErrorText.Text;
                    string loginServer = txtEditServer.Text;
                    string loginClient = txtEditClient.Text;
                    string loginUser = txtEditUser.Text;
                    string userName = Common.AppHelper.GetIdentityName();

                    AppError.AppErrorFileSave(appErrorInfoID, appName, severity, errorDate, errorStatus, errorAction,
                        loginServer, loginClient, loginUser, errorCode, userName, errorText);

                } else {
                    Common.CWarning warn = new Common.CWarning("You cannot set the status of a file based error.  You need to first Import this error.");
                    throw (warn);
                }

                ClearEditValues();
                FillResultsGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void ClearEditValues() {

            const string METHOD_NAME = "btnCloseStatus_Click";

            try {

                ddlEditApplication.SelectedIndex = 0;
                ddlEditSeverity.SelectedIndex = 0;
                ddlEditStatus.SelectedIndex = 0;
                txtEditAction.Text = "";
                txtEditServer.Text = "";
                txtEditClient.Text = "";
                txtEditUser.Text = "";
                txtEditErrorCode.Text = "";

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void btnCloseStatus_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnCloseStatus_Click";

            try {

                GridViewRow row = grdResult.SelectedRow;
                string path = row.Cells[Convert.ToInt32(ErrorResultColumn.errColPath)].Text;

                if (Common.CodeLib.IsNumeric(path)) {

                    int appErrorInfoID = Convert.ToInt32(path);
                    string appName = Common.UILib.GetDropDownText(ddlEditApplication);
                    string severity = Common.UILib.GetDropDownText(ddlEditSeverity);
                    DateTime errorDate = Convert.ToDateTime(row.Cells[Convert.ToInt32(ErrorResultColumn.errColDate)].Text);
                    string errorStatus = "Closed";
                    string errorAction = txtEditAction.Text;
                    string errorCode = txtEditErrorCode.Text;
                    string errorText = txtErrorText.Text;
                    string loginServer = txtEditServer.Text;
                    string loginClient = txtEditClient.Text;
                    string loginUser = txtEditUser.Text;
                    string userName = Common.AppHelper.GetIdentityName();

                    AppError.AppErrorFileSave(appErrorInfoID, appName, severity, errorDate, errorStatus, errorAction,
                        loginServer, loginClient, loginUser, errorCode, userName, errorText);

                } else {
                    Common.CWarning warn = new Common.CWarning("You cannot set the status of a file based error.  You need to first Import this error.");
                    throw (warn);
                }

                ClearEditValues();
                FillResultsGrid();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
