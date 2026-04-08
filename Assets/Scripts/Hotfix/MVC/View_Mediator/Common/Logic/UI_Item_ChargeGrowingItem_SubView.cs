// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_Item_ChargeGrowingItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ChargeGrowingItem_SubView : UI_SubView
    {
        protected override void BindEvent()
        {
            m_originCountColor = m_lbl_count_LanguageText.color;
        }

        public void RefreshUI(RoleInfoEntity playerRoleInfo, Data.RechargeFundDefine cfg, int cityLevel)
        {
            Color statusColor = Color.white;
            if(cityLevel < cfg.needLv || playerRoleInfo.growthFundReward.Contains(cfg.ID))
            {
                statusColor = Color.gray;
            }
            m_img_icon_PolygonImage.color = statusColor;
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, cfg.icon);
            m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(800088, cfg.needLv);
            m_lbl_desc_LanguageText.color = statusColor;
            m_img_cur_PolygonImage.color = statusColor;
            if(cityLevel < cfg.needLv)
            {
                m_btn_buy.m_btn_languageButton_GameButton.interactable = true;
                m_btn_buy.m_img_forbid_PolygonImage.gameObject.SetActive(true);
                m_btn_buy.m_lbl_Text_LanguageText.text = LanguageUtils.getText(800089);
            }
            else if(!playerRoleInfo.growthFund)
            {
                m_btn_buy.m_btn_languageButton_GameButton.interactable = true;
                m_btn_buy.m_img_forbid_PolygonImage.gameObject.SetActive(true);
                m_btn_buy.m_lbl_Text_LanguageText.text = LanguageUtils.getText(570008);
            }
            else if(playerRoleInfo.growthFundReward.Contains(cfg.ID))
            {
                m_btn_buy.m_btn_languageButton_GameButton.interactable = false;
                m_btn_buy.m_img_forbid_PolygonImage.gameObject.SetActive(true);
                m_btn_buy.m_lbl_Text_LanguageText.text = LanguageUtils.getText(570009);
            }
            else
            {
                m_btn_buy.m_btn_languageButton_GameButton.interactable = true;
                m_btn_buy.m_img_forbid_PolygonImage.gameObject.SetActive(false);
                m_btn_buy.m_lbl_Text_LanguageText.text = LanguageUtils.getText(570008);
            }
            m_lbl_count_LanguageText.text = ClientUtils.FormatComma(cfg.gem);
            m_lbl_count_LanguageText.color = m_originCountColor * statusColor;
            m_lbl_get_LanguageText.color = statusColor;
        }

        public void DisableClaim()
        {
            m_btn_buy.m_btn_languageButton_GameButton.interactable = false;
        }

        public void AddClaimClickListener(UnityAction listener)
        {
            m_btn_buy.RemoveAllClickEvent();
            m_btn_buy.AddClickEvent(listener);
        }

        private Color m_originCountColor;
    }
}