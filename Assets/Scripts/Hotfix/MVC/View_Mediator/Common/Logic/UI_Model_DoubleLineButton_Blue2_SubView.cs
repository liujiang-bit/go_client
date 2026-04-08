// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Model_DoubleLineButton_Blue2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_DoubleLineButton_Blue2_SubView : UI_SubView
    {
        public void SetGrayAndDisable(bool bValue)
        {
            m_img_forbid_PolygonImage.gameObject.SetActive(bValue);
            m_btn_languageButton_GameButton.interactable = !bValue;
        }
        public void AddEvent(UnityAction unityAction)
        {
            m_btn_languageButton_GameButton.onClick.AddListener(unityAction);
        }
        public void ClearEvent()
        {
            m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
        }
    }
}