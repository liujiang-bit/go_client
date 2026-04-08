// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月12日
// Update Time         :    2020年6月12日
// Class Description   :    UI_Item_MailType5_SubView 邮件 联盟任命报告
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;
using SprotoType;
using PureMVC.Interfaces;

namespace Game {
    public partial class UI_Item_MailType5_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;

        protected override void BindEvent()
        {
            base.BindEvent();

            SubViewManager.Instance.AddListener(
                new string[] {

                }, this.gameObject, HandleNotification);
        }

        private void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {

                default:
                    break;
            }
        }

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_isInit = true;
            }

            m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), mailInfo.titleContents);
            m_UI_Item_MailTitle.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_subheadingID), mailInfo.subTitleContents);
            m_UI_Item_MailTitle.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(mailInfo.sendTime);
            if (!string.IsNullOrEmpty(mailDefine.poster))
            {
                ClientUtils.LoadSprite(m_UI_Item_MailTitle.m_img_bg_PolygonImage, mailDefine.poster);
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(false);
            }


            if (mailInfo.emailContents.Count == 6)
            {
                mailInfo.emailContents.Add(mailInfo.emailContents[5]);
            }
            m_lbl_mes_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), mailInfo.emailContents);

            m_lbl_name_LanguageText.text = mailInfo.guildEmail.roleName;
            m_UI_Model_PlayerHead.LoadPlayerIcon(mailInfo.guildEmail.roleHeadId, mailInfo.guildEmail.roleHeadFrameId);

            m_btn_check.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
            m_btn_check.m_btn_languageButton_GameButton.onClick.AddListener(()=> {
                CoreUtils.uiManager.ShowUI(UI.s_OtherPlayerInfo, null, mailInfo.guildEmail.roleRid);
            });
 
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_UI_Item_MailType5Item_VerticalLayoutGroup.GetComponent<RectTransform>());
        }
    }
}