<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
AutoEventWireup="true" CodeBehind="BankPayeeMaintenance.aspx.cs" Inherits="WSCIEMP.Admin.BankPayeeMaintenance" Title="Admin Bank Payee Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Admin Bank Payee Maintenance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript">

    $(document).ready(function() {

        //-----------------------------------------
        // Setup calendar
        //-----------------------------------------
        $$('txtEquityDate', $('#eqDate')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+1',
            changeMonth: true, changeYear: true, duration: 'fast'
        });

        //-----------------------------------------
        // Setup Find Address dialog
        //-----------------------------------------
        $('#AddressFinder').dialog({
            title: "Address Finder", modal: true, autoOpen: false, height: 450, width: 650, draggable: true, resizable: true,
            open: function(type, data) {
                $(this).parent().appendTo("form");
            }
        });
    });

    function showDialog(id) {
        if (id == 'AddressFinder') {
            PrepAddressFinder();
        }
        $('#' + id).dialog("open");
    }

    function closeDialog(id) {
        $('#' + id).dialog("close");
    }

    function closeAndResolve(id) {
        $('#' + id).dialog("close");
        $$('btnResolveShid', $("#actionBlock")).click();
    }

    function PrepAddressFinder() {

        // clear warning
        $$('addrWarning', $('#AddressFinder')).text('');
        // select business name
        $$('radTypeBusname', $('#AddressFinder')).click();
        // clear search string
        $$('txtSearchString', $('#AddressFinder')).val('');
        // clear results list
        var lst = $$('lstAddressName', $('#AddressFinder'));        
        $('#' + lst.attr('id') + ' option').remove();

        // clear all member specific details
        $$('txtAddrSHID', $('#AddressFinder')).val('');
        $$('chkAddrSubscriber', $('#AddressFinder')).click();
        $$('txtAddrFName', $('#AddressFinder')).val('');
        $$('txtAddrLName', $('#AddressFinder')).val('');
        $$('txtAddrBName', $('#AddressFinder')).val('');
        $$('txtAddrAddress', $('#AddressFinder')).val('');
        $$('txtAddrAddressLine2', $('#AddressFinder')).val('');
        $$('txtAddrCity', $('#AddressFinder')).val('');
        $$('txtAddrState', $('#AddressFinder')).val('');
        $$('txtAddrZip', $('#AddressFinder')).val('');
        $$('txtAddrPhoneNo', $('#AddressFinder')).val('');        
    }
        
function SetAction(sAction) {
    var ctlAct = $$('txtAction', $('#PostData'));
	ctlAct.val(sAction);
}		

// Fit to the add and delete buttons.
function CheckPayeeAction(allowAction, btnCtl, e) {

	// Validate whether the action is allowed.
    var ctl = $$('txtAction', $('#PostData'));
	if (ctl.val() != allowAction) {
		BubbleOff(e);
		return false;
	}
	
	// Do we have a bank to act on?
    ctl = $$('txtBankID', $('#PostData'));
	if (ctl.val() == '') {
		alert('Please select a bank.');
		BubbleOff(e)
		return false;
	}

	// Do we have a member to act on?
    ctl = $$('txtAddressID', $('#PostData'))[0];

	if (ctl.value == '') {
		alert('Please select a member.');
		BubbleOff(e)
		return false;
	}
		
	if (btnCtl.value.indexOf('Del') != -1) {
		if (!confirm('Are you sure, absolutely certain, you want to delete this bank-payee relation?  This will Delete the bank-payee relation from the system and cannot be undone.')) {
			BubbleOff(e);	
			return false;			
		}		
	}
	
	// Determine whether this applies to other years.
	var currentYear = (new Date).getYear();
	var userYear = $$('ddlCropYear', $('#bodyTable')).val();
	var msg = "";
	var otherYear = 0;

	if (userYear == (currentYear - 1)) {
		msg = "If appropriate, do you want to also apply this change to the current crop year?";
		otherYear = currentYear;
	} else {
		if ( userYear == currentYear) {
			msg = "If appropriate, do you want to also apply this change to the prior crop year?";
			otherYear = currentYear-1;
		}
	}

    var ctlOtherYr = $$('txtOtherYears', $('#PostData'))[0];

	if (msg != "" && confirm(msg)) {
		ctlOtherYr.value = otherYear;				
	} else {
		ctlOtherYr.value = "0";
	}
}

