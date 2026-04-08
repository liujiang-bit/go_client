using Skyunion;
using Game;
using System.Collections.Generic;

public class UI
{

    #region ViewInfo
    private static UIViewInfo s_hudView = new UIViewInfo(UIViewType.hud, UILayer.HUDLayer, UIAddMode.Stack, UICloseMode.PopWin);

    //加载界面模型
    private static UIViewInfo s_loading = new UIViewInfo(UIViewType.FullView, UILayer.LoadingLayer, UIAddMode.Stack, UICloseMode.Hide);
    private static UIViewInfo s_battleLoading = new UIViewInfo(UIViewType.FullView, UILayer.LoadingLayer, UIAddMode.Stack, UICloseMode.PopWin);
    //战斗loading
    private static UIViewInfo s_story = new UIViewInfo(UIViewType.FullView, UILayer.StoryLayer, UIAddMode.Stack, UICloseMode.PopWin);
    //全屏菜单
    private static UIViewInfo s_fullViewMenu = new UIViewInfo(UIViewType.FullView, UILayer.FullViewMenuLayer, UIAddMode.Stack, UICloseMode.Hide);
    //全屏
    private static UIViewInfo s_windowMenu = new UIViewInfo(UIViewType.FullView, UILayer.WindowMenuLayer, UIAddMode.Stack, UICloseMode.PopWin);

    //菜单
    private static UIViewInfo s_fullViewMenuClose = new UIViewInfo(UIViewType.FullView, UILayer.FullViewMenuLayer, UIAddMode.Stack, UICloseMode.PopWin);


    //聊天窗口
    private static UIViewInfo s_chatview = new UIViewInfo(UIViewType.Window, UILayer.ChatLayer, UIAddMode.Stack, UICloseMode.PopWin);

    //全屏窗口模型
    private static UIViewInfo s_fullWindow = new UIViewInfo(UIViewType.FullView, UILayer.FullViewLayer, UIAddMode.Replace, UICloseMode.PopWin);

    //弹出窗口模型
    private static UIViewInfo s_popWin = new UIViewInfo(UIViewType.Window, UILayer.WindowLayer, UIAddMode.Stack, UICloseMode.PopWin);
    //弹出窗口的弹出窗口模型
    private static UIViewInfo s_popWinPop = new UIViewInfo(UIViewType.Window, UILayer.WindowPopLayer, UIAddMode.Stack, UICloseMode.PopWin);

    //清空所有窗口模型
    private static UIViewInfo s_popAllWin = new UIViewInfo(UIViewType.Window, UILayer.WindowLayer, UIAddMode.Stack, UICloseMode.PopAll);

    //弹出窗口模型,关闭是隐藏
    private static UIViewInfo s_popWinHide = new UIViewInfo(UIViewType.Window, UILayer.WindowLayer, UIAddMode.Stack, UICloseMode.Hide);
    //弹出窗口模型,关闭是隐藏
    private static UIViewInfo s_popWinPopHide = new UIViewInfo(UIViewType.Window, UILayer.WindowPopLayer, UIAddMode.Stack, UICloseMode.Hide);
    //功能开启
    private static UIViewInfo s_systemOpen = new UIViewInfo(UIViewType.Window, UILayer.SystemOpenLayer, UIAddMode.Stack, UICloseMode.PopWin);
    //引导
    private static UIViewInfo s_guide = new UIViewInfo(UIViewType.Window, UILayer.GuideLayer, UIAddMode.Stack, UICloseMode.Hide);
    //浏览器
    private static UIViewInfo s_browserViewInfo = new UIViewInfo(UIViewType.Window,UILayer.BrowserLayer,UIAddMode.Stack,UICloseMode.PopWin);
    //角色头顶资源
    private static UIViewInfo s_playerResViewInfo = new UIViewInfo(UIViewType.Window,UILayer.WindowPopLayer,UIAddMode.Replace,UICloseMode.PopWin);
    #endregion


    //-----------------------------------------Logic Info Start Here--------------------------------------------------------------------

    #region 通用菜单模型
    public static UIInfo s_mainInterface = new UIInfo(MainInterfaceView.VIEW_NAME, typeof(MainInterfaceView), s_fullWindow,   EnumMaskStatus.kNone, null, 2000);

    #endregion

    #region 公共模型
    public static UIInfo s_playerResUI = new UIInfo(UI_IF_PlayerResInfoView.VIEW_NAME,typeof(UI_IF_PlayerResInfoView), s_playerResViewInfo, EnumMaskStatus.kNone);
    public static UIInfo[] s_PlayerRes = new UIInfo[1] { s_playerResUI };
    #endregion
    #region 全屏模型
    public static UIInfo s_LoginView = new UIInfo("LoginView",typeof(LoginView), s_windowMenu, EnumMaskStatus.kNone);

    public static UIInfo s_LoginServerConfirm = new UIInfo("LoginServerSelect", typeof(LoginServerConfirm), s_windowMenu, EnumMaskStatus.kNone);

    public static UIInfo s_InputServerIdView = new UIInfo("InputServerId", typeof(InputServerIdView), s_windowMenu, EnumMaskStatus.kNone);

    public static UIInfo s_CreateCharacter = new UIInfo(CreateCharView.VIEW_NAME, typeof(CreateCharView),s_fullWindow,EnumMaskStatus.kNone);

	
    public static UIInfo s_Loading = new UIInfo("UI_IF_Loading",typeof(LoadingView),s_fullWindow,EnumMaskStatus.kNone);
    
    public static UIInfo s_reConnecting = new UIInfo("UI_Common_Spin",typeof(ReConnectView),s_loading,EnumMaskStatus.kNone,null,0,0,true,false);

    #endregion

    #region 弹出窗口模型
    public static UIInfo s_LoginServerSelect = new UIInfo("LoginServerSelect", typeof(LoginServerSelect), s_popWin, EnumMaskStatus.kNone);

