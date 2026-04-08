// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, December 26, 2019
// Update Time         :    Thursday, December 26, 2019
// Class Description   :    PlayerProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System;
using Skyunion;
using SprotoType;
using Random = UnityEngine.Random;
using PureMVC.Interfaces;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using LitJson;
using System.Globalization;
using Client;

namespace Game {
    public class ServerImformation
    {
        public string servername;
        public string host;
        public int port;
        public string sid;
    }

    public class PlayerProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "PlayerProxy";
        public static string signKey = "362a5b5bd8e36dc148e706f5c0903416";
        public static ServerImformation[] serverImformations;
        public static ServerImformation curServerImformation;
        public static bool LoginInitIsFinish = false;

        public static bool IsFogSystemInited = false;
        public static bool IsMonumentflag = false;//纪念碑标志位，打开纪念碑界面置false
        public static bool LoadCityFinished = false;//主城加载完毕//重连不重置

        private long m_loginedRoleID;
        
        
        private Dictionary<long,RoleInfoEntity> m_roleInfo = new Dictionary<long, RoleInfoEntity>();
        public bool m_isFirstGetRoleInfo = true;


        private CityBuildingProxy m_cityBuildProxy;
        private ResearchProxy m_researchProxy;

        private HeroProxy m_heroProxy;
        private SoldierProxy m_troopProxy;
        private ActivityProxy m_activityProxy;

        private ChatProxy m_chatProxy;
        private BagProxy m_bagProxy;

        private Role_RoleLogin.response m_roleLoginRes;

        private int m_maxVipLevel =-1;
        private RoleInfoEntity m_before = new RoleInfoEntity();
        public RoleInfoEntity CurrentRoleInfo
        {
            get {
                if (m_roleInfo.ContainsKey(m_loginedRoleID))
                    return m_roleInfo[m_loginedRoleID];
                return null;
                }
        }

        public long Rid
        {
            get;private set;
        }

        public long VipLevel
        {
            get
            {
                return m_curVIPLevel;
            }
            private set
            {
                if (value!= m_curVIPLevel)
                {
                    m_curVIPLevel = value;
                    AppFacade.GetInstance().SendNotification(CmdConstant.VipLevelUP,value);
                }
            }
        }

        private CivilizationDefine m_country;
        private ConfigDefine m_configDefine;
        public Dictionary<Int64, QueueInfo> TrainQueueMap = new Dictionary<Int64, QueueInfo>();
        private long currentLevel;
        private long m_curVIPLevel=-1;

        #endregion

        // Use this for initialization
        public PlayerProxy(string proxyName)
            : base(proxyName)
        {

        }

        public override void OnRegister()
        {
            Debug.Log(" PlayerProxy register");
            LoadCityFinished = false;
            LoginInitIsFinish = false;
            IsFogSystemInited = false;
            IsMonumentflag = false;
        }


        public override void OnRemove()
        {
            Debug.Log(" PlayerProxy remove");
            LoadCityFinished = false;
            LoginInitIsFinish = false;
           IsFogSystemInited = false;
            IsMonumentflag = false;
        }


        public void Init()
        {
            m_cityBuildProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
            m_chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            
            m_configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
        }

        public void SaveRoleLoginRes(Role_RoleLogin.response res)
        {
            m_roleLoginRes = res;
            if (res!=null)
            {
                m_chatProxy.OnConnection(res);
                m_chatProxy.OnGameServerConnect();
                m_chatProxy.LoadTopList();
                m_chatProxy.LoadUnTopList();
                m_chatProxy.LoadDeleteList();
            }
            m_bagProxy.GetForgeEquipIgnoreRedDotInfo();
        }

        public Role_RoleLogin.response GetRoleLoginRes()
        {
            return m_roleLoginRes;
        }

     

        /// <summary>
        /// 创建国家
        /// </summary>
        /// <param name="civilization"></param>
        public void CreateCountry(CivilizationDefine civilization)
        {
            m_country = civilization;
            
            Role_CreateRole.request rq = new Role_CreateRole.request();
            rq.name = LanguageUtils.getTextFormat(300323, Random.Range(1, 3000000));
            rq.country = civilization.ID;
            rq.version = Application.version;
            rq.languageId = (int)LanguageUtils.GetLanguage();
            AppFacade.GetInstance().SendSproto(rq);

        }
        /// <summary>
        /// 更换
        /// </summary>
        /// <param name="civilization"></param>
        public void ChangeCountry(CivilizationDefine civilization,bool isTool)
        {
            m_country = civilization;
            Role_ChangeCivilization.request rq = new Role_ChangeCivilization.request();
            rq.civilizationId = civilization.ID;
            rq.useItem = isTool;
            AppFacade.GetInstance().SendSproto(rq);
        }


