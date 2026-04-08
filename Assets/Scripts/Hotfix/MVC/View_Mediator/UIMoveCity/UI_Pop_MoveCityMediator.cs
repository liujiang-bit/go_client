// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月29日
// Update Time         :    2020年4月29日
// Class Description   :    UI_Pop_MoveCityMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using Data;
using System;
using UnityEngine.UI;
using DG.Tweening;
using Hotfix;
using System.Net.Http.Headers;
using Hotfix.Utils;

namespace Game {

    public class UI_Pop_MoveCityMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_MoveCityMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Pop_MoveCityMediator(object viewComponent ):base(NameMediator, viewComponent ) { this.IsOpenUpdate = true; }


        public UI_Pop_MoveCityView view;


        private PlayerProxy m_playerProxy;
        private BagProxy m_bagProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private CurrencyProxy m_currencyProxy;
        private WorldMapObjectProxy m_worldProxy;
        private TroopProxy m_troopProxy;
        private CityBuffProxy m_cityBuffProxy;
        private AllianceProxy m_allianceProxy;
        private Vector3 lastWorldPos;
        private Vector2 lastViewCenter;

        private ItemDefine m_cityRemoveItem1;//新手迁城消耗的道具
        private ItemDefine m_cityRemoveItem2;//领土迁城消耗的道具
        private ItemDefine m_cityRemoveItem3;//定点迁城消耗的道具

        private WorldMgrMediator m_worldMgrMediator;
        private CityGlobalMediator m_cityGlobalMediator;

        private ItemDefine m_choosedItemDefine;
        private int m_choosedItemId;
        private int m_choosedType;//1234
        

        private GameObject cityModelObj;//城市模型
        private Vector3 cityModelPosition = Vector3.one;
        private GameObject cityRadiusObj;//城市底圈
        private GameObject forbiddenMeshModelObj;//禁区
        private MeshCollider m_forbiddenMeshCollider;//禁区碰撞体
        private Vector3 cityModelAlignPosition;
        private Vector3 preCheckCityPos;

        private Transform m_root;
        private const string m_root_path = "SceneObject/rss_root";
        private Timer timer;
        private bool isDrag = false;
        private bool isDragModel = false;
        private bool m_canTeleport = false;

        private Vector3 touchStartTerrainPos;
        private Vector3 touchStartModelPos;
        private Vector2 dragTouchPos;
        private Vector2 touchPos;

        private FogSystemMediator m_fogMediator;

