// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月11日
// Update Time         :    2020年9月11日
// Class Description   :    UI_Item_EventTribeKing_SubView 活动 战斗的号角&部落之王
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
    public partial class UI_Item_EventTribeKing_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private PlayerProxy m_playerProxy;
        private RewardGroupProxy m_rewardGroupProxy;

        private bool m_isInit;
        private ActivityItemData m_menuData;

        private Timer m_timer;
        private Int64 m_endTime;

        private int m_activityType;
        private List<ActivityBehaviorData> m_behaviorList;
        private ActivityScheduleData m_scheduleData;

        private List<UI_Model_RewardGet_SubView> m_itemViewList;
        private List<UI_Item_EventTribeKingBox_SubView> m_boxList;

        private long m_score;

        private int m_rankPrefabLoadStatus = 1; //1尚未加载 2加载中 3已加载完成
        private GameObject m_rankNode;
        private object m_rankSubView;

        private List<float> m_widthList;

        private Activity_Rank.request m_rankData;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    Activity_Rank.TagName,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Activity_Rank.TagName:
                    Activity_Rank.request result = notification.Body as Activity_Rank.request;
                    if (result == null)
                    {
                        Debug.LogError("Activity_Rank return null");
                        return;
                    }
                    if (m_scheduleData != null && m_scheduleData.PreActivityId == result.activityId)
                    {
                        m_rankData = result;
                        RefreshContent();
                    }
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

                m_itemViewList = new List<UI_Model_RewardGet_SubView>();
                m_itemViewList.Add(m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_Item3);

                m_boxList = new List<UI_Item_EventTribeKingBox_SubView>();
                m_boxList.Add(m_UI_Item_EventTribeKingBox1);
                m_boxList.Add(m_UI_Item_EventTribeKingBox2);
                m_boxList.Add(m_UI_Item_EventTribeKingBox3);
                m_boxList.Add(m_UI_Item_EventTribeKingBox4);

                for (int i = 0; i < m_boxList.Count; i++)
                {
                    m_boxList[i].BtnClickEvent = ClickBox;
                    m_boxList[i].Init(i);
                }

                m_btn_info_GameButton.onClick.AddListener(OnInfo);
                m_btn_back_GameButton.onClick.AddListener(OnBack);
                m_btn_rank_GameButton.onClick.AddListener(OnRank);

                m_isInit = true;
            }

            ReadData();
            RequestRankData();

            //RefreshContent();
        }

        private void RefreshContent()
        {
            gameObject.SetActive(true);

            //m_rankData = new Activity_Rank.request();
            //m_rankData.score = 0;

            if (m_activityType == 303 || m_activityType == 304)
            {
                m_score = m_rankData.score;
            }
            else
            {
                m_score = m_rankData.allianceScore;
            }

            ActivityTimeInfo activityInfo = m_menuData.Data;
            DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.startTime);
            DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.endTime);
            m_lbl_title_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_nameID);
            m_lbl_coming_LanguageText.text = LanguageUtils.getTextFormat(762020, startTime.ToString("yyyy/MM/dd"), endTime.ToString("yyyy/MM/dd"));
            ClientUtils.LoadSprite(m_img_activitybg_PolygonImage, m_menuData.Define.background);
            m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_taglineID);

            //奖励组
            RefreshRewardGroup(m_menuData.Define.itemPackage);

            //刷新积分排名
            RefreshScoreRank();

            //刷新宝箱
            RefreshBox();

            //倒计时
            if (m_activityType == 303 || m_activityType == 305)
            {
                Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
                if (activityInfo.endTime <= serverTime)
                {
                    CancelTimer();
                    CoreUtils.uiManager.CloseUI(UI.s_eventDate);
                    return;
                }
                m_endTime = activityInfo.endTime;
                UpdateTime();
                StartTimer();
            }
            else
            {
                m_lbl_time_LanguageText.text = LanguageUtils.getText(762136);
            }
        }

        private void RequestRankData()
        {
            var sp = new Activity_GetSelfRank.request();
            sp.activityId = m_scheduleData.PreActivityId;
            AppFacade.GetInstance().SendSproto(sp);
        }

        //刷新积分排名
        private void RefreshScoreRank()
        {
            if (m_activityType == 303 || m_activityType == 304)
            {
                m_UI_Item_EventTribeKingScore1.gameObject.SetActive(true);
                m_UI_Item_EventTribeKingScore2.gameObject.SetActive(true);
                m_UI_Item_EventTribeKingScore3.gameObject.SetActive(true);

                //个人积分
                m_UI_Item_EventTribeKingScore1.m_lbl_score_LanguageText.text = LanguageUtils.getText(762246);
                m_UI_Item_EventTribeKingScore1.m_lbl_score_num_LanguageText.text = ClientUtils.FormatComma(m_rankData.score);

                //您的排名
                m_UI_Item_EventTribeKingScore2.m_lbl_score_LanguageText.text = LanguageUtils.getText(762247);
                m_UI_Item_EventTribeKingScore2.m_lbl_score_num_LanguageText.text = ClientUtils.FormatComma(m_rankData.rank);

                //联盟排名
                m_UI_Item_EventTribeKingScore3.m_lbl_score_LanguageText.text = LanguageUtils.getText(762248);
                m_UI_Item_EventTribeKingScore3.m_lbl_score_num_LanguageText.text = ClientUtils.FormatComma(m_rankData.allianceRank);
            }
            else
            {
                m_UI_Item_EventTribeKingScore1.gameObject.SetActive(true);
                m_UI_Item_EventTribeKingScore2.gameObject.SetActive(false);
                m_UI_Item_EventTribeKingScore3.gameObject.SetActive(true);

                //联盟积分
                m_UI_Item_EventTribeKingScore1.m_lbl_score_LanguageText.text = LanguageUtils.getText(124036);
                m_UI_Item_EventTribeKingScore1.m_lbl_score_num_LanguageText.text = ClientUtils.FormatComma(m_rankData.allianceScore);

                //联盟排名
                m_UI_Item_EventTribeKingScore3.m_lbl_score_LanguageText.text = LanguageUtils.getText(762248);
                m_UI_Item_EventTribeKingScore3.m_lbl_score_num_LanguageText.text = ClientUtils.FormatComma(m_rankData.allianceRank);
            }
        }

        //刷新宝箱
        private void RefreshBox()
        {
            RefreshBoxStatus();
            RefreshPro();
        }

        private void RefreshBoxStatus()
        {
            for (int i = 0; i < m_boxList.Count; i++)
            {
                ActivityIntegralTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityIntegralTypeDefine>(m_behaviorList[i].Id);

                //变更宝箱样式
                m_boxList[i].m_UI_Model_AnimationBox.m_UI_Model_AnimationBox_SkeletonGraphic.initialSkinName = define.box;

                if (m_rankData.rewards != null && m_rankData.rewards.ContainsKey(define.ID))
                {
                    m_boxList[i].m_UI_Model_AnimationBox.SetBox(true);
                }
                else
                {
                    m_boxList[i].m_UI_Model_AnimationBox.SetBox(false, false);
                }
                m_boxList[i].m_lbl_target_LanguageText.text = ClientUtils.FormatComma(define.standard);
                m_boxList[i].gameObject.SetActive(true);
            }
        }

        private void RefreshPro()
        {
            if (m_widthList == null)
            {
                float width = m_pb_rogressBar_GameSlider.GetComponent<RectTransform>().rect.width;
                m_widthList = new List<float>();
                for (int i = 0; i < m_boxList.Count; i++)
                {
                    m_widthList.Add(Mathf.Abs(m_boxList[i].m_root_RectTransform.anchoredPosition.x) / width);
                }
            }

            int count = m_behaviorList.Count;
            if (count > 0)
            {
                float pro = (float)m_score / m_behaviorList[m_behaviorList.Count - 1].Count;
                int stage = -1;
                for (int i = 0; i < m_behaviorList.Count; i++)
                {
                    if (m_score > m_behaviorList[i].Count)
                    {
                        stage = i;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            pro = (float)m_score / m_behaviorList[i].Count * m_widthList[0];
                        }
                        else
                        {
                            int beforeScore = m_behaviorList[i - 1].Count;
                            int afterScore = m_behaviorList[i].Count;
                            int diffScore = (afterScore - beforeScore);
                            if (diffScore > 0)
                            {
                                float diffPro = m_widthList[i] - m_widthList[i - 1];
                                pro = ((float)(m_score - beforeScore) / diffScore) * diffPro;
                            }
                        }
                        break;
                    }
                }
                if (stage > -1)
                {
                    pro = m_widthList[stage] + pro;
                }
                m_pb_rogressBar_GameSlider.value = pro;
            }
        }

        private void ReadData()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);
            m_behaviorList = m_scheduleData.GetBehaviorList();

            //m_behaviorList = m_activityProxy.GetChildBehaviorList7(m_menuData.Define.ID, 0);
            //ClientUtils.Print(m_behaviorList);
            m_activityType = m_menuData.Define.activityType;
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, m_lbl_time_LanguageText);
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
                m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762019, ClientUtils.FormatCountDown((int)diffTime));
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

        private void ClickBox(int index)
        {
            ActivityIntegralTypeDefine define = CoreUtils.dataService.QueryRecord<ActivityIntegralTypeDefine>(m_behaviorList[index].Id);
            //显示tip
            m_UI_Tip_BoxReward.gameObject.SetActive(true);
            m_UI_Tip_BoxReward.SetInfo2(define.itemPackage,
                                        m_boxList[index].m_btn_box_GameButton.transform.position,
                                        m_boxList[index].m_btn_box_GameButton.GetComponent<RectTransform>().rect.width / 2);
        }

        #region 活动信息

        private void OnInfo()
        {
            if (!(m_isInit))
            {
                return;
            }
            m_pl_info_CanvasGroup.gameObject.SetActive(true);
            m_pl_mes.gameObject.SetActive(false);

            if (m_scheduleData != null)
            {
                ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)m_scheduleData.PreActivityId);
                m_lbl_info_LanguageText.text = LanguageUtils.getText(define.l_ruleID);
            }
        }


        private void OnBack()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(false);
            m_pl_mes.gameObject.SetActive(true);
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
            var subView = new UI_Item_EventTypeRank_SubView(rectTrans);
            subView.AddBackListener(OnBack2);

            m_rankSubView = subView;
            m_rankNode = obj;

            ShowRank();
        }

        private void ShowRank()
        {
            m_pl_mes.gameObject.SetActive(false);
            m_rankNode.SetActive(true);
            var subView = m_rankSubView as UI_Item_EventTypeRank_SubView;

            int sourceType = 0;
            int type = 0;
            if (m_activityType == 303) //战斗号角 个人排名
            {
                sourceType = (int)EventRankSourceType.Warhorn;
                type = 2;
            }
            else if (m_activityType == 304)//战斗号角 联盟排名
            {
                sourceType = (int)EventRankSourceType.Warhorn;
                type = 1;
            }
            else if (m_activityType == 305 || m_activityType == 306) //部落之王 
            {
                sourceType = (int)EventRankSourceType.TribalKing;
            }
            subView.Refresh(m_scheduleData.PreActivityId, sourceType, type);
        }

        private void OnBack2()
        {
            m_pl_mes.gameObject.SetActive(true);
            m_rankNode.gameObject.SetActive(false);
        }

        #endregion        
    }
}