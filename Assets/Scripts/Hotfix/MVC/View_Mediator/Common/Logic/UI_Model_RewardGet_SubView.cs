// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月16日
// Update Time         :    2020年5月16日
// Class Description   :    UI_Model_RewardGet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using Data;

namespace Game {
    public partial class UI_Model_RewardGet_SubView : UI_SubView
    {
        public int ItemData1;
        public object ItemData2;
        public object ItemData3;
        private bool m_isInit;
        private float m_scale = 1f;
        public Action<UI_Model_RewardGet_SubView> BtnClickListener;

        private bool m_isShowClickTip = true;

        public void SetScale(float scale)
        {
            m_scale = scale;
        }

        public void SetShowClickTip(bool isShow)
        {
            m_isShowClickTip = isShow;
        }

        public void GuildGift(string icon)
        {
            m_btn_animButton_GameButton.onClick.RemoveAllListeners();
            m_UI_Model_Item.gameObject.SetActive(false);
            m_lbl_name_LanguageText.text = "";
            m_pl_cur_ViewBinder.gameObject.SetActive(false);
            m_pl_package_ViewBinder.gameObject.SetActive(true);
            m_pl_soldier_ViewBinder.gameObject.SetActive(false);
            
            ClientUtils.LoadSprite(m_img_packageicon_PolygonImage,icon);
            m_lbl_packagecount_LanguageText.text = "";

        }

        /// <summary>
        /// 奖励组显示 界面来源sourceType： 1活动日历 2达标+排行类型 排行奖励 
        /// </summary>
        public void RefreshByGroup(RewardGroupData rewardGroupData, int sourceType = 1, bool isShowName = false)
        {
            m_btn_animButton_GameButton.onClick.RemoveAllListeners();
            m_btn_animButton_GameButton.onClick.AddListener(RewardGroupBtnAddEvent);
            ItemData1 = sourceType;
            ItemData2 = rewardGroupData;
            switch ((EnumRewardType)rewardGroupData.RewardType)
            {
                case EnumRewardType.Currency: //货币 （包含木头粮食石头金币砖石）
                    m_UI_Model_Item.gameObject.SetActive(false);
                    m_pl_soldier_ViewBinder.gameObject.SetActive(false);
                    m_pl_package_ViewBinder.gameObject.SetActive(false);
                    m_pl_cur_ViewBinder.gameObject.SetActive(true);
                    if (sourceType == 3)
                    {
                        Refresh(rewardGroupData.CurrencyData.currencyDefine, rewardGroupData.number, isShowName);
                    }
                    else if (sourceType == 4)
                    {
                        Refresh(rewardGroupData.CurrencyData.currencyDefine, rewardGroupData.number, isShowName, true);
                    }
                    else
                    {
                        Refresh(rewardGroupData.CurrencyData.currencyDefine, 0, isShowName);
                    }
                    break;
                case EnumRewardType.Soldier: //士兵
                    if (sourceType == 3)
                    {
                        RefreshSoldier(rewardGroupData.SoldierData.armsDefine, rewardGroupData.number, isShowName);
                    }
                    else if (sourceType == 4)
                    {
                        RefreshSoldier(rewardGroupData.SoldierData.armsDefine, rewardGroupData.number, isShowName);
                    }
                    else
                    {
                        RefreshSoldier(rewardGroupData.SoldierData.armsDefine, 0, isShowName);
                    }
                    break;
                case EnumRewardType.Item: //道具
                    m_UI_Model_Item.gameObject.SetActive(true);
                    m_pl_soldier_ViewBinder.gameObject.SetActive(false);
                    m_pl_package_ViewBinder.gameObject.SetActive(false);
                    m_pl_cur_ViewBinder.gameObject.SetActive(false);
                    if (sourceType == 3)
                    {
                        m_UI_Model_Item.Refresh(rewardGroupData.ItemData.itemDefine, rewardGroupData.number, false);
                    }
                    else if (sourceType == 4)
                    {
                        m_UI_Model_Item.Refresh(rewardGroupData.ItemData.itemDefine, rewardGroupData.number, false);
                    }
                    else
                    {
                        m_UI_Model_Item.Refresh(rewardGroupData.ItemData.itemDefine, 0, false);
                    }
                    m_lbl_name_LanguageText.gameObject.SetActive(isShowName);
                    if (isShowName)
                    {
                        m_lbl_name_LanguageText.text = LanguageUtils.getText(rewardGroupData.name);
                    }
                    break;
                case EnumRewardType.AllianceGift: //礼包
                    m_UI_Model_Item.gameObject.SetActive(false);
                    m_pl_soldier_ViewBinder.gameObject.SetActive(false);
                    m_pl_package_ViewBinder.gameObject.SetActive(true);
                    m_pl_cur_ViewBinder.gameObject.SetActive(false);
                    if (sourceType == 3)
                    {
                        Refresh(rewardGroupData.AllianceGiftData, rewardGroupData.number, isShowName);
                    }
                    else if (sourceType == 4)
                    {
                        Refresh(rewardGroupData.AllianceGiftData, rewardGroupData.number, isShowName);
                    }
                    else
                    {
                        Refresh(rewardGroupData.AllianceGiftData, 0, isShowName);
                    }
                    break;
                default:
                    Debug.LogError("not find type");
                    break;
            }
        }

