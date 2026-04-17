// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月18日
// Update Time         :    2020年5月18日
// Class Description   :    UI_Item_ChargeSuperGiftItem_SubView
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
    public partial class UI_Item_ChargeSuperGiftItem_SubView : UI_SubView
    {
        private Data.RechargeSaleDefine m_SuperGiftInfo;
        private List<RewardGroupData> m_rewardGroupData;
        private Timer m_Timer;

        protected override void BindEvent()
        {
            m_btn_buy.m_btn_languageButton_GameButton.onClick.AddListener(OnBuyClickEvent);
        }

        public void InitData(Data.RechargeSaleDefine superGiftInfo)
        {
            m_SuperGiftInfo = superGiftInfo;
            if (m_SuperGiftInfo != null)
            {
                RewardGroupProxy rewardGroupProxy =
                    AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                m_rewardGroupData = rewardGroupProxy.GetRewardDataByGroup(m_SuperGiftInfo.itemPackage);
            }
        }

        public void RefreshUI()
        {
            if (m_SuperGiftInfo == null) return;
            
            m_lbl_name_LanguageText.text = LanguageUtils.getText(m_SuperGiftInfo.l_nameID);
            m_lbl_cutoff_LanguageText.text =
                LanguageUtils.getTextFormat(m_SuperGiftInfo.l_discountID, m_SuperGiftInfo.discountData);
            m_lbl_get_LanguageText.text =
                LanguageUtils.getTextFormat(m_SuperGiftInfo.l_desID1, ClientUtils.FormatComma(m_SuperGiftInfo.desData1));
            m_lbl_dec_LanguageText.text =
                LanguageUtils.getTextFormat(m_SuperGiftInfo.l_desID2, ClientUtils.FormatComma(m_SuperGiftInfo.desData2));

            if (m_SuperGiftInfo.l_labelID != null && m_SuperGiftInfo.l_labelID > 0)
            {
                m_img_bestsell_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_img_bestsell_PolygonImage.gameObject.SetActive(false);
            }

            ClientUtils.LoadSprite(m_img_banner_PolygonImage, m_SuperGiftInfo.background);
            ClientUtils.LoadSprite(m_pl_bg_PolygonImage, m_SuperGiftInfo.baseRes);
            ClientUtils.ImageSetColor(m_img_blet_PolygonImage,m_SuperGiftInfo.bannerColour);
            
            RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            m_lbl_timeLimit_LanguageText.gameObject.SetActive(true);
            m_lbl_limit_LanguageText.gameObject.SetActive(true);
            if (rechargeProxy.isSuperGiftbought(m_SuperGiftInfo.group, m_SuperGiftInfo.price))
            {
                m_btn_buy.gameObject.SetActive(false);
                m_img_sellout_PolygonImage.gameObject.SetActive(true);
                m_lbl_timeLimit_LanguageText.gameObject.SetActive(false);
                m_lbl_limit_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                m_img_sellout_PolygonImage.gameObject.SetActive(false);
                m_btn_buy.gameObject.SetActive(true);
                string strPrice = string.Empty;
                Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(m_SuperGiftInfo.price);
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
                
                if (m_Timer != null)
                {
                    m_Timer.Cancel();
                    m_Timer = null;
                }

                long leftTime = 0;
                switch (m_SuperGiftInfo.giftType)
                {
                    case 1:
                        m_lbl_limit_LanguageText.text = LanguageUtils.getText(762178);
                        m_lbl_timeLimit_LanguageText.gameObject.SetActive(false);
                        break;
                    case 2:
                        m_lbl_limit_LanguageText.text = LanguageUtils.getText(762178);
                        var date = m_SuperGiftInfo.data1.Split('|');
                        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
                        DateTime openTime = new DateTime(int.Parse(date[0]),int.Parse(date[1]),int.Parse(date[2])); 
                        int openTimeSecs = (int)(openTime - dtStart).TotalSeconds;
                        long serverTime = (int)(ServerTimeModule.Instance.GetCurrServerDateTime() - dtStart).TotalSeconds;
                        leftTime = (openTimeSecs + m_SuperGiftInfo.data2) - serverTime ;
                        if (leftTime > 0)
                        {
                            m_Timer = Timer.Register(1,OnTime,null,true,true);
                            m_lbl_timeLimit_LanguageText.text =
                                LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                        }

                        break;
                    case 3:
                        m_lbl_limit_LanguageText.text = LanguageUtils.getText(800083);
                        leftTime = ServerTimeModule.Instance.GetDistanceZeroTime();
                        if (leftTime > 0)
                        {
                            m_Timer = Timer.Register(1,OnTime,null,true,true);
                            m_lbl_timeLimit_LanguageText.text =
                                LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                        }
                        break;
                    case 4:
                        m_lbl_limit_LanguageText.text = LanguageUtils.getText(800083);
                        leftTime = ServerTimeModule.Instance.GetNextSundayTime();
                        if (leftTime > 0)
                        {
                            m_Timer = Timer.Register(1,OnTime,null,true,true);
                            m_lbl_timeLimit_LanguageText.text =
                                LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                        }
                        break;
                    case 5:
                        m_lbl_limit_LanguageText.text = LanguageUtils.getText(800083); ;
                        leftTime = ServerTimeModule.Instance.GetNextMonthOneTime();
                        if (leftTime > 0)
                        {
                            m_Timer = Timer.Register(1,OnTime,null,true,true);
                            m_lbl_timeLimit_LanguageText.text =
                                LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                        }
                        break;
                    case 6:
                        m_lbl_limit_LanguageText.text = LanguageUtils.getText(800083);
                        ActivityProxy activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                        ActivityTimeInfo activityInfo1 = activityProxy.GetActivityById(int.Parse(m_SuperGiftInfo.data1));
                        ActivityTimeInfo activityInfo2 = activityProxy.GetActivityById(m_SuperGiftInfo.data2);
                        leftTime = activityInfo2.endTime - ServerTimeModule.Instance.GetServerTime();
                        if (leftTime > 0)
                        {
                            m_Timer = Timer.Register(1,OnTime,null,true,true);
                            m_lbl_timeLimit_LanguageText.text =
                                LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                        }
                        break;
                }
            }

            m_sv_list_ListView.Clear();
            ClientUtils.PreLoadRes(gameObject, m_sv_list_ListView.ItemPrefabDataList, LoadRewardFinish);
        }
        
        private void OnTime()
        {
            if (m_Timer == null) return;
            long leftTime = 0;
            switch (m_SuperGiftInfo.giftType)
            {
                case 1:
                    break;
                case 2:
                    var date = m_SuperGiftInfo.data1.Split('|');
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
                    DateTime openTime = new DateTime(int.Parse(date[0]),int.Parse(date[1]),int.Parse(date[2])); 
                    int openTimeSecs = (int)(openTime - dtStart).TotalSeconds;
                    long serverTime = (int)(ServerTimeModule.Instance.GetCurrServerDateTime() - dtStart).TotalSeconds;
                    leftTime = (openTimeSecs + m_SuperGiftInfo.data2) - serverTime;
                    if (leftTime > 0)
                    {
                        m_lbl_timeLimit_LanguageText.text =
                            LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                    }
                    else
                    {
                        m_Timer.Cancel();
                        m_Timer = null;
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateSuperGiftInfo);
                    }

                    break;
                case 3:
                    leftTime = ServerTimeModule.Instance.GetDistanceZeroTime();
                    if (leftTime > 0)
                    {
                        m_lbl_timeLimit_LanguageText.text =
                            LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                    }
                    else
                    {
                        m_Timer.Cancel();
                        m_Timer = null;
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateSuperGiftInfo);
                    }
                    break;
                case 4:
                    leftTime = ServerTimeModule.Instance.GetNextSundayTime();
                    if (leftTime > 0)
                    {
                        m_lbl_timeLimit_LanguageText.text =
                            LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                    }
                    else
                    {
                        m_Timer.Cancel();
                        m_Timer = null;
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateSuperGiftInfo);
                    }
                    break;
                case 5:
                    leftTime = ServerTimeModule.Instance.GetNextMonthOneTime();
                    if (leftTime > 0)
                    {
                        m_lbl_timeLimit_LanguageText.text =
                            LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                    }
                    else
                    {
                        m_Timer.Cancel();
                        m_Timer = null;
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateSuperGiftInfo);
                    }
                    break;
                case 6:
                    ActivityProxy activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                    ActivityTimeInfo activityInfo1 = activityProxy.GetActivityById(int.Parse(m_SuperGiftInfo.data1));
                    ActivityTimeInfo activityInfo2 = activityProxy.GetActivityById(m_SuperGiftInfo.data2);
                    leftTime = activityInfo2.endTime - ServerTimeModule.Instance.GetServerTime();
                    if (leftTime > 0)
                    {
                        m_lbl_timeLimit_LanguageText.text =
                            LanguageUtils.getTextFormat(800084, GetCountDownStr(leftTime));
                    }
                    else
                    {
                        m_Timer.Cancel();
                        m_Timer = null;
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateSuperGiftInfo);
                    }
                    break;
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
            UI_Item_ChargeSuperGiftItem2_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_ChargeSuperGiftItem2_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_ChargeSuperGiftItem2_SubView;
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
            if (m_Timer != null)
            {
                m_Timer.Cancel();
                m_Timer = null;
            }
        }

        public void OnBuyClickEvent()
        {
            Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(m_SuperGiftInfo.price);
            if (priceCfg == null) return;
            RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            rechargeProxy.CallSdkBuyByPcid(priceCfg,priceCfg.rechargeID.ToString(),priceCfg.price.ToString("N2"));
        }
        
        private void OnItemBuy(SDKException ex, bool bUserCancel)
        {
            
        }
        
        private string GetCountDownStr(long deltTime)
        {
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
    }
}