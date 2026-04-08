// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月13日
// Update Time         :    2020年1月13日
// Class Description   :    UI_Pop_TalkTip_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Pop_TalkTip_SubView : UI_SubView
    {
        public void SetLanguageId(int id)
        {
            m_lbl_text_LanguageText.text = LanguageUtils.getText(id);
        }

        public void UpdateBgSize(float maxWidth)
        {
            if (m_lbl_text_LanguageText.rectTransform.sizeDelta.x != maxWidth)
            {
                m_lbl_text_LanguageText.rectTransform.sizeDelta = new Vector2(maxWidth, 0);
            }

            float tipBagWidth = m_lbl_text_LanguageText.preferredWidth;
            if (tipBagWidth > maxWidth)
            {
                tipBagWidth = maxWidth;
            }
            m_img_bg_PolygonImage.rectTransform.sizeDelta = new Vector2(tipBagWidth + 40, m_lbl_text_LanguageText.preferredHeight + 20);
        }
    }
}