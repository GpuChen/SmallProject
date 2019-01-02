using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Principal;

namespace AluminiumParser
{
    class Program
    {
        //第一參數:標籤使用

        static void Main(string[] args)
        {

            string tag = "";
            if (args != null && args.Length > 0)
            {
                tag = args[0].ToString();
            }
            else
            {
                tag = "";
            }

            string url = "https://www.lme.com/en-GB/Metals/Non-ferrous/Aluminium#tabIndex=0";

            WebClient web = new WebClient();
            HtmlDocument doc = new HtmlDocument(); //HAP
            string _con = ConfigurationManager.ConnectionStrings["SAEE"].ConnectionString;
            string _table = "LME_AL";
            string _tableLog = "SAI_DATACOLLECTION_LOG";

            web.Headers.Add(HttpRequestHeader.ContentType, "text/html; charset=utf-8");
            //web.Headers.Add(HttpRequestHeader.Cookie, "__language=__currentLanguage=en-GB; expires=Tue, 19-Dec-2028 00:00:00 GMT; path=/");
            web.Headers.Add(HttpRequestHeader.Cookie, "__language=__currentLanguage=en-GB; path=/");

            using (MemoryStream ms = new MemoryStream(web.DownloadData(url)))
            {
                doc.Load(ms, Encoding.UTF8);
                ms.Close();
            }
            
            HtmlNodeCollection validDate = doc.DocumentNode.SelectNodes(@"//div[@class='delayed-date left ']");
            //HtmlNodeCollection nodeData = doc.DocumentNode.SelectNodes(@"//div[@id='module-63']/table/tbody");
            //HtmlNodeCollection nodeData = doc.DocumentNode.SelectNodes(@"//div/div[1]/div[2]/table/tbody/tr/td");
            HtmlNodeCollection nodeData = doc.DocumentNode.SelectNodes(@"//div[@class='table-wrapper'][1]//tr//td");

            //LME_AL

            /*
            foreach (HtmlNode node in validDate)
            {
                Console.WriteLine(node.InnerText.Trim());
            }
            //內文檢視
            for (int i = 0; i <= 14; i++)
            {
                nodeData[i].InnerText.Trim();
                Console.WriteLine(nodeData[i].InnerText.Trim());
            }
            */

            try
            {
                string date = validDate[0].InnerText.Trim();
                Aluminium alum = new Aluminium();
                alum.valid_Date = DateTime.Parse(date.Substring(date.IndexOf("for ") + 3).Trim());
                alum.collected_Date = DateTime.Now;
                alum.cash_BID = decimal.Parse(nodeData[1].InnerText.Trim());
                alum.cash_OFFER = decimal.Parse(nodeData[2].InnerText.Trim());
                alum.three_mouths_BID = decimal.Parse(nodeData[4].InnerText.Trim());
                alum.three_mouths_OFFER = decimal.Parse(nodeData[5].InnerText.Trim());
                alum.Dec19_BID = decimal.Parse(nodeData[7].InnerText.Trim());
                alum.Dec19_OFFER = decimal.Parse(nodeData[8].InnerText.Trim());
                alum.Dec20_BID = decimal.Parse(nodeData[10].InnerText.Trim());
                alum.Dec20_OFFER = decimal.Parse(nodeData[11].InnerText.Trim());
                alum.Dec21_BID = decimal.Parse(nodeData[13].InnerText.Trim());
                alum.Dec21_OFFER = decimal.Parse(nodeData[14].InnerText.Trim());

                List<string> cmdlist = new List<string>();
                cmdlist.Add("valid_Date");
                cmdlist.Add("collected_Date");
                cmdlist.Add("cash_BID");
                cmdlist.Add("cash_OFFER");
                cmdlist.Add("three_mouths_BID");
                cmdlist.Add("three_mouths_OFFER");
                cmdlist.Add("Dec19_BID");
                cmdlist.Add("Dec19_OFFER");
                cmdlist.Add("Dec20_BID");
                cmdlist.Add("Dec20_OFFER");
                cmdlist.Add("Dec21_BID");
                cmdlist.Add("Dec21_OFFER");

                using (SqlConnection sqlcon = new SqlConnection(_con))
                {
                    sqlcon.Open();

                    List<string> temp = cmdlist;
                    //temp.Remove("valid_Date");

                    string _sql = "UPDATE " + _table + " SET " + CommandsGenerator(temp)[2] + " WHERE valid_Date=@valid_Date ";
                    int check = sqlcon.Execute(_sql, alum);
                    if (check.Equals(0))
                    {
                        _sql = "INSERT INTO " + _table + " ( " + CommandsGenerator(cmdlist)[0] + " ) VALUES ( " + CommandsGenerator(cmdlist)[1] + " ) ";
                        sqlcon.Execute(_sql, alum);
                        log("SUCCESS_INSERT", alum.ToString(), "鋁價", "", tag);
                    }
                    else
                    {
                        log("SUCCESS_UPDATE", alum.ToString(), "鋁價", "", tag);
                    }

                }
            }
            catch (SqlException err)
            {
                log("ERR_SQL", err.Message, "鋁價", err.ToString(), tag);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
                // 寫錯誤資料
                log("ERR_CSHARP", err.Message, "鋁價", err.ToString(), tag);
            }

            //Console.Read();
        }

        public static void log(string type, string param, string rmk, string memo, string args)
        {
            string _con = ConfigurationManager.ConnectionStrings["SAEE"].ConnectionString;
            string _tableLog = "SAI_DATACOLLECTION_LOG";

            using (SqlConnection sqlcon = new SqlConnection(_con))
            {
                sqlcon.Open();
                sqlcon.Execute("INSERT INTO " + _tableLog
                    + " (REF_KEY, TASK_USER, LOG_REF_TYPE, REF_PARAM, CREATETIME, RMK, REF_MEMO, ARGS) "
                    + "VALUES ('" + Guid.NewGuid().ToString()
                    + "', '" + WindowsIdentity.GetCurrent().Name + ":" + GetLocalIPAddress()
                    + "', '" + type
                    + "', '" + param
                    + "', '" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                    + "', '" + rmk
                    + "', '" + memo
                    + "', '" + args + "') ");
            }
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string[] CommandsGenerator(List<string> list)
        {
            string setList = "";
            string setListValue = "";
            string setListUpdateValue = "";
            foreach (string s in list)
            {
                setList += s + ",";
                setListValue += "@" + s + ",";
                setListUpdateValue += s + "=@" + s + ",";
            }
            setList = setList.Substring(0, setList.Length - 1);
            setListValue = setListValue.Substring(0, setListValue.Length - 1);
            setListUpdateValue = setListUpdateValue.Substring(0, setListUpdateValue.Length - 1);

            string[] items = { setList, setListValue, setListUpdateValue };
            return items;
        }

    }
    class Aluminium
    {
        public DateTime valid_Date { get; set; }
        public DateTime collected_Date { get; set; }
        public decimal cash_BID { get; set; }
        public decimal three_mouths_BID { get; set; }
        public decimal Dec19_BID { get; set; }
        public decimal Dec20_BID { get; set; }
        public decimal Dec21_BID { get; set; }
        public decimal cash_OFFER { get; set; }
        public decimal three_mouths_OFFER { get; set; }
        public decimal Dec19_OFFER { get; set; }
        public decimal Dec20_OFFER { get; set; }
        public decimal Dec21_OFFER { get; set; }


        override public string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("valid_Date:" + valid_Date.ToString());
            str.Append(", collected_Date:" + collected_Date.ToString());


            return str.ToString();
        }

    }

}
