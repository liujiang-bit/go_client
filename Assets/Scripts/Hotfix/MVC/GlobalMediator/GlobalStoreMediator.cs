// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月24日
// Update Time         :    2020年4月24日
// Class Description   :    GlobalStoreMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Client;
using PureMVC.Patterns;
using SprotoType;
using PureMVC.Interfaces;
using Skyunion;

namespace Game {
    public class GlobalStoreMediator : GameMediator {
        #region Member
        public static string NameMediator = "GlobalStoreMediator";

        private HUDUI mysteryStoreBubbleHudUI;
        private CityBuildingProxy m_cityBuildingProxy;
        private StoreProxy m_storeProxy;
        private GameObject go_mysteryStoreBuilding;
        private BuildingInfoEntity buindingInfo;
        // private const int MysteryStoreBuildingId = 142;
        // private bool b_unOpenStore;
        #endregion

        //IMediatorPlug needs
        public GlobalStoreMediator():base(NameMediator, null ) {}

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.OnMysteryStoreOpen,
                CmdConstant.HideMysteryStoreBubble,
                CmdConstant.SystemDayTimeChange,
                CmdConstant.CityBuildingLevelUP,
                CmdConstant.CityBuildingDone,
            }.ToArray();
        }

       public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnMysteryStoreOpen: 
                    ShowMysteryStoreBubble();
                    break;
                case CmdConstant.HideMysteryStoreBubble:
                    HideMysteryStoreBubble();
                    break;
                case CmdConstant.SystemDayTimeChange:
                    OnAcrossNextDay();
                    break;
                // case CmdConstant.CityBuildingLevelUP:
                //     OnMysteryStoreBuildingFinish(notification.Body);
                //     break;
                case CmdConstant.CityBuildingDone:
                    m_storeProxy.OnMysteryStoreRefresh();
                    break;
                default:
                    break;
            }
        }
        
        #region UI template method          

        protected override void InitData()
        {
            OnInitGetData();
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }

        public override void Update()
        {

        }

        public override void LateUpdate()
        {
            
        }

        public override void FixedUpdate()
        {

        }

        #endregion

        //展示驿站气泡框
        private void ShowMysteryStoreBubble()
        {
            if (!CheckMysteryStoreBuilding())
            {
                // CoreUtils.logService.Error("wwz==========错误，没有驿站建筑却妄图显示气泡框");
                return;
            }
            if (mysteryStoreBubbleHudUI == null)
            {
                mysteryStoreBubbleHudUI = HUDUI.Register(UI_Pop_IconOnMysteryStoreView.VIEW_NAME, typeof(UI_Pop_IconOnMysteryStoreView), HUDLayer.city, go_mysteryStoreBuilding)
                    .SetCameraLodDist(0, 270f).SetScaleAutoAnchor(true).SetPosOffset(new Vector2(0, 25f))
                    .SetInitCallback((ui) =>
                 {
                     var view = ui.gameView as UI_Pop_IconOnMysteryStoreView;
                     if (view == null) return;
                     view.m_btn_click_GameButton.onClick.AddListener(() =>
                     {
                         m_storeProxy.OpenMysteryStore();
                     });
                 });
                ClientUtils.hudManager.ShowHud(mysteryStoreBubbleHudUI);
                //ShowTitleFloatView();
                m_storeProxy.IsStoreBubbleShowed = true;
            }
        }
        //出现飘字界面
        private void ShowTitleFloatView()
        {
            Tip.CreateTip(new Tip.OtherAssetData(RS.Tip_MysteryStore, null,2f)).Show();
        }

        //隐藏驿站气泡框
        private void HideMysteryStoreBubble()
        {
            if (mysteryStoreBubbleHudUI != null)
            {
                mysteryStoreBubbleHudUI.Close();
                mysteryStoreBubbleHudUI = null;
            }
        }

        //当跨天
        private void OnAcrossNextDay()
        {
            if (CheckMysteryStoreBuilding())
            {
                ShowMysteryStoreBubble();
            }
        }

        //当驿站建筑完成
        private void OnMysteryStoreBuildingFinish(object buildingId)
        {
            if (buildingId == null)
            {
                return;
            }

            long id = (long)buildingId;
            if (id == (int) EnumCityBuildingType.CourierStation)
            {
                if (GetMysteryStoreBuindingInfo() != null && buindingInfo.level == 1 && m_storeProxy.CheckIsOpen_MysteryStore())
                {
                    ShowMysteryStoreBubble();
                }
            }
        }

        //初始化时设置数据
        private void OnInitGetData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_storeProxy = AppFacade.GetInstance().RetrieveProxy(StoreProxy.ProxyNAME) as StoreProxy;
            bool hasBuilding = CheckMysteryStoreBuilding();
            if (hasBuilding && m_storeProxy.CheckIsOpen_MysteryStore())
            {
                ShowMysteryStoreBubble();
            }
        }

        //检查是否有建筑go
        private bool CheckMysteryStoreBuilding()
        {
            if (go_mysteryStoreBuilding == null)
            {
                if (GetMysteryStoreBuindingInfo() == null)
                {
                    return false;
                }
                go_mysteryStoreBuilding = CityObjData.GeBuildTipTargetGameObject(buindingInfo.buildingIndex);
                if (go_mysteryStoreBuilding == null)
                {
                    return false;
                }
            }

            return true;
        }

        //得到神秘商店信息
        private BuildingInfoEntity GetMysteryStoreBuindingInfo()
        {
            if (buindingInfo == null)
            {
                buindingInfo = m_cityBuildingProxy.GetBuildingInfoByType((int) EnumCityBuildingType.CourierStation);
            }
            return buindingInfo;
        }
    }
}