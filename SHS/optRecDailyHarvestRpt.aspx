<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="optRecDailyHarvestRpt.aspx.cs" Inherits="WSCIEMP.SHS.optRecDailyHarvestRpt" Title="Western Sugar Cooperative - Daily Harvest Report Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Daily Harvest Report Settings
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table cellspacing="0" cellpadding="0">
        <tr>
            <td style="width: 10px">
                &nbsp;
            </td>
            <td>
                <table cellspacing="0" cellpadding="0" border="0" width="90%">
                    <tr>
                        <td style="width:15%">
                            <span class="LabelText">Crop Year:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        </td>
                        <td style="width:40%">
                            <asp:dropdownlist id="ddlCropYear" runat="server" CssClass="ctlWidth60" AutoPostBack="true" onselectedindexchanged="ddlCropYear_SelectedIndexChanged"></asp:dropdownlist>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="LabelText">
                            SHID:
                        </td>
                        <td>
                            <asp:textbox id="txtSHID" Runat="server" CssClass="ButtonText" Columns="12"></asp:textbox>&nbsp;&nbsp;&nbsp;
                            <asp:button id="btnFind" Runat="server" CssClass="BtnMed" Text="Find" onclick="btnFind_Click"></asp:button>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="LabelText">
                            Name:
                        </td>
                        <td>
                            <asp:Label ID="lblBusName" runat="server" CssClass="ButtonText"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr><td colspan="3">&nbsp;</td></tr>
                    <tr>
                        <td class="LabelText">
                            Email:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Columns="18"></asp:TextBox><span class="ButtonText">&nbsp;(Ex:
                                TSmith@Xyz.com)&nbsp;&nbsp;</span>
                        </td>
                        <td>
                            <asp:Label ID="lblEmail" runat="server" CssClass="WarningOff"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="LabelText">
                            Fax:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" Columns="18"></asp:TextBox><span class="ButtonText">&nbsp;(Ex:
                                303-111-2222)&nbsp;&nbsp;</span>
                        </td>
                        <td>
                            <asp:Label ID="lblFax" runat="server" CssClass="WarningOff"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 5px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10px">
                &nbsp;
            </td>
            <td class="ButtonText"><br />
                Above is your email and fax information on record. Please update this information
                if it's missing or not accurate. In order to have Daily Harvest Reports delivered
                by email or fax, we must have a valid email address or fax number on record.
            </td>
            <td style="width: 5px">
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
                <span class="SectHdg">Daily Harvest Report</span>
                <br /><span class="ButtonText">
                Please indicate how you would like to receive or view your Daily Harvest Report:</span>
                <p class="Indent">
                    <asp:RadioButton class="optionList" ID="radDHRIntranetOnly" runat="server" GroupName="DailyHarvestReport"
                        Text="Internet Only -- view Daily Haravest Report on this website."></asp:RadioButton><br />
                    <asp:RadioButton class="optionList" ID="radDHREmail" runat="server" GroupName="DailyHarvestReport"
                        Text="Receive the Daily Harvest Report via email."></asp:RadioButton><br />
                    <asp:RadioButton class="optionList" ID="radDHRFax" runat="server" GroupName="DailyHarvestReport"
                        Text="Receive the Daily Harvest Report via fax."></asp:RadioButton><br />
                    <asp:RadioButton class="optionList" ID="radDHRRegMail" runat="server" GroupName="DailyHarvestReport"
                        Text="Receive a hard copy of the Daily Harvest Report in the mail."></asp:RadioButton><br />
                    <br />
                </p>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div style="text-align: center">
                    <asp:button id="btnSave" Runat="server" CssClass="BtnMed" Text=" Save " 
                        onclick="btnSave_Click"></asp:button>
                </div>
            </td>
        </tr>
    </table>
    <div>
        <table cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
