// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, 09 October 2020
// Update Time         :    Friday, 09 October 2020
// Class Description   :    NewRoleActivityMediator 新手活动
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using Data;
using System;
using Hotfix.Utils;

namespace Game {
    public class NewRoleActiveReward
    {
        public ActivityNewPlayerDefine Define; 
        public RewardGroupData RewardGroupData;
    }

    public class NewRoleActivityMediator : GameMediator {
        #region Member
        public static string NameMediator = "NewRoleActivityMediator";

        private ActivityProxy m_activityProxy;
        private PlayerProxy m_playerProxy;

        private int m_activityId;
        private ActivityCalendarDefine m_activityDefine;

        private long m_endTime;
        private Timer m_timer;
        private ActivityTimeInfo m_activityTimeInfo;
        private ActivityScheduleData m_scheduleData;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private Dictionary<Int64, RectTransform> m_eventRewardRecord = new Dictionary<Int64, RectTransform>();

        private List<ActivityBehaviorData> m_behaviorList;
        private int m_day;

        private bool m_isInitList;

        private int m_activityNewPlayerId;
        private ActivityNewPlayerDefine m_activityActiveDefine;

        private bool m_isInitTipList;

        List<NewRoleActiveReward> m_activeRewardList;

        private bool m_isInitRefresh;

        #endregion

        //IMediatorPlug needs
        public NewRoleActivityMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public NewRoleActivityView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {
                CmdConstant.ReceiveBehaviorReward,
                Activity_ReceiveReward.TagName,
                CmdConstant.ActivityScheduleUpdate,
                Activity_ReceiveActiveReward.TagName,
                CmdConstant.ActivityActivePointChange,
                CmdConstant.SystemDayChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            if (!m_isInitRefresh)
            {
                return;
            }

            switch (notification.Name)
            {
                case CmdConstant.ReceiveBehaviorReward://领取行为奖励
                    var reward = notification.Body as Activity_Reward.request;
                    if (!reward.HasActiveReward)
                    {
                        Int64 acEventId = reward.rewardId;
                        Debug.LogFormat("ReceiveBehaviorReward acEvent:{0}", acEventId);
                        RefreshEventItem(acEventId);
                    }
                    break;
                case Activity_ReceiveReward.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        //错误码处理
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        return;
                    }
                    ProcessReceiveReward(notification.Body);
                    break;
                case CmdConstant.ActivityScheduleUpdate:
                    List<Int64> list = notification.Body as List<Int64>;
                    if (list.Contains(m_activityId))
                    {
                        RefreshContent();
                    }
                    break;
                case Activity_ReceiveActiveReward.TagName: //领取活跃度奖励
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        //错误码处理
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        return;
                    }
                    ProcessActiveReward(notification.Body);
                    break;
                case CmdConstant.ActivityActivePointChange: //活跃度更新
                    RefreshActiveRewardStatus();
                    break;
                case CmdConstant.SystemDayChange:
                    RefreshContent();
                    break;
                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {

        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_isInitRefresh = false;
            m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            m_activityId = ActivityProxy.CreateRoleActivityId;
            m_activityDefine = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>(m_activityId);

            view.gameObject.SetActive(false);
            List<string> prefabList = new List<string>();
            prefabList.AddRange(view.m_sv_tips_ListView.ItemPrefabDataList);
            prefabList.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabList, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_info_GameButton.onClick.AddListener(OnInfo);
            view.m_btn_back_GameButton.onClick.AddListener(OnBack);
            view.m_btn_rewardsinfo_GameButton.onClick.AddListener(OnRewardInfo);
            view.m_btn_closeButton_GameButton.onClick.AddListener(CloseRewardTip);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            view.gameObject.SetActive(true);

            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            ReadData();
            Refresh();

            m_isInitRefresh = true;
        }

        private void RefreshContent()
        {
            ReadData();
            Refresh();
        }


