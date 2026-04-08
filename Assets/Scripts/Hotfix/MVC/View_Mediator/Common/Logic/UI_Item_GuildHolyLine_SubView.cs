// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月13日
// Update Time         :    2020年5月13日
// Class Description   :    UI_Item_GuildHolyLine_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    
   
    
    public partial class UI_Item_GuildHolyLine_SubView : UI_SubView
    {

        public void Refresh(int id)
        {
            m_lbl_name_LanguageText.text = LanguageUtils.getText(id);
        }

    }
}