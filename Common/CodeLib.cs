using System;
using System.Text;
using System.Text.RegularExpressions;

namespace WSCIEMP.Common {

    public class CodeLib {

        // ============================================================================
        // Very generalized types of functions
        // ============================================================================
        public static bool IsNumeric(string s) {
            try {
                Double.Parse(s);
            }
            catch {
                return false;
            }
            return true;
        }

        public static bool IsNumeric(char c) {
            try {
                Double.Parse(c.ToString());
            }
            catch {
                return false;
            }
            return true;
        }

        public static bool IsDate(string s) {
            try {
                DateTime.Parse(s);
            }
            catch {
                return false;
            }
            return true;
        }

        public static string EncodeString(string src) {

            StringBuilder sbDest = new StringBuilder("");

            if (src.Length > 0) {
                for (int i = 0; i < src.Length; i++) {
                    sbDest.Append(Asc(src.Substring(i, 1)).ToString("000"));
                }
            }

            // Return the reversed string.
            return StrReverse(sbDest.ToString());
        }

        public static string DecodeString(string src) {

            StringBuilder sbDest = new StringBuilder("");

            if (src.Length > 0) {

                string s = StrReverse(src);
                for (int i = 0; i < s.Length; i = i + 3) {
                    sbDest.Append(Chr(int.Parse(s.Substring(i, 3))));
                }
            }

            return sbDest.ToString();
        }

        private static string StrReverse(string src) {

            Array arr = src.ToCharArray();
            Array.Reverse(arr);
            char[] c = (char[])arr;
            return (new string(c));
        }

        internal static string Chr(int i) {
            return Convert.ToChar(i).ToString();
        }

        internal static int Asc(string s) {
            return (int)Encoding.ASCII.GetBytes(s)[0];
        }

        public static int Asc(char[] c) {
            return (int)Encoding.ASCII.GetBytes(c)[0];
        }

        // ============================================================================
        // Less generalized types of functions.
        // ============================================================================
        public static string FormatPhoneNumber2Db(string phone) {

            if (phone == null || phone.Length == 0) {
                return phone;
            }
            StringBuilder sb = new StringBuilder();
            char[] s = phone.ToCharArray();
            foreach (char c in s) {
                if (IsNumeric(c)) {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool ValidateFax(string fax) {

            Regex re = new Regex(@"((\(\d{3}\) )|(\d{3}-))\d{3}-\d{4}");
            return re.IsMatch(fax);
        }

        public static string DateEntryFormat() {
            return System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
        }

        public static bool IsValidSHID(string shid) {

            try {

                if (shid.Length >= 4 && shid.Length <= 5) {
                    int testShid = Convert.ToInt32(shid);
                    return true;
                }
            }
            catch {
                //NOP
            }
            return false;
        }

        public static bool ValidateDate(string date) {

            try {

                if (date == null || date.Length == 0) {
                    return true;
                } else {
                    DateTime.ParseExact(date, System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern,
                        System.Globalization.CultureInfo.CurrentCulture);
                    return true;
                }
            }
            catch {
                //NOP
            }
            return false;
        }

        public static bool ValidateEmail(string email) {

            //Regex re = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            Regex re = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            return re.IsMatch(email);
        }

        public static string FormatPhoneNumber2Display(string phone) {

            if (phone == null || phone.Length == 0) {
                return "";
            }
            if (phone.Length == 10) {
                return "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
            } else {
                if (phone.Length == 7) {
                    return phone.Substring(0, 3) + "-" + phone.Substring(3, 4);
                } else {
                    return phone;
                }
            }
        }

        public static bool ValidateMMDDDate(ref string date, int year) {

            try {

                // the date may or may not contain the year.  			
                if (date == null || date.Length == 0) {
                    return true;
                } else {
                    if (date.IndexOf("-") != -1) {
                        date = date.Replace("-", @"/");
                    }
                    if (date.IndexOf(@"\") != -1) {
                        date = date.Replace("-", @"/");
                    }
                    if (date.Split(new char[] { '/' }).Length < 3) {
                        date = date + @"/" + year;
                    }

                    DateTime.ParseExact(date, System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern,
                        System.Globalization.CultureInfo.CurrentCulture);

                    return true;
                }
            }
            catch {
                //NOP
            }
            return false;

        }
    }
}
