<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="optContactInfo.aspx.cs" Inherits="WSCIEMP.SHS.optContactInfo" Title="Western Sugar Cooperative - Contact Information" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Contact Information
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table cellspacing="0" cellpadding="0">
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10px">
                &nbsp;
            </td>
            <td class="ButtonText">
                Below is the email and fax information on record. Please correct any missing or
                inaccurate information.<br />
            </td>
            <td style="width: 5px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10px">
                &nbsp;
            </td>
            <td>
                <table cellspacing="0" cellpadding="0" border="0" width="90%">
                    <tr><td colspan="3">&nbsp;</td></tr>
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
                            Name:&nbsp;&nbsp;&nbsp;
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
                            <asp:TextBox ID="txtEmail" runat="server" Columns="30"></asp:TextBox><span class="ButtonText">&nbsp;(Ex:
                                TSmith@Xyz.com)&nbsp;&nbsp;</span>
                        </td>
                        <td style="text-align:left">
                            <asp:Label ID="lblEmail" runat="server" CssClass="WarningOff"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="LabelText">
                            Fax:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" Columns="30"></asp:TextBox><span class="ButtonText">&nbsp;(Ex:
                                303-111-2222)&nbsp;&nbsp;</span>
                        </td>
                        <td style="text-align:left">
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
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <div style="text-align: center">
                    <asp:button id="btnSave" Runat="server" CssClass="BtnMed" Text="Save" onclick="btnSave_Click"></asp:button>
                </div>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <div>
        <table cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 150px">&nbsp;</td>
            </tr>
        </table>
    </div>
</asp:Content>
