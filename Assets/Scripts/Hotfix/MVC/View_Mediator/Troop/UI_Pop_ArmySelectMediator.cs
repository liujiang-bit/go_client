// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Pop_ArmySelectMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Hotfix;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;
using System;
using Data;

namespace Game
{
    public class FreeMarchParam
    {
        public int objectId;
        public int armyIndex;
        public OpenPanelType panelType;
        public long rid;
        public List<int> armyIndexList;
        public bool isCheckWar;
    }

    public class UI_Pop_ArmySelectMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Pop_ArmySelectMediator";
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private TroopProxy m_TroopProxy;
        private SoldierProxy m_soldierProxy;
        private TroopMainCreate m_mainTroopData;
        private RssProxy m_RssProxy;
        private MonsterProxy m_MonsterProxy;
        private WorldMapObjectProxy m_worldProxy;
        private PlayerProxy m_playerProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private ExpeditionProxy m_expeditionProxy;
        private HeroProxy m_heroProxy;
        private PlayerAttributeProxy playerAttributeProxy;
           
        private int rssId;
        private OpenPanelData m_OpenPanelData;

        private PolygonImage m_selectImg;

        private TroopDataObject m_troopDataObject;

        private Timer m_timer;

        private Vector2 m_targetPos;
        private List<UI_Item_CaptainSkill_SubView> m_skillSubViewList;
        private List<UI_Item_ArmyConstitute_SubView> m_ArmyConstituteSubViewList;
        private float m_initDetailItemHeigh;
        private float m_initDetailListHeigh;

        private Color m_initCostApColor;

        private int m_costAp;   //行动力

        private int m_resType; //1资源 2怪物 3其他

        private int m_lineCount;

        private float m_cameraHeight = -1;

        private TroopMainCreateData m_guideTroopData;
        private bool isRally = false;

        //非大地图上使用
        private List<ArmyData> m_activeTroopData = new List<ArmyData>();
        private int m_selectTroopIndex;
        
        #endregion

        //IMediatorPlug needs
        public UI_Pop_ArmySelectMediator(object viewComponent) : base(NameMediator, viewComponent)
        {

        }

