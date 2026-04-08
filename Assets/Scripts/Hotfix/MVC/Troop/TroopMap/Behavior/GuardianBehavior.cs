using Client;
using Game;
using UnityEngine;

namespace Hotfix
{
    public class GuardianBehavior : Behavior
    {
        private MapObjectInfoEntity infoEntity;
        private Guardian formationGuardian;
        private int curMoveIndex = 0;
        
        public override void Init(int id)
        {
            base.Init(id);
            infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (infoEntity != null)
            {
                curMoveIndex= infoEntity.autoMoveIndex;
                infoEntity.monsterArmyStatus = (ArmyStatus) State;
                formationGuardian =
                    WorldMapLogicMgr.Instance.GuardianHandler.GetFormationGuardian((int) infoEntity.objectId);
            }
        }

        public override void OnIDLE()
        {
            base.OnIDLE();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }
            Vector2 startV2 = new Vector2(infoEntity.gameobject.transform.position.x,
                infoEntity.gameobject.transform.position.z);
            Vector2 dir= (new Vector2(infoEntity.gameobject.transform.forward.x, infoEntity.gameobject.transform.forward.z)
                              .normalized * 2.0f);
            WorldMapLogicMgr.Instance.GuardianHandler.SetStates(formationGuardian, Troops.ENMU_SQUARE_STAT.IDLE,startV2,startV2+dir);
            NoFight();
            UpdateGuardianHud(startV2.x, startV2.y);
        }

        public override void OnMOVE()
        {
            base.OnMOVE();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }
            if (curMoveIndex < 0 || curMoveIndex + 1 >= infoEntity.path.Count)
            {
                Debug.LogWarning("守护者，移动路径有问题，通知服务器检查" + infoEntity.objectId);
                return;
            }
            Vector2 v2 = infoEntity.path[curMoveIndex];
            float moveSpeed = TroopHelp.GetMoveSpeed(infoEntity.path,
                infoEntity.arrivalTime, infoEntity.startTime);
            WorldMapLogicMgr.Instance.GuardianHandler.SetStates(formationGuardian, Troops.ENMU_SQUARE_STAT.MOVE,v2,infoEntity.path[curMoveIndex+1],moveSpeed);
            NoFight();

        }

        public override void OnFIGHT()
        {
            base.OnFIGHT();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }
            Vector2 attackPos = GetAttackPos(infoEntity);
            Vector2 curV2 = new Vector2(formationGuardian.gameObject.transform.position.x,
                formationGuardian.gameObject.transform.position.z);
            WorldMapLogicMgr.Instance.GuardianHandler.SetStates(formationGuardian, Troops.ENMU_SQUARE_STAT.FIGHT,curV2,attackPos);
            WorldMapLogicMgr.Instance.GuardianHandler.SetStates(formationGuardian, Troops.ENMU_SQUARE_STAT.FIGHT,curV2,attackPos);
            SendFightEvent((int)infoEntity.objectId);
        }

        public override void OnFIGHTMOVE()
        {
            base.OnFIGHTMOVE();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }
        
            OnMoveFight();
        }

        public override void OnFIGHTANDFOLLOWUP()
        {
            base.OnFIGHTANDFOLLOWUP();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }

            if (curMoveIndex >= 0 && curMoveIndex + 1 < infoEntity.path.Count)
            {
                Vector2 curV2= new Vector2(formationGuardian.transform.position.x,formationGuardian.transform.position.z);
                float moveSpeed = TroopHelp.GetMoveSpeed(infoEntity.path,
                    infoEntity.arrivalTime, infoEntity.startTime);
                WorldMapLogicMgr.Instance.GuardianHandler.SetStates(formationGuardian, Troops.ENMU_SQUARE_STAT.MOVE,curV2,infoEntity.path[curMoveIndex+1],moveSpeed);
            }

           
        }
               
        private void OnMoveFight()
        {
            if (curMoveIndex >= 0 && curMoveIndex + 1 < infoEntity.path.Count)
            {
                Vector2 curV2= new Vector2(formationGuardian.transform.position.x,formationGuardian.transform.position.z);
                curV2 = infoEntity.path[curMoveIndex+1];
                Vector2 attackPos =GetAttackPos(infoEntity);      
                WorldMapLogicMgr.Instance.GuardianHandler.SetStates(formationGuardian, Troops.ENMU_SQUARE_STAT.FIGHT,curV2,attackPos);
                WorldMapLogicMgr.Instance.GuardianHandler.SetStates(formationGuardian, Troops.ENMU_SQUARE_STAT.FIGHT,curV2,attackPos);
                WorldMapLogicMgr.Instance.GuardianHandler.SetStates(formationGuardian, Troops.ENMU_SQUARE_STAT.FIGHT,curV2,attackPos);
            }
            SendFightEvent((int)infoEntity.objectId);
        }
        
        
        private void NoFight()
        {
            if (infoEntity.objectId <= 0)
            {
                return;
            }

            
            if (!TroopHelp.IsHaveState(infoEntity.status,ArmyStatus.MONSTER_FAILED))
            {
                AppFacade.GetInstance()
                    .SendNotification(CmdConstant.MapRemoveMonsterFightHud, (int) infoEntity.objectId);
            }
            else
            {
                WorldMapLogicMgr.Instance.GuardianHandler.FadeOut_S(formationGuardian);
            }
        }

        #region 判空

        private bool CheckNull()
        {
            if (infoEntity == null || formationGuardian == null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIsTroop()
        {
            if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .IsContainTroop((int)infoEntity.objectId))
            {
                return true;
            }

            return false;
        }
        
        private void UpdateGuardianHud(float x, float z)
        {
            var obj = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(infoEntity.objectId);
            obj.objectPos.x = (long) x * 100;
            obj.objectPos.y = (long) z * 100;
            AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectHUDUpdate, obj);
        }

        #endregion
    }
}