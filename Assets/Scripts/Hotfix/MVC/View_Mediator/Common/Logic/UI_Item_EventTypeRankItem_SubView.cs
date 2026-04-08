// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    UI_Item_EventTypeRankItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;

namespace Game {
    public partial class UI_Item_EventTypeRankItem_SubView : UI_SubView
    {
        private RankItemData m_rankItemData;

        public void Refresh(RankItemData data)
        {
            m_rankItemData = data;
            if (data.RankData != null)
            {
                Refresh(data.RankData);
            }
            else if (data.StageRankData != null)
            {
                Refresh(data.StageRankData);
            }
        }

        public void Refresh(Rank_QueryRank.response.RankInfo rankInfo)
        {
            if (rankInfo.index < 4) //前三
            {
                m_img_rank_PolygonImage.gameObject.SetActive(true);
                int index = (int)rankInfo.index - 1;
                if (index >= 0 && index < 3)
                {
                    ClientUtils.LoadSprite(m_img_rank_PolygonImage, RS.ActivityRankIconBg[index]);
                }
                m_lbl_rank_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                m_img_rank_PolygonImage.gameObject.SetActive(false);
                m_lbl_rank_LanguageText.gameObject.SetActive(true);
            }

            m_lbl_source_LanguageText.text = ClientUtils.FormatComma(rankInfo.score);

            m_lbl_rank_LanguageText.text = rankInfo.index.ToString();

            if (m_rankItemData.Status == 1)
            {
                m_UI_PlayerHead.gameObject.SetActive(false);
                m_UI_Model_GuildFlag.gameObject.SetActive(true);

                string name = "";
                if (string.IsNullOrEmpty(rankInfo.abbreviationName))
                {
                    name = rankInfo.name;
                }
                else
                {
                    name = LanguageUtils.getTextFormat(300030, rankInfo.abbreviationName, rankInfo.guildName);
                }
                m_lbl_name_LanguageText.text = name;

                m_UI_Model_GuildFlag.setData(rankInfo.signs);
            }else
            {
                string name = "";
                if (string.IsNullOrEmpty(rankInfo.abbreviationName))
                {
                    name = rankInfo.name;
                }
                else
                {
                    name = LanguageUtils.getTextFormat(300030, rankInfo.abbreviationName, rankInfo.name);
                }
                m_lbl_name_LanguageText.text = name;

                m_UI_PlayerHead.gameObject.SetActive(true);
                m_UI_Model_GuildFlag.gameObject.SetActive(false);
                m_UI_PlayerHead.LoadPlayerIcon(rankInfo.headId, rankInfo.headFrameID);
            }
        }

        public void Refresh(Activity_GetRank.response.RankList.RankInfo rankInfo)
        {
            if (rankInfo.index < 4) //前三
            {
                m_img_rank_PolygonImage.gameObject.SetActive(true);
                int index = (int)rankInfo.index - 1;
                if (index >= 0 && index < 3)
                {
                    ClientUtils.LoadSprite(m_img_rank_PolygonImage, RS.ActivityRankIconBg[index]);
                }
                m_lbl_rank_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                m_img_rank_PolygonImage.gameObject.SetActive(false);
                m_lbl_rank_LanguageText.gameObject.SetActive(true);
            }

            m_lbl_source_LanguageText.text = ClientUtils.FormatComma(rankInfo.score);

            string name = "";
            if (string.IsNullOrEmpty(rankInfo.abbreviationName))
            {
                name = rankInfo.name;
            }
            else
            {
                name = LanguageUtils.getTextFormat(300030, rankInfo.abbreviationName, rankInfo.name);
            }
            m_lbl_name_LanguageText.text = name;

            m_lbl_rank_LanguageText.text = rankInfo.index.ToString();

            m_UI_PlayerHead.LoadPlayerIcon(rankInfo.headId, rankInfo.headFrameID);
        }
    }
}