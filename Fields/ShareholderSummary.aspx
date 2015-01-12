<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="ShareholderSummary.aspx.cs" Inherits="WSCIEMP.Fields.ShareholderSummary"
    Title="Western Sugar Cooperative - Shareholder Summary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Shareholder Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script language="javascript" type="text/javascript">
//<![CDATA[

	function DoOnLoad() {

	    var locPDF = $$('locPDF', $('#PostData'))[0];
		if (locPDF != null && locPDF.value.length > 0) {

			var url = locPDF.value;
			var popWidth = screen.width-150;
			var popHeight = screen.height-150;

			var win = window.open(url, "_blank", 
				"status=0,toolbar=1,location=0,top=100,left=100,menubar=1,directories=0,resizable=1,scrollbars=1,width=" + popWidth + ",height=" + popHeight);									
		}
	}	
	
	function ShowHide(objCaller, frameName) {

	    var targetFrame = $$(frameName, $('#PerformanceInfo'))[0];
		
		if (targetFrame != null) {
			if (objCaller.innerHTML == 'Show') {		
				objCaller.innerHTML = 'Hide';
				targetFrame.className = "DisplayOn";
			} else {
				objCaller.innerHTML = 'Show';
				targetFrame.className = "DisplayOff";
			}
		}				
	}

