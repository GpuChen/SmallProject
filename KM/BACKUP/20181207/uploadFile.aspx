<%@ Page Language="C#" AutoEventWireup="false" CodeFile="uploadFile.aspx.cs" Inherits="uploadFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>檔案上傳</title>
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script type="text/javascript">
        $(function () {

            $("#btnCancel").on("click", function () {
                window.close();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label style="color: #808080">(檔案格式：pdf, mth, htm)</label>
            <asp:FileUpload ID="FileUpload1" runat="server" />

            <asp:Button ID="btnSubmit" runat="server" Text="確認上傳" OnClick="btnSubmit_Click" />
            &nbsp;<asp:Button ID="btnCancel" runat="server" Text="取消" />

        </div>
    </form>
</body>
</html>
