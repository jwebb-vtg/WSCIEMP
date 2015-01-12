using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using Color = iTextSharp.text.BaseColor;

namespace PdfHelper {

    /// <summary>
    /// Summary description for PdfReports.
    /// </summary>
    internal class PdfReports {

        private const string MOD_NAME = "PdfHelper.PdfReports.";

        public static void AddTableNoSplit(Document document, ICustomPageEvent pgEvent, PdfPTable table) {

            int goStatus = 0;
            float pos = 0;
            ColumnText ct = pgEvent.GetColumnObject();

            pos = ct.YLine;
            ct.AddElement(table);
            goStatus = ct.Go(true);

            if (ColumnText.HasMoreText(goStatus)) {
                document.NewPage();
                ct = pgEvent.GetColumnObject();
                ct.AddElement(table);
                ct.Go(false);
            } else {
                ct.AddElement(table);
                ct.YLine = pos;
                ct.Go(false);
            }
        }

        public static void FillHeaderLabels(ref PdfPTable table, string[] hdrNameList, iTextSharp.text.Font font) {
            foreach (string hdr in hdrNameList) {
                PdfReports.AddText2Table(table, hdr, font, "center");
            }
        }

        public static void AddValues2Paragraph(iTextSharp.text.Paragraph para, ArrayList values, Font font) {

            const string METHOD_NAME = "AddValues2Paragraph";
            try {

                iTextSharp.text.Phrase phrase = null;
                foreach (string s in values) {
                    if (s.Length > 0) {
                        if (phrase != null) {
                            phrase = new Phrase(@" \ " + s, font);
                        } else {
                            phrase = new Phrase(s, font);
                        }
                        para.Add(phrase);
                    }
                }
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw(aex);
            }
        }

        public static PdfPTable CreateTable(PdfPTable copyTable) {

            const string METHOD_NAME = "CreateTable";

            try {

                PdfPTable table = new PdfPTable(copyTable);

                table.DefaultCell.PaddingTop = 0;
                table.DefaultCell.PaddingBottom = 0;
                table.DefaultCell.PaddingLeft = 0;
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                return table;

            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }            
        }

        public static PdfPTable CreateTable(float[] columnWidths, int border) {

            const string METHOD_NAME = "CreateTable";

            try {

                float sumWidths = 0;
                foreach (float w in columnWidths) {
                    sumWidths += w;
                }

                PdfPTable table = new PdfPTable(columnWidths.Length);
                table.DefaultCell.Border = border;

                if (Math.Round(sumWidths, 0) != 100) {
                    table.SetTotalWidth(columnWidths);
                    table.LockedWidth = true;
                } else {
                    table.SetWidths(columnWidths);
                    table.WidthPercentage = 100;
                }
             
                return table;

            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }            
        }

        public static iTextSharp.text.Font GetFont(string fontFamily, string fontSize, string fontStyle) {

            const string METHOD_NAME = "GetFont";

            try {

                iTextSharp.text.Font font = null;
                if (fontStyle == "") { fontStyle = "NORMAL"; }

                font = iTextSharp.text.FontFactory.GetFont(fontFamily, float.Parse(fontSize),
                    iTextSharp.text.Font.GetStyleValue(fontStyle));

                if (font == null) {

                    int itextFontStyle = 0;
                    switch (fontStyle.ToUpper()) {

                        case "BOLD":
                            itextFontStyle = Font.BOLD;
                            break;

                        default:
                            itextFontStyle = Font.NORMAL;
                            break;
                    }

                    float itextFontSize = float.Parse(fontSize);

                    string itextFontFamily = "";


                    switch (fontFamily.ToUpper()) {

                        case "COURIER":
                            itextFontFamily = "COURIER";
                            break;
                        case "TIMES ROMAN":
                            itextFontFamily = "TIMES_ROMAN";
                            break;
                        default:
                            itextFontFamily = "HELVETICA";
                            break;
                    }

                    font = FontFactory.GetFont(itextFontFamily, itextFontSize, itextFontStyle);
                }

                return font;

            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }            
        }

        public static iTextSharp.text.Image GetImage(string url,
            int width, int height, int hAlign) {

            const string METHOD_NAME = "GetImage";

            try {
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(url);
                jpg.ScaleAbsolute(width, height);
                jpg.Alignment = hAlign;
                return jpg;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }            
        }

