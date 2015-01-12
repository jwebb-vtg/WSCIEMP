<%@Control Language="C#" AutoEventWireup="true" CodeBehind="UserContractSelector.ascx.cs" 
Inherits="WSCIEMP.UControls.UserContractSelector" %>
<table id="userContractSelectorTab" style="width: 99.5%; BORDER-RIGHT: #000000 1px solid; BORDER-TOP: #000000 1px solid; BORDER-LEFT: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid" border="0">
	<tr>
		<td style="VERTICAL-ALIGN: middle; WIDTH: 16%">
			<div id="ctlCropYear" runat="server">
				<span class="LabelText" style="HEIGHT: 24px">Year:&nbsp;</span>
				<asp:dropdownlist id="ddlCropYear" runat="server" CssClass="ctlWidth60" 
                    AutoPostBack="True" onselectedindexchanged="ddlCropYear_SelectedIndexChanged"></asp:dropdownlist></div>
		</td>
		<td style="VERTICAL-ALIGN: middle; WIDTH: 20%; TEXT-ALIGN: left">
			<div id="ctlShid" runat="server"><span class="LabelText" style="HEIGHT: 24px">SHID:&nbsp;</span>
			    <asp:TextBox ID="txtSHID" CssClass="ctlWidth60" runat="server"></asp:TextBox>&nbsp;
				<asp:Button id="btnAdrFind" CssClass="BtnSm11" runat="server" Text="Find" 
                    Height="22px" onclick="btnAdrFind_Click"></asp:Button></div>
		</td>
		<td style="WIDTH: 9%"><span class="LabelText" style="HEIGHT: 24px">Contract:&nbsp;</span></td>
		<td style="WIDTH: 18%; TEXT-ALIGN: left">
			<div id="ctlCntNo" runat="server">
				<asp:dropdownlist id="ddlContractNumber" CssClass="ctlWidth75" runat="server" 
                    AutoPostBack="True" 
                    onselectedindexchanged="ddlContractNumber_SelectedIndexChanged"></asp:dropdownlist>&nbsp;
				<asp:Button id="btnCntFind" CssClass="BtnSm" runat="server" Text="..." Height="22px"></asp:Button>
			</div>
		</td>
		<td>
			<div id="divCntNav" runat="server">
				<asp:button id="btnPrevContractNo" CssClass="BtnMed" runat="server" 
                    Text="Prev Cnt" Height="22" onclick="btnPrevContractNo_Click"></asp:button>
				<asp:button id="btnNextContractNo" CssClass="BtnMed" runat="server" 
                    Text="Next Cnt" Height="22" onclick="btnNextContractNo_Click"></asp:button>
			</div>
		</td>
	</tr>
	<tr>
		<td colspan="2"><span class="LabelText">Name: </span>
			<asp:label id="lblBusName" CssClass="ButtonText" runat="server"></asp:label></td>
		<td>
			<div id="divSequence1" runat="server"><span class="LabelText">Field:&nbsp;</span></div>
		</td>
		<td style="TEXT-ALIGN: left">
			<div id="divSequence2" runat="server">
				<asp:dropdownlist id="ddlSequence" CssClass="SmList" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlSequence_SelectedIndexChanged"></asp:dropdownlist>
			</div>
		</td>
		<td>
			<span class="LabelText">Agriculturist: </span>
			<asp:label id="lblAgriculturist" CssClass="ButtonText" runat="server">&nbsp;</asp:label>
		</td>
	</tr>
	<tr>
		<td colspan="5"><span class="LabelText">Land Owner: </span>
			<asp:label id="lblLandOwner" CssClass="ButtonText" runat="server">&nbsp;</asp:label></td>
	</tr>
</table>
<div class="DisplayOff" id="UserContractSelectorData">
	<asp:textbox ID="txtQuery" runat="server"></asp:textbox>
	<asp:textbox ID="txtQueryAction" runat="server"></asp:textbox>
    <asp:Button ID="btnSvrFindContract" runat="server" Text="" 
        CssClass="DisplayOff" onclick="btnSvrFindContract_Click" />
</div>
<br class="vSpacer" />
