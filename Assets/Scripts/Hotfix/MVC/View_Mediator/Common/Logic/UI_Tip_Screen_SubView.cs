// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月16日
// Update Time         :    2020年4月16日
// Class Description   :    UI_Tip_Screen_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Tip_Screen_SubView : UI_SubView
    {
        public void AddClickEvent(UnityAction call)
        {
            m_img_icon_GameButton.onClick.AddListener(call);
        }
    }
}