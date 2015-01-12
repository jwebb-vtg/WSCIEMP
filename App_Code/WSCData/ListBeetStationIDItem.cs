using System;

namespace WSCData {

    public class ListBeetStationIDItem {

        public ListBeetStationIDItem() { }

        // dr.GetInt32(iStaID), dr.GetInt16(iStaNum), dr.GetString(iStaName), dr.GetString(iStaNumName)));
        public ListBeetStationIDItem(int staID, int staNum, string staName, string staNumName) {

            StationID = staID.ToString();
            StationNumber = staNum.ToString();
            StationName = staName;
            StationNumberName = staNumName;
        }

        private string _stationID = "";
        public string StationID {
            get { return _stationID; }
            set { _stationID = value; }
        }

        private string _stationNumber = "";
        public string StationNumber {
            get { return _stationNumber; }
            set { _stationNumber = value; }
        }

        private string _stationName = "";
        public string StationName {
            get { return _stationName; }
            set { _stationName = value; }
        }

        private string _stationNumberName = "";
        public string StationNumberName {
            get { return _stationNumberName; }
            set { _stationNumberName = value; }
        }
    }
}
