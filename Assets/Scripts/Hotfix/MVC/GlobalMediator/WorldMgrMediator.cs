// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    CityGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using PureMVC.Interfaces;
using Client;
using Data;
using Game.WorldObjs;
using Hotfix;
using SprotoType;
using UnityEngine;
using Skyunion;
using System.Linq;
using Client.Utils;
using UnityEngine.Assertions;

namespace Game
{
    public enum WorldEditState
    {
        Normal,
        GuildBuildCreate,
    }

    public class WorldMgrMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "WorldMgrMediator";

        private Dictionary<long, Vector2> m_IViewObjects= new Dictionary<long, Vector2>();
        private Dictionary<long, Vector2> m_OViewObjects = new Dictionary<long, Vector2>();
        private float m_viewDist = 10;
        private bool m_bDirty = false;

        private Transform m_root;
        private const string m_root_path = "SceneObject/rss_root";
        private const string m_troops_root_path = "SceneObject/Troops_root";
        private Transform m_troops_root;

        
        
        private GlobalViewLevelMediator m_mapViewLevel;
        private FogSystemMediator m_fogMediator;
        private WorldMapObjectProxy m_worldMapProxy;
        private PlayerProxy m_PlayerProxy;
        private RssProxy m_rssProxy;
        private MonsterProxy m_monsterProxy;
        private AllianceProxy m_allianceProxy;

        private TroopProxy m_troopProxy;
        private int lastGridX = -1;
        private int lastGridY = -1;
        private List<Vector2Int> m_lastVisibleGrid = new List<Vector2Int>();
        private Plane m_worldMapPlane = new Plane(Vector3.up, Vector3.zero);
        private Vector3[] m_worldMapVisibleCorners = new Vector3[4];

        private WorldEditState m_mode = WorldEditState.Normal;
        //地图圆圈障碍物
        private Dictionary<int,GameObject> m_obscaleMapObj = new Dictionary<int, GameObject>();

        private Vector3 m_guildPreCreatePos;

        private float collideRadius = 0;
        
        private Dictionary<long, IBaseWorldEffectMediator> m_mapObjectEffectMediator = new Dictionary<long, IBaseWorldEffectMediator>();
        

        #endregion

        public WorldMgrMediator() : base(NameMediator, null)
        {
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.MapObjectRemove,
                CmdConstant.MapObjectChange,
                CmdConstant.NetWorkReconnecting,
                CmdConstant.VillageCavesChange,
                Map_ScoutVillage.TagName,
                CmdConstant.AllianceTerritoryUpdate,
                CmdConstant.AllianceTerritoryStrategicUpdate,
                CmdConstant.OnTouche3D,
                CmdConstant.OnTouche3DBegin,
                CmdConstant.OnTouche3DEnd,
                CmdConstant.OnTouche3DReleaseOutside,
                CmdConstant.ChangeRolePos,
                CmdConstant.EnterCity,
                CmdConstant.EnterCityShow,
                CmdConstant.GuildIdChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case  CmdConstant.EnterCity:
                case CmdConstant.EnterCityShow:
                    CleanTerritoryArea();
                    
                    //SetVisableBuildArea(false);
                    
                        break;
                case CmdConstant.GuildIdChange:
                    {
                        var mapDatas = m_worldMapProxy.GetWorldMapObjects();
                        for (int i = 0; i < mapDatas.Count; i++)
                        {
                            UpdateMapObject(mapDatas[i]);
                        }
                    }
                 
                    break;
                case CmdConstant.ChangeRolePos:

                    if (m_lastVisibleGrid.Count == 0)
                    {
                        var pos = m_PlayerProxy.CurrentRoleInfo.pos;
                        lastGridX = WorldMapObjectProxy.GetGridX(pos.x/100);
                        lastGridY = WorldMapObjectProxy.GetGridY(pos.y/100);
                        for(int i = 0; i < 2; ++i)
                        {
                            m_lastVisibleGrid.Add(new Vector2Int(lastGridX, lastGridY));
                        }
                    }

                    break;
                case CmdConstant.MapObjectChange:
                    {
                        if (!(notification.Body is MapObjectInfoEntity mapItemInfo))
                        {
                            break;
                        }

                        var pos = new Vector2(mapItemInfo.objectPos.x / 100, mapItemInfo.objectPos.y / 100);
                        switch ((RssType)mapItemInfo.objectType)
                        {
                            case RssType.City://城市
                            case RssType.Stone://石料
                            case RssType.Farmland: //农田
                            case RssType.Wood:  //木材
                            case RssType.Gold: //金矿
                            case RssType.Gem:  //宝石
                            case RssType.Village: //村庄
                            case RssType.Cave: //山洞
                            case RssType.GuildCenter:  //12 联盟中心要塞
                            case RssType.GuildFortress1:  //联盟要塞1
                            case RssType.GuildFortress2: //联盟要塞2
                            case RssType.GuildFlag:  //15 联盟旗帜
                            case RssType.GuildFood: //16 联盟农田
                            case RssType.GuildWood:  //联盟伐木场
                            case RssType.GuildStone:  //联盟石矿床
                            case RssType.GuildGold:  //联盟金矿床
                            case RssType.GuildFoodResCenter: //20 联盟谷仓  资源中心
                            case RssType.GuildWoodResCenter:  //联盟木料场
                            case RssType.GuildGoldResCenter:  //联盟石材厂
                            case RssType.GuildGemResCenter:  //23联盟铸币场
                            case RssType.Rune:  //符文
                            case RssType.CheckPoint:  //关卡
                            case RssType.HolyLand:  // 圣地
                            case RssType.BarbarianCitadel:  //野蛮人城寨
                            case RssType.Sanctuary: //圣所
                            case RssType.Altar: //圣坛
                            case RssType.Shrine: //圣祠
                            case RssType.LostTemple: //圣庙
                            case RssType.Checkpoint_1: //关卡1
                            case RssType.Checkpoint_2: //关卡2
                            case RssType.Checkpoint_3: //关卡3
                                MapManager.Instance().AddCity(mapItemInfo.objectId.ToString(), mapItemInfo.objectPos.x / 100, mapItemInfo.objectPos.y / 100, 8);
                                break;
                            default:
                                break;
                        }
                        UpdateMapObject(mapItemInfo);
                    }
                    break;
                case CmdConstant.MapObjectRemove:
                    {
                        if (!(notification.Body is MapObjectInfoEntity mapItemInfo))
                        {
                            break; 
                        }
                        switch ((RssType)mapItemInfo.objectType)
                        {
                            case RssType.City://城市
                            case RssType.Stone://石料
                            case RssType.Farmland: //农田
                            case RssType. Wood:  //木材
                            case RssType .Gold: //金矿
                            case RssType. Gem:  //宝石
                            case RssType. Village: //村庄
                            case RssType .Cave: //山洞
                            case RssType. GuildCenter:  //12 联盟中心要塞
                            case RssType. GuildFortress1:  //联盟要塞1
                            case RssType. GuildFortress2: //联盟要塞2
                            case RssType. GuildFlag:  //15 联盟旗帜
                            case RssType. GuildFood: //16 联盟农田
                            case RssType. GuildWood:  //联盟伐木场
                            case RssType. GuildStone:  //联盟石矿床
                            case RssType. GuildGold:  //联盟金矿床
                            case RssType. GuildFoodResCenter: //20 联盟谷仓  资源中心
                            case RssType. GuildWoodResCenter:  //联盟木料场
                            case RssType. GuildGoldResCenter:  //联盟石材厂
                            case RssType. GuildGemResCenter:  //23联盟铸币场
                            case RssType. Rune:  //符文
                            case RssType. CheckPoint:  //关卡
                            case RssType. HolyLand:  // 圣地
                            case RssType. BarbarianCitadel:  //野蛮人城寨
                            case RssType. Sanctuary: //圣所
                            case RssType. Altar: //圣坛
                            case RssType. Shrine: //圣祠
                            case RssType. LostTemple: //圣庙
                            case RssType. Checkpoint_1: //关卡1
                            case RssType. Checkpoint_2: //关卡2
                            case RssType. Checkpoint_3: //关卡3
                                MapManager.Instance().RemoveCity(mapItemInfo.objectId.ToString());
                                break;
                            default:
                                break;
                        }

                        if (mapItemInfo.rssType >= RssType.GuildFood && mapItemInfo.rssType <= RssType.GuildGold &&
                            mapItemInfo.guildId > 0 && HUDManager.Instance().HasHud(mapItemInfo.objectId))
                        {
                            HUDManager.Instance().RemoveHud(mapItemInfo.objectId);
                        }

                        RemoveBlockBuild(mapItemInfo);
                        
                        RemoveMapObject(mapItemInfo);
                        break;
                    }
                case Map_ScoutVillage.TagName:
                    {
                        Map_ScoutVillage.response info = notification.Body as Map_ScoutVillage.response;
                        if (info != null)
                        {
                            m_rssProxy.UpdateViallAgeData((int)info.targetIndex, (int)info.villageRewardId);
                        }
                        
                    }
                    break;
                case CmdConstant.VillageCavesChange:
                    {
                        m_PlayerProxy.CurrentRoleInfo.villageCaves.Values.ToList().ForEach((villageCave) =>
                            {
                                for (int i = 1; i <= 64; i++)
                                {
                                    long resourcePointId = (villageCave.index - 1) * 64 + i;
                                    MapObjectInfoEntity mapItemInfo = m_worldMapProxy.GetWorldMapObjectByresourcePoint(resourcePointId);
                                    if (mapItemInfo != null)
                                    {
                                        long state = villageCave.rule & (1L << (i%64));
                                        if (!mapItemInfo.villageState && state!=0)
                                        {
                                            mapItemInfo.villageState = true;
                                            AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectChange, mapItemInfo);
                                        }
                                    }

                                }
                            });
                    }
                    break;
                
