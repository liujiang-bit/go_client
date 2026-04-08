// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_LoginTypeButton_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_LoginTypeButton_SubView : UI_SubView
    {
        public void AddClickEvent(UnityAction action)
        {
            m_btn_btn_GameButton.onClick.AddListener(action);
        }
    }
}