        public void RefreshItem(ItemDefine itemDefine, Int64 overlay, bool isShowName = false)
        {
            if (!m_isInit)
            {
                m_btn_animButton_GameButton.onClick.AddListener(ItemBtnAddEvent);
                m_isInit = true;
            }
            ItemData2 = itemDefine;
            m_UI_Model_Item.gameObject.SetActive(true);
            m_pl_soldier_ViewBinder.gameObject.SetActive(false);
            m_pl_package_ViewBinder.gameObject.SetActive(false);
            m_pl_cur_ViewBinder.gameObject.SetActive(false);

            m_lbl_name_LanguageText.gameObject.SetActive(isShowName);
            m_UI_Model_Item.Refresh(itemDefine, overlay, false, false);
        }

        public void RefreshSoldier(ArmsDefine armsDefine, Int64 overlay, bool isShowName = false)
        {
            m_UI_Model_Item.gameObject.SetActive(false);
            m_pl_soldier_ViewBinder.gameObject.SetActive(true);
            m_pl_package_ViewBinder.gameObject.SetActive(false);
            m_pl_cur_ViewBinder.gameObject.SetActive(false);
            Refresh(armsDefine,overlay,isShowName);
        }

        private void ItemBtnAddEvent()
        {
            if (!m_isShowClickTip)
            {
                return;
            }
            float offset = m_btn_animButton_GameButton.GetComponent<RectTransform>().rect.height / 2;
            if (m_scale != 1f)
            {
                offset = offset * m_scale;
            }

            ItemDefine define = ItemData2 as ItemDefine;
            string descFormat = string.Format(LanguageUtils.getText(define.l_desID), ClientUtils.FormatComma(define.desData1), ClientUtils.FormatComma(define.desData2));
            HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(5000);
            string str = LanguageUtils.getTextFormat(tipDefine.l_typeID,
                                                     LanguageUtils.getText(define.l_nameID),
                                                     descFormat);
            HelpTip.CreateTip(str, m_UI_Model_Item.m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
        }


        public void Refresh(CurrencyDefine currencyDefine, Int64 overlay, bool isShowName = false, bool currencyFormat = false)
        {
            m_btn_animButton_GameButton.onClick.RemoveAllListeners();
            m_btn_animButton_GameButton.onClick.AddListener(() =>
            {
                if (!m_isShowClickTip)
                {
                    return;
                }
                float offset = m_btn_animButton_GameButton.GetComponent<RectTransform>().rect.height / 2;
                if (m_scale != 1f)
                {
                    offset = offset * m_scale;
                }
                HelpTip.CreateTip(LanguageUtils.getText(currencyDefine.l_desID),
                                    m_img_curicon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
            });
            m_UI_Model_Item.gameObject.SetActive(false);
            m_pl_soldier_ViewBinder.gameObject.SetActive(false);
            m_pl_package_ViewBinder.gameObject.SetActive(false);
            m_pl_cur_ViewBinder.gameObject.SetActive(true);
            if (overlay < 1)
            {
                m_lbl_curcount_LanguageText.text = "";
            }
            else
            {
                if (currencyFormat)
                {
                    m_lbl_curcount_LanguageText.text = ClientUtils.CurrencyFormat(overlay);
                }
                else
                {
                    m_lbl_curcount_LanguageText.text = ClientUtils.FormatComma(overlay);
                }

            }
            //设置icon
            ClientUtils.LoadSprite(m_img_curicon_PolygonImage, currencyDefine.iconID);

            m_lbl_name_LanguageText.gameObject.SetActive(isShowName);
            if (isShowName)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(currencyDefine.l_desID);
            }
        }

        public void Refresh(ArmsDefine armsDefine, Int64 overlay, bool isShowName = false)
        {
            //设置icon
            ClientUtils.LoadSprite(m_img_soldiericon_PolygonImage, armsDefine.icon);

            if (overlay < 1)
            {
                m_lbl_soldiercount_LanguageText.text = "";
            }
            else
            {
                m_lbl_soldiercount_LanguageText.text = ClientUtils.FormatComma(overlay);
            }

            m_lbl_name_LanguageText.gameObject.SetActive(isShowName);
            if (isShowName)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(armsDefine.l_armsID);
            }
        }

