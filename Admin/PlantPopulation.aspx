<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="PlantPopulation.aspx.cs" Inherits="WSCIEMP.Admin.PlantPopulation" Title="Admin Plant Population" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin Plant Population
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <div style="text-align:center">
        <asp:Label ID="lblNewUser" runat="server" CssClass="WarningOff"></asp:Label></div>
<div style="text-align:center"><br />        
    <table cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td style="width: 10px">
                &nbsp;
            </td>
            <td style="width: 100px">
                <span class="LabelText">Crop Year:</span>
            </td>
            <td style="width: 350px; text-align:left;">
                <asp:DropDownList ID="ddlCropYear" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="ddlCropYear_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width: 60px; text-align: left;">&nbsp;
            </td>
            <td style="width: 10px">
                &nbsp;
            </td>
        </tr>
        <tr><td colspan="5">&nbsp;</td></tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="3">
                <asp:GridView ID="grdResults" runat="server" SkinID="grdColor" 
                    CssClass="grdColor600" AutoGenerateColumns="False" 
                    onselectedindexchanged="grdResults_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="plpPlantPopulationID" HeaderText="">
                            <HeaderStyle CssClass="DisplayOff" ></HeaderStyle>
                            <ItemStyle CssClass="DisplayOff" />
                        </asp:BoundField>
                        <asp:BoundField DataField="plpRowWidth" HeaderText="Row Width">
                            <HeaderStyle Width="30%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="plpBPAFactor" HeaderText="Beets Per Acre Factor">
                            <HeaderStyle Width="35%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="plpStandFactor" HeaderText="Stand Factor">
                            <HeaderStyle Width="35%" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
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
            <td>
                &nbsp;
            </td>
            <td style="text-align: left;">
                <span class="LabelText">Row Width:</span>
            </td>
            <td>
                <asp:TextBox ID="txtRowWidth" CssClass="LgText" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnNew" runat="server" CssClass="BtnMed" Text="New " 
                    onclick="btnNew_Click"></asp:Button>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="text-align: left;">
                <span class="LabelText">BPA Factor:</span>
            </td>
            <td>
                <asp:TextBox ID="txtBPAFactor" CssClass="LgText" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" CssClass="BtnMed" Text="Save" 
                    onclick="btnSave_Click"></asp:Button>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="text-align: left;">
                <span class="LabelText">Stand Factor:</span>
            </td>
            <td>
                <asp:TextBox ID="txtStandFactor" CssClass="LgText" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnDelete" runat="server" CssClass="BtnMed" Text="Delete" 
                    onclick="btnDelete_Click"></asp:Button>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5" style="line-height:150px;">
                &nbsp;
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
