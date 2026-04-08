// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月11日
// Update Time         :    2020年3月11日
// Class Description   :    TroopHUDMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Client;
using Data;
using DG.Tweening;
using Hotfix;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine.UI;

namespace Game
{
    public enum TroopHudIconType
    {
        Retreat,
        Stationed,
        Attack,
        Investigation,
    }

    public class StanceInfo
    {
        public int troopId;
        public int targetId;
        public int stanceIndex;
    }

    public class TroopHUDMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "TroopHUDMediator";
        private TroopProxy m_TroopProxy;
        private ScoutProxy m_ScoutProxy;
        private PlayerProxy m_PlayerProxy;
        private HeroProxy m_HeroProxy;
        private MonsterProxy m_MonsterProxy;
        private WorldMapObjectProxy m_worldProxy;
        private GuideProxy m_guideProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private AllianceProxy m_AllianceProxy;
        private ExpeditionProxy m_expeditionProxy;
        private FogSystemMediator m_fogMediator;
        private int curObjectId;
        private List<int> curViceObjectIdList;
        private ArmyData curArmyData;
        private MapObjectInfoEntity monsterData;
        private ConfigDefine configInfo;
        private Dictionary<long, FightUiHudView> dicHudUIFight = new Dictionary<long, FightUiHudView>();
        private Dictionary<long, HUDUI> dicHudUI = new Dictionary<long, HUDUI>();
        private Dictionary<long, HUDUI> dicHudSelect = new Dictionary<long, HUDUI>(1);
        private Dictionary<long, HUDUI> dicShootHud = new Dictionary<long, HUDUI>();
        private Dictionary<long, RuneCollectHudView> m_dictRuneHud = new Dictionary<long, RuneCollectHudView>();

        private Color colorGreen;
        private Color colorWhite;
        private Color colorBlue;
        private Color colorRed;

        private float lastCallbackTime;
        private float updateInterval = 1.5f;

        private Dictionary<int, StanceInfo> dicStanceInfo = new Dictionary<int, StanceInfo>();
        private Dictionary<int, Dictionary<int, List<StanceInfo>>> dicTargetStanceInfo = new Dictionary<int, Dictionary<int, List<StanceInfo>>>();

        #endregion

        public TroopHUDMediator() : base(NameMediator, null)
        {

        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.MapCreateTroopHud,
                CmdConstant.MapRemoveTroopHud,
                CmdConstant.MapClearTroopHud,
                CmdConstant.MapCreateTroopSelectHud,
                CmdConstant.MapCloseDoubleSelectTroopHud,
                CmdConstant.MapRemoveTroopSelectHud,
                CmdConstant.MapCreateTroopFightHud,
                CmdConstant.MapRemoveTroopFightHud,
                CmdConstant.MapCreateMonsterFightHud,
                CmdConstant.MapRemoveMonsterFightHud,
                CmdConstant.MapUpdateTroopHud,
                CmdConstant.MapCloseTroopHudScale,
                CmdConstant.MapOpenTroopHudScale,
                CmdConstant.MapPlayShottTextHud,
                CmdConstant.MapStopShottTextHud,
                CmdConstant.MapCreateShottTextHud,
                CmdConstant.MapSetFightBattleUIData,
                CmdConstant.MapCreateBuildingFightHud,
                CmdConstant.MapRemoveBuildingFightHud,
                CmdConstant.MapCreateRuneGatherHud,
                CmdConstant.MapRemoveRuneGatherHud,
                CmdConstant.MapUpdateBuildingHead,
                CmdConstant.AllianceEixt,
                CmdConstant.AllianceJoinUpdate,
                CmdConstant.MapUpdateGuildAddName,
                Guild_ModifyGuildInfo.TagName,
                Role_ModifyName.TagName,
                CmdConstant.MapUpdateArmyName,
                CmdConstant.MapObjectHUDUpdate,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {                
                case CmdConstant.MapCreateTroopHud:
                    {
                        int troopId = (int)notification.Body;
                        CreateTroopHud(troopId);
                    }
                    break;
                case CmdConstant.MapRemoveTroopHud:
                    int troopRemoveId = (int)notification.Body;
                    RemoveHUDByID(troopRemoveId);
                    break;
                case CmdConstant.MapClearTroopHud:
                    ClearTroopHud();
                    break;
                case CmdConstant.MapCreateTroopSelectHud:
                    TouchTroopInfo touchTroopInfo = notification.Body as TouchTroopInfo;
                    CreateSelectHUD(touchTroopInfo.mainArmObjectId, touchTroopInfo.viceArmObjectIdList);
                    UpdatePlayerTroopFightHudNum(touchTroopInfo.mainArmObjectId);
                    break;
                case CmdConstant.MapCloseDoubleSelectTroopHud:
                    CloseTroopDoubleSelectHudUI();
                    break;
                case CmdConstant.MapRemoveTroopSelectHud:
                    RemoveSelectHUD(0);
                    break;
                case CmdConstant.MapCreateTroopFightHud:
                    int createFightId = (int)notification.Body;
                    PlayTroopFightHud(createFightId);
                    break;
                case CmdConstant.MapRemoveTroopFightHud:
                    int RemoveFightId = (int)notification.Body;
                    RemoveTroopFightHud(RemoveFightId);
                    break;
                case CmdConstant.MapCreateMonsterFightHud:
                    int CreateMonsterFightId = (int)notification.Body;
                    PlayMonsterFightHud(CreateMonsterFightId);
                    break;
                case CmdConstant.MapRemoveMonsterFightHud:
                    int removeMonsterFightId = notification.Body is int ? (int)notification.Body : 0;
                    RemoveMonsterFightHud(removeMonsterFightId);
                    break;
                case CmdConstant.MapUpdateTroopHud:
                    int updateTroopId = (int)notification.Body;
                    OnUpdateTroopHudState(updateTroopId);
                    break;
                case CmdConstant.MapCloseTroopHudScale:
                    int closeId = (int)notification.Body;
                    CloseTroopHudScale(closeId);
                    break;
                case CmdConstant.MapOpenTroopHudScale:
                    int openId = (int)notification.Body;
                    OpenTroopHudScale(openId);
                    break;
                case CmdConstant.MapPlayShottTextHud:
                    BattleUIData info = notification.Body as BattleUIData;
                    PlayFightShootText(info);
                    break;
                case CmdConstant.MapStopShottTextHud:
                    int stopShottId = notification.Body is int ? (int)notification.Body : 0;
                    StopFightShootText(stopShottId);
                    break;
                case CmdConstant.MapCreateShottTextHud:
                    int createFightShottId = (int)notification.Body;
                    CreateFightShottText(createFightShottId);
                    break;
                case CmdConstant.MapSetFightBattleUIData:
                    BattleUIData battleUiData = notification.Body as BattleUIData;
                    SetFightBattleUIData(battleUiData);
                    break;
                case CmdConstant.MapCreateBuildingFightHud:
                    int buildIngId = (int)notification.Body;
                    PlayBuildingFightHud(buildIngId);
                    break;                                     
                case CmdConstant.MapRemoveBuildingFightHud:
                    int buildIngIdbyRemoveId = (int)notification.Body;
                    RemoveBuildingFightHud(buildIngIdbyRemoveId);
                    break;
                case CmdConstant.MapCreateRuneGatherHud:
                    {
                        int troopId = (int)notification.Body;
                        PlayCollectRuneHud(troopId);
                    }
                    break;
                case CmdConstant.MapRemoveRuneGatherHud:
                    {
                        int troopId = (int)notification.Body;
                        RemoveCollectRuneHud(troopId);
                    }
                    break;
                case CmdConstant.MapUpdateBuildingHead:
                    int buildingHeadId = (int)notification.Body;
                    UpdateBuildingFightHud(buildingHeadId);
                    break;
                case CmdConstant.AllianceEixt:
                case CmdConstant.AllianceJoinUpdate:
                    UpateMeTroopGuildAddName();
                   // RemoveSelectHUD(0);
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().UpdateAOITroopLinesColor();
                    break;
                case Guild_ModifyGuildInfo.TagName:
                case Role_ModifyName.TagName:
                    UpateMeTroopGuildAddName();
                    break;
                case CmdConstant.MapUpdateGuildAddName:
                case CmdConstant.MapUpdateArmyName:
                    int updateGuildAddId = (int)notification.Body;
                    UpdateTroopGuildAddName(updateGuildAddId);
                    break;
                case CmdConstant.MapObjectHUDUpdate:
                    MapObjectInfoEntity mapObjectInfo = notification.Body as MapObjectInfoEntity;
                    if (mapObjectInfo != null)
                    {
                        UpdateBuildingFightHudTargetObj(mapObjectInfo);
                    }
                    
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_MonsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
            m_RallyTroopsProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_AllianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_ScoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            m_fogMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            configInfo = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            colorGreen = RS.green;
            colorBlue = RS.blue;
            colorWhite = RS.white;
            colorRed = RS.red;
            HUDHelp.Init();
        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
        }

        private void AddTargetStanceInfo(StanceInfo stanceInfo)
        {
            if (!dicTargetStanceInfo.ContainsKey(stanceInfo.targetId))
            {
                dicTargetStanceInfo.Add(stanceInfo.targetId, new Dictionary<int, List<StanceInfo>>());
            }

            if (!dicTargetStanceInfo[stanceInfo.targetId].ContainsKey(stanceInfo.stanceIndex))
            {
                dicTargetStanceInfo[stanceInfo.targetId].Add(stanceInfo.stanceIndex, new List<StanceInfo>());
            }

            dicTargetStanceInfo[stanceInfo.targetId][stanceInfo.stanceIndex].Add(stanceInfo);
        }

        private void RemoveTargetStanceInfo(StanceInfo stanceInfo)
        {
            if (dicTargetStanceInfo.ContainsKey(stanceInfo.targetId) &&
                dicTargetStanceInfo[stanceInfo.targetId].ContainsKey(stanceInfo.stanceIndex) &&
                dicTargetStanceInfo[stanceInfo.targetId][stanceInfo.stanceIndex].Contains(stanceInfo))
            {
                dicTargetStanceInfo[stanceInfo.targetId][stanceInfo.stanceIndex].Remove(stanceInfo);
            }
        }

        private void ResetStanceLayer(int troopId)
        {
            if (dicHudUIFight.ContainsKey(troopId))
            {
                dicHudUIFight[troopId].stanceLayer = 0;
            }
        }

        private void UpdateStanceLayer(int targetId, int stanceIndex)
        {
            if (dicTargetStanceInfo.ContainsKey(targetId) &&
                dicTargetStanceInfo[targetId].ContainsKey(stanceIndex))
            {
                foreach (var stanceInfo in dicTargetStanceInfo[targetId][stanceIndex])
                {
                    if (dicHudUIFight.ContainsKey(stanceInfo.troopId))
                    {
                        dicHudUIFight[stanceInfo.troopId].stanceLayer = dicTargetStanceInfo[targetId][stanceIndex].IndexOf(stanceInfo);
                    }
                }
            }
        }

        private void UpdateTargetStanceInfo()
        {
            foreach (var stanceInfo in dicStanceInfo.Values)
            {
                if (!dicHudUIFight.ContainsKey(stanceInfo.troopId))
                {
                    continue;
                }

                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(stanceInfo.troopId);
                if (armyData == null)
                {
                    continue;
                }

                if (armyData.armyStatus != (int)ArmyStatus.BATTLEING && armyData.armyStatus != (int)(ArmyStatus.BATTLEING | ArmyStatus.STATIONING))
                {
                    RemoveTargetStanceInfo(stanceInfo);
                    ResetStanceLayer(stanceInfo.troopId);
                    UpdateStanceLayer(stanceInfo.targetId, stanceInfo.stanceIndex);

                    continue;
                }

                if (dicHudUIFight[stanceInfo.troopId].targetId == 0)
                {
                    RemoveTargetStanceInfo(stanceInfo);
                    ResetStanceLayer(stanceInfo.troopId);
                    UpdateStanceLayer(stanceInfo.targetId, stanceInfo.stanceIndex);

                    continue;
                }
                else
                {
                    if (stanceInfo.targetId == 0)
                    {
                        stanceInfo.targetId = dicHudUIFight[stanceInfo.troopId].targetId;
                    }
                }

                if (dicTargetStanceInfo.ContainsKey(stanceInfo.targetId) &&
                    dicTargetStanceInfo[stanceInfo.targetId].ContainsKey(stanceInfo.stanceIndex) &&
                    dicTargetStanceInfo[stanceInfo.targetId][stanceInfo.stanceIndex].Contains(stanceInfo))
                {
                    int oldTargetId = stanceInfo.targetId;
                    int newTargetId = dicHudUIFight[stanceInfo.troopId].targetId;

                    int oldStanceIndex = stanceInfo.stanceIndex;
                    int newStanceIndex = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().CalStanceIndex(dicHudUIFight[stanceInfo.troopId].targetId, dicHudUIFight[stanceInfo.troopId].stanceAngle);

                    if (newTargetId != oldTargetId)
                    {
                        RemoveTargetStanceInfo(stanceInfo);
                        ResetStanceLayer(stanceInfo.troopId);
                        UpdateStanceLayer(oldTargetId, oldStanceIndex);

                        stanceInfo.targetId = newTargetId;
                        stanceInfo.stanceIndex = newStanceIndex;

                        AddTargetStanceInfo(stanceInfo);
                        UpdateStanceLayer(newTargetId, newStanceIndex);
                    }
                    else if (newStanceIndex != oldStanceIndex)
                    {
                        RemoveTargetStanceInfo(stanceInfo);
                        ResetStanceLayer(stanceInfo.troopId);
                        UpdateStanceLayer(oldTargetId, oldStanceIndex);

                        stanceInfo.stanceIndex = newStanceIndex;

                        AddTargetStanceInfo(stanceInfo);
                        UpdateStanceLayer(oldTargetId, newStanceIndex);
                    }
                }
                else
                {
                    AddTargetStanceInfo(stanceInfo);
                    UpdateStanceLayer(stanceInfo.targetId, stanceInfo.stanceIndex);
                }
            }
        }

        public override void Update()
        {
            if (updateInterval < 0)
            {
                UpdateTargetStanceInfo();
                return;
            }

            if (Time.realtimeSinceStartup - lastCallbackTime >= updateInterval)
            {
                lastCallbackTime = Time.realtimeSinceStartup;
                UpdateTargetStanceInfo();
            }
        }

        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }

