using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine;
using Client;

namespace Hotfix
{
    public sealed class SummonerTroop : ISummonerTroop
    {
        private readonly Dictionary<RssType, Dictionary<int, ArmyData>> m_dicSummonerTroop = new Dictionary<RssType, Dictionary<int, ArmyData>>();
        private PlayerProxy m_playerProxy;
        private TroopProxy m_TroopProxy;
        private ScoutProxy m_ScoutProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_ScoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;

            m_dicSummonerTroop[RssType.Troop] = new Dictionary<int, ArmyData>();
            m_dicSummonerTroop[RssType.Scouts] = new Dictionary<int, ArmyData>();
            m_dicSummonerTroop[RssType.Transport] = new Dictionary<int, ArmyData>();
        }

        public void Clear()
        {
            foreach(var kv in m_dicSummonerTroop)
            {
  
                foreach (var keyValue in kv.Value)
                {
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().RemoveSound(keyValue.Value.objectId);
                }

                kv.Value.Clear();
            }
        }

        #region 自己的部队
        public void UpdateSummonerTroop(INotification data)
        {
            Army_ArmyList.request req = data.Body as Army_ArmyList.request;
            if (req == null)
            {
                return;
            }
            foreach(var kv in req.armyInfo)
            {
                ArmyInfo info = kv.Value;
                if (info == null || !info.HasArmyIndex)
                {
                    continue;
                }
                if(info.HasMainHeroId && info.mainHeroId == 0)
                {
                    RemoveArmyData(RssType.Troop, (int)info.armyIndex);
                }
                else
                {
                    int armyIndex = (int)info.armyIndex;
                    ArmyData armyData = GetArmyData(RssType.Troop, armyIndex);
                    if (armyData == null)
                    {
                        armyData = CreateTroop(info, armyIndex);
                    }
                    if (armyData == null) continue;
                    UpdateTroop(armyData, info);
                }               
            }           
        }

        private ArmyData CreateTroop(ArmyInfo info, int armyId)
        {
            ArmyData armyData = new SelfTroopData(armyId);
            if(info.HasTargetArg && info.targetArg.HasPos/* &&  
                 TroopHelp.IsHaveAnyState(armyData.armyStatus, (long)(ArmyStatus.GARRISONING | ArmyStatus.GARRISONING | ArmyStatus.COLLECTING | ArmyStatus.RALLY_WAIT))*/)
            {
                if (TroopHelp.GetTroopState(armyData.armyStatus) != Troops.ENMU_SQUARE_STAT.MOVE)
                {
                    armyData.Pos = new Vector2(info.targetArg.pos.x / 100f, info.targetArg.pos.y / 100f);
                }
            }
            m_dicSummonerTroop[RssType.Troop].Add(armyId, armyData);
            if (info.objectIndex > 0)
            {
                armyData.objectId = (int) info.objectIndex;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopAdd,armyData);
            return armyData;
        }

