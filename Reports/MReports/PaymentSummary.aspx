<%@ Page Title="Master Reports - Payment Summary" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="PaymentSummary.aspx.cs" Inherits="WSCIEMP.Reports.MReports.PaymentSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Payment Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <script type="text/javascript">

        $(document).ready(function() {

            //-----------------------------------------------------------------------
            // Sync the ddlPsPaymentDesc selected index with the
            // hidden ddlPsStatementDate index.  Then copy the ddlPsStatementDate
            // text over to the visable txtStatementDate
            //-----------------------------------------------------------------------
            $$('ddlPsPaymentDesc', $('#criteria')).change(function(event) {

                var selectedIndex = $$('ddlPsPaymentDesc', $('#criteria')).attr('selectedIndex');
                $$('ddlPsStatementDate', $('#dataStore')).attr('selectedIndex', selectedIndex);
                $$('txtStatementDate', $('#criteria')).val($$('ddlPsStatementDate', $('#dataStore')).val());
            });
        });
        
    </script>
<div id="dataStore" class="DisplayOff">
    <asp:dropdownlist id="ddlPsStatementDate" runat="server" CssClass="MedList"></asp:dropdownlist>
</div>
<br />
<table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria">
	<tr>
	    <th style="width:15%"></th>
	    <th style="width:25%"></th>
	    <th style="width:8%"></th>
	    <th style="width:22%"></th>
	    <th style="width:10%"></th>
	    <th style="width:20%"></th>
    </tr>
	<tr>
		<td class="LabelText">Payment: </td>
		<td><asp:dropdownlist id="ddlPsPaymentDesc" CssClass="ButtonText" Width="200" runat="server" tabIndex="2"></asp:dropdownlist></td>
		<td colspan="4"><div style="BACKGROUND-COLOR: #990000; TEXT-ALIGN: center">
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #ffffff;">Single SHID</span>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #990000;">________</span>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #ffffff;">-- OR --</span>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #990000;">________</span>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #ffffff;">SHID Range</span>
			</div></td>
	</tr>
	<tr style="height:30px;">
		<td class="LabelText">Payment Date:</td>
		<td>
            <asp:TextBox ID="txtStatementDate" CssClass="MedList" runat="server"></asp:TextBox>
		</td>
		<td class="LabelText">SHID: </td>
		<td><asp:textbox id="txtPsSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="5"></asp:textbox></td>
		<td class="LabelText">From SHID:</td>
		<td><asp:textbox id="txtPsFromSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="6"></asp:textbox></td></tr>
	<tr style="height:28px;">
		<td class="LabelText">Cumulative?</td>
		<td><asp:CheckBox id="chkPsIsPaymentSummaryCumulative" runat="server" ></asp:CheckBox></td>
		<td colspan="2"></td>
		<td class="LabelText">To SHID:</td>
		<td><asp:textbox id="txtPsToSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="7"></asp:textbox></td></tr>
	<tr><td colspan="6">&nbsp;</td></tr>
	<tr><td colspan="6">&nbsp;</td>
	</tr>
</table>
<div id="rptParams" class="DisplayOff">
    <asp:HiddenField ID="rptParam_Footer" runat="server" Value="This is a summary for the contracts included in the total amount of the enclosed check. To retrieve the detailed Information for the Current Payment, as well as, the YTD Amounts if you are a Shareholder sign on the Web, if you do not have access to the web, contact your agriculturist who can print a copy for you or call Marty Smith at 970-304-6015 and we can mail you a copy." />
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
