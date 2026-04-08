// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月10日
// Update Time         :    2020年2月10日
// Class Description   :    UI_Model_ResourcesConsumeInBCB_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_ResourcesConsumeInBCB_SubView : UI_Model_ResourcesConsume_SubView
    {
        public void SetResourcesConsume(string icon, int value, bool isEnough = true)
        {
            if (m_img_icon_PolygonImage.assetName != icon)
            {
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
            }
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
            m_lbl_languageText_LanguageText.text = value.ToString("N0");
            m_img_icon_PolygonImage.raycastTarget = false;
            m_lbl_languageText_LanguageText.raycastTarget = false;
        }
    }
}