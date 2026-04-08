// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_Model_MonumentItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_MonumentItem_SubView : UI_SubView
    {
        private int language_unlock = 183005;
        private int language_update = 183006;

        private int descId;

        private GrayChildrens _makeChildrenGray;

        private GrayChildrens m_makeChildrenGray
        {
            get
            {
                if (_makeChildrenGray == null)
                {
                    _makeChildrenGray = gameObject.GetComponent<GrayChildrens>();
                }

                if (_makeChildrenGray == null)
                {
                    _makeChildrenGray = gameObject.AddComponent<GrayChildrens>();
                }
                
                return _makeChildrenGray;
            }
        }

        public void Refresh(int desc, int flag, string img)
        {
            float offset = m_img_icon_PolygonImage.GetComponent<RectTransform>().sizeDelta.y / 4;
            
            descId = desc;
            int flagText = language_unlock;
            switch (flag)
            {
                case 0:
                    flagText = language_unlock;
                    break;
                case 1:
                    flagText = language_update;
                    break;
            }
            
            m_lbl_name_LanguageText.text = LanguageUtils.getText(flagText);
            m_img_icon_PolygonImage.assetName = "";
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, img);
            
            m_btn_animButton_GameButton.onClick.RemoveAllListeners();
            m_btn_animButton_GameButton.onClick.AddListener(() =>
                {
                    HelpTip.CreateTip(descId, m_btn_animButton_GameButton.gameObject.transform).SetOffset(offset).Show();
                });
        }

        public void SetGray(bool isGray)
        {
            if (isGray)
            {
                m_makeChildrenGray?.Gray();    
            }
            else
            {
                m_makeChildrenGray?.Normal();
            }
            
        }

    }
}