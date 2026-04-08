// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, 09 October 2020
// Update Time         :    Friday, 09 October 2020
// Class Description   :    UI_Item_NewRoleActivityTipsItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_NewRoleActivityTipsItem_SubView : UI_SubView
    {
        public void Refresh(NewRoleActiveReward data)
        {
            m_UI_Model_RewardGet.SetScale(0.5f);
            m_UI_Model_RewardGet.RefreshByGroup(data.RewardGroupData, 3, false);
            m_lbl_day_LanguageText.text = LanguageUtils.getTextFormat(762259, data.Define.day);
            m_lbl_target_cur_LanguageText.text = ClientUtils.FormatComma(data.Define.standard);
        }
    }
}