        private Timer m_timeTick;//TODO:
        private float preTime = -2;
        private float cityRadiusCollide = 0;//城市半径
        private int resourceGatherRadiusCollide;//资源田村庄、山洞半径
        private int[] monsterRadiusCollide;
        private int StrongHoldRadiusCollide;//关卡、圣所、圣坛、圣祠、神庙
        private int AllianceBuildingRadiusCollide;//联盟要塞、联盟旗帜、联盟资源中心
        private int m_zone;//当前区域id

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                 CmdConstant.OnTouche3D,
                CmdConstant.OnTouche3DBegin,
                CmdConstant.OnTouche3DEnd,
                CmdConstant.OnTouche3DReleaseOutside,
                Map_MoveCity.TagName,
                CmdConstant.FuncGuideMoveCityEffect,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnTouche3D:
                    {
                        if (notification.Body is Touche3DData)
                        {
                            Touche3DData touche3DData = (Touche3DData)notification.Body;
                            OnTouche3D(touche3DData.x, touche3DData.y, touche3DData.parentName, touche3DData.colliderName);
                        }
                    }
                    break;
                case CmdConstant.OnTouche3DBegin:
                    {
                        if (notification.Body is Touche3DData)
                        {
                            Touche3DData touche3DData = (Touche3DData)notification.Body;
                            OnTouche3DBegin(touche3DData.x, touche3DData.y, touche3DData.parentName,
                                touche3DData.colliderName);
                        }
                    }
                    break;
                case CmdConstant.OnTouche3DEnd:
                    {
                        if (notification.Body is Touche3DData)
                        {
                            Touche3DData touche3DData = (Touche3DData)notification.Body;
                            OnTouche3DEnd(touche3DData.x, touche3DData.y, touche3DData.parentName,
                                touche3DData.colliderName);
                        }
                    }
                    break;
                case CmdConstant.OnTouche3DReleaseOutside:
                    {
                        if (notification.Body is Touche3DData)
                        {
                            Touche3DData touche3DData = (Touche3DData)notification.Body;
                            OnTouche3DReleaseOutside(touche3DData.x, touche3DData.y, touche3DData.parentName,
                                touche3DData.colliderName);
                        }
                    }
                    break;
                case Map_MoveCity.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            switch ((ErrorCode)@error.errorCode)
                            {
                                case ErrorCode.MAP_MOVE_CITY_CANT_ARRIVE:
                                       // Tip.CreateTip(770009).Show();
                                    break;
                            }
                        }
                        else
                        {
                            
                        }
                        CoreUtils.uiManager.CloseUI(UI.s_moveCity);
                    }
                    break;
                case CmdConstant.FuncGuideMoveCityEffect:
                    if (cityModelObj != null)
                    {
                        FingerTargetParam param = new FingerTargetParam();
                        param.AreaTarget = cityModelObj;
                        param.EffectMountTarget = cityModelObj;
                        param.IsTouchBeginClose = true;
                        param.NodeType = 2;
                        param.ArrowDirection = (int)EnumArrorDirection.None;
                        param.IsAutoClose = false;
                        param.EffectName = "UI_10085";
                        CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
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
            m_worldMgrMediator.SetIgnoreOwn(true);
        }

        public override void WinClose(){
            if (timer != null)
            {
                timer.Cancel();
                timer = null;
            }
            DestroyCityModels();
            DestroyForbiddenMeshModels();
            m_worldMgrMediator.SetIgnoreOwn(false);

        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            if (this.view.gameObject.activeSelf)
            {
                bool HasMapCollode = m_worldMgrMediator.HasCollideMapObject();
                if (HasMapCollode)
                {
                    if (this.m_canTeleport)
                    {
                        SetCanTeleport(false);
                    }
                }
            }
        }        

        protected override void InitData()
        {
            m_worldMgrMediator = AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            m_fogMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_cityGlobalMediator = AppFacade.GetInstance().RetrieveMediator(CityGlobalMediator.NameMediator) as CityGlobalMediator;
            m_cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            m_cityRemoveItem1 = CoreUtils.dataService.QueryRecord<ItemDefine>(m_playerProxy.ConfigDefine.cityRemoveItem1);
            m_cityRemoveItem2 = CoreUtils.dataService.QueryRecord<ItemDefine>(m_playerProxy.ConfigDefine.cityRemoveItem3);
            m_cityRemoveItem3 = CoreUtils.dataService.QueryRecord<ItemDefine>(m_playerProxy.ConfigDefine.cityRemoveItem2);

            if (view.data is int)
            {
                m_choosedType = (int)view.data;
            }
            else
            {
                m_choosedType =  3;
            }

            if (m_choosedType ==1)
            {
                m_choosedItemDefine = m_cityRemoveItem1;
            }
            else if (m_choosedType == 2)
            {
                m_choosedItemDefine = m_cityRemoveItem2; 
            }
            else if (m_choosedType == 3)
            {
                m_choosedItemDefine = m_cityRemoveItem3;
            }
            else if (m_choosedType ==4 )
            {
                m_choosedItemDefine = m_cityRemoveItem2;
            }
            else if (m_choosedType == 5)
            {
                m_choosedItemDefine = m_cityRemoveItem3;
            }
            RefreshBeforeViewByItem(m_choosedItemDefine);

            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            cityRadiusCollide = config.cityRadiusCollide;
            m_zone  = MapManager.Instance().GetMapZone(new Vector2(m_playerProxy.CurrentRoleInfo.pos.x/100, m_playerProxy.CurrentRoleInfo.pos.y / 100));

            var viewCenter = WorldCamera.Instance().GetViewCenter();
            var pos = new Vector3(viewCenter.x, 0, viewCenter.y); 
            var alignPos = (pos);
            cityModelAlignPosition = alignPos;
        }

        protected override void BindUIEvent()
        {
            view.m_btn_buy.AddClickEvent(OnBuyBtnClick);
            view.m_btn_move.AddEvent(OnMoveBtnClick);
            view.m_btn_cancel.AddEvent(OnCancelBtnClick); 
        }


        protected override void BindUIData()
        {
            {
                GrayChildrens makeChildrenGray = view.m_btn_buy.m_root_RectTransform.GetComponent<GrayChildrens>();
                if (makeChildrenGray != null)

                {
                    view.m_btn_buy.m_btn_languageButton_GameButton.interactable = false;
                    makeChildrenGray.Gray();

                }
            }
            {
                GrayChildrens makeChildrenGray = view.m_btn_move.m_root_RectTransform.GetComponent<GrayChildrens>();
                if (makeChildrenGray != null)

                {
                    view.m_btn_move.m_btn_languageButton_GameButton.interactable = false;
                    makeChildrenGray.Gray();

                }
            }
            float dxf = WorldCamera.Instance().getCameraDxf("dispatch");
                WorldCamera.Instance().SetCameraDxf(dxf, 1000, () => { timer = Timer.Register(1, () => {
          
                    CheckCanTeleport(true);
                }); });
            m_worldMgrMediator.SetWorldMapState(WorldEditState.GuildBuildCreate);
            CoreUtils.inputManager.AddTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
            CreateForbidenMesh();
            CreateCityModels();
        }

        #endregion

