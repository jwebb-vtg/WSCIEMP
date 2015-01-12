<%@Page Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" AutoEventWireup="true" CodeBehind="AlertAnnouncements.aspx.cs" 
Inherits="WSCIEMP.SHS.AlertAnnouncements" Title="Western Sugar Cooperative - Announcements" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
Announcements
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">
    <table cellspacing="0" cellpadding="0">
	    <tr>
		    <td style="width:2%;"></td>
		    <td style="width:96%;"><br />
			    <div runat="server" id="divResults">
				    <asp:DataList id="dlResults" runat="server">
					    <ItemTemplate>
						    <span class="LabelText">
							    <%# DataBinder.Eval(Container.DataItem, "Title")%>
						    </span>
						    <br />
						    <span class="ButtonText">
							    <%# DataBinder.Eval(Container.DataItem, "UpdateDate")%>
						    </span>
						    <br />
						    <span class="ButtonText">
							    <%# DataBinder.Eval(Container.DataItem, "AbstractDesc")%>
						    </span>... <a class="ButtonText" href="#" onclick="PopAlert('<%# DataBinder.Eval(Container.DataItem, "Url")%>', event);">
							    Click to open... </a>
						    <br />
						    <hr style="width:80%; height: 4px; color: #ffffcc; background-color: #ffffcc; " />
						    <br />
					    </ItemTemplate>
				    </asp:DataList>
				    <br />
				    <br />
				    <br />
			    </div>
		    </td>
		    <td></td>
	    </tr>
    </table>
</asp:Content>
