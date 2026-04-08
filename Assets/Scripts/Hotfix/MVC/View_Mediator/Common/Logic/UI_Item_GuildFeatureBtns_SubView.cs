// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Item_GuildFeatureBtns_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_GuildFeatureBtns_SubView : UI_SubView
    {
        public void SetRedDot(bool bValue)
        {
            this.m_UI_Common_Redpoint.gameObject.SetActive(bValue);
        }

        public void SetRedCount(long count)
        {
            this.m_UI_Common_Redpoint.gameObject.SetActive(count>0);

            if (count>0)
            {
                this.m_UI_Common_Redpoint.m_lbl_num_LanguageText.text = count.ToString();
            }
        }
    }
}