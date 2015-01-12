<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="PaymentDescription.aspx.cs" 
Inherits="WSCIEMP.BeetAccounting.PaymentProcessing.PaymentDescription" Title="BeetAccounting Payment Description" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    BeetAccounting - Payment Description
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <table cellspacing="0" cellpadding="0" border="0" style="width:100%">
        <tr>
            <td style="width: 2%; height: 16px">
                &nbsp;
            </td>
            <td style="width: 96%; height: 16px">
                <span class="LabelText">Crop Year: </span>
                <asp:DropDownList ID="ddlCropYear" runat="server" AutoPostBack="True" CssClass="ctlWidth60"
                    onselectedindexchanged="ddlCropYear_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="height: 16px">
                &nbsp;
            </td>
        </tr>
        <tr><td colspan="3">&nbsp;</td></tr>
        <tr>
            <td style="width: 2%">
                &nbsp;
            </td>
            <td style="width: 96%">
                <div>
                    <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%;vertical-align:bottom; text-align:center;" class="LabelText" style="text-align: center">
                                Payment<br />
                                Number
                            </td>
                            <td style="width:13%;vertical-align:bottom; text-align:center;" class="LabelText" style="text-align: center">
                                Required
                            </td>
                            <td style="width:13%;vertical-align:bottom; text-align:center;" class="LabelText" style="text-align: center">
                                Finished
                            </td>
                            <td style="width:30%;vertical-align:bottom; text-align:center;" class="LabelText" style="text-align: center">
                                Transmittal<br />
                                Date
                            </td>
                            <td style="width:30%;vertical-align:bottom; text-align:center;" class="LabelText" style="text-align: center">
                                Payment<br />
                                Desc
                            </td>
                        </tr>
                    </table>            
                </div>
            <!-- Payment: #1 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum1" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList ID="ddlRequired1" runat="server" CssClass="SmList">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList ID="ddlFinished1" runat="server" CssClass="SmList">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox ID="txtTransmittalDate1" runat="server" CssClass="MedList"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox ID="txtPaymentDesc1" runat="server" CssClass="MedList"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <!-- Payment: #2 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum2" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired2" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished2" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate2" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <!-- Payment: #3 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum3" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired3" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished3" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate3" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc3" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <!-- Payment: #4 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum4" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired4" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished4" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate4" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <!-- Payment: #5 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum5" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired5" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished5" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate5" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc5" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <!-- Payment: #6 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum6" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired6" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished6" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate6" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc6" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <!-- Payment: #7 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum7" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired7" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished7" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate7" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc7" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />   
                <!-- Payment: #8 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum8" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired8" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished8" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate8" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc8" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <!-- Payment: #9 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum9" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired9" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished9" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate9" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc9" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />  
                <!-- Payment: #10 -->
                <div style="height:30px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                    border-bottom: black 1px solid">
                    <table style="width: 100%;height:100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td style="width:14%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:Label ID="lblPayNum10" runat="server"></asp:Label>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlRequired10" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:13%; text-align: center;vertical-align:middle;" class="ButtonText">
                                <asp:DropDownList CssClass="SmList" ID="ddlFinished10" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtTransmittalDate10" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:30%; text-align: center;">
                                <asp:TextBox CssClass="MedList" ID="txtPaymentDesc10" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />                                                           
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: right">
                            <asp:Button CssClass="BtnMed" ID="btnSave" runat="server" Text="Save" 
                                onclick="btnSave_Click"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
    <div class="DisplayOff" id="PostData">
        <asp:TextBox ID="txtPaymentDescID1" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID2" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID3" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID4" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID5" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID6" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID7" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID8" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID9" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPaymentDescID10" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtAction" runat="server"></asp:TextBox>
    </div>
</asp:Content>
