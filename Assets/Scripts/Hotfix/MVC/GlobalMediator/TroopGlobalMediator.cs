// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月2日
// Update Time         :    2020年1月2日
// Class Description   :    TroopGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Client;
using Hotfix;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Skyunion;
using SprotoType;
using System;
using System.Net.Http.Headers;
using Data;
using PureMVC.Core;

namespace Game
{
    public class CityReinforceInfo
    {
        public GuildMemberInfoEntity targetGuildMemberInfoEntity;
        public MapObjectInfoEntity targetMapObjectInfoEntity;
        public int type = 0; //1联盟成员，2地图对象
        public Role_GetCityReinforceInfo.response response;
    }

    public class TroopGlobalMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "TroopGlobalMediator";

        #endregion

        //IMediatorPlug needs
        public TroopGlobalMediator() : base(NameMediator, null)
        {
        }

        private TroopProxy m_TroopProxy;
        private ScoutProxy m_ScoutProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private PlayerProxy m_playerProxy;
        private GlobalViewLevelMediator m_viewLevelMediator;

        private long m_targetRid; //增援目标Rid
        private GuildMemberInfoEntity m_targetGuildMemberInfoEntity; //增援目标
        private MapObjectInfoEntity m_targetMapObjectInfoEntity; //增援目标
        private int m_type = 0; //增援界面的数据来源 1，联盟成员， 2地图对象
        private ConfigDefine m_ConfigDefine;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.MapCreateTroopGo,
                Rally_RallyBattleInfo.TagName,
                CmdConstant.MoveToPosFixedCameraDxf,
                CmdConstant.MoveToPosPullUpCameraDxf,
                CmdConstant.MoveToPosAndOpen,
                CmdConstant.MoveAndOpenCityByArmindex,
                CmdConstant.GetCityReinforceInfo,
                Role_GetCityReinforceInfo.TagName,
                Rally_JoinRally.TagName,
                CmdConstant.AllianceEixt,
                CmdConstant.AllianceJoinUpdate,
                Role_CreateArmy.TagName,
                Map_March.TagName,
                Rally_LaunchRally.TagName,
                Rally_ReinforceRally.TagName,
                CmdConstant.MoveAndOpenCityByTarget,
                CmdConstant.NetWorkReconnecting,
                Rally_RepatriationRally.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.MapCreateTroopGo:
                    int troopId = (int) notification.Body;
                    CreateTroops(troopId);
                    break;
                case CmdConstant.NetWorkReconnecting:
                    m_RallyTroopsProxy.Clear();
                    m_TroopProxy.Clear();
                    m_ScoutProxy.Clear();

                    AppFacade.GetInstance().SendNotification(CmdConstant.OnTroopDataChanged); 

                    break;
                case Rally_RallyBattleInfo.TagName:
                    Rally_RallyBattleInfo.request info = notification.Body as Rally_RallyBattleInfo.request;
                    if (info != null)
                    {
                        m_RallyTroopsProxy.Set(info);
                    }

