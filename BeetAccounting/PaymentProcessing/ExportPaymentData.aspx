<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="ExportPaymentData.aspx.cs" 
Inherits="WSCIEMP.BeetAccounting.PaymentProcessing.ExportPaymentData" Title="BeetAccounting Export Payment Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    BeetAccounting - Export Payment Data
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <div class="PagePartWide" style="padding-left:10px; width:97%;">
    <br class="halfLine" />
    <table border="0" style="width: 100%;">
        <tr>
            <td style="width:28%;"><span class="LabelText">Payment Number:</span></td>
            <td style="width:12%;"><span class="LabelText">CropYear:</span></td>
            <td style="width:60%;">&nbsp;</td>
        </tr>
        <tr style="vertical-align:top;">
            <td>
                <asp:ListBox ID="lstPaymentNumber" runat="server" Rows="14" Width="250" CssClass="ButtonText"></asp:ListBox>
            </td>
            <td style="text-align:left" colspan="2">
                <asp:DropDownList ID="ddlCropYear" CssClass="ctlWidth60" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlCropYear_SelectedIndexChanged">
                </asp:DropDownList><br /><br />
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="LabelText" 
                    onclick="btnExport_Click" /><br /><br />
                <asp:HyperLink ID="lnkPaymentFile" CssClass="LabelText" runat="server"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
    <br />
</div> 
<br />
<div class="PagePartWide" style="padding-left:10px; width:97%;">

<span class="LabelText">Export Summary</span><br /><br />

<span class="LabelText">Exported: </span><asp:TextBox ID="txtSumExported" CssClass="ctlWidth90" runat="server"></asp:TextBox><br /><br />
</div>
<br />
<br />
<br />
<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>

