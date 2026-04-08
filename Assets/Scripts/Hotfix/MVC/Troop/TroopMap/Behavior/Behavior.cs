using System.Collections.Generic;
using System.Linq;
using Client;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public abstract class Behavior : IBehavior
    {
        protected TroopProxy m_TroopProxy;
        protected WorldMapObjectProxy m_worldMapObjectProxy;
        protected Behavior()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
        }

        public int State { get; set; }
        
        public int LastState { get; set; }

        public virtual void Init(int id)
        {
        }

        public virtual void OnMOVE()
        {
        }

        public virtual void OnIDLE()
        {
        }

        public virtual void OnFIGHT()
        {
        }

        public virtual void OnFIGHTMOVE()
        {
        }

        public virtual void OnFIGHTANDFOLLOWUP()
        {
        }

        #region 部队享元

        protected void SendTroopsFightEvent(int id)
        {
            if (!TroopHelp.IsHaveState(State, ArmyStatus.BATTLEING))
            {
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopFightHud, id);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapCloseTroopHudScale, id);
        }

        protected void SendTroopsNoFightEvent(ArmyData armyData)
        {
            if (armyData == null)
            {
                return;
            }
            if (TroopHelp.IsHaveState(State, ArmyStatus.BATTLEING))
            {
                return;
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveTroopFightHud, armyData.objectId);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapOpenTroopHudScale, armyData.objectId);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapUpdateTroopHud, armyData.objectId);
        }

        protected void RemoveSound(ArmyData armyData)
        {
            if (armyData == null)
            {
                return;
            }

            WorldMapLogicMgr.Instance.BattleSoundHandler.RemoveBattleSoundByBattleHit(armyData,null);           
            WorldMapLogicMgr.Instance.BattleSoundHandler
                .RemoveSound(armyData.m_stopAudioHandler,true);
            WorldMapLogicMgr.Instance.BattleSoundHandler
                .RemoveSound(armyData.m_failAudioHandler,true);
            WorldMapLogicMgr.Instance.BattleSoundHandler
                .StopSound(armyData.m_moveAudioHandler);  
        }


    

        protected void AddSound(ArmyData armyData,  BattleSoundType type)
        {
            if (armyData == null)
            {
                return;
            }

            switch (type)
            {
                case  BattleSoundType.BattleHit:
                    WorldMapLogicMgr.Instance.BattleSoundHandler.AddBattleSoundByBattleHit(armyData,null);

                    break;
                case  BattleSoundType.Stop:
                    WorldMapLogicMgr.Instance.BattleSoundHandler
                        .AddBattleStopSound(armyData.go, (ah) =>
                        {
                            if (ah.IsDestroyed)
                            {
                                CoreUtils.audioService.SetHandlerVolume(ah, 0f);
                            }
                            else
                            {
                                CoreUtils.audioService.SetHandlerVolume(ah, 1f);
                            }
                        });
                    break;
                case  BattleSoundType.Move:
                    WorldMapLogicMgr.Instance.BattleSoundHandler
                        .PlaySound(armyData.m_moveAudioHandler);
                    break;
                case  BattleSoundType.Fail:
                    WorldMapLogicMgr.Instance.BattleSoundHandler
                        .AddBattleFailSound(armyData.go, (ah) =>
                        {
                            if (ah.IsDestroyed)
                            {
                                CoreUtils.audioService.SetHandlerVolume(ah, 0f);
                            }
                            else
                            {
                                armyData.m_failAudioHandler = ah;
                                CoreUtils.audioService.SetHandlerVolume(ah, 1f);
                            }
                        });            
                    break;    
            }
        }

        protected void RemoveLine(ArmyData armyData)
        {
            if (armyData == null)
            {
                return;
            }

            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine()
                .RemoveTroopLine(armyData.objectId);
        }

        protected Vector2 GetAttackPos(Troops formationTroop, ArmyData armyData)
        {
            Vector2 v2= Vector2.zero;
            if (armyData == null)
            {
                return v2;
            }
            
            Troops  monster = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetFormationBarbarian(armyData.targetId);
            Troops attackTroop = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetTroop(armyData.targetId);
            MapObjectInfoEntity  attackBuilding = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(armyData.targetId);

            Vector2 dir = Vector2.zero;
            Vector2 attackPos = Vector2.zero;
            if (monster != null)
            {
                attackPos = new Vector2(monster.transform.position.x, monster.transform.position.z);
            }
            else if (attackTroop != null)
            {
                attackPos = new Vector2(attackTroop.transform.position.x, attackTroop.transform.position.z);
            }
            else if (attackBuilding != null && attackBuilding.gameobject != null)
            {
                attackPos = new Vector2(attackBuilding.gameobject.transform.position.x,
                    attackBuilding.gameobject.transform.position.z);
            }
            else
            {
                attackPos = new Vector2(formationTroop.transform.position.x, formationTroop.transform.position.z);
                dir =TroopHelp.Rotated(Vector2.right, armyData.attackAngle).normalized; 
            }
            return attackPos + dir;
        }
        
        protected Vector2 GetAttackPos(MapObjectInfoEntity infoEntity)
        {
            Vector2 v2 = Vector2.zero;
      
            if (infoEntity.atkId == 0)
            {
                //异常情况下朝正右方
                CoreUtils.logService.Error("野蛮人或者守护者目标不存在！");
            }

            Troops atkformation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(infoEntity.atkId);
            if (atkformation != null)
            {
                v2 = new Vector2(atkformation.transform.position.x, atkformation.transform.position.z); ;
            }
            // 需要处理万一目标还没加载情况
            else if (infoEntity.gameobject != null)
            {
                v2 = new Vector2(infoEntity.gameobject.transform.position.x, infoEntity.gameobject.transform.position.z);
                v2 = v2 + TroopHelp.Rotated(Vector2.right, infoEntity.targetAngle / 100.0f).normalized;
            }

            return v2;
        }
        
        protected void SendFightEvent(int id)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateMonsterFightHud, id);
        }
        
        #endregion

        #region 怪物享元       

        protected void AddSound(MapObjectInfoEntity infoEntity,BattleSoundType type)
        {
            if (infoEntity == null)
            {
                return;
            }

            switch (type)
            {
                case  BattleSoundType.BattleHit:
                    WorldMapLogicMgr.Instance.BattleSoundHandler.AddBattleSoundByBattleHit(null,infoEntity);
                    break;
                    
            }
        }

        protected void RemoveSound(MapObjectInfoEntity infoEntity)
        {
            if (infoEntity == null)
            {
                return;
            }
            WorldMapLogicMgr.Instance.BattleSoundHandler.RemoveBattleSoundByBattleHit(null,infoEntity);          
        }


        #endregion
    }
}