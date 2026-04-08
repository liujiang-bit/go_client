// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    CityGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoTemp;
using Client;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using Client.FSM;
using Data;
using DG.Tweening;
using Newtonsoft.Json;
using SprotoType;
using Random = UnityEngine.Random;
using Hotfix;

namespace Game
{
    public enum MapViewLevel
    {
        None,
        City,
        Tactical,
        TacticsToStrategy_1,
        TacticsToStrategy_2,
        Strategic,
        Nationwide,
        Continental,
    }

    public enum ServerMapViewLevel
    {
        Tactical,
        Strategic,
        Nationwide,
        Continental,
    }
    
    public class LodPopDataContent
    {
        public MapObjectInfoEntity mapEntity;
        public ArmyData armyData;
        public float cacheZ;
        
        public Vector3 GetPosValue()
        {
            if (armyData != null)
            {
                if (armyData.go != null
                    && !TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.COLLECTING)
                    && !TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.GARRISONING)
                    && !TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.RALLY_WAIT))
                {
                    return armyData.go.transform.position;
                }
                else
                {
                    var pos = armyData.GetMovePos();
                    return new Vector3(pos.x,0,pos.y);
                }
            }

            if (mapEntity != null)
            {
                if (mapEntity.gameobject != null && mapEntity.gameobject.transform.position != Vector3.zero)
                    return mapEntity.gameobject.transform.position;
                else
                {
//                    if (mapEntity.objectType == (long) RssType.Troop
//                        || mapEntity.objectType == (long) RssType.Scouts
//                        || mapEntity.objectType == (long) RssType.Transport)
//                    {
                        var pos = mapEntity.GetMovePos();
                        return new Vector3(pos.x,0,pos.y);
//                    }
//                    else
//                    {
//                        var pos = mapEntity.GetMovePos();
//                        var x = pos.x / 100;
//                        var z = pos.y / 100;
//                        return new Vector3(x,0,z);
//                    }
                }
            }
            return Vector3.zero;
        }
        
        public static bool operator == (LodPopDataContent a, LodPopDataContent b)
        {
            bool ret = false;
            if (a.armyData != null && b.armyData != null)
            {
                ret = a.armyData.dataIndex == b.armyData.dataIndex;
            }

            if (a.mapEntity != null && b.mapEntity != null)
            {
                ret = a.mapEntity.objectId == b.mapEntity.objectId;
            }
            return ret;
        }

        public static bool operator !=(LodPopDataContent a, LodPopDataContent b)
        {
            return !(a == b);
        }

        public long objectId
        {
            get
            {
                if(armyData!=null && armyData.objectId>0)
                    return armyData.objectId;
                if (mapEntity != null && mapEntity.objectId > 0)
                    return mapEntity.objectId;
                return -1;
            }
        }

        public int objectType
        {
            get
            {
                if(armyData!=null && armyData.troopType>0)
                    return (int)armyData.troopType;
                if (mapEntity != null && mapEntity.objectType > 0)
                    return (int)mapEntity.objectType;
                return 0;
            }
        }
        
        public long scoutsIndex
        {
            get
            {
                if (armyData != null && armyData.dataIndex > 0)
                {
                    return armyData.dataIndex;
                }

                return mapEntity?.scoutsIndex ?? 0; 
            }
        }

        public long status
        {
            get
            {
                if(armyData!=null && armyData.armyStatus>0)
                    return (int)armyData.armyStatus;
                if (mapEntity != null && mapEntity.status > 0)
                    return (int)mapEntity.status;
                return 0;
            }
        }

        public long armyIndex
        {
            get
            {
                if(armyData!=null && armyData.dataIndex>0)
                    return armyData.dataIndex;
                if (mapEntity != null && mapEntity.armyIndex > 0)
                    return mapEntity.armyIndex;
                return 0;
            }
        }
        public long armyRid
        {
            get
            {
                if(armyData!=null && armyData.armyRid>0)
                    return armyData.armyRid;
                if (mapEntity != null && mapEntity.armyRid > 0)
                    return mapEntity.armyRid;
                return 0;
            }
        }
        public long guildId 
        {
            get
            {
                if(armyData!=null && armyData.guild>0)
                    return armyData.guild;
                if (mapEntity != null && mapEntity.guildId > 0)
                    return mapEntity.guildId;
                return 0;
            }
        }
        
        public long strongHoldId => mapEntity?.strongHoldId ?? 0;
        public long collectRid => mapEntity?.collectRid ?? 0;
        public List<long> guildFlagSigns => mapEntity?.guildFlagSigns ?? null;
        public bool isFight => mapEntity?.isFight ?? false;
        public bool villageState => mapEntity?.villageState ?? false;
        public string guildAbbName => mapEntity?.guildAbbName ?? string.Empty;
        public string cityName => mapEntity?.cityName ?? string.Empty;
        public long cityRid => mapEntity?.cityRid ?? 0;
        public long monsterId => mapEntity?.monsterId ?? 0;
        public long cityLevel => mapEntity?.cityLevel ?? 0;
        public long cityCountry => mapEntity?.cityCountry ?? 0;
        public bool IsShowRssHud => mapEntity?.IsShowRssHud ?? false;
        public string showHudicon => mapEntity?.showHudicon ?? string.Empty;
        public long resourcePointId => mapEntity?.resourcePointId ?? 0;
        public RssType rssType => mapEntity?.rssType ?? RssType.None;

        public long guildBuildStatus => mapEntity?.guildBuildStatus ?? 0;
//        public bool guildBuildIsBattle => mapEntity?.guildBuildIsBattle ?? false;
    }

    enum PopLoadType
    {
        None,
        Loading,
        Done,
    }
    enum PopViewType
    {
        HUD,
        Obj,
        Depend,
    }
    class LodPopViewContent
    {
        public PopLoadType loadType = PopLoadType.None;
        public LodPopViewConfig config;
        public GameObject obj;
        public MapElementUI hud;
        public List<LodPopViewData> lstSubElement = new List<LodPopViewData>();
    }
    class LodPopViewConfig
    {
        public PopViewType popType;
        public string prefabName;
        public Func<LodPopDataContent , bool> showCondition;             //预制创建前的判断
        public Action<GameObject, LodPopDataContent> refreshView; //预制刷新  需要注册
        public MapViewLevel minLodLevel;
        public MapViewLevel maxLodLevel;
        public MapElementUI.ElementUIType uiType;
        public LodPopViewConfig[] subConfigs;            //自身携带 依赖型视图配置   例如：主城携带了多个特效
        public Action<GameObject, LodPopDataContent,Transform> refreshSubView; //依赖预制刷新 最后一个为父节点   例如 特效自身的刷新
    };
    
    class LodPopViewData
    {
        public LodPopDataContent dataContent = new LodPopDataContent();
        public List<LodPopViewConfig> viewConfigs = new List<LodPopViewConfig>();
        public List<LodPopViewContent> lstViewContent = new List<LodPopViewContent>();
    }

    public class GlobalViewLevelMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "GlobalViewLevelMediator";
        private float TacticsToStrategy_Dxf_1;
        private float TacticsToStrategy_Dxf_2;
        private float HideStrategyObjDxf;
        private float CityObjDxf;//进入内城级(City)
        private float CityBoundDxf;//内/外城边界镜头
        private float minDxf;//内城放大最大镜头
        private float m_viewDxf = 0;
        private bool m_canActive = true;
        private MapViewLevel m_viewLevel = MapViewLevel.Continental;
        private float m_showProvinceNameDxfMin;
        private float m_showProvinceNameDxfMax;
        private bool m_IsInShowProvince = false;

        private MapViewLevel m_preViewLevel;
        
        private RssProxy m_RssProxy;
        private WorldMapObjectProxy m_worldMapProxy;
        private GuideProxy m_guideProxy;
        private FogSystemMediator m_fogMediator;
        private PlayerProxy m_playerProxy;
        private TroopProxy m_TroopProxy;
        private RssProxy m_rssProxy;
        private MonsterProxy m_MonsterProxy;
        private NetProxy m_netProxy;
        private AllianceProxy m_allianceProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private ScoutProxy m_scoutProxy;
        private Transform m_root;
        private const string m_root_path = "SceneObject/lod3_root";

        private Transform m_hudRoot;
        private const string m_hudPath = "UIRoot/Container/HUDLayer/pl_world";

        
        private bool m_isUpdate = true;
        private bool m_isInCity = false;
        private int m_isUpdateIndex = 0;
        private int m_isUpdateMaxCount = 0;

        private float m_inCityCamHeight = 250f;

        private LodMenuToggle m_curToggleType = LodMenuToggle.Alliance;
        
        private HashSet<long> m_lodMapObjectLoading = new HashSet<long>();
        private HashSet<long> m_hudObjectLoading = new HashSet<long>();

        public HashSet<long> m_isViewCityID = new HashSet<long>();
        public HashSet<long> m_isViewCityIDTactical= new HashSet<long>();

        private List<LodPopViewData> m_AllView = new List<LodPopViewData>(); 
        private List<long> m_Lst_ID_Changed = new List<long>(); 
            
            
        private List<GameObject> m_mapProvinceNamesObj = null;

        private MapViewLevel[] CameraLevelToMapView =
        {
            MapViewLevel.City,
            MapViewLevel.Tactical,
            MapViewLevel.Strategic,
            MapViewLevel.Strategic,
            MapViewLevel.Nationwide,
            MapViewLevel.Nationwide
        };

        private ServerMapViewLevel[] CameraLevelToServer =
        {
            ServerMapViewLevel.Tactical,
            ServerMapViewLevel.Tactical,
            ServerMapViewLevel.Strategic,
            ServerMapViewLevel.Strategic,
            ServerMapViewLevel.Strategic,
            ServerMapViewLevel.Strategic
        };
        
        private bool isShowTerritory = true;

        private Dictionary<long, UI_Tip_WorldObjectLodArmyView> m_scoutLodArmyDic = new Dictionary<long, UI_Tip_WorldObjectLodArmyView>();
        
        #endregion

        //IMediatorPlug needs
        public GlobalViewLevelMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        public GlobalViewLevelMediator(object viewComponent) : base(NameMediator, null)
        {
            
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.MapObjectHUDUpdate,
                CmdConstant.MapObjectRemove,
                CmdConstant.MapRemoveTroopHud,
                CmdConstant.MapPointInfoUpdate,
                CmdConstant.ChangeRolePos,
                CmdConstant.OnLodMenuChange,
                CmdConstant.ArmyDataLodPopAdd,
                CmdConstant.ArmyDataLodPopRemove,
                CmdConstant.ArmyDataLodPopUpdate,
                Guild_ApplyJoinGuild.TagName,
                CmdConstant.AllianceEixt,
                CmdConstant.NetWorkReconnecting,
                CmdConstant.SetSelectScoutHead,
            }.ToArray();
        }
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnLodMenuChange:
                    LodMenuToggle toggle = (LodMenuToggle)notification.Body;
                    bool v = Boolean.Parse(notification.Type);
                    if (toggle == LodMenuToggle.Alliance)
                    {
                        isShowTerritory = v;
                        CheckTerritoryStrategicCanShow();
                    }

                    if (v)
                    {
                        m_curToggleType = toggle;
                    }
                    else
                    {
                        if(m_curToggleType == toggle)
                            m_curToggleType = LodMenuToggle.None;
                    }
                    RefreshMapObjAndHud();
                    break;
                case CmdConstant.ChangeRolePos:
                    {
                        MapObjectInfoEntity mapItemInfo = m_worldMapProxy.GetWorldMapObjectByRid(m_playerProxy.CurrentRoleInfo.rid);
                        if (mapItemInfo != null)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectChange, mapItemInfo);
                        }
                    }
                    break;
                case CmdConstant.ArmyDataLodPopAdd:
                    {
//                        Debug.Log("++++ArmyDataLodPopAdd");
                        ArmyData armyData = notification.Body as ArmyData;
                        var dataContent = new LodPopDataContent();
                        dataContent.armyData = armyData;
                        RefreshMapObject(dataContent);
                        RefreshMapHudUI(dataContent);
                    }
                    break;
                case CmdConstant.ArmyDataLodPopRemove:
                    {
//                        Debug.Log("++++ArmyDataLodPopRemove");
                        ArmyData armyData = notification.Body as ArmyData;
                        RemoveMapObjLodAndHud(armyData.objectId);
                    }
                    break;
                case CmdConstant.ArmyDataLodPopUpdate:
                    {
//                        Debug.Log("++++ArmyDataLodPopUpdate");
                        ArmyData armyData = notification.Body as ArmyData;
                        var dataContent = new LodPopDataContent();
                        dataContent.armyData = armyData;
                        RefreshMapObject(dataContent);
                        RefreshMapHudUI(dataContent);
                    }
                    break;

                case CmdConstant.MapObjectHUDUpdate:
                    {
                        MapObjectInfoEntity mapItemInfo = notification.Body as MapObjectInfoEntity;
                        if (mapItemInfo == null)
                        {
                            return;
                        }
                        
                        var dataContent = new LodPopDataContent();
                        dataContent.mapEntity = m_worldMapProxy.GetWorldMapObjectByobjectId(mapItemInfo.objectId);
                        var obj = m_worldMapProxy.GetWorldMapObjectByobjectId(mapItemInfo.objectId);

                        if (obj == null)
                        {
                            return;
                        }

                        if (IsLodVisable(obj))
                        {
                            RefreshMapObject(dataContent);
                            RefreshMapHudUI(dataContent);
                        }

                        //联盟旗帜Icon在物体上
                        if (mapItemInfo.rssType == RssType.GuildFlag && obj.gameobject!=null)
                        {
                            if (mapItemInfo.guildFlagSigns!=null)
                            {
                                AllianceProxy.setFlag(obj.gameobject,mapItemInfo.guildFlagSigns);
                            }
                        }

//                        Debug.Log("++++MapObjectHUDUpdate" + mapItemInfo.objectId);
                    }
                    break;
                case CmdConstant.MapObjectRemove:
                    MapObjectInfoEntity del = notification.Body as MapObjectInfoEntity;
                    if (del == null)
                    {
                        return;
                    }
//                    Debug.Log("++++MapObjectRemove" + del.objectId);
                    RemoveByMapEntity(del.objectId);
                    RemoveViewCityTactical(del);
                    break;
