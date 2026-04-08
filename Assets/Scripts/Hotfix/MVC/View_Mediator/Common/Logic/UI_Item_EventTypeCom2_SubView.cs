// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_Item_EventTypeCom2_SubView 通用条件达成类活动 1基础达标类型活动界面 2每日重置达标类型
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using SprotoType;
using System;
using PureMVC.Interfaces;
using Data;

namespace Game {
    public partial class UI_Item_EventTypeCom2_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private PlayerProxy m_playerProxy;

        private bool m_isInit;
        private bool m_assetIsLoadFinish;
        private bool m_isInitList;

        private ActivityItemData m_menuData;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private Dictionary<Int64, RectTransform> m_eventRewardRecord = new Dictionary<Int64, RectTransform>();

        private ActivityScheduleData m_scheduleData;
        private ActivityScheduleData m_preScheduleData;
        private List<ActivityBehaviorData> m_behaviorList;

        private Timer m_timer;
        private Int64 m_endTime;

        private float m_UI_Item_EventType2ItemHeight = 0;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.ReceiveBehaviorReward,
                    Activity_ReceiveReward.TagName,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ReceiveBehaviorReward://领取行为奖励
                    var reward = notification.Body as Activity_Reward.request;
                    Int64 acEventId = reward.rewardId;
                    Debug.LogFormat("ReceiveBehaviorReward acEvent:{0}", acEventId);
                    RefreshEventItem(acEventId);
                    break;
                case Activity_ReceiveReward.TagName: //奖励信息
                    ProcessReceiveReward(notification.Body);
                    break;
                default:
                    break;
            }
        }

        public void Init(ActivityItemData menuData)
        {
            m_menuData = menuData;
            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;

                ReadData();

                m_UI_Model_EventType.m_btn_rank_GameButton.gameObject.SetActive(false);

                m_UI_Model_EventType.Refresh(m_menuData.Define);

                //预加载列表预设
                List<string> prefabNames = new List<string>();
                prefabNames.AddRange(m_UI_Model_EventType.m_sv_list_ListView.ItemPrefabDataList);
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
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            m_UI_Item_EventType2ItemHeight = m_assetDic["UI_Item_EventType2Item"].gameObject.GetComponent<RectTransform>().rect.height;
            gameObject.SetActive(true);
            m_assetIsLoadFinish = true;
            Refresh();
        }

        private void ReadData()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);
            m_preScheduleData = m_scheduleData.GetScheduleData();
            List <ActivityBehaviorData> dataList = m_preScheduleData.GetBehaviorList();

            //排序
            dataList.Sort(delegate (ActivityBehaviorData x, ActivityBehaviorData y)
            {
                int re = GetReceiveStatus(y).CompareTo(GetReceiveStatus(x));
                if (re == 0)
                {
                    re = x.Id.CompareTo(y.Id);
                }
                return re;
            });
            m_behaviorList = dataList;
        }

        private int GetReceiveStatus(ActivityBehaviorData data)
        {
            if (m_preScheduleData.IsReceive(data.Id))
            {
                return 1;
            }
            else
            {
                ActivityTargetTypeDefine define = m_activityProxy.GetTargetTypeDefine(data.Id);
                int val = m_preScheduleData.GetBehaviorSchedule(define.playerBehavior, define.data3);
                if (val >= define.data0)
                {
                    return 3;
                }
                return 2;
            }
        }

        private void Refresh()
        {
            m_UI_Model_EventType.m_lbl_title_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_taglineID);
            ClientUtils.TextSetColor(m_UI_Model_EventType.m_lbl_title_LanguageText, m_menuData.Define.taglineColour);
            m_UI_Model_EventType.m_lbl_name_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_nameID);
            ClientUtils.TextSetColor(m_UI_Model_EventType.m_lbl_name_LanguageText, m_menuData.Define.nameColour);
            ClientUtils.LoadSprite(m_UI_Model_EventType.m_img_top_PolygonImage, m_menuData.Define.background);

            int activityType = m_menuData.Define.activityType;
            if (activityType == 400)
            {
                m_UI_Model_EventType.m_lbl_lifeDay_LanguageText.text = "";
                m_UI_Model_EventType.m_lbl_lifeTime_LanguageText.text = "";
                m_UI_Model_EventType.m_btn_reset_GameButton.gameObject.SetActive(false);
            }
            else if(activityType == 401)
            {
                ActivityTimeInfo activityInfo = m_menuData.Data;
                DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.startTime);
                DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.endTime);
                m_UI_Model_EventType.m_lbl_lifeDay_LanguageText.text = LanguageUtils.getTextFormat(762020, startTime.ToString("yyyy/M/d"), endTime.ToString("yyyy/M/d"));

                m_UI_Model_EventType.m_lbl_reset_LanguageText.text = LanguageUtils.getText(762021);

                Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
                if (activityInfo.endTime > serverTime)
                {
                    m_endTime = activityInfo.endTime;
                    UpdateTime();
                    StartTimer();
                }
                else
                {
                    CancelTimer();
                }
            }

            //刷新行为列表
            RefreshList();
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, m_UI_Model_EventType.m_sv_list_ListView);
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
            Int64 diffTime = m_endTime - serverTime;
            if (diffTime < 0)
            {
                //倒计时结束 关闭界面
                CancelTimer();
                CoreUtils.uiManager.CloseUI(UI.s_eventDate);
            }
            else
            {
                m_UI_Model_EventType.m_lbl_lifeTime_LanguageText.text = LanguageUtils.getTextFormat(762019, ClientUtils.FormatCountDown((int)diffTime));
            }
        }

        public void RefreshList()
        {
            if (m_isInitList)
            {
                m_UI_Model_EventType.m_sv_list_ListView.FillContent(m_behaviorList.Count);
            }
            else
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex;
                functab.GetItemPrefabName = OnGetItemPrefabName;
                functab.GetItemSize = OnGetItemSize;
                m_UI_Model_EventType.m_sv_list_ListView.SetInitData(m_assetDic, functab);
                m_UI_Model_EventType.m_sv_list_ListView.FillContent(m_behaviorList.Count);
                m_isInitList = true;
            }
        }

        private string OnGetItemPrefabName(ListView.ListItem listItem)
        {
            return "UI_Item_EventType2Item";
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            return m_UI_Item_EventType2ItemHeight;
        }

        private void ItemEventByIndex(ListView.ListItem listItem)
        {
            var behaviorData = m_behaviorList[listItem.index];
            if (listItem.data == null)
            {
                var subView = new UI_Item_EventType2Item_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.BtnClickCallback = EventBtnReceiveRecord;
                subView.Refresh2(behaviorData.Id, m_preScheduleData);
            }
            else
            {
                var subView = listItem.data as UI_Item_EventType2Item_SubView;
                subView.Refresh2(behaviorData.Id, m_preScheduleData);
            }
        }

        public void RefreshSchedule()
        {
            //Debug.LogError("刷新活动进度");
            if (m_isInit && m_assetIsLoadFinish)
            {
                RefreshList();
            }
        }

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
            for (int i = 0; i < m_behaviorList.Count; i++)
            {
                if (acEventId == m_behaviorList[i].Id)
                {
                    findIndex = i;
                    break;
                }
            }
            if (findIndex < 0)
            {
                return;
            }
            ListView.ListItem listItem = m_UI_Model_EventType.m_sv_list_ListView.GetItemByIndex(findIndex);
            if (listItem == null || listItem.data == null || listItem.go == null)
            {
                return;
            }
            if (!listItem.go.activeSelf)
            {
                return;
            }
            UI_Item_EventType2Item_SubView subView = listItem.data as UI_Item_EventType2Item_SubView;
            subView.Refresh2((int)acEventId, m_preScheduleData);
        }

        #endregion   
    }
}