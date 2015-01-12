using System;
using System.Configuration;
using System.Web;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading;

namespace WSCIEMP.Common {

    public class AppHelper {

        private const string MOD_NAME = "Common.AppHelper.";
        private static string _appPath = "";
        private static string LF = "<br />";

        public static bool IsDebugBuild() {

            bool rtn = false;

#if (DEBUG)
            rtn = true;
#endif

            return rtn;
        }

        public static string AppPath() {

            if (_appPath.Length == 0) {

                // Try to find Home directory
                _appPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Path.GetDirectoryName(_appPath.ToUpper()) == "BIN") {
                    _appPath = Path.GetDirectoryName(HttpContext.Current.Server.MapPath(".") + @"\..");
                } else {
                    if (Path.GetDirectoryName(_appPath.ToUpper()) == "RELEASE") {
                        _appPath = Path.GetDirectoryName(HttpContext.Current.Server.MapPath(".") + @"\..\..");
                    }
                }
            }
            return _appPath;
        }

        public static string GetIdentityName() {

            string uName = "";
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (identity != null && identity.Name.Length > 0) {
                uName = identity.Name;
                if (uName.Contains(@"\")) {
                    int pos = uName.LastIndexOf(@"\");
                    uName = uName.Substring(pos + 1);
                }
                return uName;
            } else {
                return "";
            }
        }

        public static void ShowWarning(System.Web.UI.WebControls.Label lbl, string warnMsg, System.Exception ex) {

            // Show the original warning exception
            Exception warn = CException.FindWarning(ex);
            if (warn != null) {
                ShowWarning(lbl, warn.Message);
            } else {
                // Otherwise, show default
                ShowWarning(lbl, warnMsg);
            }
        }

        public static void ShowWarning(HtmlGenericControl div, string warnMsg, System.Exception ex) {
            
            // Show the original warning exception
            Exception warn = CException.FindWarning(ex);
            if (warn != null) {
                // Given a warning, only show the first true warning.
                if (div.InnerHtml.Length == 0) {
                    ShowWarning(div, warn.Message);
                }
            } else {
                // Otherwise, show default
                ShowWarning(div, warnMsg);
            }
        }

        public static void ShowWarning(System.Web.UI.WebControls.Label lbl, string warnMsg) {

            lbl.CssClass = "WarningOn";
            if (lbl.Text.Length > 0) {
                lbl.Text += "  " + warnMsg;
            } else {
                lbl.Text += warnMsg;
            }
        }

        public static void ShowWarning(HtmlGenericControl div, string warnMsg) {

            div.Attributes.Add("class", "WarningOn");
            if (div.InnerHtml.Length > 0) {
                div.InnerHtml += "  " + warnMsg;
            } else {
                div.InnerHtml += warnMsg;
            }
        }

        public static void ShowWarning(HtmlGenericControl div, System.Exception ex) {

            // Show the original warning exception
            Exception warn = CException.FindWarning(ex);
            if (warn != null) {
                // Given a warning, only show the first true warning.
                if (div.InnerHtml.Length == 0) {
                    ShowWarning(div, warn.Message);
                }
                return;
            } else {

                string msg = "";
                while (ex != null) {

                    msg += "Source: " + ex.Source + LF + "Description: " + ex.Message + LF + LF;
                    ex = ex.InnerException;
                }

                div.Attributes.Add("class", "WarningOn");
                if (div.InnerHtml.Length > 0) {
                    div.InnerHtml += "  " + msg;
                } else {
                    div.InnerHtml += msg;
                }
            }
        }

        public static void ShowWarning(System.Web.UI.WebControls.Label lbl, System.Exception ex) {

            // Show the original warning exception
            Exception warn = CException.FindWarning(ex);
            if (warn != null) {
                //div.InnerHtml = "";
                ShowWarning(lbl, warn.Message);
                return;
            } else {

                string msg = "";
                while (ex != null) {

                    msg += "Source: " + ex.Source + LF + "Description: " + ex.Message + LF + LF;
                    ex = ex.InnerException;
                }

                lbl.Attributes.Add("class", "WarningOn");
                if (lbl.Text.Length > 0) {
                    lbl.Text += "  " + msg;
                } else {
                    lbl.Text += msg;
                }
            }
        }

        public static void HideWarning(HtmlGenericControl div) {
            div.Attributes.Add("class", "WarningOff");
            div.InnerText = "";
        }

        public static void HideWarning(System.Web.UI.WebControls.Label lbl) {
            lbl.Text = "";
            lbl.CssClass = "WarningOff";
        }

        public static void ShowConfirmation(HtmlGenericControl div, string confirm) {

            div.Attributes.Add("class", "ButtonText");
            div.InnerHtml = confirm;
        }

        public static void LogException(string errMessage, HttpContext context) {

            DateTime dt = DateTime.Now;
            errMessage = "***********************************************************" + Environment.NewLine +
                "LOG TIME: " + dt.ToShortDateString() + " " + dt.ToLongTimeString() + Environment.NewLine +
                "***********************************************************" + Environment.NewLine +
                errMessage;

            string msgSessionInfo = "";
            msgSessionInfo = GetContextInformation(context);

            if (msgSessionInfo.Length > 0) {
                errMessage += Environment.NewLine + msgSessionInfo;
            }
            try {

                System.Collections.Specialized.NameValueCollection nv = context.Request.ServerVariables;
                string userAgent = "";
                if (nv != null) {
                    if (nv["HTTP_USER_AGENT"] != null) {
                        userAgent = nv["HTTP_USER_AGENT"];
                    }
                }

                if (errMessage != null && errMessage.IndexOf("get_aspx_ver") == -1 &&
                    userAgent.ToUpper().IndexOf("MSNBOT") == -1) {

                    try {
                        // Email a notification of the error
                        SendEmail(ConfigurationManager.AppSettings["email.smtpServer"].ToString(), 
							ConfigurationManager.AppSettings["email.smtpServerPort"].ToString(),
                            errMessage, 
							ConfigurationManager.AppSettings["email.smtpUser"].ToString(), 
							ConfigurationManager.AppSettings["email.smtpPassword"].ToString(),
							ConfigurationManager.AppSettings["email.errors.to"].ToString(),
							ConfigurationManager.AppSettings["email.errors.from"].ToString(),
							ConfigurationManager.AppSettings["email.errors.cc"].ToString(),
							ConfigurationManager.AppSettings["email.errors.subject"].ToString(), false);
                    }
                    catch {
                        // NOP
                    }

                    // Try to hit the database and if that is unsuccessful, then turn around and try to hit the file system.
                    bool isDbWritten = false;
                    try {
                        WSCData.AppError.AppErrorInfoCreate(0, "WSCIEMP", DateTime.Now, "", "", Common.AppHelper.GetIdentityName(), "",
                            Common.AppHelper.GetIdentityName(), errMessage);

                        isDbWritten = true;
                    }
                    catch {
                        // NOP
                    }

                    if (!isDbWritten) {
                        try {
                            using (StreamWriter sw = System.IO.File.AppendText(AppPath() + "Errors" + dt.ToString("yyyyMMdd") + ".txt")) {
                                sw.Write(errMessage);
                            }
                        }
                        catch {
                            // NOP
                        }
                    }

                    try {

                        string eventLogName = "Application";
                        if (EventLog.SourceExists(eventLogName)) {

                            EventLog log = new EventLog(eventLogName);
                            log.WriteEntry(errMessage, EventLogEntryType.Error);
                        }
                    }
                    catch {
                        // NOP
                    }
                }
            }
            catch (System.Exception except) {
                string newExcep = except.Message;
                // NOP
            }
        }

        public static void LogException(System.Exception e, HttpContext context) {

            // ignore logging warnings
            if (CException.FindWarning(e) != null) {
                return;
            }

            string errMessage = "Error Msg: " + CException.GetErrorMessages(e) +
                System.Environment.NewLine +
                "Stack Trace: " + e.StackTrace;

            LogException(errMessage, context);
        }

        public static void LogException(string errMessage, System.Exception e, HttpContext context) {

            // ignore logging warnings
            if (CException.FindWarning(e) != null) {
                return;
            }

            string errMsg = null;
            if (errMessage != null) {
                errMsg = "Error Msg: " + errMessage + System.Environment.NewLine +
                    CException.GetErrorMessages(e) +
                    System.Environment.NewLine +
                    "Stack Trace: " + e.StackTrace;
            } else {
                errMsg = "Error Msg: " + " Null " + System.Environment.NewLine +
                    CException.GetErrorMessages(e) +
                    System.Environment.NewLine +
                    "Stack Trace: " + e.StackTrace;
            }

            LogException(errMsg, context);
        }

        public static void SendEmail(string smtpServer, string smtpPort,
    string emailMessage, string userName, string password,
    string toAddress, string fromAddress, string ccAddress, string subject, bool isBodyHtml) {

            try {

                // Matched shid and email to a user.  Send them their password
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(fromAddress);
                foreach (string recip in toAddress.Split(new char[] { ';' })) {
                    mail.To.Add(new MailAddress(recip));
                }
                if (!String.IsNullOrEmpty(ccAddress)) {
                    foreach (string recip in ccAddress.Split(new char[] { ';' })) {
                        mail.CC.Add(new MailAddress(recip));
                    }
                }

                mail.Subject = subject;
                mail.Body = emailMessage;
                mail.IsBodyHtml = isBodyHtml;

                SmtpClient smtp = new SmtpClient(smtpServer, int.Parse(smtpPort));
                if (!String.IsNullOrEmpty(userName)) {
                    if (userName == "INTEGRATED" && String.IsNullOrEmpty(password)) {
                        // INTEGRATED Authentication
                        smtp.UseDefaultCredentials = true;
                    } else {
                        // Basic Authentication
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(userName, password);
                    }
                }

                smtp.Send(mail);

            }
            catch (Exception MailEx) {
                string errMsg = "Error in SendEmail" + LF +
                    "To Address: " + toAddress + LF +
                    "Subject: " + subject;
                CException wscEx = new CException(errMsg, MailEx);
                throw (wscEx);
            }
        }

        public static void SendEmailWithAttach(string smtpServer, string smtpPort,
            string userName, string password, string fromEmail, string toEmail, string cc, string bcc, string subject,
            string message, string pathAttach) {

            try {

                //const string cdoSchema = "http://schemas.microsoft.com/cdo/configuration/";

                // Matched shid and email to a user.  Send them their password
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(fromEmail.Trim());
                foreach (string recip in toEmail.Split(new char[] { ';' })) {
                    mail.To.Add(new MailAddress(recip.Trim()));
                }

                if (!String.IsNullOrEmpty(cc)) {
                    foreach (string recip in cc.Split(new char[] { ';' })) {
                        mail.CC.Add(new MailAddress(recip.Trim()));
                    }
                }

                if (!String.IsNullOrEmpty(bcc)) {
                    foreach (string recip in bcc.Split(new char[] { ';' })) {
                        mail.Bcc.Add(new MailAddress(recip.Trim()));
                    }
                }

                mail.Subject = subject;
                mail.Body = message;

                if (pathAttach.Length > 0) {
                    foreach (string path in pathAttach.Split(new char[] { ';' })) {
                        mail.Attachments.Add(new Attachment(path));
                    }
                }

                SmtpClient smtp = new SmtpClient(smtpServer, int.Parse(smtpPort));
                if (!String.IsNullOrEmpty(userName)) {
                    if (userName == "INTEGRATED" && String.IsNullOrEmpty(password)) {
                        // INTEGRATED Authentication
                        smtp.UseDefaultCredentials = true;
                    } else {
                        // Basic Authentication
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(userName, password);
                    }
                }

                smtp.Send(mail);

            }
            catch (System.Net.Mail.SmtpException smtpEx) {
                string errMsg = "Error in SendEmail\n" +
                    "Status Code: " + smtpEx.StatusCode.ToString("G") + "\n" +
                    "To Address: " + toEmail + "\n" +
                    "Subject: " + subject;
                CException wscEx = new CException(errMsg, smtpEx);
				LogException(wscEx, HttpContext.Current);
                throw (wscEx);
            }
            catch (Exception MailEx) {
                string errMsg = "Error in SendEmail\n" +
                    "To Address: " + toEmail + "\n" +
                    "Subject: " + subject;
                CException wscEx = new CException(errMsg, MailEx);
				LogException(wscEx, HttpContext.Current);
                throw (wscEx);
            }
        }

		public static void SendFax(string faxServerName, string toFaxName, string toFaxNumber,
			string toVoiceNumber, string toFaxBusName, string fromFaxName, string fromFaxVoiceNumber,
			string faxAttachPath, string subject) {

				int lineNum = 0;

				try {

					//lineNum = 1;
					//RFCOMAPILib.FaxServerClass faxserver = new RFCOMAPILib.FaxServerClass();
					//lineNum = 2;
					//faxserver.ServerName = faxServerName;
					//lineNum = 3;
					//faxserver.UseNTAuthentication = RFCOMAPILib.BoolType.True;
					//lineNum = 4;
					//faxserver.Protocol = RFCOMAPILib.CommunicationProtocolType.cpSecTCPIP;
					//lineNum = 5;
					//faxserver.OpenServer();
					//lineNum = 6;


//If (m_rfxServer Is Nothing) Then
        
//    ' Create the Server object -- early bind.
//    Set m_rfxServer = New RFCOMAPILib.FaxServer
    
//    ' Set Server Name, Authentication, and Protocol.  This is the
//    ' minimum property set to send a fax.  When not using NTAuth, you
//    ' must supply user name and password.
//    m_rfxServer.ServerName = g_strFAXServer
//    m_rfxServer.UseNTAuthentication = True
//    m_rfxServer.Protocol = cpSecTCPIP
    
//End If
    
//' Create the Fax object and set minimal properties
//Set objFax = m_rfxServer.CreateObject(coFax)
//With objFax
    
//    .Owner = m_rfxServer.User ' Equate Owner to current user.
//    .FromName = strFromName
//    .ToName = strToName
//    .ToFaxNumber = strFaxNumber
        
//    ' Add the attachment via the Attachments.Add method.
//    ' DO NOT CREATE ATTACHMENT OBJECT.  Bug in software
//    ' does not allow you to add the object to the collection.
//    ' this is the workaround.
//    .Attachments.Add strFileName, False
    
//    ' Send the fax
//    .Send
//    DoEvents
        
//End With

//Set objFax = Nothing





					////RFCOMAPILib.Fax fax = (RFCOMAPILib.Fax)faxserver.CreateObject(RFCOMAPILib.CreateObjectType.coFax);
					//RFCOMAPILib.Fax fax = (RFCOMAPILib.Fax)faxserver.CreateObject[RFCOMAPILib.CreateObjectType.coFax];
					//lineNum = 7;

					//fax.Owner = faxserver.User;

					//if (!String.IsNullOrEmpty(toFaxName)) {
					//    fax.ToName = toFaxName;
					//    lineNum = 8;
					//}
					//if (!String.IsNullOrEmpty(toFaxNumber)) {
					//    fax.ToFaxNumber = toFaxNumber;
					//    lineNum = 9;
					//}
					//if (!String.IsNullOrEmpty(toVoiceNumber)) {
					//    fax.ToVoiceNumber = toVoiceNumber;
					//    lineNum = 10;
					//}
					//if (!String.IsNullOrEmpty(toFaxBusName)) {
					//    fax.ToCompany = toFaxBusName;
					//    lineNum = 11;
					//}
					//if (!String.IsNullOrEmpty(fromFaxVoiceNumber)) {
					//    fax.FromName = fromFaxName;
					//    lineNum = 12;
					//}
					//if (!String.IsNullOrEmpty(fromFaxVoiceNumber)) {
					//    fax.FromVoiceNumber = fromFaxVoiceNumber;
					//    lineNum = 13;
					//}
					//fax.EmailSubject = subject;
					//lineNum = 14;

					//faxserver.AuthorizationUser.SendNotifyOnIncompleteFirstTime = RFCOMAPILib.BoolType.False;
					//lineNum = 15;
					//faxserver.AuthorizationUser.SendNotifyOnIncompletePeriodically = RFCOMAPILib.BoolType.False;
					//lineNum = 16;
					//faxserver.AuthorizationUser.SendNotifyOnNoHoldForPreview = RFCOMAPILib.BoolType.False;
					//lineNum = 17;
					//faxserver.AuthorizationUser.SendNotifyOnSendFailedWillRetry = RFCOMAPILib.BoolType.False;
					//lineNum = 18;
					////faxserver.AuthorizationUser.SendNotifyOnSendFailure = RFCOMAPILib.BoolType.False;
					//faxserver.AuthorizationUser.SendNotifyOnSendHeldForApproval = RFCOMAPILib.BoolType.False;
					//lineNum = 19;
					//faxserver.AuthorizationUser.SendNotifyOnSendingFirstTime = RFCOMAPILib.BoolType.False;
					//lineNum = 20;
					//faxserver.AuthorizationUser.SendNotifyOnSendingPeriodically = RFCOMAPILib.BoolType.False;
					//lineNum = 21;
					////faxserver.AuthorizationUser.SendNotifyOnSentSuccessfully = RFCOMAPILib.BoolType.False;

					//fax.Attachments.Add(faxAttachPath, RFCOMAPILib.BoolType.False);
					//lineNum = 22;
					//Thread.Sleep(2000);
					//lineNum = 23;

					//fax.Send();
					
					//lineNum = 24;
					//Thread.Sleep(2000);
					//lineNum = 25;

					//faxserver.CloseServer();
					//lineNum = 26;
		
//                RFXServ.OpenServer()
//                RFXDoc = RFXServ.CreateObject(RFCOMAPILib.CreateObjectType.coFax)
//                With RFXDoc
//                    'Populate the basic data... we already know that we have a non-null fax number,
//                    'so no need to check again...
//                    .ToFaxNumber = dr.FaxNumber
//                    With RFXServ.AuthorizationUser
						//'Effectively, we do not want any feedback coming back from the fax server telling
						//'us anything about the sending status...
//                        .SendNotifyOnIncompleteFirstTime = False
						//.SendNotifyOnIncompletePeriodically = False
						//.SendNotifyOnNoHoldForPreview = False
						//.SendNotifyOnSendFailedWillRetry = False
						//.SendNotifyOnSendFailure = False
						//.SendNotifyOnSendHeldForApproval = False
						//.SendNotifyOnSendingFirstTime = False
						//.SendNotifyOnSendingPeriodically = False
					//    .SendNotifyOnSentSuccessfully = False
					//End With

//                    For Each tblDocumentsToIncludeRow In mr.GetChildRows("MessageDocumentsToInclude")
//                        'This line attaches the document to the fax... we do not delete on sending
//                        'because we may send to multiple recipients.
//                        .Attachments.Add(tblDocumentsToIncludeRow.DocumentName, RFCOMAPILib.BoolType.False)
//                        Thread.Sleep(cGeneralParametersRow.SleepMilisecondsAfterFaxing)
//                    Next
				//    .Send()
				//    'We need to give the fax server time to pickup and convert the documents before we delete them...
				//    Thread.Sleep(cGeneralParametersRow.SleepMilisecondsAfterFaxing)
				//    RFXServ.CloseServer()
				//End With
				//Return True



					//RFCOMAPILib.Fax fax = (RFCOMAPILib.Fax)faxserver.get_CreateObject(RFCOMAPILib.CreateObjectType.coFax);

					// set up your 'fax' object the way you want it, below is just some sample options
					//fax.ToName = toFaxName;
					//fax.ToFaxNumber = toFaxNumber;
					//if (!String.IsNullOrEmpty(toVoiceNumber)) {
					//    fax.ToVoiceNumber = toVoiceNumber;
					//}
					//fax.ToCompany = toFaxBusName;
					//fax.FromName = fromFaxName;
					//if (!String.IsNullOrEmpty(fromFaxVoiceNumber)) {
					//    fax.FromVoiceNumber = fromFaxVoiceNumber;
					//}

					//fax.Attachments.Add(faxAttachPath, RFCOMAPILib.BoolType.False);
					//fax.EmailSubject = subject;

					//fax.Send();
				}
				catch (System.Runtime.InteropServices.COMException cex) {

					string comErrorInfo = "";
					foreach (string key in cex.Data.Keys) {						
						comErrorInfo += "Key: " + key + "; definition: " + cex.Data[key].ToString() + "\n";
					}

					string errMsg = "Error in SendFax\n" +
						"faxServerName: " + faxServerName + "\n" +
						"toFaNamex: " + toFaxName + "\n" +
						"To toFaxNumber: " + toFaxNumber + "\n" +
						"TfaxAttachPath: " + faxAttachPath + "\n" +
						"Subject: " + subject + "\n" +
						"Line Num: " + lineNum.ToString() + "\n" +
						"COM Error Info: " + (comErrorInfo.Length > 0? comErrorInfo: "interop: " + cex.Message);

					CException wscEx = new CException(errMsg, cex);
					LogException(wscEx, HttpContext.Current);
					throw (wscEx);
				}
				catch (Exception MailEx) {
					string errMsg = "Error in SendFax\n" +
						"faxServerName: " + faxServerName + "\n" +
						"toFaNamex: " + toFaxName + "\n" +
						"To toFaxNumber: " + toFaxNumber + "\n" +
						"TfaxAttachPath: " + faxAttachPath + "\n" +
						"Line Num: " + lineNum.ToString() + "\n" +
						"Subject: " + subject;
					CException wscEx = new CException(errMsg, MailEx);
					LogException(wscEx, HttpContext.Current);
					throw (wscEx);
				}
		}

        public static string GetContextInformation(HttpContext context) {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try {

                sb.Append(Environment.NewLine + "*** Application Info: ***" + Environment.NewLine);
                try {
                    foreach (string key in context.Application.Keys) {
                        try {
                            if (context.Application[key] == null) {
                                sb.Append(key + ": " + "null" + Environment.NewLine);
                            } else {
                                sb.Append(key + ": " + context.Application[key].ToString() + Environment.NewLine);
                            }
                        }
                        catch { //NOP
                        }
                    }
                }
                catch (Exception apEx) {
                    sb.Append("Error in gathering Application information.  " + apEx.Message + Environment.NewLine);
                }

                sb.Append(Environment.NewLine + "*** Session Info: ***" + Environment.NewLine);

                try {
                    try {
                        sb.Append("Security Principal: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name + Environment.NewLine);
                    }
                    catch { //NOP 
                        sb.Append("Security Principal: *UNKNOWN*" + Environment.NewLine);
                    }
                    foreach (string key in context.Session.Keys) {
                        try {
                            if (context.Session[key] == null) {
                                sb.Append(key + ": " + "null" + Environment.NewLine);
                            } else {
                                sb.Append(key + ": " + context.Session[key].ToString() + Environment.NewLine);
                            }
                        }
                        catch { //NOP 
                        }
                    }
                }
                catch (Exception sesEx) {
                    sb.Append("Error in gathering Session information.  " + sesEx.Message + Environment.NewLine);
                }

                try {
                    sb.Append(Environment.NewLine + "*** Request Info: ***" + Environment.NewLine);
                    sb.Append("Application Path: " + context.Request.ApplicationPath + Environment.NewLine);
                    sb.Append("Browser is AOL: " + context.Request.Browser.AOL.ToString() + Environment.NewLine);
                    sb.Append("Browser String: " + context.Request.Browser.Browser + Environment.NewLine);
                    sb.Append("Browser is Crawler: " + context.Request.Browser.Crawler + Environment.NewLine);
                    sb.Append("Browser script version: " + context.Request.Browser.EcmaScriptVersion.ToString() + Environment.NewLine);
                    sb.Append("Browser supports JavaScript: " + context.Request.Browser.EcmaScriptVersion.ToString() + Environment.NewLine);
                    sb.Append("Browser Version: " + context.Request.Browser.MajorVersion.ToString() + "." + context.Request.Browser.MinorVersion.ToString() + Environment.NewLine);
                    sb.Append("Browser DOM version: " + context.Request.Browser.MSDomVersion.ToString() + Environment.NewLine);
                    sb.Append("Client Platform: " + context.Request.Browser.Platform + Environment.NewLine);

                    sb.Append("Request Raw URL: " + context.Request.RawUrl + Environment.NewLine);
                    sb.Append("Request File Path: " + context.Request.FilePath + Environment.NewLine);
                    sb.Append("Request Virtual Path: " + context.Request.Path + Environment.NewLine);
                }
                catch { //NOP
                }

                string referrer = "";

                try {
                    if (context.Request.UrlReferrer != null &&
                        context.Request.UrlReferrer.AbsoluteUri != null) {
                        referrer = context.Request.UrlReferrer.AbsoluteUri.ToString();
                    }
                    if (referrer.Length > 0) {
                        sb.Append("Referrer: " + referrer + Environment.NewLine);
                    } else {
                        sb.Append("Referrer: Unknown" + Environment.NewLine);
                    }
                }
                catch { //NOP
                }

                try {
                    sb.Append("Server Name: " + context.Server.MachineName + Environment.NewLine);
                    if (context.Session != null)
                        sb.Append("Is New Session: " + context.Session.IsNewSession.ToString() + Environment.NewLine);
                    else {
                        sb.Append("Is New Session: No Session" + Environment.NewLine);
                    }
                }
                catch { //NOP
                }

                sb.Append(Environment.NewLine + "*** Form Values: ***" + Environment.NewLine);

                try {
                    System.Collections.Specialized.NameValueCollection nv = context.Request.Form;
                    if (nv != null) {
                        try {
                            foreach (string key in nv.Keys) {
                                try {
                                    if (key.IndexOf("VIEWSTATE") == -1) {
                                        if (nv[key] == null) {
                                            sb.Append(key + ": " + "null" + Environment.NewLine);
                                        } else {
                                            sb.Append(key + ": " + nv[key] + Environment.NewLine);
                                        }
                                    }
                                }
                                catch {
                                    // NOP
                                }
                            }
                        }
                        catch (Exception rqEx) {
                            sb.Append("Error in gathering Form information.  " + rqEx.Message + Environment.NewLine);
                        }
                    }
                }
                catch { //NOP
                }

                sb.Append(Environment.NewLine + "*** QueryString Values: ***" + Environment.NewLine);

                try {
                    System.Collections.Specialized.NameValueCollection nv = context.Request.QueryString;
                    if (nv != null) {
                        foreach (string key in nv.Keys) {
                            try {
                                if (nv[key] == null) {
                                    sb.Append(key + ": " + "null" + Environment.NewLine);
                                } else {
                                    sb.Append(key + ": " + nv[key] + Environment.NewLine);
                                }
                            }
                            catch {
                                // NOP
                            }
                        }
                    }
                }
                catch { //NOP
                }
            }
            catch {
                // NOP
            }

            return sb.ToString();
        }
    }
}
