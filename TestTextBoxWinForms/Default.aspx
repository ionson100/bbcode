<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" ValidateRequest="false" %>

<%@ Register Assembly="TB312" Namespace="TB312" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Просто</title>
    <script src="Scripts/jquery-1.7.1.min.js"></script>
    <link href="/TB12/Tb312.css" rel="stylesheet" />
    <link href="/TB12/dp.SyntaxHighlighter/Styles/SyntaxHighlighter.css" rel="stylesheet" />
    <script src="/TB12/tt.js"></script>
    <script src="/TB12/Tb312.js"></script>

    <script src="/TB12/dp.SyntaxHighlighter/Uncompressed/shCore.js"></script>
    <script src="/TB12/dp.SyntaxHighlighter/Uncompressed/shBrushCpp.js"></script>
    <script src="/TB12/dp.SyntaxHighlighter/Uncompressed/shBrushCSharp.js"></script>
    <script src="/TB12/dp.SyntaxHighlighter/Uncompressed/shBrushCss.js"></script>
    <script src="/TB12/dp.SyntaxHighlighter/Uncompressed/shBrushJScript.js"></script>
    <script src="/TB12/dp.SyntaxHighlighter/Uncompressed/shBrushPhp.js"></script>
    <script src="/TB12/dp.SyntaxHighlighter/Uncompressed/shBrushSql.js"></script>
    <script src="/TB12/dp.SyntaxHighlighter/Uncompressed/shBrushXml.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="width: 800px;">
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
            <div>
                <cc1:TextBoxForm ID="TextBoxForm1" runat="server"  OnSaveText="TextBoxForm1SaveText" />
            </div>

            <div>
                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1Click" /></div>
        </div>



    </form>
</body>
</html>
