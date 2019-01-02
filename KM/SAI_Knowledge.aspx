<%@ Page Language="C#" AutoEventWireup="false" CodeFile="SAI_Knowledge.aspx.cs" Inherits="SAI_Knowledge" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>巧新知識庫 SAI Knowledge</title>

    <link rel="shortcut icon" type="image/x-icon" href="favicon.png" />

    <link rel="stylesheet" href="js/jquery-ui.css" />
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <link rel="stylesheet" href="js/style.css" />
    <script>
        function enabledFuzzy() {
            var fuzzy = document.getElementById('<%= tbFuzzySearching.ClientID %>').value;;
            var cbFuzzy = document.getElementById('<%= cbFuzzySearching.ClientID %>');
            if(fuzzy == ""){
                cbFuzzy.checked = null;
            }
            if(fuzzy != ""){
                cbFuzzy.checked = "TRUE";
            }

            return true;
        }
        function disableFuzzy(){
            var cbFuzzy = document.getElementById('<%= cbFuzzySearching.ClientID %>');
            cbFuzzy.checked = null;
            return true;
        }
        $(function () {
            $("#tbDatepicker1").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#tbDatepicker2").datepicker({ dateFormat: 'yy/mm/dd' });
        });
        $(function () {
            $("#btnDefault").on("click", function () {
                window.location.href="SAI_Knowledge.aspx";
                //$('#<%=ddlCategory.ClientID%>').val($("#<%=ddlCategory.ClientID%> option:first").val());

                //$("#ddlCategory").val("所有分類");
                //$("#ddlSubCategory").val("所有分類");
                $("#ddlDateSort").val("0");
                $("#tbTitle").val("");
                $("#ddlSearchTitle").val("0");
                $("#tbTag").val("");
                $("#tbFuzzySearching").val("");
                $("#tbDatepicker1").val("");
                $("#tbDatepicker2").val("");
                $("#ddlDateSelection1").val("0");
                $("#ddlDateSelection2").val("0");
            });
        });
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <table border="0" width="100%">
                <tr>
                    <td>
                        <table tabindex="1" border="0" width="100%" style="background-color: #000099; color: #FF0000;">
                            <tr>
                                <td colspan="4">
                                    <asp:Label runat="server" Font-Size="18pt" ForeColor="White">巧新知識庫查詢</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="25%">
                                    <asp:Label ID="Label1" runat="server" Text="主分類查詢:"></asp:Label><br />
                                    <asp:DropDownList ID="ddlCategory" runat="server" Width="100%">
                                    </asp:DropDownList>
                                    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown_ddlCategory" runat="server" Category="MainCategory"
                                        Enabled="true" EmptyText="所有類別" TargetControlID="ddlCategory"
                                        ServiceMethod="GetMainCategory" ServicePath="WebService.asmx" />
                                </td>
                                <td width="25%">
                                    <asp:Label ID="Label4" runat="server" Text="次分類查詢: "></asp:Label><br />
                                    <asp:DropDownList ID="ddlSubCategory" runat="server" Width="100%"></asp:DropDownList>
                                    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown_ddlSubCategory" runat="server" Category="SubCategory"
                                        Enabled="true" EmptyText="所有類別" ParentControlID="ddlCategory" TargetControlID="ddlSubCategory"
                                        ServiceMethod="GetSubCategoryByMain" ServicePath="WebService.asmx" />
                                </td>
                                <td width="20%">
                                    <label>日期排序</label><br />
                                    <asp:DropDownList ID="ddlDateSort" ToolTip="升序:舊至新, 降序:新至舊" runat="server">
                                        <asp:ListItem Value="0">無排序</asp:ListItem>
                                        <asp:ListItem Value="1">升序</asp:ListItem>
                                        <asp:ListItem Value="2">降序</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="模糊查詢: (多條件查詢用逗號分隔)"></asp:Label>
                                    <asp:CheckBox ID="cbFuzzySearching" runat="server" ToolTip="啟用模糊查詢將會停用其他查詢條件" /><br />
                                    <asp:TextBox ID="tbFuzzySearching" runat="server" ToolTip="啟用模糊查詢將會停用其他查詢條件" Width="60%"></asp:TextBox>
                                    
                                    <asp:Button ID="btnFuzzySearch" runat="server" OnClick="btnFuzzySearch_Click" Text="搜尋" UseSubmitBehavior="true" Width="80px" OnClientClick="if (!enabledFuzzy()) return false;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table border="1" width="100%">
                <td width="45%">
                    <asp:Label ID="Label2" runat="server" Text="主旨查詢:"></asp:Label>
                    <asp:DropDownList ID="ddlSearchTitle" runat="server">
                        <asp:ListItem Value="0">Like</asp:ListItem>
                        <asp:ListItem Value="1">=</asp:ListItem>
                        <asp:ListItem Value="2"><></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="tbTitle" placeholder="替代軟體放置路徑" Width="25%" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="搜尋" Width="80px" OnClick="btnSearch_Click" UseSubmitBehavior="true" OnClientClick="if (!disableFuzzy()) return false;"/>
                    &nbsp;
                        <asp:Button ID="btnDefault" runat="server" Text="清空搜尋" Width="80px" OnClientClick=" return false;" UseSubmitBehavior="false" />
                </td>
            </table>
        </div>

        <br />

        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvDataList" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" PageSize="25"
                        AutoGenerateColumns="false" Width="100%" AllowPaging="True" OnPageIndexChanging="gvDataList_PageIndexChanging" RowDataBound="gvDataList_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="編號" ItemStyle-Height="40">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
                            </asp:TemplateField>

                        </Columns>
                        <PagerSettings Position="Bottom" />
                        <PagerTemplate>
                            <tr>
                                <td colspan="5" style="text-align: right">
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
