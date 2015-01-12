using System;

namespace WSCData {

    public class ListOverPlantMemberItem {

        public ListOverPlantMemberItem() { }

        public ListOverPlantMemberItem(int overPlantID, int memberID, string shid, string overPlantAccept, int overPlantUsed, bool isFormReceived, int patronSharesOwned,
            decimal overPlantPct, int overPlantPossible, string homeFactoryNumber, string homeFactoryName,
            string overPlantFactoryNumber, string overPlantFactoryName, bool isOverridePct, bool isOverPlantAllowed) {

            SHID = shid;
            switch (overPlantAccept) {
                case "Y":
                    OverPlantAccept = "Yes";
                    break;
                case "N":
                    OverPlantAccept = "No";
                    break;
                case "P":
                    OverPlantAccept = "Partial";
                    break;
                default:
                    OverPlantAccept = "Undecided";
                    break;
            }

            OverPlantID = overPlantID.ToString();
            MemberID = memberID.ToString();
            OverPlantUsed = overPlantUsed.ToString("#,##0");
            IsFormReceived = isFormReceived;
            PatronSharesOwned = patronSharesOwned.ToString("#,##0");
            OverPlantPct = overPlantPct.ToString("##0.0##");
            OverPlantPossible = overPlantPossible.ToString("#,##0");
            HomeFactoryNumber = homeFactoryNumber;
            HomeFactoryName = homeFactoryNumber + " - " + homeFactoryName;
            OverPlantFactoryNumber = overPlantFactoryNumber;
            OverPlantFactoryName = overPlantFactoryNumber + " - " + overPlantFactoryName;
            IsOverridePct = isOverridePct;
            IsOverPlantAllowed = isOverPlantAllowed;
        }

        private string _overPlantID = "";
        public string OverPlantID {
            get { return _overPlantID; }
            set { _overPlantID = value; }
        }

        private string _memberID = "";
        public string MemberID {
            get { return _memberID; }
            set { _memberID = value; }
        }

        private string _shid = "";
        public string SHID {
            get { return _shid; }
            set { _shid = value; }
        }

        private string _overPlantAccept = "";
        public string OverPlantAccept {
            get { return _overPlantAccept; }
            set { _overPlantAccept = value; }
        }

        private string _overPlantUsed = "";
        public string OverPlantUsed {
            get { return _overPlantUsed; }
            set { _overPlantUsed = value; }
        }

        private bool _isFormReceived = false;
        public bool IsFormReceived {
            get { return _isFormReceived; }
            set { _isFormReceived = value; }
        }

        private string _patronSharesOwned = "";
        public string PatronSharesOwned {
            get { return _patronSharesOwned; }
            set { _patronSharesOwned = value; }
        }

        private string _overPlantPct = "";
        public string OverPlantPct {
            get { return _overPlantPct; }
            set { _overPlantPct = value; }
        }

        private string _overPlantPossible = "";
        public string OverPlantPossible {
            get { return _overPlantPossible; }
            set { _overPlantPossible = value; }
        }

        private string _homeFactoryNumber = "";
        public string HomeFactoryNumber {
            get { return _homeFactoryNumber; }
            set { _homeFactoryNumber = value; }
        }

        private string _homeFactoryName = "";
        public string HomeFactoryName {
            get { return _homeFactoryName; }
            set { _homeFactoryName = value; }
        }

        private string _overPlantFactoryNumber = "";
        public string OverPlantFactoryNumber {
            get { return _overPlantFactoryNumber; }
            set { _overPlantFactoryNumber = value; }
        }

        private string _overPlantFactoryName = "";
        public string OverPlantFactoryName {
            get { return _overPlantFactoryName; }
            set { _overPlantFactoryName = value; }
        }

        private bool _isOverridePct = false;
        public bool IsOverridePct {
            get { return _isOverridePct; }
            set { _isOverridePct = value; }
        }

        private bool _isOverPlantAllowed = false;
        public bool IsOverPlantAllowed {
            get { return _isOverPlantAllowed; }
            set { _isOverPlantAllowed = value; }
        }
    }
}
