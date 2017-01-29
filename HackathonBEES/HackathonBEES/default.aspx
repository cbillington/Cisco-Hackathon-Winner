<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HackathonBEES._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-right: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            TEST PAGE!<br />
            <br />
            <br />
            <br />
            Name<br />
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <br />
            <br />
            Email<br />
            <asp:TextBox ID="txtEmail" runat="server" CssClass="auto-style1"></asp:TextBox>
            <br />
            <br />
            Phone Number<br />
            <asp:TextBox ID="txtphone" runat="server"></asp:TextBox>
            <br />
            <br />
            Team<br />
            <asp:DropDownList ID="ddlRole" runat="server">
                <asp:ListItem Value="A">Team A</asp:ListItem>
                <asp:ListItem Value="B">Team B</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="btnRegister" runat="server" OnClick="btnRegister_Click" Text="Register" />
            <br />
            <br />
            <br />
            <asp:Button ID="btnMembers" runat="server" OnClick="btnMembers_Click" Text="Member Data" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnEmergency" runat="server" OnClick="btnEmergency_Click" Text="Emergency Data" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnSensor" runat="server" OnClick="btnSensor_Click" Text="Sensor Data" />
            <br />
            <asp:GridView ID="gvData" runat="server">
            </asp:GridView>
            <br />
            <br />

            <div id="googleMap" style="width: 100%; height: 400px;"></div>

            <script>
                function myMap() {
                    var mapProp = {
                        center: new google.maps.LatLng(51.07, -114.07),
                        zoom: 12,
                    };

                    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

                    var table = document.getElementById('gvData');
                    
                    for (var r = 1, n = table.rows.length; r < n; r++) {
                        var latt = parseFloat(table.rows[r].cells[3].innerHTML.replace(',', '.').match(/([-\d.]+)/));
                        var lngg = parseFloat(table.rows[r].cells[4].innerHTML.replace(',', '.').match(/([-\d.]+)/));
                        var desc = table.rows[r].cells[2].innerHTML.value;
                                           
                        var latLng = { lat: latt, lng: lngg }

                         var marker = new google.maps.Marker({
                            position: latLng,
                            title: desc
                        });

                        marker.setMap(map);
                    }
                }
            </script>

            <script src="https://maps.googleapis.com/maps/api/js?callback=myMap"></script>
        </div>
    </form>
</body>
</html>