// Fit to the add and delete buttons.
function CheckEquityAction(allowAction, btnCtl, e) {

	// Validate whether the action is allowed.
    var ctlAct = $$('txtAction', $('#PostData'))[0]; 
	if (ctlAct.value != allowAction) {
		BubbleOff(e);
		return false;
	}
	
	// Do we have a bank to act on?
    var ctl = $$('txtEquityBankID', $('#PostData'))[0]; 
	if (ctl.value == '') {
		alert('Please select an equity bank.');
		BubbleOff(e)
		return false;
	}

	// Do we have a member to act on?
    ctl = $$('txtMemberID', $('#PostData'))[0]; 
	if (ctl.value == '') {
		alert('Please select a member.');
		BubbleOff(e)
		return false;
	}
		
	if (btnCtl.value.indexOf('Del') != -1) {
		if (!confirm('Are you sure, absolutely certain, you want to delete this bank-equity relation?  This will Delete the bank-equity relation from the system and cannot be undone.')) {
			BubbleOff(e);	
			return false;			
		}		
	}
	
	// Determine whether this applies to other years.
	var currentYear = (new Date).getYear();
	var userYear = $$('ddlCropYear', $('#bodyTable')).val(); 
	var msg = "";
	var otherYear = 0;

	if (userYear == (currentYear - 1)) {
		msg = "If appropriate, do you want to also apply this change to the current crop year?";
		otherYear = currentYear;
	} else {
		if ( userYear == currentYear) {
			msg = "If appropriate, do you want to also apply this change to the prior crop year?";
			otherYear = currentYear-1;
		}
	}

    var ctlOtherYr = $$('txtOtherYears', $('#PostData'))[0]; 
	if (msg != "" && confirm(msg)) {
		ctlOtherYr.value = otherYear;				
	} else {
		ctlOtherYr.value = "0";
	}
}

function ChangeCropYear() {

	SetAction('ChangeCropYear');
	theForm.submit();
}
	
function CheckEnterKey(e) {

	var evt = GetEvent(e);
	if ((evt && evt.which && evt.which == 13) || (evt && evt.keyCode && evt.keyCode == 13)) {
	
		SetAction('FindSHID');
		theForm.submit();
	}	
}

function AbortEnterKey(e) {

	var evt = GetEvent(e);
	if ((evt && evt.which && evt.which == 13) || (evt && evt.keyCode && evt.keyCode == 13)) {	
		BubbleOff(e);
		return false;
	}	
}

function ShowFindBank() {

    activeFrame = $$('txtActiveFrame', $('#PostData')).val();
	var targetCtl = null;	
	
	if (activeFrame == 'Payee') {
	    targetCtl = $$('txtBankName', $('#bodyTable'))[0];
	} else {
	    targetCtl = $$('txtEquityBankName', $('#bodyTable'))[0];
	}

	// check for required selection
	var myWarn = $$('divWarning', $('#MainContentCenter'))[0];
	HideWarning(myWarn);	

	var bankName = targetCtl.value;
	
	// Package parameters
	var params = 'bankName=' + bankName + '&' + 'bankNumber=';
			
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
	
	if (activeFrame == 'Payee') {
		SetAction('FindBank');
		$$('txtBankID', $('#PostData')).val(param);
	} else {
		SetAction('FindEquityBank');
		$$('txtEquityBankID', $('#PostData')).val(param);
	}
	
	theForm.submit();
}

function SelectItemRow(ctl, bankPayeeID, bankName, bankID, subRecd) {

    var btn = $$('btnAdd', $('#bodyTable'))[0];
	btn.value = "Update";
	
	// Note, colors RowSelectColor and RowUnSelectColor are defined globally.
	var grid = null;
	if (ctl.parentElement) {
		grid = ctl.parentElement.parentElement;	
	} else {
		grid = ctl.parentNode.parentNode;
	}

	// Start at row 1 in order to skip header in row zero.
	var i=0;
	for (i=1; i<grid.rows.length; i++) {
		if (i == ctl.rowIndex) {
			grid.rows[i].style.backgroundColor = RowSelectColor;
		} else {
			grid.rows[i].style.backgroundColor = RowUnSelectColor;
		}
	}

    $$('txtBankPayeeID', $('#PostData'))[0].value = bankPayeeID;
	$$('txtBankName', $('#bodyTable'))[0].value = bankName;
	$$('txtBankID', $('#PostData'))[0].value = bankID;
	$$('chkSubordination', $('#bodyTable'))[0].checked = (subRecd == 'Y');
	
	return true;
}	

