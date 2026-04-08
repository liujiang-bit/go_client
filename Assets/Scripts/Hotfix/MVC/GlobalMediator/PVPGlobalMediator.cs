// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    PVPGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client;
using DG.Tweening;
using Hotfix;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using Data;

namespace Game
{
    public class TouchNotTroopSelect
    {
        public int id;
        public bool isAutoMove = false;
        public bool isShowSelectHud = true;
        public bool isCameraFollow = false; //是否摄像头跟随目标
    }

    public class TouchTroopSelectParam
    {
        public int armIndex;
        public bool isOpenWin;
        public bool isSelect = true;
        public bool isDrag = false;
        public bool isCameraFollow { get; set; } = true;
        public bool isGuide;
    }

    public class TouchTroopInfo
    {
        public int mainArmObjectId;
        public List<int> viceArmObjectIdList;
    }

    public class DrawLineInfo
    {
        public int arnyIndex;
        public Vector2 targetPos;
    }

    public class TroopHudMapMarChInfo
    {
        public int arnyIndex;
        public TroopAttackType attackType;
    }

    public class PVPGlobalMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "PVPGlobalMediator";
        private RssProxy m_RssProxy;
        private WorldMapObjectProxy m_worldProxy;
        private MonsterProxy m_MonsterProxy;
        private TroopProxy m_TroopProxy;
        private CityGlobalMediator cityGlobal;
        private PlayerProxy m_PlayerProxy;
        private ScoutProxy m_ScoutProxy;
        private Camera worldCamera;
        private FogSystemMediator m_fogMediator;
        private GlobalViewLevelMediator m_viewLevelMediator;
        private CityBuildingProxy m_CityBuildingProxy;
        private WorldMgrMediator m_WorldMgrMediator;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private AllianceProxy m_allianceProxy;
        private CityBuildingProxy cityBuildingProxy;

        private bool isMoveTouch = false;
        private long curRoleObjectId;
        private ConfigDefine m_ConfigDefine;
        private float m_OperatingHaloTime;
        private float m_multiArmyDoubleClickTime;

        #endregion

        #region 镜头跟随移动目标

        private int m_cameraFollowStatus; //1查找中 2已找到
        private float m_startWaitTime;
        private Timer m_waitFollowTimer;
        private bool m_isCameraFollow;
        private GameObject m_followTarget;
        private int m_targetObjectId;
        private float m_cameraMoveSpeed = 1f;
        private Vector3 m_cameraLastPos = Vector3.zero;

        #endregion

        #region 多部队行军

        private int m_doubleSelectMainArmyIndex = 0;
        private List<int> m_doubleSelectViceArmyIndexList;
        private List<int> m_doubleSelectArmyIndexList;
        private bool m_doubleFlag = false;
        private int m_lastClickObjectId = 0;
        private long m_lastClickObjectTime = 0;

        #endregion

        //IMediatorPlug needs
        public PVPGlobalMediator() : base(
            NameMediator,
            null)
        {
            MediatorName = NameMediator;
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.OnTouche3D,
                CmdConstant.OnTouche3DBegin,
                CmdConstant.OnTouche3DEnd,
                CmdConstant.OnTouche3DReleaseOutside,
                CmdConstant.TouchTroopSelectByFightHudIcon,
                CmdConstant.TouchTroopSelect,
                CmdConstant.DoubleTouchTroopSelect,
                CmdConstant.SelectTroopDragMove,
                CmdConstant.OnCreatePlayBuildingSucceed,
                CmdConstant.CityBuildingDone,
                Map_SearchResource.TagName,
                Map_SearchBarbarian.TagName,
                CmdConstant.MapObjectRemove,
                CmdConstant.NetWorkReconnecting,
                CmdConstant.MapLogicObjectChange,
                CmdConstant.MapViewChange,
                CmdConstant.MapSendMapMarChData,
                Army_ArmyList.TagName,
                Role_ScoutsInfo.TagName,
                Transport_TransportList.TagName,
                CmdConstant.MapRemoveSelectEffect,
                Map_ScoutVillage.TagName,
                CmdConstant.TouchScoutSelect,
                CmdConstant.TouchTransportSelect,
                CmdConstant.ExitCity,
                CmdConstant.ExitCityHide,
                CmdConstant.ArmyDataLodPopRemove,
                CmdConstant.CancelCameraFollow,
                CmdConstant.AutoFollowTroopReturnCity,
                CmdConstant.SetSelectTroop,
                CmdConstant.MapUnSelectCity,
                CmdConstant.OnUIStartTouchTroop,
                CmdConstant.MapCreateTroopHud,
                CmdConstant.ClearGlobalTouchMoveCollide,
                Role_RoleLogin.TagName,
                Role_SetLastServerAndRole.TagName,
                Role_GetRoleList.TagName,
                CmdConstant.MapCreateDrawLine,
                CmdConstant.MapDeleteDrawLine,
                CmdConstant.MapRemoveAllDrawLine,
                CmdConstant.MapCreateSelectMyTroopEffect,
                CmdConstant.MapDeleteSelectMyTroopEffect,
                CmdConstant.MapRemoveAllSelectMyTroopEffect,
                CmdConstant.MapTroopHudMapMarCh,
                CmdConstant.MapDrawLineCity,
                CmdConstant.MapRemoveDrawLineCity
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_RoleLogin.TagName:
                    break;
                case CmdConstant.OnTouche3D:
                {
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3D(touche3DData.x, touche3DData.y, touche3DData.parentName, touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.OnTouche3DBegin:
                {
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3DBegin(touche3DData.x, touche3DData.y, touche3DData.parentName,
                            touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.OnTouche3DEnd:
                {
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3DEnd(touche3DData.x, touche3DData.y, touche3DData.parentName,
                            touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.OnTouche3DReleaseOutside:
                {
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3DReleaseOutside(touche3DData.x, touche3DData.y, touche3DData.parentName,
                            touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.SelectTroopDragMove:
                    {
                        TouchTroopSelectParam param1 = notification.Body as TouchTroopSelectParam;
                        OnSelectTroopDragMove(param1);
                    }
                    break;
                case CmdConstant.TouchTroopSelectByFightHudIcon:
                    {
                        OnRefreshSelcetTroopByFightHudIcon((int)notification.Body);
                    }
                    break;
                case CmdConstant.TouchTroopSelect:
                    {
                        TouchTroopSelectParam param1 = notification.Body as TouchTroopSelectParam;
                        OnRefreshSelcetTroop(param1);
                    }
                    break;
                case CmdConstant.DoubleTouchTroopSelect:
                    OnUIDoubleClick((int)notification.Body);
                    break;
                case CmdConstant.TouchScoutSelect:
                    TouchNotTroopSelect TouchSelectScout = notification.Body as TouchNotTroopSelect;
                    OnSelectScout(TouchSelectScout);
                    break;
                case CmdConstant.MapCreateTroopHud:
                    {
                        int objectId = (int)notification.Body;
                        UpdateByTouchNotTroopSelect(objectId);
                    }
                    break;
                case CmdConstant.TouchTransportSelect:
                    TouchNotTroopSelect TouchSelectTransport = notification.Body as TouchNotTroopSelect;
                    OnSelectTransport(TouchSelectTransport);
                    break;
                case Map_SearchResource.TagName:
                    m_RssProxy.SearchRssCallBack(notification);
                    break;
                case Map_SearchBarbarian.TagName:
                    m_RssProxy.SearchBarbarianCallBack(notification);
                    break;
                case CmdConstant.MapObjectRemove:
                    MapObjectInfoEntity deleteInfo = notification.Body as MapObjectInfoEntity;
                    if (deleteInfo != null)
                    {
                        int id = (int) deleteInfo.objectId;
                        WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveTroopLine(id);
                        WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().RemoveTroop(id);
                        if (m_selectArmId != 0 && deleteInfo.armyIndex == m_selectArmId)
                        {
                            OnTouchUnSelect(null, null);
                        }
                    }
                    break;
                case CmdConstant.NetWorkReconnecting:
                    if (GuideProxy.IsGuideing) //引导阶段不允许清空
                    {
                        return;
                    }

                    AppFacade.GetInstance().SendNotification(CmdConstant.MapClearTroopHud);
                    Clear();
                    m_WorldMgrMediator = null;
                    break;
                case CmdConstant.MapLogicObjectChange:
                    WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateMapData(notification);
                    break;
                case CmdConstant.MapViewChange:
                    MapViewLevel mapViewLevel = (MapViewLevel) notification.Body;
                    WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateTroopLine(mapViewLevel);
                    refreshSelectMyTroopEffectActive(mapViewLevel);
                    break;
                case CmdConstant.MapSendMapMarChData:
                    FreeMarchParam param = notification.Body as FreeMarchParam;
                    if (param != null) SendTroopMapMarCh(param.objectId, param.armyIndex, param.panelType, param.rid, param.armyIndexList, false, param.isCheckWar);
                    break;
                case Role_ScoutsInfo.TagName:
                    m_ScoutProxy.SetScoutData(notification);
                    FogSystemMediator fogMedia = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                    fogMedia.UpdateExplorer();
                    SummonerTroopMgr.Instance.GetISummonerTroop().UpdateSummonerScout(notification);
                    break;
                case Transport_TransportList.TagName:
                    m_TroopProxy.UpdateTransportInfo(notification);
                    SummonerTroopMgr.Instance.GetISummonerTroop().UpdateSummonerTransport(notification);
                    break;
                case Army_ArmyList.TagName:
                    Army_ArmyList.request request = notification.Body as Army_ArmyList.request;
                    if (request.armyInfo != null)
                    {
                        m_TroopProxy.UpdateArmyInfo(request.armyInfo);
                    }

                    SummonerTroopMgr.Instance.GetISummonerTroop().UpdateSummonerTroop(notification);
                    break;
                case CmdConstant.MapRemoveSelectEffect:
                    removeAllSelectMyTroopEffect();
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseSelectMainTroop);
                    break;
                case CmdConstant.ExitCity:
                case CmdConstant.ExitCityHide:

                    if (m_PlayerProxy != null && m_PlayerProxy.CurrentRoleInfo != null)
                    {
                        float x = m_PlayerProxy.CurrentRoleInfo.pos.x / 100;
                        float y = m_PlayerProxy.CurrentRoleInfo.pos.y / 100;


                        if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y, String.Empty))
                        {
                            CheckMapViewOutDis(x, y);
                        }
                    }

                    break;
                case CmdConstant.ArmyDataLodPopRemove:
                case CmdConstant.CancelCameraFollow:
                    CancelFollow();
                    break;
                case CmdConstant.AutoFollowTroopReturnCity:
                    if (notification.Body is GameObject)
                    {
                        m_isCameraFollow = true;
                        m_followTarget = notification.Body as GameObject;
                        m_cameraLastPos = m_followTarget.transform.position;
                    }

                    break;
                case CmdConstant.SetSelectTroop:
                    TouchTroopSelectParam param2 = notification.Body as TouchTroopSelectParam;
                    SetSelectTroop(param2);
                    break;
                case CmdConstant.MapUnSelectCity:
                    UnSelectCity();
                    break;
                case CmdConstant.ClearGlobalTouchMoveCollide:
                    ClearTouchMoveCollide();
                    break;
                case Role_GetRoleList.TagName:
                    var  m_RoleInfoProxy= AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
                    m_RoleInfoProxy?.UpdateRoleInfoData(notification);
                    
                    break;              
                case  Role_SetLastServerAndRole.TagName:

                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        switch ((ErrorCode) error.errorCode)
                        {
                            case ErrorCode.ROLE_CREATE_MAX:
                            case ErrorCode.ROLE_SWITCH_NOT_FOUND_ROLE:
                            case ErrorCode.ROLE_SWITCH_NOT_FOUND_NODE:
                                string name = LanguageUtils.getTextFormat(100125, error.errorCode);
                                Tip.CreateTip(name).Show();
                                break;
                        }
                    }
                    else
                    {
                        var response = notification.Body as Role_SetLastServerAndRole.response;
                        if (response != null)
                        {
                            SendNotification(CmdConstant.ReloadGame);
                            if (!response.HasSelectRid)
                            {
                                RoleInfoHelp.SetIsNewCreateRole();
                            }
                            else
                            {
                                RoleInfoHelp.DeleteNewCreateRole();
                            }
                        } 
                    }
                    break;
                case CmdConstant.MapCreateDrawLine:
                    DrawLineInfo drawLineInfo = notification.Body as DrawLineInfo;
                    CreateDrawLine(drawLineInfo.arnyIndex, drawLineInfo.targetPos);
                    break;
                case CmdConstant.MapDeleteDrawLine:
                    DeleteDrawLine((int)notification.Body);
                    break;
                case CmdConstant.MapRemoveAllDrawLine:
                    RemoveAllDraw();
                    break;
                case CmdConstant.MapDrawLineCity:
                    DrawLineInfo drawLineInfoCity = notification.Body as DrawLineInfo;
                    CreateCityMoveLine(drawLineInfoCity.targetPos);
                    break;
                case CmdConstant.MapRemoveDrawLineCity:
                    DeleteCityMoveLine();                   
                    break;
                case CmdConstant.MapCreateSelectMyTroopEffect:
                    CreateSelectMyTroopEffect((int)notification.Body);
                    break;
                case CmdConstant.MapDeleteSelectMyTroopEffect:
                    DestroySelectMyTroopEffect((int)notification.Body);
                    break;
                case CmdConstant.MapRemoveAllSelectMyTroopEffect:
                    removeAllSelectMyTroopEffect();
                    break;
                case CmdConstant.MapTroopHudMapMarCh:
                    TroopHudMapMarChInfo troopHudMapMarChInfo = notification.Body as TroopHudMapMarChInfo;
                    MapTroopHudMapMarCh(troopHudMapMarChInfo.arnyIndex, troopHudMapMarChInfo.attackType);
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            WorldMapLogicMgr.Instance.Initialize();
            SummonerTroopMgr.Instance.Initialize();
            IsOpenUpdate = true;
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_MonsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_ScoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            cityGlobal =
                AppFacade.GetInstance().RetrieveMediator(CityGlobalMediator.NameMediator) as CityGlobalMediator;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_fogMediator =
                AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_viewLevelMediator =
                AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                    GlobalViewLevelMediator;
            m_CityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_WorldMgrMediator =
                AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            m_RallyTroopsProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            cityBuildingProxy=AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            WorldCamera.Instance().AddViewChange(OnWorldViewChange);
            worldCamera = WorldCamera.Instance().GetCamera();
            CreateSelectEffect();

            m_ConfigDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_OperatingHaloTime = m_ConfigDefine.operatingHaloTime;
            m_multiArmyDoubleClickTime = m_ConfigDefine.moretTroopsClick * 1000;
        }

        protected override void BindUIEvent()
        {
            CoreUtils.inputManager.AddTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
        }

        protected override void BindUIData()
        {
        }

        public override void Update()
        {
            if (m_isCameraFollow)
            {
                if (m_cameraFollowStatus == 1) //查找中
                {
                    return;
                }
                else if (m_cameraFollowStatus == 2)//已找到
                {
                    if (m_followTarget == null)
                    {
                        CancelFollow();
                        return;
                    }
                }
                else
                {
                    return;
                }

                m_cameraLastPos = Vector3.Lerp(m_cameraLastPos, m_followTarget.transform.position,
                    Mathf.Clamp(m_cameraMoveSpeed * Time.deltaTime, 0.0f, 1.0f));
                WorldCamera.Instance().ViewTerrainPos(m_cameraLastPos.x, m_cameraLastPos.z,
                    WorldCamera.INVALID_FLOAT_VALUE, null);
            }

            if(SelectEffect != null && SelectEffect.activeSelf && m_selectEffectFlow != null)
            {
                SelectEffect.transform.position =
                    new Vector3(m_selectEffectFlow.transform.position.x, 0, m_selectEffectFlow.transform.position.z);
            }


            if (m_bDragMove)
            {
                if (CoreUtils.inputManager.GetTouchCount() <= 0)
                {
                    Vector3 v3 = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), m_movePos.x, m_movePos.y);
                    OpenCreateTroopPanel(v3.x, v3.z);
                    OnTouchUnSelect(string.Empty,string.Empty);            
                }
            }
        }

        public override void LateUpdate()
        {
            AutoMoveMgr.Instance.Update(); 
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateArmyDir();
# if UNITY_EDITOR
            GameToolMediator.UpdateView();
#endif
        }

        public override void FixedUpdate()
        {
        }

        public override void OnRemove()
        {
            base.OnRemove();
            Clear(true);
            CoreUtils.inputManager.RemoveTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
        }


        private void Clear(bool isNet=false)
        {
            WorldMapLogicMgr.Instance.Clear();
            SummonerTroopMgr.Instance.Clear();
            DeleteAllMoveLine();
            DeleteAllViceMoveLine();
            if (isNet)
            {                
                RemoveSelcetEffect();
                RemovePlaySelectEffectTimer();
                WorldMapLogicMgr.Instance.MapSelectEffectHandler.Remove();
            }

        }

        private void MapTroopHudMapMarCh(int armyIndex, TroopAttackType attackType)
        {
            m_TroopProxy.TroopMapMarCh(armyIndex, attackType, 0, null, m_doubleSelectArmyIndexList);
        }

        #region 选中我的部队光效管理

        Dictionary<int, MapObjectSelectEffect> selectMyTroopEffectDic = new Dictionary<int, MapObjectSelectEffect>();

        private void CreateSelectMyTroopEffect(int armyIndex)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndex);
            if (armyData == null || armyData.go == null) return;

            Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
            if (formation == null) return;

            MapObjectSelectEffect mapObjectSelectEffect = null;
            if (!selectMyTroopEffectDic.ContainsKey(armyIndex))
            {
                mapObjectSelectEffect = new MapObjectSelectEffect(RS.TroopSelectEffectName);
                selectMyTroopEffectDic.Add(armyIndex, mapObjectSelectEffect);
            }
            else
            {
                mapObjectSelectEffect = selectMyTroopEffectDic[armyIndex];
            }

            if (mapObjectSelectEffect == null) return;

            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            float scale = armyData.armyRadius / configDefine.mapTargetEffectScale;

            mapObjectSelectEffect.AttachTransform(armyData.go.transform, armyData.armyRadius / m_ConfigDefine.mapTargetEffectScale);
        }

