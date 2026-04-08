using System;
using System.Collections.Generic;
using System.Text;
using Client;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public  class PlayBehavior : Behavior
    {
        private ArmyData armyData;
        private Troops formationTroop;
        private Troops attackTroop;
        private MapObjectInfoEntity attackBuilding;
        private Troops monster;
        private List<Vector2> movePath = new List<Vector2>();
        private int curMoveIndex = 0;

        public override void Init(int id)
        {
            base.Init(id);
            armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetArmyData(id);
            if (armyData != null)
            {                    
                curMoveIndex = armyData.autoMoveIndex;
                //Debug.LogError("更新部队状态" + (ArmyStatus) State+"***"+id+"***"+"***"+State+"***"+curMoveIndex+"***"+(armyData.arrivalTime - armyData.startTime));  
                formationTroop =
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                        .GetTroop(id);
            }
            if (Application.isEditor)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                var values = Enum.GetValues(typeof(ArmyStatus));
                foreach (var state in values)
                {
                    var enumState = (ArmyStatus)state;
                    if (enumState == ArmyStatus.None)
                        continue;

                    if (TroopHelp.IsHaveState(State, enumState))
                    {
                        sb.Append("\t");
                        sb.Append($"{enumState.ToString()}");
                    }
                }
                Color color;
                ColorUtility.TryParseHtmlString("#" + (Time.frameCount%255*12354687).ToString("X"), out color);
                CoreUtils.logService.Debug($"{id}\tBattleData: ChangeState:{TroopHelp.GetTroopState(State)} {sb.ToString()}", color);
            }
        }

        public override void OnIDLE()
        {
            base.OnIDLE();
            if (armyData == null || formationTroop == null)
            {
                return;
            }

            Vector2 dir =
                new Vector2(formationTroop.transform.forward.x, formationTroop.transform.forward.z).normalized * 0.01f;
            // 使用待机位置，这样如果位置不对会自动矫正
            var pos = armyData.Pos;
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                formationTroop.GetFormationType(), armyData.objectId, Troops.ENMU_SQUARE_STAT.IDLE, pos, pos + dir);
            Troops.ENMU_SQUARE_STAT state = TroopHelp.GetTroopState(LastState);     
            RemoveSound(armyData);
            if (state > 0 && state == Troops.ENMU_SQUARE_STAT.MOVE)
            {
                AddSound(armyData, BattleSoundType.Stop);
            }
            RemoveLine(armyData);
            SendTroopsNoFightEvent(armyData);
        }

        public override void OnMOVE()
        {
            base.OnMOVE();
            if (armyData == null || formationTroop == null)
            {
                return;
            }

            if(armyData.movePath.Count == 0)
            {
                return;
            }

            curMoveIndex = armyData.autoMoveIndex;
            if (curMoveIndex < 0 || curMoveIndex+1 >= armyData.movePath.Count)
            {
                Debug.LogWarning("玩家部队，移动路径有问题，通知服务器检查" + armyData.objectId);
                return;
            }

            Vector2 v2 = TroopHelp.GetLineDestOffset(armyData);
            movePath = TroopHelp.GetMovePathWithOffset(armyData.movePath, v2);
            Color color = WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine()
                .GetLineColor(armyData.objectId);

            Vector2 startV2 = armyData.movePath[curMoveIndex];
            if (armyData.isCreate)
            {
                startV2 = armyData.Pos;
            }
            else
            {
                // 起始点必须是当前位置
                //startV2 = new Vector2(formationTroop.transform.position.x,formationTroop.transform.position.z);
                // 兼容表现，直接插入一个行军线
                //movePath.Insert(curMoveIndex, startV2);
                // 现在起始点可以直接使用 服务器的位置， 部队那边会自动调整位置
                startV2 = armyData.GetMovePos();
            }
            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().AddTroopLine(
                armyData.objectId, movePath,
                color);

            Vector2 targetV2 = armyData.movePath[curMoveIndex + 1];
            float moveSpeed = TroopHelp.GetMoveSpeed(armyData.movePath,armyData.arrivalTime, armyData.startTime);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                formationTroop.GetFormationType(), armyData.objectId, Troops.ENMU_SQUARE_STAT.MOVE, startV2, targetV2, moveSpeed);

            if (Application.isEditor)
            {
                Color color2;
                ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color2);
                CoreUtils.logService.Debug($"{armyData.objectId}\tBattleData: Move:{armyData.startTime} {armyData.arrivalTime} {startV2} {armyData.Pos} {ServerTimeModule.Instance.GetServerTime()- armyData.startTime} {ServerTimeModule.Instance.GetServerTimeMilli()} {(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000)/10000}", color);
            }

            RemoveSound(armyData);
            if (TroopHelp.IsHaveState((long)armyData.armyStatus,ArmyStatus.FAILED_MARCH))
            {
                if (armyData.isPlayerHave)
                {                            
                    Troops.ENMU_SQUARE_STAT state = TroopHelp.GetTroopState(LastState);
                    if (state > 0 && state == Troops.ENMU_SQUARE_STAT.MOVE)
                    {
                        WorldMapLogicMgr.Instance.BattleUIHandler
                            .SetBattleUIData(armyData.objectId, BattleUIType.BattleUI_Fail, null);
                        AddSound(armyData, BattleSoundType.Fail);
                    }
                }
                WorldMapLogicMgr.Instance.BattleUIHandler
                    .SetBattleUIData(armyData.objectId, BattleUIType.BattleUI_Rout, null);

                SendTroopsNoFightEvent(armyData);
            }
            else if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.BATTLEING))
            {
                SendTroopsFightEvent(armyData.objectId);
            }
            else
            {
                AddSound(armyData,BattleSoundType.Move);
                SendTroopsNoFightEvent(armyData);
            }
        }

        public override void OnFIGHT()
        {
            base.OnFIGHT();
            if (armyData == null || formationTroop == null)
            {
                return;
            }

            Vector2 pos = new Vector2(formationTroop.transform.position.x,
                formationTroop.transform.position.z);

            Vector2 attackPos = GetAttackPos(formationTroop, armyData);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                formationTroop.GetFormationType(), armyData.objectId, Troops.ENMU_SQUARE_STAT.FIGHT, pos, attackPos);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                formationTroop.GetFormationType(), armyData.objectId,
                Troops.ENMU_SQUARE_STAT.FIGHT, pos, attackPos);
           // RemoveSound(armyData);
            AddSound(armyData, BattleSoundType.BattleHit);
            RemoveLine(armyData);
            SendTroopsFightEvent(armyData.objectId);
        }

        public override void OnFIGHTMOVE()
        {
            if (armyData == null || formationTroop == null)
            {
                return;
            }
            // 这边战斗绕圈的，后续会改一下， 
            base.OnFIGHTMOVE();

            if (armyData.movePath.Count == 0)
            {
                return;
            }

            if (curMoveIndex < 0 || curMoveIndex + 1 >= armyData.movePath.Count)
            {
                Debug.LogWarning("玩家部队，移动路径有问题，通知服务器检查" + armyData.objectId);
                return;
            }

            // 围击点
            var startPos = armyData.movePath[curMoveIndex + 1];
            // 围击目标
            var attackPos = GetAttackPos(formationTroop, armyData);

            // 围击
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                formationTroop.GetFormationType(), armyData.objectId, Troops.ENMU_SQUARE_STAT.FIGHT, startPos, attackPos);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                formationTroop.GetFormationType(), armyData.objectId, Troops.ENMU_SQUARE_STAT.FIGHT, startPos, attackPos);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                formationTroop.GetFormationType(), armyData.objectId, Troops.ENMU_SQUARE_STAT.FIGHT, startPos, attackPos);
           // RemoveSound(armyData);
           // AddSound(armyData, BattleSoundType.BattleHit);
            SendTroopsFightEvent(armyData.objectId);
            RemoveLine(armyData);
        }


        public override void OnFIGHTANDFOLLOWUP()
        {
            if (armyData == null || formationTroop == null)
            {
                return;
            }
            base.OnFIGHTANDFOLLOWUP();
            Color color = WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine()
                .GetLineColor(armyData.objectId);

            if (armyData.movePath.Count == 0)
            {
                return;
            }

            curMoveIndex = armyData.autoMoveIndex;
            if (curMoveIndex < 0 || curMoveIndex + 1 >= armyData.movePath.Count)
            {
                Debug.LogWarning("玩家部队，移动路径有问题，通知服务器检查" + armyData.objectId);
                return;
            }
            Vector2 pos = armyData.GetMovePos();
            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine()
                .AddTroopLine(armyData.objectId, armyData.movePath, color);
            float moveSpeed = TroopHelp.GetMoveSpeed(armyData.movePath, armyData.arrivalTime, armyData.startTime);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
                formationTroop.GetFormationType(), armyData.objectId, Troops.ENMU_SQUARE_STAT.MOVE, pos,
                armyData.movePath[curMoveIndex + 1],
                moveSpeed);

            SendTroopsFightEvent(armyData.objectId);
            RemoveSound(armyData);
            if (armyData.isPlayerHave)
            {
                AddSound(armyData,BattleSoundType.Move);
            }
        }
    }
}