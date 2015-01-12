<%@ Page Title="Master Reports" Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="MasterReports.aspx.cs" Inherits="WSCIEMP.Reports.MReports.MasterReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Master Reports
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <br />
    <table cellspacing="0" cellpadding="0" border="0" width="98%">
        <tr style="line-height:16px;">
            <td style="width: 7%; background-color:#FFFFFF;">&nbsp;</td>
            <td style="width: 15%; background-color:#FFFFFF;">&nbsp;</td>
            <td style="width: 55%; background-color:#FFFFFF;border-right: 2px solid #B0B0B0;">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" style="width: 22%; background-color:#ffffff; border-left: #FFFFFF 4px solid; border-right: #FFFFFF 4px solid; border-bottom: 2px solid #B0B0B0;">
                <asp:Menu ID="mnuLocal" runat="server" SkinID="bodyMenu" 
                    CssClass="headerMenuLinkLocal"                     
                    StaticMenuItemStyle-CssClass="mnuTopStaticMenuItemStyleLocal" 
                    StaticHoverStyle-CssClass="mnuTopStaticHoverStyleLocal"
                    StaticSelectedStyle-CssClass="mnuTopStaticSelectedStyle" 
                    DataSourceID="smdUpRoot"                                                              
                    OnPreRender="mnuLocal_PreRender" />                
            </td>
            <td colspan="2" style="vertical-align:top; line-height:30px; border-right: 2px solid #B0B0B0; background-color: #F0F0F0;">
                <div id="divMenuDesc" runat="server" class="menuDesc">
                </div>
            </td>
        </tr>
        <tr style="line-height:10px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td style="background-color:#F0F0F0; border-bottom: 2px solid #B0B0B0;">&nbsp;</td>
            <td style="background-color:#F0F0F0; border-right: 2px solid #B0B0B0; border-bottom: 2px solid #B0B0B0;">&nbsp;</td>
        </tr>                   
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
    <br style="line-height:50px;"/>
    <asp:Image ID="imgFooter" runat="server" ImageUrl="~/img/DescFooter.gif" />
</asp:Content>