                case  CmdConstant.NetWorkReconnecting:
                    
                    ManorMgr.ClearAllLine_S(true,true);

                    
                    if (GuideProxy.IsGuideing)//引导阶段不允许清空
                    {
                        //var guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
                        //if (!guideProxy.IsCompletedByStage((int)EnumNewbieGuide.UpgradeCity))
                        //{
                        //    return;
                        //}
                        return;
                    }
                    m_worldMapProxy.ClearAllMapObject();
                    ClearBlocks();
                    CleanTerritoryArea();
                    foreach (var kv in m_mapObjectEffectMediator)
                    {
                        if (kv.Value != null)
                        {
                            kv.Value.DisposeEffect();
                        }
                    }
                    m_mapObjectEffectMediator.Clear();
                    SetWorldMapState(WorldEditState.Normal);
                    
                    
                    break;
                case  CmdConstant.AllianceTerritoryUpdate:
                    
                    OnWorldTerritoryChanged();
                    break;
                
                case  CmdConstant.AllianceTerritoryStrategicUpdate:
                    
                    OnWorldTerritoryStrategicChanged();
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
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            IsOpenUpdate = true;
            
            m_mapViewLevel =
                AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                    GlobalViewLevelMediator;
            m_worldMapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_fogMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            
            m_rssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_monsterProxy= AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            
            m_allianceProxy= AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
        }

        protected override void BindUIEvent()
        {
            WorldCamera.Instance().AddViewChange(OnWorldViewChange);
            
            CoreUtils.inputManager.AddTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
        }

        protected override void BindUIData()
        {
            
        }

        public override void Update()
        {
            if(m_bDirty)
            {
                var center = WorldCamera.Instance().GetViewCenter();
                OnWorldViewChange(center.x, center.y, WorldCamera.Instance().getCurrentCameraDxf());
            }
        }

        public override void LateUpdate()
        {
            // 改成一帧加载10个
            int nCount = 0;
            while (lstUpdateMapObject.Count > 0)
            {
                Do_UpdateMapObject();
                nCount++;
                if (nCount == 10)
                    break;
            }
            if (lstRemoveMapObject.Count > 0)
            {
                Do_RemoveMapObject();
            }
        }

        public override void FixedUpdate()
        {
        }

        public override void OnRemove()
        {
            m_lastVisibleGrid.Clear();
            MapManager.Instance().ClearCiity();
            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);
            
            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);
            CoreUtils.inputManager.RemoveTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);

            
            ClearBlocks();
            CleanTerritoryArea();
        }

       



        #endregion
        private Vector2 m_centerPos;
        private float m_lastDfx = -999.0f;
        public void OnWorldViewChange(float x, float y, float dxf)
        {
            if (m_lastVisibleGrid.Count == 0) return;
            if (WorldCamera.Instance().IsAutoMoving() || WorldCamera.Instance().IsMovingToPos() || WorldCamera.Instance().IsSlipping())
                return;
            MapViewLevel crrLevel = m_mapViewLevel.GetViewLevel();

            if (crrLevel > MapViewLevel.Tactical)
            {
                CleanTerritoryArea();
            }
            

            if(GameModeManager.Instance.CurGameMode == GameModeType.World && !CoreUtils.uiManager.ExistUI(UI.s_expeditionFight))
            {
                if (crrLevel >= MapViewLevel.Strategic)
                {
                    CoreUtils.audioService.PlayBgm(RS.SoundWind);
                }
                else
                {
                    GameEventGlobalMediator tMediator = AppFacade.GetInstance().RetrieveMediator(GameEventGlobalMediator.NameMediator) as GameEventGlobalMediator;
                    if (tMediator != null)
                    {
                        CoreUtils.audioService.PlayBgm(!tMediator.IsDay() ? RS.SoundCityNight : RS.SoundCityDay);
                    }
                }
            }

            SetTerritoryLineLevel(crrLevel >= MapViewLevel.Tactical);

                //SetVisableBuildArea(crrLevel<= MapViewLevel.Tactical);


            bool isVisibleGridChanged = false;
            UpdateVisibleGrid(ref isVisibleGridChanged);

            //            if (m_mode == WorldEditState.Normal && (crrLevel > MapViewLevel.Tactical || !isVisibleGridChanged))
            //            {
            //                return;
            //            }
            if (m_mode == WorldEditState.Normal && !isVisibleGridChanged)
            {
                return;
            }

            if (crrLevel <= MapViewLevel.Tactical)
            {
                OnWorldTerritoryChanged();
            }
            

            lastGridX = WorldMapObjectProxy.GetGridX(x);
            lastGridY = WorldMapObjectProxy.GetGridY(y);
            var mapDatas = m_worldMapProxy.GetWorldMapObjects();

            m_isCollideMap = false;

            for (int i = 0; i < mapDatas.Count; i++)
            {
                UpdateMapObject(mapDatas[i]);

                if (m_mode == WorldEditState.GuildBuildCreate)
                {
                    UpdateBlockBuild(mapDatas[i]);
                }
            }

            if (m_mode == WorldEditState.GuildBuildCreate)
            {
                List<int> arms = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDatas();

                foreach (var arm in arms)
                {
                    var formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(arm);
                    var rd = 3f;

                    var worldObj = m_worldMapProxy.GetWorldMapObjectByobjectId(arm);
                    if (worldObj!=null)
                    {
                        rd = worldObj.radius;
                    }
                    if (formation != null)
                    {
                        UpdateArmyBlock(formation.gameObject,rd);
                    }
                }

            }


            if (m_isCollideMap)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBlock, m_isCollideMap);
            }

        }

        private void UpdateVisibleGrid(ref bool isChanged)
        {
            isChanged = false;
            if(!GetMapPlaneVisibleCorners())
            {
                return;
            }
            Vector2Int lbGrid = WorldMapObjectProxy.GetGrid(m_worldMapVisibleCorners[0]);
            Vector2Int rbGrid = WorldMapObjectProxy.GetGrid(m_worldMapVisibleCorners[1]);
            Vector2Int ltGrid = WorldMapObjectProxy.GetGrid(m_worldMapVisibleCorners[2]);
            Vector2Int rtGrid = WorldMapObjectProxy.GetGrid(m_worldMapVisibleCorners[3]);

            Vector2Int min = new Vector2Int(Mathf.Min(lbGrid.x, ltGrid.x), Mathf.Min(lbGrid.y, rbGrid.y));
            Vector2Int max = new Vector2Int(Mathf.Max(rbGrid.x, rtGrid.x), Mathf.Max(ltGrid.y, rtGrid.y));
            if(m_lastVisibleGrid[0] != min)
            {
                isChanged = true;
                m_lastVisibleGrid[0] = min;
            }
            if(m_lastVisibleGrid[1] != max)
            {
                isChanged = true;
                m_lastVisibleGrid[1] = max;
            }
        }

        private bool GetMapPlaneVisibleCorners()
        {
            var worldCamera = WorldCamera.Instance().GetCamera();
            Ray rayBL = worldCamera.ViewportPointToRay(new Vector3(0, 0, 1));
            Ray rayBR = worldCamera.ViewportPointToRay(new Vector3(1, 0, 1));
            Ray rayTL = worldCamera.ViewportPointToRay(new Vector3(0, 1, 1));
            Ray rayTR = worldCamera.ViewportPointToRay(new Vector3(1, 1, 1));
            if (!Common.GetRayPlaneIntersection(ref m_worldMapPlane, rayBL, out m_worldMapVisibleCorners[0]) || 
                !Common.GetRayPlaneIntersection(ref m_worldMapPlane, rayBR, out m_worldMapVisibleCorners[1]) || 
                !Common.GetRayPlaneIntersection(ref m_worldMapPlane, rayTL, out m_worldMapVisibleCorners[2]) || 
                !Common.GetRayPlaneIntersection(ref m_worldMapPlane, rayTR, out m_worldMapVisibleCorners[3]))
            {
                return false;
            }
            else
            {
                return true;
            }
        }        

        private List<ManorData> m_territoryInfos;

        private void OnWorldTerritoryStrategicChanged()
        {
            var lines = m_worldMapProxy.getTerritoryLod3ActicesPoints();
            
            lines.ForEach((line) => { ManorMgr.CreateLineFromCache_S(line.points, 0, 1, line.color,line.dir); });
            
            var disablelines = m_worldMapProxy.getTerritoryLod3DisablePoints();
            
            disablelines.ForEach((line) => { ManorMgr.CreateLineFromCache_S(line.points, 6, 1, line.color,line.dir); });
            
        }
        
        public static bool ListEqual(List<ManorItem> array1, List<ManorItem> array2)
        {
            if (array1 == null && array2 == null)
                return true;
            if (array1 == null || array2 == null)
                return false;
            return array1.Count() == array2.Count() && !array1.Except(array2).Any();
        }


        private List<ManorItem> m_preFakeList;
        public void OnWorldTerritoryChanged()
        {
            ManorData tdActive;
            ManorData tdDisable;

            ManorData tdInactive;
            if (m_territoryInfos ==null)
            {
                m_territoryInfos = new List<ManorData>(3);
                tdActive = new ManorData();
                tdActive.type = "active";
                tdActive.width = 1;
                tdActive.uvStep = 0;
                
                m_territoryInfos.Add(tdActive);

                tdDisable = new ManorData();
                tdDisable.type = "disable";
                tdDisable.width = 0.4f;
                tdDisable.uvStep = 3;
                m_territoryInfos.Add(tdDisable);
            
                tdInactive = new ManorData();
                tdInactive.type = "inactive";
                tdInactive.width = 0.4f;
                tdInactive.uvStep = 3;
                m_territoryInfos.Add(tdInactive);

                
            }
            
            
            
            tdActive = m_territoryInfos[0];
            tdDisable = m_territoryInfos[1];
            tdInactive = m_territoryInfos[2];
           
            
            var newTdActive = m_worldMapProxy.GetTerritoryActices(); 
            var newTdDisable = m_worldMapProxy.GetTerritoryDisables();
            var newTdInactive = m_worldMapProxy.GetTerritoryInactives();
            var newfakeTerritoryList = m_worldMapProxy.GetTerritoryFakes();


            if (!ListEqual(newTdActive,tdActive.list) || !ListEqual(newTdDisable,tdDisable.list)|| !ListEqual(newTdInactive,tdInactive.list) || m_worldMapProxy.HasChangeFake())
            {
                tdActive.list = newTdActive;
                tdDisable.list = newTdDisable;
                tdInactive.list = newTdInactive;
                m_preFakeList= newfakeTerritoryList;
                if (newfakeTerritoryList.Count>0)
                {
                    ManorMgr.UpdateFakeTerritoryS(m_territoryInfos,newfakeTerritoryList);
                }
                else
                {
                    ManorMgr.UpdateTerritoryS(m_territoryInfos);
                }
            }
           
        }

        
        private List<MapObjectInfoEntity> lstUpdateMapObject = new List<MapObjectInfoEntity>();
        private void Do_UpdateMapObject()
        {
            if(lstUpdateMapObject.Count<=0)
                return;
            MapObjectInfoEntity objData = lstUpdateMapObject[0];
            lstUpdateMapObject.RemoveAt(0);
            if (objData.isLoading == false)
            {
                if ((objData.objectId > 0 && IsObjectInVisibleGrid(objData)) || objData.isGuide)
                {
                    string name = GetResName(objData);
                    if (!string.IsNullOrEmpty(name))
                    {
                        WorldMapViewObjFactory.SysLoadWorldObj(objData, name, OnLoadMapObj);
                    }
                    else
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectHUDUpdate, objData);
                        UpdateMapObjectEffect(objData);
                    }
                }

            }
            else
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectHUDUpdate, objData);
                UpdateMapObjectEffect(objData);
            }
            UpdateMapObjectColor(objData);
        }

        public void UpdateMapObject(MapObjectInfoEntity objData)
        {
            if(lstUpdateMapObject.Contains(objData))
                return;
            lstUpdateMapObject.Add(objData);
        }

        private void Do_RemoveMapObject()
        {
            if(lstRemoveMapObject.Count<=0)
                return;
            MapObjectInfoEntity objData = lstRemoveMapObject[0];
            lstRemoveMapObject.RemoveAt(0);
            if (objData.isLoading == false)
            {
                RemoveByObjectId(objData.objectId);
            }

        }

        public void RemoveByObjectId(long objectId)
        {
            if (m_mapObjectEffectMediator.ContainsKey(objectId))
            {
                if(m_mapObjectEffectMediator[objectId]!=null)
                    m_mapObjectEffectMediator[objectId].DisposeEffect();
                m_mapObjectEffectMediator.Remove(objectId);  
            }
        }
        private List<MapObjectInfoEntity> lstRemoveMapObject = new List<MapObjectInfoEntity>();
        public void RemoveMapObject(MapObjectInfoEntity objData)
        {
            if(lstRemoveMapObject.Contains(objData) || objData.isLoading)
                return;
            lstRemoveMapObject.Add(objData);
        }

        private bool IsObjectInVisibleGrid(MapObjectInfoEntity objData)
        {
            Assert.IsFalse(m_lastVisibleGrid.Count == 0, "visible grid data is not initialized");
            if (m_lastVisibleGrid.Count == 0) return false;
            return objData.gridX >= m_lastVisibleGrid[0].x && objData.gridX <= m_lastVisibleGrid[1].x &&
                objData.gridY >= m_lastVisibleGrid[0].y && objData.gridY <= m_lastVisibleGrid[1].y;
        }


        public string GetResName(MapObjectInfoEntity objData)
        {
            RssType resType = (RssType) objData.objectType;
            string name = string.Empty;
            switch (resType)
            {
                case RssType.Troop:
                case RssType.City:
                    break;
                case RssType.SummonAttackMonster:
                case RssType.SummonConcentrateMonster:
                case RssType.Guardian:
                case RssType.BarbarianCitadel:
                case RssType.Monster:
                    {
                        objData.monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)objData.monsterId);
                        if (objData.monsterDefine != null)
                        {
                           name = objData.monsterDefine.modelId;
                        }
                    }
                    break;
                case RssType.Village:
                case RssType.Cave:
                    {
                        objData.mapFixPointDefine =
                        CoreUtils.dataService.QueryRecord<MapFixPointDefine>((int)objData.resourcePointId);

                        if (objData.mapFixPointDefine != null)
                        {
                            objData.resourceGatherTypeDefine =
                                CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(objData.mapFixPointDefine.type);
                            name = objData.resourceGatherTypeDefine.modelId;
                        }

                        m_rssProxy.SetViallAgeState(objData);
                    }
                    break;
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                case RssType.GuildFood:
                case RssType.GuildWood:
                case RssType.GuildStone:
                case RssType.GuildGold:
                case RssType.GuildFoodResCenter:
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                    {
                        name = m_allianceProxy.GetBuildModle(objData.objectType);
                    }
                    break;
                case RssType.Rune:
                    {
                        Data.MapItemTypeDefine mapItemType = CoreUtils.dataService.QueryRecord<Data.MapItemTypeDefine>((int)objData.runeId);
                        if (mapItemType != null)
                        {
                            name = mapItemType.modelId;
                        }
                    }
                    break;
                case RssType.HolyLand:
                case RssType.CheckPoint:
                {
                    StrongHoldDataDefine data = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)objData.strongHoldId);
                    StrongHoldTypeDefine mapItemType = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(data.type);
                    if (mapItemType != null)
                    {
                        name = mapItemType.modelId;
                    }
                }
                    break;
                default:
                    {
                        objData.resourceGatherTypeDefine =
                            CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>((int)objData.resourceId);
                        if(objData.resourceGatherTypeDefine != null)
                        {
                            name = objData.resourceGatherTypeDefine.modelId;
                        }
                        objData.isShowHud = true;
                        if (objData.collectRid > 0)
                        {
                            if (m_PlayerProxy.CurrentRoleInfo.rid == objData.collectRid)
                            {
                                // itemData.name = m_PlayerProxy.CurrentRoleInfo.name;
                                objData.rssPointStateType = RssPointState.CollectedByMe;
                                Debug.LogWarning("开始采集这个资源点了");
                            }
                            else
                            {
                                objData.rssPointStateType = RssPointState.CollectedNoByally;
                            }
                        }
                        else
                        {
                            objData.rssPointStateType = RssPointState.Uncollected;
                        }                        
                    }
                    break;


            }
            return name;           
        }


        public void OnLoadMapObj(GameObject go,MapObjectInfoEntity data)
        {

            Transform t = null;
            Vector3 pos = Vector3.zero;
            
            string name = String.Empty;

            switch (data.rssType)
            {
                case RssType.Stone:
                case RssType.Farmland:
                case RssType.Wood:
                case RssType.Gold:
                case RssType.Gem:
                case RssType.Village:
                case RssType.Cave:
                    if (data.rssType == RssType.Village || data.rssType == RssType.Cave)
                    {
                        pos = new Vector3(data.mapFixPointDefine.posX, 0, data.mapFixPointDefine.posY);
                    }
                    else
                    {
                        pos = new Vector3(data.objectPos.x / 100f, 0, data.objectPos.y / 100f);
                    }

                    name = string.Format("RssItem_{0}", data.objectId);
                    t = GetRoot();
                    go.transform.SetParent(t);
                    go.transform.localScale = Vector3.one;
                    go.transform.position = pos;
                    break;
                case RssType.Rune:
                    {
                        pos = new Vector3(data.objectPos.x / 100f, 0, data.objectPos.y / 100f);
                        name = string.Format("RuneItem_{0}", data.objectId);
                        t = GetRoot();
                        go.transform.SetParent(t);
                        go.transform.localScale = Vector3.one;
                        go.transform.position = pos;
                        ShowRuneEffect(data, go);
                    }
                    break;
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFood:
                case RssType.GuildWood:
                case RssType.GuildStone:
                case RssType.GuildGold:
                case RssType.GuildFoodResCenter:
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                    
                    pos = new Vector3(data.objectPos.x / 100f, 0, data.objectPos.y / 100f);
                    
                    name = string.Format("GuildBuild_{0}", data.objectId);
                    t = GetRoot();
                    go.transform.SetParent(t);
                    go.transform.localScale = Vector3.one;
                    go.transform.position = pos;


                    if ( !m_fogMediator.HasFogAtWorldPos(pos.x, pos.z) && data.rssType>= RssType.GuildFood && data.rssType<= RssType.GuildGold && data.guildId>0 && HUDManager.Instance().HasHud(data.objectId)==false)
                    {
                        HUDUI shootHud = HUDUI
                            .Register(UI_Pop_GuildResTextView.VIEW_NAME, typeof(UI_Pop_GuildResTextView),
                                HUDLayer.world, go.gameObject).SetTargetGameObject(go.gameObject).SetData(data.objectId)
                            .SetCameraLodDist(250, 3000, (isOn, view) =>
                            {
                               
                                UI_Pop_GuildResTextView ui = view.gameView as UI_Pop_GuildResTextView;
                                
                                var buildid = m_allianceProxy.GetBuildServerTypeToConfigType(data.objectType);

                                var buildTypeConfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(buildid);

                                ui.m_lbl_num_LanguageText.text = (buildTypeConfig.holdAllianceSpeed/1000).ToString();

                                EnumCurrencyType currencyType =EnumCurrencyType.allianceFood;

                                switch (data.rssType)
                                {
                                    case RssType.GuildFood:
                                        currencyType = EnumCurrencyType.allianceFood;
                                        break;
                                    case RssType.GuildWood:
                                        currencyType = EnumCurrencyType.allianceWood;
                                        break;
                                    case RssType.GuildStone:
                                        currencyType = EnumCurrencyType.allianceStone;
                                        break;
                                    case RssType.GuildGold:
                                        currencyType = EnumCurrencyType.allianceGold;
                                        break;
                                }

                                var crrConfig = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)currencyType);
                                if (crrConfig!=null)
                                {
                                    ClientUtils.LoadSprite(ui.m_img_cur_PolygonImage,crrConfig.iconID);
                                }
                                
                            }).SetPosOffset(new Vector2(0, 0)).SetPositionAutoAnchor(true);
                        HUDManager.Instance().ShowHud(shootHud);
                        HUDManager.Instance().AddHud(data.objectId,shootHud);
                    }
                    
                    //onCheckLoadBuildPreBG(data);
                    break;
                case RssType.GuildFlag:
                    
                    
                    pos = new Vector3(data.objectPos.x / 100f, 0, data.objectPos.y / 100f);
                    
                    name = string.Format("GuildBuild_{0}", data.objectId);
                    t = GetRoot();
                    go.transform.SetParent(t);
                    go.transform.localScale = Vector3.one;
                    go.transform.position = pos;
                    
                    if (data.guildFlagSigns!=null)
                    {
                        AllianceProxy.setFlag(go,data.guildFlagSigns);
                    }
                   

                    //onCheckLoadBuildPreBG(data);

                    break;
                case RssType.SummonAttackMonster:
                case RssType.SummonConcentrateMonster:
                case RssType.Guardian:
                case RssType.BarbarianCitadel:
                case RssType.Monster:
                    name = InitMonsterObj(go,data);
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateShottTextHud, (int)data.objectId);
                    break;
                case RssType.HolyLand: // 圣地建筑
                    pos = new Vector3(data.objectPos.x / 100f, 0, data.objectPos.y / 100f);
                    name = string.Format("HolyLand_{0}", data.objectId);
                    t = GetRoot();
                    go.transform.SetParent(t);
                    go.transform.localScale = Vector3.one;
                    go.transform.position = pos;
                    break;
                case RssType.CheckPoint: // 关卡建筑
                {
                    StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)data.strongHoldId);
                    
                    pos = new Vector3(data.objectPos.x / 100f, 0, data.objectPos.y / 100f);
                    name = string.Format("CheckPoint_{0}", data.objectId);
                    t = GetRoot();
                    go.transform.SetParent(t);
                    go.transform.localScale = Vector3.one;
                    go.transform.position = pos;
                    go.transform.eulerAngles = Vector3.up * strongHoldDataDefine.posTo;
                }
                   
                    
                    break;
            }

            if (TroopHelp.IsAttackGuildType(data.rssType))
            {
                WorldMapLogicMgr.Instance.MapBuildingFightHandler.PlayBurning((int)data.objectId);
            }

            if (TroopHelp.IsAttackBuilding(data.rssType))
            {
                WorldMapLogicMgr.Instance.BehaviorHandler.ChageState((int)data.objectId, (int)data.status);                
            }

            if (TroopHelp.IsHaveState(data.status, ArmyStatus.BATTLEING))
            {
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateAttackerDir((int)data.objectId);
            }                

            go.gameObject.name = name;
            UpdateMapObjectEffect(data);
            UpdateMapObjectColor(data);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectHUDUpdate, data);
        }
        #region 地图物件颜色
        /// <summary>
        /// 更新地图物件特效
        /// </summary>
        /// <param name="objData"></param>
        private void UpdateMapObjectColor(MapObjectInfoEntity objData)
        {
            long objId = objData.objectId;
            if (objData.gameobject == null)
            {
                return;
            }
            switch (objData.rssType)
            {
                case RssType.CheckPoint:
                case RssType.Checkpoint_1:
                case RssType.Checkpoint_2:
                case RssType.Checkpoint_3:
                    {
                        Color color = RS.white;    // 未被占领，默认白色
                        if (m_allianceProxy.HasJionAlliance() && m_allianceProxy.GetAllianceId()== objData.guildId)
                        {
                            color = RS.blue;
                        }
                        else if (objData.guildId != 0)
                        {
                            color = RS.red;
                        }

                        TownBuilding cityBuildingHelper = objData.gameobject.GetComponent<TownBuilding>();
                        if (cityBuildingHelper != null)
                        {
                            cityBuildingHelper.SetColor(color);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 地图物件特效

        /// <summary>
        /// 更新地图物件特效
        /// </summary>
        /// <param name="objData"></param>
        private void UpdateMapObjectEffect(MapObjectInfoEntity objData)
        {
            long objId = objData.objectId;
            if (objData.gameobject == null)
            {
                if (m_mapObjectEffectMediator.ContainsKey(objId))
                {
                    if(m_mapObjectEffectMediator[objId]!=null)
                        m_mapObjectEffectMediator[objId].DisposeEffect();
                    m_mapObjectEffectMediator.Remove(objId);    
                }
                
                return;
            }
            
            if (!m_mapObjectEffectMediator.ContainsKey(objId))
            {
                m_mapObjectEffectMediator.Add(objId, CreateMapObjectEffectMediator(objData));
            }

            if (m_mapObjectEffectMediator[objId] != null)
            {
                m_mapObjectEffectMediator[objId].UpdateEffects(objData);    
            }
        }

        /// <summary>
        /// 创建不同类型物件特效类型
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        private IBaseWorldEffectMediator CreateMapObjectEffectMediator(MapObjectInfoEntity objData)
        {
            IBaseWorldEffectMediator mediator = null;
            switch (objData.rssType)
            {
                
                case RssType.HolyLand:    // 圣地建筑
                case RssType.CheckPoint:    // 关卡建筑
                    mediator = new HolylandEffectMediator();
                    break;
            }

            return mediator;
        }


        #endregion
        
        #region 地图点击事件

        public void OnTouchBegan(int x, int y)
        {
            m_touchStartPos = new Vector2(x,y);
        }

        public void OnTouchMoved(int x, int y)
        {
            
        }

        public void OnTouchEnded(int x, int y)
        {
        }


        private void OnTouche3D(int x, int y, string parentName, string colliderName)
        {
        }


        private string m_clickObjectName;
        private Vector2 m_touchStartPos = Vector2.zero;
        private void OnTouche3DBegin(int x, int y, string parentName, string colliderName)
        {
            MapViewLevel crrLevel = m_mapViewLevel.GetViewLevel();
            if (crrLevel <= MapViewLevel.Tactical)
            {
                m_clickObjectName = parentName;
            }
            else
            {
                m_clickObjectName = String.Empty;
            }
        }


        private GameObject m_myTerritoryBg;
        private HashSet<long> m_loadingbuildBg = new HashSet<long>();



        private void OnTouche3DEnd(int x, int y, string parentName, string colliderName)
        {
            bool needClean = true;
            
            if (IsWorldMapStateNormal())
            {
                if (m_clickObjectName== parentName && !string.IsNullOrEmpty(parentName))
                {
                    string[] name = m_clickObjectName.Split('_');

                    if (name[0] == "GuildBuild")
                    {
                        long ObjectID = long.Parse(name[1]);

                        var objMapData = m_worldMapProxy.GetWorldMapObjectByobjectId(ObjectID);

                        if (objMapData!=null)
                        {
                            if (objMapData.rssType >= RssType.GuildCenter && objMapData.rssType <= RssType.GuildFlag)
                            {
                                needClean = false;
                                ClickLoadTerritoryBg(objMapData);
                            }
                        }
                    }
                    else if(name[0] == "HolyLand" || name[0] == "CheckPoint")
                    {
                         long ObjectID = long.Parse(name[1]);
                        
                        var objMapData = m_worldMapProxy.GetWorldMapObjectByobjectId(ObjectID);

                        if (objMapData!=null)
                        {
                            needClean = false;
                            ClickLoadTerritoryBg(objMapData);
                        }
                    }
                }
                else
                {
                    needClean = Vector2.Distance(m_touchStartPos,new Vector2(x,y))<10;
                }
            }

            if (needClean )
            {
                CleanTerritoryArea();
            }
        }

        private void CleanTerritoryArea()
        {
            if ( m_myTerritoryBg!=null)
            {
                CoreUtils.assetService.Destroy(m_myTerritoryBg);
                m_myTerritoryBg = null;
            }
        }

        private void SetTerritoryLineLevel(bool isTop)
        {
            if (isTop)
            {
                foreach (Transform transform in ManorMgr.manorStrategicLineRoot)
                {
                    Material material = transform.GetComponent<MeshRenderer>().material;
                    material.SetInt("_ZTest",   0);
                }
            }
            else
            {
                foreach (Transform transform in ManorMgr.manorStrategicLineRoot)
                {
                    Material material = transform.GetComponent<MeshRenderer>().material;
                    material.SetInt("_ZTest",   4);
                }
            }
        }





        // 获取领地大小
        int GetMapObjectTerritorySize(MapObjectInfoEntity data)
        {
            int size = 0;

            RssType rssType = (RssType)data.objectType;
            
            if (rssType == RssType.HolyLand || rssType == RssType.CheckPoint)
            {
                StrongHoldDataDefine strongHoldDataDefine =
                    CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int) data.strongHoldId);
                StrongHoldTypeDefine strongHoldTypeDefine =
                    CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataDefine.type);
                size = strongHoldTypeDefine.territorySize;
            }
            else
            {
                var buildType = m_allianceProxy.GetBuildServerTypeToConfigType(data.objectType);
                var config = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(buildType);
                size = config.territorySize;
            }
            
            return size;
        }

        // 点击加载领地背景
        private void ClickLoadTerritoryBg(MapObjectInfoEntity objMapData)
        {
            if (!m_loadingbuildBg.Contains(objMapData.objectId) && m_myTerritoryBg==null)
            {
                m_loadingbuildBg.Add(objMapData.objectId);
                CoreUtils.assetService.Instantiate("alliancebuildarea_bg", (bgo) =>
                {
                    m_loadingbuildBg.Remove(objMapData.objectId);
                    m_myTerritoryBg = bgo;
                    SetTerritoryBg(m_myTerritoryBg,objMapData);
                               
                });
            }else if (m_myTerritoryBg!=null)
            {
                SetTerritoryBg(m_myTerritoryBg,objMapData);
            }
        }
        
        

        private void SetTerritoryBg(GameObject bg ,MapObjectInfoEntity data)
        {
            int territorySize = GetMapObjectTerritorySize(data);
            float scale = territorySize * 0.25f -territorySize/WorldMapObjectProxy.TerritoryPerUnit.x/2f-1.5f*(territorySize/WorldMapObjectProxy.TerritoryPerUnit.x/5);
    
            bg.transform.localScale = new Vector3(scale,scale,scale);

            bg.name = "Area" + data.objectId;

            var pos = new Vector3(data.objectPos.x/100,0,data.objectPos.y/100);
            var alignPos = pos;//UI_Pop_GuildBuildResMediator.GetAlignPos(pos);
            
            float unitX = WorldMapObjectProxy.TerritoryPerUnit.x;
            float unitY = WorldMapObjectProxy.TerritoryPerUnit.y;

            int gridPosX = (int)Mathf.Floor((alignPos.x-0.1f) / unitX);
            int gridPosY = (int)Mathf.Floor((alignPos.z-0.1f) / unitY);
            
            float centerPosX = gridPosX * unitX +
                               unitY * 0.5f;//
            
            float centerPosY = gridPosY * unitY +
                               unitY * 0.5f;
            
            bg.transform.position =  new Vector3(centerPosX,0,centerPosY);
            
            var sr = bg.GetComponent<SpriteRenderer>();

            if (data.guildFlagSigns == null || data.guildFlagSigns.Count == 0)    // 奇观建筑，白色
            {
                sr.color = Color.white;
            }
            else
            {
                var signsColor =
                    CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int) data.guildFlagSigns[1]);

                if (signsColor!=null)
                {
                    sr.color = ClientUtils.StringToHtmlColor(signsColor.colour);
                }
                else
                {
                    Debug.Log("无法找到颜色"+data.guildFlagSigns[1]);
                }
            }
        }

        private void OnTouche3DReleaseOutside(int x, int y, string parentName, string colliderName)
        {
            
        }

        #endregion

        #region 地图上的怪物

        private string InitMonsterObj(GameObject go, MapObjectInfoEntity data)
        {
            if(data.monsterDefine==null)
            {
                return string.Empty;
            }
            switch(data.monsterDefine.type)
            {
                case 1: return InitBarbarianObj(go, data);//野蛮人
                case 2: return InitBarbarianWalled(go,data);//城寨
                case 3: return InitGuardianObj(go,data);//圣地守护者
                case 4: return InitSummonBarbarianObj(go, data);//召唤类型怪物
                default:return InitBarbarianObj(go,data);
            }
        }

        private string InitBarbarianObj(GameObject go, MapObjectInfoEntity data)
        {
            string name = string.Format("{0}{1}", "BarbarianFormation_", data.objectId);
            data.name = data.monsterDefine.modelId;
            data.monsterTroopsDefine =
                CoreUtils.dataService.QueryRecord<MonsterTroopsDefine>(data.monsterDefine.monsterTroopsId);
            data.heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>(data.monsterTroopsDefine.heroID1);
            Vector2 start = data.pos = new Vector2(data.objectPos.x / 100f, data.objectPos.y / 100f);
            Troops m_Formation = go.GetComponent<Troops>();
            data.HP = (int)data.armyCount;
            data.HPMax = (int)data.armyCountMax;
            data.atkId = (int)data.targetObjectIndex;
            data.path.Clear();
            if (data.objectPath != null && data.objectPath.Count >= 2)
            {
                foreach (var pos in data.objectPath)
                {
                    Vector2 v2= new Vector2(pos.x/100f,pos.y/100);
                    data.path.Add(v2);
                }
            }

            int heroId = 9001;
            if (data.mainHeroId != 0)
            {
                heroId = (int)data.mainHeroId;
            }
            string des = m_troopProxy.GetMonsterSoldiersDes(heroId, (int)data.deputyHeroId, data.soldiers);
            Transform t = GetTroopsRoot();
            go.transform.SetParent(t);
            Troops.InitPositionS(m_Formation, start, start + new Vector2(go.transform.forward.x, go.transform.forward.z).normalized);
            Troops.InitFormationS(m_Formation, des, Color.gray);
            WorldMapLogicMgr.Instance.BehaviorHandler.ChageState((int)data.objectId, (int)data.status);
            return name;
        }


        private string InitBarbarianWalled(GameObject go, MapObjectInfoEntity data)
        {
            string name = string.Format("BarbarianWalled_{0}", data.objectId);
            Vector3 pos = new Vector3(data.objectPos.x / 100f, 0, data.objectPos.y / 100f);
            data.HP = (int)data.armyCount;
            data.HPMax = (int)data.armyCountMax;
            go.transform.SetParent(GetRoot());
            go.transform.localScale = Vector3.one;
            go.transform.position = pos;
            return name;
        }

        private string InitGuardianObj(GameObject go, MapObjectInfoEntity data)
        {
            string name = string.Format("{0}{1}", "GuardianFormation_", data.objectId);
            data.monsterTroopsDefine =
            CoreUtils.dataService.QueryRecord<MonsterTroopsDefine>(data.monsterDefine.monsterTroopsId);
            data.heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>(data.monsterTroopsDefine.heroID1);
            Vector2 start = data.pos = new Vector2(data.objectPos.x / 100f, data.objectPos.y / 100f);
            Guardian m_Formation = go.GetComponent<Guardian>();
            data.HP = (int)data.armyCount;
            data.HPMax = (int)data.armyCountMax;
            data.atkId = (int)data.targetObjectIndex;
            if (data.objectPath != null && data.objectPath.Count >= 2)
            {
                data.path.Clear();
                foreach (var pos in data.objectPath)
                {
                    Vector2 v2= new Vector2(pos.x/100,pos.y/100);
                    data.path.Add(v2);
                }         
            }
            
            
            int heroId = 9001;
            if (data.mainHeroId != 0)
            {
                heroId = (int)data.mainHeroId;
            }
            string des = m_troopProxy.GetMonsterSoldiersDes(heroId, (int)data.deputyHeroId, data.soldiers);
            Transform t = GetTroopsRoot();
            go.transform.SetParent(t);
            Guardian.InitFormationS(m_Formation, des, Color.gray);
            Guardian.InitPositionS(m_Formation, start, start);
            WorldMapLogicMgr.Instance.BehaviorHandler.ChageState((int)data.objectId, (int)data.status);
            return name;
        }

        private string InitSummonBarbarianObj(GameObject go, MapObjectInfoEntity data)
        {
            string name = string.Format("{0}{1}", "SummonBarbarianFormation_", data.objectId);
            data.name = data.monsterDefine.modelId;
            data.monsterTroopsDefine = CoreUtils.dataService.QueryRecord<MonsterTroopsDefine>(data.monsterDefine.monsterTroopsId);
            data.heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>(data.monsterTroopsDefine.heroID1);
            Vector2 start = data.pos = new Vector2(data.objectPos.x / 100f, data.objectPos.y / 100f);
            Troops m_Formation = go.GetComponent<Troops>();
            data.HP = (int)data.armyCount;
            data.HPMax = (int)data.armyCountMax;
            data.atkId = (int)data.targetObjectIndex;
            data.path.Clear();
            if (data.objectPath != null && data.objectPath.Count >= 2)
            {
                foreach (var pos in data.objectPath)
                {
                    Vector2 v2 = new Vector2(pos.x / 100f, pos.y / 100);
                    data.path.Add(v2);
                }
            }

            int heroId = 9001;
            if (data.mainHeroId != 0)
            {
                heroId = (int)data.mainHeroId;
            }
            string des = m_troopProxy.GetMonsterSoldiersDes(heroId, (int)data.deputyHeroId, data.soldiers);
            Transform t = GetTroopsRoot();
            go.transform.SetParent(t);
            Troops.InitPositionS(m_Formation, start, start + new Vector2(go.transform.forward.x, go.transform.forward.z).normalized);
            Troops.InitFormationS(m_Formation, des, Color.gray);
            WorldMapLogicMgr.Instance.BehaviorHandler.ChageState((int)data.objectId, (int)data.status);
            return name;
        }

        #endregion

        #region 联盟建筑

        private bool m_isCollideMap;

        public void SetGuildCreateBuildPos(Vector3 createObj,float cRadius)
        {
            m_guildPreCreatePos = createObj;

            collideRadius = cRadius;
        }
        private bool m_ignoreOwn = false;
        public void SetIgnoreOwn(bool Ignoreown)
        {
            m_ignoreOwn = Ignoreown;
        }


        public void ClearBlocks()
        {
            var list = m_obscaleMapObj.Values.ToArray();
            foreach (var objkv in list)
            {
                CoreUtils.assetService.Destroy(objkv);
            }
            WorldMapViewObjFactory.ClearLoading();
            m_obscaleMapObj.Clear();
            m_isCollideMap = false;
        }

        public void SetWorldMapState(WorldEditState state)
        {

            if (state == WorldEditState.Normal && m_mode == WorldEditState.GuildBuildCreate)
            {
                //exit 
                ClearBlocks();
                MathExtension.ClearMesh();
            }
            
            if (m_mode!= state)
            {
                m_mode = state;
                AppFacade.GetInstance().SendNotification(CmdConstant.WorldEditStateChanged);
            }
            
        }

        public bool IsWorldMapStateNormal()
        {
            return m_mode == WorldEditState.Normal;
        }

        public bool HasCollideMapObject()
        {
            return m_isCollideMap;
        }


        public void UpdateArmyBlock(GameObject objData,float rd)
        {
            GameObject blockObj;
            if (m_obscaleMapObj.TryGetValue(objData.GetHashCode(), out blockObj))
            {
                
            }
            else
            {
                if (!m_loadingbuildBg.Contains(objData.GetHashCode()))
                {
                    m_loadingbuildBg.Add(objData.GetHashCode());
                    string name = "teleportobscale_bg";
                    WorldMapViewObjFactory.SysLoadBlockArmyObj(objData,name,OnLoadBlockArmy,rd);
                    
                }
               
            }
        }
        
        public void OnLoadBlockArmy(GameObject blockGo, GameObject armyObj,float rd)
        {
            m_loadingbuildBg.Remove(armyObj.GetHashCode());
            if (m_mode != WorldEditState.Normal)
            {
                m_obscaleMapObj.Add(armyObj.GetHashCode(),blockGo);
                setBlockArmy(blockGo,armyObj,true,rd);
            }
            else
            {
                CoreUtils.assetService.Destroy(blockGo);
            }

        }
        
        public void setBlockArmy(GameObject blockGo, GameObject armyGo,bool isInit = true,float radius=3f)
        {
            if (isInit)
            {
                
                var pos = new Vector3(armyGo.transform.position.x, 0, armyGo.transform.position.z);
                string name = string.Format("blockArmy_{0}", armyGo.name);
                var t = GetRoot();
                blockGo.transform.SetParent(t);
                blockGo.transform.localScale = new Vector3(radius, radius, radius);
                blockGo.transform.position = pos;
                blockGo.gameObject.name = name;
                
                var fp = blockGo.GetComponent<Flow3DPos>();
                if (fp==null)
                {
                    fp = blockGo.AddComponent<Flow3DPos>();
                }
                else
                {
                    fp.m_target = null;
                }

                fp.invert = false;
                fp.m_target = armyGo.transform;
            }
            
           

            Color color;

            bool checkOverlap =  collideRadius + radius >= Vector3.Distance(m_guildPreCreatePos, blockGo.transform.position);
                
            
            if (!checkOverlap)
            {
                color = Color.white;
            }
            else
            {
                color = Color.red;
                m_isCollideMap = true;
            }
            
            var cc = blockGo.GetComponent<SpriteRenderer>();

            if (cc!=null)
            {
                cc.color = color;
            }
        }
        
        
        

        public void UpdateBlockBuild(MapObjectInfoEntity objData)
        {
            if (objData.rssType == RssType.Troop || objData.objectId <=0|| m_fogMediator.HasFogAtWorldPos(objData.objectPos.x / 100, objData.objectPos.y / 100))
            {
                return;
            }
            if (objData.cityRid == m_PlayerProxy.CurrentRoleInfo.rid)
            {
                if (m_ignoreOwn)
                {
                    return;
                }
            }
            bool check = false;
            string name = "";
            if (objData.objectType == (long)RssType.HolyLand || objData.objectType == (long)RssType.CheckPoint || objData.objectType == (long)RssType.Checkpoint_1 || (long)objData.objectType == (long)RssType.Checkpoint_2 || (long)objData.objectType == (long)RssType.Checkpoint_3 || (long)objData.objectType == (long)RssType.Sanctuary || (long)objData.objectType == (long)RssType.Altar || (long)objData.objectType == (long)RssType.Shrine || (long)objData.objectType == (long)RssType.LostTemple)
            {
                name = "alliancebuildarea_bg";
                if (Mathf.Max(objData.gridX, lastGridX) - Mathf.Min(objData.gridX, lastGridX) <= 4 && Mathf.Max(objData.gridY, lastGridY) - Mathf.Min(objData.gridY, lastGridY) <= 4)
                {
                    check = true;
                }
            }
            else
            {
                name = "teleportobscale_bg";
                if (Mathf.Max(objData.gridX, lastGridX) - Mathf.Min(objData.gridX, lastGridX) <= 2 && Mathf.Max(objData.gridY, lastGridY) - Mathf.Min(objData.gridY, lastGridY) <= 2)
                {
                    check = true;
                }
            }
            if (check)
            {
                GameObject blockObj;
                if (m_obscaleMapObj.TryGetValue(objData.GetHashCode(), out blockObj))
                {
                    setBlockObj(blockObj,objData,false);
                }
                else
                {
                    if (!m_loadingbuildBg.Contains(objData.GetHashCode()))
                    {
                        m_loadingbuildBg.Add(objData.GetHashCode());

                        WorldMapViewObjFactory.SysLoadBlockObj(objData,name,OnLoadBlockBuild);
                    }
                   
                }
            }
        }

        public void OnLoadBlockBuild(GameObject go, MapObjectInfoEntity objData)
        {
            m_loadingbuildBg.Remove(objData.GetHashCode());
            if (m_mode != WorldEditState.Normal)
            {
                bool check = false;
                if (objData.objectType == (long)RssType.HolyLand || objData.objectType == (long)RssType.CheckPoint || objData.objectType == (long)RssType.Checkpoint_1 || (long)objData.objectType == (long)RssType.Checkpoint_2 || (long)objData.objectType == (long)RssType.Checkpoint_3 || (long)objData.objectType == (long)RssType.Sanctuary || (long)objData.objectType == (long)RssType.Altar || (long)objData.objectType == (long)RssType.Shrine || (long)objData.objectType == (long)RssType.LostTemple)
                {
                    if (Mathf.Max(objData.gridX, lastGridX) - Mathf.Min(objData.gridX, lastGridX) <= 4 && Mathf.Max(objData.gridY, lastGridY) - Mathf.Min(objData.gridY, lastGridY) <= 4)
                    {
                        check = true;
                    }
                }
                else
                {
                    if (Mathf.Max(objData.gridX, lastGridX) - Mathf.Min(objData.gridX, lastGridX) <= 2 && Mathf.Max(objData.gridY, lastGridY) - Mathf.Min(objData.gridY, lastGridY) <= 2)
                    {
                        check = true;
                    }
                }
                if (check)
                {
                    m_obscaleMapObj.Add(objData.GetHashCode(), go);
                    setBlockObj(go, objData);
                }
                else
                {
                    CoreUtils.assetService.Destroy(go);
                }
            }
            else
            {
                CoreUtils.assetService.Destroy(go);
            }

        }
        /// <summary>
        /// 移除白圈
        /// </summary>
        /// <param name="data"></param>
        public void RemoveBlockBuild(MapObjectInfoEntity data)
        {
            GameObject blockObj;
            if (m_obscaleMapObj.TryGetValue(data.GetHashCode(), out blockObj))
            {
                m_obscaleMapObj.Remove(data.GetHashCode());
                CoreUtils.assetService.Destroy(blockObj);
            }
            m_loadingbuildBg.Remove(data.GetHashCode());
        }


        public void setBlockObj(GameObject go, MapObjectInfoEntity data,bool isInit = true)
        {
            if (isInit)
            {
                data.radius = BlockRadius(data);
                var pos = BlockPos(data); 
                string name = string.Format("block_{0}", data.objectId);
                var t = GetRoot();
                go.transform.SetParent(t);
                go.transform.localScale = new Vector3(data.radius, data.radius, data.radius);
                go.transform.position = pos;
                go.gameObject.name = name;


                var fp = go.GetComponent<Flow3DPos>();


                if (fp!=null)
                {
                    fp.m_target = null;
                }
                
               

                if ((data.rssType == RssType.Monster ||
                     data.rssType == RssType.City ||
                     data.rssType == RssType.Guardian ||
                     data.rssType == RssType.SummonAttackMonster ||
                     data.rssType == RssType.SummonConcentrateMonster) && data.gameobject != null)
                {
                    if (fp == null)
                    {
                        fp = go.AddComponent<Flow3DPos>();
                    }
                    
                    fp.invert = false;
                    fp.m_target = data.gameobject.transform;
                }
                   
            }
            
          

            Color color;

                        //Debug.Log("Type" + " " + data.objectType + " " +   collideRadius +"  "+data.radius+"  "+m_guildPreCreatePos+"  "+Vector3.Distance(m_guildPreCreatePos,go.transform.position));
            bool checkOverlap = false;
            if (data.objectType == (long)RssType.HolyLand || data.objectType == (long)RssType.CheckPoint || data.objectType == (long)RssType.Checkpoint_1 || (long)data.objectType == (long)RssType.Checkpoint_2 || (long)data.objectType == (long)RssType.Checkpoint_3)
            {
                Vector2 c = new Vector2(go.transform.position.x, go.transform.position.z);
                Vector2 h = new Vector2(45f, 45f);
                Vector2 p = new Vector2(m_guildPreCreatePos.x, m_guildPreCreatePos.z);
                var r = collideRadius;
                StrongHoldDataDefine strongHoldCardDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)data.strongHoldId);
                if (strongHoldCardDefine != null)
                {
                    StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldCardDefine.type);
                    if (strongHoldTypeDefine != null)
                    {
                        h = new Vector2(strongHoldTypeDefine.territorySize/2, strongHoldTypeDefine.territorySize / 2);
                    }
                }
                checkOverlap = BoxCircleIntersect(c,h,p,r);
            }
            else
            {
                checkOverlap =  collideRadius + data.radius >= Vector3.Distance(m_guildPreCreatePos, go.transform.position);
                if (m_ignoreOwn)
                {
                    if (data.objectType == 3 && data.cityRid == m_PlayerProxy.Rid)
                    {
                        checkOverlap = false;
                    }
                }
            }
            if (!checkOverlap)
            {
                color = Color.white;
            }
            else
            {
                color = Color.red;

                m_isCollideMap = true;
            }

      

            var cc = go.GetComponent<SpriteRenderer>();

            if (cc!=null)
            {
                cc.color = color;
            }
        }
        /// <summary>
        /// c为矩形中心，h为矩形半長，p为圆心，r为半径
        /// </summary>
        /// <param name="c"></param>
        /// <param name="h"></param>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private bool BoxCircleIntersect(Vector2 c, Vector2 h, Vector2 p, float r)
        {
            Vector2 v = new Vector2(Mathf.Abs(p.x - c.x), Mathf.Abs(p.y - c.y)) ;
            Vector2 u = new Vector2(Mathf.Max(v.x-h.x ,0), Mathf.Max(v.y-h.y ,0));
            return Vector2.Distance(Vector2.zero,u)  <= r ;
        }
        private float BlockRadius(MapObjectInfoEntity data)
        {
            if (data.rssType >= RssType.GuildCenter && data.rssType<=RssType.GuildGemResCenter)
            {
                var type = m_allianceProxy.GetBuildServerTypeToConfigType(data.objectType);
                var cd = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);

                if (cd!=null)
                {
                    return cd.radiusCollide;
                }
            }
            switch (data.rssType)
            {
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                case RssType.GuildFood:
                case RssType.GuildWood:
                case RssType.GuildStone:
                case RssType.GuildGold:
                case RssType.GuildFoodResCenter:
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                    var type = m_allianceProxy.GetBuildServerTypeToConfigType(data.objectType);
                    var cd = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);

                    if (cd!=null)
                    {
                        return cd.radiusCollide;
                    }
                    break;
                case RssType.Monster:
                case RssType.Guardian:
                case RssType.SummonAttackMonster:
                case RssType.SummonConcentrateMonster:
                    if (data.monsterDefine != null)
                        return data.monsterDefine.radius;
                    break;
                case RssType.Stone:
                case RssType.Farmland:
                case RssType.Gold:
                case RssType.Gem:
                case RssType.Village:
                case RssType.Cave:
                    return m_allianceProxy.Config.resourceGatherRadiusCollide;
                case RssType.Troop:
                    return data.radius;
                case RssType.CheckPoint:
                case RssType.HolyLand:
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                case RssType.Checkpoint_1:
                case RssType.Checkpoint_2:
                case RssType.Checkpoint_3:
                    {
                        StrongHoldDataDefine strongHoldCardDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)data.strongHoldId);
                        if (strongHoldCardDefine != null)
                        {
                            StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldCardDefine.type);
                            if (strongHoldTypeDefine != null)
                            {       
                                return strongHoldTypeDefine.territorySize * 0.25f - strongHoldTypeDefine.territorySize / WorldMapObjectProxy.TerritoryPerUnit.x / 2 - 1;
                            }
                        }
                    }
                    break;
                case RssType.City:
                    return m_PlayerProxy.ConfigDefine.cityRadiusCollide;
                    break;
            }
            
            return 4;
        }
        /// <summary>
        /// 碰撞区域坐标
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Vector3 BlockPos(MapObjectInfoEntity data)
        {
            var pos = new Vector3(data.objectPos.x / 100f, 0, data.objectPos.y / 100f);
            switch (data.rssType)
            {
                case RssType.CheckPoint:
                case RssType.HolyLand:
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                case RssType.Checkpoint_1:
                case RssType.Checkpoint_2:
                case RssType.Checkpoint_3:
                    {
                        float unitX = WorldMapObjectProxy.TerritoryPerUnit.x;
                        float unitY = WorldMapObjectProxy.TerritoryPerUnit.y;

                        int gridPosX = Mathf.CeilToInt(pos.x / unitX)-1;
                        int gridPosY = Mathf.CeilToInt(pos.z / unitY)-1;
                        float centerPosX = gridPosX * (unitX) +(unitX) * 0.5f;

                        float centerPosY = gridPosY * (unitY) + (unitY) * 0.5f;
                        return new Vector3(centerPosX, 0, centerPosY);
                    }
                    break;
            }

            return pos;
        }


        #endregion

        #region 符文
        private void ShowRuneEffect(MapObjectInfoEntity objData, GameObject runeObject)
        {
            var mapItemType = CoreUtils.dataService.QueryRecord<Data.MapItemTypeDefine>((int)objData.runeId);
            if (mapItemType == null || string.IsNullOrEmpty(mapItemType.itemEffectShow)) return;

            Transform eff = runeObject.transform.Find(mapItemType.itemEffectShow + "(Clone)");
            if (eff)
            {
                return;
            }

            CoreUtils.assetService.Instantiate(mapItemType.itemEffectShow, (obj) =>         
            {
                if(runeObject != null)
                {
                    obj.transform.SetParent(runeObject.transform);
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;
                }
                else
                {
                    CoreUtils.assetService.Destroy(obj);
                }
            });

        }

        #endregion
        private Transform GetRoot()
        {
            if (this.m_root == null)
            {
                this.m_root = GameObject.Find(m_root_path).transform;
            }

            return this.m_root;
        }
        
        public Transform GetTroopsRoot()
        {
            if (this.m_troops_root == null)
            {
                this.m_troops_root = GameObject.Find(m_troops_root_path).transform;
            }
            return this.m_troops_root;
        }

        
    }
}