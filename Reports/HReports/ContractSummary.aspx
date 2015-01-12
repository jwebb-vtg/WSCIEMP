<%@ Page Title="Harvest Reports - Contract Summary" Language="C#" MasterPageFile="~/HarvestReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="ContractSummary.aspx.cs" Inherits="WSCIEMP.Reports.HReports.ContractSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Harvest Reports - Contract Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<table class="wideTable" cellspacing="0" cellpadding="0" border="0">
	<tr><td colspan="2">&nbsp;</td></tr>
	<tr>
		<td width="10">&nbsp;</td>
		<td><span class="ButtonText">Press the Print button to generate a Contract Summary report.</span></td>
	</tr>
	<tr><td style="line-height:100px;">&nbsp;</td></tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
