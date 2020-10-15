using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;

namespace WebCheck
{
    class Program
    {
        public static System.Timers.Timer pingTimer = new System.Timers.Timer();
        public static System.Timers.Timer resortTimer = new System.Timers.Timer();
        static void Main(string[] args)
        {
            Console.WriteLine("### 成功启动网站监测探针服务...");
            /////////////////////////检测延时///////////////////////////
            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = Convert.ToDouble(1000 * Convert.ToInt32(ConfigurationManager.AppSettings["check_interval"]));    //可更改检测目录时间
            pingTimer.Elapsed += new System.Timers.ElapsedEventHandler(pingTimer_Elapsed);
            pingTimer.Enabled = true;
            pingTimer.AutoReset = true;
            /////////////////////////检测延时///////////////////////////

            /////////////////////////每整点整理数据///////////////////////////
            resortTimer = new System.Timers.Timer();
            resortTimer.Interval = Convert.ToDouble(1000 * 15);    //1小时
            resortTimer.Elapsed += new System.Timers.ElapsedEventHandler(resortTimer_Elapsed);
            resortTimer.Enabled = true;
            resortTimer.AutoReset = true;
            /////////////////////////每小时整理数据///////////////////////////

            Console.ReadLine();
        }

        static void pingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            checkPingData();
        }

