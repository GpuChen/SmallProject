using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// WebService 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    //字串字典 這個類 麼用過 看別人是這麼寫的 
    //也可以這麼用：string [] strValues=knownCategoryValues.Split(‘:’,’;’); 
    // 然後取值： strValues[0]是 name strValues[1]是value吧

    [WebMethod]
    public CascadingDropDownNameValue[] GetSubCategoryByMain(string knownCategoryValues, string category)
    {
        System.Collections.Specialized.StringDictionary kv =
            CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        System.Collections.Generic.List<AjaxControlToolkit.CascadingDropDownNameValue> list =
            new System.Collections.Generic.List<CascadingDropDownNameValue>();

        string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE"].ConnectionString;
        string _table = "SAI_KM";
        list.Add(new AjaxControlToolkit.CascadingDropDownNameValue("所有分類", "所有分類"));

        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            string val = (kv["MainCategory"].Equals("所有分類")) ? "" : "AND category = '" + kv["MainCategory"] + "'";
            sqlcon.Open();
            string _defaultSql = "SELECT DISTINCT subCategory FROM " + _table;
             _defaultSql += " WHERE 1=1 AND enabled = 'Y' " + val; //+ iCategory;
            SqlDataAdapter da = new SqlDataAdapter(_defaultSql, sqlcon);
            DataSet ds = new DataSet();
            da.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string item = dr["subCategory"].ToString();
                list.Add(new AjaxControlToolkit.CascadingDropDownNameValue(item, item));
            }
            sqlcon.Close();
            
        }

        return list.ToArray();
    }


    [WebMethod]
    public CascadingDropDownNameValue[] GetMainCategory(string knownCategoryValues, string category)
    {
        System.Collections.Specialized.StringDictionary kv =
            CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        System.Collections.Generic.List<AjaxControlToolkit.CascadingDropDownNameValue> list =
            new System.Collections.Generic.List<CascadingDropDownNameValue>();

        string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE"].ConnectionString;
        string _table = "SAI_KM";
        list.Add(new AjaxControlToolkit.CascadingDropDownNameValue("所有分類", "所有分類"));

        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();
            // Setting Search Item Default (if any inneed, add here)
            string _defaultSql = "SELECT DISTINCT category FROM " + _table;
            _defaultSql += " WHERE 1=1 AND enabled = 'Y'";
            SqlDataAdapter da = new SqlDataAdapter(_defaultSql, sqlcon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string item = dr["Category"].ToString();
                list.Add(new AjaxControlToolkit.CascadingDropDownNameValue(item, item));
            }
            sqlcon.Close();
        }
        return list.ToArray();
    }


    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

}
