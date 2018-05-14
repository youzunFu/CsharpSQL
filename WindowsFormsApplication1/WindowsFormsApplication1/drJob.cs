using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class drJob
    {
        static string dbHost = "163.18.57.43";//資料庫位址
        static string dbUser = "root";//資料庫使用者帳號
        static string dbPass = "2515251525";//資料庫使用者密碼
        static string drstate = "",drstart_at = "", drend_at = "",drid = "";
        static int drwork = -1, drduration = 0;

        //判斷是否為dr開始或結束 並定義start_at ,duration , work 可藉由get取得  
        public static void IfDRTime(string dbName, string dtName,string work) {
            string currentTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// now
            DateTime currentTime = System.DateTime.Now; //now
            string[] today = currentTimeStr.Split(' '); //tomorrow年月日

            //建立 DataTable
            DataTable dt = new DataTable();

            int datarow_num;
            string Qrequest = "SELECT COUNT(*) FROM " + dtName + " WHERE start_at LIKE " + "'" + today[0] + "%" + "'";
            datarow_num = DataRowNumber(dbName, Qrequest);
            //Allow Zero Datetime = true 讓Datetime可為 0000-00-00 00:00:00
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
            MySqlConnection connection = new MySqlConnection(config);
            try
            {
                //使用 MySqlDataAdapter 查詢資料，並將結果存回 DataSet 當做名為 test1 的 DataTable
                string query = "SELECT * FROM " + dtName + " WHERE start_at LIKE " + "'" + today[0] + "%" + "'";
                MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, connection);
                //MySqlDataAdapter自動 open &close
                dataAdapter1.Fill(dt);

                string[] id = new string[datarow_num];
                string[] start_at = new string[datarow_num];
                string[] end_at = new string[datarow_num];
                int[] drworkArr = new int[datarow_num];
                int[] duration = new int[datarow_num];


                for (int i = 0; i < datarow_num; i++)
                {
                    id[i] = dt.Rows[i]["id"].ToString();
                    start_at[i] = Convert.ToDateTime(dt.Rows[i]["start_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                    end_at[i] = Convert.ToDateTime(dt.Rows[i]["end_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                    drworkArr[i] = (int)dt.Rows[i][work];
                    duration[i] = (int)dt.Rows[i]["duration"];

                    DateJob.getTimeDiff(start_at[i], end_at[i]);
                    int Start_Now = (int)DateJob.getStart_Now();
                    int End_Now = (int)DateJob.getEnd_Now();
                    //可能抓到兩筆 正在發生DR  並且留下最後抓到的資料 有錯該修正180507 
                    if ((Start_Now <= 0) && (End_Now >= 0))
                    {
                        drid = id[i]; // 當前DR事件 id值
                        drstart_at = start_at[i];
                        drend_at = end_at[i];
                        drduration = duration[i];
                        drstate = "drstart";
                        drwork = drworkArr[i];  //如drwork = 0 執行dr on 令 drwork = 1 
                    }//需量反應開始 dr_on work
                    else if (End_Now <= 0)//&& (drworkArr[i]).Equals(1)
                    {
                        drid = id[i]; // 剛結束DR事件 id值
                        drstate = "drend";
                        drwork = drworkArr[i];  //如 drwork = 1 執行dr finish 令 drwork = 2 
                    }//需量反應結束 dr_on work

                }
            }
            catch (MySqlException me) { }
            catch (MySqlConversionException ee) { MessageBox.Show("MySqlConversionException : \n" + ee); }
            catch (IndexOutOfRangeException rangeE) { MessageBox.Show("IndexOutOfRangeException : " + rangeE); }
            dbName = null;
            dtName = null;

        }

        public static string getdrid() { return drid; }
        public static int getdrwork() { return drwork; }
        public static string getdrstate() {return drstate;}
        public static int getduration() { return drduration; }
        public static string getdrstart_at() { return drstart_at; }
        public static string getdrend_at() { return drend_at; }

        //---------------------------------------------------------------------------------------------------
        ////(自動手動)新增dr事件(每小時) 並判斷是否有重複  auto = 1 自動 /  auto = 0 手動  
        public static void dr_IfRepeat_Insert(string dbName,string dtName , string startTime,string endTime,int auto)
        {
            //currentTimeStr = tomorrow
            string currentTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// now
            string current1TimeStr = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");//tomorrow
            DateTime currentTime = System.DateTime.Now; //now
            string YMD = "";
            if (auto == 1) {
                string[] tomorrow = current1TimeStr.Split(' '); //tomorrow年月日
                YMD = tomorrow[0];
            } else if (auto == 0) {
               // string[] NonAuto = startTime.Split(' '); //startTime年月日
                //YMD = NonAuto[0];
            }




            DateJob.getTimeDiff(startTime, endTime);
                int duration = (int)DateJob.getEnd_Start(); //duration

                //建立 DataSet
                DataTable dt = new DataTable();

                int datarow_num;
                string Qrequest = "SELECT COUNT(*) FROM " + dtName + " WHERE start_at LIKE " + "'" + YMD + "%" + "'";
                datarow_num = DataRowNumber(dbName, Qrequest);

                //Allow Zero Datetime = true 讓Datetime可為 0000-00-00 00:00:00
                string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
                MySqlConnection connection = new MySqlConnection(config);
                try
                {
                    //使用 MySqlDataAdapter 查詢資料，並將結果存回 DataSet 當做名為 test1 的 DataTable
                    string query = "SELECT * FROM dr WHERE start_at LIKE " + "'" + YMD + "%" + "'";
                    MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, connection);
                    //MySqlDataAdapter自動 open &close
                    dataAdapter1.Fill(dt);

                    string[] Lstart_at = new string[datarow_num];
                    int checkDRinsert = 0;
                    for (int i = 0; i < datarow_num; i++)//檢查資料庫是否已有dr事件 避免重複
                    {
                        Lstart_at[i] = Convert.ToDateTime(dt.Rows[i]["start_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                        if (Lstart_at[i].Equals(startTime)) { checkDRinsert = checkDRinsert + 1; }//已新增過資料
                        else { }//新增Dr事件
                    }
                    // MessageBox.Show(Lstart_at[0] + "\n" + dt.Rows[0]["start_at"]);

                    //insert(dr事件時間);
                    if (checkDRinsert == 0) { dr_set(startTime, endTime, duration, currentTimeStr, dbName); }
                    datarow_num = 0;
                    checkDRinsert = 0;
                }
                catch (MySqlConversionException ee) { MessageBox.Show("MySqlConversionException : \n" + ee); }
                catch (IndexOutOfRangeException rangeE) { MessageBox.Show("IndexOutOfRangeException : " + rangeE); }
                catch (FormatException Fe) { MessageBox.Show("FormatException : " + Fe); }
            //開始 結束 現在 持續 
        }

        //新增dr事件
        private static void dr_set(string start_at, string end_at, int duration, string updated_at,string dbName)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            try
            {
                conn.Open();
                command.CommandText = "Insert into dr(start_at,end_at,duration,updated_at) values('" + start_at + "','" + end_at + "','" + duration + "','" + updated_at + "')";
                command.ExecuteNonQuery(); // 返回被影響的行數 
                conn.Close();
            }
            catch (MySqlException EE) { }
        }

        //select並回傳資料筆數
        private static int DataRowNumber(string DBname, string Qrequest)
        {
            int _count = 0;
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + DBname;
            MySqlConnection connection = new MySqlConnection(config);
            MySqlCommand command = new MySqlCommand(Qrequest, connection);
            try
            {
                connection.Open();
                //取得筆數值
                _count = (int)(long)command.ExecuteScalar();
                connection.Close();
            }
            catch (MySqlException sqle) { }
            return _count;
        }

    }
}
