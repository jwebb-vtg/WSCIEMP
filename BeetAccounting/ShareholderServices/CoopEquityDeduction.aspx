<%@ Page Title="Western Sugar Cooperative - Coop Equity Deduction" Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" 
AutoEventWireup="true" CodeBehind="CoopEquityDeduction.aspx.cs" Inherits="WSCIEMP.BeetAccounting.ShareholderServices.CoopEquityDeduction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    BeetAccounting - Coop Equity Deduction
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <br />
    <span class="LabelText">Equity Deduction Edit</span>
    <div style="border:solid 1px black;width:99%;" id="factoryEdit">
        <br class="halfLine" />
        <table border="0" style="width: 98%;">
            <tr style="line-height:28px;">
                <td style="width: 15%;"><span class="LabelText">Deduction Number: </span></td>
                <td style="width: 72%;"><asp:TextBox ID="txtEDedNumber" CssClass="ButtonText" Width="90" runat="server"></asp:TextBox></td>
                <td style="text-align:center;"><asp:Button ID="btnAdd" runat="server" Text=" Add " CssClass="LabelText" onclick="btnAdd_Click" /></td>
            </tr>
            <tr style="line-height:28px;">
                <td><span class="LabelText">Is Active: </span></td>
                <td><asp:CheckBox ID="chkEDedIsActive" runat="server" /></td>
                <td style="text-align:center;"><asp:Button ID="btnUpdate" runat="server" Text=" Update " CssClass="LabelText" onclick="btnUpdate_Click" /></td>
            </tr>
            <tr style="line-height:28px;">
                <td><span class="LabelText">Description: </span></td>
                <td><asp:TextBox ID="txtEDedDescription" CssClass="ButtonText" Width="640" runat="server"></asp:TextBox></td>
                <td style="text-align:center;"><asp:Button ID="btnDelete" runat="server" Text=" Delete " CssClass="LabelText" onclick="btnDelete_Click" /></td>
            </tr>
        </table>
        <br class="halfLine" />
    </div>
    <br />    
    <span class="LabelText">Equity Deductions</span>
    <div>
        <asp:GridView ID="grdEqDeduction" runat="server" SkinID="grdColor" 
            CellPadding="1" AutoGenerateColumns="False" CssClass="grdColor920"  
            width="99%" onselectedindexchanged="grdEqDeduction_SelectedIndexChanged" 
            onrowcreated="grdEqDeduction_RowCreated">
            <Columns>
                <asp:BoundField DataField="EquityDeductionID" HeaderText="EquityDeductionID">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="RowVersion" HeaderText="RowVersion">
                    <ItemStyle Width="0" />
                </asp:BoundField>                
                <asp:BoundField DataField="DeductionNumber" HeaderText="Deduction Number">
                    <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeductionDescription" HeaderText="Description">
                    <HeaderStyle HorizontalAlign="Center" Width="65%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="IsActive" HeaderText="Is Active">
                    <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>           
            </Columns>
        </asp:GridView>
    </div>
    <br />
<br />
<br />
<br />
<br />
<br />
<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
