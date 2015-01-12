<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="optChangePswd.aspx.cs" Inherits="WSCIEMP.SHS.optChangePswd" Title="Western Sugar Cooperative - Reset Password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Reset Password
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table cellspacing="0" cellpadding="0">
        <tr>
            <td style="width: 10px">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td style="width: 5px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <table style="width: 600px" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td class="LabelText" style="width: 15%">
                            SHID:
                        </td>
                        <td>
                            <asp:textbox id="txtSHID" Runat="server" CssClass="ButtonText" Columns="12"></asp:textbox>&nbsp;&nbsp;&nbsp;
                            <asp:button id="btnFind" Runat="server" CssClass="BtnMed" Text="Find" onclick="btnFind_Click"></asp:button>
                        </td>
                    </tr>
                    <tr>
                        <td class="LabelText">
                            Name:
                        </td>
                        <td>
                            <asp:Label ID="lblBusName" runat="server" CssClass="ButtonText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="LabelText">
                            Email:
                        </td>
                        <td>
                            <asp:Label ID="lblEmail" runat="server" CssClass="ButtonText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;" colspan="2" height="22">
                            <div style="text-align: center" class="ButtonText">
                                In order to reset the member's password, there must be a valid email address on
                                record. If there is not a valid email address above, please use the 
                                <span class="LabelText">
                                    Contact Info</span> menu option to fix this. After you press <span class="LabelText">
                                        Reset Password</span> the system will email the member a new password.<br />
                                <br />
                                <asp:Button ID="btnChange" runat="server" CssClass="BtnXLg" 
                                    Text="Reset Password" onclick="btnChange_Click">
                                </asp:Button></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;" text-align="center" colspan="2" height="22">&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <div>
        <table cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 130px">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
