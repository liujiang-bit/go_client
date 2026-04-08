// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月20日
// Update Time         :    2020年5月20日
// Class Description   :    UI_Win_GiftLimitedTimeMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using ILRuntime.Runtime;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Win_GiftLimitedTimeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GiftLimitedTimeMediator";
        private LimitTimePackage m_packageInfo;
        private RechargeLimitTimeBagDefine packageDefine;
        private CurrencyProxy m_CurrencyProxy;
        private Timer timer;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GiftLimitedTimeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GiftLimitedTimeView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.UpdateCurrency,
                CmdConstant.RemoveLimitTimePackage,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:
                    RefreshGem();
                    break;
                case CmdConstant.RemoveLimitTimePackage:
                    long packageIndex = notification.Body.ToInt64();
                    if (packageIndex == m_packageInfo.index)
                    {
                        Close();                        
                    }
                    break;
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            if (timer != null)
            {
                timer.Cancel();
                timer = null;
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_CurrencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_packageInfo = view.data as LimitTimePackage;
            if (m_packageInfo != null)
            {
                var packageDefines = CoreUtils.dataService.QueryRecords<Data.RechargeLimitTimeBagDefine>();
                foreach (var define in packageDefines)
                {
                    if (define.price == m_packageInfo.id)
                    {
                        packageDefine = define;
                    }
                }
                Refresh();
                long deltTime = m_packageInfo.expiredTime - ServerTimeModule.Instance.GetServerTime();
                if (deltTime > 0)
                {
                    if (timer != null)
                        timer.Cancel();
                    timer = Timer.Register(deltTime,Close);
                }
            }
            
            
            
        }

        protected override void BindUIEvent()
        {
            view.m_btn_buy.AddClickEvent(OnBuyClickEvent);
            view.m_btn_close_GameButton.onClick.AddListener(Close);
        }

        protected override void BindUIData()
        {
            RefreshGem();
        }
       
        #endregion

        private void Refresh()
        {
            if (packageDefine == null) return;
            
            ClientUtils.LoadSprite(view.m_img_picture_PolygonImage, packageDefine.background);
            view.m_lbl_title_LanguageText.text = LanguageUtils.getText(packageDefine.l_nameID);
            view.m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(packageDefine.l_desID, ClientUtils.FormatComma(packageDefine.desData1),ClientUtils.FormatComma((packageDefine.desData2)));
            
            string strPrice = string.Empty;
            Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(packageDefine.price);
            if (priceCfg != null)
            {
                var gameItems = IGGPayment.shareInstance().GetIGGGameItems();
                if (gameItems != null)
                {
                    IGGGameItem funGameItem = null;
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
            view.m_btn_buy.m_lbl_text_LanguageText.text = strPrice;
            
            RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            if (rechargeProxy.IsFirstRechargeDone())
            {
                view.m_pl_firstcharge.gameObject.SetActive(false);
            }
            else
            {
                view.m_pl_firstcharge.gameObject.SetActive(true);
            }
            
            RewardGroupProxy rewardGroupProxy =
                AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> m_rewardGroupData = rewardGroupProxy.GetRewardDataByGroup(packageDefine.itemPackage);
            for (int i = 0; i < m_rewardGroupData.Count; i++)
            {
                if (i == 0)
                {
                    view.m_UI_Model_Item.RefreshByGroup(m_rewardGroupData[0],3);
                }
                else
                {
                    GameObject modelObject = GameObject.Instantiate(view.m_UI_Model_Item.gameObject, view.m_pl_item_HorizontalLayoutGroup.transform);
                    modelObject.transform.localScale = Vector3.one;
                    var model_Subview = new UI_Model_Item_SubView(modelObject.GetComponent<RectTransform>());
                    model_Subview.RefreshByGroup(m_rewardGroupData[i],3);
                }
            }

        }
        
        private void RefreshGem()
        {
            view.m_UI_Model_Resources.m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(m_CurrencyProxy.Gem);
        }
        
        public void OnBuyClickEvent()
        {
            if (m_packageInfo != null && m_packageInfo.expiredTime <= ServerTimeModule.Instance.GetServerTime())
            {
                Tip.CreateTip(800123).Show();
                Close();
                return;
            }

            if (packageDefine == null) return;
            Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(packageDefine.price);
            if (priceCfg == null) return;
            RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            IGGPaymentPayload payload = new IGGPaymentPayload(IGGSDK.shareInstance().getCharID().ToString(), (int)IGGSDK.shareInstance().getServerID());
            payload.AddParam("index",m_packageInfo.index.ToString());
            rechargeProxy.CallSdkBuyByPcid(priceCfg,priceCfg.rechargeID.ToString(),priceCfg.price.ToString("N2"),payload);
        }
        
        private void OnItemBuy(IGGException ex, bool bUserCancel)
        {
            
        }
        
        private void Close()
        {
            if (timer != null)
            {
                timer.Cancel();
                timer = null;
            }

            CoreUtils.uiManager.CloseUI(UI.s_GiftLimit);
            AppFacade.GetInstance().SendNotification(CmdConstant.LimitTimePackageState, false);
        }
    }
}