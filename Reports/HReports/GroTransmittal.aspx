<%@ Page Title="Harvest Reports - Payment Transmittal" Language="C#" MasterPageFile="~/HarvestReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="GroTransmittal.aspx.cs" Inherits="WSCIEMP.Reports.HReports.GroTransmittal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Harvest Reports - Payment Transmittal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table class="wideTable" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td style="WIDTH:20%">&nbsp;</td>
		<td style="WIDTH:12%">&nbsp;</td>
		<td >&nbsp;</td>
	</tr>
	<tr>
		<td class="LabelText">Payment: </td>
		<td style="TEXT-ALIGN:center"><span class="LabelText">Cumulative?&nbsp;&nbsp;</span></td>
		<td class="LabelText">Contracts</td>
	</tr>
	<tr style="vertical-align:top;">
		<td>
			<asp:dropdownlist id="ddlGtPaymentDesc" CssClass="MedText" runat="server"></asp:dropdownlist>
		</td>
		<td style="TEXT-ALIGN: center"><asp:CheckBox id="chkGtIsTransmittalCumulative" runat="server"></asp:CheckBox></td>
		<td><asp:listbox id="lstGtContract" CssClass="MedList" runat="server" SelectionMode="Multiple" Rows="15"></asp:listbox></td>
	</tr>
	<tr><td colspan="3">&nbsp;</td></tr>
	<tr><td colspan="3">&nbsp;</td></tr></table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
