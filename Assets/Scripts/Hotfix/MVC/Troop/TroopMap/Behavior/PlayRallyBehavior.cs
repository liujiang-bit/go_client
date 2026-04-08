namespace Hotfix
{
    public sealed class PlayRallyBehavior : PlayBehavior
    {
//        private ArmyData armyData;
//        private Formation formationTroop;
//        private int curMoveIndex = 0;
//        private List<Vector2> movePath = new List<Vector2>();
//        private RallyTroopsProxy m_RallyTroopsProxy;
//
//        public override void Init(int id)
//        {
//            base.Init(id);
//            if (m_RallyTroopsProxy == null)
//            {
//                m_RallyTroopsProxy =
//                    AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
//            }
//
//            armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
//            if (armyData != null)
//            {              
//                curMoveIndex = armyData.autoMoveIndex;
//                Debug.Log("集结部队状态"+(ArmyStatus) State);
//                formationTroop = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(id);
//            }
//        }
//
//        public override void OnIDLE()
//        {
//            base.OnIDLE();
//            if (armyData == null || formationTroop == null)
//            {
//                return;
//            }
//
//            Vector2 dir = new Vector2(formationTroop.transform.forward.x, formationTroop.transform.forward.z).normalized * 0.001f;
//            Vector2 pos = new Vector2(formationTroop.gameObject.transform.position.x, formationTroop.gameObject.transform.position.z);
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.IDLE, pos, pos + dir);
//            RemoveSound(armyData);
//            RemoveLine(armyData);
//            SendTroopsNoFightEvent(armyData);
//        }
//
//        public override void OnMOVE()
//        {
//            base.OnMOVE();
//            if (armyData == null || formationTroop == null)
//            {
//                return;
//            }
//
//            if (curMoveIndex < 0 || curMoveIndex >= armyData.movePath.Count)
//            {
//                Debug.LogWarning("移动路径有问题,通知服务器检查" + armyData.objectId);
//                return;
//            }
//
//            Vector2 offset = Vector2.zero;
//            if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.ATTACK_MARCH))
//            {
//                offset = (armyData.movePath.Last() - armyData.movePath.First()).normalized * armyData.armyRadius;
//            }
//
//            movePath = TroopHelp.GetMovePathWithOffset(armyData.movePath, offset);
//
//            Color color = GetColor();
//            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().AddTroopLine(
//                armyData.objectId, movePath,
//                color);
//            Vector2 startV2 = armyData.movePath[curMoveIndex];
//            if (armyData.isCreate)
//            {
//                startV2 = armyData.Pos;
//            }
//            else
//            {
//                // 先注释掉，因为移动必须是当前位置来移动， 不然就是直接设置坐标得方式移动
//                //if (armyData.isAutoMove)
//                {
//                    startV2 = new Vector2(formationTroop.transform.position.x, formationTroop.transform.position.z);
//                }
//            }
//                
//
//            Vector2 targetV2 = armyData.movePath[curMoveIndex + 1];
//            float moveSpeed = TroopHelp.GetMoveSpeed(armyData.movePath,
//                armyData.arrivalTime, armyData.startTime);
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.MOVE, startV2, targetV2, moveSpeed);
//            RemoveSound(armyData);
//
//            if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH))
//            {
//                if (armyData.isPlayerHave)
//                {
//                    WorldMapLogicMgr.Instance.BattleUIHandler
//                        .SetBattleUIData(armyData.objectId, BattleUIType.BattleUI_Fail, null);
//                    AddSound(armyData, BattleSoundType.Fail);
//                }
//                WorldMapLogicMgr.Instance.BattleUIHandler
//                    .SetBattleUIData(armyData.objectId, BattleUIType.BattleUI_Rout, null);
//
//                SendTroopsNoFightEvent(armyData);
//            }
//            else if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.BATTLEING))
//            {
//                SendTroopsFightEvent(armyData.objectId);
//            }
//            else
//            {
//                SendTroopsNoFightEvent(armyData);
//            }
//        }
//        
//        
//        public override void OnFIGHTMOVE()
//        {
//            base.OnFIGHTMOVE();
//            Vector2 dir =
//                new Vector2(formationTroop.transform.forward.x, formationTroop.transform.forward.z).normalized * 0.001f;
//            float moveSpeed = TroopHelp.GetMoveSpeed(armyData.movePath, armyData.arrivalTime, armyData.startTime);
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.MOVE, armyData.movePath[curMoveIndex],
//                armyData.movePath[curMoveIndex + 1] + dir,
//                moveSpeed);
//        }
//        
//
//        public override void OnFIGHT()
//        {
//            base.OnFIGHT();
//            if (armyData == null || formationTroop == null)
//            {
//                return;
//            }
//
//            Vector2 pos = new Vector2(formationTroop.transform.position.x,
//                formationTroop.transform.position.z);
//
//            Vector2 attackPos = GetAttackPos(formationTroop, armyData);
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.FIGHT, pos, attackPos);
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.FIGHT, pos, attackPos);
//            AddSound(armyData, BattleSoundType.BattleHit);
//            RemoveLine(armyData);
//            SendTroopsFightEvent(armyData.objectId);
//        }
//
//      
//        public override void OnFIGHTADJUST()
//        {
//            base.OnFIGHTADJUST();
//            Vector2 pos = new Vector2(formationTroop.transform.position.x,
//                formationTroop.transform.position.z);
//            Vector2 dir = GetDir(armyData);
//            Vector2 attackPos = armyData.movePath.Last();
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.FIGHT, pos, attackPos + dir);
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.FIGHT, pos, attackPos + dir);
//        }
//
//        public override void OnFIGHTANDFOLLOWUP()
//        {
//            base.OnFIGHTANDFOLLOWUP();
//            Vector2 troopPos = new Vector2(formationTroop.transform.position.x, formationTroop.transform.position.z);
//            Color color = WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine()
//                .GetLineColor(armyData.objectId);
//            Vector2 v2 = Vector2.zero;
//            if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.ATTACK_MARCH))
//            {
//                Vector2 curPos = new Vector2(formationTroop.transform.position.x, formationTroop.transform.position.z);
//                v2 = (armyData.movePath.Last() - curPos).normalized *
//                     armyData.armyRadius;
//            }
//
//            movePath.Clear();
//            for (int i = 0; i < armyData.movePath.Count; i++)
//            {
//                var pos = armyData.movePath[i];
//                if (i == armyData.movePath.Count - 1)
//                {
//                    pos = pos + v2;
//                }
//
//                movePath.Add(pos);
//            }
//
//            Vector2 dir = GetDir(armyData);
//            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine()
//                .AddTroopLine(armyData.objectId, movePath, color);
//            float moveSpeed = TroopHelp.GetMoveSpeed(armyData.movePath, armyData.arrivalTime, armyData.startTime);
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.FIGHT, armyData.movePath[curMoveIndex],
//                armyData.movePath[curMoveIndex + 1] + dir,
//                moveSpeed);
//            WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().ChangeFormationState(
//                Formation.ENMU_SQUARE_TYPE.RALLY, armyData.objectId, 0,
//                Formation.ENMU_SQUARE_STAT.FIGHT, armyData.movePath[curMoveIndex],
//                armyData.movePath[curMoveIndex + 1] + dir,
//                moveSpeed);
//            SendTroopsFightEvent(armyData.objectId);
//        }
//
//        private Color GetColor()
//        {
//            Color color= Color.white;
//            if (armyData.isRally)
//            {
//                if (m_RallyTroopsProxy.IsCaptainByarmIndex(armyData.objectId))
//                {
//                    ColorUtility.TryParseHtmlString("#64d37c", out color);
//                    
//                }else if (m_RallyTroopsProxy.isRallyTroopHaveGuid(armyData.armyRid))
//                {
//                    ColorUtility.TryParseHtmlString("#52aeee", out color);
//                    
//                }else if (m_RallyTroopsProxy.HasRallyed(armyData.targetId))
//                {
//                   // ColorUtility.TryParseHtmlString("#e1554f", out color);
//                    color = RS.red;
//                }                
//            }
//            return color;
//        }
    }
}