// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_Item_TavernRewardProb_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_TavernRewardProb_SubView : UI_SubView
    {
        private BoxPreviewRewardData m_rewardData;

        public void AddBtnListener()
        {
            m_UI_Model_StandardButton_MiniBlue.m_btn_languageButton_GameButton.onClick.AddListener(ClickCallback);
        }

        public void ClickCallback()
        {
            if (m_rewardData.ShowType == (int)EnumBoxRewardShowType.Silver) //白银
            {
                CoreUtils.uiManager.ShowUI(UI.s_tavernBoxDesc, null, 2);
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_tavernBoxDesc, null, 1);
            }
        }

        public void RefreshItem(BoxPreviewRewardData rewardData)
        {
            m_rewardData = rewardData;
            if (rewardData.ShowType == (int)EnumBoxRewardShowType.Silver)//白银
            {
                m_lbl_mes_LanguageText.text = LanguageUtils.getText(760033);
                ClientUtils.LoadSprite(m_img_goldbox_PolygonImage, RS.TavernSilverBoxIcon);
            }
            else//黄金
            {
                m_lbl_mes_LanguageText.text = LanguageUtils.getText(760032);
                ClientUtils.LoadSprite(m_img_goldbox_PolygonImage, RS.TavernGoldBoxIcon);
            }
        }
    }
}