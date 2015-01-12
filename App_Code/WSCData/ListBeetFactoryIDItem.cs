using System;

namespace WSCData {

    public class ListBeetFactoryIDItem {

        public ListBeetFactoryIDItem() { }

        public ListBeetFactoryIDItem(int ftyID, int ftyNumber, string ftyName) {

            FactoryID = ftyID.ToString();
            FactoryName = ftyName;
            FactoryNumber = ftyNumber.ToString();
        }

        private string _factoryID = "";
        public string FactoryID {
            get { return _factoryID; }
            set { _factoryID = value; }
        }

        private string _factoryName = "";
        public string FactoryName {
            get { return _factoryName; }
            set { _factoryName = value; }
        }

        private string _factoryNumber = "";
        public string FactoryNumber {
            get { return _factoryNumber; }
            set { _factoryNumber = value; }
        }

        public string FactoryLongName {
            get { return _factoryNumber + " - " + _factoryName; }
        }
    }
}
