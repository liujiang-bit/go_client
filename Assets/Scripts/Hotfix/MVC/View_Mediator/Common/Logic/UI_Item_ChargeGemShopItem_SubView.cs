// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_ChargeGemShopItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_ChargeGemShopItem_SubView : UI_SubView
    {
        private RechargeGemMallDefine m_cfgData;
        public void Refresh(RechargeGemMallDefine cfgData)
        {
            m_cfgData = cfgData;
            m_lbl_name_LanguageText.text = LanguageUtils.getText(cfgData.l_nameID);
            m_lbl_count_LanguageText.text = ClientUtils.FormatComma(cfgData.denarNum).ToString();
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, cfgData.gemIcon);
            var rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            
            var cfgPrice = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(cfgData.price);
            bool isFirstAdd = rechargeProxy.IsFirstAdd(cfgPrice.ID);
            m_lbl_addName_LanguageText.text = isFirstAdd ? LanguageUtils.getText(800055) : LanguageUtils.getText(800111);
            m_lbl_add_LanguageText.text = "+" + ( isFirstAdd ? ClientUtils.FormatComma(cfgData.firstPresenter).ToString() : ClientUtils.FormatComma(cfgData.presenter).ToString());

            
            string strPrice = string.Empty;
            if (cfgPrice != null)
            {
                var gameItems = IGGPayment.shareInstance().GetIGGGameItems();
                if (gameItems != null)
                {
                    IGGGameItem funGameItem = null;
                    foreach (var gameItem in gameItems)
                    {
                        if (gameItem.getId() == cfgPrice.rechargeID.ToString())
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
                    strPrice = $"${cfgPrice.price}";
                }
            }
            
            m_lbl_price_LanguageText.text = strPrice;

            m_btn_btn_GameButton.onClick.RemoveAllListeners();
            m_btn_btn_GameButton.onClick.AddListener(()=>
            {
                rechargeProxy.CallSdkBuyByPcid(cfgPrice,cfgPrice.rechargeID.ToString(),cfgPrice.price.ToString("N2"));
            });
        }
    }
}