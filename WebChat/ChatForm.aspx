<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChatForm.aspx.cs" Inherits="WebChat.ChatForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #TextArea1 {
            height: 439px;
            width: 581px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                </asp:Timer>
                <asp:ListBox ID="ListBox1" runat="server" Height="184px" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" Width="408px"></asp:ListBox>
            </ContentTemplate>
        </asp:UpdatePanel>
        <p>
        <asp:TextBox ID="msgBox" runat="server"></asp:TextBox>
        <asp:Button ID="sendButton" runat="server" OnClick="Button1_Click" Text="Send" />
        </p>
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
