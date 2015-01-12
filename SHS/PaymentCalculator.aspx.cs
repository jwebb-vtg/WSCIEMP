using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;


namespace WSCIEMP.SHS {

    public partial class PaymentCalculator : Common.BasePage {

        private const string MOD_NAME = "SHS.PaymentCalculator.";
        private WSCShsData _shs = null;

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";
            Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

            try {

                _shs = Globals.ShsData;

                if (!Page.IsPostBack) {

                    FillCropYear();
                    lblQCPayment.Text = "$0.0000";
                }
            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillCropYear() {

            if (ddlCropYear.Items.Count > 0) {
                CropYear = Int32.Parse(ddlCropYear.SelectedValue);
            }
            if (CropYear == 0) {
                CropYear = _shs.CropYear;
            }
            if (CropYear == 0) {
                CropYear = Convert.ToInt32(WSCField.GetCropYears()[0].ToString());
            }

            WSCField.FillCropYear(ddlCropYear, CropYear.ToString());
        }

        protected void btnCalculate_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnCalculate_Click";

            try {

                Decimal qcBeetPaymentPerTon = 0, oldNorthBeetPaymentPerTon = 0, oldSouthBeetPaymentPerTon = 0;
                string sugarContent, slmPct, netReturn;
                int cropYear = Convert.ToInt32(ddlCropYear.SelectedItem.Text);

                sugarContent = txtSugarContent.Text;
                if (sugarContent.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter a Sugar Content.");
                    throw (warn);
                }
                if (!Common.CodeLib.IsNumeric(sugarContent)) {
                    Common.CWarning warn = new Common.CWarning("Please enter a number for Sugar Content, like 16.54.");
                    throw (warn);
                }
                slmPct = txtSLM.Text;
                if (slmPct.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter a SLM value.");
                    throw (warn);
                }
                if (!Common.CodeLib.IsNumeric(slmPct)) {
                    Common.CWarning warn = new Common.CWarning("Please enter a number for SLM, like 1.60.");
                    throw (warn);
                }
                netReturn = txtNetReturn.Text;
                if (netReturn.Length == 0) {
                    Common.CWarning warn = new Common.CWarning("Please enter a Net Return.");
                    throw (warn);
                }
                if (!Common.CodeLib.IsNumeric(netReturn)) {
                    Common.CWarning warn = new Common.CWarning("Please enter a number for Net Return, like 24.55.");
                    throw (warn);
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    WSCPayment.CalculatePayment(conn, Convert.ToDecimal(sugarContent),
                        Convert.ToDecimal(slmPct),
                        Convert.ToDecimal(netReturn), cropYear, ref qcBeetPaymentPerTon,
                        ref oldNorthBeetPaymentPerTon, ref oldSouthBeetPaymentPerTon);

                    lblQCPayment.Text = "$" + qcBeetPaymentPerTon.ToString();
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
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
