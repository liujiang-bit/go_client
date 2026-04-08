// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_EventStrongerPlayer2_SubView 活动 最强执政官 进行中 结束
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using System;
using System.Collections.Generic;
using Data;
using PureMVC.Interfaces;

namespace Game {
    public partial class UI_Item_EventStrongerPlayer2_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private RewardGroupProxy m_rewardGroupProxy;

        private bool m_isInit;
        private bool m_assetIsLoadFinish;
        private bool m_isInitList;
        private ActivityItemData m_menuData;

        private Timer m_timer;
        private Int64 m_endTime;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private Dictionary<Int64, RectTransform> m_eventRewardRecord = new Dictionary<Int64, RectTransform>();

        private List<UI_Item_EventStrongerPlayerCK_SubView> m_ckViewList;
        private List<UI_Model_RewardGet_SubView> m_itemViewList;

        private int m_currStage;
        private List<int> m_stageTimeList;
        private Int64 m_stageEndTime;
        private string m_stateDesc = "";
        private ActivityKillTypeDefine m_stageDefine;

        private List<ActivityBehaviorData> m_behaviorList;

        private ActivityScheduleData m_scheduleData;

        private int m_activityType;
        private bool m_tipPosRefresh;

        private int m_rankPrefabLoadStatus = 1; //1尚未加载 2加载中 3已加载完成
        private GameObject m_rankNode;
        private object m_rankSubView;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.ActivityRankOrScoreUpdate,
                    CmdConstant.ReceiveBehaviorReward,
                    Activity_ReceiveReward.TagName,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ActivityRankOrScoreUpdate://积分 排名 变更刷新
                    Int64 activityId = (Int64)notification.Body;
                    if (m_menuData != null && m_menuData.Define != null && m_menuData.Define.ID == activityId)
                    {
                        if (m_isInit && m_assetIsLoadFinish)
                        {
                            RefreshRank();
                            RefreshScore();
                            RefreshList();
                        }
                    }
                    break;
                case CmdConstant.ReceiveBehaviorReward: //领取行为奖励
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

                m_ckViewList = new List<UI_Item_EventStrongerPlayerCK_SubView>();
                m_ckViewList.Add(m_ck_day1);
                m_ckViewList.Add(m_ck_day2);
                m_ckViewList.Add(m_ck_day3);
                m_ckViewList.Add(m_ck_day4);
                m_ckViewList.Add(m_ck_day5);
                for (int i = 0; i < m_ckViewList.Count; i++)
                {
                    m_ckViewList[i].m_UI_Item_EventStrongerPlayerCK_GameToggle.interactable = false;
                    m_ckViewList[i].m_UI_Item_EventStrongerPlayerCK_GameToggle.isOn = false;
                }

                m_itemViewList = new List<UI_Model_RewardGet_SubView>();
                m_itemViewList.Add(m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_Item3);
                for (int i = 0; i < m_itemViewList.Count; i++)
                {
                    m_itemViewList[i].SetScale(m_pl_item_ArabLayoutCompment.transform.localPosition.x);
                }

                m_btn_info_GameButton.onClick.AddListener(OnInfo);
                m_btn_back_GameButton.onClick.AddListener(OnBack);
                m_btn_rank_GameButton.onClick.AddListener(OnRank);

                m_btn_question_GameButton.onClick.AddListener(OnQuest);
                m_btn_help_GameButton.onClick.AddListener(OnQuest);

                //预加载列表预设
                List<string> prefabNames = new List<string>();
                prefabNames.AddRange(m_sv_list_ListView.ItemPrefabDataList);
                ClientUtils.PreLoadRes(gameObject, prefabNames, LoadFinish);

