using System;

namespace WSCData {

    public class ListOverPlantFactoryItem {

        public ListOverPlantFactoryItem() { }

        public ListOverPlantFactoryItem(string ftyNumber, string ftyName, bool isOverPlantAllowed, decimal percentage, bool isPoolingAllowed, string poolShid, 
            string cutoffDate, bool isPosted) {

            FactoryNum = ftyNumber;
            FactoryName = ftyNumber + " - " + ftyName;
            IsOverPlantAllowed = (isOverPlantAllowed? "Yes": "No");
            Percentage = percentage.ToString("##0.0##");
            IsPoolingAllowed = (isPoolingAllowed? "Yes": "No");
            PoolSHID = poolShid;
            CutoffDate = cutoffDate;
            IsPosted = (isPosted ? "Yes" : "No"); 
        }

        private string _factoryNum = "";
        public string FactoryNum {
            get { return _factoryNum; }
            set { _factoryNum = value; }
        }

        private string _factoryName = "";
        public string FactoryName {
            get { return _factoryName; }
            set { _factoryName = value; }
        }

        private string _isOverPlantAllowed = "";
        public string IsOverPlantAllowed {
            get { return _isOverPlantAllowed; }
            set { _isOverPlantAllowed = value; }
        }

        private string _percentage = "";
        public string Percentage {
            get { return _percentage; }
            set { _percentage = value; }
        }

        private string _isPoolingAllowed = "";
        public string IsPoolingAllowed {
            get { return _isPoolingAllowed; }
            set { _isPoolingAllowed = value; }
        }

        private string _poolSHID = "";
        public string PoolSHID {
            get { return _poolSHID; }
            set { _poolSHID = value; }
        }

        private string _cutoffDate = "";
        public string CutoffDate {
            get { return _cutoffDate; }
            set { _cutoffDate = value; }
        }

        private string _isPosted = "";
        public string IsPosted {
            get { return _isPosted; }
            set { _isPosted = value; }
        }
    }
}
