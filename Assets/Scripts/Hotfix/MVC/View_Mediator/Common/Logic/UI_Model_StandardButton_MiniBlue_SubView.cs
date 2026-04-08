// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 9, 2020
// Update Time         :    Thursday, January 9, 2020
// Class Description   :    UI_Model_StandardButton_MiniBlue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_StandardButton_MiniBlue_SubView
    {
        public void Enable(bool enabled)
        {
            m_btn_languageButton_GameButton.interactable = enabled;
        }
        public void AddClickEvent(UnityAction call)
        {
            m_btn_languageButton_GameButton.onClick.AddListener(call);
        }
        public void RemoveAllClickEvent()
        {
            m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
        }

        public void SetGray(bool isGray)
        {
            m_img_forbid_PolygonImage.gameObject.SetActive(isGray);
        }

        public void SetText(string text)
        {
            m_lbl_Text_LanguageText.text = text;
        }
    }
}