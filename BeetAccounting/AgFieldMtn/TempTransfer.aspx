<%@ Page Title="BeetAccounting Temp Transfer" Language="C#" MasterPageFile="~/PrimaryTemplate.Master"
    StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="TempTransfer.aspx.cs"
    Inherits="WSCIEMP.BeetAccounting.AgFieldMtn.TempTransfer" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    BeetAccounting - Temp Transfer
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript">

        //========================================
        // Address dialog functions
        //========================================
        function showDialog(id, ctl) {
        
            switch (id) {
                case 'AddressFinder':
                    PrepAddressFinder();
                    break;
            }

            var s = ctl.id;

            $$('txtActiveControl', $("#actionBlock")).val(s);
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }

        function closeAndResolve(id, activeControlName) {

            $('#' + id).dialog("close");

            if (activeControlName == "btnFindFromShid") {
                $$('btnResolveFromShid', $("#actionBlock")).click();
            } else {
                $$('btnResolveToShid', $("#actionBlock")).click();
            }
        }

        function CheckFromEnterKey(e) {

            var evt = GetEvent(e);
            if ((evt && evt.which && evt.which == 13) || (evt && evt.keyCode && evt.keyCode == 13)) {
                $$('btnResolveFromShid', $('#actionBlock')).click();
            }
        }
        function CheckToEnterKey(e) {

            var evt = GetEvent(e);
            if ((evt && evt.which && evt.which == 13) || (evt && evt.keyCode && evt.keyCode == 13)) {
                $$('btnResolveToShid', $("#actionBlock")).click();
            }
        }
        
        //========================================
        // Crop Year selection dialog functions
        //========================================
        function showCYDialog(id, ctl) {

            PrepPrintCropYears();
            
            var cy = $$('ddlCropYear', $('#current')).val() * 1;
            $$('txtCYFirstCropYear', $('#cyCriteria')).val(cy);

            var ddlcy = $$('ddlCYLastCropYear', $("#cyCriteria"));
            ddlcy.children().remove();

            var i = 0;
            for (i = cy; i <= cy + 3; i++) {
                if (i == cy) {
                    ddlcy.append('<option value=' + i + " selected='selected'" + '>' + i + '</option>');
                } else {
                    ddlcy.append('<option value=' + i + '>' + i + '</option>');
                }
            }
            // Set selected option.
            //$("#" + ddlcy.attr('id') + " option[0]").attr('selected', 'selected');
            // selected="selected"                       

            $('#' + id).dialog("open");
        }

        function showCustomDialog(id, ctl) {

            PrepPrintCustom();           

            var cy = $$('ddlCropYear', $('#current')).val() * 1;

            var ddlFirst = $$('ddlCustomFirstCropYear', $("#customCriteria"));
            var ddlLast = $$('ddlCustomLastCropYear', $("#customCriteria"));
            ddlFirst.children().remove();
            ddlLast.children().remove();

            var i = 0;
            for (i = cy; i <= cy + 9; i++) {
                ddlFirst.append('<option value=' + i + '>' + i + '</option>');
                ddlLast.append('<option value=' + i + '>' + i + '</option>');
            }
            // Set selected option.
            $("#" + ddlFirst.attr('id') + " option[0]").attr('selected', 'selected');
            $("#" + ddlLast.attr('id') + " option[0]").attr('selected', 'selected');
            $('#' + id).dialog("open");
        }
        
        function copyCYSelection() {
            var cy = $$('ddlCYLastCropYear', $("#cyCriteria")).val();
            $$('txtLastCropYear', $('#actionBlock')).val(cy);
        }

        function copyCustomSelection() {
            var cy = $$('ddlCustomFirstCropYear', $("#customCriteria")).val();
            $$('txtCustomFirstCropYear', $('#actionBlock')).val(cy);

            var cy = $$('ddlCustomLastCropYear', $("#customCriteria")).val();
            $$('txtCustomLastCropYear', $('#actionBlock')).val(cy);            
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

        function PrepPrintCropYears() {
            // clear warning
            //$$('CYWarning', $('#PrintCropYears')).text('');
        }

        function PrepPrintCustom() {
            
            // clear warning
            //$$('CustomWarning', $('#PrintCustom')).text('');
            $$('txtCustomTransfereeSHID', $('#PrintCustom')).val('');
            $$('txtCustomTransferorSHID', $('#PrintCustom')).val('');
            $$('txtCustomToPctRetain', $('#PrintCustom')).val('');
            $$('txtCustomShares', $('#PrintCustom')).val('');
        }

        $(document).ready(function() {

            //-----------------------------------------
            // Setup Find Address dialog
            //-----------------------------------------
            $('#AddressFinder').dialog({
                title: "Address Finder", modal: true, autoOpen: false, height: 450, width: 650, draggable: true, resizable: true,
                open: function(type, data) {
                    $(this).parent().appendTo("form");
                }
            });

            //-----------------------------------------
            // Setup Crop Year dialog
            //-----------------------------------------
            $('#PrintCropYears').dialog({
                title: "Crop Year Selection", modal: true, autoOpen: false, height: 215, width: 500, draggable: true, resizable: true,
                open: function(type, data) {
                    $(this).parent().appendTo("form");
                }
            });

            //-----------------------------------------
            // Setup Print Custom dialog
            //-----------------------------------------
            $('#PrintCustom').dialog({
                title: "Print Custom Settings", modal: true, autoOpen: false, height: 300, width: 700, draggable: true, resizable: true,
                open: function(type, data) {
                    $(this).parent().appendTo("form");
                }
            });

            //-----------------------------------------
            // Setup calendar
            //-----------------------------------------
            $$('txtTransferApprovalDate', $('#TransferEdit')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
                buttonImage: '../../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: false, yearRange: '-10:+1',
                changeMonth: true, changeYear: true, duration: 'fast'
            });

            //-----------------------------------------
            // Handle PDF ready
            //-----------------------------------------
            var locPDF = $$('locPDF', $("#actionBlock"));
            if (locPDF != null && locPDF.val().length > 0) {

                // Get the pdf location
                var url = locPDF.val();

                // NOW CLEAR THE PDF LOCATION !!
                $$('locPDF', $("#actionBlock")).val('');

                // okay, continue...
                var popWidth = screen.width - 150;
                var popHeight = screen.height - 150;
                var win = window.open(url, "_blank",
		            "status=0,toolbar=1,location=0,top=100,left=100,menubar=1,directories=0,resizable=1,scrollbars=1,width=" + popWidth + ",height=" + popHeight);
            }           
        });

    </script>

    <asp:ScriptManager ID="mainScriptManager" runat="server">
    </asp:ScriptManager>
    <div class="DisplayOff" id="actionBlock">
        <asp:Button ID="btnResolveFromShid" runat="server" Text="..." OnClick="btnResolveFromShid_Click" />
        <asp:Button ID="btnResolveToShid" runat="server" Text="..." OnClick="btnResolveToShid_Click" />
        <asp:TextBox ID="txtActiveControl" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtLastCropYear" runat="server"></asp:TextBox>          
        <asp:TextBox ID="txtCustomFirstCropYear" runat="server"></asp:TextBox>          
        <asp:TextBox ID="txtCustomLastCropYear" runat="server"></asp:TextBox>          
        <asp:TextBox ID="locPDF" runat="server" CssClass="DisplayOff"></asp:TextBox>
    </div>
    <div id="current">
        <span class="LabelText">CropYear:</span>&nbsp;&nbsp;
        <asp:DropDownList ID="ddlCropYear" CssClass="ctlWidth60" runat="server" AutoPostBack="True"
            OnSelectedIndexChanged="ddlCropYear_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <br />
    <table style="width: 99%" border="0">
        <tr>
            <td style="width: 49%; vertical-align:top;">
                <span class="LabelText" style="background-color:#FFFF99"> Transfer To (Transferee) </span>
                <div id="divTransferee" style="border: solid 1px black; width: 100%; padding-left: 2px;">
                    <br class="halfLine" />
                    <table border="0" style="width: 100%;" id="selectTransferee">
                        <tr>
                            <td style="width: 25%">
                                <span class="LabelText">Shareholder ID: </span>
                            </td>
                            <td>
                                <span class="LabelText">Business Name: </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="uplToShid" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                    runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtToSHID" TabIndex="0" runat="server" CssClass="ctlWidth60"></asp:TextBox>&nbsp;
                                        <input id="btnFindToSHID" class="LabelText" type="button" onclick="showDialog('AddressFinder', this);"
                                            value="..." />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblToBusName" CssClass="ButtonText" runat="server">&nbsp;</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <span class="LabelText">To Factory: </span>&nbsp;&nbsp;
                                <asp:Label ID="lblToFactory" CssClass="ButtonText" runat="server">&nbsp;</asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br class="halfLine" />
                    <span class="LabelText">Share Summary</span><br />
                    <div id="divToSummary" class="DisplayOn" style="width: 100%;">
                        <asp:GridView ID="grdToSummary" SkinID="grdColorWhite" runat="server" AutoGenerateColumns="False"
                            CssClass="grdColor920" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="PatronShares" HeaderText="Patron Shares">
                                    <ItemStyle Width="24%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PatronOverPlant" HeaderText="Over Plant">
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TransfereeShares" HeaderText="Transfer In">
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TransfereeOverPlant" HeaderText="Over Plant">
                                    <ItemStyle Width="26%" HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:GridView ID="grdToSummary2" SkinID="grdColorWhite" runat="server" AutoGenerateColumns="False"
                            CssClass="grdColor920" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="TransferorShares" HeaderText="Transfer Out">
                                    <ItemStyle Width="24%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DeliveryShareRights" HeaderText="Delivery Rights">
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SharesUsed" HeaderText="Shares Used">
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SharesUnassigned" HeaderText="Shares Available">
                                    <ItemStyle Width="26%" HorizontalAlign="Center" Font-Bold="true" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br class="halfLine" />
                </div>
                <br class="halfLine" />
            </td>
            <td style="width:1%;">&nbsp;</td>
            <td style="width: 49%; vertical-align:top;">
                <span class="LabelText">Transfer From (Transferor)</span>
                <div id="divTransferor" style="border: solid 1px black; width: 100%; padding-left: 2px;">
                    <br class="halfLine" />
                    <table border="0" style="width: 100%;" id="selectTransferor">
                        <tr>
                            <td style="width: 25%">
                                <span class="LabelText">Shareholder ID: </span>
                            </td>
                            <td>
                                <span class="LabelText">Business Name: </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="uplFromShid" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                    runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtFromSHID" TabIndex="0" runat="server" CssClass="ctlWidth60"></asp:TextBox>&nbsp;
                                        <input id="btnFindFromShid" class="LabelText" type="button" onclick="showDialog('AddressFinder', this);"
                                            value="..." />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblFromBusName" CssClass="ButtonText" runat="server">&nbsp;</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <span class="LabelText">From Factory: </span>&nbsp;&nbsp;
                                <asp:Label ID="lblFromFactory" CssClass="ButtonText" runat="server">&nbsp;</asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br class="halfLine" />
                    <span class="LabelText">Share Summary</span><br />
                    <div id="divFromSummary" class="DisplayOn" style="width: 100%;">
                        <asp:GridView ID="grdFromSummary" SkinID="grdColorWhite" runat="server" AutoGenerateColumns="False"
                            CssClass="grdColor920" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="PatronShares" HeaderText="Patron Shares">
                                    <ItemStyle Width="24%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PatronOverPlant" HeaderText="Over Plant">
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TransfereeShares" HeaderText="Transfer In">
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TransfereeOverPlant" HeaderText="Over Plant">
                                    <ItemStyle Width="26%" HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>                                
                        </asp:GridView>                                
                        <asp:GridView ID="grdFromSummary2" SkinID="grdColorWhite" runat="server" AutoGenerateColumns="False"
                            CssClass="grdColor920" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="TransferorShares" HeaderText="Transfer Out">
                                    <ItemStyle Width="24%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DeliveryShareRights" HeaderText="Delivery Rights">
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SharesUsed" HeaderText="Shares Used">
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SharesUnassigned" HeaderText="Shares Available">
                                    <ItemStyle Width="26%" HorizontalAlign="Center" Font-Bold="true" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br class="halfLine" />
                </div>
                <br class="halfLine" />
            </td>
        </tr>
    </table>
    <br class="halfLine" />
    <span class="LabelText" style="background-color:#FFFF99"> Transferee -- Transfer Edit </span>
    <div id="div1" style="border: solid 1px black; width: 99%; padding-left: 2px;">
        <table style="width:100%; text-align:center;" border="0" id="TransferEdit">
            <tr>
                <td colspan="6" style="text-align:right;">
                    <asp:Button ID="btnTransferAdd" runat="server" Text=" Add " CssClass="LabelText"
                        onclick="btnTransferAdd_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnTransferUpdate" runat="server" Text=" Update " CssClass="LabelText"
                        onclick="btnTransferUpdate_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnTransferDelete" runat="server" Text=" Delete " CssClass="LabelText"
                        onclick="btnTransferDelete_Click" />&nbsp;&nbsp;
                    <input id="btnTransferPrint" class="LabelText" type="button" onclick="showCYDialog('PrintCropYears', this);" value=" Print " />
                    &nbsp;&nbsp;
                    <input id="btnTransferPrintCustom" class="LabelText" type="button" onclick="showCustomDialog('PrintCustom', this);" value=" Print Custom " />
                    &nbsp;&nbsp;
                </td>
            </tr>         
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr>                
                <td style="width: 17%"><span class="LabelText">Shares</span></td>
                <td style="width: 17%"><span class="LabelText">Admin Fee Paid</span></td>
                <td style="width: 17%"><span class="LabelText">Price Per Acre</span></td>
                <td style="width: 16%"><span class="LabelText">% Retain</span></td>
                <td style="width: 16%"><span class="LabelText">% Of Crop</span></td>
                <td style="width: 17%"><span class="LabelText">Approval Date</span></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtTransferShares" CssClass="ButtonText" Width="75" runat="server"></asp:TextBox></td>
                <td>
                    <asp:CheckBox ID="chkTransferAdminFee" runat="server" /></td>
                <td><asp:TextBox ID="txtTransferPricePerAcre" CssClass="ButtonText" Width="75" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="txtTransferToPctRetain" CssClass="ButtonText" Width="75" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="txtTransferToPctCrop" CssClass="ButtonText" Width="75" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="txtTransferApprovalDate" CssClass="ButtonText" Width="75" runat="server"></asp:TextBox></td>
            </tr>                       
        </table>            
        <br class="halfLine" />
    </div>
    <br class="halfLine" />
    <span class="LabelText" style="background-color:#FFFF99"> Transferee -- Temp Transfers </span>
    <div id="divTransfers" class="DisplayOn" style="width: 100%;">
        <asp:GridView ID="grdTransfers" SkinID="grdColor" runat="server" AutoGenerateColumns="False"
            CssClass="grdColor920" Width="950" OnRowCreated="grdTransfers_RowCreated" 
            onselectedindexchanged="grdTransfers_SelectedIndexChanged" 
            onrowdatabound="grdTransfers_RowDataBound">
            <Columns>
                <asp:BoundField DataField="ShareTransferID" HeaderText="ShareTransferID">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="ToMemberID" HeaderText="ToMemberID">
                    <ItemStyle Width="0" />
                </asp:BoundField>
                <asp:BoundField DataField="FromMemberID" HeaderText="FromMemberID">
                    <ItemStyle Width="0" />
                </asp:BoundField>                                
                <asp:BoundField DataField="TransferNumber" HeaderText="Transfer #">
                    <ItemStyle Width="12%" HorizontalAlign="Center" />
                </asp:BoundField>                
                <asp:BoundField DataField="ContractNumber" HeaderText="Contract">
                    <ItemStyle Width="12%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ToShid" HeaderText="To SHID">
                    <ItemStyle Width="11%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ToFactoryName" HeaderText="To Factory">
                    <ItemStyle Width="12%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ToRetainPct" HeaderText="To Retain %">
                    <ItemStyle Width="12%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Shares" HeaderText="Shares">
                    <ItemStyle Width="12%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FromShid" HeaderText="From SHID">
                    <ItemStyle Width="11%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FromFactoryName" HeaderText="From Factory">
                    <ItemStyle Width="12%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="HasLienOnShares" HeaderText="Lien?">
                    <ItemStyle Width="6%" HorizontalAlign="Center" />
                </asp:BoundField>                            
                <asp:BoundField DataField="ToCropPct" HeaderText="To Crop Pct">
                    <ItemStyle Width="0" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PricePerAcre" HeaderText="Price\Acre">
                    <ItemStyle Width="0" HorizontalAlign="Center" />
                </asp:BoundField>                
                <asp:BoundField DataField="IsFeePaid" HeaderText="Is Fee Paid">
                    <ItemStyle Width="0" />
                </asp:BoundField>                
                <asp:BoundField DataField="FromRetainPct" HeaderText="From Retain %">
                    <ItemStyle Width="0" />
                </asp:BoundField>                                                                                    
                <asp:BoundField DataField="HasConsentForm" HeaderText="Has Consent Form">
                    <ItemStyle Width="0" />
                </asp:BoundField>  
                <asp:BoundField DataField="ApprovalDate" HeaderText="Approval Date">
                    <ItemStyle Width="0" />
                </asp:BoundField>                                  
                <asp:BoundField DataField="TransferTimeStamp" HeaderText="TransferTimeStamp">
                    <ItemStyle Width="0" />
                </asp:BoundField>                    
            </Columns>
        </asp:GridView>
    </div>
    <br />    
    <!--
        =================================================================        
            JQuery UI Popup forms.
        =================================================================        
    -->    
    <div id="hideModal" class="DisplayOff">
        <!-- ***  Transfer PRINT selected record  *** -->
        <div id="PrintCropYears">
            <br />    
            <table style="width: 98%; border: solid 1px #000000;" id="cyCriteria">
                <tr>
                    <td colspan="5">
                        <span class="btnLabel">
                        Please select the First Crop Year and  Last Crop Year</span> <br />
                        <span class="btnLabel">for printing Temp Transfer document(s)
                        </span>
                        <br />
                    </td>
                </tr>
                <tr><td colspan="5">&nbsp;</td></tr>
                <tr>
                    <td style="width: 15%;" >&nbsp;</td>
                    <td style="width: 30%; text-align:center;">
                        <span class="btnLabel">First Crop Year</span><br />
                        <asp:TextBox CssClass="textEntry" ReadOnly="true" ID="txtCYFirstCropYear" EnableViewState="false" Width="70" runat="server"></asp:TextBox></td>
                    <td style="width: 10%; text-align:center;">&nbsp;</td>
                    <td style="width: 30%; text-align:center;">
                        <span class="btnLabel">Last Crop Year</span><br />
                        <asp:DropDownList ID="ddlCYLastCropYear" CssClass="textEntry" runat="server" Width="75px" EnableViewState="True">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 15%;" >&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                    <td colspan="2" style="text-align:right;">
                        <asp:Button ID="btnCYOk" CssClass="btnLabel" runat="server" Text=" Ok " OnClientClick="copyCYSelection();closeDialog('PrintCropYears');" onclick="btnCYOk_Click" />&nbsp;&nbsp;&nbsp;
                        <input type="button" id="btnCYCancel" class="btnLabel" value=" Cancel " onclick="closeDialog('PrintCropYears');" />
                    </td>
                </tr>
            </table>                           
        </div>
        <!-- ***  Transfer PRINT custom -- not related to a real existing record  *** -->
        <div id="PrintCustom">
            <br />             
            <table style="width: 98%; border: solid 1px #000000;" id="customCriteria">
                <tr>
                    <td colspan="5">
                        <span class="btnLabel">
                        This will print Temp Transfer agreements between the SHIDs you enter for the range</span> <br />
						<span class="btnLabel">of Crop Years you select, using whatever additional information you enter here.</span>
                        <br />
                    </td>
                </tr>
                <tr><td colspan="5">&nbsp;</td></tr>
                <tr>
                    <td style="width: 15%;" >&nbsp;</td>
                    <td style="width: 30%; text-align:center;">
                        <span class="btnLabel">First Crop Year</span><br />
                        <asp:DropDownList ID="ddlCustomFirstCropYear" CssClass="textEntry" runat="server" Width="75px" EnableViewState="True">
                        </asp:DropDownList>
					</td>
                    <td style="width: 10%; text-align:center;">&nbsp;</td>
                    <td style="width: 30%; text-align:center;">
                        <span class="btnLabel">Last Crop Year</span><br />
                        <asp:DropDownList ID="ddlCustomLastCropYear" CssClass="textEntry" runat="server" Width="75px" EnableViewState="True">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 15%;" >&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="text-align:center;"><span class="btnLabel">Transferee SHID</span><br />
                        <asp:TextBox CssClass="textEntry" ID="txtCustomTransfereeSHID" Width="70" runat="server"></asp:TextBox></td>                            
                    <td style="text-align:center;">&nbsp;</td>
                    <td style="text-align:center;"><span class="btnLabel">Transferor SHID</span><br />
                        <asp:TextBox CssClass="textEntry" ID="txtCustomTransferorSHID" Width="70" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="text-align:center;"><span class="btnLabel">Transferee Retain %</span><br />
                        <asp:TextBox ID="txtCustomToPctRetain" CssClass="textEntry" Width="75" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td style="text-align:center;"><span class="btnLabel">Shares</span><br />
                        <asp:TextBox ID="txtCustomShares" CssClass="textEntry" Width="75" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5" style="text-align:right;">
                        <asp:Button ID="btnCustomOk" CssClass="btnLabel" runat="server" Text=" Ok " OnClientClick="closeDialog('PrintCustom');" onclick="btnCustomOk_Click" />&nbsp;&nbsp;&nbsp;
                        <input type="button" id="btnCustomCancel" class="btnLabel" value=" Cancel " onclick="closeDialog('PrintCustom');" />
                    </td>                        
                </tr>
            </table>                                                 
        </div>        
        <!-- ***  Dialog to find address: AddressFinder  *** -->
        <div id='AddressFinder'>
            <asp:UpdatePanel ID="uplAddressFinder" UpdateMode="Conditional" ChildrenAsTriggers="true"
                runat="server">
                <ContentTemplate>
                    <br />
                    <div id="addrWarning" class="WarningOff" runat="server">
                    </div>
                    <table style="width: 600px; border: solid 1px #000000;" id="addrCriteria">
                        <tr>
                            <td>
                                <span class="btnLabel">Type</span>
                                <div title="Type" style="text-align: left; border: solid 1px #000000;">
                                    <asp:RadioButton ID="radTypeBusname" Checked="false" GroupName="AdrType" runat="server"
                                        Text="Business Name" CssClass="textEntry" /><br />
                                    <asp:RadioButton ID="radTypeLastName" Checked="true" GroupName="AdrType" runat="server"
                                        Text="Last Name" CssClass="textEntry" /><br />
                                    <asp:RadioButton ID="radTypeSHID" Checked="false" GroupName="AdrType" runat="server"
                                        Text="SHID" CssClass="textEntry" />
                                </div>
                                <br />
                            </td>
                            <td style="width: 5px;">
                                &nbsp;
                            </td>
                            <td>
                                <div style="text-align: left;">
                                    <span class="btnLabel">Search String</span><br />
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
                                <span class="btnLabel">Results</span><br />
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
