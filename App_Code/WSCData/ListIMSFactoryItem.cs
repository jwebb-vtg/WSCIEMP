using System;
using System.Xml;
using System.Runtime.Serialization;

namespace WSCData {

    [Serializable()]
    public class ListIMSFactoryItem {

        public ListIMSFactoryItem() { }

        public ListIMSFactoryItem(int factoryNumber, string factoryName) {

            _factoryNumber = factoryNumber.ToString();
            _factoryName = factoryName.ToString();
        }

        //Deserialization constructor.
        public ListIMSFactoryItem(SerializationInfo info, StreamingContext ctxt) {
            _factoryNumber = (string)info.GetValue("factoryNumber", typeof(string));
            _factoryName = (string)info.GetValue("factoryName", typeof(string));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("factoryNumber", _factoryNumber);
            info.AddValue("factoryName", _factoryName);
        }

        private string _factoryNumber = "";
        public string FactoryNumber {
            get { return _factoryNumber; }
            set { _factoryNumber = value; }
        }

        private string _factoryName = "";
        public string FactoryName {
            get { return _factoryName; }
            set { _factoryName = value; }
        }

        public int FactoryID {
            get { return Convert.ToInt32(_factoryNumber) / 10; }
        }
    }
}
