// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月15日
// Update Time         :    2020年6月15日
// Class Description   :    UI_Item_MailRank_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_MailRank_SubView : UI_SubView
    {
        private bool m_isInit;
        private float m_initHeight;
        private List<UI_Item_MailRankChild_SubView> m_childList;
        private List<UI_Item_MailRankReward_SubView> m_itemViewList;
        private float m_itemHeight;
        private RewardGroupProxy m_rewardGroupProxy;

        public void Refresh(AllianceMemberDonateInfo memberInfo)
        {
            if (!m_isInit)
            {
                m_initHeight = m_root_RectTransform.rect.height;
                m_childList = new List<UI_Item_MailRankChild_SubView>();
                m_itemHeight = m_UI_Item_MailRankChild.m_root_RectTransform.rect.height;
                m_itemViewList = new List<UI_Item_MailRankReward_SubView>();
                m_itemViewList.Add(m_UI_Item_MailRankReward1);
                m_itemViewList.Add(m_UI_Item_MailRankReward2);
                m_itemViewList.Add(m_UI_Item_MailRankReward3);
                m_itemViewList.Add(m_UI_Item_MailRankReward4);
                m_itemViewList.Add(m_UI_Item_MailRankReward5);
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                m_isInit = true;
            }
            if (memberInfo.DonateRankingDefine.targetMin == memberInfo.DonateRankingDefine.targetMax)
            {
                m_lbl_rank_title_LanguageText.text = LanguageUtils.getTextFormat(570078, memberInfo.DonateRankingDefine.targetMin);
            }
            else
            {
                m_lbl_rank_title_LanguageText.text = LanguageUtils.getTextFormat(570079, memberInfo.DonateRankingDefine.targetMin,
                                                     memberInfo.DonateRankingDefine.targetMax);
            }

            //奖励组列表
            RefreshRewardGroup(memberInfo.DonateRankingDefine.itemPackage);

            int rankNum = memberInfo.DonateRankingDefine.targetMin;
            int count = m_childList.Count;
            int infoTotal = memberInfo.DonateInfoList.Count;
            for (int i = 0; i < memberInfo.DonateInfoList.Count; i++)
            {
                if (i < count)
                {
                    m_childList[i].gameObject.SetActive(true);
                    m_childList[i].Refresh(memberInfo.DonateInfoList[i], rankNum + i);
                }
                else
                {
                    CoreUtils.assetService.Instantiate("UI_Item_MailRankChild", (node) =>
                    {
                        if (gameObject == null)
                        {
                            if (node != null)
                            {
                                CoreUtils.assetService.Destroy(node);
                                node = null;
                            }
                            return;
                        }
                        node.transform.SetParent(m_pl_player_GridLayoutGroup.transform);
                        node.transform.localScale = Vector3.one;

                        UI_Item_MailRankChild_SubView childView = new UI_Item_MailRankChild_SubView(node.GetComponent<RectTransform>());
                        m_childList.Add(childView);
                        int index = m_childList.Count - 1;
                        if (index < infoTotal)
                        {
                            childView.Refresh(memberInfo.DonateInfoList[index], rankNum + index);
                        }
                        else
                        {
                            childView.gameObject.SetActive(false);
                        }
                    });
                }
            }
            for (int i = 0; i < count; i++)
            {
                if (i >= infoTotal)
                {
                    m_childList[i].gameObject.SetActive(false);
                }
            }
            if (infoTotal > 1)
            {
                m_root_RectTransform.sizeDelta = new Vector2(m_root_RectTransform.rect.width, m_itemHeight*(infoTotal-1)+m_initHeight);
            }
            else
            {
                m_root_RectTransform.sizeDelta = new Vector2(m_root_RectTransform.rect.width, m_initHeight);
            }
        }

        //刷新奖励组物品
        private void RefreshRewardGroup(int itemPackage)
        {
            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            for (int i = 0; i < 5; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].m_UI_Model_Item.RefreshByGroup(groupDataList[i], 2);
                    m_itemViewList[i].m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(145048, ClientUtils.FormatComma(groupDataList[i].number));
                }
                else
                {
                    m_itemViewList[i].gameObject.SetActive(false);
                }
            }
        }
    }
}