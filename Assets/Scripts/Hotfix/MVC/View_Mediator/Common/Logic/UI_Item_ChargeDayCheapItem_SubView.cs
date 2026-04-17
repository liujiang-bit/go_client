// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月19日
// Update Time         :    2020年5月19日
// Class Description   :    UI_Item_ChargeDayCheapItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;

namespace Game {
    public partial class UI_Item_ChargeDayCheapItem_SubView : UI_SubView
    {
        
        private Data.RechargeDailySpecialDefine m_SpecialInfo;
        private List<RewardGroupData> m_rewardGroupData;

        protected override void BindEvent()
        {
            m_btn_buy.m_btn_languageButton_GameButton.onClick.AddListener(OnBuyClickEvent);
            m_btn_boxinfo_GameButton.onClick.AddListener(OnDetailClickEvent);
            m_btn_tipsinfo_GameButton.onClick.AddListener(OnReturnClickEvent);
        }

        public void InitData(Data.RechargeDailySpecialDefine specialInfo)
        {
            m_SpecialInfo = specialInfo;
            if (m_SpecialInfo != null)
            {
                RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
                int rewardId = rechargeProxy.GetDayCheapGiftRewardId(m_SpecialInfo);
                if (rewardId > 0)
                {
                    RewardGroupProxy rewardGroupProxy =
                        AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                    m_rewardGroupData = rewardGroupProxy.GetRewardDataByGroup(rewardId);
                }
            }
        }

        public void RefreshUI()
        {
            if (m_SpecialInfo == null) return;
            m_lbl_name_LanguageText.text = LanguageUtils.getText(m_SpecialInfo.l_nameID);
            ClientUtils.LoadSprite(m_img_box_PolygonImage, m_SpecialInfo.icon);
            m_lbl_get_LanguageText.text = LanguageUtils.getTextFormat(800070,ClientUtils.FormatComma(m_SpecialInfo.rechargeProgress));
            m_lbl_dec_LanguageText.text = LanguageUtils.getTextFormat(m_SpecialInfo.l_desID, ClientUtils.FormatComma(m_SpecialInfo.giftValue));
            
            RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            string strPrice = string.Empty;
            Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(m_SpecialInfo.price);
            if (priceCfg != null)
            {
                var gameItems = SDKPayment.shareInstance().GetIGGGameItems();
                if (gameItems != null)
                {
                    SDKGameItem funGameItem = null;
                    foreach (var gameItem in gameItems)
                    {
                        if (gameItem.getId() == priceCfg.rechargeID.ToString())
                        {
                            funGameItem = gameItem;
                            break;
                        }
                    }

                    if (funGameItem != null && funGameItem.getPurchase() != null)
                    {
                        strPrice = funGameItem.getShopCurrencyPrice();
                    }
                }

                if (string.IsNullOrEmpty(strPrice))
                {
                    strPrice = $"${priceCfg.price}";
                }
            }
            m_btn_buy.m_lbl_Text_LanguageText.text = strPrice;
            if (rechargeProxy.isDailyGiftBought(m_SpecialInfo.price))
            {
                m_btn_buy.SetGray(true);
                m_btn_buy.m_btn_languageButton_GameButton.interactable = false;
            }
            else
            {
                m_btn_buy.SetGray(false);
                m_btn_buy.m_btn_languageButton_GameButton.interactable = true;
            }

            if (m_rewardGroupData != null)
            {
                m_sv_list_ListView.Clear();
                ClientUtils.PreLoadRes(gameObject, m_sv_list_ListView.ItemPrefabDataList, LoadRewardFinish);
            }
        }

        private void LoadRewardFinish(Dictionary<string, GameObject> dic)
        {

            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitRewardListItem;
            m_sv_list_ListView.SetInitData(dic, functab);
            m_sv_list_ListView.FillContent(m_rewardGroupData.Count);

        }
        
        private void InitRewardListItem(ListView.ListItem item)
        {
            UI_Item_ChargeItemListItem_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_ChargeItemListItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_ChargeItemListItem_SubView;
            }
            if (subView == null) return;
            if (item.index >= m_rewardGroupData.Count) return;
            RewardGroupData rewardGroupData = m_rewardGroupData[item.index];
            if (rewardGroupData != null)
            {
                subView.InitData(rewardGroupData);
                subView.RefreshUI();
            }
        }
        
        public void OnItemDestroy()
        {

        }

        public void OnBuyClickEvent()
        {
            Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(m_SpecialInfo.price);
            if (priceCfg == null) return;
            RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            rechargeProxy.CallSdkBuyByPcid(priceCfg,priceCfg.rechargeID.ToString(),priceCfg.price.ToString("N2"));
        }
        
        private void OnItemBuy(SDKException ex, bool bUserCancel)
        {
            
        }

        private void OnDetailClickEvent()
        {
            m_pl_box.gameObject.SetActive(false);
            m_pl_tips.gameObject.SetActive(true);
        }

        private void OnReturnClickEvent()
        {
            m_pl_box.gameObject.SetActive(true);
            m_pl_tips.gameObject.SetActive(false);
        }
    }
}