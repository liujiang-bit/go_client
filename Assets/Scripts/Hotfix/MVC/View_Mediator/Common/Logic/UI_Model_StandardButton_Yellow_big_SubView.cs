// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月22日
// Update Time         :    2020年1月22日
// Class Description   :    UI_Model_StandardButton_Yellow_big_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_StandardButton_Yellow_big_SubView : UI_Model_StandardButton_Yellow_SubView
    {
        public void Enable(bool enabled)
        {
            m_btn_languageButton_GameButton.interactable = enabled;
        }
        public void RemoveAllClickEvent()
        {
            m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
        }

        public void SetGray(bool isGray)
        {
            m_img_forbid_PolygonImage.gameObject.SetActive(isGray);
            m_btn_languageButton_GameButton.interactable = !isGray;
        }
        public void SetText(string text)
        {
            m_lbl_text_LanguageText.text = text;
        }
    }
}