    public static UIInfo s_gameTool = new UIInfo("UI_Win_GameTool", typeof(GameToolView), s_browserViewInfo, EnumMaskStatus.kNone);
    public static UIInfo s_gameToolViewSetting = new UIInfo("UI_Win_GameToolViewSetting", typeof(UI_Win_GameToolViewSettingView), s_browserViewInfo, EnumMaskStatus.kNone);
    public static UIInfo s_testUIInfo = new UIInfo("TestUI", typeof(TestUIView), s_popWin,EnumMaskStatus.kNone);
    public static UIInfo s_soundUIInfo = new UIInfo("SoundUI", typeof(SoundUIView), s_popWin, EnumMaskStatus.kNone);
    public static UIInfo s_WorldPanelInfo = new UIInfo("WorldPanel", typeof(WorldPanelView), s_fullViewMenu, EnumMaskStatus.kNone);
    public static UIInfo s_testUICompoment = new UIInfo("UI_Win_TestUICompoment", typeof(TestUICompomentView), s_popWin, EnumMaskStatus.kNone);

    public static UIInfo s_listView = new UIInfo("UI_Win_ListViewTest", typeof(ListViewTestView), s_popWin, EnumMaskStatus.kNone);
    public static UIInfo s_chapterScene = new UIInfo(UI_IF_TaskChapterView.VIEW_NAME, typeof(UI_IF_TaskChapterView),s_popWin,EnumMaskStatus.kNone);

    public static UIInfo s_animationTest = new UIInfo(FullviewAnimationView.VIEW_NAME, typeof(FullviewAnimationView),s_fullViewMenu, EnumMaskStatus.kNone);

