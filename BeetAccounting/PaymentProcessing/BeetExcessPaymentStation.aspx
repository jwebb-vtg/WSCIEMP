<%@ Page Title="BeetAccounting - Station Excess Beet Percent" Language="C#" MasterPageFile="~/PrimaryTemplate.Master"
    StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="BeetExcessPaymentStation.aspx.cs"
    Inherits="WSCIEMP.BeetAccounting.PaymentProcessing.BeetExcessPaymentStation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    BeetAccounting - Station Excess Beet Percent
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <div class="PagePartWide" style="padding-left: 10px; width: 98%;">
        <div class="ButtonText">
            <ol>
            <li>Select criteria and Search.  The records you want to affect MUST be in the grid.</li>
            <li>Set the Excess Percentage and press Save.</li>
            <li>If you change the criteria, by selecting a contract, you must SEARCH again.</li>
            </ol>

        </div>
        <table border="0" style="width: 100%;">
            <tr>
                <td>
                    <span class="LabelText">CropYear:</span>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlCropYear" CssClass="ctlWidth60" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlCropYear_SelectedIndexChanged">
                    </asp:DropDownList>                                
                </td>
                <td>&nbsp;</td>
                <td style="text-align: center; border-left: 1px solid #000000; border-top: 1px solid #000000; border-right: 1px solid #000000;">
                    <span class="LabelText">SHID:</span>&nbsp;&nbsp;
                    <asp:TextBox ID="txtSHID" runat="server" CssClass="ctlWidth60"></asp:TextBox>    
                </td>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:center;">
                    <span class="LabelText">Factory:</span>
                </td>
                <td style="text-align:center;">
                    <span class="LabelText">Station:</span>
                </td>
                <td style="text-align: center; border-left: 1px solid #000000; border-right: 1px solid #000000;">
                    <span class="LabelText">Contract:</span>
                </td>
                <td style="text-align:center;">
                    <span class="LabelText">Payment:</span>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr style="vertical-align: top;">
                <td style="text-align:center; padding-bottom: 5px;">
                    <asp:ListBox ID="lstFactory" runat="server" Rows="14" Width="130" AutoPostBack="True" 
                        CssClass="ButtonText" onselectedindexchanged="lstFactory_SelectedIndexChanged">
                    </asp:ListBox>
                </td>
                <td style="text-align:center; padding-bottom: 5px;">
                    <asp:ListBox ID="lstStation" runat="server" Rows="14" Width="130" 
                        CssClass="ButtonText" SelectionMode="Multiple" AutoPostBack="True" 
                        onselectedindexchanged="lstStation_SelectedIndexChanged">
                    </asp:ListBox>
                </td>
                <td style="text-align: center; border-left: 1px solid #000000; border-bottom: 1px solid #000000; border-right: 1px solid #000000; padding-bottom: 5px;">
                    <asp:ListBox ID="lstContract" runat="server" Rows="14" Width="130" 
                        CssClass="ButtonText" SelectionMode="Multiple">
                    </asp:ListBox>
                </td>
                <td style="text-align:center; padding-bottom: 5px;">
                    <asp:ListBox ID="lstPayment" runat="server" Rows="14" Width="130" CssClass="ButtonText" SelectionMode="Multiple">
                    </asp:ListBox>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ButtonText"
                        onclick="btnSearch_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    <br />
    <div>
        <div style="text-align:right; vertical-align:top; padding-bottom:6px;">
            <asp:Label ID="lbl001" runat="server" Text="Excess Percent: " CssClass="LabelText"></asp:Label>
            <asp:TextBox ID="txtExcessPct" Width="70" CssClass="ButtonText" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSaveExcessPct" runat="server" Text="Save" 
                CssClass="ButtonText" onclick="btnSaveExcessPct_Click" />
        </div>
        <asp:GridView ID="grdPayDescStation" runat="server" SkinID="grdColor" CellPadding="1"
            AutoGenerateColumns="False" CssClass="grdColor1500" Width="100%" 
            OnRowCreated="grdPayDescStation_RowCreated">
            <Columns>
                <asp:BoundField DataField="pdecnt_description_contract_id" HeaderText="">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_description_id" HeaderText="">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_factory_id" HeaderText="">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_station_id" HeaderText="">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_contract_id" HeaderText="">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_rowversion" HeaderText="">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_factory_friendly_name" HeaderText="Factory">
                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_station_friendly_name" HeaderText="Station">
                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_grower_no" HeaderText="SHID">
                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_grower_name" HeaderText="Grower Name">
                    <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_contract_no" HeaderText="Contract">
                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_payment_friendly_name" HeaderText="Payment">
                    <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="pdecnt_excess_beet_pct" HeaderText="Excess Pct">
                    <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>   
    </div>
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
