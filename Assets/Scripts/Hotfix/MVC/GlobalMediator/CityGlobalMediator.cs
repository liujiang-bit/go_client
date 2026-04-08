// =============================================================================== 
// Author              :    Gen By ggx
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
using System.Text;
using Client.FSM;
using Data;
using SprotoType;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Linq;
using System.Net;

namespace Game
{
    enum CityGridState
    {
        PLACED,
        RESERVED1,
        RESERVED2,
        RESERVED5,
        FIXED,
        NORMAL,
        USED
    }

    enum CityWallState
    {
        STATE_NONE,
        STATE_WAR,
        STATE_POSTWAR,
        STATE_PATROL
    }

    public class CityGlobalMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "CityGlobalMediator";
        private CityBuildingProxy m_cityBuildingProxy;
        private DataProxy m_dataProxy;
        private PlayerProxy m_playerProxy;
        private EffectinfoProxy m_effectinfoProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private CityBuffProxy m_cityBuffProxy;
        private NetProxy m_netProxy;

        private GlobalViewLevelMediator m_globalViewLevelMediator;
        private WorldMgrMediator m_worldMgrMediator;
        private Transform m_cityBuildingContainer;

        public Transform CityBuildingContainer
        {
            get { return m_cityBuildingContainer; }
        }

        private GameObject m_land_root;
        private TownBuildingContainer m_cityBuildCp;
        private Camera m_curCamera;
        private bool LockTouch = false; //是否不可点击状态
        private bool m_hasTouchUI = false; //是否点击到了UI
        private bool m_clickDown = false; //摁下状态
        private bool m_is2Fingers = false; //是否单指点击

        //建筑物碰撞堆叠管理
        private GridCollideMgr m_tileCollederManager;

        private TownSearch m_cityMapFinder;

        private bool m_initedBuilders = false;

        private GridLines m_cityMesh;

        private bool m_hasShowBuildMenu = false;

        private Transform m_selectBuild = null;
        private TownBuilding m_selectBuildHelper = null;
        private MaskSprite m_selectBuildMakeSprite;


        private long m_selectBuildID = 0;
        private EnumCityBuildingType m_selectType;

        private float m_clickBounceTime = 200;
        private float m_clickBounceConst = 180;
        private float m_clickBounceA;
        private Timer m_clickTimer;

        private Vector2 m_startMovePos;
        private Vector2 m_startTouchPos;
        private Vector3 m_startBuildPos;
        private Vector2 m_fixedMoveStep;
        private Vector2 m_moveLogicPos;
        private bool m_showDownAnim;

        private bool isAllCityBuildInited = false;

        private int[] m_GridState;

        //是否按下建筑
        private bool m_pressed;
        private bool m_uiPressed;

        //建筑是否在移动
        private bool m_isMoving = false;
        //有建筑提起
        private bool m_liftUp = false;

        //移动中的建筑格子是否被其他建筑占用
        private bool m_isGridUsed = false;

        private bool m_isPvpTouchCity = false;

        private bool m_revBuildUpgradeBorard;

        public bool IsGridUsed
        {
            get => m_isGridUsed;
            set
            {
                if (m_isGridUsed != value)
                {
                    if (m_debug)
                    {
                        Debug.Log("搁置" + value);
                    }

                    BuildBouncing(value);

                    AppFacade.GetInstance().SendNotification(CmdConstant.CreateTempBuildEnable, value);
                    m_isGridUsed = value;
                }
            }
        }


        //移动建筑时候的箭头
        private GameObject m_arrowObj;

        //移动建筑时候的底座下面的网格
        private GameObject m_bottomBuildObj;


        //创建零时的建筑
        private BuildingInfoEntity tempBuildingInfoEntity;
        private BuildingTypeConfigDefine tempBuildType;

        //城墙相关
        private PatrolSoldier m_cityWallPatrolSoldierDummy;

        private TownCenterFloor m_buildGroundHelper;

        //
        private static int ATK_POINT_COUNT = 12;

        private static bool m_debug = false;

        //城墙上巡逻士兵个数
        private static int PARTOL_SOLDIER_COUNT = 7;
        private List<PeopleFSM> m_soldiers = new List<PeopleFSM>(PARTOL_SOLDIER_COUNT);
        private List<WorkerFMS> m_workers = new List<WorkerFMS>();

        private Dictionary<string, TrainSoldiersFMS> m_trains = new Dictionary<string, TrainSoldiersFMS>();

        private TrainProxy m_trainProxy;


        //X轴上下，Y轴左右  
        private Vector2[][] m_trainSoldierPos = new[]
        {
            new[] //兵营 右 中 左
            {
                new Vector2(1.5f, 0.5f), new Vector2(0.5f, 1.5f)//, new Vector2(0f, 0f) //, new Vector2(1, 3)
            },
            new[] //马厩
            {
                new Vector2(1f, 1f), new Vector2(0f, 1f)
            },
            new[] //靶场
            {
                new Vector2(1f, 1f)//, new Vector2(0f, 1f) //, new Vector2(3.2f, 1.6f)
            },
            new[] //攻城武器厂
            {
                new Vector2(1f, 1f)
            }
        };

        private Vector2[][] m_trainSoldierDirPos = new[]
        {
            new[]
            {
                new Vector2(-4f, 0f), new Vector2(3f, 3f), new Vector2(4f, 0f) //, new Vector2(0f, 1f)
            },
            new[]
            {
                new Vector2(-4f, 4f), new Vector2(-4f, 4f)
            },
            new[]
            {
                new Vector2(-4f, 4f), new Vector2(-4f, 4f) //, new Vector2(0f, 1f)
            },
            new[]
            {
                new Vector2(-4, 4)
            }
        };

        //城墙相关结束

        private const float HEIGHTWALL = 1000f;//城墙消失高度
        private bool m_ShowWall = false;
        private bool hudready, buildingFirstReady = true;
        private bool m_roleNotifyMoveCity = false; //有迁城通知待表现

        #endregion

        //IMediatorPlug needs
        public CityGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        public CityGlobalMediator(object viewComponent) : base(NameMediator, null)
        {
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.OnTouche3D,
                CmdConstant.OnTouche3DBegin,
                CmdConstant.OnTouche3DEnd,
                CmdConstant.OnTouche3DReleaseOutside,
                CmdConstant.CityBuildinginfoChange,
                CmdConstant.ShowBuildingMenu,
                CmdConstant.ShowBuildingMenuOnly,
                Map_ObjectInfo.TagName,
                CmdConstant.OnChapterDiaglogEnd,
                CmdConstant.CreateTempBuild,
                CmdConstant.CreateTempBuildYes,
                CmdConstant.CreateTempBuildNO,
                CmdConstant.DayNightChange,
                CmdConstant.CityBuildingDone,
                CmdConstant.CityAgeChange,
                CmdConstant.CityAgeChangeLevelUpEffect,
                CmdConstant.EnterCity,
                CmdConstant.ExitCity,
                CmdConstant.MoveCameraToBuilding,
                CmdConstant.ShowBuildingMenuAndMoveCameraToBuilding,
                CmdConstant.MapViewChange,
                CmdConstant.CityBuildingLevelUP,
                Map_GetCityDetail.TagName,
                CmdConstant.CityInViewPort,
                Build_UpGradeBuilding.TagName,
                CmdConstant.OnCloseBuildingHudMenu,
                CmdConstant.RemoveBuild,
                CmdConstant.EnterCityShow,
                CmdConstant.ExitCityHide,
                CmdConstant.UpdateCurrency,
                CmdConstant.buildQueueChange,
                CmdConstant.OtherBuildingChange,
                CmdConstant.CityBuildingStart,
                CmdConstant.RemoveOtherCity,
                CmdConstant.ArmyTrainStart,
                CmdConstant.ArmyTrainEnd,
                CmdConstant.MapObjectChange,
                CmdConstant.CityFireStateChange,
                CmdConstant.armyQueueChange,
                CmdConstant.technologyQueueChange,
                CmdConstant.treatmentChange,
                CmdConstant.MapTouchOtherCity,
                CmdConstant.ChangeRolePos,
                CmdConstant.CityBeginBurnTimeChange,
                CmdConstant.CityPosTimeChange,
                CmdConstant.CityBuffChange,
                CmdConstant.CityCitybuffChange,
                CmdConstant.NetWorkReconnecting,
                CmdConstant.LoadMapObj,
                Role_RoleLogin.TagName,
                Role_RoleNotify.TagName,
                Build_CreateBuilding.TagName,
                CmdConstant.FogSystemLoadEnd,
                CmdConstant.LoginInitFinish,
                CmdConstant.ChangeRolePosGuide,
                CmdConstant.UIPressed,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)//TODO,在log不能确定行数的前提下，有效代码不能超过10行
        {
            switch (notification.Name)
            {
                case CmdConstant.RemoveOtherCity:
                {
                    long Rid = (long) notification.Body;
                    if (Rid != m_playerProxy.CurrentRoleInfo.rid)
                    {
                        RemoveCityObjData(Rid);
                    }

                 //     Debug.LogError("remove," + Rid);
                }
                    break;
                case CmdConstant.NetWorkReconnecting:
                    Clear();
                    break;
                case Role_RoleNotify.TagName:
                {
                    Role_RoleNotify.request request = notification.Body as Role_RoleNotify.request;
                    if (request != null)
                    {
                        if (request.notifyOperate == 2)
                        {
                            m_roleNotifyMoveCity = true;
                            if (PlayerProxy.LoadCityFinished)
                            {
                                Alert.CreateAlert(770106, LanguageUtils.getText(730120)).SetLeftButton(
                                    () =>
                                    {
                                        WorldCamera.Instance().ViewTerrainPos(m_playerProxy.CurrentRoleInfo.pos.x / 100,
                                            m_playerProxy.CurrentRoleInfo.pos.y / 100, 0, () => { });
                                        CoreUtils.uiManager.CloseLayerUI(UILayer.WindowLayer);
                                        CoreUtils.uiManager.CloseLayerUI(UILayer.WindowPopLayer);
                                        float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                                        WorldCamera.Instance().SetCameraDxf(Firstdxf, 0, () => { });
                                        RefreshMyCityPos();
                                    }, LanguageUtils.getText(192009)).Show();
                            }
                        }
                    }
                }
                    break;
                case CmdConstant.LoadMapObj:
                {
                    MapObjectInfoEntity mapObjectExtEntity = notification.Body as MapObjectInfoEntity;
                    if (mapObjectExtEntity != null)
                    {
                        CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(mapObjectExtEntity.cityRid);
                        if (cityObjData != null)
                        {
                            if (mapObjectExtEntity.cityRid != m_playerProxy.CurrentRoleInfo.rid)
                            {
                                if (!cityObjData.assetLoading)
                                {
                                    cityObjData.assetLoading = true;
                                    cityObjData.CreateBuilfinginfo = false;
                                    CreateCityBuildingContainer(cityObjData, null);
                                }
                            }
                            else
                            {
                                SetCityFireState(cityObjData);
                            }

                            //   Debug.LogError(cityObjData.mapObjectExtEntity.cityRid);
                        }
                        else
                        {
                        }
                    }

                    break;
                }
                case CmdConstant.CityCitybuffChange:
                {
                    MapObjectInfoEntity mapObjectExtEntity = notification.Body as MapObjectInfoEntity;
                    if (mapObjectExtEntity != null)
                    {
                        CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(mapObjectExtEntity.cityRid);
                        if (cityObjData != null)
                        {
                            if (mapObjectExtEntity.cityRid != m_playerProxy.CurrentRoleInfo.rid)
                            {
                                AddBuffEffect(cityObjData);
                            }
                            else
                            {
                                AddBuffEffect(cityObjData);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                    break;
                case CmdConstant.CityPosTimeChange:
                {
                    MapObjectInfoEntity mapObjectExtEntity = notification.Body as MapObjectInfoEntity;
                    if (mapObjectExtEntity != null)
                    {
                        CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(mapObjectExtEntity.cityRid);
                        if (cityObjData != null)
                        {
                            if (mapObjectExtEntity.cityRid != m_playerProxy.CurrentRoleInfo.rid)
                            {
                                MoveOtherCity(cityObjData);
                            }
                        }
                        else
                        {
                        }
                    }

                    break;
                }
                case CmdConstant.CityBeginBurnTimeChange:
                    {
                        MapObjectInfoEntity mapObjectExtEntity = notification.Body as MapObjectInfoEntity;
                        if (mapObjectExtEntity != null)
                        {
                            CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(mapObjectExtEntity.cityRid);
                            if (cityObjData != null)
                            {
                                if (mapObjectExtEntity.cityRid != m_playerProxy.CurrentRoleInfo.rid)
                                {
                                    if (cityObjData.go != null)
                                    {
                                        FireOtherCityBuilding(cityObjData);
                                        cityObjData.CreateRunCitizen();
                                        SetCityFireState(cityObjData);
                                    }
                                }
                                else
                                {
                                    SetCityFireState(cityObjData);
                                }
                                //  Debug.LogError(cityObjData.mapObjectExtEntity.cityRid);
                            }
                            else
                            {
                            }
                        }
                        break;
                    }
                case CmdConstant.MapObjectChange:
                {
                    MapObjectInfoEntity mapObjectExtEntity = notification.Body as MapObjectInfoEntity;
                    if (mapObjectExtEntity != null)
                    {
                        m_cityBuildingProxy.RefOtherCityDic(mapObjectExtEntity);
                    }
                }
                    break;
                case CmdConstant.OnCloseBuildingHudMenu:
                    {
                        if (m_cityBuildingContainer == null)
                        {
                            return;
                        }
                        if (tempBuildingInfoEntity != null)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.CreateTempBuildNO);
                        }
                        else
                        {
                            ResetBuildMovePos();
                        }
                    }
                    break;
                case CmdConstant.buildQueueChange:
                    RefreshUpgradeBoard();
                    WorkAssign();
                    break;
                case CmdConstant.armyQueueChange:
                case CmdConstant.technologyQueueChange:
                case CmdConstant.treatmentChange:
                    RefreshUpgradeBoard();
                    break;
                case CmdConstant.UpdateCurrency:
                case Build_UpGradeBuilding.TagName:
                {
                    RefreshUpgradeBoard();
                }
                    break;
                case CmdConstant.CityBuildinginfoChange:
                    if (PlayerProxy.LoginInitIsFinish)
                    {
                        UpdatePlayerCity();
                        RefreshUpgradeBoard();
                    }

                    break;
                case CmdConstant.CityInViewPort:
                {
                    long cityRid = (long) notification.Body;
                    CityInViewPort(cityRid);
                }
                    break;
                case Map_GetCityDetail.TagName:
                {
                    Map_GetCityDetail.response req = notification.Body as Map_GetCityDetail.response;
                    if (req != null)
                    {
                        m_cityBuildingProxy.UpdateOtherBuildingInfo(req.targetRid, req.buildingInfo);
                    }

                    break;
                }
                case CmdConstant.OtherBuildingChange:
                {
                    long rid = (long) notification.Body;
                    CreateOtherCity(rid);
                }
                    break;
                case CmdConstant.FogSystemLoadEnd://TODO:迷雾和地表和城市建筑完成后在切换城市镜头
                    {
                    //迷雾和地表完成后在切换城市镜头
                    PlayerProxy.IsFogSystemInited = true;
                    
                    if (PlayerProxy.LoginInitIsFinish)
                    {
                        FristInitCityBuild();
                    }
                    break;
                }
                    
                case CmdConstant.LoginInitFinish:
                {
                    //迷雾和地表完成后在切换城市镜头
                    if (PlayerProxy.IsFogSystemInited)
                    {
                        FristInitCityBuild();
                    }
                    break;
                }

                
                    
                case CmdConstant.CityBuildingDone:
                    if (m_roleNotifyMoveCity)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.FirstEnterCityStartEndter);
                        WorldCamera.Instance().ViewTerrainPos(m_cityBuildingProxy.RolePos.x,
                            m_cityBuildingProxy.RolePos.y, 0, () => 
                            {
                            });
                        float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                        WorldCamera.Instance().SetCameraDxf(Firstdxf, 0, () =>
                        {
                            Alert.CreateAlert(LanguageUtils.getTextFormat(770106))
                                .SetRightButton(() => { CityDown(); }).Show();
                        });
                    }
                    else
                    {
                        if (PlayerProxy.LoadCityFinished)
                        {
                            RssProxy m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
                            var viewCenter = WorldCamera.Instance().GetViewCenter();
                            m_RssProxy.SendMapMove((long)viewCenter.x, (long)viewCenter.y);
                            RefreshUpgradeBoard();
                            if (m_globalViewLevelMediator.IsInCity())
                            {
                                StartBuilder();
                                if (m_cityBuildingProxy.MyCityObjData.fireState == FireState.FIRED)
                                {
                                    SetFireState();
                                }
                            }
                            return;
                        }

                        Debug.Log("迷雾主城创建完毕，开始拉镜头");
                        WorldCamera.Instance().SetCanDrag(false);
                        WorldCamera.Instance().SetCanZoom(false);
                        WorldCamera.Instance().SetCanClick(false);
                        WorldCamera.Instance().ViewTerrainPos(m_playerProxy.CurrentRoleInfo.pos.x / 100f,
                                 m_playerProxy.CurrentRoleInfo.pos.y / 100f, 0f, () => { });

                        float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                        WorldCamera.Instance().SetCameraDxf(Firstdxf, 0.3f, () =>
                        {
                            AppFacade.GetInstance()
                                .SendNotification(CmdConstant.FirstEnterCityStartEndter);
                            float dxf = WorldCamera.Instance().getCameraDxf("city_default");
                            int time = 2000;
                            WorldCamera.Instance().SetCameraDxf(dxf, time, () =>
                            {
                                Debug.Log("迷雾主城创建完毕，开始拉镜头2");
                                WorldCamera.Instance().SetCanDrag(true);
                                WorldCamera.Instance().SetCanZoom(true);
                                WorldCamera.Instance().SetCanClick(true);
                                //初始镜头表现完 再进行引导处理 否则城内 城外判断会有问题
                                AppFacade.GetInstance().SendNotification(CmdConstant.StartProcessGuide);
                                AppFacade.GetInstance()
                                    .SendNotification(CmdConstant.OnCityLoadFinished);
                                CheckedAgeChange();
                            });
                        });
                    }
                    CoreUtils.logService.Info("迷雾主城创建完毕", Color.green);
                    CoreUtils.audioService.StopByName(RS.SoundLogin);
                    RefreshUpgradeBoard();
                    //  StartBuilder();
                    if (m_cityBuildingProxy.MyCityObjData.fireState == FireState.FIRED)
                    {
                        SetFireState();
                    }

                    break;
                case CmdConstant.DayNightChange:
                    if(GameModeManager.Instance.CurGameMode == GameModeType.World && !CoreUtils.uiManager.ExistUI(UI.s_expeditionFight))
                    {
                        bool isDay = (bool)notification.Body;
                        Debug.Log("DayNightChange 播放音乐" + (!isDay ? RS.SoundCityNight : RS.SoundCityDay));
                        CoreUtils.audioService.PlayBgm(!isDay ? RS.SoundCityNight : RS.SoundCityDay);
                    }
                    break;

                case CmdConstant.ShowBuildingMenu:
                {
                    BuildingInfoEntity BuildingInfoEntity = notification.Body as BuildingInfoEntity;
                    if (BuildingInfoEntity != null)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ClickEnterCity);
                        ShowMenu(BuildingInfoEntity);
                        MoveCameraToBuilding(BuildingInfoEntity);
                    }
                }
                    break;
                case CmdConstant.ShowBuildingMenuOnly:
                {
                    BuildingInfoEntity BuildingInfoEntity = notification.Body as BuildingInfoEntity;
                    if (BuildingInfoEntity != null)
                    {
                        ShowMenu(BuildingInfoEntity);
                    }
                }
                    break;

                case CmdConstant.CityBuildingLevelUP:
                {
                    if (notification.Body is long)
                    {
                        long builidngindex = (long) notification.Body;
                        BuildingInfoEntity buildingInfoEntity =
                            m_cityBuildingProxy.GetBuildingInfoByindex(builidngindex);
                        if (buildingInfoEntity.type == (long) (EnumCityBuildingType.TownCenter) &&
                            m_cityBuildingProxy.NewAgeByLevel(buildingInfoEntity.level))
                        {
                            return;
                        }

                        RefreshUpgradeBoard();
                        GameObject gameObject = m_cityBuildingProxy.BuildObjDic[builidngindex];
                        Transform transform = ClientUtils.FindDeepChild(gameObject, "effect") != null
                            ? ClientUtils.FindDeepChild(gameObject, "effect")
                            : CityObjData.GetMenuTargetGameObject(buildingInfoEntity.buildingIndex).transform;
                           ClientUtils.AddEffect("build_3004", transform.position, null);
                            CoreUtils.audioService.PlayOneShot(RS.sfx_buildingUp);
                            WorkAssign();
                        }
                }

                    break;
                case CmdConstant.ShowBuildingMenuAndMoveCameraToBuilding:
                {
                    BuildingInfoEntity BuildingInfoEntity = notification.Body as BuildingInfoEntity;
                    if (BuildingInfoEntity != null)
                    {
                        MoveCameraToBuilding(BuildingInfoEntity);
                    }
                }
                    break;
                case CmdConstant.MoveCameraToBuilding:
                {
                    BuildingInfoEntity BuildingInfoEntity = notification.Body as BuildingInfoEntity;
                    if (BuildingInfoEntity != null)
                    {
                        MoveCameraToBuilding(BuildingInfoEntity);
                    }
                }
                    break;
                case CmdConstant.CreateTempBuild:


                    if (tempBuildingInfoEntity != null)
                    {
                        return;
                    }

