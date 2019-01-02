using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class getTag : System.Web.UI.Page
{

    string _table = "SAI_KM";
    string _tableTag = "SAI_KM_TAG";
    string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE_KM"].ConnectionString;

    string content = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
                sqlcon.Close();
                ddlCategory_SelectedIndexChanged(this, null);
            }
            string enabled = (Request["enabled"].ToString() == "Y") ? "Y" : "N";
            DBINIT("category=&subcategory=&enabled="+enabled);
        }

    }

    private void DBINIT(string query)
    {
        string _request = "http://sai-rpt01/KMAPI/api/KM/tag?" + query;
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_request);
        request.Method = "GET";
        request.ContentType = "application/json; charset=UTF-8";
        //request.Timeout = 30000;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            StreamReader sr = new StreamReader((Stream)response.GetResponseStream());
            content = sr.ReadToEnd();
        }
        //TextBox1.Text = content;
        DataSet tags = JsonConvert.DeserializeObject<DataSet>("{\"Table\":" + content + "}");
        ;
        CheckBoxList1.DataSource = tags.Tables[0];
        CheckBoxList1.DataTextField = "tag";
        CheckBoxList1.DataBind();

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string cate = (ddlCategory.SelectedValue.ToString().Equals("所有分類")) ? "" : ddlCategory.SelectedValue.ToString();
        string subcate = (ddlSubCategory.SelectedValue.ToString().Equals("所有分類")) ? "" : ddlSubCategory.SelectedValue.ToString();
        string enabled = (Request["enabled"].ToString() == "Y") ? "Y" : "N";
        DBINIT("category=" + cate + "&subcategory=" + subcate + "&enabled="+enabled);
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
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
}