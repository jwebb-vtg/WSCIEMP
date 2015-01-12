using System;

namespace WSCData {

    public class ListMemberStockSummaryItem {

        public ListMemberStockSummaryItem() { }

        public ListMemberStockSummaryItem(string memberNumber, string factoryName, int patronShares, int patronOverPlant, int transfereeShares, int transfereeOverPlant,
            int transferorShares, int deliveryShareRights, int sharesUsed, int sharesUnassigned, bool hasLien, bool isSubscriber) {

            SHID = memberNumber;
            FactoryName = factoryName;
            PatronShares = patronShares.ToString("#,##0");
            PatronOverPlant = patronOverPlant.ToString("#,##0");
            TransfereeShares = transfereeShares.ToString("#,##0");
            TransfereeOverPlant = transfereeOverPlant.ToString("#,##0");
            TransferorShares = transferorShares.ToString("#,##0");
            DeliveryShareRights = deliveryShareRights.ToString("#,##0");
            SharesUsed = sharesUsed.ToString("#,##0");
            SharesUnassigned = sharesUnassigned.ToString("#,##0");
            HasLien = (hasLien ? "Y" : "N");
            IsSubscriber = (isSubscriber ? "Y" : "N");            
        }

        private string _shid = "";
        private string _factoryName = "";
        private string _patronShares = "";
        private string _patronOverPlant = "";
        private string _transfereeShares = "";
        private string _transfereeOverPlant = "";
        private string _transferorShares = "";
        private string _deliveryShareRights = "";
        private string _sharesUsed = "";
        private string _sharesUnassigned = "";
        private string _hasLien = "";
        private string _isSubscriber = "";        

        public string SHID {
            get { return _shid; }
            set { _shid = value; }
        }
        public string FactoryName {
            get { return _factoryName; }
            set { _factoryName = value; }
        }
        public string PatronShares {
            get { return _patronShares; }
            set { _patronShares = value; }
        }
        public string PatronOverPlant {
            get { return _patronOverPlant; }
            set { _patronOverPlant = value; }
        }
        public string TransfereeShares {
            get { return _transfereeShares; }
            set { _transfereeShares = value; }
        }
        public string TransfereeOverPlant {
            get { return _transfereeOverPlant; }
            set { _transfereeOverPlant = value; }
        }
        public string TransferorShares {
            get { return _transferorShares; }
            set { _transferorShares = value; }
        }
        public string DeliveryShareRights {
            get { return _deliveryShareRights; }
            set { _deliveryShareRights = value; }
        }
        public string SharesUsed {
            get { return _sharesUsed; }
            set { _sharesUsed = value; }
        }
        public string SharesUnassigned {
            get { return _sharesUnassigned; }
            set { _sharesUnassigned = value; }
        }
        public string HasLien {
            get { return _hasLien; }
            set { _hasLien = value; }
        }
        public string IsSubscriber {
            get { return _isSubscriber; }
            set { _isSubscriber = value; }
        }
    }
}
