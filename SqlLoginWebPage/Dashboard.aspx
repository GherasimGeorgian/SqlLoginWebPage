<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SqlLoginWebPage.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblUserDetails" runat="server" Text="Label"></asp:Label>
            <br/>
            <asp:Button ID="bntLogout" runat="server" Text="LogOut" OnClick="bntLogout_Click" />
        </div>
    </form>
</body>
</html>
