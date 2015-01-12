<%@ Page Title="" Language="C#" MasterPageFile="~/PrimaryTemplate.Master" StylesheetTheme="ThemeA_1024" ValidateRequest="false"
AutoEventWireup="true" CodeBehind="AppErrors.aspx.cs" Inherits="WSCIEMP.Admin.AppErrors" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleHolder" runat="server">
    WSCIEMP Application Errors
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentCenter" runat="server">

    <script type="text/javascript">

        $(document).ready(function() {

            //-----------------------------------------
            // Setup calendar
            //-----------------------------------------
            $$('txtFromDate', $('#selectCriteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
                buttonImage: '../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: true, yearRange: '-10:+1',
                changeMonth: true, changeYear: true, duration: 'fast' });
            $$('txtToDate', $('#selectCriteria')).datepicker({ dateFormat: 'm/d/yy', showOn: 'button', buttonImageOnly: true,
                buttonImage: '../img/Cal2.gif', buttonText: '', alignment: 'bottomRight', closeAtTop: true, yearRange: '-10:+1',
                changeMonth: true, changeYear: true, duration: 'fast'});
        });

    </script>
    
    <table style="width: 60%; width: 50%;" class="stdTableLayout" border="0" id="selectCriteria">
        <tr>
            <td>
                <span class="LabelText">From Date:</span>
            </td>
            <td>
                <span class="LabelText">To Date:</span>
            </td>
            <td>
                <span class="LabelText">Status:</span>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 27%">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="ctlWidth75"></asp:TextBox>
            </td>
            <td style="width: 30%">
                <asp:TextBox ID="txtToDate" runat="server" CssClass="ctlWidth75"></asp:TextBox>
            </td>
            <td style="width: 30%">
                &nbsp;<asp:DropDownList ID="ddlStatus" runat="server" CssClass="ctlWidth75" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="ButtonText" OnClick="btnRefresh_Click" />
            </td>
        </tr>
    </table>
    <br />
    <div runat="server" id="resultFrame" class="DisplayOn" style="width: 940px; height:170px; overflow: auto;">
    <asp:GridView ID="grdResult" SkinID="grdColor" runat="server" AutoGenerateColumns="False"
        CssClass="grdColor920" Width="1400" OnSelectedIndexChanged="grdResult_SelectedIndexChanged"
        OnRowDataBound="grdResult_RowDataBound" OnRowCreated="grdResult_RowCreated">
        <Columns>
            <asp:TemplateField ShowHeader="True" Visible="True">
                <ItemTemplate>
                    <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                        Text="Select"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="AppName" HeaderText="Application">
                <ItemStyle Width="8%" />
            </asp:BoundField>
            <asp:BoundField DataField="Severity" HeaderText="Severity">
                <ItemStyle Width="7%" />
            </asp:BoundField>            
            <asp:BoundField DataField="ErrorDate" HeaderText="Date\Time">
                <ItemStyle HorizontalAlign="Center" Width="10%" />
            </asp:BoundField>
            <asp:BoundField DataField="Status" HeaderText="Status">
                <ItemStyle HorizontalAlign="Center" Width="6%" />
            </asp:BoundField>
            <asp:BoundField DataField="Action" HeaderText="Action">
                <ItemStyle HorizontalAlign="Left" Width="17%" />
            </asp:BoundField>
            <asp:BoundField DataField="LoginServer" HeaderText="Server">
                <ItemStyle HorizontalAlign="Left" Width="7%" />
            </asp:BoundField>
            <asp:BoundField DataField="LoginClient" HeaderText="Client">
                <ItemStyle HorizontalAlign="Left" Width="7%" />
            </asp:BoundField>
            <asp:BoundField DataField="LoginUser" HeaderText="User">
                <ItemStyle HorizontalAlign="Left" Width="7%" />
            </asp:BoundField>
            <asp:BoundField DataField="ErrorCode" HeaderText="Code">
                <ItemStyle HorizontalAlign="Left" Width="6%" />
            </asp:BoundField>                                                
            <asp:BoundField DataField="Path" HeaderText="" ShowHeader="True" Visible="True">
            </asp:BoundField>
        </Columns>
        <EmptyDataTemplate>
            <span class="highlightPlus">No Error Reports Exist.</span>
        </EmptyDataTemplate>
    </asp:GridView>
    </div>
    <br />
    <div  style="border: solid 1px #000000; padding: 4px 4px 4px 4px; text-align: left; font-size: 10px; width: 98%">
        <table style="width: 98%;" class="stdTableLayout" border="0">
            <tr>
                <td><span class="LabelText">Application:</span></td>
                <td><span class="LabelText" 
                title="Critical: Catastrophic production problem, application should be shutdown. High: Application is functional but significant problem exists.  Medium: Application is functional but some operations are impared.  Low: Application is fully functional, but problem is an annoyance.">Severity:</span></td>
                <td><span class="LabelText">Status:</span></td>
                <td colspan="3"><span class="LabelText">Action:</span></td>                
            </tr>
            <tr>
                <td><asp:DropDownList ID="ddlEditApplication" runat="server" CssClass="ctlWidth90"></asp:DropDownList></td>                
                <td><asp:DropDownList ID="ddlEditSeverity" runat="server" CssClass="ctlWidth90"></asp:DropDownList></td>                
                <td><asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="ctlWidth75"></asp:DropDownList></td>
                <td colspan="3"><asp:TextBox ID="txtEditAction" runat="server" CssClass="ctlWidth120" Width="450"></asp:TextBox></td>
            </tr>
            <tr>
                <td width="15%"><span class="LabelText">Server:</span></td>
                <td width="15%"><span class="LabelText">Client:</span></td>
                <td width="15%"><span class="LabelText">User:</span></td>                
                <td width="15%"><span class="LabelText">Error Code:</span></td>
                <td width="15%"><span class="LabelText">Break on Text*:</span></td> 
                <td width="25%">&nbsp;</td>               
            </tr>   
            <tr>
                <td><asp:TextBox ID="txtEditServer" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>
                <td><asp:TextBox ID="txtEditClient" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>
                <td><asp:TextBox ID="txtEditUser" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>                
                <td><asp:TextBox ID="txtEditErrorCode" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>
                <td><asp:TextBox ID="txtEditBreakText" runat="server" CssClass="ctlWidth90"></asp:TextBox></td>
                <td>&nbsp;</td>
            </tr>  
            <tr>
                <td colspan="6" class="ButtonText">* Copy Break on Text value into error text below to create seperate errors when using Import\Update.</td>
            </tr>                   
        </table>    
    </div>
    <br />
    <div id="divErrorFile" style="border: solid 1px #000000; padding: 4px 4px 4px 4px;
        text-align: center; font-size: 10px; width: 98%">
        <div style="text-align: right;">
            <asp:Button ID="btnImportErrorFile" runat="server" Text="Import\Update" OnClick="btnImportErrorFile_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnOpenStatus" runat="server" Text="Open Status" OnClick="btnOpenStatus_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCloseStatus" runat="server" Text="Close Status" OnClick="btnCloseStatus_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnDeleteErrorFile" runat="server" Text="Delete Error" OnClick="btnDeleteErrorFile_Click" />
        </div>
        <div style="background-color: #FFFFFF; border: solid 1px #000000;">
            <!-- <div id="divErrorFileText" runat="server" class="ButtonText" style="width: 930px; height: 500px; overflow: auto; text-align: left; background-color: #FFFFFF; padding: 5px 5px 5px 5px">
            -->
                <asp:TextBox ID="txtErrorText" runat="server" CssClass="ButtonText" Width="930" Height="500" Wrap="true" TextMode="MultiLine"></asp:TextBox>
            <!-- </div> -->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBottom" runat="server">
</asp:Content>
