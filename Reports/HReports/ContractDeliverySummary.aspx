<%@ Page Title="Harvest Reports - Contract Delivery" Language="C#" MasterPageFile="~/HarvestReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="ContractDeliverySummary.aspx.cs" Inherits="WSCIEMP.Reports.HReports.ContractDeliverySummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Harvest Reports - Contract Delivery Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table class="wideTable" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td width="10">&nbsp;</td>
		<td>&nbsp;</td></tr>
	<tr>
		<td width="10">&nbsp;</td>
		<td><span class="LabelText">Choose
				a Contract:</span><br />
			<asp:listbox id="lstCdsContract" runat="server" Rows="10" CssClass="MedList"></asp:listbox><br />
		</td>
	</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
