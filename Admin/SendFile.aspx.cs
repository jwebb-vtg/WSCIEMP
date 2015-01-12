using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using WSCData;

namespace WSCIEMP.Admin {

    public partial class SendFile : Common.BasePage {

        private const string MOD_NAME = "Admin.SendFile.";
        const int BATCH_SIZE = 75;

        private bool _isErrorDuringSummary = false;

        protected void Page_Load(object sender, EventArgs e) {

            const string METHOD_NAME = "Page_Load";

            try {

                Common.AppHelper.HideWarning((HtmlGenericControl)Master.FindControl("divWarning"));
                ((HtmlGenericControl)Master.FindControl("divWarning")).Attributes.Add("style", @"text-align:center; width:98%;");

                HtmlGenericControl body = (HtmlGenericControl)this.Master.FindControl("MasterBody");
                body.Attributes.Add("onload", "DoOnLoad();");

                // When nothing batch processing, reset data stores
                if (txtContinueBatch.Value == "") {
                    lstPendingEmail.Items.Clear();
                    hideGoodEmail.Value = "";
                    hideBadEmail.Value = "";
                }

                // Next, turn batch process off for client.  If needed, btnSendEmail_Click will turn this on.
                txtContinueBatch.Value = "";

                if (!Page.IsPostBack) {

                    // Add instructional message.
                    string infoMsg = "After you press 'Send Email' this page may reload several times to process your request.  " +
                        "Do not change or touch anything on this page until the process ends!  Progress or failure information will appear here.";

                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), infoMsg);
                    ((HtmlGenericControl)Master.FindControl("divWarning")).Attributes.Add("style", @"text-align:left; width:98%; background-color:#ffff66;");       //ffff66,ccffcc

                    chkSubYesActYes.Checked = false;
                    chkSubYesActNo.Checked = false;
                    chkSubNo.Checked = false;
                    chkInternal.Checked = false;
                    chkExternal.Checked = false;

                    WSCField.FillCropYear(ddlCropYear, DateTime.Now.Year.ToString());
                    LoadAttachList();

                }
            }
            catch (System.Exception ex) {

                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                if (Common.AppHelper.IsDebugBuild()) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), wex);
                } else {

                    string userMsg = "Unable to Load Page correctly at this time.";

                    System.Text.StringBuilder sbTodo = new System.Text.StringBuilder("");
                    string remains = "";

                    if (lstPendingEmail.Items.Count > 0) {
                        foreach (ListItem itm in lstPendingEmail.Items) {
                            sbTodo.Append(itm.Value);
                            sbTodo.Append(",");
                        }
                        sbTodo.Length = sbTodo.Length - 1;
                        remains = sbTodo.ToString();
                    }

                    if (remains.Length > 0) {
                        // Otherwise, we're blowing up somewhere during a batch process.  One or more emails remain to be sent.
                        userMsg = "An error interrupted your batch email process.  Contact your Network Administrator.\n " +
                            "Copy the detail in this error message for your Network Administrator. \n" +
                            "These SHIDs failed: " + (hideBadEmail.Value.Length == 0 ? "None" : hideBadEmail.Value) + "\n\n" +
                            "These SHIDs were emailed: " + (hideGoodEmail.Value.Length == 0 ? "None" : hideGoodEmail.Value) + "\n\n" +
                            "These SHIDs remain: " + (remains.Length == 0 ? "None" : remains);
                    }

                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), userMsg, wex);
                    Common.AppHelper.LogException(wex, HttpContext.Current);
                }
            }
        }

        private void LoadAttachList() {
            
            const string METHOD_NAME = "LoadAttachList";

            try {

                lstAttach.Items.Add("Select an Attachment");
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(Page.ResolveUrl("~/ZHost/SendAttachments")));
                foreach (FileInfo fi in di.GetFiles()) {
                    string name = fi.FullName;
                    lstAttach.Items.Add(name.Substring(name.LastIndexOf(@"\") + 1));
                }
                lstAttach.SelectedIndex = 0;
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }

        }

        protected void btnSendEmail_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnSendEmail_Click";            

            string shidList = txtSHID.Text;
            string fromEmail = txtFromEmail.Text;
            string subject = txtSubject.Text;
            string message = txtMessage.Text;

            bool SubYesActYes = chkSubYesActYes.Checked;
            bool SubYesActNo = chkSubYesActNo.Checked;
            bool SubNo = chkSubNo.Checked;            

            string pathAttach = "";

            try {

                int cropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));

                for (int i = 0; i < lstAttach.Items.Count; i++) {

                    if (i > 0) {

                        ListItem li = lstAttach.Items[i];

                        if (li.Selected) {
                            if (pathAttach.Length > 0) {
                                pathAttach += ";" + Server.MapPath(Page.ResolveUrl("~/ZHost/SendAttachments")) + @"\" + li.Text;
                            } else {
                                pathAttach += Server.MapPath(Page.ResolveUrl("~/ZHost/SendAttachments")) + @"\" + li.Text;
                            }
                        }
                    }
                }

                if (lstPendingEmail.Items.Count == 0) {

                    // check that all required fields are passed.
                    if (!SubYesActYes && !SubYesActNo && !SubNo) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please check at lease one of the checkboxes: 'Is Shareholder and Is Active' or 'Is Shareholder, but Not Active' or 'Not a Shareholder'.");
                        throw (warn);
                    }
                    if (pathAttach.Length == 0) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please select an attachement from the list.");
                        throw (warn);
                    }
                    if (txtFromEmail.Text.Length == 0) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a From Email Address.");
                        throw (warn);
                    }
                    if (!Common.CodeLib.ValidateEmail(txtFromEmail.Text)) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid From Email Address.");
                        throw (warn);
                    }
                    if (txtSubject.Text.Length == 0) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a Subject line for the email.");
                        throw (warn);
                    }
                    if (txtMessage.Text.Length == 0) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a Message for the body of the email.");
                        throw (warn);
                    }

                    bool isInternalOnly = (chkInternal.Checked);
                    bool isExternalOnly = (chkExternal.Checked);

                    if (isInternalOnly && isExternalOnly) {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("You cannot check both 'Only Internal Email' and 'Only External Emal'.  Select one or select neither of these options.");
                        throw (warn);
                    }

                    //============================================
                    // Get list of email addresses
                    //============================================
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                        using (SqlDataReader dr = WSCMember.GetEmailByShid(conn, shidList, cropYear, SubYesActYes, SubYesActNo, SubNo, isInternalOnly, isExternalOnly)) {

                            while (dr.Read()) {
                                lstPendingEmail.Items.Add(new ListItem(dr.GetString(dr.GetOrdinal("email")), dr.GetInt32(dr.GetOrdinal("memberNumber")).ToString()));
                            }
                        }
                    }

                    hideGoodEmail.Value = "";
                    hideBadEmail.Value = "";
                }

                //========================================================================================
                // Given a list of emails to send, send it; otherwise warn user about zero records.
                //========================================================================================
                if (lstPendingEmail.Items.Count != 0) {
                    DoSendFileWithAttach(fromEmail, subject, message, pathAttach);
                } else {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("No records matched your selected criteria, no emails have been sent.");
                    throw (warn);
                }        

                // Update the batch progress
                if (lstPendingEmail.Items.Count != 0) {

                    // =========================================================
                    //  Send page back to client and we'll do it again.
                    // =========================================================
                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Working (Do not click or press anything)... " + lstPendingEmail.Items.Count.ToString("#,###") + " emails still to send.");

                } else {
                    SendSummaryEmail(subject, fromEmail, pathAttach);
                }

                // Flag for client: Normal process
                if (lstPendingEmail.Items.Count > 0) {
                    txtContinueBatch.Value = "yes";
                } else {
                    txtContinueBatch.Value = "";
                }
            }
            catch (Exception ex) {

                // Flag for client: error, process interrupted.
                txtContinueBatch.Value = "";

                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                if (Common.AppHelper.IsDebugBuild()) {
                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), wex);
                } else {

                    // Clear out any status/progress message in this case.
                    ((HtmlGenericControl)Master.FindControl("divWarning")).InnerHtml = "";

                    string userMsg = "Unable to Send Email at this time.";
                    if (_isErrorDuringSummary) {
                        userMsg = "You emailing batch completed, but there was an error when trying to send you a summary.\n" +
                            "These SHID's failed, probably bad email address on file: " + hideBadEmail.Value + " \n\n" +
                            "These SHID's were emailed: " + hideGoodEmail.Value;
                    } else {

                        System.Text.StringBuilder sbTodo = new System.Text.StringBuilder("");
                        string remains = "";

                        if (lstPendingEmail.Items.Count > 0) {
                            foreach (ListItem itm in lstPendingEmail.Items) {
                                sbTodo.Append(itm.Value);
                                sbTodo.Append(",");
                            }
                            sbTodo.Length = sbTodo.Length - 1;
                            remains = sbTodo.ToString();
                        }

                            // Otherwise, we're blowing up somewhere during a batch process.  One or more emails remain to be sent.
                        userMsg = "An error interrupted your batch email process.  Contact your Network Administrator.\n " +
                            "Copy the detail in this error message for your Network Administrator. \n" +
                            "These SHIDs failed: " + (hideBadEmail.Value.Length == 0? "None":  hideBadEmail.Value) + "\n\n" +
                            "These SHIDs were emailed: " + (hideGoodEmail.Value.Length == 0? "None": hideGoodEmail.Value) + "\n\n" +
                            "These SHIDs remain: " + (remains.Length == 0 ? "None" : remains);
                    }

                    Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), userMsg, wex);
                    Common.AppHelper.LogException(wex, HttpContext.Current);
                }
            }
        }

        private void DoSendFileWithAttach(string fromEmail, string subject, string message, string pathAttach) {

            const string METHOD_NAME = "DoSendFileWithAttach";

            System.Text.StringBuilder sbGoodEmails = new System.Text.StringBuilder("");
            System.Text.StringBuilder sbBadEmails = new System.Text.StringBuilder("");            

            int sendCounter = 0;

            try {

                for (int i = 0; i < lstPendingEmail.Items.Count; i++ ) {

                    // Always pull off the 0th item
                    ListItem itm = lstPendingEmail.Items[0];
                    sendCounter += 1;

                    string toEmail = itm.Text;
                    string memberNumber = itm.Value;

                    if (sendCounter <= BATCH_SIZE) {

                        try {

                            Common.AppHelper.SendEmailWithAttach(
								ConfigurationManager.AppSettings["email.smtpServer"].ToString(),
                                ConfigurationManager.AppSettings["email.smtpServerPort"],
                                ConfigurationManager.AppSettings["email.smtpUser"].ToString(),
								ConfigurationManager.AppSettings["email.smtpPassword"].ToString(),
                                fromEmail, toEmail, "", "", subject, message, pathAttach);

                            if (sbGoodEmails.Length == 0) {
                                sbGoodEmails.Append(memberNumber);
                            } else {
                                sbGoodEmails.Append("," + memberNumber);
                            }
                        }
                        catch {

                            // ex captured for debugging only
                            if (sbBadEmails.Length == 0) {
                                sbBadEmails.Append(memberNumber);
                            } else {
                                sbBadEmails.Append("," + memberNumber);
                            }
                        }

                        // Pop the address off the list -- whether a good or bad send is performed.
                        lstPendingEmail.Items.Remove(itm);

                    } else {
                        break;
                    }
                }

                // Update lists of good/bad emails.
                if (hideGoodEmail.Value.Length == 0) {
                    hideGoodEmail.Value = sbGoodEmails.ToString();
                } else {
                    hideGoodEmail.Value = hideGoodEmail.Value + "," + sbGoodEmails.ToString();
                }
                if (hideBadEmail.Value.Length == 0) {
                    hideBadEmail.Value = sbBadEmails.ToString();
                } else {
                    hideBadEmail.Value = hideBadEmail.Value + "," + sbBadEmails.ToString();
                }
            }
            catch (Exception ex) {

                // Update lists of good/bad emails.
                if (hideGoodEmail.Value.Length == 0) {
                    hideGoodEmail.Value = sbGoodEmails.ToString();
                } else {
                    hideGoodEmail.Value = hideGoodEmail.Value + "," + sbGoodEmails.ToString();
                }
                if (hideBadEmail.Value.Length == 0) {
                    hideBadEmail.Value = sbBadEmails.ToString();
                } else {
                    hideBadEmail.Value = hideBadEmail.Value + "," + sbBadEmails.ToString();
                }   

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        private void SendSummaryEmail(string subject, string fromEmail, string pathAttach) {

            const string METHOD_NAME = "SendSummaryEmail";

            // ================================
            //  WE'RE DONE !!!
            // ================================
            string message = "";
            string goodEmails = hideGoodEmail.Value;
            string badEmails = hideBadEmail.Value;

            try {

                if (goodEmails.Length > 0) {

                    subject = "Send File Summary - " + DateTime.Now.ToShortDateString() +
                        "  (" + subject + ")";

                    message = "****  Send File Summary  **** \n\n";
                    message += "Successfully emailed to these SHIDs: " + goodEmails.ToString();
                    if (badEmails.Length > 0) {
                        message += "\n\nFailed to email to these SHIDs: " + badEmails.ToString();
                    } else {
                        message += "\n\nNo Failures.";
                    }

                    Common.AppHelper.SendEmailWithAttach(
                        ConfigurationManager.AppSettings["email.smtpServer"].ToString(),
                        ConfigurationManager.AppSettings["email.smtpServerPort"].ToString(),
                        ConfigurationManager.AppSettings["email.smtpUser"].ToString(),
                        ConfigurationManager.AppSettings["email.smtpPassword"].ToString(),
                        fromEmail, fromEmail, "",
						ConfigurationManager.AppSettings["email.bccSendFileSummary"].ToString(),
                        subject, message, pathAttach);

                    Common.AppHelper.ShowConfirmation((HtmlGenericControl)Master.FindControl("divWarning"), "Success!  A summary email has been sent to the From Email Address.");

                } else {
                    if (badEmails.Length > 0) {
                        string warnMsg = "Unable to send any emails.  Make sure the From Email Address is valid.";
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning(warnMsg);
                        throw (warn);
                    } else {
                        WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("No emails sent.  Make sure some records match your criteria.");
                        throw (warn);
                    }
                }
            }
            catch (Exception ex) {

                _isErrorDuringSummary = true;

                string errMsg = MOD_NAME + METHOD_NAME;
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        protected void btnTestFromEmail_Click(object sender, EventArgs e) {

            const string METHOD_NAME = "btnTestFromEmail_Click";

            try {

                if (txtFromEmail.Text.Length == 0) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a From Email Address.");
                    throw (warn);
                }
                if (!Common.CodeLib.ValidateEmail(txtFromEmail.Text)) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid From Email Address.");
                    throw (warn);
                }
                if (txtToTest.Text.Length == 0) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a Test To Email Address.");
                    throw (warn);
                }
                if (!Common.CodeLib.ValidateEmail(txtToTest.Text)) {
                    WSCIEMP.Common.CWarning warn = new WSCIEMP.Common.CWarning("Please enter a valid Test To Email Address.");
                    throw (warn);
                }

                string pathAttach = "";
                foreach (ListItem li in lstAttach.Items) {
                    if (li.Selected) {
                        if (pathAttach.Length > 0) {
                            pathAttach += ";" + Server.MapPath(Page.ResolveUrl("~/ZHost/SendAttachments")) + @"\" + li.Text;
                        } else {
                            pathAttach += Server.MapPath(Page.ResolveUrl("~/ZHost/SendAttachments")) + @"\" + li.Text;
                        }
                    }
                }

                Common.AppHelper.SendEmailWithAttach(
                    ConfigurationManager.AppSettings["email.smtpServer"].ToString(),
                    ConfigurationManager.AppSettings["email.smtpServerPort"].ToString(),
                    ConfigurationManager.AppSettings["email.smtpUser"].ToString(),
					ConfigurationManager.AppSettings["email.smtpPassword"].ToString(),
                    txtFromEmail.Text, txtToTest.Text, "",
                    "",
                    "Test - Send File (Check From Email Address)", "This is a successful test.", pathAttach);

                Common.AppHelper.ShowWarning((HtmlGenericControl)Master.FindControl("divWarning"), "Please check that a Test email was received at your Test To Email Address.");
                txtContinueBatch.Value = "";

            }
            catch (Exception ex) {

                txtContinueBatch.Value = "";
				Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
				((PrimaryTemplate)Page.Master).ShowWarning(ex);
            }
        }
    }
}
