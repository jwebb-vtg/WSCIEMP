using System;
using System.Collections;
using System.Web;
using System.Web.UI;

namespace WSCData {

    /// <summary>
    /// Summary description for Globals.
    /// </summary>
    public static class Globals {

        private const string WSC_AG_READ_ONLY = @"WESTERNSUGAR\WSC AG Read-Only";
        private const string DEV_AG_READ_ONLY = @"WSC AG Read-Only";

        public static WSCFieldData FieldData {
            get {

                object o = HttpContext.Current.Session["FieldData"];
                if (o == null) {
                    o = new WSCFieldData();
                    HttpContext.Current.Session["FieldData"] = o;
                }
                return (WSCFieldData)o;
            }
            set {
                HttpContext.Current.Session["FieldData"] = value;
            }
        }

        public static bool IsUserPermissionReadOnly(System.Web.Security.RolePrincipal theUser) {

            if (theUser.IsInRole(WSC_AG_READ_ONLY) || theUser.IsInRole(DEV_AG_READ_ONLY)) {
                return true;
            } else {
                return false;
            }
        }

		public static string BeetExportConnectionString() {

			System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
			object o = cache["BeetExportConnectionString"];
			if (o == null) {

				System.Web.HttpServerUtility server = System.Web.HttpContext.Current.Server;
				string s = "Provider=Microsoft.Jet.OLEDB.4.0;User Id=admin;Password=;" + "Data Source=" + server.MapPath(@"~/ZHost/Export/BeetFields.mdb");
				cache["BeetExportConnectionString"] = s;
				return s;
			} else {
				return o.ToString();
			}
		}

        public static WSCShsData ShsData {
            get {

                WSCShsData shs = (WSCShsData)HttpContext.Current.Session["ShsData"];
                if (shs == null) {
                    shs = new WSCShsData();

                    ArrayList cy = WSCField.GetCropYears();
                    if (cy.Count > 0) {
                        int cropYear = Convert.ToInt32(cy[0]);
                        shs.CropYear = cropYear;
                    }

                    HttpContext.Current.Session["ShsData"] = shs;
                }
                return shs;
            }
            set {
                HttpContext.Current.Session["ShsData"] = value;
            }
        }

        public static string WarningMessage {
            get {

                string warn = "";
                object o = HttpContext.Current.Session["WarningMessage"];
                if (o != null) {
                    warn = o.ToString();
                }
                return warn;
            }
            set {
                HttpContext.Current.Session["WarningMessage"] = value;
            }
        }

        internal static string ControlFolderPath(Page p) {
            return p.ResolveUrl("~/UControls");
        }

        public static bool IsBeetTransfer {
            get {

                bool isXFer = false;
                object o = HttpContext.Current.Session["IsBeetTransfer"];
                if (o != null) {
                    isXFer = Convert.ToBoolean(o);
                }
                return isXFer;
            }
            set {
                HttpContext.Current.Session["IsBeetTransfer"] = value;
            }
        }

        public static WSCSecurity SecurityState {
            get {

                object o = HttpContext.Current.Session["SecurityState"];
                if (o == null) {
                    o = new WSCSecurity();
                    HttpContext.Current.Session["SecurityState"] = o;
                }
                return (WSCSecurity)o;
            }
            set {
                HttpContext.Current.Session["SecurityState"] = value;
            }
        }

        public static bool IsTempCleanUpDone {
            get {

                object o = HttpContext.Current.Session["istempcleanupdone"];                
                if (o == null) {
                    return false;
                } else {
                    return Convert.ToBoolean(o.ToString());
                }
            }
            set {
                HttpContext.Current.Session["istempcleanupdone"] = value;
            }
        }

        public static string BeetCropYears {
            get {

                object o = HttpContext.Current.Session["BeetCropYears"];
                if (o == null) {
                    return "";
                } else {
                    return o.ToString();
                }
            }
            set {
                HttpContext.Current.Session["BeetCropYears"] = value;
            }
        }
    }
}
