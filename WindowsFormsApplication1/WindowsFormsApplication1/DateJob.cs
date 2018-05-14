using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    public static class DateJob
    {
        static string[] date1; 
        static string[] date2;
        static string[] date3;
        static string now;
        static double start_now, end_now, end_start;

        //2018-04-02 11:00:00   Input yyyy-MM-DD HH:mm:ss  /output 年、月、日、時、分、秒
        public static void getString1(string date) {
            date1 = date.Split(' ');
            date2 = date1[0].Split('-'); //date2[0] = 年 ,date2[1] = 月,date2[2] = 日 
            date3 = date1[1].Split(':'); //date3[0] = 時 ,date3[1] = 分,date3[2] = 秒
        }
        public static string getYMD() { return date1[0]; } //年月日
        public static string getHMS() { return date1[1]; } //時分秒
        public static string getYear() { return date2[0]; }//年
        public static string getMonth() { return date2[1]; } //月
        public static string getDay() { return date2[2]; }//日
        public static string getHour() { return date3[0]; }//秒
        public static string getMin() { return date3[1]; }//時
        public static string getSec() { return date3[2]; }//分

        //Input yyyy-MM-DD HH:mm:ss  ~~~output end-start
        public static void getTimeDiff(string date1, string date2)
        {
            string currentTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime NowDate = Convert.ToDateTime(currentTimeStr);

            DateTime sDate = Convert.ToDateTime(date1);
            DateTime fDate = Convert.ToDateTime(date2);
            TimeSpan Start_W = sDate - NowDate; 
            TimeSpan End_W = fDate - NowDate;
            TimeSpan duration = fDate - sDate;

            start_now = Start_W.TotalSeconds;
            end_now = End_W.TotalSeconds;
            end_start = duration.TotalSeconds;
        }
        public static void getTimeDiff(string now,string date1, string date2)
        {   DateTime NowDate = Convert.ToDateTime(now);

            DateTime sDate = Convert.ToDateTime(date1);
            DateTime fDate = Convert.ToDateTime(date2);
            TimeSpan Start_W = sDate - NowDate;
            TimeSpan duration = fDate - sDate;

            start_now = Start_W.TotalSeconds;
            end_start = duration.TotalSeconds;
        }

        public static double  getStart_Now() { return start_now; }//開始 減 現在 
        public static double getEnd_Now() { return end_now; }//結束 減 現在 
        public static double getEnd_Start() { return end_start; }//結束 減 開始 取得持續時間
 
    }
}
