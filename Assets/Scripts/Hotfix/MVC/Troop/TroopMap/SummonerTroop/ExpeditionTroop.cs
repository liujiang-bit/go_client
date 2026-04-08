using System.Collections.Generic;
using UnityEngine;
using Client;
using Hotfix;
using Skyunion;
using SprotoType;

namespace Game
{
    public class ExpeditionTroop : IExpeditionTroop
    {
        private Dictionary<int, ExpeditionArmyData> m_dictEnemyArmyData = new Dictionary<int, ExpeditionArmyData>();
        private Dictionary<int, ExpeditionArmyData> m_dictPlayerArmyData = new Dictionary<int, ExpeditionArmyData>();
        private Dictionary<int, Troops> m_monsterFormation = new Dictionary<int, Troops>(); //monster index 为key
        private Dictionary<int, Troops> m_playerFormation = new Dictionary<int, Troops>(); //army index 为key
        public Dictionary<int, HUDUI> m_monsterAtkHudui = new Dictionary<int, HUDUI>();

        private TroopProxy m_troopProxy = null;
        private ExpeditionProxy m_expeditionProxy = null;

        public void Init()
        {
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
        }
        
        private Transform GetTroopsRoot()
        {
            WorldMgrMediator worldMgrMediator = AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            if(worldMgrMediator != null)
            {
                return worldMgrMediator.GetTroopsRoot();
            }
            return null;
        }

        public void Clear()
        {
            foreach(var armyData in m_dictEnemyArmyData)
            {
                armyData.Value.RemoveSoundHandler();
            }
            m_dictEnemyArmyData.Clear();
            foreach (var armyData in m_dictPlayerArmyData)
            {
                armyData.Value.RemoveSoundHandler();
            }
            m_dictPlayerArmyData.Clear();
            foreach (var formation in m_monsterFormation)
            {
                CoreUtils.assetService.Destroy(formation.Value.gameObject);
            }
            m_monsterFormation.Clear();
            foreach (var formation in m_playerFormation)
            {
                CoreUtils.assetService.Destroy(formation.Value.gameObject);
            }
            m_playerFormation.Clear();

            foreach(var hud in m_monsterAtkHudui)
            {
                var hudUi = hud.Value;
                ClientUtils.hudManager.CloseSingleHud(ref hudUi);
            }
            m_monsterAtkHudui.Clear();
        }

        public void CreatePreviewMonsterFormation(ExpeditionMosnterTroopData monsterTroopData)
        {
            if (monsterTroopData == null) return;
            CoreUtils.assetService.Instantiate(monsterTroopData.MonsterCfg.modelId, (go) =>
            {
                if(m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PrepareNormal &&
                m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PreparePreview)
                {
                    CoreUtils.assetService.Destroy(go);
                    return;
                }
                if (m_monsterFormation.ContainsKey(monsterTroopData.Index))
                {
                    CoreUtils.assetService.Destroy(m_monsterFormation[monsterTroopData.Index].gameObject);
                    m_monsterFormation.Remove(monsterTroopData.Index);
                }
                go.transform.SetParent(GetTroopsRoot());
                go.transform.localScale = Vector3.one;
                go.name = $"expedition_preview_enemy_{monsterTroopData.Index}";                
                Troops formation = go.GetComponent<Troops>();
                string des = GetFormationDes(monsterTroopData.TroopsCfg.heroID1, monsterTroopData.TroopsCfg.heroID2, monsterTroopData.Soldiers);
                Troops.InitPositionS(formation, monsterTroopData.BornPosisiton, monsterTroopData.BornPosisiton + monsterTroopData.Forward);
                Troops.InitFormationS(formation, des, Color.gray);
                Troops.SetStateS(formation, Troops.ENMU_SQUARE_STAT.IDLE, monsterTroopData.BornPosisiton, monsterTroopData.BornPosisiton + monsterTroopData.Forward * 0.01f);
                Troops.FadeIn_S(formation);
                m_monsterFormation.Add(monsterTroopData.Index, formation);
                
                CreateTroopHud(go, monsterTroopData.Index);//添加攻击图标
            });
        }

        private void CreateTroopHud(GameObject go, int index)
        {
            HUDUI huduiTroop = HUDUI.Register(UI_Pop_WorldArmyCmdView.VIEW_NAME, typeof(UI_Pop_WorldArmyCmdView),
                HUDLayer.world, go).SetData(index).SetInitCallback((hud) =>
                         {
                             OnTroopHudCallBack(hud);
                         }).SetTargetGameObject(go).SetCameraLodDist(0, 3000f).SetPositionAutoAnchor(true);
            
            huduiTroop.SetUpdateCallback((HUDUI hudui) =>
            {
                huduiTroop.gameView.gameObject.SetActive(true);
            });
            
            ClientUtils.hudManager.ShowHud(huduiTroop);
            if (m_monsterAtkHudui.ContainsKey(index))
            {
                var hudUi = m_monsterAtkHudui[index];
                ClientUtils.hudManager.CloseSingleHud(ref hudUi);
                m_monsterAtkHudui.Remove(index);
            }
            m_monsterAtkHudui.Add(index, huduiTroop);
        }

