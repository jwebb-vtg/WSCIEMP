using System;
using System.Xml;
using System.Runtime.Serialization;

namespace WSCData {
    /// <summary>
    /// Summary description for WSCFieldData.
    /// </summary>
    [Serializable()]
    public class WSCFieldData : ISerializable {

        int _contractID = 0;
        int _contractNumber = 0;
        int _sequenceNumber = 0;

        public WSCFieldData() {
        }

        //Deserialization constructor.
        public WSCFieldData(SerializationInfo info, StreamingContext ctxt) {

            _contractID = (int)info.GetValue("contractID", typeof(int));
            _contractNumber = (int)info.GetValue("contractNumber", typeof(int));
            _sequenceNumber = (int)info.GetValue("sequenceNumber", typeof(int));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("contractID", _contractID.ToString());
            info.AddValue("contractNumber", _contractNumber.ToString());
            info.AddValue("sequenceNumber", _sequenceNumber.ToString());
        }

        public void ResetContract() {
            ContractID = 0;
            ContractNumber = 0;
            ResetField();
        }

        public void ResetField() {
            SequenceNumber = 0;
        }
        public int ContractID {
            get { return _contractID; }
            set { _contractID = value; }
        }
        public int ContractNumber {
            get { return _contractNumber; }
            set { _contractNumber = value; }
        }
        public int SequenceNumber {
            get { return _sequenceNumber; }
            set { _sequenceNumber = value; }
        }

        public override String ToString() {

            string s = "_contractID: " + _contractID.ToString() + "; " +
                "_contractNumber: " + _contractNumber.ToString() + "; " +
                "_sequenceNumber: " + _sequenceNumber.ToString();

            return s;
        }
    }
}
