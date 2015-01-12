using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using WSCData;

namespace WSCIEMP.UControls {

    public partial class UserContractSelector : System.Web.UI.UserControl {

        private const string MOD_NAME = "UControls.UserContractSelector.";
        private const string vsMemberID = "MemberID";

        public event WSCField.ControlHostPageLoadHandler ControlHostPageLoad;
        public event WSCField.ContractNumberChangeHandler ContractNumberChange;
        public event WSCField.ContractNumberFindHandler ContractNumberFind;
        public event WSCField.ContractNumberPrevHandler ContractNumberPrev;
        public event WSCField.ContractNumberNextHandler ContractNumberNext;
        public event WSCField.ShareholderFindHandler ShareholderFind;
        public event WSCField.SequenceNumberChangeHandler SequenceNumberChange;
        public event WSCField.FieldExceptionHandler ExceptionShow;

        private WSCShsData _shs = null;
        private WSCFieldData _fld = null;
        private string _eventWarningMsg = "";
        private bool _isErrorState = false;
   
        protected void Page_Load(object sender, EventArgs e) {

            txtQueryAction.Text += " * UControls.UserContractSelector.Page_Load";

            btnCntFind.Attributes.Add("onclick", "GetTextEntry('" + txtQueryAction.ClientID + "', 'FindContract', '" + txtQuery.ClientID + 
                "', 'Please enter a Contract Number: ', '" + btnSvrFindContract.ClientID + "', event);");
            _shs = Globals.ShsData;
            _fld = Globals.FieldData;

            try {

                if (!Page.IsPostBack) {

                    if (!Globals.IsBeetTransfer) {

                        FillCropYear();
                        SHID = _shs.SHID;
                        ContractNumber = _fld.ContractNumber;
                        SequenceNumber = _fld.SequenceNumber;
                        InitControl(SHID, CropYear, ContractNumber, SequenceNumber);

                    } else {
                        BeetTransferRequest();
                    }

                } else {

                    FillCropYear();

                    int iCurSHID = SHID;
                    string sShid = txtSHID.Text;

                    if (sShid.Length > 0) {
                        if (Common.CodeLib.IsValidSHID(sShid)) {
                            SHID = Convert.ToInt32(sShid);
                        } else {
                            Common.CWarning warn = new Common.CWarning("Please enter a valid SHID.");
                            throw (warn);
                        }
                    } else {
                        SHID = 0;
                    }
                    IsChangedSHID = (iCurSHID != SHID);

                    string sCntNo = Common.UILib.GetDropDownText(ddlContractNumber);
                    if (sCntNo.Length > 0) {
                        ContractNumber = Convert.ToInt32(sCntNo);
                    }

                    string sSeq = Common.UILib.GetDropDownText(ddlSequence);
                    if (sSeq.Length > 0) {
                        SequenceNumber = Convert.ToInt32(sSeq);
                    }

                    if (ddlSequence.Items.Count > 1) {
                        SequenceNumberMax = ddlSequence.Items.Count - 1;
                    } else {
                        SequenceNumberMax = 0;
                    }
                }

                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs();
                OnControlHostPageLoad(cntArg);

            }
            catch (Exception ex) {
                // Raise event to container
                Common.CErrorEventArgs args = new Common.CErrorEventArgs("Unable to load the page correctly.", ex);
                OnExceptionShow(args);
            }
        }

        public void InitControl(int shid, int cropYear, int contractNo, int sequenceNumber) {

            try {

                GetShareholder(shid, cropYear);
                ShowShareholder();
                GetContractList(shid, cropYear);

                if (ddlContractNumber.Items.Count > 1) {

                    // Lacking a contractNo, get selected contract number if it exists; otherwise select the first contract in the list.
                    if (contractNo == 0) {

                        if (ddlContractNumber.SelectedIndex != -1) {

                            string strCntNo = Common.UILib.GetDropDownText(ddlContractNumber);
                            if (strCntNo.Length > 0) {
                                contractNo = Convert.ToInt32(strCntNo);
                            }
                        }

                        if (contractNo == 0) {
                            ddlContractNumber.SelectedIndex = 1;
                            contractNo = Convert.ToInt32(ddlContractNumber.Items[1].Text);
                        }
                    }

                    GetContractBrowse(0, contractNo, cropYear, false);
                    Common.UILib.SelectDropDownValue(ddlContractNumber, contractNo.ToString());
                    SetSequenceNumber(SequenceNumberMax, sequenceNumber);
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + ".InitControl", ex);
                throw (wex);
            }
        }