                    break;
                case CmdConstant.MoveToPosFixedCameraDxf:
                    if (notification.Body is Vector2)
                    {
                        Vector2 pos = (Vector2)notification.Body;
                        if (pos != null)
                        {
                            float dxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                            WorldCamera.Instance().SetCameraDxf(dxf, 500, () => { WorldCamera.Instance().ViewTerrainPos(pos.x, pos.y, 500, () => { }); });
                        }
                    }
                    else  if(notification.Body is string)
                    {
                       string  str =( string )notification.Body;
                        string[] strList = str.Split(',');
                        if (strList == null || strList.Length < 2)
                        {
                            return;
                        }
                        if (strList.Length == 2)
                        {
                            int jumpX = int.Parse(strList[0]);
                            int jumpY = int.Parse(strList[1]);
                            WorldCamera.Instance().SetCameraDxf(WorldCamera.Instance().getCameraDxf("map_tactical"), 1000f, null);
                            WorldCamera.Instance().ViewTerrainPos(jumpX  , jumpY , 1000f, null);
                        }
                        else if (strList.Length == 3)//TODO:
                        {
                            int jumpK = int.Parse(strList[0]);
                            int jumpX = int.Parse(strList[1]);
                            int jumpY = int.Parse(strList[2]);
                            WorldCamera.Instance().SetCameraDxf(WorldCamera.Instance().getCameraDxf("map_tactical"), 1000f, null);
                            WorldCamera.Instance().ViewTerrainPos(jumpX * 6, jumpY *6, 1000f, null);
                        }
                    }
                    break;
                case CmdConstant.MoveToPosPullUpCameraDxf:
                    if (notification.Body is Vector2)
                    {
                        Vector2 pos = (Vector2)notification.Body;
                        if (pos != null)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
                            float curdxf = WorldCamera.Instance().getCurrentCameraDxf();
                            float dxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                            if (curdxf >= dxf)
                            {
                                WorldCamera.Instance().ViewTerrainPos(pos.x, pos.y, 500, () => { });
                            }
                            else
                            {
                                WorldCamera.Instance().SetCameraDxf(dxf, 500, () => {WorldCamera.Instance().ViewTerrainPos(pos.x, pos.y, 500, () => { }); });
                        }
                        }
                    }
                    break;
                case CmdConstant.AllianceEixt:
                    m_RallyTroopsProxy.allianceEixt();
                    WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateAllLineColor();
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateTroopsColor();
                    break;
                case CmdConstant.AllianceJoinUpdate:
                    WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateAllLineColor();
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateTroopsColor();
                    break;
                case CmdConstant.MoveAndOpenCityByArmindex:
                    int aindex = (int) notification.Body;
                    OnMoveAndOpenCity(aindex);
                    break;
                case CmdConstant.MoveToPosAndOpen:
                    Vector2Int pos1 = (Vector2Int) notification.Body;
                    OnMoveAndOpenPanelByPos(pos1);
                    break;
                case CmdConstant.MoveAndOpenCityByTarget:
                    int targetObjectIndex = (int) notification.Body;
                    OnMoveAndOpenCityByTarget(targetObjectIndex);
                    break;
                case CmdConstant.GetCityReinforceInfo:
                    OnGetCityReinforceInfo(notification);
                    break;
                case Role_GetCityReinforceInfo.TagName:
                    ShowAllianceBuildPanel(notification);
                    break;
                case Rally_JoinRally.TagName:
                    OnRallyJoinErrorCode(notification);
                    break;
                case Role_CreateArmy.TagName:
                case Map_March.TagName:
                case Rally_LaunchRally.TagName:
                    OnRoleCreateArmyAndMapMarchAndRallLaunch(notification);
                    break;
                case Rally_ReinforceRally.TagName:
                    OnRallyReinforceErrorCode(notification);
                    break;
                case   Rally_RepatriationRally.TagName:
                    OnRally_RepatriationRallyErrorCode(notification);
                    break;
            }
        }


        #region UI template method          

        protected override void InitData()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_ScoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_RallyTroopsProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_viewLevelMediator = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;
            m_ConfigDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
