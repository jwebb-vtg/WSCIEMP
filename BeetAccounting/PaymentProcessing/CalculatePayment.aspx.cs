using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Linq;
using WSCData;

namespace WSCIEMP.BeetAccounting.PaymentProcessing {

    public partial class CalculatePayment : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.PaymentProcessing.CalculatePayment.";

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                btnProcess.Attributes.Add("onclick", "confirm('Are you sure you want to calculate this payment?  Press Ok to continue or Cancel to abort.');");

                if (!Page.IsPostBack) {
                    WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    FillFactory();
                    FillPayment();
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillFactory() {

            const string METHOD_NAME = "FillFactory";

            try {

                ListBox lst = lstFactory;
                lst.Items.Clear();
                List<ListBeetFactoryIDItem> ftyList = BeetDataDomain.BeetFactoryIDGetList(Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear)));

                lst.DataTextField = "FactoryLongName";
                lst.DataValueField = "FactoryID";
                lst.DataSource = ftyList;
                lst.DataBind();

                if (lst.Items.Count > 0) {
                    lst.Items[0].Selected = true;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void FillPayment() {

            const string METHOD_NAME = "FillPayment";
            try {

                lstPayment.Items.Clear();

                lstPayment.DataValueField = "PaymentNumber";
                lstPayment.DataTextField = "PaymentNumDesc";

                // We want non-finished, but use Linq to remove the payments without valid descriptions.
                var payments = BeetDataDomain.GetPaymentDescriptions(Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear)), false)
                    .Where(p => p.PaymentDesc != "Blank");

                lstPayment.DataSource = payments;
                lstPayment.DataBind();           

                if (lstPayment.Items.Count > 0) {
                    lstPayment.Items[0].Selected = true;
                    btnProcess.Enabled = true;
                } else {
                    btnProcess.Enabled = false;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {
                DoProcessPayment();                
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }

        }

        private void DoProcessPayment() {

            const string METHOD_NAME = "DoProcessPayment";

            try {

                int factoryID = Convert.ToInt32(Common.UILib.GetListValues(lstFactory));
                int paymentNumber = Convert.ToInt32(Common.UILib.GetListValues(lstPayment));
                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    WSCPayment.PostCalculatePayment(conn, factoryID, paymentNumber, cropYear, Globals.SecurityState.UserName);
                }

                Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Payment Processing Complete!");
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }

        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "ddlCropYear_SelectedIndexChanged";

            try {

                FillFactory();
                FillPayment();                

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
