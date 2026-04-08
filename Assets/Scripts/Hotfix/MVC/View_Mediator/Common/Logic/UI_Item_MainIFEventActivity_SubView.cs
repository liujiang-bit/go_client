// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月7日
// Update Time         :    2020年5月7日
// Class Description   :    UI_Item_MainIFEventActivity_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;

namespace Game {
    public partial class UI_Item_MainIFEventActivity_SubView : UI_Item_MainIFEventBtn_SubView
    {
        private bool m_isInit;
        private ActivityProxy m_activityProxy;
        private float m_lastUpdateTime = 0;
        private bool m_isDelayTime;
        private bool m_initProxy;

        //subView事件监听
        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.UpdateActivityReddot,
                    CmdConstant.ActivityScheduleUpdate,
                    CmdConstant.UpdateActivityTotalReddot,
                    CmdConstant.RefreshActivityNewFlag,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateActivityTotalReddot:
                case CmdConstant.UpdateActivityReddot:
                case CmdConstant.ActivityScheduleUpdate:
                case CmdConstant.RefreshActivityNewFlag:
                    InitProxy();
                    UpdateReddot();
                    break;
                default:
                    break;
            }
        }

        private void InitProxy()
        {
            if (!m_initProxy)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_initProxy = true;
            }
        }

        public void Refresh()
        {
            if (!m_isInit)
            {
                InitProxy();
                AddBtnListener();
                m_isInit = true;
            }
            UpdateReddot();
        }

        public void UpdateReddot()
        {
            if (Time.realtimeSinceStartup - m_lastUpdateTime < 0.5f)
            {
                if (m_isDelayTime)
                {
                    return;
                }
                m_isDelayTime = true;
                Timer.Register(0.6f, UpdateReddot);
                return;
            }
            m_isDelayTime = false;
            if (gameObject == null)
            {
                return;
            }
            m_lastUpdateTime = Time.realtimeSinceStartup;
            int reddot = m_activityProxy.GetTotalReddot();
            if (reddot > 0)
            {
                m_img_redpoint.gameObject.SetActive(true);
                m_lbl_redpoint_LanguageText.gameObject.SetActive(true);
                m_lbl_redpoint_LanguageText.text = reddot.ToString();
            }
            else
            {
                m_img_redpoint.gameObject.SetActive(false);
            }
        }

        public void AddBtnListener()
        {
            m_btn_eventIcon_GameButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_eventDate);
        }
    }
}