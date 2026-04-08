using System.Collections.Generic;
using Client;
using Data;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public sealed class MapBuildingFightHandler :  IMapBuildingFightHandler
    {
        private WorldMapObjectProxy m_WorldMapObjectProxy;
        private readonly Dictionary<int, GameObject> dicSkillGo = new Dictionary<int, GameObject>();
        private readonly Dictionary<int,GameObject> dicBurning= new Dictionary<int, GameObject>();
        private readonly Dictionary<int,Timer> m_Timer =new Dictionary<int, Timer>();

        public void Init()
        {
            m_WorldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
        }

        public void PlaySkills(int id, int targetId, int skillId)
        {
            // 有可能一个技能打很多人，或者一个技能被给很多人加buf的时候， 会重复触发技能
            HotfixUtil.InvokOncePerfOneFrame($"PlaySkills_{id}_{targetId}_{skillId}", () =>
            {
                MapObjectInfoEntity mapObjectInfoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
                if (mapObjectInfoEntity != null)
                {
                    if (mapObjectInfoEntity.objectType != (int)RssType.Troop &&
                        mapObjectInfoEntity.objectType != (int)RssType.Monster &&
                        mapObjectInfoEntity.objectType != (int)RssType.Guardian &&
                        mapObjectInfoEntity.objectType != (int)RssType.SummonAttackMonster &&
                        mapObjectInfoEntity.objectType != (int)RssType.SummonConcentrateMonster &&
                        mapObjectInfoEntity.objectType != (int)RssType.Expedition)
                    {
                        if (mapObjectInfoEntity.gameobject != null)
                        {
                            Vector3 pos = mapObjectInfoEntity.gameobject.transform.position;

                            if (id == targetId)
                            {
                                PlaySkill(skillId, pos, mapObjectInfoEntity.gameobject);
                            }
                            else
                            {
                                Troops troopFormation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(targetId);
                                if (troopFormation != null)
                                {
                                    Vector3 skillEffectPos = Vector3.zero;
                                    EffectInfoDefine effectInfoDefine = CoreUtils.dataService.QueryRecord<EffectInfoDefine>(skillId);
                                    if (effectInfoDefine != null)
                                    {
                                        if (effectInfoDefine.beAttacked == 1)
                                        {
                                            TroopHelp.GetRssPos(targetId, out skillEffectPos);
                                        }
                                    }

                                    pos = skillEffectPos.Equals(Vector3.zero) ? pos : skillEffectPos;

                                    PlaySkill(skillId, pos, troopFormation.gameObject);
                                }
                            }

                        }
                    }
                }
            });
        }


        private void PlaySkill(int id, Vector3 pos, GameObject target)
        {
            OnStopSkill(id);
            if (dicSkillGo.ContainsKey(id))
            {
                return;
            }

            EffectInfoDefine effectInfoDefine = CoreUtils.dataService.QueryRecord<EffectInfoDefine>(id);
            if (effectInfoDefine == null)
            {
                return;
            }

            string m_skillAniChargeEffectName = effectInfoDefine.effectID;
            CoreUtils.assetService.Instantiate(m_skillAniChargeEffectName, (gameObject) =>
            {
                if (gameObject == null)
                    return;

                if (target == null)
                {
                    CoreUtils.assetService.Destroy(gameObject);
                    return;
                }

                Debug.LogWarning($"PlayEffect {m_skillAniChargeEffectName}");
                gameObject.transform.position = pos;
                // 原来特效朝向计算有问题不能使用负的来算
                var dir = target.transform.position - pos;
                dir.y = 0;
                if (dir.normalized.Equals(Vector2.zero))
                {
                    dir = gameObject.transform.forward;
                }
                gameObject.transform.forward = dir.normalized;
                gameObject.transform.localScale = Vector3.one;
                if (!dicSkillGo.ContainsKey(id))
                {
                    dicSkillGo.Add(id, gameObject);
                }
            });
        }

        public void StopSkill(int id)
        {
            OnStopSkill(id);
        }

        private void OnStopSkill(int id)
        {
            if (dicSkillGo.ContainsKey(id))
            {
                //目前技能实体的删除是自身挂脚本实现
                //技能实体的删除没有附带删除数据缓存
                //此处如果判断实体存在才删除缓存，那就会产生脏数据
                //dicSkillGo该数据缓存字典似乎没有存在的意义了
                dicSkillGo.Remove(id);
            }
        }

        public void PlayBuffEffect(int id, int buffId)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity != null)
            {
                WorldMapLogicMgr.Instance.BattleBuffHandler.CreateBuffGo(id,
                    (int) mapObjectInfoEntity.battleBuff[0].buffId,
                    mapObjectInfoEntity.gameobject.transform);
            }
        }


        public void PlayBuildingHud(int id)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateBuildingFightHud, id);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateShottTextHud, id);
        }


        public void StopBuildingHud(int id)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveBuildingFightHud, id);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapStopShottTextHud, id);
        }

        public void UpdateWorldHud(long id, bool isFight)
        {
            var obj = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            obj.isFight = isFight;
            AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectHUDUpdate, obj);
        }

        public void PlayBurning(int id)
        {
            MapObjectInfoEntity infoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (infoEntity != null)
            {
                if (infoEntity.guildBuildStatus == 3)
                {
                    var allianceBuildingType =
                        CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int) infoEntity.objectType);
                    CoreUtils.assetService.Instantiate(RS.FireName[4], (go) =>
                    {
                        if (infoEntity.guildBuildStatus != 3)
                        {
                            CoreUtils.assetService.Destroy(go); 
                            return;
                        }
                        go.name = "BuildingFire_"+infoEntity.objectId;
                        go.transform.SetParent(infoEntity.gameobject.transform);
                        go.transform.localPosition = Vector3.zero;
                        ClearTimer(id);
                        if (allianceBuildingType != null)
                        {
                            if (!m_Timer.ContainsKey(id))
                            {
                                Timer timer = Timer.Register(allianceBuildingType.burnLast, () =>
                                {
                                    if (go != null)
                                    {
                                        CoreUtils.assetService.Destroy(go);
                                    }

                                    ClearTimer(id);
                                });
                                m_Timer[id] = timer;
                            }
                        }

                        if (go != null)
                        {                           
                            if (!dicBurning.ContainsKey(id))
                            {
                                dicBurning.Add(id,go);
                            }
                        }
                    });
                }
                else
                {
                    StopStopBurnings(id);
                }
            }
        }

        public void StopBurning(int id)
        {
            StopStopBurnings(id);
        }

        private void StopStopBurnings(int id)
        {
            GameObject g = null;
            if (dicBurning.TryGetValue(id, out  g))
            {
                if (g != null)
                {                        
                    CoreUtils.assetService.Destroy(g);
                    dicBurning.Remove(id);
                }
            }
        }


        public void Clear()
        {
            foreach (var go in dicSkillGo.Values)
            {
                if (go != null)
                {
                    CoreUtils.assetService.Destroy(go);    
                }
            }
            dicSkillGo.Clear();

            foreach (var go in dicBurning.Values)
            {
                if (go != null)
                {
                    CoreUtils.assetService.Destroy(go);    
                }
            }            
            dicBurning.Clear();

            ClearTimers();
        }

        private void ClearTimer(int id)
        {
            if (m_Timer != null)
            {
                Timer timer;
                if (m_Timer.TryGetValue(id, out timer))
                {
                    timer.Cancel();
                }

                m_Timer.Remove(id);
            }
        }

        private void ClearTimers()
        {
            foreach (var timer in m_Timer.Values)
            {
                timer.Cancel();
            }
             m_Timer.Clear();
        }
    }
}