        private void DestroySelectMyTroopEffect(int objectId)
        {
            if (selectMyTroopEffectDic.ContainsKey(objectId))
            {
                MapObjectSelectEffect mapObjectSelectEffect = selectMyTroopEffectDic[objectId];
                if (mapObjectSelectEffect != null)
                {
                    mapObjectSelectEffect.Destroy();
                }

                selectMyTroopEffectDic.Remove(objectId);
            }
        }

        private void removeAllSelectMyTroopEffect()
        {
            foreach (var mapObjectSelectEffect in selectMyTroopEffectDic.Values)
            {
                if (mapObjectSelectEffect != null)
                {
                    mapObjectSelectEffect.Destroy();
                }
            }
            selectMyTroopEffectDic.Clear();
        }

        public void refreshSelectMyTroopEffectActive(MapViewLevel level)
        {
            foreach (var effect in selectMyTroopEffectDic.Values)
            {
                if (effect != null)
                {
                    effect.SetActiveState(level <= MapViewLevel.Tactical);
                }
            }
        }

        #endregion

        #region 选中GameObject管理            

        private long selectRssID;
        private GameObject SelectEffect;
        private GameObject m_selectEffectFlow = null;        
        private ChangeSpriteColor m_ChangeColorHelper;
        
        private void CreateSelectEffect()
        {
            RemoveSelcetEffect();
            CoreUtils.assetService.Instantiate(RS.EnemySelectEffectName, (go1) =>
            {
                SelectEffect = go1;
                m_ChangeColorHelper = SelectEffect.transform.Find("m_UVflow003").GetComponent<ChangeSpriteColor>();
                SelectEffect.gameObject.SetActive(false);
                selectRssID = 0;
                m_selectEffectFlow = null;
            });
            InitSelectCityEffect();
            InitSelectAnimatorGo();
        }

        private void InitSelectCityEffect()
        {
            CoreUtils.assetService.Instantiate(RS.TroopmSelectAnimatorGo, (go) =>
            {
                go.transform.SetAsFirstSibling();
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                m_selectCityEffect = go;
                m_selectCityEffect.transform.localScale = Vector3.one;
                m_selectCityEffect.SetActive(false);
            });
        }

        private void InitSelectAnimatorGo()
        {
            CoreUtils.assetService.Instantiate(RS.TroopSelectEffectName, (go) =>
            {
                go.transform.SetAsFirstSibling();
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                m_SelectAnimatorGo = go;
                m_SelectAnimatorGo.transform.localScale = Vector3.one;
                m_SelectAnimatorGo.SetActive(false);
            });
            
        }

        private void RemoveSelcetEffect()
        {
            if (SelectEffect != null)
            {
                CoreUtils.assetService.Destroy(SelectEffect);
            }

            if (m_selectCityEffect != null)
            {
                CoreUtils.assetService.Destroy(m_selectCityEffect);
            }

            if (m_SelectAnimatorGo != null)
            {
                CoreUtils.assetService.Destroy(m_SelectAnimatorGo);
            }
        }

        private void SetTargetEffectScale(TouchTargetEfeectObjectType targetType, float targetRadis = 0)
        {
            SelectEffect.transform.localScale = TroopHelp.GetSelectEffectScale(targetType, targetRadis);
        }

        #endregion

        #region 双击逻辑

