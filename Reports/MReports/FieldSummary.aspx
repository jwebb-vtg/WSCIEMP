<%@ Page Title="Master Reports - Field Summary" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="FieldSummary.aspx.cs" Inherits="WSCIEMP.Reports.MReports.FieldSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Field Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript">

        $(document).ready(function() {

            $$("btnFlip", $("#criteria")).click(function(event) {

                var bFlip = $$("btnFlip", $("#criteria"));
                var divA = $$('divStationCriteria', $('#criteria'));
                var divB = $$('divSHIDCriteria', $('#criteria'));
                var txtShowB = $$('txtFsDivSHIDShowing', $('#criteria'));

                if (divA.attr('class') == 'DisplayOff') {

                    divA.attr('class', 'DisplayOn');
                    divB.attr('class', 'DisplayOff');
                    bFlip.val(" SHID Criteria >>");
                    txtShowB.val('0');
                } else {

                    divA.attr('class', 'DisplayOff');
                    divB.attr('class', 'DisplayOn');
                    bFlip.val("<< Factory Station Criteria ");
                    txtShowB.val('1');
                }

                BubbleOff(event);
                return false;
            
            });
        });                       
    
    </script>

    <asp:ScriptManager ID="mainScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="uplCriteria" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <div class="WarningOff" id="divAjaxWarning" runat="server"></div>        
            <table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="criteria">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="LabelText" style="text-align: right">
                        <asp:Button ID="btnFlip" runat="server" CssClass="BtnXLg" Text=" SHID Criteria &gt;&gt; " />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div class="DisplayOn" id="divStationCriteria" runat="server">
                            <table class="wideTable" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <th style="width: 1%">
                                    </th>
                                    <th style="width: 25%">
                                    </th>
                                    <th style="width: 25%">
                                    </th>
                                    <th style="width: 25%">
                                    </th>
                                    <th>
                                    </th>
                                </tr>
                                <tr style="vertical-align: top;">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="LabelText">
                                        Factories
                                    </td>
                                    <td class="LabelText">
                                        Stations
                                    </td>
                                    <td class="LabelText">
                                        Contracts
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="vertical-align: top;">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstFsFactory" CssClass="MedListPlus" runat="server" Rows="10" SelectionMode="Multiple"
                                            AutoPostBack="True" OnSelectedIndexChanged="lstFsFactory_SelectedIndexChanged">
                                        </asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstFsStation" CssClass="MedListPlus" runat="server" Rows="12" SelectionMode="Multiple"
                                            AutoPostBack="True" OnSelectedIndexChanged="lstFsStation_SelectedIndexChanged">
                                        </asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstFsContract" CssClass="MedList" runat="server" Rows="12" SelectionMode="Multiple">
                                        </asp:ListBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <asp:TextBox ID="txtFsDivSHIDShowing" CssClass="DisplayOff" runat="server"></asp:TextBox>
                        </div>
                        <div class="DisplayOff" id="divSHIDCriteria" runat="server">
                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                <tr>
                                    <td colspan="4">
                                        <div style="background-color: #990000; text-align: center">
                                            <span style="font-weight: bold; font-size: 11px; color: #ffffff">Single SHID</span>
                                            <span style="font-weight: bold; font-size: 11px; color: #990000">________</span>
                                            <span style="font-weight: bold; font-size: 11px; color: #ffffff">-- OR --</span>
                                            <span style="font-weight: bold; font-size: 11px; color: #990000">________</span>
                                            <span style="font-weight: bold; font-size: 11px; color: #ffffff">SHID Range</span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelText">
                                        SHID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFsSHID" CssClass="ButtonText" runat="server" Rows="5" TabIndex="5"></asp:TextBox>
                                    </td>
                                    <td class="LabelText">
                                        From SHID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFsFromSHID" CssClass="ButtonText" runat="server" Rows="5" TabIndex="6"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    </td>
                                    <td class="LabelText">
                                        To SHID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFsToSHID" CssClass="ButtonText" runat="server" Rows="5" TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