        /// <summary>
        /// 部队被删除的时候
        /// </summary>
        private void RemoveHUDByID(int troopId)
        {
            RemoveFightHud(troopId);
            // RemoveFightShottText(troopId);
            RemoveTroopHud(troopId);
            RemoveSelectHUD(troopId);
            RemoveCollectRuneHud(troopId);
        }

        #region map 每一个部队的hud处理

        public override void OnRemove()
        {
            base.OnRemove();
            Clear();
        }

        private void Clear()
        {
            dicHudUI.Clear();
            dicShootHud.Clear();
            m_dictRuneHud.Clear();
            dicHudUIFight.Clear();
            dicHudSelect.Clear();
            dicStanceInfo.Clear();
            dicTargetStanceInfo.Clear();
        }

        private void ClearTroopHud()
        {
            List<long> listKey;

            listKey = dicHudUI.Keys.ToList<long>(); 
            foreach (long key in listKey)
            {
                RemoveTroopHud((int)key);
            }

            listKey = dicShootHud.Keys.ToList<long>();
            foreach (long key in listKey)
            {
                RemoveShootHud((int)key);
            }

            listKey = m_dictRuneHud.Keys.ToList<long>();
            foreach (long key in listKey)
            {
                RemoveCollectRuneHud((int)key);
            }

            listKey = dicHudUIFight.Keys.ToList<long>();
            foreach (long key in listKey)
            {
                RemoveFightHud((int)key);
            }

            listKey = dicHudSelect.Keys.ToList<long>();
            foreach (long key in listKey)
            {
                CloseTroopSelectHudUI((int)key);
            }

            listKey.Clear();

            Clear();
        }

        private void CreateTroopHud(int troopId)
        {
            if (dicHudUI.ContainsKey(troopId))
            {
                Debug.LogError("部队已经生成过这个hud" + troopId);
                return;
            }
            
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetArmyData(troopId);
            if (armyData != null)
            {
                HUDUI huduiTroop = HUDUI
                    .Register(UI_Pop_WorldArmyCmdView.VIEW_NAME, typeof(UI_Pop_WorldArmyCmdView),
                        HUDLayer.world, armyData.go).SetData(troopId).SetInitCallback((hud) =>
                    {
                        if (dicHudSelect.ContainsKey(troopId))
                        {
                            CloseTroopHudScale(troopId);
                        }
                        OnTroopHudCallBack(hud);
                    })
                    .SetTargetGameObject(armyData.go)
                    .SetCameraLodDist(0, 3000f).SetPositionAutoAnchor(true);
                
                if(GameModeManager.Instance.CurGameMode == GameModeType.World)
                {
                    huduiTroop.SetUpdateCallback((HUDUI hudui) =>
                    {
                        var pos = armyData.GetMovePos();
                        var inFog = m_fogMediator.HasFogAtWorldPos(pos.x, pos.y);
                        huduiTroop.gameView.gameObject.SetActive(!inFog);
                    });
                }

                this.dicHudUI.Add(troopId, huduiTroop);
                ClientUtils.hudManager.ShowHud(huduiTroop);
            }
        }

        private void RemoveTroopHud(int troopId)
        {
            HUDUI hudui = null;
            if (dicHudUI.TryGetValue(troopId, out hudui))
            {
                HUDManager.Instance().CloseSingleHud(ref hudui);
                dicHudUI.Remove(troopId);
            }
        }

        private void UpateMeTroopGuildAddName()
        {
            foreach (var info in dicHudUI)
            {
                UI_Pop_WorldArmyCmdView view = (UI_Pop_WorldArmyCmdView)dicHudUI[info.Key].gameView;
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler
                    .GetITroopMgr().GetArmyData((int)info.Key);
                if (view != null)
                {
                    if (m_TroopProxy.IsPlayOwnTroop(armyData.objectId))
                    {
                        OnTroopHudMeCallBack(view, armyData);
                    }
                    else
                    {
                        OnTroopOtherHudCallBack(view, armyData);
                    }
                    
                    if (armyData.isRally)
                    {
                        HUDHelp.OnRallyTroopHud(view, armyData, m_RallyTroopsProxy, null, true);
                    }
            
                    WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateLineColor(armyData.objectId);
                }
            }
        }


