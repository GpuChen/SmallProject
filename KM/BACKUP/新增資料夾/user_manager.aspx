<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_manager.aspx.cs" Inherits="user_manager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理人員</title>
	<script type="text/javascript" src="https://code.jquery.com/jquery-1.9.1.min.js""></script>
    <script type="text/javascript">
    function delComfirm() {
       return confirm('確認刪除？');
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnCancel" runat="server" Text="關閉" OnClientClick="window.close();" />
            <br />
            <br />
            員工工號：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><asp:Button ID="btnCreate" runat="server" Text="新增" OnClick="btnCreate_Click"/>
			<asp:Button ID="btnDelChose" runat="server" Text="刪除" OnClientClick="if (!delComfirm()) return false;" OnClick="btnDelChose_Click" />
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" PageSize="10"
                        AutoGenerateColumns="false" Width="100%" AllowPaging="True" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnDelete" Text="刪除" OnClientClick="if (!delComfirm()) return false;" CommandName="btnDelete" CommandArgument='<%# Eval("userid") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="編號">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="員工工號">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="label1" Text='<%# Eval("userid") %>' ></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                        <RowStyle BackColor="#E6E6E6" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
