<%@ Page Title="Master Reports - Direct Delivery Statement"
    Language="C#" MasterPageFile="~/MasterReportTemplate.Master" AutoEventWireup="true"
    StylesheetTheme="ThemeA_1024" CodeBehind="DirectDeliveryStatement.aspx.cs" Inherits="WSCIEMP.Reports.MReports.DirectDeliveryStatement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Master Reports - Direct Delivery Statement
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
    <table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria" style="width:75%;">
        <tr>
            <td rowspan="5" style="width: 2%;">
                &nbsp;
            </td>
            <td>
                <span class="LabelText">From Date:</span><br />
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="ButtonText" Rows="5"></asp:TextBox>
            </td>
            <td>
                <span class="LabelText">To Date:</span><br />
                <asp:TextBox ID="txtToDate" runat="server" CssClass="ButtonText" Rows="5"></asp:TextBox>
            </td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2">
                <span class="LabelText">SHID: *</span><br />
                <asp:TextBox ID="txtShid" CssClass="LgText" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td class="ButtonText" colspan="2">
                * SHID options:<br />
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
    </table>
    <br />
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