//                BattleUIHandler.Instance.SetBattleUIData(214475,BattleUIType.BattleUI_HP,"88888");
//                BattleUIHandler.Instance.SetBattleUIData(214476,BattleUIType.BattleUI_AddBlood,"88888");
                //  BattleUIHandler.Instance.SetBattleUIData(testId, BattleUIType.BattleUI_ShowBeAttack,101301);
            }
            
            WorldMapLogicMgr.Instance.BattleSoundHandler.Update();
        }

        [IFix.Patch]
        public override void LateUpdate()
        {
            ClientUtils.lodManager.LateUpdate();
        }

        public override void FixedUpdate()
        {
        }


        private void ShowAllianceBuildPanel(INotification notification)
        {
            Role_GetCityReinforceInfo.response response = notification.Body as Role_GetCityReinforceInfo.response;
            if (response != null)
            {
                if (response.armyCountMax == 0)
                {
                    Tip.CreateTip(730352, Tip.TipStyle.Middle).Show();
                }
                else
                {
                    CityReinforceInfo cityReinforceInfo = new CityReinforceInfo();
                    cityReinforceInfo.targetGuildMemberInfoEntity = m_targetGuildMemberInfoEntity;
                    cityReinforceInfo.response = response;
                    cityReinforceInfo.type = m_type;
                    cityReinforceInfo.targetMapObjectInfoEntity = m_targetMapObjectInfoEntity;
                    CoreUtils.uiManager.ShowUI(UI.s_AllianceBuild, null, cityReinforceInfo);
                }
            }
        }

        private void OnGetCityReinforceInfo(INotification notification)
        {
            if (notification.Body is GuildMemberInfoEntity)
            {
                m_targetGuildMemberInfoEntity = notification.Body as GuildMemberInfoEntity;
                if (m_targetGuildMemberInfoEntity != null)
                {
                    m_targetRid = m_targetGuildMemberInfoEntity.rid;
                    m_type = 1;
                    Role_GetCityReinforceInfo.request request = new Role_GetCityReinforceInfo.request();
                    request.targetRid = m_targetRid;
                    AppFacade.GetInstance().SendSproto(request);
                }
            }
            else if (notification.Body is MapObjectInfoEntity)
            {
                m_targetMapObjectInfoEntity = notification.Body as MapObjectInfoEntity;
                if (m_targetMapObjectInfoEntity != null)
                {
                    m_targetRid = m_targetMapObjectInfoEntity.cityRid;
                    m_type = 2;
                    Role_GetCityReinforceInfo.request request = new Role_GetCityReinforceInfo.request();
                    request.targetRid = m_targetRid;
                    AppFacade.GetInstance().SendSproto(request);
                }
            }
        }


        private void OnMoveAndOpenCityByTarget(int targetObjectIndex)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(targetObjectIndex);
            if (mapObjectInfoEntity != null && mapObjectInfoEntity.objectType == 3 )
            {
                float dxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                WorldCamera.Instance().SetCameraDxf(dxf, 500, () =>
                {
                    WorldCamera.Instance().ViewTerrainPos(mapObjectInfoEntity.objectPos.x / 100, mapObjectInfoEntity.objectPos.y / 100, 500, () =>
                    {
                        if (mapObjectInfoEntity.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_pop_WorldObjectPlayer, null, m_playerProxy.CurrentRoleInfo);
                        }
                        else
                        {
                            if (mapObjectInfoEntity.gameobject != null)
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_pop_WorldObjectPlayer, null, mapObjectInfoEntity);
                            }
                        }
                    });
                });
            }
        }


        private void OnMoveAndOpenPanelByPos(Vector2 pos)
        {
            if (pos != null)
            {
                if (pos.x == 0 || pos.y == 0)
                {
                    return;
                }
                float dxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                WorldCamera.Instance().SetCameraDxf(dxf, 200, () =>
                {
                    WorldCamera.Instance().ViewTerrainPos(pos.x, pos.y, 200, () =>
                    {
                        if (!m_viewLevelMediator.IsMenuOrinfo())
                        {
                            if (m_viewLevelMediator.IsLodVisable(pos.x, pos.y))
                            {
                                MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByPos((long)pos.x, (long)pos.y);
                                if (mapObjectInfoEntity != null)
                                {
                                    switch ((RssType) mapObjectInfoEntity.objectType)
                                    {
                                        case RssType.City:
                                            if (mapObjectInfoEntity.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                                            {
                                                CoreUtils.uiManager.ShowUI(UI.s_pop_WorldObjectPlayer, null, m_playerProxy.CurrentRoleInfo);
                                            }
                                            else
                                            {
                                                CoreUtils.uiManager.ShowUI(UI.s_pop_WorldObjectPlayer, null, mapObjectInfoEntity);
                                            }

                                            break;
                                        case RssType.GuildCenter:
                                        case RssType.GuildFortress1:
                                        case RssType.GuildFortress2:
                                        case RssType.GuildFlag:
                                        case RssType.GuildFoodResCenter:
                                        case RssType.GuildWoodResCenter:
                                        case RssType.GuildGoldResCenter:
                                        case RssType.GuildGemResCenter:
                                            CoreUtils.uiManager.ShowUI(UI.s_AllianceBuildInfoTip, null, mapObjectInfoEntity.objectId);
                                            break;
                                        default:
                                        {
                                            Debug.Log("not find type");
                                        }
                                            break;
                                    }
                                }
                                else
                                {
                                    Debug.Log("未能发现目标");
                                }
                            }
                            else
                            {
                                Debug.Log("移动摄像头过程被打断");
                            }
                        }
                        else
                        {
                            Debug.Log("移动摄像头过程被打断");
                        }
                    });
                });
            }
        }


        private void OnMoveAndOpenCity(int troopId)
        {
            long rallyDetailEntityRid = m_RallyTroopsProxy.GetRallyDetailEntityByarmIndex(troopId);
            if (rallyDetailEntityRid != 0)
            {
                MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByRid(rallyDetailEntityRid);
                if (mapObjectInfoEntity != null)
                {
                    float dxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                    WorldCamera.Instance().SetCameraDxf(dxf, 500, () =>
                    {
                        WorldCamera.Instance().ViewTerrainPos(mapObjectInfoEntity.objectPos.x / 100, mapObjectInfoEntity.objectPos.y / 100, 500, () =>
                        {
                            if (mapObjectInfoEntity.cityRid == m_playerProxy.CurrentRoleInfo.rid)
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_pop_WorldObjectPlayer, null, m_playerProxy.CurrentRoleInfo);
                            }
                            else
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_pop_WorldObjectPlayer, null, mapObjectInfoEntity);
                            }
                        });
                    });
                }
            }
        }

        #region 错误码提示

        private void OnRallyJoinErrorCode(INotification notification)
        {
            if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
            {
                ErrorMessage error = (ErrorMessage) notification.Body;
                switch ((ErrorCode) @error.errorCode)
                {
                    case ErrorCode.RALLY_OVER_MAX_MASS_TROOPS:
                        Tip.CreateTip(730268).Show();
                        break;
                    case ErrorCode.GUILD_NOT_EXIST:
                        Tip.CreateTip(732019).Show();
                        break;
                    case ErrorCode.RALLY_ACTION_FORCE_NO_ENOUGH:
                        TroopHelp.OnClickAddActionPoint();
                        break;
                    case ErrorCode.RALLY_NO_PATH_TO_TARGET:
                    case ErrorCode.RALLY_PATH_NOT_FOUND:
                        Tip.CreateTip(730263).Show();
                        break;
                    case ErrorCode.RALLY_JOIN_NOT_FOUND:
                    case ErrorCode.RALLY_JOIN_ON_ARMY_MATCH:
                        Tip.CreateTip(730372).Show();
                        break;
                }
            }
        }


        private void OnRoleCreateArmyAndMapMarchAndRallLaunch(INotification notification)
        {
            if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
            {
                ErrorMessage error = (ErrorMessage) notification.Body;
                switch ((ErrorCode) @error.errorCode)
                {
                    case ErrorCode.MAP_ATTACK_SHILED:
                    case ErrorCode.RALLY_TARGET_IN_SHIELD:
                        Tip.CreateTip(181187).Show();
                        break;
                    case ErrorCode.RALLY_GUILD_HAD_RALLY:
                        Tip.CreateTip(730261).Show();
                        break;
                    case ErrorCode.GUILD_NOT_EXIST:
                        Tip.CreateTip(732019).Show();
                        break;
                    case ErrorCode.RALLY_OVER_MAX_MASS_TROOPS:
                        Tip.CreateTip(730268).Show();
                        break;
                    case ErrorCode.RALLY_TARGET_NOT_BORDER:
                        Tip.CreateTip(732028).Show();
                        break;
                    case ErrorCode.RALLY_NOT_SAME_GUILD:
                        Tip.CreateTip(732019).Show();
                        break;
                    case ErrorCode.ROLE_ARMY_LOAD_FULL:
                        Tip.CreateTip(500208).Show();
                        break;
                    case ErrorCode.RALLY_REINFORCE_CITY_HAD_REINFORCE:
                        Tip.CreateTip(730274).Show();
                        break;
                    case ErrorCode.RALLY_SAME_GUILD:
                        Tip.CreateTip(730361).Show();
                        break;
                    case ErrorCode.RALLY_NOT_ALLIANCE_CENTER:
                        Tip.CreateTip(730352).Show();
                        break;
                    case ErrorCode.ROLE_RESOURCE_NO_TECHNOLOGY:
                        Tip.CreateTip(732099).Show();
                        break;
                    case ErrorCode.RALLY_NO_PATH_TO_TARGET:
                    case ErrorCode.RALLY_PATH_NOT_FOUND:
                        Tip.CreateTip(730263).Show();
                        break;
                    case ErrorCode.MAP_MARCH_PATH_NOT_FOUND:
                        Tip.CreateTip(730263).Show();
                        break;
                    case ErrorCode.ROLE_SOLDIER_NOT_ENOUGH:
                        Tip.CreateTip(200114).Show();
                        break;
                    case ErrorCode.RALLY_TARGET_NOT_FOUND:
                        Tip.CreateTip(200001).Show();
                        break;
                    case ErrorCode.MAP_MULTI_ARMY_SAME_TARGET:
                        Tip.CreateTip(200116).Show();
                        break;
                }
            }
        }


        private void OnRallyReinforceErrorCode(INotification notification)
        {
            if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
            {
                ErrorMessage error = (ErrorMessage) notification.Body;
                switch ((ErrorCode) @error.errorCode)
                {
                    case ErrorCode.MAP_GUILD_BUILD_ARMY_LIMIT:
                        Tip.CreateTip(732020).Show();
                        break;
                    case ErrorCode.MAP_REINFORCE_NOT_SELF_GUILD:
                        Tip.CreateTip(732019).Show();
                        break;

                    case ErrorCode.RALLY_ALLIANCE_CENTER_ARMY_LIMIT:
                    case ErrorCode.RALLY_REINFORCE_CITY_FAIL:
                    case ErrorCode.RALLY_REINFORCE_CITY_FAIL_ARMY_FULL:
                        Tip.CreateTip(730275).Show();
                        break;
                    case ErrorCode.RALLY_HAD_JOIN_TARGET_REINFORCE:
                        // Tip.CreateTip(730359).Show();   
                        break;
                    case ErrorCode.GUILD_NOT_EXIST:
                        Tip.CreateTip(732019).Show();
                        break;
                    case ErrorCode.RALLY_OVER_MAX_MASS_TROOPS:
                        Tip.CreateTip(730268).Show();
                        break;
                    case ErrorCode.GUILD_CREATE_BUILD_LEVEL_ERROR:
                        int level = m_ConfigDefine.allianceResourcePointReqLevel;
                        string des = LanguageUtils.getTextFormat(732023, level);
                        Tip.CreateTip(des).Show();
                        break;
                    case ErrorCode.MAP_ALREADY_REINFORCE_GUILD:
                        string name = LanguageUtils.getText(TroopHelp.m_ToouchAllianceName);
                        string des1 = LanguageUtils.getTextFormat(732021, name);
                        Tip.CreateTip(des1).Show();
                        break;
                    case ErrorCode.GUILD_CENTER_ALREADY_ARMY_COLLECT:
                        string name2 = LanguageUtils.getText(TroopHelp.m_ToouchAllianceName);
                        string des3 = LanguageUtils.getTextFormat(732026, name2);
                        Tip.CreateTip(des3).Show();
                        break;

                    case ErrorCode.RALLY_NOT_SAME_GUILD:
                        Tip.CreateTip(732019).Show();
                        break;
                    case ErrorCode.RALLY_NOT_ALLIANCE_CENTER:
                        Tip.CreateTip(730352).Show();
                        break;
                    case ErrorCode.ROLE_ARMY_LOAD_FULL:
                        Tip.CreateTip(500208).Show();
                        break;
                    case ErrorCode.MAP_MARCH_PATH_NOT_FOUND:
                    case ErrorCode.RALLY_NO_PATH_TO_TARGET:
                    case ErrorCode.RALLY_PATH_NOT_FOUND:
                        Tip.CreateTip(730263).Show();
                        break;
                    case ErrorCode.ROLE_RESOURCE_NO_TECHNOLOGY:
                        Tip.CreateTip(732099).Show();
                        break;
                    case ErrorCode.RALLY_REINFORCE_CITY_HAD_REINFORCE:
                        Tip.CreateTip(730274).Show();
                        break;
                    case ErrorCode.RALLY_CANT_MULTI_REINFORCE_ARMY:
                        Tip.CreateTip(200003).Show();
                        break;
                    case ErrorCode.RALLY_CANT_MULTI_REINFORCE_CITY:
                        Tip.CreateTip(730274).Show();
                        break;
                    case ErrorCode.RALLY_CANT_MULTI_SAME_OBJECT:
                        Tip.CreateTip(200116).Show();
                        break;
                }
            }
        }

        private void OnRally_RepatriationRallyErrorCode(INotification notification)
        {
            if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
            {
                ErrorMessage error = (ErrorMessage) notification.Body;
                switch ((ErrorCode) error.errorCode)
                {
                    case ErrorCode.RALLY_REPARTRIATION_TEAM_LEAVE:                      
                        Tip.CreateTip(730373).Show();
                        break;
                }
            }
        }

        #endregion


        private void CreateTroops(int id)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().AddTroop(id,
                    () =>
                    {
                        // 首次地图物件变更不会刷新数据
                        // 在此补充刷新攻击受击关系
                        WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateTarget(id, armyData.targetId);

                        WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(id, (int) armyData.armyStatus);
                        WorldMapLogicMgr.Instance.MapSelectEffectHandler.Play(id, armyData.targetId);
//                        WorldMapLogicMgr.Instance.BattleSoundHandler.AddBattleAttackSound(armyData.go,
//                            (tmp) => { armyData.m_attackAudioHandler = tmp; });

                        WorldMapLogicMgr.Instance.BattleSoundHandler.AddBattleMoveSound(armyData.go,
                            (tmp) =>
                            {
                                armyData.m_moveAudioHandler = tmp;
                                if (tmp != null && armyData.state == Troops.ENMU_SQUARE_STAT.MOVE)
                                {
                                    CoreUtils.audioService.SetHandlerVolume(tmp, 1f);
                                }
                            });

                        AutoMoveMgr.Instance.Insert(armyData.objectId);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateShottTextHud, id);
                    });
            }
        }

        #endregion
    }
}