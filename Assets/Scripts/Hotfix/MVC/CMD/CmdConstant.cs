
namespace Game
{
    public class CmdConstant
    {
        public const string LoginInitFinish = "LoginInitFinish";

        #region HUD模块
        public const string ShowBuildingHudMenu = "ShowBuildingHudMenu";
        public const string OnCloseBuildingHudMenu = "OnCloseBuildingHudMenu";
        public const string CloseBuildingHudMenu = "CloseBuildingHudMenu";
        public const string buildQueueChange = "buildQueueChange";//建造队列消息有改变
        public const string armyQueueChange = "armyQueueChange";//训练队列消息有改变
        public const string technologyQueueChange = "technologyQueueChange";//研究队列消息有改变
        public const string technologyChange = "technologyChange";//研究队列消息有改变
        public const string technologyChange2 = "technologyChange2";//研究队列消息有改变,去除第一次
        public const string treatmentChange = "treatmentChange";//治疗队列消息有改变
        public const string UpdateBuildingBarHud = "UpdateBuildingBarHud";//更新建筑进度条

		public const string InSoldierInfoChange = "InSoldierInfoChange";//城内士兵信息变更
		public const string ArmyBuildingTrainStatus = "ArmyBuildingTrainStatus";//士兵建筑训练状态通知
		public const string HospitalTreatmentStatus = "HospitalTreatmentStatus";//医院治疗状态
		public const string HospitalCanGetReward = "HospitalCanGetReward";//医院治疗完成
        public const string SeriousInjuredChange = "SeriousInjuredChange";  //伤兵信息变更 
        public const string denseFogOpenFlag = "denseFogOpenFlag";  //迷雾信息有改变
        
        public const string CreateTempBuild = "CreateTempBuild"; //创建零时建筑
        
        public const string CreateTempBuildYes = "CreateTempBuildYes"; //创建零时建筑
        public const string CreateTempBuildNO = "CreateTempBuildNO"; //创建零时建筑
        public const string BuildSelected = "BuildSelected"; //选中
        public const string BuildSelectReset = "BuildSelectReset"; //清除选中

        public const string CreateTempBuildEnable = "CreateTempBuildEnable";
        public const string RemoveBuild = "RemoveBuild"; //移除建筑
        public const string RemoveBuildHud = "RemoveBuildHud"; //移除建造进度条
        public const string AwardArmy = "AwardArmy";//训练后领取士兵
        public const string AwardTreatment = "AwardTreatment";//收取治疗士兵
        public const string GetBuildResources = "GetBuildResources";//收取建筑资源
        public const string AwardTechnology = "GetBuildResources";//领取科技
        #endregion

        #region 主城UI
        //隐藏主界面UI
        public const string HideMainCityUI = "HideMainCityUI";
        //显示主界面UI
        public const string ShowMainCityUI = "ShowMainCityUI";

        public const string ReturnToFullView = "ReturnToFullView";

        //新手引导时主城显示的UI
        public const string OnGuideMainInterfaceModule = "OnGuideMainInterfaceModule";

        //资源菜单显示变化
        public const string OnLodMenuChange = "OnLodMenuChange";

        public const string OnCityLoadFinished = "OnCityLoadFinished";
        #endregion

        #region 玩家信息
        //更新玩家战力
        public const string UpdatePlayerPower = "UpdatePlayerPower";
        //更新玩家战力
        public const string UpdatePlayerHistoryPower = "UpdatePlayerHistoryPower";
        //强制更新玩家战力
        public const string ForceUpdatePlayerPower = "ForceUpdatePlayerPower";
        //更新行动力
        public const string UpdatePlayerActionPower = "UpdatePlayerActionPower";
        //更新角色等级
        public const string UpdateMainRoleLevel = "UpdateMainRoleLevel";
        //显示角色等级和资源
        public const string ShowPlayerResInfo = "ShowPlayerResInfo";
        //国家
        public const string UpdatePlayerCountry = "UpdatePlayerCountry";
        //玩家信息
        public const string ChangeRolePos = "ChangeRolePos";
        public const string ChangeRolePosGuide = "ChangeRolePosGuide";
        //推送设置变化
        public const string NoticeSettingChange = "NoticeSettingChange";
        //连续登录天数
        public const string ContinueLoginDayChange = "ContinueLoginDay";
        public const string GrowthFundChange = "GrowthFundChange";
        public const string GuildIdChange = "GuildIdChange";//联盟id发生变化

        #endregion

        #region HelpTip信息
        public const string FilterHelpTip = "FilterHelpTip";
        #endregion

        #region 系统事件

        //登录授权问题
        public const string AuthEvent = "AuthEvent";
        
        //网络状态事件
        public const string NetEvent = "NetEvent";

        //聊天服务器登录授权事件
        public const string ChatClientAuthEvent = "ChatClientAuthEvent";

        //聊天网络状态事件
        public const string ChatClientNetEvent = "ChatClientNetEvent";

        public const string SwitchMainCityCmd = "SwitchMainCityCmd";
        
        //昼夜时间 一分钟一次
        public const string DayNightTimeTick = "DayNightTimeTick";
        
