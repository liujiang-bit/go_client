// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_ChargeCitySuppyItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ChargeCitySuppyItem_SubView : UI_SubView
    {

        private Data.RechargeSupplyDefine m_SupplyInfo;
        private Supply m_RewardInfo;
        private Timer m_Timer;

        public Data.RechargeSupplyDefine SupplyDefine
        {
            get { return m_SupplyInfo; }
        }

        protected override void BindEvent()
        {
            m_btn_buy.AddClickEvent(OnBuyClickEvent);
            m_btn_get.AddClickEvent(OnRewardClickEvent);
        }

        public void InitData(Data.RechargeSupplyDefine supplyInfo,Supply rewardInfo)
        {
            m_SupplyInfo = supplyInfo;
            m_RewardInfo = rewardInfo;
        }

        public void RefreshUI()
        {
            if (m_SupplyInfo == null)
                return;
            
            ClientUtils.LoadSprite(m_img_card_PolygonImage, m_SupplyInfo.background);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(m_SupplyInfo.l_nameID);
            m_lbl_get_LanguageText.text = LanguageUtils.getTextFormat(800070, ClientUtils.FormatComma(m_SupplyInfo.giveGem));
            m_lbl_dec_LanguageText.text = LanguageUtils.getTextFormat(m_SupplyInfo.l_desID, ClientUtils.FormatComma(m_SupplyInfo.desData));

            if (m_RewardInfo != null && m_RewardInfo.expiredTime > ServerTimeModule.Instance.GetServerTime())
            {
                //今日已领取
                if (m_RewardInfo.award)
                {
                    m_btn_get.gameObject.SetActive(false);
                    m_btn_buy.gameObject.SetActive(true);
                    m_btn_buy.m_lbl_Text_LanguageText.text = LanguageUtils.getText(800108);
                }
                else
                {
                    m_btn_get.gameObject.SetActive(true);
                    m_btn_get.m_img_redpoint_PolygonImage.gameObject.SetActive(true);
                    m_btn_buy.gameObject.SetActive(false);
                    m_btn_buy.m_lbl_Text_LanguageText.text = LanguageUtils.getText(570008);
                }

                m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762019,GetCountDownStr());
                
                if (m_Timer != null)
                {
                    m_Timer.Cancel();
                    m_Timer = null;
                }
                m_Timer = Timer.Register(1,OnTime,null,true,true);
                
            }
            else
            {
                string strPrice = string.Empty;
                Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(m_SupplyInfo.price);
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

                m_btn_get.gameObject.SetActive(false);
                m_btn_buy.gameObject.SetActive(true);
                m_btn_buy.m_lbl_Text_LanguageText.text = strPrice;
                m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(800103, m_SupplyInfo.continueDays);
            }

        }

        public void AddTipClickListener(UnityAction listener)
        {
            m_btn_tips_GameButton.onClick.AddListener(listener);
        }

        /*
         * 获得剩余时间
         */
        private string GetCountDownStr()
        {
            long deltTime = m_RewardInfo.expiredTime - ServerTimeModule.Instance.GetServerTime();
            long day = deltTime / 86400;
            long hour = (deltTime % 86400) / 3600;
            long min = ((deltTime % 86400) % 3600) / 60;
            long sec = ((deltTime % 86400) % 3600) % 60;
            
            if (day > 0)
            {
                return LanguageUtils.getTextFormat(800109, day.ToString("00"),hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00"));
            }
            else
            {
                return hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00");
            }
        }

        public void OnItemDestroy()
        {
            if (m_Timer != null)
            {
                m_Timer.Cancel();
                m_Timer = null;
            }
        }

        private void OnTime()
        {
            if (m_RewardInfo == null || m_RewardInfo.expiredTime < ServerTimeModule.Instance.GetServerTime())
            {
                
                m_Timer.Cancel();
                m_Timer = null;
                AppFacade.GetInstance().SendNotification(CmdConstant.UpdateSupplyInfo);
                return;
            }
            m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(762019,GetCountDownStr());
            
        }

        
        public void OnBuyClickEvent()
        {
            Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(m_SupplyInfo.price);
            if (priceCfg == null) return;
            RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            rechargeProxy.CallSdkBuyByPcid( priceCfg, priceCfg.rechargeID.ToString(),priceCfg.price.ToString("N2"));
        }
        
        private void OnItemBuy(SDKException ex, bool bUserCancel)
        {
            
        }

        public void OnRewardClickEvent()
        {
            Recharge_AwardRechargeSupply.request request = new Recharge_AwardRechargeSupply.request()
            {
                id = m_SupplyInfo.price
            };
            
            AppFacade.GetInstance().SendSproto(request);
        }

    }
}