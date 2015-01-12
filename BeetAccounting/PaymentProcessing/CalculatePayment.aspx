<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="CalculatePayment.aspx.cs" 
Inherits="WSCIEMP.BeetAccounting.PaymentProcessing.CalculatePayment" Title="BeetAccounting Calculate Payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    BeetAccounting - Calculate Payment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <div class="PagePartWide" style="padding-left:10px; width:97%;">
    <br class="halfLine" />
    <table border="0" style="width: 60%;">
        <tr>
            <td><span class="LabelText">Factory:</span></td>
            <td><span class="LabelText">Payment:</span></td>
            <td><span class="LabelText">CropYear:</span></td>
        </tr>
        <tr style="vertical-align:top;">
            <td>
                <asp:ListBox ID="lstFactory" runat="server" Rows="14" Width="150" CssClass="ButtonText"></asp:ListBox>
            </td>
            <td>
                <asp:ListBox ID="lstPayment" runat="server" Rows="14" Width="250" CssClass="ButtonText"></asp:ListBox>
            </td>
            <td>
                <asp:DropDownList ID="ddlCropYear" CssClass="ctlWidth60" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlCropYear_SelectedIndexChanged">
                </asp:DropDownList><br /><br />            
                <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="LabelText" 
                    onclick="btnProcess_Click" />
            </td>
        </tr>
    </table>
    <br />
</div> 
<br />
<br />
<br />
<br />
<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