        //昼夜交替
        public const string DayNightChange = "DayNightChange";

        //昼夜初始化
        public const string DayNightInit = "DayNightInit";


        public const string NetWorkReconnecting = "NetWorkReconnecting";

        public const string FirstEnterCity = "FirstEnterCity";
        public const string EnterCity = "EnterCity";
        public const string ClickEnterCity = "ClickEnterCity";

        public const string ExitCity = "ExitCity";
        public const string ClickExitCity = "ClickExitCity";

        public const string ResetGameData = "ResetGameData";

        //进入主城时候显示物件
        public const string EnterCityShow = "EnterCityShow";
        //出主城时候隐藏物件
        public const string ExitCityHide = "ExitCityHide";
		//首次进城前  准备拉镜头
	    public const string FirstEnterCityStartEndter = "FirstEnterCityStartEndter";

        public const string OnApplicationFocus = "OnApplicationFocus";

        #endregion

        #region UI控制

        public const string OnShowUI = "OnShowUI";
        public const string OnCloseUI = "OnCloseUI";
        public const string OpenUI = "OpenUI";
        public const string OpenUI2 = "OpenUI2";

        #endregion

        #region 视野变化
        public const string MapViewChange = "MapViewChange";
        public const string CityInViewPort = "CityInViewPort";//城市进入视野
        #endregion

        #region 3D点击事件

        public const string OnTouche3DBegin = "OnTouche3DBegin";
        public const string OnTouche3D = "OnTouche3D";
        public const string OnTouche3DEnd = "OnTouche3DEnd";
        public const string OnTouche3DReleaseOutside = "OnTouche3DReleaseOutside";

        #endregion
        #region 背包模块
        public const string ItemInfoChange = "ItemInfoChange";
        public const string ItemUse = "ItemUse";
        public const string ItemReddotChange = "ItemReddotChange";

        #endregion  
        #region  城市buff
        public const string CityBuffChange = "CityBuffChange";
 #endregion 
        #region NPC
        /// <summary>
        /// 显示剧情对白
        /// </summary>
        public const string ShowNPCDiaglog = "ShowNPCDiaglog";

        /// <summary>
        /// 结束剧情动画
        /// </summary>
        public const string OnNPCDiaglogEnd = "OnNPCDiaglogEnd";
        /// <summary>
        /// 跳过剧情动画
        /// </summary>
        public const string OnNPCDiaglogSkip = "OnNPCDiaglogSkip";

        /// <summary>
        /// 显示章节对白
        /// </summary>
        public const string ShowChapterDiaglog = "ShowChapterDiaglog";

        /// <summary>
        /// 结束章节对白
        /// </summary>
        public const string OnChapterDiaglogEnd = "OnChapterDiaglogEnd";
        #endregion

        #region 货币模块
        /// <summary>
        /// 浮动货币数值
        /// </summary>
        public const string UpdateFloatCurrency = "UpdateFloatCurrency";

        /// <summary>
        /// 飘飞后增加货币数值
        /// </summary>
        public const string FlyUpdatePlayerCurrency = "FlyUpdatePlayerCurrency";

        /// <summary>
        /// 更新货币数值
        /// </summary>
        public const string UpdateCurrency = "UpdateCurrency";

        public const string PlayCurrencyPopAni = "PlayCurrencyPopAni";

        /// <summary>
        /// 显示主城货币信息
        /// </summary>
        public const string ShowCurrencyMaskInfo = "ShowCurrencyMaskInfo";
        #endregion

        #region 资源生产
        /// <summary>
        /// 更新资源生产hud
        /// </summary>
        public const string UpdateBuildingResourcesHud = "UpdateBuildingResourcesHud";
        public const string UpdateBuildingTavernHud = "UpdateBuildingTavernHud"; //统帅厅免费招募
        #endregion

        #region 建筑
        public const string CityBuildinginfoFirst = "CityBuildinginfoFirst";
        public const string CityBuildinginfoChange = "CityBuildinginfoChange";
        public const string CityAgeChange = "CityAgeChange";//时代变迁
        public const string CityAgeChangeLevelUpEffect = "CityAgeChangeLevelUpEffect";//城市建筑升级替换前
        public const string BuildCityType = "BuildCityType";
        public const string ShowBuildingMenu = "ShowBuildingMenu";//显示建筑菜单  ,进入城市，显示菜单，移动建筑到中心 
        public const string ShowBuildingMenuAndFinger = "ShowBuildingMenuAndFinger";//显示建筑菜单和手指指向
        public const string ShowBuildingMenuAndMoveCameraToBuilding = "ShowBuildingMenuAndMoveCameraToBuilding";//显示建筑菜单并移动摄像头到建筑，不进入城市
        public const string ShowBuildingMenuOnly = "ShowBuildingMenuOnly";//只显示建筑菜单
        public const string MoveCameraToBuilding = "MoveCameraToBuilding";//移动摄像头到建筑
        public const string CityBuildingDone = "CityBuildingDone";//城市建筑生成完毕
        public const string CityBuildingLevelUP = "CityBuildingLevelUP";//城市升级，建筑成功
        public const string CityBuildingLevelUPCancel = "CityBuildingLevelUPCancel";//城市退出升级
        public const string CityBuildingStart = "CityBuildingStart";//城市开始建造
        public const string CityBuildingLevelUPStart = "CityBuildingLevelUPStart";//城市开始升级
        public const string CityBuildingStartCancel = "CityBuildingStartCancel";//建筑退出建造
        public const string OtherBuildingChange = "OtherBuildingChange";//其他人的城市有变动
        public const string RemoveOtherCity = "RemoveOtherBuilding";//移除 其他人的建筑
        public const string CityFireStateChange = "FireStateChange";//城市着火状态变化
        public const string CreateotherCity = "CreateotherCity";//生成其他人的城市
        public const string CreateCityDone = "CreateCityDone";
        public const string CityBeginBurnTimeChange = "CityBeginBurnTimeChange";//其他人建筑着火
        public const string CityCitybuffChange = "CityCitybuffChange";//其他人护盾变化
        public const string CityPosTimeChange = "CityPosTimeChange";//其他人位置变化
        