function SelectEqRow(ctl, bankEquityLienID, bankName, equityBankID, 
	lienShares, lienPatronage, lienRetains, releaseShares, releasePatronage, releaseRetains,
	equityDate) {

	var btn = $$('btnAddEquityBank', $('#bodyTable'))[0];
	btn.value = "Update";
	
	// Note, colors RowSelectColor and RowUnSelectColor are defined globally.
	var grid = null;
	if (ctl.parentElement) {
		grid = ctl.parentElement.parentElement;	
	} else {
		grid = ctl.parentNode.parentNode;
	}

	// Start at row 1 in order to skip header in row zero.
	var i=0;
	for (i=1; i<grid.rows.length; i++) {
		if (i == ctl.rowIndex) {
			grid.rows[i].style.backgroundColor = RowSelectColor;
		} else {
			grid.rows[i].style.backgroundColor = RowUnSelectColor;
		}
	}

$$('txtBankEquityLienID', $('#PostData'))[0].value = bankEquityLienID;
	$$('txtEquityBankName', $('#bodyTable'))[0].value = bankName;
	$$('txtEquityBankID', $('#PostData'))[0].value = equityBankID;
	$$('chkEqPatronStock', $('#bodyTable'))[0].checked = (lienShares == 'Y');
	$$('chkEqPatronage', $('#bodyTable'))[0].checked = (lienPatronage == 'Y');
	$$('chkEqRetains', $('#bodyTable'))[0].checked = (lienRetains == 'Y');
	$$('chkReleasePatronStock', $('#bodyTable'))[0].checked = (releaseShares == 'Y');
	$$('chkReleasePatronage', $('#bodyTable'))[0].checked = (releasePatronage == 'Y');
	$$('chkReleaseRetains', $('#bodyTable'))[0].checked = (releaseRetains == 'Y');
	$$('txtEquityDate', $('#bodyTable'))[0].value = equityDate;
	
	return true;
}

var _focusCtl = null;

function DoGotFocus(ctl) {
	_focusCtl = ctl;
}
function DoKeyPress(ctl, e) {

	if (ctl == _focusCtl) {
		return true;
	} else {
		BubbleOff(e);
		return false;
	}
}