        public CivilizationDefine Country()
        {
            return m_country;
        }
        public ConfigDefine ConfigDefine
        {
            get
            {
                return m_configDefine;
            }
        }

        //获取角色列表
        public void GetRoleList()
        {
            var sp = new Role_GetRoleList.request();
            CoreUtils.logService.Info("开始获取角色列表");
            AppFacade.GetInstance().SendSproto(sp);
        }

        //登录角色
        public void LoginRole(long rid)
        {
            CoreUtils.logService.Info("开始登陆角色"+rid);
            var sp = new Role_RoleLogin.request();
            sp.rid = rid;
            Rid = rid;
            sp.language = (int)LanguageUtils.GetLanguage();
            sp.phone = SystemInfo.deviceName;
            sp.version = Application.version;
            sp.area = IGGSDKUtils.shareInstance().getCountryCode();
            if(IGGSDK.appConfig == null)
            {
                sp.ip = "127.0.0.1";
            }
            else
            {
                sp.ip = IGGSDK.appConfig.getClientIp();
                if(string.IsNullOrEmpty(sp.ip))
                {
                    sp.ip = "127.0.0.1";
                }
            }
            
            var gameid = IGGSDK.shareInstance().getGameId();

            if (!string.IsNullOrEmpty(gameid))
            {
                sp.gameId = long.Parse(gameid);
            }
           

#if UNITY_EDITOR || UNITY_STANDLONE
            sp.platform = 3;
#elif UNITY_ANDROID
            sp.platform = 2;
#elif UNITY_IOS
            sp.platform = 1;
#endif

            //UnityWebRequest webRequest = UnityWebRequest.Get("http://pv.sohu.com/cityjson");
            //webRequest.SendWebRequest().completed += (op)=>
            //{
            //    if(webRequest.isNetworkError || webRequest.isHttpError)
            //    {
            //        sp.ip = PlayerPrefs.GetString($"ClientIP_{rid}", "known");
            //    }
            //    else
            //    {
            //        var text = webRequest.downloadHandler.text;

            //        int left = text.IndexOf("{");
            //        int right = text.LastIndexOf("}");
            //        if (left == -1 || right == -1)
            //        {
            //            sp.ip = PlayerPrefs.GetString($"ClientIP_{rid}", "known");
            //            //sp.area = PlayerPrefs.GetString($"ClientAREA_{rid}", "known");
            //        }
            //        else
            //        {
            //            text = text.Substring(left, right - left + 1);
            //            JsonData jsonData = JsonMapper.ToObject(text);
            //            sp.ip = jsonData["cip"].ToString();
            //            sp.area = $"{sp.area}:{jsonData["cname"].ToString()}";
            //        }
            //    }
            //    AppFacade.GetInstance().SendSproto(sp);
            //    CoreUtils.logService.Debug($"version:{sp.version}\nlanguage:{sp.language}\nphone:{sp.phone}\narea:{sp.area}\nplatform:{sp.platform}\nclient ip:{ sp.ip}", Color.green);
            //    SetCrrRoleID(rid);
            //};

            SetCrrRoleID(rid);
            AppFacade.GetInstance().SendSproto(sp);
            CoreUtils.logService.Debug($"version:{sp.version}\nlanguage:{sp.language}\nphone:{sp.phone}\narea:{sp.area}\nplatform:{sp.platform}\nclient ip:{ sp.ip}\ngameId:{ sp.gameId} ", Color.green);
            // 每次登陆角色都会发
            SendClientDeviceInfoMedia.ReportData();

        }

        public void SetCrrRoleID(long rid)
        {
            m_loginedRoleID = rid;
        }
        long m_gameNode = 1;
        public void SetGameNode(long gameNode)
        {
            m_gameNode = gameNode;
        }
        public long GetGameNode()//服务器id
        {
            return m_gameNode;
        }


