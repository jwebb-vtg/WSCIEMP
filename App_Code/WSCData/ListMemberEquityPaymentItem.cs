using System;

namespace WSCData {

    public class ListMemberEquityPaymentItem {

        private const string BLANK_CELL = "&nbsp;";

        public ListMemberEquityPaymentItem() { }

        public ListMemberEquityPaymentItem(int cropYear, string equityType, string paymentDesc, decimal payAmount, int sequence) {

            if (cropYear == 0) {
                CropYear = "*";
            } else {
                CropYear = cropYear.ToString();
                EquityType = equityType;
                PaymentDesc = paymentDesc;
                PayAmount = payAmount.ToString("0.00");
                Sequence = sequence.ToString();
            }
        }

        private string _cropYear = "*";
        public string CropYear {
            get { return _cropYear; }
            set { _cropYear = value; }
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

        private string _payAmount = "";
        public string PayAmount {
            get { return _payAmount; }
            set { _payAmount = value; }
        }

        private string _sequence = "";
        public string Sequence {
            get { return _sequence; }
            set { _sequence = value; }
        }
    }
}
