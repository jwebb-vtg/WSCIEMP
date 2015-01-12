using System;

namespace WSCData {

    public class List1099ExportItem {

        public List1099ExportItem() { }

        public List1099ExportItem(string shid, string taxID, string busName, string address1, string address2, string tCity, string tState, string tZip, string tDollars) {

            SHID = shid;
            TaxID = taxID;
            BusName = busName;
            Address1 = address1;
            Address2 = address2;
            TaxCity = tCity;
            TaxState = tState;
            TaxZip = tZip;
            TaxDollars = tDollars;
        }

        public string SHID { get; set;}
        public string TaxID { get; set;}
        public string BusName { get; set;}
        public string Address1 { get; set;}
        public string Address2 {get; set;}
        public string TaxCity { get; set;}
        public string TaxState {get; set;}
        public string TaxZip {get; set;}
        public string TaxDollars {get; set;}
    }
}