    public static UIInfo s_bagInfo = new UIInfo(BagView.VIEW_NAME, typeof(BagView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_bagGiftOpen = new UIInfo(BagGiftOpenView.VIEW_NAME, typeof(BagGiftOpenView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_itemCollection = new UIInfo(UI_Win_ItemCollectionView.VIEW_NAME, typeof(UI_Win_ItemCollectionView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_openFogShow = new UIInfo(UI_Win_OpenFogShowView.VIEW_NAME, typeof(UI_Win_OpenFogShowView), s_popWin, EnumMaskStatus.kNoMaskNoTouch);

    public static UIInfo s_buildCity = new UIInfo(BuildCityView.VIEW_NAME, typeof(BuildCityView), s_popWin, EnumMaskStatus.kTouchClose, null, 1029);
    
    public static UIInfo s_ranking = new UIInfo(UI_Win_RankingView.VIEW_NAME, typeof(UI_Win_RankingView), s_popWin, EnumMaskStatus.kTouchClose);
    
    public static UIInfo s_monument = new UIInfo(UI_Win_MonumentView.VIEW_NAME, typeof(UI_Win_MonumentView), s_popWin, EnumMaskStatus.kTouchClose);

    public static UIInfo s_monumentAlliance = new UIInfo(UI_Win_MonumentAllianceShowView.VIEW_NAME, typeof(UI_Win_MonumentAllianceShowView), s_popWin, EnumMaskStatus.kTouchClose);
    
    public static UIInfo s_Taskinfo = new UIInfo(UI_Win_QuestView.VIEW_NAME, typeof(UI_Win_QuestView), s_popWin, EnumMaskStatus.kOnlyShow, s_PlayerRes, 11000);



    //奖励展示
    public static UIInfo s_rewardGet = new UIInfo(UI_Win_RewardGetView.VIEW_NAME, typeof(UI_Win_RewardGetView), s_popWin, EnumMaskStatus.kTouchClose);
    // 工人小屋界面
    public static UIInfo s_worker = new UIInfo(UI_Win_WorkerView.VIEW_NAME, typeof(UI_Win_WorkerView), s_popWin, EnumMaskStatus.kOnlyShow,s_PlayerRes,1006);
    //建筑升级界面
    public static UIInfo s_buildingUpdate = new UIInfo(UI_Win_BuildingUpdateView.VIEW_NAME, typeof(UI_Win_BuildingUpdateView), s_popWin, EnumMaskStatus.kTouchClose, s_PlayerRes, 1030);
    //建筑加速界面
    public static UIInfo s_buildingSpeedUp = new UIInfo(UI_Win_BuildingSpeedUpView.VIEW_NAME, typeof(UI_Win_BuildingSpeedUpView), s_popWin, EnumMaskStatus.kTouchClose, s_PlayerRes);
    //研究主界面
    public static UIInfo s_ResearchMain = new UIInfo(ResearchMainView.VIEW_NAME,typeof(ResearchMainView),s_popWin,EnumMaskStatus.kTouchClose,s_PlayerRes, 1036);
    //研究升级界面
    public static UIInfo s_ResearchUpdate = new UIInfo(ResearchUpateView.VIEW_NAME,typeof(ResearchUpateView),s_popWinPop,EnumMaskStatus.kTouchClose,s_PlayerRes);
    //城市属性查看界面
    public static UIInfo s_GainEffect = new UIInfo(UI_Win_GainEffectView.VIEW_NAME, typeof(UI_Win_GainEffectView), s_popWin, EnumMaskStatus.kTouchClose,s_PlayerRes);
    //快捷使用货币道具
    public static UIInfo s_quickUseRss = new UIInfo(QuickUseResourcesView.VIEW_NAME,typeof(QuickUseResourcesView),s_popWin,EnumMaskStatus.kTouchClose, s_PlayerRes);
    //加速界面
    public static UIInfo s_speedUp = new UIInfo(AddSpeedView.VIEW_NAME,typeof(AddSpeedView),s_popWinPop,EnumMaskStatus.kTouchClose, s_PlayerRes, 1035);

    //搜索
    public static UIInfo s_worldPosSearch = new UIInfo(WorldPosSearchView.VIEW_NAME,typeof(WorldPosSearchView),s_popWinPop,EnumMaskStatus.kTouchCloseAlpha);
    
    //统帅
    public static UIInfo s_captainSummon = new UIInfo(CaptainSummonView.VIEW_NAME, typeof(CaptainSummonView), s_popWinPop, EnumMaskStatus.kNone,null, 3001);
    public static UIInfo s_captain = new UIInfo(CaptainView.VIEW_NAME, typeof(CaptainView), s_popWin, EnumMaskStatus.kTouchClose,null, 3000);
    public static UIInfo s_captainLevelUp = new UIInfo(UI_Win_CaptainLevelUpView.VIEW_NAME, typeof(UI_Win_CaptainLevelUpView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_captainItemSource = new UIInfo(UI_Win_CaptainItemSourceView.VIEW_NAME, typeof(UI_Win_CaptainItemSourceView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_captainSkillUpSuccess = new UIInfo(UI_IF_CaptainSkillUpSuccessView.VIEW_NAME, typeof(UI_IF_CaptainSkillUpSuccessView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_captainStarUpSuccess = new UIInfo(UI_IF_CaptainStarUpSuccessView.VIEW_NAME, typeof(UI_IF_CaptainStarUpSuccessView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_talentChangeAlert = new UIInfo(UI_Win_TalentChangeAlertView.VIEW_NAME, typeof(UI_Win_TalentChangeAlertView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_talentChangeNameAlert = new UIInfo(UI_Win_TalentChangeNameView.VIEW_NAME, typeof(UI_Win_TalentChangeNameView), s_popWin, EnumMaskStatus.kTouchClose);
    
    //道具
    public static UIInfo s_itemExchange = new UIInfo(UI_Win_ItemExchangeView.VIEW_NAME, typeof(UI_Win_ItemExchangeView), s_popWin, EnumMaskStatus.kTouchClose, null, 3002);
    public static UIInfo s_useItem = new UIInfo(UI_Win_UseItemView.VIEW_NAME, typeof(UI_Win_UseItemView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_exchageActionPoint = new UIInfo(UI_Win_ExChangeActionPointView.VIEW_NAME, typeof(UI_Win_ExChangeActionPointView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_quickUseItem = new UIInfo(UI_Win_QuickUseItemView.VIEW_NAME, typeof(UI_Win_QuickUseItemView), s_popWin, EnumMaskStatus.kNone, null, 0, 0, false);

    public static  UIInfo s_createMainTroops=new UIInfo(UI_Pop_ArmySelectView.VIEW_NAME,typeof(UI_Pop_ArmySelectView),s_popWin,EnumMaskStatus.kTouchCloseAlpha, null, 4004);
    public static  UIInfo s_createAnmy=new UIInfo(UI_Win_CreateArmyView.VIEW_NAME,typeof(UI_Win_CreateArmyView),s_popWin,EnumMaskStatus.kTouchClose, null, 2001);
	
	public static UIInfo s_trainArmy = new UIInfo(TrainArmyView.VIEW_NAME, typeof(TrainArmyView), s_popWin, EnumMaskStatus.kTouchClose, s_PlayerRes, 1008);
	public static UIInfo s_trainDissolve = new UIInfo(TrainDissolveView.VIEW_NAME, typeof(TrainDissolveView), s_popWinPop, EnumMaskStatus.kTouchClose);
	
	public static  UIInfo s_iF_SearchRes=new UIInfo(UI_IF_SearchResView.VIEW_NAME,typeof(UI_IF_SearchResView),s_popWin,EnumMaskStatus.kTouchCloseAlpha, null, 4001);
		
	public static  UIInfo s_Pop_WorldInfo=new UIInfo(UI_Pop_WorldObjectInfoView.VIEW_NAME,typeof(UI_Pop_WorldObjectInfoView),s_popWinPop,EnumMaskStatus.kTouchCloseAlpha);
	public static  UIInfo s_Pop_WorldObjectVillageCave=new UIInfo(UI_Pop_WorldObjectVillageCaveView.VIEW_NAME,typeof(UI_Pop_WorldObjectVillageCaveView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);
	public static  UIInfo s_Pop_WorldObjectVillageCaveFinish=new UIInfo(UI_Pop_WorldObjectVillageCaveFinishView.VIEW_NAME,typeof(UI_Pop_WorldObjectVillageCaveFinishView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);

	//玩家信息	
	public static UIInfo s_PlayerInfo = new UIInfo(PlayerDataView.VIEW_NAME,typeof(PlayerDataView),s_popWin,EnumMaskStatus.kTouchClose,null,12000);
	//角色管理
	public static UIInfo s_PlayerManage=  new UIInfo(UI_Win_PlayerManageView.VIEW_NAME,typeof(UI_Win_PlayerManageView),s_popWin);
	public static UIInfo s_PlayerNewChar= new UIInfo(UI_Win_PlayerNewCharView.VIEW_NAME,typeof(UI_Win_PlayerNewCharView),s_popWin);
	public static UIInfo s_PlayerChangeSure= new UIInfo(UI_Win_PlayerChangeSureView.VIEW_NAME,typeof(UI_Win_PlayerChangeSureView),s_popWin,EnumMaskStatus.kTouchClose);
	public static UIInfo s_WinServer= new UIInfo(UI_Win_ServerView.VIEW_NAME,typeof(UI_Win_ServerView),s_popWin,EnumMaskStatus.kTouchClose);
	
    //其他玩家信息	
    public static UIInfo s_OtherPlayerInfo = new UIInfo(UI_Win_OtherPlayerDataView.VIEW_NAME,typeof(UI_Win_OtherPlayerDataView),s_popWin,EnumMaskStatus.kTouchClose);
	//设置界面
	public static UIInfo s_Setting = new UIInfo(PlayerSettingView.VIEW_NAME,typeof(PlayerSettingView),s_popWin,EnumMaskStatus.kTouchClose);
	public static UIInfo s_PlayerChangeName = new UIInfo(ChangeNameView.VIEW_NAME,typeof(ChangeNameView),s_popWin,EnumMaskStatus.kTouchClose);
    public static UIInfo s_community = new UIInfo(UI_Win_CommunityView.VIEW_NAME, typeof(UI_Win_CommunityView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_PlayerChangeCivilization = new UIInfo(CreateCharView.VIEW_NAME, typeof(CreateCharView), s_popWin, EnumMaskStatus.kNone);
    public static UIInfo s_ChangeCivilization = new UIInfo(UI_Win_CivilizationChangeView.VIEW_NAME, typeof(UI_Win_CivilizationChangeView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_NoticeSetting = new UIInfo(UI_Win_SettingNoticeView.VIEW_NAME, typeof(UI_Win_SettingNoticeView), s_popWin, EnumMaskStatus.kTouchClose);
 
    //VIP信息页面
    public static UIInfo s_Vip = new UIInfo(UI_Win_VipView.VIEW_NAME,typeof(UI_Win_VipView),s_popWin,EnumMaskStatus.kTouchClose,null, 11001);
	public static UIInfo s_VipPointDayReward = new UIInfo(UI_IF_VipPointDayRewardView.VIEW_NAME,typeof(UI_IF_VipPointDayRewardView),s_popWin,EnumMaskStatus.kTouchClose);
	//VIP商店
	public static UIInfo s_VipStore = new UIInfo(UI_Win_VipStoreView.VIEW_NAME,typeof(UI_Win_VipStoreView),s_popWin,EnumMaskStatus.kTouchClose);
	
	//装备系统
	public static UIInfo s_Material =  new UIInfo(UI_Win_MaterialView.VIEW_NAME,typeof(UI_Win_MaterialView),s_popWin,EnumMaskStatus.kTouchClose);
	public static UIInfo s_Equip = new UIInfo(UI_Win_EquipView.VIEW_NAME,typeof(UI_Win_EquipView),s_popWin,EnumMaskStatus.kOnlyShow);
	public static UIInfo s_EquipQuick = new UIInfo(UI_Win_EquipQuickView.VIEW_NAME,typeof(UI_Win_EquipQuickView),s_popWin,EnumMaskStatus.kOnlyShow);
	public static UIInfo s_EquipTalentChoose = new UIInfo(UI_IF_EquipTalentChooseView.VIEW_NAME,typeof(UI_IF_EquipTalentChooseView),s_popWin,EnumMaskStatus.kOnlyShow);
	public static UIInfo s_EquipSuccess = new UIInfo(UI_IF_EquipSuccessView.VIEW_NAME,typeof(UI_IF_EquipSuccessView),s_popWin,EnumMaskStatus.kTouchClose);

    //邮件
    public static UIInfo s_Email = new UIInfo(MailView.VIEW_NAME,typeof(MailView),s_popWin,EnumMaskStatus.kTouchClose,null, 9000);
    public static UIInfo s_EmailEnclosure = new UIInfo(EmailEnclosureView.VIEW_NAME, typeof(EmailEnclosureView), s_popWinPop, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_writeAMail = new UIInfo(UI_Win_WriteAMailView.VIEW_NAME,typeof(UI_Win_WriteAMailView),s_popWin,EnumMaskStatus.kOnlyShow);
    public static UIInfo s_mailContactList = new UIInfo(UI_Win_MailContactView.VIEW_NAME,typeof(UI_Win_MailContactView),s_popWin,EnumMaskStatus.kTouchClose);
    public static UIInfo s_replyEmail = new UIInfo(UI_Win_ReplyAMailView.VIEW_NAME, typeof(UI_Win_ReplyAMailView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_emailBattleLog = new UIInfo(UI_Win_BattleLogView.VIEW_NAME, typeof(UI_Win_BattleLogView), s_popWinPop, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_emailBubbles = new UIInfo(UI_Pop_MailGetNewView.VIEW_NAME, typeof(UI_Pop_MailGetNewView), s_popWinPop, EnumMaskStatus.kNone);

    //战报:部队详情
    public static UIInfo s_ArmyDetail = new UIInfo(ArmyDetailsView.VIEW_NAME, typeof(ArmyDetailsView), s_popWin, EnumMaskStatus.kTouchClose);
    //战报：部队增益
    public static UIInfo s_ArmyBuff = new UIInfo(ArmyBuffView.VIEW_NAME, typeof(ArmyBuffView), s_popWin, EnumMaskStatus.kTouchClose);

    //资源短缺
    public static UIInfo s_ResShort = new UIInfo(ResShortView.VIEW_NAME,typeof(ResShortView),s_popWinPop,EnumMaskStatus.kTouchClose, s_PlayerRes);
    public static UIInfo s_resShortSpecial = new UIInfo(UI_Win_ResShortSpecialView.VIEW_NAME,typeof(UI_Win_ResShortSpecialView),s_popWinPop,EnumMaskStatus.kTouchClose, s_PlayerRes);
    //补足资源
    public static UIInfo s_AddRes = new UIInfo(AddResView.VIEW_NAME,typeof(AddResView),s_popWinPop,EnumMaskStatus.kTouchClose,s_PlayerRes);
    //代币不足
    public static UIInfo s_GemShort = new UIInfo(GemShortView.VIEW_NAME,typeof(GemShortView),s_popWinPop,EnumMaskStatus.kTouchClose);

    //提示界面
    public static UIInfo s_helpTip = new UIInfo(HelpTipView.VIEW_NAME,typeof(HelpTipView),s_popWinPop,EnumMaskStatus.kNone);
    public static UIInfo s_mailWarTips = new UIInfo(UI_Pop_MailWarTipsView.VIEW_NAME,typeof(UI_Pop_MailWarTipsView),s_popWinPop,EnumMaskStatus.kTouchCloseAlpha);

    //对话界面
    public static UIInfo s_talkTip = new UIInfo(TalkTipView.VIEW_NAME, typeof(TalkTipView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);

    //剧情对白
    public static UIInfo s_NPCDialog = new UIInfo(NPCDialogView.VIEW_NAME,typeof(NPCDialogView),s_popWinPop,EnumMaskStatus.kNone);
    public static UIInfo s_ChapterDialog = new UIInfo(ChapterDialogView.VIEW_NAME,typeof(ChapterDialogView),s_popWin,EnumMaskStatus.kNone);

    //战力
    public static UIInfo s_PlayerPower= new UIInfo(PlayerPowerViewView.VIEW_NAME,typeof(PlayerPowerViewView),s_popWin,EnumMaskStatus.kTouchClose);

    public static UIInfo s_buildingDesc = new UIInfo(BuildingDescView.VIEW_NAME, typeof(BuildingDescView), s_popWin, EnumMaskStatus.kTouchClose,s_PlayerRes);


    public static UIInfo s_Pop_LanguageSet = new UIInfo(LanguageSetView.VIEW_NAME, typeof(LanguageSetView), s_popWinPop, EnumMaskStatus.kOnlyShow);

    public static UIInfo s_hospitalInfo = new UIInfo(HospitalView.VIEW_NAME, typeof(HospitalView), s_popWin, EnumMaskStatus.kTouchClose,s_PlayerRes, 1025);
	public static  UIInfo s_WinArmyShow=new UIInfo(UI_Win_ArmyShowView.VIEW_NAME,typeof(UI_Win_ArmyShowView), s_popWin, EnumMaskStatus.kTouchClose);
	public static  UIInfo s_WinArmyConst=new UIInfo(UI_Win_ArmyConstView.VIEW_NAME,typeof(UI_Win_ArmyConstView), s_popWin, EnumMaskStatus.kTouchClose);
	public static  UIInfo s_pop_WorldMonster=new UIInfo(UI_Pop_WorldObjectMonsterView.VIEW_NAME,typeof(UI_Pop_WorldObjectMonsterView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha, null, 4005);
    public static UIInfo s_pop_WorldObjectPlayer = new UIInfo(UI_Pop_WorldObjectPlayerView.VIEW_NAME, typeof(UI_Pop_WorldObjectPlayerView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_pop_WorldTunes = new UIInfo(UI_Pop_WorldObjectInfoTypeRunesView.VIEW_NAME, typeof(UI_Pop_WorldObjectInfoTypeRunesView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha, null);

    public static UIInfo s_guideInfo = new UIInfo(GuideView.VIEW_NAME, typeof(GuideView), s_guide, EnumMaskStatus.kNone);
    public static UIInfo s_fingerInfo = new UIInfo(FingerView.VIEW_NAME, typeof(FingerView), s_popWinPop, EnumMaskStatus.kNone);
    public static UIInfo s_funcGuide = new UIInfo(FuncGuideView.VIEW_NAME, typeof(FuncGuideView), s_guide, EnumMaskStatus.kNone, null, 99001, 0,false);

    //设置界面
    public static UIInfo s_generalSetting = new UIInfo(GeneralSettingView.VIEW_NAME,typeof(GeneralSettingView),s_popWin,EnumMaskStatus.kTouchClose);

    #region 伺候相关界面
    public static UIInfo s_scoutWin = new UIInfo(UI_Win_ScoutView.VIEW_NAME, typeof(UI_Win_ScoutView), s_popWin, EnumMaskStatus.kTouchClose, s_PlayerRes, 1022);
    public static UIInfo s_scoutSelectMenu = new UIInfo(UI_Pop_ScoutSelectView.VIEW_NAME, typeof(UI_Pop_ScoutSelectView), s_popWin, EnumMaskStatus.kTouchCloseAlpha, null, 4003);
    public static UIInfo s_scoutSearchMenuu = new UIInfo(UI_Pop_ExploreMistView.VIEW_NAME, typeof(UI_Pop_ExploreMistView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha, null, 4002);
    #endregion
    //警戒塔建筑
    public static UIInfo s_theTower = new UIInfo(UI_Win_TheTowerView.VIEW_NAME, typeof(UI_Win_TheTowerView), s_popWin, EnumMaskStatus.kOnlyShow);
    //城墙建筑
    public static UIInfo s_theWall = new UIInfo(UI_Win_TheWallView.VIEW_NAME, typeof(UI_Win_TheWallView), s_popWin, EnumMaskStatus.kOnlyShow);

    //地图书签
    public static UIInfo s_MapMarker = new UIInfo(UI_Win_BookView.VIEW_NAME, typeof(UI_Win_BookView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_MapMarkerOperation = new UIInfo(UI_Win_BookAddView.VIEW_NAME, typeof(UI_Win_BookAddView), s_popWinPop, EnumMaskStatus.kOnlyShow);

    #region 联盟相关


    public static int ALLIANCE_GRPOP = 10;

    public static UIInfo s_AllianceWelcome = new UIInfo(UI_Win_GuildWelcomeView.VIEW_NAME, typeof(UI_Win_GuildWelcomeView), s_popWin, EnumMaskStatus.kOnlyShow,null, 5001);
    
    public static UIInfo s_AllianceCreateWin = new UIInfo(UI_Win_GuildSettingView.VIEW_NAME, typeof(UI_Win_GuildSettingView), s_popWin, EnumMaskStatus.kOnlyShow);
    
    public static UIInfo s_AllianceFlag = new UIInfo(UI_Win_GuildFlagSettingView.VIEW_NAME, typeof(UI_Win_GuildFlagSettingView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    public static UIInfo s_AllianceWelcomeEdit = new UIInfo(UI_Win_GuildWelcomeEditView.VIEW_NAME, typeof(UI_Win_GuildWelcomeEditView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    public static UIInfo s_AllianceNameEdit = new UIInfo(UI_Win_GuildChangeNameView.VIEW_NAME, typeof(UI_Win_GuildChangeNameView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    //加入请求
    public static UIInfo s_AllianceJionReqList = new UIInfo(UI_Win_GuildApplicationView.VIEW_NAME, typeof(UI_Win_GuildApplicationView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    //加入搜索界面
    public static UIInfo s_AllianceJionList = new UIInfo(UI_Win_GuildJoinView.VIEW_NAME, typeof(UI_Win_GuildJoinView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    //联盟信息查看
    public static UIInfo s_AllianceInfo = new UIInfo(UI_Win_GuildViewView.VIEW_NAME, typeof(UI_Win_GuildViewView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
	
    //邀请他人加入
    public static UIInfo s_AllianceInvite = new UIInfo(UI_Win_GuildInviteView.VIEW_NAME, typeof(UI_Win_GuildInviteView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    //邀请他人加入 小窗
    public static UIInfo s_AllianceInviteTip = new UIInfo(UI_Win_GuildInviteMsgView.VIEW_NAME, typeof(UI_Win_GuildInviteMsgView), s_popWinPop, EnumMaskStatus.KNoMaskTouchScale,null,0,ALLIANCE_GRPOP, false);
	//语言选择框
    public static UIInfo s_AllianceLan = new UIInfo(UI_Win_GuildLanguageView.VIEW_NAME, typeof(UI_Win_GuildLanguageView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    //主页
    public static UIInfo s_AllianceMain = new UIInfo(UI_Win_GuildMainView.VIEW_NAME, typeof(UI_Win_GuildMainView), s_popWin, EnumMaskStatus.kOnlyShow,null,5000,ALLIANCE_GRPOP);
    //权限
    public static UIInfo s_AllianceMainAccess = new UIInfo(UI_Win_GuildPurviewView.VIEW_NAME, typeof(UI_Win_GuildPurviewView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    //成员升级
    public static UIInfo s_AllianceMemUpgrade = new UIInfo(UI_Win_GuildMemUpgradeView.VIEW_NAME, typeof(UI_Win_GuildMemUpgradeView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    //成员移除
    public static UIInfo s_AllianceMemRemove = new UIInfo(UI_Win_GuildMemRemoveView.VIEW_NAME, typeof(UI_Win_GuildMemRemoveView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    //管员任命
    public static UIInfo s_AllianceOffice = new UIInfo(UI_Win_GuildAppointView.VIEW_NAME, typeof(UI_Win_GuildAppointView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    public static UIInfo s_AllianceExitConfirm = new UIInfo(UI_Win_GuildDissolutionView.VIEW_NAME, typeof(UI_Win_GuildDissolutionView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    //联盟拓展功能界面
    //联盟帮助
    public static UIInfo s_AllianceHelp = new UIInfo(UI_Win_GuildHelpView.VIEW_NAME, typeof(UI_Win_GuildHelpView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    public static UIInfo s_AllianceDepot = new UIInfo(UI_Win_GuildDepotView.VIEW_NAME, typeof(UI_Win_GuildDepotView), s_popWinPop, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    
    //联盟领土
    
    public static UIInfo s_AllianceTerrtroy = new UIInfo(UI_Win_GuildTerrtroyView.VIEW_NAME, typeof(UI_Win_GuildTerrtroyView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    public static UIInfo s_AllianceHoly = new UIInfo(UI_Win_GuildHolyView.VIEW_NAME, typeof(UI_Win_GuildHolyView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    //创建联盟资源栏信息
    public static UIInfo s_AllianceCreateBuildRes = new UIInfo(UI_Pop_GuildBuildResView.VIEW_NAME, typeof(UI_Pop_GuildBuildResView), s_popWin, EnumMaskStatus.kNone,null,0,ALLIANCE_GRPOP);
    //创建联盟建筑信息
    public static UIInfo s_AllianceCreateBuildPopup = new UIInfo(UI_Pop_GuildBuildView.VIEW_NAME, typeof(UI_Pop_GuildBuildView), s_popWin, EnumMaskStatus.kNone,null,0,ALLIANCE_GRPOP);
    
    //联盟建筑 采集增援
    public static UIInfo s_AllianceBuild= new UIInfo(UI_Win_GuildBuildView.VIEW_NAME, typeof(UI_Win_GuildBuildView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);

    //联盟建筑信息
    public static UIInfo s_AllianceBuildInfoTip = new UIInfo(UI_Pop_GuildMapBuildTipView.VIEW_NAME, typeof(UI_Pop_GuildMapBuildTipView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha,null,0);
	//联盟建筑信息详细
    public static UIInfo s_AllianceBuildInfoDes = new UIInfo(UI_Win_GuildBuildDescView.VIEW_NAME, typeof(UI_Win_GuildBuildDescView), s_popWin, EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);

    
    //研究主界面
    public static UIInfo s_AllianceResearchMain = new UIInfo(UI_Win_GuildResearchView.VIEW_NAME,typeof(UI_Win_GuildResearchView),s_popWin,EnumMaskStatus.kTouchClose,s_PlayerRes,0,ALLIANCE_GRPOP);
    //研究升级界面
    public static UIInfo s_AllianceResearchUpdate = new UIInfo(UI_Win_GuildResearchUpateView.VIEW_NAME,typeof(UI_Win_GuildResearchUpateView),s_popWinPop,EnumMaskStatus.kTouchClose,s_PlayerRes,0,ALLIANCE_GRPOP);
	
    //联盟礼物
    public static UIInfo s_AllianceGift = new UIInfo(UI_Win_GuildGiftView.VIEW_NAME,typeof(UI_Win_GuildGiftView),s_popWin,EnumMaskStatus.kTouchClose,null,0,ALLIANCE_GRPOP);
	//联盟商店
    public static UIInfo s_AllianceStore = new UIInfo(UI_Win_GuildStoreView.VIEW_NAME,typeof(UI_Win_GuildStoreView),s_popWin,EnumMaskStatus.kTouchClose,null,0,ALLIANCE_GRPOP);

    //联盟留言板
    public static UIInfo s_AllianceMsg= new UIInfo(UI_GuildMessageBoardView.VIEW_NAME,typeof(UI_GuildMessageBoardView),s_popWin,EnumMaskStatus.kTouchClose,null,0,ALLIANCE_GRPOP);
    
    //联盟排行榜
    public static UIInfo s_AllianceRanking= new UIInfo(UI_Win_GuildRankingView.VIEW_NAME,typeof(UI_Win_GuildRankingView),s_popWin,EnumMaskStatus.kTouchClose,null,0,ALLIANCE_GRPOP);
    
    public static UIInfo s_AllianceMember= new UIInfo(UI_Win_GuildMemberView.VIEW_NAME,typeof(UI_Win_GuildMemberView),s_popWin,EnumMaskStatus.kTouchClose,null,0,ALLIANCE_GRPOP);

    public static UIInfo s_AllianceGuideHelp= new UIInfo(UI_Win_GuideAnimView.VIEW_NAME,typeof(UI_Win_GuideAnimView), s_popWinPop, EnumMaskStatus.kTouchClose,null,0,ALLIANCE_GRPOP);

    public static UIInfo s_AlliancePopGuideAnim = new UIInfo(UI_Pop_GuideAnimView.VIEW_NAME, typeof(UI_Pop_GuideAnimView), s_popWinPop, EnumMaskStatus.kNoMaskNoTouch, null, 0, ALLIANCE_GRPOP);

    // 奇观建筑信息界面
    public static UIInfo s_WorldObjectInfoBuild = new UIInfo(UI_Pop_WorldObjectInfoBuildView.VIEW_NAME,typeof(UI_Pop_WorldObjectInfoBuildView),s_popWinPop,EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_WorldObjectInfoBuildInfo = new UIInfo(UI_Win_WorldObjectInfoBuildInfoView.VIEW_NAME,typeof(UI_Win_WorldObjectInfoBuildInfoView),s_popWinPop,EnumMaskStatus.kOnlyShow,null,0,ALLIANCE_GRPOP);
    
    
    #endregion

    public static UIInfo s_store = new UIInfo(StoreView.VIEW_NAME, typeof(StoreView), s_popWin, EnumMaskStatus.kTouchClose, null);

    #region 酒馆
    public static UIInfo s_tavernSummon = new UIInfo(TavernSummonView.VIEW_NAME, typeof(TavernSummonView), s_popWin, EnumMaskStatus.kOnlyShow,null,1026);
    public static UIInfo s_tavernReward = new UIInfo(TavernRewardView.VIEW_NAME, typeof(TavernRewardView), s_popWinPop, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_tavernBoxDesc = new UIInfo(TavernBoxDescView.VIEW_NAME, typeof(TavernBoxDescView), s_popWin, EnumMaskStatus.kOnlyShow);
    #endregion

    public static UIInfo s_chat = new UIInfo(UI_Win_ChatView.VIEW_NAME,typeof(UI_Win_ChatView),s_chatview,EnumMaskStatus.kNone);

    public static UIInfo s_playerHeadPic = new UIInfo(UI_Win_PlayerHeadPicView.VIEW_NAME,typeof(UI_Win_PlayerHeadPicView),s_popWin,EnumMaskStatus.kTouchClose);

    #region 活动
    public static UIInfo s_eventDate = new UIInfo(EventDateView.VIEW_NAME, typeof(EventDateView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_eventTypeRankReward = new UIInfo(EventTypeRankRewardView.VIEW_NAME, typeof(EventTypeRankRewardView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_strongerPlayerRank = new UIInfo(StrongerPlayerRankView.VIEW_NAME, typeof(StrongerPlayerRankView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_newRoleActivity = new UIInfo(NewRoleActivityView.VIEW_NAME, typeof(NewRoleActivityView), s_popWin, EnumMaskStatus.kOnlyShow);

    public static UIInfo s_rewardGetWin = new UIInfo(RewardGetWinView.VIEW_NAME, typeof(RewardGetWinView), s_popWin, EnumMaskStatus.kOnlyShow);

    public static UIInfo s_eventTurntableRewards = new UIInfo(UI_Pop_EventTurntableRewardsView.VIEW_NAME, typeof(UI_Pop_EventTurntableRewardsView), s_popWin, EnumMaskStatus.kTouchCloseAlpha);

    #endregion
    //城市管理
    public static UIInfo s_cityManager = new UIInfo(UI_Win_CityManagerView.VIEW_NAME, typeof(UI_Win_CityManagerView), s_popWin, EnumMaskStatus.kOnlyShow,null,1034);
    public static UIInfo s_buffList = new UIInfo(UI_Pop_BuffListView.VIEW_NAME, typeof(UI_Pop_BuffListView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);

    //城市迁移
    public static UIInfo s_moveCity = new UIInfo(UI_Pop_MoveCityView.VIEW_NAME, typeof(UI_Pop_MoveCityView), s_popWin, EnumMaskStatus.kNone);
	
	public static UIInfo s_FightVictory= new UIInfo(UI_Pop_FightVictoryView.VIEW_NAME,typeof(UI_Pop_FightVictoryView), s_popWin, EnumMaskStatus.kTouchCloseAlpha);

   
    //神秘商店相关
    public static UIInfo s_mysteryStoreGone = new UIInfo(UI_Win_MysteryStoreGoneView.VIEW_NAME,typeof(UI_Win_MysteryStoreGoneView),s_popWinPop,EnumMaskStatus.kTouchClose);
    public static UIInfo s_mysteryStore = new UIInfo(UI_Win_MysteryStoreView.VIEW_NAME,typeof(UI_Win_MysteryStoreView),s_popWin,EnumMaskStatus.kTouchClose,s_PlayerRes);

    //资源运输
    public static UIInfo s_assitRes = new UIInfo(UI_Win_AssitResView.VIEW_NAME,typeof(UI_Win_AssitResView),s_popWin,EnumMaskStatus.kTouchClose,s_PlayerRes);

    //帮助与联系客服
    public static UIInfo s_helper = new UIInfo(UI_Win_HelperView.VIEW_NAME,typeof(UI_Win_HelperView),s_popWin,EnumMaskStatus.kTouchClose);
    //联盟战争
    public static UIInfo s_AlianceWar = new UIInfo(UI_Win_WarView.VIEW_NAME, typeof(UI_Win_WarView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_warDetial = new UIInfo(UI_Win_WarDetialView.VIEW_NAME, typeof(UI_Win_WarDetialView), s_popWin, EnumMaskStatus.kNone);
    //联盟增援
    public static UIInfo s_AlianceReinforce = new UIInfo(UI_Win_WarDetialView.VIEW_NAME, typeof(UI_Win_WarDetialView), s_popWin, EnumMaskStatus.kOnlyShow);


    public static UIInfo s_common_Aggregation= new UIInfo(UI_Common_AggregationView.VIEW_NAME,typeof(UI_Common_AggregationView), s_popWin, EnumMaskStatus.kNone);

    //跑马灯公告
    public static UIInfo s_scrollMessage = new UIInfo(UI_Common_ScrollMessageView.VIEW_NAME,typeof(UI_Common_ScrollMessageView),s_popWinPop,EnumMaskStatus.kNone,null,0,0,false);

    public static UIInfo s_warWarning = new UIInfo(UI_Win_WarnView.VIEW_NAME, typeof(UI_Win_WarnView), s_popWin, EnumMaskStatus.kTouchClose);
    //城市增援
    public static UIInfo s_reinforcements = new UIInfo(UI_Win_ReinforcementsView.VIEW_NAME, typeof(UI_Win_ReinforcementsView), s_popWinPop, EnumMaskStatus.kOnlyShow);

    //战役主界面
    public static UIInfo s_battleMain = new UIInfo(UI_IF_BattleMainView.VIEW_NAME, typeof(UI_IF_BattleMainView), s_popWin, EnumMaskStatus.kNone,null, 6001);

    //远征
    public static UIInfo s_expeditionStore = new UIInfo(UI_Win_ExpeditionStoreView.VIEW_NAME, typeof(UI_Win_ExpeditionStoreView), s_popWin, EnumMaskStatus.kTouchClose);
    public static UIInfo s_expeditionFight = new UIInfo(UI_IF_ExpeditionFightView.VIEW_NAME, typeof(UI_IF_ExpeditionFightView), s_popWin, EnumMaskStatus.kOnlyShow, null,6000);
    public static UIInfo s_expeditionRule = new UIInfo(UI_Win_ExpeditionRuleView.VIEW_NAME, typeof(UI_Win_ExpeditionRuleView), s_popWinPop, EnumMaskStatus.kTouchClose);
    public static UIInfo s_expeditionFightTask = new UIInfo(UI_IF_ExpeditionFightTaskView.VIEW_NAME, typeof(UI_IF_ExpeditionFightTaskView), s_popWin, EnumMaskStatus.kNone,null,
        6002,0,false);
    public static UIInfo s_expeditionFightWar = new UIInfo(UI_IF_ExpeditionFightWarView.VIEW_NAME, typeof(UI_IF_ExpeditionFightWarView), s_popWin, EnumMaskStatus.kNone, null,
        0, 0, false);
    public static UIInfo s_expeditionFightWin = new UIInfo(UI_IF_ExpeditionFightWinView.VIEW_NAME, typeof(UI_IF_ExpeditionFightWinView), s_popWinPop, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_expeditionFightFail = new UIInfo(UI_IF_ExpeditionFightFailView.VIEW_NAME, typeof(UI_IF_ExpeditionFightFailView), s_popWinPop, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_battleTroopsTips = new UIInfo(UI_Pop_BattleTroopsTipsView.VIEW_NAME, typeof(UI_Pop_BattleTroopsTipsView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_finger2 = new UIInfo(UI_Win_Finger2View.VIEW_NAME, typeof(UI_Win_Finger2View), s_popWin);
    public static UIInfo s_expeditionFightReady = new UIInfo(UI_IF_ExpeditionFightReadyView.VIEW_NAME, typeof(UI_IF_ExpeditionFightReadyView), s_popWinPop, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_choseChat = new UIInfo(UI_Win_ChoseChatView.VIEW_NAME, typeof(UI_Win_ChoseChatView), s_popWinPop, EnumMaskStatus.kOnlyShow);
    
    #endregion

    #region 充值
    public static UIInfo s_Charge = new UIInfo(UI_IF_ChargeView.VIEW_NAME , typeof(UI_IF_ChargeView),s_popWinPop,EnumMaskStatus.kOnlyShow);
    public static UIInfo s_GiftLimit = new UIInfo(UI_Win_GiftLimitedTimeView.VIEW_NAME, typeof(UI_Win_GiftLimitedTimeView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_ChargePop = new UIInfo(UI_Win_ChargePopView.VIEW_NAME , typeof(UI_Win_ChargePopView),s_popWin,EnumMaskStatus.kTouchClose);


    #endregion
    #region 用户协议
    public static UIInfo s_Agreement = new UIInfo(UI_Win_AgreementView.VIEW_NAME, typeof(UI_Win_AgreementView), s_popWin, EnumMaskStatus.kOnlyShow);
    #endregion
    #region 账号相关
    public static UIInfo s_AccountSwitch = new UIInfo(UI_Win_AccountChangeView.VIEW_NAME, typeof(UI_Win_AccountChangeView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_AccountCenter = new UIInfo(UI_Win_AccountMgrView.VIEW_NAME, typeof(UI_Win_AccountMgrView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_AccountLoginMain = new UIInfo(UI_Win_LoginMainView.VIEW_NAME, typeof(UI_Win_LoginMainView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_AccountLoginOther = new UIInfo(UI_Win_LoginTypeView.VIEW_NAME, typeof(UI_Win_LoginTypeView), s_popWin, EnumMaskStatus.kOnlyShow);
    #endregion
    #region 内置评星相关
    public static UIInfo s_EvaluateStar = new UIInfo(UI_IF_EvaluateStarView.VIEW_NAME, typeof(UI_IF_EvaluateStarView), s_popWin, EnumMaskStatus.kOnlyShow);
    #endregion
    #region 维护公告
    public static UIInfo s_Maintain = new UIInfo(UI_Win_MainTainView.VIEW_NAME, typeof(UI_Win_MainTainView), s_popWin, EnumMaskStatus.kNoMaskNoTouch);
    #endregion

    #region 内嵌网页
    public static UIInfo s_WebView = new UIInfo("UI_Win_WebView", typeof(UI_Win_WebViewView), s_browserViewInfo, EnumMaskStatus.kOnlyShow);
    #endregion
    //-----------------------------------------Logic Info End Here--------------------------------------------------------------------
}
