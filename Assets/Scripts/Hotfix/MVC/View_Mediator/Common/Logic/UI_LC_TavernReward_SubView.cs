// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_LC_TavernReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;

namespace Game {
    public partial class UI_LC_TavernReward_SubView : UI_SubView
    {
        private BoxPreviewRewardData m_rewardData;
        private int m_constraintCount;
        private List<UI_Item_PreviewReward_SubView> m_viewList;

        public void Init()
        {
            m_constraintCount = m_UI_LC_TavernReward_GridLayoutGroup.constraintCount;
            m_viewList = new List<UI_Item_PreviewReward_SubView>();
            m_viewList.Add(m_UI_Item_PreviewReward1);
            m_viewList.Add(m_UI_Item_PreviewReward2);
            m_viewList.Add(m_UI_Item_PreviewReward3);

            for (int i = 0; i < m_viewList.Count; i++)
            {
                m_viewList[i].m_UI_Model_Item.ItemData1 = i;
                m_viewList[i].m_UI_Model_Item.BtnClickListener = ClickItemEvent;
                m_viewList[i].m_UI_Model_Item.AddBtnListener();

                m_viewList[i].m_UI_Model_CaptainHeadBtn.ItemData1 = i;
                m_viewList[i].m_UI_Model_CaptainHeadBtn.BtnClickListener = ClickHeadEvent;
                m_viewList[i].m_UI_Model_CaptainHeadBtn.AddBtnListener();
            }
        }

        private void ClickItemEvent(UI_Model_Item_SubView subView)
        {
            int index = subView.ItemData1;
            TavernRankDefine define = CoreUtils.dataService.QueryRecord<TavernRankDefine>(m_rewardData.DataList[index]);
            if (define != null)
            {
                if (define.type == 200)//道具
                {
                    ItemDefine define1 = CoreUtils.dataService.QueryRecord<ItemDefine>(define.typeData);
                    if (define1 != null)
                    {
                        HelpTip.CreateTip(LanguageUtils.getText(define1.l_nameID), subView.m_root_RectTransform).SetStyle(HelpTipData.Style.arrowUp).SetOffset(50).Show();
                    }
                }
            }
        }

        private void ClickHeadEvent(UI_Model_CaptainHeadBtn_SubView subView)
        {
            int index = subView.ItemData1;
            TavernRankDefine define = CoreUtils.dataService.QueryRecord<TavernRankDefine>(m_rewardData.DataList[index]);
            if (define != null)
            {
                if (define.type == 400)//统帅
                {
                    HeroDefine define1 = CoreUtils.dataService.QueryRecord<HeroDefine>(define.typeData);
                    if (define1 != null)
                    {
                        HelpTip.CreateTip(LanguageUtils.getText(define1.l_nameID), subView.m_root_RectTransform).SetStyle(HelpTipData.Style.arrowUp).SetOffset(50).Show();
                    }
                }
            }
        }

        public void RefreshItem(BoxPreviewRewardData rewardData)
        {
            m_rewardData = rewardData;
            int count = m_rewardData.DataList.Count;

            for (int i = 0; i < m_constraintCount; i++)
            {
                if (i >= count)
                {
                    m_viewList[i].gameObject.SetActive(false);
                    continue;                                        
                }
                m_viewList[i].gameObject.SetActive(true);

                TavernRankDefine define = CoreUtils.dataService.QueryRecord<TavernRankDefine>(m_rewardData.DataList[i]);
                if (define != null)
                {
                    UI_Item_PreviewReward_SubView subView = m_viewList[i];
                    if (define.type == 200)//道具
                    {
                        subView.m_UI_Model_Item.gameObject.SetActive(true);
                        subView.m_UI_Model_CaptainHeadBtn.gameObject.SetActive(false);

                        ItemDefine define1 = CoreUtils.dataService.QueryRecord<ItemDefine>(define.typeData);
                        if (define1 != null)
                        {
                            subView.m_UI_Model_Item.Refresh(define1, 0, false);
                        }
                    }
                    else if (define.type == 400)//统帅
                    {
                        subView.m_UI_Model_Item.gameObject.SetActive(false);
                        subView.m_UI_Model_CaptainHeadBtn.gameObject.SetActive(true);

                        HeroDefine define1 = CoreUtils.dataService.QueryRecord<HeroDefine>(define.typeData);
                        if (define1 != null)
                        {
                            subView.m_UI_Model_CaptainHeadBtn.m_UI_Model_CaptainHead.SetIcon(define1.heroIcon);
                            subView.m_UI_Model_CaptainHeadBtn.m_UI_Model_CaptainHead.SetRare(define1.rare);
                        }
                    }
                }
            }
        }
    }
}