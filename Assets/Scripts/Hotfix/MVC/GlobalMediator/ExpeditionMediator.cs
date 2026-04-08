// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月8日
// Update Time         :    2020年6月8日
// Class Description   :    ExpeditionMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using SprotoType;
using PureMVC.Interfaces;
using Data;
using Skyunion;
using Client;
using Hotfix;
using System;

namespace Game {

    public class SelectEnemyToMarchPreviewParam
    {
        public int PlayerTroopObjectId { get; set; }

        public int EnemyTroopObjectId { get; set; }
    }

    public class ExpeditionDrawLineInfo
    {
        public int objectId;
        public Vector2 targetPos;
    }

    public class ExpeditionMediator : GameMediator {
        #region Member

        public static string NameMediator = "ExpeditionMediator";

        #endregion

        #region 多部队行军

        private bool m_bDragMove = false;
        private int m_doubleSelectMainObjectId = 0;
        private List<int> m_doubleSelectViceObjectIdList;
        private List<int> m_doubleSelectObjectIdList;
        private bool m_doubleFlag = false;
        private int m_lastClickObjectId = 0;
        private long m_lastClickObjectTime = 0;

        #endregion

        private class WorldCameraParam
        {
            public double WorldMinX { get; set; }
            public double WorldMaxX { get; set; }
            public double WorldMinY { get; set; }
            public double WorldMaxY { get; set; }
            public float MinDxf { get; set; }
            public float MaxDxf { get; set; }
            public float CurDxf { get; set; }
            public Vector2 ViewCenter { get; set; }
        }