        private void ReadData()
        {
            m_activityTimeInfo = m_activityProxy.GetActivityById(m_activityId);
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_activityId);
            if (m_activityTimeInfo ==null || m_scheduleData == null)
            {
                Timer.Register(0.02f, OnClose);
                return;
            }

            ReadDayNum();

            ReadBehaviorData();
        }

        private void ReadDayNum()
        {
            //Debug.LogErrorFormat("活动开始时间:{0}", m_activityTimeInfo.startTime);
            int diffDay = m_activityProxy.GetDiffDay(m_activityTimeInfo.startTime, ServerTimeModule.Instance.GetServerTime());
            if (diffDay <= 0)
            {
                diffDay = 1;
            }
            else if (diffDay > 7)
            {
                diffDay = 7;
            }
            m_day = diffDay;

            Debug.LogFormat("day:{0}", m_day);

            m_activityNewPlayerId = m_activityId * 100 + m_day;
            m_activityActiveDefine = CoreUtils.dataService.QueryRecord<ActivityNewPlayerDefine>(m_activityNewPlayerId);
            if (m_activityActiveDefine == null)
            {
                Debug.LogErrorFormat("ActivityNewPlayerDefine not find:{0}", m_activityNewPlayerId);
                return;
            }
        }

        private void ReadBehaviorData()
        {
            //子行为列表
            if (m_behaviorList == null)
            {
                m_behaviorList = new List<ActivityBehaviorData>();
            }
            m_behaviorList.Clear();
            List<ActivityBehaviorData> behaviorList = m_scheduleData.GetBehaviorList();
            for (int i = 0; i < behaviorList.Count; i++)
            {
                if (behaviorList[i].data1 == m_day)
                {
                    m_behaviorList.Add(behaviorList[i]);
                }
            }
            //数据排序
            if (m_behaviorList.Count > 0)
            {
                m_behaviorList.Sort(delegate (ActivityBehaviorData x, ActivityBehaviorData y)
                {
                    int re = GetReceiveStatus(y).CompareTo(GetReceiveStatus(x));
                    if (re == 0)
                    {
                        re = x.Id.CompareTo(y.Id);
                    }
                    return re;
                });
            }
        }

        private void Refresh()
        {
            view.m_lbl_title_LanguageText.text = LanguageUtils.getText(m_activityDefine.l_nameID);
            view.m_lbl_reset_LanguageText.text = LanguageUtils.getText(762021);
            ClientUtils.LoadSprite(view.m_img_right_bg_PolygonImage, m_activityDefine.background);

            //倒计时
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            if (m_activityTimeInfo.endTime > serverTime)
            {
                m_endTime = m_activityTimeInfo.endTime;
                UpdateTime();
                StartTimer();
            }
            else
            {
                CancelTimer();
                Timer.Register(0.02f, OnClose);
                return;
            }

            //刷新活跃度
            RefreshActive();

            //刷新列表
            RefreshBehaviorList();
        }

        private int GetReceiveStatus(ActivityBehaviorData data)
        {
            if (m_scheduleData.IsReceive(data.Id))
            {
                return 1;
            }
            else
            {
                ActivityDaysTypeDefine define = m_activityProxy.GetDayTypeDefine(data.Id);
                int val = m_scheduleData.GetBehaviorSchedule(define.playerBehavior, define.data3);
                if (val >= define.data0)
                {
                    return 3;
                }
                return 2;
            }
        }

        private void RefreshBehaviorList()
        {
            if (m_isInitList)
            {
                view.m_sv_list_ListView.FillContent(m_behaviorList.Count);
            }
            else
            {
                m_isInitList = true;
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex;
                view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
                view.m_sv_list_ListView.FillContent(m_behaviorList.Count);
            }
        }

