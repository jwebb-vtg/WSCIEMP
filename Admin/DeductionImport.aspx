<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="DeductionImport.aspx.cs" Inherits="WSCIEMP.Admin.DeductionImport" Title="Admin Automatic Import for Grower Deductions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin Automatic Import for Grower Deductions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" style="width:98%;">
        <tr>
            <td width="1%">
                &nbsp;
            </td>
            <td width="*">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td width="15%">
                            &nbsp;
                        </td>
                        <td width="42.5%">
                            &nbsp;
                        </td>
                        <td width="14%">
                            &nbsp;
                        </td>
                        <td width="*">
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td class="LabelText">
                            Crop Year:
                        </td>
                        <td class="LabelText">
                            Please select an Excel file to Upload*</td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="height: 33px">
                        <td class="LabelText">
                            <asp:DropDownList ID="ddlCropYear" runat="server" CssClass="ctlWidth60">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">                            
                            <asp:FileUpload ID="uploadFile" runat="server" CssClass="LabelText" Width="220px" />
                        </td>
                        <td>
                            <asp:Button ID="btnUpload" runat="server" CssClass="BtnLg" Text="Upload File" 
                                onclick="btnUpload_Click">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr style="height: 33px">
                        <td colspan="3" rowspan="2">
                            <span class="WarningOn" style="text-align:left">Excel workbook must have the first tab named "Deductions".</span>  
                            <span  class="WarningOn" style="text-align:left">This tab must show a spreadsheet with the following columns in order:</span>
                            <span  class="WarningOn" style="text-align:left">"Contract", "Amount", and "Deduction" 
                            </span>
                        </td>
                        <td>
                            <asp:Button ID="btnPost" runat="server" CssClass="BtnLg" Text="Post Deductions" 
                                onclick="btnPost_Click">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr style="height: 33px">
                        <td>
                            <asp:Button ID="btnDelete" runat="server" CssClass="BtnLg" 
                                Text="Delete Deductions" onclick="btnDelete_Click">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="height: 150px">
                        <td colspan="4">
                            <asp:Label ID="lblOrigPath" runat="server" CssClass="LabelText"></asp:Label><br />
                            <asp:GridView ID="grdResults" runat="server" SkinID="grdColor" CssClass="grdColor600" AutoGenerateColumns="False" EnableViewState="False">
                                <Columns>
                                    <asp:BoundField DataField="Contract" HeaderText="Contract">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:0.00}">
                                        <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Deduction" HeaderText="Deduction">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QualifyDeduction" HeaderText="QA">
                                        <HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