private void RefreshAfterViewByItem(ItemDefine itemDefine)
        {
           // Debug.LogError(itemDefine.ID);
            view.m_UI_ItemAfter.Refresh(itemDefine,0,false);
            view.m_lbl_itemNameAfter_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
            view.m_lbl_itemDescAfter_LanguageText.text = LanguageUtils.getText(itemDefine.l_desID);
            view.m_btn_buy.SetNum(itemDefine.shortcutPrice.ToString("N0"));
            float width1 = view.m_btn_buy.m_lbl_line2_LanguageText.preferredWidth;
            view.m_btn_buy.m_lbl_line2_LanguageText.GetComponent<RectTransform>().sizeDelta = new Vector2(width1, 27.7f);
            long num = m_bagProxy.GetItemNum(itemDefine.ID);
            if (num <= 0)
            {
                view.m_lbl_itemTipAfter_LanguageText.text = LanguageUtils.getTextFormat( 770103,LanguageUtils.getText(  itemDefine.l_nameID));
            }
            else
            {
                view.m_lbl_itemTipAfter_LanguageText.text = LanguageUtils.getTextFormat(770104, num);
            }
           
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_btn_buy.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            if (m_bagProxy.GetItemNum(itemDefine.ID) > 0)
            {
                view.m_btn_buy.gameObject.SetActive(false);
                view.m_btn_move.gameObject.SetActive(true);
            }
            else
            {
                view.m_btn_buy.gameObject.SetActive(true);
                view.m_btn_move.gameObject.SetActive(false);
            }
        }
        private void RefreshBeforeViewByItem(ItemDefine itemDefine)
        {
        //    Debug.LogError(itemDefine.ID);
            view.m_UI_ItemBefotr.Refresh(itemDefine, 0, false);
            view.m_lbl_itemNameBefore_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
            view.m_lbl_itemDescBefore_LanguageText.text = LanguageUtils.getText(itemDefine.l_desID);
            view.m_btn_buy.SetNum(itemDefine.shortcutPrice.ToString("N0"));
            float width1 = view.m_btn_buy.m_lbl_line2_LanguageText.preferredWidth;
            view.m_btn_buy.m_lbl_line2_LanguageText.GetComponent<RectTransform>().sizeDelta = new Vector2(width1, 27.7f);
            long num = m_bagProxy.GetItemNum(itemDefine.ID);
            if (num <= 0)
            {
                view.m_lbl_itemTipBefore_LanguageText.text = LanguageUtils.getTextFormat(770103, LanguageUtils.getText(itemDefine.l_nameID));
            }
            else
            {
                view.m_lbl_itemTipBefore_LanguageText.text = LanguageUtils.getTextFormat(770104, num);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_btn_buy.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            if (m_bagProxy.GetItemNum(itemDefine.ID) > 0)
            {
                view.m_btn_buy.gameObject.SetActive(false);
                view.m_btn_move.gameObject.SetActive(true);
            }
            else
            {
                view.m_btn_buy.gameObject.SetActive(true);
                view.m_btn_move.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 购买并使用按钮
        /// </summary>
        private void OnBuyBtnClick()
        {
            if (m_cityBuffProxy.CheckMoveCity())
            {
                Alert.CreateAlert(LanguageUtils.getTextFormat(300072, m_choosedItemDefine.shortcutPrice.ToString("N0"))).SetRightButton(() =>
                {
                    if (!m_currencyProxy.ShortOfDenar(m_choosedItemDefine.shortcutPrice))
                    {
                        Map_MoveCity.request req = new Map_MoveCity.request();
                        req.type = m_choosedType;
                        if (m_choosedType == 5)
                        {
                            req.type = 3;
                        }
                        else if (m_choosedType == 4)
                        {
                            req.type = 2;
                        }
                        req.pos = new PosInfo();
                        req.pos.x = (long)(cityModelAlignPosition.x * 100);
                        req.pos.y = (long)(cityModelAlignPosition.z * 100);
                        AppFacade.GetInstance().SendSproto(req);
                    }
                }).SetLeftButton(() => { CoreUtils.uiManager.CloseUI(UI.s_moveCity); }).Show();
            }
            CoreUtils.uiManager.CloseUI(UI.s_moveCity);

        }

        /// <summary>
        /// 迁城按钮
        /// </summary>
        private void OnMoveBtnClick()
        {
            if (m_fogMediator.HasFogAtWorldPos(cityModelPosition.x, cityModelPosition.z))
            {
                Tip.CreateTip(770005).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            if (m_cityBuffProxy.CheckMoveCity())
            {
                Map_MoveCity.request req = new Map_MoveCity.request();
                req.type = m_choosedType;
                if (m_choosedType == 5)
                {
                    req.type = 3;
                }
                else if (m_choosedType == 4)
                {
                    req.type = 2;
                }
                req.pos = new PosInfo();
                req.pos.x = (long)(cityModelAlignPosition.x * 100);
                req.pos.y = (long)(cityModelAlignPosition.z * 100);
                AppFacade.GetInstance().SendSproto(req);
            }
            CoreUtils.uiManager.CloseUI(UI.s_moveCity);
        }
        private void OnCancelBtnClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_moveCity);
        }
        private void OnTouche3DBegin(int x, int y, string parentName, string colliderName)
        {
        }

        private void OnTouche3D(int x, int y, string parentName, string colliderName)
        {
        }

        private void OnTouche3DEnd(int x, int y, string parentName, string colliderName)
        {
        }
        private void OnTouche3DReleaseOutside(int x, int y, string parentName, string colliderName)
        {
          //  Debug.LogError("OnTouche3DReleaseOutside" + parentName + "  " + colliderName);
        }
        private void OnTouchBegan(int x, int y)
        {
            isDrag = true;

            isDragModel = !isDrag;

            touchPos = new Vector2(x, y);

            if (cityModelObj != null)
            {
                var terrainPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
                float radius = 6;
                var modelPos = cityModelObj.transform.position;
                var distSq = Vector3.Distance(modelPos, terrainPos);
                preTime = ServerTimeModule.Instance.GetServerTime();
                if (distSq < radius)
                {
                    isDragModel = true;
                    touchStartTerrainPos = terrainPos;
                    touchStartModelPos = modelPos;
                    dragTouchPos = new Vector2(x, y);

                    WorldCamera.Instance().SetCanDrag(false);
                    WorldCamera.Instance().SetCanClick(false);

                    setVisbleUI(false);
                }
              //  Debug.LogErrorFormat("distSq{0}", distSq);
            }

            if (!isDragModel)
            {
                UpdatePopPos();
            }
        }
        private void OnTouchMoved(int x, int y)
        {
            if (!isDrag)
            {
                return;
            }

            if (isDragModel)
            {
                dragTouchPos = new Vector2(x, y);
                var newTerrainPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);

                if (newTerrainPos != touchStartTerrainPos)
                {
                    var distVec = newTerrainPos - touchStartTerrainPos;
                    var newPos = touchStartModelPos + distVec;
                    if (cityModelObj != null)
                    {
                        var oldPos = cityModelPosition;
                        SetCityModelPos(newPos);
                        m_worldMgrMediator.OnWorldViewChange(newPos.x, newPos.z, WorldCamera.Instance().getCurrentCameraDxf());
                        CheckCanTeleport();
                        if (oldPos != cityModelPosition)
                        {
                            //TODO 播放音效
                        }
                    }
                }
            }
            else
            {
                if (cityModelObj != null)
                {
                    var modelPos = cityModelObj.transform.position;

                    Vector2 screenPos = WorldCamera.Instance().GetCamera().WorldToViewportPoint(modelPos);
                    float edgeRateX = 0.05f;
                    float edgeRateY = 0.04f;

                    var targetScreenPos = new Vector2(screenPos.x, screenPos.y);
                    if (screenPos.x < edgeRateX)
                    {
                        targetScreenPos.x = edgeRateX;
                    }
                    else
                    {
                        if (1 - edgeRateX < screenPos.x)
                        {
                            targetScreenPos.x = 1 - edgeRateX;
                        }
                    }

                    if (screenPos.y < edgeRateY)
                    {
                        targetScreenPos.y = edgeRateY;
                    }

                    else
                    {
                        if (1 - edgeRateY < screenPos.y)
                        {
                            targetScreenPos.y = 1 - edgeRateY;
                        }
                    }

                    if (targetScreenPos != screenPos)
                    {
                        float screenWidth = Screen.safeArea.width;
                        float screenHeight = Screen.safeArea.height;
                        float nx = screenWidth * targetScreenPos.x;
                        float ny = screenHeight * targetScreenPos.y;
                        var newTerrainPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), nx, ny);
                        SetCityModelPos(newTerrainPos);
                    }
                    CheckCanTeleport();
                    UpdatePopPos();
                }
            }
        }

        private void OnTouchEnded(int x, int y)
        {
            isDrag = false;
            isDragModel = false;
            var endPos = new Vector2(x, y);
            WorldCamera.Instance().SetCanDrag(true);
            WorldCamera.Instance().SetCanClick(true);
            var newPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
            m_worldMgrMediator.OnWorldViewChange(newPos.x, newPos.z, WorldCamera.Instance().getCurrentCameraDxf());
            SetCityModelPos(cityModelPosition);
            setVisbleUI(true);
            CheckCanTeleport(true);

        }
        private float m_lastSendServerCheck = -1;
        private void CheckCanTeleport(bool isMouseUp = false)
        {
            if (m_forbiddenMeshCollider == null)
            {
                return;
            }
            bool HasMapCollode = m_worldMgrMediator.HasCollideMapObject();
            if (HasMapCollode == false)
            {
                if (m_worldProxy.IsPosInNoBuilding(m_forbiddenMeshCollider, cityModelObj.transform.position, cityRadiusCollide))
                {
                    HasMapCollode = true;
                }
            }
            var pos = cityModelPosition;
            if (pos != preCheckCityPos || isMouseUp)
            {
                preCheckCityPos = cityModelPosition;

                if (m_fogMediator.HasFogAtWorldPos(pos.x, pos.z))
                {
                    SetCanTeleport(false);
                    if (isMouseUp)
                    {
                        Tip.CreateTip(770005).SetStyle(Tip.TipStyle.Middle).Show();
                    }
                    return;
                }

                if (HasMapCollode)
                {
                    SetCanTeleport(false);
                    if (isMouseUp)
                    {
                        Tip.CreateTip(770006).Show();
                    }
                    return;
                }
            
                CheckBuildCanBuilded(pos.x, pos.z, isMouseUp);
                if ( isMouseUp)
                {
                    var posModel = cityModelObj.transform.position;
                 //   SendCheckBuildCanBuilded(posModel.x, posModel.z);
                    m_lastSendServerCheck = Time.realtimeSinceStartup;
                }
            }
        }
        /// <summary>
        /// 本地校验道具类型
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CheckBuildCanBuilded(float x, float y,bool isMouseUp = false)
        {
            bool canTeleport = true;
            bool GUILD_CREATE_BUILD_CANT_ARRIVE = true;//关卡阻挡
            bool MAP_MOVE_CITY_NO_GUILD_TERRITORY = true;//目标点是其他人的联盟领土
            int lastType = m_choosedType;
            int choosedType = m_choosedType;
            int  currentZone = MapManager.Instance().GetMapZone(new Vector2(x,y));
            if (isZone1() && m_bagProxy.GetItemNum(m_cityRemoveItem1.ID) > 0)
            {
                choosedType = 1;
            }
            else
            {
                bool IsAcceptableZone = m_allianceProxy.IsAcceptableZone(m_zone, currentZone);
                bool IsGuildPreBuild = false;
                long PreBuildGuildID = 0;
                long guild = m_worldProxy.IsPosInGuildArea(x, y,ref IsGuildPreBuild,ref PreBuildGuildID);
                if (!IsAcceptableZone)
                {
                    if (guild == 0)
                    {
                        canTeleport = false;
                        GUILD_CREATE_BUILD_CANT_ARRIVE = false;
                    }
                    else if (guild == m_playerProxy.CurrentRoleInfo.guildId)
                    {
                        choosedType = 2;
                    }
                    else
                    {
                        canTeleport = false;
                        GUILD_CREATE_BUILD_CANT_ARRIVE = false;
                    }
                }
                else
                {
                    if (guild == 0)
                    {
                        if (IsGuildPreBuild && PreBuildGuildID != m_playerProxy.CurrentRoleInfo.guildId)
                        {
                            canTeleport = false;
                            MAP_MOVE_CITY_NO_GUILD_TERRITORY = false;
                        }
                        else
                        {
                            choosedType = 3;
                        }
                    }
                    else if (guild == m_playerProxy.CurrentRoleInfo.guildId)
                    {
                        choosedType = 2;
                        if (m_bagProxy.GetItemNum(m_cityRemoveItem2.ID) <=0&& m_bagProxy.GetItemNum(m_cityRemoveItem3.ID)>0)
                        {
                            choosedType = 3;
                        }
                    }
                    else
                    {
                        canTeleport = false;
                        MAP_MOVE_CITY_NO_GUILD_TERRITORY = false;
                    }
                }
            }
            if (lastType == choosedType)
            {

            }
            else
            {
                m_choosedType = choosedType;
                //  Debug.LogErrorFormat("choosedType:{0},,",  choosetype);
                if (choosedType == 1)
                {
                    MoveToBeginner();
                }
                else if (choosedType == 2)
                {
                    MoveToTerritoria();
                }
                else if (choosedType == 3)
                {
                    MoveToTargeted();
                }
            }
            if (!GUILD_CREATE_BUILD_CANT_ARRIVE)
            {
                if (isMouseUp)
                {
                    Tip.CreateTip(200032).Show();
                }
            }
            if (!MAP_MOVE_CITY_NO_GUILD_TERRITORY)
            {
                if (isMouseUp)
                {
                    Tip.CreateTip(770007).Show();
                }
            }
            SetCanTeleport(canTeleport);
        }
        private void SetCanTeleport(bool canTeleport)
        {
            this.m_canTeleport = canTeleport;

            if (cityRadiusObj)
            {
                var sr = cityRadiusObj.GetComponent<SpriteRenderer>();

                sr.color = canTeleport ? Color.green : Color.red;
            }
  //         Debug.LogErrorFormat("canTeleport:{0},,isDrag{1},isDragModel{2}", canTeleport,isDrag,isDragModel);
            view.m_btn_buy.m_btn_languageButton_GameButton .interactable = canTeleport;
            view.m_btn_move.m_btn_languageButton_GameButton .interactable = canTeleport;
            if (view.m_btn_buy.m_root_RectTransform == null)
            {
                return;
            }
            {
                GrayChildrens makeChildrenGray = view.m_btn_buy.m_root_RectTransform.GetComponent<GrayChildrens>();
                if (makeChildrenGray != null)

                {
                    if (canTeleport)
                    {
                        view.m_btn_buy.m_btn_languageButton_GameButton.interactable = true;
                        makeChildrenGray.Normal();
                    }
                    else
                    {
                        view.m_btn_buy.m_btn_languageButton_GameButton.interactable = false;
                        makeChildrenGray.Gray();
                    }

                }
            }
            {
                GrayChildrens makeChildrenGray = view.m_btn_move.m_root_RectTransform.GetComponent<GrayChildrens>();
                if (makeChildrenGray != null)

                {
                    if (canTeleport)
                    {
                        view.m_btn_move.m_btn_languageButton_GameButton.interactable = true;
                        makeChildrenGray.Normal();
                    }
                    else
                    {
                        view.m_btn_move.m_btn_languageButton_GameButton.interactable = false;

                        makeChildrenGray.Gray();
                    }

                }
            }
        }

        /// <summary>
        /// 定点迁城3
        /// </summary>
        private void MoveToTargeted()
        {
            m_choosedItemDefine = m_cityRemoveItem3;

            MoveTo();
        }
        private void MoveTo()
        {
            RefreshAfterViewByItem(m_choosedItemDefine);
            float time = 1f;
            if (LanguageUtils.IsArabic())
            {
                view.m_img_Animator_Animator.Play("Show");
            }
            else
            {
                view.m_img_Animator_Animator.Play("ShowNoArb");
            }
            Timer.Register(time,()=> {
                RefreshBeforeViewByItem(m_choosedItemDefine);
            },null,false,false,view.vb );

        }
        /// <summary>
        /// 领土迁城2
        /// </summary>
        private void MoveToTerritoria()
        {
            m_choosedItemDefine = m_cityRemoveItem2;
            MoveTo();
        }

        /// <summary>
        /// 新手迁城1
        /// </summary>
        private void MoveToBeginner()
        {
            m_choosedItemDefine = m_cityRemoveItem1;
            MoveTo();
        }


        /// <summary>
        /// 返回迁城结果类型新手迁城 > 道具-领土迁城 > 道具-定点迁城>代币-领土迁城>代币-定点迁城
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// 是否是外围区域
        /// </summary>
        /// <returns></returns>
        private bool isZone1()
        {
            int zone = MapManager.Instance().GetMapZoneLevel(new Vector2( cityModelPosition.x, cityModelPosition.z));
            if (zone == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }

        /// <summary>
        /// 禁区
        /// </summary>
        private void CreateForbidenMesh()
        {
            CoreUtils.assetService.Instantiate("map_4_NoBuilding_NavMesh", (go) => { forbiddenMeshModelObj = go; m_forbiddenMeshCollider = go.GetComponentInChildren<MeshCollider>(); });
        }
        private void CreateCityModels()
        {

            m_worldMgrMediator.SetWorldMapState(WorldEditState.GuildBuildCreate);
            string modelId = m_cityBuildingProxy.GetModelIdByType((int)EnumCityBuildingType.TownCenter);
            CoreUtils.assetService.Instantiate(modelId, (go) =>
            {
                if (go != null)
                {
                    var t = GetRoot();
                    go.transform.SetParent(t);
                    go.transform.localScale = new Vector3( 6.508772f,6.508772f,6.508772f);//TODO:根据摄像头高度换算

                    cityModelObj = go;
                    //   cityModelObj.transform.do
                    var spprite = cityModelObj.GetComponentInChildren<MaskSprite>();
                    spprite.UpdatedMaterial("map_building_ztestoff");
                    CoreUtils.assetService.Instantiate("teleportcity_bg", (rgo) =>
                    {
                        cityRadiusObj = rgo;

                        rgo.transform.localScale = new Vector3(cityRadiusCollide, cityRadiusCollide, cityRadiusCollide);
                        CenterCityModelPos();
                    });
                    cityModelObj.GetComponentInChildren<NightingMask>().SetCanChangeLight(false);

                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideCheck, (int)EnumFuncGuide.MoveCityEffect);
                }

            });
        }

        private void CenterCityModelPos()
        {
            if (cityModelObj != null)
            {
                var viewCenter = WorldCamera.Instance().GetViewCenter();
                var pos = new Vector3(viewCenter.x, 0, viewCenter.y);
                SetCityModelPos(pos);
                UpdatePopPos();
            }
        }
        /// <summary>
        /// 更新弹窗位置
        /// </summary>
        public void UpdatePopPos()
        {
            var world_pos = cityModelPosition;
            var viewCenter = WorldCamera.Instance().GetViewCenter();
            if (world_pos == lastWorldPos && viewCenter == lastViewCenter)
            {
                return;
            }

            lastWorldPos = world_pos;
            lastViewCenter = viewCenter;
            float x;
            float y;



            UICommon.WorldToUIPos(CoreUtils.uiManager.GetCanvas().gameObject, world_pos.x, world_pos.y, world_pos.z, out x, out y);
           // Debug.LogErrorFormat("更新pos{0},,,,{1},,,,{2}", x,y, lastWorldPos);
            UIHelper.CalcPopupPos(new Vector2(x, y), view.gameObject.transform as RectTransform,
                view.m_img_arrowSideL_PolygonImage.gameObject, view.m_img_arrowSideR_PolygonImage.gameObject,
                view.m_img_arrowSideTop_PolygonImage.gameObject, view.m_img_arrowSideButtom_PolygonImage.gameObject);
        }
        public void SetCityModelPos(Vector3 pos)
        {
            cityModelPosition = pos;
            var alignPos = (pos);

            float unitX = WorldMapObjectProxy.TerritoryPerUnit.x;
            float unitY = WorldMapObjectProxy.TerritoryPerUnit.y;
            if (cityModelAlignPosition.x != alignPos.x || cityModelAlignPosition.z != alignPos.z)
            {
                cityModelAlignPosition = alignPos;
            }
            if (cityRadiusCollide> 0)
            {
                m_worldMgrMediator.SetGuildCreateBuildPos(cityModelAlignPosition, cityRadiusCollide);
            }
            if(cityModelObj)
            {
                cityModelObj.transform.position = pos;
            }

            if (cityRadiusObj)
            {
                cityRadiusObj.transform.position = pos;
            }
            UpdatePopPos();
        }
        private static float ALIGN_DIST = 0.01f;
        /// <summary>
        /// w位置矫正
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector3 GetAlignPos(Vector3 pos)
        {
            float x = Mathf.RoundToInt(pos.x );
            float z = Mathf.RoundToInt(pos.z);
            return new Vector3(x, pos.y, z);
        }
        /// <summary>
        /// 删除模型关闭界面
        /// </summary>
        private void DestroyCityModels()
        {
            if (cityModelObj != null)
            {
                m_worldProxy.ClearFakeTerritory();
                CoreUtils.assetService.Destroy(cityModelObj);
                cityModelObj = null;
                m_forbiddenMeshCollider = null;
            }
            if (cityRadiusObj != null)
            {
                CoreUtils.assetService.Destroy(cityRadiusObj);
                cityRadiusObj = null;
            }
            if (forbiddenMeshModelObj != null)
            {
                CoreUtils.assetService.Destroy(forbiddenMeshModelObj);
                forbiddenMeshModelObj = null;
            }

            CoreUtils.inputManager.RemoveTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
        }
        /// <summary>
        /// 大地图返回正常状态
        /// </summary>
        private void DestroyForbiddenMeshModels()
        {
            m_worldMgrMediator.SetWorldMapState(WorldEditState.Normal);
        }
        private Transform GetRoot()
        {
            if (this.m_root == null)
            {
                this.m_root = GameObject.Find(m_root_path).transform;
            }

            return this.m_root;
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        private void setVisbleUI(bool isV)
        {
            this.view.gameObject.SetActive(isV);
        }

    }
}