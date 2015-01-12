<%@ Page Title="Harvest Reports - Daily Grower Tare Detail" Language="C#" MasterPageFile="~/HarvestReportTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="DailyGrowerTareDetail.aspx.cs" 
Inherits="WSCIEMP.Reports.HReports.DailyGrowerTareDetail" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Harvest Reports - Daily Grower Tare Detail
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<script type="text/javascript" language="javascript">
    //<![CDATA[
    function SetSubRefOptions(lstPrimaryCtrl, lstSubCtrlName, lstRefCtrlName) {

        var idx = lstPrimaryCtrl.selectedIndex;
        primaryCtrlValue = lstPrimaryCtrl.options[idx].text;

        lstRefCtrl = GetObjById(lstRefCtrlName);
        lstSubCtrl = GetObjById(lstSubCtrlName);

        ClearList(lstSubCtrl);

        var i = 0;
        for (i = 0; i < lstRefCtrl.options.length; i++) {
            if (lstRefCtrl.options[i].value == primaryCtrlValue) {

                var opt = document.createElement("OPTION");
                opt.text = lstRefCtrl.options[i].text;
                lstSubCtrl.options.add(opt);
            }
        }

    }
    //]]>       
</script>

<table class="wideTable" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td width="10">&nbsp;</td>
		<td colspan="4">&nbsp;</td>
	</tr>
	<tr>
		<td width="10">&nbsp;</td>
		<td width="90">
			<span class="LabelText">Choose<br />a Contract:</span><br />
			<asp:listbox id="lstDgtdContract" CssClass="MedList" Rows="10" runat="server"></asp:listbox><br />
		</td>
		<td width="25">&nbsp;</td>
		<td width="160">
			<div>
				<span class="LabelText">Optionally, Choose one<br />
				or more Delivery Dates:</span><br />
				<asp:listbox id="lstDgtdDeliveryDate" CssClass="MedList" Rows="10" runat="server" SelectionMode="Multiple"></asp:listbox>
			</div>
			<div class="DisplayOff">
				<asp:listbox id="lstDgtdRefDeliveryDate" CssClass="MedList" Rows="10" runat="server"></asp:listbox>
			</div>
		</td>
		<td class="ButtonText" style="VERTICAL-ALIGN: middle" width=*>(Hold down the Ctrl key to
			select multiple dates.)
		</td>
	</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
