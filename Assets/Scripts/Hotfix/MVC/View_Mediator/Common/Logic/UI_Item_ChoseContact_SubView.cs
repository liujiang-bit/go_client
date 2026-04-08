// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月14日
// Update Time         :    2020年9月14日
// Class Description   :    UI_Item_ChoseContact_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Item_ChoseContact_SubView : UI_SubView
    {
        public void SetNane(string name)
        {
            m_lbl_name_LanguageText.text = name;
        }
    }
}