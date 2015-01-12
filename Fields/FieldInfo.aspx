<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="FieldInfo.aspx.cs" Inherits="WSCIEMP.Fields.FieldInfo"
    Title="Western Sugar Cooperative - Field Description" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Field Description
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript" language="javascript">

        function IsShidOk(evt) {
            if ($$('lblBusName', $('#userContractSelectorTab')).text().length == 0) {               
                var e = GetEvent(evt);
                BubbleOff(e);
                return false;
            } else {
                return true;
            }
        }   
        
        function SelectContract(lldID) {

            CloseDialog('FieldFinder');
            $$('txtLldID', $('#FieldInfoData')).val(lldID);
            $$('btnSvrFindField', $('#actionBlock')).click();
        }

        function DoCancel() {
            $$('divFFWarning', $('#FieldFinderContainer')).text('');
            CloseDialog('FieldFinder');
        }   

        function CheckFFRequired() {
        
            // check for minimal criteria
            if ($$('txtFSANumber', $('#dataEntry')).val().length == 0 && $$('txtWscFieldName', $('#dataEntry')).val().length == 0 && 
                $$('ddlState', $('#dataEntry')).val().length == 0) {

                alert('You must enter either part of a FSA Number, part of a WSC Field Name or at least select a State before using the Find.');
                return false;
                
            } else {
                return true;
            }
        }
             
        function GetFieldFinderURL() {

            // Package parameters
            var params = 'fsaNumber=' + escape($$('txtFSANumber', $('#dataEntry')).val()) + '&' +
			'fieldName=' + $$('txtWscFieldName', $('#dataEntry')).val() + '&' +
			'cy=' + $$('ddlCropYear', $('#userContractSelectorTab')).val() + '&' +
			'acres=' + $$('txtAcres', $('#dataEntry')).val() + '&' +
			'state=' + $$('ddlState', $('#dataEntry')).val() + '&' +
			'county=' + escape($$('ddlCounty', $('#dataEntry')).val()) + '&' +
			'township=' + $$('ddlTownship', $('#dataEntry')).val() + '&' +
			'range=' + $$('ddlRange', $('#dataEntry')).val() + '&' +
			'section=' + $$('ddlSection', $('#dataEntry')).val() + '&' +
			'quadrant=' + $$('ddlQuadrant', $('#dataEntry')).val() + '&' +
			'quarterQuadrant=' + $$('ddlQuarterQuadrant', $('#dataEntry')).val() + '&' +
			'fsaState=' + $$('ddlFSAState', $('#dataEntry')).val() + '&' +
			'fsaCounty=' + escape($$('ddlFSACounty', $('#dataEntry')).val()) + '&' +
			'farmNo=' + $$('txtFarmNo', $('#dataEntry')).val() + '&' +
			'tractNo=' + $$('txtTractNo', $('#dataEntry')).val() + '&' +
			'fieldNo=' + $$('txtFieldNo', $('#dataEntry')).val() + '&' +
			'quarterField=' + $$('ddlQuarterField', $('#dataEntry')).val() + '&' +
			'latitude=' + $$('txtLatitude', $('#dataEntry')).val() + '&' +
			'longitude=' + $$('txtLongitude', $('#dataEntry')).val() + '&' +
			'description=' + escape($$('txtDescription', $('#dataEntry')).val());		

            return "FieldDescFinder.aspx?" + params;
        }              
                
        function CheckDelete(evt) {

            var e = GetEvent(evt);
            if (e != null) {

                //you cannot delete a field that has a cntlldid
                var cntlldID = $$('txtCntLldID', $('#FieldInfoData')).val();
                if (cntlldID.length > 0 && cntlldID > 0) {

                    alert('You cannot delete a Legal Land Description that is contracted.  You must first Remove this field from the Contract.');
                    BubbleOff(e);
                    return false;
                }

                if (!confirm('Are you sure, absolutely certain, you want to delete this Legal Land Description?  This will Delete the Legal Land Description from the system and cannot be undone.')) {
                    BubbleOff(e);
                    return false;
                }

            } else {
                alert('You must use Microsoft Internet Explorer to use this function.');
            }
        }

        function ShowDialog(id) {
            if (id == 'QFielddSaveOption') {
                PrepQFielddSaveOption();
            }
            if (IsShidOk()) {
                $('#' + id).dialog("open");
            }
        }

        function ShowFFDialog(id) {

            if (IsShidOk()) {
                if (CheckFFRequired()) {
                    RestoreFFState();
                    $('#' + id).dialog("open");
                }
            }
        }

        function CloseDialog(id) {
            $('#' + id).dialog("close");
        }

        function CloseAndSaveField(id) {
        
            $('#' + id).dialog("close");
            $$('txtSaveToPriorYears', $('#actionBlock')).val("");
            
            if ($('#radQSaveApplyAll').attr('checked') === true) {
                $$('txtSaveToPriorYears', $('#actionBlock')).val("1");
            } else {
                if ($('#radQSaveCreateNew').attr('checked') === true) {
                    $$('txtSaveToPriorYears', $('#actionBlock')).val("0");
                }
            }

            if ($$('txtSaveToPriorYears', $('#actionBlock')).val().length > 0) {
                $$('btnSvrSave', $('#actionBlock')).click();
            }
        }

        function RestoreFFState() {
            $('#FieldFinderContainer').remove();
            $('#FFProcessing').clone().appendTo('#FieldFinder');
        }

        function PrepQFielddSaveOption() {
            $$('radQSaveCancel', $('#QFielddSaveOption')).click();
        }

        $(document).ready(function() {

            //-----------------------------------------
            // Setup Query Save Option Dialog
            //-----------------------------------------
            $('#QFielddSaveOption').dialog({

                title: "Western Sugar Cooperative - Save Options", modal: true, autoOpen: false, height: 250, width: 600, draggable: true, resizable: true,
                open: function(type, data) {
                    $(this).parent().appendTo("form:first");
                }
            });

            //-----------------------------------------
            // Setup Field Finder Dialog
            //-----------------------------------------
            $('#FieldFinder').dialog({
            title: "Western Sugar Cooperative - Field Finder", modal: true, autoOpen: false, height: 680, width: 930, draggable: true, 
                cache: false, resizable: true,
                open: function(type, data) {
                    $('#FieldFinder').load(GetFieldFinderURL() + ' #FieldFinderContainer');
                }
            });
        });

    </script>

    <asp:ScriptManager ID="mainScriptManager" runat="server">
    </asp:ScriptManager>
    <div id="actionBlock" class="DisplayOff">
        <asp:Button ID="btnSvrFindField" runat="server" Text="Find" OnClick="btnSvrFindField_Click" />
        <asp:Button ID="btnSvrSave" runat="server" CssClass="BtnLg" Text="Save" OnClick="btnSvrSave_Click">
        </asp:Button>
        <asp:TextBox ID="txtSaveToPriorYears" runat="server"></asp:TextBox>
    </div>
    <table style="width: 99%; padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
        margin: 0px; vertical-align: bottom; padding-top: 0px" cellspacing="0" cellpadding="0"
        border="0" id="motherShip">
        <tr>
            <td width="1" class="ButtonText">
                &nbsp;
            </td>
            <td style="text-align: left">
                <!-- !! UsrCntSelector goes here !! -->
                <uc:UCSelector ID="UsrCntSelector" runat="server"></uc:UCSelector>
            </td>
        </tr>
        <tr>
            <td width="1">
                &nbsp;
            </td>
            <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                vertical-align: top; padding-top: 0px">
                <!-- Field Description -->
                <br style="line-height: 9px;" />
                <span class="LabelText">Field:</span>
                <div style="border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    margin-right: 10px; border-bottom: black 1px solid" id="dataEntry">
                    <div align="center">
                        <asp:Label ID="lblNewField" runat="server" CssClass="WarningOff"></asp:Label></div>
                    <table width="98%" border="0">
                        <tr>
                            <td width="24%" class="LabelText">
                                FSA Number:
                            </td>
                            <td width="53%">
                                <asp:TextBox ID="txtFSANumber" runat="server" CssClass="ButtonText" Columns="28"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkFSAOfficial" runat="server" CssClass="LabelText" Text="Official"
                                    TextAlign="Left"></asp:CheckBox>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnAddField" runat="server" CssClass="BtnLg" Text="Add to Cnt" ToolTip="Add the Field to the Contract"
                                    OnClientClick="IsShidOk()" OnClick="btnAddField_Click"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText" style="height: 28px">
                                WSC Field Name:
                            </td>
                            <td class="ButtonText" style="height: 28px">
                                <asp:TextBox ID="txtWscFieldName" runat="server" CssClass="ButtonText" ToolTip="Not editable, this is system generated."
                                    Columns="25"></asp:TextBox>&nbsp;
                            </td>
                            <td style="height: 28px" align="right">
                                <asp:Button ID="btnRemoveField" runat="server" CssClass="BtnLg" Text="Drop from Cnt"
                                    ToolTip="Remove the Field from the Contract" OnClick="btnRemoveField_Click" OnClientClick="IsShidOk()">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                Acres:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAcres" runat="server" CssClass="ButtonText"></asp:TextBox>&nbsp;
                            </td>
                            <td align="right">
                                <input class="BtnLg" id="btnFindField" onclick="ShowFFDialog('FieldFinder');" type="button" value="Find Field" name="btnFindField" />
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText" style="height: 20px">
                                State:
                            </td>
                            <td style="height: 20px">
                                <table width="100%" border="0">
                                    <tr>
                                        <td width="30%">
                                            <asp:DropDownList ID="ddlState" runat="server" CssClass="MedList" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="39%" align="center">
                                            <span class="LabelText">FSA State:</span>
                                        </td>
                                        <td width="*" align="left">
                                            <asp:DropDownList ID="ddlFSAState" runat="server" CssClass="MedList" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlFSAState_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="height: 20px" align="right">
                                <asp:Button ID="btnReset" runat="server" CssClass="BtnLg" Text="Clear Field" ToolTip="Clear Field information data entry areas."
                                    OnClick="btnReset_Click"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText" style="height: 25px">
                                County:
                            </td>
                            <td>
                                <table width="100%" border="0">
                                    <tr>
                                        <td width="30%">
                                            <asp:DropDownList ID="ddlCounty" runat="server" CssClass="MedList">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                        <td width="39%" align="center">
                                            <span class="LabelText">FSA County:</span>
                                        </td>
                                        <td width="*" align="left">
                                            <asp:DropDownList ID="ddlFSACounty" runat="server" CssClass="MedList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="height: 25px;" align="right">
                                <asp:Button ID="btnNew" runat="server" CssClass="BtnLg" Text="New Field" ToolTip="Add a new field description to the system."
                                    OnClick="btnNew_Click" OnClientClick="IsShidOk()"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                1/4 Quadrant:
                            </td>
                            <td>
                                <table width="100%" border="0">
                                    <tr>
                                        <td width="30%">
                                            <asp:DropDownList ID="ddlQuarterQuadrant" runat="server" CssClass="MedList">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                        <td width="39%" align="center">
                                            <span class="LabelText">Farm No.:</span>
                                        </td>
                                        <td width="*" align="left">
                                            <asp:TextBox ID="txtFarmNo" CssClass="MedList" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="text-align:right;">
                                <input class="BtnLg" id="btnFieldSave" onclick="ShowDialog('QFielddSaveOption');"
                                    type="button" value="Save Field" />
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText" style="height: 15px">
                                Quadrant:
                            </td>
                            <td style="height: 15px">
                                <table width="100%">
                                    <tr>
                                        <td width="30%">
                                            <asp:DropDownList ID="ddlQuadrant" runat="server" CssClass="MedList">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                        <td width="39%" align="center">
                                            <span class="LabelText">Tract No.: </span>
                                        </td>
                                        <td width="*" align="left">
                                            <asp:TextBox ID="txtTractNo" CssClass="MedList" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnDelete" runat="server" CssClass="BtnLg" Text="Delete Field" ToolTip="Delete this field from the system."
                                    OnClick="btnDelete_Click" OnClientClick="IsShidOk()"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                Section:
                            </td>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td width="30%">
                                            <asp:DropDownList ID="ddlSection" runat="server" CssClass="MedList">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                        <td width="39%" align="center">
                                            <span class="LabelText">Field No.:</span>
                                        </td>
                                        <td width="*" align="left">
                                            <asp:TextBox ID="txtFieldNo" CssClass="MedList" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText" style="height: 11px">
                                Township:
                            </td>
                            <td style="height: 11px">
                                <table width="100%">
                                    <tr>
                                        <td width="30%">
                                            <asp:DropDownList ID="ddlTownship" runat="server" CssClass="MedList">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                        <td width="39%" align="center">
                                            <span class="LabelText">1/4 Field:</span>
                                        </td>
                                        <td width="*" align="left">
                                            <asp:DropDownList ID="ddlQuarterField" runat="server" CssClass="MedList">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="height: 15px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                Range:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRange" runat="server" CssClass="MedList">
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                GPS Latitude:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLatitude" runat="server" CssClass="MedList"></asp:TextBox>&nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                GPS Longitude:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLongitude" runat="server" CssClass="MedList"></asp:TextBox>&nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText">
                                Description (max 100 characters):
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="ButtonText" Columns="50"
                                    TextMode="MultiLine" Rows="4"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="ButtonText" colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </td>
        </tr>
    </table>
    <div class="DisplayOff">
        <!-- ********************************************************
          Dialog to query for the field description save option 
         ******************************************************** -->
        <div id="QFielddSaveOption">
            <div>
                <table width="100%" border="0" id="popFieldSave">
                    <tr>
                        <td width="1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="1">
                            &nbsp;
                        </td>
                        <td style="text-align: left;" class="ButtonText">
                            <input id="radQSaveApplyAll" type="radio" value="All" name="QSaveApply" />&nbsp;Apply these edits to any contract, even in prior
                            years, using this land description.
                        </td>
                    </tr>
                    <tr>
                        <td width="1">
                            &nbsp;
                        </td>
                        <td style="text-align: left;" class="ButtonText">
                            <input id="radQSaveCreateNew" type="radio" value="New" name="QSaveApply" />&nbsp;Create a new land description, new field name,
                            really a new field in the system.
                        </td>
                    </tr>
                    <tr>
                        <td width="1">
                            &nbsp;
                        </td>
                        <td style="text-align: left;" class="ButtonText">
                            <input id="radQSaveCancel" type="radio" checked="checked" value="Cancel" name="QSaveApply" />Cancel, I have to think about this.
                        </td>
                    </tr>
                    <tr>
                        <td width="1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="1">
                            &nbsp;
                        </td>
                        <td align="center">
                            <input id="btnQSaveOk" type="button" value=" Ok " onclick="CloseAndSaveField('QFielddSaveOption');" class="LabelText" />&nbsp;
                            <input id="btnQSaveCancel" type="button" value=" Cancel " name="btnCancel" onclick="CloseDialog('QFielddSaveOption');" class="LabelText" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <!-- ********************************************************
          Dialog FieldFinder
         ******************************************************** -->
        <div id="FieldFinder">
        </div>
    </div>
    <div class="DisplayOff" id="FieldInfoData">
        <asp:TextBox ID="txtCntLldID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtLldID" runat="server"></asp:TextBox>        
        <asp:TextBox ID="txtOtherLldContracts" runat="server"></asp:TextBox>
        <div class="WarningOn" style="text-align:center;" id="FFProcessing">
            <br />
            <br />
            Processing Your Request...
        </div>        
    </div>
</asp:Content>
