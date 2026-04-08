// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月29日
// Update Time         :    2019年12月29日
// Class Description   :    UI_Model_CaptainTalent_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_CaptainTalent_SubView : UI_SubView
    {
        private Data.HeroTalentDefine m_config;
        protected override void BindEvent()
        {
            m_btn_btn_GameButton.onClick.AddListener(OnClick);
        }
        private void OnClick()
        {
            if (m_config != null)
            {              
                HelpTip.CreateTip(LanguageUtils.getText(m_config.l_tipsID), m_btn_btn_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
            }
        }
        public void SetTalentId(int id)
        {
            m_config = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>(id);
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, m_config.icon1);
            m_btn_text_LanguageText.GetComponent<Text>().text = LanguageUtils.getText(m_config.l_talentID);
        }
    }
}