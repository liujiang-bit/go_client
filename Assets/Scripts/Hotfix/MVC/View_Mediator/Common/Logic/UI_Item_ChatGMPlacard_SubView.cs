// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月12日
// Update Time         :    2020年6月12日
// Class Description   :    UI_Item_ChatGMPlacard_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_ChatGMPlacard_SubView : UI_SubView
    {
        public void SetInfo(ChatMsg msg)
        {
            m_lbl_content_LanguageText.text = msg.msg;
            m_lbl_content_ContentSizeFitter.SetLayoutVertical();
        }

        public float GetHeight()
        {
            return m_lbl_content_LanguageText.preferredHeight + m_lbl_title_LanguageText.preferredHeight + 40;
        }
    }
}