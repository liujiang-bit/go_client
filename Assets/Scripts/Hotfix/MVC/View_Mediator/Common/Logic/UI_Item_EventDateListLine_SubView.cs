// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月16日
// Update Time         :    2020年4月16日
// Class Description   :    UI_Item_EventDateListLine_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_EventDateListLine_SubView : UI_SubView
    {
        public void RefreshItem(ActivityItemData data)
        {
            m_lbl_name_LanguageText.text = data.Title;
        }
    }
}