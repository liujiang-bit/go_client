// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月24日
// Update Time         :    2020年9月24日
// Class Description   :    UI_Item_AliGuide_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using System;
using SprotoType;

namespace Game {
    public partial class UI_Item_AliGuide_SubView : UI_SubView
    {
        private bool m_isRegisterEvent;
        private int m_sourceType; //1主界面按钮
        private Action m_callback;
        private bool m_isRequesting;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.AllianceBuildCanCreateFlag,
                    Guild_GetGuildInfo.TagName,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AllianceBuildCanCreateFlag:
                    if (m_sourceType == 1)
                    {
                        bool isShow = (bool)notification.Body;
                        gameObject.SetActive(isShow);
                    }
                    break;
                case Guild_GetGuildInfo.TagName:
                    if (!m_isRequesting)
                    {
                        return;
                    }
                    if (m_sourceType != 1)
                    {
                        return;
                    }
                    Guild_GetGuildInfo.response result = notification.Body as Guild_GetGuildInfo.response;
                    if (result != null && result.reqType == 2)
                    {
                        m_isRequesting = false;
                        CoreUtils.uiManager.ShowUI(UI.s_AllianceTerrtroy, null, 1);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Init(int sourceType)
        {
            m_sourceType = sourceType;
            if (sourceType == 1)
            {
                RegisterBtnEvent(RequestGuildInfo);
            }
        }

        public void OnClick()
        {
            if (m_callback != null)
            {
                m_callback();
            }
        }

        public void RegisterBtnEvent(Action callback)
        {
            m_callback = callback;
            if (!m_isRegisterEvent)
            {
                m_isRegisterEvent = true;
                m_btn_aliguide_GameButton.onClick.AddListener(OnClick);
            }
        }

        public void RequestGuildInfo()
        {
            if (m_isRequesting)
            {
                return;
            }
            m_isRequesting = true;
            var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            allianceProxy.SendAllianceInfo(2);
        }

    }
}