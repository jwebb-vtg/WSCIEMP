<%@ Page Title="Passthrough Management" Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" 
AutoEventWireup="true" CodeBehind="PassthroughManagement.aspx.cs" Inherits="WSCIEMP.Admin.PassthroughManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Admin Coop Passthrough Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript">

    $(document).ready(function() {

        //-----------------------------------------
        // Setup calendar
        //-----------------------------------------
        $$('txtReportDate', $('#bodyTable')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+2',
            changeMonth: true, changeYear: true, duration: 'fast'
        });

        $$('txtFiscalYearEndDate', $('#bodyTable')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+2',
            changeMonth: true, changeYear: true, duration: 'fast'
        });
    });

    </script>
    <br />
    <table cellspacing="0" cellpadding="0" border="0" style="width: 80%;" id="bodyTable">
        <tr>
            <td>
                <span class="LabelText">Crop Year*</span>
            </td>
            <td>
                <asp:DropDownList ID="ddlCropYear" TabIndex="0" runat="server" 
                    CssClass="ctlWidth60" AutoPostBack="True" 
                    onselectedindexchanged="ddlCropYear_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                <span class="LabelText">Tax Year</span>
            </td>
            <td>
                <asp:DropDownList ID="ddlTaxYear" TabIndex="0" runat="server" 
                    CssClass="ctlWidth60">
                </asp:DropDownList>
            </td>
        </tr>
        <tr><td colspan="5">&nbsp;</td></tr>
        <tr style="line-height:30px;">
            <td><span class="LabelText">Rate Per Ton</span></td>
            <td><asp:TextBox ID="txtRatePerTon" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>
            <td><span class="LabelText">Percentage to Apply</span></td>
            <td><asp:TextBox ID="txtPercentageToApply" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />            
            </td>
        </tr>       
        <tr style="line-height:30px;">
            <td><span class="LabelText">Report Date</span></td>
            <td><asp:TextBox ID="txtReportDate" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>
            <td><span class="LabelText">Fiscal Year End Date</span></td>
            <td><asp:TextBox ID="txtFiscalYearEndDate" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <br />
    <p style="font-size:120%">
    * Changing the Crop Year resets the page with any data previously saved for the selected Crop Year.<br />
    Tax Year does not do this.  The Tax Year is just an attribute of the Crop Year.
    </p>
    <br />
    <br style="line-height:100px;" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
