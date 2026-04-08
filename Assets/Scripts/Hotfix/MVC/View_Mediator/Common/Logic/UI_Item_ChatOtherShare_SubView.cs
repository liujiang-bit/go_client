// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月21日
// Update Time         :    2020年9月21日
// Class Description   :    UI_Item_ChatOtherShare_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ChatOtherShare_SubView : UI_SubView
    {
        public void AddEvent(UnityAction unityAction)
        {
            m_btn_head_GameButton.onClick.RemoveAllListeners();
            m_btn_head_GameButton.onClick.AddListener(unityAction);
        }
    }
}