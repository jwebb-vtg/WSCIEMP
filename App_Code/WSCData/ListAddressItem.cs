using System;

namespace WSCData {

    public class ListAddressItem {

        public ListAddressItem() { }

        public ListAddressItem(int addressID, int memberID, string shid, bool isSubscriber, string firstName, string lastName, string busName, string taxID, string adrLine1, string adrLine2, string cityName, 
            string stateName, string postalCode, string phoneNo, string email, int addressType) {

            AddressID = addressID;
            MemberID = memberID;
            SHID = shid;
            IsSubscriber = isSubscriber;
            FirstName = firstName;
            LastName = lastName;
            BusName = busName;
            TaxID = taxID;
            AdrLine1 = adrLine1;
            AdrLine2 = adrLine2;
            CityName = cityName;
            StateName = stateName;
            PostalCode = postalCode;
            PhoneNo = phoneNo;
            Email = email;
            AddressType = addressType;
        }

        public int AddressID { get; set; }
        public int MemberID { get; set; }
        public string SHID { get; set; }
        public bool IsSubscriber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusName { get; set; }
        public string TaxID { get; set; }
        public string AdrLine1 { get; set; }
        public string AdrLine2 {get; set;}
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public int AddressType { get; set; }
    }
}