        //IMediatorPlug needs
        public ExpeditionMediator():base(NameMediator, null ) {}

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.EnterExpeditionMap,
                CmdConstant.ExitExpeditionMap,
                CmdConstant.ExpeditionTroopRemove,
                CmdConstant.ExpeditionFightFinish,
                CmdConstant.RetryExpedtionFight,
                CmdConstant.ExpeditionFightStart,
                CmdConstant.ExpeditionPrepareTroopChanage,
                CmdConstant.NetWorkReconnecting,
                CmdConstant.ExpeditionSelectTroop,
                CmdConstant.ExpeditionUISelectTroop,
                CmdConstant.MapRemoveSelectEffect,
                CmdConstant.DoubleTouchTroopSelect,
                CmdConstant.ExpeditionCreateDrawLine,
                CmdConstant.ExpeditionDeleteDrawLine,
                CmdConstant.ExpeditionRemoveAllDrawLine,
                CmdConstant.ExpeditionCreateSelectMyTroopEffect,
                CmdConstant.ExpeditionDeleteSelectMyTroopEffect,
                CmdConstant.ExpeditionRemoveAllSelectMyTroopEffect,
                CmdConstant.ExpeditionTroopHudMapMarCh,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.EnterExpeditionMap:
                    EnterExpeditionMap(notification);
                    break;
                case CmdConstant.ExitExpeditionMap:
                    ExitExpeditionMap();
                    break;
                case CmdConstant.ExpeditionTroopRemove:
                    OnExpeditionTroopRemove(notification.Body as ArmyData);
                    break;
                case CmdConstant.ExpeditionFightFinish:
                    ExpeditionFightFinished();
                    break;
                case CmdConstant.RetryExpedtionFight:
                    PrepareExpedtionFight();
                    break;
                case CmdConstant.ExpeditionFightStart:
                    OnFightStart();
                    break;
                case CmdConstant.ExpeditionPrepareTroopChanage:
                    OnPreparePlayerTroopChanged(notification);
                    break;
                case CmdConstant.NetWorkReconnecting:
                    OnNetworkReconnecting();
                    break;
                case CmdConstant.ExpeditionSelectTroop:
                    int troopId = (int)notification.Body;
                    OnUIDragTroopMove(troopId);
                    break;
                case CmdConstant.ExpeditionUISelectTroop:
                    OnUISelectTroop((int)notification.Body);
                    break;
                case CmdConstant.MapRemoveSelectEffect:
                    removeAllSelectMyTroopEffect();
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseSelectMainTroop);
                    break;
                case CmdConstant.DoubleTouchTroopSelect:
                    OnUIDoubleClick((int)notification.Body);
                    break;
                case CmdConstant.ExpeditionCreateDrawLine:
                    ExpeditionDrawLineInfo drawLineInfo = notification.Body as ExpeditionDrawLineInfo;
                    CreateDrawLine(drawLineInfo.objectId, drawLineInfo.targetPos);
                    break;
                case CmdConstant.ExpeditionDeleteDrawLine:
                    DeleteDrawLine((int)notification.Body);
                    break;
                case CmdConstant.ExpeditionRemoveAllDrawLine:
                    RemoveAllDraw();
                    break;
                case CmdConstant.ExpeditionCreateSelectMyTroopEffect:
                    CreateSelectMyTroopEffect((int)notification.Body);
                    break;
                case CmdConstant.ExpeditionDeleteSelectMyTroopEffect:
                    DestroySelectMyTroopEffect((int)notification.Body);
                    break;
                case CmdConstant.ExpeditionRemoveAllSelectMyTroopEffect:
                    removeAllSelectMyTroopEffect();
                    break;
                case CmdConstant.ExpeditionTroopHudMapMarCh:
                    ExpeditionTroopHudStation((int)notification.Body);
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            InitTouchObjectHandler();
        }

        protected override void BindUIEvent()
        {
            CoreUtils.inputManager.AddTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
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

        public override void OnRemove()
        {
            CoreUtils.inputManager.RemoveTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
            ExpeditionFightFinished();
            DestroyCreatePlayerTroopOnGround();
            if (m_checkTwoTouchTimer != null)
            {
                m_checkTwoTouchTimer.Cancel();
                m_checkTwoTouchTimer = null;
            }
        }

        #endregion

        #region 双击逻辑

        private bool OnWorldDoubleClick(int objectId)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null)
            {
                return false;
            }

            if (!armyData.isPlayerHave)
            {
                return false;
            }

            if (armyData.isRally)
            {
                return false;
            }

            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            float multiArmyDoubleClickTime = configDefine.moretTroopsClick * 1000;

            long localTime = ServerTimeModule.Instance.GetTicks() / 10000;
            long timeDif = localTime - m_lastClickObjectTime;            
            if (m_lastClickObjectId == objectId &&
                timeDif >= 0 &&
                timeDif <= multiArmyDoubleClickTime)
            {
                List<int> selectViceObjectIdList = new List<int>();
                SummonerTroopMgr.Instance.ExpeditionTroop.CalScreenViceArmList(objectId, ref selectViceObjectIdList);
                foreach (var id in selectViceObjectIdList)
                {
                    CreateSelectMyTroopEffect(id);
                }

                m_doubleSelectMainObjectId = objectId;
                m_doubleSelectViceObjectIdList = selectViceObjectIdList;

                if (m_doubleSelectObjectIdList != null)
                {
                    m_doubleSelectObjectIdList.Clear();
                }
                else
                {
                    m_doubleSelectObjectIdList = new List<int>();
                }

                m_doubleSelectObjectIdList.Add(objectId);
                foreach (var viceObjectId in selectViceObjectIdList)
                {
                    m_doubleSelectObjectIdList.Add(viceObjectId);
                }

                m_doubleFlag = true;
                m_lastClickObjectId = 0;
                m_lastClickObjectTime = 0;

                TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                touchTroopInfo.mainArmObjectId = objectId;
                touchTroopInfo.viceArmObjectIdList = selectViceObjectIdList;

                AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
                AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionUIDoubleSelect, m_doubleSelectObjectIdList);

                return true;
            }
            else
            {
                m_lastClickObjectId = objectId;
                m_lastClickObjectTime = localTime;

                return false;
            }
        }

        private void OnUIDoubleClick(int objectId)
        {
            ArmyData clickArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (clickArmyData == null)
            {
                return;
            }

            List<int> selectViceObjectIdList = new List<int>();

            SummonerTroopMgr.Instance.ExpeditionTroop.CalWorldViceArmList(objectId, ref selectViceObjectIdList);

            CreateSelectMyTroopEffect(objectId);
            foreach (var id in selectViceObjectIdList)
            {
                CreateSelectMyTroopEffect(id);
            }

            m_doubleSelectMainObjectId = objectId;
            m_doubleSelectViceObjectIdList = selectViceObjectIdList;

            if (m_doubleSelectObjectIdList != null)
            {
                m_doubleSelectObjectIdList.Clear();
            }
            else
            {
                m_doubleSelectObjectIdList = new List<int>();
            }

            m_doubleSelectObjectIdList.Add(objectId);
            foreach (var viceObjectId in selectViceObjectIdList)
            {
                m_doubleSelectObjectIdList.Add(viceObjectId);
            }

            TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
            touchTroopInfo.mainArmObjectId = clickArmyData.objectId;
            touchTroopInfo.viceArmObjectIdList = selectViceObjectIdList;

            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo); 
            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionUIDoubleSelect, m_doubleSelectObjectIdList);
        }

        private void CheckExitDoubleClick()
        {
            if (m_doubleFlag == true)
            {
                m_doubleFlag = false;
            }
            else
            {
                if (m_doubleSelectMainObjectId != 0 &&
                    m_doubleSelectViceObjectIdList != null &&
                    m_doubleSelectObjectIdList != null)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseSelectMainTroop);

                    removeAllSelectMyTroopEffect();
                    if (!m_bDragMove)
                    {
                        CreateSelectMyTroopEffect(m_selectObjectId);
                    }

                    AppFacade.GetInstance().SendNotification(CmdConstant.MapCloseDoubleSelectTroopHud);
                    if (!m_bDragMove)
                    {
                        TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                        touchTroopInfo.mainArmObjectId = m_selectObjectId;

                        AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
                    }

                    DeleteAllViceMoveLine();

                    m_doubleSelectMainObjectId = 0;
                    m_doubleSelectViceObjectIdList.Clear();
                    m_doubleSelectViceObjectIdList = null;
                    m_doubleSelectObjectIdList.Clear();
                    m_doubleSelectObjectIdList = null;
                }
            }
        }

        #endregion

        private void ExpeditionTroopHudStation(int objectId)
        {
            m_expeditionProxy.TroopStation(objectId, m_doubleSelectObjectIdList);
        }

        private void OnNetworkReconnecting()
        {
            if(m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.None)
            {
                ExitExpeditionMap();
            }
        }

        private void OnTouchBegan(int x, int y)
        {
            if (GameModeManager.Instance.CurGameMode != GameModeType.Expedition) return;
            m_touchBeginColliderName = string.Empty;
            Ray ray = WorldCamera.Instance().GetCamera().ScreenPointToRay(new Vector3(x, y, 0));
            RaycastHit rayHit;
            if (!Physics.Raycast(ray, out rayHit, 1000))
            {
                return;
            }
            m_touchBeginColliderName = rayHit.transform.gameObject.name;
            foreach (var handler in m_onTouchObjectBeginHandlers)
            {
                if (m_touchBeginColliderName.Contains(handler.Key))
                {
                    handler.Value?.Invoke(m_touchBeginColliderName);
                    break;
                }
            }
        }

        private void OnTouchMoved(int x, int y)
        {
            if (GameModeManager.Instance.CurGameMode != GameModeType.Expedition) return;

            m_bDragMove = true;

            if (!string.IsNullOrEmpty(m_touchBeginColliderName) && m_selectObjectId != 0)
            {
                if (m_checkTwoTouchTimer != null)
                {
                    m_checkTwoTouchTimer.Cancel();
                    m_checkTwoTouchTimer = null;
                }
                m_checkTwoTouchTimer = Timer.Register(0.1f, null, (float dt) =>
                {
                    // 拖拽过程中右键，取消拖拽或者多指操作取消操作
                    if (CoreUtils.inputManager.GetTouchCount() > 1)
                    {
                        ClearTouchData();
                        return;
                    }
                }, true);

                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(m_selectObjectId);
                if (armyData != null && armyData.isPlayerHave)
                {
                    UpdateTroopPreviewMvoe(x, y);
                }

                Ray ray = WorldCamera.Instance().GetCamera().ScreenPointToRay(new Vector2(x, y));
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, 1000))
                {
                    string colliderName = rayHit.transform.gameObject.name;
                    foreach (var kv in m_onTouchMoveHitObjectHandlers)
                    {
                        if (colliderName.Contains(kv.Key))
                        {
                            kv.Value?.Invoke(colliderName);
                            break;
                        }
                   }
                }
                else
                {
                    DestroyEnemySelectEffect();
                }
            }
        }

        private void OnTouchEnded(int x, int y)
        {
            if (GameModeManager.Instance.CurGameMode != GameModeType.Expedition) return;

            if (m_checkTwoTouchTimer != null)
            {
                m_checkTwoTouchTimer.Cancel();
                m_checkTwoTouchTimer = null;
            }

            Ray ray = WorldCamera.Instance().GetCamera().ScreenPointToRay(new Vector3(x, y, 0));
            RaycastHit rayHit;
            if (!Physics.Raycast(ray, out rayHit, 1000))
            {
                if(!string.IsNullOrEmpty(m_touchBeginColliderName))
                {
                    PlayerTroopTryMarchTo(0, x, y);
                }
                ClearTouchData();
                return;
            }

            string colliderName = rayHit.collider.gameObject.name;
            if(colliderName.Equals(m_touchBeginColliderName))
            {
                foreach (var handler in m_onTouchObjectHandlers)
                {
                    if (m_touchBeginColliderName.Contains(handler.Key))
                    {
                        handler.Value?.Invoke(m_touchBeginColliderName);
                        break;
                    }
                }
            }
            else
            {
                int objectId = TroopHelp.GetTroopObjectIdByColliderName(colliderName);
                PlayerTroopTryMarchTo(objectId, x, y);
                ClearTouchData();
                return;
            }

            if (string.IsNullOrEmpty(m_touchBeginColliderName))
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseSelectMainTroop);
            }

            ClearTouchMoveData();
            CheckExitDoubleClick();

            m_bDragMove = false;
        }

        private void PlayerTroopTryMarchTo(int objectId, int x, int y)
        {
            if (m_selectObjectId == 0) return;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(m_selectObjectId);
            if (armyData == null || !armyData.isPlayerHave) return;
            if(objectId != 0)
            {
                if(objectId != m_selectObjectId)
                {
                    ArmyData touchEndArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
                    if (touchEndArmyData != null)
                    {
                        if (!touchEndArmyData.isPlayerHave)
                        {
                            m_expeditionProxy.TroopMarchToEnemy(m_selectObjectId, objectId, m_doubleSelectObjectIdList);
                        }
                        else
                        {
                            Vector3 v3 = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
                            m_expeditionProxy.TroopMarchToSpace(m_selectObjectId, v3, m_doubleSelectObjectIdList);
                        }
                    }
                }                
            }
            else
            {
                Vector3 v3 = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
                m_expeditionProxy.TroopMarchToSpace(m_selectObjectId, v3, m_doubleSelectObjectIdList);
            }
        }

        private void InitTouchObjectHandler()
        {
            m_onTouchObjectHandlers.Add("expedition_preview_enemy", OnTouchPreviewEnemyTroop);
            m_onTouchObjectHandlers.Add("expedition_preview_player", OnTouchPreviewPlayerTroop);
            m_onTouchObjectHandlers.Add("build_create_troop_on_ground", OnTouchCreatePlayerTroopObject);
            m_onTouchObjectHandlers.Add("Formation", OnTouchTroop);

            m_onTouchObjectBeginHandlers.Add("Formation", OnTouchTroopBegin);

            m_onTouchMoveHitObjectHandlers.Add("Formation", OnTouchMoveHitTroop);
        }

        private void ClearTouchData()
        {
            if (m_checkTwoTouchTimer != null)
            {
                m_checkTwoTouchTimer.Cancel();
                m_checkTwoTouchTimer = null;
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseSelectMainTroop);

            ClearTouchMoveData();            
            ClearSelectTroop();
            ClearPreviewSelectEnemy();
            CheckExitDoubleClick();
        }

        private void ClearTouchMoveData()
        {
            DestroyEnemySelectEffect();
            DestroyTroopPreviewMoveLine();
            DeleteAllViceMoveLine();
            ReleaseSelectTroopMoveTimer();
            SetDragTroopCamaraOffset(0);
            WorldCamera.Instance().SetCanDrag(true);
            WorldCamera.Instance().enableReboundXY = true;
        }

        private void SetDragTroopCamaraOffset(float offset)
        {
            var battleCfg = m_expeditionProxy.ExpeditionBattleCfg;
            Vector2 initPos = m_expeditionProxy.ExpeditionPosToWorldPos(battleCfg.initialCamera[0], battleCfg.initialCamera[1]);
            WorldCamera.Instance().worldMinX = initPos.x - battleCfg.deviationX + offset;
            WorldCamera.Instance().worldMaxX = initPos.x + battleCfg.deviationX - offset;
            WorldCamera.Instance().worldMinY = initPos.y - battleCfg.deviationY + offset;
            WorldCamera.Instance().worldMaxY = initPos.y + battleCfg.deviationY - offset;
        }

        private void ClearPreviewSelectEnemy()
        {
            if (m_previewSelectEnemyTroopIndex == 0) return;
            m_previewSelectEnemyTroopIndex = 0;
        }

        private void OnExpeditionTroopRemove(ArmyData armyData)
        {
            if (armyData == null) return;
            int id = (int)armyData.objectId;
            if(id == m_selectObjectId)
            {
                DestroyEnemySelectEffect();
                DestroyTroopPreviewMoveLine();
                ClearSelectTroop();
            }
            else
            {
                DeleteViceMoveLine(id);
            }
        }

        private void OnTouchMoveHitTroop(string colliderName)
        {
            int objectId = TroopHelp.GetTroopObjectIdByColliderName(colliderName);
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData != null && !armyData.isPlayerHave)
            {
                CreateEnemySelectEffect(armyData.objectId);
            }
        }

        private int GetTouchPreviewObjectId(int objectIdIndexInStr, string colliderName)
        {
            string[] strP = colliderName.Split('_');
            if (strP.Length <= objectIdIndexInStr) return 0;
            int objectId = 0;
            int.TryParse(strP[objectIdIndexInStr], out objectId);
            return objectId;
        }

        private void OnTouchPreviewEnemyTroop(string colliderName)
        {
            if(m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PreparePreview)
            {
                return;
            }
            int enemyTroopId = GetTouchPreviewObjectId(3, colliderName);
            if (enemyTroopId == 0) return;
            m_previewSelectEnemyTroopIndex = enemyTroopId;
            Troops formation = SummonerTroopMgr.Instance.ExpeditionTroop.GetPreviewMonsterFormation(m_previewSelectEnemyTroopIndex);
            CoreUtils.uiManager.ShowUI(UI.s_battleTroopsTips, null, new BattleTroopsTipsData()
            {
                ScreenPosition = WorldCamera.Instance().GetCamera().WorldToScreenPoint(formation.transform.position),
                EnemyTroopIndex = m_previewSelectEnemyTroopIndex,
            });
        }

        private void OnTouchPreviewPlayerTroop(string colliderName)
        {
            if (m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PreparePreview) return;
            int playerTroopId = GetTouchPreviewObjectId(3, colliderName);
            if (playerTroopId == 0) return;
            FightHelper.Instance.OpenCreateArmyPanel(new OpenPanelData(0, OpenPanelType.Common)
            {
                ExpeditionTroopIndex = playerTroopId,
            });
        }

        private void OnTouchCreatePlayerTroopObject(string colliderName)
        {
            if (m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PreparePreview) return;
            int playerTroopId = GetTouchPreviewObjectId(5, colliderName);
            if (playerTroopId == 0) return;
            FightHelper.Instance.OpenCreateArmyPanel(new OpenPanelData(0, OpenPanelType.Common)
            {
                ExpeditionTroopIndex = playerTroopId,
            });
        }

        private void OnTouchTroop(string colliderName)
        {
            int objectId = TroopHelp.GetTroopObjectIdByColliderName(colliderName);
            if (!OnWorldDoubleClick(objectId))
            {
                OnSelectTroop(objectId);
            }            
        }


        private void OnUIDragTroopMove(int objectId)
        {
            WorldCamera.Instance().SetCanDrag(false);

            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null) return;

            if (m_selectObjectId != armyData.objectId)
            {
                ClearSelectTroop();
            }
            m_selectObjectId = armyData.objectId;

            m_touchBeginColliderName = "Expeditiontroop";
        }

        private void OnUISelectTroop(int objectId)
        {
            OnSelectTroop(objectId);
            removeAllSelectMyTroopEffect();
            CreateSelectMyTroopEffect(objectId);
        }

        private void OnSelectTroop(int objectId)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null) return;
            if(m_selectObjectId != armyData.objectId)
            {
                ClearSelectTroop();
            }
            m_selectObjectId = armyData.objectId;

            TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
            touchTroopInfo.mainArmObjectId = armyData.objectId;

            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);
        }

        private void OnTouchTroopBegin(string colliderName)
        {
            int objectId = TroopHelp.GetTroopObjectIdByColliderName(colliderName);
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null || !armyData.isPlayerHave) return;
            m_selectObjectId = objectId;
            SetDragTroopCamaraOffset(m_expeditionProxy.ExpeditionBattleCfg.rangeDistance * 1.0f / 100);
            WorldCamera.Instance().SetCanDrag(false);
            WorldCamera.Instance().enableReboundXY = false;
            if (m_doubleSelectObjectIdList == null)
            {
                removeAllSelectMyTroopEffect();
                CreateSelectMyTroopEffect(objectId);
            }            
        }

        private void ClearSelectTroop()
        {
            if (m_selectObjectId == 0) return;
            removeAllSelectMyTroopEffect();
            DestroyEnemySelectEffect();
            AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveTroopSelectHud, m_selectObjectId);
            m_selectObjectId = 0;
        }

        private void OnFightStart()
        {
            ShowPreviewCreatePlayerTroopOnGround(false);
            if(m_expeditionProxy.ExpeditionCfg.level == 1 && !m_playerProxy.CurrentRoleInfo.expeditionInfo.ContainsKey(1))
            {
                ShowDragTroopGuide();
            }
            if (CoreUtils.audioService.GetCurBgmName() != ExpeditionProxy.BattleBgm)
            {
                CoreUtils.audioService.PlayBgm(ExpeditionProxy.BattleBgm);
            }
        }

        #region 远征拖动部队引导

        private void ShowDragTroopGuide()
        {
            CoreUtils.assetService.Instantiate(m_dragTroopGuideOnGroundName, (go) =>
            {
                if(m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.Fightting)
                {
                    CoreUtils.assetService.Destroy(go);
                    return;
                }
                m_dragTroopGuideOnGroundObject = go;
                ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                Vector2 worldPos = m_expeditionProxy.ExpeditionPosToWorldPos(config.guideTroopsMove[0], config.guideTroopsMove[1]);
                m_dragTroopGuideOnGroundObject.transform.position = new Vector3(worldPos.x, 0, worldPos.y);
                var troopData = m_expeditionProxy.GetPlayerTroopData(1);
                CoreUtils.uiManager.ShowUI(UI.s_finger2, null, new Finger2ViewData()
                {
                    BeginWorldPos = new Vector3(troopData.BornPosisiton.x, 0, troopData.BornPosisiton.y),
                    EndWorldPos = m_dragTroopGuideOnGroundObject.transform.position,
                });
                m_checkTroopMovedForGuideTimer = Timer.Register(0, null, (time) =>
                {
                    var armyData = SummonerTroopMgr.Instance.ExpeditionTroop.GetArmyData(true, 1);
                    if(armyData != null && armyData.state == Troops.ENMU_SQUARE_STAT.MOVE)
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_finger2);
                        if (m_checkTroopMovedForGuideTimer != null)
                        {
                            m_checkTroopMovedForGuideTimer.Cancel();
                            m_checkTroopMovedForGuideTimer = null;
                        }
                    }
                   
                }, true);
                m_checkTroopMoveToPosForGuideTimer = Timer.Register(0, null, (time) =>
                {
                    var armyData = SummonerTroopMgr.Instance.ExpeditionTroop.GetArmyData(true, 1);
                    if(armyData != null && armyData.go != null)
                    {
                        BoxCollider collider1 = armyData.go.GetComponentInChildren<BoxCollider>();
                        BoxCollider collider2 = m_dragTroopGuideOnGroundObject.GetComponentInChildren<BoxCollider>();
                        if(collider1 != null && collider2 != null && collider1.bounds.Intersects(collider2.bounds))
                        {
                            ClearDragTroop();
                        }
                    }
                }, true);
            });
        }

        private void ClearDragTroop()
        {
            if(m_dragTroopGuideOnGroundObject != null)
            {
                CoreUtils.assetService.Destroy(m_dragTroopGuideOnGroundObject);
                m_dragTroopGuideOnGroundObject = null;
                CoreUtils.uiManager.CloseUI(UI.s_finger2);
            }
            if(m_checkTroopMovedForGuideTimer != null)
            {
                m_checkTroopMovedForGuideTimer.Cancel();
                m_checkTroopMovedForGuideTimer = null;
            }
            if(m_checkTroopMoveToPosForGuideTimer != null)
            {
                m_checkTroopMoveToPosForGuideTimer.Cancel();
                m_checkTroopMoveToPosForGuideTimer = null;
            }
        }

        #endregion

        #region ExpeditionMap
        private void EnterExpeditionMap(INotification notification)
        { 
            ExpeditionDefine cfg = notification.Body as ExpeditionDefine;
            if (cfg == null) return;
            ExpeditionBattleDefine battleCfg = CoreUtils.dataService.QueryRecord<ExpeditionBattleDefine>(cfg.battleID);
            if (battleCfg == null) return;
            GameModeManager.Instance.ChangeMode(GameModeType.Expedition);
            string mapDataName = $"{battleCfg.mapID}_data" ;         
            MapManager.Instance().ClearMapTileBrief();
            WorldCamera.Instance().enableReboundXY = true;
            AppFacade.GetInstance().SendNotification(CmdConstant.SetFogBorderVisible, false);
            MapManager.Instance().ReadMapBriefDataFromFile2(mapDataName, 0, 3, () =>
            {
                m_expeditionProxy.SetMapRange(0, 7199, 21600, 28799); 
                StoreWorldCameraParam();
                CoreUtils.uiManager.CloseUI(UI.s_expeditionFight);
                InitExpeditionScene(cfg, battleCfg);               
            });
        }

        private void ExitExpeditionMap()
        {
            m_expeditionProxy.ExpeditionStatus = ExpeditionFightStatus.None;
            ExpeditionFightFinished();
            DestroyCreatePlayerTroopOnGround();
            m_expeditionProxy.ClearAllData();
            MapManager.Instance().ClearMapTileBrief();
            AppFacade.GetInstance().SendNotification(CmdConstant.SetFogBorderVisible, true);
            ClientUtils.mapManager.ReadMapBriefDataFromFile2("map_4_data", 0, 0, () =>
            {
                RestoreWorldCameraParam();
                CoreUtils.uiManager.ShowUI(UI.s_expeditionFight);
                GameModeManager.Instance.ChangeMode(GameModeType.World);
            });           
        }

        private void ExpeditionFightFinished()
        {
            ReleaseSelectTroopMoveTimer();
            removeAllSelectMyTroopEffect();
            DestroyEnemySelectEffect();
            DestroyTroopPreviewMoveLine();
            DeleteAllViceMoveLine();
            ClearDragTroop();
            SummonerTroopMgr.Instance.ExpeditionTroop.Clear();
        }

        private void PrepareExpedtionFight()
        {
            SetWorldCameraParam(CreateExpeditionCameraParam(m_expeditionProxy.ExpeditionBattleCfg));
            ClearDragTroop();
            m_expeditionProxy.ExpeditionStatus = ExpeditionFightStatus.PrepareNormal;
            InitEnemySelectEffect();
            ShowPreviewCreatePlayerTroopOnGround(true);
            var allMonsterTroopData = m_expeditionProxy.GetAllMonsterTroopData();
            foreach (var data in allMonsterTroopData)
            {
                SummonerTroopMgr.Instance.ExpeditionTroop.CreatePreviewMonsterFormation(data);
            }
            CoreUtils.uiManager.ShowUI(UI.s_expeditionFightTask, null, new ExpeditionFightTaskViewData()
            {
                ExpeditionCfg = m_expeditionProxy.ExpeditionCfg,
                ExpeditionBattleCfg = m_expeditionProxy.ExpeditionBattleCfg,
            });
        }

        private void InitExpeditionScene(ExpeditionDefine cfg, ExpeditionBattleDefine battleCfg)
        {
            m_expeditionProxy.InitData(cfg, battleCfg);
            CreatePreviewCreatePlayerTroopOnGround(cfg, () =>
            {
                PrepareExpedtionFight();
            });           
        }

        private WorldCameraParam CreateExpeditionCameraParam(ExpeditionBattleDefine battleCfg)
        {
            Vector2 initPos = m_expeditionProxy.ExpeditionPosToWorldPos(battleCfg.initialCamera[0], battleCfg.initialCamera[1]);
            WorldCameraParam param = new WorldCameraParam()
            {
                WorldMinX = initPos.x - battleCfg.deviationX,
                WorldMaxX = initPos.x + battleCfg.deviationX,
                WorldMinY = initPos.y - battleCfg.deviationY,
                WorldMaxY = initPos.y + battleCfg.deviationY,
                MinDxf = WorldCamera.Instance().getCameraDxf("showbuild") + 1,
                MaxDxf = 2700,
                CurDxf = WorldCamera.Instance().getCameraDxf("init"),
                ViewCenter = initPos,
            };
            return param;
        }    

        private void SetWorldCameraParam(WorldCameraParam param)
        {
            WorldCamera.Instance().worldMinX = param.WorldMinX;
            WorldCamera.Instance().worldMaxX = param.WorldMaxX;
            WorldCamera.Instance().worldMinY = param.WorldMinY;
            WorldCamera.Instance().worldMaxY = param.WorldMaxY;
            WorldCamera.Instance().customMinDxf = param.MinDxf;
            WorldCamera.Instance().customMaxDxf = param.MaxDxf;
            WorldCamera.Instance().SetCameraDxf(param.CurDxf, WorldCamera.INVALID_FLOAT_VALUE, null);
            WorldCamera.Instance().ViewTerrainPos(param.ViewCenter.x, param.ViewCenter.y, WorldCamera.INVALID_FLOAT_VALUE, null);
        }

        private void StoreWorldCameraParam()
        {
            if(m_storeWorldCameraParam == null)
            {
                m_storeWorldCameraParam = new WorldCameraParam();
            }
            m_storeWorldCameraParam.WorldMinX = WorldCamera.Instance().worldMinX;
            m_storeWorldCameraParam.WorldMaxX = WorldCamera.Instance().worldMaxX;
            m_storeWorldCameraParam.WorldMinY = WorldCamera.Instance().worldMinY;
            m_storeWorldCameraParam.WorldMaxY = WorldCamera.Instance().worldMaxY;
            m_storeWorldCameraParam.MinDxf = WorldCamera.Instance().customMinDxf;
            m_storeWorldCameraParam.MaxDxf = WorldCamera.Instance().customMaxDxf;
            m_storeWorldCameraParam.CurDxf = WorldCamera.Instance().getCameraDxf("map_tactical");
            m_storeWorldCameraParam.ViewCenter = PosHelper.ServerUnitToClientUnit_v2(m_playerProxy.CurrentRoleInfo.pos);
        }

        private void RestoreWorldCameraParam()
        {
            if(m_storeWorldCameraParam != null)
            {
                SetWorldCameraParam(m_storeWorldCameraParam);
            }
        }

        #endregion

        #region preview create player troop on ground
        private void OnPreparePlayerTroopChanged(INotification notification)
        {
            int index = (int)notification.Body;
            if (index - 1 >= m_previewCreatePlayerOnGroundObject.Count) return;
            m_previewCreatePlayerOnGroundObject[index - 1].gameObject.SetActive(m_expeditionProxy.GetPlayerTroopData(index) == null);
        }

        private void CreatePreviewCreatePlayerTroopOnGround(ExpeditionDefine cfg, Action callback)
        {
            CoreUtils.assetService.LoadAssetAsync<GameObject>(m_previewCreateTroopOnGroundName, (asset) =>
            {
                if (asset == null || asset.asset() == null)
                {
                    callback?.Invoke();
                    return;
                }
                var prefab = asset.asset() as GameObject;
                for(int i = 0; i < m_expeditionProxy.ExpeditionCfg.troopsNumber; ++i)
                {
                    int x= 0,  y = 0, z = 0;
                    m_expeditionProxy.GetPlayerTroopInitParam(i + 1, ref x, ref y, ref z);
                    var obj = CoreUtils.assetService.Instantiate(prefab);
                    asset.Attack(obj);
                    obj.transform.SetParent(GetMapTroopsRoot());
                    obj.transform.localScale = Vector3.one;
                    obj.name = $"build_create_troop_on_ground_{i+1}";
                    Vector2 pos =  m_expeditionProxy.ExpeditionPosToWorldPos(x, y);
                    obj.transform.position = new Vector3(pos.x, 0, pos.y);
                    m_previewCreatePlayerOnGroundObject.Add(obj);
                }
                callback?.Invoke();
            }, null);
        }

        private Transform GetMapTroopsRoot()
        {
            WorldMgrMediator worldMgrMediator = AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            if (worldMgrMediator != null)
            {
                return worldMgrMediator.GetTroopsRoot();
            }
            return null;
        }

        private void ShowPreviewCreatePlayerTroopOnGround(bool isShow)
        {
            foreach (var obj in m_previewCreatePlayerOnGroundObject)
            {
                obj.SetActive(isShow);
            }
        }


        private void DestroyCreatePlayerTroopOnGround()
        {
            foreach (var obj in m_previewCreatePlayerOnGroundObject)
            {
                CoreUtils.assetService.Destroy(obj);
            }
            m_previewCreatePlayerOnGroundObject.Clear();
        }

        #endregion

        #region 我的部队光效管理

        Dictionary<int, MapObjectSelectEffect> selectMyTroopEffectDic = new Dictionary<int, MapObjectSelectEffect>();

        public void CreateSelectMyTroopEffect(int objectId)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null || armyData.go == null) return;

            MapObjectSelectEffect mapObjectSelectEffect = null;
            if (!selectMyTroopEffectDic.ContainsKey(objectId))
            {
                mapObjectSelectEffect = new MapObjectSelectEffect(RS.TroopSelectEffectName);
                selectMyTroopEffectDic.Add(objectId, mapObjectSelectEffect);
            }
            else
            {
                mapObjectSelectEffect = selectMyTroopEffectDic[objectId];
            }

            if (mapObjectSelectEffect == null) return;

            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            float scale = armyData.armyRadius / configDefine.mapTargetEffectScale;

            mapObjectSelectEffect.AttachTransform(armyData.go.transform, scale);
        }

        private void DestroySelectMyTroopEffect(int objectId)
        {
            if (selectMyTroopEffectDic.ContainsKey(objectId))
            {
                MapObjectSelectEffect mapObjectSelectEffect = selectMyTroopEffectDic[objectId];
                if (mapObjectSelectEffect != null)
                {
                    mapObjectSelectEffect.Destroy();
                }

                selectMyTroopEffectDic.Remove(objectId);
            }
        }

        public void removeAllSelectMyTroopEffect()
        {
            foreach (var mapObjectSelectEffect in selectMyTroopEffectDic.Values)
            {
                if (mapObjectSelectEffect != null)
                {
                    mapObjectSelectEffect.Destroy();
                }
            }
            selectMyTroopEffectDic.Clear();
        }

        #endregion

        #region 敌方部队光效管理

        private void InitEnemySelectEffect()
        {
            if (m_enemyTroopSelectEffect == null)
            {
                m_enemyTroopSelectEffect = new MapObjectSelectEffect(RS.EnemySelectEffectName);
            }
        }

        public void CreateEnemySelectEffect(int objectId)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null || armyData.go == null) return;

            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            float scale = armyData.armyRadius / config.mapTargetEffectScale;

            if (m_enemyTroopSelectEffect == null)
            {
                m_enemyTroopSelectEffect = new MapObjectSelectEffect(RS.EnemySelectEffectName);
            }
            m_enemyTroopSelectEffect.AttachTransform(armyData.go.transform, scale);
        }

        private void DestroyEnemySelectEffect()
        {
            if(m_enemyTroopSelectEffect != null)
            {
                m_enemyTroopSelectEffect.Destroy();
                m_enemyTroopSelectEffect = null;
            }
        }
        #endregion

        #region 部队选中移动预览

        private Dictionary<int, TroopLineInfo> m_drawLineDic = new Dictionary<int, TroopLineInfo>();
        private Dictionary<int, Vector2> m_drawLineTargetPosDic = new Dictionary<int, Vector2>();
        private Timer m_drawLineTimer = null;

        private void RemoveAllDraw()
        {
            foreach (var drawLine in m_drawLineDic.Values)
            {
                if (drawLine != null)
                {
                    drawLine.Destroy();
                }
            }
            m_drawLineDic.Clear();

            m_drawLineTargetPosDic.Clear();

            if (m_drawLineTimer != null)
            {
                Timer.Cancel(m_drawLineTimer);
                m_drawLineTimer = null;
            }
        }

        private void CreateDrawLine(int objectId, Vector2 targetPos)
        {
            if (targetPos.Equals(Vector2.zero)) return;

            if (!m_drawLineDic.ContainsKey(objectId))
            {
                TroopLineInfo drawLine = new TroopLineInfo();
                drawLine.LoadTroopLine(false, "troop_line_drag_move");
                drawLine.ChangeLineColor(RS.white);

                m_drawLineDic.Add(objectId, drawLine);
            }

            if (!m_drawLineTargetPosDic.ContainsKey(objectId))
            {
                m_drawLineTargetPosDic.Add(objectId, targetPos);
            }
            else
            {
                m_drawLineTargetPosDic[objectId] = targetPos;
            }

            SetDrawLine(objectId);

            if (m_drawLineDic.Count > 0 && m_drawLineTargetPosDic.Count > 0)
            {
                if (m_drawLineTimer == null)
                {
                    m_drawLineTimer = Timer.Register(0.05f, null, RefreshDrawLine, true);
                }
            }
        }

        private void DeleteDrawLine(int objectId)
        {
            if (m_drawLineDic.ContainsKey(objectId))
            {
                m_drawLineDic[objectId].Destroy();
                m_drawLineDic.Remove(objectId);
            }

            if (m_drawLineTargetPosDic.ContainsKey(objectId))
            {
                m_drawLineTargetPosDic.Remove(objectId);
            }

            if (m_drawLineDic.Count <= 0 && m_drawLineTargetPosDic.Count <= 0)
            {
                if (m_drawLineTimer != null)
                {
                    Timer.Cancel(m_drawLineTimer);
                    m_drawLineTimer = null;
                }
            }
        }

        private void SetDrawLine(int objectId)
        {
            if (!m_drawLineDic.ContainsKey(objectId)) return;

            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData == null || armyData.go == null) return;

            Vector2 targetPos = Vector2.zero;
            if (m_drawLineTargetPosDic.ContainsKey(objectId))
            {
                targetPos = m_drawLineTargetPosDic[objectId];
            }

            var paths = new Vector2[2];
            paths[0].x = armyData.go.transform.position.x;
            paths[0].y = armyData.go.transform.position.z;
            paths[1].x = targetPos.x;
            paths[1].y = targetPos.y;

            m_drawLineDic[objectId].SetTroopLinePath(paths);
        }

        private void RefreshDrawLine(float dt)
        {
            foreach (var armyIndex in m_drawLineDic.Keys)
            {
                SetDrawLine(armyIndex);
            }
        }

        private Dictionary<int, TroopLineInfo> m_viceMoveLineDic = new Dictionary<int, TroopLineInfo>();

        private void UpdateTroopPreviewMvoe(int x, int y)
        {           
            if (x > Screen.width * 0.9 || x < Screen.width * 0.1 || y > Screen.height * 0.9 || y < Screen.height * 0.1)
            {
                m_curMovePos = new Vector2Int(x, y);
                if (m_selectTroopMoveTimer == null)
                {
                    m_selectTroopMoveTimer = Timer.Register(0.05f, null, (deltaTime) =>
                    {
                        var offset = m_curMovePos - new Vector2(Screen.width / 2, Screen.height / 2);
                        offset.Normalize();
                        var center = WorldCamera.Instance().GetViewCenter();
                        center += offset * deltaTime * WorldCamera.Instance().getCurrentCameraDist() / 5.0f;
                        WorldCamera.Instance().ViewTerrainPos(center.x, center.y, WorldCamera.INVALID_FLOAT_VALUE, null);
                        UpdateTroopPreviewMoveLine(m_curMovePos.x, m_curMovePos.y);

                    }, true);
                    UpdateTroopPreviewMoveLine(x, y);
                }
            }
            else
            {
                ReleaseSelectTroopMoveTimer();
                UpdateTroopPreviewMoveLine(x, y);
            }
        }

        private void ReleaseSelectTroopMoveTimer()
        {
            if (m_selectTroopMoveTimer != null)
            {
                m_selectTroopMoveTimer.Cancel();
                m_selectTroopMoveTimer = null;
            }
        }

        private void UpdateTroopPreviewMoveLine(int x, int y)
        {
            ArmyData mainArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(m_selectObjectId);
            if (mainArmyData == null || mainArmyData.go == null) return;

            var paths = new Vector2[2];
            var pos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
            paths[0].x = mainArmyData.go.transform.position.x;
            paths[0].y = mainArmyData.go.transform.position.z;
            paths[1].x = pos.x;
            paths[1].y = pos.z;

            SetTroopPreveiwMovePath(paths);

            if (m_doubleSelectObjectIdList != null)
            {
                foreach (var objectId in m_doubleSelectObjectIdList)
                {
                    ArmyData viceArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
                    if (viceArmyData == null || viceArmyData.go == null) continue;

                    SetViceMoveLine(objectId, viceArmyData.go.transform.position.x, viceArmyData.go.transform.position.z, pos.x, pos.z);
                }
            }
        }

        private void SetTroopPreveiwMovePath(Vector2[] path)
        {
            if(m_troopMovePreviewLine == null)
            {
                m_troopMovePreviewLine = new TroopLineInfo();
                m_troopMovePreviewLine.LoadTroopLine(false, "troop_line_drag_move");
                m_troopMovePreviewLine.ChangeLineColor(Color.white);
            }
            if(!m_troopMovePreviewLine.IsActive)
            {
                m_troopMovePreviewLine.SetActive(true);
            }
            m_troopMovePreviewLine.SetTroopLinePath(path);
        }
        
        private void DestroyTroopPreviewMoveLine()
        {
            if(m_troopMovePreviewLine != null)
            {
                m_troopMovePreviewLine.Destroy();
                m_troopMovePreviewLine = null;
            }
        }

        private void SetViceMoveLine(int objectId, float start_x, float start_y, float end_x, float end_y)
        {
            if (!m_viceMoveLineDic.ContainsKey(objectId))
            {
                TroopLineInfo moveLine = new TroopLineInfo();
                moveLine.LoadTroopLine(false, "troop_line_drag_move");
                moveLine.ChangeLineColor(RS.white);

                m_viceMoveLineDic.Add(objectId, moveLine);
            }

            var paths = new Vector2[2];

            paths[0].x = start_x;
            paths[0].y = start_y;
            paths[1].x = end_x;
            paths[1].y = end_y;

            m_viceMoveLineDic[objectId].SetTroopLinePath(paths);
        }

        private void DeleteViceMoveLine(int objectId)
        {
            if (m_viceMoveLineDic.ContainsKey(objectId))
            {
                TroopLineInfo troopLineInfo = m_viceMoveLineDic[objectId];
                if (troopLineInfo != null)
                {
                    m_viceMoveLineDic[objectId].Destroy();
                }                
                m_viceMoveLineDic.Remove(objectId);
            }
        }

        private void DeleteAllViceMoveLine()
        {
            foreach (var viceMoveLine in m_viceMoveLineDic.Values)
            {
                if (viceMoveLine != null)
                {
                    viceMoveLine.Destroy();
                }
            }
            m_viceMoveLineDic.Clear();
        }

        #endregion

        private ExpeditionProxy m_expeditionProxy = null;
        private PlayerProxy m_playerProxy = null;
        private WorldCameraParam m_storeWorldCameraParam = null;
        private Dictionary<string, Action<string>> m_onTouchObjectHandlers = new Dictionary<string, Action<string>>();
        private Dictionary<string, Action<string>> m_onTouchObjectBeginHandlers = new Dictionary<string, Action<string>>();
        private Dictionary<string, Action<string>> m_onTouchMoveHitObjectHandlers = new Dictionary<string, Action<string>>();

        public int m_selectObjectId = 0;
        private int m_previewSelectEnemyTroopIndex = 0;
        private MapObjectSelectEffect m_enemyTroopSelectEffect = null;
        private TroopLineInfo m_troopMovePreviewLine = null;
        private Vector2Int m_curMovePos = Vector2Int.zero;
        private Timer m_selectTroopMoveTimer = null;
        private List<GameObject> m_previewCreatePlayerOnGroundObject = new List<GameObject>();
        private readonly string m_previewCreateTroopOnGroundName = "build_create_troop_on_ground";
        private GameObject m_dragTroopGuideOnGroundObject = null;
        private Timer m_checkTroopMovedForGuideTimer = null;
        private Timer m_checkTroopMoveToPosForGuideTimer = null;
        private readonly string m_dragTroopGuideOnGroundName = "build_guide_on_ground";

        private string m_touchBeginColliderName = string.Empty;
        Timer m_checkTwoTouchTimer;

    }
}