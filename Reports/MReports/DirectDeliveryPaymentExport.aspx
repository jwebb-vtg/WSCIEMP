<%@ Page Title="Master Reports - Direct Delivery Payment Export" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" AutoEventWireup="true" StylesheetTheme="ThemeA_1024"
CodeBehind="DirectDeliveryPaymentExport.aspx.cs" Inherits="WSCIEMP.Reports.MReports.DirectDeliveryPaymentExport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Master Reports - Direct Delivery Payment Export
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <script language="javascript" type="text/javascript">
        //<![CDATA[

        $(document).ready(function () {
            
            //-----------------------------------------
            // Setup calendar
            //-----------------------------------------
            $$('txtFromDate', $('#criteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
                buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: true, yearRange: '-10:+1',
                changeMonth: true, changeYear: true, duration: 'fast'
            });
            $$('txtToDate', $('#criteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
                buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: true, yearRange: '-10:+1',
                changeMonth: true, changeYear: true, duration: 'fast'
            });
        });

        //]]>					
    </script>
    <table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria" style="width: 70%;">
        <tr>
            <td rowspan="2" style="width: 2%;">
                &nbsp;
            </td>
            <td style="width: 35%;">
                <span class="LabelText">From Date:</span><br />
                <asp:textbox id="txtFromDate" runat="server" CssClass="ButtonText" Rows="5"></asp:textbox>
            </td>
            <td>
                
                <span class="LabelText">Payment Number:</span><br />
                <asp:DropDownList ID="ddlPaymentNumber" runat="server" CssClass="LgText">
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="height:50px;">
            <td>                            
                <span class="LabelText">To Date:</span><br />
                <asp:textbox id="txtToDate" runat="server" CssClass="ButtonText" Rows="5"></asp:textbox>
            </td>
            <td>&nbsp;
                <asp:HyperLink ID="lnkPaymentFile" CssClass="LabelText" runat="server"></asp:HyperLink>          
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
