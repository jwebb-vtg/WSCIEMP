<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="PACAdministration.aspx.cs" Inherits="WSCIEMP.Admin.PACAdministration" Title="PAC Administrator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    PAC Administrator
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript">

        function showDialog(id) {
            $('#<%=pacMessages.ClientID%>').html('');

            if (id == 'AddressFinder') {
                PrepAddressFinder();
            }
            if (id == 'PACIndividuals') {
                if ($('#<%=lblAddressType.ClientID%>').html().length == 0) {
                    alert("Please select a valid SHID");
                    return false;
                }
            }
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }

        function closeAndResolve(id) {
            $('#' + id).dialog("close");
        }

        function CheckEnterKey(e) {
            var evt = GetEvent(e);
            if ((evt && evt.which && evt.which == 13) || (evt && evt.keyCode && evt.keyCode == 13)) {
                $$('btnResolveShid', $("#actionBlock")).click();
            }
        }

        function downloadContracts() {
            window.location = '/WSCIEMP_LEE/PDF/2015 Shareholder Agreement.doc';
        }

        function parseTable() {
            try {
                var $inputs = $('#<%=indTable.ClientID%> :input');
                var data = '';
                var shid = '';

                $inputs.each(function () {
                    if ($(this).hasClass('shid')) {
                        shid = $(this).val();
                    }
                    if ($(this).hasClass('sort')) {
                        data += $(this).val() + "|";
                    }
                    if ($(this).hasClass('individualid')) {
                        data += $(this).val() + "|";
                    }
                    if ($(this).hasClass('percentage')) {
                        data += $(this).val() + "|";
                    }
                    if ($(this).hasClass('signedDate')) {
                        if ($(this).val().length > 0) {
                            data += "true|" + $(this).val() + "|" + shid + "~";
                        } else {
                            data += "false|" + shid + "~";
                        }
                    }
                });

                $('#<%=indTableField.ClientID%>').val(data);
            } catch (e) {
                alert(e);
            }
        }

        function DeleteIndRow(row) {
            var td = $(row).parent();
            var tr = td.parent();
            
            tr.css("background-color", "#FF3700");

            tr.fadeOut(400, function () {
                tr.remove();
            });
        }

        function LaunchDatePicker(element) {
            $(element).datepicker();
            $(element).datepicker('show');
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

        $(document).ready(function () {
            //-----------------------------------------
            // Setup Find Address dialog
            //-----------------------------------------
            $('#AddressFinder').dialog({
                title: "Address Finder", modal: true, autoOpen: false, height: 450, width: 650, draggable: true, resizable: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
            //-----------------------------------------
            // Setup Add Individual dialog
            //-----------------------------------------
            $('#PACIndividuals').dialog({
                title: "Add Individual", modal: true, autoOpen: false, height: 300, width: 650, draggable: true, resizable: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
        });

    </script>

    <div class="DisplayOff" id="actionBlock">
        <asp:Button ID="btnResolveShid" runat="server" Text="..." OnClick="btnResolveShid_Click" />
    </div>
    <div class="PagePartWide" style="padding-left: 10px; width: 97%;">
        <asp:ScriptManager ID="mainScriptManager" runat="server"></asp:ScriptManager>
        <br class="halfLine" />
        <table border="0" style="width: 98%;" id="selectionCriteria">
            <tr>
                <td>
                    <asp:UpdatePanel ID="uplShid" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                        <ContentTemplate>
                    <div id="pacMessages" style="text-align: center; font-weight: bold; color: Green; font-size: 1.2em;" runat="server"></div>
                    <table border="0" style="width: 100%;">
                        <tr>
                            <td>
                                <span class="LabelText">Crop Year: </span>
                            </td>
                            <td style="width: 16%">
                                <span class="LabelText">Shareholder ID: </span>
                            </td>
                            <td style="width: 4%">&nbsp;</td>
                            <td>
                                <span class="LabelText">Business Name: </span>
                            </td>
                            <td>
                                <span class="LabelText">Address Type: </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlCropYear" TabIndex="0" runat="server" CssClass="ctlWidth60" OnSelectedIndexChanged="ddlCropYear_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSHID" TabIndex="0" runat="server" CssClass="ctlWidth60"></asp:TextBox>&nbsp;&nbsp;
                                <input id="btnFindShid" class="LabelText" type="button" onclick="showDialog('AddressFinder');" value="..." />
                            </td>
                            <td>
                                <asp:Label ID="lblBusName" CssClass="ButtonText" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAddressType" CssClass="ButtonText" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel ID="UpdatePACDetails" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                        <ContentTemplate>
                            <div id="PACDetails" runat="server">
                                <table border="0">
                                    <tr>
                                        <td><span class="LabelText">PAC Contribution: </span></td>
                                        <td><asp:TextBox ID="pACContibution" CssClass="textEntry" Width="35" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><span class="LabelText">PAC Date: </span></td>
                                        <td><asp:TextBox ID="pACDate" CssClass="textEntry" Width="90" runat="server" onclick="LaunchDatePicker(this);"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Table ID="contractTable" runat="server" />
                                <br />
                                <asp:HiddenField ID="indTableField" runat="server" />
                                <asp:Table ID="indTable" runat="server" width="50%" />
                                <div id="PACDetailsActions" runat="server">
                                    <br />
                                        <div style="text-align: center;">
                                            <input type="button" id="showInd" class="LabelText" onclick="parseTable(); showDialog('PACIndividuals');" value="Add Singer" />&nbsp;
                                            <input type="button" id="downloadContracts" class="LabelText" runat="server" onclick="downloadContracts();" value="Download Contracts" style="display: none;" />
                                        </div>
                                    <br />
                                    <div style="text-align: right; padding-right: 30px;">
                                        <input type="checkbox" id="pushPAC" name="pushPAC" />Apply to all Contracts
                                        <input type="button" class="LabelText" onclick ="parseTable(); $('#<%=btnSave.ClientID%>').click()" value="Save" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnLabel" style="display: none;" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <div id="hideModal" class="DisplayOff">
        <!-- Address Finder Dialog -->
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
                    <asp:HiddenField ID="txtAddrType" value="" runat="server"></asp:HiddenField>
                    <br />
                    <div style="text-align: right; width: 600px;">
                        <asp:Button ID="btnAddrOk" OnClick="btnAddrOk_Click" CssClass="btnLabel" runat="server"
                            Text=" Ok " />&nbsp;&nbsp;&nbsp;
                        <input type="button" id="btnAddrCancel" class="btnLabel" value=" Cancel " onclick="closeDialog('AddressFinder')" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- PAC Individual List -->
        <div id='PACIndividuals'>
            <asp:UpdatePanel ID="uplIndividuals" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                <ContentTemplate>
                    <br />
                    <div id="indWarning" class="WarningOff" runat="server"></div>
                    <table style="width: 600px; border: solid 1px #000000" id="Table2">
                        <tr>
                            <td style="width: 45%; text-align: center; vertical-align: top;">
                                 <div style="text-align: left; width: 250px; margin: 0 auto;">
                                    <span class="addrLabel">Search String</span><br />
                                    <asp:TextBox CssClass="textEntry" ID="IndName" Width="150" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:Button ID="Button3" runat="server" Text="Find" CssClass="btnLabel" OnClick="btnIndFind_Click" />
                                </div>
                                <br />
                                <span class="addrLabel">Results</span><br />
                                <asp:ListBox ID="IndividualsListBox" CssClass="textEntry" Width="250" Height="100" runat="server" DataTextField="FullName"
                                    DataValueField="IndividualID" OnSelectedIndexChanged="Individual_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                            </td>
                            <td style="width: 45%; text-align: left;" valign="middle">
                                <span class="textEntry">New Individual:</span><br />
                                <asp:TextBox ID="newIndividualName" CssClass="textEntry" Width="250" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="text-align: center;">
                                    <asp:Button ID="Button1" OnClick="btnIndSel_Click" CssClass="btnLabel" runat="server" Text=" Select " />
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:Button ID="Button2" OnClick="btnIndAdd_Click" CssClass="btnLabel" runat="server" Text=" Add " />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style="text-align: right; width: 600px;">
                        <input type="button" id="Button5" class="btnLabel" value=" Cancel " onclick="closeDialog('PACIndividuals')" />
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