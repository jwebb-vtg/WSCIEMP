<%@ Page Title="Master Reports - Field Agronomy" Language="C#" MasterPageFile="~/MasterReportTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="FieldAgronomy.aspx.cs" Inherits="WSCIEMP.Reports.MReports.FieldAgronomy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports - Field Agronomy
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <asp:ScriptManager ID="mainScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="uplCriteria" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <div class="WarningOff" id="divAjaxWarning" runat="server"></div>
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
                <tr>
                    <td colspan="5">
                        &nbsp;
                    </td>
                </tr>
                <tr style="vertical-align: bottom;">
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
                        <asp:ListBox ID="lstFaFactory" CssClass="MedListPlus" runat="server" Rows="10" SelectionMode="Multiple"
                            AutoPostBack="True" 
                            onselectedindexchanged="lstFaFactory_SelectedIndexChanged">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="lstFaStation" CssClass="MedListPlus" runat="server" Rows="12" SelectionMode="Multiple"
                            AutoPostBack="True" 
                            onselectedindexchanged="lstFaStation_SelectedIndexChanged">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="lstFaContract" CssClass="MedList" runat="server" Rows="12" SelectionMode="Multiple">
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
                <tr>
                    <td colspan="5" style="line-height: 100px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
