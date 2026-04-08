// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月17日
// Update Time         :    2020年2月17日
// Class Description   :    UI_Model_StandardButton_MiniGreen_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_StandardButton_MiniGreen_SubView : UI_SubView
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
    }
}