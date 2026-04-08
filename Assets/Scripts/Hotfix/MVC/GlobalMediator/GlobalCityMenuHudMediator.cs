// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    GlobalCityMenuHudMediator
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
using DG.Tweening;
using ILRuntime.CLR.Utils;

namespace Game
{
    public enum MenuHudType
    {
        Title,//只显示标题
        Button,//只显示按钮
        All,//两个都显示
    }
    public class GlobalCityMenuHudMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "GlobalCityMenuHudMediator";
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private HospitalProxy m_hospitalProxy;
        private StoreProxy m_storeProxy;
        private AllianceProxy m_allianceProxy;
        private CurrencyProxy m_currencyProxy;

        private GameObject m_curtBuilding;
        private HUDUI m_curHud;
        private HUDUI m_curTitleHud;
        private Dictionary<long, GameObject> m_buildingDic = new Dictionary<long, GameObject>();
        private long m_curIndex;
        private string  m_collider;
        private bool m_both = false;//同时执行收集和打开主菜单
        private BuildingInfoEntity m_curBuildingInfo;
        private BuildingTypeConfigDefine m_curBuildingTypeConfig;
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private bool m_assetsReady = false;
        private string m_menuItem;//要引导的菜单项
        #endregion

        //IMediatorPlug needs
        public GlobalCityMenuHudMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        public GlobalCityMenuHudMediator(object viewComponent) : base(NameMediator, null)
        {
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {   CmdConstant.CityAgeChange,
                CmdConstant.ShowBuildingHudMenu,
                CmdConstant.OnShowUI,
                Build_UpGradeBuilding.TagName,
                CmdConstant.CityBuildingLevelUP,
                CmdConstant.CityBuildingStart,
                CmdConstant.ShowBuildingMenuAndFinger,
                CmdConstant.CreateTempBuildEnable,
                CmdConstant.ShowBuildingMenuAndMoveCameraToBuilding,
                CmdConstant.OnCloseBuildingHudMenu,
                CmdConstant.CloseBuildingHudMenu
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ShowBuildingHudMenu:
                    {
                        OnShowBuildingMenu(notification.Body);
                    }
                    break;
                case CmdConstant.CityAgeChange:
                    {
                        CloseBuildingMenu();
                    }
                    break;
                case CmdConstant.CityBuildingLevelUP:
                    {
                        if (m_curHud != null&&!m_curHud.bDispose)
                        {
                            if (notification.Body is long)
                            {
                                long builidngindex = (long)notification.Body;
                                if (builidngindex == m_curBuildingInfo.buildingIndex)
                                {
                                    CloseBuildingMenu(true);
                                }
                            }
                        }
                    }
                    break;
                case CmdConstant.CityBuildingStart:
                    {
                        if (m_curHud != null)
                        {
                            if (notification.Body is long)
                            {
                                long builidngindex = (long)notification.Body;
                                GameObject obj = CityObjData.GetMenuTargetGameObject(builidngindex);
                                GameObject obj1 = CityObjData.GetMenuTitleTargetGameObject(builidngindex);
                                if (obj != null)
                                {
                                    InitValue(builidngindex);
                                    ShowBuildingMenu(obj, obj1);
                                }
                            }
                        }
                    }
                    break;
                case CmdConstant.ShowBuildingMenuAndMoveCameraToBuilding:
                    {
                        BuildingInfoEntity BuildingInfoEntity = notification.Body as BuildingInfoEntity;
                        if (BuildingInfoEntity != null)
                        {
                            long builidngindex = BuildingInfoEntity.buildingIndex;
                            GameObject obj = CityObjData.GetMenuTargetGameObject(builidngindex, false);
                            GameObject obj1 = CityObjData.GetMenuTitleTargetGameObject(builidngindex,false);
                            if (obj != null)
                            {
                                InitValue(builidngindex, true);
                                ShowBuildingMenu(obj, obj1);
                            }
                        }
                    }
                    break;
                case CmdConstant.ShowBuildingMenuAndFinger:
                    {
                        GOScrptGuide goScrptGuide = notification.Body as GOScrptGuide;
                        if (goScrptGuide != null)
                        {
                            long builidngindex = goScrptGuide.param1;
                            m_menuItem = goScrptGuide.menuItemName;
                            BuildingInfoEntity BuildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByindex(builidngindex);
                            AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseBuildingHudMenu);
                            GameObject obj = CityObjData.GetMenuTargetGameObject(builidngindex, false);
                            GameObject obj1 = CityObjData.GetMenuTitleTargetGameObject(builidngindex, false);
                            if (obj != null)
                            {
                                InitValue(builidngindex, true);
                                ShowBuildingMenu(obj, obj1);
                            }
                        }
                    }
                    break;
                case Build_UpGradeBuilding.TagName:
                    {
                        Build_UpGradeBuilding.response req = notification.Body as Build_UpGradeBuilding.response;
                        if (req != null)
                        {
                            BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByindex(req.buildingIndex);
                            if (buildingInfoEntity != null && buildingInfoEntity.finishTime != -1 && !req.immediately)
                            {
                                GameObject obj = CityObjData.GetMenuTargetGameObject(req.buildingIndex);
                                GameObject obj1 = CityObjData.GetMenuTitleTargetGameObject(req.buildingIndex);
                                if (obj != null)
                                {
                                    InitValue(req.buildingIndex);
                                    ShowBuildingMenu(obj, obj1);

                                    //功能介绍引导
                                    if (buildingInfoEntity.type == (int)EnumCityBuildingType.TownCenter)
                                    {
                                        if (buildingInfoEntity.level == 2)
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.UseSpeedItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case CmdConstant.OnShowUI:
                    {

                        UIInfo uiInfo = notification.Body as UIInfo;
                        if (uiInfo != null)
                        {
                            if (uiInfo == UI.s_guideInfo) //如果是新手引导界面 不需要往下执行
                            {
                                return;
                            }
                            else if (uiInfo == UI.s_fingerInfo) //如果是新手引导界面 不需要往下执行
                            {
                                return;
                            }
                            else if (uiInfo == UI.s_reConnecting)
                            {
                                if (GuideProxy.IsGuideing)
                                {
                                    return;
                                }
                            }
                            else if (uiInfo == UI.s_funcGuide)
                            {
                                return;
                            }
                            else if (uiInfo == UI.s_NPCDialog)
                            {
                                return;
                            }
                            else if (uiInfo == UI.s_resShortSpecial)
                            {
                                return;
                            }
                            else if (uiInfo == UI.s_ResShort)
                            {
                                return;
                            }
                            else if (uiInfo == UI.s_playerResUI)
                            {
                                return;
                            }
                            else if (uiInfo == UI.s_AddRes)
                            {
                                return;
                            }
                            else if (uiInfo == UI.s_helpTip)
                            {
                                return;
                            }
                            if (CoreUtils.uiManager.ExistUI(UI.s_worker))
                            {
                                return;
                            }
                        }
                        CloseBuildingMenu(true);
                    }
                    break;
                case CmdConstant.CreateTempBuildEnable:
                    bool v = (bool)notification.Body;
                    CheckBtnState(v);
                    break;
                case CmdConstant.CloseBuildingHudMenu:
                    {
                        CloseBuildingMenu(false);
                    }
                    break;
                default:
                    break;
            }
        }


        #region UI template method


        private void CheckBtnState(bool v)
        {
            if (m_createBuildBtn != null && m_createBuildBtn.m_btn_noTextButton_GameButton != null)
            {
                if (v)
                {
                    m_createBuildBtn.m_btn_noTextButton_GameButton.interactable = false;
                    m_createBuildBtnGray.Gray();

                }
                else
                {
                    m_createBuildBtn.m_btn_noTextButton_GameButton.interactable = true;
                    m_createBuildBtnGray.Normal();
                }

            }

        }

        public override void OpenAniEnd()
        {
        }

        public override void WinFocus()
        {
        }

        public override void WinClose()
        {
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        protected override void InitData()
        {
            m_cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_hospitalProxy = AppFacade.GetInstance().RetrieveProxy(HospitalProxy.ProxyNAME) as HospitalProxy;
            m_storeProxy = AppFacade.GetInstance().RetrieveProxy(StoreProxy.ProxyNAME) as StoreProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
            m_preLoadRes.Add("UI_Model_CommandBtn");
            ClientUtils.PreLoadRes(CoreUtils.uiManager.GetCanvas().gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                m_assetsReady = true;
            });
        }

        #endregion

        /// <summary>
        /// 建筑菜单
        /// </summary>
        /// <param name="body"></param>
        private void OnShowBuildingMenu(object body)
        {
            if (body is string)
            {
                //这里正常传建筑位置 buildname|Collidername  如果是箭塔或者城墙的话会传一个碰撞体的名称进来，需要把菜单放到碰撞体的位置上去
                string[] strArr = (body as string).Split('|');
                long index = 0;
                GameObject go = m_cityBuildingProxy.GetBuildingByName(strArr[0], out index);
                if (go != null)
                {
                    bool same = InitValue(go,false, strArr);
                    if (same)
                    {
                        return;
                    }
                    GameObject followGameObject = CityObjData.GetMenuTargetGameObject(index,false);
                    GameObject followGameObject1 = CityObjData.GetMenuTitleTargetGameObject(index,false);

                    if ((m_curBuildingInfo.type == (long)EnumCityBuildingType.GuardTower))
                    {
                        TownBuilding help = go.GetComponent<TownBuilding>();
                        if (help != null)
                        {
                            string name = string.Empty;
                            if (strArr.Length == 1 || (string.IsNullOrEmpty(strArr[1]) && strArr.Length == 2))
                            {
                                string[] s_followGameObject = (m_cityBuildingProxy.FollowGameObject).Split('|');
                                if (s_followGameObject.Length == 2)
                                {
                                    name = s_followGameObject[1];
                                }

                            }
                            else
                            {
                                name = strArr[1];
                            }
                            if (string.IsNullOrEmpty(name))
                            {
                                foreach (var c in help.colliders)
                                {
                                    followGameObject = c.transform.gameObject;
                                    m_cityBuildingProxy.FollowGameObject = string.Format("{0}|{1}", go.name, c.transform.gameObject.name);
                                    break;
                                }
                            }
                            else
                            {
                                foreach (var c in help.colliders)
                                {
                                    if (c.name == name)
                                    {
                                        followGameObject = c.transform.gameObject;
                                        m_cityBuildingProxy.FollowGameObject = string.Format("{0}|{1}", go.name, c.transform.gameObject.name);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                        ShowBuildingMenu(followGameObject, followGameObject1);
                }
            }
        }
        private void ShowBuildingMenu(GameObject followGameObject, GameObject followGameObject1 = null)
        {
            if (IsSepicalProcess())
            {
                CloseBuildingMenu(true);
                return;
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.BuidingMenuOpen, m_curIndex);
            if (followGameObject1 == null)
            {
                if (m_curHud == null || m_curHud.bDispose)
                {
                    m_curHud = HUDUI
                        .Register(BuildingMenuView.VIEW_NAME, typeof(BuildingMenuView), HUDLayer.citymenu,
                            followGameObject).SetInitCallback((ui) => { OnHudInit(ui, MenuHudType.All); }).SetScaleAutoAnchor(true)
                        .SetCameraLodDist(0, 270f);
                    ClientUtils.hudManager.ShowHud(m_curHud);
                }
                else
                {
                    if (m_curHud != null)
                    {
                        m_curHud.SetTargetGameObject(followGameObject);
                        OnHudInit(m_curHud, MenuHudType.All);
                        if (m_curHud.gameView != null && m_curHud.gameView.gameObject != null)
                        {
                            m_curHud.gameView.gameObject.SetActive(false);
                            m_curHud.gameView.gameObject.SetActive(true);
                        }
                    }
                }
                if (m_curTitleHud != null)
                {
                    m_curTitleHud.Close();
                }
                }
            else
            {
                if (m_curHud == null || m_curHud.bDispose)
                {
                    m_curHud = HUDUI
                        .Register(BuildingMenuView.VIEW_NAME, typeof(BuildingMenuView), HUDLayer.citymenu,
                            followGameObject).SetInitCallback((ui) => { OnHudInit(ui, MenuHudType.Button); }).SetScaleAutoAnchor(true)
                        .SetCameraLodDist(0, 270f);
                    ClientUtils.hudManager.ShowHud(m_curHud);
                }
                else
                {
                    if (m_curHud != null)
                    {
                        m_curHud.SetTargetGameObject(followGameObject);
                        OnHudInit(m_curHud, MenuHudType.Button);
                        if (m_curHud.gameView != null && m_curHud.gameView.gameObject != null)
                        {
                            m_curHud.gameView.gameObject.SetActive(false);
                            m_curHud.gameView.gameObject.SetActive(true);
                        }
                    }
                }
                if (m_curTitleHud == null || m_curTitleHud.bDispose)
                {
                    m_curTitleHud = HUDUI
                        .Register(BuildingMenuView.VIEW_NAME, typeof(BuildingMenuView), HUDLayer.citymenu,
                            followGameObject1).SetInitCallback((ui) => { OnHudInit(ui, MenuHudType.Title); }).SetScaleAutoAnchor(true)
                        .SetCameraLodDist(0, 270f);
                    ClientUtils.hudManager.ShowHud(m_curTitleHud);
                }
                else
                {
                    if (m_curTitleHud != null)
                    {
                        m_curTitleHud.SetTargetGameObject(followGameObject1);
                        OnHudInit(m_curTitleHud, MenuHudType.Title);
                        if (m_curTitleHud.gameView != null && m_curTitleHud.gameView.gameObject != null)
                        {
                            m_curTitleHud.gameView.gameObject.SetActive(false);
                            m_curTitleHud.gameView.gameObject.SetActive(true);
                        }
                    }
                }
            }

        }
        private void CloseBuildingMenu(bool SendNotification = false)
        {
            if (m_curHud != null)
            {
                m_curHud.Close();
            }
            if (m_curTitleHud != null)
            {
                m_curTitleHud.Close();
            }
            Clear();
            if (SendNotification)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseBuildingHudMenu);
            }
        }

        private bool IsSepicalProcess()
        {
            BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByindex(m_curIndex);
            if (info != null)
            {
                //如果是造兵部队 则需要判断是否可以领取
                if (info.type == (int)EnumCityBuildingType.Barracks ||
                    info.type == (int)EnumCityBuildingType.Stable ||
                    info.type == (int)EnumCityBuildingType.ArcheryRange ||
                    info.type == (int)EnumCityBuildingType.SiegeWorkshop)
                {
                    if (m_cityBuildingProxy.GetTrainStatus(m_curIndex) == SoldierTrainStatus.Finished)
                    {
                        ArmyTrainStatusGlobalMediator armyTrainStatusGlobalMediator = AppFacade.GetInstance().RetrieveMediator(ArmyTrainStatusGlobalMediator.NameMediator) as ArmyTrainStatusGlobalMediator;
                        if (armyTrainStatusGlobalMediator != null && armyTrainStatusGlobalMediator.m_huds.ContainsKey(info.buildingIndex))
                        {
                            HUDUI hud = armyTrainStatusGlobalMediator.m_huds[info.buildingIndex];
                            if (hud != null && hud.uiObj != null)
                            {
                                armyTrainStatusGlobalMediator.OnClickGet(hud);
                            }
                        }
                        return true;
                    }
                }
                else if (info.type == (int)EnumCityBuildingType.Hospital)
                {
                    if (!m_cityBuildingProxy.IsUpgrading(m_curIndex) && !m_cityBuildingProxy.IsPlaceStatus(m_curIndex))
                    {
                        HospitalProxy hospitalProxy =
                            AppFacade.GetInstance().RetrieveProxy(HospitalProxy.ProxyNAME) as HospitalProxy;
                        if (hospitalProxy.GetHospitalStatus() == (int)EnumHospitalStatus.Finished)
                        {
                            var sp = new Role_AwardTreatment.request();
                            AppFacade.GetInstance().SendSproto(sp);
                            AppFacade.GetInstance().SendNotification(CmdConstant.AwardTreatment);
                            return true;
                        }
                    }
                }
                else if (m_curBuildingInfo.type == (long)EnumCityBuildingType.Farm || m_curBuildingInfo.type == (long)EnumCityBuildingType.Sawmill || m_curBuildingInfo.type == (long)EnumCityBuildingType.Quarry || m_curBuildingInfo.type == (long)EnumCityBuildingType.SilverMine)
                {
                    BuildingResourcesProxy buildingResourcesProxy = AppFacade.GetInstance().RetrieveProxy(BuildingResourcesProxy.ProxyNAME) as BuildingResourcesProxy;
                    GlobalResourceMediator globalCityHudMediator = AppFacade.GetInstance().RetrieveMediator(GlobalResourceMediator.NameMediator) as GlobalResourceMediator;
                    Dictionary<long, float> rss = globalCityHudMediator.GetRssDicByType((EnumCityBuildingType)m_curBuildingInfo.type);
                    if (rss != null && rss.ContainsKey(m_curBuildingInfo.buildingIndex))
                    {
                        BuildingResourcesProduceDefine define = buildingResourcesProxy.BuildingRssDefine.Find((i) => { return i.type == m_curBuildingInfo.type && i.level == m_curBuildingInfo.level; });
                        if (define != null && rss[m_curBuildingInfo.buildingIndex] >= define.gatherMin)
                        {
                            globalCityHudMediator.OnHarvestRss((EnumCityBuildingType)m_curBuildingInfo.type);
                            if (!m_both)
                            {
                                return true;
                            }
                        }
                    }
                    if (globalCityHudMediator.m_guideForceShowResCollectHud != null && !globalCityHudMediator.m_guideForceShowResCollectHud.bDispose)
                    {
                        globalCityHudMediator.OnHarvestRss((EnumCityBuildingType)m_curBuildingInfo.type);
                        if (!m_both)
                        {
                            return true;
                        }
                    }
                }
                else if (m_curBuildingInfo.type == (long)EnumCityBuildingType.Academy)
                {
                    QueueInfo technologieQueue = m_playerProxy.GetTechnologieQueue();

                    if (technologieQueue != null && technologieQueue.HasTechnologyType && technologieQueue.technologyType > 0 && technologieQueue.finishTime == -1)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.AwardTechnology, technologieQueue.technologyType);
                        if (string.Equals(m_menuItem, "study"))
                        {
                            if (!GuideProxy.IsGuideing)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                } else if (m_curBuildingInfo.type == (long)EnumCityBuildingType.Smithy)
                {
                    if (m_playerProxy.HasCompleteProduceItems())
                    {
                        ItemCollectGlobalMediator itemCollectGlobalMediator =
                            AppFacade.GetInstance().RetrieveMediator(ItemCollectGlobalMediator.NameMediator) as
                                ItemCollectGlobalMediator;
                        itemCollectGlobalMediator.GetItemByBuildIndex(m_curBuildingInfo.buildingIndex);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 暂存一些参数
        /// </summary>
        /// <param name="go"></param>
        private bool InitValue(GameObject go, bool both, string[] strattr )
        {
            bool same = false;
            long curIndex = 0;
            m_both = both;
            m_curtBuilding = go;

            m_buildingDic = m_cityBuildingProxy.BuildObjDic;
            foreach (KeyValuePair<long, GameObject> kvp in m_buildingDic)
            {
                if (kvp.Value == m_curtBuilding)
                {
                    curIndex = kvp.Key;
                    break;
                }
            }
            BuildingInfoEntity temp = m_cityBuildingProxy.GetBuildingInfoByindex(curIndex);
            if (curIndex == m_curIndex )
            {
                if (temp.type != (long)EnumCityBuildingType.GuardTower)
                {
                    return true;
                } 
                else
                {
                    if (strattr.Length == 2&&! string.IsNullOrEmpty(strattr[1])&& string.Equals(strattr[1],m_collider) || string.Equals(strattr[1], "groundCollider"))
                    {
                        return true;
                    }
                }
            }
            m_curIndex = curIndex;
            m_curBuildingInfo = m_cityBuildingProxy.GetBuildingInfoByindex(m_curIndex);
            m_curBuildingTypeConfig = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>((int)m_curBuildingInfo.type);
            if (strattr.Length == 2)
            {
                m_collider = strattr[1];  
            }
            return false;
        }
        private void InitValue(long buildingIndex, bool both = false)
        {
            m_both = both;
            m_curIndex = buildingIndex;
            m_curBuildingInfo = m_cityBuildingProxy.GetBuildingInfoByindex(m_curIndex);
            m_curBuildingTypeConfig =
                CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>((int)m_curBuildingInfo.type);
        }

        private UI_Model_CommandBtnView CreateMenuItem(RectTransform rectTransform, int funindex)
        {
            GameObject tmp = null;
            int childCount = rectTransform.childCount;
            if (childCount == 0)
            {
                tmp = CoreUtils.assetService.Instantiate(m_assetDic["UI_Model_CommandBtn"]);
                tmp.transform.SetParent(rectTransform);
                tmp.transform.localScale = Vector3.one;
                tmp.transform.localPosition = Vector3.zero;
            }
            else if (rectTransform.childCount == 1)
            {
                tmp = rectTransform.GetChild(0).gameObject;
                tmp.transform.localPosition = Vector3.zero;
            }
            else
            {
            }
            tmp.name = funindex.ToString();
            tmp.gameObject.SetActive(true);
            UI_Model_CommandBtnView CommandBtnView =
                MonoHelper.AddHotFixViewComponent<UI_Model_CommandBtnView>(tmp);
            InitCommandBtnView(funindex, CommandBtnView);
            return CommandBtnView;
        }

        private GrayChildrens m_createBuildBtnGray;
        private UI_Model_CommandBtnView m_createBuildBtn;


        private void IninMenuButtonUI(BuildingMenuView view)
        {
            List<int> menuButtons = m_curBuildingTypeConfig.menuButtons;
            menuButtons = m_cityBuildingProxy.GetCurmenuButtons(m_curIndex, m_curBuildingTypeConfig);
            //            Debug.Log("建筑菜单数量: "+menuButtons.Count);
            m_createBuildBtn = null;
            m_createBuildBtnGray = null;
            switch (menuButtons.Count)
            {
                case 1:
                    {
                        view.m_UI_Item_FeatureBtns.m_pl_tp11.gameObject.SetActive(true);
                        view.m_UI_Item_FeatureBtns.m_pl_tp21.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp22.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp31.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp32.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp33.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp41.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp42.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp43.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp44.gameObject.SetActive(false);

                        CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp11, menuButtons[0]);

                        break;
                    }
                case 2:
                    {
                        view.m_UI_Item_FeatureBtns.m_pl_tp11.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp21.gameObject.SetActive(true);
                        view.m_UI_Item_FeatureBtns.m_pl_tp22.gameObject.SetActive(true);
                        view.m_UI_Item_FeatureBtns.m_pl_tp31.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp32.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp33.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp41.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp42.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp43.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp44.gameObject.SetActive(false);

                        if (menuButtons.Contains(1))
                        {
                            view.m_pl_Title_UIDefaultValue.gameObject.SetActive(false);
                        }

                        var btn1 = CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp21, menuButtons[0]);
                        var btn2 = CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp22, menuButtons[1]);

                        bool needCheckBtn = false;

                        if (menuButtons[0] == 2)
                        {
                            m_createBuildBtn = btn1;
                            m_createBuildBtnGray = btn1.m_btn_noTextButton_GameButton.GetComponent<GrayChildrens>();
                            needCheckBtn = true;
                        } else if (menuButtons[1] == 2)
                        {
                            m_createBuildBtn = btn2;
                            m_createBuildBtnGray = btn2.m_btn_noTextButton_GameButton.GetComponent<GrayChildrens>();
                            needCheckBtn = true;
                        }

                        if (needCheckBtn)
                        {
                            var md = AppFacade.GetInstance().RetrieveMediator(CityGlobalMediator.NameMediator) as
                                CityGlobalMediator;
                            CheckBtnState(md.IsGridUsed);
                        }

                        break;
                    }
                case 3:
                    {
                        view.m_UI_Item_FeatureBtns.m_pl_tp11.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp21.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp22.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp31.gameObject.SetActive(true);
                        view.m_UI_Item_FeatureBtns.m_pl_tp32.gameObject.SetActive(true);
                        view.m_UI_Item_FeatureBtns.m_pl_tp33.gameObject.SetActive(true);
                        view.m_UI_Item_FeatureBtns.m_pl_tp41.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp42.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp43.gameObject.SetActive(false);
                        view.m_UI_Item_FeatureBtns.m_pl_tp44.gameObject.SetActive(false);


                        CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp31, menuButtons[0]);
                        CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp32, menuButtons[1]);
                        CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp33, menuButtons[2]);

                        break;
                    }
                case 4:
                    view.m_UI_Item_FeatureBtns.m_pl_tp11.gameObject.SetActive(false);
                    view.m_UI_Item_FeatureBtns.m_pl_tp21.gameObject.SetActive(false);
                    view.m_UI_Item_FeatureBtns.m_pl_tp22.gameObject.SetActive(false);
                    view.m_UI_Item_FeatureBtns.m_pl_tp31.gameObject.SetActive(false);
                    view.m_UI_Item_FeatureBtns.m_pl_tp32.gameObject.SetActive(false);
                    view.m_UI_Item_FeatureBtns.m_pl_tp33.gameObject.SetActive(false);
                    view.m_UI_Item_FeatureBtns.m_pl_tp41.gameObject.SetActive(true);
                    view.m_UI_Item_FeatureBtns.m_pl_tp42.gameObject.SetActive(true);
                    view.m_UI_Item_FeatureBtns.m_pl_tp43.gameObject.SetActive(true);
                    view.m_UI_Item_FeatureBtns.m_pl_tp44.gameObject.SetActive(true);
                    CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp41, menuButtons[0]);
                    CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp42, menuButtons[1]);
                    CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp42, menuButtons[2]);
                    CreateMenuItem(view.m_UI_Item_FeatureBtns.m_pl_tp42, menuButtons[3]);
                    break;
                default:
                    Debug.Log("not find type ");
                    ClientUtils.hudManager.CloseAllFromSingleUIInfo(HUDLayer.citymenu);
                    break;
            }
        }

        /// <summary>
        /// 菜单按钮事件处理
        /// </summary>
        /// <param name="func"></param>
        /// <param name="CommandBtnView"></param>
        private void InitCommandBtnView(int func, UI_Model_CommandBtnView CommandBtnView)
        {
            BuildingMenuDataDefine BuildingMenuData = CoreUtils.dataService.QueryRecord<BuildingMenuDataDefine>(func);

            if (BuildingMenuData == null)
            {
                Debug.LogError("not find menu id:" + func);
                return;
            }
            ClientUtils.LoadSprite(CommandBtnView.m_btn_noTextButton_PolygonImage, BuildingMenuData.btnIcon);
            if (m_curIndex == 0)
            {
                return;
            }
            //            Debug.Log(BuildingMenuData.func);
            CommandBtnView.m_btn_noTextButton_GameButton.onClick.RemoveAllListeners();
            CommandBtnView.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
            {
                switch (BuildingMenuData.func)
                {
                    case "openBuildingInfo":
                        if (m_curIndex != 0)
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_buildingDesc, null, m_curIndex);
                        }
                        break;
                    case "openBuildingUpdata":
                    case "openBuildingUpdataAge":
                        {
                            if (m_curIndex != 0)
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_buildingUpdate, null, m_curIndex);
                            }
                        }
                        break;
                    case "openBuildingSpeedUp":
                        {
                            if (m_curBuildingInfo != null)
                            {
                                if (m_cityBuildingProxy.IsStudding(m_curIndex))
                                {
                                    OnQueueInfoSpeedUp();
                                }
                                else
                                {
                                    m_cityBuildingProxy.AddBuildingUpdateSpeed(m_curBuildingInfo);
                                }
                            }
                            break;
                        }
                    case "openUI":
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.OpenUI, BuildingMenuData.param);
                        }
                        break;
                    case "drill":
                        {
                            if (m_curIndex != 0)
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_trainArmy, null, m_curIndex);
                            }
                        }
                        break;
                    case "harvestFood":
                        if (m_curIndex != 0)
                        {
                            OnHarvestRss(EnumCurrencyType.food);
                        }
                        return;
                    case "harvestWood":
                        if (m_curIndex != 0)
                        {
                            OnHarvestRss(EnumCurrencyType.wood);
                        }
                        return;
                    case "harvestStone":
                        if (m_curIndex != 0)
                        {
                            OnHarvestRss(EnumCurrencyType.stone);
                        }
                        return;
                    case "harvestGold":
                        if (m_curIndex != 0)
                        {
                            OnHarvestRss(EnumCurrencyType.gold);
                        }
                        return;
                    case "openWorkHouse":
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_worker);
                            CloseBuildingMenu(true);
                        }
                        break;
                    case "cure": //治疗
                        {
                            if (m_curIndex != 0)
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_hospitalInfo, null, m_curIndex);
                            }
                        }
                        break;
                    case "study": //学院
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_ResearchMain);
                        }
                        break;
                    case "buildingClose":
                        AppFacade.GetInstance().SendNotification(CmdConstant.CreateTempBuildNO);
                        break;
                    case "buildingCreate":
                        {
                            FuncBuildingCreate();
                        }
                        break;
                    case "destoryBuilding":
                        {
                            OnDestroyBuilding();
                        }
                        break;
                    case "ordinaryShop": //商店
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_store);
                        }
                        break;
                    case "openMileStone": // 打开纪念碑
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_monument);
                        }
                        break;
                    case "tavernCall": //酒馆招募
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_tavernSummon);
                        }
                        break;
                    case "openMysteryStore": //神秘商店
                        {
                            m_storeProxy.OpenMysteryStore();
                        }
                        break;
                    case "openFreight": //资源运输
                        {
                            if (SystemOpen.IsSystemClose(EnumSystemOpen.alliance))
                            {
                                return;
                            }
                            if (m_allianceProxy.HasJionAlliance())
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceMain, null, true);
                            }
                            else
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
                            }
                        }
                        break;
                    case "openWar"://联盟战争
                        {
                            if (m_allianceProxy.HasJionAlliance())
                            {
                                RallyTroopsProxy rallyTroopsProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
                                CoreUtils.uiManager.ShowUI(UI.s_AlianceWar);
                            }
                            else
                            {
                                Tip.CreateTip(181265, Tip.TipStyle.Middle).Show();
                                AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseBuildingHudMenu);
                            }
                        }
                        break;
                    case "openVipStore":
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_VipStore);
                        }
                        break;
                    case "openAllianceCenter":
                        {
                            if (m_allianceProxy.HasJionAlliance())
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_reinforcements);
                            }
                            else
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
                            }
                        }
                        break;
                    case "openEquipMaterial":
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_Material);
                        }
                        break;
                    case "openEquipMake":
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_Equip);

                        }
                        break;
                    default:
                        Debug.LogFormat("not find type:{0}", BuildingMenuData.func);
                        break;
                }
            });

            if (string.Equals(m_menuItem, BuildingMenuData.func))
            {
                if (!GuideProxy.IsGuideing)
                {
                    FingerTargetParam param = new FingerTargetParam();
                    param.AreaTarget = CommandBtnView.gameObject;
                    param.ArrowDirection = (int)EnumArrorDirection.Up;
                    CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                }
            }
        }
        #region 建造菜单行为跳转

        private bool FuncBuildingCreate()
        {
            bool success = false;
            if (m_curBuildingInfo != null)
            {
                BuildingLevelDataDefine buildingLevelDataDefine = m_cityBuildingProxy.BuildingLevelDataBylevel(m_curBuildingInfo.type, 1);
                if (buildingLevelDataDefine != null)
                {
                    bool IsbuildQueueleisur = false;
                    if (!m_cityBuildingProxy.CanRemoveBuildType(m_curBuildingInfo.type))
                    {
                        if (m_cityBuildingProxy.IsbuildQueueleisur(buildingLevelDataDefine.buildingTime))
                        {
                            IsbuildQueueleisur = true;
                        }
                    }
                    else
                    {
                        IsbuildQueueleisur = true;
                    }

                    if (IsbuildQueueleisur)
                    {     //新建建筑菜单确认
                        if (buildingLevelDataDefine.coin <= m_playerProxy.CurrentRoleInfo.gold &&
                            buildingLevelDataDefine.stone <= m_playerProxy.CurrentRoleInfo.stone &&
                            buildingLevelDataDefine.wood <= m_playerProxy.CurrentRoleInfo.wood &&
                            buildingLevelDataDefine.food <= m_playerProxy.CurrentRoleInfo.food)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.CreateTempBuildYes);

                        }
                        else
                        {
                            m_currencyProxy.LackOfResources(buildingLevelDataDefine.food, buildingLevelDataDefine.wood, buildingLevelDataDefine.stone, buildingLevelDataDefine.coin);
                        }

                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_worker, null, buildingLevelDataDefine.buildingTime);
                    }
                }
            }
            return success;
        }
        #endregion

        private void OnHudInit(HUDUI info, MenuHudType menuHudType)
        {
            if (m_curBuildingInfo == null)
            {
                return;
            }
            BuildingMenuView buildingMenuViewview = info.gameView as BuildingMenuView;
            if (buildingMenuViewview != null)
            {
                switch (menuHudType)
                {
                    case MenuHudType.All:
                        buildingMenuViewview.m_pl_Title_UIDefaultValue.gameObject.SetActive(true);
                        buildingMenuViewview.m_UI_Item_FeatureBtns.gameObject.SetActive(true);
                        break;
                    case MenuHudType.Button:
                        buildingMenuViewview.m_pl_Title_UIDefaultValue.gameObject.SetActive(false);
                        buildingMenuViewview.m_UI_Item_FeatureBtns.gameObject.SetActive(true);
                        break;
                    case MenuHudType.Title:
                        buildingMenuViewview.m_pl_Title_UIDefaultValue.gameObject.SetActive(true);
                        buildingMenuViewview.m_UI_Item_FeatureBtns.gameObject.SetActive(false);
                        break;
                }

                buildingMenuViewview.m_lbl_buildingName_LanguageText.text = LanguageUtils.getText(m_curBuildingTypeConfig.l_nameId);
                if (!m_cityBuildingProxy.CanUpGradeBuildType(m_curBuildingInfo.type))
                {
                    buildingMenuViewview.m_lbl_level_LanguageText.text = string.Empty;
                    buildingMenuViewview.m_img_levelBg_PolygonImage.gameObject.SetActive(false);
                }
                else
                {
                    if (m_curBuildingInfo.level == 0)
                    {
                        buildingMenuViewview.m_lbl_level_LanguageText.text = string.Empty;
                        buildingMenuViewview.m_img_levelBg_PolygonImage.gameObject.SetActive(false);
                    }
                    else
                    {
                        buildingMenuViewview.m_img_levelBg_PolygonImage.gameObject.SetActive(true);
                        buildingMenuViewview.m_lbl_level_LanguageText.text = LanguageUtils.getTextFormat(180306, m_curBuildingInfo.level);
                    }
                }

                IninMenuButtonUI(buildingMenuViewview);
                m_menuItem = string.Empty;
            }
        }

        private void OnHarvestRss(EnumCurrencyType type)
        {
            BuildingResourcesProxy rssProxy =
                AppFacade.GetInstance().RetrieveProxy(BuildingResourcesProxy.ProxyNAME) as BuildingResourcesProxy;
            CurrencyProxy currencyProxy =
                AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            List<BuildingInfoEntity> building;
            Dictionary<long, float> rss;
            switch (type)
            {
                case EnumCurrencyType.food:
                    building = rssProxy.FoodBuilding;
                    rss = rssProxy.FoodRss;
                 //   CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[0]);
                    break;
                case EnumCurrencyType.wood:
                    building = rssProxy.WoodBuilding;
                    rss = rssProxy.WoodRss;
                 //   CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[1]);
                    break;
                case EnumCurrencyType.stone:
                    building = rssProxy.StoneBuilding;
                    rss = rssProxy.StoneRss;
                  //  CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[2]);
                    break;
                case EnumCurrencyType.gold:
                    building = rssProxy.GoldBuilding;
                    rss = rssProxy.GoldRss;
                  //  CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[3]);
                    break;
                default:
                    building = rssProxy.FoodBuilding;
                    rss = rssProxy.FoodRss;
                    break;
            }
            //rssProxy.UpdateSingleRss(building, rss);

            if(rss.Count == 0)
            {
                Debug.LogWarning("服务器数据还没下发");
                return;
            }
            if (rss[m_curIndex] >= 0)
            {
                var req = new Build_GetBuildResources.request
                {
                    buildingIndexs = new List<long>(),
                };
                req.buildingIndexs.Add(m_curIndex);
                long num = rssProxy.GetCurrencyNum(m_curBuildingInfo);
                if (num != 0)
                {
                    rss[m_curIndex] = 0;
                }

                OnHarvestHud(m_buildingDic[m_curIndex], num);
                AppFacade.GetInstance().SendSproto(req);
                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                mt.FlyUICurrencyFromWorld(type, num, m_buildingDic[m_curIndex].transform.position);
            }
            else
            {
                Debug.LogWarning("已经采集过了");
            }
        }

        private void OnHarvestHud(GameObject go, long num)
        {
            HUDUI.Register("UI_Hud_HarvestNum", typeof(UI_Hud_HarvestNumView), HUDLayer.city, go).SetInitCallback(
                (ui) =>
                {
                    UI_Hud_HarvestNumView itemView =
                        MonoHelper.AddHotFixViewComponent<UI_Hud_HarvestNumView>(ui.uiObj);
                    itemView.m_lbl_languageText_LanguageText.text = num.ToString("N0");
                    itemView.m_lbl_languageText_LanguageText.rectTransform.DOLocalMoveY(90, 1).OnComplete(() =>
                    {
                        ui.Close();
                    });
                }).Show();
        }

        //队列加速
        private void OnQueueInfoSpeedUp()
        {
            SpeedUpData speedUpData = new SpeedUpData();
            long serverTime = ServerTimeModule.Instance.GetServerTime();
            switch (m_curBuildingInfo.type)
            {
                case (int)EnumCityBuildingType.Hospital:
                    {
                        if (m_playerProxy.CurrentRoleInfo.treatmentQueue.finishTime <= serverTime)
                        {
                            return;
                        }
                        speedUpData.type = EnumSpeedUpType.heal;
                        speedUpData.queue = m_playerProxy.CurrentRoleInfo.treatmentQueue;
                        CivilizationDefine civilizationDefine = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_playerProxy.GetCivilization());
                        if (civilizationDefine != null)
                        {
                            speedUpData.iconRes = RS.HospitalMarkFrame[civilizationDefine.hospitalMark];
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.Academy:
                    {
                        ResearchProxy resProxy = AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;
                        if (resProxy.GetCrrTechnologying().finishTime <= serverTime)
                        {
                            return;
                        }
                        speedUpData.type = EnumSpeedUpType.research;
                        speedUpData.queue = resProxy.GetCrrTechnologying();
                        var dinfo = resProxy.GetTechnologyList((int)speedUpData.queue.technologyType)[0];
                        var isSoldierStudy1 = resProxy.IsSoldierRes(dinfo.ID);
                        var spImg = isSoldierStudy1 != null ? isSoldierStudy1.icon : dinfo.icon;
                        speedUpData.iconRes = spImg;
                    }
                    break;
                case (int)EnumCityBuildingType.ArcheryRange:
                case (int)EnumCityBuildingType.Barracks:
                case (int)EnumCityBuildingType.SiegeWorkshop:
                case (int)EnumCityBuildingType.Stable:
                    speedUpData.type = EnumSpeedUpType.train;
                    TrainProxy trainProxy = AppFacade.GetInstance().RetrieveProxy(TrainProxy.ProxyNAME) as TrainProxy;
                    QueueInfo queueInfo = trainProxy.GetTrainInfo(m_curIndex);
                    if (queueInfo == null || queueInfo.finishTime <= serverTime)
                    {
                        return;
                    }
                    speedUpData.type = EnumSpeedUpType.train;
                    speedUpData.queue = queueInfo;
                    SoldierProxy soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
                    int tempId = soldierProxy.GetTemplateId((int)queueInfo.armyType, (int)queueInfo.newArmyLevel);
                    ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                    speedUpData.iconRes = define.icon;
                    speedUpData.cancelCallback = () =>
                    {
                        string name = LanguageUtils.getText(define.l_armsID);
                        ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                        string str = LanguageUtils.getTextFormat(192031, name, config.trainingTerminate / 10);
                        Alert.CreateAlert(str, LanguageUtils.getText(192030)).SetLeftButton().SetRightButton(() =>
                        {
                            //发包
                            var sp = new Role_TrainEnd.request();
                            sp.type = queueInfo.armyType;
                            sp.buildingIndex = queueInfo.buildingIndex;
                            AppFacade.GetInstance().SendSproto(sp);
                            AppFacade.GetInstance().SendNotification(CmdConstant.CloseAddSpeed);
                        }).Show();
                    };
                    break;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.SpeedUp, speedUpData);
        }
        private void Clear()
        {
            m_curtBuilding = null;
            m_curHud = null;
            m_curIndex = 0;
            m_both = false;
            m_curBuildingInfo = null;
            m_curBuildingTypeConfig = null;
            m_menuItem = string.Empty;
            m_collider = string.Empty;
        }

        private void OnDestroyBuilding()
        {
            int percent = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).destoryBuildingScale/10;
            string buildingName = LanguageUtils.getText(m_curBuildingTypeConfig.l_nameId);
            BuildingLevelDataDefine define = m_cityBuildingProxy.BuildingLevelDataBylevel(m_curBuildingInfo.type, 1);

            Alert.CreateAlert(LanguageUtils.getTextFormat(180367,buildingName,percent),LanguageUtils.getText(180366)).SetRightButton(()=>
            {
                CoreUtils.audioService.PlayOneShot("Sound_Ui_DestroyPlant");
                Vector3 buildingPos = m_curtBuilding.transform.position;
                //播放光效
                CoreUtils.assetService.Instantiate("build_3003", (go)=>
                {
                    go.transform.position = buildingPos;
                    go.transform.localScale = Vector3.one;
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                    if (define.food > 0)
                    {
                        mt.FlyUICurrencyFromWorld(EnumCurrencyType.food, (long)(define.food * percent / 100f), buildingPos);
                    }
                    if (define.wood > 0)
                    {
                        mt.FlyUICurrencyFromWorld(EnumCurrencyType.wood, (long)(define.wood * percent / 100f), buildingPos);
                    }
                    if (define.stone > 0)
                    {
                        mt.FlyUICurrencyFromWorld(EnumCurrencyType.stone, (long)(define.stone * percent / 100f), buildingPos);
                    }
                    if (define.coin > 0)
                    {
                        mt.FlyUICurrencyFromWorld(EnumCurrencyType.gold, (long)(define.coin * percent / 100f), buildingPos);
                    }
                    if (define.denar > 0)
                    {
                        mt.FlyUICurrencyFromWorld(EnumCurrencyType.denar, (long)(define.denar * percent / 100f), buildingPos);
                    }
                });
                Build_DismantleBuilding.request req = new Build_DismantleBuilding.request();
                req.buildingIndex = m_curIndex;
                AppFacade.GetInstance().SendSproto(req);
                AppFacade.GetInstance().SendNotification(CmdConstant.RemoveBuild, m_curBuildingInfo);
            }).SetLeftButton().Show();
        }
    }
}
