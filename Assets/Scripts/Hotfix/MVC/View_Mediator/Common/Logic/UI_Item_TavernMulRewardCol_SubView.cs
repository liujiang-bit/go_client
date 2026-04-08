// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_Item_TavernMulRewardCol_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;
using System;

namespace Game {
    public partial class UI_Item_TavernMulRewardCol_SubView : UI_SubView
    {
        public List<UI_Item_TavernMulRewardView> ElemItemList = new List<UI_Item_TavernMulRewardView>();
        public List<TavernRewardItemData> rewardDataList = new List<TavernRewardItemData>(); 
        public int ConstraintCount;

        public void Init(GameObject itemPrefab)
        {
            ConstraintCount = m_UI_Item_TavernMulRewardCol_GridLayoutGroup.constraintCount;
            for (int i = 0; i < ConstraintCount; i++)
            {
                GameObject itemObj = CoreUtils.assetService.Instantiate(itemPrefab);
                itemObj.transform.SetParent(gameObject.transform);
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.transform.localScale = Vector3.one;
                UI_Item_TavernMulRewardView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_TavernMulRewardView>(itemObj.gameObject);
                ElemItemList.Add(itemView);
                rewardDataList.Add(null);
                int num = i;
                itemView.m_UI_Model_Item.AddBtnListener(()=> {
                    ClickItem(ElemItemList[num], num);
                });
                itemView.m_UI_Model_CaptainHeadBtn.m_btn_languageButton_GameButton.onClick.AddListener(() => {
                    ClickItem(ElemItemList[num], num);
                });
            }
        }

        private void ClickItem(UI_Item_TavernMulRewardView itemView, int index)
        {
            TavernRewardItemData itemData = rewardDataList[index];
            string str = "";
            if (itemData.HeroInfo != null) //英雄
            {
                HeroDefine define = CoreUtils.dataService.QueryRecord<HeroDefine>((int)itemData.HeroInfo.heroId);
                if (define != null)
                {
                    str = LanguageUtils.getText(define.l_nameID);
                }
            }
            else //道具
            {
                ItemDefine define = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemData.ItemInfo.itemId);
                str = LanguageUtils.getText(define.l_nameID);
            }

            HelpTip.CreateTip(str, itemView.gameObject.GetComponent<RectTransform>()).SetStyle(HelpTipData.Style.arrowDown).SetOffset(50).Show();
        }

        public void RefreshItem(TavernRewardItemData itemData, int index)
        {
            UI_Item_TavernMulRewardView nodeView = ElemItemList[index];
            rewardDataList[index] = itemData;
            if (itemData.HeroInfo != null) //英雄
            {
                nodeView.m_UI_Model_Item.gameObject.SetActive(false);
                nodeView.m_UI_Model_CaptainHeadBtn.gameObject.SetActive(true);

                HeroDefine define = CoreUtils.dataService.QueryRecord<HeroDefine>((int)itemData.HeroInfo.heroId);
                if (define != null)
                {
                    nodeView.m_UI_Model_CaptainHeadBtn.m_UI_Model_CaptainHead.SetIcon(define.heroIcon);
                    nodeView.m_UI_Model_CaptainHeadBtn.m_UI_Model_CaptainHead.SetRare(define.rare);
                }
                nodeView.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(145048, ClientUtils.FormatComma(itemData.HeroInfo.num));
                //nodeView.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(itemData.HeroInfo.num);
            }
            else //道具
            {
                nodeView.m_UI_Model_Item.gameObject.SetActive(true);
                nodeView.m_UI_Model_CaptainHeadBtn.gameObject.SetActive(false);

                ItemDefine define1 = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemData.ItemInfo.itemId);
                nodeView.m_UI_Model_Item.Refresh(define1, 0, false);
                nodeView.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(145048, ClientUtils.FormatComma(itemData.ItemInfo.itemNum));
                //nodeView.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(itemData.ItemInfo.itemNum);
            }
        }
    }
}