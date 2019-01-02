using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SAI_Knowledge : System.Web.UI.Page
{

    // DBINIT
    // Connection : Using()
    // SQLstr
    // 條件式

    // DATABIND


    //Using oCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("SAEE").ConnectionString)

    // Connection String
    //string _con = "SERVER =192.168.1.129;DATABASE=SAEE_KM;UID=saee;PWD=92vsk48mdf";
    string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE_KM"].ConnectionString;
    string _table = "SAI_KM";
    string _tableTag = "SAI_KM_TAG";
    string _tableManager = "SAI_KM_MANAGER";
    string _sql = "";


    protected void Page_Load(object sender, EventArgs e)
    {

        // Setting default
        // User Checking not using temporary.

        // init bind data for selecting operator
        if (!IsPostBack)
        {
            tbTitle.Focus();
            /*
            if (Request.Params["category"] != "")
            {
                if (CascadingDropDown_ddlCategory.SelectedValue.ToString().Contains((Request.Params["category"])))
                    ddlCategory.SelectedValue = Request.Params["category"];
            }*/
            //CascadingDropDown_ddlCategory.SelectedValue = "";
            //subCategory();

            //DBINIT();
        }

    }

    private void DBINIT()
    {
        // Control Main Function
        // initial variable
        _sql = "";

        // Define Search Variable String from WebPage
        // ddl=DropDownList, tb=TextBox

        string searchTitle = "";
        switch (ddlSearchTitle.SelectedItem.ToString())
        {
            case "Like":
                searchTitle = " and title like '%" + tbTitle.Text + "%'";
                break;
            case "=":
                searchTitle = " and title in ('" + tbTitle.Text + "')";
                break;
            case "<>":
                searchTitle = " and title not in ('" + tbTitle.Text + "')";
                break;

        }
        string title = (tbTitle.Text.Equals("")) ? "" : searchTitle;
        string category = (ddlCategory.SelectedValue.Equals("所有分類")) ? "" : " and category='" + ddlCategory.SelectedValue + "'";
        string subCategroy = (ddlSubCategory.SelectedValue.Equals("所有分類")) ? "" : " and subCategory='" + ddlSubCategory.SelectedValue + "'";

        string dateSort = "";
        switch (ddlDateSort.SelectedIndex)
        {
            case 0:
                dateSort = "";
                break;
            case 1:
                dateSort = " ORDER BY updateTime ASC";
                break;
            case 2:
                dateSort = " ORDER BY updateTime DESC";
                break;
        }

        // 這個部分使用 and以後內容可能要以or的方式去撈 語意"為包含有"
        // 弱條件, 不用全符合
        // tag container

        string[] tag = tbTag.Text.Split(",".ToArray());
        string tagSql = "";
        foreach (string s in tag)
        {
            if (!tagSql.Contains("and"))
            {
                tagSql += "and (tag like '%" + s.Trim(' ') + "%' ";
            }
            else
            {
                tagSql += "or tag like '%" + s.Trim(' ') + "%' ";
            }
        }
        tagSql += ")";
        if (tbTag.Text.Equals(""))
            tagSql = "";

        // FuzzySearch setting (if using fuzzySearch, will disable other condition)
        string FuzzySearching = "";
        if (cbFuzzySearching.Checked)
            FuzzySearching = (tbFuzzySearching.Text.Equals("")) ? "" : tbFuzzySearching.Text.Trim();
        string FuzzySql = "and (title like '%" + FuzzySearching + "%' or category like '%" + FuzzySearching + "%' or subCategory like '%" + FuzzySearching + "%' or tag like '%" + FuzzySearching + "%' )";


        if (tbDatepicker1.Text.Length < 10 && tbDatepicker1.Text != "")
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('日期輸入不符合規範');", true);
            return;
        }
        if (tbDatepicker2.Text.Length < 10 && tbDatepicker2.Text != "")
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('日期輸入不符合規範');", true);
            return;
        }

        string _DateSelType1 = "";
        string _DateSelType2 = "";
        switch (ddlDateSelType1.SelectedIndex)
        {
            case 0:
                _DateSelType1 = ">=";
                break;
            case 1:
                _DateSelType1 = "=";
                break;
            case 2:
                _DateSelType1 = "<=";
                break;
        }
        switch (ddlDateSelType2.SelectedIndex)
        {
            case 0:
                _DateSelType2 = ">=";
                break;
            case 1:
                _DateSelType2 = "=";
                break;
            case 2:
                _DateSelType2 = "<=";
                break;
        }
        string dateTime1Searching = (tbDatepicker1.Text.Equals("")) ? "" : " and " + ddlDateSelection1.SelectedItem + " " + _DateSelType1 + " '" + tbDatepicker1.Text + "' ";
        string dateTime2Searching = (tbDatepicker2.Text.Equals("")) ? "" : " and " + ddlDateSelection2.SelectedItem + " " + _DateSelType2 + " '" + tbDatepicker2.Text + "' ";

        // object condition signin (查詢條件註冊)
        List<string> sqlStrList = new List<string>();
        sqlStrList.Add(title);
        sqlStrList.Add(category);
        sqlStrList.Add(subCategroy);
        sqlStrList.Add(tagSql);
        sqlStrList.Add(dateTime1Searching);
        sqlStrList.Add(dateTime2Searching);

        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            // try and catch not using in asp web page.
            sqlcon.Open();

            _sql = "SELECT DISTINCT title, category, subCategory, path, filetype, CONVERT(VARCHAR, updatetime, 111) AS updateTime, updateuser ";
            _sql += "FROM " + _table + " AS A left join " + _tableTag + " AS B on A.id = B.id ";
            _sql += "WHERE 1 = 1 and enabled = 'Y '";

            // Condition (if using fuzzySearch, will disable other condition)
            if (cbFuzzySearching.Checked)
            {
                _sql += FuzzySql;
            }
            else
            {
                foreach (string s in sqlStrList)
                    _sql += s;
            }

            // DateSort
            _sql += dateSort;

            // Data Struction
            SqlDataAdapter da = new SqlDataAdapter(_sql, sqlcon); // To Get Data and fill into DataSet
            DataSet ds = new DataSet();
            da.Fill(ds);

            gvDataList.DataSource = ds;
            gvDataList.DataBind();

            sqlcon.Close();
        }

    }
    /*
    private void subCategory()
    {
        // define variable
        string category = (ddlCategory.SelectedValue.Equals("所有分類")) ? "" : " and category='" + ddlCategory.SelectedValue + "'";

        // setting sub category by which ddlCategory has been selected
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();
            string _defaultSql = "SELECT DISTINCT subCategory FROM " + _table;
            _defaultSql += " WHERE 1=1 AND enabled = 'Y' " + category;
            SqlDataAdapter da = new SqlDataAdapter(_defaultSql, sqlcon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ddlSubCategory.DataSource = ds;
            ddlSubCategory.DataTextField = "subCategory";
            ddlSubCategory.DataBind();

            ddlSubCategory.Items.Add("所有分類");
            ddlSubCategory.SelectedValue = "所有分類";
            // .DataValueField 指定編號給ASP使用 (ID) for Dev
            // .DataTextField  指定的是sql資料庫內文 (item) for User
            sqlcon.Close();
        }
    }
    */
    protected void gvDataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gvw = (GridView)sender;

        if (e.NewPageIndex < 0)
        {
            TextBox pageNum = (TextBox)gvw.BottomPagerRow.FindControl("txtNewPageIndex");
            int Pa = int.Parse(pageNum.Text);
            if (Pa <= 0)
            {
                gvw.PageIndex = 0;
            }
            else
            {
                gvw.PageIndex = Pa - 1;
            }
        }
        else
        {
            gvw.PageIndex = e.NewPageIndex;
        }
        DBINIT();
    }

    protected void gvDataList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    // 固定呼叫函式定義

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DBINIT();
    }

    protected void btnDefault_Click(object sender, EventArgs e)
    {
        tbTitle.Text = "";
        tbTag.Text = "";
        tbFuzzySearching.Text = "";
        cbFuzzySearching.Checked = false;
        ddlCategory.SelectedIndex = 0;
        ddlSubCategory.SelectedIndex = 0;
        ddlDateSort.SelectedIndex = 0;
        ddlDateSelection1.SelectedIndex = 0;
        ddlDateSelection2.SelectedIndex = 0;
        ddlDateSelType1.SelectedIndex = 0;
        ddlDateSelType2.SelectedIndex = 2;
        tbDatepicker1.Text = "";
        tbDatepicker2.Text = "";
    }
    protected void btnTagDialog_Click(object sender, EventArgs e)
    {

    }

    protected void btnInfomationSafe_Click(object sender, EventArgs e)
    {
        // default setting first
        //btnDefault_Click(this, null);
        // any other setting in here
        //if (ddlCategory.Items.FindByText("資訊安全") != null)
        //    ddlCategory.SelectedValue = "資訊安全";
        //Response.Redirect("./SAI_Knowledge.aspx?category=資訊安全");
        DBINIT();
    }
    protected void btnInstall_Click(object sender, EventArgs e)
    {
        // default setting first
        btnDefault_Click(this, null);
        // any other setting in here
        //if (ddlCategory.Items.FindByText("軟體安裝及操作") != null)
        //    ddlCategory.SelectedValue = "軟體安裝及操作";
        Response.Redirect("./SAI_Knowledge.aspx?category=軟體安裝及操作");
        DBINIT();
    }

    protected void btnSAP_Click(object sender, EventArgs e)
    {
        // default setting first
        btnDefault_Click(this, null);
        // any other setting in here
        //if (ddlCategory.Items.FindByText("SAP/ERP系統") != null)
        //    ddlCategory.SelectedValue = "SAP/ERP系統";
        Response.Redirect("./SAI_Knowledge.aspx?category=SAP/ERP系統");
        DBINIT();
    }

    protected void btnOffice_Click(object sender, EventArgs e)
    {
        // default setting first
        btnDefault_Click(this, null);
        // any other setting in here
        //if (ddlCategory.Items.FindByText("文書處理") != null)
        //    ddlCategory.SelectedValue = "文書處理";
        Response.Redirect("./SAI_Knowledge.aspx?category=文書處理");
        DBINIT();
    }

    protected void btnBPM_Click(object sender, EventArgs e)
    {
        // default setting first
        btnDefault_Click(this, null);
        // any other setting in here
        //if (ddlCategory.Items.FindByText("BPM") != null)
        //    ddlCategory.SelectedValue = "BPM";
        Response.Redirect("./SAI_Knowledge.aspx?category=BPM");
        DBINIT();
    }
    protected void btnEngineSortWare_Click(object sender, EventArgs e)
    {
        // default setting first
        btnDefault_Click(this, null);
        // any other setting in here
        //if (ddlCategory.Items.FindByText("工程軟體") != null)
        //    ddlCategory.SelectedValue = "工程軟體";
        Response.Redirect("./SAI_Knowledge.aspx?category=工程軟體");
        DBINIT();
    }
    protected void tbFuzzySearching_TextChanged(object sender, EventArgs e)
    {
        if (tbFuzzySearching.Text.Equals(""))
            cbFuzzySearching.Checked = false;
        else
            cbFuzzySearching.Checked = true;
    }


}