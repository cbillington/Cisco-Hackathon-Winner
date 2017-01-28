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
        <asp:Button ID="btnRegister" runat="server" OnClick="btnRegister_Click" Text="Register" />
        <br />
        <br />
        <br />
        </div>
    </form>
</body>
</html>
