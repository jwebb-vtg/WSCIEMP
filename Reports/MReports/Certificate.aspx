<%@ Page Title="Master Reports - Certificates" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" 
StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="Certificate.aspx.cs" Inherits="WSCIEMP.Reports.MReports.Certificate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Certificates
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<script language="javascript" type="text/javascript">
//<![CDATA[

    $(document).ready(function () {
        
        //-----------------------------------------
        // Setup calendar
        //-----------------------------------------
        $$('txtCertificateDate', $('#criteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+1',
            changeMonth: true, changeYear: true, duration: 'fast'
        });
    });
    
//]]>					
</script>
<br />
<table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria">
    <tr>
        <th style="width: 14%"></th>
        <th style="width: 26%"></th>
        <th style="width: 9%"></th>
        <th style="width: 21%"></th>
        <th style="width: 12%"></th>
        <th style="width: 18%"></th>
    </tr>
	<tr style="height:28px;">
		<td class="LabelText">Equity Type: </td>
		<td><asp:dropdownlist id="ddlEquityType" CssClass="MedList" runat="server" tabIndex="1">
		</asp:dropdownlist></td>
		<td colspan="4">&nbsp;</td>
    </tr>
	<tr style="height:28px;">
		<td class="LabelText">Certificate Date: </td>
		<td><asp:TextBox ID="txtCertificateDate" CssClass="MedList" runat="server"></asp:TextBox>	
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
		<td class="LabelText">&nbsp;</td>
		<td>&nbsp;</td>
		<td class="LabelText">SHID: </td>
		<td><asp:textbox id="txtPsSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="5"></asp:textbox></td>
		<td class="LabelText">From SHID:</td>
		<td><asp:textbox id="txtPsFromSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="6"></asp:textbox></td></tr>
	<tr style="height:28px;">
		<td class="LabelText">Signature Name: </td>
		<td><asp:Label ID="lblSigName" CssClass="MedText" runat="server" BackColor="White"></asp:Label></td>
		<td colspan="2"></td>
		<td class="LabelText">To SHID:</td>
		<td><asp:textbox id="txtPsToSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="7"></asp:textbox></td></tr>
	<tr>
	    <td class="LabelText">Signature Title</td>
	    <td colspan="5"><asp:Label ID="lblSigTitle" CssClass="MedText" runat="server" BackColor="White"></asp:Label></td>
	</tr>
	<tr>
        <td colspan="6">&nbsp;</td></tr>
    </tr>
	<tr>
	    <td>&nbsp;</td>
	    <td colspan="5">
            <asp:Button ID="btnDeletePDF" runat="server" CssClass="ButtonText" 
                Text="Delete PDF" onclick="btnDeletePDF_Click" />  
	    </td>
	</tr>
</table>
<br />
<br />
<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
