<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="SoilLab.aspx.cs" Inherits="WSCIEMP.Fields.SoilLab"
    Title="Western Sugar Cooperative - Soil Labs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Soil Labs
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

	    var targetFrame = $$(frameName, $('#tableTabs'))[0];
		
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

    <!-- PAGE CONTENT TABLE BEGIN -->
    <table class="copyTableNoVMenu" cellspacing="0" cellpadding="0">
        <tr>
            <td style="text-align: left">
                <table style="border-right: #000000 1px solid; border-top: #000000 1px solid; border-left: #000000 1px solid;
                    border-bottom: #000000 1px solid" width="100%" border="0">
                    <tr>
                        <td style="vertical-align: middle; width: 18%">
                            <div id="ctlCropYear" runat="server">
                                <span class="LabelText" style="height: 24px">Year:&nbsp;</span>
                                <asp:DropDownList ID="ddlCropYear" runat="server" CssClass="ctlWidth60" 
                                    AutoPostBack="True" onselectedindexchanged="ddlCropYear_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td style="vertical-align: middle; width: 21%; text-align: left">
                            <div id="ctlShid" runat="server">
                                <span class="LabelText" style="height: 24px">SHID: </span>&nbsp;
                                <asp:TextBox ID="txtSHID" CssClass="ctlWidth60" runat="server"></asp:TextBox>&nbsp;
                                <asp:Button id="btnAdrFind" CssClass="BtnSm11" runat="server" Text="Find" Height="22px" onclick="btnAdrFind_Click"></asp:Button>
                            </div>
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
    <table class="copyTableNoVMenu" cellspacing="0" cellpadding="0" id="tableTabs">
        <tr>
            <td>
                <span class="LabelText">Contract Fields:</span>&nbsp;&nbsp; (<a class="permLink"
                    id="switchContractFields" onclick="ShowHide(this, 'divFields'); $$('txtContractFields', $('#tableTabs'))[0].value=this.innerHTML;"
                    href="#" runat="server">Hide</a>)
                <div id="divFields" style="border-right: black 1px solid; border-top: black 1px solid;
                    overflow: auto; border-left: black 1px solid; width: 935px; border-bottom: black 1px solid;
                    height: 200px" runat="server">
                    <br class="halfLine" />
                    <div class="DisplayOff" id="divFieldResultsEmpty" runat="server">
                    </div>
                    <asp:GridView ID="grdFieldResults" SkinID="grdColor" runat="server" 
                        Width="1100px" AutoGenerateColumns="False"
                        CssClass="grdColor920" 
                        onselectedindexchanged="grdFieldResults_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="FieldID" HeaderText="FieldID">
                                <HeaderStyle CssClass="DisplayOff" />
                                <ItemStyle CssClass="DisplayOff" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ContractNumber" HeaderText="Contract">
                                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldName" HeaderText="Field Name">
                                <HeaderStyle HorizontalAlign="Center" Width="11%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldAcres" HeaderText="Acres">
                                <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldCounty" HeaderText="County">
                                <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldTownship" HeaderText="Township">
                                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldRange" HeaderText="Range">
                                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldQuadrant" HeaderText="Quad">
                                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldQuarterQuadrant" HeaderText="Qtr Quad">
                                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldFarmNumber" HeaderText="Farm No">
                                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldNumber" HeaderText="Field No">
                                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldTractNumber" HeaderText="Tract No">
                                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldDescription" HeaderText="Field Description">
                                <HeaderStyle HorizontalAlign="Center" Width="19%"></HeaderStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <br />
                <span class="LabelText">Field's Soil Sample Lab Results</span>&nbsp;&nbsp; (<a class="permLink"
                    id="switchFieldLabResults" onclick="ShowHide(this, 'divFieldLabs'); $$('txtFieldSamples', $('#tableTabs'))[0].value=this.innerHTML;"
                    href="#" runat="server">Hide</a>)
                <div id="divFieldLabs" style="border-right: black 1px solid; border-top: black 1px solid;
                    overflow: auto; border-left: black 1px solid; width: 935px; border-bottom: black 1px solid;
                    height: 150px" runat="server">
                    <div style="width: 100%; text-align: right">
                        <asp:Button ID="btnRemoveLab" runat="server" CssClass="LabelText" 
                            Text="Remove Lab from Field" onclick="btnRemoveLab_Click">
                        </asp:Button></div>
                    <br class="halfLine" />
                    <div class="DisplayOff" id="divFieldLabResultsEmpty" runat="server">
                    </div>
                    <asp:GridView ID="grdFieldLabResults" runat="server"  SkinID="grdColor" Width="1300px" AutoGenerateColumns="False"
                        CssClass="grdColor920">
                        <Columns>
                            <asp:BoundField DataField="SoilSampleLabID" HeaderText="SoilSampleLabID">
                                <HeaderStyle CssClass="DisplayOff" />
                                <ItemStyle CssClass="DisplayOff" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ContractNumber" HeaderText="Contract">
                                <HeaderStyle Width="6%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldName" HeaderText="Field Name">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AccountNumber" HeaderText="Account No">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="LabNumber" HeaderText="Lab No">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AssocNumber" HeaderText="Assoc No">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SubmittedBy" HeaderText="Submit By">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Address1" HeaderText="Address 1">
                                <HeaderStyle Width="12%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Address2" HeaderText="Address 2">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CityStateZip" HeaderText="City State Zip">
                                <HeaderStyle Width="12%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Grower" HeaderText="Grower">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DateReceived" HeaderText="Received">
                                <HeaderStyle Width="7%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DateReported" HeaderText="Reported">
                                <HeaderStyle Width="7%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <br />
                <span class="LabelText">All Lab Results:</span>&nbsp;&nbsp; (<a class="permLink"
                    id="switchOtherLabResults" onclick="ShowHide(this, 'divOtherLabs'); $$('txtAllSamples', $('#tableTabs'))[0].value=this.innerHTML;"
                    href="#" runat="server">Hide</a>)
                <div id="divOtherLabs" style="border-right: black 1px solid; border-top: black 1px solid;
                    overflow: auto; border-left: black 1px solid; width: 935px; border-bottom: black 1px solid;
                    height: 150px" runat="server">
                    <div style="width: 100%; text-align: right">
                        <asp:Button ID="btnPrintSelected" runat="server" CssClass="LabelText" 
                            Text="Print Selected" onclick="btnPrintSelected_Click">
                        </asp:Button>&nbsp;&nbsp;
                        <asp:Button ID="btnPrintAll" runat="server" CssClass="LabelText" 
                            Text="Print All" onclick="btnPrintAll_Click">
                        </asp:Button>&nbsp;&nbsp;
                        <asp:Button ID="btnAddLab" runat="server" CssClass="LabelText" 
                            Text="Add Lab to Field" onclick="btnAddLab_Click">
                        </asp:Button></div>
                    <br class="halfLine" />
                    <div class="DisplayOff" id="divOtherLabResultsEmpty" runat="server">
                    </div>
                    <asp:GridView ID="grdOtherLabResults" runat="server" SkinID="grdColor" Width="1300px" AutoGenerateColumns="False"
                        CssClass="grdColor920">
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="" ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAllLabsSelection" runat="server" />
                                    <asp:Label ID="lblAllLabsSoilSampleLabID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "SoilSampleLabID") %>'
                                        runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SoilSampleLabID" HeaderText="SoilSampleLabID">
                                <HeaderStyle CssClass="DisplayOff" />
                                <ItemStyle CssClass="DisplayOff" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ContractNumber" HeaderText="Contract">
                                <HeaderStyle Width="6%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FieldName" HeaderText="Field Name">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AccountNumber" HeaderText="Account No">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="LabNumber" HeaderText="Lab No">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AssocNumber" HeaderText="Assoc No">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SubmittedBy" HeaderText="Submit By">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Address1" HeaderText="Address 1">
                                <HeaderStyle Width="12%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Address2" HeaderText="Address 2">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CityStateZip" HeaderText="City State Zip">
                                <HeaderStyle Width="12%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Grower" HeaderText="Grower">
                                <HeaderStyle Width="8%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DateReceived" HeaderText="Received">
                                <HeaderStyle Width="6%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DateReported" HeaderText="Reported">
                                <HeaderStyle Width="6%" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <br />
                <br />
            </td>
        </tr>
    </table>
    <!-- PAGE CONTENT TABLE END -->
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="locPDF" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtContractFields" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtFieldSamples" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtAllSamples" runat="server"></asp:TextBox>
    </div>
</asp:Content>
