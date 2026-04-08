// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    ActivityProxy 活动
// Copyright IGG All rights reserved.
// ===============================================================================

using Client;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {

    public class ActivityBehaviorData
    {
        public int Id;      //模版id
        public int Count;   //总进度
        public int PlayerBehavior;
        public int Condition;
        public int data1;
    }

    public class ActivityScheduleData
    {
        public Int64 ActivityId;
        public Activity Info;

        //前置活动进度数据
        public Int64 PreActivityId;     //前置活动ID  用于获取活动进度
        private ActivityScheduleData m_preAcitivitySchedule;

        //红点相关
        private int m_reddotNum;
        public bool IsReddotChange;
        private int m_lastReddotNum;

        //已领取奖励字典映射
        public bool IsRewardIdChange = true;
        private Dictionary<Int64, bool> m_rewardIdDic;

        //活动对应的子行为列表
        public bool IsBehaviorChange;
        private List<ActivityBehaviorData> m_behaviorList;

        //1宝箱型 2过期型 3兑换型 4掉落型 5最强执政官 6创角活动 7幸运转盘
        public int ActivityType;
        //1宝箱领取状态 2宝箱倒计时状态 3没过期 4已过期 5兑换红点不提示(物品变更 需要切换到提醒状态)
        public int ActivityType2;

        private void InitBehaviorList()
        {
            if (!IsBehaviorChange)
            {
                return;
            }
            var activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)PreActivityId);
            if (define.activityType == 100 || define.activityType == 101)
            {
                //开服活动
                m_behaviorList = activityProxy.GetChildBehaviorList1(PreActivityId);
            }
            else if (define.activityType == 400 || define.activityType == 401 || define.activityType == 402 || define.activityType == 403)
            {
                //基础达标类型(通用条件达成) 
                ReadPreActivityData();
                m_behaviorList = activityProxy.GetChildBehaviorList2(PreActivityId, m_preAcitivitySchedule.Info.level);
            }
            else if (define.activityType == 200)
            {
                //兑换类活动
                m_behaviorList = activityProxy.GetChildBehaviorList3(PreActivityId);
            }
            else if (define.activityType == 500 || define.activityType == 501 || define.activityType == 502)
            {
                //掉落类活动
                ReadPreActivityData();
                m_behaviorList = activityProxy.GetChildBehaviorList4(PreActivityId);
            }
            else if (define.activityType == 301)
            {
                //最强执政官
                if (ActivityId == PreActivityId)
                {
                    ReadPreActivityData();
                    m_behaviorList = activityProxy.GetChildBehaviorList5(PreActivityId, (int)Info.stage, (int)m_preAcitivitySchedule.Info.level);
                }
                else
                {
                    m_behaviorList = new List<ActivityBehaviorData>();
                }
            }
            else if (define.activityType == 800)
            {
                //地狱活动
                m_behaviorList = activityProxy.GetChildBehaviorList6(Info.ids);
            }
            else if (define.activityType == 303 || define.activityType == 304 || define.activityType == 305 || define.activityType == 306)
            {
                ReadPreActivityData();
                m_behaviorList = activityProxy.GetChildBehaviorList7(PreActivityId, 0);
            }
            else if (define.activityType == 1000)//创角活动
            {
                m_behaviorList = activityProxy.GetChildBehaviorList8(ActivityId);
            }
            else
            {
                m_behaviorList = new List<ActivityBehaviorData>();
            }
            IsBehaviorChange = false;
        }

        private void InitRewardIdDic()
        {
            if (IsRewardIdChange)
            {
                if (m_rewardIdDic == null)
                {
                    m_rewardIdDic = new Dictionary<Int64, bool>();
                }
                else
                {
                    m_rewardIdDic.Clear();
                }
                if (Info != null)
                {
                    if (Info.rewardId == null)
                    {
                        Info.rewardId = new List<Int64>();
                    }
                    for (int i = 0; i < Info.rewardId.Count; i++)
                    {
                        m_rewardIdDic[Info.rewardId[i]] = true;
                    }
                }
                //ClientUtils.Print(m_rewardIdDic);
                IsRewardIdChange = false;
            }
        }

        private void InitExchangeItem()
        {
            if (m_rewardIdDic == null)
            {
                m_rewardIdDic = new Dictionary<Int64, bool>();

                InitBehaviorList();
                var activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                for (int i = 0; i < m_behaviorList.Count; i++)
                {
                    //判断是否满足兑换条件
                    ActivityConversionTypeDefine define = activityProxy.GetConversionTypeDefine(m_behaviorList[i].Id);
                    if (define.conversionItem != null && define.conversionItem.Count >0)
                    {
                        for (int k = 0; k < define.conversionItem.Count; k++)
                        {
                            m_rewardIdDic[define.conversionItem[k]] = true;
                        }
                    }
                }
            }
        }

        //统计红点数量
        private void CalReddotNum()
        {
            if (ActivityType == 1) // 宝箱型
            {
                if (ActivityType2 == 1)
                {
                    ReadPreActivityData();
                    if (m_preAcitivitySchedule.Info != null && m_preAcitivitySchedule.Info.rewardId.Count == 0)
                    {
                        m_reddotNum = 0;
                    }
                    else
                    {
                        m_reddotNum = Info.rewardBox ? 0 : 1;
                    }
                }
                else
                {
                    ReadPreActivityData();
                    m_reddotNum = m_preAcitivitySchedule.GetCanReceiveNum();
                }
            }
            else if (ActivityType == 2) //过期型
            {
                if (ActivityType2 == 4) //已过期
                {
                    m_reddotNum = 0;
                    return;
                }

                ReadPreActivityData();
                m_reddotNum = m_preAcitivitySchedule.GetCanReceiveNum();
            }
            else if (ActivityType == 3) //兑换型
            {
                m_reddotNum = CalExchangeRedpot();
            }
            else if (ActivityType == 4) //掉落型
            {
                m_reddotNum = 0;
            }
            else if (ActivityType == 5) //最强执政官 地狱活动
            {
                m_reddotNum = GetCanReceiveNum2();
            }
            else if (ActivityType == 6)//创角活动
            {
                m_reddotNum = GetCanReceiveNum3();
            }
            else if (ActivityType == 7) //幸运转盘
            {
                m_reddotNum = GetCanReceiveNum4();
            }
        }

        //计算兑换类红点
        public int CalExchangeRedpot(bool isForceCal = false)
        {
            InitBehaviorList();

            var activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;

            if (!isForceCal)
            {
                //判断一下是否需要计算红点
                if (!activityProxy.IsExchangeRemind())
                {
                    return 0;
                }

                if (ActivityType2 == 5)
                {
                    return 0;
                }
            }

            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

            int vipLevel = (int)playerProxy.GetVipLevel();
            int count = 0;
            ActivityBehaviorData behaviorData = null;
            for (int i = 0; i < m_behaviorList.Count; i++)
            {
                behaviorData = m_behaviorList[i];
                //判断vip等级 
                if (vipLevel < behaviorData.PlayerBehavior)
                {
                    continue;
                }
                //判断兑换次数是否已满
                if (Info.exchange != null && Info.exchange.ContainsKey(behaviorData.Id))
                {
                    if (Info.exchange[behaviorData.Id].count >= behaviorData.Count)
                    {
                        continue;
                    }
                }
                //判断是否满足兑换条件
                ActivityConversionTypeDefine define = activityProxy.GetConversionTypeDefine(behaviorData.Id);
                if (define.conversionItem != null && define.num!=null)
                {
                    if (define.conversionItem.Count == define.num.Count)
                    {
                        int total = define.conversionItem.Count;
                        int enoughCount = 0;
                        for (int k = 0; k < total; k++)
                        {
                            Int64 itemCount = bagProxy.GetItemNum(define.conversionItem[k]);
                            if (itemCount >= define.num[k])
                            {
                                enoughCount = enoughCount + 1;
                            }
                        }
                        if (enoughCount >= total)
                        {
                            count = count + 1;
                        }
                    }
                }
            }
            m_lastReddotNum = count;
            return count;
        }

        //获取可领取奖励数量
        private int GetCanReceiveNum()
        {
            InitBehaviorList();
            InitRewardIdDic();

            Dictionary<Int64, Activity.ScheduleInfo> scheduleDic = Info.scheduleInfo;
            int count = 0;
            ActivityBehaviorData bData = null;
            for (int i = 0; i < m_behaviorList.Count; i++)
            {
                bData = m_behaviorList[i];
                if (m_rewardIdDic.ContainsKey(bData.Id))
                {
                    continue;
                }
                if (scheduleDic != null && scheduleDic.ContainsKey(bData.PlayerBehavior))
                {
                    if (scheduleDic[bData.PlayerBehavior].data != null && scheduleDic[bData.PlayerBehavior].data.ContainsKey(bData.Condition))
                    {
                        if (scheduleDic[bData.PlayerBehavior].data[bData.Condition].count >= bData.Count)
                        {
                            count = count + 1;
                        }
                    }
                }
            }
            return count;
        }

        //最强执政官红点计算
        private int GetCanReceiveNum2()
        {
            InitBehaviorList();
            int count = 0;
            ActivityBehaviorData bData = null;
            for (int i = 0; i < m_behaviorList.Count; i++)
            {
                bData = m_behaviorList[i];

                if (Info.rewards != null && Info.rewards.ContainsKey(bData.PlayerBehavior))
                {
                    continue;
                }
                else
                {
                    if (Info.score >= bData.Count)
                    {
                        count = count + 1;
                    }
                }
            }
            return count;
        }

        //创角活动红点计算
        private int GetCanReceiveNum3()
        {
            InitBehaviorList();
            InitRewardIdDic();

            var activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            var timeInfo = activityProxy.GetActivityById(Info.activityId);
            if (timeInfo == null)
            {
                return 0;
            }

            int diffDay = activityProxy.GetDiffDay(timeInfo.startTime, ServerTimeModule.Instance.GetServerTime());
            if (diffDay < 0)
            {
                diffDay = 1;
            }

            int count = 0;
            Dictionary<Int64, Activity.ScheduleInfo> scheduleDic = Info.scheduleInfo;
            ActivityBehaviorData bData = null;
            for (int i = 0; i < m_behaviorList.Count; i++)
            {
                bData = m_behaviorList[i];
                if (bData.data1 != diffDay)
                {
                    continue;
                }
                if (m_rewardIdDic.ContainsKey(bData.Id))
                {
                    continue;
                }
                if (scheduleDic != null && scheduleDic.ContainsKey(bData.PlayerBehavior))
                {
                    if (scheduleDic[bData.PlayerBehavior].data != null && scheduleDic[bData.PlayerBehavior].data.ContainsKey(bData.Condition))
                    {
                        if (scheduleDic[bData.PlayerBehavior].data[bData.Condition].count >= bData.Count)
                        {
                            count = count + 1;
                        }
                    }
                }
            }

            //活跃度奖励 红点计算
            if (!Info.activeReward)//奖励未领取
            {
                int id = (int)Info.activityId * 100 + diffDay;
                ActivityNewPlayerDefine define = CoreUtils.dataService.QueryRecord<ActivityNewPlayerDefine>(id);
                if (define != null)
                {
                    var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                    if (playerProxy != null)
                    {
                        if (playerProxy.CurrentRoleInfo.activityActivePoint >= define.standard)
                        {
                            count = count + 1;
                        }
                    }
                }
            }

            return count;
        }

        //幸运转盘红点计算
        private int GetCanReceiveNum4()
        {
            return (Info.free > 0) ? 0 : 1;
        }

        private void ReadPreActivityData()
        {
            if (m_preAcitivitySchedule == null)
            {
                var activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_preAcitivitySchedule = activityProxy.GetActivitySchedule(PreActivityId);
            }
        }

        #region 外部使用

        //更新积分
        public void UpdateScore(Int64 Score)
        {
            Info.score = Score;
            IsReddotChange = true;
        }

        //更新行为进度
        public void UpdateScheduleVal(Int64 behaviorType, Int64 condition, Int64 count)
        {
            Info.scheduleInfo[behaviorType].data[condition].count = count;
            IsReddotChange = true;
        }

        //更新已领取列表
        public void UpdateRewardList(Int64 eventId)
        {
            if (ActivityType == 5)
            {
                if (Info.rewards == null)
                {
                    Info.rewards = new Dictionary<long, Activity.Rewards>();
                }
                Activity.Rewards rewards = new Activity.Rewards();
                rewards.index = eventId;
                Info.rewards[eventId] = rewards;
            }
            else
            {
                InitRewardIdDic();
                if (!m_rewardIdDic.ContainsKey(eventId))
                {
                    Info.rewardId.Add(eventId);
                    m_rewardIdDic[eventId] = true;
                }
            }
            IsReddotChange = true;
        }

        //更新宝箱领取状态
        public void UpdateRewardBoxStatus()
        {
            Info.rewardBox = true;
            IsReddotChange = true;
        }

        //是否已兑换
        public bool IsExchangeItem(int itemId)
        {
            InitExchangeItem();
            if (m_rewardIdDic.ContainsKey(itemId))
            {
                return true;
            }
            return false;
        }

        //是否已领取
        public bool IsReceive(Int64 acEventId)
        {
            InitRewardIdDic();
            if (m_rewardIdDic.ContainsKey(acEventId))
            {
                return true;
            }
            return false;
        }

        //是否已领取（最强执政官）
        public bool IsReceive2(int index)
        {
            if (Info.rewards != null)
            {
                if (Info.rewards.ContainsKey(index))
                {
                    return true;
                }
            }
            return false;
        }

        //获取红点数量
        public int GetReddotNum()
        {
            if (IsReddotChange)
            {
                CalReddotNum();
                IsReddotChange = false;
            }
            return m_reddotNum;
        }

        //获取上一次的红点数量
        public int GetLastReddotNum()
        {
            return m_lastReddotNum;
        }

        //获取行为进度
        public int GetBehaviorSchedule(int playerBehavior, int condition)
        {
            ReadPreActivityData();
            Activity tempInfo = m_preAcitivitySchedule.Info;
            if (tempInfo.scheduleInfo != null && tempInfo.scheduleInfo.ContainsKey(playerBehavior) &&
                tempInfo.scheduleInfo[playerBehavior].data != null && tempInfo.scheduleInfo[playerBehavior].data.ContainsKey(condition))
            {
                return (int)tempInfo.scheduleInfo[playerBehavior].data[condition].count;
            }
            return 0;
        }

        //获取活动的行为列表
        public List<ActivityBehaviorData> GetBehaviorList()
        {
            InitBehaviorList();
            return m_behaviorList;
        }

        //获取活动进度数据
        public ActivityScheduleData GetScheduleData()
        {
            ReadPreActivityData();
            return m_preAcitivitySchedule;
        }

        //是否是新活动
        public bool IsNewActivity()
        {            
            return Info.isNew;
        }

        #endregion 
    }

    public class ActivityCalendarData
    {
        public ActivityTimeInfo ActivityInfo;
        public int StartWeekDay;
        public int EndWeekDay;
    }

    public class ActivityDetialData
    {
        public Int64 AcivityId;
        public ActivityScheduleData ScheduleData;
        public Dictionary<int, Dictionary<int, List<int>>> DataDic;
    }

    public class ActivityProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "ActivityProxy";

        private PlayerProxy m_playerProxy;
        private BagProxy m_bagProxy;

        private Dictionary<int, ActivityDetialData> m_detailDic = new Dictionary<int, ActivityDetialData>();
        private Dictionary<Int64, ActivityScheduleData> m_scheduleDic = new Dictionary<Int64, ActivityScheduleData>();

        private bool m_isReadSchedule;

        private bool m_isDelaying;

        private bool m_isDispose;

        //兑换红点是否提醒
        private bool m_isExchangeRemind;

        //兑换类活动列表
        private List<ActivityTimeInfo> m_exchangeActivityList = new List<ActivityTimeInfo>();

        private List<Int64> m_itemChangeList = new List<Int64>();

        public static int CreateRoleActivityId = 100001;

        private bool m_isCalendarReddotChange = true;  //日历红点是否变更
        private int m_calendarReddotCount;

        private int m_lastSelectActivityId = -1;

        #endregion

        // Use this for initialization
        public ActivityProxy(string proxyName)
            : base(proxyName)
        {

        }

        public override void OnRegister()
        {
            Debug.Log(" ActivityProxy register");
        }


        public override void OnRemove()
        {
            m_isDispose = true;
            Debug.Log(" ActivityProxy remove");
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            int remind = PlayerPrefs.GetInt(TipRemindProxy.ExchangeActivityRemind);
            SetExchangeRemindStatus(remind, false);
        }

        public void SetExchangeRemindStatus(int remind, bool isSave)
        {
            if (remind < 2)
            {
                m_isExchangeRemind = true;
            }
            else
            {
                m_isExchangeRemind = false;
            }
            if (isSave)
            {
                PlayerPrefs.SetInt(TipRemindProxy.ExchangeActivityRemind, remind);
            }
        }

        public bool IsExchangeRemind()
        {
            return m_isExchangeRemind;
        }

        //活动是否开启
        public bool IsOpen(int activityId)
        {
            Dictionary<Int64, ActivityTimeInfo> dic = m_playerProxy.GetActivityTimeInfo();
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            if (dic.ContainsKey(activityId))
            {
                if (serverTime >= dic[activityId].startTime && serverTime <= dic[activityId].endTime)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsOpenByGroup(int activityType)
        {
            bool isOpen = false;
            Dictionary<Int64, ActivityTimeInfo> dic = m_playerProxy.GetActivityTimeInfo();
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            dic.Values.ToList().ForEach((temp) => {
                if (serverTime >= temp.startTime && serverTime <= temp.endTime)
                {
                    ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)temp.activityId);
                    if (define != null)
                    {
                        if (define.group == activityType)
                        {
                            isOpen = true;
                        }
                    }
                }
            });
            return isOpen;
        }

        public List<ActivityTimeInfo> GetActivityMenuList()
        {
            Dictionary<Int64, ActivityTimeInfo> list = m_playerProxy.GetActivityTimeInfo();

            //测试数据
            //ActivityTimeInfo testInfo = new ActivityTimeInfo();
            //testInfo.activityId = 60001;
            //testInfo.startTime = ServerTimeModule.Instance.GetServerTime();
            //testInfo.endTime = testInfo.startTime + 3600 * 24;
            //list[testInfo.activityId] = testInfo;

            List<ActivityTimeInfo> newList = new List<ActivityTimeInfo>();
            foreach (var data in list)
            {
                ActivityTimeInfo info = data.Value;
                if (info.activityId != CreateRoleActivityId && IsShowAcitivity((int)info.activityId, info))
                {
                    newList.Add(info);
                }
            }
            return newList;
        }

        public bool IsShowAcitivity(int activityId, ActivityTimeInfo timeInfo)
        {
            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>(activityId);
            if (define != null && define.timeType == 4)
            {
                ActivityScheduleData scheduleData = GetActivitySchedule(activityId);
                if (scheduleData != null)
                {
                    bool isBool = true;
                    List<ActivityBehaviorData> behaviorList = scheduleData.GetBehaviorList();
                    for (int i = 0; i < behaviorList.Count; i++)
                    {
                        if (!scheduleData.IsReceive(behaviorList[i].Id))
                        {
                            isBool = false;
                            break;
                        }
                    }
                    if (isBool)
                    {
                        return false;
                    }
                }
            }

            ActivityTimeInfo info = timeInfo;
            if (info == null)
            {
                Dictionary<Int64, ActivityTimeInfo> dic = m_playerProxy.GetActivityTimeInfo();
                dic.TryGetValue(activityId, out info);
                if (info == null)
                {
                    return false;
                }
            }

            if (info.startTime < 0) //永久
            {
                return true;
            }

            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            if (serverTime >= info.startTime && serverTime < info.endTime)
            {
                return true;
            }

            return false;
        }

        public List<ActivityCalendarData> GetActivityDateList()
        {
            Int64 startWeekTime = GetWeekStartTime();
            Int64 endWeekTime = GetWeekEndTime(startWeekTime);

            //Debug.LogError("开始：" + ServerTimeModule.Instance.ConvertToDateTime(startWeekTime));

            Dictionary<Int64, ActivityTimeInfo> list = m_playerProxy.GetActivityTimeInfo();
            List<ActivityCalendarData> newList = new List<ActivityCalendarData>();
            foreach (var data in list)
            {
                ActivityTimeInfo info = data.Value;
                if (info.startTime < 0) //永久存在不需要显示
                {
                    continue;
                }
                if (info.startTime >= startWeekTime && info.startTime <= endWeekTime ||
                    info.endTime >= startWeekTime && info.endTime <= endWeekTime)
                {
                    if (info.startTime > info.endTime)
                    {
                        Debug.LogErrorFormat("startTime:{0} 大于 endTime:{1} {2} {3}",
                                            info.startTime,
                                            info.endTime,
                                            ServerTimeModule.Instance.ConverToServerDateTime(info.startTime),
                                            ServerTimeModule.Instance.ConverToServerDateTime(info.endTime));
                        continue;
                    }

                    ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)info.activityId);
                    if (define != null && define.ifShow == 1)
                    {
                        ActivityCalendarData tData = new ActivityCalendarData();
                        tData.ActivityInfo = info;

                        int startWeekDay = -1;
                        if (info.startTime >= startWeekTime && info.startTime <= endWeekTime)
                        {
                            startWeekDay = (int)ServerTimeModule.Instance.ConverToServerDateTime(info.startTime).DayOfWeek;
                        }
                        int endWeekDay = -1;
                        if (info.endTime >= startWeekTime && info.endTime <= endWeekTime)
                        {
                            endWeekDay = (int)ServerTimeModule.Instance.ConverToServerDateTime(info.endTime).DayOfWeek;
                        }

                        if (startWeekDay == -1 && endWeekDay >= 0)
                        {
                            startWeekDay = 0;
                        }
                        else if (startWeekDay >= 0 && endWeekDay == -1)
                        {
                            endWeekDay = 6;
                        }
                        if (startWeekDay > endWeekDay)
                        {
                            Debug.LogErrorFormat("startWeekDay:{0} 大于 endWeekDay:{1}", startWeekDay, endWeekDay);
                            continue;
                        }

                        tData.StartWeekDay = startWeekDay;
                        tData.EndWeekDay = endWeekDay;

                        newList.Add(tData);
                    }
                }
            }
            return newList;
        }

        public Int64 GetWeekStartTime()
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            DateTime dTime = ServerTimeModule.Instance.GetCurrServerDateTime();
            Int64 time = dTime.Hour * 3600 + dTime.Minute * 60 + dTime.Second;

            Int64 time1 = serverTime - time;
            Int64 startTime = 0;
            int week = (int)dTime.DayOfWeek;
            if (week == 0) //周日
            {
                startTime = time1;
                //endTime = time1 + 7 * 86400 - 1;
            }
            else//周一~周六
            {
                startTime = time1 - 86400 * week;
                //endTime = time1 + (7 - week) * 86400 - 1;
            }
            return startTime;
        }

        public Int64 GetWeekEndTime(Int64 startTime)
        {
            return startTime + (7 * 86400 - 1);
        }

        public ActivityTimeInfo GetActivityById(Int64 activityId)
        {
            Dictionary<Int64, ActivityTimeInfo> dic = m_playerProxy.GetActivityTimeInfo();
            ActivityTimeInfo info;
            dic.TryGetValue(activityId, out info);
            return info;
        }

        // 获取开服活动的子行为列表
        public List<ActivityBehaviorData> GetChildBehaviorList1(Int64 activityId)
        {
            List<ActivityBehaviorData> behaviorList = new List<ActivityBehaviorData>();
            int id = (int)activityId * 1000;
            ActivityDaysTypeDefine define2;
            for (int i = 1; i < 1000; i++)
            {
                define2 = GetDayTypeDefine(id + i);
                if (define2 != null)
                {
                    if (define2.data0 <= 0)
                    {
                        continue;
                    }
                    ActivityBehaviorData bData = new ActivityBehaviorData();
                    bData.Id = define2.ID;
                    bData.Count = define2.data0;
                    bData.PlayerBehavior = define2.playerBehavior;
                    bData.Condition = define2.data3;
                    behaviorList.Add(bData);
                }
                else
                {
                    break;
                }
            }
            return behaviorList;
        }

        // 获取活动子行为列表
        public List<ActivityBehaviorData> GetChildBehaviorList2(Int64 activityId, Int64 level)
        {
            int acId = (int)activityId;
            List<ActivityBehaviorData> behaviorList = new List<ActivityBehaviorData>();

            //优先读取级别为-1的数据
            int id = acId * 10000 + (-1 * 100);
            ActivityTargetTypeDefine define;
            for (int i = 1; i < 500; i++)
            {
                define = GetTargetTypeDefine(id + i);
                if (define == null)
                {
                    break;
                }
                else
                {
                    ActivityBehaviorData bData = new ActivityBehaviorData();
                    bData.Id = define.ID;
                    bData.Count = define.data0;
                    bData.PlayerBehavior = define.playerBehavior;
                    bData.Condition = define.data3;
                    behaviorList.Add(bData);
                }
            }

            //判断对应等级是否有数据 如果没有则取下一等级数据
            int findLevel = 0;
            if (level > 0)
            {
                int max = (int)level;
                for (int i = max; i > 0; i--)
                {
                    id = acId * 10000 + (i * 100) + 1;
                    define = GetTargetTypeDefine(id);
                    if (define != null)
                    {
                        findLevel = i;
                        break;
                    }
                }
            }

            if (findLevel > 0)
            {
                id = acId * 10000 + (findLevel * 100);
                for (int i = 1; i < 500; i++)
                {
                    define = GetTargetTypeDefine(id + i);
                    if (define == null)
                    {
                        break;
                    } else
                    {
                        ActivityBehaviorData bData = new ActivityBehaviorData();
                        bData.Id = define.ID;
                        bData.Count = define.data0;
                        bData.PlayerBehavior = define.playerBehavior;
                        bData.Condition = define.data3;
                        behaviorList.Add(bData);
                    }
                }
            }

            return behaviorList;
        }

        // 获取活动子行为列表
        public List<ActivityBehaviorData> GetChildBehaviorList3(Int64 activityId)
        {
            List<ActivityBehaviorData> behaviorList = new List<ActivityBehaviorData>();
            int id = (int)activityId * 1000;
            ActivityConversionTypeDefine define2;
            for (int i = 1; i < 1000; i++)
            {
                define2 = GetConversionTypeDefine(id + i);
                if (define2 != null)
                {
                    ActivityBehaviorData bData = new ActivityBehaviorData();
                    bData.Id = define2.ID;
                    bData.Count = define2.timeLimit;
                    bData.PlayerBehavior = define2.vipLimit;
                    bData.Condition = 0;
                    behaviorList.Add(bData);
                }
                else
                {
                    break;
                }
            }
            return behaviorList;
        }

        // 获取掉落类的子行为
        public List<ActivityBehaviorData> GetChildBehaviorList4(Int64 activityId)
        {
            List<ActivityBehaviorData> behaviorList = new List<ActivityBehaviorData>();
            int id = (int)activityId * 1000;
            ActivityDropTypeDefine define2;
            for (int i = 1; i < 1000; i++)
            {
                define2 = GetDropTypeDefine(id + i);
                if (define2 != null)
                {
                    ActivityBehaviorData bData = new ActivityBehaviorData();
                    bData.Id = define2.ID;
                    bData.Count = 0;
                    bData.PlayerBehavior = define2.playerBehavior;
                    bData.Condition = 0;
                    behaviorList.Add(bData);
                }
                else
                {
                    break;
                }
            }
            return behaviorList;
        }

        // 获取最强执政官的子行为
        public List<ActivityBehaviorData> GetChildBehaviorList5(Int64 activityId, int stage, int level)
        {
            List<ActivityBehaviorData> behaviorList = new List<ActivityBehaviorData>();
            int id = (int)activityId * 1000 + stage * 100 + level;
            ActivityKillTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityKillTypeDefine>(id);
            if (define.standard != null && define.standard.Count > 0)
            {
                int count = define.standard.Count;
                for (int i = 0; i < count; i++)
                {
                    ActivityBehaviorData bData = new ActivityBehaviorData();
                    bData.Id = id;
                    bData.Count = define.standard[i];
                    bData.PlayerBehavior = i + 1;
                    bData.Condition = 0;
                    behaviorList.Add(bData);
                }
            }
            return behaviorList;
        }

        // 获取最强执政官的子行为
        public List<ActivityBehaviorData> GetChildBehaviorList6(List<Int64> list)
        {
            int order = -1;
            ActivityInfernalDefine findDefine = null;
            List<ActivityBehaviorData> behaviorList = new List<ActivityBehaviorData>();
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ActivityInfernalDefine define = CoreUtils.dataService.QueryRecord<ActivityInfernalDefine>((int)list[i]);
                    if (define != null && order < define.order)
                    {
                        order = define.order;
                        findDefine = define;
                    }
                }
            }
            if (findDefine != null)
            {
                int total = 0;
                for (int i = 0; i < 3; i++)
                {
                    ActivityBehaviorData bData = new ActivityBehaviorData();
                    bData.Id = findDefine.ID;
                    bData.Count = findDefine.score[i];
                    bData.PlayerBehavior = i + 1;
                    bData.Condition = total;
                    behaviorList.Add(bData);
                    total = findDefine.score[i];
                }
            }
            return behaviorList;
        }

        // 获取活动子行为列表
        public List<ActivityBehaviorData> GetChildBehaviorList7(Int64 activityId, Int64 level)
        {
            int acId = (int)activityId;
            List<ActivityBehaviorData> behaviorList = new List<ActivityBehaviorData>();

            //优先读取级别为-1的数据
            int id = acId * 10000 + (-1 * 100);
            ActivityIntegralTypeDefine define;
            for (int i = 1; i < 500; i++)
            {
                define = GetIntegralTypeDefine(id + i);
                if (define == null)
                {
                    break;
                }
                else
                {
                    ActivityBehaviorData bData = new ActivityBehaviorData();
                    bData.Id = define.ID;
                    bData.Count = define.standard;
                    bData.PlayerBehavior = define.stage;
                    bData.Condition = 0;
                    behaviorList.Add(bData);
                }
            }

            //判断对应等级是否有数据 如果没有则取下一等级数据
            int findLevel = 0;
            if (level > 0)
            {
                int max = (int)level;
                for (int i = max; i > 0; i--)
                {
                    id = acId * 10000 + (i * 100) + 1;
                    define = GetIntegralTypeDefine(id);
                    if (define != null)
                    {
                        findLevel = i;
                        break;
                    }
                }
            }

            if (findLevel > 0)
            {
                id = acId * 10000 + (findLevel * 100);
                for (int i = 1; i < 500; i++)
                {
                    define = GetIntegralTypeDefine(id + i);
                    if (define == null)
                    {
                        break;
                    }
                    else
                    {
                        ActivityBehaviorData bData = new ActivityBehaviorData();
                        bData.Id = define.ID;
                        bData.Count = define.standard;
                        bData.PlayerBehavior = define.stage;
                        bData.Condition = 0;
                        behaviorList.Add(bData);
                    }
                }
            }

            return behaviorList;
        }

        // 获取创建活动的子行为列表
        public List<ActivityBehaviorData> GetChildBehaviorList8(Int64 activityId)
        {
            List<ActivityBehaviorData> behaviorList = new List<ActivityBehaviorData>();
            int id = (int)activityId * 1000;
            ActivityDaysTypeDefine define2;
            for (int i = 1; i < 1000; i++)
            {
                define2 = GetDayTypeDefine(id + i);
                if (define2 != null)
                {
                    if (define2.data0 <= 0)
                    {
                        continue;
                    }
                    ActivityBehaviorData bData = new ActivityBehaviorData();
                    bData.Id = define2.ID;
                    bData.Count = define2.data0;
                    bData.PlayerBehavior = define2.playerBehavior;
                    bData.Condition = define2.data3;
                    bData.data1 = define2.day;
                    behaviorList.Add(bData);
                }
                else
                {
                    break;
                }
            }
            return behaviorList;
        }

        //获取开服活动详情
        public ActivityDetialData GetOpenActivityDetail(Int64 activityId)
        {
            ActivityScheduleData scheduleData = GetActivitySchedule(activityId);

            if (scheduleData == null)
            {
                Debug.LogErrorFormat("GetOpenActivityDetail not find activityId:{0}", activityId);
            }

            ActivityDetialData detailData = new ActivityDetialData();
            detailData.AcivityId = activityId;
            detailData.ScheduleData = scheduleData;

            ActivityScheduleData realSchedule = scheduleData.GetScheduleData();
            if (realSchedule == null)
            {
                Debug.LogErrorFormat("realSchedule is null activityId:{0}", activityId);
            }

            //day-event  字典映射
            List<ActivityBehaviorData> behaviorList = realSchedule.GetBehaviorList();
            Dictionary<int, Dictionary<int, List<int>>> tempDic = new Dictionary<int, Dictionary<int, List<int>>>();
            ActivityDaysTypeDefine define2;
            for (int i = 0; i < behaviorList.Count; i++)
            {
                define2 = GetDayTypeDefine(behaviorList[i].Id);
                if (define2 != null)
                {
                    if (!tempDic.ContainsKey(define2.day))
                    {
                        tempDic[define2.day] = new Dictionary<int, List<int>>();
                    }
                    if (!tempDic[define2.day].ContainsKey(define2.paging))
                    {
                        tempDic[define2.day][define2.paging] = new List<int>();
                    }
                    tempDic[define2.day][define2.paging].Add(define2.ID);
                }
            }
            detailData.DataDic = tempDic;

            return detailData;
        }

        public ActivityScheduleData GetActivitySchedule(Int64 activityId)
        {
            ActivityScheduleData tData = null;
            m_scheduleDic.TryGetValue(activityId, out tData);
            return tData;
        }

        public int GetPreActivityId(int activityId)
        {
            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>(activityId);
            if (define.prepositionID > 0)
            {
                return define.prepositionID;
            }
            return activityId;
        }

        //获取活动红点总数
        public int GetTotalReddot()
        {
            int total = 0;
            Dictionary<Int64, ActivityTimeInfo> activityDic = m_playerProxy.GetActivityTimeInfo();
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            if (activityDic != null)
            {
                foreach (var data in activityDic)
                {
                    ActivityTimeInfo info = data.Value;
                    if ((info.startTime < 0) || serverTime >= info.startTime && serverTime <= info.endTime && info.activityId != CreateRoleActivityId)
                    {
                        ActivityScheduleData scheduleData = null;
                        m_scheduleDic.TryGetValue(info.activityId, out scheduleData);
                        if (scheduleData != null)
                        {
                            total = total + scheduleData.GetReddotNum();
                        }
                    }
                }
            }
            total = total + GetCalendarReddot()+ CalNewActivityCount();

            return total;
        }

        //获取日历红点数量
        public int GetCalendarReddot()
        {
            if (m_isCalendarReddotChange)
            {
                SetCalendarReddotStatus(false);

                int total = CalActivityDateReddotTotal();
                m_calendarReddotCount = total;
                return total;
            }
            else
            {
                return m_calendarReddotCount;
            }
        }

        public void SetCalendarReddotStatus(bool isBool)
        {
            m_isCalendarReddotChange = isBool;
        }

        //前往跳转
        public void GoJump(int jumpType)
        {
            CityBuildingProxy buildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;

            int jumpId = jumpType * 1000;
            List<JumpTypeDefine> jumpList = new List<JumpTypeDefine>();
            for (int i = 1; i < 500; i++)
            {
                int tempId = jumpId + i;
                JumpTypeDefine define = CoreUtils.dataService.QueryRecord<JumpTypeDefine>(tempId);
                if (define == null)
                {
                    break;
                }
                else
                {
                    jumpList.Add(define);
                }
            }
            if (jumpList.Count < 1)
            {
                Debug.LogFormat("not find jumpType:{0}", jumpType);
                return;
            }

            JumpTypeDefine findDefine = null;

            for (int i = 0; i < jumpList.Count; i++)
            {
                JumpTypeDefine tDefine = jumpList[i];
                if (tDefine.type == 100)
                {
                    findDefine = tDefine;
                    break;
                }
                else if (tDefine.type == 200) //界面跳转
                {
                    if (SystemOpen.IsCanOpenByUiId(tDefine.typeData1, false))
                    {
                        findDefine = tDefine;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (tDefine.type == 300) //跳转到指定建筑菜单
                {
                    BuildingInfoEntity buildingInfo = buildingProxy.GetBuildingInfoByType(tDefine.typeData1);
                    if (buildingInfo == null)
                    {
                        continue;
                    }
                    else
                    {
                        findDefine = tDefine;
                        break;
                    }
                }
                else if (tDefine.type == 400) //跳转到指定建造分页
                {
                    if (buildingProxy.HasBuildableBuild((EnumBuildingGroupType)tDefine.typeData1))
                    {
                        findDefine = tDefine;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (tDefine.type == 500) //跳转到城外
                {
                    findDefine = tDefine;
                    break;
                }
            }

            if (findDefine == null)
            {
                return;
            }

            CoreUtils.uiManager.CloseUI(UI.s_eventDate);

            if (findDefine.type == 100)
            {
                GlobalViewLevelMediator globalViewLevel = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;
                if (!globalViewLevel.IsInSide())
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ClickEnterCity);
                }
            }
            else if (findDefine.type == 200) //界面跳转
            {
                if (findDefine.typeData1 >= 4006 && findDefine.typeData1 <= 4010)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ExitCity);
                    AppFacade.GetInstance().SendNotification(CmdConstant.OpenUI, findDefine.typeData1);
                }
                else
                {
                    if (findDefine.typeData1 == 4002) //如果是迷雾探索 判断迷雾是否全开
                    {
                        if (WarFogMgr.IsAllFogOpen())
                        {
                            Debug.Log("迷雾已全开");
                            return;
                        }
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.OpenUI, findDefine.typeData1);
                }
            }
            else if (findDefine.type == 300) //跳转到指定建筑菜单
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.BuildingMenuJump, findDefine);
            }
            else if (findDefine.type == 400) //跳转到指定建造分页
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
                AppFacade.GetInstance().SendNotification(CmdConstant.EnterCity);
                if (findDefine.typeData2 != 0)
                {
                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide(findDefine.typeData2));
                }
                else
                {
                    CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, new GOScrptGuide((EnumBuildingGroupType)findDefine.typeData1));
                }
            }
            else if(findDefine.type == 500) //跳转到城外
            {
                GlobalViewLevelMediator globalViewLevel = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;
                if (globalViewLevel.IsInSide())
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ExitCity);
                }
            }
        }

        public static string RankNumFormat(Int64 num, int limit = 999, bool isShowLimit = false)
        {
            if (isShowLimit)
            {
                return string.Format("{0}+", limit);
            }
            return (num > limit) ?string.Format("{0}+", limit)  : num.ToString();
        }

        public int GetDiffDay(long startTime, long endTime)
        {
            DateTime startDate = ServerTimeModule.Instance.ConverToServerDateTime(startTime);
            Int64 times = startDate.Hour * 3600 + startDate.Minute * 60 + startDate.Second;
            Int64 t1 = startTime - times;
            Int64 diffTime = endTime - t1;
            if (diffTime % 86400 == 0)
            {
                return (int)Math.Ceiling((float)diffTime / 86400) + 1;
            }
            else
            {
                return (int)Math.Ceiling((float)diffTime / 86400);
            }
        }

        public int GetLastSelectActivityId()
        {
            if (m_lastSelectActivityId < 0)
            {
                m_lastSelectActivityId = PlayerPrefs.GetInt(TipRemindProxy.LastSelectActivityId, -1);
            }
            return m_lastSelectActivityId;
        }

        public void SetLastSelectActivityId(int activityId)
        {
            m_lastSelectActivityId = activityId;
            PlayerPrefs.SetInt(TipRemindProxy.LastSelectActivityId, activityId);
        }

        #region 兑换类活动红点刷新

        //更新兑换类活动
        public void UpdateExchangeActivity()
        {
            if (m_isDispose)
            {
                return;
            }
            if (!m_isExchangeRemind)
            {
                return;
            }
            if (m_bagProxy.ItemIdChangeList.Count >0 && m_itemChangeList.Count < 100)
            {
                m_itemChangeList.AddRange(m_bagProxy.ItemIdChangeList);
            }
            if (m_isDelaying)
            {
                return;
            }
            m_isDelaying = true;
            Timer.Register(1f, DelayUpdateExchage);
        }

        public void DelayUpdateExchage()
        {
            if (m_isDispose)
            {
                return;
            }
            m_isDelaying = false;
            if (!m_isExchangeRemind)
            {
                m_itemChangeList.Clear();
                return;
            }

            //判断一下是否有兑换类活动
            int count = m_exchangeActivityList.Count;
            if (count < 1)
            {
                m_itemChangeList.Clear();
                return;
            }

            int itemCount = m_itemChangeList.Count;

            bool isHasExchange = false;
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            for(int i= 0; i<count;i++)
            {
                ActivityTimeInfo info = m_exchangeActivityList[i];
                if ((info.startTime < 0) || serverTime >= info.startTime && serverTime <= info.endTime)
                {
                    ActivityScheduleData scheduleData = null;
                    m_scheduleDic.TryGetValue(info.activityId, out scheduleData);
                    if (scheduleData != null)
                    {
                        bool isHasExchangeItem = false;
                        if (itemCount < 100)
                        {
                            for (int k = 0; k < itemCount; k++)
                            {
                                if (scheduleData.IsExchangeItem((int)m_itemChangeList[k]))
                                {
                                    isHasExchangeItem = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            isHasExchangeItem = true;
                        }

                        if (isHasExchangeItem)
                        {
                            int beforeNum = scheduleData.GetLastReddotNum();
                            int afterNum = scheduleData.CalExchangeRedpot(true);
                            Debug.LogFormat("beforeNum:{0} afterNum:{1} activityId:{2}", beforeNum, afterNum, info.activityId);
                            if (beforeNum != afterNum)
                            {
                                isHasExchange = true;
                                scheduleData.IsReddotChange = true;
                                scheduleData.ActivityType2 = 0;
                            }
                        }
                    }
                }
            }
            m_itemChangeList.Clear();
            if (isHasExchange)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.UpdateActivityTotalReddot);
            }
        }

        public void FindExchangeActivity()
        {
            m_exchangeActivityList.Clear();
            Dictionary<Int64, ActivityTimeInfo> activityDic = m_playerProxy.GetActivityTimeInfo();
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            foreach (var data in activityDic)
            {
                ActivityTimeInfo info = data.Value;
                if ((info.startTime < 0) || serverTime >= info.startTime && serverTime <= info.endTime)
                {
                    ActivityScheduleData scheduleData = null;
                    m_scheduleDic.TryGetValue(info.activityId, out scheduleData);
                    if (scheduleData != null && scheduleData.ActivityType == 3)
                    {
                        m_exchangeActivityList.Add(data.Value);
                    }
                }
            }
        }

        #endregion

        #region 读取模版表

        public ActivityTargetTypeDefine GetTargetTypeDefine(int id)
        {
            ActivityTargetTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityTargetTypeDefine>(id);
            return define;
        }

        public ActivityDaysTypeDefine GetDayTypeDefine(int id)
        {
            ActivityDaysTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityDaysTypeDefine>(id);
            return define;
        }

        public ActivityConversionTypeDefine GetConversionTypeDefine(int id)
        {
            ActivityConversionTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityConversionTypeDefine>(id);
            return define;
        }

        public ActivityDropTypeDefine GetDropTypeDefine(int id)
        {
            ActivityDropTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityDropTypeDefine>(id);
            return define;
        }

        public ActivityKillTypeDefine GetStageDefine(int activityId, int stage, int level)
        {
            int id = activityId * 1000 + stage * 100 + level;
            ActivityKillTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityKillTypeDefine>(id);
            return define;
        }

        public ActivityIntegralTypeDefine GetIntegralTypeDefine(int id)
        {
            ActivityIntegralTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityIntegralTypeDefine>(id);
            return define;
        }

        #endregion

        #region 活动数据更新

        public void UpdateActivityTime()
        {
            //找出兑换类活动
            FindExchangeActivity();
            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateActivityTotalReddot);
        }

        //更新活动进度
        public void UpdateSchedule(Dictionary<Int64, Activity> dicInfo)
        {
            if (dicInfo == null)
            {
                return;
            }
            List<Int64> changeList = new List<Int64>();
            foreach (var data in dicInfo)
            {
                //Debug.Log("Time:"+ServerTimeModule.Instance.GetCurrServerDateTime());
                //ClientUtils.Print(data.Value);
                ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)data.Value.activityId);
                if (define == null)
                {
                    Debug.LogFormat("UpdateSchedule not find activityid:{0}", data.Value.activityId);
                    continue;
                }

                ActivityScheduleData tData = null;
                m_scheduleDic.TryGetValue(data.Key, out tData);
                if (tData == null)
                {
                    tData = new ActivityScheduleData();
                    tData.ActivityId = data.Value.activityId;
                    m_scheduleDic[data.Key] = tData;
                    if (define.activityType == 100 || define.activityType == 101) //开服活动特殊处理
                    {
                        tData.ActivityType = 1;
                        if (define.prepositionID > 0)
                        {
                            tData.ActivityType2 = 1; //宝箱领取状态
                            tData.PreActivityId = define.prepositionID;
                        }
                        else
                        {
                            tData.ActivityType2 = 2;//宝箱倒计时状态
                            tData.PreActivityId = define.ID;
                        }
                    }
                    else if (define.activityType == 200)
                    {
                        tData.ActivityType = 3;
                        tData.PreActivityId = define.ID;
                    }
                    else if (define.activityType == 500 || define.activityType == 501 || define.activityType == 502)
                    {
                        tData.ActivityType = 4;
                        if (define.prepositionID > 0)
                        {
                            tData.PreActivityId = define.prepositionID;
                            tData.ActivityType2 = 4; //已过期
                        }
                        else
                        {
                            tData.PreActivityId = define.ID;
                            tData.ActivityType2 = 3;//未过期
                        }
                    }
                    else if (define.activityType == 301 || define.activityType == 302)
                    {
                        tData.ActivityType = 5;
                        if (define.prepositionID > 0)
                        {
                            tData.PreActivityId = define.prepositionID;
                        }
                        else
                        {
                            tData.PreActivityId = define.ID;
                        }
                    }
                    else if (define.activityType == 700)
                    {
                        tData.ActivityType = 5;
                        tData.PreActivityId = define.ID;
                    }
                    else if (define.activityType == 800)
                    {
                        tData.ActivityType = 5;
                        tData.PreActivityId = define.ID;
                    }
                    else if (define.activityType == 1000)
                    {
                        tData.ActivityType = 6;
                        tData.PreActivityId = define.ID;
                    }
                    else if (define.activityType == 600)
                    {
                        tData.ActivityType = 7;
                        tData.PreActivityId = define.ID;
                    }
                    else
                    {
                        tData.ActivityType = 2;
                        if (define.prepositionID > 0)
                        {
                            tData.PreActivityId = define.prepositionID;
                            tData.ActivityType2 = 4; //已过期
                        }
                        else
                        {
                            tData.PreActivityId = define.ID;
                            tData.ActivityType2 = 3;//未过期
                        }
                    }
                    tData.IsBehaviorChange = true;
                }
                else
                {
                    if (define.activityType == 301 || define.activityType == 302 || define.activityType == 800)
                    {
                        tData.IsBehaviorChange = true;
                    }
                }
                tData.Info = data.Value;
                tData.IsReddotChange = true;
                tData.IsRewardIdChange = true;

                changeList.Add(data.Key);
            }
            if (m_isReadSchedule) //首次不更新
            {
                //ClientUtils.Print(changeList);
                AppFacade.GetInstance().SendNotification(CmdConstant.ActivityScheduleUpdate, changeList);
            }
            m_isReadSchedule = true;
        }

        //活动进度更新回包
        public void UpdateScheduleVal(object body)
        {
            var tempInfo = body as Activity_ScheduleInfo.request;
            if (tempInfo == null)
            {
                return;
            }
            ActivityScheduleData localData = null;
            m_scheduleDic.TryGetValue(tempInfo.activityId, out localData);
            if (localData == null)
            {
                return;
            }
            switch (localData.ActivityType)
            {
                case 7:
                    if (tempInfo.HasCount)
                    {
                        localData.Info.count = tempInfo.count;
                    }
                    if (tempInfo.HasDayCount)
                    {
                        localData.Info.dayCount = tempInfo.dayCount;
                    }
                    if (tempInfo.HasFree)
                    {
                        localData.Info.free = tempInfo.free;
                    }
                    if (tempInfo.HasDiscount)
                    {
                        localData.Info.discount = tempInfo.discount;
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateActivityReddot, tempInfo.activityId);
                    break;
            }

            //ClientUtils.Print(tempInfo);
            if (localData.Info.scheduleInfo.ContainsKey(tempInfo.type))
            {
                if (localData.Info.scheduleInfo[tempInfo.type].data != null && localData.Info.scheduleInfo[tempInfo.type].data.ContainsKey(tempInfo.condition))
                {
                    localData.UpdateScheduleVal(tempInfo.type, tempInfo.condition, tempInfo.num);
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateActivityReddot, tempInfo.activityId);
                }
            }
        }

        //奖励领取更新回包
        public void UpdateReward(object body)
        {
            var reward = body as Activity_Reward.request;
            if (reward == null)
            {
                return;
            }
            ActivityScheduleData localData = null;
            m_scheduleDic.TryGetValue(reward.activityId, out localData);
            if (localData == null)
            {
                return;
            }
            if (reward.activityId == CreateRoleActivityId) //创角活动
            {
                if (reward.HasActiveReward)
                {
                    if (localData.Info != null)
                    {
                        localData.IsReddotChange = true;
                        localData.Info.activeReward = reward.activeReward;
                    }
                }else
                {
                    localData.UpdateRewardList(reward.rewardId);
                }
            }
            else
            {
                localData.UpdateRewardList(reward.rewardId);
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateActivityReddot, reward.activityId);
            AppFacade.GetInstance().SendNotification(CmdConstant.ReceiveBehaviorReward, reward);
        }

        //宝箱领取回包
        public void RewardBoxUpdate(object body)
        {
            var result = body as Activity_RewardBox.request;
            if (result == null)
            {
                return;
            }
            ActivityScheduleData localData = null;
            m_scheduleDic.TryGetValue(result.activityId, out localData);
            if (localData == null)
            {
                return;
            }
            //ClientUtils.Print(result);
            if (result.rewardBox)
            {
                localData.UpdateRewardBoxStatus();
                AppFacade.GetInstance().SendNotification(CmdConstant.UpdateActivityReddot, result.activityId);
                AppFacade.GetInstance().SendNotification(CmdConstant.ReceiveOpenServerBox, result.activityId);
            }
        }

        //兑换数据更新
        public void UpdateExchageData(object body)
        {
            var result = body as Activity_Exchange.response;
            if (result == null)
            {
                return;
            }
            if (result.rewardInfo == null)
            {
                return;
            }
            //ClientUtils.Print(result);
            ActivityScheduleData localData = null;
            m_scheduleDic.TryGetValue(result.activityId, out localData);
            if (localData == null)
            {
                return;
            }
            if (localData.Info.exchange == null)
            {
                localData.Info.exchange = new Dictionary<long, Activity.Exchange>();
            }
            if (!localData.Info.exchange.ContainsKey(result.id))
            {
                localData.Info.exchange[result.id] = new Activity.Exchange();
            }
            localData.Info.exchange[result.id].count = result.count;
            localData.Info.exchange[result.id].id = result.id;
            AppFacade.GetInstance().SendNotification(CmdConstant.ActivityExchangeRefresh, body);
        }

        //更新最强执政官数据
        public void UpdateArchonData(object body)
        {
            var result = body as Activity_Rank.request;
            if (result == null)
            {
                return;
            }
            //ClientUtils.Print(result);
            if (!result.HasActivityId)
            {
                return;
            }
            ActivityScheduleData scheduleData = GetActivitySchedule(result.activityId);
            if (scheduleData == null)
            {
                Debug.LogErrorFormat("not find acitvityId:{0}", result.activityId);
                return;
            }
            bool isUpdate = false;
            if (result.HasRank)
            {
                isUpdate = true;
                scheduleData.Info.rank = result.rank;
            }
            if (result.HasScore)
            {
                isUpdate = true;
                scheduleData.UpdateScore(result.score);
                AppFacade.GetInstance().SendNotification(CmdConstant.UpdateActivityReddot, result.activityId);
            }
            if (isUpdate)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.ActivityRankOrScoreUpdate, result.activityId);
            }
        }

        //更新活动new标志
        public void UpdateActivityNewFlag(object body)
        {
            var result = body as Activity_ClickActivity.response;
            if (result == null)
            {
                return;
            }
            ActivityScheduleData tData = GetActivitySchedule(result.activityId);
            if (tData == null)
            {
                return;
            }
            if (tData.Info != null)
            {
                tData.Info.isNew = false;
                SetCalendarReddotStatus(true);
                AppFacade.GetInstance().SendNotification(CmdConstant.RefreshActivityNewFlag, result.activityId);
            }
        }

        #endregion

        #region 日历活动红点计数 新活动红点计数

        //获取日历活动列表
        public int CalActivityDateReddotTotal()
        {
            Int64 startWeekTime = GetWeekStartTime();
            Int64 endWeekTime = GetWeekEndTime(startWeekTime);

            Dictionary<Int64, ActivityTimeInfo> list = m_playerProxy.GetActivityTimeInfo();
            int total = 0;
            foreach (var data in list)
            {
                ActivityTimeInfo info = data.Value;
                ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)info.activityId);
                if (define == null)
                {
                    continue;
                }
                if (info.startTime < 0) //永久存在不需要显示
                {
                    continue;
                }
                if (info.startTime >= startWeekTime && info.startTime <= endWeekTime ||
                    info.endTime >= startWeekTime && info.endTime <= endWeekTime)
                {
                    if (info.startTime > info.endTime)
                    {
                        continue;
                    }
                    if (define != null && define.ifShow == 1)
                    {
                        int startWeekDay = -1;
                        if (info.startTime >= startWeekTime && info.startTime <= endWeekTime)
                        {
                            startWeekDay = (int)ServerTimeModule.Instance.ConverToServerDateTime(info.startTime).DayOfWeek;
                        }
                        int endWeekDay = -1;
                        if (info.endTime >= startWeekTime && info.endTime <= endWeekTime)
                        {
                            endWeekDay = (int)ServerTimeModule.Instance.ConverToServerDateTime(info.endTime).DayOfWeek;
                        }

                        if (startWeekDay == -1 && endWeekDay >= 0)
                        {
                            startWeekDay = 0;
                        }
                        else if (startWeekDay >= 0 && endWeekDay == -1)
                        {
                            endWeekDay = 6;
                        }
                        if (startWeekDay > endWeekDay)
                        {
                            continue;
                        }

                        ActivityScheduleData scheduleData = null;
                        m_scheduleDic.TryGetValue(info.activityId, out scheduleData);
                        if (scheduleData != null && scheduleData.Info.isNew)
                        {
                            total = total + 1;
                        }
                    }
                }
            }
            return total;
        }

        //新活动红点计数(除日历显示的活动)
        public int CalNewActivityCount()
        {
            List<ActivityTimeInfo> newList = new List<ActivityTimeInfo>();
            Dictionary<Int64, ActivityTimeInfo> list = m_playerProxy.GetActivityTimeInfo();

            int total = 0;
            foreach (var data in list)
            {
                ActivityTimeInfo info = data.Value;
                if (info.activityId != CreateRoleActivityId)
                {
                    ActivityScheduleData scheduleData = null;
                    m_scheduleDic.TryGetValue(info.activityId, out scheduleData);
                    if (scheduleData == null || scheduleData.Info.isNew == false)
                    {
                        continue;
                    }
                    ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)info.activityId);
                    if (define == null)
                    {
                        continue;
                    }
                    if (define.ifShow == 1)
                    {
                        continue;
                    }
                    if (define.timeType == 4)
                    {
                        total = total + 1;
                        continue;
                    }
                    if (info.startTime < 0) //永久
                    {
                        total = total + 1;
                        continue;
                    }
                    Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
                    if (serverTime >= info.startTime && serverTime < info.endTime)
                    {
                        total = total + 1;
                        continue;
                    }
                }
            }
            return total;
        }

        #endregion
    }
}