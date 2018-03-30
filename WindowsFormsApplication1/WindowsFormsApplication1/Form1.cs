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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string dbHost = "163.18.57.43";//資料庫位址
        string dbUser = "root";//資料庫使用者帳號
        string dbPass = "2515251525";//資料庫使用者密碼
        string dbName ; //資料庫名稱
        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;

            inital_DataTime();
        }


        //測試insert 資料 
        private void button1_Click(object sender, EventArgs e)
        {
             dbName = "u0452060"; //資料庫名稱

            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
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

        //未完成 無用
        private void timer1_Tick(object sender, EventArgs e)
        {
            int i = 0;
            i = i + 1;
            if (i >= 7) {
                //測試 5 秒 抓資料 計算 ....

                // (假設實際每3分鐘更新總功率) 抓資料 計算 總功率 
                i = 0;
            }
        }

        //新增dr事件
        private void dr_set(string start_at ,string end_at, string updated_at) {
            dbName = "hems";

            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            command.CommandText = "Insert into dr(start_at,end_at,updated_at) values('" + start_at + "','" + end_at + "','" + updated_at + "')";
           // command.CommandText = "Insert into dr(id, event_id, price, quantity, boolean, pre_p, actual_p, target_p, start_at, end_at, duration, capacity, dr_load, expected_reward, actual_reward, result, updated_at) values('" + "12" + "', '" + "1" + "','" + "50" + "', '" + "20" + "', '" + "1" + "', '" + "0" + "','" + "0" + "', '" + "0" + "', '" + start_at + "', '" + end_at + "','" + updated_at + "', '" + "0" + "', '" + "0" + "', '" + "0" + "','" + "0" + "', '" + "0" + "', '" + "0" + "', '" + updated_at + "')";

            command.ExecuteNonQuery(); // 返回被影響的行數 
           
            Console.ReadLine();
            conn.Close();
        
            dbName = null;

           // dr_on();
        }
        //執行dr工作 
        private void dr_on() {

            dbName = "hems";
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();

            //command.CommandText = "select * from  plug ";



        }

        private void dr_ing() {

        }
        //初始化 日期時間選擇器
        private void inital_DataTime(){
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


        //設定需量反應事件
        private void button2_Click(object sender, EventArgs e)
        {
            string start_at = label3.Text;
            string end_at = label4.Text;
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");

            dr_set(start_at, end_at, currentTime);
        }

        //確認選擇時間 
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

        private void button3_Click(object sender, EventArgs e)
        {
            dbName = "hems";
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            // MySqlCommand command = conn.CreateCommand();
            conn.Open();

            //查詢 test_table 資料表中 id 欄位為 1 的 name 欄位值
            string sql = "SELECT * FROM  plug WHERE 1 ";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            //取得 name 欄位值
            // MySqlDataReader abc = cmd.MySqlDataReader();
            MySqlDataReader Reader = cmd.ExecuteReader();

            //String _name = (string)cmd.ExecuteScalar().ToString();
            try
            {
                while (Reader.Read())
                {
                    int i = 0;
                    while (Reader[i].ToString() != null)
                    {
                        label6.Text = label6.Text + Reader[i].ToString();
                        i = i + 1;
                    }
                }
            }
            catch (MySqlException EX)
            {

            }
            catch (IndexOutOfRangeException outofRange) {
            }

            //Console.ReadLine();
            conn.Close();



        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i ;

            dbName = "hems";
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;

           // string query2 = "SELECT 'power','PF' FROM plug WHERE 1 ";
           
            
            //查詢 test_table 資料表的資料筆數
            string query = "SELECT COUNT(*) FROM plug WHERE 1"; // it works 

            MySqlConnection connection = new MySqlConnection(config);
            MySqlCommand command = new MySqlCommand(query, connection);

            connection.Open();



            //取得筆數值

            //try{

                int _count = (int)(long)command.ExecuteScalar();

                MessageBox.Show("_count : " + _count + "\n");




                MySqlDataReader Reader = command.ExecuteReader();

                PagesJSON pj = new PagesJSON();

                JsonStr pr = new JsonStr();

                while (Reader.Read())
                {

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



                    // DataTable dt = new DataTable();

                    pj.plug.Add(pr);
                }
                catch (MySql.Data.Types.MySqlConversionException ConEx)
                {
                    // MessageBox.Show( "" +ConEx);
                }
                catch (MySqlException sqlEX)
                {
                    MessageBox.Show("" + sqlEX);
                }
                catch (IndexOutOfRangeException ee) {
                    MessageBox.Show("IndexOutOfRangeException" + "\n" + ee);
                }


                    // MessageBox.Show(JsonConvert.SerializeObject(pj));

                    //如果此路徑沒有資料夾
                    if (!Directory.Exists("D:\\Thisway_Log"))
                    {
                        //新增資料夾
                        Directory.CreateDirectory("D:\\Thisway_Log");
                    }

                    //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                    // File.AppendAllText("D:\\Thisway_Log\\" + "logJson" + ".txt", "\r\n" + JsonConvert.SerializeObject(pj) + "：" + "內容");

                }
            
                    connection.Close();

                
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
            
            public string id;
            public string name;
            public string status;
            public string voltage;
            public string current;
            public string power;
            public string S;   
            public string PF;
            public string schedule;
            public string before_schedule;
            public string before_status;
            public string checked_;
            public string recover;
            public string now;
            public string start;
            public string end;
            public string timedif;
            public string count;
            public string total;
            public string updated_at;
            public string updating;
            public string timediff;
            
            /*
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
            */

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

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Visible = false;
            form2.Visible = true;
        }
    }
}
