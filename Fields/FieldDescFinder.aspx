<%@Page Language="C#" AutoEventWireup="true" StylesheetTheme="ThemeA_1024" CodeBehind="FieldDescFinder.aspx.cs" Inherits="WSCIEMP.Fields.FieldDescFinder" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Field Finder</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="FieldFinderContainer" class="copyTableNoVMenu" style="border-left: white 0px solid;" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td>
                    <table style="width:100%" border="0">
                        <tr>
                            <td class="pgTitle" style="width:85%;">
                                Field Finder
                            </td>
                            <td style="text-align:center;">
                                <input class="LabelText" id="btnCancel" onclick="DoCancel();" type="button" value="Cancel" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="margin: 0px; text-align: center">
                    <div class="WarningOn" id="divFFWarning" runat="server">
                    </div>
                </td>
            </tr>
            <tr>
                <td><div class="LabelText" style="text-align:left;">Select a Field</div>
                    <div class="ButtonText" id="temp" style="overflow: auto; width: 880px; height: 500px" runat="server"
                        name="temp">                        
                        <asp:GridView ID="grdResults" runat="server" AutoGenerateColumns="False" 
                            SkinID="grdColor" CssClass="grdColor1500" onrowcreated="grdResults_RowCreated" EnableViewState="false">
                            <Columns>
                                <asp:BoundField DataField="lld_lld_id" HeaderText="">
                                    <ItemStyle Width="0%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Lld_fsa_number" HeaderText="FSA Number"></asp:BoundField>
                                <asp:BoundField DataField="Lld_field_name" HeaderText="WSC Field Name"></asp:BoundField>
                                <asp:BoundField DataField="Lld_acres" HeaderText="Acres"></asp:BoundField>
                                <asp:BoundField DataField="Lld_fsa_official" HeaderText="FSA Official"></asp:BoundField>
                                <asp:BoundField DataField="Lld_contract_no" HeaderText="Contracted"></asp:BoundField>
                                <asp:BoundField DataField="Lld_state" HeaderText="State"></asp:BoundField>
                                <asp:BoundField DataField="Lld_county" HeaderText="County"></asp:BoundField>
                                <asp:BoundField DataField="Lld_township" HeaderText="Township"></asp:BoundField>
                                <asp:BoundField DataField="Lld_range" HeaderText="Range"></asp:BoundField>
                                <asp:BoundField DataField="Lld_section" HeaderText="Section"></asp:BoundField>
                                <asp:BoundField DataField="Lld_quadrant" HeaderText="Quadrant"></asp:BoundField>
                                <asp:BoundField DataField="Lld_quarter_quadrant" HeaderText="1/4 Quadrant"></asp:BoundField>
                                <asp:BoundField DataField="Lld_fsa_state" HeaderText="FSA State"></asp:BoundField>
                                <asp:BoundField DataField="Lld_fsa_county" HeaderText="FSA County"></asp:BoundField>
                                <asp:BoundField DataField="Lld_farm_number" HeaderText="Farm No"></asp:BoundField>
                                <asp:BoundField DataField="Lld_tract_number" HeaderText="Tract No"></asp:BoundField>
                                <asp:BoundField DataField="Lld_field_number" HeaderText="Field No"></asp:BoundField>
                                <asp:BoundField DataField="Lld_quarter_field" HeaderText="1/4 Field"></asp:BoundField>
                                <asp:BoundField DataField="Lld_latitude" HeaderText="Latitude"></asp:BoundField>
                                <asp:BoundField DataField="Lld_longitude" HeaderText="Longitude"></asp:BoundField>
                                <asp:BoundField DataField="Lld_description" HeaderText="Description"></asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate><div class="LabelText" style="text-align:left;">No Records Matched Your Criteria.</div></EmptyDataTemplate>
                        </asp:GridView></div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
