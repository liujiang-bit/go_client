// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    GlobalScoutsCampMediator 斥候营地状态
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using Client;
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class GlobalScoutsCampMediator : GameMediator
    {
        public static string NameMediator = "GlobalScoutsCampMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private ScoutProxy m_scoutProxy;
        private PlayerProxy m_playerProxy;

        private bool m_isNeedRemind;

        private long m_selectBuildID; 
        private long m_buildingIndex; 

        private HUDUI m_buildingHudUI;

        //IMediatorPlug needs
        public GlobalScoutsCampMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public GlobalScoutsCampMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.ScoutQueueUpdate,           
                CmdConstant.CityBuildingDone,           //城市建筑创建完成
                CmdConstant.CityBuildingLevelUP,        //建筑创建升级
                CmdConstant.UnlockAllFog,
                CmdConstant.FogUnlock,
                CmdConstant.BuildSelected,
                CmdConstant.BuildSelectReset,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            base.HandleNotification(notification);
            switch (notification.Name)
            {
                case CmdConstant.BuildSelected:
                    {
                        m_selectBuildID = (long)notification.Body;
                        UpdateBuildingStatus();
                    }

                    break;
                case CmdConstant.BuildSelectReset:
                    {
                        m_selectBuildID = 0;
                        UpdateBuildingStatus();
                    }
                    break;
                case CmdConstant.CityBuildingLevelUP:
                    long buildingIndex2 = (long)notification.Body;
                    BuildingInfoEntity info2 = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex2);
                    if (info2 != null)
                    {
                        if (info2.type == (int)EnumCityBuildingType.ScoutCamp)
                        {
                            UpdateBuildingStatus();
                        }
                    }
                    break;
                case CmdConstant.CityBuildingDone:
                    UpdateBuildingStatus();
                    break;
                case CmdConstant.ScoutQueueUpdate:
                    UpdateBuildingStatus();
                    break;
                case CmdConstant.UnlockAllFog:          //解锁所有迷雾
                    m_isNeedRemind = false;
                    CloseHudUI();
                    break;
                case CmdConstant.FogUnlock:
                    UpdateBuildingStatus();
                    break;
                default:
                    break;
            }
        }

        #region UI template method

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_isNeedRemind = true;

        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        #endregion  

        private void UpdateBuildingStatus()
        {
            if (!m_isNeedRemind)
            {
                return;
            }
            if (WarFogMgr.IsAllFogOpen()) //迷雾已全开
            {
                m_isNeedRemind = false;
                CloseHudUI();
                return;
            }
            if (m_buildingIndex <= 0)
            {
                GetScoutsCampBuildingIndex();
                if (m_buildingIndex <= 0)
                {
                    return;
                }
            }
            if ( m_selectBuildID == m_buildingIndex)
            {
                CloseHudUI();
                return;
            }
            GameObject go = CityObjData.GeBuildTipTargetGameObject(m_buildingIndex);
            if (go == null)
            {
                return;
            }
            //判断一下是否直接刷新
            if (m_buildingHudUI !=null)
            {
                if (!m_buildingHudUI.bDispose)
                {
                    if (m_buildingHudUI.uiObj != null)
                    {
                        RefreshStatus(m_buildingHudUI);
                    }
                    return;
                }
            }
            //创建
            HudCreate(go);
        }

        private void GetScoutsCampBuildingIndex()
        {
            BuildingInfoEntity buildingInfo = m_cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.ScoutCamp));
            if (buildingInfo != null)
            {
                m_buildingIndex = buildingInfo.buildingIndex;
            }
        }

        private void HudCreate(GameObject go)
        {
            HUDUI curHud = HUDUI.Register(UI_Pop_TextOnBuildingView.VIEW_NAME, typeof(UI_Pop_TextOnBuildingView), HUDLayer.city, go);
            curHud.SetScaleAutoAnchor(true);
            curHud.SetCameraLodDist(0, 270f);
            curHud.SetPosOffset(new Vector2(0, 100f));
            curHud.SetInitCallback(HudCreateCallback);
            ClientUtils.hudManager.ShowHud(curHud);

            m_buildingHudUI = curHud;
        }

        private void HudCreateCallback(HUDUI hud)
        {
            if (hud.targetObj == null || hud.uiObj == null)
            {
                Debug.LogWarning("节点被干掉了");
                return;
            }
            UI_Pop_TextOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_TextOnBuildingView>(hud.uiObj);
            hud.SetData(hudView);
            RefreshStatus(hud);
        }

        private void RefreshStatus(HUDUI hud)
        {
            if (hud.targetObj == null || hud.uiObj == null) //还没创建好 直接return
            {
                return;
            }
            if (m_scoutProxy.GetCanDispatchCount() > 0)
            {
                if (hud.data == null)
                {
                    return;
                }
                UI_Pop_TextOnBuildingView hudView = hud.data as UI_Pop_TextOnBuildingView;
                hudView.m_lbl_languageText_LanguageText.text = LanguageUtils.getText(181146);
            }
            else
            {
                hud.Close();
            }
        }

        private void CloseHudUI()
        {
            if (m_buildingHudUI != null && m_buildingHudUI.uiObj != null)
            {
                m_buildingHudUI.Close();
            }
        }

    }
}

