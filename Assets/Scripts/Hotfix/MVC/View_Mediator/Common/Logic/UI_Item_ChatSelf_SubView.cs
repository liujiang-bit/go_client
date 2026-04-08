// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月9日
// Update Time         :    2020年7月9日
// Class Description   :    UI_Item_ChatSelf_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_ChatSelf_SubView : UI_SubView
    {

        public void SetName( string guildName, string name)
        {
            if (!string.IsNullOrEmpty(guildName))
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030,guildName, name);
            }
            else
            {
                m_lbl_name_LanguageText.text = name; 
            }
        }
    }
}