using System;

namespace WSCData {

    public class ListShareTransferItem {

        public ListShareTransferItem() { }

        public ListShareTransferItem(int shareTransferID, string contractNumber, int toMemberID, string toShid, string toFactoryName, decimal toRetainPct,
            decimal toCropPct, decimal pricePerAcre, int shares, int fromMemberID, string fromShid, string fromFactoryName, decimal fromRetainPct, bool hasLienOnShares,
            bool hasConsentForm, string approvalDate, bool isFeePaid, int transferNumber, string transferTimeStamp) {

            if (transferNumber == 0) {
                TransferNumber = "*";
            } else {
                TransferNumber = transferNumber.ToString();
                ShareTransferID = shareTransferID.ToString();
                ContractNumber = contractNumber;
                ToMemberID = toMemberID.ToString();
                ToShid = toShid;
                ToFactoryName = toFactoryName;

                if (ShareTransferID.Length > 0) {
                    ToRetainPct = toRetainPct.ToString("0.###");
                } else {
                    ToRetainPct = "";
                }
                ToCropPct = toCropPct.ToString("0.###");
                PricePerAcre = pricePerAcre.ToString("0.00");

                if (ShareTransferID.Length > 0) {
                    Shares = shares.ToString("#,##0");
                } else {
                    Shares = "";
                }
                FromMemberID = fromMemberID.ToString();
                FromShid = fromShid;
                FromFactoryName = fromFactoryName;
                FromRetainPct = fromRetainPct.ToString("0.###");
                HasLienOnShares = (hasLienOnShares ? "Y" : "N");
                HasConsentForm = (hasConsentForm ? "Y" : "N");
                ApprovalDate = approvalDate;
                IsFeePaid = (isFeePaid ? "Y" : "N");
                TransferTimeStamp = transferTimeStamp;
            }           
        }

        private string _shareTransferID = "";
        private string _contractNumber = "";
        private string _toMemberID = "";
        private string _toShid = "";
        private string _toFactoryName = "";
        private string _toRetainPct = "";
        private string _toCropPct = "";
        private string _pricePerAcre = "";
        private string _shares = "";
        private string _fromMemberID = "";
        private string _fromShid = "";
        private string _fromFactoryName = "";
        private string _fromRetainPct = "";
        private string _hasLienOnShares = "";
        private string _hasConsentForm = "";
        private string _approvalDate = "";
        private string _isFeePaid = "";
        private string _transferNumber = "";
        private string _transferTimeStamp = "";

        public string ShareTransferID {
            get { return _shareTransferID; }
            set { _shareTransferID = value; }
        }
        public string ContractNumber {
            get { return _contractNumber; }
            set { _contractNumber = value; }
        }
        public string ToMemberID {
            get { return _toMemberID; }
            set { _toMemberID = value; }
        }
        public string ToShid {
            get { return _toShid; }
            set { _toShid = value; }
        }
        public string ToFactoryName {
            get { return _toFactoryName; }
            set { _toFactoryName = value; }
        }
        public string ToRetainPct {
            get { return _toRetainPct; }
            set { _toRetainPct = value; }
        }
        public string ToCropPct {
            get { return _toCropPct; }
            set { _toCropPct = value; }
        }
        public string PricePerAcre {
            get { return _pricePerAcre; }
            set { _pricePerAcre = value; }
        }
        public string Shares {
            get { return _shares; }
            set { _shares = value; }
        }
        public string FromMemberID {
            get { return _fromMemberID; }
            set { _fromMemberID = value; }
        }
        public string FromShid {
            get { return _fromShid; }
            set { _fromShid = value; }
        }
        public string FromFactoryName {
            get { return _fromFactoryName; }
            set { _fromFactoryName = value; }
        }
        public string FromRetainPct {
            get { return _fromRetainPct; }
            set { _fromRetainPct = value; }
        }
        public string HasLienOnShares {
            get { return _hasLienOnShares; }
            set { _hasLienOnShares = value; }
        }
        public string HasConsentForm {
            get { return _hasConsentForm; }
            set { _hasConsentForm = value; }
        }
        public string ApprovalDate {
            get { return _approvalDate; }
            set { _approvalDate = value; }
        }
        public string IsFeePaid {
            get { return _isFeePaid; }
            set { _isFeePaid = value; }
        }
        public string TransferNumber {
            get { return _transferNumber; }
            set { _transferNumber = value; }
        }
        public string TransferTimeStamp {
            get { return _transferTimeStamp; }
            set { _transferTimeStamp = value; }
        }
    }
}
