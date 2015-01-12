<%@ Page Title="" Language="C#" MasterPageFile="~/HarvestReportTemplate.Master" StylesheetTheme="ThemeA_1024"
AutoEventWireup="true" CodeBehind="BeetPaymentBreakdown.aspx.cs" Inherits="WSCIEMP.Reports.HReports.BeetPaymentBreakdown" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Harvest Reports - Beet Payments by Year
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<table class="wideTable" cellspacing="0" cellpadding="0" border="0">
	<tr style="height:28px;">
		<td style="width:15%;">
            <asp:RadioButton ID="radCalYear" GroupName="CalControl" Text="Calendar Year" CssClass="LabelText" runat="server" />            
        </td>
        <td rowspan="2" style="vertical-align:middle;">
            <asp:DropDownList ID="ddlYear" CssClass="ButtonText" runat="server">
            </asp:DropDownList>
        </td>
	</tr>
    <tr>
        <td>
            <asp:RadioButton ID="radCropYear" GroupName="CalControl" Text="Crop Year" CssClass="LabelText" runat="server" />
        </td>
    </tr>
	<tr>
		<td style="line-height:30px;">&nbsp;</td>
	</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