                    tempBuildType = notification.Body as BuildingTypeConfigDefine;
                    tempBuildingInfoEntity = new BuildingInfoEntity();
                    Vector2 pos = GetRandBuildLogic(tempBuildType.width, tempBuildType.length);
                    tempBuildingInfoEntity.level = 0;
                    tempBuildingInfoEntity.pos = new PosInfo();
                    tempBuildingInfoEntity.pos.x = (int) pos.x;
                    tempBuildingInfoEntity.pos.y = (int) pos.y;
                    tempBuildingInfoEntity.type = tempBuildType.type;
                    tempBuildingInfoEntity.buildingIndex = m_cityBuildingProxy.GenServerBuildIndexID();
                    tempBuildingInfoEntity.finishTime = 0;


                    Debug.LogFormat("创建临时建筑{0} x:{1} y:{2} index:{3}", tempBuildType.type, pos.x, pos.y,
                        tempBuildingInfoEntity.buildingIndex);

                    IsGridUsed = CheckGridIsUeded(pos.x, pos.y, GetBuildCellRow((int) tempBuildType.type));

                    CreateCityBuilding(tempBuildingInfoEntity, m_cityBuildingContainer,
                        ((go) =>
                        {
                            if (IsGridUsed)
                            {
                                m_isMoving = true;
                                DrawMesh();
                            }

                            ShowMenu(tempBuildingInfoEntity, true);
                            MoveCameraToBuilding(tempBuildingInfoEntity);


                            if (IsGridUsed)
                            {
                                BuildMoveMat(m_isMoving);
                            }
                        }), false, true);


                    break;
                case CmdConstant.CreateTempBuildYes:
                    {
                        CreateTempBuildYes();
                    }
                    break;
                case CmdConstant.CreateTempBuildNO:
                    //取消建筑菜单
                    if (tempBuildingInfoEntity != null)
                    {
                        Debug.Log("移除临时建筑" + tempBuildingInfoEntity.buildingIndex);
                        SelectBuild("");
                        ShowBuildMenu(false);
                        RemoveCityBuild(tempBuildingInfoEntity, true);

                        tempBuildingInfoEntity = null;
                        tempBuildType = null;
                        IsGridUsed = false;
                    }

                    break;
                case Build_CreateBuilding.TagName:


                    break;

