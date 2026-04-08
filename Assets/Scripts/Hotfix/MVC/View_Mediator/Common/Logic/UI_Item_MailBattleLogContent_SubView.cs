// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月2日
// Update Time         :    2020年7月2日
// Class Description   :    UI_Item_MailBattleLogContent_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_MailBattleLogContent_SubView : UI_SubView
    {
        public void Refresh(string desc)
        {
            m_lbl_desc_LanguageText.text = desc;
            m_root_RectTransform.sizeDelta = new Vector2(m_root_RectTransform.sizeDelta.x, m_lbl_desc_LanguageText.preferredHeight);
        }
    }
}