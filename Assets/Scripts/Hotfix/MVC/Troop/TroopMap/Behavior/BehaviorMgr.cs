using System.Collections.Generic;
using Client;
using Game;
using UnityEngine;

namespace Hotfix
{
    public enum BehaviorType
    {
        None = 0,
        //玩家自己的部队
        PlayTroop,
        //其他玩家的部队
        OtherTroop,
        //野蛮人
        Monster,
        //斥候
        Scout,
        //玩家斥候
        PlayScout,
        //建筑战斗
        BuildingFight,
        //集结部队
        PlayRallyBehavior,
        //运输车
        TransportBehavior,
        //圣地守护者
        GuardianBehavior,
    }

    public class BehaviorMgr : IBehaviorHandler
    {
        private readonly Dictionary<BehaviorType, IBehavior> dicBehaviors = new Dictionary<BehaviorType, IBehavior>();
        private TroopProxy m_TroopProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;

        public void Init()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            dicBehaviors[BehaviorType.PlayTroop] = new PlayBehavior();
            dicBehaviors[BehaviorType.Monster] = new MonsterBehavior();
            dicBehaviors[BehaviorType.Scout] = new ScoutBehavior();
            dicBehaviors[BehaviorType.PlayScout] = new PlayScoutBehavior();
            dicBehaviors[BehaviorType.BuildingFight] = new BuildingFightBehavior();
            dicBehaviors[BehaviorType.PlayRallyBehavior] = new PlayRallyBehavior();
            dicBehaviors[BehaviorType.TransportBehavior] = new TransportBehavior();
            dicBehaviors[BehaviorType.GuardianBehavior] = new GuardianBehavior();
        }

        public void Clear()
        {

        }

        public void ChageState(int id, int stateType)
        {
            IBehavior behavior = CreateBehaviorFactory(id);
            if (behavior == null)
            {
                return;
            }
            Troops.ENMU_SQUARE_STAT state = TroopHelp.GetTroopState(stateType);
            if (behavior.LastState != behavior.State)
            {
                behavior.LastState = behavior.State;
            }

            behavior.State = stateType;
            behavior.Init(id);
            switch (state)
            {
                case Troops.ENMU_SQUARE_STAT.IDLE:
                    // 待机
                    {
                        behavior.OnIDLE();
                    }
                    break;
                case Troops.ENMU_SQUARE_STAT.MOVE:
                    // 追击
                    if (TroopHelp.IsHaveState(stateType, ArmyStatus.FOLLOWUP))
                    {
                        behavior.OnFIGHTANDFOLLOWUP();
                    }
                    // 围击
                    else if (TroopHelp.IsHaveState(stateType, ArmyStatus.MOVE))
                    {
                        behavior.OnFIGHTMOVE();
                    }
                    // 移动
                    else
                    {
                        behavior.OnMOVE();
                    }
                    break;
                case Troops.ENMU_SQUARE_STAT.FIGHT:
                    {
                        // 攻击 内部需要判断是否有目标        
                        MapObjectInfoEntity mapobjectinfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
                        if (mapobjectinfo != null)
                        {
                            if (mapobjectinfo.targetObjectIndex != 0 || TroopHelp.IsAttackBuilding(mapobjectinfo.rssType))
                            {
                                behavior.OnFIGHT();
                            }
                            else
                            {
                                behavior.OnIDLE();
                            }
                        }
                    }
                    break;
            }
            // 这个逻辑问题太大 需要重写，先注释掉
            //WorldMapLogicMgr.Instance.BgSoundHandler.WantChangeBGMOnBehaviorStateChange(id);
        }

        private IBehavior CreateBehaviorFactory(int id)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                if (armyData.troopType == RssType.Scouts)
                {
                    if (armyData.isPlayerHave)
                    {
                        return GetIBehavior(BehaviorType.PlayScout);
                    }

                    return GetIBehavior(BehaviorType.Scout);
                }
                if (armyData.troopType == RssType.Transport)
                {
                    return GetIBehavior(BehaviorType.TransportBehavior);
                }
                if (armyData.isRally)
                {
                    return GetIBehavior(BehaviorType.PlayRallyBehavior);
                }
                return GetIBehavior(BehaviorType.PlayTroop);
            }

            MapObjectInfoEntity mapobjectinfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapobjectinfo != null)
            {
                if (!WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().IsContainTroop((int)mapobjectinfo.objectId))
                {
                    if (mapobjectinfo.objectType == (int)RssType.Monster ||
                        mapobjectinfo.objectType == (int)RssType.SummonAttackMonster ||
                        mapobjectinfo.objectType == (int)RssType.SummonConcentrateMonster)
                    {
                        return GetIBehavior(BehaviorType.Monster);
                    }
                    else if (mapobjectinfo.objectType == (int)RssType.Guardian)
                    {
                        return GetIBehavior(BehaviorType.GuardianBehavior);
                    }
                    else
                    {
                        return GetIBehavior(BehaviorType.BuildingFight);
                    }
                }
            }

            return null;
        }

        private IBehavior GetIBehavior(BehaviorType type)
        {
            IBehavior behavior;
            if (dicBehaviors.TryGetValue(type, out behavior))
            {
                return behavior;
            }

            return null;
        }
    }
}