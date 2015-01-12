<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="RehaulViewer.aspx.cs" Inherits="WSCIEMP.SHS.Rehaul.RehaulViewer" Title="Rehaul Viewer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Shrink Report</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
<script language="javascript" type="text/javascript">
//<![CDATA[

    $(document).ready(function() {

        //-----------------------------------------
        // Setup calendar
        //-----------------------------------------
        $$('txtDate', $('#hdrStuff')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
            buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+1',
            changeMonth: true, changeYear: true, duration: 'fast',
            onClose: SetDateSubmit
        });
    });
	
	function SetDateSubmit(newDate) {
	    $$('txtAction', $('#footer')).val('datechange');
	    DoSubmit();
	}

//]]>						
    </script>
    <div style="font-size: 10px;">
        <!-- *********************************************
                Page Header
            *********************************************  -->
        <br />
        <table id="hdrStuff" style="width: 99%; background-color:White;" border="0">
            <tr>                
                <td style="width: 43%; vertical-align:top;">
                    &nbsp;
                </td>
                <td class="LabelText" style="width: 8%; vertical-align:top;">
                    Factory:
                </td>
                <td style="width: 12%; text-align: left; font-size: 10px; vertical-align: top;">
                    <asp:DropDownList ID="ddlFactory" runat="server" Width="90" CssClass="ButtonText" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlFactory_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="width:20%; text-align: right; font-size: 10px; vertical-align: top;">
                    <asp:ImageButton ID="btnPrevReportDate" runat="server" ImageUrl="~/img/PrevGreen.gif"
                        OnClick="btnPrevReportDate_Click" ToolTip="Previous Date" />
                    <asp:TextBox ID="txtDate" runat="server" class="ButtonText" 
                        Style="width: 70px;"></asp:TextBox>
                    <asp:ImageButton ID="btnNextReportDate" runat="server" ImageUrl="~/img/NextGreen.gif"
                        OnClick="btnNextReportDate_Click" ToolTip="Next Date" />
                </td>                                                             
                <td style="text-align:right; padding-right:5px; vertical-align:top;">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <!-- *********************************************
                Rehaul Data Entry
            *********************************************  -->
        <div>
            <div class="PagePartWide">
                <br class="halfLine" />
                <table border="0" style="width: 100%;">
                    <tr>
                        <td style="width:50%; vertical-align:top;">
                            <br />
                            <table border="0" style="width: 100%; vertical-align:top;">
                                <tr style="line-height: 24px;">
                                    <td rowspan="6" style="width:1%">&nbsp;</td>
                                    <td style="width: 25%;" class="LabelText">
                                        Re-haul Load Average Weight
                                    </td>
                                    <td style="width: 25%;" class="ButtonText">
                                        <asp:TextBox ID="txtRehaulAvgWt" runat="server" CssClass="ctlWidth60" ReadOnly="true" style="text-align:right; padding-right:3px;"></asp:TextBox>  
                                    </td>
                                </tr>
                                <tr style="line-height: 24px;">
                                    <td class="LabelText">
                                        Yard Load Average Weight
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtYardAvgWt" runat="server" CssClass="ctlWidth60" ReadOnly="true" style="text-align:right; padding-right:3px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="line-height: 24px;">
                                    <td class="LabelText">
                                        Tailings with Chips (tons)
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChipsDiscarded" runat="server" CssClass="ctlWidth60" ReadOnly="true" style="text-align:right; padding-right:3px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="line-height: 24px;">
                                    <td class="LabelText">
                                        Chips Percent Tailings
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChipsPctTailings" runat="server" CssClass="ctlWidth60" ReadOnly="true" style="text-align:right; padding-right:3px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="line-height: 24px;">
                                    <td class="LabelText">
                                        Beets Slid Loads
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBeetsSlidLoads" runat="server" CssClass="ctlWidth60" ReadOnly="true" style="text-align:right; padding-right:3px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="line-height: 24px;">
                                    <td class="LabelText">
                                        Total Loads Re-hauled
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalLoadsRehauled" runat="server" CssClass="ctlWidth60" ReadOnly="true" style="text-align:right; padding-right:3px; background-color:#FFFFCC"></asp:TextBox>
                                    </td>
                                </tr>                                
                            </table>
                        </td>
                        <td>
                            <asp:Repeater ID="rptrStations" runat="server">
                                <HeaderTemplate>
                                    <table border="0" style="width: 100%;">
                                        <tr>
                                            <td style="width: 45%" class="LabelText">
                                                Station Name
                                            </td>
                                            <td class="LabelText">
                                                Re-haul Loads
                                            </td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStationName" runat="server" CssClass="ButtonText" Text='<%# Eval("StationName") %>'></asp:Label>                                            
                                        </td>
                                        <td class="ButtonText">
                                            <asp:TextBox ID="txtRehaulLoads" CssClass="ctlWidth60" runat="server" ReadOnly="true" style="text-align:right; padding-right:3px;" Text='<%# Eval("RehaulLoads") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table></FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
        </div>
    </div>
    <br />
</asp:Content>