        public void UpdateRoleInfo(RoleInfo serverData)
        {
            RoleInfoEntity et = null;

            if (m_roleInfo.ContainsKey(serverData.rid))
            {
                et = m_roleInfo[serverData.rid];
            }
            else
            {
                et = new RoleInfoEntity();
                m_roleInfo.Add(serverData.rid,et);
                long country = serverData.HasCountry ? (int)serverData.country : 101; 
                m_country = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)country);
                var sdk = IGGSDK.shareInstance();
                sdk.setCharID(serverData.rid.ToString());
                sdk.setServerID(serverData.rid / 10000000);
                SetGameNode(serverData.rid / 10000000);
                //CoreUtils.adService.SetCharacterID(serverData.rid.ToString());
                AppFacade.GetInstance().SendNotification(CmdConstant.UpdatePlayerCountry, country);
            }


            

            if (m_loginedRoleID == serverData.rid)
            {
                if (!m_isFirstGetRoleInfo)//更新角色信息
                {
                    GetBeforeRoleAttr(ref m_before);
                }
                //Debug.LogError("rid:"+ serverData.rid);
                HashSet<string> chanageArr = RoleInfoEntity.updateEntity(et, serverData,true);
                if (!m_isFirstGetRoleInfo)//更新角色信息
                {
                    if (chanageArr.Contains("level"))
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateMainRoleLevel);
                        if (m_before != null)
                        {
                            if (et.level > m_before.level)
                            {
                                if (m_cityBuildProxy.NewAgeByLevel(et.level))
                                {
                                    AppFacade.GetInstance().SendNotification(CmdConstant.CityAgeChange);
                                }
                            }
                        }
                    }
                    if (chanageArr.Contains("denar"))
                    {
                        if (m_before != null)
                        {
                            var value = et.denar - m_before.denar;
                            if (value < 0)
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.spent_credits, Mathf.Abs(value).ToString()));
                            }
                        }
                    }
                    if (chanageArr.Contains("actionForce"))
                    {
                        if (m_before != null)
                        {
                            var value = et.actionForce - m_before.actionForce;
                            if (value < 0)
                            {
                                GlobalEffectMediator globalEffectMediator = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                               // globalEffectMediator.FlyActionForceEffect((long)Mathf.Abs(value));
                                globalEffectMediator.FlyActionForceEffect((long)(value));
                            }
                        }
                    }
                    if (chanageArr.Contains("food"))
                    {
                        if (m_before != null)
                        {
                         //   Debug.LogErrorFormat("food,boefore = {0},,,,,cur = {1},,,, dif = {2}", m_before.food, et.food, et.food - m_before.food);
                        }
                    }
                    if (chanageArr.Contains("guildId"))
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.GuildIdChange);
                    }
                }

                if (m_isFirstGetRoleInfo)//首次更新角色信息
                {
                    m_isFirstGetRoleInfo = false;

                    if (et.pos == null)
                    {
                        Debug.LogError("服务器首次下发roleInfo pos属性为空");
                        ClientUtils.Print(et);
                    }

                    //处理网络重连情况 
                    var guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
                    if (guideProxy.IsChangeCityPos())
                    {
                        PosInfo pos = new PosInfo();
                        pos.x = et.pos.x;
                        pos.y = et.pos.y;
                        guideProxy.SetCityPos(pos);

                        Debug.LogFormat("变更城市坐标 X:{0} Y:{1}", et.pos.x, et.pos.y);
                        Vector2 centerPos = guideProxy.GetGuideCityPos();
                        ChangeCityPos((long)centerPos.x, (long)centerPos.y);
                    }

                    CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                    currencyProxy.UpdateCurrency();
                    EmailProxy emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                    emailProxy.OnLoginToGame();
                    ServerTimeModule.Instance.UpdateServerTime(serverData.serverTime, 0);
                    //更新战斗力
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdatePlayerPower);
                    AppFacade.GetInstance().SendNotification(CmdConstant.GameTimeInit);
                    currentLevel = et.level;
                    m_cityBuildProxy.CreteCityData();
                    //if (chanageArr.Contains("noviceGuideStep"))
                    //{
                    //    Debug.LogError("noviceGuideStep:"+serverData.noviceGuideStep);
                    //}
                }

                if(chanageArr.Contains("food") || chanageArr.Contains("wood") || chanageArr.Contains("stone") || chanageArr.Contains("gold")|| chanageArr.Contains("denar"))
                {
                    CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                    currencyProxy.UpdateCurrency();
                   // if(serverData.denar<)
                }
                if (chanageArr.Contains("guildId"))
                {
                    if (serverData.guildId != 0)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.join_group));
                    }
                }
                if (chanageArr.Contains("country"))
                {
                    TaskProxy taskProxy = AppFacade.GetInstance().RetrieveProxy(TaskProxy.ProxyNAME) as TaskProxy;
                    taskProxy.InitData();
                }
                if (chanageArr.Contains("chapterId"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ChapterId);
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateTaskStatistics);
                    if (serverData.chapterId == 2)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Stage1));
                    }
                    else if (serverData.chapterId == 3)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Stage2));
                    }
                    else if (serverData.chapterId == 4)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Stage3));
                    }
                    else if (serverData.chapterId == 5)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.Stage4));
                    }

                }
                if (chanageArr.Contains("finishSideTasks"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.FinishSideTasks, serverData.finishSideTasks);
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateTaskStatistics);
                }

                if (chanageArr.Contains("chapterTasks"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ChapterTasks, serverData.chapterTasks);
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateTaskStatistics);
                }
                if (chanageArr.Contains("mainLineTaskId"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MainLineTaskId);
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateTaskStatistics);
                }
                if ( chanageArr.Contains("taskStatisticsSum") )
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.TaskStatisticsSum, serverData.taskStatisticsSum);
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateTaskStatistics);
                }
                if (chanageArr.Contains("reinforces"))
                {
                    CurrentRoleInfo.reinforces = serverData.reinforces;
                    AppFacade.GetInstance().SendNotification(CmdConstant.ReinforcesChange);
                }
                if (chanageArr.Contains("activePoint"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ActivePointChange);
                    if (serverData.activePoint >= 100)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking,new EventTrackingData(EnumEventTracking.Activity, serverData.activePoint.ToString()));
                    }
                }
                if (chanageArr.Contains("level"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.LevelChange);
                    if (serverData.level == 5)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.reach_castlelv5, serverData.level.ToString()));
                    } else if (serverData.level == 10)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.reach_castlelv10, serverData.level.ToString()));

                    }
                    else if (serverData.level == 13)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.reach_castlelv13, serverData.level.ToString()));

                    }
                    else if (serverData.level == 15)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.reach_castlelv15, serverData.level.ToString()));

                    }
                    else if (serverData.level == 19)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.reach_castlelv19, serverData.level.ToString()));

                    }
                    else if (serverData.level == 22)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.reach_castlelv22, serverData.level.ToString()));
                    }
                }

                if (chanageArr.Contains("activePointRewards"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ActivePointRewardsChange);
                }
                if (chanageArr.Contains("villageCaves"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.VillageCavesChange);
                }
                if (chanageArr.Contains("buildQueue"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.buildQueueChange,serverData.buildQueue);
                }
                if (chanageArr.Contains("armyQueue"))
                {
                    foreach (var data in serverData.armyQueue)
                    {
                        TrainQueueMap[data.Value.buildingIndex] = data.Value;
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.armyQueueChange, serverData);
                }
                if (chanageArr.Contains("technologyQueue"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.technologyQueueChange,serverData.technologyQueue);
                }
                 if (chanageArr.Contains("cityBuff"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.CityBuffChange);
                    //foreach (var temp in serverData.cityBuff)
                    //{
                    //    Debug.LogErrorFormat(" {0}   {1}    {2}", temp.Key, temp.Value.id, temp.Value.expiredTime);
                    //}
                }
                if (chanageArr.Contains("expeditionInfo"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionInfoChange);
                }
                if(chanageArr.Contains("expeditionCoin"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionCoinChange);
                }
                if (chanageArr.Contains("seriousInjured"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.SeriousInjuredChange);
                }
                if (chanageArr.Contains("treatmentQueue"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.treatmentChange, serverData);
                }
                if (chanageArr.Contains("soldiers"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.InSoldierInfoChange, serverData);
                }
                
                if (chanageArr.Contains("technologies"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.technologyChange,serverData.technologies);
                }

                if (chanageArr.Contains("mainHeroId"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MainHeroIdChange, serverData.mainHeroId);
                }
                if (chanageArr.Contains("guardTowerHp"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.GuardTowerHpChange, serverData.guardTowerHp);
                }
                
                if (chanageArr.Contains("deputyHeroId"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.DeputyHeroIdChange, serverData.deputyHeroId);
                }
                if (chanageArr.Contains("finishSideTasks"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.FinishSideTasks, serverData.finishSideTasks);
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateTaskStatistics);
                }

                if (chanageArr.Contains("emailVersion"))
                {
                    EmailProxy emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                    emailProxy.UpdateEmailVersion();
                }

                if (chanageArr.Contains("armies"))
                {
                   // AppFacade.GetInstance().SendNotification(CmdConstant.TroopUpdateEvent);
                }

                if (chanageArr.Contains("combatPower"))
                {
                    if (serverData.HasCombatPowerType && serverData.combatPowerType == 1)
                    {
                        et.combatPowerType = 0;
                    }
                    else
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdatePlayerPower);
                    }
                }
                if (chanageArr.Contains("historyPower"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdatePlayerHistoryPower);
                }
                if (chanageArr.Contains("denseFogOpenFlag"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.denseFogOpenFlag);
                }
                if (chanageArr.Contains("actionForce"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdatePlayerActionPower);
                }
                if (chanageArr.Contains("isChangeAge"))
                {
                    m_cityBuildProxy.AgeChange = serverData.isChangeAge;
                }
                if (chanageArr.Contains("pos"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ChangeRolePos);
                }
                if (chanageArr.Contains("silverFreeCount") || chanageArr.Contains("openNextSilverTime"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.TavernBoxInfoChange, 1);
                }
                if (chanageArr.Contains("goldFreeCount") || chanageArr.Contains("addGoldFreeAddTime"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.TavernBoxInfoChange, 2);
                }


                if (chanageArr.Contains("guildId") && et.guildId == 0)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.AllianceEixt,et.rid);
                    AppFacade.GetInstance().SendNotification(CmdConstant.AllianceEixtEx);

                    (AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy).CleanAll();
                    (AppFacade.GetInstance().RetrieveProxy(AllianceResarchProxy.ProxyNAME) as AllianceResarchProxy).Clear();
                    (AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy).ClearGuildMapMarkerInfoDic();

                    AppFacade.GetInstance().SendNotification(CmdConstant.GuildMapMarkerInfoCleared);
                }
  
                if (chanageArr.Contains("activity"))
                {
                    m_activityProxy.UpdateSchedule(serverData.activity);
                }

                if (chanageArr.Contains("activityTimeInfo"))
                {
                    m_activityProxy.UpdateActivityTime();
                }

                if (chanageArr.Contains("mysteryStore"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnMysteryStoreRefresh);
                }
                if (chanageArr.Contains("growthFund"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.GrowthFundChange);
                }
                if (chanageArr.Contains("rechargeFirst"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnServerCallbackChangedRecharge);
                }
                if (chanageArr.Contains("recharge"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnServerCallbackChangedRecharge);
                }
                if (chanageArr.Contains("riseRoadPackage")||chanageArr.Contains("riseRoad"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnServerCallbackChangedRiseRoad);
                }
                if (chanageArr.Contains("vip"))
                {
                    VipLevel = VipPointToLevel(CurrentRoleInfo.vip);
                    AppFacade.GetInstance().SendNotification(CmdConstant.VipPointChange);
                }
                if (chanageArr.Contains("continuousLoginDay"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.ContinueLoginDayChange);
                }

                if (chanageArr.Contains("vipExpFlag"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.VipDailyPointFlagChange);
                }
                if (chanageArr.Contains("vipFreeBox"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.VipFreeBoxFlagChange);
                }

                if (chanageArr.Contains("vipStore"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.VipStoreInfoChange,serverData.vipStore);
                }

                if (chanageArr.Contains("vipSpecialBox"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.VipSpecialBoxChange);

                }
                
                if (chanageArr.Contains("supply"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateSupplyInfo);
                    AppFacade.GetInstance().SendNotification(Recharge_AwardRechargeSupply.TagName);
                }
                
                if (chanageArr.Contains("rechargeSale"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateSuperGiftInfo);
                }
                
                if (chanageArr.Contains("freeDaily"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateFreeDailyBoxInfo);
                }
                
                if (chanageArr.Contains("dailyPackage"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateDailySpecialInfo);
                }
                
                if (chanageArr.Contains("limitTimePackage"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.LimitTimePackageChange,serverData.limitTimePackage);
                }

                if (chanageArr.Contains("pushSetting"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.NoticeSettingChange,serverData.pushSetting);

                }
                //if (chanageArr.Contains("villageCaves"))
                //{
                //    foreach (var villageCaves in serverData.villageCaves.Values)
                //    {
                //        for (int i = 1; i <= 64; i++)
                //        {
                //            Debug.LogErrorFormat("index = {0},{1} ,pointid = {2} value = {3},state = {4},", villageCaves.index, i, ((villageCaves.index - 1) * 64 + i-1), Convert.ToString(villageCaves.rule, 2), (villageCaves.rule & (long)Math.Pow(2, i -1)));
                //        }
                //    }
                //}

                if (chanageArr.Contains("materialQueue"))
                {    
                    AppFacade.GetInstance().SendNotification(CmdConstant.EquipMaterialProductionInfoChange);
                }

                if (chanageArr.Contains("lastGuildDonateTime"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.AllianceStudyDonateRedCount);
                }
                if(chanageArr.Contains("expeditionInfo"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.PlayerExpeditionInfoChange);
                }
                if (chanageArr.Contains("denseFogOpenFlag") && serverData.HasDenseFogOpenFlag && serverData.denseFogOpenFlag)
                {
                    if (!m_isFirstGetRoleInfo)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.UnlockAllFog);
                    }
                }

                if (chanageArr.Contains("headId") || chanageArr.Contains("headFrameID")|| chanageArr.Contains("guildId")
                || chanageArr.Contains("guildName") || chanageArr.Contains("name"))
                {
                    m_chatProxy.UpdateContactInfo(CurrentRoleInfo);
                }

                if (chanageArr.Contains("markers"))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.PersonMapMarkerInfoChanged, serverData.markers);
                }

                if (chanageArr.Contains("activityActivePoint"))
                {

                    AppFacade.GetInstance().SendNotification(CmdConstant.ActivityActivePointUpdate);
                }
            }
        }

        //获取市政大厅等级
        public Int64 GetTownHall()
        {
            return m_roleInfo[m_loginedRoleID].level;
        }

        //获取国家文明
        public Int64 GetCivilization()
        {
            return m_roleInfo[m_loginedRoleID].country;
        }

        //获取资源数量
        public Int64 GetResNumByType(int type)
        {
            if (type == (int)EnumCurrencyType.food)
            {
                return m_roleInfo[m_loginedRoleID].food;
            }
            else if (type == (int)EnumCurrencyType.wood)
            {
                return m_roleInfo[m_loginedRoleID].wood;
            }
            else if (type == (int)EnumCurrencyType.stone)
            {
                return m_roleInfo[m_loginedRoleID].stone;
            }
            else if (type == (int)EnumCurrencyType.gold)
            {
                return m_roleInfo[m_loginedRoleID].gold;
            }
            else if (type == (int)EnumCurrencyType.denar)
            {
                return m_roleInfo[m_loginedRoleID].denar;
            }
            return 0;
        }

        //根据索引获取训练数据
        public QueueInfo GetTrainInfo(Int64 buildingIndex)
        {
            if (TrainQueueMap.ContainsKey(buildingIndex))
            {
                return TrainQueueMap[buildingIndex];
            }
            return null;
        }

        //获取训练信息
        public Dictionary<Int64, QueueInfo> GetTrainQueue()
        {
            return m_roleInfo[m_loginedRoleID].armyQueue;
        }

        //获取在家部队信息
        public Dictionary<Int64, SoldierInfo> GetInArmyInfo()
        {
            return m_roleInfo[m_loginedRoleID].soldiers;
        }

        //获取受伤部队
        public Dictionary<Int64, SoldierInfo> GetWoundedInfo()
        {
            return m_roleInfo[m_loginedRoleID].seriousInjured;
        }

        //获取治疗队列
        public QueueInfo GetTreatmentQueue()
        {
            return m_roleInfo[m_loginedRoleID].treatmentQueue;
        }

        public Dictionary<Int64, QueueInfo> GetBuildQueue()
        {
            return m_roleInfo[m_loginedRoleID].buildQueue;
        }

        //有神秘商店---建了就有--不一定开启
        public bool HasMysteryStoreInfo()
        {
            if (CurrentRoleInfo != null && CurrentRoleInfo.mysteryStore != null)
            {
                return true;
            }

            return false;
        }

        //恢复城市坐标
        public void RecoverCityPos(PosInfo pos)
        {
            CurrentRoleInfo.pos = pos;
        }

        public void ChangeCityPos(long x, long y)
        {
            CurrentRoleInfo.pos.x = x;
            CurrentRoleInfo.pos.y = y;
        }

