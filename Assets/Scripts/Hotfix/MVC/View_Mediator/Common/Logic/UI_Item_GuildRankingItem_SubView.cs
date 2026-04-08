// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月2日
// Update Time         :    2020年6月2日
// Class Description   :    UI_Item_GuildRankingItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using PureMVC.Core;
using SprotoType;

namespace Game {
    public partial class UI_Item_GuildRankingItem_SubView : UI_SubView
    {
        public void SetInfo(Rank_QueryRank.response.RankInfo info)
        {
            if (info.index <= 3 && info.index > 0)
            {
                ClientUtils.LoadSprite(m_img_rank_PolygonImage,RS.RankingTop3IconName[info.index-1]);
                m_img_rank_PolygonImage.gameObject.SetActive(true);
                m_lbl_rank_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                m_lbl_rank_LanguageText.text = info.index.ToString();
                m_img_rank_PolygonImage.gameObject.SetActive(false);     
                m_lbl_rank_LanguageText.gameObject.SetActive(true);
            }

            
            var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            var OfficialInfo = allianceProxy.getMemberOfficer(info.rid);
            if (OfficialInfo != null)
            {
                var cdata = CoreUtils.dataService.QueryRecord<AllianceOfficiallyDefine>((int) OfficialInfo.officerId);
                m_lbl_job_LanguageText.text = LanguageUtils.getText(cdata.l_officiallyID);
            }
            else
            {
                m_lbl_job_LanguageText.text = "-";
            }

            m_lbl_power_LanguageText.text = ClientUtils.FormatComma(info.score);

            m_lbl_name_LanguageText.text = info.name;
            m_UI_PlayerHead.LoadPlayerIcon(info.headId,info.headFrameID);
        }
    }
}