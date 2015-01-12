<%@ Page Title="Harvest Reports - Tons By Truck By Contract" Language="C#" MasterPageFile="~/HarvestReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="TonsByTruckByContract.aspx.cs" Inherits="WSCIEMP.Reports.HReports.TonsByTruckByContract" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Harvest Reports - Tons By Truck By Contract
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<table class="wideTable" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <th style="width:1%"></th>
        <th style="width:38%"></th>
        <th></th>
    </tr>
	<tr>
		<td>&nbsp;</td>
		<td colspan="2">&nbsp;</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td>
			<span class="LabelText">Contract:</span><br />
			<span class="ButtonText">(Hold down the Ctrl key to select multiple Contracts.)</span><br />
			<asp:listbox id="lstTtcContract" CssClass="MedList" Rows="10" runat="server" SelectionMode="Multiple"></asp:listbox><br />
		</td>
		<td colspan="2" class="ButtonText" style="VERTICAL-ALIGN: middle"><br />
			<asp:RadioButton GroupName="optPrint" id="radTtcPrintPDF" runat="server" Text="Generate a PDF file"></asp:RadioButton><br /><br />
			<asp:RadioButton GroupName="optPrint" id="radTtcPrintCSV" runat="server" Text="Generate a CSV file (Compatible with Excel)"></asp:RadioButton><br />
			<span class="WarningOn" style="TEXT-ALIGN:left">IMPORTANT: When generating a CSV file you must save it as a .CSV</span>
			
		</td>
	</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