        #endregion

        #region 加速
        public const string SpeedUp = "SpeedUp";
        #endregion
        #region 邮件
        /// <summary>
        /// 更新邮件
        /// </summary>
        public const string UpdateEmail = "UpdateEmail";
        public const string ChangeSelectEmailIndex = "ChangeSelectEmailIndex";
        public const string AddEmailBubble = "AddEmailBubble";
        public const string GetEmailFightDetail = "GetEmailFightDetail";
        //选择发件人
        public const string OnSelectEmailTarget = "OnSelectEmailTarget";
        #endregion
        #region 任务
        public const string FinishSideTasks = "FinishSideTasks";//已完成并领取奖励的支线任务列表
        public const string TaskStatisticsSum = "TaskStatisticsSum";//任务累计统计信息
        public const string ActivePointChange = "ActivePointChange";//任务活跃度
        public const string ActivePointRewardsChange = "ActivePointRewardsChange";//已领取奖励的活跃度值
        public const string LevelChange = "LevelChange";//等级变化

        public const string MainLineTaskId = "MainLineTaskId";//主线任务ID，0为当前无主线任务
        public const string ChapterTasks = "ChapterTasks";//章节任务信息
        public const string DailyTasks = "DailyTasks";//每日任务信息
        public const string UpdateTaskStatistics = "UpdateTaskStatistics";//更新任务进度信息
        public const string ChapterId = "ChapterId";//当前的章节ID，0为无章节任务, -1为完成所有的章节

        public const string GoScript = "GoScript";//任务的前往脚本
        public const string ChapterIdChange = "ChapterIdChange";//章节改变
        public const string TaskRewardEnd = "TaskRewardEnd";//章节任务奖励结束
        public const string TaskStateChange = "TaskStateChange";//任务状态改变
        public const string TipRewardGroup = "TipRewardGroup";//奖励组tip显示

        public const string BuildingMenuJump = "BuildingMenuJump";

        public const string TaskGuideRemind = "TaskGuideRemind"; //任务引导提醒
        public const string CancelGuideRemind = "CancelGuideRemind";
        #endregion
        #region 驻防
        public const string MainHeroIdChange = "MainHeroIdChange";//驻防主将
        public const string DeputyHeroIdChange = "DeputyHeroIdChange";//驻防副将

        #endregion
        #region 警戒塔
        public const string GuardTowerHpChange = "GuardTowerHpChange";//警戒塔生命值

        #endregion
        #region 部队

        public const string OnTroopDataChanged = "OnTroopDataChanged";
        public const string TouchTroopSelectByFightHudIcon = "TouchTroopSelectByFightHudIcon";
        public const string TouchTroopSelect = "TouchTroopSelect";
        public const string DoubleTouchTroopSelect = "DoubleTouchTroopSelect";
        public const string UIPressed = "UIPressed";
        public const string SelectTroopDragMove = "SelectTroopDragMove";
        public const string OnCreatePlayBuildingSucceed = "OnCreatePlayBuildingSucceed";
        public const string TouchScoutSelect = "TouchScoutSelect";
        public const string TouchTransportSelect = "TouchTransportSelect";
        public const string CancelCameraFollow = "CancelCameraFollow";
        public const string SetSelectTroop = "SetSelectTroop";
        public const string OnRefreshTroopSave = "OnRefreshTroopSave";
        public const string OnRefreshSaveIndexViewBule = "OnRefreshSaveIndexView";
        public const string OnRefreshSaveIndexViewYellow = "OnRefreshSaveIndexViewYellow";
        public const string OnRefreshSaveIndexViewRed = "OnRefreshSaveIndexViewRed";
        public const string OnRefreshSaveNumView = "OnRefreshSaveNumView";
        public const string OnAutoRefreshSaveIndexView = "OnAutoRefreshSaveIndexView";

        public const string OnUIStartTouchTroop = "OnUIStartTouchTroop";
	    public const string OnCloseSelectMainTroop = "OnCloseSelectMainTroop";
	    public const string OnOpenSelectMainTroop = "OnOpenSelectMainTroop";
        public const string OnOpenSelectDoubleTroop = "OnOpenSelectDoubleTroop";
        public const string ClearGlobalTouchMoveCollide = "ClearGlobalTouchMoveCollide";
	    public const string OnTouchMoveUITroopCallBack = "OnTouuchMoveUITroopCallBack";

