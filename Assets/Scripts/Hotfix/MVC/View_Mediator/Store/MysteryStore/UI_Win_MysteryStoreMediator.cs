// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月27日
// Update Time         :    2020年4月27日
// Class Description   :    UI_Win_MysteryStoreMediator
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
using UnityEngine.UI;

namespace Game {
    public class UI_Win_MysteryStoreMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_MysteryStoreMediator";

        private StoreProxy storeProxy;
        private PlayerProxy playerProxy;
        private CurrencyProxy currencyProxy;
        
        private List<MysteryStoreItemGroupData> itemData;
        private Timer mysteryStoreTimer;
        private Alert alert;
        private bool bIsOnOpen;
        
        private GrayChildrens makeChildrenGray;

        #endregion

        //IMediatorPlug needs
        public UI_Win_MysteryStoreMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_MysteryStoreView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.OnMysteryStoreRefresh,
                CmdConstant.OnMysteryStoreClose,
                Shop_BuyPostItem.TagName,
                Shop_RefreshPostItem.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnMysteryStoreRefresh:
                    if (bIsOnOpen)
                    {
                        OnRefresh();
                    }
                    break;
                case CmdConstant.OnMysteryStoreClose:
                    if (bIsOnOpen)
                    {
                        OnMysteryStoreClose();
                    }
                    break;
                case Shop_BuyPostItem.TagName:
                    break;
                case Shop_RefreshPostItem.TagName:
                    CoreUtils.logService.Info("wwz===========购买或刷新成功----记得做后续处理操作");
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
            bIsOnOpen = true;
            OnRefreshTime();
            mysteryStoreTimer?.Cancel();
            mysteryStoreTimer = null;
            mysteryStoreTimer = Timer.Register(1, delegate
            {
                OnRefreshTime();
            }, null, true, true);
        }

        public override void WinClose()
        {
            bIsOnOpen = false;
            mysteryStoreTimer?.Cancel();
            mysteryStoreTimer = null;
            
            alert?.DestroySelf();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            storeProxy = AppFacade.GetInstance().RetrieveProxy(Game.StoreProxy.ProxyNAME) as StoreProxy;
            playerProxy = AppFacade.GetInstance().RetrieveProxy(Game.PlayerProxy.ProxyNAME) as PlayerProxy;
            currencyProxy = AppFacade.GetInstance().RetrieveProxy(Game.CurrencyProxy.ProxyNAME) as CurrencyProxy;
            itemData = storeProxy.GetMysteryStoreGoodses();
            ClientUtils.PreLoadRes(view.gameObject,view.m_sv_list_ListView.ItemPrefabDataList,OnItemPrefabLoadFinish);
            OnRefreshViewInfo();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.m_btn_close_GameButton.onClick.AddListener(
                delegate
                {
                    CoreUtils.uiManager.CloseUI(UI.s_mysteryStore);
                });
            view.m_btn_Free.m_btn_languageButton_GameButton.onClick.AddListener(OnClickRefresh);
            view.m_btn_refresh.m_btn_languageButton_GameButton.onClick.AddListener(OnClickRefresh);
        }

        protected override void BindUIData()
        {
            
        }
       
        #endregion

        private void OnRefreshTime()
        {
            var leaveTime = storeProxy.GetLeaveTime_MysteryStore();
            if (leaveTime < 0)
            {
                leaveTime = 0;
            }
            view.m_lbl_lastTime_LanguageText.text =
                LanguageUtils.getTextFormat(787003, ClientUtils.FormatTimeTroop((int)leaveTime));
            if (leaveTime <= 0)
            {
                mysteryStoreTimer.Cancel();
                mysteryStoreTimer = null;
                OnMysteryStoreClose();
            }
        }

        private void OnMysteryStoreClose()
        { 
            bIsOnOpen = false;
            alert = Alert.CreateAlert(787010).SetLeftButton(delegate
            {
                CoreUtils.uiManager.CloseUI(UI.s_mysteryStore);
            }).Show();
            // CoreUtils.uiManager.CloseUI(UI.s_mysteryStore);
        }
        private void OnRefresh()
        {
            OnRefreshViewInfo();
            itemData = storeProxy.GetMysteryStoreGoodses();
            view.m_sv_list_ListView.FillContent(itemData != null ? itemData.Count : 0);
        }

        private void OnRefreshViewInfo()
        {
            view.m_btn_Free.gameObject.SetActive(storeProxy.HasFreeRefreshCount_MysteryStore());
            view.m_btn_refresh.gameObject.SetActive(!storeProxy.HasFreeRefreshCount_MysteryStore());
            bool isRefrshEnable = storeProxy.HasCostDiamondRefreshCount_MysteryStore();
            view.m_btn_refresh.m_btn_languageButton_GameButton.interactable = isRefrshEnable;
            view.m_btn_refresh.m_img_forbid_PolygonImage.gameObject.SetActive(!isRefrshEnable);
            view.m_lbl_times_LanguageText.text = LanguageUtils.getTextFormat(787004,
                storeProxy.GetMaxRefreshCount_MysteryStore() - storeProxy.GetCurRefreshCount_MysteryStore(), storeProxy.GetMaxRefreshCount_MysteryStore());
            view.m_btn_refresh.m_lbl_line2_LanguageText.text = storeProxy.GetCurRefreshCost_MysteryStore().ToString();
            ClientUtils.LoadSprite(view.m_btn_refresh.m_img_icon2_PolygonImage,currencyProxy.GeticonIdByType(EnumCurrencyType.denar));
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_btn_refresh.m_lbl_line2_LanguageText.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_btn_refresh.m_pl_line2_HorizontalLayoutGroup.transform as RectTransform);
        }
        
        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_list_ListView.SetInitData(dict, funcTab);
            view.m_sv_list_ListView.FillContent(itemData != null ? itemData.Count : 0);
        }
        private void ItemEnter(ListView.ListItem item)
        {
            if (item == null || itemData == null || item.index >= itemData.Count) return;
            int index = 0;
            List<MysteryStoreItemData> datas = new List<MysteryStoreItemData>();
            for (int i = 0; i < itemData.Count; i++)
            {
                if (item.index == index)
                {
                    datas = itemData[i].GetItems();
                    break;
                }
                index++;
            }

            UI_LC_MysteryStore_SubView subView = null;
            
            if (item.data == null)
            {
                subView = new UI_LC_MysteryStore_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_LC_MysteryStore_SubView;
            }
            if (subView == null) return;
            subView.OnRefreshItem(datas);

        }

        private void OnClickRefresh()
        {
            if (storeProxy.HasCostDiamondRefreshCount_MysteryStore())
            {
                storeProxy.RefreshItemList_MysteryStore();
            }
        }
        
        private GrayChildrens GetMakeChildrenGray()
        {
            if (makeChildrenGray == null)
            {
                makeChildrenGray = view.m_btn_refresh.gameObject.GetComponent<GrayChildrens>();
            }

            return makeChildrenGray;
        }
    }
}