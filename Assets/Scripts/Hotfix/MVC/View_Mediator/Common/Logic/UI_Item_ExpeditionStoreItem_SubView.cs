// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Item_ExpeditionStoreItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ExpeditionStoreItem_SubView : UI_SubView
    {
        public int ItemID;
        public void RefreshItem(int expeditionShopId,  bool isSoldOut)
        {
            ItemID = expeditionShopId;
            Data.ExpeditionShopDefine shopCfg = CoreUtils.dataService.QueryRecord<Data.ExpeditionShopDefine>(expeditionShopId);
            if (shopCfg == null) return;
            Data.ItemDefine itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(shopCfg.itemID);
            if (itemCfg == null) return;
            m_UI_Model_Item.Refresh(itemCfg, shopCfg.number, false, true);
            m_UI_Model_Item.SetGray(isSoldOut);
            m_btn_buy.gameObject.SetActive(!isSoldOut);
            m_lbl_soldOut_LanguageText.gameObject.SetActive(isSoldOut);
            if(!isSoldOut)
            {
                m_btn_buy.m_lbl_Text_LanguageText.text = ClientUtils.FormatComma(shopCfg.price);
            }
        }

        public void RefreshHead(int itemId, int price, int avaiableNum = -1)
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(itemId);
            if (itemCfg == null) return;
            m_UI_Model_Item.Refresh(itemCfg, 0, false, true);
            m_btn_buy.m_lbl_Text_LanguageText.text = ClientUtils.FormatComma(price);
            m_lbl_lastNum_LanguageText.gameObject.SetActive(avaiableNum != -1);
            if (avaiableNum != -1)
            {
                m_lbl_lastNum_LanguageText.text = LanguageUtils.getTextFormat(805031, ClientUtils.FormatComma(avaiableNum));
                Color color = avaiableNum == 0 ? Color.gray : Color.white;
                m_UI_Model_Item.m_img_icon_PolygonImage.color = color;
                m_btn_buy.m_img_forbid_PolygonImage.gameObject.SetActive(avaiableNum == 0);
            }
        }

        public void RemoveAllButtonListener()
        {
            m_btn_buy.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
        }

        public void AddBuyButtonListener(UnityAction action)
        {
            m_btn_buy.m_btn_languageButton_GameButton.onClick.AddListener(action);
        }
    }
}