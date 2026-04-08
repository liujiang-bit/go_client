// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_Item_TalentSpecialText_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_TalentSpecialText_SubView : UI_SubView
    {
        public void Init(HeroTalentMasteryDefine masteryDefine,int talentPoint)
        {
            m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(181104, talentPoint,masteryDefine.needTalentPoint);
            m_lbl_message_LanguageText.text = LanguageUtils.getText(masteryDefine.descID);
            m_lbl_lv_LanguageText.text = LanguageUtils.getText(300239 + masteryDefine.level - 1);
            if (masteryDefine.needTalentPoint > talentPoint)
            {
                m_lbl_lv_LanguageText.color = Color.gray;
                m_lbl_message_LanguageText.color = Color.gray;
                m_lbl_num_LanguageText.color = Color.gray;
            }
            else
            {
                
                m_lbl_lv_LanguageText.color = Color.black;
                m_lbl_message_LanguageText.color = Color.black;
                m_lbl_num_LanguageText.color = Color.black;
            }
        }
    }
}