        #endregion
        #region 迷雾相关

        public const string FogSystemLoadEnd = "FogSystemLoadEnd";
        public const string UnlockAllFog = "UnlockAllFog";
        public const string FogUnlock = "FogUnlock";
        #endregion
        #region 英雄
        public const string GetNewHero = "GetNewHero";
        public const string UpdateHero = "UpdateHero";
        public const string ClickHeroEquip = "ClickHeroEquip";
        public const string ClickHeroEquipItem = "ClickHeroEquipItem";
        public const string HeroListVisible= "HeroListVisible";
        public const string RefreshEquipRedPoint= "RefreshEquipRedPoint";
        public const string RefreshMainChatPoint= "RefreshMainChatPoint";//主界面的聊天红点
        public const string HeroSceneVisible = "HeroSceneVisible";
        public const string SkillUpSourceRefresh = "SkillUpSourceRefresh";
        #endregion

        #region 统帅
        public const string ClickTalentPage = "ClickTalentPage";
        public const string HeroSkillUpSuccess = "HeroSkillUpSuccess";
        public const string SetHeroSkillLineSort = "SetHeroSkillLineSort";
        #endregion

        #region 新手引导
        public const string ShowUE_Finger = "ShowUE_Finger";
        public const string GuideTriggerCheck = "GuideTriggerCheck";
        public const string GuideInitOther = "GuideInitOther";
        public const string GuideInitData = "GuideInitData";
        public const string GuideShow = "GuideShow";
        public const string NextGuideStep = "NextGuideStep";
        public const string GuideForceShowResCollect = "GuideForceShowResCollect";
        public const string BuidingMenuOpen = "BuidingMenuOpen";
        public const string FirstExploreFog = "FirstExploreFog";
        public const string FirstGetHeroGuide = "FirstGetHeroGuide";
        public const string ForceCloseGuide = "ForceCloseGuide";
        public const string StartProcessGuide = "StartProcessGuide";
        public const string MainViewFirstInitEnd = "MainViewFirstInitEnd";
        public const string GuideFinished = "GuideFinished";                           //引导结束
        public const string HideGuideMask = "HideGuideMask";
        public const string TaskFinishToGuide = "TaskFinishToGuide";

        public const string GuideFindMonster = "GuideFindMonster";                     //查找怪物通知
        public const string GuideFoundMonster = "GuideFoundMonster";                   //找到怪物通知
        public const string GuideClickMonster = "GuideClickMonster";                   //点击怪物通知
        public const string GuideFirstMarch = "GuideFirstMarch";                       //第一次行军
        public const string MonsterFightEnd = "MonsterFightEnd";                       //战斗结束通知
        public const string GuideSecondSearchMonster = "GuideSecondSearchMonster";     //第二次查找怪物通知
        public const string GuideSecondMarch = "GuideSecondMarch";                     //第二次行军
        public const string GuideArmyReturn = "GuideArmyReturn";                       //通知军队回城

        //自动跟随部队回城
        public const string AutoFollowTroopReturnCity = "AutoFollowTroopReturnCity";   //镜头跟随野蛮人回城
        //点击野蛮人
        //public const string ClickBarbarian = "ClickBarbarian";
        #endregion

        #region 其他
        public const string AddResWinClose = "AddResWinClose";
        public const string TestReLogin = "TestReLogin";
        public const string ReRoleLogin = "ReRoleLogin";
        public const string SwitchDayNight = "SwitchDayNight";
        public const string GameTimeInit = "GameTimeInit";  //用来初始化跨天检测
        public const string SystemDayTimeChange = "SystemDayTimeChange";//跨天通知
        public const string CollectRuneFinish = "CollectRuneFinish";
		public const string SystemDayChange = "SystemDayChange";        //天数变更通知
		public const string SystemHourChange = "SystemHourChange";        //天数变更通知
        public const string CloseAddSpeed = "CloseAddSpeed";
        #endregion
        #region 奖励展示
        public const string RewardGet = "RewardGet";
        #endregion
        #region Map

