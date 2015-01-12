using System;

namespace WSCData {

    public class ListBeetFieldFinderItem {

        public ListBeetFieldFinderItem() { }

        public ListBeetFieldFinderItem(int lld_lld_id, string lld_state, string lld_county, string lld_township, string lld_range, string lld_section,
            string lld_quadrant, string lld_quarter_quadrant, decimal lld_latitude, decimal lld_longitude, string lld_description, string lld_field_name,
            int lld_acres, string lld_fsa_official, string lld_fsa_number, string lld_fsa_state, string lld_fsa_county, string lld_contract_no,
            string lld_farm_number, string lld_tract_number, string lld_field_number, string lld_quarter_field) {

            if (lld_state == "*") {
                Lld_fsa_number = "*";
            } else {
                Lld_lld_id = lld_lld_id.ToString();
                Lld_state = lld_state;
                Lld_county = lld_county;
                Lld_township = lld_township;
                Lld_range = lld_range;
                Lld_section = lld_section;
                Lld_quadrant = lld_quadrant;
                Lld_quarter_quadrant = lld_quarter_quadrant;
                Lld_latitude = lld_latitude.ToString("0.000000");
                Lld_longitude = lld_longitude.ToString("0.000000");
                Lld_description = lld_description;
                Lld_field_name = lld_field_name;
                Lld_acres = lld_acres.ToString("0");
                Lld_fsa_official = lld_fsa_official;
                Lld_fsa_number = lld_fsa_number;
                Lld_fsa_state = lld_fsa_state;
                Lld_fsa_county = lld_fsa_county;
                Lld_contract_no = lld_contract_no;
                Lld_farm_number = lld_farm_number;
                Lld_tract_number = lld_tract_number;
                Lld_field_number = lld_field_number;
                Lld_quarter_field = lld_quarter_field;
            }
        }

        private string _lld_lld_id = "";
        private string _lld_state = "";
        private string _lld_county = "";
        private string _lld_township = "";
        private string _lld_range = "";
        private string _lld_section = "";
        private string _lld_quadrant = "";
        private string _lld_quarter_quadrant = "";
        private string _lld_latitude = "";
        private string _lld_longitude = "";
        private string _lld_description = "";
        private string _lld_field_name = "";
        private string _lld_acres = "";
        private string _lld_fsa_official = "";
        private string _lld_fsa_number = "";
        private string _lld_fsa_state = "";
        private string _lld_fsa_county = "";
        private string _lld_contract_no = "";
        private string _lld_farm_number = "";
        private string _lld_tract_number = "";
        private string _lld_field_number = "";
        private string _lld_quarter_field = "";

        public string Lld_lld_id {
            get { return _lld_lld_id; }
            set { _lld_lld_id = value; }
        }
        public string Lld_state {
            get { return _lld_state; }
            set { _lld_state = value; }
        }
        public string Lld_county {
            get { return _lld_county; }
            set { _lld_county = value; }
        }
        public string Lld_township {
            get { return _lld_township; }
            set { _lld_township = value; }
        }
        public string Lld_range {
            get { return _lld_range; }
            set { _lld_range = value; }
        }
        public string Lld_section {
            get { return _lld_section; }
            set { _lld_section = value; }
        }
        public string Lld_quadrant {
            get { return _lld_quadrant; }
            set { _lld_quadrant = value; }
        }
        public string Lld_quarter_quadrant {
            get { return _lld_quarter_quadrant; }
            set { _lld_quarter_quadrant = value; }
        }
        public string Lld_latitude {
            get { return _lld_latitude; }
            set { _lld_latitude = value; }
        }
        public string Lld_longitude {
            get { return _lld_longitude; }
            set { _lld_longitude = value; }
        }
        public string Lld_description {
            get { return _lld_description; }
            set { _lld_description = value; }
        }
        public string Lld_field_name {
            get { return _lld_field_name; }
            set { _lld_field_name = value; }
        }
        public string Lld_acres {
            get { return _lld_acres; }
            set { _lld_acres = value; }
        }
        public string Lld_fsa_official {
            get { return _lld_fsa_official; }
            set { _lld_fsa_official = value; }
        }
        public string Lld_fsa_number {
            get { return _lld_fsa_number; }
            set { _lld_fsa_number = value; }
        }
        public string Lld_fsa_state {
            get { return _lld_fsa_state; }
            set { _lld_fsa_state = value; }
        }
        public string Lld_fsa_county {
            get { return _lld_fsa_county; }
            set { _lld_fsa_county = value; }
        }
        public string Lld_contract_no {
            get { return _lld_contract_no; }
            set { _lld_contract_no = value; }
        }
        public string Lld_farm_number {
            get { return _lld_farm_number; }
            set { _lld_farm_number = value; }
        }
        public string Lld_tract_number {
            get { return _lld_tract_number; }
            set { _lld_tract_number = value; }
        }
        public string Lld_field_number {
            get { return _lld_field_number; }
            set { _lld_field_number = value; }
        }
        public string Lld_quarter_field {
            get { return _lld_quarter_field; }
            set { _lld_quarter_field = value; }
        }
    }
}
