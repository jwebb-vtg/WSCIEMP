using System;

namespace WSCData {

    public class ListMemberEquityDeductionItem {

        private const string BLANK_CELL = "&nbsp;";

        public ListMemberEquityDeductionItem() { }

        public ListMemberEquityDeductionItem(int memberEquityDeductionID, int equityDeductionID, string rowVersion, int equityCropYear, string equityType, 
            int paySequence, string payDesc, string deductionDesc, decimal deductionAmount) {

            if (equityCropYear == 0) {
                EquityCropYear = "*";
            } else {
                MemberEquityDeductionID = memberEquityDeductionID.ToString();
                EquityDeductionID = equityDeductionID.ToString();
                RowVersion = rowVersion;
                EquityCropYear = equityCropYear.ToString();
                EquityType = equityType;
                PaySequence = paySequence.ToString();
                PayDesc = payDesc;
                DeductionDesc = deductionDesc;
                DeductionAmount = deductionAmount.ToString("0.00");
            }
        }

        public ListMemberEquityDeductionItem(string memberEquityDeductionID, string equityDeductionID, string rowVersion, string equityCropYear, string equityType, 
            string paySequence, string payDesc, string deductionDesc, string deductionAmount) {

            string sID = memberEquityDeductionID.Replace(BLANK_CELL, "0");
            MemberEquityDeductionID = (sID.Length == 0 ? "0" : sID);

            string edID = equityDeductionID.Replace(BLANK_CELL, "0");
            EquityDeductionID = (edID.Length == 0 ? "0" : edID);

            string rv = rowVersion.Replace(BLANK_CELL, "");
            RowVersion = rv;

            string ecy = equityCropYear.Replace(BLANK_CELL, "");
            EquityCropYear = ecy;

            string et = equityType.Replace(BLANK_CELL, "");
            EquityType = et;

            string ps = paySequence.Replace(BLANK_CELL, "");
            PaySequence = ps;

            string pd = payDesc.Replace(BLANK_CELL, "");
            PayDesc = pd;

            string dd = deductionDesc.Replace(BLANK_CELL, "");
            DeductionDesc = dd;

            string da = deductionAmount.Replace(BLANK_CELL, "");
            DeductionAmount = da;
        }

        private string _memberEquityDeductionID = "0";
        public string MemberEquityDeductionID {
            get { return _memberEquityDeductionID; }
            set { _memberEquityDeductionID = value; }
        }

        private string _equityDeductionID = "0";
        public string EquityDeductionID {
            get { return _equityDeductionID; }
            set { _equityDeductionID = value; }
        }

        private string _rowVersion = "";
        public string RowVersion {
            get { return _rowVersion; }
            set { _rowVersion = value; }
        }

        private string _equityCropYear = "0";
        public string EquityCropYear {
            get { return _equityCropYear; }
            set { _equityCropYear = value; }
        }

        private string _equityType = "";
        public string EquityType {
            get { return _equityType; }
            set { _equityType = value; }
        }


        private string _paySequence = "";
        public string PaySequence {
            get { return _paySequence; }
            set { _paySequence = value; }
        }

        private string _payDesc = "";
        public string PayDesc {
            get { return _payDesc; }
            set { _payDesc = value; }
        }

        private string _deductionDesc = "";
        public string DeductionDesc {
            get { return _deductionDesc; }
            set { _deductionDesc = value; }
        }

        private string _deductionAmount = "";
        public string DeductionAmount {
            get { return _deductionAmount; }
            set { _deductionAmount = value; }
        }
    }
}
