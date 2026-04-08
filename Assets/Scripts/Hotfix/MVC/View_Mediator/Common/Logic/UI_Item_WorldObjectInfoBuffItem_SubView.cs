// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    UI_Item_WorldObjectInfoBuffItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_WorldObjectInfoBuffItem_SubView : UI_SubView
    {
        public void RefreshBuff(Data.CityBuffDefine cityBuff)
        {
            if (cityBuff == null) return;
            m_lbl_buffDesc_LanguageText.text = LanguageUtils.getTextFormat(cityBuff.tag, cityBuff.tagData[0]);
            ClientUtils.LoadSprite(m_img_buff_PolygonImage, cityBuff.tagIcon);            
        }
    }
}