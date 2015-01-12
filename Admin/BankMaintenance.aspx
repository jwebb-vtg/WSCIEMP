<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="BankMaintenance.aspx.cs" Inherits="WSCIEMP.Admin.BankMaintenance" Title="Admin Bank Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin Bank Maintenance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript" language="javascript" defer="defer">			

function SetAction(sAction) {
    var ctlAct = $$('txtAction', $('#PostData'))[0];
	ctlAct.value = sAction;
}		
	
function ShowFindBank() {

	// check for required selection
    var myWarn = $$('divWarning', $('#MainContentCenter'))[0];
	HideWarning(myWarn);

	var bankName = $$('txtBankName', $('#pageLayout'))[0].value;
	var bankNumber = $$('txtBankNumber', $('#pageLayout'))[0].value;
	
	// Package parameters
	var params = 'bankName=' + bankName + '&' +
		'bankNumber=' + bankNumber;

	var url = "BankFinder.aspx?" + params;
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

function ShowBank(param) {

    var ctlBankID = $$('txtBankID', $('#PostData'))[0]
	ctlBankID.value = param;
	//ASP.NET: MS creates JScript 'theForm' variable.
	theForm.submit();
}

function CheckDelete(evt) {

	var e = GetEvent(evt);
	if (e != null) {	
		
		if (!confirm('Are you sure, absolutely certain, you want to delete this Bank?  This will Delete the Bank from the system and cannot be undone.')) {
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
                    <asp:Label ID="lblNewBank" runat="server" CssClass="WarningOff"></asp:Label></div>
            </td>
        </tr>
    </table>
    <table class="layoutAvg" cellspacing="0" cellpadding="0" border="0" width="100%" id="pageLayout">
        <tr>
            <td style="width: 5%;">
                &nbsp;
            </td>
            <td style="width: 20%;">
                <span class="LabelText">Bank Name:</span>
            </td>
            <td style="width: 48%;">
                <asp:TextBox ID="txtBankName" CssClass="LgText" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                <input class="BtnSm" id="btnFindBank" style="width: 50px;" onclick="SetAction('FindBank');ShowFindBank();"
                    type="button" value="..." name="btnFindBank" />
            </td>
            <td style="width: 25%;">
                <asp:Button ID="btnNew" runat="server" CssClass="BtnMed" Text="New" OnClick="btnNew_Click">
                </asp:Button>
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
                <span class="LabelText">Bank Short Name:</span>
            </td>
            <td>
                <asp:TextBox ID="txtShortName" CssClass="LgText" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnDelete" runat="server" CssClass="BtnMed" Text="Delete" OnClick="btnDelete_Click">
                </asp:Button>
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
                <span class="LabelText">WSC Bank Number:</span>
            </td>
            <td>
                <asp:TextBox ID="txtBankNumber" CssClass="MedText" runat="server" BackColor="#FFFFC0"
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" CssClass="BtnMed" Text="Save" OnClick="btnSave_Click">
                </asp:Button>
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
            <td>
                &nbsp;
            </td>
            <td>
                <span class="LabelText">Addr Line 1:</span>
            </td>
            <td>
                <asp:TextBox ID="txtAddrLine1" CssClass="LgText" runat="server"></asp:TextBox>
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
                <span class="LabelText">Addr Line 2:</span>
            </td>
            <td>
                <asp:TextBox ID="txtAddrLine2" CssClass="LgText" runat="server"></asp:TextBox>
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
                <span class="LabelText">City:</span>
            </td>
            <td>
                <asp:TextBox ID="txtCity" CssClass="LgText" runat="server"></asp:TextBox>
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
                <span class="LabelText">State:</span>
            </td>
            <td>
                <asp:DropDownList ID="ddlState" runat="server" CssClass="SmList">
                </asp:DropDownList>
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
                <span class="LabelText">Zip:</span>
            </td>
            <td>
                <asp:TextBox ID="txtZip" CssClass="MedText" runat="server"></asp:TextBox>
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
            <td style="height: 23px">
                &nbsp;
            </td>
            <td style="height: 23px">
                <span class="LabelText">Contact Name:</span>
            </td>
            <td style="height: 23px">
                <asp:TextBox ID="txtContactName" CssClass="LgText" runat="server"></asp:TextBox>
            </td>
            <td style="height: 23px">
                &nbsp;
            </td>
            <td style="height: 23px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <span class="LabelText">Phone:</span>
            </td>
            <td>
                <asp:TextBox ID="txtPhone" CssClass="MedText" runat="server"></asp:TextBox>
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
                <span class="LabelText">Fax:</span>
            </td>
            <td>
                <asp:TextBox ID="txtFax" CssClass="MedText" runat="server"></asp:TextBox>
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
                <span class="LabelText">Email:</span>
            </td>
            <td>
                <asp:TextBox ID="txtEmail" CssClass="LgText" runat="server"></asp:TextBox>
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
                <span class="LabelText">Other:</span>
            </td>
            <td>
                <asp:TextBox ID="txtOther" CssClass="LgText" runat="server"></asp:TextBox>
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
            <td colspan="5" style="height: 150px">&nbsp;</td>
        </tr>
    </table>
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="txtBankID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtAction" runat="server"></asp:TextBox>
    </div>
</asp:Content>
