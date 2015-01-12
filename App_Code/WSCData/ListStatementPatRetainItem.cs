using System;

namespace WSCData {

    public class ListStatementPatRetainItem {

        public ListStatementPatRetainItem() { }

        //=================================
        // Patronage constructor
        //=================================
        public ListStatementPatRetainItem(int equityCropYear, string refundDate, string qualified, int memberID, decimal ratePerTon,
            string shid, string busName, string addr1, string addr2, string csz, string deductionDesc, decimal deductionAmt,
            decimal equityTons, decimal equityAmt, decimal patCertPct, decimal patInitPayPct, string patInitPayDate, decimal patInitPayment) {

            // member assignments
            EquityCropYear = equityCropYear.ToString();
            RefundDate = refundDate;
            Qualified = qualified;
            MemberID = memberID.ToString();
            RatePerTon = ratePerTon.ToString("0.00");
            SHID = shid;
            BusName = busName;
            Addr1 = addr1;
            Addr2 = addr2;
            CSZ = csz;
            DeductionDesc = deductionDesc;
            DeductionAmt = deductionAmt.ToString();
            EquityTons = equityTons.ToString("0.0000");
            EquityAmt = equityAmt.ToString("0.00");

            PatCertPct = patCertPct.ToString("0.0");
            PatInitPayPct = patInitPayPct.ToString("0.00");
            PatInitPayDate = patInitPayDate;
            PatInitPayment = patInitPayment.ToString("0.00");
        }

        //=================================
        // Patronage Redeem constructor
        //=================================
        public ListStatementPatRetainItem(int equityCropYear, string refundDate, string qualified, int memberID, decimal ratePerTon,
            string shid, string busName, string addr1, string addr2, string csz, string deductionDesc, decimal deductionAmt,
            decimal equityTons, decimal equityAmt, decimal redeemPct, decimal redeemAmt, decimal patInitPayment) {

            // member assignments
            EquityCropYear = equityCropYear.ToString();
            RefundDate = refundDate;
            Qualified = qualified;
            MemberID = memberID.ToString();
            RatePerTon = ratePerTon.ToString("0.00");
            SHID = shid;
            BusName = busName;
            Addr1 = addr1;
            Addr2 = addr2;
            CSZ = csz;
            DeductionDesc = deductionDesc;
            DeductionAmt = deductionAmt.ToString();
            EquityTons = equityTons.ToString("0.0000");
            EquityAmt = equityAmt.ToString("0.00");

            RedeemPct = redeemPct.ToString("0.00");
            RedeemAmt = redeemAmt.ToString("0.00");
            PatInitPayment = patInitPayment.ToString("0.00");
        }

        //=================================
        // Retain Redeem constructor
        //=================================
        public ListStatementPatRetainItem(int equityCropYear, string refundDate, string qualified, int memberID, decimal ratePerTon,
            string shid, string busName, string addr1, string addr2, string csz, string deductionDesc, decimal deductionAmt,
            decimal equityTons, decimal equityAmt, decimal redeemPct, decimal redeemAmt) {

            // member assignments
            EquityCropYear = equityCropYear.ToString();
            RefundDate = refundDate;
            Qualified = qualified;
            MemberID = memberID.ToString();
            RatePerTon = ratePerTon.ToString("0.00");
            SHID = shid;
            BusName = busName;
            Addr1 = addr1;
            Addr2 = addr2;
            CSZ = csz;
            DeductionDesc = deductionDesc;
            DeductionAmt = deductionAmt.ToString();
            EquityTons = equityTons.ToString("0.0000");
            EquityAmt = equityAmt.ToString("0.00");

            RedeemPct = redeemPct.ToString("0.00");
            RedeemAmt = redeemAmt.ToString("0.00");
        }

        // Common private properties
        private string _equityCropYear = "";
        private string _refundDate = "";
        private string _qualified = "";
        private string _memberID = "";
        private string _ratePerTon = "";
        private string _shid = "";
        private string _busName = "";
        private string _addr1 = "";
        private string _addr2 = "";
        private string _csz = "";
        private string _deductionDesc = "";
        private string _deductionAmt = "";
        private string _equityTons = "";
        private string _equityAmt = "";

        // Specific private properties
        private string _patCertPct = "";
        private string _patInitPayDate = "";
        private string _patInitPayment = "";
        private string _patInitPayPct = "";
        private string _redeemAmt = "";
        private string _redeemPct = "";

        public string EquityCropYear { 
            get { return _equityCropYear; } 
            set { _equityCropYear = value; } 
        }
        public string RefundDate { 
            get { return _refundDate; } 
            set { _refundDate = value; } 
        }
        public string Qualified { 
            get { return _qualified; } 
            set { _qualified = value; } 
        }
        public string MemberID { 
            get { return _memberID; } 
            set { _memberID = value; } 
        }
        public string RatePerTon { 
            get { return _ratePerTon; } 
            set { _ratePerTon = value; } }
        public string SHID { 
            get { return _shid; } 
            set { _shid = value; } 
        }
        public string BusName { 
            get { return _busName; } 
            set { _busName = value; } 
        }
        public string Addr1 { 
            get { return _addr1; } 
            set { _addr1 = value; } 
        }
        public string Addr2 { 
            get { return _addr2; } 
            set { _addr2 = value; } 
        }
        public string CSZ { 
            get { return _csz; } 
            set { _csz = value; } 
        }
        public string DeductionDesc { 
            get { return _deductionDesc; } 
            set { _deductionDesc = value; } 
        }
        public string DeductionAmt { 
            get { return _deductionAmt; } 
            set { _deductionAmt = value; } 
        }
        public string EquityTons { 
            get { return _equityTons; } 
            set { _equityTons = value; } 
        }
        public string EquityAmt { 
            get { return _equityAmt; } 
            set { _equityAmt = value; } 
        }

        public string PatCertPct { 
            get { return _patCertPct; } 
            set { _patCertPct = value; } 
        }
        public string PatInitPayDate { 
            get { return _patInitPayDate; } 
            set { _patInitPayDate = value; } 
        }
        public string PatInitPayment { 
            get { return _patInitPayment; } 
            set { _patInitPayment = value; } 
        }
        public string PatInitPayPct { 
            get { return _patInitPayPct; } 
            set { _patInitPayPct = value; } 
        }
        public string RedeemAmt { 
            get { return _redeemAmt; } 
            set { _redeemAmt = value; } 
        }
        public string RedeemPct { 
            get { return _redeemPct; } 
            set { _redeemPct = value; } 
        }
    }
}
