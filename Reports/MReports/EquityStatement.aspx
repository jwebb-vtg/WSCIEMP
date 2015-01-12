<%@ Page Title="Master Reports - Equity Statement" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="EquityStatement.aspx.cs" Inherits="WSCIEMP.Reports.MReports.EquityStatement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Equity Statement
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<script language="javascript" type="text/javascript">
//<![CDATA[

    $(document).ready(function() {

        //-----------------------------------------
        // Setup calendar
        //-----------------------------------------
        $$('txtEsReportDate', $('#rptCriteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+1',
            changeMonth: true, changeYear: true, duration: 'fast'
        });
        $$('txtActivityFromDate', $('#rptCriteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+1',
            changeMonth: true, changeYear: true, duration: 'fast'
        });
        $$('txtActivityToDate', $('#rptCriteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+1',
            changeMonth: true, changeYear: true, duration: 'fast'
        });
    });

//]]>					
</script>
<table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="rptCriteria">
	<tr>
		<td style="width:1%">&nbsp;</td>
		<td style="width:25%">&nbsp;</td>
		<td style="width:30%">&nbsp;</td>
		<td>&nbsp;</td></tr>
	<tr>
		<td>&nbsp;</td>
		<td class="LabelText">SHID *</td>
		<td><asp:textbox id="txtEsSHID" CssClass="LgText" runat="server"></asp:textbox></td>
		<td>&nbsp;</td></tr>
	<tr><td colspan="4">&nbsp;</td></tr>
	<tr>
		<td>&nbsp;</td>
		<td class="LabelText">Report Date</td>
		<td><asp:textbox id="txtEsReportDate" CssClass="MedList" runat="server"></asp:textbox>
		</td>
		<td>&nbsp;</td></tr>
	<tr><td colspan="4">&nbsp;</td></tr>
	<tr>
		<td>&nbsp;</td>
		<td class="LabelText">Active Only</td>
		<td><asp:checkbox id="chkEsActiveOnly" runat="server"></asp:checkbox></td>
		<td>&nbsp;</td></tr>
    <tr><td colspan="4">&nbsp;</td></tr>    
	<tr>
		<td>&nbsp;</td>
		<td class="LabelText">Print Loan Info</td>
		<td><asp:checkbox id="chkIsLienInfoWanted" runat="server"></asp:checkbox></td>
		<td>&nbsp;</td></tr>
    <tr><td colspan="4">&nbsp;</td></tr>
    <tr style="line-height:30px;">
        <td>&nbsp;</td>
        <td class="LabelText">Activity From Date (Optional)</td>
        <td><asp:textbox id="txtActivityFromDate" CssClass="MedList" runat="server"></asp:textbox></td>
        <td>&nbsp;</td>
    </tr>
    <tr style="line-height:30px;">
        <td>&nbsp;</td>
        <td class="LabelText">Activity To Date (Optional)</td>
        <td><asp:textbox id="txtActivityToDate" CssClass="MedList" runat="server"></asp:textbox></td>
        <td>&nbsp;</td>
    </tr>
	<tr>
		<td colspan="4">&nbsp;</td></tr>
	<tr>
		<td class="ButtonText" colspan="4">
            <span class="LabelText">SHID: *</span><br />
            <ul>
                <li><span class="ButtonText">Leave blank to process the entire coop.</span></li>
                <li><span class="ButtonText">Enter one SHID to report on that one shareholder.</span></li>
                <li><span class="ButtonText">Enter a comma separated list of SHIDs such as 20003,20007,20008</span></li>
                <li><span class="ButtonText">Enter a range by placing a dash between SHIDs, such as
                    60001-60114.</span></li>
                <li><span class="ButtonText">You CANNOT combine the above options!</span></li>
            </ul>
        </td>
    </tr>
	<tr>
		<td colspan="4">&nbsp;</td></tr></table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
