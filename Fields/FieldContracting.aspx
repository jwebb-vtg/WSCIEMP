<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="FieldContracting.aspx.cs" Inherits="WSCIEMP.Fields.FieldContracting"
    Title="Western Sugar Cooperative - Field Contracting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Field Contracting
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script language="javascript" type="text/javascript">
//<![CDATA[	
	function DoOnload() {

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

    <table style="width: 99%; padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
        margin: 0px; vertical-align: bottom; padding-top: 0px" cellspacing="0" cellpadding="0"
        border="0">
        <tr>
            <td class="ButtonText" width="1">
                &nbsp;
            </td>
            <td style="text-align: left">
            <!-- !! UsrCntSelector goes here !! -->
            <uc:UCSelector id="UsrCntSelector" runat="server"></uc:UCSelector>
            </td>
            <td class="ButtonText" width="1">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td width="1">
                &nbsp;
            </td>
            <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                vertical-align: top; padding-top: 0px" colspan="2">
                <br />
                <table width="98%" border="0">
                    <tr>
                        <td class="ButtonText" colspan="3">
                            <span class="LabelText">Email: </span>
                            <asp:TextBox CssClass="ButtonText" ID="txtEmail" runat="server" Columns="50"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSaveAddress" runat="server" Text="Save Email" 
                                CssClass="BtnMed" onclick="btnSaveAddress_Click">
                            </asp:Button>                            
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td width="1">
                &nbsp;
            </td>
            <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                vertical-align: top; padding-top: 0px" colspan="2">
                <br style="line-height:9px;"/>
                <span class="LabelText" style="font-style: italic; font-size: 12px;">Field Description:</span>
                <div style="border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    margin-right: 10px; border-bottom: black 1px solid">
                    <table style="vertical-align: bottom" width="98%" border="0">
                        <tr>
                            <td class="LabelText" style="width:13%;">
                                WSC Field Name:
                            </td>
                            <td class="ButtonText" style="vertical-align: bottom; width:13.5%;">
                                <asp:Label ID="lblFieldName" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText" style="width:11%;">
                                Acres:
                            </td>
                            <td class="ButtonText" style="width:13.5%;">
                                <asp:Label ID="lblAcres" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText" style="width:11%;">
                                State:
                            </td>
                            <td class="ButtonText" style="width:13.5%;">
                                <asp:Label ID="lblState" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText" style="width:11%;">
                                County:
                            </td>
                            <td style="width:13.5%;">
                                <asp:Label ID="lblCounty" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                1/4 Quadrant:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblQuarterQuadrant" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                Quadrant:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblQuadrant" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                Section:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblSection" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                Township:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblTownship" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="LabelText">
                                Range:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblRange" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                Latitude:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblLatitude" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                Longitude:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblLongitude" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                FSA Official:
                            </td>
                            <td>
                                <asp:Label ID="lblFsaOfficial" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="LabelText">
                                FSA Number:
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblFsaNumber" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                FSA State:
                            </td>
                            <td>
                                <asp:Label ID="lblFsaState" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                FSA County:
                            </td>
                            <td>
                                <asp:Label ID="lblFsaCounty" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="LabelText">
                                Farm No:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblFarmNo" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                Tract No:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblTractNo" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                Field No:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblFieldNo" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                            <td class="LabelText">
                                1/4 Field:
                            </td>
                            <td class="ButtonText">
                                <asp:Label ID="lblQuarterField" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                Description:
                            </td>
                            <td class="ButtonText" colspan="7">
                                <asp:Label ID="lblDescription" runat="server" CssClass="ButtonText"></asp:Label>
                            </td>
                        </tr>                        
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td width="1">
                &nbsp;
            </td>
            <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                vertical-align: top; padding-top: 0px" colspan="2">
                <!-- Field Contract -->
                <br style="line-height:9px;"/>
                <span class="LabelText" style="font-style: italic; font-size: 12px;">Contracting Information:</span>
                <div style="border-right: black 1px solid; padding-right: 0px; border-top: black 1px solid;
                    padding-left: 0px; padding-bottom: 0px; border-left: black 1px solid; margin-right: 10px;
                    padding-top: 0px; border-bottom: black 1px solid">
                    <table style="width:98%;" border="0">
                        <tr style="height: 3px;">
                            <th style="width: 31%"></th>
                            <th style="width: 26%"></th>
                            <th style="width: 17%"></th>
                            <th></th>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Land Ownership:
                            </td>
                            <td colspan="3" class="ButtonText" style="height: 26px">
                                <asp:DropDownList ID="ddlLandOwnership" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Rotation Length (years):
                            </td>
                            <td class="ButtonText">
                                <asp:DropDownList ID="ddlRotationLength" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                            <td class="LabelText" style="height: 20px">
                                Prior Crop:
                            </td>
                            <td style="height: 20px">
                                <asp:DropDownList ID="ddlPriorCrop" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText" style="height: 20px">
                                Years Field Has Had Beets:
                            </td>
                            <td style="height: 20px">
                                <asp:DropDownList ID="ddlBeetYears" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                            <td class="LabelText" style="height: 20px">
                                Tillage:
                            </td>
                            <td style="height: 20px">
                                <asp:DropDownList ID="ddlTillage" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table width="98%" border="0">
                        <tr>
                            <th width="31%">
                            </th>
                            <th width="26%">
                            </th>
                            <th width="*">
                            </th>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <span class="LabelText">Pre-Harvest</span>
                            </td>
                            <td>
                                <span class="LabelText">Post-Harvest</span>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Rhizomania Suspect Field:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkRhizomania" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostRhizomania" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText" style="height: 11px">
                                Aphanomyces Suspect Field:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkAphanomyces" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostAphanomyces" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Curly Top Suspect Area:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkCurlyTop" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostCurlyTop" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Fusarium Suspect Field:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkFusarium" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostFusarium" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Rhizoctonia Suspect Field:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkRhizoctonia" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostRhizoctonia" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Nematode Suspect Field:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkNematode" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostNematode" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Cercospora Suspect Field:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkCercospora" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostCercospora" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Root Aphid Suspect Area:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkRootAphid" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostRootAphid" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Powdery Mildew Suspect Field:
                            </td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkPowderyMildew" runat="server" CssClass="LabelText"></asp:CheckBox>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPostPowderyMildew" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Irrigation System:
                            </td>
                            <td>
                                &nbsp;<asp:DropDownList ID="ddlIrrigation" runat="server" CssClass="MedListPlus">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="tRowH25">
                            <td class="LabelText">
                                Water Source:
                            </td>
                            <td>
                                &nbsp;<asp:DropDownList ID="ddlWaterSource" runat="server" CssClass="MedListPlus">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="LabelText">Water: </span>&nbsp;<asp:CheckBox ID="chkPostWater" runat="server"
                                    CssClass="LabelText"></asp:CheckBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
                <br style="line-height:7px;"/>
                <table width="98%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnPrintForm" runat="server" Text="Print Form" 
                                CssClass="BtnMed" onclick="btnPrintForm_Click">
                            </asp:Button>&nbsp;&nbsp;
                            <asp:Button ID="btnSaveContract" runat="server" Text="Save" 
                                CssClass="BtnMed" onclick="btnSaveContract_Click">
                            </asp:Button>
                        </td>
                    </tr>
                </table>
                <br style="line-height:7px;"/>
            </td>
        </tr>
    </table>
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="locPDF" runat="server" CssClass="DisplayOff"></asp:TextBox></div>
</asp:Content>
