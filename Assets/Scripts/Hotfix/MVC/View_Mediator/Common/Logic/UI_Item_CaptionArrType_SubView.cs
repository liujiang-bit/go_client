// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Item_CaptionArrType_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_CaptionArrType_SubView : UI_SubView
    {
        public void AddClickEvent(UnityAction call)
        {
            m_btn_btn_GameButton.onClick.AddListener(call);
        }
        public void SetSelected(bool bSelected)
        {
            m_img_img_PolygonImage.gameObject.SetActive(bSelected);
        }
    }
}