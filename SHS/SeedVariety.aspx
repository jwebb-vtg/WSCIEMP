<%@ Page Title="" Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="SeedVariety.aspx.cs" Inherits="WSCIEMP.SHS.SeedVariety" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Manage Seed Varieties for Agronomy
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <div style="text-align:center">
        <asp:Label ID="lblNewUser" runat="server" CssClass="WarningOff"></asp:Label></div>
<div style="text-align:center"><br />  
      
    <table cellspacing="0" cellpadding="0" border="0" style="width:100%;">
        <tr>
            <th style="width:5%"></th>
            <th style="width:20%"></th>
            <th style="width:15%"></th>
            <th style="width:40%"></th>
            <th style="width:20%"></th>
        </tr>
        <tr>
            <td rowspan="6">&nbsp;</td>
            <td colspan="4" style="text-align:left;">
                <span class="LabelText">Factory Area</span>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlFactoryArea" runat="server" AutoPostBack="True" cssClass="BtnMed"
                    onselectedindexchanged="ddlFactoryArea_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="4">&nbsp;</td>
        </tr>
        <tr>
            <td rowspan="4" colspan="2" style="vertical-align:top">
                <asp:GridView ID="grdResults" runat="server" SkinID="grdColor" 
                    CssClass="grdColor325" AutoGenerateColumns="False" 
                    onselectedindexchanged="grdResults_SelectedIndexChanged" EmptyDataText="No Seed Variety found for this Factory Area.">
                    <Columns>
                        <asp:BoundField DataField="svtyVariety" HeaderText="Seed Variety">
                            <HeaderStyle Width="100%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>            
            </td>
            <td colspan="2" style="vertical-align:top; padding-right:10px;">
                <div class="highlightPlus" style="padding-left:5px; padding: 5px 10px 5px 10px; text-align:left;">
                    To Add a new variety, enter the name of a new Seed Variety and press Add.
                    To Delete a Seed Variety, select a variety from the Seed Variety listing and press Delete.  In order to
                    Update a Seed Variety name, in the case of a typo, please Delete that variety and then Add it with the correct spelling.            
                </div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top;">
                <span class="LabelText">Variety Name</span><br />
                <asp:TextBox ID="txtVarietyName" CssClass="LgText" runat="server"></asp:TextBox>
            </td>
            <td style="vertical-align:top;">
                <asp:Button ID="btnAdd" runat="server" CssClass="BtnMed" Text="Add" 
                    onclick="btnAdd_Click"></asp:Button><br /><br />
                <asp:Button ID="btnDelete" runat="server" CssClass="BtnMed" Text="Delete" 
                    onclick="btnDelete_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="2"><br />&nbsp;<br /></td>
        </tr>
        <tr>
            <td colspan="2"><br />&nbsp;<br /></td>
        </tr>
        <tr><td colspan="5">&nbsp;</td></tr>
        <tr>
            <td colspan="5" style="line-height:30px;">
                &nbsp;
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
