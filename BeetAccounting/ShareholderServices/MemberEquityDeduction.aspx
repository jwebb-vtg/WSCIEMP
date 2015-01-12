<%@ Page Title="Western Sugar Cooperative - Member Equity Deduction" Language="C#"
    MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true"
    CodeBehind="MemberEquityDeduction.aspx.cs" Inherits="WSCIEMP.BeetAccounting.ShareholderServices.MemberEquityDeduction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    BeetAccounting - Member Equity Deduction
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript">

        function showDialog(id) {
            if (id == 'AddressFinder') {
                PrepAddressFinder();
            }        
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }

        function closeAndResolve(id) {
            $('#' + id).dialog("close");
            $$('btnResolveShid', $("#actionBlock")).click();
        }

        function CheckEnterKey(e) {

            var evt = GetEvent(e);
            if ((evt && evt.which && evt.which == 13) || (evt && evt.keyCode && evt.keyCode == 13)) {

                $$('btnResolveShid', $("#actionBlock")).click();
            }
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
        });

    </script>

    <div class="PagePartWide" style="padding-left: 10px; width: 97%;">
        <asp:ScriptManager ID="mainScriptManager" runat="server">
        </asp:ScriptManager>
        <br class="halfLine" />
        <table border="0" style="width: 98%;" id="selectionCriteria">
            <tr>
                <td style="width: 12%;">
                    <span class="LabelText">CropYear:</span><br />
                    <asp:DropDownList ID="ddlCropYear" CssClass="ctlWidth60" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlCropYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <table border="0" style="width: 100%;">
                        <tr>
                            <td style="width: 16%">
                                <span class="LabelText">Shareholder ID: </span>
                            </td>
                            <td style="width: 4%">
                                &nbsp;
                            </td>
                            <td>
                                <span class="LabelText">Business Name: </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="uplShid" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                    runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSHID" TabIndex="0" runat="server" CssClass="ctlWidth60"></asp:TextBox>&nbsp;&nbsp;
                                        <input id="btnFindShid" class="LabelText" type="button" onclick="showDialog('AddressFinder');"
                                            value="..." />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblBusName" CssClass="ButtonText" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br class="halfLine"/>
        <div class="DisplayOff" id="actionBlock">
            <asp:Button ID="btnResolveShid" runat="server" Text="..." OnClick="btnResolveShid_Click" />
        </div>
        <div id="divMemberEdit" style="width: 99%;">
            <br class="halfLine"/>
            <table border="0" style="width: 99%;" id="editLayout">
                <tr>
                    <td style="width: 90%;">
                        <span class="LabelText">Member Equity Deductions</span>
                        <div class="ButtonText" style="overflow: auto; height: 85px; border: solid 1px #FFFFFF; text-align:center;">
                            <asp:GridView ID="grdEqDeduction" runat="server" SkinID="grdColor" 
                                CellPadding="1" AutoGenerateColumns="False" CssClass="grdColor920"  
                                width="100%" onselectedindexchanged="grdEqDeduction_SelectedIndexChanged" 
                                onrowcreated="grdEqDeduction_RowCreated">
                                <Columns>
                                    <asp:BoundField DataField="MemberEquityDeductionID" HeaderText="MemberEquityDeductionID">
                                        <ItemStyle Width="0" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EquityDeductionID" HeaderText="EquityDeductionID">
                                        <ItemStyle Width="0" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="RowVersion" HeaderText="RowVersion">
                                        <ItemStyle Width="0" />
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="PaySequence" HeaderText="PaySequence">
                                        <ItemStyle Width="0" />
                                    </asp:BoundField>                                                                         
                                    <asp:BoundField DataField="EquityCropYear" HeaderText="Equity Year">
                                        <HeaderStyle HorizontalAlign="Center" Width="11%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EquityType" HeaderText="Equity Type">
                                        <HeaderStyle HorizontalAlign="Center" Width="19%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PayDesc" HeaderText="Payment Desc.">
                                        <HeaderStyle HorizontalAlign="Center" Width="19%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DeductionDesc" HeaderText="Deduction Desc.">
                                        <HeaderStyle HorizontalAlign="Center" Width="34%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="DeductionAmount" HeaderText="Amount">
                                        <HeaderStyle HorizontalAlign="Center" Width="17%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>                                                                          
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td style="text-align:center; vertical-align:top;">
                        <asp:Button ID="btnAddDeduction" runat="server" Text=" Add " onclick="btnAddDeduction_Click" /><br /><br class="halfLine" />
                        <asp:Button ID="btnUpdateDeduction" runat="server" Text=" Update " onclick="btnUpdateDeduction_Click" /><br /><br class="halfLine" />
                        <asp:Button ID="btnDeleteDeduction" runat="server" Text=" Delete" onclick="btnDeleteDeduction_Click" />
                    </td>
                </tr>
            </table>
            <br class="halfLine" />
            <table border="0" style="width: 100%;">
                <tr>
                    <td style="width:57%; text-align:left;"><span class="LabelText">Equity Deduction:</span></td>
                    <td style="width:20%; text-align:left;"><span class="LabelText">Deduction Amount:</span></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:DropDownList ID="ddlEquityDeductions" CssClass="ButtonText" Width="420" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeductionAmount" CssClass="ButtonText" runat="server"></asp:TextBox>                        
                    </td>
                    <td style="text-align:right;">&nbsp;</td>
                </tr>    

                <tr>
                    <td colspan="2"><span class="LabelText">Available Equity Payments:</span></td>
                    <td style="text-align:right;">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="ButtonText" style="overflow: auto; height: 140px; border: solid 1px #FFFFFF; text-align:center;">
                            <asp:GridView ID="grdPayment" runat="server" SkinID="grdColor" 
                                CellPadding="1" AutoGenerateColumns="False" CssClass="grdColor920"  
                                width="100%" onselectedindexchanged="grdPayment_SelectedIndexChanged" onrowcreated="grdPayment_RowCreated">
                                <Columns>            
                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence #">
                                        <ItemStyle Width="0" />
                                    </asp:BoundField>                                                    
                                    <asp:BoundField DataField="CropYear" HeaderText="Equity Year">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EquityType" HeaderText="Equity Type">
                                        <HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>                                   
                                    <asp:BoundField DataField="PaymentDesc" HeaderText="Payment Desc.">
                                        <HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PayAmount" HeaderText="Amount">
                                        <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>                                                                          
                                </Columns>
                            </asp:GridView>
                        </div>   
                    </td>
                    <td>&nbsp;</td>
                </tr>            
            </table>         
            <br />
        </div>
        <br />
    </div>
    <div id="hideModal" class="DisplayOff">
        <!--  ***********************************************************
                Address Finder Dialog
              ***********************************************************  -->
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
                                <span class="addrLabel">Type</span>
                                <div title="Type" style="text-align: left; border: solid 1px #000000;">
                                    <asp:RadioButton ID="radTypeBusname" Checked="false" GroupName="AdrType" runat="server"
                                        Text="Business Name" CssClass="radio" /><br />
                                    <asp:RadioButton ID="radTypeLastName" Checked="true" GroupName="AdrType" runat="server"
                                        Text="Last Name" CssClass="radio" /><br />
                                    <asp:RadioButton ID="radTypeSHID" Checked="false" GroupName="AdrType" runat="server"
                                        Text="SHID" CssClass="radio" />
                                </div>
                                <br />
                            </td>
                            <td style="width: 5px;">
                                &nbsp;
                            </td>
                            <td>
                                <div style="text-align: left;">
                                    <span class="addrLabel">Search String</span><br />
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
                                <span class="addrLabel">Results</span><br />
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
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
