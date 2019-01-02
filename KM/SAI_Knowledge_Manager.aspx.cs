using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SAI_Knowledge_Manager : System.Web.UI.Page
{
    //TODO 快速啟用與停用的設計(GridView data for enabled setting)

    //string _con = "SERVER =192.168.1.130;DATABASE=SAEE;UID=saee;PWD=92vsk48mdf";
    string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE_KM"].ConnectionString;
    //string _table = "SAI_Knowledge";
    string _table = "SAI_KM";
    string _tableTag = "SAI_KM_TAG";
    string _tableManager = "SAI_KM_MANAGER";

    string _sql = "";

    // 歷程cookies
    //HttpCookie cookies_titleList;

    // 工號
    string USER_ID = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        //Session["USERID"] = Request["USERID"];

        // init bind data for selecting operator
        if (!IsPostBack)
        {
            tbTitle.Focus();
            
            //Session["USERID"] = "H7925";
            // Session 檢查
            SessionCheck();
			
			
            using (SqlConnection sqlcon = new SqlConnection(_con))
            {

                sqlcon.Open();
                // Setting Search Item Default (if any inneed, add here)
                string _defaultSql = "SELECT DISTINCT category FROM " + _table;
                SqlDataAdapter da = new SqlDataAdapter(_defaultSql, sqlcon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ddlCategory.DataSource = ds;
                ddlCategory.DataTextField = "category";
                ddlCategory.DataBind();

                ddlCategory.Items.Add("所有分類");
                ddlCategory.SelectedValue = "所有分類";

                // .DataValueField 指定編號給ASP使用 (ID) for Dev
                // .DataTextField  指定的是sql資料庫內文 (item) for User

                sqlcon.Close();
            }

            if (Request.Params["category"] != "")
            {
                if (ddlCategory.Items.FindByText(Request.Params["category"]) != null)
                    ddlCategory.SelectedValue = Request.Params["category"];
            }

            subCategory();
            //DBINIT();
        }
    }

    private void DBINIT()
    {
        // Control Main Function

        // Cookies
        /*
        if (!Request.Cookies["titleList"].Value.Contains(tbTitle.Text))
        {
            if (Request.Cookies["titleList"].Value.IndexOf(',') == -1)
                cookies_titleList.Value = tbTitle.Text;
            else
                cookies_titleList.Value += ", " + tbTitle.Text;
        }*/

        // initial variable
        _sql = "";

        // Define Search Variable String from WebPage
        // ddl=DropDownList, tb=TextBox

        string searchTitle = "";
        switch (ddlSearchTitle.SelectedValue)
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

        // 這個部分使用 and之後內容可能要以or的方式去撈 語意"為包含有"
        // 弱條件, 不用全符合
        // tag container

        string[] tag = tbTag.Text.Split(",;；|".ToArray());
        string tagSql = "";
        foreach (string s in tag)
        {
            if (!tagSql.Contains("and"))
            {
                tagSql += " and (tag like '%" + s.Trim(' ') + "%' ";
            }
            else
            {
                tagSql += " or tag like '%" + s.Trim(' ') + "%' ";
            }
        }
        tagSql += ")";
        if (tbTag.Text.Equals(""))
            tagSql = "";

        // FuzzySearch setting (if using fuzzySearch, will disable other condition)
        string FuzzySearching = "";
        if (cbFuzzySearching.Checked)
            FuzzySearching = (tbFuzzySearching.Text.Equals("")) ? "" : tbFuzzySearching.Text.Trim();
        string FuzzySql = " and (title like '%" + FuzzySearching + "%' or category like '%" + FuzzySearching + "%' or subCategory like '%" + FuzzySearching + "%' or tag like '%" + FuzzySearching + "%' )";

        if (tbDatepicker1.Text.Length < 10 && tbDatepicker1.Text != "" || tbDatepicker1.Text.Length > 10)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('日期輸入不符合規範');", true);
            return;
        }
        if (tbDatepicker2.Text.Length < 10 && tbDatepicker2.Text != "" || tbDatepicker2.Text.Length > 10)
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
        string dateTime1Searching = (tbDatepicker1.Text.Equals("")) ? "" : " and " + ddlDateSelection1.SelectedValue + " " + _DateSelType1 + " '" + tbDatepicker1.Text + "' ";
        string dateTime2Searching = (tbDatepicker2.Text.Equals("")) ? "" : " and " + ddlDateSelection2.SelectedValue + " " + _DateSelType2 + " '" + tbDatepicker2.Text + "' ";

        string enabledSearching = (ddlEnabled.SelectedValue.Equals("ALL")) ? "" : " and enabled = '" + ddlEnabled.SelectedValue + "' ";

        // object condition signin (查詢條件註冊)
        List<string> sqlStrList = new List<string>();
        sqlStrList.Add(title);
        sqlStrList.Add(category);
        sqlStrList.Add(subCategroy);
        sqlStrList.Add(tagSql);
        sqlStrList.Add(dateTime1Searching);
        sqlStrList.Add(dateTime2Searching);
        sqlStrList.Add(enabledSearching);

        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            // try and catch not using in asp web page.
            sqlcon.Open();

            _sql = "SELECT DISTINCT A.id, title, category, subCategory, enabled, path, filetype, CONVERT(VARCHAR, updatetime, 111) AS updateTime, updateuser ";
            _sql += "FROM " + _table + " AS A left join " + _tableTag + " AS B on A.id = B.id ";
            _sql += "WHERE 1 = 1 ";

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

    private void subCategory()
    {

        // define variable
        string category = (ddlCategory.SelectedValue.Equals("所有分類")) ? "" : " and category='" + ddlCategory.SelectedValue + "'";
        string s = Request.Form["ddlCategory"];

        // setting sub category by which ddlCategory has been selected
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();
            string _defaultSql = "SELECT DISTINCT subCategory FROM " + _table;
            _defaultSql += " WHERE 1=1 " + category;
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

    protected void gvDataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gvw = (GridView)sender;

        if (e.NewPageIndex < 0)
        {
            TextBox pageNumBottom = (TextBox)gvw.BottomPagerRow.FindControl("txtNewPageIndex");
            int Pa = int.Parse(pageNumBottom.Text);
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
        ddlCategory.SelectedValue = "所有分類";
        ddlSubCategory.SelectedValue = "所有分類";
        ddlDateSort.SelectedValue = "日期無排序";
        ddlDateSelection1.SelectedIndex = 0;
        ddlDateSelection2.SelectedIndex = 0;
        ddlDateSelType1.SelectedIndex = 0;
        ddlDateSelType2.SelectedIndex = 2;
        tbDatepicker1.Text = "";
        tbDatepicker2.Text = "";
        ddlEnabled.SelectedIndex = 0;
    }
    protected void btnTagDialog_Click(object sender, EventArgs e)
    {

    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        // this fucntion inneed "AutoPostBack" to work well. (add in ASP object tag)
        subCategory();
    }

    protected void tbFuzzySearching_TextChanged(object sender, EventArgs e)
    {
        if (tbFuzzySearching.Text.Equals(""))
            cbFuzzySearching.Checked = false;
        else
            cbFuzzySearching.Checked = true;
    }


    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("./SAI_Knowledge_Editor.aspx?op=new&id=" + Guid.NewGuid().ToString().ToUpper());
    }



    protected void btnExport_Click(object sender, EventArgs e)
    {
        string appPath = Request.PhysicalApplicationPath;
        string fileDir = appPath + "/export.csv";
        //System.IO.Directory.CreateDirectory(fileDir);
        DataSet ds = new DataSet();

        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            // try and catch not using in asp web page.
            sqlcon.Open();
            _sql += "SELECT KM.id, title,(SELECT cast(TAG AS NVARCHAR ) + ';' from " + _tableTag + " where ID = KM.ID FOR XML PATH('')) AS TAGS, category, subCategory, fileName, path, filetype, enabled, updateuser, CONVERT(VARCHAR, updatetime, 111) AS updateTime, creator,  CONVERT(VARCHAR, createdTime, 111) AS createdTime ";
            _sql += " FROM " + _table + " KM left join ";
            _sql += " (SELECT ID, left(T.tags_list, len(T.tags_list)-1) as tags ";
            _sql += " FROM ";
            _sql += " (SELECT ID, ";
            _sql += "	(SELECT cast(TAG AS NVARCHAR ) + ';' ";
            _sql += "	FROM " + _tableTag + " ";
            _sql += "	WHERE ID = KM.ID ";
            _sql += "	FOR XML PATH('')) as tags_list";
            _sql += " FROM " + _table + " KM ) T ) AS B on KM.id = b.id";
            _sql += " WHERE 1 = 1";


            // Data Struction
            SqlDataAdapter da = new SqlDataAdapter(_sql, sqlcon); // To Get Data and fill into DataSet
            da.Fill(ds);

            sqlcon.Close();
        }


        using (StreamWriter wr = new StreamWriter(fileDir, false, System.Text.Encoding.UTF8))
        {
            //標頭提供給IMPORTER, 若有更動要再換成此設定
            wr.WriteLine("id,tags,title,category,subCategory,fileName,fileType,path,enabled,creator,createdTime,updateUser,updateTime");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                List<string> list = new List<string>();

                string tag_handler = "";
                if (!dr["tags"].ToString().Equals(""))
                    tag_handler = dr["tags"].ToString().Substring(0, dr["tags"].ToString().Length - 1);
                else
                    tag_handler = " ";
                list.Add(dr["id"].ToString());
                list.Add(tag_handler);
                list.Add(dr["title"].ToString().Replace(",","，"));
                list.Add(dr["category"].ToString());
                list.Add(dr["subCategory"].ToString());
                list.Add(dr["fileName"].ToString().Replace(",", "，"));
                list.Add(dr["filetype"].ToString());
                list.Add(dr["path"].ToString().Replace(",", "，"));
                list.Add(dr["enabled"].ToString());
                list.Add(dr["creator"].ToString());
                list.Add(dr["createdTime"].ToString());
                list.Add(dr["updateuser"].ToString());
                list.Add(dr["updatetime"].ToString());

                string line = "";
                foreach (string s in list)
                    line += s + ",";
                line = line.Substring(0, line.Length - 1);

                wr.WriteLine(line);

            }
        }
        using (System.Net.WebClient wc = new System.Net.WebClient())
        {
            byte[] xfile = null;
            xfile = wc.DownloadData(fileDir);
            string xfileName = fileDir;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=export.csv");
            HttpContext.Current.Response.ContentType = "application/octet-stream"; //二進位方式
            HttpContext.Current.Response.BinaryWrite(xfile); //內容轉出作檔案下載
            HttpContext.Current.Response.End();
        }
    }

    private void SessionCheck()
    {
		
		//Session["USERID"] = Request["USERID"];
        if (Session["USERID"] != null)
        {
			//Session["USERID"] = Convert.ToString(System.Web.HttpContext.Current.Session["USERID"]);
            using (SqlConnection sqlcon = new SqlConnection(_con))
            {
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand("SELECT userid FROM " + _tableManager + " WHERE 1=1 ", sqlcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                bool check = false;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["userid"].ToString().Equals(Session["USERID"].ToString()))
                    {
                        check = true;
                        USER_ID = Session["USERID"].ToString().ToUpper();
                        labelUSER.Text = USER_ID + " 歡迎使用管理員工具！";
                    }
                }
                if (!check)
                {
                    USER_ID = Session["USERID"].ToString().ToUpper();
                    labelUSER.Text = USER_ID;
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "",
                        "alert('您的身分不具有管理權限！');window.location ='SAI_Knowledge.aspx';",
                        true);
                    return;
                }

                sqlcon.Close();
            }
        }
        else
        {
            labelUSER.Text = "NO USER";
            ScriptManager.RegisterStartupScript(this, this.GetType(),
            "",
            "alert('您的號已無效，煩請重新進入！');window.location ='SAI_Knowledge.aspx';",
            true);
            return;
        }
    }

}