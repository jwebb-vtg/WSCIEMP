<%@ Page Title="Western Sugar Cooperative - Coop Over Plant Settings" Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="CoopSettings.aspx.cs" Inherits="WSCIEMP.BeetAccounting.ShareholderServices.CoopSettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Beet Accounting - Coop Over Plant Settings
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript">

        $(document).ready(function() {

            //-----------------------------------------
            // Setup calendar
            //-----------------------------------------
            $$('txtCutoffDate', $('#factoryEdit')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
                buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+1',
                changeMonth: true, changeYear: true, duration: 'fast'
            });
        });
        
    </script>
    <div class="PagePartWide" style="padding-left:10px; width:97%;">
    <br class="halfLine" />
    <table border="0" style="width: 25%;">
           <tr>
            <td style="width:40%;"><span class="LabelText">CropYear:</span></td>
            <td style="width:60%;"><asp:DropDownList ID="ddlCropYear" CssClass="ctlWidth60" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlCropYear_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <span class="LabelText">Over Plant Factory Details</span>
    <div>
        <asp:GridView ID="grdFactory" runat="server" SkinID="grdColor" CellPadding="1" AutoGenerateColumns="False" CssClass="grdColor920"  
            width="99%" onselectedindexchanged="grdFactory_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="FactoryNum" HeaderText="FactoryNum">
                    <HeaderStyle CssClass="DisplayOff" />
                    <ItemStyle CssClass="DisplayOff" />
                </asp:BoundField>
                <asp:BoundField DataField="FactoryName" HeaderText="Factory">
                    <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="IsOverPlantAllowed" HeaderText="Over-Plant Allowed">
                    <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Percentage" HeaderText="Percentage">
                    <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="IsPoolingAllowed" HeaderText="Pooling Allowed">
                    <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="PoolSHID" HeaderText="Pool SHID">
                    <HeaderStyle HorizontalAlign="Center" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CutoffDate" HeaderText="Cutoff Date">
                    <HeaderStyle HorizontalAlign="Center" Width="17%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="IsPosted" HeaderText="Posted">
                    <HeaderStyle HorizontalAlign="Center" Width="11%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>                
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <span class="LabelText">Factory Edit</span>
    <div style="border:solid 1px black;width:99%;" id="factoryEdit">
        <br />
        <table border="0" style="width: 98%;">
            <tr style="line-height:28px;">
                <td style="width:22%;"><span class="LabelText">Over Plant Allowed?</span></td>
                <td style="width:28%;"><asp:CheckBox ID="chkIsOverPlantAllowed" runat="server" /></td>
                <td style="width:16%;"><span class="LabelText">Percentage</span></td>
                <td style="width:24%;"><asp:TextBox ID="txtPercentage" CssClass="ButtonText" runat="server"></asp:TextBox></td>
                <td style="width:10%;text-align:right;"><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="LabelText" 
                        onclick="btnSave_Click" /></td>
            </tr>
            <tr style="line-height:28px;">
                <td><span class="LabelText">Allow Pooling?</span></td>
                <td><asp:CheckBox ID="chkIsPoolingAllowed" runat="server" /></td>
                <td><span class="LabelText">Pool SHID</span></td>
                <td><asp:TextBox ID="txtPoolSHID" CssClass="ButtonText" runat="server" ReadOnly="true"></asp:TextBox></td>
                <td style="text-align:right;"><asp:Button ID="btnPost" runat="server" Text="Post" CssClass="LabelText" 
                        onclick="btnPost_Click" />
                </td>
            </tr>
            <tr style="line-height:28px;">
                <td><span class="LabelText">Cutoff Date</span></td>
                <td colspan="4"><asp:TextBox ID="txtCutoffDate" CssClass="ButtonText" runat="server"></asp:TextBox></td>
            </tr>                        
        </table>
        <br />
    </div>
    <br />
</div> 
<br />
<br />
<br />
<br />
<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
