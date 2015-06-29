using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using WSCData;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.Globalization;

namespace WSCIEMP.PDF
{
    public partial class Downloader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["q"] != null)
                LoadParams();
            else
                LoadPDF();
        }

        private void LoadPDF()
        {

            /*  
             * ContractNumber1
                ContractNumber2
                SumOfMoneyPerTon
                CropYear1
                WithdrawalDate
                CurrentDayMonth
                Shareholder Signature
                PrintLandOwnerName
                PrintShareholderName
                ShareholderAddress
                Director WSCPAC
                Company Representative
                CurrentTwoDigitYear
            */
            var pdfReader = new PdfReader(System.Web.HttpContext.Current.Server.MapPath("~/PDF/2015.pdf"));
            var output = new MemoryStream();
            var stamper = new PdfStamper(pdfReader, output);
            var date = DateTime.Now;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(date.Month).ToString();

            stamper.AcroFields.SetField("ContractNumber1", "");
            stamper.AcroFields.SetField("ContractNumber2", "");
            stamper.AcroFields.SetField("SumOfMoneyPerTon", "");
            stamper.AcroFields.SetField("CropYear1", "");
            stamper.AcroFields.SetField("WithdrawalDate", "");
            stamper.AcroFields.SetField("CurrentDayMonth", mfi.GetMonthName(date.Month).ToString() + " " + date.Day);
            stamper.AcroFields.SetField("Shareholder Signature", "");
            stamper.AcroFields.SetField("PrintLandOwnerName", "");
            stamper.AcroFields.SetField("PrintShareholderName", "");
            stamper.AcroFields.SetField("ShareholderAddress", "");
            stamper.AcroFields.SetField("Director WSCPAC", "");
            stamper.AcroFields.SetField("Company Representative", "");
            stamper.AcroFields.SetField("CurrentTwoDigitYear", date.ToString("yy"));

            stamper.FormFlattening = true;
            stamper.Close();
            pdfReader.Close();

            Response.AddHeader("Content-Disposition", "attachment; filename=YourPDF.pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(output.ToArray());
            Response.End();
        }

        private void LoadParams()
        {
            var pdfReader = new PdfReader(System.Web.HttpContext.Current.Server.MapPath("~/PDF/2015.pdf"));
            var output = new MemoryStream();
            var stamper = new PdfStamper(pdfReader, output);

            foreach (var f in stamper.AcroFields.Fields)
            {
                Response.Write(f.Key + "<br />");
            }
        }
    }
}