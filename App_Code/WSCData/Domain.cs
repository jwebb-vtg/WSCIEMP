using System;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Data.SqlClient;

namespace WSCData {

    /// <summary>
    /// Summary description for Domain.
    /// </summary>
    [XmlRootAttribute(ElementName = "Domain", IsNullable = false)]
    public class Domain {

        private const string MOD_NAME = "Domain.";
        private const string NO_VALUE = "None";
        string _fileName = "";
        TItemList _stateList = null;
        TItem2List _countyList = null;
        TItem2List _townshipList = null;
        TItem2List _rangeList = null;
        TItemList _sectionList = null;
        TItemList _quadrantList = null;
        TItemList _quarterQuadrantList = null;
        TItemList _fieldRotationLengthList = null;
        TItemList _fieldPriorCropList = null;
        TItemList _fieldBeetYearsList = null;
        TItemList _fieldIrrigationSystemList = null;
        TItemList _fieldWaterSourceList = null;
        TItemList _diseaseSeverityList = null;
        TItemList _tillageTypeList = null;
        TItemList _landOwnershipList = null;

        public Domain() {

            _stateList = new TItemList();
            _countyList = new TItem2List();
            _townshipList = new TItem2List();
            _rangeList = new TItem2List();
            _sectionList = new TItemList();
            _quadrantList = new TItemList();
            _quarterQuadrantList = new TItemList();
            _fieldRotationLengthList = new TItemList();
            _fieldPriorCropList = new TItemList();
            _fieldBeetYearsList = new TItemList();
            _fieldIrrigationSystemList = new TItemList();
            _fieldWaterSourceList = new TItemList();
            _diseaseSeverityList = new TItemList();
            _tillageTypeList = new TItemList();
            _landOwnershipList = new TItemList();
        }

        public void Load(string fileName) {

            _fileName = fileName;
            XmlSerializer serializer = new XmlSerializer(typeof(Domain));

            // A FileStream is needed to read the XML document.
            // Declare an object variable of the type to be deserialized.
            Domain domain;

            /* Use the Deserialize method to restore the object's state with
            data from the XML document. */
            using (FileStream fs = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)) {
                domain = (Domain)serializer.Deserialize(fs);
            }

            this._stateList = domain.StateList;
            this._countyList = domain.CountyList;
            this._townshipList = domain.TownshipList;
            this._rangeList = domain.RangeList;

            GetSectionList();
            GetQuadrantList();
            GetQuarterQuadrantList();
            GetFieldPriorCropList();
            GetFieldRotationLengthList();
            GetBeetYearList();
            GetIrrigationSystemList();
            GetWaterSourceList();

            GetDiseaseSeverityList();
            GetTillageTypeList();
            GetLandOwnershipList();
        }

        [XmlElement("stateList")]
        public TItemList StateList {
            get { return this._stateList; }
            set { this._stateList = value; }
        }

        [XmlElement("countyList")]
        public TItem2List CountyList {
            get { return this._countyList; }
            set { this._countyList = value; }
        }

        [XmlElement("townshipList")]
        public TItem2List TownshipList {
            get { return this._townshipList; }
            set { this._townshipList = value; }
        }

        [XmlElement("rangeList")]
        public TItem2List RangeList {
            get { return this._rangeList; }
            set { this._rangeList = value; }
        }

        [XmlElement("sectionList")]
        public TItemList SectionList {
            get { return this._sectionList; }
            set { this._sectionList = value; }
        }

        [XmlElement("quadrantList")]
        public TItemList QuadrantList {
            get { return this._quadrantList; }
            set { this._quadrantList = value; }
        }

        [XmlElement("quarterQuadrantList")]
        public TItemList QuarterQuadrantList {
            get { return this._quarterQuadrantList; }
            set { this._quarterQuadrantList = value; }
        }

        [XmlElement("FieldRotationLengthList")]
        public TItemList FieldRotationLengthList {
            get { return this._fieldRotationLengthList; }
            set { this._fieldRotationLengthList = value; }
        }

        [XmlElement("FieldPriorCropList")]
        public TItemList FieldPriorCropList {
            get { return this._fieldPriorCropList; }
            set { this._fieldPriorCropList = value; }
        }

        [XmlElement("FieldBeetYearsList")]
        public TItemList FieldBeetYearsList {
            get { return this._fieldBeetYearsList; }
            set { this._fieldBeetYearsList = value; }
        }

        [XmlElement("FieldIrrigationSystemList")]
        public TItemList FieldIrrigationSystemList {
            get { return this._fieldIrrigationSystemList; }
            set { this._fieldIrrigationSystemList = value; }
        }

        [XmlElement("FieldWaterSourceList")]
        public TItemList FieldWaterSourceList {
            get { return this._fieldWaterSourceList; }
            set { this._fieldWaterSourceList = value; }
        }


        [XmlElement("DiseaseSeverityList")]
        public TItemList DiseaseSeverityList {
            get { return this._diseaseSeverityList; }
            set { this._diseaseSeverityList = value; }
        }

