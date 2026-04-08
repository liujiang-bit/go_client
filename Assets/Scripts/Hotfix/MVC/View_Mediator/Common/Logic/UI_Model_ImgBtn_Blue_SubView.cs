// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月12日
// Update Time         :    2020年8月12日
// Class Description   :    UI_Model_ImgBtn_Blue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_ImgBtn_Blue_SubView : UI_SubView
    {
        public void AddClickEvent(UnityAction action)
        {
            m_btn_languageButton_GameButton.onClick.AddListener(action);
        }

        public void SetEnabled(bool bEnable)
        {
            m_btn_languageButton_GameButton.enabled = bEnable;

            var markGray = m_btn_languageButton_PolygonImage.GetComponent<GrayChildrens>();
            if (markGray == null)
            {
                markGray = m_btn_languageButton_PolygonImage.gameObject.AddComponent<GrayChildrens>();
            }
            if (bEnable)
            {
                markGray.Normal();
            }
            else
            {
                markGray.Gray();
            }
        }
    }
}