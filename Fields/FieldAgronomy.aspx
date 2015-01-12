<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="True" CodeBehind="FieldAgronomy.aspx.cs" Inherits="WSCIEMP.Fields.FieldAgronomy"
    Title="Western Sugar Cooperative - Field Agronomy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Field Agronomy
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
//]]>						
    </script>

    <div class="ButtonText" style="text-align: center">
        ( Enter all dates in MM/DD format )</div>
    <table class="copyTableNoVMenu" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="ButtonText" style="width:1%;">
                &nbsp;
            </td>
            <td style="padding-left: 1px; text-align: left" align="left" colspan="2">
                <!-- !! UsrCntSelector goes here !! -->
                <uc:UCSelector id="UsrCntSelector" runat="server"></uc:UCSelector>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td style="padding-right: 0; padding-left: 0; padding-bottom: 0; margin: 0; 
                vertical-align: top; padding-top: 0px;">
                <div class="tableBorder" style="width:99%;">
                    <table style="width:100%;" border="0">
                        <tr>
                            <td style="width: 15%">
                                <span class="LabelText">WSC Field Name: </span>
                            </td>
                            <td style="width: 20%">
                                <asp:Label ID="lblFieldName" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="ButtonText" style="width: 8%">
                                <span class="LabelText">Acres:</span>
                            </td>
                            <td style="width: 10%">
                                <asp:Label ID="lblAcres" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td style="width: 15%">
                                <span class="LabelText">FSA Number: </span>
                            </td>
                            <td>
                                <asp:Label ID="lblFsaNumber" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="ButtonText">
                                <span class="LabelText">Desc: </span>
                            </td>
                            <td colspan="5">
                                <asp:Label ID="lblDesc" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <br class="vSpacer" />
                <div class="tableBorder" style="width:99%;">
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th style="width: 15%">
                            </th>
                            <th style="width: 19%">
                            </th>
                            <th style="width: 17%">
                            </th>
                            <th style="width: 19%">
                            </th>
                            <th style="width: 14%">
                            </th>
                            <th style="width: 16%">
                            </th>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <span class="LabelText" style="font-style: italic">-- Planting --</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">Variety</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVariety" runat="server" CssClass="MedListPlus">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Seed</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSeed" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: top" rowspan="3">
                                <span class="LabelText">Treatment</span>
                            </td>
                            <td style="vertical-align: top" rowspan="3">
                                <asp:CheckBox ID="chkSeedRxCruiser" Text="Cruiser" runat="server" CssClass="LabelText">
                                </asp:CheckBox><br />
                                <asp:CheckBox ID="chkSeedRxPoncho" Text="Poncho" runat="server" CssClass="LabelText">
                                </asp:CheckBox><br />
                                <asp:CheckBox ID="chkSeedRxTachigaren" Text="Tachigaren" runat="server" CssClass="LabelText">
                                </asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">Row Spacing</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRowSpacing" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Plant Spacing</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPlantSpacing" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">Plant Date</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPlantingDate" runat="server" CssClass="ButtonText" Columns="10"></asp:TextBox>
                            </td>
                            <td>
                                <span class="LabelText">80% Emerg Date</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txt80EmergDate" runat="server" CssClass="ButtonText" Columns="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <span class="LabelText" style="font-style: italic">-- Replanting --</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">Replant Date</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReplantingDate" runat="server" CssClass="ButtonText" Columns="10"></asp:TextBox>
                            </td>
                            <td>
                                <span class="LabelText">Replant Variety</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlReplantVariety" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Replant Acres</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReplantAcres" runat="server" CssClass="ButtonText" Columns="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">Replant Reason</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlReplantReason" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Reason Lost</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLostReason" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Lost Acres</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLostAcres" runat="server" CssClass="ButtonText" Columns="10"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br class="vSpacer" />
                <div class="tableBorder" style="width:99%;">
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th width="28%">
                            </th>
                            <th width="23%">
                            </th>
                            <th width="24%">
                            </th>
                            </TH></tr>
                        <tr>
                            <td>
                                <span class="LabelText">Soil Texture</span>
                                <asp:DropDownList ID="ddlSoilTexture" runat="server" CssClass="MedListPlus">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Soil Sample</span>
                                <asp:DropDownList ID="ddlSoilTest" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">N Sample Depth</span>
                                <asp:DropDownList ID="ddlSampleDepth" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Grid/Zone Sample</span>
                                <asp:DropDownList ID="ddlSampleGridZone" runat="server" CssClass="SmList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th style="width: 2%">
                            </th>
                            <th style="width: 17%">
                            </th>
                            <th style="width: 16%">
                            </th>
                            <th style="width: 17%">
                            </th>
                            <th style="width: 16%">
                            </th>
                            <th style="width: 15%">
                            </th>
                            <th>
                            </th>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <span class="LabelText" style="font-style: italic">-- Test Results --</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <span class="LabelText">N (lbs)</span>
                                <asp:TextBox ID="txtTestedN" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td>
                                <span class="LabelText">P </span>
                                <asp:DropDownList ID="ddlTestedP" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">K (ppm)</span>
                                <asp:TextBox ID="txtTestedK" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td>
                                <span class="LabelText">Salts</span>
                                <asp:TextBox ID="txtTestedSalts" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td>
                                <span class="LabelText">pH</span>
                                <asp:TextBox ID="txtTestedpH" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td>
                                <span class="LabelText">O.M. (%)</span>
                                <asp:TextBox ID="txtTestedOm" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br class="vSpacer" />
                <div class="tableBorder" style="width:99%;">
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th width="12%">
                            </th>
                            <th width="9%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="9%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="*">
                            </th>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="LabelText">Last Yr Manure Applied</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlManureYear" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td align="center" colspan="8">
                                <span class="LabelText">Actual units/Acre (Average over whole field)</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span class="LabelText">Fall Fertilizer Applied</span>
                            </td>
                            <td align="center">
                                <span class="LabelText">N (lbs)</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFertFallN" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td align="center">
                                <span class="LabelText">P (lbs)</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFertFallP" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td align="center">
                                <span class="LabelText">K (lbs) </span>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtFertFallK" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span class="LabelText">Spring Fertilizer Applied </span>
                            </td>
                            <td align="center">
                                <span class="LabelText">N (lbs) </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFertSpringN" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td align="center">
                                <span class="LabelText">P (lbs) </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFertSpringP" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td align="center">
                                <span class="LabelText">K (lbs) </span>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtFertSpringK" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span class="LabelText">In Season Fertilizer Applied </span>
                            </td>
                            <td align="center">
                                <span class="LabelText">N (lbs) </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFertSeasonN" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td align="center">
                                <span class="LabelText">P (lbs) </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFertSeasonP" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                            <td align="center">
                                <span class="LabelText">K (lbs) </span>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtFertSeasonK" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="LabelText">Starter Fertilizer</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlFertStarter" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td colspan="8">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <br class="vSpacer" />
                <div class="tableBorder" style="width:99%;">
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th width="13%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="22%">
                            </th>
                            <th width="6%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="9%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="9%">
                            </th>
                            <th width="*">
                            </th>
                        </tr>
                        <tr>
                            <td style="height: 34px" colspan="2">
                                <span class="LabelText">Pre-Emerg. Insecticide</span>
                            </td>
                            <td style="height: 34px" align="left">
                                <asp:DropDownList ID="ddlPreInsecticide" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="height: 34px" align="left">
                                <span class="LabelText">&nbsp;Post-Emerg. Insecticide: </span>
                            </td>
                            <td style="height: 34px" align="left" colspan="2">
                                <asp:DropDownList ID="ddlPostInsecticide" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="height: 34px" align="left" colspan="3">
                                &nbsp;
                            </td>
                            <td style="height: 34px" align="left">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th width="13%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="9%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="*">
                            </th>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="LabelText">Insecticide-Root Maggot</span>
                            </td>
                            <td colspan="9">
                                <asp:DropDownList ID="ddlRootMaggot" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: bottom" width="100%" colspan="11">
                                <table>
                                    <tr>
                                        <td align="left" width="22%">
                                            <span class="LabelText">Chemical (Rate Used)</span>
                                        </td>
                                        <td align="center" width="26%">
                                            <span class="LabelText">Counter 15G (lbs/A) </span>
                                            <asp:TextBox ID="txtChemCounter" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                                        </td>
                                        <td align="center" width="26%">
                                            <span class="LabelText">Temik (lbs/A)</span>
                                            <asp:TextBox ID="txtChemTemik" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                                        </td>
                                        <td align="center" width="26%">
                                            <span class="LabelText">Thimet (lbs/A)</span>
                                            <asp:TextBox ID="txtChemThimet" runat="server" CssClass="ButtonText" Columns="7"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th width="13%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="10%">
                            </th>
                            <th width="16%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="9%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="9%">
                            </th>
                            <th width="*">
                            </th>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <span class="LabelText" style="font-style: italic">-- Weed Control --</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="LabelText">Pre-Emerg. Weed Control</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPreWeedControl" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <span class="LabelText">Number of Herbicide Treatments</span>
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="ddlHerbicideRxCount" runat="server" CssClass="SmList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="LabelText">Layby Herbicide</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlIsLaybyHerbicide" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Chemical</span>
                            </td>
                            <td colspan="6">
                                <asp:DropDownList ID="ddlLaybyHerbicide" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th width="33%">
                            </th>
                            <th width="16%">
                            </th>
                            <th width="20%">
                            </th>
                            <th width="7%">
                            </th>
                            <th width="8%">
                            </th>
                            <th width="*">
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText" style="font-style: italic">-- Cercospora Program --</span>
                            </td>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">&nbsp;Application 1</span>
                            </td>
                            <td>
                                <span class="LabelText">&nbsp;Chemical</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlApp1Chem" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <span class="LabelText">Date</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApp1Date" runat="server" CssClass="ButtonText" Columns="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">&nbsp;Application 2</span>
                            </td>
                            <td>
                                <span class="LabelText">&nbsp;Chemical</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlApp2Chem" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <span class="LabelText">Date</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApp2Date" runat="server" CssClass="ButtonText" Columns="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="LabelText">&nbsp;Application 3</span>
                            </td>
                            <td>
                                <span class="LabelText">&nbsp;Chemical</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlApp3Chem" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <span class="LabelText">Date</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApp3Date" runat="server" CssClass="ButtonText" Columns="10"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br class="vSpacer" />
                <div class="tableBorder" style="width:99%;">
                    <table style="vertical-align: bottom" width="100%" border="0">
                        <tr>
                            <th width="13%">
                            </th>
                            <th width="9%">
                            </th>
                            <th width="11%">
                            </th>
                            <th width="22%">
                            </th>
                            <th width="6%">
                            </th>
                            <th width="6%">
                            </th>
                            <th width="3%">
                            </th>
                            <th width="15%">
                            </th>
                            <th width="*">
                            </th>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="LabelText">Treated Powdery Mildew</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTreatedPowderyMildew" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">&nbsp;Hail Stress</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlHailStress" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <span class="LabelText">Weed Control</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlWeedControl" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span class="LabelText">Treated for Nematode</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTreatedNematode" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">&nbsp;Treated for Rhizoctonia</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlTreatedRhizoctonia" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <table style="vertical-align: bottom" width="100%" border="0">
                    <tr>
                        <th style="width: 56%">
                        </th>
                        <th style="width: 15%">
                        </th>
                        <th style="width: 10%">
                        </th>
                        <th>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <span class="LabelText">Field Comment: (200 character max.)</span>
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="3">
                            <asp:TextBox ID="txtComment" CssClass="ButtonText" Columns="67" Rows="5" runat="server"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td align="right">
                            <span class="LabelText">Reviewed: </span>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="ddlReviewed" runat="server" CssClass="SmList">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnSave" runat="server" Text="Save Form" 
                                onclick="btnSave_Click"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span class="LabelText">Include Data</span>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="ddlIncludeData" runat="server" CssClass="SmList">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnPrint" runat="server" Text="Print Form" 
                                onclick="btnPrint_Click"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="locPDF" runat="server" CssClass="DisplayOff"></asp:TextBox>
    </div>
</asp:Content>
