using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello world!");
            //SqlConnection con;

            //string constr = "DRIVER ={MySQL ODBC 3.51 Driver};SERVER =192.168.1.129;DATABSE=SAEE_QAS;UID=-;PWD=-;OPTION=3";
            //conn = new OdbcConnection(constr);                        

            //string barcode_constr = "SERVER =192.168.1.130;DATABASE=SAIBARCODE;UID=-;PWD=-";
            //con = new SqlConnection(constr);



            string constr = "SERVER =192.168.1.129;DATABASE=SAEE_QAS;UID=-;PWD=-";
            string bcConstr = "SERVER =192.168.1.130;DATABASE=SAIBARCODE;UID=-;PWD=-";
            string plmConstr = "SERVER =plmdb.saimtlg.com;DATABASE=SAIPLM930;UID=-;PWD=-";
            //string constr = "SERVER =192.168.1.129;DATABASE=SAEE_QAS;UID=-;PWD=-";
            //string bcConstr = constr;
            //string plmConstr = constr;

            string opTable = "PART_BARCODE_MATERIALDATA";                        // Operator Table from PART table of PLM by trigger created.
            string materialDataTable = "MaterialData";         // MaterialData Table from SAIBARCODE.

            //string constr ="";
            //string opTable = "PART_TEST_OP";                        // Operator Table from PART table of PLM by trigger created.
            //string materialDataTable = "PART_materialData";         // MaterialData Table from SAIBARCODE.


            using (SqlConnection plmcon = new SqlConnection(plmConstr)) //TODO:
            {
                try
                {
                    plmcon.Open();
                    //Console.WriteLine("Connect Successes.");

                    // SqlCommand sqlqry = new SqlCommand(_sql, con);
                    // Dataset loading from Operator Table
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlda = new SqlDataAdapter("select [MatNo], [MatDesc], [LastUpdateTime], [FlagOp] from " + opTable + " where FlagOp = 'N'", plmcon); //TODO: notice which con is using.
                    sqlda.Fill(ds);

                    using (SqlConnection bccon = new SqlConnection(bcConstr))
                    {
                        bccon.Open();
                        try
                        {
                            // TO check flag and insert data.
                            int Roweffect = 0;
                            //int updateRoweffect = 0;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                // To check is MatNo already has in db from BARCode, if not then insert a new one.
                                // Otherwise, update the Descripts and updatetime.
                                SqlCommand sqlUpdate = new SqlCommand("update " + materialDataTable + " set LastUpdateTime=@LastUpdateTime, MatDesc=@MatDesc where MatNo=@MatNo", bccon);
                                sqlUpdate.Parameters.AddWithValue("@MatNo", dr["MatNo"].ToString());
                                sqlUpdate.Parameters.AddWithValue("@MatDesc", dr["MatDesc"].ToString());
                                sqlUpdate.Parameters.AddWithValue("@LastUpdateTime", dr["LastUpdateTime"]);
                                int updateRowCheck = sqlUpdate.ExecuteNonQuery();
                                Roweffect += updateRowCheck;

                                if (updateRowCheck.Equals(0) && dr["MatNo"].ToString() != null)
                                {
                                    // 若有重複, 則進行轉交後, 並改寫標記
                                    SqlCommand sqlInsert = new SqlCommand("insert into " + materialDataTable + " (MatNo, MatDesc, FacMatNo, LabFmt, LastUpdateUser, LastUpdateTime, Remark, Printer, LOGO, CompNo) "
                                                                         + "values(@MatNo, @MatDesc, '#N/A', '產品條碼.prn', '9000', CONVERT(varchar, GETDATE(), 120), '', '', 'LOGO.jpg', '')", bccon);
                                    sqlInsert.Parameters.AddWithValue("@MatNo", dr["MatNo"].ToString());
                                    sqlInsert.Parameters.AddWithValue("@MatDesc", dr["MatDesc"].ToString());
                                    Roweffect += sqlInsert.ExecuteNonQuery();
                                }
                                // After finish operate, and update flag.
                                SqlCommand sqlFlagUpdate = new SqlCommand("update " + opTable + " set FlagOp='Y' where MatNo=@MatNo", plmcon); //TODO: notice which con is using.
                                sqlFlagUpdate.Parameters.AddWithValue("@MatNo", dr["MatNo"].ToString());
                                sqlFlagUpdate.ExecuteNonQuery();
                            }
                            // return sucesses message and write a record. 
                            LogFileHandler("ActionEvent: 異動資料筆數: " + Roweffect);
                        }
                        catch (SqlException e)
                        {
                            // if connect have any error, write a record.
                            LogFileHandler("SqlErrorEvent from BCcon: " + e.Message);
                        }
                        catch (Exception e)
                        {
                            // if connect have any error, write a record.
                            LogFileHandler("ErrorEvent from BCcon: " + e.Message);
                        }
                        finally
                        {
                            bccon.Close();
                        }
                    }

                }
                catch (SqlException e)
                {
                    // if connect have any error, write a record.
                    LogFileHandler("SqlErrorEvent from PLMcon: " + e.Message);
                }
                catch (Exception e)
                {
                    // if connect have any error, write a record.
                    LogFileHandler("ErrorEvent from PLMcon: " + e.Message);
                }
                finally
                {
                    plmcon.Close();
                }
            }

            //System.Threading.Thread.Sleep(500); // pause for debug.
            //Console.ReadLine();
        }

        public static void LogFileHandler(string log)
        {

            string logFolder = "EventLog";
            string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), logFolder);
            string logPath = Path.Combine(folderPath, "syslog_" + DateTime.Now.ToString("yyyy-MM") + ".txt");

            System.IO.Directory.CreateDirectory(folderPath);
            using (FileStream fs = new FileStream(logPath, FileMode.Append))
            {
                using (StreamWriter wr = new StreamWriter(fs))
                {
                    wr.WriteLine("DateTime: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000") + ", " + log);
                    wr.Close();
                }
            }
        }

    }

    // Scripts Note:
    // SqlCommand sc = new SqlCommand(sql, connection);
    // SqlCommand insert = new SqlCommand("insert into MyOrder(id, prod_name, prod_time, prod_state) values (@id, @prod_name, GETDATE(), @prod_state)", con);
    // SqlCommand.Parameters.AddWithValue("@obj", obj);
    // SqlCommand.ExecuteNonQuery(); //return an int

}
