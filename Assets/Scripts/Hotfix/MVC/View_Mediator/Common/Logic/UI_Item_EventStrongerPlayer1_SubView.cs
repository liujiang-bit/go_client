// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_EventStrongerPlayer1_SubView 活动 最强执政官预告 
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using System.Collections.Generic;
using SprotoType;

namespace Game {
    public partial class UI_Item_EventStrongerPlayer1_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private RewardGroupProxy m_rewardGroupProxy;

        private bool m_isiInit;
        private ActivityItemData m_menuData;

        private Timer m_timer;
        private Int64 m_endTime;

        private List<UI_Model_RewardGet_SubView> m_itemViewList;

        public void Init(ActivityItemData menuData)
        {
            m_menuData = menuData;
            if (!m_isiInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

                m_itemViewList = new List<UI_Model_RewardGet_SubView>();
                m_itemViewList.Add(m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_Item3);
                m_itemViewList.Add(m_UI_Model_Item4);
                for (int i = 0; i < m_itemViewList.Count; i++)
                {
                    m_itemViewList[i].SetScale(m_pl_rewards_ArabLayoutCompment.transform.localScale.x);
                }

                m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(OnHistoryRank);

                m_isiInit = true;
            }

            m_lbl_title_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_nameID);
            m_lbl_coming_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_taglineID);
            m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_ruleID);
            ClientUtils.LoadSprite(m_img_big_bg_PolygonImage, m_menuData.Define.background);

            //奖励组
            RefreshRewardGroup(m_menuData.Define.itemPackage);

            //倒计时
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

            gameObject.SetActive(true);
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true);
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
            if (gameObject == null)
            {
                CancelTimer();
                return;
            }
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
                m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762195, ClientUtils.FormatCountDown((int)diffTime));
            }
        }

        //刷新奖励组物品
        private void RefreshRewardGroup(int itemPackage)
        {
            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            for (int i = 0; i < 4; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].RefreshByGroup(groupDataList[i], 3);
                }
                else
                {
                    m_itemViewList[i].gameObject.SetActive(false);
                }
            }
        }

        //历史排名
        private void OnHistoryRank()
        {
            CoreUtils.uiManager.ShowUI(UI.s_strongerPlayerRank);
        }
    }
}