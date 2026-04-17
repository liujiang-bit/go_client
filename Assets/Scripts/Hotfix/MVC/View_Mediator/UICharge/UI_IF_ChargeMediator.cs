// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_IF_ChargeMediator
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
using PureMVC.Interfaces;
using SprotoType;

namespace Game {

    public class UI_IF_ChargeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_ChargeMediator";

        private CurrencyProxy m_CurrencyProxy;
        private RechargeProxy m_RechargeProxy;

        private List<RechargeListDefine> m_ItemCfg;
        private Dictionary<string, GameObject> m_assetDic;
        private int m_curIsOnToggle_PagingType = -1;
        private Dictionary<EnumRechargeListPageType, GameObject> m_DicPageGo;
        #endregion

        //IMediatorPlug needs
        public UI_IF_ChargeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_ChargeView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.UpdateCurrency,
                CmdConstant.ChargeListToggleChanged,
                CmdConstant.JumpToChargeListByPageType,
                CmdConstant.OnServerCallbackChangedRecharge,
                CmdConstant.OnServerCallbackChangedRiseRoad,
                CmdConstant.BuyItemCallBack,
                Recharge_GetGrowthFundReward.TagName,
                Recharge_AwardRechargeSupply.TagName,
                Recharge_AwardRisePackage.TagName,
                Role_GetFreeDaily.TagName,
                CmdConstant.GrowthFundChange
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:
                    RefreshGem();
                    break;
                case CmdConstant.ChargeListToggleChanged:
                    OnToggleChanged(notification.Body is int ? (int) notification.Body : -1);
                    break;
                case CmdConstant.JumpToChargeListByPageType:
                    JumpToPage(notification.Body is int ? notification.Body : -1);
                    break;
                case Recharge_GetGrowthFundReward.TagName:
                    {
                        var response = notification.Body as Recharge_GetGrowthFundReward.response;
                        if (response == null || response.denar == 0) return;
                        if (view.m_UI_Item_ChargeGrowing.gameObject.activeSelf)
                        {
                            view.m_UI_Item_ChargeGrowing.OnGetGrowthFunRewardSuccess();
                        }
                        RefreshToggleListItem();
                    }
                    break;
                case CmdConstant.GrowthFundChange:
                    {
                        if (view.m_UI_Item_ChargeGrowing.gameObject.activeSelf)
                        {
                            view.m_UI_Item_ChargeGrowing.Show(view.m_img_gem.m_img_icon_PolygonImage.transform.position);
                        }
                        RefreshToggleListItem();
                    }
                    break;
                case Recharge_AwardRechargeSupply.TagName:
                    {
                        RefreshToggleListItem();
                    }
                    break;
                case Role_GetFreeDaily.TagName:
                {
                    RefreshToggleListItem();
                    ShowFreeDailyBoxReward(notification.Body as Role_GetFreeDaily.response);
                }
                    break;
                case CmdConstant.OnServerCallbackChangedRecharge:
                    OnServerCallbackChangedRecharge();
                    break;
                case CmdConstant.OnServerCallbackChangedRiseRoad:
                    OnServerCallbackChangedRiseRoad();
                    break;
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.OpenMall));
        }

        public override void WinClose()
        {
            m_curIsOnToggle_PagingType = -1;
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_CurrencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_RechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;

            if (view.data != null)
            {
                m_curIsOnToggle_PagingType = (int)view.data;
            }

            InitItemCfgIds();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Interface.m_btn_back_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_Charge);
            });
            view.m_btn_service_GameButton.onClick.AddListener(OnServiceEvent);
        }

        protected override void BindUIData()
        {
            RefreshGem();
            ClientUtils.PreLoadRes(view.gameObject,view.m_sv_list_ListView.ItemPrefabDataList,OnItemPrefabLoadFinish);
            if (m_DicPageGo == null)
                m_DicPageGo = new Dictionary<EnumRechargeListPageType, GameObject>();
            m_DicPageGo.Clear();
            m_DicPageGo[EnumRechargeListPageType.ChargeFirst] = view.m_UI_Item_ChargeFirst.gameObject;
            m_DicPageGo[EnumRechargeListPageType.ChargeRiseRoad] = view.m_UI_Item_ChargeRiseRoad.gameObject;
            m_DicPageGo[EnumRechargeListPageType.ChargeSuperGift] = view.m_UI_Item_ChargeSuperGift.gameObject;
            m_DicPageGo[EnumRechargeListPageType.ChargeDayCheap] = view.m_UI_Item_ChargeDayCheap.gameObject;
            m_DicPageGo[EnumRechargeListPageType.ChargeCitySupply] = view.m_UI_Item_ChargeCitySupply.gameObject;
            m_DicPageGo[EnumRechargeListPageType.ChargeGrowing] = view.m_UI_Item_ChargeGrowing.gameObject;
            m_DicPageGo[EnumRechargeListPageType.ChargeGemShop] = view.m_UI_Item_ChargeGemShop.gameObject;
        }
       
        #endregion

        private void RefreshGem()
        {
            view.m_img_gem.m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(m_CurrencyProxy.Gem);
        }

        private void RefreshToggleListItem()
        {
            view.m_sv_list_ListView.ForceRefresh();
            SendNotification(CmdConstant.UpdateRechargeReddot);
        }


        private void OnToggleChanged(int cfgId)
        {
            var cfgData = CoreUtils.dataService.QueryRecord<Data.RechargeListDefine>(cfgId);
            m_curIsOnToggle_PagingType = cfgData.pagingType;
            //TODO 显示与隐藏相关界面
            //            Debug.Log("显示" + cfgId);
            foreach (var v in m_DicPageGo)
            {
                v.Value.SetActive((int) v.Key == m_curIsOnToggle_PagingType);
            }
            // if(!Enum.IsDefined(typeof(EnumRechargeListPageType),  m_curIsOnToggle_PagingType))
            // {
            //     return;
            // }
            switch ((EnumRechargeListPageType)m_curIsOnToggle_PagingType)
            {
                case EnumRechargeListPageType.ChargeDayCheap:
                {
                    view.m_UI_Item_ChargeDayCheap.Show();
                }
                    break;
                case EnumRechargeListPageType.ChargeSuperGift:
                {
                    view.m_UI_Item_ChargeSuperGift.Show();
                }
                    break;
                case EnumRechargeListPageType.ChargeCitySupply:
                {
                    view.m_UI_Item_ChargeCitySupply.Show();
                }
                    break;
                case EnumRechargeListPageType.ChargeGrowing:
                    {
                        view.m_UI_Item_ChargeGrowing.Show(view.m_img_gem.m_img_icon_PolygonImage.transform.position);
                        TipRemindProxy.SaveRemind("GrowingFuncRemind");
                    }
                    break;
            }  
            RefreshToggleListItem();
            AppFacade.GetInstance().SendNotification(CmdConstant.PageClick);
        }

        private int GetItemIndex(EnumRechargeListPageType pageType)
        {
            for (int i = 0; i < m_ItemCfg.Count; ++i)
            {
                if (m_ItemCfg[i].pagingType == (int)pageType)
                {
                    return i;
                }
            }
            return 0;
        }

        private void InitItemCfgIds()
        {
            if (m_ItemCfg == null)
                m_ItemCfg = new List<RechargeListDefine>();
            m_ItemCfg.Clear();
            var result = m_RechargeProxy.GetRechargeListItemCfgIds();
            m_ItemCfg.AddRange(result);
        }

        private void OnItemPrefabLoadFinish(Dictionary<string,GameObject> assetDic)
        {
            m_assetDic = assetDic;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            
            view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list_ListView.FillContent(m_ItemCfg.Count != null ? m_ItemCfg.Count : 0);
        }

        private void ItemEnter(ListView.ListItem scrollItem)
        {
            if (scrollItem == null || m_ItemCfg == null || scrollItem.index >= m_ItemCfg.Count) return;
            var cfgId = m_ItemCfg[scrollItem.index].ID;
            UI_Item_ChargeListItem_SubView itemView = null;
            if (scrollItem.data == null)
            {
                itemView = new UI_Item_ChargeListItem_SubView(scrollItem.go.GetComponent<RectTransform>());
                scrollItem.data = itemView;
            }
            else
            {
                itemView = scrollItem.data as UI_Item_ChargeListItem_SubView;
            }
            if(itemView == null) return;
            var cfgData = CoreUtils.dataService.QueryRecord<Data.RechargeListDefine>(cfgId);
            itemView.Refresh(cfgId, GetRedDotCount((EnumRechargeListPageType)cfgData.pagingType));
            itemView.m_ck_type_GameToggle.group = view.m_c_list_view_SideBtns_ToggleGroup;          
            if (m_curIsOnToggle_PagingType == -1)
            {
                m_curIsOnToggle_PagingType = cfgData.pagingType; 
            }
            if (cfgData.pagingType == m_curIsOnToggle_PagingType)
            {
                itemView.m_ck_type_GameToggle.isOn = true;
                itemView.m_lbl_name_LanguageText.color = new Color(106/255.0f,103/255.0f,88/255.0f);
            }
            else
            {
                itemView.m_ck_type_GameToggle.isOn = false;
                itemView.m_lbl_name_LanguageText.color = Color.white;
            }
        }

        private int GetRedDotCount(EnumRechargeListPageType pageType)
        {
            int redDotCount = 0;
            switch (pageType)
            {
                case EnumRechargeListPageType.ChargeCitySupply:
                {
                    redDotCount = m_RechargeProxy.GetCitySupplyReddotCount();
                }
                    break;
                case EnumRechargeListPageType.ChargeGrowing:
                {
                    redDotCount = m_RechargeProxy.GetGrowingReddotCount();
                }
                    break;
                case EnumRechargeListPageType.ChargeRiseRoad:
                {
                    redDotCount = m_RechargeProxy.GetRiseRoadReddotCount();
                }
                    break;
                case EnumRechargeListPageType.ChargeDayCheap:
                {
                    redDotCount = m_RechargeProxy.GetDayCheapReddotCount();
                }
                    break;
                default:
                    break;
            }
            return redDotCount;
        }

        private void OnServerCallbackChangedRiseRoad()
        {
            if (m_RechargeProxy.TryGetCurRiseRoadCfg(out var data))
            {
                view.m_UI_Item_ChargeRiseRoad.Refresh();
                for (int i = 0; i < m_ItemCfg.Count; i++)
                {
                    if (m_ItemCfg[i].pagingType == m_curIsOnToggle_PagingType)
                    {
                        var item = view.m_sv_list_ListView.GetItemByIndex(i);
                        if (item != null)
                        {
                            UI_Item_ChargeListItem_SubView subView = (UI_Item_ChargeListItem_SubView)item.data;
                            subView.Refresh(m_ItemCfg[i].ID,GetRedDotCount((EnumRechargeListPageType)m_ItemCfg[i].pagingType));
                        }
                    }
                }
            }
            else
            {
                view.m_UI_Item_ChargeFirst.gameObject.SetActive(false);
                view.m_sv_list_ListView.FillContent(0);
                view.m_sv_list_ListView.ForceRefresh();
                Timer.Register(0.05f, () =>
                {
                    var pageType = (int)EnumRechargeListPageType.ChargeGemShop;
                    var cfgDatas = CoreUtils.dataService.QueryRecords<Data.RechargeFirstDefine>();
                    m_RechargeProxy.TryGetJumpToPageType(cfgDatas[cfgDatas.Count-1].jumpType,ref pageType);
                    m_curIsOnToggle_PagingType = pageType;
                    InitItemCfgIds();
                    view.m_sv_list_ListView.FillContent(m_ItemCfg.Count != null ? m_ItemCfg.Count : 0);
                    view.m_sv_list_ListView.ForceRefresh();
                    // foreach (var v in m_DicPageGo)
                    // {
                    //     v.Value.SetActive((int) v.Key == m_curIsOnToggle_PagingType);
                    // }
                    AppFacade.GetInstance().SendNotification(CmdConstant.JumpToChargeListByPageType, pageType);
                });
            }
        }
        private void OnServerCallbackChangedRecharge()
        {
            RefreshGem();
            if (view.m_UI_Item_ChargeGemShop.gameObject.activeSelf || m_curIsOnToggle_PagingType == (int)EnumRechargeListPageType.ChargeGemShop)
            {
                view.m_UI_Item_ChargeGemShop.RefreshListView();
            }
            if (view.m_UI_Item_ChargeRiseRoad.gameObject.activeSelf || m_curIsOnToggle_PagingType == (int)EnumRechargeListPageType.ChargeRiseRoad)
            {
                view.m_UI_Item_ChargeRiseRoad.Refresh();
            }
            view.m_sv_list_ListView.ForceRefresh();
            CheckRechargeFirst();
        }
        /**
         * 如果完成首充则切换到崛起之路
         */
        public void CheckRechargeFirst()
        {
            if (view.m_UI_Item_ChargeFirst.gameObject.activeSelf )//&& m_RechargeProxy.IsFirstRechargeDone())
            {
                view.m_UI_Item_ChargeFirst.gameObject.SetActive(false);
                view.m_sv_list_ListView.FillContent(0);
                view.m_sv_list_ListView.ForceRefresh();
                Timer.Register(0.05f, () =>
                {
                    m_curIsOnToggle_PagingType = (int)EnumRechargeListPageType.ChargeRiseRoad;
                    InitItemCfgIds();
                    view.m_sv_list_ListView.FillContent(m_ItemCfg.Count != null ? m_ItemCfg.Count : 0);
                    view.m_sv_list_ListView.ForceRefresh();
                    foreach (var v in m_DicPageGo)
                    {
                        v.Value.SetActive((int) v.Key == m_curIsOnToggle_PagingType);
                    }
                });
            }
        }

        public void JumpToPage(object pageType)
        {
            m_curIsOnToggle_PagingType = (int)pageType;
            Debug.Log("JumpToPage" + m_curIsOnToggle_PagingType);
            var index = 0;
            for (int i = 0; i < m_ItemCfg.Count; i++)
            {
                if (m_ItemCfg[i].pagingType == m_curIsOnToggle_PagingType)
                {
                    index = i;
                    break;
                }
            }
            var item = view.m_sv_list_ListView.GetItemByIndex(index);
            if (item != null)
            {
                UI_Item_ChargeListItem_SubView subView = (UI_Item_ChargeListItem_SubView)item.data;
                subView.m_ck_type_GameToggle.isOn = true;
            }
            OnToggleChanged(m_ItemCfg[index].ID);
        }

        private void ShowFreeDailyBoxReward(Role_GetFreeDaily.response info)
        {
            if (info == null || info.rewardInfo == null)
            {
                return;
            }
            
            view.m_UI_Item_ChargeDayCheap.ShowRewardItemFly(info.rewardInfo);
        }

        private void OnServiceEvent()
        {
            //暂时只有问题提交
            SDKURLBundle.shareInstance().serviceURL((exception, url) =>
            {
                if (exception.isNone())
                {
                    SDKUtils.shareInstance().OpenBrowser(url);
                }
            });
        }
    }
}