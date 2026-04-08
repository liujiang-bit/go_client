// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    RssProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Data;
using DG.Tweening;
using Hotfix;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public enum RssType
    {
        None = 0,
        Troop = 1, //军队
        Monster = 2,//怪物
        City = 3,//城市
        Stone = 4,//石料
        Farmland = 5,//农田
        Wood = 6, //木材
        Gold = 7, //金矿
        Gem = 8, //宝石
        Scouts = 9,//斥候
        Village = 10,//村庄
        Cave = 11,//山洞
        GuildCenter = 12, //12 联盟中心要塞
        GuildFortress1 = 13, //联盟要塞1
        GuildFortress2 = 14,//联盟要塞2
        GuildFlag = 15, //15 联盟旗帜
        GuildFood = 16,//16 联盟农田
        GuildWood = 17, //联盟伐木场
        GuildStone = 18, //联盟石矿床
        GuildGold = 19, //联盟金矿床
        GuildFoodResCenter = 20,//20 联盟谷仓  资源中心
        GuildWoodResCenter = 21, //联盟木料场
        GuildGoldResCenter = 22, //联盟石材厂
        GuildGemResCenter = 23, //23联盟铸币场
        Rune = 24,  //符文
        CheckPoint = 25, //关卡
        HolyLand = 26, // 圣地
        Transport = 27, //运输车
        BarbarianCitadel = 28, //野蛮人城寨
        Guardian = 29, //圣地守护者
        Expedition = 30, //远征内对象 
        Sanctuary = 31,//圣所
        Altar = 32,//圣坛
        Shrine = 33,//圣祠
        LostTemple = 34,//圣庙
        Checkpoint_1 = 35,//关卡1
        Checkpoint_2 = 36,//关卡2
        Checkpoint_3 = 37,//关卡3
        SummonAttackMonster = 42,//召唤类型单人挑战怪物
        SummonConcentrateMonster = 43,//召唤类型集结挑战怪物
    }

    public enum RssPointState
    {
        None = 0,

        /// <summary>
        /// 未采集
        /// </summary>
        Uncollected,

        /// <summary>
        /// 自己采集
        /// </summary>
        CollectedByMe,

        /// <summary>
        /// 盟友采集
        /// </summary>
        CollectedByally,

        /// <summary>
        /// 不是盟友
        /// </summary>
        CollectedNoByally
    }


    public class RssProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "RssProxy";

        /// <summary>
        /// map资源点数据字典
        /// </summary>
        private MonsterProxy m_MonsterProxy;

        private PlayerProxy m_PlayerProxy;
        private CityBuildingProxy m_buildingProxy;
        private WorldMapObjectProxy m_worldMapProxy;
        private TroopProxy m_TroopProxy;
        private SearchProxy m_SearchProxy;

        private int slectTroopId;
        private bool isCd = false;
        private ArmyData armySelectData;
        private int m_SearchResourceLevel;
        private int m_searchCameraMoveSpeed;

        public int OldLevel = 0;
        public SearchType OldSearchType = SearchType.None;

        private Timer m_findSearchObjectTimer = null;
        private int CONSTCD = 0;

        private List<BarbarianPosInfoEntity> m_barianPosEntity = new List<BarbarianPosInfoEntity>();
        private int m_nCurrentLevel = -1;
        private int m_nCurrentBarbarianIndex = 0;
        private float m_lastBarbarianTime = 0.0f;
        #endregion

        public RssProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" RssProxy register");
        }

        public void Init()
        {
            m_MonsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_buildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_worldMapProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_SearchProxy = AppFacade.GetInstance().RetrieveProxy(SearchProxy.ProxyNAME) as SearchProxy;

            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_searchCameraMoveSpeed = config.cameraMoveTime;
            CONSTCD = config.resourceGatherSearchCd;
        }

        public override void OnRemove()
        {
            CancleFindSearchObjectTimer();
        }

        #region 场景上资源点数据创建入口

        public void UpdateRss(MapObjectInfoEntity mapItemInfo)
        {
            if (mapItemInfo != null)
            {
                if (mapItemInfo.collectRid > 0)
                {
                    if (m_PlayerProxy.CurrentRoleInfo.rid == mapItemInfo.collectRid)
                    {
                        Debug.LogWarning("开始采集这个资源点了" + mapItemInfo.objectId);
                        if (!string.IsNullOrEmpty(mapItemInfo.cityName))
                        {
                            mapItemInfo.name = mapItemInfo.cityName;
                        }
                        mapItemInfo.rssPointStateType = RssPointState.CollectedByMe;
                    }
                    else if (m_PlayerProxy.CurrentRoleInfo.guildId == mapItemInfo.guildId && mapItemInfo.guildId != 0)
                    {
                        mapItemInfo.rssPointStateType = RssPointState.CollectedByally;
                        mapItemInfo.name = mapItemInfo.cityName;
                    }
                    else
                    {
                        mapItemInfo.rssPointStateType = RssPointState.CollectedNoByally;
                        mapItemInfo.name = mapItemInfo.cityName;
                    }
                }
                else
                {
                    mapItemInfo.rssPointStateType = RssPointState.Uncollected;
                    mapItemInfo.name = "";
                }

                mapItemInfo.isShowHud = true;
                mapItemInfo.showHudIcon = GetHudUIIconByRssPointState(mapItemInfo.rssPointStateType);
            }
        }

        public void UpdateResourcePoint(MapObjectInfoEntity mapItemInfo)
        {
            if (mapItemInfo != null)
            {
                mapItemInfo.isShowHud = true;
                if (m_PlayerProxy != null)
                {
                    if (mapItemInfo.objectType == (int)RssType.Cave)
                    {
                        mapItemInfo.showHudIcon = m_PlayerProxy.ConfigDefine.canveFlag;
                    }
                    else if (mapItemInfo.objectType == (int)RssType.Village)
                    {
                        mapItemInfo.showHudIcon = m_PlayerProxy.ConfigDefine.villageFlag;
                    }
                }
            }
        }

        public void UpdateViallAgeData(int id, int viallAgeId)
        {
            MapObjectInfoEntity data = m_worldMapProxy.GetWorldMapObjectByobjectId(id);
            if (data != null)
            {
                VillageRewardDataDefine villageRewardDataDefine =
                    CoreUtils.dataService.QueryRecord<VillageRewardDataDefine>(viallAgeId);
                if (villageRewardDataDefine == null)
                {
                    Debug.LogWarningFormat("找不到这个ID的奖励表", viallAgeId);
                    return;
                }

                data.villageRewardDataDefine = villageRewardDataDefine;
                CoreUtils.uiManager.ShowUI(UI.s_Pop_WorldObjectVillageCave, null, data.objectId);
            }
        }
        /// <summary>
        /// 分享名
        /// </summary>
        public string GetShareName( MapObjectInfoEntity mapObjectInfoEntity )
        {
            string chatShareName = "";
            switch (mapObjectInfoEntity.rssType)
            {
                case RssType.City:
                    {
                        chatShareName = mapObjectInfoEntity.cityName;
                    }
                    break;
                case RssType.Stone://石料
                case RssType.Farmland://农田
                case RssType.Wood://木材
                case RssType.Gold: //金矿
                case RssType.Gem: //宝石
                    {
                        chatShareName = "";
                    }
                    break;
                case RssType.CheckPoint:
                case RssType.HolyLand:
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                case RssType.Checkpoint_1://关卡1
                case RssType.Checkpoint_2://关卡2
                case RssType.Checkpoint_3://关卡3
                    {
                        chatShareName = "";
                    }
                    break;
                case RssType.GuildCenter:
                case RssType.GuildFortress1: //联盟要塞1
                case RssType.GuildFortress2://联盟要塞2
                case RssType.GuildFlag: //15 联盟旗帜
                case RssType.GuildFood://16 联盟农田
                case RssType.GuildWood://联盟伐木场
                case RssType.GuildStone: //联盟石矿床
                case RssType.GuildGold://联盟金矿床
                case RssType.GuildFoodResCenter://20 联盟谷仓  资源中心
                case RssType.GuildWoodResCenter://联盟木料场
                case RssType.GuildGoldResCenter: //联盟石材厂
                case RssType.GuildGemResCenter://23联盟铸币场
                    {
                        chatShareName = "";
                    }
                    break;
                case RssType.Monster:
                case RssType.BarbarianCitadel:
                case RssType.SummonAttackMonster:
                case RssType.SummonConcentrateMonster:
                    {
                        chatShareName = "";
                    }
                    break;
                case RssType.None:
                case RssType.Troop:  //军队
                                     // case RssType.Monster: //怪物
                                     //    case RssType.City: //城市
                                     //     case RssType.Stone: //石料
                                     //     case RssType.Farmland: //农田
                                     //   case RssType.Wood: //木材
                                     //      case RssType.Gold:  //金矿
                                     //       case RssType.Gem:  //宝石
                case RssType.Scouts: //斥候
                case RssType.Village: //村庄
                case RssType.Cave: //山洞
                                   //   case RssType.GuildCenter:  //12 联盟中心要塞
                                   //      case RssType.GuildFortress1: //联盟要塞1
                                   //       case RssType.GuildFortress2: //联盟要塞2
                                   //       case RssType.GuildFlag: //15 联盟旗帜
                                   //     case RssType.GuildFood: //16 联盟农田
                                   //        case RssType.GuildWood:  //联盟伐木场
                                   //           case RssType.GuildStone:  //联盟石矿床
                                   //        case RssType.GuildGold:  //联盟金矿床
                                   //      case RssType.GuildFoodResCenter: //20 联盟谷仓  资源中心
                                   //   case RssType.GuildWoodResCenter:  //联盟木料场
                                   //      case RssType.GuildGoldResCenter:  //联盟石材厂
                                   //        case RssType.GuildGemResCenter:  //23联盟铸币场
                case RssType.Rune:  //符文
                                    //        case RssType.CheckPoint: //关卡
                                    //        case RssType.HolyLand: // 圣地
                case RssType.Transport: //运输车
                                        //       case RssType.BarbarianCitadel:  //野蛮人城寨
                case RssType.Guardian: //圣地守护者
                case RssType.Expedition:  //远征内对象 
                                          //     case RssType.Sanctuary: //圣所
                                          //        case RssType.Altar: //圣坛
                                          //       case RssType.Shrine: //圣祠
                                          //          case RssType.LostTemple: //圣庙
                                          //      case RssType.Checkpoint_1: //关卡1
                                          //             case RssType.Checkpoint_2: //关卡2
                                          //         case RssType.Checkpoint_3: //关卡3
                                          //           case RssType.SummonAttackMonster: //召唤类型单人挑战怪物
                                          //        case RssType.SummonConcentrateMonster: //召唤类型集结挑战怪物
                    {
                        Debug.LogError("未处理的类型");
                    }
                    break;
            }
            return chatShareName;
        }
        public void SetViallAgeState(MapObjectInfoEntity rssItemData)
        {
            if (rssItemData == null)
            {
                return;
            }

            if (rssItemData.objectType == (long)RssType.Village || rssItemData.objectType == (long)RssType.Cave)
            {
                int index = (int)Math.Ceiling(rssItemData.resourcePointId / 64f);
                long bitIndex = rssItemData.resourcePointId % 64;

                foreach (var villageCaves in m_PlayerProxy.CurrentRoleInfo.villageCaves.Values)
                {
                    if (villageCaves.index == index)
                    {
                        rssItemData.villageState = GetState(villageCaves.rule, (int)bitIndex);
                        // Debug.LogError("领取状态" + rssItemData.villageState +"***" + index + bitIndex + "***" + rssItemData.objectId + "***"+villageCaves.index+"***"+rssItemData.resourcePointId);
                    }
                }
            }
        }

        private bool GetState(long num, int index)
        {
            return (num & (1L << (index))) != 0;
        }


        public void OpenUI(string rssGo)
        {
            string[] s = rssGo.Split('_');
            int id = int.Parse(s[1]);
            switch (s[0])
            {
                case "RssItem":
                    MapObjectInfoEntity rssItemData = m_worldMapProxy.GetWorldMapObjectByobjectId(id);
                    if (rssItemData != null)
                    {
                        GlobalViewLevelMediator m =
                            AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                                GlobalViewLevelMediator;
                        if (!m.IsLodVisable(rssItemData.objectPos.x / 100, rssItemData.objectPos.y / 100))
                        {
                            return;
                        }

                        if (rssItemData.rssType == RssType.Village || rssItemData.rssType == RssType.Cave)
                        {
                            if (rssItemData.villageState)
                            {
                                Debug.LogWarning("已经领取过这个奖励了" + rssItemData.villageState);
                                if (!CoreUtils.uiManager.ExistUI(UI.s_Pop_WorldObjectVillageCave))
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_Pop_WorldObjectVillageCaveFinish, null, rssItemData.objectId);
                                }
                                return;
                            }


                            Debug.LogWarning("还没有领取过这个奖励了" + rssItemData.villageState + "***" + rssItemData.objectId +
                                             "***" + rssItemData.rssType);
                            if (rssItemData.rssType == RssType.Village)
                            {
                                SendMapScountVillage(rssItemData.objectId);
                            }
                            else if (rssItemData.rssType == RssType.Cave)
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_Pop_WorldObjectVillageCave, null, rssItemData.objectId);
                            }
                        }
                        else
                        {
                            int resType = rssItemData.resourceGatherTypeDefine.type;
                            //前置条件是否满足
                            if (rssItemData.resourceGatherTypeDefine.scienceReq > 0 &&
                                !m_PlayerProxy.IsTechnologyUnlockByType(rssItemData.resourceGatherTypeDefine.scienceReq)
                            )
                            {
                                StudyDefine define =
                                    CoreUtils.dataService.QueryRecord<StudyDefine>((int)rssItemData
                                        .resourceGatherTypeDefine
                                        .scienceLevReq);
                                if (define == null)
                                {
                                    Debug.LogErrorFormat("StudyDefine not find {0}",
                                        rssItemData.resourceGatherTypeDefine.scienceLevReq);
                                    return;
                                }

                                //功能介绍引导
                                if (resType == (int)EnumResType.Stone || resType == (int)EnumResType.Gold || resType == (int)EnumResType.Diamond)
                                {
                                    var funcGuideProxy = AppFacade.GetInstance().RetrieveProxy(FuncGuideProxy.ProxyNAME) as FuncGuideProxy;
                                    if (resType == (int)EnumResType.Stone)
                                    {
                                        if (!funcGuideProxy.IsCompletedByStage((int)EnumFuncGuide.CollectStone))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.CollectStone);
                                            return;
                                        }
                                    }
                                    else if (resType == (int)EnumResType.Gold)
                                    {
                                        if (!funcGuideProxy.IsCompletedByStage((int)EnumFuncGuide.CollectGold))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.CollectGold);
                                            return;
                                        }
                                    }
                                    else if (resType == (int)EnumResType.Diamond)
                                    {
                                        if (!funcGuideProxy.IsCompletedByStage((int)EnumFuncGuide.CollectDenar))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.CollectDenar);
                                            return;
                                        }
                                    }
                                }

                                Tip.CreateTip(LanguageUtils.getTextFormat(500101,
                                        LanguageUtils.getText(define.l_nameID),
                                        LanguageUtils.getText(rssItemData.resourceGatherTypeDefine.l_nameId)),
                                    Tip.TipStyle.Middle).Show();
                                return;
                            }

                            CoreUtils.uiManager.ShowUI(UI.s_Pop_WorldInfo, null, rssGo);
                            AppFacade.GetInstance().SendNotification(CmdConstant.MapUnSelectCity);
                        }
                    }

                    break;
            }
        }

        //播放选中音效
        private void PlaySelectAudio(RssType type)
        {
            switch (type)
            {
                case RssType.Gold:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_SelectGold");
                    break;
                case RssType.Stone:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_SelectStone");
                    break;
                case RssType.Wood:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_SelectWood");
                    break;
                case RssType.Farmland:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_SelectFarm");
                    break;
                default:
                    break;
            }
        }

        private string tmpLastStr;

        public void OpenMonsterUI(int id)
        {
            CoreUtils.audioService.PlayOneShot("Sound_Ui_SelectBarbarian");
            if (m_MonsterProxy == null)
            {
                Debug.LogWarning("MonsterProxy为空！");
                m_MonsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            }

            MapObjectInfoEntity monsterData = m_worldMapProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null)
            {
                if (!GuideProxy.IsGuideing)
                {
                    GlobalViewLevelMediator m = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;
                    if (monsterData.gameobject != null)
                    {
                        if (!m.IsLodVisable(monsterData.gameobject.transform.position.x, monsterData.gameobject.transform.position.z))
                        {
                            return;
                        }
                    }
                }

                AppFacade.GetInstance().SendNotification(CmdConstant.GuideClickMonster);
                AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveTroopSelectHud);
                CoreUtils.uiManager.ShowUI(UI.s_pop_WorldMonster, null, id);
            }
        }


        #region 资源点搜索相关回包处理

        private List<Map_SearchResource.response.ResourcePosInfo> lsPos =
            new List<Map_SearchResource.response.ResourcePosInfo>(); //服务器下发的资源田数据

        private List<Map_SearchResource.response.ResourcePosInfo> hasSearchPos =
            new List<Map_SearchResource.response.ResourcePosInfo>(); //已搜索的资源田数据

        public void SearchRssCallBack(INotification notification)
        {
            Map_SearchResource.response info = notification.Body as Map_SearchResource.response;
            if (info != null)
            {
                int resourceId = ((int)info.resourceType * 10000 + OldLevel);
                ResourceGatherTypeDefine ResourceGatherTypeDefine =
                    CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(resourceId);
                if (info.resources.Count > 0)
                {
                    OpenTimeCd(m_SearchProxy.searchType, m_SearchProxy.currCurrLevel);
                    lsPos.Clear();
                    hasSearchPos.Clear();
                    foreach (var item in info.resources)
                    {
                        lsPos.Add(item);
                    }

                    List<Vector2> Collecting = IsCollecting();
                    Map_SearchResource.response.ResourcePosInfo v2 = GetNearestDistance(m_SearchResourceLevel,
                        Collecting);
                    float x = v2.pos.x / 100f;
                    float y = v2.pos.y / 100f;

                    WorldCamera.Instance().ViewTerrainPos(x,
                        y, m_searchCameraMoveSpeed, () =>
                        {
                            FogSystemMediator fogMedia =
                                AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as
                                    FogSystemMediator;
                            if (fogMedia.HasFogAtWorldPos(x, y))
                            {
                                SearchProxy searchProxy =
                                    AppFacade.GetInstance().RetrieveProxy(SearchProxy.ProxyNAME) as SearchProxy;
                                Tip.CreateTip(
                                    LanguageUtils.getTextFormat(500102,
                                        searchProxy.GetNameByType((SearchType)(info.resourceType)),
                                        LanguageUtils.getText(170014)), Tip.TipStyle.Middle).Show();
                            }
                            else
                            {
                                CancleFindSearchObjectTimer();
                                if (!ShowFingerAttachObject(v2.objectId))
                                {
                                    CreateShowFingerTimer(v2.objectId);
                                }
                            }
                        });
                    float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                    WorldCamera.Instance().SetCameraDxf(Firstdxf, m_searchCameraMoveSpeed, null);
                    SendMapMove((long)x, (long)y);
                    hasSearchPos.Add(v2);
                }
                else
                {
                    if (ResourceGatherTypeDefine != null)
                    {
                        string str = LanguageUtils.getTextFormat(500100,
                            LanguageUtils.getText(ResourceGatherTypeDefine.l_nameId));
                        Tip.CreateTip(str, Tip.TipStyle.Middle).Show();
                    }
                }
            }
        }

        private void CancleFindSearchObjectTimer()
        {
            if (m_findSearchObjectTimer != null)
            {
                m_findSearchObjectTimer.Cancel();
                m_findSearchObjectTimer = null;
            }
        }

        private void CreateShowFingerTimer(long searchObjectId)
        {
            CancleFindSearchObjectTimer();
            m_findSearchObjectTimer = Timer.Register(0, null, (time) =>
            {
                if (ShowFingerAttachObject(searchObjectId))
                {
                    CancleFindSearchObjectTimer();
                }
            }, true);
        }

        private bool ShowFingerAttachObject(long searchObjectId)
        {
            MapObjectInfoEntity obj = m_worldMapProxy.GetWorldMapObjectByobjectId(searchObjectId);
            if (obj != null && obj.gameobject != null)
            {
                FingerTargetParam param = new FingerTargetParam();
                param.AreaTarget = obj.gameobject;
                param.NodeType = 2;
                param.ArrowDirection = (int)EnumArrorDirection.Up;
                CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在采集或采集行军中
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<Vector2> IsCollecting()
        {
            List<Vector2> IsCollecting = new List<Vector2>();
            var armies = m_TroopProxy.GetArmys();
            foreach (var ArmyInfo in armies)
            {
                if (TroopHelp.IsHaveState(ArmyInfo.status, ArmyStatus.COLLECT_MARCH))
                {
                    int count = ArmyInfo.path.Count;
                    IsCollecting.Add(new Vector2(ArmyInfo.path[count - 1].x / 100, ArmyInfo.path[count - 1].y / 100));
                }
                else if (TroopHelp.IsHaveState(ArmyInfo.status, ArmyStatus.COLLECTING))
                {
                    // 之前有报错，我临时处理一下容错 【【BUG】版本1.1.11，客户端搜索资源报错，搜索野蛮人不会报错】https://www.tapd.cn/64603723/bugtrace/bugs/view?bug_id=1164603723001020012
                    if (ArmyInfo.collectResource.pos != null)
                    {
                        IsCollecting.Add(new Vector2(ArmyInfo.collectResource.pos.x / 100,
                            ArmyInfo.collectResource.pos.y / 100));
                    }
                    else if (ArmyInfo.targetArg != null && ArmyInfo.targetArg.pos != null)
                    {
                        IsCollecting.Add(new Vector2(ArmyInfo.targetArg.pos.x / 100,
                            ArmyInfo.targetArg.pos.y / 100));
                    }
                }
            }

            return IsCollecting;
        }

        /// <summary>
        /// 获取最近的位置， （1等级最近 ，2距离最近，）and（没有人在采集）
        /// </summary>
        /// <param name="level"></param>
        /// <param name="Collecting"></param>
        /// <returns></returns>
        private Map_SearchResource.response.ResourcePosInfo GetNearestDistance(int level, List<Vector2> Collecting)
        {
            long x = (long)m_buildingProxy.RolePos.x;
            long y = (long)m_buildingProxy.RolePos.y;
            long resultX = 0, resultY = 0;
            float Distance = 0;

            Map_SearchResource.response.ResourcePosInfo resourcePosInfo = null;
            bool jump = false;
            do
            {
                for (int i = 0; i < lsPos.Count; i++)
                {
                    Map_SearchResource.response.ResourcePosInfo ResourcePosInfo = lsPos[i];
                    Debug.Log(ResourcePosInfo.pos.x + " " + ResourcePosInfo.pos.y + " " +
                              ResourcePosInfo.resourceLevel);
                    jump = false;
                    if (ResourcePosInfo.resourceLevel == level)
                    {
                        for (int j = 0; j < Collecting.Count; j++)
                        {
                            if (Collecting[j].x == ResourcePosInfo.pos.x && Collecting[j].y == ResourcePosInfo.pos.y)
                            {
                                Debug.LogFormat("{0}", "有人采集");
                                jump = true;
                                break;
                            }
                        }

                        for (int j = 0; j < hasSearchPos.Count; j++)
                        {
                            if (hasSearchPos[j].pos.x == ResourcePosInfo.pos.x &&
                                hasSearchPos[j].pos.y == ResourcePosInfo.pos.y)
                            {
                                jump = true;
                                break;
                            }
                        }

                        if (jump)
                        {
                            continue;
                        }

                        float result = (Mathf.Pow(ResourcePosInfo.pos.x / 100 - x, 2) +
                                        Mathf.Pow(ResourcePosInfo.pos.y / 100 - y, 2));
                        if (Distance == 0)
                        {
                            Distance = result;
                            resourcePosInfo = ResourcePosInfo;
                        }

                        if (result < Distance)
                        {
                            Distance = result;
                            resourcePosInfo = ResourcePosInfo;
                        }
                    }
                }

                level++;
            } while (Distance == 0 && level <= 6);

            return resourcePosInfo;
        }

        public void SearchBarbarianCallBack(INotification notification)
        {
            Map_SearchBarbarian.response info = notification.Body as Map_SearchBarbarian.response;
            if (info != null)
            {
                m_barianPosEntity.Clear();
                if (info.barbarians != null && info.barbarians.Count > 0)
                {
                    for (int i = 0; i < info.barbarians.Count; i++)
                    {
                        var entity = new BarbarianPosInfoEntity();
                        BarbarianPosInfoEntity.updateEntity(entity, info.barbarians[i]);
                        m_barianPosEntity.Add(entity);
                    }
                }

                var rolePos = new Vector2Int((int)m_PlayerProxy.CurrentRoleInfo.pos.x, (int)m_PlayerProxy.CurrentRoleInfo.pos.y);
                m_barianPosEntity.Sort((BarbarianPosInfoEntity x, BarbarianPosInfoEntity y) =>
                {
                    float far1 = Vector2Int.Distance(rolePos, new Vector2Int((int)x.pos.x, (int)x.pos.y));
                    float far2 = Vector2Int.Distance(rolePos, new Vector2Int((int)y.pos.x, (int)y.pos.y));

                    return far1.CompareTo(far2);
                });
                m_nCurrentBarbarianIndex = 0;
                SearchNearlyBarbarian(info.level);
            }
        }
        public void SearchNearlyBarbarian(long nLevel)
        {
            if (m_barianPosEntity.Count > 0)
            {
                Debug.LogWarning("搜索到了野蛮人");
                CoreUtils.uiManager.CloseUI(UI.s_iF_SearchRes);
                if (m_nCurrentBarbarianIndex >= m_barianPosEntity.Count)
                {
                    m_nCurrentBarbarianIndex = 0;
                }
                var entity = m_barianPosEntity[m_nCurrentBarbarianIndex];
                m_nCurrentBarbarianIndex++;
                float x = entity.pos.x / 100f;
                float y = entity.pos.y / 100f;
                WorldCamera.Instance().ViewTerrainPos(x, y, m_searchCameraMoveSpeed,
                    () => { });

                float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                WorldCamera.Instance().SetCameraDxf(Firstdxf, m_searchCameraMoveSpeed, () =>
                {
                    FogSystemMediator fogMedia =
                        AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as
                            FogSystemMediator;
                    if (fogMedia.HasFogAtWorldPos(x, y))
                    {
                        Tip.CreateTip(LanguageUtils.getTextFormat(500102, LanguageUtils.getText(500200),
                            LanguageUtils.getText(170011)), Tip.TipStyle.Middle).Show();
                        return;
                    }
                    CancleFindSearchObjectTimer();
                    if (!ShowFingerAttachObject(entity.objectId))
                    {
                        CreateShowFingerTimer(entity.objectId);
                    }
                });
                SendMapMove((long)x, (long)y);
            }
            else
            {
                int id = (int)(1000 + nLevel);
                MonsterDefine monster = CoreUtils.dataService.QueryRecord<MonsterDefine>(id);
                if (monster != null)
                {
                    string tips = LanguageUtils.getTextFormat(500100, LanguageUtils.getText(monster.l_nameId));
                    Tip.CreateTip(tips, Tip.TipStyle.Middle).Show();
                }
                else
                {
                    Debug.LogErrorFormat("not find level{0} ", id);
                }
            }
        }

        public void SearchRssDef()
        {
            if (lsPos.Count > 0)
            {
                List<Vector2> Collecting = IsCollecting();
                Map_SearchResource.response.ResourcePosInfo v2 = GetNearestDistance(m_SearchResourceLevel, Collecting);
                if (v2 == null)
                {
                    return;
                }

                float x = v2.pos.x / 100f;
                float y = v2.pos.y / 100f;

                WorldCamera.Instance().ViewTerrainPos(x, y, m_searchCameraMoveSpeed, null);
                float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                WorldCamera.Instance().SetCameraDxf(Firstdxf, m_searchCameraMoveSpeed, null);
                SendMapMove((long) x, (long) y);
                hasSearchPos.Add(v2);
            }
        }

        #endregion

        private Vector2 m_rssVec;

        public Vector2 GetV2(int rssId)
        {
            if (rssId == 0)
            {
                return m_TroopProxy.GetTroopMoveSpacePos();
            }

            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(rssId);
            if (armyData != null && armyData.go != null && armyData.go.gameObject != null)
            {
                m_rssVec.Set(armyData.go.gameObject.transform.position.x, armyData.go.gameObject.transform.position.z);
                return m_rssVec;
            }
            
 
            MapObjectInfoEntity monsterData = m_worldMapProxy.GetWorldMapObjectByobjectId(rssId);
            if (monsterData != null)
            {
                if (monsterData.rssType == RssType.Monster ||
                    monsterData.rssType == RssType.Guardian ||
                    monsterData.rssType == RssType.SummonAttackMonster ||
                    monsterData.rssType == RssType.SummonConcentrateMonster)
                {
                    Vector2 startPos = new Vector2(monsterData.gameobject.transform.position.x,
                        monsterData.gameobject.transform.position.z);
                    return startPos;
                }
                else
                {
                    m_rssVec.Set(monsterData.objectPos.x / 100f, monsterData.objectPos.y / 100f);
                    return m_rssVec;
                }
            }
            return Vector2.zero;
        }


        //获取两点之间的距离
        public double GetDistance(Vector2 pos1, Vector2 pos2)
        {
            double result = Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));
            return result;
        }

        public string GetHudUIIcon(RssType type)
        {
            string icon = "ui_map[img_map_state_collect1]";
            switch (type)
            {
                case RssType.Monster:
                case RssType.Guardian:
                case RssType.SummonAttackMonster:
                case RssType.SummonConcentrateMonster:
                    icon = "ui_map[img_map_state_war]";
                    break;
                case RssType.City:
                    icon = "ui_map[img_map_state_back]";
                    break;
            }

            return icon;
        }

        public string GetHudUIIconByRssPointState(RssPointState type)
        {
            string icon = "";
            switch (type)
            {
                case RssPointState.CollectedByMe:
                    icon = RS.StateCollect1;
                    break;
                case RssPointState.CollectedByally:
                    icon = RS.StateCollect2;
                    break;
                case RssPointState.CollectedNoByally:
                    icon = RS.StateCollect3;
                    break;
            }

            return icon;
        }

        private int cd = 0;
        private Timer timer;

        public void OpenTimeCd(SearchType type, int level)
        {
            cd = 0;
            timer = Timer.Register(1, () =>
            {
                cd += 1;
                isCd = true;
            }, null, true);
        }


        public bool IsCd()
        {
            if (cd >= CONSTCD)
            {
                isCd = false;
                if (timer != null)
                {
                    timer.Cancel();
                }
            }

            return isCd;
        }

        //资源类型负载
        public int GetResWeight(int resType)
        {
            int num = 1;
            if (resType == (int) EnumResType.Food)
            {
                num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).foodRaito;
            }
            else if (resType == (int) EnumResType.Wood)
            {
                num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).woodRaito;
            }
            else if (resType == (int) EnumResType.Stone)
            {
                num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).stoneRaito;
            }
            else if (resType == (int) EnumResType.Gold)
            {
                num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).goldRaito;
            }
            else if (resType == (int) EnumResType.Diamond)
            {
                num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).diamonRaito;
            }

            return num;
        }

        #endregion

        #region 资源点相关协议上发

        public void SendSearchRss(int type, int level)
        {
            this.OldSearchType = (SearchType) type;
            this.OldLevel = level;
            Map_SearchResource.request req = new Map_SearchResource.request();
            req.resourceType = type;
            req.resourceLevel = level;
            AppFacade.GetInstance().SendSproto(req);
            m_SearchResourceLevel = level;
        }

        public void SendSearchBarbarian(int level)
        {
            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if(Time.realtimeSinceStartup - m_lastBarbarianTime > config.monsterSearchCd || level != m_nCurrentLevel)
            {
                Map_SearchBarbarian.request req = new Map_SearchBarbarian.request();
                req.level = level;
                AppFacade.GetInstance().SendSproto(req);
                m_lastBarbarianTime = Time.realtimeSinceStartup;
            }
            else
            {
                SearchNearlyBarbarian(level);
            }
            m_nCurrentLevel = level;
        }

        public void SendMapScountVillage(long targetIndex)
        {
            Map_ScoutVillage.request req = new Map_ScoutVillage.request();
            req.targetIndex = targetIndex;
            AppFacade.GetInstance().SendSproto(req);
        }

        public void SendMapMove(long x, long y)
        {
            // if(m_GlobalViewLevelMediator == null)
            var globalViewLevelMediator =
                    AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                        GlobalViewLevelMediator;
            
            Map_Move.request req = new Map_Move.request();
            req.posInfo = new PosInfo();
            req.posInfo.x = x * 100;
            req.posInfo.y = y * 100;
            req.isPreview = globalViewLevelMediator?.IsStrategic() ?? false;
            AppFacade.GetInstance().SendSproto(req);
        }

        #endregion
    }
}