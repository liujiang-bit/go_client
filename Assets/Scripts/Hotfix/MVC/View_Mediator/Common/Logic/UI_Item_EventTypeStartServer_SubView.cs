// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月16日
// Update Time         :    2020年4月16日
// Class Description   :    UI_Item_EventTypeStartServer_SubView 开服活动界面
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using System;
using Data;
using System.Collections.Generic;
using PureMVC.Interfaces;

namespace Game {

    public partial class UI_Item_EventTypeStartServer_SubView : UI_SubView
    {
        private bool m_isInit;
        private bool m_assetIsLoadFinish;
        private ActivityProxy m_activityProxy;

        private ActivityItemData m_menuData;
        private ActivityDetialData m_detailData;
        private ActivityScheduleData m_scheduleData; //活动进度数据

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private int m_currDay;
        private int m_currEvent;

        private bool m_isInitList;

        private bool m_isForceSwitchEvent;
        private List<UI_Model_EventTypeStartServerCk2_SubView> m_eventViewList;
        private List<UI_Model_EventTypeStartServerBtn_SubView> m_dayViewList;
        private List<int> m_eventList;

        private Timer m_timer;
        private Int64 m_canRewardTime;  //可领取奖励时间

        private int m_openDay;  //已开启天数

        private Dictionary<int, bool> m_dayRedDic = new Dictionary<int, bool>();
        private Dictionary<int, bool> m_eventRedDic = new Dictionary<int, bool>();

        private Dictionary<Int64, RectTransform> m_eventRewardRecord = new Dictionary<Int64, RectTransform>();

