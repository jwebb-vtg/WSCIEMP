namespace WSCData {

    public class ListBeetContractIDItem {

        public ListBeetContractIDItem() { }

        public ListBeetContractIDItem(int cntID, string cntNumber) {

            ContractID = cntID.ToString();
            ContractNumber = cntNumber;
        }

        private string _contractID = "";
        public string ContractID {
            get { return _contractID; }
            set { _contractID = value; }
        }

        private string _contractNumber = "";
        public string ContractNumber {
            get { return _contractNumber; }
            set { _contractNumber = value; }
        }
    }

    public class ListBeetContractDeliveryDateItem {

        public ListBeetContractDeliveryDateItem() {}

        public ListBeetContractDeliveryDateItem(string contractNumber, string deliveryDate) {
            
            ContractNumber = contractNumber;
            DeliveryDate = deliveryDate;
        }

        public string ContractNumber { get; set; }
        public string DeliveryDate { get; set; }
    }
}
