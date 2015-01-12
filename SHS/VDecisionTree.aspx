<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="VDecisionTree.aspx.cs" Inherits="WSCIEMP.SHS.VDecisionTree"
    Title="Western Sugar Cooperative - Seed Decision Tree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Seed Decision Tree
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script language="javascript" defer="defer" type="text/javascript">		

function ShowNursery(nurseryName) {

	// check for required selection
	var url = "../ZHost/SeedDecisionTree/" + nurseryName + ".html";
	var popWidth = screen.width * .8;  //775;	//screen.width-200;
	var popHeight = screen.height * .75; //590; //screen.height-200;
	
	var top = (screen.height-popHeight)/2;
	var left = (screen.width-popWidth)/2;
	
	if ( top <= 0 ) {
		top = 25;
	}
	if (left <= 0) {
		left = 25;
	}

	var win = window.open(url, "_blank", 
		"status=0,toolbar=0,top=" + top + ",left=" + left + ",location=0,menubar=1,directories=0,resizable=1,scrollbars=1,width=" + popWidth + ",height=" + popHeight);
}

</script>

    <table cellspacing="0" cellpadding="0" border="0" style="width: 100%">
        <tr>
            <td style="width: 2px">
                &nbsp;
            </td>
            <td>
                <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td style="vertical-align: bottom; width: 18%; height: 20px; text-align: left">
                            <span class="LabelText">SHID</span>&nbsp;&nbsp;
                            <asp:TextBox ID="txtSHID" runat="server" CssClass="ButtonText" Columns="12"></asp:TextBox>
                        </td>
                        <td style="vertical-align: bottom; width: 8%; height: 20px; text-align: left">
                            <asp:Button ID="btnFind" runat="server" CssClass="BtnMed" Text="Find" OnClick="btnFind_Click">
                            </asp:Button>
                        </td>
                        <td style="vertical-align: bottom; height: 20px; text-align: left">
                            &nbsp; <span class="LabelText">Business Name: </span>
                            <asp:Label ID="lblBusName" runat="server" CssClass="ButtonText"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 5px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td style="width: 34%;">
                            &nbsp;
                        </td>
                        <td style="width: 37%;">
                            &nbsp;
                        </td>
                        <td rowspan="9" style="padding-right: 4px;">
                            <br style="line-height: 4px" />
                            &nbsp;
                            <div class="BlockNav" id="divNurseryNorth" style="background-color: #ffffcc; text-align: center" runat="server">
                                <br style="line-height: 8px" />
                                &nbsp; <span class="LabelText">North Nursery Data</span><br />
                                <a id="A6" onclick="ShowNursery('Aphanomyces-North');" href="#">Aphanomyces &gt;&gt;</a><br />
                                <a id="northD1" onclick="ShowNursery('Cercospora-North');" href="#">Cercospora &gt;&gt;</a><br />
                                <a id="northD2" onclick="ShowNursery('CurlyTop-North');" href="#">Curly Top &gt;&gt;</a><br />
                                <a id="northD3" onclick="ShowNursery('Fusarium-North');" href="#">Fusarium &gt;&gt;</a><br />
                                <a id="northD4" onclick="ShowNursery('Rhizoctonia-North');" href="#">Rhizoctonia &gt;&gt;</a><br />
                                <a id="northD5" onclick="ShowNursery('RootAphid-North');" href="#">Root Aphid &gt;&gt;</a><br />
                                &nbsp;
                            </div>
                            <div class="DisplayOff" id="divNurserySouth" style="background-color: #ffffcc; text-align: center" runat="server">
                                <br style="line-height: 8px" />
                                &nbsp; <span class="LabelText">South Nursery Data</span><br />
                                <a id="A2" onclick="ShowNursery('Aphanomyces-South');" href="#">Aphanomyces &gt;&gt;</a><br />
                                <a id="A1" onclick="ShowNursery('Cercospora-South');" href="#">Cercospora &gt;&gt;</a><br />
                                <a id="southD3" onclick="ShowNursery('CurlyTop-South');" href="#">Curly Top &gt;&gt;</a><br />
                                <a id="A3" onclick="ShowNursery('Fusarium-South');" href="#">Fusarium &gt;&gt;</a><br />
                                <a id="A4" onclick="ShowNursery('Rhizoctonia-South');" href="#">Rhizoctonia &gt;&gt;</a><br />
                                <a id="A5" onclick="ShowNursery('RootAphid-South');" href="#">Root Aphid &gt;&gt;</a><br />
                                &nbsp;
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ButtonText" colspan="2">
                            <br />
                            (In order of most to least importance, where 1 is most and 5 is least, select one
                            or more disease\pest, then choose a level of resistance for each. Press Search to 
                            see the varieties that match your criteria.)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="LabelText">
                            &nbsp;&nbsp;&nbsp;&nbsp;Disease \ Pest
                        </td>
                        <td class="LabelText">
                            Resistance Level
                        </td>
                    </tr>
                    <tr style="line-height: 22px;">
                        <td>
                            <span class="LabelText">1. </span>
                            <asp:DropDownList ID="ddlQuery1" runat="server" CssClass="VLgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRes1" runat="server" CssClass="LgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="line-height: 22px;">
                        <td>
                            <span class="LabelText">2. </span>
                            <asp:DropDownList ID="ddlQuery2" runat="server" CssClass="VLgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRes2" runat="server" CssClass="LgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="line-height: 22px;">
                        <td>
                            <span class="LabelText">3. </span>
                            <asp:DropDownList ID="ddlQuery3" runat="server" CssClass="VLgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRes3" runat="server" CssClass="LgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="line-height: 22px;">
                        <td>
                            <span class="LabelText">4. </span>
                            <asp:DropDownList ID="ddlQuery4" runat="server" CssClass="VLgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRes4" runat="server" CssClass="LgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="line-height: 22px;">
                        <td>
                            <span class="LabelText">5. </span>
                            <asp:DropDownList ID="ddlQuery5" runat="server" CssClass="VLgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRes5" runat="server" CssClass="LgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="line-height: 22px;">
                        <td>
                            <span class="LabelText">6. </span>
                            <asp:DropDownList ID="ddlQuery6" runat="server" CssClass="VLgText" AutoPostBack="False">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlRes6" runat="server" CssClass="LgText" AutoPostBack="False">
                            </asp:DropDownList>
                            &nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="BtnMed"
                                OnClick="btnSearch_Click"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td class="LabelText" colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="ButtonText" colspan="3">&nbsp;
                            <span class="LabelText">Search Results: </span><span>Varieties that meet
                                your criteria appear in </span><span style="background-color: #ffff33">yellow highlight</span>.
                            If no highlighted varieties appear try relaxing your selection criteria. You can
                            sort by clicking on a column heading.
                        </td>
                    </tr>
                    <tr><td colspan="3">&nbsp;</td></tr>
                </table>
                <table>
                    <tr>
                        <td colspan="3">
                            <div id="divResult" style="text-align: center;" runat="server" class="DisplayOff">
                                <table style="background-color:#990000; font-weight:bold; text-align:center; color:#ffffff; width:100%; border-bottom:1px solid #990000;">
                                    <tr>
                                        <td style="width:20%; padding: 0 1px 0 1px;">&nbsp;</td>
                                        <td style="width:9%; padding: 0 1px 0 1px;">&nbsp;</td>
                                        <td style="width:9%; padding: 0 1px 0 1px;">&nbsp;</td>
                                        <td style="width:10%; padding: 0 1px 0 1px;">&nbsp;</td>
                                        <td style="width:34%; padding: 0 1px 0 1px; border-left: 1px solid #ffffff; border-right: 1px solid #ffffff;" colspan="3">Recoverable</td>
                                        <td style="width:9%; padding: 0 1px 0 1px;">&nbsp;</td>
                                        <td style="width:9%; padding: 0 1px 0 1px; border-left: 1px solid #ffffff;">Years</td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdResults" runat="server" SkinID="grdColor" 
                                    EnableViewState="false" EmptyDataText="No seeds match your criteria."
                                    AutoGenerateColumns="False" AllowSorting="true"
                                    CssClass="grdColor920" Style="border-right: black 1px solid; border-bottom: black 1px solid;
                                    border-left: black 1px solid;" ShowHeader="true" 
                                    OnRowDataBound="grdResults_RowDataBound" onsorting="grdResults_Sorting" 
                                    onrowcreated="grdResults_RowCreated">
                                    <Columns>
                                        <asp:BoundField DataField="svdVariety" SortExpression="1" HeaderText="Variety" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="svdTonsPerAcre" DataFormatString="{0:N1}" SortExpression="2" HeaderText="Tons/Acre" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="svdSugarPct" DataFormatString="{0:N2}" SortExpression="3" HeaderText="Sugar %" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="svdSugarPerAcre" SortExpression="4" HeaderText="Sugar/Acre" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="svdRecovSugarPct" DataFormatString="{0:N2}" SortExpression="5" HeaderText="Sugar %" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="11%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="svdRecovSugarPerAcre" SortExpression="6" HeaderText="Sugar/Acre" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="14%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="svdRecovSugarPerTon" SortExpression="7" HeaderText="Sugar/Ton" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="svdSLM" DataFormatString="{0:N4}" SortExpression="8" HeaderText="SLM" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="svdYearsTreated" SortExpression="9" HeaderText="Tested" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="selected">
                                            <ItemStyle Width="0" CssClass="DisplayOff" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
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
            <div style="text-align: center">
                &nbsp;</div>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    </table>
    <div>
        <table cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 150px">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div class="DisplayOff">
        <asp:TextBox ID="txtNorthSouth" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtSortCol" runat="server"></asp:TextBox>
    </div>
</asp:Content>
