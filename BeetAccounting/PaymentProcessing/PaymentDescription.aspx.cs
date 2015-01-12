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

namespace WSCIEMP.BeetAccounting.PaymentProcessing {

    public partial class PaymentDescription : Common.BasePage {

        private const string MOD_NAME = "BeetAccounting.PaymentProcessing.PaymentDescription.";
        private int _cropYear = 0;

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));

                if (!Page.IsPostBack) {

                    WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());                    
                    FillRequired();
                    FillFinished();
                    _cropYear = Int32.Parse(Common.UILib.GetDropDownText(ddlCropYear));
                    FillPaymentDescription();
                    EnableDisablePayments();
                } else {
                    _cropYear = Int32.Parse(Common.UILib.GetDropDownText(ddlCropYear));
                }

            }
            catch (System.Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
//				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        private void FillPaymentDescription() {

            ClearPaymentDescription();
            List<ListPaymentDescItem> stateList = BeetDataDomain.GetPaymentDescriptions(_cropYear, false);

            foreach (ListPaymentDescItem state in stateList) {

                int paymentNumber = Convert.ToInt32(state.PaymentNumber);

                switch (paymentNumber) {
                    case 1:
                        ShowPaymentInfo(state, lblPayNum1, ddlFinished1, ddlRequired1,
                            txtTransmittalDate1, txtPaymentDesc1, txtPaymentDescID1);
                        break;

                    case 2:
                        ShowPaymentInfo(state, lblPayNum2, ddlFinished2, ddlRequired2,
                            txtTransmittalDate2, txtPaymentDesc2, txtPaymentDescID2);
                        break;

                    case 3:
                        ShowPaymentInfo(state, lblPayNum3, ddlFinished3, ddlRequired3,
                            txtTransmittalDate3, txtPaymentDesc3, txtPaymentDescID3);
                        break;

                    case 4:
                        ShowPaymentInfo(state, lblPayNum4, ddlFinished4, ddlRequired4,
                            txtTransmittalDate4, txtPaymentDesc4, txtPaymentDescID4);
                        break;

                    case 5:
                        ShowPaymentInfo(state, lblPayNum5, ddlFinished5, ddlRequired5,
                            txtTransmittalDate5, txtPaymentDesc5, txtPaymentDescID5);
                        break;

                    case 6:
                        ShowPaymentInfo(state, lblPayNum6, ddlFinished6, ddlRequired6,
                            txtTransmittalDate6, txtPaymentDesc6, txtPaymentDescID6);
                        break;

                    case 7:
                        ShowPaymentInfo(state, lblPayNum7, ddlFinished7, ddlRequired7,
                            txtTransmittalDate7, txtPaymentDesc7, txtPaymentDescID7);
                        break;

                    case 8:
                        ShowPaymentInfo(state, lblPayNum8, ddlFinished8, ddlRequired8,
                            txtTransmittalDate8, txtPaymentDesc8, txtPaymentDescID8);
                        break;

                    case 9:
                        ShowPaymentInfo(state, lblPayNum9, ddlFinished9, ddlRequired9,
                            txtTransmittalDate9, txtPaymentDesc9, txtPaymentDescID9);
                        break;

                    case 10:
                        ShowPaymentInfo(state, lblPayNum10, ddlFinished10, ddlRequired10,
                            txtTransmittalDate10, txtPaymentDesc10, txtPaymentDescID10);
                        break;
                }
            }
        }

        private void ClearPaymentDescription() {

            Common.UILib.SelectDropDown(ddlRequired1, 0);
            Common.UILib.SelectDropDown(ddlRequired2, 0);
            Common.UILib.SelectDropDown(ddlRequired3, 0);
            Common.UILib.SelectDropDown(ddlRequired4, 0);
            Common.UILib.SelectDropDown(ddlRequired5, 0);
            Common.UILib.SelectDropDown(ddlRequired6, 0);
            Common.UILib.SelectDropDown(ddlRequired7, 0);
            Common.UILib.SelectDropDown(ddlRequired8, 0);
            Common.UILib.SelectDropDown(ddlRequired9, 0);
            Common.UILib.SelectDropDown(ddlRequired10, 0);

            Common.UILib.SelectDropDown(ddlFinished1, 0);
            Common.UILib.SelectDropDown(ddlFinished2, 0);
            Common.UILib.SelectDropDown(ddlFinished3, 0);
            Common.UILib.SelectDropDown(ddlFinished4, 0);
            Common.UILib.SelectDropDown(ddlFinished5, 0);
            Common.UILib.SelectDropDown(ddlFinished6, 0);
            Common.UILib.SelectDropDown(ddlFinished7, 0);
            Common.UILib.SelectDropDown(ddlFinished8, 0);
            Common.UILib.SelectDropDown(ddlFinished9, 0);
            Common.UILib.SelectDropDown(ddlFinished10, 0);

            lblPayNum1.Text = "";
            lblPayNum2.Text = "";
            lblPayNum3.Text = "";
            lblPayNum4.Text = "";
            lblPayNum5.Text = "";
            lblPayNum6.Text = "";
            lblPayNum7.Text = "";
            lblPayNum8.Text = "";
            lblPayNum9.Text = "";
            lblPayNum10.Text = "";

            txtTransmittalDate1.Text = "";
            txtTransmittalDate2.Text = "";
            txtTransmittalDate3.Text = "";
            txtTransmittalDate4.Text = "";
            txtTransmittalDate5.Text = "";
            txtTransmittalDate6.Text = "";
            txtTransmittalDate7.Text = "";
            txtTransmittalDate8.Text = "";
            txtTransmittalDate9.Text = "";
            txtTransmittalDate10.Text = "";

            txtPaymentDesc1.Text = "";
            txtPaymentDesc2.Text = "";
            txtPaymentDesc3.Text = "";
            txtPaymentDesc4.Text = "";
            txtPaymentDesc5.Text = "";
            txtPaymentDesc6.Text = "";
            txtPaymentDesc7.Text = "";
            txtPaymentDesc8.Text = "";
            txtPaymentDesc9.Text = "";
            txtPaymentDesc10.Text = "";

            txtPaymentDescID1.Text = "";
            txtPaymentDescID2.Text = "";
            txtPaymentDescID3.Text = "";
            txtPaymentDescID4.Text = "";
            txtPaymentDescID5.Text = "";
            txtPaymentDescID6.Text = "";
            txtPaymentDescID7.Text = "";
            txtPaymentDescID8.Text = "";
            txtPaymentDescID9.Text = "";
            txtPaymentDescID10.Text = "";
        }

        private void ShowPaymentInfo(ListPaymentDescItem state, Label lblPayNum, DropDownList ddlFinished,
            DropDownList ddlRequired, TextBox txtTransmittalDate, TextBox txtPaymentDesc, TextBox txtPaymentDescID) {

            lblPayNum.Text = state.PaymentNumber;
            bool tmpBool = state.IsFinished;
            Common.UILib.SelectDropDown(ddlFinished, (tmpBool == true ? 1 : 0));
            tmpBool = state.IsRequired;
            Common.UILib.SelectDropDown(ddlRequired, (tmpBool == true ? 1 : 0));
            txtTransmittalDate.Text = state.TransmittalDate;
            txtPaymentDesc.Text = state.PaymentDesc;
            txtPaymentDescID.Text = state.PaymentDescID;
        }

        private void FillRequired() {

            Common.UILib.FillYesNo(ddlRequired1, "No");
            Common.UILib.FillYesNo(ddlRequired2, "No");
            Common.UILib.FillYesNo(ddlRequired3, "No");
            Common.UILib.FillYesNo(ddlRequired4, "No");
            Common.UILib.FillYesNo(ddlRequired5, "No");
            Common.UILib.FillYesNo(ddlRequired6, "No");
            Common.UILib.FillYesNo(ddlRequired7, "No");
            Common.UILib.FillYesNo(ddlRequired8, "No");
            Common.UILib.FillYesNo(ddlRequired9, "No");
            Common.UILib.FillYesNo(ddlRequired10, "No");
        }

        private void FillFinished() {

            Common.UILib.FillYesNo(ddlFinished1, "No");
            Common.UILib.FillYesNo(ddlFinished2, "No");
            Common.UILib.FillYesNo(ddlFinished3, "No");
            Common.UILib.FillYesNo(ddlFinished4, "No");
            Common.UILib.FillYesNo(ddlFinished5, "No");
            Common.UILib.FillYesNo(ddlFinished6, "No");
            Common.UILib.FillYesNo(ddlFinished7, "No");
            Common.UILib.FillYesNo(ddlFinished8, "No");
            Common.UILib.FillYesNo(ddlFinished9, "No");
            Common.UILib.FillYesNo(ddlFinished10, "No");
        }

        private void EnableDisablePayments() {

            bool allowAddlPayments = false;

            allowAddlPayments = (txtPaymentDescID7.Text.Length > 0);
            ddlRequired7.Enabled = allowAddlPayments;
            ddlFinished7.Enabled = allowAddlPayments;
            lblPayNum7.Enabled = allowAddlPayments;
            txtTransmittalDate7.Enabled = allowAddlPayments;
            txtPaymentDesc7.Enabled = allowAddlPayments;
            txtPaymentDescID7.Enabled = allowAddlPayments;

            allowAddlPayments = (txtPaymentDescID8.Text.Length > 0);
            ddlRequired8.Enabled = allowAddlPayments;
            ddlFinished8.Enabled = allowAddlPayments;
            lblPayNum8.Enabled = allowAddlPayments;
            txtTransmittalDate8.Enabled = allowAddlPayments;
            txtPaymentDesc8.Enabled = allowAddlPayments;
            txtPaymentDescID8.Enabled = allowAddlPayments;

            allowAddlPayments = (txtPaymentDescID9.Text.Length > 0);
            ddlRequired9.Enabled = allowAddlPayments;
            ddlFinished9.Enabled = allowAddlPayments;
            lblPayNum9.Enabled = allowAddlPayments;
            txtTransmittalDate9.Enabled = allowAddlPayments;
            txtPaymentDesc9.Enabled = allowAddlPayments;
            txtPaymentDescID9.Enabled = allowAddlPayments;

            allowAddlPayments = (txtPaymentDescID10.Text.Length > 0);
            ddlRequired10.Enabled = allowAddlPayments;
            ddlFinished10.Enabled = allowAddlPayments;
            lblPayNum10.Enabled = allowAddlPayments;
            txtTransmittalDate10.Enabled = allowAddlPayments;
            txtPaymentDesc10.Enabled = allowAddlPayments;
            txtPaymentDescID10.Enabled = allowAddlPayments;
        }

        private void PaymentDescriptionSave(Label lblPayNum, DropDownList ddlFinished,
            DropDownList ddlRequired, TextBox txtTransmittalDate,
            TextBox txtPaymentDesc, TextBox txtPaymentDescID) {

            try {

                // validate data
                if (txtTransmittalDate.Text.Length > 0) {
                    if (!Common.CodeLib.IsDate(txtTransmittalDate.Text)) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("If you enter a Transmittal Date for payment number " +
                            lblPayNum.Text + ", it must be a valid date (mm/dd/yyyy).");
                        throw warn;
                    }
                }
                WSCSecurity auth = Globals.SecurityState;                
                WSCPayment.PaymentDescriptionSave(Int32.Parse(txtPaymentDescID.Text),
                    Int32.Parse(lblPayNum.Text), _cropYear,
                    txtPaymentDesc.Text,
                    (Common.UILib.GetDropDownText(ddlRequired) == "Yes" ? true : false),
                    (Common.UILib.GetDropDownText(ddlFinished) == "Yes" ? true : false),
                    txtTransmittalDate.Text, auth.UserName);
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException("PaymentDescription.PaymentDescriptionSave", ex);
                throw (wex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            try {

                //Payment 1
                PaymentDescriptionSave(lblPayNum1, ddlFinished1,
                    ddlRequired1, txtTransmittalDate1,
                    txtPaymentDesc1, txtPaymentDescID1);

                //Payment 2
                PaymentDescriptionSave(lblPayNum2, ddlFinished2,
                    ddlRequired2, txtTransmittalDate2,
                    txtPaymentDesc2, txtPaymentDescID2);

                //Payment 3
                PaymentDescriptionSave(lblPayNum3, ddlFinished3,
                    ddlRequired3, txtTransmittalDate3,
                    txtPaymentDesc3, txtPaymentDescID3);

                //Payment 4
                PaymentDescriptionSave(lblPayNum4, ddlFinished4,
                    ddlRequired4, txtTransmittalDate4,
                    txtPaymentDesc4, txtPaymentDescID4);

                //Payment 5
                PaymentDescriptionSave(lblPayNum5, ddlFinished5,
                    ddlRequired5, txtTransmittalDate5,
                    txtPaymentDesc5, txtPaymentDescID5);

                //Payment 6
                PaymentDescriptionSave(lblPayNum6, ddlFinished6,
                    ddlRequired6, txtTransmittalDate6,
                    txtPaymentDesc6, txtPaymentDescID6);

                if (txtPaymentDescID7.Text.Length > 0) {

                    //Payment 7
                    PaymentDescriptionSave(lblPayNum7, ddlFinished7,
                        ddlRequired7, txtTransmittalDate7,
                        txtPaymentDesc7, txtPaymentDescID7);
                }

                if (txtPaymentDescID8.Text.Length > 0) {

                    //Payment 8
                    PaymentDescriptionSave(lblPayNum8, ddlFinished8,
                        ddlRequired8, txtTransmittalDate8,
                        txtPaymentDesc8, txtPaymentDescID8);
                }

                if (txtPaymentDescID9.Text.Length > 0) {

                    //Payment 9
                    PaymentDescriptionSave(lblPayNum9, ddlFinished9,
                        ddlRequired9, txtTransmittalDate9,
                        txtPaymentDesc9, txtPaymentDescID9);
                }

                if (txtPaymentDescID10.Text.Length > 0) {

                    //Payment 10
                    PaymentDescriptionSave(lblPayNum10, ddlFinished10,
                        ddlRequired10, txtTransmittalDate10,
                        txtPaymentDesc10, txtPaymentDescID10);
                }

                // Now display the updated data.
                FillPaymentDescription();

            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSave_Click";

            try {
                
                FillPaymentDescription();
                EnableDisablePayments();
            }
            catch (Exception ex) {
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
