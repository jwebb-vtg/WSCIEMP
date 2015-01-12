using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class DeductionImport : Common.BasePage {

        private const string MOD_NAME = "Admin.DeductionImport.";
        private const string FILE_LABEL = "File Path: ";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

            try {

                btnDelete.Enabled = false;
                btnPost.Enabled = false;

                if (!Page.IsPostBack) {
                    WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    lblOrigPath.Visible = false;
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillResults(string filePath) {

            const string METHOD_NAME = "FillResults";
            DataSet ds;
            decimal dAmt;
            string cntNo = "";
            string amt = "";
            string ded = "";

			string dbg = "0";

            try {

                string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;";
				dbg = "1";
                using (System.Data.OleDb.OleDbConnection xConn = new System.Data.OleDb.OleDbConnection(connStr)) {
					dbg = "2";
                    xConn.Open();
					dbg = "3";
                    ds = new DataSet();
					dbg = "4";
                    System.Data.OleDb.OleDbDataAdapter xAdapt = new System.Data.OleDb.OleDbDataAdapter("select Contract, Amount, Deduction from [Deductions$] where len(Contract) > 0", xConn);
					dbg = "5";
                    xAdapt.Fill(ds);
					dbg = "6";

                    // fix amounts
                    foreach (System.Data.DataRow dRow in ds.Tables[0].Rows) {
                        try {
							dbg = "7";
                            cntNo = dRow["Contract"].ToString();
							dbg = "8";
                            if (cntNo.Length == 0) {
                                break;
                            }
                            amt = dRow["Amount"].ToString();
							dbg = "9";
                            ded = dRow["Deduction"].ToString();
							dbg = "10";

                            dAmt = Math.Round(Convert.ToDecimal(amt), 2);
							dbg = "11";
                            dRow["Amount"] = dAmt.ToString("#.00");
							dbg = "12";
                        }
                        catch (Exception ex) {
                            WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Error processing the Amount, " + amt + ", for Contract " + cntNo + " with Deduction " + ded + ".", ex);
                            throw (warn);
                        }
                    }
                }

                // Save excel data into an XML document.
                string fileExt = System.IO.Path.GetExtension(filePath);
				dbg = "13";
                string xmlFilePath = filePath.Replace(fileExt, ".xml");
				dbg = "14";
                ds.WriteXml(xmlFilePath, System.Data.XmlWriteMode.IgnoreSchema);
				dbg = "15";

                // Retrieve the xml formatted input data and qualify it in the database.
                string xmlData = "";
				dbg = "16";
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
				dbg = "17";

                // Retrieve the xml document of deductions: contract, amount, and deduction.
                using (System.IO.StreamReader reader = new StreamReader(xmlFilePath)) {
					dbg = "18";
                    xmlData = reader.ReadToEnd().Replace("\r\n", "");
					dbg = "19";
                }

                // Send to db to qualify
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
					dbg = "20";
                    SqlDataReader dr = WSCAdmin.ContractDeductionQualify(conn, cropYear, xmlData);
					dbg = "21";
                    // Load results into  grid
                    grdResults.DataSource = dr;
					dbg = "22";
                    grdResults.DataBind();dbg = "20";
					dbg = "23";
                }

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME
					+ "\filePath: " + filePath
					+ "\ndbg: " + dbg, ex);
                throw (wex);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnUpload_Click";

            try {

                if (uploadFile.PostedFile != null) {
                    
                    string filePath = uploadFile.PostedFile.FileName;

                    lblOrigPath.Visible = true;
                    lblOrigPath.Text = FILE_LABEL + filePath;

                    if (!filePath.ToUpper().EndsWith("XLS")) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You must selet an Excel workbook to upload.");
                        throw (warn);
                    }

                    string fileName = System.IO.Path.GetFileName(filePath);
                    string saveFileFullName = Server.MapPath(@"~/PDF") + @"\" + fileName;
                    uploadFile.PostedFile.SaveAs(saveFileFullName);

                    // We only get here if everything works.
                    lblOrigPath.Visible = true;
                    lblOrigPath.Text = FILE_LABEL + fileName;

                    // Now process the saveFileFullName
                    FillResults(saveFileFullName);

                    if (grdResults.Rows.Count > 0) {
                        btnDelete.Enabled = true;
                        btnPost.Enabled = true;
                    } else {
                        btnDelete.Enabled = false;
                        btnPost.Enabled = false;
                    }
                }
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnPost_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnPost_Click";

            /*
                <?xml version="1.0" standalone="yes" ?> 
                <NewDataSet>
                    <Table>
                        <Contract>10018</Contract> 
                        <Amount>43.22</Amount> 
                        <Deduction>1,2</Deduction> 
                    </Table>
                </NewDataSet>	
            */
            try {

                string xmlData = "";

                // Retrieve the xml document of deductions: contract, amount, and deduction.
                string selectedPath = lblOrigPath.Text;
                if (selectedPath.StartsWith(FILE_LABEL)) {
                    selectedPath = selectedPath.Substring(FILE_LABEL.Length);
                }

                string fileName = System.IO.Path.GetFileName(selectedPath);
                string filePath = Server.MapPath(@"~/PDF") + @"\" + fileName;

                string xmlFilePath = filePath.Replace(System.IO.Path.GetExtension(filePath), ".xml");
                using (System.IO.StreamReader reader = new StreamReader(xmlFilePath)) {
                    xmlData = reader.ReadToEnd().Replace("\r\n", "");
                }

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                // Send this xml document, the crop year, and user name to the database.
                WSCSecurity auth = Globals.SecurityState;
                WSCAdmin.ContractDeductionSave(cropYear, auth.UserName, xmlData);

                // We only get here if everything works.
                lblOrigPath.Visible = true;
                lblOrigPath.Text = "Successfully Posted Deductions for: " + fileName;

            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnDelete_Click";

            try {

                string xmlData = "";

                // Retrieve the xml document of deductions: contract, amount, and deduction.
                string selectedPath = lblOrigPath.Text;
                if (selectedPath.StartsWith(FILE_LABEL)) {
                    selectedPath = selectedPath.Substring(FILE_LABEL.Length);
                }

                string fileName = System.IO.Path.GetFileName(selectedPath);
                string filePath = Server.MapPath(@"~/PDF") + @"\" + fileName;

                string xmlFilePath = filePath.Replace(System.IO.Path.GetExtension(filePath), ".xml");
                using (System.IO.StreamReader reader = new StreamReader(xmlFilePath)) {
                    xmlData = reader.ReadToEnd().Replace("\r\n", "");
                }

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                // Send this xml document, the crop year, and user name to the database.
                WSCSecurity auth = Globals.SecurityState;
                WSCAdmin.ContractDeductionDelete(cropYear, auth.UserName, xmlData);

                // We only get here if everything works.
                lblOrigPath.Visible = true;
                lblOrigPath.Text = "Successfully Deleted Deductions for: " + fileName;

            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }	
        }
    }
}
