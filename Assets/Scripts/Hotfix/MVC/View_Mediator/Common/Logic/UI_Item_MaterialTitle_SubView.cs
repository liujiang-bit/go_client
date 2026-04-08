// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月18日
// Update Time         :    2020年5月18日
// Class Description   :    UI_Item_MaterialTitle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_MaterialTitle_SubView : UI_SubView
    {
        public void SetTitle(string title)
        {
            m_lbl_typeTitle_LanguageText.text = title;
        }
    }
}