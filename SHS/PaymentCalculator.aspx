<%@ Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024"
    AutoEventWireup="true" CodeBehind="PaymentCalculator.aspx.cs" Inherits="WSCIEMP.SHS.PaymentCalculator"
    Title="Western Sugar Cooperative - Payment Calculator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    Payment Calculator
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table cellspacing="0" cellpadding="0" style="width:99%;" border="0">
        <tr>
            <td style="width: 10px">
                &nbsp;
            </td>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0" class="layoutAvg">
                    <tr>
                        <th style="width:1%;"></th>
                        <th style="width:20%"></th>
                        <th></th>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <span class="LabelText">Crop Year: </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCropYear" runat="server" CssClass="ctlWidth60">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <span class="LabelText">Sugar Content: </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSugarContent" runat="server" CssClass="ctlWidth75"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <span class="LabelText">SLM: </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSLM" runat="server" CssClass="ctlWidth75"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <span class="LabelText">Net Return: </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNetReturn" runat="server" CssClass="ctlWidth75"></asp:TextBox>
                            &nbsp;&nbsp;<asp:Button ID="btnCalculate" runat="server" Text="Calculate" 
                                CssClass="BtnMed" onclick="btnCalculate_Click"></asp:Button>
                        </td>
                    </tr>
                    <tr>                        
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="0" width="85%" border="0">
                    <tr>
                        <td rowspan="2" style="width: 30%">
                            &nbsp;
                        </td>
                        <td rowspan="2" style="width: 24%; text-align: center; vertical-align: bottom;">
                            <span class="LabelText">Quality Contract</span>
                        </td>
                        <td colspan="2" style="text-align: center">
                            <span class="DisplayOff">Old Method</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 23%; text-align: center">
                            <span class="DisplayOff">North</span>
                        </td>
                        <td style="text-align: center">
                            <span class="DisplayOff">South</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="LabelText">Beet Payment Per Ton: </span>
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lblQCPayment" runat="server" CssClass="ButtonText"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lblOldNorthPayment" runat="server" CssClass="DisplayOff"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lblOldSouthPayment" runat="server" CssClass="DisplayOff"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 10px">
                &nbsp;
            </td>
        </tr>
    </table>
    <div>
        <table cellspacing="0" cellpadding="0">
            <tr>
                <td style="line-height:150px;">&nbsp;</td>
            </tr>
        </table>
    </div>
</asp:Content>