        public const string MapRemoveTroopLine = "MapRemoveTroopLine";
        public const string MapCloseTroopHudUI = "MapCloseTroopHudUI";
        public const string MapSendMapMarChData = "MapSendMapMarChData";
        public const string MapSendchangeHudIcon = "MapSendchangeHudIcon";
        public const string MapRssItemCreate = "MapRssItemCreate";
        public const string MapRemoveRssItem = "MapRemoveRssItem";
        public const string MapObjectChange = "MapObjectChange";
        public const string MapObjectChange_ObjectInfoReq = "MapObjectChange_ObjectInfoReq";
        public const string MapLogicObjectChange = "MapLogicObjectChange";
        public const string MapObjectHUDUpdate = "MapObjectHUDUpdate";
        public const string MapObjectRemove = "MapObjectRemove";
        public const string MapCreateTroopSelectHud = "MapCreateTroopSelectHud";
        public const string MapCloseDoubleSelectTroopHud = "MapCloseDoubleSelectTroopHud";
        public const string MapRemoveTroopSelectHud = "MapRemoveTroopSelectHud";
        public const string MapCreateTroopFightHud = "MapCreateTroopFightHud";
        public const string MapRemoveTroopFightHud = "MapRemoveTroopFightHud";
        public const string MapCreateMonsterFightHud = "MapCreateMonsterFightHud";
        public const string MapRemoveMonsterFightHud = "MapRemoveMonsterFightHud";
        public const string MapPointInfoUpdate = "MapPointInfoUpdate";
        public const string MapCreateTroopHud = "MapCreateTroopHud";
        public const string MapRemoveTroopHud = "MapRemoveTroopHud";
        public const string MapClearTroopHud = "MapClearTroopHud";
        public const string MapUpdateTroopHud = "MapUpdateTroopHud";
        public const string MapCloseTroopHudScale = "MapCloseTroopHudScale";
        public const string MapOpenTroopHudScale = "MapOpenTroopHudScale";
        public const string MapPlayShottTextHud = "MapPlayShottTextHud";
        public const string MapStopShottTextHud = "MapStopShottTextHud";
        public const string MapCreateShottTextHud = "MapCreateShottTextHud";
        public const string MapTouchMyCity = "MapTouchMyCity";
        public const string MapUnSelectCity = "MapUnSelectCity";
        public const string MapCreateTroopGo = "MapCreateTroopGo";
        public const string MapSetFightBattleUIData = "MapSetFightBattleUIData";
        public const string MapCreateBuildingFightHud = "MapCreateBuildingFightHud";
        public const string MapRemoveBuildingFightHud = "MapRemoveBuildingFightHud";
        public const string MapTouchOtherCity = "MapTouchOtherCity";
        public const string MapCreateRuneGatherHud = "MapCreateRuneGatherHud";
        public const string MapRemoveRuneGatherHud = "MapRemoveRuneGatherHud";
	    public const string MapShowBroadcasts = "MapShowBroadcasts";
	    public const string LoadMapObj = "LoadMapObj";
	    public const string MapUpdateBuildingHead = "MapUpdateBuildingHead";
	    public const string MapUpdateGuildAddName = "MapUpdateGuildAddName";
	    public const string MapUpdateArmyName = "MapUpdateArmyName";
        public const string MapObjectStatusChange = "MapObjectStatusChange";
        public const string MapCreateDrawLine = "MapCreateDrawLine";
        public const string MapDeleteDrawLine = "MapDeleteDrawLine";
        public const string MapRemoveAllDrawLine = "MapRemoveAllDrawLine";
        public const string MapRemoveSelectEffect = "MapRemoveSelectEffect";
        public const string MapCreateSelectMyTroopEffect = "MapCreateSelectMyTroopEffect";
        public const string MapDeleteSelectMyTroopEffect = "MapDeleteSelectMyTroopEffect";
        public const string MapRemoveAllSelectMyTroopEffect = "MapRemoveAllSelectMyTroopEffect";
        public const string MapTroopHudMapMarCh = "MapTroopHudMapMarCh";

        public const string WorldObject_Add = "WorldObject_Add"; // long objId
        public const string WorldObject_Remove = "WorldObject_Remove"; // long objId
        public const string MoveAndOpenCityByArmindex = "MoveAndOpenCity";//移动并打开城市，传军队index
        public const string MoveAndOpenCityByTarget = "MoveAndOpenCityByTarget";//移动并打开城市，传目标index

        public const string ArmyDataLodPopAdd = "ArmyDataLodPopAdd"; //玩家自己的大地图lod气泡
        public const string ArmyDataLodPopRemove = "ArmyDataLodPopRemove"; //玩家自己的大地图lod气泡
	    public const string ArmyDataLodPopUpdate = "ArmyDataLodPopUpdate"; //玩家自己的大地图lod气泡
	    
	    public const string RefreshGuildMemberPosView = "RefreshGuildMemberPosView"; //根据下发的联盟成员位置刷新视图
	    public const string DeleteMiniMapGuildMemberPos = "DeleteMiniMapGuildMemberPos"; //删除联盟成员位置视图

        public const string SetSelectScoutHead = "SetSelectScoutHead";
	    public const string MapDrawLineCity = "MapDrawLineCity";
	    public const string MapRemoveDrawLineCity = "MapRemoveDrawLineCity";

        #endregion

        #region 主城动画相关通知
        public const string ArmyTrainStart = "ArmyTrainStart"; //训练开始
        public const string ArmyTrainEnd = "ArmyTrainEnd"; //训练结束

        #endregion

        #region 时代变迁章节变化动画
        public const string BuildingLevelUpShow = "BuildingLevelUpShow";
        public const string ChapterTimelineShow = "ChapterTimelineShow";
        public const string ShowMask = "ShowMask";
        public const string HideMask = "HideMask";
        public const string AgeStart = "AgerStart";
        public const string AgeEnd = "AgeEnd";
        #endregion
   
        #region 斥候
        public const string ScoutQueueUpdate = "ScoutQueueUpdate";
        #endregion

