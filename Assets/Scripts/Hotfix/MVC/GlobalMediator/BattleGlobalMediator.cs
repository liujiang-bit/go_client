// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月2日
// Update Time         :    2020年1月2日
// Class Description   :    TroopGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Client;
using Hotfix;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Skyunion;
using SprotoType;
using Data;

namespace Game
{
    public class BattleGlobalMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "BattleGlobalMediator";
        private TroopProxy m_TroopProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private SquareHelper m_SquareHelper;
        private GlobalViewLevelMediator m_GlobalViewLevelMediator;

        #endregion

        //IMediatorPlug needs
        public BattleGlobalMediator() : base(NameMediator, null)
        {
        }


        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Battle_BattleDamageInfo.TagName,
                CmdConstant.FightUpdateMonsteAttackDir,
                Map_CityPundler.TagName,
                CmdConstant.CreateCityDone,
                CmdConstant.FightUpdateTroopAttackDir,
                Guild_GuildNotify.TagName,
                CmdConstant.FightUpdateHeroLevel
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Battle_BattleDamageInfo.TagName:
                    SetBattleDamageInfo(notification);
                    break;
                case CmdConstant.FightUpdateMonsteAttackDir:
                    int monsterId = (int) notification.Body;
                    UpdateMonsterDir(monsterId);
                    break;
                case  Map_CityPundler.TagName:
                    ShowFightVictory(notification);
                    break;
                case CmdConstant.CreateCityDone:
                    long rid = (long)notification.Body;
                    SetBuildIngState(rid);
                    break;
                case CmdConstant.FightUpdateTroopAttackDir:
                    int troopId = (int) notification.Body;
                    UpdateTroopDir(troopId);                    
                    break;
                case  Guild_GuildNotify.TagName:
                    Guild_GuildNotify.request info= notification.Body as Guild_GuildNotify.request;
                    if (info != null)
                    {
                        WorldMapLogicMgr.Instance.BattleBroadcastsHandler.Show(0, info);
                    }
                    break;
                case  CmdConstant.FightUpdateHeroLevel:
                    long heroId = (long) notification.Body;
                    UpdateHeroLevel(heroId);                  
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_GlobalViewLevelMediator =
                AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                    GlobalViewLevelMediator;
            m_SquareHelper = SquareHelper.Instance;
            
