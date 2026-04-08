using System;
using System.Collections.Generic;
using System.Linq;
using Client;
using Game;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Hotfix
{
    public sealed class TroopMgr : ITroopMgr
    {
        private SquareHelper squareHelper;
        private Dictionary<int, Troops> dicFormation = new Dictionary<int, Troops>();
        private Dictionary<int, ArmyData> m_worldArmyData = new Dictionary<int, ArmyData>();
        private PlayerProxy m_PlayerProxy;
        private ScoutProxy m_scoutProxy;
        private TroopProxy m_troopProxy;
        private WorldMapObjectProxy m_WorldMapObjectProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private Timer m_Timer;

        private Dictionary<int, HashSet<int>> m_dicBeAttackerSet = new Dictionary<int, HashSet<int>>();
        private Dictionary<int, int> m_dicAttacker = new Dictionary<int, int>();
        private Dictionary<int, int> m_dicBeAttacker = new Dictionary<int, int>();

        public TroopMgr()
        {
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_WorldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_RallyTroopsProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            CellDatas.InitSquareData_S();
            squareHelper = m_troopProxy.GetSquareHelper();
        }

        public void CreateTroopData(INotification notification)
        {
            Map_ObjectInfo.request mapItemInfo = notification.Body as Map_ObjectInfo.request;
            if (mapItemInfo != null)
            {
                if (mapItemInfo.mapObjectInfo.objectId <= 0)
                {
                    return;
                }

                ArmyData armyData = CreateArmyData(mapItemInfo);
                if (armyData == null)
                {
                    return;
                }

                InitArmyDataByMapObjectInfo(armyData, mapItemInfo.mapObjectInfo);
                AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopGo, armyData.objectId);
            }
        }

        ArmyData CreateArmyData(Map_ObjectInfo.request mapItemInfo)
        {
            ArmyData armyData = null;
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                {
                    if (IsPlayerHaveTroop(mapItemInfo.mapObjectInfo.armyRid))
                    {
                        armyData = SummonerTroopMgr.Instance.GetISummonerTroop()
                            .AddSummonerArmyByMapObjectInfo(mapItemInfo.mapObjectInfo);
                        armyData.FillMapObjectInfo((int) mapItemInfo.mapObjectInfo.objectId);
                    }
                    else
                    {
                        if (!m_worldArmyData.ContainsKey((int) mapItemInfo.mapObjectInfo.objectId))
                        {
                            armyData = new ArmyData((int) mapItemInfo.mapObjectInfo.objectId);
                            m_worldArmyData[armyData.objectId] = armyData;
                        }
                    }
                }
                    break;
                case GameModeType.Expedition:
                    armyData = SummonerTroopMgr.Instance.ExpeditionTroop.CreateArmyData(mapItemInfo.mapObjectInfo);
                    break;
            }

            return armyData;
        }

        public void UpdateArmyData(int objectId, MapObjectInfo info)
        {
            ArmyData armyData = GetArmyData(objectId);
            if (armyData == null) return;
            if (info.HasObjectPos)
            {
                armyData.Pos = new Vector2(info.objectPos.x / 100f, info.objectPos.y / 100f);
                if (Application.isEditor)
                {
                    Color color;
                    ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                    CoreUtils.logService.Debug($"{objectId}\tBattleData: Pos:{armyData.Pos}", color);
                }
            }

            if (info.HasMainHeroId)
            {
                armyData.heroId = (int) info.mainHeroId;
            }

            if (info.HasDeputyHeroId)
            {
                armyData.viceId = (int) info.deputyHeroId;
            }

            if (info.HasStatus)
            {
                armyData.armyStatus = info.status;
            }

            if (info.HasArrivalTime)
            {
                armyData.arrivalTime = info.arrivalTime;
            }

            if (info.HasStartTime)
            {
                armyData.startTime = info.startTime;
            }

            if (info.HasObjectId)
            {
                armyData.objectId = (int) info.objectId;
            }

            if (info.HasArmyCountMax)
            {
                armyData.troopNumMax = (int)info.armyCountMax;
                if (Application.isEditor)
                {
                    Color color;
                    ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                    CoreUtils.logService.Debug($"{objectId}\tBattleData: troopNumMax:{armyData.troopNumMax}", color);
                }
            }

            if (info.HasArmyCount)
            {
                armyData.troopNums = (int) info.armyCount;
                if (Application.isEditor)
                {
                    Color color;
                    ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                    CoreUtils.logService.Debug($"{objectId}\tBattleData: troopNums:{armyData.troopNums}", color);
                }
            }
        }

        private void InitArmyDataByMapObjectInfo(ArmyData armyData, MapObjectInfo info)
        {
            UpdateArmyData((int) info.objectId, info);

            if (info.HasObjectPath)
            {
                armyData.ClearMovePath();
                foreach (var path in info.objectPath)
                {
                    Vector2 v2 = new Vector2(path.x / 100.0f, path.y / 100.0f);
                    armyData.SetMovePath(v2);
                }

                armyData.autoMoveIndex = armyData.GetMoveIndex();
            }

            if (info.HasObjectPath && info.objectPath.Count > 0)
            {
                armyData.FormationInitTargetPos = new Vector2(info.objectPath[armyData.autoMoveIndex].x / 100f,
                    info.objectPath[armyData.autoMoveIndex].y / 100f);
            }
            else
            {
                Vector2 t = armyData.Pos;
                t = t + TroopHelp.Rotated(Vector2.right, armyData.attackAngle);                
                armyData.FormationInitTargetPos = t;
            }

            armyData.soldiers = info.soldiers;
            string des = string.Empty;
            RssType objectType = (RssType) info.objectType;
            switch (objectType)
            {
                case RssType.Transport:
                    des = squareHelper.GetMapCreateTroopDes(10, 0, null);
                    break;
                case RssType.Scouts:
                    des = squareHelper.GetMapCreateTroopDes((int) info.scoutsIndex, 0, null);
                    break;
                case RssType.Troop:
                {
                    if (armyData.isRally)
                    {
                        des = squareHelper.GetMapCreateTroopDes((int) info.mainHeroId,
                            (int) info.deputyHeroId, info.soldiers,
                            Troops.ENMU_MATRIX_TYPE.RALLY);
                    }
                    else
                    {
                        des = squareHelper.GetMapCreateTroopDes((int) info.mainHeroId,
                            (int) info.deputyHeroId, info.soldiers);
                    }
                }
                    break;
                case RssType.Expedition:
                    des = squareHelper.GetMapCreateTroopDes((int) info.mainHeroId,
                        (int) info.deputyHeroId, info.soldiers);
                    break;
            }

            armyData.isCreate = true;
            armyData.troopNumMax = (int) info.armyCountMax;
            armyData.des = des;
        }

        public void Clear()
        {
            foreach (var formation in dicFormation.Values)
            {
                CoreUtils.assetService.Destroy(formation.gameObject);
            }

            dicFormation.Clear();
            WorldArmyDataClear();
            m_dicAttacker.Clear();
            if (m_Timer != null)
            {
                m_Timer.Cancel();
            }
        }
        
        private void WorldArmyDataClear()
        {
            foreach (var keyValue in m_worldArmyData)
            {
                RemoveSound(keyValue.Value.objectId);
            }
            
            m_worldArmyData.Clear();
        }


        public ArmyData GetArmyData(int objectId)
        {
            if (objectId == 0)
            {
                return null;
            }

            ArmyData data = null;
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    if (!m_worldArmyData.TryGetValue(objectId, out data))
                    {
                        data = SummonerTroopMgr.Instance.GetISummonerTroop().GetArmyDataByObjectId(objectId);
                    }

                    break;
                case GameModeType.Expedition:
                    data = SummonerTroopMgr.Instance.ExpeditionTroop.GetArmyData(objectId);
                    break;
            }

            return data;
        }

        public ArmyData GetArmyDataByArmyId(int id)
        {
            return SummonerTroopMgr.Instance.GetISummonerTroop().GetArmyData(RssType.Troop, id);
        }

        public ArmyData GetTransportData(int objectId)
        {
            return SummonerTroopMgr.Instance.GetISummonerTroop().GetArmyDataByObjectId(RssType.Transport, objectId);
        }

        public ArmyData GetTransportDataById(int transportId)
        {
            return SummonerTroopMgr.Instance.GetISummonerTroop().GetArmyData(RssType.Transport, transportId);
        }

        public ArmyData GetScoutData(int objectId)
        {
            return SummonerTroopMgr.Instance.GetISummonerTroop().GetArmyDataByObjectId(RssType.Scouts, objectId);
        }

        public ArmyData GetScoutDataByScoutId(int id)
        {
            return SummonerTroopMgr.Instance.GetISummonerTroop().GetArmyData(RssType.Scouts, id);
        }

        public bool IsShowEffect(int id)
        {
            foreach (var info in SummonerTroopMgr.Instance.GetISummonerTroop().GetSummonerArmyDatas())
            {
                if (info.targetId == id)
                {
                    if (info.isPlayerHave)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool isHeroPlaySkill(int id, int skillId)
        {
            bool isShow = false;
            ArmyData armyData = GetArmyData(id);
            if (armyData != null && armyData.mainSkillInfo != null)
            {
                armyData.mainSkillInfo.ForEach((info) =>
                {
                    if (info.skillId == skillId)
                    {
                        isShow = true;
                    }
                });
            }

            return isShow;
        }

        public bool isVicePlaySkill(int id, int skillId)
        {
            bool isShow = false;
            ArmyData armyData = GetArmyData(id);
            if (armyData != null && armyData.viceSkillInfo != null)
            {
                armyData.viceSkillInfo.ForEach((info) =>
                {
                    if (info.skillId == skillId)
                    {
                        isShow = true;
                    }
                });
            }

            return isShow;
        }

        public bool IsRallyTroop(int id)
        {
            ArmyData armyData = GetArmyData(id);
            if (armyData != null)
            {
                return armyData.isRally;
            }

            return false;
        }

        private bool IsPlayerHaveTroop(long rid)
        {
            return m_PlayerProxy.CurrentRoleInfo.rid == rid;
        }


        #region 部队伺候map实体管理

        private const string formation = "Formation";
        private const string m_troops_root_path = "SceneObject/Troops_root";
        private Transform m_troops_root;

        public void AddTroop(int troopId, Action callback)
        {
            if (troopId <= 0)
            {
                return;
            }

            if (dicFormation.ContainsKey(troopId))
            {
                Debug.LogErrorFormat("已经生成了这个{0}的部队了", troopId);
                return;
            }

            ArmyData armyData = GetArmyData(troopId);
            if (armyData == null)
            {
                Debug.LogError("数据空了" + troopId);
                return;
            }

            CoreUtils.assetService.Instantiate(formation, (GameObject go) =>
            {
                ArmyData army = GetArmyData(troopId);
                if (army == null)
                {
                    CoreUtils.assetService.Destroy(go);
                    return;
                }
                Transform t = this.GetTroopsRoot();
                go.transform.SetParent(t);
                go.transform.localScale = Vector3.one;
                go.name = string.Format("{0}_{1}", formation, troopId);
                Troops m_Formation = go.GetComponent<Troops>();
                var pos = armyData.Pos;
                if (armyData.movePath.Count > 0)
                {
                    pos = armyData.GetMovePos();
                }

                Troops.InitPositionS(m_Formation, pos, armyData.FormationInitTargetPos);
                Troops.SetRadiusS(m_Formation, armyData.armyRadius);
                if (armyData.isPlayerHave || (m_PlayerProxy.CurrentRoleInfo.guildId != 0 && armyData.guild == m_PlayerProxy.CurrentRoleInfo.guildId))
                {
                    Troops.InitFormationS(m_Formation, armyData.des, RS.blue_troop);
                }
                else
                {
                    Troops.InitFormationS(m_Formation, armyData.des, RS.red_troop);
                }
                Troops.FadeIn_S(m_Formation);
                armyData.go = go;
                AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopHud, armyData.objectId);

                if (!dicFormation.ContainsKey(troopId))
                {
                    dicFormation.Add(troopId, m_Formation);
                }

                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        }

        public Troops GetTroop(int troopId)
        {
            return GetFormation(troopId);
        }

        public List<int> GetArmyDatas()
        {
            List<int> armys = new List<int>();
            foreach (var id in m_worldArmyData.Keys)
            {
                armys.Add(id);
            }

            foreach (var summonerArmyData in SummonerTroopMgr.Instance.GetISummonerTroop().GetSummonerArmyDatas())
            {
                armys.Add(summonerArmyData.objectId);
            }

            return armys;
        }

        public List<Troops> GetTroops()
        {
            List<Troops> formations = new List<Troops>();
            foreach (var formationValue in dicFormation.Values)
            {
                formations.Add(formationValue);
            }

            return formations;
        }

        public Troops GetFormationBarbarian(int id)
        {
            return GetFormationBarbarians(id);
        }

        public bool IsContainTroop(int id)
        {
            Troops formation = GetFormation(id);
            return formation != null;
        }

        public bool IsContainBarbarian(int id)
        {
            return GetFormationBarbarians(id);
        }


        public void ChangeFormationState(Troops.ENMU_MATRIX_TYPE type, int objectId,
            Troops.ENMU_SQUARE_STAT state,
            Vector2 current_pos,
            Vector2 target_pos, float move_speed = 2f)
        {
            if (type == Troops.ENMU_MATRIX_TYPE.BARBARIAN)
            {
                Troops formation = GetFormationBarbarians(objectId);
                if (formation != null)
                {
                    formation.SetState(state, current_pos, target_pos, move_speed);
                }
            }
            else
            {
                Troops formation = GetFormation(objectId);
                if (formation != null)
                {
                    formation.SetState(state, current_pos, target_pos, move_speed);
                }
            }
        }

        public void SwitchShowMode(int troopId, string info)
        {
            Troops formation = this.GetFormation(troopId);
            if (formation != null)
            {
                Troops.ReservedFunc1S(formation, info);
            }
        }

        public bool RemoveTroop(int id)
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    ArmyData checkArmyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                    if (checkArmyData != null)
                    {
                        //自己的部队删除请走对应接口 MapObjectDelete推送不删除自己的部队
                        if (checkArmyData.isPlayerHave&& !checkArmyData.isRally && !checkArmyData.isGuide)
                        {
                            return false;
                        }
                    }

                    RemoveTroopGo(id);

                    if (m_worldArmyData.ContainsKey(id))
                    {
                        m_worldArmyData.Remove(id);
                    }

                    break;
                case GameModeType.Expedition:
                    {
                        ArmyData armyData = SummonerTroopMgr.Instance.ExpeditionTroop.GetArmyData(id);
                        //部队死亡,做一个溃败表现
                        if (armyData != null)
                        {
                            SummonerTroopMgr.Instance.ExpeditionTroop.PlayArmyDeadPerofrmance(id, GetFormation(id));
                            Timer.Register(1f, () =>
                            {
                                RemoveTroopGo(id);
                                SummonerTroopMgr.Instance.ExpeditionTroop.RemoveArmyData(id);
                            });
                        }
                        else
                        {
                            RemoveTroopGo(id);
                            if (m_worldArmyData.ContainsKey(id))
                            {
                                m_worldArmyData.Remove(id);
                            }
                        }
                    }
                    break;
            }
            return true;
        }

        public bool RemoveOwnTroop(int id)
        {
            RemoveTroopGo(id);
            return true;
        }

        public void RemoveSound(int id)
        {
            ArmyData armyData = GetArmyData(id);
            if (armyData != null)
            {
                armyData.RemoveSoundHandler();
            }
        }

        public void SetFormationInfo(int id, string des)
        {
            Troops formation = GetFormation(id);
            if (formation != null)
            {
                Troops.SetFormationInfoS(formation, des);
            }
            else
            {
                Troops formationBar = GetFormationBarbarians(id);
                if (formationBar != null)
                {
                    Troops.SetFormationInfoS(formationBar, des);
                }
            }
        }


        public void TriggerSkillS(int id, string heroId, Vector3 pos)
        {
            Troops formation = GetFormation(id);
            if (formation != null)
            {
                Troops.TriggerSkillS(formation, heroId, pos);
            }
            else
            {
                Troops formationBar = GetFormationBarbarians(id);
                if (formationBar != null)
                {
                    Troops.TriggerSkillS(formation, heroId, pos);
                }
            }
        }


        private Troops GetFormation(int troopId)
        {
            Troops formation = null;
            if (dicFormation.TryGetValue(troopId, out formation))
            {
                return formation;
            }

            return null;
        }


        private Troops GetFormationBarbarians(int id)
        {
            ///TODD:暂时先屏蔽部队的
            if (dicFormation.ContainsKey(id))
            {
                return null;
            }

            MapObjectInfoEntity mapObjectExtEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectExtEntity == null)
            {
                return null;
            }

            if ((mapObjectExtEntity.rssType == RssType.Monster ||
                 mapObjectExtEntity.rssType == RssType.SummonAttackMonster ||
                 mapObjectExtEntity.rssType == RssType.SummonConcentrateMonster) && mapObjectExtEntity.gameobject != null)
            {
                return mapObjectExtEntity.gameobject.GetComponent<Troops>();
            }

            return null;
        }


        private void RemoveTroopGo(int id)
        {
            RemoveSound(id);
            Troops formation;
            if (dicFormation.TryGetValue(id, out formation))
            {
               // Vector2 dir = new Vector2(formation.transform.forward.x, formation.transform.forward.z).normalized * 0.01f;
               // Vector2 pos = new Vector2(formation.transform.position.x, formation.transform.position.z).normalized * 0.01f;
                //Formation.SetStateS(formation, Formation.ENMU_SQUARE_STAT.IDLE, pos, pos + dir);
                Troops.FadeOut_S(formation);
                //要先做完淡出表现
                m_Timer=  Timer.Register(0.2f, () =>
                {
                    if (formation != null && formation.gameObject != null)
                    {
                        CoreUtils.assetService.Destroy(formation.gameObject);  
                        dicFormation.Remove(id);
                       // Debug.LogError("通知删除AOI线"+id);
                        WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().DeleteAOITroopLines(id);
                    }
                }); 
                AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveTroopHud, id);
                AppFacade.GetInstance().SendNotification(CmdConstant.MapStopShottTextHud, id);
                WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveTroopLine(id);
                WorldMapLogicMgr.Instance.BattleUIHandler.Remove(id);
                WorldMapLogicMgr.Instance.BattleBuffHandler.ClearBuff(id);
            }
        }


        private Transform GetTroopsRoot()
        {
            if (this.m_troops_root == null)
            {
                this.m_troops_root = GameObject.Find(m_troops_root_path).transform;
            }

            return this.m_troops_root;
        }

        public void UpdateArmyDir()
        {
            foreach (var troopId in dicFormation.Keys)
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(troopId);
                if (armyData != null)
                {
                    if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.BATTLEING) ||
                        TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.SPACE_MARCH | ArmyStatus.BATTLEING))
                    {
                        HashSet<int> attackerIdSet;
                        if (m_dicBeAttackerSet.TryGetValue(troopId, out attackerIdSet))
                        {
                            foreach (var attackerId in attackerIdSet)
                            {
                                MapObjectInfoEntity infoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(attackerId);
                                if (infoEntity != null)
                                {
                                    if (infoEntity.objectType == (long)RssType.Monster ||
                                        infoEntity.objectType == (long)RssType.Guardian ||
                                        infoEntity.objectType == (long)RssType.SummonAttackMonster ||
                                        infoEntity.objectType == (long)RssType.SummonConcentrateMonster)
                                    {
                                        if (infoEntity.status == (int)ArmyStatus.BATTLEING)
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.FightUpdateMonsteAttackDir, attackerId);
                                        }
                                    }
                                    else if (infoEntity.objectType == (long)RssType.Troop || infoEntity.objectType == (long)RssType.Expedition)
                                    {
                                        if (infoEntity.status == (int)ArmyStatus.BATTLEING || infoEntity.status == (int)(ArmyStatus.BATTLEING | ArmyStatus.STATIONING))
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.FightUpdateTroopAttackDir, attackerId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateAttackerDir(int beAttackId)
        {
            HashSet<int> attackerIdSet;
            if (m_dicBeAttackerSet.TryGetValue(beAttackId, out attackerIdSet))
            {
                foreach (var attackerId in attackerIdSet)
                {
                    MapObjectInfoEntity mapObjectInfo = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(attackerId);
                    if (mapObjectInfo != null)
                    {
                        WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(attackerId, (int)mapObjectInfo.status);
                    }
                }
            }
        }

        public void UpdateTarget(int objectId, int target_id)
        {
            //调试使用
            //Debug.Log("UpdateTarget: " + objectId + "-->" + target_id);

            //增援行军无需刷新攻击受击关系
            MapObjectInfoEntity attackerObjectInfo = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(objectId);
            if (attackerObjectInfo != null)
            {
                if (TroopHelp.IsHaveState(attackerObjectInfo.status, ArmyStatus.REINFORCE_MARCH))
                {
                    return;
                }
            }

            // 被攻击者字典
            if (target_id != 0)
            {
                HashSet<int> attacker;
                if (!m_dicBeAttackerSet.TryGetValue(target_id, out attacker))
                {
                    attacker = new HashSet<int>();
                    attacker.Add(objectId);
                    m_dicBeAttackerSet.Add(target_id, attacker);
                }
                else
                {
                    if (!attacker.Contains(objectId))
                    {
                        attacker.Add(objectId);
                    }
                }

                // 被攻击者字典
                if (!m_dicBeAttacker.ContainsKey(target_id))
                {
                    m_dicBeAttacker.Add(target_id, objectId);
                }

                // 相互攻击关系优先 
                if (m_dicBeAttacker.ContainsKey(objectId))
                {
                    HashSet<int> attackerSet;
                    if (m_dicBeAttackerSet.TryGetValue(objectId, out attackerSet))
                    {
                        if (attackerSet.Contains(target_id))
                        {
                            m_dicBeAttacker[objectId] = target_id;
                        }
                    }
                }
            }
            else
            {
                int id;
                // 如果原来有攻击目标
                if (m_dicAttacker.TryGetValue(objectId, out id))
                {
                    HashSet<int> attackerSet;
                    // 获取以前的被攻击者的 攻击者列表
                    if (m_dicBeAttackerSet.TryGetValue(id, out attackerSet))
                    {
                        // 攻击列表中移除掉攻击者
                        attackerSet.Remove(objectId);

                        int atkId;
                        if (m_dicBeAttacker.TryGetValue(id, out atkId))
                        {
                            if (!attackerSet.Contains(atkId))
                            {
                                if (attackerSet.Count == 0)
                                {
                                    // 清除攻击者
                                    m_dicBeAttacker.Remove(id);
                                }
                                else
                                {
                                    // 调整攻击者
                                    m_dicBeAttacker[id] = attackerSet.First();
                                }
                            }
                        }
                    }
                }
            }

            // 攻击者字典
            if (!m_dicAttacker.ContainsKey(objectId))
            {
                m_dicAttacker.Add(objectId, target_id);
            }

            m_dicAttacker[objectId] = target_id;
        }

        public bool GetAttackerPos(int beAttackId, out int attackerId, out Vector3 attackerPos)
        {
            attackerId = 0;
            attackerPos = Vector3.zero;
            
            m_dicBeAttacker.TryGetValue(beAttackId, out attackerId);
            if (attackerId == 0)
            {
                m_dicAttacker.TryGetValue(beAttackId, out attackerId);
            }

            if (attackerId == 0)
                return false;
            Troops attackTroop = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(attackerId);
            if (attackTroop != null)
            {
                attackerPos = new Vector3(attackTroop.transform.position.x, 0, attackTroop.transform.position.z);
                return true;
            }

            Troops monster = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetFormationBarbarian(attackerId);
            if (monster != null)
            {
                attackerPos = new Vector3(monster.transform.position.x, 0, monster.transform.position.z);
                return true;
            }

            MapObjectInfoEntity attackBuilding = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(attackerId);
            if (attackBuilding != null)
            {
                if (attackBuilding.gameobject != null)
                {
                    attackerPos = new Vector3(attackBuilding.gameobject.transform.position.x, 0, attackBuilding.gameobject.transform.position.z);
                }
                else
                {
                    attackerPos = PosHelper.ServerUnitToClientUnit(attackBuilding.objectPos);
                }
                
                return true;
            }
            else
            {
                Troops troop = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(beAttackId);
                if (troop != null)
                {
                    attackerPos = new Vector3(troop.transform.position.x, 0, troop.transform.position.z) +
                                  new Vector3(troop.transform.forward.x, 0, troop.transform.forward.z);
                    return true;
                }
            }

            return false;
        }

        public int CalStanceIndex(int targetId, float stanceAngle)
        {
            int stanceIndex = 0;

            MapObjectInfoEntity mapObjectInfo = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(targetId);
            if (mapObjectInfo !=  null)
            {
                //8个站位点
                if (mapObjectInfo.objectType == (long)RssType.Troop ||
                    mapObjectInfo.objectType == (long)RssType.Monster ||
                    mapObjectInfo.objectType == (long)RssType.City ||
                    mapObjectInfo.objectType == (long)RssType.Stone ||
                    mapObjectInfo.objectType == (long)RssType.Farmland ||
                    mapObjectInfo.objectType == (long)RssType.Wood ||
                    mapObjectInfo.objectType == (long)RssType.Gold ||
                    mapObjectInfo.objectType == (long)RssType.Gem ||
                    mapObjectInfo.objectType == (long)RssType.GuildCenter ||
                    mapObjectInfo.objectType == (long)RssType.GuildFortress1 ||
                    mapObjectInfo.objectType == (long)RssType.GuildFortress2 ||
                    mapObjectInfo.objectType == (long)RssType.GuildFlag ||
                    mapObjectInfo.objectType == (long)RssType.BarbarianCitadel ||
                    mapObjectInfo.objectType == (long)RssType.Guardian ||
                    mapObjectInfo.objectType == (long)RssType.SummonAttackMonster ||
                    mapObjectInfo.objectType == (long)RssType.SummonConcentrateMonster ||
                    mapObjectInfo.objectType == (long)RssType.Expedition)
                {
                    if ((stanceAngle >= 0 && stanceAngle < 22.5) || (stanceAngle >= 337.5 && stanceAngle <= 360))
                    {
                        stanceIndex = 1;
                    }
                    else
                    {
                        stanceIndex = (int)Mathf.Ceil((stanceAngle - 22.5f) / 45) + 1;
                    }
                }
                //12个站位点
                else if (mapObjectInfo.objectType == (long)RssType.HolyLand ||
                         mapObjectInfo.objectType == (long)RssType.Sanctuary ||
                         mapObjectInfo.objectType == (long)RssType.Altar ||
                         mapObjectInfo.objectType == (long)RssType.Shrine ||
                         mapObjectInfo.objectType == (long)RssType.LostTemple)
                {
                    if ((stanceAngle >= 0 && stanceAngle < 15) || (stanceAngle >= 345 && stanceAngle <= 360))
                    {
                        stanceIndex = 1;
                    }
                    else
                    {
                        stanceIndex = (int)Mathf.Ceil((stanceAngle - 15) / 30) + 1;
                    }
                }
                //6个站位点
                else if (mapObjectInfo.objectType == (long)RssType.CheckPoint ||
                         mapObjectInfo.objectType == (long)RssType.Checkpoint_1 ||
                         mapObjectInfo.objectType == (long)RssType.Checkpoint_2 ||
                         mapObjectInfo.objectType == (long)RssType.Checkpoint_3)
                {
                    if ((stanceAngle >= 0 && stanceAngle < 22.5) || (stanceAngle >= 337.5 && stanceAngle <= 360))
                    {
                        stanceIndex = 1;
                    }
                    else if (stanceAngle >= 22.5 && stanceAngle < 67.5)
                    {
                        stanceIndex = 2;
                    }
                    else if (stanceAngle >= 112.5 && stanceAngle < 157.5)
                    {
                        stanceIndex = 3;
                    }
                    else if (stanceAngle >= 157.5 && stanceAngle < 202.5)
                    {
                        stanceIndex = 4;
                    }
                    else if (stanceAngle >= 202.5 && stanceAngle < 247.5)
                    {
                        stanceIndex = 5;
                    }
                    else if (stanceAngle >= 292.5 && stanceAngle <= 360)
                    {
                        stanceIndex = 6;
                    }
                }
            }

            return stanceIndex;
        }
        public void UpdateTroopsColor()
        {
            foreach (var troopElement in dicFormation)
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(troopElement.Key);
                if (armyData != null)
                {
                    if (armyData.isPlayerHave || (m_PlayerProxy.CurrentRoleInfo.guildId != 0 && armyData.guild == m_PlayerProxy.CurrentRoleInfo.guildId))
                    {
                        Troops.ChangeUnitColorS(troopElement.Value, RS.blue_troop);
                    }
                    else
                    {
                        Troops.ChangeUnitColorS(troopElement.Value, RS.red_troop);
                    }
                }
            }
        }
        public void UpdateTroopColor(int troopId)
        {
            Troops formation;
            if (dicFormation.TryGetValue(troopId, out formation))
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(troopId);
                if (armyData != null)
                {
                    if (armyData.isPlayerHave || (m_PlayerProxy.CurrentRoleInfo.guildId != 0 && armyData.guild == m_PlayerProxy.CurrentRoleInfo.guildId))
                    {
                        Troops.ChangeUnitColorS(formation, RS.blue_troop);
                    }
                    else
                    {
                        Troops.ChangeUnitColorS(formation, RS.red_troop);
                    }
                }
            }
        }
        #endregion
    }
}