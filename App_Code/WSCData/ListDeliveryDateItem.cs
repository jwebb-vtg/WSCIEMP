using System;

namespace WSCData {

    public class ListDeliveryDateItem {

        public ListDeliveryDateItem() { }

        public ListDeliveryDateItem(string deliveryDate) {
            DeliveryDate = deliveryDate;
        }

        private string _deliveryDate = "";
        public string DeliveryDate {
            get { return _deliveryDate; }
            set { _deliveryDate = value; }
        }
    }
}
