<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SAI_Knowledge_Manager.aspx.cs" Inherits="SAI_Knowledge_Manager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>巧新知識庫管理員 SAI Knowledge Managmenter</title>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css" />
    <script src="https://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="http://jqueryui.com/resources/demos/style.css" />
    <script>
        $(function () {
            $("#tbDatepicker1").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#tbDatepicker2").datepicker({ dateFormat: 'yy/mm/dd' });
        });
        $(function () {
            $("#btnTagDialog").on("click", function () {
                window.open("getTag.aspx", "_blank", "width=250px,height=350px");
            });
        });
    </script>

</head>
<body >
    <form id="form1" runat="server" defaultbutton="btnSearch" autocomplete="on">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <table tabindex="0" border="0" width="100%">
                <tr>
                    <td width="255px">
                        <img src="./img/sai-logo-large.png" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="labelUSER" Text="使用者 歡迎您！"></asp:Label>
                        <table border="2" width="100%">
                            <tr>
                                <td width="150px" style="text-align: center" height="30px">
                                    <asp:LinkButton runat="server" Text="資訊安全知識分享" OnClick="btnInfomationSafe_Click" />
                                </td>
                                <td width="150px" style="text-align: center" height="30px">
                                    <asp:LinkButton runat="server" Text="軟體安裝及一般知識" OnClick="btnInstall_Click" />
                                </td>
                                <td width="150px" style="text-align: center" height="30px">
                                    <asp:LinkButton runat="server" Text="SAP/ERP系統" OnClick="btnSAP_Click" />
                                </td>
                                <td width="150px" style="text-align: center" height="30px">
                                    <asp:LinkButton runat="server" Text="文書處理" OnClick="btnOffice_Click" />
                                </td>
                                <td width="150px" style="text-align: center" height="30px">
                                    <asp:LinkButton runat="server" Text="BPM" OnClick="btnBPM_Click" />
                                </td>
                                <td width="150px" style="text-align: center" height="30px">
                                    <asp:LinkButton runat="server" Text="工程軟體" OnClick="btnEngineSortWare_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />

            <table tabindex="1" border="0" width="100%" style="background-color: #000099; color: #FF0000;">
                <tr>
                    <td colspan="4">
                        <asp:Label runat="server" Font-Size="18pt" ForeColor="White">巧新管理員知識庫查詢</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="主分類查詢:"></asp:Label>
                        <asp:DropDownList ID="ddlCategory" runat="server" Height="20px" Width="60%" AutoPostBack="true"
                            OnTextChanged="ddlCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="次分類查詢: "></asp:Label>
                        <asp:DropDownList ID="ddlSubCategory" runat="server" Height="20px" Width="60%"></asp:DropDownList>
                    </td>
                    <td width="15%">
                        <label>日期排序</label>
                        <asp:DropDownList ID="ddlDateSort" ToolTip="升序:舊至新, 降序:新至舊" runat="server">
                            <asp:ListItem Value="無排序"></asp:ListItem>
                            <asp:ListItem Value="升序"></asp:ListItem>
                            <asp:ListItem Value="降序"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="模糊查詢:"></asp:Label>
                        <asp:CheckBox ID="cbFuzzySearching" runat="server" ToolTip="啟用模糊查詢將會停用其他查詢條件" />
                        <asp:TextBox ID="tbFuzzySearching" runat="server" ToolTip="啟用模糊查詢將會停用其他查詢條件"
                            OnTextChanged="tbFuzzySearching_TextChanged" Width="60%" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <table border="1" width="100%">
                <td>
                    <asp:Label ID="Label2" runat="server" Text="主旨查詢:"></asp:Label>
                    <asp:DropDownList ID="ddlSearchTitle" runat="server">
                        <asp:ListItem>Like</asp:ListItem>
                        <asp:ListItem>=</asp:ListItem>
                        <asp:ListItem><></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="tbTitle" placeholder="替代軟體放置路徑" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="關鍵字查詢:"></asp:Label>
                    <asp:TextBox ID="tbTag" placeholder="EMAIL,代理人" ToolTip="使用逗號區隔" Width="50%" runat="server"></asp:TextBox>
                    <asp:Button ID="btnTagDialog" runat="server" Width="80px" Text="選單選取" OnClientClick="return false;" UseSubmitBehavior="false" />
                </td>
                <td>
                    <label>啟用狀態</label>
                    <asp:DropDownList ID="ddlEnabled" runat="server">
                        <asp:ListItem Selected="True">ALL</asp:ListItem>
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="text-align: center">
                    <asp:Button ID="btnSearch" runat="server" Text="搜尋" Width="80px" OnClick="btnSearch_Click" UseSubmitBehavior="true" />
                    &nbsp;
                        <asp:Button ID="btnDefault" runat="server" Text="清空搜尋" Width="80px" OnClick="btnDefault_Click" />
                    &nbsp;
                        <asp:Button ID="btnCreate" runat="server" Text="新增" Width="80px" OnClick="btnCreate_Click" />
                    &nbsp;
                        <asp:Button ID="btnManager" runat="server" Text="管理人員" Width="80px" OnClientClick="window.open('user_manager.aspx', '_blank', 'width=350px,height=500px'); return false;" />
                    &nbsp;
                        <asp:Button ID="btnExport" runat="server" Text="匯出資料" OnClick="btnExport_Click" />
                </td>
                <tr>
                    <td colspan="4">
                        <label>日期查詢</label>
                        <asp:DropDownList ID="ddlDateSelection1" runat="server">
                            <asp:ListItem>UpdateTime</asp:ListItem>
                            <asp:ListItem>CreatedTime</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlDateSelType1" runat="server">
                            <asp:ListItem>大於等於</asp:ListItem>
                            <asp:ListItem>等於</asp:ListItem>
                            <asp:ListItem>小於等於</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="tbDatepicker1" runat="server"></asp:TextBox>

                        <asp:DropDownList ID="ddlDateSelection2" runat="server">
                            <asp:ListItem>UpdateTime</asp:ListItem>
                            <asp:ListItem>CreatedTime</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlDateSelType2" runat="server">
                            <asp:ListItem>大於等於</asp:ListItem>
                            <asp:ListItem>等於</asp:ListItem>
                            <asp:ListItem Selected="True">小於等於</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="tbDatepicker2" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>

        </div>

        <br />

        <div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvDataList" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" PageSize="25"
                        AutoGenerateColumns="false" Width="100%" AllowPaging="True" OnPageIndexChanging="gvDataList_PageIndexChanging" RowDataBound="gvDataList_RowDataBound">
                        <Columns>
                            <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" Width="40px" PostBackUrl='<%# "~/KM/SAI_Knowledge_Editor.aspx?op=update&id="+Eval("id") %>'>編輯</asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="編號" ItemStyle-Height="40">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="啟用狀態" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("enabled") %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="主分類" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lable_category" runat="server" Style="text-align: center" Text='<%# Eval("category") %>' Width="70"></asp:Label>
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="次分類" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lable_subCategory" runat="server" Style="text-align: center"
                                        Text='<%# Eval("subCategory") %>' Width="70"></asp:Label>
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="主旨" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <a href='<%# Eval("path") %>' target="_blank"><%# Eval("title") %> </a>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="更新日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lable_updateTime" runat="server" Text='<%# Eval("updateTime") %>' Width="200"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>

                        </Columns>
                        <PagerSettings Position="Bottom" />
                        <PagerTemplate>
                            <tr>
                                <td colspan="7" style="text-align: right">
                                    <asp:PlaceHolder ID="phdPageNumber" runat="server"></asp:PlaceHolder>
                                    第<asp:Label ID="lblPageIndex" runat="server" Text="<%#((GridView)Container.Parent.Parent).PageIndex + 1 %>"></asp:Label>頁           
                            共<asp:Label ID="lblPageCount" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label>頁
                            <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First" CommandName="Page" Text="第一頁"></asp:LinkButton>
                                    <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev"
                                        CommandName="Page" Text="上一頁"></asp:LinkButton>
                                    <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next"
                                        CommandName="Page" Text="下一頁"></asp:LinkButton>
                                    <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last"
                                        CommandName="Page" Text="最後頁"></asp:LinkButton>
                                    <asp:TextBox ID="txtNewPageIndex" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1%>" Width="20px"></asp:TextBox>
                                    <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-1" CommandName="Page" Text="GO"></asp:LinkButton>
                                </td>
                            </tr>
                        </PagerTemplate>
                        <FooterStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                        <RowStyle BackColor="#E6E6E6" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

            &nbsp;
        </div>
    </form>
</body>

</html>
