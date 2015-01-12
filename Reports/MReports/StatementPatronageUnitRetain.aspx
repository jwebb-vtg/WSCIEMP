<%@ Page Title="Master Reports - Statement Patronage - Unit Retain" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" 
StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="StatementPatronageUnitRetain.aspx.cs" 
Inherits="WSCIEMP.Reports.MReports.StatementPatronageUnitRetain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Statement Patronage - Unit Retain
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <script type="text/javascript">

        $(document).ready(function() {

            $$('ddlEquityType', $('#criteria')).change(function(event) {

                // 1 = Patronage, 2 = Unit Retain
                if ($$('ddlEquityType', $('#criteria')).val() == '1') {
                    $$('wrapPatPaymentDesc', $('#criteria')).attr('class', 'DisplayOn');
                    $$('wrapRetPaymentDesc', $('#criteria')).attr('class', 'DisplayOff');
                    $$('ddlPatPaymentDesc', $('#criteria')).change();
                } else {
                    $$('wrapPatPaymentDesc', $('#criteria')).attr('class', 'DisplayOff');
                    $$('wrapRetPaymentDesc', $('#criteria')).attr('class', 'DisplayOn');
                    $$('ddlRetPaymentDesc', $('#criteria')).change();
                }
            });
            $$('ddlEquityType', $('#criteria')).change();

            //-----------------------------------------------------------------------
            // Sync the related payment type selection to the correct hidden date 
            // control in order to populate the visible date control.
            //-----------------------------------------------------------------------
            $$('ddlPatPaymentDesc', $('#criteria')).change(function(event) {

                var selectedIndex = $$('ddlPatPaymentDesc', $('#criteria')).attr('selectedIndex');
                $$('ddlPatStatementDate', $('#dataStore')).attr('selectedIndex', selectedIndex);
                $$('txtStatementDate', $('#criteria')).val($$('ddlPatStatementDate', $('#dataStore')).val());
            });

            $$('ddlRetPaymentDesc', $('#criteria')).change(function(event) {

                var selectedIndex = $$('ddlRetPaymentDesc', $('#criteria')).attr('selectedIndex');
                $$('ddlRetStatementDate', $('#dataStore')).attr('selectedIndex', selectedIndex);
                $$('txtStatementDate', $('#criteria')).val($$('ddlRetStatementDate', $('#dataStore')).val());
            });

            if ($$('ddlEquityType', $('#criteria')).val() == '1') {
                $$('ddlPatPaymentDesc', $('#criteria')).change();
            } else {
                $$('ddlRetPaymentDesc', $('#criteria')).change();
            }
        });
        
    </script>
<br />
<div id="dataStore" class="DisplayOff">
    <asp:dropdownlist id="ddlPatStatementDate" runat="server" CssClass="MedList" ></asp:dropdownlist>
    <asp:dropdownlist id="ddlRetStatementDate" runat="server" CssClass="MedList" ></asp:dropdownlist>
</div>
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
		<td class="LabelText">Payment Type: </td>
		<td>
		    <div id="wrapPatPaymentDesc" runat="server">
		        <asp:dropdownlist id="ddlPatPaymentDesc" CssClass="ButtonText" Width="180" runat="server" tabIndex="2"></asp:dropdownlist>
		    </div>
		    <div id="wrapRetPaymentDesc" runat="server">
		        <asp:dropdownlist id="ddlRetPaymentDesc" CssClass="ButtonText" Width="180" runat="server" tabIndex="2"></asp:dropdownlist>
		    </div>		
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
		<td class="LabelText">Payment Date:</td>
		<td><asp:TextBox ID="txtStatementDate" CssClass="MedList" runat="server"></asp:TextBox>
		</td>
		<td class="LabelText">SHID: </td>
		<td><asp:textbox id="txtPsSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="5"></asp:textbox></td>
		<td class="LabelText">From SHID:</td>
		<td><asp:textbox id="txtPsFromSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="6"></asp:textbox></td></tr>
	<tr style="height:28px;">
		<td class="LabelText">&nbsp;</td>
		<td>&nbsp;</td>
		<td colspan="2"></td>
		<td class="LabelText">To SHID:</td>
		<td><asp:textbox id="txtPsToSHID" CssClass="ButtonText" runat="server" Rows="5" tabIndex="7"></asp:textbox></td></tr>
	<tr><td colspan="6">&nbsp;</td></tr>
</table>
<br />
<br />
<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
