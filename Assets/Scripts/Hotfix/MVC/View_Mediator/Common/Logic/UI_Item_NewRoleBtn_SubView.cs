// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, 30 September 2020
// Update Time         :    Wednesday, 30 September 2020
// Class Description   :    UI_Item_NewRoleBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using Data;
using System;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_NewRoleBtn_SubView : UI_Item_MainIFEventBtn_SubView
    {
        private ActivityProxy m_activityProxy;
        private bool m_isInit;

        private int m_activityId;

        private float m_lastUpdateTime;
        private bool m_isDelayTime;

        private Timer m_stimer;
        private long m_activityEndTime;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.SystemDayChange,
                    CmdConstant.UpdateActivityReddot,
                    CmdConstant.ActivityActivePointChange,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            if (!m_isInit)
            {
                return;
            }
            switch (notification.Name)
            {
                case CmdConstant.SystemDayChange:
                    RefreshActivityShowStatus();
                    break;
                case CmdConstant.UpdateActivityReddot:
                    Int64 activityId = (Int64)notification.Body;
                    if (activityId != m_activityId)
                    {
                        return;
                    }
                    DelayRefreshReddot();
                    break;
                case CmdConstant.ActivityScheduleUpdate:
                    List<Int64> list = notification.Body as List<Int64>;
                    if (list.Contains(m_activityId))
                    {
                        DelayRefreshReddot();
                    }
                    break;
                case CmdConstant.ActivityActivePointChange:
                    DelayRefreshReddot();
                    break;
                default:
                    break;
            }
        }

        public void Init()
        {
            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;

                m_btn_eventIcon_GameButton.onClick.AddListener(ClickEvent);

                m_activityId = ActivityProxy.CreateRoleActivityId;

                if (m_activityProxy.IsShowAcitivity(m_activityId, null))
                {
                    var timeInfo = m_activityProxy.GetActivityById(m_activityId);
                    if (timeInfo != null)
                    {
                        long serverTime = ServerTimeModule.Instance.GetServerTime();
                        if (timeInfo.endTime > serverTime)
                        {
                            m_activityEndTime = timeInfo.endTime;
                            StartTimer();
                        }
                    }
                }

                m_isInit = true;
            }

            RefreshActivityShowStatus();
            if (gameObject.activeSelf)
            {
                RefreshReddot();
            }
        }

        //开启定时器
        private void StartTimer()
        {
            if (m_stimer == null)
            {
                m_stimer = Timer.Register(1.0f, UpdateTime, null, true, true, m_lbl_redpoint_LanguageText);
            }
        }

        private void UpdateTime()
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 diffTime = m_activityEndTime - serverTime;
            if (diffTime < 0)
            {
                CancelTimer();
                gameObject.SetActive(false);
            }
        }

        //取消定时器
        private void CancelTimer()
        {
            if (m_stimer != null)
            {
                m_stimer.Cancel();
                m_stimer = null;
            }
        }

        public void RefreshActivityShowStatus()
        {
            if (m_activityProxy.IsShowAcitivity(m_activityId, null))
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void RefreshReddot()
        {
            ActivityScheduleData scheduleData = m_activityProxy.GetActivitySchedule(m_activityId);
            if (scheduleData != null)
            {
                int count = scheduleData.GetReddotNum();
                if (count > 0)
                {
                    m_img_redpoint.gameObject.SetActive(true);
                    m_lbl_redpoint_LanguageText.text = count.ToString();
                    return;
                }
            }
            m_img_redpoint.gameObject.SetActive(false);
        }

        private void DelayRefreshReddot()
        {
            if (IsDelayUpdate())
            {
                return; 
            }
            m_isDelayTime = false;
            m_lastUpdateTime = Time.realtimeSinceStartup;
            RefreshReddot();
        }

        private void ClickEvent()
        {
            CoreUtils.uiManager.ShowUI(UI.s_newRoleActivity);
        }

        private bool IsDelayUpdate()
        {
            if (Time.realtimeSinceStartup - m_lastUpdateTime < 0.5f)
            {
                if (m_isDelayTime)
                {
                    return true;
                }
                m_isDelayTime = true;
                Timer.Register(0.6f, DelayRefreshReddot);
                return true;
            }
            return false;
        }
    }
}