                case CmdConstant.CityBuildingStart:
                {
                    if (notification.Body is long)
                    {
                        if (tempBuildingInfoEntity != null)
                        {
                            SelectBuild("");

                            CoreUtils.audioService.PlayOneShot(RS.SoundBuildingStartLevelup);

                            tempBuildingInfoEntity = null;
                            tempBuildType = null;
                        }


                        long builidngindex = (long) notification.Body;
                        BuildingInfoEntity building = m_cityBuildingProxy.GetBuildingInfoByindex(builidngindex);
                        m_selectBuild = m_cityBuildingProxy.BuildObjDic[builidngindex].transform;
                        m_selectBuild.name = string.Format("{0}_{1}", (EnumCityBuildingType) building.type,
                            building.buildingIndex);
                        m_selectBuildID = builidngindex;
                        m_selectType = (EnumCityBuildingType) building.type;

                        var tileCollideItem = m_selectBuild.GetComponent<GridCollideItem>();


                        if (tileCollideItem != null)
                        {
                            tileCollideItem.m_priority = m_cityBuildingProxy.GetprioritydByType(building.type);
                            m_tileCollederManager.Add(tileCollideItem);
                            tileCollideItem.size = GetBuildGirdSize(building.type);
                        }

                        ShowMenu(building);

                            ClientUtils.AddEffect("build_3003", m_selectBuild.transform.position, null);

                        UpdateBuildGrids(builidngindex);

                        if (builidngindex != m_selectBuildID)
                        {
                            Debug.Log("网络延迟了   需要重新刷新建筑");
                        }

                        if (m_cityBuildingProxy.IsTranBuild(building.type))
                        {
                            CreateTrainSoldiers();
                        }
                    }
                }
                    break;
                case CmdConstant.RemoveBuild:
                {
                    BuildingInfoEntity BuildingInfoEntity = notification.Body as BuildingInfoEntity;
                    //取消建筑菜单
                    if (BuildingInfoEntity != null)
                    {
                        if (m_cityBuildingProxy.IsTranBuild(BuildingInfoEntity.type))
                        {
                            RemoveTrainSoldier(BuildingInfoEntity.buildingIndex);
                        }

                        SelectBuild(""); 
                       RemoveCityBuild(BuildingInfoEntity);
                       ShowBuildMenu(false);

                    }
                }
                    break;
                case CmdConstant.CityAgeChangeLevelUpEffect:
                {
                    m_GridState = null;
                    if (m_GridState == null)
                    {
                        m_GridState = m_cityBuildingProxy.MakeCityMapEmpty();
                        Debug.Log("初始化网格数据" + m_cityBuildingProxy.GetAgeType());
                    }
                }
                    break;
                case CmdConstant.CityAgeChange:
                {
                    GlobalViewLevelMediator m =
                        AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                            GlobalViewLevelMediator;

                    MapViewLevel crrLevel = m.GetViewLevel();
                    MapViewLevel preLevel = m.GetPreMapViewLevel();

                    //进城
                    if (crrLevel == MapViewLevel.City)
                    {
                        m_cityBuildingProxy.AgeChange = false;
                        CoreUtils.uiManager.CloseLayerUI(UILayer.WindowMenuLayer);
                        CoreUtils.uiManager.CloseLayerUI(UILayer.WindowPopLayer);
                        CoreUtils.uiManager.CloseLayerUI(UILayer.WindowLayer);
                        AgeChangeEffect();
                    }
                    else
                    {
                        m_cityBuildingProxy.AgeChange = true;
                    }
                }
                    break;
                case CmdConstant.OnChapterDiaglogEnd:
                {
                    int m_currentGroup = (int) notification.Body;
                    if (m_currentGroup == m_cityBuildingProxy.GetCityAgeSizeDefine().ageDialogBegin)
                    {
                        AgeChangeEffect1();
                    }

                    if (m_currentGroup == m_cityBuildingProxy.GetCityAgeSizeDefine().ageDialogEnd)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.AgeEnd);
                    }
                }
                    break;
                case CmdConstant.EnterCity:
                {
                    int time = 200;
                    if (notification.Body is int)
                    {
                        time = (int) notification.Body;
                    }
                    
                    Debug.Log("迷雾开始拉近镜头");

                    //播放切换音效
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_CameraZoom");
                    WorldCamera.Instance().SetCanDrag(false);
                    WorldCamera.Instance().SetCanZoom(false);
                    WorldCamera.Instance().SetCanClick(false);
                    WorldCamera.Instance()
                        .ViewTerrainPos(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y, 300, () =>
                        {
                            Timer.Register(0.1f, () =>
                            {
                                float dxf = WorldCamera.Instance().getCameraDxf("city_default");
                                WorldCamera.Instance().SetCameraDxf(dxf, time, () =>
                                {
                                    CheckedAgeChange();
                                    RefreshUpgradeBoard();
                                    if (!GuideProxy.IsGuideing)
                                    {
                                        WorldCamera.Instance().SetCanZoom(true);
                                    }
                                    WorldCamera.Instance().SetCanDrag(true);
                                    WorldCamera.Instance().SetCanClick(true);
                                });
                            });
                        });
                }
                    break;
                case CmdConstant.EnterCityShow:
                {
                    MapObjectInfoEntity req = notification.Body as MapObjectInfoEntity;
                    if (req != null)
                    {
                        if (req.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                        {
                            if (PlayerProxy.LoginInitIsFinish)
                            {
                                Tip.CreateTip(LanguageUtils.getText(300110), Tip.TipStyle.City).Show();
                            }
                        }
                        else
                        {
                            Tip.CreateTip(LanguageUtils.getTextFormat(300056, req.cityName), Tip.TipStyle.City).Show();
                        }
                    }

                    if (!m_initedBuilders)
                    {
                        StartBuilder(true);
                    }
                    else
                    {
                        StartBuilder(false);
                    }
                        ShowBuildDetail();
                }
                    break;
                case CmdConstant.ExitCityHide:
                    AppFacade.GetInstance().SendNotification(CmdConstant.CreateTempBuildNO);
                    ResetBuildMovePos();
                    StopBuilder();
                    SelectBuild();
                    HideBuilddetail();
                    ShowBuildMenu(false);

                    break;
                case CmdConstant.ExitCity:
                    {
                        int time = 300;
                        if (m_cityBuildingProxy.RolePos == null)
                        {
                            return;//客服端城市位置未初始化完成
                        }
                    WorldCamera.Instance()
                        .ViewTerrainPos(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y, 500, () => { });

                    float dxf2 = WorldCamera.Instance().getCameraDxf("map_tactical");
                    WorldCamera.Instance().SetCameraDxf(dxf2, time, () =>
                    {
                        GameObject obj = null;
                        BuildingInfoEntity buildinginfo =
                            m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.TownCenter);
                        if (buildinginfo != null)
                        {
                            if (m_cityBuildingProxy.BuildObjDic.TryGetValue(buildinginfo.buildingIndex, out obj))
                            {
                                ShowUpgradeBoard(obj, false);
                            }
                        }
                    });
                    //播放切换音效
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_CameraZoom");
                    ShowBuildMenu(false);
                }
                    break;
                case CmdConstant.MapViewChange:
                {
                    if (m_globalViewLevelMediator == null)
                    {
                    }

                    m_globalViewLevelMediator =
                        AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                            GlobalViewLevelMediator;


                    MapViewLevel crrLevel = m_globalViewLevelMediator.GetViewLevel();
                    MapViewLevel preLevel = m_globalViewLevelMediator.GetPreMapViewLevel();
                    //进城
                    if (crrLevel == MapViewLevel.City)
                    {
                        Debug.Log("进入城市");
                       // StartEnterCity();
                        RefreshUpgradeBoard();
                    }
                    else if (crrLevel == MapViewLevel.Tactical)
                    {
                        Debug.Log("退出城市");
                       // StartExitCity();
                        GameObject obj = null;
                        BuildingInfoEntity buildinginfo =
                            m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.TownCenter);
                        if (buildinginfo != null)
                        {
                            if (m_cityBuildingProxy.BuildObjDic.TryGetValue(buildinginfo.buildingIndex, out obj))
                            {
                                ShowUpgradeBoard(obj, false);
                            }
                        }
                    }
                    else if (crrLevel == MapViewLevel.TacticsToStrategy_1)
                    {
                    }
                    else if (crrLevel == MapViewLevel.TacticsToStrategy_2)
                    {
                    }
                }
                    break;

                case CmdConstant.ArmyTrainStart:
                    long index2 = (long) notification.Body;

                    Debug.Log("开始训练");
                    foreach (var train in m_trains)
                    {
                        if (train.Value.buildIndex == index2)
                        {
                            train.Value.Train();
                        }
                    }


                    break;

                case CmdConstant.ArmyTrainEnd:
                    long index = (long) notification.Body;

                    Debug.Log("停止训练");
                    foreach (var train in m_trains)
                    {
                        if (train.Value.buildIndex == index)
                        {
                            train.Value.TrainStop();
                        }
                    }


                    break;


                case CmdConstant.OnTouche3D:
                {
                    if (m_worldMgrMediator == null) 
                    m_worldMgrMediator =
                        AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
                    if (!m_worldMgrMediator.IsWorldMapStateNormal()) return;
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3D(touche3DData.x, touche3DData.y, touche3DData.parentName, touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.OnTouche3DBegin:
                {
                    if (m_worldMgrMediator == null) 
                    m_worldMgrMediator =
                        AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
                    if (!m_worldMgrMediator.IsWorldMapStateNormal()) return;
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
                    if (m_worldMgrMediator == null) 
                    m_worldMgrMediator =
                        AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
                    if (!m_worldMgrMediator.IsWorldMapStateNormal()) return;
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3DEnd(touche3DData.x, touche3DData.y, touche3DData.parentName,
                            touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.UIPressed:
                    m_uiPressed = true;
                    break;
                case CmdConstant.OnTouche3DReleaseOutside:
                {
                    if (m_worldMgrMediator == null) 
                    m_worldMgrMediator =
                        AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
                    if (!m_worldMgrMediator.IsWorldMapStateNormal()) return;
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3DReleaseOutside(touche3DData.x, touche3DData.y, touche3DData.parentName,
                            touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.MapTouchOtherCity:
                    bool isShow = (bool) notification.Body;
                    m_isPvpTouchCity = isShow;
                    break;
                case CmdConstant.ChangeRolePos:
                    if (!m_roleNotifyMoveCity)
                    {
                        RefreshMyCityPos();
                    }

                    break;
                case CmdConstant.ChangeRolePosGuide:
                    {
                        if (m_cityBuildCp != null)
                        {
                            m_cityBuildCp.transform.position = new Vector3(m_playerProxy.CurrentRoleInfo.pos.x / 100f, 0,
                                m_playerProxy.CurrentRoleInfo.pos.y / 100f);
                            clearBuilder();
                            m_initedBuilders = false;
                            m_cityBuildingProxy.MyCityObjData.pos = new Vector2(m_playerProxy.CurrentRoleInfo.pos.x / 100f, m_playerProxy.CurrentRoleInfo.pos.y / 100f);
                        }
                        WorldCamera.Instance().ViewTerrainPos(m_playerProxy.CurrentRoleInfo.pos.x / 100f, m_playerProxy.CurrentRoleInfo.pos.y / 100f, 0, () => { });
                    }

                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_dataProxy = AppFacade.GetInstance().RetrieveProxy(DataProxy.ProxyNAME) as DataProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_effectinfoProxy = AppFacade.GetInstance().RetrieveProxy(EffectinfoProxy.ProxyNAME) as EffectinfoProxy;
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            m_globalViewLevelMediator =
                AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                    GlobalViewLevelMediator;
            m_cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            m_trainProxy = AppFacade.GetInstance().RetrieveProxy(TrainProxy.ProxyNAME) as TrainProxy;
            m_cityBuildingProxy.SetBuildingInfo();
            m_curCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            m_land_root = GameObject.Find("SceneObject/land_root");
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
        }
        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void OnRemove()
        {
            Clear();
            foreach (Transform trs in m_land_root.transform)
            {
                if (trs.gameObject.name.Split('_')[0] == "CityBuildingContainer")
                {
                    CoreUtils.assetService.Destroy(trs.gameObject);
                }
            }
            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);
        }
        public void Clear()
        {
            if (PlayerProxy.LoadCityFinished && GuideProxy.IsGuideing)
            {
                return;
            }
            foreach (Transform trs in m_land_root.transform)
            {
                if (trs.gameObject.name.Split('_')[0] == "CityBuildingContainer")
                {
                    CoreUtils.assetService.Destroy(trs.gameObject);
                }
            }

            m_cityBuildingProxy.ResetData();
            StopBuilder();
            m_soldiers.Clear();
            m_workers.Clear();
            m_trains.Clear();
            m_initedBuilders = false;
            m_showDownAnim = false;
            m_roleNotifyMoveCity = false;
            m_cityBuildingContainer = null;
            //alldone = true;
            if (timeFIREFIGHTING != null)
            {
                timeFIREFIGHTING.Cancel();
                timeFIREFIGHTING = null;
            }
            m_globalViewLevelMediator.m_isViewCityIDTactical.Clear();
        }

        #endregion

        #region 进出城市的时候处理


        private void FristInitCityBuild()
        {
            if (PlayerProxy.LoadCityFinished&& GuideProxy.IsGuideing)
            {
                return;
            }

            Debug.Log("首次创建主城");
            StarttimeDownDown();
            CreateCityBuildingContainer(m_playerProxy.CurrentRoleInfo.rid,
                m_playerProxy.CurrentRoleInfo.pos,
                m_playerProxy.GetTownHall(), 0, () =>
                {
                    if (m_cityBuildingProxy.RolePos == null)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.OnCityLoadFinished);
                        CheckedAgeChange();
                        return;
                    }
                });
        }
            private void OnWorldViewChange(float x, float y, float dxf)
        {
            UpdateCityWallState(dxf);
             }
        private void UpdateCityWallState(float dxf)
        {
            bool showWall = dxf <= HEIGHTWALL;
                if (showWall)
                {
                    StartEnterCity();
                }
                else
                {
                    StartExitCity();
                }
        }

        public void StartEnterCity()
        {
            var wallInfo = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.CityWall);

            if (wallInfo != null)
            {
                var wallObj = m_cityBuildingProxy.GetBuildObjByID(wallInfo.buildingIndex);
                if (wallObj != null)
                {
                    if (!wallObj.activeSelf)
                    {
                        wallObj.SetActive(true);
                    }
                }
            }

            var gdInfo = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.GuardTower);

            if (gdInfo != null)
            {
                var gdObj = m_cityBuildingProxy.GetBuildObjByID(gdInfo.buildingIndex);
                if (gdObj != null)
                {
                    if (!gdObj.activeSelf)
                    {
                        gdObj.SetActive(true);
                    }
                }
            }


            if (m_buildGroundHelper != null)
            {
                m_buildGroundHelper.SetState(TownCenterFloor.State.City);
            }

            ShowCityCityWall();
        }


        public void StartExitCity()
        {
            var wallInfo = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.CityWall);
            if (wallInfo != null)
            {
                var wallObj = m_cityBuildingProxy.GetBuildObjByID(wallInfo.buildingIndex);
                if (wallObj != null)
                {
                    if (wallObj.activeSelf)
                    {
                        wallObj.SetActive(false);
                    }
                }
            }

            var gdInfo = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.GuardTower);

            if (gdInfo != null)
            {
                var gdObj = m_cityBuildingProxy.GetBuildObjByID(gdInfo.buildingIndex);
                if (gdObj != null)
                {
                    if (gdObj.activeSelf)
                    {
                        gdObj.SetActive(false);
                    }
                }
            }

            if (m_buildGroundHelper != null)
            {
                m_buildGroundHelper.SetState(TownCenterFloor.State.Map);
            }

            HIdeOtherCityCityWall();
        }

        /// <summary>
        /// 停止其他人的小人//TODO：合并
        /// </summary>
        public void StopOtherBuilder()
        {
            foreach (var othercity in m_globalViewLevelMediator.m_isViewCityID)
            {
                if (othercity != m_playerProxy.CurrentRoleInfo.rid)
                {
                    CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(othercity);
                    if (cityObjData != null)
                    {
                        cityObjData.StopBuilder();
                    }
                }
            }
        }

        public void StartOtherBuilder()
        {
            foreach (var othercity in m_globalViewLevelMediator.m_isViewCityID)
            {
                if (othercity != m_playerProxy.CurrentRoleInfo.rid)
                {
                    CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(othercity);
                    if (cityObjData != null&& cityObjData.go!=null)
                    {
                        cityObjData.incity = true;
                        cityObjData.StartBuilder();
                    }
                }
            }
        }

        /// <summary>
        /// Lod2时隐藏其他人的城墙
        /// </summary>
        private void HIdeOtherCityCityWall()
        {
            List<MapObjectInfoEntity> mapObjectInfoEntities = m_worldMapObjectProxy.GetWorldMapObjects();
            mapObjectInfoEntities.ForEach((mapObjectInfoEntity) =>
            {
                if (m_globalViewLevelMediator.IsLodVisable(mapObjectInfoEntity.objectPos.x / 100,
                    mapObjectInfoEntity.objectPos.y / 100))
                {
                    if (mapObjectInfoEntity.objectType == 3)
                    {
                        CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(mapObjectInfoEntity.cityRid);
                        if (cityObjData != null)
                        {
                            if (mapObjectInfoEntity.cityRid != m_playerProxy.CurrentRoleInfo.rid)
                            {
                                {
                                    Dictionary<long, BuldingObjData> buildingList =
                                        new Dictionary<long, BuldingObjData>();
                                    cityObjData.buildingListByType.TryGetValue(EnumCityBuildingType.GuardTower,
                                        out buildingList);
                                    if (buildingList != null)
                                    {
                                        buildingList.Values.ToList().ForEach((buldingObjData) =>
                                        {
                                            if (buldingObjData.gameObject != null)
                                            {
                                                if (buldingObjData.gameObject.activeSelf)
                                                {
                                                    buldingObjData.gameObject.SetActive(false);
                                                }
                                            }
                                        });
                                    }
                                }

                                {
                                    Dictionary<long, BuldingObjData> buildingList =
                                        new Dictionary<long, BuldingObjData>();
                                    cityObjData.buildingListByType.TryGetValue(EnumCityBuildingType.CityWall,
                                        out buildingList);
                                    if (buildingList != null)
                                    {
                                        buildingList.Values.ToList().ForEach((buldingObjData) =>
                                        {
                                            if (buldingObjData.gameObject != null)
                                            {
                                                if (buldingObjData.gameObject.activeSelf)
                                                { 
                                                    buldingObjData.gameObject.SetActive(false);
                                            }
                                                //   Debug.LogErrorFormat("{0}", 11);
                                            }
                                            else
                                            {
                                                // Debug.LogErrorFormat("{0}", "NULL");
                                            }
                                        });
                                    }
                                }
                            }

                            if (cityObjData.go != null)
                            {
                                SetCityFireState(cityObjData, true);
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Lod1时显示其他人的城墙
        /// </summary>
        private void ShowCityCityWall()
        {
            foreach (var othercity in m_globalViewLevelMediator.m_isViewCityIDTactical)
            {
                CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(othercity);
                if (cityObjData != null)
                {
                    if (othercity != m_playerProxy.CurrentRoleInfo.rid)
                    {
                        {
                            Dictionary<long, BuldingObjData> buildingList = new Dictionary<long, BuldingObjData>();
                            cityObjData.buildingListByType.TryGetValue(EnumCityBuildingType.GuardTower,
                                out buildingList);
                            if (buildingList != null)
                            {
                                buildingList.Values.ToList().ForEach((buldingObjData) =>
                                {
                                    if (buldingObjData.gameObject != null)
                                    {
                                        if (!buldingObjData.gameObject.activeSelf)
                                        {
                                            buldingObjData.gameObject.SetActive(true);
                                            // Debug.LogErrorFormat("{0}", 11);
                                        }
                                    }
                                });
                            }
                        }

                        {
                            Dictionary<long, BuldingObjData> buildingList = new Dictionary<long, BuldingObjData>();
                            cityObjData.buildingListByType.TryGetValue(EnumCityBuildingType.CityWall, out buildingList);
                            if (buildingList != null)
                            {
                                buildingList.Values.ToList().ForEach((buldingObjData) =>
                                {
                                    if (buldingObjData.gameObject != null)
                                    {
                                        if (!buldingObjData.gameObject.activeSelf)
                                        {
                                            buldingObjData.gameObject.SetActive(true);
                                        }
       
                                    }
                                });
                            }
                        }
                    }

                    SetCityFireState(cityObjData, false);
                }
            }
        }

        #endregion


        #region 城市和建筑创建

        private void CreateTempBuildYes()
        {
            if (tempBuildingInfoEntity != null)
            {
                Debug.Log("确认创建新建筑" + tempBuildingInfoEntity.buildingIndex);
                //新建建筑菜单确认

                var newPos = ConvertCityObjLocalToTile((int)m_selectType, m_selectBuild.localPosition.x,
                    m_selectBuild.localPosition.z);

                tempBuildingInfoEntity.pos.x = (long)newPos.x;
                tempBuildingInfoEntity.pos.y = (long)newPos.y;
                m_cityBuildingProxy.SendCreateBuild(tempBuildType, tempBuildingInfoEntity.pos);

                tempBuildingInfoEntity.level = 1;
                //UpdateBuildGrids(tempBuildingInfoEntity.buildingIndex);
            }
        }
        /// <summary>
        /// 生成玩家城市
        /// </summary>
        private void CreatePlayerCity()
        {
            UpdatePlayerCity();
        }

        public void UpdatePlayerCity()
        {
            Dictionary<long, BuildingInfoEntity> CityBuildingInfoDic = m_cityBuildingProxy.CityBuildingInfoDic;
            foreach (var key in CityBuildingInfoDic.Keys)
            {
                if (!m_cityBuildingProxy.BuildObjDic.ContainsKey(key))
                {
                    if (m_cityBuildingContainer != null)
                    {
                        CreateCityBuilding(CityBuildingInfoDic[key], m_cityBuildingContainer.transform);
                    }
                }
            }
        }

        /// <summary>
        /// 生成其他人的城市
        /// </summary>
        /// <param name="response"></param>
        public void CreateOtherCity(long rid)
        {
            int i = 0;
            CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(rid);
            if (cityObjData == null)
            {
                return;
            }

            if (cityObjData.go == null)
            {
                cityObjData.CreateBuilfinginfo = true;
            }

            if (cityObjData != null && cityObjData.go != null)
            {
                Dictionary<long, BuldingObjData> CityBuildingInfoDic = cityObjData.buildingList;
                GridCollideMgr tileCollideManager = cityObjData.go.GetComponent<GridCollideMgr>();
                int count = CityBuildingInfoDic.Count;
                tileCollideManager.RemoveAll();
                foreach (var temp in CityBuildingInfoDic.Values)
                {
                    BuldingObjData buldingObjData = temp;
                    CreateOtherCityBuilding(cityObjData, buldingObjData, cityObjData.go.transform, rid, (gameObject) =>
                    {
                        i++;
                        buldingObjData.isGameObjectLoading = false;
                        if (cityObjData.go == null)
                        {
                            CoreUtils.assetService.Destroy(gameObject);
                            return;
                        }
                        gameObject.transform.SetParent(cityObjData.go.transform);
                        gameObject.name = string.Format("{0}_other_{1}", (buldingObjData.type), rid);
                        gameObject.transform.localPosition = ConvertCityObjTileToLocal(buldingObjData.buildingInfo.type,
                            buldingObjData.buildingInfo.pos.x, buldingObjData.buildingInfo.pos.y);
                        gameObject.transform.localEulerAngles = Vector3.zero;
                        GridCollideItem tileCollideItem = gameObject.GetComponent<GridCollideItem>();
                        if (tileCollideItem != null)
                        {
                            tileCollideManager.Add(tileCollideItem);
                            GridCollideItem.ResetInitLocalPosS(tileCollideItem);
                            tileCollideItem.m_auto_registre = true;
                            tileCollideItem.m_priority =
                                m_cityBuildingProxy.GetprioritydByType(buldingObjData.buildingInfo.type);
                            tileCollideItem.size = GetBuildGirdSize(buldingObjData.buildingInfo.type);
                        }

                        //特殊处理
                        if (buldingObjData.buildingInfo.type == (int) EnumCityBuildingType.CityWall)
                        {
                            var PatrolDummy = gameObject.transform.Find("PatrolDummy");
                            if (PatrolDummy)
                            {
                                //删除巡逻点
                                //cityObjData.cityWallPatrolSoldierDummy =
                                //    PatrolDummy.GetComponent<PatrolSoldier>();
                            }
                            var groundCollider = gameObject.transform.Find("groundCollider");
                            if (groundCollider)
                            {
                                groundCollider.GetComponent<BoxCollider>().enabled = true;
                            }
                            
                        }

                        buldingObjData.SetGameObject(gameObject);
                        buldingObjData.t_effects = new List<Transform>();
                        List<Transform> list = ClientUtils.FindDeepMulChild(gameObject, "fireeffect");

                        if (buldingObjData.type == EnumCityBuildingType.GuardTower ||
                            buldingObjData.type == EnumCityBuildingType.CityWall)
                        {
                            MapViewLevel crrLevel = m_globalViewLevelMediator.GetViewLevel();
                            if (crrLevel == MapViewLevel.Tactical)
                            {
                                gameObject.SetActive(false);
                            }
                        }

                        if (buldingObjData.type == EnumCityBuildingType.CityWall)
                        {
                            {
                                Transform transform = ClientUtils.FindDeepChild(gameObject, "city_wall_fire_0");
                                if (transform != null)
                                {
                                    buldingObjData.t_effects.Add(transform);
                                }
                            }
                            {
                                Transform transform = ClientUtils.FindDeepChild(gameObject, "city_wall_fire_1");
                                if (transform != null)
                                {
                                    buldingObjData.t_effects.Add(transform);
                                }
                            }
                            {
                                Transform transform = ClientUtils.FindDeepChild(gameObject, "city_wall_fire_2");
                                if (transform != null)
                                {
                                    buldingObjData.t_effects.Add(transform);
                                }
                            }
                            {
                                Transform transform = ClientUtils.FindDeepChild(gameObject, "city_wall_fire_3");
                                if (transform != null)
                                {
                                    buldingObjData.t_effects.Add(transform);
                                }
                            }
                            {
                                Transform transform = ClientUtils.FindDeepChild(gameObject, "city_wall_fire_4");
                                if (transform != null)
                                {
                                    buldingObjData.t_effects.Add(transform);
                                }
                            }
                            {
                                Transform transform = ClientUtils.FindDeepChild(gameObject, "city_wall_fire_5");
                                if (transform != null)
                                {
                                    buldingObjData.t_effects.Add(transform);
                                }
                            }
                        }
                        else
                        {
                            if (list.Count == 0)
                            {
                                buldingObjData.t_effects.Add(gameObject.transform);
                            }
                        }

                        list.ForEach((tr) => { buldingObjData.t_effects.Add(tr); });
                        ForceUpdateScaleS(cityObjData.go);
                        if (i == count)
                        {
                            UpdateCityView(cityObjData);
                        }
                    });
                }
            }
        }

        private void ForceUpdateScaleS(GameObject obj)
        {
            LevelDetailCastle lodCastle = obj.GetComponent<LevelDetailCastle>();
            LevelDetailCastle.ForceUpdateScaleS(lodCastle);
        }

        /// <summary>
        /// 更新其他人的城市视图
        /// </summary>
        private void UpdateCityView(CityObjData cityObjData)
        {
          float  CurrentCameraDxf  =  WorldCamera.Instance().getCurrentCameraDxf();
            //进城
            UpdateCityWallState(CurrentCameraDxf);
            if (m_globalViewLevelMediator.IsInCity())
            {
                cityObjData.incity = true;
                if (cityObjData.go != null)
                {
                    cityObjData.StartBuilder();
                }
            }
            else
            {
                cityObjData.StopBuilder();
            }
        }

        private void CityInViewPort(long cityRid)
        {
            if (cityRid != m_playerProxy.CurrentRoleInfo.rid)
            {
                Map_GetCityDetail.request req = new Map_GetCityDetail.request();
                req.targetRid = cityRid;
                m_netProxy.SendSproto(req);
            }
        }


        /// <summary>
        /// 创建城市建筑显示对象 添加数据
        /// </summary>
        /// <param name="cityBuildInfoTemp"></param>
        /// <param name="parentTransform"></param>
        private void CreateCityBuilding(BuildingInfoEntity cityBuildInfoTemp, Transform parentTransform,
            Action<GameObject> callback = null, bool replace = false, bool isNewCreate = false)
        {
            CoreUtils.assetService.Instantiate(m_cityBuildingProxy.GetModelIdByType(cityBuildInfoTemp.type),
                (GameObject gameObject) =>
                {
                    if (parentTransform == null)
                    {
                        return;
                    }

                    gameObject.transform.SetParent(parentTransform);
                    gameObject.name = string.Format("{0}_{1}", ((EnumCityBuildingType) cityBuildInfoTemp.type),
                        cityBuildInfoTemp.buildingIndex);
                    gameObject.transform.localPosition = ConvertCityObjTileToLocal(cityBuildInfoTemp.type,
                        cityBuildInfoTemp.pos.x,
                        cityBuildInfoTemp.pos.y);
                    //数据关联
                    m_cityBuildingProxy.AddBuild(cityBuildInfoTemp, gameObject);
                    if (!isNewCreate)
                    {
                        UpdateBuildGrids(cityBuildInfoTemp.buildingIndex);
                    }


                    gameObject.transform.localEulerAngles = Vector3.zero;

                    if (m_tileCollederManager == null)
                    {
                        m_tileCollederManager = parentTransform.GetComponent<GridCollideMgr>();
                    }

                    //特殊处理
                    if (cityBuildInfoTemp.type == (int) EnumCityBuildingType.CityWall)
                    {
                        var PatrolDummy = gameObject.transform.Find("PatrolDummy");
                        if (PatrolDummy)
                        {
                            //m_cityWallPatrolSoldierDummy = PatrolDummy.GetComponent<PatrolSoldier>();
                        }
                        var groundCollider = gameObject.transform.Find("groundCollider");
                        if (groundCollider)
                        {
                            groundCollider.GetComponent<BoxCollider>().enabled = true;
                        }
                    }

                    if (cityBuildInfoTemp.type == (int) EnumCityBuildingType.TownCenter)
                    {
                        m_buildGroundHelper = gameObject.GetComponentInChildren<TownCenterFloor>();
                        if (m_buildGroundHelper == null)
                        {
                            Debug.LogError("市政厅需要挂上 BuildingGroundHelper");
                        }
                    }

                    GridCollideItem tileCollideItem = gameObject.GetComponent<GridCollideItem>();

                    if (tileCollideItem != null && isNewCreate == false)
                    {
                        if (cityBuildInfoTemp.type == (int) EnumCityBuildingType.TownCenter)
                        {
                            GridCollideItem.ResetInitLocalPosS(tileCollideItem);
                        }

                        tileCollideItem.m_priority = m_cityBuildingProxy.GetprioritydByType(cityBuildInfoTemp.type);
                        m_tileCollederManager.Add(tileCollideItem);
                        tileCollideItem.size = GetBuildGirdSize(cityBuildInfoTemp.type);
                    }

                    if (callback != null)
                    {
                        callback(gameObject);
                    }

                    if (replace)
                    {
                        if (cityBuildInfoTemp.type == (int) EnumCityBuildingType.GuardTower)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.CityBuildingDone);
                        }
                    }
                    else
                    {
                        if (m_cityBuildingProxy.BuildObjDic.Count == m_cityBuildingProxy.CityBuildingInfoDic.Count &&
                            m_cityBuildingProxy.CityBuildingInfoDic.Count > 2)
                        {
                            if (cityBuildInfoTemp.level != 0)
                            {
                                if (m_cityBuildingContainer != null)
                                {
                                    ForceUpdateScaleS(m_cityBuildingContainer.gameObject);
                                    AppFacade.GetInstance().SendNotification(CmdConstant.CityBuildingDone);
                                    if (!PlayerProxy.LoadCityFinished)
                                    {
                                        PlayerProxy.LoadCityFinished = true;
                                    }
                                }
                            }
                        }
                    }
                });
        }

        /// <summary>
        /// 创建城市建筑显示对象 添加数据
        /// </summary>
        /// <param name="cityBuildInfoTemp"></param>
        /// <param name="parentTransform"></param>
        private void CreateOtherCityBuilding(CityObjData cityObjData, BuldingObjData buldingObjData,
            Transform parentTransform, long rid,
            Action<GameObject> callback = null)
        {
            //   Debug.LogErrorFormat("index:{0}objmodel{1}model{2}isGameObjectLoading{3}",buldingObjData.index, buldingObjData.modelObjId, buldingObjData.curmodelId, buldingObjData.isGameObjectLoading);
            if (!buldingObjData.isGameObjectLoading)
            {
                buldingObjData.isGameObjectLoading = true;
                if (buldingObjData.gameObject == null)
                {
                    if (buldingObjData.type == EnumCityBuildingType.TownCenter)
                    {
                        Transform townCenter =
                            parentTransform.Find(string.Format("{0}_other_{1}", EnumCityBuildingType.TownCenter, rid));
                        if (townCenter != null)
                        {
                            callback?.Invoke(townCenter.gameObject);
                        }
                        else
                        {
                            CoreUtils.assetService.Instantiate(buldingObjData.curmodelId,
                                (GameObject gameObject) =>
                                {
                                    if (cityObjData.townCenter != null)
                                    {
                                        CoreUtils.assetService.Destroy(cityObjData.townCenter);
                                    }

                                    callback?.Invoke(gameObject);
                                });
                        }
                    }
                    else
                    {
                        CoreUtils.assetService.Instantiate(buldingObjData.curmodelId,
                            (GameObject gameObject) => { callback?.Invoke(gameObject); });
                    }
                }
                else
                {
                    if (buldingObjData.curmodelId == buldingObjData.modelObjId)
                    {
                        callback?.Invoke(buldingObjData.gameObject);
                    }
                    else
                    {
                        CoreUtils.assetService.Destroy(buldingObjData.gameObject);
                        buldingObjData.gameObject = null;
                        CoreUtils.assetService.Instantiate(buldingObjData.curmodelId,
                            (GameObject gameObject) => { callback?.Invoke(gameObject); });
                    }
                }
            }
        }

        //移除建筑和建筑数据
        private void RemoveCityBuild(BuildingInfoEntity cityBuildInfoTemp, bool isNewCreate = false)
        {

            if (cityBuildInfoTemp==null)
            {
                return;
            }
            
            string name = string.Format("{0}_{1}", (EnumCityBuildingType) cityBuildInfoTemp.type,
                cityBuildInfoTemp.buildingIndex);
            if (m_cityBuildingContainer != null)
            {
                var cityBuild = m_cityBuildingContainer.Find(name);

                if (cityBuild != null)
                {
                    CoreUtils.assetService.Destroy(cityBuild.gameObject);
                }
                if (isNewCreate == false)
                {
                    UpdateBuildGrids(cityBuildInfoTemp.buildingIndex, CityObjData.CITY_GRID_STATE_NORMAL);
                }
                m_cityBuildingProxy.RemoveBuild(cityBuildInfoTemp);
            }
        }


        //创建主城
        private void CreateCityBuildingContainer(long cityRid, PosInfo posInfo, long cityLevel, long townCenterID,
            Action createEnd = null)
        {
            CoreUtils.assetService.Instantiate("CityBuildingContainer", (GameObject cityBuildingRoot) =>
            {
                 //服务器下发坐标转地图坐标
                cityBuildingRoot.name = string.Format("{0}_{1}", "CityBuildingContainer", cityRid);
                cityBuildingRoot.transform.SetParent(GameObject.Find("SceneObject/land_root").transform);
                LevelDetailCastle lodCastle = cityBuildingRoot.GetComponent<LevelDetailCastle>();

                cityBuildingRoot.transform.localPosition =
                    new Vector3( posInfo.x / 100f, 0, posInfo.y / 100f);
                cityBuildingRoot.transform.localEulerAngles = new Vector3(0, -45, 0);

                GridCollideMgr tileCollederManager = cityBuildingRoot.GetComponent<GridCollideMgr>();
                TownSearch cityMapFinder = cityBuildingRoot.GetComponent<TownSearch>();
                float size = m_cityBuildingProxy.GetCityAgeSize(cityLevel);
                if (tileCollederManager != null)
                {
                    var newSize = size * 0.1f + 0.05f;
                    tileCollederManager.SetSize(new Vector2(newSize, newSize));
                    float maxScale = (float) size / (float) m_cityBuildingProxy.GetCityAgeMaxSize();
                    LevelDetailCastle.SetMaxScaleS(lodCastle, maxScale);
                }

                if (cityRid == m_playerProxy.CurrentRoleInfo.rid)
                {
                    cityBuildingRoot.name = "CityBuildingContainer";
                    m_cityBuildingContainer = cityBuildingRoot.transform;
                    m_cityBuildCp = m_cityBuildingContainer.GetComponent<TownBuildingContainer>();
                    if (m_cityBuildCp)
                    {
                        m_cityBuildCp.SetApplicationFocus(OnApplicationFocus);
                    }

                    m_tileCollederManager = tileCollederManager;
                    m_cityMapFinder = cityMapFinder;
                    if (m_GridState == null)
                    {
                        m_GridState = m_cityBuildingProxy.MakeCityMapEmpty();
                        Debug.Log(cityRid + "初始化网格数据" + m_cityBuildingProxy.GetAgeType());
                    }

                    if (m_cityBuildingProxy.MyCityObjData != null)
                    {
                        m_cityBuildingProxy.MyCityObjData.SetGameobject(cityBuildingRoot);
                        if (m_cityBuildingProxy.MyCityObjData.mapObjectExtEntity != null)
                        {
                            m_cityBuildingProxy.MyCityObjData.mapObjectExtEntity.gameobject = cityBuildingRoot;
                            AddBuffEffect(m_cityBuildingProxy.MyCityObjData);
                        }
                    }

                    CreatePlayerCity();
                    AppFacade.GetInstance().SendNotification(CmdConstant.FirstEnterCity, 2000);
                }

                AppFacade.GetInstance().SendNotification(CmdConstant.CreateCityDone, cityRid);
                if (createEnd != null)
                {
                    createEnd();
                }
            });
        }

        /// <summary>
        /// 10秒后显示落下动画
        /// </summary>
        private void StarttimeDownDown()
        {
            Timer.Register(10, () => { m_showDownAnim = true; }, null, false, false, null);
        }
        public void RemoveCityObjData(long rid)
        {
            CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(rid);
            if (cityObjData != null)
            {
                if (cityObjData.go != null)
                {
                    cityObjData.OnRemove();
                    if (m_globalViewLevelMediator.IsCityInViewPortTactical(cityObjData.pos.x, cityObjData.pos.y,""))
                    {
                        if (WorldCamera.Instance().IsAutoMoving() || WorldCamera.Instance().IsMovingToPos() || WorldCamera.Instance().IsSlipping())
                        {

                        }
                        else
                        {
                            CoreUtils.assetService.Instantiate(RS.Cityfly, (go) =>
                            {
                                CoreUtils.audioService.PlayOneShot("Sound_operation_2018");
                                go.transform.position = new Vector3(cityObjData.pos.x, 0, cityObjData.pos.y);
                            });
                        }
                    }
                }
                m_cityBuildingProxy.RemoveCity(rid);
            }
        }
        private void CreateCityBuildingContainer(CityObjData cityObjData,
            Action createEnd = null)
        {
            if (cityObjData.go == null)
            {
                CoreUtils.assetService.Instantiate("CityBuildingContainer", (GameObject cityBuildingRoot) =>
                {
                    GridCollideMgr tileCollederManager = cityBuildingRoot.GetComponent<GridCollideMgr>();
                    TownSearch cityMapFinder = cityBuildingRoot.GetComponent<TownSearch>();
                    cityObjData.SetGameobject(cityBuildingRoot);
                    cityObjData.mapObjectExtEntity.gameobject = cityBuildingRoot;
                    cityObjData.cityMapFinder = cityMapFinder;
     //服务器下发坐标转地图坐标
                    cityBuildingRoot.name = string.Format("{0}_{1}", "CityBuildingContainer",
                        cityObjData.mapObjectExtEntity.cityRid);
                    cityBuildingRoot.transform.SetParent(GameObject.Find("SceneObject/land_root").transform);
                    LevelDetailCastle lodCastle = cityBuildingRoot.GetComponent<LevelDetailCastle>();

                    cityBuildingRoot.transform.localPosition =
                        new Vector3(cityObjData.mapObjectExtEntity.objectPos.x / 100f, 0, cityObjData.mapObjectExtEntity.objectPos.y / 100f);
                    cityBuildingRoot.transform.localEulerAngles = new Vector3(0, -45, 0);
                    float size = m_cityBuildingProxy.GetCityAgeSize(cityObjData.mapObjectExtEntity.cityLevel);
                    if (tileCollederManager != null)
                    {
                        var newSize = size * 0.1f + 0.05f;
                        tileCollederManager.SetSize(new Vector2(newSize, newSize));
                        float maxScale = (float) size / (float) m_cityBuildingProxy.GetCityAgeMaxSize();
                        LevelDetailCastle.SetMaxScaleS(lodCastle, maxScale);
                    }
                    m_globalViewLevelMediator.RefreshMapObjAndHud();
                    CreateCityBuildingContainerEnd(cityObjData, cityBuildingRoot, createEnd);
                    if (WorldCamera.Instance().IsAutoMoving() || WorldCamera.Instance().isMovingToPos|| !m_showDownAnim || Math.Abs(
                            cityObjData.mapObjectExtEntity.cityPosTime - ServerTimeModule.Instance.GetServerTime()) > 5)
                    {
                        // Debug.LogErrorFormat("IsAutoMoving{0}m_showDownAnim{1}cityPosTime{2}GetServerTime{3}timespan{4}", WorldCamera.Instance().IsAutoMoving(), m_showDownAnim, cityObjData.mapObjectExtEntity.cityPosTime, ServerTimeModule.Instance.GetServerTime(), cityObjData.mapObjectExtEntity.cityPosTime - ServerTimeModule.Instance.GetServerTime());
                    }
                    else
                    {
                        cityBuildingRoot.gameObject.SetActive(false);
                        CoreUtils.assetService.Instantiate(RS.CityDown,
                            (go) => { 
                                go.transform.position = cityBuildingRoot.transform.position;
                                if (PlayerProxy.LoginInitIsFinish)
                                {
                                    CoreUtils.audioService.PlayOneShot("Sound_operation_2019");
                                }
                            });
                        Timer.Register(3, () =>
                        {
                            if (cityBuildingRoot != null)
                            {
                                cityBuildingRoot.gameObject.SetActive(true);
                                LevelDetailCastle lodCastle1 = cityBuildingRoot.GetComponent<LevelDetailCastle>();

                                cityBuildingRoot.transform.localPosition =
                                    new Vector3(cityObjData.mapObjectExtEntity.objectPos.x / 100f, 0, cityObjData.mapObjectExtEntity.objectPos.y / 100f);
                                cityBuildingRoot.transform.localEulerAngles = new Vector3(0, -45, 0);
                                float size1 =
                                    m_cityBuildingProxy.GetCityAgeSize(cityObjData.mapObjectExtEntity.cityLevel);
                                if (tileCollederManager != null)
                                {
                                    var newSize = size1 * 0.1f + 0.05f;
                                    tileCollederManager.SetSize(new Vector2(newSize, newSize));
                                    float maxScale = (float) size1 / (float) m_cityBuildingProxy.GetCityAgeMaxSize();
                                    LevelDetailCastle.SetMaxScaleS(lodCastle1, maxScale);
                                }
                                m_globalViewLevelMediator.RefreshMapObjAndHud();
                            }
                        });
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.CreateCityDone, cityObjData.mapObjectExtEntity.cityRid);
                });
            }
            else
            {
                if ((cityObjData.go.transform.position.x) !=
                    cityObjData.mapObjectExtEntity.objectPos.x / 100f &&
                    (cityObjData.go.transform.position.y) !=
                    cityObjData.mapObjectExtEntity.objectPos.y / 100f)
                {
                    cityObjData.go.transform.localPosition =
                        new Vector3(cityObjData.mapObjectExtEntity.objectPos.x / 100f, 0,
                            cityObjData.mapObjectExtEntity.objectPos.y / 100f);
                }

                CreateCityBuildingContainerEnd(cityObjData, cityObjData.go, createEnd);
            }
        }

        public void CreateCityBuildingContainerEnd(CityObjData cityObjData, GameObject cityBuildingRoot,
            Action createEnd)
        {
            MapViewLevel crrLevel = m_globalViewLevelMediator.GetViewLevel();
            MapViewLevel preLevel = m_globalViewLevelMediator.GetPreMapViewLevel();
            cityObjData.assetLoading = false;
            string modelObjId = m_cityBuildingProxy.GetModelIdByType((long) EnumCityBuildingType.TownCenter,
                cityObjData.mapObjectExtEntity.cityCountry, cityObjData.AgeType);
            bool createTownCenter = false;
            if (modelObjId != cityObjData.modelObjId)
            {
                if (cityObjData.townCenter != null)
                {
                    CoreUtils.assetService.Destroy(cityObjData.townCenter);
                    createTownCenter = true;
                }
                else
                {
                    createTownCenter = true;
                }
            }
            cityObjData.StopBuilder();
            //进城
            if (crrLevel >= MapViewLevel.City && createTownCenter)
            {
                if (cityObjData.CreateBuilfinginfo)
                {
                    CreateOtherCity(cityObjData.mapObjectExtEntity.cityRid);
                }
                else
                {
                    CoreUtils.assetService.Instantiate(modelObjId, (GameObject gameObject) =>
                       {
                           if (cityObjData != null && cityObjData.go != null)
                           {
                               GridCollideMgr tileCollideManager = cityObjData.go.GetComponent<GridCollideMgr>();
                               cityObjData.modelObjId = modelObjId;
                               cityObjData.SetTownCenter(gameObject);
                               gameObject.transform.SetParent(cityBuildingRoot.transform);
                               gameObject.name = string.Format("{0}_other_{1}", EnumCityBuildingType.TownCenter,
                                   cityObjData.mapObjectExtEntity.cityRid);
                               gameObject.transform.localPosition =
                                   ConvertCityObjTileToLocal((long)EnumCityBuildingType.TownCenter, 0, 0);
                               gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);

                               GridCollideItem tileCollideItem = gameObject.GetComponent<GridCollideItem>();
                               if (tileCollideItem != null)
                               {
                                   tileCollideManager.Add(tileCollideItem);
                                   GridCollideItem.ResetInitLocalPosS(tileCollideItem);
                                   tileCollideItem.m_priority =
                                       m_cityBuildingProxy.GetprioritydByType((int)EnumCityBuildingType.TownCenter);
                                   tileCollideItem.m_auto_registre = true;
                                   tileCollideItem.size = GetBuildGirdSize((int)EnumCityBuildingType.TownCenter);
                                   ForceUpdateScaleS(cityObjData.go);
                               }
                           }
                           else
                           {
                               CoreUtils.assetService.Destroy(gameObject);
                           }
                       });
                }

                createEnd?.Invoke();
            }
            else
            {
                createEnd?.Invoke();
            }

            AddBuffEffect(cityObjData);
            SetCityFireState(cityObjData);
        }

        private void RefreshMyCityPos()
        {
            if (GuideProxy.IsGuideing)
            {
                return;
            }
            if (m_cityBuildingProxy.RolePos == null)
            {
                return;
            }

            if (m_playerProxy.CurrentRoleInfo.pos.x == m_cityBuildingProxy.RolePos.x &&
                m_playerProxy.CurrentRoleInfo.pos.x == m_cityBuildingProxy.RolePos.y)
            {
                return;
            }

            if (m_cityBuildCp == null)
            {
                return;
            }

            if (!m_globalViewLevelMediator.IsCityInViewPortTactical(m_playerProxy.CurrentRoleInfo.pos.x / 100f,
                m_playerProxy.CurrentRoleInfo.pos.y / 100f, ""))
            {
                WorldCamera.Instance().ViewTerrainPos(m_playerProxy.CurrentRoleInfo.pos.x / 100f,
                    m_playerProxy.CurrentRoleInfo.pos.y / 100f, 100, () => { });
            }

            CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(m_playerProxy.CurrentRoleInfo.rid);
            if (cityObjData != null && cityObjData.go != null)
            {
                WorldCamera.Instance().SetCanClick(false);
                WorldCamera.Instance().SetCanDrag(false);
                WorldCamera.Instance().SetCanZoom(false);
                m_cityBuildingProxy.LockMoveEvent = true;

                CoreUtils.assetService.Instantiate(RS.Cityfly,
                    (go) => { go.transform.position = new Vector3(cityObjData.pos.x, 0, cityObjData.pos.y); });
                CoreUtils.assetService.Instantiate(RS.CityDown,
                    (go) =>
                    {
                        if (PlayerProxy.LoginInitIsFinish)
                        {
                            CoreUtils.audioService.PlayOneShot("Sound_operation_2019");
                        }
                        go.transform.position = new Vector3(m_playerProxy.CurrentRoleInfo.pos.x / 100, 0,
                            m_playerProxy.CurrentRoleInfo.pos.y / 100);
                    });
                Timer.Register(1.5f, () =>
                {
                    m_cityBuildingProxy.LockMoveEvent = false;
                    WorldCamera.Instance().SetCanClick(true);
                    WorldCamera.Instance().SetCanDrag(true);
                    WorldCamera.Instance().SetCanZoom(true);
                    if (m_cityBuildCp != null)
                    {
                        m_cityBuildCp.transform.position = new Vector3(m_playerProxy.CurrentRoleInfo.pos.x / 100f, 0,
                            m_playerProxy.CurrentRoleInfo.pos.y / 100f);
                        clearBuilder();
                        m_initedBuilders = false;
                        cityObjData.pos = new Vector2(m_playerProxy.CurrentRoleInfo.pos.x / 100f,
                            m_playerProxy.CurrentRoleInfo.pos.y / 100f);
                        cityObjData.ProvinceName = MapManager.Instance().GetMapProvinceName(cityObjData.pos);
                        m_roleNotifyMoveCity = false;
                        {
                            MapObjectInfoEntity mapItemInfo = cityObjData.mapObjectExtEntity;
                            if (mapItemInfo != null)
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectHUDUpdate, mapItemInfo);
                            }
                            m_globalViewLevelMediator.RefreshMapObjAndHud();
                        }
                        AppFacade.GetInstance().SendNotification(CmdConstant.ChangeRoleObjPos);
                    }
                });
            }
        }

        #endregion


        #region 拖动时候选择物件的创建

        private void DrawMesh()
        {
            m_liftUp = true;
            if (m_cityMesh == null)
            {
                CoreUtils.assetService.Instantiate("CityMesh",
                    (GameObject gameObject) =>
                    {
                        gameObject.transform.SetParent(m_cityBuildingContainer);
                        gameObject.transform.localPosition = new Vector3(0, 0, 0.1f);
                        gameObject.transform.localRotation = Quaternion.Euler(0f, -45f, 0f);
                        gameObject.transform.localEulerAngles = Vector3.zero;
                        gameObject.name = "CityMesh";
                        m_cityMesh = gameObject.GetComponent<GridLines>();
                        int size = m_cityBuildingProxy.GetCitySize();
                        float[] vlist = new float[size * size * 2];
                        int vindex = -1;
                        var index = -1;

                        int[] states = m_cityBuildingProxy.MakeCityMapEmpty();
                        int len = states.Length;

                        int vlen = 0;
                        for (int i = 0; i < len; i++)
                        {
                            if (states[i] == CityObjData.CITY_GRID_STATE_NORMAL)
                            {
                                Vector2 logicPos = SplitCityGridLogicPos(i, size);
                                Vector3 pos = ConvertMeshGirdToLocal((long) logicPos.x, (long) logicPos.y);
                                var p = m_cityBuildingContainer.TransformVector(new Vector3(pos.x, 0.01f, pos.z));

                                if (vindex < vlist.Length - 1)
                                {
                                    vlist[++vindex] = p.x;
                                    vlist[++vindex] = p.z;
                                    vlen = vlen + 2;
                                }
                            }
                        }

                        m_cityMesh.CombineMeshes(vlist, vlen);
                        m_cityMesh.Fade(true);

                        //                        m_cityMesh.GetComponent<Renderer>().material.color = Color.blue;

                        m_cityMesh.transform.localRotation = Quaternion.Euler(0f, 45f, 0f);
                    });
            }
            else
            {
                m_cityMesh.Fade(true);
            }
        }

        private void HideMesh()
        {
            if (m_cityMesh != null)
            {
                m_cityMesh.Fade(false);
            }

            SetBuildBottom(true);
        }


        private bool isLoadingBottom;

        private void SetBuildBottom(bool fb = false)
        {
            if (fb)
            {
                if (m_selectBuild != null && m_bottomBuildObj != null)
                {
                    m_bottomBuildObj.SetActive(false);
                }

                return;
            }

            if (m_bottomBuildObj == null)
            {
                if (isLoadingBottom == false)
                {
                    isLoadingBottom = true;
                    CoreUtils.assetService.Instantiate("building_move_bottom",
                        (GameObject gameObject) =>
                        {
                            isLoadingBottom = false;
                            m_bottomBuildObj = gameObject;
                            gameObject.transform.SetParent(m_cityBuildingContainer);
                            SetBuildBottomColor();
                        });
                }
            }
            else
            {
                SetBuildBottomColor();
            }
        }

        private void SetBuildBottomColor()
        {
            if (m_selectBuild != null && m_isMoving)
            {
                Renderer renderer = m_bottomBuildObj.GetComponent<Renderer>();
                Vector2 size = GetBuildCellRow((int) m_selectType);
                if (renderer)
                {
                    ChangeSpriteColor cc = m_bottomBuildObj.GetComponent<ChangeSpriteColor>();
                    if (IsGridUsed)
                    {
                        ChangeSpriteColor.SetColor(cc, Color.red);
                    }
                    else
                    {
                        ChangeSpriteColor.SetColor(cc, Color.green);
                    }

                    renderer.material.SetTextureScale("_MainTex", size);
                }

                m_bottomBuildObj.transform.localScale = new Vector3(size.x, 0.01f, size.y);

                m_bottomBuildObj.transform.localPosition = m_selectBuild.localPosition;
                m_bottomBuildObj.SetActive(true);
            }
            else
            {
                if (m_bottomBuildObj != null)
                {
                    m_bottomBuildObj.SetActive(false);
                }
            }
        }


        private bool isLoadingArrow;

        private void SetMoveArrow()
        {
            if (m_arrowObj == null)
            {
                if (isLoadingArrow == false)
                {
                    isLoadingArrow = true;
                    CoreUtils.assetService.Instantiate("building_move_arrow",
                        (GameObject gameObject) =>
                        {
                            isLoadingArrow = false;
                            m_arrowObj = gameObject;
                            gameObject.transform.SetParent(m_cityBuildingContainer);
                        });
                }
            }
            else
            {
                if (m_selectBuild != null && IsSelectBuildCanMove())
                {
                    //TODO 优化
                    var arrow1 = m_arrowObj.transform.Find("arrow1");
                    var arrow2 = m_arrowObj.transform.Find("arrow2");
                    var arrow3 = m_arrowObj.transform.Find("arrow3");
                    var arrow4 = m_arrowObj.transform.Find("arrow4");
                    Vector2 size = GetBuildCellRow((long) m_selectType);
                    float w = (size.x + 1) * 0.05f;
                    float h = (size.y + 1) * 0.05f;
                    Vector3 p1 = Vector3.zero;
                    p1.z = p1.z + w;
                    arrow1.localPosition = p1;

                    Vector3 p2 = Vector3.zero;
                    p2.x = p2.x + h;
                    arrow2.localPosition = p2;

                    Vector3 p3 = Vector3.zero;
                    p3.z = p3.z - w;
                    arrow3.localPosition = p3;

                    Vector3 p4 = Vector3.zero;
                    p4.x = p4.x - h;
                    arrow4.localPosition = p4;

                    m_arrowObj.transform.localPosition = m_selectBuild.localPosition;
                    m_arrowObj.SetActive(true);
                }
                else
                {
                    m_arrowObj.SetActive(false);
                }
            }
        }

        #endregion


        private void RefreshUpgradeBoard()
        {
            List<BuldingObjData> allList = m_cityBuildingProxy.GetAllBuldingObjData();
            allList.ForEach((buldingObjData) => {
                if (buldingObjData.buildingInfoEntity != null)
                {
                    if (m_cityBuildingProxy.ShowUpgradeBoard(buldingObjData.buildingInfoEntity.buildingIndex))
                    {
                        if (buldingObjData.gameObject != null)
                        {
                            ShowUpgradeBoard(buldingObjData.gameObject,true);
                        }
                    }
                    else
                    {
                        if (buldingObjData.gameObject != null)
                        {
                            ShowUpgradeBoard(buldingObjData.gameObject, false);
                        }
                    }
                }
           
            });
        }

        #region 着火相关

        /// <summary>
        /// 在建筑生成buff特效
        /// </summary>
        /// <param name="objData"></param>
        public void AddBuffEffect(CityObjData cityObjData)
        {
            if (cityObjData != null && cityObjData.go != null)
            {
                Transform transform = cityObjData.transform_buffEffect;
                if (transform == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.transform.SetParent(cityObjData.go.transform);
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localScale = Vector3.one;
                    transform = gameObject.transform;
                    cityObjData.transform_buffEffect = transform;
                }

                var cityEffectBuffs = m_cityBuffProxy.GetEffectBuff(cityObjData.rid);
                
                foreach (var cityEffectBuff in cityEffectBuffs)
                {
                    Transform transformCityBuff = transform.Find(cityEffectBuff.effect);
                    if (transformCityBuff == null)
                    {
                        CoreUtils.assetService.Instantiate(cityEffectBuff.effect, (go) =>
                        {
                            if (!m_cityBuffProxy.HasEffect(cityObjData.rid, cityEffectBuff.ID))
                            {
                                CoreUtils.assetService.Destroy(go);
                                return;
                            }

                            go.name = cityEffectBuff.effect;
                            go.transform.SetParent(transform);
                            go.transform.localPosition = Vector3.zero;
                        });
                    }
                }

                for (int i = 0; i < transform.childCount; i++)
                {
                    var name = transform.GetChild(i).name;
                    if (cityEffectBuffs.Find(x => x.effect.Equals(name)) == null)
                    {
                        CoreUtils.assetService.Destroy(transform.GetChild(i).gameObject);
                    }
                }
            }
        }

        public void SetCityFireState(CityObjData cityObjData)
        {
            MapViewLevel crrLevel = m_globalViewLevelMediator.GetViewLevel();
            MapViewLevel preLevel = m_globalViewLevelMediator.GetPreMapViewLevel();
            if (crrLevel == MapViewLevel.City)
            {
                SetCityFireState(cityObjData, false);
            }
            else if (crrLevel == MapViewLevel.Tactical)
            {
                SetCityFireState(cityObjData, true);
            }
        }

        /// <summary>
        /// 显示城市上方的火焰
        /// </summary>
        /// <param name="cityObjData"></param>
        public void SetCityFireState(CityObjData cityObjData, bool show)
        {
            //  Debug.LogErrorFormat("{0}{1}{2}", cityObjData.mapObjectExtEntity.cityRid, show, cityObjData.fireState);
            bool ToFire = false;
            bool ToNone = false;
            if (cityObjData.go == null || cityObjData.mapObjectExtEntity == null)
            {
                return;
            }
            if (cityObjData.fireState == FireState.FIRED)
            {
                Transform transform = cityObjData.transform_effect;
                if (transform == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.transform.SetParent(cityObjData.go.transform);
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localScale = Vector3.one;
                    transform = gameObject.transform;
                    cityObjData.transform_effect = transform;
                }
                if (cityObjData.fireobjState == FireState.NONE && !cityObjData.fireLoading)
                {
                    cityObjData.fireLoading = true;
                    CoreUtils.assetService.Instantiate(RS.FireName[4], (go) =>
                    {
                        go.name = "Fire";
                        go.transform.SetParent(transform);
                        go.transform.localPosition = Vector3.zero;
                        if (show)
                        {
                            go.SetActive(true);
                        }
                        else
                        {
                            go.SetActive(false);
                        }
                        cityObjData.fireLoading = false;
                        cityObjData.fireobjState = FireState.FIRED;
                        if (cityObjData.fireState == FireState.NONE)
                        {
                            CoreUtils.assetService.Destroy(go);
                            cityObjData.fireobjState = FireState.NONE;
                        }
                    });
                }
                else
                {
                    Transform transformFire = transform.Find("Fire");
                    if (transformFire != null)
                    {
                        if (show)
                        {
                            transformFire.gameObject.SetActive(true);
                        }
                        else
                        {
                            transformFire.gameObject.SetActive(false);
                        }
                    }
                }
            }
            else if (cityObjData.fireState == FireState.NONE)
            {
                if (cityObjData.fireobjState == FireState.FIRED)
                {
                    Transform transform = cityObjData.transform_effect;
                    if (transform != null)
                    {
                        Transform transformFire = transform.Find("Fire");
                        if (transformFire != null)
                        {
                            cityObjData.fireobjState = FireState.NONE;
                            ToNone = true;
                            CoreUtils.assetService.Destroy(transformFire.gameObject);
                        }
                    }
                }
            }

            if (show)
            {
                if (cityObjData.mapObjectExtEntity.cityRid != m_playerProxy.CurrentRoleInfo.rid)
                {
                    List<BuldingObjData> allList = cityObjData.buildingList.Values.ToList();
                    allList.ForEach((buldingObj) =>
                    {
                        if (buldingObj.type != EnumCityBuildingType.CityWall &&
                            buldingObj.type != EnumCityBuildingType.GuardTower)
                        {
                            buldingObj.SetfireState(FireState.NONE);
                            buldingObj.UpdateFireStateShow();
                        }
                    });
                }
                else
                {
                    List<BuldingObjData> allList = cityObjData.buildingList.Values.ToList();
                    allList.ForEach((buldingObj) =>
                    {
                        if (buldingObj.type == EnumCityBuildingType.TownCenter)
                        {
                            buldingObj.t_effects.ForEach((trans) =>
                            {
                                Transform fire = trans.Find("Fire");
                                if (fire != null)
                                {
                                    fire.gameObject.SetActive(false);
                                }
                            });
                        }
                    });
                }
            }
            else if (!show)
            {
                if (cityObjData.mapObjectExtEntity.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.CityFireStateChange);
                }
                if (cityObjData.fireState == FireState.FIRED)
                {
                    if (cityObjData.mapObjectExtEntity.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                    {
                        SetFireState();
                    }
                    else
                    {
                        List<BuldingObjData> allList = cityObjData.buildingList.Values.ToList();
                        allList.ForEach((buldingObj) =>
                        {
                            if (buldingObj.type == EnumCityBuildingType.CityWall)
                            {
                                buldingObj.SetfireState(FireState.FIRED);
                                buldingObj.UpdateFireStateShow();
                            }
                        });
                    }
                }
                else if (cityObjData.fireState == FireState.NONE)
                {
                    if (cityObjData.mapObjectExtEntity.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                    {
                        if (ToNone)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.CityFireStateChange);
                            SetFireState();
                        }
                    }
                    else
                    {
                        Transform transform = cityObjData.transform_effect;
                        if (transform != null)
                        {
                            Transform transformFire = transform.Find("Fire");
                            if (transformFire != null)
                            {
                                cityObjData.fireobjState = FireState.NONE;
                                CoreUtils.assetService.Destroy(transformFire.gameObject);
                            }
                        }

                        List<BuldingObjData> allList = cityObjData.buildingList.Values.ToList();
                        allList.ForEach((buldingObj) =>
                        {
                            buldingObj.SetfireState(FireState.NONE);
                            buldingObj.UpdateFireStateShow();
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 移动其他人的城市
        /// </summary>
        /// <param name="rid"></param>
        public void MoveOtherCity(CityObjData cityObjData)
        {
            if (cityObjData != null)
            {
                float x = cityObjData.mapObjectExtEntity.objectPos.x / 100f;
                float z = cityObjData.mapObjectExtEntity.objectPos.y / 100f;
                if (cityObjData.go != null)
                {
                    if ((cityObjData.go.transform.position.x) !=
                        cityObjData.mapObjectExtEntity.objectPos.x / 100f &&
                        (cityObjData.go.transform.position.y) !=
                        cityObjData.mapObjectExtEntity.objectPos.y / 100f)
                    {
                        cityObjData.go.transform.localPosition =
                            new Vector3(cityObjData.mapObjectExtEntity.objectPos.x / 100f, 0,
                                cityObjData.mapObjectExtEntity.objectPos.y / 100f);
                        cityObjData.StopBuilder(true);
                    }

                    if (m_globalViewLevelMediator.IsLodVisable(x, z))
                    {
                        if (WorldCamera.Instance().IsAutoMoving() || WorldCamera.Instance().isMovingToPos || !m_showDownAnim ||
                            Math.Abs(cityObjData.mapObjectExtEntity.cityPosTime -
                                     ServerTimeModule.Instance.GetServerTime()) > 5)
                        {
                            //Debug.LogErrorFormat(
                            //    "IsAutoMoving{0}m_showDownAnim{1}cityPosTime{2}GetServerTime{3}timespan{4}",
                            //    WorldCamera.Instance().IsAutoMoving(), m_showDownAnim,
                            //    cityObjData.mapObjectExtEntity.cityPosTime, ServerTimeModule.Instance.GetServerTime(),
                            //    cityObjData.mapObjectExtEntity.cityPosTime - ServerTimeModule.Instance.GetServerTime());
                        }
                        else
                        {
                            cityObjData.go.SetActive(false);
                            CoreUtils.assetService.Instantiate(RS.CityDown,
                                (go) => {
                                    go.transform.position = cityObjData.go.transform.position;
                                    CoreUtils.audioService.PlayOneShot("Sound_operation_2019");
                                });
                            Timer.Register(3, () =>
                            {
                                if (cityObjData.go != null)
                                {
                                    cityObjData.go.SetActive(true);
                                }
                            });
                        }
                    }
                }
                else
                {
                    if (m_globalViewLevelMediator.IsCityInViewPortTactical(x, z,
                        cityObjData.mapObjectExtEntity.cityName))
                    {
                        AppFacade.GetInstance()
                            .SendNotification(CmdConstant.LoadMapObj, cityObjData.mapObjectExtEntity);
                    }
                }
            }
        }

        /// <summary>
        /// 在其他人的城市建筑上生成火焰
        /// </summary>
        /// <param name="rid"></param>
        public void FireOtherCityBuilding(CityObjData cityObjData)
        {
            if (cityObjData != null)
            {
                if (cityObjData.go != null)
                {
                    float x = cityObjData.mapObjectExtEntity.objectPos.x / 100f;
                    float z = cityObjData.mapObjectExtEntity.objectPos.y / 100f;
                    if (m_globalViewLevelMediator.IsCityInViewPortTactical(x, z,
                        cityObjData.mapObjectExtEntity.cityName))
                    {
                        List<BuldingObjData> allList = cityObjData.buildingList.Values.ToList();
                        if (cityObjData.mapObjectExtEntity.beginBurnTime != 0)
                        {
                            if (m_globalViewLevelMediator.GetViewLevel() == MapViewLevel.City)
                            {
                                m_cityBuildingProxy.SetFireState(allList, true);
                            }
                            else
                            {
                                m_cityBuildingProxy.SetFireState(allList, false);
                            }
                            allList.ForEach((buldingObj) => { buldingObj.UpdateFireStateShow(); });
                        }
                        else
                        {
                            allList.ForEach((buldingObj) =>
                            {
                                buldingObj.SetfireState(FireState.NONE);
                                buldingObj.UpdateFireStateShow();
                            });
                        }
                    }
                }
            }
        }

        public void SetFireState()
        {
            string key_firestate = string.Format("{0}_{1}", "Firestate", (int) m_playerProxy.CurrentRoleInfo.rid);
            string key_burnTime = string.Format("{0}_{1}", "BurnTime", (int) m_playerProxy.CurrentRoleInfo.rid);
            int value_firestate = PlayerPrefs.GetInt(key_firestate);
            int value_burnTime = PlayerPrefs.GetInt(key_burnTime);
            //    Debug.LogErrorFormat("value_firestate{0}value_burnTime{1}", value_firestate, value_burnTime);
            List<BuldingObjData> allList = m_cityBuildingProxy.GetAllBuldingObjData();
            if (m_cityBuildingProxy.MyCityObjData.mapObjectExtEntity == null)
            {
                return;
            }

            if (m_cityBuildingProxy.MyCityObjData.fireState == FireState.FIRED)
            {
                if (value_firestate == 0 || (value_firestate == 1 && value_burnTime <
                                             (int) m_cityBuildingProxy.MyCityObjData.mapObjectExtEntity.beginBurnTime))
                {
                    PlayerPrefs.SetInt(key_burnTime,
                        (int) m_cityBuildingProxy.MyCityObjData.mapObjectExtEntity.beginBurnTime);
                    Timer.Register(0, () => { PlayerPrefs.SetInt(key_firestate, 1); });
                    m_cityBuildingProxy.SetFireState(allList);
                    allList.ForEach((buldingObj) => { buldingObj.UpdateFireStateShow(); });
                    CreateRunCitizen();
                }
                else
                {
                    allList.ForEach((buldingObj) =>
                    {
                        if (buldingObj.type == EnumCityBuildingType.CityWall)
                        {
                            buldingObj.SetfireState(FireState.FIRED);
                            buldingObj.UpdateFireStateShow();
                        } 
                    });
                }
            }
            else if (m_cityBuildingProxy.MyCityObjData.fireState == FireState.NONE)
            {
                allList.ForEach((buldingObj) => { buldingObj.DestroyFireObj(); });
                PlayerPrefs.SetInt(key_firestate, 0);
            }
        }

        #endregion

        /// <summary>
        /// 显示隐藏建造下的所有升级标志
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Show"></param>
        private bool ShowUpgradeBoard(GameObject obj, bool Show)
        {
            if (obj != null)
            {
                if (Show)
                {
                    Transform transform = obj.transform.Find("UpgradeBoard");
                    if (transform != null && !transform.gameObject.activeSelf)
                    {
                        transform.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Transform transform = obj.transform.Find("UpgradeBoard");
                    if (transform != null && transform.gameObject.activeSelf)
                    {

                        if (m_selectBuild!=null && transform == m_selectBuild)
                        {
                            m_revBuildUpgradeBorard = false;
                        }
                        
                        transform.gameObject.SetActive(false);
                        return true;
                    }
                }
            }

            return false;
        }


        #region 城墙和城内兵相关

        private void StartBuilder(bool needCreate = true)
        {
            if (m_cityMapFinder == null || m_cityWallPatrolSoldierDummy == null)
            {
                return;
            }

            if (m_workers.Count > 0)
            {
                foreach (var worker in m_workers)
                {
                    worker.gameObject.SetActive(true);
                }

                foreach (var soldier in m_soldiers)
                {
                    soldier.gameObject.SetActive(true);
                }

                foreach (var train in m_trains)
                {
                    train.Value.gameObject.SetActive(true);
                }
            }

            if (m_workers.Count == 0 && m_initedBuilders == false && needCreate)
            {
                m_initedBuilders = true;

                CreateWallSoldiers();

                CreateTrainSoldiers();

                CreateCityWorker();

                if (m_debug)
                {
                    CoreUtils.logService.Info("创建工人", Color.green);
                    PrintMap("创建者城市工作者");

                    //m_cityMapFinder.Draw(m_GridState, null, new List<Vector2>());
                }
            }

            StartOtherBuilder();
        }

        private void clearBuilder()
        {
            if (m_workers.Count > 0)
            {
                foreach (var worker in m_workers)
                {
                    CoreUtils.assetService.Destroy(worker.gameObject);
                }

                m_workers.Clear();

                foreach (var soldier in m_soldiers)
                {
                    CoreUtils.assetService.Destroy(soldier.gameObject);
                }

                m_soldiers.Clear();

                foreach (var train in m_trains)
                {
                    CoreUtils.assetService.Destroy(train.Value.gameObject);
                }

                m_trains.Clear();
                m_initedBuilders = false;
            }
        }


        private void StopBuilder()
        {
            if (m_workers.Count > 0)
            {
                foreach (var worker in m_workers)
                {
                    worker.gameObject.SetActive(false);
                }

                foreach (var soldier in m_soldiers)
                {
                    soldier.gameObject.SetActive(false);
                }

                foreach (var train in m_trains)
                {
                    train.Value.gameObject.SetActive(false);
                }
            }
            StopOtherBuilder();
        }
        /// <summary>
        /// 隐藏城内建筑细节
        /// </summary>
        private void HideBuilddetail()
        {
            m_cityBuildingProxy.GetAllBuldingObjData().ForEach((buidlObjData) =>
            {
                if (buidlObjData.type > EnumCityBuildingType.Road)
                {
                    if (buidlObjData.gameObject != null)
                    {
                        buidlObjData.gameObject.SetActive(false);
                    }
                }

                if (buidlObjData.transform_shelf != null)
                {
                    buidlObjData.transform_shelf.gameObject.SetActive(false);
                }
            });
        }

        /// <summary>
        /// 显示城内建筑细节
        /// </summary>
        private void ShowBuildDetail()
        {
            m_cityBuildingProxy.GetAllBuldingObjData().ForEach((buidlObjData) =>
            {
                if (buidlObjData.type > EnumCityBuildingType.Road)
                {
                    if (buidlObjData.gameObject != null)
                    {
                        buidlObjData.gameObject.SetActive(true);
                    }
                }

                if (buidlObjData.transform_shelf != null)
                {
                    buidlObjData.transform_shelf.gameObject.SetActive(true);
                }

            });
        }

        private void CreateWallSoldiers()
        {
            for (int i = 1; i <= PARTOL_SOLDIER_COUNT; i++)
            {
                Vector2[] sp = null;
                var p = Vector3.zero;
                if (m_cityWallPatrolSoldierDummy != null)
                {
                    sp = m_cityWallPatrolSoldierDummy.GetSoldierPath(i);
                    p = m_cityWallPatrolSoldierDummy.GetSoldierPos(i);
                }
                var index = i;
                CoreUtils.assetService.Instantiate("citizen",
                    (GameObject gameObject) =>
                    {
                        gameObject.transform.SetParent(m_cityBuildingContainer, false);
                        gameObject.transform.localPosition = p;
                        gameObject.name = "patrol_" + index;

                        var citizen = gameObject.GetComponent<People>();
                        string namepefab = m_trainProxy.GetMaxSoldier();
                        CoreUtils.assetService.Instantiate(namepefab,
                            (GameObject uint_obj) =>
                            {
                                if (gameObject == null)
                                {
                                    Debug.LogError("找不到城墙上的兵" + namepefab);
                                }

                                People.InitCitizenS(citizen, uint_obj, RS.blue_troop);
                                People.SetUnitFootprintsActiveS(citizen, false);
                                People.ResetUnitPosS(citizen);
                                if (sp[0] != null)
                                {
                                    var startPos = MakeWorldPosFormLocal(sp[0].x, sp[0].y);
                                    var endPos = MakeWorldPosFormLocal(sp[1].x, sp[1].y);

                                    citizen.WorldPaths.Add(startPos);
                                    citizen.WorldPaths.Add(endPos);
                                    var fsm = gameObject.AddComponent<PeopleFSM>();
                                    fsm.Owner = citizen;

                                    m_soldiers.Add(fsm);
                                }
                            });
                    });
            }
        }


        private void CreateCityWorker()
        {
            CityAgeSizeDefine cityAgeSizeDefine = m_cityBuildingProxy.GetCityAgeSizeDefine();

            int builderCount = cityAgeSizeDefine.BuilderFemaleNum + cityAgeSizeDefine.BuilderMaleNum;


            m_cityMapFinder.SetFinderMap(m_GridState, m_cityBuildingProxy.GetCityAgeSize(), m_cityBuildingContainer);

            if (builderCount == m_workers.Count)
            {
                return;
            }
            
            var config = CoreUtils.dataService.QueryRecords<ConfigDefine>()[0];

            for (int i = 0; i < builderCount; i++)
            {
                var index = i;

                CoreUtils.assetService.Instantiate("citizen",
                    (GameObject gameObject) =>
                    {
                        //                        Vector2 slocal = m_cityMapFinder.GetRandPos();//GetRandPosLogic();
                        //                        Vector2 elocal = m_cityMapFinder.GetRandPos();//GetRandPosLogic();
                        gameObject.transform.SetParent(m_cityBuildingContainer, false);
                        gameObject.name = "builder_" + index;

                        //                        gameObject.transform.localPosition = ConvertCityObjTileToLocal(slocal.x, slocal.y);

                        var citizen = gameObject.GetComponent<People>();
                        string name = config.builderMaleModel;

                        if (index >= cityAgeSizeDefine.BuilderMaleNum)
                        {
                            name = config.builderFemaleModel;
                        }

                        CoreUtils.assetService.Instantiate(name,
                            (GameObject uint_obj) =>
                            {
                                People.InitCitizenS(citizen, uint_obj, Color.white);
                                People.SetUnitFootprintsActiveS(citizen, true);
                                People.ResetUnitPosS(citizen);

                                var fms = gameObject.AddComponent<WorkerFMS>();

                                fms.Finder = m_cityMapFinder;
                                fms.Owner = citizen;
                                fms.name = gameObject.name;


                                fms.index = index;


                                fms.SetBorn(m_cityMapFinder.GetRandPos());
                                fms.WalkAround();

                                m_workers.Add(fms);

                                //                                fms.WorldPaths = m_cityMapFinder.GetFindWorldPaths( slocal, elocal);

                                if (m_workers.Count == builderCount)
                                {
                                    Timer.Register(0.1f, () => { WorkAssign(); });
                                }
                            });
                    });
            }
        }


        public void CreateTrainSoldiers()
        {
            var builds = m_cityBuildingProxy.GetTrainBuilds();

            foreach (var build in builds)
            {
                var posArr = m_trainSoldierPos[build.type - 9];
                var dirArr = m_trainSoldierDirPos[build.type - 9];
                for (int i = 0; i < posArr.Length; i++)
                {
                    string sname = string.Format("TrainSoldiers_{0}_{1}", build.type, i);

                    if (!m_trains.ContainsKey(sname))
                    {
                        Vector2 off = posArr[i];
                        Vector2 dir = dirArr[i];
                        BuildingInfoEntity tempBuild = build;
                        long buildType = build.type;
                        long buildIndex = build.buildingIndex;
                        int index = i;
                        CoreUtils.assetService.Instantiate("citizen",
                            (GameObject gameObject) =>
                            {
                                if (!m_trains.ContainsKey(sname))
                                {
                                    gameObject.transform.SetParent(m_cityBuildingContainer, false);
                                    gameObject.name = sname;

                                    var citizen = gameObject.GetComponent<People>();

                                    var arm = m_trainProxy.GetMaxUnlockSoldier(tempBuild.buildingIndex);
                                    if (arm == null)
                                    {
                                        CoreUtils.assetService.Destroy(gameObject);
                                        return;
                                    }
                                    if (string.IsNullOrEmpty(arm.armsModel))
                                    {
                                        Debug.Log("兵种模型没有配置@刘晨" + arm.armsShow);
                                    }
                           
                                        CoreUtils.assetService.Instantiate(arm.armsModel,
                                        (GameObject uint_obj) =>
                                        {
                                            if (!m_trains.ContainsKey(sname))
                                            {
                                                People.InitCitizenS(citizen, uint_obj, RS.blue_troop);
                                                People.SetUnitFootprintsActiveS(citizen, false);
                                                People.ResetUnitPosS(citizen);
                                                AnimationBase component = uint_obj.GetComponent<AnimationBase>();
                                                if (component != null)
                                                {
                                                    component.ReleaseParticle();
                                                }
                                                var fms = gameObject.AddComponent<TrainSoldiersFMS>();

                                                fms.Owner = citizen;
                                                fms.name = sname;
                                                fms.Finder = m_cityMapFinder;

                                                fms.buildType = buildType;
                                                fms.buildIndex = buildIndex;
                                                fms.index = index;

                                                fms.WaitFor(new Vector2(tempBuild.pos.x, tempBuild.pos.y), off, dir);
                                                var status = m_cityBuildingProxy.GetTrainStatus(tempBuild.buildingIndex);
                                                if (status == SoldierTrainStatus.Training)
                                                {
                                                    fms.Train();
                                                }
                                                m_trains.Add(sname, fms);
                                            }
                                        });
                                }
                            });
                    }
                }
            }
        }

        private void RemoveTrainSoldier(long buildID)
        {
            if (m_trains.Count > 0)
            {
                List<string> remove = new List<string>();
                foreach (var train in m_trains)
                {
                    if (train.Value.buildIndex == buildID)
                    {
                        CoreUtils.assetService.Destroy(train.Value.gameObject);

                        remove.Add(train.Key);
                    }
                }

                foreach (var train in remove)
                {
                    m_trains.Remove(train);
                }
            }
        }

        //训练兵种归为
        private void TrainSoldiersFindHome(long buildID, int posx, int posy)
        {
            foreach (var train in m_trains)
            {
                if (train.Value.buildIndex == buildID)
                {
                    train.Value.FindNewHomePos(new Vector2(posx, posy));
                }
            }
        }

        //建筑移动的时候重新设置工人
        public void WorkResetOnBuildMove()
        {
        }

        //分配建筑工人从灭火接收功能过来
        public void WorkAssign(bool formfireFighting = false)
        {
            if (m_workers.Count == 0)
            {
                return;
            }
            if (timeRunCitizen != null || timeFIREFIGHTING != null)
            {
                return;
            }
            Dictionary<long, QueueInfo> buildqQueueInfos = m_playerProxy.CurrentRoleInfo.buildQueue;
            int buildingCount = 0;
            int workerCount = m_workers.Count;
            List<long> buildIDs = new List<long>();
            foreach (var info in buildqQueueInfos)
            {
                QueueInfo queueInfo = info.Value;
                if (queueInfo.finishTime > 0)
                {
                    var stInfo = m_cityBuildingProxy.GetBuildingInfoByindex(queueInfo.buildingIndex);
                    if (stInfo == null || (stInfo.type == (long) EnumCityBuildingType.CityWall ||
                                           stInfo.type == (long) EnumCityBuildingType.GuardTower ||
                                           IsTree((EnumCityBuildingType) stInfo.type)
                        ))
                    {
                        continue;
                    }

                    buildIDs.Add(queueInfo.buildingIndex);
                }

                if (m_debug)
                {
                    Debug.Log(queueInfo.finishTime + " 分配建筑工人 " + queueInfo.beginTime + "  " + queueInfo.buildingIndex +
                              "  " + ServerTimeModule.Instance.GetServerTime());
                }
            }

            if (buildIDs.Count > 0)
            {
                int split = workerCount / buildIDs.Count;

                for (int i = 0; i < buildIDs.Count; i++)
                {
                    WorkStartBuilding(buildIDs[i], split * i, split * i + split);
                }
            }


            if (buildIDs.Count == 0)
            {
                if (!formfireFighting)
                {
                    WorkStopBuilding();
                }
                else
                {
                    WorkStopFireFighting();
                }
            }
        }


        //建筑工人开始去建筑边工作
        public void WorkStartBuilding(long buildID, int start, int end)
        {
            List<Vector2> paths = GetBuildingFinderPosition(buildID);

            var etInfo = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.Storage);
            var stInfo = m_cityBuildingProxy.GetBuildingInfoByindex(buildID);


            List<Vector2> stPaths = GetBuildingFinderPosition(etInfo.buildingIndex);

            //Debug.Log("分配建筑工人 开始去建筑了"+buildID+"  "+start+"  "+end);

            var buildRes = m_cityBuildingProxy.BuildingLevelDataBylevel(stInfo.type, stInfo.level + 1);

            List<People.ENMU_CARRY_RESOURCE_TYPE> resList = new List<People.ENMU_CARRY_RESOURCE_TYPE>(3);

            //TODO:修复建筑数据为空的问题
            if (buildRes == null)
            {
                return;
            }

            if (buildRes.food > 0) 
            {
                resList.Add(People.ENMU_CARRY_RESOURCE_TYPE.FOOD);
            }
            if (buildRes.wood > 0) 
            {
                resList.Add(People.ENMU_CARRY_RESOURCE_TYPE.WOOD);
            }
            if (buildRes.stone > 0) 
            {
                resList.Add(People.ENMU_CARRY_RESOURCE_TYPE.STONE);
            }
            if (buildRes.coin > 0) 
            {
                resList.Add(People.ENMU_CARRY_RESOURCE_TYPE.GOLD);
            }
            foreach (var m_work in m_workers)
            {
                if (m_work.index >= start && m_work.index < end)
                {
                    //Debug.Log(m_work.index+" 建筑 派遣 分配建筑工人 "+buildID);
                    if (m_work.index % 2 == 0)
                    {
                        if (!m_work.IsWorkForBuild(buildID))
                        {
                            m_work.GoOnAroundBuild(paths, buildID);
                        }
                    }
                    else
                    {
                        if (!m_work.IsWorkForBuild(buildID))
                        {
                            m_work.GoGetResToBuild(stPaths, paths, buildID, etInfo.buildingIndex, resList);
                        }
                    }
                }
            }
        }

        public void WorkStopBuilding()
        {
            foreach (var m_work in m_workers)
            {
                if (m_debug)
                {
                    Debug.Log(m_work.index + " 建筑工人" + m_work.buildIndex);
                }

                if (m_work.IsWorking())
                {
                    m_work.GoOnWalkAround();
                }
            }
        }
        public void WorkStopFireFighting()
        {
            foreach (var m_work in m_workers)
            {
                if (m_debug)
                {
                    Debug.Log(m_work.index + " 建筑工人" + m_work.buildIndex);
                }

                if (!m_work.IsWorking())
                {
                    m_work.GoOnWalkAround();
                }
            }
        }
        /*
        /// <summary>
        /// 士兵逃跑
        /// </summary>
        public void CreateRunCitizen()
        {
            m_cityBuildingProxy.MyCityObjData.CreateRunCitizen();
        }
        public void CreateFIREFIGHTINGCitizen()
        {
            m_cityBuildingProxy.MyCityObjData.CreateFIREFIGHTINGCitizen();
        }
       */

        /// <summary>
        /// 士兵逃跑
        /// </summary>
        public void CreateRunCitizen()
        {
            var etInfo = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.TownCenter);
            List<Vector2> stPaths = GetBuildingFinderPosition(etInfo.buildingIndex);
            if (timeFIREFIGHTING != null)
            {
                timeFIREFIGHTING.Cancel();
                timeFIREFIGHTING = null;
            }
            foreach (var work in m_workers)
            {
                work.RunToCitizen(stPaths[5]);
            }
            timeRunCitizen = Timer.Register(1, () =>
            {
                bool home = true;//所有的工人都躲进市政厅了
                foreach (var m_work in m_workers)
                {
                    if (!m_work.IsEndStep())
                    {
                        home = false;
                    }
                }
                if (home)
                {
                    if (timeRunCitizen != null)
                    {
                        timeRunCitizen.Cancel();
                        timeRunCitizen = null;
                        CreateFIREFIGHTINGCitizen();
                    }
                }
            }, null,true, true);
        }

        bool home = false;//所有的工人都躲进市政厅了
        Timer timeFIREFIGHTING;//救火
        Timer timeRunCitizen;//逃跑
        /// <summary>
        /// 士兵救火
        /// </summary>
        public void CreateFIREFIGHTINGCitizen()
        {
            List<BuldingObjData> allList = m_cityBuildingProxy.GetAllBuldingObjData();
            if (m_cityBuildingProxy.MyCityObjData.mapObjectExtEntity == null|| m_cityBuildingProxy.MyCityObjData == null)
            {
                return;
            }
            var resBuildInfo = m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.TownCenter);

            timeFIREFIGHTING = Timer.Register(1, () =>
            {
                if (resBuildInfo == null)
                {
                    return;
                }
                List<Vector2> stPaths = GetBuildingFinderPosition(resBuildInfo.buildingIndex);
                foreach (var m_work in m_workers)
               {
                   //if (m_work.index != 0)
                   //{
                   //    continue;
                   //}
                   if (!m_work.IsWorking())
                   {
                       for (int i = 0; i < allList.Count; i++)
                       {
                           BuldingObjData buldingObj = allList[i];
                           if (buldingObj.type == EnumCityBuildingType.CityWall || buldingObj.type == EnumCityBuildingType.GuardTower)
                           {
                               continue;
                           }
                           if (buldingObj.fireState == FireState.FIRED && buldingObj.fireObjState == FireState.FIRED)
                           {
                               if (buldingObj.workerNum == 5)
                               {
                                   continue;
                               }
                               buldingObj.workerNum++;
                               long buildID = buldingObj.buildingInfoEntity.buildingIndex;
                               List<Vector2> paths = GetBuildingFinderPosition(buildID);
                               m_work.gameObject.SetActive(true);

                             //  var workBuildInfo = m_cityBuildingProxy.GetBuildingInfoByindex(buildID);

                               List<People.ENMU_CARRY_RESOURCE_TYPE> resList = new List<People.ENMU_CARRY_RESOURCE_TYPE>();

                               resList.Add(People.ENMU_CARRY_RESOURCE_TYPE.BUCKET);

                               Debug.Log(m_work.index + " 建筑 派遣 分配建筑工人去灭火 " + buildID + "paths.Count)" + paths.Count);

                               m_work.GoGetWaterToFireBuild(stPaths, paths, buildID, resBuildInfo.buildingIndex, resList, FireCountCallBack);
                               break;
                           }
                       }
                   }
               }
               bool alldone = true;
               allList.ForEach((temp) => {
                   if (temp.fireObjState == FireState.FIRED && temp.type != EnumCityBuildingType.CityWall)
                   {
                       alldone = false;
                   }
               });
               if (alldone)
               {
               //    Debug.LogError("灭火完成");
                   if (timeFIREFIGHTING != null)
                   {
                       timeFIREFIGHTING.Cancel();
                       timeFIREFIGHTING = null;
                   }
                   WorkAssign(true);
               }
           }, null, true, true);
        }

        private void FireCountCallBack(long buildID, long workIndex)
        {
            BuldingObjData buldingObjData = m_cityBuildingProxy.GetBuldingObjDataByIndex(buildID);
            if (buldingObjData == null)
            {
                return;
            }

            m_workers[(int)workIndex].Clear();
            buldingObjData.fireFightingNum--;
            if (buldingObjData.fireFightingNum <= 0)
            {
                buldingObjData.fireFightingNum = 0;
                buldingObjData.SetfireState(FireState.NONE);
            }
            buldingObjData.UpdateFireStateShow();

          //  Debug.Log(workIndex + "灭火次数回调了 " + buildID + " " + buldingObjData.fireFightingNum);
        }

        private bool BuildIsResOrStorage(EnumCityBuildingType type)
        {
            if (type >= EnumCityBuildingType.Farm && type <= EnumCityBuildingType.SilverMine ||
                type == EnumCityBuildingType.Storage)
            {
                return true;
            }

            return false;
        }


        private bool IsTree(EnumCityBuildingType type)
        {
            if (type >= EnumCityBuildingType.tree && type <= EnumCityBuildingType.tree4)
            {
                return true;
            }

            return false;
        }


        private void BuildMovedDownBefore(EnumCityBuildingType type, long buildIndex, long x, long y)
        {
            if (IsTree(type))
            {
                return;
            }

            if (m_cityBuildingProxy.IsTranBuild((long) type))
            {
                TrainSoldiersFindHome(m_selectBuildID, (int) x, (int) y);
            }

            if (m_workers != null)
            {
                List<Vector2> buildOutSide = GetBuildingFinderPosition(buildIndex);
                var stInfo = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.Storage);
                var stInfo1 = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.TownCenter);
                List<Vector2> storageBuildOutSide = GetBuildingFinderPosition(stInfo.buildingIndex);

                foreach (var worker in m_workers)
                {
                    if (worker.IsWorkResBuildMove(stInfo.buildingIndex, buildIndex))
                    {
//                        Debug.Log(worker.index+"分配建筑工人  "+worker.ResBuildIndex+"  "+stInfo.buildingIndex+"  "+buildIndex);
                        worker.GoGetResToBuild(storageBuildOutSide,
                            type == EnumCityBuildingType.Storage ? null : buildOutSide, buildIndex,
                            stInfo.buildingIndex, null);
                    }
                    else if (worker.IsWorkFireBuildMove(stInfo1.buildingIndex, buildIndex))
                    {
                                                Debug.Log(worker.index+"分配建筑工人  "+worker.ResBuildIndex+"  "+stInfo.buildingIndex+"  "+buildIndex);
                     
                    }
                    else if (worker.IsWorkForBuild(buildIndex))
                    {
//                        Debug.Log(worker.index+"分配建筑工人*  "+ worker.buildIndex +"   "+buildIndex+"  "+buildIndex);
                        worker.GoOnAroundBuild(buildOutSide, buildIndex);
                    }
                    else
                    {
                    }
                }
            }
        }


        private void BuildMovedDownAfter(EnumCityBuildingType type, long buildIndex)
        {
            if (m_selectType == EnumCityBuildingType.TownCenter)
            {
                if (m_selectBuild != null)
                {
                    var tileCollideItem = m_selectBuild.GetComponent<GridCollideItem>();
                    GridCollideItem.ResetInitLocalPosS(tileCollideItem);
                }
            }

            if (m_workers != null)
            {
                foreach (var worker in m_workers)
                {
                    worker.CheckWaklAroundEndPoint();
                }
            }
        }

        #endregion

        #region 位置坐标系相关

        public List<Vector2> GetBuildingFinderPosition(long buildIndex)
        {
            List<Vector2> poss = new List<Vector2>();
            var buildEntiy = m_cityBuildingProxy.GetBuildingInfoByindex(buildIndex);

            var buildConfig = m_cityBuildingProxy.GetBuildConfig((EnumCityBuildingType) buildEntiy.type);

            Vector2 p = new Vector2(buildEntiy.pos.x, buildEntiy.pos.y);

            int w = buildConfig.width;
            int l = buildConfig.length;


            int posX = (int) buildEntiy.pos.x - 1;
            int posY = (int) buildEntiy.pos.y - 1;

            int posXExt = posX + w;
            int posYExt = posY + l;


            for (int x = posX; x < posXExt; x++)
            {
                if (CheckPosInGird(x, posY))
                {
                    poss.Add(new Vector2(x, posY));
                }
            }

            for (int y = posY; y < posYExt; y++)
            {
                if (CheckPosInGird(posXExt, y))
                {
                    poss.Add(new Vector2(posXExt, y));
                }
            }

            for (int x = posXExt; x > posX; x--)
            {
                if (CheckPosInGird(x, posYExt))
                {
                    poss.Add(new Vector2(x, posYExt));
                }
            }

            for (int y = posYExt; y > posY; y--)
            {
                if (CheckPosInGird(posX, y))
                {
                    poss.Add(new Vector2(posX, y));
                }
            }

            //            Debug.Log(posX+" 建筑周围点 "+posY+"  "+posXExt+"   "+posYExt+poss[0]);

            //            foreach (var pp in poss)
            //            {
            //                Debug.Log(pp);
            //            }

            return poss;
        }


        private int[][] offBuildPos = new[]
        {
            new[] {1, 1},
            new[] {-1, 1},
            new[] {-1, -1},
            new[] {1, -1}
        };

        public Vector2 GetRandBuildLogic(int w, int len)
        {
            //从中间到外面创建
            var size = m_cityBuildingProxy.GetCitySize();
            int hsize = size / 2;

            Vector2 buildsize = new Vector2(w, len);

            int x = 0;
            int y = 0;


//            List<Vector2> temp = new List<Vector2>();
            for (int i = 1; i < hsize; i++)
            {
                for (int j = 0; j < offBuildPos.Length; j++)
                {
                    var sx = offBuildPos[j][0] * i;
                    var sy = offBuildPos[j][1] * i;

                    var tt = j + 1;
                    if (tt >= 4)
                    {
                        tt = 0;
                    }

                    var ex = offBuildPos[tt][0] * i;
                    var ey = offBuildPos[tt][1] * i;

                    var minsx = Mathf.Min(sx, ex);
                    var maxex = Mathf.Max(sx, ex);

                    var minsy = Mathf.Min(sy, ey);
                    var miney = Mathf.Max(sy, ey);

                    for (int xx = minsx; xx < maxex; xx++)
                    {
//                        Debug.Log(xx+"  "+ey);
//                        temp.Add(new Vector2(xx,ey));
                        if (CheckGridIsUeded(xx, ey, buildsize) == false)
                        {
//                            m_cityMapFinder.Draw(m_GridState,null,temp);
                            return new Vector2(xx,
                                ey);
                        }
                    }

                    for (int yy = minsy; yy < miney; yy++)
                    {
//                        Debug.Log(ex+"  "+yy);
//                        temp.Add(new Vector2(ex,yy));
                        if (CheckGridIsUeded(ex, yy, buildsize) == false)
                        {
//                            m_cityMapFinder.Draw(m_GridState,null,temp);
                            return new Vector2(ex,
                                yy);
                        }
                    }
                }
            }

//            m_cityMapFinder.Draw(m_GridState,null,temp);

            return new Vector2(-w,
                -len);
        }


        public Vector2 GetRandPosLogic()
        {
            var size = m_cityBuildingProxy.GetCitySize();
            int hsize = (size - 8) / 2;

            int x = 0;
            int y = 0;
            int index = 0;
            do
            {
                x = Random.Range(-hsize, hsize);
                y = Random.Range(-hsize, hsize);
                index = MakeCityGridKey(x, y);
            } while (index < 0 || index > m_GridState.Length - 1 || m_GridState[index] != CityObjData.CITY_GRID_STATE_NORMAL);

            return new Vector2(x,
                y);
        }

        public Vector2 MakeWorldPosFormLocal(float x, float y)
        {
            var p = new Vector3(x, 0, y);
            var l = m_cityBuildingContainer.transform.TransformPoint(p);
            return new Vector2(l.x, l.z);
        }

        /// <summary>
        /// 获取建筑碰撞面积大小
        /// </summary>
        /// <param name="buildType"></param>
        /// <returns></returns>
        public Vector2 GetBuildGirdSize(long buildType)
        {
            var bt = m_cityBuildingProxy.GetBuildingTypeConfigByType((EnumCityBuildingType) buildType);
            return new Vector2(bt.width * CityObjData.CITY_GRID_SIZE - CityObjData.CITY_GRID_SIZE_HALF,
                bt.length * CityObjData.CITY_GRID_SIZE - CityObjData.CITY_GRID_SIZE_HALF);
        }

        /// <summary>
        /// 获取建筑占用格子宽度
        /// </summary>
        /// <param name="buildType"></param>
        /// <returns></returns>
        public Vector2 GetBuildCellRow(long buildType)
        {
            var bt = m_cityBuildingProxy.GetBuildingTypeConfigByType((EnumCityBuildingType) buildType);
            return new Vector2(bt.width, bt.length);
        }

        //世界坐标转容器坐标
        public Vector2 MakeLocalPos(float x, float y)
        {
            var p = new Vector3(x, 0, y);
            var localPos = m_cityBuildingContainer.InverseTransformPoint(p);
            return new Vector2(localPos.x, localPos.z);
        }


        //逻辑位置转为一维数组索引
        private int MakeCityGridKey(int x, int y)
        {
            int size = m_cityBuildingProxy.GetCitySize();
            float a = Mathf.Ceil((size - 1f) * 0.5f);
            if (x > a || x < -a || y > a || y < -a)
            {
                return -1;
            }

            float line = y + a;
            float col = x + a + 1f;
            float key = line * size + col;
            return Mathf.FloorToInt(key + 0.5f);
        }

        /// <summary>
        /// 根据服务器建筑格子数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private void UpdateBuildGrids(long id, int state = 7)
        {
            if (m_cityBuildingContainer == null || m_cityMapFinder ==null || m_cityBuildingProxy ==null)
            {
                return;
            }

            if (tempBuildingInfoEntity!=null && tempBuildingInfoEntity.buildingIndex == id)
            {
                return;
            }

            var info = m_cityBuildingProxy.GetBuildingInfoByindex(id);

            if (info ==null)
            {
                Debug.Log("无法取到建筑"+id);
                return;
            }
            
            var size = GetBuildCellRow(info.type);
            int xend = (int) size.x + (int) info.pos.x;
            int yend = (int) size.y + (int) info.pos.y;

            if (size.x <= 0)
            {
                return;
            }

            bool isBorder = false;
                

            for (int xindex = (int) info.pos.x; xindex < xend; xindex++)
            {
                for (int yindex = (int) info.pos.y; yindex < yend; yindex++)
                {

                    isBorder = xindex == (xend-1) || yindex == (yend-1);
                    
                    
                    int index = MakeCityGridKey(xindex, yindex);

                    if (index > -1 && index < m_GridState.Length)
                    {
                        if (isBorder && state == CityObjData.CITY_GRID_RESERVED_OFFSET)
                        {
                            m_GridState[index] = CityObjData.CITY_GRID_BODEDER;
                        }
                        else
                        {
                            m_GridState[index] = state;
                        }

                       
                    }
                    else
                    {
                        //Debug.Log(m_GridState.Length + " index out " + index);
                    }
                }
            }

            m_cityMapFinder.SetFinderMap(m_GridState, m_cityBuildingProxy.GetCityAgeSize(), m_cityBuildingContainer);

            if (m_debug)
            {
                if (m_selectBuild != null)
                {
                    PrintMap(m_selectBuild.name + " UpdateBuildGrids " + state);
                }
                else
                {
                    PrintMap(id + " UpdateBuildGrids " + state);
                }
            }
        }

        private void PrintMap(string name = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("地图数据" + name);
            int size = m_cityBuildingProxy.GetCitySize();
            int[] line = new int[size];
            for (int s = m_GridState.Length - 1; s >= 0; s--)
            {
                int index = s % size;

                line[index] = m_GridState[s];
                if (index == 0)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        sb.Append(" ");
                        sb.Append(line[i]);
                    }

                    sb.AppendLine("");
                }
            }

            CoreUtils.logService.Debug(sb.ToString(), Color.green);
        }

        private bool CheckPosIsUesed(int x, int y)
        {
            int index = MakeCityGridKey(x, y);
            if (index > m_GridState.Length - 1 || index < 0 || m_GridState[index] != CityObjData.CITY_GRID_STATE_NORMAL)
            {
                return true;
            }

            return false;
        }
        
        private bool CheckPosInGird(int x, int y)
        {
            int index = MakeCityGridKey(x, y);
            if (index > m_GridState.Length - 1 || index < 0)
            {
                return false;
            }

            return true;
        }



        //检查移动中的建筑新位置是否有被占用
        private bool CheckGridIsUeded(float xLogic, float yLogic, Vector2 size)
        {
            int xend = (int) size.x + (int) xLogic;
            int yend = (int) size.y + (int) yLogic;
            int index;
            var isGridUsed = false;
//            Debug.Log("CheckGridIsUeded **************"+xLogic+"  "+yLogic);
            for (int xindex = (int) xLogic; xindex < xend; xindex++)
            {
                if (isGridUsed)
                {
                    break;
                }

                for (int yindex = (int) yLogic; yindex < yend; yindex++)
                {
                    index = MakeCityGridKey(xindex, yindex);
                    if (index > m_GridState.Length - 1 || index < 0 || m_GridState[index] != CityObjData.CITY_GRID_STATE_NORMAL)
                    {
//                        if (m_debug)
//                        {
//                            Debug.Log(index+" block: 1 "+" "+xindex+"   "+yindex);
//                        }

                        isGridUsed = true;
                    }

                    else
                    {
//                        Debug.Log(index+" 0 "+xindex+"   "+yindex);
                    }
                }
            }

            return isGridUsed;
        }

        /// <summary>
        /// 配置的网格数据 坐标系为 游戏的坐标系水平镜像  也就是右下角为第一  
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Vector2 SplitCityGridLogicPos(int key, int size)
        {
            float hsize = Mathf.Ceil(size * 0.5f);
            float line = Mathf.Ceil(key / size);
            float col = key % size;
            if (col == 0)
            {
                col = size;
            }

            return new Vector2(col - hsize, line - hsize);
        }

        /// <summary>
        /// 检查点是否在城内
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool IsCityOutBounds(Vector3 pos)
        {
            int size = m_cityBuildingProxy.GetCitySize();
            float hsize = ((float)size / 20);
            if (Mathf.Abs(pos.x) >= hsize || Mathf.Abs(pos.z) >= hsize)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 转换逻辑网格的位置为容器的内的位置坐标，这个坐标带上一半的
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xLogic"></param>
        /// <param name="yLogic"></param>
        /// <returns></returns>
        public Vector3 ConvertCityObjTileToLocal(long type, long xLogic, long yLogic)
        {
            if (type == (int) EnumCityBuildingType.CityWall || type == (int) EnumCityBuildingType.GuardTower) //
            {
                return Vector3.zero;
            }

            Vector2 size = GetBuildCellRow(type);
            return new Vector3(xLogic * CityObjData.CITY_GRID_SIZE + size.x * CityObjData.CITY_GRID_SIZE_HALF - CityObjData.CITY_GRID_SIZE_HALF, 0.01f,
                yLogic * CityObjData.CITY_GRID_SIZE + size.y * CityObjData.CITY_GRID_SIZE_HALF - CityObjData.CITY_GRID_SIZE_HALF);
        }

        public Vector3 ConvertCityObjTileToLocal(float xLogic, float yLogic)
        {
            return new Vector3(xLogic * CityObjData.CITY_GRID_SIZE - CityObjData.CITY_GRID_SIZE_HALF, 0.01f,
                yLogic * CityObjData.CITY_GRID_SIZE - CityObjData.CITY_GRID_SIZE_HALF);
        }

        public Vector3 ConvertTileToLocal(float xLogic, float yLogic)
        {
            return new Vector3(xLogic * CityObjData.CITY_GRID_SIZE + CityObjData.CITY_GRID_SIZE_HALF, 0.01f,
                yLogic * CityObjData.CITY_GRID_SIZE + CityObjData.CITY_GRID_SIZE_HALF);
        }


        /// <summary>
        /// 容器的内的位置坐标转为网格坐标
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xLocal"></param>
        /// <param name="yLocal"></param>
        /// <returns></returns>
        public Vector2 ConvertCityObjLocalToTile(long type, float xLocal, float yLocal)
        {
            if (type == (int) EnumCityBuildingType.CityWall || type == (int) EnumCityBuildingType.GuardTower)
            {
                return Vector3.zero;
            }

            Vector2 size = GetBuildCellRow(type);
            return new Vector2(Mathf.Floor((xLocal - size.x * 0.05f) * CityObjData.CITY_GRID_SIZE_RECIPROCAL + 0.55f),
                Mathf.Floor((yLocal - size.y * 0.05f) * CityObjData.CITY_GRID_SIZE_RECIPROCAL + 0.55f));
        }


        /// <summary>
        /// 网格位置转为实际位置 不存在偏移量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector3 ConvertMeshGirdToLocal(long x, long y)
        {
            return new Vector3(x * CityObjData.CITY_GRID_SIZE, 0.01f, y * CityObjData.CITY_GRID_SIZE);
        }

        public static Vector3 ScreenToMapPosition(float x, float y)
        {
            return WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
        }

        #endregion


        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus == false)
            {
                if (GuideProxy.IsGuideing)
                {
                    return;
                }
#if UNITY_EDITOR

#else
                if (tempBuildingInfoEntity != null)
                {
                    //AppFacade.GetInstance().SendNotification(CmdConstant.CreateTempBuildNO);
                }
                else
                {
                    ResetBuildMovePos();
                }
#endif
            }
        }

        private void OnTouche3D(int x, int y, string parentName, string colliderName)
        {
            if (m_debug)
            {
                Debug.LogFormat("OnTouche3D x = {0},y = {1},name1 = {2},name2 = {3}", x, y, parentName, colliderName);
            }
        }


        private void OnTouche3DBegin(int x, int y, string parentName, string colliderName)
        {
            
            m_liftUp = false;
            if (m_cityBuildingContainer == null) return;//重连有可能出现城堡未完成初始化
            Transform crrSelectBuild = m_cityBuildingContainer.Find(parentName);
            if (colliderName == "groundCollider")
            {
                crrSelectBuild = null;
            }
            if (m_debug)
            {
                Debug.LogFormat("禁用拖动{0}  {1}  {2} x:{3} y:{4}", WorldCamera.Instance().CanDrag(), crrSelectBuild,
                    m_selectBuild, x, y);
            }

            m_startTouchPos = new Vector2(x, y);

            if (IsGridUsed && crrSelectBuild != m_selectBuild  )
            {
                if (tempBuildingInfoEntity==null && crrSelectBuild!=null)
                {
                    ResetBuildMovePos();
                }
                else
                {
                    if (tempBuildingInfoEntity!=null && m_selectBuild!=null && parentName.Contains("CityWall_") && IsGridUsed && IsCityOutBounds(m_selectBuild.localPosition))
                    {
                        var wp = ScreenToMapPosition(x, y);
                        var curMovePos = MakeLocalPos(wp.x, wp.z);
                        Vector2 preLogicPos = ConvertCityObjLocalToTile((int) m_selectType, curMovePos.x, curMovePos.y);

                        m_selectBuild.localPosition =
                            ConvertCityObjTileToLocal((long) m_selectType, (long) preLogicPos.x, (long) preLogicPos.y);
                        
                        IsGridUsed = CheckGridIsUeded(preLogicPos.x, preLogicPos.y,
                            GetBuildCellRow((int) m_selectType));
                        
                        SetMoveArrow();
                        var m = m_isMoving;

                        m_isMoving = true;
                        SetBuildBottom();
                        m_isMoving = m;

                    }
                }
            }

            //第二次选择的建筑还是一个开始拖动 
            if (m_selectBuild != null && crrSelectBuild == m_selectBuild && WorldCamera.Instance().CanDrag())
            {
                WorldCamera.Instance().SetCanDrag(false);
                WorldCamera.Instance().SetCanClick(false);
            }
        }


        private void ResetBuildMovePos()
        {
            if (m_debug)
            {
                Debug.Log("还原坐标" + IsGridUsed);
            }

            if (IsGridUsed)
            {
                if (tempBuildingInfoEntity != null)
                {
                    if (m_debug)
                    {
                        Debug.Log("还原坐标22" + IsGridUsed);
                    }

                    RemoveCityBuild(tempBuildingInfoEntity, true);

                    tempBuildingInfoEntity = null;
                    tempBuildType = null;
                    IsGridUsed = false;
                }
                else
                {
                    //还原坐标
                    if (m_selectBuild != null)
                    {
                        m_selectBuild.localPosition = m_startBuildPos;
                    }


                    UpdateBuildGrids(m_selectBuildID);
                }
            }

//            Debug.Log(m_selectBuildID+" 还原数据");
            ShowBuildMenu(false);
            SelectBuild("");
            SetMoveArrow();
            SetBuildBottom();
            IsGridUsed = false;
            m_selectBuildID = 0;
        }
        /// <summary>
        /// 距离小于20判断未点击
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="colliderName"></param>
        /// <param name="click"></param>
        public void BuildMoveNewPos(string parentName, string colliderName,bool click)
        {
            var HasChange = false;

            if (m_selectBuildID > 0 && m_selectBuild != null)
            {
                var info = m_cityBuildingProxy.GetBuildingInfoByindex(m_selectBuildID);
                m_moveLogicPos = ConvertCityObjLocalToTile((int) m_selectType, m_selectBuild.localPosition.x,
                    m_selectBuild.localPosition.z);
                if (m_debug)
                {
                    Debug.Log(m_selectBuildID + "移动到新位置" + m_moveLogicPos + "  x:" + info.pos.x + "  y:" + info.pos.y);
                }

                if (info != null && (info.pos.x != m_moveLogicPos.x || info.pos.y != m_moveLogicPos.y))
                {
                    info.pos.x = (long) m_moveLogicPos.x;
                    info.pos.y = (long) m_moveLogicPos.y;


                    //保存逻辑点位 m_moveLogicPos  保存新位置到服务器 临时建筑不保存到服务器
                    if (tempBuildingInfoEntity == null)
                    {
                        m_cityBuildingProxy.BuildMove(m_selectBuildID, (int) m_moveLogicPos.x,
                            (int) m_moveLogicPos.y);
                        GameObject gameObject = m_cityBuildingProxy.BuildObjDic[info.buildingIndex];
                        ClientUtils.AddEffect("build_3003", gameObject.transform.position, null);
                        BuildMovedDownBefore(m_selectType, m_selectBuildID, info.pos.x, info.pos.y);
                    }


                    HasChange = true;
                }

//                Debug.Log(m_selectBuildID+" 放下数据 "+HasChange);
                UpdateBuildGrids(m_selectBuildID);

                if (HasChange)
                {
                    BuildMovedDownAfter(m_selectType, m_selectBuildID);
                    CoreUtils.audioService.PlayOneShot(RS.SoundBuildSet);
                }

                if (tempBuildingInfoEntity == null&& m_liftUp)
                {
                    ShowMenu(info, false, parentName, colliderName);
                }
            }
        }


        private void OnTouche3DEnd(int x, int y, string parentName, string colliderName)
        {
            var endTouchPos = new Vector2(x, y);
            if (m_cityBuildingContainer == null) return;//重连有可能出现城堡未完成初始化
            GlobalViewLevelMediator m =
                AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                    GlobalViewLevelMediator;

            MapViewLevel crrLevel = m.GetViewLevel();
            FogSystemMediator fogSystemMediator =
                AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as
                    FogSystemMediator;

            if (m.IsMenuOrinfo())
            {
                if (!GuideProxy.IsGuideing)
                {
                    WorldCamera.Instance().SetCanDrag(true);
                }

                WorldCamera.Instance().SetCanClick(true);


                if (m_selectBuild != null)
                {
                    Vector2 prePos = new Vector2(m_selectBuild.localPosition.x,
                        m_selectBuild.localPosition.z);
                    Vector2 preLogicPos = ConvertCityObjLocalToTile((int) m_selectType, prePos.x, prePos.y);

                    if (!m_startBuildPos.Equals(m_selectBuild.localPosition))
                    {
                        IsGridUsed = CheckGridIsUeded(preLogicPos.x, preLogicPos.y,
                            GetBuildCellRow((int) m_selectType));
                    }
                }

                if (m_debug)
                {
                    Debug.Log(parentName + "点击结束 :" + parentName + " IsGridUsed: " + IsGridUsed + "  m_selectBuildID:" +
                              m_selectBuildID + "  x:" + x + " y:" + y + "  " + m_startTouchPos);
                }



                if (Vector2.Distance(m_startTouchPos, endTouchPos) > 20 && parentName.Contains("CityWall"))
                {
                    Debug.Log("拖动结束不处理,并且没有点到物体的情况下");

                    if (IsGridUsed == false && m_selectBuild != null)
                    {
                        if (m_debug)
                        {
                            Debug.Log(parentName + "点击结束2 :" + parentName + " IsGridUsed: " + IsGridUsed + "  m_selectBuildID:" +
                                      m_selectBuildID + "  x:" + x + " y:" + y + "  " + m_startTouchPos+"  "+m_selectBuild.name);
                        }
                        ShowBuildMenu(m_selectBuild != null, m_selectBuild.gameObject.name, colliderName);
                        SetBuildBottom();
                        SetMoveArrow();
                        
                    }

                   

                    return;
                }

                if (IsGridUsed == false && tempBuildingInfoEntity == null)
                {
                    //pc上特殊情况会出现点击和起来不一致的情况
                    if (m_selectBuild!=null && m_selectBuild.name != parentName && Vector2.Distance(m_startTouchPos, endTouchPos) > 20 )
                    {
                        ResetBuildMovePos();
                        if (m_selectBuild != null)
                        {
                            SelectBuild(m_selectBuild.name, "");
                        }
                        return;
                    }

                        BuildMoveNewPos(parentName, colliderName, Vector2.Distance(m_startTouchPos, endTouchPos) <= 20);
                    bool lastSelect = m_selectBuildID != 0;
                    if (!lastSelect && Vector2.Distance(m_startTouchPos, endTouchPos) >= 20)
                    {

                    }
                    else
                    {
                        bool HasChange = SelectBuild(parentName, colliderName);

                        if (HasChange)
                        {
                            ShowBuildMenu(m_selectBuild != null, parentName, colliderName, Vector2.Distance(m_startTouchPos, endTouchPos) < 20);
                        }
                    }
                }

                if (IsGridUsed && parentName.Contains("CityWall") && tempBuildingInfoEntity == null)
                {
                    ResetBuildMovePos();
                    SelectBuild(parentName, colliderName);
                }
            }
            else
            {
                if (Vector2.Distance(m_startTouchPos, endTouchPos) > 20)
                {
                    return;
                }
                //   Debug.LogError(parentName + "  " + colliderName);
                if (crrLevel < MapViewLevel.TacticsToStrategy_1)
                {
                    string[] strArr = parentName.Split('_');
                    if (string.Equals(strArr[0], EnumCityBuildingType.TownCenter.ToString()) &&
                        string.Equals(strArr[1], "other"))
                    {
                        long cityRid = 0;
                        if (long.TryParse(strArr[2], out cityRid))
                        {
                            MapObjectInfoEntity request = m_worldMapObjectProxy.GetWorldMapObjectByRid(cityRid);
                            if (request != null)
                            {
                                if (m.IsLodVisable(request.objectPos.x / 100, request.objectPos.y / 100))
                                {
                                    if (!m_isPvpTouchCity)
                                    {
                                        CoreUtils.uiManager.ShowUI(UI.s_pop_WorldObjectPlayer, null, request);
                                    }

                                    m_isPvpTouchCity = false;
                                }
                                else
                                {
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    else if (IsClickCity(x, y, parentName, colliderName))
                    {
                        if (m_cityBuildingProxy.LockMoveEvent)
                        {
                            return;
                        }
          
                            CoreUtils.uiManager.ShowUI(UI.s_pop_WorldObjectPlayer, null, m_playerProxy.CurrentRoleInfo);
                            AppFacade.GetInstance().SendNotification(CmdConstant.MapUnSelectCity);

                        
                    }
                    else
                    {
                    }
                }
            }
        }


        private void OnTouche3DReleaseOutside(int x, int y, string parentName, string colliderName)
        {
            //            Debug.LogFormat("OnTouche3DReleaseOutside x = {0},y = {1},name1 = {2},name2 = {3}", x, y, parentName,
            //                colliderName);
        }

        #region 地图拖动处理

        public void OnTouchBegan(int x, int y)
        {
            m_startTouchPos = new Vector2(x, y);
            if (!IsSelectBuildCanMove())
            {
                return;
            }

            m_pressed = true;
            var wp = ScreenToMapPosition(x, y);
            m_startMovePos = MakeLocalPos(wp.x, wp.z);
            m_fixedMoveStep = Vector2.zero;


            if (m_debug)
            {
                Debug.Log("起始点:" + m_startBuildPos + IsGridUsed + "  " + x + " y:" + y + "  " + wp + "  " +
                          m_startTouchPos);
            }

            if (m_selectBuild != null && IsGridUsed == false)
            {
                m_startBuildPos = m_selectBuild.localPosition;
            }
        }
        public void OnTouchMoved(int x, int y)
        {
            if (m_debug)
            {
                Debug.Log("start OnTouchMoved:" + x + "  " + y + "  " + m_pressed + "  " + m_isMoving);
            }
            if (!IsSelectBuildCanMove())
            {
                return;
            }

            if (m_pressed && m_selectBuild != null&& !m_uiPressed)
            {
                if (m_hasShowBuildMenu)
                {
                    if (tempBuildingInfoEntity == null)
                    {
                        ShowBuildMenu(false);
                    }
                }

                var wp = ScreenToMapPosition(x, y);
                var curMovePos = MakeLocalPos(wp.x, wp.z);

                var moveStep1 = curMovePos - m_startMovePos + m_fixedMoveStep;
                var moveStep = new Vector2(0, 0);
                moveStep.x = Mathf.Floor(Mathf.Abs(moveStep1.x) * 10f) * 0.1f * (moveStep1.x).NumberSymbol();
                moveStep.y = Mathf.Floor(Mathf.Abs(moveStep1.y) * 10f) * 0.1f * (moveStep1.y).NumberSymbol();

                if (moveStep.x == 0 && moveStep.y == 0)
                {
                    return;
                }

                if (m_isMoving == false)
                {
                    if (IsGridUsed == false)
                    {
                        m_moveLogicPos =
                            ConvertCityObjLocalToTile((int) m_selectType, m_startBuildPos.x, m_startBuildPos.z);
                    }

                    //更新坐标
                    if (m_debug)
                    {
                        Debug.Log(m_selectBuildID + " 提起 " + IsGridUsed);
                    }

                    if (IsGridUsed == false)
                    {
                        UpdateBuildGrids(m_selectBuildID, CityObjData.CITY_GRID_STATE_NORMAL);
                    }

                    if (tempBuildingInfoEntity == null)
                    {
                        ShowBuildMenu(false);
                    }

                    m_isMoving = true;
                    
                    m_revBuildUpgradeBorard = ShowUpgradeBoard(m_selectBuild.gameObject,false);
                    
                    BuildMoveMat(m_isMoving);
                    DrawMesh();
                }

                m_fixedMoveStep = moveStep1 - moveStep;
                Vector2 prePos = new Vector2(m_selectBuild.localPosition.x + moveStep.x,
                    m_selectBuild.localPosition.z + moveStep.y);
                Vector2 preLogicPos = ConvertCityObjLocalToTile((int) m_selectType, prePos.x, prePos.y);

                m_selectBuild.localPosition =
                    ConvertCityObjTileToLocal((long) m_selectType, (long) preLogicPos.x, (long) preLogicPos.y);

                IsGridUsed = CheckGridIsUeded(preLogicPos.x, preLogicPos.y, GetBuildCellRow((int) m_selectType));
//                Debug.Log("m_startMovePos: "+m_startMovePos+"curMovePos: "+curMovePos+" moveStep"+moveStep+" moveStep1"+moveStep1+"  fix:"+m_fixedMoveStep+ "prePos: "+prePos+" preLogicPos:"+preLogicPos);

                if (m_moveLogicPos != preLogicPos)
                {
                    CoreUtils.audioService.PlayOneShot(RS.SoundBuildMove);
                }

                SetMoveArrow();
                SetBuildBottom();

                m_startMovePos = curMovePos;
                m_moveLogicPos = preLogicPos;
            }
        }

        public void OnTouchEnded(int x, int y)
        {
            if (m_debug)
            {
                Debug.Log("拖动结束:" + IsGridUsed);
            }
            m_pressed = false;
            m_uiPressed = false;
            m_startMovePos = Vector2.zero;
            m_fixedMoveStep = Vector2.zero;
            m_isMoving = false;

            if (m_selectBuild!=null && m_revBuildUpgradeBorard)
            {
                ShowUpgradeBoard(m_selectBuild.gameObject,true);
                m_revBuildUpgradeBorard = false;
            }
            BuildMoveMat(m_isMoving);
        }

        public bool IsSelectBuildCanMove()
        {
            if (m_GridState == null || m_cityBuildingContainer == null)
            {
                return false;
            }

            //建筑物被缩放过就不能在移动了
            if (m_selectBuild != null && m_selectBuild.transform.localScale.x > 1f)
            {
                return false;
            }

            return m_selectType != EnumCityBuildingType.CityWall && m_selectType != EnumCityBuildingType.GuardTower;
        }

        public bool IsClickCity(int x, int y, string parentName, string colliderName)
        {
            string[] strArr = parentName.Split('_');

            if (!string.IsNullOrEmpty(parentName) && !string.IsNullOrEmpty(colliderName) && m_cityBuildingContainer!=null&&
                m_cityBuildingContainer.Find(parentName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SelectBuild(string parentName = "", string colliderName = "")
        {
            bool HasChange = true;
            if (string.IsNullOrEmpty(parentName)|| string.Equals(colliderName, "groundCollider"))
            {
                if (m_debug && m_selectBuild)
                {
                    Debug.Log(parentName + "取消选择建筑" + m_selectBuild.name);
                }

                BuildResetMat();
                HideMesh();
                if (m_selectBuildHelper != null)
                {
                    m_selectBuildHelper.SetColliderOrder(0.0f);
                }

                m_selectBuild = null;
                m_selectBuildHelper = null;
                m_selectBuildMakeSprite = null;
            }
            else
            {
                var crrSelectedBuild = m_cityBuildingContainer.Find(parentName);
                if (crrSelectedBuild != null)
                {
                    if (crrSelectedBuild != m_selectBuild)
                    {
                        BuildResetMat();
                        m_selectBuild = crrSelectedBuild;

                        m_selectBuildHelper = m_selectBuild.GetComponent<TownBuilding>();

                        if (m_selectBuildHelper != null)
                        {
                            m_selectBuildHelper.SetColliderOrder(0.02f);
                        }

                        var sp = m_selectBuild.Find("sprite");
                        if (sp != null)
                        {
                            m_selectBuildMakeSprite = sp.GetComponent<MaskSprite>();
                        }

                        string[] names = m_selectBuild.gameObject.name.Split('_');
                        if (names.Length > 1)
                        {
                            m_selectType = (EnumCityBuildingType) Enum.Parse(typeof(EnumCityBuildingType), names[0]);
                            m_selectBuildID = long.Parse(names[1]);
                        }

                        if (!string.IsNullOrEmpty(parentName) && !string.IsNullOrEmpty(colliderName))
                        {
                            if (m_selectType <= EnumCityBuildingType.Smithy)
                            {
                                CoreUtils.audioService.PlayOneShot(RS.SoundBuildSelected);
                            }
                            else if (m_selectType == EnumCityBuildingType.Road)
                            {
                                CoreUtils.audioService.PlayOneShot(RS.SoundRoadSelected);
                            }
                            else if (m_selectType > EnumCityBuildingType.Road &&
                                     m_selectType <= EnumCityBuildingType.tree4)
                            {
                                CoreUtils.audioService.PlayOneShot(RS.SoundTreeSelected);
                            }
                        }


                        BuildSelectedMat();


                        if (m_debug)
                        {
                            Debug.Log(m_selectType + "选择建筑" + m_selectBuild.name);
                        }
                    }
                    else
                    {
                        HasChange = false;
                        if (m_selectType == EnumCityBuildingType.GuardTower )
                        {
                            string[] split = m_cityBuildingProxy.FollowGameObject.Split('|');

                            if (split.Length == 2 && colliderName != split[1])
                            {
                                HasChange = true;
                            }
                        }
                      
                    }
                }
                else
                {
                    if (m_debug && m_selectBuild)
                    {
                        Debug.Log(parentName + "不选择建筑2//" + m_selectBuild.name);
                    }

                    BuildResetMat();
                    HideMesh();
                    if (m_selectBuildHelper != null)
                    {
                        m_selectBuildHelper.SetColliderOrder(0.0f);
                    }

                    m_selectBuild = null;
                    m_selectBuildHelper = null;
                    m_selectBuildMakeSprite = null;
                }
            }

            SetBuildBottom();
            SetMoveArrow();
            return HasChange;
        }

        private void BuildSelectedMat()
        {
            if (m_selectBuildMakeSprite != null)
            {
                if (LightManager.Instance().isNight)
                {
                    m_selectBuildMakeSprite.UpdatedMaterial("city_building_selected_night", BuildClickBounciong);
                }
                else
                {
                    m_selectBuildMakeSprite.UpdatedMaterial("city_building_selected", BuildClickBounciong);
                }
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.BuildSelected, m_selectBuildID);
        }

        private void BuildResetMat()
        {
            if (m_selectBuild != null)
            {
                if (m_selectBuildMakeSprite != null)
                {
                    var matname = LightManager.Instance().isNight ? "city_building_night" : "map_building";
                    //m_selectBuildMakeSprite.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    m_selectBuildMakeSprite.UpdatedMaterial(matname, () =>
                    {
                        if (m_selectBuildMakeSprite!=null)
                        {
                            m_selectBuildMakeSprite.SetFloat("_Bouncing",0);
                            m_selectBuildMakeSprite.SetCanChangeLight(true);
                        }
                    });
                    
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.BuildSelectReset,m_selectBuildID);
            }
        }

        private void BuildMoveMat(bool isMoving)
        {
            if (m_selectBuild != null)
            {
                if (m_selectBuildMakeSprite != null)
                {
                    if (isMoving || IsGridUsed || tempBuildingInfoEntity!=null)
                    {
                        m_selectBuildMakeSprite.UpdatedMaterial("city_building_move",()=>{
                            if (m_selectBuildMakeSprite!=null)
                            {
                                m_selectBuildMakeSprite.SetFloat("_Bouncing",2);
                                m_selectBuildMakeSprite.SetCanChangeLight(false);
                            }
                        });
                    }
                    else
                    {
                        if (IsGridUsed == false)
                        {
                            var matname = LightManager.Instance().isNight ? "city_building_selected_night" : "city_building_selected";
                            m_selectBuildMakeSprite.UpdatedMaterial(matname, () =>
                            {
                                if (m_selectBuildMakeSprite!=null)
                                {
                                    m_selectBuildMakeSprite.SetFloat("_Bouncing",0);
                                    m_selectBuildMakeSprite.SetCanChangeLight(true);
                                }
                            });
                        }
                    }
                }
            }
        }

        //选择缓动
        private void BuildClickBounciong()
        {
            m_clickBounceTime = 200;
            float s = 0.5f;
            m_clickBounceConst = 180;
            m_clickBounceA = Mathf.Pow(s, 0.5f) / m_clickBounceConst * 2f;

            m_clickTimer = Timer.Register(1, EndClickBounciong, UpdateClickBounciong, true);
        }

        private void UpdateClickBounciong(float dt)
        {
            m_clickBounceTime = m_clickBounceTime - 30; //Time.deltaTime*1000;
            if (m_clickBounceTime < 0)
                m_clickBounceTime = 0;

            if (m_clickBounceTime > 0)
            {
                float t = Mathf.Pow((m_clickBounceConst - m_clickBounceTime) * m_clickBounceA, 2);


                t = Mathf.Min(2, Mathf.Max(0, t));

                if (t > 1f)
                {
                    t = 2 - t;
                }

                t = t * 0.5f;

                if (t < 0.03)
                {
                    t = 0;
                }

                if (m_clickBounceTime < 31)
                {
                    t = 0;
                }

//                Debug.Log(Time.deltaTime+"  "+m_clickBounceTime+"_ClickBounceStep:"+t);
                if (m_selectBuildMakeSprite != null)
                {
                    m_selectBuildMakeSprite.SetFloat("_ClickBounceStep", -t);
                    m_selectBuildMakeSprite.SetFloat("_Scale", t / 20 + 1);
                }

                if (m_clickBounceTime <= 0)
                {
                    EndClickBounciong();
                }
            }
        }

        private void EndClickBounciong()
        {
            if (m_clickTimer != null)
            {
                Debug.Log("移除");
                m_clickTimer.Cancel();
                m_clickTimer = null;
            }
        }

        private void BuildBouncing(bool isBouncing)
        {
            if (m_selectBuild != null)
            {
                if (m_selectBuildMakeSprite != null)
                {
//                    Debug.Log("搁置_Bouncing "+(isBouncing?2:0));
                    m_selectBuildMakeSprite.SetFloat("_Bouncing", isBouncing ? 2 : 0);
                }
            }
        }

        #endregion


        private void ShowBuildMenu(bool show, string parentName = "", string colliderName = "",bool isClickNeedFix = false)
        {
            //Debug.Log("菜单"+show);
            if (show)
            {
                if (m_selectBuild != null)
                {
                    if (isClickNeedFix)
                    {

                        Vector3 showMenuObj = m_selectBuild.position;
                        if (!string.IsNullOrEmpty(colliderName))
                        {
                            var objTransform = m_selectBuild.Find(colliderName);

                            if (objTransform!=null)
                            {
                                showMenuObj = objTransform.position;
                            }
                        }
                        
                        Vector2 screenPos = WorldCamera.Instance().GetCamera()
                            .WorldToViewportPoint(showMenuObj);
                        float edgeRateX = 0.1f;
                        float edgeRateY = 0.08f;

                        var targetScreenPos = new Vector2(screenPos.x, screenPos.y);
                        if (screenPos.x < edgeRateX)
                        {
                            //左
                            targetScreenPos.x = edgeRateX*2;
                        }
                        else
                        {
                            if (1 - edgeRateX < screenPos.x)
                            {
                                //右
                                targetScreenPos.x = 1 - edgeRateX;
                            }
                        }

                        if (screenPos.y < edgeRateY * 2)
                        {
                            //下
                            targetScreenPos.y = edgeRateY * 3;
                        }

                        else
                        {
                            if (1 - edgeRateY < screenPos.y)
                            {
                                //上
                                targetScreenPos.y = 1 - edgeRateY*2;
                            }
                        }

                        if (targetScreenPos != screenPos)
                        {
                            float screenWidth = Screen.safeArea.width;
                            float screenHeight = Screen.safeArea.height;
                            float nx = screenWidth * targetScreenPos.x;
                            float ny = screenHeight * targetScreenPos.y;
                            var newTerrainPos = WorldCamera.Instance()
                                .GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), nx, ny);

                            var midle = WorldCamera.Instance().GetViewCenter();

                            Vector2 fixDis = new Vector2(showMenuObj.x, showMenuObj.z) -
                                             new Vector2(newTerrainPos.x, newTerrainPos.z);

                            Vector2 newPoint = midle + fixDis * 2;

                            WorldCamera.Instance()
                                .ViewTerrainPos(newPoint.x, newPoint.y, 100, () => { });
                        }
                    }
                }


                AppFacade.GetInstance()
                    .SendNotification(CmdConstant.ShowBuildingHudMenu, parentName + "|" + colliderName);
            }
            else
            {
                AppFacade.GetInstance()
                    .SendNotification(CmdConstant.CloseBuildingHudMenu);
            }

            m_hasShowBuildMenu = show;
        }

        /// <summary>
        /// 打开类型和等级的建筑
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        private void ShowMenu(BuildingInfoEntity CityBuildingInfo, bool isNewBuild = false, string parentName = "",
            string colliderName = "")
        {

            if (CityBuildingInfo==null)
            {
                return;
            }
                
            if (isNewBuild == false)
            {
                ResetBuildMovePos();
            }

            string buildName = string.Format("{0}_{1}", (EnumCityBuildingType) CityBuildingInfo.type,
                CityBuildingInfo.buildingIndex);
            SelectBuild(buildName);
            if (m_selectBuild != null)
            {
                //if (string.IsNullOrEmpty (parentName) && string.IsNullOrEmpty(colliderName)&&tempBuildingInfoEntity==null)
                //{
                //    return;
                //}
                AppFacade.GetInstance()
                    .SendNotification(CmdConstant.ShowBuildingHudMenu, buildName + "|" + colliderName);
            }
        }

        /// <summary>
        /// 摄像头移动到建筑
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        private void MoveCameraToBuilding(BuildingInfoEntity CityBuildingInfo)
        {
            GameObject obj = m_cityBuildingProxy.BuildObjDic[CityBuildingInfo.buildingIndex];
            if (obj != null)
            {
                BoxCollider boxCollider = obj.GetComponentInChildren<BoxCollider>();
                if (boxCollider != null)
                {
                    Transform findObj = boxCollider.transform;
                    WorldCamera.Instance().ViewTerrainPos(findObj.position.x, findObj.position.z, 500, () => { });
                }
            }
        }

        /// <summary>
        /// 耐久未0的迁城表现
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        private void CityDown()
        {
            m_cityBuildingContainer.gameObject.SetActive(false);
            m_roleNotifyMoveCity = false;
            CoreUtils.assetService.Instantiate(RS.CityDown,
                (go) => {
                    go.transform.position = m_cityBuildingContainer.position;
                    CoreUtils.audioService.PlayOneShot("Sound_operation_2019");
                });
            Timer.Register(3, () =>
            {
                if (m_cityBuildingContainer != null) m_cityBuildingContainer.gameObject.SetActive(true);
                m_globalViewLevelMediator.StartShowHud();
            }, null, false, false);
        }

        #region 特效

        private void CheckedAgeChange()
        {
            if (m_cityBuildingProxy.AgeChange)
            {
                m_cityBuildingProxy.AgeChange = false;
                AgeChangeEffect();
            }
        }

        public void AgeChangeEffect()
        {
            CityAgeSizeDefine cityAgeSizeDefine = m_cityBuildingProxy.GetCityAgeSizeDefine();
            if (cityAgeSizeDefine != null)
            {
                if (cityAgeSizeDefine.ageDialogBegin != 0)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.AgeStart);
                    AppFacade.GetInstance()
                        .SendNotification(CmdConstant.ShowChapterDiaglog, cityAgeSizeDefine.ageDialogBegin);
                }
                else
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.AgeStart);
                    AgeChangeEffect1();
                }
            }
        }

        public void AgeChangeEffect1()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.ShowMask);
            WorldCamera.Instance()
                .ViewTerrainPos(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y, 0, () => { });

            float dxf = WorldCamera.Instance().getCameraDxf("city_age");
            WorldCamera.Instance().SetCameraDxf(dxf, 500, () =>

            {
                WorldCamera.Instance().SetCanDrag(false);
                WorldCamera.Instance().SetCanClick(false);
                WorldCamera.Instance().SetCanZoom(false);
                int age = (int)m_cityBuildingProxy.GetAgeType(m_playerProxy .CurrentRoleInfo.level);
                CityAgeSizeDefine cityAgeSize = CoreUtils.dataService.QueryRecord<CityAgeSizeDefine>(age);
                ClientUtils.UIAddEffect(RS.AgeTips, CoreUtils.uiManager.GetUILayer((int) UILayer.StoryLayer).transform, (go) =>
                {
                    m_tileCollederManager.RemoveAll();
                    //List<BuildingInfoEntity> BuildGroupInfos = m_cityBuildingProxy.GetBuildingInfosByGroupType((int)EnumBuildingGroupType.ZHUANGSHI);
                    //for (int j = 0; j < BuildGroupInfos.Count; j++)
                    //{
                    //    BuildingInfoEntity info = BuildGroupInfos[j];
                    //    GameObject destroyGameObject = m_cityBuildingProxy.BuildObjDic[info.buildingIndex];
                    //    m_cityBuildingProxy.RemoveBuildGameObject(info.buildingIndex);
                    //    CreateCityBuilding(info, m_cityBuildingContainer, (obj) =>
                    //    {
                    //        CoreUtils.assetService.Destroy(destroyGameObject);

                    //    }, true);
                    //}
                    Timer.Register(2, () =>
                    {
                        GameObject m_effectGO = ClientUtils.GetEffect(RS.AgeTips,
                            CoreUtils.uiManager.GetUILayer((int) UILayer.StoryLayer).transform);
                        if (m_effectGO != null)
                        {
                            GameObject.Destroy(m_effectGO);
                            List<BuildingInfoEntity> BuildingInfos = GetSortByDis();
                            int i = 0;
                            Timer time = null;

                            AppFacade.GetInstance().SendNotification(CmdConstant.CityAgeChangeLevelUpEffect);
                            {
                                BuildingInfoEntity info =
                                    m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.TownCenter);
                                GameObject destroyGameObject = m_cityBuildingProxy.BuildObjDic[info.buildingIndex];
                                m_cityBuildingProxy.RemoveBuildGameObject(info.buildingIndex);
                                CreateCityBuilding(info, m_cityBuildingContainer, (obj) =>
                                {
                                    obj.SetActive(true);
                                    CoreUtils.assetService.Destroy(destroyGameObject);
                                    ClientUtils.AddEffect("build_3004", obj.transform.position, null);
                                    CoreUtils.audioService.PlayOneShot(RS.sfx_buildingUp);
                                }, true);
                            }

                            Timer.Register(0.5f, () =>
                            {
                                time = Timer.Register(0.15f, () =>
                                {
                                    BuildingInfoEntity buildingInfo = BuildingInfos[i];

                                    if (buildingInfo.type != (long) EnumCityBuildingType.CityWall &&
                                        buildingInfo.type != (long) EnumCityBuildingType.GuardTower)
                                    {
                                        GameObject destroyGameObject =
                                            m_cityBuildingProxy.BuildObjDic[buildingInfo.buildingIndex];
                                        m_cityBuildingProxy.RemoveBuildGameObject(buildingInfo.buildingIndex);
                                        //  CoreUtils.assetService.Destroy(destroyGameObject);
                                        CreateCityBuilding(buildingInfo, m_cityBuildingContainer, (gameObject) =>
                                        {
                                            GameObject.DestroyImmediate(destroyGameObject);
                                            gameObject.SetActive(true);
                                            ClientUtils.AddEffect("build_3004", gameObject.transform.position, null);
                                            CoreUtils.audioService.PlayOneShot(RS.sfx_buildingUp);
                                        }, true);
                                        i++;
                                    }

                                    if (i == BuildingInfos.Count)
                                    {
                                        time.Cancel();
                                        time = null;

                                        Timer.Register(0.5f, () =>
                                        {
                                            {
                                                BuildingInfoEntity info =
                                                    m_cityBuildingProxy.GetBuildingInfoByType(
                                                        (long) EnumCityBuildingType.CityWall);
                                                GameObject destroyGameObject =
                                                    m_cityBuildingProxy.BuildObjDic[info.buildingIndex];
                                                m_cityBuildingProxy.RemoveBuildGameObject(info.buildingIndex);
                                                CreateCityBuilding(info, m_cityBuildingContainer, (obj) =>
                                                {
                                                    CoreUtils.assetService.Destroy(destroyGameObject);
                                                    Transform transform =
                                                        ClientUtils.FindDeepChild(obj, "effect") != null
                                                            ? ClientUtils.FindDeepChild(obj, "effect")
                                                            : CityObjData.GetMenuTargetGameObject(info.buildingIndex).transform;
                                                    ClientUtils.AddEffect("build_3004", transform.position, null);
                                                }, true);
                                            }
                                            {
                                                BuildingInfoEntity info =
                                                    m_cityBuildingProxy.GetBuildingInfoByType(
                                                        (long) EnumCityBuildingType.GuardTower);
                                                GameObject destroyGameObject =
                                                    m_cityBuildingProxy.BuildObjDic[info.buildingIndex];
                                                m_cityBuildingProxy.RemoveBuildGameObject(info.buildingIndex);
                                                CreateCityBuilding(info, m_cityBuildingContainer, (obj) =>
                                                {
                                                    CoreUtils.assetService.Destroy(destroyGameObject);
                                                    BoxCollider[] transforms =
                                                        obj.transform.GetComponentsInChildren<BoxCollider>();

                                                    for (int j = 0; j < transforms.Length; j++)
                                                    {
                                                        ClientUtils.AddEffect("build_3004", transforms[j].transform.position, null);
                                                    }
                                                    CoreUtils.audioService.PlayOneShot(RS.sfx_buildingUp);
                                                }, true);
                                            }
                                            Timer.Register(0.7f, () =>
                                            {
                                                clearBuilder();
                                                StartBuilder();
                                                CoreUtils.uiManager.CloseLayerUI(UILayer.WindowLayer);
                                                CoreUtils.uiManager.CloseLayerUI(UILayer.WindowMenuLayer);
                                                CoreUtils.uiManager.CloseLayerUI(UILayer.WindowPopLayer);
                                                CoreUtils.audioService.PlayOneShot(RS.SoundVictory);
                                                ClientUtils.UIAddEffect(cityAgeSize.ageShowUI,
                                                    CoreUtils.uiManager.GetUILayer((int) UILayer.StoryLayer).transform,
                                                    (go1) =>
                                                    {
                                                        go1.GetComponentInChildren<LanguageText>(true).text =
                                                            LanguageUtils.getText(cityAgeSize.l_nameId);
                                                        Timer.Register(4.8f, () =>
                                                        {
                                                            m_effectGO = ClientUtils.GetEffect(cityAgeSize.ageShowUI,
                                                                CoreUtils.uiManager.GetUILayer((int) UILayer.StoryLayer)
                                                                    .transform);

                                                            if (m_effectGO != null)
                                                            {
                                                                GameObject.Destroy(m_effectGO);
                                                            }

                                                            List<BuildingInfoEntity> BuildGroupInfos =
                                                                m_cityBuildingProxy.GetBuildingInfosByGroupType(
                                                                    (int) EnumBuildingGroupType.Decorative);
                                                            for (int j = 0; j < BuildGroupInfos.Count; j++)
                                                            {
                                                                BuildingInfoEntity info = BuildGroupInfos[j];
                                                                GameObject destroyGameObject =
                                                                    m_cityBuildingProxy.BuildObjDic[info.buildingIndex];
                                                                m_cityBuildingProxy.RemoveBuildGameObject(
                                                                    info.buildingIndex);
                                                                CreateCityBuilding(info, m_cityBuildingContainer,
                                                                    (obj) =>
                                                                    {
                                                                        CoreUtils.assetService.Destroy(
                                                                            destroyGameObject);
                                                                    }, true);
                                                            }

                                                            float city_default = WorldCamera.Instance()
                                                                .getCameraDxf("city_default");
                                                            WorldCamera.Instance().SetCameraDxf(city_default, 500, () =>
                                                            {
                                                                WorldCamera.Instance().SetCanDrag(true);
                                                                WorldCamera.Instance().SetCanClick(true);
                                                                WorldCamera.Instance().SetCanZoom(true);
                                                                AppFacade.GetInstance()
                                                                    .SendNotification(CmdConstant.HideMask);
                                                                CityAgeSizeDefine cityAgeSizeDefine =
                                                                    m_cityBuildingProxy.GetCityAgeSizeDefine();
                                                                if (cityAgeSizeDefine.ageDialogBegin != 0)
                                                                {
                                                                    AppFacade.GetInstance()
                                                                        .SendNotification(
                                                                            CmdConstant.ShowChapterDiaglog,
                                                                            cityAgeSizeDefine.ageDialogEnd);
                                                                }
                                                                else
                                                                {
                                                                }
                                                            });
                                                        });
                                                    });
                                            });
                                        });
                                    }
                                }, null, true, false);
                            });
                        }
                    });
                });
            });
        }

        private List<BuildingInfoEntity> GetSortByDis()
        {
            List<BuildingInfoEntity> List = m_cityBuildingProxy.CityBuildingInfoDic.Values.ToList();
            long X = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.TownCenter).pos.x;
            long Y = m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.TownCenter).pos.y;
            List.Sort((BuildingInfoEntity x, BuildingInfoEntity y) =>
            {
                int re = 0;
                if ((Math.Pow((x.pos.x - X), 2) + Math.Pow((x.pos.y - Y), 2)) >
                    (Math.Pow((y.pos.x - X), 2) + Math.Pow((y.pos.y - Y), 2)))
                {
                    re = 1;
                }
                else if ((Math.Pow((x.pos.x - X), 2) + Math.Pow((x.pos.y - Y), 2)) <
                         (Math.Pow((y.pos.x - X), 2) + Math.Pow((y.pos.y - Y), 2)))
                {
                    re = -1;
                }
                else
                {
                    re = 0;
                }

                return re;
            });
            for (int i = List.Count - 1; i >= 0; i--)
            {
                if (List[i].type == (long) EnumCityBuildingType.GuardTower
                    || List[i].type == (long) EnumCityBuildingType.CityWall
                    || List[i].type == (long) EnumCityBuildingType.TownCenter
                    || List[i].type == (long) EnumCityBuildingType.Road
                    || List[i].type == (long) EnumCityBuildingType.tree
                    || List[i].type == (long) EnumCityBuildingType.tree2
                    || List[i].type == (long) EnumCityBuildingType.tree3
                    || List[i].type == (long) EnumCityBuildingType.tree4
                )
                {
                    List.RemoveAt(i);
                }
            }

            return List;
        }

        #endregion
    }
}