#region 科技相关

        /// <summary>
        /// 获取科技
        /// </summary>
        /// <returns></returns>
        public Dictionary<Int64, TechnologyInfo> GetTechnologies()
        {
            return m_roleInfo[m_loginedRoleID].technologies;
        }

        /// <summary>
        /// 获取科技队列
        /// </summary>
        /// <returns></returns>
        public QueueInfo GetTechnologieQueue()
        {
            return m_roleInfo[m_loginedRoleID].technologyQueue;
        }

        //科技是否解锁
        public bool IsTechnologyUnlock(Int64 technologyId)
        {
            StudyDefine define = CoreUtils.dataService.QueryRecord<StudyDefine>((int)technologyId);
            if(define == null)
            {
                return false;
            }
            if (m_roleInfo[m_loginedRoleID].technologies != null && m_roleInfo[m_loginedRoleID].technologies.ContainsKey(define.studyType))
            {
                if (m_roleInfo[m_loginedRoleID].technologies[define.studyType].level >= define.studyLv)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsTechnologyUnlockByType(int studyType)
        {
            if (m_roleInfo[m_loginedRoleID].technologies != null && m_roleInfo[m_loginedRoleID].technologies.ContainsKey(studyType))
            {
                if (m_roleInfo[m_loginedRoleID].technologies[studyType].level >= 0)
                {
                    return true;
                }
            }
            return false;
        }

#endregion

        #region 战力接口
        //总战力
        public long Power()
        {
            return BuildPower()+TechnologyPower()+ArmyPower()+HeroPower();
        }
        //建筑战力
        public long BuildPower()
        {
            if (m_cityBuildProxy==null)
            {
                m_cityBuildProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            }
            return m_cityBuildProxy.BuildPower();
        }
        //科技战力
        public long TechnologyPower()
        {
            if (m_researchProxy==null)
            {
                m_researchProxy= AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;
            }
            return m_researchProxy.GetTechnologyPower();
        }
        //军队战力
        public long ArmyPower()
        {
            if (m_troopProxy==null)
            {
                m_troopProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            }

            return m_troopProxy.GetTroopsTotalPower();
        }
        //英雄战力
        public long HeroPower()
        {
            if (m_heroProxy==null)
            {
                m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            }
            return m_heroProxy.GetHeroTotalPower();
        }
        //历史最高战力
        public long HistoryHightPower()
        {
            return this.CurrentRoleInfo.historyPower;
        }

        private long GetSiatistis(long vv, RoleInfoEntity roleInfoEntity = null)
        {
            long v = 0;
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            RoleStatistics data;
            if (roleInfoEntity.roleStatistics.TryGetValue(vv,out data))
            {
                v = data.num;
            }
            return v;
        }

        //战斗胜利次数
        public long WinCount(RoleInfoEntity roleInfoEntity = null)
        {
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            return GetSiatistis(1, roleInfoEntity);
        }
        //战斗失败次数
        public long LoseCount(RoleInfoEntity roleInfoEntity = null)
        {
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            return GetSiatistis(2, roleInfoEntity);
        }
        
        //死亡次数
        public long DeadCount(RoleInfoEntity roleInfoEntity = null)
        {
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            return GetSiatistis(3, roleInfoEntity);
        }
        
        //占察失败次数
        public long SkyCount(RoleInfoEntity roleInfoEntity = null)
        {
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            return GetSiatistis(4, roleInfoEntity);
        }
        //资源采集次数
        public long ResourceCollection(RoleInfoEntity roleInfoEntity = null)
        {
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            return GetSiatistis(5, roleInfoEntity);
        }
        
        //资源援助次数
        public long HelpCount(RoleInfoEntity roleInfoEntity = null)
        {
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            return GetSiatistis(6, roleInfoEntity);
        }
        
        //击杀次数
        public long killCount(RoleInfoEntity roleInfoEntity = null)
        {
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            var v = roleInfoEntity.killCount;
            long data = 0;
            if (v!=null)
            {
                foreach (var soldierKillInfo in v)
                {
                    if (soldierKillInfo.Key>=1 && soldierKillInfo.Key<=5)
                    {
                        data += soldierKillInfo.Value.count;
                    }
                }
            }
            return data;
        }

        public long KillCountOther(Dictionary<long,KillCount> v)
        {
            long data = 0;
            if (v!=null)
            {
                foreach (var soldierKillInfo in v)
                {
                    data += soldierKillInfo.Value.count;
                }
            }

            return data;
        }

        //联盟帮助次数
        public long GuildHelpCount(RoleInfoEntity roleInfoEntity = null)
        {
            if (roleInfoEntity == null)
            {
                roleInfoEntity = this.CurrentRoleInfo;
            }
            return GetSiatistis(7, roleInfoEntity);
        }

#endregion

#region ID异或
        const long key = 1106859L;
        public long GetEncryptId(long id = 0)
        {
            long encryptId = 0;
            if (id == 0)
            {
                id = CurrentRoleInfo.rid;
            }
            encryptId = id ^ key;

            long serverId = id / 10000000;
            encryptId = serverId * 10000000 + encryptId;
            //encryptId = GetServerID() * 10000000 + encryptId;
            GetDecodeId(encryptId);
            return encryptId;
        }

        public long GetDecodeId(long id)
        {
            long decodeId = 0;
            long serverId = id / 10000000;
            decodeId = id - serverId * 10000000;
            decodeId = decodeId ^ key;
            //  Log.GGXFormat("{0}", decodeId);
            return decodeId;
        }

        #endregion

        #region 活动

        public Dictionary<Int64, ActivityTimeInfo> GetActivityTimeInfo()
        {
            return m_roleInfo[m_loginedRoleID].activityTimeInfo;
        }

        public Dictionary<Int64, Activity> GetActivitySchedule()
        {
            return m_roleInfo[m_loginedRoleID].activity;
        }

        #endregion

        #region VIP
        public Int64 GetVipLevel()
        {
            return VipLevel;
        }

        public VipDefine GetCurVipInfo()
        {
            var vipDefineLst = CoreUtils.dataService.QueryRecords<Data.VipDefine>();
            var vipDefine = vipDefineLst.Find(x => x.point > CurrentRoleInfo.vip);
            if (vipDefine == null && vipDefineLst.Count > 0)
            {
                vipDefine = vipDefineLst[vipDefineLst.Count-1];
            }

            return vipDefine;
        }

        public VipStore GetVIPStoreInfo(int id)
        {
            foreach (var vipStore in CurrentRoleInfo.vipStore)
            {
                if (id == vipStore.Key)
                {
                    return vipStore.Value;
                }
            }
            
            return null;
        }

        //最高等级vip
        public int GetMaxVipLevel()
        {
            if (m_maxVipLevel > -1)
            {
                return m_maxVipLevel;
            }
            var vipConfigs = CoreUtils.dataService.QueryRecords<Data.VipDefine>();
            if (vipConfigs.Count > 0)
            {
                m_maxVipLevel = vipConfigs[vipConfigs.Count - 1].level;
            }
            return m_maxVipLevel;
        }

        //获取当日VIP点数配置
        public VipDayPointDefine GetVipDayPointInfo(long continuousLoginDay = -1)
        {
            if (continuousLoginDay < 0)
            {
                continuousLoginDay = CurrentRoleInfo.continuousLoginDay;
            }
            var vipDayPointConfigs = CoreUtils.dataService.QueryRecords<Data.VipDayPointDefine>();
            
            foreach (var config in vipDayPointConfigs)
            {
                if (config.day == continuousLoginDay)
                {
                    return config;
                }
            }
            
            if ( vipDayPointConfigs.Count >= 1)
            {
                return vipDayPointConfigs[vipDayPointConfigs.Count - 1];
            }
            
            CoreUtils.logService.Error("Vip=====配置表s_VipDayPoint未配置数据");
            return null;
        }
        
        private long VipPointToLevel(long vipPoint)
        {
            var vipConfigs = CoreUtils.dataService.QueryRecords<Data.VipDefine>();

            for (int i = 0; i < vipConfigs.Count; i++)
            {
                if (vipConfigs[i].point > vipPoint)
                {
                    return vipConfigs[i].level;
                }
            }

            if ( vipConfigs.Count > 0)
            {
                return vipConfigs[vipConfigs.Count-1].level;
            }

            return 0;
        }

        public bool IsShowVipStorePop()
        {
            var lastTime = PlayerPrefs.GetInt("ShowVipStoreBubble" + Rid.ToString(), 0);
            if (lastTime != 0)
            {
                var lastData = ServerTimeModule.Instance.ConverToServerDateTime(lastTime);
                var curData = ServerTimeModule.Instance.GetCurrServerDateTime();
                if (lastData.Year == curData.Year && lastData.Month == curData.Month && lastData.Day == curData.Day)
                {
                    return false;
                }
            }

            var vipStoreConfigs = CoreUtils.dataService.QueryRecords<VipStoreDefine>();
            foreach (var vipStoreConfig in vipStoreConfigs)
            {
                if (vipStoreConfig.vipLevel <= VipLevel)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetShowVipStorePop()
        {
            PlayerPrefs.SetInt("ShowVipStoreBubble"+Rid.ToString(),(int)ServerTimeModule.Instance.GetServerTime());
            AppFacade.GetInstance().SendNotification(CmdConstant.VipStoreShowBubbleChange);
        }

        #endregion

        #region 装备

        public bool HasCompleteProduceItems()
        {
            if (CurrentRoleInfo.materialQueue != null && CurrentRoleInfo.materialQueue.completeItems != null &&
                CurrentRoleInfo.materialQueue.completeItems.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool IsProducingMaterial()
        {
            if (CurrentRoleInfo.materialQueue != null && CurrentRoleInfo.materialQueue.produceItems != null &&
                                                          CurrentRoleInfo.materialQueue.produceItems.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool CanProduceItem()
        {
            return CurrentRoleInfo.materialQueue?.produceItems == null || CurrentRoleInfo.materialQueue.produceItems.Count < 5;
        }
        public QueueInfo GetCurProduceItems()
        {
            return CurrentRoleInfo.materialQueue;
        }

        public bool IsMaterialProduceQueueFull()
        {
            return CurrentRoleInfo.materialQueue?.produceItems != null && CurrentRoleInfo.materialQueue.produceItems.Count >= 5;
        }
        
        public void AwardProduceMaterial()
        {
            Build_AwardProduceMaterial.request req = new Build_AwardProduceMaterial.request();
            AppFacade.GetInstance().SendSproto(req);
        }

        #endregion
        private void GetBeforeRoleAttr(ref RoleInfoEntity before)
        {
            before.rid = CurrentRoleInfo.rid;
            before.level = CurrentRoleInfo.level;
            before.denar = CurrentRoleInfo.denar;
            before.food = CurrentRoleInfo.food;
            before.wood = CurrentRoleInfo.wood;
            before.stone = CurrentRoleInfo.stone;
            before.actionForce = CurrentRoleInfo.actionForce;
        }
    }
}
