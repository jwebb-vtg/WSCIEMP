<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="ThemeA_1024" CodeBehind="TextEntry.aspx.cs" Inherits="WSCIEMP.UControls.TextEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Enter Value</title>
		<script type="text/javascript" language=javascript>				

	function DoCancel() {
		window.close();
	}
	
	function DoOk(clkCtrlName) {
		
		var entry = GetObjById("txtTextEntry");
		if (entry.value != "") {
		
			var mstCtlName = GetObjById('MasterControlName');
			var actCtlName = GetObjById('ActionControlName');
			var actCtl = GetObjById('Action');
			window.opener.SetText(actCtlName.value, actCtl.value, mstCtlName.value, entry.value);
			
			if (clkCtrlName != '') {
	            var ctrl = window.opener.GetObjById(clkCtrlName);
	            ctrl.click();	
			}
			
			BubbleOff();
			window.close();		
		}		
	}
	
	function DoOnload() {
		var ctl = GetObjById('txtTextEntry');
		if (ctl != null) {
			ctl.focus();
		}
	}
		</script>    
</head>
<body onload="DoOnload();">
    <form id="form1" runat="server">
            <div class="WarningOff" id="divWarning" runat="server"></div>    
			<div style="text-align:center">
				<asp:Label id="lblTextEntryLabel" CssClass="LabelText" runat="server"></asp:Label>
				<br />
				<asp:TextBox id="txtTextEntry" runat="server" CssClass="ButtonText" Width="160"></asp:TextBox>
				<br />
			</div>
			<br />
			<div style="TEXT-ALIGN:center">
				<asp:Button id="btnOk" runat="server" CssClass="BtnMed" Text="Ok"></asp:Button>
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<input class="BtnMed" id="btnCancel" onclick="DoCancel();" type="button" value="Cancel" />
			</div>
			<div class="DisplayOff">				
				<asp:TextBox ID="MasterControlName" runat="server"></asp:TextBox>
				<asp:TextBox ID="ActionControlName" runat="server"></asp:TextBox>
				<asp:TextBox ID="Action" runat="server"></asp:TextBox>
			</div>
    </form>
</body>
</html>
