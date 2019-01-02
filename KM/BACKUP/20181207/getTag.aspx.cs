using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class getTag : System.Web.UI.Page
{

    string _tableTag = "SAI_KM_TAG";

    //string _con = "SERVER =192.168.1.129;DATABASE=SAEE_QAS;UID=saee;PWD=92vsk48mdf";
	
    string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE_QAS"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {

            sqlcon.Open();
            // Setting Search Item Default (if any inneed, add here)
            string _defaultSql = "SELECT DISTINCT tag FROM "+ _tableTag + " ORDER BY tag ASC";
            SqlDataAdapter da = new SqlDataAdapter(_defaultSql, sqlcon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            CheckBoxList1.DataSource = ds;
            CheckBoxList1.DataTextField = "tag";
            CheckBoxList1.DataBind();
            sqlcon.Close();
        }
    }
}