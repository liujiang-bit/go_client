// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月15日
// Update Time         :    2020年6月15日
// Class Description   :    UI_Item_MailType10_SubView 邮件 联盟科技
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_MailType10_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;

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

            m_lbl_mes_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), mailInfo.emailContents);

            if (mailInfo.guildEmail != null && mailInfo.guildEmail.technologyId > 0)
            {
                AllianceStudyDefine define = CoreUtils.dataService.QueryRecord<AllianceStudyDefine>((int)mailInfo.guildEmail.technologyId);
                if (define != null)
                {
                    ClientUtils.LoadSprite(m_img_icon_PolygonImage, define.icon);
                    m_btn_go.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    m_btn_go.m_btn_languageButton_GameButton.onClick.AddListener(() => {
                        var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                        if (allianceProxy.HasJionAlliance())
                        {
                            GOScrptGuide param = new GOScrptGuide(EnumTaskType.None, (int)define.studyType, 0, "");
                            CoreUtils.uiManager.ShowUI(UI.s_AllianceResearchMain, null, param);
                        }
                        else
                        {
                            Tip.CreateTip(570091).Show();
                        }
                    });
                }
            }            
        }
    }
}