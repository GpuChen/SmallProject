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

        string FuzzySearching = "";

        //if (cbFuzzySearching.Checked)
        FuzzySearching = (tbFuzzySearching.Text.Equals("")) ? "" : tbFuzzySearching.Text.Trim();
        //string FuzzySql = "and (title like '%" + FuzzySearching + "%' or category like '%" + FuzzySearching + "%' or subCategory like '%" + FuzzySearching + "%' or tag like '%" + FuzzySearching + "%' )";

        string[] Fuzzys = tbFuzzySearching.Text.Split(",".ToArray());
        string FuzzySql = "";
        foreach (string s in Fuzzys)
        {
            if (!FuzzySql.Contains("and"))
            {
                FuzzySql += "and (title like '%" + s + "%' or category like '%" + s + "%' or subCategory like '%" + s + "%' or tag like '%" + s + "%' ";
            }
            else
            {
                FuzzySql += "or title like '%" + s + "%' or category like '%" + s + "%' or subCategory like '%" + s + "%' or tag like '%" + s + "%' ";
            }
        }
        FuzzySql += ")";
        if (tbFuzzySearching.Text.Equals(""))
            FuzzySql = "";

        // object condition signin (查詢條件註冊)
        List<string> sqlStrList = new List<string>();
        sqlStrList.Add(title);
        sqlStrList.Add(category);
        sqlStrList.Add(subCategroy);
        //sqlStrList.Add(tagSql);
        //sqlStrList.Add(dateTime1Searching);
        //sqlStrList.Add(dateTime2Searching);

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
    protected void btnFuzzySearch_Click(object sender, EventArgs e)
    {
        DBINIT();
    }

    protected void btnDefault_Click(object sender, EventArgs e)
    {
        tbTitle.Text = "";
        //tbTag.Text = "";
        tbFuzzySearching.Text = "";
        //cbFuzzySearching.Checked = false;
        ddlCategory.SelectedIndex = 0;
        ddlSubCategory.SelectedIndex = 0;
        ddlDateSort.SelectedIndex = 0;
        //ddlDateSelection1.SelectedIndex = 0;
        //ddlDateSelection2.SelectedIndex = 0;
        //ddlDateSelType1.SelectedIndex = 0;
        //ddlDateSelType2.SelectedIndex = 2;
        //tbDatepicker1.Text = "";
        //tbDatepicker2.Text = "";
    }

}