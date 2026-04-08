// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月16日
// Update Time         :    2020年4月16日
// Class Description   :    UI_Item_EventDateDateItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_EventDateDateItem_SubView : UI_SubView
    {
        private bool m_isSaveInitColor = true;
        private Color m_selectedColor;
        private Color m_noSelectedColor;

        public void SetHighLight(bool isLight)
        {
            if (m_isSaveInitColor)
            {
                m_isSaveInitColor = false;
                m_selectedColor = ClientUtils.StringToHtmlColor("#ffffff");
                m_noSelectedColor = ClientUtils.StringToHtmlColor("#ffffff");
                m_noSelectedColor.a = 0.5f;
            }
            m_img_selectBg_PolygonImage.gameObject.SetActive(isLight);
            m_img_bg_PolygonImage.gameObject.SetActive(!isLight);
            if (isLight)
            {
                m_lbl_week_LanguageText.color = m_selectedColor;
                m_lbl_day_LanguageText.color = m_selectedColor;
                m_lbl_week_LanguageText.fontSize = 20;
                m_lbl_day_LanguageText.fontSize = 20;
            }
            else
            {
                m_lbl_week_LanguageText.color = m_noSelectedColor;
                m_lbl_day_LanguageText.color = m_noSelectedColor;
                m_lbl_week_LanguageText.fontSize = 18;
                m_lbl_day_LanguageText.fontSize = 18;
            }
        }
    }
}