        public void RefreshReward(ItemPackageDefine reward)
        {
            switch ((EnumRewardType)reward.type)
            {
                case EnumRewardType.Currency:
                    var currencyCfg = CoreUtils.dataService.QueryRecord<CurrencyDefine>(reward.typeData);
                    if(currencyCfg != null)
                    {
                        Refresh(currencyCfg, reward.number);
                    }
                    break;
                case EnumRewardType.Item:
                    var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(reward.typeData);
                    if (itemCfg != null)
                    {
                        RefreshItem(itemCfg, reward.number);
                    }
                    break;
                case EnumRewardType.Soldier:
                    var armysCfg = CoreUtils.dataService.QueryRecord<ArmsDefine>(reward.typeData);
                    if(armysCfg != null)
                    {
                        RefreshSoldier(armysCfg, reward.number);
                    }
                    break;
            }
        }

        public void RefreshReward(ItemDefine itemDefine, Int64 overlay,int nameType = 1)
        {
            m_UI_Model_Item.Refresh(itemDefine, overlay, false, true);
            if (nameType == 1)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(itemDefine.l_tipsID);
            }
            else if (nameType == 2)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
            }
            else
            {
                
            }
            m_lbl_name_LanguageText.gameObject.SetActive(true);

        }

        public void RefreshReward(CurrencyDefine currencyDefine, Int64 overlay)
        {
            if (overlay < 1)
            {
                m_lbl_curcount_LanguageText.text = "";
            }
            else
            {
                m_lbl_curcount_LanguageText.text = ClientUtils.FormatComma(overlay);
            }
            //设置icon
            ClientUtils.LoadSprite(m_img_curicon_PolygonImage, currencyDefine.iconID);
            m_lbl_name_LanguageText.gameObject.SetActive(true);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(currencyDefine.l_desID);
        }

        public void RefreshReward(ArmsDefine armsDefine, Int64 overlay)
        {
            //设置icon
            ClientUtils.LoadSprite(m_img_soldiericon_PolygonImage, armsDefine.icon);

            if (overlay < 1)
            {
                m_lbl_soldiercount_LanguageText.text = "";
            }
            else
            {
                m_lbl_soldiercount_LanguageText.text = ClientUtils.FormatComma(overlay);
            }
            m_lbl_name_LanguageText.gameObject.SetActive(true);

            m_lbl_name_LanguageText.text = LanguageUtils.getText(armsDefine.l_armsID);
        }
        public void RefreshReward(HeroDefine heroDefine, Int64 overlay)
        {
            //设置icon
            ClientUtils.LoadSprite(m_img_packageicon_PolygonImage, heroDefine.heroIcon);

            if (overlay < 1)
            {
                m_lbl_packagecount_LanguageText.text = "";
            }
            else
            {
                m_lbl_packagecount_LanguageText.text = ClientUtils.FormatComma(overlay);
            }
            m_lbl_name_LanguageText.gameObject.SetActive(true);

            m_lbl_name_LanguageText.text = LanguageUtils.getText(heroDefine.l_nameID);
        }

        public void Refresh(RewardAllianceGift giftData, Int64 overlay, bool isShowName = false)
        {
            //设置icon
            ClientUtils.LoadSprite(m_img_packageicon_PolygonImage, giftData.iconImg);

            if (overlay < 1)
            {
                m_lbl_packagecount_LanguageText.text = "";
            }
            else
            {
                m_lbl_packagecount_LanguageText.text = ClientUtils.FormatComma(overlay);
            }

            m_lbl_name_LanguageText.gameObject.SetActive(isShowName);
            if (isShowName)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(giftData.l_desc);
            }
        }

        public void RefreshRewardInfo(SprotoType.RewardInfo rewardInfo)
        {
            if(rewardInfo.HasFood)
            {
                CurrencyDefine cfg = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)EnumCurrencyType.food);
                Refresh(cfg, rewardInfo.food);
                return;
            }
            if (rewardInfo.HasWood)
            {
                CurrencyDefine cfg = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)EnumCurrencyType.wood);
                Refresh(cfg, rewardInfo.wood);
                return;
            }
            if (rewardInfo.HasStone)
            {
                CurrencyDefine cfg = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)EnumCurrencyType.stone);
                Refresh(cfg, rewardInfo.stone);
                return;
            }
            if (rewardInfo.HasGold)
            {
                CurrencyDefine cfg = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)EnumCurrencyType.gold);
                Refresh(cfg, rewardInfo.gold);
                return;
            }
            if (rewardInfo.HasDenar)
            {
                CurrencyDefine cfg = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)EnumCurrencyType.denar);
                Refresh(cfg, rewardInfo.denar);
                return;
            }
            if (rewardInfo.HasExpeditionCoin)
            {
                CurrencyDefine cfg = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)EnumCurrencyType.conquerorMedal);
                Refresh(cfg, rewardInfo.expeditionCoin);
                return;
            }
            if(rewardInfo.HasItems)
            {
                var item = rewardInfo.items[0];
                ItemDefine cfg = CoreUtils.dataService.QueryRecord<ItemDefine>((int)item.itemId);
                RefreshItem(cfg, (int)item.itemNum);
                return;
            }
        }

        private void RewardGroupBtnAddEvent()
        {
            float offset = m_btn_animButton_GameButton.GetComponent<RectTransform>().rect.height/2;
            if (m_scale != 1f)
            {
                offset = offset * m_scale;
            }
            //if (ItemData1 == 2)
            //{
            //    offset = 10;
            //}
            RewardGroupData rewardGroupData = ItemData2 as RewardGroupData;
            if (rewardGroupData == null)
            {
                return;
            }
            switch ((EnumRewardType)rewardGroupData.RewardType)
            {
                case EnumRewardType.Currency:

                    HelpTip.CreateTip(LanguageUtils.getText(rewardGroupData.CurrencyData.currencyDefine.l_desID),
                                      m_img_curicon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                    break;
                case EnumRewardType.Soldier:
                    HelpTip.CreateTip(LanguageUtils.getText(rewardGroupData.SoldierData.armsDefine.l_armsID),
                                      m_img_soldiericon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                    break;
                case EnumRewardType.Item:
                    HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(5000);
                    string str = LanguageUtils.getTextFormat(tipDefine.l_typeID,
                                                             LanguageUtils.getText(rewardGroupData.ItemData.l_nameID),
                                                             rewardGroupData.ItemData.descFormat);
                    HelpTip.CreateTip(str, m_UI_Model_Item.m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                    break;
                case EnumRewardType.AllianceGift:
                    HelpTipsDefine tipDefine_gift = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(5000);
                    string str_gift = LanguageUtils.getTextFormat(tipDefine_gift.l_typeID,
                        LanguageUtils.getText(rewardGroupData.name),
                        LanguageUtils.getText(rewardGroupData.AllianceGiftData.l_desc));
                    HelpTip.CreateTip(str_gift, m_UI_Model_Item.m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                    break;
                default:
                    Debug.LogError("not find type");
                    break;
            }

            if (BtnClickListener != null)
            {
                BtnClickListener(this);
            }
        }
    }
}