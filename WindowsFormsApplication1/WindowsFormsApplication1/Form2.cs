using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Newtonsoft.Json;
using MySql.Data.Types;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {

        string dbHost = "163.18.57.43";//資料庫位址
        string dbUser = "root";//資料庫使用者帳號
        string dbPass = "2515251525";//資料庫使用者密碼
        string dbName; //資料庫名稱
        object DBJson; 

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Visible = false;
            form1.Visible = true;
        }

        //MySqlReader select
        private void button2_Click(object sender, EventArgs e)
        {

            dbName = "hems";
            string config = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection connection = new MySqlConnection(config);
            

            //string query = "SELECT * FROM equipment WHERE `id` != 0 ";
            //string query = "SELECT * FROM equipment WHERE `id` = 2 ";

            string query = "SELECT * FROM plug WHERE 1 ";
            // string query = "SELECT * FROM plug WHERE 1 ";
            MySqlCommand command = new MySqlCommand(query, connection);

            connection.Open();

            MySqlDataReader Reader = command.ExecuteReader();

                while (Reader.Read()) {
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
                        MessageBox.Show("MySqlException : " + "\n" + sqlEX);

                    }
                    catch (ArgumentOutOfRangeException ee)
                    {
                        MessageBox.Show("ArgumentOutOfRangeException : " + "\n" + ee);

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
                        MessageBox.Show("IOException : " + "\n" + eee);
                    }

                DBJson = DBJson + JsonConvert.SerializeObject(pj);
            } //while  
            connection.Close();

            MessageBox.Show(""+DBJson);
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
            /*
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
            */

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



        //dataTable select
        private void button14_Click(object sender, EventArgs e)
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
                MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, connection);
                //MySqlDataAdapter自動 open &close

                dataAdapter1.Fill(dt);

                // dataAdapter1.Fill(dataSet, "test1");

                // test1 的 DataTable
                //DataTable dataTable = dataSet.Tables["test1"];

                //列出 test1 的第 4 筆資料
                MessageBox.Show("" + dt.Rows[0]["id"].ToString() + "\n" + dt.Rows[0]["name"].ToString() +
                    "\n" + dt.Rows[1]["id"].ToString() + "\n" + dt.Rows[1]["name"].ToString()+ "\n"+ 
                    "\n" + dt.Rows[2]["id"].ToString() + "\n" + dt.Rows[2]["name"].ToString() + "\n"+ 
                    "\n" + dt.Rows[3]["id"].ToString() + "\n" + dt.Rows[3]["name"].ToString() 
                    );
                // Console.WriteLine("id={0} , name={1}", dataTable.Rows[3]["id"], dataTable.Rows[3]["name"]);
            }
            catch (MySqlConversionException ee) {
                MessageBox.Show("MySqlConversionException : \n" + ee);
            }

        }


    }
}