        public static Paragraph GetAddressBlock(string adrName,
            string adrLine1, string adrLine2, string csz, float leftIndent, float leading, int hAlign, Font font) {

            const string METHOD_NAME = "GetAddressBlock";

            try {

                Phrase phrase = null;
                Paragraph para = new Paragraph("", font);

                para.IndentationLeft = leftIndent;
                para.Leading = leading;
                para.Alignment = hAlign;

                if (adrName != null && adrName.Length > 0) {
                    phrase = new Phrase(adrName + "\n", font);
                    para.Add(phrase);
                }

                if (adrLine1 != null && adrLine1.Length > 0) {
                    phrase = new Phrase(adrLine1 + "\n", font);
                    para.Add(phrase);
                }

                if (adrLine2 != null && adrLine2.Length > 0) {
                    phrase = new Phrase(adrLine2 + "\n", font);
                    para.Add(phrase);
                }

                if (csz != null && csz.Length > 0) {
                    phrase = new Phrase(csz + "\n", font);
                    para.Add(phrase);
                }

                return para;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddImage2Table(PdfPTable table, iTextSharp.text.Image img) {

            const string METHOD_NAME = "AddImage2Table";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(img);
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddImage2Table(PdfPTable table, iTextSharp.text.Image img, string align) {

            const string METHOD_NAME = "AddImage2Table";

            try {

                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(img);
                cell.BorderWidth = 0;

                switch (align.ToLower()) {
                    case "left":
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        break;
                    case "center":
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        break;

                    case "right":
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        break;
                }

                table.AddCell(cell);
                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table,
            iTextSharp.text.Paragraph para) {

            const string METHOD_NAME = "AddText2Table";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(para);
                cell.BorderWidth = 0;
                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table,
            iTextSharp.text.Paragraph para, string align) {

            const string METHOD_NAME = "AddText2Table";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(para);
                cell.BorderWidth = 0;

                switch (align.ToLower()) {
                    case "left":
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        break;
                    case "center":
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        break;

                    case "right":
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        break;
                }

                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table,
            string text, iTextSharp.text.Font font, int colSpan) {

            const string METHOD_NAME = "AddText2Table";

            try {
                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;
                cell.Colspan = colSpan;
                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table,
            string text, iTextSharp.text.Font font, string align, int colSpan) {

            const string METHOD_NAME = "AddText2Table";

            try {
                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;
                cell.Colspan = colSpan;

                switch (align.ToLower()) {
                    case "left":
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        break;
                    case "center":
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        break;

                    case "right":
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        break;
                }

                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table,
            iTextSharp.text.Paragraph para, string align, int colSpan) {

            const string METHOD_NAME = "AddText2Table";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(para);
                cell.BorderWidth = 0;
                cell.Colspan = colSpan;

                switch (align.ToLower()) {
                    case "left":
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        break;
                    case "center":
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        break;

                    case "right":
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        break;
                }

                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table, iTextSharp.text.Paragraph para, int colSpan) {

            const string METHOD_NAME = "AddText2Table";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(para);
                cell.BorderWidth = 0;
                cell.Colspan = colSpan;

                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table,
            iTextSharp.text.Phrase phrase) {

            const string METHOD_NAME = "AddText2Table";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;
                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table,
            string text, iTextSharp.text.Font font, string align) {

            const string METHOD_NAME = "AddText2Table";

            try {

                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;
                // debug: comment out above and uncomment below to see cell borders.
                //cell.BorderWidth = 1.0F;
                //cell.BorderColor = Color.BLACK;

                switch (align.ToLower()) {
                    case "left":
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        break;
                    case "center":
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        break;

                    case "right":
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        break;
                }
                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Table(PdfPTable table,
            string text, iTextSharp.text.Font font) {

            const string METHOD_NAME = "AddText2Table";

            try {
                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;
                table.AddCell(cell);

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Cell(iTextSharp.text.Paragraph para) {

            const string METHOD_NAME = "AddText2Cell";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(para);
                cell.BorderWidth = 0;

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Cell(string text,
            iTextSharp.text.Font font, string align, int colSpan) {

            const string METHOD_NAME = "AddText2Cell";

            try {
                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;
                cell.Colspan = colSpan;

                switch (align.ToLower()) {
                    case "left":
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        break;
                    case "center":
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        break;

                    case "right":
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        break;
                }

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Cell(iTextSharp.text.Paragraph para, int colSpan) {

            const string METHOD_NAME = "AddText2Cell";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(para);
                cell.BorderWidth = 0;
                cell.Colspan = colSpan;

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Cell(iTextSharp.text.Phrase phrase) {

            const string METHOD_NAME = "AddText2Cell";

            try {
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Cell(string text, iTextSharp.text.Font font, string align) {

            const string METHOD_NAME = "AddText2Cell";

            try {
                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;

                switch (align.ToLower()) {
                    case "left":
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        break;
                    case "center":
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        break;

                    case "right":
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        break;
                }

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Cell(string text, iTextSharp.text.Font font) {

            const string METHOD_NAME = "AddText2Cell";

            try {
                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Cell(string text, iTextSharp.text.Font font, int colSpan) {

            const string METHOD_NAME = "AddText2Cell";

            try {
                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                cell.BorderWidth = 0;
                cell.Colspan = colSpan;

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }

        public static iTextSharp.text.pdf.PdfPCell AddText2Cell(string text, iTextSharp.text.Font font, int hAlign, int vAlign, float fPadding, float borderWidth, int borderType, Color borderColor) {

            const string METHOD_NAME = "AddText2Cell";

            try {
                iTextSharp.text.Phrase phrase = new iTextSharp.text.Phrase(text, font);
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(phrase);

                cell.HorizontalAlignment = hAlign;
                cell.VerticalAlignment = vAlign;
                cell.Padding = fPadding;
                cell.BorderWidth = borderWidth;
                cell.Border = borderType;
                cell.BorderColor = borderColor;

                return cell;
            }
            catch (Exception ex) {
                ApplicationException aex = new ApplicationException(MOD_NAME + METHOD_NAME, ex);
                throw (aex);
            }
        }
    }

    // ==========================================================
    // Make adjustment to document header and margins here.
    // ==========================================================
    internal struct LandscapePageSize {

        private static Rectangle _pageSize = PageSize.LETTER.Rotate();
        public static Rectangle PgPageSize {
            get { return _pageSize; }
        }
        private static float _leftMargin = 36.0F;
        public static float PgLeftMargin {
            get { return _leftMargin; }
        }
        private static float _rightMargin = 36.0F;
        public static float PgRightMargin {
            get { return _rightMargin; }
        }
        private static float _topMargin = 54.0F;
        public static float PgTopMargin {
            get { return _topMargin; }
        }
        private static float _bottomMargin = 72.0F;
        public static float PgBottomMargin {
            get { return _bottomMargin; }
        }
        private static float _leading = 18.0F;
        public static float PgLeading {
            get { return _leading; }
        }
        private static float _hdrHeight = _pageSize.Top - _topMargin - _bottomMargin;
        public static float HdrHeight {
            get { return _hdrHeight; }
        }
        // Header Column Lower Left X-coord: LLX
        private static float _hdrColumnLLX = _leftMargin;
        public static float HdrLowerLeftX {
            get { return _hdrColumnLLX; }
        }
        // Header Column Lower Left Y-coord: LLY
        private static float _hdrColumnLLY = _pageSize.Top - _topMargin - _hdrHeight;
        public static float HdrLowerLeftY {
            get { return _hdrColumnLLY; }
        }
        // Header Column Upper Right X-coord: URX
        private static float _hdrColumnURX = _pageSize.Right - _rightMargin;
        public static float HdrUpperRightX {
            get { return _hdrColumnURX; }
        }
        // Header Column Upper Right Y-coord: URY
        private static float _hdrColumnURY = _pageSize.Top - _topMargin;
        public static float HdrUpperRightY {
            get { return _hdrColumnURY; }
        }

        public static float HdrTopYLine {
            get { return _hdrColumnURY; }
        }

        public static float HdrBottomYLine {
            get { return _hdrColumnLLY; }
        }
    }

    internal struct PortraitPageSize {

        private static Rectangle _pageSize = PageSize.LETTER;
        public static Rectangle PgPageSize {
            get { return _pageSize; }
        }
        private static float _leftMargin = 36.0F;
        public static float PgLeftMargin {
            get { return _leftMargin; }
        }
        private static float _rightMargin = 36.0F;
        public static float PgRightMargin {
            get { return _rightMargin; }
        }
        private static float _topMargin = 54.0F;
        public static float PgTopMargin {
            get { return _topMargin; }
        }
        private static float _bottomMargin = 72.0F;
        public static float PgBottomMargin {
            get { return _bottomMargin; }
        }
        private static float _leading = 18.0F;
        public static float PgLeading {
            get { return _leading; }
        }
        private static float _hdrHeight = _pageSize.Top - _topMargin - _bottomMargin;
        public static float HdrHeight {
            get { return _hdrHeight; }
        }
        // Header Column Lower Left X-coord: LLX
        private static float _hdrColumnLLX = _leftMargin;
        public static float HdrLowerLeftX {
            get { return _hdrColumnLLX; }
        }
        // Header Column Lower Left Y-coord: LLY
        private static float _hdrColumnLLY = _pageSize.Top - _topMargin - _hdrHeight;
        public static float HdrLowerLeftY {
            get { return _hdrColumnLLY; }
        }
        // Header Column Upper Right X-coord: URX
        private static float _hdrColumnURX = _pageSize.Right - _rightMargin;
        public static float HdrUpperRightX {
            get { return _hdrColumnURX; }
        }
        // Header Column Upper Right Y-coord: URY
        private static float _hdrColumnURY = _pageSize.Top - _topMargin;
        public static float HdrUpperRightY {
            get { return _hdrColumnURY; }
        }

        public static float HdrTopYLine {
            get { return _hdrColumnURY; }
        }

        public static float HdrBottomYLine {
            get { return _hdrColumnLLY; }
        }
    }

    interface ICustomPageEvent {
        ColumnText GetColumnObject();
        float HeaderBottomYLine {
            get;
        }
        bool IsDocumentClosing {
            get;
            set;
        }
    }
}
