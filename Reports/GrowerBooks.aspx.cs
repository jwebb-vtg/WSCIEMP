using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using WSCData;


namespace WSCIEMP.Reports {

    public partial class GrowerBooks : Common.BasePage {

        private const string MOD_NAME = "Reports.GrowerBooks.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                if (!Page.IsPostBack) {
                    tvwBrowse.Nodes[0].Value = Server.MapPath(tvwBrowse.Nodes[0].NavigateUrl); 
                }

            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {

            // on Render, ensure that the node for this year and last year if fully expanded.
            // Pass fully expand any children of the root node having a year stamp of this year or last year.
            TreeNode tn = null;
            if (tvwBrowse.Nodes.Count > 0) {
                tn = tvwBrowse.Nodes[0];
                if (tn.ChildNodes.Count > 0) {

                    foreach (TreeNode tnc in tn.ChildNodes) {
                        if (tnc.Text.Contains(YearNow) || tnc.Text.Contains(YearPrior)) {
                            tnc.Expanded = true;
                            if (tnc.ChildNodes.Count > 0) {
                                foreach (TreeNode tnc2Child in tnc.ChildNodes) {
                                    tnc2Child.Expanded = true;
                                }
                            }
                        }
                    }
                }
            }

            base.Render(writer);
        }

        protected void tvwBrowse_TreeNodePopulate(object sender, TreeNodeEventArgs e) {

            const string METHOD_NAME = "tvwBrowse_TreeNodePopulate";

            try {

                LoadChildNode(e.Node);
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void LoadChildNode(TreeNode node) {

            const string METHOD_NAME = "LoadChildNode";

            DirectoryInfo directory = new DirectoryInfo(node.Value);
            string rootPath = tvwBrowse.Nodes[0].Value;
            string rootVirtualPath = tvwBrowse.Nodes[0].NavigateUrl;
            TreeNode subNode = null;

            try {

                foreach (DirectoryInfo subDir in directory.GetDirectories()) {

                    subNode = new TreeNode(subDir.Name);
                    subNode.Value = subDir.FullName;

                    if (subDir.GetDirectories().Length > 0 || subDir.GetFiles().Length > 0) {

                        subNode.SelectAction = TreeNodeSelectAction.Select;
                        subNode.PopulateOnDemand = true;

                    } else {

                        subNode.SelectAction = TreeNodeSelectAction.None;
                        subNode.PopulateOnDemand = false;
                    }

                    node.ChildNodes.Add(subNode);
                }

                // Prep work for setting subNode's Navigate URL.
                string nodeRelativeURL = node.Value.Replace(tvwBrowse.Nodes[0].Value, "");
                string nodeURL = "";

                if (nodeRelativeURL.Length > 0) {
                    nodeURL = rootVirtualPath + @"/" + nodeRelativeURL;
                } else {
                    nodeURL = rootVirtualPath;
                }

                foreach (FileInfo fi in directory.GetFiles()) {

                    subNode = new TreeNode(fi.Name);
                    subNode.NavigateUrl = nodeURL + @"/" + fi.Name;
                    subNode.Target = "_blank";
                    node.ChildNodes.Add(subNode);                    
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }            
        }

        private string _yearNow = "";
        private string YearNow {
            get {
                if (_yearNow.Length == 0) {
                    _yearNow = WSCField.GetCropYears()[0].ToString();
                }
                return _yearNow;
            }
        }
        private string _yearPrior = "";
        private string YearPrior {
            get {
                if (_yearPrior.Length == 0) {
                    _yearPrior = WSCField.GetCropYears()[1].ToString();
                }
                return _yearPrior;
            }
        }
    }
}
