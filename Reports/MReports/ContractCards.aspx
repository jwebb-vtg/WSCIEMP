<%@ Page Title="Master Reports - Contract Cards" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="ContractCards.aspx.cs" Inherits="WSCIEMP.Reports.MReports.ContractCards" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Contract Cards
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<table class="wideTable" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td style="width:1%">&nbsp;</td>
		<td style="width:25%">&nbsp;</td>
		<td style="width:25%">&nbsp;</td>
		<td>&nbsp;</td>
	</tr>
	<tr style="height:28px;">
		<td>&nbsp;</td>
		<td class="LabelText">First Contract Number</td>
		<td><asp:textbox id="txtCcContractNumberStart" runat="server" CssClass="ButtonText"></asp:textbox></td>
		<td>&nbsp;</td>
	</tr>
	<tr style="height:28px;">
		<td>&nbsp;</td>
		<td class="LabelText">Last Contract Number</td>
		<td><asp:textbox id="txtCcContractNumberStop" runat="server" CssClass="ButtonText"></asp:textbox></td>
		<td>&nbsp;</td>
	</tr>
	<tr style="height:28px;">
		<td>&nbsp;</td>
		<td class="LabelText">File Name</td>
		<td><asp:textbox id="txtCcFileName" runat="server" CssClass="ButtonText" ></asp:textbox></td>
		<td align="left"><span class="WarningOn" style="TEXT-ALIGN:left">&nbsp;You must save as .CSV</span></td>
	</tr>
	<tr>
		<td colspan="4">&nbsp;</td></tr>
	<tr>
		<td colspan="4">&nbsp;</td></tr></table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
