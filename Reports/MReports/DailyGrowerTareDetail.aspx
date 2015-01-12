<%@ Page Title="" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="DailyGrowerTareDetail.aspx.cs" Inherits="WSCIEMP.Reports.MReports.DailyGrowerTareDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Master Reports - Daily Grower Tare Detail
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
    <asp:ScriptManager ID="mainScriptManager" runat="server">
    </asp:ScriptManager>
    <div id="dataStore" class="DisplayOff">
        <asp:DropDownList ID="ddlTxStatementDate" runat="server" CssClass="MedList">
        </asp:DropDownList>
    </div>
    <table class="wideTable" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td>
                <table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria">
                    <tr>
                        <td style="vertical-align: top;">
                            <table style="width: 98%;" border="0">
                                <tr>
                                    <td style="width: 50%;" class="LabelText">
                                        From Date
                                    </td>
                                    <td class="LabelText">
                                        To Date
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" CssClass="ctlWidth75" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" CssClass="ctlWidth75" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkPosted" runat="server" CssClass="LabelText" Text="Posted" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkPreview" runat="server" CssClass="ButtonText" Text="Preview *" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkWebHardCopyOnly" runat="server" CssClass="ButtonText" Text="Web Hard Copy Only" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkEmail" runat="server" CssClass="ButtonText" Text="Email" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkFax" runat="server" CssClass="ButtonText" Text="Fax" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:UpdatePanel ID="uplCriteria" UpdateMode="Conditional" ChildrenAsTriggers="true"
                    runat="server">
                    <ContentTemplate>
                        <div class="WarningOff" id="popWarning" runat="server">
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
                                    <asp:ListBox ID="lstTxFactory" runat="server" CssClass="MedListPlus" AutoPostBack="True"
                                        OnSelectedIndexChanged="lstTxFactory_SelectedIndexChanged" Rows="15" SelectionMode="Multiple">
                                    </asp:ListBox>
                                </td>
                                <td>
                                    <asp:ListBox ID="lstTxStation" runat="server" CssClass="MedListPlus" AutoPostBack="True"
                                        OnSelectedIndexChanged="lstTxStation_SelectedIndexChanged" Rows="15" SelectionMode="Multiple">
                                    </asp:ListBox>
                                </td>
                                <td>
                                    <asp:ListBox ID="lstTxContract" runat="server" CssClass="MedList" Rows="15" SelectionMode="Multiple">
                                    </asp:ListBox>
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
    </table><br />
    <span class="ButtonText">* Preview mode will generate a PDF.  Nothing will be emailed, nothing will be faxed.  Use preview to check what will be faxed/emailed.
    </span>
    <br /><br /><br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
