// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月22日
// Update Time         :    2020年4月22日
// Class Description   :    UI_Model_EventTypeStartServerCk2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_EventTypeStartServerCk2_SubView : UI_SubView
    {
        public void SetRedpotStatus(bool isShow)
        {
            m_img_red_PolygonImage.gameObject.SetActive(isShow);
        }
    }
}