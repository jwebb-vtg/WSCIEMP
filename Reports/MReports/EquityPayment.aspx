<%@ Page Title="Master Reports - Equity Payment Export" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="EquityPayment.aspx.cs" Inherits="WSCIEMP.Reports.MReports.EquityPayment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Export File (Equity Payment Export)
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <script type="text/javascript">

        $(document).ready(function() {

            $$('ddlEpEquityType', $('#criteria')).change(function(event) {

                // 1 = Patronage, 2 = Unit Retain
                if ($$('ddlEpEquityType', $('#criteria')).val() == '1') {       
                    $$('wrapPatList', $('#criteria')).attr('class', 'DisplayOn');
                    $$('wrapRetList', $('#criteria')).attr('class', 'DisplayOff');
                    $$('ddlEpPaymentPat', $('#criteria')).change();
                } else {
                    $$('wrapPatList', $('#criteria')).attr('class', 'DisplayOff');
                    $$('wrapRetList', $('#criteria')).attr('class', 'DisplayOn');
                    $$('ddlEpPaymentRet', $('#criteria')).change();
                }
            });
            $$('ddlEquityType', $('#criteria')).change();

            //-----------------------------------------------------------------------
            // Sync the related payment type selection to the correct hidden date 
            // control in order to populate the visible date control.
            //-----------------------------------------------------------------------
            $$('ddlEpPaymentPat', $('#criteria')).change(function(event) {

                var selectedIndex = $$('ddlEpPaymentPat', $('#criteria')).attr('selectedIndex');
                $$('ddlPatStatementDate', $('#dataStore')).attr('selectedIndex', selectedIndex);
                $$('txtStatementDate', $('#criteria')).val($$('ddlPatStatementDate', $('#dataStore')).val());
            });

            $$('ddlEpPaymentRet', $('#criteria')).change(function(event) {

                var selectedIndex = $$('ddlRetPaymentDesc', $('#criteria')).attr('selectedIndex');
                $$('ddlRetStatementDate', $('#dataStore')).attr('selectedIndex', selectedIndex);
                $$('txtStatementDate', $('#criteria')).val($$('ddlRetStatementDate', $('#dataStore')).val());
            });

            if ($$('ddlEpEquityType', $('#criteria')).val() == '1') {
                $$('ddlEpPaymentPat', $('#criteria')).change();
            } else {
                $$('ddlEpPaymentRet', $('#criteria')).change();
            }
        });
        
    </script>

<div id="dataStore" class="DisplayOff">
    <asp:dropdownlist id="ddlPatStatementDate" runat="server" CssClass="MedList" ></asp:dropdownlist>
    <asp:dropdownlist id="ddlRetStatementDate" runat="server" CssClass="MedList" ></asp:dropdownlist>
</div>   
<table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria">
	<tr>
		<td style="width:1%">&nbsp;</td>
		<td style="width:12%">&nbsp;</td>
		<td style="width:40%;">&nbsp;</td>
		<td style="text-align:center;"><span class="LabelText">&nbsp;</span></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td class="LabelText"><span class="LabelText">Equity Type:</span></td>
		<td><asp:dropdownlist id="ddlEpEquityType" CssClass="LgList" runat="server"></asp:dropdownlist>
		</td>
		<td rowspan="2" style="text-align:center;">&nbsp;</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td class="LabelText" colspan="2">&nbsp;</td>
	</tr>
	<tr style="height: 30px;">
		<td>&nbsp;</td>
		<td class="LabelText">Payment Type:
		</td>
		<td><div id="wrapPatList" class="DisplayOn" runat="server"><asp:dropdownlist id="ddlEpPaymentPat" CssClass="LgList" runat="server"></asp:dropdownlist></div>
		<div id="wrapRetList" class="DisplayOff" runat="server"><asp:dropdownlist id="ddlEpPaymentRet" CssClass="LgList" runat="server"></asp:dropdownlist></div></td>
		<td>&nbsp;</td>
	</tr>
	<tr style="height: 30px;">
	    <td>&nbsp;</td>
		<td class="LabelText">Payment Date:
		</td>	    
		<td><asp:TextBox ID="txtStatementDate" CssClass="MedList" runat="server"></asp:TextBox></td>
	    <td>&nbsp;</td>
	</tr>
	<tr style="height: 30px;">
		<td>&nbsp;</td>
		<td class="LabelText">File Name:
		</td>
		<td><asp:textbox id="txtEpFileName" CssClass="ButtonText" runat="server"></asp:textbox>
		<span class="ButtonText" style="color:red; TEXT-ALIGN: left">&nbsp;&nbsp;&nbsp;You must save 
				as .CSV</span></td>
		<td align="left">&nbsp;</td>
	</tr>
	<tr><td colspan="4">&nbsp;</td></tr>
	<tr>
		<td>&nbsp;</td>
		<td class="ButtonText" colspan="3"><span class="LabelText">Warning: </span>Below may 
			appear a listing of SHIDs having more than 125 characters in the Payee Description (Check the export file.)
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td colspan="3"><asp:textbox id="txtEpWarnSHID" runat="server" TextMode="MultiLine" Width="500px" Rows="5"></asp:textbox></td>
	</tr>
	<tr>
		<td colspan="4" style="line-height:100px;">&nbsp;</td>
	</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