//                case CmdConstant.MapRemoveTroopHud:
//                    var id = notification.Body is int ? (int) notification.Body : 0;
//                    
//                    Debug.Log("++++MapRemoveTroopHud" + id);
//                    DeleteElement(id);
//                    break;
                case Guild_ApplyJoinGuild.TagName:
                    if (notification.Type != RpcTypeExtend.RESPONSE_ERROR)
                    {
                        Guild_ApplyJoinGuild.response response = notification.Body as Guild_ApplyJoinGuild.response;

                        if (response.HasType && response.type == 2)
                        {
                            RefreshMapObjAndHud();
                        }
                    }
                    break;
                case CmdConstant.AllianceEixt:
                    RefreshMapObjAndHud();
                    break;
                case CmdConstant.NetWorkReconnecting:
                    ClearAllView();

                    break;
                case CmdConstant.SetSelectScoutHead:
                    long scountIndex = (long)notification.Body;
                    foreach (var data in m_scoutLodArmyDic)
                    {
                        if (data.Value.gameObject != null)
                        {
                            data.Value.m_img_choose_PolygonImage.gameObject.SetActive(data.Key == scountIndex);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            m_worldMapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_fogMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;

            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
            m_rssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_MonsterProxy=AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;

            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            InitCameraInfo();
            
            InitPopViewInfo();
             

             m_viewLevel = CalcMapViewLevel(WorldCamera.Instance().getCurrentCameraDxf());
        }

        protected override void BindUIEvent()
        {
            WorldCamera.Instance().AddViewChange(OnWorldViewChange);

            CoreUtils.inputManager.AddTouch2DEvent(OnTouchBegan,OnTouchMoved,OnTouchEnded);
            
            LevelDetailCamera.instance.AddLodChange(LodLevelChange);
        }

        
        

        protected override void BindUIData()
        {
        }

        public override void Update()
        {
           
        }

        private int DoCountPerUpdate = 5;
        public override void LateUpdate()
        {
            if (m_isUpdate)
            {
                for (int i = 0; i < DoCountPerUpdate; i++)
                {
                    var mapDatas = m_worldMapProxy.GetWorldMapObjects();
                    if (m_isUpdateMaxCount != mapDatas.Count)
                    {
                        m_isUpdateMaxCount = mapDatas.Count;
                        m_isUpdateIndex = 0;
                    }
                    if (m_isUpdateIndex < m_isUpdateMaxCount && m_isUpdateIndex>=0)
                    {
                        DoRefreshMapObjAndHud(m_isUpdateIndex);
                        m_isUpdateIndex++;
                    }
                    else
                    {
                        m_isUpdateIndex = 0;
                        m_isUpdate = false;
                        return;
                    }
                }
            }
            
            m_AllView.Sort((a, b) => -((a.dataContent.cacheZ  ).CompareTo(b.dataContent.cacheZ ) * 100
                                       + ((int)a.dataContent.rssType).CompareTo((int)b.dataContent.rssType) * 10
                                       + a.dataContent.objectId.CompareTo(b.dataContent.objectId)
                                       ));
            var t_SiblingIndex = 0;
            for (int i = 0; i < m_AllView.Count; i++)
            {
                var ele = m_AllView[i];

                var pos = ele.dataContent.GetPosValue();
                for (int j = 0; j < ele.lstViewContent.Count; j++)
                {
                    var view = ele.lstViewContent[j];
                    if (view.hud != null)
                    {
                        view.hud.SetPosition(pos.x,0,pos.z);
                        view.hud.transform.SetSiblingIndex(t_SiblingIndex);
                        ele.dataContent.cacheZ = pos.z;
                        t_SiblingIndex++;
                    }
                    if (view.obj != null)
                    {
                        view.obj.transform.position = pos;
                    }
                }
            }

            DeleteIdChangePlayerTroop();
        }

        public override void FixedUpdate()
        {
        }

        public override void OnRemove()
        {
            base.OnRemove();
            m_hudObjectLoading.Clear();

            ClearAllView();
            
            m_isViewCityID.Clear();
            m_isViewCityIDTactical.Clear();
            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);

            CoreUtils.inputManager.RemoveTouch2DEvent(OnTouchBegan,OnTouchMoved,OnTouchEnded);
            
            LevelDetailCamera.instance.RemoveLodChange(LodLevelChange);
        }

        #endregion

        #region 视图级别切换

        public void OnTouchBegan(int x, int y)
        {
            
        }

        public void OnTouchMoved(int x, int y)
        {
            RefreshMapObjAndHud();
        }
        
        public void OnTouchEnded(int x, int y)
        {
            //            Debug.Log("OnTouchEnded" + m_isGridUsed);
        }


        private void InitCameraInfo()
        {
            var cameraParams = CoreUtils.dataService.QueryRecords<CameraParamLodDefine>();
            List<WorldCamera.cameraInfoItem> cameraInfos = new List<WorldCamera.cameraInfoItem>();
            cameraParams.ForEach((cameraInfo) =>
            {
                WorldCamera.cameraInfoItem item = new WorldCamera.cameraInfoItem();

                item.dist = cameraInfo.dist;
                item.dxf = cameraInfo.dxf;
                item.forward = new Vector3(cameraInfo.forwardX, cameraInfo.forwardY, cameraInfo.forwardZ);
                item.fov = cameraInfo.fov;
                item.Id = cameraInfo.ID;

                cameraInfos.Add(item);
            });
            WorldCamera.Instance().ResetCamera(cameraInfos);

            TacticsToStrategy_Dxf_1 = WorldCamera.Instance().getCameraDxf("TacticsToStrategy1");
            TacticsToStrategy_Dxf_2 = WorldCamera.Instance().getCameraDxf("TacticsToStrategy2");
            HideStrategyObjDxf = WorldCamera.Instance().getCameraDxf("init");
            CityObjDxf = m_inCityCamHeight;
             //  CityBoundDxf = WorldCamera.Instance().getCameraDxf("city_bound"); 
            CityBoundDxf = 500f;
            minDxf = WorldCamera.Instance().getCameraDxf("min");
            a = (CityBoundDxf ) / endDIstance - startDIstance;
            b = minDxf - a * startDIstance;
            m_showProvinceNameDxfMin = WorldCamera.Instance().getCameraDxf("city_bound");
            m_showProvinceNameDxfMax = WorldCamera.Instance().getCameraDxf("dispatch");
        }

        private MapViewLevel CalcMapViewLevel(float dfx)
        {
            int lodLevel = LevelDetailCamera.instance.GetLodLevelByDxf(dfx);
            var viewLevel = CameraLevelToMapView[lodLevel];
            if (viewLevel == MapViewLevel.Strategic)
            {
                if (dfx <= TacticsToStrategy_Dxf_1)
                {
                    viewLevel = MapViewLevel.TacticsToStrategy_1;
                }
                else if (dfx <= TacticsToStrategy_Dxf_2)
                {
                    viewLevel = MapViewLevel.TacticsToStrategy_2;
                }
            }

            return viewLevel;
        }

        public MapViewLevel GetViewLevel()
        {
            return m_viewLevel;
        }

        public bool IsStrategic()
        {
            return m_viewLevel >= MapViewLevel.Strategic;
        }

        public MapViewLevel GetPreMapViewLevel()
        {
            return m_preViewLevel;
        }


        private void LodLevelChange(int preLod,int crrLod)
        {
//            Debug.Log("lod "+preLod+" => "+crrLod);
        }


        private ServerMapViewLevel CalcServerViewLevel(float dfx)
        {
            int lodLevel = LevelDetailCamera.instance.GetLodLevelByDxf(dfx);
            var viewLevel = CameraLevelToServer[lodLevel];
            if (viewLevel == ServerMapViewLevel.Strategic)
            {
                if (dfx <= TacticsToStrategy_Dxf_2)
                {
                    viewLevel = ServerMapViewLevel.Tactical;
                }
            }

            return viewLevel;
        }
        /// <summary>
        /// 判断是显示菜单，还是个人信息,只根据摄像头高度
        /// </summary>
        public bool IsMenuOrinfo()
        {
            bool IsMenu = true;
            if (m_viewDxf <= m_inCityCamHeight)
            {
                IsMenu = true;
            }
            else
            {
                IsMenu = false;
            }
            return IsMenu;
        }

        public bool IsInCity()
        {

            return m_isInCity;
        }

        public bool IsInSide()
        {
            float crrCamHeight = Common.GetLodDistance();
            if (crrCamHeight >= m_inCityCamHeight)
            {
                return false;
            }
            float cityInsideDxf = WorldCamera.Instance().getCameraDxf("city_bound");
            float currCameraDxf = WorldCamera.Instance().getCurrentCameraDxf();
            bool outSide = currCameraDxf > cityInsideDxf || !Common.IsInViewPort2D(WorldCamera.Instance().GetCamera(),
                   m_cityBuildingProxy.RolePos.x, 
                   m_cityBuildingProxy.RolePos.y,
                   0.3f * (cityInsideDxf / currCameraDxf) - 0.3f);
            if (outSide)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void OnWorldViewChange(float x, float y, float dxf)
        {
          
            if (m_canActive == false)
            {
                return;
            }
            
            m_viewDxf = dxf;
            
            float crrCamHeight = Common.GetLodDistance();
            var viewLevel = CalcMapViewLevel(dxf);
            
            bool isInCity = crrCamHeight <= m_inCityCamHeight;

            if (isInCity != m_isInCity)
            {
                m_isInCity = isInCity;
                if (isInCity)
                {
                    Debug.Log("进城"+m_inCityCamHeight);
                    FadeLod12Out();
                    if (m_fogMediator == null)
                    {
                        m_fogMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                    }
                    MapObjectInfoEntity request = GetRidOnWorldView();
                    AppFacade.GetInstance().SendNotification(CmdConstant.EnterCityShow, request);
                }
                else
                {
                    Debug.Log("出城"+m_inCityCamHeight);
                    FadeLod12In();
                    AppFacade.GetInstance().SendNotification(CmdConstant.ExitCityHide);
                }
                CheckTerritoryStrategicCanShow();
            }

            bool isShowProvince = m_viewDxf < m_showProvinceNameDxfMax && m_viewDxf > m_showProvinceNameDxfMin;
            if (!WorldCamera.Instance().IsAutoMoving() && isShowProvince != m_IsInShowProvince)
            {
                if(isShowProvince)
                {
                    Debug.Log($"显示省份:{m_viewDxf}");
                    var nameId = MapManager.Instance().GetMapProvinceName(new Vector2(x, y));
                    if (!nameId.Equals(string.Empty))
                    {
                        Debug.Log(nameId);
                        int lId;
                        if (int.TryParse(nameId, out lId))
                        {
                            Tip.CreateTip(lId, Tip.TipStyle.City).Show();
                        }
                    }
                }
                m_IsInShowProvince = isShowProvince;
            }

            if (!WorldCamera.Instance().IsAutoMoving() && !WorldCamera.Instance().IsSlipping())
            {
                if (viewLevel >= MapViewLevel.Tactical && viewLevel < MapViewLevel.TacticsToStrategy_1)
                {
                    WorldMapLogicMgr.Instance.BattleSoundHandler.RefreshBattleSoundHitPos();
                }
            }

            if (viewLevel != m_viewLevel)
            {
                var oldViewLevel = m_viewLevel;
                m_preViewLevel = oldViewLevel;
                m_viewLevel = viewLevel;

                if (oldViewLevel > MapViewLevel.Tactical && MapViewLevel.Tactical <= viewLevel)
                {
                    CheckTerritoryStrategicCanShow();                 
                }
                else if (MapViewLevel.Tactical <= oldViewLevel && viewLevel > MapViewLevel.Tactical)
                {
                    CheckTerritoryStrategicCanShow();
                }


                if (oldViewLevel < MapViewLevel.Strategic && MapViewLevel.Strategic <= viewLevel)
                {
                    TacticalViewToStrategic();
                    
                    m_RssProxy.SendMapMove((long) x, (long) y);
                }
                else 
                {
                    if (MapViewLevel.Strategic <= oldViewLevel && viewLevel < MapViewLevel.Strategic)
                    {
                        StrategicViewToTactical();
                    
                        m_RssProxy.SendMapMove((long) x, (long) y);
                    }
                    
                }

                if (oldViewLevel < MapViewLevel.TacticsToStrategy_2 && MapViewLevel.TacticsToStrategy_2 <= viewLevel)
                {
                    TacticalViewToStrategic_Fade();
                }
                else 
                {
                    if (MapViewLevel.TacticsToStrategy_2 <= oldViewLevel &&
                        viewLevel < MapViewLevel.TacticsToStrategy_2)
                    {
                        StrategicViewToTactical_Fade();
                    }
                    
                }

                if (oldViewLevel != MapViewLevel.TacticsToStrategy_1 && viewLevel == MapViewLevel.TacticsToStrategy_1)
                {
                    EnterTacticalToStrategic_L1();
                }
                else 
                {
                    if (oldViewLevel == MapViewLevel.TacticsToStrategy_1 &&
                                             viewLevel != MapViewLevel.TacticsToStrategy_1)
                    {
                        LeaveTacticalToStrategic_L1();
                    }
                    
                }

                if ((oldViewLevel < MapViewLevel.TacticsToStrategy_1 ||
                     MapViewLevel.TacticsToStrategy_2 < oldViewLevel) &&
                    MapViewLevel.TacticsToStrategy_1 <= viewLevel && viewLevel <= MapViewLevel.TacticsToStrategy_2)
                {
                    EnterTacticalToStrategic_L1L2();
                }
                else
                {
                    if (MapViewLevel.TacticsToStrategy_1 <= oldViewLevel && 
                        oldViewLevel <= MapViewLevel.TacticsToStrategy_2 &&
                        (viewLevel < MapViewLevel.TacticsToStrategy_1 || MapViewLevel.TacticsToStrategy_2 < viewLevel))
                    {
                        LeaveTacticalToStrategic_L1L2();
                    }
                }

                
                Debug.Log(m_preViewLevel + "镜头等级切换 =>" + m_viewLevel + "  lod:" +
                          ClientUtils.lodManager.GetLodDistance() + " lv:" + LevelDetailCamera.instance.GetCurrentLodLevel());
                AppFacade.GetInstance().SendNotification(CmdConstant.MapViewChange, m_viewLevel);

                UpdateMapProvinceName();

            }
            ControlCameraHgeight();

            RefreshMapObjAndHud();
        }
        private  const int endDIstance = 9;
        private  const int startDIstance =3;
        private float a = 0;
        private float b = 0;
        /// <summary>
        /// 控制摄像头高度
        /// </summary>
        private void ControlCameraHgeight()
        {
            if (GuideProxy.IsGuideing)
            {
                return;
            }
            if (!PlayerProxy.LoginInitIsFinish)
            {
                return;
            }
            float crrDxf = Common.GetLodDistance();
            if (crrDxf >= CityBoundDxf)
            {
                return;
            }

            Vector2 viewCenter_2 = WorldCamera.Instance().GetViewCenter();
            Vector3 viewCenter_3 = new Vector3(viewCenter_2.x, 0, viewCenter_2.y);

            float minDistance = 100;
            float temp = 0;
            var mapDatas = m_worldMapProxy.GetWorldMapObjectCitys();
            mapDatas.ForEach((objData) => {
                if (objData.objectPos!=null)
                {
                    temp = Vector3.Distance(new Vector3(objData.objectPos.x / 100, 0, objData.objectPos.y / 100), viewCenter_3);
                    //  Debug.LogErrorFormat("{0},,,{1}", temp, new Vector3(objData.objectPos.x / 100, 0, objData.objectPos.y / 100));
                    if (temp < minDistance)
                    {
                        minDistance = temp;
                    }
                }
            });
            float TargetDxf = ((minDistance) * a+b);
           //    Debug.LogErrorFormat("{0},,,,{1},,,{2},,,{3},,,{4}", crrDxf, CityBoundDxf, TargetDxf, minDistance, viewCenter_3);
            if (TargetDxf > CityBoundDxf)
            {
                TargetDxf = CityBoundDxf;
            }
            if ( TargetDxf< minDxf)
            {
                TargetDxf = minDxf;
            }
            WorldCamera.Instance().setAdditionHeightForMinDxf((TargetDxf- minDxf));
        }
        /// <summary>
        /// 获取视野内的rid
        /// </summary>
        private MapObjectInfoEntity GetRidOnWorldView()
        {
            MapObjectInfoEntity objectInfo = null;
            var mapDatas = m_worldMapProxy.GetWorldMapObjects();

            for (int i = 0; i < mapDatas.Count; i++)
            {
                var objData = mapDatas[i];
                float x = objData.objectPos.x / 100;
                float z = objData.objectPos.y / 100;

                if (IsLodVisable(x, z))
                {
                    if (objData.objectType == 3)//判断是否为城市
                    {
                        if (IsCityInViewPort(x, z, objData.cityName))
                        {
                            objectInfo = objData;
                            break;
                        }
                    }
                }
            }
            return objectInfo;
        }

        /// <summary>
        /// 获取视野内的rid根据位置
        /// </summary>
        private MapObjectInfoEntity GetRidOnWorldView(long targetX,long targetZ)
        {
            MapObjectInfoEntity objectInfo = null;
            var mapDatas = m_worldMapProxy.GetWorldMapObjects();

            for (int i = 0; i < mapDatas.Count; i++)
            {
                var objData = mapDatas[i];
                float x = objData.objectPos.x / 100;
                float z = objData.objectPos.y / 100;

                if (IsLodVisable(x, z))
                {
                    if (objData.objectType == 3)//判断是否为城市
                    {
                        if (targetX ==x&& targetZ == z)
                        {
                            objectInfo = objData;
                            break;
                        }
                    }
                }
            }
            return objectInfo;
        }

        // 战术切换到战略
        private void TacticalViewToStrategic()
        {
            Debug.Log("TacticalViewToStrategic  2-5  3000-8000 "+ClientUtils.lodManager.GetLodDistance());
        }

        // 战略切换到战术
        private void StrategicViewToTactical()
        {
            Debug.Log("StrategicViewToTactical 5-2  8000-3000 "+ClientUtils.lodManager.GetLodDistance());
        }

        //隐藏1-2级
        private void TacticalViewToStrategic_Fade()
        {
            Debug.Log("StrategicViewToTactical_Fade lod  2-3 "+ClientUtils.lodManager.GetLodDistance());

            FadeLod12Out(); 
             
            // CheckTerritoryStrategicCanShow();
             
            //显示高级别领地样式
//            ShowStrategicTerritoryOutline();
        }
        
        //显示有1-2级
        private void StrategicViewToTactical_Fade()
        {
             
            FadeLod12In();
 
            Debug.Log("StrategicViewToTactical_Fade  lod  "+ClientUtils.lodManager.GetLodDistance());
 
            // CheckTerritoryStrategicCanShow();
            //显示低级别领地样式
//            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceTerritoryUpdate);
        }


        private void FadeLod12Out()
        {
            // TODO 该淡出方式处理不妥，经常忽明忽暗，待优化，目前先注释
            // SetLodUI12(MapElementUI.FadeType.AllFadeOut);
        }

        private void FadeLod12In()
        {
            // TODO 该淡出方式处理不妥，经常忽明忽暗，待优化，目前先注释
            // SetLodUI12(MapElementUI.FadeType.AllFadeIn);
        }


        private void SetLodUI12(MapElementUI.FadeType t)
        {
            int v = (int) t;

            for (int i = 0; i < m_AllView.Count; i++)
            {
                for (int j = 0; j < m_AllView[i].lstViewContent.Count; j++)
                {
                    var view = m_AllView[i].lstViewContent[j];
                    if (view.loadType == PopLoadType.Done && view.hud != null)
                    {
                        view.hud.SetUIFadeShow(v);
                    }
                }
            }
        }

       

        private void EnterTacticalToStrategic_L1()
        {
            Debug.Log("EnterTacticalToStrategic_L1  4501-4500 "+ClientUtils.lodManager.GetLodDistance());
            //CheckTerritoryStrategicCanShow();
        }

        private void LeaveTacticalToStrategic_L1()
        {
            
//            SetLodUI12(MapElementUI.FadeType.DirectOut);
            Debug.Log("LeaveTacticalToStrategic_L1  4500-4501 "+ClientUtils.lodManager.GetLodDistance());
            //CheckTerritoryStrategicCanShow();
        }

        private void EnterTacticalToStrategic_L1L2()
        {
           
        }

        private void LeaveTacticalToStrategic_L1L2()
        {
            
        }

        public void SetCanActive(bool active)
        {
            m_canActive = active;
        }

        public void CheckTerritoryStrategicCanShow()
        {
            if (m_isInCity)
            {
                Debug.Log("领土lod不可见");
                ManorMgr.SetStrategicShow_S(false,false);
            }
            else
            {
                Debug.Log(m_viewLevel+"领土lod可见"+isShowTerritory+"  "+ClientUtils.lodManager.GetLodDistance());

                if (m_viewLevel <= MapViewLevel.Tactical)
                {
                    ManorMgr.SetStrategicShow_S(false,true);
                }
                else
                {
                    if (isShowTerritory)
                    {
                        ManorMgr.SetStrategicShow_S(true,false);
                    }
                    else
                    {
                        ManorMgr.SetStrategicShow_S(false,false);
                    }

                    //TerritoryMgr.ClearAllLine_S(true, false);
                }
            }
        }
        public void StartShowHud()
        {
            if (m_fogMediator == null)
            {
                m_fogMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            }
            RefreshMapObjAndHud();
        }

        public void RefreshMapObjAndHud()
        {
            m_isUpdate = true;
        }
        public void DoRefreshMapObjAndHud(int index)
        {
            
            var mapDatas = m_worldMapProxy.GetWorldMapObjects();
            if (mapDatas.Count<=index || index<0)
            {
                Debug.LogWarning("DoRefreshMapObjAndHud index out of range.... why?  index:" + index + " mapDatas.Count"+mapDatas.Count);
                return;
            }
            if (m_viewLevel > MapViewLevel.Tactical)
            {
                m_isViewCityID.Clear();
            }
            

            // var hideObjectId = new List<long>();
//            for (int i = 0; i < mapDatas.Count; i++)
//            {
                var objData = mapDatas[index];

                if (objData == null || objData.objectPos == null )
                {
                    
                }
                else
                {
                    float x = objData.objectPos.x / 100;
                    float z = objData.objectPos.y / 100;
                    if (IsLodVisable(objData))
                    {
                        var dataContent = new LodPopDataContent();
                        dataContent.mapEntity = objData;
                        var army = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)objData.objectId);
                        if (army != null)
                        {
                            dataContent.armyData = army;
                        }
                        
                        if (m_viewLevel <= MapViewLevel.TacticsToStrategy_1)
                        {
                            if (objData.objectType == 3 )//判断是否为城市
                            {
                                if (IsCityInViewPort(x,z,objData.cityName))
                                {
                                    if (!m_isViewCityID.Contains(objData.cityRid))
                                    {
                                        m_isViewCityID.Add(objData.cityRid);
                                       // Debug.LogError("城堡进入视野"+objData.cityName);
                                        AppFacade.GetInstance().SendNotification(CmdConstant.CityInViewPort, objData.cityRid);
                                    }
                                }
                                else
                                {
                                    RemoveViewCity(objData);
                                }
                                if (IsCityInViewPortTactical(x, z, objData.cityName))
                                {
                                    if (!m_isViewCityIDTactical.Contains(objData.cityRid))
                                    {
                                        m_isViewCityIDTactical.Add(objData.cityRid);
                                    //    Debug.LogError("城堡进入视野" + objData.cityRid);
                                        AppFacade.GetInstance().SendNotification(CmdConstant.LoadMapObj, objData);
                                    }
                                }
                                else
                                {
                                    RemoveViewCityTactical(objData);
                                }
                            }
                        }
                        
                        //如果部队是自己的但是m_worldMapProxy还是包含这里判空一下自己是否有这个部队，没有就不用刷新了
                        if (objData.armyRid == m_playerProxy.CurrentRoleInfo.rid && dataContent.armyData == null)
                        {
                            return;
                        }
                        RefreshMapObject(dataContent);
                        RefreshMapHudUI(dataContent);
                    }
                    else
                    {
    //                    HideHudOutView(objData);
                        // hideObjectId.Add(objData.objectId);
                        
                        if (objData.objectType == 3 && m_viewLevel <= MapViewLevel.Tactical)
                        {
                            RemoveViewCity(objData);
                            RemoveViewCityTactical(objData);
                        }
                    }
                }
