// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月30日
// Update Time         :    2020年6月30日
// Class Description   :    UI_Item_MailWarTips_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_MailWarTips_SubView : UI_SubView
    {
        public void Refresh(FightReportReinforceData itemData)
        {
            m_UI_Model_PlayerHead.LoadPlayerIcon(itemData.ReinforceInfo.headId, itemData.ReinforceInfo.headFrameID);
            m_lbl_playername_LanguageText.text = itemData.ReinforceInfo.name;
            m_lbl_troopsnum_LanguageText.text = LanguageUtils.getTextFormat(570025, ClientUtils.FormatComma(itemData.ReinforceInfo.armyCount));
            if (itemData.ReinforceType == 1) //援军加入
            {
                if (itemData.ReinforceInfo.isCityJoin)
                {
                    m_lbl_title_LanguageText.text = LanguageUtils.getText(200561);
                }
                else if (itemData.ReinforceInfo.isArmyBack)
                {
                    m_lbl_title_LanguageText.text = LanguageUtils.getText(570027);
                }
                else
                {
                    m_lbl_title_LanguageText.text = LanguageUtils.getText(570027);
                }
            }
            else//援军撤离
            {
                m_lbl_title_LanguageText.text = LanguageUtils.getText(570028);
            }
        }
    }
}