            CoreUtils.audioService.SetEnvSoundMaxPlayCount(200);
        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
        }

        public override void Update()
        {
        }

        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }

        private void SetBattleDamageInfo(INotification notification)
        {
            Battle_BattleDamageInfo.request mapItemInfo = notification.Body as Battle_BattleDamageInfo.request;
            if (mapItemInfo != null)
            {
                OnBattlePlay(mapItemInfo);
            }
        }

        private void OnBattlePlay(Battle_BattleDamageInfo.request mapInfo)
        {
            MapViewLevel crrLevel = m_GlobalViewLevelMediator.GetViewLevel();
            if (crrLevel > MapViewLevel.Tactical)
            {
                return;
            }

            Battle_BattleDamageInfo.request mapItemInfo = mapInfo;
            if (mapItemInfo == null)
            {
                return;
            }

            if (!mapItemInfo.HasBattleDamageInfo)
            {
                return;
            }

            foreach (var infoData in mapItemInfo.battleDamageInfo.Values)
            {
                //与服务端对数据时使用
                //if (infoData.HasSkillInfo)
                //{
                //    if (infoData.skillInfo.Count > 0)
                //    {
                //        infoData.skillInfo.ForEach((info) =>
                //        {
                //            Debug.Log("debug battleDamageInfo : attackId：" + info.objectIndex + " targetId:" + infoData.objectIndex + " skillId:" + info.skillId + " skillLevel:" + info.skillLevel);
                //        });
                //    }
                //}

                int objectIndex = (int) infoData.objectIndex;
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectIndex);
                MapObjectInfoEntity infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(objectIndex);
                string damagestr = string.Format("{0}{1}", "-", infoData.damage);

                #region 除了部队
                    if (infoEntity != null && !WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                        .IsContainTroop(objectIndex))
                {
                    infoEntity.shootTextDes = damagestr;
                    if (infoData.HasDamage)
                    {
                        WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int) infoEntity.objectId,
                            BattleUIType.BattleUI_GeneralAttack, infoData.damage);
                    }

                    if (infoEntity.rssType == RssType.Monster ||
                        infoEntity.rssType == RssType.SummonAttackMonster ||
                        infoEntity.rssType == RssType.SummonConcentrateMonster)
                    {
                        if (infoData.HasBattleRemainSoldiers)
                        {
                            foreach (var info in infoData.battleRemainSoldiers.Values)
                            {
                                UpdateMonsterSoldiers((int) infoEntity.objectId,
                                    info.remainSoldier);
                            }

                            WorldMapLogicMgr.Instance.BattleRemainSoldiersHandler.AddBattleRemainSoldiersInfo(
                                (int) infoEntity.objectId, infoData.battleRemainSoldiers);
                        }
                    }
                    else if (infoEntity.rssType == RssType.Guardian)
                    {
                        //TODO:守护者需要单独处理
                    }


                    if (infoData.HasSkillInfo)
                    {
                        if (infoData.skillInfo.Count > 0)
                        {
                            UpdateSkillDamageHeal((int) infoEntity.objectId, infoData.skillInfo);
                        }
                    }

                    if (infoData.HasDotDamage)
                    {
                        if (infoData.dotDamage > 0)
                        {
                            WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int) infoEntity.objectId, BattleUIType.BattleUI_DOTHP, (int) infoData.dotDamage);
                        }
                    }

                    if (infoData.HasArmyRadius)
                    {
                        infoEntity.armyRadius = infoData.armyRadius;
                    }
                    
                    if (infoData.HasHotHeal)
                    {
                        if (infoData.hotHeal > 0)
                        {
                            //Debug.LogError("下发了hot伤害+1");
                            WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)infoEntity.objectId, BattleUIType.BattleUI_HOT, (int) infoData.hotHeal);
                        }
                    }                   
                }

                #endregion

                #region 部队

                if (armyData != null)
                {
                    if (infoData.HasDamage)
                    {
                        WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(armyData.objectId,
                            BattleUIType.BattleUI_GeneralAttack,
                            infoData.damage);
                    }

                    if (infoData.HasBattleRemainSoldiers)
                    {
                        TroopHelp.UpdateTroopSoldiers(armyData.objectId, infoData.battleRemainSoldiers);
                        WorldMapLogicMgr.Instance.BattleRemainSoldiersHandler.AddBattleRemainSoldiersInfo(
                            armyData.objectId, infoData.battleRemainSoldiers);
                    }

                    if (infoData.HasSkillInfo)
                    {
                        if (infoData.skillInfo.Count > 0)
                        {
                            UpdateSkillDamageHeal(armyData.objectId, infoData.skillInfo);
                        }
                    }

                    if (infoData.HasDotDamage)
                    {
                        if (infoData.dotDamage > 0)
                        {
                            WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(armyData.objectId, BattleUIType.BattleUI_DOTHP, (int) infoData.dotDamage);
                        }
                    }


                    if (infoData.HasArmyRadius)
                    {
                        if (infoEntity != null)
                        {
                            infoEntity.armyRadius = infoData.armyRadius;
                        }
                    }

                    if (infoData.HasHotHeal)
                    {
                        if (infoData.hotHeal > 0)
                        {
                            //Debug.LogError("下发了hot伤害+0"+(int) infoData.hotHeal);
                            WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(armyData.objectId, BattleUIType.BattleUI_HOT, (int) infoData.hotHeal);
                        }
                    }
                }               
                #endregion    
           }
        }

        private void UpdateMonsterSoldiers(int id, Dictionary<Int64, SoldierInfo> soldiers)
        {
            MapObjectInfoEntity monsterData = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null)
            {
                bool isServer = soldiers!=null&&soldiers.Count > 0;
                if (!isServer)
                {
                    foreach (var info in monsterData.soldiers.Values)
                    {
                        info.num = 0;
                    }
                }

                if (monsterData.monsterTroopsDefine != null)
                {
                    string des =
                        m_SquareHelper.GetMapCreateTroopDes(monsterData.monsterTroopsDefine.heroID1, 0,
                            isServer ? soldiers : monsterData.soldiers, Troops.ENMU_MATRIX_TYPE.BARBARIAN);
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().SetFormationInfo(id, des);
                }
            }
        }


        private void UpdateSkillDamageHeal(int id, List<SkillDamageHeal> skillDamageHeals)
        {
            skillDamageHeals.ForEach((info) =>
            {
                if (info.HasObjectIndex)
                {
                    int attackId = (int) info.objectIndex;
                    if (info.HasSkillId)
                    {
                        long skillId = info.skillId * 100 + info.skillLevel;
                        SkillBattleDefine skillBattleDefine = CoreUtils.dataService.QueryRecord<SkillBattleDefine>((int)skillId);
                        bool isMainSkill = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().isHeroPlaySkill(attackId, (int) info.skillId);
                        bool isViceSkill =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().isVicePlaySkill(attackId, (int) info.skillId);
                        if (isMainSkill)
                        {
                            WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(attackId, BattleUIType.BattleUI_MainPlaySkill, (int) skillId);
                        }
                        else if (isViceSkill)
                        {
                            WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(attackId, BattleUIType.BattleUI_ViceSkill, (int)skillId);
                            if (skillBattleDefine != null && skillBattleDefine.autoActive == 0) 
                            {
                                WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(attackId, BattleUIType.BattleUI_ShowViceHead, (int)skillId);
                            }
                        }
                        
                        if (skillBattleDefine != null)
                        {
                            //主动技能才表现 技能光效&选中动画
                            if (skillBattleDefine.autoActive == 0)
                            {
                                WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(attackId, BattleUIType.BattleUI_Skills, (int)info.skillId, id);
                                WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(id, BattleUIType.BattleUI_ShowBeAttack, (int)info.skillId);
                            }
                        }
                                
                        WorldMapLogicMgr.Instance.MapBuildingFightHandler.PlaySkills(attackId, id, (int) info.skillId);                        
                    }

                    //治疗值为0不显示
                    if (info.HasSkillHeal && Math.Abs(info.skillHeal) != 0)
                    {
                        WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(id, BattleUIType.BattleUI_AddBlood, (int)info.skillHeal);
                    }

                    //技能伤害为0不显示
                    if (info.HasSkillDamage && Math.Abs(info.skillDamage) != 0)
                    {
                        WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(id, BattleUIType.BattleUI_HP, (int)info.skillDamage, (int)info.skillId);
                    }
                }
            });
        }

        private void UpdateMonsterDir(int id)
        {
            MapObjectInfoEntity monsterData = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null)
            {
                
                if (TroopHelp.IsHaveState(monsterData.status,ArmyStatus.BATTLEING) ||
                    TroopHelp.IsHaveState(monsterData.status,ArmyStatus.BATTLEING|ArmyStatus.MOVE))
                {
                    WorldMapLogicMgr.Instance.BehaviorHandler.ChageState((int) monsterData.objectId, (int) ArmyStatus.BATTLEING);
                }
            }
        }

        private void UpdateTroopDir(int id)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetArmyData(id);
            if (armyData != null)
            {
                if (TroopHelp.IsHaveState((long)armyData.armyStatus,ArmyStatus.BATTLEING))
                {  
                    WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(armyData.objectId, (int) ArmyStatus.BATTLEING);
                }
            }
        }

        private void ShowFightVictory(INotification notification)
        {
            Map_CityPundler.request mapCityPundler = notification.Body as Map_CityPundler.request;
            if (mapCityPundler != null)
            {              
                CoreUtils.uiManager.ShowUI(UI.s_FightVictory, null,mapCityPundler.name);
            }
        }

        private void SetBuildIngState(long rid)
        {
            MapObjectInfoEntity infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByRid(rid);
            if (infoEntity != null)
            {
                if (TroopHelp.IsHaveState(infoEntity.status, ArmyStatus.BATTLEING))
                {
                    WorldMapLogicMgr.Instance.BehaviorHandler.ChageState((int)infoEntity.objectId, (int)infoEntity.status);
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().UpdateAttackerDir((int)infoEntity.objectId);
                }
            }
        }

        private void UpdateHeroLevel(long heroid)
        {
           List<int> lsTroops=  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDatas();
            foreach (var id in lsTroops)
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                if (armyData != null)
                {
                    if (armyData.heroId == heroid)
                    {
                        Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(id);                      
                        if (formation != null)
                        {
                           // WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().UpdateHeroLevel(id, heroid);
                            WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(armyData.objectId,BattleUIType.UpdateHeroLevel,(int)heroid);
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}