//            }

            for (int i = 0; i < m_AllView.Count; i++)
            {
                if (m_AllView[i].dataContent.armyRid == m_playerProxy.CurrentRoleInfo.rid
                    && 
                    (m_AllView[i].dataContent.objectType == (int) RssType.Troop
                     ||m_AllView[i].dataContent.objectType == (int) RssType.Scouts
                     ||m_AllView[i].dataContent.objectType == (int) RssType.Transport)
                    )
                {
                    RefreshMapObject(m_AllView[i].dataContent);
                    RefreshMapHudUI(m_AllView[i].dataContent);
                }

                // if (hideObjectId.Contains(m_AllView[i].dataContent.objectId))
                // {
                //     for (int j = 0; j < m_AllView[i].lstViewContent.Count; j++)
                //     {
                //         if (m_AllView[i].lstViewContent[j].hud != null)
                //             m_AllView[i].lstViewContent[j].hud.SetUIFadeShow((int)MapElementUI.FadeType.DirectOut);
                //     }
                // }
            }
        }


        private void RemoveViewCity(MapObjectInfoEntity objData)
        {
            if (m_isViewCityID.Contains(objData.cityRid))
            {
                m_isViewCityID.Remove(objData.cityRid);
               // Debug.LogError("城堡city移除视野"+objData.cityName);
            }
            
        }
        private void RemoveViewCityTactical(MapObjectInfoEntity objData)
        {
            if (m_isViewCityIDTactical.Contains(objData.cityRid))
            {
                m_isViewCityIDTactical.Remove(objData.cityRid);
               // Debug.LogError("城堡移除视野" + objData.cityRid);
            }

        }
        #endregion

        #region 视野
        private bool IsLodVisable(MapObjectInfoEntity objData)
        {

            var x = objData.objectPos.x / 100;
            var y = objData.objectPos.y / 100;
            if (objData.gameobject != null)
            {
                x = (long) objData.gameobject.transform.position.x;
                y = (long) objData.gameobject.transform.position.z;
            }
            var result = IsLodVisable(x,y);
            if (result)
            {
                return true;
            }
            else
            {
                if (objData != null && objData.armyRid == m_playerProxy.CurrentRoleInfo.rid)
                {
                    if (objData.objectType == (long)RssType.Scouts)
                    {
                        float scoutx, scouty = 0;
                        ArmyData armyData = null;
                        armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId((int)objData.scoutsIndex);
                        if (armyData != null&& armyData.go!=null)
                        {
                            scoutx = armyData.go.transform.position.x;
                            scouty = armyData.go.transform.position.z;
                            if (m_fogMediator != null && Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), scoutx, scouty))
                            {
                                return true;
                            }
                        }
                    }
                    else if(objData.objectType == (long)RssType.Troop)
                    {
                        float scoutx, scouty = 0;
                        ArmyData armyData = null;
                        armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)objData.objectId);
                        if (armyData != null && armyData.go != null)
                        {
                            scoutx = armyData.go.transform.position.x;
                            scouty = armyData.go.transform.position.z;
                            if (m_fogMediator != null && Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), scoutx, scouty))
                            {
                                if (!m_fogMediator.HasFogAtWorldPos(scoutx, scouty))
                                {
                                    return true;
                                }
                            }
                        }
                    }  
                }
            }
            return false;   
        }
        //lod是否在迷雾内  是否在屏幕内
        public bool IsLodVisable(float x, float y)
        {
            var isInView = Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y);
            if (m_fogMediator!=null && isInView)
            {
                if (!m_fogMediator.HasFogAtWorldPos(x, y))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 城堡进入视野（层级city）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsCityInViewPort(float x, float y,string name)
        {
            if (m_viewLevel == MapViewLevel.City)
            {
                bool isInView = Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y, name);
                return isInView;
            }

            return false;
        }
        /// <summary>
        /// 城堡进入视野（层级Tactica）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsCityInViewPortTactical(float x, float y, string name)
        {
            if (m_viewLevel <= MapViewLevel.Tactical)
            {
                bool isInView = Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y, name);
                return isInView;
            }
            return false; 
        }
        #endregion

        #region SetXXXObj

        //城市颜色设置
        bool Check_SetCityObj( LodPopDataContent data)
        {
            if(data.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                return true;
            else
            {
                return m_viewLevel < MapViewLevel.Strategic;
            }
        }
        public void SetCityObj(GameObject obj,LodPopDataContent data)
        {
            var lod = obj.GetComponent<LevelDetailScale>();
            
            if (data.cityRid == m_playerProxy.CurrentRoleInfo.rid)    // 自己，绿色
            {
                lod.IsExplore = true;
            }

            
            Color color = GetCityColor(data);
            var helper = obj.GetComponent<ChangeSpriteColor>();
            if (helper!=null)
            {
                ChangeSpriteColor.SetColor(helper, color);
            }
            obj.SetActive(Check_SetCityObj(data));
        }

        private bool Check_SetCityObj_Effect(LodPopDataContent data)
        {
            return data.cityRid == m_playerProxy.CurrentRoleInfo.rid;
        }
        public void SetCityObj_Effect(GameObject obj, LodPopDataContent data ,Transform parent)
        {
            obj.gameObject.SetActive(false);
            if(data.cityRid != m_playerProxy.CurrentRoleInfo.rid)
                return;
//            for (int i = 0; i < m_AllView.Count; i++)
//            {
//                var v = m_AllView[i];
//                if (data.objectId == v.dataContent.objectId && v.config.prefabName == "city_lod3")
//                {
                    obj.gameObject.SetActive(true);
//                    var parent = parent;
                    obj.transform.SetParent(parent);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;
//                    break;
//                }
//            }
        }
        public void SetVillageObj(GameObject obj, LodPopDataContent data)
        {
            string icon = string.Empty;
            icon = RS.village_lod3_icon[0];
            if (data.villageState)
            {
                icon =  RS.village_lod3_icon[1];
            }
            var image = obj.GetComponent<SpriteRenderer>();
            if (image != null)
            {
                ClientUtils.LoadSprite(image, icon);
            }
//            var lod = obj.GetComponent<LodAutoScale>();
//            lod.IsExplore = true;
//            obj.SetActive(Check_Toggle_Explore(data));
//            AddObjElement(data.objectId , obj);
        }
        public void SetCaveObj(GameObject obj, LodPopDataContent data)
        {
            string icon = string.Empty;
            icon = RS.cave_lod3_icon[0];
            if (data.villageState)
            {
                icon = RS.cave_lod3_icon[1];
            }
            var image = obj.GetComponent<SpriteRenderer>();
            if (image != null)
            {
                ClientUtils.LoadSprite(image, icon);
            }
//            var lod = obj.GetComponent<LodAutoScale>();
//            lod.IsExplore = true;
//            obj.SetActive(Check_Toggle_Explore(data));
        }

        private void SetBarbarianObj(GameObject obj, LodPopDataContent data)
        {
//            var lod = obj.GetComponent<LodAutoScale>();
//            if (lod != null)
//            {           
//                lod.IsExplore = true; 
//            }
        }

        public bool Check_SetTroopObj(LodPopDataContent data)
        {
            if (data.armyRid == m_playerProxy.CurrentRoleInfo.rid)
            {
                if (TroopHelp.IsHaveState(data.status , ArmyStatus.COLLECTING)  || TroopHelp.IsHaveState(data.status , ArmyStatus.GARRISONING ))
                {
                    return m_viewLevel >= MapViewLevel.Strategic;
                }
                return true;
            }
            else
            {
                return m_viewLevel <= MapViewLevel.TacticsToStrategy_2;
            }
            
        }
//        public bool Check_SetArmyObj(LodPopDataContent data)
//        {
//            bool isTroopShow = Check_SetTroopObj(data);
//            
//            return isTroopShow && !TroopHelp.IsHaveState(data.status , ArmyStatus.COLLECTING)  && !TroopHelp.IsHaveState(data.status , ArmyStatus.GARRISONING);
//        }
//        
        public void  SetArmyObj(GameObject obj, LodPopDataContent data)
        {
            if (data.armyRid == m_playerProxy.CurrentRoleInfo.rid)
            {
                if (data.armyData == null)
                {
                    data.armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId((int)data.armyIndex);
                }
            }
            else
            {
                if (data.armyData == null)
                {
                    data.armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)data.objectId);
                }
            }
            
            var c = Color.white;
            var lod = obj.GetComponent<LevelDetailScale>();

            if (data.armyData != null)
            {
                if (data.armyData.isPlayerHave)
                {
                    lod.IsExplore = true;
                    c = RS.LodCityMine;
                }else if (data.armyData.guild == m_playerProxy.CurrentRoleInfo.guildId && m_playerProxy.CurrentRoleInfo.guildId != 0)
                {
                    c = RS.LodCityAlly;
                }
                else
                {
                    c = HUDHelp.GetOtherTroopColor(data.armyData);
                }
            }

            var helper = obj.GetComponent<ChangeSpriteColor>();
            if (helper != null)
            {
                ChangeSpriteColor.SetColor(helper, c);
            }
            obj.SetActive(Check_SetTroopObj(data));
        }
        
        public void SetScoutObj(GameObject obj, LodPopDataContent data)
        {
            if (data.armyData == null)
            {
                data.armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId((int)data.scoutsIndex);
            }

            var c = Color.white;
            var lod = obj.GetComponent<LevelDetailScale>();

//            if (data.armyData != null)
//            {
                if (data.armyData != null && data.armyData.isPlayerHave)
                {
                    lod.IsExplore = true;
                    c = RS.LodCityMine;
                }
                else if ( m_playerProxy.CurrentRoleInfo.guildId != 0 && data.guildId == m_playerProxy.CurrentRoleInfo.guildId)
                {
                    c = RS.blue;
                }
//            }
            var helper = obj.GetComponent<ChangeSpriteColor>();
            if (helper != null)
            {
                ChangeSpriteColor.SetColor(helper, c);
            }
            obj.SetActive(Check_SetTroopObj(data));
        }

        public void SetTransportObj(GameObject obj, LodPopDataContent data)
        {
            if (data.armyData == null)
            {
                data.armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTransportData((int)data.objectId);
            }
            var c = Color.white;
            var lod = obj.GetComponent<LevelDetailScale>();

            if (data.armyData != null)
            {
                if (data.armyData.isPlayerHave)
                {
                    lod.IsExplore = true;
                    c = RS.LodCityMine;
                }else if (m_playerProxy.CurrentRoleInfo.guildId != 0 && data.armyData.guild == m_playerProxy.CurrentRoleInfo.guildId)
                {
                    c = RS.LodCityAlly;
                }
            }
            var helper = obj.GetComponent<ChangeSpriteColor>();
            if (helper != null)
            {
//                Debug.Log("++++设置颜色 c " + c.ToString() + " obj.name :" + obj.name);
                ChangeSpriteColor.SetColor(helper, c);
            }
            obj.SetActive(Check_SetTroopObj(data));
        }

        bool Check_Toggle_Explore(LodPopDataContent data)
        {
            if (m_viewLevel >= MapViewLevel.Strategic)
            {
                return m_curToggleType == LodMenuToggle.Explore;
            }
            return true;
        }
        bool Check_Toggle_Resoureces(LodPopDataContent data)
        {
            if (m_viewLevel >= MapViewLevel.Strategic)
            {
                return m_curToggleType == LodMenuToggle.Resoureces;
            }
            return true;
        }
        bool Check_Toggle_Alliance(LodPopDataContent data)
        {
            if (m_viewLevel >= MapViewLevel.Strategic)
            {
                return m_curToggleType == LodMenuToggle.Alliance;
            }
            return true;
        }

        public void SetGuildBuildObj(GameObject obj, LodPopDataContent data)
        {


            var c = RS.LodGuildBuildAlly;
            
            if (data.guildId == m_playerProxy.CurrentRoleInfo.guildId)
            {
                c = RS.LodGuildBuildMine;
            }
            
            
            var helper = obj.GetComponent<ChangeSpriteColor>();
            if (helper != null)
            {
                ChangeSpriteColor.SetColor(helper, c);
            }
            
            obj.SetActive(Check_Toggle_Alliance(data));
        }
        
        public void SetGuildFlagObj(GameObject obj, LodPopDataContent data)
        {

            var helper = obj.GetComponent<ChangeSpriteColor>();

            if (helper!=null)
            {
                var flagColorDefine = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int)data.guildFlagSigns[1]);
                
                Color color = Color.white;

