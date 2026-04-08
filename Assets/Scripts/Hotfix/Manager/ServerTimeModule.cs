
using System;
using System.Globalization;
using Hotfix;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class ServerTimeModule : TSingleton<ServerTimeModule>
    {
        private Int64 Lose_Time = 0;

        public long Ping = 0;

        private long LostMaintentTime = 0;

        private DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
        public Int64 m_serverZone = 0;
        public Int64 m_cacheStamp = 0;
        public DateTime m_cacheDateTime;
        public Int64 m_tmpServerTime;

        private Int64 m_lastDayNightTime = 0;

        private decimal m_decimalTime = 0;

        public void UpdateServerTime(Int64 serverTime, Int64 clientTime)
        {
            if (clientTime == 0)
            {
                clientTime = GetTicks();
            }
            //Debug.LogErrorFormat("同步前serverTime：{0} 同步localServertime：{1}", serverTime, GetServerTime());
            Int64 local_time = GetTicks();

            // 毫秒
            long diffTime = (local_time - clientTime) / 10000;

            Ping = diffTime / 2;
            Lose_Time = serverTime + Ping - clientTime/10000;

            Debug.Log($"Lose_Time:{Lose_Time} server:{serverTime} client:{clientTime / 10000}");

            //Debug.LogErrorFormat("local_time:{0} clientTime:{1} Ping:{2} Lose_Time:{3} serverTime{4} localServerTime:{5} TimeScene:{6} timestamp:{7} 现在时间:{8}",
            //    local_time, clientTime, Ping, Lose_Time, serverTime, GetServerTime(), Time.realtimeSinceStartup, GetTimestamp(), DateTime.Now.ToString());

            //if ((local_time/ 10000000)- m_lastDayNightTime>60)
            //{
            //    m_lastDayNightTime = local_time/ 10000000;
            //    AppFacade.GetInstance().SendNotification(CmdConstant.DayNightTimeTick);
            //}

            if (Mathf.Abs(serverTime - GetServerTimeMilli()) >= 1000)
            {
                Debug.LogFormat("心跳误差太大：{0}秒", serverTime - GetServerTime());
                var net = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                net.SetHeartSend();
            }
        }

        public long GetServerTime()
        {
            return GetServerTimeMilli() / 1000;
        }
        public long GetServerTimeMilli()
        {
            return GetTicks() / 10000 + Lose_Time;
        }

        // 获取本地时间戳
        public Int64 GetTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public Int64 GetTicks()
        {
            return DateTime.Now.ToUniversalTime().Ticks - 621355968000000000;
        }

        public void Execute(object body)
        {
            Role_Heart.response data = body as Role_Heart.response;
            UpdateServerTime(data.serverTime, data.clientTime);
        }

        public void UpdateMaintentTime(Int64 serverTime, Int64 clientTime)
        {
            long local_time = GetTimestamp();
            long Ping = (long)Math.Floor((float)(local_time - clientTime) / 2);
            LostMaintentTime = serverTime + Ping - local_time;
        }

        public DateTime GetMaintentTime()
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(GetTimestamp()+ LostMaintentTime));
        }

        #region 日期转换

        // 设置服务器时区
        public void SetServerZone(Int64 serverZone)
        {
            Debug.LogFormat("时区：{0}", serverZone);
            m_serverZone = serverZone;
        }

        //获取当前服务器日期时间
        public DateTime GetCurrServerDateTime()
        {
            m_tmpServerTime = ServerTimeModule.Instance.GetServerTime();
            if (m_cacheStamp != m_tmpServerTime)
            {
                m_cacheStamp = m_tmpServerTime;
                m_cacheDateTime = ConverToServerDateTime(m_tmpServerTime);
            }
            return m_cacheDateTime;
        }

        //转化为服务器日期时间
        public DateTime ConverToServerDateTime(Int64 timeStamp)
        {
            var now = DateTime.Now;
            string zone = now.ToString("%z");
            int zoneInt = int.Parse(zone);
            if(now.IsDaylightSavingTime()) //判断当前是否是夏令时 如果是夏令时减一个小时
            {
                zoneInt -= 1;
            }
            //算出时间戳对应的本地时区差
            Int64 zoneOffset = zoneInt * 3600;

            return dtStart.AddSeconds(timeStamp - zoneOffset + m_serverZone * 3600);
        }

        /// <summary>       
        /// 获取时区差
        /// </summary>       
        /// <param name=”timeStamp”></param>       
        /// <returns></returns>  
        private Int64 GetTimeZoneOffset(DateTime dt)
        {
            return TimeZone.CurrentTimeZone.GetUtcOffset(dt).Ticks;
        }

        /// <summary>
        /// DateTime时间格式转换为10位不带毫秒的Unix时间戳
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        private int ConverDateTimeToUnixTime(DateTime time)
        {
            // DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(oDateTime);
            return (int)(time - dtStart).TotalSeconds;
        }

        /// <summary>       
        /// 时间戳转为C#格式时间timeStamp=1530779666   
        /// </summary>       
        /// <param name=”timeStamp”></param>       
        /// <returns></returns>      
        public DateTime ConvertToDateTime(Int64 timeStamp)
        {
            // DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(oDateTime);
            //long lTime = long.Parse(string.Format("{0}0000000", timeStamp));
            //TimeSpan toNow = new TimeSpan(lTime);
            //return dtStart.Add(toNow);
            return dtStart.AddSeconds(timeStamp);
        }

        //获取距离0点的秒数
        public Int64 GetDistanceZeroTime()
        {
            DateTime time1 = GetCurrServerDateTime();
            Int64 times = time1.Hour * 3600 + time1.Minute * 60 + time1.Second;
            return 86400 - times;
        }

        //到下周日零点的秒数0123456
        public Int64 GetNextSundayTime(int endDayOfWeek = 0)
        {
            DateTime time1 = GetCurrServerDateTime();
            var dayOfWeek = Convert.ToInt32(time1.DayOfWeek);
            if (dayOfWeek < endDayOfWeek)
            {
                dayOfWeek += 7;
            }
            int remainDay = 7+ endDayOfWeek - (dayOfWeek);
            
            Int64 times = time1.Hour * 3600 + time1.Minute * 60 + time1.Second;
            return 86400 * remainDay - times;
        }
        
        //到这个月最后一天零点的秒数
        public Int64 GetNextMonthOneTime()
        {
            DateTime time1 = GetCurrServerDateTime();
            Int64 times = time1.Day * 86400 + time1.Hour * 3600 + time1.Minute * 60 + time1.Second;
            return 86400 * DateTime.DaysInMonth(time1.Year,time1.Month) - times;
        }

        #endregion

    }
}

