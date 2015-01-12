using System;

namespace WSCData {

    public class ListEquityPaymentScheduleItem {

        public ListEquityPaymentScheduleItem() { }

        public ListEquityPaymentScheduleItem(int sequence, string groupType, string equityType, string paymentDesc, string payDate) {

            Sequence = sequence.ToString();
            GroupType = groupType;
            EquityType = equityType;
            PaymentDesc = paymentDesc;
            PayDate = payDate;
        }

        private string _sequence = "0";
        public string Sequence {
            get { return _sequence; }
            set { _sequence = value; }
        }

        private string _groupType = "";
        public string GroupType {
            get { return _groupType; }
            set { _groupType = value; }
        }

        private string _equityType = "";
        public string EquityType {
            get { return _equityType; }
            set { _equityType = value; }
        }

        private string _paymentDesc = "";
        public string PaymentDesc {
            get { return _paymentDesc; }
            set { _paymentDesc = value; }
        }

        private string _payDate = "";
        public string PayDate {
            get { return _payDate; }
            set { _payDate = value; }
        }

    }
}
