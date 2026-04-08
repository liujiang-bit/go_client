// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月27日
// Update Time         :    2020年4月27日
// Class Description   :    UI_LC_MysteryStore_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_LC_MysteryStore_SubView : UI_SubView
    {
        public void OnRefreshItem(List<MysteryStoreItemData> datas)
        {
            int count = datas.Count;
            int groupType = 1;
            if (count > 0 && datas[0] != null)
            {
                groupType = datas[0].groupType;
            }
            m_img_shoping1.OnRefreshItem(count >= 1 ? datas[0]: null);
            m_img_shoping2.OnRefreshItem(count >= 2 ? datas[1]: null);
            m_img_shoping3.OnRefreshItem(count >= 3 ? datas[2]: null);
            m_img_shoping4.OnRefreshItem(count >= 4 ? datas[3]: null);
            m_lbl_lineText_LanguageText.text = LanguageUtils.getText(787004 + groupType);
            
        }
        
    }
}