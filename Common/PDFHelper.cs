using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;

namespace WSCIEMP.Common
{
    public class PDFHelper
    {
        public static string[] GetFields(string pdfPath)
        {
            //var pdfReader = new PdfReader(pdfPath);
            // create and populate a string builder with each of the 
            // field names available in the subject PDF
            /*
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry de in pdfReader.AcroFields.Fields)
            {
                sb.Append(de.Key.ToString() + Environment.NewLine);
            }
            // Write the string builder's content to the form's textbox

            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = 0;
             * */
            return new string[] {"test","test"};
        }
    }
}