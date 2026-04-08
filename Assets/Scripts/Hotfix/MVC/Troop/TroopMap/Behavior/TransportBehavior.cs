using Client;
using Game;
using UnityEngine;

namespace Hotfix
{
    public class TransportBehavior : Behavior
    {
        private ArmyData armyData;
        private int curMoveIndex = 0;
        public override void Init(int id)
        {
            base.Init(id);
            armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                curMoveIndex = armyData.autoMoveIndex;
            }
        }

        public override void OnMOVE()
        {
            base.OnMOVE();

            if (armyData == null)
            {
                Debug.LogError("数据空了不能移动");
                return;
            }

            if (armyData.movePath.Count == 0)
            {
                return;
            }

            if (curMoveIndex < 0 || curMoveIndex + 1 >= armyData.movePath.Count)
            {
                Debug.LogWarning("运输车，移动路径有问题，通知服务器检查" + armyData.objectId);
                return;
            }

            Color color = GetLineColor();
            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().AddTroopLine(armyData.objectId, armyData.movePath,
                color);
            
            Vector2 startV2 = armyData.movePath[curMoveIndex];
            if (armyData.isCreate)
            {
                startV2 = armyData.Pos;
            }
            float moveSpeed = TroopHelp.GetMoveSpeed(armyData.movePath,
                armyData.arrivalTime, armyData.startTime);
            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(Troops.ENMU_MATRIX_TYPE.COMMON, armyData.objectId, Troops.ENMU_SQUARE_STAT.MOVE,
                startV2, armyData.movePath[curMoveIndex + 1], moveSpeed);
        }

        private Color GetLineColor()
        {
            Color color = Color.white;
            if (armyData == null)
            {
                return color;
            }

            if (armyData.isPlayerHave)
            {
//                ColorUtility.TryParseHtmlString( "#66CD00", out color);
                return RS.green;
            }

            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if(playerProxy != null && playerProxy.CurrentRoleInfo.guildId != 0 && armyData.guild == playerProxy.CurrentRoleInfo.guildId)
            {
//                ColorUtility.TryParseHtmlString("#4876FF", out color);
                return RS.blue;
            }            

            return color;
        }
    }
}