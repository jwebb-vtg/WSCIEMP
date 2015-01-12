using System;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Data;

namespace WSCData {
    /// <summary>
    /// Summary description for RptAgronomy.
    /// </summary>
    [XmlRootAttribute(ElementName = "RptAgronomy", IsNullable = false)]
    public class RptAgronomy {

        private const string MOD_NAME = "RptAgronomy.";

//        string _fileName = "";
        TItem2List _varietyList = null;
        TItem2List _factoryVarietyList = null;
        TItemList _seedTreatmentList = null;
        TItemList _laybyHerbicideList = null;
        TItemList _bandWidthList = null;
        TItemList _sampleDepthList = null;
        TItemList _samplePList = null;
        TItemList _plantSpacingList = null;
        TItemList _soilTextureList = null;
        TItemList _soilTestList = null;
        TItemList _rowSpacingList = null;
        TItemList _seedList = null;
        TItemList _weedControlMethodList = null;
        TItemList _cercosporaChemList = null;
        TItemList _hailStressList = null;
        TItemList _weedControlList = null;
        TItemList _replantReasonList = null;
        TItemList _lostReasonList = null;
        TItemList _herbicideRxCountList = null;

        public RptAgronomy() {
            _varietyList = new TItem2List();
            _factoryVarietyList = new TItem2List();
            _seedTreatmentList = new TItemList();
            _laybyHerbicideList = new TItemList();
            _bandWidthList = new TItemList();
            _sampleDepthList = new TItemList();
            _samplePList = new TItemList();
            _plantSpacingList = new TItemList();
            _soilTextureList = new TItemList();
            _soilTestList = new TItemList();
            _rowSpacingList = new TItemList();
            _seedList = new TItemList();
            _weedControlMethodList = new TItemList();
            _cercosporaChemList = new TItemList();
            _hailStressList = new TItemList();
            _weedControlList = new TItemList();
            _replantReasonList = new TItemList();
            _lostReasonList = new TItemList();
            _herbicideRxCountList = new TItemList();
        }

		public void Load() {

			//            _fileName = fileName;
			//XmlSerializer serializer = new XmlSerializer(typeof(RptAgronomy));

			// A FileStream is needed to read the XML document.            
			// Declare an object variable of the type to be deserialized.
			RptAgronomy rptAgronomy = new RptAgronomy();

			/* Use the Deserialize method to restore the object's state with
			data from the XML document. */

			//using (FileStream fs = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)) {
			//    rptAgronomy = (RptAgronomy)serializer.Deserialize(fs);
			//}

			//this._varietyList = rptAgronomy.VarietyList;
			//this._factoryVarietyList = rptAgronomy.FactoryVarietyList;

			GetFactoryAreaList();
			GetSeedVarietyList();
			GetSeedListList();
            GetSeedTreatmentList();
            GetLaybyHerbicideList();
            GetBandWidthList();
            GetSampleDepthList();
            GetSamplePList();
            GetPlantSpacingList();
            GetRowSpacingList();
            GetSoilTextureList();
            GetSoilSampleList();
            GetCercosporaChemList();
            GetHailStressList();
            GetWeedControlList();
            GetReplantReasonList();
            GetLostReasonList();
            GetHerbicideRxCountList();

            }

        [XmlElement("varietyList")]
        public TItem2List VarietyList {
            get { return this._varietyList; }
            set { this._varietyList = value; }
        }

        [XmlElement("factoryVarietyList")]
        public TItem2List FactoryVarietyList {
            get { return this._factoryVarietyList; }
            set { this._factoryVarietyList = value; }
        }

        [XmlElement("seedList")]
        public TItemList SeedList {
            get { return this._seedList; }
            set { this._seedList = value; }
        }

        [XmlElement("seedTreatmentList")]
        public TItemList SeedTreatmentList {
            get { return this._seedTreatmentList; }
            set { this._seedTreatmentList = value; }
        }

        [XmlElement("laybyHerbicideList")]
        public TItemList LaybyHerbicideList {
            get { return this._laybyHerbicideList; }
            set { this._laybyHerbicideList = value; }
        }

        [XmlElement("bandWidthList")]
        public TItemList BandWidthList {
            get { return this._bandWidthList; }
            set { this._bandWidthList = value; }
        }

        [XmlElement("sampleDepthList")]
        public TItemList SampleDepthList {
            get { return this._sampleDepthList; }
            set { this._sampleDepthList = value; }
        }

        [XmlElement("samplePList")]
        public TItemList SamplePList {
            get { return this._samplePList; }
            set { this._samplePList = value; }
        }

        [XmlElement("plantSpacingList")]
        public TItemList PlantSpacingList {
            get { return this._plantSpacingList; }
            set { this._plantSpacingList = value; }
        }

        [XmlElement("rowSpacingList")]
        public TItemList RowSpacingList {
            get { return this._rowSpacingList; }
            set { this._rowSpacingList = value; }
        }

        [XmlElement("soilTextureList")]
        public TItemList SoilTextureList {
            get { return this._soilTextureList; }
            set { this._soilTextureList = value; }
        }

        [XmlElement("soilTestList")]
        public TItemList SoilTestList {
            get { return this._soilTestList; }
            set { this._soilTestList = value; }
        }

        [XmlElement("weedControlMethodList")]
        public TItemList WeedControlMethodList {
            get { return this._weedControlMethodList; }
            set { this._weedControlMethodList = value; }
        }

        [XmlElement("cercosporaChemList")]
        public TItemList CercosporaChemList {
            get { return this._cercosporaChemList; }
            set { this._cercosporaChemList = value; }
        }

