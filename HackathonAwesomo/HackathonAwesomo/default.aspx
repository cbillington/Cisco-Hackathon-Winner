<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HackathonAwesomo._default" Theme="Chart" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Team Awesomo</title>
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)" />
    <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)" />
</head>
<body>
    <form id="form1" runat="server">
        
        
        Select Patient:<br />
        <asp:DropDownList ID="ddlPatients" runat="server" OnSelectedIndexChanged="ddlPatients_SelectedIndexChanged" AutoPostBack="True" EnableViewState="True">
        </asp:DropDownList>
        <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_OnTick"></asp:Timer>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                <asp:AsyncPostBackTrigger ControlID="ddlPatients" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Chart runat="server" ID="ctHeartRate" EnableViewState="True" ThemePath="chart.skin">
                                <Series>
                                    <asp:Series Name="Series1" ChartType="Line" XValueMember="time" XValueType="Time" YValueMembers="value"></asp:Series>
                                </Series>
                                <Titles>
                                    <asp:Title Alignment="TopLeft" BackColor="Black" BorderColor="64, 0, 0" BorderWidth="3" Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="White" Name="Heart Rate" Text="Heart Rate" TextOrientation="Horizontal" TextStyle="Shadow">
                                    </asp:Title>
                                </Titles>
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

