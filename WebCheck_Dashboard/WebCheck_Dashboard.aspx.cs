using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebCheck_Dashboard
{
    public partial class WebCheck_Dashboard : System.Web.UI.Page
    {
        public string five_mins_msgcount = "5";
        public string ten_mins_msgcount = "10";
        public string halfhour_msgcount = "30";
        public string hour_msgcount = "60";
        public static string accessSussPercent = "100", accessFailPercent = "100";
        public static DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00"));

        protected void Page_Load(object sender, EventArgs e)
        {
            nowTime = DateTime.Now;
            string outputlog = Request.QueryString["outputlog"];
            if (Convert.ToString(outputlog) != "")
            {
                switch (outputlog)
                {
                    case "daily":
                        DataOutputToCSV("day");
                        break;
                    case "month":
                        DataOutputToCSV("month");
                        break;
                }
            }
        }

        private static string getConn()
        {
            return ConfigurationManager.AppSettings["mysqlConn"];
        }

        public static double getDelayAvgByMins(int msgSpanMINS)
        {
            try
            {
                string delayAvgSql = "SELECT AVG(`pingDelay`) FROM `pingcheck` WHERE pingStatus='200' AND `pingTime` >='" + getTimeStamp(nowTime.AddMinutes(-msgSpanMINS)) + "' and `pingTime`<'" + getTimeStamp(nowTime) + "'";
                double delayAvgJson = Convert.ToDouble(SQLDB.ExecuteScalar(getConn(), delayAvgSql));
                return delayAvgJson;
            }
            catch
            { return -1; }
        }

        public static string getAvailByMins(int msgSpanMINS)
        {
            try
            {
                string availCountSuss = "SELECT COUNT(*) FROM `pingcheck` WHERE pingStatus='200' AND `pingTime` >='" + getTimeStamp(nowTime.AddMinutes(-msgSpanMINS)) + "' AND `pingTime` <='" + getTimeStamp(nowTime) + "'";
                string availCountFail = "SELECT COUNT(*) FROM `pingcheck` WHERE pingStatus<>'200' AND `pingTime` >='" + getTimeStamp(nowTime.AddMinutes(-msgSpanMINS)) + "' AND `pingTime` <='" + getTimeStamp(nowTime) + "'";
                int availCountSussCount = Convert.ToInt32(SQLDB.ExecuteScalar(getConn(), availCountSuss));
                int availCountFailCount = Convert.ToInt32(SQLDB.ExecuteScalar(getConn(), availCountFail));
                double availPercent = (availCountSussCount / (Convert.ToDouble(availCountSussCount + availCountFailCount))) * 100;
                return "<span class=\"right badge badge-success\">连通率 " + availPercent.ToString("f2") + "%</span>";
            }
            catch
            { return "<span class=\"right badge badge-success\">查询失败</span>"; }
        }

        public static string getDelayStatusByMins(int CountDownTimes, string sumType)
        {
            try
            {
                string jsonData = "", msgSUMSql = "", sqlcondition = "";
                double MsgSUMSJson = 0.00;
                DateTime standTime = Convert.ToDateTime(nowTime);
                for (int i = 1; i < CountDownTimes; i++)
                {
                    sqlcondition = "pingTime >='" + getTimeStamp(standTime.AddMinutes(-i - 1)) + "' AND pingTime < '" + getTimeStamp(standTime.AddMinutes(-i)) + "'";
                    switch (sumType)
                    {
                        case "TOTAL_DELAY":
                            msgSUMSql = "SELECT AVG(`pingDelay`) FROM `pingcheck` WHERE pingStatus='200' AND " + sqlcondition;
                            break;
                        case "DNS_DELAY":
                            msgSUMSql = "SELECT AVG(`dnsTime`) FROM `pingcheck` WHERE pingStatus='200' AND " + sqlcondition;
                            break;
                        case "TCP_DELAY":
                            msgSUMSql = "SELECT AVG(`tcpTime`) FROM `pingcheck` WHERE pingStatus='200' AND " + sqlcondition;
                            break;
                        case "SSL_DELAY":
                            msgSUMSql = "SELECT AVG(`sslTime`) FROM `pingcheck` WHERE pingStatus='200' AND " + sqlcondition;
                            break;
                        case "SERV_DELAY":
                            msgSUMSql = "SELECT AVG(`servTime`) FROM `pingcheck` WHERE pingStatus='200' AND " + sqlcondition;
                            break;
                        case "TRANSFER_DELAY":
                            msgSUMSql = "SELECT AVG(`transferTime`) FROM `pingcheck` WHERE pingStatus='200' AND " + sqlcondition;
                            break;
                    }
                    try
                    {
                        MsgSUMSJson = Convert.ToDouble(SQLDB.ExecuteScalar(getConn(), msgSUMSql));
                    }
                    catch { MsgSUMSJson = 0; }

                    jsonData += "" + MsgSUMSJson.ToString("f0") + ",";
                }
                return jsonData;
            }
            catch
            { return ""; }
        }

        public static string getDelayStatus(int CountDownTimes, string statusType, string timeType)
        {
            try
            {
                string jsonData = "", statusSUMSql = "", sqlcondition = "";
                switch (timeType)
                {
                    case "hour":
                        sqlcondition = "pingDatetime > '" + getTimeStamp(Convert.ToDateTime(nowTime.AddHours(-CountDownTimes - 1).ToString("yyyy-MM-dd HH:00:00"))) + "' AND pingDatetime < '" + getTimeStamp(Convert.ToDateTime(nowTime.ToString("yyyy-MM-dd HH:00:00"))) + "' GROUP BY DATE_FORMAT(pingDatetime, \"%Y-%m-%d %H\") ORDER BY `pingDatetime` DESC";      //返回整点的时间戳
                        break;
                    case "day":
                        sqlcondition = "pingDatetime > '" + getTimeStamp(Convert.ToDateTime(nowTime.AddDays(-CountDownTimes - 1).ToString("yyyy-MM-dd 00:00:00"))) + "' AND pingDatetime < '" + getTimeStamp(Convert.ToDateTime(nowTime.ToString("yyyy-MM-dd 23:59:59"))) + "' GROUP BY DATE_FORMAT(pingDatetime, \"%Y-%m-%d\") ORDER BY `pingDatetime` DESC";      //返回整点的时间戳
                        break;
                    case "month":
                        sqlcondition = "pingDatetime > '" + getTimeStamp(Convert.ToDateTime(nowTime.AddMonths(-CountDownTimes - 1).ToString("yyyy-MM-dd 00:00:00"))) + "' AND pingDatetime < '" + getTimeStamp(Convert.ToDateTime(nowTime.ToString("yyyy-MM-dd 23:59:59"))) + "' GROUP BY DATE_FORMAT(pingDatetime, \"%Y-%m\") ORDER BY `pingDatetime` DESC";      //返回整点的时间戳
                        break;
                }

                switch (statusType)
                {
                    case "TOTAL_DELAY":
                        statusSUMSql = "SELECT AVG(totalTime) FROM `pingCheckLog` WHERE " + sqlcondition;
                        break;
                    case "DNS_DELAY":
                        statusSUMSql = "SELECT AVG(dnsTime) FROM `pingCheckLog` WHERE " + sqlcondition;
                        break;
                    case "TCP_DELAY":
                        statusSUMSql = "SELECT AVG(tcpTime) FROM `pingCheckLog` WHERE " + sqlcondition;
                        break;
                    case "SSL_DELAY":
                        statusSUMSql = "SELECT AVG(sslTime) FROM `pingCheckLog` WHERE " + sqlcondition;
                        break;
                    case "SERV_DELAY":
                        statusSUMSql = "SELECT AVG(servTime) FROM `pingCheckLog` WHERE " + sqlcondition;
                        break;
                    case "TRANSFER_DELAY":
                        statusSUMSql = "SELECT AVG(transferTime) FROM `pingCheckLog` WHERE " + sqlcondition;
                        break;
                }

                DataSet DelayStatusByHour_DS = SQLDB.SelectToDataSet(getConn(), statusSUMSql, "Delayhistory");
                for (int i = 0; i < CountDownTimes; i++)
                {
                    double DelaySUMSJson = 0;
                    try
                    {
                        DelaySUMSJson = Convert.ToDouble(DelayStatusByHour_DS.Tables[0].Rows[i][0]);
                    }
                    catch { DelaySUMSJson = 0; }
                    jsonData += "" + DelaySUMSJson.ToString("f0") + ",";
                }
                return jsonData;
            }
            catch
            { return ""; }
        }

        public static string MsgStatus24Hour()
        {
            try
            {
                string jsonDataTemplte = "#SUSS_ACCESS#,#FAIL_ACCESS#", status24HourSql = "", sqlcondition = "";
                int SUSS = 0, FAIL = 0;
                sqlcondition = "`pingDatetime` >= '" + ToTimeStamp(-60 * 24).ToString() + "' AND `pingDatetime` <= '" + ToTimeStamp(0).ToString() + "'";
                status24HourSql = "SELECT SUM(`pingSuss`),SUM(`pingFail`) FROM `pingCheckLog` WHERE " + sqlcondition;
                DataSet SUMS24hJson = SQLDB.SelectToDataSet(getConn(), status24HourSql, "pinghistory");
                SUSS = Convert.ToInt32(SUMS24hJson.Tables[0].Rows[0][0]);
                jsonDataTemplte = jsonDataTemplte.Replace("#SUSS_ACCESS#", SUSS.ToString());

                FAIL += Convert.ToInt32(SUMS24hJson.Tables[0].Rows[0][1]);
                jsonDataTemplte = jsonDataTemplte.Replace("#FAIL_ACCESS#", FAIL.ToString());
                accessSussPercent = ((SUSS / (Convert.ToDouble(SUSS + FAIL))) * 100).ToString("f2");
                accessFailPercent = ((FAIL / (Convert.ToDouble(SUSS + FAIL))) * 100).ToString("f2");
                return jsonDataTemplte.Replace("#SUSS_ACCESS#", "0").Replace("#FAIL_ACCESS#", "0");
            }
            catch
            { return ""; }
        }

        public static string countDownByMonth(int CountDownTimes, bool isExcel = false)
        {
            string jsonData = "";
            for (int i = 0; i < CountDownTimes; i++)
            {
                if (isExcel)
                {
                    jsonData += "'" + nowTime.AddMonths(-i).ToString("yyyy-MM") + "月',";
                }
                else
                {
                    jsonData += "'" + nowTime.AddMonths(-i).ToString("MM") + "月',";
                }
            }
            return jsonData;
        }

        public static string countDownByDay(int CountDownTimes, bool isExcel = false)
        {
            string jsonData = "";
            for (int i = 0; i < CountDownTimes; i++)
            {
                if (isExcel)
                {
                    jsonData += "'" + nowTime.AddDays(-i).ToString("yyyy-MM-dd") + "',";
                }
                else
                {
                    jsonData += "'" + nowTime.AddDays(-i).ToString("MM-dd") + "',";
                }
            }
            return jsonData;
        }

        public static string countDownByHour(int CountDownTimes)
        {
            string jsonData = "";
            for (int i = 0; i < CountDownTimes; i++)
            {
                jsonData += "'" + nowTime.AddHours(-i - 1).ToString("HH") + "-" + nowTime.AddHours(-i).ToString("HH") + "点',";
            }
            return jsonData;
        }

        public static string countDownByMins(int CountDownTimes)
        {
            string jsonData = "";
            for (int i = 1; i < CountDownTimes; i++)
            {
                jsonData += "'" + nowTime.AddMinutes(-i).ToString("HH:mm") + "',";
            }
            return jsonData;
        }

        private static string ToTimeStamp(int timeSpan)
        {
            return nowTime.AddMinutes(timeSpan).ToString();

            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            //long timeStamp = (long)(nowTime.AddMinutes(timeSpan) - startTime).TotalMilliseconds; // 相差毫秒数
            //return timeStamp;
        }

        private static string getTimeStamp(DateTime giveTime)
        {
            return giveTime.ToString();

            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            //long timeStamp = (long)(giveTime - startTime).TotalMilliseconds; // 相差毫秒数
            //return timeStamp.ToString();
        }

        private static DateTime ToDateTime(long timeStr)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddMilliseconds(timeStr);
            return dt;
        }

        public void DataOutputToCSV(string dateType)
        {
            string DATES = "", DELAYS = "";
            string[] DATESArray = null, DELAYSArray = null;
            StringWriter sw = new StringWriter();
            switch (dateType)
            {
                case "day":
                    //读取日期数组
                    DATES = countDownByDay(365, true);
                    DATES = DATES.Substring(0, DATES.Length - 1);
                    DATESArray = DATES.Split(',');
                    //读取延时数组
                    DELAYS = getDelayStatus(365, "TOTAL_DELAY", "day");
                    DELAYS = DELAYS.Substring(0, DELAYS.Length - 1);
                    DELAYSArray = DELAYS.Split(',');
                    break;
                case "month":
                    //读取日期数组
                    DATES = countDownByMonth(12, true);
                    DATES = DATES.Substring(0, DATES.Length - 1);
                    DATESArray = DATES.Split(',');
                    //读取延时数组
                    DELAYS = getDelayStatus(12, "TOTAL_DELAY", "month");
                    DELAYS = DELAYS.Substring(0, DELAYS.Length - 1);
                    DELAYSArray = DELAYS.Split(',');
                    break;
            }

            sw.WriteLine("统计日期,总平均延时(毫秒)");
            //生成延时数据
            for (int i=0; i< DATESArray.Length; i++)
            {
                sw.WriteLine("\"" + DATESArray[i].Replace("'", "") + "\"," + DELAYSArray[i] + "ms");
            }
            
            sw.Close();
            string fileNameDATE = dateType == "day" ? "日" : "月";
            Response.AddHeader("Content-Disposition", "attachment; filename=按" + fileNameDATE + "导出平均延时_" + DateTime.Now.ToString("yyyyMMdd") + ".csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
        }
    }

}