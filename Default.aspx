<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #InsertRecordBtn {
            height: 32px;
            width: 137px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>

     <asp:Label ID="welcomeLbl" runat="server" Text="Welcome, Guest!" style="text-align:right"></asp:Label>
        &nbsp;<br />
    <asp:Button ID="btnGoogleLogin" runat="server" CommandArgument="https://www.google.com/accounts/o8/id" 
    OnCommand="btnGoogleLogin_Click" Text="Sign In with Google"/>

    </div>
</body>
</html>
    <p>
        <strong>&nbsp;Alt Text:</strong>&nbsp;
        <asp:TextBox ID="alt_text_box" runat="server" OnTextChanged="alt_text_box_TextChanged"></asp:TextBox>
    </p>
    <p>
        <strong>Enter text here:</strong>
        <asp:TextBox ID="Meme_Text_Box" runat="server" OnTextChanged="Meme_Text_Box_TextChanged"></asp:TextBox>
    </p>
    <p style="font-weight: 700">
        <asp:FileUpload ID="FileUpload1" runat="server" />
    </p>
    <p>
        <asp:Button ID="insertRecordBtn" runat="server" Text="Upload" OnClick="insertRecordBtn_Click" />
    </p>
    <p>
        <asp:Image ID="Image1" runat="server" />
    </p>
    <p style="font-weight: 700">
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        &nbsp;</p>

    </form>


