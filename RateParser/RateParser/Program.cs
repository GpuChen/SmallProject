using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Dapper;
using System.Configuration;
using System.Security.Principal;

namespace RateParse
{
    class Program
    {
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

            string constr = "SERVER =192.168.1.130;DATABASE=SAEE;UID=-;PWD=-";
            string content;

            // 讀檔方式取JSON
            /*
            string s = "";
            using (StreamReader sr = new StreamReader("JArray.json"))
            {
                s = sr.ReadToEnd();
                sr.Close();
            }
            */
            // 台銀取CSV, 文字字串處理後轉換JSON
            // 線上抓檔案存資料庫的方法
            // Stream 與 Stream Reader
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string filename = "";
            string fileContent = "";
            using (WebClient wc = new WebClient())
            {
                using (Stream stream = wc.OpenRead("https://rate.bot.com.tw/xrt/flcsv/0/day"))
                {
                    filename = wc.ResponseHeaders["content-disposition"];
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        fileContent = sr.ReadToEnd();
                        sr.Close();
                    }
                    stream.Close();
                }
            }
            // 檔案日期處理 (字符抓剛好的, 未來可能有問題) 
            string temp = filename.Substring(filename.IndexOf('@') + 1, 12);
            string FileDate = temp.Substring(0, 4) + "/" + temp.Substring(4, 2) + "/" + temp.Substring(6, 2) +
                " " + temp.Substring(8, 2) + ":" + temp.Substring(10, 2) + ":00";
            //Console.WriteLine(FileDate); //讀測試文字


            // 只有內容的存取
            //string curent = SAICommonLib.HTTPLib.DoHTTPGet("https://rate.bot.com.tw/xrt/flcsv/0/day");
            //string[] lines = curent.Split("\r\n".ToArray()); // 行列處理

            JArray bankCurrency = new JArray();
            string[] lines = fileContent.Split("\r\n".ToArray()); // 行列處理

            lines[0] = ""; // 標題行列前處理
            foreach (string line in lines)
            {
                if (line == "") continue;
                string[] items = line.Split(',');
                JObject Currency = new JObject();
                Currency.Add("幣別", items[0].ToString());
                Currency.Add("買入現金", items[2].ToString());
                Currency.Add("買入即期", items[3].ToString());
                Currency.Add("買入遠期10天", items[4].ToString());
                Currency.Add("買入遠期30天", items[5].ToString());
                Currency.Add("買入遠期60天", items[6].ToString());
                Currency.Add("買入遠期90天", items[7].ToString());
                Currency.Add("買入遠期120天", items[8].ToString());
                Currency.Add("買入遠期150天", items[9].ToString());
                Currency.Add("買入遠期180天", items[10].ToString());

                Currency.Add("賣出現金", items[12].ToString());
                Currency.Add("賣出即期", items[13].ToString());
                Currency.Add("賣出遠期10天", items[14].ToString());
                Currency.Add("賣出遠期30天", items[15].ToString());
                Currency.Add("賣出遠期60天", items[16].ToString());
                Currency.Add("賣出遠期90天", items[17].ToString());
                Currency.Add("賣出遠期120天", items[18].ToString());
                Currency.Add("賣出遠期150天", items[19].ToString());
                Currency.Add("賣出遠期180天", items[20].ToString());
                bankCurrency.Add(Currency);
            }
            // Json 轉換
            // 如果有要轉出在這邊取content寫成檔案
            content = bankCurrency.ToString();
            DataTable dsTb = JsonConvert.DeserializeObject<DataTable>(content);