        [XmlElement("TillageTypeList")]
        public TItemList TillageTypeList {
            get { return this._tillageTypeList; }
            set { this._tillageTypeList = value; }
        }

        [XmlElement("LandOwnershipList")]
        public TItemList LandOwnershipList {
            get { return this._landOwnershipList; }
            set { this._landOwnershipList = value; }
        }

        private void GetSectionList() {

            const string METHOD_NAME = "GetSectionList";
            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "SECTION")) {

                        this._sectionList.AddItem("");
                        while (dr.Read()) {
                            this._sectionList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetQuadrantList() {

            const string METHOD_NAME = "GetQuadrantList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "QUADRANTS")) {

                        this._quadrantList.AddItem("");
                        while (dr.Read()) {
                            this._quadrantList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetQuarterQuadrantList() {

            const string METHOD_NAME = "GetQuarterQuadrantList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "QUARTER_QUAD")) {

                        this._quarterQuadrantList.AddItem("");
                        while (dr.Read()) {
                            this._quarterQuadrantList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetFieldPriorCropList() {

            const string METHOD_NAME = "GetFieldPriorCropList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "PREVIOUS_CROP")) {

                        this._fieldPriorCropList.AddItem(NO_VALUE);
                        while (dr.Read()) {
                            this._fieldPriorCropList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetFieldRotationLengthList() {

            const string METHOD_NAME = "GetFieldRotationLengthList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "FIELD_ROTATE")) {

                        this._fieldRotationLengthList.AddItem("0");
                        while (dr.Read()) {
                            this._fieldRotationLengthList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetBeetYearList() {

            const string METHOD_NAME = "GetBeetYearList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "BEET_YEARS")) {

                        this._fieldBeetYearsList.AddItem("0");
                        while (dr.Read()) {
                            this._fieldBeetYearsList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetIrrigationSystemList() {

            const string METHOD_NAME = "GetIrrigationSystemList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "IRRIG_METHOD")) {

                        this._fieldIrrigationSystemList.AddItem(NO_VALUE);
                        while (dr.Read()) {
                            this._fieldIrrigationSystemList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetWaterSourceList() {

            const string METHOD_NAME = "GetWaterSourceList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "IRRIG_SOURCE")) {

                        this._fieldWaterSourceList.AddItem(NO_VALUE);
                        while (dr.Read()) {
                            this._fieldWaterSourceList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetDiseaseSeverityList() {

            const string METHOD_NAME = "GetDiseaseSeverityList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "DISEASE_SEVERITY")) {

                        this._diseaseSeverityList.AddItem(NO_VALUE);
                        while (dr.Read()) {
                            this._diseaseSeverityList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetTillageTypeList() {

            const string METHOD_NAME = "GetTillageTypeList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "TILLAGE_TYPE")) {

                        this._tillageTypeList.AddItem(NO_VALUE);
                        while (dr.Read()) {
                            this._tillageTypeList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }

        private void GetLandOwnershipList() {

            const string METHOD_NAME = "GetLandOwnershipList";

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "LAND_OWNERSHIP")) {

                        this._landOwnershipList.AddItem(NO_VALUE);
                        while (dr.Read()) {
                            this._landOwnershipList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + METHOD_NAME, ex);
                throw (wex);
            }
        }
    }

    public class TItem2List {

        private ArrayList _item2s = null;

        public TItem2List() {
            _item2s = new ArrayList();
        }

		public void AddItem(string field1, string field2) {

			TItem2 item = new TItem2();
			item.Field1 = field1;
			item.Field2 = field2;
			_item2s.Add(item);
		}

        public bool Contains(string field1, string field2) {

            foreach (TItem2 item in _item2s) {
                if (item.Field1 == field1 && item.Field2 == field2) {
                    return true;
                }
            }
            return false;
        }

        [XmlElement(ElementName = "Item2", Type = typeof(TItem2))]
        public ArrayList Items {
            get { return this._item2s; }
            set { this._item2s = value; }
        }

    }

    public class TItem2 {

        private string _field1 = "";
        private string _field2 = "";

        public TItem2() {
        }

        [XmlAttribute("field1")]
        public string Field1 {
            get { return this._field1; }
            set { this._field1 = value; }
        }

        [XmlAttribute("field2")]
        public string Field2 {
            get { return this._field2; }
            set { this._field2 = value; }
        }
    }

    public class TItemList {

        private ArrayList _items = null;

        public TItemList() {
            _items = new ArrayList();
        }

        public void AddItem(string name) {

            TItem item = new TItem();
            item.Name = name;
            _items.Add(item);
        }

        public bool Contains(string name) {

            foreach (TItem item in _items) {
                if (item.Name == name) {
                    return true;
                }
            }
            return false;
        }

        [XmlElement(ElementName = "Item", Type = typeof(TItem))]
        public ArrayList Items {
            get { return this._items; }
            set { this._items = value; }
        }
    }

    public class TItem {
        private string _name = "";
        public TItem() { }
        [XmlAttribute("name")]
        public string Name {
            get { return this._name; }
            set { this._name = value; }
        }
    }
}
