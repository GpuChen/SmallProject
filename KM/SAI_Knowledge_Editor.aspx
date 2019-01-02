<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SAI_Knowledge_Editor.aspx.cs" Inherits="SAI_Knowledge_Editor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=big5" />
    <title></title>
    <style>
        .auto-style1 {
            width: 15%;
        }
    </style>
    
    <link rel="shortcut icon" type="image/x-icon" href="favicon.png" />

    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        function checkEvent() {
            var title = document.getElementById('<%= tbTitle.ClientID %>').value;
            var fileName = document.getElementById('<%= tbUploadFileName.ClientID %>').value;
            var checkUpload = document.getElementById('<%= cbUploadFile.ClientID %>');
            var operation = document.getElementById('<%= labelOperation.ClientID %>').textContent;
            var Path = document.getElementById('<%= tbPath.ClientID %>').value;
			
            var selValue;
            var table = document.getElementById("rblObjectType");
            for (i = 0; i < table.rows[0].cells.length; i++)
                if (table.rows[0].cells[i].childNodes[0].checked == true)
                    selValue = table.rows[0].cells[i].childNodes[0].value;
            if (title == "") {
                alert("�D�����ର��");
                return false;
            }
            if (selValue == "0" && fileName == "" && !checkUpload.checked) {
                alert("�ɮץ�������W��");
                return false;
            }
            if (selValue == "1" && Path == "") {
                alert("���}���|���ର��");
                return false;
            }
            return true;
            //return confirm('Hello!');
        }
        $(function () {
            $("#btnTagSelect").on("click", function () {
                window.open("getTag.aspx?enabled=N", "_blank", "width=250px,height=450px");
            });
            $("#btnUploadFile").on("click", function () {
                var newCategory = document.getElementById('<%= tbNewCategory.ClientID %>').value;
                var newSubCategory = document.getElementById('<%= tbNewSubCategory.ClientID %>').value;
                var category = document.getElementById('<%= ddlCategory.ClientID %>').value;
                var subCategory = document.getElementById('<%= ddlSubCategory.ClientID %>').value;
                var _cate = (newCategory == "") ? category : newCategory;
                var _subCate = (newSubCategory == "") ? subCategory : newSubCategory;
                var w = window.open(("uploadFile.aspx?category=" + _cate + "&subCategory=" + _subCate + ""), "_blank", "width=250px,height=150px");
                w.document.title = "�W���ɮ�";
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table border="0" width="100%">
                <tr>
                    <td>
                        <img src="./img/sai-logo-large.png" />
                    </td>
                </tr>
            </table>
            <br />
			<asp:Label ID="labelOperation" runat="server" Font-Size="18pt" />
            <asp:Label runat="server" Visible="true">��ƽs���G</asp:Label>
            <asp:TextBox ID="tbID" Width="288px" runat="server" ReadOnly="true" BackColor="#CCCCCC" Visible="true"></asp:TextBox>
            <table border="1" width="100%" cellpadding="5">
                <tr>
                    <td>
                        <label>IT�t�d�H�G</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbCreator" Width="288px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <label>�إߤ���G</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbCreateTime" Width="288px" ReadOnly="true" runat="server" BackColor="#CCCCCC"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>�̫��s�H���G</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbUpdateUser" Width="288px" ReadOnly="true" runat="server" BackColor="#CCCCCC"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>�̫��s����G</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbUpdateTime" Width="288px" ReadOnly="true" runat="server" BackColor="#CCCCCC"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <label>�D���� / �������G</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCategory" Width="15%" runat="server" AutoPostBack="true"
                            OnTextChanged="ddlCategory_TextChanged">
                        </asp:DropDownList>
                        /
                        <asp:DropDownList ID="ddlSubCategory" Width="15%" runat="server"></asp:DropDownList>
                        ||
                        �s�W�D����<asp:TextBox ID="tbNewCategory" runat="server" placeholder="�ŭȫh�M�ΤU�Կﶵ"></asp:TextBox>
                        /
                        �s�W������<asp:TextBox ID="tbNewSubCategory" placeholder="�ŭȫh�M�ΤU�Կﶵ" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <label>�D���G</label></td>
                    <td>
                        <asp:TextBox ID="tbTitle" Width="80%" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <label>����r�G</label></td>
                    <td>
                        <asp:Button ID="btnTagSelect" runat="server" Text="�������r" />
                        <asp:TextBox ID="tbTag" Width="80%" ReadOnly="false" runat="server" placeholder="�d��1,�d��2"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <label>�ҥα��ءG</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbEnabled" runat="server" Checked="True" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <label>�ɮ׳]�w�G</label>
                    </td>
                    <td>
                        <asp:Button ID="btnUploadFile" runat="server" Text="�W���ɮ�" />
                        <asp:TextBox ID="tbUploadFileName" runat="server" ReadOnly="true" BackColor="#CCCCCC"></asp:TextBox>
                        <asp:CheckBox ID="cbUploadFile" runat="server" Checked ="false" onclick="return false;"/>
                        <br />
                        <asp:RadioButtonList ID="rblObjectType" runat="server"
                            OnSelectedIndexChanged="rblObjectType_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="true">
                            <asp:ListItem Value="0">�ɮ׸��|</asp:ListItem>
                            <asp:ListItem Value="1">���}���|</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <label>���}�]�w�G</label>
                    </td>
                    <td>���}���|�G<asp:TextBox ID="tbPath" Width="80%" ReadOnly="false" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="text-align: center">
                        <asp:Button ID="btnSubmit" Width="10%" runat="server" Text="�e�X" OnClientClick="if (!checkEvent()) return false;" OnClick="btnSubmit_Click" />
                        &nbsp;<asp:Button ID="btnCancel" Width="10%" runat="server" Text="�^�쭺��" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
