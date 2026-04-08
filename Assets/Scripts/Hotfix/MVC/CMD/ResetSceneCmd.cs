using Client;
using Hotfix;
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class ResetSceneCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            CoreUtils.logService.Info($"Start ResetScene:{Time.realtimeSinceStartup}", Color.green);

            AppFacade facade = AppFacade.GetInstance();

//            facade.RemoveProxy(NetProxy.ProxyNAME);
            facade.RemoveProxy(WorldMapObjectProxy.ProxyNAME);
            facade.RemoveProxy(DataProxy.ProxyNAME);
            facade.RemoveProxy(PlayerProxy.ProxyNAME);
            facade.RemoveProxy(CurrencyProxy.ProxyNAME);
            facade.RemoveProxy(CityBuildingProxy.ProxyNAME);
            facade.RemoveProxy(HeroProxy.ProxyNAME);
            facade.RemoveProxy(TroopProxy.ProxyNAME);
            facade.RemoveProxy(RallyTroopsProxy.ProxyNAME);         
            facade.RemoveProxy(ResearchProxy.ProxyNAME);
            facade.RemoveProxy(RssProxy.ProxyNAME);
            facade.RemoveProxy(SearchProxy.ProxyNAME);
            facade.RemoveProxy(BagProxy.ProxyNAME);
            facade.RemoveProxy(TrainProxy.ProxyNAME);
            facade.RemoveProxy(SoldierProxy.ProxyNAME);
            facade.RemoveProxy(MonsterProxy.ProxyNAME);
            facade.RemoveProxy(TaskProxy.ProxyNAME);
            facade.RemoveProxy(EmailProxy.ProxyNAME);
            facade.RemoveProxy(HospitalProxy.ProxyNAME);
            facade.RemoveProxy(BuildingResourcesProxy.ProxyNAME);
            facade.RemoveProxy(ScoutProxy.ProxyNAME);
            facade.RemoveProxy(EffectinfoProxy.ProxyNAME);
            facade.RemoveProxy(GuideProxy.ProxyNAME);
            facade.RemoveProxy(RewardGroupProxy.ProxyNAME);
            facade.RemoveProxy(PlayerAttributeProxy.ProxyNAME);
            facade.RemoveProxy(AllianceProxy.ProxyNAME);
			facade.RemoveProxy(StoreProxy.ProxyNAME);
            facade.RemoveProxy(ChatProxy.ProxyNAME);
            facade.RemoveProxy(ActivityProxy.ProxyNAME);
            facade.RemoveProxy(RechargeProxy.ProxyNAME);
            facade.RemoveProxy(WorkerProxy.ProxyNAME);
            facade.RemoveProxy(CityBuffProxy.ProxyNAME);
            facade.RemoveProxy(MinimapProxy.ProxyNAME);
            facade.RemoveProxy(MoveCityProxy.ProxyNAME);
            facade.RemoveProxy(AllianceResarchProxy.ProxyNAME);
            facade.RemoveProxy(ScrollMessageProxy.ProxyNAME);
            facade.RemoveProxy(FuncGuideProxy.ProxyNAME);
            facade.RemoveProxy(ExpeditionProxy.ProxyNAME);
            facade.RemoveProxy(MapMarkerProxy.ProxyNAME);
            facade.RemoveProxy(RoleInfoProxy.ProxyNAME);

            var netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            if (netProxy == null)
            {
                facade.RegisterProxy(new NetProxy(NetProxy.ProxyNAME));
            }
            //在这里注册所有的Proxy
            facade.RegisterProxy(new WorldMapObjectProxy(WorldMapObjectProxy.ProxyNAME));
            facade.RegisterProxy(new DataProxy(DataProxy.ProxyNAME));
            facade.RegisterProxy(new PlayerProxy(PlayerProxy.ProxyNAME));
            facade.RegisterProxy(new CurrencyProxy(CurrencyProxy.ProxyNAME));
            facade.RegisterProxy(new CityBuildingProxy(CityBuildingProxy.ProxyNAME));
            facade.RegisterProxy(new HeroProxy(HeroProxy.ProxyNAME));
            facade.RegisterProxy(new TroopProxy(TroopProxy.ProxyNAME));
            facade.RegisterProxy(new RallyTroopsProxy(RallyTroopsProxy.ProxyNAME));
            facade.RegisterProxy(new ResearchProxy(ResearchProxy.ProxyNAME));
            facade.RegisterProxy(new RssProxy(RssProxy.ProxyNAME));
            facade.RegisterProxy(new SearchProxy(SearchProxy.ProxyNAME));
            facade.RegisterProxy(new BagProxy(BagProxy.ProxyNAME));
            facade.RegisterProxy(new TrainProxy(TrainProxy.ProxyNAME));
            facade.RegisterProxy(new SoldierProxy(SoldierProxy.ProxyNAME));
            facade.RegisterProxy(new MonsterProxy(MonsterProxy.ProxyNAME));
            facade.RegisterProxy(new TaskProxy(TaskProxy.ProxyNAME));
            facade.RegisterProxy(new EmailProxy(EmailProxy.ProxyNAME));
            facade.RegisterProxy(new HospitalProxy(HospitalProxy.ProxyNAME));
            facade.RegisterProxy(new BuildingResourcesProxy(BuildingResourcesProxy.ProxyNAME));
            facade.RegisterProxy(new ScoutProxy(ScoutProxy.ProxyNAME));
            facade.RegisterProxy(new EffectinfoProxy(EffectinfoProxy.ProxyNAME));
            facade.RegisterProxy(new GuideProxy(GuideProxy.ProxyNAME));
            facade.RegisterProxy(new RewardGroupProxy(RewardGroupProxy.ProxyNAME));
            facade.RegisterProxy(new PlayerAttributeProxy(PlayerAttributeProxy.ProxyNAME));
            facade.RegisterProxy(new AllianceProxy(AllianceProxy.ProxyNAME));
			facade.RegisterProxy(new StoreProxy(StoreProxy.ProxyNAME));
            facade.RegisterProxy(new ChatProxy(ChatProxy.ProxyNAME));
            facade.RegisterProxy(new ActivityProxy(ActivityProxy.ProxyNAME));
            facade.RegisterProxy(new RechargeProxy(RechargeProxy.ProxyNAME));
            facade.RegisterProxy(new WorkerProxy(WorkerProxy.ProxyNAME));
            facade.RegisterProxy(new CityBuffProxy(CityBuffProxy.ProxyNAME));
            facade.RegisterProxy(new MinimapProxy(MinimapProxy.ProxyNAME));
            facade.RegisterProxy(new MoveCityProxy(MoveCityProxy.ProxyNAME));
            facade.RegisterProxy(new AllianceResarchProxy(AllianceResarchProxy.ProxyNAME));
            facade.RegisterProxy(new ScrollMessageProxy(ScrollMessageProxy.ProxyNAME));
            facade.RegisterProxy(new WarWarningProxy(WarWarningProxy.ProxyNAME));
            facade.RegisterProxy(new FuncGuideProxy(FuncGuideProxy.ProxyNAME));
            facade.RegisterProxy(new ExpeditionProxy(ExpeditionProxy.ProxyNAME));
            facade.RegisterProxy(new MapMarkerProxy(MapMarkerProxy.ProxyNAME));
            facade.RegisterProxy(new AccountProxy(AccountProxy.ProxyNAME));
            facade.RegisterProxy(new RoleInfoProxy(RoleInfoProxy.ProxyNAME));
            facade.RegisterProxy(new GeneralSettingProxy(GeneralSettingProxy.ProxyNAME));

            var hospitalProxy = AppFacade.GetInstance().RetrieveProxy(HospitalProxy.ProxyNAME) as HospitalProxy;
            hospitalProxy.Init();
            var guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
            guideProxy.Init();
            var taskProxy = AppFacade.GetInstance().RetrieveProxy(TaskProxy.ProxyNAME) as TaskProxy;
            taskProxy.Init();
            var troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            troopProxy.Init();
            var trainProxy = AppFacade.GetInstance().RetrieveProxy(TrainProxy.ProxyNAME) as TrainProxy;
            trainProxy.Init();
            var worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            worldMapObjectProxy.Init();
            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            playerProxy.Init();
            var scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            scoutProxy.Init();
            var storeProxy = AppFacade.GetInstance().RetrieveProxy(StoreProxy.ProxyNAME) as StoreProxy;
            storeProxy.Init();
            var activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            activityProxy.Init();
            var rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            rechargeProxy.Init();
            var rssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            rssProxy.Init();
            var rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            rewardGroupProxy.Init();
            var rallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            rallyTroopsProxy.Init();
            var funcGuideProxy = AppFacade.GetInstance().RetrieveProxy(FuncGuideProxy.ProxyNAME) as FuncGuideProxy;
            funcGuideProxy.Init();
            var emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
            emailProxy.Init();
            var mapMarkerProxy = AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy;
            mapMarkerProxy.Init();
            var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            allianceProxy.Init();
			var accountProxy = AppFacade.GetInstance().RetrieveProxy(AccountProxy.ProxyNAME) as AccountProxy;
            accountProxy.Init();
            var generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
            generalSettingProxy.Init();
            FightHelper.Instance.InitProxy();
            ArmyInfoHelp.Instance.InitProxy();

            
            GlobalBehaviourManger.Instance.RemoveAllGlobalMediator();
            
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalEffectMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalViewLevelMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GameEventGlobalMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<ServerPushMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<InputGlobalMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GameToolsGlobalMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<CityGlobalMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<PVPGlobalMediator>(true);

            GlobalBehaviourManger.Instance.AddGlobalMeditor<HospitalStatusGlobalMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<MonumentGloablMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<ArmyTrainStatusGlobalMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<FogSystemMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalTechnologyMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalResourceMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalCityBuidlingMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalAllianceHelpMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalCityMenuHudMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalWorldHudMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GuideGlobalMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<ItemCollectGlobalMediator>(false);

            GlobalBehaviourManger.Instance.AddGlobalMeditor<TaskScriptGlobalMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalFilmMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<SendClientDeviceInfoMedia>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<WorldMgrMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<TroopHUDMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<BattleGlobalMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<TroopGlobalMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<MapUIiconMyCityMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalGameTimeMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalStoreMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalLimitTimePackageMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<FuncGuideGlobalMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<ExpeditionMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<BattleMainlMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<GlobalScoutsCampMediator>(false);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<TaskRemindGlobalMediator>(false);
			GlobalBehaviourManger.Instance.AddGlobalMeditor<MapMarkerGlobalMediator>(true);
            GlobalBehaviourManger.Instance.AddGlobalMeditor<AllianceBuildCheckGlobalMediator>(false);
			GlobalBehaviourManger.Instance.AddGlobalMeditor<OtherGlobalMediator>(false);

            if (!Application.isMobilePlatform)
            {
                if (PlayerPrefs.GetInt("TroopLineRecord", 0) == 1)
                {
                    GlobalBehaviourManger.Instance.AddGlobalMeditor<TroopLineMediator>(true);
                }
            }
            
            
            ManorMgr.ClearAllLine_S(true,true);
        }
    }
}