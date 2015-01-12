using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.Fields {

    public partial class BeetAcctConnect : Common.BasePage {

        private const string MOD_NAME = "Fields.BeetAcctConnect.";
        private WSCShsData _shs = null;
        private WSCFieldData _fld = null;

        protected void Page_Load(object sender, EventArgs e) {

            try {

                _shs = Globals.ShsData;
                _fld = Globals.FieldData;

                // C:\Program Files\Internet Explorer\IEXPLORE.EXE http://localhost/wsciemp/fields/
                // BeetAcctConnect.aspx?SHID=20060&CY=2006&CNT=38941&SEQ=1
                System.Collections.Specialized.NameValueCollection queryStr = Request.QueryString;
                string shid = queryStr["SHID"].ToString();
                string cropYear = queryStr["CY"].ToString();
                string contractID = queryStr["CNT"].ToString();
                string sequenceNo = queryStr["SEQ"].ToString();
                if (String.IsNullOrEmpty(sequenceNo)) {
                    sequenceNo = "0";
                }

                Globals.IsBeetTransfer = true;
                _shs.SHID = Convert.ToInt32(shid);
                _shs.CropYear = Convert.ToInt32(cropYear);
                _fld.ContractID = Convert.ToInt32(contractID);
                _fld.SequenceNumber = Convert.ToInt32(sequenceNo);

            }
            catch (System.Exception ex) {
                Common.AppHelper.LogException(ex, HttpContext.Current);
            }

            Server.Transfer(Page.ResolveUrl(@"~/Fields/FieldInfo.aspx"));
        }
    }
}
