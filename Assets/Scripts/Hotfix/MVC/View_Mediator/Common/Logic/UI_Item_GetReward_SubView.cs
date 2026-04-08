// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Item_GetReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_GetReward_SubView : UI_SubView
    {
        public void SetIcon(string icon)
        {
          //  ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
        }
        public void SetCount(long count)
        {
         //   m_lbl_count_LanguageText.text = count.ToString("N0");
        }
        public void SetName(string name)
        {
           // m_lbl_name_LanguageText.text = name;
        }
    }
}