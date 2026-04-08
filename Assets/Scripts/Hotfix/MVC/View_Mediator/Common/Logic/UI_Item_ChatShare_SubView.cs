// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月9日
// Update Time         :    2020年9月9日
// Class Description   :    UI_Item_ChatShare_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ChatShare_SubView : UI_SubView
    {
        Color defaultcolor;
        protected virtual void BindEvent() {
            defaultcolor = m_img_title_PolygonImage.color;
        }

        public void SetDec(string lbl_dec)
        {
            m_lbl_dec_LanguageText.text = lbl_dec;

        }
        public void SetTitle(string lbl_title)
        {
            m_lbl_title_LanguageText.text = lbl_title;

        }
        public void SetColor(string color )
        {
            if (string.IsNullOrEmpty(color))
            {
                m_img_title_PolygonImage.color = defaultcolor;
            }
            else
            {
                m_img_title_PolygonImage.color = ClientUtils.StringToHtmlColor(color); ;
            }
        }
        public void SetIcon(string icon)
        {
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, icon);
        }
        public void AddEvent(UnityAction unityAction)
        {
            m_btn_choose_GameButton.onClick.RemoveAllListeners();
            m_btn_choose_GameButton.onClick.AddListener(unityAction);
        }
    }
}