        private void UpdateTroopGuildAddName(int troopId)
        {
            if (dicHudUI.ContainsKey(troopId))
            {
                UI_Pop_WorldArmyCmdView view = (UI_Pop_WorldArmyCmdView)dicHudUI[troopId].gameView;
                if (view == null)
                {
                    return;
                }
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler
                    .GetITroopMgr().GetArmyData(troopId);
                if (armyData != null)
                {
                    if (!m_TroopProxy.IsPlayOwnTroop(troopId))
                    {
                       OnTroopOtherHudCallBack(view, armyData);                     
                        WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateLineColor(troopId);
                    }
                    else
                    {
                        OnTroopHudMeCallBack(view, armyData);
                    }

                    if (armyData.isRally)
                    {
                        HUDHelp.OnRallyTroopHud(view, armyData, m_RallyTroopsProxy);
                    }
                }
            }

            if (dicHudSelect.ContainsKey(troopId))
            {
                UI_Pop_WorldArmyCmdView view = (UI_Pop_WorldArmyCmdView)dicHudSelect[troopId].gameView;
                if (view == null)
                {
                    return;
                }

                var hudInfo = dicHudSelect[troopId];
                OnSelectTroopHudCallBack(hudInfo);
            }
        }


        private void SetTroopHudActive(int troopId, bool active)
        {
            if (dicHudUI.ContainsKey(troopId))
            {
                UI_Pop_WorldArmyCmdView view = (UI_Pop_WorldArmyCmdView)dicHudUI[troopId].gameView;
                if (view == null)
                {
                    return;
                }

                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler
                    .GetITroopMgr().GetArmyData(troopId);
                if (armyData != null)
                {
                    if (armyData.state == Troops.ENMU_SQUARE_STAT.FIGHT ||
                        TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.BATTLEING) ||
                        TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.ARMY_SATNBY)) 
                    {
                        active = false;
                    }
                }

