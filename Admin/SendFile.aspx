<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="SendFile.aspx.cs" Inherits="WSCIEMP.Admin.SendFile" Title="Admin Send File" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin Send File
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

<script language="javascript" type="text/javascript">
//<![CDATA[
function DoOnLoad() {

    // check the pending email control for any entries.  If not empty,
    // immediately press the send email button to post the page back and continue
    // the email batch process.
    var btn = GetObjById('ctl00_ContentCenter_btnSendEmail');
    var lst = GetObjById('ctl00_ContentCenter_lstPendingEmail');
    var isProcessing = GetObjById('ctl00_ContentCenter_txtContinueBatch');

    if (isProcessing.value != '') {
        if (lst.options.length > 0) {   // Redundant check after isProcessing flag.
            btn.click();
        }
    }
}
//]]>
</script>

    <table cellspacing="0" cellpadding="0" border="0" class="layoutAvg" style="width: 99%;">
        <tr>
            <td width="1%">
                &nbsp;
            </td>
            <td width="17%">
                &nbsp;
            </td>
            <td width="32%">
                &nbsp;
            </td>
            <td width="13%">
                &nbsp;
            </td>
           <td width="*">
                &nbsp;
            </td>                        
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <span class="LabelText">From Email Address:</span>
            </td>
            <td>
                <asp:TextBox ID="txtFromEmail" runat="server" CssClass="LgText"></asp:TextBox>
            </td>
            <td>
                <span class="LabelText">Crop Year:</span></td>
            <td>
                <asp:DropDownList ID="ddlCropYear" runat="server" CssClass="ctlWidth60">
                </asp:DropDownList>
            </td>                
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <span class="LabelText">Subject:</span>
            </td>
            <td>
                <asp:TextBox ID="txtSubject" runat="server" CssClass="LgText"></asp:TextBox>
            </td>
            <td><span  class="LabelText">SHID: *</span></td>
            <td>
                <asp:TextBox ID="txtSHID" runat="server" EnableViewState="False" CssClass="LgText"></asp:TextBox>
            </td>            
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="LabelText">
                Message:
            </td>
            <td>
                <asp:TextBox ID="txtMessage" runat="server" CssClass="LgText" TextMode="MultiLine" style="height: 150px;"></asp:TextBox>
            </td>
            <td style="vertical-align: top;">
                <span class="LabelText">Attachment:</span>
            </td>
            <td style="vertical-align:top;">
                <asp:ListBox ID="lstAttach" runat="server" CssClass="LgText" Rows="8" SelectionMode="Multiple">
                </asp:ListBox>
            </td>                
        </tr>
        <tr><td colspan="5">&nbsp;</td></tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2" style="text-align: center">
                <span class="LabelText">Select one or more of the following (required)</span>
            </td>
            <td colspan="2" rowspan="2">
                <asp:Button ID="btnSendEmail" CssClass="BtnMed" runat="server" Text="Send Email"
                        OnClick="btnSendEmail_Click"></asp:Button><br /><br style="line-height:5px;"/><br style="line-height:5px;"/>
            </td>           
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkSubYesActYes" runat="server" EnableViewState="True"></asp:CheckBox><span class="LabelText">&nbsp;&nbsp;Is Shareholder and Is Active</span>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkSubYesActNo" runat="server" EnableViewState="True"></asp:CheckBox><span class="LabelText">&nbsp;&nbsp;Is Shareholder, but Not Active</span>
            </td>
            <td style="vertical-align:top; border-top: dashed 1px #000000; border-left: dashed 1px #000000; padding-top: 2px; padding-left:2px;">                
                <asp:Button ID="btnTestFromEmail" CssClass="BtnMed" runat="server" Text="Test It" OnClick="btnTestFromEmail_Click"></asp:Button>
            </td>
            <td style="vertical-align:top; border-top: dashed 1px #000000; border-right: dashed 1px #000000; padding-top: 2px;">
                <span class="ButtonText">To: </span><asp:TextBox ID="txtToTest" runat="server" CssClass="LgText"></asp:TextBox><br />
            </td>             
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkSubNo" runat="server" EnableViewState="True"></asp:CheckBox><span class="LabelText">&nbsp;&nbsp;Not a Shareholder</span>
            </td>
            <td colspan="2" rowspan="3" style="border-left: dashed 1px #000000; border-right: dashed 1px #000000; border-bottom: dashed 1px #000000; padding-left:2px;">
                <span class="ButtonText">Use the </span><span class="LabelText">Test It</span><span
                    class="ButtonText"> button to send an email to the above </span><span class="LabelText">To:</span>
                    <span class="ButtonText">address, using the </span><span class="LabelText">From
                        Email Address</span><span class="ButtonText">, </span><span class="LabelText">
                            Subject</span><span class="ButtonText">, and </span><span class="LabelText">Message</span>
                <span class="ButtonText">you enter. This includes the attachment.</span>            
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkInternal" runat="server" EnableViewState="True"></asp:CheckBox><span class="LabelText">&nbsp;&nbsp;Only Internal Email Addresses</span>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkExternal" runat="server" EnableViewState="True"></asp:CheckBox><span class="LabelText">&nbsp;&nbsp;Only External Email Addresses</span>
            </td>
        </tr>                        
        <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <span class="ButtonText">
                * SHID can be left blank to email all shareholders having an email address on record.
                Enter one SHID to report on that shareholder, privided an email address exists.
                Enter a comma separated list of SHIDs such as 20003,20007,20008 or use a dash to
                enter a single range of SHIDs such as 20003-20200.</span>
            </td>
        </tr>
    </table>
    <div class="DisplayOff">
        <asp:ListBox ID="lstPendingEmail" runat="server"></asp:ListBox>
        <asp:HiddenField ID="hideGoodEmail" runat="server" />
        <asp:HiddenField ID="hideBadEmail" runat="server" />
        <asp:HiddenField ID="txtContinueBatch" runat="server" />
    </div>
</asp:Content>