        public UI_Pop_ArmySelectView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.OnTroopDataChanged,
                CmdConstant.MapObjectStatusChange,
                CmdConstant.ExpeditionTroopRemove,
                CmdConstant.ExpeditionFightFinish,
                CmdConstant.UpdatePlayerActionPower,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnTroopDataChanged:
                    if (GuideProxy.IsGuideing)
                    {
                        return;
                    }
                    OnRefreshView();
                    break;
                case CmdConstant.MapObjectStatusChange:
                    OnObjectStatusChange((int)notification.Body);
                    break;
                case CmdConstant.ExpeditionTroopRemove:
                    OnExpeditionTroopRemove(notification.Body as ArmyData);
                    break;
                case CmdConstant.ExpeditionFightFinish:
                    CoreUtils.uiManager.CloseUI(UI.s_createMainTroops);
                    break;
                case CmdConstant.UpdatePlayerActionPower:
                    OnActionPowerUpdata();
                    break;
                default:
                    break;
            }
        }

        #region UI template method

        public override void OpenAniEnd()
        {
        }

        public override void WinFocus()
        {
        }

        public override void WinClose()
        {
            WorldCamera.Instance().SetCanDrag(true);
            CancelTimer();
        }

        public override void OnRemove()
        {
            base.OnRemove();

            CancelTimer();

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllDrawLine);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveDrawLineCity);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllSelectMyTroopEffect);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionRemoveAllDrawLine);
                        AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionRemoveAllSelectMyTroopEffect);

                        AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionShowArmySelectUI);                        
                    }
                    break;
            }

            if (m_cameraHeight != -1)
            {
                WorldCamera.Instance().SetCameraDxf(m_cameraHeight, 300, null);
            }

            m_selectArmyList.Clear();
            m_selectArmyList = null;
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        protected override void InitData()
        {
            if(GameModeManager.Instance.CurGameMode == GameModeType.Expedition)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionHideArmySelectUI);
            }

            m_cameraHeight = WorldCamera.Instance().getCurrentCameraDxf();
            m_selectArmyList = new List<int>();

             m_OpenPanelData = view.data as OpenPanelData;
            if (m_OpenPanelData != null) rssId = m_OpenPanelData.id;
            if (m_OpenPanelData.type != OpenPanelType.JoinRally)
            {                
                float Firstdxf = WorldCamera.Instance().getCameraDxf("FTE_Scout");
                WorldCamera.Instance().SetCameraDxf(Firstdxf, 300, null);
            }
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_MonsterProxy= AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_RallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            
            bool isShow = true;
            ArmyData army = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(rssId);
            if (army != null)
            {
                if (army.isRally)
                {
                    isShow = false;
                }
            }

            if (m_OpenPanelData.type == OpenPanelType.JoinRally)
            {
                m_targetPos = new Vector2(m_OpenPanelData.pos.x * 100f, m_OpenPanelData.pos.y * 100);
                m_resType = 3;
                Vector2 v2= new Vector2(m_OpenPanelData.pos.x,m_OpenPanelData.pos.y);
                AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, v2);
                
            }else if (m_OpenPanelData.type == OpenPanelType.Reinfore&& isShow)
            {
                m_targetPos = new Vector2(m_OpenPanelData.pos.x * 100f, m_OpenPanelData.pos.y * 100);
                m_resType = 3;

                if (m_OpenPanelData.pos.x > 0 && m_OpenPanelData.pos.y > 0 && m_OpenPanelData.viewFlag)
                {
                    WorldCamera.Instance().SetCanDrag(false);
                    WorldCamera.Instance().SetCanClick(false);
                    WorldCamera.Instance().SetCanZoom(false);
                    WorldCamera.Instance().ViewTerrainPos(m_OpenPanelData.pos.x, m_OpenPanelData.pos.y, 200, () =>
                    {
                        WorldCamera.Instance().SetCanDrag(true);
                        WorldCamera.Instance().SetCanZoom(true);
                        WorldCamera.Instance().SetCanClick(true);
                    });
                }
            }
            else
            {
                MapObjectInfoEntity mapObjectInfo = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                if (mapObjectInfo != null)
                {
                    RssType rssType = mapObjectInfo.rssType;
                    if (rssType == RssType.Monster || rssType == RssType.SummonAttackMonster || rssType == RssType.SummonConcentrateMonster)
                    {
                        m_resType = 2;
                        m_targetPos = new Vector2(mapObjectInfo.gameobject.transform.position.x * 100, mapObjectInfo.gameobject.transform.position.z * 100);             
                    }
                    else
                    {
                        if (rssType == RssType.Stone || rssType == RssType.Farmland || rssType == RssType.Wood || rssType == RssType.Gold || rssType == RssType.Gem ||
                            rssType == RssType.GuildFoodResCenter || rssType == RssType.GuildWoodResCenter || rssType == RssType.GuildGoldResCenter || rssType == RssType.GuildGemResCenter)
                        {
                            m_resType = 1;
                        }
                        else
                        {
                            m_resType = 3;
                        }

                        if (rssType == RssType.Troop)
                        {
                            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                                .GetArmyData(rssId);
                            if (armyData != null)
                            {
                                m_targetPos= new Vector2(armyData.go.transform.position.x*100,armyData.go.transform.position.z*100);
                            }
                        }
                        else
                        {                           
                            m_targetPos = new Vector2(mapObjectInfo.objectPos.x, mapObjectInfo.objectPos.y);
                        }

                    }
                }
            }
        
            m_initCostApColor = view.m_lbl_valCostAp_LanguageText.color;
            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_list_Troops_ListView.ItemPrefabDataList);
            prefabNames.Add("UI_Item_ArmyConstitute");
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, (assetDic) =>
            {
                m_assetDic = assetDic;
                OnAssetLoadFinish();
            });

            if (!GuideManager.Instance.IsGuideFightSecondBarbarian)
            {
                if (m_timer == null)
                {
                    m_timer = Timer.Register(1.0f, UpdateTimer, null, true, true);
                }
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Item_ArmyQueueNew.m_img_normal_GameButton.onClick.AddListener(OnCreateTroopBtnClick);
            view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(OnCreateClick);
            view.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(OnTroopMapMarChClick);
            view.m_btn_info_GameButton.onClick.AddListener(OnBtnInfoClick);
            view.m_btn_ApQuestion_GameButton.onClick.AddListener(OnApQuestIonClick);
            view.m_ck_selectAll_GameToggle.onValueChanged.AddListener(OnMultiSelectToggleChanged);
            AddBuildUpGoUIEvent();
        }

        protected override void BindUIData()
        {
            if (m_OpenPanelData.type == OpenPanelType.CreateRally ||
                m_OpenPanelData.type == OpenPanelType.JoinRally)
            {
                view.m_ck_selectAll_GameToggle.gameObject.SetActive(false);
            }
            else
            {
                view.m_ck_selectAll_GameToggle.gameObject.SetActive(true);
            }

            if (GuideProxy.IsGuideing)
            {
                view.m_ck_selectAll_GameToggle.gameObject.SetActive(false);
            }
        }

        private void OnMonsterRemove(int objectId)
        {
            if (objectId == rssId)
            {
                CoreUtils.uiManager.CloseUI(UI.s_createMainTroops);
            }
        }

        private void OnObjectStatusChange(int objectId)
        {
            if (objectId == rssId)
            {
                MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                if (rssData != null)
                {
                    if (TroopHelp.IsHaveState(rssData.status, ArmyStatus.FAILED_MARCH))
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_createMainTroops); 
                    }
                }                
            }
        }

        private void OnExpeditionTroopRemove(ArmyData armyData)
        {
            if(armyData.objectId == rssId)
            {
                CoreUtils.uiManager.CloseUI(UI.s_createMainTroops);
                return;
            }
            else if(armyData.isPlayerHave)
            {
                OnRefreshView();
            }
        }

        private void OnActionPowerUpdata()
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        if (m_multiSelectFlag)
                        {
                            RefreshMultiMarChPrePanel();
                        }
                        else
                        {
                            if (view.m_pl_Go_ArabLayoutCompment.gameObject.activeSelf && m_troopDataObject != null)
                            {
                                int findIndex = -1;
                                TroopMainCreateData data = null;
                                int count = m_mainTroopData.GetDataCount();
                                for (int i = 0; i < count; i++)
                                {
                                    data = m_mainTroopData.GetData(i);
                                    if (data.id == m_troopDataObject.id)
                                    {
                                        findIndex = i;
                                        break;
                                    }
                                }
                                if (findIndex > -1)
                                {
                                    ListView.ListItem listItem = view.m_list_Troops_ListView.GetItemByIndex(findIndex);
                                    if (listItem != null)
                                    {
                                        ClickHead(listItem); 
                                    }
                                    return;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private void OnRefreshView()
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        OnRefreshViewOnWorld();
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        OnRefreshViewOnExpedition();
                    }
                    break;
            }
        }

        private void OnRefreshViewOnWorld() 
        {
            if (m_mainTroopData == null) return;

            m_mainTroopData.Update();
            ReadListData();
            RefreshCountTitle();

            int count = m_mainTroopData.GetDataCount();
            view.m_list_Troops_ListView.FillContent(count);

            if (m_multiSelectFlag)
            {
                List<int> newAllArmyList = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    var data = m_mainTroopData.GetData(i);
                    if (data != null)
                    {
                        if (CheckArmyStatus((long)data.armyStatus))
                        {
                            newAllArmyList.Add(data.id);
                        }                            
                    }
                }

                List<int> newSelectArmyList = new List<int>();
                List<int> removeSelectArmyList = new List<int>();
                foreach (var armyIndex in m_selectArmyList)
                {
                    if (newAllArmyList.Contains(armyIndex))
                    {
                        newSelectArmyList.Add(armyIndex);
                    }
                    else
                    {
                        removeSelectArmyList.Add(armyIndex);
                    }
                }

                foreach (var armyIndex in removeSelectArmyList)
                {
                    MultiUnSelectArmyOnWorld(armyIndex);
                }

                foreach (var armyIndex in newSelectArmyList)
                {
                    MultiSelectArmyOnWorld(armyIndex);
                }

                m_selectArmyList = newSelectArmyList;

                RefreshMultiMarChPrePanel();
            }
            else
            {
                if (count > 0)
                {
                    if (m_troopDataObject != null)
                    {
                        int findIndex = -1;
                        TroopMainCreateData data = null;
                        for (int i = 0; i < count; i++)
                        {
                            data = m_mainTroopData.GetData(i);
                            if (data.id == m_troopDataObject.id)
                            {
                                findIndex = i;
                                break;
                            }
                        }
                        if (findIndex > -1)
                        {
                            m_troopDataObject.Update(data.ArmyInfo);
                            MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                            if (rssData != null)
                            {
                                m_troopDataObject.CalCollectData();
                            }
                            RefreshArmyDetail(data);

                            ListView.ListItem listItem = view.m_list_Troops_ListView.GetItemByIndex(findIndex);
                            if (listItem != null)
                            {
                                SetGoPos(listItem);
                            }
                            return;
                        }
                    }
                }

                if (m_troopDataObject != null)
                {
                    m_troopDataObject = null;
                    view.m_UI_Item_ArmyQueueNew.gameObject.SetActive(true);
                    ShowCreateTroopBtn();
                }
            }

            refreshMultiToggleState(); 
        }

        void OnRefreshViewOnExpedition()
        {
            m_lineCount = 0;
            InitActiveTroopData();
            m_lineCount = m_activeTroopData.Count;
            RefreshCountTitle();
            view.m_list_Troops_ListView.FillContent(m_lineCount);

            if (m_multiSelectFlag)
            {
                List<int> newAllArmyList = new List<int>();
                foreach (var armyData in m_activeTroopData)
                {
                    if (CheckArmyStatus(armyData.armyStatus))
                    {
                        newAllArmyList.Add(armyData.objectId);
                    }
                }

                List<int> newSelectArmyList = new List<int>();
                List<int> removeSelectArmyList = new List<int>();
                foreach (var objectId in m_selectArmyList)
                {
                    if (newAllArmyList.Contains(objectId))
                    {
                        newSelectArmyList.Add(objectId);
                    }
                    else
                    {
                        removeSelectArmyList.Add(objectId);
                    }
                }

                foreach (var armyIndex in removeSelectArmyList)
                {
                    MultiUnSelectArmyOnExpedition(armyIndex);
                }

                foreach (var armyIndex in newSelectArmyList)
                {
                    MultiSelectArmyOnExpedition(armyIndex);
                }

                m_selectArmyList = newSelectArmyList;

                RefreshMultiMarChPrePanel();
            }
            else
            {
                if (m_lineCount > 0)
                {
                    if (m_selectTroopIndex > 0)
                    {
                        int findIndex = GetSelectTroopListItemIndex();
                        if (findIndex > -1)
                        {
                            var armyData = m_activeTroopData[findIndex];
                            RefreshArmyDetailOnExpeidtion(armyData);
                            ListView.ListItem listItem = view.m_list_Troops_ListView.GetItemByIndex(findIndex);
                            if (listItem != null)
                            {
                                SetGoPos(listItem);
                            }
                            return;
                        }
                        else
                        {
                            m_selectTroopIndex = 0;
                            view.m_pl_Go_ArabLayoutCompment.gameObject.SetActive(false);
                        }
                    }
                }
            }

            refreshMultiToggleState(); 
        }

        private void OnCreateTroopBtnClick()
        {
            view.m_ck_selectAll_GameToggle.isOn = false;

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveDrawLineCity);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllDrawLine);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllSelectMyTroopEffect);
                    }
                    break;
            }

            ShowCreateTroopBtn();
        }

        private void ShowCreateTroopBtn()
        {
            if (m_OpenPanelData.type == OpenPanelType.Common || 
                m_OpenPanelData.type== OpenPanelType.JoinRally ||
                m_OpenPanelData.type==OpenPanelType.Reinfore)
            {
                SetSelectImg(view.m_UI_Item_ArmyQueueNew.m_img_select_PolygonImage);
                ShowCreateTroopPanel(true);
            }
            else if (m_OpenPanelData.type == OpenPanelType.CreateRally)
            {
                view.m_pl_NewArmy_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_pl_Go_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_UI_Pop_BuildUpGo.gameObject.SetActive(true);
                SetBuildUpGoData();
                OnDrawLine(rssId,2);
            }
        }

        private void AddBuildUpGoUIEvent()
        {
            view.m_UI_Pop_BuildUpGo.m_UI_Item_BuildUpGoTime0.m_ck_toogle_GameToggle.onValueChanged.AddListener(
                (ison) =>
                {
                    m_OpenPanelData.timesType=1;
                });
            view.m_UI_Pop_BuildUpGo.m_UI_Item_BuildUpGoTime1.m_ck_toogle_GameToggle.onValueChanged.AddListener(
                (ison) =>
                {
                    m_OpenPanelData.timesType=2;
                });
            view.m_UI_Pop_BuildUpGo.m_UI_Item_BuildUpGoTime2.m_ck_toogle_GameToggle.onValueChanged.AddListener(
                (ison) =>
                {
                    m_OpenPanelData.timesType=3;
                });
            view.m_UI_Pop_BuildUpGo.m_UI_Item_BuildUpGoTime3.m_ck_toogle_GameToggle.onValueChanged.AddListener(
                (ison) =>
                {
                    m_OpenPanelData.timesType=4;
                });
            
            view.m_UI_Pop_BuildUpGo.m_UI_BuildUp.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_createMainTroops);
                FightHelper.Instance.OpenCreateArmyPanel(m_OpenPanelData);
            });
        }

        private void SetBuildUpGoData()
        {
            RallyTimesDefine rallyTimesDefine = CoreUtils.dataService.QueryRecord<RallyTimesDefine>(1);
            if (rallyTimesDefine != null)
            {
                view.m_UI_Pop_BuildUpGo.m_UI_Item_BuildUpGoTime0.m_lbl_time_LanguageText.text =
                    ClientUtils.FormatTime(rallyTimesDefine.rallyTime1);
                view.m_UI_Pop_BuildUpGo.m_UI_Item_BuildUpGoTime1.m_lbl_time_LanguageText.text =
                    ClientUtils.FormatTime(rallyTimesDefine.rallyTime2);
                view.m_UI_Pop_BuildUpGo.m_UI_Item_BuildUpGoTime2.m_lbl_time_LanguageText.text =
                    ClientUtils.FormatTime(rallyTimesDefine.rallyTime3);
                view.m_UI_Pop_BuildUpGo.m_UI_Item_BuildUpGoTime3.m_lbl_time_LanguageText.text =
                    ClientUtils.FormatTime(rallyTimesDefine.rallyTime4);
            }       
        }

        private void OnBtnInfoClick()
        {
            //逻辑在界面绑定动画
        }

        private  long killMonsterReduceVit =0;
        private long poweNum=0;
        private int GetCostMobilityNum(TroopDataObject troopDataObject)
        {
            float vitalityReduction = 0;
            if (troopDataObject != null)
            {
                if (troopDataObject.ArmyData != null)
                {
                    var attributes = playerAttributeProxy.GetTroopAttribute(troopDataObject.ArmyData.mainHeroId, troopDataObject.ArmyData.deputyHeroId);
                    if (attributes != null)
                    {
                        vitalityReduction= attributes[(int) attrType.vitalityReduction - 1].value;
                    }

                    killMonsterReduceVit = troopDataObject.ArmyData.killMonsterReduceVit;
                }
            }

            int power = 0;
            int vitalityReduceUnit = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).vitalityReduceUnit;
            long monsterId=0;
            MapObjectInfoEntity mapObjectInfoEntity = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            if (mapObjectInfoEntity != null)
            {
                monsterId = mapObjectInfoEntity.monsterId;
            }

            MonsterDefine monsterDefine  = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)monsterId);
            if (monsterDefine != null)
            {
                if (m_OpenPanelData.type == OpenPanelType.Common)
                {
                    var constAp = monsterDefine.costAP;
                    power = (int)(constAp - vitalityReduction - (killMonsterReduceVit * vitalityReduceUnit));
                }else if (m_OpenPanelData.type == OpenPanelType.JoinRally ||
                          m_OpenPanelData.type== OpenPanelType.Reinfore)
                {
                    var rallyAp = monsterDefine.rallyAP;
                    power = (int)(rallyAp - vitalityReduction - (killMonsterReduceVit * vitalityReduceUnit));
                }
            }

            poweNum = (killMonsterReduceVit * vitalityReduceUnit);
            return power;
        }

        private void OnApQuestIonClick()
        {
            if (m_troopDataObject != null) GetCostMobilityNum(m_troopDataObject);
            string des = LanguageUtils.getTextFormat(100718, killMonsterReduceVit, poweNum);
            HelpTip.CreateTip(des,view.m_btn_ApQuestion_GameButton.transform).SetAutoFilter(false).SetOffset(5).SetWidth(500).Show();
        }

        private void ShowCreateTroopPanel(bool state)
        {
            view.m_pl_NewArmy_ArabLayoutCompment.gameObject.SetActive(state);
            view.m_pl_Go_ArabLayoutCompment.gameObject.SetActive(!state);
            view.m_UI_Pop_BuildUpGo.gameObject.SetActive(false);
            if (state)
            {
                OnDrawLine(0,2);
            }
        }

        private void SetGoPos(ListView.ListItem item)
        {
            Vector2 localPos;
            Vector3 pos = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(item.go.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            RectTransform targetRect = item.go.GetComponent<RectTransform>();
            RectTransform goRect = view.m_pl_Go_ArabLayoutCompment.GetComponent<RectTransform>();
            view.m_pl_Go_ArabLayoutCompment.transform.localPosition = new Vector2(view.m_pl_Go_ArabLayoutCompment.transform.localPosition.x,
                                                               localPos.y - targetRect.pivot.y * targetRect.rect.height + targetRect.rect.height / 2 - 25);
        }

        //行军
        private void OnTroopMapMarChClick()
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        if (GuideManager.Instance.IsGuideFightSecondBarbarian)
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_createMainTroops);
                            AppFacade.GetInstance().SendNotification(CmdConstant.GuideSecondMarch);
                            return;
                        }

                        //判断负载是否已满
                        MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                        if (rssData != null)
                        {

                            if (rssData.rssType == RssType.Monster ||
                                rssData.rssType == RssType.SummonAttackMonster)
                            {
                                if (m_playerProxy.CurrentRoleInfo.actionForce < m_costAp)
                                {
                                    //Tip.CreateTip(LanguageUtils.getText(500207)).Show();
                                    PlayerDataHelp.ShowActionUI(m_costAp);
                                    return;
                                }
                            }
                            else
                            {
                                if (rssData.resourceGatherTypeDefine != null)
                                {
                                    int resWeight = m_RssProxy.GetResWeight(rssData.resourceGatherTypeDefine.type);
                                    if (m_troopDataObject != null)
                                    {
                                        Int64 diffWeight = m_troopDataObject.GetTotalWeight() - m_troopDataObject.GetCollectWeight();
                                        if (diffWeight < resWeight)
                                        {
                                            Tip.CreateTip(LanguageUtils.getText(500208)).Show();
                                            return;
                                        }
                                    }
                                }

                                if (m_OpenPanelData.type == OpenPanelType.Reinfore && TroopHelp.IsStrongHoldType(rssData.rssType)&&m_troopDataObject != null)
                                {
                                    if (m_troopDataObject.GetSoldierNum() >
                                        m_OpenPanelData.rallyTroopNum)
                                    {
                                        Tip.CreateTip(LanguageUtils.getText(200009)).Show();
                                        return;
                                    }

                                    if (TroopHelp.IsHaveState(m_troopDataObject.ArmyData.status, ArmyStatus.REINFORCE_MARCH)&&
                                        m_troopDataObject.ArmyData.targetArg.targetObjectIndex == m_OpenPanelData.id)
                                    {
                                        Tip.CreateTip(LanguageUtils.getText(200110)).Show();
                                        return;
                                    }
                                }
                                
                            }
                        }

                        FreeMarchParam param = new FreeMarchParam();
                        param.objectId = rssId;                        
                        param.panelType = m_OpenPanelData.type;
                        param.rid = m_OpenPanelData.jonRid;
                        List<int> armyIndexList = new List<int>();
                        if (m_multiSelectFlag)
                        {
                            foreach (var armyIndex in m_selectArmyList)
                            {
                                armyIndexList.Add(armyIndex);
                            }
                        }
                        else
                        {
                            if (m_troopDataObject != null)
                            {
                                armyIndexList.Add((int)m_troopDataObject.ArmyData.armyIndex);
                            }
                        }
                        if (armyIndexList.Count > 0)
                        {
                            param.armyIndex = armyIndexList[0];
                        }
                        param.armyIndexList = armyIndexList;
                        param.isCheckWar = false;
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapSendMapMarChData, param);
                        CoreUtils.uiManager.CloseUI(UI.s_createMainTroops);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        var armyData = GetSelectTroopData();
                        var enemyData = SummonerTroopMgr.Instance.ExpeditionTroop.GetArmyData(rssId);
                        if (!TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.FAILED_MARCH) &&
                            !TroopHelp.IsHaveState(enemyData.armyStatus, ArmyStatus.FAILED_MARCH))
                        {
                            List<int> objectIdList = new List<int>();
                            if (m_multiSelectFlag)
                            {
                                foreach (var objectId in m_selectArmyList)
                                {
                                    objectIdList.Add(objectId);
                                }
                            }
                            else
                            {
                                objectIdList.Add(armyData.objectId);
                            }
                            m_expeditionProxy.TroopMarchToEnemy(armyData.objectId, rssId, objectIdList);
                        }
                        CoreUtils.uiManager.CloseUI(UI.s_createMainTroops);
                    }
                    break;
            }         
        }

        private ArmyData GetSelectTroopData()
        {
            foreach(var troopData in m_activeTroopData)
            {
                if(troopData.dataIndex == m_selectTroopIndex)
                {
                    return troopData;
                }
            }
            return null;
        }

        private int GetSelectTroopListItemIndex()
        {
            for(int i = 0; i < m_activeTroopData.Count; ++i)
            {
                if(m_activeTroopData[i].dataIndex == m_selectTroopIndex)
                {
                    return i;
                }
            }
            return -1;
        }

        //创建部队
        private void OnCreateClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_createMainTroops);
            FightHelper.Instance.OpenCreateArmyPanel(m_OpenPanelData);
        }

        private void ReadListData()
        {
            if (m_OpenPanelData.type == OpenPanelType.CreateRally)
            {
                m_mainTroopData.SetGray(true);
            }
            else
            {
                m_mainTroopData.SetGray(false);
            }
            m_lineCount = m_mainTroopData.GetDataCount();
        }

        private void OnAssetLoadFinish()
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        OnAssetLoadFinishOnWorld();
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        OnAssetLoadFinishOnExpedition();
                    }
                    break;
            }
        }

        private void OnAssetLoadFinishOnWorld()
        {
            Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
            prefabNameList["UI_Item_ArmyQueueHead"] = m_assetDic["UI_Item_ArmyQueueHead"];
            m_mainTroopData = new TroopMainCreate();
            m_mainTroopData.Init(new List<TroopMainCreateDataType>() { TroopMainCreateDataType.Troop });
            ReadListData();
            int count = 0;
            if (GuideManager.Instance.IsGuideFightSecondBarbarian)
            {
                m_guideTroopData = m_mainTroopData.CreateGuildFadeData();
                count = 1;
            }
            else
            {
                count = m_mainTroopData.GetDataCount();
            }

            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ListItemByIndex;
            view.m_list_Troops_ListView.SetInitData(prefabNameList, funcTab);
            view.m_list_Troops_ListView.FillContent(count);
            //list禁止滚动
            view.m_list_Troops_ListView.GetComponent<ScrollRect>().vertical = false;

            RefreshCountTitle();

            int dispatchNum = m_TroopProxy.GetTroopDispatchNum();
            if (GuideProxy.IsGuideing)
            {
                dispatchNum = 1;
            }

            if (count < dispatchNum)
            {
                ShowCreateTroopBtn();
            }
            else
            {
                view.m_UI_Item_ArmyQueueNew.gameObject.SetActive(false);
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_pl_layout_VerticalLayoutGroup.GetComponent<RectTransform>());

                //默认选中第一个
                ListView.ListItem listItem = view.m_list_Troops_ListView.GetItemByIndex(0);
                if (listItem != null)
                {
                    ClickHead(listItem);
                }
            }

            refreshMultiToggleState();
        }

        private void OnAssetLoadFinishOnExpedition()
        {
            Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
            prefabNameList["UI_Item_ArmyQueueHead"] = m_assetDic["UI_Item_ArmyQueueHead"];

            m_lineCount = 0;
            InitActiveTroopData();
            m_lineCount = m_activeTroopData.Count;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ListItemByIndex;
            view.m_list_Troops_ListView.SetInitData(prefabNameList, funcTab);
            view.m_list_Troops_ListView.FillContent(m_lineCount);
            //list禁止滚动
            view.m_list_Troops_ListView.GetComponent<ScrollRect>().vertical = false;

            RefreshCountTitle();
            view.m_UI_Item_ArmyQueueNew.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_pl_layout_VerticalLayoutGroup.GetComponent<RectTransform>());

            //默认选中第一个
            ListView.ListItem listItem = view.m_list_Troops_ListView.GetItemByIndex(0);
            if (listItem != null)
            {
                ClickHead(listItem);
            }

            refreshMultiToggleState();
        }

        private void InitActiveTroopData()
        {
            switch(GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.Expedition:
                    {
                        m_activeTroopData.Clear();
                           var playerTroopDatas = SummonerTroopMgr.Instance.ExpeditionTroop.GetPlayerTroopDatas();
                        foreach (var troopData in playerTroopDatas)
                        {
                            if (!TroopHelp.IsHaveState(troopData.armyStatus, ArmyStatus.FAILED_MARCH))
                            {
                                m_activeTroopData.Add(troopData);
                            }
                        }
                    }
                    break;
            }
        }

        private void RefreshCountTitle()
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        int canCount = m_TroopProxy.GetTroopDispatchNum();
                        if (GuideProxy.IsGuideing)
                        {
                            canCount = 1;
                        }
                        view.m_lbl_title_LanguageText.text = string.Format(LanguageUtils.getText(180313) + ":{0}/{1}", m_lineCount, canCount);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        view.m_lbl_title_LanguageText.text = string.Format(LanguageUtils.getText(180313) + ":{0}/{1}", m_lineCount, m_lineCount);
                    }
                    break;
            }           
        }

        private void ListItemByIndex(ListView.ListItem listItem)
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        ListItemByIndexOnWorld(listItem);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        ListItemByIndexOnExpedtiion(listItem);
                    }
                    break;
            }
        }

        private void ListItemByIndexOnWorld(ListView.ListItem listItem)
        {
            if (m_mainTroopData == null)
            {
                return; 
            }

            if (!GuideManager.Instance.IsGuideFightSecondBarbarian)
            {
                if (listItem.index >= m_mainTroopData.GetDataCount())
                {
                    return;
                }
            }

            UI_Item_ArmyQueueHeadView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ArmyQueueHeadView>(listItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_ArmyQueueHeadView>(listItem.go);
            }

            if (itemView != null)
            {
                TroopMainCreateData data = null;
                if (GuideManager.Instance.IsGuideFightSecondBarbarian)
                {
                    data = m_guideTroopData;
                }
                else
                {
                    data = m_mainTroopData.GetData(listItem.index);
                }

                if (data != null)
                {
                    itemView.m_img_checkEffect_PolygonImage.gameObject.SetActive(m_multiSelectFlag && CheckArmyStatus((long)data.armyStatus));
                    itemView.m_img_check_PolygonImage.gameObject.SetActive(m_selectArmyList.Contains(data.id) && CheckArmyStatus((long)data.armyStatus));
                    itemView.m_img_select_PolygonImage.gameObject.SetActive(m_selectArmyList.Contains(data.id) && CheckArmyStatus((long)data.armyStatus));
                    itemView.m_pl_time_PolygonImage.gameObject.SetActive(m_selectArmyList.Contains(data.id) && CheckArmyStatus((long)data.armyStatus));

                    if (m_selectArmyList.Contains(data.id) && CheckArmyStatus((long)data.armyStatus))
                    {
                        TroopDataObject troopDataObject = new TroopDataObject();
                        troopDataObject.Init(data.ArmyInfo);
                        troopDataObject.CalCollectData();
                        itemView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(troopDataObject.GetMarchTime(rssId, m_targetPos));
                    }

                    itemView.m_UI_Model_CaptainHead.SetIcon(data.hero.config.heroIcon);
                    itemView.m_UI_Model_CaptainHead.SetRare(data.hero.config.rare);
                    itemView.m_UI_Model_CaptainHead.SetLevel(data.level);
                    itemView.m_btn_noTextButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
                    {
                        ClickHead(listItem);
                    });
                    ClientUtils.LoadSprite(itemView.m_img_state_PolygonImage, data.icon);
                    itemView.m_UI_Common_TroopsState.SetData((long)data.armyStatus);
                    Int64 count = 0;
                    if (data.ArmyInfo != null)
                    {
                        if (data.ArmyInfo.soldiers != null)
                        {
                            foreach (var soldier in data.ArmyInfo.soldiers.Values)
                            {
                                count = count + soldier.num;
                            }
                        }

                        itemView.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(count);
                    }
                    
                    if (data.isGray||count<=0)
                    {
                        itemView.m_btn_noTextButton_GameButton.interactable = false;
                        itemView.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_GrayChildrens.Gray();
                    }
                    else
                    {
                        itemView.m_btn_noTextButton_GameButton.interactable = true;
                        itemView.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_GrayChildrens.Normal();
                    }                    
                    
                    if (m_troopDataObject != null)
                    {
                        if (m_troopDataObject.ArmyData != null)
                        {
                            if (m_troopDataObject.ArmyData.armyIndex == data.id)
                            {
                                SetSelectImg(itemView.m_img_select_PolygonImage);
                            }
                        }                        
                    }
                }
            }
        }

        private void ListItemByIndexOnExpedtiion(ListView.ListItem listItem)
        {
            if (listItem.index >= m_activeTroopData.Count) return;

            UI_Item_ArmyQueueHeadView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ArmyQueueHeadView>(listItem.go);
            if (itemView != null)
            {
                var armyData = m_activeTroopData[listItem.index];
                HeroProxy.Hero hero = m_heroProxy.GetHeroByID(armyData.heroId);

                itemView.m_img_checkEffect_PolygonImage.gameObject.SetActive(m_multiSelectFlag && CheckArmyStatus(armyData.armyStatus));
                itemView.m_img_check_PolygonImage.gameObject.SetActive(m_selectArmyList.Contains(armyData.objectId) && CheckArmyStatus(armyData.armyStatus));
                itemView.m_img_select_PolygonImage.gameObject.SetActive(m_selectArmyList.Contains(armyData.objectId) && CheckArmyStatus(armyData.armyStatus));
                itemView.m_pl_time_PolygonImage.gameObject.SetActive(m_selectArmyList.Contains(armyData.objectId) && CheckArmyStatus(armyData.armyStatus));

                if (m_selectArmyList.Contains(armyData.objectId) && CheckArmyStatus(armyData.armyStatus))
                {
                    itemView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(CalExpeditionMarchTime(armyData));
                }

                itemView.m_UI_Model_CaptainHead.SetIcon(hero.config.heroIcon);
                itemView.m_UI_Model_CaptainHead.SetRare(hero.config.rare);
                itemView.m_UI_Model_CaptainHead.SetLevel(hero.level);
                itemView.m_btn_noTextButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
                {
                    ClickHead(listItem);
                });
                ClientUtils.LoadSprite(itemView.m_img_state_PolygonImage, TroopHelp.GetIcon(armyData.armyStatus));
                
                itemView.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
                if(listItem.index == GetSelectTroopListItemIndex())
                {
                    SetSelectImg(itemView.m_img_select_PolygonImage);
                }                
            }
        }

        private void SetSelectImg(PolygonImage img)
        {
            if (m_selectImg != null)
            {
                m_selectImg.gameObject.SetActive(false);
            }
            if (img != null)
            {
                img.gameObject.SetActive(true);
            }            
            m_selectImg = img;
        }

        private void ClickHead(ListView.ListItem listItem)
        {
            UI_Item_ArmyQueueHeadView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ArmyQueueHeadView>(listItem.go);
            
            if (m_multiSelectFlag)
            {
                int id = -1;
                long armyStatus = 0;

                switch (GameModeManager.Instance.CurGameMode)
                {
                    case GameModeType.World:
                        {
                            if (m_mainTroopData != null)
                            {
                                var troopMainCreateData = m_mainTroopData.GetData(listItem.index);
                                id = troopMainCreateData.id;
                                armyStatus = (long)troopMainCreateData.armyStatus;
                            }
                        }
                        break;
                    case GameModeType.Expedition:
                        {
                            if (m_activeTroopData != null)
                            {
                                var armyData = m_activeTroopData[listItem.index];
                                id = armyData.objectId;
                                armyStatus = armyData.armyStatus;
                            }
                        }
                        break;
                }

                if (id != -1 && armyStatus != 0 && CheckArmyStatus(armyStatus))
                {
                    if (m_selectArmyList.Contains(id))
                    {
                        m_selectArmyList.Remove(id);
                        OnMultiListItemClick(listItem, false);
                        RefreshMultiMarChPrePanel();
                    }
                    else
                    {
                        m_selectArmyList.Add(id);
                        OnMultiListItemClick(listItem, true);
                        RefreshMultiMarChPrePanel();
                    }
                }                
            }
            else
            {
                SetSelectImg(itemView.m_img_select_PolygonImage);
                ShowMarChPrePanel(listItem);
            }
        }
                
        private Vector2 lastV2= Vector2.zero;
        private Timer drawLineTimer;

        private void ShowMarChPrePanel(ListView.ListItem listItem)
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        ShowMarchPrePanelOnWorld(listItem);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        ShowMarchPrePanelOnExpedition(listItem);
                    }
                    break;
            }
        }


        //1 有部队的创建白线 2：无部队的创建白线
        private void OnDrawLine(int id,int type)
        {
            Vector2 v2 = Vector2.zero;
            if (m_OpenPanelData.type == OpenPanelType.JoinRally ||
                m_OpenPanelData.type== OpenPanelType.Reinfore)
            {
                v2 = m_OpenPanelData.pos;
            }
            else
            {
                v2 = m_RssProxy.GetV2(rssId);
            }

            if (v2.Equals(Vector2.zero))
            {
                return;
            }

            lastV2 = v2;           
            DrawLineInfo drawLineInfo = new DrawLineInfo();
            drawLineInfo.arnyIndex = id;
            drawLineInfo.targetPos = v2;
            AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveDrawLineCity);
            if (type == 1)
            {                
                AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateDrawLine, drawLineInfo);
            }
            else if(type==2)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.MapDrawLineCity, drawLineInfo);
            }

            //每秒更新下画线
            if (drawLineTimer != null)
            {
                drawLineTimer.Cancel();
                drawLineTimer = null;
            }
            drawLineTimer = Timer.Register(1f, () =>
            {
                if (rssId > 0)
                {
                    Vector2 pos = m_RssProxy.GetV2(rssId);
                    if (!pos.Equals(Vector2.zero) && pos != lastV2)
                    {
                        DrawLineInfo drawLineInfo1 = new DrawLineInfo();
                        drawLineInfo1.arnyIndex = id;
                        drawLineInfo1.targetPos = pos;
                        if (type == 1)
                        {                
                            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateDrawLine, drawLineInfo1);
                        }
                        else if(type==2)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.MapDrawLineCity, drawLineInfo1);
                        }
                        lastV2 = pos;
                    }
                }
            }, null, true);
        }

        private void ShowMarchPrePanelOnWorld(ListView.ListItem listItem)
        {
            view.m_pl_warnTip_LayoutElement.gameObject.SetActive(false);
            TroopMainCreateData data = null;
            bool isGuide = false;
            if (GuideManager.Instance.IsGuideFightSecondBarbarian)
            {
                data = m_guideTroopData;
                isGuide = true;
            }
            else
            {
                if (listItem.index >= m_mainTroopData.GetDataCount())
                {
                    return;
                }
                data = m_mainTroopData.GetData(listItem.index);
            }
            if (data == null)
            {
                return;
            }
            TouchTroopSelectParam param = new TouchTroopSelectParam();
            param.armIndex = data.id;
            param.isOpenWin = false;
            param.isGuide = isGuide;
            AppFacade.GetInstance().SendNotification(CmdConstant.SetSelectTroop, param);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllDrawLine);
            OnDrawLine(data.id,1);
            ShowCreateTroopPanel(false);
            SetGoPos(listItem);
            if (m_OpenPanelData.type == OpenPanelType.JoinRally)
            {
                MapObjectInfoEntity mapObjectInfoEntity = m_worldProxy.GetWorldMapObjectByRid(m_OpenPanelData.jonRid);
                if (mapObjectInfoEntity != null)
                {
                    rssId = (int)mapObjectInfoEntity.objectId;
                }
            }

            if (m_OpenPanelData.type == OpenPanelType.Reinfore)
            {
                rssId = (int)m_OpenPanelData.reinforceObjectIndex;
            }

            bool isRefreshToop = true;
            if (m_troopDataObject == null)
            {
                m_troopDataObject = new TroopDataObject();
                m_troopDataObject.Init(data.ArmyInfo);
                m_troopDataObject.CalCollectData();
                isRefreshToop = false;
            }

            MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            if (rssData != null)
            {
                if (rssData.rssType == RssType.Monster ||
                    rssData.rssType == RssType.SummonAttackMonster) //打怪
                {
                    view.m_img_mist_VerticalLayoutGroup.gameObject.SetActive(false);
                    view.m_img_CostAp_VerticalLayoutGroup.gameObject.SetActive(true);

                    if (isRefreshToop)
                    {
                        m_troopDataObject.Update(data.ArmyInfo);
                    }
                    RefreshArmyDetail(data);

                    //兵力
                    long troopNum = m_troopDataObject.GetSoldierNum();
                    view.m_lbl_countCostAp_LanguageText.text = ClientUtils.FormatComma(troopNum);
                    //体力
                    m_costAp = GetCostMobilityNum(m_troopDataObject);
                    view.m_lbl_valCostAp_LanguageText.text = m_costAp.ToString();
                    if (m_playerProxy.CurrentRoleInfo.actionForce < m_costAp)
                    {
                        view.m_lbl_valCostAp_LanguageText.color = Color.red;
                    }
                    else
                    {
                        view.m_lbl_valCostAp_LanguageText.color = m_initCostApColor;
                    }
                    //行军时间
                    view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(true);
                    RefershMarchTime();
                    OnRefreshTextView(troopNum);

                    if (GuideManager.Instance.IsGuideFightSecondBarbarian)
                    {
                        SetGuideTime();
                    }
                     
                    SetMarchBtnGray(troopNum>0);
                    if (m_OpenPanelData.type != OpenPanelType.Common)
                    {
                        bool isGray;
                        if (m_RallyTroopsProxy.GetRallyDetailEntityByarmIndex(m_troopDataObject.ArmyData.armyIndex) == 0)
                        {
                            isGray = false;
                        }
                        else
                        {
                            isGray = true;
                            view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = LanguageUtils.getText(200089);
                            isRally = true;
                        }
                        SetMarchBtnGray(isGray);
                    }                 
                    LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                }
                else //采集
                {
                    view.m_img_mist_VerticalLayoutGroup.gameObject.SetActive(true);
                    view.m_img_CostAp_VerticalLayoutGroup.gameObject.SetActive(false);

                    if (isRefreshToop)
                    {
                        m_troopDataObject.Update(data.ArmyInfo);
                        m_troopDataObject.CalCollectData();
                    }

                    RefreshArmyDetail(data);
                    // 兵力
                    long troopNum = m_troopDataObject.GetSoldierNum();
                    view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(troopNum);
                    //负载
                    RefreshWeight();;
                
                    //行军时间
                    view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(true);
                    RefershMarchTime();
                    OnRefreshTextView(troopNum);

                    LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                }

                RefreshDifficultTip(rssData);
            }
            else
            {
                view.m_img_mist_VerticalLayoutGroup.gameObject.SetActive(true);
                view.m_img_CostAp_VerticalLayoutGroup.gameObject.SetActive(false);

                if (isRefreshToop)
                {
                    m_troopDataObject.Update(data.ArmyInfo);
                    m_troopDataObject.CalCollectData();
                }

                RefreshArmyDetail(data);
                // 兵力
                long troopNum = m_troopDataObject.GetSoldierNum();
                view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(troopNum);
                //负载
                RefreshWeight();
                //行军时间
                view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(true);
                RefershMarchTime();
                OnRefreshTextView(troopNum);

                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            }
        }

        private void ShowMarchPrePanelOnExpedition(ListView.ListItem listItem)
        {
            view.m_pl_warnTip_LayoutElement.gameObject.SetActive(false);
            Vector2 v2 = m_RssProxy.GetV2(rssId);
            lastV2 = v2;
            var armyData = m_activeTroopData[listItem.index];

            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionRemoveAllSelectMyTroopEffect);

            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionCreateSelectMyTroopEffect, armyData.objectId);

            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionRemoveAllDrawLine);

            if (!v2.Equals(Vector2.zero))
            {
                ExpeditionDrawLineInfo expeditionDrawLineInfo = new ExpeditionDrawLineInfo();
                expeditionDrawLineInfo.objectId = armyData.objectId;
                expeditionDrawLineInfo.targetPos = v2;
                AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionCreateDrawLine, expeditionDrawLineInfo);
            }

            ShowCreateTroopPanel(false);
            SetGoPos(listItem);

            if (drawLineTimer != null)
            {
                drawLineTimer.Cancel();
                drawLineTimer = null;
            }
            drawLineTimer = Timer.Register(1f, () =>
            {
                Vector2 pos = m_RssProxy.GetV2(rssId);
                if (!pos.Equals(Vector2.zero) && pos != lastV2)
                {
                    ExpeditionDrawLineInfo expeditionDrawLineInfo1 = new ExpeditionDrawLineInfo();
                    expeditionDrawLineInfo1.objectId = armyData.objectId;
                    expeditionDrawLineInfo1.targetPos = v2;
                    AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionCreateDrawLine, expeditionDrawLineInfo1);

                    lastV2 = pos;
                }
            }, null, true);

            view.m_img_mist_VerticalLayoutGroup.gameObject.SetActive(true);
            view.m_img_CostAp_VerticalLayoutGroup.gameObject.SetActive(false);
            m_selectTroopIndex = armyData.dataIndex;
            RefreshArmyDetailOnExpeidtion(armyData);

            // 兵力
            view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
            //负载
            RefreshWeight();            
            //行军时间
            view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(true);
            RefershMarchTime();
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            if(rssData != null)
            {
                RefreshDifficultTip(rssData);
            }
        }

        private void RefreshArmyDetail(TroopMainCreateData data)
        {
            //部队详情
            view.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(200059, ClientUtils.FormatComma(data.soldiersNum));

            //英雄名称钱钱钱
            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(data.hero.config.l_nameID);
            //level
            view.m_lbl_level_LanguageText.text = LanguageUtils.getTextFormat(300003, data.hero.level);
            //star
            view.m_UI_Model_HeadStar1.SetShow(data.hero.star > 0);
            view.m_UI_Model_HeadStar2.SetShow(data.hero.star > 1);
            view.m_UI_Model_HeadStar3.SetShow(data.hero.star > 2);
            view.m_UI_Model_HeadStar4.SetShow(data.hero.star > 3);
            view.m_UI_Model_HeadStar5.SetShow(data.hero.star > 4);
            view.m_UI_Model_HeadStar6.SetShow(data.hero.star > 5);
            //技能
            if (m_skillSubViewList == null)
            {
                m_skillSubViewList = new List<UI_Item_CaptainSkill_SubView>();
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill1);
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill2);
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill3);
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill4);
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill5);
            }
            for (int i = 0; i < 5; i++)
            {
                if (i < data.hero.config.skill.Count)
                {
                    m_skillSubViewList[i].gameObject.SetActive(true);
                    m_skillSubViewList[i].SetSkillInfo(data.hero, i, 0);
                }
                else
                {
                    m_skillSubViewList[i].gameObject.SetActive(false);
                }
            }
            //队伍列表
            if (m_ArmyConstituteSubViewList == null)
            {
                m_ArmyConstituteSubViewList = new List<UI_Item_ArmyConstitute_SubView>();
                m_initDetailListHeigh = view.m_c_page2.GetComponent<RectTransform>().rect.height;
                m_initDetailItemHeigh = view.m_pl_allarmys_GridLayoutGroup.GetComponent<RectTransform>().rect.height;
            }
            int showCount = 0;
            if (data.ArmyInfo != null && data.ArmyInfo.soldiers != null)
            {
                showCount = data.ArmyInfo.soldiers.Count;
                int diffNum = data.ArmyInfo.soldiers.Count - m_ArmyConstituteSubViewList.Count;
                if (diffNum > 0)
                {
                    for (int i = 0; i < diffNum; i++)
                    {
                        GameObject obj = GameObject.Instantiate(m_assetDic["UI_Item_ArmyConstitute"], view.m_pl_allarmys_GridLayoutGroup.gameObject.transform);
                        UI_Item_ArmyConstitute_SubView subView = new UI_Item_ArmyConstitute_SubView(obj.GetComponent<RectTransform>());
                        m_ArmyConstituteSubViewList.Add(subView);
                    }
                }
                else if(diffNum < 0)
                {
                    diffNum = m_ArmyConstituteSubViewList.Count - Mathf.Abs(diffNum);
                    for (int i = m_ArmyConstituteSubViewList.Count - 1; i >= diffNum; i--)
                    {
                        m_ArmyConstituteSubViewList[i].SetVisible(false);
                    }
                }
                int j = 0;
                foreach (var soldier in data.ArmyInfo.soldiers.Values)
                {
                    m_ArmyConstituteSubViewList[j].SetVisible(true);
                    int id = m_soldierProxy.GetTemplateId((int)soldier.type, (int)soldier.level);
                    ArmsDefine config = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                    if (config != null)
                    {
                        ClientUtils.LoadSprite(m_ArmyConstituteSubViewList[j].m_img_icon_PolygonImage, config.icon);
                        m_ArmyConstituteSubViewList[j].m_lbl_armyCount_LanguageText.text = ClientUtils.FormatComma(soldier.num);
                    }
                    j++;
                }
            }
            else
            {
                showCount = 0;
                for (int i = 0; i < m_ArmyConstituteSubViewList.Count; i++)
                {
                    m_ArmyConstituteSubViewList[i].SetVisible(false);
                }
            }
            //重设list高度
            if (showCount > 2)
            {
                float height = (int)Mathf.Ceil(showCount/2) * m_ArmyConstituteSubViewList[0].gameObject.GetComponent<RectTransform>().rect.height;
                view.m_c_page2.sizeDelta = new Vector2(view.m_c_page2.sizeDelta.x, m_initDetailListHeigh + height - m_initDetailItemHeigh);
            }
            else
            {
                view.m_c_page2.sizeDelta = new Vector2(view.m_c_page2.sizeDelta.x, m_initDetailListHeigh);
            }
        }

        private void RefreshArmyDetailOnExpeidtion(ArmyData armyData)
        {
            //部队详情
            view.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(200059, ClientUtils.FormatComma(armyData.troopNums));

            var hero = m_heroProxy.GetHeroByID(armyData.heroId);
            //英雄名称钱钱钱
            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(hero.config.l_nameID);
            //level
            view.m_lbl_level_LanguageText.text = LanguageUtils.getTextFormat(300003, hero.level);
            //star
            view.m_UI_Model_HeadStar1.SetShow(hero.star > 0);
            view.m_UI_Model_HeadStar2.SetShow(hero.star > 1);
            view.m_UI_Model_HeadStar3.SetShow(hero.star > 2);
            view.m_UI_Model_HeadStar4.SetShow(hero.star > 3);
            view.m_UI_Model_HeadStar5.SetShow(hero.star > 4);
            view.m_UI_Model_HeadStar6.SetShow(hero.star > 5);
            //技能
            if (m_skillSubViewList == null)
            {
                m_skillSubViewList = new List<UI_Item_CaptainSkill_SubView>();
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill1);
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill2);
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill3);
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill4);
                m_skillSubViewList.Add(view.m_UI_Item_CaptainSkill5);
            }
            for (int i = 0; i < 5; i++)
            {
                if (i < hero.config.skill.Count)
                {
                    m_skillSubViewList[i].gameObject.SetActive(true);
                    m_skillSubViewList[i].SetSkillInfo(hero, i, 2);
                }
                else
                {
                    m_skillSubViewList[i].gameObject.SetActive(false);
                }
            }
            //队伍列表
            if (m_ArmyConstituteSubViewList == null)
            {
                m_ArmyConstituteSubViewList = new List<UI_Item_ArmyConstitute_SubView>();
                m_initDetailListHeigh = view.m_c_page2.GetComponent<RectTransform>().rect.height;
                m_initDetailItemHeigh = view.m_pl_allarmys_GridLayoutGroup.GetComponent<RectTransform>().rect.height;
            }
            int showCount = 0;
            if (armyData.soldiers != null)
            {
                showCount = armyData.soldiers.Count;
                int diffNum = showCount - m_ArmyConstituteSubViewList.Count;
                if (diffNum > 0)
                {
                    for (int i = 0; i < diffNum; i++)
                    {
                        GameObject obj = GameObject.Instantiate(m_assetDic["UI_Item_ArmyConstitute"], view.m_pl_allarmys_GridLayoutGroup.gameObject.transform);
                        UI_Item_ArmyConstitute_SubView subView = new UI_Item_ArmyConstitute_SubView(obj.GetComponent<RectTransform>());
                        m_ArmyConstituteSubViewList.Add(subView);
                    }
                }
                else if (diffNum < 0)
                {
                    diffNum = m_ArmyConstituteSubViewList.Count - Mathf.Abs(diffNum);
                    for (int i = m_ArmyConstituteSubViewList.Count - 1; i >= diffNum; i--)
                    {
                        m_ArmyConstituteSubViewList[i].SetVisible(false);
                    }
                }
                int j = 0;
                foreach (var soldier in armyData.soldiers.Values)
                {
                    m_ArmyConstituteSubViewList[j].SetVisible(true);
                    int id = m_soldierProxy.GetTemplateId((int)soldier.type, (int)soldier.level);
                    ArmsDefine config = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                    if (config != null)
                    {
                        ClientUtils.LoadSprite(m_ArmyConstituteSubViewList[j].m_img_icon_PolygonImage, config.icon);
                        m_ArmyConstituteSubViewList[j].m_lbl_armyCount_LanguageText.text = ClientUtils.FormatComma(soldier.num);
                    }
                    j++;
                }
            }
            else
            {
                showCount = 0;
                for (int i = 0; i < m_ArmyConstituteSubViewList.Count; i++)
                {
                    m_ArmyConstituteSubViewList[i].SetVisible(false);
                }
            }
            //重设list高度
            if (showCount > 2)
            {
                float height = (int)Mathf.Ceil(showCount / 2) * m_ArmyConstituteSubViewList[0].gameObject.GetComponent<RectTransform>().rect.height;
                view.m_c_page2.sizeDelta = new Vector2(view.m_c_page2.sizeDelta.x, m_initDetailListHeigh + height - m_initDetailItemHeigh);
            }
            else
            {
                view.m_c_page2.sizeDelta = new Vector2(view.m_c_page2.sizeDelta.x, m_initDetailListHeigh);
            }
        }

        private void UpdateTimer()
        {
            if(m_troopDataObject != null)
            {
                if (m_resType == 1)
                {
                    RefreshWeight();
                }
                RefershMarchTime();
            }

            if(m_selectTroopIndex > 0)
            {
                if (m_resType == 1)
                {
                    RefreshWeight();
                }
                RefershMarchTime();
            }
        }

        private void RefreshWeight()
        {
            switch(GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        long collectWeight = 0;
                        long totalWeight = 0;                        

                        if (m_multiSelectFlag)
                        {
                            if (m_mainTroopData != null)
                            {
                                int count = m_mainTroopData.GetDataCount();
                                for (int i = 0; i < count; i++)
                                {
                                    var data = m_mainTroopData.GetData(i);
                                    if (data != null)
                                    {
                                        if (m_selectArmyList.Contains(data.id))
                                        {
                                            TroopDataObject troopDataObject = new TroopDataObject();
                                            troopDataObject.Init(data.ArmyInfo);
                                            troopDataObject.CalCollectData();

                                            totalWeight += troopDataObject.GetTotalWeight();
                                            collectWeight += troopDataObject.GetCollectWeight();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            collectWeight = m_troopDataObject.GetCollectWeight();
                            totalWeight = m_troopDataObject.GetTotalWeight();                            
                        }

                        string collect = ClientUtils.FormatComma(collectWeight);
                        string weight= ClientUtils.FormatComma(totalWeight);                        
                        string str = LanguageUtils.getTextFormat(181104, collect, weight);
                        view.m_lbl_weight_LanguageText.text = LanguageUtils.getTextFormat(200047, str);
                        if (m_resType == 1 && !isRally)
                        {
                            SetMarchBtnGray(totalWeight > collectWeight);
                        }
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        long collectWeight = 0;

                        if (m_multiSelectFlag)
                        {
                            foreach (var armyData in m_activeTroopData)
                            {
                                if (m_selectArmyList.Contains(armyData.objectId))
                                {
                                    Dictionary<int, int> soldiers = new Dictionary<int, int>();
                                    foreach (var soldier in armyData.soldiers)
                                    {
                                        soldiers.Add((int)soldier.Key, (int)soldier.Value.num);
                                    }
                                    collectWeight += m_TroopProxy.GetArmyWeight(soldiers, armyData.heroId, armyData.viceId);
                                }
                            }
                        }
                        else
                        {
                            var armyData = GetSelectTroopData();
                            Dictionary<int, int> soldiers = new Dictionary<int, int>();
                            foreach (var soldier in armyData.soldiers)
                            {
                                soldiers.Add((int)soldier.Key, (int)soldier.Value.num);
                            }
                            collectWeight = m_TroopProxy.GetArmyWeight(soldiers, armyData.heroId, armyData.viceId);
                        }
                        
                        var str = LanguageUtils.getTextFormat(181104, 0, ClientUtils.FormatComma(collectWeight));
                        view.m_lbl_weight_LanguageText.text = LanguageUtils.getTextFormat(200047, str);
                    }
                    break;                
            }
        }

        private void SetMarchBtnGray(bool isBool)
        {
            if (isBool != view.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.IsInteractable())
            {
                view.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.interactable = isBool;
                if (isBool)
                {
                    view.m_UI_Model_StandardButton_Yellow.SetGray(false);
                }
                else
                {
                    view.m_UI_Model_StandardButton_Yellow.SetGray(true);
                }
            }
        }

        private void OnRefreshTextView(long troopNum)
        {
            ResetDefText();

            isRally = false;
            if (m_OpenPanelData.type == OpenPanelType.JoinRally)
            {
                if (troopNum > m_OpenPanelData.rallyTroopNum)
                {
                    view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = LanguageUtils.getText(732020);
                    SetDefText();
                    return;
                }
            }

            if (!m_multiSelectFlag)
            {
                if (m_OpenPanelData.type == OpenPanelType.Reinfore)
                {
                    if (TroopHelp.IsHaveState(m_troopDataObject.ArmyData.status, ArmyStatus.GARRISONING) &&
                        m_troopDataObject.ArmyData.targetArg.targetObjectIndex == m_OpenPanelData.id)
                    {
                        view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = LanguageUtils.getText(170005);
                        SetDefText();
                        return;
                    }
                }

                if (m_RallyTroopsProxy.GetRallyDetailEntityByarmIndex(m_troopDataObject.ArmyData.armyIndex) != 0)
                {
                    view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = LanguageUtils.getText(200089);
                    SetDefText();
                    isRally = true;
                    return;
                }
            }

            SetMarchBtnGray(troopNum > 0);
        }

        private void ResetDefText()
        {
            view.m_UI_Model_StandardButton_Yellow.m_lbl_line1_LanguageText.gameObject.SetActive(true);
            view.m_UI_Model_StandardButton_Yellow.m_img_icon2_PolygonImage.gameObject.SetActive(true);
            view.m_UI_Model_StandardButton_Yellow.SetGray(false);
        }

        private void SetDefText()
        {
            view.m_UI_Model_StandardButton_Yellow.m_lbl_line1_LanguageText.gameObject.SetActive(false);
            view.m_UI_Model_StandardButton_Yellow.m_img_icon2_PolygonImage.gameObject.SetActive(false);
            view.m_UI_Model_StandardButton_Yellow.SetGray(true);
        }

        private void RefershMarchTime()
        {
            switch(GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        if (m_OpenPanelData.type == OpenPanelType.JoinRally)
                        {
                            if (m_troopDataObject.GetSoldierNum() > m_OpenPanelData.rallyTroopNum)
                            {
                                return;
                            }
                        }

                        if (m_OpenPanelData.type == OpenPanelType.Reinfore)
                        {
                            if (m_troopDataObject.ArmyData != null)
                            {
                                if (TroopHelp.IsHaveState(m_troopDataObject.ArmyData.status, ArmyStatus.GARRISONING) && m_troopDataObject.ArmyData.targetArg.targetObjectIndex == m_OpenPanelData.id)
                                {
                                    return;
                                }
                            }                            
                        }

                        if (m_troopDataObject.ArmyData != null)
                        {
                            if (m_RallyTroopsProxy.GetRallyDetailEntityByarmIndex(m_troopDataObject.ArmyData.armyIndex) != 0)
                            {
                                return;
                            }
                        }

                        //行军时间
                        view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown(m_troopDataObject.GetMarchTime(rssId, m_targetPos));
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        int times = CalExpeditionMarchTime(GetSelectTroopData());
                        //行军时间
                        view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown(times);
                    }
                    break;
            }
        }

        private int CalExpeditionMarchTime(ArmyData armyData)
        {
            var targetPos = new Vector2(m_targetPos.x / 100, m_targetPos.y / 100);
            targetPos = m_expeditionProxy.ExpeditionPosToWorldPos(targetPos.x, targetPos.y);
            targetPos.x = targetPos.x * 100;
            targetPos.y = targetPos.y * 100;
            var distance = TroopHelp.GetDistance(new Vector2(armyData.go.transform.position.x * 100, armyData.go.transform.position.z * 100), targetPos);
            Dictionary<int, int> soldiers = new Dictionary<int, int>();
            foreach (var soldier in armyData.soldiers)
            {
                soldiers.Add((int)soldier.Key, (int)soldier.Value.num);
            }
            var speed = m_TroopProxy.GetArmySpeed(soldiers, armyData.heroId, armyData.viceId);
            int times = (int)(distance / speed);
            times = times > 0 ? times : 0;
            return times;
        }

        private void SetGuideTime()
        {
            //行军时间
            view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown(5);
        }

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }

            if (drawLineTimer != null)
            {
                drawLineTimer.Cancel();
                drawLineTimer = null;
            }

            if (m_multiMarchDrawLineTimer != null)
            {
                m_multiMarchDrawLineTimer.Cancel();
                m_multiMarchDrawLineTimer = null;
            }
        }

        private void RefreshDifficultTip(MapObjectInfoEntity rssData)
        {
            view.m_pl_warnTip_LayoutElement.gameObject.SetActive(false);
            if (rssData == null || rssData.monsterDefine == null)
            {
                return;
            }

            if (rssData.monsterDefine.type != 1 && rssData.monsterDefine.type != 3)
            {
                return;
            }
            view.m_pl_warnTip_LayoutElement.gameObject.SetActive(true);
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {

                        view.m_lbl_warnTip_LanguageText.text = LanguageUtils.getText(
                            FightHelper.Instance.GetFightingDifficultTip((int)m_troopDataObject.GetSoldierPower(), rssData.monsterDefine.powerAdvise));
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        var armyData = GetSelectTroopData();
                        view.m_lbl_warnTip_LanguageText.text = LanguageUtils.getText(
                            FightHelper.Instance.GetFightingDifficultTip((int)m_TroopProxy.GetTroopPowerByArmyData(armyData), rssData.monsterDefine.powerAdvise));
                    }
                    break;
            }

        }

        #endregion

        #region 多部队行军

        private bool m_multiSelectFlag = false;
        private List<int> m_selectArmyList;
        private List<int> m_lastSelectArmyList;
        private Timer m_multiMarchDrawLineTimer;

        private void refreshMultiToggleState()
        {
            if (m_OpenPanelData.type == OpenPanelType.CreateRally ||
                m_OpenPanelData.type == OpenPanelType.JoinRally)
            {
                return;
            }

            if (GuideProxy.IsGuideing)
            {
                return;
            }

            if (m_lineCount <= 1)
            {
                if (m_multiSelectFlag)
                {
                    view.m_ck_selectAll_GameToggle.isOn = false;
                }

                view.m_ck_selectAll_GameToggle.gameObject.SetActive(false);
            }
            else
            {
                view.m_ck_selectAll_GameToggle.gameObject.SetActive(true);
            }
        }

        private void multiMarchDrawLineUpdate()
        {
            if (rssId > 0)
            {
                switch (GameModeManager.Instance.CurGameMode)
                {
                    case GameModeType.World:
                        {
                            Vector2 targetPos = m_RssProxy.GetV2(rssId);
                            if (!targetPos.Equals(Vector2.zero))
                            {
                                foreach (var armyIndex in m_selectArmyList)
                                {
                                    DrawLineInfo drawLineInfo = new DrawLineInfo();
                                    drawLineInfo.arnyIndex = armyIndex;
                                    drawLineInfo.targetPos = targetPos;
                                    AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateDrawLine, drawLineInfo);
                                }
                            }                            
                        }
                        break;
                    case GameModeType.Expedition:
                        {
                            Vector2 targetPos = m_RssProxy.GetV2(rssId);
                            if (!targetPos.Equals(Vector2.zero))
                            {
                                foreach (var objectId in m_selectArmyList)
                                {
                                    ExpeditionDrawLineInfo expeditionDrawLineInfo = new ExpeditionDrawLineInfo();
                                    expeditionDrawLineInfo.objectId = objectId;
                                    expeditionDrawLineInfo.targetPos = targetPos;
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionCreateDrawLine, expeditionDrawLineInfo);
                                }
                            }                                
                        }
                        break;
                }
            }
        }

        private bool CheckArmyStatus(long armyStatus)
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        if (TroopHelp.IsHaveState(armyStatus, ArmyStatus.PALLY_MARCH) ||
                            TroopHelp.IsHaveState(armyStatus, ArmyStatus.FAILED_MARCH) ||
                            TroopHelp.IsHaveState(armyStatus, ArmyStatus.RALLY_WAIT) ||
                            TroopHelp.IsHaveState(armyStatus, ArmyStatus.RALLY_BATTLE))
                        {
                            return false;
                        }
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        if (TroopHelp.IsHaveState(armyStatus, ArmyStatus.PALLY_MARCH) ||
                            TroopHelp.IsHaveState(armyStatus, ArmyStatus.FAILED_MARCH) ||
                            TroopHelp.IsHaveState(armyStatus, ArmyStatus.RALLY_WAIT) ||
                            TroopHelp.IsHaveState(armyStatus, ArmyStatus.RALLY_BATTLE))
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        private void EnterMultiSelectMode()
        {
            m_selectArmyList.Clear();

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllDrawLine);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveDrawLineCity);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllSelectMyTroopEffect);

                        if (m_mainTroopData != null)
                        {
                            if (m_lastSelectArmyList == null)
                            {
                                int count = m_mainTroopData.GetDataCount();
                                for (int i = 0; i < count; i++)
                                {
                                    var data = m_mainTroopData.GetData(i);
                                    if (data != null)
                                    {
                                        if (CheckArmyStatus((long)data.armyStatus))
                                        {
                                            m_selectArmyList.Add(data.id);
                                            MultiSelectArmyOnWorld(data.id);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                List<int> newAllArmyList = new List<int>();
                                int count = m_mainTroopData.GetDataCount();
                                for (int i = 0; i < count; i++)
                                {
                                    var data = m_mainTroopData.GetData(i);
                                    if (data != null)
                                    {
                                        if (CheckArmyStatus((long)data.armyStatus))
                                        {
                                            newAllArmyList.Add(data.id);
                                        }                                            
                                    }
                                }

                                List<int> newSelectArmyList = new List<int>();
                                foreach (var armyIndex in m_lastSelectArmyList)
                                {
                                    if (newAllArmyList.Contains(armyIndex))
                                    {
                                        newSelectArmyList.Add(armyIndex);
                                    }
                                }

                                foreach (var armyIndex in newSelectArmyList)
                                {
                                    m_selectArmyList.Add(armyIndex);
                                    MultiSelectArmyOnWorld(armyIndex);
                                }
                            }
                        }
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionRemoveAllDrawLine);
                        AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionRemoveAllSelectMyTroopEffect);

                        if (m_activeTroopData != null)
                        {
                            if (m_lastSelectArmyList == null)
                            {
                                foreach (var armyData in m_activeTroopData)
                                {
                                    if (CheckArmyStatus(armyData.armyStatus))
                                    {
                                        m_selectArmyList.Add(armyData.objectId);
                                        MultiSelectArmyOnExpedition(armyData.objectId);
                                    }
                                }
                            }
                            else
                            {
                                List<int> newAllArmyList = new List<int>();
                                foreach (var armyData in m_activeTroopData)
                                {
                                    if (CheckArmyStatus(armyData.armyStatus))
                                    {
                                        newAllArmyList.Add(armyData.objectId);
                                    }
                                }

                                List<int> newSelectArmyList = new List<int>();
                                foreach (var objectId in m_lastSelectArmyList)
                                {
                                    if (newAllArmyList.Contains(objectId))
                                    {
                                        newSelectArmyList.Add(objectId);
                                    }
                                }

                                foreach (var objectId in newSelectArmyList)
                                {
                                    m_selectArmyList.Add(objectId);
                                    MultiSelectArmyOnExpedition(objectId);
                                }
                            }                                
                        }                        
                    }
                    break;
            }

            ShowCreateTroopPanel(false);
            SetSelectImg(null);
            ShowMultiMarChPrePanel(true);
            RefreshMultiMarChPrePanel();

            view.m_list_Troops_ListView.ForceRefresh();

            if (drawLineTimer != null)
            {
                drawLineTimer.Cancel();
                drawLineTimer = null;
            }

            if (m_multiMarchDrawLineTimer == null)
            {
                m_multiMarchDrawLineTimer = Timer.Register(1f, multiMarchDrawLineUpdate, null, true);
            }
        }

        private void ExitMultiSelectMode()
        {
            if (m_lastSelectArmyList == null)
            {
                m_lastSelectArmyList = new List<int>();
            }
            else
            {
                m_lastSelectArmyList.Clear();
            }

            foreach (var id in m_selectArmyList)
            {
                m_lastSelectArmyList.Add(id);
            }

            m_selectArmyList.Clear();

            ShowMultiMarChPrePanel(false);

            view.m_list_Troops_ListView.ForceRefresh();

            if (m_multiMarchDrawLineTimer != null)
            {
                m_multiMarchDrawLineTimer.Cancel();
                m_multiMarchDrawLineTimer = null;
            }

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllDrawLine);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveDrawLineCity);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveAllSelectMyTroopEffect);

                        if (m_troopDataObject != null)
                        {
                            int findIndex = -1;
                            int count = m_mainTroopData.GetDataCount();
                            for (int i = 0; i < count; i++)
                            {
                                TroopMainCreateData data = m_mainTroopData.GetData(i);
                                if (data != null && data.id == m_troopDataObject.id)
                                {
                                    findIndex = i;
                                    break;
                                }
                            }
                            if (findIndex > -1)
                            {
                                ListView.ListItem listItem = view.m_list_Troops_ListView.GetItemByIndex(findIndex);
                                if (listItem != null)
                                {
                                    ClickHead(listItem);
                                }
                            }
                        }
                        else
                        {
                            if (m_mainTroopData.GetDataCount() < m_TroopProxy.GetTroopDispatchNum())
                            {
                                ShowCreateTroopBtn();
                            }
                            else
                            {
                                ListView.ListItem listItem = view.m_list_Troops_ListView.GetItemByIndex(0);
                                if (listItem != null)
                                {
                                    ClickHead(listItem);
                                }
                            }
                        }                        
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionRemoveAllDrawLine);
                        AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionRemoveAllSelectMyTroopEffect);

                        ListView.ListItem listItem = view.m_list_Troops_ListView.GetItemByIndex(0);
                        if (listItem != null)
                        {
                            ClickHead(listItem);
                        }
                    }
                    break;
            }
        }

        private void OnMultiSelectToggleChanged(bool isOn)
        {
            m_multiSelectFlag = isOn;

            if (isOn)
            {
                EnterMultiSelectMode();
            }
            else
            {
                ExitMultiSelectMode();
            }
        }

        private void ShowMultiMarChPrePanel(bool state)
        {
            if (state)
            {
                view.m_btn_info_GameButton.gameObject.SetActive(false);
                view.m_pl_warnTip_LayoutElement.gameObject.SetActive(false);
                view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(false); 
                view.m_img_go_arrowSideR_PolygonImage.gameObject.SetActive(false);

                Vector3 referenceWorldPos = view.m_ck_selectAll_GameToggle.gameObject.transform.position;
                Vector3 referenceScreenPos = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(referenceWorldPos);
                Vector2 referenceLocalPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                        referenceScreenPos,
                                                                        CoreUtils.uiManager.GetUICamera(),
                                                                        out referenceLocalPos);
                RectTransform referenceRect = view.m_ck_selectAll_GameToggle.gameObject.GetComponent<RectTransform>();
                RectTransform goRect = view.m_pl_Go_ArabLayoutCompment.GetComponent<RectTransform>();
                Vector2 goLocalPos = new Vector2(view.m_pl_Go_ArabLayoutCompment.transform.localPosition.x,
                                                 referenceLocalPos.y + goRect.rect.height / 2.0f - referenceRect.rect.height / 2.0f);
                view.m_pl_Go_ArabLayoutCompment.transform.localPosition = goLocalPos;
            }
            else
            {
                view.m_btn_info_GameButton.gameObject.SetActive(true);
                view.m_pl_warnTip_LayoutElement.gameObject.SetActive(true);
                view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(true);
                view.m_img_go_arrowSideR_PolygonImage.gameObject.SetActive(true);
            }
        }

        private void OnMultiListItemClick(ListView.ListItem listItem, bool addFlag)
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        OnMultiListItemClickOnWorld(listItem, addFlag);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        OnMultiListItemClickOnExpedition(listItem, addFlag);
                    }
                    break;
            }
        }

        private void MultiSelectArmyOnWorld(int armyIndex)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateSelectMyTroopEffect, armyIndex);

            Vector2 targetPos = Vector2.zero;
            if (m_OpenPanelData.type == OpenPanelType.JoinRally ||
                m_OpenPanelData.type == OpenPanelType.Reinfore)
            {
                targetPos = m_OpenPanelData.pos;
            }
            else
            {
                targetPos = m_RssProxy.GetV2(rssId);
            }

            if (!targetPos.Equals(Vector2.zero))
            {
                DrawLineInfo drawLineInfo = new DrawLineInfo();
                drawLineInfo.arnyIndex = armyIndex;
                drawLineInfo.targetPos = targetPos;
                AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateDrawLine, drawLineInfo);
            }            
        }

        private void MultiUnSelectArmyOnWorld(int armyIndex)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.MapDeleteDrawLine, armyIndex);
            AppFacade.GetInstance().SendNotification(CmdConstant.MapDeleteSelectMyTroopEffect, armyIndex);
        }

        private void OnMultiListItemClickOnWorld(ListView.ListItem listItem, bool addFlag)
        {
            TroopMainCreateData troopMainCreateData = m_mainTroopData.GetData(listItem.index);
            if (troopMainCreateData == null) return;

            UI_Item_ArmyQueueHeadView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ArmyQueueHeadView>(listItem.go);
            if (itemView == null) return;

            itemView.m_img_select_PolygonImage.gameObject.SetActive(addFlag);
            itemView.m_img_check_PolygonImage.gameObject.SetActive(addFlag);
            itemView.m_pl_time_PolygonImage.gameObject.SetActive(addFlag);

            int armyIndex = troopMainCreateData.id;

            if (addFlag)
            {
                MultiSelectArmyOnWorld(armyIndex);
            }
            else
            {
                MultiUnSelectArmyOnWorld(armyIndex);
            }
        }

        private void MultiSelectArmyOnExpedition(int objectId)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionCreateSelectMyTroopEffect, objectId);

            Vector2 targetPos = m_RssProxy.GetV2(rssId);

            if (!targetPos.Equals(Vector2.zero))
            {
                ExpeditionDrawLineInfo expeditionDrawLineInfo = new ExpeditionDrawLineInfo();
                expeditionDrawLineInfo.objectId = objectId;
                expeditionDrawLineInfo.targetPos = targetPos;
                AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionCreateDrawLine, expeditionDrawLineInfo);
            }
        }

        private void MultiUnSelectArmyOnExpedition(int objectId)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionDeleteDrawLine, objectId);
            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionDeleteSelectMyTroopEffect, objectId);
        }

        private void OnMultiListItemClickOnExpedition(ListView.ListItem listItem, bool addFlag)
        {
            ArmyData armyData = m_activeTroopData[listItem.index];
            if (armyData == null) return;

            UI_Item_ArmyQueueHeadView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ArmyQueueHeadView>(listItem.go);
            if (itemView == null) return;

            itemView.m_img_select_PolygonImage.gameObject.SetActive(addFlag);
            itemView.m_img_check_PolygonImage.gameObject.SetActive(addFlag);
            itemView.m_pl_time_PolygonImage.gameObject.SetActive(addFlag);

            int objectId = armyData.objectId;

            if (addFlag)
            {
                MultiSelectArmyOnExpedition(objectId);
            }
            else
            {
                MultiUnSelectArmyOnExpedition(objectId);
            }
        }

        private void RefreshMultiMarChPrePanel()
        {
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        RefreshMultiMarchPrePanelOnWorld();
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        RefreshMultiMarchPrePanelOnExpedition();
                    }
                    break;
            }
        }

        private void RefreshMultiMarchPrePanelOnWorld()
        {
            if (m_OpenPanelData.type == OpenPanelType.JoinRally)
            {
                MapObjectInfoEntity mapObjectInfoEntity = m_worldProxy.GetWorldMapObjectByRid(m_OpenPanelData.jonRid);
                if (mapObjectInfoEntity != null)
                {
                    rssId = (int)mapObjectInfoEntity.objectId;
                }
            }

            if (m_OpenPanelData.type == OpenPanelType.Reinfore)
            {
                rssId = (int)m_OpenPanelData.reinforceObjectIndex;
            }

            MapObjectInfoEntity rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            if (rssData != null)
            {
                if (rssData.rssType == RssType.Monster ||
                    rssData.rssType == RssType.SummonAttackMonster) //打怪
                {
                    view.m_img_mist_VerticalLayoutGroup.gameObject.SetActive(false);
                    view.m_img_CostAp_VerticalLayoutGroup.gameObject.SetActive(true);

                    long soldierNum = 0;
                    int costMobilityNum = 0;

                    if (m_mainTroopData != null)
                    {
                        int count = m_mainTroopData.GetDataCount();
                        for (int i = 0; i < count; i++)
                        {
                            var data = m_mainTroopData.GetData(i);
                            if (data != null)
                            {
                                if (m_selectArmyList.Contains(data.id))
                                {
                                    TroopDataObject troopDataObject = new TroopDataObject();
                                    troopDataObject.Init(data.ArmyInfo);
                                    troopDataObject.CalCollectData();

                                    soldierNum += troopDataObject.GetSoldierNum();
                                    costMobilityNum += GetCostMobilityNum(troopDataObject);
                                }
                            }
                        }
                    }

                    //兵力
                    view.m_lbl_countCostAp_LanguageText.text = ClientUtils.FormatComma(soldierNum);
                    //体力
                    m_costAp = costMobilityNum;
                    view.m_lbl_valCostAp_LanguageText.text = m_costAp.ToString();

                    if (m_playerProxy.CurrentRoleInfo.actionForce < m_costAp)
                    {
                        view.m_lbl_valCostAp_LanguageText.color = Color.red;
                    }
                    else
                    {
                        view.m_lbl_valCostAp_LanguageText.color = m_initCostApColor;
                    }

                    OnRefreshTextView(soldierNum);

                    if (m_OpenPanelData.type != OpenPanelType.Common)
                    {
                        bool inRallyFlag = false;

                        foreach (var index in m_selectArmyList)
                        {
                            if (m_RallyTroopsProxy.GetRallyDetailEntityByarmIndex(index) != 0)
                            {
                                inRallyFlag = true;
                                break;
                            }
                        }

                        if (inRallyFlag)
                        {
                            isRally = true;
                            view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.text = LanguageUtils.getText(200089);
                            SetMarchBtnGray(true);
                        }
                        else
                        {
                            SetMarchBtnGray(false);
                        }
                    }
                    LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                }
                else //采集
                {
                    view.m_img_mist_VerticalLayoutGroup.gameObject.SetActive(true);
                    view.m_img_CostAp_VerticalLayoutGroup.gameObject.SetActive(false);

                    long soldierNum = 0;

                    if (m_mainTroopData != null)
                    {
                        int count = m_mainTroopData.GetDataCount();
                        for (int i = 0; i < count; i++)
                        {
                            var data = m_mainTroopData.GetData(i);
                            if (data != null)
                            {
                                if (m_selectArmyList.Contains(data.id))
                                {
                                    TroopDataObject troopDataObject = new TroopDataObject();
                                    troopDataObject.Init(data.ArmyInfo);
                                    troopDataObject.CalCollectData();

                                    soldierNum += troopDataObject.GetSoldierNum();
                                }
                            }
                        }
                    }

                    // 兵力
                    view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(soldierNum);
                    //负载
                    RefreshWeight();

                    OnRefreshTextView(soldierNum);

                    LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                }
            }
            else
            {
                view.m_img_mist_VerticalLayoutGroup.gameObject.SetActive(true);
                view.m_img_CostAp_VerticalLayoutGroup.gameObject.SetActive(false);

                long soldierNum = 0;

                if (m_mainTroopData != null)
                {
                    int count = m_mainTroopData.GetDataCount();
                    for (int i = 0; i < count; i++)
                    {
                        var data = m_mainTroopData.GetData(i);
                        if (data != null)
                        {
                            if (m_selectArmyList.Contains(data.id))
                            {
                                TroopDataObject troopDataObject = new TroopDataObject();
                                troopDataObject.Init(data.ArmyInfo);
                                troopDataObject.CalCollectData();

                                soldierNum += troopDataObject.GetSoldierNum();
                            }
                        }
                    }
                }

                // 兵力
                view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(soldierNum);
                //负载
                RefreshWeight();

                OnRefreshTextView(soldierNum);

                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            }
        }

        private void RefreshMultiMarchPrePanelOnExpedition()
        {
            view.m_img_mist_VerticalLayoutGroup.gameObject.SetActive(true);
            view.m_img_CostAp_VerticalLayoutGroup.gameObject.SetActive(false);

            long soldierNum = 0;

            foreach (var armyData in m_activeTroopData)
            {
                if (m_selectArmyList.Contains(armyData.objectId))
                {
                    soldierNum += armyData.troopNums;
                }
            }            

            // 兵力
            view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(soldierNum);
            //负载
            RefreshWeight();

            SetMarchBtnGray(soldierNum > 0);

            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_StandardButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }

        #endregion
    }
}