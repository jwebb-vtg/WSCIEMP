using System;
using System.IO;

namespace WSCData {

    public class ListAppErrorItem {

        public ListAppErrorItem() { }

        public ListAppErrorItem(string path) {

            string appName;
            string errorDate;
            string fileName = "";

            FileInfo fi = new System.IO.FileInfo(path);
            if (fi.Name.Length > 4) {
                fileName = fi.Name.Substring(0, fi.Name.Length - 4);
            } else {
                fileName = fi.Name;
            }

            appName = "WSCIEMP";
            errorDate = fi.CreationTime.ToString("MM/dd/yyyy HH:mm");

            this.Path = path;
            this.AppName = appName;
            this.ErrorDate = errorDate;
            this.Action = "Pending Import";
            this.Status = "Open";
        }

        public ListAppErrorItem(int appErrorInfoID, string appName, DateTime errorDate, string status, string action,
            string loginServer, string loginClient, string loginUser, string errorCode, string severity) {

            this.Path = appErrorInfoID.ToString();
            this.AppName = appName;
            this.ErrorDate = errorDate.ToString("MM/dd/yyyy HH:mm");
            this.Status = status;
            this.Action = action;
            this.LoginServer = loginServer;
            this.LoginClient = loginClient;
            this.LoginUser = loginUser;
            this.ErrorCode = errorCode;
            this.Severity = severity;
        }

        private string _path = "";
        public string Path {
            get { return _path; }
            set { _path = value; }
        }

        private string _appName = "";
        public string AppName {
            get { return _appName; }
            set {
                _appName = value.Replace("*", "").Replace("-", "").Trim();
            }
        }

        private string _errorDate = "";
        public string ErrorDate {
            get { return _errorDate; }
            set { _errorDate = value; }
        }

        private string _status = "";
        public string Status {
            get { return _status; }
            set { _status = (value == "C" ? "Closed" : "Open"); }
        }

        private string _action = "";
        public string Action {
            get { return _action; }
            set { _action = value; }
        }

        private string _loginServer = "";
        public string LoginServer {
            get { return _loginServer; }
            set { _loginServer = value; }
        }

        private string _loginClient = "";
        public string LoginClient {
            get { return _loginClient; }
            set { _loginClient = value; }
        }

        private string _loginUser = "";
        public string LoginUser {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        private string _errorCode = "";
        public string ErrorCode {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        private string _userName = "";
        public string UserName {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _errorText = "";
        public string ErrorText {
            get { return _errorText; }
            set { _errorText = value; }
        }

        private string _severity = "";
        public string Severity {
            get { return _severity; }
            set { _severity = value; }
        }
    }
}
