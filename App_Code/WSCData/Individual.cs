using System;
using System.Runtime.Serialization;

namespace WSCData {

    [Serializable]
    public class Individual : ISerializable {

        public Individual() {}

        public Individual(int individualId, string fullName)
        {
            IndividualID = individualId;
            FullName = fullName;
        }

        public Individual(int individualId, double percentage, Boolean signed, char sort)
        {
            IndividualID = individualId;
            Percentage = percentage;
            Signed = signed;
            Sort = sort;
        }

        public Individual(int individualId, string fullName, double percentage, Boolean signed, DateTime signedDate, char sort)
        {
            IndividualID = individualId;
            FullName = fullName;
            Percentage = percentage;
            Signed = signed;
            SignedDate = signedDate;
            Sort = sort;
        }

        public Individual(int individualId, string fullName, double percentage, Boolean signed, DateTime signedDate, char sort, int shid)
        {
            IndividualID = individualId;
            FullName = fullName;
            Percentage = percentage;
            Signed = signed;
            SignedDate = signedDate;
            Sort = sort;
            SHID = shid;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IndividualID", IndividualID, typeof(int));
            info.AddValue("FullName", FullName, typeof(string));
            info.AddValue("Percentage", Percentage, typeof(double));
            info.AddValue("Signed", Signed, typeof(Boolean));
            info.AddValue("SignedDate", SignedDate, typeof(DateTime));
            info.AddValue("Sort", Sort, typeof(char));
            info.AddValue("SHID", SHID, typeof(int));
        }

        public Individual(SerializationInfo info, StreamingContext context)
        {
            IndividualID = (int)info.GetValue("IndividualID", typeof(int));
            FullName = (string)info.GetValue("FullName", typeof(string));
            Percentage = (double)info.GetValue("Percentage", typeof(double));
            Signed = (Boolean)info.GetValue("Signed", typeof(Boolean));
            SignedDate = (DateTime)info.GetValue("SignedDate", typeof(DateTime));
            Sort = (char)info.GetValue("Sort", typeof(char));
            SHID = (int)info.GetValue("SHID", typeof(int));
        }

        public override string ToString() {
            if (Signed)
                return Convert.ToString(IndividualID) + "|" + Convert.ToChar(Sort) + "|" + Convert.ToString(Percentage) + "|true|" + SignedDate.ToString("MM/dd/yyyy");
            else
                return Convert.ToString(IndividualID) + "|" + Convert.ToChar(Sort) + "|" + Convert.ToString(Percentage) + "|false|";
        }
        
        public int IndividualID { get; set; }
        public string FullName { get; set; }
        public double Percentage { get; set; }
        public Boolean Signed { get; set; }
        public DateTime SignedDate { get; set; }
        public char Sort { get; set; }
        public int SHID { get; set; }
    }
}