<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="ExportPerformance.aspx.cs" Inherits="WSCIEMP.Admin.ExportPerformance" Title="Admin Export Performance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin Export Performance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <table cellspacing="0" cellpadding="0" border="0" style="width:90%">
        <tr>
            <td style="width: 2%">
                &nbsp;
            </td>
            <td style="width: 12%">
                <span class="LabelText">Crop Year:</span>
            </td>
            <td style="width: 70%">
                <asp:DropDownList ID="ddlCropYear" TabIndex="0" runat="server" CssClass="ctlWidth60">
                </asp:DropDownList>
            </td>
            <td style="width: 15%">
                &nbsp;&nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="0" border="0" class="layoutAvg">
        <tr>
            <td style="width: 10px">
                &nbsp;
            </td>
            <td style="width: 500px">
                &nbsp;
            </td>
            <td style="width: 20px">
                &nbsp;
            </td>
        </tr>
        <tr style="line-height: 15px">
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <span style="font-weight: bold">One Step: Build, Clear and Export All Data</span><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportAll" runat="server" Text="Export All Performance Tables">
                </asp:RadioButton>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="line-height: 23px">
                <span style="font-weight: bold">Multiple Step: Build, Clear and Export Data in Parts (Execute each step in squence).</span><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportAgronomy" runat="server" Text="1. Export to Access Only Field Agronomy Table">
                </asp:RadioButton><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportContracting" runat="server" Text="2. Export to Access Only Field Contracting Table">
                </asp:RadioButton><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportReportCard" runat="server" Text="3. Build Only Grower Report Card Rankings in Beet Accounting Database">
                </asp:RadioButton><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportDeletePerformance" runat="server"
                    Text="4. Clear Export Field Performance Data in Access"></asp:RadioButton><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportDeleteDirt" runat="server" Text="5. Clear Export Dirt Data in Access">
                </asp:RadioButton><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportGenPerformance" runat="server"
                    Text="6. Build Field Performance Data in Beet Accounting"></asp:RadioButton><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportGenDirt" runat="server" Text="7. Build Dirt Data in Beet Accounting">
                </asp:RadioButton><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportPerformanceData" runat="server"
                    Text="8. Export to Access Field Performance Data"></asp:RadioButton><br />
                <asp:RadioButton GroupName="choiceX" ID="chkExportDirtData" runat="server" Text="9. Export to Access Dirt Data">
                </asp:RadioButton>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td style="width: 410px">
                &nbsp;
            </td>
            <td style="width: 110px">
                &nbsp;<asp:Button ID="btnExport" runat="server" CssClass="BtnMed" Text="Execute" 
                    onclick="btnExport_Click">
                </asp:Button>
            </td>
            <td style="width: 10px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
    </table>
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="txtUserID" runat="server"></asp:TextBox><asp:TextBox ID="txtAction"
            runat="server"></asp:TextBox></div>
</asp:Content>
