using Client;
using Game;
using UnityEngine;
using System.Text;
using Skyunion;
using System;

namespace Hotfix
{
    public sealed class MonsterBehavior : Behavior
    {
        private MapObjectInfoEntity infoEntity;
        private Troops formationMonster;
        private int curMoveIndex = 0;

        public override void Init(int id)
        {
            base.Init(id);
            infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (infoEntity != null)
            {
                curMoveIndex = infoEntity.autoMoveIndex;
                formationMonster =
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetFormationBarbarian((int) infoEntity.objectId);
            }
            //if (Application.isEditor)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine();
            //    var values = Enum.GetValues(typeof(ArmyStatus));
            //    foreach (var state in values)
            //    {
            //        var enumState = (ArmyStatus)state;
            //        if (enumState == ArmyStatus.None)
            //            continue;

            //        if (TroopHelp.IsHaveState(State, enumState))
            //        {
            //            sb.Append("\t");
            //            sb.Append($"{enumState.ToString()}");
            //        }
            //    }
            //    Color color;
            //    ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
            //    CoreUtils.logService.Debug($"{id}\t Monster BattleData: ChangeState:{TroopHelp.GetTroopState(State)} {sb.ToString()}", color);
            //}
        }

        public override void OnIDLE()
        {
            base.OnIDLE();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }

            Vector2 dir =
                new Vector2(infoEntity.gameobject.transform.forward.x, infoEntity.gameobject.transform.forward.z)
                    .normalized * 0.001f;
            Vector2 startV2 = new Vector2(infoEntity.gameobject.transform.position.x,
                infoEntity.gameobject.transform.position.z);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(Troops.ENMU_MATRIX_TYPE.BARBARIAN, (int) infoEntity.objectId, Troops.ENMU_SQUARE_STAT.IDLE,
                startV2, startV2 + dir
            );
            NoFight();
        }

        public override void OnMOVE()
        {
            base.OnMOVE();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }

            curMoveIndex = infoEntity.autoMoveIndex;
            if (curMoveIndex < 0 || curMoveIndex + 1 >= infoEntity.path.Count)
            {
                Debug.LogWarning("野蛮人，移动路径有问题，通知服务器检查" + infoEntity.objectId);
                return;
            }

            Vector2 v2 = new Vector2(formationMonster.transform.position.x,formationMonster.transform.position.z);
            // 先注释掉，所有的行军，起点都要是当前位置， 如果不是当前位置，请使用瞬移的操作，直接设置坐标。
            //v2 = infoEntity.path[curMoveIndex];
            float moveSpeed = TroopHelp.GetMoveSpeed(infoEntity.path,
                infoEntity.arrivalTime, infoEntity.startTime);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(Troops.ENMU_MATRIX_TYPE.BARBARIAN, (int) infoEntity.objectId, Troops.ENMU_SQUARE_STAT.MOVE,
                v2,
                infoEntity.path[curMoveIndex + 1], moveSpeed);
            NoFight();

        }

        public override void OnFIGHT()
        {
            base.OnFIGHT();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }
            int id = (int) infoEntity.objectId;   
            Vector2 v2 = GetAttackPos(infoEntity);
            Vector2 curV2 = new Vector2(formationMonster.gameObject.transform.position.x,
                formationMonster.gameObject.transform.position.z);
            if (infoEntity.path.Count == 0)
            {
                curV2 = new Vector2(infoEntity.objectPos.x / 100.0f, infoEntity.objectPos.y / 100.0f);
            }
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(Troops.ENMU_MATRIX_TYPE.BARBARIAN, id,
                Troops.ENMU_SQUARE_STAT.FIGHT,
                curV2,
                v2);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(Troops.ENMU_MATRIX_TYPE.BARBARIAN, id,
                Troops.ENMU_SQUARE_STAT.FIGHT,
                curV2,
                v2);
            SendFightEvent(id);
            AddSound(infoEntity,BattleSoundType.BattleHit);
        }

        public override void OnFIGHTMOVE()
        {
            base.OnFIGHTMOVE();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }

            if (infoEntity.path.Count == 0)
            {
                return;
            }

            if (curMoveIndex >= 0 && curMoveIndex + 1 < infoEntity.path.Count)
            {
                // 围击点
                var startPos = infoEntity.path[curMoveIndex + 1];
                // 围击目标
                var attackPos = GetAttackPos(infoEntity);

                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                    Troops.ENMU_MATRIX_TYPE.BARBARIAN, (int)infoEntity.objectId, Troops.ENMU_SQUARE_STAT.FIGHT, startPos, attackPos);
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                    Troops.ENMU_MATRIX_TYPE.BARBARIAN, (int)infoEntity.objectId, Troops.ENMU_SQUARE_STAT.FIGHT, startPos, attackPos);
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                    Troops.ENMU_MATRIX_TYPE.BARBARIAN, (int)infoEntity.objectId, Troops.ENMU_SQUARE_STAT.FIGHT, startPos, attackPos);
            }

            SendFightEvent((int)infoEntity.objectId);
        }

        public override void OnFIGHTANDFOLLOWUP()
        {
            base.OnFIGHTANDFOLLOWUP();
            if (CheckNull() || CheckIsTroop())
            {
                return;
            }

            Vector2 v2 = new Vector2(formationMonster.transform.position.x, formationMonster.transform.position.z);
            if (curMoveIndex >= 0 && curMoveIndex + 1 < infoEntity.path.Count)
            {
                float moveSpeed = TroopHelp.GetMoveSpeed(infoEntity.path,
                    infoEntity.arrivalTime, infoEntity.startTime);
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(Troops.ENMU_MATRIX_TYPE.BARBARIAN,
                    (int)infoEntity.objectId, Troops.ENMU_SQUARE_STAT.MOVE, v2, infoEntity.path[curMoveIndex + 1], moveSpeed);
            }

            SendFightEvent((int)infoEntity.objectId);
            RemoveSound(infoEntity);
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
                WorldMapLogicMgr.Instance.BattleUIHandler
                    .SetBattleUIData((int)infoEntity.objectId, BattleUIType.BattleUI_Rout, null);
            }
            RemoveSound(infoEntity);
        }


        #region 判空

        private bool CheckNull()
        {
            if (infoEntity == null || formationMonster == null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIsTroop()
        {
            if (CheckNull())
            {
                return true;
            }

            if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .IsContainTroop((int)infoEntity.objectId))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}