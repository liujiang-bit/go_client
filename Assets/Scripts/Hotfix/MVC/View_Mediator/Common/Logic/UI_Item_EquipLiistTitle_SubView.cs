// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Item_EquipLiistTitle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_EquipLiistTitle_SubView : UI_SubView
    {
        public void SetText(string txt)
        {
            m_lbl_title_LanguageText.text = txt;
        }
    }
}