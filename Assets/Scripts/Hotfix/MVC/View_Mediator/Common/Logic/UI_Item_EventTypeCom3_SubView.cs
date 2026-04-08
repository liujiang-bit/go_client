// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月29日
// Update Time         :    2020年4月29日
// Class Description   :    UI_Item_EventTypeCom3_SubView 通用条件达成类活动 达标+排行界面 掉落类活动
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using Skyunion;
using Client;
using System.Collections.Generic;
using SprotoType;
using System;
using PureMVC.Interfaces;
using Data;

namespace Game {
    public partial class UI_Item_EventTypeCom3_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private RewardGroupProxy m_rewardGroupProxy;

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

        private List<UI_Model_RewardGet_SubView> m_itemViewList;

        private int m_rankPrefabLoadStatus = 1; //1尚未加载 2加载中 3已加载完成
        private GameObject m_rankNode;
        private object m_rankSubView;

        private bool m_isExpire;

        private float m_UI_Item_EventType2ItemHeight = 0;
        private float m_UI_Item_EventType3ItemHeight = 0;

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
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

                ReadData();

                m_UI_Model_EventType.Refresh(m_menuData.Define);

                m_UI_Model_EventType.m_btn_reset_GameButton.gameObject.SetActive(false);

                m_UI_Model_EventType.m_btn_rank_GameButton.onClick.AddListener(OnRank);

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
            m_UI_Item_EventType3ItemHeight = m_assetDic["UI_Item_EventType3Item"].gameObject.GetComponent<RectTransform>().rect.height;

