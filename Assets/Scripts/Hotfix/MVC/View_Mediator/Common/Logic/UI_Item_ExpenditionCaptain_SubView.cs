// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月11日
// Update Time         :    2020年6月11日
// Class Description   :    UI_Item_ExpenditionCaptain_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ExpenditionCaptain_SubView : UI_SubView
    {
        public void Show(int mainHeroId, int level, UnityAction clickAction)
        {
            m_UI_Model_CaptainHead.SetHero(mainHeroId, level);
            m_btn_btn_GameButton.onClick.RemoveAllListeners();
            m_btn_btn_GameButton.onClick.AddListener(clickAction);
        }
    }
}