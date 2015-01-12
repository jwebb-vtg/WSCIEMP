<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="GlobalException.aspx.cs" 
Inherits="WSCIEMP.GlobalException" Title="Western Sugar Cooperative - Error Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
&nbsp;
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
	    <tr>
		    <td>
		        <div style="text-align:center;"><span class="WarningLarge">Western Sugar Cooperative website encountered an 
					    unexpected problem.</span></div>
			    <div>
				    <br />
				    <p><span class="ErrorHeading">What Happened:</span></p>
				    <span class="ButtonText">There was an unexpected error that may be due to your session
					    expiring, or a busy server, or a programming bug.</span>
				    <p><span class="ErrorHeading">How does this affect you:</span></p>
				    <span class="ButtonText">The current page cannot load.</span>
				    <p><span class="ErrorHeading">What you can do:</span></p>
				    <span class="ButtonText">Close your browser, navigate back to this 
					    website, and try repeating your last action.</span>
			    </div>
		    </td>
	    </tr>
    </table>
    <table cellpadding="0" cellspacing="0" border="0">
	    <tr><td>&nbsp;</td></tr>
	    <tr><td>&nbsp;</td></tr>
	    <tr><td>&nbsp;</td></tr>					
    </table>
<b />
<b />
</asp:Content>
