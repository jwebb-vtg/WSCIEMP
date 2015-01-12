using System;

namespace WSCData {

    public class ListEquityDeductionItem {

        private const string BLANK_CELL = "&nbsp;";

        public ListEquityDeductionItem() { }

        public ListEquityDeductionItem(int equityDeductionID, int deductionNumber, string deductionDescription, bool isActive, string rowVersion) {

            if (deductionNumber == 0) {
                DeductionNumber = "*";
            } else {
                EquityDeductionID = equityDeductionID.ToString();
                DeductionNumber = deductionNumber.ToString();
                DeductionDescription = deductionDescription;
                IsActive = (isActive ? "Y" : "N");
                RowVersion = rowVersion;
            }
        }

        public ListEquityDeductionItem(string equityDeductionID, string deductionNumber, string deductionDescription, string isActive, string rowVersion) {

            string sID = equityDeductionID.Replace(BLANK_CELL, "0");
            EquityDeductionID = (sID.Length == 0 ? "0" : sID);
            string dNum = deductionNumber.Replace(BLANK_CELL, "0");
            DeductionNumber = (dNum.Length == 0? "0": dNum);
            DeductionDescription = deductionDescription.Replace(BLANK_CELL, "");
            IsActive = (isActive.Replace(BLANK_CELL, "") == "Y" ? "Y" : "N");
            RowVersion = rowVersion.Replace(BLANK_CELL, "");
        }

        private string _equityDeductionID = "0";
        public string EquityDeductionID {
            get { return _equityDeductionID; }
            set { _equityDeductionID = value;}
        }

        private string _deductionNumber = "0";
        public string DeductionNumber {
            get { return _deductionNumber; }
            set { _deductionNumber = value; }
        }

        private string _deductionDescription = "";
        public string DeductionDescription {
            get { return _deductionDescription; }
            set { _deductionDescription = value; }
        }

        private string _isActive = "";
        public string IsActive {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public bool IsActiveAsBool() {
            return (_isActive == "Y" ? true : false);
        }

        private string _rowVersion = "";
        public string RowVersion {
            get { return _rowVersion; }
            set { _rowVersion = value; }
        }

    }
}