        #region 战斗 

        public const string FightUpdateMonsteAttackDir = "FightUpdateMonsteAttackDir";
	    public const string FightUpdateTroopAttackDir = "FightUpdateTroopAttackDir";
	    public const string Fire = "FightUpdateTroopAttackDir";
        public const string WarWarningInfoChanged = "WarWarningInfoChanged";
	    public const string FightUpdateHeroLevel = "FightUpdateHeroLevel";

        #endregion

        #region 村庄山洞
        public const string VillageCavesChange = "VillageCavesChange";//村庄山洞    
        #endregion
        #region 联盟
        public const string AllianceInfoUpdate = "AllianceInfoUpdate";
        
        public const string AllianceApplys = "AllianceApplys";
        
        public const string AllianceMembers = "AllianceMembers";
        public const string GuildOfficerInfoChange = "GuildOfficerInfoChange";

        public const string AllianceDepot = "AllianceDepot";

        
        public const string AllianceHelp = "AllianceHelp";

        public const string AllianceEixt = "AllianceEixt"; 
        public const string AllianceEixtEx = "AllianceEixtEx"; 
        public const string AllianceBuildUpdate = "AllianceBuildUpdate";
        
        public const string AllianceTerritoryUpdate = "AllianceTerritoryUpdate";//lod3下领土变化
        
        public const string AllianceTerritoryStrategicUpdate = "AllianceTerritoryStrategicUpdate";//lod3上领土变化
        
        
        public const string AllianceBuildArmyUpdate = "AllianceBuildArmyUpdate";
        
        public const string AllianceHolyLandUpdate = "AllianceHolyLandUpdate";	// 联盟圣地变化
        public const string AllianceTechUpdate = "AllianceTechUpdate";

        public const string AllianceSettingWelcomeMailChange = "AllianceSettingWelcomeMailChange";
	    public const string AllianceJoinUpdate = "AllianceJoinUpdate";

	    public const string AllianceStudyDonateRedCount = "AllianceDoRedCount";

	    public const string AllianceBlock = "AllianceBlock";

	    public const string AllianceGiftRedPoint = "AllianceGiftRedPoint";
	    
	    
	    public const string AllianceRssRedPoint = "AllianceRssRedPoint";

        public const string AllianceBuildCanCreateFlag = "AllianceBuildCanCreateFlag";
        public const string AllianceBuildCanCreateCheck = "AllianceBuildCanCreateCheck";

        #endregion
        #region 集结
        public const string MoveToPosFixedCameraDxf = "MoveToPosFixedCameraDxf";//移动视角高度强制设置map_tactical
        public const string MoveToPosPullUpCameraDxf = "MoveToPosPullUpCameraDxf";//移动视角,高度拉高到至少map_tactical
        public const string MoveToPosAndOpen = "MoveToPosAndOpen";//移动视角打开城市菜单
        public const string RallyTroopChange = "RallyTroopChange";//集结被集结部队态改变   

        public const string GetCityReinforceInfo = "GetCityReinforceInfo";//请求目标的城市增援信息

        public const string ChangeGuildCityHud = "ClearGuildCityHud"; //集结队伍hud变化
        #endregion

        #region 组界面ui显示情况
        public const string QuestBtnChange = "QuestBtnChange";
        public const string FeatureBtnChange = "FeatureBtnChange";
        public const string ChangeRoleObjPos = "ChangeRoleObjPos";

        
        #endregion
        #region 酒馆
        public const string TavernBoxInfoChange = "TavernBoxInfoChange";

        #endregion

        #region OpenUI参数列表
        public const string ChangeResearchMainToggle = "ChangeResearchMainToggle";
        #endregion

        #region 跑马灯
        public const string AddScrollMessage = "";
        #endregion

        #region 聊天
        //更新聊天内容
        public const string UpdateChatMsg = "UpdateChatMsg";
        //更新频道
        public const string UpdateChatContact = "UpdateChatContact";
        //更新聊天红点
        public const string UpdateChatRedDot = "UpdateChatRedDot";

        public const string DeleteChatContact = "DeleteChatContact";
        
        #region 商店相关

        public const string OnMysteryStoreOpen = "OnMysteryStoreOpen";//神秘商店开启
        public const string OnMysteryStoreClose = "OnMysteryStoreClose";//神秘商店结束
        public const string HideMysteryStoreBubble = "HideMysteryStoreBubble";//隐藏神秘商店气泡
        public const string OnMysteryStoreRefresh = "OnMysteryStoreRefresh";//神秘商店刷新

        #endregion

        #endregion

