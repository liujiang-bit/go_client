// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月26日
// Update Time         :    2020年8月26日
// Class Description   :    UI_Pop_MailGetNew_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using Data;

namespace Game {
    public partial class UI_Pop_MailGetNew_SubView : UI_SubView
    {
        private bool m_isInit;
        private Color m_defaultColor;
        private Timer m_timer;
        private EmailInfoEntity m_emailInfo;
        private EmailProxy m_emailProxy;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {
                    CmdConstant.AddEmailBubble,
                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AddEmailBubble:
                    m_emailInfo = notification.Body as EmailInfoEntity;
                    if (m_emailInfo == null)
                    {
                        return;
                    }
                    GlobalViewLevelMediator globalViewLevelMediator = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;
                    if (globalViewLevelMediator != null)
                    {
                        if (globalViewLevelMediator.GetViewLevel() <= MapViewLevel.TacticsToStrategy_1)
                        {
                            RefreshView();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void Init()
        {
            if (!m_isInit)
            {
                m_defaultColor = m_lbl_name_LanguageText.color;
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_btn_click_GameButton.onClick.AddListener(OnClick);
                m_isInit = true;
            }
        }
        private void RefreshView()
        {
            Init();
            CancelTimer();
            m_timer = Timer.Register(2, HideBubble);
            RefreshContent();
        }

        private void RefreshContent()
        {
            gameObject.SetActive(true);
            MailDefine mailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)m_emailInfo.emailId);
            if (mailDefine == null)
            {
                return;
            }
            if (mailDefine.messageName > 0)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(mailDefine.messageName);

                if (!string.IsNullOrEmpty(mailDefine.messageNameColor))
                {
                    ClientUtils.TextSetColor(m_lbl_name_LanguageText, mailDefine.messageNameColor);
                }
                else
                {
                    m_lbl_name_LanguageText.color = m_defaultColor;
                }
            }
            else
            {
                m_lbl_name_LanguageText.text = "";
            }

            if (string.IsNullOrEmpty(mailDefine.icon))
            {
                m_img_icon_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                m_img_icon_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, mailDefine.icon);
            }

            m_lbl_desc_LanguageText.text = m_emailProxy.FormatBubbleMessage(m_emailInfo, mailDefine);
        }

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private void HideBubble()
        {
            if (gameObject == null)
            {
                return;
            }
            gameObject.SetActive(false);
        }

        private void OnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_Email, null, m_emailInfo);
        }
    }
}