        private bool m_isReadTotalCount;
        private int m_totalCount;   //总进度

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.UpdateActivityReddot,
                    CmdConstant.ReceiveBehaviorReward,
                    CmdConstant.ReceiveOpenServerBox,
                    Activity_ReceiveReward.TagName,
                    CmdConstant.SystemDayChange,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateActivityReddot: //刷新红点
                    Int64 activityId = (Int64)notification.Body;
                    if (m_detailData != null && m_detailData.AcivityId == activityId)
                    {
                        //Debug.LogError("刷新红点");
                        DayChangeRefresh();
                    }
                    break;
                case CmdConstant.ReceiveBehaviorReward://领取行为奖励
                    var reward = notification.Body as Activity_Reward.request;
                    Int64 acEventId = reward.rewardId;
                    Debug.LogFormat("ReceiveBehaviorReward acEvent:{0}", acEventId);
                    RefreshEventItem(acEventId);
                    RefreshSchedule();
                    break;
                case CmdConstant.ReceiveOpenServerBox://领取开服宝箱奖励
                    RefreshBoxShow();
                    break;
                case Activity_ReceiveReward.TagName: //奖励信息
                    ProcessReceiveReward(notification.Body);
                    break;
                case CmdConstant.SystemDayChange:
                    DayChangeRefresh();
                    break;
                default: break;
            }
        }

        public void Init(ActivityItemData menuData)
        {
            m_menuData = menuData;
            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;

                ReadData();

                m_btn_day1.m_UI_Model_EventTypeStartServerBtn_GameButton.onClick.AddListener(ClickDay1);
                m_btn_day2.m_UI_Model_EventTypeStartServerBtn_GameButton.onClick.AddListener(ClickDay2);
                m_btn_day3.m_UI_Model_EventTypeStartServerBtn_GameButton.onClick.AddListener(ClickDay3);
                m_btn_day4.m_UI_Model_EventTypeStartServerBtn_GameButton.onClick.AddListener(ClickDay4);
                m_btn_day5.m_UI_Model_EventTypeStartServerBtn_GameButton.onClick.AddListener(ClickDay5);
                m_dayViewList = new List<UI_Model_EventTypeStartServerBtn_SubView>();
                m_dayViewList.Add(m_btn_day1);
                m_dayViewList.Add(m_btn_day2);
                m_dayViewList.Add(m_btn_day3);
                m_dayViewList.Add(m_btn_day4);
                m_dayViewList.Add(m_btn_day5);

                m_ck_event1.m_UI_Model_EventTypeStartServerCk2_GameToggle.onValueChanged.AddListener(ToggleEvent1);
                m_ck_event2.m_UI_Model_EventTypeStartServerCk2_GameToggle.onValueChanged.AddListener(ToggleEvent2);
                m_ck_event3.m_UI_Model_EventTypeStartServerCk2_GameToggle.onValueChanged.AddListener(ToggleEvent3);
                m_eventViewList = new List<UI_Model_EventTypeStartServerCk2_SubView>();
                m_eventViewList.Add(m_ck_event1);
                m_eventViewList.Add(m_ck_event2);
                m_eventViewList.Add(m_ck_event3);

                m_btn_box_GameButton.onClick.AddListener(OnClickBox);
                m_btn_get.m_btn_languageButton_GameButton.onClick.AddListener(OnClickBoxGet);
                m_btn_info_GameButton.onClick.AddListener(ClickInfo);
                m_btn_back_GameButton.onClick.AddListener(ClickBack);

                //预加载列表预设
                List<string> prefabNames = new List<string>();
                prefabNames.AddRange(m_sv_list_ListView.ItemPrefabDataList);
                ClientUtils.PreLoadRes(gameObject, prefabNames, LoadFinish);
                m_isInit = true;
            }
            else
            {
                ReadData();

                if (m_assetIsLoadFinish)
                {
                    Refresh();
                }
            }
        }

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            //Debug.LogError("加载完成");
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            m_assetIsLoadFinish = true;
            gameObject.SetActive(true);

            m_currDay = 1;

            Refresh();
            SetSelectDay();
        }

        private void ReadData()
        {
            m_detailData = m_activityProxy.GetOpenActivityDetail(m_menuData.Data.activityId);
            m_scheduleData = m_detailData.ScheduleData.GetScheduleData();

            m_openDay = GetOpenDay();

            UpdateDayEventRedpotData();
        }

        private int GetOpenDay()
        {
            if (m_detailData.ScheduleData.PreActivityId != m_detailData.AcivityId) //后置活动
            {
                return 5;
            }
            DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(m_menuData.Data.startTime);
            Int64 times = startTime.Hour * 3600 + startTime.Minute * 60 + startTime.Second;
            Int64 t1 = m_menuData.Data.startTime - times;
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 diffTime = serverTime - t1;
            int openDay = (int)Math.Ceiling((float)diffTime / 86400);
            if (openDay < 0)
            {
                openDay = 1;
            }
            if (openDay > 5)
            {
                openDay = 5;
            }
            return openDay;
        }

        private void DayChangeRefresh()
        {
            int day = GetOpenDay();
            if (day != m_openDay)
            {
                m_openDay = day;
                RefreshDayLockStatus();
            }
            UpdateDayEventRedpotData();
            RefreshDayEventRedStatus();
        }

        private void UpdateDayEventRedpotData()
        {
            m_dayRedDic.Clear();
            m_eventRedDic.Clear();

            if (m_detailData.ScheduleData.ActivityType2 == 2)
            {
                //更新红点显示
                List<ActivityBehaviorData> behaviorList = m_scheduleData.GetBehaviorList();
                ActivityDaysTypeDefine define = null;
                for (int i = 0; i < behaviorList.Count; i++)
                {
                    if (!m_scheduleData.IsReceive(behaviorList[i].Id))
                    {
                        define = m_activityProxy.GetDayTypeDefine(behaviorList[i].Id);
                        if (define != null)
                        {
                            int count = m_scheduleData.GetBehaviorSchedule(define.playerBehavior, define.data3);
                            if (count >= define.data0)
                            {
                                m_dayRedDic[define.day] = true;
                                m_eventRedDic[GetEventIndex(define.day, define.paging)] = true;
                            }
                        }
                    }
                }
            }
        }

        private int GetEventIndex(int day, int paging)
        {
            return day * 10 + paging;
        }

        private void Refresh()
        {
            ActivityTimeInfo activityInfo = m_menuData.Data;
            ActivityCalendarDefine define = m_menuData.Define;

            //Debug.LogError("类型：" + define.activityType);

            ClientUtils.LoadSprite(m_img_bg_PolygonImage, define.background);

            m_lbl_name_LanguageText.text = LanguageUtils.getText(define.l_nameID);
            DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.startTime);
            DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.endTime);
            m_lbl_life_LanguageText.text = LanguageUtils.getTextFormat(762012, startTime.ToString("yyyy/M/d"), endTime.ToString("yyyy/M/d"));

            RefreshSchedule();

            RefreshDayLockStatus();

            if (m_detailData.ScheduleData.ActivityType2 == 2)//宝箱待开启状态
            {
                //获取后置活动的开启时间
                ActivityTimeInfo info = m_activityProxy.GetActivityById(define.postpositionID);
                if (info != null)
                {
                    Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
                    if (info.startTime > serverTime)
                    {
                        m_canRewardTime = info.startTime;
                        UpdateTime();
                        StartTimer();
                    }
                    else
                    {
                        CancelTimer();
                    }
                }

                //显示倒计时
                m_btn_get.gameObject.SetActive(false);
            }
            else //宝箱已开启状态
            {
                CancelTimer();
                RefreshBoxShow();
            }

            //刷新红点显示
            RefreshDayEventRedStatus();
        }

        private void RefreshBoxShow()
        {
            if (m_detailData.ScheduleData.Info.rewardBox) //已领取
            {
                m_btn_get.gameObject.SetActive(false);
                m_lbl_gettime_LanguageText.text = LanguageUtils.getText(762150);
                m_img_box_red_PolygonImage.gameObject.SetActive(false);
            }
            else//未领取
            {
                if(m_scheduleData.Info.rewardId.Count ==0)
                {
                    m_btn_get.gameObject.SetActive(false);
                    m_lbl_gettime_LanguageText.text = LanguageUtils.getText(733010);
                    m_img_box_red_PolygonImage.gameObject.SetActive(false);
                    return;
                }
                m_lbl_gettime_LanguageText.text = "";
                m_btn_get.gameObject.SetActive(true);
                m_img_box_red_PolygonImage.gameObject.SetActive(true);
            }
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, m_sv_list_ListView);
            }
        }

        //取消定时器
        private void CancelTimer()
        {
            if (m_timer != null) 
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private void UpdateTime()
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 diffTime = m_canRewardTime - serverTime;
            if (diffTime < 0)
            {
                //倒计时结束 刷新左侧菜单列表 以及右侧内容
                CancelTimer();
                CoreUtils.uiManager.CloseUI(UI.s_eventDate);
            }
            else
            {
                m_lbl_gettime_LanguageText.text = LanguageUtils.getTextFormat(762141, ClientUtils.FormatCountDown((int)diffTime));

            }
        }

        //刷新领取进度
        private void RefreshSchedule()
        {
            ActivityDaysTypeDefine define;

            if (!m_isReadTotalCount)
            {
                int total = 0;
                List<ActivityBehaviorData> behaviorList = m_scheduleData.GetBehaviorList();
                for (int i = 0; i < behaviorList.Count; i++)
                {
                    define = m_activityProxy.GetDayTypeDefine(behaviorList[i].Id);
                    total = total + define.specialItemNum;
                }
                m_totalCount = total;
                m_isReadTotalCount = true;
            }

            int count = 0;
            if (m_scheduleData.Info.rewardId != null)
            {
                for (int i = 0; i < m_scheduleData.Info.rewardId.Count; i++)
                {
                    define = m_activityProxy.GetDayTypeDefine((int)m_scheduleData.Info.rewardId[i]);
                    count = count + define.specialItemNum;
                }
            }
            m_pb_rogressBar_GameSlider.value = (float)count / m_totalCount;
            m_lbl_val_LanguageText.text = LanguageUtils.getTextFormat(181104, count, m_totalCount);
        }

        private void RefreshDayLockStatus()
        {
            for (int i = 0; i < 5; i++)
            {
                m_dayViewList[i].SetLockStatus((i + 1)>m_openDay);
            }
        }

        private void RefreshDayEventRedStatus()
        {
            for(int i=0;i<5;i++)
            {
                if (m_dayRedDic.ContainsKey(i+1))
                {
                    m_dayViewList[i].SetRedpotStatus(true);
                }
                else
                {
                    m_dayViewList[i].SetRedpotStatus(false);
                }
            }
            RefershEventRedStatus();
        }

        private void RefreshEventTitle()
        {
            for (int i = 1; i < 4; i++)
            {
                int id = m_detailData.DataDic[m_currDay][i][0];
                ActivityDaysTypeDefine define = m_activityProxy.GetDayTypeDefine(id);
                m_eventViewList[i - 1].m_lbl_name_LanguageText.text = LanguageUtils.getText(define.l_pagingDesID);
            }
        }

        private void RefershEventRedStatus()
        {
            for (int i = 0; i < 3; i++)
            {
                int index = GetEventIndex(m_currDay, i+1);
                if (m_eventRedDic.ContainsKey(index))
                {
                    m_eventViewList[i].SetRedpotStatus(true);
                }
                else
                {
                    m_eventViewList[i].SetRedpotStatus(false);
                }
            }
        }

        private void SetSelectDay()
        {
            for (int i = 0; i < 5; i++)
            {
                m_dayViewList[i].SetSelectStatus(false);
            }
            SwitchDay(m_currDay, true);
        }

        private void ClickDay1()
        {
            SwitchDay(1);
        }

        private void ClickDay2()
        {
            SwitchDay(2);
        }

        private void ClickDay3()
        {
            SwitchDay(3);
        }

        private void ClickDay4()
        {
            SwitchDay(4);
        }

        private void ClickDay5()
        {
            SwitchDay(5);
        }

        private void SwitchDay(int day, bool isForce = false)
        {
            if (day > m_openDay)
            {
                Debug.Log("不运行切换");
                return;
            }
            if (!isForce)
            {
                if (day == m_currDay)
                {
                    return;
                }
            }
            m_dayViewList[m_currDay - 1].SetSelectStatus(false);
            m_dayViewList[day-1].SetSelectStatus(true);
            m_currDay = day;
            //Debug.LogError("day:"+day);

            m_currEvent = 1;
            ForceSwitchEvent();
            RefreshEventTitle();
            RefreshDayEventRedStatus();
        }

        private void ForceSwitchEvent()
        {
            m_isForceSwitchEvent = true;
            int index = m_currEvent - 1;
            bool isRefresh = m_eventViewList[index].m_UI_Model_EventTypeStartServerCk2_GameToggle.isOn;
            m_eventViewList[index].m_UI_Model_EventTypeStartServerCk2_GameToggle.isOn = true;
            SetEventTextColor(index, true);
            if (isRefresh)
            {
                SwitchEvent(m_currEvent);
            }
        }

        private void ToggleEvent1(bool isBool)
        {
            SetEventTextColor(0, isBool);
            if (isBool)
            {
                SwitchEvent(1);
            }
        }

        private void ToggleEvent2(bool isBool)
        {
            SetEventTextColor(1, isBool);
            if (isBool)
            {
                SwitchEvent(2);
            }
        }

        private void ToggleEvent3(bool isBool)
        {
            SetEventTextColor(2, isBool);
            if (isBool)
            {
                SwitchEvent(3);
            }
        }

        private void SetEventTextColor(int index, bool isBool)
        {
            if (isBool)
            {
                ClientUtils.TextSetColor(m_eventViewList[index].m_lbl_name_LanguageText, "#ffffff");
            } else
            {
                ClientUtils.TextSetColor(m_eventViewList[index].m_lbl_name_LanguageText, "#a49d92");
            }
        }

        private void SwitchEvent(int type)
        {
            if (!m_isForceSwitchEvent)
            {
                if (type == m_currEvent)
                {
                    return;
                }
            }
            m_isForceSwitchEvent = false;
            m_currEvent = type;
            m_eventList = m_detailData.DataDic[m_currDay][m_currEvent];

            //排序
            m_eventList.Sort(delegate (int x, int y)
            {
                int re = GetReceiveStatus(y).CompareTo(GetReceiveStatus(x));
                if (re == 0)
                {
                    re = x.CompareTo(y);
                }
                return re;
            });

            //Debug.LogError("Event:" + type);
            //ClientUtils.Print(m_eventList);

            RefreshEventList();
        }

        private int GetReceiveStatus(int eventId)
        {
            if (m_scheduleData.IsReceive(eventId))
            {
                return 1;
            }
            else
            {
                ActivityDaysTypeDefine define = m_activityProxy.GetDayTypeDefine(eventId);
                int val = m_scheduleData.GetBehaviorSchedule(define.playerBehavior, define.data3);
                if (val >= define.data0)
                {
                    return 3;
                }
                return 2;
            }
        }

        private void RefreshEventList()
        {
            if (m_isInitList)
            {
                m_sv_list_ListView.FillContent(m_eventList.Count);
            }
            else
            {
                m_isInitList = true;
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex;
                m_sv_list_ListView.SetInitData(m_assetDic, functab);
                m_sv_list_ListView.FillContent(m_eventList.Count);
            }
        }

        private void ItemEventByIndex(ListView.ListItem listItem)
        {
            int acEventId = m_eventList[listItem.index];
            if (listItem.data == null)
            {
                var subView = new UI_Item_EventType2Item_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.BtnClickCallback = EventBtnReceiveRecord;
                subView.Refresh1(acEventId, m_scheduleData, m_detailData.ScheduleData.ActivityType2 == 1);
            }
            else
            {
                var subView = listItem.data as UI_Item_EventType2Item_SubView;
                subView.Refresh1(acEventId, m_scheduleData, m_detailData.ScheduleData.ActivityType2 == 1);
            }
        }

        //点击宝箱
        private void OnClickBox()
        {
            if (m_menuData.Define.activityType == 100)
            {
                Tip.CreateTip(LanguageUtils.getText(762139)).Show();
                return;
            }
        }

        //领取宝箱
        private void OnClickBoxGet()
        {
            if (m_menuData.Define.activityType != 101)
            {
                return;
            }
            var sp = new Activity_ReceiveReward.request();
            sp.activityId = m_detailData.AcivityId;
            AppFacade.GetInstance().SendSproto(sp);
            //Debug.LogError("sp.activityId:" + sp.activityId);
        }

        #region 活动描述

        private void ClickInfo()
        {
            m_pl_con_CanvasGroup.gameObject.SetActive(false);
            m_pl_desc_CanvasGroup.gameObject.SetActive(true);

            m_lbl_eventDesc_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_ruleID);
        }

        private void ClickBack()
        {
            m_pl_con_CanvasGroup.gameObject.SetActive(true);
            m_pl_desc_CanvasGroup.gameObject.SetActive(false);
        }

        #endregion

        #region 行为奖励领取

        //行为事件奖励领取按钮记录
        private void EventBtnReceiveRecord(UI_Item_EventType2Item_SubView subView)
        {
            int eventId = subView.GetEventId();
            m_eventRewardRecord[eventId] = subView.m_btn_get.m_root_RectTransform;
        }

        //处理奖励信息
        private void ProcessReceiveReward(object body)
        {
            var result = body as Activity_ReceiveReward.response;
            if (result == null)
            {
                return;
            }
            if (result.rewardInfo == null)
            {
                return;
            }
            RectTransform rectTrans = null;
            if (result.id > 0)
            {
                m_eventRewardRecord.TryGetValue(result.id, out rectTrans);
                m_eventRewardRecord.Remove(result.id);
            }
            else
            {
                rectTrans = m_btn_box_GameButton.gameObject.GetComponent<RectTransform>();
            }
            if (rectTrans == null)
            {
                return;
            }
            //飘飞特效
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            if (result.rewardInfo.items != null)
            {
                for (int i = 0; i < result.rewardInfo.items.Count; i++)
                {
                    mt.FlyItemEffect((int)result.rewardInfo.items[i].itemId,
                                     (int)result.rewardInfo.items[i].itemNum,
                                     rectTrans);
                }
            }
        }

        //行为奖励领取后刷新
        private void RefreshEventItem(Int64 acEventId)
        {
            int findIndex = -1;
            for (int i = 0; i < m_eventList.Count; i++)
            {
                if (acEventId == m_eventList[i])
                {
                    findIndex = i;
                    break;
                }
            }
            if (findIndex < 0)
            {
                return;
            }
            ListView.ListItem listItem = m_sv_list_ListView.GetItemByIndex(findIndex);
            if (listItem == null || listItem.data == null || listItem.go == null)
            {
                return;
            }
            if (!listItem.go.activeSelf)
            {
                return;
            }
            UI_Item_EventType2Item_SubView subView = listItem.data as UI_Item_EventType2Item_SubView;
            subView.Refresh1((int)acEventId, m_scheduleData, m_detailData.ScheduleData.ActivityType2 == 1);
        }

        #endregion   
    }
}