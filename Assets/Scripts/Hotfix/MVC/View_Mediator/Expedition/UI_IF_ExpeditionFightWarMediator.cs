// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_IF_ExpeditionFightWarMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using Data;
using Hotfix;
using UnityEngine.UI;

namespace Game {

    public class ExpeditionFightWarViewData
    {
        public ExpeditionDefine ExpeditionCfg { get; set; }
        public ExpeditionBattleDefine ExpeditionBattleCfg { get; set; }
    }

    public class UI_IF_ExpeditionFightWarMediator : GameMediator {
        #region Member

        public static string NameMediator = "UI_IF_ExpeditionFightWarMediator";

        #endregion

        //IMediatorPlug needs
        public UI_IF_ExpeditionFightWarMediator(object viewComponent ):base(NameMediator, viewComponent ) {}

        public UI_IF_ExpeditionFightWarView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Expedition_Exit.TagName,
                CmdConstant.MapObjectChange,
                Expedition_BattleInfo.TagName,
                CmdConstant.ExpeditionTroopRemove,
                CmdConstant.ExpeditionTaskUICloseWhenFightting,
                CmdConstant.ExpeditionShowArmySelectUI,
                CmdConstant.ExpeditionHideArmySelectUI,
                CmdConstant.OnOpenSelectMainTroop,
                CmdConstant.OnCloseSelectMainTroop,
                CmdConstant.ExpeditionUIDoubleSelect,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Expedition_Exit.TagName:
                    {
                        var response = notification.Body as Expedition_Exit.response;
                        if (response == null) return;
                        OnExitResponse(response);
                    }
                    break;
                case CmdConstant.MapObjectChange:
                    OnMapObjectChange();
                    break;
                case Expedition_BattleInfo.TagName:
                    Expedition_BattleInfo.request request = notification.Body as Expedition_BattleInfo.request;
                    if (request == null) return;
                    OnBattleFinish(request);
                    break;
                case CmdConstant.ExpeditionTroopRemove:
                    OnTroopRemove();
                    RemoveTroopCaptainHeadView(notification.Body as ArmyData);
                    break;
                case CmdConstant.ExpeditionTaskUICloseWhenFightting:
                    view.m_pl_target_ArabLayoutCompment.gameObject.SetActive(true);
                    break;
                case CmdConstant.ExpeditionShowArmySelectUI:
                    view.m_pl_myself_ContentSizeFitter.gameObject.SetActive(true);
                    break;
                case CmdConstant.ExpeditionHideArmySelectUI:
                    view.m_pl_myself_ContentSizeFitter.gameObject.SetActive(false);
                    break;
                case CmdConstant.OnOpenSelectMainTroop:
                    SetSelectObjectId((int)notification.Body);
                    break;
                case CmdConstant.OnCloseSelectMainTroop:
                    ClearSelectObjectId();
                    break;
                case CmdConstant.ExpeditionUIDoubleSelect:
                    List<int> objectIdList = notification.Body as List<int>;
                    SetDoubleSelectObjectIdList(objectIdList);
                    break;
                default:
                    break;
            }
        }        

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            if (m_timeLeftTimer != null)
            {
                m_timeLeftTimer.Cancel();
                m_timeLeftTimer = null;
            }
            WorldCamera.Instance().RemoveViewChange(UpdateTroopCaptainHeadViews);
            RemoveTroopCaptainHeadViews();
            CoreUtils.uiManager.CloseUI(UI.s_expeditionFightTask);
            CoreUtils.uiManager.CloseUI(UI.s_buffList);
        }

        public override void PrewarmComplete(){
            
        }

        private bool m_bObjectDirty = false;

        public override void Update()
        {
            if (m_bObjectDirty)
            {
                view.m_sv_target_ListView.ForceRefresh();
                RefreshPlayerTroopList();
                RrfreshBossUI();
                m_bObjectDirty = false;
            }            
        }        

        protected override void InitData()
        {
            IsOpenUpdate = true;
            m_viewData = view.data as ExpeditionFightWarViewData;
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            m_playerTroopHead.Add(view.m_UI_Item_MainIFArm1);
            m_playerTroopHead.Add(view.m_UI_Item_MainIFArm2);
            m_playerTroopHead.Add(view.m_UI_Item_MainIFArm3);
            m_playerTroopHead.Add(view.m_UI_Item_MainIFArm4);
            m_playerTroopHead.Add(view.m_UI_Item_MainIFArm5);

            InitEnemyAndPlayerTroopData();
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_target_ListView.ItemPrefabDataList, OnItemPrefabLoadFinish);
            InitTimeLeft();
            RefreshPlayerTroopList();
            InitBossUI();
            view.m_lbl_turn_LanguageText.text = LanguageUtils.getTextFormat(805048, m_viewData.ExpeditionCfg.level, m_expeditionProxy.MaxExpeditionLevel);
            view.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(181104, m_enemyList.Count, m_expeditionProxy.GetMonsterTroopCount());
            RefreshBuff();

            WorldCamera.Instance().AddViewChange(UpdateTroopCaptainHeadViews);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Interface.AddClickEvent(OnClickBack);
            view.m_btn_icon_GameButton.onClick.AddListener(OnClickFightTask);
            view.m_btn_arrow_GameButton.onClick.AddListener(OnClickArrow);
        }

        protected override void BindUIData()
        {

        }

        public override bool onMenuBackCallback()
        {
            OnClickBack();
            return true;
        }

        #endregion

        #region 多部队选择

        List<int> selectObjectIdList = new List<int>();
        private Timer troopClickTimer = null;
        private bool troopClickFlag = false;
        private bool troopDoubleClickFlag = false;

        private void ClearSelectObjectId()
        {
            selectObjectIdList.Clear();

            RefreshPlayerTroopSelectState();

            troopDoubleClickFlag = false;
        }

        private void SetSelectObjectId(int objectId)
        {
            selectObjectIdList.Clear();
            selectObjectIdList.Add(objectId);

            RefreshPlayerTroopSelectState();
        }

        private void SetDoubleSelectObjectIdList(List<int> objectIdList)
        {
            selectObjectIdList.Clear();
            selectObjectIdList.AddRange(objectIdList);

            RefreshPlayerTroopSelectState();
        }

        #endregion

        private void InitEnemyAndPlayerTroopData()
        {
            m_enemyList = SummonerTroopMgr.Instance.ExpeditionTroop.GetEnemyDatas();
            m_playerTroopList = SummonerTroopMgr.Instance.ExpeditionTroop.GetPlayerTroopDatas();
        }

        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_target_ListView.SetInitData(dict, funcTab);
            view.m_sv_target_ListView.FillContent(m_enemyList.Count);

            CreateTroopCaptainHeadViews();
        }

        private void ItemEnter(ListView.ListItem item)
        {
            UI_Item_ExpeditionFightWarQueue_SubView subView = null;
            if(item.data != null)
            {
                subView = item.data as UI_Item_ExpeditionFightWarQueue_SubView;
            }
            else
            {
                subView = new UI_Item_ExpeditionFightWarQueue_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            if (item.index >= m_enemyList.Count) return;
            int index = item.index;
            subView?.Show(m_enemyList[item.index], () =>
            {
                if (m_enemyList.Count > index && !TroopHelp.IsHaveState(m_enemyList[index].armyStatus, ArmyStatus.FAILED_MARCH))
                {
                    WorldCamera.Instance().ViewTerrainPos(m_enemyList[index].go.transform.position.x, m_enemyList[index].go.transform.position.z, 1000, null);

                    TouchTroopInfo touchTroopInfo = new TouchTroopInfo();
                    touchTroopInfo.mainArmObjectId = m_enemyList[index].objectId;

                    AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateTroopSelectHud, touchTroopInfo);

                    var expeditionMedia = GlobalBehaviourManger.Instance.GetGlobalMediator(ExpeditionMediator.NameMediator) as ExpeditionMediator;
                    if (expeditionMedia != null)
                    {
                        expeditionMedia.removeAllSelectMyTroopEffect();
                        expeditionMedia.m_selectObjectId = m_enemyList[index].objectId;
                    }
                }
            });
        }

        private void OnClickBack()
        {
            if (m_expeditionProxy.ExpeditionStatus == ExpeditionFightStatus.WatingForFinish) return;
            Alert.CreateAlert(805010).SetLeftButton().SetRightButton(() =>
            {
                m_expeditionProxy.ExpeditionStatus = ExpeditionFightStatus.WatingForFinish;
                Expedition_Exit.request request = new Expedition_Exit.request();
                AppFacade.GetInstance().SendSproto(request);

            }).Show();
            
        }

        private void OnExitResponse(Expedition_Exit.response response)
        {
            if (response.result)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.ExitExpeditionMap);
                CoreUtils.uiManager.CloseUI(UI.s_expeditionFightWar);
            }
        }

        private void InitTimeLeft()
        {
            m_timeLeftTimer = Timer.Register(1, () =>
            {
                RefreshTimeLeft();
            }, null, true);
            RefreshTimeLeft();
        }

        private void RefreshTimeLeft()
        {
            long endTime = m_expeditionProxy.ExpeditionFightEndTime;
            long serverTime = ServerTimeModule.Instance.GetServerTime();
            int leftTime = Mathf.Max(0, (int)(endTime - serverTime));
            view.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(leftTime);
            if (leftTime == 0)
            {
                m_timeLeftTimer.Cancel();
                m_timeLeftTimer = null;
            }
        }

        private void InitBossUI()
        {
            view.m_pl_hp.gameObject.SetActive(m_viewData.ExpeditionCfg.type == (int)ExpeditionLevelType.Boss);
            if (m_viewData.ExpeditionCfg.type == (int)ExpeditionLevelType.Boss && m_enemyList.Count >= 1)
            {
                ArmyData bossData = m_enemyList[0];
                HeroProxy.Hero boss = m_heroProxy.GetHeroByID(bossData.heroId);
                view.m_UI_Model_CaptainHead.SetHero(boss);
                RrfreshBossUI();
            }            
        }

        private void RrfreshBossUI()
        {
            if (m_viewData.ExpeditionCfg.type != (int)ExpeditionLevelType.Boss) return;
            if (m_enemyList.Count == 0) return;
            ArmyData bossData = m_enemyList[0];
            view.m_pb_Hp_GameSlider.value = bossData.troopNums * 1.0f / bossData.troopNumMax;
        }

        private void RefreshPlayerTroopSelectState()
        {
            for (int i = 0; i < m_playerTroopList.Count && i < m_playerTroopHead.Count; ++i)
            {
                ArmyData armyData = m_playerTroopList[i];
                m_playerTroopHead[i].SetChooseState(selectObjectIdList.Contains(armyData.objectId));
            }
        }

        private void RefreshPlayerTroopList()
        {
            for(int i = 0; i < m_playerTroopList.Count && i < m_playerTroopHead.Count; ++i)
            {
                ArmyData armyData = m_playerTroopList[i];
                m_playerTroopHead[i].Show(armyData, ()=>
                {
                    troopDoubleClickFlag = false;

                    if (troopClickTimer != null)
                    {
                        Timer.Cancel(troopClickTimer);
                        troopClickTimer = null;
                    }

                    if (troopClickFlag == true)
                    {
                        if (armyData.armyStatus == (long)ArmyStatus.PALLY_MARCH ||
                            armyData.armyStatus == (long)ArmyStatus.FAILED_MARCH ||
                            armyData.armyStatus == (long)ArmyStatus.COLLECTING ||
                            armyData.armyStatus == (long)ArmyStatus.GARRISONING ||
                            armyData.armyStatus == (long)ArmyStatus.RALLY_WAIT ||
                            armyData.armyStatus == (long)ArmyStatus.RALLY_BATTLE)
                        {
                            if (armyData.go != null && !TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                            {
                                SetSelectObjectId(armyData.objectId);

                                WorldCamera.Instance().ViewTerrainPos(armyData.go.transform.position.x, armyData.go.transform.position.z, 1000, null);

                                Timer.Register(0.0f, null, (float dt) =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionUISelectTroop, armyData.objectId);
                                });
                            }
                        }
                        else
                        {
                            Timer.Register(0.0f, null, (float dt) =>
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.DoubleTouchTroopSelect, armyData.objectId);
                            });
                        }                        

                        troopClickFlag = false; 
                        troopDoubleClickFlag = true;

                        return;
                    }

                    troopClickFlag = true;

                    ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                    troopClickTimer = Timer.Register(configDefine.moretTroopsClick, () =>
                    {
                        troopClickFlag = false;

                        if (armyData.go != null && !TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH))
                        {
                            SetSelectObjectId(armyData.objectId);

                            WorldCamera.Instance().ViewTerrainPos(armyData.go.transform.position.x, armyData.go.transform.position.z, 1000, null);

                            Timer.Register(0.0f, null, (float dt) =>
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionUISelectTroop, armyData.objectId);
                            });
                        }
                    });                    
                }, () =>
                {
                    WorldCamera.Instance().SetCanDrag(false);
                    Timer timer = null;
                    timer = Timer.Register(0.1f, null, (float dt) =>
                    {
                        if (!Input.GetMouseButton(0))
                        {
                            timer.Cancel();
                            WorldCamera.Instance().SetCanDrag(true);
                            return;
                        }
                        
                        var camera = CoreUtils.uiManager.GetUICamera();
                        Vector2 localMouse;
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(view.m_pl_myself_PolygonImage.rectTransform, Input.mousePosition, camera, out localMouse))
                        {
                            if (!view.m_pl_myself_PolygonImage.rectTransform.rect.Contains(localMouse))
                            {
                                int objectId = armyData.objectId;

                                if (!troopDoubleClickFlag)
                                {
                                    SetSelectObjectId(objectId);
                                }
                                else
                                {
                                    if (!selectObjectIdList.Contains(objectId))
                                    {
                                        if (selectObjectIdList.Count > 0)
                                        {
                                            objectId = selectObjectIdList[0];
                                        }
                                    }

                                    troopDoubleClickFlag = false;
                                }

                                WorldCamera.Instance().SetCanDrag(true);
                                AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionSelectTroop, objectId);

                                timer.Cancel();
                            }
                        }                        
                    }, true, false, view.m_pl_myself_PolygonImage);
                });
            }
            for(int i = m_playerTroopList.Count; i < m_playerTroopHead.Count; ++i)
            {
                m_playerTroopHead[i].gameObject.SetActive(false);
            }
        }

        private void OnMapObjectChange()
        {
            m_bObjectDirty = true;
        }

        private void OnTroopRemove()
        {
            view.m_sv_target_ListView.ForceRefresh();
            RefreshPlayerTroopList();
            int aliveEnemyCount = 0;
            foreach(var enemy in m_enemyList)
            {
                if(!TroopHelp.IsHaveState(enemy.armyStatus, ArmyStatus.FAILED_MARCH))
                {
                    aliveEnemyCount++;
                }
            }
            view.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(181104, aliveEnemyCount, m_expeditionProxy.GetMonsterTroopCount());
        }

        private void OnBattleFinish(Expedition_BattleInfo.request request)
        {
            CoreUtils.uiManager.CloseUI(UI.s_expeditionFightWar);
            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionFightFinish);
            ExpeditionFightResult result = (ExpeditionFightResult)request.win;
            if(result == ExpeditionFightResult.Win)
            {
                m_expeditionProxy.LastSelectedLevelId = 0;
                CoreUtils.uiManager.ShowUI(UI.s_expeditionFightWin, null, new ExpeditionFightWinViewData()
                {
                    ExpeditionCfg = m_viewData.ExpeditionCfg,
                    Star = (int)request.star,
                    IsFirstReward = request.firstReward,
                    Rewards = request.rewardInfo,
                    StarResult = request.starResult,
                });
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_expeditionFightFail, null, new ExpeditionFightFailViewData()
                {
                    ExpeditionCfg = m_viewData.ExpeditionCfg,
                    Result = result
                });
            }
        }

        private void OnClickArrow()
        {
            Animator animator = view.m_pl_target_ArabLayoutCompment.GetComponent<Animator>();
            if (animator != null)
            {
                if(m_btnArrowExpand)
                {
                    animator.Play("toHide", 0);
                }
                else
                {
                    animator.Play("toShow", 0);
                }
                m_btnArrowExpand = !m_btnArrowExpand;
            }
        }

        private void OnClickFightTask()
        {
            CoreUtils.uiManager.ShowUI(UI.s_expeditionFightTask, null, new ExpeditionFightTaskViewData()
            {
                ExpeditionCfg = m_viewData.ExpeditionCfg,
                ExpeditionBattleCfg = m_viewData.ExpeditionBattleCfg
            });
            view.m_pl_target_ArabLayoutCompment.gameObject.SetActive(false);
        }

        private void RefreshBuff()
        {
            var objCityBuff = view.m_UI_Item_MainIFBuff.gameObject;
            view.m_UI_Item_MainIFBuff.gameObject.SetActive(false);
            Dictionary<int, CityBuff> cityBuffDic = new Dictionary<int, CityBuff>();
            List<CityBuff> cityBuffList = new List<CityBuff>();

            if (m_playerProxy.CurrentRoleInfo.cityBuff != null)
            {
                foreach (var cityBuff in m_playerProxy.CurrentRoleInfo.cityBuff)
                {
                    if (cityBuff.Value.expiredTime != -2)
                    {
                        CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)cityBuff.Value.id);
                        if (cityBuffDefine != null)
                        {
                            if (!cityBuffDic.ContainsKey(cityBuffDefine.group))
                            {
                                cityBuffDic.Add(cityBuffDefine.group, cityBuff.Value);
                            }
                        }
                    }
                }
                cityBuffList.AddRange(cityBuffDic.Values);
                cityBuffList.Sort((x, y) => (int)(x.id - y.id));
            }

            foreach (var cityBuff in cityBuffList)
            {
                if (cityBuff.expiredTime != -2)
                {
                    CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)cityBuff.id);
                    CityBuffGroupDefine cityBuffGroupDefine = CoreUtils.dataService.QueryRecord<CityBuffGroupDefine>((int)cityBuffDefine.group);
                    if (cityBuffDefine != null && cityBuffGroupDefine != null)
                    {
                        GameObject obj = CoreUtils.assetService.Instantiate(objCityBuff);
                        obj.transform.SetParent(view.m_pl_buff_GridLayoutGroup.transform);
                        obj.transform.localPosition = Vector3.zero;
                        obj.transform.localScale = Vector3.one;
                        obj.gameObject.SetActive(true);
                        obj.name = cityBuffDefine.ID.ToString();
                        UI_Item_MainIFBuff_SubView SubView = new UI_Item_MainIFBuff_SubView(obj.GetComponent<RectTransform>());
                        SubView.SetCityBuffId((int)cityBuff.id);
                        SubView.SetIcon(cityBuffDefine.icon);
                        SubView.AddBtnListener(() =>
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_buffList, null, obj.transform.position);
                        });
                    }
                }
            }
        }

        #region 部队武将头像

        private void CreateTroopCaptainHeadViews()
        {
            CoreUtils.assetService.LoadAssetAsync<GameObject>(m_troopCaptainHeadPrefabName, (asset) =>
            {
                if (asset == null || asset.asset() == null || m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.Fightting)
                {
                    return;
                }
                var go = asset.asset() as GameObject;
                if (go == null) return;
                var enemyArmyDatas = SummonerTroopMgr.Instance.ExpeditionTroop.GetEnemyDatas();
                foreach (var armyData in enemyArmyDatas)
                {
                    m_enemyCaptainHeadViews.Add(armyData.dataIndex, CreateTroopCaptainHeadViews(armyData, go));
                }
                var playerArmyDatas = SummonerTroopMgr.Instance.ExpeditionTroop.GetPlayerTroopDatas();
                foreach (var armyData in playerArmyDatas)
                {
                    m_playerCaptainHeadViews.Add(armyData.dataIndex, CreateTroopCaptainHeadViews(armyData, go));
                }

                InitFightWarUIMapRect(go.transform as RectTransform);
                var viewCenter = WorldCamera.Instance().GetViewCenter();
                UpdateTroopCaptainHeadViews(viewCenter.x, viewCenter.y, WorldCamera.Instance().getCurrentCameraDxf());
            }, view.gameObject);
        }

        private void InitFightWarUIMapRect(RectTransform captainHeadView)
        {
            //LayoutRebuilder.ForceRebuildLayoutImmediate(view.gameObject.transform as RectTransform);
            m_screenSize = CoreUtils.uiManager.GetUILayer((int)UILayer.FullViewLayer).rect.size;
            Vector2 rectSize = new Vector2(captainHeadView.rect.width, captainHeadView.rect.height);
            Vector2 sizeOffset = new Vector2(rectSize.x / m_screenSize.x / 2, rectSize.y / m_screenSize.y / 2);
            float[] rectOffset = new float[4] { -sizeOffset.x, -sizeOffset.y, sizeOffset.x, sizeOffset.y };           
            AddUIMapRect(view.m_pl_target_ArabLayoutCompment.transform as RectTransform, rectOffset);
            AddUIMapRect(view.m_pl_myself_ArabLayoutCompment.transform as RectTransform, rectOffset);
            AddUIMapRect(view.m_pl_timeAndTurn, new float[4] { -sizeOffset.x, - sizeOffset.y, 0, 0 });
            if(m_viewData.ExpeditionCfg.type == (int)ExpeditionLevelType.Boss)
            {
                AddUIMapRect(view.m_pl_hp, rectOffset);
            }
            AddUIMapRect(view.m_UI_Model_Interface.m_root_RectTransform, rectOffset);
            AddUIMapRect(view.m_pl_buff_ArabLayoutCompment.transform as RectTransform, rectOffset);
            m_fightWarMapUIRect.Add(new MapUIRect() { x1 = sizeOffset.x, y1 = sizeOffset.y, x2 = 1 - sizeOffset.x, y2 = 1 - sizeOffset.y }); 
        }

        private void AddUIMapRect(RectTransform rectTransform, float[] offset)
        {
            if (rectTransform != null && offset.Length == 4)
            {
                Vector3[] fourCornersArray = new Vector3[4];
                rectTransform.GetWorldCorners(fourCornersArray);
                fourCornersArray[0] = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(fourCornersArray[0]);
                fourCornersArray[2] = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(fourCornersArray[2]);
                m_fightWarMapUIRect.Add(new MapUIRect()
                {
                    x1 = Mathf.Max(0, fourCornersArray[0].x / m_screenSize.x + offset[0]),
                    y1 = Mathf.Max(0, fourCornersArray[0].y / m_screenSize.y + offset[1]),
                    x2 = Mathf.Min(1, fourCornersArray[2].x / m_screenSize.x + offset[2]),
                    y2 = Mathf.Min(1, fourCornersArray[2].y / m_screenSize.y + offset[3]),
                });
            }
        }

        private UI_Pop_CaptainSet_SubView CreateTroopCaptainHeadViews(ArmyData armyData, GameObject prefab)
        {
            var obj = CoreUtils.assetService.Instantiate(prefab);
            obj.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.FullViewLayer));
            obj.transform.localScale = Vector3.one;
            UI_Pop_CaptainSet_SubView subView = new UI_Pop_CaptainSet_SubView(obj.GetComponent<RectTransform>());
            subView.Show(armyData, LocationToTroop);
            return subView;
        }

        private void RemoveTroopCaptainHeadViews()
        {
            foreach (var view in m_enemyCaptainHeadViews)
            {
                CoreUtils.assetService.Destroy(view.Value.gameObject);
            }
            m_enemyCaptainHeadViews.Clear();
            foreach (var view in m_playerCaptainHeadViews)
            {
                CoreUtils.assetService.Destroy(view.Value.gameObject);
            }
            m_playerCaptainHeadViews.Clear();
        }

        private void RemoveTroopCaptainHeadView(ArmyData armyData)
        {
            if (armyData.isPlayerHave)
            {
                if (m_playerCaptainHeadViews.ContainsKey(armyData.dataIndex))
                {
                    CoreUtils.assetService.Destroy(m_playerCaptainHeadViews[armyData.dataIndex].gameObject);
                    m_playerCaptainHeadViews.Remove(armyData.dataIndex);
                }

            }
            else
            {
                if (m_enemyCaptainHeadViews.ContainsKey(armyData.dataIndex))
                {
                    CoreUtils.assetService.Destroy(m_enemyCaptainHeadViews[armyData.dataIndex].gameObject);
                    m_enemyCaptainHeadViews.Remove(armyData.dataIndex);
                }
            }
        }

        private void UpdateTroopCaptainHeadViews(float x, float y, float dxf)
        {
            if (GameModeManager.Instance.CurGameMode != GameModeType.Expedition) return;
            if (m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.Fightting) return;
            foreach (var subView in m_enemyCaptainHeadViews)
            {
                UpdateTroopCaptainHeadView(x, y, subView.Value);
            }
            foreach (var subView in m_playerCaptainHeadViews)
            {
                UpdateTroopCaptainHeadView(x, y, subView.Value);
            }
        }

        private void UpdateTroopCaptainHeadView(float x, float y, UI_Pop_CaptainSet_SubView subView)
        {
            if (subView.ArmyData == null || subView.ArmyData.go == null) return;
            var position = subView.ArmyData.go.transform.position;
            float distance = (Vector2.Distance(new Vector2(position.x, position.z), new Vector2(x, y))) / 10;
            if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), position.x, position.z, string.Empty) || distance < 1)
            {
                subView.gameObject.SetActive(false);
                return;
            }
            subView.gameObject.SetActive(true);
            var startUIPos = WorldCamera.Instance().GetCamera().WorldToViewportPoint(position);
            if (startUIPos.x > 0 && startUIPos.x < 1 && startUIPos.y > 0 && startUIPos.y < 1)
            {
                subView.gameObject.SetActive(false);
                return;
            }
            if (startUIPos.z <= 0)
            {
                startUIPos.y = -startUIPos.y;
                startUIPos.x = -startUIPos.x;
            }
            Vector2 cursorPos = GetCursorPos(startUIPos);
            Vector2 screenPos = new Vector2(cursorPos.x * m_screenSize.x, cursorPos.y * m_screenSize.y);
            subView.m_root_RectTransform.anchoredPosition = screenPos;
            Vector2 dir = (cursorPos - Vector2.one / 2).normalized;
            double theta = Mathf.Atan2(dir.y, dir.x);
            double theta1 = Mathf.Atan2(dir.y, 0);
            if (theta <= 0)
            {
                theta = theta + 2 * Mathf.PI;
            }
            subView.m_img_arrow_PolygonImage.transform.localEulerAngles = new Vector3(0, 0, (float)((180 / Mathf.PI) * theta + 90));
        }

        private Vector2 GetCursorPos(Vector3 viewPortPos)
        {
            Vector2 result = new Vector2(viewPortPos.x, viewPortPos.y);
            List<Vector2> temp = null;
            Vector2 centerPos = Vector2.one / 2;
            foreach (var fightWarMapUIRect in m_fightWarMapUIRect)
            {
                temp = MathUtil.RectIntersectSegment(fightWarMapUIRect, result, centerPos);
                if (temp.Count == 2)
                {
                    result = MathUtil.FindClosestPoint(temp[0].x, temp[0].y, temp[1].x, temp[1].y);
                }
            }
            return result;
        }

        private void LocationToTroop(ArmyData armyData)
        {
            if (armyData == null || armyData.go == null) return;
            WorldCamera.Instance().ViewTerrainPos(armyData.go.transform.position.x, armyData.go.transform.position.z, 500, null);
        }

        #endregion

        private List<ArmyData> m_enemyList = new List<ArmyData>();
        private List<ArmyData> m_playerTroopList = new List<ArmyData>();
        private ExpeditionFightWarViewData m_viewData = null;
        private ExpeditionProxy m_expeditionProxy = null;
        private HeroProxy m_heroProxy = null;
        private PlayerProxy m_playerProxy = null;
        private List<UI_Item_MainIFArm_SubView> m_playerTroopHead = new List<UI_Item_MainIFArm_SubView>();
        private Timer m_timeLeftTimer = null;

        private Dictionary<int, UI_Pop_CaptainSet_SubView> m_playerCaptainHeadViews = new Dictionary<int, UI_Pop_CaptainSet_SubView>();
        private Dictionary<int, UI_Pop_CaptainSet_SubView> m_enemyCaptainHeadViews = new Dictionary<int, UI_Pop_CaptainSet_SubView>();
        private List<MapUIRect> m_fightWarMapUIRect = new List<MapUIRect>();
        private readonly string m_troopCaptainHeadPrefabName = "UI_Pop_CaptainSet";
        private Vector2 m_screenSize = Vector2.zero;
        private bool m_btnArrowExpand = true;
    }
}