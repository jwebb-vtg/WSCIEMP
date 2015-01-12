<%@ Page Title="Master Reports - Landowner Letter" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="LandownerLetter.aspx.cs" Inherits="WSCIEMP.Reports.MReports.LandownerLetter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Landowner Letter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script language="javascript" type="text/javascript">
        //<![CDATA[

        $(document).ready(function() {

            //-----------------------------------------
            // Setup calendar
            //-----------------------------------------
            $$('txtLlLetterDate', $('#selectCriteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
                buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: true, yearRange: '-10:+1',
                changeMonth: true, changeYear: true, duration: 'fast'
            });
            $$('txtLlDeadlineDate', $('#selectCriteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
                buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: true, yearRange: '-10:+1',
                changeMonth: true, changeYear: true, duration: 'fast'
            });
        });

        //]]>					
    </script>

    <asp:ScriptManager ID="mainScriptManager" runat="server">
    </asp:ScriptManager>
    <table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="selectCriteria">
        <tr>
            <td>
                <table class="wideTable" cellspacing="0" cellpadding="0" border="0" id="Table2">
                    <tr>
                        <td style="width: 2%;">
                            &nbsp;
                        </td>
                        <td>
                            <span class="LabelText">Letter Date:</span><br />
                            <asp:TextBox ID="txtLlLetterDate" CssClass="ButtonText" runat="server" Rows="5"></asp:TextBox>
                            <br />
                            <br />
                            <span class="LabelText">Deadline Date:</span><br />
                            <asp:TextBox ID="txtLlDeadlineDate" CssClass="ButtonText" runat="server" Rows="5"></asp:TextBox>
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
                                    <asp:ListBox ID="lstLlFactory" CssClass="MedListPlus" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="lstLlFactory_SelectedIndexChanged" Rows="15"></asp:ListBox>
                                </td>
                                <td>
                                    <asp:ListBox ID="lstLlStation" CssClass="MedListPlus" runat="server" SelectionMode="Multiple"
                                        Rows="15" AutoPostBack="True" OnSelectedIndexChanged="lstLlStation_SelectedIndexChanged">
                                    </asp:ListBox>
                                </td>
                                <td>
                                    <asp:ListBox ID="lstLlContract" CssClass="MedList" runat="server" SelectionMode="Multiple"
                                        Rows="15"></asp:ListBox>
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
