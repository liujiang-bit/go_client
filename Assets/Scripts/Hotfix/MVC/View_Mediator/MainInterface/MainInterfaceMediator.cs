// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月26日
// Update Time         :    2019年12月26日
// Class Description   :    MainInterfaceMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using System;
using Hotfix;
using UnityEngine.UI;
using SprotoType;
using Data;
using System.Linq;
using ILRuntime.Runtime;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Game
{
    //主界面的各模块UI
    //按照UI_IF_MainInterface的模块顺序排序
    //https://docs.google.com/spreadsheets/d/1QWPVgLknsuFbiDYbmWvgRVO3bDj5kDZieHJdevanQuU/edit?usp=sharing
    public enum EnumMainModule
    {
        None = 0,
        CurAge = 1,
        Events = 2,
        HotTask = 4,
        Chat = 8,
        Features = 16,
        FlagWarn = 32,
        Queue = 64,
        Position = 256,
        Map = 512,
        LodMenu = 1024,
        PlayerPowerInfo = 2048,
        PlayerResources = 4096,
        SearchOrBuild = 8192,
        CityOrWorld = 16384,
        Server = 32768,
        Mail = 65536,
        AliGuide = 131072,
        All = -1,
    }

    public enum LodMenuToggle
    {
        None, //都不显示
        Alliance, //联盟
        Explore, //探索
        Resoureces, //资源
        Markers //标记
    }

    public enum OpenLodUILevel
    {
        city_bound,
        dispatch,
        TacticsToStrategy1,
        TacticsToStrategy2,
        strategic_min,
        max,
        limit_max
    }

    public class MainInterfaceMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "MainInterfaceMediator";

        //proxies
        private CurrencyProxy m_CurrencyProxy;
        private PlayerProxy m_playerProxy;
        private EmailProxy m_emailProxy;
        private TaskProxy m_taskProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private GuideProxy m_guideProxy;
        private ChatProxy m_chatProxy;
        private AllianceProxy m_allianceProxy;
        private RallyTroopsProxy m_rallyTroopsProxy;
        private HeroProxy m_heroProxy;
        private BagProxy m_bagProxy;
        private CityBuffProxy m_cityBuffProxy;
        private MapMarkerProxy m_mapMarkerProxy;

        private List<string> m_preLoadRes = new List<string>();

        private long m_mainLineTaskId; //当前主线任务
        private TaskMainDefine m_curTaskMain = new TaskMainDefine(); //当前主线任务
        private long m_chapterId; //当前章节 
        private TaskChapterDataDefine m_curTaskChapterData = new TaskChapterDataDefine(); //当前章节
        private int m_taskchapterCount = 0; //章节任务数量
        private int m_taskchapterFinishCount = 0; //章节已完成任务数量
        private List<TaskData> m_TaskContentList = new List<TaskData>(); //列表任务数据
        private bool taskAnimShow = false;//正在表现任务动画
        private bool waitrefreshTaskView= false;//等待刷新任务界面
        private ListView.ListItem m_deleteItem;//正在表现的任务item
        private float m_deleteTime;//删除动画时间
        private float m_height;

        private Timer m_timer;

        private float m_cityInsideDxf;
        private GlobalViewLevelMediator m_viewLevelMediator;

        private float[] m_openLodUILevel = new float[7];
        private const int fontsize = 18;

        private GameObject m_objCityBuff;//城市buff
        private GameEventGlobalMediator m_gameEventGlobalMediator;
        private int m_lastLighIndex;

        #endregion

        //IMediatorPlug needs
        public MainInterfaceMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public MainInterfaceView view;
        private TroopProxy m_TroopProxy;
        private TroopMainCreate m_TroopMainData;
        private MonsterProxy m_MonsterProxy;
        private WarWarningProxy m_warWarningProxy = null;
        private RoleInfoProxy m_RoleInfoProxy;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private bool m_assetsReady = false;

        private Timer m_warWarningTimer = null;
        private WarWarningType m_lastWarWarningType = WarWarningType.None;
        private int m_warWarningSplashTimes = 0;
        private GameObject m_warWarningSplashObject = null;
        private List<UI_Item_MainIFBuff_SubView> m_cityBuffViewList = new List<UI_Item_MainIFBuff_SubView>();
        private Dictionary<int, CityBuff> m_cityBuffDic = new Dictionary<int, CityBuff>();
        private List<CityBuff> m_cityBuffList = new List<CityBuff>();
        private GameObject img_SelectQueue;
        //private bool m_bOnUITouchMove = false;
        private bool m_flagCaptain = false;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.OnTroopDataChanged,
                CmdConstant.UpdateEmail,
                CmdConstant.CityBuildinginfoFirst,
                CmdConstant.UpdateTaskStatistics,
                CmdConstant.ShowNPCDiaglog,
                CmdConstant.OnNPCDiaglogEnd,
                CmdConstant.CityBuildingLevelUP,
                CmdConstant.ClickExitCity,
                CmdConstant.EnterCity,
                CmdConstant.ClickEnterCity,
                CmdConstant.OnGuideMainInterfaceModule,
                CmdConstant.OnShowUI,
                CmdConstant.OnCloseUI,
                CmdConstant.ForceCloseGuide,
                Task_ChapterFinish.TagName,
                Task_TaskFinish.TagName,
                CmdConstant.CityAgeChange,
                CmdConstant.technologyChange,
                CmdConstant.OnCityLoadFinished,
                CmdConstant.GuideFinished,
                CmdConstant.AgeStart,
                CmdConstant.AgeEnd,
                CmdConstant.InSoldierInfoChange,
                CmdConstant.UpdateChatMsg,
                CmdConstant.HideMainCityUI,
                CmdConstant.ShowMainCityUI,
                CmdConstant.CityBuffChange,
                CmdConstant.DayNightChange,
                CmdConstant.CollectRuneFinish,
                CmdConstant.AllianceEixt,
                Guild_ApplyJoinGuild.TagName,
                Guild_CreateGuild.TagName,
                Task_TaskInfo.TagName,
                CmdConstant.OnServerCallbackChangedRecharge,
                CmdConstant.OnServerCallbackChangedRiseRoad,
                CmdConstant.WarWarningInfoChanged,
                CmdConstant.NewLimitTimePackage,
                CmdConstant.RemoveLimitTimePackage,
                CmdConstant.AllianceStudyDonateRedCount,
                CmdConstant.RefreshEquipRedPoint,
                CmdConstant.GameModeChanged,
                CmdConstant.UpdateChatRedDot,
                CmdConstant.AllianceGiftRedPoint,
                Guild_GuildRequestHelps.TagName,
                CmdConstant.OnCloseSelectMainTroop,
                CmdConstant.OnOpenSelectMainTroop,
                CmdConstant.OnOpenSelectDoubleTroop,
                CmdConstant.AllianceJoinUpdate,
                CmdConstant.BattleMainRedPointChanged,
                CmdConstant.ItemReddotChange,
                CmdConstant.RallyTroopChange,
                CmdConstant.ExpeditionInfoChange,
                CmdConstant.UpdatePlayerHistoryPower,
                Task_TaskInfo.TagName,
                CmdConstant.denseFogOpenFlag,
                Hero_HeroInfo.TagName,
                CmdConstant.ItemInfoChange,
                CmdConstant.GetNewHero,
                CmdConstant.UpdateHero,
                CmdConstant.NetWorkReconnecting,
                Build_BuildingInfo.TagName,
                CmdConstant.OnTouchMoveUITroopCallBack,
                CmdConstant.TaskGuideRemind,
                CmdConstant.GuildMapMarkerInfoChanged,
                CmdConstant.AccountBindReddotStatus,
                CmdConstant.RefreshMainChatPoint,
                CmdConstant.ActivePointChange,
                CmdConstant.ActivePointRewardsChange,
                CmdConstant.LevelChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.RallyTroopChange:
                case CmdConstant.AllianceStudyDonateRedCount: 
                case CmdConstant.AllianceGiftRedPoint:
                case Guild_GuildRequestHelps.TagName:
                    HotfixUtil.InvokOncePerfOneFrame("CheckStudyRedPoint", () =>
                    {
                        CheckStudyRedPoint();
                    });
                    break;
                case CmdConstant.LevelChange:
                    HotfixUtil.InvokOncePerfOneFrame("RefreshBattleMainRedPoint", () =>
                    {
                        RefreshBattleMainRedPoint();
                    });
                    break;
                case CmdConstant.OnTroopDataChanged:
                    HotfixUtil.InvokOncePerfOneFrame("OnRefreshTroopView", () =>
                    {
                        OnRefreshTroopView();
                    });
                    break;
                case CmdConstant.UpdateEmail:
                    HotfixUtil.InvokOncePerfOneFrame("OnEmailUpdate", () =>
                    {
                        OnEmailUpdate();
                    });
                    break;
                case CmdConstant.CityBuffChange:
                    {
                        RefreshCityBuffData();
                        RefreshCityBuffView();
                    }
                    break;
                case CmdConstant.CollectRuneFinish:
                    {
                        PlayCollectRuneFinishEffect(notification);
                    }
                    break;
                case CmdConstant.ClickExitCity:
                    {
                        OnMap();
                    }
                    break;
                case CmdConstant.ClickEnterCity:
                    {
                        OnCity();
                    }
                    break;
                case CmdConstant.ShowNPCDiaglog:
                    OnNpcDialog(true);
                    break;
                case CmdConstant.OnNPCDiaglogEnd:
                    OnNpcDialog(false);
                    break;
                case CmdConstant.CityBuildingLevelUP:
                    {
                        BuildingLevelUP(notification.Body);
                        OnUpdateTaskStatistics();
                    }
                    break;
                case Build_BuildingInfo.TagName:
                    HotfixUtil.InvokOncePerfOneFrame("CheckBuildRedPoint", () =>
                    {
                        CheckBuildRedPoint();
                    });
                    break;
                case CmdConstant.technologyChange:
                case CmdConstant.CityBuildinginfoFirst:
                case CmdConstant.UpdateTaskStatistics:
                case CmdConstant.InSoldierInfoChange:
                case CmdConstant.ExpeditionInfoChange:
                case CmdConstant.UpdatePlayerHistoryPower:
                case Task_TaskInfo.TagName:
                case CmdConstant.denseFogOpenFlag:
                case Hero_HeroInfo.TagName:
                case CmdConstant.ActivePointChange:
                case CmdConstant.ActivePointRewardsChange:
                    {
                        HotfixUtil.InvokOncePerfOneFrame("OnUpdateTaskStatistics", () =>
                        {
                            OnUpdateTaskStatistics();
                        });
                    }
                    break;
                case CmdConstant.OnGuideMainInterfaceModule:
                    {
                        OnGuideMainModule();
                        OnForceShowModuleByGuide(notification.Body as GuideDefine);
                    }
                    break;
                case CmdConstant.GuideFinished:
                    {
                        ForceCloseGuide();
                    }
                    break;
                case CmdConstant.OnShowUI:
                    {
                        OnCloseWindowUI(notification.Body as UIInfo);
                    }
                    break;
                case CmdConstant.OnCloseUI:
                    {
                        UIInfo uIInfo = notification.Body as UIInfo;
                        if (uIInfo!=null)
                        {
                            OnShowWindowUI(notification.Body as UIInfo);
                            if (uIInfo == UI.s_captain)
                            {
                                m_flagCaptain = true;
                                CheckCaptainRedPoint(true);
                            }
                        }
                    }
                    break;
                case CmdConstant.RefreshMainChatPoint:
                    CheckChatRedPoint();
                    break;

                case CmdConstant.ForceCloseGuide:
                    {
                        ForceCloseGuide();
                    }
                    break;
                case CmdConstant.CityAgeChange:
                    {
                        OnAgeChange();
                    }
                    break;
                case CmdConstant.OnCityLoadFinished:
                    {
                        OnLoadCityFinished();
                    }
                    break;
                case CmdConstant.AgeStart:
                    {
                        OnChapterChange(true);
                    }
                    break;
                case CmdConstant.AgeEnd:
                    {
                        OpenAppRating();
                        ShowChargePop();
                        OnChapterChange(false);
                    }
                    break;
                case CmdConstant.UpdateChatMsg:
                    {
                        RefreshChatPage();
                        if (!CoreUtils.uiManager.ExistUI(UI.s_chat))
                        {
                            UpdateChatRedDot();
                        }
                    }
                    break;
                case CmdConstant.HideMainCityUI:
                    {
                        OnHideOutSideControlModules(notification.Body);
                    }
                    break;
                case CmdConstant.ShowMainCityUI:
                    {
                        OnShowOutSideControlModules();
                    }
                    break;
                case CmdConstant.DayNightChange:
                    ChangeDateTimeColor();
                    break;
                case Guild_ApplyJoinGuild.TagName:
                case Guild_CreateGuild.TagName:
                case CmdConstant.AllianceEixt:
                case CmdConstant.AllianceJoinUpdate:
                    HotfixUtil.InvokOncePerfOneFrame("OnShowOrHideAlliancePage", () =>
                    {
                        OnShowOrHideAlliancePage();
                    });
                    HotfixUtil.InvokOncePerfOneFrame("CheckStudyRedPoint", () =>
                    {
                        CheckStudyRedPoint();
                    });
                    break;
                case CmdConstant.OnServerCallbackChangedRecharge:
                case CmdConstant.OnServerCallbackChangedRiseRoad:
                    HotfixUtil.InvokOncePerfOneFrame("RefreshRechargeIF", () =>
                    {
                        RefreshRechargeIF();
                    });
                    break;
                case CmdConstant.WarWarningInfoChanged:
                    OnWarWarningInfoChanged();
                    break;
                case CmdConstant.NewLimitTimePackage:
                    NewlimitPackage(notification.Body as LimitTimePackage);
                    break;
                case CmdConstant.RemoveLimitTimePackage:
                    RemoveLimitPackage(notification.Body.ToInt64());
                    break;
                case CmdConstant.RefreshEquipRedPoint:
                    HotfixUtil.InvokOncePerfOneFrame("CheckCaptainRedPoint", ()=>
                    {
                        CheckCaptainRedPoint();
                    });
                    break;
                case CmdConstant.GameModeChanged:
                    OnGameModeChanged();
                    break;
                case CmdConstant.UpdateChatRedDot:
                    HotfixUtil.InvokOncePerfOneFrame("UpdateChatRedDot", () =>
                    {
                        UpdateChatRedDot();
                    });
                    break;
                case CmdConstant.OnCloseSelectMainTroop:
                    SetCloseSelectTroopImgByIndex();
                    break;
                case CmdConstant.OnOpenSelectMainTroop:
                    SetOpenSelectTroopImgByIndex(notification.Body.ToInt32());
                    break;
                case CmdConstant.OnOpenSelectDoubleTroop:
                    List<int> indexList = notification.Body as List<int>;
                    SetDoubleSelectTroopImg(indexList);
                    break;
                case CmdConstant.BattleMainRedPointChanged:
                    OnBattleMainRedPointChanged(notification.Body.ToInt32());
                    break;
                case CmdConstant.ItemReddotChange:
                    CheckBagRedPoint((int)notification.Body);
                    break;
                case CmdConstant.ItemInfoChange:
                case CmdConstant.GetNewHero:
                case CmdConstant.UpdateHero:
                    HotfixUtil.InvokOncePerfOneFrame("CheckCaptainRedPoint", () =>
                    {
                        CheckCaptainRedPoint();
                    });
                    break;
                case CmdConstant.NetWorkReconnecting:
                    OnNetWorkReconnecting();
                    break;
                case CmdConstant.OnTouchMoveUITroopCallBack:
                    //if (m_bOnUITouchMove)
                    //{
                    //    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    //    view.m_sv_list_view_ListView.OnEndDrag(pointerEventData);
                    //    m_bOnUITouchMove = false;
                    //}
                    break;
                case CmdConstant.TaskGuideRemind:
                    JumpGuideTask((int)notification.Body);
                    break;
				case CmdConstant.GuildMapMarkerInfoChanged:
                    HotfixUtil.InvokOncePerfOneFrame("CheckMapMarkerRedPoint", () =>
                    {
                        CheckMapMarkerRedPoint();
                    });
                    break;
                case CmdConstant.AccountBindReddotStatus:
                    CheckAccountBindReddot((bool)notification.Body);
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
            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);
        }

        public override void OnRemove()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
            SystemOpen.ClearSystemOpen();
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
            if (taskAnimShow)
            {
                if (m_deleteItem != null)
                {
                    if (m_deleteItem.data is TaskData)
                    {
                        TaskData taskData = m_deleteItem.data as TaskData;
                        m_deleteTime = m_deleteTime - Time.deltaTime;
                        if (taskData.taskPageType == EnumTaskPageType.TaskMain || taskData.taskPageType == EnumTaskPageType.TaskSide)
                        {
                            view.m_sv_list_tasks_ListView.UpdateItemSize(m_deleteItem.index, m_height * m_deleteItem.go.transform.localScale.y);
                            if (m_deleteTime <= 0)
                            {
                                PlayAnimEnd(m_deleteItem);
                            }
                        }
                    }
                }
            }
    }

        protected override void InitData()
        {
            IsOpenUpdate = true;
            RoleInfoHelp.DeleteNewCreateRole();
            m_CurrencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_TroopProxy?.InitSaveKey();
            m_RoleInfoProxy= AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            m_RoleInfoProxy?.InitServerList();
            m_RoleInfoProxy?.SendRoleInfo();

                      
            m_MonsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
            m_taskProxy = AppFacade.GetInstance().RetrieveProxy(TaskProxy.ProxyNAME) as TaskProxy;
            m_cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
            m_chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_warWarningProxy = AppFacade.GetInstance().RetrieveProxy(WarWarningProxy.ProxyNAME) as WarWarningProxy;
            
            m_rallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy; 
            m_cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            m_mapMarkerProxy = AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy;

            RefreshTaskData();
            InitCityBuffData();
            m_preLoadRes.AddRange(view.m_sv_list_tasks_ListView.ItemPrefabDataList);
            m_preLoadRes.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            m_preLoadRes.AddRange(view.m_sv_list_dialog_ListView.ItemPrefabDataList);
            UpdateServerTime();
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetsReady = true;
                m_assetDic = assetDic;
                AssetLoadFinish();
                RefreshChatPage();
                RefreshTask();

                AppFacade.GetInstance().SendNotification(CmdConstant.AccountBindReddotCheck);
            });

            m_cityInsideDxf = WorldCamera.Instance().getCameraDxf("city_bound");
            m_openLodUILevel[0] = m_cityInsideDxf;
            m_openLodUILevel[1] = WorldCamera.Instance().getCameraDxf("dispatch");
            m_openLodUILevel[2] = WorldCamera.Instance().getCameraDxf("TacticsToStrategy1");
            m_openLodUILevel[3] = WorldCamera.Instance().getCameraDxf("TacticsToStrategy2");
            m_openLodUILevel[4] = WorldCamera.Instance().getCameraDxf("strategic_min");
            m_openLodUILevel[5] = WorldCamera.Instance().getCameraDxf("max");
            m_openLodUILevel[6] = WorldCamera.Instance().getCameraDxf("limit_max");
            m_viewLevelMediator =
                AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as
                    GlobalViewLevelMediator;
            RefreshBattleMainRedPoint();
            CheckBuildRedPoint();

            //检测联盟是否有可创建的建筑
            Timer.Register(0.2f, () =>
            {
                if (view.gameObject == null)
                {
                    return;
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceBuildCanCreateCheck);
            });
        }


        protected override void BindUIEvent()
        {
            view.m_btn_task_GameButton.onClick.AddListener(OnTaskBtnClick);
            view.m_btn_blue_GameButton.onClick.AddListener(OnTaskUIAnimation);

            view.m_UI_Model_more.m_btn_btn_GameButton.onClick.AddListener(OnFeatureBtnAnimation);
            view.m_btn_build_GameButton.onClick.AddListener(OnBuildBtnClick);
            view.m_btn_search_GameButton.onClick.AddListener(OnSearchBtnClick);
            view.m_UI_Model_Captain.AddListener(OnCaptainClick);
            view.m_UI_Model_Item.AddListener(OnBag);
            view.m_btn_queueButton_GameButton.onClick.AddListener(OnTroop);
            // view.m_UI_Model_War.AddListener(OnSearch);
            //邮件
            view.m_UI_Model_mail.m_btn_btn_GameButton.onClick.AddListener(ShowEmail);
            //聊天
            view.m_btn_chat_GameButton.onClick.AddListener(OnClickChatBtn);

            view.m_UI_Model_Guild.AddListener(OnGuild);

            view.m_btn_map_GameButton.onClick.AddListener(OnMap);
            view.m_btn_city_GameButton.onClick.AddListener(OnCity);

            view.m_UI_Item_MainIFEventActivity.Refresh();
            view.m_UI_Item_MainIFEventCharge.Refresh();
            CheckNewLimitPackage();

            //地图坐标变化绑定
            WorldCamera.Instance().AddViewChange(OnWorldViewChange);

            //联盟
            view.m_UI_Model_Guild.m_btn_btn_GameButton.onClick.AddListener(OnAlliance);
            //战役
            view.m_UI_Model_War.m_btn_btn_GameButton.onClick.AddListener(OnWarClick);
            //地图位置搜索
            view.m_btn_posBtn_GameButton.onClick.AddListener(OnPositionBtn);
            //地图书签
            view.m_btn_collect_GameButton.onClick.AddListener(OnCollectBtnClick);
            //代币获取
            view.m_UI_Item_PlayerResources.m_UI_Model_gem.m_btn_btn_GameButton.onClick.AddListener(OnGemBtnClick);
            //vip功能()
            view.m_UI_Item_PlayerPowerInfo.m_btn_vip_GameButton.onClick.AddListener(OnVipClick);
            //服务器列表(未开放)
            view.m_UI_Model_world.m_btn_btn_GameButton.onClick.AddListener(OnLockBtnClick);
            view.m_UI_Item_PlayerPowerInfo.m_btn_arrow_GameButton.onClick.AddListener(OnArrowBtnClick);
            view.m_btn_warn_GameButton.onClick.AddListener(OnClickShowWarWarning);

            view.m_UI_Item_AliGuide.Init(1);

            view.m_UI_Item_NewRoleBtn.Init();

            //初始化各模块
            OnInitMainModule();
        }
        
        
        private void CheckStudyRedPoint()
        {
            long count = m_allianceProxy.AllRedPoint();
            view.m_UI_Model_Guild.SetRedCount(count+m_rallyTroopsProxy.GetRallyRedPoint());
        }
        private void CheckBuildRedPoint()
        {
            int countEconomic = 0;//经济
            int countMilitary = 0;//军事
            int countDecorative = 0;
            countEconomic = m_cityBuildingProxy.CountBuildableBuild(out countMilitary);
           view.m_UI_Common_Redpoint.ShowRedPoint(countEconomic+ countMilitary);
        }
        private void CheckChatRedPoint()
        {
           // bool hasAnyUnread = m_chatProxy.AllianceContact.GetUnreadCount() > 0;
            if (ChatProxy.AtRedPoint)
            {
                view.m_img_redpoint_chat.ShowStringRedPoint("@");
            }
            else
            {
                view.m_img_redpoint_chat.HideRedPoint();
            }
            
        }
        int captainRedPoint = 0;
        int captainRedPointClear = 0;
        private void CheckCaptainRedPoint(bool clear = false)
        {           
            int count = m_bagProxy.GetRegionRedPointCount();
            count += m_heroProxy.GetCanSummonerHeroCount();
            count += m_heroProxy.GetHeroStarCount();
            if (count > captainRedPoint)
            {
                view.m_UI_Model_Captain.SetRedCount(count - captainRedPointClear);
            }
            else
            {
                if (m_flagCaptain)
                {
                    if(count < captainRedPointClear)
                    {
                        captainRedPointClear = count;
                    }
                    view.m_UI_Model_Captain.SetRedCount(count-captainRedPointClear);
                }
                else
                {
                    view.m_UI_Model_Captain.SetRedCount(count);
                }
            }
            if (clear)
            {
                view.m_UI_Model_Captain.SetRedCount(0);
                captainRedPointClear = captainRedPoint;
            }
      
            captainRedPoint = count;
        }

        private void CheckBagRedPoint(int isNowUpdate)
        {
            if (isNowUpdate == 0)
            {
                view.m_UI_Model_Item.UpdateBagReddot();
            }
            else
            {
                view.m_UI_Model_Item.DelayUpdateBagReddot();
            }
        }

        private void CheckMapMarkerRedPoint()
        {
            view.m_UI_Common_Redpoint_collect.ShowSmallRedPoint(m_mapMarkerProxy.CalNewGuildMapMarkerInfoNum());
        }

        private void OnGuild()
        {
            //零时打开研究界面
            //            CoreUtils.uiManager.ShowUI(UI.s_ResearchMain);
        }

        protected override void BindUIData()
        {
            //邮件
            OnEmailUpdate();
            OnAgeChange();
            RefreshCityBuffData();
            RefreshCityBuffView();
            OnWarWarningInfoChanged();
            UpdateChatRedDot();
            CheckMapMarkerRedPoint(); 
            view.m_btn_task_GameButton.gameObject.SetActive(true);
            view.m_lbl_position_server_LanguageText.text = m_playerProxy.GetGameNode().ToString("N0");
        }

        #endregion

        #region 点击事件

        private void OnAlliance()
        {
            if(SystemOpen.IsSystemClose(EnumSystemOpen.alliance))
            {
                return;
            }
            if (m_allianceProxy.HasJionAlliance())
            {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceWelcome);
            }

        }

        private void OnGemBtnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_Charge,null,(int)EnumRechargeListPageType.ChargeGemShop);
        }

        private void OnLockBtnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_WinServer);
        }
        private void OnArrowBtnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_cityManager);
            
        }
        private void OnBuildBtnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_buildCity);
        }

        private void OnSearchBtnClick()
        {
            //
            OnSearch();
        }

        private void OnTaskBtnClick()
        {
            int redPointTaskChapter = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskChapter);
            int redPointTaskMain = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskMain);
            int redPointTaskSide = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskSide);
            int redPointTaskDaily = m_taskProxy.GetRedPoint(EnumTaskPageType.TaskDaily);
            int redPointActivePointRewards = m_taskProxy.GetRedPointActivePointRewards();
            if (m_taskProxy.ShowOtherTask())
            {
                if (redPointTaskChapter > 0)
                {
                    CoreUtils.uiManager.ShowUI(UI.s_Taskinfo,null, 1);
                    return;
                }
                if (redPointTaskMain > 0|| redPointTaskSide>0)
                {
                    CoreUtils.uiManager.ShowUI(UI.s_Taskinfo, null, 2);
                    return;
                }
                if (redPointTaskDaily > 0|| redPointActivePointRewards>0)
                {
                    CoreUtils.uiManager.ShowUI(UI.s_Taskinfo, null, 3);
                    return;
                }
                CoreUtils.uiManager.ShowUI(UI.s_Taskinfo, null, 1);
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_Taskinfo,null,1);
            }
        }


        private void OnCaptainClick()
        {
            if(SystemOpen.IsSystemOpen(EnumSystemOpen.captain))
            {
                CoreUtils.uiManager.ShowUI(UI.s_captain, null);
            }
        }

        private void OnMap()
        {
            if (!m_cityBuffProxy.CheckGuildBuildCreatePre())
            {
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.ExitCity);
        }

        private void OnCity()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
            if (WorldCamera.Instance().IsAutoMoving())
            {
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.EnterCity);
        }

        private void OnPositionBtn()
        {
            CoreUtils.uiManager.ShowUI(UI.s_worldPosSearch);
        }

        private void OnWarClick()
        {
            if (SystemOpen.IsSystemClose(EnumSystemOpen.war))
            { 
                return;
            }
            //CoreUtils.uiManager.ShowUI(UI.s_battleMain);

            //直接打开远征界面
            if (SystemOpen.IsSystemClose(EnumSystemOpen.war_expedition))
            {
                Data.SystemOpenDefine cfg = CoreUtils.dataService.QueryRecord<Data.SystemOpenDefine>(10005);
                if (cfg != null)
                {
                    Tip.CreateTip(805001, cfg.openLv);
                }
                return;
            }
            CoreUtils.uiManager.ShowUI(UI.s_expeditionFight);
        }

        private void OnVipClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_Vip);

        }

        private void OnCollectBtnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_MapMarker);
        }

        #endregion

        private void OnGameModeChanged()
        {
            switch(GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    view.gameObject.SetActive(true);
                    break;
                case GameModeType.Expedition:
                    view.gameObject.SetActive(false);
                    break;
            }
        }

        private void AssetLoadFinish()
        {
            m_TroopMainData = new TroopMainCreate();
            m_TroopMainData.Init();
            Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
            foreach(var prefabName in view.m_sv_list_view_ListView.ItemPrefabDataList)
            {
                if(m_assetDic.ContainsKey(prefabName))
                {
                    prefabNameList[prefabName] = m_assetDic[prefabName];
                }
            }
          
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = OnTroopListItemEnter;
            funcTab.GetItemPrefabName = OnTroopListGetPrefabName;
            view.m_sv_list_view_ListView.SetInitData(prefabNameList, funcTab);
            view.m_sv_list_view_ListView.FillContent(m_TroopMainData.GetDataCount());

            RefreshTroopsDispatchNum();
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateServerTime, null, true, true);
            }
            view.m_lbl_time_LanguageText.horizontalOverflow = HorizontalWrapMode.Overflow;

            m_gameEventGlobalMediator = AppFacade.GetInstance().RetrieveMediator(GameEventGlobalMediator.NameMediator) as GameEventGlobalMediator;
            ChangeDateTimeColor();
        }

        private void ChangeDateTimeColor()
        {
            if (m_gameEventGlobalMediator == null)
            {
                return;
            }
            int index = m_gameEventGlobalMediator.GetCurrLightIndex();

            bool showBlack = false;

            if (index == (int)DayNightSwitch.DAWN)
            {
                //黎明
                showBlack = true;
            }
            else if (index == (int)DayNightSwitch.DAY || index == (int)DayNightSwitch.DAWN_TO_DAY)
            {
                //白天/黎明->白天
                showBlack = true;
            }
            else if (index == (int)DayNightSwitch.DUST || index == (int)DayNightSwitch.DAY_TO_DUST)
            {
                //黄昏/白天->黄昏
                showBlack = true;
            }
            else if (index == (int)DayNightSwitch.NIGHT || index == (int)DayNightSwitch.DUST_NIGHT)
            {
                //夜晚/黄昏->夜晚
            }
            else if (index == (int)DayNightSwitch.AUTO)
            {
                //从夜晚->黎明 
            }
            if (showBlack)
            {
                view.m_lbl_time_LanguageText.color = Color.black;
                view.m_lbl_age_LanguageText.color = Color.black;
            } else
            {
                view.m_lbl_time_LanguageText.color = Color.white;
                view.m_lbl_age_LanguageText.color = Color.white;
            }
        }

        //更新服务器时间
        private void UpdateServerTime()
        {
            DateTime time = ServerTimeModule.Instance.GetCurrServerDateTime();

            string str = ServerTimeModule.Instance.GetCurrServerDateTime().ToString("MM/dd HH:mm");
            str = LanguageUtils.getTextFormat(100717, str);
#if UNITY_EDITOR
            view.m_lbl_time_LanguageText.text = str +
                $"     lod:{ClientUtils.lodManager.GetLodDistance()} lv:{LevelDetailCamera.instance.GetCurrentLodLevel()} ping:{ServerTimeModule.Instance.Ping}ms";
#else
            if(HotfixUtil.IsDebugable())
            {
                view.m_lbl_time_LanguageText.text = $"{str}\tping:{ServerTimeModule.Instance.Ping}ms";
            }
            else
            {
                view.m_lbl_time_LanguageText.text = str;
            }
#endif
        }


        private void BuildingLevelUP(object body)
        {
            Int64 buildingIndex = (Int64) body;
            BuildingInfoEntity buildingInfo = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex);
            if (buildingInfo.type == (int) EnumCityBuildingType.TownCenter && buildingInfo.finishTime < 0
            ) //如果是市政大厅 则需要刷新下派遣上限
            {
                RefreshTroopsDispatchNum();
            }
        }

        #region 时代变迁

        private void OnAgeChange()
        {
            int languageID = 0;
            List<CityAgeSizeDefine> list = CoreUtils.dataService.QueryRecords<CityAgeSizeDefine>();
            for (int i = 0; i < list.Count; i++)
            {
                if (m_playerProxy.CurrentRoleInfo.level >= list[i].townLevel)
                {
                    languageID = list[i].l_nameId;
                }
            }

            view.m_lbl_age_LanguageText.text = LanguageUtils.getText(languageID);
        }

        #endregion

        #region 行军

        //更新可派遣队列数量
        private void RefreshTroopsDispatchNum()
        {
            if (m_TroopMainData == null) return;
            int count = m_TroopProxy.GetAllTroopCount();
            int num = m_TroopMainData.GetDataCount();
            view.m_lbl_count_LanguageText.text =
                string.Format("{0}/{1}", count, m_TroopProxy.GetTroopDispatchNum());
            view.m_pl_queueGroup_CanvasGroup.alpha = num > 0 ? 1 : 0;
            view.m_pl_queueGroup_CanvasGroup.interactable = num > 0;
            OnRefreshTroopBgView();
        }

        private void OnRefreshTroopView()
        {
            if (m_TroopMainData == null) return;
            int oldCount = m_TroopMainData.GetDataCount();
            m_TroopMainData.Update();
            int count = m_TroopMainData.GetDataCount();
            if (oldCount != count)
            {
                view.m_sv_list_view_ListView.FillContent(count);
            }
            else
            {
                view.m_sv_list_view_ListView.ForceRefresh();
            }
            RefreshTroopsDispatchNum();
        }

        private void OnRefreshTroopBgView()
        {
            float btnHeight = view.m_btn_queueButton_PolygonImage.rectTransform.rect.height;
            float itemHeight = view.m_sv_list_view_ListView.GetItemSizeByIndex(0);
            int count = m_TroopMainData.GetDataCount();
            int viewConut = count > 3 ? 3 : count;

            view.m_pl_queueGroup_PolygonImage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, btnHeight + itemHeight * viewConut);
            view.m_sv_list_view_PolygonImage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, btnHeight, itemHeight * viewConut);
        }

        private Timer _timerTouch;

        private string OnTroopListGetPrefabName(ListView.ListItem listItem)
        {
            return "UI_Item_MainIFArm";
        }
        
        private string OnTroopListLineGetPrefabName(ListView.ListItem listItem)
        {
            return "UI_Item_MainIFArmLine";
        }
        
        private TroopMainCreateDataType selectType = TroopMainCreateDataType.Troop;
        List<int> selectArmyIndexList = new List<int>();
        private int selectScoutIndex = -1;
        private int selectTransportIndex = -1;

        private Timer troopClickTimer = null;
        private bool troopClickFlag = false;
        private bool troopDoubleClickFlag = false;

        private void SetCloseSelectTroopImgByIndex()
        {
            selectType = TroopMainCreateDataType.None;

            selectArmyIndexList.Clear();
            selectScoutIndex = -1;
            selectTransportIndex = -1;

            view.m_sv_list_view_ListView.ForceRefresh();

            troopDoubleClickFlag = false;
        }
        
        private void SetOpenSelectTroopImgByIndex(int objectId)
        {
            var armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
            if (armyData != null)
            {
                switch (armyData.troopType)
                {
                    case RssType.Troop:
                        selectArmyIndexList.Clear();
                        selectArmyIndexList.Add(armyData.troopId);
                        selectType = TroopMainCreateDataType.Troop;
                        break;
                    case RssType.Scouts:
                        selectScoutIndex = armyData.scoutIndex;
                        selectType = TroopMainCreateDataType.Scout;
                        break;
                    case RssType.Transport:
                        selectTransportIndex = armyData.dataIndex;
                        selectType = TroopMainCreateDataType.Transport;
                        break;                    
                }

                view.m_sv_list_view_ListView.ForceRefresh();
            }
        }

        private void SetDoubleSelectTroopImg(List<int> indexList)
        {
            selectArmyIndexList.Clear();
            foreach (var index in indexList)
            {
                selectArmyIndexList.Add(index);
            }
            selectType = TroopMainCreateDataType.Troop;

            view.m_sv_list_view_ListView.ForceRefresh();
        }

        private void RefreshTroopListItemChoose(TroopMainCreateDataType type, int indx)
        {
            switch (type)
            {
                case TroopMainCreateDataType.Troop:
                    selectArmyIndexList.Clear();
                    selectArmyIndexList.Add(indx);
                    selectType = TroopMainCreateDataType.Troop;
                    break;
                case TroopMainCreateDataType.Scout:
                    selectScoutIndex = indx;
                    selectType = TroopMainCreateDataType.Scout;
                    break;
                case TroopMainCreateDataType.Transport:
                    selectTransportIndex = indx;
                    selectType = TroopMainCreateDataType.Transport;
                    break;
            }

            view.m_sv_list_view_ListView.ForceRefresh();
        }

        private void OnTroopListItemEnter(ListView.ListItem listItem)
        { 
            UI_Item_MainIFArmView itemView = MonoHelper.GetHotFixViewComponent<UI_Item_MainIFArmView>(listItem.go);
            UIPressBtn uiPressBtn = null;
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_MainIFArmView>(listItem.go);                
            }

            if (itemView != null)
            {               
                TroopMainCreateData data = m_TroopMainData.GetData(listItem.index);
                if (data != null)
                {
                    if (data.type == TroopMainCreateDataType.Troop)
                    {
                        if (data.hero != null)
                        {
                            if (selectType == TroopMainCreateDataType.Troop)
                            {
                                bool inFlag = false;

                                ArmyData armyData1 = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(data.id);
                                ArmyData armyData2 = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(armyData1.objectId);

                                foreach (var armyIndex in selectArmyIndexList)
                                {
                                    if (armyData1 != null && armyData2 != null && armyData2.isRally)
                                    {
                                        if (armyData2.troopId == armyIndex)
                                        {
                                            inFlag = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (armyIndex == data.id)
                                        {
                                            inFlag = true;
                                            break;
                                        }
                                    }
                                }

                                itemView.m_img_choose_PolygonImage.gameObject.SetActive(inFlag);
                            }
                            else
                            {
                                itemView.m_img_choose_PolygonImage.gameObject.SetActive(false);
                            }

                            itemView.m_UI_Model_CaptainHead.SetIcon(data.hero.config.heroIcon);
                            itemView.m_UI_Model_CaptainHead.SetRare(data.hero.config.rare);
                            if (!string.IsNullOrEmpty(data.icon))
                            {
                                ClientUtils.LoadSprite(itemView.m_img_collect_PolygonImage, data.icon);
                            }  
                            itemView.m_UI_Common_TroopsState.SetData((long)data.armyStatus);

                            itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                            itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
                            {
                                troopDoubleClickFlag = false;

                                if (troopClickTimer != null)
                                {
                                    Timer.Cancel(troopClickTimer);
                                    troopClickTimer = null;
                                }

                                if (troopClickFlag == true)
                                {
                                    if (data.armyStatus == ArmyStatus.PALLY_MARCH ||
                                        data.armyStatus == ArmyStatus.FAILED_MARCH ||
                                        data.armyStatus == ArmyStatus.COLLECTING ||
                                        data.armyStatus == ArmyStatus.GARRISONING ||
                                        data.armyStatus == ArmyStatus.RALLY_WAIT ||
                                        data.armyStatus == ArmyStatus.RALLY_BATTLE)
                                    {
                                        RefreshTroopListItemChoose(TroopMainCreateDataType.Troop, data.id);

                                        TouchTroopSelectParam param = new TouchTroopSelectParam();
                                        param.armIndex = data.id;
                                        param.isOpenWin = true;
                                        param.isSelect = true;
                                        Timer.Register(0.0f, null, (float dt) =>
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.TouchTroopSelect, param);
                                        });
                                    }
                                    else
                                    {
                                        Timer.Register(0.0f, null, (float dt) =>
                                        {
                                            AppFacade.GetInstance().SendNotification(CmdConstant.DoubleTouchTroopSelect, data.id);
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

                                    RefreshTroopListItemChoose(TroopMainCreateDataType.Troop, data.id);
									
                                    TouchTroopSelectParam param = new TouchTroopSelectParam();
                                    param.armIndex = data.id;
                                    param.isOpenWin = true;
                                    param.isSelect = true;
                                    Timer.Register(0.0f, null, (float dt) =>
                                    {
                                        AppFacade.GetInstance().SendNotification(CmdConstant.TouchTroopSelect, param);
                                    });
                                });
                            });

                            uiPressBtn = itemView.m_btn_btn_GameButton.GetComponent<UIPressBtn>();
                            if (uiPressBtn != null)
                            {
                                uiPressBtn.RemoveAllPressClick();
                                uiPressBtn.Register(0.0f,true);
                                uiPressBtn.AddPressClick(() =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.UIPressed);

                                    if (m_viewLevelMediator.GetViewLevel() > MapViewLevel.Tactical)
                                    {
                                        return;
                                    }
                                    
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
                                        RectTransform rectTransform = view.m_pl_queue_set_ArabLayoutCompment.gameObject.GetComponent<RectTransform>();
                                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, camera, out localMouse))
                                        {
                                            if (!rectTransform.rect.Contains(localMouse))
                                            {
                                                int armIndex = data.id;

                                                if (!troopDoubleClickFlag)
                                                {
                                                    RefreshTroopListItemChoose(TroopMainCreateDataType.Troop, armIndex);
                                                }
                                                else
                                                {
                                                    if (!selectArmyIndexList.Contains(data.id))
                                                    {
                                                        if (selectArmyIndexList.Count > 0)
                                                        {
                                                            armIndex = selectArmyIndexList[0];
                                                        }                                                        
                                                    }

                                                    troopDoubleClickFlag = false;
                                                }

                                                TouchTroopSelectParam param = new TouchTroopSelectParam();
                                                param.armIndex = armIndex;
                                                param.isSelect = true;
                                                param.isDrag = true;
                                                param.isCameraFollow = false;
                                                WorldCamera.Instance().SetCanDrag(true);
                                                AppFacade.GetInstance().SendNotification(CmdConstant.SelectTroopDragMove, param);

                                                timer.Cancel();

                                                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                                                ExecuteEvents.Execute(view.m_sv_list_view_ListView.gameObject, pointerEventData, ExecuteEvents.endDragHandler);
                                            }
                                        }
                                    }, true, false, view.m_pl_queue_set_ArabLayoutCompment);
                                });
                                uiPressBtn.OnPointUpCallback = () =>
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.ClearGlobalTouchMoveCollide);
                                };
                            }

                            GrayChildrens gray = itemView.gameObject.GetComponent<GrayChildrens>();
                            if (gray)
                            {                             
                                if (TroopHelp.IsHaveState(data.ArmyInfo.status, ArmyStatus.FAILED_MARCH))
                                {
                                    gray.Gray();
                                }
                                else
                                {
                                    gray.Normal();
                                }
                            }
                            itemView.m_img_line_PolygonImage.gameObject.SetActive(false);
                        }
                    }
                    else if (data.type == TroopMainCreateDataType.Scout)
                    {
                        if (selectType == TroopMainCreateDataType.Scout)
                        {
                            itemView.m_img_choose_PolygonImage.gameObject.SetActive(selectScoutIndex == data.id);
                        }
                        else
                        {
                            itemView.m_img_choose_PolygonImage.gameObject.SetActive(false);
                        }

                        itemView.m_UI_Model_CaptainHead.SetIcon(data.scoutIcon);
                        itemView.m_UI_Model_CaptainHead.SetDefRare();
                        ClientUtils.LoadSprite(itemView.m_img_collect_PolygonImage, data.icon);
                        itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                        itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
                        {
                            RefreshTroopListItemChoose(TroopMainCreateDataType.Scout, data.id);

                            TouchNotTroopSelect touchSelectScout = new TouchNotTroopSelect();
                            touchSelectScout.id = data.id;
                            touchSelectScout.isAutoMove = false;
                            AppFacade.GetInstance().SendNotification(CmdConstant.TouchScoutSelect, touchSelectScout);
                        });
                        uiPressBtn = itemView.m_btn_btn_GameButton.GetComponent<UIPressBtn>();
                        if (uiPressBtn != null)
                        {
                            uiPressBtn.RemoveAllPressClick();
                            if (uiPressBtn != null)
                            {
                                uiPressBtn.RemoveAllPressClick();
                                uiPressBtn.Register(0.0f, true);
                                uiPressBtn.AddPressClick(() =>
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
                                    });
                                });
                            }
                        }
                        GrayChildrens gray = itemView.gameObject.GetComponent<GrayChildrens>();
                        if (gray)
                        {
                            gray.Normal();
                        }
                        itemView.m_img_line_PolygonImage.gameObject.SetActive(data.isShowLine);
                        itemView.m_UI_Common_TroopsState.SetData(0);
                    }
                    else if (data.type == TroopMainCreateDataType.Transport)
                    {
                        if (selectType == TroopMainCreateDataType.Transport)
                        {
                            itemView.m_img_choose_PolygonImage.gameObject.SetActive(selectTransportIndex == data.id);
                        }
                        else
                        {
                            itemView.m_img_choose_PolygonImage.gameObject.SetActive(false);
                        }

                        itemView.m_UI_Model_CaptainHead.SetIcon(data.scoutIcon);
                        ClientUtils.LoadSprite(itemView.m_img_collect_PolygonImage, data.icon);
                        itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                        itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
                        {
                            RefreshTroopListItemChoose(TroopMainCreateDataType.Transport, data.id);

                            TouchNotTroopSelect touchSelectScout = new TouchNotTroopSelect();
                            touchSelectScout.id = (int)data.id ;
                            touchSelectScout.isAutoMove = false;
                            AppFacade.GetInstance().SendNotification(CmdConstant.TouchTransportSelect, touchSelectScout);
                        });
                        uiPressBtn = itemView.m_btn_btn_GameButton.GetComponent<UIPressBtn>();
                        if (uiPressBtn != null)
                        {
                            uiPressBtn.RemoveAllPressClick();
                            uiPressBtn.Register(0.0f, true);
                            uiPressBtn.AddPressClick(() =>
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
                                });
                            });
                        }
                        GrayChildrens gray = itemView.gameObject.GetComponent<GrayChildrens>();
                        if (gray)
                        {
                            gray.Normal();
                        }
                        itemView.m_img_line_PolygonImage.gameObject.SetActive(false);
                        itemView.m_UI_Common_TroopsState.SetData(0);
                    }                                                        
                }                             
            }
        }

        #endregion

        #region 地图坐标和相机视角

        private int m_mapPosX;
        private int m_mapPosY;

        private void OnWorldViewChange(float x, float y, float dxf)
        {
            int posX = (Mathf.FloorToInt(x) / 6);
            int posY = (Mathf.FloorToInt(y) / 6);
            if (m_mapPosX != posX)
            {
                m_mapPosX = posX;
                view.m_lbl_positionX_LanguageText.text = posX.ToString();
            }

            if (m_mapPosY != posY)
            {
                m_mapPosY = posY;
                view.m_lbl_positionY_LanguageText.text = posY.ToString();
            }

            OnCityZoom((OpenLodUILevel) GetOpenLodUILevel(dxf));
            OnCityButtonChange(dxf);
            RefreshMinimapViewArea();
            OnCameraMoveLodMneuFunc();
        }

        #endregion

        #region UI动画

        public bool isTaskAniOn;

        private void OnTaskUIAnimation()
        {
            if (isTaskAniOn)
            {
                isTaskAniOn = false;
                view.m_UI_Item_HotTask_Animator.Play("UA_MainIF_HotTask_Off");
            }
            else
            {
                isTaskAniOn = true;
                view.m_UI_Item_HotTask_Animator.Play("UA_MainIF_HotTask_On");
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.QuestBtnChange, isTaskAniOn);
        }

        private bool isFeatureBtnOn = true;

        private void OnFeatureBtnAnimation()
        {
            if (isFeatureBtnOn)
            {
                SetFeatureBtnOff();
            }
            else
            {
                SetFeatureBtnOn();
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.FeatureBtnChange, isFeatureBtnOn);
        }

        private void SetFeatureBtnOn()
        {
            if (!view.m_pl_rect_Animator.enabled)
            {
                view.m_pl_rect_Animator.enabled = true;
            }

            isFeatureBtnOn = true;
            view.m_UI_Model_more.m_img_hide_PolygonImage.gameObject.SetActive(true);
            string aniName = LanguageUtils.IsArabic() ? "UA_MainIF_Rotation_LOn" : "UA_MainIF_Rotation_ROn";
            view.m_pl_rect_Animator.Play(aniName, -1, 0);
        }

        private void SetFeatureBtnOff()
        {
            if (!view.m_pl_rect_Animator.enabled)
            {
                view.m_pl_rect_Animator.enabled = true;
            }

            isFeatureBtnOn = false;
            view.m_UI_Model_more.m_img_hide_PolygonImage.gameObject.SetActive(false);
            string aniName = LanguageUtils.IsArabic() ? "UA_MainIF_Rotation_LOff" : "UA_MainIF_Rotation_ROff";
            view.m_pl_rect_Animator.Play(aniName, -1, 0);
        }

        #endregion

        #region 邮件

        private void ShowEmail()
        {
            if(SystemOpen.IsSystemClose(EnumSystemOpen.mail))
            {
                return;
            }
            if (m_emailProxy.EmailReceived)
            {
                CoreUtils.uiManager.ShowUI(UI.s_Email);
            }
            else
            {
                Tip.CreateTip(570013).Show();
            }
        }

        private bool m_isDelayUpdateEmailing = true;
        private float m_lastUpdateEmailTime = 0f;
        private void OnEmailUpdate()
        {
            if (Time.realtimeSinceStartup - m_lastUpdateEmailTime > 0.1f)
            {
                m_lastUpdateEmailTime = Time.realtimeSinceStartup;
                DelayUpdateEmail();
            }
            else
            {
                if (m_isDelayUpdateEmailing)
                {
                    return;
                }
                m_isDelayUpdateEmailing = true;
                Timer.Register(0.1f, DelayUpdateEmail);
            }
        }

        private void DelayUpdateEmail()
        {
            if (view.gameObject == null)
            {
                return;
            }
            int count = m_emailProxy.GetEmailRedPoint();
            if (count > 0)
            {
                view.m_UI_Model_mail.m_img_redpoint_PolygonImage.gameObject.SetActive(true);
                string countStr = UIHelper.NumerBeyondFormat(count);
                view.m_UI_Model_mail.m_lbl_count_LanguageText.text = countStr;
            }
            else
            {
                view.m_UI_Model_mail.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
            }
            m_isDelayUpdateEmailing = false;
        }

        #endregion

        #region 主界面显示或隐藏相关模块

        private enum MainMenuState
        {
            OnNormal = 0x01,
            OnNPCDialog = 0x02,
            OnWin = 0x04,
            OnGuide = 0x08,
            OnChapterChange = 0x10,
            OnOutSideControl = 0x20,
        }

        //当前主界面状态
        private int m_currentMainMenuState = (int) MainMenuState.OnNormal;

        private int CurrentMainMenuState
        {
            get { return m_currentMainMenuState; }
            set
            {
                m_currentMainMenuState = value;
                if (CurrentMainMenuState == (int) MainMenuState.OnNormal)
                {
                    ShowOrHideCityModule(m_lastZoomLevel);
                }
            }
        }

        //主界面各模块
        private Dictionary<EnumMainModule, GameObject> m_modules = new Dictionary<EnumMainModule, GameObject>();

        //各模块配置
        private Dictionary<EnumMainModule, OpenLodUiDefine> m_moduleDefines =
            new Dictionary<EnumMainModule, OpenLodUiDefine>();

        private Dictionary<int, EnumMainModule> m_moduleKeyMap = new Dictionary<int, EnumMainModule>();

        //各模块缓存的开关状态
        public Dictionary<EnumMainModule, bool> m_moduleState = new Dictionary<EnumMainModule, bool>();

        //各模块新手引导下的开关状态
        private Dictionary<EnumMainModule, bool> m_moduleGuideState = new Dictionary<EnumMainModule, bool>();

        //各模块的lod层级
        private Dictionary<EnumMainModule, List<OpenLodUILevel>> m_moduleLods =
            new Dictionary<EnumMainModule, List<OpenLodUILevel>>();

        //初始化各模块
        private bool m_initCityUI = false;

        private void OnInitMainModule()
        {
            m_modules[EnumMainModule.CurAge] = view.m_UI_Item_CurAge_Animator.gameObject;
            m_modules[EnumMainModule.Events] = view.m_UI_Item_Events_ArabLayoutCompment.gameObject;
            m_modules[EnumMainModule.HotTask] = view.m_UI_Item_HotTask_Animator.gameObject;
            m_modules[EnumMainModule.Chat] = view.m_UI_Item_ChatSearchBuild_Animator.gameObject;
            m_modules[EnumMainModule.Features] = view.m_UI_Item_Features_Animator.gameObject;
            m_modules[EnumMainModule.FlagWarn] = view.m_UI_Item_FlagWarn_Animator.gameObject;
            m_modules[EnumMainModule.Queue] = view.m_UI_Item_Queuebg_Animator.gameObject;
            m_modules[EnumMainModule.Position] = view.m_UI_Item_Position_Animator.gameObject;
            m_modules[EnumMainModule.Map] = view.m_UI_Item_Map.gameObject;
            m_modules[EnumMainModule.LodMenu] = view.m_UI_Item_LodMenu.gameObject;
            m_modules[EnumMainModule.PlayerPowerInfo] = view.m_UI_Item_PlayerPowerInfo.gameObject;
            m_modules[EnumMainModule.PlayerResources] = view.m_UI_Item_PlayerResources.gameObject;
            m_modules[EnumMainModule.SearchOrBuild] = view.m_pl_BuildAndSearch_Animator.gameObject;
            m_modules[EnumMainModule.CityOrWorld] = view.m_pl_MapAndCity_Animator.gameObject;
            m_modules[EnumMainModule.Server] = view.m_UI_Model_world.gameObject;
            m_modules[EnumMainModule.Mail] = view.m_UI_Item_mail_Animator.gameObject;
            m_modules[EnumMainModule.AliGuide] = view.m_pl_midmenu_GridLayoutGroup.gameObject;
            InitModuleDefines();
            InitModuleState();
            if (PlayerProxy.LoadCityFinished)
            {
                OnLoadCityFinished();
            }

            float limitDxf = m_cityInsideDxf;
            bool outSide = WorldCamera.Instance().getCurrentCameraDxf() > limitDxf;
            m_lastCityButtonState = outSide;
            SetCityButton(outSide);
            CheckStudyRedPoint();
            CheckCaptainRedPoint();
        }

        private void OnLoadCityFinished()
        {
            ShowOrHideCityModule(m_lastZoomLevel);
        }


        public RectTransform GetPowerTransform()
        {
            return view.m_UI_Item_PlayerPowerInfo.m_lbl_powerVal_LanguageText.rectTransform;
        }

        private int GetOpenLodUILevel(float dxf)
        {
            for (int i = 0; i < this.m_openLodUILevel.Length; i++)
            {
                if (dxf <= this.m_openLodUILevel[i])
                {
                    return i;
                }
            }

            return this.m_openLodUILevel.Length - 1;
        }

        private void InitModuleState()
        {
            foreach (var element in m_modules)
            {
                m_moduleState[element.Key] = true;
                m_moduleGuideState[element.Key] = true;
                OnShowModule(element.Key, false);
            }
        }

        private void InitModuleDefines()
        {
            List<EnumMainModule> tmpList = m_modules.Keys.ToList();
            for (int i = 0; i < tmpList.Count; i++)
            {
                OpenLodUiDefine lodDefine = CoreUtils.dataService.QueryRecord<OpenLodUiDefine>(i+1);
                if (lodDefine != null)
                {
                    m_moduleDefines[tmpList[i]] = lodDefine;
                    m_moduleLods[tmpList[i]] = InitModuleLod(m_moduleDefines[tmpList[i]]);

                    m_moduleKeyMap[lodDefine.ID] = tmpList[i];
                }
                else
                {
                    Debug.LogError("OpenLodUi配置表读取错误：" + i);
                }
            }
        }

        private List<OpenLodUILevel> InitModuleLod(OpenLodUiDefine define)
        {
            List<OpenLodUILevel> tmpList = new List<OpenLodUILevel>();
            if (define.City == 1)
            {
                tmpList.Add(OpenLodUILevel.city_bound);
            }

            if (define.Tactical == 1)
            {
                tmpList.Add(OpenLodUILevel.dispatch);
            }

            if (define.TacticsToStrategy_1 == 1)
            {
                tmpList.Add(OpenLodUILevel.TacticsToStrategy1);
            }

            if (define.TacticsToStrategy_2 == 1)
            {
                tmpList.Add(OpenLodUILevel.TacticsToStrategy2);
            }

            if (define.Strategic == 1)
            {
                tmpList.Add(OpenLodUILevel.strategic_min);
            }

            if (define.NationWide == 1)
            {
                tmpList.Add(OpenLodUILevel.max);
            }

            if (define.Continental == 1)
            {
                tmpList.Add(OpenLodUILevel.limit_max);
            }

            return tmpList;
        }

        //显示或隐藏单个模块
        private void OnShowModule(EnumMainModule type, bool show)
        {
            if (m_modules.ContainsKey(type))
            {
                bool isActive = m_moduleState[type];
                if (!m_moduleGuideState[type])
                {
                    show = false;
                }

                if (isActive != show)
                {
                    m_moduleState[type] = show;
                    Animator ani = m_modules[type].GetComponent<Animator>();
                    if (ani != null)
                    {
                        if (show && !m_modules[type].activeSelf)
                        {
                            m_modules[type].SetActive(true);
                        }

                        if (m_modules[type].activeInHierarchy)
                        {
                            string aniPreName = LanguageUtils.IsArabic()
                                ? m_moduleDefines[type].Animator
                                : m_moduleDefines[type].unArbAnimator;
                            string aniName = string.Concat(aniPreName, show ? "On" : "Off");
                            ani.Play(aniName, -1, 0);
                        }
                    }
                    else
                    {
                        m_modules[type].SetActive(show);
                    }
                }
            }
        }

        public bool GetTaskMenuShowStatus()
        {
            if (m_moduleState.ContainsKey(EnumMainModule.HotTask))
            {
                return m_moduleState[EnumMainModule.HotTask];
            }
            return false;   
        }

        #region NPCDialog

        //记录NPC对话之前的模块状态
        private Dictionary<EnumMainModule, bool> m_tmpNpcModule = new Dictionary<EnumMainModule, bool>();

        private void OnNpcDialog(bool isHide)
        {
            if (isHide)
            {
                if ((CurrentMainMenuState & (int) MainMenuState.OnNPCDialog) <= 0)
                {
                    CurrentMainMenuState |= (int) MainMenuState.OnNPCDialog;
                    foreach (var element in m_moduleDefines)
                    {
                        OnShowModule(element.Key, element.Value.openWinNpcDialog == 1);
                    }
                }
            }
            else
            {
                CurrentMainMenuState &= ~(int) MainMenuState.OnNPCDialog;
            }
        }

        #endregion

        #region CityZoom

        private OpenLodUILevel m_lastZoomLevel;

        private void OnCityZoom(OpenLodUILevel level)
        {
            if (level == m_lastZoomLevel)
            {
                return;
            }

            m_lastZoomLevel = level;

            if (CurrentMainMenuState == (int) MainMenuState.OnNormal)
            {
                ShowOrHideCityModule(level);
            }
        }

        private void ShowOrHideCityModule(OpenLodUILevel level)
        {
            if (!PlayerProxy.LoadCityFinished)
            {
                Debug.Log("未加载进主城");
                return;
            }

            foreach (var element in m_moduleLods)
            {
                OnShowModule(element.Key, element.Value.Contains(level));
            }
        }

        private bool m_lastCityButtonState;

        private void OnCityButtonChange(float dxf)
        {
            float limitDxf = m_cityInsideDxf;
            if (m_cityBuildingProxy.RolePos == Vector2.zero) //临时处理 不知道这里为什么为空了
            {
               // Debug.LogError("CityBuildingProxy的RolePos为空");
                return;
            }
            RectTransform rt = CoreUtils.uiManager.GetCanvas().transform as RectTransform;
           float m_border = (rt.sizeDelta.x / rt.sizeDelta.y);

            float cityInsideDxf = WorldCamera.Instance().getCameraDxf("city_bound");
            float currCameraDxf = WorldCamera.Instance().getCurrentCameraDxf();
            float p1 = (0.00002889f * currCameraDxf * currCameraDxf - 0.014f * currCameraDxf + 2.058f);
            float p2 = -0.511175f* m_border + 0.8935f;

            bool outSide = dxf > limitDxf || !Common.IsInViewPort2D(WorldCamera.Instance().GetCamera(),
                               m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y,
                            p1 + p2);

            if (outSide != m_lastCityButtonState)
            {
                m_lastCityButtonState = outSide;
                SetCityButton(outSide);
            }
        }

        private void SetCityButton(bool outSide)
        {
            if (view.m_btn_build_GameButton.gameObject.activeSelf == outSide)
                view.m_btn_build_GameButton.gameObject.SetActive(!outSide);
            if (view.m_btn_search_GameButton.gameObject.activeSelf != outSide)
                view.m_btn_search_GameButton.gameObject.SetActive(outSide);


            if (view.m_btn_map_GameButton.gameObject.activeSelf == outSide)
                view.m_btn_map_GameButton.gameObject.SetActive(!outSide);
            if (view.m_btn_city_GameButton.gameObject.activeSelf != outSide)
                view.m_btn_city_GameButton.gameObject.SetActive(outSide);
        }

        #endregion

        #region Guide

        //新手引导某个步骤前隐藏所有UI
        private void OnGuideMainModule()
        {
            if ((CurrentMainMenuState & (int) MainMenuState.OnGuide) <= 0)
            {
                CurrentMainMenuState |= (int) MainMenuState.OnGuide;
            }

            bool isGuideFinished = true;
            foreach (var element in m_moduleDefines)
            {
                if (!m_guideProxy.IsCompletedByStage(element.Value.guideStage) && element.Value.guideStage > 0)
                {
                    m_moduleGuideState[element.Key] = false;
                    isGuideFinished = false;
                }
                else
                {
                    m_moduleGuideState[element.Key] = true;
                }
            }

            ShowOrHideCityModule(m_lastZoomLevel);
            if (isGuideFinished)
            {
                CurrentMainMenuState &= ~(int) MainMenuState.OnGuide;
            }
        }

        //新手引导某个步骤强制显示某部分UI
        private void OnForceShowModuleByGuide(GuideDefine currentDefine)
        {
            if (currentDefine != null && currentDefine.showUi != null)
            {
                for (int i = 0; i < currentDefine.showUi.Count; i++)
                {
                    if (m_moduleKeyMap.ContainsKey(currentDefine.showUi[i]))
                    {
                        EnumMainModule type = m_moduleKeyMap[currentDefine.showUi[i]];
                        if (m_modules.ContainsKey(type))
                        {
                            m_moduleGuideState[type] = true;
                            OnShowModule(type, true);
                        }
                    }
                }
            }
        }

        //强制关闭新手引导
        private void ForceCloseGuide()
        {
            foreach (var element in m_moduleDefines)
            {
                m_moduleGuideState[element.Key] = true;
            }

            ShowOrHideCityModule(m_lastZoomLevel);
            CurrentMainMenuState &= ~(int) MainMenuState.OnGuide;
        }

        #endregion

        #region OnWindow

        int m_windowLayerDeep = 0;

        private void OnCloseWindowUI(UIInfo ui)
        {
            if (NeedCountIntoWindowUI(ui))
            {
                m_windowLayerDeep++;
                Debug.Log("窗口增加后剩余：" + m_windowLayerDeep);
                if (ui == UI.s_iF_SearchRes)
                {
                    foreach (var element in m_moduleDefines)
                    {
                        OnShowModule(element.Key, element.Value.openWinSearch == 1);
                    }
                }
                else if (ui == UI.s_createMainTroops || ui == UI.s_scoutSelectMenu)
                {
                    foreach (var element in m_moduleDefines)
                    {
                        OnShowModule(element.Key, element.Value.openWinCreateTroops == 1);
                    }
                }
                else if (ui == UI.s_openFogShow)
                {
                    foreach (var element in m_moduleDefines)
                    {
                        OnShowModule(element.Key, element.Key == EnumMainModule.Map);
                    }
                }
                else if (ui == UI.s_AllianceCreateBuildRes || ui == UI.s_AllianceCreateBuildPopup)
                {
                    foreach (var element in m_moduleDefines)
                    {
                        OnShowModule(element.Key, false);
                    }
                }
                else
                {
                    foreach (var element in m_moduleDefines)
                    {
                        OnShowModule(element.Key, element.Value.openWinDefault == 1);
                    }
                }
            }
            if (ui == UI.s_Taskinfo)
            {
                OnUpdateTaskStatistics();
            }
        }


        private void OnShowWindowUI(UIInfo ui)
        {
            if (ui.info.layer == UILayer.WindowLayer)
            {
                m_windowLayerDeep--;
                Debug.Log("窗口减少后剩余：" + m_windowLayerDeep);
            }

            if (m_windowLayerDeep <= 0)
            {
                m_windowLayerDeep = 0;
                if ((CurrentMainMenuState & (int) MainMenuState.OnWin) > 0)
                {
                    CurrentMainMenuState &= ~(int) MainMenuState.OnWin;
                }
            }
        }

        //是否要隐藏或显示主界面UI
        private bool NeedCountIntoWindowUI(UIInfo ui)
        {
            bool needCount = false;
            if (ui.info.layer == UILayer.WindowLayer)
            {
                if (ui == UI.s_quickUseItem)
                {
                    return false;
                }

                if ((CurrentMainMenuState & (int)MainMenuState.OnWin) <= 0)
                {
                    CurrentMainMenuState |= (int) MainMenuState.OnWin;
                }
                //else if ((CurrentMainMenuState &(int) MainMenuState.OnWin)>0)
                //{
                //    return needCount;
                //}

                needCount = true;
            }

            return needCount;
        }

        #endregion

        #region ChapterChange
        /// <summary>
        /// 评星
        /// </summary>
        private void OpenAppRating()
        {
            if (m_playerProxy.ConfigDefine.reviewsCityAge ==(int)m_cityBuildingProxy.GetAgeType())
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.OpenAppRating);
            }
        }
        private void OnChapterChange(bool ChanpterOn)
        {
            CurrentMainMenuState = ChanpterOn
                ? CurrentMainMenuState | (int) MainMenuState.OnChapterChange
                : CurrentMainMenuState & ~(int) MainMenuState.OnChapterChange;
            if (ChanpterOn)
            {
                foreach (var element in m_moduleDefines)
                {
                    OnShowModule(element.Key, element.Value.ageUpdate == 1);
                }
            }
        }

        #endregion


        #region OnOutSideControl

        private void OnShowOutSideControlModules()
        {
            CurrentMainMenuState &= ~(int)MainMenuState.OnOutSideControl;
        }

        private void OnHideOutSideControlModules(object body)
        {
            if (CurrentMainMenuState == (int)MainMenuState.OnNormal)
            {
                CurrentMainMenuState |= (int)MainMenuState.OnOutSideControl;
                int value = (int)body;
                foreach (var element in m_modules)
                {
                    OnShowModule(element.Key,((int)element.Key & value)<=0);
                }

            }
            else
            {
                Debug.Log("主界面UI状态正在被其他窗口占用");
            }
        }
        #endregion


        #endregion

        #region 充值

        private void ShowChargePop()
        {
            var key = RS.PlayerPrefs_Key_HasShownChargePop + m_playerProxy.Rid;
            var hasShown = PlayerPrefs.GetInt( key, 0);
            var isFirstRecharge = m_playerProxy.CurrentRoleInfo.rechargeFirst;
            if (SystemOpen.IsSystemOpen(EnumSystemOpen.charge_pop,false) && !isFirstRecharge && hasShown == 0)
            {
                PlayerPrefs.SetInt(key , 1);
                CoreUtils.uiManager.ShowUI(UI.s_ChargePop);
                RefreshRechargeIF();
            }
        }

        private void RefreshRechargeIF()
        {
            view.m_UI_Item_MainIFEventCharge.Refresh();
        }
        
        private class LimitPackageObject
        {
            private GameObject gameObject;
            public UI_Item_MainIFEventBtn_SubView view;

            public LimitPackageObject(GameObject gameObject)
            {
                this.gameObject = gameObject;
            }
        }
        
        private Dictionary<long,LimitPackageObject> m_limitPackageObjectMap = new Dictionary<long,LimitPackageObject>();

        private void CheckNewLimitPackage()
        {
            var packageInfos = m_playerProxy.CurrentRoleInfo.limitTimePackage;
            foreach (var KeyValue in packageInfos)
            {
                var packageInfo = KeyValue.Value;
                //  -1 表示过期或已购买
                if (packageInfo != null && packageInfo.id != -1 && packageInfo.expiredTime > 0 &&
                           packageInfo.expiredTime > ServerTimeModule.Instance.GetServerTime())
                {
                    NewlimitPackage(packageInfo);
                }
            }
        }

        private void NewlimitPackage(LimitTimePackage packageInfo)
        {
            if (packageInfo != null)
            {
                if (m_limitPackageObjectMap.ContainsKey(packageInfo.index))
                {
                    return;
                }
                
                var packageDefines = CoreUtils.dataService.QueryRecords<Data.RechargeLimitTimeBagDefine>();
                RechargeLimitTimeBagDefine packageDefine = null;
                foreach (var define in packageDefines)
                {
                    if (define.price == packageInfo.id)
                    {
                        packageDefine = define;
                    }
                }

                if (packageDefine == null) return;
                List<string> prefabNames = new List<string>();
                prefabNames.Add("UI_Item_MainIFEventBtn");
                ClientUtils.PreLoadRes(view.gameObject, prefabNames, (assetDic) =>
                {
                    GameObject gameObject = CoreUtils.assetService.Instantiate(assetDic["UI_Item_MainIFEventBtn"]);
                    gameObject.transform.parent = view.m_UI_Item_Events_CanvasGroup.transform;
                    gameObject.transform.localScale = Vector3.one;
                    
                    LimitPackageObject packageObject = new LimitPackageObject(gameObject);

                    UI_Item_MainIFEventBtn_SubView subView = null;
                    subView = new UI_Item_MainIFEventBtn_SubView(gameObject.GetComponent<RectTransform>());
                    subView.InitData(packageInfo.index,packageDefine.icon,packageDefine.iconBorder, () =>
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_GiftLimit,null,packageInfo);
                        AppFacade.GetInstance().SendNotification(CmdConstant.LimitTimePackageState, true);
                    });
                    subView.Refresh();
                    subView.SetTimer(packageInfo.expiredTime);

                    packageObject.view = subView;
                    
                    m_limitPackageObjectMap.Add(packageInfo.index, packageObject);
                });
            }
        }

        private void RemoveLimitPackage(long packageIndex)
        {
            if (packageIndex <= 0 ) return;
            if (m_limitPackageObjectMap.ContainsKey(packageIndex))
            {
                LimitPackageObject packageObject = m_limitPackageObjectMap[packageIndex];
                if (packageObject.view != null)
                {
                    packageObject.view.Destroy();
                }

                m_limitPackageObjectMap.Remove(packageIndex);
            }
        }

        #endregion

        #region LodMenu

        private void OnCameraMoveLodMneuFunc()
        {
            view.m_UI_Item_LodMenu.OnCaveAndVillage(m_moduleState[EnumMainModule.LodMenu]);
            view.m_UI_Item_LodMenu.OnAllianceBuilding(m_moduleState[EnumMainModule.LodMenu]);
            view.m_UI_Item_LodMenu.OnRssMiniMap(m_moduleState[EnumMainModule.LodMenu]);
            view.m_UI_Item_LodMenu.OnPersonMapMarker(m_moduleState[EnumMainModule.LodMenu]);
        }

        #endregion

        #region MiniMap
        private void RefreshMinimapViewArea()
        {
            if (!m_modules.ContainsKey(EnumMainModule.Map) || !m_moduleState[EnumMainModule.Map])
            {
                return;
            }

            view.m_UI_Item_Map.RefreshMinimapViewArea();
        }
        #endregion

        //背包
        private void OnBag()
        {
            if (SystemOpen.IsSystemOpen(EnumSystemOpen.bag))
            {
                CoreUtils.uiManager.ShowUI(UI.s_bagInfo);
            }
        }

        private void OnTroop()
        {
            CoreUtils.uiManager.ShowUI(UI.s_WinArmyShow, null, 1011);
        }

        private void OnSearch()
        {
            CoreUtils.uiManager.ShowUI(UI.s_iF_SearchRes);
        }

        #region 城市buff
        private void InitCityBuffData()
        {
            m_objCityBuff = view.m_UI_Item_PlayerPowerInfo.m_UI_Item_MainIFBuff.gameObject;
            view.m_UI_Item_PlayerPowerInfo.m_UI_Item_MainIFBuff.gameObject.SetActive(false);
        }

        private void RefreshCityBuffData()
        {
            m_cityBuffDic.Clear();
            
            if (m_playerProxy.CurrentRoleInfo.cityBuff != null)
            {
                m_playerProxy.CurrentRoleInfo.cityBuff.Values.ToList().ForEach((cityBuff) => {
                    if (cityBuff.expiredTime != -2)
                    {
                        CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)cityBuff.id);
                        if (cityBuffDefine != null)
                        {
                            if (m_cityBuffDic.ContainsKey(cityBuffDefine.group))
                            {
                            }
                            else
                            {
                                m_cityBuffDic.Add(cityBuffDefine.group, cityBuff);
                            }
                    }
                    }
                });
                m_cityBuffList =  m_cityBuffDic.Values.ToList();
            m_cityBuffList.Sort((x, y) => (int)(x.id - y.id));
            }

        }

        private void PlayCollectRuneFinishEffect(INotification notification)
        {
            Role_RoleNotify.request notify = notification.Body as Role_RoleNotify.request;
            if (notify == null || notify.numArg.Count < 2) return;
            int troopId = (int)notify.numArg[0];
            int runeItemTypeId = (int)notify.numArg[1];
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId((int)troopId);
            if (armyData == null || armyData.go == null) return;
            if (!Common.IsInViewPort(WorldCamera.Instance().GetCamera(), armyData.go.transform.position, 0))
            {
                return;
            }
            var mapItemTypeCfg = CoreUtils.dataService.QueryRecord<Data.MapItemTypeDefine>(runeItemTypeId);
            if (mapItemTypeCfg == null) return;
          
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            if (mt == null) return;
            foreach(var subView in m_cityBuffViewList)
            {
                if (subView == null) continue;
                bool isFound = false;
                foreach(var buffId in mapItemTypeCfg.buffData)
                {
                    if(buffId == subView.CityBuffId)
                    {
                        CityBuffDefine cityBuffCfg = CoreUtils.dataService.QueryRecord<CityBuffDefine>(buffId);
                        if(cityBuffCfg != null && !String.IsNullOrEmpty(cityBuffCfg.icon))
                        {
                            isFound = true;
                            break;
                        }
                    }
                }
                if (!isFound) continue;
                Vector2 pos;
                var screenPos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), armyData.go.transform.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform,
                    screenPos, CoreUtils.uiManager.GetUICamera(), out pos);
                mt.FlyBuffEffect(mapItemTypeCfg, pos, subView.m_root_RectTransform, () =>
                {

                });
                break;
            }

            var configCfg = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0);
            if (configCfg == null) return;
            CoreUtils.assetService.Instantiate(configCfg.collectFinishEffect, (obj) =>
            {
                ArmyData data = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId((int)troopId);
                if(data == null || data.go == null)
                {
                    CoreUtils.assetService.Destroy(obj);
                    return;
                }
                obj.transform.SetParent(data.go.transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
               
            });
        }

        private void RefreshCityBuffView()
        {
            view.m_UI_Item_PlayerPowerInfo.m_UI_Item_MainIFBuff.gameObject.SetActive(false);
            int count = view.m_UI_Item_PlayerPowerInfo.m_Pl_Buffs_GridLayoutGroup.transform.childCount; ;
            for (int i = count - 1; i > 0; i--)
            {
                CoreUtils.assetService.Destroy(view.m_UI_Item_PlayerPowerInfo.m_Pl_Buffs_GridLayoutGroup.transform.GetChild(i).gameObject);
            }
            m_cityBuffViewList.Clear();
            if (m_playerProxy.CurrentRoleInfo.cityBuff != null)
            {
                m_cityBuffList.ForEach((cityBuff) =>
                {
                    if (cityBuff.expiredTime != -2)
                    {
                        CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)cityBuff.id);
                        CityBuffGroupDefine cityBuffGroupDefine = CoreUtils.dataService.QueryRecord<CityBuffGroupDefine>((int)cityBuffDefine.group);
                        if (cityBuffDefine != null&& cityBuffGroupDefine!=null)
                        {
                            GameObject obj = CoreUtils.assetService.Instantiate(m_objCityBuff);
                            obj.transform.SetParent(view.m_UI_Item_PlayerPowerInfo.m_Pl_Buffs_GridLayoutGroup.transform);
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
                            m_cityBuffViewList.Add(SubView);
                        }
                    }

                });
            }
        }
        #endregion
        #region 任务相关

        private bool m_taskGuideReminding = false;
        private void JumpGuideTask(int type)
        {
            if (m_playerProxy == null || m_playerProxy.CurrentRoleInfo == null)
            {
                return;
            }
            int findIndex = -1;
            if (type == 1)
            {
                if (m_playerProxy.CurrentRoleInfo.mainLineTaskId > 0)
                {
                    if (m_TaskContentList != null)
                    {
                        for (int i = 0; i < m_TaskContentList.Count; i++)
                        {
                            if (m_TaskContentList[i].taskPageType == EnumTaskPageType.TaskMain &&
                                m_TaskContentList[i].taskMainDefine != null && m_TaskContentList[i].taskMainDefine.ID == m_playerProxy.CurrentRoleInfo.mainLineTaskId)
                            {
                                findIndex = i;
                                break;
                            }
                        }
                    }
                }
                if (findIndex < 0 && m_playerProxy.CurrentRoleInfo.chapterId > 0)
                {
                    if (m_TaskContentList != null)
                    {
                        for (int i = 0; i < m_TaskContentList.Count; i++)
                        {
                            if (m_TaskContentList[i].taskPageType == EnumTaskPageType.TaskChapter)
                            {
                                findIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                findIndex = 0;
            }
           
            if (m_assetsReady && findIndex>=0)
            {
                m_taskGuideReminding = false;
                OnUpdateTaskStatistics();
                view.m_sv_list_tasks_ListView.MovePanelToItemIndex(findIndex);

                ListView.ListItem listItem = view.m_sv_list_tasks_ListView.GetItemByIndex(findIndex);
                if (listItem != null)
                {
                    FingerTargetParam param = new FingerTargetParam();
                    param.AreaTarget = listItem.go;
                    param.ArrowDirection = LanguageUtils.IsArabic()? (int)EnumArrorDirection.Left:(int)EnumArrorDirection.Right;
                    param.IsShowGuideAreaBorder = true;
                    param.IsAutoClose = false;
                    param.IsTouchBeginClose = true;
                    param.SourceType = 1;
                    CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                }
                m_taskGuideReminding = true;
            }
        }

        /// <summary>
        /// 刷新任务数据 主线和章节没有变动
        /// </summary>
        private void OnUpdateTaskStatistics()
        {
            if (m_assetsReady)
            {
                if (m_taskGuideReminding)
                {
                    m_taskGuideReminding = false;
                    AppFacade.GetInstance().SendNotification(CmdConstant.CancelGuideRemind, 1);
                }
                if (!taskAnimShow)
                {
                    RefreshTaskData();
                    RefreshTaskRedPointView();
                    view.m_sv_list_tasks_ListView.FillContent(m_TaskContentList.Count);
                    view.m_sv_list_tasks_ListView.ForceRefresh();
                }
            }
        }

        private void RefreshTaskData()
        {
            m_mainLineTaskId = m_playerProxy.CurrentRoleInfo.mainLineTaskId;
            m_chapterId = m_playerProxy.CurrentRoleInfo.chapterId;
            m_curTaskMain = CoreUtils.dataService.QueryRecord<TaskMainDefine>((int) m_mainLineTaskId);
            m_curTaskChapterData = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>((int) m_chapterId);
            m_TaskContentList = m_taskProxy.GetTaskList();
            m_taskchapterFinishCount = m_taskProxy.GetTaskChapterFinishCount(m_chapterId, out m_taskchapterCount);
        }
    private void PlayAnimEnd(ListView.ListItem listItem)
    {
        if (listItem.data is TaskData)
        {
            TaskData taskData= listItem.data as TaskData;
                if (taskData.taskPageType == EnumTaskPageType.TaskMain)
                {
                    taskAnimShow = false;
                    OnUpdateTaskStatistics();
                }
                else if ( taskData.taskPageType == EnumTaskPageType.TaskSide)
                {
                    view.m_sv_list_tasks_ListView.RemoveAt(m_deleteItem.index);
                    m_deleteItem = null;
                    taskAnimShow = false;
                    OnUpdateTaskStatistics();
                }
            }
    }
        public void PlayTaskMainAnim(ListView.ListItem ListItem, Action action)
        {
            if (ListItem.data is TaskData)
            {
                taskAnimShow = true;
                TaskData taskData = ListItem.data as TaskData;
                GameObject obj = ListItem.go;
                RectTransform rt = obj.transform.GetComponent<RectTransform>();
                MaskableGraphic[] MaskableGraphics = obj.GetComponentsInChildren<MaskableGraphic>();

                //for (int i = 0; i < MaskableGraphics.Length; i++)
                //{
                //    int j = i;
                //    MaskableGraphic maskableGraphics = MaskableGraphics[j];
                //    float myValue2 = maskableGraphics.color.a;
                //    if (myValue2 != 0)
                //    {
                //        DOTween.To(() => myValue2, x => myValue2 = x, 0, 0.3f).SetUpdate(true).OnUpdate(() =>
                //        {
                //            maskableGraphics.color = new Color(maskableGraphics.color.r, maskableGraphics.color.g, maskableGraphics.color.b, myValue2);
                //        }).OnComplete(() =>
                //        {
                //            maskableGraphics.color = new Color(maskableGraphics.color.r, maskableGraphics.color.g, maskableGraphics.color.b, 1);
                //        });
                //    }
                //}
                obj.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).SetUpdate(true).OnComplete(() =>
                {
                    if (ListItem != null && ListItem.go != null)
                    {
                        action?.Invoke();
                    }

                });
            }
        }

    /// <summary>
    /// 刷新任务侧边框
    /// </summary>
    private void RefreshTask()
        {
            RefreshTaskRedPointView();
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemTaskEnter;
            view.m_sv_list_tasks_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list_tasks_ListView.FillContent(m_TaskContentList.Count);
        }

        private void RefreshTaskRedPointView()
        {
            int redPoint = m_taskProxy.GetRedPoint();
            string s_redPoint = UIHelper.NumerBeyondFormat(redPoint);
            view.m_img_taskredpot_PolygonImage.gameObject.SetActive(redPoint != 0);
            view.m_lbl_taskreddotcount_LanguageText.text = s_redPoint;
        }


        void ItemTaskEnter(ListView.ListItem scrollItem)
        {
            int index = scrollItem.index;
            TaskData taskData = m_TaskContentList[scrollItem.index];
            scrollItem.data = taskData;
            if (taskData.taskPageType == EnumTaskPageType.TaskChapter)
            {
                SetTaskChapterItem(scrollItem, taskData, index);
            }
            else
            {
                SetTaskMainSideItem(scrollItem, taskData, index);
            }
        }
        private void SetTaskChapterItem(ListView.ListItem listItem, TaskData taskData, int index)
        {
            UI_Item_HotTaskView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_HotTaskView>(listItem.go);
            itemView.m_pl_effect_ArabLayoutCompment.gameObject.SetActive(m_taskchapterFinishCount == m_taskchapterCount);
            itemView.m_lbl_title_LanguageText.text = LanguageUtils.getText(m_curTaskChapterData.l_titleNameId);
            itemView.m_lbl_title_LanguageText.color = ClientUtils.StringToHtmlColor("#ECAE55");
            itemView.m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(700014,
                LanguageUtils.getText(m_curTaskChapterData.l_descId1),
                LanguageUtils.getTextFormat(181104, m_taskchapterFinishCount, m_taskchapterCount));
            itemView.m_lbl_desc_LanguageText.fontSize = fontsize;
            BestFit(itemView.m_lbl_desc_LanguageText.rectTransform.sizeDelta.y, itemView.m_lbl_desc_LanguageText);
            itemView.m_img_bg_GameButton.onClick.RemoveAllListeners();
            itemView.m_img_bg_GameButton.onClick.AddListener(() => { CoreUtils.uiManager.ShowUI(UI.s_Taskinfo); });
            itemView.gameObject.name = m_curTaskChapterData.ID.ToString();
        }

        private void SetTaskMainSideItem(ListView.ListItem listItem, TaskData taskData, int index)
        {
            UI_Item_HotTaskView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_HotTaskView>(listItem.go);
            if (taskData.taskPageType == EnumTaskPageType.TaskMain)

            {
                itemView.m_lbl_title_LanguageText.color = ClientUtils.StringToHtmlColor("#ECAE55");
            }
            else if (taskData.taskPageType == EnumTaskPageType.TaskSide)
            {
                itemView.m_lbl_title_LanguageText.color = ClientUtils.StringToHtmlColor("#FFFFFF");
            }
            switch (taskData.taskState)
            {
                case TaskState.received:
                    {
                        Debug.LogError("已完成的任务");
                    }
                    break;
                case TaskState.finished:
                    {
                        itemView.m_pl_effect_ArabLayoutCompment.gameObject.SetActive(true);

                        itemView.m_img_bg_GameButton.onClick.RemoveAllListeners();
                        itemView.m_img_bg_GameButton.onClick.AddListener(() =>
                        {
                            OnTaskDataItemClick(listItem);
                        });
                    }
                    break;
                case TaskState.unfinished:
                    {
                        itemView.m_pl_effect_ArabLayoutCompment.gameObject.SetActive(false);
                        itemView.m_img_bg_GameButton.onClick.RemoveAllListeners();
                        itemView.m_img_bg_GameButton.onClick.AddListener(() =>
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.GoScript, taskData);
                        });
                    }
                    break;
            }

            itemView.m_lbl_title_LanguageText.text = LanguageUtils.getText(taskData.l_nameId);
            itemView.m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(700014, (taskData.desc),
                LanguageUtils.getTextFormat(181104, taskData.Num, taskData.needNum));
            itemView.m_lbl_desc_LanguageText.fontSize = fontsize;
            BestFit(itemView.m_lbl_desc_LanguageText.rectTransform.sizeDelta.y, itemView.m_lbl_desc_LanguageText);
            itemView.gameObject.name = taskData.taskID.ToString();
        }

        void OnTaskDataItemClick(ListView.ListItem listItem)
        {
            if (listItem.data is TaskData)
            {
                TaskData taskData = listItem.data as TaskData;
                if (taskData.taskState != TaskState.finished)
                {
                    return;
                }
                if (taskAnimShow)
                {
                    return;
                }
                Task_TaskFinish.request req = new Task_TaskFinish.request();
                req.taskId = taskData.taskID;
                AppFacade.GetInstance().SendSproto(req);
                FlyRewardEffect(listItem.go.transform, taskData.rewardGroupDataList);
                //   Debug.LogError(taskDataItem.taskData.desc);
                if (taskData.taskPageType == EnumTaskPageType.TaskMain)
                {
                    PlayTaskMainAnim(listItem, () =>
                    {
                        PlayAnimEnd(listItem);
                    });
                }
                else
                {
                    m_deleteItem = listItem;
                    Animation ani = listItem.go.GetComponent<Animation>();
                    AnimationClip clip = ani.GetClip("MissionItemRemove");
                    m_deleteTime = clip.length;
                    taskAnimShow = true;
                    m_height = view.m_sv_list_tasks_ListView.GetItemSizeByIndex(m_deleteItem.index);
                    if (ani != null)
                    {
                        ani.Play("MissionItemRemove");
                    }
                }
            }
        }
        private void FlyRewardEffect(Transform rewards, List<RewardGroupData> rewardGroupDataList)
        {
            if (rewards != null)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.TipRewardGroup, rewardGroupDataList);
                for (int i = 0; i < rewardGroupDataList.Count; i++)
                {
                    RewardGroupData rewardGroupData = rewardGroupDataList[i];
                    switch ((EnumRewardType) rewardGroupData.RewardType)
                    {
                        case EnumRewardType.Currency:
                        {
                            GlobalEffectMediator mt =
                                AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as
                                    GlobalEffectMediator;
                            mt.FlyUICurrency(rewardGroupData.CurrencyData.ID, rewardGroupData.number,
                                rewards.transform.position);
                        }
                            break;
                        case EnumRewardType.Soldier:
                            {
                                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;

                                    CoreUtils.assetService.LoadAssetAsync<Sprite>(rewardGroupData.SoldierData.icon, (asset) =>
                                    {
                                        UnityEngine.Object go = asset.asset() as UnityEngine.Object;
                                        Sprite sprite = go as Sprite;
                                        GameObject tmpObj = new GameObject();
                                        tmpObj.AddComponent<Image>().sprite = sprite;
                                        mt.FlyPowerUpEffect(tmpObj, rewards.GetComponent<RectTransform>(), Vector3.one);
                                        GameObject.DestroyImmediate(tmpObj);
                                    }, view.gameObject);
                                }
                            break;
                        case EnumRewardType.Item:
                        {
                            GlobalEffectMediator mt =
                                AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as
                                    GlobalEffectMediator;
                            mt.FlyItemEffect(rewardGroupData.ItemData.ID, (int)rewardGroupData.number,
                                rewards.GetComponent<RectTransform>());
                        }
                            break;
                    }
                }
            }
        }

        void BestFit(float height, LanguageText text)
        {
            while (text.preferredHeight > height)
            {
                text.fontSize -= 1;
            }
        }

        #endregion

        #region 聊天

        private EnumChatChannel m_chatChannel = EnumChatChannel.world;
        private ChatMsgList m_currentChatList;
        private bool m_canChangeChannel = false;

        private void OnClickChatBtn()
        {
            CoreUtils.uiManager.ShowUI(UI.s_chat);
        }

        bool isChatPageInited;
        private void InitChatPage()
        {
            if (isChatPageInited)
            {
                return;
            }

            isChatPageInited = true;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = OnChatPageItemEnter;
            funcTab.GetItemSize = OnChatPageItemSize;
            funcTab.GetItemPrefabName = OnChatItemPrefabName;
            view.m_sv_list_dialog_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list_dialog_ListView.onDragBegin += OnDialogDragBegin;
            view.m_sv_list_dialog_ListView.onDragEnd += OnDialogDragEnd;
            view.m_sv_list_dialog_ScrollRect.onValueChanged.RemoveAllListeners();
            view.m_sv_list_dialog_ScrollRect.onValueChanged.AddListener(OnChatScrollRectValueChanged);
            UIEventTrigger trigger =  view.m_sv_list_dialog_ListView.gameObject.AddComponent<UIEventTrigger>();
            trigger.onPointerClick = OnDialogClick;
            OnShowOrHideAlliancePage();
        }

        private Vector2 tmpMousePos;
        private void OnDialogDragBegin(PointerEventData data)
        {
            tmpMousePos = data.position;
        }

        private void OnDialogDragEnd(PointerEventData data)
        {
            if(Mathf.Abs(data.position.y-tmpMousePos.y)>= view.m_sv_list_dialog_PolygonImage.rectTransform.rect.height/2f)
            {
                return;
            }
            if(data.position.x-tmpMousePos.x>= view.m_sv_list_dialog_PolygonImage.rectTransform.rect.width/3f)
            {
                if(m_chatChannel!= EnumChatChannel.world)
                {
                    m_chatChannel = EnumChatChannel.world;
                    RefreshChatPage();
                }
            }
            else if(data.position.x - tmpMousePos.x<=-view.m_sv_list_dialog_PolygonImage.rectTransform.rect.width / 3f)
            {
                if (m_chatChannel != EnumChatChannel.alliance&& m_allianceProxy.HasJionAlliance())
                {
                    m_chatChannel = EnumChatChannel.alliance;
                    RefreshChatPage();
                }
            }
        }

        private void OnChatScrollRectValueChanged(Vector2 value)
        {
            if (value.y < 0)
            {
                value.y = 0;
                view.m_sv_list_dialog_ScrollRect.normalizedPosition = value;
            }
            view.m_sv_list_dialog_ListView.ShowContentAt(view.m_sv_list_dialog_ListView.GetContainerPos());
        }

        private void OnDialogClick(PointerEventData data)
        {
            if (!data.dragging)
            {
                //if (m_currentChatList.ChatMsg.Count > 0)
                //{
                //    for (int i = 0; i < m_currentChatList.ChatMsg.Count; i++)
                //    {
                //        var item = view.m_sv_list_dialog_ListView.GetItemByIndex(i);
                //        if (item != null && item.go != null)
                //        {
                //            ExecuteEvents.Execute(item.go, data, ExecuteEvents.pointerClickHandler);
                //        }
                //    }
                //}

                CoreUtils.uiManager.ShowUI(UI.s_chat, null, m_chatChannel);
            }
        }

        private void OnShowOrHideAlliancePage()
        {
            bool show =  m_allianceProxy.HasJionAlliance();
            if(show)
            {
                m_canChangeChannel = true;
                view.m_img_chat2di_PolygonImage.gameObject.SetActive(true);
                view.m_img_chat2_PolygonImage.gameObject.SetActive(true);
                view.m_img_chat2_PolygonImage.color = m_chatChannel == EnumChatChannel.alliance?Color.white:Color.clear;
            }
            else
            {
                m_canChangeChannel = false;
                view.m_img_chat2di_PolygonImage.gameObject.SetActive(false);
                view.m_img_chat2_PolygonImage.gameObject.SetActive(false);
                if(m_chatChannel == EnumChatChannel.alliance)
                {
                    m_chatChannel = EnumChatChannel.world;
                    RefreshChatPage();
                }
            }
        }

        private void RefreshChatPage()
        {
            if(!m_assetsReady)
            {
                return;
            }
            InitChatPage();
            m_chatProxy.InitChatChannel();
            CheckChatRedPoint();
            switch (m_chatChannel)
            {
                case EnumChatChannel.world:
                    view.m_img_chat1_PolygonImage.color = Color.white;
                    view.m_img_chat2_PolygonImage.color = Color.clear;
                    m_currentChatList = m_chatProxy.WorldContact.ChatMsgList;
                    if (!CoreUtils.uiManager.ExistUI(UI.s_chat))
                    {
                        m_chatProxy.WorldContact.SetRead();
                    }

                    bool  hasAnyUnread = m_chatProxy.AllianceContact.GetUnreadCount() > 0;
                    if(hasAnyUnread)
                    {
                        view.m_img_chat2_Animator.enabled = true;
                        view.m_img_chat2_Animator.Play("Flash");
                    }
                    else
                    {
                        view.m_img_chat2_Animator.enabled = false;
                    }
                    break;
                case EnumChatChannel.alliance:
                    view.m_img_chat2_Animator.enabled = false;
                    view.m_img_chat1_Animator.enabled = false;
                    view.m_img_chat1_PolygonImage.color = Color.clear;
                    view.m_img_chat2_PolygonImage.color = Color.white;
                    m_currentChatList = m_chatProxy.AllianceContact.ChatMsgList;
                    if (!CoreUtils.uiManager.ExistUI(UI.s_chat))
                    {
                        m_chatProxy.WorldContact.SetRead();
                    }
                    break;
                default: break;
            }
            
            view.m_sv_list_dialog_ListView.FillContent(m_currentChatList.ChatMsg.Count);
            view.m_sv_list_dialog_ListView.MovePanelToItemIndex(m_currentChatList.ChatMsg.Count - 1);
            view.m_sv_list_dialog_ListView.autoScrollTime = 0.4f;
            view.m_sv_list_dialog_ListView.ScrollToPos(view.m_sv_list_dialog_ListView.listContainer.rect.height - view.m_sv_list_dialog_PolygonImage.rectTransform.rect.height);
            //view.m_sv_list_dialog_ListView.ScrollToPos(view.m_sv_list_dialog_ListView.listContainer.rect.height-view.m_sv_list_dialog_PolygonImage.rectTransform.rect.height);
        }
        private float OnChatPageItemSize(ListView.ListItem item)
        {
            return 25f;
        }
        private string OnChatItemPrefabName(ListView.ListItem item)
        {
            int index = item.index;
            ChatMsg msgData = m_currentChatList.ChatMsg[index];
            switch (msgData.msgType)
            {
                case EnumMsgType.ChatShare:
                    {
                        return "UI_Item_ChatDialogLink";
                    }
                    break;
                default:
                    {
                        return "UI_Item_ChatDialog";
                    }
                    break;
            }
        }

        private void OnChatPageItemEnter(ListView.ListItem item)
        {
            int index = item.index;
            ChatMsg msgData = m_currentChatList.ChatMsg[index];
            switch (msgData.msgType)
            {
                case EnumMsgType.ChatShare:
                    {
                        SettemChatDialogLinkView(item, msgData, index);
                    }
                    break;
                default:
                    {
                        SettemChatDialogView(item, msgData, index);
                    }
                    break;
            }
        }

        private void SettemChatDialogView(ListView.ListItem item, ChatMsg msgData, int index)
        {
            UI_Item_ChatDialog_SubView itemView = null;
            if (item.data == null)
            {
                itemView = new UI_Item_ChatDialog_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            else
            {
                itemView = item.data as UI_Item_ChatDialog_SubView;
            }
            if (msgData.msg == null || msgData.msg == null)
            {
                view.m_sv_list_dialog_ListView.UpdateItemSize(index, 0);
                itemView.m_lbl_name_LanguageText.text = string.Empty;
                itemView.m_lbl_mes_LanguageText.text = string.Empty;
                return;
            }

            string chatMsg;
            if (msgData.msgType == EnumMsgType.Emoji)
            {
                chatMsg = LanguageUtils.getTextFormat(730138, msgData.msg);
            }
            else if (msgData.msgType == EnumMsgType.ATUser)
            {
              m_chatProxy.ConvertStringToMainViewMapMarkerType(msgData, out chatMsg);
            }
            else 
            {
                chatMsg = msgData.msg;
            }
            string msg = null;
            if (msgData.msgType == EnumMsgType.Announcement)
            {
                msg = LanguageUtils.getText(750080);
            }
            else
            {
                msg = msgData.contactInfo.name;
            }

            if (!string.IsNullOrEmpty(msgData.contactInfo.guildName))
            {
                itemView.m_lbl_ali_LanguageText.text =
                    LanguageUtils.getTextFormat(300278, msgData.contactInfo.guildName);
            }
            else
            {
                itemView.m_lbl_ali_LanguageText.text = String.Empty;
            }

            itemView.m_lbl_name_LanguageText.text = msg;

            var sizeDelta = itemView.m_lbl_mes_LanguageText.rectTransform.sizeDelta;
            sizeDelta.x = itemView.m_root_RectTransform.rect.width - itemView.m_lbl_name_LanguageText.preferredWidth - itemView.m_lbl_ali_LanguageText.preferredWidth;
            itemView.m_lbl_mes_LanguageText.rectTransform.sizeDelta = sizeDelta;

            UIHelper.SetTextWithEllipsis(itemView.m_lbl_mes_LanguageText, chatMsg, "...");
            //            view.m_sv_list_dialog_ListView.UpdateItemSize(index, itemView.m_lbl_text_LanguageText.preferredHeight + 4f);
        }
        private void SettemChatDialogLinkView(ListView.ListItem item, ChatMsg msgData, int index)
        {
            UI_Item_ChatDialogLink_SubView itemView = null;
            if (item.data == null)
            {
                itemView = new UI_Item_ChatDialogLink_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            else
            {
                itemView = item.data as UI_Item_ChatDialogLink_SubView;
            }
            if (msgData.msg == null || msgData.msg == null)
            {
                view.m_sv_list_dialog_ListView.UpdateItemSize(index, 0);
                itemView.m_lbl_name_LanguageText.text = string.Empty;
                itemView.m_lbl_linkmes_HrefText.text = string.Empty;
                return;
            }

            string chatMsg;
            if (msgData.msgType == EnumMsgType.Emoji)
            {
                chatMsg = LanguageUtils.getTextFormat(730138, msgData.msg);
            }
            else
            {
                chatMsg = msgData.msg;
            }
            string msg = null;
            string coordinate = "";
            if (msgData.msgType == EnumMsgType.Announcement)
            {
                msg = LanguageUtils.getText(750080);
            }
            else
            {
                msg = msgData.contactInfo.name;
            }

            if (!string.IsNullOrEmpty(msgData.contactInfo.guildName))
            {
                itemView.m_lbl_ali_LanguageText.text =
                    LanguageUtils.getTextFormat(300278, msgData.contactInfo.guildName);
            }
            else
            {
                itemView.m_lbl_ali_LanguageText.text = String.Empty;
            }

            itemView.m_lbl_name_LanguageText.text = msg;
            var sizeDelta = itemView.m_lbl_linkmes_HrefText.rectTransform.sizeDelta;
            sizeDelta.x = itemView.m_root_RectTransform.rect.width - itemView.m_lbl_name_LanguageText.preferredWidth - itemView.m_lbl_ali_LanguageText.preferredWidth;
            itemView.m_lbl_linkmes_HrefText.rectTransform.sizeDelta = sizeDelta;
            itemView.m_lbl_name_LanguageText.text = msg;
            if (m_chatProxy.ConvertStringToMainView(msgData, out chatMsg,out coordinate))
            {
                itemView.m_lbl_linkmes_HrefText.raycastTarget = true;
                UIHelper.SetHrefTextWithEllipsis(itemView.m_lbl_linkmes_HrefText, chatMsg, "...");
                itemView.m_lbl_linkmes_HrefText.onHrefClick.RemoveAllListeners();
                itemView.m_lbl_linkmes_HrefText.onHrefClick.AddListener((str) =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, coordinate);
                });
            }
            else
            {
                UIHelper.SetTextWithEllipsis(itemView.m_lbl_linkmes_HrefText, chatMsg, "...");
            }


            //            view.m_sv_list_dialog_ListView.UpdateItemSize(index, itemView.m_lbl_text_LanguageText.preferredHeight + 4f);
        }

        private void UpdateChatRedDot()
        {
            var show = m_chatProxy.HasNewMsg;
            //TODO:显示红点
        }

        #endregion


        #region 战斗预警

        private void OnClickShowWarWarning()
        {
            CoreUtils.uiManager.ShowUI(UI.s_warWarning);
        }

        private void ClearWarWarning()
        {
            if (m_warWarningTimer != null)
            {
                m_warWarningTimer.Cancel();
                m_warWarningTimer = null;
            }
            if (m_warWarningSplashObject != null)
            {
                CoreUtils.assetService.Destroy(m_warWarningSplashObject);
                m_warWarningSplashObject = null;
            }
        }

        private void OnWarWarningInfoChanged()
        {
            if (m_warWarningProxy == null) return;
            var lastInfo = m_warWarningProxy.GetLastNotIgnoreWarning();
            if (lastInfo == null)
            {
                lastInfo = m_warWarningProxy.GetLastWarning();               
            }
            if(lastInfo == null)
            {
                m_lastWarWarningType = WarWarningType.None;
                view.m_UI_Item_warn_CanvasGroup.gameObject.SetActive(false);
                ClearWarWarning();
                return;
            }
            view.m_UI_Item_warn_CanvasGroup.gameObject.SetActive(true);
            ClientUtils.PlaySpine(view.m_UI_Model_Warning.m_UI_Model_Warning_SkeletonGraphic, WarWarningProxy.GetWarningSkinName((WarWarningType)lastInfo.earlyWarningType, lastInfo.isRally),
                "idle", true);
           
            if(lastInfo.isShield)
            {
                m_lastWarWarningType = WarWarningType.None;
                ClearWarWarning();
                return;
            }
            var configCfg = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if (configCfg == null) return;
            if (!IsNeedBreakLastWarning(lastInfo))
            {
                if(lastInfo.earlyWarningType == (long)WarWarningType.Reinforce)
                {
                    m_warWarningSplashTimes = configCfg.reinforceTips;
                }
                return;
            }
            m_lastWarWarningType = (WarWarningType)lastInfo.earlyWarningType;
            switch(m_lastWarWarningType)
            {
                case WarWarningType.Reinforce:
                case WarWarningType.Transport:
                    CoreUtils.assetService.Instantiate("UE_IF_WarnFlash_2", (go) =>
                    {
                        ClearWarWarning();
                        if (m_lastWarWarningType == WarWarningType.None)
                        {
                            return;
                        }
                        m_warWarningSplashObject = go;
                        go.transform.SetParent(view.gameObject.transform);
                        RectTransform rect = m_warWarningSplashObject.transform as RectTransform;
                        if (rect != null)
                        {
                            rect.offsetMin = Vector2.zero;
                            rect.offsetMax = Vector2.zero;
                        }
                        go.transform.localScale = Vector3.one;
                        if (configCfg != null)
                        {
                            m_warWarningSplashTimes = configCfg.reinforceTips;
                        }
                        var animator = go.GetComponentInChildren<Animator>();
                        float splashOnceTime = ClientUtils.GetAnimationLength(animator, 0);
                        m_warWarningTimer = Timer.Register(splashOnceTime,  () =>
                        {
                            m_warWarningSplashTimes--;
                            if(m_warWarningSplashTimes == 0)
                            {
                                m_lastWarWarningType = WarWarningType.None;
                                ClearWarWarning();
                            }
                        },null, true);
                    });
                    break;
                case WarWarningType.Scout:
                case WarWarningType.War:
                    CoreUtils.assetService.Instantiate("UE_IF_WarnFlash_1", (go) =>
                    {
                        ClearWarWarning();
                        if (m_lastWarWarningType == WarWarningType.None)
                        {
                            return;
                        }                        
                        m_warWarningSplashObject = go;
                        m_warWarningSplashObject.SetActive(true);
                        m_warWarningSplashObject.transform.SetParent(view.gameObject.transform);
                        RectTransform rect = m_warWarningSplashObject.transform as RectTransform;
                        if(rect != null)
                        {
                            rect.offsetMin = Vector2.zero;
                            rect.offsetMax = Vector2.zero;
                        }
                        m_warWarningSplashObject.transform.localScale = Vector3.one;
                    });
                    break;
            }
        }


        private bool IsNeedBreakLastWarning(EarlyWarningInfoEntity lastInfo)
        {
            bool needBreak = true;
            switch(m_lastWarWarningType)
            {
                case WarWarningType.Reinforce:
                    needBreak = lastInfo.earlyWarningType != (long)m_lastWarWarningType;
                    break;
                case WarWarningType.Scout:
                case WarWarningType.War:
                    needBreak = lastInfo.earlyWarningType != (long)WarWarningType.Scout &&
                        lastInfo.earlyWarningType != (long)WarWarningType.War;
                    break;
            }
            return needBreak;
        }
        #endregion

        #region 战役相关

        private void RefreshBattleMainRedPoint()
        {
            var battleMain = AppFacade.GetInstance().RetrieveMediator(BattleMainlMediator.NameMediator) as BattleMainlMediator;
            int num = battleMain.CalculateRedPointNumber();
            OnBattleMainRedPointChanged(num);
        }

        private void OnBattleMainRedPointChanged(int num)
        {
            view.m_UI_Model_War.m_img_redpoint_PolygonImage.gameObject.SetActive(num > 0);
            view.m_UI_Model_War.m_lbl_count_LanguageText.text = num.ToString();
        }
        #endregion


        private void OnNetWorkReconnecting()
        {
            m_warWarningProxy.Clear();
            m_lastWarWarningType = WarWarningType.None;
            view.m_UI_Item_warn_CanvasGroup.gameObject.SetActive(false);
            ClearWarWarning();
        }

        private void CheckAccountBindReddot(bool isShow)
        {
            view.m_UI_Item_PlayerPowerInfo.m_UI_Common_Redpoint.gameObject.SetActive(isShow);
        }
    }
}