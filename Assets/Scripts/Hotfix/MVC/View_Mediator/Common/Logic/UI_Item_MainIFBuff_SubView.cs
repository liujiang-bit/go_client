// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Item_MainIFBuff_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_MainIFBuff_SubView : UI_SubView
    {

        public int CityBuffId { get; private set; }
        public void SetCityBuffId(int buffId)
        {
            CityBuffId = buffId;
        }

        public void Setquality(int quality)
        {
            ClientUtils.LoadSprite(m_img_bg_PolygonImage, GetQualityImg(quality));
        }
        public void SetIcon(string icon)
        {
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
        }

        public void AddBtnListener(UnityAction callback)
        {
            m_btn_noTextButton_GameButton.onClick.AddListener(callback);
        }


        public string GetQualityImg(int quality)
        {
            quality = quality - 1;
            return RS.ItemQualityBg[quality];
        }
    }
}