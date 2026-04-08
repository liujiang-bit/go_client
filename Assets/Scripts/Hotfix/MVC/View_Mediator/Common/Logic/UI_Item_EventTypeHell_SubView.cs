// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_EventTypeHell_SubView 地狱活动
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using SprotoType;
using System;
using Data;
using PureMVC.Interfaces;

namespace Game {
    public partial class UI_Item_EventTypeHell_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private RewardGroupProxy m_rewardGroupProxy;

        private ActivityItemData m_menuData;

        private bool m_isInit;

        private ActivityScheduleData m_scheduleData;
        private List<ActivityBehaviorData> m_behaviorList;

        private Timer m_timer;
        private Int64 m_endTime;

        private int m_maxScore;
        private int m_lastType;
        private List<UI_Item_EventBtnSource_SubView> m_sourceViewList;
        private List<UI_Item_EventHell_SubView> m_boxList;
        private ActivityInfernalDefine m_infernalDefine;

        private int m_rankPrefabLoadStatus = 1; //1尚未加载 2加载中 3已加载完成
        private GameObject m_rankNode;
        private object m_rankSubView;

        private bool m_isRequesting;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.ActivityRankOrScoreUpdate,
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
                        if (m_isInit)
                        {
                            RefreshRank();
                            RefreshScore();
                            RefreshSilder();
                            RefreshBoxStatus();
                        }
                    }
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

            var sp = new Activity_GetSelfRank.request();
            sp.activityId = m_menuData.Define.ID;
            Debug.LogFormat("activityId: {0}", sp.activityId);
            AppFacade.GetInstance().SendSproto(sp);

            m_UI_Tip_BoxReward.gameObject.SetActive(false);
            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

                m_sourceViewList = new List<UI_Item_EventBtnSource_SubView>();
                m_sourceViewList.Add(m_btn_source1);
                m_sourceViewList.Add(m_btn_source2);
                m_sourceViewList.Add(m_btn_source3);

                m_boxList = new List<UI_Item_EventHell_SubView>();
                m_boxList.Add(m_UI_Item_EventHell3);
                m_boxList.Add(m_UI_Item_EventHell2);
                m_boxList.Add(m_UI_Item_EventHell1);

                m_btn_info_GameButton.onClick.AddListener(OnInfo);
                m_btn_back_GameButton.onClick.AddListener(OnBack);
                m_btn_rank_GameButton.onClick.AddListener(OnRank);

                m_lbl_score_LanguageText.text = LanguageUtils.getText(762220);

                m_isInit = true;
            }

            ReadData();
            Refresh();
        }

        private void ReadData()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);
            m_behaviorList = m_scheduleData.GetBehaviorList();

            m_infernalDefine = CoreUtils.dataService.QueryRecord<ActivityInfernalDefine>(m_behaviorList[0].Id);

            m_maxScore = m_behaviorList[2].Count;
        }

        private void Refresh()
        {
            m_lbl_title_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_taglineID);
            ClientUtils.TextSetColor(m_lbl_title_LanguageText, m_menuData.Define.taglineColour);

            RefreshScore();

            RefreshRank();

            ActivityTimeInfo activityInfo = m_menuData.Data;
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
                CoreUtils.uiManager.CloseUI(UI.s_eventDate);
                return;
            }

            //刷新奖励组
            RefreshRewardGroup(m_menuData.Define.itemPackage);

            //刷新行为
            RefreshBehavior();

            //更新进度
            RefreshSilder();

            //刷新宝箱
            RefreshBox();

            gameObject.SetActive(true);
        }

        private void RefreshBox()
        {
            for (int i = 0; i < 3; i++)
            {
                m_boxList[i].m_lbl_name0_LanguageText.text = LanguageUtils.getText(762223);
                m_boxList[i].m_lbl_source0_LanguageText.text = ClientUtils.FormatComma(m_infernalDefine.score[i]);
                m_boxList[i].m_lbl_name1_LanguageText.text = LanguageUtils.getText(762224);
                m_boxList[i].m_lbl_source1_LanguageText.text = ClientUtils.FormatComma(m_infernalDefine.worth[i]);
                m_boxList[i].Refresh(i);
                m_boxList[i].BtnAction = ClickBox;
                m_boxList[i].RefreshBoxStatus(GetBoxStatus(i));
            }
        }

        private int GetBoxStatus(int index)
        {
            int status = 0;
            if (m_scheduleData.IsReceive2(index + 1)) //已领取
            {
                status = (int)EventHellBoxStatus.AlreadyReceive;
            }
            else
            {
                if (m_scheduleData.Info.score >= m_infernalDefine.score[index])//可领取
                {
                    status = (int)EventHellBoxStatus.CanReceive;
                }
                else//不可领取
                {
                    status = (int)EventHellBoxStatus.NotCanReceive;
                }
            }
            return status;
        }

        //点击宝箱
        private void ClickBox(UI_Item_EventHell_SubView subView)
        {
            int index = subView.GetIndex();
            int boxStatus = subView.GetBoxStatus();
            if (boxStatus != (int)EventHellBoxStatus.CanReceive)
            {
                int itemPackage = m_infernalDefine.reward[index];
                //List<RewardGroupData> groupList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
                //ClientUtils.Print(groupList);
                //显示tip
                m_UI_Tip_BoxReward.gameObject.SetActive(true);
                m_UI_Tip_BoxReward.SetInfo2(itemPackage, 
                                            m_boxList[index].m_btn_box_GameButton.transform.position, 
                                            m_boxList[index].m_btn_box_GameButton.GetComponent<RectTransform>().rect.width/2);
                return;
            }
            if (m_isRequesting)
            {
                return;
            }
            m_isRequesting = true;
            //领取奖励
            var sp = new Activity_ReceiveReward.request();
            sp.activityId = m_menuData.Define.ID;
            sp.index = index+1;
            AppFacade.GetInstance().SendSproto(sp);
            Debug.LogFormat("sp.activityId: {0} index:{1}", sp.activityId, sp.index);
        }

        private void RefreshBoxStatus()
        {
            for (int i = 0; i < 3; i++)
            {
                m_boxList[i].RefreshBoxStatus(GetBoxStatus(i));
            }
        }

        private void RefreshSilder()
        {
            int total = 0;
            float currProcess = 0f;
            for (int i = 0; i < 3; i++)
            {
                if (m_scheduleData.Info.score >= m_behaviorList[i].Count)
                {
                    total = total + 1;
                }
                else
                {
                    float score1 = m_scheduleData.Info.score - m_behaviorList[i].Condition;
                    float score2 = m_behaviorList[i].Count - m_behaviorList[i].Condition;
                    currProcess = score1 / score2;
                    break;
                }
            }
            float process = (float)total / 3 + currProcess/3;
            m_pb_rogressBar0_GameSlider.value = process; //黄色

            float greenProcess = 0f;
            if (process >= 1)
            {
                greenProcess = 1f;
            }
            else if (process >= ((float)2 / 3))
            {
                greenProcess = (float)2 / 3;
            }
            else if (process >= ((float)1 / 3))
            {
                greenProcess = (float)1 / 3;
            }
            else
            {
                greenProcess = 0f;
            }
            m_pb_rogressBar_GameSlider.value = greenProcess; //绿色
        }

        private void RefreshBehavior()
        {
            if (m_scheduleData.Info.ids == null)
            {
                return;
            }
            int count = m_scheduleData.Info.ids.Count;
            for (int i = 0; i < 3; i++)
            {
                if (i < count)
                {
                    m_sourceViewList[i].gameObject.SetActive(true);
                    m_sourceViewList[i].Refresh((int)m_scheduleData.Info.ids[i]);
                }
                else
                {
                    m_sourceViewList[i].gameObject.SetActive(false);
                }
            }
        }

        private void RefreshScore()
        {
            m_lbl_score_val_LanguageText.text = ClientUtils.FormatComma(m_scheduleData.Info.score);
        }

        private void RefreshRank()
        {
            if (m_scheduleData.Info.score >= m_maxScore) //显示排名
            {
                m_lbl_difference_LanguageText.text = LanguageUtils.getText(762222);
                m_lbl_difference_val_LanguageText.text = m_scheduleData.Info.rank.ToString();
            }
            else
            {
                int num = 0;
                for (int i = 0; i < m_behaviorList.Count; i++)
                {
                    if (m_scheduleData.Info.score < m_behaviorList[i].Count)
                    {
                        num = m_behaviorList[i].Count - (int)m_scheduleData.Info.score;
                        break;
                    }
                }
                m_lbl_difference_LanguageText.text = LanguageUtils.getText(762221);
                m_lbl_difference_val_LanguageText.text = ClientUtils.FormatComma(num);
            }
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, m_lbl_title_LanguageText);
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
                Tip.CreateTip(LanguageUtils.getText(762232)).Show();
                //倒计时结束 关闭界面
                CancelTimer();
                if (m_sourceViewList != null)
                {
                    for (int i = 0; i < m_sourceViewList.Count; i++)
                    {
                        m_sourceViewList[i].CloseTip();
                    }
                }
                CoreUtils.uiManager.CloseUI(UI.s_eventDate);
            }
            else
            {
                int type = diffTime > 600 ? 1 : 2;
                if (type != m_lastType)
                {
                    m_lastType = type;
                    m_img_time_PolygonImage.color = (m_lastType == 1) ? Color.green : Color.red;
                }
                m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(787003, ClientUtils.FormatCountDown((int)diffTime));
            }
        }

        //刷新奖励组物品
        private void RefreshRewardGroup(int itemPackage)
        {
            List<UI_Model_RewardGet_SubView> itemViewList = new List<UI_Model_RewardGet_SubView>();
            itemViewList.Add(m_UI_Model_Item1);
            itemViewList.Add(m_UI_Model_Item2);
            itemViewList.Add(m_UI_Model_Item3);

            float scale = m_pl_item_ArabLayoutCompment.transform.localScale.x;

            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            for (int i = 0; i < 3; i++)
            {
                if (i < count)
                {
                    itemViewList[i].gameObject.SetActive(true);
                    itemViewList[i].RefreshByGroup(groupDataList[i], 2);
                    itemViewList[i].SetScale(scale);
                }
                else
                {
                    itemViewList[i].gameObject.SetActive(false);
                }
            }
        }

        #region 信息

        private void OnInfo()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(true);
            m_pl_mes_CanvasGroup.gameObject.SetActive(false);

            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)m_scheduleData.ActivityId);
            m_lbl_info_LanguageText.text = LanguageUtils.getText(define.l_ruleID);
        }

        private void OnBack()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(false);
            m_pl_mes_CanvasGroup.gameObject.SetActive(true);
        }

        #endregion

        #region 排行奖励

        private void OnRank()
        {
            EventTypeRankParam param = new EventTypeRankParam();
            param.ActivityId = m_menuData.Define.ID;
            param.SubId = m_infernalDefine.ID;
            param.Type = 3;
            CoreUtils.uiManager.ShowUI(UI.s_eventTypeRankReward, null, param);
        }

        #endregion

        //处理奖励信息
        private void ProcessReceiveReward(object body)
        {
            m_isRequesting = false;
            var result = body as Activity_ReceiveReward.response;
            if (result == null)
            {
                return;
            }
            //ClientUtils.Print(result);
            if (result.rewardInfo == null)
            {
                return;
            }

            if (gameObject == null)
            {
                return;
            }

            if (!result.HasIndex)
            {
                return;
            }

            int index = (int)result.index - 1;

            if (index < m_boxList.Count)
            {
                //开宝箱动画
                m_boxList[index].OpenBoxAni(()=> {
                    if (gameObject == null)
                    {
                        return;
                    }
                    //宝箱开启动画表现完再更新状态
                    if (index < m_boxList.Count)
                    {
                        m_boxList[index].RefreshBoxStatus(GetBoxStatus(index));
                    }
                });
            }

            //奖励飘飞表现
            RectTransform rectTrans = null;
            if (index >= 0 && index < m_boxList.Count)
            {
                rectTrans = m_boxList[index].m_root_RectTransform;
                m_boxList[index].ChangeBoxStatus(GetBoxStatus(index));
            }
            if (rectTrans == null)
            {
                return;
            }
            //飘飞特效
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            if (result.rewardInfo.items != null)
            {
                //CoreUtils.uiManager.ShowUI(UI.s_rewardGetWin, null, result.rewardInfo);
                for (int i = 0; i < result.rewardInfo.items.Count; i++)
                {
                    mt.FlyItemEffect((int)result.rewardInfo.items[i].itemId,
                                     (int)result.rewardInfo.items[i].itemNum,
                                     rectTrans);
                }
            }
        }
    }
}