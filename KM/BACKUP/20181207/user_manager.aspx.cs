using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_manager : System.Web.UI.Page
{
    string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE_QAS"].ConnectionString;
    string _table = "SAI_KM_MANAGER";

    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["USERID"] = Request["USERID"];
        //Session["USERID"] = "h7925";
        SessionCheck();
        DBINIT();

    }

    private void DBINIT()
    {
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT userid FROM " + _table + " WHERE 1=1", sqlcon);
            DataSet ds = new DataSet();
            da.Fill(ds);

            GridView1.DataSource = ds;
            GridView1.DataBind();

            sqlcon.Close();
        }
    }
    private void SessionCheck()
    {
        if (Session["USERID"] != null)
        {
            using (SqlConnection sqlcon = new SqlConnection(_con))
            {
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand("SELECT userid FROM " + _table + " WHERE 1=1 ", sqlcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                bool check = false;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["userid"].ToString().Equals(Session["USERID"].ToString()))
                    {
                        check = true;
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

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();

            string newUserid = (TextBox1.Text.Equals("")) ? "" : TextBox1.Text;
            string _sql = "INSERT INTO " + _table + " (userid) VALUES ('" + newUserid.ToUpper() + "')";
            SqlCommand insertSQL = new SqlCommand(_sql, sqlcon);
            insertSQL.ExecuteNonQuery();
            sqlcon.Close();
        }

        DBINIT();
    }
    protected void btnDelChose_Click(object sender, EventArgs e)
    {
		btnDelete(Request.Form["TextBox1"]);
    }
	
    private void btnDelete(string id)
    {
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();
            string _sql = "DELETE " + _table + " WHERE userid ='" + id.ToUpper() + "'";
            SqlCommand deleteSQL = new SqlCommand(_sql, sqlcon);
            deleteSQL.ExecuteNonQuery();
            sqlcon.Close();
        }
        DBINIT();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "btnDelete")
        {
            if (e.CommandArgument.ToString().Equals("H7925")) return;
            btnDelete(e.CommandArgument.ToString());
        }

    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        DBINIT();

    }
}