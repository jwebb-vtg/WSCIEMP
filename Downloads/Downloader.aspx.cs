using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WSCIEMP.Downloads
{
    public partial class Downloader : System.Web.UI.Page
    {

        private PdfStamper stamper;
        private string filename;

        protected void Page_Load(object sender, EventArgs e)
        {
            filename = "~/Downloads/" + Request.QueryString["Filename"] + ".pdf";

            var pdfReader = new PdfReader(System.Web.HttpContext.Current.Server.MapPath(filename));
            var output = new MemoryStream();
            stamper = new PdfStamper(pdfReader, output);

            OutputParams();
            Request.QueryString.AllKeys.ToList().ForEach(k => stamper.AcroFields.SetField(k, Request.QueryString[k]));

            stamper.FormFlattening = true;
            stamper.Close();
            pdfReader.Close();

            Response.AddHeader("Content-Disposition", "attachment; filename=" + Request.QueryString["Filename"].ToString() + "-" + DateTime.Now.ToString() + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(output.ToArray());
            Response.End();
        }

        private void OutputParams()
        {
            Debug.WriteLine("");
            Debug.WriteLine(filename);
            Debug.WriteLine("KEYS");

            stamper.AcroFields.Fields.ToList().ForEach(f => Debug.WriteLine(f.Key));
            Debug.WriteLine("QUERYSTRING");

            Request.QueryString.AllKeys.ToList().ForEach(k => Debug.WriteLine(k.ToString() + ": " + Request.QueryString[k].ToString()) );
            Debug.WriteLine("");
        }
    }
}