                m_isInit = true;
            }
            else
            {
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
            gameObject.SetActive(true);
            m_assetIsLoadFinish = true;
            Refresh();
        }

        public void RefreshView()
        {
            if (m_isInit && m_assetIsLoadFinish)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            m_lbl_title_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_nameID);
            ClientUtils.LoadSprite(m_img_top_PolygonImage, m_menuData.Define.background);

            ActivityTimeInfo activityInfo = m_menuData.Data;
            DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.startTime);
            DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.endTime);
            m_lbl_lifeDay_LanguageText.text = LanguageUtils.getTextFormat(762020, startTime.ToString("yyyy/MM/dd"), endTime.ToString("yyyy/MM/dd"));

            m_activityType = m_menuData.Define.activityType;
            if (m_activityType == 301) //活动进行中
            {
                ActivityStart();
            }
            else if (m_activityType == 302) //活动结束
            {
                ActivityEnd();
            }
        }

        private void ActivityStart()
        {
            ActivityTimeInfo activityInfo = m_menuData.Data;
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();

            if (activityInfo.endTime <= serverTime)
            {
                CancelTimer();
                CoreUtils.uiManager.CloseUI(UI.s_eventDate);
                return;
            }

            ReadData();

            m_pl_result.gameObject.SetActive(false);
            m_pl_rank.gameObject.SetActive(true);
            m_sv_list_ListView.gameObject.SetActive(true);
            if (!m_pl_pos_ArabLayoutCompment.gameObject.activeSelf)
            {
                m_pl_pos_ArabLayoutCompment.gameObject.SetActive(true);
            }

            //我的排名
            RefreshRank();

            //我的积分
            RefreshScore();

            //阶段显示刷新
            StageRefresh();

            //倒计时
            if (activityInfo.endTime > serverTime)
            {
                m_endTime = activityInfo.endTime;
                UpdateTime();
                StartTimer();
            }

            //奖励组
            RefreshRewardGroup(m_menuData.Define.itemPackage);

            //刷新列表
            RefreshList();
        }

        private void ReadData()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);

            m_behaviorList = m_scheduleData.GetBehaviorList();

            ReadStageData();
        }

        private void RefreshRank()
        {
            string rankStr = "";
            if (m_scheduleData.Info.score > 0 && m_scheduleData.Info.rank == 0)
            {
                rankStr = ActivityProxy.RankNumFormat(m_scheduleData.Info.rank, 999, true);
            }
            else
            {
                rankStr = ActivityProxy.RankNumFormat(m_scheduleData.Info.rank);
            }
            m_lbl_myRank_LanguageText.text = LanguageUtils.getTextFormat(762200, rankStr);
        }

        private void RefreshScore()
        {
            m_lbl_myScore_LanguageText.text = LanguageUtils.getTextFormat(762201, ClientUtils.FormatComma(m_scheduleData.Info.score));
        }

        private void RefreshList()
        {
            if (m_isInitList)
            {
                m_sv_list_ListView.FillContent(m_behaviorList.Count);
            }
            else
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ItemEventByIndex;
                m_sv_list_ListView.SetInitData(m_assetDic, functab);
                m_sv_list_ListView.FillContent(m_behaviorList.Count);
                m_isInitList = true;
            }
        }

        private void ItemEventByIndex(ListView.ListItem listItem)
        {
            var behaviorData = m_behaviorList[listItem.index];
            if (listItem.data == null)
            {
                var subView = new UI_Item_EventType2Item_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.BtnClickCallback = EventBtnReceiveRecord;
                subView.Refresh3(behaviorData, m_scheduleData);
            }
            else
            {
                var subView = listItem.data as UI_Item_EventType2Item_SubView;
                subView.Refresh3(behaviorData, m_scheduleData);
            }
        }

        //读取阶段数据
        private void ReadStageData()
        {
            ActivityTimeInfo activityInfo = m_menuData.Data;
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            int level = (int)m_scheduleData.Info.level;

            if (m_stageTimeList == null)
            {
                m_stageTimeList = new List<int>();
                int times = 0;
                for (int i = 1; i < 6; i++)
                {
                    ActivityKillTypeDefine define = m_activityProxy.GetStageDefine(m_menuData.Define.ID, i, level);
                    times = times + define.continueTime;
                    m_stageTimeList.Add(times);
                }
            }

            m_currStage = (int)m_scheduleData.Info.stage;
            m_stageEndTime = m_stageTimeList[m_currStage-1] + activityInfo.startTime - 1;
            m_stageDefine = m_activityProxy.GetStageDefine(m_menuData.Define.ID, m_currStage, level);
            m_stateDesc = LanguageUtils.getText(m_stageDefine.l_nameID);
            Debug.LogFormat("ActivityKillType id:{0} stage:{1}", m_stageDefine.ID, m_currStage);
        }

        //刷新阶段
        private void StageRefresh()
        {
            for (int i = 0; i < 6; i++)
            {
                if (i + 1 <= m_currStage)
                {
                    m_ckViewList[i].m_UI_Item_EventStrongerPlayerCK_GameToggle.isOn = true;
                }
            }

            RectTransform trans = m_pl_pos_ArabLayoutCompment.GetComponent<RectTransform>();
            trans.anchorMin = new Vector2(0, 0);
            trans.anchorMax = new Vector2(0, 0);
            trans.offsetMin = new Vector2(0, 0);
            trans.offsetMax = new Vector2(0, 0);

            if (m_tipPosRefresh)
            {
                RefreshTipPos();
                return;
            }
            m_pl_offset_Animator.gameObject.SetActive(false);
            Timer.Register(0.02f, () =>
            {
                m_tipPosRefresh = true;
                if (gameObject == null)
                {
                    return;
                }
                RefreshTipPos();
            });
        }

        private void RefreshTipPos()
        {
            Vector3 screenPoint = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(m_ckViewList[m_currStage - 1].m_UI_Item_EventStrongerPlayerCK_GameToggle.transform.position);
            Vector2 localpos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_pl_pos_ArabLayoutCompment.GetComponent<RectTransform>(), screenPoint, CoreUtils.uiManager.GetUICamera(), out localpos);
            m_pl_offset_Animator.transform.localPosition = new Vector3(localpos.x, localpos.y - m_ckViewList[m_currStage - 1].m_root_RectTransform.rect.height);
            if (!m_pl_offset_Animator.gameObject.activeSelf)
            {
                m_pl_offset_Animator.gameObject.SetActive(true);
            }
        }

        //刷新奖励组物品
        private void RefreshRewardGroup(int itemPackage)
        {
            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            for (int i = 0; i < 3; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].SetScale(0.25f);
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
            if (m_activityType == 301)
            {
                if (m_timer == null)
                {
                    m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, m_sv_list_ListView);
                }
            }
            else if (m_activityType == 302)
            {
                if (m_timer == null)
                {
                    m_timer = Timer.Register(1.0f, UpdateTime2, null, true, true, m_sv_list_ListView);
                }
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
                return;
            }
            else
            {
                m_lbl_lifeTime_LanguageText.text = LanguageUtils.getTextFormat(762019, ClientUtils.FormatCountDown((int)diffTime));
            }
            Int64 diffTime2 = m_stageEndTime - serverTime;
            if (diffTime2 < 0)
            {
                diffTime2 = 0;
            }
            m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762199, m_currStage, m_stateDesc, ClientUtils.FormatCountDown((int)diffTime2));
        }

        private void ActivityEnd()
        {
            ReadData2();

            m_pl_rank.gameObject.SetActive(false);
            m_sv_list_ListView.gameObject.SetActive(false);
            m_pl_pos_ArabLayoutCompment.gameObject.SetActive(false);

            m_pl_result.gameObject.SetActive(true);
            gameObject.SetActive(true);

            if (m_scheduleData == null)
            {
                Debug.LogErrorFormat("ActivitySchedule not find:{0}", m_menuData.Define.ID);
                return;
            }

            CancelTimer();

            ActivityTimeInfo activityInfo = m_menuData.Data;
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            if (activityInfo.endTime <= serverTime)
            {
                CoreUtils.uiManager.CloseUI(UI.s_eventDate);
                return;
            }

            for (int i = 0; i < m_ckViewList.Count; i++)
            {
                m_ckViewList[i].m_UI_Item_EventStrongerPlayerCK_GameToggle.isOn = true;
            }

            AllianceProxy allianceProxy = allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_lbl_lifeTime_LanguageText.text = LanguageUtils.getText(762210);           
            m_lbl_name_LanguageText.text = allianceProxy.GetName();
            m_lbl_source_LanguageText.text = ClientUtils.FormatComma(m_scheduleData.Info.score);

            //头像
            m_UI_PlayerHead.LoadPlayerIcon();

            //阶段排名
            if (m_scheduleData.Info.ranks != null)
            {
                m_lbl_stageRank1_LanguageText.text = m_scheduleData.Info.ranks.ContainsKey(1)? ActivityProxy.RankNumFormat(m_scheduleData.Info.ranks[1].rank):"0";
                m_lbl_stageRank2_LanguageText.text = m_scheduleData.Info.ranks.ContainsKey(2)? ActivityProxy.RankNumFormat(m_scheduleData.Info.ranks[2].rank) : "0";
                m_lbl_stageRank3_LanguageText.text = m_scheduleData.Info.ranks.ContainsKey(3)? ActivityProxy.RankNumFormat(m_scheduleData.Info.ranks[3].rank) : "0";
                m_lbl_stageRank4_LanguageText.text = m_scheduleData.Info.ranks.ContainsKey(4)? ActivityProxy.RankNumFormat(m_scheduleData.Info.ranks[4].rank) : "0";
                m_lbl_stageRank5_LanguageText.text = m_scheduleData.Info.ranks.ContainsKey(5)? ActivityProxy.RankNumFormat(m_scheduleData.Info.ranks[5].rank) : "0";
            }
            //总排名
            m_lbl_stageRank6_LanguageText.text = m_scheduleData.Info.ranks.ContainsKey(6) ? ActivityProxy.RankNumFormat(m_scheduleData.Info.ranks[6].rank) : "0";

            //倒计时
            if (activityInfo.endTime > serverTime)
            {
                m_endTime = activityInfo.endTime;
                UpdateTime2();
                StartTimer();
            }

            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)m_scheduleData.PreActivityId);
            if (define != null)
            {
                //奖励组
                RefreshRewardGroup(define.itemPackage);
            }
        }

        private void ReadData2()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);
        }

        private void UpdateTime2()
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
                m_lbl_result_LinkImageText.text = LanguageUtils.getTextFormat(762209, m_scheduleData.Info.season, ClientUtils.FormatCountDown((int)diffTime));
            } 
        }

        private void OnQuest()
        {
            HelpTip.CreateTip(LanguageUtils.getText(m_stageDefine.l_tipsID), m_btn_question_GameButton.GetComponent<RectTransform>())
                .SetStyle(HelpTipData.Style.arrowUp)
                .SetOffset(10)
                .Show();
        }

        #region 活动信息

        private void OnInfo()
        {
            if (!(m_isInit && m_assetIsLoadFinish))
            {
                return;
            }
            m_pl_info_CanvasGroup.gameObject.SetActive(true);
            m_pl_mes_CanvasGroup.gameObject.SetActive(false);

            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)m_scheduleData.PreActivityId);

            m_lbl_info_LanguageText.text = LanguageUtils.getText(define.l_ruleID);
        }


        private void OnBack()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(false);
            m_pl_mes_CanvasGroup.gameObject.SetActive(true);
        }

        #endregion

        #region 排行

        //排行
        private void OnRank()
        {
            if (m_scheduleData == null)
            {
                return;
            }
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

            obj.transform.SetParent(gameObject.transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            RectTransform rectTrans = obj.GetComponent<RectTransform>();
            //rectTrans.offsetMin = Vector2.zero;
            //rectTrans.offsetMax = Vector2.zero;
            var subView = new UI_Item_EventTypeRank_SubView(rectTrans);
            subView.AddBackListener(OnBack2);

            m_rankSubView = subView;
            m_rankNode = obj;

            ShowRank();
        }

        private void ShowRank()
        {
            m_pl_mes_CanvasGroup.gameObject.SetActive(false);
            m_rankNode.SetActive(true);
            var subView = m_rankSubView as UI_Item_EventTypeRank_SubView;

            int type = 0;
            if (m_activityType == 301) //活动进行中
            {
                type = 2;
            }
            else if (m_activityType == 302) //活动结束
            {
                type = 1;
            }

            subView.Refresh(m_scheduleData.PreActivityId, 2, type);
        }

        private void OnBack2()
        {
            m_pl_mes_CanvasGroup.gameObject.SetActive(true);
            m_rankNode.gameObject.SetActive(false);
        }

        #endregion

        #region 行为奖励领取

        //行为事件奖励领取按钮记录
        private void EventBtnReceiveRecord(UI_Item_EventType2Item_SubView subView)
        {
            int index = subView.GetIndex();
            m_eventRewardRecord[index] = subView.m_btn_get.m_root_RectTransform;
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
            if (result.HasIndex)
            {
                m_eventRewardRecord.TryGetValue(result.index, out rectTrans);
                m_eventRewardRecord.Remove(result.index);
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
                if (acEventId == m_behaviorList[i].PlayerBehavior)
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
            subView.Refresh3(m_behaviorList[findIndex], m_scheduleData);
        }

        #endregion
    }
}