<%@ Page Title="Notice of Passthrough" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024"
AutoEventWireup="true" CodeBehind="NoticeOfPassthrough.aspx.cs" Inherits="WSCIEMP.Reports.MReports.NoticeOfPassthrough" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Notice of Passthrough
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<br />
<table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria">
    <tr>
        <th style="width: 16%"></th>
        <th style="width: 26%"></th>
        <th style="width: 7%"></th>
        <th style="width: 21%"></th>
        <th style="width: 12%"></th>
        <th style="width: 18%"></th>
    </tr>
	<tr style="height:28px;">
		<td class="LabelText">Crop Year: </td>
		<td><asp:TextBox ID="txtCropYear" CssClass="MedList" runat="server" Enabled="false"></asp:TextBox></td>
		<td colspan="4">&nbsp;</td>
    </tr>
	<tr style="height:28px;">
		<td class="LabelText">Tax Year: </td>
		<td><asp:TextBox ID="txtTaxYear" CssClass="MedList" runat="server" Enabled="false"></asp:TextBox>	
		</td>
		<td colspan="4"><div style="BACKGROUND-COLOR: #990000; TEXT-ALIGN: center">
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #ffffff">Single SHID</span>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #990000">________</span>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #ffffff">-- OR --</span>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #990000">________</span>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #ffffff">SHID Range</span>
			</div></td>
	</tr>
	<tr style="height:28px;">
		<td class="LabelText">Rate Per Ton: </td>
		<td><asp:TextBox ID="txtRatePerTon" CssClass="MedList" runat="server" Enabled="false"></asp:TextBox></td>
		<td class="LabelText">SHID: </td>
		<td><asp:textbox id="txtSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="5"></asp:textbox></td>
		<td class="LabelText">From SHID:</td>
		<td><asp:textbox id="txtFromSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="6"></asp:textbox></td></tr>
	<tr style="height:28px;">
		<td class="LabelText">Percentage to Apply: </td>
		<td><asp:TextBox ID="txtPctToApply" CssClass="MedList" runat="server" Enabled="false"></asp:TextBox></td>
		<td colspan="2"></td>
		<td class="LabelText">To SHID:</td>
		<td><asp:textbox id="txtToSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="7"></asp:textbox></td></tr>
	<tr style="height:28px;">
	    <td class="LabelText">Report Date: </td>
	    <td colspan="5"><asp:TextBox ID="txtReportDate" CssClass="MedList" runat="server" Enabled="false"></asp:TextBox></td>
	</tr>
	<tr style="height:28px;">
	    <td class="LabelText">Fiscal Year End Date: </td>
	    <td colspan="5"><asp:TextBox ID="txtFiscalYearEndDate" CssClass="MedList" runat="server" Enabled="false"></asp:TextBox></td>    
    </tr>
	<tr>
	    <td>&nbsp;</td>
	    <td colspan="5">
            &nbsp;
	    </td>
	</tr>
</table>
<br />
<br />
<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>