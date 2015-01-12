<%@ Page Title="Master Reports - Shareholder Summary" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="ShareholderSummary.aspx.cs" Inherits="WSCIEMP.Reports.MReports.ShareholderSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Shareholder Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<table class="wideTable" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td style="width:1%">&nbsp;</td>
		<td style="width:20%">&nbsp;</td>
		<td style="width:30%">&nbsp;</td>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td class="LabelText">SHID *</td>
		<td><asp:textbox id="txtSsSHID" CssClass="LgText" runat="server"></asp:textbox></td>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td colspan="4">&nbsp;</td>
	</tr>
	<tr>
		<td class="ButtonText" colspan="4">* SHID options:<br />
			<ul>
				<li>
				    <span class="ButtonText">Enter one SHID to report on that one shareholder.</span></li>
				<li>
				<span class="ButtonText">Enter a comma separated list of SHIDs such as 20003,20007,20008</span></li>
				<li>
				<span class="ButtonText">Enter a range by placing a dash between SHIDs, such as 60001-60114.</span></li>
				<li>
					<span class="ButtonText">You CANNOT combine the above options!</span></li>
			</ul>
		</td>
	</tr>
	<tr>
		<td colspan="4">&nbsp;</td>
	</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
