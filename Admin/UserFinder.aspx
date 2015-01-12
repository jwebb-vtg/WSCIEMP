<%@Page Language="C#" AutoEventWireup="true" StylesheetTheme="ThemeA_1024" CodeBehind="UserFinder.aspx.cs" Inherits="WSCIEMP.Admin.UserFinder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Finder</title>
    
		<script type="text/javascript" language="javascript">
	
	var caller = null;
	function SelectUser(userID) {
		window.opener.ShowUser(userID);
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
    <form id="form1" runat="server">
    <div id="MainSectionNoVMenu">
        <table class="copyTableNoVMenu" style="border-left: white 0px solid; width: 815px"
            cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td>
                    <table width="815" border="0">
                        <tr>
                            <td class="pgTitle" style="text-align:center; width:95%;">
                                User Finder
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
                    <table cellspacing="0" cellpadding="0" style="width:95%; text-align:center" border="0">
                        <tr>
                            <td style="width: 15%">
                                <span class="LabelText">Logon Name</span>
                            </td>
                            <td style="width: 20%">
                                <asp:RadioButton ID="radLogonName" runat="server" GroupName="mode"></asp:RadioButton>
                            </td>
                            <td style="width: 50%">
                                &nbsp;
                            </td>
                            <td style="width: 15%">
                                <input class="BtnMed" id="btnCancel" onclick="DoCancel();" type="button" value="Cancel" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">Display Name</span>
                            </td>
                            <td>
                                <asp:RadioButton ID="radDisplayName" runat="server" GroupName="mode"></asp:RadioButton>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCriteria" runat="server" CssClass="LgText"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnFind" runat="server" CssClass="BtnMed" Text="Find" 
                                    onclick="btnFind_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="95%">
                    <div class="ButtonText" id="temp" style="overflow: auto; width: 720px; height: 420px; text-align:center" runat="server">
                        <asp:GridView ID="grdResults" runat="server" SkinID="grdColor" AutoGenerateColumns="False" CssClass="grdColor700">
                        <Columns>
                            <asp:BoundField DataField="usr_user_id">
                                <HeaderStyle Width="0%" CssClass="DisplayOff" />
                                <ItemStyle Width="0%" CssClass="DisplayOff" />
                            </asp:BoundField>
                            <asp:BoundField DataField="usr_login_name" HeaderText="Logon">
                                <HeaderStyle Width="30%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="usr_display_name" HeaderText="Display Name">
                                <HeaderStyle Width="40%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="usr_phone_number" HeaderText="Phone No">
                                <HeaderStyle Width="30%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>                                                           
                        </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="DisplayOff" id="PostData">
        <!-- hidden controls -->
    </div>
    </form>
</body>
</html>
