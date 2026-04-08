// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月27日
// Update Time         :    2020年8月27日
// Class Description   :    UI_Item_EventNarmer_SubView 活动 洛哈的试炼
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

namespace Game {
    public partial class UI_Item_EventNarmer_SubView : UI_SubView
    {
        private ActivityProxy m_activityProxy;
        private RewardGroupProxy m_rewardGroupProxy;

        private ActivityItemData m_menuData;
        private bool m_isInit;

        private ActivityScheduleData m_scheduleData;

        private List<UI_Model_RewardGet_SubView> m_itemViewList;
        private Timer m_timer;
        private Int64 m_endTime;

        public void Init(ActivityItemData menuData)
        {
            m_menuData = menuData;
            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

                m_btn_go.m_btn_languageButton_GameButton.onClick.AddListener(OnGo);
                m_btn_info_GameButton.onClick.AddListener(OnInfo);
                m_btn_back_GameButton.onClick.AddListener(OnBack);

                m_isInit = true;
            }
            ReadData();
            Refresh();
        }

        private void ReadData()
        {
            m_scheduleData = m_activityProxy.GetActivitySchedule(m_menuData.Define.ID);

        }

        private void Refresh()
        {
            gameObject.SetActive(true);

            m_lbl_title_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_nameID);

            ActivityTimeInfo activityInfo = m_menuData.Data;
            DateTime startTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.startTime);
            DateTime endTime = ServerTimeModule.Instance.ConverToServerDateTime(activityInfo.endTime);
            m_lbl_coming_LanguageText.text = LanguageUtils.getTextFormat(762020, startTime.ToString("yyyy/M/d"), endTime.ToString("yyyy/M/d"));

            ClientUtils.LoadSprite(m_img_activitybg_PolygonImage, m_menuData.Define.background);

            m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_menuData.Define.l_taglineID);

            RefreshRewardGroup(m_menuData.Define.itemPackage);

            //倒计时
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
            }
        }

        //刷新奖励组物品
        private void RefreshRewardGroup(int itemPackage)
        {
            if (m_itemViewList == null)
            {
                m_itemViewList = new List<UI_Model_RewardGet_SubView>();
                m_itemViewList.Add(m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_Item3);
                m_itemViewList.Add(m_UI_Model_Item4);
                m_itemViewList.Add(m_UI_Model_Item5);
                for (int i = 0; i < m_itemViewList.Count; i++)
                {
                    m_itemViewList[i].SetScale(m_pl_rewards_GridLayoutGroup.transform.localScale.x);
                }
            }
            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            for (int i = 0; i < m_itemViewList.Count; i++)
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
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, true, m_pl_rewards_GridLayoutGroup);
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
                m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762019, ClientUtils.FormatCountDown((int)diffTime));
            }
        }

        //前往
        private void OnGo()
        {
            int id = (m_menuData.Define.ID * 1000) + 1;
            ActivityDropTypeDefine define = m_activityProxy.GetDropTypeDefine(id);
            if (define == null)
            {
                return;
            }
            m_activityProxy.GoJump(define.jumpType);
        }

        private void OnInfo()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(true);
            m_pl_mes.gameObject.SetActive(false);

            ActivityCalendarDefine define = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>((int)m_scheduleData.ActivityId);
            m_lbl_info_LanguageText.text = LanguageUtils.getText(define.l_ruleID);
        }

        private void OnBack()
        {
            m_pl_info_CanvasGroup.gameObject.SetActive(false);
            m_pl_mes.gameObject.SetActive(true);
        }
    }
}