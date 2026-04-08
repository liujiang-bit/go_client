// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月27日
// Update Time         :    2020年4月27日
// Class Description   :    UI_Model_MysticStoreItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Model_MysticStoreItem_SubView : UI_SubView
    {
        private MysteryStoreItemData selfData;
        private CurrencyProxy currencyProxy;
        private StoreProxy storeProxy;
        private GrayChildrens makeChildrenGray;

        public GrayChildrens GetMakeChildrenGray()
        {
            if (makeChildrenGray == null)
            {
                makeChildrenGray = m_UI_Model_Item.gameObject.GetComponent<GrayChildrens>();
            }

            return makeChildrenGray;
        }
        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_buy.m_btn_languageButton_GameButton.onClick.AddListener(OnClickBuy);
        }

        private CurrencyProxy GetCurrencyProxy()
        {
            if (currencyProxy == null)
            {
                currencyProxy = AppFacade.GetInstance().RetrieveProxy(Game.CurrencyProxy.ProxyNAME) as CurrencyProxy;
            }

            return currencyProxy;
        }
        private StoreProxy GetStoreProxy()
        {
            if (storeProxy == null)
            {
                storeProxy = AppFacade.GetInstance().RetrieveProxy(Game.StoreProxy.ProxyNAME) as StoreProxy;
            }

            return storeProxy;
        }
        private void OnClickBuy()
        {
            if (selfData.isBuy)
            {
                //提示已经购买过
                return;
            }
            //触发购买流程
            GetStoreProxy().BuyItem_MysteryStore(selfData, delegate
            {
                storeProxy.PlayItemFlyAnim_MysteryStore(selfData.itemTypeId,m_UI_Model_Item.gameObject.GetComponent<RectTransform>());
            });
        }

        public void OnRefreshItem(MysteryStoreItemData data)
        {
            if (data == null)
            {
                gameObject.SetActive(false);
                return;
            }
            //
            // if (selfData != null && selfData.id == data.id)
            // {
            //     selfData = data;
            //     SetShowInfo();
            //     return;
            // }
            //
            selfData = data;
            
            var config = CoreUtils.dataService.QueryRecord<ItemDefine>(data.itemTypeId);
            m_UI_Model_Item.RemoveBtnAllListener();
            m_UI_Model_Item.Refresh(config,data.num,false);
            m_UI_Model_Item.AddBtnListener(() =>
            {
                HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(5000);
                if (tipDefine == null)
                {
                    return;
                }
                HelpTip.CreateTip(LanguageUtils.getTextFormat(tipDefine.l_typeID, LanguageUtils.getText(config.l_nameID), LanguageUtils.getTextFormat(config.l_desID,config.desData1,config.desData2)), m_UI_Model_Item.m_btn_animButton_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(60).Show();                
            });
            
            SetShowInfo();

            
        }

        private void SetShowInfo()
        {
            if (selfData.isBuy)
            {
                m_btn_buy.gameObject.SetActive(false);
                GetMakeChildrenGray()?.Gray();
            }
            else
            {
                m_btn_buy.gameObject.SetActive(true);
                GetMakeChildrenGray()?.Normal();
                ClientUtils.LoadSprite(m_btn_buy.m_img_icon_PolygonImage,GetCurrencyProxy().GeticonIdByType((EnumCurrencyType)selfData.costType));
                m_btn_buy.m_lbl_Text_LanguageText.text = ClientUtils.FormatComma(selfData.costNum);
            }

            m_img_cutBg_PolygonImage.gameObject.SetActive(selfData.discount > 0);
            if(selfData.discount > 0)
            {
                m_lbl_cutOff_LanguageText.text = LanguageUtils.getTextFormat(300107, selfData.discount);
                if (selfData.discount > 50)
                {
                    ClientUtils.LoadSprite(m_img_cutBg_PolygonImage, RS.YellowPriceBg);
                }
                else
                {
                    ClientUtils.LoadSprite(m_img_cutBg_PolygonImage, RS.PurplePriceBg);
                }
            }

        }
    }
}