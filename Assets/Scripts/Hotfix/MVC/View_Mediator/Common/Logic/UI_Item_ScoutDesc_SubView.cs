// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Item_ScoutDesc_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_ScoutDesc_SubView : UI_SubView
    {
        protected override void BindEvent()
        {
            UpdateData();
        }
        public void UpdateData()
        {
            float num = 100 - (float)WarFogMgr.FogNumber * 100 / (400 * 400);
            if (num >= 100)
            {
                m_lbl_progress_LanguageText.text = LanguageUtils.getText(181147);
                m_lbl_test_LanguageText.text = LanguageUtils.getText(181164);
            }
            else
            {
                m_lbl_progress_LanguageText.text = LanguageUtils.getTextFormat(181145, string.Format("{0:F}", num));
                m_lbl_test_LanguageText.text = LanguageUtils.getText(181144);
            }
        }
    }
}