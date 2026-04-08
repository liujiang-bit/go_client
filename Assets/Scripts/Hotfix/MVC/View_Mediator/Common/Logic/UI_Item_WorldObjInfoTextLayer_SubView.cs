// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Item_WorldObjInfoTextLayer_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_WorldObjInfoTextLayer_SubView : UI_SubView
    {
        public void setLine1(string title,string value)
        {
            this.m_lbl_title_LanguageText.text = title;
            this.m_lbl_content_LanguageText.text = value;

        }
        public void setLine2(string title,string value,string colorStr="")
        {
            this.m_lbl_title_LanguageText.text = title;
            this.m_lbl_content_LanguageText.text = value;

            if (colorStr!="")
            {
                Color color = this.m_lbl_content_LanguageText.color;

                ColorUtility.TryParseHtmlString(colorStr, out color);
                this.m_lbl_content_LanguageText.color = color;
            }
        }
        
        public void setLine3(string title,string value)
        {
            this.m_lbl_title_LanguageText.text = title;
            this.m_lbl_content_LanguageText.text = value;

        }
    }
}