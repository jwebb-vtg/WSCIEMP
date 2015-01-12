<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="Security.aspx.cs" Inherits="WSCIEMP.Admin.Security"
    Title="Admin Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin Security
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script language="javascript" type="text/javascript">
        //<![CDATA[

        $(document).ready(function() {

            $("#tabs").tabs();

            if ($$("txtActiveTab", $("#divHelper")).val() == "USER") {
                $('#tabs').tabs("select", 0);
            } else {
                $('#tabs').tabs("select", 1);
            }

            $('#tabs').bind('tabsselect', function(event, ui) {

                if (ui.index == 0) {
                    $$("txtActiveTab", $("#divHelper")).val("USER");
                } else {
                    $$("txtActiveTab", $("#divHelper")).val("FACTORY");
                }

            });
        });

        //]]>
    </script>

    <div id="tabs" style="width: 98%; background-color: White;">
        <ul>                       
            <li><a href="#tabUser">User</a></li>
            <li><a href="#tabFactory">Factory</a></li>
        </ul>
        <div id="tabUser" style="background-color: #ffff99">
            <table style="width: 100%" cellspacing="0" cellpadding="3" border="0">
                <tr>
                    <th style="width: 20%">
                    </th>
                    <th style="width: 20%">
                    </th>
                    <th style="width: 20%">
                    </th>
                    <th>
                    </th>
                </tr>
                <tr>
                    <td style="background-color: #ccff99; text-align: center" colspan="4">
                        <span class="LabelText">Search Criteria</span>
                    </td>
                </tr>
                <tr>
                    <td class="LabelText" style="text-align: left;">
                        Login
                    </td>
                    <td class="LabelText" style="text-align: left;">
                        Role
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        &nbsp;<asp:TextBox ID="txtUserSearchLogin" runat="server" CssClass="ButtonText" Columns="20"></asp:TextBox>
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList CssClass="ButtonText" ID="ddlUserSearchRole" runat="server" Width="160">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left;">
                        <asp:CheckBox ID="chkUserSearchShowInActive" TabIndex="0" runat="server" CssClass="LabelText"
                            Text="Show In-Active?" TextAlign="Left"></asp:CheckBox>
                    </td>
                    <td style="text-align: left;">
                        <asp:Button ID="btnUserSearch" runat="server" CssClass="BtnMed" Text="Search" OnClick="btnUserSearch_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table>
            <br style="line-height: 6px" />
            <table cellspacing="0" cellpadding="0" style="width: 100%;" border="0">
                <tr>
                    <td colspan="2" class="LabelText" style="text-align: left">
                        &nbsp;User Search Results
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="overflow: auto; width: 940px; height: 200px;">
                            <asp:GridView ID="grdUserResults" SkinID="grdColor" runat="server" CssClass="grdColor900"
                                AutoGenerateColumns="False" OnSelectedIndexChanged="grdUserResults_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="usr_user_id" HeaderText="usr_user_id">
                                        <HeaderStyle CssClass="DisplayOff" />
                                        <ItemStyle CssClass="DisplayOff" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="usr_login_name" HeaderText="Login Name">
                                        <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="usr_display_name" HeaderText="Display Name">
                                        <HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="usr_user_active" HeaderText="Is Active">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="usr_phone_number" HeaderText="Phone">
                                        <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td style="text-align: left;">
                        <br />
                        <br style="line-height: 12px" />
                    </td>
                </tr>
            </table>
            <br style="line-height: 6px" />
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="background-color: #ccff99; text-align: center">
                        <span class="LabelText">Edit User</span>
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr style="line-height: 22px; text-align: left;">
                    <td style="width: 20%;">
                        <span class="LabelText">Role</span>
                    </td>
                    <td style="width: 20%;">
                        <span class="LabelText">Region</span>
                    </td>
                    <td style="width: 20%;">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr style="vertical-align: top; text-align: left;">
                    <td>
                        <asp:ListBox ID="lstUserEditRole" runat="server" CssClass="ButtonText" Width="160"
                            Rows="8" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td>
                        <asp:ListBox ID="lstUserEditRegion" runat="server" CssClass="ButtonText" Width="160"
                            Rows="4" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkUserEditIsActive" TabIndex="0" runat="server" CssClass="LabelText"
                            Text="Is Active?" TextAlign="Left"></asp:CheckBox>
                    </td>
                    <td>
                        <asp:Button ID="btnUserEditSave" runat="server" CssClass="BtnMed" Text="Save" OnClick="btnUserEditSave_Click">
                        </asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabFactory" style="background-color: #ffff99">
            <table style="width: 100%" cellspacing="0" cellpadding="3" border="0">
                <tr>
                    <th style="width: 30%;">
                    </th>
                    <th style="width: 30%;">
                    </th>
                    <th style="width: 20%;">
                    </th>
                    <th>
                    </th>
                </tr>
                <tr>
                    <td style="background-color: #ccff99; text-align: center" colspan="4">
                        <span class="LabelText">Factory Security</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td class="LabelText">
                        Region
                    </td>
                    <td class="LabelText">
                        Factory
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td>
                        <asp:DropDownList CssClass="ButtonText" ID="ddlFactoryRegion" runat="server" Width="160"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlFactoryRegion_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <div class="ButtonText" style="margin-left: 8px; margin-right: 16px; background-color: #ffff66">
                            Factories are related to region by factory number. If two factories share a number,
                            they will be related to the same region.
                            <br />
                            ONLY associate a factory to one region.<br />
                            Do Not associate any factory to All Coop.</div>
                    </td>
                    <td>
                        <asp:ListBox ID="lstFactory" runat="server" CssClass="ButtonText" Width="180" Rows="12"
                            SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td>
                        <asp:Button ID="btnFactorySave" runat="server" CssClass="BtnMed" Text="Save" OnClick="btnFactorySave_Click">
                        </asp:Button>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="line-height: 100px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table style="width: 99%" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td style="margin: 0px; text-align: center">
                <div class="DisplayOn" id="divUser" style="background-color: #ffff99">
                </div>
                <div class="DisplayOff" id="divFactory" style="background-color: #ffff99">
                </div>
            </td>
        </tr>
    </table>
    <div id="divHelper" class="DisplayOff">
        <asp:TextBox ID="txtActiveTab" runat="server"></asp:TextBox></div>
</asp:Content>
