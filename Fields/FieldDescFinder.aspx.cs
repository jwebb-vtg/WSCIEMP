using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Fields {

    public partial class FieldDescFinder : Common.BasePage {

        private const string MOD_NAME = "Fields.FieldDescFinder.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning(divFFWarning);

                ShowFieldResults();
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // Add click event to GridView do not do this in _RowCreated or _RowDataBound
            AddRowSelectToGridView(grdResults);
            base.Render(writer);
        }

        private void AddRowSelectToGridView(GridView gv) {

            foreach (GridViewRow row in gv.Rows) {

                if (row.Cells[1].Text != "*") {
                    row.Attributes["onmouseover"] = "HoverOn(this)";
                    row.Attributes["onmouseout"] = "HoverOff(this)";
                    row.Attributes.Add("onclick", "SelectRow(this); SelectContract('" + row.Cells[0].Text + "');");
                    //row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), true));
                }
            }
        }

        private void ShowFieldResults() {

            try {

                // Get passed parameters
                System.Collections.Specialized.NameValueCollection queryStr = Request.QueryString;

                string fsaNumber = queryStr["fsaNumber"];
                if (fsaNumber == "null") {fsaNumber = "";}

                string fieldName = queryStr["fieldName"];
                if (fieldName == "null") { fieldName = ""; }

                string cy = queryStr["cy"];
                if (cy == "null") { cy = ""; }

                string acres = queryStr["acres"];
                if (acres == "null") { acres = ""; }

                string state = queryStr["state"];
                if (state == "null") { state = ""; }

                string county = queryStr["county"];
                if (county == "null") { county = ""; }

                string township = queryStr["township"];
                if (township == "null") { township = ""; }

                string range = queryStr["range"];
                if (range == "null") { range = ""; }

                string section = queryStr["section"];
                if (section == "null") { section = ""; }

                string quadrant = queryStr["quadrant"];
                if (quadrant == "null") { quadrant = ""; }

                string quarterQuadrant = queryStr["quarterQuadrant"];
                if (quarterQuadrant == "null") { quarterQuadrant = ""; }

                string latitude = queryStr["latitude"];
                if (latitude == "null") { latitude = ""; }

                string longitude = queryStr["longitude"];
                if (longitude == "null") { longitude = ""; }

                string description = queryStr["description"];
                if (description == "null") { description = ""; }

                string fsaState = queryStr["fsaState"];
                if (fsaState == "null") { fsaState = ""; }

                string fsaCounty = queryStr["fsaCounty"];
                if (fsaCounty == "null") { fsaCounty = ""; }

                string farmNo = queryStr["farmNo"];
                if (farmNo == "null") { farmNo = ""; }

                string tractNo = queryStr["tractNo"];
                if (tractNo == "null") { tractNo = ""; }

                string fieldNo = queryStr["fieldNo"];
                if (fieldNo == "null") { fieldNo = ""; }

                string quarterField = queryStr["quarterField"];
                if (quarterField == "null") { quarterField = ""; }

                List<ListBeetFieldFinderItem> stateList = WSCData.WSCField.FieldFind(Convert.ToInt32(cy), fsaNumber, fieldName, state,
                    county, township, range, section, quadrant, quarterQuadrant, description, fsaState, fsaCounty, farmNo,
                    tractNo, fieldNo, quarterField);

                    grdResults.DataSource = stateList;
                    grdResults.DataBind();

                    if (stateList.Count == 300) {
                        Common.AppHelper.ShowWarning(divFFWarning, "Your criteria is too general and resulted in more than 300 fields.  " +
                            "This dialog may be VERY SLOW to respond.  Try entering more specific criteria and select Find Field again.  " +
                            "Your results were truncated to the first 300 fields.");
                    }
                
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("FieldDescFinder.ShowFieldResults", ex);
                throw (wex);
            }
        }

        protected void grdResults_RowCreated(object sender, GridViewRowEventArgs e) {

            //========================================================================
            // This is just too funny.  I call it either a BUG or a Design-MISTAKE!
            // In order to Hide a grid row that you want to hold data you must
            // turn visibility off here, after databinding has taken place.  It 
            // seems the control was not designed to understand this basic need.
            //========================================================================
            if (e.Row.RowType != DataControlRowType.EmptyDataRow) {
                e.Row.Cells[0].CssClass = "DisplayNone";
            }
        }
    }
}
