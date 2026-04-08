// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月24日
// Update Time         :    2020年4月24日
// Class Description   :    UI_Item_GuildRankingBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_GuildRankingBtn_SubView : UI_SubView
    {
        public void SetBtnInfo(Rank_ShowRankFirst.response.RankInfo info,UnityAction<int> clickCallback)
        {
            int type = (int)info.type;
            var rankingCfg = CoreUtils.dataService.QueryRecord<LeaderboardDefine>(type);
            m_lbl_title_LanguageText.text = LanguageUtils.getText(rankingCfg.nameID);
            m_lbl_name_LanguageText.text = info.name;
            ClientUtils.LoadSprite(m_img_icon_PolygonImage,rankingCfg.icon);
            ClientUtils.LoadSprite(m_img_flag_PolygonImage,rankingCfg.flag);
            
            m_btn_btn_GameButton.onClick.RemoveAllListeners();
            m_btn_btn_GameButton.onClick.AddListener(() =>
            {
                clickCallback?.Invoke(type);
            });
        }
    }
}