        private void BeetTransferRequest() {

            try {

                CropYear = _shs.CropYear;
                int shid = _shs.SHID;
                int contractID = _fld.ContractID;
                int sequenceNumber = _fld.SequenceNumber;

                txtQueryAction.Text += " * BeetTransferRequest: cy: " + CropYear.ToString() + "; shid: " + shid.ToString() +
                    "; contractID: " + contractID.ToString() + "; sequenceNumber: " + sequenceNumber.ToString();

                ResetShareholder();
                ResetContract();
                ResetField();

                GetContractBrowse(contractID, 0, CropYear, false);

                GetShareholder(shid, CropYear);
                ShowShareholder();
                GetContractList(shid, CropYear);
                Common.UILib.SelectDropDownValue(ddlContractNumber, ContractNumber.ToString());
                SetSequenceNumber(SequenceNumberMax, sequenceNumber);

            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + ".InitControl", ex);
                throw (wex);
            }
        }

        private void GetShareholder(int shid, int cropYear) {

            try {

                int memberID = 0, addressID = 0;
                string busName = "", phone = "", email = "", fax = "";

                WSCMember.GetInfo(shid.ToString(), ref memberID, ref addressID, ref busName, ref phone, ref email, ref fax);

                SHID = shid;
                IsChangedSHID = false;
                AddressID = addressID;
                MemberID = memberID;
                if (lblBusName.Text.Length == 0) {
                    lblBusName.Text = busName;
                }
                EmailAddress = email;
                FaxNumber = fax;
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + ".GetShareholder", ex);
                throw (wex);
            }
        }

        private void ShowShareholder() {
            Common.UILib.SelectDropDown(ddlCropYear, CropYear.ToString());
            txtSHID.Text = SHID.ToString();
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

        private void GetContractList(int shid, int cropYear) {

            try {

                ddlContractNumber.Items.Clear();
                ddlSequence.Items.Clear();
                WSCSecurity auth = Globals.SecurityState;

                ddlContractNumber.Items.Add("");

                List<ListBeetContractIDItem> stateList = WSCReportsExec.GetContractListSecure(MemberID, cropYear, auth.UserID);
                foreach (ListBeetContractIDItem state in stateList) {
                    ddlContractNumber.Items.Add(state.ContractNumber);
                }

                if (ddlContractNumber.Items.Count == 1) {
                    ContractNumber = 0;
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + ".GetContractList", ex);
                throw (wex);
            }
        }

        private void GetContractBrowse(int contractID, int contractNo, int cropYear, bool allowNewSHID) {

            try {

                ContractNumber = 0;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    WSCSecurity auth = Globals.SecurityState;

                    using (SqlDataReader dr = WSCField.ContractBrowser(conn, contractID, contractNo, cropYear, auth.UserID)) {

                        if (dr.Read()) {

                            if (allowNewSHID) {
                                SHID = dr.GetInt32(dr.GetOrdinal("cnt_address_no"));
                                IsChangedSHID = false;
                                AddressID = dr.GetInt32(dr.GetOrdinal("cnt_address_id"));
                                MemberID = dr.GetInt32(dr.GetOrdinal("cnt_member_id"));
                            }

                            lblBusName.Text = dr.GetString(dr.GetOrdinal("cnt_business_name"));
                            ContractID = dr.GetInt32(dr.GetOrdinal("cnt_contract_id"));
                            ContractNumber = dr.GetInt32(dr.GetOrdinal("cnt_contract_no")); 
                            lblAgriculturist.Text = dr.GetString(dr.GetOrdinal("cnt_agriculturist_name"));
                            lblLandOwner.Text = dr.GetString(dr.GetOrdinal("cnt_landowner_name"));
                            SequenceNumberMax = dr.GetInt32(dr.GetOrdinal("cnt_max_sequence")); ;
                            SequenceNumber = 0;
                            IsOwner = dr.GetBoolean(dr.GetOrdinal("cnt_is_owner"));                            
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + ".GetContractBrowse", ex);
                throw (wex);
            }
        }

        private void GetContractBrowseNext(int contractID, int contractNo, int cropYear, bool allowNewSHID) {

            const string METHOD_NAME = "GetContractBrowseNext";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    WSCSecurity auth = Globals.SecurityState;

                    using (SqlDataReader dr = WSCField.ContractBrowserNext(conn, contractNo, cropYear, auth.UserID)) {

                        if (dr.Read()) {

                            if (allowNewSHID) {
                                SHID = dr.GetInt32(dr.GetOrdinal("cnt_address_no"));
                                IsChangedSHID = false;
                                AddressID = dr.GetInt32(dr.GetOrdinal("cnt_address_id"));
                                MemberID = dr.GetInt32(dr.GetOrdinal("cnt_member_id"));
                            }

                            lblBusName.Text = dr.GetString(dr.GetOrdinal("cnt_business_name"));
                            ContractID = dr.GetInt32(dr.GetOrdinal("cnt_contract_id"));
                            ContractNumber = dr.GetInt32(dr.GetOrdinal("cnt_contract_no"));
                            lblAgriculturist.Text = dr.GetString(dr.GetOrdinal("cnt_agriculturist_name"));
                            lblLandOwner.Text = dr.GetString(dr.GetOrdinal("cnt_landowner_name"));
                            SequenceNumberMax = dr.GetInt32(dr.GetOrdinal("cnt_max_sequence"));
                            SequenceNumber = 0;

                            IsOwner = dr.GetBoolean(dr.GetOrdinal("cnt_is_owner"));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetContractBrowsePrev(int contractID, int contractNo, int cropYear, bool allowNewSHID) {

            const string METHOD_NAME = "GetContractBrowsePrev";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {

                    WSCSecurity auth = Globals.SecurityState;

                    using (SqlDataReader dr = WSCField.ContractBrowserPrev(conn, contractNo, cropYear, auth.UserID)) {

                        if (dr.Read()) {

                            if (allowNewSHID) {
                                SHID = dr.GetInt32(dr.GetOrdinal("cnt_address_no"));
                                IsChangedSHID = false;                                
                                AddressID = dr.GetInt32(dr.GetOrdinal("cnt_address_id"));
                                MemberID = dr.GetInt32(dr.GetOrdinal("cnt_member_id"));
                            }

                            lblBusName.Text = dr.GetString(dr.GetOrdinal("cnt_business_name"));
                            ContractID = dr.GetInt32(dr.GetOrdinal("cnt_contract_id"));
                            ContractNumber = dr.GetInt32(dr.GetOrdinal("cnt_contract_no"));
                            lblAgriculturist.Text = dr.GetString(dr.GetOrdinal("cnt_agriculturist_name"));
                            lblLandOwner.Text = dr.GetString(dr.GetOrdinal("cnt_landowner_name"));
                            SequenceNumberMax = dr.GetInt32(dr.GetOrdinal("cnt_max_sequence"));
                            SequenceNumber = 0;

                            IsOwner = dr.GetBoolean(dr.GetOrdinal("cnt_is_owner"));                            
                        }
                    }
                }
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        public void SetSequenceNumber(int sequenceNumberMax) {

            try {

                FillSequenceNumber(sequenceNumberMax, 1);
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + ".SetSequenceNumber", ex);
                throw (wex);
            }
        }

        public void SetSequenceNumber(int sequenceNumberMax, int sequenceNumberSelected) {

            try {
                FillSequenceNumber(sequenceNumberMax, sequenceNumberSelected);
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + ".SetSequenceNumber", ex);
                throw (wex);
            }
        }

        private void FillSequenceNumber(int sequenceNumberMax, int selectedSequence) {

            try {

                ddlSequence.Items.Clear();

                ddlSequence.Items.Add("");
                for (int i = 1; i <= sequenceNumberMax; i++) {
                    ddlSequence.Items.Add(i.ToString());
                }

                if (ddlSequence.Items.Count > selectedSequence) {
                    ddlSequence.SelectedIndex = selectedSequence;
                    SequenceNumber = selectedSequence;
                } else {
                    SequenceNumber = 0;
                }
                SequenceNumberMax = sequenceNumberMax;

                string display = "DisplayOff";
                if (sequenceNumberMax > 1) {
                    display = "DisplayOn";
                }
                divSequence1.Attributes.Add("class", display);
                divSequence2.Attributes.Add("class", display);
            }
            catch (Exception ex) {
                Common.CException wex = new Common.CException(MOD_NAME + ".FillSequenceNumber", ex);
                throw (wex);
            }
        }

        private void ResetShareholder() {

            SHID = 0;
            MemberID = 0;                        
            EmailAddress = "";
            FaxNumber = "";
            txtSHID.Text = "";
        }

        private void ResetContract() {

            ContractID = 0;
            ContractNumber = 0;
            FieldFactoryNumber = 0;
            lblBusName.Text = "";
            lblLandOwner.Text = "";
            lblAgriculturist.Text = "";
        }

        public void ResetField() {
            CntLLDID = 0;
            FieldID = 0;
            FieldName = "";
            FsaNumber = "";
            LldID = 0;
            SequenceNumber = 0;
            SequenceNumberMax = 0;
            FieldState = "";
        }

        protected virtual void OnControlHostPageLoad(ContractSelectorEventArgs e) {
            if (ControlHostPageLoad != null) {
                ControlHostPageLoad(this, e);
            }
        }

        protected virtual void OnContractNumberChange(ContractSelectorEventArgs e) {
            if (ContractNumberChange != null) {
                ContractNumberChange(this, e);
            }
        }

        protected virtual void OnContractNumberFind(ContractSelectorEventArgs e) {
            if (ContractNumberFind != null) {
                ContractNumberFind(this, e);
            }
        }

        protected virtual void OnContractNumberPrev(ContractSelectorEventArgs e) {
            if (ContractNumberPrev != null) {
                ContractNumberPrev(this, e);
            }
        }

        protected virtual void OnContractNumberNext(ContractSelectorEventArgs e) {
            if (ContractNumberNext != null) {
                ContractNumberNext(this, e);
            }
        }

        protected virtual void OnShareholderFind(ContractSelectorEventArgs e) {
            if (ShareholderFind != null) {
                ShareholderFind(this, e);
            }
        }

        protected virtual void OnSequenceNumberChange(ContractSelectorEventArgs e) {
            if (SequenceNumberChange != null) {
                SequenceNumberChange(this, e);
            }
        }

        protected virtual void OnExceptionShow(Common.CErrorEventArgs e) {
            if (ExceptionShow != null) {
                ExceptionShow(this, e);
            }
        }

        private void FindContract(int contractNo) {

            try {

                ResetShareholder();
                ResetContract();
                ResetField();

                GetContractBrowse(0, contractNo, CropYear, true);

                if (contractNo == ContractNumber) {

                    GetShareholder(SHID, CropYear);
                    ShowShareholder();
                    GetContractList(SHID, CropYear);
                    Common.UILib.SelectDropDownValue(ddlContractNumber, contractNo.ToString());
                    SetSequenceNumber(SequenceNumberMax);

                    // Raise event to container
                    ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs();
                    OnContractNumberFind(cntArg);

                } else {
                    
                    // Clear everything and get out of here.
                    ContractNumber = 0;
                    SHID = 0;
                    MemberID = 0;
                    ContractID = 0;
                    CntLLDID = 0;
                    FieldID = 0;
                    FieldFactoryNumber = 0;
                    IsOwner = false;
                    SequenceNumber = 0;
                    SequenceNumberMax = 0;

                    ddlContractNumber.Items.Clear();
                    ddlSequence.Items.Clear();
                    txtQuery.Text = "";
                    txtQueryAction.Text = "";
                    lblAgriculturist.Text = "";
                    lblBusName.Text = "";
                    lblLandOwner.Text = "";

                    // Raise event to container
                    _isErrorState = true;
                    ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(new Common.CWarning("Could not find Contract Number " + contractNo.ToString()));
                    OnContractNumberFind(cntArg);
                }
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnContractNumberFind(cntArg);
            }
        }

        private void FindAddress(int shid) {

            ResetShareholder();
            ResetContract();
            ResetField();

            GetShareholder(shid, CropYear);
            ShowShareholder();
            GetContractList(shid, CropYear);

            if (ddlContractNumber.Items.Count > 1) {

                ddlContractNumber.SelectedIndex = 1;
                ContractNumber = Convert.ToInt32(Common.UILib.GetDropDownText(ddlContractNumber));
                GetContractBrowse(0, ContractNumber, CropYear, false);
                SetSequenceNumber(SequenceNumberMax);
            }
        }

        private void FindAddress() {

            try {

                int shid = 0;
                if (Common.CodeLib.IsValidSHID(txtSHID.Text)) {
                    shid = Convert.ToInt32(txtSHID.Text);
                } else {
                    Common.CWarning warn = new Common.CWarning("Please enter a number for the SHID.");
                    throw (warn);
                }

                FindAddress(shid);
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnShareholderFind(cntArg);
            }
        }

        private void FindContract() {

            try {
                string query = txtQuery.Text;
                txtQuery.Text = "";

                int contractNo = 0;
                if (query.Length > 0) {
                    if (Common.CodeLib.IsNumeric(query)) {
                        contractNo = Convert.ToInt32(query);
                    } else {
                        Common.CWarning warn = new Common.CWarning("Please enter a number for the Contract Number.");
                        throw (warn);
                    }
                }

                FindContract(contractNo);
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnShareholderFind(cntArg);
            }
        }

        protected void ddlCropYear_SelectedIndexChanged(object sender, EventArgs e) {

            try {

                CropYear = Convert.ToInt32(Common.UILib.GetDropDownText(ddlCropYear));
                FindAddress(SHID);

                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs();
                OnShareholderFind(cntArg);
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnShareholderFind(cntArg);
            }
        }

        protected void btnAdrFind_Click(object sender, EventArgs e) {

            try {

                FindAddress(SHID);

                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs();
                OnShareholderFind(cntArg);
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnShareholderFind(cntArg);
            }
        }

        protected void ddlContractNumber_SelectedIndexChanged(object sender, EventArgs e) {

            try {

                int contractNo = ContractNumber;
                if (IsChangedSHID) {                    
                    GetShareholder(SHID, CropYear);
                }

                if (contractNo > 0) {                    

                    ResetContract();
                    ResetField();

                    GetContractBrowse(0, contractNo, CropYear, false);
                    Common.UILib.SelectDropDownValue(ddlContractNumber, ContractNumber.ToString());
                    SetSequenceNumber(SequenceNumberMax);

                } else {

                    SetSequenceNumber(0, 0);
                    ResetContract();
                    ResetField();
                }

                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs();
                OnContractNumberChange(cntArg);
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnContractNumberChange(cntArg);
            }
        }

        protected void btnPrevContractNo_Click(object sender, EventArgs e) {

            try {

                int iOrigContractNo = ContractNumber;

                ResetShareholder();
                ResetContract();
                ResetField();

                GetContractBrowsePrev(0, iOrigContractNo, CropYear, true);

                if (iOrigContractNo == ContractNumber) {
                    _eventWarningMsg = "You are currently viewing the first Contract in the system for this Agriculturist.  There isn't a Prev Contract.";
                }

                GetShareholder(SHID, CropYear);
                ShowShareholder();
                GetContractList(SHID, CropYear);
                Common.UILib.SelectDropDownValue(ddlContractNumber, ContractNumber.ToString());
                SetSequenceNumber(SequenceNumberMax);

                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs();
                OnContractNumberPrev(cntArg);
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnContractNumberPrev(cntArg);
            }
        }

        protected void btnNextContractNo_Click(object sender, EventArgs e) {

            try {

                int iOrigContractNo = ContractNumber;

                ResetShareholder();
                ResetContract();
                ResetField();

                GetContractBrowseNext(0, iOrigContractNo, CropYear, true);

                if (iOrigContractNo == ContractNumber) {
                    _eventWarningMsg = "You are currently viewing the last Contract in the system for this Agriculturist.  There isn't a Next Contract.";
                }

                GetShareholder(SHID, CropYear);
                ShowShareholder();
                GetContractList(SHID, CropYear);
                Common.UILib.SelectDropDownValue(ddlContractNumber, ContractNumber.ToString());
                SetSequenceNumber(SequenceNumberMax);

                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs();                
                OnContractNumberNext(cntArg);
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnContractNumberNext(cntArg);
            }
        }

        protected void ddlSequence_SelectedIndexChanged(object sender, EventArgs e) {

            try {

                int seqMax = SequenceNumberMax;
                ResetField();
                SetSequenceNumber(seqMax, ddlSequence.SelectedIndex);

                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs();
                OnSequenceNumberChange(cntArg);
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnSequenceNumberChange(cntArg);
            }
        }

        protected void btnSvrFindContract_Click(object sender, EventArgs e) {

            try {
                FindContract();
            }
            catch (Exception ex) {
                // Raise event to container
                ContractSelectorEventArgs cntArg = new ContractSelectorEventArgs(ex);
                OnSequenceNumberChange(cntArg);
            }
        }

        protected override void OnPreRender(EventArgs e) {

            try {

                if (!_isErrorState) {

                    if (MemberID == 0) {
                        Common.CWarning warn = new Common.CWarning("Enter a valid SHID and press the Find button.");
                        throw (warn);
                    } else {

                        string warnMsg = "";
                        if (ContractNumber == 0) {
                            warnMsg = "You need to add a contract before editing Field Contracting information";
                        } else {

                            if (_eventWarningMsg.Length > 0) {
                                warnMsg = _eventWarningMsg;
                            } else {

                                if (SequenceNumberMax == 0) {
                                    warnMsg = "You need to add a field to this contract before editing Field Contracting information";
                                }
                            }
                        }

                        if (warnMsg.Length > 0) {
                            Common.CWarning warn = new WSCIEMP.Common.CWarning(warnMsg);
                            throw (warn);
                        }
                    }
                }
            }
            catch (Exception ex) {
                // Raise event to container
                Common.CErrorEventArgs args = new Common.CErrorEventArgs(ex);
                OnExceptionShow(args);
            }

            base.OnPreRender(e);
        }

        //=====================================
        // PROPERTIES
        //=====================================

        private bool _isChangedSHID = false;
        public bool IsChangedSHID {

            set { _isChangedSHID = value; }
            get { return _isChangedSHID; }
        }

        private string _contractNumber = null;
        public int ContractNumber {

            set { 
                ViewState["ContractNumber"] = value;
                _contractNumber = value.ToString();
                _fld.ContractNumber = value;
            }
            get {
                if (_contractNumber == null) {
                    _contractNumber = (ViewState["ContractNumber"] != null ? Convert.ToInt32(ViewState["ContractNumber"]).ToString() : "0");
                }
                return Convert.ToInt32(_contractNumber);
            }
        }
        private string _shid = null;
        public int SHID {

            set { 
                ViewState["SHID"] = value;
                _shid = value.ToString();
                _shs.SHID = value;
            }
            get {
                if (_shid == null) {
                    _shid = (ViewState["SHID"] != null ? Convert.ToInt32(ViewState["SHID"]).ToString() : "0");
                }
                return Convert.ToInt32(_shid);
            }
        }

        private string _memberID = null;
        public int MemberID {

            set { 
                ViewState["MemberID"] = value;
                _memberID = value.ToString();
            }
            get {
                if (_memberID == null) {
                    _memberID = (ViewState["MemberID"] != null ? Convert.ToInt32(ViewState["MemberID"]).ToString() : "0");
                }
                return Convert.ToInt32(_memberID);
            }
        }
        private string _addressID = null;
        public int AddressID {

            set {
                ViewState["AddressID"] = value;
                _addressID = value.ToString();
            }
            get {
                if (_addressID == null) {
                    _addressID = (ViewState["AddressID"] != null ? Convert.ToInt32(ViewState["AddressID"]).ToString() : "0");
                }
                return Convert.ToInt32(_addressID);
            }
        }
        private string _contractID = null;
        public int ContractID {

            set { 
                ViewState["ContractID"] = value;
                _contractID = value.ToString();
                _fld.ContractID = value;
            }
            get {
                if (_contractID == null) {
                    _contractID = (ViewState["ContractID"] != null ? Convert.ToInt32(ViewState["ContractID"]).ToString() : "0");
                }
                return Convert.ToInt32(_contractID);
            }
        }
        private string _cntLLDID = null;
        public int CntLLDID {

            set { 
                ViewState["CntLLDID"] = value;
                _cntLLDID = value.ToString();
            }
            get {
                if (_cntLLDID == null) {
                    _cntLLDID = (ViewState["CntLLDID"] != null ? Convert.ToInt32(ViewState["CntLLDID"]).ToString() : "0");
                }
                return Convert.ToInt32(_cntLLDID);
            }
        }
        private string _fieldID = null;
        public int FieldID {

            set { 
                ViewState["FieldID"] = value;
                _fieldID = value.ToString();
            }
            get {
                if (_fieldID == null) {
                    _fieldID = (ViewState["FieldID"] != null ? Convert.ToInt32(ViewState["FieldID"]).ToString() : "0");
                }
                return Convert.ToInt32(_fieldID);
            }
        }
        private string _fieldFactoryNumber = null;
        public int FieldFactoryNumber {

            set { 
                ViewState["FieldFactoryNumber"] = value;
                _fieldFactoryNumber = value.ToString();
            }
            get {
                if (_fieldFactoryNumber == null) {
                    _fieldFactoryNumber = (ViewState["FieldFactoryNumber"] != null ? Convert.ToInt32(ViewState["FieldFactoryNumber"]).ToString() : "0");
                }
                return Convert.ToInt32(_fieldFactoryNumber);
            }
        }

        private string _isOwner = null;
        public bool IsOwner {

            set { 
                ViewState["IsOwner"] = value;
                _isOwner = value.ToString();
            }
            get {
                if (_isOwner == null) {
                    _isOwner = (ViewState["IsOwner"] != null ? Convert.ToBoolean(ViewState["IsOwner"]).ToString() : false.ToString());
                }
                return Convert.ToBoolean(_isOwner);
            }
        }
        private string _cropYear = null;
        public int CropYear {

            set { 
                ViewState["CropYear"] = value;
                _shs.CropYear = value;
                _cropYear = value.ToString();
            }
            get {
                if (_cropYear == null) {
                    _cropYear = (ViewState["CropYear"] != null ? Convert.ToInt32(ViewState["CropYear"]).ToString() : "0");
                }
                return Convert.ToInt32(_cropYear);
            }
        }
        private string _sequenceNumber = null;
        public int SequenceNumber {

            set { 
                ViewState["SEQUENCE"] = value;
                _fld.SequenceNumber = value;
                _sequenceNumber = value.ToString();
            }
            get {
                if (_sequenceNumber == null) {
                    _sequenceNumber = (ViewState["SEQUENCE"] != null ? Convert.ToInt32(ViewState["SEQUENCE"]).ToString() : "0");
                }
                return Convert.ToInt32(_sequenceNumber);
            } 
        }
        private string _sequenceNumberMax = null;
        public int SequenceNumberMax {

            set { 
                ViewState["SEQUENCE_MAX"] = value;
                _sequenceNumberMax = value.ToString();
            }
            get {
                if (_sequenceNumberMax == null) {
                    _sequenceNumberMax = (ViewState["SEQUENCE_MAX"] != null ? Convert.ToInt32(ViewState["SEQUENCE_MAX"]).ToString() : "0");
                }
                return Convert.ToInt32(_sequenceNumberMax);
            } 
        }
        private string _lldID = null;
        public int LldID {

            set { 
                ViewState["LldID"] = value;
                _lldID = value.ToString();
            }
            get {
                if (_lldID == null) {
                    _lldID = (ViewState["LldID"] != null ? Convert.ToInt32(ViewState["LldID"]).ToString() : "0");
                }
                return Convert.ToInt32(_lldID);
            }
        }
        private string _fieldName = null;
        public string FieldName {

            set { 
                ViewState["FieldName"] = value;
                _fieldName = value;
            }
            get {
                if (_fieldName == null) {
                    _fieldName = (ViewState["FieldName"] != null ? ViewState["FieldName"].ToString() : "");
                }
                return _fieldName;
            }
        }
        private string _fsaNumber = null;
        public string FsaNumber {

            set { 
                ViewState["FsaNumber"] = value;
                _fsaNumber = value;
            }
            get {
                if (_fsaNumber == null) {
                    _fsaNumber = (ViewState["FsaNumber"] != null ? ViewState["FsaNumber"].ToString() : "");
                }
                return _fsaNumber;
            }
        }
        private string _fieldState = null;
        public string FieldState {

            set { 
                ViewState["FieldState"] = value;
                _fieldState = value;
            }
            get {
                if (_fieldState == null) {
                    _fieldState = (ViewState["FieldState"] != null ? ViewState["FieldState"].ToString() : "");
                }
                return _fieldState;
            }
        }

        private string _emailAddress = null;
        public string EmailAddress {

            set { 
                ViewState["EmailAddress"] = value;
                _emailAddress = value;
            }
            get {
                if (_emailAddress == null) {
                    _emailAddress = (ViewState["EmailAddress"] != null ? ViewState["EmailAddress"].ToString() : "");
                }
                return _emailAddress;
            }
        }

        private string _faxNumber = null;
        public string FaxNumber {

            set { 
                ViewState["FaxNumber"] = value;
                _faxNumber = value;
            }
            get {
                if (_faxNumber == null) {
                    _faxNumber = (ViewState["FaxNumber"] != null ? ViewState["FaxNumber"].ToString() : ""); 
                }
                return _faxNumber; 
            }
        }

    }
}