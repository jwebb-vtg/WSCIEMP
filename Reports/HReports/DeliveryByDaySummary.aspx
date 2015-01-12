<%@ Page Title="Harvest Reports - Delivery By Day Summary" Language="C#" MasterPageFile="~/HarvestReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="DeliveryByDaySummary.aspx.cs" Inherits="WSCIEMP.Reports.HReports.DeliveryByDaySummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Harvest Reports - Delivery By Day Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<table class="wideTable" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td width="10">&nbsp;</td>
		<td>&nbsp;</td></tr>
	<tr>
		<td>&nbsp;</td>
		<td>
			<span class="LabelText">Choose a Delivery Day:</span><br />
			<asp:listbox id="lstDdsDeliveryDay" runat="server" Rows="10" CssClass="MedList"></asp:listbox><br /></td>
	</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
