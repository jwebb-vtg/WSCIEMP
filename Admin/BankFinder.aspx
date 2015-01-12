<%@Page Language="C#" AutoEventWireup="true" StylesheetTheme="ThemeA_1024" CodeBehind="BankFinder.aspx.cs" Inherits="WSCIEMP.Admin.BankFinder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Bank Finder</title>

    <script type="text/javascript" language="javascript">
	
var caller = null;
function SelectBank(bankID) {
	window.opener.ShowBank(bankID);
	window.close();
}

function DoCancel() {
	window.close();
}

function DoOnload() {
	GetObjById("txtCriteria").focus();
}	

    </script>

</head>
<body onload="DoOnload();">
    <form id="Form1" method="post" runat="server">
    <div id="MainSectionNoVMenu">
        <table class="copyTableNoVMenu" style="border-left: white 0px solid; width: 815px"
            cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td>
                    <table width="815" border="0">
                        <tr>
                            <td class="pgTitle" style="text-align:center; width:95%">
                                Bank Finder
                            </td>
                            <td style="text-align:center; width:15%;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="margin: 0px; text-align: center">
                    <div class="WarningOff" id="divWarning" runat="server">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="95%" style="text-align: left;" border="0">
                        <tr>
                            <td style="width: 15%">
                                <span class="LabelText">Bank Name</span>
                            </td>
                            <td style="width: 20%">
                                <asp:RadioButton ID="radBankName" runat="server" GroupName="mode"></asp:RadioButton>
                            </td>
                            <td style="width: 50%">
                                &nbsp;<asp:CheckBox ID="chkIsActive" CssClass="LabelText" runat="server" Text="Active">
                                </asp:CheckBox>
                            </td>
                            <td style="width: 15%">
                                <input class="BtnMed" id="btnCancel" onclick="DoCancel();" type="button" value="Cancel" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">Bank Number</span>
                            </td>
                            <td>
                                <asp:RadioButton ID="radBankNumber" runat="server" GroupName="mode"></asp:RadioButton>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCriteria" runat="server" CssClass="LgText"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnFind" runat="server" CssClass="BtnMed" Text="  Find  " onclick="btnFind_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width:95%;">
                    <div class="ButtonText" id="temp" style="overflow: auto; width: 720px; height: 420px; text-align:center;" runat="server">
                        <asp:GridView ID="grdResults" runat="server" SkinID="grdColor" AutoGenerateColumns="False" CssClass="grdColor700">
                            <Columns>
                                <asp:BoundField DataField="bnk_bank_id">
                                    <HeaderStyle Width="0%" CssClass="DisplayOff" />
                                    <ItemStyle Width="0%" CssClass="DisplayOff" />
                                </asp:BoundField>
                                <asp:BoundField DataField="bnk_number" HeaderText="Bank Number">
                                    <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="bnk_name" HeaderText="Bank Name">
                                    <HeaderStyle Width="50%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField> 
                                <asp:BoundField DataField="bnk_contact_name" HeaderText="Contact Name">
                                    <HeaderStyle Width="30%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>                                                           
                            </Columns>
                        </asp:GridView>
                        </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
