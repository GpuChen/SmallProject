<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getTag.aspx.cs" Inherits="getTag" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>關鍵字表單</title>

    <script type="text/javascript" src="https://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#Button1").on("click", function () {
                var tagStr = (window.opener.document.getElementById("tbTag").value == "") ? "" : ",";
                var checkList1 = document.getElementById('<%= CheckBoxList1.ClientID %>');
                var checkBoxList1 = checkList1.getElementsByTagName("input");
                for (var i = 0; i < checkBoxList1.length; i++) {
                    if (checkBoxList1[i].checked)
                        tagStr += checkBoxList1[i].value + ",";
                }
                tagStr = tagStr.substr(0, tagStr.length - 1);
                window.opener.document.getElementById("tbTag").value += tagStr;
                //window.close();
            });
            $("#Button2").on("click", function () {
                window.close();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
            &nbsp;
            <asp:Button ID="Button2" runat="server" Text="關閉" UseSubmitBehavior="false" />
            <br />
            <asp:Label runat="server" Text="主分類" />
            <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
            <br />
            <asp:Label runat="server" Text="次分類" />
            <asp:DropDownList ID="ddlSubCategory" runat="server"></asp:DropDownList>
            <br />

            <asp:Button ID="Button1" runat="server" Text="選擇選取項目" Width="40%" UseSubmitBehavior="false" />
            <div style="overflow-y: scroll; width: 100%; height: 300px">
                <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                </asp:CheckBoxList>
            </div>

        </div>
    </form>
</body>
</html>