//                if (data.mapEntity.guildBuildStatus!=0)
//                {
                    ColorUtility.TryParseHtmlString(flagColorDefine.colour, out color);
//                }
                ChangeSpriteColor.SetColor(helper,color);
            }
        }

        
        public void SetGuildRssObj(GameObject obj, LodPopDataContent data)
        {
            var lod = obj.GetComponent<LevelDetailScale>();
            lod.IsExplore = true;
            obj.SetActive(Check_Toggle_Resoureces(data));
        }
        public void SetGuildSuperRssObj(GameObject obj, LodPopDataContent data)
        {
            var lod = obj.GetComponent<LevelDetailScale>();
            lod.IsExplore = true;
            obj.SetActive(Check_Toggle_Alliance(data));
        }

        public void SetCheckPointObj(GameObject obj, LodPopDataContent data)
        {
            Color color = GetCheckPointColor(data);
                        
            var helper = obj.GetComponent<ChangeSpriteColor>();
            if (helper!=null)
            {
                ChangeSpriteColor.SetColor(helper, color);
            }
            var lod = obj.GetComponent<LevelDetailScale>();
            lod.IsExplore = true;
            lod.UpdateLod();
        }
        public void SetHolyLandObj(GameObject obj, LodPopDataContent data)
        {
            Color color = GetHolyLandColor(data);
                        
            var helper = obj.GetComponent<ChangeSpriteColor>();
            if (helper!=null)
            {
                ChangeSpriteColor.SetColor(helper, color);
            }
            var lod = obj.GetComponent<LevelDetailScale>();
            lod.IsExplore = true;
            lod.UpdateLod();
        }

        #endregion
        #region SetXXXHud 

        public void SetCityHud(GameObject obj, LodPopDataContent data)
        {

            UI_Tip_WorldObjectPlayerLevelView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectPlayerLevelView>(obj);

            view.gameObject.SetActive(data.isFight);
            if (view.m_lbl_level_LanguageText != null)
            {
                if (data.guildId == 0)
                {
                    view.m_lbl_playerName_LanguageText.text = data.cityName;
                }
                else
                {
                    view.m_lbl_playerName_LanguageText.text = LanguageUtils.getTextFormat(300030, data.guildAbbName, data.cityName);
                }

                if (data.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                {
                    if (!m_guideProxy.IsCompletedByStage(m_playerProxy.ConfigDefine.guideHideCityName))
                    {
                        view.m_lbl_playerName_LanguageText.text = LanguageUtils.getText(300110);
                    }

                }

                view.m_lbl_playerName_LanguageText.color = GetCityColor(data);

                if (data.cityLevel <= 0)
                {
                    //  CoreUtils.logService.Error($"[SetCityHud]    服务器错误  下发了等级小于等于0的主城   cityLevel:[{data.cityLevel}]");
                }

                view.m_lbl_level_LanguageText.text = data.cityLevel.ToString();
                view.m_img_cion_PolygonImage.gameObject.SetActive(data.cityCountry == 1);
            }
        }

        private bool Check_SetCityHudExt(LodPopDataContent data)
        {
            return data.cityRid == m_playerProxy.CurrentRoleInfo.rid;
        }

        private void SetCityHudExt(GameObject obj, LodPopDataContent data)
        {
            UI_Pop_WorldObjectLod3PlayerView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_WorldObjectLod3PlayerView>(obj);

            if (view != null)
            {
                view.m_UI_Model_PlayerHead.LoadPlayerIcon();
            }
        }

//        private bool Check_SetRssHudExt(LodPopDataContent data)
//        {
//            return data.collectRid == m_playerProxy.CurrentRoleInfo.rid;
//        }
        private void SetRssHudExt(GameObject obj, LodPopDataContent data)
        {
            UI_Tip_WorldObjectLodCaptainView view =  MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectLodCaptainView>(obj);
            if (view != null)
            {    
                view.gameObject.SetActive(data.collectRid == m_playerProxy.CurrentRoleInfo.rid);
                 if (data.objectType == (long)RssType.Stone || data.objectType == (long)RssType.Farmland || data.objectType == (long)RssType.Gold || data.objectType == (long)RssType.Wood)
                {
                    var armyInfo = m_TroopProxy.GetArmyByIndex(data.armyIndex);
                    if(armyInfo != null && armyInfo.armyIndex == data.armyIndex)
                    {
                        var heroConfig = CoreUtils.dataService.QueryRecord<HeroDefine>((int)armyInfo.mainHeroId);
                        if(heroConfig != null)
                        {
                            view.m_UI_Model_CaptainHead.SetIcon(heroConfig.heroIcon);
                            view.m_UI_Model_CaptainHead.SetRare(heroConfig.rare);
                            
                        }         
                        view.m_UI_Model_CaptainHead.gameObject.SetActive(true);              
                    }
                    else
                    {
                        view.m_UI_Model_CaptainHead.gameObject.SetActive(false);
                    }

                    GuildMemberInfoEntity  guildMemberInfoEntity= m_allianceProxy.getMemberInfo(data.collectRid);
                    string iconPath = RS.StateCollect1;
                    if (guildMemberInfoEntity != null)
                    {
                        iconPath = RS.StateCollect2;
                    }
                    ClientUtils.LoadSprite(view.m_img_state_PolygonImage, iconPath);
                }
            }
        }

        public void SetTransportHud(GameObject obj, LodPopDataContent data)
        {
            UI_Tip_WorldObjectLodArmyView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectLodArmyView>(obj);
            if (view != null)
            {
                if (data.objectType == (long) RssType.Transport)
                {
                    view.m_UI_Model_CaptainHead.SetIcon(CoreUtils.dataService.QueryRecord<ConfigDefine>(0).transportIcon);
                    ClientUtils.LoadSprite(view.m_img_state_PolygonImage, RS.StateTransport);
                }
                view.m_img_choose_PolygonImage.gameObject.SetActive(false);
            }
            if (data.armyData == null)
            {
                data.armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTransportData((int)data.objectId);
            }
        }
        //        private void SetGuardianHud(GameObject obj, LodPopDataContent data)
        //        {
        //            UI_Tip_WorldObjectRssLevelView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectRssLevelView>(obj);
        //            if (view != null)
        //            {
        //                var monsterCfg = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)data.monsterId);
        //                if (monsterCfg != null)
        //                {
        //                    view.m_img_bg_PolygonImage.gameObject.SetActive(true);
        //                    ClientUtils.LoadSprite(view.m_img_bg_PolygonImage,
        //                        monsterCfg.subscriptIcon,true);
        //                    
        ////                    view.m_lbl_icon_PolygonImage.gameObject.SetActive(false);
        ////                    view.m_lbl_level_LanguageText.gameObject.SetActive(false);
        //                }
        //            }
        //        }

        private void SetSummonAttackBarbarianHud(GameObject obj, LodPopDataContent data)
        {
            UI_Tip_GuardianLevelView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_GuardianLevelView>(obj);
            if (view != null)
            {
                var monsterCfg = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)data.monsterId);
                if (monsterCfg != null)
                {
                    ClientUtils.LoadSprite(view.m_img_bg_PolygonImage, monsterCfg.subscriptIcon, true);
                }
            }
        }

        private void SetSummonConcentrateBarbarianHud(GameObject obj, LodPopDataContent data)
        {
            UI_Tip_GuardianLevelView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_GuardianLevelView>(obj);
            if (view != null)
            {
                var monsterCfg = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)data.monsterId);
                if (monsterCfg != null)
                {
                    ClientUtils.LoadSprite(view.m_img_bg_PolygonImage, monsterCfg.subscriptIcon, true);
                }
            }
        }

        private void SetBarbarGuardianHud(GameObject obj, LodPopDataContent data)
        {
            UI_Tip_GuardianLevelView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_GuardianLevelView>(obj);
            if (view != null)
            {
                var monsterCfg = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)data.monsterId);
                if (monsterCfg != null)
                {
                    ClientUtils.LoadSprite(view.m_img_bg_PolygonImage, monsterCfg.subscriptIcon,true);
                }
            }
        }

        private void SetBarbarCityHud(GameObject obj, LodPopDataContent data)
        {
            UI_Tip_WorldObjectRssLevelView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectRssLevelView>(obj);
            if (view != null)
            {
//                view.m_lbl_icon_PolygonImage.gameObject.SetActive(false);
                view.m_lbl_level_LanguageText.gameObject.SetActive(true);
                var monsterCfg = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)data.monsterId);
                if (monsterCfg != null)
                {
                    view.m_lbl_level_LanguageText.text = monsterCfg.level.ToString();
                }
