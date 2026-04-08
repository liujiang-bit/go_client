// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_WarTarget_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_WarTarget_SubView : UI_SubView
    {
        public void SetDistance(string distance)
        {
            m_lbl_dis_LanguageText.text = distance;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iconname"></param>
        public void SetBuildIcon(string iconname)
        {
            ClientUtils.LoadSprite(m_img_build_PolygonImage, iconname);
        }
    }
}