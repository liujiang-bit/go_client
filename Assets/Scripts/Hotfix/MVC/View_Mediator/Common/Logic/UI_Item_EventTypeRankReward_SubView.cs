// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    UI_Item_EventTypeRankReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;
using System;

namespace Game {
    public partial class UI_Item_EventTypeRankReward_SubView : UI_SubView
    {
        private List<UI_Model_RewardGet_SubView> m_itemViewList;
        private RewardGroupProxy m_rewardGroupProxy;
        private bool m_isInit;

        private float m_baseHeight;
        private float m_itemHeight;
        private int m_constraintCount;

        public void Refresh(ActivityRankingTypeDefine define)
        {
            if (!m_isInit)
            {
                m_itemViewList = new List<UI_Model_RewardGet_SubView>();
                m_itemViewList.Add(m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_Item3);
                m_itemViewList.Add(m_UI_Model_Item4);
                m_itemViewList.Add(m_UI_Model_Item5);

                for (int i = 0; i < m_itemViewList.Count; i++)
                {
                    m_itemViewList[i].SetScale(m_pl_reward_ArabLayoutCompment.transform.localScale.x);
                }

                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;

                m_baseHeight = m_root_RectTransform.rect.height;

                m_itemHeight = m_pl_reward_GridLayoutGroup.cellSize.y * m_pl_reward_ArabLayoutCompment.transform.localScale.x +
                               m_pl_reward_GridLayoutGroup.spacing.y * m_pl_reward_ArabLayoutCompment.transform.localScale.x;

                m_constraintCount = m_pl_reward_GridLayoutGroup.constraintCount;

                m_isInit = true;
            }
            if (define.targetMin == define.targetMax)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(762121, define.targetMax);
            }
            else
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(762122, define.targetMin, define.targetMax);
            }

            if (define.targetMin > 0 && define.targetMin < 4)
            {
                ClientUtils.LoadSprite(m_img_title_PolygonImage, RS.ActivityRankRewardTitleBg[define.targetMin - 1]);
            }
            else
            {
                ClientUtils.LoadSprite(m_img_title_PolygonImage, RS.ActivityRankRewardTitleBg[3]);
            }

            RefreshRewardGroup(define.itemPackage);
        }

        private void RefreshRewardGroup(int itemPackage)
        {
            List<RewardGroupData> groupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            int itemCount = m_itemViewList.Count;
            int diff = count - itemCount;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    GameObject go = CoreUtils.assetService.Instantiate(m_itemViewList[4].gameObject);
                    go.transform.SetParent(m_pl_reward_ArabLayoutCompment.transform);
                    go.transform.localScale = Vector3.one;
                    UI_Model_RewardGet_SubView subView = new UI_Model_RewardGet_SubView(go.GetComponent<RectTransform>());
                    subView.SetScale(m_pl_reward_ArabLayoutCompment.transform.localScale.x);
                    m_itemViewList.Add(subView);
                }
            }
            itemCount = m_itemViewList.Count;
            for (int i = 0; i < itemCount; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].RefreshByGroup(groupDataList[i], 3);
                }
                else
                {
                    m_itemViewList[i].gameObject.SetActive(false);
                }
            }

            int line = (int)Math.Ceiling((float)count/ m_constraintCount);
            if (line > 1)
            {
                m_root_RectTransform.sizeDelta = new Vector2(m_root_RectTransform.rect.width, m_baseHeight+(line-1)*m_itemHeight);
            }
            else
            {
                m_root_RectTransform.sizeDelta = new Vector2(m_root_RectTransform.rect.width, m_baseHeight);
            }
        }

        public float GetHeight()
        {
            return m_root_RectTransform.rect.height;
        }
    }
}