<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="ManageLookups.aspx.cs" 
Inherits="WSCIEMP.Admin.ManageLookups" Title="Admin Manage Lookups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin Manage Lookups
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script language="javascript" defer="defer" type="text/javascript">
		
function SetupNew(evt) {

    var ctl = $$('txtDescription', $('#pageLayout'))[0];
	ctl.value = '';

	ctl = $$('ddlDescription', $('#pageLayout'))[0];
	ctl.selectedIndex = -1;
	ctl.disabled = true;

	ctl = $$('chkIsActive', $('#pageLayout'))[0];
	ctl.checked = false;	
	
	BubbleOff(evt);	
	return false;	
}		

function CheckChange(evt) {

	var e = GetEvent(evt);
	if (e != null) {

	    var ctl = $$('ddlDescription', $('#pageLayout'))[0];
		
		// ddlDescription is not disabled for an update.
		if (ctl.disabled == false) {		
			
			if (!confirm('Do you want to apply the new description to all existing records using the old description?')) {
				BubbleOff(e);	
				return false;			
			}

            var ctlAct = $$('txtAction', $('#PostData'))[0];
			ctlAct.value = "UPDATE";
			
		} else {

        var ctlNew = $$('txtIsNew', $('#PostData'))[0]; 
			ctlNew.value = "NEW";			
		}
	
	} else {
		alert('You must use Microsoft Internet Explorer to use this function.');
	}				
}	

function CheckDelete(evt) {

	var e = GetEvent(evt);
	if (e != null) {	
		
		if (!confirm('Are you sure you want to delete this Description?')) {
			BubbleOff(e);	
			return false;			
		}
	
	} else {
		alert('You must use Microsoft Internet Explorer to use this function.');
	}				
}	
			
    </script>

    <div>&nbsp;</div>
    <table cellspacing="0" cellpadding="0" border="0" class="layoutAvg" id="pageLayout">
        <tr>
            <td style="width: 2%">
                &nbsp;
            </td>
            <td style="width: 17%">
                <span class="LabelText">Type:</span>
            </td>
            <td style="width: 50%">
                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" CssClass="ButtonText" 
                    onselectedindexchanged="ddlType_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width: 10%; text-align: left">
                <asp:Button ID="btnNew" runat="server" CssClass="BtnMed" Text="New "></asp:Button>
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
                <span class="LabelText">Description:</span>
            </td>
            <td>
                <asp:DropDownList ID="ddlDescription" runat="server" CssClass="ButtonText" 
                    AutoPostBack="true" 
                    onselectedindexchanged="ddlDescription_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="text-align: left">
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
            <td>
                <span class="LabelText">Edit Description:</span>
            </td>
            <td>
                <asp:TextBox ID="txtDescription" CssClass="LgText" runat="server"></asp:TextBox>
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
            <td>
                &nbsp;
            </td>
            <td>
                <span class="LabelText">Is Active:</span>
            </td>
            <td>
                <asp:CheckBox ID="chkIsActive" runat="server"></asp:CheckBox>
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
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
    </table>
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="txtAction" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtIsNew" runat="server"></asp:TextBox>
    </div>
</asp:Content>
