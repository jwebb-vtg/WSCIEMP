using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Data.SqlClient;

namespace WSCData {
    /// <summary>
    /// Summary description for RehaulFactoryStationData.
    /// </summary>
    [XmlRootAttribute(ElementName = "Rehaul", IsNullable = false)]
    public class RehaulFactoryStationData {

        private const string MOD_NAME = "RehaulFactoryStationData.";
        private string _fileName = "";
        TFactoryList _factoryList = null;

        private enum idx : int {
            iCropYear = 0, iFactoryID, iFactoryNumber, iFactoryName, iDate, iChipsPctTailings, iRehaulLoadAvgWt, iYardLoadAvgWt, iChipsDiscardedTons,
            iBeetsSlidLoads, iStationID, iStationNumber, iStationName, iRehaulLoads
        };

        public RehaulFactoryStationData() {
            _factoryList = new TFactoryList();
        }

        public void FillFactoryDetail(string factoryNumber, SqlDataReader dr) {

            const string METHOD_NAME = "FillFactoryDetail";
            bool isFirstPass = true;
            TFactory factory = null;

            try {
                
                factory = this.FactoryList.GetFactoryByNumber(factoryNumber);
                factory.ClearRehaul();
                factory.StationList.ClearRehaul();

                while (dr.Read()) {

                    if (isFirstPass) {

                        factory.ChipsPercentTailings = dr.GetString((int)idx.iChipsPctTailings);
                        factory.RehaulLoadAverageWeight = dr.GetString((int)idx.iRehaulLoadAvgWt);
                        factory.YardLoadAverageWeight = dr.GetString((int)idx.iYardLoadAvgWt);
                        factory.ChipsDiscardedTons = dr.GetString((int)idx.iChipsDiscardedTons);
                        factory.BeetsSlidLoads = dr.GetString((int)idx.iBeetsSlidLoads);
                        isFirstPass = false;
                    }

                    if (dr.GetString((int)idx.iStationName) != "") {
                        // We have a station name because we have some rehaul.
                        TStation station = factory.StationList.GetStationByNumber(dr.GetInt32((int)idx.iStationNumber).ToString());
                        if (station != null) {
                            station.RehaulLoads = dr.GetString((int)idx.iRehaulLoads);
                        } else {
                            TStation stat = new TStation(dr.GetInt32((int)idx.iStationNumber).ToString(), dr.GetString((int)idx.iStationName));
                            stat.RehaulLoads = dr.GetString((int)idx.iRehaulLoads);
                            factory.StationList.Stations.Add(stat);
                        }
                    }

                    this.FactoryList.IsEmpty = isFirstPass;
                }
            }
            catch (Exception ex) {
                string errMsg = MOD_NAME + METHOD_NAME + "; factory number: " + (factoryNumber == null? "0": factoryNumber) + "; dr is Null: " + (dr == null? "Yes": "No") +
                    "; factory is null: " + (factory == null? "Yes": "No");
                WSCIEMP.Common.CException wscEx = new WSCIEMP.Common.CException(errMsg, ex);
                throw (wscEx);
            }
        }

        public void Load(string fileName) {

            _fileName = fileName;
            XmlSerializer serializer = new XmlSerializer(typeof(RehaulFactoryStationData));

            // A FileStream is needed to read the XML document.            
            // Declare an object variable of the type to be deserialized.
            RehaulFactoryStationData rehaulData;

            // Use the Deserialize method to restore the object's state with
            // data from the XML document. 
            using (FileStream fs = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)) {
                rehaulData = (RehaulFactoryStationData)serializer.Deserialize(fs);
            }

            this._factoryList = rehaulData.FactoryList;
        }