        private void ItemEventByIndex(ListView.ListItem listItem)
        {
            var data = m_behaviorList[listItem.index];
            if (listItem.data == null)
            {
                var subView = new UI_Item_NewRoleActivityItem_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.BtnClickCallback = EventBtnReceiveRecord;
                subView.Refresh(data.Id, m_scheduleData);
            }
            else
            {
                var subView = listItem.data as UI_Item_NewRoleActivityItem_SubView;
                subView.Refresh(data.Id, m_scheduleData);
            }
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, view.m_sv_list_ListView);
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
                CoreUtils.uiManager.CloseUI(UI.s_newRoleActivity);
            }
            else
            {
                view.m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762019, ClientUtils.FormatCountDown((int)diffTime));
            }
        }

        #region 活跃度奖励

        //刷新活跃度
        private void RefreshActive()
        {
            if (m_activityActiveDefine == null)
            {
                return;
            }

            int packageId = m_activityActiveDefine.itemPackage * 1000 + 1;
            ItemPackageShowDefine packageDefine = CoreUtils.dataService.QueryRecord<ItemPackageShowDefine>(packageId);
            if (packageDefine != null)
            {
                if (packageDefine.showType == 1) //货币
                {
                    CurrencyDefine currencyDefine = CoreUtils.dataService.QueryRecord<CurrencyDefine>(packageDefine.showGroupIcon);
                    if (currencyDefine != null)
                    {
                        view.m_UI_Model_Item.Refresh(currencyDefine, packageDefine.number, false);
                        view.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.RemoveAllListeners();
                        view.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.AddListener(OnReceiveActiveReward);
                    }
                }
                else if (packageDefine.showType == 2)//物品
                {
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(packageDefine.showGroupIcon);
                    if (itemDefine != null)
                    {
                        view.m_UI_Model_Item.Refresh(itemDefine, (packageDefine.number > 1) ? packageDefine.number : 0, false);
                        view.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.RemoveAllListeners();
                        view.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.AddListener(OnReceiveActiveReward);
                    }
                }
            }

            //刷新奖励领取状态
            RefreshActiveRewardStatus();
        }

        //刷新奖励领取状态
        private void RefreshActiveRewardStatus()
        {
            long currVal = m_playerProxy.CurrentRoleInfo.activityActivePoint;

            if (m_activityActiveDefine != null)
            {
                if (currVal >= m_activityActiveDefine.standard)
                {
                    view.m_lbl_actcur_LanguageText.text = LanguageUtils.getTextFormat(300001, ClientUtils.FormatComma(currVal), ClientUtils.FormatComma(m_activityActiveDefine.standard));
                }
                else
                {
                    view.m_lbl_actcur_LanguageText.text = LanguageUtils.getTextFormat(182025, ClientUtils.FormatComma(currVal), ClientUtils.FormatComma(m_activityActiveDefine.standard));
                }
            }


            if (m_scheduleData != null && m_scheduleData.Info != null)
            {
                view.m_lbl_received_LanguageText.gameObject.SetActive(m_scheduleData.Info.activeReward);
                if (m_scheduleData.Info.activeReward) //已领取
                {
                    view.m_UI_Model_Item.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
                }
                else//未领取
                {
                    view.m_UI_Model_Item.m_img_redpoint_PolygonImage.gameObject.SetActive(currVal >= m_activityActiveDefine.standard);
                }
            }
            else//未领取
            {
                view.m_UI_Model_Item.m_img_redpoint_PolygonImage.gameObject.SetActive(currVal >= m_activityActiveDefine.standard);
                view.m_lbl_received_LanguageText.gameObject.SetActive(false);
            }
        }

        //领取活跃度奖励
        private void OnReceiveActiveReward()
        {
            if (m_activityActiveDefine == null)
            {
                Debug.Log("m_activityActiveDefine is null");
                return;
            }
            //是否可领取
            if (m_playerProxy.CurrentRoleInfo.activityActivePoint < m_activityActiveDefine.standard)
            {
                return;
            }

            //是否已领取
            if (m_scheduleData == null || m_scheduleData.Info == null)
            {
                return;
            }
            if (m_scheduleData.Info.activeReward)
            {
                return;
            }
            var sp = new Activity_ReceiveActiveReward.request();
            sp.activityId = m_activityId;
            sp.id = m_activityNewPlayerId;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //处理活跃度奖励领取表现
        private void ProcessActiveReward(object body)
        {
            var result = body as Activity_ReceiveActiveReward.response;
            if (result == null)
            {
                return;
            }
            if (!(result.activityId == m_activityId && result.id == m_activityNewPlayerId))
            {
                return;
            }

            //刷新显示状态
            RefreshActiveRewardStatus();

            if (result.rewardInfo == null)
            {
                return;
            }
            RectTransform rectTrans = view.m_UI_Model_Item.gameObject.GetComponent<RectTransform>();
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

        #endregion

        #region 活动信息

        private void OnInfo()
        {
            view.m_pl_mes.gameObject.SetActive(false);
            view.m_pl_info_CanvasGroup.gameObject.SetActive(true);
            view.m_lbl_info_LanguageText.text = LanguageUtils.getText(m_activityDefine.l_ruleID);
        }

        private void OnBack()
        {
            view.m_pl_mes.gameObject.SetActive(true);
            view.m_pl_info_CanvasGroup.gameObject.SetActive(false);
        }

        #endregion

        #region 奖励tip

        //奖励信息
        private void OnRewardInfo()
        {
            view.m_UI_Tip_reward_ViewBinder.gameObject.SetActive(true);

            if (m_activeRewardList == null)
            {
                m_activeRewardList = new List<NewRoleActiveReward>();

                List<ActivityNewPlayerDefine> defineList = new List<ActivityNewPlayerDefine>();
                for (int i = 1; i < 100; i++)
                {
                    int id = m_activityId * 100 + i;
                    ActivityNewPlayerDefine newPlayerDefine = CoreUtils.dataService.QueryRecord<ActivityNewPlayerDefine>(id);
                    if (newPlayerDefine == null)
                    {
                        break;
                    }
                    else
                    {
                        defineList.Add(newPlayerDefine);
                    }
                }
                var rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                for (int i = 0; i < defineList.Count; i++)
                {
                    var groupDataList = rewardGroupProxy.GetRewardDataByGroup(defineList[i].itemPackage);
                    for (int k = 0; k < groupDataList.Count; k++)
                    {
                        var rewardObj = new NewRoleActiveReward();
                        rewardObj.Define = defineList[i];
                        rewardObj.RewardGroupData = groupDataList[k];
                        m_activeRewardList.Add(rewardObj);
                    }
                }
            }

            if (LanguageUtils.IsArabic())
            {
                view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(false);
                view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
            }

            if (!m_isInitTipList)
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ListViewItemByIndex;
                view.m_sv_tips_ListView.SetInitData(m_assetDic, functab);
                m_isInitList = true;
            }

            view.m_sv_tips_ListView.FillContent(m_activeRewardList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            var data = m_activeRewardList[listItem.index];
            if (listItem.data == null)
            {
                var subView = new UI_Item_NewRoleActivityTipsItem_SubView(listItem.go.GetComponent<RectTransform>());
                subView.Refresh(data);
            }
            else
            {
                var subView = listItem.data as UI_Item_NewRoleActivityTipsItem_SubView;
                subView.Refresh(data);
            }
        }

        private void CloseRewardTip()
        {
            view.m_UI_Tip_reward_ViewBinder.gameObject.SetActive(false);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_newRoleActivity);
        }

        #endregion

        #region 行为奖励领取

        //行为事件奖励领取按钮记录
        private void EventBtnReceiveRecord(UI_Item_NewRoleActivityItem_SubView subView)
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
            ListView.ListItem listItem = view.m_sv_list_ListView.GetItemByIndex(findIndex);
            if (listItem == null || listItem.data == null || listItem.go == null)
            {
                return;
            }
            if (!listItem.go.activeSelf)
            {
                return;
            }
            UI_Item_NewRoleActivityItem_SubView subView = listItem.data as UI_Item_NewRoleActivityItem_SubView;
            subView.Refresh((int)acEventId, m_scheduleData);
        }

        #endregion   
    }
}