// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Model_CaptainStar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_CaptainStar_SubView : UI_SubView
    {
        public void setHight(bool bHight)
        {
            m_img_starHighlight_PolygonImage.gameObject.SetActive(bHight);
        }
    }
}