//                view.m_img_bg_PolygonImage.gameObject.SetActive(true);
//                ClientUtils.LoadSprite(view.m_img_bg_PolygonImage, RS.RssLevel_Bg,true);
            }
        }

        public void SetBarbarianHud(GameObject obj,LodPopDataContent data)
        {
            UI_Tip_WorldObjectRssLevelView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectRssLevelView>(obj);
            if (view.m_lbl_level_LanguageText!=null)
            {
                var monsterCfg = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)data.monsterId);
                if (monsterCfg != null)
                {
//                    view.m_lbl_level_LanguageText.gameObject.SetActive(true);
                    view.m_lbl_level_LanguageText.text = monsterCfg.level.ToString();
//                    view.m_lbl_icon_PolygonImage.gameObject.SetActive(data.IsShowRssHud);
//                    if (!string.IsNullOrEmpty(data.showHudicon))
//                    {
//                        ClientUtils.LoadSprite(view.m_lbl_icon_PolygonImage, data.showHudicon);
//                    }
//                    view.m_img_bg_PolygonImage.gameObject.SetActive(true);
//                    if (!string.IsNullOrEmpty(monsterCfg.subscriptIcon))
//                    {
//                        ClientUtils.LoadSprite(view.m_img_bg_PolygonImage, monsterCfg.subscriptIcon,true);
//                    }
//                    else
//                    {
//                        ClientUtils.LoadSprite(view.m_img_bg_PolygonImage, RS.RssLevel_Bg,true);
//                    }
                }           
            }
            
        }
        public bool Check_SetScoutHudExt(LodPopDataContent data)
        {
            return data.armyRid == m_playerProxy.CurrentRoleInfo.rid;
        }

        /// <summary>
        /// 斥候
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="data"></param>
        public void SetScoutHudExt(GameObject obj, LodPopDataContent data)
        {
            UI_Tip_WorldObjectLodArmyView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectLodArmyView>(obj);

            if (view != null)
            {
                if (data.armyRid == m_playerProxy.CurrentRoleInfo.rid)
                {
                    string icon = ScoutProxy.GetScoutIcon((int) data.armyIndex);
                    if (!string.IsNullOrEmpty(icon))
                    {
                        view.m_UI_Model_CaptainHead.SetIcon(icon);
                    }
                    else
                    {
                        icon = ScoutProxy.GetScoutIcon((int) data.scoutsIndex);
                        if (!string.IsNullOrEmpty(icon))
                        {
                            view.m_UI_Model_CaptainHead.SetIcon(icon);
                        }
                    }
                    //Debug.LogErrorFormat("状态:{0} 选中:{1}", data.scoutsIndex, m_scoutProxy.GetCurrSelectIndex());
                    view.m_img_choose_PolygonImage.gameObject.SetActive(data.scoutsIndex == m_scoutProxy.GetCurrSelectIndex());


                    ClientUtils.LoadSprite(view.m_img_state_PolygonImage, TroopHelp.GetIcon(data.status));
                    view.m_img_state_PolygonImage.gameObject.SetActive(true);
                    if (data.armyData == null)
                    {
                        data.armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId((int)data.scoutsIndex);
                    }
                    m_scoutLodArmyDic[data.scoutsIndex] = view;
                }
                else
                {
                    view.gameObject.SetActive(false);
                }
                
            }
        }

        public bool Check_SetRssStateHud(LodPopDataContent data)
        {
            var rssData = m_worldMapProxy.GetWorldMapObjectByobjectId(data.objectId);
            if (rssData != null)
            {
                var isShowIcon = rssData.isShowHud;
                isShowIcon = isShowIcon && (data.collectRid != 0);
                isShowIcon = isShowIcon && (
                                 (data.collectRid == m_playerProxy.CurrentRoleInfo.rid &&
                                  m_viewLevel <= MapViewLevel.Tactical)
                                 || (data.collectRid != m_playerProxy.CurrentRoleInfo.rid &&
                                     m_viewLevel <= MapViewLevel.TacticsToStrategy_1)
                             );
                isShowIcon = isShowIcon && data.isFight;
                return isShowIcon;
            }

            var armys = SummonerTroopMgr.Instance.GetISummonerTroop().GetSummonerArmyDatas();
            for (int i = 0; i < armys.Count; i++)
            {
                if (armys[i].troopType == RssType.Troop && armys[i].targetId == data.objectId) //检查军队
                {
                    return true;
                }
            }

            return false;
        }
        public void SetRssStateHud(GameObject obj,LodPopDataContent data)
        {
            UI_Tip_ComBuildStateView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_ComBuildStateView>(obj);
            
            var isShow = Check_SetRssStateHud(data);
            
            view.m_img_bg_PolygonImage.gameObject.SetActive(isShow);
            view.m_pl_distance.transform.gameObject.SetActive(false);
            if (isShow)
            {
                // obj.GetComponent<MapElementUI>().SetUIFadeShow((int) MapElementUI.FadeType.DirectIn);
                var rssData = m_worldMapProxy.GetWorldMapObjectByobjectId(data.objectId);
                if (rssData != null)
                {
                    if (view.m_lbl_icon_PolygonImage != null)
                    {
                        if (!string.IsNullOrEmpty(rssData.showHudIcon))
                        {
                            ClientUtils.LoadSprite(view.m_lbl_icon_PolygonImage, rssData.showHudIcon);
                        }
                    }
                }
            }
        }
        public void SetRssHud(GameObject obj,LodPopDataContent data)
        {
            if (data.mapEntity!=null)
                m_rssProxy.UpdateRss(data.mapEntity);
                
            UI_Tip_WorldObjectRssLevelView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectRssLevelView>(obj);
            var rssData = m_worldMapProxy.GetWorldMapObjectByobjectId(data.objectId);
            if (rssData != null)
            {
                if (view.m_lbl_level_LanguageText != null)
                {
//                    view.m_lbl_level_LanguageText.gameObject.SetActive(false);
                    ResourceGatherTypeDefine resouceGatherCfg = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>((int)rssData.resourceId);
                    if(resouceGatherCfg != null)
                    {
                        view.m_lbl_level_LanguageText.gameObject.SetActive(true);
                        view.m_lbl_level_LanguageText.text = resouceGatherCfg.level.ToString();
                    } 
                }

                if (data.collectRid == m_playerProxy.CurrentRoleInfo.rid)
                {
                    var playerHeadData = m_worldMapProxy.GetWorldMapObjectByobjectId(data.objectId);
                    if (playerHeadData == null)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectChange, playerHeadData);
                    }
                }
//                view.m_img_bg_PolygonImage.gameObject.SetActive(true);
//                ClientUtils.LoadSprite(view.m_img_bg_PolygonImage, RS.RssLevel_Bg,true);
                
            }
        }
        /// <summary>
        /// 山洞
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="data"></param>
        public void SetCavePointHud(GameObject obj, LodPopDataContent data)
        {
            if(data.mapEntity != null)
                m_rssProxy.UpdateResourcePoint(data.mapEntity);
            UI_Tip_WorldCaveMarkView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldCaveMarkView>(obj);
            var mapFixPointDefine = CoreUtils.dataService.QueryRecord<MapFixPointDefine>((int)data.resourcePointId);
            if (mapFixPointDefine != null)
            {
                view.m_img_icon_PolygonImage.gameObject.SetActive(!data.villageState);
            }
        }


        public void SetGuildBuildHud(GameObject obj, LodPopDataContent data)
        {

            UI_Pop_GuildBuildNameView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_GuildBuildNameView>(obj);

            if (view!=null)
            {
                var type = m_allianceProxy.GetBuildServerTypeToConfigType(data.objectType);
                var cconfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);

                if (cconfig!=null)
                {
                    view.m_UI_Model_GuildFlag.setData(data.guildFlagSigns);
                    view.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, data.guildAbbName, LanguageUtils.getText(cconfig.l_nameId)); 
                }
            }

        }

        public void SetGuildBuildHudExt(GameObject obj, LodPopDataContent data)
        {
//            if (data.rssType >= RssType.GuildCenter && data.rssType <= RssType.GuildFortress2)
//            {
                UI_Pop_GuildFortressTopView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_GuildFortressTopView>(obj);

                if (view!=null)
                {
                    view.m_UI_Flag.setData(data.guildFlagSigns);

                    var type = m_allianceProxy.GetBuildServerTypeToConfigType(data.objectType);
                        
                    var cconfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);

                    if (cconfig!=null)
                    {
                        view.m_UI_Pop_TextTip.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(300030, data.guildAbbName, string.Empty); 
                    }
                    
                    view.gameObject.SetActive(Check_Toggle_Alliance(data));
                }
//            }
        }

        public bool Check_SetGuildBuildStateHud(LodPopDataContent data)
        {
            if (( m_playerProxy.CurrentRoleInfo.guildId !=0 && data.guildId == m_playerProxy.CurrentRoleInfo.guildId)
                &&(data.guildBuildStatus == (long) GuildBuildState.building
                ||data.guildBuildStatus == (long) GuildBuildState.fire
                )
                )
                return true;
            if (TroopHelp.GetTroopState(data.status) == Troops.ENMU_SQUARE_STAT.FIGHT)
                return true;
            
            int armyIndex = m_TroopProxy.GetArmyIndex((int)data.objectId);;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndex);
            if (armyData != null && (armyIndex > 0 && TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.COLLECTING)))
            {
                return true;
            }

            return false;

        }
        public void SetGuildBuildStateHud(GameObject obj, LodPopDataContent data)
        {
            UI_Tip_ComBuildStateView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_ComBuildStateView>(obj);

            if (view!=null)
            {
                var isShow = Check_SetGuildBuildStateHud(data);
                view.m_img_bg_PolygonImage.gameObject.SetActive(isShow);
                view.gameObject.SetActive(isShow);
                if (isShow)
                {
                    var iconStr = string.Empty;
                    var state = data.guildBuildStatus;
                    if ( TroopHelp.GetTroopState(data.status) == Troops.ENMU_SQUARE_STAT.FIGHT)
//                    if (data.guildBuildIsBattle)
                    {
                        iconStr = RS.GuildBuildState_battle;
                    }
                    else
                    {
                        switch ((GuildBuildState)state)
                        {
                            case GuildBuildState.building:
                                iconStr = RS.GuildBuildState_building;
                                break;
//                            case GuildBuildState.battle:
//                                iconStr = RS.GuildBuildState_battle;
//                                break;
                            case GuildBuildState.fire:
                                iconStr = RS.GuildBuildState_fire;
                                break;
                        }
                    }
                    
                    int armyIndex = m_TroopProxy.GetArmyIndex((int)data.objectId);;
                    ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndex);
                    if (armyData != null && (armyIndex > 0 && TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.COLLECTING)))
                    {
                        iconStr = RS.StateCollect1;
                    }

                    if (string.IsNullOrEmpty(iconStr))
                    {
                        view.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.gameObject.SetActive(true);
                        ClientUtils.LoadSprite(view.m_lbl_icon_PolygonImage, iconStr);
                    }
                }
            }
        }

        /// <summary>
        /// 村庄
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="data"></param>
        public void SetVillagePointHud(GameObject obj, LodPopDataContent data)
        {
            if(data.mapEntity!=null)
                m_rssProxy.UpdateResourcePoint(data.mapEntity);
            UI_Tip_WorldVillageBoxView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldVillageBoxView>(obj);
            var mapFixPointDefine = CoreUtils.dataService.QueryRecord<MapFixPointDefine>((int)data.resourcePointId);
            if (mapFixPointDefine != null)
            {
                view.m_img_icon_PolygonImage.gameObject.SetActive(!data.villageState);
                view.m_pl_bg_PolygonImage.gameObject.SetActive(!data.villageState);
            }
        }

        public bool Check_SetArmyExtHud(LodPopDataContent data)
        {
            return data.armyRid == m_playerProxy.CurrentRoleInfo.rid;
        }
        public void SetArmyExtHud(GameObject obj, LodPopDataContent data)
        {
            
//            ArmyData armyData = null;
//            armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)data.objectId);
            if (data.armyData == null)
            {
                data.armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)data.objectId);
            }