        private bool OnWorldDoubleClick(int objectId)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null)
            {
                return false;
            }

            if (!armyData.isPlayerHave)
            {
                return false;
            }

            if (armyData.isRally)
            {
                return false;
            }

            long localTime = ServerTimeModule.Instance.GetTicks() / 10000;
            long timeDif = localTime - m_lastClickObjectTime;
            if (m_lastClickObjectId == objectId &&
                timeDif >= 0 &&
                timeDif <= m_multiArmyDoubleClickTime)
            {
                List<int> selectViceArmyIndexList = new List<int>();
                SummonerTroopMgr.Instance.GetISummonerTroop().CalScreenViceArmList(m_selectArmId, ref selectViceArmyIndexList);
                foreach (var armyIndex in selectViceArmyIndexList)
                {
                    CreateSelectMyTroopEffect(armyIndex);
                }

                m_doubleSelectMainArmyIndex = m_selectArmId;
                m_doubleSelectViceArmyIndexList = selectViceArmyIndexList;

                if (m_doubleSelectArmyIndexList != null)
                {
                    m_doubleSelectArmyIndexList.Clear();
                }
                else
                {
                    m_doubleSelectArmyIndexList = new List<int>();
                }

                m_doubleSelectArmyIndexList.Add(m_selectArmId);
                foreach (var viceArmyIndex in selectViceArmyIndexList)
                {
                    m_doubleSelectArmyIndexList.Add(viceArmyIndex);
                }

                m_doubleFlag = true;
                m_lastClickObjectId = 0;
                m_lastClickObjectTime = 0;

                List<int> viceArmObjectIdList = new List<int>();
                foreach (var armyIndex in selectViceArmyIndexList)
                {
                    ArmyData viceArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndex);
                    if (viceArmyData == null) continue;

                    Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(viceArmyData.objectId) as Troops;
                    if (formation == null) continue;

                    viceArmObjectIdList.Add(viceArmyData.objectId);
                }

                TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                touchTroopInfo.mainArmObjectId = objectId;
                touchTroopInfo.viceArmObjectIdList = viceArmObjectIdList;

                AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
                AppFacade.GetInstance().SendNotification(CmdConstant.OnOpenSelectDoubleTroop, m_doubleSelectArmyIndexList);

                return true;
            }
            else
            {
                m_lastClickObjectId = objectId;
                m_lastClickObjectTime = localTime;

                return false;
            }
        }

        private void OnUIDoubleClick(int armyIndex)
        {
            ArmyData clickArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndex);
            if (clickArmyData == null)
            {
                return;
            }

            List<int> selectViceArmyIndexList = new List<int>();

            SummonerTroopMgr.Instance.GetISummonerTroop().CalWorldViceArmList(armyIndex, ref selectViceArmyIndexList);

            CreateSelectMyTroopEffect(armyIndex);
            foreach (var index in selectViceArmyIndexList)
            {
                CreateSelectMyTroopEffect(index);
            }

            m_doubleSelectMainArmyIndex = armyIndex;
            m_doubleSelectViceArmyIndexList = selectViceArmyIndexList;

            if (m_doubleSelectArmyIndexList != null)
            {
                m_doubleSelectArmyIndexList.Clear();
            }
            else
            {
                m_doubleSelectArmyIndexList = new List<int>();
            }

            m_doubleSelectArmyIndexList.Add(armyIndex);
            foreach (var viceArmyIndex in selectViceArmyIndexList)
            {
                m_doubleSelectArmyIndexList.Add(viceArmyIndex);
            }

            List<int> viceArmObjectIdList = new List<int>();
            foreach (var index in selectViceArmyIndexList)
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(index);
                if (armyData == null) continue;

                viceArmObjectIdList.Add(armyData.objectId);
            }

            TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
            touchTroopInfo.mainArmObjectId = clickArmyData.objectId;
            touchTroopInfo.viceArmObjectIdList = viceArmObjectIdList;

            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
            AppFacade.GetInstance().SendNotification(CmdConstant.OnOpenSelectDoubleTroop, m_doubleSelectArmyIndexList);
        }

        private void CheckExitDoubleClick()
        {
            if (m_doubleFlag == true)
            {
                m_doubleFlag = false;
            }
            else
            {
                if (m_doubleSelectMainArmyIndex != 0 &&
                    m_doubleSelectViceArmyIndexList != null &&
                    m_doubleSelectArmyIndexList != null)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseSelectMainTroop);

                    removeAllSelectMyTroopEffect();
                    if (!m_bDragMove)
                    {
                        CreateSelectMyTroopEffect(m_selectArmId);
                    }

                    AppFacade.GetInstance().SendNotification(CmdConstant.MapCloseDoubleSelectTroopHud);
                    if (!m_bDragMove)
                    {
                        TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                        touchTroopInfo.mainArmObjectId = m_selectObjectId;

                        AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
                    }

                    DeleteAllViceMoveLine();

                    m_doubleSelectMainArmyIndex = 0;
                    m_doubleSelectViceArmyIndexList.Clear();
                    m_doubleSelectViceArmyIndexList = null;
                    m_doubleSelectArmyIndexList.Clear();
                    m_doubleSelectArmyIndexList = null;                    
                }
            }
        }

        #endregion

        #region 3D事件管理

        private Vector2 m_startTouchPos ;
        private void OnTouche3DBegin(int x, int y, string parentName, string colliderName)
        {
            if (m_WorldMgrMediator == null)
            {
                m_WorldMgrMediator =
                    AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            }

            if (!m_WorldMgrMediator.IsWorldMapStateNormal())//迁城模式
            {
                return;
            }

             m_startTouchPos = new Vector2(x, y);          
            m_touchMoveCollideParent = parentName;
            m_touchMoveCollideName = colliderName;
            Debug.Log($"OnTouche3DBegin:{parentName}\t{colliderName}");
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return;
            MapViewLevel crrLevel = m_viewLevelMediator.GetViewLevel();
            if (crrLevel <= MapViewLevel.Tactical)
            {
                // 城内不能拖拽
                if (crrLevel > MapViewLevel.City)
                {
                    string[] strp = parentName.Split('_');
                    string[] citName = TroopHelp.GetOnClickTargetType(RssType.City);
                    if (citName.Contains(strp[0]))
                    {
                        if (strp[1] != "other")
                        {
                            SelectMyCity();
                        }
                    }

                    string[] rssItem = TroopHelp.GetOnClickTargetType(RssType.Stone);
                    if (rssItem.Contains(strp[0]))
                    {
                        int rssItemId = int.Parse(strp[1]);
                        SelectMyRssItem(rssItemId);
                    }

                    string[] guildName = TroopHelp.GetOnClickTargetType(RssType.GuildCenter);
                    if (guildName[0] == strp[0])
                    {
                        int rssItemId = int.Parse(strp[1]);
                        OnLongClickBuilding(rssItemId);
                    }
                }
                string[] str = colliderName.Split('_');
                string[] troopName = TroopHelp.GetOnClickTargetType(RssType.Troop);
                if (!troopName.Contains(str[0]))
                {
                    return;
                }

                int id = int.Parse(str[1]);
                if (GuideManager.Instance.IsGuideFightSecondBarbarian) //防止点击到引导部队
                {
                    return;
                }
                
                ArmyData army = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                if(army != null)
                {
                    m_selectObjectId = id;
                    if (army.isPlayerHave)
                    {
                        SelectTroopByArmId(army.troopId);
                    }
                }
            }
        }

        private void OnTouche3D(int x, int y, string parentName, string colliderName)
        {
            if (m_WorldMgrMediator == null)
            {
                m_WorldMgrMediator =
                    AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            }
            if (!m_WorldMgrMediator.IsWorldMapStateNormal())//迁城模式
            {
                return;
            }
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return;
            MapViewLevel crrLevel = m_viewLevelMediator.GetViewLevel();
            if (crrLevel > MapViewLevel.Tactical)
            {
                return;
            }

            if (m_bDragTroop && m_bDragMove || m_TouchPressUI)
            {
                return;
            }

            //判断是否点击到未解锁的迷雾
            FogSystemMediator fogMediator =
                AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            if (fogMediator != null)
            {
                if (fogMediator.IsClickUnlockFog(x, y))
                {
                    Debug.Log("点击到未解锁的迷雾");
                    return;
                }
            }
            
            Vector2  endTouchPos= new Vector2(x,y);
            if (Vector2.Distance(m_startTouchPos, endTouchPos) > 20)
            {
                return;                    
            }

            string[] strP = null;

            if (!string.IsNullOrEmpty(parentName))
            {
                strP = parentName.Split('_');
            }

            if (strP != null && strP.Length > 0 )
            {
                long id = 0;

                if (strP[0] != "Troops")
                {
                    if (strP.Length < 3)
                    {
                        id = Convert.ToInt64(strP[1]);
                        MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(id);
                        if (infoEntity != null)
                        {
                            WorldMapLogicMgr.Instance.MapSoundHandler.AddMapSound(infoEntity.rssType);
                        }
                    }
                    OnTouchUnSelect("", "");
                }

                if (strP[0] == "HolyLand" || strP[0] == "CheckPoint")
                {
                    CoreUtils.uiManager.ShowUI(UI.s_WorldObjectInfoBuild, null, id);
                    return;
                }

                if (strP[0] == "RssItem")
                {
                    m_RssProxy.OpenUI(parentName);
                    return;
                }

                if (strP[0] == "GuildBuild")
                {
                    CoreUtils.uiManager.ShowUI(UI.s_AllianceBuildInfoTip, null, id);
                    return;
                }

                if (strP[0] == "RuneItem")
                {
                    CoreUtils.uiManager.ShowUI(UI.s_pop_WorldTunes, null, id);
                    return;
                }

                if (strP[0] == "BarbarianWalled")
                {
                    string[] s = parentName.Split('_');
                    int barbarianWalledId = int.Parse(s[1]);
                    m_RssProxy.OpenMonsterUI(barbarianWalledId);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(colliderName))
            {
                string[] str = colliderName.Split('_');
                if (str.Length < 2)
                {
                    return;
                }
                int objectId = int.Parse(str[1]);
                MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(objectId);
                if (infoEntity != null)
                {
                    WorldMapLogicMgr.Instance.MapSoundHandler.AddMapSound(infoEntity.rssType);
                }
                
                switch (str[0])
                {
                    case "SummonBarbarianFormation":
                    case "GuardianFormation":
                    case "BarbarianFormation":
                        string[] s = colliderName.Split('_');
                        int mosterId = int.Parse(s[1]);
                        m_RssProxy.OpenMonsterUI(mosterId);
                        OnTouchUnSelect("", "");
                        break;
                    case "Formation":
                        if (GuideManager.Instance.IsGuideFightSecondBarbarian) //防止点击到引导部队
                        {
                            return;
                        }
                      
                        var data = m_worldProxy.GetWorldMapObjectByobjectId(objectId);
                        if (data != null)
                        {
                            if (data.objectType == (long) RssType.Transport && data.armyRid != m_PlayerProxy.CurrentRoleInfo.rid) // 他人运输车无法点击
                            {
                                return;
                            }
                            if(data.armyRid == m_PlayerProxy.CurrentRoleInfo.rid)
                            {
                                float dxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                                if (WorldCamera.Instance().getCurrentCameraDxf() < dxf)
                                {
                                    WorldCamera.Instance().SetCameraDxf(dxf, 500, null);
                                }
                            }
                        }

                        if (!OnWorldDoubleClick(objectId))
                        {
                            TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                            touchTroopInfo.mainArmObjectId = objectId;

                            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
                        }

                        break;
                }
            }
        }

        private void OnTouche3DEnd(int x, int y, string parentName, string colliderName)
        {
            if (m_WorldMgrMediator == null)
            {
                m_WorldMgrMediator = AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            }
            if (!m_WorldMgrMediator.IsWorldMapStateNormal())//迁城模式
            {
                return;
            }
            
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return;

            MapViewLevel crrLevel = m_viewLevelMediator.GetViewLevel();
            if (crrLevel <= MapViewLevel.Tactical)
            {
                parentName = m_touchMoveCollideParent;
                colliderName = m_touchMoveCollideName;                

                if (!m_TouchPressUI)
                {
                    OnTouchSelect(parentName, colliderName);
                }

                CheckExitDoubleClick();

                OnTouchUnSelect(parentName, colliderName);
                if (m_SelectAnimatorGo != null)
                {
                    m_SelectAnimatorGo.gameObject.SetActive(false);
                }
                
                if (m_selectCityEffect != null)
                {
                    m_selectCityEffect.gameObject.SetActive(false);
                    if (timerTouchCityTwo != null)
                    {                        
                        timerTouchCityTwo.Cancel();
                    }
                }
                isSelectTouchEnd = true;
            }
       
            m_touchMoveCollideParent = string.Empty;
            m_touchMoveCollideName = string.Empty;
        }

        private void OnTouche3DReleaseOutside(int x, int y, string parentName, string colliderName)
        {
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return;
            if (m_viewLevelMediator.GetViewLevel() > MapViewLevel.Tactical)
            {
                return;
            }

            if(!string.IsNullOrEmpty(m_touchMoveCollideName))
            {
                return;
            }

            bool isClick3D = ClickMoveHandle(new Vector2(x, y));
            if (isClick3D)
            {
                Debug.LogWarning("检测到3d物体，不可发送向空地行军");
                return;
            }

            if (isSelectMyTroop)
            {

                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(m_selectArmId);

                if (armyData == null || !armyData.isPlayerHave)
                {
                    return;
                }
                if (armyData.isRally)
                {
                    Debug.LogWarning("集结部队无法操作");
                    return;
                }

                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                {
                    Debug.LogWarning("溃败状态无法操作");
                    return;
                }

                if (armyData.troopType == RssType.Scouts)
                {
                    Debug.LogWarning("斥候部队不可以拖动操作");
                    return;
                }
            }

            Vector3 v3 = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
            if (isSelectMyCity)
            {
                OpenCreateTroopPanel(v3.x, v3.z);
            }
            else if (isSelectMyRssItem)
            {
                int mainArmyIndex;
                List<int> viceArmyIndexList;
                GetStationArmy(curSelectRssItemId, out mainArmyIndex, out viceArmyIndexList);

                TroopPosInfo troopPosInfo = new TroopPosInfo();
                troopPosInfo.x = (long) v3.x * 100;
                troopPosInfo.y = (long) v3.z * 100;

                if (mainArmyIndex > 0)
                {            
                    m_TroopProxy.TroopMapMarCh(mainArmyIndex, TroopAttackType.Space, 0, troopPosInfo, viceArmyIndexList); 
                }
            }
            else if (isSelectMyTroop)
            {
                OnTouchTroopMarchBySpace(v3);
            }
            
            WorldMapLogicMgr.Instance.MapTouchHandler.StopSpace();
        }

        private void OpenCreateTroopPanel(float x, float y)
        {
            if (isSelectMyCity)
            {
                Vector2 pos = new Vector2(x, y);
                m_TroopProxy.SetTroopMoveSpacePos(pos);
                FightHelper.Instance.OpenCreateArmyPanel(0);
            }
        }

        private void OnTouchTroopMarchBySpace(Vector3 v3)
        {
            if (m_selectArmId != 0)
            {
                TroopPosInfo troopPosInfo = new TroopPosInfo();
                troopPosInfo.x = PosHelper.ClientUnitToServerUnit(v3.x);
                troopPosInfo.y = PosHelper.ClientUnitToServerUnit(v3.z);
                m_TroopProxy.TroopMapMarCh(m_selectArmId, TroopAttackType.Space, 0, troopPosInfo, m_doubleSelectArmyIndexList);
                WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(0);
                AppFacade.GetInstance().SendNotification(CmdConstant.OnTouchMoveUITroopCallBack);
            }
        }


        private void OnTouchSelect(string parentName, string colliderName)
        {
            if (!string.IsNullOrEmpty(parentName))
            {
                string[] str = parentName.Split('_');
                string[] buildName = TroopHelp.GetOnClickTargetType(RssType.City);
                if (buildName.Contains(str[0]))
                {
                    if (str[1] == "other")
                    {
                        int id = int.Parse(str[2]);
                        OnTouchOtherCity(id);
                    }
                    else
                    {
                        OnTouchCity();
                    }
                }
                else
                {
                    if (str[0] == "RssItem")
                    {
                        OnTouchRssItem(parentName);
                    }
                    else if (str[0] == "RuneItem")
                    {
                        OnTouchRuneItem(parentName);
                    }
                }
                OnTouchAllianceBuilding(parentName);
            }

            if (!string.IsNullOrEmpty(colliderName))
            {
                OnTouchMonster(colliderName);
                OnTouchTroop(colliderName);              
            }
        }

        private void OnTouchUnSelect(string parentName, string colliderName)
        {
            m_TouchPressUI = false;
            RemoveRssHUD();
            StopDragAndMove();
            DeleteAllMoveLine();

            if (m_autoMoveTimer != null)
            {
                Timer.Cancel(m_autoMoveTimer);
                m_autoMoveTimer = null;
            }

            RemoveAllDraw();
            if (string.IsNullOrEmpty(parentName) && string.IsNullOrEmpty(colliderName))
            {
                if (m_selectObjectId != 0)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveTroopSelectHud, m_selectObjectId);
                }
                else
                {
                    ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(m_selectArmId);
                    if (armyData != null && armyData.objectId != 0)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveTroopSelectHud, armyData.objectId);
                    }
                }
                removeAllSelectMyTroopEffect();
                UnSelectEffect();
                AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseSelectMainTroop);
            }

            UnSelectCity();
        }


        private void OnTouchCity()
        {
            isSelectMyCity = false;
            if (timerTouchCity != null)
            {
                timerTouchCity.Cancel();
            }

            if (m_bDragTroop && m_bDragMove)
            {
                if (isSelectMyTroop)
                {
                    ArmyData data = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(m_selectArmId);
                    if (data != null)
                    {
                        if (!TroopHelp.IsHaveState((long) data.armyStatus, ArmyStatus.RETREAT_MARCH))
                        {
                            m_TroopProxy.TroopMapMarCh(data.troopId, TroopAttackType.Retreat, 0, null, m_doubleSelectArmyIndexList);
                            WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(m_selectObjectId);
                            AppFacade.GetInstance().SendNotification(CmdConstant.OnTouchMoveUITroopCallBack);
                        }
                        else
                        {
                            Debug.LogWarning("当前部队已经是撤退行军状态了");
                        }
                    }
                }
                else if (isSelectMyRssItem)
                {
                    int mainArmyIndex;
                    List<int> viceArmyIndexList;
                    GetStationArmy(curSelectRssItemId, out mainArmyIndex, out viceArmyIndexList);

                    if (mainArmyIndex > 0)
                    {
                        m_TroopProxy.TroopMapMarCh(mainArmyIndex, TroopAttackType.Retreat, 0, null, viceArmyIndexList);
                    }                    
                }
            }
        }


        private void OnTouchOtherCity(int id)
        {
            if (m_bDragTroop && m_bDragMove)
            {
                MapObjectInfoEntity mapObjectInfoEntity = m_worldProxy.GetWorldMapObjectByRid(id);
                if (mapObjectInfoEntity == null)
                {
                    return;
                }
                
                bool isGuild = false;
                if (mapObjectInfoEntity.guildId == 0 && m_PlayerProxy.CurrentRoleInfo.guildId == 0)
                {
                    isGuild = false;
                }
                else
                {
                    isGuild = mapObjectInfoEntity.guildId == m_PlayerProxy.CurrentRoleInfo.guildId;
                }

                if (isSelectMyTroop)
                {
                    ArmyData data = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(m_selectArmId);
                    if (data != null)
                    {
                        if (isGuild)
                        {
                            m_RallyTroopsProxy.SendReinforeRallyByArmyIndex(mapObjectInfoEntity.objectId, data.troopId, m_doubleSelectArmyIndexList);
                            WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(m_selectObjectId);
                        }
                        else
                        {
                            if (!TroopHelp.IsHaveState((long) data.armyStatus, ArmyStatus.FAILED_MARCH))
                            {
                                m_TroopProxy.TroopMapMarCh(data.troopId, TroopAttackType.Attack,
                                    (int) mapObjectInfoEntity.objectId, null, m_doubleSelectArmyIndexList);
                                WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(m_selectObjectId);
                            }
                            else
                            {
                                Debug.LogWarning("当前部队已经是撤退行军状态了");
                            }
                        }
                        AppFacade.GetInstance().SendNotification(CmdConstant.OnTouchMoveUITroopCallBack);
                    }
                }
                else if (isSelectMyRssItem)
                {                                    
                    if (!isGuild)
                    {
                        int mainArmyIndex;
                        List<int> viceArmyIndexList;
                        GetStationArmy(curSelectRssItemId, out mainArmyIndex, out viceArmyIndexList);

                        if (mainArmyIndex > 0)
                        {
                            m_TroopProxy.TroopMapMarCh(mainArmyIndex, TroopAttackType.Attack, (int)mapObjectInfoEntity.objectId, null, viceArmyIndexList);
                        }                        
                    }                    
                }
                else if (isSelectMyCity)
                {
                    if (isGuild)
                    {
                        FightHelper.Instance.Reinfore((int) mapObjectInfoEntity.objectId,0,(int) mapObjectInfoEntity.objectId);
                    }
                    else
                    {
                        if (m_TroopProxy.CheckAttackOtherCity((int) mapObjectInfoEntity.objectId))
                        {
                            return;
                        }
                        
                        if (!m_RallyTroopsProxy.IsWasFever((int) mapObjectInfoEntity.objectId, OpenPanelType.Common))
                        {
                            return;
                        }

                        if (m_TroopProxy.CheckAttackMyBuff())
                        {
                            TroopHelp.ShowCheckCityBuffPanel(() =>
                            {
                                FightHelper.Instance.OpenCreateArmyPanel((int) mapObjectInfoEntity.objectId);
                            });
                            return;
                        }
                        FightHelper.Instance.OpenCreateArmyPanel((int) mapObjectInfoEntity.objectId);
                    }              
                }
            }
        }

        private void OnTouchRssItem(string parentName)
        {
            if (m_bDragTroop && m_bDragMove)
            {
                string[] s = parentName.Split('_');
                int rssId = int.Parse(s[1]);
                MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                if (rssData != null)
                {
                    if (TroopHelp.IsNoTouchType(rssData.rssType))
                    {
                        Debug.LogWarning("当前类型不可以发协议");
                        return;
                    }
                    
                    var selectTroopPos = GetSelectTroopPos();
                    float radius = m_ConfigDefine.cityRadius;
                    Vector3 v2 = (rssData.gameobject.transform.position - selectTroopPos).normalized *
                                 radius;
                    Vector2 v3 = new Vector2(rssData.gameobject.transform.position.x,
                        rssData.gameobject.transform.position.z);
                    Vector2 v1 = new Vector2(v2.x, v2.z);
                    Vector2 v4 = v3 - v1;
                    TroopPosInfo troopPosInfo = new TroopPosInfo();
                    troopPosInfo.x = (long) v4.x * 100;
                    troopPosInfo.y = (long) v4.y * 100;

                    if (isSelectMyTroop)
                    {
                        CheckCollectOrAttack(rssData, m_selectArmId, rssId, troopPosInfo, m_doubleSelectArmyIndexList);
                        WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(m_selectObjectId);
                    }
                    else if (isSelectMyCity)
                    {
                        bool isCollect = m_TroopProxy.GetIsCollectRssItem(rssId);
                        if (isCollect)
                        {
                            if (rssData.collectRid != 0)
                            {                              
                                FightHelper.Instance.Attack((int) rssData.objectId);
                            }
                            else
                            {
                                FightHelper.Instance.OpenCreateArmyPanel((int) rssData.objectId);
                            }
                        }
                    }
                    else if (isSelectMyRssItem)
                    {
                        int mainArmyIndex;
                        List<int> viceArmyIndexList;
                        GetStationArmy(curSelectRssItemId, out mainArmyIndex, out viceArmyIndexList);

                        if (mainArmyIndex > 0)
                        {
                            CheckCollectOrAttack(rssData, mainArmyIndex, rssId, troopPosInfo, viceArmyIndexList);
                        }                        
                    }
                }
            }
        }

        private void OnTouchRuneItem(string parentName)
        {
            if (m_bDragTroop && m_bDragMove)
            {
                string[] s = parentName.Split('_');
                int rssId = int.Parse(s[1]);
                MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                if (rssData != null)
                {
                    TroopPosInfo troopPosInfo = new TroopPosInfo();
                    troopPosInfo.x = rssData.objectPos.x;
                    troopPosInfo.y = rssData.objectPos.y;
                    if (isSelectMyTroop)
                    {
                        CheckCollectOrAttack(rssData, m_selectArmId, rssId, troopPosInfo, m_doubleSelectArmyIndexList);
                    }
                    else if (isSelectMyCity)
                    {
                        FightHelper.Instance.OpenCreateArmyPanel((int) rssData.objectId);
                        WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(m_selectObjectId);
                    }
                    else if (isSelectMyRssItem)
                    {
                        int mainArmyIndex;
                        List<int> viceArmyIndexList;
                        GetStationArmy(curSelectRssItemId, out mainArmyIndex, out viceArmyIndexList);

                        if (mainArmyIndex > 0)
                        {
                            CheckCollectOrAttack(rssData, mainArmyIndex, rssId, troopPosInfo, viceArmyIndexList);
                        }
                    }
                }
            }
        }

        private void CheckCollectOrAttack(MapObjectInfoEntity rssData, int armyIndex, int rssId, TroopPosInfo troopPosInfo, List<int> armyIndexList = null)
        {
            if (rssData.collectRid != 0)
            {
                if (rssData.collectRid != m_PlayerProxy.CurrentRoleInfo.rid)
                {
                    bool isGuild = false;
                    if (rssData.guildId == 0 && m_PlayerProxy.CurrentRoleInfo.guildId == 0)
                    {
                        isGuild = false;
                    }
                    else
                    {
                        isGuild = rssData.guildId == m_PlayerProxy.CurrentRoleInfo.guildId;
                    }

                    if (isGuild)
                    {
                        m_TroopProxy.TroopMapMarCh(armyIndex, TroopAttackType.Space, 0, troopPosInfo, armyIndexList);
                    }
                    else
                    {                     
                        m_TroopProxy.TroopMapMarCh(armyIndex, TroopAttackType.Attack, rssId, troopPosInfo, armyIndexList);
                    }
                }
                else
                {
                    //1.多选部队，逻辑上不会选中资源点中的部队 
                    //2.单选部队，该部队不能在目标资源点中 
                    if (rssData.armyIndex != armyIndex)
                    {                      
                        m_TroopProxy.TroopMapMarCh(armyIndex, TroopAttackType.Space, 0, troopPosInfo, armyIndexList);
                    }

                }
            }
            else
            {
                m_TroopProxy.TroopMapMarCh(armyIndex, TroopAttackType.Collect, rssId, troopPosInfo, armyIndexList);
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.OnTouchMoveUITroopCallBack);
        }


        private void OnTouchMonster(string colliderName)
        {
            if (!m_bDragTroop && !m_bDragMove)
            {
                return;
            }

            string[] str = colliderName.Split('_');
            string[] monsterName = TroopHelp.GetOnClickTargetType(RssType.Monster);

            if (monsterName.Contains(str[0]))
            {
                int id = int.Parse(str[1]);
                MapObjectInfoEntity monsterData = m_worldProxy.GetWorldMapObjectByobjectId(id);
                if (monsterData != null)
                {
                    Vector2 v2 = new Vector2(monsterData.gameobject.transform.position.x, monsterData.gameobject.transform.position.z);
                    if (m_viewLevelMediator.IsLodVisable(v2.x, v2.y))
                    {
                        if (monsterData.monsterDefine == null) return;

                        if (monsterData.objectType == (long)RssType.SummonConcentrateMonster)
                        {
                            return;
                        }

                        if (monsterData.objectType == (long)RssType.Monster)
                        {
                            if (monsterData.monsterDefine.level > m_PlayerProxy.CurrentRoleInfo.barbarianLevel + 1)
                            {
                                Tip.CreateTip(LanguageUtils.getTextFormat(500201, m_PlayerProxy.CurrentRoleInfo.barbarianLevel + 1), Tip.TipStyle.Middle).Show();
                                return;
                            }
                        }
                        
                        if (isSelectMyTroop)
                        {
                            int count = 1;

                            if (m_doubleSelectArmyIndexList != null)
                            {
                                count = m_doubleSelectArmyIndexList.Count;
                            }

                            int costAP = monsterData.monsterDefine.costAP * count;

                            if (monsterData.objectType == (long)RssType.Monster)
                            {
                                if (costAP > 0 && m_PlayerProxy.CurrentRoleInfo.actionForce < costAP)
                                {
                                    PlayerDataHelp.ShowActionUI(costAP);
                                    return;
                                }
                            }
                            if (monsterData.objectType == (long)RssType.SummonAttackMonster)
                            {
                                if (costAP > 0 && m_PlayerProxy.CurrentRoleInfo.actionForce < costAP)
                                {
                                    PlayerDataHelp.ShowActionUI(costAP);
                                    return;
                                }
                            }

                            m_TroopProxy.TroopMapMarCh(m_selectArmId, TroopAttackType.Attack, id, null, m_doubleSelectArmyIndexList);
                            WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(m_selectObjectId);
                            AppFacade.GetInstance().SendNotification(CmdConstant.OnTouchMoveUITroopCallBack);
                        }
                        else if (isSelectMyCity)
                        {
                            if (monsterData.objectType == (long)RssType.Monster)
                            {
                                if (monsterData.monsterDefine.costAP > 0 && m_PlayerProxy.CurrentRoleInfo.actionForce < monsterData.monsterDefine.costAP)
                                {
                                    PlayerDataHelp.ShowActionUI(monsterData.monsterDefine.costAP);
                                    return;
                                }
                            }
                            if (monsterData.objectType == (long)RssType.SummonAttackMonster)
                            {
                                if (monsterData.monsterDefine.costAP > 0 && m_PlayerProxy.CurrentRoleInfo.actionForce < monsterData.monsterDefine.costAP)
                                {
                                    PlayerDataHelp.ShowActionUI(monsterData.monsterDefine.costAP);
                                    return;
                                }
                            }

                            FightHelper.Instance.OpenCreateArmyPanel((int) monsterData.objectId);
                        }
                        else if (isSelectMyRssItem)
                        {
                            int mainArmyIndex;
                            List<int> viceArmyIndexList;
                            GetStationArmy(curSelectRssItemId, out mainArmyIndex, out viceArmyIndexList);

                            int count = 1;

                            if (viceArmyIndexList != null)
                            {
                                count = viceArmyIndexList.Count;
                            }

                            int costAP = monsterData.monsterDefine.costAP * count;

                            if (monsterData.objectType == (long)RssType.Monster)
                            {
                                if (costAP > 0 && m_PlayerProxy.CurrentRoleInfo.actionForce < costAP)
                                {
                                    PlayerDataHelp.ShowActionUI(costAP);
                                    return;
                                }
                            }
                            if (monsterData.objectType == (long)RssType.SummonAttackMonster)
                            {
                                if (costAP > 0 && m_PlayerProxy.CurrentRoleInfo.actionForce < costAP)
                                {
                                    PlayerDataHelp.ShowActionUI(costAP);
                                    return;
                                }
                            }

                            if (mainArmyIndex > 0)
                            {
                                m_TroopProxy.TroopMapMarCh(mainArmyIndex, TroopAttackType.Attack, id, null, viceArmyIndexList);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("当前自由行军目的在迷雾里"); 
                    }
                }
            }
        }

        private void OnTouchTroop(string colliderName)
        {
            string[] str1 = colliderName.Split('_');
            string[] troopName = TroopHelp.GetOnClickTargetType(RssType.Troop);
            if (troopName.Contains(str1[0]))
            {
                if (m_bDragTroop && m_bDragMove)
                {
                    int id = int.Parse(str1[1]);
                    ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                    if (armyData == null)
                    {
                        return;
                    }
                    if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                    {
                        return;
                    }

                    if (isSelectMyTroop)
                    {
                        bool isGuild = m_PlayerProxy.CurrentRoleInfo.guildId != 0 &&
                                       m_PlayerProxy.CurrentRoleInfo.guildId == armyData.guild;
                        if (armyData != null && armyData.isRally)
                        {
                            if (isGuild)
                            {
                                if (m_RallyTroopsProxy.IsJoinRallyCheck(armyData.armyRid))
                                {
                                    return;
                                } 
                            }
                        }
                      
                        TouchTroopMapMarch(armyData, m_selectArmId, m_doubleSelectArmyIndexList);
                    }
                    else if (isSelectMyCity)
                    {
                        if (armyData != null)
                        {
                            bool isGuild = m_PlayerProxy.CurrentRoleInfo.guildId != 0 &&
                                           m_PlayerProxy.CurrentRoleInfo.guildId == armyData.guild;
                            if (!isGuild|| armyData.isPlayerHave)
                            {
                                if (!armyData.isPlayerHave)
                                {
                                    if (!m_RallyTroopsProxy.IsWasFever(armyData.objectId, OpenPanelType.Common))
                                    {
                                        return;
                                    }
                                }

                                FightHelper.Instance.OpenCreateArmyPanel(armyData.objectId);   
                            }
                            else if (isGuild && armyData.isRally)
                            {
                                FightHelper.Instance.ReinforeTroop(armyData.objectId,0,armyData.objectId);
                            }
                            else
                            {
                                FightHelper.Instance.OpenCreateArmyPanel(armyData.objectId);
                            }
                        }
                    }
                    else if (isSelectMyRssItem)
                    {
                        bool isGuild = m_PlayerProxy.CurrentRoleInfo.guildId != 0 &&
                                       m_PlayerProxy.CurrentRoleInfo.guildId == armyData.guild;
                        if (armyData != null && armyData.isRally)
                        {
                            if (isGuild)
                            {
                                if (m_RallyTroopsProxy.IsJoinRallyCheck(armyData.armyRid))
                                {
                                    return;
                                }
                            }
                        }

                        int mainArmyIndex;
                        List<int> viceArmyIndexList;
                        GetStationArmy(curSelectRssItemId, out mainArmyIndex, out viceArmyIndexList);

                        if (mainArmyIndex > 0)
                        {
                            TouchTroopMapMarch(armyData, mainArmyIndex, viceArmyIndexList);
                        }
                    }
                }
            }
        }

        //拖到联盟建筑发协议

        private void OnTouchAllianceBuilding(string colliderName)
        {
            string[] str1 = colliderName.Split('_');
            string[] guildName = TroopHelp.GetOnClickTargetType(RssType.GuildCenter);
            if (guildName.Contains(str1[0]))
            {
                if (m_bDragTroop && m_bDragMove)
                {
                    long objectId = int.Parse(str1[1]);
                    MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(objectId);

                    if (infoEntity == null) return;

                    var type = m_allianceProxy.GetBuildServerTypeToConfigType((int)infoEntity.rssType);
                    AllianceBuildingTypeDefine allianceBuildingTypeDefine = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);
                    if (allianceBuildingTypeDefine != null)
                    {
                        TroopHelp.m_ToouchAllianceName = allianceBuildingTypeDefine.l_nameId;
                    }

                    bool isGuild = m_PlayerProxy.CurrentRoleInfo.guildId != 0 && m_PlayerProxy.CurrentRoleInfo.guildId == infoEntity.guildId;
                    if (isSelectMyTroop || isSelectMyRssItem)
                    {
                        //圣地 关卡
                        if (!isGuild &&
                            (infoEntity.rssType == RssType.CheckPoint || infoEntity.rssType == RssType.HolyLand) &&
                            FightHelper.Instance.IsStrongHolyCanProtecting(infoEntity))
                        {
                            Tip.CreateTip(730264).Show();
                            return;
                        }

                        WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(m_selectObjectId);
                        var selectTroopPos = GetSelectTroopPos();
                        float radius = m_ConfigDefine.cityRadius;
                        Vector3 v2 = (infoEntity.gameobject.transform.position - selectTroopPos).normalized * radius;
                        Vector2 v3 = new Vector2(infoEntity.gameobject.transform.position.x, infoEntity.gameobject.transform.position.z);
                        Vector2 v1 = new Vector2(v2.x, v2.z);
                        Vector2 v4 = v3 - v1;
                        TroopPosInfo troopPosInfo = new TroopPosInfo();
                        troopPosInfo.x = (long)v4.x * 100;
                        troopPosInfo.y = (long)v4.y * 100;

                        if (isSelectMyTroop)
                        {
                            if (TroopHelp.IsTouchMoveAllianceBuilding(infoEntity.rssType))
                            {
                                m_TroopProxy.TroopMapMarCh(m_selectArmId, TroopAttackType.Space,
                                    (int)infoEntity.objectId, troopPosInfo, m_doubleSelectArmyIndexList);
                            }
                            else
                            {
                                if (isGuild)
                                {
                                    //1.多选部队，逻辑上不会选中建筑中的部队
                                    //2.单选部队，该部队不能在目标建筑中
                                    if (!m_TroopProxy.IsHaveArmyIndex(infoEntity.objectId, m_selectArmId))
                                    {
                                        m_RallyTroopsProxy.SendReinforeRallyByArmyIndex(infoEntity.objectId, m_selectArmId, m_doubleSelectArmyIndexList);
                                    }
                                }
                                else
                                {
                                    if (TroopHelp.IsTouchGuildRss(infoEntity.rssType))
                                    {
                                        m_TroopProxy.TroopMapMarCh(m_selectArmId, TroopAttackType.Space,
                                            (int)infoEntity.objectId, troopPosInfo, m_doubleSelectArmyIndexList);
                                    }
                                    else
                                    {
                                        m_TroopProxy.TroopMapMarCh(m_selectArmId, TroopAttackType.Attack,
                                            (int)infoEntity.objectId, null, m_doubleSelectArmyIndexList);
                                    }
                                }
                            }
                        }
                        else if (isSelectMyRssItem)
                        {
                            int mainArmyIndex;
                            List<int> viceArmyIndexList;
                            GetStationArmy(curSelectRssItemId, out mainArmyIndex, out viceArmyIndexList);

                            if (TroopHelp.IsTouchMoveAllianceBuilding(infoEntity.rssType))
                            {
                                m_TroopProxy.TroopMapMarCh(mainArmyIndex, TroopAttackType.Space, 0, troopPosInfo, viceArmyIndexList);
                            }
                            else
                            {
                                if (isGuild)
                                {
                                    m_RallyTroopsProxy.SendReinforeRallyByArmyIndex(infoEntity.objectId, mainArmyIndex, viceArmyIndexList);
                                }
                                else
                                {
                                    if (TroopHelp.IsTouchGuildRss(infoEntity.rssType))
                                    {
                                        m_TroopProxy.TroopMapMarCh(mainArmyIndex, TroopAttackType.Space, 0, troopPosInfo, viceArmyIndexList);
                                    }
                                    else
                                    {
                                        m_TroopProxy.TroopMapMarCh(mainArmyIndex, TroopAttackType.Attack, (int)infoEntity.objectId, null, viceArmyIndexList);
                                    }
                                }
                            }
                        }

                        AppFacade.GetInstance().SendNotification(CmdConstant.OnTouchMoveUITroopCallBack);
                    }
                    else if (isSelectMyCity)
                    {
                        if (TroopHelp.IsTouchMoveAllianceBuilding(infoEntity.rssType))
                        {
                            FightHelper.Instance.OpenCreateArmyPanel((int)infoEntity.objectId);
                        }
                        else
                        {
                            if (isGuild)
                            {
                                FightHelper.Instance.ReinforeTroop((int)infoEntity.objectId, 0, (int)infoEntity.objectId, 0, 0, true, GetTroopReinforeNum(infoEntity));
                            }
                            else
                            {
                                //其他联盟的联盟中心。需要向空地行军
                                if (TroopHelp.IsTouchGuildRss(infoEntity.rssType))
                                {
                                    FightHelper.Instance.OpenCreateArmyPanel((int)infoEntity.objectId);
                                }
                                else
                                {
                                    FightHelper.Instance.Attack((int)infoEntity.objectId);
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<AllianceBuildArmyLevel> m_armysList = new List<AllianceBuildArmyLevel>();
        private long GetTroopReinforeNum(MapObjectInfoEntity infoEntity)
        {
            long num = 0;
            int m_buildType = m_allianceProxy.GetBuildServerTypeToConfigType(infoEntity.objectType);
            m_armysList.Clear();
            List<AllianceBuildArmyLevel> initdatas;
            bool canCreateArmy = false;
            long m_armyCount = 0;
            AllianceBuildingTypeDefine m_buildTypeConfig =
                CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(m_buildType);
            if (m_buildTypeConfig != null)
            {
                RssType rssType = (RssType) infoEntity.objectType;
                if (rssType == RssType.HolyLand || rssType == RssType.CheckPoint)
                {
                    initdatas = m_allianceProxy.GetBuildArmyInHolyLand(infoEntity.objectId);
                    canCreateArmy = m_allianceProxy.GetMyArmyCountInHolyLand(infoEntity.objectId) <
                                    m_TroopProxy.GetTroopDispatchNum();
                }
                else
                {
                    initdatas = m_allianceProxy.GetBuildArmy(infoEntity.objectId);
                    canCreateArmy = !m_allianceProxy.HasMyArmyInBuild(infoEntity.objectId);

                    if (infoEntity.guildBuildStatus != (long) GuildBuildState.building &&
                        infoEntity.rssType < RssType.GuildFoodResCenter)
                    {
                        canCreateArmy = true;
                    }
                }

                if (canCreateArmy)
                {
                    AllianceBuildArmyLevel  myCreate = new AllianceBuildArmyLevel();
                    myCreate.prefab_index = 0;
                    m_armysList.Add(myCreate);
                }


                if (initdatas != null)
                {
                    m_armysList.AddRange(initdatas);
                }

                m_armyCount = 0;
                foreach (var army in m_armysList)
                {
                    army.isSelected = false;
                    m_armyCount += army.armyCount;
                }

              num = m_buildTypeConfig.armyCntLimit - m_armyCount;
            }
            return num;
        }



        private void TouchTroopMapMarch(ArmyData armyData, int selectArmyIndex, List<int> selectArmyIndexList = null)
        {
            ArmyData data = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(selectArmyIndex);
            if (armyData != null && data != null && data != armyData)
            {
                var selectTroopPos = GetSelectTroopPos();
                Vector3 v2 = (armyData.go.transform.position - selectTroopPos).normalized *
                             armyData.armyRadius;
                Vector2 v3 = new Vector2(armyData.go.transform.position.x,
                    armyData.go.transform.position.z);
                Vector2 v1 = new Vector2(v2.x, v2.z);
                Vector2 v4 = v3 - v1;
                TroopPosInfo troopPosInfo = new TroopPosInfo();
                troopPosInfo.x = (long) v4.x * 100;
                troopPosInfo.y = (long) v4.y * 100;
                
                bool isGuild = m_PlayerProxy.CurrentRoleInfo.guildId != 0 &&
                               m_PlayerProxy.CurrentRoleInfo.guildId == armyData.guild;
                if (armyData.isRally)
                {
                    if (isGuild)
                    {
                        m_RallyTroopsProxy.SendReinforeRallyByArmyIndex(armyData.objectId, data.troopId, selectArmyIndexList);
                    }
                    else
                    {
                        m_TroopProxy.TroopMapMarCh(data.troopId, TroopAttackType.Attack, armyData.objectId, null, selectArmyIndexList);
                    }
                }
                else
                {
                    if (armyData.isPlayerHave)
                    {
                        m_TroopProxy.TroopMapMarCh(data.troopId, TroopAttackType.Space, 0, troopPosInfo, selectArmyIndexList);
                        
                        WorldMapLogicMgr.Instance.UpdateMapDataHandler.UpdateLastTroopId(m_selectObjectId);
                    }
                    else
                    {
                        if (isGuild)
                        {
                            m_TroopProxy.TroopMapMarCh(data.troopId, TroopAttackType.Space, 0, troopPosInfo, selectArmyIndexList);
                        }
                        else
                        {
                            m_TroopProxy.TroopMapMarCh(data.troopId, TroopAttackType.Attack, armyData.objectId, null, selectArmyIndexList);
                        }
                    }
                }

                AppFacade.GetInstance().SendNotification(CmdConstant.OnTouchMoveUITroopCallBack);
            }
        }

        private bool IsChickMove3D(Vector2 clickPos)
        {
            string collideParent = string.Empty;
            string collideName = string.Empty;
            int nIndex = -1;
            RaycastHit[] rayHits = CoreUtils.inputManager.RayCashHit3D((int)clickPos.x, (int)clickPos.y, out nIndex);
            if (nIndex != -1)
            {
                collideParent = rayHits[nIndex].transform.parent.name;
                collideName = rayHits[nIndex].transform.gameObject.name;
            }
            if (!string.IsNullOrEmpty(collideParent) || !string.IsNullOrEmpty(collideName))
            {
                return true;
            }

            return false;
        }


        private bool ClickMoveHandle(Vector2 clickPos)
        {
            string collideParent = string.Empty;
            string collideName = string.Empty;
            int nIndex = -1;
            RaycastHit[] rayHits = CoreUtils.inputManager.RayCashHit3D((int)clickPos.x, (int)clickPos.y, out nIndex);
            if (nIndex != -1)
            {
                collideParent = rayHits[nIndex].transform.parent.name;
                collideName = rayHits[nIndex].transform.gameObject.name;
            }
            if (!string.IsNullOrEmpty(collideName))
            {
                OnTouchMonster(collideName);
                OnTouchTroop(collideName);
            }

            if (!string.IsNullOrEmpty(collideParent))
            {
                OnTouchAllianceBuilding(collideParent);
            }

            if (!string.IsNullOrEmpty(collideParent) || !string.IsNullOrEmpty(collideName))
            {
                return true;
            }

            return false;
        }

        private void UnSelectCity()
        {
            if (m_selectCityEffect != null)
            {
                m_selectCityEffect.SetActive(false);
                isSelectMyCity = false;
                isSelectMyRssItem = false;
                curSelectRssItemId = 0;
                if (timerTouchRssItem != null)
                {
                    timerTouchRssItem.Cancel();
                }

                AppFacade.GetInstance().SendNotification(CmdConstant.MapTouchMyCity, false);
            }
        }


        private Timer playSelectEffectTimer;
        private void PlaySelectEffectAnimator(bool isTimes=false)
        {
            RemovePlaySelectEffectTimer();
            if (string.IsNullOrEmpty(m_touchMoveCollideParent) || string.IsNullOrEmpty(m_touchMoveCollideName))
            {
                if (SelectEffect!=null&&SelectEffect.gameObject.activeSelf)
                {
                    SelectEffect.gameObject.GetComponent<Animator>().Play("operation_2003_end");
                }
            }
            if (isTimes)
            {
                playSelectEffectTimer = Timer.Register(0.15f, () =>
                {
                    if (string.IsNullOrEmpty(m_touchMoveCollideParent) || string.IsNullOrEmpty(m_touchMoveCollideName))
                    {
                        if (SelectEffect != null)
                        {                           
                            SelectEffect.SetActive(false);
                        }

                        RemovePlaySelectEffectTimer();
                        WorldMapLogicMgr.Instance.MapTouchHandler.Stop();
                    }
                });
            }
            else
            {
                if (string.IsNullOrEmpty(m_touchMoveCollideParent) || string.IsNullOrEmpty(m_touchMoveCollideName))
                {
                    if (SelectEffect != null)
                    {
                        SelectEffect.SetActive(false);
                    }
                    WorldMapLogicMgr.Instance.MapTouchHandler.Stop();
                    WorldMapLogicMgr.Instance.MapTouchHandler.StopSpace();
                }
            }
        }

        private void RemovePlaySelectEffectTimer()
        {
            if (playSelectEffectTimer != null)
            {
                playSelectEffectTimer.Cancel();
            }
        }


        private void UnSelectEffect()
        {
            if (SelectEffect != null)
            {
                selectRssID = 0;
                isBar = false;
                isCity = false;
                isRss = false;
                isSelectMyTroop = false;
                m_touchMoveCollideParent = string.Empty;
                m_touchMoveCollideName = string.Empty;
                PlaySelectEffectAnimator();                             
                m_selectEffectFlow = null;
                if (m_selectObjectId != 0)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapOpenTroopHudScale, m_selectObjectId);
                }
                else
                {
                    ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(m_selectArmId);
                    if (armyData != null && armyData.objectId != 0)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapOpenTroopHudScale, armyData.objectId);
                    }
                }
                m_selectArmId = 0;
                m_selectObjectId = 0;
                m_touchNotTroopSelect = null;
            }
        }

        // 奇观建筑是否可被行军
        private bool IsStrongHolyCanProtecting(MapObjectInfoEntity mapObjectInfoEntity)
        {
            UI_Item_IconAndTime_SubView.BuildingState holyLandStatus = (UI_Item_IconAndTime_SubView.BuildingState)mapObjectInfoEntity.holyLandStatus;
            if (holyLandStatus == UI_Item_IconAndTime_SubView.BuildingState.NotUnlock ||
                holyLandStatus == UI_Item_IconAndTime_SubView.BuildingState.InitProtecting ||
                holyLandStatus == UI_Item_IconAndTime_SubView.BuildingState.Protecting)
            {
                return false;
            }            
            return true;
        }

        #endregion

        #region 选中map对象

        private TouchNotTroopSelect m_touchNotTroopSelect;
        
        private int m_selectArmId = 0;
        private int m_selectObjectId = 0;        
        private int curSelectRssItemId = 0;
        private bool isSelectMyCity = false;
        private bool isSelectMyTroop = false;
        private bool isSelectMyRssItem = false;
        private bool isSelectTouchEnd = false;      
        private Timer timerTouchCity;
        private Timer timerTouchCityTwo;
        private Timer timerTouchRssItem;
        private GameObject m_selectCityEffect;
        private GameObject m_SelectAnimatorGo;
        private string m_touchMoveCollideParent = string.Empty;
        private string m_touchMoveCollideName = string.Empty;

        private void SelectMyRssItem(int rssId)
        {
            if (!isSelectMyRssItem)
            {
                if (timerTouchRssItem != null)
                {
                    timerTouchRssItem.Cancel();
                }

                MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                Vector3 v3 =Vector3.zero;
                if (rssData != null)
                {  
                     v3 = new Vector3(rssData.objectPos.x / 100, 0, rssData.objectPos.y / 100);                   
                }

                isSelectTouchEnd = false;
                timerTouchRssItem = Timer.Register(m_OperatingHaloTime, () =>
                {
                    if (m_selectCityEffect != null)
                    {
                        if (rssData != null)
                        {
                            if (rssData.collectRid == m_PlayerProxy.CurrentRoleInfo.rid)
                            {
                                curSelectRssItemId = rssId;
                                m_selectCityEffect.SetActive(true);
                                m_selectCityEffect.transform.DOLocalMove(v3, 0);
                                m_selectCityEffect.transform.DOScale(TroopHelp.GetSelectEffectScale(TouchTargetEfeectObjectType.Resource), 0);
                                isSelectMyCity = false;
                                isSelectMyTroop = false;
                                isSelectMyRssItem = true;                           
                                WorldCamera.Instance().SetCanDrag(false);
                                if (!m_bDragTroop)
                                {
                                    m_bDragTroop = true;
                                }
                                CreateMoveLine();
                                if (timerTouchRssItem != null)
                                {
                                    timerTouchRssItem.Cancel();
                                }

                                OnLongTouchBuilding(v3,TouchTargetEfeectObjectType.Resource);
                                if (isSelectTouchEnd)
                                {
                                    m_selectCityEffect.SetActive(false);
                                    m_bDragTroop = false;
                                  
                                }
                            }
                        }
                    }
                }, (v) =>
                {
                });
            }
        }

        /// <summary>
        /// 长按主城
        /// </summary>
        private void SelectMyCity()
        {
            if (m_WorldMgrMediator == null)
            {
                m_WorldMgrMediator =
                    AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            }

            if (!m_WorldMgrMediator.IsWorldMapStateNormal())//迁城模式
            {
                return;
            }

            if (m_CityBuildingProxy.LockMoveEvent)//迁城动画正在表现
            {
                return;
            }

            if (!isSelectMyCity)
            {
                if (timerTouchCity != null)
                {
                    timerTouchCity.Cancel();
                }
                Vector3 v3 = new Vector3(m_CityBuildingProxy.RolePos.x, 0, m_CityBuildingProxy.RolePos.y);
                isSelectTouchEnd = false;
                timerTouchCity = Timer.Register(m_OperatingHaloTime, () =>
                {
                    if (m_selectCityEffect != null)
                    {                      
                        m_selectCityEffect.SetActive(true);  
                        m_selectCityEffect.transform.DOLocalMove(v3, 0);
                        m_selectCityEffect.transform.DOScale(TroopHelp.GetSelectEffectScale(TouchTargetEfeectObjectType.City), 0);
                        isSelectMyCity = true;
                        isSelectMyTroop = false;
                        isSelectMyRssItem = false;
                        curSelectRssItemId = 0;
                        WorldCamera.Instance().SetCanDrag(false);
                        if (!m_bDragTroop)
                        {
                            m_bDragTroop = true;
                        }

                        CreateMoveLine();
                        if (timerTouchCity != null)
                        {
                            timerTouchCity.Cancel();
                        }

                        if (isSelectTouchEnd)
                        {
                            m_selectCityEffect.SetActive(false);
                            m_bDragTroop = false;
                        }

                        OnLongTouchBuilding(v3, TouchTargetEfeectObjectType.City);

                    }
                }, (v) =>
                {
                });
            }
        }


        private void OnLongTouchBuilding(Vector3 v3, TouchTargetEfeectObjectType type, long objectType=0)
        {
            if (timerTouchCityTwo != null)
            {
                timerTouchCityTwo.Cancel();
            }

            float radius = 0;
            timerTouchCityTwo= Timer.Register(1.46f, () =>
            {           
                if (type == TouchTargetEfeectObjectType.City)
                {
                    WorldMapLogicMgr.Instance.MapSoundHandler.PlayTouchMyCity();
                    
                }else if (type == TouchTargetEfeectObjectType.AllianceBuilding)
                {
                    int type1 = m_allianceProxy.GetBuildServerTypeToConfigType(objectType);
                    AllianceBuildingTypeDefine allianceBuildingTypeDefine =
                        CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type1);
                    if (allianceBuildingTypeDefine != null)
                    {
                        radius = allianceBuildingTypeDefine.radius;
                    }
                }

                SetSelectAnimatorGo(v3, TroopHelp.GetSelectEffectScale(type,radius));
                if (timerTouchCityTwo != null)
                {
                    timerTouchCityTwo.Cancel();
                }

                if (isSelectTouchEnd)
                {
                    m_SelectAnimatorGo.SetActive(false);
                }

            });
        }


        private void SetSelectAnimatorGo(Vector3 v3, Vector3 scale)
        {
            if (m_SelectAnimatorGo != null)
            {
                if (!m_SelectAnimatorGo.activeSelf)
                {
                    m_SelectAnimatorGo.SetActive(false);
                }
                          
                m_SelectAnimatorGo.SetActive(true);                  
                m_SelectAnimatorGo.transform.DOLocalMove(v3, 0);
                m_SelectAnimatorGo.transform.DOScale(scale, 0);
            }      
        }

        //设置选中队伍
        private void SetSelectTroop(TouchTroopSelectParam param)
        {
            int id = param.armIndex;
            ArmyData armyData = null;
            if (param.isGuide)
            {
                armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            }
            else
            {
                armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(id);
            }

            if (armyData != null)
            {
                //行军队伍跳转
                if (!armyData.isPlayerHave)
                {
                    return;
                }
                SelectTroopByArmId(armyData.troopId, false);
            }
        }

        private void OnSelectTroopDragMove(TouchTroopSelectParam param)
        {
            int armId = param.armIndex;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armId);
            if (armyData == null) return;
            if (!armyData.isPlayerHave) return;
            m_TouchPressUI = param.isDrag;
            m_selectObjectId = armyData.objectId;
            SelectTroopByArmId(armyData.troopId, param.isDrag);
        }

        private void OnRefreshSelcetTroopByFightHudIcon(int objectId)
        {
            m_selectObjectId = objectId;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData != null)
            {
                if (armyData.isPlayerHave)
                {
                    SelectTroopByArmId(armyData.troopId);
                }

                TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                touchTroopInfo.mainArmObjectId = armyData.objectId;

                AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
            }            
        }

        private void OnRefreshSelcetTroop(TouchTroopSelectParam param)
        {
            int armId = param.armIndex;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armId);
            var armyInfo = m_TroopProxy.GetArmyByIndex(armId);
            bool isShow = false;
            if (armyData != null && armyInfo != null)
            {
                if (!armyData.isPlayerHave) return;
                m_TouchPressUI = param.isDrag;
                m_selectObjectId = armyData.objectId;
                if (param.isSelect)
                {                 
                    if (TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.COLLECTING) ||
                        TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.RALLY_WAIT) ||
                        TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.GARRISONING))
                    {
                        isShow = false;
                    }
                    else
                    {                      
                        if (param.isCameraFollow)
                        {
                            var viewPos = GetSelectTroopPos(armId);
                            param.isCameraFollow = false;
                            Debug.Log($"TroopPos:{viewPos.ToString()}");
                            WorldCamera.Instance().SetCanDrag(false);
                            WorldCamera.Instance().SetCanClick(false);
                            WorldCamera.Instance().SetCanZoom(false);
                            float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                            WorldCamera.Instance().SetCameraDxf(Firstdxf, 200, () =>
                            {
                                if (viewPos.x > 0 || viewPos.z > 0)
                                {
                                    WorldCamera.Instance().ViewTerrainPos(viewPos.x, viewPos.z, 200, () =>
                                    {
                                        WorldCamera.Instance().SetCanDrag(true);
                                        WorldCamera.Instance().SetCanZoom(true);
                                        WorldCamera.Instance().SetCanClick(true);
                                    });
                                }
                            });
                        }

                        CheckExitDoubleClick();
                        SelectTroopByArmId(armyData.troopId, param.isDrag);

                        if (!param.isDrag)
                        {
                            TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                            touchTroopInfo.mainArmObjectId = armyData.objectId;

                            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);

                            isShow = true;
                        }
                    }               
                }
            }

            if (isShow)
            {
                return;
            }

            if (param.isOpenWin && armyInfo != null)
            {
                var troopState = TroopHelp.GetTroopState(armyInfo.status);

                if (TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.COLLECTING)) //采集中
                {
                    if (armyInfo.collectResource != null)
                    {
                        param.isCameraFollow = false;
                        if (armyInfo.collectResource != null)
                        {
                            if (armyInfo.collectResource.pos != null)
                            {
                                var viewPos = PosHelper.ServerUnitToClientUnit(armyInfo.collectResource.pos);
                                if (viewPos.x > 0 || viewPos.z > 0)
                                {
                                    WorldCamera.Instance().ViewTerrainPos(viewPos.x, viewPos.z, 200, () =>
                                    {
                                        float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");

                                        WorldCamera.Instance().SetCameraDxf(Firstdxf, 200, () =>
                                        {
                                            MapObjectInfoEntity data = m_worldProxy.GetWorldMapObjectByobjectId(armyInfo.targetArg.targetObjectIndex);
                                            if (data == null || data.objectId <= 0)
                                            {
                                                return;
                                            }
                                            if (data.gameobject == null)
                                            {
                                                return;
                                            }

                                            CoreUtils.uiManager.ShowUI(UI.s_Pop_WorldInfo, null, string.Format("RssItem_{0}", data.objectId));
                                        });
                                    });
                                }
                            }
                            else
                            {
                                SkipPanel(armyInfo, param); 
                            }
                        }
                    }
                }
                else if (troopState == Troops.ENMU_SQUARE_STAT.IDLE || troopState == Troops.ENMU_SQUARE_STAT.FIGHT)
                {
                    SkipPanel(armyInfo, param);
                }
            }

            if (param.isCameraFollow)
            {
                float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                WorldCamera.Instance().SetCameraDxf(Firstdxf, 1000, null);
            }
        }

        private void SkipPanel(ArmyInfoEntity armyInfo,TouchTroopSelectParam param)
        {
            if (TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.RALLY_WAIT) ||
                TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.GARRISONING)||
                TroopHelp.IsHaveState(armyInfo.status, ArmyStatus.COLLECTING))
            {
                ArmyData arllyArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                    .GetArmyDataByArmyId((int) armyInfo.armyIndex);
                if (arllyArmyData != null)
                {
                    SelectTroopByArmId(arllyArmyData.troopId, param.isDrag);
                    Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                        .GetTroop(arllyArmyData.objectId);
                    if (formation != null && param.isCameraFollow)
                    {
                        if (formation.gameObject.transform.position.x > 0 || formation.gameObject.transform.position.z>0)
                        {
                            WorldCamera.Instance().ViewTerrainPos(formation.gameObject.transform.position.x,
                                formation.gameObject.transform.position.z, 1000, null);
                        }

                        TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                        touchTroopInfo.mainArmObjectId = arllyArmyData.objectId;

                        AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
                    }
                    else
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosAndOpen,
                            new Vector2Int((int) arllyArmyData.Pos.x, (int) arllyArmyData.Pos.y));
                    }
                }
            }
        }


        private void OnSelectTransport(TouchNotTroopSelect data)
        {
            CancelFollow();
            if (data == null)
            {
                return;
            }

            int transportId = data.id;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetTransportDataById(transportId);
            if (armyData != null)
            {
                SelectTroopByArmId(armyData.troopId, false);
                if (data.isShowSelectHud)
                {
                    TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                    touchTroopInfo.mainArmObjectId = armyData.objectId;

                    AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
                }
                AdjustCameraHeightForLookAtObject(armyData);
            }
        }

        private void AdjustCameraHeightForLookAtObject(ArmyData armyData)
        {
            float dxf = WorldCamera.Instance().getCameraDxf("FTE_Scout");
            if (dxf > WorldCamera.Instance().getCurrentCameraDxf())
            {
                WorldCamera.Instance().SetCameraDxf(dxf, 300, () => { LocationToTargetObejct(armyData); });
            }
            else
            {
                LocationToTargetObejct(armyData);
            }
        }

        private void LocationToTargetObejct(ArmyData armyData)
        {
            if (armyData == null) return;
            Troops formation =
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
            if (formation != null && formation.gameObject != null)
            {
                m_cameraFollowStatus = 0;
                CancelWaitFollowTimer();
                CameraMoveToTargetPos(formation.gameObject);
            }
            else
            {
                WaitCreateFormationFollow(armyData.objectId);
                Vector2 pos = armyData.GetMovePos();
                WorldCamera.Instance().ViewTerrainPos(pos.x, pos.y, 300, null);
            }
        }

        //等待模型创建出来后再跟随
        private void WaitCreateFormationFollow(int objectId)
        {
            m_isCameraFollow = true;
            m_cameraFollowStatus = 1;
            m_targetObjectId = objectId;
            m_followTarget = null;
            m_startWaitTime = Time.realtimeSinceStartup;
            CancelWaitFollowTimer();
            m_waitFollowTimer = Timer.Register(0.1f, ()=> {
                Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(m_targetObjectId) as Troops;
                if (formation != null && formation.gameObject != null)
                {
                    m_cameraFollowStatus = 2;
                    m_followTarget = formation.gameObject;
                    m_cameraLastPos = formation.gameObject.transform.position;
                    CancelWaitFollowTimer();
                }
                else
                {
                    if (Time.realtimeSinceStartup - m_startWaitTime >= 3f)//超过3秒没找到 结束查找
                    {
                        CancelWaitFollowTimer();
                        m_isCameraFollow = false;
                        m_followTarget = null;
                        m_cameraFollowStatus = 0;
                    }
                }
            },null, true);
        }

        private void CancelWaitFollowTimer()
        {
            if (m_waitFollowTimer != null)
            {
                m_waitFollowTimer.Cancel();
                m_waitFollowTimer = null;
            }
        }

        private void OnSelectScout(TouchNotTroopSelect data)
        {
            CancelFollow();
            if (data == null)
            {
                return;
            }

            int scoutId = data.id;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId(scoutId);
            if (armyData != null)
            {
                m_touchNotTroopSelect = data;
                Troops formation =
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
                if (formation != null)
                {
                    m_selectObjectId = armyData.objectId;
                    SelectTroopByArmId(armyData.troopId, false);
                    if (data.isShowSelectHud)
                    {
                        TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                        touchTroopInfo.mainArmObjectId = armyData.objectId;

                        AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
                    }
                }

                AdjustCameraHeightForLookAtObject(armyData);
            }
        }

        private void UpdateByTouchNotTroopSelect(int objectId)
        {
            if (m_touchNotTroopSelect != null)
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                    .GetScoutDataByScoutId(m_touchNotTroopSelect.id);
                // 当前斥候不在视野内时，军队数据内没有该斥候id的数据，所以等镜头到达且斥候数据创建后，再处理选中与Hud。
                if (armyData != null && armyData.objectId == objectId)
                {
                    m_selectObjectId = armyData.objectId;
                    SelectTroopByArmId(armyData.troopId, false);
                    if (m_touchNotTroopSelect.isShowSelectHud)
                    {
                        TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                        touchTroopInfo.mainArmObjectId = armyData.objectId;

                        AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);                   
                    }

                    AdjustCameraHeightForLookAtObject(armyData);
                }
            }
        }

        private void SelectTroopByArmId(int armid, bool isDrag = true)
        {
            if (m_viewLevelMediator.GetViewLevel() > MapViewLevel.Tactical)
            {
                return;
            }

            ArmyData army = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armid);
            if (army == null)
            {
                return;
            }

            //播放统帅选中对白
            HeroDefine define = CoreUtils.dataService.QueryRecord<HeroDefine>(army.heroId);
            if (define != null)
            {
                CoreUtils.audioService.PlayOneShot(define.voiceSelect);
            }

            m_selectArmId = armid;
            isSelectMyTroop = true;
            isSelectMyCity = false;
            isSelectMyRssItem = false;
            curSelectRssItemId = 0;
            WorldCamera.Instance().SetCanDrag(false);            

            if (m_doubleSelectArmyIndexList != null)
            {
                foreach (var armyIndex in m_doubleSelectArmyIndexList)
                {
                    CreateViceMoveLine(armyIndex);
                }
            }
            else
            {                
                CreateMoveLine();
                removeAllSelectMyTroopEffect();
                CreateSelectMyTroopEffect(armid);
            }

            if (!m_bDragTroop && isDrag)
            {
                m_bDragTroop = true;
            }
        }

        #endregion

        #region 伺候镜头跟随

        private void CancelFollow()
        {
            m_isCameraFollow = false;
            m_followTarget = null;
            m_cameraFollowStatus = 0;
            m_targetObjectId = 0;
            CancelWaitFollowTimer();
        }

        private void CameraMoveToTargetPos(GameObject target)
        {
            if (target == null)
            {
                return;
            }

            m_cameraLastPos = target.transform.position;
            WorldCamera.Instance().ViewTerrainPos(target.transform.position.x,
                target.transform.position.z, 300, () =>
                {
                    m_isCameraFollow = true;
                    m_cameraFollowStatus = 2;
                    m_followTarget = target;
                });
        }

        public bool IsCameraFollow()
        {
            return m_isCameraFollow;
        }

        #endregion

        #region 手势操作管理

        private Vector2 m_startMouse = Vector2.zero;
        private bool m_bDragTroop = false;
        private Timer m_autoMoveTimer = null;
        private bool m_bDragMove = false;
        private bool m_TouchPressUI = false;

        private void OnTouchBegan(int x, int y)
        {
            if (m_WorldMgrMediator == null)
            {
                m_WorldMgrMediator =
                    AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            }

            if (!m_WorldMgrMediator.IsWorldMapStateNormal())//迁城模式
            {
                return;
            }
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return;
            CancelFollow();
            MapViewLevel crrLevel = m_viewLevelMediator.GetViewLevel();
            if (crrLevel > MapViewLevel.Tactical)
            {
                m_startMouse = new Vector2(x, y);
            }
        }

        private void OnTouchEnded(int x, int y)
        {
            if (m_WorldMgrMediator == null)
            {
                m_WorldMgrMediator =
                    AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            }

            if (!m_WorldMgrMediator.IsWorldMapStateNormal())//迁城模式
            {
                return;
            }
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return;
            var endMouse = new Vector2(x, y);
            MapViewLevel crrLevel = m_viewLevelMediator.GetViewLevel();
            if (crrLevel > MapViewLevel.Tactical)
            {
                if (m_startMouse.Equals(endMouse))
                {
                    var wp = CityGlobalMediator.ScreenToMapPosition(x, y);

                    if (m_fogMediator == null)
                    {
                        m_fogMediator =
                            AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as
                                FogSystemMediator;
                    }

                    // 自动移动的时候，不允许此操作
                    if(WorldCamera.Instance().IsAutoMoving())
                    {
                        return;
                    }
                    var pos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
                    if (!m_fogMediator.HasFogAtWorldPos(pos.x, pos.z))
                    {
                        WorldCamera.Instance()
                            .ViewTerrainPos(wp.x, wp.z, 1000f, () =>
                            {
                                float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                                WorldCamera.Instance().SetCameraDxf(Firstdxf, 1000f, () => { });
                            });
                    }
                }
            }
            else
            {
                if (m_TouchPressUI)
                {
                    CheckTouchMoveEndHandle(new Vector2(x, y));
                    CheckExitDoubleClick();
                }                
            }
        }

        private Vector2 lastPos= Vector2.zero;
        private void OnTouchMoved(int x, int y)
        {
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return;
            if (m_viewLevelMediator.GetViewLevel() > MapViewLevel.Tactical)
            {
                return;
            }

            if (m_WorldMgrMediator == null)
            {
                m_WorldMgrMediator =
                    AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            }

            if (!m_WorldMgrMediator.IsWorldMapStateNormal())
            {
                return;
            }
            
            if (!isSelectMyCity)
            {
                if (timerTouchCity != null)
                {
                    timerTouchCity.Cancel();
                }
            }

            if (m_bDragTroop == false)
                return;

            if (isSelectMyTroop)
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(m_selectArmId);

                if (armyData ==null || !armyData.isPlayerHave)
                {
                    return;
                }
                if(armyData.isRally)
                {
                    Debug.LogWarning("集结部队无法操作");
                    return;
                }

                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                {
                    Debug.LogWarning("溃败状态无法操作");
                    return;
                }

                if (armyData.troopType == RssType.Scouts)
                {
                    Debug.LogWarning("斥候部队不可以拖动操作");
                    return;
                }
            }


            var pos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
            m_movePos = new Vector2(x, y);
            UpdateDragMove(pos.x, pos.z);
            if (m_autoMoveTimer == null)
            {
                m_autoMoveTimer = Timer.Register(0.05f, null, OnDragTroopMoveOutViw, true);
            }

            m_bDragMove = true;
            isSelectTouchEnd = false;
            CheckTouchMoveOnItem(new Vector2(x, y));
            OnDragMoveShowHUD(pos);
        }

        private void OnDragMoveShowHUD(Vector3 pos)
        {
            if (!IsChickMove3D(m_movePos))
            {
                Vector2 pos1= new Vector2(pos.x,pos.z);
                if (pos1 != lastPos)
                {
                    WorldMapLogicMgr.Instance.MapTouchHandler.Stop();
                    UpdateRssHudBySpace(new Vector2(pos.x,pos.z)); 
                }
                lastPos= new Vector2(pos.x,pos.z);
            }
            else
            {
                WorldMapLogicMgr.Instance.MapTouchHandler.StopSpace();
            }
        }


        private void StopDragAndMove()
        {
            if (!GuideProxy.IsGuideing)
            {
                WorldCamera.Instance().SetCanDrag(true);
            }

            m_bDragTroop = false;
            m_bDragMove = false;
        }

        #endregion

        #region 拖动事件处理  

        private float cityX;
        private float cityY;
        private bool isCity = false;
        private bool isRss = false;
        private bool isBar = false;

        private void CheckTouchMoveEndHandle(Vector2 touchPos)
        {
            var collideParent = m_touchMoveCollideParent;
            var collideName = m_touchMoveCollideName;    
            if (string.IsNullOrEmpty(collideParent) && string.IsNullOrEmpty(collideName))
            {
                RemoveRssHUD();
                Vector3 v3 = WorldCamera.Instance()
                    .GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), touchPos.x, touchPos.y);
                OnTouchTroopMarchBySpace(v3);
            }
            else
            {
               OnTouchSelect(collideParent, collideName);
            }
            
            collideParent = String.Empty;
            collideName = String.Empty;
            OnTouchUnSelect(collideParent, collideName);
        }


        private void CheckTouchMoveOnItem(Vector2 clickPos)
        {
            int nIndex = -1;
            RaycastHit [] rayHits = CoreUtils.inputManager.RayCashHit3D((int)clickPos.x, (int)clickPos.y, out nIndex);
            if(nIndex == -1)
            {
                m_touchMoveCollideParent = string.Empty;
                m_touchMoveCollideName = string.Empty;
                RemoveRssHUD();
                return;
            }
            var rayHit = rayHits[nIndex];
            string collideParent = rayHit.transform.parent.name;
            string collideName = rayHit.transform.gameObject.name;
            if(!m_touchMoveCollideParent.Equals(collideParent) || !m_touchMoveCollideName.Equals(collideName))
            {
                RemoveRssHUD();
            }
            m_touchMoveCollideParent = string.Empty;
            m_touchMoveCollideName = string.Empty;

            Debug.Log($"CheckTouchMoveOnItem:{collideParent} {collideName}");
            string[] sP = collideParent.Split('_');
            string[] sN = collideName.Split('_');
            m_touchMoveCollideParent = collideParent;
            m_touchMoveCollideName = collideName;
            if (TouchMoveOnOtherCity(sP, sN)) return;
            if (TouchMoveOnCity(sP, sN)) return;
            if (TouchMoveOnTroopOrMonster(sP, sN)) return;
            if (TouchMoveOnRssItem(sP, sN)) return;
            if (TouchMoveOnRuneItem(sP, sN)) return;
            if (TouuchMoveOnAllianceBuilding(sP, sN)) return;
            if (TouuchMoveOnWonder(sP, sN)) return;                    
        }

        private bool isShowTips = false;
        private void ClearTouchMoveCollide()
        {
            m_touchMoveCollideParent = string.Empty;
            m_touchMoveCollideName = string.Empty;
        }

        //拖到其他玩家城市上
        private bool TouchMoveOnOtherCity(string[] sP, string[] sN)
        {
            if (sP.Length == 3 && sP.Length > 0)
            {
                if (sP[1] == "other")
                {
                    if (!isShowTips)
                    {
                        long rid = long.Parse(sP[2]);
                        if (SelectEffect != null)
                        {
                            MapObjectInfoEntity mapObjectInfoEntity = m_worldProxy.GetWorldMapObjectByRid(rid);
                            if (mapObjectInfoEntity != null)
                            {
                                bool isGuild = false;
                                if (mapObjectInfoEntity.guildId == 0 && m_PlayerProxy.CurrentRoleInfo.guildId == 0)
                                {
                                    isGuild = false;
                                }
                                else
                                {
                                    isGuild = mapObjectInfoEntity.guildId == m_PlayerProxy.CurrentRoleInfo.guildId;
                                }

                                if (isGuild)
                                {
                                    ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.blue);                                  
                                }
                                else
                                {
                                    ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.red);
                                }


                                SelectEffect.SetActive(false);
                                SelectEffect.SetActive(true);
                                SelectEffect.gameObject.transform.position = new Vector3(
                                    mapObjectInfoEntity.objectPos.x / 100f, 0,
                                    mapObjectInfoEntity.objectPos.y / 100f);
                                SetTargetEffectScale(TouchTargetEfeectObjectType.City);
                                AppFacade.GetInstance().SendNotification(CmdConstant.MapTouchOtherCity, true);
                                UpdateRssHud(mapObjectInfoEntity.objectId);
                            }
                        }

                        isShowTips = true;
                    }
                }

                return true;
            }

            return false;
        }

        //拖到自己城市上
        private bool TouchMoveOnCity(string[] sP, string[] sN)
        {
            if (isSelectMyCity)
            {
                return false;
            }

            if (sP.Length <= 2 && sP.Length > 0)
            {
                string[] cityName = TroopHelp.GetOnClickTargetType(RssType.City);
                if (cityName.Contains(sP[0]))
                {

                    if (isShowTips)
                    {
                        return false;
                    }

                    if (SelectEffect != null)
                    {
                        if (sP[1] != "other")
                        {
                            float cityX = cityGlobal.CityBuildingContainer.position.x;
                            float cityY = cityGlobal.CityBuildingContainer.position.z;
                            ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.green);
                            SelectEffect.SetActive(false);
                            SelectEffect.SetActive(true);
                            SelectEffect.gameObject.transform.position = new Vector3(
                                cityGlobal.CityBuildingContainer.position.x, 0,
                                cityGlobal.CityBuildingContainer.position.z);
                            this.cityX = cityX;
                            this.cityY = cityY;
                            curRoleObjectId = m_CityBuildingProxy.MyCityObjData.mapObjectExtEntity.objectId;
                            UpdateRssHud(curRoleObjectId);
                            isCity = true;
                            SetTargetEffectScale(TouchTargetEfeectObjectType.City);
                            isShowTips = true;
                            return true;
                        }
                    }
                    return true;
                }  
            }
            return false;
        }

        //拖到部队或者怪物上
        private bool TouchMoveOnTroopOrMonster(string[] sP, string[] sN)
        {
            if (sN.Length < 2)
            {
                return false;
            }

            string name = sN[0];
            int id = int.Parse(sN[1]);
            string[] mosterName = TroopHelp.GetOnClickTargetType(RssType.Monster);
            string[] troopName = TroopHelp.GetOnClickTargetType(RssType.Troop);
            if (mosterName.Contains(name))
            {
                MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(id);
                if (infoEntity != null)
                {
                    if (infoEntity.rssType == RssType.SummonConcentrateMonster)
                    {
                        return false;
                    }

                    if (isShowTips)
                    {
                        return false;
                    }

                    if (infoEntity.gameobject != null)
                    {
                        if (SelectEffect != null)
                        {
                            ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.red);
                            SelectEffect.SetActive(false);
                            SelectEffect.gameObject.SetActive(true);
                            SelectEffect.transform.position = new Vector3(infoEntity.gameobject.transform.position.x, 0, infoEntity.gameobject.transform.position.z);
                            selectRssID = infoEntity.objectId;
                            m_selectEffectFlow = infoEntity.gameobject;
                            UpdateRssHud(infoEntity.objectId);
                            isBar = true;
                            if (infoEntity.monsterDefine != null)
                            {
                                SetTargetEffectScale(TouchTargetEfeectObjectType.Monster, infoEntity.monsterDefine.radius);
                                isShowTips = true;
                                return true;
                            }
                        }
                    }
                }
            }
            else if (troopName.Contains(name))
            {
                TouchMoveOnOhterTroop(id);
            }

            return false;
        }

        /// <summary>
        /// 拖到其他人部队上
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool TouchMoveOnOhterTroop(int id)
        {
            if (isShowTips)
            {
                return false;
            }

            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                {
                    return false;
                }

                if (armyData.isRally)
                {
                    if (!m_RallyTroopsProxy.isRallyTroopHaveGuid(armyData.armyRid))
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.red);
                    }
                    else if (!m_RallyTroopsProxy.IsJoinRallyCheck(armyData.armyRid,false))
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.blue);
                    }
                    else
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.green);
                    }

                    if (SelectEffect != null)
                    {
                        SelectEffect.SetActive(false);
                        SelectEffect.gameObject.SetActive(true);
                        Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(id);
                        if (formation != null)
                        {
                            SelectEffect.transform.position =
                                new Vector3(formation.transform.position.x, 0, formation.transform.position.z);
                            m_selectEffectFlow = formation.gameObject;
                        }

                        SetTargetEffectScale(TouchTargetEfeectObjectType.OtherPlayerTroop, armyData.armyRadius);
                        isShowTips = true;
                    }
                    
                }
                else if (!armyData.isPlayerHave)
                {
                    MapObjectInfoEntity mapObjectInfoEntity = m_worldProxy.GetWorldMapObjectByobjectId(id);
                    if (mapObjectInfoEntity != null)
                    {
                        if (SelectEffect != null)
                        {
                            bool isGuild = false;
                            if (mapObjectInfoEntity.guildId == 0 && m_PlayerProxy.CurrentRoleInfo.guildId == 0)
                            {
                                isGuild = false;
                            }
                            else
                            {
                                isGuild = mapObjectInfoEntity.guildId == m_PlayerProxy.CurrentRoleInfo.guildId;
                            }

                            if (isGuild)
                            {
                                ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.blue);
                            }
                            else
                            {
                                ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.red);
                            }

                            SelectEffect.SetActive(false);
                            SelectEffect.gameObject.SetActive(true);
                            Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(id);
                            if (formation != null)
                            {
                                SelectEffect.transform.position =
                                    new Vector3(formation.transform.position.x, 0, formation.transform.position.z);
                                m_selectEffectFlow = formation.gameObject;
                            }

                            SetTargetEffectScale(TouchTargetEfeectObjectType.OtherPlayerTroop, armyData.armyRadius);
                            isShowTips = true;
                        }
                    }
                }    
                UpdateRssHud(id);
                
                return true;
            }
            return false;
        }

        //拖到资源点上
        private bool TouchMoveOnRssItem(string[] sP, string[] sN)
        {
            if (sP.Length <= 0 && sP.Length > 2)
            {
                return false;
            }

            if (isShowTips)
            {
                return false;
            }

            ///拖到资源点身上
            if (sP[0] == "RssItem")
            {
                int rssId = int.Parse(sP[1]);
                MapObjectInfoEntity rssGo = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                if (rssGo != null)
                {
                    if (TroopHelp.IsNoTouchType(rssGo.rssType))
                    {
                        Debug.LogWarning("当前类型不可以选中");
                        return false;
                    }
                    
                    if (curSelectRssItemId == rssGo.objectId)
                    {
                        return false;
                    }

                    Color color = RS.green;
                    if (rssGo.collectRid != 0)
                    {
                        GuildMemberInfoEntity guildMemberInfoEntity = m_allianceProxy.getMemberInfo(rssGo.collectRid);
                        bool isGuild = guildMemberInfoEntity != null;
                        if (m_PlayerProxy.Rid == rssGo.collectRid) //自己部队在采集
                        {
                            isGuild = true;
                        }

                        color = isGuild ? RS.blue : RS.red;
                    }

                    if (SelectEffect != null)
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, color);
                        SelectEffect.SetActive(false);
                        SelectEffect.SetActive(true);
                        SelectEffect.transform.position =
                            new Vector3(rssGo.objectPos.x / 100f, 0, rssGo.objectPos.y / 100f);
                        rssGo.isShowHud = true;
                        rssGo.showHudIcon = m_RssProxy.GetHudUIIcon(RssType.Stone);
                        selectRssID = rssGo.objectId;
                        UpdateRssHud(rssGo.objectId);
                        isRss = true;
                        SetTargetEffectScale(TouchTargetEfeectObjectType.Resource);
                        isShowTips = true;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 拖到符文上
        /// </summary>
        /// <param name="sP"></param>
        /// <param name="sN"></param>
        /// <returns></returns>
        private bool TouchMoveOnRuneItem(string[] sP, string[] sN)
        {
            if (sP.Length == 0) return false;
            if (sP[0] != "RuneItem") return false;
            int objectId = int.Parse(sP[1]);
            if (isShowTips)
            {
                return false;
            }

            MapObjectInfoEntity runeItem = m_worldProxy.GetWorldMapObjectByobjectId(objectId);
            if (runeItem != null)
            {

                Color color = RS.green;
                if (SelectEffect != null)
                {
                    ChangeSpriteColor.SetColor(m_ChangeColorHelper, color);
                    SelectEffect.SetActive(false);
                    SelectEffect.SetActive(true);
                    SelectEffect.transform.position =
                        new Vector3(runeItem.objectPos.x / 100f, 0, runeItem.objectPos.y / 100f);
                    isRss = true;
                    var runeCfg = CoreUtils.dataService.QueryRecord<Data.MapItemTypeDefine>((int) runeItem.runeId);
                    if (runeCfg != null)
                    {
                        UpdateRssHud(objectId);
                        SetTargetEffectScale(TouchTargetEfeectObjectType.Rune, runeCfg.radius);
                        isShowTips = true;
                    }

                    return true;
                }
            }

            return true;
        }


        //拖到联盟建筑上面

        private bool TouuchMoveOnAllianceBuilding(string[] sP, string[] sN)
        {
            if (sP.Length == 0) return false;
            if (isShowTips)
            {
                return false;
            }

            if (sP[0] == "GuildBuild")
            {
                int objectId = int.Parse(sP[1]);       
                MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(objectId);
                if (infoEntity != null)
                {
                    if (infoEntity.guildId == m_PlayerProxy.CurrentRoleInfo.guildId)
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.blue);
                    }
                    else
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.red);
                    }

                    if (TroopHelp.IsTouchMoveAllianceBuilding(infoEntity.rssType))
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.green);
                    }

                    if (SelectEffect != null)
                    {
                        SelectEffect.SetActive(false);
                        SelectEffect.SetActive(true);
                        SelectEffect.transform.position =
                            PosHelper.ServerUnitToClientUnit_Vec3(new Vector2(infoEntity.objectPos.x,
                                infoEntity.objectPos.y));
                        isRss = true;

                        int type = m_allianceProxy.GetBuildServerTypeToConfigType(infoEntity.objectType);
                        AllianceBuildingTypeDefine allianceBuildingTypeDefine =
                            CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);
                        if (allianceBuildingTypeDefine != null)
                        {
                            UpdateRssHud(objectId);
                            SetTargetEffectScale(TouchTargetEfeectObjectType.AllianceBuilding,
                                allianceBuildingTypeDefine.radius);
                            isShowTips = true;
                        }

                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

 

        //拖到奇观上
        private bool TouuchMoveOnWonder(string[] sP, string[] sN)
        {
            if (sP.Length == 0) return false;
            if (isShowTips)
            {
                return false;
            }

            if (sP[0] == "CheckPoint" || sP[0] == "HolyLand")
            {
                int objectId = int.Parse(sP[1]);
                MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(objectId);
                if (infoEntity != null)
                {
                    if (infoEntity.guildId == m_PlayerProxy.CurrentRoleInfo.guildId)
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.blue);
                    }
                    else
                    {
                        ChangeSpriteColor.SetColor(m_ChangeColorHelper, RS.red);
                    }

                    if (SelectEffect != null)
                    {
                        SelectEffect.SetActive(false);
                        SelectEffect.SetActive(true);
                        SelectEffect.transform.position =
                            PosHelper.ServerUnitToClientUnit_Vec3(new Vector2(infoEntity.objectPos.x,
                                infoEntity.objectPos.y));
                        isRss = true;

                        float radis = 1f;
                        var strongHoldCfg =
                            CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int) infoEntity.strongHoldId);
                        if (strongHoldCfg != null)
                        {
                            StrongHoldTypeDefine strongHoldTypeDefine =
                                CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldCfg.type);
                            if (strongHoldTypeDefine != null)
                            {
                                radis = strongHoldTypeDefine.radius;
                            }
                        }

                        SetTargetEffectScale(TouchTargetEfeectObjectType.CheckPoint, radis);
                        UpdateRssHud(objectId);
                        isShowTips = true;
                        return true;
                    }
                }
            }

            return true;
        }

        private void RemoveRssHUD()
        {
            if (isCity)
            {
                if (cityX > 0 && cityY > 0)
                {
                    curRoleObjectId = m_CityBuildingProxy.MyCityObjData.mapObjectExtEntity.objectId;
                    WorldMapLogicMgr.Instance.MapTouchHandler.Stop();
                    isCity = false;
                }
            }

            if (isBar)
            {
                MapObjectInfoEntity monsterData = m_worldProxy.GetWorldMapObjectByobjectId(selectRssID);
                if (monsterData != null)
                {
                    monsterData.IsShowRssHud = false;
                    WorldMapLogicMgr.Instance.MapTouchHandler.Stop();
                    selectRssID = 0;
                    isBar = false;
                }
            }

            if (isRss)
            {
                MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(selectRssID);
                if (rssData != null)
                {
                    rssData.isShowHud = false;
                    WorldMapLogicMgr.Instance.MapTouchHandler.Stop();
                    selectRssID = 0;
                    isRss = false;
                }
            }

            if (SelectEffect != null)
            {
                PlaySelectEffectAnimator(true);   
                m_selectEffectFlow = null;
            }

            isShowTips = false;
        }

        private void UpdateRssHudBySpace(Vector2 targetPos)
        {
            Vector2 pos = GetHudShowPos();
            WorldMapLogicMgr.Instance.MapTouchHandler.Play(pos, targetPos);      
        }

        private void UpdateRssHud(long id)
        {
            Vector2 pos = GetHudShowPos();
            if (pos != Vector2.zero)
            {               
                WorldMapLogicMgr.Instance.MapTouchHandler.Play((int)id, pos);
            }
        }

        private Vector2 GetHudShowPos()
        {
            Vector2 pos= Vector2.zero;
            if (isSelectMyCity)
            {
                float cityX = cityGlobal.CityBuildingContainer.position.x;
                float cityY = cityGlobal.CityBuildingContainer.position.z;
                pos= new Vector2(cityX,cityY);
             }                 
            else
            {
                var p = GetSelectTroopPos();
                pos = new Vector2(p.x,p.z);
            }

            return pos;
        }

        #endregion

        #region 画白色线管理

        private Dictionary<int, TroopLineInfo> m_viceMoveLineDic = new Dictionary<int, TroopLineInfo>();
        private TroopLineInfo m_moveLine = null;
        private Dictionary<int, TroopLineInfo> m_drawLineDic = new Dictionary<int, TroopLineInfo>();
        private Dictionary<int, Vector2> m_drawLineTargetPosDic = new Dictionary<int, Vector2>();
        private Timer m_drawLineTimer = null;
        private Vector2 m_movePos;
        private Vector2 m_dragToPos;
        private TroopLineInfo m_DrawLineByCity;

        private void OnDragTroopMoveOutViw(float dt)
        {
            if (m_movePos.x > Screen.width * 0.9 || m_movePos.x < Screen.width * 0.1 ||
                m_movePos.y > Screen.height * 0.9 || m_movePos.y < Screen.height * 0.1)
            {
                var offset = m_movePos - new Vector2(Screen.width / 2, Screen.height / 2);
                offset.Normalize();
                var center = WorldCamera.Instance().GetViewCenter();
                center += offset * dt * WorldCamera.Instance().getCurrentCameraDist() / 5.0f;
                // 需要这两句，不然自动移动的时候地图不会刷新
                WorldCamera.Instance().isMovingToPos = false;
                MapManager.Instance().Update();

                WorldCamera.Instance().ViewTerrainPos(center.x, center.y, 0.05f, null);
                var pos = WorldCamera.Instance()
                    .GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), m_movePos.x, m_movePos.y);
                UpdateDragMove(pos.x, pos.z);
                OnDragMoveShowHUD(pos);
            }
            else
            {
                UpdateDragMove(m_dragToPos.x, m_dragToPos.y);
            }
        }

        private void UpdateDragMove(float ex, float ey)
        {
            m_dragToPos = new Vector2(ex, ey);

            if (m_viewLevelMediator == null || m_viewLevelMediator.GetViewLevel() > MapViewLevel.Tactical)
            {
                return;
            }

            if (isSelectMyCity)
            {
                var troopPos = new Vector3(m_CityBuildingProxy.RolePos.x, 0, m_CityBuildingProxy.RolePos.y);
                SetMoveLine(troopPos.x, troopPos.z, ex, ey);
            }
            else if (isSelectMyRssItem)
            {
                MapObjectInfoEntity rssItemData = m_worldProxy.GetWorldMapObjectByobjectId(curSelectRssItemId);
                if (rssItemData != null)
                {
                    var troopPos = new Vector3(rssItemData.objectPos.x / 100, 0, rssItemData.objectPos.y / 100);
                    SetMoveLine(troopPos.x, troopPos.z, ex, ey);
                }
            }
            else if (isSelectMyTroop)
            {
                var troopPos = GetSelectTroopPos();

                SetMoveLine(troopPos.x, troopPos.z, ex, ey);

                if (m_doubleSelectArmyIndexList != null)
                {
                    foreach (var armyIndex in m_doubleSelectArmyIndexList)
                    {
                        ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndex);
                        if (armyData == null) continue;

                        Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
                        if (formation != null)
                        {
                            troopPos = formation.transform.position;
                        }
                        else
                        {
                            Vector2 movePos = armyData.GetMovePos();
                            troopPos = new Vector3(movePos.x, 0, movePos.y);
                        }

                        SetViceMoveLine(armyIndex, troopPos.x, troopPos.z, ex, ey);
                    }
                }                
            }

            if (m_bDragTroop == false) return;
             
            // 拖拽过程中右键，取消拖拽或者多指操作取消操作
            if (CoreUtils.inputManager.GetTouchCount() > 1)
            {
                CheckExitDoubleClick();
                OnTouchUnSelect("", "");
                return;
            }
        }

        private void RemoveAllDraw()
        {
            foreach (var drawLine in m_drawLineDic.Values)
            {
                if (drawLine != null)
                {
                    drawLine.Destroy();
                }
            }
            m_drawLineDic.Clear();

            m_drawLineTargetPosDic.Clear();

            if (m_drawLineTimer != null)
            {
                Timer.Cancel(m_drawLineTimer);
                m_drawLineTimer = null;
            }
        }

        private void CreateDrawLine(int armyIndex, Vector2 targetPos)
        {
            if (targetPos.Equals(Vector2.zero)) return;

            if (!m_drawLineDic.ContainsKey(armyIndex))
            {
                TroopLineInfo drawLine = new TroopLineInfo();
                drawLine.LoadTroopLine(false, "troop_line_drag_move");
                drawLine.ChangeLineColor(RS.white);

                m_drawLineDic.Add(armyIndex, drawLine);
            }

            if (!m_drawLineTargetPosDic.ContainsKey(armyIndex))
            {
                m_drawLineTargetPosDic.Add(armyIndex, targetPos);
            }
            else
            {
                m_drawLineTargetPosDic[armyIndex] = targetPos;
            }

            SetDrawLine(armyIndex);

            if (m_drawLineDic.Count > 0 && m_drawLineTargetPosDic.Count > 0)
            {
                if (m_drawLineTimer == null)
                {
                    m_drawLineTimer = Timer.Register(0.05f, null, RefreshDrawLine, true);
                }
            }
        }

        private void DeleteDrawLine(int armyIndex)
        {
            if (m_drawLineDic.ContainsKey(armyIndex))
            {
                m_drawLineDic[armyIndex].Destroy();
                m_drawLineDic.Remove(armyIndex);
            }

            if (m_drawLineTargetPosDic.ContainsKey(armyIndex))
            {
                m_drawLineTargetPosDic.Remove(armyIndex);
            }

            if (m_drawLineDic.Count <= 0 && m_drawLineTargetPosDic.Count <= 0)
            {
                if (m_drawLineTimer != null)
                {
                    Timer.Cancel(m_drawLineTimer);
                    m_drawLineTimer = null;
                }
            }
        }

        private void SetDrawLine(int armyIndex)
        {
            if (!m_drawLineDic.ContainsKey(armyIndex)) return;

            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndex);
            if (armyData == null) return;

            Vector3 troopPos = Vector3.zero;
            Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
            if (formation != null)
            {
                troopPos = formation.transform.position;
            }
            else
            {
                Vector2 movePos = armyData.GetMovePos();
                troopPos = new Vector3(movePos.x, 0, movePos.y);
            }

            Vector2 targetPos = Vector2.zero;
            if (m_drawLineTargetPosDic.ContainsKey(armyIndex))
            {
                targetPos = m_drawLineTargetPosDic[armyIndex];
            }

            var paths = new Vector2[2];
            paths[0].x = troopPos.x;
            paths[0].y = troopPos.z;
            paths[1].x = targetPos.x;
            paths[1].y = targetPos.y;

            m_drawLineDic[armyIndex].SetTroopLinePath(paths);
        }

        private void RefreshDrawLine(float dt)
        {
            foreach (var armyIndex in m_drawLineDic.Keys)
            {
                SetDrawLine(armyIndex);
            }
        }

        private void CreateMoveLine()
        {
            if (m_moveLine == null)
            {
                m_moveLine = new TroopLineInfo();
                m_moveLine.LoadTroopLine(false, "troop_line_drag_move");
                m_moveLine.ChangeLineColor(RS.white);
            }
        }

        private void SetMoveLine(float start_x, float start_y, float end_x, float end_y)
        {
            if (m_moveLine != null)
            {
                var paths = new Vector2[2];

                paths[0].x = start_x;
                paths[0].y = start_y;
                paths[1].x = end_x;
                paths[1].y = end_y;

                m_moveLine.SetTroopLinePath(paths);
            }
        }

        private void DeleteAllMoveLine()
        {
            if (m_moveLine != null)
            {
                m_moveLine.Destroy();
                m_moveLine = null;
            }
        }

        private void CreateViceMoveLine(int armyIndex)
        {
            if (!m_viceMoveLineDic.ContainsKey(armyIndex))
            {
                TroopLineInfo moveLine = new TroopLineInfo();
                moveLine.LoadTroopLine(false, "troop_line_drag_move");
                moveLine.ChangeLineColor(RS.white);

                m_viceMoveLineDic.Add(armyIndex, moveLine);
            }
        }

        private void SetViceMoveLine(int armyIndex, float start_x, float start_y, float end_x, float end_y)
        {
            if (m_viceMoveLineDic.ContainsKey(armyIndex))
            {
                var paths = new Vector2[2];

                paths[0].x = start_x;
                paths[0].y = start_y;
                paths[1].x = end_x;
                paths[1].y = end_y;

                m_viceMoveLineDic[armyIndex].SetTroopLinePath(paths);
            }
        }

        private void DeleteAllViceMoveLine()
        {
            foreach (var viceMoveLine in m_viceMoveLineDic.Values)
            {
                if (viceMoveLine != null)
                {
                    viceMoveLine.Destroy();
                }
            }
            m_viceMoveLineDic.Clear();
        }

        private Vector2 CreateTroopTargetPos;
        private Timer CreateTroopMoveLineTimes;
        private void CreateCityMoveLine(Vector2  targetPos)
        {
            CreateTroopTargetPos = targetPos;
            if (m_DrawLineByCity == null)
            {
                m_DrawLineByCity = new TroopLineInfo();
                m_DrawLineByCity.LoadTroopLine(false, "troop_line_drag_move");
                m_DrawLineByCity.ChangeLineColor(RS.white);
                OnRefreshCityMoveLine(0);              
            }

            if (CreateTroopMoveLineTimes != null)
            {
                CreateTroopMoveLineTimes.Cancel();
            }

            CreateTroopMoveLineTimes = Timer.Register(0.05f, null, OnRefreshCityMoveLine, true);
        }

        private void OnRefreshCityMoveLine(float dt)
        {
            var paths = new Vector2[2];
            paths[0].x = cityBuildingProxy.RolePos.x;
            paths[0].y = cityBuildingProxy.RolePos.y;
            if (CreateTroopTargetPos != Vector2.zero)
            {
                paths[1].x = CreateTroopTargetPos.x;
                paths[1].y = CreateTroopTargetPos.y;
            }

            m_DrawLineByCity.SetTroopLinePath(paths);
        }

        private void DeleteCityMoveLine()
        {
            if (m_DrawLineByCity != null)
            {
                m_DrawLineByCity.Destroy();
                if (CreateTroopMoveLineTimes != null)
                {
                    CreateTroopMoveLineTimes.Cancel();
                }

                m_DrawLineByCity = null;
            }
        }

        #endregion

        #region 摄像机移动处理

        private Vector2 oldPos = Vector2.zero;
        private bool isSend = true;

        private void OnWorldViewChange(float x, float y, float dxf)
        {
            if (m_PlayerProxy.CurrentRoleInfo == null)
            {
                return;
            }

            if (WorldCamera.Instance().IsAutoMoving() || WorldCamera.Instance().IsSlipping())
                return;

            if (oldPos.Equals(Vector2.zero))
            {
                oldPos = new Vector2(m_PlayerProxy.CurrentRoleInfo.pos.x / 100,
                    m_PlayerProxy.CurrentRoleInfo.pos.y / 100);
            }

            CheckMapViewOutDis(x, y);
        }

        private void CheckMapViewOutDis(float x, float y)
        {
            Vector2 movePos = new Vector2(x, y);
            if (oldPos == movePos)
            {
                return;
            }

            int count = 0;
            float dis = Vector2.Distance(oldPos, movePos);
            int distance = (int) dis;

            if (distance >= 60)
            {
                isSend = true;
            }

            if (isSend)
            {
                m_RssProxy.SendMapMove((long) x, (long) y);
                isSend = false;
                oldPos = movePos;
            }
        }

        #endregion

        #region 自由行军协议

        private void SendTroopMapMarCh(int targetId, int armyIndex, OpenPanelType panelType, long rid,
            List<int> armyIndexList = null, bool isSituStation = false, bool isCheckWar = true)
        {
            if (panelType == OpenPanelType.JoinRally)
            {
                m_RallyTroopsProxy.SendJoinRallyByHaveTroop(rid, armyIndex);
                return;
            }

            if (panelType == OpenPanelType.Reinfore)
            {
                m_RallyTroopsProxy.SendReinforeRallyByArmyIndex(targetId, armyIndex, armyIndexList);
                return;
            }


            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(targetId);
            if (armyData != null)
            {
                if (!armyData.isPlayerHave)
                {
                    m_TroopProxy.TroopMapMarCh(armyIndex, TroopAttackType.Attack, targetId, null, armyIndexList, isSituStation);
                }
                return;
            }

            TroopAttackType type = TroopAttackType.Space;
            MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(targetId);
            TroopPosInfo troopPosInfo = new TroopPosInfo();
            if (rssData != null)
            {
                bool isHaveGuild = false;
                if (rssData.guildId == 0 && m_PlayerProxy.CurrentRoleInfo.guildId == 0)
                {
                    isHaveGuild = false;
                }
                else
                {
                    isHaveGuild = rssData.guildId == m_PlayerProxy.CurrentRoleInfo.guildId;
                }

                if (rssData.rssPointStateType == RssPointState.CollectedNoByally)
                {
                    type = TroopAttackType.Attack;
                    troopPosInfo.x = rssData.objectPos.x;
                    troopPosInfo.y = rssData.objectPos.y;
                }
                else if (rssData.guildBuildStatus == (int)GuildBuildState.building && isHaveGuild)
                {
                    type = TroopAttackType.Reinforce;
                    troopPosInfo.x = rssData.objectPos.x;
                    troopPosInfo.y = rssData.objectPos.y;
                }
                else if (rssData.rssType == RssType.Monster ||
                         rssData.rssType == RssType.Guardian ||
                         rssData.rssType == RssType.SummonAttackMonster ||
                         rssData.rssType == RssType.SummonConcentrateMonster)
                {
                    type = TroopAttackType.Attack;
                }
                else
                {
                    if (rssData.collectRid != 0)
                    {
                        GuildMemberInfoEntity guildMemberInfoEntity = m_allianceProxy.getMemberInfo(rssData.collectRid);
                        bool isGuild = guildMemberInfoEntity != null;
                        if (!isGuild && TroopHelp.IsAttackBuilding(rssData.rssType))
                        {
                            type = TroopAttackType.Attack;
                        }
                    }
                    else
                    {
                        if (TroopHelp.IsAttackBuildings(rssData.rssType))
                        {
                            type = TroopAttackType.Attack;
                        }
                        else
                        {
                            type = TroopAttackType.Collect;
                        }
                    }

                    troopPosInfo.x = rssData.objectPos.x;
                    troopPosInfo.y = rssData.objectPos.y;
                }                
            }

            m_TroopProxy.TroopMapMarCh(armyIndex, type, targetId, troopPosInfo, armyIndexList, isSituStation, isCheckWar);
        }

        #endregion

        #region 获取选择部队的pos
        private Vector3 GetSelectTroopPos(int armID = -1)
        {
            if(armID == -1 )
            {
                armID = m_selectArmId;
            }
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armID);
            if(armyData == null)
            {
                var pos = PosHelper.ServerUnitToClientUnit(m_PlayerProxy.CurrentRoleInfo.pos);
                return pos;
            }
            Troops formation =
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;

            if(formation != null)
            {
                return formation.transform.position;
            }
            else
            {
                var pos = armyData.GetMovePos();
                if(pos.Equals(Vector3.zero))
                {
                    return new Vector3(armyData.Pos.x, 0, armyData.Pos.y);
                }
                return new Vector3(pos.x, 0, pos.y);
            }
        }

        #endregion

        #region 长按有驻地部队的建筑

        private void GetStationArmy(int objectId, out int mainArmyIndex, out List<int> viceArmyIndexList)
        {
            mainArmyIndex = 0;
            viceArmyIndexList = new List<int>();

            MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(objectId);
            if (infoEntity == null) return;

            RssType rssType = (RssType)infoEntity.objectType;
            //资源点
            if (rssType == RssType.Stone ||
                rssType == RssType.Farmland ||
                rssType == RssType.Wood ||
                rssType == RssType.Gold ||
                rssType == RssType.Gem)
            {
                mainArmyIndex = (int)infoEntity.armyIndex;
            }
            //联盟资源场
            else if (rssType == RssType.GuildFoodResCenter ||
                     rssType == RssType.GuildWoodResCenter ||
                     rssType == RssType.GuildGoldResCenter ||
                     rssType == RssType.GuildGemResCenter)
            {
                TroopProxy troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
                List<ArmyInfoEntity> armyInfoList = troopProxy.GetArmys();
                foreach (var armyInfo in armyInfoList)
                {
                    if (armyInfo.status == (long)ArmyStatus.GARRISONING ||
                        armyInfo.status == (long)ArmyStatus.COLLECTING)
                    {
                        if (armyInfo.targetArg.targetObjectIndex == objectId)
                        {
                            int armyIndex = (int)armyInfo.armyIndex;
                            if (mainArmyIndex == 0)
                            {
                                mainArmyIndex = armyIndex;
                            }
                            else
                            {
                                if (!viceArmyIndexList.Contains(armyIndex))
                                {
                                    viceArmyIndexList.Add(armyIndex);
                                }
                            }
                        }
                    }
                }
            }
            //圣地 关卡 联盟建筑 城市
            else
            {
                TroopProxy troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
                List<ArmyInfoEntity> armyInfoList = troopProxy.GetArmys();
                foreach (var armyInfo in armyInfoList)
                {
                    if (armyInfo.status == (long)ArmyStatus.GARRISONING)
                    {
                        if (armyInfo.targetArg.targetObjectIndex == objectId)
                        {
                            int armyIndex = (int)armyInfo.armyIndex;
                            if (mainArmyIndex == 0)
                            {
                                mainArmyIndex = armyIndex;
                            }
                            else
                            {
                                if (!viceArmyIndexList.Contains(armyIndex))
                                {
                                    viceArmyIndexList.Add(armyIndex);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnLongClickBuilding(int objectId)
        {
            int mainArmyIndex; 
            List<int> viceArmyIndexList; 
            GetStationArmy(objectId, out mainArmyIndex, out viceArmyIndexList);

            if (mainArmyIndex != 0)
            {
                if (!isSelectMyRssItem)
                {
                    if (timerTouchRssItem != null)
                    {
                        timerTouchRssItem.Cancel();
                    }

                    MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(objectId);
                    if (infoEntity == null)
                    {
                        return;
                    }

                    Vector3 v3 =PosHelper.ServerUnitToClientUnit_Vec3(new Vector2(infoEntity.objectPos.x, infoEntity.objectPos.y));
                    timerTouchRssItem = Timer.Register(m_OperatingHaloTime, () =>
                    {
                        if (m_selectCityEffect != null)
                        {
                            curSelectRssItemId = objectId;
                            m_selectCityEffect.SetActive(true);
                            m_selectCityEffect.transform.DOLocalMove(v3, 0);                           
                            int type = m_allianceProxy.GetBuildServerTypeToConfigType(infoEntity.objectType);
                            AllianceBuildingTypeDefine allianceBuildingTypeDefine =
                                CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);
                            if (allianceBuildingTypeDefine != null)
                            {
                                m_selectCityEffect.transform.localScale = TroopHelp.GetSelectEffectScale(TouchTargetEfeectObjectType.AllianceBuilding, allianceBuildingTypeDefine.radius);
                            }

                            isSelectMyCity = false;
                            isSelectMyTroop = false;
                            isSelectMyRssItem = true;
                            WorldCamera.Instance().SetCanDrag(false);
                            if (!m_bDragTroop)
                            {
                                m_bDragTroop = true;
                            }

                            CreateMoveLine();
                            if (timerTouchRssItem != null)
                            {
                                timerTouchRssItem.Cancel();
                            }
                            OnLongTouchBuilding(v3, TouchTargetEfeectObjectType.AllianceBuilding,infoEntity.objectType);
                        }
                    }, (v) =>
                        {
                        }
                    );
                }
            }
        }

        #endregion
        #endregion
    }
}