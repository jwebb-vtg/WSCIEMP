using System;
using System.Xml;
using System.Runtime.Serialization;

namespace WSCData {
    /// <summary>
    /// Summary !!!   THIS CLASS IS DEPENDENT ON WSCFIELD for the value of CropYear   !!!!.
    /// </summary>
    [Serializable()]
    public class WSCShsData : ISerializable {

        int _shid = 0;
        int _cropYear = 0;

        public WSCShsData() {
        }

        //Deserialization constructor.
        public WSCShsData(SerializationInfo info, StreamingContext ctxt) {

            _shid = (int)info.GetValue("shid", typeof(int));
            _cropYear = (int)info.GetValue("cropYear", typeof(int));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("shid", _shid.ToString());
            info.AddValue("cropYear", _cropYear.ToString());
        }

        public void ResetShareholder() {
            _shid = 0;
        }

        public int SHID {
            get { return _shid; }
            set { _shid = value; }
        }
        public int CropYear {
            get {
                if (_cropYear == 0) {
                    _cropYear = Convert.ToInt32(WSCField.GetCropYears()[0].ToString());
                }
                return _cropYear; 
            }
            set { _cropYear = value; }
        }

        public override String ToString() {

            string s = "_shid: " + _shid.ToString() + "; _cropYear: " + _cropYear.ToString();
            return s;
        }
    }
}
