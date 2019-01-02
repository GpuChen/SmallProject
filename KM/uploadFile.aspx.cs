using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class uploadFile : System.Web.UI.Page
{

    string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE_KM"].ConnectionString;
    string _table = "SAI_KM";

    List<string> uploadRule = new List<string>();
    string category = "";
    string subCategory = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        
        //string appPath = Request.PhysicalApplicationPath;
		string appPath = @"D:\KM_UPLOAD\";
        string FolderDir = "temp\\"; //主機存放檔案實體位置
        string savePath = "";

        category = Request.Params["category"];
        subCategory = Request.Params["subCategory"];
        if (FileUpload1.FileName.Equals("")) return;

        if (FileUpload1.HasFile)
        {
            if (FileUpload1.FileName.IndexOf(".") == -1)
            {
                Page.RegisterStartupScript("", "<script language='javascript'>window.alert('提醒：上傳內容必須是檔案');</script>");
                return;
            }
            if (!FileNameCheck(FileUpload1.FileName))
            {
                Page.RegisterStartupScript("", "<script language='javascript'>window.alert('提醒：上傳內容必須符合規範檔案');</script>");
                return;
            }
            if (!FileUploadCheck(FileUpload1.FileName))
            {
                // TODO 可以改成複寫
                Page.RegisterStartupScript("", "<script language='javascript'>window.alert('提醒：上傳檔案中有相同名稱，送出時將會覆蓋舊檔案');</script>");
                //return;
            }
            //savePath = appPath + FolderDir + category + @"\" + subCategory + @"\" + FileUpload1.FileName;
            savePath = appPath + FolderDir + FileUpload1.FileName;

            // temp file
            try
            {
                //if (!Directory.Exists(appPath + FolderDir + category + @"\" + subCategory))
                //{
                //    Directory.CreateDirectory(appPath + FolderDir + category + @"\" + subCategory);
                //}

                if (!Directory.Exists(appPath + FolderDir))
                {
                    Directory.CreateDirectory(appPath + FolderDir);
                }
                FileUpload1.SaveAs(savePath);
            }
            catch(Exception err)
            {
                Response.Write("<Script language='JavaScript'>alert('上傳失敗錯誤訊息:" +err.ToString() + "');</Script>");
                return;
            }
            //Response.Write("<Script language='JavaScript'>alert('成功上傳');</Script>");
        }

        string pageFolderDir = @"/uploads/files/";
        //string htmlPath = "http://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"];
        //       htmlPath += @"/KM/" + FolderDir + category + @"/" + subCategory + @"/" + FileUpload1.FileName;
        
		string htmlPath = "http://" + Request.Url.Host + ":" + Request.ServerVariables["SERVER_PORT"];
        //htmlPath += @"/human/KM/uploads/" + category + @"/" + subCategory + @"/" + FileUpload1.FileName;
        htmlPath += @"/human/KM" + pageFolderDir + FileUpload1.FileName;

        string jsStr = "";
        jsStr += "window.opener.document.getElementById('tbPath').value = '"+htmlPath+"' ;";
        jsStr += "window.opener.document.getElementById('tbUploadFileName').value = '" + FileUpload1.FileName+"' ;";
        jsStr += "window.opener.document.getElementById('cbUploadFile').checked = true ;";
        //jsStr += "window.opener.document.getElementById('labelUploadCheck').textContent = '" + "Uploaded" + "' ;";
        jsStr += "window.close();";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ret", jsStr, true);

    }


    private bool FileNameCheck(string name)
    {
        uploadRule.Add("pdf");
        uploadRule.Add("htm");
        uploadRule.Add("mht");
        uploadRule.Add("txt"); // for testing
        int uploadFiletype = name.Length - name.LastIndexOf(".");
        string checkVal = name.Substring(name.LastIndexOf(".") + 1, uploadFiletype - 1);
        foreach (string s in uploadRule)
        {
            if (checkVal.ToLower().Equals(s))
                return true;
        }

        return false;
    }
    private bool FileUploadCheck(string fileName)
    {
        DataSet ds = new DataSet();
        using (SqlConnection sqlcon = new SqlConnection(_con))
        {
            sqlcon.Open();
            string _Sql = "SELECT fileName FROM " + _table;
            //_Sql += " WHERE category ='" + category + "' and subCategory = '" + subCategory + "' and fileName is not null";
            _Sql += " WHERE fileName IS NOT null";
            SqlDataAdapter da = new SqlDataAdapter(_Sql, sqlcon);
            da.Fill(ds);
            sqlcon.Close();
        }
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (fileName.Equals(dr["fileName"].ToString()))
            {
                return false;
            }
        }
        return true;
    }

}