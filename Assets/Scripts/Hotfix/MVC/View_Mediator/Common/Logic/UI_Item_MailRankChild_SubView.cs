// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月15日
// Update Time         :    2020年6月15日
// Class Description   :    UI_Item_MailRankChild_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;

namespace Game {
    public partial class UI_Item_MailRankChild_SubView : UI_SubView
    {
        public void Refresh(RoleDonateInfo donateInfo, int rankNum)
        {
            m_lbl_name_LanguageText.text = donateInfo.name;
            m_lbl_score_LanguageText.text = LanguageUtils.getTextFormat(570099, ClientUtils.FormatComma(donateInfo.donateNum));
            if (rankNum >= 1 && rankNum <= 3)
            {
                m_img_rank_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(m_img_rank_PolygonImage, RS.DonateRankingIcon[rankNum - 1]);
                m_lbl_rank_LanguageText.text = "";
            }
            else
            {
                m_img_rank_PolygonImage.gameObject.SetActive(false);
                m_lbl_rank_LanguageText.text = rankNum.ToString();
            }
        }
    }
}