        #region 活动
        public const string UpdateActivityReddot = "UpdateActivityReddot";
        public const string UpdateActivityTotalReddot = "UpdateActivityTotalReddot";
        public const string ReceiveOpenServerBox = "ReceiveOpenServerBox";        //领取开服活动宝箱奖励
        public const string ReceiveBehaviorReward = "ReceiveBehaviorReward";      //领取行为奖励
        public const string ActivityScheduleUpdate = "ActivityScheduleUpdate";
        public const string ActivitySwitch = "ActivitySwitch";                    //活动切换            
        public const string ActivityExchangeRefresh = "ActivityExchangeRefresh";  //兑换刷新         
        public const string ActivityRankOrScoreUpdate = "ActivityRankOrScoreUpdate";        //最强执政官数据刷新      
        public const string ActivityActivePointUpdate = "ActivityActivePointUpdate";   //新手活跃度数据更新
        public const string ActivityActivePointChange= "ActivityActivePointChange";   //新手活跃度变更
        public const string ActivityTurntableAniMask = "ActivityTurntableAniMask";    //幸运转盘遮罩开启通知

        public const string SwitchActivityMenu = "SwitchActivityMenu";
        public const string ActivityTurnTableReturn = "ActivityTurnTableReturn";
        public const string RefreshActivityNewFlag = "RefreshActivityNewFlag";

        #endregion

        #region 充值
        public const string ChargeListToggleChanged = "ChargeListToggleChanged";
        public const string OnServerCallbackChangedRecharge = "OnServerCallbackChangedRecharge";
        public const string OnServerCallbackChangedRiseRoad = "OnServerCallbackChangedRiseRoad";
        public const string UpdateRechargeReddot = "UpdateRechargeReddot";
        public const string JumpToChargeListByPageType = "JumpToChargeListByPageType";
        public const string UpdateSupplyInfo = "UpdateSupplyInfo";
        public const string UpdateSuperGiftInfo = "UpdateSuperGiftInfo";
        public const string UpdateFreeDailyBoxInfo = "UpdateFreeDailyBoxInfo";
        public const string UpdateDailySpecialInfo = "UpdateDailySpecialInfo";
        public const string LimitTimePackageChange = "LimitTimePackageChange";
        public const string LimitTimePackageState = "LimitTimePackageState";
        public const string NewLimitTimePackage = "NewLimitTimePackage";
        public const string RemoveLimitTimePackage = "RemoveLimitTimePackage";
        public const string PageClick = "PageClick";
        public const string BuyItemCallBack = "BuyItemCallBack";//充值成功回调

        #endregion

        #region VIP
        //VIP系统
        public const string VipDailyPointFlagChange = "VipDailyPointFlagChange";
        public const string VipPointChange = "VipPointChange";
        public const string VipFreeBoxFlagChange = "VipChangeFreeBoxFlag";
        public const string VipClaimFreeBox = "ClaimFreeVipBox";
        public const string VipSpecialBoxChange = "VipSpecialBoxChange";
        public const string VipBuySpecialBox = "BuySpecialVipBox";
        public const string VipLevelUP = "VipLevelUP";
        
        //VIP商店
        public const string VipStoreInfoChange = "VipStoreInfoChange";
        public const string VipStoreShowBubbleChange = "VipStoreShowBubbleChange";
        #endregion
        #region 联盟中心
        public const string ReinforcesChange = "ReinforcesChange";
        #endregion

        #region 更换文明
        public const string ChangeCivilizationCmd = "ChangeCivilizationCmd";
        #endregion

        #region 装备
        //材料
        public const string EquipMaterialProductionInfoChange = "EquipMaterialProductionInfoChange";
        public const string EquipQuickForge = "EquipQuickForge";
        public const string EquipForgeRedDotChange = "EquipForgeRedDotChange";
        #endregion

        #region 功能引导

        public const string FuncGuideTrigger = "FuncGuideTrigger";
        public const string NextFuncGuideStep = "NextFuncGuideStep";
        public const string FuncGuideCheck = "FuncGuideCheck";
        public const string FuncGuideFiveSpeed = "FuncGuideFiveSpeed";
        public const string FuncGuideId = "FuncGuideId";
        public const string FuncGuideMonsterPowerShow = "FuncGuideMonsterPowerShow";
        public const string FuncGuideMonsterPowerHide = "FuncGuideMonsterPowerHide";
        public const string FuncGuideMoveCityEffect = "FuncGuideMoveCityEffect";

        #endregion

        #region 远征

        public const string EnterExpeditionMap = "EnterExpeditionMap";
        public const string ExitExpeditionMap = "ExitExpeditionMap";
        public const string SetFogBorderVisible = "SetFogBorderVisible";
        public const string CreateExpeditionTroop = "CreateExpeditionTroop";
        public const string ExpeditionFightFinish = "ExpeditionFightFinish";
        public const string RetryExpedtionFight = "RetryExpedtionFight";
        public const string ExpeditionInfoChange = "ExpeditionInfoChange";
        public const string ExpeditionCoinChange = "ExpeditionCoinChange";
        public const string PlayerExpeditionInfoChange = "PlayerExpeditionInfoChange";
        public const string ExpeditionFightStart = "ExpeditionFightStart";
        public const string ExpeditionPrepareTroopChanage = "ExpeditionPrepareTroopChanage";
        public const string ExpeditionTroopRemove = "ExpeditionTroopRemove";
        public const string ExpeditionTaskUICloseWhenFightting = "ExpeditionTaskUICloseWhenFightting";
        public const string ExpeditionShowArmySelectUI = "ExpeditionShowArmySelectUI";
        public const string ExpeditionHideArmySelectUI = "ExpeditionHideArmySelectUI";
        public const string ExpeditionReadyAniEnd = "ExpeditionReadyAniEnd";
	    public const string ExpeditionSelectTroop = "ExpeditionSelectTroop";
        public const string ExpeditionUISelectTroop = "ExpeditionUISelectTroop";
        public const string ExpeditionUIDoubleSelect = "ExpeditionUIDoubleSelect";
        public const string ExpeditionCreateDrawLine = "ExpeditionCreateDrawLine";
        public const string ExpeditionDeleteDrawLine = "ExpeditionDeleteDrawLine";
        public const string ExpeditionRemoveAllDrawLine = "ExpeditionRemoveAllDrawLine";
        public const string ExpeditionCreateSelectMyTroopEffect = "ExpeditionCreateSelectMyTroopEffect";
        public const string ExpeditionDeleteSelectMyTroopEffect = "ExpeditionDeleteSelectMyTroopEffect";
        public const string ExpeditionRemoveAllSelectMyTroopEffect = "ExpeditionRemoveAllSelectMyTroopEffect";
        public const string ExpeditionTroopHudMapMarCh = "ExpeditionTroopHudMapMarCh";

