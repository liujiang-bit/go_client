// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月2日
// Update Time         :    2020年1月2日
// Class Description   :    UI_Model_TrainUpHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_TrainUpHead_SubView : UI_SubView
    {
        public void SetHead(string image)
        {
            ClientUtils.LoadSprite(m_img_army_icon_PolygonImage, image);
        }

        public void SetName(string name)
        {
            m_lbl_name_LanguageText.text = name;
        }
    }
}