using Game;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine;

namespace Hotfix
{
    public class UpdateMapDataHandler : IUpdateMapDataHandler
    {
        private long m_lastTouTroopId = 0;
        public void Init()
        {
        }

        public void Clear()
        {
            m_lastTouTroopId = 0;
        }

        public void UpdateLastTroopId(long troopId)
        {
            m_lastTouTroopId = troopId;
        }

        public void InitMapData(Map_ObjectInfo.request mapItemInfo)
        {
            if (mapItemInfo != null)
            {
                if (!mapItemInfo.mapObjectInfo.HasObjectId)
                {
                    return;
                } 
                
                if (mapItemInfo.mapObjectInfo.objectType <= 0)
                {
                    return;
                }
                
                int objectId = (int)mapItemInfo.mapObjectInfo.objectId;
                if (mapItemInfo.mapObjectInfo.HasArmyMarchInfos)
                {
                    if (mapItemInfo.mapObjectInfo.armyMarchInfos.Count > 0)
                    {
                        WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().SetAOITroopLines(objectId);
                    }
                }
            }
        }

        public void UpdateMapData(INotification notification)
        {
            Map_ObjectInfo.request mapItemInfo = notification.Body as Map_ObjectInfo.request;
            if (mapItemInfo != null)
            {
                if (!mapItemInfo.mapObjectInfo.HasObjectId)
                {
                    return;
                }

                int objectId = (int)mapItemInfo.mapObjectInfo.objectId;
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateArmyData(objectId, mapItemInfo.mapObjectInfo);             
                if (mapItemInfo.mapObjectInfo.HasTargetObjectIndex)
                {
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateTarget(objectId, (int)mapItemInfo.mapObjectInfo.targetObjectIndex);
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateMonsterData().UpdateAttackTargetId(
                        objectId,
                        (int) mapItemInfo.mapObjectInfo.targetObjectIndex);
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().UpdateAttackTargetId(
                        objectId,
                        (int) mapItemInfo.mapObjectInfo.targetObjectIndex);
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateBuildingFightData().UpdateAttackTargetId(
                        objectId,
                        (int) mapItemInfo.mapObjectInfo.targetObjectIndex);
                }

                if (mapItemInfo.mapObjectInfo.objectPath != null && mapItemInfo.mapObjectInfo.HasArrivalTime &&
                    mapItemInfo.mapObjectInfo.HasStartTime)
                {
                    //Debug.LogError("更新部队移动路径" +mapItemInfo.mapObjectInfo.objectId);
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().UpdateMovePath(notification);
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateMonsterData().UpdateMovePath(notification);
                }

                if (mapItemInfo.mapObjectInfo.HasObjectPath)
                {
                    if (mapItemInfo.mapObjectInfo.objectPath != null && mapItemInfo.mapObjectInfo.objectPath.Count <= 0)
                    {
                        WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().ClearMovePath(objectId);
                        WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateMonsterData().ClearMovePath(objectId);
                    }
                }

                if (mapItemInfo.mapObjectInfo.HasStatus)
                {
                    //Debug.LogError("更新状态" + mapItemInfo.mapObjectInfo.objectId + "+++" + mapItemInfo.mapObjectInfo.status + "***"
                    //+ (ArmyStatus)mapItemInfo.mapObjectInfo.status);
                    WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(objectId, (int)mapItemInfo.mapObjectInfo.status);

                    AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectStatusChange, objectId);
                }

                if (mapItemInfo.mapObjectInfo.HasObjectId && mapItemInfo.mapObjectInfo.HasArmyCount)
                {
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateMonsterData().UpdateHp(objectId,
                        (int) mapItemInfo.mapObjectInfo.armyCount);
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateBuildingFightData().UpdateHp(objectId,
                        (int) mapItemInfo.mapObjectInfo.armyCount);
                }

                if (mapItemInfo.mapObjectInfo.HasBattleBuff)
                {
                    if (mapItemInfo.mapObjectInfo.battleBuff.Count > 0)
                    {
                        WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData()
                            .UpdateBuff(objectId, mapItemInfo.mapObjectInfo.battleBuff);
                        WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateMonsterData()
                            .UpdateBuff(objectId, mapItemInfo.mapObjectInfo.battleBuff);
                        WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateBuildingFightData()
                            .UpdateBuff(objectId, mapItemInfo.mapObjectInfo.battleBuff);
                    }
                }

                if (mapItemInfo.mapObjectInfo.HasArmyCountMax)
                {
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateBuildingFightData()
                        .UpdateHpMax(objectId, (int) mapItemInfo.mapObjectInfo.armyCountMax);
                }

                if (mapItemInfo.mapObjectInfo.HasCollectRuneTime)
                {
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData()
                        .UpdateCollectRuneTime(objectId, mapItemInfo.mapObjectInfo.collectRuneTime);
                }

                if (mapItemInfo.mapObjectInfo.HasSoldiers)
                {
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData()
                        .UpdateTroopSoldiers(objectId, mapItemInfo.mapObjectInfo.soldiers);
                }

                if (mapItemInfo.mapObjectInfo.HasMainHeroId)
                {
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateBuildingFightData().UpdateHead(objectId);
                }

                if (mapItemInfo.mapObjectInfo.HasGuildId)
                {
                    WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateLineColor(objectId);
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateTroopColor(objectId);
                }

                if (mapItemInfo.mapObjectInfo.HasGuildAbbName)
                {
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().UpdateGuildAddName(objectId);
                }

                if (mapItemInfo.mapObjectInfo.HasGuildBuildStatus)
                {
                    WorldMapLogicMgr.Instance.MapBuildingFightHandler.PlayBurning(objectId);
                }
                
                if (mapItemInfo.mapObjectInfo.HasTargetObjectIndex)
                {
                    if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().IsContainTroop(objectId))
                    {
                        if (m_lastTouTroopId == objectId)
                        {
                            m_lastTouTroopId = 0;
                            WorldMapLogicMgr.Instance.MapSelectEffectHandler.Play(objectId, (int)mapItemInfo.mapObjectInfo.targetObjectIndex);
                        }
                    }
                }
                if (mapItemInfo.mapObjectInfo.HasArmyName)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapUpdateArmyName,objectId);
                }

                if (mapItemInfo.mapObjectInfo.HasAttackCount)
                {
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().UpdateAttackCount(objectId,(int)mapItemInfo.mapObjectInfo.attackCount);
                }

                if (mapItemInfo.mapObjectInfo.HasArmyMarchInfos)
                {
                    if (mapItemInfo.mapObjectInfo.armyMarchInfos.Count > 0)
                    {
                        WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().SetAOITroopLines(objectId);
                    }
                }
            }
        }
    }
}