        private void OnTroopHudCallBack(HUDUI info)
        {
            UI_Pop_WorldArmyCmdView view = info.gameView as UI_Pop_WorldArmyCmdView;
            if (view != null)
            {
                int index = (int)info.data;
                view.gameObject.name = string.Format("{0}_{1}_{2}", UI_Pop_WorldArmyCmdView.VIEW_NAME, "PrivewTroop", index);
                info.gameView.gameObject.GetComponent<MapElementUI>().enabled = false;
                view.m_pl_head.gameObject.SetActive(false);
                view.m_pl_time.gameObject.SetActive(false);
                view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                view.m_img_state_atk_PolygonImage.gameObject.SetActive(true);
            }
        }

        public Troops GetPreviewMonsterFormation(int monsterIndex)
        {
            Troops formation = null;
            m_monsterFormation.TryGetValue(monsterIndex, out formation);
            return formation;
        }

        public void CreatePreviewPlayerFormation(ExpeditionPlayerTroopData playerTroopData)
        {
            if (playerTroopData == null) return;
            CoreUtils.assetService.Instantiate("Formation", (go) =>
            {
                if (m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PrepareNormal &&
                m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PreparePreview)
                {
                    CoreUtils.assetService.Destroy(go);
                    return;
                }
                if (m_playerFormation.ContainsKey(playerTroopData.Index))
                {
                    CoreUtils.assetService.Destroy(m_playerFormation[playerTroopData.Index].gameObject);
                    m_playerFormation.Remove(playerTroopData.Index);
                }
                go.transform.SetParent(GetTroopsRoot());
                go.transform.localScale = Vector3.one;
                go.name = $"expedition_preview_player_{playerTroopData.Index}";
                Troops formation = go.GetComponent<Troops>();
                string des = GetFormationDes(playerTroopData.MainHeroId, playerTroopData.DeputyHeroId, playerTroopData.Soldiers);
                Troops.InitPositionS(formation, playerTroopData.BornPosisiton, playerTroopData.BornPosisiton + playerTroopData.Forward);
                Troops.InitFormationS(formation, des, RS.blue_troop);
                Troops.SetStateS(formation, Troops.ENMU_SQUARE_STAT.IDLE, playerTroopData.BornPosisiton, playerTroopData.BornPosisiton + playerTroopData.Forward * 0.01f);
                Troops.FadeIn_S(formation);
                m_playerFormation.Add(playerTroopData.Index, formation);
            });
        }

        public void DestroyPreviewPlayerFormation(int troopIndex)
        {
            if(m_playerFormation.ContainsKey(troopIndex))
            {
                CoreUtils.assetService.Destroy(m_playerFormation[troopIndex].gameObject);
                m_playerFormation.Remove(troopIndex);
            }
        }

        private string GetFormationDes(int mainHeroId, int deputyHeroId, Dictionary<long, SoldierInfo> soldiers)
        {
            string des = string.Empty;
            var squareHelper = m_troopProxy.GetSquareHelper();
            if(squareHelper != null)
            {
                des = squareHelper.GetMapCreateTroopDes(mainHeroId, deputyHeroId, soldiers);
            }
            return des;
        }

        public ArmyData CreateArmyData(SprotoType.MapObjectInfo mapObjectInfo)
        {
            bool isMonster = mapObjectInfo.armyRid == 0;
            int index = isMonster ? (int)mapObjectInfo.monsterIndex : (int)mapObjectInfo.armyIndex;
            ExpeditionArmyData data = InternalGetArmyData(isMonster, index);
            if(data == null)
            {
                data = new ExpeditionArmyData((int)mapObjectInfo.objectId);
                if(data.IsEnemy)
                {
                    m_dictEnemyArmyData.Add(data.dataIndex, data);
                }
                else
                {
                    m_dictPlayerArmyData.Add(data.dataIndex, data);
                }
            }
            else
            {
                data.FillMapObjectInfo((int)mapObjectInfo.objectId);
            }
            Vector2 forwamrd = Vector2.zero;
            if(data.IsEnemy)
            {
                var monsterData = m_expeditionProxy.GetMonsterTroopData(data.dataIndex);
                if(monsterData != null)
                {
                    forwamrd = monsterData.Forward;
                }
            }
            else
            {
                var playerData = m_expeditionProxy.GetPlayerTroopData(data.dataIndex);
                if(playerData != null)
                {
                    forwamrd = playerData.Forward;
                }
            }
            if(forwamrd != Vector2.zero)
            {
                Vector2 initPos = m_expeditionProxy.ExpeditionPosToWorldPos(mapObjectInfo.objectPos.x / 100f, mapObjectInfo.objectPos.y / 100f);
                data.FormationInitTargetPos = initPos + forwamrd;
            }          
            return data;
        }

        private ExpeditionArmyData InternalGetArmyData(bool isMonster, int index)
        {
            ExpeditionArmyData data = null;
            if (isMonster)
            {
                m_dictEnemyArmyData.TryGetValue(index, out data);
            }
            else
            {
                m_dictPlayerArmyData.TryGetValue(index, out data);
            }
            return data;
        }

