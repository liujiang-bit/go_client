// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月11日
// Update Time         :    2020年6月11日
// Class Description   :    UI_Item_ExpeditionFightTask_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_ExpeditionFightTask_SubView : UI_SubView
    {
        public void Show(Data.ExpeditionDefine cfg)
        {
            if (cfg.starDesc.Count < 3) return;
            m_lbl_target1_LanguageText.text = LanguageUtils.getText(cfg.starDesc[0]);
            m_lbl_target2_LanguageText.text = LanguageUtils.getTextFormat(cfg.starDesc[1], cfg.starCondition2[1]);
            m_lbl_target3_LanguageText.text = LanguageUtils.getTextFormat(cfg.starDesc[2], cfg.starCondition3[1]);
        }

        public void Show(Data.ExpeditionDefine cfg, List<long> starResult)
        {
            Show(cfg);
            if(starResult[0] == 0)
            {
                m_lbl_target1_LanguageText.color = Color.gray;
            }
            if (starResult[1] == 0)
            {
                m_lbl_target2_LanguageText.color = Color.gray;
            }
            if (starResult[2] == 0)
            {
                m_lbl_target3_LanguageText.color = Color.gray;
            }
        }
    }
}