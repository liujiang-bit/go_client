// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Sunday, 27 September 2020
// Update Time         :    Sunday, 27 September 2020
// Class Description   :    UI_Pop_BookIcon_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Pop_BookIcon_SubView : UI_SubView
    {
        public void SetIcon(string iconImg)
        {
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, iconImg);
        }
    }
}