        public void CreateEnemyArmyData(ExpeditionMosnterTroopData monsterTroopData)
        {
            ExpeditionArmyData data = new ExpeditionArmyData(monsterTroopData.Index);
            data.heroId = monsterTroopData.TroopsCfg.heroID1;
            data.viceId = monsterTroopData.TroopsCfg.heroID2;
            int soldierNum = 0;
            foreach(var kv in monsterTroopData.Soldiers)
            {
                soldierNum += (int)(kv.Value.num);
            }
            data.troopNums = data.troopNumMax = soldierNum;
            data.MonsterRadius = monsterTroopData.MonsterCfg.radius;
            m_dictEnemyArmyData.Add(monsterTroopData.Index, data);
        }

        public void CreatePlayerArmyData(ExpeditionPlayerTroopData playerTroopData)
        {
            ExpeditionArmyData data = new ExpeditionArmyData(playerTroopData.Index);
            data.heroId = playerTroopData.MainHeroId;
            data.viceId = playerTroopData.DeputyHeroId;
            int soldierNum = 0;
            foreach (var kv in playerTroopData.Soldiers)
            {
                soldierNum += (int)(kv.Value.num);
            }
            data.troopNums = data.troopNumMax = soldierNum;
            data.armyStatus = (long)ArmyStatus.STATIONING;
            m_dictPlayerArmyData.Add(playerTroopData.Index, data);
        }

        public ArmyData GetArmyData(int objectId)
        {
            return InternalGetArmyData(objectId);
        }

        public ArmyData GetArmyData(bool isPlayer, int troopIndex)
        {
            ExpeditionArmyData data = null;
            if(isPlayer)
            {
                m_dictPlayerArmyData.TryGetValue(troopIndex, out data);
            }
            else
            {
                m_dictEnemyArmyData.TryGetValue(troopIndex, out data);
            }
            return data;
        }

        private ExpeditionArmyData InternalGetArmyData(int objectId)
        {
            if (objectId == 0) return null;
            foreach (var kv in m_dictEnemyArmyData)
            {
                if (kv.Value.objectId == objectId) return kv.Value;
            }
            foreach (var kv in m_dictPlayerArmyData)
            {
                if (kv.Value.objectId == objectId) return kv.Value;
            }
            return null;
        }

        public void RemoveArmyData(int objectId)
        {
            ExpeditionArmyData data = InternalGetArmyData(objectId);
            if (data == null) return;
            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionTroopRemove, data);
        }

        public List<ArmyData> GetEnemyDatas()
        {
            return new List<ArmyData>(m_dictEnemyArmyData.Values);
        }

        public List<ArmyData> GetPlayerTroopDatas()
        {
            return new List<ArmyData>(m_dictPlayerArmyData.Values);
        }

        public void PlayArmyDeadPerofrmance(int objectId, Troops formation)
        {
            if (formation == null) return;
            ArmyData armyData = GetArmyData(objectId);
            if (armyData == null) return;
            Vector3 forward = formation.transform.forward;
            armyData.armyStatus = (long)ArmyStatus.FAILED_MARCH;
            armyData.ClearMovePath();
            Vector3 endPos = formation.transform.position - forward;
            armyData.SetMovePath(new Vector2(formation.transform.position.x, formation.transform.position.z));
            armyData.SetMovePath(new Vector2(endPos.x, endPos.z));
            armyData.startTime = ServerTimeModule.Instance.GetServerTime();
            armyData.arrivalTime = armyData.startTime + 1;
            WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(objectId, (int)armyData.armyStatus);
            Troops.FadeOut_S(formation);
        }

        public void CalScreenViceArmList(int objectId, ref List<int> viceArmyObjectIdList)
        {
            viceArmyObjectIdList.Clear();

            foreach (var armyData in m_dictPlayerArmyData.Values)
            {
                if (armyData.objectId == objectId) continue;

                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.PALLY_MARCH)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.COLLECTING)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.GARRISONING)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.RALLY_WAIT)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.RALLY_BATTLE)) continue;

                Vector3 troopPos = Vector3.zero;
                if (armyData.go != null)
                {
                    troopPos = armyData.go.transform.position;
                }
                else
                {
                    Vector2 pos = armyData.GetMovePos();
                    troopPos = new Vector3(pos.x, 0, pos.y);
                }

                if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), troopPos.x, troopPos.z))
                {
                    viceArmyObjectIdList.Add(armyData.objectId);
                }
            }
        }

        public void CalWorldViceArmList(int objectId, ref List<int> viceArmyObjectIdList)
        {
            viceArmyObjectIdList.Clear();

            foreach (var armyData in m_dictPlayerArmyData.Values)
            {
                if (armyData.objectId == objectId) continue;

                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.PALLY_MARCH)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.COLLECTING)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.GARRISONING)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.RALLY_WAIT)) continue;
                if (TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.RALLY_BATTLE)) continue;

                viceArmyObjectIdList.Add(armyData.objectId);
            }
        }
    }

}