//]]>			
    </script>

    <table class="copyTableNoVMenu" cellspacing="0" cellpadding="0">
        <tr>
            <td style="text-align: left">
                <table style="border-right: #000000 1px solid; border-top: #000000 1px solid; border-left: #000000 1px solid;
                    border-bottom: #000000 1px solid" width="100%" border="0">
                    <tr>
                        <td style="vertical-align: middle; width: 18%">
                            <div id="ctlCropYear" runat="server">
                                <span class="LabelText" style="height: 24px">Year:&nbsp;</span>
                                <asp:DropDownList ID="ddlCropYear" runat="server" AutoPostBack="True" 
                                    CssClass="ctlWidth60" onselectedindexchanged="ddlCropYear_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td style="vertical-align: middle; width: 21%; text-align: left">
                            <div id="ctlShid" runat="server">
                                <span class="LabelText" style="height: 24px">SHID: </span>&nbsp;
                                <asp:TextBox ID="txtSHID" CssClass="ctlWidth60" runat="server"></asp:TextBox>&nbsp;
                                <asp:Button ID="btnAdrFind" runat="server" CssClass="BtnSm11" Height="22px" 
                                    Text="Find" onclick="btnAdrFind_Click">
                                </asp:Button></div>
                        </td>
                        <td style="width: 61%; text-align: left">
                            <span class="LabelText">Name: </span>
                            <asp:Label ID="lblBusName" runat="server" CssClass="ButtonText">&nbsp;</asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table cellpadding="0" style="width:99%">
        <tr>
            <td>
                <span class="LabelText">Select a Region/Area to review Contract Performance and edit
                    Summary recommendations.</span>
                <div class="DisplayOff" id="divRegionAreaEmpty" runat="server">
                </div>
                <asp:GridView ID="grdRegionArea" runat="server" SkinID="grdColor" CellPadding="1" AutoGenerateColumns="False" CssClass="grdColor920"  
                    onselectedindexchanged="grdRegionArea_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="GrowerPerformanceID" HeaderText="GrowerPerformanceID">
                            <HeaderStyle CssClass="DisplayOff" />
                            <ItemStyle CssClass="DisplayOff" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RegionCode" HeaderText="RegionCode">
                            <HeaderStyle CssClass="DisplayOff" />
                            <ItemStyle CssClass="DisplayOff" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AreaCode" HeaderText="AreaCode">
                            <HeaderStyle CssClass="DisplayOff" />
                            <ItemStyle CssClass="DisplayOff" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RegionName" HeaderText="Region">
                            <HeaderStyle HorizontalAlign="Center" Width="22%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="AreaName" HeaderText="Area Name">
                            <HeaderStyle HorizontalAlign="Center" Width="44%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="FinalNetTons" HeaderText="Final Net Tons" DataFormatString="{0:N4}">
                            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TonsPerAcre" HeaderText="Tons / Acre" DataFormatString="{0:N1}">
                            <HeaderStyle HorizontalAlign="Center" Width="9%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="HasAdvice" HeaderText="Has Summary">
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="0" style="width:99%" id="PerformanceInfo">
        <tr>
            <td>
                <br />
                <span class="LabelText">Contract Performance:</span>&nbsp;&nbsp; (<a class="permLink"
                    id="switchContractPerf" onclick="ShowHide(this, 'divPerf'); $$('txtContractPerf', $('#PostData'))[0].value=this.innerHTML;"
                    href="#" runat="server">Hide</a>)
                <div id="divPerf" style="border-right: black 1px solid; border-top: black 1px solid;
                    overflow: auto; border-left: black 1px solid; width: 935px; border-bottom: black 1px solid;
                    height: 200px" runat="server">
                    <br class="halfLine" />
                    <div class="DisplayOff" id="divPerfResultsEmpty" runat="server">
                    </div>
                    <asp:Table ID="tabPerfResults" runat="server" CellPadding="2" CellSpacing="0">
                    </asp:Table>
                    <br />
                </div>
            </td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="0" style="width:99%">
        <tr>
            <td>
                <table style="width: 100%" cellspacing="5" border="0">
                    <tr>
                        <td style="width: 80%; text-align: right">
                            <asp:Button ID="btnSave" runat="server" CssClass="BtnMed" Text="Save" 
                                onclick="btnSave_Click"></asp:Button>
                        </td>
                        <td style="width: 10%; text-align: right">
                            <asp:Button ID="btnPrint" runat="server" CssClass="BtnMed" Text="Print" 
                                onclick="btnPrint_Click"></asp:Button>
                        </td>
                        <td style="width: 10%; text-align: right">
                            <asp:Button ID="btnPrintAll" runat="server" CssClass="BtnMed" 
                                Text="Print All" onclick="btnPrintAll_Click">
                            </asp:Button>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%" cellspacing="5" border="0">
                    <tr>
                        <td style="border-right: #000000 1px solid; border-top: #000000 1px solid; font-weight: bold;
                            font-size: 14px; vertical-align: middle; border-left: #000000 1px solid; width: 75%;
                            border-bottom: #000000 1px solid; text-align: center">
                            Big Six Grower Practices
                        </td>
                        <td style="border-right: #000000 1px solid; border-top: #000000 1px solid; font-weight: bold;
                            font-size: 14px; vertical-align: middle; border-left: #000000 1px solid; width: 10%;
                            border-bottom: #000000 1px solid; text-align: center">
                            Okay
                        </td>
                        <td style="border-right: #000000 1px solid; border-top: #000000 1px solid; font-weight: bold;
                            font-size: 14px; vertical-align: middle; border-left: #000000 1px solid; width: 15%;
                            border-bottom: #000000 1px solid; text-align: center">
                            Needs to<br />
                            Improve
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 75%;
                            text-align: left">
                            <br />
                            Fertility Management<br />
                            <span style="font-weight: normal; font-size: 11px">(Recommendation)</span>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 10%;
                            text-align: center">
                            <asp:CheckBox ID="chkFertilityOkay" runat="server"></asp:CheckBox>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 15%;
                            text-align: center">
                            <asp:CheckBox ID="chkFertilityBad" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtFertilityRec" Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 75%;
                            text-align: left">
                            Irrigation Water Management<br />
                            <span style="font-weight: normal; font-size: 11px">(Recommendation)</span>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 10%;
                            text-align: center">
                            <asp:CheckBox ID="chkIrrigationOkay" runat="server"></asp:CheckBox>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 15%;
                            text-align: center">
                            <asp:CheckBox ID="chkIrrigationBad" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtIrrigationRec" Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 75%;
                            text-align: left">
                            Stand Establishment (Harvest Plant Population)<br />
                            <span style="font-weight: normal; font-size: 11px">(Recommendation)</span>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 10%;
                            text-align: center">
                            <asp:CheckBox ID="chkStandOkay" runat="server"></asp:CheckBox>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 15%;
                            text-align: center">
                            <asp:CheckBox ID="chkStandBad" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtStandRec" Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 75%;
                            text-align: left">
                            Weed Control<br />
                            <span style="font-weight: normal; font-size: 11px">(Recommendation)</span>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 10%;
                            text-align: center">
                            <asp:CheckBox ID="chkWeedOkay" runat="server"></asp:CheckBox>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 15%;
                            text-align: center">
                            <asp:CheckBox ID="chkWeedBad" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtWeedRec" Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 75%;
                            text-align: left">
                            Disease &amp; Insect Control<br />
                            <span style="font-weight: normal; font-size: 11px">(Recommendation)</span>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 10%;
                            text-align: center">
                            <asp:CheckBox ID="chkDiseaseOkay" runat="server"></asp:CheckBox>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 15%;
                            text-align: center">
                            <asp:CheckBox ID="chkDiseaseBad" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtDiseaseRec" Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 75%;
                            text-align: left">
                            Proper Variety Selection<br />
                            <span style="font-weight: normal; font-size: 11px">(Recommendation)</span>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 10%;
                            text-align: center">
                            <asp:CheckBox ID="chkVarietyOkay" runat="server"></asp:CheckBox>
                        </td>
                        <td style="font-weight: bold; font-size: 14px; vertical-align: bottom; width: 15%;
                            text-align: center">
                            <asp:CheckBox ID="chkVarietyBad" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtVarietyRec" Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </td>
        </tr>
    </table>
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="locPDF" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtContractPerf" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtGrowerPerformanceID" runat="server"></asp:TextBox>
    </div>
</asp:Content>