        [XmlElement("hailStressList")]
        public TItemList HailStressList {
            get { return this._hailStressList; }
            set { this._hailStressList = value; }
        }

        [XmlElement("weedControlList")]
        public TItemList WeedControlList {
            get { return this._weedControlList; }
            set { this._weedControlList = value; }
        }

        [XmlElement("replantReasonList")]
        public TItemList ReplantReasonList {
            get { return this._replantReasonList; }
            set { this._replantReasonList = value; }
        }

        [XmlElement("lostReasonList")]
        public TItemList LostReasonList {
            get { return this._lostReasonList; }
            set { this._lostReasonList = value; }
        }

        [XmlElement("herbicideRxCountList")]
        public TItemList HerbicideRxCountList {
            get { return this._herbicideRxCountList; }
            set { this._herbicideRxCountList = value; }
        }

		private void GetSeedVarietyList() {

			try {

				DataTable dt = WSCField.SeedVarietyGetByArea("");
				foreach (DataRow row in dt.Rows) {
					this._varietyList.AddItem(row["svtyFactoryArea"].ToString(), row["svtyVariety"].ToString());
				}
			}
			catch (Exception ex) {
				WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetSeedVarietyList", ex);
				throw (wex);
			}
		}

		private void GetFactoryAreaList() {

			try {

				DataTable dt = WSCField.FactoryAreaGetAll();
				foreach (DataRow row in dt.Rows) {
					this._factoryVarietyList.AddItem(row["factoryNumber"].ToString(), row["factoryArea"].ToString());
				}
			}
			catch (Exception ex) {
				WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetFactoryAreaList", ex);
				throw (wex);
			}
		}

        private void GetSeedListList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "SEEDPRIME")) {
                        while (dr.Read()) {
                            this._seedList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetSeedListList", ex);
                throw (wex);
            }
        }

        private void GetSeedTreatmentList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "SEED_TREATMENT")) {

                        while (dr.Read()) {
                            this._seedTreatmentList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetSeedTreatmentList", ex);
                throw (wex);
            }
        }

        private void GetLaybyHerbicideList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "LAYBY_HERBICIDE")) {

                        while (dr.Read()) {
                            this._laybyHerbicideList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetLaybyHerbicideList", ex);
                throw (wex);
            }
        }

        private void GetBandWidthList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "BAND_WIDTH")) {

                        this._bandWidthList.AddItem("0");
                        while (dr.Read()) {
                            this._bandWidthList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetBandWidthList", ex);
                throw (wex);
            }
        }

        private void GetSampleDepthList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "SAMPLE_DEPTH")) {

                        this._sampleDepthList.AddItem("0");
                        while (dr.Read()) {
                            this._sampleDepthList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetSampleDepthList", ex);
                throw (wex);
            }
        }

        private void GetSamplePList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "SAMPLE_P_PPM")) {

                        while (dr.Read()) {
                            this._samplePList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetSamplePList", ex);
                throw (wex);
            }
        }

        private void GetPlantSpacingList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "PLANT_SPACING")) {

                        this._plantSpacingList.AddItem("0");
                        while (dr.Read()) {
                            this._plantSpacingList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetPlantSpacingList", ex);
                throw (wex);
            }
        }

        private void GetRowSpacingList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "ROW_SPACING")) {

                        this._rowSpacingList.AddItem("0");
                        while (dr.Read()) {
                            this._rowSpacingList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetRowSpacingList", ex);
                throw (wex);
            }
        }

        private void GetSoilTextureList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "SOIL_TEXTURE")) {

                        while (dr.Read()) {
                            this._soilTextureList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetSoilTextureList", ex);
                throw (wex);
            }
        }

        private void GetSoilSampleList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "SOIL_TEST")) {

                        while (dr.Read()) {
                            this._soilTestList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetSoilSampleList", ex);
                throw (wex);
            }
        }

        private void GetCercosporaChemList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "CERCOSPORA_CHEM")) {

                        while (dr.Read()) {
                            this._cercosporaChemList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetCercosporaChemList", ex);
                throw (wex);
            }
        }

        private void GetHailStressList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "HAIL_STRESS")) {

                        while (dr.Read()) {
                            this._hailStressList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetHailStressList", ex);
                throw (wex);
            }
        }

        private void GetHerbicideRxCountList() {

            try {
                for (int i = 0; i <= 5; i++) {
                    this._herbicideRxCountList.AddItem(i.ToString());
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetHerbicideRxCountList", ex);
                throw (wex);
            }
        }

        private void GetWeedControlList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "WEED_CONTROL")) {

                        while (dr.Read()) {
                            this._weedControlList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetWeedControlList", ex);
                throw (wex);
            }
        }

        private void GetReplantReasonList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "REPLANT_REASON")) {

                        while (dr.Read()) {
                            this._replantReasonList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetReplantReasonList", ex);
                throw (wex);
            }
        }

        private void GetLostReasonList() {

            try {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BeetConn"].ToString())) {
                    using (SqlDataReader dr = WSCField.GetLookupList(conn, "LOST_REASON")) {

                        while (dr.Read()) {
                            this._lostReasonList.AddItem(dr.GetString(dr.GetOrdinal("lkp_description")));
                        }
                    }
                }
            }
            catch (Exception ex) {
                WSCIEMP.Common.CException wex = new WSCIEMP.Common.CException(MOD_NAME + "GetLostReasonList", ex);
                throw (wex);
            }
        }
    }
}
