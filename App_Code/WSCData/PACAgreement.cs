using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WSCData {

    [Serializable]
    public class PACAgreement : ISerializable {

        public PACAgreement() { }

        public PACAgreement(string shid, double contribution, string pacDate, int crop_year, List<Individual> individuals)
        {
            SHID = shid;
            Contribution = contribution;
            PACDate = pacDate;
            PACCropYear = crop_year;
            Individuals = individuals;
        }

        public PACAgreement(string shid, double contribution, string pacDate, int crop_year, string individuals)
        {
            SHID = shid;
            Contribution = contribution;
            PACDate = pacDate;
            PACCropYear = crop_year;
            Individuals = parseIndString(individuals);
        }

        private List<Individual> parseIndString(string indiviString)
        {
            List<Individual> results = new List<Individual>();

            if (indiviString != null && indiviString.Length > 1)
            {
                string[] lines = indiviString.Split('~');
                for (int l = 0; l < lines.Length; l++) {
                    if (lines[l] != null && lines[l].Length > 1)
                    {
                        string[] items = lines[l].Split('|');
                        int iID = Convert.ToInt16(items[0]);
                        char sort = Convert.ToChar(items[1]);
                        double p = Convert.ToDouble(items[2]);
                        Boolean s = ("true" == items[3]);
                        if (s && items.Length > 4 && items[4].Length > 1)
                        {
                            results.Add(new Individual(iID, "", p, s, Convert.ToDateTime(items[4]), sort));
                        } else {
                            results.Add(new Individual(iID, p, false, sort));
                        }
                    }
                }
            }

            return results;
        }

        public string IndividualsString
        {
            get
            {
                string result = "";
                for (int i = 0; i < Individuals.Count; i++)
                {
                    result += Convert.ToString((Individual)Individuals[i]) + "~";
                }
                return result;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SHID", SHID, typeof(string));
            info.AddValue("Contribution", Contribution, typeof(double));
            info.AddValue("PACDate", PACDate, typeof(string));
            info.AddValue("PACCropYear", PACCropYear, typeof(int));
            info.AddValue("Individuals", Individuals, typeof(List<Individual>));
        }

        public PACAgreement(SerializationInfo info, StreamingContext context)
        {
            SHID = (string)info.GetValue("SHID", typeof(string));
            Contribution = (double)info.GetValue("Contribution", typeof(double));
            PACDate = (string)info.GetValue("PACDate", typeof(string));
            PACCropYear = (int)info.GetValue("PACCropYear", typeof(int));
            Individuals = (List<Individual>)info.GetValue("Individuals", typeof(List<Individual>));
        }

        public string SHID { get; set; }
        public double Contribution { get; set; }
        public string PACDate { get; set; }
        public int PACCropYear { get; set; }
        public List<Individual> Individuals { get; set; }
    }
}