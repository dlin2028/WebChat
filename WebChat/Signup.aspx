<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="WebChat.Signup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Username"></asp:Label>
        <p>
            <asp:TextBox ID="userBox" runat="server"></asp:TextBox>
        </p>
        <asp:Label ID="Label2" runat="server" Text="Room Name"></asp:Label>
        <p>
            <asp:TextBox ID="roomBox" runat="server" Height="16px"></asp:TextBox>
            <asp:DropDownList ID="roomList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="roomList_SelectedIndexChanged">
            </asp:DropDownList>
        </p>
        <asp:Label ID="Label3" runat="server" Text="Room password"></asp:Label>
        <p>
            <asp:TextBox ID="pwBox" runat="server"></asp:TextBox>
        </p>
        <asp:DropDownList ID="colorList" runat="server">
        </asp:DropDownList>
        <p>
        <asp:Button ID="goButton" runat="server" OnClick="Button1_Click" Text="Go" style="height: 26px" />
        </p>
        <p>
            <asp:Label ID="outLabel" runat="server"></asp:Label>
        </p>
    </form>
</body>
</html>
