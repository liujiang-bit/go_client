// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_EventBtnSource_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_EventBtnSource_SubView : UI_SubView
    {
        private bool m_isInit;
        private ActivityInfernalDefine m_define;
        private HelpTip m_helpTip;

        public void Refresh(int id)
        {
            if (!m_isInit)
            {
                m_UI_Item_EventBtnSource_GameButton.onClick.AddListener(OnClick);
                m_isInit = true;
            }
            ActivityInfernalDefine define = CoreUtils.dataService.QueryRecord<ActivityInfernalDefine>(id);
            m_define = define;
            if (define != null)
            {
                m_lbl_source_LanguageText.text = LanguageUtils.getText(define.desID);
                ClientUtils.LoadSprite(m_img_source_PolygonImage, define.icon);
                ClientUtils.ImageSetColor(m_img_sourcefra_PolygonImage, define.color);
            }
        }

        private void OnClick()
        {
            if (m_define == null)
            {
                return;
            }
            m_helpTip = HelpTip.CreateTip(m_define.tipsID, m_root_RectTransform)
                            .SetAutoFilter(true)
                            .SetOffset(m_root_RectTransform.rect.width/2)
                            .Show();
        }

        public void CloseTip()
        {
            if (m_helpTip != null)
            {
                m_helpTip.Close();
                m_helpTip = null;
            }
        }
    }
}