//            Debug.Log("++++data.status SetArmyExtHud" + data.status);
            UI_Tip_WorldObjectLodArmyView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_WorldObjectLodArmyView>(obj);

            if (view != null)
            {
                var armyInfo = m_TroopProxy.GetArmyByIndex(data.armyIndex);
                if(armyInfo != null)
                {
                    var heroCfg = CoreUtils.dataService.QueryRecord<HeroDefine>((int)armyInfo.mainHeroId);
                    
                    if(heroCfg != null)
                    {
                        view.m_UI_Model_CaptainHead.SetIcon(heroCfg.heroIcon);
                        view.m_UI_Model_CaptainHead.SetRare(heroCfg.rare);
//                        if (data.armyData != null)
//                        {
//                            view.m_UI_Model_CaptainHead.SetFightArmyRare(data.armyData);
//                        }
//                        else if (data.mapEntity != null)
//                        {
//                            view.m_UI_Model_CaptainHead.SetFightArmyRare(data.mapEntity);
//                        }
                    }
                    view.m_img_choose_PolygonImage.gameObject.SetActive(false);
                    view.m_img_state_PolygonImage.gameObject.SetActive(true);
                    ClientUtils.LoadSprite(view.m_img_state_PolygonImage, TroopHelp.GetIcon(data.status));
//                    if (data.status == (long) ArmyStatus.COLLECTING)
//                    {
//                        view.m_img_state_PolygonImage.gameObject.SetActive(true);
////                        GuildMemberInfoEntity  guildMemberInfoEntity= m_allianceProxy.getMemberInfo(data.collectRid);
//                        string iconPath = RS.StateCollect3;
//                        if (m_playerProxy.CurrentRoleInfo.rid == data.armyRid)
//                        {
//                            iconPath = RS.StateCollect1;
//                        }
//                        else if (m_playerProxy.CurrentRoleInfo.guildId != 0 && data.guildId == m_playerProxy.CurrentRoleInfo.guildId)
//                        {
//                            iconPath = RS.StateCollect2;
//                        }
//                        ClientUtils.LoadSprite(view.m_img_state_PolygonImage , iconPath);
//                    }
                }
            }
        }

        public void SetHolylandHud(GameObject obj, LodPopDataContent data)
        {
            UI_Pop_GuildBuildNameView view = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_GuildBuildNameView>(obj);

            if (view!=null)
            {
                try
                {
                    StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)data.strongHoldId);
                    StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataDefine.type);
                    string holylandName = LanguageUtils.getText(strongHoldTypeDefine.l_nameId);
                    

                    if (data.guildId == 0)
                    {
                        view.m_lbl_name_LanguageText.text = holylandName;
                        //view.m_UI_Model_GuildFlag.setData(m_allianceProxy.GetDefaultGuildFlagSign());
                        view.m_UI_Model_GuildFlag.setDefaultFlag(strongHoldTypeDefine.defaultFlag);
                    }
                    else
                    {
                        view.m_UI_Model_GuildFlag.setData(data.mapEntity.guildFlagSigns);
                        view.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, data.guildAbbName, holylandName);    
                    }
                }
                catch (Exception e)
                {
                    CoreUtils.logService.Error($"[SetHolylandHud]  errorInfo:[{e.ToString()}] ");                    
                }
            }
        }

        #endregion

        
        #region MAP OBJ HUD 增删改

        private void RefreshMapObject(LodPopDataContent objData)
        {
            //符文无需显示缩略图
            if ((RssType)objData.objectType == RssType.Rune)
            {
                return;
            }

            if (m_viewLevel <= MapViewLevel.Tactical)
            {
                return;
            }

//            if(objData.objectId == -1)
//                return;
            var objType = objData.objectType - 1;
            
            var index = m_AllView.FindIndex(view => view.dataContent.objectId == objData.objectId);
            
            LodPopViewData ele;
            if (-1 != index)
            {
                ele = m_AllView[index];
            }
            else
            {
                //Debug.Log("++++新增" + objType + " id" + objData.objectId);
                ele = AddElement(objType , objData);
            }

            if (ele != null)
            {
                RefreshObjElementView(ele);
            }

            // DelNoArmyDataElement();
        }
        private string getMapLod3PrefabName(LodPopViewConfig config, LodPopDataContent objData)
        {
            string prefabName = string.Empty;

            var objType = (int)objData.objectType - 1;
            switch ((RssType)objData.objectType)
            {
                case RssType.HolyLand:

                    try
                    {
                        StrongHoldDataDefine strongHoldDataDefine =
                            CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int) objData.strongHoldId);
                        StrongHoldTypeDefine strongHoldTypeDefine =
                            CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataDefine.type);
