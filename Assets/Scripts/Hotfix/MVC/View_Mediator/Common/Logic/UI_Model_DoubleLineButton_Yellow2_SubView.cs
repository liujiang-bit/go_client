// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月13日
// Update Time         :    2020年5月13日
// Class Description   :    UI_Model_DoubleLineButton_Yellow2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_DoubleLineButton_Yellow2_SubView : UI_SubView
    {
        public void SetResInfo(string iconPath,int number)
        {
            SetResVisible(true);
            ClientUtils.LoadSprite(m_img_icon2_PolygonImage,iconPath);
            m_lbl_line2_LanguageText.text = ClientUtils.FormatComma(number);
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        public void SetResVisible(bool isVisible)
        {
            m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(isVisible);
        }
        
        public void SetGray(bool gray)
        {
            GrayChildrens makeChildrenGray=null;
            if (makeChildrenGray == null)
            {
                makeChildrenGray= this.gameObject.GetComponent<GrayChildrens>();
                if (makeChildrenGray == null)
                {
                    makeChildrenGray= this.gameObject.AddComponent<GrayChildrens>();
                }
            }

            if (gray)
            {             
                makeChildrenGray.Gray(); 
            }
            else
            {
                makeChildrenGray.Normal();
            }
        }
    }
}