using System;
using System.Runtime.Serialization;

namespace WSCData {

    [Serializable]
    public class Contract : ISerializable {

        public Contract() {}

        public Contract(int ContractId, int CropYear, int ContractNo, int FactoryID, Double PacDues)
        {
            contractId = ContractId;
            cropYear = CropYear;
            contractNo = ContractNo;
            factoryID = FactoryID;
            pacDues = PacDues;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("contractId", contractId, typeof(int));
            info.AddValue("cropYear", cropYear, typeof(int));
            info.AddValue("contractNo", contractNo, typeof(int));
            info.AddValue("factoryID", factoryID, typeof(int));
            info.AddValue("pacDues", pacDues, typeof(double));
        }

        public Contract(SerializationInfo info, StreamingContext context)
        {
            contractId = (int)info.GetValue("contractId", typeof(int));
            cropYear = (int)info.GetValue("cropYear", typeof(int));
            contractNo = (int)info.GetValue("contractNo", typeof(int));
            factoryID = (int)info.GetValue("factoryID", typeof(int));
            pacDues = (double)info.GetValue("pacDues", typeof(double));
        }

        public int contractId { get; set; }
        public int cropYear { get; set; }
        public int contractNo { get; set; }
        public int factoryID { get; set; }
        public Double pacDues { get; set; }
    }
}