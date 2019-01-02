using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SAI_Knowledge_Editor : System.Web.UI.Page
{

    // 工號
    string USER_ID = "";

    // initial variable
    string _ID = string.Empty;
    string _title = string.Empty;
    string _category = string.Empty;
    string _subCategory = string.Empty;
    string _path = string.Empty;
    string _fileName = string.Empty;
    string _fileType = string.Empty;
    bool _enabled;
    string _creater = string.Empty;
    string _updateTime;
    string _updateUser = string.Empty;
    string _createdTime;
    string _creator = string.Empty;

    string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE_KM"].ConnectionString;

    //string _table = "SAI_Knowledge";
    //string _tagTable = "SAI_Knowledge_Tag";
    string _table = "SAI_KM";
    string _tableTag = "SAI_KM_TAG";
    string _tableManager = "SAI_KM_MANAGER";
    string _sql = "";

    List<string> tags = new List<string>();
    string _tag;

    List<string> uploadRule = new List<string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionCheck();

        if (!IsPostBack)
        {

            // define new creater if this is new object
            //  GetInfo ready for setting up
            if (Request["op"].Equals("new"))
            {

                labelOperation.Text = "新增";
                //_ID = Guid.NewGuid().ToString();
				_ID = Request["id"];
				tbID.Text = _ID.ToUpper();
				tbCreateTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                tbCreator.Text = USER_ID;

                rblObjectType.SelectedIndex = 0;
                tbPath.ReadOnly = true;
                tbPath.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");

                CategoryList();
                subCategory();

            }
            else
            {
                labelOperation.Text = "更新";
                _ID = Request["id"];
                tbID.Text = _ID.ToUpper();
                using (SqlConnection sqlcon = new SqlConnection(_con))
                {
                    // try and catch not using in asp web page.
                    sqlcon.Open();

                    _sql = "SELECT title, category, subCategory, fileName, path, fileType, enabled, creator, createdTime, updateTime, updateUser ";
                    //_sql = "SELECT title, category, subCategory, fileName, path, fileType, enabled, updateTime, updateUser ";
                    _sql += "FROM " + _table + " ";
                    _sql += "WHERE 1=1 and id='" + _ID + "'";

                    // Data Struction
                    SqlDataAdapter da = new SqlDataAdapter(_sql, sqlcon); // To Get Data and fill into DataSet
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        _title = dr["title"].ToString();
                        _category = dr["category"].ToString();
                        _subCategory = dr["subCategory"].ToString();
                        _path = dr["path"].ToString();
                        _fileName = dr["fileName"].ToString();
                        _fileType = dr["fileType"].ToString();
                        _enabled = (dr["enabled"].ToString().Equals("Y") || dr["enabled"].ToString().Equals("y")) ? true : false;
                        _updateTime = dr["updateTime"].ToString();
                        _updateUser = dr["updateUser"].ToString();
                        _creator = dr["creator"].ToString();
                        _createdTime = dr["createdTime"].ToString();
                    }

                    // handle Tag String
                    _sql = "SELECT tag FROM " + _tableTag + " WHERE 1=1 and id='" + _ID + "'";
                    SqlDataAdapter da_tag = new SqlDataAdapter(_sql, sqlcon);
                    DataSet ds_tag = new DataSet();
                    da_tag.Fill(ds_tag);
                    foreach (DataRow dr in ds_tag.Tables[0].Rows)
                    {
                        tags.Add(dr["tag"].ToString());
                    }
                    sqlcon.Close();
                    CategoryList(); // reset and get category
                }

                // bind data
                tbTitle.Text = _title;
                ddlCategory.SelectedValue = _category;
                subCategory(); // reset and get subCategory
                ddlSubCategory.SelectedValue = _subCategory;
                //tbFileType.Text = _fileType;
                tbPath.Text = _path;
                if (_fileType.Equals("HTML") || _fileType.Equals("html"))
                    rblObjectType.Items[1].Selected = true;
                else
                    rblObjectType.Items[0].Selected = true;

                cbEnabled.Checked = _enabled;
                tbUploadFileName.Text = _fileName;
                tbUpdateTime.Text = DateTime.Parse(_updateTime).ToString("yyyy/MM/dd hh:mm:ss");
                tbUpdateUser.Text = _updateUser;
                tbCreator.Text = _creator;
                tbCreateTime.Text = DateTime.Parse(_createdTime).ToString("yyyy/MM/dd hh:mm:ss");

                if (!tags.Count.Equals(0))
                {
                    foreach (string s in tags)
                    {
                        _tag += s.Trim() + ",";
                    }
                    _tag = _tag.Substring(0, _tag.Length - 1);
                    tbTag.Text = _tag;
                }

                // UI setting, base on which method is using (for create or update)
                //rblObjectType.Enabled = false; //限制選擇
                //ddlCategory.Enabled = false;
                //ddlSubCategory.Enabled = false;
                //tbNewCategory.Enabled = false;
                //tbNewSubCategory.Enabled = false;

                tbCreator.ReadOnly = true;
                tbCreator.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");

                if (rblObjectType.SelectedIndex.Equals(1))
                {
                    btnUploadFile.Enabled = false;
                }

            } // end of else

        }
    }



    protected void btnSubmit_Click(object sender, EventArgs e)
    {


        //if (rblObjectType.SelectedIndex.Equals(0) && !labelOperation.Text.Equals("更新"))
        if (rblObjectType.SelectedIndex.Equals(0) && cbUploadFile.Checked)
        {
            //string appPath = Request.PhysicalApplicationPath;
            //string FolderDir = "uploads\\temp\\";
            string savePathDir = @"files\";
			string appPath = @"D:\KM_UPLOAD\";
			string FolderDir = "temp\\"; //主機存放檔案實體位置
			
            string fileCategory = (tbNewCategory.Text.Equals("")) ? ddlCategory.Text : tbNewCategory.Text;
            string fileSubCategory = (tbNewSubCategory.Text.Equals("")) ? ddlSubCategory.Text : tbNewSubCategory.Text;

            //string tempFilePathDir = appPath + FolderDir + fileCategory + @"\" + fileSubCategory + @"\" + Request.Form["tbUploadFileName"];
            //string saveFilePathDir = appPath + savePathDir + fileCategory + @"\" + fileSubCategory + @"\" + Request.Form["tbUploadFileName"];
            string tempFilePathDir = appPath + FolderDir + Request.Form["tbUploadFileName"];
            string saveFilePathDir = appPath + savePathDir + Request.Form["tbUploadFileName"];

            if (System.IO.File.Exists(tempFilePathDir))
            {
                //if (!Directory.Exists(appPath + savePathDir + fileCategory + @"\" + fileSubCategory))
                //{
                //    Directory.CreateDirectory(appPath + savePathDir + fileCategory + @"\" + fileSubCategory);
                //}
                if (!Directory.Exists(appPath + savePathDir))
                {
                    Directory.CreateDirectory(appPath + savePathDir);
                }

                if (System.IO.File.Exists(saveFilePathDir))
                {
                    Response.Write("<Script language='JavaScript'>alert('覆蓋舊檔案');</Script>");
                }
                File.Copy(tempFilePathDir, saveFilePathDir, true);
            }
            else
            {
                Response.Write("<Script language='JavaScript'>alert('找不到檔案');</Script>");
                return;
            }
        }

        // SQL 處理
        // Update 處理 or Insert 處理
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            // try and catch not using in asp web page.
            sqlcon.Open();
						
            string setList = "";
            string _updateSql = "";
            List<string> items = new List<string>();
            items.Add("title");
            items.Add("category");
            items.Add("subCategory");
            items.Add("path");
            items.Add("fileName");
            items.Add("fileType");
            items.Add("enabled");
            items.Add("updateTime");
            items.Add("updateUser");
			
            foreach (string s in items)
            {
                setList += "," + s + "=@" + s + " ";
            }
            setList = setList.Substring(1, setList.Length - 1);
            _updateSql += "UPDATE " + _table + " SET " + setList + " ";
            _updateSql += "WHERE id=@id";

            SqlCommand sqlUpdate = new SqlCommand(_updateSql, sqlcon);
			if (Request["op"].Equals("new"))
			{
				//tbID.Text = Guid.NewGuid().ToString().ToUpper();
				sqlUpdate.Parameters.AddWithValue("@id", Request.Form["tbID"]);
			}
			else
			{
				sqlUpdate.Parameters.AddWithValue("@id", Request.Form["tbID"]);		
			}
            sqlUpdate.Parameters.AddWithValue("@title", tbTitle.Text);

            // 新舊分類的使用檢查
            if (tbNewCategory.Text.Equals(""))
                sqlUpdate.Parameters.AddWithValue("@category", ddlCategory.SelectedValue);
            else
                sqlUpdate.Parameters.AddWithValue("@category", tbNewCategory.Text);
            if (tbNewSubCategory.Text.Equals(""))
                sqlUpdate.Parameters.AddWithValue("@subCategory", ddlSubCategory.SelectedValue);
            else
                sqlUpdate.Parameters.AddWithValue("@subCategory", tbNewSubCategory.Text);

            // 注意這裡路徑是提供給連結！
            sqlUpdate.Parameters.AddWithValue("@path", tbPath.Text);
            // 檔案格式檢查 
            if (rblObjectType.Items[1].Selected)
            {
                sqlUpdate.Parameters.AddWithValue("@fileType", "html");
                sqlUpdate.Parameters.AddWithValue("@fileName", Request.Form["tbUploadFileName"]);
            }
            else
            {
                sqlUpdate.Parameters.AddWithValue("@fileType", "file");
                sqlUpdate.Parameters.AddWithValue("@fileName", Request.Form["tbUploadFileName"]);
            }

            sqlUpdate.Parameters.AddWithValue("@enabled", (cbEnabled.Checked) ? "Y" : "N");
            sqlUpdate.Parameters.AddWithValue("@updateTime", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            sqlUpdate.Parameters.AddWithValue("@updateUser", Session["USERID"].ToString()); //todo 更新時需要得知使用者

            int updateRowCheck = sqlUpdate.ExecuteNonQuery();

            if (updateRowCheck.Equals(0)) // 新增資料
            {
				
                string setListValue = "";
                setList = "";
                foreach (string s in items)
                {
                    setList += s + ",";
                    setListValue += "@" + s + ",";
                }
                setList = setList.Substring(0, setList.Length - 1);
                setListValue = setListValue.Substring(0, setListValue.Length - 1);

                string _InsertSql = "INSERT INTO " + _table + " (id, creator, createdTime," + setList + ") ";
                _InsertSql += "VALUES (@id, @creator, @createdTime, " + setListValue + ")";
                SqlCommand sqlInsert = new SqlCommand(_InsertSql, sqlcon);
                sqlInsert.Parameters.AddWithValue("@id", Request["tbID"]);
                sqlInsert.Parameters.AddWithValue("@title", tbTitle.Text);

                // 新舊分類的使用檢查
                if (tbNewCategory.Text.Equals(""))
                    sqlInsert.Parameters.AddWithValue("@category", ddlCategory.SelectedValue);
                else
                    sqlInsert.Parameters.AddWithValue("@category", tbNewCategory.Text);
                if (tbNewSubCategory.Text.Equals(""))
                    sqlInsert.Parameters.AddWithValue("@subCategory", ddlSubCategory.SelectedValue);
                else
                    sqlInsert.Parameters.AddWithValue("@subCategory", tbNewSubCategory.Text);

                sqlInsert.Parameters.AddWithValue("@path", Request.Form["tbPath"]); //readonly 狀態會取不到Control
                if (rblObjectType.Items[1].Selected)
                {
                    sqlInsert.Parameters.AddWithValue("@fileName", "");
                    sqlInsert.Parameters.AddWithValue("@fileType", "html"); // TODO
                }
                else
                {
                    sqlInsert.Parameters.AddWithValue("@fileName", Request.Form["tbUploadFileName"]);
                    sqlInsert.Parameters.AddWithValue("@fileType", "file"); // TODO
                }
                sqlInsert.Parameters.AddWithValue("@enabled", (cbEnabled.Checked) ? "Y" : "N");
                sqlInsert.Parameters.AddWithValue("@updateTime", Request.Form["tbCreateTime"]);
                sqlInsert.Parameters.AddWithValue("@updateUser", Request.Form["tbCreator"]);
                sqlInsert.Parameters.AddWithValue("@createdTime", Request.Form["tbCreateTime"]);
                sqlInsert.Parameters.AddWithValue("@creator", tbCreator.Text);
                sqlInsert.ExecuteNonQuery();
				
				// new Tag handler
				
				
				string[] temp_new_tags = Request.Form["tbTag"].ToString().Split(",;；|&".ToArray());
                foreach (string s in temp_new_tags)
				{
					if (s.Equals(",") || s.Equals("") || s.Equals(" ")) continue;
					tags.Add(s.Trim());
				}
				foreach (string s in tags)
				{
					string _InsertTagSql = "INSERT INTO " + _tableTag + " (id, tag) VALUES (@id, @tag)";
					SqlCommand sqlTagInsert = new SqlCommand(_InsertTagSql, sqlcon);
					sqlTagInsert.Parameters.AddWithValue("@id", Request.Form["tbID"]);
					sqlTagInsert.Parameters.AddWithValue("@tag", s);
					sqlTagInsert.ExecuteNonQuery();
				}
				ScriptManager.RegisterStartupScript(this, this.GetType(),
				"",
				"alert('Insert OK');window.location='SAI_Knowledge_Manager.aspx';", true);
				return;
				
            }
			
            // tag handler
            tags.Clear();
            string[] temp_tags = Request.Form["tbTag"].ToString().Split(",;；|&".ToArray());
            foreach (string s in temp_tags)
            {
                if (s.Equals(",") || s.Equals("")) continue;
                tags.Add(s.Trim());
            }
            // reset tag to fill in
            string _resetSql = "DELETE FROM " + _tableTag + " WHERE id=@id";
            SqlCommand sqlTagReset = new SqlCommand(_resetSql, sqlcon);
            sqlTagReset.Parameters.AddWithValue("@id", Request.Form["tbID"]);
            sqlTagReset.ExecuteNonQuery();
            foreach (string s in tags)
            {
                string _InsertTagSql = "INSERT INTO " + _tableTag + " (id, tag) VALUES (@id, @tag)";
                SqlCommand sqlTagInsert = new SqlCommand(_InsertTagSql, sqlcon);
                sqlTagInsert.Parameters.AddWithValue("@id", Request.Form["tbID"]);
                sqlTagInsert.Parameters.AddWithValue("@tag", s);
                sqlTagInsert.ExecuteNonQuery();
			}
			
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(),
            "",
            "alert('Update OK');window.location='SAI_Knowledge_Manager.aspx';", true);
			
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/KM/SAI_Knowledge_Manager.aspx?USERID=" + USER_ID, false);
    }


    private void CategoryList()
    {
        // setting sub category by which ddlCategory has been selected
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();

            string _defaultSql = "SELECT DISTINCT category FROM " + _table;
            SqlDataAdapter da = new SqlDataAdapter(_defaultSql, sqlcon);
            DataSet dsCategory = new DataSet();
            da.Fill(dsCategory);
            ddlCategory.DataSource = dsCategory;
            ddlCategory.DataTextField = "category";
            ddlCategory.DataBind();

        }
    }

    private void subCategory()
    {
        // define variable
        string category = " and category='" + ddlCategory.SelectedValue + "'";
        // setting sub category by which ddlCategory has been selected
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();
            string _defaultSql = "SELECT DISTINCT subCategory FROM " + _table;
            _defaultSql += " WHERE 1=1" + category;
            SqlDataAdapter da = new SqlDataAdapter(_defaultSql, sqlcon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ddlSubCategory.DataSource = ds;
            ddlSubCategory.DataTextField = "subCategory";
            ddlSubCategory.DataBind();

            sqlcon.Close();
        }
    }

    protected void ddlCategory_TextChanged(object sender, EventArgs e)
    {
        subCategory();
    }

    protected void rblObjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblObjectType.SelectedIndex.Equals(0))
        {
            btnUploadFile.Enabled = true;
            tbPath.ReadOnly = true;
            tbPath.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
        }
        else if (rblObjectType.SelectedIndex.Equals(1))
        {
            btnUploadFile.Enabled = false;
            tbPath.ReadOnly = false;
            tbPath.BackColor = System.Drawing.Color.White;
        }
    }

    private void SessionCheck()
    {
        if (Session["USERID"] != null)
        {
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
                        USER_ID = Session["USERID"].ToString();
                    }
                }
                if (!check)
                {
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
            ScriptManager.RegisterStartupScript(this, this.GetType(),
            "",
            "alert('您的號已無效，煩請重新進入！');window.location ='SAI_Knowledge.aspx';",
            true);
            return;
        }
    }
}