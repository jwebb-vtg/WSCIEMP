<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="UserMaintenance.aspx.cs" Inherits="WSCIEMP.Admin.UserMaintenance" Title="Admin User Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin User Maintenance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript" language="javascript">
		
function SetAction(sAction) {
     $$('txtAction', $('#PostData')).val(sAction);
}		
	
function ShowFindUser() {

	// check for required selection
    var myWarn = $$('divWarning', $('#MainContentCenter'))[0];
    HideWarning(myWarn);	
			
	var url = "UserFinder.aspx";
	var popWidth = 865;	//screen.width-200;
	var popHeight = 565; //screen.height-200;
	
	var top = (screen.height-popHeight)/2;
	var left = (screen.width-popWidth)/2;
	
	if ( top <= 0 ) {
		top = 25;
	}
	if (left <= 0) {
		left = 25;
	}
	
	var win = window.open(url, "_blank", 
		"status=0,toolbar=0,top=" + top + ",left=" + left + ",location=0,menubar=0,directories=0,resizable=1,scrollbars=0,width=" + popWidth + ",height=" + popHeight);

}

function ShowUser(param) {
    $$('txtUserID', $('#PostData')).val(param);
	theForm.submit();
}

function CheckDelete(evt) {

	var e = GetEvent(evt);
	if (e != null) {	
		
		if (!confirm('Are you sure, absolutely certain, you want to delete this User?  This will Delete the User from the system and cannot be undone.')) {
			BubbleOff(e);	
			return false;			
		}
	
	} else {
		alert('You must use Microsoft Internet Explorer to use this function.');
	}				
}	
			
    </script>

    <table width="100%">
        <tr>
            <td>
                <div style="text-align:center;">
                    <p>For a new user, enter the Window's Logon Name, Full Name and Phone Number.</p>  
                    <p>For an existing user, USE THE FIND BUTTON (...) to identify the user record you want to update.</p>
                </div>
            </td>
        </tr>
    </table>
    <table class="layoutAvg" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
            <td style="width: 5%;">
                &nbsp;
            </td>
            <td style="width: 20%">
                <span class="LabelText">Logon Name:</span>
            </td>
            <td style="width: 48%">
                <asp:TextBox ID="txtUserName" CssClass="LgText" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                <input class="BtnSm" id="btnFindUser" style="width: 50px" onclick="SetAction('FindUser');ShowFindUser();" type="button" value="..." name="btnFindUser" />
            </td>
            <td style="width: 25%">
                <asp:Button ID="btnSave" runat="server" CssClass="BtnMed" Text="Save" OnClick="btnSave_Click">
                </asp:Button>                
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <span class="LabelText">Full Name:</span>
            </td>
            <td>
                <asp:TextBox ID="txtDisplayName" CssClass="LgText" runat="server"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <span class="LabelText">Phone Number:</span>
            </td>
            <td>
                <asp:TextBox ID="txtPhoneNumber" CssClass="MedText" runat="server"></asp:TextBox>
            </td>
            <td>
                &nbsp;
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
            <td colspan="3">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
            <td>
                &nbsp;
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
        <tr><td colspan="5">&nbsp;</td>
        </tr>
    </table>
    <div class="DisplayOff" id="PostData">
        <asp:textbox id="txtUserID" runat="server"></asp:textbox>
        <asp:textbox id="txtAction" runat="server"></asp:textbox>
    </div>
</asp:Content>
