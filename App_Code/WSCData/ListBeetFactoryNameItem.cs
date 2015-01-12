using System;

namespace WSCData {

    public class ListBeetFactoryNameItem {

        public ListBeetFactoryNameItem() { }

        public ListBeetFactoryNameItem(int ftyNumber, string ftyName) {

            FactoryName = ftyName;
            FactoryNumber = ftyNumber.ToString();
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