function ShowHide(frameName) {

    var anchEqBank = $$('switchEqBank', $('#bodyTable'))[0];
    var anchPayBank = $$('switchCropBank', $('#bodyTable'))[0];
	var onFrameName = "";
	var onAnchor = null;
	var onDiv = null;
	var offFrameName = "";
	var offAnchor = null;
	var offDiv = null;
	
	if ( (frameName == 'Payee' && anchPayBank.innerHTML == 'Show') ||
		(frameName == 'Equity' && anchEqBank.innerHTML == 'Hide') ) {
		
		// Payee frame wants to become the active frame					
		onFrameName = 'Payee';
		onAnchor = $$('switchCropBank', $('#bodyTable'))[0];
		onDiv = $$('CropBanks', $('#bodyTable'))[0]; 
				
		offFrameName = 'Equity';
		offAnchor = $$('switchEqBank', $('#bodyTable'))[0];
		offDiv = $$('EquityBanks', $('#bodyTable'))[0];

	} else {
		// Equity frame wants to become the active frame
		onFrameName = 'Equity';
		onAnchor = $$('switchEqBank', $('#bodyTable'))[0];
		onDiv = $$('EquityBanks', $('#bodyTable'))[0];
		
		offFrameName = 'Payee';
		offAnchor = $$('switchCropBank', $('#bodyTable'))[0];
		offDiv = $$('CropBanks', $('#bodyTable'))[0]; 			
	}

    $$('txtActiveFrame', $('#PostData')).val(onFrameName);
	onAnchor.innerHTML = "Hide";
	onDiv.className = "DisplayOn";
	offAnchor.innerHTML = "Show";
	offDiv.className = "DisplayOff";
}
    </script>
    <asp:ScriptManager ID="mainScriptManager" runat="server">
    </asp:ScriptManager>
    <div class="DisplayOff" id="actionBlock">
        <asp:Button ID="btnResolveShid" runat="server" Text="..." OnClick="btnResolveShid_Click" />
    </div>    
    <table cellspacing="0" cellpadding="0" border="0" style="width: 98%;" id="bodyTable">
        <tr>
            <td style="width: 38%">
                <br />
                <span class="LabelText">Crop Year: </span>
                <asp:DropDownList ID="ddlCropYear" TabIndex="0" runat="server" CssClass="ctlWidth60">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align:center">
                <div style="width: 90%; border-bottom: black 1px solid; text-align:center">
                    <span class="LabelText">Payee</span></div>
            </td>
            <td style="text-align:center">
                <div style="width: 90%; border-bottom: black 1px solid; text-align:center;">
                    <span class="LabelText">Bank Associations (on Crop)</span>&nbsp;&nbsp; (<a class="permLink"
                        id="switchCropBank" onclick="ShowHide('Payee');" href="#" runat="server">Hide</a>)
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0" class="layoutAvg">
                    <tr>
                        <td style="height: 47px; width: 30%">
                            <br />
                            <span class="LabelText">SHID:</span>
                        </td>
                        <td style="height: 47px">
                            <asp:UpdatePanel ID="uplShid" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                runat="server">
                                <ContentTemplate>
                                    <br />
                                    <asp:TextBox ID="txtSHID" TabIndex="0" runat="server" CssClass="ctlWidth60"></asp:TextBox>&nbsp;&nbsp;
                                    <input class="BtnSm" id="btnFindAddress" style="width: 30px"
                                        onfocus="DoGotFocus(this);" onclick="DoKeyPress(this, event);showDialog('AddressFinder');"
                                        tabindex="0" type="button" value="..." />
                                </ContentTemplate>
                            </asp:UpdatePanel>                                
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px">
                            <span class="LabelText">First Name:</span>
                        </td>
                        <td style="height: 25px">
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px">
                            <span class="LabelText">Last Name:</span>
                        </td>
                        <td style="height: 21px">
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">
                            <span class="LabelText">Bus Name:</span>
                        </td>
                        <td style="height: 22px">
                            <asp:TextBox ID="txtBusName" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="LabelText">Addr Line 1:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddrLine1" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="LabelText">Addr Line 2:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddrLine2" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="LabelText">City:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="LabelText">State:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtState" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">
                            <span class="LabelText">Zip:</span>
                        </td>
                        <td style="height: 22px">
                            <asp:TextBox ID="txtZip" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">
                            <span class="LabelText">Tax ID:</span>
                        </td>
                        <td style="height: 22px">
                            <asp:TextBox ID="txtTaxID" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="LabelText">Phone:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="LabelText">Email:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="LgText" ReadOnly="True" BackColor="#FFFFC0"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="text-align:center">
                <div id="CropBanks" runat="server">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="width: 37%">
                                <span class="LabelText">Bank Name:</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtBankName" TabIndex="0" runat="server" CssClass="LgList"></asp:TextBox>&nbsp;&nbsp;<input
                                    class="BtnSm" id="btnFindBank" style="width: 30px" 
                                    onfocus="DoGotFocus(this);" onclick="DoKeyPress(this, event);ShowFindBank();"
                                    tabindex="0" type="button" value="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:CheckBox ID="chkSubordination" TabIndex="0" runat="server" CssClass="LabelText"
                                    TextAlign="Left" Text="Subordination Received?"></asp:CheckBox>
                            </td>
                            <td style="text-align: right;">
                                <br />
                                &nbsp;&nbsp;<asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="BtnMedSm"
                                    Text="Add" OnClick="btnAdd_Click"></asp:Button>&nbsp;<asp:Button ID="btnDelete" TabIndex="0"
                                        runat="server" CssClass="BtnMedSm" Text="Delete" OnClick="btnDelete_Click">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="LabelText" style="border: solid 1px #000000; height: 170px; text-align:center">
                                    <asp:GridView ID="grdResults" runat="server" SkinID="grdColor" AutoGenerateColumns="False"
                                        CssClass="grdColor600">
                                        <Columns>
                                            <asp:BoundField DataField="bnkpay_bank_payee_id" HeaderText="">
                                                <HeaderStyle CssClass="DisplayOff" />
                                                <ItemStyle CssClass="DisplayOff" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkpay_bank_id" HeaderText="">
                                                <HeaderStyle CssClass="DisplayOff" />
                                                <ItemStyle CssClass="DisplayOff" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkpay_sequence_number" HeaderText="">
                                                <HeaderStyle CssClass="DisplayOff" />
                                                <ItemStyle CssClass="DisplayOff" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkpay_bank_number" HeaderText="Bank No">
                                                <HeaderStyle Width="20%"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkpay_bank_name" HeaderText="Bank Name">
                                                <HeaderStyle Width="60%"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkpay_sub_recd" HeaderText="Sub Rec'd">
                                                <HeaderStyle Width="20%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div style="width: 90%; border-bottom: black 1px solid; text-align:center">
                    <span class="LabelText">Bank Associations (on Equity)</span>&nbsp;&nbsp; (<a class="permLink"
                        id="switchEqBank" onclick="ShowHide('Equity');" href="#" runat="server">Show</a>)
                </div>
                <div id="EquityBanks" runat="server">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <th style="width: 35%">
                            </th>
                            <th>
                            </th>
                        </tr>
                        <tr>
                            <td style="text-align: left; vertical-align: bottom;">
                                <span class="LabelText">Bank Name:</span>
                            </td>
                            <td style="text-align:left;">
                                <asp:TextBox ID="txtEquityBankName" TabIndex="0" runat="server" CssClass="LgList"></asp:TextBox>&nbsp;&nbsp;<input
                                    class="BtnSm" id="btnFindEqBank" style="width: 30px" 
                                    onfocus="DoGotFocus(this);" onclick="DoKeyPress(this, event);ShowFindBank();"
                                    tabindex="0" type="button" value="..." />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; vertical-align: bottom;">
                                <span class="LabelText">Date: </span>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">                                
                                <div id="eqDate"><asp:TextBox ID="txtEquityDate" TabIndex="0" runat="server" CssClass="ctlWidth75"></asp:TextBox></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="chkEqPatronStock" TabIndex="0" runat="server" CssClass="LabelText"
                                    TextAlign="Right" Text="Lien Patron Stock?"></asp:CheckBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="chkReleasePatronStock" TabIndex="0" runat="server" CssClass="LabelText"
                                    TextAlign="Right" Text="Release Patron Stock?"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="chkEqPatronage" TabIndex="0" runat="server" CssClass="LabelText"
                                    TextAlign="Right" Text="Lien Patronage?"></asp:CheckBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="chkReleasePatronage" TabIndex="0" runat="server" CssClass="LabelText"
                                    TextAlign="Right" Text="Release Patronage?"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="chkEqRetains" TabIndex="0" runat="server" CssClass="LabelText"
                                    TextAlign="Right" Text="Lien Retains?"></asp:CheckBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="chkReleaseRetains" TabIndex="0" runat="server" CssClass="LabelText"
                                    TextAlign="Right" Text="Release Retains?"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: right" colspan="2">
                                            <br />
                                            <asp:Button ID="btnAddEquityBank" TabIndex="0" runat="server" CssClass="BtnMedSm"
                                                Text="Add" OnClick="btnAddEquityBank_Click"></asp:Button>&nbsp;<asp:Button ID="btnDeleteEquityBank"
                                                    TabIndex="0" runat="server" CssClass="BtnMedSm" Text="Delete" OnClick="btnDeleteEquityBank_Click">
                                                </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="LabelText" style="border: solid 1px #000000; overflow: auto; height: 170px; text-align:center;">
                                    <asp:GridView ID="grdEquityResults" runat="server" SkinID="grdColor" AutoGenerateColumns="False"
                                        CssClass="grdColor600">
                                        <Columns>
                                            <asp:BoundField DataField="bnkeql_bank_equity_lien_id" HeaderText="">
                                                <HeaderStyle CssClass="DisplayOff" />
                                                <ItemStyle CssClass="DisplayOff" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_bank_id" HeaderText="">
                                                <HeaderStyle CssClass="DisplayOff" />
                                                <ItemStyle CssClass="DisplayOff" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_sequence_number" HeaderText="">
                                                <HeaderStyle CssClass="DisplayOff" />
                                                <ItemStyle CssClass="DisplayOff" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_bank_number" HeaderText="Bank No">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_bank_name" HeaderText="Bank Name">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_lien_patron_stock" HeaderText="Lien Shares">
                                                <HeaderStyle></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_lien_patronage" HeaderText="Lien Patronage">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_lien_retains" HeaderText="Lien Retains">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_release_recd_patron_stock" HeaderText="Release Shares">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_release_recd_patronage" HeaderText="Release Patronage">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_release_recd_retains" HeaderText="Release Retains">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="bnkeql_date" HeaderText="Date">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    <div id="hideModal" class="DisplayOff">
        <!--  ***********************************************************
                Address Finder Dialog: Customized from regular.  This
                version uses a hidden txtAFAddressID field.
              ***********************************************************  -->
        <div id='AddressFinder'>
            <asp:UpdatePanel ID="uplAddressFinder" UpdateMode="Conditional" ChildrenAsTriggers="true"
                runat="server">
                <ContentTemplate>
                    <br />
                    <div id="popWarning" class="WarningOff" runat="server">
                    </div>
                    <table style="width: 600px; border: solid 1px #000000;" id="addrCriteria">
                        <tr>
                            <td>
                                <span class="addrLabel">Type</span>
                                <div title="Type" style="text-align: left; border: solid 1px #000000;">
                                    <asp:RadioButton ID="radTypeBusname" Checked="false" GroupName="AdrType" runat="server"
                                        Text="Business Name" CssClass="radio" /><br />
                                    <asp:RadioButton ID="radTypeLastName" Checked="true" GroupName="AdrType" runat="server"
                                        Text="Last Name" CssClass="radio" /><br />
                                    <asp:RadioButton ID="radTypeSHID" Checked="false" GroupName="AdrType" runat="server"
                                        Text="SHID" CssClass="radio" />
                                </div>
                                <br />
                            </td>
                            <td style="width: 5px;">
                                &nbsp;
                            </td>
                            <td>
                                <div style="text-align: left;">
                                    <span class="addrLabel">Search String</span><br />
                                    <asp:TextBox CssClass="textEntry" ID="txtSearchString" Width="150" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:Button ID="btnAddrFind" runat="server" Text="Find" CssClass="btnLabel" OnClick="btnAddrFind_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 600px; border: solid 1px #000000" id="addrResult">
                        <tr>
                            <td rowspan="9" style="width: 45%; text-align: center; vertical-align: top;">
                                <span class="addrLabel">Results</span><br />
                                <asp:ListBox ID="lstAddressName" CssClass="textEntry" Width="250" Height="200" DataTextField="BusName"
                                    DataValueField="SHID" runat="server" OnSelectedIndexChanged="lstAddressName_SelectedIndexChanged"
                                    AutoPostBack="True"></asp:ListBox>
                                <br />
                            </td>
                            <td style="width: 15%; text-align: left;">
                                <span class="textEntry">SHID:</span>
                            </td>
                            <td style="width: 12%; text-align: left;">
                                <asp:TextBox ID="txtAddrSHID" CssClass="textEntry" Width="40" runat="server"></asp:TextBox>
                            </td>
                            <td style="text-align: right;">
                                <asp:CheckBox ID="chkAddrSubscriber" runat="server" CssClass="textEntry" Text="Is Subscriber"
                                    TextAlign="Left" />&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <span class="textEntry">First Name:</span>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <asp:TextBox ID="txtAddrFName" CssClass="textEntry" Width="200" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <span class="textEntry">Last Name:</span>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <asp:TextBox ID="txtAddrLName" CssClass="textEntry" Width="200" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <span class="textEntry">Bus Name:</span>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <asp:TextBox ID="txtAddrBName" CssClass="textEntry" Width="200" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <span class="textEntry">Address:</span>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <asp:TextBox ID="txtAddrAddress" CssClass="textEntry" Width="200" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <span class="textEntry">Line 2:</span>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <asp:TextBox ID="txtAddrAddressLine2" CssClass="textEntry" Width="200" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <span class="textEntry">City:</span>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <asp:TextBox ID="txtAddrCity" CssClass="textEntry" Width="200" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <span class="textEntry">State:</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtAddrState" CssClass="textEntry" Width="40" runat="server"></asp:TextBox>
                            </td>
                            <td style="text-align: right;">
                                <span class="textEntry">Zip:</span>&nbsp;&nbsp;<asp:TextBox ID="txtAddrZip" CssClass="textEntry"
                                    Width="60" runat="server"></asp:TextBox>&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <span class="textEntry">Phone No:</span>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <asp:TextBox ID="txtAddrPhoneNo" CssClass="textEntry" Width="120" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style="text-align: right; width: 600px;">
                        <asp:Button ID="btnAddrOk" OnClick="btnAddrOk_Click" CssClass="btnLabel" runat="server"
                            Text=" Ok " />&nbsp;&nbsp;&nbsp;
                        <input type="button" id="btnAddrCancel" class="btnLabel" value=" Cancel " onclick="closeDialog('AddressFinder')" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>    
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="txtBankPayeeID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtBankEquityLienID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtBankID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtEquityBankID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtAddressID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtMemberID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtAction" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtOtherYears" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtActiveFrame" runat="server"></asp:TextBox>
    </div>    
</asp:Content>