        private void UpdateTroop(ArmyData armyData, ArmyInfo info)
        {
            if (info.HasStatus)
            {
                armyData.armyStatus = info.status;
                var state = TroopHelp.GetTroopState(info.status);
                ArmyData delectArmyDatarmy = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(armyData.objectId);
                switch (state)
                {
                    case Troops.ENMU_SQUARE_STAT.FIGHT:
                    case Troops.ENMU_SQUARE_STAT.IDLE:
                        {
                            if (delectArmyDatarmy != null)
                            {
                                if (!delectArmyDatarmy.isRally)
                                {                                 
                                   WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveSummonerTroopLine(armyData.objectId);  
                                }
                            }
                        }
                        break;
                }
                
                if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.COLLECTING) ||
                    TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.GARRISONING) ||
                    TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.RALLY_WAIT)||
                    TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.PALLY_MARCH)||
                    TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.RALLY_BATTLE)                  
                    )
                {      
                    if (armyData.isPlayerHave)
                    {
                        if (delectArmyDatarmy != null)
                        {
                            if (!delectArmyDatarmy.isRally)
                            {
                                //Debug.LogError("召唤师自己删除了"+armyData.armyName+"***"+armyData.isPlayerHave+"***"+armyData.objectId);
                                WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveSummonerTroopLine(armyData.objectId);
                                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().RemoveOwnTroop(armyData.objectId);
                            }
                        }
                    }
                }
            }
            if (info.HasMainHeroId)
            {
                armyData.heroId = (int)info.mainHeroId;
            }
            if (info.HasDeputyHeroId)
            {
                armyData.viceId = (int)info.deputyHeroId;
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
            if (info.HasObjectIndex)
            {
                armyData.objectId = (int)info.objectIndex;
            }
            if (info.HasTargetArg/* &&  TroopHelp.IsHaveAnyState(armyData.armyStatus,(long)(ArmyStatus.GARRISONING |ArmyStatus.COLLECTING | ArmyStatus.RALLY_WAIT))*/)
            {
                if (info.targetArg.HasPos)
                {
                    if (TroopHelp.GetTroopState(armyData.armyStatus) != Troops.ENMU_SQUARE_STAT.MOVE)
                    {
                        armyData.Pos = new Vector2(info.targetArg.pos.x / 100, info.targetArg.pos.y / 100);
                    }
                }

                if (info.targetArg.HasTargetObjectIndex)
                {
                    armyData.targetArgObjectId = info.targetArg.targetObjectIndex;
                }             
            }
            if (info.HasPath)
            {
                UpdateData(armyData, info.path);
            }
            if (info.HasSoldiers)
            {
                Dictionary<Int64, BattleRemainSoldiers> remainSoldiersDic = new Dictionary<long, BattleRemainSoldiers>();
                BattleRemainSoldiers remainSoldiers = new BattleRemainSoldiers();
                remainSoldiers.rid = m_playerProxy.Rid;
                remainSoldiers.remainSoldier = info.soldiers;
                remainSoldiersDic.Add(m_playerProxy.Rid, remainSoldiers);
                ArmyData changeArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(armyData.objectId);
                if (changeArmyData != null)
                {
                    if (!changeArmyData.isRally)
                    {
                       TroopHelp.UpdateTroopSoldiers(armyData.objectId, remainSoldiersDic);  
                    }
                }
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopUpdate,armyData);
        }
        #endregion

        #region 自己的运输车
        public void UpdateSummonerTransport(INotification data)
        {
            Transport_TransportList.request req = data.Body as Transport_TransportList.request;
            if (req == null)
            {
                return;
            }
            foreach (var info in req.transportInfo.Values)
            {
                if (info.HasTargetObjectIndex && info.targetObjectIndex == -1)
                {
                    RemoveArmyData(RssType.Transport, (int)info.transportIndex);
                }
                else
                {
                    ArmyData armyData = GetArmyData(RssType.Transport, (int)info.transportIndex);
                    if(armyData == null)
                    {
                        armyData = CreateTransport(info);
                    }
                    if (armyData == null) continue;
                    UpdateTransport(armyData, info);                    
                }               
            }
        }

        private ArmyData CreateTransport(TransportInfo info)
        {
            int id = (int) info.transportIndex;
            ArmyData armyData = new SelfTransportData(id);
            if(info.objectIndex>0)
                armyData.objectId = (int) info.objectIndex;
            m_dicSummonerTroop[RssType.Transport].Add(id, armyData);
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopAdd,armyData);
            return armyData;
        }
        
        private void UpdateTransport(ArmyData armyData, TransportInfo info)
        {
//            if (info.HasTransportStatust)//该字段客户端不宜使用
//            {
                armyData.armyStatus = (long) ArmyStatus.SPACE_MARCH;
//            }
            if (info.HasArrivalTime)
            {
                armyData.arrivalTime = info.arrivalTime;
            }
            if (info.HasStartTime)
            {
                armyData.startTime = info.startTime;
            }
            if (info.HasObjectIndex)
            {
                armyData.objectId = (int)info.objectIndex;
            }
            if(info.HasPath)
            {
                UpdateData(armyData, info.path);
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopUpdate,armyData);
        }

        #endregion

        #region 自己的斥候
        public void UpdateSummonerScout(INotification notification)
        {
            Role_ScoutsInfo.request scoutsInfo = notification.Body as Role_ScoutsInfo.request;
            if (scoutsInfo == null || !scoutsInfo.HasScoutsQueue)
            {
                return;
            }
            foreach (var info in scoutsInfo.scoutsQueue.Values)
            {
                if (!info.HasScoutsIndex)
                {
                    continue;
                }
                ScoutProxy.ScoutInfo scoutInfo = m_ScoutProxy.GetSoutInfoById((int)info.scoutsIndex);
                if (scoutInfo == null) continue;
                if (scoutInfo.state == ScoutProxy.ScoutState.Fog ||
                    scoutInfo.state == ScoutProxy.ScoutState.Return ||
                    scoutInfo.state == ScoutProxy.ScoutState.Back_City || 
                    scoutInfo.state == ScoutProxy.ScoutState.Scouting ||
                    scoutInfo.state == ScoutProxy.ScoutState.Surveing)
                    
                {
                    ArmyData armyData = GetArmyData(RssType.Scouts, (int)scoutInfo.id);
                    if(armyData == null)  
                    {
                        armyData = CreateScout(scoutInfo);
                    }
                    if (armyData == null) continue;

                    if (!info.HasScoutsPath || info.scoutsPath.Count == 0)
                    {
                        WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveSummonerTroopLine(armyData.objectId);
                    }

                    UpdateScout(armyData, info);
                    
                }
                else if (scoutInfo.state == ScoutProxy.ScoutState.None || scoutInfo.state == ScoutProxy.ScoutState.Scouting_Delete)
                {
                    RemoveArmyData(RssType.Scouts, scoutInfo.id);
                }
            }
        }

        private ArmyData CreateScout(ScoutProxy.ScoutInfo info)
        {
            ArmyData armyData = new SelfScoutData(info.id);
            m_dicSummonerTroop[RssType.Scouts].Add(info.id, armyData);
            if(info.ObjectId>0)
                armyData.objectId = (int) info.ObjectId;
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopAdd,armyData);
            return armyData;
        }

        private void UpdateScout(ArmyData armyData, ScoutsInfo info)
        {
            if (info.HasScoutsStatus)
            {
                armyData.armyStatus = info.scoutsStatus;
            }
            if (info.HasArrivalTime)
            {
                armyData.arrivalTime = info.arrivalTime;
            }
            if (info.HasStartTime)
            {
                armyData.startTime = info.startTime;
            }
            if (info.HasObjectIndex)
            {
                armyData.objectId = (int)info.objectIndex;
            }
            if (info.HasScoutsPath)
            {
                UpdateData(armyData, info.scoutsPath);
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopUpdate,armyData);
        }

        #endregion


        public ArmyData AddSummonerArmyByMapObjectInfo(SprotoType.MapObjectInfo info)
        {
            RssType objectType = (RssType)info.objectType;
            int id = 0;
            switch(objectType)
            {
                case RssType.Troop:
                    id = (int)info.armyIndex;
                    break;
                case RssType.Scouts:
                    id = (int)info.scoutsIndex;
                    break;
                case RssType.Transport:
                    id = (int)info.transportIndex;
                    break;
            }
            ArmyData armyData = GetArmyData(objectType, id);
            if(armyData == null)
            {
                switch (objectType)
                {
                    case RssType.Troop:
                        armyData = new SelfTroopData((int)info.armyIndex);
                       
                        break;
                    case RssType.Scouts:
                        armyData = new SelfScoutData((int)info.scoutsIndex);
                        break;
                    case RssType.Transport:
                        armyData = new SelfTransportData((int)info.transportIndex);
                        break;
                }
                m_dicSummonerTroop[objectType].Add(id, armyData);
                Debug.Log("++++AddSummonerArmyByMapObjectInfo armyData dataIndex" +armyData.dataIndex + " objectId" + armyData.objectId);
            }
            return armyData;
        }

        private void UpdateData(ArmyData armyData, List<PosInfo> path)
        {
            if (path != null && path.Count > 0)
            {
                armyData.ClearMovePath();
                foreach (var pos in path)
                {
                    armyData.SetMovePath(PosHelper.ServerUnitToClientUnit_Vec2( new Vector2(pos.x, pos.y)));
                }
                armyData.autoMoveIndex = armyData.GetMoveIndex();
                // 客户端兼容处理， 如果是idle状态，就不显示行军线
                if (armyData.objectId != 0 && armyData.state != Troops.ENMU_SQUARE_STAT.IDLE)
                {
                    if (!TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.MOVE))
                    {
                        Vector2 offset = TroopHelp.GetLineDestOffset(armyData);
                        List<Vector2> movePath = TroopHelp.GetMovePathWithOffset(armyData.movePath, offset);
                        WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().AddSummonerTroopLine(armyData.objectId, movePath);
                        AutoMoveMgr.Instance.Insert(armyData.objectId);
                        WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveAoiTroopLines(armyData.objectId);
                    }
                }
                WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(armyData.objectId, (int) armyData.armyStatus);
            }
        }

        public ArmyData GetArmyData(RssType objectType, int id)
        {
            Dictionary<int, ArmyData> dicArmy;
            if(!m_dicSummonerTroop.TryGetValue(objectType, out dicArmy))
            {
                return null;
            }
            ArmyData armyData = null;
            if(dicArmy.TryGetValue(id, out armyData))
            {
                return armyData;
            }
            return null;
        }

        public ArmyData GetArmyDataByObjectId(RssType objectType, int objectId)
        {
            if (objectId == 0)
            {
                return null;
            }
            Dictionary<int, ArmyData> dicArmy;
            if (!m_dicSummonerTroop.TryGetValue(objectType, out dicArmy))
            {
                return null;
            }
            foreach(var kv in dicArmy)
            {
                if(kv.Value.objectId == objectId)
                {
                    return kv.Value;
                }
            }
            return null;
        }

        public ArmyData GetArmyDataByObjectId(int objectId)
        {
            if (objectId == 0)
            {
                return null;
            }
            foreach (var kv in m_dicSummonerTroop)
            {
                foreach (var child in kv.Value)
                {
                    if(child.Value.objectId == objectId)
                    {
                        return child.Value;
                    }
                }
            }
            return null;
        }

        public List<ArmyData> GetSummonerArmyDatas()
        {
           List<ArmyData> lsArmyDatas=new List<ArmyData>();
            foreach (var kv in m_dicSummonerTroop.Values)
            {
                foreach (var child in kv)
                {
                    lsArmyDatas.Add(child.Value);
                }
            }

            return lsArmyDatas;
        }

        public int GetStationingArmyId(int objectId)
        {
            if (m_dicSummonerTroop.ContainsKey(RssType.Troop))
            {
                foreach (var info in m_dicSummonerTroop[RssType.Troop].Values)
                {
                    if (info.targetArgObjectId == objectId)
                    {
                        return info.troopId;
                    }
                }
            }   
            return 0;
        }

        public void CalScreenViceArmList(int troopId, ref List<int> viceArmyIndexList)
        {
            viceArmyIndexList.Clear();

            if (m_dicSummonerTroop.ContainsKey(RssType.Troop))
            {
                foreach (var armyData in m_dicSummonerTroop[RssType.Troop].Values)
                {
                    if (armyData.troopId == troopId) continue;

                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.PALLY_MARCH)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.COLLECTING)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.GARRISONING)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.RALLY_WAIT)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.RALLY_BATTLE)) continue;

                    Vector3 troopPos = Vector3.zero;
                    Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
                    if (formation != null)
                    {
                        troopPos = formation.transform.position;
                    }
                    else
                    {
                        Vector2 pos = armyData.GetMovePos();
                        troopPos = new Vector3(pos.x, 0, pos.y);
                    }

                    if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), troopPos.x, troopPos.z))
                    {
                        viceArmyIndexList.Add(armyData.troopId);
                    }
                }
            }
        }

        public void CalWorldViceArmList(int troopId, ref List<int> viceArmyIndexList)
        {
            viceArmyIndexList.Clear();

            if (m_dicSummonerTroop.ContainsKey(RssType.Troop))
            {
                foreach (var armyData in m_dicSummonerTroop[RssType.Troop].Values)
                {
                    if (armyData.troopId == troopId) continue;

                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.PALLY_MARCH)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.COLLECTING)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.GARRISONING)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.RALLY_WAIT)) continue;
                    if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.RALLY_BATTLE)) continue;

                    viceArmyIndexList.Add(armyData.troopId);
                }
            }
        }

        private void RemoveArmyData(RssType objectType, int id)
        {
            ArmyData armyData = GetArmyData(objectType, id);
            if (armyData == null) return;
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopRemove,armyData);
            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveSummonerTroopLine(armyData.objectId);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().RemoveOwnTroop(armyData.objectId);
            m_dicSummonerTroop[objectType].Remove(id);
        }
    }
}