        #endregion

        #region 战役

        public const string BattleMainRedPointChanged = "BattleMainRedPointChanged";

        #endregion

	    #region 角色管理

	    public const string RoleInfo_Refresh="RoleInfo_Refresh";
	    

	    #endregion

        #region 地图书签

        public const string PersonMapMarkerInfoChanged = "PersonMapMarkerInfoChanged";
        public const string GuildMapMarkerInfoChanged = "GuildMapMarkerInfoChanged";
        public const string GuildMapMarkerInfoCleared = "GuildMapMarkerInfoCleared";        
        public const string GuildMapMarkerInfoDelete = "GuildMapMarkerInfoDelete";
        public const string GuildMapMarkerInfoAdd = "GuildMapMarkerInfoAdd";
        public const string GuildMapMarkerInfoEdit = "GuildMapMarkerInfoEdit";

        #endregion

        #region 帐号

        public const string AccountBindReddotCheck = "AccountBindReddotCheck";
        public const string AccountBindReddotStatus = "AccountBindReddotStatus";

        #endregion


        public const string GameModeChanged = "GameModeChanged";
        public const string WorldEditStateChanged = "WorldEditStateChanged";

        //------------------------------江海整理确认过的CMD放到下面， 主要方便后续， 整理优化代码的时候放置--------------------------------------------------
        // 已经移除，移到 AutoLogin 里面去
        //public const string ResetSceneCmd = "ResetSceneCmd";
        // 重新加载游戏
        public const string ReloadGame = "ReloadGame";
        // 退出游戏
        public const string ExitGame = "ExitGame";
        // 重置所有的 MVC 数据
        public const string ResetSceneCmd = "ResetSceneCmd";
        // 摄像头相关指令
        public const string MoveCameraCmd = "MoveCameraCmd";

        // 加载游戏配置
        public const string LoadAppConfig = "LoadAppConfig";
        // 自动登陆
        public const string AutoLogin = "AutoLogin";
        // 自动登陆
        public const string AutoLoginFinished = "AutoLoginFinished";
        // 账号登陆
        public const string LoginAccount = "LoginAccount";
        // 账号登陆完成
        public const string LoginAccountFinished = "LoginAccountFinished";
        // 切换账号
        public const string SwitchAccount = "SwitchAccount";
        // 切换账号完成
        public const string SwitchAccountFinished = "SwitchAccountFinished";
        // 连接游戏服务器
        public const string LoginToServer = "LoginToServer";
        // 检测维护公告
        public const string MaintainCheck = "MaintainCheck";
        // 检测维护公告单服维护
        public const string MaintainCheckSingleServer = "MaintainCheckSingleServer";
        // 检测整包更新
        public const string PackageUpdateCheck = "PackageUpdateCheck";
        // 检测热更新
        public const string HotfixUpteCheck = "HotfixUpteCheck";
        // 开始下载热更新包
        public const string HotfixStartDownload= "HotfixStartDownload";
        // 更新下载热更新包进度
        public const string HotfixDownloadProgress = "HotfixDownloadProgress";
        // 热更新包下载完成
        public const string HotfixDownCompleted = "HotfixDownCompleted";
        // 热更新包开始解压
        public const string HotfixUnCompress = "HotfixStartUnCompress";
        // 热更新包解压进度
        public const string HotfixUnCompressProgress = "HotfixUnCompressProgress";

        // 绑定账号

        public const string LoadAccountProfile = "LoadAccountProfile";
        public const string LoadAccountProfileFinished = "LoadAccountProfileFinished";
        public const string BindindAccount = "BindindAccount";
        public const string BindindAccountFinished = "BindindAccountFinished";
        public const string AccountBan = "AccountBan";

        // 用户协议
        public const string AgreementCheck = "AgreementCheck";

        // 应用评星
        public const string OpenAppRating = "OpenAppRating";
        public const string AppRatingLike = "AppRatingLike";
        //埋点
        public const string EventTracking = "EventTracking";
    }
}

