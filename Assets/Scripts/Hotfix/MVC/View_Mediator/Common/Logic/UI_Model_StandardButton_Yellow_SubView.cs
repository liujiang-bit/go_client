// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Model_StandardButton_Yellow_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_StandardButton_Yellow_SubView : UI_SubView
    {
        public void AddClickEvent(UnityAction action)
        {
            m_btn_languageButton_GameButton.onClick.AddListener(action);
        }
    }
}