        static void resortTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute == 0)   //检测到分为零整时
            {
                resortData();
            }
        }

        private static string getConn()
        {
            return ConfigurationManager.AppSettings["mysqlConn"];
        }

        static void checkPingData()
        {
            try
            {
                ProcessStartInfo start = new ProcessStartInfo("curl.exe");//设置运行的命令行程序，不在系统环境变量的要输入完整路径
                start.Arguments = "-o /dev/null --connect-timeout 10 --max-time 10 -s -k -w httpcode=%{http_code},namelookup=%{time_namelookup},connect=%{time_connect},appconnect=%{time_appconnect},pretransfer=%{time_pretransfer},starttransfer=%{time_starttransfer},total=%{time_total} " + ConfigurationManager.AppSettings["website_URL"];

                start.CreateNoWindow = true;//不显示dos命令行窗口
                start.RedirectStandardOutput = true;//标准输出
                start.RedirectStandardInput = true;//标准输入
                start.RedirectStandardError = true;//错误输出
                start.UseShellExecute = false;// //指定不使用系统的外壳程序，而是直接启动被调用程序本身

                Process p = Process.Start(start);
                StreamReader reader = p.StandardOutput;//截取输出流

                string line = reader.ReadLine();//每次读取一行
                formatData(line);
                //Console.WriteLine(line + "\n");

                p.WaitForExit();    //等待程序执行完退出进程
                p.Close(); 
                reader.Close();
            }
            catch { }
        }

        private static void formatData(string lineData)
        {
            try
            {
                string httpcode = "", namelookup = "", connect = "", appconnect = "", pretransfer = "", starttransfer = "", total_delayTime = "";
                string tcptime = "", ssltime = "", servertime = "", toclienttime = "";
                if (!String.IsNullOrEmpty(lineData))
                {
                    string[] networkStatusList = lineData.Split(',');
                    httpcode = networkStatusList[0].ToString().Replace("httpcode=", "");
                    namelookup = (Convert.ToDouble(networkStatusList[1].ToString().Replace("namelookup=", "")) * 1000).ToString("f0");
                    connect = (Convert.ToDouble(networkStatusList[2].ToString().Replace("connect=", "")) * 1000).ToString("f0"); ;
                    appconnect = (Convert.ToDouble(networkStatusList[3].ToString().Replace("appconnect=", "")) * 1000).ToString("f0"); ;
                    pretransfer = (Convert.ToDouble(networkStatusList[4].ToString().Replace("pretransfer=", "")) * 1000).ToString("f0"); ;
                    starttransfer = (Convert.ToDouble(networkStatusList[5].ToString().Replace("starttransfer=", "")) * 1000).ToString("f0"); ;
                    total_delayTime = (Convert.ToDouble(networkStatusList[6].ToString().Replace("total=", "")) * 1000).ToString("f0");
                    string testTime = DateTime.Now.ToString();
                    if (httpcode != "200")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[" + testTime + "] 网站服务不可用：当前为HTTP " + httpcode + "状态");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        tcptime = (Convert.ToInt32(connect) - Convert.ToInt32(namelookup)).ToString();
                        if (appconnect != "0")
                        {
                            //进行SSL加密的appconnect不为0
                            ssltime = (Convert.ToInt32(appconnect) - Convert.ToInt32(connect)).ToString();
                        }
                        else
                        {
                            ssltime = "0";
                        }
                        servertime = (Convert.ToInt32(starttransfer) - Convert.ToInt32(pretransfer)).ToString();
                        toclienttime = (Convert.ToInt32(total_delayTime) - Convert.ToInt32(starttransfer)).ToString();
                        Console.WriteLine("[" + testTime + "] DNS查询：" + namelookup + " ms TCP握手：" + tcptime + " ms SSL耗时：" + ssltime + " ms 服务器耗时：" + servertime + " ms 发送耗时：" + toclienttime + " ms 总延时：" + total_delayTime + " ms");
                    }

                    string sql_command = "INSERT INTO `pingCheck`(pingTime,pingStatus,pingDelay,dnsTime,sslTime,tcpTime,servTime,transferTime)VALUES('" + testTime + "','" + httpcode + "'," + total_delayTime + "," + namelookup + "," + ssltime + "," + tcptime + "," + servertime + "," + toclienttime + ")";
                    SQLDB.ExecuteNonQuery(getConn(), sql_command);
                }
            }
            catch { }
        }

        private static void resortData()
        {
            try
            {
                int pingSussCount = 0, pingFailCount = 0;
                double _dnstime = 0.00, _ssltime = 0.00, _tcptime = 0.00, _servtime = 0.00, _transfertime = 0.00, _totaltime = 0.00;
                List<double> _dnstimeList = new List<double>();
                List<double> _ssltimeList = new List<double>();
                List<double> _tcptimeList = new List<double>();
                List<double> _servtimeList = new List<double>();
                List<double> _transfertimeList = new List<double>();
                List<double> _totaltimeList = new List<double>();
                DateTime resortTime = DateTime.Now.AddHours(-1);
                string query_log = "SELECT COUNT(*) FROM `pingCheckLog` WHERE `pingDatetime`='" + resortTime.ToString("yyyy-MM-dd HH:00:00") + "'";
                string get_pingdata = "SELECT * FROM `pingCheck` WHERE `pingTime` >= '" + resortTime.ToString("yyyy-MM-dd HH:00:00") + "' AND `pingTime` <= '" + resortTime.ToString("yyyy-MM-dd HH:59:59") + "'";
                int hasThisRec = Convert.ToInt32(SQLDB.ExecuteScalar(getConn(), query_log));

                if (hasThisRec == 0)
                {
                    DataSet tmpPingData_ds = SQLDB.SelectToDataSet(getConn(), get_pingdata, "pingHistory");
                    if (tmpPingData_ds != null && tmpPingData_ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmpPingData_ds.Tables[0].Rows.Count; i++)
                        {
                            if (tmpPingData_ds.Tables[0].Rows[i]["pingStatus"].ToString() == "200")   //可访问性检测
                            {
                                _dnstimeList.Add(Convert.ToDouble(tmpPingData_ds.Tables[0].Rows[i]["dnsTime"].ToString()));  //可访问时才记录延时
                                _ssltimeList.Add(Convert.ToDouble(tmpPingData_ds.Tables[0].Rows[i]["sslTime"].ToString()));
                                _tcptimeList.Add(Convert.ToDouble(tmpPingData_ds.Tables[0].Rows[i]["tcpTime"].ToString()));
                                _servtimeList.Add(Convert.ToDouble(tmpPingData_ds.Tables[0].Rows[i]["servTime"].ToString()));
                                _transfertimeList.Add(Convert.ToDouble(tmpPingData_ds.Tables[0].Rows[i]["transferTime"].ToString()));
                                _totaltimeList.Add(Convert.ToDouble(tmpPingData_ds.Tables[0].Rows[i]["pingDelay"].ToString()));
                                pingSussCount++;
                            }
                            else
                            {
                                pingFailCount++;
                            }
                        }
                        _dnstime = _dnstimeList.Average();
                        _ssltime = _ssltimeList.Average();
                        _tcptime = _tcptimeList.Average();
                        _servtime = _servtimeList.Average();
                        _transfertime = _transfertimeList.Average();
                        _totaltime = _totaltimeList.Average();
                        string insert_pingHistory = "INSERT INTO `pingCheckLog`(pingDatetime,pingSuss,pingFail,dnsTime,sslTime,tcpTime,servTime,transferTime,totalTime)VALUES('" + resortTime.ToString("yyyy-MM-dd HH:00:00") + "'," + pingSussCount + "," + pingFailCount + "," + _dnstime + "," + _ssltime + "," + _tcptime + "," + _servtime + "," + _transfertime + "," + _totaltime + ")";
                        SQLDB.ExecuteNonQuery(getConn(), insert_pingHistory);
                    }
                }
                string clearPingdata_sql = "DELETE FROM `pingCheck` WHERE pingTime<='" + DateTime.Now.AddHours(-4).ToString() + "'";//清除4小时前的详细记录
                SQLDB.ExecuteNonQuery(getConn(), clearPingdata_sql);
            }
            catch { }
        }
    }
}
