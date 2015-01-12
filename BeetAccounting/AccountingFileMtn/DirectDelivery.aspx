<%@ Page Title="BeetAccounting - Direct Deliveries" Language="C#" MasterPageFile="~/PrimaryTemplate.Master"
    StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="DirectDelivery.aspx.cs"
    Inherits="WSCIEMP.BeetAccounting.AccountingFileMtn.DirectDelivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    BeetAccounting - Direct Deliveries
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <script type="text/javascript">
        // Needed ???
    </script>
    <div class="PagePartWide" style="padding-left: 10px; width: 98%;">
        <br class="halfLine" />
        <table border="0" style="width: 98%;" id="selectionCriteria">
            <tr>
                <td style="width: 11%;">
                    <span class="LabelText">CropYear:</span><br />
                    <asp:DropDownList ID="ddlCropYear" CssClass="ctlWidth60" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlCropYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="width: 28%;">
                    <span class="LabelText">Delivery Station:</span><br />
                    <asp:DropDownList ID="ddlCriteriaDeliveryStation" CssClass="LgText" runat="server"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlCriteriaDeliveryStation_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="width: 28%;">
                    <span class="LabelText">Contract Station:</span><br />
                    <asp:DropDownList ID="ddlCriteriaContractStation" CssClass="LgText" runat="server"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlCriteriaContractStation_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="vertical-align: bottom;">
                    <div style="border: solid 1px #aaa; padding: 4px 0px 2px 0px; vertical-align: bottom;
                        text-align: center;">
                        <span class="LabelText">Contract: </span>&nbsp;&nbsp;<asp:TextBox ID="txtContractSearch"
                            CssClass="ctlWidth75" runat="server"></asp:TextBox>
                        &nbsp;&nbsp;<asp:Button ID="btnContractSearch" runat="server" Text="Search" CssClass="ButtonText"
                            OnClick="btnContractSearch_Click" />
                    </div>
                </td>
            </tr>
        </table>
        <br class="halfLine" />
    </div>
    <br />
    <span class="LabelText">Direct Deliveries</span>
    <div id="divDirectDeliveries" style="width: 99%;" class="PagePartWide">
        <br class="halfLine" />
        <table border="0" style="width: 99%;" id="tabDirectDelivery">
            <tr>
                <td style="width: 70%;">
                    <div>
                        <table class="fixHeader" style="width: 100%;">
                            <tr>
                                <th style="width: 39%; border-right: 1px solid #ffffff;">
                                    Delivery Station
                                </th>
                                <th style="width: 39%; border-right: 1px solid #ffffff;">
                                    Contract Station
                                </th>
                                <th>
                                    Rate Per Ton
                                </th>
                            </tr>
                        </table>
                    </div>
                    <div class="ButtonText" style="overflow: auto; height: 100px; border: solid 1px #FFFFFF;
                        text-align: center;">
                        <asp:GridView ID="grdDirectDelivery" runat="server" SkinID="grdColorNoHead" CellPadding="1"
                            AutoGenerateColumns="False" CssClass="grdColor920" Width="100%" OnSelectedIndexChanged="grdDirectDelivery_SelectedIndexChanged"
                            OnRowCreated="grdDirectDelivery_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="DirectDeliveryID" HeaderText="">
                                    <ItemStyle Width="0" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RowVersion" HeaderText="">
                                    <ItemStyle Width="0" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DeliveryStation" HeaderText="">
                                    <ItemStyle HorizontalAlign="Left" Width="40%"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ContractStation" HeaderText="">
                                    <ItemStyle HorizontalAlign="Left" Width="40%"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RatePerTon" HeaderText="">
                                    <ItemStyle HorizontalAlign="Center" Width="20%"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
                <td style="width: 20%">
                    &nbsp;
                </td>
                <td style="text-align: center; vertical-align: top;">
                    <asp:Button ID="btnDirectDeliveryAdd" runat="server" Text=" Add " CssClass="ButtonText"
                        OnClick="btnDirectDeliveryAdd_Click" /><br />
                    <br class="halfLine" />
                    <asp:Button ID="btnDirectDeliveryUpdate" runat="server" Text=" Update " CssClass="ButtonText"
                        OnClick="btnDirectDeliveryUpdate_Click" /><br />
                    <br class="halfLine" />
                    <asp:Button ID="btnDirectDeliveryDelete" runat="server" Text=" Delete" CssClass="ButtonText"
                        OnClick="btnDirectDeliveryDelete_Click" />
                </td>
            </tr>
        </table>
        <table border="0" style="width: 80%;" id="editDirectDelivery">
            <tr>
                <td style="width: 35%">
                    <span class="LabelText">Delivery Station:</span><br />
                    <asp:DropDownList ID="ddlEditDeliveryStation" CssClass="LgText" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="width: 35%">
                    <span class="LabelText">Contract Station:</span><br />
                    <asp:DropDownList ID="ddlEditContractStation" CssClass="LgText" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="width: 15%">
                    <span class="LabelText">Rate Per Ton:</span><br />
                    <asp:TextBox ID="txtEditRatePerTon" CssClass="ctlWidth75" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <br class="halfLine" />
    </div>
    <br />
    <span class="LabelText">Contract Overrides</span>
    <div id="divContractOverride" style="width: 99%;" class="PagePartWide">
        <br class="halfLine" />
        <table border="0" style="width: 99%;" id="tabContractOverride">
            <tr>
                <td style="width: 40%;">
                    <div>
                        <table class="fixHeader" style="width: 100%;">
                            <tr>
                                <th style="width: 60%; border-right: 1px solid #ffffff;">
                                    Contract
                                </th>
                                <th>
                                    Rate Per Ton
                                </th>
                            </tr>
                        </table>
                    </div>
                    <div class="ButtonText" style="overflow: auto; height: 100px; border: solid 1px #FFFFFF;
                        text-align: center;">
                        <asp:GridView ID="grdContract" runat="server" SkinID="grdColorNoHead" CellPadding="1"
                            AutoGenerateColumns="False" CssClass="grdColor920" Width="100%" OnSelectedIndexChanged="grdContract_SelectedIndexChanged"
                            OnRowCreated="grdContract_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="DirectDeliveryID" HeaderText="">
                                    <ItemStyle Width="0" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ContractID" HeaderText="">
                                    <ItemStyle Width="0" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RowVersion" HeaderText="">
                                    <ItemStyle Width="0" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ContractNumber" HeaderText="">
                                    <ItemStyle HorizontalAlign="Center" Width="60%"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RatePerTon" HeaderText="">
                                    <ItemStyle HorizontalAlign="Center" Width="40%"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
                <td style="width: 50%;">
                    &nbsp;
                </td>
                <td style="text-align: center; vertical-align: top;">
                    <asp:Button ID="btnOverrideAdd" runat="server" Text=" Add " CssClass="ButtonText"
                        OnClick="btnOverrideAdd_Click" /><br />
                    <br class="halfLine" />
                    <asp:Button ID="btnOverrideUpdate" runat="server" Text=" Update " CssClass="ButtonText"
                        OnClick="btnOverrideUpdate_Click" /><br />
                    <br class="halfLine" />
                    <asp:Button ID="btnOverrideDelete" runat="server" Text=" Delete" CssClass="ButtonText"
                        OnClick="btnOverrideDelete_Click" />
                </td>
            </tr>
        </table>
        <table border="0" style="width: 50%;" id="tabEditContract">
            <tr>
                <td style="width: 55%">
                    <span class="LabelText">Contract:</span><br />
                    <asp:DropDownList ID="ddlEditContractNumber" CssClass="LgText" runat="server" DataTextField="ContractNumber"
                        DataValueField="ContractID">
                    </asp:DropDownList>
                </td>
                <td style="width: 45%">
                    <span class="LabelText">Rate Per Ton:</span><br />
                    <asp:TextBox ID="txtEditContractRatePerTon" CssClass="ctlWidth75" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br class="halfLine" />
    </div>
    <br />
    <br />
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
