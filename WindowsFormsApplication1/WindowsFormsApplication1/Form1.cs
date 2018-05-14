using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.IO;
using MySql.Data.Types;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        string dbHost = "163.18.57.43";//資料庫位址
        string dbUser = "root";//資料庫使用者帳號
        string dbPass = "2515251525";//資料庫使用者密碼
        string dbName; //資料庫名稱
        string dtName; //資料表名稱
        string[] dtArray = {"plug","equipment"}; //設備資料表名稱
        string[] pickworkArray = { "work0", "work1" }; //DR執行狀態欄位名稱
        string[] picksaveArray = { "save0", "save1" }; //節電量欄位名稱
        string[] historiesArray = { "plug_histories", "equipment_histories" }; //歷史資料表名稱
        object DBJson;
        string drdone = "",drfee = "";


        int numTick1 = 0;
        int numTick2 = 0;
        int _count;
        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;

            inital_DataTime();

            //DR開始 預設現在+2分鐘   ~~~DR結束 預設現在+3分鐘
            string current1TimeStr = DateTime.Now.AddMinutes(2).ToString("yyyy-MM-dd HH:mm:ss");
            string current2TimeStr = DateTime.Now.AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss");
            DateJob.getString1(current1TimeStr);
            dtp.Value = Convert.ToDateTime(DateJob.getYMD());
            dtpTime.Value = Convert.ToDateTime(DateJob.getHMS());
            DateJob.getString1(current2TimeStr);
            dtp2.Value = Convert.ToDateTime(DateJob.getYMD());
            dtpTime2.Value = Convert.ToDateTime(DateJob.getHMS());
        }


        //insert 資料  u0452060(DataBase).login(DataTable)
        private void button1_Click(object sender, EventArgs e)
        {
            dbName = "u0452060"; //資料庫名稱

            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            String account;
            String password;
            int level;
            for (int i = 0; i < 2; i++)
            {
                account = "account" + i.ToString();
                password = "password" + i.ToString();
                level = i * 10;
                command.CommandText = "Insert into login(username,password) values('" + account + "','" + password + "')";
                command.ExecuteNonQuery();
            }
            Console.ReadLine();
            conn.Close();

            dbName = null;
        }


        //執行dr工作 //欄位work = 1 不執行 /work = 0 執行 
        private void dr_on(string dtname,string pickwork,string picksave,string dthistories)
        {
            //dtArray[0] 
            //建立 DataTable
            DataTable dt = new DataTable();

            int datarow_num;
            dbName = "hems";
            string Qrequest = "SELECT COUNT(*) FROM " + dtname + " WHERE checked = 1";
            datarow_num = DataRowNumber(dbName, Qrequest);
            //MessageBox.Show("資料筆數 : " + datarow_num);
            //Allow Zero Datetime = true 讓Datetime可為 0000-00-00 00:00:00
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
            MySqlConnection connection = new MySqlConnection(config);
            try
            {
                //使用 MySqlDataAdapter 查詢資料，並將結果存回 DataSet 當做名為 test1 的 DataTable
                string query = "SELECT * FROM " + dtname + " WHERE checked = 1 ";
                MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, connection);
                //MySqlDataAdapter自動 open &close
                dataAdapter1.Fill(dt);

                string message = "";
                string[] id = new string[datarow_num];
                string[] name = new string[datarow_num];
                string[] status = new string[datarow_num];
                string[] schedule = new string[datarow_num];

                for (int i = 0; i < datarow_num; i++)
                {
                    //message = message + dt.Rows[i]["id"].ToString() + "~~name~~" + dt.Rows[i]["name"].ToString() + "\n";
                    id[i] = dt.Rows[i]["id"].ToString();
                    name[i] = dt.Rows[i]["name"].ToString();
                    schedule[i] = dt.Rows[i]["schedule"].ToString();
                    status[i] = dt.Rows[i]["status"].ToString();

                    dr_actionset(dtname, id[i], name[i], status[i], schedule[i],pickwork, picksave, dthistories);
                }

                drJob.IfDRTime(dbName, "dr", pickwork);
                string drid = "";
                drid = drJob.getdrid();
                //確認執行一次dr_on
                string sql2 = "UPDATE `dr` SET `" + pickwork + "`= '1'" + " WHERE `id`=" + drid;
                update(sql2);
            }
            catch (MySqlConversionException ee){/*MessageBox.Show( "MySqlConversionException : \n" + ee);*/}
            catch (IndexOutOfRangeException rangeE){/*MessageBox.Show("IndexOutOfRangeException : " + rangeE);*/}

            dbName = null;
        }
        //before_status&before_schedule = 原資料/ 令status&schedule  = 0
        public void dr_actionset(string dtname,string id,string name, string status, string schedule,string pickwork, string picksave,string dthistories)
        {
            dbName = "hems"; //資料庫名稱
            drJob.IfDRTime(dbName, "dr", pickwork);
            string drid = "";
            int work = -1;
            int duration = -1;
            string start_at = "";
            drid = drJob.getdrid();
            work = drJob.getdrwork();
            duration = drJob.getduration();
            start_at = drJob.getdrstart_at();

            if (work.Equals(0))
            {
                //儲存 status,schedule 狀態 並歸零
                string sql = "UPDATE "+ dtname +" SET before_status=" + status + "," + "before_schedule =" + schedule + "," + "status ='0'" + "," + "schedule ='0'" + " WHERE id=" + id ;
                update(sql);


                //計算電費 name(OK),start_at(OK),duration(OK),dthistories(OK) 移過來 可以用drid拿到當前dr事件 
                //現在dr事件start_at - 1min

                DateTime drMinus1minDT = Convert.ToDateTime(start_at).AddMinutes(-1);
                string drMinus1minStr = drMinus1minDT.ToString("yyyy-MM-dd HH:mm"); //不考慮秒數

                
                //MessageBox.Show("drMinus1minStr : " + drMinus1minStr);
                //取得 name[n] && start_at - 1min 的energy ~~出來計算
                float energy = 0f;
                float save = 0f;
                //取得dr-save 和 ooxx_histories-energy  資料
                string Qrequest3 = "SELECT `energy` FROM `" + dthistories + "` WHERE `name` = " + name + " AND `inserted_at` LIKE '" + drMinus1minStr + "%'";
                //MessageBox.Show("Qrequest3 : " + Qrequest3);
                energy = getEnergy(Qrequest3);
                string Qrequest4 = "SELECT `"+ picksave + "` FROM `dr` WHERE `id`=" + drid;
                save = getEnergy(Qrequest4);
                //更新dr_save
                save = save + energy;
                string sql3 = "UPDATE `dr` SET `" + picksave + "` =" + save + " WHERE `id`=" + drid;
                update(sql3);

                
                label6.Text = "目　前　電　價　： " + getFeeNum(drMinus1minStr + ":00") + " 元";
            }

            dbName = null;
        }
        //update資料庫 資料 
        private void update(string sql) {
            dbName = "hems";
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            try
            {
                conn.Open();
                int n = cmd.ExecuteNonQuery();
                conn.Close();
                
            }
            catch (MySqlException ee) {
                //MessageBox.Show("MySqlException : " +ee);
            }
            dbName = null;
        }

        //執行dr_finish工作 
        private void dr_finish(string dtname, string pickwork, string picksave)
        {

            //建立 DataSet
            DataTable dt = new DataTable();

            int datarow_num;

            dbName = "hems";
            string Qrequest = "SELECT COUNT(*) FROM " + dtname + " WHERE checked = 1 ";
            datarow_num = DataRowNumber(dbName, Qrequest);
            // MessageBox.Show("資料筆數 : " + datarow_num);
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName+"; Allow Zero Datetime = true";
            MySqlConnection connection = new MySqlConnection(config);
            try
            {
                //使用 MySqlDataAdapter 查詢資料，並將結果存回 DataSet 當做名為 test1 的 DataTable
                string query = "SELECT * FROM " + dtname + " WHERE checked = 1 ";
                MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, connection);
                //MySqlDataAdapter自動 open &close
                dataAdapter1.Fill(dt);

                string message = "";
                string[] id = new string[datarow_num];
                string[] recover = new string[datarow_num];
                string[] before_status = new string[datarow_num];
                string[] before_schedule = new string[datarow_num];

                // string[] check, status, schedule = new schedule();

                for (int i = 0; i < datarow_num; i++)
                {
                    id[i] = dt.Rows[i]["id"].ToString();
                    before_schedule[i] = dt.Rows[i]["before_schedule"].ToString();
                    before_status[i] = dt.Rows[i]["before_status"].ToString();
                    recover[i] = dt.Rows[i]["recover"].ToString();
                    
                    if (recover[i].Equals("1"))//参與&復電 ~DR
                    {
                        dr_actionoff(dtname,id[i], before_status[i], before_schedule[i], pickwork, picksave);
                        // set 
                    }
                    if  (recover[i].Equals("0"))////参與DR
                    {
                        dr_actionoff(dtname,id[i], "0", before_schedule[i], pickwork, picksave);
                        // set 
                    }
                }

                drJob.IfDRTime(dbName, "dr", pickwork);
                string drid = "";
                drid = drJob.getdrid();
                //確認執行一次dr_on
                string sql2 = "UPDATE `dr` SET `" + pickwork + "`= '2'" + " WHERE `id`=" + drid;
                update(sql2);
                //MessageBox.Show(message);

            }
            catch (MySqlConversionException ee) {
                //MessageBox.Show("MySqlConversionException : \n" + ee);
            }
            catch (IndexOutOfRangeException rangeE) {
               // MessageBox.Show("IndexOutOfRangeException : " + rangeE);
            }

            dbName = null;
            dtName = null;
        }
        //status&schedule = 原資料/ 令before_statu s&before_schedule  歸零
        private void dr_actionoff(string dtname, string id, string before_status, string before_schedule,string pickwork, string picksave)
        {
            dbName = "hems"; //資料庫名稱
            drJob.IfDRTime(dbName, "dr",pickwork);
            int work = -1;
            string drid = "";
            drid = drJob.getdrid();
            work = drJob.getdrwork();
            if (work == 1)
            {
                //儲存 status,schedule 狀態 並歸零
                string sql = "UPDATE " + dtname + " SET status=" + before_status + "," + "schedule =" + before_schedule + "," + "before_status ='0'" + "," + "before_schedule ='0'" + " WHERE id=" + id;
                update(sql);

                dbName = null;

            }
        }


        //dr執行中判斷
        private void dr_ing()
        {
            dbName = "hems";
            dtName = "dr";
            for (int k = 0; k < 2; k++)
            {
                drJob.IfDRTime(dbName, dtName, pickworkArray[k]);
                string drstate = drJob.getdrstate();
                string id = drJob.getdrid();
                int drwork = -1;
                drwork = drJob.getdrwork();
                if (drstate.Equals("drstart") && drwork.Equals(0)) //有DR事件正在發生  work=0 ==> 還未執行dr_on
                {
                dr_on(dtArray[k], pickworkArray[k], picksaveArray[k], historiesArray[k]);

                float save1 = 0;
                float duration = 0;
                drJob.IfDRTime("hems", "dr", pickworkArray[k]);
                string Qrequest1 = "SELECT `"+ picksaveArray[k] + "` FROM dr WHERE `id`=" + id;
                save1 = getEnergy(Qrequest1);

                string Qrequest2 = "SELECT `duration` FROM `dr` WHERE `id`=" + id;
                duration = (float)getDuration(Qrequest2);
                    string totalsave = "" + save1 * (duration / 3600); 
                    //save1 = save1 * (duration / (float)3600);  //度 * 小時 
                if (k == 0){label14.Text = "展示設備 節電量 ： " + totalsave + " 度";}
                else if (k == 1) { label15.Text = "模擬家電 節電量 ： " + totalsave + " 度";}


               // string Qrequest4 = "SELECT `duration` FROM `dr` WHERE `id`=" + drid;
                //save = getEnergy(Qrequest4);
                    
                }
                else if (drstate.Equals("drend") && drwork.Equals(1))//結束DR事件work=1 ==> 還未執行dr_finish
                {
                        //drJob.IfDRTime(dbName, dtName, pickworkArray[k]);
                        dr_finish(dtArray[k], pickworkArray[k], picksaveArray[k]);              
                }
            }

            dbName = null;
            dtName = null;
        }


        //初始化 日期時間選擇器
        private void inital_DataTime()
        {
            // 設定 dtp 日期格式
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "yyyy-MM-dd";

            // 設定 dtpTime 顯示時間，tt 代表 上午/下午
            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "HH:mm";
            dtpTime.ShowUpDown = true;

            // 設定 dtp 日期格式
            dtp2.Format = DateTimePickerFormat.Custom;
            dtp2.CustomFormat = "yyyy-MM-dd";

            // 設定 dtpTime 顯示時間，tt 代表 上午/下午
            dtpTime2.Format = DateTimePickerFormat.Custom;
            dtpTime2.CustomFormat = "HH:mm";
            dtpTime2.ShowUpDown = true;
        }

        //手動設定需量反應事件
        private void button2_Click(object sender, EventArgs e)
        {
            string currentTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// 2018-04-17 01:20:26
            string start_at = dtp.Text + " " + dtpTime.Text + ":00"; ;
            string end_at = dtp2.Text + " " + dtpTime2.Text + ":00"; ;
            DateJob.getTimeDiff(start_at, end_at); //取得時間差
            double Start_Now = DateJob.getStart_Now(); //開始 減 現在
            double End_Start= DateJob.getEnd_Start();//結束 減 開始

            if (Start_Now > 0 && End_Start > 0)
            { //判斷 需量時間合理 (現在-開始 >0   && 結束-開始>0
                    dbName = "hems";
                    dtName = "dr";
                    drJob.dr_IfRepeat_Insert(dbName, dtName, start_at, end_at, 0);
                    dbName = null;
                    dtName = null;
                label13.Visible = true;
                label13.Text = "需量反應事件 \n 設定完成";
                drdone = "done";
            }
            else if(Start_Now < 0){MessageBox.Show("請設定開始時間於未來");}
            else if (Start_Now > 0 && End_Start < 0){MessageBox.Show("請設定結束時間大於開始時間");}

            //label13.Text = "Start_Now : " + Start_Now + "\n" +"End_Start : " + End_Start + " \n";
            

        }


        //確認選擇時間 選擇時間自動更新於Label
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = dtp.Text + " " + dtpTime.Text + ":00";
        }
        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = dtp.Text + " " + dtpTime.Text + ":00";
        }
        private void dtp2_ValueChanged(object sender, EventArgs e)
        {
            label4.Text = dtp2.Text + " " + dtpTime2.Text + ":00";
        }
        private void dtpTime2_ValueChanged_1(object sender, EventArgs e)
        {
            label4.Text = dtp2.Text + " " + dtpTime2.Text + ":00";
        }


        //dataTable select
        private void button3_Click(object sender, EventArgs e)
        {
            dbName = "hems";
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection connection = new MySqlConnection(config);

            try
            {
                //建立 DataSet
                DataTable dt = new DataTable();
                //使用 MySqlDataAdapter 查詢資料，並將結果存回 DataSet 當做名為 test1 的 DataTable
                string query = "SELECT * FROM plug WHERE 1 ";
                MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, connection);//MySqlDataAdapter自動 open &close
                dataAdapter1.Fill(dt);

                //列出 test1 的第 4 筆資料
                MessageBox.Show("id : " + dt.Rows[0]["id"].ToString() + "\n" + "name : " + dt.Rows[0]["name"].ToString() +
                    "\n" + "id : " + dt.Rows[1]["id"].ToString() + "\n" + "name : " + dt.Rows[1]["name"].ToString() +
                    "\n" + "id : " + dt.Rows[2]["id"].ToString() + "\n" + "name : " + dt.Rows[2]["name"].ToString() +
                    "\n" + "id : " + dt.Rows[3]["id"].ToString() + "\n" + "name : " + dt.Rows[3]["name"].ToString()
                    );
            }
            catch (MySqlConversionException ee)
            {
               // MessageBox.Show("MySqlConversionException : \n" + ee);
            }


            dbName = null;
            dtName = null;
        }

        //按鈕4~~~selet並回傳資料筆數
        private void button4_Click(object sender, EventArgs e)
        {

            dbName = "hems";
            dtName = "plug";
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            //查詢 test_table 資料表的資料筆數
            //string query = "SELECT COUNT(*) FROM plug WHERE 1"; // it works 
            string query = "SELECT COUNT(*) FROM " + dtName + " WHERE 1"; // it works    "string(空格)" + dtname + "string(空格)"


            MySqlConnection connection = new MySqlConnection(config);
            MySqlCommand command = new MySqlCommand(query, connection);

            connection.Open();
            //取得筆數值
            try
            {
                int _count = (int)(long)command.ExecuteScalar();
                MessageBox.Show("_count : " + _count + "\n");
            }
            catch (MySqlException sqle) { }
            connection.Close();

            dbName = null;
            dtName = null;
        }
        
        //select並回傳資料筆數
        private int DataRowNumber(string DBname, string Qrequest)
        {
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
        

        //切換至form2
        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Visible = false;
            form2.Visible = true;
        }

        //timer 1 
        private void timer1_Tick(object sender, EventArgs e)
        {
            numTick1++;
           // label6.Text = "程式啟動時間 ： " + numTick1 + "秒";
            label16.Text ="現在時間 ： " +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        //updateTest
        private void button9_Click(object sender, EventArgs e)
        {
            dbName = "hems"; //資料庫名稱

            string sql = "UPDATE plug SET status='1' WHERE id='4'";
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            conn.Open();
            int n = cmd.ExecuteNonQuery();
            MessageBox.Show("" + n);
            conn.Close();

            dbName = null;
        }
 
        private void btnDate1_Click(object sender, EventArgs e)
        {
            label14.Text = "展示設備 節電量 ： ";
            label15.Text = "模擬家電 節電量 ： ";
            label6.Text = "目　前　電　價　： ";

        }



        //測試三段電價if完成
        private void btnYear_Click(object sender, EventArgs e)
        {
            string timer2 = "2018-04-06 10:30:00";
            getFeeNum(timer2);

        }
        //判斷三段電價
        public double getFeeNum(string date1)
        {

            double price;

            DateJob.getString1(date1);
            string Year = DateJob.getYear();
            string Month = DateJob.getMonth();
            string Day = DateJob.getDay();
            string Hour = DateJob.getHour();
            string Min = DateJob.getMin();
            string Sec = DateJob.getSec();
            /*
            MessageBox.Show("年 : " + DateJob.getYear() + "\n" +"月 : " + DateJob.getMonth() + "\n" +"日 : " + DateJob.getDay() + "\n" +
            "時 : " + DateJob.getHour() + "\n" +"分 : " + DateJob.getMin() + "\n" +"秒 : " + DateJob.getSec() + "\n");
            */
            DateTime dt = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(Day));

            //判斷夏月 做出 三段式電價表 
            if (Month.Equals("06") || Month.Equals("07") || Month.Equals("08") || Month.Equals("09"))
            {
                //夏月
                if (GetDayName(dt).Equals("6") || GetDayName(dt).Equals("7"))
                { price = 1.71; }//假日_全日離峰
                else
                {
                    //非假日
                    if (Hour.Equals("10") || Hour.Equals("11") || Hour.Equals("13") ||
                        Hour.Equals("14") || Hour.Equals("15") || Hour.Equals("16"))
                    { price = 5.84; }//夏日平日_尖峰10-11.59 +13-16.59
                    else if ((Hour.Equals("07") && (Convert.ToInt32(Min) >= 30)) ||
                      Hour.Equals("08") || Hour.Equals("09") || Hour.Equals("12") ||
                      Hour.Equals("17") || Hour.Equals("18") || Hour.Equals("19") ||
                      Hour.Equals("20") || Hour.Equals("21") ||
                     (Hour.Equals("22") && (Convert.ToInt32(Min) < 30)))
                    { price = 3.85; }//夏日平日_半尖峰
                    else { price = 1.71; }//夏日平日_離峰
                }
            }
            else
            {
                //非夏月
                if (GetDayName(dt).Equals("6") || GetDayName(dt).Equals("7"))
                { price = 1.65; }//假日_全日離峰
                else
                {
                    //非假日
                    if ((Hour.Equals("07") && (Convert.ToInt32(Min) < 30)) ||
                      Hour.Equals("00") || Hour.Equals("01") || Hour.Equals("02") ||
                      Hour.Equals("03") || Hour.Equals("04") || Hour.Equals("05") ||
                      Hour.Equals("06") || Hour.Equals("11") ||
                      (Hour.Equals("22") && (Convert.ToInt32(Min) >= 30)))
                    { price = 1.65; }//非夏月平日_離峰0-7.30 + 22.30-24.00
                    else
                    { price = 3.69; }//非夏月平日_半尖峰

                }
            }
            return price;
        }
        //判斷星期幾
        private string GetDayName(DateTime dtt)
        {
            string result = "";
            DateTime dt = dtt;
            if (dt.DayOfWeek == DayOfWeek.Monday) { result = "1"; }
            else if (dt.DayOfWeek == DayOfWeek.Tuesday) { result = "2"; }
            else if (dt.DayOfWeek == DayOfWeek.Wednesday) { result = "3"; }
            else if (dt.DayOfWeek == DayOfWeek.Thursday) { result = "4"; }
            else if (dt.DayOfWeek == DayOfWeek.Friday) { result = "5"; }
            else if (dt.DayOfWeek == DayOfWeek.Saturday) { result = "6"; }
            else if (dt.DayOfWeek == DayOfWeek.Sunday) { result = "7"; }
            return result;
        }


        //新增dr事件(每小時) 並判斷是否有重複    ----drJob
        public void dr_new()
        {
            //current1TimeStr = tomorrow
            string current1TimeStr = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");//tomorrow
            string[] tomorrow = current1TimeStr.Split(' '); //tomorrow年月日
            string[] startTime = { " 10:00:00", " 13:00:00" }; //dr事件 開始時間*2
            string[] endTime = { " 12:00:00", " 17:00:00" };//dr事件 結束時間*2

            for (int k = 0; k < startTime.Length; k++)
            {
                dbName = "hems";
                dtName = "dr";
                drJob.dr_IfRepeat_Insert(dbName,dtName, tomorrow[0]+ startTime[k], tomorrow[0] + endTime[k],1);
                dbName = null;
                dtName = null;
            }

            //開始 結束 現在 持續 
        }

        //很多電器 
        public void W_air() { eq_work("00:00:00", "05:59:59", "03", 2200); }
        public void W_air2() { eq_work("13:00:00", "16:59:59", "12", 2200); }
        public void W_WashMachine() { eq_work("20:00:00", "21:59:59", "01", 250); }
        public void W_Fan1() { eq_work("10:00:00", "12:59:59", "02", 50 * 3); }
        public void W_Fan2() { eq_work("18:00:00", "21:59:59", "14", 50 * 4); }
        public void W_TV1() { eq_work("10:00:00", "12:59:59", "04", 80); }
        public void W_TV2() { eq_work("20:00:00", "22:59:59", "13", 80); }
        public void W_PC() { eq_work("18:00:00", "22:59:59", "05", 150 * 3); }
        public void W_Oven() { eq_work("11:00:00", "11:59:59", "06", 1200 * 3); }
        public void W_fridge() { eq_work("00:00:00", "23:59:59", "07", 60); }  //100*0.6W 冰箱   
        public void W_WaterHeater() { eq_work("18:00:00", "23:59:59", "08", 2000); }// (6kw *2小 )/ 6 = 2kw  
        public void W_TeaHeater() { eq_work("06:00:00", "07:59:59", "09", 800); }
        public void W_hotpot() { eq_work("11:00:00", "11:59:59", "11", 800); }
        public void W_Light1() { eq_work("06:00:00", "17:59:59", "10", 180); } //6燈*30W
        public void W_Light2() { eq_work("18:00:00", "23:59:59", "15", 270); } //9燈*30W

        public void eq_work(string eqStart, string eqFinish, string eqName, int eqPower)
        {//name=03 P=2200 reality =>ac

            DateJob.getTimeDiff(eqStart,eqFinish);//計算時間差 開始 現在 結束
            int Start_Now = (int)DateJob.getStart_Now(); 
            int End_Now = (int)DateJob.getEnd_Now();
            drJob.IfDRTime("hems","dr", "work1");
            
            if (Start_Now <= 0 && End_Now >= 0 && !((drJob.getdrstate()).Equals("drstart")))
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode()); //真亂數
                double randNum1 = ((rnd.NextDouble() * 6) - 3); //+- 3
                randNum1 = Math.Round(randNum1, 5);//小數點五位
                randNum1 = eqPower * (randNum1 / 100) + eqPower; // P +-3%
                randNum1 = randNum1 / 1000; //(Wh --> kWh)
                //上傳 name / energy /inserted_at
                eqHistory_insert(eqName, (float)randNum1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string sql = "UPDATE `equipment` SET `status` = '1',`power` = '"+ (float)(randNum1*1000) + "' WHERE `name`='" + eqName+"'";
                update(sql);
                randNum1 = 0;
                //P
            }
            else
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode()); //真亂數
                double randNum1 = (rnd.NextDouble() * 3); //+- 3
                randNum1 = Math.Round(randNum1, 5);//小數點五位
                randNum1 = randNum1 / 100; // +-3
                randNum1 = randNum1 / 1000; // //(Wh --> kWh)
                //insert
                eqHistory_insert(eqName, (float)randNum1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string sql = "UPDATE `equipment` SET `status` = '0',`power` = '" + (float)(randNum1 * 1000) + "' WHERE `name`='" + eqName + "'";
                update(sql);
                randNum1 = 0;
            }
        }

        //新增需量設備歷史事件
        public void eqHistory_insert(string name, float energy, string inserted_at)
        {
            dbName = "hems"; //資料庫名稱
            try
            {
                string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
                MySqlConnection conn = new MySqlConnection(connStr);
                MySqlCommand command = conn.CreateCommand();
                conn.Open();
                command.CommandText = "Insert into equipment_histories(name,energy,inserted_at) values('" + name + "','" + energy + "','" + inserted_at + "')";
                command.ExecuteNonQuery(); // 返回被影響的行數 

                conn.Close();
            }
            catch (MySqlException EE) { }

            dbName = null;
        }


        //每分鐘更新電器功率
        private void timer3_Tick(object sender, EventArgs e)
        {
            W_air(); W_air2();
            W_WashMachine();
            W_Fan1(); W_Fan2();
            W_TV1(); W_TV2();
            W_PC();
            W_Oven();
            W_fridge();
            W_WaterHeater();
            W_TeaHeater();
            W_hotpot();
            W_Light1(); W_Light2();
            try { 
                if (drdone.Equals("done")) { 
                label13.Visible = false;
                }
            }
            catch (NullReferenceException ee) { }
        }
        //每小時新增明日dr事件
        private void timer4_Tick(object sender, EventArgs e) { dr_new(); }
        //每分鐘判斷dr是否開始or結束 並執行對應工作
        private void timer5_Tick(object sender, EventArgs e) { dr_ing(); }

        //取得xx_histories欄位資料
        public float getEnergy(string query)
        {
            dbName = "hems";
            float energy = 0.0f;

            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
            MySqlConnection connection = new MySqlConnection(config);
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                energy = (float)command.ExecuteScalar();
                connection.Close();
            }
            catch (InvalidCastException ICE) {
                //MessageBox.Show("InvalidCastException : " + ICE);
            }
            catch (NullReferenceException nre) { /*MessageBox.Show("NullReferenceException : " + nre);*/ }
            catch (MySqlException me) {
              //  MessageBox.Show("MySqlException : " + me);
            }
            catch (MySqlConversionException ee) {
                //MessageBox.Show("MySqlConversionException : \n" + ee);
            }
            catch (IndexOutOfRangeException rangeE) {
              //  MessageBox.Show("IndexOutOfRangeException : " + rangeE);
            }

            return energy;
        }
        //取得dr_save欄位資料
        public int getDuration(string query)
        {
            int duration = 0;
            dbName = "hems";
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + "; Allow Zero Datetime = true";
            MySqlConnection connection = new MySqlConnection(config);
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                duration = (int)command.ExecuteScalar();
                connection.Close();
            }
            catch (InvalidCastException ICE) {
                //MessageBox.Show("InvalidCastException : " + ICE); 
            }
            catch (NullReferenceException nre) {
                //MessageBox.Show("NullReferenceException : " + nre);
            }
            catch (MySqlException me) {
               // MessageBox.Show("MySqlException : " + me);
            }
            catch (MySqlConversionException ee) {
                //MessageBox.Show("MySqlConversionException : \n" + ee);
            }
            catch (IndexOutOfRangeException rangeE) {
               // MessageBox.Show("IndexOutOfRangeException : " + rangeE);
            }
            
            return duration;
        }

        //--------------目前以下無用-------------

        //input dtname1 = 設備eq... / plug ; dtname2 = 設備歷史eq..histories /plug_histories
        public void CalculateFee(string dtName1,string dtName2)
        {
            //建立 DataTable
            DataTable dt = new DataTable();

            int datarow_num ;
            dbName = "hems";
            //取得plug資料筆數當設備参與DR
            string Qrequest1 = "SELECT COUNT(*) FROM " + dtName1 + " WHERE checked = '1'";
            datarow_num = DataRowNumber(dbName, Qrequest1);
            //取得plug参與DR設備name
            string Qrequest2 = "SELECT name FROM " + dtName1 + " WHERE checked = 1";
            string[] getname = getEqCheckedName(datarow_num, dbName, Qrequest2);

            //取得 現在發生dr事件的start_at
            drJob.IfDRTime("hems", "dr", "work0");
            string drstart_at =drJob.getdrstart_at();
            string drend_at=drJob.getdrend_at();
            int duration = drJob.getduration();

            dbName = null;
            dtName = null;

            //1 ~判斷資料表check == 1 拿下 id(name) "0X" ex:01 02 03 (select name,checked from plug where checked = 1 ) 
            //2~ 到hems-dr 拿下 現在dr開始時間 & 持續時間 ((SELECT start_at,duration FROM dr WHERE convert(varchar,start_at,120) like  '現在時間年月日') )
            //年月日 = DateJob.getYMD(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00")) + "%";
            //---convert(varchar,start_at,120) like   '2006-04-01%'

            //3~到hems-eq...._history   拿下 where insert_at = [現在dr開始時間("yyyy-MM-dd hh:mm%") - 1min ] && name = "0x"
            //select energy from Meeting where convert(varchar(10),PublishTime,121))  --->varchar(10)  ==yyyy-MM-dd      --->varchar(16)  ==yyyy-MM-dd hh:mm ??
            //SELECT energy FROM T WHERE Convert(varchar,sendTime,120) LIKE '2007-12-30%'
            //4~ energy * (duration /3600) = 單一電器 用電量 

        }
        public string[] getEqCheckedName(int datarow_num, string dbName, string Qrequest)
        {
            DataTable dt = new DataTable();
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection connection = new MySqlConnection(config);
            string[] name = new string[datarow_num];
            try
            {
                //使用 MySqlDataAdapter 查詢資料，並將結果存回 DataSet 當做名為 test1 的 DataTable
                string query = Qrequest;//"SELECT * FROM plug WHERE 1 ";
                MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, connection);
                //MySqlDataAdapter自動 open &close
                dataAdapter1.Fill(dt);

                for (int i = 0; i < datarow_num; i++)
                {
                    name[i] = dt.Rows[i]["name"].ToString();
                }
            }
            catch (MySqlConversionException ee) {
               // MessageBox.Show("MySqlConversionException : \n" + ee);
            }
            catch (IndexOutOfRangeException rangeE) {
               // MessageBox.Show("IndexOutOfRangeException : " + rangeE);
            }
            // dbName = null; //是否需要?
            // dtName = null;//是否需要?
            return name;
        }

        //---------------笨方法抓資料庫資料-------------不採用 
        //MySqlReader select
        private void button6_Click(object sender, EventArgs e)
        {
            dbName = "hems";
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection connection = new MySqlConnection(config);

            string query = "SELECT * FROM plug WHERE 1 ";
            // string query = "SELECT * FROM plug WHERE 1 ";
            MySqlCommand command = new MySqlCommand(query, connection);

            connection.Open();

            MySqlDataReader Reader = command.ExecuteReader();

            while (Reader.Read())
            {
                PagesJSON pj = new PagesJSON();

                JsonStr pr = new JsonStr();
                try
                {
                    // loop on columns
                    pr.id = Reader[0].ToString();
                    pr.name = Reader[1].ToString();
                    pr.status = Reader[2].ToString();
                    pr.voltage = Reader[3].ToString();
                    pr.current = Reader[4].ToString();
                    pr.power = Reader[5].ToString();
                    pr.S = Reader[6].ToString();
                    pr.PF = Reader[7].ToString();
                    pr.schedule = Reader[8].ToString();
                    pr.before_schedule = Reader[9].ToString();
                    pr.before_status = Reader[10].ToString();
                    pr.checked_ = Reader[11].ToString();
                    pr.recover = Reader[12].ToString();

                    pr.now = string.Format("{0:yyyy-MM-dd HH:mm:ss}", Reader[13]);
                    pr.start = string.Format("{0:yyyy-MM-dd HH:mm:ss}", Reader[14]);
                    pr.end = string.Format("{0:yyyy-MM-dd HH:mm:ss}", Reader[15]);

                    pr.timedif = Reader[16].ToString();
                    pr.count = Reader[17].ToString();
                    pr.total = Reader[18].ToString();

                    pr.updated_at = string.Format("{0:yyyy-MM-dd HH:mm:ss}", Reader[19]);
                    pr.updating = string.Format("{0:yyyy-MM-dd HH:mm:ss}", Reader[20]);

                    pr.timediff = Reader[21].ToString();

                    pj.plug.Add(pr);

                }
                catch (MySql.Data.Types.MySqlConversionException ConEx)
                {
                    //MessageBox.Show("MySqlConversionException : " + "\n" + ConEx);
                }
                catch (MySqlException sqlEX)
                {
                    //MessageBox.Show("MySqlException : " + "\n" + sqlEX);

                }
                catch (ArgumentOutOfRangeException ee)
                {
                 //   MessageBox.Show("ArgumentOutOfRangeException : " + "\n" + ee);

                }


                // D:\\Thisway_Log產生log檔案(txt) 可查詢log資料
                try
                {
                    //如果此路徑沒有資料夾
                    if (!Directory.Exists("D:\\Thisway_Log"))
                    {
                        //新增資料夾
                        Directory.CreateDirectory("D:\\Thisway_Log");
                    }
                    //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                    File.AppendAllText("D:\\Thisway_Log\\" + "logJson1" + ".txt", "\r\n" + JsonConvert.SerializeObject(pj) + "：" + "內容");
                }
                catch (IOException eee)
                {
                    //MessageBox.Show("IOException : " + "\n" + eee);
                }

                DBJson = DBJson + JsonConvert.SerializeObject(pj);
            } //while  
            connection.Close();

            MessageBox.Show("" + DBJson);

            dbName = null;
            dtName = null;
        }

        public class PagesJSON
        {
            public List<JsonStr> plug;

            public PagesJSON()
            {
                plug = new List<JsonStr>();
            }

        }
        public class JsonStr
        {
            public string id { get; set; }
            public string name { get; set; }
            public string status { get; set; }
            public string voltage { get; set; }
            public string current { get; set; }
            public string power { get; set; }
            public string S { get; set; }
            public string PF { get; set; }
            public string schedule { get; set; }
            public string before_schedule { get; set; }
            public string before_status { get; set; }
            public string checked_ { get; set; }
            public string recover { get; set; }
            public string now { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string timedif { get; set; }
            public string count { get; set; }
            public string total { get; set; }
            public string updated_at { get; set; }
            public string updating { get; set; }
            public string timediff { get; set; }


            public JsonStr()
            {
                id = "";
                name = "";
                status = "";
                voltage = "";
                current = "";
                power = "";
                S = "";
                PF = "";
                schedule = "";
                before_schedule = "";
                before_status = "";
                checked_ = "";
                recover = "";
                now = "";
                start = "";
                end = "";
                timedif = "";
                count = "";
                total = "";
                updated_at = "";
                updating = "";
                timediff = "";
            }

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            label16.Text = "現在時間 ： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
