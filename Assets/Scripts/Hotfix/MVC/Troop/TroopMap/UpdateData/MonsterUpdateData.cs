using System;
using System.Collections.Generic;
using Data;
using Game;
using ILRuntime.Runtime;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Hotfix
{
    public sealed class MonsterUpdateData : IMonsterUpdateData
    {
        private TroopProxy m_TroopProxy;
        private WorldMapObjectProxy m_worldProxy;
        private readonly List<long> lsbuffId = new List<long>();

        public MonsterUpdateData()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_worldProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
        }

        public void UpdateAttackTargetId(int id, int targetId)
        {
            if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .IsContainTroop(id))
            {
                return;
            }

            MapObjectInfoEntity monsterData = m_worldProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null)
            {
                monsterData.atkId = targetId;          
                if (TroopHelp.IsHaveState(monsterData.status,ArmyStatus.BATTLEING))
                {
                    if (monsterData.atkId != 0)
                    {
                        if(TroopHelp.IsHaveState(monsterData.status, ArmyStatus.PATROL))
                        {
                            Debug.LogWarning("服务器忘记去掉巡逻，客户端兼容处理" + targetId);
                            monsterData.status = monsterData.status&(~(int)ArmyStatus.PATROL);
                        }
                        WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(id, (int)monsterData.status);
                    }

                    Debug.Log("更新野蛮人攻击对象id" + monsterData.atkId);
                }
                else
                {
                    monsterData.atkId = 0;
                    Debug.LogWarning("服务器发错数值：客户端兼容处理:" + targetId);
                }
            }
        }

        public void UpdateMovePath(INotification data)
        {
            Map_ObjectInfo.request mapItemInfo = data.Body as Map_ObjectInfo.request;
            if (mapItemInfo != null)
            {
                int id = (int)mapItemInfo.mapObjectInfo.objectId;
                if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().IsContainTroop(id))
                {
                    return;
                }

                MapObjectInfoEntity monsterData =
                    m_worldProxy.GetWorldMapObjectByobjectId(mapItemInfo.mapObjectInfo.objectId);
                if (monsterData != null && mapItemInfo.mapObjectInfo.HasObjectPath)
                {
                    monsterData.ClearMovePath();
                    foreach (var pos in mapItemInfo.mapObjectInfo.objectPath)
                    {
                        Vector2 v2 = new Vector2(pos.x / 100f, pos.y / 100f);
                        monsterData.path.Add(v2);
                    }

                    //------------野蛮人多点路径测试代码----------- 
                    //PosInfo lastPosInfo = mapItemInfo.mapObjectInfo.objectPath[1];
                    //Vector2 lastPos = new Vector2(lastPosInfo.x / 100f, lastPosInfo.y / 100f);

                    //Vector2 pos2 = new Vector2(lastPos.x + 10, lastPos.y);
                    //monsterData.path.Add(pos2);

                    //Vector2 pos3 = new Vector2(pos2.x, pos2.y + 10);
                    //monsterData.path.Add(pos3);
                    //-------------------------------------------- 

                    monsterData.startTime = mapItemInfo.mapObjectInfo.startTime;
                    monsterData.arrivalTime = mapItemInfo.mapObjectInfo.arrivalTime;

                    monsterData.autoMoveIndex = monsterData.GetMoveIndex();
                    AutoMoveMgr.Instance.Insert(id);
                }
            }
        }

        public void UpdateHp(int id, int hp)
        {
            MapObjectInfoEntity monsterData = m_worldProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null)
            {
                if (monsterData.rssType == RssType.Monster ||
                    monsterData.rssType == RssType.Guardian ||
                    monsterData.rssType == RssType.SummonAttackMonster ||
                    monsterData.rssType == RssType.SummonConcentrateMonster)
                {                  
                    monsterData.HP = hp;
                }
            }
        }

        public void UpdateBuff(int id, object buff)
        {
            if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .IsContainTroop(id))
            {
                return;
            }

            MapObjectInfoEntity monsterData = m_worldProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null &&
                (monsterData.rssType == RssType.Monster ||
                monsterData.rssType == RssType.SummonAttackMonster ||
                monsterData.rssType == RssType.SummonConcentrateMonster))
            {
                lsbuffId.Clear();
                foreach (var info in monsterData.battleBuff)
                {
                    int skillId = (int) info.buffId;
                    SkillStatusDefine statusDefine = CoreUtils.dataService.QueryRecord<SkillStatusDefine>(skillId);
                    if (statusDefine != null)
                    {
                        SkillBattleDefine skillBattleDefine = CoreUtils.dataService.QueryRecord<SkillBattleDefine>(statusDefine.type * 100 + statusDefine.level);
                        if (skillBattleDefine != null)
                        {
                            if (skillBattleDefine.autoActive == 0)
                            {
                                lsbuffId.Insert(0, info.buffId);
                            }
                            else
                            {
                                lsbuffId.Add(info.buffId);
                            }                            
                        }

                        if (info.isNew)
                        {
                            WorldMapLogicMgr.Instance.BattleUIHandler.PushBattleBuff(id, skillId);
                        }
                    }
                }

                if (lsbuffId.Count > 0)
                {
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(id, BattleUIType.BattleUI_BuffSkill, (int)lsbuffId[0]);
                }
                else
                {
                    WorldMapLogicMgr.Instance.BattleBuffHandler.ClearBuff(id);
                }
            }
        }

        public void ClearMovePath(int id)
        {
            MapObjectInfoEntity monsterData =
                m_worldProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null && monsterData.objectPath != null&& monsterData.objectPath.Count<=0)
            {
                monsterData.path.Clear();
            }
        }

        public void ClearData()
        {
            lsbuffId.Clear();
        }
    }
}