            int updateRowCheck = 0;
            int insertRowCheck = 0;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                try
                {
                    foreach (DataRow dr in dsTb.Rows)
                    {
                        // 檢查重複性
                        SqlCommand sqlUpdateCheck = new SqlCommand("update Rate set " +
                        "UpdateTime=@UpdateTime," +
                        "Currency=@Currency," +
                        "BuyingCash=@BuyingCash," +
                        "BuyingSpot=@BuyingSpot," +
                        "BuyingForward_D10=@BuyingForward_D10," +
                        "BuyingForward_D30=@BuyingForward_D30," +
                        "BuyingForward_D60=@BuyingForward_D60," +
                        "BuyingForward_D90=@BuyingForward_D90," +
                        "BuyingForward_D120=@BuyingForward_D120," +
                        "BuyingForward_D150=@BuyingForward_D150," +
                        "BuyingForward_D180=@BuyingForward_D180," +
                        "SellingCash=@SellingCash," +
                        "SellingSpot=@SellingSpot," +
                        "SellingForward_D10=@SellingForward_D10," +
                        "SellingForward_D30=@SellingForward_D30," +
                        "SellingForward_D60=@SellingForward_D60," +
                        "SellingForward_D90=@SellingForward_D90," +
                        "SellingForward_D120=@SellingForward_D120," +
                        "SellingForward_D150=@SellingForward_D150," +
                        "SellingForward_D180=@SellingForward_D180" + " where UpdateTime=@UpdateTime and Currency=@Currency", con);

                        sqlUpdateCheck.Parameters.AddWithValue("@UpdateTime", FileDate.ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@Currency", dr[0].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingCash", dr[1].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingSpot", dr[2].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingForward_D10", dr[3].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingForward_D30", dr[4].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingForward_D60", dr[5].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingForward_D90", dr[6].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingForward_D120", dr[7].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingForward_D150", dr[8].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@BuyingForward_D180", dr[9].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingCash", dr[10].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingSpot", dr[11].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingForward_D10", dr[12].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingForward_D30", dr[13].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingForward_D60", dr[14].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingForward_D90", dr[15].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingForward_D120", dr[16].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingForward_D150", dr[17].ToString());
                        sqlUpdateCheck.Parameters.AddWithValue("@SellingForward_D180", dr[18].ToString());
                        updateRowCheck = sqlUpdateCheck.ExecuteNonQuery();

                        if (updateRowCheck.Equals(0)) //確認是沒有的資料才新增
                        {
                            string _sql = "Insert into Rate (UpdateTime, Currency, BuyingCash, BuyingSpot, BuyingForward_D10, " +
                                "BuyingForward_D30, BuyingForward_D60, BuyingForward_D90, BuyingForward_D120, BuyingForward_D150, BuyingForward_D180, " +
                                "SellingCash, SellingSpot, SellingForward_D10, SellingForward_D30, SellingForward_D60, SellingForward_D90, " +
                                "SellingForward_D120, SellingForward_D150, SellingForward_D180) " +
                                "Values (@UpdateTime, @Currency, @BuyingCash, @BuyingSpot, @BuyingForward_D10, " +
                                "@BuyingForward_D30, @BuyingForward_D60, @BuyingForward_D90, @BuyingForward_D120, @BuyingForward_D150, @BuyingForward_D180, " +
                                "@SellingCash, @SellingSpot, @SellingForward_D10, @SellingForward_D30, @SellingForward_D60, @SellingForward_D90, " +
                                "@SellingForward_D120, @SellingForward_D150, @SellingForward_D180)";
                            SqlCommand sqlCom = new SqlCommand(_sql, con);
                            sqlCom.Parameters.AddWithValue("@UpdateTime", FileDate.ToString());
                            sqlCom.Parameters.AddWithValue("@Currency", dr[0].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingCash", dr[1].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingSpot", dr[2].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingForward_D10", dr[3].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingForward_D30", dr[4].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingForward_D60", dr[5].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingForward_D90", dr[6].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingForward_D120", dr[7].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingForward_D150", dr[8].ToString());
                            sqlCom.Parameters.AddWithValue("@BuyingForward_D180", dr[9].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingCash", dr[10].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingSpot", dr[11].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingForward_D10", dr[12].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingForward_D30", dr[13].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingForward_D60", dr[14].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingForward_D90", dr[15].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingForward_D120", dr[16].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingForward_D150", dr[17].ToString());
                            sqlCom.Parameters.AddWithValue("@SellingForward_D180", dr[18].ToString());
                            insertRowCheck = sqlCom.ExecuteNonQuery();
                            
                        }
                    }
                }
                catch (SqlException err)
                {
                    LogFileHandler("SqlExcepiton錯誤: " + err.ToString());
                    log("ERR_SQL", err.Message, "匯率", err.ToString(), tag);
                }
                catch (Exception err)
                {
                    LogFileHandler("CSharpException錯誤: " + err.ToString());
                    log("ERR_CSHARP", err.Message, "匯率", err.ToString(), tag);
                }
                if (insertRowCheck > 0)
                    log("SUCCESS_INSERT", filename, "匯率", "", tag);
                if(updateRowCheck > 0)
                    log("SUCCESS_UPDATE", filename, "匯率", "", tag);

                con.Close();
            }
            //Console.ReadLine();
        }

        // 錯誤參照紀錄器
        public static void LogFileHandler(string log)
        {
            string logFolder = "ErrorEventLog";
            string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), logFolder);
            string logPath = Path.Combine(folderPath, "Errorlog_" + DateTime.Now.ToString("yyyy-MM") + ".txt");

            System.IO.Directory.CreateDirectory(folderPath);
            using (FileStream fs = new FileStream(logPath, FileMode.Append))
            {
                using (StreamWriter wr = new StreamWriter(fs))
                {
                    wr.WriteLine("DateTime: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"));
                    wr.WriteLine(log);
                    wr.WriteLine("===================");
                    wr.Close();
                }
            }
        }
        public static void log(string type, string param, string rmk, string memo, string args)
        {
            string _con = "SERVER =192.168.1.130;DATABASE=SAEE;UID=saee;PWD=92vsk48mdf"; ;
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
    }

}
