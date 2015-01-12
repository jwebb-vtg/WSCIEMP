using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Xml;

namespace PdfHelper {

    /// <summary>
    /// Summary description for Xml2PDF.
    /// </summary>
    public class Xml2PDF {

        private const string MOD_NAME = "PdfHelper.Xml2PDF.";

        public delegate Phrase CustomNodeHandler(XmlNode xmlCustom, ref string text);
        private CustomNodeHandler _customNodeHandler;
        private System.Web.UI.Page _page = null;
        private Font _defaultFont = null;
        private float _defaultLeading = 15;
        private float _marginTop = 0;
        private float _marginRight = 0;
        private float _marginBottom = 0;
        private float _marginLeft = 0;
        private XmlElement _docElem = null;

        public Xml2PDF(XmlDocument xmlDoc) {

            const string METHOD_NAME = "Xml2PDF";

            try {
                _docElem = xmlDoc.DocumentElement;
                _defaultFont = GetNodeFont(_docElem);
                _defaultLeading = GetLeading(_docElem);
                SetMargins(_docElem);
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public Xml2PDF(CustomNodeHandler customHandler, XmlDocument xmlDoc) {

            const string METHOD_NAME = "Xml2PDF";

            try {
                _docElem = xmlDoc.DocumentElement;
                _defaultFont = GetNodeFont(_docElem);
                _defaultLeading = GetLeading(_docElem);
                _customNodeHandler = customHandler;
                SetMargins(_docElem);
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public void Convert(Document pdfDoc, System.Web.UI.Page page) {

            const string METHOD_NAME = "Convert";

            try {
                _page = page;

                float width = pdfDoc.PageSize.Width;
                float marginLeft = pdfDoc.LeftMargin;
                float marginRight = pdfDoc.RightMargin;

                foreach (XmlNode node in _docElem.ChildNodes) {

                    Paragraph para = null;

                    switch (node.Name) {

                        case "addressBlock":
                            para = GetAddressBlock(node);
                            pdfDoc.Add(para);
                            break;

                        case "image":
                            pdfDoc.Add(GetImage(node));
                            break;

                        case "newLine":
                            Phrase phrase = new Phrase("\n");
                            pdfDoc.Add(phrase);
                            break;

                        case "paragraph":
                            para = GetParagraph(node);
                            pdfDoc.Add(para);
                            break;

                        case "table":
                            pdfDoc.Add(GetTable(node));
                            break;
                    }
                }
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private void SetMargins(XmlNode node) {

            const string METHOD_NAME = "SetMargins";

            try {

                _marginTop = 36F;
                _marginRight = 36F;
                _marginBottom = 36F;
                _marginLeft = 36F;

                string margins = "36 36 36 36";
                if (node.Attributes["margins"] != null) {
                    margins = node.Attributes["margins"].Value;
                }

                string[] marginValues = margins.Split(new char[] { ' ' });
                for (int i = 0; i < marginValues.Length; i++) {
                    switch (i) {

                        case 1:
                            _marginTop = float.Parse(marginValues[i]);
                            break;
                        case 2:
                            _marginRight = float.Parse(marginValues[i]);
                            break;
                        case 3:
                            _marginBottom = float.Parse(marginValues[i]);
                            break;
                        case 4:
                            _marginLeft = float.Parse(marginValues[i]);
                            break;
                    }
                }
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public float MarginTop {
            get { return _marginTop; }
            set { _marginTop = value; }
        }

        public float MarginRight {
            get { return _marginRight; }
            set { _marginRight = value; }
        }

        public float MarginBottom {
            get { return _marginBottom; }
            set { _marginBottom = value; }
        }

        public float MarginLeft {
            get { return _marginLeft; }
            set { _marginLeft = value; }
        }

        private iTextSharp.text.Image GetImage(XmlNode xmlImg) {

            const string METHOD_NAME = "GetImage";

            try {

                string url = _page.MapPath(xmlImg.Attributes["url"].Value);
                string width = xmlImg.Attributes["width"].Value;
                string height = xmlImg.Attributes["height"].Value;
                int actualWidth = (width.Length > 0 ? Int32.Parse(width) : 0);
                int actualHeight = (height.Length > 0 ? Int32.Parse(height) : 0);

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(url);
                jpg.ScaleAbsolute(actualWidth, actualHeight);
                jpg.Alignment = GetHAlignment(xmlImg);
                return jpg;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private PdfPTable GetTable(XmlNode xmlTable) {

            const string METHOD_NAME = "GetTable";

            try {
                XmlNodeList trs = xmlTable.SelectNodes("tr");
                PdfPTable table = null;

                for (int i = 0; i < trs.Count; i++) {

                    if (i == 0) {
                        XmlNodeList ths = trs[i].SelectNodes("th");
                        float[] columns = new float[ths.Count];

                        for (int j = 0; j < ths.Count; j++) {
                            XmlNode node = ths.Item(j);
                            columns[j] = float.Parse(node.Attributes["width"].Value);
                        }
                        table = PdfReports.CreateTable(columns, 0);

                    } else {

                        XmlNodeList tds = trs[i].SelectNodes("td");
                        foreach (XmlNode td in tds) {

                            iTextSharp.text.pdf.PdfPCell cell = null;
                            int align = GetHAlignment(td);

                            if (td.ChildNodes.Count == 0) {
                                table.AddCell(" ");
                            } else {

                                foreach (XmlNode node in td.ChildNodes) {

                                    Paragraph para = null;
                                    Phrase phrase = null;
                                    string text = null;

                                    switch (node.Name) {

                                        case "custom":
                                            phrase = GetCustomTag(node, ref text);
                                            cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                                            cell.HorizontalAlignment = align;
                                            cell.Border = 0;
                                            table.AddCell(cell);
                                            break;

                                        case "addressBlock":
                                            para = GetAddressBlock(node);
                                            cell = new iTextSharp.text.pdf.PdfPCell(para);
                                            cell.HorizontalAlignment = align;
                                            cell.Border = 0;
                                            table.AddCell(cell);
                                            break;

                                        case "paragraph":
                                            para = GetParagraph(node);
                                            cell = new iTextSharp.text.pdf.PdfPCell(para);
                                            cell.HorizontalAlignment = align;
                                            cell.Border = 0;
                                            table.AddCell(cell);
                                            break;

                                        case "image":
                                            iTextSharp.text.Image img = GetImage(node);
                                            cell = new iTextSharp.text.pdf.PdfPCell(img);
                                            cell.HorizontalAlignment = align;
                                            cell.Border = 0;
                                            table.AddCell(cell);
                                            break;

                                        case "phrase":
                                            phrase = GetPhrase(node);
                                            cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                                            cell.HorizontalAlignment = align;
                                            cell.Border = 0;
                                            table.AddCell(cell);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }

                return table;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private Paragraph GetAddressBlock(XmlNode xmlAddr) {

            const string METHOD_NAME = "GetAddressBlock";

            try {
                Phrase phrase = null;
                Font paraFont = GetNodeFont(xmlAddr);
                if (paraFont == null) {
                    paraFont = _defaultFont;
                }
                Paragraph para = new Paragraph("", paraFont);
                para.IndentationLeft = GetIndentLeft(xmlAddr);
                para.Leading = GetLeading(xmlAddr);
                para.Alignment = GetHAlignment(xmlAddr);
                string text = "";

                foreach (XmlNode node in xmlAddr.ChildNodes) {

                    switch (node.Name) {

                        case "phrase":
                            phrase = GetPhrase(node);
                            para.Add(phrase);
                            break;

                        case "custom":
                            phrase = GetCustomTag(node, ref text);
                            if (text.Length > 0) {
                                phrase.Add("\n");
                                para.Add(phrase);
                            }
                            break;

                    }
                }
                return para;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private Paragraph GetParagraph(XmlNode xmlPara) {

            const string METHOD_NAME = "GetParagraph";

            try {
                Phrase phrase = null;
                Font paraFont = GetNodeFont(xmlPara);
                if (paraFont == null) {
                    paraFont = _defaultFont;
                }
                Paragraph para = new Paragraph("", paraFont);
                para.IndentationLeft = GetIndentLeft(xmlPara);
                para.Leading = GetLeading(xmlPara);
                para.Alignment = GetHAlignment(xmlPara);

                foreach (XmlNode node in xmlPara.ChildNodes) {

                    if (node.NodeType == System.Xml.XmlNodeType.Text) {
                        phrase = new Phrase(node.Value);
                        para.Add(phrase);
                    } else {

                        switch (node.Name) {

                            case "phrase":
                                phrase = GetPhrase(node);
                                para.Add(phrase);
                                break;

                            case "newLine":
                                phrase = new Phrase("\n");
                                para.Add(phrase);
                                break;

                            default:
                                string text = "";
                                phrase = GetCustomTag(node, ref text);
                                para.Add(phrase);
                                break;
                        }
                    }
                }
                return para;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private Phrase GetCustomTag(XmlNode xmlCustom, ref string text) {

            const string METHOD_NAME = "GetCustomTag";

            try {
                Phrase p = null;
                Font font = GetNodeFont(xmlCustom);
                if (font == null) { font = _defaultFont; }
                p = _customNodeHandler(xmlCustom, ref text);

                return p;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private Phrase GetPhrase(XmlNode xmlPhrase) {

            const string METHOD_NAME = "GetPhrase";

            try {
                Phrase p = null;
                string s = xmlPhrase.InnerText;
                Font font = GetNodeFont(xmlPhrase);

                if (font != null) {
                    p = new Phrase(s, font);
                } else {
                    p = new Phrase(s);
                }

                return p;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private float GetIndentLeft(XmlNode node) {

            const string METHOD_NAME = "GetIndentLeft";

            try {

                float indentLeft = 0F;
                if (node.Attributes["identLeft"] != null) {
                    indentLeft = float.Parse(node.Attributes["identLeft"].Value);
                }

                return indentLeft;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }

        }

        private int GetHAlignment(XmlNode node) {

            const string METHOD_NAME = "GetHAlignment";

            try {
                int alignment = Element.ALIGN_LEFT;
                string hAlign = "";

                if (node.Attributes["hAlign"] != null) {
                    hAlign = node.Attributes["hAlign"].Value;

                    switch (hAlign.ToUpper()) {
                        case "RIGHT":
                            alignment = Element.ALIGN_RIGHT;
                            break;
                        case "LEFT":
                            alignment = Element.ALIGN_LEFT;
                            break;
                        case "CENTER":
                            alignment = Element.ALIGN_CENTER;
                            break;
                    }
                }

                return alignment;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private float GetLeading(XmlNode node) {

            const string METHOD_NAME = "GetLeading";

            try {
                float leading = _defaultLeading;
                if (node.Attributes["leading"] != null) {
                    leading = float.Parse(node.Attributes["leading"].Value);
                }

                return leading;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private Font GetNodeFont(XmlNode node) {

            const string METHOD_NAME = "GetNodeFont";

            try {

                Font font = null;
                string fontFamily = "";
                if (node.Attributes["font-family"] != null) {
                    fontFamily = node.Attributes["font-family"].Value;
                }
                string fontSize = "0";
                if (node.Attributes["font-size"] != null) {
                    fontSize = node.Attributes["font-size"].Value;
                }

                string fontStyle = "NORMAL";
                if (node.Attributes["font-style"] != null) {
                    fontStyle = node.Attributes["font-style"].Value;
                }

                if (fontSize != "0" && fontFamily != "") {
                    font = PdfReports.GetFont(fontFamily, fontSize, fontStyle);
                }

                return font;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        private Font GetNodeFont(XmlElement elem) {

            const string METHOD_NAME = "GetNodeFont";

            try {
                Font font = null;
                string fontFamily = "";
                if (elem.Attributes["font-family"] != null) {
                    fontFamily = elem.Attributes["font-family"].Value;
                }
                string fontSize = "0";
                if (elem.Attributes["font-size"] != null) {
                    fontSize = elem.Attributes["font-size"].Value;
                }

                string fontStyle = "NORMAL";
                if (elem.Attributes["font-style"] != null) {
                    fontStyle = elem.Attributes["font-style"].Value;
                }

                if (fontSize != "0" && fontFamily != "") {
                    font = PdfReports.GetFont(fontFamily, fontSize, fontStyle);
                }

                return font;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }
    }
}
