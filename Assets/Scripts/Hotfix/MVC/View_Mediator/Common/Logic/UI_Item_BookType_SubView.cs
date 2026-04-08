// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月15日
// Update Time         :    2020年9月15日
// Class Description   :    UI_Item_BookType_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_BookType_SubView : UI_SubView
    {
        private long m_markerId = 0;

        public void Init(long markerId)
        {
            m_markerId = markerId;
        }

        public void SetSelectState(bool state)
        {
            m_img_choose_PolygonImage.gameObject.SetActive(state);
        }

        public void setClickCallback(UnityAction<long> Callback)
        {
            m_btn_type_GameButton.onClick.RemoveAllListeners();
            m_btn_type_GameButton.onClick.AddListener(() =>
            {
                if (Callback != null)
                {
                    Callback(m_markerId);
                }                
            });
        }
    }
}