//                        prefabName = mapObjectObj[objData.objectType - 1][strongHoldTypeDefine.@group - 1];
                        if (dicObjPopViewInfo.ContainsKey(objType))
                        {
                            prefabName = config.prefabName + "_" + strongHoldTypeDefine.@group;
                        }
                        
                    }
                    catch (Exception e)
                    {
                        CoreUtils.logService.Warn(e.ToString());
                        CoreUtils.logService.Warn($"[getMapLod3Prefab]  获取圣地模型失败");
                    }

                    CoreUtils.logService.Warn($"getMapLod3Prefab  HolyLand   prefabName:[{prefabName}]");

                    break;
                default:
                    if (dicObjPopViewInfo.ContainsKey(objType))
                    {
                        prefabName = config.prefabName;
                    }
                    break;
            }

            return prefabName;
        }

        public void RemoveByMapEntity(long id)
        {
//            if (m_AllView.Exists(view => view.dataContent.objectId == id))
//            {
//                Debug.Log("++++ id"+id);
//                DeleteElement(id);
//            }
            for (int i = 0; i < m_AllView.Count; i++)
            {
                if (m_AllView[i].dataContent.objectId == id)
                {
                    if (m_AllView[i].dataContent.armyRid != m_playerProxy.CurrentRoleInfo.rid)
                    {
                        DeleteElement(id);
                        break;
                    }
                }
            }
        }
        public void RemoveMapObjLodAndHud(long id)
        {
//            if (id == m_worldMapProxy.MyCityId || id == -m_worldMapProxy.MyCityId)
//            {
//                return;
//            }
            if(m_hudObjectLoading.Contains(id))
            {
                m_hudObjectLoading.Remove(id);
            }         
            if(m_lodMapObjectLoading.Contains(id))
            {
                m_lodMapObjectLoading.Remove(id);
            }           

            DeleteElement(id);
        }

        // private void HideHudOutView(MapObjectInfoEntity objData)
        // {
        //     for (int i = 0; i < m_AllView.Count; i++)
        //     {
        //         if (m_AllView[i].dataContent.objectId == objData.objectId)
        //         {
        //             for (int j = 0; j < m_AllView[i].lstViewContent.Count; j++)
        //             {
        //                 if (m_AllView[i].lstViewContent[j].hud != null)
        //                     m_AllView[i].lstViewContent[j].hud.SetUIFadeShow((int)MapElementUI.FadeType.DirectOut);
        //             }
        //         }
        //     }
        // }

        private void RefreshMapHudUI(LodPopDataContent objData)//TODO 考虑合并 hud lod
        {
//            if(objData.objectId == -1)
//                return;
            var objType = objData.objectType - 1;
            if (objType < 0 || !dicHudPopViewInfo.ContainsKey(objType))
            {
//                Debug.Log(objType+"没有类型"+objData.objectId);
//                ClientUtils.Print(objData);
                return;
            }
            if(FilterGuideHudObject(objData.objectId))
            {
                Debug.Log("新手引导过滤掉HUD");
                return;
            }

            LodPopViewData ele = null;
            var index = m_AllView.FindIndex(view => view.dataContent.objectId == objData.objectId);
            if (-1 != index)
            {
                ele = m_AllView[index];
            }
            else
            {
                ele = AddElement(objType , objData);
            }

            if (ele != null)
            {
                RefreshHUDElementView(ele);
            }

            // DelNoArmyDataElement();
        }

        private void RefreshObjElementView(LodPopViewData data)
        {
            var SetLodLevel = new Action<GameObject , LodPopViewConfig>(( obj , config) =>
            {
                int cur = (int) m_viewLevel;
                int min = (int) config.minLodLevel;
                int max = (int) config.maxLodLevel;
                var visible = cur >= min && cur <= max;
                obj.gameObject.SetActive(visible);
                if (visible)
                {
                    config.refreshView?.Invoke(obj.gameObject, data.dataContent);
                }
            });
            var RefreshSubView = new Action<GameObject , LodPopViewContent>(( obj , parentData) =>
            {
                if (parentData.config.subConfigs != null && parentData.config.subConfigs.Length > 0 && parentData.lstSubElement.Count <= 0)
                {
                    AddSubElement(parentData , data.dataContent);
                }
                RefreshDependSubElementView(parentData);
            });
            for (int i = 0; i < data.lstViewContent.Count; i++)
            {
                if (data.lstViewContent[i].config.popType != PopViewType.Obj)
                    continue;
                var index = i;
                var view = data.lstViewContent[index];
                if (view.loadType == PopLoadType.Done)
                {
                    SetLodLevel(view.obj, view.config);
                    RefreshSubView(view.obj, view);
                }
                else
                {
                    if (view.loadType == PopLoadType.Loading)
                        continue;
                    
                    var config = view.config;
                    var prefabName = getMapLod3PrefabName(config , data.dataContent);
                    
                    if (config.showCondition != null && !config.showCondition(data.dataContent))
                    {
                        continue;
                    }

                    if (data.dataContent.mapEntity != null && data.dataContent.mapEntity.isGuide) //新手引导不需要创建
                    {
                        continue;
                    }

                    view.loadType = PopLoadType.Loading;
                    CoreUtils.assetService.Instantiate(prefabName, (lod3GameObj) =>
                    {
                        if (view.loadType == PopLoadType.Done || view.loadType == PopLoadType.None)
                        {
                            Debug.Log("RefreshObjElementView加载完成但是状态不正确！"+ prefabName + view.loadType.ToString());
                            CoreUtils.assetService.Destroy(lod3GameObj);
                        }
                        view.loadType = PopLoadType.Done;
                        lod3GameObj.transform.SetParent(GetLodRoot());
                        lod3GameObj.transform.position = data.dataContent.GetPosValue();
                        lod3GameObj.gameObject.name = string.Format("{0}_{1}", prefabName, data.dataContent.objectId);
//                        Debug.Log("++++加载完成"+ lod3GameObj.gameObject.name);
                        view.obj = lod3GameObj;
                        SetLodLevel(view.obj, view.config);
                        RefreshSubView(view.obj, view);
                        
                    });
                }
            }
        }
        private void RefreshHUDElementView(LodPopViewData data)
        {
            var SetLodLevel = new Action<MapElementUI , LodPopViewConfig>(( ui , config ) =>
            {
                if(ui==null||config==null)
                    return;
                if (m_isInCity)
                {
                    ui.SetUIFadeShow((int) MapElementUI.FadeType.DirectOut);
                }
                else
                {
                    var min = config.minLodLevel;
                    var max = config.maxLodLevel;
                    if (m_viewLevel >= min && m_viewLevel <= max)
                    {
                        ui.SetUIFadeShow((int) MapElementUI.FadeType.AllFadeIn);
                    }
                    else
                    {
                        if (data.dataContent.rssType == RssType.Cave || data.dataContent.rssType == RssType.Village)
                        {
                            ui.SetUIFadeShow((int)MapElementUI.FadeType.DirectOut);
                        }
                        else
                        {
                            ui.SetUIFadeShow((int)MapElementUI.FadeType.AllFadeOut);
                        }

                    }
                    config.refreshView?.Invoke(ui.gameObject, data.dataContent);
                }
            });
            

            for (int i = 0; i < data.lstViewContent.Count; i++)
            {
                if (data.lstViewContent[i].config.popType != PopViewType.HUD)
                    continue;
                var index = i;
                var view = data.lstViewContent[index];
                if (view.loadType == PopLoadType.Done)
                {
                    if (m_isInCity)
                    {
                        view.hud.SetUIFadeShow((int) MapElementUI.FadeType.DirectOut);
                    }
                    else
                    {
                        SetLodLevel(view.hud, view.config);
                    }
                }
                else
                {
                    if (view.loadType == PopLoadType.Loading)
                        continue;
                    
                    var config = view.config;
                    var prefabName = config.prefabName;
                    
                    if (config.showCondition != null && !config.showCondition(data.dataContent))
                    {
                        continue;
                    }
                    if (data.dataContent.mapEntity != null && data.dataContent.mapEntity.isGuide) //新手引导不需要创建
                    {
                        continue;
                    }
                    
                    var min = config.minLodLevel;
                    var max = config.maxLodLevel;
                    if (!(m_viewLevel >= min && m_viewLevel <= max))
                    {
                        continue;
                    }

                    view.loadType = PopLoadType.Loading;
                    CoreUtils.assetService.Instantiate(prefabName, (hudUIGameObj) =>
                    {
                        if (view.loadType == PopLoadType.Done || view.loadType == PopLoadType.None)
                        {
                            Debug.Log("RefreshHUDElementView加载完成但是状态不正确！" + view.loadType.ToString());
                            CoreUtils.assetService.Destroy(hudUIGameObj);
                        }
                        view.loadType = PopLoadType.Done;
                        hudUIGameObj.transform.SetParent(GetHudRoot());
                        hudUIGameObj.transform.position = Vector3.up * 99999; //防止出现闪现在镜头前，后续被曲线控制，如果未配置曲线需要检查相关设置
                        hudUIGameObj.gameObject.name = string.Format("{0}_{1}", prefabName, data.dataContent.objectId);
                        
                        MapElementUI ui = hudUIGameObj.GetComponent<MapElementUI>();
                        if (ui)
                        {
                            view.hud = ui;
                            ui.InitData();
                            ui.SetUIType((int) config.uiType);
                            SetLodLevel(ui, config);
                        }

                    });
                }
            }
        }

        private void AddSubElement(LodPopViewContent parentView , LodPopDataContent parentData)
        {
            if (parentView.lstSubElement.Count > 0)
            {
                Debug.Log("已经拥有子内容");
                return;
            }
            parentView.lstSubElement.Clear();
            for (int i = 0; i < parentView.config.subConfigs.Length; i++)
            {
                var data = new LodPopViewData();
                parentView.lstSubElement.Add(data);
                data.dataContent = parentData;
                data.viewConfigs.AddRange(parentView.config.subConfigs);
                for (int j = 0; j < data.viewConfigs.Count; j++)
                {
                    LodPopViewContent item = new LodPopViewContent();
                    item.config = data.viewConfigs[j];
                    item.loadType = PopLoadType.None;
                    data.lstViewContent.Add(item);
                }
            }
        }

        private void DeleteSubElement()
        {
            
        }
        private void RefreshDependSubElementView(LodPopViewContent parentView)
        {
            for (int i = 0; i < parentView.lstSubElement.Count; i++)
            {
                var ele = parentView.lstSubElement[i];
                for (int j = 0; j < ele.lstViewContent.Count; j++)
                {
                    var view = ele.lstViewContent[j];
                    if (view.loadType == PopLoadType.Done)
                    {
                        view.config.refreshSubView(view.obj, ele.dataContent, parentView.obj.transform);
                    }
                    else
                    {
                        if (view.loadType == PopLoadType.Loading)
                            continue;
                    
                        var config = view.config;
                        var prefabName = config.prefabName;
                    
                        if (config.showCondition != null && !config.showCondition(ele.dataContent))
                        {
                            continue;
                        }
                    
                        view.loadType = PopLoadType.Loading;
                        CoreUtils.assetService.Instantiate(prefabName, (dependObj) =>
                        {
                            if (view.loadType == PopLoadType.Done || view.loadType == PopLoadType.None)
                            {
                                Debug.Log("RefreshDependSubElementView加载完成但是状态不正确！" + view.loadType.ToString());
                                CoreUtils.assetService.Destroy(dependObj);
                            }

                            view.loadType = PopLoadType.Done;
                            dependObj.gameObject.name = string.Format("{0}_{1}", prefabName, ele.dataContent.objectId);
                            view.obj = dependObj;
                            view.config.refreshSubView(view.obj, ele.dataContent, parentView.obj.transform);
                        });
                    }
                }
            }
        }
        private LodPopViewData AddElement(long objType ,LodPopDataContent objData)
        {
            var ele = new LodPopViewData();
            if(dicHudPopViewInfo.ContainsKey(objType))
                ele.viewConfigs.AddRange(dicHudPopViewInfo[objType]);//TODO 可以把info  合成一个了
            if(dicObjPopViewInfo.ContainsKey(objType))
                ele.viewConfigs.AddRange(dicObjPopViewInfo[objType]);
            ele.dataContent = objData;
            //data.lstViewContent 填充
            for (int i = 0; i < ele.viewConfigs.Count; i++)
            {
                LodPopViewContent item = new LodPopViewContent();
                item.config = ele.viewConfigs[i];
                item.loadType = PopLoadType.None;
                ele.lstViewContent.Add(item);
            }

            CheckOldElementAndDelete(ele);
            // if (!CheckOldElementAndDelete(ele))
            // {
            //     return null;
            // }

            var insertIndex = 0;
            for (int i = 0; i < m_AllView.Count; i++)
            {
                if (m_AllView[i].dataContent.GetPosValue().z <= ele.dataContent.GetPosValue().z)
                {
                    insertIndex = i;
                    break;
                }
            }

            if (m_AllView.Count > insertIndex) 
                m_AllView.Insert(insertIndex,ele);
            else
                m_AllView.Add(ele);
//            Debug.Log("++++AddElement" + objData.objectId);
            return ele;
        }

        private void DeleteIdChangePlayerTroop()
        {
            if (m_Lst_ID_Changed.Count > 0)
            {
                for (int i = 0; i < m_Lst_ID_Changed.Count; i++)
                {
                    DeleteElement(m_Lst_ID_Changed[i]);
                }
            }
        }
        private void CheckOldElementAndDelete(LodPopViewData ele)
        {
            //检查旧的
            if (ele.dataContent.armyRid == m_playerProxy.CurrentRoleInfo.rid
                && 
                (ele.dataContent.objectType == (int) RssType.Troop
                 ||ele.dataContent.objectType == (int) RssType.Scouts
                 ||ele.dataContent.objectType == (int) RssType.Transport))
            {
                if (ele.dataContent.objectType == (int) RssType.Troop)
                {
                    var index = m_AllView.FindIndex(view =>
                        view.dataContent.armyIndex == ele.dataContent.armyIndex &&
                        view.dataContent.objectType == (int) RssType.Troop&&
                        ele.dataContent.objectId != view.dataContent.objectId
                        );
                    if (index > -1)
                    {
                        // isAdd = false;
                        //DeleteElement(m_AllView[index].dataContent.objectId);
                        m_Lst_ID_Changed.Add(m_AllView[index].dataContent.objectId);
                    }
                }
                if (ele.dataContent.objectType == (int) RssType.Scouts)
                {
                    var index = m_AllView.FindIndex(view =>
                        view.dataContent.scoutsIndex == ele.dataContent.scoutsIndex &&
                        ele.dataContent.armyIndex == view.dataContent.armyIndex &&
                        view.dataContent.objectType == (int) RssType.Scouts&&
                        ele.dataContent.objectId != view.dataContent.objectId 
                        );
                    if (index > -1)
                    {
                        // isAdd = false;
                        //DeleteElement(m_AllView[index].dataContent.objectId);
                        m_Lst_ID_Changed.Add(m_AllView[index].dataContent.objectId);
                    }
                }
                if (ele.dataContent.objectType == (int) RssType.Transport)
                {
                    var index = m_AllView.FindIndex(view =>
                        view.dataContent.armyIndex == ele.dataContent.armyIndex &&
                        view.dataContent.objectType == (int) RssType.Transport&&
                        ele.dataContent.objectId != view.dataContent.objectId);
                    if (index > -1)
                    {
                        // isAdd = false;
                        //DeleteElement(m_AllView[index].dataContent.objectId);
                        m_Lst_ID_Changed.Add(m_AllView[index].dataContent.objectId);
                    }
                }
            }

            // return isAdd;
        }

        // 斥候/运输车/军队重复添加删除
        private void DelScoutsErrorElement()
        {
            var lstRemoveIndex = new List<int>();

            for (int i = 0; i < m_AllView.Count; i++)
            {
                if (m_AllView[i].dataContent.armyRid == m_playerProxy.CurrentRoleInfo.rid &&  
                    (m_AllView[i].dataContent.objectType == (int)RssType.Troop
                 || m_AllView[i].dataContent.objectType == (int)RssType.Scouts
                 || m_AllView[i].dataContent.objectType == (int)RssType.Transport))
                {
                    bool isError = false;
                    for (int j = 0; j < m_AllView[i].lstViewContent.Count; j++)
                    {
                        if (m_AllView[i].viewConfigs[j].popType == PopViewType.HUD)
                        {
                            string hudName = string.Format("{0}_{1}", m_AllView[i].viewConfigs[j].prefabName, m_AllView[i].dataContent.objectId);
                            
                            if (m_AllView[i].lstViewContent[j].hud != null)
                            {
                                if (m_AllView[i].lstViewContent[j].hud.gameObject.name != hudName)
                                {
                                    isError = true;
                                }
                            }
                        }

                        if (m_AllView[i].viewConfigs[j].popType == PopViewType.Obj)
                        {
                            string lodName = string.Format("{0}_{1}", m_AllView[i].viewConfigs[j].prefabName, m_AllView[i].dataContent.objectId);

                            if (m_AllView[i].lstViewContent[j].obj != null)
                            {
                                if (m_AllView[i].lstViewContent[j].obj.gameObject.name != lodName)
                                {
                                    isError = true;
                                }
                            }
                        }
                    }

                    if (isError)
                    {
                        lstRemoveIndex.Add(i);
                    }
                }
            }

            for (int i = lstRemoveIndex.Count - 1; i >= 0; i--)
            {
                if (m_AllView[lstRemoveIndex[i]].lstViewContent.Count > 0)
                {
                    for (int j = 0; j < m_AllView[lstRemoveIndex[i]].lstViewContent.Count; j++)
                    {
                        m_AllView[lstRemoveIndex[i]].lstViewContent[j].loadType = PopLoadType.None;
                        if (m_AllView[lstRemoveIndex[i]].lstViewContent[j].obj != null)
                        {
                            CoreUtils.assetService.Destroy(m_AllView[lstRemoveIndex[i]].lstViewContent[j].obj);
                            m_AllView[lstRemoveIndex[i]].lstViewContent[j].obj = null;
                        }

                        if (m_AllView[lstRemoveIndex[i]].lstViewContent[j].hud != null)
                        {
                            CoreUtils.assetService.Destroy(m_AllView[lstRemoveIndex[i]].lstViewContent[j].hud.gameObject);
                            m_AllView[lstRemoveIndex[i]].lstViewContent[j].hud = null;
                        }
                    }
                    m_AllView[lstRemoveIndex[i]].lstViewContent.Clear();
                }
                m_AllView.RemoveAt(lstRemoveIndex[i]);
            }
        }

        private void DelNoArmyDataElement()
        {
            var lstRemoveIndex = new List<long>();

            for (int i = 0; i < m_AllView.Count; i++)
            {
                if (m_AllView[i].dataContent.armyRid == m_playerProxy.CurrentRoleInfo.rid
                    && (m_AllView[i].dataContent.objectType == (int)RssType.Troop
                     || m_AllView[i].dataContent.objectType == (int)RssType.Scouts
                     || m_AllView[i].dataContent.objectType == (int)RssType.Transport)
                    )
                {
                    if (m_AllView[i].dataContent.armyData == null)
                    {
                        lstRemoveIndex.Add(m_AllView[i].dataContent.objectId);
                    }
                }
            }

            for (int i = lstRemoveIndex.Count - 1; i >= 0; i--)
            {
                DeleteElement(lstRemoveIndex[i]);
            }

            DelScoutsErrorElement();
        }

        private void DeleteElement(long mapObjId)
        {
            var lstRemoveIndex = new List<int>();
            for (int i = 0; i < m_AllView.Count; i++)
            {
                if (m_AllView[i].dataContent.objectId == mapObjId)
                {
                    lstRemoveIndex.Add(i);
                }
            }

            for (int i = lstRemoveIndex.Count - 1; i >= 0 ; i--)
            {
                if (m_AllView[lstRemoveIndex[i]].lstViewContent.Count > 0)
                {
                    for (int j = 0; j < m_AllView[lstRemoveIndex[i]].lstViewContent.Count; j++)
                    {
                        m_AllView[lstRemoveIndex[i]].lstViewContent[j].loadType = PopLoadType.None;
                        if (m_AllView[lstRemoveIndex[i]].lstViewContent[j].obj != null)
                        {
                            CoreUtils.assetService.Destroy(m_AllView[lstRemoveIndex[i]].lstViewContent[j].obj);
                            m_AllView[lstRemoveIndex[i]].lstViewContent[j].obj = null;
                        }

                        if (m_AllView[lstRemoveIndex[i]].lstViewContent[j].hud != null)
                        {
                            CoreUtils.assetService.Destroy(m_AllView[lstRemoveIndex[i]].lstViewContent[j].hud.gameObject);
                            m_AllView[lstRemoveIndex[i]].lstViewContent[j].hud = null;
                        }
                    }
                    m_AllView[lstRemoveIndex[i]].lstViewContent.Clear();
                }
                m_AllView.RemoveAt(lstRemoveIndex[i]);
            }
        }

        private void ClearAllView()
        {
            var mapDatas = m_worldMapProxy.GetWorldMapObjects();
            for (int i = 0; i < mapDatas.Count; i++)
            {
                RemoveMapObjLodAndHud(mapDatas[i].objectId);
            }
            if (m_mapProvinceNamesObj != null)
            {
                for (int i = 0; i < m_mapProvinceNamesObj.Count; i++)
                {
                    CoreUtils.assetService.Destroy(m_mapProvinceNamesObj[i]);
                }
                m_mapProvinceNamesObj = null;
            }

            List<long> lstRemoveMine = new List<long>();
            
            
            for (int i = 0; i < m_AllView.Count; i++)
            {
                if (m_AllView[i].dataContent.armyRid == m_playerProxy.CurrentRoleInfo.rid
                    && 
                    (m_AllView[i].dataContent.objectType == (int) RssType.Troop
                     ||m_AllView[i].dataContent.objectType == (int) RssType.Scouts
                     ||m_AllView[i].dataContent.objectType == (int) RssType.Transport)
                )
                {
                    lstRemoveMine.Add(m_AllView[i].dataContent.objectId);
                }

                if (m_AllView[i].dataContent.rssType == RssType.City)
                {
                    lstRemoveMine.Add(m_AllView[i].dataContent.objectId);
                }
            }

            for (int i = 0; i < lstRemoveMine.Count; i++)
            {
                RemoveMapObjLodAndHud(lstRemoveMine[i]);
            }
            m_AllView.Clear();
        }
 #endregion       
 
        //新手引导过滤掉的HUD
        private bool FilterGuideHudObject(long mapObjectID)
        {
            if(GuideManager.Instance.IsGuideFightBarbarian || GuideManager.Instance.IsGuideFightSecondBarbarian)
            {
                if(mapObjectID == GlobalFilmMediator.GuideFilm3.FirstTroopID || mapObjectID == GlobalFilmMediator.GuideFilm3.SecondTroopID || mapObjectID == GlobalFilmMediator.GuideFilm3.FirstBarbarianID || mapObjectID == GlobalFilmMediator.GuideFilm3.SecondBarbarianID)
                    return true;
            }
            return false;
        }
        private void UpdateMapProvinceName()
        {
            if(m_viewLevel == MapViewLevel.Nationwide && m_mapProvinceNamesObj == null)
            {
                m_mapProvinceNamesObj = new List<GameObject>();

                var list = m_mapProvinceNamesObj;
                var provinces = MapManager.Instance().GetMapProvinceNameStruct();
                provinces.ForEach(province =>
                {
                    var pos = new Vector3(province.m_pos.x, 0, province.m_pos.z);
                    var strText = LanguageUtils.getText(int.Parse(province.m_province_name));
                    if (LanguageUtils.IsArabic())
                    {
                        strText = ArabicSupport.ArabicFixer.Fix(strText);
                    }
                    strText = $"<b>{strText}</b>";

                    CoreUtils.assetService.Instantiate("ProvinceNameTextBar_lod4", (gameObject)=>
                    {
                        if (list != m_mapProvinceNamesObj || m_viewLevel != MapViewLevel.Nationwide)
                        {
                            CoreUtils.assetService.Destroy(gameObject);
                            return;
                        }
                        var textMeshs = gameObject.GetComponentsInChildren<TextMesh>();
                        for(int i = 0; i < textMeshs.Length; i++)
                        {
                            textMeshs[i].text = strText;
                        }
                        gameObject.transform.position = pos;
                        gameObject.transform.SetParent(GetLodRoot());
                        m_mapProvinceNamesObj.Add(gameObject);
                    });
                });
            }
            else if((m_viewLevel < MapViewLevel.Nationwide) && m_mapProvinceNamesObj != null)
            {
                if (m_mapProvinceNamesObj != null)
                {
                    for (int i = 0; i < m_mapProvinceNamesObj.Count; i++)
                    {
                        CoreUtils.assetService.Destroy(m_mapProvinceNamesObj[i]);
                    }
                    m_mapProvinceNamesObj = null;
                }
            }
        }

        private Color GetCityColor(LodPopDataContent data)
        {
            Color color = RS.white;
            if (m_playerProxy.CurrentRoleInfo != null)
            {
                if (data.cityRid == m_playerProxy.CurrentRoleInfo.rid)    // 自己，绿色
                {
                    color = RS.green;
                }
                else if (data.guildId == m_playerProxy.CurrentRoleInfo.guildId && m_playerProxy.CurrentRoleInfo.guildId != 0)    // 同联盟处理
                {
                    var guildInfo = m_allianceProxy.GetAlliance();

                    if (m_allianceProxy.HasJionAlliance() &&
                        guildInfo != null &&
                        guildInfo.leaderRid == data.cityRid &&
                        m_viewLevel > MapViewLevel.Tactical)
                    {
                        color = RS.purple;
                    }
                    else
                    {
                        color = RS.blue;
                    }
                }
            }
            return color;
        }

        // 获取关卡不同颜色
        private Color GetCheckPointColor(LodPopDataContent data)
        {
            Color color = RS.white;    // 未被占领，默认白色
            if (m_allianceProxy.HasJionAlliance() && m_allianceProxy.GetAlliance().guildId == data.guildId)
            {
                color = RS.blue;
            }
            else if (data.guildId != 0)
            {
                color = RS.red;
            }

            return color;
        }
        // 获取圣地不同颜色
        private Color GetHolyLandColor(LodPopDataContent data)
        {
            Color color = RS.yellow;

            var guildInfo = m_allianceProxy.GetAlliance();

            if (m_allianceProxy.HasJionAlliance() &&
                guildInfo != null &&
                guildInfo.guildId == data.guildId)
            {
                color = RS.blue;
            }
            else if (data.guildId != 0)
            {
                color = RS.red;
            }

            return color;
        }

        private Transform GetLodRoot()
        {
            if (this.m_root == null)
            {
                this.m_root = GameObject.Find(m_root_path).transform;
            }

            return this.m_root;
        }
        
        private Transform GetHudRoot()
        {
            if (this.m_hudRoot == null)
            {
                this.m_hudRoot = GameObject.Find(m_hudPath).transform;
            }

            return this.m_hudRoot;
        }


        #region 大地图lod obj和hud 定义


        

        private Dictionary<long , LodPopViewConfig[]> dicObjPopViewInfo = new Dictionary<long, LodPopViewConfig[]>();
        private Dictionary<long , LodPopViewConfig[]> dicHudPopViewInfo = new Dictionary<long, LodPopViewConfig[]>();

        private void InitPopViewInfo()
        {
            /*
             * *****************************************
             * 依赖的视图配置 相关配置
             * *****************************************
             */
            var CityDepend = new LodPopViewConfig
            {
                popType = PopViewType.Depend,
                prefabName = "operation_2009",
                showCondition = Check_SetCityObj_Effect,
                refreshSubView = SetCityObj_Effect,
            };
            /*
             * *****************************************
             * LOD OBJ 相关配置
             * *****************************************
             */
            var ArmyObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "troops_lod3",
                showCondition = Check_SetTroopObj,
                refreshView = SetArmyObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var BarbarianObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "barbarian_lod3",
                showCondition = null,
                refreshView = SetBarbarianObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var CityObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "city_lod3",
                showCondition = Check_SetCityObj,
                refreshView = SetCityObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
                subConfigs = new []{CityDepend},
            };
            var StoneObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "stone_lod3",
                showCondition = null,
                refreshView = null,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var FoodObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "food_lod3",
                showCondition = null,
                refreshView = null,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var WoodObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "wood_lod3",
                showCondition = null,
                refreshView = null,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var CoinObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "coin_lod3",
                showCondition = null,
                refreshView = null,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var GemObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "gem_lod3",
                showCondition = null,
                refreshView = null,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var ScoutObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "troops_lod3",
                showCondition = Check_SetTroopObj,
                refreshView = SetScoutObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var VillageObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "village_lod3",
                showCondition = null,
                refreshView = SetVillageObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var CaveObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "cave_lod3",
                showCondition = null,
                refreshView = SetCaveObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var GuildBuildObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_building_lod3",
                showCondition = Check_Toggle_Alliance,
                refreshView = SetGuildBuildObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var GuildFlagObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_flag_lod3",
                showCondition = null,
                refreshView = SetGuildFlagObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Continental,
            };
                
            var AllianceFoodObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_food_lod3",
                showCondition = Check_Toggle_Resoureces,
                refreshView = SetGuildRssObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var AllianceWoodObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_wood_lod3",
                showCondition = Check_Toggle_Resoureces,
                refreshView = SetGuildRssObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var AllianceStoneObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_stone_lod3",
                showCondition = Check_Toggle_Resoureces,
                refreshView = SetGuildRssObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var AllianceCoinObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_coin_lod3",
                showCondition = Check_Toggle_Resoureces,
                refreshView = SetGuildRssObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var AllianceSuperFoodObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_super_mine_food_lod3",
                showCondition = Check_Toggle_Alliance,
                refreshView = SetGuildSuperRssObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var AllianceSuperWoodObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_super_mine_wood_lod3",
                showCondition = Check_Toggle_Alliance,
                refreshView = SetGuildSuperRssObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var AllianceSuperStoneObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_super_mine_stone_lod3",
                showCondition = Check_Toggle_Alliance,
                refreshView = SetGuildSuperRssObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var AllianceSuperCoinObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_super_mine_coin_lod3",
                showCondition = Check_Toggle_Alliance,
                refreshView = SetGuildSuperRssObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.TacticsToStrategy_2,
            };
            var RuneObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "alliance_super_mine_stone_lod3",
                showCondition = null,
                refreshView = null,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
                
            var CheckPointObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "fortress_lod3",
                showCondition = null,
                refreshView = SetCheckPointObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var HolyLandObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "holy_land_lod3",
                showCondition = null,
                refreshView = SetHolyLandObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var TransportObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "troops_lod3",
                showCondition = Check_SetTroopObj,
                refreshView = SetTransportObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var BarbarianFortObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "barbarian_fort_lod3",
                showCondition = null,
                refreshView = SetBarbarianObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var BarbarianGuardianObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "barbarian_guardian_lod3",
                showCondition = null,
                refreshView = SetBarbarianObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var SummonAttackBarbarianObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "barbarian_narmer_lod3",
                showCondition = null,
                refreshView = SetBarbarianObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            var SummonConcentrateBarbarianObj = new LodPopViewConfig
            {
                popType = PopViewType.Obj,
                prefabName = "barbarian_narmer_lod3",
                showCondition = null,
                refreshView = SetBarbarianObj,
                uiType = MapElementUI.ElementUIType.Null,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Nationwide,
            };
            dicObjPopViewInfo[0] = new[] {ArmyObj};
            dicObjPopViewInfo[1] = new[] {BarbarianObj};
            dicObjPopViewInfo[2] = new[] {CityObj };
            dicObjPopViewInfo[3] = new[] {StoneObj };
            dicObjPopViewInfo[4] = new[] {FoodObj };
            dicObjPopViewInfo[5] = new[] {WoodObj };
            dicObjPopViewInfo[6] = new[] {CoinObj };
            dicObjPopViewInfo[7] = new[] {GemObj };
            dicObjPopViewInfo[8] = new[] {ScoutObj};
            dicObjPopViewInfo[9] = new[] {VillageObj};
            dicObjPopViewInfo[10] = new[] {CaveObj};
            dicObjPopViewInfo[11] = new[] {GuildBuildObj};
            dicObjPopViewInfo[12] = new[] {GuildBuildObj};
            dicObjPopViewInfo[13] = new[] {GuildBuildObj};
            dicObjPopViewInfo[14] = new[] {GuildFlagObj};
            dicObjPopViewInfo[15] = new[] {AllianceFoodObj};
            dicObjPopViewInfo[16] = new[] {AllianceWoodObj};
            dicObjPopViewInfo[17] = new[] {AllianceStoneObj};
            dicObjPopViewInfo[18] = new[] {AllianceCoinObj};
            dicObjPopViewInfo[19] = new[] {AllianceSuperFoodObj};
            dicObjPopViewInfo[20] = new[] {AllianceSuperWoodObj};
            dicObjPopViewInfo[21] = new[] {AllianceSuperStoneObj};
            dicObjPopViewInfo[22] = new[] {AllianceSuperCoinObj};
            dicObjPopViewInfo[23] = new[] {RuneObj};
            dicObjPopViewInfo[24] = new[] {CheckPointObj};
            dicObjPopViewInfo[25] = new[] {HolyLandObj};
            dicObjPopViewInfo[26] = new[] {TransportObj};
            dicObjPopViewInfo[27] = new[] {BarbarianFortObj};
            dicObjPopViewInfo[28] = new[] {BarbarianGuardianObj};
            
            dicObjPopViewInfo[34] = new[] {CheckPointObj};
            dicObjPopViewInfo[35] = new[] {CheckPointObj};
            dicObjPopViewInfo[36] = new[] {CheckPointObj};

            dicObjPopViewInfo[41] = new[] { SummonAttackBarbarianObj };
            dicObjPopViewInfo[42] = new[] { SummonConcentrateBarbarianObj };

            /*
             * *****************************************
             * LOD HUD 相关配置
             * *****************************************
             */

            //            var ArmyHud_1 = new LodPopViewData
            //            {
            //                prefabName = "UI_Tip_WorldObjectArmyLevel",
            //                //showCondition = null,
            //                refreshView = SetArmyHud,
            //                uiType = MapElementUI.ElementUIType.Troop,
            //                minLodLevel = MapViewLevel.City,
            //                maxLodLevel = MapViewLevel.Tactical,
            //            };
            var ArmyHud_2 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldObjectLodArmy",
                showCondition = Check_SetArmyExtHud,
                refreshView = SetArmyExtHud,
                uiType = MapElementUI.ElementUIType.Troop,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Continental,
            };
            var BarbarianHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldObjectRssLevel",
                showCondition = null,
                refreshView = SetBarbarianHud,
                uiType = MapElementUI.ElementUIType.NPCTitle,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.TacticsToStrategy_1,
            };
            var CityHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldObjectPlayerLevel",
                showCondition = null,
                refreshView = SetCityHud,
                uiType = MapElementUI.ElementUIType.CastleTitle,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.TacticsToStrategy_1,
            };
            var CityHud_2 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Pop_WorldObjectLod3Player",
                showCondition = Check_SetCityHudExt,
                refreshView = SetCityHudExt,
                uiType = MapElementUI.ElementUIType.Troop,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Continental,
            };
            var RssHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldObjectRssLevel",
                showCondition = null,
                refreshView = SetRssHud,
                uiType = MapElementUI.ElementUIType.RssTitle,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.TacticsToStrategy_1,
            };
            var RssHud_2 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_ComBuildState",
                showCondition = Check_SetRssStateHud,
                refreshView = SetRssStateHud,
                uiType = MapElementUI.ElementUIType.RssStatus,
                minLodLevel = MapViewLevel.Tactical,
                maxLodLevel = MapViewLevel.TacticsToStrategy_1,
            };
            var ScoutHud_2 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldObjectLodArmy",
                showCondition = Check_SetScoutHudExt,
                refreshView = SetScoutHudExt,
                uiType = MapElementUI.ElementUIType.Troop,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Continental,
            };
            var VillagePointHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldVillageBox",
                showCondition = null,
                refreshView = SetVillagePointHud,
                uiType = MapElementUI.ElementUIType.RssStatus,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.Tactical,
            };
            var CavePointHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldCaveMark",
                showCondition = null,
                refreshView = SetCavePointHud,
                uiType = MapElementUI.ElementUIType.RssStatus,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.Tactical,
            };
            var GuildBuildHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Pop_GuildBuildName",
                showCondition = null,
                refreshView = SetGuildBuildHud,
                uiType = MapElementUI.ElementUIType.CastleTitle,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.Tactical,
            };
            var GuildBuildHud_2 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Pop_GuildFortressTop",
                showCondition = Check_Toggle_Alliance,
                refreshView = SetGuildBuildHudExt,
                uiType = MapElementUI.ElementUIType.RssStatus,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Continental,
            };
            var GuildBuildStateHud = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_ComBuildState",
                showCondition = Check_SetGuildBuildStateHud,
                refreshView = SetGuildBuildStateHud,
                uiType = MapElementUI.ElementUIType.RssStatus,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Continental,
            };
            var HolylandHud = new LodPopViewConfig
            {
                prefabName = "UI_Pop_GuildBuildName",
                showCondition = null,
                refreshView = SetHolylandHud,
                uiType = MapElementUI.ElementUIType.CastleTitle2,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.Tactical,
            };
            var CheckPointHud = new LodPopViewConfig
            {
                prefabName = "UI_Pop_GuildBuildName",
                showCondition = null,
                refreshView = SetHolylandHud,
                uiType = MapElementUI.ElementUIType.CastleTitle4,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.Tactical,
            };
            var TransportHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldObjectLodArmy",
                showCondition = null,
                refreshView = SetTransportHud,
                uiType = MapElementUI.ElementUIType.Troop,
                minLodLevel = MapViewLevel.TacticsToStrategy_1,
                maxLodLevel = MapViewLevel.Continental,
            };
            var BarbarCityHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_WorldObjectRssLevel",
                showCondition = null,
                refreshView = SetBarbarCityHud,
                uiType = MapElementUI.ElementUIType.NPCTitle,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.TacticsToStrategy_1,
            };
            var GuardianHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_GuardianLevel",
                showCondition = null,
                refreshView = SetBarbarGuardianHud,
                uiType = MapElementUI.ElementUIType.NPCTitle,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.TacticsToStrategy_1,
            };
            var SummonAttackBarbarianHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_GuardianLevel",
                showCondition = null,
                refreshView = SetSummonAttackBarbarianHud,
                uiType = MapElementUI.ElementUIType.NPCTitle,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.TacticsToStrategy_1,
            };
            var SummonConcentrateBarbarianHud_1 = new LodPopViewConfig
            {
                popType = PopViewType.HUD,
                prefabName = "UI_Tip_GuardianLevel",
                showCondition = null,
                refreshView = SetSummonConcentrateBarbarianHud,
                uiType = MapElementUI.ElementUIType.NPCTitle,
                minLodLevel = MapViewLevel.City,
                maxLodLevel = MapViewLevel.TacticsToStrategy_1,
            };


            //            dicHudPopViewInfo[0] = new[] {ArmyHud_1,ArmyHud_2};
            dicHudPopViewInfo[0] = new[] {ArmyHud_2};
            dicHudPopViewInfo[1] = new[] {BarbarianHud_1};
            dicHudPopViewInfo[2] = new[] {CityHud_1,CityHud_2};
            dicHudPopViewInfo[3] = new[] {RssHud_1,RssHud_2};
            dicHudPopViewInfo[4] = new[] {RssHud_1,RssHud_2};
            dicHudPopViewInfo[5] = new[] {RssHud_1,RssHud_2};
            dicHudPopViewInfo[6] = new[] {RssHud_1,RssHud_2};
            dicHudPopViewInfo[7] = new[] {RssHud_1,RssHud_2};
            dicHudPopViewInfo[8] = new[] {ScoutHud_2};
            dicHudPopViewInfo[9] = new[] {VillagePointHud_1};
            dicHudPopViewInfo[10] = new[] {CavePointHud_1};
            dicHudPopViewInfo[11] = new[] {GuildBuildHud_1,GuildBuildHud_2,GuildBuildStateHud};
            dicHudPopViewInfo[12] = new[] {GuildBuildHud_1,GuildBuildHud_2,GuildBuildStateHud};
            dicHudPopViewInfo[13] = new[] {GuildBuildHud_1,GuildBuildHud_2,GuildBuildStateHud};
            dicHudPopViewInfo[14] = new[] {GuildBuildStateHud};
            
            dicHudPopViewInfo[19] = new[] {GuildBuildHud_1,GuildBuildStateHud};
            dicHudPopViewInfo[20] = new[] {GuildBuildHud_1,GuildBuildStateHud};
            dicHudPopViewInfo[21] = new[] {GuildBuildHud_1,GuildBuildStateHud};
            dicHudPopViewInfo[22] = new[] {GuildBuildHud_1,GuildBuildStateHud};
            dicHudPopViewInfo[24] = new[] {CheckPointHud};
            dicHudPopViewInfo[25] = new[] {HolylandHud};
            
            dicHudPopViewInfo[26] = new[] {TransportHud_1};
            dicHudPopViewInfo[27] = new[] {BarbarCityHud_1};
            dicHudPopViewInfo[28] = new[] {GuardianHud_1};
            dicHudPopViewInfo[41] = new[] {SummonAttackBarbarianHud_1};
            dicHudPopViewInfo[42] = new[] {SummonConcentrateBarbarianHud_1};
        }


        #endregion
    }

}