        [XmlElement("FactoryList")]
        public TFactoryList FactoryList {
            get { return this._factoryList; }
            set { this._factoryList = value; }
        }
    }

    public class TFactoryList {

        private ArrayList _factories = null;

        public TFactoryList() {
            _factories = new ArrayList();
        }

        [XmlElement(ElementName = "Factory", Type = typeof(TFactory))]
        public ArrayList Factories {
            get { return this._factories; }
            set { this._factories = value; }
        }

        public TFactory GetFactoryByNumber(string factoryNumber) {

            foreach (TFactory factory in _factories) {
                if (factory.FactoryNumber == factoryNumber) {
                    return factory;
                }
            }

            return null;
        }

        private bool _isEmpty = true;
        public bool IsEmpty {
            get { return _isEmpty; }
            set { _isEmpty = value; }
        }
    }

    public class TFactory {

        string _factoryID = "";
        string _factoryName = "";
        TStationList _stationList = null;

        string _chipsPercentTailings = "";
        string _rehaulLoadAverageWeight = "";
        string _yardLoadAverageWeight = "";
        string _chipsDiscardedTons = "";
        string _beetsSlidLoads = "";

        public TFactory() {
            _stationList = new TStationList();
        }

        public TFactory(string factoryID, string factoryName) {
            _factoryID = factoryID;
            _factoryName = factoryName;
            _stationList = new TStationList();
        }

        public void ClearRehaul() {
            _chipsPercentTailings = "";
            _rehaulLoadAverageWeight = "";
            _yardLoadAverageWeight = "";
            _chipsDiscardedTons = "";
            _beetsSlidLoads = "";
        }

        [XmlAttribute("factoryID")]
        public string FactoryID {
            get { return this._factoryID; }
            set { this._factoryID = value; }
        }

        [XmlAttribute("factoryName")]
        public string FactoryName {
            get { return this._factoryName; }
            set { this._factoryName = value; }
        }

        [XmlElement("StationList")]
        public TStationList StationList {
            get { return this._stationList; }
            set { this._stationList = value; }
        }

        public string FactoryNumber {
            get {
                return (Convert.ToInt32(this._factoryID) * 10).ToString();
            }
        }

        public string ChipsPercentTailings {
            get { return this._chipsPercentTailings; }
            set { this._chipsPercentTailings = value; }
        }
        public string RehaulLoadAverageWeight {
            get { return this._rehaulLoadAverageWeight; }
            set { this._rehaulLoadAverageWeight = value; }
        }
        public string YardLoadAverageWeight {
            get { return this._yardLoadAverageWeight; }
            set { this._yardLoadAverageWeight = value; }
        }
        public string ChipsDiscardedTons {
            get { return this._chipsDiscardedTons; }
            set { this._chipsDiscardedTons = value; }
        }
        public string BeetsSlidLoads {
            get { return this._beetsSlidLoads; }
            set { this._beetsSlidLoads = value; }
        }
    }

    public class TStationList {

        private ArrayList _stations = null;

        public TStationList() {
            _stations = new ArrayList();
        }

        [XmlElement(ElementName = "Station", Type = typeof(TStation))]
        public ArrayList Stations {
            get { return this._stations; }
            set { this._stations = value; }
        }

        public TStation GetStationByNumber(string stationNumber) {

            stationNumber = Convert.ToInt32(stationNumber).ToString();
            foreach (TStation station in _stations) {
                if (station.StationNumber == stationNumber) {
                    return station;
                }
            }

            return null;
        }

        public string TotalStationRehaulLoads {
            get {

                decimal tot = 0;
                foreach (TStation station in _stations) {
                    if (station.RehaulLoads.Length > 0) {
                        tot += Convert.ToDecimal(station.RehaulLoads);
                    }
                }

                return (tot != 0 ? tot.ToString() : "");
            }
        }

        public void ClearRehaul() {

            foreach (TStation station in _stations) {
                station.RehaulLoads = "";
            }
        }
    }
    public class TStation {

        string _stationNumber = "";
        string _stationName = "";
        string _rehaulLoads = "";

        public TStation() { }

        public TStation(string stationNumber, string stationName) {
            _stationNumber = stationNumber;
            _stationName = stationName;
        }

        [XmlAttribute("stationNumber")]
        public string StationNumber {
            get { return this._stationNumber; }
            set { this._stationNumber = value; }
        }

        [XmlAttribute("stationName")]
        public string StationName {
            get { return this.StationNumber.PadLeft(2, '0') + " - " + this._stationName; }
            set { this._stationName = value; }
        }

        public string RehaulLoads {
            get {
                if (this._rehaulLoads.Length > 0) {
                    return Convert.ToDecimal(this._rehaulLoads).ToString("0.###");
                } else {
                    return ""; ;
                }
            }
            set { this._rehaulLoads = value; }
        }
    }
}

