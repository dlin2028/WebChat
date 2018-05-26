<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChatForm.aspx.cs" Inherits="WebChat.ChatForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:MultiView ID="MultiView1" runat="server">
            <br />
            <br />
        </asp:MultiView>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Send" />
        <asp:Timer ID="Timer1" runat="server" Interval="250" OnTick="Timer1_Tick">
        </asp:Timer>
    </form>
</body>
</html>