                if (active)
                {
                    view.m_pl_state.transform.DOScale(new Vector3(1, 1, 1), 0);
                }
                else
                {
                    switch (GameModeManager.Instance.CurGameMode)
                    {
                        case GameModeType.World:
                            {
                                view.m_pl_state.transform.DOScale(new Vector3(0, 0, 0), 0);
                            }
                            break;
                        case GameModeType.Expedition:
                            {
                            }
                            break;
                    }
                    
                }
            }
        }

        private void OnTroopHudCallBack(HUDUI info)
        {
            UI_Pop_WorldArmyCmdView view = info.gameView as UI_Pop_WorldArmyCmdView;
            if (view != null)
            {
                int troopId = (int)info.data;
                view.gameObject.name =
                    string.Format("{0}_{1}_{2}", UI_Pop_WorldArmyCmdView.VIEW_NAME, "Troop", troopId);
                info.gameView.gameObject.GetComponent<MapElementUI>().enabled = false;
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler
                    .GetITroopMgr().GetArmyData(troopId);
                if (armyData == null)
                {
                    Debug.LogWarning("没有这个部队数据。hud数据没办法处理" + troopId);
                    return;
                }

                if (!m_TroopProxy.IsPlayOwnTroop(troopId))
                {
                    SetStateOtherIcon(view, armyData);
                    OnTroopOtherHudCallBack(view, armyData);
                }
                else
                {
                    SetStateMeIcon(view, armyData);
                    OnTroopHudMeCallBack(view, armyData);
                }

                if (armyData.isRally)
                {
                    OnRallyTroopHudMeCallBack(view, armyData);
                }
            }
        }


        private void SetStateOtherIcon(UI_Pop_WorldArmyCmdView view, ArmyData armyData)
        {
            if (view == null || armyData == null)
            {
                return;
            }

            view.m_pl_state.gameObject.SetActive(true);
            string icon = TroopHelp.GetIcon(armyData.armyStatus);
            bool imgStateActive = false;
            if (armyData.state == Troops.ENMU_SQUARE_STAT.MOVE)
            {
                if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                {
                    SetTroopHudActive(armyData.objectId, true);
                    if (!armyData.isPlayerHave)
                    {
                        HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(armyData.heroId);
                        if (hero != null)
                        {
                            view.m_pl_head.gameObject.SetActive(false);
                            view.m_pl_namebg_PolygonImage.gameObject.SetActive(false);
                            view.m_pb_Hp_GameSlider.gameObject.SetActive(false);
                            view.m_pl_ap.gameObject.SetActive(false);
                            view.m_UI_Model_CaptainHead.SetIcon(hero.config.heroIcon);
                            view.m_UI_Model_CaptainHead.SetFightArmyRare(armyData);
                            view.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_GrayChildrens.Gray();
                        }
                    }

                    imgStateActive = true;
                    ClientUtils.LoadSprite(view.m_img_state_fail_PolygonImage, icon);
                }
                else
                {
                    view.m_pl_head.gameObject.SetActive(false);              
                }
            }
            else if (armyData.state == Troops.ENMU_SQUARE_STAT.IDLE)
            {
                view.m_pl_head.gameObject.SetActive(false);
            }
            else if (armyData.state == Troops.ENMU_SQUARE_STAT.FIGHT)
            {
                view.m_pl_head.gameObject.SetActive(false);
            }

            view.m_img_state_stop_PolygonImage.gameObject.SetActive(false);
            view.m_img_state_fail_PolygonImage.gameObject.SetActive(imgStateActive);
            
            updateAtkIcon(view, true);
        }


        private void SetStateMeIcon(UI_Pop_WorldArmyCmdView view, ArmyData armyData)
        {
            if (view == null || armyData == null)
            {
                return;
            }

            view.m_pl_state.gameObject.SetActive(true);
            string icon = TroopHelp.GetIcon(armyData.armyStatus);
            bool imgStateActive = false;
            bool imgStateStopActive = true;
            if (armyData.state == Troops.ENMU_SQUARE_STAT.MOVE)
            {
                imgStateActive = false;
                imgStateStopActive = false;
                if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                {
                    imgStateActive = true;
                    SetTroopHudActive(armyData.objectId, true);
                    ClientUtils.LoadSprite(view.m_img_state_fail_PolygonImage, icon);
                }
            }
            else if (armyData.state == Troops.ENMU_SQUARE_STAT.IDLE)
            {
                imgStateStopActive =true;
                if(TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.COLLECTING_NO_DELETE))
                {
                    imgStateStopActive = false;
                    imgStateActive = false;
                }
                else if(armyData.armyStatus != 0)
                {
                    imgStateActive = false;
                    ClientUtils.LoadSprite(view.m_icon_state_stop_PolygonImage, icon);
                }
            }
            else if (armyData.state == Troops.ENMU_SQUARE_STAT.FIGHT)
            {
                imgStateStopActive = false;
            }

            view.m_img_state_stop_PolygonImage.gameObject.SetActive(imgStateStopActive);
            view.m_img_state_fail_PolygonImage.gameObject.SetActive(imgStateActive);
            
            updateAtkIcon(view, false);
        }

        private void updateAtkIcon(UI_Pop_WorldArmyCmdView view, bool isShow)
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        view.m_img_state_atk_PolygonImage.gameObject.SetActive(false);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        view.m_img_state_atk_PolygonImage.gameObject.SetActive(isShow);
                    }
                    break;
            }
        }

        private void OnUpdateTroopHudState(int id)
        {
            HUDUI hudui;
            if (dicHudUI.TryGetValue(id, out hudui))
            {
                UI_Pop_WorldArmyCmdView view = hudui.gameView as UI_Pop_WorldArmyCmdView;
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler
                    .GetITroopMgr().GetArmyData(id);
                if (m_TroopProxy.IsPlayOwnTroop(id))
                {
                    SetStateMeIcon(view, armyData);
                    OnTroopHudMeCallBack(view, armyData);
                }
                else
                {
                    SetStateOtherIcon(view, armyData);
                    OnTroopOtherHudCallBack(view, armyData);
                }
            }
        }

        private void OnTroopHudMeCallBack(UI_Pop_WorldArmyCmdView view, ArmyData data)
        {
            if (view == null || data == null)
            {
                return;
            }

            view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
            view.m_pl_time.gameObject.SetActive(false);
            view.m_pl_head.gameObject.SetActive(false);

            if (GameModeManager.Instance.CurGameMode == GameModeType.World)
            {
                view.m_pl_stateName_LanguageText.color = RS.green;
                if (!m_guideProxy.IsCompletedByStage(m_PlayerProxy.ConfigDefine.guideHideCityName))
                {
                    view.m_pl_stateName_LanguageText.text = LanguageUtils.getText(200071);
                }
                else
                {
                    string guildAddName = string.Empty;
                    if (data.troopType == RssType.Scouts)
                    {
                        if (m_AllianceProxy.GetAlliance() != null)
                        {
                            if (!string.IsNullOrEmpty(m_AllianceProxy.GetAlliance().abbreviationName))
                            {
                                guildAddName = string.Format("[{0}]", m_AllianceProxy.GetAlliance().abbreviationName);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(data.guildAbbName))
                        {
                            guildAddName = string.Format("[{0}]", data.guildAbbName);
                        }
                    }

                    HUDHelp.SetAllianceIcon(view, data);
                    view.m_pl_stateName_LanguageText.text = string.Format("{0}{1}", guildAddName, m_PlayerProxy.CurrentRoleInfo.name);
                }
            }
            else
            {
                view.m_pl_stateName_LanguageText.gameObject.SetActive(false);
            }
        }

        private Color GetOtherTroopColor(ArmyData data)
        {
            return HUDHelp.GetOtherTroopColor(data);
        }

        private void OnRallyTroopHudMeCallBack(UI_Pop_WorldArmyCmdView view, ArmyData data)
        {
            if (view == null || data == null)
            {
                return;
            }

            view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
            view.m_pl_time.gameObject.SetActive(false);
            view.m_pl_head.gameObject.SetActive(false);
            view.m_img_state_stop_PolygonImage.gameObject.SetActive(false);
            if (m_RallyTroopsProxy.IsCaptainByarmIndex(data.troopId))
            {
                view.m_pl_stateName_LanguageText.color = colorGreen;
            }
            else if (m_RallyTroopsProxy.isRallyTroopHaveGuid(data.armyRid))
            {
                view.m_pl_stateName_LanguageText.color = colorBlue;
            }
            else if (m_RallyTroopsProxy.HasRallyed(data.targetId))
            {
                view.m_pl_stateName_LanguageText.color = colorRed;
            }
            else
            {
                view.m_pl_stateName_LanguageText.color = colorWhite;
            }

            string guildAddName = string.Empty;
            if (!string.IsNullOrEmpty(data.guildAbbName))
            {
                guildAddName = string.Format("[{0}]", data.guildAbbName);
            }
            HUDHelp.SetAllianceIcon(view, data);

            view.m_pl_stateName_LanguageText.text = string.Format("{0}{1}", guildAddName, data.armyName);
        }


        private void OnTroopOtherHudCallBack(UI_Pop_WorldArmyCmdView view, ArmyData data)
        {
            if (view == null || data == null)
            {
                return;
            }

            view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
            view.m_pl_time.gameObject.SetActive(false);

            view.m_pl_stateName_LanguageText.color = GetOtherTroopColor(data);

            string guildAddName = string.Empty;
            if (!string.IsNullOrEmpty(data.guildAbbName))
            {
                guildAddName = string.Format("[{0}]", data.guildAbbName);
            }
            HUDHelp.SetAllianceIcon(view, data);
            view.m_pl_stateName_LanguageText.text = string.Format("{0}{1}", guildAddName, data.armyName);
        }


        #endregion

        #region 选中的部队hud处理

        private void CreateSelectHUD(int mainArmObjectId, List<int> viceArmObjectIdList)
        {
            List<int> oldArmObjectIdList = new List<int>();
            if (curObjectId != 0) oldArmObjectIdList.Add(curObjectId);
            if (curViceObjectIdList != null)
            {
                foreach (var objectId in curViceObjectIdList)
                {
                    oldArmObjectIdList.Add(objectId);
                }
            }

            List<int> newArmObjectIdList = new List<int>();
            if (mainArmObjectId != 0) newArmObjectIdList.Add(mainArmObjectId);
            if (viceArmObjectIdList != null)
            {
                foreach (var objectId in viceArmObjectIdList)
                {
                    newArmObjectIdList.Add(objectId);
                }
            }

            List<int> closeArmObjectIdList = new List<int>();
            foreach (var objectId in oldArmObjectIdList)
            {
                if(!newArmObjectIdList.Contains(objectId))
                {
                    closeArmObjectIdList.Add(objectId);
                }
            }

            List<int> createArmObjectIdList = new List<int>();
            foreach (var objectId in newArmObjectIdList)
            {
                if (!oldArmObjectIdList.Contains(objectId))
                {
                    createArmObjectIdList.Add(objectId);
                }
            }

            foreach (var objectId in closeArmObjectIdList)
            {
                CloseTroopSelectHudUI(objectId);
            }

            foreach (var objectId in createArmObjectIdList)
            {
                if (dicHudSelect.ContainsKey(objectId)) continue;

                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
                if (armyData == null || armyData.go == null) continue;

                SetTroopHudActive(objectId, false);

                HUDUI hudui = HUDUI
                    .Register(UI_Pop_WorldArmyCmdView.VIEW_NAME, typeof(UI_Pop_WorldArmyCmdView), HUDLayer.world, armyData.go)
                    .SetData(objectId)
                    .SetInitCallback(OnSelectTroopHudCallBack)
                    .SetUpdateCallback(OnSelectTroopUpdateHudCallBack, 1f)
                    .SetTargetGameObject(armyData.go)
                    .SetPositionAutoAnchor(true)
                    .SetCameraLodDist(0, 3000f)
                    .SetPosOffset(new Vector2(0, 150));
                ClientUtils.hudManager.ShowHud(hudui);

                if (!dicHudSelect.ContainsKey(objectId))
                {
                    dicHudSelect.Add(objectId, hudui);
                }
            }

            bool inRallyFlag = false; 
            ArmyData mainArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(mainArmObjectId);
            if (mainArmyData != null)
            {
                inRallyFlag = m_RallyTroopsProxy.IsRallyTroopAttackMe(mainArmyData.armyRid);
            }
            
            if (!inRallyFlag && !m_TroopProxy.IsPlayOwnTroop(mainArmObjectId))
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveSelectEffect);
            }
            else
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.OnOpenSelectMainTroop, mainArmObjectId);
            }

            curObjectId = mainArmObjectId;
            curViceObjectIdList = viceArmObjectIdList;
        }

        private void RemoveSelectHUD(int troopId)
        {
            CloseTroopSelectHudUI(troopId);
        }

        private void CloseTroopHudScale(int id)
        {
            SetTroopHudActive(id, false);
        }

        private void OpenTroopHudScale(int id)
        {
            SetTroopHudActive(id, true);
        }

        private void CloseTroopDoubleSelectHudUI()
        {
            CloseTroopSelectHudUI(0);
        }

        [IFix.Patch]
        private void CloseTroopSelectHudUI(int troopId)
        {
            if (troopId == 0)
            {
                foreach (var item in dicHudSelect)
                {
                    HUDUI hudui = item.Value;
                    ClientUtils.hudManager.CloseSingleHud(ref hudui);

                    if (dicHudUIFight.ContainsKey(item.Key))
                    {
                        PlayerFightUiHudView playerFightUiHudView = dicHudUIFight[item.Key] as PlayerFightUiHudView;
                        if (playerFightUiHudView != null)
                        {
                            playerFightUiHudView.CloseOtherTroopHudView();
                        }
                    }

                    OpenTroopHudScale((int)item.Key);
                }

                dicHudSelect.Clear();

                curObjectId = 0;

                if (curViceObjectIdList != null)
                {
                    curViceObjectIdList.Clear();
                    curViceObjectIdList = null;
                }                
            }
            else if (dicHudSelect.ContainsKey(troopId))
            {
                HUDUI hudui = dicHudSelect[troopId];
                if (hudui != null)
                {
                    ClientUtils.hudManager.CloseSingleHud(ref hudui);
                    dicHudSelect.Remove(troopId);
                }

                OpenTroopHudScale(troopId);

                if (curObjectId == troopId)
                {
                    curObjectId = 0;
                }

                if (curViceObjectIdList != null)
                {
                    if (curViceObjectIdList.Contains(troopId))
                    {
                        curViceObjectIdList.Remove(troopId);
                    }
                }
            }
        }

        private void OnSelectTroopUpdateHudCallBack(HUDUI hudui)
        {
            UI_Pop_WorldArmyCmdView view = hudui.gameView as UI_Pop_WorldArmyCmdView;
            if (view == null)
            {
                return;
            }

            int objectId = (int)hudui.data;

            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null)
            {
                return;
            }

            int times = 0;
            if (armyData.state == Troops.ENMU_SQUARE_STAT.MOVE)
            {
                times = TroopHelp.GetRemainingTime(armyData.arrivalTime, armyData.startTime);
            }
            view.m_pl_time.gameObject.SetActive(times > 0);
            view.m_lbl_time_LanguageText.text = ClientUtils.FormatTimeTroop(times);

            HUDHelp.SetBloodProgress(view.m_pl_sd_GameSlider_GameSlider, view.m_img_Fill_PolygonImage, armyData.troopNumMax, armyData.troopNums);

            if (armyData.isRally)
            {
                HUDHelp.OnRallyUpdateTroopHud(view, armyData, m_RallyTroopsProxy);
                return;
            }

            if (!m_TroopProxy.IsPlayOwnTroop(objectId))
            {
                return;
            }

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    bool btnReturnActive = true; //返回按钮
                    bool btnStationedActive = true; //驻扎按钮
                    if (armyData.state == Troops.ENMU_SQUARE_STAT.MOVE)
                    {
                        btnReturnActive = true;
                        btnStationedActive = true;
                        if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                        {
                            btnReturnActive = false;
                            btnStationedActive = false;
                        }
                    }
                    else if (armyData.state == Troops.ENMU_SQUARE_STAT.IDLE)
                    {
                        btnReturnActive = true;
                        btnStationedActive = false;
                    }
                    else if (armyData.state == Troops.ENMU_SQUARE_STAT.FIGHT)
                    {
                        btnReturnActive = true;
                        btnStationedActive = true;
                    }

                    view.m_UI_Item_CMDBtns.gameObject.SetActive(btnReturnActive || btnStationedActive);

                    if (curViceObjectIdList != null && curViceObjectIdList.Contains(objectId))
                    {
                        view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                    }

                    break;
            }

            if (armyData.troopType == RssType.Transport)
            {
                var txt = RS.TransportNameIndex[armyData.dataIndex - 1];
                view.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(184002, txt);
            }
            else if (armyData.troopType == RssType.Troop)
            {
                view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
            }
            else if (armyData.troopType == RssType.Scouts)
            {
                view.m_lbl_count_LanguageText.text = LanguageUtils.getText(181163);
            }
        }


        private void OnSelectTroopOtherHudCallBack(UI_Pop_WorldArmyCmdView view, ArmyData armyData)
        {
            if (view == null && armyData == null)
            {
                return;
            }
            List<CMDBtnsParam> troopBtnParams = new List<CMDBtnsParam>();

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    if (armyData.troopType == RssType.Scouts) break;
                    if (TroopHelp.IsHaveState((long) armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                    {
                        view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                        break;
                    }
                    
                    if (armyData.state == Troops.ENMU_SQUARE_STAT.MOVE ||
                        armyData.state == Troops.ENMU_SQUARE_STAT.IDLE ||
                        armyData.state == Troops.ENMU_SQUARE_STAT.FIGHT)
                    {
                        troopBtnParams.Add(new CMDBtnsParam()
                        {
                            IconType = TroopHudIconType.Attack,
                            ClickParam = armyData,
                            OnBtnClickListener = OnOtherTroopAttackClicked,
                        });
                        troopBtnParams.Add(new CMDBtnsParam()
                        {
                            IconType = TroopHudIconType.Investigation,
                            ClickParam = armyData,
                            OnBtnClickListener = OnOtherTroopInvestigationClicked,
                        });
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        if (TroopHelp.IsHaveState((long) armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                        {
                            view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                            break;
                        }
                        troopBtnParams.Add(new CMDBtnsParam()
                        {
                            IconType = TroopHudIconType.Attack,
                            ClickParam = armyData,
                            OnBtnClickListener = OnOtherTroopAttackClicked,
                        });
                    }
                    break;
            }
            
            view.m_UI_Item_CMDBtns.SetBtnParams(troopBtnParams);

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        Color color = HUDHelp.GetOtherTroopColor(armyData);
                        view.m_lbl_count_LanguageText.color = color;
                        if (armyData.troopType == RssType.Scouts)
                        {
                            view.m_lbl_count_LanguageText.text = LanguageUtils.getText(181163);
                            if (armyData.dataIndex > 0)
                            {
                                string heroIcon = string.Empty;
                                switch (armyData.dataIndex)
                                {
                                    case 1:
                                        heroIcon = configInfo.toScoutsIcon1;
                                        break;
                                    case 2:
                                        heroIcon = configInfo.toScoutsIcon2;
                                        break;
                                    case 3:
                                        heroIcon = configInfo.toScoutsIcon3;
                                        break;
                                }

                                view.m_pb_Hp_GameSlider.gameObject.SetActive(false);
                                view.m_pl_ap.gameObject.SetActive(false);
                                view.m_UI_Model_CaptainHead.gameObject.SetActive(true);
                                view.m_UI_Model_CaptainHead.SetIcon(heroIcon);
                                view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            if (color== RS.blue)
                            {
                                view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
                            }
                            else
                            {
                                view.m_lbl_count_LanguageText.text = "????";
                            }       
                        }

                        view.m_pl_ap.gameObject.SetActive(false);
                        view.m_lbl_playerName_LanguageText.color = color;

                        string guildAddName = string.Empty;
                        if (!string.IsNullOrEmpty(armyData.guildAbbName))
                        {
                            guildAddName = string.Format("[{0}]", armyData.guildAbbName);
                        }
                        HUDHelp.SetAllianceIcon(view, armyData);

                        view.m_lbl_playerName_LanguageText.text =
                            string.Format("{0}{1}", guildAddName, armyData.armyName);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        Color col = Color.white;
                        view.m_lbl_count_LanguageText.color = col;
                        view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
                        view.m_lbl_playerName_LanguageText.color = col;
                        HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(armyData.heroId);
                        if (hero != null)
                        {
                            view.m_lbl_playerName_LanguageText.text = LanguageUtils.getText(hero.config.l_nameID);
                        }
                    }
                    break;
            }

        }

        private void OnOtherTroopAttackClicked(object param)
        {
            ArmyData armyData = param as ArmyData;
            if (armyData == null) return;

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    FightHelper.Instance.Attack(armyData.objectId);
                    break;
                case GameModeType.Expedition:
                    {
                        OpenPanelData openPanelData = new OpenPanelData(armyData.objectId, OpenPanelType.Common);
                        CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
                    }
                    break;
            }
            CloseTroopSelectHudUI(armyData.objectId);
        }

        private void OnOtherTroopInvestigationClicked(object param)
        {
            ArmyData armyData = param as ArmyData;
            if (armyData == null) return;
            m_ScoutProxy.CheckScoutCondition(armyData.objectId, () =>
            {
                Vector2 v2 = new Vector2(armyData.go.transform.position.x, armyData.go.transform.position.z);
                UI_Pop_ScoutSelectMediator.Param viewParam = new UI_Pop_ScoutSelectMediator.Param();
                viewParam.pos = v2;
                viewParam.targetIndex = armyData.objectId;
                CoreUtils.uiManager.ShowUI(UI.s_scoutSelectMenu, null, viewParam);
            });
            CloseTroopSelectHudUI(armyData.objectId);
        }

        private void OnSelectTroopHudCallBack(UI_Pop_WorldArmyCmdView view, ArmyData armyData)
        {
            if (view == null || armyData == null)
            {
                return;
            }

            #region 操作盘处理

            List<CMDBtnsParam> troopBtnParams = new List<CMDBtnsParam>();
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {

                        switch (armyData.troopType)
                        {
                            case RssType.Scouts:
                            case RssType.Transport:
                                troopBtnParams.Add(new CMDBtnsParam()
                                {
                                    IconType = TroopHudIconType.Retreat,
                                    ClickParam = armyData,
                                    OnBtnClickListener = OnTroopRetreatClicked,
                                });
                                break;
                            case RssType.Troop:

                                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                                {
                                    break;
                                }

                                if (armyData.state == Troops.ENMU_SQUARE_STAT.MOVE ||
                                    armyData.state == Troops.ENMU_SQUARE_STAT.FIGHT)
                                {
                                    troopBtnParams.Add(new CMDBtnsParam()
                                    {
                                        IconType = TroopHudIconType.Retreat,
                                        ClickParam = armyData,
                                        OnBtnClickListener = OnTroopRetreatClicked,
                                    });
                                    troopBtnParams.Add(new CMDBtnsParam()
                                    {
                                        IconType = TroopHudIconType.Stationed,
                                        ClickParam = armyData,
                                        OnBtnClickListener = OnTroopStationedClicked,
                                    });
                                }
                                else if (armyData.state == Troops.ENMU_SQUARE_STAT.IDLE)
                                {
                                    troopBtnParams.Add(new CMDBtnsParam()
                                    {
                                        IconType = TroopHudIconType.Retreat,
                                        ClickParam = armyData,
                                        OnBtnClickListener = OnTroopRetreatClicked,
                                    });
                                }
                                break;
                        }
                    }
                    break;
                case GameModeType.Expedition:
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                    {
                        break;
                    }
                    if (armyData.state == Troops.ENMU_SQUARE_STAT.MOVE)
                    {
                        troopBtnParams.Add(new CMDBtnsParam()
                        {
                            IconType = TroopHudIconType.Stationed,
                            ClickParam = armyData,
                            OnBtnClickListener = OnTroopStationedClicked,
                        });
                    }
                    break;
            }
            
            view.m_UI_Item_CMDBtns.SetBtnParams(troopBtnParams);

            #endregion

            string guildAddName = string.Empty;
            if (!string.IsNullOrEmpty(armyData.guildAbbName))
            {
                guildAddName = string.Format("[{0}]", armyData.guildAbbName);
            }

            if (armyData.troopType == RssType.Scouts)
            {
                view.m_UI_Model_CaptainHead.gameObject.SetActive(false);
                view.m_pb_Hp_GameSlider.gameObject.SetActive(false);
                view.m_pl_ap.gameObject.SetActive(false);
                view.m_pl_state.gameObject.SetActive(false);
                view.m_pl_namebg_PolygonImage.gameObject.SetActive(false);
                view.m_lbl_ScoutsNameName_LanguageText.gameObject.SetActive(true);
                view.m_lbl_ScoutsNameName_LanguageText.color = colorGreen;

                view.m_lbl_ScoutsNameName_LanguageText.text = string.Format("{0}{1}", guildAddName, m_PlayerProxy.CurrentRoleInfo.name); ;
            }
            else if (armyData.troopType == RssType.Transport)
            {
                view.m_UI_Model_CaptainHead.gameObject.SetActive(true);
                view.m_UI_Model_CaptainHead.SetIcon(CoreUtils.dataService.QueryRecord<ConfigDefine>(0).transportIcon);
                view.m_pb_Hp_GameSlider.gameObject.SetActive(false);
                view.m_pl_ap.gameObject.SetActive(false);
                view.m_pl_namebg_PolygonImage.gameObject.SetActive(true);
                view.m_lbl_ScoutsNameName_LanguageText.gameObject.SetActive(false);
                view.m_lbl_count_LanguageText.color = colorGreen;
                var txt = RS.TransportNameIndex[armyData.dataIndex - 1];
                view.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(184002, txt);
            }
            else
            {
                view.m_lbl_count_LanguageText.color = colorGreen;

                view.m_lbl_count_LanguageText.text = TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH)
                    ? "0"
                    : ClientUtils.FormatComma(armyData.troopNums);

            }

            view.m_lbl_playerName_LanguageText.color = colorGreen;

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    view.m_lbl_playerName_LanguageText.text = string.Format("{0}{1}", guildAddName, m_PlayerProxy.CurrentRoleInfo.name);
                    break;
                case GameModeType.Expedition:
                    HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(armyData.heroId);
                    if (hero != null)
                    {
                        view.m_lbl_playerName_LanguageText.text = LanguageUtils.getText(hero.config.l_nameID);
                    }
                    break;
            }
        }
        
        
        private  PlayerFightUiHudView GetPlayerFightUiHudView(ArmyData armyData)
        {
            if (dicHudUIFight.ContainsKey(armyData.objectId))
            {
                PlayerFightUiHudView playerFightUiHudView =
                    dicHudUIFight[armyData.objectId] as PlayerFightUiHudView;
                if (playerFightUiHudView != null)
                {
                    return playerFightUiHudView;
                }
            }
            return null;
        }

        private void OnSelectTroopHudCallBack(HUDUI info)
        {
            UI_Pop_WorldArmyCmdView view = info.gameView as UI_Pop_WorldArmyCmdView;
            if (view != null)
            {
                int objectId = (int)info.data;
                info.gameView.gameObject.GetComponent<MapElementUI>().enabled = false;
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
                if (armyData != null)
                {
                    view.gameObject.name = string.Format("{0}_{1}_{2}", UI_Pop_WorldArmyCmdView.VIEW_NAME, "SelectTroop", objectId);
                    HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(armyData.heroId);
                    if (hero != null)
                    {
                        view.m_UI_Model_CaptainHead.SetIcon(hero.config.heroIcon);
                        view.m_UI_Model_CaptainHead.SetArmyRare(armyData);
                    }

                    #region 行军时间显示

                    int times = 0;
                    if (armyData.state == Troops.ENMU_SQUARE_STAT.MOVE)
                    {
                        times = TroopHelp.GetRemainingTime(armyData.arrivalTime, armyData.startTime);
                    }

                    view.m_lbl_time_LanguageText.text =
                        ClientUtils.FormatTimeTroop(times);
                    view.m_pl_time.gameObject.SetActive(times > 0);

                    #endregion

                    HUDHelp.SetBloodProgress(view.m_pl_sd_GameSlider_GameSlider, view.m_img_Fill_PolygonImage, armyData.troopNumMax, armyData.troopNums);

                    #region 头像边框

                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                    {
                        view.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_GrayChildrens.Gray();
                    }
                    else
                    {
                        view.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_GrayChildrens.Normal();
                    }
                    
                    #endregion
                    if (armyData.isRally)
                    {
                        HUDHelp.OnRallyTroopHud(view, armyData, m_RallyTroopsProxy, () =>
                        {
                            CloseTroopSelectHudUI(0);
                        });
                    }
                    else
                    {
                        if (!m_TroopProxy.IsPlayOwnTroop(objectId))
                        {
                            OnSelectTroopOtherHudCallBack(view, armyData);

                            if (armyData.guild == 0 || armyData.guild != m_PlayerProxy.CurrentRoleInfo.guildId)
                            {
                                if (armyData.troopType != RssType.Scouts)
                                {
                                    view.m_UI_Item_CMDBtns.gameObject.SetActive(true);
                                }
                                
                                if (TroopHelp.IsHaveState((long) armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                                {
                                    view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            OnSelectTroopHudCallBack(view, armyData);

                            if (curViceObjectIdList != null && curViceObjectIdList.Contains(objectId))
                            {
                                view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                            }
                        }
                    }
                    
                    PlayerFightUiHudView playerFightUiHudView=  GetPlayerFightUiHudView(armyData);
                    if (playerFightUiHudView != null)
                    {
                        view.m_pl_head.gameObject.SetActive(false);
                        if (armyData.isRally)
                        {                       
                            playerFightUiHudView.OpenRallyTroopHUDView();    
                        }
                    }
                    else
                    {
                        view.m_pl_head.gameObject.SetActive(true);
                    }
                }
            }
        }

        private void OnTroopRetreatClicked(object param)
        {
            ArmyData armyData = param as ArmyData;
            if (armyData == null) return;
            switch (armyData.troopType)
            {
                case RssType.Scouts:
                    {
                        m_TroopProxy.TroopMapMarCh(armyData.objectId, TroopAttackType.ScoutsBack, 0, null);
                    }
                    break;
                case RssType.Transport:
                    {
                        m_TroopProxy.TroopMapMarCh(armyData.objectId, TroopAttackType.TransportBack, 0, null);
                    }
                    break;
                case RssType.Troop:
                    {
                        TroopHudMapMarChInfo troopHudMapMarChInfo = new TroopHudMapMarChInfo();
                        troopHudMapMarChInfo.arnyIndex = armyData.troopId;
                        troopHudMapMarChInfo.attackType = TroopAttackType.Retreat;
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapTroopHudMapMarCh, troopHudMapMarChInfo);
                    }
                    break;
            }
            CloseTroopSelectHudUI(0);
        }

        private void OnTroopStationedClicked(object param)
        {
            ArmyData armyData = param as ArmyData;
            if (armyData == null) return;
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    TroopHudMapMarChInfo troopHudMapMarChInfo = new TroopHudMapMarChInfo();
                    troopHudMapMarChInfo.arnyIndex = armyData.troopId;
                    troopHudMapMarChInfo.attackType = TroopAttackType.Stationed;
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapTroopHudMapMarCh, troopHudMapMarChInfo);
                    break;
                case GameModeType.Expedition:
                    AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionTroopHudMapMarCh, armyData.objectId);
                    break;
            }
            CloseTroopSelectHudUI(0);
            SetTroopHudActive(armyData.troopId, true);
        }

        #endregion

        #region 野蛮人战斗hud

        private void PlayMonsterFightHud(int monsterid)
        {
            if (dicHudUIFight.ContainsKey(monsterid))
            {
                return;
            }

            monsterData = m_worldProxy.GetWorldMapObjectByobjectId(monsterid);
            if (monsterData == null)
            {
                return;
            }


            GameObject go = null;
            int id = (int)monsterData.objectId;
            if (monsterData.rssType == RssType.Monster ||
                monsterData.rssType == RssType.SummonAttackMonster ||
                monsterData.rssType == RssType.SummonConcentrateMonster)
            {
                Troops formation =
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                        .GetFormationBarbarian(id);
                if (formation != null)
                {
                    go = formation.gameObject;
                }

            }
            else if (monsterData.rssType == RssType.Guardian)
            {
                Guardian formationGuardian =
                    WorldMapLogicMgr.Instance.GuardianHandler.GetFormationGuardian(id);
                if (formationGuardian != null)
                {
                    go = formationGuardian.gameObject;
                }
            }

            if (go == null)
            {
                return;
            }

            MonsterFightUiHudView hud = new MonsterFightUiHudView();
            hud.Create(monsterid, go, monsterData);

            dicHudUIFight.Add(monsterData.objectId, hud);
        }

        private void RemoveMonsterFightHud(int monsterId)
        {
            RemoveFightHud(monsterId);
        }

        #endregion

        #region 部队战斗hud

        private int armyFightId;

        private void PlayTroopFightHud(int troopId)
        {
            if (dicHudUIFight.ContainsKey(troopId))
            {
                return;
            }

            Troops go = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(troopId) as Troops;
            if (go == null)
            {
                return;
            }

            RemoveSelectHUD(0);

            PlayerFightUiHudView hud = new PlayerFightUiHudView();
            hud.Create(troopId, go.gameObject);

            dicHudUIFight.Add(troopId, hud);

            if (!dicStanceInfo.ContainsKey(troopId))
            {
                StanceInfo info = new StanceInfo();

                info.troopId = troopId;
                info.targetId = hud.targetId;
                info.stanceIndex = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().CalStanceIndex(hud.targetId, hud.stanceAngle);

                dicStanceInfo.Add(troopId, info);
            }            
        }

        private void SetFightBattleUIData(BattleUIData battleUiData)
        {
            if (battleUiData == null)
            {
                return;
            }

            if (!dicHudUIFight.ContainsKey(battleUiData.id))
            {
                return;
            }

            dicHudUIFight[battleUiData.id].SetBattleUIData(battleUiData);
        }

        private void UpdatePlayerTroopFightHudNum(int troopId)
        {
            if (!dicHudUIFight.ContainsKey(troopId))
            {
                return;
            }

            if (GameModeManager.Instance.CurGameMode != GameModeType.Expedition)
            {
                if (!m_TroopProxy.IsPlayOwnTroop(troopId))
                {
                    return;
                }
            }

            dicHudUIFight[troopId].ShowFightStatus();
        }

        private void RemoveTroopFightHud(int troopId)
        {
            RemoveFightHud(troopId);
        }

        private void RemoveFightHud(int id)
        {
            if (dicHudUIFight.ContainsKey(id))
            {
                dicHudUIFight[id].FadeOut();
                dicHudUIFight.Remove(id);
            }

            if (dicStanceInfo.ContainsKey(id))
            {
                RemoveTargetStanceInfo(dicStanceInfo[id]);
                UpdateStanceLayer(dicStanceInfo[id].targetId, dicStanceInfo[id].stanceIndex);

                dicStanceInfo.Remove(id);
            }
        }

        #endregion

        #region 城市战斗HUD

        private void PlayBuildingFightHud(int id)
        {
            if (dicHudUIFight.ContainsKey(id))
            {
                return;
            }

            MapObjectInfoEntity mapObjectInfoEntity = m_worldProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity == null)
            {
                return;
            }
            
            if (mapObjectInfoEntity.gameobject != null)
            {
                MapBuildingFightHudView hud = new MapBuildingFightHudView();
                hud.Create(id, mapObjectInfoEntity.gameobject, mapObjectInfoEntity);            
                dicHudUIFight.Add(id, hud);
            }

        }

        private void RemoveBuildingFightHud(int id)
        {
            RemoveFightHud(id);
            FightUiHudView hud = null;
            if (dicHudUIFight.TryGetValue(id, out hud))
            {
                MapBuildingFightHudView mapBuildingFightHudView = hud as MapBuildingFightHudView;
                mapBuildingFightHudView?.SetDefMaxValue();
                dicHudUIFight.Remove(id);
            }

        }


        private void UpdateBuildingFightHud(int id)
        {
            FightUiHudView fightUiHudView;
            if (dicHudUIFight.TryGetValue(id, out fightUiHudView))
            {
                MapBuildingFightHudView mapBuildingFightHudView = fightUiHudView as MapBuildingFightHudView;
                if (mapBuildingFightHudView != null)
                {
                    mapBuildingFightHudView.UpdateBuildingHead();
                }                
            }
        }

        private void UpdateBuildingFightHudTargetObj(MapObjectInfoEntity mapObjectInfo)
        {
            long id = mapObjectInfo.objectId;
            FightUiHudView fightUiHudView;
            if (dicHudUIFight.TryGetValue(id, out fightUiHudView))
            {
                MapBuildingFightHudView mapBuildingFightHudView = fightUiHudView as MapBuildingFightHudView;
                if (mapBuildingFightHudView != null && mapObjectInfo.gameobject != null)
                {
                    mapBuildingFightHudView.UpdateHudTargetObj(mapObjectInfo.gameobject);    
                }
            }
        }

        #endregion

        #region 符文采集HUD

        private void PlayCollectRuneHud(int id)
        {
            if (m_dictRuneHud.ContainsKey(id)) return;
            var mapOjbectInfo = m_worldProxy.GetWorldMapObjectByobjectId(id);
            if (mapOjbectInfo == null) return;
            var hudView = new RuneCollectHudView();
            hudView.Create(mapOjbectInfo);
            m_dictRuneHud.Add(id, hudView);
        }

        private void RemoveCollectRuneHud(int id)
        {
            RuneCollectHudView hudView = null;
            if (m_dictRuneHud.TryGetValue(id, out hudView))
            {
                hudView.Close();
                m_dictRuneHud.Remove(id);
            }
        }

        #endregion

        #region 战斗飘字HUD

        private Tween _tween;
        private HUDUI shootHud;
        private int index = 0;
        private int lastIndex = 0;
        private int indexBlood = 0;
        private int lastBloodIndex = 0;

        private readonly Dictionary<int, Dictionary<int, UI_Text_SkillAtk_SubView>> dicSkillAtk =
            new Dictionary<int, Dictionary<int, UI_Text_SkillAtk_SubView>>();

        private readonly Dictionary<int, Dictionary<int, UI_Text_SkillAddBlood_SubView>> dicAddBlood =
            new Dictionary<int, Dictionary<int, UI_Text_SkillAddBlood_SubView>>();


        private void PlayFightShootText(BattleUIData info)
        {
            if (info == null)
            {
                return;
            }

            MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(info.id);
            if (infoEntity == null)
            {
                return;
            }

            switch ((RssType)infoEntity.objectType)
            {
                case RssType.Troop:
                case RssType.Expedition:
                    SetShootTextTroopData(info);
                    break;
                case RssType.Monster:
                case RssType.Guardian:
                case RssType.SummonAttackMonster:
                case RssType.SummonConcentrateMonster:
                    SetShottTextMonsterData(info);
                    break;
                default:
                    SetBuildingFightShottTextData(info);
                    break;
            }
        }

        private void StopFightShootText(int id)
        {
            RemoveFightShottText(id);
        }

        private void CreateFightShottText(int id)
        {
            OnShootTextShow(id);
        }

        private void RemoveFightShottText(int id)
        {
            RemoveShootHud(id);
        }

        private void OnShootTextShow(int id)
        {
            if (dicShootHud.ContainsKey(id))
            {
                return;
            }

            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetArmyData(id);
            Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetTroop(id) as Troops;
            if (armyData != null && formation != null)
            {
                HUDUI shootHud = HUDUI
                    .Register(UI_Text_DamageDeadView.VIEW_NAME, typeof(UI_Text_DamageDeadView),
                        HUDLayer.fight, formation.gameObject).SetTargetGameObject(formation.gameObject).SetData(id)
                    .SetInitCallback(InitFightHudView)
                    .SetCameraLodDist(0, 3000, (isOn, view) =>
                    {
                        if (!isOn)
                        {
                            UI_Text_DamageDeadView ui = view.gameView as UI_Text_DamageDeadView;
                            RestShootView(ui);
                            foreach (var info in dicSkillAtk[id].Values)
                            {
                                info.RestPos();
                            }

                            foreach (var info in dicAddBlood[id].Values)
                            {
                                info.RestPos();
                            }
                        }
                    }).SetPosOffset(new Vector2(0, 0)).SetPositionAutoAnchor(true);
                HUDManager.Instance().ShowHud(shootHud);
                dicShootHud.Add(armyData.objectId, shootHud);
            }

            if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .IsContainTroop(id))
            {
                return;
            }

            MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(id);
            if (infoEntity != null)
            {
                HUDUI shootHud = HUDUI
                    .Register(UI_Text_DamageDeadView.VIEW_NAME, typeof(UI_Text_DamageDeadView),
                        HUDLayer.fight, infoEntity.gameobject).SetTargetGameObject(infoEntity.gameobject)
                    .SetData(id)
                    .SetInitCallback(InitFightHudView)
                    .SetCameraLodDist(500, 3000, (isOn, view) =>
                    {
                        if (!isOn)
                        {
                            UI_Text_DamageDeadView ui = view.gameView as UI_Text_DamageDeadView;
                            RestShootView(ui);
                            foreach (var info in dicSkillAtk[id].Values)
                            {
                                info.RestPos();
                            }

                            foreach (var info in dicAddBlood[id].Values)
                            {
                                info.RestPos();
                            }
                        }
                    }).SetPosOffset(new Vector2(0, 0)).SetPositionAutoAnchor(true);
                HUDManager.Instance().ShowHud(shootHud);
                dicShootHud.Add(infoEntity.objectId, shootHud);
            }
        }

        [IFix.Patch]
        private void RestShootView(UI_Text_DamageDeadView view)
        {
            if (view == null || view.gameObject == null)
            {
                return;
            }

            view.m_lbl_fail_LanguageText.gameObject.SetActive(false);
            view.m_lbl_rout_LanguageText.gameObject.SetActive(false);
            view.m_lbl_atkbyatk_LanguageText.gameObject.SetActive(false);
            view.m_lbl_deBuff_LanguageText.gameObject.SetActive(false);
            view.m_lbl_buff_LanguageText.gameObject.SetActive(false);
            view.m_lbl_ordinaryAtk_LanguageText.gameObject.SetActive(false);
            view.m_UI_CaptainLevelUpOnHead.gameObject.SetActive(false);
        }


        private void SetShootTextTroopData(BattleUIData info)
        {
            HUDUI hud;
            if (dicShootHud.TryGetValue(info.id, out hud))
            {
                SetShootViewData(hud, info);
            }
        }


        private void SetShottTextMonsterData(BattleUIData info)
        {
            HUDUI hud;
            if (dicShootHud.TryGetValue(info.id, out hud))
            {
                if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                    .IsContainTroop(info.id))
                {
                    return;
                }

                SetShootMonsterViewData(hud, info);
            }
        }

        private void SetBuildingFightShottTextData(BattleUIData info)
        {
            HUDUI hud;
            if (dicShootHud.TryGetValue(info.id, out hud))
            {
                if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                    .IsContainTroop(info.id))
                {
                    return;
                }

                SetBuildingFightView(hud, info);
            }
        }


        private void InitFightHudView(HUDUI info)
        {
            int id = (int)info.data;
            InitShootView(info, id);
        }

        private const int skiilAtkNum = 5;
        private void InitShootView(HUDUI info, int id)
        {
            UI_Text_DamageDeadView view = info.gameView as UI_Text_DamageDeadView;
            if (view == null)
            {
                return;
            }

            info.gameView.gameObject.GetComponent<MapElementUI>().enabled = false;
            if (!dicSkillAtk.ContainsKey(id))
            {
                dicSkillAtk[id] = new Dictionary<int, UI_Text_SkillAtk_SubView>();
            }

            if (!dicAddBlood.ContainsKey(id))
            {
                dicAddBlood[id] = new Dictionary<int, UI_Text_SkillAddBlood_SubView>();
            }

            for (int i = 0; i < skiilAtkNum; i++)
            {
                int index = i;
                CoreUtils.assetService.Instantiate("UI_Text_SkillAtk", (go) =>
                {
                    go.transform.SetParent(view.m_pl_skillAtkOffset);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    UI_Text_SkillAtk_SubView uiTextSkillAtkSubView =
                        new UI_Text_SkillAtk_SubView(go.GetComponent<RectTransform>());
                    if (dicSkillAtk.ContainsKey(id))
                    {
                        if (!dicSkillAtk[id].ContainsKey(index))
                        {
                            dicSkillAtk[id].Add(index, uiTextSkillAtkSubView);
                        }
                    }

                  
                });

                CoreUtils.assetService.Instantiate("UI_Text_SkillAddBlood", (go) =>
                {
                    go.transform.SetParent(view.m_pl_skillAtkOffset);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    UI_Text_SkillAddBlood_SubView uiTextSkillAddBloodSubView =
                        new UI_Text_SkillAddBlood_SubView(go.GetComponent<RectTransform>());
                    if (dicAddBlood.ContainsKey(id))
                    {
                        if (!dicAddBlood[id].ContainsKey(index))
                        {
                            dicAddBlood[id].Add(index, uiTextSkillAddBloodSubView);
                        }
                    }
                });
            }
        }

        private void SetShootViewData(HUDUI info, BattleUIData data)
        {
            if (info == null)
            {
                return;
            }

            UI_Text_DamageDeadView view = info.gameView as UI_Text_DamageDeadView;
            if (view == null)
            {
                return;
            }

            view.gameObject.name = string.Format("{0}_{1}_{2}", UI_Text_DamageDeadView.VIEW_NAME,
                "FightShottTextTroop", data.id);
            SetShootView(info, data);
        }


        private void SetShootMonsterViewData(HUDUI info, BattleUIData data)
        {
            if (info == null)
            {
                return;
            }

            UI_Text_DamageDeadView view = info.gameView as UI_Text_DamageDeadView;
            if (view == null)
            {
                return;
            }

            if (view.gameObject != null)
            {
                view.gameObject.name = string.Format("{0}_{1}_{2}", UI_Text_DamageDeadView.VIEW_NAME,
                    "FightShottTextMonster", data.id); 
            }
            SetShootView(info, data);
        }

        private void SetBuildingFightView(HUDUI info, BattleUIData data)
        {
            if (info == null)
            {
                return;
            }

            UI_Text_DamageDeadView view = info.gameView as UI_Text_DamageDeadView;
            if (view == null)
            {
                return;
            }
            if (view.gameObject == null)
            {
                Debug.LogWarningFormat("SetBuildingFightView error : invalid view gameObject, ID:{0}.", data.id);
                return;
            }
            view.gameObject.name = string.Format("{0}_{1}_{2}", UI_Text_DamageDeadView.VIEW_NAME,
                "FightShottTextBuilding", data.id);
            SetShootView(info, data);
        }


        private Timer _timer;

        public void FlyTo(Text graphic)
        {
            if (_timer != null)
            {
                _timer.Cancel();
            }

            _timer = Timer.Register(1, () =>
            {
                if (graphic == null)
                {
                    if (_timer != null)
                    {
                        _timer.Cancel();
                    }
                }

                if (graphic != null)
                {
                    graphic.gameObject.SetActive(false);
                }

                if (_timer != null)
                {
                    _timer.Cancel();
                }
            });
        }

        private void RemoveShootHud(int id)
        {
            HUDUI hudui = null;
            if (dicShootHud.TryGetValue(id, out hudui))
            {
                RestShootView(hudui.gameView as UI_Text_DamageDeadView);
                HUDManager.Instance().CloseSingleHud(ref hudui);
                dicShootHud.Remove(id);
            }

            if (dicSkillAtk.ContainsKey(id))
            {
                foreach (var skillAtkUI in dicSkillAtk[id].Values)
                {
                    skillAtkUI.RestPos();
                    if (skillAtkUI.gameObject != null)
                    {
                        CoreUtils.assetService.Destroy(skillAtkUI.gameObject);
                    }
                }
                dicSkillAtk[id].Clear();
                dicSkillAtk.Remove(id);
            }

            if (dicAddBlood.ContainsKey(id))
            {
                foreach (var addBloodUI in dicAddBlood[id].Values)
                {
                    addBloodUI.RestPos();
                    if (addBloodUI.gameObject != null)
                    {
                        CoreUtils.assetService.Destroy(addBloodUI.gameObject);
                    }
                }
                dicAddBlood[id].Clear();
                dicAddBlood.Remove(id);
            }
        }


        private void SetShootView(HUDUI info, BattleUIData data)
        {
            if (data == null || info == null|| info.gameView==null)
            {
                return;
            }

            UI_Text_DamageDeadView view = info.gameView as UI_Text_DamageDeadView;
            if (view == null)
            {
                return;
            }

            switch (data.type)
            {
                case BattleUIType.BattleUI_GeneralAttack:
                    view.m_lbl_ordinaryAtk_LanguageText.gameObject.SetActive(false);
                    view.m_lbl_ordinaryAtk_LanguageText.gameObject.SetActive(true);
                    view.m_lbl_ordinaryAtk_LanguageText.text = string.Format("{0}{1}", "-", data.parm1);
                    this.FlyTo(view.m_lbl_ordinaryAtk_LanguageText);
                    break;
                case BattleUIType.BattleUI_Fail:
                    view.m_lbl_fail_LanguageText.gameObject.SetActive(false);
                    view.m_lbl_ordinaryAtk_LanguageText.text = string.Empty;
                    view.m_lbl_fail_LanguageText.gameObject.SetActive(true);
                    break;
                case BattleUIType.BattleUI_Rout:
                    MapObjectInfoEntity monsterDa = m_worldProxy.GetWorldMapObjectByobjectId(data.id);
                    if (monsterDa != null)
                    {
                        view.m_lbl_fail_LanguageText.gameObject.SetActive(false);
                        bool isShow = WorldMapLogicMgr.Instance.MapTroopHandler
                                          .GetITroopMgr().IsShowEffect((int)monsterDa.objectId);
                        view.m_lbl_rout_LanguageText.gameObject.SetActive(isShow);
                        if (!TroopHelp.IsHaveState(monsterDa.status, ArmyStatus.BATTLEING))
                        {
                            view.m_lbl_ordinaryAtk_LanguageText.text = string.Empty;
                        }
                    }

                    break;
                case BattleUIType.BattleUI_Attack:
                    view.m_lbl_atkbyatk_LanguageText.gameObject.SetActive(false);
                    view.m_lbl_atkbyatk_LanguageText.gameObject.SetActive(true);
                    int num = (int)data.parm1;
                    string des = num == 2 ? "!" : num.ToString();
                    if (num <= 1)
                    {
                        view.m_lbl_atkbyatk_LanguageText.text = string.Empty;
                    }
                    else
                    {
                        view.m_lbl_atkbyatk_LanguageText.text = LanguageUtils.getTextFormat(300115, des);
                    }


                    break;
                case BattleUIType.BattleUI_HP:
                case BattleUIType.BattleUI_DOTHP:
                    string parm1 = string.Format("-{0}", data.parm1);
                    if (dicSkillAtk.ContainsKey(data.id))
                    {
                        if (dicSkillAtk[data.id].ContainsKey(lastIndex))
                        {
                            dicSkillAtk[data.id][lastIndex].RestPos();
                        }

                        if (dicSkillAtk[data.id].ContainsKey(index))
                        {
                            dicSkillAtk[data.id][index].SetDes(parm1, Vector2.zero);
                        }
                    }

                    index += 1;
                    if (index >= skiilAtkNum)
                    {
                        index = 0;
                    }

                    lastIndex = index;

                    break;
                case BattleUIType.BattleUI_AddBlood:
                case BattleUIType.BattleUI_HOT:
                    string parm = string.Format("+{0}", data.parm1);
                    if (dicAddBlood.ContainsKey(data.id))
                    {
                        if (dicAddBlood[data.id].ContainsKey(lastBloodIndex))
                        {                         
                            dicAddBlood[data.id][lastBloodIndex].RestPos();
                        }

                        if (dicAddBlood[data.id].ContainsKey(indexBlood))
                        {                          
                            dicAddBlood[data.id][indexBlood].SetDes(parm, Vector2.zero); 
                        }
                    }
          
                    indexBlood += 1;
                    if (indexBlood >= skiilAtkNum)
                    {
                        indexBlood = 0;
                    }

                    lastBloodIndex = indexBlood;

                    break;
                case BattleUIType.BattleUI_BuffIconRed:
                    view.m_lbl_deBuff_LanguageText.gameObject.SetActive(false);
                    view.m_lbl_deBuff_LanguageText.gameObject.SetActive(true);
                    view.m_lbl_deBuff_LanguageText.text = LanguageUtils.getTextFormat((int)data.parm1);
                    break;
                case BattleUIType.BattleUI_BuffIconGreen:
                    view.m_lbl_buff_LanguageText.gameObject.SetActive(false);
                    view.m_lbl_buff_LanguageText.gameObject.SetActive(true);
                    view.m_lbl_buff_LanguageText.text = LanguageUtils.getTextFormat((int)data.parm1);
                    break;
                case BattleUIType.UpdateHeroLevel:
                    HeroProxy.Hero hero =m_HeroProxy.GetHeroByID((int) data.parm1);
                    if (hero != null)
                    {
                        view.m_UI_CaptainLevelUpOnHead.SetData(hero);
                    }
                    break;
            }
        }

        #endregion

        #endregion
    }
}