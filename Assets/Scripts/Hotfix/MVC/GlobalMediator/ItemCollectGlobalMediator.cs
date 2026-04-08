// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月20日
// Update Time         :    2020年5月20日
// Class Description   :    BlacksmithGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Client;
using Data;
using PureMVC.Patterns;
using SprotoType;
using PureMVC.Interfaces;
using Skyunion;

namespace Game {
    public class ItemCollectGlobalMediator : GameMediator {
        #region Member
        public static string NameMediator = "ItemCollectGlobalMediator";
        
        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private BagProxy m_bagProxy;
        private Dictionary<long,HUDUI> m_huds = new Dictionary<long, HUDUI>();
        private List<long> m_hudsLoading = new List<long>();
        private Dictionary<long,int> m_itemIDS = new Dictionary<long, int>();
        private Timer m_refreshItemInfoTimer;
        private long m_selectBuildID = 0;
        private List<int> m_idList = new List<int>() { (int)EnumCityBuildingType.Smithy,(int)EnumCityBuildingType.Market};
        #endregion

        //IMediatorPlug needs
        public ItemCollectGlobalMediator():base(NameMediator, null ) {}

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.EquipMaterialProductionInfoChange,
                CmdConstant.CityBuildingLevelUP,
                CmdConstant.CityBuildingDone,
                CmdConstant.VipStoreShowBubbleChange,
                CmdConstant.VipLevelUP,
                CmdConstant.SystemDayTimeChange,
                CmdConstant.ItemInfoChange,
                CmdConstant.EquipForgeRedDotChange,  
                CmdConstant.BuildSelected,
                CmdConstant.BuildSelectReset,
            }.ToArray();
        }

       public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.EquipMaterialProductionInfoChange:
                case CmdConstant.EquipForgeRedDotChange:
                    OnSmithyQueueChange();
                    break;
                case CmdConstant.BuildSelected:
                    {
                        m_selectBuildID = (long)notification.Body;
                        OnSmithyQueueChange();
                    }
                    break;
                case CmdConstant.BuildSelectReset:
                    {
                        m_selectBuildID = 0;
                        OnSmithyQueueChange();
                    }
                    break;
                case CmdConstant.ItemInfoChange:
                    if (m_refreshItemInfoTimer == null)
                    {
                        m_refreshItemInfoTimer = Timer.Register(0.5f, OnItemInfoChange, null, false);
                    }
                    break;
                case CmdConstant.CityBuildingLevelUP:
                case CmdConstant.CityBuildingDone:
                    UpdateAllHudStatus();
                    break;
                case CmdConstant.VipStoreShowBubbleChange:
                case CmdConstant.VipLevelUP:
                case CmdConstant.SystemDayTimeChange:
                    OnShowVipStoreBubbleChanged();
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

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

        private void OnItemInfoChange()
        {
            OnSmithyQueueChange();
            m_refreshItemInfoTimer = null;
        }
        
        private void OnSmithyQueueChange()
        {
            UpdateHudStatus((int)EnumCityBuildingType.Smithy);
        }

        private void OnShowVipStoreBubbleChanged()
        {
            UpdateHudStatus((int)EnumCityBuildingType.Market);
        }
        
        private void UpdateAllHudStatus()
        {
            for (int i = 0; i < m_idList.Count; i++)
            {
                UpdateHudStatus(m_idList[i]);
            }
        }

        private void UpdateHudStatus(int buildingType)
        {
            BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByType(buildingType);
            if (info != null)
            {
                UpdateBuildingTrainStatus(info);
            }
        }
        
        private void UpdateBuildingTrainStatus(BuildingInfoEntity info)
        {
            GameObject go = CityObjData.GeBuildTipTargetGameObject(info.buildingIndex);
            if (go == null)
            {
                return;
            }

            if (m_huds.ContainsKey(info.buildingIndex) && m_huds[info.buildingIndex].uiObj != null) //直接刷新
            {
                RefreshStatus(m_huds[info.buildingIndex]);
            }
            else if(CheckCanShow(info)) //创建
            {
                if (!m_hudsLoading.Contains(info.buildingIndex))
                {
                    HudCreate(go, info);
                    m_hudsLoading.Add(info.buildingIndex);
                }

            }
        }
        
        private void HudCreate(GameObject go, BuildingInfoEntity info)
        {
            HUDUI curHud = GetHud(go,info);
            curHud.SetData(info);
            curHud.SetScaleAutoAnchor(true);
            curHud.SetCameraLodDist(0, 270f);
            curHud.SetPosOffset(new Vector2(0, 25f));
            curHud.SetInitCallback((ui)=> {
                HudCreateCallback(ui);
                m_hudsLoading.Remove(info.buildingIndex);
            });
            ClientUtils.hudManager.ShowHud(curHud);
            if (!m_huds.ContainsKey(info.buildingIndex))
            {
                m_huds.Add(info.buildingIndex,null);
            }

            m_huds[info.buildingIndex] = curHud;
        }

        private HUDUI GetHud(GameObject go, BuildingInfoEntity info)
        {
            switch (info.type)
            {
                case (int)EnumCityBuildingType.Smithy:
                    return HUDUI.Register(UI_Pop_IconOnEquip_SubView.VIEW_NAME, null,
                        HUDLayer.city, go);
                case (int)EnumCityBuildingType.Market:
                    return HUDUI.Register(UI_Pop_IconOnVipView.VIEW_NAME, typeof(UI_Pop_IconOnVipView),
                        HUDLayer.city, go);
            }
            return HUDUI.Register(UI_Pop_IconOnBuildingView.VIEW_NAME, typeof(UI_Pop_IconOnBuildingView),
                HUDLayer.city, go);
        }

        private void HudCreateCallback(HUDUI hud)
        {
            if (hud.targetObj == null || hud.uiObj == null)
            {
                Debug.LogWarning("节点被干掉了");
                return;
            }
            RefreshStatus(hud);
        }

        private bool CheckCanShow(BuildingInfoEntity info)
        {
            switch (info.type)
            {
                case (int)EnumCityBuildingType.Smithy:
                    if (m_playerProxy.IsProducingMaterial() || m_playerProxy.HasCompleteProduceItems() ||m_bagProxy.HasEquipCanBeForged()
                        ||m_playerProxy.CanProduceItem()||m_bagProxy.GetCanMixDrawingMaterialNum()>0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case (int)EnumCityBuildingType.Market:
                    return m_playerProxy.IsShowVipStorePop();
                    break;
            }

            return false;
        }

        private void RefreshStatus(HUDUI hud)
        {
            var buildingInfo = hud.data as BuildingInfoEntity;
            switch (buildingInfo.type)
            {
                case (int) EnumCityBuildingType.Smithy:
                    RefreshSmithy(hud);
                    break;
                case (int) EnumCityBuildingType.Market:
                    RefreshMarket(hud);
                    break;
            }
        }

        private void RefreshSmithy(HUDUI hud)
        {
            var materialQueue = m_playerProxy.GetCurProduceItems();
            var data = hud.data as BuildingInfoEntity;
            if (data.finishTime != -1)
            {
                hud.Close();
                m_itemIDS.Remove(data.buildingIndex);
                return;
            }
            if (materialQueue == null)
            {
                return;
            }
            if (m_selectBuildID == data.buildingIndex)
            {
                hud.Close();
                m_itemIDS.Remove(data.buildingIndex);
                return;
            }
            UI_Pop_IconOnEquip_SubView hudView = hud.viewData as UI_Pop_IconOnEquip_SubView;
            if (hudView == null)
            {
                hudView = new UI_Pop_IconOnEquip_SubView(hud.uiObj.GetComponent<RectTransform>());
                hud.viewData = hudView;
            }
            
            hudView.SetCollectMaterialInfo(materialQueue, () =>
            {
                ClickGet(hud);
            });
            if (m_playerProxy.HasCompleteProduceItems()){
                int itemID = (int)materialQueue.completeItems[0].itemId;
                if (m_itemIDS.ContainsKey(data.buildingIndex))
                {
                    m_itemIDS[data.buildingIndex]=itemID;
                }
                else
                {
                    m_itemIDS.Add(data.buildingIndex, itemID);
                }
            }
            else if (m_bagProxy.HasEquipCanBeForged())
            {
                hudView.SetCanForgeInfo();
            }
            else if (m_playerProxy.CanProduceItem())
            {
                hudView.SetCanProduceInfo();
            }else if (m_bagProxy.GetCanMixDrawingMaterialNum() > 0)
            {
                hudView.SetCanMixDrawingMaterial();
            }
            else if (m_playerProxy.IsProducingMaterial())
            {
                
            }else
            {
                hud.Close();
                m_itemIDS.Remove(data.buildingIndex);
                return;
            }
        }

        private void RefreshMarket(HUDUI hud)
        {
            if (m_selectBuildID == m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.Market).buildingIndex)
            {
                hud.Close();
                return;
            }
            if (!m_playerProxy.IsShowVipStorePop())
            {
                hud.Close();
                return;
            }
            else
            {
                UI_Pop_IconOnVipView hudView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_IconOnVipView>(hud.uiObj);
                hudView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                hudView.m_btn_click_GameButton.onClick.AddListener(() =>
                {
                    ClickGet(hud);
                });
                
                ClientUtils.LoadSprite(hudView.m_img_icon_PolygonImage, RS.VipStoreBubbleIcon);
            }
        }

        public void GetItemByBuildIndex(long index)
        {
            if (!m_huds.ContainsKey(index))
            {
                return;
            }
            ClickGet(m_huds[index]);
        }

        public void ClickGet(HUDUI hudui)
        {
            if (hudui == null||hudui.uiObj == null)
            {
                return;
            }

            var info = hudui.data as BuildingInfoEntity;
            ItemInfoEntity itemInfo = null;
            RectTransform flyPosTransform = null;
            switch (info.type)
            {
                case (int) EnumCityBuildingType.Smithy:
                    if (m_playerProxy.HasCompleteProduceItems())
                    {
                        m_playerProxy.AwardProduceMaterial();
                        UI_Pop_IconOnEquip_SubView hudView = hudui.viewData as UI_Pop_IconOnEquip_SubView;
                        if (hudView != null)
                        {
                            flyPosTransform = hudView.m_img_icon_PolygonImage.rectTransform;
                        }
                    }

                    break;
                case (int) EnumCityBuildingType.Market:
                    CoreUtils.uiManager.ShowUI(UI.s_VipStore);
                    break;
            }
            
            if (flyPosTransform != null&&m_itemIDS.ContainsKey(info.buildingIndex))
            {
                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                mt.FlyItemEffect(m_itemIDS[info.buildingIndex],0,flyPosTransform,
                    () =>
                    {
                        RefreshStatus(hudui);
                        m_itemIDS.Remove(info.buildingIndex);
                    });
            }
            else
            {
                hudui.Close();
            }
        }
    }
}