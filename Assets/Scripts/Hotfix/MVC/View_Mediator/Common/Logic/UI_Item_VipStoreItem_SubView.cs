// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_Item_VipStoreItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using PureMVC.Core;

namespace Game {
    public partial class UI_Item_VipStoreItem_SubView : UI_SubView
    {
        public void SetInfo(VipStoreDefine vipStoreDefine,Action<VipStoreDefine,int,Transform> onClick)
        {
            if (vipStoreDefine == null)
            {
                gameObject.SetActive(false);
                return;
            }
        
            gameObject.SetActive(true);
            var itemConfig = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(vipStoreDefine.itemID);
            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            if (itemConfig != null)
            {
                m_pl_item.Refresh(itemConfig, 0, false,true);
                m_lbl_itemName_LanguageText.text = LanguageUtils.getText(itemConfig.l_nameID);
            }

            if (vipStoreDefine.discount > 0)
            {
                m_img_cutOff_PolygonImage.gameObject.SetActive(true);
                m_lbl_cutOff_LanguageText.gameObject.SetActive(true);
                if (vipStoreDefine.discount >= 50)
                {
                    ClientUtils.LoadSprite(m_img_cutOff_PolygonImage,RS.VipStorePurplePriceBg);
                }
                else
                {
                    ClientUtils.LoadSprite(m_img_cutOff_PolygonImage,RS.VipStoreYellowPriceBg);
                }
                m_lbl_cutOff_LanguageText.text = LanguageUtils.getTextFormat(300107, vipStoreDefine.discount);
            }
            else
            {
                m_img_cutOff_PolygonImage.gameObject.SetActive(false);
                m_lbl_cutOff_LanguageText.gameObject.SetActive(false);
            }

            if (playerProxy.VipLevel < vipStoreDefine.vipLevel)
            {
                m_pl_buy_ArabLayoutCompment.gameObject.SetActive(false);
                m_lbl_vipLimit_ArabLayoutCompment.gameObject.SetActive(true);
                m_lbl_vipLimit_LanguageText.text = LanguageUtils.getTextFormat(800120, vipStoreDefine.vipLevel);
                return;
            }
            else
            {
                m_pl_buy_ArabLayoutCompment.gameObject.SetActive(true);
                m_lbl_vipLimit_ArabLayoutCompment.gameObject.SetActive(false);
            }
            
            var vipItemInfo = playerProxy.GetVIPStoreInfo(vipStoreDefine.ID);
            var remainCount = vipStoreDefine.number;
            if (vipItemInfo != null)
            {
                remainCount -= (int)vipItemInfo.count;
            }

            m_lbl_itemCount_LanguageText.text = LanguageUtils.getTextFormat(800121,ClientUtils.FormatComma(remainCount));
            
            var currencyConfig = CoreUtils.dataService.QueryRecord<CurrencyDefine>(vipStoreDefine.type);
            if (currencyConfig != null)
            {
                ClientUtils.LoadSprite(m_btn_buy.m_img_icon_PolygonImage,currencyConfig.iconID);
            }
            m_btn_buy.m_lbl_Text_LanguageText.text = ClientUtils.FormatComma(vipStoreDefine.price);
            m_btn_buy.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();

            if (remainCount > 0)
            {
                m_btn_buy.m_img_forbid_PolygonImage.gameObject.SetActive(false);
                m_btn_buy.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    onClick(vipStoreDefine, 1,m_btn_buy.m_btn_languageButton_GameButton.gameObject.transform);
                });
            }
            else
            {
                m_btn_buy.m_img_forbid_PolygonImage.gameObject.SetActive(true);
            }
        }
    }
}