            gameObject.SetActive(true);
            m_assetIsLoadFinish = true;
            Refresh();
        }

        private void ReadData()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);
            m_preScheduleData = m_scheduleData.GetScheduleData();
            List<ActivityBehaviorData> dataList = m_preScheduleData.GetBehaviorList();

            if (m_menuData.Define.activityType == 402 || m_menuData.Define.activityType == 403)
            {
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
            }
            m_behaviorList = dataList;
        }

        private void Refresh()
        {
            m_UI_Model_EventType.m_lbl_title_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_taglineID);
            ClientUtils.TextSetColor(m_UI_Model_EventType.m_lbl_title_LanguageText, m_menuData.Define.taglineColour);
            m_UI_Model_EventType.m_lbl_name_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_nameID);
            ClientUtils.TextSetColor(m_UI_Model_EventType.m_lbl_name_LanguageText, m_menuData.Define.nameColour);
            ClientUtils.LoadSprite(m_UI_Model_EventType.m_img_top_PolygonImage, m_menuData.Define.background);

            int activityType = m_menuData.Define.activityType;
            if (activityType == 402 || activityType == 403)
            {
                ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)m_preScheduleData.ActivityId);
                RefreshRewardGroup(define.itemPackage);

                ActivityTimeInfo activityInfo = m_menuData.Data;
                DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.startTime);
                DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.endTime);
                m_UI_Model_EventType.m_lbl_lifeDay_LanguageText.text = LanguageUtils.getTextFormat(762020, startTime.ToString("yyyy/M/d"), endTime.ToString("yyyy/M/d"));

                //倒计时
                if (activityType == 403)
                {
                    m_isExpire = true;
                    CancelTimer();
                    m_UI_Model_EventType.m_lbl_lifeTime_LanguageText.text = LanguageUtils.getText(762136);
                }
                else
                {
                    m_isExpire = false;
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
            else if (activityType == 500 || activityType == 501 || activityType == 502)
            {
                ActivityTimeInfo activityInfo = m_menuData.Data;
                DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.startTime);
                DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.endTime);
                m_UI_Model_EventType.m_lbl_lifeDay_LanguageText.text = LanguageUtils.getTextFormat(762020, startTime.ToString("yyyy/M/d"), endTime.ToString("yyyy/M/d"));

                if (activityType == 502)
                {
                    m_isExpire = true;
                    CancelTimer();
                    m_UI_Model_EventType.m_lbl_lifeTime_LanguageText.text = LanguageUtils.getText(762136);
                }
                else
                {
                    m_isExpire = false;
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

                //排行按钮
                if (activityType == 500)
                {
                    m_UI_Model_EventType.m_btn_rank_GameButton.gameObject.SetActive(false);
                }
                else
                {
                    m_UI_Model_EventType.m_btn_rank_GameButton.gameObject.SetActive(true);
                    ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)m_preScheduleData.ActivityId);
                    RefreshRewardGroup(define.itemPackage);
                }

                RefreshList2();
            }
        }

        //刷新奖励组物品
        private void RefreshRewardGroup(int itemPackage)
        {
            if (m_itemViewList == null)
            {
                m_itemViewList = new List<UI_Model_RewardGet_SubView>();
                m_itemViewList.Add(m_UI_Model_EventType.m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_EventType.m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_EventType.m_UI_Model_Item3);
                for (int i = 0; i < m_itemViewList.Count; i++)
                {
                    m_itemViewList[i].SetScale(m_UI_Model_EventType.m_pl_item_ArabLayoutCompment.transform.localScale.x);
                }
            }
            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            for (int i = 0; i < 3; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].RefreshByGroup(groupDataList[i], 2);
                }
                else
                {
                    m_itemViewList[i].gameObject.SetActive(false);
                }
            }
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

        #region 掉落类活动

        public void RefreshList2()
        {
            if (m_isInitList)
            {
                m_UI_Model_EventType.m_sv_list_ListView.FillContent(m_behaviorList.Count);
            }
            else
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex2;
                functab.GetItemPrefabName = OnGetItemPrefabName2;
                functab.GetItemSize = OnGetItemSize2;
                m_UI_Model_EventType.m_sv_list_ListView.SetInitData(m_assetDic, functab);
                m_UI_Model_EventType.m_sv_list_ListView.FillContent(m_behaviorList.Count);
                m_isInitList = true;
            }
        }

        private string OnGetItemPrefabName2(ListView.ListItem listItem)
        {
            return "UI_Item_EventType3Item";
        }

        private float OnGetItemSize2(ListView.ListItem listItem)
        {
            return m_UI_Item_EventType3ItemHeight;
        }

        private void ItemEventByIndex2(ListView.ListItem listItem)
        {
            var behaviorData = m_behaviorList[listItem.index];
            if (listItem.data == null)
            {
                var subView = new UI_Item_EventType3Item_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.Refresh(behaviorData.Id);
            }
            else
            {
                var subView = listItem.data as UI_Item_EventType3Item_SubView;
                subView.Refresh(behaviorData.Id);
            }
        }

        #endregion

        #region 通用条件类活动

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

        public void RefreshList()
        {
            if (m_isInitList)
            {
                m_UI_Model_EventType.m_sv_list_ListView.FillContent(m_behaviorList.Count);
            }
            else
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex1;
                functab.GetItemPrefabName = OnGetItemPrefabName1;
                functab.GetItemSize = OnGetItemSize1;
                m_UI_Model_EventType.m_sv_list_ListView.SetInitData(m_assetDic, functab);
                m_UI_Model_EventType.m_sv_list_ListView.FillContent(m_behaviorList.Count);
                m_isInitList = true;
            }
        }

        private string OnGetItemPrefabName1(ListView.ListItem listItem)
        {
            return "UI_Item_EventType2Item";
        }

        private float OnGetItemSize1(ListView.ListItem listItem)
        {
            return m_UI_Item_EventType2ItemHeight;
        }

        private void ItemEventByIndex1(ListView.ListItem listItem)
        {
            var behaviorData = m_behaviorList[listItem.index];
            if (listItem.data == null)
            {
                var subView = new UI_Item_EventType2Item_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.BtnClickCallback = EventBtnReceiveRecord;
                subView.Refresh2(behaviorData.Id, m_preScheduleData, m_isExpire);
            }
            else
            {
                var subView = listItem.data as UI_Item_EventType2Item_SubView;
                subView.Refresh2(behaviorData.Id, m_preScheduleData, m_isExpire);
            }
        }

        #endregion

        #region 排行

        //排行
        private void OnRank()
        {
            if (m_rankPrefabLoadStatus == 2)
            {
                return;
            }
            else if (m_rankPrefabLoadStatus == 3)
            {
                ShowRank();
                return;
            }
            m_rankPrefabLoadStatus = 2;
            CoreUtils.assetService.Instantiate("UI_Item_EventTypeRank", RankPrefabLoadFinish);
        }

        private void RankPrefabLoadFinish(GameObject obj)
        {
            if (gameObject == null)
            {
                CoreUtils.assetService.Destroy(obj);
                return;
            }
            m_rankPrefabLoadStatus = 3;

            obj.transform.SetParent(m_UI_Model_EventType.gameObject.transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            RectTransform rectTrans = obj.GetComponent<RectTransform>();
            //rectTrans.offsetMin = Vector2.zero;
            //rectTrans.offsetMax = Vector2.zero;
            var subView = new UI_Item_EventTypeRank_SubView(rectTrans);
            subView.AddBackListener(OnBack);

            m_rankSubView = subView;
            m_rankNode = obj;

            ShowRank();
        }

        private void ShowRank()
        {
            m_UI_Model_EventType.m_pl_mes_CanvasGroup.gameObject.SetActive(false);
            m_rankNode.SetActive(true);
            var subView = m_rankSubView as UI_Item_EventTypeRank_SubView;
            subView.Refresh(m_preScheduleData.ActivityId, 1);
        }

        private void OnBack()
        {
            m_UI_Model_EventType.m_pl_mes_CanvasGroup.gameObject.SetActive(true);
            m_rankNode.SetActive(false);
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
            subView.Refresh2((int)acEventId, m_preScheduleData, m_isExpire);
        }

        #endregion   
    }
}