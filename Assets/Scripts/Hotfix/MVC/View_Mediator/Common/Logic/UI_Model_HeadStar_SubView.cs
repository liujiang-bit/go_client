// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月29日
// Update Time         :    2019年12月29日
// Class Description   :    UI_Model_HeadStar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_HeadStar_SubView : UI_SubView
    {
        public void SetShow(bool bShow)
        {
            m_img_star_PolygonImage.enabled = bShow;
        }

        public void SetShow2(bool bShow)
        {
            m_img_star_PolygonImage.enabled = bShow;
            m_img_stardark_PolygonImage.gameObject.SetActive(true);
        }
    }
}