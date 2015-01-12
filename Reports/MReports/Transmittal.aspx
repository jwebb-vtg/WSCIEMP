<%@ Page Title="Master Reports - Payment Transmittal" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="Transmittal.aspx.cs" Inherits="WSCIEMP.Reports.MReports.Transmittal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Payment Transmittal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<script language="javascript" type="text/javascript">
//<![CDATA[

    $(document).ready(function() {

        //-----------------------------------------
        // Setup calendar
        //-----------------------------------------
        $$('txtTxFromDate', $('#criteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: true, yearRange: '-10:+1',
            changeMonth: true, changeYear: true, duration: 'fast'
        });
        $$('txtTxToDate', $('#criteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: true, yearRange: '-10:+1',
            changeMonth: true, changeYear: true, duration: 'fast'
        });

        //-----------------------------------------------------------------------
        // Sync the ddlTxPaymentDesc selected index with the
        // hidden ddlTxStatementDate index.  Then copy the ddlTxStatementDate
        // text over to the visable txtStatementDate
        //-----------------------------------------------------------------------
        $$('ddlTxPaymentDesc', $('#criteria')).change(function(event) {

            var selectedIndex = $$('ddlTxPaymentDesc', $('#criteria')).attr('selectedIndex');
            $$('ddlTxStatementDate', $('#dataStore')).attr('selectedIndex', selectedIndex);
            $$('txtStatementDate', $('#criteria')).val($$('ddlTxStatementDate', $('#dataStore')).val());
        });
        $$('ddlTxPaymentDesc', $('#criteria')).change();
    });

//]]>    				
</script>
    <asp:ScriptManager ID="mainScriptManager" runat="server">
    </asp:ScriptManager>
    <div id="dataStore" class="DisplayOff">
        <asp:dropdownlist id="ddlTxStatementDate" runat="server" CssClass="MedList" ></asp:dropdownlist>
    </div>
    <table class="wideTable" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td>
                <table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria">
                    <tr>
                        <td style="width: 2%;">
                            &nbsp;
                        </td>
                        <td>
                            <span class="LabelText">Payment:</span><br />
                            <asp:dropdownlist id="ddlTxPaymentDesc" runat="server" CssClass="ButtonText" Width="160" ></asp:dropdownlist><br /><br />
                            
                            <span class="LabelText">From Date:</span><br />
                            <asp:textbox id="txtTxFromDate" runat="server" CssClass="ButtonText" Rows="5"></asp:textbox><br /><br />
                            
                            <span class="LabelText">To Date:</span><br />
                            <asp:textbox id="txtTxToDate" runat="server" CssClass="ButtonText" Rows="5"></asp:textbox><br /><br />
                            
                            <span class="LabelText">Statement Date:</span><br />
                            <asp:TextBox ID="txtStatementDate" CssClass="MedList" runat="server"></asp:TextBox><br /><br />
                            
                            <span class="LabelText">Cumulative?&nbsp;&nbsp;</span> 
                            <asp:checkbox id="chkTxIsTransmittalCumulative" runat="server" ></asp:checkbox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:UpdatePanel ID="uplCriteria" UpdateMode="Conditional" ChildrenAsTriggers="true"
                    runat="server">
                    <ContentTemplate>
                        <div class="WarningOff" id="divAjaxWarning" runat="server">
                        </div>
                        <table class="wideTable" cellspacing="0" cellpadding="0" border="0">
                            <tr style="vertical-align: bottom;">
                                <td class="LabelText">
                                    Factories
                                </td>
                                <td class="LabelText">
                                    Stations
                                </td>
                                <td class="LabelText">
                                    Contracts
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:listbox id="lstTxFactory" runat="server" CssClass="MedListPlus" AutoPostBack="True" 
                                        onselectedindexchanged="lstTxFactory_SelectedIndexChanged" Rows="15" SelectionMode="Multiple"></asp:listbox>
                                </td>
                                <td>
                                    <asp:listbox id="lstTxStation" runat="server" CssClass="MedListPlus" AutoPostBack="True" 
                                        onselectedindexchanged="lstTxStation_SelectedIndexChanged" Rows="15" SelectionMode="Multiple"></asp:listbox>
                                </td>
                                <td>
                                    <asp:listbox id="lstTxContract" runat="server" CssClass="MedList" Rows="15" SelectionMode="Multiple"></asp:listbox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
