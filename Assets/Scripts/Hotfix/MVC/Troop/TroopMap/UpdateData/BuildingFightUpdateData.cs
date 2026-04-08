using System.Collections.Generic;
using Client;
using Data;
using Game;
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public sealed class BuildingFightUpdateData : IBuildingFightUpdateData
    {
        private WorldMapObjectProxy m_WorldMapObjectProxy;
        private readonly List<long> lsbuffId = new List<long>();

        public BuildingFightUpdateData()
        {
            m_WorldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
        }

        public void UpdateAttackTargetId(int id, int targetId)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity != null)
            {
                if (mapObjectInfoEntity.objectType != (int)RssType.Troop &&
                    mapObjectInfoEntity.objectType != (int)RssType.Monster &&
                    mapObjectInfoEntity.objectType != (int)RssType.Guardian &&
                    mapObjectInfoEntity.objectType != (int)RssType.SummonAttackMonster &&
                    mapObjectInfoEntity.objectType != (int)RssType.SummonConcentrateMonster)
                
                {
                    mapObjectInfoEntity.attackTargetId = targetId;
                    if (Application.isEditor)
                    {
                        Color color;
                        ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                        CoreUtils.logService.Debug($"{id}\ttBattleBuildingData: Building targetIndex:{targetId}", color);
                    }
                }
            }
        }

        public void UpdateMovePath(INotification data)
        {
        }

        public void UpdateHpMax(int id, int hpMax)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity != null)
            {
                mapObjectInfoEntity.HPMax = hpMax;
            }
        }

        public void UpdateHead(int id)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity != null)
            {
                //此处最好加上建筑类型判断

                AppFacade.GetInstance().SendNotification(CmdConstant.MapUpdateBuildingHead, id);
            }
        }

        public void UpdateHp(int id, int hp)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity != null)
            {
                if (mapObjectInfoEntity.objectType != (int)RssType.Troop &&
                    mapObjectInfoEntity.objectType != (int)RssType.Monster &&
                    mapObjectInfoEntity.objectType != (int)RssType.Guardian &&
                    mapObjectInfoEntity.objectType != (int)RssType.SummonAttackMonster &&
                    mapObjectInfoEntity.objectType != (int)RssType.SummonConcentrateMonster)
                {
                    mapObjectInfoEntity.HP = hp;
                }
            }
        }

        public void UpdateBuff(int id, object buff)
        {
            MapObjectInfoEntity mapobjectinfo = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapobjectinfo != null &&
                mapobjectinfo.rssType != RssType.Troop &&
                mapobjectinfo.rssType != RssType.Monster &&
                mapobjectinfo.rssType != RssType.Guardian &&
                mapobjectinfo.rssType != RssType.SummonAttackMonster &&
                mapobjectinfo.rssType != RssType.SummonConcentrateMonster)
            {
                lsbuffId.Clear();
                foreach (var info in mapobjectinfo.battleBuff)
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

                        //UI上的buff提示不应该受实体buff光效逻辑影响
                        if (info.isNew)
                        {
                            WorldMapLogicMgr.Instance.BattleUIHandler.PushBattleBuff(id, skillId);
                        }
                    }
                }

                if (lsbuffId.Count > 0)
                {
                    if (mapobjectinfo.gameobject != null)
                    {
                        WorldMapLogicMgr.Instance.BattleBuffHandler.CreateBuffGo(id, (int)lsbuffId[0], mapobjectinfo.gameobject.transform);
                    }
                }
                else
                {
                    WorldMapLogicMgr.Instance.BattleBuffHandler.ClearBuff(id);
                }
            }
        }

        public void ClearMovePath(int id)
        {
            
        }
    }
}