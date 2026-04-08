// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    UI_Model_PageButton_Side_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_PageButton_Side_SubView : UI_SubView
    {
        public void HideLight(bool isHideLight)
        {
            m_img_highLight_PolygonImage.gameObject.SetActive(!isHideLight);
            m_img_dark_PolygonImage.gameObject.SetActive(isHideLight);
        }
    }
}