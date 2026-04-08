using System;
using System.Text;
using Client;
using Game;
using UnityEngine;
using Skyunion;

namespace Hotfix
{
    public sealed class PlayScoutBehavior : Behavior
    {
        private ArmyData armyData;
        private int curMoveIndex = 0;

        public override void Init(int id)
        {
            base.Init(id);
            armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutData(id);
            if (armyData != null)
            {
                curMoveIndex = armyData.autoMoveIndex;
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
            //    CoreUtils.logService.Debug($"{id}\tBattleData: ChangeState:{TroopHelp.GetTroopState(State)} curMoveIndex:{curMoveIndex} {sb.ToString()}", color);
            //}
        }

        public override void OnMOVE()
        {
            base.OnMOVE();
            if (armyData == null)
            {
                Debug.LogError("数据空了不能移动");
                return;
            }

            if (curMoveIndex < 0 || curMoveIndex + 1 >= armyData.movePath.Count)
            {
                Debug.LogWarning("玩家斥候，移动路径有问题" + armyData.objectId+"*"+curMoveIndex+"*"+armyData.movePath.Count);
                return;
            }

            Color color = GetLineColor();
            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveSummonerTroopLine(armyData.scoutIndex);
            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().AddTroopLine(armyData.objectId, armyData.movePath,
                color);

            Vector2 startV2 = armyData.movePath[curMoveIndex];

            //如果此处取初始坐标，由于网络的延迟时间，并且斥候移动速度快，表现上会有一段误差距离
            if (armyData.isCreate)
            {
                startV2 = armyData.Pos;
            }
            else
            {
                startV2 = armyData.GetMovePos();
            }

            float moveSpeed = TroopHelp.GetMoveSpeed(armyData.movePath,
                armyData.arrivalTime, armyData.startTime);

            if ((curMoveIndex + 1) <armyData.movePath.Count)
            {
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(Troops.ENMU_MATRIX_TYPE.COMMON, armyData.objectId, Troops.ENMU_SQUARE_STAT.MOVE,
                startV2, armyData.movePath[curMoveIndex + 1], moveSpeed);
            }
        }

        private Color GetLineColor()
        {
            Color color = Color.white;
            ColorUtility.TryParseHtmlString("#64d37c", out color);
            return color;
        }
    }
}