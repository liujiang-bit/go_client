// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_StrongerPlayerRank_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using System.Collections.Generic;
using System;

namespace Game {
    public partial class UI_Item_StrongerPlayerRank_SubView : UI_SubView
    {
        private bool m_isInit;
        private List<UI_Item_EventTypeRankItem_SubView> m_rankViewList;

        public void Refresh(Activity_GetHistoryRank.response.HistoryRank rankData)
        {
            if (!m_isInit)
            {
                m_rankViewList = new List<UI_Item_EventTypeRankItem_SubView>();
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem1);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem2);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem3);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem4);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem5);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem6);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem7);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem8);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem9);
                m_rankViewList.Add(m_UI_Item_EventTypeRankItem10);

                int count = m_rankViewList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i > 2)
                    {
                        m_rankViewList[i].m_img_rank_PolygonImage.gameObject.SetActive(false);
                        m_rankViewList[i].m_lbl_rank_LanguageText.gameObject.SetActive(true);
                    }
                    else
                    {
                        m_rankViewList[i].m_lbl_rank_LanguageText.gameObject.SetActive(false);
                    }
                    m_rankViewList[i].m_UI_PlayerHead.gameObject.SetActive(false);
                    m_rankViewList[i].m_lbl_rank_LanguageText.text = (i + 1).ToString();
                }
                m_isInit = true;
            }
            m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(762198, rankData.index);
            DateTime times = ServerTimeModule.Instance.ConverToServerDateTime(rankData.time);
            m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(100717, times.ToString("yyyy/MM/dd"));

            int count2 = 0;
            if (rankData.historyInfo != null)
            {
                count2 = rankData.historyInfo.Count;
            }
            int count1 = m_rankViewList.Count;
            for (int i = 0; i < count1; i++)
            {
                if (i < count2)
                {
                    m_rankViewList[i].gameObject.SetActive(true);
                    m_rankViewList[i].m_lbl_source_LanguageText.text = ClientUtils.FormatComma(rankData.historyInfo[i].score);

                    string name = "";
                    if (string.IsNullOrEmpty(rankData.historyInfo[i].abbreviationName))
                    {
                        name = rankData.historyInfo[i].name;
                    }
                    else
                    {
                        name = LanguageUtils.getTextFormat(300030, rankData.historyInfo[i].abbreviationName, rankData.historyInfo[i].name);
                    }
                    m_rankViewList[i].m_lbl_name_LanguageText.text = name;
                }
                else
                {
                    m_rankViewList[i].m_lbl_source_LanguageText.text = "";
                    m_rankViewList[i].m_lbl_name_LanguageText.text = "";
                }
            }
        }
    }
}