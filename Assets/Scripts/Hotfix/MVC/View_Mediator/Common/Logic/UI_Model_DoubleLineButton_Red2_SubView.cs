// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Model_DoubleLineButton_Red2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_DoubleLineButton_Red2_SubView : UI_SubView
    {

        public void AddBtnCancelClick(UnityAction callback)
        {
            if (callback == null)
            {
                return;
            }

            m_btn_languageButton_GameButton.onClick.AddListener(callback);
        }
    }
}