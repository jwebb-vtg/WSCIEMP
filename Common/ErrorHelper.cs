using System;
using System.Web;
using System.IO;

namespace WSCIEMP.Common {

    /// <summary>
    /// Summary description for CException.
    /// </summary>
    public class CException : System.ApplicationException {

        public enum KnownError {
            DataWarning = 50000
        };

        public CException() {

        }

        public CException(string message)
            : base(message) {
        }

        public CException(string message, Exception inner)
            : base(message, inner) {
        }

		public static bool IsWarning(Exception ex) {
			return (FindWarning(ex) != null);
		}

        public static string GetErrorMessages(System.Exception err) {

            System.Text.StringBuilder errMsg = new System.Text.StringBuilder();

            // Accumulate all exception messages.
            while (err != null) {

                errMsg.Append(System.Environment.NewLine);
                errMsg.Append("Description: " + err.Message);
                errMsg.Append(System.Environment.NewLine);
                err = err.InnerException;
            }
            if (errMsg.Length > 0)
                return errMsg.ToString();
            else
                return "No error message found.";
        }

		public static string GetWarningMessage(Exception ex) {

			Exception warn = FindWarning(ex);
			if (warn != null) {
				return warn.Message;
			} else {
				return "";
			}
		}

        public static Exception FindWarning(System.Exception ex) {

            Exception candidate = ex;
            while (candidate != null) {
                if (candidate.GetType().ToString().IndexOf("Warning") != -1) {
                    return candidate;
                }
                candidate = candidate.InnerException;
            }

            return candidate;
        }
    }

    public class CWarning : System.ApplicationException {

        public CWarning(string message)
            : base(message) {
        }

        public CWarning(string message, Exception inner)
            : base(message, inner) {
        }
    }

    public class CErrorEventArgs : System.EventArgs {

        private System.Exception _error = null;

        public CErrorEventArgs() {
        }

        public CErrorEventArgs(System.Exception ex) {
            _error = ex;
        }

        public CErrorEventArgs(string msg, System.Exception ex) {
            WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(msg, ex);
            _error = wex;
        }

        public System.Exception Error() {
            return _error;
        }
    }
}
