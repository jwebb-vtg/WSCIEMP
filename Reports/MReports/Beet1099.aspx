<%@ Page Title="Master Reports - Export Sectioin 199 1099" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" 
StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="Beet1099.aspx.cs" Inherits="WSCIEMP.Reports.MReports.Beet1099" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Export Section 199 1099's 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria">
	<tr>
		<td style="width:1%">&nbsp;</td>
		<td style="width:12%">&nbsp;</td>
		<td style="width:40%;">&nbsp;</td>
		<td style="text-align:center;">&nbsp;</td>
	</tr>
	<tr style="height: 30px;">
		<td>&nbsp;</td>
		<td class="LabelText">File Name:
		</td>
		<td><asp:textbox id="txtFileName" CssClass="ButtonText" runat="server"></asp:textbox>
		<span class="ButtonText" style="color:red; TEXT-ALIGN: left">&nbsp;&nbsp;&nbsp;You must save 
				as .CSV</span></td>
		<td align="left">&nbsp;</td>
	</tr>
	<tr><td colspan="4">&nbsp;</td></tr>
	<tr>
		<td colspan="4" style="line-height:100px;">&nbsp;</td>
	</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
