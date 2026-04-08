using System.Collections.Generic;
using Client;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public enum MapSelectEffectType
    {
        None,
        Green,
        Blue,
        Red
    }

    public class MapSelectEffectHandler : IMapSelectEffectHandler
    {
        private readonly Dictionary<MapSelectEffectType, GameObject> dicMapEffect = new Dictionary<MapSelectEffectType, GameObject>(3);
        private readonly Dictionary<MapSelectEffectType, bool> dicMapEffectLod= new Dictionary<MapSelectEffectType, bool>(3);
        private WorldMapObjectProxy m_WorldMapObjectProxy;
        private Timer m_Timers;

        public void Init()
        {
            m_WorldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            if (!GetIsLod(MapSelectEffectType.Green))
            {
                CoreUtils.assetService.Instantiate(RS.greenGoName, (go) =>
                {
                    go.gameObject.SetActive(false);
                    if (!dicMapEffect.ContainsKey(MapSelectEffectType.Green))
                    {
                        dicMapEffect.Add(MapSelectEffectType.Green, go);
                    }
                });
            }
            AddLod(MapSelectEffectType.Green);
            if (!GetIsLod(MapSelectEffectType.Blue))
            {
                CoreUtils.assetService.Instantiate(RS.blueGoName, (go) =>
                {
                    go.gameObject.SetActive(false);
                    if (!dicMapEffect.ContainsKey(MapSelectEffectType.Blue))
                    {
                        dicMapEffect.Add(MapSelectEffectType.Blue, go);
                    }
                });
            }
            AddLod(MapSelectEffectType.Blue);
            if (!GetIsLod(MapSelectEffectType.Red))
            {
                CoreUtils.assetService.Instantiate(RS.redGoName, (go) =>
                {
                    go.gameObject.SetActive(false);
                    if (!dicMapEffect.ContainsKey(MapSelectEffectType.Red))
                    {
                        dicMapEffect.Add(MapSelectEffectType.Red, go);
                    }
                }); 
            }
            AddLod(MapSelectEffectType.Red);       
        }

        public void Clear()
        {
           // OnClear();
           // dicMapEffectLod.Clear();
        }

        public void Play(int id, int targetId)
        {
            ArmyData army = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetArmyData(id);

            if (army == null)
            {
                return;
            }

            if (!army.isPlayerHave)
            {
                return;
            }

            if (army.state != Troops.ENMU_SQUARE_STAT.MOVE)
            {                
                return;
            }
            MapSelectEffectType type = MapSelectEffectType.Green;
            if (TroopHelp.IsHaveState(army.armyStatus, ArmyStatus.COLLECT_MARCH))
            {
                type = MapSelectEffectType.Green;
            }
            else if (TroopHelp.IsHaveState(army.armyStatus, ArmyStatus.ATTACK_MARCH))
            {
                type = MapSelectEffectType.Red;
            }
            else if (TroopHelp.IsHaveState(army.armyStatus, ArmyStatus.REINFORCE_MARCH))
            {
                type = MapSelectEffectType.Blue;
            }

            MapObjectInfoEntity infoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(targetId);
            if (infoEntity != null)
            {
                
                Vector3 pos= Vector3.zero;
                ArmyData armyData = null;;
                if (infoEntity.rssType == RssType.Troop||
                    infoEntity.rssType== RssType.Expedition)
                {
                     armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                        .GetArmyData(targetId);
                    if (armyData != null)
                    {
                        pos= new Vector3(armyData.go.transform.position.x,0,armyData.go.transform.position.z);
                    }
                }
                else if (infoEntity.rssType == RssType.Monster ||
                    infoEntity.rssType == RssType.SummonAttackMonster ||
                    infoEntity.rssType == RssType.SummonConcentrateMonster)
                {
                    Troops formationMonster = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetFormationBarbarian((int)infoEntity.objectId);
                    if (formationMonster != null)
                    {
                        pos = new Vector3(formationMonster.transform.position.x, 0, formationMonster.transform.position.z);
                    }
                }
                else
                {
                    pos = PosHelper.ServerUnitToClientUnit(infoEntity.objectPos);
                }

                if (!dicMapEffect.ContainsKey(type))
                {
                    Debug.LogWarning("当前类型资源没加载"+type);
                    return;
                }

                dicMapEffect[type].SetActive(true);
                dicMapEffect[type].transform.position = pos;
                Vector3 scale = Vector3.one;
                switch (infoEntity.rssType)
                {
                    case RssType.City:
                        scale = TroopHelp.GetSelectEffectScale(TouchTargetEfeectObjectType.City);
                        break;
                    case RssType.Monster:
                    case RssType.Guardian:
                    case RssType.SummonAttackMonster:
                    case RssType.SummonConcentrateMonster:
                        if (infoEntity.monsterDefine != null)
                        {
                            scale = TroopHelp.GetSelectEffectScale(TouchTargetEfeectObjectType.Monster,
                                infoEntity.monsterDefine.radius);
                        }

                        break;
                    case RssType.Troop:
                 
                        if (armyData != null)
                        {
                            scale = TroopHelp.GetSelectEffectScale(TouchTargetEfeectObjectType.OtherPlayerTroop,
                                armyData.armyRadius);
                        }

                        break;
                    case RssType.Rune:
                        var runeCfg =
                            CoreUtils.dataService.QueryRecord<Data.MapItemTypeDefine>((int) infoEntity.runeId);
                        if (runeCfg != null)
                        {
                            scale = TroopHelp.GetSelectEffectScale(TouchTargetEfeectObjectType.OtherPlayerTroop,
                                runeCfg.radius);
                        }

                        break;
                    default:
                        scale = TroopHelp.GetSelectEffectScale(TouchTargetEfeectObjectType.Resource);
                        break;
                }

                dicMapEffect[type].transform.localScale = scale;
                if (m_Timers != null)
                {
                    m_Timers.Cancel();
                    m_Timers = null;
                }

                m_Timers = Timer.Register(0.5f, () =>
                {
                    dicMapEffect[type].gameObject.SetActive(false);
                    if (m_Timers != null)
                    {
                        m_Timers.Cancel();
                        m_Timers = null;
                    }
                });
            }
        }

        public void Remove()
        {
           OnClear();
        }

        private void OnClear()
        {
            foreach (var go in dicMapEffect.Values)
            {
                if (go != null)
                {
                    CoreUtils.assetService.Destroy(go);
                }                
            }
            dicMapEffect.Clear();

            if (m_Timers != null)
            {
                m_Timers.Cancel();
            }
        }

        private bool GetIsLod(MapSelectEffectType type)
        {
            return dicMapEffectLod.ContainsKey(type);
        }

        private void AddLod(MapSelectEffectType type)
        {
            if (!dicMapEffectLod.ContainsKey(type))
            {
                dicMapEffectLod.Add(type,true);
            }
        }
    }
}